angular.module('PlayerApp')
    .controller('boardgameModal', ['$scope', '$uibModalInstance',  "$http",
        function ($scope, $uibModalInstance, $http) {
            $scope.BG;
            console.log($scope.BG);
            $http.get("BoardgameData",
                {
                    headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' },
                    params: { searchText: $scoope.BG }
                }).then(function (data) {
                    console.log(error);
                }, function (error) {
                    console.error(error);
            });

            $scope.submitForm = function () {
              console.log("submitting bg");
            };

            $scope.cancel = function () {
              $modalInstance.dismiss('cancel');
            };
    }]);
