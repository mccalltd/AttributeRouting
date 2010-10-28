<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="AttributeRouting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	New
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>New Resource</h2>

    <% Html.BeginForm("Create"); %>
        <input type="submit" value="Create" />
    <% Html.EndForm(); %>

    <p><%: Html.ActionLink("Back to List", "Index") %></p>

</asp:Content>
