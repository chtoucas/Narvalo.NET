<%@ Control Language="C#" CodeBehind="AsyncTplControl.ascx.cs" Inherits="Playground.Controls.AsyncTplControl" %>
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
