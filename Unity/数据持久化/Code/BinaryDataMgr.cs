using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// 二进制数据管理器
/// </summary> <summary>
/// 
/// </summary>
public class BinaryDataMgr 
{
    private static BinaryDataMgr instance = new BinaryDataMgr();
    private BinaryDataMgr(){}
    private static string SAVE_PATH = Application.persistentDataPath + "/BinaryData/"; //持久化数据路径
    public static BinaryDataMgr Instance
    {
        get { return instance; }
    }

    public void Save(object obj , string fileName)
    {
        using(FileStream fs = new FileStream(SAVE_PATH+fileName+ ".bao",FileMode.OpenOrCreate,FileAccess.Write))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, obj);   //序列化对象 生成2进制字节数组 写入到内存流中
            fs.Close();
        }
    } 

    /// <summary>
    /// 读取二进制数据转换为对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns> <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public T Load<T>(string fileName) where T : class
    {
        if(!File.Exists(SAVE_PATH + fileName + ".bao"))
        {
            Debug.LogError("文件不存在！");
            return default(T);
        }
        T obj ;
        using(FileStream fs = new FileStream(SAVE_PATH + fileName + ".bao",FileMode.Open,FileAccess.Read))
        {
            BinaryFormatter bf = new BinaryFormatter();
            obj = bf.Deserialize(fs) as T; //反序列化对象 读取2进制字节数组 写入到内存流中
            fs.Close();
        }
        return obj;
    }
}
