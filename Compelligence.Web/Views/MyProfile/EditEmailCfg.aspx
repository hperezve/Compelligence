<%@ Page Title="Compelligence - Email configuration" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/BackEndSite.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.DataTransfer.Entity.EmailUserCfgRegistrationDTO>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
    <style type="text/css">
    .buttns
    {
     color :#959595;
    }
        .buttonDisable
        {
	        width:auto;/*75px;	*/
	        cursor:pointer;
	        background:transparent url(<%= ResolveUrl("~/Images/Styles/BGYellowGrad1.gif") %>) repeat top left;
	        border: 1px solid #aaaaaa;
	        height:20px;
	        font-weight:bold;
	        _width:auto;/*75px; /*for ie6*/
	        padding-left:8px;
	        padding-right:8px;
	        -moz-border-radius:4px 4px 4px 4px;
	        -webkit-border-radius: 4px 4px 4px 4px;
        	
        }

        .buttondisabled
        {
            background: transparent url(<%= ResolveUrl("~/Images/Styles/button_off.gif") %>) repeat top left;
        }
        .buttondisabled:hover{
	cursor:pointer;
	background: transparent url(<%= ResolveUrl("~/Images/Styles/button_off.gif") %>) repeat top left;
	border: 1px solid #aaaaaa;/*0;*/
	height:20px;
	width:auto;/*75px;*/
	_width:auto;/*75px; /*for ie6*/
	font-size:11px;
	font-weight:bold;
	padding-left:8px;
	padding-right:8px;
	-moz-border-radius:4px 4px 4px 4px;
	-webkit-border-radius: 4px 4px 4px 4px;
	color:#666666;
}
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"
        type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Dialogs.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/MicrosoftAjax.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.js") %>" type="text/javascript"></script>
    
    <script src="<%= Url.Content("~/Scripts/CKeditor/ckeditor.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.maskedinput-1.2.2.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>"       type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.mousewheel.min.js") %>" type="text/javascript"></script>
    <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-ui/ui.spinner.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.locale-en-min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/ui.multiselect.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/jquery.jqGrid.src.js") %>" type="text/javascript"></script>


    <script src="<%= Url.Content("~/Scripts/jquery.treeTable.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.rte.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/ext/ext-base.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>
