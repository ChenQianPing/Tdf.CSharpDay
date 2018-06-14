using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter7
{
    public class TestParallelLinq04
    {
        public static void TestMethod1()
        {
            ConcurrentQueue<int> products = new ConcurrentQueue<int>();
            /*向集合中添加多条数据*/
            Parallel.For(0, 6000000, (num) =>
            {
                products.Enqueue(num);
            });

            /*采用LINQ 返回 IEumerable<int>*/
            var productListLinq = (from product in products
                                   select product).Average();
            Console.WriteLine("采用Average计算平均值：{0}", productListLinq);

            /*采用PLINQ 返回 ParallelQuery<int>*/
            var productListPLinq = (from product in products.AsParallel()
                                    select product).Average();
            Console.WriteLine("采用Average计算平均值：{0}", productListPLinq);
            Console.ReadLine();
        }
    }
}

/*
 * 通过PLINQ执行归约操作

PLINQ可以简化对一个序列或者一个组中所有成员应用一个函数的过程，
这个过程称之为归约操作，
如在PLINQ查询中使用类似于Average,Max,Min,Sum之类的聚合函数就可以充分利用并行所带来好处。

并行执行的规约和串行执行的规约的执行结果可能会不同，
因为在操作不能同时满足可交换和可传递的情况下产生摄入，
在每次执行的时候，序列或组中的元素在不同并行任务中分布可能也会有区别，
因而在这种操作的情况下可能会产生不同的最终结果，
因此，一定要通过对于的串行版本来兴义原始的数据源，这样才能帮助PLINQ获得最优的执行结果。

如上述代码所示

在LINQ版本中，该方法会返回一个 IEumerable<int>，即调用 Eumerable.Range方法生成指定范围整数序列的结果，
在PLINQ版本中，该方法会返回一个 ParallelQuery<int>，即调用并行版本中System.Linq.ParallelEumerable的ParallelEumerable.Range方法，通过这种方法得到的结果序列也是并行序列，可以再PLINQ中并行运行。

如果想对特定数据源进行LINQ查询时，可以定义为  private IEquatable<int> products
如果想对特定数据源进行PLINQ查询时，可以定义为 private ParallelQuery<int> products

 */
