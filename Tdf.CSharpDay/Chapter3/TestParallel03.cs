using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter3
{
    public class TestParallel03
    {
        public static void TestMethod1()
        {
            Thread.Sleep(3000);
            ForSetProcuct_100();

            Thread.Sleep(3000);
            ParallelForSetProcuct_100();
            Console.ReadLine();
        }

        private static void ForSetProcuct_100()
        {
            var sw = new Stopwatch();
            sw.Start();
            var productList = new List<Product>();
            for (var index = 1; index < 100; index++)
            {
                var model = new Product
                {
                    Category = "Category" + index,
                    Name = "Name" + index,
                    SellPrice = index
                };
                productList.Add(model);
                Console.WriteLine("for SetProcuct index: {0}", index);
            }
            sw.Stop();
            Console.WriteLine("for SetProcuct 10 执行完成 耗时：{0}", sw.ElapsedMilliseconds);
        }

        private static void ParallelForSetProcuct_100()
        {
            var sw = new Stopwatch();
            sw.Start();
            var productList = new List<Product>();
            Parallel.For(1, 100, index =>
            {
                var model = new Product
                {
                    Category = "Category" + index,
                    Name = "Name" + index,
                    SellPrice = index
                };
                productList.Add(model);
                Console.WriteLine("ForSetProcuct SetProcuct index: {0}", index);
            });
            sw.Stop();
            Console.WriteLine("ForSetProcuct SetProcuct 20000 执行完成 耗时：{0}", sw.ElapsedMilliseconds);
        }

    }
}


