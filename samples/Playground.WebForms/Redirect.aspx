<%@ Page Language="C#" CodeBehind="Redirect.aspx.cs" Inherits="Playground.WebForms.RedirectPage" %>

<%@ Register Src="~/Controls/RedirectControl.ascx" TagPrefix="uc" TagName="Redirect" %>

<asp:content contentplaceholderid="MainContent" runat="server">
 <h2>Redirect From Presenter Demo</h2>
 <form runat="server">
    <uc:Redirect runat="server" />
  </form>
</asp:content>
