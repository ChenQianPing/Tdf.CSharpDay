using System;
using System.Collections.Generic;
using System.Linq;

namespace Tdf.CSharpDay.Chapter2
{
    public class TestLinQ
    {
        public static void TestMethod1()
        {
            // 初始化点测试数据

            var user1 = new User()
            {
                Id = 1,
                Name = "荀彧",
                Address = "河南许昌",
                Phone = "13700673590",
                Age = 49,
                Sex = "男",
                SchId = 1
            };

            var user2 = new User()
            {
                Id = 2,
                Name = "甄宓",
                Address = "河北省无极县",
                Phone = "13700673591",
                Age = 38,
                Sex = "女",
                SchId = 1
            };

            var user3 = new User()
            {
                Id = 3,
                Name = "荀攸",
                Address = "河南许昌",
                Phone = "13700673592",
                Age = 57,
                Sex = "男",
                SchId = 1
            };

            var user4 = new User()
            {
                Id = 4,
                Name = "郭嘉",
                Address = "河南禹州",
                Phone = "13700673593",
                Age = 37,
                Sex = "男",
                SchId = 1
            };

            var lstUsers = new List<User>(4) {user1, user2, user3, user4};

            var lstSchools = new List<School>()
            {
                new School(1, "武汉大学"),
                new School(2, "华中科技大学"),
                new School(3, "华中师范大学")
            };

            // 求和
            var sum1 = lstUsers.Where(a => a.Id > 0).Sum(a => a.Id);
            Console.WriteLine(sum1);

            var sum2 = (from a in lstUsers where a.Id > 0 select a.Id).Sum();
            Console.WriteLine(sum2);

            // 求最大值
            var max = lstUsers.Max(a => a.Id);
            Console.WriteLine(max);

            // 最小值
            var min = lstUsers.Min(a => a.Id);
            Console.WriteLine(min);

            // 循环输出
            lstUsers.ForEach(a =>
            {
                if (a.Age > 20)
                {
                    Console.WriteLine(a.Id);
                }

            });

            // 条件筛选，查询不到会报错
            var user = lstUsers.Single(a => a.Id == 1);

            // 筛选所有男性用户
            var lstTemp = lstUsers.Where(a => a.Sex == "男").ToList();

            // 降序
            lstTemp = lstUsers.OrderByDescending(a => a.Id).ToList();

            // 升序
            lstTemp = lstUsers.OrderBy(a => a.Id).ToList();
            
            // 分组ToLookup
            var lookup = lstUsers.ToLookup(a => a.Sex);
            foreach (var item in lookup)
            {
                Console.WriteLine(item.Key);
                foreach (var sub in item)
                {
                    Console.WriteLine("\t\t" + sub.Name + " " + sub.Age);
                }
            }

            // 另一种GroupBy
            var dic = lstUsers.GroupBy(a => a.Sex);
            foreach (var item in dic)
            {
                Console.WriteLine(item.Key);
                foreach (var sub in item)
                {
                    Console.WriteLine("\t\t" + sub.Name + " " + sub.Age);
                }
            }

            // 联接，这里只展示内联接
            var temp = from usertemp in lstUsers
                join sch in lstSchools on usertemp.SchId equals sch.SchId
                select new {Id = usertemp.Id, Name = usertemp.Name, Age = usertemp.Age, Schname = sch.SchName};

            // 类型查找 OfType
            var lstObjs = new List<object>() {1, "2", false, "s", new User() {Id = 1, Name = "xx"}};

            IEnumerable<string> query1 = lstObjs.OfType<string>();

            foreach (var fruit in query1)
            {
                Console.WriteLine(fruit);
            }

            // 查找深层嵌套
            // 初始化数据
            var chinaMobile = new Company("中国移动", lstUsers);
            var chinaUnicom = new Company("中国联通", lstUsers);
            var lstCompanys = new List<Company>() {chinaMobile, chinaUnicom};

            // 找出2个公司所有女性成员
            var selectlist = lstCompanys.SelectMany(a => a.Users).Where(b => b.Sex == "女");
            foreach (var item in selectlist)
            {
                Console.WriteLine(item.Name + ":" + item.Sex);
            }

            // 正常情况下要想取得这数据的话，要经过2层嵌套循环
            foreach (var c in lstCompanys)
            {
                foreach (var item in c.Users)
                {
                    if (item.Sex == "女")
                    {
                        Console.WriteLine(item.Name + ":" + item.Sex);
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
