﻿$(document).ready(function () {
    /////////////// PAGE NUMBERS //////////////

    //load default page number selection
    var pNumber = $(".gallery-page-number a");
    var lastUrlSegment = window.location.href.substr(window.location.href.lastIndexOf('/') + 1);
    if ($.isNumeric(lastUrlSegment)) {
        $("#" + lastUrlSegment).css("border", "1px solid");
    } else {
        $("#1").css("border", "1px solid");
    }

    ////////////// BLUR //////////////

    //image blur on hover
    $(".gallery-image img").hover(function () {
        $(".gallery-image img").css("filter", "blur(5px)");
        $(this).css("filter","blur(0px)");
    },
    function () {
        $(".gallery-image img").css("filter", "blur(0px)");
    });

    //gallery image view button handling for blur
    $(".gallery-image-overlay").hover(function () {
        $(".gallery-image img").css("filter", "blur(5px)");
        $(this).prevAll('img').first().css("filter", "blur(0px)");
    });

});