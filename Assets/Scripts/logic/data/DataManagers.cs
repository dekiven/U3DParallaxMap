using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManagers {

    private static DataManagers sInstance;

    public DataManagerBase<ObjData, ObjDataList> ObjDataManager;

    public static DataManagers Instance{
        get
        {
            if(null == sInstance)
            {
                sInstance = new DataManagers();
            }
            return sInstance;
        }
    }

    private DataManagers()
    {
        initManagers();
    }

    private void initManagers()
    {
        ObjDataManager = DataManagerBase<ObjData, ObjDataList>.Instance;
        //ObjDataManager.LoadData("");
    }
}
