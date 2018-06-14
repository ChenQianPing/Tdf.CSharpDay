using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter7
{
    public class TestParallelLinq07
    {
        public static void TestMethod1()
        {
            ConcurrentQueue<Product> products = new ConcurrentQueue<Product>();
            /*向集合中添加多条数据*/
            Parallel.For(0, 1000, (num) =>
            {
                products.Enqueue(new Product() { Category = "Category" + num, Name = "Name" + num, SellPrice = num });
            });

            products.AsParallel().Where(P => P.Name.Contains("1") && P.Name.Contains("2") && P.Name.Contains("3")).ForAll(product =>
            {
                Console.WriteLine("Name:{0}", product.Name);
            });

            Console.ReadLine();
        }
    }
}

/*
 * Name:Name123
Name:Name231
Name:Name312
Name:Name321
Name:Name132
Name:Name213

    使用ForAll 并行遍历结果

ForAll是并行，foreach是串行，如果需要以特定的顺序处理数据，那么必须使用上述串行循环或方法。


 */
