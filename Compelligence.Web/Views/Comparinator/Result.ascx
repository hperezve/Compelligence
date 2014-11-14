<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%@ Import Namespace="Compelligence.DataTransfer.Comparinator" %>
<%@ Import Namespace="Compelligence.DataTransfer.FrontEnd" %>

<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="System.Globalization" %>
<%  IList<Product> Titles = (IList<Product>)ViewData["Products"];

    IList<Product> G1 = (IList<Product>)ViewData["G1"];
    IList<Product> G2 = (IList<Product>)ViewData["G2"];

    string c = (string)ViewData["C"];
    string u = (string)ViewData["U"];
    String ProductId = Titles[0].Id.ToString();
    String IndustryId = ViewData["IndustryId"].ToString();
    String CompetitorId = ViewData["CompetitorId"].ToString();
    ConfigurationDefaults configurationdefaults = (ConfigurationDefaults)ViewData["ConfigurationDefaults"];
    String DefaultsComparinatorExport = configurationdefaults.ComparinatorExport;
    String DefaultsEnableTools = configurationdefaults.EnableTools;
    String DefaultsIndustryStandars = configurationdefaults.IndustryStandars;
    String DefaultsSameValues = configurationdefaults.SameValues;
    String DefaultsBenefit = configurationdefaults.Benefit;
    String DefaultsCost = configurationdefaults.Cost;
    bool DefaultsIndustryInfoTab = Convert.ToBoolean(configurationdefaults.IndustryInfoTab);
    bool DefaultsInfoTab = Convert.ToBoolean(configurationdefaults.InfoTab);
    bool DefaultsSilverBulletsTab = Convert.ToBoolean(configurationdefaults.SilverBulletsTab);
    bool DefaultsPositioningTab = Convert.ToBoolean(configurationdefaults.PositioningTab);
    bool DefaultsPricingTab = Convert.ToBoolean(configurationdefaults.PricingTab);
    bool DefaultsFeaturesTab = Convert.ToBoolean(configurationdefaults.FeaturesTab);
    bool DefaultsSalesToolsTab = Convert.ToBoolean(configurationdefaults.SalesToolsTab);
    bool DefaultsDisabPublComm = Convert.ToBoolean(configurationdefaults.DisabledPublicComment);
    bool DefaultsNewsTab = Convert.ToBoolean(configurationdefaults.NewsTab);

    bool SomeTab = DefaultsIndustryInfoTab || DefaultsInfoTab || DefaultsSilverBulletsTab || DefaultsPositioningTab || DefaultsPricingTab || DefaultsFeaturesTab || DefaultsSalesToolsTab || DefaultsNewsTab; %>

<script type="text/javascript">
    function CallCellRelevancySave(urlSave, criteriaid, value) {

        $.get(urlSave, { cid: criteriaid, relevant: value }, function(data) {
            //Colorize corner
            var target = $("#C" + criteriaid).find("td .relevancy ");
            target.removeClass("comp_crbg");
            target.removeClass("comp_crbr");
            if (value == "HIGH") target.addClass("comp_crbg");
            if (value == "LOW") target.addClass("comp_crbr");
        });
    };

    /**/
    /**/

    var FormConfirmFilterBox = function(state, dialogContent) {
        var confirmObject = $("#ConfirmBox");
        confirmObject.empty();
        confirmObject.dialog({
            width: 250,
            heigth: 150,
            modal: true,
            buttons: {
                "Ok": function() {
                    //toprodsfeature(state);
                    resetfilter();
                    UpdatedOptionsToFilter();
                    ResetRelevancyFilter();
                    $(this).dialog('destroy');
                    toprodsfeature(state);
                    ResetDisplayValues();
                },
                "Cancel": function() {
                    if (state == 0) {
                        $('#chkboxselprods').prop('checked', true);
                        $('#chkboxallprods').prop('checked', false);
                    } else {
                        $('#chkboxselprods').prop('checked', false);
                        $('#chkboxallprods').prop('checked', true);
                    }
                    $(this).dialog('destroy');
                }
            }
        });
        confirmObject.dialog('option', 'title', 'Confirm Reset Filter');
        confirmObject.html(dialogContent);
        confirmObject.dialog('open');
    };

    var showConfirmFilterDialog = function(state) {
        var strCFDForm = "<form id='ConfirmFilterFormEdit'>";
        strCFDForm += "<label>All filters will be reset, do you want to continue?</label>";
        strCFDForm += "</form>";
        FormConfirmFilterBox(state, strCFDForm);
    };

    function updateDefaults() {
        if ("<%=DefaultsComparinatorExport%>" == "true") {
            $(".ExportTools").show();
            $(".comp_export").show();
        }
        else {
            $(".ExportTools").hide();
            $(".comp_export").hide();
            $('.ExportToolsWidht').css('width', '50%');
        }
        if ("<%=DefaultsIndustryStandars%>" == "true") {
            $("#chkindstandard").prop('checked', true);
            HideColumn(true, 2);
        }
        else {
            $("#chkindstandard").prop('checked', false);
            HideColumn(false, 2);
        }

        if ("<%=DefaultsEnableTools%>" == "true") {
            $("#chkenabletools").prop('checked', true);
            EnableTools(true);
        }
        else {
            $("#chkenabletools").prop('checked', false);
            EnableTools(false);
        }

        if ("<%=DefaultsSameValues%>" == "all") {
            $("#rallvalues").prop('checked', true);
            DisplaySameValues("all");
        } else if ("<%=DefaultsSameValues%>" == "same") {
            $("#ronlysamevalues").prop('checked', true);
            DisplaySameValues("same");
        } else if ("<%=DefaultsSameValues%>" == "diff") {
            $("#ronlydiff").prop('checked', true);
            DisplaySameValues("diff");
        }
        if ("<%=DefaultsBenefit%>" == "true") {
            $("#chkhidebenefit").prop('checked', true);
            HideBenefitColumn(true, $("#chkhidebenefit"));
        }
        else {
            $("#chkhidebenefit").prop('checked', false);
            HideBenefitColumn(false, $("#chkhidebenefit"));
        }
        if ("<%=DefaultsCost%>" == "true") {
            $("#chkhidecost").prop('checked', true);
            HideCostColumn(true, $("#chkhidecost"));
        }
        else {
            $("#chkhidecost").prop('checked', false);
            HideCostColumn(false, $("#chkhidecost"));
        }
    }  
</script>

