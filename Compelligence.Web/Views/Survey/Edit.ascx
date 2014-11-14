<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Quiz>" %>
<% string formId = ViewData["Scope"].ToString() + "SurveyEditForm"; %>

<link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />

       <script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>

    <script type="text/javascript">
        var loadMiniHtmlEditor = function() {
            $('#<%=formId%>Description').cleditor();
            $(".cleditorMain iframe").contents().find('body').bind('keyup', function() {
                var v = $(this).text(); // or .html() if desired
                $('#<%=formId%>Description').html(v);
            });
        };

        $(function() {
            loadMiniHtmlEditor();

        });
        
      
    </script>
<script type="text/javascript">
    var ResizeHeightForm = function() {
    var div = document.getElementById('ValidationSummarySurvey');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#SurveyEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };

    $(window).bind('resize', function() {

        $('#surveyIndexTwo').width($(window).width() - 9);

    });
</script>
<script type="text/javascript">
    var showDescription = function() {
    var selectedValue = $('#<%= formId %>Type > option:selected').prop('value');
        if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.SurveyType.Long %>' ||
            selectedValue == '<%= Compelligence.Domain.Entity.Resource.SurveyType.SalesForce %>') {
            $('#Description').show();
        }
        else {
            $('#Description').hide();
        }
    };
</script>

<script type="text/javascript">
    var visible = function() {
    var selectedValue = $('#<%= formId %>Type > option:selected').prop('value');
    var statusValue = $('#<%= formId %>Status > option:selected').prop('value');
    var visibleValue = $('#<%= formId %>Visible > option:selected').prop('value');

        if (selectedValue != '') {
            if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.SurveyType.Long %>') {
                //$('#<%= formId %>Visible').attr("disabled", "disabled");
                $('#SalesForceType').css("display", "none");
                if (statusValue == '<%= Compelligence.Domain.Entity.Resource.SurveyStatus.Incomplete %>') {
                    $('#<%= formId %>Visible').prop("value", '<%= Compelligence.Domain.Entity.Resource.SurveyVisible.No %>'); //visible = no
                }
             }
            else if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.SurveyType.Short %>') //selectedValue=short
            {
                $('#<%= formId %>Visible').removeAttr("disabled");
                $('#SalesForceType').css("display", "none");
                if (statusValue == '<%= Compelligence.Domain.Entity.Resource.SurveyStatus.Incomplete %>') {
                    $('#<%= formId %>Visible').prop("value", '<%= Compelligence.Domain.Entity.Resource.SurveyVisible.No %>');
                }
                if (visibleValue == '<%= Compelligence.Domain.Entity.Resource.SurveyVisible.Yes %>') {
                    $('#<%= formId %>Status').prop("value", '<%= Compelligence.Domain.Entity.Resource.SurveyStatus.Complete %>')
                }

            }
            else if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.SurveyType.SalesForce %>') //selectedValue=short
            {
                $('#SalesForceType').css("display", "block");
                if (statusValue == '<%= Compelligence.Domain.Entity.Resource.SurveyStatus.Incomplete %>') {
                    $('#<%= formId %>Visible').prop("value", '<%= Compelligence.Domain.Entity.Resource.SurveyVisible.No %>');
                }
                if (visibleValue == '<%= Compelligence.Domain.Entity.Resource.SurveyVisible.Yes %>') {
                    $('#<%= formId %>Status').prop("value", '<%= Compelligence.Domain.Entity.Resource.SurveyStatus.Complete %>')
                }
            }
        }
    };
    
</script>

<script type="text/javascript">
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        visible();
       showDescription();
       ResizeHeightForm();

       //CKEditor setting
    });
</script>

<div id="ValidationSummarySurvey">
<%= Html.ValidationSummary()%>
</div>

<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "SurveyEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "');showDescription();loadMiniHtmlEditor(); visible();ResizeHeightForm();executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','Survey', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div id="surveyIndexTwo" class="indexTwo">
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <%--<%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>--%>
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Survey', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OldTitle")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="SurveyEditFormInternalContent" class="contentFormEdit resizeMe">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Title" class="required">
                        <asp:Literal ID="SurveyTitle" runat="server" Text="<%$ Resources:LabelResource, SurveyTitle %>" />:</label>
                    <%= Html.TextBox("Title", null, new { id = formId + "Title"})%>
                    <%= Html.ValidationMessage("Title", "*")%>
                </div>

                <div class="field">
                    <label for="<%= formId %>Status" class="required">
                        <asp:Literal ID="SurveyStatus" runat="server" Text="<%$ Resources:LabelResource, SurveyStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], new { id = formId + "Status", onchange = "javascript: visible();"})%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Type" class="required">
                        <asp:Literal ID="SurveyType" runat="server" Text="<%$ Resources:LabelResource, SurveyType %>" />:</label>
                    <%= Html.DropDownList("Type", (SelectList)ViewData["TypeList"], new { id = formId + "Type", onchange = "javascript: visible();showDescription();" })%>
                    <%= Html.ValidationMessage("Type", "*")%>
                </div>            
                <div class="field" id="SalesForceType" style="display: none">
                    <label for="<%= formId %>SalesForceType">
                        <asp:Literal ID="SalesForceType" runat="server" Text="<%$ Resources:LabelResource, SurveySalesForceType %>" />:</label>
                    <%= Html.DropDownList("SalesForceType", (SelectList)ViewData["SalesForceTypeList"], string.Empty, new { id = formId + "SalesForceType" })%>
                    <%= Html.ValidationMessage("SalesForceType", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="SurveyAssignedTo" runat="server" Text="<%$ Resources:LabelResource, SurveyAssignedTo %>" />:</label>
                    <%=Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%=Html.ValidationMessage("AssignedTo", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field" id="Description" >
                    <label for="<%= formId %>Description">
                        <asp:Literal ID="SurveyDescription" runat="server" Text="<%$ Resources:LabelResource, SurveyDescription %>" />:</label>
                    <%= Html.TextArea("Description", new { id = formId + "Description" })%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>MetaData">
                        <asp:Literal ID="SurveyMetaData" runat="server" Text="<%$ Resources:LabelResource, SurveyMetaData %>" />:</label>
                    <%= Html.TextArea("MetaData", new { id = formId + "MetaData" })%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>

