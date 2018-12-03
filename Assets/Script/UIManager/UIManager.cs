using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;
using Unity.Collections;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour {

    public static EntityArchetype entityArchetype;
    public GameObject cube;
    [Header("Mesh Info")]
    public Mesh mesh;
    [Header("Material Info")]
    public Material maTemp;
    [Header("Btn Info")]
    public Button btn;
    public EntityManager entityManager;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]//在场景加载之前运行的方法
    public static void Initialize()
    {
        EntityManager manager = World.Active.GetOrCreateManager<EntityManager>();
        entityArchetype = manager.CreateArchetype(
              typeof(Position)
            );
    }
    void Start()
    {
        entityManager = World.Active.GetOrCreateManager<EntityManager>();       
        btn.onClick.AddListener(ChunkGanerator);
    }
    void ChunkGanerator()
    {
        Entity entitys = entityManager.CreateEntity(entityArchetype);
        entityManager.SetComponentData(entitys, new Position { Value = new int3(UnityEngine.Random.Range(1,100), 0, 0) });
        entityManager.AddSharedComponentData(entitys, new MeshInstanceRenderer
        {
            mesh = mesh,
            material = maTemp
        });
    }
}
  