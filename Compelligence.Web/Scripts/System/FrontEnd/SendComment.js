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

function CommentDlg(urlForm,urlAction, EntityId,ParentResponseId,dlgTitle) 
{
  if (dlgTitle == null) 
    dlgTitle = "Comment Response";
  
  var dlg = $("#CommentForm");
  
  dlg.dialog({
    autoOpen: false,
       title: dlgTitle,
      height: 220,
       width: 460,
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
                var txtComment = $("#txtresponse");
                var tips = $("#validateTips");

                txtComment.removeClass('ui-state-error');

               // bValid = bValid && checkLength(tips, txtComment, "Response", 3, 255);
               // bValid = bValid && checkRegexp(tips, txtComment, /^[a-z]|[0-9]|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]+$/i, "Comment accept only alfabetic values.");

                if (bValid) {
                     $.ajax({  
                       type: "POST",  
                       url: urlAction,  
                       data: "response="+txtComment.val()+"&Id="+EntityId+"&ParentResponseId="+ParentResponseId,  
                       success: function() {  location.href=urlForm; }  
                    }); 
                    
                    $(this).dialog('close');
                }
            }
        },
        close: function() {
            var tips = $("#validateTips");
            var txtComment = $("#response");
            txtComment.val("").removeClass('ui-state-error');
            tips.html("");
        }
    });
	//End-Dialog defined
	DialogContent = '<strong id="validateTips"></strong>'+
	        '<form>'+
            '<textarea name="txtresponse" id="txtresponse" WRAP=SOFT class="text ui-widget-content ui-corner-all" style="width:420px"></textarea>'+
	        '</form>';
	dlg.html(DialogContent);


   //$.get(urlForm,{},function(data) //Close
    //dlg.html(data);
 	dlg.dialog('open');

    return false;
}	
	
