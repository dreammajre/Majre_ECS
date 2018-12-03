using UnityEngine;
using UnityEditor;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;

public class MoveSystem : ComponentSystem
{

    public struct Group {
        public readonly int Length;
        public ComponentDataArray<VelocityComponent> VelocityCom;
        public ComponentDataArray<Position> Position;
    }
    [Inject] Group group;
    protected override void OnUpdate()
    {
        float deltaTime = Time.deltaTime;
        for (int i = 0; i < group.Length; i++)
        {
            float3 vector = group.VelocityCom[i].moveVelicity;
            float3 pos = group.Position[i].Value;
            pos += deltaTime * vector;
            group.Position[i] = new Position { Value = pos };
        }
    }
}