# PlayerPrefs 相关API

```c#
PlayerPrefs.SetInt("score", 100);  
PlayerPrefs.SetFloat("money", 100.0f);
PlayerPrefs.SetString("name", "张三");
//具有局限性 只能存储3种类型数据
//如果你想要存储别的类型的数据 只能降低精度 或者上升精度来进行存储
//同一键名存入数据将会覆盖  (存入相同类型和不同类型都会覆盖)
```

# PlayerPrefs 存储数据

**存在的问题：**

直接调用Set相关方法 只会把数据存储到内存中

当游戏结束时 unity会自动把内存中的数据存储到硬盘中

如果游戏异常结束 数据不会存储到硬盘中

 **解决方法：**

调用Save方法就会立即存储到硬盘

PlayerPrefs.Save();

# PlayerPrefs 读取数据

注意 运行时 只要你set了对应键值对

即使你没有马上存储save在本地

也能够读取出信息     会读取内存和硬盘两个地方的数据

**判断是否存在**

```C#
if(PlayerPrefs.HasKey("score"))
{
    Debug.Log("存在score");     //可以防止使用存在的键值进行覆盖 丢失数据
}
```

# PlayerPrefs删除数据

```C#
//删除指定键值对 
PlayerPrefs.DeleteKey("score");
//删除所有键值对
PlayerPrefs.DeleteAll();

//也存在set的问题  异常退出时 硬盘数据不会删除
//可以手动调用save方法进行存储
```

# PlayerPrefs数据存储的位置

不同平台不同

**Windows**

PlayerPrefs 存储在

HKCU\software\[公司名称]\[产品名称]项下的注册表中    ----注册表中的路径

其中公司和产品名称是 在“Project settings”中设置的名称

# 封装PlayerPrefs实现对其他数据的存储

[PlayerprefsDateMgr](./Code/PlayerPrefsDataMgr.cs)