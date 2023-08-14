$(document).ready(function () {
    $(".datepicker").flatpickr({
        locale: "en",
        dateFormat: "m/d/Y"
    });
});

$(document).ready(function () {
    $(".datetimepicker").flatpickr({
        locale: "en",
        enableTime: true,
        dateFormat: "m/d/Y H:i",
        time_24hr: true
    });
});