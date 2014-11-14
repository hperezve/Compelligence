<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>
   <script src="<%= Url.Content("~/Scripts/System/BackEnd/Dialogs.js") %>" type="text/javascript"></script>
<script type="text/javascript">
    var TypeServer;
    window["showMessageSave"] = "";
    function inspectAllInputFields() {
        var password = $("#Kennwort").val();
        var retypePassword = $("#ReKennwort").val();
        var MySelectEmail = $('#InboxEmail').val();
        //var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
        var emailReg = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;                
        if (emailReg.test(MySelectEmail)) {
            $('#InboxEmail').removeClass("validationError");
            $('#messageEmail').removeClass("labelPassword");
            if (retypePassword != '' && password != retypePassword) {
                $('#ReKennwort').addClass("validationError");
                $('#showPassword').addClass("labelPassword");
            }
            if (password == retypePassword) {
                $('#ReKennwort').removeClass("validationError");
                $('#showPassword').removeClass("labelPassword");                 
                var count = 0;
                var countPass = 0;
                var Colors = "#959595";
                $('.inputIsRequired').each(function(i) {

                    if ($(this).val() === '') {
                        //show a warning?
                        count++;
                    }
                    if (count == 0) {
                        $('#BtnTesting').prop('disabled', '');
                        //                    $('#BtnTesting').removeClass("buttondisabled");
                        $('#BtnTesting').addClass("button");
                    } else {
                    $('#BtnTesting').prop('disabled', 'disabled');
                        $('#BtnTesting').removeClass("button");
                        //                    $('#BtnTesting').addClass("buttondisabled");
                    }

                });
            }
            else {
                $('#BtnTesting').prop('disabled', 'disabled');
                $('#BtnTesting').removeClass("button");
            }
        }
        else {
            if (password == retypePassword || retypePassword == '') {                
                $('#ReKennwort').removeClass("validationError");
                $('#showPassword').removeClass("labelPassword");
            }
            if (retypePassword != '' && password != retypePassword) {
                $('#ReKennwort').addClass("validationError");
                $('#showPassword').addClass("labelPassword");
            }
            $('#InboxEmail').addClass("validationError");
            $('#messageEmail').addClass("labelPassword");
            $('#BtnTesting').prop('disabled', 'disabled');
            $('#BtnTesting').removeClass("button");
        }
        

    };
    function validatePassword() {
        var password = $("#Kennwort").val();
        var retypePassword = $("#ReKennwort").val();
        if (password != retypePassword) {
            $('#ReKennwort').addClass("validationError");
            $('#showPassword').addClass("labelPassword");
        }
    }
    function removeConfiguration() {
        $('#InboxEmail').val("");
        $('#Kennwort').val("");
        $('#ReKennwort').val("");
        $('#PopPort').val("");
        $(":radio[value=Pop]").prop('checked', true)        
        $('#PopServer').val("");
        $("#PopRequireSsl").prop('checked', false);
        $('#ReKennwort').removeClass("validationError");
        $('#showPassword').removeClass("labelPassword");
        $('#InboxEmail').removeClass("validationError");
        $('#messageEmail').removeClass("labelPassword");
        $('#BtnTesting').prop('disabled', 'disabled');
        $('#BtnTesting').removeClass("button");
        $('#vrfMail').val("true");
        TypeServer = "Pop";
        window["showMessageSave"] = "";
    }
    function setValueServer(type) {
        if (type == '1') {
            TypeServer = "Pop";
        }
        else {
            TypeServer = "Imap";
        }
    }
    var trim = function(textField) {
        var resultText = '';
        for (var i = 0; i < textField.value.length; i++) {
            if (textField.value.charAt(i) != ' ')
                resultText += textField.value.charAt(i);
        }
        textField.value = resultText;
    };
    function inputChange() {
        window["showMessageSave"] = "alertTest";
        var MySelectEmail = trim($('#InboxEmail').val());
        var MySelectPassword = trim($('#Kennwort').val());
        var MySelectReTypePassword = trim($('#ReKennwort').val());
        var MySelectPort = trim($('#PopPort').val());
        var MySelectServer = trim($('#PopServer').val());
        if (MySelectEmail != '' && MySelectPassword != '' && MySelectReTypePassword != '' && MySelectPort != '' && MySelectServer == '') {
            $('#vrfMail').val("false");                       
        }
        else {
            $('#vrfMail').val("true");
            window["showMessageSave"] = "";          
        }
    }

    function TestConfigtions(urlCheck, urlAction) {
        var MySelectServerType = TypeServer;
        var xmlhttp;
        var results = null;
        var MySelectEmail = $('#InboxEmail');
        var MySelectPassword = $('#Kennwort');
        var MySelectPort = $('#PopPort');
        var MySelectServer = $('#PopServer');        
        var MySelectRequireSSL = $('#PopRequireSsl');

       var parametro = { Email: MySelectEmail.val(), KeyPassword: MySelectPassword.val(), Port: MySelectPort.val(), Server: MySelectServer.val(), ServerType: MySelectServerType, RequireSSL: MySelectRequireSSL.val() };
       showLoadingDialog();

       $.get(
            urlCheck,
            parametro,
            function(data) {
                if (data != null && data != '') {
                    results = data;
                    if (results == '1') {
                        window["showMessageSave"] = "";
                        results = "The inbox email configuration was Successfull.";
                        $('#vrfMail').val("true");
                    }
                    else {
                        window["showMessageSave"] = "Error";
                        results = "The inbox mail does not have the correct format. Please review the configuration.";
                        $('#vrfMail').val("false");
                    }
                    $('#AlertReturnMessageDialog').html(results);
                    $('#AlertReturnMessageDialog').dialog('open');
                    hideLoadingDialog();
                }
            });
    }
    function validateEmail() {
        var MySelectEmail = $('#InboxEmail').val();
        var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
        if (emailReg.test(MySelectEmail)) {
            $('#InboxEmail').removeClass("validationError");
            $('#messageEmail').removeClass("labelPassword");            
        }
        else {
            $('#InboxEmail').addClass("validationError");
            $('#messageEmail').addClass("labelPassword"); 
        }

    }
    function showMessageResult() {
        var takeMessage = window["showMessageSave"];
        if (takeMessage == "Error") {
            $('#AlertReturnMessageDialog').html('The test configuration was not successfull. The configuration will not be saved!');
            $('#AlertReturnMessageDialog').dialog('open');
        }
        if (takeMessage == "alertTest") {
            $('#AlertReturnMessageDialog').html('You need to click on buttom "Test Configuration" for saved the Inbox Mail Configuration.');
            $('#AlertReturnMessageDialog').dialog('open');
        }
        window["showMessageSave"] = "";
    }
    $(function() {
        inspectAllInputFields();
        var MySelectServerTypeImap = '<%= ViewData["TypeServerImap"] %>';
        var MySelectServerTypePop = '<%= ViewData["TypeServerPop"] %>';
        if (MySelectServerTypePop == "True") {
            TypeServer = "Pop";
        }
        else {
            TypeServer = "Imap";
        }
        $('#BtnTesting').prop('disabled', 'disabled');
       
    });
    
