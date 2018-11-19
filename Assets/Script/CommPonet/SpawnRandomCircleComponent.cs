using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;

public class SpawnRandomCircleComponent:SharedComponentDataWrapper<SpawnRandomCircle>  { 
}

[Serializable]
public struct SpawnRandomCircle : ISharedComponentData {
    public GameObject prefab;
    public bool spawnLocak;
    public float readius;
    public int count;
}