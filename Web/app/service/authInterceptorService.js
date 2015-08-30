'use strict';
app.factory('authInterceptorService', ['$q', '$location', function ($q, $location) {

    var authInterceptorServiceFactory = {};

    var _request = function (config) {

        config.headers = config.headers || {};

        if (localStorage && localStorage.getItem('authorizationData')) {
            var authorizationDataString = localStorage.getItem('authorizationData');
            authorizationData = JSON.parse(authorizationDataString);
            config.headers.Authorization = 'Bearer ' + authorizationData.token;
        }
        return config;
    }

    var _responseError = function (rejection) {
        if (rejection.status === 401) {
            $location.path('/login');
        }
        return $q.reject(rejection);
    }

    authInterceptorServiceFactory.request = _request;
    authInterceptorServiceFactory.responseError = _responseError;

    return authInterceptorServiceFactory;
}]);
