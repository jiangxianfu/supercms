using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SuperCms.Lib;
using System.Text.RegularExpressions;

namespace SuperCms.Web
{
    public partial class LabelTagSqlTest : System.Web.UI.Page
    {
        public string ResultHtml;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }
        private void BindData()
        {
            LabelSqlInfo sqlinfo = LabelSqlConfigs.LoadConfig();
            this.ddlSqls.DataSource = sqlinfo.LabelSqls;
            this.ddlSqls.DataTextField = "Name";
            this.ddlSqls.DataValueField = "FileName";
            this.ddlSqls.DataBind();
            LabelTagInfo taginfo = LabelTagConfigs.LoadConfig();
            this.ddlTags.DataSource = taginfo.LabelTags;
            this.ddlTags.DataTextField = "Name";
            this.ddlTags.DataValueField = "FileName";
            this.ddlTags.DataBind();
            ListItem emptyitem = new ListItem("--请选择--", "");
            this.ddlSqls.Items.Insert(0, emptyitem);
            this.ddlTags.Items.Insert(0, emptyitem);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            string sql = this.ddlSqls.SelectedValue;
            string tag = this.ddlTags.SelectedValue;
            if (!string.IsNullOrWhiteSpace(sql) && !string.IsNullOrWhiteSpace(tag))
            {
                this.txtTagSql.Text = string.Format("<%label(data={0},tag={1})%>", sql, tag);
            }
            else
            {
                Response.Write("<scirpt>alert('请填写完整标签内容');</script>");
            }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            string itemdata = this.txtTagSql.Text.Trim();

            if (!string.IsNullOrWhiteSpace(itemdata))
            {
              Regex r=  new Regex(@"<%label\(data=([\s\S]+?),tag=([\s\S]+?)\)%>",  RegexOptions.None);
              Match m=  r.Match(itemdata);
              if (m.Groups.Count == 3)
              {
                  try
                  {
                      ResultHtml = TemplateFunc.Label(m.Groups[1].Value, m.Groups[2].Value);
                  }
                  catch (Exception ex)
                  {
                      Response.Write("<scirpt>alert('数据显示错误:" + ex.Message + "');</script>");
                  }
              }
              else
              {
                  Response.Write("<scirpt>alert('标签格式错误');</script>");
              }
            }
            else
            {
                Response.Write("<scirpt>alert('请填写完整标签内容');</script>");
            }
        }
    }
}