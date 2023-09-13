using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckCooldown : CharacterCondition
{
    private float passedTime;
    public float coolDown;

    public override void OnStart()
    {
        
    }

    public override TaskStatus OnUpdate()
    {
        passedTime += Time.deltaTime;

        if (passedTime > coolDown)
        {
            passedTime = 0;
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}