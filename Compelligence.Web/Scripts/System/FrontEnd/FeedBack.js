
function updateTips(target,t) 
{
	var tips = $("#validateTips");
	tips.text(t).effect("highlight",{},1500);
}

function checkLength(target,o,n,min,max) 
{

	if ( o.val().length > max || o.val().length < min ) 
	{
		o.addClass('ui-state-error');
		if( o.val().length < min)
		  updateTips(target,"Length of " + n + " is less than "+min+".");
		else
		  updateTips(target,"Length of " + n + " is greater than "+max+".");
		
		return false;
	} else {
		return true;
	}

}

function checkRegexp(target,o,regexp,n) 
{

	if ( !( regexp.test( o.val() ) ) ) {
		o.addClass('ui-state-error');
		updateTips(target,n);
		return false;
	} else {
		return true;
	}

}

function setClassOnFocus(txtArea) {
    txtArea.addClass('textareadialog');
}

function FeedBackFormDlg(urlAction, dlgTitle) {
   
  if (dlgTitle == null) 
    dlgTitle = "FeedBack Dialog";
  
  var dlg = $("#FormFeedBack");
 
  dlg.dialog({
    autoOpen: false,
       title: dlgTitle,
      height: 240,
       width: 360,
       modal: true,
     buttons: 
        {
            'Close': function() 
             {
                $(this).dialog('close');
             },
             'Send': function() 
            {
                var bValid = true;
                var txtComment = $("#txtComment");
                var tips = $("#validateTips");

                txtComment.removeClass('ui-state-error');

                bValid = bValid && checkLength(tips, txtComment, "Comment", 3, 255);
                bValid = bValid && checkRegexp(tips, txtComment, /^[a-z]|[0-9]|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]+$/i, "Comment accept only alfabetic values.");

                if (bValid) {
                    //Send FeedBack
                    $.post(urlAction, {Comment:txtComment.val()});
                    $(this).dialog('close');
                }
            }
        },
        close: function() {
            var tips = $("#validateTips");
            var txtComment = $("#txtComment");
            txtComment.val("").removeClass('ui-state-error');
            tips.html("");
        }
    });
	//End-Dialog defined
	DialogContent = '<strong id="validateTips"></strong>'+
	        '<form>'+'<label for="txtComment">Comment</label>'+
            '<textarea name="txtComment" id="txtComment" WRAP=SOFT class="textareadialog text ui-widget-content ui-corner-all" COLS=50 ROWS=4 onClick="setClassOnFocus(this)"></textarea>' +
	        '</form>';
	dlg.html(DialogContent);
	dlg.dialog('open');
    return false;
}

function FeedBackFormSalesDlg(urlAction, dlgTitle, C, U) {

    if (dlgTitle == null)
        dlgTitle = "FeedBack Dialog";

    var dlg = $("#FormFeedBack");

    dlg.dialog({
        autoOpen: false,
        title: dlgTitle,
        height: 240,
        width: 360,
        modal: true,
        buttons:
        {
            'Close': function() {
                $(this).dialog('close');
            },
            'Send': function() {
                var bValid = true;
                var txtComment = $("#txtComment");
                var hdnC = $("#hdnC");
                var hdnU = $("#hdnU");
                var tips = $("#validateTips");

                txtComment.removeClass('ui-state-error');

                bValid = bValid && checkLength(tips, txtComment, "Comment", 3, 255);
                bValid = bValid && checkRegexp(tips, txtComment, /^[a-z]|[0-9]|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]+$/i, "Comment accept only alfabetic values.");

                if (bValid) {
                    //Send FeedBack
                    $.post(urlAction, { Comment: txtComment.val(), C: hdnC.val(), U: hdnU.val() });
                    $(this).dialog('close');
                }
            }
        },
        close: function() {
            var tips = $("#validateTips");
            var txtComment = $("#txtComment");
            txtComment.val("").removeClass('ui-state-error');
            tips.html("");
        }
    });
    //End-Dialog defined
    DialogContent = '<strong id="validateTips"></strong>' +
	        '<form>' + '<label for="txtComment">Comment</label>' +
            '<textarea name="txtComment" id="txtComment" WRAP=SOFT class="textareadialog text ui-widget-content ui-corner-all" COLS=50 ROWS=4 onClick="setClassOnFocus(this)"></textarea>' +
	        '<input type="hidden" value="' + C + '" name="hdnC" id="hdnC"/>' +
	        '<input type="hidden" value="' + U + '" name="hdnU" id="hdnU" />' +
	        '</form>';
    dlg.html(DialogContent);
    dlg.dialog('open');
    return false;
}

