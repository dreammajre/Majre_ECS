using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;

public class CreatCubeComponent : SharedComponentDataWrapper<CreateCube>
{
}
[Serializable]
public struct CreateCube:ISharedComponentData{
    public GameObject cube;
    [Header("Mesh Info")]
    public Mesh mesh;
    [Header("Material Info")]
    public Material material;
    public int count;
    [Header("MaterialList Info")]
    public List<Material> materials;
}