<script type="text/javascript">
    var loadContentToNews = function(urlAction, pC, pU) {
        showLoadingDialog();
        $('#FormSalesTools').load(urlAction, { C: pC, U: pU }, function() { hideLoadingDialog(); });
    };

    var openPopupCenter = function(url) {
        var w = 800, h = 600;
        if (document.body && document.body.offsetWidth) {
            w = document.body.offsetWidth;
            h = document.body.offsetHeight;
        }
        if (document.compatMode == 'CSS1Compat' &&
    document.documentElement &&
    document.documentElement.offsetWidth) {
            w = document.documentElement.offsetWidth;
            h = document.documentElement.offsetHeight;
        }
        if (window.innerWidth && window.innerHeight) {
            w = window.innerWidth;
            h = window.innerHeight;
        }

        var pw = 700, ph = 300;

        var left = (w - pw) / 2, top = (h - ph) / 2;

        var popupWindow = window.open(url, 'PopupWindow', 'width=' + pw + ',height=' + ph + ',scrollbars=yes' + ',top=' + top + ',left=' + left);
        if (window.focus) {
            popupWindow.focus()
        }
    };

    function GetObject() {
        var numberColumn;
        var table = $('#comp_table_result');
        if (table.length > 0) {
            numberColumn = GetNumberOfColumn(table);
        }
        return numberColumn;
    }

    function DisplayFirstRowFeature() {
        $('.comp_table').find('.comp_head').find('.comp_thv').removeClass('comp_thv'); //to hidden the current row with filter
        var headerRows = $('.comp_table').find('.comp_head');
        if (headerRows != null && headerRows != undefined) {
            if (headerRows.length > 0) {
                $('.comp_table:not(.comp_hiddentable):first').find('.comp_head').find('.comp_thc').addClass("comp_thv"); //show first
            }
        }
    }



    //lines for add new row
    var showNewCriteriaDialog = function(source, groupname, groupid, setname, setid, urlaction, u, c) {


        $("#comp_new").dialog({
            bgiframe: true,
            title: 'New Criteria',
            //resizable: false, //removed beacuase generate big title
            height: 340,
            width: 320,
            autoOpen: false,
            modal: true,
            overlay: {
                backgroundColor: '#000',
                opacity: 0.5
            },
            buttons: {
                'Save': function() {
                    var groupid = $(this).dialog('option', 'groupid');
                    var setid = $(this).dialog('option', 'setid');
                    var urlaction = $(this).dialog('option', 'urlaction');
                    var source = $(this).dialog('option', 'source');

                    var indstandard = $(this).dialog('option', 'indstandard');
                    var benefit = $(this).dialog('option', 'benefit');


                    var name = $("#comp_new").find("#name").val();
                    name = name.trim();
                    var benefitValue = $("#comp_new").find("#benefitValue").val();
                    benefitValue = benefitValue.trim();
                    var relevant = $("#comp_new").find("#relevant_on").prop("checked");
                    if (relevant == true) relevant = "Y";
                    if (relevant == false) relevant = "N";

                    var Type = $("#comp_new").find("#Type").val();
                    var Relevancy = $("#comp_new").find("#Relevancy").val();
                    var SelMostDesiredValue = $("#comp_new").find("#SelMostDesiredValue").val();

                    var pis = $("#chkindstandard").prop("checked");
                    var pb = $("#chkhidebenefit").prop("checked");
                    var pc = $("#chkhidecost").prop("checked");

                    if (name.length > 3) {
                        var valuesTruncated = false;
                        var messageToValuesTruncated = '';
                        if (name.length > 100) {
                            valuesTruncated = true;
                            name = name.substring(0, 100);
                            messageToValuesTruncated = "The criteria name entered is longer than 100 characters and has been truncated.";
                        }
                        if (benefitValue.length > 200) {
                            valuesTruncated = true;
                            benefitValue = benefitValue.substring(0, 200); //get the first 200 characters
                            if (messageToValuesTruncated != '') messageToValuesTruncated = messageToValuesTruncated + "<br />";
                            messageToValuesTruncated = "The Benefit entered is longer than 200 characters and has been truncated.";
                        }
                        if (valuesTruncated) {
                            var messageDialog = $('#AlertMessageDialog');
                            messageDialog.dialog({ title: "Alert Message", buttons: { 'Ok': function() { $(this).dialog('close'); } } });
                            messageDialog.html(messageToValuesTruncated);
                            messageDialog.dialog("open");
                        }
                        $.get(urlaction, { groupid: groupid, setid: setid, name: name, relevancy: Relevancy, indstandard: indstandard, benefit: benefit, benefitValue: benefitValue, type: Type, mostDesiredValue: SelMostDesiredValue, pis: pis, pb: pb, pc: pc, U: u, C: c }, function(data) {
                            //show
                            $('#tbl' + setid).append(data);
                            //next line enable tooptip
                            SetIndustryStandarCorner('#tbl' + setid + ' tr:last div.indstd', '<%=ViewData["IndustryId"] %>');
                            SetValueCorner('#tbl' + setid + ' tr:last div.cp', '<%=Url.Action("CellDetail", "Comparinator") %>', '<%=ViewData["IndustryId"] %>', '<%=ViewData["U"] %>', '<%=ViewData["C"] %>', '<%=DefaultsDisabPublComm%>');
                            SetRelevancyCorner('#tbl' + setid + ' tr:last div.relevancy', '<%=Url.Action("CellRelevancy", "Comparinator") %>');
                            SetBenefitCorner('#tbl' + setid + ' tr:last div.benefit', '<%=ViewData["IndustryId"] %>'); //enable benefit corner
                            SetCostCorner('#tbl' + setid + ' tr:last div.cost', '<%=ViewData["IndustryId"] %>'); //enable cost corner

                        });
                    }
                    $(this).dialog('close');
                },
                Cancel: function() {
                    $(this).dialog('close');
                }
            }
        });

        $("#comp_new").dialog('option', 'groupname', groupname);
        $("#comp_new").dialog('option', 'groupid', groupid);
        $("#comp_new").dialog('option', 'setname', setname);
        $("#comp_new").dialog('option', 'setid', setid);
        $("#comp_new").dialog('option', 'urlaction', urlaction);
        $("#comp_new").dialog('option', 'source', source);

        $("#comp_new").find("#groupname").text(groupname);
        $("#comp_new").find("#setname").text(setname);
        $("#comp_new").find("#name").val("");
        $("#comp_new").find("#benefitValue").val("");
        var indstandard = $("#chkindstandard").prop("checked");


        $("#comp_new").dialog('option', 'indstandard', indstandard);

        var benefit = $("#chkhidebenefit").prop("checked");
        $("#comp_new").dialog('option', 'benefit', benefit);

        $("#comp_new").dialog('open');
    };

    function togglePrevent(imageprefix, source, criteriaid) {
        if (source.getAttribute("state") == 'on') {
            source.src = imageprefix + 'off.png';
            source.setAttribute('state', 'off');
            source.setAttribute('title', 'Relevant');
        }
        else {
            source.src = imageprefix + 'on.png';
            source.setAttribute('state', 'on');
            source.setAttribute('title', 'Prevent');
        }

        $.get('<%= Url.Action("UpdateCriteria", "Comparinator") %>', { criteriaid: criteriaid, relevant: source.getAttribute("state") }, function(data) {
            if (source.getAttribute("state") == 'on')
                $(source).parent().parent().prop('Prevent', 'Y');
            else
                $(source).parent().parent().prop('relevant', 'N');
        });

    }

    function togglePreventDynamic(imageprefix, source, criteriaid) {
        if (source.getAttribute("state") == 'h') {
            source.src = imageprefix + 'd.png';
            source.setAttribute('state', 'm');
            source.setAttribute('title', 'Medium');
        }
        else if (source.getAttribute("state") == 'm') {
            source.src = imageprefix + 'r.png';
            source.setAttribute('state', 'l');
            source.setAttribute('title', 'Low');
        }
        else {
            source.src = imageprefix + 'g.png';
            source.setAttribute('state', 'h');
            source.setAttribute('title', 'High');
        }

        $.get('<%= Url.Action("UpdateCriteria", "Comparinator") %>', { criteriaid: criteriaid, relevant: source.getAttribute("state") }, function(data) {
            if (source.getAttribute("state") == 'h') {
                $(source).parent().parent().prop('relevant', 'HIGH');
            }
            else if (source.getAttribute("state") == 'm') {
                $(source).parent().parent().prop('relevant', 'MEDI');
            }
            else {
                $(source).parent().parent().prop('relevant', 'LOW');
            }
        });
    };

    function radioPreventDynamic(imageprefix, source, criteriaid, value, imgobject) {
        $(source).prop('checked', true);
        // $(source).attr('checked', 'checked');
        // source.attr('checked', 'checked');
        //var $radios = $(source).parent()
        // GetTest(source, value);
        //setTimeout('SetValueCheked('+value+','+ criteriaid+');', 100); 

        if (value == 'HIGH') {
            $('#' + imgobject).prop('src', imageprefix + 'g.png');
            $('#' + imgobject).prop('state', 'h');
        }
        else if (value == 'MEDI') {
            $('#' + imgobject).prop('src', imageprefix + 'd.png');
            $('#' + imgobject).prop('state', 'm');
        }
        else {
            $('#' + imgobject).prop('src', imageprefix + 'r.png');
            $('#' + imgobject).prop('state', 'l');
        }

        $.get('<%= Url.Action("UpdateCriteria", "Comparinator") %>', { criteriaid: criteriaid, relevant: value }, function(data) {
            if (value == 'HIGH') {
                $('#' + imgobject).parent().parent().parent().parent().parent().parent().prop('relevant', 'HIGH');
            }
            else if (value == 'MEDI') {
                $('#' + imgobject).parent().parent().parent().parent().parent().parent().prop('relevant', 'MEDI');
            }
            else {
                $('#' + imgobject).parent().parent().parent().parent().parent().parent().prop('relevant', 'LOW');
            }

        });
    };

    function GetValueByKey(value, key) {
        var result = '';
        var pos = value.indexOf(key);
        if (pos != -1) {
            var subValu = value.substring(pos + key.length + 1);
            var tokenPos = subValu.indexOf("[T+K]");
            if (tokenPos != -1) {
                result = subValu.substring(0, tokenPos);
            }
            else {
                result = subValu;
            }
        }
        return result;
    };


    function GetNewsOfCompetitorByProductSelected(select, selected, urlAction, target) {
        var MySelect = $('#CompetitorIdByProduct');
        var MyCompanyId = '<%= ViewData["C"] %>';
        if (MySelect.val() != null && MySelect.val() != '' && MySelect.val() != undefined) {
            var parameters = { CompetitorId: MySelect.val(), EntityType: 'COMPT', asc: 'DESC', C: MyCompanyId };
            $.get(urlAction, parameters, function(data) {
                $('#' + target).html(data);
            });
        }
    };

    function GetNewsOfCompetitorByProduct(urlAction, target) {
        var MySelect = $('#CompetitorIdByProduct');
        if (MySelect.val() != null && MySelect.val() != '' && MySelect.val() != undefined) {
            //var parameters = { CompetitorId: MySelect.val(), EntityType: 'COMPT' };
            //$.get(urlAction, parameters, function(data) {
            $.get(urlAction, function(data) {
                $('#' + target).html(data);
            });
        }
    };
    function GetNewsOfCompetitorByDefault(competitorId, urlAction, target) {
        var MyCompanyId = '<%= ViewData["C"] %>';
        if (competitorId != null && competitorId != '' && competitorId != undefined) {
            var parameters = { CompetitorId: competitorId, EntityType: 'COMPT', asc: 'DESC', C: MyCompanyId };
            $.get(urlAction, parameters, function(data) {
                $('#' + target).html(data);
            });
        }
    };


    function OpenCommentDlg() {
        var c = '<%= c%>';
        var u = '<%= u%>';
        var MySelect = $('#ProductList');
        var ProductId = MySelect.val();

        //?EntityId=19071721096825980&ForumResponseId=0&ObjectType=PRODT
        ExternalCommentsDlg('<%= Url.Action("GetComments","Forum")%>?EntityId=' + ProductId + '&ObjectType=PRODT' + '&C=' + c + '&U=' + u, '<%= Url.Action("ExternalResponse","Forum")%>?EntityId=' + ProductId + '&ForumResponseId=0&ObjectType=PRODT&C=' + c + '&U=' + u);
        // ExternalCommentsDlg('<%= Url.Action("GetComments","Forum", new { EntityId = ProductId,ObjectType=DomainObjectType.Product,C=c,U=u })%>', 'Comment Form', '<%=Url.Action("ExternalResponse", "Forum",new {EntityId=ProductId,ForumResponseId=0,ObjectType=DomainObjectType.Product, C=c, U=u},null)%>', ProductId);
    }

    function OpenFeedBackDlg() {
        var c = '<%= c%>';
        var u = '<%= u%>';
        var MySelect = $('#ProductList');
        var ProductId = MySelect.val();

        ExternalFeedBackWithAttachedDlg('<%= Url.Action("FeedBackMessage", "Forum")%>?EntityId=' + ProductId + '&EntityType=PRODT&U=' + u + '&SubmittedVia=Positioning', 'FeedBack Dialog')
    }


    function ChangeImageSRC(select, selected) {

        var MySelect = $('#ProductList');
        var parametro = { ProductId: MySelect.val() };
        var idComments = 'ImgComents' + MySelect.val();
        var xmlhttp;
        $.get(
            '<%= Url.Action("GetImageUrl", "Comparinator") %>',
            parametro,
            function(data) {
                $('#imgddl').prop('src', data.Path);
                $('#CCP img').removeAttr('class');
                $('#CCP img').attr("id", idComments);
                $('#' + idComments).addClass(data.HasComment);
            });
    }
    var UpdateRowsByConfig = function() {
        //Need Complete
    };


    function FormClientPositioningBox(object, dialogTitle, urlAction, dialogContent, MyCompanyId, MyUserId) {
        var positioningObject = $("#PositioningBox");
        positioningObject.empty();
        positioningObject.dialog({
            width: 575,
            modal: true,
            buttons: {
                "Ok": function() {
                    var hdnIndustryId = $('#CompanyPositionigDialogFormEdit input[name=hdnIndustryId]').val();
                    var hdnProductId = $('#CompanyPositionigDialogFormEdit input[name=hdnProductId]').val();
                    var hdnPositioningRelation = $('#CompanyPositionigDialogFormEdit input[name=hdnPositioningRelation]').val();
                    var hdnAction = $('#CompanyPositionigDialogFormEdit input[name=hdnAction]').val();
                    var hdnPositioningId = $('#CompanyPositionigDialogFormEdit input[name=hdnPositioningId]').val();
                    var hcnIsCompany = $('#CompanyPositionigDialogFormEdit input[name=hcnIsCompany]').val();
                    var statementField = $('#CompanyPositionigDialogFormEdit input[name=txtStatment]').val();
                    statementField = convertTextPlainHtml(statementField);
                    var howWePositionField = $('#CompanyPositionigDialogFormEdit textarea[name=TxtHowWePosition]').val();
                    var lblValidSum = $('#LblValidationSumary');
                    var isValidTheForm = true;
                    var messageOfValidation = '';
                    var hwpEncode = htmlEncode(howWePositionField);
                    var parameters = { hdnIndustryId: hdnIndustryId, hdnCompetitorId: '', hdnProductId: hdnProductId,
                        hdnPositioningRelation: hdnPositioningRelation, hdnAction: hdnAction, hdnPositioningId: hdnPositioningId, txtStatment: statementField,
                        TxtHowWePosition: hwpEncode,
                        hdnC: MyCompanyId, hdnU: MyUserId, hcnIsCompany: hcnIsCompany
                    };
                    if (isEmpty(statementField) || statementField.length > 255 || isEmpty(howWePositionField) || howWePositionField.length>8000) {
                        isValidTheForm = false;
                    }
                    if (!isValidTheForm) {
                        if (isEmpty(statementField)) {
                            messageOfValidation += 'Statement is required<br/>';
                        }
                        if (statementField.length > 255) {
                            messageOfValidation += 'Statement has more than 255 characters<br/>';
                        }
                        if (isEmpty(howWePositionField)) {
                            messageOfValidation += 'How We Position is required<br/>';
                        }
                        if (howWePositionField.length > 8000) {
                            messageOfValidation += 'Fields have a limit of 8000 characters including HTML characters which may not be visible<br/>';
                        }
                        lblValidSum.prop('innerHTML', messageOfValidation);
                        lblValidSum.css("color", "red");
                    }
                    else {
                        var divHWPToChange = $('#hwpBoxDataList');
                        divHWPToChange[0].innerHTML = howWePositionField;
                        $.post(urlAction, parameters, function(data) {
                            var positioningStatment = $('#HdnClPPositioningStatement');
                            positioningStatment[0].value = statementField;
                            var positioningIdValue = $('#HdnClPPositioningId');
                            if (positioningIdValue != null && positioningIdValue != undefined) {
                                if (positioningIdValue[0] != null && positioningIdValue[0] != undefined) {
                                    positioningIdValue[0].value = data;
                                } else {
                                    positioningIdValue.value = data;
                                }
                            }

                            var positioningAction = $('#HdnClPPositioningAction');
                            if (positioningAction.val() == 'Create') {
                                positioningAction[0].value = 'Update';
                            }
                        });
                        $(this).dialog('destroy');
                    }
                },
                "Close": function() {
                    $(this).dialog('destroy');
                }
            }
        });
        positioningObject.dialog('option', 'title', dialogTitle);
        positioningObject.html(dialogContent);
        positioningObject.dialog("open");
        $('.txtBoxHtml').cleditor({ height: 150, width: 545 });
    };
    function AddCompanyPositioningDialog(object, Title, urlAction, urlActionC, urlActionU, IndustryId, positioningRelation, MyCompanyId, MyUserId) {
        var divHWPToChange = $('#hwpBoxDataList').html();
        var hdnPositioningId = $('#HdnClPPositioningId').val();
        var hdnPositioningAction = $('#HdnClPPositioningAction').val();
        var MySelect = $('#ProductList');
        var urlActionTo = urlActionC;
        var statementValue = $('#HdnClPPositioningStatement').val;
        if (hdnPositioningAction == 'Update') {
            urlActionTo = urlActionU;
            var parametro = { PositioningId: hdnPositioningId };
            var xmlhttp;
            var results = null;
            $.get(
            '<%= Url.Action("GetPositioningById", "Comparinator") %>',
            parametro,
            function(data) {
                if (data != null && data != '') {
                    results = data;
                    if (results != "") {
                        statementValue = GetValueByKeyAndToken(results, 'PositioningStatment_' + hdnPositioningId, '[TK' + hdnPositioningId + ']');
                        divHWPToChange = GetValueByKeyAndToken(results, 'PositioningHowWePosition_' + hdnPositioningId, '[TK' + hdnPositioningId + ']');
                        var divHWPTempo = $('#hwpBoxDataList');
                        divHWPTempo[0].innerHTML = divHWPToChange;
                        var hdnStatement = $('#HdnClPPositioningStatement');
                        hdnStatement[0].value = statementValue;
                    }
                }
            });
        }
        var hdnStatementPositioning = $('#HdnClPPositioningStatement').val();
        divHWPToChange = $('#hwpBoxDataList').html();
        var strCMPDForm = "<div id='CompanyPositionigDialogFormEdit'>";
        strCMPDForm += "<input type='hidden' name='hdnIndustryId' value='" + IndustryId + "' />";
        strCMPDForm += "<input type='hidden' name='hdnProductId' value='" + MySelect.val() + "' />";
        strCMPDForm += "<input type='hidden' name='hdnPositioningRelation' value='" + positioningRelation + "' />";
        strCMPDForm += "<input type='hidden' name='hdnPositioningId' value='" + hdnPositioningId + "' />";
        strCMPDForm += "<input type='hidden' name='hdnAction' value='" + urlActionTo + "' />";
        strCMPDForm += "<input type='hidden' name='hdnC' value='" + MyCompanyId + "' />";
        strCMPDForm += "<input type='hidden' name='hdnU' value='" + MyUserId + "' />";
        strCMPDForm += "<input type='hidden' name='hcnIsCompany' value='Y' />";
        strCMPDForm += "<label id='LblValidationSumary'></label>";
        strCMPDForm += "<label>Statement Name:</label><br /><input type='text' name='txtStatment' ";
        if (hdnPositioningAction == 'Update') {
            strCMPDForm += " value='" + hdnStatementPositioning + "' ";
        }
        strCMPDForm += "/><br />";
        strCMPDForm += "<label>How We Position:</label><br /><textarea name='TxtHowWePosition' WRAP=SOFT COLS=50 ROWS=4 class='txtBoxHtml'>";
        if (hdnPositioningAction == 'Update') {
            strCMPDForm += divHWPToChange;
        }
        strCMPDForm += "</textarea><br />";
        strCMPDForm += "</div>";
        FormClientPositioningBox(object, Title, urlActionTo, strCMPDForm, MyCompanyId, MyUserId);
    };


    var FormCompetitorPositioningBox = function(objectttt, dialogTitle, urlAction, action, dialogContent, productId, competitorId, industryId, hdnPositioningIdVa, MyCompanyId, MyUserId) {
        var positioningObject = $("#PositioningBox");
        positioningObject.empty();
        positioningObject.dialog({
            width: 575, //if change the width on FrontEndSite.css
            modal: true,
            buttons: {
                "Ok": function() {
                    var hdnIndustryId = $('#CompetitorPositionigFormEdit input[name=hdnIndustryId]').val();
                    var hdnCompetitorId = $('#CompetitorPositionigFormEdit input[name=hdnCompetitorId]').val();
                    var hdnProductId = $('#CompetitorPositionigFormEdit input[name=hdnProductId]').val();
                    var hdnPositioningRelation = $('#CompetitorPositionigFormEdit input[name=hdnPositioningRelation]').val();
                    var hdnAction = $('#CompetitorPositionigFormEdit input[name=hdnAction]').val();
                    var hdnPositioningId = $('#CompetitorPositionigFormEdit input[name=hdnPositioningId]').val();
                    var hcnIsCompany = $('#CompetitorPositionigFormEdit input[name=hcnIsCompany]').val();
                    var statementField = $('#CompetitorPositionigFormEdit input[name=txtStatment]').val();
                    statementField = convertTextPlainHtml(statementField);
                    var howTheyPositionField = $('#CompetitorPositionigFormEdit textarea[name=TxtHowTheyPosition]').val();
                    var howWeAttackField = $('#CompetitorPositionigFormEdit textarea[name=TxtHowWeAttack]').val();
                    var lblValidSum = $('#LblValidationSumary');
                    var isValidTheForm = true;
                    var messageOfValidation = '';
                    var htpEncode = htmlEncode(howTheyPositionField);
                    var hwaEncode = htmlEncode(howWeAttackField);
                    var parameters = { hdnIndustryId: hdnIndustryId, hdnCompetitorId: hdnCompetitorId, hdnProductId: hdnProductId,
                        hdnPositioningRelation: hdnPositioningRelation, hdnAction: hdnAction, hdnPositioningId: hdnPositioningId, txtStatment: statementField,
                        TxtHowTheyPosition: htpEncode, TxtHowWeAttack: hwaEncode,
                        hdnC: MyCompanyId, hdnU: MyUserId, hcnIsCompany: hcnIsCompany
                    };

                    if (isEmpty(statementField) || statementField.length > 255 || isEmpty(howTheyPositionField) || isEmpty(howWeAttackField) || howTheyPositionField.length > 8000 || howWeAttackField.length > 8000) {
                        isValidTheForm = false;
                    }
                    if (!isValidTheForm) {
                        if (isEmpty(statementField)) {
                            messageOfValidation += 'Statement is required<br/>';
                        }
                        if (statementField.length > 255) {
                            messageOfValidation += 'Statement has more than 255 characters<br/>';
                        }
                        if (isEmpty(howTheyPositionField)) {
                            messageOfValidation += 'How They Position is required<br/>';
                        }                        
                        if (isEmpty(howWeAttackField)) {
                            messageOfValidation += 'How We De-Position Them is required<br/>';
                        }
                        if ((howTheyPositionField.length > 8000) || (howWeAttackField.length > 8000)) {
                            messageOfValidation += 'Fields have a limit of 8000 characters including HTML characters which may not be visible<br/>';
                        }
                        lblValidSum.prop('innerHTML', messageOfValidation);
                        lblValidSum.css("color", "red");
                    }
                    else {
                        var divHTP = $('#DivHowTheyPosition' + productId);
                        divHTP[0].innerHTML = howTheyPositionField;
                        var divHWA = $('#DivHowWeAttack' + productId);
                        divHWA[0].innerHTML = howWeAttackField;
                        $.post(urlAction, parameters, function(data) {
                            var positioningStatment = $('#HdnPositioningStatment' + productId);
                            positioningStatment[0].value = statementField;
                            var positioningIdValue = $('#' + hdnPositioningIdVa);
                            if (positioningIdValue != null && positioningIdValue != undefined) {
                                if (positioningIdValue[0] != null && positioningIdValue[0] != undefined) {

                                    positioningIdValue[0].value = data;
                                } else {
                                    positioningIdValue.value = data;
                                }
                            }
                            var positioningAction = $('#HdnPositioningAction' + productId);
                            if (positioningAction.val() == 'Create') {
                                positioningAction[0].value = 'Update';
                            }
                        });

                        $(this).dialog("destroy");
                    }
                },
                "Close": function() {
                    $(this).dialog('destroy');
                }
            }
        });
        positioningObject.dialog('option', 'title', dialogTitle);
        positioningObject.html(dialogContent);
        positioningObject.dialog("open");
        $('.txtBoxHtml').cleditor({ height: 150, width: 545 });
    };
    var AddCompetitorPositioningDialog = function(object, Title, urlActionC, urlActionU, action, productId, competitorId, IndustryId, option, divHTPId, divHWAId, hdnStatename, hdnPositioningId, positioningRelation, MyCompanyId, MyUserId, urlGetPositioning) {
        var divHTPToChange = $('#' + divHTPId).text();
        var divHWAToChange = $('#' + divHWAId).text();
        var hdnStatenameToChange = $('#' + hdnStatename).val();
        var hdnPositioningIdValue = $('#' + hdnPositioningId).val();
        var hdnPositioningAction = $('#HdnPositioningAction' + productId).val();
        var urlAction = urlActionC;
        if (hdnPositioningAction == 'Update') {
            //Set the URL Action
            urlAction = urlActionU;
            //Update the values IN database
            var parametro = { PositioningId: hdnPositioningIdValue };
            var xmlhttp;
            var results = null;
            $.get(
            urlGetPositioning,
            parametro,
            function(data) {
                if (data != null && data != '') {
                    results = data;
                    if (results != "") {
                        var test = $('#' + hdnStatename);
                        test.prop('value', GetValueByKeyAndToken(results, 'PositioningStatment_' + hdnPositioningIdValue, '[TK' + hdnPositioningIdValue + ']'));
                        hdnStatenameToChange = GetValueByKeyAndToken(results, 'PositioningStatment_' + hdnPositioningIdValue, '[TK' + hdnPositioningIdValue + ']');
                        divHTPToChange = GetValueByKeyAndToken(results, 'PositioningHowTheyPosition_' + hdnPositioningIdValue, '[TK' + hdnPositioningIdValue + ']');
                        divHWAToChange = GetValueByKeyAndToken(results, 'PositioningHowWeAttack_' + hdnPositioningIdValue, '[TK' + hdnPositioningIdValue + ']');
                        var divHTP = $('#DivHowTheyPosition' + productId);
                        divHTP[0].innerHTML = divHTPToChange;
                        var divHWA = $('#DivHowWeAttack' + productId);
                        divHWA[0].innerHTML = divHWAToChange;
                        var positioningStatment = $('#HdnPositioningStatment' + productId);
                        positioningStatment[0].value = hdnStatenameToChange;
                    }
                }
            });
        }
        var divHTPToChangeAU = $('#' + divHTPId).html();
        var divHWAToChangeAU = $('#' + divHWAId).html();
        var hdnStatenameToChangeAU = $('#' + hdnStatename).val();
        var strCPPDForm = "<div id='CompetitorPositionigFormEdit'>";
        strCPPDForm += "<input type='hidden' name='hdnIndustryId' value='" + IndustryId + "' />";
        strCPPDForm += "<input type='hidden' name='hdnCompetitorId' value='" + competitorId + "' />";
        strCPPDForm += "<input type='hidden' name='hdnProductId' value='" + productId + "' />";
        strCPPDForm += "<input type='hidden' name='hdnPositioningRelation' value='" + positioningRelation + "' />";
        strCPPDForm += "<input type='hidden' name='hdnAction' value='" + urlAction + "' />";
        strCPPDForm += "<input type='hidden' name='hdnPositioningId' value='" + hdnPositioningIdValue + "' />";
        strCPPDForm += "<input type='hidden' name='hdnC' value='" + MyCompanyId + "' />";
        strCPPDForm += "<input type='hidden' name='hdnU' value='" + MyUserId + "' />";
        strCPPDForm += "<input type='hidden' name='hcnIsCompany' value='N' />";
        strCPPDForm += "<label id='LblValidationSumary'></label>";
        strCPPDForm += "<label>Statement Name:</label><input type='text' name='txtStatment' ";
        if (hdnPositioningAction == 'Update') {
            strCPPDForm += " value='" + hdnStatenameToChangeAU + "' ";
        }
        strCPPDForm += "/><br />";
        strCPPDForm += "<label>How They Position:</label><textarea id='TxtHowTheyPosition' name='TxtHowTheyPosition' WRAP=SOFT COLS=50 ROWS=4 class='txtBoxHtml'>";
        if (hdnPositioningAction == 'Update') {
            strCPPDForm += divHTPToChangeAU;
        }
        strCPPDForm += "</textarea><br />";
        strCPPDForm += "<label>How We De-Position Them:</label><textarea id='TxtHowWeAttack' name='TxtHowWeAttack' WRAP=SOFT COLS=50 ROWS=4 class='txtBoxHtml'>";
        if (hdnPositioningAction == 'Update') {
            strCPPDForm += divHWAToChangeAU;
        }
        strCPPDForm += "</textarea><br />";
        strCPPDForm += "</div>";
        FormCompetitorPositioningBox(object, Title, urlAction, action, strCPPDForm, productId, competitorId, IndustryId, hdnPositioningId, MyCompanyId, MyUserId);
    };

    var FormCompetitiveMessagingBox = function(object, dialogTitle, urlAction, action, dialogContent, productId, competitorId, industryId, hdnPositioningId) {
        var positioningObject = $("#PositioningBox");
        var MyCompanyId = '<%= ViewData["C"] %>';
        var MyUserId = '<%= ViewData["U"] %>';
        positioningObject.empty();
        positioningObject.dialog({
            width: 575, //if change the width on FrontEndSite.css
            modal: true,
            buttons: {
                "Ok": function() {
                    var hdnIndustryId = $('#CompetitiveMessagingFormEdit input[name=hdnIndustryId]').val();
                    var hdnCompetitorId = $('#CompetitiveMessagingFormEdit input[name=hdnCompetitorId]').val();
                    var hdnProductId = $('#CompetitiveMessagingFormEdit input[name=hdnProductId]').val();
                    var hdnPositioningRelation = $('#CompetitiveMessagingFormEdit input[name=hdnPositioningRelation]').val();
                    var hdnAction = $('#CompetitiveMessagingFormEdit input[name=hdnAction]').val();
                    var hdnPositioningId = $('#CompetitiveMessagingFormEdit input[name=hdnPositioningId]').val();
                    var hcnIsCompany = $('#CompetitiveMessagingFormEdit input[name=hcnIsCompany]').val();
                    var statementField = $('#CompetitiveMessagingFormEdit input[name=txtStatment]').val();
                    statementField = convertTextPlainHtml(statementField);
                    var howTheyAttackField = $('#CompetitiveMessagingFormEdit textarea[name=TxtHowTheyAttack]').val();
                    var howToDefendField = $('#CompetitiveMessagingFormEdit textarea[name=TxtHowToDefend]').val();
                    var lblValidSum = $('#LblValidationSumary');
                    var isValidTheForm = true;
                    var messageOfValidation = '';
                    var htaEncode = htmlEncode(howTheyAttackField);
                    var htdEncode = htmlEncode(howToDefendField);
                    var parameters = { hdnIndustryId: hdnIndustryId, hdnCompetitorId: hdnCompetitorId, hdnProductId: hdnProductId,
                        hdnPositioningRelation: hdnPositioningRelation, hdnAction: hdnAction, hdnPositioningId: hdnPositioningId, txtStatment: statementField,
                        TxtHowTheyAttack: htaEncode, TxtHowToDefend: htdEncode,
                        hdnC: MyCompanyId, hdnU: MyUserId, hcnIsCompany: hcnIsCompany
                    };
                    if (isEmpty(statementField) || statementField.length > 255 || isEmpty(howTheyAttackField) || isEmpty(howToDefendField) || howTheyAttackField.length > 8000 || howToDefendField.length > 8000) {
                        isValidTheForm = false;
                    }
                    if (!isValidTheForm) {
                        if (isEmpty(statementField)) {
                            messageOfValidation += 'Statement is required<br/>';
                        }
                        if (statementField.length > 255) {
                            messageOfValidation += 'Statement has more than 255 characters<br/>';
                            heighOfDiv = heighOfDiv + 15;
                        }
                        if (isEmpty(howTheyAttackField)) {
                            messageOfValidation += 'How They De-Position Us is required<br/>';
                        }                        
                        if (isEmpty(howToDefendField)) {
                            messageOfValidation += 'How We Respond is required<br/>';
                        }
                        if ((howTheyAttackField.length > 8000) || (howToDefendField.length > 8000)) {
                            messageOfValidation += 'Fields have a limit of 8000 characters including HTML characters which may not be visible<br/>';
                        }
                        lblValidSum.prop('innerHTML', messageOfValidation);
                        lblValidSum.css("color", "red");
                    } else {
                        $('div.dhta' + competitorId).each(function() {
                            this.innerHTML = howTheyAttackField;
                        });
                        $('div.dhtd' + competitorId).each(function() {
                            this.innerHTML = howToDefendField;
                        });
                        $.post(urlAction, parameters, function(data) {
                            $('.HdnCMStatmentId' + competitorId).each(function() {
                                this.value = statementField;
                            });
                            $('.HdnCMId' + competitorId).each(function() {
                                this.value = data;
                            });
                            $('.HdnCMAction' + competitorId).each(function() {
                                this.value = 'Update';
                            });
                        });
                        $(this).dialog('destroy');
                    }
                },
                "Close": function() {
                    $(this).dialog('destroy');
                }
            }
        });
        positioningObject.dialog('option', 'title', dialogTitle);
        positioningObject.html(dialogContent);
        positioningObject.dialog("open");
        $('.txtBoxHtml').cleditor({ height: 150, width: 545 });
    };

    var AddCompetitiveMessagingDialog = function(object, Title, urlActionC, urlActionU, action, productId, competitorId, industryId, option, divHTAId, divHTDId, hdnStatename, hdnPositioningId, positioningRelation) {
        var MyCompanyId = '<%= ViewData["C"] %>'; //cambiar
        var MyUserId = '<%= ViewData["U"] %>'; //cambira
        var MyProductCompany = $('#ProductList');
        var divHTAIdToChange = $('#' + divHTAId).text();
        var divHTDIdToChange = $('#' + divHTDId).text();
        var hdnStatenameToChange = $('#' + hdnStatename).val();
        var hdnPositioningIdValue = $('#' + hdnPositioningId).val();
        var hdnPositioningAction = $('#HdnCMPositioningAction' + productId).val();
        var urlAction = urlActionC;
        if (hdnPositioningAction == 'Update') {
            urlAction = urlActionU;
            //Update the values IN database
            var parametro = { PositioningId: hdnPositioningIdValue };
            var xmlhttp;
            var results = null;
            $.get(
            '<%= Url.Action("GetPositioningById", "Comparinator") %>',
            parametro,
            function(data) {
                if (data != null && data != '') {
                    results = data;
                    if (results != "") {
                        hdnStatenameToChange = GetValueByKeyAndToken(results, 'PositioningStatment_' + hdnPositioningIdValue, '[TK' + hdnPositioningIdValue + ']');
                        divHTAIdToChange = GetValueByKeyAndToken(results, 'PositioningHowTheyAttack_' + hdnPositioningIdValue, '[TK' + hdnPositioningIdValue + ']');
                        divHTDIdToChange = GetValueByKeyAndToken(results, 'PositioningHowToDefend_' + hdnPositioningIdValue, '[TK' + hdnPositioningIdValue + ']');
                        var divHTA = $('#DivHowTheyAttack' + productId);
                        divHTA[0].innerHTML = divHTAIdToChange;
                        var divHTD = $('#DivHowToDefend' + productId);
                        divHTD[0].innerHTML = divHTDIdToChange;
                        var positioningStatment = $('#HdnCMPositioningStatment' + productId);
                        positioningStatment[0].value = hdnStatenameToChange;
                    }
                }
            });
        }
        var divHTAToChangeAU = $('#' + divHTAId).html();
        var divHTDToChangeAU = $('#' + divHTDId).html();
        var hdnStatenameToChangeAU = $('#' + hdnStatename).val();
        //
        var strCMDForm = "<div id='CompetitiveMessagingFormEdit'>";
        strCMDForm += "<input type='hidden' name='hdnIndustryId' value='" + industryId + "' />";
        strCMDForm += "<input type='hidden' name='hdnCompetitorId' value='" + competitorId + "' />";
        strCMDForm += "<input type='hidden' name='hdnProductId' value='" + MyProductCompany.val() + "' />";
        strCMDForm += "<input type='hidden' name='hdnPositioningRelation' value='CM' />";
        strCMDForm += "<input type='hidden' name='hdnAction' value='" + urlAction + "' />";
        strCMDForm += "<input type='hidden' name='hdnPositioningId' value='" + hdnPositioningIdValue + "' />";
        strCMDForm += "<input type='hidden' name='hdnC' value='" + MyCompanyId + "' />";
        strCMDForm += "<input type='hidden' name='hdnU' value='" + MyUserId + "' />";
        strCMDForm += "<input type='hidden' name='hcnIsCompany' value='Y' />";
        strCMDForm += "<label id='LblValidationSumary'></label>";
        strCMDForm += "<label>Statement Name:</label><input type='text' name='txtStatment' ";

        if (hdnPositioningAction == 'Update') {
            strCMDForm += " value='" + hdnStatenameToChangeAU + "' ";
        }
        strCMDForm += "/><br />";
        strCMDForm += "<label>How They De-Position Us:</label><textarea name='TxtHowTheyAttack' WRAP=SOFT COLS=50 ROWS=4 class='txtBoxHtml'>";
        if (hdnPositioningAction == 'Update') {
            strCMDForm += divHTAToChangeAU;
        }
        strCMDForm += "</textarea><br />";
        strCMDForm += "<label>How We Respond:</label><textarea name='TxtHowToDefend' WRAP=SOFT COLS=50 ROWS=4 class='txtBoxHtml'>";
        if (hdnPositioningAction == 'Update') {
            strCMDForm += divHTDToChangeAU;
        }
        strCMDForm += "</textarea><br />";
        strCMDForm += "</div>";
        FormCompetitiveMessagingBox(object, Title, urlAction, action, strCMDForm, productId, competitorId, industryId, hdnPositioningId);
    };
    function editProductInfo(id) {
        var productInfoObject = $("#ProductInfo");
        var description = "";
        var pdValue = "";
        if ($('#' + id + "_ProductDescription").html() != null && $('#' + id + "_ProductDescription").html() != 'undefined' && $('#' + id + "_ProductDescription").html() != '') {
            pdValue = $('#' + id + "_ProductDescription").html();
        }
        var editProduct = "<textarea id='ProductDecription' WRAP=SOFT COLS=50 ROWS=4 class='txtBoxHtml' >" + pdValue + "</textarea>";
        productInfoObject.empty();
        productInfoObject.dialog({
            width: 575,
            modal: true,
            buttons: {
                "Ok": function() {
                    description = htmlEncode($("#ProductDecription").val());
                    var parameters = { Description: description, Id: id };
                    $.post('<%= Url.Action("EditProductInfo", "Comparinator") %>', parameters, function(data) {
                        $('#' + id + "_ProductDescription").html(data);
                    });
                    $(this).dialog('destroy');
                }
            }
        });
        productInfoObject.dialog('option', 'title', 'Edit Description');
        productInfoObject.html(editProduct);
        productInfoObject.dialog("open");
        $('#ProductDecription').cleditor({ height: 300, width: 545 });
    };
        
