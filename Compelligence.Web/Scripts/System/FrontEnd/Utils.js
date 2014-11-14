//String Functions
function Alltrim(value) {
    return value.replace(/^\s+|\s+$/g, '');
}

//Url Functions
function isUrl(pUrl) {
    return pUrl.match(/^(http|ftp)\:\/\/\w+([\.\-]\w+)*\.\w{2,4}(\:\d+)*([\/\.\-\?\&\%\#\=]\w+)*\/?$/i);
}
function isUrls(pUrls) {
    var aUrls = Alltrim(pUrls).split(" ");
    var vurls = 0;
    for (var i = 0; i < aUrls.length; i++) {
        vurls += isUrl(aUrls[i]) ? 0 : 1;
    }
    return vurls == 0;
}

//Helper for string link  to <a> tag
function ToLinks(Urls) 
{
      var Result = "";
      var allUrl = Urls.split(" ");
      for (var i = 0; i < allUrl.length; i++) 
      {
          if ( allUrl[i].length>7 )
            Result += "<a href='" + allUrl[i] + "' target='_blank'>" + allUrl[i] + "</a><br />";
      }
      return Result;
}

//Showing Functions
function showLoadingDialog() 
{
    var content = $("<div><img src='/Content/Images/Ajax/loader.gif' alt='' class='float-left'/><p class='paddingTop10'>Loading ...</p></div>");
    $.blockUI({ message: content, css: { width: '20%', margin: 'auto'} });
}

function hideLoadingDialog() {
    $.unblockUI();
}
function convertTextPlainEnconde(str) {
    return str
      .replace(/&/g, "&amp;")
      .replace(/</g, "&lt;")
      .replace(/>/g, "&gt;")
      .replace(/"/g, "&quot;")
      .replace(/'/g, "&#039;");
}
function convertTextPlain(str) {
    return str
      .replace("/b&gt;", "")
      .replace("b&gt;", "")
      .replace("/tr&gt;", "")
      .replace("tr&gt;", "")
      .replace("bold&gt;", "")
      .replace("/bold&gt;", "")
      .replace("/h1&gt;", "")
      .replace("h1&gt;", "")
      .replace("td&gt;", "")
      .replace("/td&gt;", "")
      .replace("li&gt;", "")
      .replace("/li&gt;", "")
      .replace("/div&gt;", "")
      .replace("div&gt;", "")
      .replace("/p&gt;", "")
      .replace("p&gt;", "")
      .replace("script&gt;", "")
      .replace("/script&gt;", "")
      .replace("style&gt;", "")
      .replace("/style&gt;", "")
      .replace("/style&gt;", "")
      .replace("/br&gt;", "")
      .replace("br&gt;", "")
      .replace("&amp;", "")
      .replace("&lt;", "")
      .replace("&gt;", "")
      .replace("&quot;", "")
      .replace("&#039;", "")
      .replace("&lt;", "");   
}
function convertTextPlainHtml(str) {
    var s = str;
    var result = "";
    $.each($(s), function(i) {
        result += " " + $(this).html();
    });
    if (result.length > 1) {
        str = result;
        return str
    }
    else {
        str = convertTextPlainEnconde(str);
        str = convertTextPlain(str);
        return str;
    }
}


//Function push inner Array methods, initial usage on comparisons.js

Array.prototype.destroy = function(obj) {
    // Return null if no objects were found and removed
    var destroyed = null;

    for (var i = 0; i < this.length; i++) {

        // Use while-loop to find adjacent equal objects
        while (this[i] === obj) {

            // Remove this[i] and store it within destroyed
            destroyed = this.splice(i, 1)[0];
        }
    }

    return destroyed;
}