
angular.module('IGI', ['ngRoute', 'Enterprise.Controller', 'Enterprise.Services'])

.run(['$rootScope', function ($rootScope) {
    localStorage['webApiUrl'] = window.location.protocol + "//" + window.location.hostname + "/api/";
}])

.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/login', { templateUrl: 'partials/Login.html', controller: 'loginController' });
    $routeProvider.when('/Home', { templateUrl: 'partials/Home.html', controller: 'homeController' });
    $routeProvider.when('/ContactUs', { templateUrl: 'partials/Contact.html', controller: 'contactController' });
    $routeProvider.when('/AboutUs', { templateUrl: 'partials/AboutUs.html', controller: 'aboutusController' });
    $routeProvider.when('/Courses', { templateUrl: 'partials/Courses.html', controller: 'courseController' });
    $routeProvider.when('/ToolKit', { templateUrl: 'partials/Courses.html', controller: 'toolkitController' });
    $routeProvider.when('/Login', { templateUrl: 'partials/Login.html', controller: 'LoginController' });
    $routeProvider.when('/Register', { templateUrl: 'partials/Login.html', controller: 'LoginController' });
    $routeProvider.otherwise({ redirectTo: '/Home' });
}]);