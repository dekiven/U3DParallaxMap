using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour {

    public float MoveScale = 1;
    public int Depth = 0;
    public float Distance = 0;
    public string LeftID = "";
    public string RightID = "";

    private ParallaxLayerData mCurData;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Clear()
    {
        
    }

    public void SetLayerData(ParallaxLayerData data, Action<bool> callback = null)
    {
        StartCoroutine(startSetLayerData(data, callback));
    }

    public void FoucsTo(float posX)
    {
        var pos = transform.position;
        pos.x = posX * MoveScale;
        transform.position = pos;
    }

    public static ParallaxLayer NewLayerObj(Transform parent, string name)
    {
        GameObject obj = new GameObject();
        obj.name = name;
        obj.transform.SetParent(parent);
        ParallaxLayer layer = obj.AddComponent<ParallaxLayer>();
        return layer;
    }

    private IEnumerator startSetLayerData(ParallaxLayerData data, Action<bool> callback = null)
    {
        mCurData = data;
        Distance = data.Distance;
        for (int i = 0; i < data.Items.Count; ++i )
        {
            var d = data.Items[i];
        }
        yield return null;
        if (null != callback)
        {
            callback(true);
        }
    }
}
