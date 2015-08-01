angular.module('Enterprise.Services', [])
.factory('EnterpriseService', [
        '$http',
        function ($http) {
            return {
                GetAllCourses: function () {
                    return $http({
                        method: 'Get',
                        url: 'api/Course/GetAllCourses'
                    });
                }
            }
        }]);