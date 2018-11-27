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
    public Material material0;
    public Material material1;
    public Material material2;
    public Material material3;
    public Material material4;
    public Material material5;
    public Material material6;
    public Material materialNo;
    [Header("ChuckBase")]
    public int ChuckBase = 1;

    public EntityManager entityManager;
    public Entity entity;
    public static Texture2D hightMap;

    Material maTemp;



    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]//在场景加载之前运行的方法
    public static void Initialize() {
        EntityManager manager = World.Active.GetOrCreateManager<EntityManager>();
        entityArchetype = manager.CreateArchetype(
              typeof(Position)
            );      
    }

    private void Awake()
    {

    }

    void Start()
    {
        entityManager = World.Active.GetOrCreateManager<EntityManager>();
        ChunkGanerator(ChuckBase);
    }

    void ChunkGanerator(int amount)
    {
        int hightLevel;
        int totalamount = (amount * amount) * 1500; 
        bool airChecker;
        for (int y = 0; y <  15; y++)
        {
            for (int x = 0; x < 10* amount; x++)
            {
                for (int z = 0; z <10* amount; z++)
                {
                    hightLevel = (int)(hightMap.GetPixel(x,z).r * 100) - y; 
                    airChecker = false;
                    switch (hightLevel)
                    {
                        case 0:
                            maTemp = material0;
                            break;
                        case 1:
                            maTemp = material1;
                            break;
                        case 2:
                            maTemp = material2;
                            break;
                        case 3:
                            maTemp = material3;
                            break;
                        case 4:
                            maTemp = material4;
                            break;
                        case 5:
                            maTemp = material5;
                            break;
                        case 6:
                            maTemp = material6;
                            break;
                        default:
                            maTemp = materialNo;
                            airChecker = true;
                            break;
                    }
                    if (!airChecker)
                    {
                        Entity entitys = entityManager.CreateEntity(entityArchetype);                        
                        entityManager.SetComponentData(entitys, new Position { Value = new int3(x, y, z) });
                        entityManager.AddSharedComponentData(entitys, new MeshInstanceRenderer {
                            mesh = mesh,
                            material = maTemp                            
                        });
                    }
                }
            }
        }
    }

    /* 
     * 两种不同的方式生成Entity
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
      */

    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]

}
