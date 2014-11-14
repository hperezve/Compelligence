<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.ProductCriteria>" %>
<% string formId = ViewData["Scope"].ToString() + "ProductCriteriaEditForm"; %>
<script src="<%= Url.Content("~/Scripts/jquery.autocomplete.min.js") %>" type="text/javascript"></script>

<style type='text/css'>

.fieldTextArea 
{
    float: left;
    margin-left: 10px;
    padding: 7px 25px 5px 0;
    width: 360px;
}
</style>


<script type="text/javascript">
    function isNumbersAndPointKey(event, element, _float) {
        event = event || window.event;
        var charCode = event.which || event.keyCode;
        if (charCode == 8 || charCode == 13 || (_float ? (element.value.indexOf('.') == -1 ? charCode == 46 : false) : false))
            return true;
        else if ((charCode < 48) || (charCode > 57))
            return false;
        return true;
    };
    function isNumbersAndPointKeyOrValue(event, element, _float) {
        event = event || window.event;
        var charCode = event.which || event.keyCode;
        var patronA = /[\^(\d|.)]/; //THIS PATRON IS TO NUMERIC AND POINT
        var patronB = /([nN])|([nN]+\/)/; //TO SEARCH /N/A
        var patronC = /[a-zA-Z\/]/; //TO ALL CHARACTER AND /
        var result = false;
        // TO FUTURE
        // shift + 5 = % = 37 , event.shiftKey = TRUE if key press shift
        // shift + 4 = $ = 36
        //             / = 47
        // left arrow    = 37
        // apostrophe ( ' )  = 39 , event.keyCode= 0 & event.which=39 [FireFox]
        // apostrophe ( ' )  = 39 ,  event.keyCode= 39 & event.which=undefined [IE]
        // right arrow  = 39, event.keyCode= 39 & event.which=0
        // backspace=8, delete = 46
        // left arrow, right arrow no working to IE and Safary    
        if (charCode == 8 || charCode == 13 || (charCode == 39 && (event.keyCode == 39 && event.which == 0) && !patronB.test(element.value)) || (!event.shiftKey && charCode == 37 && !patronB.test(element.value)) || (_float ? (element.value.indexOf('.') == -1 ? charCode == 46 : false) : false))
            result = true;
        else if ((((_float ? ((element.value.indexOf('A') == -1 && element.value.indexOf('a') == -1) ? charCode == 65 : false) : false) && (element.value != '' && (element.value == 'n/' || element.value == 'N/'))) ||
                  ((_float ? ((element.value.indexOf('A') == -1 && element.value.indexOf('a') == -1) ? charCode == 97 : false) : false) && (element.value != '' && (element.value == 'n/' || element.value == 'N/'))) ||
                  (_float ? ((element.value.indexOf('N') == -1 && element.value.indexOf('n') == -1) ? charCode == 110 : false) : false) ||
                  (_float ? ((element.value.indexOf('N') == -1 && element.value.indexOf('n') == -1) ? charCode == 78 : false) : false) ||
                  ((_float ? (element.value.indexOf('/') == -1 ? charCode == 47 : false) : false) && element.value != ''))
                  && !patronA.test(element.value) && (element.value == '' || patronB.test(element.value)))
            result = true;
        else if ((charCode < 48) || (charCode > 57))
            result = false;
        else
            result = !patronC.test(element.value);
        ShowMessage(result);
        return result;
    };
    function ShowMessage(value) {
        if (value) $('#divMessageAlert').hide();
        else $('#divMessageAlert').show();
    };
    function clearTextBox(element) {
        var type = '<%=ViewData["CriteriaType"] %>';
        if (element.value != 'N/A' && type == '<%= Compelligence.Domain.Entity.Resource.CriteriaType.Numeric %>') {
            $('#'+element.id).val('');
        }
    };
    var SetValuesByCriteria = function() {
        var type = '<%=ViewData["CriteriaType"] %>';
        var mostDesired = '<%=ViewData["CriteriaMostDesired"] %>';
        if (type != '') {
            if (type == '<%= Compelligence.Domain.Entity.Resource.CriteriaType.Boolean %>') {
                //Change TextBox to Select
                //$('#<%=formId %>Value').replaceWith('<select name="Value" id="<%=formId %>Value">');
                $('#DivTxtValue').hide();
                $('#<%=formId %>ValueSelectFrm').show();
                $('#<%=formId %>ValueSelectFrm').value = $('#<%=formId %>Value').val();
                //Disabled Ranking(feature)
                $('#<%=formId %>Feature').prop("disabled", "disabled");

            } else if (type == '<%= Compelligence.Domain.Entity.Resource.CriteriaType.Numeric %>') {
                //Validation to only Numeric
                $('#DivTxtValue').hide();
                $('#<%=formId %>ValueTextFrm').show();
                $('#<%=formId %>ValueTextFrm').value = $('#<%=formId %>Value').val();
                if (mostDesired == '<%= Compelligence.Domain.Entity.Resource.CriteriaMostDesiredNumeric.NA %>') {
                    var vFeature = $('#<%=formId %>Feature').val();
                    if (vFeature == '') {
                        $('#<%=formId %>Feature').val("MP");
                    }
                }
                else {
                    $('#<%=formId %>Feature').prop("disabled", "disabled");
                }

            } else if (type == '<%= Compelligence.Domain.Entity.Resource.CriteriaType.List %>') {
                var vFeature2 = $('#<%=formId %>Feature').val();
                if (vFeature2 == '') {
                    $('#<%=formId %>Feature').val("MP");
                }
            }
        }
    };

    var SetAutocompleteToValue = function() {
    var Industryid = '<%=ViewData["IndustryId"]%>';
    var Criteriaid = '<%=ViewData["CriteriaIdOld"]%>';

    $.ajax({
        type: "POST",
        url: '<%= Url.Action("GetValues_Industry", "ProductCriteria")%>',
        dataType: "json",
        data: { IndustryId: Industryid, CriteriaId: Criteriaid },
        success: function(data) {
            $("#Value").autocomplete(data, {
                matchContains: true,
                minChars: 0,
                max: 200,
                formatItem: function(row, i, max) {
                    return row.Text;
                }
            });
        }
    });
    };

    $(function() {

        SetAutocompleteToValue();

    });


    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        $('#EnvironmentProductProductCriteriaEditFormCriteriaGroupId').prop("disabled", true);
        $('#EnvironmentProductProductCriteriaEditFormCriteriaSetId').prop("disabled", true);
        $('#EnvironmentProductProductCriteriaEditFormCriteriaId').prop("disabled", true);
        SetValuesByCriteria();
    });

        var setIndustryStandard = function(id) {
            var textStandard = document.getElementById(id);
            var index = textStandard.selectedIndex;
            if (index == 0) {
                $('#EnvironmentProductProductCriteriaEditFormIndustryStandard').prop("value", "");
            } else {

                var idCriteria = textStandard.options[textStandard.selectedIndex].value;
                var xmlhttp;
                var parameters = { id: idCriteria };
                var entitypath = $('#EnvironmentProductProductCriteriaEditFormPath').val();
                var url = entitypath + '/ProductCriteria.aspx/GetIndustryStandard/' + idCriteria;
                $.get(
                url,
                null,
                function(data) {
                    if (data != null && data != '') {
                        results = data;
                        $('#EnvironmentProductProductCriteriaEditFormIndustryStandard').prop("value", result);
                    }
                });                
            }
        }
        function updateCriterias(prmGroupId,prmSetId) {
            var groupId = $('#' + prmGroupId).val();
            var setId = $('#' + prmSetId).val();
            var IndustryId = '<%=ViewData["IndustryId"] %>';

            //alert(IndustryId + "-" + groupId + "-" + setId);
            $.ajax({
                type: "POST",

                url: '<%= Url.Action("GetCriteriasByGroupSet", "ProductCriteria")%>',
                dataType: "json",
                data: { IndustryId: IndustryId, CriteriaGroupId: groupId,CriteriaSetId:setId },
                success: function(data) {
                    $('#<%= formId %>CriteriaId option').remove();
                    $.each(data,  function() {
                var dropdownList = $('#<%= formId %>CriteriaId');
                dropdownList.append($("<option></option>").prop("value", this.Value).text(this.Text)); 
                    }
                     );
                       }
            }); //end-$.ajax
        }
        function validateLengthData() {
            var typeName = $("#CriteriaType").val();
            if (typeName == "LIS") {
                var value = $("#Value").val();
                if (value.length > 225) {
                    var messageDialog = $('#AlertReturnMessageDialog');
                    var content = "The value... </br></br> " + value + " </br></br> ...is longer than 225 characters. </br></br> Only the first 225 of characters have been accepted."
                    messageDialog.dialog({width: 500});
                    messageDialog.html(content);
                    messageDialog.dialog("open");
                }
            }
        }
