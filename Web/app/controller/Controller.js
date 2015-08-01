angular.module('Enterprise.Controller', [])

.controller('IndexController', ['$scope', function ($scope) {

}])

.controller('homeController', ['$scope', function ($scope) {
}])

.controller('LoginController', ['$scope', function ($scope) {
    $("#home").removeAttr("style");

    $scope.dateOptions = {
        'year-format': "'yy'",
        'starting-day': 1
    };
}])

.controller('contactController', ['$scope', function ($scope) {
    $("#home").removeAttr("style");
}])

.controller('aboutusController', ['$scope', function ($scope) {
    $("#home").removeAttr("style");
}])

.controller('courseController', ['$scope', 'EnterpriseService', function ($scope, EnterpriseService) {
    $("#home").removeAttr("style");
    $scope.CourseList = {
        courseName: '',
        courseCode: '',
        price: 0.0,
        imageURL: '',
        description: ''
    };
    EnterpriseService.GetAllCourses().success(function (data) {
        $scope.CourseList = data;
    }).error(function () { });
}])

.controller('toolkitController', ['$scope', function ($scope) {
    $("#home").removeAttr("style");
}])

.controller('loginController', ['$scope', function ($scope) {

}]);