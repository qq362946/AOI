采用十字链表+快慢针方式的AOI算法（支持ET框架）,并针对GC问题进行了优化。

性能测试、2W人在同AOI场景下，每次更新AOI耗时平均在3MS以内。插入在6MS以内。

理论上10W人或者更多效率不会下降、具体没有测试，因为不可能有一个场景下管理这么多玩家的情况。

使用说明:

首次进入AOI需要用到AoiComponent.Enter方法，里面有三个参数（Id，x坐标,y坐标）。

角色位置变动需要用到AoiComponent.Update方法，里面有四个参数（Id，AOI范围,x坐标,y坐标）。

AOI范围：是一个Vector2类型,x代表x轴的距离，y代表y轴的距离。该ID统计的数据不会超过这个距离。

操作例子：

var aoi = new AoiComponent();

var role1 = aoi.Enter(1, 12, 8);

Console.WriteLine($"玩家一ID:{role1.Id}");

var role2 = aoi.Enter(2, 12, 8);

Console.WriteLine($"玩家二ID:{role2.Id}");

aoi.Update(2, new Vector2(1, 1), 13, 8);  // 玩家二移动

Console.WriteLine($"玩家二周围列表");
            
foreach (var aoiNode in role2.AoiInfo.MovesSet)
{
    Console.WriteLine(aoi.GetNode(aoiNode).Position);
}
            
Console.WriteLine($"玩家二进入列表");
            
foreach (var aoiNode in role2.AoiInfo.EntersSet)
{
    Console.WriteLine(aoi.GetNode(aoiNode).Position);
}
            
Console.WriteLine($"玩家二离开列表");
            
foreach (var aoiNode in role2.AoiInfo.LeavesSet)
{
    Console.WriteLine(aoi.GetNode(aoiNode).Position);
}
            
Console.WriteLine($"玩家二移动列表");
            
foreach (var aoiNode in role2.AoiInfo.MoveOnlySet)
{
    Console.WriteLine(aoi.GetNode(aoiNode).Position);
}

