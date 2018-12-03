using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;

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
        entityManager.SetComponentData(player, new Position { Value = new float3(0,0.5f,0) });
        entityManager.AddSharedComponentData(player, new MeshInstanceRenderer {
            mesh = mesh,
            material = material,
            castShadows = UnityEngine.Rendering.ShadowCastingMode.On
        });
        entityManager.AddComponentData(player, new PlayerComponent());
        entityManager.AddComponentData(player, new InputComponent());
        entityManager.AddComponentData(player, new VelocityComponent());
    }
}
