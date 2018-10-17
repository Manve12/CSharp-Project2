function getSearch() {
    var splitUrl = window.location.href.split('/');

    var searchId = splitUrl[5];
    var queryId = null;

    if (splitUrl.length == 7) {
        queryId = splitUrl[6];
    }

    if (queryId != null) {
        $("#search-container").load('/Search/Index/' + searchId + '/' + queryId);
    } else {
        $("#search-container").load('/Search/Index/' + searchId);
    }
}

$(".navigation-search-button").click(function () { getSearch() });
