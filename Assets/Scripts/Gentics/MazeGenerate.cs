using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GenerateMaze : MonoBehaviour
{
    [SerializeField][Range(3, 100)] private int _size = 5;
    [SerializeField][Range(5, 10)] private int _wallSize = 5;
    [SerializeField] private GameObject _groundPrefab;
    [SerializeField] private GameObject _roofPrefab;
    [SerializeField] private GameObject _verticalWallPrefab, _horizontalWallPrefab;
    [SerializeField] private GameObject _npc, _ammo;
    [SerializeField][Range(1, 100)] private int _ammoAmount = 10;
    [SerializeField] private bool _showRoof = true;
    private GameObject _wallParent, _floorParent, _roofParent;
    private const int N = 1, S = 2, E = 4, W = 8;
    private int[,] _grid;
    private GameObject[,] _verticalWalls, _horizontalWalls, _floor, _roof;
    private List<GameObject> _ammoObjects;
    // Start is called before the first frame update
    void Start()
    {
        Innit();
    }

    private void Innit()
    {
        _verticalWallPrefab.transform.localScale = new Vector3(0.1f, 4f, _wallSize);
        _horizontalWallPrefab.transform.localScale = new Vector3(_wallSize, 4f, 0.1f);

        _groundPrefab.transform.localScale = new Vector3(_wallSize, 0.1f, _wallSize);
        _roofPrefab.transform.localScale = new Vector3(_wallSize, 0.1f, _wallSize);

        _verticalWalls = new GameObject[_size + 1, _size + 1];
        _horizontalWalls = new GameObject[_size + 1, _size + 1];
        _floor = new GameObject[_size + 1, _size + 1];
        _roof = new GameObject[_size + 1, _size + 1];

        _wallParent = new GameObject("Walls");
        _wallParent.transform.parent = transform;
        _floorParent = new GameObject("floor");
        _floorParent.transform.parent = transform;
        _roofParent = new GameObject("roof");
        _roofParent.transform.parent = transform;

        GenerateGrid();
        DrawGrid();
        CarveGrid();
        DrawFloorAndRoof();

        _ammoObjects = new List<GameObject>();

        for (int i = 0; i < _ammoAmount; i++)
        {
            _ammoObjects.Add(SpawnRandomLocation(_ammo));
        }

        _roofParent.SetActive(_showRoof);
    }

    private GameObject SpawnRandomLocation(GameObject _prefab, int offset = 2)
    {
        var go = Instantiate(_prefab);

        int x = Random.Range(0, _size);
        int z = Random.Range(0, _size);

        var tile = _floor[x, z].transform.position;

        go.transform.position = transform.position +  new Vector3(tile.x, tile.y + offset, tile.z);

        return go;
    }

    public void Reset()
    {
        foreach (var go in _ammoObjects)
        {
            go.SetActive(true);
        }
    }

    public Vector3 GetTileLocation(Vector2 tile)
    {
        return _floor[(int)tile.x, (int)tile.y].transform.position;
    }

    private void CarveGrid()
    {
        for (int row = 0; row < _size; row++)
        {
            for (int cell = 0; cell < _size; cell++)
            {
                if (_grid[cell, row] == N) _horizontalWalls[cell, row + 1].SetActive(false);
                if (_grid[cell, row] == E) _verticalWalls[cell + 1, row].SetActive(false);
            }
        }
    }

    private void DrawGrid()
    {
        for (int i = 0; i <= _size; i++)
        {
            for (int j = 0; j <= _size; j++)
            {
                if (i < _size)
                {
                    float vWallSize = _verticalWallPrefab.transform.localScale.z;
                    float vWallheight = _verticalWallPrefab.transform.localScale.y;

                    float xOffset = -(_size * vWallSize) / 2;
                    float zOffset = -(_size * vWallSize) / 2;

                    var go = Instantiate(_verticalWallPrefab, transform.position + new Vector3(-vWallSize / 2 + j * _wallSize + xOffset, vWallheight / 2, i * vWallSize + zOffset), Quaternion.identity);
                    go.SetActive(true);
                    go.name = "v" + i + j;
                    go.tag = "wall";
                    go.transform.parent = _wallParent.transform;
                    _verticalWalls[j, i] = go;
                }

                if (j < _size)
                {
                    float hWallSize = _horizontalWallPrefab.transform.localScale.x;
                    float hWallheight = _verticalWallPrefab.transform.localScale.y;

                    float xOffset = -(_size * hWallSize) / 2;
                    float zOffset = -(_size * hWallSize) / 2;

                    var go = Instantiate(_horizontalWallPrefab, transform.position + new Vector3(j * hWallSize + xOffset, hWallheight / 2, -(hWallSize / 2) + i * hWallSize + zOffset), Quaternion.identity);
                    go.SetActive(true);
                    go.name = "h" + i + j;
                    go.tag = "wall";
                    go.transform.parent = _wallParent.transform;
                    _horizontalWalls[j, i] = go;
                }
            }
        }
    }

    private void GenerateGrid()
    {
        _grid = new int[_size, _size];
        float randomNumber;
        int carvingDirection;

        for (int row = 0; row < _size; row++)
        {
            for (int cell = 0; cell < _size; cell++)
            {
                randomNumber = Random.Range(0, 100);

                if (randomNumber > 50) carvingDirection = N;
                else carvingDirection = E;
                if (cell == _size - 1)
                {
                    if (row < _size - 1) carvingDirection = N;
                    else carvingDirection = W;
                }
                else if (row == _size - 1)
                {
                    if (cell < _size - 1) carvingDirection = E;
                    else carvingDirection = -1;
                }
                _grid[cell, row] = carvingDirection;
            }
        }
    }

    private void DrawFloorAndRoof()
    {
        for (int x = 0; x < _size; x++)
        {
            float hWallSize = _horizontalWallPrefab.transform.localScale.x;
            float wallHeight = _horizontalWallPrefab.transform.localScale.y;

            float xOffset = -(_size * hWallSize) / 2;
            float zOffset = -(_size * hWallSize) / 2;

            for (int y = 0; y < _size; y++)
            {
                var floor = Instantiate(_groundPrefab, transform.position + new Vector3(x * _wallSize + xOffset, 0, y * _wallSize + zOffset), Quaternion.identity);
                var roof = Instantiate(_roofPrefab, transform.position + new Vector3(x * _wallSize + xOffset, wallHeight, y * _wallSize + zOffset), Quaternion.identity);

                floor.SetActive(true);
                floor.name = "f" + x + y;
                floor.tag = "floor";
                floor.transform.parent = _floorParent.transform;

                roof.SetActive(true);
                roof.name = "r" + x + y;
                roof.tag = "roof";
                roof.transform.parent = _roofParent.transform;

                _floor[x, y] = floor;
                _roof[x, y] = roof;
            }
        }
    }
}
