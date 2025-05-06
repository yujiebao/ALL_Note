# Lua如何在Unity中执行

Lua无法直接访问C# 一定是先从C#调用Lua脚本后
才把核心逻辑交给Lua脚本处理

```C#
public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LuaManager luaMgr = LuaManager.GetInstance();
        luaMgr.Init();
        luaMgr.DoFile("Main");   //Main.lua又作为Lua其他程序的主入口
    }

    //Update is called once per frame
    void Update()
    {
        
    }
}
```

# Lua调用C#的Class

**申明类**

lua中使用C#的类非常简单

固定套路

CS.命名空间.类名

Unity的类比如 GameObject Transform等等   ————CS.UnityEngine.类名

**实例化类**

通过C#中的类 实例化一个对象 Lua中没有new 所以我们直接 类名括号就是实例化对象
默认调用的 相当于就是无参构造

```Lua
--通过C#中的类 实例化一个对象 Lua中没有new 所以我们直接 类名括号就是实例化对象
--默认调用的 相当于就是无参构造
--local gameObject = CS.UnityEngine.GameObject()
local gameObject2 = CS.UnityEngine.GameObject("test")   --重载函数
```

**调用静态函数和变量**

```Lua
--为了方便使用 并且节约性能  定义全局变量存储 C#中的类
Gameobject = CS.UnityEngine.GameObject
--类中的静态对象 可以直接使用.来调用
local gameObject4 = Gameobject.Find("test")
```

**使用类中的变量和函数**

```Lua
--得到对象中的成员变量直接对象.即可
-- print(gameObject4.transform.position)     --Lua打印
CS.UnityEngine.Debug.Log(gameObject4.transform.position)   --C#打印

Vector3 = CS.UnityEngine.Vector3
--要使用对象中的成员方法  一定要使用":"  -----是对象调用该函数  而不是类  
gameObject4.transform:Translate(Vector3.right)

--自己定义的类
local t = CS.Test()
t:Speak("speak")
local t2 = CS.SelfSpace.Test2()
t:Speak("speak")
```

**注意:**

 ```Lua
--继承了Mono的类  --不能直接new
local gameObject5 = Gameobject("测试添加脚本")
--通过AddComponent添加脚本   Lua没有泛型
--XLua提供了一个重要方法 typeof 可以得到类的Type
--XLua中不支持无参泛型函数 所以我们要使用另一个重载
gameObject5:AddComponent(typeof(CS.LuaCallCSharp))
 ```

# Lua调用C#的枚举

```Lua
--调用Unity当中的枚举
--枚举的调用规则 和 类的调用规则是一样的
--CS.命名空间.枚举名.枚举成员
PrimitiveType = CS.UnityEngine.PrimitiveType
GameObject = CS.UnityEngine.GameObject
local obj = GameObject.CreatePrimitive(PrimitiveType.Cube)     -- 枚举矩形

--自定义枚举
--存储枚举类型
E_MyEnum = CS.E_MyEnum
local myEnum = E_MyEnum.Idle
print(myEnum)

--枚举转换相关
--数值转枚举      --->__CastFrom
local myEnum2 = E_MyEnum.__CastFrom(1)
print(myEnum2)
--字符串转枚举
local myEnum3 = E_MyEnum.__CastFrom("Die")
print(myEnum3)

```

# Lua调用C#的List和Dictionary 

```C#
public class Lesson3
{
    public int[] array = new int[5]{1,2,3,4,5};

    public List<int> list = new List<int>();
    public Dictionary<int,string> dict = new Dictionary<int, string>();
}
```

**数组**

1.操作原有的数组

```Lua
local obj = CS.Lesson3()

--Lua使用C#数组相关知识
--长度 userdata
--C#怎么用 lua就怎么用 不能使用#去获取长度
print(obj.array.Length)

--遍历要注意 虽然lua中索引从1开始
--但是数组是C#那边的规则 所以 还是得按c#的来
--最大值减1
for i=0,obj.array.Length-1 do
    print(obj.array[i])
end
```

2.创建一个数组

```Lua
--Lua中创建一个C#的数组  Lua中表示数组和List可以用表
local array2 = CS.System.Array.CreateInstance(typeof(CS.System.Int32), 10)
-- print(type(array2))   userdata类型
-- print(array2.Length)
-- for i=0,array2.Length-1 do
--     print(array2[i])
-- end
```

