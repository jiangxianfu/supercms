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
    public partial class LabelTagEdit : System.Web.UI.Page
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
            LabelTagInfo info = LabelTagConfigs.LoadConfig();
            LabelTagInfoItem item = info.LabelTags.Where(p => p.FileName == filename).First();

            this.txtName.Text = item.Name;
            this.txtFileName.Text = item.FileName;
            this.txtContent.Text = FileManager.LoadContent(Utils.GetMapPath(GlobalConst.virtualFloderByLabelTag), item.FileName);

            this.txtFileName.ReadOnly = true;
            this.txtFileName.Enabled = false;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool issucess = false;

            string filefloder = Utils.GetMapPath(GlobalConst.virtualFloderByLabelTag);
            string filename = this.txtFileName.Text.Trim();

            if (!File.Exists(filefloder + filename))
            {
                FileManager.SaveContent(filefloder, filename, this.txtContent.Text);

                LabelTagInfoItem t = new LabelTagInfoItem();

                t.Name = this.txtName.Text.Trim();
                t.FileName = filename;
                t.CreateTime = DateTime.Now;
                t.LastUpdateTime = DateTime.Now;

                LabelTagInfo info = LabelTagConfigs.LoadConfig();
                info.LabelTags.Add(t);
                issucess = LabelTagConfigs.SaveConfig(info);

            }
            else
            {
                LabelTagInfo info = LabelTagConfigs.LoadConfig();
                LabelTagInfoItem t = info.LabelTags.Where(p => p.FileName == filename).First();
                FileManager.SaveContent(filefloder, filename, this.txtContent.Text);


                t.Name = this.txtName.Text.Trim();
                t.FileName = filename;

                t.CreateTime = DateTime.Now;
                t.LastUpdateTime = DateTime.Now;

                issucess = LabelTagConfigs.SaveConfig(info);
            }
            if (issucess)
            {
                Response.Redirect("LabelTagList.aspx");
            }
            else
            {
                Response.Write("<script>alert('error')</script>");
                Response.End();
            }
        }
      
    }
}