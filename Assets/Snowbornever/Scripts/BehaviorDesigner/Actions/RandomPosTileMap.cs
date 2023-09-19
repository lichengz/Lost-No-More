using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class RandomPosTileMap : CharacterAction
{
    public SharedVector2 randomPos;
    public float range = 5;
    
    void RandomPoint(Vector3 center, float range)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                randomPos.Value = hit.position;
                return;
            }
        }
        randomPos.Value = transform.position;
    }

    public override TaskStatus OnUpdate()
    {
        RandomPoint(transform.position, range);
        return TaskStatus.Success;
    }
}
