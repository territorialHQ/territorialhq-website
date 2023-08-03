$(document).ready(function () {
    var history = $('#history');
    var btn = $('#history-expand');

    if (history.innerHeight() > 305) {
        history.css("max-height", "305px");
        history.css("overflow", "hidden");
        history.css("margin-bottom", "20px");
        btn.show();

        btn.click(function () {
            history.css("max-height", "inherit");
            btn.hide();
        })
    }

})