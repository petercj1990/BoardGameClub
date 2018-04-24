PlaySessionsApp.factory('PlayDataService',
    ["$http", "$q",
        function ($http, $q) {
   
            function getPlaysessions(option) {
              var defer = $q.defer();
              //this may need to be refactored
                $http.post("getplaySessions").then(function (data) {
                    defer.resolve(data.data);
                }, function (err) {
                    defer.reject(err);
                });
                return defer.promise;
            }

            function playerSearch(searchText) {
              var defer = $q.defer();
              $http.post("PlayerSearch", { searchText: searchText }
              ).then(function (data) {
                console.log(data.data);
                defer.resolve(data.data);
              }, function (err) {
                defer.reject(err);
              });
              return defer.promise;
            }

            function getPlaySession(id) {
              console.log(id);
              var defer = $q.defer();
              //this may need to be refactored
              var list = [];
              list.push(id);
              $http.post("getplaySession", { id: list }).then(function (data) {
                console.log(data.data[0]);
                defer.resolve(data.data[0]);
              }, function (err) {
                defer.reject(err);
              });
              return defer.promise;
            }

            function UpdatePlaySessionRecord(participant, id){
              $http.post("UpdatePlaySessionRecord", { participants: participant, PSId: id}).then(function (data) {
                console.log(data.data);
              }, function (err) {
                console.log(err);
              });
            }

            function DeleteSession(id) {
              $http.post("DeletePlaySession", {SessionId: id}).then(function (data) {
                console.log(data.data);
              }, function (err) {
                console.log(err);
              });
            }
            
        return {
          getPlaysessions: getPlaysessions,
          getPlaySession: getPlaySession,
          playerSearch: playerSearch,
          UpdatePlaySessionRecord: UpdatePlaySessionRecord,
          DeleteSession: DeleteSession
        };
    }]);
