<%@ Page Async="true" Language="C#" CodeBehind="Parallel.aspx.cs" Inherits="Playground.ParallelPage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
 <h2>Parallel Tasks</h2>
 <p>
  Start three tasks in parallel, each task taking at least 100ms to complete. 
  Nevertheless the overall time spent is under 300ms.
 </p>
 <h3>Chronology</h3>
 <uc:Parallel runat="server" />
</asp:Content>
