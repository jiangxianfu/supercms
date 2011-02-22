﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace SuperCms.Lib
{
    public interface ISerializableInfo { }
    /// <summary>
    /// SerializationHelper 的摘要说明。
    /// </summary>
    public class SerializationHelper<T> where T : ISerializableInfo,new()
    {
        /// <summary>
        /// 使用XmlSerializer序列化对象
        /// </summary>
        /// <typeparam name=“T“>需要序列化的对象类型，必须声明[Serializable]特征</typeparam>
        /// <param name=“obj“>需要序列化的对象</param>
        public static string XmlSerialize(T obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 使用XmlSerializer反序列化对象
        /// </summary>
        /// <param name=“xmlOfObject“>需要反序列化的xml字符串</param>
        public static T XmlDeserialize(string xmlOfObject)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sr = new StreamWriter(ms, Encoding.UTF8))
                {
                    sr.Write(xmlOfObject);
                    sr.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(ms);
                }
            }
        }

        public static T Load(string filename)
        {
            T info;

            if (File.Exists(filename))
            {
                string xml = File.ReadAllText(filename, Encoding.UTF8);
                info = XmlDeserialize(xml);
            }
            else
            {
                info = new T();
                Save(filename, info);
            }
            return info;
        }

        public static bool Save(string filename, T templetConfigInfo)
        {
            try
            {
                string xml = XmlSerialize(templetConfigInfo);
                File.WriteAllText(filename, xml, Encoding.UTF8);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
