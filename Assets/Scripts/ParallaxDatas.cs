using System;
using System.Collections.Generic;
using UnityEngine;

public enum ParallaxLayerEnum
{
    //离屏幕最近的层
    L_Near = 0,
    //玩家所在层
    L_Player,
    //中景层，建筑等
    L_Middle,
    //远景层，山、云等
    L_Far,
    //天空层，太阳月亮等
    L_Sky,
    //最远层，固定的bg
    L_Bg,
}

//地图上Sprite等的数据
public class ParallaxItemData : JsonConfig
{
    public string ID;
    //public string Type;
    //public string Name;
    public Vector2 Pos;
    //public string Res;
    //public float Rotate;
}

//地图的数据
public class ParallaxMapdData : JsonConfig
{
    public string LeftID;
    public string RightID;
    public float Distance;

    public List<ParallaxItemData> SepecialItems;

    public ParallaxLayerData GenLayerData(int indx, EnviromentData data)
    {
        ParallaxLayerData layerData = new ParallaxLayerData();
        layerData.Distance = Distance;
        var items = new List<ParallaxItemData>();
        var idx = (ParallaxLayerEnum)Enum.ToObject(typeof(ParallaxLayerEnum), indx);
        switch(idx)
        {
            case ParallaxLayerEnum.L_Bg :
                genBGData(data, ref layerData);
                break;
            case ParallaxLayerEnum.L_Sky :
                genSkyData(data, ref layerData);
                break;
            case ParallaxLayerEnum.L_Far:
                genFarData(data, ref layerData);
                break;
            case ParallaxLayerEnum.L_Middle:
                genMiddleData(data, ref layerData);
                break;
            case ParallaxLayerEnum.L_Player:
                genPlayerData(data, ref layerData);
                break;
            case ParallaxLayerEnum.L_Near:
                genNearData(data, ref layerData);
                break;
        }
        layerData.Items = items;
        return layerData;
    }

    public List<ParallaxLayerData> GenLayerDatas(EnviromentData data)
    {
        List<ParallaxLayerData> layerDatas = new List<ParallaxLayerData>();
        return layerDatas;
    }

    private void genBGData(EnviromentData enviromentData, ref ParallaxLayerData data)
    {
        if (enviromentData.time < 0)
        {
            Debug.Log("哈哈哈哈，环境数据没有用");
        }
        //TODO:
    }

    private void genSkyData(EnviromentData enviromentData, ref ParallaxLayerData data)
    {
        if (enviromentData.time < 0)
        {
            Debug.Log("哈哈哈哈，环境数据没有用");
        }
        //TODO:
    }

    private void genFarData(EnviromentData enviromentData, ref ParallaxLayerData data)
    {
        if (enviromentData.time < 0)
        {
            Debug.Log("哈哈哈哈，环境数据没有用");
        }
        //TODO:
    }

    private void genMiddleData(EnviromentData enviromentData, ref ParallaxLayerData data)
    {
        if (enviromentData.time < 0)
        {
            Debug.Log("哈哈哈哈，环境数据没有用");
        }
        //TODO:
    }

    private void genPlayerData(EnviromentData enviromentData, ref ParallaxLayerData data)
    {
        if (enviromentData.time < 0)
        {
            Debug.Log("哈哈哈哈，环境数据没有用");
        }
        //TODO:
    }

    private void genNearData(EnviromentData enviromentData, ref ParallaxLayerData data)
    {
        if (enviromentData.time < 0)
        {
            Debug.Log("哈哈哈哈，环境数据没有用");
        }
        //TODO:
    }
}

//地图单层的数据
public class ParallaxLayerData : JsonConfig
{
    public float Distance;
    public List<ParallaxItemData> Items;
}


