using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;

[UnityEngine.DisallowMultipleComponent]
public class RotationAccelerationComponent:ComponentDataWrapper<RotationAcceleration> { 

}

[Serializable]
public struct RotationAcceleration:IComponentData {
    public float speed;
}