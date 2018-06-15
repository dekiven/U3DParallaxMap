using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxPlayerLayer : ParallaxLayer
{
    public PlayerObj Player;
    //public ParallaxMap Map;

    // Use this for initialization
    void Start()
    {
        Player = Tools.NewComponentObj<PlayerObj>(transform, "PlayerObj");
        Player.InitByID("");
        Player.transform.localPosition = new Vector3(1, 3, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
