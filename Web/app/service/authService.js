angular.module('Enterprise.authServices', [])
.factory('authService', ['$http', '$q', function ($http, $q) {

    var serviceBase = localStorage['webApiUrl'];

    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        userName: ""
    };

    var _saveRegistration = function (registration) {

        _logOut();
        var requestHeaders = {
            'Access-Control-Allow-Origin': 'http://localhost:63249'
        };

        return $http({
            method: 'POST',
            headers: requestHeaders,
            url: serviceBase + 'api/Account/RegisterCustomer',
            data: registration
        });

    };

    var _login = function (loginData) {

        var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

        var deferred = $q.defer();

        var requestHeaders = {
            'Access-Control-Allow-Origin': 'http://localhost:63249',
            'Content-Type': 'application/x-www-form-urlencoded'
        };

        $http({
            method: 'POST',
            url: serviceBase + 'token',
            headers: requestHeaders,
            data: data
        }).then(function (response) {

            localStorage.setItem('authorizationData', { token: response.data.access_token, userName: loginData.userName });
            _authentication.isAuth = true;
            _authentication.userName = loginData.userName;

            deferred.resolve(response);

        });
        return deferred.promise;

    };

    var _logOut = function () {

        localStorage.removeItem('authorizationData');

        _authentication.isAuth = false;
        _authentication.userName = "";

    };

    var _fillAuthData = function () {

        var authData = localStorage.getItem('authorizationData');
        if (authData) {
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
        }

    }

    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;

    return authServiceFactory;
}]);