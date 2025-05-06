public class TestLesson3:IXmlSerializable
{
    public int test1;
    public string test2;

    //返回结构    
    public XmlSchema GetSchema()
    {
        return null;
    }

    //反序列化时会自动调用的方法
    public void ReadXml(XmlReader reader)
    {
        //里面可以自定义反序列化 的规则
        //读属性
        // test1 = int.Parse(reader.GetAttribute("test1"));
        // test2 = reader.GetAttribute("test2");
        
        //读节点
        //方式一 
        // reader.Read();  //这时读到的是test1节点
        // reader.Read();  //这时读到的是test1节点包裹的内容
        // this.test1 = int.Parse(reader.Value);
        // reader.Read();  //这时读到的是test1尾部包裹节点
        // reader.Read();  //读到test2节点
        // reader.Read();  //读到test2节点包裹的内容
        // this.test2 = reader.Value;
        // //方式二
        // while(reader.Read())
        // {
        //     if(reader.NodeType == XmlNodeType.Element)
        //     {
        //        switch (reader.Name)
        //            {
        //                case "test1":
        //                    reader.Read();
                        //    this.test1 = (int)(reader.ReadElementContentAsString());
        //                    break;
        //                case "test2":
        //                    this.test2 = reader.Value;
        //                    break;
        //            }
        //     }
        // }
        //读包裹节点
        XmlSerializer s = new XmlSerializer(typeof(int));
        XmlSerializer s2 = new XmlSerializer(typeof(string));
        reader.Read();  //跳过根节点
        reader.ReadStartElement("test1");
        test1 = (int)s.Deserialize(reader);
        reader.ReadEndElement();
        reader.ReadStartElement("test2");
        test2 = (string)s2.Deserialize(reader);
        reader.ReadEndElement();
    } 

    //序列化时会自动调用的方法
    public void WriteXml(XmlWriter writer)
    {
        //里面可以自定义序列化 的规则

        //如果要自定义 序列化的规则 一定会用到XmlWriter中的一些方法 来进行序列化
        //写属性
        // writer.WriteAttributeString("test1",test1.ToString());
        // writer.WriteAttributeString("test2",test2);

        //写节点
        // writer.WriteElementString("test1",test1.ToString());
        // writer.WriteElementString("test2",test2);

        //写包裹节点
        XmlSerializer s = new XmlSerializer(typeof(int));
        writer.WriteStartElement("test1");
        s.Serialize(writer,test1);
        writer.WriteEndElement();
        XmlSerializer s2 = new XmlSerializer(typeof(string));
        writer.WriteStartElement("test2");
        s2.Serialize(writer,test2);
        writer.WriteEndElement();
    }

    //在序列化时 引用类型如果为空 不会序列化  xml中看不到该字段
    public override string ToString()
    {
        return $"{test1} {test2}";
    }
}