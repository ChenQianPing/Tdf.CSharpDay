using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Tdf.CSharpDay.Chapter1
{
    /// <summary>
    /// 加密、解密
    /// </summary>
    public class Encrypter
    {
        // DES默认密钥向量
        private static byte[] _desIv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        // AES默认密钥向量   
        public static readonly byte[] AesIv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        #region MD5
        /// <summary>
        /// MD5加密为32字符长度的16进制字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string EncryptByMd5(string input)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sBuilder = new StringBuilder();
            // 将每个字节转为16进制
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
        #endregion

        #region SHA1
        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string EncryptBySha1(string input)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            var bytes = Encoding.Unicode.GetBytes(input);
            var result = sha.ComputeHash(bytes);
            return BitConverter.ToString(result);
        }
        #endregion

        #region DES
        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncryptByDes(string input, string key)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input); 
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var encryptBytes = EncryptByDes(inputBytes, keyBytes, keyBytes);

            using (DES des = new DESCryptoServiceProvider())
            {
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (var writer = new StreamWriter(cs))
                        {
                            writer.Write(inputBytes);
                        }
                    }
                }
            }

            var result = Convert.ToBase64String(encryptBytes);

            return result;
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="inputBytes">输入byte数组</param>
        /// <param name="key">密钥，只能是英文字母或数字</param>
        /// <param name="IV">偏移向量</param>
        /// <returns></returns>
        public static byte[] EncryptByDes(byte[] inputBytes, byte[] key, byte[] IV)
        {
            DES des = new DESCryptoServiceProvider();
            // 建立加密对象的密钥和偏移量
            des.Key = key;
            des.IV = IV;

            string result = string.Empty;

            // 如果通过CryptoStreamMode.Write方式进行加密，然后CryptoStreamMode.Read方式进行解密，解密成功。
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputBytes, 0, inputBytes.Length);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DecryptByDes(string input, string key)
        {
            byte[] inputBytes = Convert.FromBase64String(input);

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] resultBytes = DecryptByDes(inputBytes, keyBytes, keyBytes);

            string result = Encoding.UTF8.GetString(resultBytes);

            return result;
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static byte[] DecryptByDes(byte[] inputBytes, byte[] key, byte[] iv)
        {
            // 建立加密对象的密钥和偏移量，此值重要，不能修改
            DESCryptoServiceProvider des = new DESCryptoServiceProvider
            {
                Key = key,
                IV = iv
            };

            // 通过read方式解密
            using (var ms = new MemoryStream(inputBytes))
            {
                using (var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (var reader = new StreamReader(cs))
                    {
                        var result = reader.ReadToEnd();
                        return Encoding.UTF8.GetBytes(result);
                    }
                }
            }
            
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string EncryptString(string input, string sKey)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = Encoding.ASCII.GetBytes(sKey);
                des.IV = Encoding.ASCII.GetBytes(sKey);
                ICryptoTransform desencrypt = des.CreateEncryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return BitConverter.ToString(result);
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string DecryptString(string input, string sKey)
        {
            string[] sInput = input.Split("-".ToCharArray());
            byte[] data = new byte[sInput.Length];

            for (int i = 0; i < sInput.Length; i++)
            {
                data[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
            }

            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = Encoding.ASCII.GetBytes(sKey);
                des.IV = Encoding.ASCII.GetBytes(sKey);
                ICryptoTransform desencrypt = des.CreateDecryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return Encoding.UTF8.GetString(result);
            }
        }
        #endregion

        #region AES
        /// <summary>  
        /// AES加密算法  
        /// </summary>  
        /// <param name="input">明文字符串</param>  
        /// <param name="key">密钥</param>  
        /// <returns>字符串</returns>  
        public static string EncryptByAes(string input, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 32));
            using (var aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = AesIv;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }

                        var bytes = msEncrypt.ToArray();
                        return BitConverter.ToString(bytes);
                    }
                }
            }
        }


        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="input">密文字节数组</param>  
        /// <param name="key">密钥</param>  
        /// <returns>返回解密后的字符串</returns>  
        public static string DecryptByAes(string input, string key)
        {
            var sInput = input.Split("-".ToCharArray());
            var inputBytes = new byte[sInput.Length];
            for (var i = 0; i < sInput.Length; i++)
            {
                inputBytes[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
            }

            var keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 32));
            using (var aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = AesIv;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream(inputBytes))
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srEncrypt = new StreamReader(csEncrypt))
                        {
                            return srEncrypt.ReadToEnd();
                        }
                    }
                }
            }
        }


        /// <summary> 
        /// AES加密        
        /// </summary> 
        /// <param name="inputdata">输入的数据</param>         
        /// <param name="iv">向量128位</param>         
        /// <param name="key">加密密钥</param>         
        /// <returns></returns> 
        public static byte[] EncryptByAes(byte[] inputdata, byte[] key, byte[] iv)
        {
            using (var aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(inputdata);
                        }

                        var encrypted = msEncrypt.ToArray();
                        return encrypted;
                    }
                }
            }
        }

        /// <summary>         
        /// AES解密         
        /// </summary> 
        /// <param name="inputBytes">输入的数据</param>                
        /// <param name="key">key</param>         
        /// <param name="iv">向量128</param> 
        /// <returns></returns> 
        public static byte[] DecryptByAes(byte[] inputBytes, byte[] key, byte[] iv)
        {
            Aes aes = new AesCryptoServiceProvider();
            aes.Key = key;
            aes.IV = iv;
            byte[] decryptBytes;
            using (var ms = new MemoryStream(inputBytes))
            {
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (var reader = new StreamReader(cs))
                    {
                        var result = reader.ReadToEnd();
                        decryptBytes = Encoding.UTF8.GetBytes(result);
                    }
                }
            }

            return decryptBytes;
        }
        #endregion

        #region RSA
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <param name="publicKey">公钥</param>
        /// <returns>密文字符串</returns>
        public static string EncryptByRsa(string plaintext, string publicKey)
        {
            var byteConverter = new UnicodeEncoding();
            var dataToEncrypt = byteConverter.GetBytes(plaintext);
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                var encryptedData = rsa.Encrypt(dataToEncrypt, false);
                return Convert.ToBase64String(encryptedData);
            }
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>明文字符串</returns>
        public static string DecryptByRsa(string ciphertext, string privateKey)
        {
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                byte[] encryptedData = Convert.FromBase64String(ciphertext);
                byte[] decryptedData = rsa.Decrypt(encryptedData, false);
                return byteConverter.GetString(decryptedData);
            }
        }
        
        /// <summary>
        /// 数字签名
        /// </summary>
        /// <param name="plaintext">原文</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>签名</returns>
        public static string HashAndSignString(string plaintext, string privateKey)
        {
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = byteConverter.GetBytes(plaintext);

            using (RSACryptoServiceProvider rsAalg = new RSACryptoServiceProvider())
            {
                rsAalg.FromXmlString(privateKey);
                // 使用SHA1进行摘要算法，生成签名
                byte[] encryptedData = rsAalg.SignData(dataToEncrypt, new SHA1CryptoServiceProvider());
                return Convert.ToBase64String(encryptedData);
            }
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="plaintext">原文</param>
        /// <param name="signedData">签名</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public static bool VerifySigned(string plaintext, string signedData, string publicKey)
        {
            using (RSACryptoServiceProvider rsAalg = new RSACryptoServiceProvider())
            {
                rsAalg.FromXmlString(publicKey);
                UnicodeEncoding byteConverter = new UnicodeEncoding();
                byte[] dataToVerifyBytes = byteConverter.GetBytes(plaintext);
                byte[] signedDataBytes = Convert.FromBase64String(signedData);
                return rsAalg.VerifyData(dataToVerifyBytes, new SHA1CryptoServiceProvider(), signedDataBytes);
            }
        }

        /// <summary>
        /// 获取Key
        /// 键为公钥，值为私钥
        /// </summary>
        /// <returns></returns>
        public static KeyValuePair<string, string> CreateRsaKey()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            var privateKey = rsa.ToXmlString(true);
            var publicKey = rsa.ToXmlString(false);

            return new KeyValuePair<string, string>(publicKey, privateKey);
        }
        #endregion

        #region other
        /// <summary>
        /// GetBytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string input)
        {
            string[] sInput = input.Split("-".ToCharArray());
            byte[] inputBytes = new byte[sInput.Length];
            for (var i = 0; i < sInput.Length; i++)
            {
                inputBytes[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
            }
            return inputBytes;
        }
        #endregion
    }
}
