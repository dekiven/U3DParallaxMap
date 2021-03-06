﻿using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UObj = UnityEngine.Object;

#if USE_TOLUA
using LuaInterface;
#else
public class LuaFunction{
    public void Call(object obj)
    {

    }
    public void Call(object[] objs)
    {
        
    }
    public void Dispose()
    {
        
    }
}
public class LuaByteBuffer{
    public LuaByteBuffer(byte[] bytes)
    {
        
    }
}
#endif


//载入方式参考LuaFramework_UGUI->ResourceManager.cs
//github: https://github.com/jarjin/LuaFramework_UGUI



public class GameResManager : MonoBehaviour
{
    //单例模式组件 begin----------------------------------------------
    private static volatile GameResManager sInstance;
    private static object syncRoot = new object();
    public static GameResManager Instance
    {
        get
        {
            if (sInstance == null)
            {
                lock (syncRoot)
                {
                    if (sInstance == null)
                    {
                        GameResManager[] instances = FindObjectsOfType<GameResManager>();
                        if (instances != null)
                        {
                            for (var i = 0; i < instances.Length; i++)
                            {
                                Destroy(instances[i].gameObject);
                            }
                        }
                        GameObject go = new GameObject();
                        go.name = typeof(GameResManager).ToString();
                        sInstance = go.AddComponent<GameResManager>();
                        DontDestroyOnLoad(go);
                    }
                }
            }
            return sInstance;
        }
    }
    //单例模式组件 end================================================


    //逻辑 begin ------------------------------------------------------------
    string[] mAllManifest = null;
    AssetBundleManifest m_AssetBundleManifest = null;
    Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();
    Dictionary<string, AssetBundleInfo> m_LoadedAssetBundles = new Dictionary<string, AssetBundleInfo>();
    Dictionary<string, List<LoadAssetRequest>> m_LoadRequests = new Dictionary<string, List<LoadAssetRequest>>();

