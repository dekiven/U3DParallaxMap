using System;
using System.Collections.Generic;
using UnityEngine;

//地图单层的数据
[Serializable]
public class ParallaxLayerData : BaseConfig
{
    public float Distance;
    public List<ParallaxItemData> Items;


    public ParallaxLayerData()
    {
        Items = new List<ParallaxItemData>();
    }
}
