<%@ Page Language="C#" CodeBehind="ViewsAddedInPageInit.aspx.cs" Inherits="MvpWebForms.ViewsAddedInPageInitPage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
 <h2>View User Controls Being Added in Page_Init</h2>
 <p>
  This page demonstrates a user control with a declared presenter binding being dynamically
  loaded into the page during the Page_Init phase. This is a common scenario in CMS
  platforms, e.g. DNN.
 </p>
 <asp:Panel ID="DynamicPanel" runat="server"></asp:Panel>
</asp:Content>
