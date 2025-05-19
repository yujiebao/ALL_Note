# UGUI的六大基础控件

Canvas对象上依附的:

Canvas:画布组件，主要用于渲染UI控件

Canvas Scaler:画布分辨率自适应组件，主要用于分辨率自适应

Graphic Raycaster:射线事件交互组件，主要用于控制射线响应相关

RectTransform:UI对象位置锚点控制组件，主要用于控制位置和对其方式

EventSystem对象上依附的:

EventSystem和Standalone Input Module

玩家输入事件响应系统和独立输入模块组件，主要用于监听玩家操作

# Canvas

## 1.Canvas组件用来干什么?

Canvas的意思是画布

它是UGUI中所有UI元素能够被显示的根本它主要负责渲染自己的所有UI子对象

如果UI控件对象不是Canvas的子对象，那么控件将不能被染

我们可以通过修改Canvas组件上的参数修改染方式

 

场景中可以有多个Canvas对象

场景中允许有多个Canvas对象

可以分别管理不同画布的渲染方式，分辨率适应方式等等参数

如果没有特殊需求  一般情况 场景上一个Canvas即可

## 2.Canvas 的三种渲染模式

Screen Space -Overlay: 屏幕空间 覆盖模式，UI始终在前 

Screen Space -Camera: 屏幕空间 摄像机模式，3D物体可以显示在UI之前，

World Space:世界空间，3D模式

# Image和RawImage

## Image

Image是图像组件

是UGUI中用于显示精灵图片的关键组件

除了背景图等大图，一般都使用Image来显示UI中的图片元素

## RawImage

RawImage是原始图像组件

是UGUI中用于显示任何纹理图片的关键组件

它和Image的区别是 一般RawImage用于显示大图(背景图，不需要打入图集的图片，网络下载的图等)

# 常用控件

## Button

Button是按钮组件

是UGUI中用于处理玩家按钮相关交互的关键组件

​        

默认创建的Button由2个对象组成

父对象-Button组件依附对象 同时挂载了一个Image组件 作为按钮背景图

子对象一按钮文本(可选)

## Toggle

Toggle是开关组件

是UGUI中用于处理玩家单选框多选框相关交互的关键组件

开关组件 默认是多选框

​        

可以通过配合ToggleGroup组件制作为单选框

默认创建的Toggle由4个对象组成

父对象-Toggle组件依附

子对象一背景图(必备)选中图(必备)说明文字(可选)

## InputField 

InputField是输入字段组件

是UGUI中用于处理玩家文本输入相关交互的关键组件

 

默认创建的InputField由3个对象组成

父对象——InputField组件依附对象 以及 同时在其上挂载了一个Image作为背景图

子对象——文本显示组件(必备)、默认显示文本组件(必备)

## slider

是UGUI中用于处理滑动条相关交互的关键组件

​        

默认创建的slider由4组对象组成

父对象-slider组件依附的对象

子对象一背景图、进度图、滑动块三组对象

## scrollbar

scrollbar是滚动条组件

是UGUI中用于处理滚动条相关交互的关键组件

​        

默认创建的scrollbar由2组对象组成

父对象-scrollbar组件依附的对象

子对象一滚动块对象

   

一般情况下我们不会单独使用滚动条

都是配合scrollview滚动视图来使用

## scrollRect

scrollRect是滚动视图组件

是UGUI中用于处理滚动视图相关交互的关键组件

 

默认创建的scrollRect由4组对象组成

父对象-scrollRect组件依附的对象 还有一个Image组件 最为背最图

子对象

viewport控制滚动视图可视范围和内容显示

Scrollbar Horizontal 水平滚动条

Scrollbar Vertical 垂直滚动条

## DropDown

DropDown是下拉列表(下拉选单)组件

是UGUI中用于处理下拉列表相关交互的关键组件

​        

默认创建的DropDown由4组对象组成

父对象

DropDown组件依附的对象 还有一个Image组件 作为背景图

​        

子对象

Labe1是当前选项描述

Arrow右侧小箭头

Template下拉列表选单

# 图集

UGUI和NGUI使用上最大的不同是 NGUI使用前就要打图集

UGUI可以再之后再打图集

​        

打图集的目的就是减少Drawcall 提高性能



Drawcall 就是CPU通知GPU进行一次渲染的命令

如果Drawcall 次数较多会导致游戏卡顿

我们可以通过打图集，将小图合并成大图，将本应n次的Drawcall 变成1次DC来提高性能

# UI事件监听

## 为什么使用UI事件监听

目前所有的控件都只提供了常用的事件监听列表

如果想做一些类似长按，双击，拖拽等功能是无法制作的

或者想让Image和Text，RawImage三大基础控件能够响应玩家输入也是无法制作的

​        

而事件接口就是用来处理类似问题

让所有控件都能够添加更多的事件监听处理对应的逻辑

## 常用的事件接口

```C#
IPointerEnterHandler-OnPointerEnter-当指针进入对象时调用 (鼠标进入)
IPointerExitHandler-OnPointerExit-当指针离开对象时调用 (鼠标离开)
IPointerDownHandler-OnPointerDown-在对象上按下指针时调用(按下)
IPointerupHandler - OnPointerup - 松开指针时调用(在指针正在点击的游戏对象上调用)(抬起)
IPointerclickHandler -0nPointerclick-在同一对象上按下再松开指针时调用 (点击)


IBeginDragHandler-OnBeginDrag-即将开始拖动时在拖动对象上调用(开始拖拽)
IDragHandler-OnDrag-发生拖动时在拖动对象上调用(拖拽中)
IEndDragHandler - OnEndDrag - 拖动完成时在拖动对象上调用(结束拖拽)
```

