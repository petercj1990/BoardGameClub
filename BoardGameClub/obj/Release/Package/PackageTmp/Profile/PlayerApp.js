var PlayerApp = angular.module('PlayerApp', ["ngRoute", "ui.bootstrap", 'ngAnimate', 'ngSanitize']);
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
            .otherwise({
                redirectTo: "/home"
            })
    //$locationProvider
    //    .html5Mode({
    //      enabled: true,
    //      requireBase: false
    //    });
}]);

