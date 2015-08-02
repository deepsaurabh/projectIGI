angular.module('Enterprise.Controller', [])

.controller('IndexController', ['$scope', function ($scope) {

}])

.controller('homeController', ['$scope', function ($scope) {
}])

.controller('LoginController', ['$scope', function ($scope) {
    $("#home").removeAttr("style");
    $scope.emailPattern = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/i;
}])

.controller('contactController', ['$scope', function ($scope) {
    $("#home").removeAttr("style");
}])

.controller('aboutusController', ['$scope', function ($scope) {
    $("#home").removeAttr("style");
}])

.controller('courseController', ['$scope', 'EnterpriseService', '$filter', '$route', function ($scope, EnterpriseService, $filter, $route) {
    $("#home").removeAttr("style");
    $scope.CourseList = {
        CourseName: '',
        Price: 0.0,
        imageURL: '',
        CurrencyType: '',
        StartDate: '',
        EndDate: '',
        CourseFreeContent: '',
        CoursePublicContent: '',
        CoursePaidContent: ''
    };
    $scope.Courses = [];
    $scope.showAddCourse = true;
    EnterpriseService.GetAllFreeCourse().success(function (data) {
        if (data && data.course) {
            $scope.Courses = data.course;
            $scope.Courses.forEach(function (key, value) {
                key.startDate = getFormattedDate(new Date(key.startDate));
                key.endDate = getFormattedDate(new Date(key.endDate));
            });
        }
    }).error(function () { });

    function getFormattedDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return day + '/' + month + '/' + year;
    }

    $scope.SaveCourse = function () {
        EnterpriseService.SaveCourse($scope.CourseList)
        .success(function () {
            $route.reload();
        })
        .error(function () { });
    }

    //  Checking extension of the uploaded file
    $scope.checkFileExtension = function (fileAttached) {
        for (var id = 0; id < fileAttached.length; id++) {
            var filename = fileAttached[id].name;
            var ext;

            var dot_pos = filename.lastIndexOf(".");
            if (dot_pos == -1)
                return "";
            ext = filename.substr(dot_pos + 1).toLowerCase();

            if (ext === "exe" || ext === "dll" || ext == "js" || ext == "htm" || ext == "html") {
                $('#txtFileUpload').val("");
                $scope.showAttachmentAlertMessage("Invalid file type.", true);
            } else {
                continue;
            }
        }
    }

    $scope.showAttachmentAlertMessage = function (alertMessage, isShow) {
        if (isShow) {
            $("#attachmentLabel").show();
            $("#errorAttachmentText").html(alertMessage);

            $timeout(function () {
                $("#attachmentLabel").hide();
                $("#errorAttachmentText").html("");
            }, 15000);
        }
    }

    $scope.fileAttachments = [];

    $scope.setFiles = function (element) {
        // Only atmost 2 files can be selected
        if (element.files.length <= 2) {
            var totalSizeLimitInBytes = 1024 * 1024 * parseInt(10);
            var usedSizeLimit = 0
            var attachedFiles = element.files;
            $scope.checkFileExtension(attachedFiles);
            // Filtering those attachments that have IsDeleted=false.
            var unDeletedAttachments = $filter('filter')($scope.fileAttachments, { IsDeleted: false });

            var remainingSizeLimit = 0;
            var limitedNoOfFiles = 3;

            // Checking number of file attached
            // Checking size limit for the attachments
            // Saving attachment to the DB
            if ((attachedFiles.length + unDeletedAttachments.length) <= limitedNoOfFiles) {
                for (var ctr = 0; ctr < attachedFiles.length; ctr++) {
                    if (attachedFiles[ctr].size) {
                        usedSizeLimit = usedSizeLimit + parseInt(attachedFiles[ctr].size)
                    }
                }
                for (var ctr = 0; ctr < unDeletedAttachments.length; ctr++) {
                    if (unDeletedAttachments[ctr].FileSize) {
                        usedSizeLimit = usedSizeLimit + parseInt(unDeletedAttachments[ctr].FileSize)
                    }
                }
                var remainingSizeLimit = totalSizeLimitInBytes - usedSizeLimit;

                // If the size of the attachments size are lesser than the required size
                if (remainingSizeLimit <= 0) {
                    $('#txtFileUpload').val("");
                    $scope.showAttachmentAlertMessage("Size of attachment exceeds the maximum size limit.", true);
                } else {

                    attachedFiles = $.merge(attachedFiles, unDeletedAttachments);

                    // pushing checked attachments to the fileAttachments array.
                    if (attachedFiles.length <= limitedNoOfFiles) {
                        var fd = new FormData();
                        for (var i in attachedFiles) {
                            fd.append("uploadedFile", attachedFiles[i])
                        }

                        //createIntershipRequest.uploadAttachment(fd)
                        //    .success(function (data) {
                        //        if (data) {
                        //            if (data.attachedFiles && data.attachedFiles.length) {
                        //                for (var i = 0, iLen = data.attachedFiles.length; i < iLen; i++) {
                        //                    $scope.InternshipDetails.fileAttachments.push(data.attachedFiles[i]);
                        //                }
                        //            }
                        //            if (data.erroredAttachments) {
                        //                $scope.showAttachmentAlertMessage(data.erroredAttachments, true);
                        //            }
                        //        } else {
                        //            $scope.showAttachmentAlertMessage("File cannot be uploaded due to some error. Please try again.", true);
                        //        }
                        //    }).error(function (data, status, headers, config) {
                        //        $scope.showAttachmentAlertMessage(data.Message, true);
                        //        log.error("Error ~ " + status + config.toString() + data.toString() + headers.toString());
                        //    });
                        $scope.fileAttachments.push({ OriginalName: element.files[0].name, IsDeleted: false });
                        if (!$scope.$$phase) {
                            $scope.$apply();
                        }

                    }
                    else {
                        $('#txtFileUpload').val("");
                        $scope.showAttachmentAlertMessage("Number of attachments reached the maximum limit.", true);
                    }
                }
            }
            else {
                $('#txtFileUpload').val("");
                $scope.showAttachmentAlertMessage("Number of attachments reached the maximum limit.", true);
            }
        }
        else {
            $scope.showAttachmentAlertMessage("Number of attachments reached the maximum limit.", true);
        }
    }

    $scope.DeleteFile = function (fileDetail) {
        fileDetail.IsDeleted = true;
    }

}])

.controller('toolkitController', ['$scope', function ($scope) {
    $("#home").removeAttr("style");
}])

.controller('loginController', ['$scope', 'authService', function ($scope, authService) {
    $("#home").removeAttr("style");
    $scope.UserRegisteration = {
        UserName: '',
        Password: '',
        ConfirmPassword: '',
        EmailAddress: '',
        PhoneNumber: '',
        FirstName: '',
        Gender: '',
        DateOfBirth: ''
    }
    $scope.SubmitRegisteration = function (UserRegisteration) {
        UserRegisteration.DateOfBirth = new Date();
        authService.saveRegistration(UserRegisteration)
        .success(function (data) {
            alert('Successfully registered');
        })
        .error(function () { });
    }
    $scope.Login = function (UserName, Password) {
        authService.login({ userName: UserName, password: Password })
        .success(function (data) {
            var abc = data;
        })
        .error(function () { });
    }
}]);