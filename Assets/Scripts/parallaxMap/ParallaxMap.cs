﻿using System;
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
    private DataManagers mDataManagers;

    //视差节点层
    private List<ParallaxLayer> mLayers;
    //当前地图数据
    private ParallaxMapData mCurMapData;
    public float Distance;
    //test
    public float ScreenWidth;
    //-----------------------------------------------------------------------properties end-------------------------------------------------------------------------


    //=======================================================================u3d funcs begin=======================================================================

    private void Awake()
    {
        mDataManagers = DataManagers.Instance;

    }

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
        if (Input.GetKeyUp("1"))
        {
            FoucsPosX += 1f;
            FoucsTo(FoucsPosX);
        }
        if (Input.GetKeyUp("2"))
        {
            FoucsPosX -= 1f;
            FoucsTo(FoucsPosX);
        }
        updateCamera();
        //FoucsLeft();
    }
    //-----------------------------------------------------------------------u3d funcs end-------------------------------------------------------------------------

    //=======================================================================public funcs begin=======================================================================
    public bool InitMap()
    {
        mLayers = new List<ParallaxLayer>();
        for (int i = 0; i < LayerCount; ++i)
        {
            ParallaxLayer layer = null;
            if (1 == i)
            {
                layer = Tools.NewComponentObj<ParallaxPlayerLayer>(transform, ((ParallaxLayerEnum)i).ToString());
            }
            else
            {
                layer = Tools.NewComponentObj<ParallaxLayer>(transform, ((ParallaxLayerEnum)i).ToString());
            }
            layer.Depth = i;
            layer.transform.localPosition = new Vector3(0, 0, i);
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

    public bool SetMapData(ParallaxMapData data, Action<bool> callback=null)
    {
        if (null == data)
        {
            return false;
        }
        StartCoroutine(startSetMapData(data, callback));
        return true;
    }

    //移动map的焦点（玩家所在位置，一般情况先玩家在地图中居中，但玩家可能会移动到地图左右两个边缘）
    public float MoveFocus(float offset)
    {
        FoucsPosX += offset;
        return FoucsTo(FoucsPosX);
    }

    public float FoucsTo(float posX)
    {
        if (Distance < ScreenWidth) 
        {
            Debug.LogError("mDistance < ScreenWith, use a larger one!");
            return -1;
        }
        posX = Mathf.Clamp(posX, ScreenWidth / 2, Distance - ScreenWidth / 2);
        FoucsPosX = posX;
        foreach (ParallaxLayer layer in mLayers)
        {
            layer.FoucsTo(posX);
        }
        return posX;
    }

    public float FoucsLeft()
    {
        return FoucsTo(0f);
    }

    public float FoucsRight()
    {
        return FoucsTo(Distance);
    }
    //-----------------------------------------------------------------------public funcs end-------------------------------------------------------------------------

    //=======================================================================private funcs begin=======================================================================
    private IEnumerator startSetMapData(ParallaxMapData data, Action<bool> callback=null)
    {
        mCurMapData = data;
        //string leftID = mCurMapData.LeftID;
        //string rightID = mCurMapData.RightID;
        Distance = mCurMapData.Distance;

        //创建地图
        var layerDatas = mCurMapData.GenLayerDatas(new EnviromentData());
        int i = mLayers.Count - 1;
        int curI = -10086;
        while (i > -1)
        {
            if (i != curI)
            {
                //Debug.Log("set layer :" + i);
                curI = i;
                var layer = mLayers[i];
                //根据地图配置调整layer移动比例
                layer.MoveScale = ParallaxConst.LayerScales[i];
                layer.SetLayerData(layerDatas[i], delegate (bool rst) {
                    i -= 1;
                    //Debug.Log("set layer i -= 1");
                });
            }
            yield return null;
        }

        FoucsLeft();
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
        var size = CurHeight / 2;

        if (!Equals(MainCamera.orthographicSize, size))
        {
            ScreenWidth = CurHeight * MainCamera.aspect * 2;

            CurHeight = Mathf.Clamp(CurHeight, MinHeight, MaxHeight);
            var pos = MainCamera.transform.position;
            pos.y = HorizontalHeight + size * (1f - (HorizontalHeight / MaxHeight) * 2);
            MainCamera.transform.position = pos;

            FoucsTo(FoucsPosX);

            MainCamera.orthographicSize = size;
        }
    }
    //-----------------------------------------------------------------------private funcs end-------------------------------------------------------------------------
    //test
    private void test()
    {
        Debug.Log(typeof(ParallaxMap));
        //test
        mDataManagers.ObjDataManager.LoadData("conf/obj/test/testObjs.asset", delegate (bool rst) {
            Debug.Log("load Obj list:" + rst.ToString());
            if (!rst)
            {
                Debug.LogError("load testObjs.asset failed");   
                return;
            }
            BaseConfig.LoadFromFile<ParallaxMapData>("conf/map/test", "test1.asset", delegate (ParallaxMapData d)
            {
                if (null != d)
                {
                    Debug.Log(d.SepecialItems.Count);
                    foreach (var item in d.SepecialItems)
                    {
                        Debug.Log(item.ID);
                    }
                    SetMapData(d);
                }
                else 
                {
                    Debug.LogError("load test1.asset failed");   
                }
            });

            //mDataManager = ObjDataManager.Instance;
            //if (null != mDataManager)
            //{
            //    Debug.Log(mDataManager.mDatas);
            //}
        });
    }
}
