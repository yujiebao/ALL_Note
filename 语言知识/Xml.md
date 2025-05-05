# 1.XML的基本格式

```xml
<?xml version = "1.0" encoding = "UTF-8"?>
<!-- version代表版本 encoding代表编码格式(读取文件时，解析字符串使用的编码格式)-->
<!--
注释的内容
中间的都是
-->

<!-- 必须有一个根节点  XML为树形结构 树根-->
<!-- <节点名>
中间可以填写数据  或者在其中包裹别的节点
</节点名> -->
<PlayerInfo>  <!-- 这个就是此例的根节点 -->
	<Item name = "小明" age = "8"></Item>
	
	<!-- <a name = "ss" age = "61">
	</a> 下面是简写-->
	<a name = "ss" age = "61"/>

</PlayerInfo>
```

# 2.XML语法

## 1.头部信息

<?xml version = "1.0" encoding = "UTF-8"?>

version代表版本 encoding代表编码格式(读取文件时，解析字符串使用的编码格式) 

必须写在第一句

## 2.注释

<--

中间是注释的内容

-->

## 3.规则

必须有一个根节点  XML为树形结构 树根   有根才能查找

## 4.节点和属性

属性和元素节点只是写法上的区别而已 我们可以选择自己喜欢的方式来记录数据

```xml
<?xml version="1.0" encoding="utf-8"?>
<TestLesson3>
  <test1 num ="1">
    <int>0</int>
  </test1>
  <test2>
    <string>test2</string>
  </test2>
</TestLesson3>
```

# 