## 如何使用事件接口

1.实现事件接口

2.在事件接口对应的方法中添加逻辑

3.在需要响应事件的控件上添加脚本

## PointerEventData参数 

父类:BaseEventData

pointerId: 鼠标左右中键点击鼠标的ID 通过它可以判断右键点击

position:当前指针位置(屏幕坐标系)

pressPosition:按下的时候指针的位置

delta:指针移动增量

clickcount:连击次数

clickTime:点击时间

​        

pressEventcamera:最后一个OnPointerPress按下事件关联的摄像机

enterEvetnCamera:最后一个OnPointerEnter进入事件关联的摄像机

# EventTrigger

## 事件触发器是什么

事件触发器是EventTrigger组件

它是一个集成了上面所有事件接口的脚本

它可以让我们更方便的为控件添加事件监听

## 如何使用事件触发器 

1.拖拽脚本进行关联

2.代码添加

```C#
//声明一个希望监听的事件对象
EventTrigger.Entry entry =  new EventTrigger.Entry();

//声明监听函数的类型
entry.eventID = EventTriggerType.PointerUp;

//监听函数关联
entry.callback.AddListener((data)=>{print("抬起");});

//把声明好的事件对象添加到事件触发器中
ET.triggers.Add(entry);

```

# Mask遮罩

## 遮罩是什么

在不改变图片的情况下

让图片在游戏中只显示具中的一部分

## 使用Mask遮罩

实现遮罩效果的关键组件时Mask组件'

通过在父对象上添加Mask组件即可遮罩其子对象

注意:

1.想要被遮罩的Image需要勾选Maskable

2.只要父对象添加了Mask组件，那么所有的UI子对象都会被遮罩

3.遮置父对象图片的制作，不透明的地方显示，透明的地方被遮罩

# 模型和粒子显示在UI前面

**1.直接摄像机渲染3D模型**

canvas的渲染模式要不是覆盖模式

摄像机模式 和 世界(3D)模式都可以让模型显示在UI之前(z轴在UI元素之前即可)

注意:

(1).摄像机模式时建议用专门的摄像机渲染UI相关

(2).面板上的3D物体建议也用UI摄像机进行渲染

**2.将3D物体渲染在图片上,通过图片显示**

专门使用一个摄像机渲染3D模型，将其渲染内容输出到Render Texture上

类似小地图的制作方式

再将渲染的图显示在uI上

该方式 不管canvas的渲染模式是哪种都可以使用

大量不建议使用  摄像机更多  影响性能

# 异形按钮

## 如何让异形按钮正确点击

**1.通过添加子对象的形式** 

按钮之所以能够响应点击，主要是根据图片矩形范围进行判断的

它的范围判断是自下而上的，意思是如果有子对象图片，子对象图片的范围也会算为可点击范围

那么我们就可以用多个透明图拼凑不规则图形作为按钮子对象用于进行射线检测

**2.通过代码改变图片的透明度响应阈值** 

1.第一步:修改图片参数 开启Read/Write Enabled开关

2.第二步:通过代码修改图片的响应阈值

该参数含义:指定一个像素必须具有的最小alpha值，以变能够认为射线命中了图片

说人话:当像素点alpha值小于了 该值 就不会被射线检测了

# 自动布局组件

**水平垂直布局组件**

 组件名：Horizontal Layout Group 和 Vertical Layout Group

参数相关：

Padding：左右上下边缘偏移位置

Spacing:子对象之间的间距

 

ChildAlignment:九宫格对其方式

Control Child Size：是否控制子对象的宽高

Use Child Scale：在设置子对象大小和布局时，是否考虑子对象的缩放

Child Force Expand：是否强制子对象拓展以填充额外可用空间

 

**网格布局组件**

组件名：Grid Layout Group

参数相关：

Padding：左右上下边缘偏移位置

Cell Size：每个格子的大小

Spacing：格子间隔

Start Corner:第一个元素所在位置（4个角）

Start Axis：沿哪个轴放置元素；Horizontal水平放置满换行，Vertical竖直放置满换列

Child Alignment：格子对其方式（9宫格）

Constraint：行列约束

Flexible：灵活模式，根据容器大小自动适应

Fixed Column Count：固定列数

Fixed Row Count：固定行数

**宽高比适配器**

1.让布局元素按照一定比例来调整自己的大小

2.使布局元素在父对象内部根据父对象大小进行适配

组件名：Aspect Ratio Fitter

参数相关：

Aspect Mode：适配模式，如果调整矩形大小来实施宽高比

None：不让矩形适应宽高比

Width Controls Height：根据宽度自动调整高度

Height Controls Width：根据高度自动调整宽度

Fit In Parent：自动调整宽度、高度、位置和锚点，使矩形适应父项的矩形，同时保持宽高比，会出现“黑边”

Envelope Parent：自动调整宽度、高度、位置和锚点，使矩形覆盖父项的整个区域，同时保持宽高比，会出现“裁剪”

 Aspect Ratio：宽高比；宽除以高的比值



# CanvasGroup

为面板父对象添加 CanvasGroup组件 即可整体控制

 

参数相关：

Alpha：整体透明度控制

Interactable:整体启用禁用设置

Blocks Raycasts：整体射线检测设置

Ignore Parent Groups：是否忽略父级CanvasGroup的作用