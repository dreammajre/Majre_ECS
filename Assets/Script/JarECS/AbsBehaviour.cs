using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

/*
 * 单例模式
 */
public abstract class AbsBehaviour<T> : MonoBehaviour where T : class ,new()
{
    static T _instance = null;
    public static T Instance { get { return _instance ?? (_instance = new T()); } }

    void Awake()
    {
        Debug.Log("AbsBehaviour");
        _instance = this as T;
    }

}

