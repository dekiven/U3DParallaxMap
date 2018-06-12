using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxPlayerLayer : ParallaxLayer
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static new ParallaxPlayerLayer NewLayerObj(Transform parent, string name)
    {
        GameObject obj = new GameObject();
        obj.name = name;
        obj.transform.SetParent(parent);
        ParallaxPlayerLayer layer = obj.AddComponent<ParallaxPlayerLayer>();
        return layer;
    }
}