**链表**

类对象已经申明了

1.操作原有的链表

```Lua
obj.list:Add(3)
obj.list:Add(5)
obj.list:Add(7)
obj.list:Add(4)
print(obj.list.Count)
for i = 0,obj.list.Count-1 do
    print(obj.list[i])
end
```

2.创建一个链表

```Lua
--在Lua中创建一个C#的List对象
--老版本
-- local list2 = CS.System.Collections.Generic["List`1[System.String]"]()
-- list2:Add("123")
-- print(list2[0])
--新版本
list_String = CS.System.Collections.Generic.List(typeof(CS.System.String))   --指定类型
list2 = list_String()   --实例化
list2:Add("123")
print(list2[0])
```

**字典**

1.操作字典

```Lua
obj.dict:Add(1,"123")
print(obj.dict)
print(obj.dict[1])   --[key]

--遍历
for k,v in pairs (obj.dict) do
    print(k,v)
end

```

2.创建一个字典

```Lua
--在Lua中创建一个字典
local Dic_String_Vector3 = CS.System.Collections.Generic.Dictionary(typeof(CS.System.String),typeof(CS.UnityEngine.Vector3))
dic = Dic_String_Vector3()
dic:Add("123",CS.UnityEngine.Vector3(1,2,3))
dic:Add("124",CS.UnityEngine.Vector3(1,2,3))

-- print(dic["123"])  直接通过[]是nil
-- 如果要通过键获取值 要通过这个固定方法
print(dic:get_Item("123"))
-- print(dic:TryGetValue("123",CS.UnityEngine.Vector3()))   --Unity中的方法
--设置值
dic:set_Item("123",CS.UnityEngine.Vector3(2,3,4))
print(dic:get_Item("123"))

for k,v in pairs (dic) do
    print(k,v)
end
```

# Lua调用C#的函数

**一般函数和拓展函数**

C#代码

```C#
public class Lesson4
{
    public string name = "Lesson4";

    public void Speak()
    {
        Debug.Log("Speak:"+name);
    }
    public static void Eat()
    {
        Debug.Log("吃东西");

    }
}

//想要在Lua中使用拓展方法 一定要在工具类前面加上特定
//建议 Lua中要使用的类 都加上该特性 可以提升性能
//如果不加该特性 除了拓展方法对应的类 其它类的使用 都不会报错
//但是Lua是通过反射的机制去调用的c#类 效率较低
[XLua.LuaCallCSharp]
public static class Tool
{
    public static void Move(this Lesson4 lesson4)
    {
        Debug.Log(lesson4.name + "移动");
    }
}
```

Lua代码:

```Lua
print("************Lua调用C# 拓展方法相关知识点*************")
Lesson4 = CS.Lesson4
Lesson4.Eat();   --静态方法  属于类  用.就行  只有一个类

local obj = Lesson4()
--成员方法  一定要使用:  确定是哪个对象调用的
obj:Speak("你好")
--该方法为拓展方法
--需要再使用的C#脚本加上Lua调用C#的特性
obj:Move()   
```

**有ref和out**

C#代码:

```C#
public class Lesson5
{
    public int RefFun(int a ,ref int b,ref int c,int d)
    {
        b = a + d;
        c = a - d;
        return b*c;
    }

    public int OutFun(int a,out int b,out int c,int d)
    {
        b = a + d;
        c = a - d;
        return b*c;
    }

    public int RefOutFun(int a,ref int b,out int c,int d)
    {
        b = a + d;
        c = a - d;
        return b*c;
    }
}
```

Lua代码:

```Lua
Lesson5 = CS.Lesson5
local obj = Lesson5()

--ref参数 会以多返回值的形式返回给Lua
local a,b,c = obj:RefFun(1,2,3,4)   --传入的参数不够的时候 会自动将后面的补0   为保持一致性 一帮传入值占位
-- a——>返回值  b——>ref参数1  c——>ref参数
print(a,b,c)

--out参数  会以多返回值的形式返回给Lua
--如果函数存在返回值 那么第一个值 就是该返回值
--之后的返回值 就是out的结果 从左到右一一对应
--out参数 不需要传占位置的值
print("*********out*************")
local a,b,c = obj:OutFun(1,4)
print(a,b,c)

