using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter5
{
    public class TestConcurrency05
    {
        private static object _o = new object();
        private static ConcurrentStack<Product> ConcurrenProducts { get; set; }
        public static void TestMethod1()
        {
            ConcurrenProducts = new ConcurrentStack<Product>();
            /*执行添加操作*/
            Console.WriteLine("执行添加操作");
            Parallel.Invoke(AddConcurrenProducts, AddConcurrenProducts);
            Console.WriteLine("ConcurrentStack<Product> 当前数据量为：" + ConcurrenProducts.Count);

            /*执行TryPeek操作   尝试返回不移除*/
            Console.WriteLine("执行TryPeek操作   尝试返回不移除");
            Parallel.Invoke(PeekConcurrenProducts, PeekConcurrenProducts);
            Console.WriteLine("ConcurrentStack<Product> 当前数据量为：" + ConcurrenProducts.Count);

            /*执行TryDequeue操作  尝试返回并移除*/
            Console.WriteLine("执行TryPop操作  尝试返回并移除");
            Parallel.Invoke(PopConcurrenProducts, PopConcurrenProducts);
            Console.WriteLine("ConcurrentStack<Product> 当前数据量为：" + ConcurrenProducts.Count);

            Console.ReadLine();
        }

        /// <summary>
        /// 执行集合数据添加操作
        /// </summary>
        static void AddConcurrenProducts()
        {
            Parallel.For(0, 100, (i) =>
            {
                var product = new Product
                {
                    Name = "name" + i,
                    Category = "Category" + i,
                    SellPrice = i
                };
                ConcurrenProducts.Push(product);
            });
        }

        /// <summary>
        /// 尝试返回但不移除
        /// </summary>
        static void PeekConcurrenProducts()
        {
            Parallel.For(0, 2, (i) =>
            {
                Product product = null;
                var excute = ConcurrenProducts.TryPeek(out product);
                Console.WriteLine(product.Name);
            });
        }

        /// <summary>
        /// 尝试返回并移除
        /// </summary>
        static void PopConcurrenProducts()
        {
            Parallel.For(0, 2, (i) =>
            {
                Product product = null;
                var excute = ConcurrenProducts.TryPop(out product);
                Console.WriteLine(product.Name);
            });
        }

    }
}


/*
 * ConcurrentStack 还有另外两种方法：TryPop 尝试移除并返回 和 TryPeek 尝试返回但不移除
 * 
 * 执行添加操作
ConcurrentStack<Product> 当前数据量为：200
执行TryPeek操作   尝试返回不移除
name99
name99
name99
name99

ConcurrentStack<Product> 当前数据量为：200
执行TryPop操作  尝试返回并移除
name99
name97
name98
name96

ConcurrentStack<Product> 当前数据量为：196

Tips：在实际的项目中用到安全集合，说实话是有遇到问题的，
没有直接用Lock省心；虽然lock的性能差。

 */
