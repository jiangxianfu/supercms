using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using System.Data;

namespace SuperCms.Lib
{

    public abstract class AbsTemplate
    {
        protected static Regex[] regex = null;
        static AbsTemplate()
        {
            regex = new Regex[9];
            RegexOptions options = RegexOptions.None;
            regex[0] = new Regex(@"<%@(.*)%>", options);
            regex[1] = new Regex(@"<%template ([^\[\]\{\}\s]+)%>", options);
            regex[2] = new Regex(@"<%#(.*)%>", options);
            regex[3] = new Regex(@"<%=(.*)%>", options);
            regex[4] = new Regex(@"<%label\(data=([\s\S]+?),tag=([\s\S]+?)\)%>", options);
            regex[5] = new Regex(@"<%pager\(data=([\s\S]+?),pagesize=([0-9]+?)\)%>", options);
            regex[6] = new Regex(@"<%var\(([\s\S]+?)\)%>", options);
            regex[7] = new Regex(@"<%templatelabel ([^\[\]\{\}\s]+)%>", options);
            regex[8] = new Regex(@"<%bind\(([\s\S]+?)\)%>", options);
        }
    }
    /// <summary>
    /// Template为动态模板类.
    /// </summary>
    public class AspxTemplate : AbsTemplate
    {
        public AspxTemplate()
        {
        }

