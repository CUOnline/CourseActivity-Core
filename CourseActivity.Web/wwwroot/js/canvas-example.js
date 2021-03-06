﻿/*
 *   Load script on roster pages to display usage data in course-activity wolf app.
 *   https://github.com/CUOnline/course-activity
 */

$(function () {
    if (window.location.pathname.match(/courses\/\d+\/users/)) {
        var script_elem = document.createElement("script")
        var head_elem = document.head || document.getElementsByTagName("head")[0];
        script_elem.onload = function () { head_elem.removeChild(script_elem) };
        script_elem.src = "<%= scheme %>://<%= File.join(domain, mount, 'access-report.js') %>";
        head_elem.appendChild(script_elem);
    }
});
