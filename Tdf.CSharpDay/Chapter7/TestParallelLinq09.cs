using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter7
{
    public class TestParallelLinq09
    {
        public static void TestMethod1()
        {
            ConcurrentQueue<string> list = new ConcurrentQueue<string>();
            list.Enqueue("A");
            list.Enqueue("B");
            list.Enqueue("C");
            list.Enqueue("D");
            list.Enqueue("A");
            list.Enqueue("D");

            Console.WriteLine("Select.......");
            list.AsParallel().Select(p => new
            {
                Name = p,
                Count = 1
            }).ForAll((p) =>
            {
                Console.WriteLine("{0}\t{1}", p.Name, p.Count);
            });

            Console.WriteLine("ILookup.......");
            /*map操作生成的键值对由一个单词和数量1组成，该代码意在将每个单词作为键并将1作为值加入*/
            ILookup<string, int> map = list.AsParallel().ToLookup(p => p, k => 1);
            foreach (var v in map)
            {
                Console.Write(v.Key);
                foreach (int val in v)
                    Console.WriteLine("\t{0}", val);
            }
            /*reduce操作单词出现的次数*/
            var reduce = from IGrouping<string, int> reduceM in map.AsQueryable()
                         select new
                         {
                             key = reduceM.Key,
                             count = reduceM.Count()
                         };
            Console.WriteLine("IGrouping.......");
            foreach (var v in reduce)
            {
                Console.Write(v.key);
                Console.WriteLine("\t{0}", v.count);
            }

            Console.ReadLine();
        }
    }
}


/*
 * Select.......
A       1
A       1
D       1
C       1
B       1
D       1
ILookup.......
D       1
        1
C       1
B       1
A       1
        1
IGrouping.......
D       2
C       1
B       1
A       2

    使用PLINQ执行MapReduce算法 ILookup IGrouping

mapreduce ，也称为Map/reduce 或者Map&Reduce ,是一种非常流行的框架，能够充分利用并行化处理巨大的数据集，MapReduce的基本思想非常简单：将数据处理问题分解为以下两个独立且可以并行执行的操作：

映射（Map）-对数据源进行操作，为每一个数据项计算出一个键值对。运行的结果是一个键值对的集合，根据键进行分组。
规约（Reduce）-对映射操作产生的根据键进行分组的所有键值对进行操作，对每一个组执行归约操作，这个操作可以返回一个或多个值。

下面贴代码，方便大家理解，但是该案列所展示的并不是一个纯粹的MapReduce算法实现：
 */
