<%@ Control Language="C#" CodeBehind="AsyncTapControl.ascx.cs" Inherits="MvpWebForms.Controls.AsyncTapControl" %>
<div class="async-messages">
 <asp:Repeater runat="server" DataSource="<%# Model.Messages %>">
  <HeaderTemplate>
   <ol>
  </HeaderTemplate>
  <ItemTemplate>
   <li><%# Container.DataItem %></li>
  </ItemTemplate>
  <FooterTemplate>
   </ol>
  </FooterTemplate>
 </asp:Repeater>
</div>
