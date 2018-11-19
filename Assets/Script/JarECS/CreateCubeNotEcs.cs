using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;
using Unity.Collections;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;


public class CreateCubeNotEcs : MonoBehaviour {

    /*
     EntityArchetype

     An EntityArchetype是一个独特的数组ComponentType。EntityManager使用EntityArchetypestucts在Chunks中使用相同的组件类型对所有实体进行分组。
         */
    public static EntityArchetype entityArchetype;
    public GameObject cube;
    [Header("Mesh Info")]
    public Mesh mesh;
    [Header("Material Info")]
    public Material material;

    public EntityManager entityManager;
    public Entity entity;

   

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]//在场景加载之前运行的方法
    public static void Initialize() {
        EntityManager manager = World.Active.GetOrCreateManager<EntityManager>();
        entityArchetype = manager.CreateArchetype(
              typeof(Position)
            );
    }
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	void Start () {
      
            entityManager = World.Active.GetOrCreateManager<EntityManager>();
            Entity entity = entityManager.CreateEntity(entityArchetype);
            entityManager.SetComponentData(entity, new Position { Value = new int3(2, 0,0) });
            entityManager.AddSharedComponentData(entity, new MeshInstanceRenderer
            {
                mesh = mesh,
                material = material
            });
          if (cube) {
            NativeArray<Entity> entitiesArray = new NativeArray<Entity> (1000, Allocator.Temp);
            entityManager.Instantiate(cube,entitiesArray);
            entityManager.SetComponentData(entity, new Position { Value = new int3(4, 0, 0) });
            entitiesArray.Dispose();
          }
	}
}
