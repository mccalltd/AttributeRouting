<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Nested Items</h2>

    <p><%: Html.ActionLink("Show", "Show", new { id = 2 }) %></p>
    
    <p><%: Html.ActionLink("Back to Resource", "Show", "Restful", new { id = RouteData.Values["resourceId"] }, null) %></p>

</asp:Content>
