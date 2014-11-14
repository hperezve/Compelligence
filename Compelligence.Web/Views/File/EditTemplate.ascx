<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.File>" %>
<% string formId = ViewData["Scope"].ToString() + "FileEditForm"; %>

<script type="text/javascript">
    $(function() {
        initializeForm('<%= ViewData["Scope"] %>', 'File');

    });

    $(document).ready(function() {
        $('#submit').click(function() {

            $("#FileTemplateConfirmDialog").dialog('open');
            return false; // prevents the default behaviour
        });
        $("#FileTemplateConfirmDialog").dialog({
            bgiframe: true,
            resizable: false,
            //        height: 140,
            autoOpen: false,
            modal: true,
            overlay: {
                backgroundColor: '#000',
                opacity: 0.5
            },
            buttons: {
                'Yes': function() {
                    //uploadFileComponent.submit();
                    $(this).dialog('close');
                    $('#<%= formId %>').submit();
                },
                'No': function() {
                    $(this).dialog('close');
                }
            }
        });
    });

    function verifyCkeditor() {

        if (CKEDITOR.instances['AdminTemplateFileEditFormHtmlDescription']) {
            CKEDITOR.instances.AdminTemplateFileEditFormHtmlDescription.destroy();
        }
        CKEDITOR.replace('AdminTemplateFileEditFormHtmlDescription');
        CKEDITOR.instances['AdminTemplateFileEditFormHtmlDescription'].updateElement();
    }
    var htmlEncode = function(value) {
        return $('<div/>').text(value).html();
    };
    function ConvertTheText() {
        $('#<%= formId %>HtmlDescription').ckeditorGet().destroy();
        alert('1: ' + $('#<%= formId %>HtmlDescription').val());
        $('#<%= formId %>HtmlDescription').val(htmlEncode($('#HtmlDescription').val()));
        alert('2: ' + $('#HtmlDescription').val());
    };
    function copyText() {
        var GetEditor = CKEDITOR.instances.AdminTemplateFileEditFormHtmlDescription.getData();
        $("#<%= formId %>HtmlDescription").val(GetEditor);
        CKEDITOR.instances.AdminTemplateFileEditFormHtmlDescription.destroy();

    }
    function confirmSubmit() { alert("Are you sure?"); }
    function presubmit() {
        alert('entro');
        $("#FileTemplateConfirmDialog").dialog('open');
    };
</script>

<%= Html.ValidationSummary() %>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "FileEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'File', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div style="height: 516px; overflow: auto">
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <input class="button" type="submit" value="Save" onclick="copyText()" />
            <%--<input class="button" type="button" value="TESTSUBMIT" onclick="presubmit();" />--%>
            <input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#<%= ViewData["Scope"] %>FileEditForm');" />
            <input class="button" type="button" value="Cancel" onclick="javascript: copyText();cancelEntity('<%= ViewData["Scope"] %>', 'File', '<%= ViewData["BrowseId"] %>', <%= ViewData["IsDetail"].ToString().ToLower() %>);" />
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div class="line">
            <div class="field">
                <label for="<%= formId %>FileName" class="required">
                    <asp:Literal ID="FileFileName" runat="server" Text="Name" />:</label>
                <%= Html.TextBox("FileName", null, new { id = formId + "FileName" })%>
                <%= Html.ValidationMessage("FileName", "*")%>
            </div>
            <div class="field">
                <label for="<%= formId %>AssignedTo" class="required">
                    <asp:Literal ID="FileAssignedTo" runat="server" Text="<%$ Resources:LabelResource, FileAssignedTo %>" />:</label>
                <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["UserIdlist"], string.Empty, new { id = formId + "AssignedTo" })%>
                <%= Html.ValidationMessage("AssignedTo", "*")%>
            </div>
        </div>
        <div class="line">
            <div class="field">
                <label for="<%= formId %>HtmlDescription" class="required">
                    <asp:Literal ID="FileHtmlDescription" runat="server" Text="<%$ Resources:LabelResource, FileDescription %>" />:</label>
                <%= Html.TextArea("HtmlDescription", new { id = formId + "HtmlDescription", Class = "ckeditor" })%>
                <%= Html.ValidationMessage("HtmlDescription", "*")%>

                <script type="text/javascript">
                    verifyCkeditor();
                </script>

            </div>
        </div>
    </fieldset>
</div>
<% } %>
<div id="FileTemplateConfirmDialog" title="Confirm upload" style="display: none">
    <p>
        <span class="ui-icon ui-icon-alert confirmDialog"></span>Do you want to upload a
        new version?</p>
</div>