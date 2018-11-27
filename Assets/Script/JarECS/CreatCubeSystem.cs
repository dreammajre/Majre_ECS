using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;

//[UpdateBefore(typeof(Move))]//定义哪一个Sysytm在前面
public class CreatCubeSystem : ComponentSystem
{

    struct CreateCubeGroup
    {
        [ReadOnly]
        public SharedComponentDataArray<CreateCube> CreateCube;
        public EntityArray Entity;
        public ComponentDataArray<Position> Position;
        public ComponentDataArray<Scale> scal;
        public readonly int Length;
    }
    [Inject] CreateCubeGroup cubeGroup;
    protected override void OnUpdate()
    {
        for (int i = 0; i < cubeGroup.Length; i++)
        {
            var spawner = cubeGroup.CreateCube[0];
            var sourceEntity = cubeGroup.Entity[0];
            var center = cubeGroup.Position[0].Value;
            
            var entities = new NativeArray<Entity>(spawner.count, Allocator.Temp);
            EntityManager.Instantiate(spawner.cube, entities); 
            var pos = new NativeArray<float3>(spawner.count, Allocator.Temp);
             setPos(ref pos);
            //使用for循环
            for (int j = 0; j < spawner.count; j++)
            {
                EntityManager.SetComponentData(entities[j], new Position { Value = pos[j] });
                EntityManager.SetComponentData(entities[j], new Scale { Value = new float3(1, 1f, 1) });

                EntityManager.AddSharedComponentData(entities[j], new MeshInstanceRenderer
                {                   
                    material = spawner.materials[UnityEngine.Random.Range(1,spawner.materials.Count)],
                    mesh = spawner.mesh
                });
            }
            entities.Dispose();
            pos.Dispose();
            EntityManager.RemoveComponent<CreateCube>(sourceEntity);         
            UpdateInjectedComponentGroups();
        }     
        #region
        // 使用while循环

        while (cubeGroup.Length != 0)
        {
            //var spawner = cubeGroup.CreateCube[0];
            //var sourceEntity = cubeGroup.Entity[0];
            //var center = cubeGroup.Position[0].Value;

            //var entities = new NativeArray<Entity>(spawner.count, Allocator.Temp);
            //EntityManager.Instantiate(spawner.cube, entities);
            //entities.Dispose();
            //EntityManager.RemoveComponent<CreateCube>(sourceEntity);
            //UpdateInjectedComponentGroups();
        }
        #endregion
        
    }
    //获取位置
    void setPos(ref NativeArray<float3> pos)
    {
        var count = pos.Length;
        var poss = new float3(0, 0, 0);
        int k = 0;
        int dates = (int)(math.pow(count, 1/3f));
        for (int y = 0; y < dates; y++)
        {
            for (int x = 0; x < dates; x++)
            {
                for (int z = 0; z < dates; z++)
                {       
                        pos[k] = new float3(x*1, y*1, z*1);
                        k++;    
                }           
            }
        }
    }
    //获取材质
    
}
