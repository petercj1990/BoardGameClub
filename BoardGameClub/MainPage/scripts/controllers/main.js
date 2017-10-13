'use strict';

angular.module('BGCApp')
  .controller('MainCtrl', function () {
    this.awesomeThings = [
      'HTML5 Boilerplate',
      'AngularJS',
      'Karma'
      ];
    var runOnLoad = function () {
        console.log("pweaz kiw mee");
    };
    runOnLoad();
  });
