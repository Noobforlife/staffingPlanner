//Code to search for data in course table table
$('#courseListTable').DataTable(
    {
        "pageLength": 20,
        "language": {
            "lengthMenu": "Show _MENU_ courses",
            "infoFiltered": "(filtered from _MAX_ total courses)",
            "infoEmpty": "No courses available",
            "infoFiltered": "(filtered from _MAX_ total courses)",
            "info": "Showing _START_ to _END_ of _TOTAL_ courses",
        }
        
});

$('#teacherListTable').DataTable(
    {
        "pageLength": 25,
        "language": {
            "lengthMenu": "Show _MENU_ teachers",
            "infoFiltered": "(filtered from _MAX_ total teachers)",
            "infoEmpty": "No teachers available",
            "infoFiltered": "(filtered from _MAX_ total teachers)",
            "info": "Showing _START_ to _END_ of _TOTAL_ teachers",
        }
    });

$(document).ready(function () {
    $("#notificationLink").click(function () {
        $("#notificationContainer").fadeToggle(300);
        $("#notification_count").fadeOut("slow");
        return false;
    });

    //Document Click hiding the popup 
    $(document).click(function () {
        $("#notificationContainer").hide();
    });

    //Popup on click
    $("#notificationContainer").click(function () {
        return false;
    });

});
//javascript or making a request
$(document).ready(function () {
    $("#requestLink").click(function () {
        $(".overlay-box").fadeToggle(200);
        return false;
    });

    //Document Click hiding the popup 
    $(document).click(function () {
        $(".overlay-box").hide();
    });

    //Popup on click
    $(".overlay-box").click(function () {
        return false;
    });

});