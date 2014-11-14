<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script language="javascript">
    Ext.onReady(function() {
        //parent tabs
        var tabs = new Ext.TabPanel({
            renderTo: 'tabsContainer',
            autoWidth:true,//width: 742,
            height: 688,
            activeTab: 0,
            frame: true,
            defaults: { autoHeight: true },
            items: [
            { contentEl: 'CriteriaContent', title: 'Criteria',
                listeners: { activate: function() {
                document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Criterias</u> > Criteria";
                loadContent ('<%= Url.Action("Index", "Criteria") %>', '#CriteriaContent', 'Environment')                
                } }
            },
            { contentEl: 'CriteriaSetContent', title: 'CriteriaSet',
                listeners: { activate: function() {
                document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Criterias</u> > Criteria Set";
                loadContent ('<%= Url.Action("Index", "CriteriaSet") %>', '#CriteriaSetContent', 'Environment')                      
                } }
            },
            { contentEl: 'CriteriaGroupContent', title: 'CriteriaGroup',
                listeners: { activate: function() {
                    document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Criterias</u> > Criteria Group";
                    loadContent('<%= Url.Action("Index", "CriteriaGroup") %>', '#CriteriaGroupContent', 'Environment')
                    //alert('CriteriaGroup') 
                } 
                }
            }
            ]
        });
    });
</script>

<script type="text/javascript">
    var CriteriaSetSubtabs;
    var CriteriaSubtabs;    
    var CriteriaGrouptabs;
</script>
<div id="tabsContainer">
                    <div id="CriteriaContent" class="x-hide-display heightPanel">
                    </div> 
                    <div id="CriteriaSetContent" class="x-hide-display heightPanel">
                    </div>
                    <div id="CriteriaGroupContent" class="x-hide-display heightPanel">
                    </div>
</div>
