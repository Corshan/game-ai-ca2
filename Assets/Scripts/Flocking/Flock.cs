using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehaviour behaviour;

    [Range(10, 500)] public int startingCount = 250;
    const float AgentDensity = 0.08f;

    [Range(1f, 100f)] public float driveFactor = 10f;
    [Range(1f, 100f)] public float maxSpeed = 5f;
    [Range(1f, 10f)] public float neighborRadius = 1.5f;
    [Range(0f, 1f)] public float avoidanceradiusMultipler = 0.5f;

    float squareMaxSpeed, squareneighbourRadius, squareAvoidanceRaduis;

    public float SquareAvoidanceRaduis => squareAvoidanceRaduis;

    [Range(1, 10)] public float yOffset = 1;

    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareneighbourRadius = squareneighbourRadius * squareneighbourRadius;
        squareAvoidanceRaduis = squareneighbourRadius * avoidanceradiusMultipler * avoidanceradiusMultipler;

        for (int i = 0; i < startingCount; i++)
        {
            var rand = Random.insideUnitCircle;
            var pos = AgentDensity * startingCount * new Vector3(rand.x, 0, rand.y);
            pos.y = yOffset;

            FlockAgent newAgent = Instantiate(agentPrefab, pos, Quaternion.identity, transform);

            newAgent.name = $"Agent {i}";

            agents.Add(newAgent);
        }
    }

    private void Update()
    {
        foreach (var agent in agents)
        {
            List<Transform> context = GetNearByObjects(agent);


            Vector2 move = behaviour.CalculateMove(agent, context, this);

            move *= driveFactor;

            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    List<Transform> GetNearByObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);

        foreach (Collider c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }

        return context;
    }
}