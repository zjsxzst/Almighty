using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Almighty
{

    public class TextProcessing
    {
        /// <summary>
        /// 基础MD5码转换
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string TransformationMD5(string data)
        {
            MD5CryptoServiceProvider MD5Data= new MD5CryptoServiceProvider();
            return BitConverter.ToString(MD5Data.ComputeHash(Encoding.Default.GetBytes(data))).Replace("-","");
        }
        /// <summary>
        /// 基础DES加密
        /// </summary>
        /// <param name="pToEncrypt">待加密数据</param>
        /// <param name="sKey">密匙</param>
        /// <param name="sIV">偏移量</param>
        /// <returns>密匙、偏移量只能为8位</returns>
        public string Encrypt(string pToEncrypt, string sKey,string sIV)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider(); //把字符串放到byte数组中

            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            //byte[]　inputByteArray=Encoding.Unicode.GetBytes(pToEncrypt);
            if (sKey.Length < 8 || sKey.Length > 8)
                return "密匙只能为8位长度";
            if(sIV.Length<8 || sIV.Length>8)
                return "偏移量只能为8位长度";
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
        public string SuperEncrypt(string pToEncrypt, string sKey, string sIV)
        {
            char[] pToEncrypt_Bat= pToEncrypt.ToCharArray();//转成Char
            for(int i=0;i< pToEncrypt_Bat.Length;i++)//位移i位
            {
                pToEncrypt_Bat[i] = Convert.ToChar(Convert.ToInt32(pToEncrypt_Bat[i]) + i);
            }
            pToEncrypt = new string(pToEncrypt_Bat);//Char转string
            string Data= Encrypt(pToEncrypt, sKey, sIV);
            pToEncrypt_Bat = Data.ToCharArray();
            for (int i = 0; i < pToEncrypt_Bat.Length; i++)//位移i位
            {
                pToEncrypt_Bat[i] = Convert.ToChar(Convert.ToInt32(pToEncrypt_Bat[i]) + i);
            }
            pToEncrypt = new string(pToEncrypt_Bat);//Char转string
            Data = Encrypt(pToEncrypt, sKey, sIV);
            return Data;
        }
        /// <summary>
        /// DES加密结果
        /// </summary>
        /// <param name="pToEncrypt">待加密数据</param>
        /// <param name="sKey">密匙</param>
        /// <param name="sIV">偏移量</param>
        /// <param name="Data">加密后数据</param>
        /// <returns>密匙、偏移量只能为8位</returns>
        public bool EncryptionResults(string pToEncrypt, string sKey, string sIV,ref string Data)
        {
            Data=Encrypt(pToEncrypt, sKey, sIV);
            if (Data == "密匙只能为8位长度" || Data == "偏移量只能为8位长度")
                return false;
            return true;
        }
        /// <summary>
        /// 基础DES解密
        /// </summary>
        /// <param name="pToEncrypt">待加密数据</param>
        /// <param name="sKey">密匙</param>
        /// <param name="sIV">偏移量</param>
        /// <returns>密匙、偏移量只能为8位</returns>
        public string DesDecrypt(string pToDecrypt, string sKey,string sIV)
        {
            MemoryStream ms = new MemoryStream();

            try
            {
                if (sKey.Length < 8 || sKey.Length > 8)
                    return "密匙只能为8位长度";
                if (sIV.Length < 8 || sIV.Length > 8)
                    return "偏移量只能为8位长度";
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
            catch
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
        public string SuperDesDecrypt(string pToEncrypt, string sKey, string sIV)
        {
            string Data = DesDecrypt(pToEncrypt, sKey, sIV);
            char[] pToEncrypt_Bat = Data.ToCharArray();//转成Char
            for (int i=0;i< pToEncrypt_Bat.Length;i++)
                pToEncrypt_Bat[i] = Convert.ToChar(Convert.ToInt32(pToEncrypt_Bat[i]) - i);
            Data = new string(pToEncrypt_Bat);//Char转string
            Data = DesDecrypt(Data, sKey, sIV);
            pToEncrypt_Bat = Data.ToCharArray();//转成Char
            for (int i = 0; i < pToEncrypt_Bat.Length; i++)//位移i位
            {
                pToEncrypt_Bat[i] = Convert.ToChar(Convert.ToInt32(pToEncrypt_Bat[i]) - i);
            }
            Data = new string(pToEncrypt_Bat);//Char转string
            return Data;
        }
    }
}
