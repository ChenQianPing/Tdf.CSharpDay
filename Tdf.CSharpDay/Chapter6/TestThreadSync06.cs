using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter6
{
    public class TestThreadSync06
    {
        private static Task[] CookTasks { get; set; }
        /*定义一个变量 该变量指示是否可以进行下一步操作*/
        private static bool _stepbool = false;
        /*获取当前计算机处理器数*/
        private static readonly int Particpants = Environment.ProcessorCount;

        public static void TestMethod1()
        {
            CookTasks = new Task[Particpants];
            for (var taskIndex = 0; taskIndex < Particpants; taskIndex++)
            {
                CookTasks[taskIndex] = Task.Factory.StartNew((num) =>
                {
                    CookStep1();
                    /*等待5秒钟 _stepbool变为true ，如果5秒钟内没有淘好米 则提示超时*/
                    if (!SpinWait.SpinUntil(() => (_stepbool), 1000))
                    {
                        Console.WriteLine("淘个米都花这么长时间....");
                    }
                    else
                    {
                        /*按时淘好米开始煮饭*/
                        Console.WriteLine("淘好米煮饭....");
                    }
                }, taskIndex);
            }
            /*主线程创造超时条件*/
            Thread.Sleep(3000);
            _stepbool = true;

            Console.ReadLine();
        }

        static void CookStep1()
        {
            Console.WriteLine("淘米....");
        }
    }
}

/*
 * 淘米....
淘米....
淘米....
淘米....
淘米....
淘米....
淘米....
淘米....
淘个米都花这么长时间....
淘个米都花这么长时间....
淘个米都花这么长时间....
淘个米都花这么长时间....
淘个米都花这么长时间....
淘个米都花这么长时间....
淘个米都花这么长时间....
淘个米都花这么长时间....

基于自旋锁的等待-System.Threading.SpinWait
如果等待某个条件满足需要的时间很短，而且不希望发生昂贵的上下文切换，
那么基于自旋的等待时一种很好的替换方案，
SpinWait不仅提供了基本自旋功能，而且还提供了SpinWait.SpinUntil方法，
使用这个方法能够自旋直到满足某个条件为止，
此外SpinWait是一个Struct，从内存的角度上说，开销很小。SpinLock是对SpinWait的简单封装。

需要注意的是：长时间的自旋不是很好的做法，因为自旋会阻塞更高级的线程及其相关的任务，
还会阻塞垃圾回收机制。SpinWait并没有设计为让多个任务或线程并发使用，
因此多个任务或线程通过SpinWait方法进行自旋，
那么每一个任务或线程都应该使用自己的SpinWait实例。

当一个线程自旋时，会将一个内核放入到一个繁忙的循环中，
而不会让出当前处理器时间片剩余部分，当一个任务或者线程调用Thread.Sleep方法时，
底层线程可能会让出当前处理器时间片的剩余部分，这是一个大开销的操作。

因此，在大部分情况下，不要在循环内调用Thread.Sleep方法等待特定的条件满足。
 */
