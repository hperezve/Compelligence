<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    function exportResultOnFile(formId, fieldId, hdnId) {
        var content = $('#' + fieldId).html().replace(/(<br>)|(<p><\/p>)/g, "?");
        content = $(content).text();
        $('#' + hdnId).val(" ");
        $('#' + hdnId).val(content);
        $('#' + formId).submit();
    };
    $(function() {
        $("#BtnDownload").prop("disabled", "true");
        $("#Button2").prop("disabled", "true");
        $("#CriteriaUploaderId").change(function(e) {
            var m = $(this);
            $("#TxtShowValue").prop("value", m.val());
        });
        $("#File1").change(function(e) {
            var m = $(this);
            $("#TxtShowValue2").prop("value", m.val());
        });
        $("#BtnBrowse").focus(function(e) {
            var ua = $.browser;
            if (ua.msie) {
                var t = $("#CriteriaUploaderId");
                var value = t.val();
                if (value != null && value != undefined) {
                    $("#TxtShowValue").prop("value", value);
                }
            }
        });
        $("#ajaxUploadForm").ajaxForm({
            iframe: true,
            dataType: "json",
            beforeSubmit: function() {
                $("#ajaxUploadForm").block({ message: '<h1> Uploading file...</h1>' });
            },
            success: function(result) {
                $("#ajaxUploadForm").unblock();
                // $("#ajaxUploadForm").resetForm();
                $("#UploadFormText").html(result.message);
                $("#UploadFormResults").css("display", "block");
                //$.growlUI(null, result.message);
            },
            error: function(xhr, textStatus, errorThrown) {
                $("#ajaxUploadForm").unblock();
                $("#ajaxUploadForm").resetForm();
                //$.growlUI(null, 'Error uploading file');
            }
        });

        $("#ajaxUploadFormPricing").ajaxForm({
            iframe: true,
            dataType: "json",
            beforeSubmit: function() {
            $("#ajaxUploadFormPricing").block({ message: '<h1> Uploading file...</h1>' });
            },
            success: function(result) {
            $("#ajaxUploadFormPricing").unblock();
                // $("#ajaxUploadForm").resetForm();
                $("#PriceTex").html(result.message);
                $("#UpdatedResul").css("display", "block");
                //$.growlUI(null, result.message);
            },
            error: function(xhr, textStatus, errorThrown) {
            $("#ajaxUploadFormPricing").unblock();
            $("#ajaxUploadFormPricing").resetForm();
                //$.growlUI(null, 'Error uploading file');
            }
        });
    });

    var hash = {
        '.xls': 1,
        '.xlsx': 1
    };

    var check_extension = function(filename, submitId, labelId) {
        var re = /\..+$/;
        var ext = filename.match(re);
        var submitEl = document.getElementById(submitId);
        var labelEl = document.getElementById(labelId);
        if (hash[ext]) {
            submitEl.disabled = false;
            labelEl.innerHTML = 'Ensure that all industries and products are already configured before uploading criteria.';
            labelEl.style.color = "blue";
            return true;
        } else {
            labelEl.innerHTML = 'Invalid filename, please select another file';
            labelEl.style.color = "red";
            submitEl.disabled = true;

            return false;
        }

    };

    var GetTemplateByIndustry = function(selelect) {
        var MySelect = $('#IndustryId');
        var MySelect1 = $('#chkValue');
        if (MySelect.val() != '' && MySelect.val() != null && MySelect.val() != undefined) {
            $('#BtnDownload').removeAttr("disabled");
            if (MySelect.val() == -1) {
                document.getElementById('chkValue').disabled = true;
            }
            else {
                document.getElementById('chkValue').disabled = false;
            }
        } else {
            $("#BtnDownload").prop("disabled", "disabled");
        }
    };
    var GetTemplateByIndustryPricing = function(selelect) {
    var MySelect = $('#IndustryIdPricing');
    var MySelect1 = $('#CheckboxIndustryPricing');
        if (MySelect.val() != '' && MySelect.val() != null && MySelect.val() != undefined) {
            $('#Button2').removeAttr("disabled");
            if (MySelect.val() == -1) {
                document.getElementById('CheckboxIndustryPricing').disabled = true;
            }
            else {
                document.getElementById('CheckboxIndustryPricing').disabled = false;
            }
        } else {
         $("#Button2").prop("disabled", "disabled");
        }
    };

    var donwloadTemplateExel = function(urlCheck, urlAction) {

        var xmlhttp;
        var results = null;
        var MySelect = $('#IndustryId').val();
        var MySelect1 = $('#chkValue:checked').val();
        var parametro = { IndustryId: MySelect, chkValue: MySelect1 };
        $("#ajaxUploadForm").block({ message: '<h1> Download a Template:...</h1>' });
        $.post(
            urlCheck,
            parametro,
            function(data) {
                if (data != null && data != '') {
                    results = data;
                    if (results != "") {
                        $("#ajaxUploadForm").unblock();
                        var newResult = results.replace('"', '');
                        newResult = newResult.replace('"', '');
                        if (newResult == 'Exist') {
                            $('#DownloadFileSection').prop("src", urlAction);
                        }
                        else {
                            var errorMessage = '<p>' + results + '</p>';
                            $('#AlertReturnMessageDialog').html(errorMessage);
                            $('#AlertReturnMessageDialog').dialog('open');
                        }
                    }
                }
            });
    };
    var UploadExelPricing = function(urlCheck, urlAction) {

        var xmlhttp;
        var results = null;
        var MySelect = $('#IndustryIdPricing');
        var MySelect1 = $('#CheckboxIndustryPricing:checked');
        var parametro = { IndustryId: MySelect.val(), chkValue: MySelect1.val() };
        $("#ajaxUploadFormPricing").block({ message: '<h1> Download a Template:...</h1>' });
        $.get(
            urlCheck,
            parametro,
            function(data) {
                if (data != null && data != '') {
                    results = data;                    
                }
            });        
    };
    var donwloadTemplateExelPricing = function(urlCheck, urlAction) {

        var xmlhttp;
        var results = null;
        var MySelect = $('#IndustryIdPricing');
        var MySelect1 = $('#CheckboxIndustryPricing:checked');
        var parametro = { IndustryId: MySelect.val(), chkValue: MySelect1.val() };
        $.get(
            urlCheck,
            parametro,
            function(data) {
                if (data != null && data != '') {
                    results = data;
                    if (results != "") {
                        $("#ajaxUploadFormPricing").unblock();
                        var newResult = results.replace('"', '');
                        newResult = newResult.replace('"', '');
                        if (newResult == 'Exist') {
                            $('#DownloadFileSection').prop("src", urlAction);
                        }
                        else {
                            var errorMessage = '<p>' + results + '</p>';
                            $('#AlertReturnMessageDialog').html(errorMessage);
                            $('#AlertReturnMessageDialog').dialog('open');
                        }
                    }
                }
            });            
        
    };
    var SetOnclick = function() {
        var buttonTypeFile = $("#CriteriaUploaderId");
        buttonTypeFile.click();
    };
    var SetOnclickprice = function() {
        var buttonTypeFile = $("#File1");
        buttonTypeFile.click();
    };
