$(document).ready(function () {
    $(".lb-tab").click(function (e) {
        e.preventDefault();
        $(".lb-tab").removeClass("active");
        $(e.target).addClass("active");

        $(".tthq-lb-table").hide();
        $($(e.target).data("target")).show();
    });
});