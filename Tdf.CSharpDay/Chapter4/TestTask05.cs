using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter4
{
    public class TestTask05
    {
        public static void TestMethod1()
        {
            var tk1 = Task<List<Product>>.Factory.StartNew(SetProduct);
            Task.WaitAll(tk1);

            Console.WriteLine(tk1.Result.Count);
            Console.WriteLine(tk1.Result[0].Name);
            Console.ReadLine();
        }

        static List<Product> SetProduct()
        {
            var result = new List<Product>();
            for (var i = 0; i < 500; i++)
            {
                var model = new Product
                {
                    Name = "Name" + i,
                    SellPrice = i,
                    Category = "Category" + i
                };
                result.Add(model);
            }
            Console.WriteLine("SetProduct 执行完成");
            return result;
        }

    }
}

/*
 * Task返回值  Task<TResult>

 * 
 * SetProduct 执行完成
500
Name0

 */
