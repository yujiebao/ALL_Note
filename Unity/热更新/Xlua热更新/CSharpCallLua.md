# Lua解析器 

```C#

XLua提供的一个重定向的方法
        允许我们自定义 加载 Lua文件的规则
        当我们执行Lua语言 require 时 相当于执行一个lua脚本
        它就会 执行 我们自定义传入的这个函数//XLua解析器  能够让我们在Unity中执行XLua
LuaEnv env = new LuaEnv();

//执行Lua语言
env.DoString("print('Hello World')","错误了");//第二个参数--执行出错时打印
env.DoString("require('test')");

//帮助我们清除Lua中我们没有手动释放的对象 垃圾回收
//帧更新中定时执行 或者 切场景时执行
// env.Tick();

//销毁LuaEnv
env.Dispose();

```

# 文件加载重定向

**1.Lua中文件重定向**

XLua提供的一个重定向的方法
允许我们自定义 加载 Lua文件的规则
当我们执行Lua语言 require 时 相当于执行一个lua脚本
它就会 执行 我们自定义传入的这个函数

```C#
LuaEnv env = new LuaEnv();
env.AddLoader(MyCustomLoader);
env.DoString("require('test')");


//有自定义Loader优先执行自定义的Loader
private byte[] MyCustomLoader(ref string name)  //自动传入require中的name
{
    //传入的参数 是 require执行的lua脚本文件名
    //拼接一个Lua文件所在路径
    string path = Application.dataPath + "/Lua/" + name+".lua";  //不需要更改后缀为.txt了 已经通过File读取了字节码
    print("path="+path);
    
    //有路径 就去加载文件
    //File知识点 c#提供的文件读写的类
    if(File.Exists(path))
    {
        return File.ReadAllBytes(path);
    }
    else
    {
        print("文件不存在,文件名为:"+ name);
    }
    return null;
}
```

**补充:**

1.默认的从Resources加载Lua文件的缺点:
(1)打包后不能修改
(2)后缀为.txt  每次都要手动修改 麻烦

# LuaMgr

[LuaMgr](./Code/LuaManager.cs)

# C#读写变量

通过G表来读写变量

但是注意:通过C#不能直接获取Lua中的Local变量 

```C#
LuaManager luaMgr = LuaManager.GetInstance();
luaMgr.Init();
luaMgr.DoFile("Main");
//使用lua解析器luaenv中的Global属性
//读
int i = luaMgr.Global.Get<int>("testNumber");  //值拷贝，不会影响lua中的值  

//写
luaMgr.Global.Set("testNumber", 100);       
```

#   C#调用Lua函数

也是通过G表来获取函数，但注意:

==使用委托去接收==Lua中的函数(对应参数形式一致)

为委托==加上CSharpCallLua特性==

```C#
[CSharpCallLua]
delegate void LuaFunction1();//无参无返回
//该特性是在Lua的命名空间  需要再XLua组件中生成代码
[CSharpCallLua]
delegate int LuaFunction2(int a );  //有参有返回 
[CSharpCallLua]
public delegate int LuaFunction3(int a ,out int b,out int c,out int d);
[CSharpCallLua]
public delegate int LuaFunction3_1(int a , ref int b,ref int c,ref int d);
[CSharpCallLua]delegate void LuaFunction4(params int[] arr);  //变长参数的类型是根据实际情况来定的 
```

**对应不同形式的Lua函数：**

(1)接收无参无返回

Lua函数

```Lua
testFunc = function()
    print("testFunc no in no out 无参无返回")
end
```

C#代码

```C#
[CSharpCallLua]
delegate void LuaFunction1();//无参无返回

//无参无返回
//委托
LuaFunction1 call = luaManager.Global.Get<LuaFunction1>("testFunc");
call();
UnityAction call_1 = luaManager.Global.Get<UnityAction>("testFunc");  //Unity定义的委托
call_1();
Action call_2 = luaManager.Global.Get<Action>("testFunc");   //C#定义的委托
call_2();
//Lua提供的一种获取函数的方式 少用
LuaFunction call_3 = luaManager.Global.Get<LuaFunction>("testFunc");
```

