using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseState
{
    public BaseState(GameObject gameobject)
    {
        this.gameObject = gameobject;
    }
    protected GameObject gameObject;
    protected Transform transform;

    public abstract Type Tick();
    
}
