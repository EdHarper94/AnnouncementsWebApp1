$(document).ready(function () {

    $.ajax({
        url: '/Announcements/BuildAnnouncementsTable',
        success: function (result) {
            $('#announcementsDiv').html(result);
        }
    });
});