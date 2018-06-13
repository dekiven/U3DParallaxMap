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

public class ParallaxConst
{
    public static readonly float[] LayerScales = { 1.2f, 1f, 0.8f, 0.5f, 0.2f, 0f };

    ////离屏幕最近的层
    //public const float ScaleNear = 1.2f;
    ////玩家所在层
    //public const float ScalePlayer = 1f;
    ////中景层，建筑等
    //public const float ScaleMiddle = 0.8f;
    ////远景层，山、云等
    //public const float ScaleFar = 0.5f;
    ////天空层，太阳月亮等
    //public const float ScaleSky = 0.2f;
    ////最远层，固
    //public const float ScaleBg = 0f;
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
    public int Index;

    public List<ParallaxItemData> SepecialItems;

    public ParallaxMapdData()
    {
        SepecialItems = new List<ParallaxItemData>();
    }

    //public new string ToJson(bool prettyPrint = false)
    //{
    //    return JsonUtility.ToJson(this, prettyPrint);
    //}

    public ParallaxLayerData GenLayerData(int index, EnviromentData data)
    {
        ParallaxLayerData layerData = new ParallaxLayerData();
        layerData.Distance = Distance;
        Index = index;
        var idx = (ParallaxLayerEnum)Enum.ToObject(typeof(ParallaxLayerEnum), index);
        switch (idx)
        {
            case ParallaxLayerEnum.L_Bg:
                genBGData(data, ref layerData);
                break;
            case ParallaxLayerEnum.L_Sky:
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

        return layerData;
    }

    public List<ParallaxLayerData> GenLayerDatas(EnviromentData data)
    {
        List<ParallaxLayerData> layerDatas = new List<ParallaxLayerData>();
        for (int i = 0; i < Enum.GetNames(typeof(ParallaxLayerEnum)).Length; i++)
        {
            layerDatas.Add(GenLayerData(i, data));
        }
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
        //test 测试代码，正式版本使用配置的数据
        float w = 10.24f;
        int count = (int)Math.Ceiling(Distance * ParallaxConst.LayerScales[Index] / w);
        for (int i = 0; i < count; ++i)
        {
            var itemData = new ParallaxItemData();
            itemData.ID = "fb2bg" + i % 3;
            itemData.Pos = new Vector2((0.5f + i) * w, 2.33f);
            data.Items.Add(itemData);
        }
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


    public ParallaxLayerData()
    {
        Items = new List<ParallaxItemData>();
    }
}


