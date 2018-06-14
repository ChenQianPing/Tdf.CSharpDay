using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter7
{
    public class TestParallelLinq02
    {
        public static void TestMethod1()
        {
            var products = new ConcurrentQueue<string>();
            products.Enqueue("E");
            products.Enqueue("F");
            products.Enqueue("B");
            products.Enqueue("G");
            products.Enqueue("A");
            products.Enqueue("C");
            products.Enqueue("SS");
            products.Enqueue("D");

            /*不采用并行化  其数据输出结果  不做任何处理   */
            var productListLinq = from product in products
                                  where (product.Length == 1)
                                  select product;

            var appendStr = string.Empty;
            foreach (var str in productListLinq)
            {
                appendStr += str + " ";
            }
            Console.WriteLine($"不采用并行化 输出:{appendStr}");

            /*不采用任何排序策略  其数据输出结果 是直接将分区数据结果合并起来 不做任何处理   */
            var productListPLinq = from product in products.AsParallel()
                                   where (product.Length == 1)
                                   select product;

            appendStr = string.Empty;
            foreach (var str in productListPLinq)
            {
                appendStr += str + " ";
            }
            Console.WriteLine($"不采用AsOrdered 输出:{appendStr}");

            /*采用 AsOrdered 排序策略  其数据输出结果 是直接将分区数据结果合并起来 并按原始数据顺序排序*/
            var productListPLinq1 = from product in products.AsParallel().AsOrdered()
                                    where (product.Length == 1)
                                    select product;
            appendStr = string.Empty;
            foreach (var str in productListPLinq1)
            {
                appendStr += str + " ";
            }
            Console.WriteLine($"采用AsOrdered 输出:{appendStr}");

            /*采用 orderby 排序策略  其数据输出结果 是直接将分区数据结果合并起来 并按orderby要求进行排序*/
            var productListPLinq2 = from product in products.AsParallel()
                                    where (product.Length == 1)
                                    orderby product
                                    select product;
            appendStr = string.Empty;
            foreach (var str in productListPLinq2)
            {
                appendStr += str + " ";
            }
            Console.WriteLine($"采用orderby 输出:{appendStr}");

            Console.ReadLine();
        }
    }
}


/*
 * 
 * 不采用并行化 输出:E F B G A C D
不采用AsOrdered 输出:B E F G A C D
采用AsOrdered 输出:E F B G A C D
采用orderby 输出:A B C D E F G



 * AsOrdered()与orderby
AsOrdered:保留查询的结果按源序列排序，在并行查询中，多条数据会被分在多个区域中进行查询，
查询后再将多个区的数据结果合并到一个结果集中并按源序列顺序返回。

orderby：将返回的结果集按指定顺序进行排序

 在PLINQ查询中，AsOrdered()和orderby子句都会降低运行速度，
 所以如果顺序并不是必须的，那么在请求特定顺序的结果之前，
 将加速效果与串行执行的性能进行比较是非常重要的。



 */
