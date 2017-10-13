BGCApp.factory('DataService',
    ["$http",
        function ($http) {


            var insertEmployee = function () {
                console.log('the service works');
            }


            return {
                insertEmployee: insertEmployee,
                
            };
        }]);
