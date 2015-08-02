angular.module('Enterprise.Services', [])
.factory('EnterpriseService', [
        '$http',
        function ($http) {
            return {
                GetAllFreeCourse: function () {
                    return $http({
                        method: 'Get',
                        url: localStorage['webApiUrl'] + 'api/Course/GetAllFreeCourse'
                    });
                },
                SaveCourse: function (date) {
                    return $http({
                        method: 'POST',
                        url: localStorage['webApiUrl'] + 'api/Course/Post',
                        data: date
                    });
                }
            }
        }]);