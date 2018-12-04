using UnityEngine;
using UnityEditor;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

/*
 移动系统 ECS 解决方案
     */
//public class MoveSystem : ComponentSystem
//{

//    public struct Group {
//        public readonly int Length;
//        public ComponentDataArray<VelocityComponent> VelocityCom;
//        public ComponentDataArray<Position> Position;
//    }
//    [Inject] Group group;
//    protected override void OnUpdate()
//    {
//        float deltaTime = Time.deltaTime;
//        for (int i = 0; i < group.Length; i++)
//        {
//            float3 vector = group.VelocityCom[i].moveVelicity;
//            float3 pos = group.Position[i].Value;
//            pos += deltaTime * vector;
//            group.Position[i] = new Position { Value = pos };
//        }
//    }
//}

/*
 ECS + Job解决方案 未来Unity的方向
     */
public class MoveSystem : JobComponentSystem {

    [BurstCompile]
    public struct Group : IJobProcessComponentData<VelocityComponent,Position> {
        public float deltaTime;
        public void Execute(ref VelocityComponent velocityComponent,ref Position position) {
            float3 vector = velocityComponent.moveVelicity;
            float3 pos = position.Value;
            pos += deltaTime * vector;
            position = new Position { Value = pos };
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new Group() { deltaTime = Time.deltaTime };
        return job.Schedule(this,inputDeps);
    }
}
