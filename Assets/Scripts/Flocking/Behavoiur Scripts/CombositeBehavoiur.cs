using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Flock/Behaviour/Composite")]
public class CombositeBehavoiur : FlockBehaviour
{
    public FlockBehaviour[] behaviours;
    public float[] weights;
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (behaviours.Length != weights.Length)
        {
            Debug.LogError("Data mismatch " + name, this);
            return Vector2.zero;
        }

        Vector2 move = Vector2.zero;

        for (int i = 0; i < behaviours.Length; i++)
        {
            Vector2 partialMove = behaviours[i].CalculateMove(agent, context, flock) * weights[i];

            if (partialMove != Vector2.zero)
            {
                if (partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                move += partialMove;
            }
        }

        return move;

    }
}
