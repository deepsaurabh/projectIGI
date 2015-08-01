angular.module('Enterprise.Controller', [])

.controller('IndexController', ['$scope', function ($scope) {

}])

.controller('homeController', ['$scope', function ($scope) {
}])

.controller('contactController', ['$scope', function ($scope) {
    $("#home").removeAttr("style");
}])

    .controller('aboutusController', ['$scope', function ($scope) {
        $("#home").removeAttr("style");
    }])

        .controller('courseController', ['$scope', 'EnterpriseService', function ($scope, EnterpriseService) {
            $("#home").removeAttr("style");
            EnterpriseService.GetAllCourses().success(function () { }).error(function () { });
        }])
    .controller('toolkitController', ['$scope', function ($scope) {
        $("#home").removeAttr("style");
    }])


.controller('loginController', ['$scope', function ($scope) {

}]);