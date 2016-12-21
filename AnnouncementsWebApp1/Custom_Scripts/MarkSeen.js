/// Ajax function to post Announcement ID. to SeenAnnouncements controller when page loads.

$(document).ready(function () {
    var token = $('[name=__RequestVerificationToken]').val();
    var id = $('#AnnouncementId').val();
    $.ajax({
        type: "POST",
        url: '/SeenAnnouncements/MarkSeenAJAX',
        data: { "__RequestVerificationToken": token, AnnouncementId: id },
        error: function(){
            alert('You Fucked Up')
        },
        success: function () {
            alert('this worked');
        }
    });
});