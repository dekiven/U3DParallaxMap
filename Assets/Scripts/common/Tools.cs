using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class Tools
{
    public static string GetWriteableDataPath(string subPath = "")
    {
        string root = string.Empty;

#if UNITY_EDITOR
        root = Application.streamingAssetsPath;
#elif UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
        root = Application.streamingAssetsPath;
#else
        root = Application.persistentDataPath;        
#endif

        if (string.IsNullOrEmpty(subPath))
        {
            return root;
        }
        else
        {
            return PathCombine(root, subPath);
        }
    }

    public static string GetReadOnlyPath(string suPath = "")
    {
        return Application.streamingAssetsPath;
    }

    public static string PathCombine(string root, string file)
    {
        return FormatPathStr(Path.Combine(root, file));
    }

    public static string PathCombine(string root, string subRoot, string file)
    {
        return PathCombine(PathCombine(root, subRoot), file);
    }

    public static string FormatPathStr(string path)
    {
        return path.Replace('\\', '/');
    }

    public static bool CheckDirExists(string path, bool createIfNot = false)
    {
        bool exists = Directory.Exists(path);
        if (!exists && createIfNot)
        {
            Directory.CreateDirectory(path);
            exists = true;
        }
        return exists;
    }

    public static bool CheckFileExists(string path, bool createIfNot = false)
    {
        bool exists = File.Exists(path);
        if (!exists && createIfNot)
        {
            if (CheckDirExists(Directory.GetParent(path).FullName, true))
            {
                File.Create(path).Close();
                exists = true;
            }
        }
        return exists;
    }

    public static string RelativeTo(string fullPath, string relative2, bool withParentDir = false)
    {
        fullPath = FormatPathStr(fullPath);
        if (withParentDir) 
        {
            relative2 = Directory.GetParent(relative2).ToString();
        }
        relative2 = FormatPathStr(relative2);
        if (!relative2.EndsWith("/", System.StringComparison.Ordinal))
        {
            relative2 = relative2 + '/';
        }

        if (Path.IsPathRooted(fullPath) && fullPath.StartsWith(relative2, System.StringComparison.Ordinal))
        {
            fullPath = fullPath.Replace(relative2, "");

        }
        return fullPath;
    }

    //public static string GetAsbName(string path)
    //{
    //    path = FormatPathStr(path).TrimEnd('/');
    //    //return path.Replace("/", ".").ToLower() + GameConfig.STR_ASB_EXT;
    //    return path.ToLower() + GameConfig.STR_ASB_EXT;
    //}

    public static string GetUrlPath(string path, string subPath = "")
    {
        if (!string.IsNullOrEmpty(subPath))
        {
            path = PathCombine(path, subPath);
        }

        if (Application.isEditor)
        {
            return "file://" + path;
        }
        else if (Application.isMobilePlatform || Application.isConsolePlatform)
        {
            return "file:///" + path;
        }
        else // For standalone player.
        {
            return "file://" + path;
        }
    }

    ///// <summary>
    ///// 获取运行时的assetbundle所在根目录
    ///// </summary>
    ///// <returns></returns>
    //public static string GetAsbPath()
    //{
    //    return GetWriteableDataPath(GameConfig.STR_ASB_MANIFIST);
    //}
    ///// <summary>
    ///// 获取某assetbundle的url
    ///// </summary>
    ///// <param name="name"></param>
    ///// <returns></returns>
    //public static string GetAsbUrl(string name)
    //{
    //    return GetUrlPath(GetAsbPath(), name);
    //}
    ///// <summary>
    ///// 获取原始资源的根目录，在Assets/BundleRes,一般情况下只有编辑器会使用本函数
    ///// </summary>
    ///// <returns></returns>
    //public static string GetResPath(string path = "")
    //{
    //    if (string.IsNullOrEmpty(path))
    //    {
    //        return PathCombine(Application.dataPath, GameConfig.STR_RES_FOLDER);
    //    }else
    //    {
    //        return PathCombine(PathCombine(Application.dataPath, GameConfig.STR_RES_FOLDER), path);
    //    }

    //}

    ///// <summary>
    ///// 获取某原始资源的url
    ///// </summary>
    ///// <param name="name"></param>
    ///// <returns></returns>
    //public static string GetResUrl(string name)
    //{
    //    return GetUrlPath(GetResPath(), name);
    //}

    ////test
    //public static string GetLuaSrcPath()
    //{
    //    return PathCombine(Application.dataPath, GameConfig.STR_LUA_FOLDER);
    //}


    //public static string GetLuaAsbPath(string bundleName)
    //{
    //    return PathCombine(GetAsbPath(), "lua/" + bundleName).ToLower();
    //}

    ///// <summary>
    ///// 根据传入的路径获取正式加载资源时的asbPath和filePath
    ///// 在地图编辑器会用到
    ///// </summary>
    ///// <param name="fullPath"></param>
    ///// <param name="asbPath"></param>
    ///// <param name="filePath"></param>
    //public static void SplitResPath(string fullPath, out string asbPath , out string filePath, bool useFullPath = false)
    //{
    //    asbPath = null;
    //    filePath = null;
    //    if(string.IsNullOrEmpty(fullPath))
    //    {
    //        return;
    //    }
    //    string relative = Tools.RelativeTo(fullPath, Tools.GetResPath());
    //    if (!useFullPath || fullPath != relative)
    //    {
    //        int index = 0;
    //        for(int i = 0; i < 3 ; ++i)
    //        {
    //            if(index < relative.Length)
    //            {
    //                index = relative.IndexOf('/', index + 1);
    //            }
    //        }
    //        if (-1 != index )
    //        {
    //            asbPath = relative.Substring(0, index);
    //            filePath = relative.Substring(index + 1);
    //        }
    //    }
        
    //}

    public static string GetResFullPath(string subPath=null)
    {
        string path = PathCombine(Application.dataPath, GameConfig.STR_RES_FOLDER);
        if (string.IsNullOrEmpty(subPath))
        {
            return path;
        }
        else
        {
            return PathCombine(path, subPath);
        }
            
    }

    public static T NewComponentObj<T>(Transform parent, string name = null) where T : UnityEngine.Component
    {
        if(string.IsNullOrEmpty(name))
        {
            name = typeof(T).ToString();
        }
        var obj = new GameObject(name);
        obj.transform.SetParent(parent);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        return obj.AddComponent<T>();
    }

    //根据组件名“comName”生成新的带comName组件的Gameobject，comName组件是泛型T的子类
    public static T NewGenericComponentObj<T>(string comName, Transform parent, string name=null) where T :UnityEngine.Component
    {
        T component = null;
        Type type = typeof(Tools);
        if (null != type)
        {
            MethodInfo method = type.GetMethod("NewComponentObj", BindingFlags.Static | BindingFlags.Public);
            if (null != method)
            {
                Type GType = Type.GetType(comName);
                if (null != GType)
                {
                    var obj = method.MakeGenericMethod(new Type[] { GType, }).Invoke(type, new object[] { parent, name });
                    component = obj as T;
                    if (null != component)
                    {
                        Debug.Log(component.transform);
                    }
                }

            }
        }
        return component;
    }


    public static string GetAssetPath(string subPath = "", bool isFullPath = false)
    {
        string asset = isFullPath ? Application.dataPath : "Assets" ;
        if (!string.IsNullOrEmpty(subPath))
        {
            asset = PathCombine(asset, subPath);
        }
        return asset;
    }

    public static string RelativeToAsset(string fullPath)
    {
        return RelativeTo(fullPath, Directory.GetParent(Application.dataPath).ToString(), true);
    }

    ///// <summary>
    ///// 反序列化json字符串。
    ///// 需要第三方的 SimpleJson.cs 文件。
    ///// </summary>
    ///// <typeparam name="T">反序列化后的类型，可以是容器或类</typeparam>
    ///// <param name="json">json字符串</param>
    ///// <returns></returns>
    //public static T DeserializeJson<T>(string json)
    //{
    //    return SimpleJson.SimpleJson.DeserializeObject<T>(json);
    //}

    ///// <summary>
    ///// 将数据序列化为json字符串。
    ///// 需要第三方的 SimpleJson.cs 文件。
    ///// </summary>
    ///// <param name="data">要序列化的数据</param>
    ///// <returns></returns>
    //public static string SerializeObject(object data)
    //{
    //    return SimpleJson.SimpleJson.SerializeObject(data);
    //}
}
