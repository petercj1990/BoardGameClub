angular.module('PlayerApp')
    .controller('playSession', ['$scope', '$uibModalInstance', "$http",
        function ($scope, $uibModalInstance, $http) {
            
            $http.defaults.headers.common["Content-Type"] = "application/x-www-form-urlencoded";
            
            $scope.user = DataService.pullUser("self");

        }]);
