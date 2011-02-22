<%@ Page Language="C#" Title="标签模块修改与新建"  MasterPageFile="~/Sites.Master" AutoEventWireup="true" CodeBehind="LabelTagEdit.aspx.cs" Inherits="SuperCms.Web.LabelTagEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
        <tr>
            <td>
                名称
            </td>
            <td>
                <asp:TextBox ID="txtName" runat="server" Width="361px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                文件名称
            </td>
            <td>
                <asp:TextBox ID="txtFileName" runat="server" Width="362px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                文件内容
            </td>
            <td>
                <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Height="427px" 
                    Width="514px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSubmit" runat="server" Text="提交" onclick="btnSubmit_Click" />
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="LabelTagList.aspx">取消</asp:HyperLink>
            </td>
        </tr>
    </table>
</asp:Content>