$(document).ready(function () {
    var dateContainers = $(".local-date");

    var dateOptions = { weekday: 'long', year: 'numeric', month: 'short', day: 'numeric' };


    $.each(dateContainers, function () {
        var date = $(this).data("date");
        var localDate = new Date(date);

        console.log(date);
        console.log(localDate);


        $(this).html("(" + localDate.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" }) + " your time)");
    });
});