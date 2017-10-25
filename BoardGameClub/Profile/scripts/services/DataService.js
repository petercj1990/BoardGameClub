PlayerApp.factory('DataService',
    ["$http", 
        function ($http) {
            function pullUser(id) {
                if (id === "self") {
                    console.log("I GET HERE");
                    $http.get('Self').then(function (response) {
                        console.log('Here be the response', response);
                        return response.data;
                    }, function (error) {
                        console.log(error);
                    });
                }
                else {
                    $http.get('Find', id).then(function (response) {
                        return response.data;
                    }, function (error) {
                        alert(error);
                    });
                }
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



            return {
                pullUser: pullUser,
                addGameToLibrary: addGameToLibrary,
                removeGameFromLibrary: removeGameFromLibrary
                
            };
        }]);
