// Add checkbox AJAX
$(document).ready(function () {

    $('.ActiveCheck').change(function () {
        var self = $(this);
        var id = self.attr('id');
        var value = self.prop('checked')

        $.ajax({
            url: '/Announcements/AJAXEdit',
            data: {
                id: id,
                value: value
            },
            type: 'POST',
            success: function (result) {
                $('#announcementsDiv').html(result);
            }
        });
    });
    
    
})