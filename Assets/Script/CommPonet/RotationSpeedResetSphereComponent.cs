using System;
using Unity.Entities;

[Serializable]
public struct RotationSpeedResetSphere : IComponentData
{
    public float speed;
}

[UnityEngine.DisallowMultipleComponent]
public class RotationSpeedResetSphereComponent : ComponentDataWrapper<RotationSpeedResetSphere> { }