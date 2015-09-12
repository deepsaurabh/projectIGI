
var app = angular.module('IGI', ['ngRoute', 'Enterprise.Controller', 'Enterprise.Services', 'Enterprise.authServices'])

.run(['$rootScope', function ($rootScope) {
    localStorage['webApiUrl'] = 'http://localhost:51473/';

    $rootScope.userName = '';
    $rootScope.isLoggedIn = false;
    $rootScope.role = "free";

    if (localStorage && localStorage.getItem('authorizationData')) {
        var authorizationDataString = localStorage.getItem('authorizationData');
        authorizationData = JSON.parse(authorizationDataString);
        $rootScope.userName = authorizationData.userName;
        $rootScope.isLoggedIn = true;
        $rootScope.role = authorizationData.role;
    }

    $rootScope.isAdmin = function () {
        if ($rootScope.role.toLowerCase() == 'admin')
            return true;
        
        return false;
    }

    $rootScope.isCustomer = function () {
        if ($rootScope.role.toLowerCase() == 'customer')
            return true;

        return false;
    }

    $rootScope.isRegisteredUser = function () {
        if ($rootScope.role.toLowerCase() == 'free' )
            return false;
        return true;
    }

    
}])
    
.config(['$routeProvider', function ($routeProvider) {    
    $routeProvider.when('/Home', { templateUrl: 'partials/Home.html', controller: 'homeController' });
    $routeProvider.when('/ContactUs', { templateUrl: 'partials/Contact.html', controller: 'contactController' });
    $routeProvider.when('/AboutUs', { templateUrl: 'partials/AboutUs.html', controller: 'aboutusController' });
    $routeProvider.when('/Courses', { templateUrl: 'partials/Courses.html', controller: 'courseController' });
    $routeProvider.when('/ToolKit', { templateUrl: 'partials/ToolkitContent.html', controller: 'toolkitController' });
    $routeProvider.when('/AddToolkit', { templateUrl: 'partials/AddToolkitContent.html', controller: 'toolkitController' });
    $routeProvider.when('/Login', { templateUrl: 'partials/Login.html', controller: 'loginController' });
    $routeProvider.when('/Register', { templateUrl: 'partials/Login.html', controller: 'loginController' });
    $routeProvider.when('/AddCourse', { templateUrl: 'partials/AddContent.html', controller: 'courseController' });
    $routeProvider.when('/PurchasedCourse', { templateUrl: 'partials/Courses.html', controller: 'courseController' });
    $routeProvider.when('/PurchasedToolkit', { templateUrl: 'partials/ToolkitContent.html', controller: 'toolkitController' });
    $routeProvider.when('/Cart', { templateUrl: 'partials/Cart.html', controller: 'cartController' });

    $routeProvider.when('/Address', { templateUrl: 'partials/AddAddress.html', controller: 'addressController' });
    $routeProvider.otherwise({ redirectTo: '/Home' });
}]);


app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});