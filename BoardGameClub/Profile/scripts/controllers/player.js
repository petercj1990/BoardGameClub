'use strict';

angular.module('PlayerApp')
  .controller('PlayerCtrl', function () {
   
    var runOnLoad = function () {
        console.log("I load the players section");
    };
    runOnLoad();
  });
