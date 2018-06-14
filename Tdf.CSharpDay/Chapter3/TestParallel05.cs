using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter3
{
    public class TestParallel05
    {
        public static void TestMethod1()
        {
            var productList = GetProcuctList_500();
            Thread.Sleep(1000);
            Parallel.For(0, productList.Count, (i, loopState) =>
            {
                if (i < 100)
                {
                    Console.WriteLine("采用Stop index:{0}", i);
                }
                else
                {
                    /* 满足条件后 尽快停止执行，无法保证小于100的索引数据全部输出*/
                    loopState.Stop();
                    return;
                }
            });

            Thread.Sleep(1000);
            Parallel.For(0, productList.Count, (i, loopState) =>
            {
                if (i < 100)
                {
                    Console.WriteLine("采用Break index:{0}", i);
                }
                else
                {
                    /* 满足条件后 尽快停止执行，保证小于100的索引数据全部输出*/
                    loopState.Break();
                    return;
                }
            });

            Thread.Sleep(1000);
            Parallel.ForEach(productList, (model, loopState) =>
            {
                if (model.SellPrice < 10)
                {
                    Console.WriteLine("采用Stop index:{0}", model.SellPrice);
                }
                else
                {
                    /* 满足条件后 尽快停止执行，无法保证满足条件的数据全部输出*/
                    loopState.Stop();
                    return;
                }
            });

            Thread.Sleep(1000);
            Parallel.ForEach(productList, (model, loopState) =>
            {
                if (model.SellPrice < 10)
                {
                    Console.WriteLine("采用Break index:{0}", model.SellPrice);
                }
                else
                {
                    /* 满足条件后 尽快停止执行，保证满足条件的数据全部输出 */
                    loopState.Break();
                    return;
                }
            });

            Console.WriteLine("执行完毕");

            Console.ReadLine();
        }

        private static List<Product> GetProcuctList_500()
        {
            var result = new List<Product>();
            for (var index = 1; index < 500; index++)
            {
                var model = new Product
                {
                    Category = "Category" + index,
                    Name = "Name" + index,
                    SellPrice = index
                };
                result.Add(model);
            }
            return result;
        }


    }
}


/*
 * 采用Stop index:0
采用Stop index:1
采用Stop index:62
采用Stop index:63
采用Stop index:64
采用Stop index:65
采用Stop index:66
采用Stop index:67
采用Stop index:68
采用Stop index:69
采用Stop index:70
采用Stop index:71
采用Stop index:72
采用Stop index:73
采用Stop index:74
采用Stop index:75
采用Stop index:76
采用Stop index:77
采用Stop index:78
采用Stop index:79
采用Stop index:80
采用Stop index:81
采用Stop index:2

采用Break index:0
采用Break index:1
采用Break index:2
采用Break index:3
采用Break index:4
采用Break index:5
采用Break index:6
采用Break index:7
采用Break index:8
采用Break index:9
采用Break index:10
采用Break index:11
采用Break index:12
采用Break index:13
采用Break index:14
采用Break index:15
采用Break index:16
采用Break index:17
采用Break index:18
采用Break index:19
采用Break index:20
采用Break index:21
采用Break index:22
采用Break index:23
采用Break index:24
采用Break index:25
采用Break index:62
采用Break index:63
采用Break index:64
采用Break index:65
采用Break index:66
采用Break index:67
采用Break index:68
采用Break index:69
采用Break index:70
采用Break index:71
采用Break index:72
采用Break index:73
采用Break index:74
采用Break index:75
采用Break index:76
采用Break index:77
采用Break index:78
采用Break index:79
采用Break index:80
采用Break index:81
采用Break index:82
采用Break index:83
采用Break index:84
采用Break index:85
采用Break index:86
采用Break index:87
采用Break index:88
采用Break index:89
采用Break index:90
采用Break index:91
采用Break index:92
采用Break index:93
采用Break index:94
采用Break index:95
采用Break index:96
采用Break index:26
采用Break index:27
采用Break index:28
采用Break index:29
采用Break index:30
采用Break index:31
采用Break index:32
采用Break index:33
采用Break index:34
采用Break index:35
采用Break index:36
采用Break index:37
采用Break index:38
采用Break index:39
采用Break index:40
采用Break index:41
采用Break index:42
采用Break index:43
采用Break index:44
采用Break index:45
采用Break index:97
采用Break index:98
采用Break index:99
采用Break index:46
采用Break index:47
采用Break index:48
采用Break index:49
采用Break index:50
采用Break index:51
采用Break index:52
采用Break index:53
采用Break index:54
采用Break index:55
采用Break index:56
采用Break index:57
采用Break index:58
采用Break index:59
采用Break index:60
采用Break index:61

采用Stop index:1

采用Break index:1
采用Break index:2
采用Break index:3
采用Break index:4
采用Break index:5
采用Break index:6
采用Break index:7
采用Break index:8
采用Break index:9

执行完毕

ParallelLoopState

ParallelLoopState该实例提供了以下两个方法用于停止 Parallel.For，Parallel.ForEach

Break-这个方法告诉并行循环应该在执行了当前迭代后尽快地停止执行。
吐过调用Break时正在处理迭代100，那么循环仍然会处理所有小于100的迭代。

Stop-这个方法告诉并行循环应该尽快停止执行，如果调用Stop时迭代100正在被处理，
那么循环无法保证处理完所有小于100的迭代
 */
