using System;
using System.Security.Cryptography;
using System.Text;

namespace QuickBootstrap.Services.Util
{
    public delegate char GetKey();
    /// <summary>
    /// 表示加密解密类
    /// </summary>
    public class EncryptUtil
    {
        /// <summary>
        /// 给字符串MD5 加密
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string MD5ForPHP(string stringToHash)
        {
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] emailBytes = Encoding.UTF8.GetBytes(stringToHash.ToLower());
            byte[] hashedEmailBytes = md5.ComputeHash(emailBytes);
            StringBuilder sb = new StringBuilder();
            foreach (var b in hashedEmailBytes)
            {
                sb.Append(b.ToString("x2").ToLower());
            }
            return sb.ToString();
        } 

        internal static string Keys()
        {
            return "XXOO";
        }

        /// <summary> 
        /// 使用缺省密钥字符串加密 
        /// </summary> 
        /// <param name="original">明文</param> 
        /// <returns>密文</returns> 
        public static string Encrypt(string original)
        {
            return Encrypt(original, Keys());
        }
        /// <summary> 
        /// 使用缺省密钥解密 
        /// </summary> 
        /// <param name="original">密文</param> 
        /// <returns>明文</returns> 
        public static string Decrypt(string original)
        {
            return Decrypt(original, Keys(), Encoding.UTF8);
        }
        /// <summary> 
        /// 使用给定密钥解密 
        /// </summary> 
        /// <param name="original">密文</param> 
        /// <param name="key">密钥</param> 
        /// <returns>明文</returns> 
        public static string Decrypt(string original, string key)
        {
            return Decrypt(original, key, Encoding.UTF8);
        }
        /// <summary> 
        /// 使用缺省密钥解密,返回指定编码方式明文 
        /// </summary> 
        /// <param name="original">密文</param> 
        /// <param name="encoding">编码方式</param> 
        /// <returns>明文</returns> 
        public static string Decrypt(string original, Encoding encoding)
        {
            return Decrypt(original, Keys(), encoding);
        }
        /// <summary> 
        /// 使用给定密钥加密 
        /// </summary> 
        /// <param name="original">原始文字</param> 
        /// <param name="key">密钥</param> 
        /// <returns>密文</returns> 
        public static string Encrypt(string original, string key)
        {
            var buff = Encoding.Default.GetBytes(original);
            var kb = Encoding.Default.GetBytes(key);
            return Convert.ToBase64String(Encrypt(buff, kb));
        }

        /// <summary> 
        /// 使用给定密钥解密 
        /// </summary> 
        /// <param name="encrypted">密文</param> 
        /// <param name="key">密钥</param> 
        /// <param name="encoding">字符编码方案</param> 
        /// <returns>明文</returns> 
        public static string Decrypt(string encrypted, string key, Encoding encoding)
        {
            var buff = Convert.FromBase64String(encrypted);
            var kb = Encoding.Default.GetBytes(key);
            return encoding.GetString(Decrypt(buff, kb));
        }
        /// <summary> 
        /// 生成MD5摘要 
        /// </summary> 
        /// <param name="original">数据源</param> 
        /// <returns>摘要</returns> 
        public static byte[] MakeMD5(byte[] original)
        {
            var hashmd5 = new MD5CryptoServiceProvider();
            var keyhash = hashmd5.ComputeHash(original);
            return keyhash;
        }

        /// <summary> 
        /// 使用给定密钥加密 
        /// </summary> 
        /// <param name="original">明文</param> 
        /// <param name="key">密钥</param> 
        /// <returns>密文</returns> 
        public static byte[] Encrypt(byte[] original, byte[] key)
        {
            var des = new TripleDESCryptoServiceProvider
            {
                Key = MakeMD5(key),
                Mode = CipherMode.ECB
            };

            return des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
        }

        /// <summary> 
        /// 使用给定密钥解密数据 
        /// </summary> 
        /// <param name="encrypted">密文</param> 
        /// <param name="key">密钥</param> 
        /// <returns>明文</returns> 
        public static byte[] Decrypt(byte[] encrypted, byte[] key)
        {
            var des = new TripleDESCryptoServiceProvider
            {
                Key = MakeMD5(key),
                Mode = CipherMode.ECB
            };

            return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
        }

        /// <summary> 
        /// 使用给定密钥加密 
        /// </summary> 
        /// <param name="original">原始数据</param>
        /// <returns>密文</returns> 
        public static byte[] Encrypt(byte[] original)
        {
            var key = Encoding.Default.GetBytes(Keys());
            return Encrypt(original, key);
        }

        /// <summary> 
        /// 使用缺省密钥解密数据 
        /// </summary> 
        /// <param name="encrypted">密文</param>
        /// <returns>明文</returns> 
        public static byte[] Decrypt(byte[] encrypted)
        {
            var key = Encoding.Default.GetBytes(Keys());
            return Decrypt(encrypted, key);
        }
    }
}