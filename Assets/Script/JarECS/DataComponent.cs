using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;
public class DataComponent { 
}

public struct PlayerComponent : IComponentData {
   
}

public struct InputComponent : IComponentData {

}
public struct VelocityComponent : IComponentData {
    public float3 moveVelicity;
}

