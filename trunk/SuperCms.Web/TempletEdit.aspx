<%@ Page Language="C#" Title="模板修改与新建"  MasterPageFile="~/Sites.Master" AutoEventWireup="true" CodeBehind="TempletEdit.aspx.cs" Inherits="SuperCms.Web.TempletEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
        <tr><td>模板类型:</td><td>
                <asp:DropDownList ID="ddlType" runat="server">
                    <asp:ListItem Text="系统模板" Value="1"  />
                    <asp:ListItem Text="公共模板" Value="2"  />
                    <asp:ListItem Text="自定义模板" Value="3" Selected="True" />
                </asp:DropDownList>                
                </td></tr>
                <tr><td>所属板块</td><td><asp:TextBox ID="txtArea" runat="server" ></asp:TextBox></td></tr>
                <tr><td>模板属性</td><td>
                    <asp:DropDownList ID="ddlTempletAttr" runat="server">
                     <asp:ListItem Text="动态模板" Value="1"  />
                    <asp:ListItem Text="静态模板" Value="2"  />
                    </asp:DropDownList>
                </td></tr>
        <tr>
            <td>
                模板名称:
            </td>
            <td>
                <asp:TextBox ID="txtTempletName" runat="server" Width="574px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                模板文件名称(带后缀的文件名):
            </td>
            <td>
                <asp:TextBox ID="txtTempletFileName" runat="server" Width="589px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                前台页面路径(完整虚拟路径):
            </td>
            <td>
                <asp:TextBox ID="txtOutputPath" runat="server" Width="586px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                模板内容:
            </td>
            <td>
                <asp:TextBox ID="txtTempletContent" runat="server" TextMode="MultiLine" Width="590px"
                    Height="500px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSubmit" runat="server" Text="添加" onclick="btnSubmit_Click" />
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="TempletList.aspx">取消</asp:HyperLink>
            </td>
        </tr>
    </table>
</asp:Content>