</script>
<input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', '<%= Compelligence.Domain.Entity.Resource.Pages.CriteriaUploader %>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>','<%= ViewData["ActionFrom"] %>','Tools:Criteria Uploader');" style="float: right;margin-right: 5px;margin-top:5px"/>

<div id="UploadDiv" style="height: 300px;">
    <form id="ajaxUploadForm" action="<%= Url.Action("AjaxUpload", "ComparinatorCriteriaUploader")%>"
    method="post" enctype="multipart/form-data">
    <fieldset>
        <div class="contentFormEdit" style="height: auto;">           
            <div class="line">
            <legend style="font:bold 1.3em 'Trebuchet MS',Tahoma">Feature Uploader</legend>
            <legend>Upload a file</legend>
            <div class="line">
                <div class="fieldUpload">
                    <input type="file" name="file" id="CriteriaUploaderId" size="23" onchange="javascript:check_extension(this.value,'ajaxUploadButton','validationText');"
                        style="display: none" />
                    <label>
                        <asp:Literal ID="UserFirstName" runat="server" Text="<%$ Resources:LabelResource, ComparinatorCriteriaUploaderFile %>" />:
                    </label>
                    <%= Html.TextBox("TxtShowValue", null, new { id =  "TxtShowValue" })%>
                    <input class="button" id="BtnBrowse" type="button" value="Browse" onclick='SetOnclick();' />
                     <input class="button"  id="ajaxUploadButton" type="submit" value="Submit" />
                </div>
            </div>
            <div class="field" style="width:800px">
              <legend style="padding-left: 0px;">Download a Template:</legend>
               <div class="line">
                    <label>Industry</label>
                    <%= Html.DropDownList("IndustryId", (SelectList)ViewData["CriteriaDowloap"], string.Empty, new { onchange="GetTemplateByIndustry(this)" })%>
                    <input class="checkbox" type="checkbox" id="chkValue" />  Include Values 
                    <input id="BtnDownload" class="button" type="button" value="Download" onclick="javascript:donwloadTemplateExel('<%= Url.Action("CheckExistFile", "ComparinatorCriteriaUploader") %>','<%= Url.Action("CheckOut", "ComparinatorCriteriaUploader") %>')"/>
                </div>
            </div>
            <div class="lineMessage">
                <label id="validationText">
                </label>
            </div>
            <div class="line">
                <div id="UploadFormResults" style="display: none;">
                    <legend>Results</legend>
                    <%--<label>Results</label>--%>
                    <div rows="3" cols="30" style="background-color:White;overflow-x:scroll;overflow-y:scroll;overflow:auto; border: 1px solid #6495C1;width: 750px;height: 150px;">
                    <p id="UploadFormText"></p></div>
                    <div style="margin-top: 5px;">
                        <input type="button" class="button" id="btnExpFileToCriteria" value="Export Result"  onclick="exportResultOnFile('frmExportFile','UploadFormText','hdnContent');" />
                    </div>
                </div>
            </div>
          </div>  
          </div>
    </fieldset>
    </form>
    <div class="line" style="border:1px solid"></div>
    <form id="ajaxUploadFormPricing" action="<%= Url.Action("AjaxUploadPricing", "ComparinatorCriteriaUploader")%>"
    method="post" enctype="multipart/form-data">
     <div class="line">
          <legend style="font:bold 1.3em 'Trebuchet MS',Tahoma">Pricing Uploader</legend>
          <legend>Upload a file</legend>
           <div class="line">
                <div class="fieldUpload">
                    <input type="file" name="file" id="File1" size="23"  
                        style="display: none" />
                        <div class="line">
                    <label>
                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:LabelResource, ComparinatorCriteriaUploaderFile %>" />:
                    </label></div>
                    <%= Html.TextBox("TxtShowValue2", null, new { id =  "TxtShowValue2" })%>
                    <input class="button" id="Button1" type="button" value="Browse" onclick='SetOnclickprice();' />
                     <input class="button"  id="Submit1" type="submit" value="Submit" />
                </div>
            </div>
            <div class="field" style="width:800px">
              <legend style="padding-left: 0px;">Download a Template:</legend>
               <div class="line">
                    <div class="line">
                    <label>Industry</label></div>
                    <%= Html.DropDownList("IndustryIdPricing", (SelectList)ViewData["CriteriaDowloap"], string.Empty, new { onchange="GetTemplateByIndustryPricing(this)" })%>
                    <input class="checkbox" type="checkbox" id="CheckboxIndustryPricing" />  Include Values 
                    <input id="Button2" class="button" type="button" value="Download" onclick="javascript:donwloadTemplateExelPricing('<%= Url.Action("CheckExistFilePricing", "ComparinatorCriteriaUploader") %>','<%= Url.Action("CheckOutPricing", "ComparinatorCriteriaUploader") %>')"/>
                </div>
            </div>
            <div class="lineMessage">
                <label id="Label1">
                </label>
            </div>
            <div class="line">
                <div id="UpdatedResul" style="display: none;margin-left: 10px;">
                    <legend>Results</legend>
                    <%--<label>Results</label>--%>
                    <div rows="3" cols="30" style="background-color:White;overflow-x:scroll;overflow-y:scroll;overflow:auto; border: 1px solid #6495C1;width: 750px;height: 150px;">
                    <p id="PriceTex"></p></div>
                    <div style="margin-top: 5px;">
                        <input type="button" class="button" id="btnExpFileToPricing" value="Export Result"  onclick="exportResultOnFile('frmExportFile','PriceTex','hdnContent');" />
                    </div>
                </div>
            </div>
            </div>
 </form>
  <div class="displayNone">
    <form id="frmExportFile" method="POST" action="<%= Url.Action("ExportTextFile", "ComparinatorCriteriaUploader")%>">
        <input type="hidden" name="hdnContent" id="hdnContent" value="" />
    </form>
 </div>
</div>
