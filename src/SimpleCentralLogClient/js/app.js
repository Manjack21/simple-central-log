const baseurl = "http://localhost:8080/";

var html = {
    smallGrey : (text) => '<span class="w3-small w3-text-grey">' + text + '</span>',
    paleRed : (text) => '<span class="w3-container w3-pale-red">' + text + '</span>',
    span : (text, classes = "") => '<span class="' + classes + '">' + text + '</span>',
    row : (text, classes = "") => '<div class="w3-row w3-small w3-border-bottom ' + classes + '">' + text + '</div>',
    nl : () => '<br />',
    colRest : (text) => '<div class="w3-rest">' + text + '</div>',
    col : (text, classes = "", style = "") => '<div class="w3-col ' + classes + '" style="' + style + '">' + text + '</div>',
    closeButtonLevel2 : () => '<span onclick="this.parentElement.parentElement.style.display=\'none\'" style="float:right;">&times;</span>',
    xlarge : (inner) => '<span class="w3-xlarge">' + inner + '</span>',
    
    entryBackgroundClass : (messageClass) => {
        switch (messageClass) {
            case 1: return "w3-pale-red";
            default: return "";
        }
    },

    messageclassIcon : function(messageclass) {
        switch (messageclass) {
            case 1: return '<i class="fa fa-times-circle"></i>';
            case 2: return '<i class="fa fa-exclamation-triangle"></i>';
            case 3: return '<i class="fa fa-info"></i>';
            default: return '';
        }
    },
    getFilterClasses : () => Array.from(document.getElementsByName('filterClass'))
        .reduce((total, item, index) => { if(item.checked) total.push(item.value); return total;}, [])
}

async function queryLogs(query) {
    var response = await fetch(baseurl + "v1/query", 
        {method: 'POST',
        body : JSON.stringify(query)});
    var logs = await response.json();
    return logs
}

function loadDailyErrors() {
    return queryLogs({Messageclass : ["Error"]})
}

function displayErrors(errors) {
    var errorHtml = html.smallGrey('last update: ' + (new Date()).toLocaleTimeString()) +
         errors.map(errorHtmlString).join(" ");
    document.getElementById("errors").innerHTML = errorHtml;
}

async function displayDailyErrors() {
    var errors = await loadDailyErrors();
    displayErrors(errors);
}

function errorHtmlString(error) {
    return ("<div class='w3-card-2 w3-pale-red w3-margin-bottom'>\n" + 
            "<header class='w3-container w3-red w3-color-white'>Application: <strong>{0}</strong><span class='w3-small w3-margin-left'>{2}</span>" +
            html.closeButtonLevel2() + "</header>\n" +
            "<div class='w3-container w3-padding'>" + html.xlarge(html.messageclassIcon(error.Class)) + " {1}</div>\n" + 
        "</div>")
        .replace("{0}", error.Application)
        .replace("{1}", error.Message)
        .replace("{2}", new Date(Date.parse(error.Logdate)).toLocaleTimeString());
}

async function displayFilteredEntries() {
    var entries = await filteredEntries();

    var entriesHtml = entries.map((entry) => html.row(        
            html.col(html.xlarge(html.messageclassIcon(entry.Class)), "w3-center", "width:40px;") +
            html.col(entry.Application + html.nl() + (new Date(Date.parse(entry.Logdate))).toLocaleString(), "s3 m3 l2") +
            html.colRest(entry.Message)
        , html.entryBackgroundClass(entry.Class))        
    );
    document.getElementById("entries").innerHTML = entriesHtml.join(" ")
}

function filteredEntries(){
    var Messageclass = html.getFilterClasses();
    var query = {
        Application: document.getElementById("Application").value,
        Message: document.getElementById("Message").value
    }
    if (Messageclass.length > 0) query.Messageclass = Messageclass;

    return queryLogs(query);
}



displayDailyErrors();
window.setInterval(displayDailyErrors, 10000);
