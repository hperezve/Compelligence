<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>

<script src="<%= Url.Content("~/Scripts/System/BackEnd/Dialogs.js") %>" type="text/javascript"></script>
<style type="text/css">

</style>
<% string formId = "Defaults";
   ViewData["UserSecurityAccess"] = 3000;
   ViewData["EntityLocked"] = false;
   ViewData["IsDetail"] = false;
   ViewData["Scope"] = "Admin";
%>

<script type="text/javascript">
    function SucessDialog() {
        setTimeout("$.blockUI({ message: $('#SuccessUpdateMessage'), fadeIn: 700, fadeOut: 700, timeout: 3000," +
                            "showOverlay: false, centerY: false, css: { width: '300px', top: '10px', left: '', right: '10px', " +
                                "border: 'none', padding: '5px', backgroundColor: '#000', '-webkit-border-radius': '10px', '-moz-border-radius': '10px', opacity: 0.6, color: '#fff' } });", 1000);
    };
    function ConfimDialogo() {
        if (($('#IndustryInfoTab_ADMIN:checked').val() != 'true' && $('#InfoTab_ADMIN:checked').val() != 'true' && $('#SilverBulletsTab_ADMIN:checked').val() != 'true' && $('#PositioningTab_ADMIN:checked').val() != 'true' && $('#PricingTab_ADMIN:checked').val() != 'true' && $('#FeaturesTab_ADMIN:checked').val() != 'true' && $('#SalesToolsTab_ADMIN:checked').val() != 'true' && $('#NewsTab_ADMIN:checked').val() != 'true') ||
        ($('#IndustryInfoTab_ANALYST:checked').val() != 'true' && $('#InfoTab_ANALYST:checked').val() != 'true' && $('#SilverBulletsTab_ANALYST:checked').val() != 'true' && $('#PositioningTab_ANALYST:checked').val() != 'true' && $('#PricingTab_ANALYST:checked').val() != 'true' && $('#FeaturesTab_ANALYST:checked').val() != 'true' && $('#SalesToolsTab_ANALYST:checked').val() != 'true' && $('#NewsTab_ANALYST:checked').val() != 'true') ||
        ($('#IndustryInfoTab_MANAGER:checked').val() != 'true' && $('#InfoTab_MANAGER:checked').val() != 'true' && $('#SilverBulletsTab_MANAGER:checked').val() != 'true' && $('#PositioningTab_MANAGER:checked').val() != 'true' && $('#PricingTab_MANAGER:checked').val() != 'true' && $('#FeaturesTab_MANAGER:checked').val() != 'true' && $('#SalesToolsTab_MANAGER:checked').val() != 'true' && $('#NewsTab_MANAGER:checked').val() != 'true') ||
        ($('#IndustryInfoTab_ENDUSER:checked').val() != 'true' && $('#InfoTab_ENDUSER:checked').val() != 'true' && $('#SilverBulletsTab_ENDUSER:checked').val() != 'true' && $('#PositioningTab_ENDUSER:checked').val() != 'true' && $('#PricingTab_ENDUSER:checked').val() != 'true' && $('#FeaturesTab_ENDUSER:checked').val() != 'true' && $('#SalesToolsTab_ENDUSER:checked').val() != 'true' && $('#NewsTab_ENDUSER:checked').val() != 'true') ||
        ($('#IndustryInfoTab_PARTNER:checked').val() != 'true' && $('#InfoTab_PARTNER:checked').val() != 'true' && $('#SilverBulletsTab_PARTNER:checked').val() != 'true' && $('#PositioningTab_PARTNER:checked').val() != 'true' && $('#PricingTab_PARTNER:checked').val() != 'true' && $('#FeaturesTab_PARTNER:checked').val() != 'true' && $('#SalesToolsTab_PARTNER:checked').val() != 'true' && $('#NewsTab_PARTNER:checked').val() != 'true')) {

            $('#AlertReturnMessageDialog').html('Warning! If all tabs are hidden, no content will be shown on Comparinator');

            $('#AlertReturnMessageDialog').dialog('open');

        }
    };
    function CheckedTxtAll(idItem, classItem) {
        $('#' + idItem).blur(function(event) {
            if ($(this).val().length > 0) {
                $('.' + classItem).val($(this).val());
            } 
        });
    };
    function CheckedAll(idItem, classItem) {
        $('#' + idItem).click(function(event) {
            if (this.checked) {
                $('.' + classItem).each(function() { //loop through each checkbox
                    this.checked = true;  //select all checkboxes with class "chkComparinatorExport"              
                });
            } else {
            $('.' + classItem).each(function() { //loop through each checkbox
                    this.checked = false;  //select all checkboxes with class "chkComparinatorExport"              
                });
            }
        });
    };
    function CheckedItemForLengthItem(idItem, classItem, lengthItem) {
        $('.' + classItem).click(function(event) {
            if (this.checked) {
                var count = $('.' + classItem + '[type=checkbox]:checked').length;
                if (count === lengthItem) { ///to LENGTH types of users
                    $('#' + idItem).attr('checked', true);
                }
            } else {
                $('#' + idItem).removeAttr('checked');
            }
        });
    };
    function CheckValueInTextBoxes(idItem, classItem) {
        $('.' + classItem).blur(function(event) {
            if ($(this).val().length > 0) {
                var inputs = $('.' + classItem);
                var allTxtsEqual = true;
                for (var i = 0; i < inputs.length; i++) {
                    if ($(inputs[i]).val() != $(this).val()) {
                        allTxtsEqual = false;
                        i = inputs.length;
                    }
                }
                if (allTxtsEqual) {
                    $('#' + idItem).val($(this).val());
                } else {
                    $('#' + idItem).val("");
                }
            }
        });
    };
    function CheckedItem(idItem, classItem) {
        CheckedItemForLengthItem(idItem, classItem, 5);
    };
    function SetFunctionsCheckboxs() {
        CheckedAll('ComparinatorExport_ALL', 'chkComparinatorExport');
        CheckedAll('EnableTools_ALL', 'chkEnableTools');
        CheckedAll('IndustryInfoTab_ALL', 'chkIndustryInfoTab');
        CheckedAll('InfoTab_ALL', 'chkInfoTab');
        CheckedAll('SilverBulletsTab_ALL', 'chkSilverBulletsTab');
        CheckedAll('PositioningTab_ALL', 'chkPositioningTab');
        CheckedAll('PricingTab_ALL', 'chkPricingTab');
        CheckedAll('FeaturesTab_ALL', 'chkFeaturesTab');
        CheckedAll('SalesToolsTab_ALL', 'chkSalesToolsTab');
        CheckedAll('NewsTab_ALL', 'chkNewsTab');
        CheckedAll('SocialLog_ALL', 'chkSocialLog');
        CheckedAll('IndustryStandars_ALL', 'chkIndustryStandars');
        CheckedAll('Benefit_ALL', 'chkBenefit');
        CheckedAll('Cost_ALL', 'chkCost');
        CheckedTxtAll('Comparinator_ALL', 'txtComparinator');
        CheckedTxtAll('Content_ALL', 'txtContent');

        CheckedItem('ComparinatorExport_ALL', 'chkComparinatorExport');
        CheckedItemForLengthItem('EnableTools_ALL', 'chkEnableTools', 3);
        CheckedItem('IndustryInfoTab_ALL', 'chkIndustryInfoTab');
        CheckedItem('InfoTab_ALL', 'chkInfoTab');
        CheckedItem('SilverBulletsTab_ALL', 'chkSilverBulletsTab');
        CheckedItem('PositioningTab_ALL', 'chkPositioningTab');
        CheckedItem('PricingTab_ALL', 'chkPricingTab');
        CheckedItem('FeaturesTab_ALL', 'chkFeaturesTab');
        CheckedItem('SalesToolsTab_ALL', 'chkSalesToolsTab');
        CheckedItem('NewsTab_ALL', 'chkNewsTab');
        CheckedItem('SocialLog_ALL', 'chkSocialLog');
        CheckedItem('IndustryStandars_ALL', 'chkIndustryStandars');
        CheckedItem('Benefit_ALL', 'chkBenefit');
        CheckedItem('Cost_ALL', 'chkCost');
        CheckValueInTextBoxes('Comparinator_ALL', 'txtComparinator');
        CheckValueInTextBoxes('Content_ALL', 'txtContent');
    };
    $(document).ready(function() {
        SetFunctionsCheckboxs();
    });
