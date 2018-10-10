$(document).ready(function () {
    //load default page number selection
    var pNumber = $(".gallery-page-number");
    var lastUrlSegment = window.location.href.substr(window.location.href.lastIndexOf('/') + 1);
    if (isNaN(lastUrlSegment)) {
        selectPageNumber(pNumber,1);
    }
    else {
        selectPageNumber(pNumber,lastUrlSegment);
    }
});

function selectPageNumber(pNumber, pageNumber) {
    pNumber.each(function (i, obj) {
        if (i == pageNumber - 1) {
            $(this).css("border", "1px solid");
            return;
        }
    });
}

//image blur on hover
$(".gallery-image img").hover(function () {
    $(".gallery-image img").css("filter", "blur(5px)");
    $(this).css("filter","blur(0px)");
},
function (){
    $(".gallery-image img").css("filter", "blur(0px)");
});

//page number selection
$(".gallery-page-number").click(function () {
    $(".gallery-page-number").css("border", "none");
    $(this).css("border", "1px solid");
});