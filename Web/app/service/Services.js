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
                SaveCourse: function (date) {
                    return $http({
                        method: 'POST',
                        url: localStorage['webApiUrl'] + 'api/Course/Post',
                        data: date
                    });
                },
                uploadAttachment: function (formData, documentScope) {
                    var apiUrl = localStorage['webApiUrl'] + 'api/Course/PostUploadAttachment?scope=' + documentScope;
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