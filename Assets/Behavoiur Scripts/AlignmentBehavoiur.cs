using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Alignment")]
public class AlignmentBehavoiur : FlockBehaviour
{
     public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (context.Count == 0) return agent.transform.forward;

        Vector2 ailgnmentMove = Vector2.zero;
        foreach (Transform t in context)
        {
            ailgnmentMove += (Vector2) t.transform.forward;
        }
        // ailgnmentMove /= context.Count;

        return ailgnmentMove;
    }
}