/*
 * for SetProcuct index: 1
for SetProcuct index: 2
for SetProcuct index: 3
for SetProcuct index: 4
for SetProcuct index: 5
for SetProcuct index: 6
for SetProcuct index: 7
for SetProcuct index: 8
for SetProcuct index: 9
for SetProcuct index: 10
for SetProcuct index: 11
for SetProcuct index: 12
for SetProcuct index: 13
for SetProcuct index: 14
for SetProcuct index: 15
for SetProcuct index: 16
for SetProcuct index: 17
for SetProcuct index: 18
for SetProcuct index: 19
for SetProcuct index: 20
for SetProcuct index: 21
for SetProcuct index: 22
for SetProcuct index: 23
for SetProcuct index: 24
for SetProcuct index: 25
for SetProcuct index: 26
for SetProcuct index: 27
for SetProcuct index: 28
for SetProcuct index: 29
for SetProcuct index: 30
for SetProcuct index: 31
for SetProcuct index: 32
for SetProcuct index: 33
for SetProcuct index: 34
for SetProcuct index: 35
for SetProcuct index: 36
for SetProcuct index: 37
for SetProcuct index: 38
for SetProcuct index: 39
for SetProcuct index: 40
for SetProcuct index: 41
for SetProcuct index: 42
for SetProcuct index: 43
for SetProcuct index: 44
for SetProcuct index: 45
for SetProcuct index: 46
for SetProcuct index: 47
for SetProcuct index: 48
for SetProcuct index: 49
for SetProcuct index: 50
for SetProcuct index: 51
for SetProcuct index: 52
for SetProcuct index: 53
for SetProcuct index: 54
for SetProcuct index: 55
for SetProcuct index: 56
for SetProcuct index: 57
for SetProcuct index: 58
for SetProcuct index: 59
for SetProcuct index: 60
for SetProcuct index: 61
for SetProcuct index: 62
for SetProcuct index: 63
for SetProcuct index: 64
for SetProcuct index: 65
for SetProcuct index: 66
for SetProcuct index: 67
for SetProcuct index: 68
for SetProcuct index: 69
for SetProcuct index: 70
for SetProcuct index: 71
for SetProcuct index: 72
for SetProcuct index: 73
for SetProcuct index: 74
for SetProcuct index: 75
for SetProcuct index: 76
for SetProcuct index: 77
for SetProcuct index: 78
for SetProcuct index: 79
for SetProcuct index: 80
for SetProcuct index: 81
for SetProcuct index: 82
for SetProcuct index: 83
for SetProcuct index: 84
for SetProcuct index: 85
for SetProcuct index: 86
for SetProcuct index: 87
for SetProcuct index: 88
for SetProcuct index: 89
for SetProcuct index: 90
for SetProcuct index: 91
for SetProcuct index: 92
for SetProcuct index: 93
for SetProcuct index: 94
for SetProcuct index: 95
for SetProcuct index: 96
for SetProcuct index: 97
for SetProcuct index: 98
for SetProcuct index: 99
for SetProcuct 10 执行完成 耗时：71

ForSetProcuct SetProcuct index: 1
ForSetProcuct SetProcuct index: 85
ForSetProcuct SetProcuct index: 61
ForSetProcuct SetProcuct index: 97
ForSetProcuct SetProcuct index: 49
ForSetProcuct SetProcuct index: 13
ForSetProcuct SetProcuct index: 14
ForSetProcuct SetProcuct index: 73
ForSetProcuct SetProcuct index: 37
ForSetProcuct SetProcuct index: 2
ForSetProcuct SetProcuct index: 25
ForSetProcuct SetProcuct index: 26
ForSetProcuct SetProcuct index: 29
ForSetProcuct SetProcuct index: 30
ForSetProcuct SetProcuct index: 31
ForSetProcuct SetProcuct index: 32
ForSetProcuct SetProcuct index: 33
ForSetProcuct SetProcuct index: 34
ForSetProcuct SetProcuct index: 35
ForSetProcuct SetProcuct index: 36
ForSetProcuct SetProcuct index: 40
ForSetProcuct SetProcuct index: 41
ForSetProcuct SetProcuct index: 42
ForSetProcuct SetProcuct index: 43
ForSetProcuct SetProcuct index: 44
ForSetProcuct SetProcuct index: 45
ForSetProcuct SetProcuct index: 46
ForSetProcuct SetProcuct index: 47
ForSetProcuct SetProcuct index: 48
ForSetProcuct SetProcuct index: 52
ForSetProcuct SetProcuct index: 53
ForSetProcuct SetProcuct index: 54
ForSetProcuct SetProcuct index: 55
ForSetProcuct SetProcuct index: 56
ForSetProcuct SetProcuct index: 57
ForSetProcuct SetProcuct index: 98
ForSetProcuct SetProcuct index: 99
ForSetProcuct SetProcuct index: 7
ForSetProcuct SetProcuct index: 8
ForSetProcuct SetProcuct index: 9
ForSetProcuct SetProcuct index: 10
ForSetProcuct SetProcuct index: 11
ForSetProcuct SetProcuct index: 12
ForSetProcuct SetProcuct index: 19
ForSetProcuct SetProcuct index: 20
ForSetProcuct SetProcuct index: 21
ForSetProcuct SetProcuct index: 22
ForSetProcuct SetProcuct index: 23
ForSetProcuct SetProcuct index: 24
ForSetProcuct SetProcuct index: 64
ForSetProcuct SetProcuct index: 65
ForSetProcuct SetProcuct index: 66
ForSetProcuct SetProcuct index: 67
ForSetProcuct SetProcuct index: 68
ForSetProcuct SetProcuct index: 69
ForSetProcuct SetProcuct index: 70
ForSetProcuct SetProcuct index: 50
ForSetProcuct SetProcuct index: 58
ForSetProcuct SetProcuct index: 15
ForSetProcuct SetProcuct index: 3
ForSetProcuct SetProcuct index: 4
ForSetProcuct SetProcuct index: 76
ForSetProcuct SetProcuct index: 77
ForSetProcuct SetProcuct index: 78
ForSetProcuct SetProcuct index: 79
ForSetProcuct SetProcuct index: 80
ForSetProcuct SetProcuct index: 81
ForSetProcuct SetProcuct index: 82
ForSetProcuct SetProcuct index: 83
ForSetProcuct SetProcuct index: 84
ForSetProcuct SetProcuct index: 88
ForSetProcuct SetProcuct index: 89
ForSetProcuct SetProcuct index: 90
ForSetProcuct SetProcuct index: 91
ForSetProcuct SetProcuct index: 92
ForSetProcuct SetProcuct index: 93
ForSetProcuct SetProcuct index: 94
ForSetProcuct SetProcuct index: 95
ForSetProcuct SetProcuct index: 96
ForSetProcuct SetProcuct index: 71
ForSetProcuct SetProcuct index: 72
ForSetProcuct SetProcuct index: 27
ForSetProcuct SetProcuct index: 74
ForSetProcuct SetProcuct index: 75
ForSetProcuct SetProcuct index: 16
ForSetProcuct SetProcuct index: 62
ForSetProcuct SetProcuct index: 38
ForSetProcuct SetProcuct index: 39
ForSetProcuct SetProcuct index: 5
ForSetProcuct SetProcuct index: 6
ForSetProcuct SetProcuct index: 51
ForSetProcuct SetProcuct index: 28
ForSetProcuct SetProcuct index: 63
ForSetProcuct SetProcuct index: 59
ForSetProcuct SetProcuct index: 86
ForSetProcuct SetProcuct index: 17
ForSetProcuct SetProcuct index: 18
ForSetProcuct SetProcuct index: 60
ForSetProcuct SetProcuct index: 87
ForSetProcuct SetProcuct 20000 执行完成 耗时：2419

    由图中我们可以看出，使用Parallel.For所迭代的顺序是无法保证的。


 */