</script>
<script type="text/javascript">


    $.fn.changeToHidden = function() {
        this.each(function(i, ele) {
            var $ele = $(ele),
            $new = $('<input type="hidden">').prop('rel', $ele.html())
                                             .prop('value', $ele.prop('value'))
                                             .prop('class', $ele.prop('class'));
            $ele.parent().append($new);
            $ele.remove();
        });
    };

    $.fn.changeToOption = function() {
        this.each(function(i, ele) {
            var $ele = $(ele),
            $new = $('<option>').html($ele.prop('rel'))
                                .prop('value', $ele.prop('value'))
                                .prop('class', $ele.prop('class'));
            $ele.parent().append($new);
            $ele.remove();
        });
    };
    var GetIdByPosition = function(str, element) {
        /// <IdPartText>_<     first id  >_<   second id   >  
        ///  ImgFeedBack_18821786158532650_19071721096826085
        str = str.split('_'); ///
        if (str.length > element) 
            return str[element];
        else
            return null;
    };

    $('.silverFeedback').on({
        'click': function() {
            var id = $(this).attr('id');
            var src = $(this).attr('src');
            var industryId = GetIdByPosition(id, 1);
            var productId = GetIdByPosition(id, 2);
            var companyId = '<%= ViewData["C"] %>'; //cambiar
            var userId = '<%= ViewData["U"] %>'; //cambira
            if (industryId != null && productId != null) {
                var urlActionFeed = '<%= Url.Action("SendFeedBackSilverBullets","Comparinator") %>?Id=' + industryId + '&pId=' + productId + '&U=' + userId + '&C=' + companyId;
                FeedBackFormDlg(urlActionFeed);
            }

        }
    });
    $('.silverComment').on({
        'click': function() {
            var id = $(this).attr('id');
            var src = $(this).attr('src');
            var industryId = GetIdByPosition(id, 1);
            var productId = GetIdByPosition(id, 2);
            var companyId = '<%= ViewData["C"] %>'; //cambiar
            var userId = '<%= ViewData["U"] %>'; //cambira
            if (industryId != null && productId != null) {
                var urlActionGetComment = '<%= Url.Action("GetSilverBulletsPublicComments","Forum") %>?IndustryId=' + industryId + '&ProductId=' + productId + '&ObjectType=SILVER&U=' + userId + '&C=' + companyId;
                var urlActionNewComment = '<%= Url.Action("ExternalResponse","Forum") %>?EntityId=' + productId + '&ForumResponseId=0&ObjectType=SILVER&IndustryId=' + industryId + '&ProductId=' + productId + '&C=' + companyId + '&U=' + userId;
                ExternalCommentsDlg(urlActionGetComment, urlActionNewComment);
            }

        }
    });
