using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// 使用该工具  要使用自己封装的字典(实现了IXmlSerializable 接口) 不能处理原有的字典
/// </summary>
public class XMLDataMgr 
{
   private static XMLDataMgr _instance = new XMLDataMgr();
   public static XMLDataMgr Instance => _instance;

   private XMLDataMgr()
   {
   }


    /// <summary>
    /// 保存文件到xml
    /// 
    /// </summary>
    /// <param name="data">保存的数据</param>
    /// <param name="name">文件名</param>
   public void SaveData(object data,string name)
   {
        //1.得到路径
        string path = Application.persistentDataPath + "/" + name + ".xml";

        //2.存储文件
        using(StreamWriter sw = new StreamWriter(path))
        {
            //3.序列化
            XmlSerializer s = new XmlSerializer(data.GetType());
            s.Serialize(sw,data);
        }
   } 

    public object LoadData(Type type ,string name)
    {
        //1.首先判断文件是否存在
        string path = Application.persistentDataPath + "/" + name + ".xml";
        if(!File.Exists(path))
        path = Application.streamingAssetsPath + "/" + name + ".xml";  //处理不存在文件  换个路径读取
        // if(File.Exists(path))
        {
            //2.读取文件
            using(StreamReader sr = new StreamReader(path))
            {
                //3.反序列化
                XmlSerializer s = new XmlSerializer(type);
                return s.Deserialize(sr);
            }
        }
    }
}
