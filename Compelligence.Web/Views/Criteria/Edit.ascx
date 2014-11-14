<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Criteria>" %>
<% string formId = ViewData["Scope"].ToString() + "CriteriaEditForm"; %>

<script src="<%= Url.Content("~/Scripts/jquery.autocomplete.min.js") %>" type="text/javascript"></script>

<style type="text/css">
    .contentFormLeft
    {
        float: left;
        width: 620px;
    }
    .contentFormRigth
    {
        float: left;
    }
</style>

<script type="text/javascript">

    function SetValueToMostDesire() {
//        var select = $("#<%= formId %>Type");
        //        var valueOfType = $("#<%= formId %>Type").val();
        var select = $("#Type");
        var valueOfType = $("#Type").val();
        if (valueOfType != '') {
            if (valueOfType == '<%= Compelligence.Domain.Entity.Resource.CriteriaType.List %>') {
                $('#DivMostDesiredValue').hide();
            }
            else {
                $('#DivMostDesiredValue').show();
            }
        }
        else {
            $('#DivMostDesiredValue').show();
        }
    };
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        SetValueToMostDesire();
        // SetValueToMostDesire();
    });
</script>

<script type="text/javascript">
    
    $(function() {
        //        $("#Type").change(function() {
        //            var selectedValue = $("Type option:selected").val();
        //            alert(selectedValue);
        //            if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.CriteriaType.List %>') {
        //                $('#DivMostDesiredValue').hide();
        //            } else {
        //                $('#DivMostDesiredValue').show();
        //            }
        //        });
       
       // SetValueToMostDesire();
        var Industryid = '<%=ViewData["IndustryId"]%>';
        $.ajax({
            type: "POST",
            url: '<%= Url.Action("GetCriteriaGroups", "Criteria")%>',
            dataType: "json",
            data: { IndustryId: Industryid },
            success: function(data) {
                $("#CriteriaGroupName").autocomplete(data, {
                    matchContains: true,
                    minChars: 0,
                    max: 200,
                    formatItem: function(row, i, max) {
                        return row.Text;
                    }
                });
            }
        });

        $("#CriteriaSetName").autocomplete([], {
            matchContains: true,
            minChars: 0,
            max: 200,
            formatItem: function(row, i, max) {
                return row.Text;
            }

        });

        $("#CriteriaGroupName").result(findValueCriteriaGroupCallback);

        $("#CriteriaSetName").result(findValueCriteriaSetCallback);

        $("#CriteriaGroupName").blur(function() {
            if ($(this).val() == '') {
                $("#CriteriaGroupId").val('');
                $("#CriteriaSetId").val('');
                //                    $("#CriteriaSetName").val('').setOptions({ data: [] }).attr("readonly", true);
            }
            //                else {
            //                    $("#CriteriaSetName").attr("readonly", false);
            //                }
        });

        $("#CriteriaSetName").blur(function() {
            if ($(this).val() == '') {
                $("#CriteriaSetId").val('');
            }
        });

        if ($('#CriteriaGroupId').val() != '' && $('#CriteriaGroupId').val() != null) {
            //                $('#CriteriaSetName').attr("readonly", false);
            loadCriteriaSetData();
        }
    });
    function findValueCriteriaGroupCallback(event, data, formatted) {
        $('#CriteriaGroupId').val(data.Value);
        $("#CriteriaSetId").val('');
        //		    $('#CriteriaSetName').attr("readonly", false).val('');
        if ($('#CriteriaGroupId')) {
            loadCriteriaSetData();
        }
    };

    function findValueCriteriaSetCallback(event, data, formatted) {
        $('#CriteriaSetId').val(data.Value);
    };

    function loadCriteriaSetData() {
        $.ajax({
            type: "POST",
            url: '<%= Url.Action("GetCriteriaSetByCriteriaGroup", "Criteria")%>/' + $('#CriteriaGroupId').val(),
            data: 'CriteriaGroupId=' + $('#CriteriaGroupId').val(),
            dataType: "json",
            success: function(data) {
                $("#CriteriaSetName").setOptions({ data: data });
            }
        });
    };

    function showLengthLimits() {
        var criteriaName = "Criteria name " + $('#<%= formId %>Name').val() + " is longer than 100 characters and has been truncated.";
        var criteriaGroupName = "Group name " + $('#CriteriaGroupName').val() + " is longer than 100 characters and has been truncated.";
        var criteriaSetName = "Set name " + $('#CriteriaSetName').val() + " is longer than 100 characters and has been truncated.";
        var benefit = "The Benefit... </br></br>\"" + $('#<%= formId %>Benefit').val() + "\"</br></br> ...is longer than 200 characters and has been truncated.";
        var cost = "The Cost... </br></br> \"" + $('#<%= formId %>Cost').val() + "\"</br></br> ...is longer than 200 characters and has been truncated.";
        var messageDialog = $('#AlertReturnMessageDialog');
        var content = "";
        if ($('#<%= formId %>Name').val().length > 100)
            content = criteriaName + '<br \>';
        if ($('#CriteriaGroupName').val().length > 100)
            content = content + criteriaGroupName + '<br \>';
        if ($('#CriteriaSetName').val().length > 100)
            content = content + criteriaSetName + '<br \>';
        if ($('#<%= formId %>Benefit').val().length > 200) {
            if (content != '') content = content + '<br \>';
            content = content + benefit + '<br \>';
        }
        if ($('#<%= formId %>Cost').val().length > 200) {
            if (content != '') content = content + '<br \>';
            content = content + cost + '<br \>';
        }
        if (content != null && content != "") {
            messageDialog.dialog();
            messageDialog.html(content);
            messageDialog.dialog("open");
        }
    }
