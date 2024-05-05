using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tents : MonoBehaviour
{
    [SerializeField] private GameObject _npcPrefab;
    [SerializeField] private GameObject _itemPrefab;
    private List<TentNpc> _tents;

    // Start is called before the first frame update
    void Start()
    {
        var gos = GameObject.FindGameObjectsWithTag("Tent");
        _tents = new List<TentNpc>();

        foreach (var t in gos)
        {
            _tents.Add(t.GetComponent<TentNpc>());
        }

        foreach (var t in _tents)
        {
            if (Random.Range(0, 101) < 50) t.SpawnNpc(_npcPrefab);
            else t.SpawnItem(_itemPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
