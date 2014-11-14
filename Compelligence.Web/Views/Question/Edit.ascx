<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Question>" %>
<% string formId = ViewData["Scope"].ToString() + "QuestionEditForm"; %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>
<script type="text/javascript">
    var ResizeHeightForm = function() {
        var div = document.getElementById('ValidationSummaryQuestion');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#QuestionEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
        $('#QMessage').hide();
    };
    var hiddenDetailFieldsYN = function() {
        $('#divlineboxpermanent').show();
        $('#divLineResponse1').show();
        $('#divLineResponse2').show();
        $('#divLineResponse3').hide();
        $('#divLineResponse4').hide();
        $('#divLineResponse5').hide();

    };
    var visible = function() {
        var selectedValue = $('#<%= formId %>Type > option:selected').prop('value');
        if (selectedValue != '') {
            if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.QuestionType.YesorNot %>') {
                $('#divlineboxpermanent').show();
                hiddenDetailFieldsYN();
            }
            else if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.QuestionType.MultipleChoice %>') {
                $('#divlineboxpermanent').show();
                $("#btnAddDivLine").show();
            }
            else if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.QuestionType.Competitors %>') {
                $('#divlineboxpermanent').show();
            }
            else { }
        }
    };
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        visible();
        ResizeHeightForm();
    });

    var cleanDeatilFields = function() {
        var j = 0; for (j = 0; j <= 5; j++) {
            $('#<%= formId %> input[type=text][name=ResponseValue' + j + ']').prop("value", "");
            $('#<%= formId %> input[type=text][name=ResponseText' + j + ']').prop("value", "");
        }
    };

    var showDetailFields = function() {
    $('#divlineboxpermanent').show();
        $('#divLineResponse1').show();
        $('#divLineResponse2').show();
        $('#divLineResponse3').show();
        $('#divLineResponse4').show();
        $('#divLineResponse5').show();
    };

    var hiddenDetailFields = function() {
        $('#divLineResponse1').hide();
        $('#divLineResponse2').hide();
        $('#divLineResponse3').hide();
        $('#divLineResponse4').hide();
        $('#divLineResponse5').hide();
        cleanDeatilFields();
    };
    
    var cleanFieldsMultipleChoice = function() {
        $('#Counter').val('0');
        $("#btnAddDivLine").hide();  
        $('#divlinebox').empty();
    };

    var checkResponseFields = function() {
        var selectedValue = $('#<%= formId %> [name=Type] > option:selected').prop('value');
        if (selectedValue != '') {
            if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.QuestionType.WinningCompetitor %>') {
                //                alert('1');
                var i = 0; for (m = 0; m <= 5; m++) {
                    $('#<%= formId %> input[type=text][name=ResponseValue' + m + ']').prop("disabled", "disabled");
                    $('#<%= formId %> input[type=text][name=ResponseText' + m + ']').prop("disabled", "disabled");
                }
                $('#QMessage').show();
                cleanFieldsMultipleChoice();
                hiddenDetailFields();

            }
            else if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.QuestionType.OpenText %>') {
                //                alert('1'); 
                var i = 0; for (i = 0; i <= 5; i++) {
                    $('#<%= formId %> input[type=text][name=ResponseValue' + i + ']').prop("disabled", "disabled");
                    $('#<%= formId %> input[type=text][name=ResponseText' + i + ']').prop("disabled", "disabled");
                }
                $('#QMessage').hide();
                cleanFieldsMultipleChoice();
                hiddenDetailFields();
            }
            else if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.QuestionType.YesorNot %>') {
                cleanDeatilFields();
                $('#<%= formId %> input[type=text][name=ResponseValue1]').prop("value", '<%= QuestionTypeResponses.Y %>');
                $('#<%= formId %> input[type=text][name=ResponseText1]').prop("value", '<%= QuestionTypeResponses.Yes %>');
                $('#<%= formId %> input[type=text][name=ResponseValue2]').prop("value", '<%= QuestionTypeResponses.N %>');
                $('#<%= formId %> input[type=text][name=ResponseText2]').prop("value", '<%= QuestionTypeResponses.No %>');
                var j = 0; for (j = 0; j <= 5; j++) {
                    $('#<%= formId %> input[type=text][name=ResponseValue' + j + ']').prop("disabled", "disabled");
                    $('#<%= formId %> input[type=text][name=ResponseText' + j + ']').prop("disabled", "disabled");
                }
                $('#QMessage').hide();
                cleanFieldsMultipleChoice();
                hiddenDetailFieldsYN();
            }
            else if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.QuestionType.MultipleChoice %>') {
                showDetailFields();
                var k = 0; for (k = 0; k <= 5; k++) {
                    $('#<%= formId %> input[type=text][name=ResponseValue' + k + ']').removeAttr("disabled");
                    $('#<%= formId %> input[type=text][name=ResponseText' + k + ']').removeAttr("disabled");
                }
                $('#QMessage').hide();
                $("#btnAddDivLine").show();
                cleanDeatilFields();
            }
            else if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.QuestionType.Competitors %>') {
                showDetailFields();
                $('#<%= formId %> input[type=text][name=ResponseValue1]').prop("value", 'Ind');
                $('#<%= formId %> input[type=text][name=ResponseText1]').prop("value", 'Industry');
                $('#<%= formId %> input[type=text][name=ResponseValue2]').prop("value", 'Com');
                $('#<%= formId %> input[type=text][name=ResponseText2]').prop("value", 'Competitor');
                $('#<%= formId %> input[type=text][name=ResponseValue3]').prop("value", 'Pro');
                $('#<%= formId %> input[type=text][name=ResponseText3]').prop("value", 'Product');
                $('#<%= formId %> input[type=text][name=ResponseValue4]').prop("value", 'PI');
                $('#<%= formId %> input[type=text][name=ResponseText4]').prop("value", 'Primary Industry');
                $('#<%= formId %> input[type=text][name=ResponseValue5]').prop("value", 'PC');
                $('#<%= formId %> input[type=text][name=ResponseText5]').prop("value", 'Primary Competitor');

                var j = 0; for (l = 0; l <= 5; l++) {
                    $('#<%= formId %> input[type=text][name=ResponseValue' + l + ']').prop("disabled", "disabled");
                    $('#<%= formId %> input[type=text][name=ResponseText' + l + ']').prop("disabled", "disabled");
                }
                $('#QMessage').show();
                cleanFieldsMultipleChoice();
            } else {
                $('#divlineboxpermanent').hide();
            }
        }
        //$(formId + ' input[type=text][name=' + dateFormFields[i] + ']').datepicker();
    };

    var checkResponseFieldsSuccess = function() {
        var selectedValue = $('#<%= formId %> [name=Type] > option:selected').prop('value');
        if (selectedValue != '') {
            if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.QuestionType.WinningCompetitor %>') {
                //                alert('1');
                var i = 0; for (m = 0; m <= 5; m++) {
                    $('#<%= formId %> input[type=text][name=ResponseValue' + m + ']').prop("disabled", "disabled");
                    $('#<%= formId %> input[type=text][name=ResponseText' + m + ']').prop("disabled", "disabled");
                }
                $('#QMessage').show();
                cleanFieldsMultipleChoice();
                hiddenDetailFields();

            }
            else if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.QuestionType.OpenText %>') {
                //                alert('1'); 
                var i = 0; for (i = 0; i <= 5; i++) {
                    $('#<%= formId %> input[type=text][name=ResponseValue' + i + ']').prop("disabled", "disabled");
                    $('#<%= formId %> input[type=text][name=ResponseText' + i + ']').prop("disabled", "disabled");
                }
                $('#QMessage').hide();
                cleanFieldsMultipleChoice();
                hiddenDetailFields();
            }
            else if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.QuestionType.YesorNot %>') {
                cleanDeatilFields();
                $('#<%= formId %> input[type=text][name=ResponseValue1]').prop("value", '<%= QuestionTypeResponses.Y %>');
                $('#<%= formId %> input[type=text][name=ResponseText1]').prop("value", '<%= QuestionTypeResponses.Yes %>');
                $('#<%= formId %> input[type=text][name=ResponseValue2]').prop("value", '<%= QuestionTypeResponses.N %>');
                $('#<%= formId %> input[type=text][name=ResponseText2]').prop("value", '<%= QuestionTypeResponses.No %>');
                var j = 0; for (j = 0; j <= 5; j++) {
                    $('#<%= formId %> input[type=text][name=ResponseValue' + j + ']').prop("disabled", "disabled");
                    $('#<%= formId %> input[type=text][name=ResponseText' + j + ']').prop("disabled", "disabled");
                }
                $('#QMessage').hide();
                cleanFieldsMultipleChoice();
                hiddenDetailFieldsYN();
            }
            else if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.QuestionType.MultipleChoice %>') {
                showDetailFields();
                var k = 0; for (k = 0; k <= 5; k++) {
                    $('#<%= formId %> input[type=text][name=ResponseValue' + k + ']').removeAttr("disabled");
                    $('#<%= formId %> input[type=text][name=ResponseText' + k + ']').removeAttr("disabled");
                }
                $('#QMessage').hide();
                $("#btnAddDivLine").show();
                //cleanDeatilFields();
            }
            else if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.QuestionType.Competitors %>') {
                showDetailFields();
                $('#<%= formId %> input[type=text][name=ResponseValue1]').prop("value", 'Ind');
                $('#<%= formId %> input[type=text][name=ResponseText1]').prop("value", 'Industry');
                $('#<%= formId %> input[type=text][name=ResponseValue2]').prop("value", 'Com');
                $('#<%= formId %> input[type=text][name=ResponseText2]').prop("value", 'Competitor');
                $('#<%= formId %> input[type=text][name=ResponseValue3]').prop("value", 'Pro');
                $('#<%= formId %> input[type=text][name=ResponseText3]').prop("value", 'Product');
                $('#<%= formId %> input[type=text][name=ResponseValue4]').prop("value", 'PI');
                $('#<%= formId %> input[type=text][name=ResponseText4]').prop("value", 'Primary Industry');
                $('#<%= formId %> input[type=text][name=ResponseValue5]').prop("value", 'PC');
                $('#<%= formId %> input[type=text][name=ResponseText5]').prop("value", 'Primary Competitor');

                var j = 0; for (l = 0; l <= 5; l++) {
                    $('#<%= formId %> input[type=text][name=ResponseValue' + l + ']').prop("disabled", "disabled");
                    $('#<%= formId %> input[type=text][name=ResponseText' + l + ']').prop("disabled", "disabled");
                }
                $('#QMessage').show();
                cleanFieldsMultipleChoice();
            } else {
                $('#divlineboxpermanent').hide();
            }
        }
        //$(formId + ' input[type=text][name=' + dateFormFields[i] + ']').datepicker();
    };
    
    $(document).ready(function() {
        $('#btnAddDivLine').click(function() {
            AddNewDivLine($(this));
        });
    });

    var AddNewDivLine = function() {
        var id = 'divlinebox';
        var el = document.getElementById(id);
        var number = 5;
        var divLines = el.childNodes;
        var idOtherDiv = 'divlineboxpermanent';
        var divP = document.getElementById(idOtherDiv);
        var divLinesPerm = divP.childNodes;
        if (divLinesPerm != null && divLinesPerm != undefined && divLinesPerm.length > 0) {
            if (divLinesPerm.length > 11) {
                var t = divLinesPerm.length - 11;
                var q = t / 2;
                var intvalue = Math.round(q);
                number = intvalue + number;
            }
        }
        var formIdJ = '<%= ViewData["Scope"] %>QuestionEditForm';
        var divLines = el.childNodes;

        if (divLines != null && divLines != undefined && divLines.length > 0) {
            number = divLines.length + number;
        }
        $('#Counter').val(number);
        // DIV LINE
        var divLine = document.createElement("div");
        divLine.setAttribute('class', 'line');
        divLine.setAttribute('className', 'line');
        divLine.id = 'divline' + number;

        // FIRST DIV FIELD
        var firstDivLine = document.createElement("div");
        firstDivLine.setAttribute('class', 'field');
        firstDivLine.setAttribute('className', 'field');

        // FIRST LABEL
        var questionLabel = document.createElement("label");
        questionLabel.setAttribute('style', 'font-size:12px;');
        questionLabel.innerHTML = 'Response Value:';

        // FIRST TEXT
        var questionText = document.createElement('input');
        questionText.type = 'text';
        questionText.id = formIdJ + 'ResponseValue' + number;
        questionText.name = 'ResponseValue' + number;

        firstDivLine.appendChild(questionLabel);
        firstDivLine.appendChild(questionText);

        // SECOND DIV FIELD
        var secondDivLine = document.createElement("div");
        secondDivLine.setAttribute('class', 'field');
        secondDivLine.setAttribute('className', 'field');

        // SECOND LABEL
        var responseLabel = document.createElement("label");
        responseLabel.setAttribute('style', 'font-size:12px;');
        responseLabel.innerHTML = 'Response Text:';

        // FIRST TEXT
        var responseText = document.createElement('input');
        responseText.type = 'text';
        responseText.id = formIdJ + 'ResponseText' + number;
        responseText.name = 'ResponseText' + number;

        secondDivLine.appendChild(responseLabel);
        secondDivLine.appendChild(responseText);

        divLine.appendChild(firstDivLine);
        divLine.appendChild(secondDivLine);
        el.appendChild(divLine);

    };

    var reloadEdit = function() {
        checkResponseFieldsSuccess();
    };