    public void Initialize(string manifestName, Action initOK)
    {
        if (GameConfig.Instance.useAsb)
        {
            LoadAsset<AssetBundleManifest>(manifestName, new string[] { "AssetBundleManifest" }, delegate(UObj[] objs)
            {
                if (objs.Length > 0)
                {
                    m_AssetBundleManifest = objs[0] as AssetBundleManifest;
                    mAllManifest = m_AssetBundleManifest.GetAllAssetBundles();
                }
                if (initOK != null) initOK();
            });
        }else
        {
            if (initOK != null) initOK();
        }
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    //public void LoadAsset<T>(string abName, string[] assetNames, Action<UObj[]> action = null) where T : UObj
    void LoadAsset<T>(string abName, string[] assetNames, Action<UObj[]> action = null, LuaFunction func = null) where T : UObj
    {
        string path = abName;
        // manifest Assetbundle文件没有后缀，文件名跟release的Assetbundle根目录同名
        bool isManifest = true;
        if (path.EndsWith(GameConfig.STR_ASB_EXT))
        {
            isManifest = false;
            path = path.Substring(0, path.Length - GameConfig.STR_ASB_EXT.Length);
        }
        if (!isManifest)
        {
            List<string> names = new List<string>();
            foreach (var item in assetNames)
            {
                names.Add(Tools.PathCombine("Assets/" + GameConfig.STR_RES_FOLDER, path, item));
            }
            assetNames = names.ToArray();
        }

        LoadAssetRequest request = new LoadAssetRequest();
        request.assetType = typeof(T);
        request.assetNames = assetNames;
        request.luaFunc = func;
        request.sharpFunc = action;

        List<LoadAssetRequest> requests = null;
        if (!m_LoadRequests.TryGetValue(abName, out requests))
        {
            requests = new List<LoadAssetRequest>();
            requests.Add(request);
            m_LoadRequests.Add(abName, requests);
            StartCoroutine(onLoadAsset<T>(abName));
        }
        else
        {
            requests.Add(request);
        }
    }

    IEnumerator onLoadAsset<T>(string abName) where T : UObj
    {
        AssetBundleInfo bundleInfo = GetLoadedAssetBundle(abName);
        if (bundleInfo == null)
        {
            yield return StartCoroutine(OnLoadAssetBundle(abName, typeof(T)));

            bundleInfo = GetLoadedAssetBundle(abName);
            if (bundleInfo == null)
            {
                m_LoadRequests.Remove(abName);
                Debug.LogError("OnLoadAsset--->>>" + abName);
                yield break;
            }
        }
        List<LoadAssetRequest> list = null;
        if (!m_LoadRequests.TryGetValue(abName, out list))
        {
            m_LoadRequests.Remove(abName);
            yield break;
        }
        for (int i = 0; i < list.Count; i++)
        {
            string[] assetNames = list[i].assetNames;
            List<UObj> result = new List<UObj>();

            AssetBundle ab = bundleInfo.m_AssetBundle;
            for (int j = 0; j < assetNames.Length; j++)
            {
                string assetPath = assetNames[j];
                AssetBundleRequest request = ab.LoadAssetAsync(assetPath, list[i].assetType);
                yield return request;
                result.Add(request.asset);

                ////TODO:UnloadAsset
                //Resources.UnloadAsset(request.asset);
            }
            if (list[i].sharpFunc != null)
            {
                list[i].sharpFunc(result.ToArray());
                list[i].sharpFunc = null;
            }
            if (list[i].luaFunc != null)
            {
                list[i].luaFunc.Call((object)result.ToArray());
                list[i].luaFunc.Dispose();
                list[i].luaFunc = null;
            }
            bundleInfo.m_ReferencedCount++;
        }
        m_LoadRequests.Remove(abName);
    }

    IEnumerator OnLoadAssetBundle(string abName, Type type)
    {
        yield return null;
        //string url = Tools.GetAsbUrl(abName);

        //WWW download = null;
        //if (type == typeof(AssetBundleManifest))
        //    download = new WWW(url);
        //else
        //{
        //    string[] dependencies = m_AssetBundleManifest.GetAllDependencies(abName);
        //    if (dependencies.Length > 0)
        //    {
        //        m_Dependencies.Add(abName, dependencies);
        //        for (int i = 0; i < dependencies.Length; i++)
        //        {
        //            string depName = dependencies[i];
        //            AssetBundleInfo bundleInfo = null;
        //            if (m_LoadedAssetBundles.TryGetValue(depName, out bundleInfo))
        //            {
        //                bundleInfo.m_ReferencedCount++;
        //            }
        //            else if (!m_LoadRequests.ContainsKey(depName))
        //            {
        //                yield return StartCoroutine(OnLoadAssetBundle(depName, type));
        //            }
        //        }
        //    }
        //    download = WWW.LoadFromCacheOrDownload(url, m_AssetBundleManifest.GetAssetBundleHash(abName), 0);
        //}
        //yield return download;

        //AssetBundle assetObj = download.assetBundle;
        //if (assetObj != null)
        //{
        //    m_LoadedAssetBundles.Add(abName, new AssetBundleInfo(assetObj));
        //}
    }

    AssetBundleInfo GetLoadedAssetBundle(string abName)
    {
        AssetBundleInfo bundle = null;
        m_LoadedAssetBundles.TryGetValue(abName, out bundle);
        if (bundle == null) return null;

        // No dependencies are recorded, only the bundle itself is required.
        string[] dependencies = null;
        if (!m_Dependencies.TryGetValue(abName, out dependencies))
            return bundle;

        // Make sure all dependencies are loaded
        foreach (var dependency in dependencies)
        {
            AssetBundleInfo dependentBundle;
            m_LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
            if (dependentBundle == null) return null;
        }
        return bundle;
    }

    /// <summary>
    /// 此函数交给外部卸载专用，自己调整是否需要彻底清除AB
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="isThorough"></param>
    public void UnloadAssetBundle(string abName, bool isThorough = false)
    {
        abName = GetRealAssetPath(abName);
        Debug.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory before unloading " + abName);
        UnloadAssetBundleInternal(abName, isThorough);
        UnloadDependencies(abName, isThorough);
        Debug.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory after unloading " + abName);
    }

    void UnloadDependencies(string abName, bool isThorough)
    {
        string[] dependencies = null;
        if (!m_Dependencies.TryGetValue(abName, out dependencies))
            return;

        // Loop dependencies.
        foreach (var dependency in dependencies)
        {
            UnloadAssetBundleInternal(dependency, isThorough);
        }
        m_Dependencies.Remove(abName);
    }

    void UnloadAssetBundleInternal(string abName, bool isThorough)
    {
        AssetBundleInfo bundle = GetLoadedAssetBundle(abName);
        if (bundle == null) return;

        if (--bundle.m_ReferencedCount <= 0)
        {
            if (m_LoadRequests.ContainsKey(abName))
            {
                return;     //如果当前AB处于Async Loading过程中，卸载会崩溃，只减去引用计数即可
            }
            bundle.m_AssetBundle.Unload(isThorough);
            m_LoadedAssetBundles.Remove(abName);
            Debug.Log(abName + " has been unloaded successfully");
        }
    }


    string GetRealAssetPath(string abName)
    {
        //TODO:实现

        if (abName.Equals(""))
        {
            return abName;
        }
        abName = abName.ToLower();
        if (!abName.EndsWith(GameConfig.STR_ASB_EXT))
        {
            abName += GameConfig.STR_ASB_EXT;
        }
        if (abName.Contains("/"))
        {
            return abName;
        }
        //string[] paths = m_AssetBundleManifest.GetAllAssetBundles();  产生GC，需要缓存结果
        if (null != mAllManifest)
        {
            for (int i = 0; i < mAllManifest.Length; i++)
            {
                int index = mAllManifest[i].LastIndexOf('/');
                string path = mAllManifest[i].Remove(0, index + 1);    //字符串操作函数都会产生GC
                if (path.Equals(abName))
                {
                    return mAllManifest[i];
                }
            }
        }
        //Debug.LogError("GetRealAssetPath Error:>>" + abName);
        return string.Empty;
    }

    /// <summary>
    /// 异步读取原始资源，只能在Editor模式 的情况下使用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path">资源相对于Assets/BundleRes的路径</param>
    /// <param name="assetNames">资源相对于path的路径，若资源在path下，则是文件名</param>
    /// <param name="action">delegate</param>
    private void loadRes<T>(string path, string[] assetNames, Action<UObj[]> action = null, LuaFunction luaFunc = null) where T : UObj
    {
        List<string> names = new List<string>();
        foreach (var name in assetNames)
        {
            names.Add(Tools.PathCombine("Assets/" + GameConfig.STR_RES_FOLDER, path, name));
            //Debug.Log(names[names.Count - 1]);
        }
        StartCoroutine(onLoadRes<T>(names.ToArray(), action, luaFunc));
    }

    /// <summary>
    /// 只能在编辑器模式下使用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assetNames"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator onLoadRes<T>(string[] assetNames, Action<UObj[]> action = null, LuaFunction luaFunc = null) where T : UObj
    {
        List<T> list = new List<T>();
#if UNITY_EDITOR
        foreach (var name in assetNames)
        {
            Debug.Log("load res:"+name);
            T t = AssetDatabase.LoadAssetAtPath<T>(name);
            list.Add(t);
            //yield return null;
        }
#endif
        if (null != action)
        {
            action(list.ToArray());
        }
        if (luaFunc != null)
        {
            luaFunc.Call((object)list.ToArray());
            luaFunc.Dispose();
            //luaFunc = null;
        }
        yield return null;
    }

    /// <summary>
    /// 从Assetbundle或者原始资源中加载一个资源，编辑器和正式游戏均使用本函数加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="abName"></param>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void LoadRes<T>(string abName, string name, Action<UObj> action = null, LuaFunction luaFunc = null) where T : UObj
    {
//#if UNITY_EDITOR
        if (!GameConfig.Instance.useAsb)
        {
            loadRes<T>(
                abName
                , new string[] { name, }
                , delegate(UObj[] objs)
                {
                    if (null != action && objs.Length == 1)
                    {
                        action(objs[0]);
                    }
                }
                , luaFunc
            );
            return;
        }
//#endif
        {
            //abName = Tools.GetAsbName(abName);
            //LoadAsset<T>(
            //    abName
            //    , new string[] { name, }
            //    , delegate(UObj[] objs)
            //    {
            //        if (null != action && objs.Length == 1)
            //        {
            //            action(objs[0]);
            //        }
            //    }
            //    , luaFunc
            //);
        }
    }

    /// <summary>
    /// 从Assetbundle或者原始资源中加载多个资源，编辑器和正式游戏均使用本函数加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="abName"></param>
    /// <param name="names">文件名需带后缀</param>
    /// <param name="action"></param>
    public void LoadRes<T>(string abName, string[] names, Action<UObj[]> action = null, LuaFunction luaFunc = null) where T : UObj
    {
//#if UNITY_EDITOR
        if (!GameConfig.Instance.useAsb)
        {
            loadRes<T>(abName, names, action, luaFunc);
            return;
        }
//#endif
        {
            //abName = Tools.GetAsbName(abName);
            //LoadAsset<T>(abName, names, action, luaFunc);
        }
    }

//    /// <summary>
//    /// 同步载入资源
//    /// </summary>

//    //TODO:dekiven
//    public UObj[] LoadResSync<T>(string abName, string[] names) where T : UObj
//    {
//        List<UObj> list = new List<UObj>();
//#if UNITY_EDITOR
//        if (!GameConfig.Instance.useAsb)
//        {
//            T t = AssetDatabase.LoadAssetAtPath<T>(name);
//            list.Add(t);
//        }else
//#endif
//        {
//            AssetBundle ab = AssetBundle.LoadFromFile(abName);
//        }
//        return list.ToArray();
//    }

    //-----------------------方便lua使用的函数 begin--------------------------
    public void LoadGameObj(string abName, string name, LuaFunction luaFunc)
    {
        LoadRes<GameObject>(abName, name, null, luaFunc);
    }

    public void LoadGameObj(string abName, string[] names, LuaFunction luaFunc)
    {
        LoadRes<GameObject>(abName, names, null, luaFunc);
    }

    public void LoadTextAsset(string abName, string name, LuaFunction luaFunc)
    {
        LoadRes<TextAsset>(abName, name, null, luaFunc);
    }

    public void LoadTextAsset(string abName, string[] names, LuaFunction luaFunc)
    {
        LoadRes<TextAsset>(abName, names, null, luaFunc);
    }

    public void LoadTextAssetBytes(string abName, string name, LuaFunction luaFunc)
    {
        LoadTextAssetBytes(abName, new string[] { name, }, luaFunc);
    }

    public void LoadTextAssetBytes(string abName, string[] names, LuaFunction luaFunc)
    {
        LoadRes<TextAsset>(abName, names, delegate(UObj[] objs)
        {
            List<LuaByteBuffer> list = new List<LuaByteBuffer>();
            if (objs.Length > 0)
            {
                foreach (var obj in objs)
                {
                    TextAsset text = obj as TextAsset;
                    if (null != text)
                    {
                        LuaByteBuffer buffer = new LuaByteBuffer(text.bytes);
                        list.Add(buffer);
                    }else
                    {
                        list.Add(null);
                    }                    
                }                
            }
            luaFunc.Call(list.ToArray());
            luaFunc.Dispose();
        });
    }
    //==============================方便lua使用的函数 end=========================

    //------------------------------辅助类 begin ------------------------------------
    public class AssetBundleInfo
    {
        public AssetBundle m_AssetBundle;
        public int m_ReferencedCount;

        public AssetBundleInfo(AssetBundle assetBundle)
        {
            m_AssetBundle = assetBundle;
            m_ReferencedCount = 0;
        }
    }

    class LoadAssetRequest
    {
        public Type assetType;
        public string[] assetNames;
        public LuaFunction luaFunc;
        public Action<UObj[]> sharpFunc;
    }
    //===============================辅助类 end ======================================

    //public LuaByteBuffer buffer { get; set; }

    /// <summary>
    /// 游戏退出检测，GameResManager一定会初始化，且不会被Distroy，所以通过他来监听游戏退出
    /// </summary>
    void OnApplicationQuit()
    {
        LogFile.CloseLog();
    }
}