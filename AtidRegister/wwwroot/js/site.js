// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

const loginControllerPath = "/Home";

/* Display the countdown timer on the students' navbar */
var destination = document.querySelector("#countdown");
if (destination) {
    var countDownDate = new Date("Feb 27, 2020 8:00:00").getTime();
    var x = setInterval(() => {
        var now = new Date().getTime();
        var distance = countDownDate - now;
        var days = Math.floor(distance / (1000 * 60 * 60 * 24));
        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((distance % (1000 * 60)) / 1000);
        // Format and print 01:01:01:01
        destination.innerHTML = days + ":" + (hours < 10 ? "0" : "") + hours + ":"
            + (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds + "";
        if (distance < 0) {
            clearInterval(x);
            destination.innerHTML = "תודה שהשתתפתם!";
        }
    }, 1000);
}

/* fetch grades of class after selecting grade */
$("#selectClass").change(() => {
    var thisSelect = $("#selectClass");
    const selectionId = thisSelect.val();
    const select = $("#selectGrade");
    select.text($("<option></option>").attr("value", true).attr("selected", true).attr("disabled", true).text("טוען"));
    const StuGroup = $("#studentSelectGroup select");
    StuGroup.text($("<option></option>").attr("value", true).attr("selected", true).attr("disabled", true).text("השם שלי"));
    fetch(`${loginControllerPath}/GetGrades/${selectionId}`).then(body => body.json())
        .then(res => {
            // insert to the select the items
            select.append($("<option></option>").attr("value", true).attr("selected", true).attr("disabled", true).text("אני בכיתה"));
            res.map(classObj => {
                select.append($("<option></option>").attr("value", classObj.id).text(classObj.classNumber));
            });
            // show hidden element
            const group = $("#gradeSelectGroup");
            group.css("visibility", "");
        })
        .catch(err => console.error(err));
});

/* fetch grades of class after selecting grade */
$("#selectGrade").change(() => {
    var thisSelect = $("#selectGrade");
    const selectionId = thisSelect.val();
    const select = $("#selectStudent");
    select.text($("<option></option>").attr("value", true).attr("selected", true).attr("disabled", true).text("טוען"));
    fetch(`${loginControllerPath}/GetStudents/${selectionId}`).then(body => body.json())
        .then(res => {
            select.append($("<option></option>").attr("value", true).attr("selected", true).attr("disabled", true).text("השם שלי"));
            // insert to the select the items
            res.map(classObj => {
                select.append($("<option></option>").attr("value", classObj.id).text(classObj.userName));
            });
            // show hidden element
            const group = $("#studentSelectGroup");
            group.css("visibility", "");
        })
        .catch(err => console.error(err));
});

/* Display Prioritization of Contents and store in the hidden input for model binding */
$(".reg-chkbx-input").on('change', e => {
    var chkbx = e.target; // sender
    var hiddenInput = $("#priorities"); // hidden input
    var currValue = hiddenInput.val(); // value of hidden input = selected cards
    var currValueSections = currValue.split('$'); // split by time strips
    const typeOffset = parseInt($(chkbx).data("type-offset")); // find offset of strip in array by data property
    const contentId = $(chkbx).data("content-id"); // find id of content by sender data property
    const priorityRateElement = $(chkbx).parent().find("label").find(".priority-rate"); // priority header
    // add
    if (chkbx.checked) {
        // add to hidden and show priority
        currValueSections[typeOffset] += contentId + ".";
        priorityRateElement.text("עדיפות: " + (currValueSections[typeOffset].split(".").findIndex(s => s == contentId) + 1));
    }
    // remove
    else {
        // remove from hidden
        const indexOfRemoved = currValueSections[typeOffset].indexOf(contentId);
        currValueSections[typeOffset] = currValueSections[typeOffset].replace(contentId + ".", "");
        var removeFrom = currValueSections[typeOffset].slice(indexOfRemoved);
        // and decrease pripority in UI if needed (e.g. we dropped priority 1 therefore 2 => 1 and 3 => 2 and so on..)
        removeFrom.split(".").forEach(element => {
            if (element) {
                // take priority number back in UI
                var elem = $(`input[type=checkbox][data-content-id=${element}]`).parent().find("label").find(".priority-rate");
                var priorityIndx = parseInt(elem.text().replace("עדיפות: ", ""));
                elem.text("עדיפות: " + (priorityIndx - 1));
            }
        });
        priorityRateElement.text("לחצו עליי כדי להוסיף");
    }
    hiddenInput.val(currValueSections.join("$"));
});