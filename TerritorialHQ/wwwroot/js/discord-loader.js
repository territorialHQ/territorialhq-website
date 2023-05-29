$(document).ready(function () {

    var loaders = $(".discord-loader");

    $.each(loaders, function () {

        var id = $(this).data("id");

        var getData = new Object();
        getData.id = id;

        var discordRequest = $.ajax({
            url: (window.location.origin + "/Ajax/Discord/?handler=DiscordUserData"),
            type: 'GET',
            cache: false,
            data: getData,
            success: function (data) {
                $(".name-" + id).html(data.username !== null ? data.username : id)
                if (data.leftGuild === true) {
                    $(".name-" + id).css("text-decoration", "line-through")
                    $(".name-" + id).css("color", "#999")
                }

                if (data.avatar !== "" && data.avatar != null) {
                    $(".avatar-" + id).attr("src", "https://cdn.discordapp.com/avatars/" + id + "/" + "" + data.avatar + ".png");
                }
                else {
                    $(".avatar-" + id).attr("src", "/img/default-avatar.png");
                }
            },
            error: function (a, b, c) {

            }
        });


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