        /// <summary>
        /// 获得模板字符串. 首先查找缓存. 如果不在缓存中则从设置中的模板路径来读取模板文件.
        /// </summary>
        /// <param name="templatePath">模板路径(虚拟路径)</param>
        /// <param name="templateName">模板文件的文件名称(含扩展名称)</param>
        ///<param name="outputFullName">模板文件输出的文件名称(含扩展名称)(虚拟路径)</param>
        /// <param name="nest">嵌套次数</param>
        /// <returns>string值,如果失败则为"",成功则为模板内容的string</returns>
        public string Generated(string templatePath, string templateName, string outputFullName, int nest)
        {
            StringBuilder strReturn = new StringBuilder();
            if (nest < 1)
            {
                nest = 1;
            }
            else if (nest > 5)
            {
                throw new Exception("嵌套超过了5层");
            }
            string extNamespace = "";
            string htmFilePath = string.Format("{0}{1}{2}", Utils.GetMapPath(templatePath), Path.DirectorySeparatorChar, templateName);

            if (!System.IO.File.Exists(htmFilePath))    //如果指定风格的htm模板文件存在
            {
                throw new Exception("没有模板文件[" + htmFilePath + "]");
            }


            using (System.IO.StreamReader objReader = new System.IO.StreamReader(htmFilePath, Encoding.UTF8))
            {
                System.Text.StringBuilder textOutput = new System.Text.StringBuilder();

                textOutput.Append(objReader.ReadToEnd());
                objReader.Close();

                //处理命名空间
                if (nest == 1)
                {
                    //命名空间
                    foreach (Match m in regex[0].Matches(textOutput.ToString()))
                    {
                        extNamespace += "\r\n" + m.Groups[0].Value;
                        textOutput.Replace(m.Groups[0].ToString(), string.Empty);
                    }
                }

                textOutput.Replace("\r\n", "\r\r\r");
                textOutput.Replace("<%", "\r\r\n<%");
                textOutput.Replace("%>", "%>\r\r\n");

                string[] strlist = Utils.SplitString(textOutput.ToString(), "\r\r\n");
                int count = strlist.GetUpperBound(0);

                for (int i = 0; i <= count; i++)
                {
                    strReturn.Append(ConvertTags(nest, templatePath, strlist[i]));
                }
            }
            if (nest == 1)
            {
                if (!string.IsNullOrWhiteSpace(outputFullName))
                {
                    string PhyOutputFullName = Utils.GetMapPath(outputFullName);
                    int lastflag = PhyOutputFullName.LastIndexOf('\\');
                    string pageDir = PhyOutputFullName.Substring(0, lastflag + 1);
                    if (!Directory.Exists(pageDir))
                    {
                        Directory.CreateDirectory(pageDir);
                    }
                    StringBuilder template = new StringBuilder();
                    template.Append("<%@ Page language=\"c#\" AutoEventWireup=\"false\" EnableViewState=\"false\" %>");
                    template.Append("\r\n<%@ Import namespace=\"System.Data\" %>");
                    template.Append("\r\n<%@ Import namespace=\"SuperCms.Lib\" %>");
                    template.AppendFormat("\r\n{0}", extNamespace);
                    template.Append("\r\n<script runat=\"server\">");
                    template.Append("\r\noverride protected void OnInit(EventArgs e)");
                    template.Append("\r\n{");
                    template.AppendFormat("\r\n\r\n\t/*\r\n\t\tThis page was created by STEVEN Template Engine at {0}.\r\n\t*/\r\n", DateTime.Now.ToString());
                    template.Append("\r\n\tbase.OnInit(e);");
                    template.Append("\r\n\tSystem.Text.StringBuilder templateBuilder = new System.Text.StringBuilder();");
                    template.AppendFormat("\r\n{0}", strReturn.ToString());
                    template.Append("\r\n\tResponse.Write(templateBuilder.ToString());");
                    template.Append("\r\n}");
                    template.Append("\r\n</script>\r\n");

                    using (FileStream fs = new FileStream(PhyOutputFullName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        Byte[] info = System.Text.Encoding.UTF8.GetBytes(template.ToString());
                        fs.Write(info, 0, info.Length);
                        fs.Close();
                    }
                }
            }
            return strReturn.ToString();
        }


        /// <summary>
        /// 转换标签
        /// </summary>
        /// <param name="nest">深度</param>
        /// <param name="templatePath">模板路径</param>
        /// <param name="inputStr">模板内容</param>
        /// <returns></returns>
        private string ConvertTags(int nest, string templatePath, string inputStr)
        {
            string strReturn = "";

            string strTemplate = "";
            strTemplate = inputStr.Replace("\\", "\\\\");
            strTemplate = strTemplate.Replace("\"", "\\\"");
            strTemplate = strTemplate.Replace("</script>", "</\" + \"script>");

            bool IsCodeLine = false;

            //<%template ([^\[\]\{\}\s]+)%><%template _xxxx%>
            foreach (Match m in regex[1].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].Value, "\r\n" + Generated(templatePath, m.Groups[1].Value, "", nest + 1) + "\r\n");

            }
            //<%xxx%>
            foreach (Match m in regex[2].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].Value, "\r\n" + m.Groups[1].Value);
                strTemplate = strTemplate.Replace("\\\"", "\"");
            }
            //<%=xxx%>
            foreach (Match m in regex[3].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].Value, m.Groups[1].Value);
                strTemplate = "\ttemplateBuilder.Append(" + strTemplate + ");\r\n";

            }
            foreach (Match m in regex[4].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].Value, "TemplateFunc.Label(\"" + m.Groups[1].Value + "\",\"" + m.Groups[2].Value + "\")");
                strTemplate = "\ttemplateBuilder.Append(" + strTemplate + ");\r\n";
            }

            if (IsCodeLine)
            {
                strReturn = strTemplate + "\r\n";
            }
            else
            {
                if (strTemplate.Trim() != "")
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string temp in Utils.SplitString(strTemplate, "\r\r\r"))
                    {
                        if (temp.Trim() == "")
                            continue;
                        sb.Append("\ttemplateBuilder.Append(\"" + temp + "\");\r\n");
                    }
                    strReturn = sb.ToString();
                }
            }
            return strReturn;
        }
    }
    /// <summary>
    /// Template为静态模板类.
    /// </summary>
    public class HtmlTemplate : AbsTemplate
    {
        public HtmlTemplate()
        {
        }
        /// <summary>
        /// 获得模板字符串. 首先查找缓存. 如果不在缓存中则从设置中的模板路径来读取模板文件.
        /// </summary>
        /// <param name="templatePath">模板路径(虚拟路径)</param>
        /// <param name="templateName">模板文件的文件名称(含扩展名称)</param>
        ///<param name="outputFullName">模板文件输出的文件名称(含扩展名称)(虚拟路径)</param>
        /// <param name="nest">嵌套次数</param>
        /// <returns>string值,如果失败则为"",成功则为模板内容的string</returns>
        public string Generated(string templatePath, string templateName, string outputFullName, int nest)
        {
            StringBuilder strReturn = new StringBuilder();
            if (nest < 1)
            {
                nest = 1;
            }
            else if (nest > 5)
            {
                throw new Exception("嵌套超过了5层");
            }
            string htmFilePath = string.Format("{0}{1}{2}", Utils.GetMapPath(templatePath), Path.DirectorySeparatorChar, templateName);

            if (!System.IO.File.Exists(htmFilePath))    //如果指定风格的htm模板文件存在
            {
                throw new Exception("没有模板文件[" + htmFilePath + "]");
            }


            using (System.IO.StreamReader objReader = new System.IO.StreamReader(htmFilePath, Encoding.UTF8))
            {
                System.Text.StringBuilder textOutput = new System.Text.StringBuilder();

                textOutput.Append(objReader.ReadToEnd());
                objReader.Close();

                textOutput.Replace("\r\n", "\r\r\r");
                textOutput.Replace("<%", "\r\r\n<%");
                textOutput.Replace("%>", "%>\r\r\n");

                string[] strlist = Utils.SplitString(textOutput.ToString(), "\r\r\n");
                int count = strlist.GetUpperBound(0);

                for (int i = 0; i <= count; i++)
                {
                    strReturn.Append(ConvertTags(nest, templatePath, strlist[i]));
                }
            }
            if (nest == 1)
            {
                if (!string.IsNullOrWhiteSpace(outputFullName))
                {
                    string PhyOutputFullName = Utils.GetMapPath(outputFullName);
                    int lastflag = PhyOutputFullName.LastIndexOf('\\');
                    string pageDir = PhyOutputFullName.Substring(0, lastflag + 1);
                    if (!Directory.Exists(pageDir))
                    {
                        Directory.CreateDirectory(pageDir);
                    }
                    using (FileStream fs = new FileStream(PhyOutputFullName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        Byte[] info = System.Text.Encoding.UTF8.GetBytes(strReturn.ToString());
                        fs.Write(info, 0, info.Length);
                        fs.Close();
                    }
                }
            }
            return strReturn.ToString();
        }
        /// <summary>
        /// 转换标签
        /// </summary>
        /// <param name="nest">深度</param>
        /// <param name="templatePath">模板路径</param>
        /// <param name="inputStr">模板内容</param>
        /// <returns></returns>
        private string ConvertTags(int nest, string templatePath, string inputStr)
        {
            string strReturn = "";

            string strTemplate = inputStr;

            bool IsCodeLine = false;

            //<%template ([^\[\]\{\}\s]+)%><%template _xxxx%>
            foreach (Match m in regex[1].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].Value, Generated(templatePath, m.Groups[1].Value, "", nest + 1));
            }
            //<%var([\s\S]+?)%><%var(function)%>
            foreach (Match m in regex[6].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].Value, TemplateFunc.Var(m.Groups[1].Value));
                //strTemplate = "\t" + strTemplate + "\r\n";
            }
            //<%label\(([\s\S]+?),([\s\S]+?)\)%><%label(data=sql,tag=tag)%>
            foreach (Match m in regex[4].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].Value, TemplateFunc.Label(m.Groups[1].Value, m.Groups[2].Value));
                //strTemplate = "\t" + strTemplate + "\r\n";
            }

            if (IsCodeLine)
            {
                strReturn = strTemplate;// +"\r\n";
            }
            else
            {
                if (strTemplate.Trim() != "")
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string temp in Utils.SplitString(strTemplate, "\r\r\r"))
                    {
                        if (temp.Trim() == "")
                            continue;
                        sb.Append("\t" + temp + "\r\n");
                    }
                    strReturn = sb.ToString();
                }
            }
            return strReturn;
        }
    }
    /// <summary>
    /// 标签中的模板类
    /// </summary>
    public class LabelTemplate : AbsTemplate
    {
        public LabelTemplate()
        {
        }
        /// <summary>
        /// 获得模板字符串. 首先查找缓存. 如果不在缓存中则从设置中的模板路径来读取模板文件.
        /// </summary>
        /// <param name="templatePath">模板路径(虚拟路径)</param>
        /// <param name="templateName">模板文件的文件名称(含扩展名称)</param>
        ///<param name="outputFullName">模板文件输出的文件名称(含扩展名称)(虚拟路径)</param>
        /// <param name="nest">嵌套次数</param>
        /// <returns>string值,如果失败则为"",成功则为模板内容的string</returns>
        public string Generated(string templatePath, string templateName, int nest, DataRow row)
        {
            StringBuilder strReturn = new StringBuilder();
            if (nest < 1)
            {
                nest = 1;
            }
            else if (nest > 5)
            {
                throw new Exception("嵌套超过了5层");
            }
            string htmFilePath = string.Format("{0}{1}{2}", Utils.GetMapPath(templatePath), Path.DirectorySeparatorChar, templateName);

            if (!System.IO.File.Exists(htmFilePath))    //如果指定风格的htm模板文件存在
            {
                throw new Exception("没有模板文件[" + htmFilePath + "]");
            }


            using (System.IO.StreamReader objReader = new System.IO.StreamReader(htmFilePath, Encoding.UTF8))
            {
                System.Text.StringBuilder textOutput = new System.Text.StringBuilder();

                textOutput.Append(objReader.ReadToEnd());
                objReader.Close();

                textOutput.Replace("\r\n", "\r\r\r");
                textOutput.Replace("<%", "\r\r\n<%");
                textOutput.Replace("%>", "%>\r\r\n");

                string[] strlist = Utils.SplitString(textOutput.ToString(), "\r\r\n");
                int count = strlist.GetUpperBound(0);

                for (int i = 0; i <= count; i++)
                {
                    strReturn.Append(ConvertTags(nest, templatePath, strlist[i], row));
                }
            }
            return strReturn.ToString();
        }
        /// <summary>
        /// 转换标签
        /// </summary>
        /// <param name="nest">深度</param>
        /// <param name="templatePath">模板路径</param>
        /// <param name="inputStr">模板内容</param>
        /// <returns></returns>
        private string ConvertTags(int nest, string templatePath, string inputStr, DataRow row)
        {
            string strReturn = "";

            string strTemplate = inputStr;

            bool IsCodeLine = false;

            //<%templatelabel ([^\[\]\{\}\s]+)%><%templatelabel _xxxx%>
            foreach (Match m in regex[7].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].Value, Generated(templatePath, m.Groups[1].Value, nest + 1, row));
            }
            //<%var([\s\S]+?)%><%var(function)%>
            foreach (Match m in regex[6].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].Value, TemplateFunc.Var(m.Groups[1].Value));
                //strTemplate = "\t" + strTemplate + "\r\n";
            }
            //<%bind([\s\S]+?)%><%bind(tag)%>
            foreach (Match m in regex[8].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].Value, Convert.ToString(row[m.Groups[1].Value]));
            }
            if (IsCodeLine)
            {
                strReturn = strTemplate;// +"\r\n";
            }
            else
            {
                if (strTemplate.Trim() != "")
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string temp in Utils.SplitString(strTemplate, "\r\r\r"))
                    {
                        if (temp.Trim() == "")
                            continue;
                        sb.Append("\t" + temp + "\r\n");
                    }
                    strReturn = sb.ToString();
                }
            }
            return strReturn;
        }
    }
}