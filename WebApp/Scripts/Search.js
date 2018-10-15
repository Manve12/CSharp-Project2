$(".navigation-search-button").click(function () {
    
    //$.ajax({
    //    type: "POST",
    //    url: '@Url.Content("~/Search/Index")',
    //    data: {},
    //    success: function (result) {
    //        $("#search-wrapper").html(result);
    //    }
    //});
    $("#search-wrapper").load('/Search/Index');
});