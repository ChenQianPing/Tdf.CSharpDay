using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter3
{
    public class TestParallel01
    {
        private static List<Product> _productList = null;

        public static void TestMethod1()
        {
            _productList = new List<Product>();
            Thread.Sleep(3000);

            var swTask = new Stopwatch();
            swTask.Start();

            // 执行并行操作
            Parallel.Invoke(SetProcuct1_500, SetProcuct2_500, SetProcuct3_500, SetProcuct4_500);
            swTask.Stop();
            Console.WriteLine("500条数据 并行编程所耗时间:" + swTask.ElapsedMilliseconds);

            _productList = new List<Product>();
            Thread.Sleep(3000);  // 防止并行操作与顺序操作冲突
            Stopwatch sw = new Stopwatch();
            sw.Start();
            SetProcuct1_500();
            SetProcuct2_500();
            SetProcuct3_500();
            SetProcuct4_500();
            sw.Stop();
            Console.WriteLine("500条数据  顺序编程所耗时间:" + sw.ElapsedMilliseconds);

            _productList = new List<Product>();
            Thread.Sleep(3000);
            swTask.Restart();
            // 执行并行操作
            Parallel.Invoke(SetProcuct1_10000, SetProcuct2_10000, SetProcuct3_10000, SetProcuct4_10000);
            swTask.Stop();
            Console.WriteLine("10000条数据 并行编程所耗时间:" + swTask.ElapsedMilliseconds);

            _productList = new List<Product>();
            Thread.Sleep(3000);
            sw.Restart();
            SetProcuct1_10000();
            SetProcuct2_10000();
            SetProcuct3_10000();
            SetProcuct4_10000();
            sw.Stop();
            Console.WriteLine("10000条数据 顺序编程所耗时间:" + sw.ElapsedMilliseconds);

            Console.ReadLine();
        }

        private static void SetProcuct1_500()
        {
            for (var index = 1; index < 500; index++)
            {
                var model = new Product
                {
                    Category = "Category" + index,
                    Name = "Name" + index,
                    SellPrice = index
                };
                _productList.Add(model);
            }
            Console.WriteLine("SetProcuct1 执行完成");
        }

        private static void SetProcuct2_500()
        {
            for (var index = 500; index < 1000; index++)
            {
                var model = new Product
                {
                    Category = "Category" + index,
                    Name = "Name" + index,
                    SellPrice = index
                };
                _productList.Add(model);
            }
            Console.WriteLine("SetProcuct2 执行完成");
        }

        private static void SetProcuct3_500()
        {
            for (var index = 1000; index < 2000; index++)
            {
                var model = new Product
                {
                    Category = "Category" + index,
                    Name = "Name" + index,
                    SellPrice = index
                };
                _productList.Add(model);
            }
            Console.WriteLine("SetProcuct3 执行完成");
        }

        private static void SetProcuct4_500()
        {
            for (var index = 2000; index < 3000; index++)
            {
                var model = new Product
                {
                    Category = "Category" + index,
                    Name = "Name" + index,
                    SellPrice = index
                };
                _productList.Add(model);
            }
            Console.WriteLine("SetProcuct4 执行完成");
        }


        private static void SetProcuct1_10000()
        {
            for (var index = 1; index < 20000; index++)
            {
                var model = new Product
                {
                    Category = "Category" + index,
                    Name = "Name" + index,
                    SellPrice = index
                };
                _productList.Add(model);
            }
            Console.WriteLine("SetProcuct1 执行完成");
        }

        private static void SetProcuct2_10000()
        {
            for (var index = 20000; index < 40000; index++)
            {
                var model = new Product
                {
                    Category = "Category" + index,
                    Name = "Name" + index,
                    SellPrice = index
                };
                _productList.Add(model);
            }
            Console.WriteLine("SetProcuct2 执行完成");
        }

        private static void SetProcuct3_10000()
        {
            for (var index = 40000; index < 60000; index++)
            {
                var model = new Product
                {
                    Category = "Category" + index,
                    Name = "Name" + index,
                    SellPrice = index
                };
                _productList.Add(model);
            }
            Console.WriteLine("SetProcuct3 执行完成");
        }

        private static void SetProcuct4_10000()
        {
            for (var index = 60000; index < 80000; index++)
            {
                var model = new Product
                {
                    Category = "Category" + index,
                    Name = "Name" + index,
                    SellPrice = index
                };
                _productList.Add(model);
            }
            Console.WriteLine("SetProcuct4 执行完成");
        }
    }

    
}


/*
 * SetProcuct2 执行完成
SetProcuct1 执行完成
SetProcuct4 执行完成
SetProcuct3 执行完成
500条数据 并行编程所耗时间:32
SetProcuct1 执行完成
SetProcuct2 执行完成
SetProcuct3 执行完成
SetProcuct4 执行完成
500条数据  顺序编程所耗时间:5
SetProcuct4 执行完成
SetProcuct1 执行完成
SetProcuct2 执行完成
SetProcuct3 执行完成
10000条数据 并行编程所耗时间:64
SetProcuct1 执行完成
SetProcuct2 执行完成
SetProcuct3 执行完成
SetProcuct4 执行完成
10000条数据 顺序编程所耗时间:101

图中我们可以看出利用 Parallel.Invoke编写的并发执行代，它的并发执行顺序也是不定的。
但是所执行的时间上比不采用并行编程所耗的时间差不多。

这是因为我们在并行编程中操作了共享资源 _productList
 */
