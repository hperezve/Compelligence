  function DynamicTextResume(str)
  {
    return str.substring(0,100)+"...";
  }
  function DynamicText(source,id,flag,content)
  {
   var togButtons=$("#txt"+id);
   if ( flag == 0 )
     {     
       togButtons.html(DynamicTextResume(content));
       source.onclick=function() { DynamicText(source,id,1,content); };
     }
     else
     {
       togButtons.html(content);
       source.onclick=function() { DynamicText(source,id,0,content); };
     }
  }