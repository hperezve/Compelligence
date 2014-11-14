<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Product>" %>
<% string formId = ViewData["Scope"].ToString() + "ProductEditForm"; %>

<link href="<%= Url.Content("~/Content/Styles/jquery.autocomplete.css") %>" rel="stylesheet" type="text/css" />
<link href="<%= Url.Content("~/Content/Styles/thickbox.css") %>" rel="stylesheet" type="text/css" />
    

<script src="<%= Url.Content("~/Scripts/jquery.autocomplete.min.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jquery-ui/jquery.numeric.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>

  <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
  
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
    function convertText() {
        var textboxes = document.getElementsByTagName("input");
        for (var i = 0; i < textboxes.length; i++) {
            if ((textboxes[i].type == "text")) {
                if (textboxes[i].value != "") {
                    textboxes[i].value = convertTextPlainHtml(textboxes[i].value);
                }
            }
        }
    }
    var ResizeHeightForm = function() {
        var div = document.getElementById('ValidationSummaryProduct');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#ProductEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };
    
    $(window).bind('resize', function() {
        $('#productIndexTwo').width($(window).width() - 9);
    });
</script>
  
	<script type="text/javascript">

	    $("#<%= formId %>ListPrice").numeric(",");

	</script>	
	
<script type="text/javascript">
    var hiddenSubTab = function() {
    //When Create a new Product and then save, the ViewData["showSubTab"] does not update the data
    //that's why I keep it in hidden
        var showSubTab = $("#showTabsHidden").val();      
        if (showSubTab == 'false') {
            if (ProductSubtabs != null && ProductSubtabs != undefined) {
                var idOfProductsTabs = ProductSubtabs.id;
                var subTab = $('#' + idOfProductsTabs + '__EnvironmentProductCompetitiveMessagingContent');
                subTab.hide();
            }
        }
    };
    
    var setParameterToUploadFile = function(){
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

    var loadUrl = function() {
        var url = $('#<%=formId %>Url').prop('value');
        if (url != '') {
            if (url.indexOf("http://") == -1) {
                url = "http://" + url;
            }
            window.open(url, "Url", "scrollbars=1,width=900,height=500")
        }
    };
    
    
</script>

<script type="text/javascript">
    var visibleLink = function() {
        var url = $('#<%=formId %>Url').prop('value');
        if (url != '') {
            $('#ProductShowLink').css("display", "block");
        }
    };
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>', ['dt:#<%= formId %>DateOfLaunchFrm', 'dt:#<%= formId %>EndOfLifeFrm']);
        visibleLink();
        ResizeHeightForm();
        hiddenSubTab();
        setParameterToUploadFile();
        resizeImageOfItem('ImageUrlProduct','ImageUrl');
    });
</script>
<script type="text/javascript">
    var ProductFamilyAutoComplete = function() {
    var competitorid = $('#<%=formId%>CompetitorId').val();
    if (competitorid.length > 0) {
        //Fill product family by competitor
        $.ajax({
            type: "POST",
            url: '<%= Url.Action("GetProductsFamily", "ProductFamily")%>/' + $('#<%=formId%>CompetitorId').val(),
            dataType: "json",
            success: function(data) {
                $("#ProductFamilyName").autocomplete(data, {
                    matchContains: true,
                    minChars: 0,
                    max: 200,
                    formatItem: function(row, i, max) {
                        return row.Text;
                    }
                });
            }
        });
        $('#ProductFamilyName').result(findValueProductFamilyCallback);
    }
    };
    $(function() {
        ProductFamilyAutoComplete();
    });

    function findValueProductFamilyCallback(event, data, formatted) {
        $('#ProductFamilyId').val(data.Value);
    }
</script>

