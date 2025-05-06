using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;

public enum JsonType
{
    JsonUtility,
    LitJson
}
public class JsonMgr 
{
    static JsonMgr()
    {
        Debug.Log("JsonMgr单例模式创建成功");
    }
    private static JsonMgr _instance;
    public static JsonMgr Instance =>Instance;    public static void SaveDate<T>(T obj,string fileName,JsonType type = JsonType.LitJson)  
    {
        string path = Application.persistentDataPath + "/JsonData";
        string strJson;
        if (Directory.Exists(path))
        {
            if(type == JsonType.LitJson)
            {
                Debug.Log("LitJson序列化");
                strJson = JsonMapper.ToJson(obj);   
                File.WriteAllText(path + "/" + fileName, strJson); 
            }
            else if(type == JsonType.JsonUtility)
            {
                Debug.Log("JsonUtility序列化");
                strJson = JsonUtility.ToJson(obj);
                File.WriteAllText(path + "/" + fileName, strJson);
            }
        }
    }

    public static T LoadData<T>(string fileName,JsonType type = JsonType.LitJson)
    {
        string path = Application.streamingAssetsPath + "/JsonData/"+fileName + ".json";
        Debug.Log(path);
        if(!File.Exists(path))
        path = Application.persistentDataPath + "/JsonData/" + fileName + ".json";
        if(!File.Exists(path))
        {
            Debug.Log(path);
            Debug.Log("文件不存在");
            return default(T);
        }
        T obj = default(T);
        string strjson  = File.ReadAllText(path);
        
            if(type == JsonType.LitJson)
            {
                Debug.Log("LitJson反序列化");
                obj = JsonMapper.ToObject<T>(strjson);
            }
            else if(type == JsonType.JsonUtility)
            {
                Debug.Log("JsonUtility反序列化");
                obj =  JsonUtility.FromJson<T>(strjson);
            }
        return  obj;
    } 

}
