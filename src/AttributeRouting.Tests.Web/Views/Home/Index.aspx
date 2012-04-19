<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>Attribute-Based Routing for ASP.NET MVC</h2>

    <p>
        The AttributeRouting documentation is available at 
        <a href="http://wiki.github.com/mccalltd/AttributeRouting/" target="_blank">github</a>.
    </p>
   
    <p>
        Do you want to <a href="routes.axd">debug the routes for this sample</a>?
    </p>

    <p>
        Just some test links:
        <%: Html.ActionLink("About", "About", "Home") %>
    </p>

</asp:Content>