<div id="ValidationSummaryProduct">
<%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "ProductEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "');loadMiniHtmlEditor(); ResizeHeightForm();setParameterToUploadFile();executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'Product', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");visibleLink();ProductFamilyAutoComplete();hiddenSubTab();resizeImageToSuccess('ImageUrlProduct','ImageUrl');}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div id="productIndexTwo" class="indexTwo">
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
        
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save, new { onClick = "javascript: convertText();" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Product', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OldName")%>
        <%= Html.Hidden("OldDateOfLaunch")%>
        <%= Html.Hidden("OldStatus")%>
        <%= Html.Hidden("OldCompetitorIdStr")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <input type="hidden" id="showTabsHidden" value="<%= ViewData["ShowSubTab"]%>"/>
        <div id="ProductEditFormInternalContent"  class="contentFormEditProduct">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="ProductName" runat="server" Text="<%$ Resources:LabelResource, ProductName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name" })%>
                    <input type="hidden" name="dupName" value="<%=(Model==null? "":Model.Name)%>" />
                    <%= Html.ValidationMessage("Name", "*") %>
                </div>
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="ProductAssignedTo" runat="server" Text="<%$ Resources:LabelResource, ProductAssignedTo %>" />:</label>
                    <%=Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%=Html.ValidationMessage("AssignedTo", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>CompetitorId" class="required">
                        <asp:Literal ID="ProductCompetitorId" runat="server" Text="<%$ Resources:LabelResource, ProductCompetitorId %>" />:</label>
                    <% if (ViewData["HeaderType"] == null || !ViewData["HeaderType"].ToString().Equals(DomainObjectType.Competitor))
                       {%>
                    <%= Html.DropDownList("CompetitorId", (SelectList)ViewData["CompetitorIdList"], string.Empty, new { id = formId + "CompetitorId" })%>
                    <% }
                       else
                       {%>
                    <%= Html.DropDownList("CompetitorId", (SelectList)ViewData["CompetitorIdList"], string.Empty,  new { id = formId + "CompetitorId", @disabled = "disabled" })%>
                    <% }%>
                    <%= Html.ValidationMessage("CompetitorId", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>DateOfLaunchFrm">
                        <asp:Literal ID="DateOfLaunch" runat="server" Text="<%$ Resources:LabelResource, ProductDateOfLaunch %>" />:</label>
                    <%= Html.TextBox("DateOfLaunchFrm", null, new { id = formId + "DateOfLaunchFrm" })%>
                    <%= Html.ValidationMessage("DateOfLaunchFrm", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>EndOfLifeFrm">
                        <asp:Literal ID="EndOfLife" runat="server" Text="<%$ Resources:LabelResource, ProductEndOfLife %>" />:</label>
                    <%= Html.TextBox("EndOfLifeFrm", null, new { id = formId + "EndOfLifeFrm" })%>
                    <%= Html.ValidationMessage("EndOfLifeFrm", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Status">
                        <asp:Literal ID="ProductStatus" runat="server" Text="<%$ Resources:LabelResource, ProductStatus %>" />:</label>
                        
                      <%--  if the product has not industries should show 2 states, 
                            but if there is no industry and before the state was enabled, it must retain its state ...  --%>
                            
                     <% if (ViewData["HeaderType"] == null || !ViewData["HeaderType"].ToString().Equals(DomainObjectType.Industry))
                        {%>
                    <%   if (Model != null && Model.Status == "ENBL")
                         {%>
                         
                        <%= Html.DropDownList("Status", (SelectList)ViewData["StatusListByIndustry"], string.Empty, new { id = formId + "Status" })%> 
                          <% }
                         else
                         { %>    
                 <%=  Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id = formId + "Status" })%>
                     
                    <% }
                        }
                        else
                        { %>
                        <%= Html.DropDownList("Status", (SelectList)ViewData["StatusListByIndustry"], string.Empty, new { id = formId + "Status" })%>
                    <% } %>
                    
                    
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
                <div class="field">
                   <label for="<%= formId %>TransitionProduct">
                        <asp:Literal ID="ProductProductFamilyName" runat="server" Text="Product Family" />:</label>               
                    <%= Html.TextBox("ProductFamilyName")%>
                    <%= Html.ValidationMessage("ProductFamilyName", "*")%>
                    <%= Html.Hidden("ProductFamilyId")%>
               </div>
               <div class="field">
                    <label for="<%= formId %>Tier"><asp:Literal ID="ProductTier" runat="server" Text="<%$ Resources:LabelResource, ProductTier %>" />:</label>
                    <%= Html.DropDownList("Tier",(SelectList)ViewData["TierList"], string.Empty, new {id = formId + "Tier"}) %>
                    <%= Html.ValidationMessage("Tier", "*")%>
               </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>PriceUnits">
                        <asp:Literal ID="ProductPriceUnits" runat="server" Text="<%$ Resources:LabelResource, ProductPriceUnits %>" />:</label>
                    <%= Html.TextBox("PriceUnits", null, new { id = formId + "PriceUnits", @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("PriceUnits", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>ListPrice">
                        <asp:Literal ID="ListPrice" runat="server" Text="<%$ Resources:LabelResource, ProductListPrice %>" />:</label>
                    <%= Html.TextBox("ListPrice", null, new { id = formId + "ListPrice", @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("ListPrice", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Currency">
                        <asp:Literal ID="Currency" runat="server" Text="<%$ Resources:LabelResource, ProductCurrency %>" />:</label>
                    <%= Html.DropDownList("Currency", (SelectList)ViewData["CurrencyList"], string.Empty, new { id = formId + "Currency" })%>
                    <%= Html.ValidationMessage("Currency", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                   <label for="<%= formId %>TransitionProduct">
                        <asp:Literal ID="TransitionProduct" runat="server" Text="<%$ Resources:LabelResource, ProductTransitionProduct %>" />:</label>               
                  <select size="4" style="height:100px">
                      <%IList<Product> TransitionProducts = (IList<Product>)ViewData["TransitionProduct"];
                        if (TransitionProducts == null) TransitionProducts = new List<Product>();
                        foreach (Product p in TransitionProducts)
                        { 
                         %>
                          <option><%=p.Name %></option>
                      <%} %>
                  </select>
                </div>
                <div class="field">
                    <label for="<%= formId %>Url">
                        <asp:Literal ID="ProductUrl" runat="server" Text="<%$ Resources:LabelResource, ProductUrl %>"></asp:Literal>:</label>
                    <%= Html.TextBox("Url", null, new {id=formId+"Url"}) %>
                    <%= Html.ValidationMessage("Url","*") %>
                    <div id="ProductShowLink" style="display: none">
                        <a href="javascript:void(0);" onclick="loadUrl();">Go To URL</a>
                    </div>
                </div>

                <div class="field">
                    <label for="<%= formId %>ImageUrl">
                        <asp:Literal ID="ProductImageUrl" runat="server" Text="<%$ Resources:LabelResource, ProductImageUrl %>" />:</label>
                    <%= Html.TextBox("ImageUrl",null, new { id = formId + "ImageUrl" })%>
                    <%= Html.ValidationMessage("ImageUrl", "*")%>
                    
                    <div id="ProductImageUrl" class="imageContentHorizontal">
                        <% var StaticImageUrl = Model != null ? Html.Encode(Model.ImageUrl): string.Empty;
                         %>
                        <div class="imageContentVertical">
                               <img id="ImageUrl"  width="75" height="75" />
                               <img id="ImageUrlProduct" src="<%=StaticImageUrl %>" class="imageInContent"/>
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
                        <asp:Literal ID="ProductDescription" runat="server" Text="<%$ Resources:LabelResource, ProductDescription %>" />:</label>

                    <%= Html.TextArea("Description", new { id = formId + "Description" })%>
                   <%= Html.ValidationMessage("Description", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>MetaData">
                        <asp:Literal ID="ProductMetaData" runat="server" Text="<%$ Resources:LabelResource, ProductMetaData %>" />:</label>
                    <%= Html.TextArea("MetaData", new { id = formId + "MetaData" })%>
                </div> 
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>KeyWord">
                        <asp:Literal ID="ProductKeyWord" runat="server" Text="<%$ Resources:LabelResource, ProductKeyWord %>" />:</label>
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
            <div class="line">&nbsp;</div>
        </div>
    </fieldset>
</div>
<% } %>