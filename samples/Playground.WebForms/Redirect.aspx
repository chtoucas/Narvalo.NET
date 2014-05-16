<%@ Page Language="C#" CodeBehind="Redirect.aspx.cs" Inherits="Playground.RedirectPage" %>

<%@ Register Src="~/Controls/RedirectControl.ascx" TagPrefix="uc" TagName="Redirect" %>

<asp:content contentplaceholderid="MainContent" runat="server">
 <h2>Redirect from Presenter</h2>
 <p>This page demonstrates a click event handled by a presenter.</p>
 <form runat="server">
    <uc:Redirect runat="server" />
  </form>
</asp:content>
