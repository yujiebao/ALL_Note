```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ABMgr : SingletoAutonMono<ABMgr>
{
    //AB包管理器  --让外部便捷的进行资源加载

    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();   //保证每个包只加载一次
    private AssetBundle mainAB = null;    //主包加载一次就行
    private AssetBundleManifest manifest = null;  //主包固定文件  也是加载一次就行
    /// <summary>
    /// AB包存放路径  方便修改
    /// </summary>
    /// <value></value>
    private string pathUrl
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }

    /// <summary>
    /// 主包名方便修改
    /// </summary> <summary>
    /// 
    /// </summary>
    /// <value></value>
    private string MainABName
    {
        get
        {
#if UNITY_IOS
        return "IOS";
#elif UNITY_ANDROID
        return "Android";
#elif UNITY_NOW
        return "StandaloneWindows";
#else
        print("else");
#endif
        }
    }
    public void LoadAB(string abName)
    {
        //加载AB包
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(pathUrl + MainABName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //获取依赖包相关信息
        AssetBundle ab;
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            //判断包是否加载过
            if (!abDic.ContainsKey(strs[i]))
            {
                 ab = AssetBundle.LoadFromFile(pathUrl + strs[i]);
                abDic.Add(strs[i], ab);
            }
        }
        //加载资源包
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(pathUrl + abName);
            abDic.Add(abName, ab);
        }
    }

    //同步加载  不指定类型
    public Object LoadRes(string abName, string resName)
    {
        //加载AB包
        LoadAB(abName);

        //加载资源
        Object obj = abDic[abName].LoadAsset(resName);
        if(obj is GameObject)   //特别优化了加载资源为GameObject的情况  直接实例化对象
            return Instantiate(obj);
        else
            return obj;
    }

    //同步加载  根据Type指定类型    lua热更新使用  lua没有泛型
    public Object LoadRes(string abName, string resName,System.Type type)
    {
        LoadAB(abName);
         //加载资源
        Object obj = abDic[abName].LoadAsset(resName,type);
        if(obj is GameObject)   //特别优化了加载资源为GameObject的情况  直接实例化对象
            return Instantiate(obj);
        else
            return obj;
    }

    //同步加载  根据泛型指定类型    
    public T LoadRes<T>(string abName, string resName) where T:Object
    {
        LoadAB(abName);
         //加载资源
        T obj = abDic[abName].LoadAsset<T>(resName);
        if(obj is GameObject)   //特别优化了加载资源为GameObject的情况  直接实例化对象
            return Instantiate(obj);
        else
            return obj;
    }

    //异步加载
    //这里异步加载 AB包并没有使用异步加载
    //只是从AB包中 加载资源时 使用异步
    public void LoadResAsync(string abName, string resName,UnityAction<Object> callback)
    {
        StartCoroutine(LoadResAsyncFunc(abName, resName, callback));
    }

    private IEnumerator LoadResAsyncFunc(string abName, string resName,UnityAction<Object> callback)
    {
        //加载AB包
        LoadAB(abName);

        //加载资源
        AssetBundleRequest obj = abDic[abName].LoadAssetAsync(resName);
        yield return obj;
        //异步加载结束后通过委托传递给外部
        if (obj.asset is GameObject)
            callback(Instantiate(obj.asset));
        else
            callback(obj.asset);
    }

    //异步加载  类型
    public void LoadResAsync(string abName, string resName,System.Type type,UnityAction<Object> callback)
    {
        StartCoroutine(LoadResAsyncFunc(abName, resName,type ,callback));
    }

    private IEnumerator LoadResAsyncFunc(string abName, string resName,System.Type type,UnityAction<Object> callback)
    {
        //加载AB包
        LoadAB(abName);

        //加载资源
        AssetBundleRequest obj = abDic[abName].LoadAssetAsync(resName,type);
        yield return obj;
        //异步加载结束后通过委托传递给外部
        if (obj.asset is GameObject)
            callback(Instantiate(obj.asset));
        else
            callback(obj.asset);
    }

    //异步加载 泛型
    public void LoadResAsync<T>(string abName, string resName,UnityAction<T> callback) where T:Object
    {
        StartCoroutine(LoadResAsyncFunc<T>(abName, resName, callback));
    }

    private IEnumerator LoadResAsyncFunc<T>(string abName, string resName,UnityAction<T> callback) where T:Object
    {
        //加载AB包
        LoadAB(abName);

        //加载资源
        AssetBundleRequest obj = abDic[abName].LoadAssetAsync<T>(resName);
        yield return obj;
        //异步加载结束后通过委托传递给外部
        if (obj.asset is GameObject)
            callback(Instantiate(obj.asset) as T);
        else
            callback(obj.asset as T);
    }

    //卸载单个包
    public void UnLoadAB(string abName)
    {
        if (abDic.ContainsKey(abName))
        {
            abDic[abName].Unload(false);
            abDic.Remove(abName);
        }
    }
    //卸载所有包
    public void UnLoadAllAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        mainAB = null;
        manifest = null;
    }
}

```

# 面试问题:

**AB包卸载资源时Unload(true)其中true和false代表什么?**

在Unity中，AssetBundle.Unload()方法用于卸载AssetBundle及其资源，其布尔参数决定了卸载行为：

使用AssetBundle.Unload(true)：不仅会卸载AssetBundle本身，还会强制卸载所有从这个AssetBundle加载出来的资源，即使这些资源当前正在被使用。这可能会导致依赖这些资源的对象出现问题。
使用AssetBundle.Unload(false)：仅卸载AssetBundle的容器，不影响那些已经加载并在使用的资源。如果需要再次访问AssetBundle中的资源，则必须重新加载整个AssetBundle。

