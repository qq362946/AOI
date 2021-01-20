using System;
using System.Linq;
using System.Numerics;

namespace AOI
{
    class Program
    {
        static void Main(string[] args)
        {
            float floatBase = 1.41421354f;
            
            var zone = new AoiZone();
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

            Console.WriteLine("---------------------------");

            try
            {
                // 加入自己
                var mine = zone.Enter(123456789, 15, 6, area, out var mineEnters);
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
        }
        
        // static void Main(string[] args)
        // {
        //     var zone = new AoiZone();
        //     var area = new Vector2(3, 3);
        //
        //     for (var i = 1; i <= 5; i++)
        //     {
        //         var ss = zone.Enter(i, i, i);
        //     }
        //
        //     zone.Update(3, area, out var enters);
        //
        //     Console.WriteLine("---------------加入玩家范围的玩家列表--------------");
        //
        //     foreach (var aoiKey in enters)
        //     {
        //         var findEntity = zone[aoiKey];
        //         Console.WriteLine($"X:{findEntity.X.Value} Y:{findEntity.Y.Value}");
        //     }
        //
        //     // 更新key为50的坐标
        //
        //     var entity = zone.Update(50, 3, 3, new Vector2(3, 3));
        //
        //     Console.WriteLine("---------------离开玩家范围的玩家列表--------------");
        //
        //     foreach (var aoiKey in entity.Leave)
        //     {
        //         var findEntity = zone[aoiKey];
        //         Console.WriteLine($"X:{findEntity.X.Value} Y:{findEntity.Y.Value}");
        //     }
        //     
        //     Console.WriteLine("---------------key为50的玩家离开当前AoiZone--------------");
        //
        //     zone.Exit(50);
        // }
    }
}