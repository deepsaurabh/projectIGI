
angular.module('IGI', ['ngRoute', 'Enterprise.Controller', 'Enterprise.Services', 'Enterprise.authServices'])

.run(['$rootScope', function ($rootScope) {
    //localStorage['webApiUrl'] = window.location.protocol + "//" + window.location.host + "/api/";
    localStorage['webApiUrl'] = 'http://localhost:51473/';    
}])

.config(['$routeProvider', function ($routeProvider) {    
    $routeProvider.when('/Home', { templateUrl: 'partials/Home.html', controller: 'homeController' });
    $routeProvider.when('/ContactUs', { templateUrl: 'partials/Contact.html', controller: 'contactController' });
    $routeProvider.when('/AboutUs', { templateUrl: 'partials/AboutUs.html', controller: 'aboutusController' });
    $routeProvider.when('/Courses', { templateUrl: 'partials/Courses.html', controller: 'courseController' });
    $routeProvider.when('/ToolKit', { templateUrl: 'partials/Courses.html', controller: 'toolkitController' });
    $routeProvider.when('/Login', { templateUrl: 'partials/Login.html', controller: 'loginController' });
    $routeProvider.when('/Register', { templateUrl: 'partials/Login.html', controller: 'loginController' });
    $routeProvider.when('/AddCourse', { templateUrl: 'partials/AddContent.html', controller: 'courseController' });
    $routeProvider.otherwise({ redirectTo: '/Home' });
}]);