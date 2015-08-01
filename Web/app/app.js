angular.module('IGI', ['ngRoute', 'Enterprise.Controller', 'Enterprise.Services'])

.run(['$rootScope', function ($rootScope) {
    localStorage['webApiUrl'] = window.location.protocol + "//" + window.location.hostname + "/api/";
}])

.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/login', { templateUrl: 'partials/Login.html', Controller: 'loginController' });
    $routeProvider.when('/Home', { templateUrl: 'partials/Home.html', Controller: 'homeController' });
    $routeProvider.when('/ContactUs', { templateUrl: 'partials/Contact.html', Controller: 'contactController' });
    $routeProvider.when('/AboutUs', { templateUrl: 'partials/AboutUs.html', Controller: 'aboutusController' });
    $routeProvider.when('/Courses', { templateUrl: 'partials/Courses.html', Controller: 'courseController' });
    $routeProvider.when('/ToolKit', { templateUrl: 'partials/Courses.html', Controller: 'toolkitController' });
    $routeProvider.when('/Login', { templateUrl: 'partials/Login.html', Controller: 'LoginController' });
    $routeProvider.when('/Register', { templateUrl: 'partials/Login.html', Controller: 'LoginController' });
    $routeProvider.otherwise({ redirectTo: '/Home' });
}]);