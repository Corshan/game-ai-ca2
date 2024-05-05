using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Advoidance")]
public class AvoidanceBehavoiur : FlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (context.Count == 0) return Vector2.zero;

        Vector2 avoidenceMove = Vector2.zero;
        int nAvoid = 0;

        foreach (Transform t in context)
        {
            if (Vector2.SqrMagnitude(t.position - agent.transform.position) < flock.SquareAvoidanceRaduis)
            {
                nAvoid++;
                avoidenceMove += (Vector2)(agent.transform.position - t.position);
            }
        }
        if (nAvoid > 0) avoidenceMove /= nAvoid;

        return avoidenceMove;
    }
}
