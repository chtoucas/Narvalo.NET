﻿<%@ Page Language="C#" MasterPageFile="~/Layouts/Site.Master" CodeBehind="PageView.aspx.cs"
 Inherits="Playground.WebForms.PageView" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
 <h1>Page View Demo</h1>
 <p>In this demo, the page itself is acting as the view.</p>
 <p>We prefer to use controls but this approach works too.</p>
 <p class="hello-world">
  <%# Model.Message %>
 </p>
</asp:Content>
