AOI库介绍
==========

<p>1.使用跳跃表+十字链方式实现的一个AOI库。</p>
<p>2.可做简单的碰撞检测、客户端资源、服务器AOI。</p>
<p>3.测试效率插入、移动、查找均到毫秒一下。</p>

使用例子
==========
            float floatBase = 1.41421354f;   
            //  创建一个区域
            var zone = new AoiZone();
            //  创建一个查找访问
            var area = new Vector2(2 * floatBase, 2 * floatBase);
            //  加入机器人单位
            int counter = 0;
            for (var i = 1; i <= 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    Console.WriteLine($"已加入机器人 x:{j} y:{i} id:{counter}");
                    zone.Enter(counter, j, i);
                    counter++;
                }
            }
            try
            {
                // 加入自己
                var mine = zone.Enter(123456789, 15, 6, area, out var mineEnters);
                // 刷新区域
                zone.Update(123456789, area, out var updateList);
                Console.WriteLine(updateList.Count() + " 数量");
                foreach (var aoiKey in updateList)
                {
                    var findEntity = zone[aoiKey];
                    Console.WriteLine($"周围的单位 - Key:{findEntity.Key} X:{findEntity.X.Value} Y:{findEntity.Y.Value}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
相关资料
==========
[AOI算法实现和原理（一）](https://zhuanlan.zhihu.com/p/56114206?from_voters_page=true)
[AOI算法实现和原理（二）](https://zhuanlan.zhihu.com/p/345741408)

  
