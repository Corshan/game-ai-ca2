using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaceDanger : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    private List<FlockM2> _flocks;
    // Start is called before the first frame update
    void Start()
    {
        _flocks = new List<FlockM2>();
    }

    // Update is called once per frame
    void Update()
    {
        _flocks = new List<FlockM2>();
        foreach (var f in GameObject.FindGameObjectsWithTag("Flock"))
        {
            _flocks.Add(f.GetComponent<FlockM2>());
        }

        // Debug.Log(_flocks.Count);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hitInfo))
            {
                var go = Instantiate(_prefab, hitInfo.point, _prefab.transform.rotation);
                foreach (var f in _flocks)
                {
                    f.SetFlee(hitInfo.point);
                }

                Destroy(go, 5);
            }
        }
    }
}
