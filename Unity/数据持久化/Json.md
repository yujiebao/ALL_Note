# JsonUtlity

## JsonUtlity序列化

**JsonUtlity是什么?**

 Jsonutlity 是Unity自带的用于解析Json的公共类

它可以

1.将内存中对象序列化为Json格式的字符串

2.将Json字符串反序列化为类对象

**使用JsonUtility进行序列化**

```C#
//方法:
// JsonUtility.ToJson(需要序列化的对象)
Test test = new Test
("张三", 18, 1.8, true, 
new int[] { 1, 2, 3 }, new List<int>() { 1, 2, 3 }, 
new Dictionary<string, int>() { { "a", 1 }, { "b", 2 } }, 
new Dictionary<string, string>() { { "a", "1" }, { "b", "2" } }, 
new Student("张三",18), 
new List<Student>{new Student("ss",4)}, 0, 0);

//JsonUtility提供了现成的方法 可以把类对象 序列化为 json字符串
string str = JsonUtility.ToJson(test);

File.WriteAllText(Application.persistentDataPath + "/Test.json", str);
```

**注意:**

1.float序列化时看起来会有一些误差

2.自定义类需要加上序列化特性[system.serializable]

3.想要序列化私有变量 需要加上特性[serializeField]

4.JsonUtility不支持字典

5.Jsonutlity存储null对象不会是null 而是默认值的数据

##  JsonUtlity反序列化

**使用JsonUtility反序列化**

```C#
string path = Application.persistentDataPath + "/Test.json";
string jsonData = File.ReadAllText(path);
// Test t1 = JsonUtility.FromJson(jsonData, typeof(Test)) as Test;
Test t1 = JsonUtility.FromJson<Test>(jsonData);
```

**注意**

1.JsonUtility无法直接读取数据集合(数组)     通过包裹在一个类中来实现读取

2.文本编码格式需要使用UTF-8格式，否者无法加载



# LitJson

**LitJson是什么**

它是一个第三方库，用于处理json的序列化和反序列化

LitJson是c#编写的，体积小、速度快、易于使用

它可以很容易的嵌入到我们的代码中

只需要将LitJson代码拷贝到工程中即可

## LitJson序列化

```C#
//方法：
// JsonMapper.ToJson(对象)
Test2 test = new Test2
("张三", 18, 1.8, true, 
new int[] { 1, 2, 3 }, new List<int>() { 1, 2, 3 }, 
new Dictionary<string, string>() { { "a", "1" }, { "b", "2" } }, 
null, 
new Student2("张三",18), 
new List<Student2>{new Student2("ss",4)}, 5, 0);

string jsonStr = JsonMapper.ToJson(test);
File.WriteAllText(Application.persistentDataPath + "/Lesson3.json", jsonStr); 
print(Application.persistentDataPath); 


```

**注意:**
1.相对JsonUtlity不需要加特性
2.不能序列化私有变量
3.支持字典类型,字典的键 建议都是字符串 因为 Json的特点 Json中的键会加上双引
4.需要引用LitJson命名空间
5.LitJson可以准确保存null类型

## LitJson反序列化 

```C#
//方法：
//1.JsonMapper.ToObject(json字符串)
jsonStr = File.ReadAllText(Application.persistentDataPath + "/Lesson3.json");
// JsonData 是LitJson提供的一个类对象 可以用键值对的形式去访问其中数据
JsonData data = JsonMapper.ToObject(jsonStr);    //返回一个JsonData对象
print(data["name"]);
print(data["age"]);

//2.通过泛型转换
Test2 t2 = JsonMapper.ToObject<Test2>(jsonStr);
print(t2);
t2.show();

```

**注意 :**
1.类结构需要无参构造函数，否则反序列化时报错
2.字典虽然支持 但是键在使用为数值时会有问题 需要使用字符串类型

## 注意:

1**.LitJson可以直接读取数据集合(数组)** 

 ```C#
jsonStr = File.ReadAllText(Application.persistentDataPath + "/Array.json");

List<RoleData> roleDataList = JsonMapper.ToObject<List<RoleData>>(jsonStr);

foreach (var item in roleDataList)

{

    print(item.hp);

}

jsonStr = File.ReadAllText(Application.persistentDataPath + "/Dic.json");

Dictionary<string, int> roleDataDic = JsonMapper.ToObject<Dictionary<string, int>>(jsonStr);

foreach (var item in roleDataDic)

{

    print(item.Key);

    print(item.Value);

}

 ```

2.**文件编码格式是UTF-8格式，否者无法加载**

## 总结:

1.LitJson提供的序列化反序列化方法 JsonMapper.ToJson和ToObject
2.LitJson无需加特性
3.LitJson不支持私有变量
4.LitJson支持字典序列化反序列化
5.LitJson可以直接将数据反序列化为数据集合
6.LitJson反序列化时 自定义类型需要无参构造
7.Json文档编码格式必须是UTF-8

# JsonUtility和LitJson的异同 

**相同点:**

1.他们都是用于Json的序列化反序列化
2.Json文档编码格式必须是UTF-8
3.都是通过静态类进行方法调用

**不同点:**

1.Jsonutlity是Unity自带，LitJson是第三方需要引用命名空间
2.Jsonutlity使用时自定义类需要加特性，Litison不需要
3.Jsonutlity支持私有变量(加特性)，LitIson不支持
4.Jsonutlity不支持字典,LitJson支持(但是健只能是字符串)
5.Jsonutlity不能直接将数据反序列化为数据集合(数组字典)，LitJson可以
6.Jsonutlity对自定义类不要求有无参构造，LitJson需要
7.Jsonutlity存储空对象时会存储默认值而不是null，LitJson会存null

# JsonMgr：

[JsonMgr](./Code/JsonMgr.cs)

# 补充知识：

**在文件中存读字符串**

```C#
必备知识点————在文件中存读字符串
        //1.存储字符串到指定路径文件中
        //第一个参数 填写的是 存储的路径   确定使用的文件夹是存在的 否则报错
        //第二个参数 填写的是 存储的字符串内容
        File.WriteAllText(Application.persistentDataPath + "/Test.txt", "Hello World");
        print(Application.persistentDataPath);
        
        //2.在指定路径文件中读取字符串
        print(File.ReadAllText(Application.persistentDataPath + "/Test.txt"));
```

