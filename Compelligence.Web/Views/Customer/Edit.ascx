<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Customer>" %>
<% string formId = ViewData["Scope"].ToString() + "CustomerEditForm"; %>

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
    var div = document.getElementById('ValidationSummaryCustomer');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#CustomerEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };
</script>
<script type="text/javascript">
        $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        ResizeHeightForm();
    });

    var loadUrl = function() {
    var url = $('#<%=formId %>Website').prop('value');
        if (url != '') {
            if (url.indexOf("http://") == -1) {
                url = "http://" + url;
            }
            window.open(url, "Website", "scrollbars=1,width=900,height=500")
        }
    };
</script>

<script type="text/javascript">
    var visibleLink = function() {
    var url = $('#<%=formId %>Website').prop('value');
        if (url != '') {
            $('#CustomerShowLink').css("display", "block");
        }
    };
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        visibleLink();
        ResizeHeightForm();
    });
</script>

<div id="ValidationSummaryCustomer">
<%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "CustomerEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "');loadMiniHtmlEditor();ResizeHeightForm(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','Customer', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");visibleLink();}",
               
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Customer', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OldName")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="CustomerEditFormInternalContent" class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="CustomerName" runat="server" Text="<%$ Resources:LabelResource, CustomerName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name" })%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="CustomerAssignedTo" runat="server" Text="<%$ Resources:LabelResource, CustomerAssignedTo %>" />:</label>
                    <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%= Html.ValidationMessage("AssignedTo", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Status">
                        <asp:Literal ID="CustomerStatus" runat="server" Text="<%$ Resources:LabelResource, CustomerStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id = formId + "Status" })%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
            </div>
            <%--<div class="line">--%>
            <%--        
        <div class="field">
            <label for="Type" class="required">
                <asp:Literal ID="CustomerType" runat="server" Text="<%$ Resources:LabelResource, CustomerType %>" />:</label>
            <%= Html.DropDownList("Type", (SelectList)ViewData["TypeList"], string.Empty)%>
            <%= Html.ValidationMessage("Type", "*")%>
        </div>
    </div>--%>
    
             <div class="line">
                <div class="field">
                    <label for="<%= formId %>Website">
                        <asp:Literal ID="CustomerWebsite" runat="server" Text="<%$ Resources:LabelResource, CustomerWebsite %>" />:</label>
                    <%= Html.TextBox("Website", null, new { id = formId + "Website" })%>                    
                    <%= Html.ValidationMessage("Website", "*")%>
                    <div id="CustomerShowLink" style="display: none">
                        <a href="javascript:void(0);" onclick="loadUrl();">Go To URL</a>
                    </div>
                </div>
            </div>
        
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Description" class="required">
                        <asp:Literal ID="CustomerDescription" runat="server" Text="<%$ Resources:LabelResource, CustomerDescription %>" />:</label>
                    <%= Html.TextArea("Description", new { id = formId + "Description"})%>
                    <%= Html.ValidationMessage("Description", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>MetaData">
                        <asp:Literal ID="CustomerMetaData" runat="server" Text="<%$ Resources:LabelResource, CustomerMetaData %>" />:</label>
                    <%= Html.TextArea("MetaData", new { id = formId + "MetaData" })%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>