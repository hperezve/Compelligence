<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.MarketType>" %>
<% string formId = ViewData["Scope"].ToString() + "MarketTypeEditForm"; %>
<script type="text/javascript">
    var ResizeHeightForm = function() {
    var div = document.getElementById('ValidationSummaryMarketType');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#MarketTypeEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };

    $(window).bind('resize', function() {

        $('#marketTypeIndexTwo').width($(window).width() - 9);

    });
</script>
<script type="text/javascript">
        $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        ResizeHeightForm();
        });
        $(function() {
            //alert('#<%=formId%>ImageUrl');
            if ($('#<%= ViewData["Scope"] %>FileCheckIn').length) {
                initializeUploadField('<%= Url.Action("UploadImage", "File") %>',
             '<%= ViewData["Scope"] %>',
              'FileDetail',
               '#<%= ViewData["Scope"] %>FileCheckIn',
               '#<%= ViewData["Scope"] %>FileUploadResult',
                '#<%=formId%>ImageUrl',
                 '<%= ViewData["Container"] %>',
                  '<%= ViewData["HeaderType"] %>',
                   '<%= ViewData["DetailFilter"] %>', false);
            }
        });
</script>

<div id="ValidationSummaryMarketType">
<%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "MarketTypeEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); ResizeHeightForm(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','MarketType', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div id="marketTypeIndexTwo" class="indexTwo">
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'MarketType', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OriginalStatus")%>
        <%= Html.Hidden("OldName")%>
        <%= Html.Hidden("OldParent")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="MarketTypeEditFormInternalContent"  class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="MarketTypeName" runat="server" Text="<%$ Resources:LabelResource, MarketTypeName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name" })%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="MarketTypeAssignedTo" runat="server" Text="<%$ Resources:LabelResource, MarketTypeAssignedTo %>" />:</label>
                    <%=Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%=Html.ValidationMessage("AssignedTo", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Status">
                        <asp:Literal ID="MarketTypeStatus" runat="server" Text="<%$ Resources:LabelResource, MarketTypeStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id = formId + "Status" })%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Parent">
                        <asp:Literal ID="MarketTypeParent" runat="server" Text="<%$ Resources:LabelResource, MarketTypeParent %>" />:</label>
                    <%= Html.DropDownList("Parent", (SelectList)ViewData["MarketTypeParentList"], string.Empty, new { id = formId + "Parent" })%>
                    <%= Html.ValidationMessage("Parent", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Website">
                        <asp:Literal ID="CompetitorWebsite" runat="server" Text="<%$ Resources:LabelResource, MarketTypeWebsite %>" />:</label>
                    <%= Html.TextBox("Website", null, new { id = formId + "Website" })%>
                    <%= Html.ValidationMessage("Website", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>ImageUrl">
                        <asp:Literal ID="MarketTypeImageUrl" runat="server" Text="<%$ Resources:LabelResource, MarketTypeImageUrl %>" />:</label>
                    <%= Html.TextBox("ImageUrl",null, new { id = formId + "ImageUrl" })%>
                    <%= Html.ValidationMessage("ImageUrl", "*")%>
                    
                    <div id="MarketTypeImageUrl" style="width: 100px; height: 100px; background-color: White;
                        margin-top: 10px; float: left; text-align: center;">
                        <% var StaticImageUrl = Model != null ? Html.Encode(Model.ImageUrl): string.Empty;
                         %>
                        <img id="ImageUrl" src="<%=StaticImageUrl %>" width="75" height="75" />
                    </div>
                    <div style="float: left; margin-top: 10px; margin-left: 10px">
                        <input type="button" value="Show" onclick="loadImageUrl('<%=StaticImageUrl %>');return false;" />
                        <input class="button" id="<%= ViewData["Scope"] %>FileCheckIn" type="button" value="Upload" />
                    </div>
                </div> 
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Description">
                        <asp:Literal ID="MarketTypeDescription" runat="server" Text="<%$ Resources:LabelResource, MarketTypeDescription %>" />:</label>
                    <%= Html.TextArea("Description", new { id = formId + "Description" })%>
                    <%= Html.ValidationMessage("Description", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>MetaData">
                        <asp:Literal ID="MarketTypeMetaData" runat="server" Text="<%$ Resources:LabelResource, MarketTypeMetaData %>" />:</label>
                    <%= Html.TextArea("MetaData", new { id = formId + "MetaData" })%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
