<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/BackEndPopupSite.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.Domain.Entity.File>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">

    <% if (Model != null)
       {
    %>

    <script type="text/javascript">
        $(function() {
            location.href = '<%= Url.Content("~" + ConfigurationSettings.AppSettings["LibraryFilePath"]) + Model.PhysicalName %>';
         });
    </script>

    <% } %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <% if (Model == null)
       {
    %>
    <h2>    
        <div class="field" id="StartDateForm" >
            <label for="LibraryMessageFile">
            <asp:Literal ID="LibraryMessageFile" runat="server" Text="<%$ Resources:LabelResource, LibraryMessageFile %>" /></label>               
         </div>
    </h2>
    <% }
       else
       { %>
    
        <div class="field" id="Div1" >
            <label for="LibraryMessageLoading">
            <asp:Literal ID="LibraryMessageLoading" runat="server" Text="<%$ Resources:LabelResource, LibraryMessageLoading %>" /></label>               
         </div>
        
        
    <% } %>
</asp:Content>
