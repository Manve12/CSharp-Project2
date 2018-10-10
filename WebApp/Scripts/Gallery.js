$(".gallery-image img").hover(function () {
    $(".gallery-image img").css("filter", "blur(5px)");
    $(this).css("filter","blur(0px)");
},
function (){
    $(".gallery-image img").css("filter", "blur(0px)");
});