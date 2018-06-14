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