function FeedBackToEntityFormDlg(urlAction, dlgTitle, entityId, entityType) {

    if (dlgTitle == null)
        dlgTitle = "FeedBack Dialog";

    var dlg = $("#FormFeedBack");

    dlg.dialog({
        autoOpen: false,
        title: dlgTitle,
        height: 240,
        width: 360,
        modal: true,
        buttons:
        {
            'Close': function() {
                $(this).dialog('close');
            },
            'Send': function() {
                var bValid = true;
                var txtComment = $("#txtComment");
                var tips = $("#validateTips");
                var hdnEntityId = $("#hdnEntityId");
                var hdnEntityType = $("#hdnEntityType");
                txtComment.removeClass('ui-state-error');

                bValid = bValid && checkLength(tips, txtComment, "Comment", 3, 255);
                bValid = bValid && checkRegexp(tips, txtComment, /^[a-z]|[0-9]|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]+$/i, "Comment accept only alfabetic values.");

                if (bValid) {
                    //Send FeedBack
                    $.post(urlAction, { EntityId: hdnEntityId.val(), EntityType: hdnEntityType.val(), Comment: txtComment.val() });
                    $(this).dialog('close');
                }
            }
        },
        close: function() {
            var tips = $("#validateTips");
            var txtComment = $("#txtComment");
            txtComment.val("").removeClass('ui-state-error');
            tips.html("");
        }
    });
    //End-Dialog defined
    DialogContent = '<strong id="validateTips"></strong>' +
	        '<form>' + '<label for="txtComment">Comment</label>' +
            '<textarea name="txtComment" id="txtComment" WRAP=SOFT class="textareadialog text ui-widget-content ui-corner-all" COLS=50 ROWS=4 onClick="setClassOnFocus(this)"></textarea>' +
	        '<input type="hidden" value="' + entityId + '" name="hdnEntityId" id="hdnEntityId"/>' +
	        '<input type="hidden" value="' + entityType + '" name="hdnEntityType" id="hdnEntityType" />' +
	        '</form>';
    dlg.html(DialogContent);
    dlg.dialog('open');
    return false;
}

function FeedBackToEntityWithIndustryFormDlg(urlAction, dlgTitle, entityId, entityType, industryId) {

    if (dlgTitle == null)
        dlgTitle = "FeedBack Dialog";

    var dlg = $("#FormFeedBack");

    dlg.dialog({
        autoOpen: false,
        title: dlgTitle,
        height: 240,
        width: 360,
        modal: true,
        buttons:
        {
            'Close': function() {
                $(this).dialog('close');
            },
            'Send': function() {
                var bValid = true;
                var txtComment = $("#txtComment");
                var tips = $("#validateTips");
                var hdnEntityId = $("#hdnEntityId");
                var hdnIndustryId = $("#hdnIndustryId");
                var hdnEntityType = $("#hdnEntityType");
                txtComment.removeClass('ui-state-error');

                bValid = bValid && checkLength(tips, txtComment, "Comment", 3, 255);
                bValid = bValid && checkRegexp(tips, txtComment, /^[a-z]|[0-9]|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]+$/i, "Comment accept only alfabetic values.");

                if (bValid) {
                    //Send FeedBack
                    $.post(urlAction, { EntityId: hdnEntityId.val(), EntityType: hdnEntityType.val(), Comment: txtComment.val(), IndustryId: hdnIndustryId.val() });
                    $(this).dialog('close');
                }
            }
        },
        close: function() {
            var tips = $("#validateTips");
            var txtComment = $("#txtComment");
            txtComment.val("").removeClass('ui-state-error');
            tips.html("");
        }
    });
    //End-Dialog defined
    DialogContent = '<strong id="validateTips"></strong>' +
	        '<form>' + '<label for="txtComment">Comment</label>' +
            '<textarea name="txtComment" id="txtComment" WRAP=SOFT class="textareadialog text ui-widget-content ui-corner-all" COLS=50 ROWS=4 onClick="setClassOnFocus(this)"></textarea>' +
	        '<input type="hidden" value="' + entityId + '" name="hdnEntityId" id="hdnEntityId"/>' +
	        '<input type="hidden" value="' + industryId + '" name="hdnIndustryId" id="hdnIndustryId"/>' +
	        '<input type="hidden" value="' + entityType + '" name="hdnEntityType" id="hdnEntityType" />' +
	        
	        '</form>';
    dlg.html(DialogContent);
    dlg.dialog('open');
    return false;
}

