using System;

namespace Tdf.CSharpDay.Chapter12
{
    /// <summary>
    /// 验证int类型属性长度的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]   // 表示该特性只能作用于属性上
    public class MyValidateAttribute : Attribute
    {
        private readonly int _min = 0;
        private readonly int _max = 0;

        /// <summary>
        /// 自定义构造函数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public MyValidateAttribute(int min, int max)
        {
            this._min = min;
            this._max = max;
        }

        /// <summary>
        /// 封装校验是否合法的方法
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool CheckIsRational(int num)
        {
            return num >= this._min && num <= this._max;
        }

    }
}
