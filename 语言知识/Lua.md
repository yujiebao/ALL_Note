

# 变量

## 1.变量类型

lua当中的简单变量类型:nil number string boolean

lua中所有的变量申明 都不需要申明变量类型 他会自动的判断类型         ---类似C# 里面的 var

lua中的一个变量 可以随便赋值 -自动识别类型

字符串的声明 使用单引号或者双引号包裹

lua里 没有char

## 2.type()

通过type函数 来得到变量的类型   type是string类型  type(type())  

print(c)  --未定义的c是nil   

# string

## 1.获取字符串长度   #

```Lua
a = "6666"
print(#a)
```

此方法获取的长度不可靠  其中nil和序号都会影响长度

## 2.多行打印   [[]]

```Lua
a = [[nihao
sdasdas
]]
print(a)
```

## 3.字符串拼接

```lua
print("nihao".."sdsad"..111)
print(string.format("I am %s",'xiaozhang'))
```

## 4.别的类型转字符串    tostring()

## 5.其他公共方法

```lua
a = "nihao"
print(string.upper(a))   
print(string.lower(a))
print(string.reverse(a))
print(a)  -- 这些方法不会改变原来字符串  返回一个新的字符串
print(string.find(a,'ih'))    --返回位置   Lua中下标起始位1不是0
print(string.sub(a,1,2))
print(string.gsub(a,'i','*'))    --替换字符串
```

# 操作符

## 1.算术运算符  + - * / % ^

没有自增 自减

没有复合运算符 += -= *= /= %= ^=

字符串  可以进行算术运算 会自动将字符串转换为number

```Lua
print(51 + "215")
print(51 - "215")
print(51 * "215")
print(51 / "215")
```

## 2.条件运算符

### 不等于 ~=

### 逻辑运算    or and not

lua中也遵循逻辑短路原则

## 3.位运算符和三目运算符  在lua中不支持 需要自己实现

# 条件分支语句

## 1.if  then  end  单分支

相当于   if(){}

```lua
if a == 9 then
print("a is 9")    
end 
```

## 2.if then else end  双分支

相当于  if(){}else{}

```lua
if a > 5 then
print("a is bigger than 5")    
else
print("a is smaller than 5")
end
```

## 3.if then elseif else end  多分支

相当于  if(){} else if(){} else {}

```lua
if a > 5 then
print("a is bigger than 5")    
elseif a < 5 then    --elseif不能分开写
print("a is smaller than 5")
else
print("a is equal to 5")
end
```

## 4.Lua中没有Switch语句

# 循环语句

## 1.while循环

```lua
num = 0;
while num < 5  do   --执行条件 满足条件的执行
    print(num)
    num = num + 1
end
```

## 2.do....while循环

```lua
num = 0
repeat
    print(num)
    num = num + 1
until num > 5  --结束条件 不满足条件的执行
```

## 3.for循环

```lua
for i = 1, 5  do   --默认自增
    print(i)
end

--自增2
print("for Add 2")
for i =1, 5,2  do    --for后的第三个参数是每次自增的增量
print(i)
end
```

# 函数

## 注意：

1.函数声明后才能执行    ---程序顺序执行

2.函数不支持重载 		默认调用最近声明的函数

## 变长参数    ...

```lua
function F5(...)
    print("F5 function")
    arr = {...}
    --变长参数先使用一个表存储 再使用
    for i= 1,#arr do
        print(arr[i])
    end
end
```

## 函数嵌套和闭包

```lua
function F8(x)
    print("F8 function")
    return function (y)
        print("F8 in function")
        print(x,y)
    end
end

F9 = F8(1)     --1 闭包  1在F9中使用 1作为F8的参数在F8执行结束时候没有释放  在F9中再次使用 改变了1的生命周期
F9(2)
```

# 表

所有的复杂类型都是table(表) 

## 1.数组

下标起始为1 

### 二维数组

a = {{},{},{}}

### 自定义索引

a={[0]=1,2,3,4,5,6}    

## 2.迭代器

### 1.为什么使用迭代器

迭代器遍历 主要是用来遍历表的

#得到长度 其实并不准确 一般不要用#来遍历表

### 2.ipairs遍历

ipairs遍历 还是 从1开始往后遍历的 小于等于0的值得不到(自定义索引小于等于0)

