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
    public partial class TempletEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtTempletFileName.Text = Request.QueryString["gid"];
                if (!string.IsNullOrWhiteSpace(txtTempletFileName.Text))
                {
                    BindData(txtTempletFileName.Text);
                    this.btnSubmit.Text = "修改";
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool issucess = false;
            string filefloder = Utils.GetMapPath(GlobalConst.virtualFloderByTemplet);
            string filename = this.txtTempletFileName.Text.Trim();
            if (!File.Exists(filefloder + filename))
            {
                FileManager.SaveContent(filefloder, filename, this.txtTempletContent.Text);

                TempletInfoItem t = new TempletInfoItem();

                t.Type = (TempletInfoItemType)Convert.ToInt32(this.ddlType.SelectedValue);
                t.Name = this.txtTempletName.Text.Trim();
                t.FileName = filename;
                t.OutputPath = this.txtOutputPath.Text.Trim();
                t.Area = this.txtArea.Text.Trim();
                t.TempletAttr = (TempletInfoItemAttr)Convert.ToInt32(this.ddlTempletAttr.SelectedValue);
                t.CreateTime = DateTime.Now;
                t.LastUpdateTime = DateTime.Now;

                TempletInfo info = TempletConfigs.LoadConfig();
                info.Templets.Add(t);
                issucess = TempletConfigs.SaveConfig(info);

            }
            else
            {
                TempletInfo info = TempletConfigs.LoadConfig();
                TempletInfoItem t = info.Templets.Where(p => p.FileName == filename).First();

                FileManager.SaveContent(filefloder, filename, this.txtTempletContent.Text);

                t.Type = (TempletInfoItemType)Convert.ToInt32(this.ddlType.SelectedValue);
                t.Name = this.txtTempletName.Text.Trim();
                t.FileName = filename;
                t.OutputPath = this.txtOutputPath.Text.Trim();
                t.Area = this.txtArea.Text.Trim();
                t.TempletAttr = (TempletInfoItemAttr)Convert.ToInt32(this.ddlTempletAttr.SelectedValue);
                t.CreateTime = DateTime.Now;
                t.LastUpdateTime = DateTime.Now;

                issucess = TempletConfigs.SaveConfig(info);
            }
            if (issucess)
            {
                Response.Redirect("TempletList.aspx");
            }
            else
            {
                Response.Write("<script>alert('error')</script>");
                Response.End();
            }
        }

        private void BindData(string templetFileName)
        {
            TempletInfo info = TempletConfigs.LoadConfig();
            TempletInfoItem item = info.Templets.Where(p => p.FileName == templetFileName).First();


            this.ddlType.SelectedValue = ((int)item.Type).ToString();
            this.txtArea.Text = item.Area;
            this.ddlTempletAttr.SelectedValue = ((int)item.TempletAttr).ToString();
            this.txtTempletName.Text = item.Name;
            this.txtTempletFileName.Text = item.FileName;
            this.txtOutputPath.Text = item.OutputPath;
            this.txtTempletContent.Text = FileManager.LoadContent(Utils.GetMapPath(GlobalConst.virtualFloderByTemplet),item.FileName);


            this.txtTempletFileName.ReadOnly = true;
            this.txtTempletFileName.Enabled = false;
        }
    }
}