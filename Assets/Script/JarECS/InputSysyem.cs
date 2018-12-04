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
 计算输入数据的系统，然后将数据给接受输入的VelocityComponent
     */

//ECS 解决方案
//public class InputSysyem : ComponentSystem
//{
//    public struct InputGroup {
//        public readonly int Length;
//        public ComponentDataArray<PlayerComponent> PlayerCom;
//        public ComponentDataArray<InputComponent> InputCom;
//        public ComponentDataArray<VelocityComponent> VelocityCom;

//    }

//    [Inject] InputGroup inputGroup;

//    protected override void OnUpdate()
//    {        
//        for (int i = 0; i < inputGroup.Length; i++)
//        {
//            float x = Input.GetAxis("Horizontal");
//            float y = Input.GetAxis("Vertical");
//            float3 normalized = new float3();
//            if (x != 0 || y != 0)
//                normalized = math.normalize(new float3(x,0,y));
//            inputGroup.VelocityCom[i] = new VelocityComponent { moveVelicity = normalized };    
//        }
//    }
//}

//ECS + Job 解决方案

public class InputSystem : JobComponentSystem {
    [BurstCompile]
    public struct InputGroup: IJobProcessComponentData<PlayerComponent, InputComponent, VelocityComponent> {
        public float x;
        public float y;
        public void Execute(ref PlayerComponent playerComponent,ref InputComponent inputComponent,ref VelocityComponent velocityComponent) {
         
            float3 normalized = new float3();
            if (x != 0 || y != 0)
                normalized = math.normalize(new float3(x, 0, y));
            velocityComponent = new VelocityComponent { moveVelicity = normalized };
        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new InputGroup {
         x = Input.GetAxis("Horizontal"),
         y = Input.GetAxis("Vertical")
         };
        return job.Schedule(this, inputDeps);
    }

}