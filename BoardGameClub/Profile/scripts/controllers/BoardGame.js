angular.module('PlayerApp')
    .controller('BGCtrl', ['$scope', "$http", '$routeParams', '$window', '$sce', '$location',
        function ($scope, $http, $routeParams, $window, $sce, $location) {       
        $http.get("BG",
            {
                headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' },
                params: { GameId: $routeParams.id }
            }).then(function (result) {
                $scope.BG = result.data;
            }, function (error) {
                console.error(error);
            });
        $http.get("isOwned", {
              headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' },
              params: { GameId: $routeParams.id }
              }).then(function (result) {
              if (result.data == "False") {
                  $scope.owned = false;
              }
              else {
                  $scope.owned = true;
              }
          }, function (error) {
              console.error(error);
          })

        $scope.removeFromCollection = function (id) {
            console.log('im here to delete ', id)
            id = parseInt(id);
            $http.post("DeleteGame", { GameId: id }).then(function (result) {
                $scope.owned = result.data;
                console.log(result);
                $location.path("/");
            }, function (error) {
                console.error(error);
            })
        }

        $scope.addToFavorites = function (id) {
            console.log('im here ', id)
            $http.post("FavoriteGame", { GameId: id })
                .then(function (result) {
                $scope.owned = result.data;
                console.log(result);
            }, function (error) {
                console.error(error);
            })
        }
         
        $scope.goToBGG = function(BGGID){
            $window.location.href = "https://www.boardgamegeek.com/boardgame/" + BGGID;
        }

        $scope.renderHtml = function (html_code) {
            return $sce.trustAsHtml(html_code);
        };
    }]);
