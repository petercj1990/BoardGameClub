'use strict';

angular.module('PlayerApp')
    .controller('PlayerCtrl', ['$scope', '$routeParams', 'DataService', "$http", "$timeout", "$filter", "$uibModal", '$sce', '$location', 'Upload',
        function ($scope, $routeParams, DataService, $http, $timeout, $filter, $uibModal, $sce, $location, Upload) {
          //load Player values
            $scope.Games = [];
            $scope.aGames = [];
            $scope.userStats = {};
            $scope.totalItems = 0;
            $scope.currentPage = 1;
            $scope.ItemsPerPage = 5;
            $scope.maxSize = 5;
            $scope.numPages = 0;
            $scope.otherProfile = false;
            $http.defaults.headers.common["Content-Type"] = "application/x-www-form-urlencoded";
            $scope.setPage = function (pageNo) {
                $scope.currentPage = pageNo;
            };

            $scope.pageChanged = function () {
                setPagingData($scope.currentPage);
            };

            $scope.setPage = function (pageNo) {
              $scope.currentPage = pageNo;
            };

            $scope.add = function () {
              var f = document.getElementById('file').files[0],
                r = new FileReader();

              r.onloadend = function (e) {
                var data = e.target.result;
                //send your binary data via $http or $resource or do anything else with it
              };

              r.readAsBinaryString(f);
            };

            function setPagingData(page) {
              $timeout(function() {
                var slicingGames = [];
                var start = ((page - 1) * $scope.ItemsPerPage);
                var end = page * $scope.ItemsPerPage;
                if (end > $scope.Games.length) {
                  end = $scope.Games.length;
                }
                for (var x = start;
                  x <
                    end;
                  x++) {
                  slicingGames.push($scope.Games[x]);
                }
                $scope.aGames = slicingGames;
              });
            }
            $scope.winPercentageEval = function (wins, losses) {
              var percentage = (wins / (wins + losses)) * 100;
              return percentage;
            };
            function onLoad(cb) {
                if ($routeParams.id) {
                  $scope.otherProfile = true;
                  console.log($routeParams.id);
                  DataService.pullUser($routeParams.id).then(function(result) {
                    $scope.user = result;
                    console.log(result);
                    if (result.ProfilePicPath) {
                      $scope.user.ProfilePicPath = "../Content/profilePics/" + result.ProfilePicPath.toString();
                    }
                    else {
                      $scope.user.ProfilePicPath = "../Content/profilePics/profile.png";
                    }
                      console.log(result);
                      for (let x in result.PlayersGames) {
                        $timeout(function() {
                          $scope.Games.push(result.PlayersGames[x]);
                          $scope.otherProfile = true;
                        });
                      }
                      cb();
                    },
                    function(err) {
                      console.log(err);
                    });
                    DataService.getStats($routeParams.id).then(function (result) {
                      $scope.userStats = result; 
                      console.log($scope.userStats);
                    },
                    function (err) {
                      console.log(err);
                    });
                }
                else {
                  $scope.otherProfile = false;
                  DataService.pullUser("self").then(function(result) {
                      $scope.user = result;
                      console.log(result);
                      if (result.ProfilePicPath) {
                        $scope.user.ProfilePicPath = "../Content/profilePics/" + result.ProfilePicPath.toString();
                      }
                      else {
                        $scope.user.ProfilePicPath = "../Content/profilePics/profile.png";
                      }
                      for (let x in result.PlayersGames) {
                        $timeout(function() {
                          $scope.Games.push(result.PlayersGames[x]);
                          $scope.totalItems = $scope.Games.length;
                        });
                      }
                      cb();
                    },
                    function(err) {
                      console.log(err);
                    });
                  DataService.getStats("self").then(function (result) {
                    $timeout(function () {
                      $scope.userStats = result;
                    });
                    console.log(result);

                  },
                  function (err) {
                    console.log(err);
                  });
                }
            }

            onLoad(function(){
                setPagingData(1);
                $timeout(function () {
                  $scope.totalItems = $scope.Games.length;
                  $scope.numPages = Math.round($scope.totalItems / $scope.ItemsPerPage);
                });
            });

            //data declarations
            $scope.responseData = [];
            $scope.filterText = '';
            $scope.selectedBG = {};
            var parser = new DOMParser();
            var tempFilterText = '',
                filterTextTimeout;

            var dataList = document.getElementById('boardgame-list');

            //functions
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
            DataService.searchSite($scope.siteSearchText).then(function(result) {
                var searchResults = [];
                for (let player in result.PlayerResults) {
                  result.PlayerResults[player].Type = "Player";
                  searchResults.push(result.PlayerResults[player]);
                }
                for (let game in result.BGResults) {
                  console.log(game);
                  result.BGResults[game].Type = "Game";
                  searchResults.push(result.BGResults[game]);
                }
                $timeout(function() {
                  $scope.siteSearchResponse = searchResults;
                  console.log($scope.siteSearchResponse);
                });

              },function(err) {
                console.log(err);
              });
          }
        });

        // BGG search
        $scope.addGame = function (searchText) {
            for (let entry of $scope.responseData) {
                if ($scope.searchText === entry.boardgame._value[0].name._value) {
                    console.log(entry.boardgame.objectid);
                    showForm(entry.boardgame.objectid);
                }
            }
            $scope.searchText = "";
        };
            
        $scope.$watch('searchText', function (tmpStr) {
          if (!tmpStr || tmpStr.length <= 2) {
              return 0;
          }
            
          if (tmpStr === $scope.searchText) {
              $http.get("FindBoardgame", 
              {                  
                  params: { searchText: $scope.searchText }
              }).then(function (data) {
                if (data.data.boardgames._value) {
                  console.log(data.data.boardgames._value);
                  $scope.responseData = data.data.boardgames._value;
                  if ($scope.responseData.length === 1) {
                      $scope.selectedBG = $scope.responseData;
                  }
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
                      }];
                      }
              }, function (err) {
                  console.log(err);
              });
            }   
          });

          $scope.removeGame = function (GameData) {
              DataService.removeGameFromLibrary(GameData);
          };

          $scope.openImageUploadModal = function () {
            var modalInstance = $uibModal.open({
              templateUrl: 'addProfilePic.html',
              controller: uploadModal,
              scope: $scope,
              resolve: {
              }
            });

            modalInstance.result.then(function (selectedItem) {
              $scope.selected = selectedItem;
            }, function () {
              //console.info('Modal dismissed at: ' + new Date());
            });
          };

          //$scope.addFriend = function (Id) {
          //    console.log('adding a friend');
          //    $http.post("AddFriend", BoardGame,
          //        {
          //            headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' },
          //            Data: { BoardGame: $scope.pendingGame }
          //        }).then(function (data) {
          //            console.log("success");
                     
          //        }, function (error) {
          //            console.log(error);
          //        });
          //}

          $scope.uploadProfileModal = function () {
            var modalInstance = $uibModal.open({
              templateUrl: 'addProfilePic.html',
              controller: uploadModal,
              scope: $scope
            });

            modalInstance.result.then(function (selectedItem) {
              $scope.selected = selectedItem;
            }, function () {
              //console.info('Modal dismissed at: ' + new Date());
            });
          };

          var uploadModal = ["$scope", "$http", "$timeout", "Upload", "$uibModalInstance", function ($scope, $http, $timeout, Upload, $uibModalInstance) {
            $scope.upload = function (dataUrl, name) {
              Upload.upload({
                url: 'UploadProfilePic',
                file: Upload.dataUrltoBlob(dataUrl, name)
              }).then(function (response) {
                $timeout(function () {
                  $scope.result = response.data;
                  DataService.pullUser().then(function (results) {
                    $scope.user = results;
                  },
                    function (err) { console.error(err) });
                });
              }, function (response) {
                if (response.status > 0) $scope.errorMsg = response.status + ': ' + response.data;
              }, function (evt) {
                $scope.progress = parseInt(100.0 * evt.loaded / evt.total);
              });
            };
            $scope.close = function () {
              console.log("tries to close");
              $uibModalInstance.close();
            };
          }];

          $scope.routeToGame = function (id) {
            $location.path('/BG/' + id);
          };

          //modal requirements
          function showForm(BG) {
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
              });
          }
        
          var boardgameModal = ["$scope", "$http", "$uibModalInstance", function ($scope, $http, $uibModalInstance) {
            $scope.pendingGame = {};
            $scope.pendingGame.BGGID = $scope.$resolve.BG;
            $http.get("BoardgameData",
              {
                headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' },
                params: { GameId: $scope.$resolve.BG }
              }).then(function (data) {
                console.log(data.data.boardgames._value);
                for (let attribute of data.data.boardgames._value[0].boardgame._value) {
                  if (attribute.minplayers) {
                    $scope.minPlayer = attribute.minplayers._value;
                    $scope.pendingGame.MinPlayer = parseInt(attribute.minplayers._value);
                    console.log('i add a min player');
                  }
                  if (attribute.maxplayers) {
                    $scope.maxPlayer = attribute.maxplayers._value;
                    $scope.pendingGame.maxPlayer = parseInt(attribute.maxplayers._value);
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
                    $scope.pendingGame.category = attribute.boardgamecategory._value;
                    console.log('i add a boardgamecategory');
                  }
                }
              }, function (error) {
                console.error(error);
              });

            $scope.ok = function () {
              var BoardGame = $scope.pendingGame;
              console.log(BoardGame);
              $http.post("AddGameToLibrary", BoardGame,
                {
                  headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' },
                  Data: { BoardGame: $scope.pendingGame }
                }).then(function (data) {
                  console.log('makes a game');
                  $timeout(function () {
                    $scope.Games.push(BoardGame);
                  });
                  setPagingData($scope.currentPage);

                }, function (error) {
                  console.log(error);
                });

              console.log($scope.pendingGame);
              console.log($scope.Games);
              $scope.Games.push({
                BGGID: $scope.pendingGame.BGGID,
                Category: $scope.pendingGame.category,
                Image: $scope.pendingGame.image,
                MaxPlayer: $scope.pendingGame.maxPlayer,
                MinPlayer: $scope.pendingGame.minPlayer,
                Name: $scope.pendingGame.name
              });
              setPagingData($scope.currentPage);
              $uibModalInstance.close();
            };

            $scope.renderHtml = function (html_code) {
              return $sce.trustAsHtml(html_code);
            };

            $scope.cancel = function () {
              $uibModalInstance.close();
            };

            $scope.toString = function (value) {
              return value.toString();
            };
          }];
  }]);
