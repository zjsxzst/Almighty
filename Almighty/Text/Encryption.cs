using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
//|------------------------------------------------------------|
//|                    基础信息：                              |
//|     名称：Encryption                                       |
//|     功能：字符串加解密                                     |
//|     最后修改时间：2017/10/27                               |
//|------------------------------------------------------------|

namespace Almighty.Text
{
    public class Encryption
    {
        /// <summary>
        /// 基础MD5码转换
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string TransformationMD5(string data)
        {
            MD5CryptoServiceProvider MD5Data = new MD5CryptoServiceProvider();
            return BitConverter.ToString(MD5Data.ComputeHash(Encoding.Default.GetBytes(data))).Replace("-", "");
        }
        /// <summary>
        /// 基础DES加密
        /// </summary>
        /// <param name="pToEncrypt">待加密数据</param>
        /// <param name="sKey">密匙</param>
        /// <param name="sIV">偏移量</param>
        /// <returns>密匙、偏移量只能为8位</returns>
        public static string DESEncrypt(string pToEncrypt, string sKey, string sIV)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider(); //把字符串放到byte数组中

            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            //byte[]　inputByteArray=Encoding.Unicode.GetBytes(pToEncrypt);
            //if (sKey.Length < 8 || sKey.Length > 8)
            //    return "密匙只能为8位长度";
            //if (sIV.Length < 8 || sIV.Length > 8)
            //    return "偏移量只能为8位长度";
            if (sKey.Length < 8)
                sKey = sKey.PadRight(8, '#');
            else
                sKey = sKey.Substring(0, 8);
            if (sIV.Length < 8)
                sIV = sIV.PadRight(8, '#');
            else
                sIV = sIV.Substring(0, 8);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey); //建立加密对象的密钥和偏移量
            des.IV = ASCIIEncoding.ASCII.GetBytes(sIV);  //原文使用ASCIIEncoding.ASCII方法的GetBytes方法
            MemoryStream ms = new MemoryStream();   //使得输入密码必须输入英文文本
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }
        /// <summary>
        /// 二次DES加密方法
        /// </summary>
        /// <param name="pToEncrypt">待加密数据</param>
        /// <param name="sKey">密匙</param>
        /// <param name="sIV">偏移量</param>
        /// <returns>密匙、偏移量只能为8位</returns>
        public static string SuperEncrypt(string pToEncrypt, string sKey, string sIV)
        {
            char[] pToEncrypt_Bat = pToEncrypt.ToCharArray();//转成Char
            for (int i = 0; i < pToEncrypt_Bat.Length; i++)//位移i位
            {
                int num = Convert.ToInt32(pToEncrypt_Bat[i]) + i;
                if (num > 127)//超过127则-127
                    num = num - 127;
                pToEncrypt_Bat[i] = Convert.ToChar(num);
            }
            pToEncrypt = new string(pToEncrypt_Bat);//Char转string
            string Data = DESEncrypt(pToEncrypt, sKey, sIV);
            pToEncrypt_Bat = Data.ToCharArray();
            for (int i = 0; i < pToEncrypt_Bat.Length; i++)//位移i位
            {
                int num = Convert.ToInt32(pToEncrypt_Bat[i]) + i;
                if (num > 127)
                    num = num - 127;
                pToEncrypt_Bat[i] = Convert.ToChar(Convert.ToInt32(num));
            }
            pToEncrypt = new string(pToEncrypt_Bat);//Char转string
            Data = DESEncrypt(pToEncrypt, sKey, sIV);
            return Data;
        }
        /// <summary>
        /// 基础DES解密
        /// </summary>
        /// <param name="pToEncrypt">待加密数据</param>
        /// <param name="sKey">密匙</param>
        /// <param name="sIV">偏移量</param>
        /// <returns>密匙、偏移量只能为8位</returns>
        public static string DesDecrypt(string pToDecrypt, string sKey, string sIV)
        {
            MemoryStream ms = new MemoryStream();

            try
            {
                //if (sKey.Length < 8 || sKey.Length > 8)
                //    return "密匙只能为8位长度";
                //if (sIV.Length < 8 || sIV.Length > 8)
                //    return "偏移量只能为8位长度";
                if (sKey.Length < 8)
                    sKey = sKey.PadRight(8, '#');
                else
                    sKey = sKey.Substring(0, 8);
                if (sIV.Length < 8)
                    sIV = sIV.PadRight(8, '#');
                else
                    sIV = sIV.Substring(0, 8);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sIV);

                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();

            }
            catch (Exception ex)
            {

            }

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        /// <summary>
        /// 二次DES解密
        /// </summary>
        /// <param name="pToEncrypt">待加密数据</param>
        /// <param name="sKey">密匙</param>
        /// <param name="sIV">偏移量</param>
        /// <returns>密匙、偏移量只能为8位</returns>
        public static string SuperDesDecrypt(string pToEncrypt, string sKey, string sIV)
        {
            string Data = DesDecrypt(pToEncrypt, sKey, sIV);
            char[] pToEncrypt_Bat = Data.ToCharArray();//转成Char
            for (int i = 0; i < pToEncrypt_Bat.Length; i++)
            {
                int num = Convert.ToInt32(pToEncrypt_Bat[i]) - i;
                if (num < 0)
                {
                    num += 127;
                }
                pToEncrypt_Bat[i] = Convert.ToChar(num);
            }

            Data = new string(pToEncrypt_Bat);//Char转string
            Data = DesDecrypt(Data, sKey, sIV);
            pToEncrypt_Bat = Data.ToCharArray();//转成Char
            for (int i = 0; i < pToEncrypt_Bat.Length; i++)//位移i位
            {
                int num = Convert.ToInt32(pToEncrypt_Bat[i]) - i;
                if (num < 0)//低于0则+127
                {
                    num += 127;
                }
                pToEncrypt_Bat[i] = Convert.ToChar(num);
            }
            //for (int i = 0; i < pToEncrypt_Bat.Length; i++)
            //{
            //    pToEncrypt_Bat[i] = Convert.ToChar(Convert.ToInt32(pToEncrypt_Bat[i]) - i);
            //}
            Data = new string(pToEncrypt_Bat);//Char转string
            return Data;
        }
        /// <summary>
        /// 基础AES加密
        /// </summary>
        /// <param name="toEncrypt">待加密数据</param>
        /// <param name="sKey">密匙</param>
        /// <returns></returns>
        public static string AESEncrypt(string toEncrypt, string sKey)
        {
            //判断密匙长度，如低于32位则补满，高于32则区前32位
            if (sKey.Length < 32)
                sKey = sKey.PadRight(32, '#');
            else
                sKey = sKey.Substring(0, 32);
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(sKey);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// 二次AEC加密
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string SuperAESEncrypt(string toEncrypt, string sKey)
        {
            string Time = DataTimes.StrConvertDateTimeToInt(DateTime.Now);
            if (Time.Length > 10)
                Time = Time.Substring(1, 10);
            else
            {
                Time = Time.PadRight(10, '0');
                Time = Time.Substring(0, 10);
            }

            toEncrypt = DESEncrypt(toEncrypt, Time, Time);
            toEncrypt = Time + toEncrypt;
            return AESEncrypt(toEncrypt, sKey);
        }
        /// <summary>
        /// 基础AES解密
        /// </summary>
        /// <param name="toDecrypt">待解密数据</param>
        /// <param name="sKey">密匙</param>
        /// <returns></returns>
        public static string AESDecrypt(string toDecrypt, string sKey)
        {
            //判断密匙长度，如低于32位则补满，高于32则区前32位
            if (sKey.Length < 32)
                sKey = sKey.PadRight(32, '#');
            else
                sKey = sKey.Substring(0, 32);
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(sKey);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        /// <summary>
        /// 二次AES解密
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string SuperAESDecrypt(string toDecrypt, string sKey)
        {
            toDecrypt = AESDecrypt(toDecrypt, sKey);
            string pwd = toDecrypt.Substring(0, 10);
            return DesDecrypt(toDecrypt.Substring(10, toDecrypt.Length - 10), pwd, pwd);
        }
    }
}