<%--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet" type="text/css" />--%>
<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        window["showMessageSave"] = "";
        var showMessageResult = function() {
            var takeMessage = window["showMessageSave"];
            if (takeMessage == "Error") {
                $('#AlertReturnMessageDialog').html('The test configuration was not successfull. The configuration will not be saved!');
                $('#AlertReturnMessageDialog').dialog('open');
                return false;
            }
            if (takeMessage == "alertTest") {
                $('#AlertReturnMessageDialog').html('You need to click on buttom "Test Configuration" for saved the Inbox Mail Configuration.');
                $('#AlertReturnMessageDialog').dialog('open');
                return false;
            }
            if (takeMessage == '') {
                return true;
            }
        }
        function inspectAllInputFields() {
            var count = 0;
            var active = false;
            var Colors = "#959595";
            $('.inputIsRequired').each(function(i) {
                if ($(this).val() === '') {
                    //show a warning?
                    count++;
                }
                else {
                    active = true;
                }
                if (count == 0) {
                    $('#BtnTesting').prop('disabled', '');
                    window["showMessageSave"] = "alertTest";
                    //                    $('#BtnTesting').removeClass("buttondisabled");
                    $('#BtnTesting').addClass("button");
                } else {
                    $('#BtnTesting').prop('disabled', 'disabled');
                    window["showMessageSave"] = "";
                    $('#BtnTesting').removeClass("button");
                    //                    $('#BtnTesting').addClass("buttondisabled");
                }
            });
            if (active) {
                window["showMessageSave"] = "alertTest";
            }
        };
        $(function() {
            var smptserver = $("#SMTPServer").val();
            if (smptserver == null || smptserver == '' || smptserver == undefined) {
                $('#SecurityMethod').prop('value', '');
            }
            inspectAllInputFields();
            $("#ReturnReponseTestingDialog").dialog({
                bgiframe: true,
                autoOpen: false,
                modal: true,
                buttons: {
                    "Ok": function() {
                        $('#ReturnReponseTestingDialog').empty();
                        $(this).dialog('close');
                        hideLoadingDialog();
                    }
                }
            });
        });
        var showReturnReponseTestingDialog = function(Message) {
            $('#ReturnReponseTestingDialog').html(Message);
            $('#ReturnReponseTestingDialog').dialog('open');
        };
        function TestConfigtions(urlCheck, urlAction) {
            window["showMessageSave"] = "alertTest";
            var xmlhttp;
            var results = null;
            var MySelectSMTPServer = $('#SMTPServer');
            var MySelectPortNumber = $('#PortNumber');
            var MySelectUserIDs = $('#UserIDs');
            var MySelectSecurityMethod = $('#SecurityMethod');
            var MySelectAuthenticationMethod = $('#IsAuthenticationMethod');
            var MySelectPassword = $('#Password');

            var parametro = { SMTPServer: MySelectSMTPServer.val(), PortNumber: MySelectPortNumber.val(), UserIDs: MySelectUserIDs.val(), SecurityMethod: MySelectSecurityMethod.val(), AuthenticationMethod: MySelectAuthenticationMethod.val(), Password: MySelectPassword.val() };
            showLoadingDialog();
            $.get(
            urlCheck,
            parametro,
            function(data) {
                if (data != null && data != '') {
                    results = data;
                    if (results != "") {
                        var error = results.split('_K_');
                        if (error[0].replace('"', '') == "false") {
                            $('#AlertReturnMessageDialog').html('Error:' + error[1].replace('"', ''));
                            $('#AlertReturnMessageDialog').dialog('open');
                            window["showMessageSave"] = "Error";
                        }
                        else {
                            $('#AlertReturnMessageDialog').html(error[1].replace('"', ''));
                            $('#AlertReturnMessageDialog').dialog('open');
                            window["showMessageSave"] = "";
                        }
                        //var Message = '<p>' + results + '</p>';
                        //showReturnReponseTestingDialog(Message);
                        hideLoadingDialog();
                    }

                }
            });
        }
        function EnabledTexPassword() {
            var MySelect = $('#IsAuthenticationMethod');
            if (MySelect.val() != '' && MySelect.val() != null && MySelect.val() != undefined && MySelect.val() == "None") {
                document.getElementById('Password').disabled = true;
                $('#Password').val('');
                $('#Password').removeClass("inputIsRequired");
            }
            else {
                $('#Password').addClass("inputIsRequired");
                document.getElementById('Password').disabled = false;
            }
        }
        function CheckFormulario() {
            window["showMessageSave"] = "";
            $("#PortNumber").prop('value', '');
            $("#SMTPServer").prop('value', '');
            $("#Password").prop('value', '');
            $("#UserIDs").prop('value', '');
            $('#SecurityMethod').prop('value', '');
            $('#IsAuthenticationMethod').prop('value', '');
            var xmlhttp;

            xmlhttp = $.post('<%= Url.Action("DeleteEmailCfg","MyProfile") %>');
            xmlhttp.onreadystatechange = function() {
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("BackEndFormMessages"); %>
    <div class="EditProfile">
        <h1>
            Outgoing Email Server Configuration</h1>
        <%= Html.ValidationSummary()%>
        <% using (Html.BeginForm("EditEmailCfg", "MyProfile",
               FormMethod.Post, new { id = "EmailConfigurationEditForm", 
                   align = "left" }))
           { %>
        <%= Html.Hidden("UserId")%>
       
        <%= Html.Hidden("ClientCompany")%>
        <%= Html.Hidden("EditEmailPassword")%>
        <fieldset>
            <div class="line">
                <div class="line">
                    <label for="SMTPServer">
                        Outgoing SMTP server:</label>
                    <%= Html.TextBox("SMTPServer", null, new { id = "SMTPServer", @class = "inputIsRequired", onblur = "inspectAllInputFields()" })%>
                    <%= Html.ValidationMessage("SMTPServer", "*")%>
                </div>                
                 <div class="line">
                    <label for="SmtpPort">
                        Port Number:</label>
                    <%= Html.TextBox("SmtpPort", null, new { id = "PortNumber", @class = "inputIsRequired", onblur = "inspectAllInputFields()" })%>
                    <%= Html.ValidationMessage("PortNumber", "*")%>
                </div>
                <div class="line">
                    <label for="EmailUser">
                        User ID:</label>
                    <%= Html.TextBox("EmailUser", null, new { id = "UserIDs", @class = "inputIsRequired", onblur = "inspectAllInputFields()" })%>
                </div>
               <div class="line">
                    <label for="SmtpRequireSsl">
                        Security Method:</label>
                    <%= Html.DropDownList("SmtpRequireSsl", (SelectList)ViewData["SecurityMethod"], string.Empty, new { id = "SecurityMethod", onchange="inspectAllInputFields();" ,@class = "inputIsRequired"  })%>
                </div> 
              <div class="line">
                    <label for="IsAuthenticationMethod">
                        Authentication Method:</label>
                     <%= Html.DropDownList("IsAuthenticationMethod", (SelectList)ViewData["AuthenticationMethod"], string.Empty, new { id = "IsAuthenticationMethod", onchange = "EnabledTexPassword();inspectAllInputFields();", @class = "inputIsRequired" })%>
                   
                </div>
                <div class="line">
                
                    <label for="EmailPassword">
                        Password:</label>
                    <%= Html.Password("EmailPassword", (string)ViewData["passwordView"], new { id = "Password", @class = "inputIsRequired", onblur = "inspectAllInputFields()", autocomplete = "off" })%>
                </div>          
            </div>
            <div class="line">
            </div>
            <div class="line">
                <div class="field" style="margin-left:8%  ">
                    <input class="buttns"   value="Test Configuration"  id="BtnTesting"   onclick="javascript:TestConfigtions('<%= Url.Action("TestConfig", "MyProfile") %>')";/>
                </div>
               
            </div>          
             
            <div class="float-left" style="margin-top: 20px;padding-bottom: 10px;">
                <input class="button" style="width: 42px" type="submit" value="Save" onclick="return showMessageResult();"/>
                <input class="button" style="width: 145px" type="button" value="Use System Defaults" onclick="javascript: CheckFormulario();"/>
                <input class="button" style="width: 58px" type="button" value="Cancel" onclick="location.href='<%=Url.Action("EditProfile","MyProfile")%>'" />
            </div>
                            
            <div id="Instructions" style=" margin-top : -300px; margin-right: 15px; float:right; width:300px; height: 343px; border: 1px solid #000000;overflow: auto;">
                <label style= " color:#444444;margin-top: 20px;margin-left:10px; display: block;font-weight: bold; font-size: 14px;">Enter configurations for your personal SMTP server.</label>
                <label style= " color:#444444;margin-top: 20px;margin-left:10px; display: block;font-weight: bold; font-size: 14px;">This will cause newsletters which are owned by you to be sent using your SMTP server.</label>
                <label style= " color:#DC143C;margin-top: 20px;margin-left:115px; display: block;font-weight: bold; font-size: 14px;"><u>WARNING</u></label>
                <label style= " color:#444444;margin-top: 20px;margin-left:10px; display: block;font-weight: bold; font-size: 14px;">Some corporate e-mail servers will not allow connections from outside systems. Please ensure your IT environment is set up properly to allow Compelligence access to your SMTP server.</label>
                <label style= " color:#444444;margin-top: 20px;margin-left:10px; display: block;font-weight: bold; font-size: 14px;">As a best practice, SMTP login ID should be the same as Compelligence User ID.</label>
            </div>                 
            
        </fieldset>
        <% } %>
    </div>
    <div id="ReturnReponseTestingDialog" title="Test Configuration">
        </div>
             <div id="AlertReturnMessageDialog" title="Return Message">
        </div>
		<div id="ReturnReponseTestingDialog" title="Test Configuration">
        </div>
        <div id="LoadingDialog" class="displayNone">
            <p>
                <img src="<%= Url.Content("~/Content/Images/Ajax/loader.gif") %>" alt=""
                    class="left" /><span class="loadingDialog">Loading ...</span>
            </p>
        </div>
</asp:Content>
