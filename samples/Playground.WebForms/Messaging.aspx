<%@ Page Language="C#" MasterPageFile="~/Layouts/Site.Master" CodeBehind="Messaging.aspx.cs"
 Inherits="Playground.WebForms.MessagingPage" %>

<%@ Register Src="~/Controls/Messaging1Control.ascx" TagPrefix="uc" TagName="Messaging1" %>
<%@ Register Src="~/Controls/Messaging2Control.ascx" TagPrefix="uc" TagName="Messaging2" %>
<asp:Content ContentPlaceHolderID="content" runat="server">
 <h1>Cross Presenter Messaging Demo</h1>
 <uc:Messaging1 runat="server" />
 <uc:Messaging2 runat="server" />
</asp:Content>
