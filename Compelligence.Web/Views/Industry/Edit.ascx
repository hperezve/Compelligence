<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Industry>" %>
<% string formId = ViewData["Scope"].ToString() + "IndustryEditForm"; %>

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

        var showIndustryParentByHierarchy = function(invoke) {
            var idParent = '<%= formId%>' + "Parent";
            var realvaluesIndustry = [];
            var textvaluesIndustry = [];
            var xmlhttp;
            $('#' + idParent + ' :selected').each(function(i, selected) {
                realvaluesIndustry[i] = $(selected).val();
                textvaluesIndustry[i] = $(selected).text();
            });
            var parameters = { IsChecked: invoke.checked, idParent: realvaluesIndustry };
            $.get(
            '<%= Url.Action("CheckSomething", "Industry") %>',
            parameters,
            function(data) {
                if (data != null && data != '') {
                    results = data;                    
                    if (results != "") {
                        addNewIndustriesToList(results);
                    }
                }
            });
        };

        function addNewIndustriesToList(results) {
            var idParent = '<%= formId%>' + "Parent";
            var i = $(this);
            i.queue(function() {
                setTimeout(function() {
                    i.dequeue();
                }, 1000);
            });

            var arrayIndustries = [];
            arrayIndustries = results.split("_");
            var options = $('#' + idParent).prop('options');            
            $('#' + idParent)[0].options.length = 0;
            for (j = 0; j < arrayIndustries.length; j++) {
                var arrayCompet = arrayIndustries[j].split(":");
                if (arrayCompet[2] == 'True') {
                    options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, true);
                }
                else {
                    options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, false);
                }
            }

        };
        
        $(function() {
            loadMiniHtmlEditor();

        });
        
      
    </script>
    <script type="text/javascript">
    var ResizeHeightForm = function() {
    var div = document.getElementById('ValidationSummaryIndustry');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#IndustryEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };
    var setParameterToUploadFile = function() {
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
    };
    $(window).bind('resize', function() {

        $('#industryIndexTwo').width($(window).width() - 9);

    });
</script>
<script type="text/javascript">
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        ResizeHeightForm();
        setParameterToUploadFile();
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
        var webSiteUrlExp = /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i;
        if (url != '') {
            if (webSiteUrlExp.test(url)) {
              $('#CompetitorShowLink').css("display", "block");
            } 
            
        }
    };
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        //ResizeHeightForm('#<%= ViewData["Scope"] %>IndustryList');
        ResizeHeightForm();
        visibleLink();
        resizeImageOfItem('ImageUrlIndustry', 'ImageUrl');
    });
</script>

<div id="ValidationSummaryIndustry">
    <%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "IndustryEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); loadMiniHtmlEditor();ResizeHeightForm();setParameterToUploadFile(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','Industry', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");visibleLink();resizeImageToSuccess('ImageUrlIndustry','ImageUrl');}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div id="industryIndexTwo" class="indexTwo">
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Industry', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
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
        <div id="IndustryEditFormInternalContent" class="contentFormEdit">
            <div class="line">               
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="IndustryName" runat="server" Text="<%$ Resources:LabelResource, IndustryName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name" })%>
                    <input type="hidden" name="dupName" value="<%=(Model==null? "":Model.Name)%>" />
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="IndustryAssignedTo" runat="server" Text="<%$ Resources:LabelResource, IndustryOwnerId %>" />:</label>
                    <%=Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%=Html.ValidationMessage("AssignedTo", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Status">
                        <asp:Literal ID="IndustryStatus" runat="server" Text="<%$ Resources:LabelResource, IndustryStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id = formId + "Status" })%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Parent" style="float: left;">
                        <asp:Literal ID="IndustryParent" runat="server" Text="<%$ Resources:LabelResource, IndustryParent %>" />:</label>
                    <input style="float: left;margin-left: 5px;margin-right: 5px;margin-top: 3px;" type="checkbox" onclick="showIndustryParentByHierarchy(this);" /><label style="float: left;" for="CheckIndustryIds">By Hierarchy</label>
                    <%= Html.DropDownList("Parent", (SelectList)ViewData["IndustryParentList"], string.Empty, new { id = formId + "Parent" })%>
                    <%= Html.ValidationMessage("Parent", "*")%>
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
                        <asp:Literal ID="IndustryImageUrl" runat="server" Text="<%$ Resources:LabelResource, ProductImageUrl %>" />:</label>
                    <%= Html.TextBox("ImageUrl",null, new { id = formId + "ImageUrl" })%>
                    <%= Html.ValidationMessage("ImageUrl", "*")%>
                    
                    <div id="IndustryImageUrl" class="imageContentHorizontal">
                        <% var StaticImageUrl = Model != null ? Html.Encode(Model.ImageUrl): string.Empty;
                         %>
                         <div class="imageContentVertical">
                               <img id="ImageUrl"  width="75" height="75" />
                               <img id="ImageUrlIndustry" src="<%=StaticImageUrl %>" class="imageInContent"/>
                         </div>
                    </div>
                    <div style="float: left; margin-top: 10px; margin-left: 10px">
                        <input type="button" value="Show" onclick="loadImageUrl('<%=StaticImageUrl %>');return false;" />
                        <input class="button" id="<%= ViewData["Scope"] %>FileCheckIn" type="button" value="Upload" />
                    </div>
                </div> 
                <%--<div class="field">
                    <label for="<%= formId %>BudgetTime">
                        <asp:Literal ID="IndustryBudgetTime" runat="server" Text="<%$ Resources:LabelResource, IndustryBudgetTime %>" />:</label>
                    <%= Html.TextBox("BudgetTimeFrm", null, new { id = formId + "BudgetTime", @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("BudgetTimeFrm", "*")%>
                </div>--%>
                <div class="field">
                    <label for="<%= formId %>Tier"><asp:Literal ID="IndustryTier" runat="server" Text="<%$ Resources:LabelResource, IndustryTier %>" />:</label>
                    <%= Html.DropDownList("Tier",(SelectList)ViewData["TierList"], string.Empty, new {id = formId + "Tier"}) %>
                    <%= Html.ValidationMessage("Tier", "*")%>
               </div>
            </div>
            <div class="line">
               <div class="field">
                    <label for="<%= formId %>Description">
                        <asp:Literal ID="IndustryDescription" runat="server" Text="<%$ Resources:LabelResource, IndustryDescription %>" />:</label>
                    <%= Html.TextArea("Description", new { id = formId + "Description" })%>
                    <%= Html.ValidationMessage("Description", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>MetaData">
                        <asp:Literal ID="IndustryMetaData" runat="server" Text="<%$ Resources:LabelResource, IndustryMetaData %>" />:</label>
                    <%= Html.TextArea("MetaData", new { id = formId + "MetaData" })%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>KeyWord">
                        <asp:Literal ID="IndustryKeyWord" runat="server" Text="<%$ Resources:LabelResource, IndustryKeyWord %>" />:</label>
                    <%= Html.TextBox("KeyWord", null, new { id = formId + "KeyWord" })%>
                    <%= Html.ValidationMessage("KeyWord", "*")%>
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
