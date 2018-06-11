using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionAdaptor : MonoBehaviour {

    public SpriteRenderer Sprite1;
    public int width = 0;//Screen.width;
    public int height = 0;//Screen.height;

	// Use this for initialization
	void Start () {
        Debug.Log(Screen.currentResolution);
        Debug.Log(Sprite1.sprite.pixelsPerUnit);
	}
	
	// Update is called once per frame
	void Update () {
        if (width != Screen.width || height != Screen.height)
        {
            width = Screen.width;
            height = Screen.height;
            Debug.Log(Screen.width + "," + Screen.height);
        }
	}
}
