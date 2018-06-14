using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter5
{
    public class TestConcurrency03
    {
        private static object _o = new object();
        private static ConcurrentQueue<Product> ConcurrenProducts { get; set; }

        public static void TestMethod1()
        {
            ConcurrenProducts = new ConcurrentQueue<Product>();
            /*执行添加操作*/
            Console.WriteLine("执行添加操作");
            Parallel.Invoke(AddConcurrenProducts, AddConcurrenProducts);
            Console.WriteLine("ConcurrentQueue<Product> 当前数据量为：" + ConcurrenProducts.Count);
            /*执行TryPeek操作   尝试返回不移除*/
            Console.WriteLine("执行TryPeek操作   尝试返回不移除");
            Parallel.Invoke(PeekConcurrenProducts, PeekConcurrenProducts);
            Console.WriteLine("ConcurrentQueue<Product> 当前数据量为：" + ConcurrenProducts.Count);

            /*执行TryDequeue操作  尝试返回并移除*/
            Console.WriteLine("执行TryDequeue操作  尝试返回并移除");
            Parallel.Invoke(DequeueConcurrenProducts, DequeueConcurrenProducts);
            Console.WriteLine("ConcurrentQueue<Product> 当前数据量为：" + ConcurrenProducts.Count);

            Console.ReadLine();
        }

        /*执行集合数据添加操作*/
        static void AddConcurrenProducts()
        {
            Parallel.For(0, 100, (i) =>
            {
                Product product = new Product();
                product.Name = "name" + i;
                product.Category = "Category" + i;
                product.SellPrice = i;
                ConcurrenProducts.Enqueue(product);
            });
        }

        /*尝试返回 但不移除*/
        static void PeekConcurrenProducts()
        {
            Parallel.For(0, 2, (i) =>
            {
                Product product = null;
                bool excute = ConcurrenProducts.TryPeek(out product);
                Console.WriteLine(product.Name);
            });
        }

        /*尝试返回 并 移除*/
        static void DequeueConcurrenProducts()
        {
            Parallel.For(0, 2, (i) =>
            {
                Product product = null;
                bool excute = ConcurrenProducts.TryDequeue(out product);
                Console.WriteLine(product.Name);
            });
        }
    }
}

/*
 * 
 * ConcurrentQueue 还有另外两种方法：TryDequeue  尝试移除并返回 
 * 和 TryPeek 尝试返回但不移除，
 * 
 * 需要注意 TryDequeue  和  TryPeek 的无序性，在多线程下

 * 执行添加操作
ConcurrentQueue<Product> 当前数据量为：200
执行TryPeek操作   尝试返回不移除
name0
name0
name0
name0

ConcurrentQueue<Product> 当前数据量为：200
执行TryDequeue操作  尝试返回并移除
name0
name0
name1
name1

ConcurrentQueue<Product> 当前数据量为：196

 */
