using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SuperCms.Lib;
using System.IO;
using System.Text;

namespace SuperCms.Web
{
    public partial class TempletList : System.Web.UI.Page
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
            TempletInfo info = TempletConfigs.LoadConfig();
            this.gvTempletList.DataSource = info.Templets;
            this.gvTempletList.DataBind();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TempletEdit.aspx");
        }

        protected void gvTempletList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "modify")
            {
                Response.Redirect("~/TempletEdit.aspx?gid=" + e.CommandArgument);
            }
            else if (e.CommandName == "delete")
            {
                string filename = e.CommandArgument.ToString();
                TempletInfo info = TempletConfigs.LoadConfig();
                TempletInfoItem item = info.Templets.Where(p => p.FileName == filename).First();
                info.Templets.Remove(item);
                TempletConfigs.SaveConfig(info);
                Response.Redirect("~/TempletList.aspx");
            }
            else if (e.CommandName == "makecode")
            {
                string filename = e.CommandArgument.ToString();
                TempletInfo info = TempletConfigs.LoadConfig();
                TempletInfoItem item = info.Templets.Where(p => p.FileName == filename).First();
                if (item.Type != TempletInfoItemType.Common)
                {
                    if (item.TempletAttr == TempletInfoItemAttr.Aspx)
                    {
                        new AspxTemplate().Generated(GlobalConst.virtualFloderByTemplet, item.FileName, item.OutputPath, 1);
                    }
                    else
                    {
                        new HtmlTemplate().Generated(GlobalConst.virtualFloderByTemplet, item.FileName, item.OutputPath, 1);
                    }
                }
            }
        }   
    }
}