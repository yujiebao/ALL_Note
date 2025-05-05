# Json语法

json语法较为简单  理解键值对形式即可

```Json
//注释

/*

Json是一种键值对的结构

Json中的键一定要用双引号包起来

*/

//大括号代表一个对象

{

    "name" : "张三", //字符串类型

    "age" : 18, //数字类型

    "sex" : true, //布尔类型

    "TestF" : 1.5,//浮点数类型

    "ids" : [1, 2, 3], //数组类型

    "students" : [{"name":"张三","age":18,"sex":true},{"name":"李四","age":19,"sex":false}] ,//对象数组类型   最后一个不要加逗号可能会出错

    "home":{"address":"北京","street":"北京街道"}, //对象类型

    "test" : null, //null类型

    "dic" : {"key1":"value1","key2":"value2"} //字典类型  键只能是字符串 

}

```

