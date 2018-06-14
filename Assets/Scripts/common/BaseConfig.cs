using System;
using UnityEngine;
using UObj = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseConfig : ScriptableObject
{

    //public string ToJson(bool prettyPrint = false)
    //{
    //    return Tools.SerializeObject(this);
    //}

    public static void LoadFromFile<T>(string asbPath, string fileName, Action<T> action = null) where T : BaseConfig
    {
        GameResManager.Instance.LoadRes<ScriptableObject>(asbPath, fileName, delegate (UObj obj)
        {
            T t = obj as T;
            if (null != obj)
            {
                if (null != action)
                {
                    action(t);
                }
            }

        });
    }

    public void SaveToFile<T>(string path) where T : BaseConfig
    {
        Debug.Log(path);
#if UNITY_EDITOR
        T t = this as T;
        if (t)
        {
            Tools.CheckFileExists(path, true);
            AssetDatabase.CreateAsset(this as T, Tools.GetAssetPath(path));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError("t is null");
        }
#endif
    }

    public void SaveToFile(string path)
    {
        Debug.Log(path);
#if UNITY_EDITOR
        Tools.CheckFileExists(path, true);
        AssetDatabase.CreateAsset(this, Tools.GetAssetPath(path));
        AssetDatabase.Refresh();

#endif
    }

    public static T NewConifg<T>() where T : BaseConfig
    {
        T v = CreateInstance<T>() as T;
        return v;
    }
}
