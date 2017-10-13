var PlayerApp = angular.module('PlayerApp', ["ngRoute", "ui.bootstrap"]);
PlayerApp.config(
    ["$routeProvider", "$locationProvider",
    function ($routeProvider, $locationProvider) {
        $routeProvider
            .when("/", {
                templateUrl: "../Profile/views/playerView.html",
                controller: "PlayerCtrl"
            })
            .when("/about", {
                templateUrl: "../Profile/views/about.html",
                controller: "About2Ctrl"
            });
        
    //$locationProvider
    //    .html5Mode({
    //      enabled: true,
    //      requireBase: false
    //    });
}]);

