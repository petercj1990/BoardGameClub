angular.module('PlayerApp') 
    .controller('searchBarCtrl', ['$scope', function ($scope) {
        $scope.searchSite = function (searchText) {
            console.log(searchText);
            for (let entry of $scope.siteSearchResponse) {
                if (searchText === entry.Name) {
                    console.log(entry);
                    if (entry.Type === "Player") {
                        $location.path('/' + entry.Id);
                    }
                    if (entry.Type === "Game") {
                        $location.path('/BG/' + entry.Id);
                    }
                }
            }
            $scope.siteSearchText = "";
        };


        $scope.$watch('siteSearchText', function (tmpStr) {
            if (!tmpStr || tmpStr.length <= 2) {
                return 0;
            }
            if (tmpStr === $scope.siteSearchText) {
                DataService.searchSite($scope.siteSearchText).then(function (result) {
                    var searchResults = [];
                    for (let player in result.PlayerResults) {
                        console.log(player);
                        result.PlayerResults[player].Type = "Player";
                        searchResults.push(result.PlayerResults[player]);
                    }
                    for (let game in result.BGResults) {
                        console.log(game)
                        result.BGResults[game].Type = "Game"
                        searchResults.push(result.BGResults[game]);
                    }
                    $timeout(function () {
                        $scope.siteSearchResponse = searchResults;
                        console.log($scope.siteSearchResponse);
                    });

                }, function (err) {
                    console.log(err);
                })
            }
        });
     }])
    .directive('searchBar', ['$scope', '$http', function () {
        return {
            tempalteUrl: "search-bar.html"
        };
    }])
