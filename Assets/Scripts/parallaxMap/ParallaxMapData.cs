using System;
using System.Collections.Generic;
using UnityEngine;

//地图的数据
[CreateAssetMenu(menuName = "Datas/ParallaxMapdData")]
[Serializable]
public class ParallaxMapData : BaseConfig
{
    public string LeftID;
    public string RightID;
    public float Distance;
    public int Index;

    [SerializeField]
    public List<ParallaxItemData> SepecialItems;
    //public List<string> testList;

    public ParallaxMapData()
    {
        SepecialItems = new List<ParallaxItemData>();
        //testList = new List<string>();
        //for (int i = 0; i < 10; i++)
        //{
        //    testList.Add("string_" + i);
        //}
    }

    //public new string ToJson(bool prettyPrint = false)
    //{
    //    return JsonUtility.ToJson(this, prettyPrint);
    //}

    public ParallaxLayerData GenLayerData(int index, EnviromentData data)
    {
        ParallaxLayerData layerData = NewConifg<ParallaxLayerData>();
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
            itemData.ID = "map_bg_fb2bg" + i % 3;
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