</script>
<div id="AlertMessageDialog"></div>
<div id="comp_new" style="display: none">
    <table>
        <tr>
            <td style='text-align: right'>
                Group:
            </td>
            <td>
                <label id="groupname">
                </label>
            </td>
        </tr>
        <tr>
        </tr>
        <tr>
            <td style='text-align: right'>
                Set:
            </td>
            <td>
                <label id="setname">
                </label>
            </td>
        </tr>
        <tr>
        </tr>
        <tr>
            <td style='text-align: right'>
                Name:
            </td>
            <td>
                <input type='text' id='name' value='' />
            </td>
        </tr>
        <tr>
            <td style='text-align: right'>
                Benefit:
            </td>
            <td>
                <input type='text' id='benefitValue' value='' />
            </td>
        </tr>
        <tr>
            <td style='text-align: right'>Type</td>
            <td>
                <select onchange="UpdateMostDesiredField(this, this.value)" name="Type" id="Type">
                    <option value="BOL">Boolean</option>
                    <option value="LIS" selected="selected">List</option>
                    <option value="NUM">Numeric</option>
                </select>
            </td>
        </tr>
        <tr class="trmostdesiredvalue" style="display:none">
            
            <td style='text-align: right'>Most Desired Value</td>
            <td>
                <select name="SelMostDesiredValue" id="SelMostDesiredValue">
                    <option value=""></option>
                    <option value="N" class="Boolean">No</option>
                    <option value="Y" class="Boolean">Yes</option>
                    <option value="H" class="Numeric">Highest</option>
                    <option value="L" class="Numeric">Lowest</option>
                    <option value="NA" class="Numeric">N/A</option>
                </select>
            </td>
        </tr>
        <tr>

            <td style='text-align: right'>
                Relevancy:
            </td>
            <td>
               <select name="Relevancy" id="Relevancy">
                    <option value="HIGH">High</option>
                    <option value="MEDI" selected="selected">Medium</option>
                    <option value="LOW">Low</option>
               </select>
            </td>
        </tr>
    </table>
</div>
<%if (SomeTab)
      {%>
        <div class="comp_blockt">
        <label class="comp_row_title">COMPARINATOR RESULT</label>
        <br />
        
        </div>
    <%}%>
<%UserProfile user = (UserProfile)ViewData["User"]; %>
        <%string tblPricingWidth  = "width:" + (Titles.Count * 203 + 330 + 2 + 8).ToString() + "px";%>
        <%string tblFeaturesWidth = "width:" + (Titles.Count * 203 + 500).ToString() + "px";%>
   <div class="<%=!SomeTab || (SomeTab && DefaultsComparinatorExport.Equals("false")) ? "comp_dn":"" %>">
     <div class="comp_export" >
        <label>Export Tools:</label>
        <a href="<%= Url.Action("ExportToWord", "Comparinator",new {U=(string)ViewData["U"],C=(string)ViewData["C"]}) %>" title="Export to Word format" >
            <img alt="Export to Word format" src="<%= Url.Content("~/Content/Images/Icons/export_word.gif")%>" class="export_icon" />
        </a>
        <a href="<%= Url.Action("ExportComparinatorTabsToPDF", "Comparinator",new {U=(string)ViewData["U"],C=(string)ViewData["C"]}) %>"  title="Export to PDF format" >
            <img alt="Export to PDF format" src="<%= Url.Content("~/Content/Images/Icons/export_pdf.gif")%>" class="export_icon" />
        </a>
        <a href="<%= Url.Action("ExportComparinatorTabsToExcel", "Comparinator",new {U=(string)ViewData["U"],C=(string)ViewData["C"]}) %>" title="Export to Excel format" >
            <img alt="Export to Excel format" src="<%= Url.Content("~/Content/Images/Icons/export_excel.png")%>" class="export_icon" />
        </a>
    </div>
    
    </div>
    <ul class="comp_tabs" style='<%=SomeTab? "":"display:none" %>'>
    <%if (DefaultsIndustryInfoTab)
      {%>
    <li ><a target="#tab0"><%=ViewData["IndustryLabel"]%> Info</a></li>
    <%}%>
    <%if (DefaultsInfoTab)
      {%>
      <li ><a target="#tab1"><%=ViewData["ProductLabel"]%> Info</a></li>
    <%}%>
    <%if (DefaultsSilverBulletsTab)
      {%>
      <li><a target="#tab7"  onclick="silverbullets_update()">Silver Bullets</a></li>
     <%}%>
     <%if (DefaultsPositioningTab)
      {%>
      <li><a target="#tab2">Positioning</a></li>
    <%}%>
     <%if (DefaultsPricingTab)
      {%>
       <li><a target="#tab3">Pricing</a></li>
    <%}%>
     <%if (DefaultsFeaturesTab)
      {%>
      <li id="featuresTab"><a target="#tab4">Features</a></li>
    <%}%>
     <%if (DefaultsSalesToolsTab)
      {%>
      <li><a target="#tab5">Sales Tools</a></li>
    <%}%> 
    <%if (DefaultsNewsTab)
      {%>
        <li><a target="#tab6">News</a></li> 
    <%}%>
