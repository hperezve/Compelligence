<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.ProductFamily>" %>
<% string formId = ViewData["Scope"].ToString() + "ProductFamilyEditForm"; %>
    <script src="<%= Url.Content("~/Scripts/jquery.autocomplete.min.js") %>" type="text/javascript"></script>
<script type="text/javascript">
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>', []);
        ResizeHeightForm();
    });
    var ResizeHeightForm = function() {
        var div = document.getElementById('ValidationSummaryProductFamily');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#ProductFamilyEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };
</script>
<script type="text/javascript">
    $(function() {
    //Fill product family by competitor
    $.ajax({
        type: "POST",
        url: '<%= Url.Action("GetProductsFamily", "ProductFamily")%>/' + getIdValue('Environment', 'Competitor'),
        dataType: "json",
        success: function(data) {
            $("#<%=formId%>Name").autocomplete(data, {
                matchContains: true,
                minChars: 0,
                max: 200,

                formatItem: function(row, i, max) {
                    return row.Text;
                }
            });
        }
    });        
    $("input:visible:enabled:first").focus(); //it's for click necesary on Autocomplete field
    });
</script>
<div id="ValidationSummaryProductFamily">
<%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "ProductFamilyEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "');ResizeHeightForm(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'ProductFamily', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div class="indexTwo">
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'ProductFamily', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("Container") %>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <%= Html.Hidden("UserId") %>
        <div id="ProductFamilyEditFormInternalContent" class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>ProductFamily" class="required">
                        <asp:Literal ID="ProductFamily" runat="server" Text="<%$ Resources:LabelResource, ProductFamilyName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name" })%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Description">
                        <asp:Literal ID="ProductFamilyDescription" runat="server" Text="<%$ Resources:LabelResource, ProductFamilyDescription %>" />:</label>
                    <%= Html.TextArea("Description", new { id = formId + "Description" })%>
                    <%= Html.ValidationMessage("Description", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
