using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UObj = UnityEngine.Object;

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
        if(data.Items.Count > 0)
        {
            GameResManager.Instance.LoadRes<Sprite>("textures/map/mapBg", new string[] { "fb2bg0.png", "fb2bg1.png", "fb2bg2.png" }, delegate (UObj[] objs) {
                var sdic = new Dictionary<string, Sprite>();
                if (objs.Length > 0)
                {
                    foreach (var s in objs)
                    {
                        var ss = s as Sprite;
                        if (null != ss)
                        {
                            //Debug.Log(ss.name);
                            sdic[ss.name] = ss;
                        }
                    }
                }
                for (int i = 0; i < data.Items.Count; ++i)
                {
                    
                    var d = data.Items[i];
                    var sprite = Tools.NewComponentObj<SpriteRenderer>(transform, "bg_" + i);

                    sprite.transform.localPosition = d.Pos;
                    Sprite s; 
                    if (sdic.TryGetValue(d.ID, out s))
                    {
                        sprite.sprite = s;
                    }
                }
            });
        }

        yield return null;
        if (null != callback)
        {
            callback(true);
        }
    }
}
