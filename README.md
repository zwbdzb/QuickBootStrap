# QuickBootStrap

## 背景
    QuickBootstrap 广告营销业绩管理平台最早是基于网上开源QuickBootstrap开发的外包项目。
      项目主要是 实时接收服务端传递的广告佣金分成数据，服务端确认有效佣金之后[服务端最早1个月确认数据，最晚需要4个月确认数据]，  
      主动请求接口更新实时存储的佣金数据。
    后来集成了baiduLBS自定义实时标记的开发功能： 从jstree树上拖拽节点到百度地图上标记定位。 
    
## 技术难点
-    Webapi接口实时接口数据
-    服务端确认数据的时间不确定性 怎么转化为计算机算法？

## 方案
-    AutoMapp对象映射功能将 请求数据、响应数据和 DTO模型数据关联
-    每日定时任务 去获得间隔时间确认的有效佣金，以期随时获得更新的佣金数据
      
        比如： 今天请求[-120,-30]总共90天的确认数据，明天请求[-119,-29]总共90天的确认数据，这样不断的循环覆盖更新,       
            就能满足服务端确认数据的时间不确定性,使用Quartz.net 做定时轮询任务。
    
-    前端展示使用了Bootstrap及其插件，特别指出 完全应用了BootstrapTable
      因为前端查询条件不确定，分析需要使用动态LINQ语句，因而使用了 EF+ DynamicLinq

-    后续集成功能： baiduLBS 自定义标记和jstree实时拖拽标记 功能组件相对独立：
	
	-  baiduLBS自定义Marker
	-  jstree api及其types，dnd插件功能
	-  JqueryUI draggable插件拖拽div的能力
      
      需要将这三项知识点结合起来，完成以上交互。
    
    

![baiduLBS](https://raw.githubusercontent.com/mi12205599/QuickBootStrap/master/QuickBootstrap/Content/ImageGui/UI_1.jpg)
