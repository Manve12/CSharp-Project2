$(document).ready(function () {
    //// VARIABLES ////
    var detailsShown = false;

    /////////////// PAGE NUMBERS //////////////

    //load default page number selection
    var pNumber = $(".gallery-page-number a");
    var lastUrlSegment = window.location.href.substr(window.location.href.lastIndexOf('/') + 1);
    if ($.isNumeric(lastUrlSegment)) {
        $("#" + lastUrlSegment).css("border", "1px solid");
    } else {
        $("#1").css("border", "1px solid");
    }

    //image blur on hover
    $(".gallery-image img").hover(function () {
        if (readCookie("ImageBlur").split("=")[1] == "false") {
            $(".gallery-image img").css("filter", "blur(5px)");
            $(this).find("img").css("filter", "blur(0px)");
            $(this).css("filter", "blur(0px)");
        }
    },
    function () {
        $(".gallery-image img").css("filter", "blur(0px)");
        $(".gallery-image-overlay-button").parent().prev().css("display", "none");
        detailsShown = false;
    });

    //gallery image view button handling for blur
    $(".gallery-image-overlay").hover(function () {
        
        if (readCookie("ImageBlur") != null) {
            if (readCookie("ImageBlur").split("=")[1] == "false") {
                $(".gallery-image img").css("filter", "blur(5px)");
                $(this)
                    .find(".gallery-image-overlay-wrapper")
                    .find(".gallery-image-overlay-author")
                    .find("img")
                    .css("filter", "blur(0px)");
                $(this).prevAll('img').first().css("filter", "blur(0px)");
            }
        }
    });

    //show/hide the image wrapper (author details etc.)
    $(".gallery-image-overlay-button").click(function () {
        if (detailsShown == false) {
            $(this).parent().prev().css("display", "block");
        }

        if (detailsShown == true) {
            $(this).parent().prev().css("display", "none");
        }

        detailsShown = !detailsShown;
    });

    function readCookie(name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    }
});