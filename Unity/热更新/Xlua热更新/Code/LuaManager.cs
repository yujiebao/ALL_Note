using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;
/// <summary>
/// Lua管理器
/// 提供Lua解析器
/// 保证解析器的唯一性
/// </summary> <summary>
/// 
/// </summary>
public class LuaManager :BaseManager<LuaManager>
{
    //执行Lua语言的函数
    //释放垃圾
    //销毁
    //重定向
    private LuaEnv luaEnv;
    /// <summary>
    /// 得到lua的_G
    /// </summary>
    public LuaTable Global{
        get
        {
            if(luaEnv == null)
            {
                Debug.LogError("luaEnv is null");
                return null;
            }
            return luaEnv.Global;
        }
    }
    public void Init()
    {
        if (luaEnv == null)
        {
            luaEnv = new LuaEnv();
        }

        //加载Lua脚本重定向
        luaEnv.AddLoader(MyCustomLoader);
        luaEnv.AddLoader(MyCustomABLoader);
    }

 
    /// <summary>
    /// 调用Lua语言
    /// </summary>
    /// <param name="str"></param>
    public void DoString(string str)
    {
        if(luaEnv == null)
        {
            Debug.LogError("luaEnv is null");
            return;
        }
        luaEnv.DoString(str);
    }

    public void DoFile(string filename)
    {
        string str = "require ('"+filename+"')";
        DoString(str);
    }

    /// <summary>
    /// 释放Lua垃圾
    /// </summary> <summary>
    /// 
    /// </summary>
    public void Tick()
    {
        luaEnv.Tick();
    }

    /// <summary>
    /// 销毁Lua解析器
    /// </summary>
    public void Dispose()
    {
        luaEnv.Dispose();
        luaEnv = null;
    }

    private byte[] MyCustomLoader(ref string name)  //自动传入require中的name
    {
        //传入的参数 是 require执行的lua脚本文件名
        //拼接一个Lua文件所在路径
        string path = Application.dataPath + "/Lua/" + name+".lua";  //不需要更改后缀为.txt了 已经通过File读取了字节码
        
        //有路径 就去加载文件
        //File知识点 c#提供的文件读写的类
        if(File.Exists(path))
        {
            return File.ReadAllBytes(path);
        }
        else
        {
            Debug.Log("文件不存在,文件名为:"+ name);
        }
        return null;
    }

    //Lua脚本会放在AB包中
    //最终我们会通过AB包再加载其中的Lua脚本
    //AB包中如果要加载本文 后缀还是有一定的限制 .lua不能被识别
    //打包时 要把lua文件后缀改为txt

    //加载AB包中的Lua脚本
    //最终我们会通过加载AB包再加载其中的Lua脚本资源 来执行它
    //重定向加载AB包中的Lua脚本
    private byte[] MyCustomABLoader(ref string name)  
    {
        // //加载AB包  
        // string path = Application.streamingAssetsPath + "/lua";
        // AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
        
        // //加载AB包中的Lua脚本
        // TextAsset textAsset = assetBundle.LoadAsset<TextAsset>(name+".lua");
        // //加载lua中的byte数组
        // return textAsset.bytes;

        //使用ABMgr
        TextAsset lua = ABMgr.GetInstance().LoadRes<TextAsset>("lua","test.lua");   //lua得到就要用 不要使用异步
        if(lua == null)
        {
            Debug.LogError("lua is null");
            return null;
        }
        return lua.bytes;
    }
}