</script>
<% string formId = "Configuration";
   ViewData["UserSecurityAccess"] = 3000;
   ViewData["EntityLocked"] = false;
   ViewData["IsDetail"] = false;
   ViewData["Scope"] = "Admin";
%>

<% using (Ajax.BeginForm("Configuration", null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = "Configuration",
               OnBegin = "function(){ showLoadingDialogForSection('#ConfigurationsContent'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('#ConfigurationsContent'); }",
               OnSuccess = "function(){ executePostActionsConfigurationsTab('#" + formId + "', '" + ViewData["Scope"] + "', 'Configuration', " + ViewData["IsDetail"].ToString().ToLower() + ");showMessageResult();}",               
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
   <style type="text/css">
    .buttns
    {
     color :#959595;
    }
    .validationError 
    {
        background-color: #FFFFFF;
        border: 1px solid red;
        padding: 1px;
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
    .buttondisabled:hover
    {
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
    .divConfig
    {
        border: 1px solid #000000;
        margin-left: 10px;
        overflow: auto;
        padding-bottom: 13px;
        padding-top: 13px;
        float:left;
        width:85%;
    }
    .labelPassword
    {
        color:Red;
        display:block !important;
    }
    </style>
     <%=Html.Hidden("U","", new  { id = "vrfMail"})%>
     <%= Html.Hidden("OperationStatus")%>
<%--<div>
    <div class="line">
        <div class="field">
            <label for="ConfigurationSendEmailToNews" style="font-size: small">
                <asp:Literal ID="ConfigurationSendEmailToNews" runat="server" Text="<%$ Resources:LabelResource, ConfigurationSendEmailToNews %>" />:</label>
            <%= Html.CheckBox("SendEmailToNews", Convert.ToBoolean(ViewData["SendEmailToNewsCheck"]), new { Style = "height:12px" })%>
        </div>
    </div>
</div>--%>

<input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', 'EMAIL','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','Admin:Configurations:Email');" style="float: right;margin-right: 5px;margin-top:5px"/>
<div class="divConfig" style="margin-top:5px;">
    <div class="line">
        <label id="TitleConfigInboxMail" style="margin-left: 10px;">
            Inbox Mail Configuration</label>
    </div>
    <%= Html.Hidden("Scope")%>
    <%= Html.Hidden("Container")%>
    <div class="line">
        <div class="field">
            <label for="InboxEmail">
                <asp:Literal ID="InboxEmail" runat="server" Text="<%$ Resources:LabelResource, UserEmail %>" />:</label>
            <%= Html.TextBox("InboxEmail", ViewData["InboxEmail"].ToString(), new { @class = "inputIsRequired", onblur = "inspectAllInputFields()", onChange = "javascript: trim(this);inputChange();" })%>
            <%= Html.ValidationMessage("InboxEmail", "*")%>
            <label id="messageEmail" style="display:none">Email is not valid</label>
        </div>
        <div class="field">
            <label for="Password">
                <asp:Literal ID="InboxPassword" runat="server" Text="<%$ Resources:LabelResource, UserNewPassword %>" />:</label>
            <%= Html.Password("Kennwort", ViewData["Kennwort"].ToString(), new { @class = "inputIsRequired", onblur = "inspectAllInputFields()", onChange = "javascript: trim(this);inputChange();", autocomplete = "off"  })%>
            <%= Html.ValidationMessage("Password", "*")%>
        </div>
        <div class="field">
            <label for="RePassword">
                <asp:Literal ID="InboxRePassword" runat="server" Text="<%$ Resources:LabelResource, UserRePassword %>" />:</label>
            <%= Html.Password("ReKennwort", ViewData["Kennwort"].ToString(), new { @class = "inputIsRequired", onblur = "inspectAllInputFields()", onChange = "javascript: trim(this);validatePassword();inputChange();", autocomplete = "off" })%>
            <%= Html.ValidationMessage("RePassword", "*")%>  
            <label id="showPassword" style="display:none">These passwords don't match</label>         
        </div>               
    </div>
    <div class="line">
        <div class="field">
            <label for="PopPort">
                Port:</label>
            <%= Html.TextBox("PopPort", ViewData["PopPort"].ToString(), new { @class = "inputIsRequired", onblur = "inspectAllInputFields()", onChange = "javascript: trim(this);inputChange();" })%>
            <%= Html.ValidationMessage("PopPort", "*")%>
        </div>
        <div class="field">
            <label for="PopServer">
                Server:</label>
            <%= Html.TextBox("PopServer", ViewData["PopServer"].ToString(), new { @class = "inputIsRequired", onblur = "inspectAllInputFields()", onChange = "javascript: trim(this);inputChange();" })%>
            <%= Html.ValidationMessage("PopServer", "*")%>
        </div>
        <div class="field" style="width: 113px">
            <label for="TypeServer">
                Server Type:</label>
            <div>
                <label for="PopType">
                    POP:</label>
                <%= Html.RadioButton("TypeServer", "Pop", (bool)ViewData["TypeServerPop"], new {name = "Pop", style = "vertical-align:bottom;", onblur = "setValueServer('1')", onChange = "inputChange();" })%>
                <label for="ImapType">
                    IMAP:</label>
                <%= Html.RadioButton("TypeServer", "Imap", (bool)ViewData["TypeServerImap"], new { style = "vertical-align:bottom;", onblur = "setValueServer('2')", onChange = "inputChange();" })%>
            </div>
        </div>
        <div class="field" style="padding-top: 22px">
            <div style="display: table-cell; vertical-align: middle; height: 18px; float: left;
                margin-right: 3px">
                <label for="PopRequireSsl">
                    Require Ssl:</label></div>
            <div style="float:left; width: 12px;">
                <%= Html.CheckBox("PopRequireSsl", Convert.ToBoolean(ViewData["PopRequireSsl"]), new { Style = "height:15px", onChange = "inputChange();" })%></div>
        </div>
    </div>    
      <div class="line">
                <div class="field" style="width:54%;">
                <div style="float:left">
                    <input class="buttns" disabled="" style="width:124px;height:15px"value="Test Configuration"  id="BtnTesting"   onclick="javascript:TestConfigtions('<%= Url.Action("TestConfig", "Configuration") %>');"/>
                 </div>   
                 <div style="float:right">
                    <input type="button" value="Remove Configuration" onclick="javascript:removeConfiguration();"/>
                </div>
                </div>
               
            </div> 
            </div>
    <div class="line">
        <div class="field">
            <input class="shortButton" type="submit" id="buttonSaveConfiguration" value="Save" />
        </div>
    </div>
    
    <div id="Instructions" style=" float:left; width:600px; margin-top : 20px; margin-left : 120px; border: 1px solid #000000;overflow: auto;">
                <label style= " color:#444444;margin-left:10px; margin-top : 10px; margin-bottom: 10px; display: block;font-weight: lighter; font-size: 19px;">Configure details for an e-mail drop box on this screen. E-mail that is received by this address will be imported into Compelligence and associated with competitors and products.</label>                
    </div>   
    
    
    <% } %>
