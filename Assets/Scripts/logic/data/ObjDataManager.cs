using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjDataManager
{
    private static ObjDataManager sInstance;

    public Dictionary<string, ObjBaseData> mDatas;


    public static ObjDataManager Instance
    {
        //get {return sInstance;}
        get
        {
            if (sInstance == null)
            {
                sInstance = new ObjDataManager();
            }
            return sInstance;
        }
    }

    private ObjDataManager()
    {
        mDatas = new Dictionary<string, ObjBaseData>();
    }

    public void LoadData(string path, Action<bool> callback = null)
    {
        string fname = Path.GetFileName(path);
        BaseConfig.LoadFromFile<ObjDataList>(path.Replace("/" + fname, ""), fname, delegate (ObjDataList obj) {
            if (null != obj)
            {
                foreach (var item in obj.Objs)
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

    public ObjBaseData GetDataById(string id)
    {
        ObjBaseData data;
        mDatas.TryGetValue(id, out data);
        return data;
    }

    //public void LoadFromFile(string asbPath, string fileName)
    //{
    //    Debug.Log(asbPath);
    //    Debug.Log(fileName);
    //    GameResManager.Instance.LoadRes<TextAsset>(asbPath, fileName, delegate (UObj obj)
    //    {
    //        TextAsset text = obj as TextAsset;
    //        if (null != obj)
    //        {
    //            mDatas = JsonUtility.FromJson<Serialization<string, ObjData>>(text.text).ToDictionary();

    //        }

    //    });
    //}
}
