using System;
using System.Collections.Generic;
using UnityEngine;

//地图上Sprite等的数据
[Serializable]
public class ParallaxItemData //: BaseConfig
{
    public string ID;
    //public string Type;
    //public string Name;
    [SerializeField]
    public Vector2 Pos;
    //public string Res;
    //public float Rotate;
}
