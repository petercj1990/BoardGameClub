'use strict';

angular.module('PlayerApp')
    .controller('PlayerCtrl', ['$scope', '$routeParams', 'DataService', "$http", "$timeout", "$filter", "$uibModal" , 
        function ($scope, $routeParams, DataService, $http, $timeout, $filter, $uibModal) {
          //load olayer valuew
          $http.defaults.headers.common["Content-Type"] = "application/x-www-form-urlencoded";
          if ($routeParams.id) {
              DataService.pullUser($routeParams.id).then(function (result) {
                  $scope.user = result;
              });
          }
          else {
              $scope.user = DataService.pullUser("self");
          }

          //data declarations
          $scope.responseData = [];
          $scope.filterText = '';
          $scope.selectedBG = {};
          var parser = new DOMParser();
          var tempFilterText = '',
              filterTextTimeout;
          var dataList = document.getElementById('boardgame-list');

          //functions
          $scope.addGame = function (searchText) {

              console.log($scope.searchText);
              for (let entry of $scope.responseData) {
                  if ($scope.searchText === entry.boardgame._value[0].name._value) {
                      console.log(entry.boardgame.objectid);
                      showForm(entry.boardgame.objectid);
                  }
              }
              $scope.searchText = "";
          };

          $scope.removeGame = function (GameData) {
              DataService.removeGameFromLibrary(GameData);
          };
                 
          $scope.$watch('searchText', function (tmpStr) {
            if (!tmpStr || tmpStr.length === 0) {
                return 0;
            }
            
            if (tmpStr === $scope.searchText) {
                $http.get("FindBoardgame", 
                {
                    headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' },
                    params: { searchText: $scope.searchText }
                }).then(function (data) {
                    if (data.data.boardgames._value) {
                            $scope.responseData = data.data.boardgames._value;
                            if ($scope.responseData.lentght === 1) {
                                $scope.selectedBG = $scope.responseData;
                            };
                        }
                        else {
                            $scope.responseData = [{
                               boardgame: {
                                    _value: [
                                        {
                                            name: { _value: "no results..." }
                                        }
                                    ]
                                }
                            }]
                        }
                }, function (err) {
                    console.log(err);
                });
            };   
          });

          //modal requirements
                 
          function showForm(BG) {
              $scope.message = "Show Form Button Clicked";

              var modalInstance = $uibModal.open({
                  templateUrl: 'addGame.html',
                  controller: boardgameModal,
                  scope: $scope,
                  resolve: {
                      BG: function () {
                          return BG;
                      }
                  }
              });

              modalInstance.result.then(function (selectedItem) {
                  $scope.selected = selectedItem;
              }, function () {
                  console.info('Modal dismissed at: ' + new Date());
              });
          };
        
          var boardgameModal = function ($scope, $http, $uibModalInstance) {
              console.log($scope.$resolve.BG);
              $scope.pendingGame = {};
              $http.get("BoardgameData",
                  {
                      headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' },
                      params: { GameId: $scope.$resolve.BG }
                  }).then(function (data) {
                      console.log(data.data.boardgames._value);
                      for (let attribute of data.data.boardgames._value[0].boardgame._value) {
                          if (attribute.minplayers) {
                              $scope.pendingGame.minPlayers = attribute.minplayers._value;
                              console.log('i add a min player');
                          }
                          if (attribute.maxplayers) {
                              $scope.pendingGame.maxPlayers = attribute.maxplayers._value;
                              console.log('i add a max player');
                          }
                          if (attribute.playingtime) {
                              $scope.pendingGame.playingtime = attribute.playingtime._value;
                              console.log('i add a playtime');
                          }
                          if (attribute.name) {
                              if (attribute.name.primary) {
                                  $scope.pendingGame.name = attribute.name._value;
                                  console.log("i add a name");
                              }
                          }
                          if (attribute.thumbnail) {
                              $scope.pendingGame.image = attribute.thumbnail._value;
                              console.log('i add an Image');
                          }
                          if (attribute.description) {
                              $scope.pendingGame.description = attribute.description._value;
                              console.log('i add a description');
                          }
                          if (attribute.boardgamecategory) {
                              $scope.pendingGame.boardgamecategory = attribute.boardgamecategory._value;
                              console.log('i add a boardgamecategory');
                          }
                          

                      }
                      console.log($scope.pendingGame);
                  }, function (error) {
                      console.error(error);
                  });

              $scope.ok = function () {
                  var BoardGame = $scope.pendingGame;
                  console.log(BoardGame);
                  $http.post("AddGameToLibrary",BoardGame,
                      {
                          headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' },
                          Data: { BoardGame: $scope.pendingGame }
                      }).then(function (data) {

                      }, function (error) {
                          console.log(error);
                      })
           
                  console.log($scope.pendingGame);
                  $uibModalInstance.close();
              };

              $scope.cancel = function () {
                  $uibModalInstance.close();
              };
          }
  }]);
