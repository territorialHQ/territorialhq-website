$(document).ready(function () {

    var loaders = $(".discord-loader option");

    $.each(loaders, function () {

        var loader = $(this);

        var id = $(this).html();
        if (id !== '' && id !== null && id !== undefined) {
            var getData = new Object();
            getData.id = id;

            var discordRequest = $.ajax({
                url: (window.location.origin + "/Ajax/Discord/?handler=DiscordUserData"),
                type: 'GET',
                cache: false,
                data: getData,
                success: function (data) {
                    loader.html(data.username !== null ? data.username : id);
                },
                error: function (a, b, c) {
                    console.log(a + " " + b + " " + c);
                }
            });
        }

    });





    //var i = 0;
    //var loadInterval = setInterval(function () {
    //    LoadData(loaders[i]);
    //    i++;

    //    if (i == loaders.length)
    //        clearInterval(loadInterval);
    //}, 1);


});

//function LoadData(loader) {
//    var id = $(loader).data("id");
//    var $getData = new Object();
//    $getData.id = id;

//    var facilitySectionList = $.ajax({
//        url: (window.location.origin + "/Ajax/Discord/?handler=DiscordUserData"),
//        type: 'GET',
//        cache: false,
//        data: $getData,
//        success: function (data) {
//            console.log(id);
//            //$("#name-" + id).html(data.username);
//            //if (data.avatar !== "" && data.avatar != null) {
//            //    $("#avatar-" + id).attr("src", "https://cdn.discordapp.com/avatars/" + id + "/" + "" + data.avatar + ".png");
//            //}
//            //else {
//            //    $("#avatar-" + id).attr("src", "/img/avatar-missing.png");
//            //}
//        },
//        error: function (a, b, c) {
//            alert("Error loading data for " + $getData.id);
//        }
//    });
//}
