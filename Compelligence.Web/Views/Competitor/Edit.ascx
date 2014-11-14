<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Competitor>" %>
<% string formId = ViewData["Scope"].ToString() + "CompetitorEditForm"; %>

<link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />

<script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>

<script type="text/javascript">
    var hiddenCompetitorSubTab = function() {    
        var showSubTab = $("#showTabsHidden").val();
        if (showSubTab == 'false') {
            if (CompetitorSubtabs != null && CompetitorSubtabs != undefined && CompetitorSubtabs != '') {
                var idOfCompetitorsTabs = CompetitorSubtabs.id;
                var subCMTab = $('#' + idOfCompetitorsTabs + '__EnvironmentCompetitorCompetitiveMessagingContent');
                subCMTab.hide();
            }
        }
    };

    var loadMiniHtmlEditor = function() {
        $('#<%=formId%>Description').cleditor();
        $(".cleditorMain iframe").contents().find('body').bind('keyup', function() {
            var v = $(this).text(); // or .html() if desired
            $('#<%=formId%>Description').html(v);            
        });
    };

    var ResizeHeightForm = function() {
        var div = document.getElementById('ValidationSummaryCompetitor');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#CompetitorEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };

    $(window).bind('resize', function() {
        $('#competitorIndexTwo').width($(window).width() - 9);
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

    var visibleLink = function() {
    var url = $('#<%=formId %>Website').prop('value');
        if (url != '') {
            $('#CompetitorShowLink').css("display", "block");
        }
    };
    var setParameterToUploadFile = function() {
    if ($('#<%= ViewData["Scope"] %>FileCheckIn').length) {
        initializeUploadField('<%= Url.Action("UploadImage", "File") %>', '<%= ViewData["Scope"] %>', 'FileDetail',
		   '#<%= ViewData["Scope"] %>FileCheckIn', '#<%= ViewData["Scope"] %>FileUploadResult', '#<%=formId%>ImageUrl',
			 '<%= ViewData["Container"] %>', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>', false);
    }
    };
</script>
<script type="text/javascript">
    $(function() {
        loadMiniHtmlEditor();
        hiddenCompetitorSubTab();
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        ResizeHeightForm();
        setParameterToUploadFile();
        visibleLink();
        resizeImageOfItem('ImageUrlCompetitor', 'ImageUrl');
    });
</script>
<div id="ValidationSummaryCompetitor">
<%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "CompetitorEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); loadMiniHtmlEditor();ResizeHeightForm(); setParameterToUploadFile();executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'Competitor', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");visibleLink();hiddenCompetitorSubTab();resizeImageToSuccess('ImageUrlCompetitor','ImageUrl');}",               
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div id="competitorIndexTwo" class="indexTwo">
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <% if (ViewData["UserSecurityAccess"].ToString().Equals(UserSecurityAccess.Edit))
           { %>
        <div class="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Competitor', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <% } %>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OriginalStatus")%>
        <%= Html.Hidden("OldName") %>
        <%= Html.Hidden("Id", default(decimal))%>
        <input type="hidden" id="showTabsHidden" value="<%= ViewData["ShowSubTab"]%>"/>
        <div id="CompetitorEditFormInternalContent"  class="contentFormEdit">        
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="CompetitorName" runat="server" Text="<%$ Resources:LabelResource, CompetitorName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name" })%>
                    <input type="hidden" name="dupName" value="<%=(Model==null? "":Model.Name)%>" />
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="CompetitorAssignedTo" runat="server" Text="<%$ Resources:LabelResource, CompetitorAssignedTo %>" />:</label>
                    <%=Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%=Html.ValidationMessage("AssignedTo", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Status">
                        <asp:Literal ID="CompetitorStatus" runat="server" Text="<%$ Resources:LabelResource, CompetitorStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id = formId + "Status" })%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
            </div>
            <div class="line">            
                <div class="field">
                    <label for="<%= formId %>Phone">
                        <asp:Literal ID="CompetitorPhone" runat="server" Text="<%$ Resources:LabelResource, CompetitorPhone %>" />:</label>
                    <%= Html.TextBox("Phone", null, new { id = formId + "Phone" })%>
                    <%= Html.ValidationMessage("Phone", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Fax">
                        <asp:Literal ID="CompetitorFax" runat="server" Text="<%$ Resources:LabelResource, CompetitorFax %>" />:</label>
                    <%= Html.TextBox("Fax", null, new { id = formId + "Fax" })%>
                    <%= Html.ValidationMessage("Fax", "*")%>
                </div>
            </div>
            <div class="line">
                 <div class="field">
                    <label for="<%= formId %>Tier">
                        <asp:Literal ID="CompetitorTier" runat="server" Text="<%$ Resources:LabelResource, CompetitorTier %>" />:</label>
                    <%= Html.DropDownList("Tier", (SelectList)ViewData["TierList"], string.Empty, new { id=formId + "Tier"})%>
                    <%= Html.ValidationMessage("Tier", "*") %>
                </div>
                <div class="field">
                    <label for="<%= formId %>MarketTypeId"><asp:Literal ID="CompetitorMarketTiypeId" runat="server" Text="<%$ Resources:LabelResource, CompetitorMarketTypeId %>" />:</label>
                    <%= Html.DropDownList("MarketTypeId", (SelectList)ViewData["MarketTypeList"], string.Empty, new { id=formId+"MarketTypeId"})%>
                    <%= Html.ValidationMessage("MarketTypeId","*") %>
                </div>
                <div class="field">
                    <label for="<%= formId %>Website">
                        <asp:Literal ID="CompetitorWebsite" runat="server" Text="<%$ Resources:LabelResource, CompetitorWebsite %>" />:</label>
                    <%= Html.TextBox("Website", null, new { id = formId + "Website" })%>
                    <%= Html.ValidationMessage("Website", "*")%>
                    <div id="CompetitorShowLink" style="display: none">
                        <a href="javascript:void(0);" onclick="loadUrl();">Go To URL</a>
                    </div>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>ImageUrl">
                        <asp:Literal ID="CompetitorImageUrl" runat="server" Text="<%$ Resources:LabelResource, ProductImageUrl %>" />:</label>
                    <%= Html.TextBox("ImageUrl",null, new { id = formId + "ImageUrl" })%>
                    <%= Html.ValidationMessage("ImageUrl", "*")%>
                    
                    <div id="CompetitorImageUrl" class="imageContentHorizontal">
                        <% var StaticImageUrl = Model != null ? Html.Encode(Model.ImageUrl): string.Empty;
                         %>
                        <div class="imageContentVertical">
                            <img id="ImageUrl"  width="75" height="75" />
                            <img id="ImageUrlCompetitor" src="<%=StaticImageUrl %>" class="imageInContent"/>
                        </div>
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
                        <asp:Literal ID="CompetitorDescription" runat="server" Text="<%$ Resources:LabelResource, CompetitorDescription %>" />:</label>
                    <%= Html.TextArea("Description", new { id = formId + "Description" })%>
                    <%= Html.ValidationMessage("Description", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>MetaData">
                        <asp:Literal ID="CompetitorMetaData" runat="server" Text="<%$ Resources:LabelResource, CompetitorMetaData %>" />:</label>
                    <%= Html.TextArea("MetaData", new { id = formId + "MetaData" })%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>KeyWord">
                        <asp:Literal ID="CompetitorKeyWord" runat="server" Text="<%$ Resources:LabelResource, CompetitorKeyWord %>" />:</label>
                    <%= Html.TextBox("KeyWord", null, new { id = formId + "KeyWord" })%>
                    <%= Html.ValidationMessage("KeyWord", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>TickerSymbol">
                        <asp:Literal ID="CompetitorTickerSymbol" runat="server" Text="<%$ Resources:LabelResource, CompetitorTickerSymbol %>" />:</label>
                    <%= Html.TextBox("TickerSymbol", null, new { id = formId + "TickerSymbol" })%>
                    <%= Html.ValidationMessage("TickerSymbol", "*")%>
                </div>
            </div>          
           <div class="line">
               <% IList<CustomField> customfields = (IList<CustomField>)ViewData["CustomFieldList"];%>
               <% foreach(CustomField customfield in customfields) {%>
                  <div class="field">
                   <label> <%=customfield.Name %>:</label>
                   <%if (Model == null || (Model != null && Model.CustomFields.Count==0) )
                     { %>
                     <input name="<%=customfield.PhysicalName%>" type="text" />
                   <%}
                     else
                     { %>
                   <input name="<%=customfield.PhysicalName%>" type="text" value="<%=Model.CustomFields[customfield.PhysicalName] %>"/>
                   <%} %>
                   </div>
               <% } %>
               
            </div>
            <div class="line">&nbsp;</div>            
        </div>
    </fieldset>
</div>
<% } %>