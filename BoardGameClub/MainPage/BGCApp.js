var BGCApp = angular.module('BGCApp', ["ngRoute", "ui.bootstrap"]);
BGCApp.config(
    ["$routeProvider", "$locationProvider",
    function ($routeProvider, $locationProvider) {
    $routeProvider
        .when("/", {
            templateUrl: "MainPage/views/main.html",
            controller: "MainCtrl"
        })
        .when("/about", {
            templateUrl: "MainPage/views/about.html",
            controller: "AboutCtrl"
        })
        
    //$locationProvider.html5Mode({
    //    enabled: true,
    //    requireBase: true
    //});
}]);

