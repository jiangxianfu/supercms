<%@ Page Language="C#" Title="标签测试向导"  MasterPageFile="~/Sites.Master" AutoEventWireup="true" CodeBehind="LabelTagSqlTest.aspx.cs" Inherits="SuperCms.Web.LabelTagSqlTest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
    <tr><td>标签名称<asp:DropDownList ID="ddlTags" runat="server">
        </asp:DropDownList>
    </td></tr>
    <tr><td>标签数据名称<asp:DropDownList ID="ddlSqls" runat="server"></asp:DropDownList></td></tr>
        <tr><td colspan="2">
            <asp:Button ID="btnNext" runat="server" Text="下一步" onclick="btnNext_Click" /></td></tr>
            <tr><td>
                <asp:TextBox ID="txtTagSql" runat="server" Width="672px"></asp:TextBox></td></tr>
                <tr><td><asp:Button ID="btnPreview" runat="server" Text="预览" 
                        onclick="btnPreview_Click" /></td></tr>
                <tr><td>
                <div style="border:1px solid #FF0000;">
                <%=ResultHtml %>
                </div>
                </td></tr>
    </table>
</asp:Content>
