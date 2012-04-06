<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	RenderAction Test
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>RenderAction Test</h2>

    <% Html.RenderAction("Partial"); %>

    <p>Submit this form:</p>

    <% using (Html.BeginForm()) { %>
	    <p><%: Html.TextArea("Test") %></p>
        <input type="submit" value="Click Me" />
    <% } %>
</asp:Content>