function SalesForceFeedDlg(urlAction,urlSave) {

    var dlg = $("#FormFeedBack");

    dlg.dialog({
        autoOpen: false,
        title: "FeedBack Dialog",
        height: 240,
        width: 360,
        modal: true,
        buttons:
        {
            'Close': function() {
                $(this).dialog('close');
            },
            'Send': function() {
                var bValid = true;
                var frmFeedBack = $("#frmFeedBack");
                var txtComment = frmFeedBack.find("#txtComment");
                var tips = frmFeedBack.find("#validateTips");

                txtComment.removeClass('ui-state-error');

                bValid = bValid && checkLength(tips, txtComment, "Comment", 3, 255);
                bValid = bValid && checkRegexp(tips, txtComment, /^[a-z]|[0-9]|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]+$/i, "Comment accept only alfabetic values.");

                if (bValid) {
                    //Send FeedBack
                    $.post(urlSave, frmFeedBack.serialize());
                    $(this).dialog('close');
                }
            }
        },
        close: function() {
            var tips = $("#validateTips");
            var txtComment = $("#txtComment");
            txtComment.val("").removeClass('ui-state-error');
            tips.html("");
        }
    });
    //End-Dialog defined
    $.get(urlAction, {}, function(data) {
       dlg.html(data);
       dlg.dialog('open');
     });
}

function FollowerForm(urlAction, entityid, email, name, dlgTitle) {
    if (dlgTitle == null)
        dlgTitle = "Followers";

    var dlg = $("#FrmFollowers");
    dlg.dialog({
        autoOpen: false,
        title: dlgTitle,
        height: 360,
        width: 310,
        modal: true,
        buttons:
        {
            'Close': function() {
                $(this).dialog('close');
            },
            'Subscribe': function() {
                var bValid = true;
                var txtName = $("#txtName");
                var txtEmail = $("#txtEmail");
                var hdId = $("#hdId");
                var tips = $("#validateTips");
                var operation = 'Subscribe';
                //var re  = /^([a-zA-Z0-9_.-])+@(([a-zA-Z0-9-])+.)+([a-zA-Z0-9]{2,4})+$/;
                var re= /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;                
                bValid = bValid && checkRegexp(tips, txtEmail, re, "Email is not valid.");
                if (bValid) {
                    $.post(urlAction, { Name: txtName.val(), Email: txtEmail.val(), EntityId: hdId.val(), Operation: operation });
                    $(this).dialog('close');
                }
            },
            'Unsubscribe': function() {
                var bValid = true;
                var txtName = $("#txtName");
                var txtEmail = $("#txtEmail");
                var hdId = $("#hdId");
                var tips = $("#validateTips");
                var operation = 'Unsubscribe';
                //var re = /^([a-zA-Z0-9_.-])+@(([a-zA-Z0-9-])+.)+([a-zA-Z0-9]{2,4})+$/;
                var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;                
                bValid = bValid && checkRegexp(tips, txtEmail, re, "Email is not valid.");
                if (bValid) {
                    $.post(urlAction, { Name: txtName.val(), Email: txtEmail.val(), EntityId: hdId.val(), Operation: operation });
                    $(this).dialog('close');
                }
            }
        },
        close: function() {
            var tips = $("#validateTips");
            var txtName = $("#txtName");
            txtName.val("").removeClass('ui-state-error');
            tips.html("");
        }
    });
    DialogContent = '<strong id="validateTips"></strong>' +
	                '<form>' + '<table  border="0">'+
	                '<tr><td>Name</td><td>:<input type="text" name="txtName" id="txtName" value="' + name + '" /><input type="hidden" id="hdId" value="' + entityid + '" name="Hidden1" /></td></tr>' +
	                '<tr><td>E-Mail</td><td>:<input type="text" name="txtEmail" id="txtEmail" value="' + email + '" /></td></tr>' +
                    '</table></form>';
    dlg.html(DialogContent);
    dlg.dialog('open');
    return false;
}	