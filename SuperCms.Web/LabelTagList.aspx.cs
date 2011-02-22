using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SuperCms.Lib;

namespace SuperCms.Web
{
    public partial class LabelTagList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }
        private void BindData()
        {
            LabelTagInfo info = LabelTagConfigs.LoadConfig();
            this.gvLabelTagList.DataSource = info.LabelTags;
            this.gvLabelTagList.DataBind();
        }
        protected void gvLabelTagList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "modify")
            {
                Response.Redirect("~/LabelTagEdit.aspx?gid=" + e.CommandArgument);
            }
            else if (e.CommandName == "delete")
            {
                string filename = e.CommandArgument.ToString();
                LabelTagInfo info = LabelTagConfigs.LoadConfig();
                LabelTagInfoItem item = info.LabelTags.Where(p => p.FileName == filename).First();
                info.LabelTags.Remove(item);
                LabelTagConfigs.SaveConfig(info);
                Response.Redirect("~/LabelTagList.aspx");
            }          
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/LabelTagEdit.aspx");
        }
    }
}