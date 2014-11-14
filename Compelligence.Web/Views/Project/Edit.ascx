<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Project>" %>
<% string formId = ViewData["Scope"].ToString() + "ProjectEditForm"; %>

<link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />

    <script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/Retrieve.js") %>"></script>
    <script>
        var ubfhierarchy = '<%= Url.Action("ChangeIndustryList", "Project") %>';
    </script>
    <!--Need set parameter ubfHierarchy-->
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/BackEnd/hierarchy.js")%>"></script>

    
    <script type="text/javascript">
        var loadMiniHtmlEditor = function() {
            $('#<%=formId%>Description').cleditor({ Id: 'HtmlEditorDescription' });
            $("#HtmlEditorDescription iframe").contents().find('body').bind('keyup', function() {
                var v = $(this).text(); // or .html() if desired
                $('#<%=formId%>Description').html(v);
            });

            $('#<%=formId%>TextToDisplay').cleditor({ Id: 'HtmlEditorTextToDisplay' });
            $("#HtmlEditorTextToDisplay iframe").contents().find('body').bind('keyup', function() {
                var v = $(this).text(); // or .html() if desired
                $('#<%=formId%>TextToDisplay').html(v);
            });
        };

        $(function() {
            loadMiniHtmlEditor();

        });
        
      
    </script>
<script type="text/javascript">
    
    var getHeight = function() {
        var div = document.getElementById('ValidationSummaryProject');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var edt = $('#ProjectEditFormInternalContent');
                var tempo = 328 - 58 - (10 * lis.length);
                var edt = $('#ProjectEditFormInternalContent').css('height', tempo + 'px');
            }
        }
    };


    var validatelist = function() {

    var count1 = ('<%= ViewData["IndustryIdMultiListCount"]%>') * 16 + 10;
    var count2 = ($('#CompetitorIds')[0].options.length) * 16 + 10;
    var count3 = ($('#ProductIds')[0].options.length) * 16 + 10;

    $('.contentscrollableI select').css('height', count1 + "px");
    $('.contentscrollableC select').css('height', count2 + "px");
    $('.contentscrollableP select').css('height', count3 + "px");

    fixOptionTitle("#IndustryIds");
    fixOptionTitle("#CompetitorIds");
    fixOptionTitle("#ProductIds");

    } 
    
    

        var updateCheckByHierarchy = function() {
        var checked = $('#checkedbyHierarchy').val();
            if (checked != '') {
                if (checked == 'true') {
                    $('#CheckIndustryIds').prop('checked', true);
                }
                else if (checked == 'false') {
                    $('#CheckIndustryIds').prop('checked', false);
                }
            }
        }


    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>', ['dt:#<%= formId %>DueDateFrm']);
        getHeight();
        updateCheckByHierarchy();
    });

    $(window).bind('resize', function() {
        $('#projectIndexTwo').width($(window).width() - 9);
    });
</script>


<script type="text/javascript">
    

    function confirmDueDate() {
        if ($('#WorkspaceProjectEditFormDueDateFrm').val() != "") {
            var dateNow = new Date();
            var dayNow = dateNow.getDate();
            var MonthNow = dateNow.getMonth() + 1;
            var yearNow = dateNow.getFullYear();
            var textDateNow = MonthNow + '/' + dayNow + '/' + yearNow;

            var currentDate = new Date(textDateNow);
            var dueDateFrm = new Date($('#WorkspaceProjectEditFormDueDateFrm').val());

            if (dueDateFrm < currentDate) {

                $("#ChooseConfirmDueDate").dialog("open");
            }
        }
    }


</script>
<script type="text/javascript">
    var ubfcompetitors = '<%= Url.Action("GetMasiveCompetitors", "Project")%>'; //set for portability
    var ubfproducts = '<%= Url.Action("GetMasiveProducts", "Project")%>'; //set for portability
</script>
<script type="text/javascript" src="<%= Url.Content("~/Scripts/System/BackEnd/updateicp.js")%>"></script>