</ul>
 
<% var ProductCollection = (IList<Product>)ViewData["Products"]; %>
<div class="comp_rtc">
    <div id="tab0" class="<%=DefaultsIndustryInfoTab ? "comp_tab_content":"comp_dn" %>">
        <% Industry industry = (Industry)ViewData["IndustrySelected"]; %>
        <div style="margin-left: 10px; margin-right: 10px; min-height: 250px;">
        <span id="spnDescription">
        <%= industry.Description %>
        </span>
        </div>
    </div>
    <div id="tab1" class="<%=DefaultsInfoTab ? "comp_tab_content":"comp_dn" %>">
        <!--Product Info-->
        <% if (ProductCollection != null && ProductCollection.Count > 0)
           {%>
        <%
       
            foreach (Product oProduct in ProductCollection)
            { %>
        <div>
            <table class="elements" style="width:100%; table-layout:fixed;">
                <thead style="background: #e0e0e0;">
                    <tr>
                        <th style="width: 30%;">
                            <%=ViewData["ProductLabel"]%>
                        </th>
                        <th style="width: 60%;">
                            Description
                            <% if (user.SecurityGroupId != "ENDUSER")
                               {%>
                                <img alt="" style="float: right; margin-right: 5px;" title='Edit Statement' src="<%= Url.Content("~/Content/Images/Icons/properties.png")%>" onclick="editProductInfo('<%=oProduct.Id%>')"/>
                            <%} %>                            
                        </th>
                    </tr>
                </thead>
                <tr style="height:30px;" >
                    <td style="height:30px;" valign="top" align="center">
                      <div style="display:inline">  <!-- For not overflow image-->
                        <%=oProduct.Competitor.Name%><br />
                        <%=oProduct.Name%><br />
                        <%if (string.IsNullOrEmpty(c))
                      { %>
                      <a href="<%= Url.Action("ChangeProduct", "ContentPortal")+ "?Industry=" + oProduct.Competitor.Industry.Id + "&Competitor=" + oProduct.Competitor.Id + "&Product=" + oProduct.Id%>">
                           <img src="<%=Html.ImageUrl(oProduct.ImageUrl)%>" alt="" style="height:auto;width:auto;max-width:170px;max-height:170px;border-color:White;"/>
                        </a>
                       <%}
                      else
                      {%>
                           <img src="<%=Html.ImageUrl(oProduct.ImageUrl)%>" alt="" style="height:auto;width:auto;max-width:170px;max-height:170px;border-color:White;"/>
                    <%} %>
                        <br /><br />
                        <%if (!DefaultsDisabPublComm)
                          { %>
                                <a id="A1" href="javascript:void(0)" onclick="ExternalCommentsDlg('<%=Url.Action("GetComments", "Forum",new {EntityId=oProduct.Id,ObjectType=DomainObjectType.Product, C=c, U=u},null)%>','<%=Url.Action("ExternalResponse", "Forum",new {EntityId=oProduct.Id,ForumResponseId=0,ObjectType=DomainObjectType.Product, C=c, U=u},null)%>','<%=oProduct.Id%>')">
                                <%string cssImgComments = oProduct.HasComment ? "ImageCommentsY" : "ImageCommentsN"; %>
                                    <img name="ImgComents<%=oProduct.Id%>" class="<%=cssImgComments %>" src="<%=Url.Content("~/Content/Images/Icons/CommentWhite.png") %>" width="22px" title="Add public comment" />
                                </a>
                        <%} %>
                        <a id="A2" href="javascript: void(0);" onclick="ExternalFeedBackWithAttachedDlg('<%= Url.Action("FeedBackMessage", "Forum", new {EntityId=oProduct.Id,EntityType=DomainObjectType.Product, U=u, SubmittedVia=FeedBackSubmitedVia.ProductInfo  }) %>','FeedBack Dialog');">
                        <img src="<%=Url.Content("~/Content/Images/Icons/testfeedback.gif") %>" width="22px" title="Add private feedback" />
                        </a>
                     </div>   
                    </td>
                    <td valign="top" style="list-style:none;" class="AddTargetBlankToLin">
                        <span id="<%=oProduct.Id%>_ProductDescription"><%=oProduct.Description%></span>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <%
            } %>
        <%} //for by products
        %>
    </div>
    <div id="tab7" class="<%=DefaultsSilverBulletsTab ? "comp_tab_content":"comp_dn" %>">
      <div id="sb_content">
            <div class="divCheckBoxSilver">
                <input type="checkbox" id="chkBoxSilverBullets" onclick="hiddenIcons(this.checked);" checked />Show Discussion / Feedback<br />
            </div>
            <div id="sb_content_s1">
            <h3><%=SilverBulletsSections.S1%></h3>
            <%foreach(Product p in G1)  
              { %>
                <div class="lineSilver">
                    <div class="itemLineSilver labelSilver">
                        <label>
                            <%=p.Name %>
                        </label>
                    </div>
                    <div id="div_sb_<%=p.Id%>" class="itemLineSilver">
                    <%if (!DefaultsDisabPublComm)
                          { %>
                        <%string cssImgSilvComm = p.HasSilverComment ? "ImageCommentsY" : "ImageCommentsN"; %>
                        <img id="ImgComents_<%=p.IndustryId%>_<%=p.Id%>" name="ImgComents_<%=p.IndustryId%>_<%=p.Id%>" class="<%=cssImgSilvComm %> silverComment" src="<%=Url.Content("~/Content/Images/Icons/CommentWhite.png") %>" width="22px" title="Add public comment" />
                        <%} %>
                        <img id="ImgFeedBack_<%=p.IndustryId%>_<%=p.Id%>" name="ImgFeedBack_<%=p.IndustryId%>_<%=p.Id%>" class="silverFeedback" src="<%=Url.Content("~/Content/Images/Icons/testfeedback.gif") %>" width="22px" title="Add private feedback" />
                    </div>
                </div>
                <table id="sb_<%=p.Id%>" > 
                  <tbody></tbody>
                </table>
                <br />
            <%} %>
            </div>
            <div id="sb_content_s2">
            <h3><%=SilverBulletsSections.S2%></h3>
            <%foreach(Product p in G1)  
              { %>
                <div class="lineSilver">
                    <div class="itemLineSilver labelSilver">
                        <label>
                            <%=p.Name %>
                        </label>
                    </div>
                    <div id="div_sb_<%=p.Id%>" class="itemLineSilver">
                    <%if (!DefaultsDisabPublComm)
                          { %>
                        <%string cssImgSilvComm = p.HasSilverComment ? "ImageCommentsY" : "ImageCommentsN"; %>
                        <img id="ImgComents_<%=p.IndustryId%>_<%=p.Id%>" name="ImgComents_<%=p.IndustryId%>_<%=p.Id%>" class="<%=cssImgSilvComm %> silverComment" src="<%=Url.Content("~/Content/Images/Icons/CommentWhite.png") %>" width="22px" title="Add public comment" />
                        <%} %>
                        <img id="ImgFeedBack_<%=p.IndustryId%>_<%=p.Id%>" name="ImgFeedBack_<%=p.IndustryId%>_<%=p.Id%>" class="silverFeedback" src="<%=Url.Content("~/Content/Images/Icons/testfeedback.gif") %>" width="22px" title="Add private feedback" />
                    </div>
                </div>
                <table id="sb_<%=p.Id%>" > 
                  <tbody></tbody>
                </table>
                <br />
            <%} %>
            </div>            
            <div id="sb_content_s3">
            <h3><%=SilverBulletsSections.S3%></h3>
            <%foreach(Product p in G2)  
              { %>
                <div class="lineSilver">
                    <div class="itemLineSilver labelSilver">
                        <label>
                            <%=p.Name %>
                        </label>
                    </div>
                    <div id="div_sb_<%=p.Id%>" class="itemLineSilver">
                    <%if (!DefaultsDisabPublComm)
                          { %>
                        <%string cssImgSilvComm = p.HasSilverComment ? "ImageCommentsY" : "ImageCommentsN"; %>
                        <img id="ImgComents_<%=p.IndustryId%>_<%=p.Id%>" name="ImgComents_<%=p.IndustryId%>_<%=p.Id%>" class="<%=cssImgSilvComm %> silverComment" src="<%=Url.Content("~/Content/Images/Icons/CommentWhite.png") %>" width="22px" title="Add public comment" />
                        <%} %>
                        <img id="ImgFeedBack_<%=p.IndustryId%>_<%=p.Id%>" name="ImgFeedBack_<%=p.IndustryId%>_<%=p.Id%>" class="silverFeedback" src="<%=Url.Content("~/Content/Images/Icons/testfeedback.gif") %>" width="22px" title="Add private feedback" />
                    </div>
                </div>
                <table id="sb_<%=p.Id%>" > 
                  <tbody></tbody>
                </table>
                <br />
            <%} %>
            </div>            

            <div id="sb_content_s4">
            <h3><%=SilverBulletsSections.S4%></h3>
            <%foreach(Product p in G2)  
              { %>
                <div class="lineSilver">
                    <div class="itemLineSilver labelSilver">
                        <label>
                            <%=p.Name %>
                        </label>
                    </div>
                    <div  id="div_sb_<%=p.Id%>" class="itemLineSilver">
                    <%if (!DefaultsDisabPublComm)
                          { %>
                        <%string cssImgSilvComm = p.HasSilverComment ? "ImageCommentsY" : "ImageCommentsN"; %>
                        <img id="ImgComents_<%=p.IndustryId%>_<%=p.Id%>" name="ImgComents_<%=p.IndustryId%>_<%=p.Id%>" class="<%=cssImgSilvComm %> silverComment" src="<%=Url.Content("~/Content/Images/Icons/CommentWhite.png") %>" width="22px" title="Add public comment" />
                        <%} %>
                        <img id="ImgFeedBack_<%=p.IndustryId%>_<%=p.Id%>" name="ImgFeedBack_<%=p.IndustryId%>_<%=p.Id%>" class="silverFeedback" src="<%=Url.Content("~/Content/Images/Icons/testfeedback.gif") %>" width="22px" title="Add private feedback" />
                    </div>
                </div>
                <table id="sb_<%=p.Id%>" > 
                  <tbody></tbody>
                </table>
                <br />
            <%} %>
            </div>            
            
      </div>
    </div>
    <div id="tab2" class="<%=DefaultsPositioningTab ? "comp_tab_content":"comp_dn" %>">
        <!--Positioning-->
        <div>
            <div >
                <table id="PositioningClientProductTable" class="elments" width="100%">
                    <tr>
                        <td style="width: 30%; text-align:center; vertical-align:top;">
                            <%= ViewData["CurrentCompanyName"].ToString()%> <br />
                            <%= Html.DropDownList("ProductList", (SelectList)ViewData["ProductList"], new { id = "ProductList", onchange = "UpdateHowWePositionByProduct(); ChangeImageSRC(this, this.selected)" })%>
                                                    <br /><br />
                                            <img id="imgddl" alt="" style="height:auto;width:auto;max-width:170px;max-height:170px;border-color:White;"/>
                                   
                       <br /><br />

                            <%if (!DefaultsDisabPublComm)
                              { %>
                              <a id="CCP" href="javascript:void(0)" onclick="OpenCommentDlg()" >
                                 <img src="<%=Url.Content("~/Content/Images/Icons/CommentWhite.png") %>" width="22px" title="Add public comment" />
                              </a>
                            <%} %>                           
                            <a id="FCP" href="javascript: void(0);" onclick="OpenFeedBackDlg();">
                                <img src="<%=Url.Content("~/Content/Images/Icons/testfeedback.gif") %>" width="22px" title="Add private feedback" />
                            </a>
                        </td>
                        <td style="width: 70%;">
                            <div id="hwpBoxData" class="contentBoxData">
                                <input type="hidden" id="HdnClPPositioningId" value="" />
                                <input type="hidden" id="HdnClPPositioningAction" value="" />
                                <input type="hidden" id="HdnClPPositioningStatement" value="" />
                                <div id="hwpHeader" class="contentBoxDataHeader">
                                    How We Position
                                    <% if (user.SecurityGroupId != "ENDUSER")
                                       {%>
                                    <img alt="" style="float: right; margin-right: 5px;" title='Edit Statement' src="<%= Url.Content("~/Content/Images/Icons/properties.png")%>" onclick='AddCompanyPositioningDialog(this, "Edit Statment","", "<%= Url.Action("CreatePositioningOnComparinator", "Comparinator") %>", "<%= Url.Action("UpdatePositioningOnComparinator", "Comparinator") %>","<%= ViewData["IndustryId"].ToString() %>","<%= Compelligence.Domain.Entity.Resource.PositioningRelation.Positioning %>", "<%= ViewData["C"] %>","<%= ViewData["U"] %>")'/>
                                    <%} %>
                                </div>
                                    <div id="hwpBoxDataList" class="contentBoxDataList">
                                    </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div>
             <% var CompetitorProductCollection = (IList<Product>)ViewData["CompetitorProducts"];
                foreach (Product oProduct in CompetitorProductCollection)
                { %>
                <table class="elements" width="100%">
                    <thead style="background: #e0e0e0;">
                        <tr>
                            <th style="width: 30%;">
                                <%=ViewData["CompetitorLabel"]%> <%=ViewData["ProductLabel"]%>
                            </th>
                            <th style="width: 70%;">
                                Positioning
                            </th>
                        </tr>
                    </thead>
                    <tr class ="comp_table_tr_top comp_table_tr_bottom">
                        <td style="height: 30px;"  valign="middle" align="center">
                            <%=oProduct.Competitor.Name%><br />
                            <%=oProduct.Name%><br />
                            <%if (string.IsNullOrEmpty(c))
                            { %>
                                <a href="<%= Url.Action("ChangeProduct", "ContentPortal")+ "?Industry=" + oProduct.Competitor.Industry.Id + "&Competitor=" + oProduct.Competitor.Id + "&Product=" + oProduct.Id%>">
                                    <img src="<%=Html.ImageUrl(oProduct.ImageUrl)%>"  alt="" style="height:auto;width:auto;max-width:170px;max-height:170px;border-color:White;"/>
                                </a>
                             <%}
                            else
                            {%>
                                    <img src="<%=Html.ImageUrl(oProduct.ImageUrl)%>"  alt="" style="height:auto;width:auto;max-width:170px;max-height:170px;border-color:White;"/>
                            <%} %>

                          <br /><br />
                            <%if (!DefaultsDisabPublComm)
                              { %>
                                    <a id="testforumicon" href="javascript:void(0)" onclick="ExternalCommentsDlg('<%=Url.Action("GetComments", "Forum",new {EntityId=oProduct.Id,ObjectType=DomainObjectType.Product, C=c, U=u},null)%>','<%=Url.Action("ExternalResponse", "Forum",new {EntityId=oProduct.Id,ForumResponseId=0,ObjectType=DomainObjectType.Product, C=c, U=u},null)%>','<%=oProduct.Id%>')">
                                    <%string cssImgComments = oProduct.HasComment ? "ImageCommentsY" : "ImageCommentsN"; %>
                                        <img id ="ImgComents<%=oProduct.Id%>" class="<%=cssImgComments %>" src="<%=Url.Content("~/Content/Images/Icons/CommentWhite.png") %>" width="22px" title="Add public comment" />
                                    </a>
                            <%} %>
                            <a id="testfeedbackicon" href="javascript: void(0);" onclick="ExternalFeedBackWithAttachedDlg('<%= Url.Action("FeedBackMessage", "Forum", new {EntityId=oProduct.Id,EntityType=DomainObjectType.Product, U=u, SubmittedVia=FeedBackSubmitedVia.Positioning  }) %>','FeedBack Dialog');">
                            <img src="<%=Url.Content("~/Content/Images/Icons/testfeedback.gif") %>" width="22px" title="Add private feedback" />
                            </a> 
                        </td>
                        <td style="height: 30px" >
                            <div style="height: 33%;">
                                <div class="line" style="width:100%;">
                                    <div id="hta" class="contentFieldBoxLeftData">
                                        <input type="hidden" id="HdnCMPositioningId<%= oProduct.Id%>" value="" class="HdnCMId<%= oProduct.CompetitorId%>"/>
                                        <input type="hidden" id="HdnCMPositioningStatment<%= oProduct.Id%>" value=""  class="HdnCMStatmentId<%= oProduct.CompetitorId%>"/>
                                        <input type="hidden" id="HdnCMPositioningAction<%= oProduct.Id%>" value=""  class="HdnCMAction<%= oProduct.CompetitorId%>"/>
                                        <div id="contentPanelTitle" class="contentBoxDataHeader">
                                        How They De-Position Us
                                        <% if (user.SecurityGroupId != "ENDUSER")
                                           {%>
                                        <img alt="" style="float: right; margin-right: 5px;" title='Edit Statement' src="<%= Url.Content("~/Content/Images/Icons/properties.png")%>" onclick='javascript:AddCompetitiveMessagingDialog(this,"Edit Statment", "<%= Url.Action("CreatePositioningOnComparinator", "Comparinator") %>", "<%= Url.Action("UpdatePositioningOnComparinator", "Comparinator") %>", "<%= oProduct.CompetitiveMessaging.VirtualAction %>","<%= oProduct.Id %>","<%= oProduct.CompetitiveMessaging.VirtualCompetitorId %>","<%= oProduct.CompetitiveMessaging.VirtualIndustryId %>","0","DivHowTheyAttack<%= oProduct.Id%>","DivHowToDefend<%= oProduct.Id%>","HdnCMPositioningStatment<%= oProduct.Id%>", "HdnCMPositioningId<%= oProduct.Id%>", "<%= Compelligence.Domain.Entity.Resource.PositioningRelation.CompetitiveMessaging %>")'/>
                                        <%} %>
                                        </div>
                                        <div id="DivHowTheyAttack<%= oProduct.Id%>" class="contentBoxDataList dhta<%= oProduct.CompetitorId%>">
                                            <% if(oProduct.CompetitiveMessaging!= null && !string.IsNullOrEmpty(oProduct.CompetitiveMessaging.HowTheyAttack)) %>
                                            <%= oProduct.CompetitiveMessaging.HowTheyAttack %>
                                        </div>
                                    </div>
                                    <div id="htd" class="contentFieldBoxRightData">
                                        <div id="htdb" class="contentBoxDataHeader">
                                            How We Respond
                                            <% if (user.SecurityGroupId != "ENDUSER")
                                             {%>
                                            <img alt="" style="float: right; margin-right: 5px;" title='Edit Statement' src="<%= Url.Content("~/Content/Images/Icons/properties.png")%>" onclick='javascript:AddCompetitiveMessagingDialog(this,"Edit Statment", "<%= Url.Action("CreatePositioningOnComparinator", "Comparinator") %>", "<%= Url.Action("UpdatePositioningOnComparinator", "Comparinator") %>", "<%= oProduct.CompetitiveMessaging.VirtualAction %>","<%= oProduct.Id %>","<%= oProduct.CompetitiveMessaging.VirtualCompetitorId %>","<%= oProduct.CompetitiveMessaging.VirtualIndustryId %>","0","DivHowTheyAttack<%= oProduct.Id%>","DivHowToDefend<%= oProduct.Id%>","HdnCMPositioningStatment<%= oProduct.Id%>", "HdnCMPositioningId<%= oProduct.Id%>", "<%= Compelligence.Domain.Entity.Resource.PositioningRelation.CompetitiveMessaging %>")'/>
                                            <%}%>
                                        </div>
                                        <div id="DivHowToDefend<%= oProduct.Id%>" class="contentBoxDataList dhtd<%= oProduct.CompetitorId%>">
                                            <% if (oProduct.CompetitiveMessaging != null && !string.IsNullOrEmpty(oProduct.CompetitiveMessaging.HowWeDefend)) %>
                                            <%= oProduct.CompetitiveMessaging.HowWeDefend %>
                                        </div>
                                    </div>
                                </div>
                                <div class="line" style="width:100%;">
                                        <input type="hidden" id="HdnPositioningId<%= oProduct.Id%>" value="<%= oProduct.CompetitiveMessaging.Id %>" />
                                        <input type="hidden" id="HdnPositioningStatment<%= oProduct.Id%>" value="<%= oProduct.CompetitiveMessaging.Name %>" />
                                        <input type="hidden" id="HdnPositioningAction<%= oProduct.Id%>" value="<%= oProduct.CompetitiveMessaging.VirtualAction %>" />
                                        <div id="htp" class="contentFieldBoxLeftData">
                                            <div id="Div3" class="contentBoxDataHeader">
                                                How They Position
                                                <% if (user.SecurityGroupId != "ENDUSER")
                                                   {%>
                                                <img alt="" style="float: right; margin-right: 5px;" title='Edit Statement' src="<%= Url.Content("~/Content/Images/Icons/properties.png")%>" onclick='javascript:AddCompetitorPositioningDialog(this,"Edit Statment", "<%= Url.Action("CreatePositioningOnComparinator", "Comparinator") %>", "<%= Url.Action("UpdatePositioningOnComparinator", "Comparinator") %>", "<%= oProduct.CompetitiveMessaging.VirtualAction %>","<%= oProduct.CompetitiveMessaging.VirtualProductId %>","<%= oProduct.CompetitiveMessaging.VirtualCompetitorId %>","<%= oProduct.CompetitiveMessaging.VirtualIndustryId %>","0","DivHowTheyPosition<%= oProduct.Id%>","DivHowWeAttack<%= oProduct.Id%>","HdnPositioningStatment<%= oProduct.Id%>", "HdnPositioningId<%= oProduct.Id%>", "<%= Compelligence.Domain.Entity.Resource.PositioningRelation.Positioning %>", "<%= ViewData["C"] %>", "<%= ViewData["U"] %>", "<%= Url.Action("GetPositioningById", "Comparinator") %>")'/>
                                                <%} %>
                                            </div>
                                                <div id="DivHowTheyPosition<%= oProduct.Id%>" class="contentBoxDataList">
                                                    <% if (oProduct.CompetitiveMessaging != null && !string.IsNullOrEmpty(oProduct.CompetitiveMessaging.HowTheyPosition)) %>
                                                    <%= oProduct.CompetitiveMessaging.HowTheyPosition %>
                                                </div>    
                                        </div>
                                        <div id="hwa" class="contentFieldBoxRightData">
                                            <div id="Div5" class="contentBoxDataHeader">
                                                How We De-Position Them
                                                <% if (user.SecurityGroupId != "ENDUSER")
                                                   {%>
                                                <img alt="" style="float: right; margin-right: 5px;" title='Edit Statement' src="<%= Url.Content("~/Content/Images/Icons/properties.png")%>" onclick='javascript:AddCompetitorPositioningDialog("ffff","Edit Statment", "<%= Url.Action("CreatePositioningOnComparinator", "Comparinator") %>", "<%= Url.Action("UpdatePositioningOnComparinator", "Comparinator") %>", "<%= oProduct.CompetitiveMessaging.VirtualAction %>","<%= oProduct.CompetitiveMessaging.VirtualProductId %>","<%= oProduct.CompetitiveMessaging.VirtualCompetitorId %>","<%= oProduct.CompetitiveMessaging.VirtualIndustryId %>","1","DivHowTheyPosition<%= oProduct.Id%>","DivHowWeAttack<%= oProduct.Id%>","HdnPositioningStatment<%= oProduct.Id%>", "HdnPositioningId<%= oProduct.Id%>", "<%= Compelligence.Domain.Entity.Resource.PositioningRelation.Positioning %>", "<%= ViewData["C"] %>", "<%= ViewData["U"] %>", "<%= Url.Action("GetPositioningById", "Comparinator") %>")'/>
                                                <%} %>
                                            </div>
                                                <div id="DivHowWeAttack<%= oProduct.Id%>" class="contentBoxDataList">
                                                    <% if (oProduct.CompetitiveMessaging != null && !string.IsNullOrEmpty(oProduct.CompetitiveMessaging.HowWeAttack)) %>
                                                    <%= oProduct.CompetitiveMessaging.HowWeAttack %>
                                                </div>    
                                        </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                  
                </table>
                  <%
                 } %>
            </div>
        </div>
    </div>
    <div id="tab3" class="<%=DefaultsPricingTab ? "comp_tab_content":"comp_dn" %>">
   <!--Pricing-->
         <div id="comp_princinggroup" class="comp_pricinggroup" style="width:100%;">
            <p class="comp_pricingheadname" style="padding-left: 5px;">
                Pricing</p>
        </div>
         <% IList<ComparinatorPriceRequired> pricingList = (IList<ComparinatorPriceRequired>)ViewData["ComparinatorPriceRequired"];
            if (pricingList != null && pricingList.Count > 0)
            {
             %>
             <%
             foreach (var oRequired in (IList<ComparinatorPriceRequired>)ViewData["ComparinatorPriceRequired"])
             { %>
        <% if (oRequired.PriceTypes.Count > 0 && oRequired.PriceTypes[0].Prices.Count != null && oRequired.PriceTypes[0].Prices.Count > 0)
           { %> 
        <div id="comp_pricinghead" class="comp_pricinghead" style="width:100%;">
            <div class='comp_criteriacollapse'>
            </div>
            <div class="comp_pricingheadname">
                <%=oRequired.Name%></div>
        </div>
       
       <table id="Table1" class="comp_pricingtable" style="width:100%;height:100%">
            <colgroup>
                <col style="width:10%" />
               <%decimal total = (oRequired.PriceTypes[0].Prices.Count * 2);
                 total = 90 / total;
                 decimal resul = ((total * 90) / 100);

                 for (int g = 0; g < (oRequired.PriceTypes[0].Prices.Count * 2); g++)
                 {
                     if (g % 2 == 0)
                     { %>
                     
                     <col  style="word-wrap: break-word" width='<%=((total+resul)-3)%>%'/>
                     <%}
                     else
                     { %>
                         <col  style="word-wrap: break-word" width='<%=((total-resul)+3)%>%'/>
                     <%} %>
                <%} %>
            </colgroup>

            <thead class="comp_head">
                <tr>
                    <th style="border-color:Black;border-style:solid;border-width:1px">
                        Criteria
                    </th>
              
                    <%foreach (var oPrice in oRequired.PriceTypes[0].Prices)
                      { %>
                      <th colspan="2" style="border-color:Black;border-style:solid;border-width:1px">
                        <%=oPrice.Product.Name%>
                    </th>
                   
                    <%} %>
                </tr>
            </thead>
            <tbody>
                <%foreach (var oType in oRequired.PriceTypes)
                  {
                     
                      %>
                  
                    <% for (int m = 0; m < oType.x; m++)
                       {%>
                           <tr>
                        <% if (m == 0)
                           { %>
                       <th  class="comp_head" style='border-color:Black;border-style:solid;border-width:1px;background-color:#FFFFFF;width: 250px; font-family:verdana;font-size:12px;' rowspan='<%=(oType.x*3)%>'><%=oType.Name%></th>
                        <% }
                           for (int j = 0; j < 3; j++)
                           { %>
                     
                        <% for (int n = 0; n < oType.y; n++)
                           { %>
                           <%if (j == 0)
                             {%>
                                <td colspan="2" style="border-top: 1px solid black;border-left: 1px solid black;border-right: 1px solid black;vertical-align:bottom;text-align:left">&nbsp;
                                    <p style="margin-left:5px;margin-right:5px"><b><%= oType.ArrayPrices[m, n].Name.ToUpper()%></b></p>
                                 </td>
                             
                            <%} %>
                            <%if (j == 1)
                              {%>
                          
                            <td colspan="2" style="border-top: thin dotted gray;border-right: 1px solid black;border-left:1px solid black;vertical-align:bottom;text-align:left">&nbsp;
                                    <p style="margin-left:5px;margin-right:5px"><%=oType.ArrayPrices[m, n].PartNumber%> </p>
                            </td>
                          
                             
                           <%} %>        
                                    
                           <%if (j == 2)
                             {
                                 if (m == (oType.x - 1))
                                 { %>
                                 <td colspan="2" style="border-top: thin dotted gray;border-right: 1px solid black;border-left: 1px solid black;border-bottom:1px solid black;vertical-align:bottom;text-align:right">&nbsp;
                             <%}
                                 else
                                 { %>
                                   <td colspan="2" style="border-top: thin dotted gray;border-right: 1px solid black;border-left: 1px solid black;border-bottom:1px solid black;vertical-align:bottom;text-align:right">&nbsp;
                             <%} %>
                            <%if (oType.ArrayPrices[m, n].Value != null)
                              {
                                  decimal value = (decimal)oType.ArrayPrices[m, n].Value;
                                  string price = value.ToString("N2", CultureInfo.InvariantCulture);
                                  if (oType.ArrayPrices[m, n].Units.Equals("DOLLAR"))
                                  {%>
                                  <p style="margin-right:5px">                                     
                                    $<%}%>                                                                         
                                     <%=price%> 
                                    
                                     <%}else{%> 
                                                                         
                                    <%=oType.ArrayPrices[m, n].Value%>
                                  </p> 
                                    <%} %>
                                </td>
                              
                           <%} %>
                            
                        <% } %>
                        </tr>
                  <% }
                       } %>
                <%} %>
            </tbody>
        </table>
        <%}%>
        <%}
        %>
        <%} else {  %>
        <br />
       <p style="margin-left:5px"><label class="comp_labels_feature">No pricing data is available for these "<%=ViewData["ProductLabel"]%>"</label></p>
            <% } %>
        <br />  
    
        
    </div>
    <div id="tab4" class="<%=DefaultsFeaturesTab ? "comp_tab_content" : "comp_dn"%>">
        <div id="comp_tools" class="comp_tools">
            <table style="width: 100%" style="width: 100%;" class="elements">
                <thead>
                    <tr>                   
                        <th class="ExportToolsWidht">
                            FILTERING TOOLS
                        </th>
                        <th class="ExportToolsWidht">
                            ADDITIONAL INFORMATION TOOLS
                        </th>
                        <th class="ExportTools">
                            EXPORT TOOLS
                        </th>
                    </tr>
                </thead>
                <colgroup>
                    <col width="35%" />
                    <col width="48%" />
                    <col width="17%" />
                </colgroup>
                <tbody>
                    <tr>
                    <td valign="top">
                       <div style="width:78%;float:left">                  
                            <input type="radio" id="chkboxselprods" name="chktoprods" class="toprodsfeature"
                                onclick="javascript:SwapFeature(1);"  value="sel" checked="checked"/><label for="chkcolorizeon" class="comp_labels_feature">Compare to Selected Products</label><br />
                            <input type="radio" id="chkboxallprods" name="chktoprods" class="toprodsfeature"
                                onclick="javascript:SwapFeature(0);" value="all"/><label for="chkcolorizeoff" class="comp_labels_feature">Compare to All Products for Industry</label><br />
                                
                            <input type="hidden" id="hdnfeatures" />
                            <input type="hidden" id="hdnfeaturesall" />
                            <input type="hidden" id="hdnproductids" />
                            <input type="checkbox" id="chkcritrelhig" class="criteriarelevancy" onclick="ApplyDynamicFilter()"
                                value="<%= Compelligence.Domain.Entity.Resource.CriteriaRelevancy.High %>" /><label for="chkcritrelhig" class="comp_labels_feature">Relevancy High</label><br />
                            <input type="checkbox" id="chkcritrelmed" class="criteriarelevancy" onclick="ApplyDynamicFilter()"
                                value="<%= Compelligence.Domain.Entity.Resource.CriteriaRelevancy.Medium %>" /><label for="chkcritrelmed" class="comp_labels_feature">Relevancy Medium</label><br />
                            <input type="checkbox" id="chkcritrellow" class="criteriarelevancy" onclick="ApplyDynamicFilter()"
                                value="<%= Compelligence.Domain.Entity.Resource.CriteriaRelevancy.Low %>" /><label for="chkcritrellow" class="comp_labels_feature">Relevancy Low</label><br />
                            
                            <div id="colorize">
                               <input type="radio"  id="radcolorizeon" class="colorizefeature" onclick="DoColorize(1)" />
                                 <label class="comp_labels_feature">Colors ON/OFF</label><br />
                            </div>
                            <div>
                                <label for="txtFilter">
                                    Filter :</label>
                                <input id="txtFilter" type="text" value="" style="width: 160px" />
                            </div>
                      
                        </div>
                        <div style="float: left; width: 13%;"><input type="button" id="resetbutton" value="Reset" class='button' onclick="javascript:showConfirmResetFilterDialog()"/></div>
                        </td>
                        <td valign="top">
                            <div style="float:left;width:60%">
                            <input type="checkbox" id="chkindstandard" onclick="javascript:HideColumn(this.checked,2)" /><label class="comp_labels_feature">Display
                                <%=ViewData["IndustryLabel"]%> Standard</label>
                            <br />
                            <%--<input type="checkbox" id="chkenabletools" onclick="javascript:EnableTools(this.checked)" /><label class="comp_labels_feature">Disable Comparinator-Features Edit Tools</label>
                            <br />--%>
                            <input type= "radio" name="SameValues" id="rallvalues" onclick="ApplyDynamicFilter()" /><label class="comp_labels_feature">Show
                                All Values</label>
                            <br />
                            <input type= "radio" name="SameValues" id="ronlysamevalues" onclick="ApplyDynamicFilter()" /><label class="comp_labels_feature">Show
                                Only Same Values</label>
                            <br />
                            <input type= "radio" name="SameValues" id="ronlydiff" onclick="ApplyDynamicFilter()" /><label class="comp_labels_feature">Show
                                Only Differences</label>
                            <br />
                            <input type="checkbox" id="chkhidebenefit" onclick="javascript:HideBenefitColumn(this.checked,this)" /><label class="comp_labels_feature">Display
                                Benefit</label>
                            <br />
                            <input type="checkbox" id="chkhidecost" onclick="javascript:HideCostColumn(this.checked,this)" /><label class="comp_labels_feature">Display
                                Cost</label>
                            <br />
                            </div>
                            <div id="statistics" ></div>
                        </td>
                        <td valign="top" class="ExportTools">
                            <input type="checkbox" id="chkexportfiltered" /><label for="chkexportfiltered" class="comp_labels_feature">Export
                                Filtered List</label><br />
                            <br />
                            <a href="<%= Url.Action("Export", "Comparinator") %>" onclick="javascript: setExportParams(this, '1');">
                                <img alt="Export to CSV format" src="<%= Url.Content("~/Content/Images/Icons/icon_csv.png")%>" width='22px' /></a>
                            <label class="comp_labels_feature"><a href="<%= Url.Action("Export", "Comparinator") %>" onclick="javascript: setExportParams(this, '1');" style="margin-left: -7px;">Export to CSV format</a></label>
                            <br />
                            <br />
                            <a href="<%= Url.Action("ExportToPDF", "Comparinator") %>" onclick="javascript: setExportParams(this, '1');">
                                <img alt="Export to PDF format" src="<%= Url.Content("~/Content/Images/Icons/export_pdf.gif")%>" width='22px' /></a>
                            <label class="comp_labels_feature"><a href="<%= Url.Action("ExportToPDF", "Comparinator") %>" onclick="javascript: setExportParams(this, '1');" style="margin-left: -7px;">Export to PDF format</a></label>        
                            <br />
                            <br />
                            <a href="<%= Url.Action("ExportToExcel", "Comparinator") %>" onclick="javascript: setExportParams(this, '1');">
                                <img alt="Export to Excel format" src="<%= Url.Content("~/Content/Images/Icons/export_excel.png")%>" width='22px' /></a>
                           <label class="comp_labels_feature"><a href="<%= Url.Action("ExportToExcel", "Comparinator") %>" onclick="javascript: setExportParams(this, '1');" style="margin-left: -7px;">Export to XLS format</a></label>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div>
            <div id="LegendName" class="comp_grouphead" style="width: 100%;">
                <p class="comp_groupheadname"> Keys/Legend</p>
            </div>
            <div id="LegenFieldSet" class="comfieldsetlegend" >                              
                <div  class="comp_fhdot"><div class="redcomp bgg" style="margin-top:0px"></div><label style="float:left;"class="comp_labels_feature_legend">Best in Class</label>   </div>             
                <div  class="comp_fhdot"><div class="redcomp bgl" style="margin-top:0px"></div><label style="float:left;"class="comp_labels_feature_legend">Market Advantage</label>   </div>             
                <div  class="comp_fhdot"><div class="redcomp bgw" style="margin-top:0px"></div><label style="float:left;"class="comp_labels_feature_legend">Market Parity</label>   </div>             
                <div  class="comp_fhdot"><div class="redcomp bgp" style="margin-top:0px"></div><label style="float:left;"class="comp_labels_feature_legend">Market Disadvantage</label>   </div>             
                <div  class="comp_fhdot"><div class="redcomp bgr" style="margin-top:0px"></div><label style="float:left;"class="comp_labels_feature_legend">Lagging Market</label> </div>             
             </div>      
            </div>
        </div>
        <div id="comp_tools_button" class="expand_button">
            <div id="comp_tools_button_close"><img src="<%=Url.Content("~/Content/Images/Icons/tools_close.png") %>"
                onclick="update_tools(1)" /> Hide Tools Menu</div>
            <div id="comp_tools_button_show" style="display: none"><img src="<%=Url.Content("~/Content/Images/Icons/tools_show.png") %>"
                onclick="update_tools(0)"/> Show Tools Menu</div>
        </div>
        <% bool drawheaderfilter=true; %> <!--It's for draw unique title -->
        <%foreach (var oHeadProduct in Titles)
          {%>
            <input type="hidden" id="hdn_<%= oHeadProduct.Id %>" class="hdnproductid" value="<%= oHeadProduct.Id %>"/>              
            <!-- Next line was `put because every references to productid  is unrecognized-->
            <input type="hidden" class="lstproducts" value="<%=oHeadProduct.Id %>"/>              
        <%} %>
        <%foreach (var oRow in (IEnumerable<ComparinatorGroup>)ViewData["ComparinatorGroups"])
          {   %>
        <div id="comp_grouphead" class="comp_grouphead">
            <p class="comp_groupheadname">
                <%=oRow.Name%></p>
        </div>
        <% foreach (var oRowTwo in oRow.ComparinatorSets)
           {%>
        <div id="comp_sethead" class="comp_sethead">
            <div class='comp_criteriacollapse'>
            </div>
            <div class="comp_setheadname">
                <%=oRowTwo.Name%></div>
        </div>
        <table id="comp_table_result" class="comp_table filtered">
            <thead class="comp_head">
                <tr>
                    <%--Show rows--%>
                    <th>
                        Criteria
                        <% if (user.SecurityGroupId != "ENDUSER")
                           {%>
                        <span class="comp_add" onclick="showNewCriteriaDialog(this,'<%=oRow.Name%>','<%=oRow.Id%>','<%=oRowTwo.Name%>','<%=oRowTwo.Id%>','<%=Url.Action("SaveCriteria","Comparinator") %>', '<%= u %>','<%=c %>' )"
                            alt="Add Criteria">
                            <img src="<%=Url.Content("~/Content/Images/Icons/add.png") %>" />
                        </span>
                        <%} %>
                    </th>
                    <th>
                        <%=ViewData["IndustryLabel"]%> Standard
                    </th>
                    <%foreach (var oHeadProduct in Titles)
                      {%>
                        <th>
                            <div class="comp_pt">
                                <%=oHeadProduct.Name %>
                            </div>
                            <div class="comp_thc">
                                <a class='comp_thf ' rel='<%=Url.Action("RowHeaderFilter","Comparinator") %>' pid="<%=oHeadProduct.Id %>" ></a>
                            </div>
                        </th>
                    <%}%>
                    <th>
                        Benefit
                    </th>
                    <th>
                        Cost
                    </th>
                </tr>
            </thead>
            <tbody id="tbl<%=oRowTwo.Id %>">
                <% foreach (var oRowThree in oRowTwo.ComparinatorCriterias)
                   { %>
                  <%=Html.CriteriaRow(oRowThree, user, Titles, IndustryId, DefaultsDisabPublComm,DefaultsIndustryStandars,DefaultsBenefit,DefaultsCost,c,u)%>
                <% } %>
            </tbody>
        </table>
        <% } %>
    <% } %>
    </div>
    <div id="tab5" class="<%=DefaultsSalesToolsTab ? "comp_tab_content":"comp_dn" %>">
    <div class="comboBoxContent2" style="padding-left: 5px; padding-right: 5px;">
        <label for="Product" class="comboBoxHeader">
            <%=ViewData["ProductLabel"]%><img src="<%= Url.Content("~/Content/Images/Icons/next.png") %>" alt=":" align="top"/>
        </label><br />
        <%= Html.DropDownList("Product", new MultiSelectList(ProductCollection, "Id", "Name"), new { onchange = "javascript: loadContentToNews('" + Url.Action("SalesToolsProducts", "Comparinator", new { IndustryId = IndustryId, CompetitorId = CompetitorId }) + "&ProductId='+this.value,'"+ViewData["C"]+"','"+ViewData["U"]+"');" })%>
    </div> 
    <br /> 
    <div id="FormSalesTools" style="display:block; margin-left: 5px; margin-right: 5px;">     </div> 
    </div>
    <div id="tab6" class="<%=DefaultsNewsTab ? "comp_tab_content":"comp_dn" %>">
        <div style="margin-left: 5px; margin-right: 5px;">
        <label for="Competitor" class="comboBoxHeader">
            <asp:Literal ID="CompetitorIdByProduct" runat="server" Text="Company"/><img src="<%= Url.Content("~/Content/Images/Icons/next.png") %>" alt=":" align="top"/>
        </label><br />
            <%= Html.DropDownList("CompetitorIdByProduct", (SelectList)ViewData["CompetitorIdByProduct"], null, new { id = "CompetitorIdByProduct", onchange = "GetNewsOfCompetitorByProductSelected(this, this.selected,'" + Url.Action("GetNewsByProduct", "Comparinator") + "','ResultNewsContent')" })%>
        </div><br />
        
        <div id="ResultNewsContent" style="margin-left: 5px;">
        </div>
    </div>
</div>
<div style="display: none;">
<div id="DownloadFileNotFound" title="Error">
    <p>
        <span class="ui-icon ui-icon-alert"></span><strong>File not found...!</strong>
    </p>
</div>
<div id="SuccessSubmitted" title="Message">
    <p>
        <strong>It was successfully submitted!</strong>
    </p>
</div>
</div>
<script type="text/javascript">

    $('#DownloadFileNotFound').dialog({
        bgiframe: true,
        autoOpen: false,
        modal: false,
        buttons: {
            Ok: function() {
                $(this).dialog('close');
            }
        }
    });

    $('#SuccessSubmitted').dialog({
        bgiframe: true,
        autoOpen: false,
        modal: false,
        buttons: {
            Ok: function() {
                $(this).dialog('close');
            }
        }
    });


 
</script>
<script type="text/javascript">


    function UpdateHowWePositionByProduct() {
        GetHowWePositionByProduct('<%= Url.Action("GetHowWePosition", "Comparinator") %>', '<%= ViewData["CompetitorProductIds"] %>', '<%= ViewData["C"] %>');
    }

    $(document).ready(function() {

        $("#txtFilter").keyup(function() {
            ApplyDynamicFilter();
        });
        CollapseCriteriaSet();
        CollapsePricingSet();
        HideColumn(false, 2); //Industry Standard ?

        var table = $('#comp_table_result'); //table.size() ~ table.length
        if (table.length > 0) {
            var numberColumn = GetNumberOfColumn(table[0]);
            HideColumn(false, numberColumn);
        }
        initializeStatistics();
        var rowseq = $('.comp_eq');
        var allrows = $(".filtered tbody tr");
        updateStatistics(rowseq.size(), allrows.size());
        var competitorId = '<%= ViewData["ClientCompetitorId"] %>';
        $('#CompetitorIdByProduct').val(competitorId);
        GetNewsOfCompetitorByDefault(competitorId, '<%= Url.Action("GetNewsByProduct", "Comparinator")%>', 'ResultNewsContent');
        ResizeWithOfDivHead();
        DisplayFirstRowFeature();
    });

    $(document).ready(function() {
        ChangeImageSRC();
        UpdatedOptionsToFilter();
        UpdateRowsByConfig();
    });

    var colorizeon; //important for colorize, not remove
    $(document).ready(function() {
        //required for colorize, not remove
        colorizeon = $("#radcolorizeon").is(":checked");

        $('.AddTargetBlankToLin a').attr('target', '_blank');
        $('.contentBoxDataList a').attr('target', '_blank');

    });

    $(document).ready(function() {
        $(".comp_tab_content").hide();
        $("ul.comp_tabs li:first").addClass("comp_active").show();
        $(".comp_tab_content:first").show();

        $("ul.comp_tabs li").click(function() {
            $("ul.comp_tabs li").removeClass("comp_active");
            $(this).addClass("comp_active");
            $(".comp_tab_content").hide();
            var activeTab = $(this).find("a").prop("target");
            //$(activeTab).show();
            //$(activeTab).attr("style", "display:inline-block");
            $(activeTab).attr("style", "display:inline");

            return false;
        });

        update_tools(1);
        //Next line was disable because at show result not require show Salestool
        loadContentToNews('<%=Url.Action("SalesToolsProducts", "Comparinator", new { IndustryId = IndustryId, CompetitorId = CompetitorId, ProductId=ProductId })%>', '<%=ViewData["C"]%>', '<%=ViewData["U"] %>');
        //$("#Product").prepend("<option value=''></option>").val(''); //used for Salestool

        UpdateHowWePositionByProduct();
        updateDefaults();
    });


    function startPopupsValues() {
        $('#chkcolorizeoff').prop('checked', false);
        colorizeFeatureForProducts(0);
        SetValueCorner('table div.cp', '<%=Url.Action("CellDetail", "Comparinator") %>', '<%=ViewData["IndustryId"] %>', '<%=ViewData["U"] %>', '<%=ViewData["C"] %>', '<%=DefaultsDisabPublComm%>');
    }



    function startPopupsHeads() {
        $('a.comp_thf').cluetip({ ajaxCache: false, width: '200px', closePosition: 'title', showTitle: false, cluetipClass: 'jtip', mouseOutClose: false, activation: 'click', sticky: true, arrows: true, cursor: 'pointer', urlParser: function(link) {
            var pid = $(this).attr("pid");
            //search current setting for header filter by current product
            var features = '';
            var lstproducts = $(".lstproducts");
            for (q = 0; q < lstproducts.size(); q++) {
                var fvalue = $(lstproducts[q]).data("header_filter");
                //console.log(fvalue.features);
                if (fvalue.id == pid) {
                    features = $.param({ features: fvalue.features });
                    break;
                }
            }
            if (features == []) features = $.param({ features: ["NF"] }); ;

            //Count features by column
            var pid = $(this).attr("pid");
            var bc = 0, ma = 0, mp = 0, md = 0, lm = 0;
            var currcol = $(".comp_table tbody tr td[id='P" + pid + "']");
            currcol.each(function(index, element) {
                var curr_td = $(element);
                if (!curr_td.parent().hasClass("hidden")) {
                    if (curr_td.hasClass("BC")) bc++;
                    if (curr_td.hasClass("MA")) ma++;
                    if (curr_td.hasClass("MP")) mp++;
                    if (curr_td.hasClass("MD")) md++;
                    if (curr_td.hasClass("LM")) lm++;
                }

            });

            return link + '?pid=' + pid + '&' + features + '&BC=' + bc + '&MA=' + ma + '&MP=' + mp + '&MD=' + md + '&LM=' + lm;
        },
            onShow: function() {
                $(document).click(function(e) {
                    var isInClueTip = $(e.target).closest('#cluetip');
                    if (isInClueTip.length === 0) {
                        $(document).trigger('hideCluetip');
                    }
                })
            }
        });
    }


    //start corners settings
    startPopupsHeads();

    startPopupsValues();

    SetRelevancyCorner('div.relevancy', '<%=Url.Action("CellRelevancy", "Comparinator") %>');
    SetBenefitCorner('table div.benefit', '<%=ViewData["IndustryId"] %>');
    SetCostCorner('table div.cost', '<%=ViewData["IndustryId"] %>');
    SetIndustryStandarCorner('table div.indstd', '<%=ViewData["IndustryId"] %>');



    //Setting header filters buffer, Not Change
    $("#comp_tools").data("header_filter", []);
    var iproducts = $(".lstproducts");
    for (ip = 0; ip < iproducts.size(); ip++) {
        var productid = iproducts[ip].value;
        $(iproducts[ip]).data("header_filter", { id: productid, features: [] });
    }


    //temporally
    var silverbullets_update_flag = 0;
    function silverbullets_update() {
        var url = '<%=Url.Action("SilverBullets","Comparinator") %>';
        silverbullets_update_flag++;
        //if (silverbullets_update_flag > 1) return; //it's control run unique time

        $.getJSON(url, {}, function(data) {
            if (data != "") {
                //G1
                //Section 1
                for (b = 0; b < data.g1.length; b++) {
                    var criterias_bc = data.g1[b].Value;
                    var approw = "";
                    for (br = 0; br < criterias_bc.length; br++) {
                        if (criterias_bc[br].benefit.length > 0) {
                            approw += "<tr>"
                            approw += "<td style='width:300px'><li>" + criterias_bc[br].benefit + "</li></td>";
                            approw += "</tr>"
                        }
                    }
                    if (approw.length == 0) {
                        $("#sb_content_s1 #sb_" + data.g1[b].Key + " tbody").html("<tr><td style='width:350px'><li><%=SilverBulletsSections.NoSilverBullets %></li></td></tr>");
                        $('#sb_content_s1 #div_sb_' + data.g1[b].Key).addClass('disnone');
                    }
                    else
                        $("#sb_content_s1 #sb_" + data.g1[b].Key + " tbody").empty();
                    $("#sb_content_s1 #sb_" + data.g1[b].Key + " tbody").append(approw);
                }

                //G1
                //Section 2
                for (b = 0; b < data.g1.length; b++) {
                    var criterias_bc = data.g1[b].Value;
                    var approw = "";
                    for (br = 0; br < criterias_bc.length; br++) {
                        if (criterias_bc[br].benefit.length > 0) {
                            approw += "<tr>"
                            approw += "<td style='width:300px'><li>" + criterias_bc[br].criteria + "</li></td>";
                            approw += "</tr>"
                        }
                    }
                    if (approw.length == 0) {
                        $("#sb_content_s2 #sb_" + data.g1[b].Key + " tbody").html("<tr><td style='width:350px'><li><%=SilverBulletsSections.NoSilverBullets %></li></td></tr>");
                        $('#sb_content_s2 #div_sb_' + data.g1[b].Key).addClass('disnone');
                    }
                    else
                        $("#sb_content_s2 #sb_" + data.g1[b].Key + " tbody").empty();
                    $("#sb_content_s2 #sb_" + data.g1[b].Key + " tbody").append(approw);
                }
                //G2
                //Section 3

                for (b = 0; b < data.g2.length; b++) {
                    var criterias_bc = data.g2[b].Value;
                    var approw = "";
                    for (br = 0; br < criterias_bc.length; br++) {
                        if (criterias_bc[br].benefit != "") {
                            approw += "<tr>"
                            approw += "<td style='width:300px'><li>" + criterias_bc[br].benefit + "</li></td>";
                            approw += "</tr>"
                        }
                    }
                    if (approw.length == 0) {
                        $("#sb_content_s3 #sb_" + data.g2[b].Key + " tbody").html("<tr><td style='width:350px'><li><%=SilverBulletsSections.NoSilverBullets %></li></td></tr>");
                        $('#sb_content_s3 #div_sb_' + data.g2[b].Key).addClass('disnone');
                    }
                    else
                        $("#sb_content_s3 #sb_" + data.g2[b].Key + " tbody").empty();
                    $("#sb_content_s3 #sb_" + data.g2[b].Key + " tbody").append(approw);
                }


                //G2
                //Section 4
                for (b = 0; b < data.g2.length; b++) {
                    var criterias_bc = data.g2[b].Value;
                    var approw = "";
                    for (br = 0; br < criterias_bc.length; br++) {
                        if (criterias_bc[br].cost != "") {
                            approw += "<tr>"
                            approw += "<td style='width:300px'><li>" + criterias_bc[br].cost + "</li></td>";
                            approw += "</tr>"
                        }
                    }
                    if (approw.length == 0) {
                        $("#sb_content_s4 #sb_" + data.g2[b].Key + " tbody").html("<tr><td style='width:350px'><li><%=SilverBulletsSections.NoSilverBullets %></li></td></tr>");
                        $('#sb_content_s4 #div_sb_' + data.g2[b].Key).addClass('disnone');
                    }
                    else
                        $("#sb_content_s4 #sb_" + data.g2[b].Key + " tbody").empty();
                    $("#sb_content_s4 #sb_" + data.g2[b].Key + " tbody").append(approw);
                }
            } //if
        });
    }
     
</script>
