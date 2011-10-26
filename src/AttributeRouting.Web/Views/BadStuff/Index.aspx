<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Submit this form</h2>

    <% using (Html.BeginForm()) { %>
        <%: Html.TextArea("danger", "<p>ooooh, dangerous!</p>") %>
        <input type="submit"/>
    <% } %>

</asp:Content>
