angular.module("PlaySessionsApp")
    .controller("playSession", ["$scope", "$uibModal", "$http", "PlayDataService", "$routeParams", "$timeout", 
        function ($scope, $uibModal, $http, PlayDataService, $routeParams, $timeout) {
            
          $http.defaults.headers.common["Content-Type"] = "application/x-www-form-urlencoded";
            
          $scope.Plays = Array();
          $scope.aPlays = [];
          $scope.totalItems = 0;
          $scope.currentPage = 1;
          $scope.ItemsPerPage = 10;
          $scope.maxSize = 10;
          $scope.numPages = 0;
          $scope.option = 0;
          $scope.error = {};
          var d = new Date();
          $scope.playDate = d.getTime();


          $scope.pageChanged = function () {
            console.log("im changing");
              setPagingData($scope.currentPage);
          };

          $scope.setPage = function (pageNo) {
              console.log(pageNo);
              $scope.currentPage = pageNo;
          }

          function setPagingData(page) {
            $timeout(function () {
              console.log(page);
              var slicingPlays = [];
              var start = ((page - 1) * $scope.ItemsPerPage);
              var end = (page * $scope.ItemsPerPage);
              if (end > $scope.Plays.length) {
                end = $scope.Plays.length;
              }
              for (var x = start; x < end; x++) {
                slicingPlays.push($scope.Plays[x]);
              }
              $scope.aPlays = slicingPlays;
              console.log($scope.aPlays);
            });
          }

          $scope.players = new Array();

          $scope.Filters = [
            { id: 1, name: "My Plays" },
            { id: 2, name: "Recents" },
            { id: 3, name: "By Boardgame" }
          ];

          function onLoad(cb) {
              console.log($scope.options)
              PlayDataService.getPlaysessions($scope.option).then(function (result) {
                  console.log(result);
                  $timeout(function () {
                    $scope.Plays = result;
                    cb();
                  });
                },
              function(err) {
                console.log(err);
              });
          }

          onLoad(function () {
              setPagingData(1);
              $timeout(function() {
                $scope.totalItems = $scope.Plays.length;
                $scope.numPages = Math.round($scope.totalItems / $scope.ItemsPerPage);
              });
          });

          $scope.FinishPlaySession = function (PSID) {
            console.log('im here', PSID);
            $scope.PSID = PSID;
            var currPS;
            PlayDataService.getPlaySession(PSID).then(function (data) {
              var modalInstance;
              currPS = data;
              if (currPS.Recorded) {
                modalInstance = $uibModal.open({
                  templateUrl: "RecordedPlaySession.html",
                  controller: playSessionFinalizeModal,
                  scope: $scope,
                  resolve: {
                    editPS: function () {
                      var editPS = PlayDataService.getPlaySession(PSID);
                      return editPS;
                    }
                  }
                });
              }
              else {
                modalInstance = $uibModal.open({
                  templateUrl: "RecordPlaySession.html",
                  controller: playSessionFinalizeModal,
                  scope: $scope,
                  resolve: {
                    editPS: function () {
                      var editPS = PlayDataService.getPlaySession(PSID);
                      return editPS;
                    }
                  }
                });
              }
            }, function (err) {
              console.log(err);
            });
            

            modalInstance.result.then(function (selectedItem) {
              $scope.selected = selectedItem;
            }, function () {
              $scope.PSID = "";
            });
            $scope.popup2 = {
              opened: false
            };
          };

          var playSessionFinalizeModal = ["$scope", "$http", "$uibModalInstance",  function ($scope, $http, $uibModalInstance) {

                $scope.EditPS = $scope.$resolve.editPS;
                console.log($scope.EditPS);
                for (var participant in $scope.EditPS.Participants) {
                  $scope.EditPS.Participants[participant].winner = false;
                }

                $scope.models = [
                  { listName: "A", items: [], dragging: false },
                  { listName: "B", items: [], dragging: false }
                ];

               
               $scope.getSelectedItemsIncluding = function(list, item) {
                item.selected = true;
                return list.items.filter(function (item) { return item.selected; });
              };

              /**
               * We set the list into dragging state, meaning the items that are being
               * dragged are hidden. We also use the HTML5 API directly to set a custom
               * image, since otherwise only the one item that the user actually dragged
               * would be shown as drag image.
               */
              $scope.onDragstart = function (list, event) {
                list.dragging = true;
                if (event.dataTransfer.setDragImage) {
                  var img = new Image();
                  img.src = 'framework/vendor/ic_content_copy_black_24dp_2x.png';
                  event.dataTransfer.setDragImage(img, 0, 0);
                }
              };

              /**
               * In the dnd-drop callback, we now have to handle the data array that we
               * sent above. We handle the insertion into the list ourselves. By returning
               * true, the dnd-list directive won't do the insertion itself.
               */
              $scope.onDrop = function (list, items, index) {
                angular.forEach(items, function (item) { item.selected = false; });
                list.items = list.items.slice(0, index)
                  .concat(items)
                  .concat(list.items.slice(index));
                return true;
              }

              /**
               * Last but not least, we have to remove the previously dragged items in the
               * dnd-moved callback.
               */
              $scope.onMoved = function (list) {
                list.items = list.items.filter(function (item) { return !item.selected; });
              };

              // Generate the initial model
              angular.forEach($scope.models, function (list) {
                for (var i = 1; i <= 4; ++i) {
                  list.items.push({ label: "Item " + list.listName + i });
                }
              });

              // Model to JSON for demo purpose
              $scope.$watch('models', function (model) {
                $scope.modelAsJson = angular.toJson(model, true);
              }, true);
                $scope.ok = function () {
                  console.log($scope.EditPS.Participants);
                  PlayDataService.UpdatePlaySessionRecord($scope.EditPS.Participants, $scope.EditPS.Id)
                  $uibModalInstance.close();
                };

                $scope.delete = function (id) {
                  PlayDataService.DeleteSession(id);
                  $uibModalInstance.close();
                }
              $scope.cancel = function () {
                $uibModalInstance.close();
              };
          }]

          $scope.showForm = function () {

            $scope.today = function () {
              $scope.playDate = new Date();
            };

            $scope.today();

            $scope.inlineOptions = {
              customClass: getDayClass,
              minDate: new Date(),
              showWeeks: true
            };

            $scope.dateOptions = {
              formatYear: 'yy',
              startingDay: 1
            };
            
            $scope.toggleMin = function () {
              $scope.inlineOptions.minDate = $scope.inlineOptions.minDate ? null : new Date();
              $scope.dateOptions.minDate = $scope.inlineOptions.minDate;
            };

            $scope.toggleMin();

            $scope.open1 = function () {
              $scope.popup1.opened = true;
            };
            
            $scope.setDate = function (year, month, day) {
              $scope.dt = new Date(year, month, day);
            };

            $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
            $scope.format = $scope.formats[0];
            $scope.altInputFormats = ['M!/d!/yyyy'];

            $scope.playTime = new Date();

            $scope.hstep = 1;
            $scope.mstep = 1;

            $scope.ismeridian = true;
            $scope.toggleMode = function () {
              $scope.ismeridian = !$scope.ismeridian;
            };

            $scope.update = function () {
              var d = new Date();
              d.setHours(14);
              d.setMinutes(0);
              $scope.playTime = d;
            };

            $scope.popup1 = {
              opened: false
            };

            function getDayClass(data) {
              var date = data.date,
                mode = data.mode;
              if (mode === 'day') {
                var dayToCheck = new Date(date).setHours(0, 0, 0, 0);

                for (var i = 0; i < $scope.events.length; i++) {
                  var currentDay = new Date($scope.events[i].date).setHours(0, 0, 0, 0);

                  if (dayToCheck === currentDay) {
                    return $scope.events[i].status;
                  }
                }
              }
            }

            var modalInstance = $uibModal.open({
                templateUrl: "addPlaySession.html",
                controller: playSessionModal,
                scope: $scope,
                resolve: {}
            });

            modalInstance.result.then(function (selectedItem) {
                $scope.selected = selectedItem;
            }, function () {
                //console.info('Modal dismissed at: ' + new Date());
            });
          }
         
          var playSessionModal = ["$scope", "$http", "$uibModalInstance", function ($scope, $http, $uibModalInstance) {
            $scope.players = [];
            loadGames();
            loadSelf();
            $scope.selectedGame = {};
            
            $scope.ok = function() {
              $scope.error = {};
              if (!$scope.players || $scope.players.length == 0) {
                $timeout(function() {
                  $scope.error.Players = "there are no players for this playsession";
                });
              }

              if ($scope.players > $scope.selectedGame.MaxPlayer || $scope.players > $scope.selectedGame.MinPlayers) {
                var errVal;
                if ($scope.players > $scope.selectedGame.MaxPlayer) {
                  errVal = "many";
                } else {
                  errVal = "few";
                }
                $timeout(function() {
                  $scope.error.Players = "there are too " + errVal + " players for this playsession";
                });
                return;
              }
              if (!$scope.playDate || $scope.playTime) {
                $timeout(function() {
                  $scope.error.Date = "there is no date for the playsession";
                });
              }
              if (!$scope.selectedGame) {
                $timeout(function() {
                  $scope.error.Game = "there is no game for the playsession";
                });
              }
              var hours = $scope.playTime.getHours();
              var minutes = $scope.playTime.getMinutes();
              var finalTime = new Date($scope.playDate.getFullYear(), $scope.playDate.getMonth(), $scope.playDate.getDate(), hours, minutes, 00);
              console.log(finalTime);
              var newPlaySession = {
                DatePlayed: finalTime,
                BoardGameId: $scope.selectedGame.Id
              }

              var playerIds = new Array;
              for(let player of $scope.players) {
                playerIds.push(player.Id);
              }

              $http.post("AddPlaySession", { PlaySession: newPlaySession, playerIds: playerIds },
              {
                Data: { BoardGame: $scope.pendingPlaySession }
                }).then(function (response) {
                console.log(response.data)
                $timeout(function() {
                  $scope.Plays.push(response.data);
                });
                if (response.data.DatePlayed < Date.now()) {
                  $scope.FinishPlaySession(response.data.Id);
                  console.log("scheduled before now");
                }
                setPagingData($scope.currentPage);
              }, function (error) {
                  console.log(error);
                });
              
              $uibModalInstance.close();
            };

            function loadGames() {
              $http.get('BGcollection').then(function(response) {
                  $timeout(function() {
                    $scope.GameCollection = response.data;
                    console.log($scope.GameCollection);
                  });
                },
                function(error) {
                  console.log(error);
                });
            }

            function loadSelf() {
              $http.get('Self').then(function (response) {
                $timeout(function() {
                  $scope.players.push(response.data);
                });
              }, function (error) {
                console.log(error);
              });
            }

            $scope.searchPlayers = function (searchText) {
              for (let entry of $scope.playerSearchResponse) {
                console.log(searchText, entry.Name);
                  if (searchText === entry.Name) {
                    $timeout(function() {
                      $scope.players.push(entry);
                      console.log($scope.players);
                    });
                  }
                }
              $scope.playerSearchText = "";
              $scope.playerSearchResponse = [];
            };

            $scope.$watch("playerSearchText", function (tmpStr) {
                if (!tmpStr || tmpStr.length <= 2) {
                    return 0;
                }
                if (tmpStr === $scope.playerSearchText) {
                  PlayDataService.playerSearch($scope.playerSearchText).then(function (result) {
                      console.log(result);
                    var searchResults = [];
                      for (let player in result) {
                        var alreadyAdded = false;
                        for (let participant in $scope.players) {
                          if ($scope.players[participant].Id == result[player].Id) {
                            alreadyAdded = true;
                          }
                        }
                        if (!alreadyAdded) {
                          console.log('good stuff happening here');
                          searchResults.push(result[player]);
                        }
                      }
                      $timeout(function() {
                        $scope.playerSearchResponse = searchResults;
                        console.log($scope.playerSearchResponse);
                      });
                    },
                    function(err) {
                      console.log(err);
                    });
                }
            });
             
            $scope.removePlayer = function(player) {
              var index = $scope.players.indexOf(player);
              $timeout(function() {
                $scope.players.splice(index, 1);
              });
            }

            $scope.cancel = function () {
                $uibModalInstance.close();
            };

            $scope.toString = function (value) {
                return value.toString();
            }
          }];

}]);
