using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;


public class DataComponent { 
}

//标记用户 由于ECS查找的时候会找所有挂载的脚本，给主角添加单独的脚本区分
public struct PlayerComponent : IComponentData {
   
}
//添加接受输入
public struct InputComponent : IComponentData {

}
//记录输入数据
public struct VelocityComponent : IComponentData {
    public float3 moveVelicity;
}

//C# 作业系统
public struct MyJob : IJob
{
    public float a;
    public float b;
    public NativeArray<float> result;
    public void Execute() {    
            result[0] = a + b;        
    }
}

public struct MyJob1 : IJob {
    public NativeArray<float> result;
    public void Execute() {
        result[0] = result[0] + 1;
    }
}