(2)有参有返回 

Lua函数

```Lua
testFunc2 = function(a)
    print("testFunc2 in out 有参有有返回")
    return a+1
end
```

C#代码

```C#
[CSharpCallLua]
delegate int LuaFunction2(int a );  //有参有返回

//有参有返回
LuaFunction2 call2 = luaManager.Global.Get<LuaFunction2>("testFunc2");
Debug.Log(call2(1));
Func<int,int> call2_1 = luaManager.Global.Get<Func<int,int>>("testFunc2");  //C#
Debug.Log(call2_1(1));
//XLua提供
LuaFunction call2_2 = luaManager.Global.Get<LuaFunction>("testFunc2");
Debug.Log(call2_2.Call(1)[0]);   //返回值是数字 只有一个返回值 所以应该是0

```

(3)多返回值           --------使用out和ref来接收

Lua函数:

```Lua
--多返回
testFunc3 = function (a)
    print("testFunc3 multiply out 多返回")
    return a*a,1,2,3
end
```

C#代码:

```C#
[CSharpCallLua]
public delegate int LuaFunction3(int a ,out int b,out int c,out int d);   //使用out
[CSharpCallLua]
public delegate int LuaFunction3_1(int a , ref int b,ref int c,ref int d);//使用ref

LuaFunction3 call3 = luaManager.Global.Get<LuaFunction3>("testFunc3");
int a,b,c;
Debug.Log(call3(1,out a,out b,out c));
print(a+" "+b+" "+c);
LuaFunction3_1 call3_1 = luaManager.Global.Get<LuaFunction3_1>("testFunc3");
int a1= 1;
int b1 = 2;
int c1 = 3;
Debug.Log(call3_1(1,ref a1,ref b1,ref c1));
Debug.Log(a1+" "+b1+" "+c1);
//XLua提供
LuaFunction call3_2 = luaManager.Global.Get<LuaFunction>("testFunc3");
object[] objs = call3_2.Call(1,2,3,4);
for(int i = 0;i<objs.Length;i++)
{
    Debug.Log(objs[i]);
}
```

(4)变长参数

Lua函数:

```Lua
testFunc4 = function(a,...)   
    --在unity中定义委托  第一个与后面变长类型一致时，不要分开写 直接一个params就行
    print("testFunc4 in out 变长参数")
    print(a)
    args = {...}
    for key, value in pairs(args) do
        print(key,value)
    end
end
```

C#代码:

```C#
[CSharpCallLua]
delegate void LuaFunction4(params int[] arr);  //变长参数的类型是根据实际情况来定的  

//变长参数
LuaFunction4 call4 = luaManager.Global.Get<LuaFunction4>("testFunc4");
call4(1,2,3,4,5,6,7,8,9,10);
//XLua  引起更大销毁  unity不推荐
LuaFunction call4_1 = luaManager.Global.Get<LuaFunction>("testFunc4");
call4_1.Call("test",1,2,3,4,5,6,7,8,9,10);
```

# C#使用Lua的List和Dictionary

**List**

Lua定义:

```Lua
testList = {1,2,3,4,5}
testList2 = {1,"123",3,4,5}
```

C#代码:

```C#
//浅拷贝
//同一类型 List
List<int> list = luaMgr.Global.Get<List<int>>("testList");
foreach (var item in list)
{
    Debug.Log("testList:" + item);
}
//不同类型
List<object> list2 = luaMgr.Global.Get<List<object>>("testList2");
foreach (var item in list2)
{
    Debug.Log("testList2:" + item);
}
```

**Dictionary**

Lua定义:

```lua
testDic = {
    ["1"] = 1,
    ["2"] = 2,
    ["3"] = 3,
    ["4"] = 4
}

testDic2 = {
    ["1"] = 1,
    [true] = 1,
    [false] = true,
    ['123'] = false
}

```

