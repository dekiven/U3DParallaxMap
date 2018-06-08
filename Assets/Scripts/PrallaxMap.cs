using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrallaxMap : MonoBehaviour
{

    //整个节点跟随的对象，摄像机也会跟随
    public Transform MoveTarget;

    //视差节点层
    private Dictionary<string, ParallaxLayer> mLayers;

    public void ClearMap()
    {

    }

    public bool SetMap(ParallaxMapdData data)
    {
        return true;
    }

    public bool MoveHorizontal(float offsetX)
    {
        return true;
    }
}

public class ParallaxMapdData
{
    
}