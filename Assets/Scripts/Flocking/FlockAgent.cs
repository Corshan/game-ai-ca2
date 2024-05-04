using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    Collider _agentCollider;
    public Collider AgentCollider { get { return _agentCollider; } }

    // Start is called before the first frame update
    void Start()
    {
        _agentCollider = GetComponent<Collider>();
    }

    public void Move(Vector2 velocity)
    {
        // transform.forward = velocity;
        transform.position += new Vector3(velocity.x, 0, velocity.y) * Time.deltaTime;
    }
}