只能找到连续索引的 键 如果中间断序了 它也无法遍历出后面的内容

```lua
for i, k in ipairs(a) do
    print(i,k)
end

```

### 3.pairs遍历

-- 它能够把所有的键都找到 通过键可以得到值 

```lua
for index, value in pairs(a) do
    print(index,value)
end
print()
for index in pairs(a) do
    print(index)    --只遍历键
end
```

## 3.Dictionary字典

```lua
--1.字典的声明与访问
a = {["nihao"] = "nihao",["sdsad"] = "sdsad",[1] = 5,["ss"] = nil}
--访问字典
--两种方式  []和.   但是如果键是number类型时 只能用[]的形式访问
print(a.ssd)   --不存在的key会返回nil  
print(a["nihao"])
--2.增删查改
--修改值
a["sdsad"] = string.upper(a["sdsad"])
print(a["sdsad"])
--新增
a["nihao1"] = "nihao1"
print(a["nihao1"])
--删除
a["nihao1"] = nil
print(a["nihao1"])
```

### 1.遍历字典   --一定要用pairs

```lua
for k,v in pairs(a) do
    print(k,v)   --print 可以传多个参数
end
--只遍历键  不能只遍历值
for k in pairs(a) do
    print(k)   --print 可以传多个参数
end

```

## 4.类和结构体

### 1.类的申明

```lua
Student = {
    name = "nihao",
    age = 18,
    sex = "man",
    up = function ()
        --这样写 age 和表中的age没有任何关系 它是一个全局变量
        -- print(age)
        --想要在表内部函数中 调用表本身的属性或者方法
        --一定要指定是谁的 所以要使用 表名·属性 或 表名·方法
        print(Student.age)
    end,
    --把自己作为参数 传递给函数
    show = function(self)    
        --不写self  和表中的age没有任何关系 它是一个全局变量
        print(self.name,self.age,self.sex)
    end
}
```

注意类中函数使用类中变量的时候不可直接写 ---直接使用的是全局变量不是类中的变量

解决方案:

1.外部将类作为参数传入

```lua
student.show(student)   --或者student:show()
```

2.在函数中使用的时候使用本类中的变量   age --->student.age

### 2.类的使用

```lua
--调用函数
student.up();
--需要自己传入参数
student.show(student)     -- 一般形式  
--语法糖
student:show()
--添加成员变量和函数
Student.a ='a'
Student.show2 = function(self)
print(self.name,self.age,self.sex,self.a)
end
```

### 3.：语法糖

```lua
--声明的时候
function Student:show3()   --:不需要把self传进去  注意后面的self代表的是第一个参数，而不是this
print(self.name,self.age,self.sex,self.a)
end
--调用的时候
Student:show2()

--具体测试
a = {
    an = 5,
    test = function(self)
        print(self.an+10)
    end
}

b = {
    an = 60,
    test = function(self)
        print(self.an+10)
    end
}
function a:show()
    print(self.an)
end

a:show()
a:test()

a.show(b)    --这里调用时使用的self就是b
a.test(b)	 --这里调用时使用的self是b
-- 结果
-- 5
-- 15
-- 60
-- 70
```

### 4.表中提供的一些公共方法

```lua
t1 = {{age = 18,name = "nihao"} ,{age = 19,name = "nihao1"}}
t2 = {name = "xiaoli",sex = true}

--插入
print(#t1)   --两张表  {age = 18,name = "nihao"} {age = 19,name = "nihao1"}}
table.insert(t1,t2)
print(#t1)    --三张表  添加了t2表

--删除
table.remove(t1,1)   --删除第一张表   不传第二个参数默认删除最后一张表

t2 ={5,7,9,6,20,94,16}
--排序
table.sort(t2)   --默认是从小到大排序
for i,v in ipairs(t2) do
    print(i,v)
end
table.sort(t2,function(a,b)
    return a>b   --定义排序规则  类似使用了委托
end)
print("*********************")
for i,v in ipairs(t2) do
    print(i,v)
end

--拼接
arr = {"aa","bb","cc","dd"}
str = table.concat(arr,"-")
print(str)
```

# 多脚本执行

## 1.全局变量和本地变量

本地变量仅可以在声明的范围内使用

```lua
for i = 1, 3 do
    local d = 5
end
print(d)   --d = nil

function tt()
    local d1 = 5
end
tt()
-- print(d1)   访问不到
```

