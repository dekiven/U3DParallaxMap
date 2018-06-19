using System;
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
        updatePos();
	}

    public void MoveX(float offset)
    {
        float x = transform.localPosition.x + offset;
        MaxPosX = Map.Distance;
        if (!Equals(offset, 0f) && Map && x > MinPosX && x < MaxPosX)
        {
            transform.Translate(offset, 0, 0);
            Map.FoucsTo(transform.localPosition.x);
        }
    }

    private void updatePos()
    {
        float h = Input.GetAxis("Horizontal");
        MoveX(h);
    }
}
