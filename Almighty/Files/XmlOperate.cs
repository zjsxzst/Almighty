using Almighty.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
//|------------------------------------------------------------|
//|                    基础信息：                              |
//|     名称：XmlOperate                                       |
//|     功能：Xml操作                                          |
//|     最后修改时间：2017/10/27                               |
//|------------------------------------------------------------|
namespace Almighty.Files
{
    //public class XmlOperate
    //{
    //    /// <summary>
    //    /// 读取XML数据
    //    /// </summary>
    //    /// <param name="Path">XML文件地址,默认为Config.xml</param>
    //    /// <param name="ChildNode">子节点名</param>
    //    /// <param name="ChildNodeInformation">带获取数据(','分开)</param>
    //    /// <returns></returns>
    //    public static string[][] GetXMLData(string Path, string ChildNode, string ChildNodeInformation)
    //    {
    //        if (!string.IsNullOrWhiteSpace(Path) || Path.Length <= 0)
    //            Path = "Config.xml";
    //        XmlDocument doc = new XmlDocument();
    //        doc.Load(Path);
    //        //XmlElement rootElem = doc.DocumentElement;
    //        XmlNodeList personNodes = XmlNodes(doc.DocumentElement, ChildNode);
    //        return GetChildNodeData(personNodes, ChildNodeInformation);
    //    }
    //    /// <summary>
    //    /// 获取XML中所有的目标子节点
    //    /// </summary>
    //    /// <param name="Data"></param>
    //    /// <param name="ChildNode"></param>
    //    /// <returns></returns>
    //    private static XmlNodeList XmlNodes(XmlElement Data, string ChildNode)
    //    {
    //        return Data.GetElementsByTagName(ChildNode);
    //    }
    //    /// <summary>
    //    /// 获取具体值
    //    /// </summary>
    //    /// <param name="Nodes">XML子节点数据</param>
    //    /// <param name="DataList">需要读取的数据</param>
    //    /// <returns></returns>
    //    private static string[][] GetChildNodeData(XmlNodeList Nodes, string DataList)
    //    {
    //        string[] Data = DataList.Split(',');
    //        string[][] dates = new string[Nodes.Count][];
    //        for (int i = 0; i < Nodes.Count; i++)
    //        {
    //            dates[i] = new string[Data.Length];
    //            for (int j = 0; j < Data.Length; j++)
    //            {
    //                dates[i][j] = ((XmlElement)Nodes.Item(i)).GetAttribute(Data[j]);
    //            }
    //        }
    //        return dates;
    //    }
    //    /// <summary>
    //    /// 增加
    //    /// </summary>
    //    /// <param name="stu"></param>
    //    /// <param name="root"></param>
    //    /// <param name="path"></param>
    //    /// <returns></returns>
    //    public static bool addXml(string path, string superclass, string subclass, string listing, string data)
    //    {
    //        try
    //        {
    //            XmlDocument doc = new XmlDocument();
    //            if (!string.IsNullOrWhiteSpace(path))
    //                path = "Config.xml";
    //            doc.Load(path);
    //            XmlNode node = doc.SelectSingleNode(superclass);
    //            XmlNodeList xnl = node.ChildNodes;
    //            XmlElement ChildNode = doc.CreateElement(subclass);
    //            string[] RowNameList = listing.Split(',');
    //            string[] DataList = data.Split(',');
    //            //XmlElement Branch = doc.CreateElement(subclass);
    //            for (int i = 0; i < RowNameList.Length; i++)
    //                ChildNode.SetAttribute(RowNameList[i], DataList[i]);
    //            //ChildNode.AppendChild(Branch);
    //            //xe1.SetAttribute("name", stu.ClassNo);
    //            //XmlElement xe2 = doc.CreateElement("student");
    //            //xe2.SetAttribute("studentId", stu.StudentId);
    //            //xe2.SetAttribute("name", stu.StudentName);
    //            //xe1.AppendChild(xe2);
    //            node.AppendChild(ChildNode);
    //            doc.Save(path);

    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            return false;
    //        }
    //    }
    //}
    public class XmlOperate<T> where T : new()
    {
        /// <summary>
        /// 单表序列化
        /// </summary>
        /// <param name="details"></param>
        /// <param name="Ptah"></param>
        static public bool Serialize(T details, string Ptah, ref string erro)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (TextWriter writer = new StreamWriter(Ptah))
                {
                    serializer.Serialize(writer, details);
                }
                return true;
            }
            catch (Exception ex)
            {
                erro = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 列表序列化
        /// </summary>
        /// <param name="list"></param>
        /// <param name="Ptah"></param>
        public static bool Serialize(List<T> list, string Ptah, ref string erro)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                using (TextWriter writer = new StreamWriter(Ptah))
                {
                    serializer.Serialize(writer, list);
                }
                return true;
            }
            catch (Exception ex)
            {
                erro = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 单表反序列化
        /// </summary>
        /// <param name="details"></param>
        /// <param name="Ptah"></param>
        /// <param name="erro"></param>
        /// <returns></returns>
        public static bool DSerialize(ref T details, string Ptah, ref string erro)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                TextReader reader = new StreamReader(Ptah);
                object obj = deserializer.Deserialize(reader);
                details = (T)obj;
                reader.Close();
                return true;
            }
            catch (Exception ex)
            {
                erro = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 列表反序列化
        /// </summary>
        /// <param name="details"></param>
        /// <param name="Ptah"></param>
        /// <param name="erro"></param>
        /// <returns></returns>
        public static bool DSerialize(ref List<T> details, string Ptah, ref string erro)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<T>));
                TextReader reader = new StreamReader(Ptah);
                object obj = deserializer.Deserialize(reader);
                details = (List<T>)obj;
                reader.Close();
                return true;
            }
            catch (Exception ex)
            {
                erro = ex.ToString();
                return false;
            }
        }
    }
}