## 2.多脚本执行

### 2-1.加载脚本   require

  加载脚本的时候会把脚本中的代码进行执行

   访问不到加载脚本的本地变量     ---可以通过脚本的return返回值获取

### 2-2脚本卸载

require已经加载执行的脚本，加载一次过后就不会再被执行了

#### 1.package.loaded

  `package.loaded` 是一个表（table），用于跟踪已经加载的模块。当你使用 `require` 函数加载一个模块时，Lua 会检查 `package.loaded` 表中是否已经存在该模块。如果模块已经被加载过，`require` 会直接返回 `package.loaded` 表中存储的值(模块的返回值)

#### 2.卸载脚本

```lua
package.loaded["Lesson11_Test"] = nil
local TestLocalA = print(package.loaded["Lesson11_Test"])
print(TestLocalA)

```

## 3.G表

G表是一个总表(table)他将我们申明的所有全局的变量都存储在其中    本地变量不会存储到G表 

```lua
for key, value in pairs(_G) do
    print(key, value)
end
```

# 特殊用法

## 1.多变量赋值

```lua
lua a, b, c = 1, 2, '12212'   --少写默认后面补空  多写后面默认省略
```

## 2.多返回值

```lua
function test()
    return 1,2,3   
    --多返回值时 你用几个变量接 就有几个值
    --如果少了 就少接几个 如果多了 就自动补空
end
```

## 3.and or

and or 他们不仅可以连接 boolean 任何东西都可以用来连接

lua中 只用nil 和 false 才认为是假

 "短路"-对于and来说 有假则假 对于or来说 有真则真

所以 他们只需要判断 第一个 是否满足 就会停止计算了

```lua
print(true and 1)   --1
print(false and 1)  --false
print(true or 1)    --true
print(false or 1)   --1
```

## 4.通过短路来实现三目运算

```lua
x = 3
y = 2
local res = (x > y ) and x or y
print(res)
-- x > y
--true   
--(true and x)  -->x 
--x or y ----->x
--false
--(false and x)    --->false
--false or y  --->y
```

# 协同程序

## 1.创建协程

### 1.create

```lua
Func = function ()
    print("CorFunc")
end

cc = coroutine.create(Func)
print(cc)   --协程的本质是一个线程对象   thread
```

### 2.warp

```lua
cc2 = coroutine.wrap(Func)
print(cc2)     --function
```

## 2.运行协程

上面的两种声明方式也就对应了两种运行方式

```lua
--第一种方式
coroutine.resume(cc)
--第二种方式
cc2()
```

## 3.协程的挂起

```lua
Func2 = function ()
    local i = 1;
    while true do
        print("CorFunc2_"..i)
        i = i + 1
        --协程的挂起函数   挂起后等待再次被调用
        coroutine.yield(i)
    end
end
```

## 4.协程的返回值

```lua
cc3 = coroutine.create(Func2)
isOk,tem =coroutine.resume(cc3)    --此方式的返回值第一个为协程是否执行成功，后面的自己定义的
print(isOk,tem)
isOk,tem = coroutine.resume(cc3)
print(isOk,tem)

cc4 = coroutine.wrap(Func2)
tmp = cc4()    --此方式的返回值第一个就是自己定义的
print(tmp)
tmp = cc4()
print(tmp)
```

## 5.协程的状态

 coroutine.status( )   -- 获取状态

三种状态:suspended   dead  running(内部获取)

# 元表

## 1.什么是元表

任何表变量都可以作为另一个表变量的元表

任何表变量都可以有自己的元表(爸爸)

当我们子表中进行一些特定操作时会执行元表中的内容

## 2.设置元表

```lua
meta = {}
myTable = {}
--第一个参数:子表 第二个参数:元表(爸爸)
setmetatable(myTable,meta)
```

## 3.特定操作

### 1.__Tostring

```Lua
meta = {
    __tostring = function (t)    --第一个参数:默认传入子表
        print("myTable")
        return t.name
    end
}
myTable = {
    name = "xiaozhang"
}
--第一个参数:子表 第二个参数:元表(爸爸)
setmetatable(myTable,meta)
print(myTable)
```

### 2.__call

