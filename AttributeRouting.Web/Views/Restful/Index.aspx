<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Resources</h2>

    <p><%: Html.ActionLink("New", "New") %></p>
    <p><%: Html.ActionLink("Edit", "Edit", new { id = 1 }, null) %></p>
    <p><%: Html.ActionLink("Show", "Show", new { id = 1 }, null) %></p>
    <p><%: Html.ActionLink("Delete", "Delete", new { id = 1 }, null) %></p>

</asp:Content>
