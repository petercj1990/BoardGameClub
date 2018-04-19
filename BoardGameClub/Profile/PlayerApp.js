var PlayerApp = angular.module('PlayerApp', ["ngRoute", "ui.bootstrap", 'ngAnimate', 'ngSanitize', 'ngFileUpload', 'ngImgCrop']);
PlayerApp.config(
    ["$routeProvider", "$locationProvider",
    function ($routeProvider, $locationProvider) {
        $routeProvider
            .when("/", {
                templateUrl: "../Profile/views/playerView.html",
                controller: "PlayerCtrl"
            })
            .when("/:id", {
                templateUrl: "../Profile/views/playerView.html",
                controller: "PlayerCtrl"
            })
            .when("/BG/:id", {
                templateUrl: "../Profile/views/BGView.html",
                controller: "BGCtrl"
            })
            .when("/playSessions",{
                templateUrl: "../Profile/views/playSessions.html",
                controller: "playSession"
            })
            .otherwise({
                redirectTo: "/home"
            })
    //$locationProvider
    //    .html5Mode({
    //      enabled: true,
    //      requireBase: false
    //    });
}]);

