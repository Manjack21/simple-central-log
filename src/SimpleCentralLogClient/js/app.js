const baseurl = "http://localhost:8080/";

(function(){
    function loadDailyErrors() {
        promise.post(baseurl + "query", '{"Messageclass" : ["Error"]}')
            .then(function (error, text, xhr) {
                let time = "<span class='w3-container w3-small w3-text-grey'>last update: " + (new Date()).toLocaleTimeString() + "</span>";
                let errors = JSON.parse(text)
                    .map(errorHtmlString)
                    .join(" ");
                
                document.getElementById("errors").innerHTML = time + errors;
            });
    }

    function errorHtmlString(error) {
        return ("<div class='w3-card-2 w3-pale-red w3-margin-bottom'>\n" + 
                "<header class='w3-container w3-red w3-color-white'>Application: <strong>{0}</strong>" +
                "<span onclick='this.parentElement.parentElement.style.display=\"none\"' style='float:right;'>&times;</span></header>\n" +
                "<div class='w3-container w3-padding'><i class='fa fa-exclamation-triangle w3-xxlarge'></i> {1}</div>\n" + 
            "</div>")
            .replace("{0}", error.Application)
            .replace("{1}", error.Message);
    }
    
    loadDailyErrors();
    window.setInterval(loadDailyErrors, 10000);
})();
