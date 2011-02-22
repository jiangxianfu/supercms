<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="SuperCms.Lib" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/*
		This page was created by STEVEN Template Engine at 2011-01-28 17:24:32.
	*/

	base.OnInit(e);
	System.Text.StringBuilder templateBuilder = new System.Text.StringBuilder();

	templateBuilder.Append("<html>");
	templateBuilder.Append("<head><title>sss</title>");
	templateBuilder.Append("</head>");
	templateBuilder.Append("<body>");


	templateBuilder.Append("<html>");
	templateBuilder.Append("<div>");
	templateBuilder.Append(TemplateFunc.Label("index_loop.sql","test_index.html"));


 if (true){
	templateBuilder.Append("aaa");

}
	templateBuilder.Append("</div>");
	templateBuilder.Append("</html>");

	Response.Write(templateBuilder.ToString());
}
</script>
