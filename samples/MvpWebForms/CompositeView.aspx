<%@ Page Language="C#" CodeBehind="CompositeView.aspx.cs" Inherits="MvpWebForms.CompositeViewPage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
 <h2>Composite View</h2>
 <p>
  Normally, each control instance would have their own presenter instance.
  Shared Presenters lets you attach one presenter instance to multiple
  control instances instead.
 </p>
 <p>
  This is done transparently, without requiring any changes to the view or presenter.
 </p>
 <p>
  <em>Here Shared Presenter mode is requested at the page level.</em>
 </p>
 <p>
  If this is working, these two GUIDs will match:
 </p>
 <uc:Composite runat="server" />
 <uc:Composite runat="server" />
</asp:Content>
