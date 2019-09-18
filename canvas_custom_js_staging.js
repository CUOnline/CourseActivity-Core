/*
 * canvas-hide-self-enrollment-options.js
 * by: David Lyons hi@lyonsinbeta.com
 * Hides self-enrollment options from any
 * user who is not an admin
 */

$(function () {
    if (window.location.pathname == "/courses/" + ENV.COURSE_ID + "/settings") {
        if ($.inArray("admin", ENV.current_user_roles) == -1) {
            $("#course_self_enrollment").hide();
            $("label[for='course_self_enrollment']").hide();
            $(".open_enrollment_holder").hide();
            $("div.public_options").hide();
        }
    }
});


/*
 * Load script on roster pages to send
 * usage data to course-activity wolf app.
 * https://github.com/CUOnline/course-activity
 */

$(function () {
    if (window.location.pathname.match(/courses\/\d+\/users/)) {
        var a = document.createElement("script"), b = document.head || document.getElementsByTagName("head")[0]; a.onload = function () { b.removeChild(a); }; a.src = "https://cuo-courseactivity.rsstest.com/js/access-report.js"; b.appendChild(a);
    }
});


/* Proctorio JavaScript */

(function () { var a = document.createElement("script"), b = document.head || document.getElementsByTagName("head")[0]; a.onload = function () { b.removeChild(a) }; a.src = "//az545770.vo.msecnd.net/snippets/8/gbl/canvas-global-embed.js"; b.appendChild(a) })();

/* Blackboard Ally JavaScript */

window.ALLY_CFG = {
    'baseUrl': 'https://prod.ally.ac',
    'clientId': 4404
};
$.getScript(ALLY_CFG.baseUrl + '/integration/canvas/ally.js');