</script>

<div id="ValidationSummaryQuestion">
    <%= Html.ValidationSummary()%>
</div>

<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "QuestionEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); ResizeHeightForm();reloadEdit(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'Question', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Question', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("Counter")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="QuestionEditFormInternalContent"  class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Type" onclick="habtextbox()"  class="required">
                        <asp:Literal ID="QuestionType" runat="server" Text="<%$ Resources:LabelResource, QuestionType %>" />:</label>
                                      
                    <%= Html.DropDownList("Type", (SelectList)ViewData["TypeList"], string.Empty, new { onchange = "javascript: checkResponseFields();" , id = formId + "Type" })%>
                    <%= Html.ValidationMessage("Type", "*")%>
                </div>
                
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>QuestionText" class="required">
                        <asp:Literal ID="QuestionText" runat="server" Text="<%$ Resources:LabelResource, QuestionText %>" />:</label>
                    <%= Html.TextArea("QuestionText", new { id = formId + "QuestionText" })%>
                    <%= Html.ValidationMessage("QuestionText", "*")%>
                </div>
            </div>
            <div id="QMessage" class="line"><div class="field"><label  class="required"> Only one question of this type can be added</label></div></div>
            <div id="divlineboxpermanent" style="display:none;">
            <% IList<QuestionDetail> list = (List<QuestionDetail>)ViewData["QuestionDetail"];
                   if (list != null && list.Count > 0)
                   {
                       for (int l = 0; l < list.Count; l++)
                       {%>
                
                <%=Html.QuestionOptionRow(formId, (l + 1), LabelResource.QuestionResponseValue, list[l].ResponseValue, LabelResource.QuestionResponseText, list[l].ResponseText)%>
                <% }
                   }%>
            </div>
            <div id="divlinebox">
            </div>
            <div class="field">
                <input type="button" id="btnAddDivLine" value="Add another response field" style="display:none;"/>
            </div>
        </div>
    </fieldset>
</div>
<% } %>