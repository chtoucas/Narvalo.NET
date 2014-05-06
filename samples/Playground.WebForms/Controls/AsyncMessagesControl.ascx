<%@ Control Language="C#" CodeBehind="AsyncMessagesControl.ascx.cs" Inherits="Playground.WebForms.Controls.AsyncMessagesControl" %>
<div class="async-messages">
 <asp:Repeater runat="server" DataSource="<%# Model.Messages %>">
  <HeaderTemplate>
   <ol>
  </HeaderTemplate>
  <ItemTemplate>
   <li><%# Container.DataItem %></li>
  </ItemTemplate>
  <FooterTemplate></ol></FooterTemplate>
 </asp:Repeater>
</div>
