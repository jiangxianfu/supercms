using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SuperCms.Lib;
using System.IO;

namespace SuperCms.Web
{
    public partial class LabelSqlEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.txtFileName.Text = Request.QueryString["gid"];
                if (!string.IsNullOrWhiteSpace(txtFileName.Text))
                {
                    BindData(txtFileName.Text.Trim());
                    this.btnSubmit.Text = "修改";
                }
            }
        }
        private void BindData(string filename)
        {
            LabelSqlInfo info = LabelSqlConfigs.LoadConfig();
            LabelSqlInfoItem item = info.LabelSqls.Where(p => p.FileName == filename).First();

            this.txtName.Text = item.Name;
            this.txtFileName.Text = item.FileName;
            this.txtContent.Text = FileManager.LoadContent(Utils.GetMapPath(GlobalConst.virtualFloderByLabelSql), item.FileName);

            this.txtFileName.ReadOnly = true;
            this.txtFileName.Enabled = false;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool issucess = false;

            string filefloder = Utils.GetMapPath(GlobalConst.virtualFloderByLabelSql);
            string filename = this.txtFileName.Text.Trim();

            if (!File.Exists(filefloder + filename))
            {
                FileManager.SaveContent(filefloder, filename, this.txtContent.Text);

                LabelSqlInfoItem t = new LabelSqlInfoItem();

                t.Name = this.txtName.Text.Trim();
                t.FileName = filename;
                t.CreateTime = DateTime.Now;
                t.LastUpdateTime = DateTime.Now;

                LabelSqlInfo info = LabelSqlConfigs.LoadConfig();
                info.LabelSqls.Add(t);
                issucess = LabelSqlConfigs.SaveConfig(info);

            }
            else
            {
                LabelSqlInfo info = LabelSqlConfigs.LoadConfig();
                LabelSqlInfoItem t = info.LabelSqls.Where(p => p.FileName == filename).First();
                FileManager.SaveContent(filefloder, filename, this.txtContent.Text);


                t.Name = this.txtName.Text.Trim();
                t.FileName = filename;

                t.CreateTime = DateTime.Now;
                t.LastUpdateTime = DateTime.Now;

                issucess = LabelSqlConfigs.SaveConfig(info);
            }
            if (issucess)
            {
                Response.Redirect("LabelSqlList.aspx");
            }
            else
            {
                Response.Write("<script>alert('error')</script>");
                Response.End();
            }
        }
    }
}