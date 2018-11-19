using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;

[UnityEngine.DisallowMultipleComponent]//防止相同类型(或子类型)的单行为被添加到GameObject不止一次
public class RotationSpeedComponent:ComponentDataWrapper<RotationSpeed> { 
}
[Serializable]
public struct RotationSpeed : IComponentData {
    public float value;
}
