using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SuperCms.Lib;

namespace SuperCms.Web
{
    public partial class LabelSqlList : System.Web.UI.Page
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
            LabelSqlInfo info = LabelSqlConfigs.LoadConfig();
            this.gvLabelSqlList.DataSource = info.LabelSqls;
            this.gvLabelSqlList.DataBind();
        }
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/LabelSqlEdit.aspx");
        }

        protected void gvLabelSqlList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "modify")
            {
                Response.Redirect("~/LabelSqlEdit.aspx?gid=" + e.CommandArgument);
            }
            else if (e.CommandName == "delete")
            {
                string filename = e.CommandArgument.ToString();
                LabelSqlInfo info = LabelSqlConfigs.LoadConfig();
                LabelSqlInfoItem item = info.LabelSqls.Where(p => p.FileName == filename).First();
                info.LabelSqls.Remove(item);
                LabelSqlConfigs.SaveConfig(info);
                Response.Redirect("~/LabelSqlList.aspx");
            }           
        }
    }
}