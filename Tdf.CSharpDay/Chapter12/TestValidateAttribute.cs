using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter12
{
    public class TestValidateAttribute
    {
        public static void TestMethod1()
        {
            // 制作一个可以限制int类型属性长度的特性，并封装对应的校验方法
            UserModel userModel = new UserModel();
            // userModel.Id = 1000;
            userModel.Id = 1001;  // 不合法
            Save<UserModel>(userModel);

            Console.ReadLine();
        }

        /// <summary>
        /// 校验并保存的方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void Save<T>(T t)
        {
            bool isSafe = true;
            // 1. 获取实例t所在的类
            Type type = t.GetType();
            // 2. type.GetProperties() 获取类中的所有属性
            foreach (var property in type.GetProperties())
            {
                // 3. 获取该属性上的所有特性
                object[] attributesList = property.GetCustomAttributes(true);
                // 4. 找属性中的特性
                foreach (var item in attributesList)
                {
                    if (item is MyValidateAttribute)
                    {
                        MyValidateAttribute attribute = item as MyValidateAttribute;
                        // 调用特性中的校验方法
                        // 表示获取属性的值：property.GetValue(t)
                        isSafe = attribute.CheckIsRational((int)property.GetValue(t));
                    }
                }
                if (!isSafe)
                {
                    break;
                }
            }
            if (isSafe)
            {
                Console.WriteLine("保存到数据库");
            }
            else
            {
                Console.WriteLine("数据不合法");
            }
        }

    }

    public class UserModel
    {
        [MyValidate(10, 1000)]
        public int Id { get; set; }
    }
}