</script>

<%= Html.ValidationSummary()%>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "CriteriaEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); showLengthLimits();}",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "');SetValueToMostDesire(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','Criteria', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Criteria', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OldType")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div class="contentFormEdit">
            <div class="contentFormLeft">
                <div class="line">
                    <div class="field">
                        <label for="<%= formId %>Name" class="required">
                            <asp:Literal ID="CriteriaName" runat="server" Text="<%$ Resources:LabelResource, CriteriaName %>" />:</label>
                        <%= Html.TextBox("Name", null, new { id = formId + "Name" })%>
                        <%= Html.ValidationMessage("Name", "*")%>
                    </div>
                    <div class="field">
                        <label for="<%= formId %>IndustryStandard">
                            <asp:Literal ID="IndustryStandard" runat="server" Text="<%$ Resources:LabelResource, CriteriaIndustryStandard %>" />:</label>
                        <%= Html.TextBox("IndustryStandard", null, new { id = formId + "IndustryStandard" })%>
                        <%= Html.ValidationMessage("IndustryStandard", "*")%>
                    </div>
                </div>
                <% if (ViewData["Scope"].Equals("EnvironmentIndustry"))
                   {%>
                <div class="line">
                    <div class="field">
                        <label for="CriteriaGroupName" class="required">
                            <asp:Literal ID="CriteriaCriteriaGroupName" runat="server" Text="<%$ Resources:LabelResource, CriteriaCriteriaGroupId %>" />:</label>
                        <%= Html.TextBox("CriteriaGroupName")%>
                        <%= Html.ValidationMessage("CriteriaGroupName", "*")%>
                        <%= Html.Hidden("CriteriaGroupId")%>
                        <%--<%= Html.CascadingParentDropDownList("IndustryId", (SelectList)ViewData["IndustryList"], string.Empty, Url.Action("GetCompetitorsByIndustry", "DealSupport"), "CompetitorId", new string[] { "ProductId" })%>
                <%= Html.ValidationMessage("IndustryId", "*") %>--%>
                    </div>
                    <div class="field">
                        <label for="CriteriaSetName" class="required">
                            <asp:Literal ID="CriteriaCriteriaSetName" runat="server" Text="<%$ Resources:LabelResource, CriteriaCriteriaSetId %>" />:</label>
                        <%= Html.TextBox("CriteriaSetName")%>
                        <%= Html.ValidationMessage("CriteriaSetName", "*")%>
                        <%= Html.Hidden("CriteriaSetId")%>
                        <%--<%= Html.CascadingParentDropDownList("CompetitorId", (SelectList)ViewData["CompetitorList"], string.Empty, true, Url.Action("GetProductsByCompetitor", "DealSupport"), new string[] { "IndustryId" }, "ProductId")%>
                <%= Html.ValidationMessage("CompetitorId", "*") %>--%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="<%= formId %>Visible">
                            <asp:Literal ID="CriteriaVisible" runat="server" Text="<%$ Resources:LabelResource, CriteriaVisible %>" />:</label>
                        <%= Html.DropDownList("Visible", (SelectList)ViewData["VisibleList"],new { id = formId + "Visible" })%>
                        <%= Html.ValidationMessage("Visible", "*")%>
                    </div>
                </div>
                <%}%>
                <%--            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Visible" class="required">
                        <asp:Literal ID="CriteriaVisible" runat="server" Text="<%$ Resources:LabelResource, CriteriaVisible %>" />:</label>
                    <%= Html.DropDownList("Visible", (SelectList)ViewData["CriteriaVisibleList"], string.Empty, new { id = formId + "Visible" })%>
                    <%= Html.ValidationMessage("Visible", "*") %>
                </div>
            </div>--%>
                <div class="line">
                    <div class="field">
                        <label for="<%= formId %>Type">
                        <asp:Literal ID="CriteriaType" runat="server" Text="<%$ Resources:LabelResource, CriteriaType %>" />:</label>
                       <%-- <%= Html.DropDownList("Type", (SelectList)ViewData["CriteriaTypeList"], new { id = formId + "Type", onchange="SetValueToMostDesire(this);" })%>--%>
                       <%-- <%= Html.CascadingParentDropDownList("Type", (SelectList)ViewData["CriteriaTypeList"], string.Empty, Url.Action("GetMostDesiredValueByType", "Criteria"), "MostDesiredValue")%>--%>
                        <%= Html.CascadingParentDropDownListWP("Type", (SelectList)ViewData["CriteriaTypeList"], null, false, Url.Action("GetMostDesiredValue", "Criteria"), new string[] { }, string.Empty, formId + "MostDesiredValue", new string[] { }, null, "SetValueToMostDesire();", string.Empty)%>
                        <%--<%= Html.CascadingParentDropDownList("Type", (SelectList)ViewData["CriteriaTypeList"], string.Empty, Url.Action("GetMostDesiredValue", "Criteria"), formId, "MostDesiredValue")%>--%>
                        <%= Html.ValidationMessage("Type", "*")%>
                    </div>
                    <div id="DivMostDesiredValue"  class="field">
                        <label for="<%= formId %>MostDesiredValue">
                        <asp:Literal ID="CriteriaMostDesiredValue" runat="server" Text="<%$ Resources:LabelResource, CriteriaMostDesiredValue %>" />:</label>
                        <%--<%= Html.DropDownList("MostDesiredValue", (SelectList)ViewData["CriteriaMostDesiredValueList"], string.Empty, new { id = formId + "MostDesiredValue" })%>--%>
                        <%= Html.CascadingChildDropDownList("MostDesiredValue", (SelectList)ViewData["CriteriaMostDesiredValueList"], string.Empty, formId)%>
                        <%= Html.ValidationMessage("MostDesiredValue", "*")%>
                    </div>
                    
                </div>
                <div class="line">
                    <div class="field">
                        <label for="<%= formId %>Relevancy">
                        <asp:Literal ID="CriteriaRelevancy" runat="server" Text="<%$ Resources:LabelResource, CriteriaRelevancy %>" />:</label>
                        <%= Html.DropDownList("Relevancy", (SelectList)ViewData["CriteriaRelevancyList"], new { id = formId + "Relevancy" })%>
                        <%= Html.ValidationMessage("Relevancy", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="<%= formId %>Description">
                            <asp:Literal ID="CriteriaDescription" runat="server" Text="<%$ Resources:LabelResource, CriteriaDescription %>" />:</label>
                        <%= Html.TextArea("Description", new { id = formId + "Description"})%>
                        <%= Html.ValidationMessage("Description", "*")%>
                    </div>
                </div>
            </div>
            <div class="contentFormRigth">
                <div class="field">
                    <label for="<%= formId %>Benefit">
                        <asp:Literal ID="CriteriaBenefit" runat="server" Text="<%$ Resources:LabelResource, CriteriaBenefit %>" />:</label>
                    <%= Html.TextArea("Benefit", null, new { id = formId + "Benefit", Style = "height:130px;" })%>
                    <%= Html.ValidationMessage("Benefit", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Cost">
                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:LabelResource, CriteriaCost %>" />:</label>
                    <%= Html.TextArea("Cost", null, new { id = formId + "Cost", Style = "height:130px;" })%>
                    <%= Html.ValidationMessage("Cost", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>    
<% } %>
