BGCApp.factory('PlayerService',
    ["$http",
        function ($http) {

          var login = function (employee) {
            return $http.post("Account/Hi", employee);
          };

          var logout = function () {
            console.log('the service works you have logged out');
          };

            return {
                login: login,
                logout: logout
            };
        }]);
