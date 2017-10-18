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
