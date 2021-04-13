using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Client.Tool
{
   public static class JsonTool
    {
        public static string ToJSON(this object o)
        {
            if (o == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(o, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.Default });
        }

        public static T FromJSON<T>(this string input)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(input);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static string ToXml<T>(T obj)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                XmlSerializer xmlSer = new XmlSerializer(typeof(T));
                xmlSer.Serialize(stream, obj);

                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception("将实体对象转换成XML异常", ex);
            }
        }


        public static string XmlSerialize<T>(T obj, System.Text.Encoding encoding)
        {
            string result = string.Empty;
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                    //序列化对象
                    System.Xml.Serialization.XmlSerializerNamespaces namespaces = new System.Xml.Serialization.XmlSerializerNamespaces();
                    namespaces.Add("", "");

                    XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, encoding);

                    xmlTextWriter.Formatting = System.Xml.Formatting.None;
                    xmlSerializer.Serialize(xmlTextWriter, obj, namespaces);
                    xmlTextWriter.Flush();
                    xmlTextWriter.Close();

                    result = encoding.GetString(memoryStream.ToArray());
                }
            }
            catch
            {
            }
            return result;
        }


        public static string ObjListToXml<T>(T enitities)
        {
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] propinfos = null;
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"GBK\" standalone=\"yes\"?>");
            sb.AppendLine("<xml>");
            //初始化propertyinfo
            if (propinfos == null)
                {
                    Type objtype = enitities.GetType();
                    propinfos = objtype.GetProperties();
                }
                foreach (PropertyInfo propinfo in propinfos)
                {
                    sb.Append("<");
                    sb.Append(propinfo.Name);
                    sb.Append(">");
                    sb.Append(propinfo.GetValue(enitities, null));
                    sb.Append("</");
                    sb.Append(propinfo.Name);
                    sb.AppendLine(">");
                }
                sb.AppendLine("</xml>");

            return sb.ToString();
        }


        public static object GetCosts(string xmlString)
        {
            
            //Xml解析
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            XmlNodeList xxList = doc.GetElementsByTagName("xml"); //取得节点名为Object的集合
            foreach (XmlNode xxNode in xxList)  //xxNode 是每一个<CL>...</CL>体
            {
                XmlNodeList childList = xxNode.ChildNodes; //取得CL下的子节点集合

                foreach (XmlNode node in childList)
                {
                    String temp = node.Name;
                    switch (temp)
                    {
                        case "ID":    //编码
                            //myCosts.ID = node.InnerText;
                            break;
                        case "ITEM_CODE":       //材料编码
                            //myCosts.ITEM_CODE = node.InnerText;
                            break;
                    }
                }
            }
            return new object();
        }

    }
}
