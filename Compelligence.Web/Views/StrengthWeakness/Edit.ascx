<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.StrengthWeakness>" %>
<% string formId = ViewData["Scope"].ToString() + "StrengthWeaknessEditForm"; %>

<script type="text/javascript">


   var ResizeHeightForm = function() {
       var div = document.getElementById('ValidationSummaryStrengthWeakness');
       var ul = div.getElementsByTagName('ul');
       if ((ul != null) || (ul != undefined)) {
           var lis = div.getElementsByTagName('li');
           if (lis.length > 0) {
               var newHeigth = 328 - 58 - (10 * lis.length);
               var edt = $('#StrengthWeaknessEditFormInternalContent').css('height', newHeigth + 'px');
           }
       }
   };

    $(window).bind('resize', function() {

    $('#StrengthWeaknessIndexTwo').width($(window).width() - 9);
        
    });

</script>
<script type="text/javascript">
        $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>', ['dt:#<%= formId %>LastDateFrm']);
        ResizeHeightForm();
       });
</script>


<div id="ValidationSummaryStrengthWeakness">
    <%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "StrengthWeaknessEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "', ['dt:#" + formId + "LastDateFrm']); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','StrengthWeakness', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + "); }",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>

    <fieldset>
        <legend><%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');resetMultiSelect();" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'StrengthWeakness', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OldIndustriesIds")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="StrengthWeaknessEditFormInternalContent" class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="StrengthWeaknessName" runat="server" Text="<%$ Resources:LabelResource, StrengthWeaknessName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name" })%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Type" class="required">
                        <asp:Literal ID="StrengthWeaknessType" runat="server" Text="<%$ Resources:LabelResource, StrengthWeaknessType %>" />:</label>
                    <%= Html.DropDownList("Type",(SelectList)ViewData["TypeList"], string.Empty, new {id = formId + "Type"}) %>
                    <%= Html.ValidationMessage("Type", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>LastDateFrm">
                        <asp:Literal ID="StrengthWeaknessLastDate" runat="server" Text="<%$ Resources:LabelResource, StrengthWeaknessLastDate %>" />:</label>
                    <%= Html.TextBox("LastDateFrm", null, new { id = formId + "LastDateFrm" })%>
                    <%= Html.ValidationMessage("LastDateFrm", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field" >
                    <label for="<%= formId %>Description" class="required"><asp:Literal ID="StrengthWeaknessDescription" runat="server" Text="<%$ Resources:LabelResource, StrengthWeaknessDescription %>"></asp:Literal>:</label>
                    <%= Html.TextArea("Description", new  { id = formId+"Description"})%>
                    <%= Html.ValidationMessage("Description","*") %>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>IndustryIds">
                        <asp:Literal ID="IndustryIds" runat="server" Text="<%$ Resources:LabelResource, StrengthWeaknessIndustry %>" />:</label>
                    <%= Html.ListBox("IndustryIds", (MultiSelectList)ViewData["IndustryIdList"], new { id = formId + "IndustryIds", Multiple = "true", style = "height:100px;" })%>
                    <%= Html.ValidationMessage("StrengthWeaknessIndustry", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
<% } %>
