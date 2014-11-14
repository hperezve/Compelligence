<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    var showSendingDialog = function() {

        $.blockUI({ message: $('#SendingDialog'),
            css: { width: '20%', margin: 'auto' }
        });
    };
    var hideSendingDialog = function() {
        $.unblockUI();
    };
    $(function() {
        NewsletterSubtabs = new Ext.TabPanel({
            renderTo: 'NewsletterContent',
            autoWidth: true,
            frame: true,
            defaults: { autoHeight: true },
            listeners: {
                render: function(tabPanel) {
                    hideSubtabs(tabPanel);
                }
            },
            items: [
                    { contentEl: '<%= ViewData["Scope"] %>NewsletterEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>NewsletterEditFormContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Newsletter</u> > Header";
                        //getEntity('<%= Url.Action("Edit", "Newsletter") %>', '<%= ViewData["Scope"] %>', 'Newsletter', id, 'NewsletterAll','#NewsletterContent');
                        }
                        }
                    }
                    /*{ contentEl: '<%= ViewData["Scope"] %>NewsletterEditorFormContent', title: 'Html Editor', id: '<%= ViewData["Scope"] %>NewsletterEditorFormContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Newsletter</u>";
                        var id = getIdValue('<%= ViewData["Scope"] %>', 'Newsletter');
                        loadContent('<%= Url.Action("Editor", "Newsletter") %>' + '?id=' + id + '&BrowseId=' + 'NewsletterAll', '#<%= ViewData["Scope"] %>NewsletterEditorFormContent', 'Workspace');
                        }
                        }
                    }*/
                ]
        });
        resizeContent('#<%= ViewData["Scope"] %>NewsletterList');
    });
</script>
<script type="text/javascript">
    function NewsPreview(urlAction) {
        var id = getIdValue('<%= ViewData["Scope"] %>', 'Newsletter');

        openPopup(urlAction + '/' + id.toString());
    }
    function NewsSend(urlAction) {
        var id = getIdValue('<%= ViewData["Scope"] %>', 'Newsletter');
        if (!id) //if not have id, show alert and return
        {
            showAlertSelectItemDialog();
            return;
        }
        
        showSendingDialog();
        $.get(urlAction + '/' + id.toString(), {}, function(data) {
            if (data == 'Success') {
                
            }
            else {
                $('#SuccessMessageNewsletter').empty();
                $('#SuccessMessageNewsletter').html(data);
            }
            hideSendingDialog();
            $("#SuccessMessageNewsletter").dialog({
                bgiframe: true,
                autoOpen: false,
                modal: true,
                title: 'Send Newsletter',
                close: function() { hideSendingDialog(); },
                buttons: { Ok: function() { $(this).dialog('close'); hideSendingDialog(); } }
            });

            $('#SuccessMessageNewsletter').dialog("open");
        });
        
    }
    
    function getIdNewsletter() {
        return formId = $('#Id').val();
//        resizeContent('#<%= ViewData["Scope"] %>NewsletterList');
    }
</script>

<asp:Panel ID="NewsletterListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>NewsletterList" class="indexOne">
    <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="NewsletterFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>NewsletterEditFormContent" class="x-hide-display" />
    <!--<div id="<%= ViewData["Scope"] %>NewsletterEditorFormContent" class="x-hide-display" />-->
</asp:Panel>
<div id="SendingDialog" class="displayNone">
            <p>
                <img src="<%= Url.Content("~/Content/Images/Ajax/loader.gif") %>" alt=""
                    class="left" /><span class="loadingDialog">Sending ...</span>
            </p>
</div>
