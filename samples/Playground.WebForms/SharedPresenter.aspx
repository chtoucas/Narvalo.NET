<%@ Page Language="C#" CodeBehind="SharedPresenter.aspx.cs" Inherits="Playground.WebForms.SharedPresenterPage" %>

<%@ Register Src="~/Controls/SharedPresenterControl.ascx" TagPrefix="uc" TagName="Shared" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
 <h2>Shared Presenter Demo</h2>
 <p>
  Normally, each control instance would have their own presenter instance.
  Shared Presenters lets you attach one presenter instance to multiple
  control instances instead.
 </p>
 <p>
  This is done transparently, without requiring any changes to the view or presenter.
 </p>
 <p>
  If this is working, these two GUIDs will match:
 </p>
 <uc:Shared runat="server" />
 <uc:Shared runat="server" />
</asp:Content>
