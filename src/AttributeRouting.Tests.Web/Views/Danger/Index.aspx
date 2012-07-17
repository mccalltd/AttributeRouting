<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Submit Me If You Dare</h2>

    <% using (Html.BeginForm("Index_Post", "Danger"))
       { %>
        <%: Html.TextArea("badstuff", "<p>ooooh, html -- danger!</p>") %>
        <input type="submit"/>
    <% } %>

</asp:Content>