</script>

<div id="<%= ViewData["Scope"] %>Defaults">
    <% using (Ajax.BeginForm("DefaultsSave", null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = "Defaults",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ executePostActionsConfigurationsTab('#" + formId + "', '" + ViewData["Scope"] + "', 'Configuration', " + ViewData["IsDetail"].ToString().ToLower() + ");ConfimDialogo();SetFunctionsCheckboxs();SucessDialog();}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
       { %>
<div>
        <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', 'DFAULT','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','Admin:Configurations:Defaults');" style="float: right;margin-right: 5px;margin-top:5px"/>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("Container")%>
        

        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("HeaderType")%>
        
        <div class="line" style="width: 99%;">
            <div class="firstContentConfiguration">
                <div class="lineTitleConfiguration">
                    <div class="headerTitleFirstContent">
                        <label class="lblTitleConfiguration">
                            <asp:Literal ID="CompariantorDisplay" runat="server" Text="<%$ Resources:LabelResource, CompariantorDisplay %>" />
                        </label>
                    </div>
                    <div class="fieldItemConfiguration">
                        <label class="lblTitleItemConfiguration">All</label>
                    </div>
                    <div class="fieldItemConfiguration">
                        <label class="lblTitleItemConfiguration">Admin</label>
                    </div>
                    <div class="fieldItemConfiguration">
                        <label class="lblTitleItemConfiguration">Analyst</label>
                    </div>
                    <div class="fieldItemConfiguration">
                        <label class="lblTitleItemConfiguration">Manager</label>
                    </div>
                    <div class="fieldItemConfiguration">
                        <label class="lblTitleItemConfiguration">End User</label>
                    </div>
                    <div class="fieldItemConfiguration">
                        <label class="lblTitleItemConfiguration">Partner</label>
                    </div>
                </div>
                
                <%--To Export Tools--%>
                <div>
                    <div class="headerTitleContent">
                        <label class="lblSubTitleConfig">Export Tools</label>
                    </div>
                    <div class="lineTitleConfiguration">
                        <div class="headerTitleProperty">
                            <label class="lblConfiguration">
                                <asp:Literal ID="lblComparinatorExport" runat="server" Text="<%$ Resources:LabelResource, EnableComparinatorExport %>" />
                            </label>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("ComparinatorExport_ALL", Convert.ToBoolean(ViewData["ComparinatorExport_ALL"]), new { id = "ComparinatorExport_ALL", @class = "chkItemConfiguration", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("ComparinatorExport_ADMIN", Convert.ToBoolean(ViewData["ComparinatorExport_ADMIN"]), new { id = "ComparinatorExport_ADMIN", @class = "chkItemConfiguration chkComparinatorExport", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("ComparinatorExport_ANALYST", Convert.ToBoolean(ViewData["ComparinatorExport_ANALYST"]), new { id = "ComparinatorExport_ANALYST", @class = "chkItemConfiguration chkComparinatorExport", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("ComparinatorExport_MANAGER", Convert.ToBoolean(ViewData["ComparinatorExport_MANAGER"]), new { id = "ComparinatorExport_MANAGER", @class = "chkItemConfiguration chkComparinatorExport", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("ComparinatorExport_ENDUSER", Convert.ToBoolean(ViewData["ComparinatorExport_ENDUSER"]), new { id = "ComparinatorExport_ENDUSER", @class = "chkItemConfiguration chkComparinatorExport", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("ComparinatorExport_PARTNER", Convert.ToBoolean(ViewData["ComparinatorExport_PARTNER"]), new { id = "ComparinatorExport_PARTNER", @class = "chkItemConfiguration chkComparinatorExport", style = "height:10px; " })%>
                        </div>
                    </div>
                </div>
                
                <%--To Comparinator Tools--%>
                <div>
                    <div class="headerTitleContent">
                        <label class="lblSubTitleConfig">Comparinator Tools</label>
                    </div>
                    <div class="lineTitleConfiguration">
                        <div class="headerTitleProperty">
                            <label class="lblConfiguration">
                                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:LabelResource, EnabledComparinatorFeaturesEditTools %>" />
                            </label>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("EnableTools_ALL", Convert.ToBoolean(ViewData["EnableTools_ALL"]), new { id = "EnableTools_ALL", @class = "chkItemConfiguration", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("EnableTools_ADMIN", Convert.ToBoolean(ViewData["EnableTools_ADMIN"]), new { id = "EnableTools_ADMIN", @class = "chkItemConfiguration chkEnableTools", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("EnableTools_ANALYST", Convert.ToBoolean(ViewData["EnableTools_ANALYST"]), new { id = "EnableTools_ANALYST", @class = "chkItemConfiguration chkEnableTools", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("EnableTools_MANAGER", Convert.ToBoolean(ViewData["EnableTools_MANAGER"]), new { id = "EnableTools_MANAGER", @class = "chkItemConfiguration chkEnableTools", style = "height:10px; " })%>
                        </div>                                              
                    </div>
                </div>
                
                <%--To Result--%>
                <div>
                    <div class="headerTitleContent">
                        <label class="lblSubTitleConfig">Result</label>
                    </div>
                    <div class="lineTitleConfiguration">
                        <div class="headerTitleProperty">
                            <label class="lblConfiguration">
                                <%= ViewData["IndustryLabel"]%>
                            </label>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("IndustryInfoTab_ALL", Convert.ToBoolean(ViewData["IndustryInfoTab_ALL"]), new { id = "IndustryInfoTab_ALL", @class = "chkItemConfiguration", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("IndustryInfoTab_ADMIN", Convert.ToBoolean(ViewData["IndustryInfoTab_ADMIN"]), new { id = "IndustryInfoTab_ADMIN", @class = "chkItemConfiguration chkIndustryInfoTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("IndustryInfoTab_ANALYST", Convert.ToBoolean(ViewData["IndustryInfoTab_ANALYST"]), new { id = "IndustryInfoTab_ANALYST", @class = "chkItemConfiguration chkIndustryInfoTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("IndustryInfoTab_MANAGER", Convert.ToBoolean(ViewData["IndustryInfoTab_MANAGER"]), new { id = "IndustryInfoTab_MANAGER", @class = "chkItemConfiguration chkIndustryInfoTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("IndustryInfoTab_ENDUSER", Convert.ToBoolean(ViewData["IndustryInfoTab_ENDUSER"]), new { id = "IndustryInfoTab_ENDUSER", @class = "chkItemConfiguration chkIndustryInfoTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("IndustryInfoTab_PARTNER", Convert.ToBoolean(ViewData["IndustryInfoTab_PARTNER"]), new { id = "IndustryInfoTab_PARTNER", @class = "chkItemConfiguration chkIndustryInfoTab", style = "height:10px; " })%>
                        </div>
                    </div>
                    <div class="lineTitleConfiguration">
                        <div class="headerTitleProperty">
                            <label class="lblConfiguration">
                                <asp:Literal ID="lblInfoTab" runat="server" Text="<%$ Resources:LabelResource, ComparinatorResultInfoTab %>" />
                            </label>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("InfoTab_ALL", Convert.ToBoolean(ViewData["InfoTab_ALL"]), new { id = "InfoTab_ALL", @class = "chkItemConfiguration", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("InfoTab_ADMIN", Convert.ToBoolean(ViewData["InfoTab_ADMIN"]), new { id = "InfoTab_ADMIN", @class = "chkItemConfiguration chkInfoTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("InfoTab_ANALYST", Convert.ToBoolean(ViewData["InfoTab_ANALYST"]), new { id = "InfoTab_ANALYST", @class = "chkItemConfiguration chkInfoTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("InfoTab_MANAGER", Convert.ToBoolean(ViewData["InfoTab_MANAGER"]), new { id = "InfoTab_MANAGER", @class = "chkItemConfiguration chkInfoTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("InfoTab_ENDUSER", Convert.ToBoolean(ViewData["InfoTab_ENDUSER"]), new { id = "InfoTab_ENDUSER", @class = "chkItemConfiguration chkInfoTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("InfoTab_PARTNER", Convert.ToBoolean(ViewData["InfoTab_PARTNER"]), new { id = "InfoTab_PARTNER", @class = "chkItemConfiguration chkInfoTab", style = "height:10px; " })%>
                        </div>
                    </div>
                    <div class="lineTitleConfiguration">
                        <div class="headerTitleProperty">
                            <label class="lblConfiguration">
                                <asp:Literal ID="lblSilverBulletsTab" runat="server" Text="<%$ Resources:LabelResource, ComparinatorResultSilverBulletsTab %>" />
                            </label>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("SilverBulletsTab_ALL", Convert.ToBoolean(ViewData["SilverBulletsTab_ALL"]), new { id = "SilverBulletsTab_ALL", @class = "chkItemConfiguration", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("SilverBulletsTab_ADMIN", Convert.ToBoolean(ViewData["SilverBulletsTab_ADMIN"]), new { id = "SilverBulletsTab_ADMIN", @class = "chkItemConfiguration chkSilverBulletsTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("SilverBulletsTab_ANALYST", Convert.ToBoolean(ViewData["SilverBulletsTab_ANALYST"]), new { id = "SilverBulletsTab_ANALYST", @class = "chkItemConfiguration chkSilverBulletsTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("SilverBulletsTab_MANAGER", Convert.ToBoolean(ViewData["SilverBulletsTab_MANAGER"]), new { id = "SilverBulletsTab_MANAGER", @class = "chkItemConfiguration chkSilverBulletsTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("SilverBulletsTab_ENDUSER", Convert.ToBoolean(ViewData["SilverBulletsTab_ENDUSER"]), new { id = "SilverBulletsTab_ENDUSER", @class = "chkItemConfiguration chkSilverBulletsTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("SilverBulletsTab_PARTNER", Convert.ToBoolean(ViewData["SilverBulletsTab_PARTNER"]), new { id = "SilverBulletsTab_PARTNER", @class = "chkItemConfiguration chkSilverBulletsTab", style = "height:10px; " })%>
                        </div>
                    </div>
                    <div class="lineTitleConfiguration">
                        <div class="headerTitleProperty">
                            <label class="lblConfiguration">
                                <asp:Literal ID="lblPositioningTab" runat="server" Text="<%$ Resources:LabelResource, ComparinatorResultPositioningTab %>" />
                            </label>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("PositioningTab_ALL", Convert.ToBoolean(ViewData["PositioningTab_ALL"]), new { id = "PositioningTab_ALL", @class = "chkItemConfiguration", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("PositioningTab_ADMIN", Convert.ToBoolean(ViewData["PositioningTab_ADMIN"]), new { id = "PositioningTab_ADMIN", @class = "chkItemConfiguration chkPositioningTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("PositioningTab_ANALYST", Convert.ToBoolean(ViewData["PositioningTab_ANALYST"]), new { id = "PositioningTab_ANALYST", @class = "chkItemConfiguration chkPositioningTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("PositioningTab_MANAGER", Convert.ToBoolean(ViewData["PositioningTab_MANAGER"]), new { id = "PositioningTab_MANAGER", @class = "chkItemConfiguration chkPositioningTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("PositioningTab_ENDUSER", Convert.ToBoolean(ViewData["PositioningTab_ENDUSER"]), new { id = "PositioningTab_ENDUSER", @class = "chkItemConfiguration chkPositioningTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("PositioningTab_PARTNER", Convert.ToBoolean(ViewData["PositioningTab_PARTNER"]), new { id = "PositioningTab_PARTNER", @class = "chkItemConfiguration chkPositioningTab", style = "height:10px; " })%>
                        </div>
                    </div>
                    <div class="lineTitleConfiguration">
                        <div class="headerTitleProperty">
                            <label class="lblConfiguration">
                                <asp:Literal ID="lblPricingTab" runat="server" Text="<%$ Resources:LabelResource, ComparinatorResultPricingTab %>" />
                            </label>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("PricingTab_ALL", Convert.ToBoolean(ViewData["PricingTab_ALL"]), new { id = "PricingTab_ALL", @class = "chkItemConfiguration", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("PricingTab_ADMIN", Convert.ToBoolean(ViewData["PricingTab_ADMIN"]), new { id = "PricingTab_ADMIN", @class = "chkItemConfiguration chkPricingTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("PricingTab_ANALYST", Convert.ToBoolean(ViewData["PricingTab_ANALYST"]), new { id = "PricingTab_ANALYST", @class = "chkItemConfiguration chkPricingTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("PricingTab_MANAGER", Convert.ToBoolean(ViewData["PricingTab_MANAGER"]), new { id = "PricingTab_MANAGER", @class = "chkItemConfiguration chkPricingTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("PricingTab_ENDUSER", Convert.ToBoolean(ViewData["PricingTab_ENDUSER"]), new { id = "PricingTab_ENDUSER", @class = "chkItemConfiguration chkPricingTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("PricingTab_PARTNER", Convert.ToBoolean(ViewData["PricingTab_PARTNER"]), new { id = "PricingTab_PARTNER", @class = "chkItemConfiguration chkPricingTab", style = "height:10px; " })%>
                        </div>
                    </div>
                    <div class="lineTitleConfiguration">
                        <div class="headerTitleProperty">
                            <label class="lblConfiguration">
                                <asp:Literal ID="lblFeaturesTab" runat="server" Text="<%$ Resources:LabelResource, ComparinatorResultFeaturesTab %>" />
                            </label>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("FeaturesTab_ALL", Convert.ToBoolean(ViewData["FeaturesTab_ALL"]), new { id = "FeaturesTab_ALL", @class = "chkItemConfiguration", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("FeaturesTab_ADMIN", Convert.ToBoolean(ViewData["FeaturesTab_ADMIN"]), new { id = "FeaturesTab_ADMIN", @class = "chkItemConfiguration chkFeaturesTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("FeaturesTab_ANALYST", Convert.ToBoolean(ViewData["FeaturesTab_ANALYST"]), new { id = "FeaturesTab_ANALYST", @class = "chkItemConfiguration chkFeaturesTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("FeaturesTab_MANAGER", Convert.ToBoolean(ViewData["FeaturesTab_MANAGER"]), new { id = "FeaturesTab_MANAGER", @class = "chkItemConfiguration chkFeaturesTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("FeaturesTab_ENDUSER", Convert.ToBoolean(ViewData["FeaturesTab_ENDUSER"]), new { id = "FeaturesTab_ENDUSER", @class = "chkItemConfiguration chkFeaturesTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("FeaturesTab_PARTNER", Convert.ToBoolean(ViewData["FeaturesTab_PARTNER"]), new { id = "FeaturesTab_PARTNER", @class = "chkItemConfiguration chkFeaturesTab", style = "height:10px; " })%>
                        </div>
                    </div>
                    <div class="lineTitleConfiguration">
                        <div class="headerTitleProperty">
                            <label class="lblConfiguration">
                                <asp:Literal ID="lblSalesToolsTab" runat="server" Text="<%$ Resources:LabelResource, ComparinatorResultSalesToolsTab %>" />
                            </label>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("SalesToolsTab_ALL", Convert.ToBoolean(ViewData["SalesToolsTab_ALL"]), new { id = "SalesToolsTab_ALL", @class = "chkItemConfiguration", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("SalesToolsTab_ADMIN", Convert.ToBoolean(ViewData["SalesToolsTab_ADMIN"]), new { id = "SalesToolsTab_ADMIN", @class = "chkItemConfiguration chkSalesToolsTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("SalesToolsTab_ANALYST", Convert.ToBoolean(ViewData["SalesToolsTab_ANALYST"]), new { id = "SalesToolsTab_ANALYST", @class = "chkItemConfiguration chkSalesToolsTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("SalesToolsTab_MANAGER", Convert.ToBoolean(ViewData["SalesToolsTab_MANAGER"]), new { id = "SalesToolsTab_MANAGER", @class = "chkItemConfiguration chkSalesToolsTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("SalesToolsTab_ENDUSER", Convert.ToBoolean(ViewData["SalesToolsTab_ENDUSER"]), new { id = "SalesToolsTab_ENDUSER", @class = "chkItemConfiguration chkSalesToolsTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("SalesToolsTab_PARTNER", Convert.ToBoolean(ViewData["SalesToolsTab_PARTNER"]), new { id = "SalesToolsTab_PARTNER", @class = "chkItemConfiguration chkSalesToolsTab", style = "height:10px; " })%>
                        </div>
                    </div>
                    <div class="lineTitleConfiguration">
                        <div class="headerTitleProperty">
                            <label class="lblConfiguration">
                                <asp:Literal ID="lblNewsTab" runat="server" Text="<%$ Resources:LabelResource, ComparinatorResultNewsTab %>" />
                            </label>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("NewsTab_ALL", Convert.ToBoolean(ViewData["NewsTab_ALL"]), new { id = "NewsTab_ALL", @class = "chkItemConfiguration", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("NewsTab_ADMIN", Convert.ToBoolean(ViewData["NewsTab_ADMIN"]), new { id = "NewsTab_ADMIN", @class = "chkItemConfiguration chkNewsTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("NewsTab_ANALYST", Convert.ToBoolean(ViewData["NewsTab_ANALYST"]), new { id = "NewsTab_ANALYST", @class = "chkItemConfiguration chkNewsTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("NewsTab_MANAGER", Convert.ToBoolean(ViewData["NewsTab_MANAGER"]), new { id = "NewsTab_MANAGER", @class = "chkItemConfiguration chkNewsTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("NewsTab_ENDUSER", Convert.ToBoolean(ViewData["NewsTab_ENDUSER"]), new { id = "NewsTab_ENDUSER", @class = "chkItemConfiguration chkNewsTab", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("NewsTab_PARTNER", Convert.ToBoolean(ViewData["NewsTab_PARTNER"]), new { id = "NewsTab_PARTNER", @class = "chkItemConfiguration chkNewsTab", style = "height:10px; " })%>
                        </div>
                    </div>
                </div>
                
                <%--To Social--%>
                <div>
                    <div class="headerTitleContent">
                        <label class="lblSubTitleConfig">Social Log</label>
                    </div>
                    <div class="lineTitleConfiguration">
                        <div class="headerTitleProperty">
                            <label class="lblConfiguration">
                                <asp:Literal ID="lblSocialLog" runat="server" Text="<%$ Resources:LabelResource, ComparinatorSocialLog %>" />
                            </label>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("SocialLog_ALL", Convert.ToBoolean(ViewData["SocialLog_ALL"]), new { id = "SocialLog_ALL", @class = "chkItemConfiguration", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("SocialLog_ADMIN", Convert.ToBoolean(ViewData["SocialLog_ADMIN"]), new { id = "SocialLog_ADMIN", @class = "chkItemConfiguration chkSocialLog", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("SocialLog_ANALYST", Convert.ToBoolean(ViewData["SocialLog_ANALYST"]), new { id = "SocialLog_ANALYST", @class = "chkItemConfiguration chkSocialLog", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("SocialLog_MANAGER", Convert.ToBoolean(ViewData["SocialLog_MANAGER"]), new { id = "SocialLog_MANAGER", @class = "chkItemConfiguration chkSocialLog", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("SocialLog_ENDUSER", Convert.ToBoolean(ViewData["SocialLog_ENDUSER"]), new { id = "SocialLog_ENDUSER", @class = "chkItemConfiguration chkSocialLog", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("SocialLog_PARTNER", Convert.ToBoolean(ViewData["SocialLog_PARTNER"]), new { id = "SocialLog_PARTNER", @class = "chkItemConfiguration chkSocialLog", style = "height:10px; " })%>
                        </div>
                    </div>
                </div>
                
                
                <%--To Default Features Display--%>
                <div>
                    <div class="headerTitleContent">
                        <label class="lblSubTitleConfig">
                            <asp:Literal ID="lblFeatureDisplays" runat="server" Text="<%$ Resources:LabelResource, ComparinatorResultDefaultFeatureDisplays %>" />
                        </label>
                    </div>
                    <div class="lineTitleConfiguration">
                        <div class="headerTitleProperty">
                            <label class="lblConfiguration">
                                <asp:Literal ID="lblIndustryStandars" runat="server" Text="<%$ Resources:LabelResource, ComparinatorResultIndustryStandars %>" />
                            </label>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("IndustryStandars_ALL", Convert.ToBoolean(ViewData["IndustryStandars_ALL"]), new { id = "IndustryStandars_ALL", @class = "chkItemConfiguration", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("IndustryStandars_ADMIN", Convert.ToBoolean(ViewData["IndustryStandars_ADMIN"]), new { id = "IndustryStandars_ADMIN", @class = "chkItemConfiguration chkIndustryStandars", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("IndustryStandars_ANALYST", Convert.ToBoolean(ViewData["IndustryStandars_ANALYST"]), new { id = "IndustryStandars_ANALYST", @class = "chkItemConfiguration chkIndustryStandars", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("IndustryStandars_MANAGER", Convert.ToBoolean(ViewData["IndustryStandars_MANAGER"]), new { id = "IndustryStandars_MANAGER", @class = "chkItemConfiguration chkIndustryStandars", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("IndustryStandars_ENDUSER", Convert.ToBoolean(ViewData["IndustryStandars_ENDUSER"]), new { id = "IndustryStandars_ENDUSER", @class = "chkItemConfiguration chkIndustryStandars", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("IndustryStandars_PARTNER", Convert.ToBoolean(ViewData["IndustryStandars_PARTNER"]), new { id = "IndustryStandars_PARTNER", @class = "chkItemConfiguration chkIndustryStandars", style = "height:10px; " })%>
                        </div>
                    </div>
                    <div class="lineTitleConfiguration">
                        <div class="headerTitleProperty">
                            <label class="lblConfiguration">
                                <asp:Literal ID="lblBenefit" runat="server" Text="<%$ Resources:LabelResource, ComparinatorResultBenefit %>" />
                            </label>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("Benefit_ALL", Convert.ToBoolean(ViewData["Benefit_ALL"]), new { id = "Benefit_ALL", @class = "chkItemConfiguration", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("Benefit_ADMIN", Convert.ToBoolean(ViewData["Benefit_ADMIN"]), new { id = "Benefit_ADMIN", @class = "chkItemConfiguration chkBenefit", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("Benefit_ANALYST", Convert.ToBoolean(ViewData["Benefit_ANALYST"]), new { id = "Benefit_ANALYST", @class = "chkItemConfiguration chkBenefit", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("Benefit_MANAGER", Convert.ToBoolean(ViewData["Benefit_MANAGER"]), new { id = "Benefit_MANAGER", @class = "chkItemConfiguration chkBenefit", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("Benefit_ENDUSER", Convert.ToBoolean(ViewData["Benefit_ENDUSER"]), new { id = "Benefit_ENDUSER", @class = "chkItemConfiguration chkBenefit", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("Benefit_PARTNER", Convert.ToBoolean(ViewData["Benefit_PARTNER"]), new { id = "Benefit_PARTNER", @class = "chkItemConfiguration chkBenefit", style = "height:10px; " })%>
                        </div>
                    </div>
                    <div class="lineTitleConfiguration">
                        <div class="headerTitleProperty">
                            <label class="lblConfiguration">
                                <asp:Literal ID="lblCost" runat="server" Text="<%$ Resources:LabelResource, ComparinatorResultCost %>" />
                            </label>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("Cost_ALL", Convert.ToBoolean(ViewData["Cost_ALL"]), new { id = "Cost_ALL", @class = "chkItemConfiguration", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("Cost_ADMIN", Convert.ToBoolean(ViewData["Cost_ADMIN"]), new { id = "Cost_ADMIN", @class = "chkItemConfiguration chkCost", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("Cost_ANALYST", Convert.ToBoolean(ViewData["Cost_ANALYST"]), new { id = "Cost_ANALYST", @class = "chkItemConfiguration chkCost", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("Cost_MANAGER", Convert.ToBoolean(ViewData["Cost_MANAGER"]), new { id = "Cost_MANAGER", @class = "chkItemConfiguration chkCost", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("Cost_ENDUSER", Convert.ToBoolean(ViewData["Cost_ENDUSER"]), new { id = "Cost_ENDUSER", @class = "chkItemConfiguration chkCost", style = "height:10px; " })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.CheckBox("Cost_PARTNER", Convert.ToBoolean(ViewData["Cost_PARTNER"]), new { id = "Cost_PARTNER", @class = "chkItemConfiguration chkCost", style = "height:10px; " })%>
                        </div>
                    </div>
                    <div class="lineTitleConfiguration">
                        <div class="headerTitleProperty">
                            <label class="lblConfiguration">
                                <asp:Literal ID="LblContent" runat="server" Text="Content" />
                            </label>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.TextBox("Content_ALL", ViewData["Content_ALL"], new { id = "Content_ALL", @class = "txtItemConfiguration", style = "width: 90px;" })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.TextBox("Content_ADMIN", ViewData["Content_ADMIN"], new { id = "Content_ADMIN", @class = "txtItemConfiguration txtContent", style = "width: 90px;" })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.TextBox("Content_ANALYST", ViewData["Content_ANALYST"], new { id = "Content_ANALYST", @class = "txtItemConfiguration txtContent", style = "width: 90px;" })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.TextBox("Content_MANAGER", ViewData["Content_MANAGER"], new { id = "Content_MANAGER", @class = "txtItemConfiguration txtContent", style = "width: 90px;" })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.TextBox("Content_ENDUSER", ViewData["Content_ENDUSER"], new { id = "Content_ENDUSER", @class = "txtItemConfiguration txtContent", style = "width: 90px;" })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.TextBox("Content_PARTNER", ViewData["Content_PARTNER"], new { id = "Content_PARTNER", @class = "txtItemConfiguration txtContent", style = "width: 90px;" })%>
                        </div>
                    </div>
                    <div class="lineTitleConfiguration">
                        <div class="headerTitleProperty">
                            <label class="lblConfiguration">
                                <asp:Literal ID="Literal2" runat="server" Text="Comparinator" />
                            </label>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.TextBox("Comparinator_ALL", ViewData["Comparinator_ALL"], new { id = "Comparinator_ALL", @class = "txtItemConfiguration", style = "width: 90px;" })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.TextBox("Comparinator_ADMIN", ViewData["Comparinator_ADMIN"], new { id = "Comparinator_ADMIN", @class = "txtItemConfiguration txtComparinator", style = "width: 90px;"  })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.TextBox("Comparinator_ANALYST", ViewData["Comparinator_ANALYST"], new { id = "Comparinator_ANALYST", @class = "txtItemConfiguration txtComparinator", style = "width: 90px;"  })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.TextBox("Comparinator_MANAGER", ViewData["Comparinator_MANAGER"], new { id = "Comparinator_MANAGER", @class = "txtItemConfiguration txtComparinator", style = "width: 90px;"  })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.TextBox("Comparinator_ENDUSER", ViewData["Comparinator_ENDUSER"], new { id = "Comparinator_ENDUSER", @class = "txtItemConfiguration txtComparinator", style = "width: 90px;" })%>
                        </div>
                        <div class="fieldItemConfiguration">
                            <%= Html.TextBox("Comparinator_PARTNER", ViewData["Comparinator_PARTNER"], new { id = "Comparinator_PARTNER", @class = "txtItemConfiguration txtComparinator", style = "width: 90px;"  })%>
                        </div>
                    </div>
                </div>
             </div>
            <div class="secondContentConfiguration">
                <label class="lblTitleConfiguration">
                    <asp:Literal ID="ConfigurationDefaultOtherFeatures" runat="server" Text="<%$ Resources:LabelResource, OtherFeatures %>" /></label>
                <div style="padding-bottom:5px;padding-top:5px;">
                        <label class="lblSubTitleConfig">Public Comment</label><br />
                    </div>
                <div class="line">
                    <%= Html.CheckBox("DisabledPublicComment", Convert.ToBoolean(ViewData["DisabledPublicComment"]), new { id = "DisabledPublicComment", @class = "chkConfiguration", style = "height:10px;" })%>
                    <label for="DisabledPublicComment" class="lblConfiguration">
                        <asp:Literal ID="ConfigurationDefaultDisabledPublicComment" runat="server" Text="<%$ Resources:LabelResource, OtherFeaturesDisabledPublicComment %>" /></label><br />
                </div>
                <div style="padding-bottom:5px;padding-top:5px;">                        
                        <label class="lblSubTitleConfig"></label><br/>
                    </div>                                            
                <div class="line" style="margin-left: 18px;">
                        <label for="SameValues" style="top: 20px;  ">
                           <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:LabelResource, ComparinatorResultSameValues %>" /> </label>
                            <%= Html.DropDownList("SameValues", ViewData["SameValuesList"] as SelectList, new { style = "width: 150px; " })%>
                        <br />              
                </div>
                <br/> 
            </div>            
        </div>
<%--        <div class="line">
            <label for="SameValues" style="top: 5px;  ">
               <asp:Literal ID="ConfigurationDefaultSameValues" runat="server" Text="<%$ Resources:LabelResource, ComparinatorResultSameValues %>" /> </label>
            <%= Html.DropDownList("SameValues", ViewData["SameValuesList"] as SelectList, new { style = "width: 150px; " })%>
            <br />
        </div>--%>
        <div class="line" style="top: 15px; margin-left: 10px;">
            <div class="field">
                <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            </div>
        </div>
    </div>
    <% } %>
</div>
