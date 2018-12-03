using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine.UI;
using Unity.Jobs;
using Unity.Collections;
/*
 
     */
public class Bootstrap:MonoBehaviour {

    private static EntityManager entityManager;
    private static EntityArchetype entityArchetype;
    [Header("Mesh")]
    public  Mesh mesh;
    [Header("Materials")]
    public  Material material;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init() {
        entityManager = World.Active.GetOrCreateManager<EntityManager>();
        entityArchetype = entityManager.CreateArchetype(typeof(Position));
    }
    public void Start() {
        Entity player = entityManager.CreateEntity(entityArchetype);
        entityManager.AddComponentData(player, new PlayerComponent());
        entityManager.AddComponentData(player, new InputComponent());
        entityManager.AddComponentData(player, new VelocityComponent());
        entityManager.SetComponentData(player, new Position { Value = new float3(0,0.5f,0) });
        entityManager.AddSharedComponentData(player, new MeshInstanceRenderer {
            mesh = mesh,
            material = material,
            castShadows = UnityEngine.Rendering.ShadowCastingMode.On
        });
        //StartCoroutine(Des(player));



        NativeArray<float> result = new NativeArray<float>(1, Allocator.Temp);
        //设置工作数据
        MyJob myJob = new MyJob();
        myJob.a = 10;
        myJob.b = 20;
        myJob.result = result;
        //安排作业
        JobHandle jobHandle = myJob.Schedule();
        MyJob1 myJob1 = new MyJob1();
        myJob1.result = result;
        JobHandle jobHandle1 = myJob1.Schedule(jobHandle);//第二个依赖第一个的数据
        //等待作业完成
        jobHandle1.Complete();
        //NativeArray的所有副本指向同一个内存，我们可以访问NativeArray中的结果
        float results = result[0];
        result.Dispose();
        Debug.Log("获取的结果"+results);
        
    }

    IEnumerator Des(Entity player) {
        yield return new WaitForSeconds(2.0f);
    //    entityManager.DestroyEntity(player);//删除
     //   entityManager.RemoveComponent<InputComponent>(player);//移除脚本
    }


}
