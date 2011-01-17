<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Delete Resource</h2>

    <% Html.BeginForm("Destroy", "RestfulConvention"); %>
        <%: Html.HttpMethodOverride(HttpVerbs.Delete) %>
        <input type="submit" value="Destroy" />
    <% Html.EndForm(); %>

</asp:Content>