--out和ref都包含的参数
print("*********out和ref*************")
local a,b,c = obj:RefOutFun(1,2,4)
print(a,b,c)
--综合使用的时候各自遵循他们的规则 
--ref占位  out不传
```

**函数重载**

Lua不支持  但是Lua支持调用C#的重载函数

C#代码:

```Lua
public class Lesson6
{
    public int Calc()
    {
        return 1;
    }

    public int Calc(int a , int b)
    {
        return a + b;
    }

    public int Calc(int a)
    {
        return a;
    }

    public float Calc(float a , float b)
    {
        return a + b ;
    }
    
}
```

Lua代码:

```Lua
print("*************Lua调用C# 重载函数相关知识点**************")
Lesson6 = CS.Lesson6
local obj = Lesson6()

print(obj:Calc())
print(obj:Calc(1))
print(obj:Calc(1,2))
--Lua虽然支持调用C#重载函数
--但是因为Lua中的数值类型 只有Number  --->对C#中的多精度的重载函数 支持不好  傻傻分不清
--可能会引起问题  
print(obj:Calc(1.5,2.6))

--解决重载函数含糊的问题
--xlua提供了解决方案 反射机制
--这种方法只做了解 尽量别用  尽量不使用精度来进行重载(int float double)
--Type是反射的关键类
--得到指定函数的相关信息
local m1 =  typeof(CS.Lesson6):GetMethod("Calc",{typeof(CS.System.Single),typeof(CS.System.Single)})
local f1 = xlua.tofunction(m1)
print(f1(obj,1.5,2.6))

```

# Lua调用C#委托和事件

C#代码:

```C#
[XLua.LuaCallCSharp]
public class Lesson7
{
    public UnityAction del;
    // public UnityEvent eventAction;
    public event UnityAction eventAction;

    public void DoEvent()    //事件只能内部调用
    {
        eventAction?.Invoke();
    }

    public void ClearEvent()
    {
        eventAction = null;
    }
}
```

Lua代码:

```Lua
print("******************Lua调用 C#委托和事件相关******************")
Lesson7 = CS.Lesson7
local obj = Lesson7()

print("******************Lua调用 C#委托******************")
--委托就是用来装函数
--使用C#中的委托  就是用来装Lua函数的
local fun= function ()
    print("Lua Call delegate")
end

local fun2 = function ()
    print("Lua Call delegate2")
end

--Lua中没有复合运算符 不能+=
--如果第一次向委托中添加函数直接赋值 先让变量确定类型
obj.del = fun
--向del中以多播的形式再添加函数  不建议直接添加function(类似匿名函数)不好管理 移除不了
obj.del = obj.del + fun2
-- obj.del = obj.del + function ()
--     print("Lua Call delegate3 不建议使用")
-- end
obj.del()
--移除函数
print("移除函数")
obj.del = obj.del - fun2
-- obj.del = nil    全部清空
obj.del()

print("******************Lua调用 C#事件******************")
local fun3 = function ()
    print("Lua Call event")
end
--事件加减和委托非常不一样
obj:eventAction("+",fun)
obj:eventAction("+",fun2)
obj:eventAction("+",fun3)
obj:DoEvent()  --事件只能内部调用 调用类中包裹的方法
obj:eventAction("-",fun2)
obj:DoEvent()
--事件清除  不能直接nil
obj:ClearEvent()
obj:DoEvent()
```

# Lua调用C#的二维数组

C#代码:

```C#
public class Lesson8
{
    public int[,] array = new int[3,4]{
        {1,2,3,4},
        {5,6,7,8},
        {9,10,11,12}
    };
}
```

Lua代码:

```Lua
-- print("**************Lua调用C# 二维数组相关知识点*************")
Lesson8 = CS.Lesson8
local obj = Lesson8()
print("行："..obj.array:GetLength(0))
print("列："..obj.array:GetLength(1))

--获取元素
--不能通过[0,0]或者[0][0]访问元素  Lua只支持一位数组
for i = 0 , obj.array:GetLength(0)-1 do
    for j = 0 , obj.array:GetLength(1)-1 do
        print(obj.array:GetValue(i,j))
    end
