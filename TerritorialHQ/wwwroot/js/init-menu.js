$(document).ready(function () {
    $(".menu-link").click(function (e) {

        var p = $(e.target).parent();
        if (p.is("li")) {
            p.toggleClass("active");
        } else {
            p.parent().toggleClass("active");
        }
    });
    $(".menu-toggle").click(function (e) {
        $(e.currentTarget).toggleClass("active");
        $(".menu-navigation").toggleClass("active");
    });
    $("#menu-search").click(function (e) {
        $("#search").toggle();
    });
});