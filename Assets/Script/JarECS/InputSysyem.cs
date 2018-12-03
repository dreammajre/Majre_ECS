using UnityEngine;
using UnityEditor;
using Unity.Rendering;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class InputSysyem : ComponentSystem
{
    public struct InputGroup {
        public readonly int Length;
        public ComponentDataArray<PlayerComponent> PlayerCom;
        public ComponentDataArray<InputComponent> InputCom;
        public ComponentDataArray<VelocityComponent> VelocityCom;
    }

    [Inject] InputGroup inputGroup;

    protected override void OnUpdate()
    {
        for (int i = 0; i < inputGroup.Length; i++)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            float3 normalized = new float3();
            if (x != 0 || y != 0)
                normalized = math.normalize(new float3(x,0,y));
            inputGroup.VelocityCom[i] = new VelocityComponent { moveVelicity = normalized };    
        }
    }
}