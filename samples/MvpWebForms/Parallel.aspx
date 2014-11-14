<%@ Page Async="true" Language="C#" CodeBehind="Parallel.aspx.cs" Inherits="MvpWebForms.ParallelPage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
 <h2>Parallel Tasks</h2>
 <p>
  Start three tasks in parallel, each task taking at least 100ms to complete. 
  Nevertheless the overall elapsed time is under 300ms.
 </p>
 <h3>Chronology</h3>
 <uc:Parallel ID=ParallelControl runat="server" />
</asp:Content>
