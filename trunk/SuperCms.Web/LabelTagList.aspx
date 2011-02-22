<%@ Page Language="C#" Title="标签模板块管理"  MasterPageFile="~/Sites.Master" AutoEventWireup="true" CodeBehind="LabelTagList.aspx.cs" Inherits="SuperCms.Web.LabelTagList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:Button ID="btnAddNew" runat="server" Text="添加" onclick="btnAddNew_Click" />
    <asp:GridView ID="gvLabelTagList" runat="server" AutoGenerateColumns="False" BackColor="White"
        BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="1px" 
        CellPadding="3" GridLines="Both"
        Width="100%" onrowcommand="gvLabelTagList_RowCommand">
        <AlternatingRowStyle BackColor="#F7F7F7" />
        <Columns>
            <asp:BoundField HeaderText="模板名称" DataField="Name"/>
            <asp:BoundField HeaderText="文件名" DataField="FileName"/>           
            <asp:BoundField HeaderText="创建日期" DataField="CreateTime" DataFormatString="{0:yyyy-MM-dd}"/>
            <asp:BoundField HeaderText="最后修改日期" DataField="LastUpdateTime" DataFormatString="{0:yyyy-MM-dd}"/>
             <asp:TemplateField HeaderText="生成">
             <ItemTemplate>
                    <asp:Button ID="btnMakeCode" runat="server" Text="生成" CommandArgument='<%# Eval("FileName") %>' CommandName="makecode" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="修改">
                <ItemTemplate>
                    <asp:Button ID="btnModify" runat="server" Text="修改" CommandArgument='<%# Eval("FileName") %>'  CommandName="modify"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="删除">
             <ItemTemplate>
                    <asp:Button ID="btnDelete" runat="server" Text="删除" CommandArgument='<%# Eval("FileName") %>' CommandName="delete" />
                </ItemTemplate>
            </asp:TemplateField>            
        </Columns>
        <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
        <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
        <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
        <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
        <SortedAscendingCellStyle BackColor="#F4F4FD" />
        <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
        <SortedDescendingCellStyle BackColor="#D8D8F0" />
        <SortedDescendingHeaderStyle BackColor="#3E3277" />
    </asp:GridView>
</asp:Content>
