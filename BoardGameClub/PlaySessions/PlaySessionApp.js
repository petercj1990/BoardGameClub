var PlaySessionsApp = angular.module("PlaySessionsApp", ["ngRoute", "ui.bootstrap", "ngAnimate", "ngSanitize", "dndLists"]);
PlaySessionsApp.config(
    ["$routeProvider", "$locationProvider",
    function ($routeProvider, $locationProvider) {
      $routeProvider
        .when("/",
          {
            templateUrl: "../PlaySessions/views/playSessions.html",
            controller: "playSession"
          })
        .otherwise({
          redirectTo: "/"
        });

    }]);

