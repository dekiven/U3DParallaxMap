using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMap : MonoBehaviour
{
    //=======================================================================properties begin=======================================================================
    //整个节点跟随的对象，摄像机也会跟随
    public Transform MoveTarget;
    public Camera MainCamera;
    //地图全部显示的最大高度（1080 / 100）
    public float MaxHeight = 11f;
    //地图全部显示的最小高度（500 / 100
    public float MinHeight = 5f;
    //当前显示高度
    public float CurHeight = 5f;
    //地平线位置
    public float HorizontalHeight = 1.2f;
    // 摄像机看向的视差节点位置（默认以玩家为中心）
    public float FoucsPosX = 0f;


    /**
     *视差地图有6层，分别是：
     *最大背景层：固定的层，不移动, 定义该层的深度和在list的index最大
     *太阳月亮层：太阳月亮等层，层移动位置极其缓慢，内部对象自己有移动（太阳根据时间有移动）
     *远景层：较远的层，远山等
     *中景层：树木建筑等
     *玩家层：玩家所在层，道路、可交互的建筑等都在这个层 ,这个层的idx为1
     *近景层：将玩家遮挡的层， 定义该层的深度和在list的index最小（0）
     **/
    public int LayerCount = Enum.GetNames(typeof(ParallaxLayerEnum)).Length;


    //视差节点层
    private List<ParallaxLayer> mLayers;
    //当前地图数据
    private ParallaxMapdData mCurMapData;
    private float mDistance;
    //test
    public float ScreenWidth;
    //-----------------------------------------------------------------------properties end-------------------------------------------------------------------------

    //=======================================================================u3d funcs begin=======================================================================
    void Start()
    {
        InitMap();
        if (null == MainCamera)
        {
            MainCamera = Camera.main;
        }
        MainCamera.orthographic = true;

        //test
        test();
    }

    void Update()
    {
        //if (Input.GetKeyUp("1"))
        //{
        //    CurHeight += 0.1f;
        //    updateCamera();
        //}
        //if (Input.GetKeyUp("2"))
        //{
        //    CurHeight -= 0.1f;
        //    updateCamera();
        //}
        updateCamera();
        FoucsLeft();
    }
    //-----------------------------------------------------------------------u3d funcs end-------------------------------------------------------------------------

    //=======================================================================public funcs begin=======================================================================
    public bool InitMap()
    {
        mLayers = new List<ParallaxLayer>();
        for (int i = LayerCount - 1; i >= 0; --i)
        {
            ParallaxLayer layer = null;
            if (1 == i)
            {
                layer = ParallaxPlayerLayer.NewLayerObj(transform, "layer_" + i);
            }
            else
            {
                layer = ParallaxLayer.NewLayerObj(transform, "layer_" + i);
            }
            layer.Depth = i;
            mLayers.Add(layer);

        }
        return true;
    }

    public void ClearMap()
    {
        foreach (ParallaxLayer layer in mLayers)
        {
            layer.Clear();
        }
    }

    public bool SetMapData(ParallaxMapdData data, Action<bool> callback=null)
    {
        if (null == data)
        {
            return false;
        }
        StartCoroutine(startSetMapData(data, callback));
        return true;
    }

    //移动map的焦点（玩家所在位置，一般情况先玩家在地图中居中，但玩家可能会移动到地图左右两个边缘）
    public bool MoveFocus(float offset)
    {
        FoucsPosX += offset;
        return FoucsTo(FoucsPosX);
    }

    public bool FoucsTo(float posX)
    {
        if (mDistance < ScreenWidth) 
        {
            Debug.LogError("mDistance < ScreenWith, use a larger one!");
            return false;
        }
        posX = Mathf.Clamp(posX, ScreenWidth / 2, mDistance - ScreenWidth / 2);
        FoucsPosX = posX;
        foreach (ParallaxLayer layer in mLayers)
        {
            layer.FoucsTo(posX);
        }
        return true;
    }

    public bool FoucsLeft()
    {
        return FoucsTo(0f);
    }

    public bool FoucsRight()
    {
        return FoucsTo(mDistance);
    }
    //-----------------------------------------------------------------------public funcs end-------------------------------------------------------------------------

    //=======================================================================private funcs begin=======================================================================
    private IEnumerator startSetMapData(ParallaxMapdData data, Action<bool> callback=null)
    {
        mCurMapData = data;
        //string leftID = mCurMapData.LeftID;
        //string rightID = mCurMapData.RightID;
        mDistance = mCurMapData.Distance;

        for (int i = 0; i < mLayers.Count; ++i)
        {
            ParallaxLayer layer = mLayers[i];
            //TODO:根据地图配置调整layer移动比例
            layer.MoveScale = LayerCount - i;
        }
        //TODO:创建地图

        if (null != callback)
        {
            callback(true);
        }
        yield return null;
    }

    private void updateCamera()
    {
        if (null == mCurMapData)
        {
            return;
        }
        ScreenWidth = CurHeight * MainCamera.aspect;

        CurHeight = Mathf.Clamp(CurHeight, MinHeight,MaxHeight);
        var pos = MainCamera.transform.position;
        var size = CurHeight / 2;
        pos.y = HorizontalHeight + size * (1f-(HorizontalHeight / MaxHeight) * 2);
        MainCamera.transform.position = pos;
        MainCamera.orthographicSize = size;

        FoucsTo(FoucsPosX);
    }
    //-----------------------------------------------------------------------private funcs end-------------------------------------------------------------------------
    //test
    private void test()
    {
        //test
        //var data = ParallaxMapdData.LoadFromFile()
        var data = new ParallaxMapdData();
        //data.ID = "testMap";
        data.Distance = 100f;
        data.LeftID = "left_test_1";
        data.RightID = "right_test_1";
        data.SepecialItems = new List<ParallaxItemData>();
        //data.SaveToFile(Tools.GetResFullPath("conf/map/test/test1.json"));
        SetMapData(data);
    }
}
