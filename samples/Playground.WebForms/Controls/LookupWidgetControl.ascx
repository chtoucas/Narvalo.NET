<%@ Control Language="C#" CodeBehind="LookupWidgetControl.ascx.cs" Inherits="Playground.Controls.LookupWidgetControl" %>
<div class="lookup-widget">
 <fieldset>
  <legend>Enter ID or Name of widget</legend>
  <p>
   <asp:Label runat="server" AssociatedControlID="WidgetId">ID</asp:Label>
   <asp:TextBox runat="server" ID="WidgetId" MaxLength="3" />
   <div class="validators">
    <asp:CompareValidator runat="server" ControlToValidate="widgetId"
     ValidationGroup="LookupWidget"
     Display="Dynamic" Type="Integer" Operator="DataTypeCheck"
     ErrorMessage="ID must be a valid whole number" />
    <asp:RangeValidator runat="server" ControlToValidate="widgetId"
     ValidationGroup="LookupWidget"
     Display="Dynamic" Type="Integer" MinimumValue="1" MaximumValue="999"
     ErrorMessage="ID must be a positive whole number" />
   </div>
  </p>
  <p>
   <asp:Button runat="server" Text="Find" ValidationGroup="LookupWidget" OnClick="Find_Click" />
  </p>
 </fieldset>
 <div class="results">
  <asp:DetailsView runat="server" DataSource="<%# Model.Widgets %>"
   EmptyDataText="No matching results found"
   Visible="<%# Model.ShowResults %>" />
 </div>
</div>
