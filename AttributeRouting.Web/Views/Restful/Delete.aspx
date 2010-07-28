<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="AttributeRouting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Delete Resource</h2>

    <% Html.BeginFormDELETE("Destroy"); %>
        <input type="submit" value="Destroy" />
    <% Html.EndForm(); %>

</asp:Content>
