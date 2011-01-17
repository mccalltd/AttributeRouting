<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>Attribute-Based Routing for ASP.NET MVC</h2>

    <h3>Why?</h3>
    <ul>
        <li>
            Because your routes map to actions on your controllers. 
            They belong together, not in separate locations.
        </li>
        <li>
            Because specifying the route directly above an action 
            is a clean way of documenting how your code interacts with your site's urls.
        </li>
        <li>
            Because it's less error prone and more predictable than using 
            just a few wildcard routes in the Global.asax. 
        </li>
        <li>
            Because by using a specific route for every action and every verb,
            it becomes easier to ensure that requests fall through to catch-all route definitions.
        </li>
    </ul>

    <% Html.RenderPartial("Setup"); %>
    <% Html.RenderPartial("Quickstart"); %>
    <% Html.RenderPartial("Using"); %>
    <% Html.RenderPartial("Configuration"); %>
    <% Html.RenderPartial("Extras"); %>
   
</asp:Content>