end
```

# Lua调用C#  nil和null 的区别

C#代码:

```C#
[XLua.LuaCallCSharp]
public static class Lesson9
{
    public static bool IsNull(this UnityEngine.Object obj)
    {
        return obj == null;
    }
}
```

Lua代码:

```Lua
GameObject = CS.UnityEngine.GameObject
Rigidbody = CS.UnityEngine.Rigidbody

local obj = GameObject("测试添加脚本 --nil和null的检测")
local rig = obj:GetComponent(typeof(Rigidbody))
print(rig)
--判断是否为空
--nil和null不能 == 比较
--lua中定义全局的IsNull方法
-- if IsNull(rig) then   --定义了IsNull全局方法 包装了Equals方法
--     print("rig为空")
--     rig = obj:AddComponent(typeof(Rigidbody))
-- end
-- print(rig)

--C#中定义IsNull方法
if rig:IsNull() then
    print("rig为空")
    rig = obj:AddComponent(typeof(Rigidbody))
end
print(rig)
```

# 在C#中批量添加特性

```C#
public static class Lesson10
{
    [CSharpCallLua]   //批量添加特性
    public static List<Type> csharpCallLuaList = new List<Type>()
    {
         typeof(UnityAction<float>),
    };

    [LuaCallCSharp]
    public static List<Type> luaCallCSharpList = new List<Type>()
    {
        typeof(UnityAction<float>),
    };
}
```

# Lua调用C#协程

Lua代码:

```Lua
-- 一定需要执行了(require)了才能使用
util = require("xlua.util")
--C#中协程后动都是通过继承了Mono的类 通过里面的启动数startcoroutine
WaitForSeconds = CS.UnityEngine.WaitForSeconds
--在场景中新建空物体  挂载脚本
local obj = GameObject("Coroutine")
local mono = obj:AddComponent(typeof(CS.LuaCallCSharp))

--想要开启的协程函数
fun = function  ()
    local a = 1
    while true do
        coroutine.yield(WaitForSeconds(1))
        print("协程执行中"..a)
        a = a + 1
        if a >10 then
            mono:StopCoroutine(b)
        end
    end
end

--我们不能直接将 lua函数传到开始协程中
--如果要把lua函数当做协程函数传入
--必须 先调用 xlua.util中的cs_generator(lua函数)
b = mono:StartCoroutine(util.cs_generator(fun))
```

# Lua调用C# 泛型相关知识

```C#
public class Lesson12
{
    public interface ITest
    {
    }
    public class TestFather
    {
        
    }

    public class TestSon : TestFather,ITest
    {

    }

    public void TestFun<T>(T a,T b) where T : TestFather
    {
        Debug.Log("有参数的约束泛型方法");
    }

    public void TestFun2<T>(T a,T b) 
    {
        Debug.Log("有参数的无约束泛型方法");
    }

    public void TestFun3<T>() where T : TestFather
    {
        Debug.Log("无参数的约束泛型方法");
    }

    public void TestFun4<T>(T a) where T : ITest
    {
        Debug.Log("有参数的有接口约束泛型方法");
    }
    
}
```

Lua代码:

```Lua
Lesson12 = CS.Lesson12
local obj = Lesson12()

local son = Lesson12.TestSon()
local father = Lesson12.TestFather()
obj:TestFun(son,father)
obj:TestFun(father,son)

--Lua中不支持没有约束的泛型函数
-- obj:TestFun2(son,father)

--Lua中不支持有约束但是没有参数的泛型函数
-- obj:TestFun3()

--Lua中不支持非class的约束
-- obj:TestFun4(son)


--有一定的使用限制
--Mono打包 这种方式支持使用
---IL2CPP打包  如果泛型参数是引用类型才可以使用
---IL2CPP打包  如果泛型参数是值类型，除非C#那边已经调用过了 同类型的泛型参数 lua中才能够被使用
---
--补充知识 让上面 不支持使用的泛型函数  变得能用
-- 得到通用函数
-- 设置泛型类型再使用
--xlua.get_generic_method(类名,函数名)
local testFunc2 = xlua.get_generic_method(Lesson12,"TestFun2")
local testFunc2_R = testFunc2(CS.System.Int32)
--调用泛型函数
--成员方法  第一个参数 传调函数的对象
--静态方法 不用传
testFunc2_R(obj,1)
```

