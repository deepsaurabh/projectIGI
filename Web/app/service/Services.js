angular.module('Enterprise.Services', [])
.factory('EnterpriseService', [
        '$http', '$q',
        function ($http, $q) {
            return {
                GetAllFreeCourse: function () {
                    return $http({
                        method: 'Get',
                        url: localStorage['webApiUrl'] + 'api/Course/GetAllFreeCourse'
                    });
                },
                GetAllPaidToolkit: function () {
                    return $http({
                        method: 'Get',
                        url: localStorage['webApiUrl'] + 'api/Toolkit/GetAllPaidToolkit'
                    });
                },
                GetAllPaidCourse: function () {
                    return $http({
                        method: 'Get',
                        url: localStorage['webApiUrl'] + 'api/Course/GetAllPaidCourse'
                    });
                },
                GetAllPublicCourse: function () {
                    return $http({
                        method: 'Get',
                        url: localStorage['webApiUrl'] + 'api/Course/GetAllFreeCourse'
                    });
                },
                GetAllCourse: function () {
                    return $http({
                        method: 'Get',
                        url: localStorage['webApiUrl'] + 'api/Course/GetAllCourse'
                    });
                },
                GetAllPublicToolkit: function () {
                    return $http({
                        method: 'Get',
                        url: localStorage['webApiUrl'] + 'api/Toolkit/GetAllPublicToolkit'
                    });
                },
                GetAllFreeToolkit: function () {
                    return $http({
                        method: 'Get',
                        url: localStorage['webApiUrl'] + 'api/Toolkit/GetAllFreeToolkit'
                    });
                },
                GetAllToolkit: function () {
                    return $http({
                        method: 'Get',
                        url: localStorage['webApiUrl'] + 'api/Toolkit/GetAllToolkit'
                    });
                },
                SaveToolkit: function (date) {
                    return $http({
                        method: 'POST',
                        url: localStorage['webApiUrl'] + 'api/Toolkit/Post',
                        data: date
                    });
                },
                SaveCourse: function (date) {
                    return $http({
                        method: 'POST',
                        url: localStorage['webApiUrl'] + 'api/Course/Post',
                        data: date
                    });
                },
                GetCourseById: function (Id, documentScope) {
                    return $http({
                        method: 'GET',
                        url: localStorage['webApiUrl'] + 'api/Course/GetAllCoursebyId?id=' + Id + '&scope=' + documentScope
                    });
                },
                GetToolkitById: function (Id, documentScope) {
                    return $http({
                        method: 'GET',
                        url: localStorage['webApiUrl'] + 'api/Toolkit/GetAllToolkitbyId?id=' + Id + '&scope=' + documentScope
                    });
                },
                uploadAttachment: function (formData, documentScope) {
                    var apiUrl = localStorage['webApiUrl'] + 'api/Document/PostUploadAttachment?scope=' + documentScope;
                    var deferred = $q.defer();
                    var promise = $http.post(apiUrl, formData, {
                        headers: { 'Content-Type': undefined },
                        transformRequest: angular.identity
                        //headers: { 'Content-Type': false},
                        //transformRequest: function (data) { return data; }
                    }).success(function (data) {
                        deferred.resolve(data);
                    }).error(function (data, status) {
                        log.error('Error occured while serving uploadAttachment API ~ Message: ' + data.Message + '  MessageDetail : ' + data.MessageDetail + ' HTTP Status : ' + status);
                        deferred.reject(data);
                    });
                    return promise;
                },
            }
        }]);