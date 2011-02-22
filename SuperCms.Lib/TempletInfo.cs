using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Web;
using System.Xml;

namespace SuperCms.Lib
{
    [Serializable]
    public class TempletInfo : ISerializableInfo
    {
        public TempletInfo()
        {
            Templets = new List<TempletInfoItem>();
        }
        public List<TempletInfoItem> Templets { get; set; }
    }
    [Serializable]
    public class TempletInfoItem
    {
        public TempletInfoItem()
        {
            CreateTime = DateTime.Now;
            LastUpdateTime = DateTime.Now;
        }
        /// <summary>
        /// 模板分类
        /// </summary>
        public TempletInfoItemType Type { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }
        //模板文件名称
        public string FileName { get; set; }

        public string OutputPath { get; set; }
        public string Area { get; set; }
        public TempletInfoItemAttr TempletAttr { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
    public enum TempletInfoItemType
    {
        /// <summary>
        /// 系统模板
        /// </summary>
        System = 1,
        /// <summary>
        /// 公用模板
        /// </summary>
        Common = 2,
        /// <summary>
        /// 自定义模板
        /// </summary>
        Customized = 3
    }
    public enum TempletInfoItemAttr
    {
        Aspx = 1,
        Html = 2
    }

    [Serializable]
    public class LabelSqlInfo : ISerializableInfo
    {
        public LabelSqlInfo()
        {
            LabelSqls = new List<LabelSqlInfoItem>();
        }
        public List<LabelSqlInfoItem> LabelSqls { get; set; }
    }
    [Serializable]
    public class LabelSqlInfoItem
    {
        public LabelSqlInfoItem()
        {

        }
        public string Name { get; set; }
        public string FileName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
    [Serializable]
    public class LabelTagInfo : ISerializableInfo
    {
        public LabelTagInfo()
        {

        }
        public List<LabelTagInfoItem> LabelTags { get; set; }
    }
    [Serializable]
    public class LabelTagInfoItem
    {
        public LabelTagInfoItem()
        {

        }
        public string Name { get; set; }
        public string FileName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }



    public class LabelTagConfigs
    {
        private static string Filename = HttpContext.Current.Server.MapPath(GlobalConst.virtualConfigByLabelTagInfo);
        public static LabelTagInfo LoadConfig()
        {
            return SerializationHelper<LabelTagInfo>.Load(Filename);
        }
        public static bool SaveConfig(LabelTagInfo templetConfigInfo)
        {
            return SerializationHelper<LabelTagInfo>.Save(Filename, templetConfigInfo);
        }
    }

    public class LabelSqlConfigs
    {
        private static string Filename = HttpContext.Current.Server.MapPath(GlobalConst.virtualConfigByLabelSqlInfo);
        public static LabelSqlInfo LoadConfig()
        {
            return SerializationHelper<LabelSqlInfo>.Load(Filename);
        }
        public static bool SaveConfig(LabelSqlInfo templetConfigInfo)
        {
            return SerializationHelper<LabelSqlInfo>.Save(Filename, templetConfigInfo);
        }
    }

    public class TempletConfigs
    {
        private static string Filename = HttpContext.Current.Server.MapPath(GlobalConst.virtualConfigByTempletInfo);
        public static TempletInfo LoadConfig()
        {
            return SerializationHelper<TempletInfo>.Load(Filename);
        }
        public static bool SaveConfig(TempletInfo templetConfigInfo)
        {
            return SerializationHelper<TempletInfo>.Save(Filename, templetConfigInfo);
        }
    }

    public class FileManager
    {
        public static string LoadContent(string filefloder, string filename)
        {
            string fullname = filefloder + filename;
            if (File.Exists(fullname))
            {
                return File.ReadAllText(fullname, Encoding.UTF8);
            }
            return "";
        }
        public static bool SaveContent(string filefloder, string filename, string content)
        {
            string fullname = filefloder + filename;
            try
            {
                File.WriteAllText(fullname, content, Encoding.UTF8);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public class GlobalConst
    {
        public static string virtualFloderByLabelSql = "~/TempletConfig/LabelSql/";
        public static string virtualFloderByLabelTag = "~/TempletConfig/LabelTag/";
        public static string virtualFloderByTemplet = "~/TempletConfig/Templet/";

        public static string virtualConfigByLabelSqlInfo = "~/TempletConfig/LabelSqlInfo.config";
        public static string virtualConfigByLabelTagInfo = "~/TempletConfig/LabelTagInfo.config";
        public static string virtualConfigByTempletInfo = "~/TempletConfig/TempletInfo.config";
    }
}