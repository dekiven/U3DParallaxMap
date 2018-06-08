using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UObj = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class JsonConfig
{

    public string ToJson(bool prettyPrint = false)
    {
        return JsonUtility.ToJson(this, prettyPrint);
    }

    public static void LoadFromFile<T>(string asbPath, string fileName, Action<T> action = null) where T : JsonConfig
    {
        GameResManager.Instance.LoadRes<TextAsset>(asbPath, fileName, delegate (UObj obj)
        {
            TextAsset text = obj as TextAsset;
            if (null != obj)
            {
                T t = JsonUtility.FromJson<T>(text.text);
                //Resources.UnloadAsset(text);
                if (null != action)
                {
                    action(t);
                }
            }

        });
    }

    public void SaveToFile(string path)
    {
        string s = ToJson();
        Tools.CheckFileExists(path, true);
        File.WriteAllText(path, s, Encoding.UTF8);
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}
