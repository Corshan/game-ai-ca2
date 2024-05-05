using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentNpc : MonoBehaviour
{
    public GameObject Npc;
    public GameObject Item;
    public GameObject SpawnPoint;
    private int _health = 100;
    public bool IsNpc => Npc != null;
    public bool IsItem => Item != null;
    public bool IsAlive => _health > 0;
    public void ChangeHealth(int health) => _health -= health; 
    private Animator _animator;

    public void SpawnNpc(GameObject npc)
    {
        Npc = Instantiate(npc, SpawnPoint.transform.position, Quaternion.identity);
        _animator = Npc.GetComponent<Animator>();
        Item = null;
    }

    public void SpawnItem(GameObject item)
    {
        Item = Instantiate(item, SpawnPoint.transform.position, Quaternion.identity);
        Npc = null;
    }

    void Update()
    {
        if(!IsAlive) _animator.SetBool("dead", true);
    }
}
