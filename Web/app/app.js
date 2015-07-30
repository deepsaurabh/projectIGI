angular.module('IGI', ['ngRoute', 'Enterprise.Controller'])

.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/login', { templateUrl: 'partials/Login.html', Controller: 'loginController' });
    $routeProvider.when('/Home', { templateUrl: 'partials/Home.html', Controller: 'homeController' });
    $routeProvider.when('/ContactUs', { templateUrl: 'partials/Contact.html', Controller: 'contactController' });
    $routeProvider.when('/AboutUs', { templateUrl: 'partials/AboutUs.html', Controller: 'aboutusController' });
    $routeProvider.otherwise({ redirectTo: '/home' });
}]);