</script>
<div id="ValidationSummaryProductCriteria">
<%= Html.ValidationSummary()%>
</div>

<% using (Ajax.BeginForm((string)ViewData["ActionMethod"] + "ProductCriteria", null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "ProductCriteriaEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "');validateLengthData(); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "');SetValuesByCriteria();  SetAutocompleteToValue();executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'ProductCriteria', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div style="height: 316px; overflow: auto; width: 100%;">   
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'ProductCriteria', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("ProductId")%>
        <%= Html.Hidden("CriteriaIdOld")%> 
        <%= Html.Hidden("CriteriaType")%> 
        <%= Html.Hidden("CriteriaMostDesired")%>              
        <div >
       
        <div class="line">
            <div class="field">
                <label for="<%= formId %>CriteriaGroupId" >
                    <asp:Literal ID="ProductCriteriaCriteriaGroupId" runat="server" Text="<%$ Resources:LabelResource, ProductCriteriaCriteriaGroupId %>" />:</label>
                <%= Html.CascadingParentDropDownList("CriteriaGroupId", (SelectList)ViewData["CriteriaGroupList"], string.Empty, Url.Action("GetCriteriaSetsByCriteriaGroup", "ProductCriteria"), formId, "CriteriaSetId", new string[] { "CriteriaId" }, "updateCriterias('" + formId + "CriteriaGroupId','"+formId+"CriteriaSetId')")%>
                <%= Html.ValidationMessage("CriteriaGroupId", "*")%>
            </div>
            <div class="field">
                <label for="<%= formId %>CriteriaSetId" >
                    <asp:Literal ID="ProductCriteriaCriteriaSetId" runat="server" Text="<%$ Resources:LabelResource, ProductCriteriaCriteriaSetId %>" />:</label>
                <%= Html.CascadingParentDropDownList("CriteriaSetId", (SelectList)ViewData["CriteriaSetList"], string.Empty, true, Url.Action("GetCriteriasByCriteriaSet", "ProductCriteria"), formId, "CriteriaId", "updateCriterias('" + formId + "CriteriaGroupId','" + formId + "CriteriaSetId')")%>
                <%= Html.ValidationMessage("CriteriaSetId", "*")%>
            </div>  
            
               <div class="field">
                <label for="<%= formId %>CriteriaId" class="required">
                    <asp:Literal ID="ProductCriteriaCriteriaId" runat="server" Text="<%$ Resources:LabelResource, ProductCriteriaCriteriaId %>" />:</label>
                <%= Html.DropDownList("CriteriaId", (SelectList)ViewData["CriteriaList"], string.Empty, new { id = formId + "CriteriaId", onchange = "javascript: setIndustryStandard(id);" })%>
                <%--<%= Html.CascadingParentDropDownList("CriteriaId", (SelectList)ViewData["CriteriaList"], string.Empty, true, Url.Action("GetIndustryStandard", "ProductCriteria"), formId, "EnvironmentProductProductCriteriaEditFormIndustryStandard")%>--%>
                <%--<%= Html.CascadingChildDropDownList("CriteriaId", (SelectList)ViewData["CriteriaList"], string.Empty, formId)%>--%>
                <%= Html.ValidationMessage("CriteriaId", "*")%>
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
            <div class="field">
                <label for="<%= formId %>Feature" >
                    <asp:Literal ID="ProductCriteriaFeature" runat="server" Text="<%$ Resources:LabelResource, ProductCriteriaFeature %>" />:</label>
                <%= Html.DropDownList("Feature", (SelectList)ViewData["Features"], string.Empty, new { id = formId + "Feature" })%>
                <%= Html.ValidationMessage("Feature", "*")%>
            </div>
            <div class="field">
              <label for="<%= formId %>Value" class="required">
                    <asp:Literal ID="ProductCriteriaValue" runat="server" Text="<%$ Resources:LabelResource, ProductCriteriaValue %>" />:</label>
                <div id="DivTxtValue"><%= Html.TextBox("Value")%></div>
                <%= Html.DropDownList("ValueSelectFrm", (SelectList)ViewData["ProductCriteriaBooleanValue"], string.Empty,new { id = formId + "ValueSelectFrm", style = "display:none;" })%>
                <%= Html.TextBox("ValueTextFrm", null, new { id = formId + "ValueTextFrm", style = "display:none;", onkeypress = "return isNumbersAndPointKeyOrValue(event, this, true)" })%>
                <%= Html.ValidationMessage("Value", "*")%>
                <%= Html.ValidationMessage("ValueTextFrm", "*")%>
                <%= Html.ValidationMessage("ValueSelectFrm", "*")%>
                <div id="divMessageAlert" style="display: none; position: absolute;">
                    <label id="lblMessageAlert" style="color: rgb(255, 0, 0); font-size:smaller;">System will only accept a number or the value N/A</label><br />
                </div>
            </div>              
        </div>    

        <div class="line">
            <div class="field">
                <div>
                <label for="<%= formId %>Notes">
                    <asp:Literal ID="ProductCriteriaNotes" runat="server" Text="<%$ Resources:LabelResource, ProductCriteriaNotes %>" />:</label>
                <%= Html.TextArea("Notes", null, new { id = formId + "Notes" })%>
                <%= Html.ValidationMessage("Notes", "*")%>
                </div>
            </div>   
        </div>     
        <div class="line">      
            <div class="field">
                <div>
                    <label for="<%= formId %>Links">
                        <asp:Literal ID="ProductCriteriaLinks" runat="server" Text="<%$ Resources:LabelResource, ProductCriteriaLinks %>" />:</label>
                    <%= Html.TextArea("Links", null, new { id = formId + "Links" })%>
                    <%= Html.ValidationMessage("Links", "*")%>
                </div>            
            </div>
        </div>
        </div>
    </fieldset>
</div>
<% } %>
