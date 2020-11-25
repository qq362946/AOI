using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Transactions;

namespace AOI
{
    class Program
    {
        static void Main(string[] args)
        {
            var zone = new AoiZone();

            for (var i = 1; i <= 100000; i++)
            {
                var ss = zone.Enter(i, i, i);
            }

            // 更新key为50的坐标

            var entity = zone.Update(50, 3, 3, new Vector2(3, 3));

            Console.WriteLine("---------------玩家可视范围列表--------------");

            foreach (var aoiKey in entity.ViewEntity)
            {
                var findEntity = zone[aoiKey];
                Console.WriteLine($"X:{findEntity.X.Value} Y:{findEntity.Y.Value}");
            }
        }
    }
}