using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Client.Http
{
    class HttpTool
    {
        static CookieContainer cookie = new CookieContainer();
        static string Url = ConfigurationManager.AppSettings["Url"];
        public static string doHttpPost(string Urlstr, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + Urlstr);
            request.Method = "POST";
            request.ContentType = "application/json";
            //request.Accept = "application/json";
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        public static string TestdoHttpPost(string Urlstr, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create( Urlstr);
            request.Method = "POST";
            request.ContentType = "application/json";
            //request.Accept = "application/json";
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        public static string FYdoHttpPost(string Urlstr, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Urlstr);
            request.Method = "POST";
            request.ContentType = "application/json";
            //request.Accept = "application/json";
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }


        public static string FYHttpPost( string postDataStr)
        {
   
            //postDataStr = Encoding.Unicode();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://fundwx.fuiou.com/micropay?req="+ postDataStr);
            request.Method = "POST";
            request.ContentType = "application/json";
            //request.Accept = "application/json";
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            //request.CookieContainer = cookie;
            //Stream myRequestStream = request.GetRequestStream();
            //StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            //myStreamWriter.Write(postDataStr);
            //myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("GBK"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            string key = ConfigurationManager.AppSettings["PUBLIC_KEY"];
           string str= Decrypt(retString,key);
            return retString;
        }


        //public static string Decrypt(string scource)
        //{
        //    DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
        //    byte[] buffer = Convert.FromBase64String(scource);
        //    using (MemoryStream memStream = new MemoryStream())
        //    {
        //        CryptoStream cryptoStream = new CryptoStream(memStream, provider.CreateDecryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write);
        //        StreamWriter sw = new StreamWriter(cryptoStream);
        //        cryptoStream.Write(buffer, 0, buffer.Length);
        //        cryptoStream.FlushFinalBlock();
        //        return ASCIIEncoding.UTF8.GetString(memStream.ToArray());
        //    }

        //}

        public static string Decrypt(string scource, string key)
        {
            try
            {
                scource = ExceptBlanks(scource);
                byte[] DataToEncrypt = Convert.FromBase64String(scource);
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(key);
                byte[] resultBytes = rsa.Decrypt(DataToEncrypt, false);
                UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
                return unicodeEncoding.GetString(resultBytes);
            }
            catch (Exception ex)
            {

                return "";
            }
        }

        public static string ExceptBlanks(string str)
        {
            int _length = str.Length;
            if (_length > 0)
            {
                StringBuilder _builder = new StringBuilder(_length);
                for (int i = 0; i < str.Length; i++)
                {
                    char _c = str[i];
                    //switch (_c)
                    //{
                    //    case '\r':
                    //    case '\n':
                    //    case '\t':
                    //    case ' ':
                    //        continue;
                    //    default:
                    //        _builder.Append(_c);
                    //        break;
                    //}
                    if (!char.IsWhiteSpace(_c))
                        _builder.Append(_c);
                }
                return _builder.ToString();
            }
            return str;
        }
    }
}
