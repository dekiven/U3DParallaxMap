using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManagerBase<D, L> where D : BaseData where L : BaseDataList<D>
{
    private static DataManagerBase<D, L> sInstance;

    public Dictionary<string, D> mDatas;


    public static DataManagerBase<D, L> Instance
    {
        //get {return sInstance;}
        get
        {
            if (sInstance == null)
            {
                sInstance = new DataManagerBase<D, L>();
            }
            return sInstance;
        }
    }

    private DataManagerBase()
    {
        //mDatas = new Dictionary<string, D>();
        Clear();
    }

    public void LoadData(string path, Action<bool> callback = null)
    {
        string fname = Path.GetFileName(path);
        BaseConfig.LoadFromFile<L>(path.Replace("/" + fname, ""), fname, delegate (L obj) {
            if (null != obj)
            {
                foreach (D item in obj.DataList)
                {
                    if (!string.IsNullOrEmpty(item.ID))
                    {
                        mDatas[item.ID] = item;
                    }
                }
            }
            if (null != callback)
            {
                Debug.Log(mDatas.Count);
                callback(null != obj);
            }
        });

    }

    public D GetDataById(string id)
    {
        D data;
        mDatas.TryGetValue(id, out data);
        return data;
    }

    public void Clear()
    {
        mDatas = new Dictionary<string, D>();
    }
}
