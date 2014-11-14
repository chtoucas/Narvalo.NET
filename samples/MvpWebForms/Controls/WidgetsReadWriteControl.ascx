<%@ Control Language="C#" CodeBehind="WidgetsReadWriteControl.ascx.cs" Inherits="MvpWebForms.Controls.WidgetsReadWriteControl" %>
<div class="edit-widget">
 <asp:FormView runat="server" DataSourceID="WidgetDataSource" DefaultMode="ReadOnly"
  DataKeyNames="Id" AllowPaging="true">
  <ItemTemplate>
   <ul>
    <li>ID: <%# Eval("Id") %></li>
    <li>Name: <%# Eval("Name") %></li>
    <li>Description: <%# Eval("Description") %></li>
   </ul>
   <asp:Button runat="server" CommandName="New" Text="New" />
   <asp:Button runat="server" CommandName="Edit" Text="Edit" />
   <asp:Button runat="server" CommandName="Delete" Text="Delete" />
  </ItemTemplate>
  <EditItemTemplate>
   <fieldset>
    <legend>Widget Details</legend>
    <ol>
     <li class="widget-id">
      <asp:Label runat="server" AssociatedControlID="widgetId">ID:</asp:Label>
      <asp:TextBox runat="server" ID="widgetId" Text='<%# Bind("Id") %>' ReadOnly="true" />
     </li>
     <li class="widget-name">
      <asp:Label runat="server" AssociatedControlID="widgetName">Name:</asp:Label>
      <asp:TextBox runat="server" ID="widgetName" Text='<%# Bind("Name") %>' />
      <div class="validation">
       <asp:RequiredFieldValidator runat="server" ValidationGroup="Edit"
        ControlToValidate="widgetName" Display="Dynamic"
        ErrorMessage="Please enter a value for Name" />
      </div>
     </li>
     <li class="widget-name">
      <asp:Label runat="server" AssociatedControlID="widgetDescription">Description:</asp:Label>
      <asp:TextBox runat="server" ID="widgetDescription" Text='<%# Bind("Description") %>'
       TextMode="MultiLine" />
      <div class="validation">
       <asp:RequiredFieldValidator runat="server" ValidationGroup="Edit"
        ControlToValidate="widgetDescription" Display="Dynamic"
        ErrorMessage="Please enter a value for Description" />
      </div>
     </li>
     <li class="action save">
      <asp:Button runat="server" CommandName="Update" Text="Save" ValidationGroup="Edit" />
     </li>
     <li class="action cancel">
      <asp:Button runat="server" CommandName="Cancel" Text="Cancel" />
     </li>
    </ol>
   </fieldset>
  </EditItemTemplate>
  <InsertItemTemplate>
   <fieldset>
    <legend>New Widget Details</legend>
    <ol>
     <li class="widget-name">
      <asp:Label runat="server" AssociatedControlID="widgetName">Name:</asp:Label>
      <asp:TextBox runat="server" ID="widgetName" Text='<%# Bind("Name") %>' />
     </li>
     <li class="widget-name">
      <asp:Label runat="server" AssociatedControlID="widgetDescription">Description:</asp:Label>
      <asp:TextBox runat="server" ID="widgetDescription" Text='<%# Bind("Description") %>'
       TextMode="MultiLine" />
     </li>
     <li class="action save">
      <asp:Button runat="server" CommandName="Insert" Text="Save" />
     </li>
     <li class="action cancel">
      <asp:Button runat="server" CommandName="Cancel" Text="Cancel" />
     </li>
    </ol>
   </fieldset>
  </InsertItemTemplate>
  <EmptyDataTemplate>
   <p class="empty">No widget could be found</p>
   <asp:Button runat="server" CommandName="New" Text="New" />
  </EmptyDataTemplate>
 </asp:FormView>

 <mvp:PageDataSource
  ID="WidgetDataSource"
  runat="server"
  EnablePaging="true"
  DataObjectTypeName="MvpWebForms.Entities.Widget"
  ConflictDetection="CompareAllValues"
  OldValuesParameterFormatString="original{0}"
  SelectMethod="GetWidgets"
  SelectCountMethod="CountWidgets"
  UpdateMethod="UpdateWidget"
  InsertMethod="InsertWidget"
  DeleteMethod="DeleteWidget">
 </mvp:PageDataSource>
</div>
