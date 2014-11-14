<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Library>" %>

<% string formId = ViewData["Scope"].ToString() + "LibraryEditForm"; %>

<script type="text/javascript">
        $(function() {
            initializeForm('#<%= formId %>', ['LibraryPublishDateFrm']);
        });
</script>

<%= Html.ValidationSummary() %>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "LibraryEditFormContent",
               OnBegin = "showLoadingDialog",
               OnComplete = "hideLoadingDialog",
               OnSuccess = "function(){ initializeForm('#" + formId + "', ['LibraryPublishDateFrm']); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'Library', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}", 
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<fieldset>
    <legend>
        <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
    <div class="buttonLink">
        <input type="submit" value="Save" />
        <input type="button" value="Reset" onclick="javascript: resetFormFields('#<%= formId %>');" />
        <input type="button" value="Cancel" onclick="javascript: cancelEntity('<%= ViewData["Scope"] %>', 'Library', '<%= ViewData["BrowseId"] %>', <%= ViewData["IsDetail"].ToString().ToLower() %>);" />
    </div>
    <%= Html.Hidden("Scope")%>
    <%= Html.Hidden("BrowseId")%>
    <%= Html.Hidden("IsDetail")%>
    <%= Html.Hidden("OperationStatus")%>
    <%= Html.Hidden("HeaderType")%>
    <%= Html.Hidden("DetailFilter")%>
    <%= Html.Hidden("Id", default(decimal))%>
    <div class="line">
        <div class="field">
            <label for="Name" class="required">
                <asp:Literal ID="LibraryName" runat="server" Text="<%$ Resources:LabelResource, LibraryName %>" />:</label>
            <%= Html.TextBox("Name")%>
            <%= Html.ValidationMessage("Name", "*")%>
            <%--<label for="AssignedTo" class="required">
                <asp:Literal ID="LibraryOwnerId" runat="server" Text="<%$ Resources:LabelResource, LibraryOwnerId %>" />:</label>
            <%=Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty)%>
            <%=Html.ValidationMessage("AssignedTo", "*")%>
            <label for="Status">
                <asp:Literal ID="LibraryStatus" runat="server" Text="<%$ Resources:LabelResource, LibraryStatus %>" />:</label>
            <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty)%>
            <%= Html.ValidationMessage("Status", "*")%>--%>            
        </div>
        <div class="field">
            <label for="Type" class="required">
                <asp:Literal ID="LibraryType" runat="server" Text="<%$ Resources:LabelResource, LibraryType %>" />:</label>
            <%= Html.DropDownList("Type", (SelectList)ViewData["TypeList"], string.Empty)%>
            <%= Html.ValidationMessage("Type", "*")%>
        </div>
        
    </div>
    <div class="line">
        
    </div>
    <div class="line">
        <div class="field">
            <label for="Source">
                <asp:Literal ID="LibrarySource" runat="server" Text="<%$ Resources:LabelResource, LibrarySource %>" />:</label>
            <%= Html.TextBox("Source")%>
            <%= Html.ValidationMessage("Source", "*")%>
        </div>
        <div class="field">
            <label for="DateAddedFrm" class="required">
                <asp:Literal ID="LibraryPublishDate" runat="server" Text="<%$ Resources:LabelResource, LibraryPublishDate %>" />:</label>
            <%= Html.TextBox("LibraryPublishDateFrm")%>
            <%= Html.ValidationMessage("LibraryPublishDateFrm", "*")%>
        </div>
        
    </div>
</fieldset>
<% } %>
