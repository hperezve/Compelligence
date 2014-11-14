<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.CompetitorCriteria>" %>
<% string formId = ViewData["Scope"].ToString() + "CompetitorCriteriaEditForm"; %>

<script type="text/javascript">
        $(function() {
            initializeForm('#<%= formId %>','<%= ViewData["UserSecurityAccess"] %>' , '<%= ViewData["EntityLocked"] %>');
        });

        var setIndustryStandard = function(id) {
            var textStandard = document.getElementById(id);
            var index = textStandard.selectedIndex;
            if (index == 0) {
                $('#EnvironmentCompetitorCompetitorCriteriaEditFormIndustryStandard').prop("value", "");
            } else {

                var idCriteria = textStandard.options[textStandard.selectedIndex].value;
                var xmlhttp;
                var parameters = { id: idCriteria };
                var entitypath = $('#EnvironmentCompetitorCompetitorCriteriaEditFormPath').val();
                var url = entitypath + '/CompetitorCriteria.aspx/GetIndustryStandard/' + idCriteria;
                $.get(
                url,
                null,
                function(data) {
                    if (data != null && data != '') {
                        results = data;
                        $('#EnvironmentCompetitorCompetitorCriteriaEditFormIndustryStandard').prop("value", results);
                    }
                });               
            }
        }
        function updateCriterias(prmGroupId, prmSetId) {
            var groupId = $('#' + prmGroupId).val();
            var setId = $('#' + prmSetId).val();
            var IndustryId = '<%=ViewData["IndustryId"] %>';

            //alert(IndustryId + "-" + groupId + "-" + setId);
            $.ajax({
                type: "POST",

                url: '<%= Url.Action("GetCriteriasByGroupSet", "CompetitorCriteria")%>',
                dataType: "json",
                data: { IndustryId: IndustryId, CriteriaGroupId: groupId, CriteriaSetId: setId },
                success: function(data) {
                    $('#<%= formId %>CriteriaId option').remove();
                    $.each(data, function() {
                        var dropdownList = $('#<%= formId %>CriteriaId');
                        dropdownList.append($("<option></option>").prop("value", this.Value).text(this.Text));
                    }
                     );
                }
            }); //end-$.ajax           
        }
</script>

<%= Html.ValidationSummary()%>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"] + "CompetitorCriteria", null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "CompetitorCriteriaEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'CompetitorCriteria', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div style="height: 316px; overflow: auto">
    <fieldset>
        <legend><%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'CompetitorCriteria', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("CompetitorId")%>
        <%= Html.Hidden("CriteriaIdOld")%>
        <%= Html.Hidden("IndustryId", ViewData["IndustryId"])%>
         <div class="line">
            <div class="field">
                <label for="<%= formId %>CriteriaGroupId">
                    <asp:Literal ID="CompetitorCriteriaCriteriaGroupId" runat="server" Text="<%$ Resources:LabelResource, CompetitorCriteriaCriteriaGroupId %>" />:</label>
                <%= Html.CascadingParentDropDownList("CriteriaGroupId", (SelectList)ViewData["CriteriaGroupList"], string.Empty, Url.Action("GetCriteriaSetsByCriteriaGroup", "CompetitorCriteria"), formId, "CriteriaSetId", new string[] { "CriteriaId" }, "updateCriterias('" + formId + "CriteriaGroupId','" + formId + "CriteriaSetId')")%>
                <%= Html.ValidationMessage("CriteriaGroupId", "*")%>
            </div>
            <div class="field">
                <label for="<%= formId %>CriteriaSetId">
                    <asp:Literal ID="CompetitorCriteriaCriteriaSetId" runat="server" Text="<%$ Resources:LabelResource, CompetitorCriteriaCriteriaSetId %>" />:</label>
                <%string IndustryId = (string)ViewData["IndustryId"]; %>
                <%--<%= Html.CascadingParentDropDownList("CriteriaSetId", (SelectList)ViewData["CriteriaSetList"], string.Empty, Url.Action("GetCriteriasByCriteriaSet", "CompetitorCriteria"), new string[] { "IndustryId" }, formId, "CriteriaId")%>--%>
                <%= Html.CascadingParentDropDownList("CriteriaSetId", (SelectList)ViewData["CriteriaSetList"], string.Empty, true, Url.Action("GetCriteriasByCriteriaSet", "CompetitorCriteria"), formId, "CriteriaId", "updateCriterias('" + formId + "CriteriaGroupId','" + formId + "CriteriaSetId')")%>
                <%= Html.ValidationMessage("CriteriaSetId", "*")%>
            </div>
        </div>
        <div class="line">
            <div class="field">
                <label for="<%= formId %>CriteriaId" class="required">
                    <asp:Literal ID="CompetitorCriteriaCriteriaId" runat="server" Text="<%$ Resources:LabelResource, CompetitorCriteriaCriteriaId %>" />:</label>
                <%--<%= Html.CascadingChildDropDownList("CriteriaId", (SelectList)ViewData["CriteriaList"], string.Empty, formId)%>--%>
                <%= Html.DropDownList("CriteriaId", (SelectList)ViewData["CriteriaList"], string.Empty, new { id = formId + "CriteriaId", onchange = "javascript: setIndustryStandard(id);" })%>
                <%= Html.ValidationMessage("CriteriaId", "*")%>
            </div>
            <div class="field">
                <label for="<%= formId %>Value" class="required">
                    <asp:Literal ID="CompetitorCriteriaValue" runat="server" Text="<%$ Resources:LabelResource, CompetitorCriteriaValue %>" />:</label>
                <%= Html.TextBox("Value", null, new { id = formId + "Value"})%>
                <%= Html.ValidationMessage("Value", "*")%>
            </div>
        </div>
        <div class="line">
            <div class="field">
                <label for="<%= formId %>IndustryStandard">
                    <asp:Literal ID="ProductCriteriaIndustryStandard" runat="server" Text="<%$ Resources:LabelResource, ProductCriteriaIndustryStandard %>" />:</label>
                    <%= Html.TextBox("IndustryStandard", ViewData["ValStandard"], new { id = formId + "IndustryStandard", @readonly = "readonly" })%>
                    <%--<%= Html.CascadingChildDropDownList("IndustryStandard", (SelectLqist)ViewData["StandardCriteriaList"], string.Empty, formId)%>--%>
                <%= Html.ValidationMessage("IndustryStandard", "*")%>
                <%= Html.Hidden("Path", ViewData["Path"], new { id = formId + "Path" })%>
               
            </div>
        </div>
    </fieldset>
</div>
<% } %>
