﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit Resource</h2>

    <% Html.BeginForm("Update", "RestfulConvention"); %>
        <%: Html.HttpMethodOverride(HttpVerbs.Put) %>
        <input type="submit" value="Update" />
    <% Html.EndForm(); %>

    <p><%: Html.ActionLink("Back to List", "Index") %></p>

</asp:Content>
