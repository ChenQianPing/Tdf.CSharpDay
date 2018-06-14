using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter4
{
    public class TestTask02
    {
        private static ConcurrentQueue<Product> _queue = null;

        public static void TestMethod1()
        {
            _queue = new ConcurrentQueue<Product>();

            var tk1 = new Task(() =>
            {
                SetProduct(1);
                SetProduct(3);
            });

            var tk2 = new Task(() => SetProduct(2));

            tk1.Start();
            tk2.Start();


            Console.ReadLine();
        }

        public static void TestMethod2()
        {
            _queue = new ConcurrentQueue<Product>();
            Task tk1 = new Task(() =>
            {
                SetProduct(1);
                SetProduct(3);
            });

            Task tk2 = new Task(() => SetProduct(2));

            tk1.Start();
            tk2.Start();
            /*等待任务执行完成后再输出 ====== */
            Task.WaitAll(tk1, tk2);
            Console.WriteLine("等待任务执行完成后再输出 ======");

            Task tk3 = new Task(() =>
            {
                SetProduct(1);
                SetProduct(3);
            });

            Task tk4 = new Task(() => SetProduct(2));
            tk3.Start();
            tk4.Start();
            /*等待任务执行前输出 ====== */
            Console.WriteLine("等待任务执行前输出 ======");
            Task.WaitAll(tk3, tk4);

            Console.ReadLine();
        }

        public static void TestMethod3()
        {
            _queue = new ConcurrentQueue<Product>();
            Task tk1 = new Task(() => { SetProduct(1); SetProduct(3); });
            Task tk2 = new Task(() => SetProduct(2));
            tk1.Start();
            tk2.Start();

            /*
             * Task.WaitAll 限定等待时长
             * 如果tk1 tk2 没能在10毫秒内完成 则输出 *****  */
            if (!Task.WaitAll(new Task[] { tk1, tk2 }, 10))
            {
                Console.WriteLine("******");
            }

            Console.ReadLine();
        }

        static void SetProduct(int index)
        {
            Parallel.For(0, 10000, (i) =>
            {
                Product model = new Product
                {
                    Name = "Name" + i,
                    SellPrice = i,
                    Category = "Category" + i
                };
                _queue.Enqueue(model);
            });
            Console.WriteLine("SetProduct {0} 执行完成", index);
        }


    }
}


/*
 * 使用任务来对代码进行并行化

使用Parallel.Invoke可以并行加载多个方法，使用Task实例也能完成同样的工作

 * SetProduct 2 执行完成
SetProduct 1 执行完成
SetProduct 3 执行完成

等待任务完成Task.WaitAll
Task.WaitAll 方法，这个方法是同步执行的，在Task作为参数被接受，
所有Task结束其执行前，主线程不会继续执行下一条指令

SetProduct 2 执行完成
SetProduct 1 执行完成
SetProduct 3 执行完成
等待任务执行完成后再输出 ======
等待任务执行前输出 ======
SetProduct 1 执行完成
SetProduct 2 执行完成
SetProduct 3 执行完成

Task.WaitAll 限定等待时长
10毫秒没有完成任务，则输出了****

******
SetProduct 1 执行完成
SetProduct 2 执行完成
SetProduct 3 执行完成
 */
