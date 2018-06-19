using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObj : RenderObj {
    public ParallaxMap Map;
    public float MinPosX = 0;
    public float MaxPosX = 0;

	// Use this for initialization
	void Start () {
        if (null == Map)
        {
            Map = FindObjectOfType<ParallaxMap>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");
        float x = transform.localPosition.x + h;
        MaxPosX = Map.Distance;
        if (!Equals(h, 0f) && Map && x > MinPosX && x < MaxPosX)
        {
            transform.Translate(h, 0, 0);
            Map.FoucsTo(transform.localPosition.x);
        }
	}
}
