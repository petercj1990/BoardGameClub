PlayerApp.factory('DataService',
    ["$http", "$q",
        function ($http, $q) {
            function pullUser(id) {
                var defer = $q.defer();
                if (id === "self") {
                    $http.get('Self').then(function (response) {
                        defer.resolve(response.data.Player);
                    }, function (error) {
                        defer.reject(error)
                    });
                }
                else {
                    console.log(id);
                    $http.get('Find',
                        {
                            headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' },
                            params: { Id: id }
                        }).then(function (response) {
                        defer.resolve(response.data.Player);
                    }, function (error) {
                        defer.reject(error)
                    });
                }
                return defer.promise;
             }

             function addGameToLibrary(boardGameData) {
                 $http.post('Player/Boardgame', boardGameData).then(function (result) {
                     return true;
                 }, function (error) {
                     return error;
                 });
             }

             function removeGameFromLibrary(boardGameData) {
                 $http.delete('Player/Boardgame', boardGameData.id).then(function (result) {
                     return true;
                 }, function (error) {
                     return error;
                 });
             }

             function searchSite(searchText) {
                 var defer = $q.defer();
                 $http.post("Search", { searchText: searchText }
                     ).then(function (data) {
                         defer.resolve(data.data);
                     }, function (err) {
                         defer.reject(err);
                     });
                 return defer.promise;
             }
            
            return {
                pullUser: pullUser,
                addGameToLibrary: addGameToLibrary,
                removeGameFromLibrary: removeGameFromLibrary,
                searchSite: searchSite
                
            };
        }]);
