using System;
using UnityEngine;

//[CreateAssetMenu(menuName = "Datas/ObjData")]
[Serializable]
public class ObjData : BaseData
{
    //public string ID;
    public string Name;
    //public float HP;
    public string Type;
    //public bool IsStatic;
    //public int Level;
    //资源路径，场景上的物体显示路径或者道具技能等的图标
    public string Sprite;
    public string Introduction;
}