C#代码:

```C#
//浅拷贝
//统一类型
Dictionary<string,int> dic = luaMgr.Global.Get<Dictionary<string,int>>("testDic");
foreach (var item in dic)   //遍历出来顺序有点不对
{
    Debug.Log("testDic:" + item.Key + ":" + item.Value);  
}
//不统一类型
Dictionary<object,object> dic2 = luaMgr.Global.Get<Dictionary<object,object>>("testDic2");
foreach (var item in dic2)   //遍历出来顺序有点不对
{
    Debug.Log("testDic:" + item.Key + ":" + item.Value);  
}
```

**注意:**

对于List和Dictionary Lua中都是用表存储  存在一个问题:类型不保持一致  解决方法:使用万物之父(object)来读取

# C#读取Lua的类

Lua类定义:

```c#
testClass = 
{
    testInt = 2,
    testBool = true,
    testString = "test",
    testFloat = 1.0,
    -- testList = {1,2,3,4,5},
    testfunc = function ()
        print("testClass.testfunc")
    end,
    -- testinClass =
    -- {
    --     testinInt = 5
    -- }
}
```

**1.使用类来读取类**

C#代码：

```C#
public class CallLuaClass
{
    //这个自定义中的变量可以更多或者更少  少了忽略 多了也不会赋值

    //在这个类中去声明成员变量
    //名字一定要和 Lua那边的一样
    //公共 私有和保护没办法赋值
    public int testInt;
    public bool testBool ;
    public string testString ;
    public float testFloat ;
    public CallLuaInClass testinClass;
    public UnityAction testfunc;
}

LuaManager LuaMgr =  LuaManager.GetInstance();
LuaMgr.Init();
LuaMgr.DoFile("Main");
CallLuaClass obj = LuaMgr.Global.Get<CallLuaClass>("testClass");  //值拷贝  
Debug.Log("testInt:" + obj.testInt);
Debug.Log("testBool:" + obj.testBool);
Debug.Log("testString:" + obj.testString);
Debug.Log("testFloat:" + obj.testFloat);
Debug.Log("testinInt:" + obj.testinClass.testinInt);
obj.testfunc();
```

**2.使用接口来读取类**

```C#
//接口中不允许有成员变量
//我们用属性来接收类   
//接口拷贝是引用拷贝  接口数据改变lua的数据也会改变
[CSharpCallLua]
public interface CallInterface
{
    int testInt {get;set;}    //使用属性来接收Lua 中的变量
    bool testBool {get;set;}
    string testString {get;set;}
    float testFloat {get;set;}
    UnityAction testfunc {get;set;}
}

LuaManager LuaMgr =  LuaManager.GetInstance();
LuaMgr.Init();
LuaMgr.DoFile("Main");

CallInterface obj = LuaMgr.Global.Get<CallInterface>("testClass");
Debug.Log("testInt:" + obj.testInt);
Debug.Log("testBool:" + obj.testBool);
Debug.Log("testString:" + obj.testString);
Debug.Log("testFloat:" + obj.testFloat);
obj.testfunc();

```

**3.使用XLua提供的LuaTable来获取类**

```C#
LuaManager luaMgr = LuaManager.GetInstance();
luaMgr.Init();
luaMgr.DoFile("Main");
//不建议使用LuaTable和LuaFunction 效率低
//引用设置
LuaTable luaTable = luaMgr.Global.Get<LuaTable>("testClass");  
Debug.Log("testInt:" + luaTable.Get<int>("testInt"));
Debug.Log("testBool:" + luaTable.Get<bool>("testBool"));
Debug.Log("testString:" + luaTable.Get<string>("testString"));
Debug.Log("testFloat:" + luaTable.Get<float>("testFloat"));
luaTable.Get<LuaFunction>("testfunc").Call();
// luaTable.Set("testString", "testString");    --引用设置
luaTable.Dispose();   //记住需要释放   可能造成内存泄露
```





