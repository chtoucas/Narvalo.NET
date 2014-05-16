<%@ Control Language="C#" CodeBehind="DynamicallyLoadedControl.ascx.cs" Inherits="Playground.Controls.DynamicallyLoadedControl" %>
<div class="dynamically-loaded">
 <asp:PlaceHolder ID=cph1 runat="server" Visible=false>
  <p class="success">
   If this content is visible, the presenter was bound successfully
   after the control was dynamically loaded.
  </p>
 </asp:PlaceHolder>
 <asp:PlaceHolder ID=cph2 runat="server" Visible=true>
  <p class="failed">
   If this content is visible, there was a problem in binding the
   dynamically loaded control's presenter.
  </p>
 </asp:PlaceHolder>
</div>
