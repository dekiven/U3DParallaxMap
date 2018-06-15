using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Datas/ObjDataList")]
[Serializable]
public class ObjDataList : BaseConfig
{
    //private List<ObjBaseData> mObjs;

    ////[SerializeField]
    //public List<ObjBaseData> Objs
    //{
    //get { return mObjs; }
    //set { mObjs = value; }
    //}
    public List<ObjBaseData> Objs;

    public ObjDataList()
    {
        Objs = new List<ObjBaseData>();
    }
}
