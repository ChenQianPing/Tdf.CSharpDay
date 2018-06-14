using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter3
{
    public class TestParallel04
    {
        public static void TestMethod1()
        {
            var productList = GetProcuctList();

            Parallel.ForEach(productList, (model) => {
                Console.WriteLine(model.Name);
            });
            Console.ReadLine();
        }

        private static List<Product> GetProcuctList()
        {
            var result = new List<Product>();
            for (var index = 1; index < 100; index++)
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
 * Name1
Name2
Name3
Name4
Name5
Name6
Name7
Name8
Name9
Name10
Name13
Name14
Name15
Name16
Name17
Name18
Name19
Name20
Name21
Name22
Name23
Name24
Name26
Name27
Name28
Name29
Name30
Name31
Name32
Name33
Name34
Name35
Name37
Name25
Name40
Name41
Name42
Name43
Name44
Name45
Name46
Name47
Name48
Name50
Name51
Name52
Name53
Name54
Name55
Name56
Name57
Name58
Name59
Name60
Name62
Name63
Name64
Name65
Name66
Name67
Name38
Name39
Name74
Name75
Name76
Name77
Name36
Name86
Name87
Name88
Name89
Name90
Name91
Name92
Name93
Name94
Name95
Name96
Name98
Name99
Name85
Name11
Name12
Name97
Name61
Name68
Name69
Name70
Name71
Name72
Name49
Name73
Name78
Name79
Name80
Name81
Name82
Name83
Name84

Parallel.ForEach提供一个并行处理一组数据的机制，
可以利用一个范围的整数作为一组数据，
然后通过一个自定义的分区器将这个范围转换为一组数据块，
每一块数据都通过循环的方式进行处理，而这些循环式并行执行的。


 */