```Lua
meta2 = {
    __call = function (t,a,b)
        --子表作为一个函数使用的时候会执行此函数
        print(t)    --第一个参数:默认传入子表(调用者自己)  此处相当于执行了一次__tostring  并打印了__tostring的返回值
        print("__call")
        return a+b
    end,
    __tostring = function (t)
        print("myTable")
        return t.name
    end
}
myTable2 = {
    name = "xiaozhang"
}
setmetatable(myTable2,meta2)
print(myTable2(1,2))   --必须实现了__call元方法 才能使用子表作为函数调用  否者报错
```

### 3.运算符重载

```Lua
meta3 ={
    __add= function (t1,t2)
        return t1.age + t2.age
    end ,
    __sub= function (t1,t2)
        return t1.age - t2.age
    end
}
mytable3 = {
    age = 18
}
setmetatable(mytable3,meta3)

mytable4 = {
    age = 19
}
print(mytable3 + mytable4)   --没有实现__add元方法 会报错
print(mytable3 - mytable4)   --没有实现__sub元方法 会报错
-- __mul  *
-- __div  /
-- __mod  %
-- __pow  ^
-- __concat  ..   一个使用原表即可

--使用逻辑运算符时 需要两个子表都使用相同的元表  才能准确调用
-- __eq  ==
-- __lt  <
-- __le  <=
```

### 4.__index

  `__index`:当我们访问一个表中不存在的属性时 会自动调用`__index`元方法 去`__index`指定的表中去找

```Lua
--可以嵌套  查找时一层一层向上找  
meta5_father = {
    name = "xiaozhang",
    age = 19,
}
meta5_father.__index = meta5_father    //子表找不到来找元表时  访问元表的__index  
meta5 = {
}
meta5.__index = meta5   --写在外部好一点，确保表中全部数据都能访问到(写在内部可能表没有加载完成)
myTable5 = {
 
}
setmetatable(meta5,meta5_father)  --嵌套元表   
setmetatable(myTable5,meta5)

```

### 5.__newindex

```lua
__newindex:当赋值时，如果赋值一个不存在的索引  那么会把这个值赋值到newindex所指的表中 不会修改自己
--也支持嵌套
myTable6 = {}
meta6 = {} 
setmetatable(myTable6,meta6)
myTable6.age = 18  
print(myTable6.age)     --没有重定向  添加到mytable6中
meta6.__newindex = meta6    --重定向了赋值的表  
myTable6.name ="sss"
print(myTable6.name)   --设置了重定向 赋值到meta6中
```

# 垃圾回收

```Lua
--垃圾回收
test = { id = 1}

--关键字collectgarbage
--获取当前lua占用的内存数 K字节
print(collectgarbage("count"))
print(collectgarbage("count"))

--进行垃圾回收
collectgarbage("collect")
print(collectgarbage("count"))
test = nil   --去除引用加速回收
collectgarbage("collect")
print(collectgarbage("count"))

--lua中有自动垃圾回收
--Unity中热更新开发 尽量不要去用自动垃圾回收   销毁性能  一般切场景时回收
```

# Lua中实现面向对象

```Lua
--面向对象实现
--万物之父  所有面向对象的基类 Object
Object = {}
--封装
--实例化方法  给空表(obj)设置原表  以及__index元方法
function Object:new()
    local obj = {}   --代表一个新的地址
    self.__index = self 
    setmetatable(obj, self) 
    return obj
end
--继承
function Object:subClass(className)
    --根据名字生产一个类(表)
    _G[className] = {}   
    --设置元表为父类 
    local obj = _G[className]
    obj.base = self
    --给子类设置元表  以及__index元方法
    self.__index = self
    setmetatable(obj, self)
end

--声明一个类
Object:subClass("GameObject")
GameObject.Posx = 0
GameObject.Posy = 0
function GameObject:Move()
    self.Posx = 1 + self.Posx
    self.Posy = 1 + self.Posy
    print(self.Posx..","..self.Posy)
end

--实例化对象
local gameObject = GameObject:new()
print(gameObject.Posx..","..gameObject.Posy) --输出0,0
gameObject:Move() --调用方法

GameObject:subClass("Player")
function Player:Move()
    self.base.Move(self)
    self.Posx = 2 + self.Posx
    self.Posy = 2 + self.Posy
    print(self.Posx..","..self.Posy)
end

local player = Player:new()
print(player.Posx..","..player.Posy) --输出0,0
player:Move() --调用方法
```

