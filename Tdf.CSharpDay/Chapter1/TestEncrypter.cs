using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter1
{
    public class TestEncrypter
    {
        public static void TestMethod1()
        {
            string input = "abc";
            input = "银行密码统统都给我";

            string key = "justdoit";
            string result = string.Empty;

            // MD5加密结果：ee1124a603a7d6b5a8b352cf9635234d
            result = Encrypter.EncryptByMd5(input);
            Console.WriteLine("MD5加密结果：{0}", result);

            // SHA1加密结果：85-9D-97-0F-55-E7-A0-4C-60-CE-37-BE-0D-51-F0-F1-23-DD-C5-52
            result = Encrypter.EncryptBySha1(input);
            Console.WriteLine("SHA1加密结果：{0}", result);

            // DES加密结果：64-22-8B-65-BB-C3-15-E1-87-C7-28-A8-C5-75-CF-80-12-A3-57-A7-E8-2D-FC - 81 - 1A - 2C - 5F - 86 - 0A - 4A - 6E-37
            result = Encrypter.EncryptString(input, key);
            Console.WriteLine("DES加密结果：{0}", result);
            
            result = Encrypter.DecryptString(result, key);
            Console.WriteLine("DES解密结果：{0}", result);

            // DES加密结果：ZCKLZbvDFeGHxyioxXXPgBKjV6foLfyBGixfhgpKbjc=
            result = Encrypter.EncryptByDes(input, key);
            Console.WriteLine("DES加密结果：{0}", result);

            result = Encrypter.DecryptByDes(result, key);
            Console.WriteLine("DES解密结果：{0}", result); //结果："银行密码统统都给我�\nJn7"，与明文不一致，为什么呢？在加密后，通过base64编码转为字符串，可能是这个问题。

            key = "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111";

            // AES加密结果：AB-60-0A-59-FA-02-19-32-81-2839-6A-75-A4-8C-E5-3D-3B-2A-A4-3A-F7-53 - 7E-17 - A9 - 5F - 35 - 68 - 88 - 84 - 9D
            result = Encrypter.EncryptByAes(input, key);
            Console.WriteLine("AES加密结果：{0}", result);

            result = Encrypter.DecryptByAes(result, key);
            Console.WriteLine("AES解密结果：{0}", result);

            KeyValuePair<string, string> keyPair = Encrypter.CreateRsaKey();
            var privateKey = keyPair.Value;
            var publicKey = keyPair.Key;

            /*
             * 应该是公钥加密，私钥解密的说
             * RSA私钥加密后的结果：P1403dWvJczLwUjP3ubkzgxLVdW/GE4YqC2oys5j6fXRagNBMxRcKm5Iz1/
                kg2AkewVMBBLpP91g3AbSNE4agykr3hS6PKhc+o9ZdJIKiVOZtZchkotETAtSzkTLxw7LL30tLgqe5pE
                zA5QrXkQf4+fv/htwSrKZr3o17GCxUzE=
             */
            result = Encrypter.EncryptByRsa(input, publicKey);
            Console.WriteLine("RSA私钥加密后的结果：{0}", result);

            result = Encrypter.DecryptByRsa(result, privateKey);
            Console.WriteLine("RSA公钥解密后的结果：{0}", result);

            // 密钥加密，公钥解密
            result = Encrypter.EncryptByRsa(input, privateKey);
            Console.WriteLine("RSA私钥加密后的结果：{0}", result);

            /*
             * RSA算法指出私钥加密的信息，只有公钥可以解密。
             * 这就给我们实际编程过程中造成了误解，认为可以使用私钥加密，公钥解密。
             * 然而，加密时不出错，而解密时会收到“不正确的项”的错误。
             */
            // result = Encrypter.DecryptByRsa(result, publicKey);
            // Console.WriteLine("RSA公钥解密后的结果：{0}", result);

            /*
             * 数字签名:WLuHd9/k7NrngvN+wcNMjpdSY/6RKk2Pmk8YnFXM187gNa9Rxy7Y43i7babch/tf3wkX0FT
                2y75RC0OTNPEjcQF2ETgk/Nl7zoICRM5xn874gQQy9MdfO/4BAuiEQrgD03bvf2MosXfx49/Jg0oacna
                9LKjRGMjnyQXr8sVgzuE=
             */
            TestSign();

            Console.WriteLine("输入任意键退出！");
            Console.ReadKey();
        }

        /// <summary>
        /// 测试数字签名
        /// </summary>
        public static void TestSign()
        {
            var originalData = "文章不错，这是我的签名：奥巴马！";
            Console.WriteLine($"签名数为：{originalData}");

            var keyPair = Encrypter.CreateRsaKey();
            var privateKey = keyPair.Value;
            var publicKey = keyPair.Key;

            // 1、生成签名，通过摘要算法
            var signedData = Encrypter.HashAndSignString(originalData, privateKey);
            Console.WriteLine($"数字签名:{signedData}");

            // 2、验证签名
            var verify = Encrypter.VerifySigned(originalData, signedData, publicKey);
            Console.WriteLine($"签名验证结果：{verify}");
        }
    }
}
