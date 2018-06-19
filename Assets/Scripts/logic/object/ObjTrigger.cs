using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[DisallowMultipleComponent(typeof(Collider2D))]
[RequireComponent(typeof(Collider2D))]
public class ObjTrigger : MonoBehaviour {

    private Collider2D mTrigger;
    public string TriggerType;

    public Action<ObjTrigger, Collider2D> EnterAction;
    public Action<ObjTrigger, Collider2D> StayAction;
    public Action<ObjTrigger, Collider2D> ExitAction;

	// Use this for initialization
	void Start () {
        if(null == mTrigger)
        {
            mTrigger = GetComponent<Collider2D>();
            if (null != mTrigger)
            {
                mTrigger.isTrigger = true;
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (null != EnterAction)
        {
            EnterAction(this, collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (null != StayAction)
        {
            StayAction(this, collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (null != ExitAction)
        {
            ExitAction(this, collision);
        }
    }
}