<script type="text/javascript">
    //
    //
    function resetMultiSelect() {
        $('#CompetitorIds')[0].options.length = 0;
        $('#ProductIds')[0].options.length = 0;

        var competitorids = $('#OldCompetitorsIds').val().split(",");
        var productids = $('#OldProductsIds').val().split(",");
        var industryids = getOptionValues('#IndustryIds', true); //???????

        updCompetitorMasiveList('#', industryids, competitorids, productids);

        //remember get only products of competitors selected
        setTimeout(function() {
            //console.log("Ps:" + productids); //only for debug
            updProductMasiveList('#', competitorids, productids, industryids)
        }, 1000);
    }

    $(function() {
    //<%= ViewData["IndustryIdMultiListCount"]%>
    updateUpHeight('#IndustryIds');
    updateUpHeight('#CompetitorIds');
    updateUpHeight('#ProductIds');

    fixOptionTitle("#IndustryIds");
    fixOptionTitle("#CompetitorIds");
    fixOptionTitle("#ProductIds");
      
}); 
    
    
    

</script>

<div id="ValidationSummaryProject">
<%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "ProjectEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "', ['dt:#" + formId + "DueDateFrm']);loadMiniHtmlEditor();getHeight(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'Project', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");validatelist(); updateCheckByHierarchy();}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div id="projectIndexTwo" class="indexTwo">
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');resetMultiSelect();" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Project', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
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
        <%= Html.Hidden("OldIndustryId")%>
        <%= Html.Hidden("OldCompetitorId")%>
        <%= Html.Hidden("OldProductId")%>
        <%= Html.Hidden("OldIndustriesIds")%>
        <%= Html.Hidden("OldCompetitorsIds")%>
        <%= Html.Hidden("OldProductsIds")%>
        <%= Html.Hidden("OldAssignedTo")%>
        <%= Html.Hidden("checkedbyHierarchy")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="ProjectEditFormInternalContent" class="contentFormEdit resizeMe">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="ProjectName" runat="server" Text="<%$ Resources:LabelResource, ProjectName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name" })%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="ProjectAssignedTo" runat="server" Text="<%$ Resources:LabelResource, ProjectAssignedTo %>" />:</label>
                    <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%= Html.ValidationMessage("AssignedTo", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>CreatedByFrm">
                        <asp:Literal ID="ProjectCreatedByFrm" runat="server" Text="<%$ Resources:LabelResource, ProjectOpenedBy %>" />:</label>
                    <%= Html.TextBox("CreatedByFrm", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("CreatedByFrm", "*") %>
                </div>                
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>IndustryId" style="float:left">
                    <asp:Literal ID="ProjectIndustryId" runat="server" Text="<%$ Resources:LabelResource, ProjectIndustryId %>" />:</label>
                    <%SelectList l = (SelectList)ViewData["IndustryIdList"]; %>
                    <%= Html.CheckBox("CheckIndustryIds", (bool)ViewData["CheckIndustryIds"], new { onclick = "ShowIndustriesByHierarchy('#',this);", Style = "float:left;margin-left:5px;height:25px;" })%><label for="CheckIndustryIds" >&nbsp;By Hierarchy</label>
                    <div class="contentscrollableI">
                    <%= @Html.ListBox("IndustryIds", (MultiSelectList)ViewData["IndustryIdMultiList"], new { Multiple = "true", onchange = "javascript: updCompetitors('#');", @class = "fileListForm" })%>
                    </div>
                    <%= Html.ValidationMessage("IndustryId", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>CompetitorId">
                    <asp:Literal ID="ProjectCompetitorId" runat="server" Text="<%$ Resources:LabelResource, ProjectCompetitorId %>" />:</label>
                    <div class='contentscrollableC'> 
                    <%= @Html.ListBox("CompetitorIds", (MultiSelectList)ViewData["CompetitorIdList"], new { Multiple = "true", onchange = "javascript: updProducts('#');", @class = "fileListForm" })%>
                    <%= Html.ValidationMessage("CompetitorId", "*")%>
                    </div>
                </div>
                <div class="field">
                    <label for="<%= formId %>ProductId">
                    <asp:Literal ID="ProductId" runat="server" Text="<%$ Resources:LabelResource, ProjectProductId %>" />:</label>
                    <div class='contentscrollableP'> 
                    <%= @Html.ListBox("ProductIds", (MultiSelectList)ViewData["ProductIdList"], new { Multiple = "true", @class = "fileListForm" })%>
                    <asp:ListBox id="ListBox2" ></asp:ListBox>
                    <%= Html.ValidationMessage("ProductId", "*")%>
                    </div>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Status">
                        <asp:Literal ID="ProjectStatus" runat="server" Text="<%$ Resources:LabelResource, ProjectStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id = formId + "Status" })%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>DueDateFrm">
                        <asp:Literal ID="ProjectDueDate" runat="server" Text="<%$ Resources:LabelResource, ProjectDueDate %>" />:</label>
                    <%= Html.TextBox("DueDateFrm", null, new { id = formId + "DueDateFrm" , onchange = "javascript: confirmDueDate();"})%>
                    <%= Html.ValidationMessage("DueDateFrm", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Type">
                        <asp:Literal ID="ProjectType" runat="server" Text="<%$ Resources:LabelResource, ProjectType %>" />:</label>
                    <%= Html.DropDownList("ContentTypeId", (SelectList)ViewData["ContentTypeList"], string.Empty, new { id = formId + "Type"})%>
                    <%= Html.ValidationMessage("ContentTypeId", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>CreatedDateFrm">
                        <asp:Literal ID="DealCreatedDateFrm" runat="server" Text="<%$ Resources:LabelResource, ProjectCreatedDate %>" />:</label>
                    <%= Html.TextBox("CreatedDateFrm", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("CreatedDateFrm", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>FinancialBudgetUnit">
                    <asp:Literal ID="ProjectFinancialBudgetUnit" runat="server" Text="<%$ Resources:LabelResource, ProjectFinancialBudgetUnit %>" /></label>
                    <%= Html.TextBox("FinancialBudgetUnit", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("FinancialBudgetUnit", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>TotalFinancialBudget">
                    <asp:Literal ID="ProjectTotalFinancialBudget" runat="server" Text="<%$ Resources:LabelResource, ProjectTotalFinancialBudget %>" /></label>
                    <%= Html.TextBox("TotalFinancialBudget", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("TotalFinancialBudget", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>TimeBudgetUnit">
                    <asp:Literal ID="ProjectTimeBudgetUnit" runat="server" Text="<%$ Resources:LabelResource, ProjectTimeBudgetUnit %>" /></label>
                    <%= Html.TextBox("TimeBudgetUnit", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("TimeBudgetUnit", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>TotalTimeBudget">
                    <asp:Literal ID="ProjectTotalTimeBudget" runat="server" Text="<%$ Resources:LabelResource, ProjectTotalTimeBudget %>" /></label>
                    <%= Html.TextBox("TotalTimeBudget", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("TotalTimeBudget", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>MarketTypeId"><asp:Literal ID="ProjectMarketTiypeId" runat="server" Text="<%$ Resources:LabelResource, ProjectMarketTypeId %>" />:</label>
                    <%= Html.DropDownList("MarketTypeId", (SelectList)ViewData["MarketTypeList"], string.Empty, new { id=formId+"MarketTypeId"})%>
                    <%= Html.ValidationMessage("MarketTypeId","*") %>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Visibility">
                        <asp:Literal ID="ProjectVisibility" runat="server" Text="<%$ Resources:LabelResource, ProjectVisibility %>" />:</label>
                    <%= Html.DropDownList("Visibility", (SelectList)ViewData["VisibilityList"], new { id = formId + "Visibility" })%>
                    <%= Html.ValidationMessage("Visibility", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>TextToDisplay"><asp:Literal ID="ProjectTextToDisplay" runat="server" Text="Text to Display"></asp:Literal>:</label>
                    <%= Html.TextArea("TextToDisplay", new { id = formId + "TextToDisplay" })%>
                    <%= Html.ValidationMessage("TextToDisplay", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Description"><asp:Literal ID="ProjectDescription" runat="server" Text="<%$ Resources:LabelResource, ProjectDescription %>"></asp:Literal>:</label>
                    <%= Html.TextArea("Description", new  { id = formId+"Description"})%>
                    <%= Html.ValidationMessage("Description","*") %>
                </div>

            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>MetaData">
                        <asp:Literal ID="ProjectMetaData" runat="server" Text="<%$ Resources:LabelResource, ProjectMetaData %>" />:</label>
                    <%= Html.TextArea("MetaData", new { id = formId + "MetaData" })%>
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
       </div>
    </fieldset>
</div>
<% } %>

<script type="text/javascript">
    function openSubWindow(url, scope) {
        browsePopup = window.open(url + '?scope=Environment&ActionMethod=Create' + '&Container=IndustryContent&BrowseId=IndustryAll&IsDetail=false', "BrowsePopup", "resizable=NO, location=NO, width=935,height=700, scrollbars=YES");
        if (window.focus) {
            browsePopup.focus();
        }
        return false;
    }
</script>