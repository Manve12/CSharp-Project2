$(".navigation-search-button").click(function () {
    var lastElement = window.location.href.split('/');
    lastElement = lastElement[lastElement.length - 1];
    
    if (!$.isNumeric(lastElement)) {
        lastElement = 1;
    }

    $("#search-container").load('/Search/Index/'+lastElement);
});

//$("form").on("submit", function (e) {
//    e.preventDefault();
//});