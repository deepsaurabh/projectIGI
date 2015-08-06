angular.module('Enterprise.Controller', [])

.controller('IndexController', ['$scope', function ($scope) {

}])

.controller('homeController', ['$scope', function ($scope) {
}])

.controller('LoginController', ['$scope', 'authService', function ($scope, authService) {
    $("#home").removeAttr("style");
    $scope.emailPattern = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/i;

    //$scope.login = function (userName, password) {
    //    loginData = {};
    //    loginData.userName = userName;
    //    loginData.password = password;
    //    authService.login(loginData).success(function (response) {

    //        console.log("login Successful")
    //    }).error(function () {
    //        console.log("Error in login");
    //    });
    //}

    

}])

.controller('contactController', ['$scope', function ($scope) {
    $("#home").removeAttr("style");
}])

.controller('aboutusController', ['$scope', function ($scope) {
    $("#home").removeAttr("style");
}])

.controller('courseController', ['$scope', 'EnterpriseService', '$filter', '$route', function ($scope, EnterpriseService, $filter, $route) {
    $("#home").removeAttr("style");
    $scope.CurrencyType = [
        { Key: '1', Value: 'INR' },
        { Key: '2', Value: 'Dollar' },
        { Key: '3', Value: 'Euro' }
    ];
    $scope.CourseList = {
        CourseID: 0,
        CourseName: '',
        Price: 0.0,
        imageURL: '',
        CurrencyType: '1',
        StartDate: '',
        EndDate: '',
        CourseFreeContent: '',
        CoursePublicContent: '',
        CoursePaidContent: '',
        FreeContentImageId: '',
        PublicContentImageId: '',
        PaidContentImageId: '',
        ImageURL: '',
        Description: '',
        isSelected: false
    };

    $scope.Courses = [];

    $scope.showAddCourse = true;

    $scope.Init = function () {
        EnterpriseService.GetAllFreeCourse().success(function (data) {
            if (data && data.course) {
                $scope.Courses = data.course;
                $scope.Courses.forEach(function (key, value) {
                    if (key.courseFreeContent)
                        key.description = key.courseFreeContent;
                    else if (key.coursePaidContent)
                        key.description = key.coursePaidContent;
                    else if (key.coursePublicContent)
                        key.description = key.coursePublicContent;
                    key.startDate = getFormattedDate(new Date(key.startDate));
                    key.endDate = getFormattedDate(new Date(key.endDate));
                });
                if ($scope.Courses.length > 0)
                    $scope.DisplayCourse($scope.Courses[0]);
            }
        }).error(function () { });
    }

    $scope.DisplayCourse = function (course) {
        $scope.Courses.forEach(function (key, value) {
            key.isSelected = false;
        });
        course.isSelected = true;
        $scope.CourseList.CourseID = course.courseID;
        $scope.CourseList.CourseName = course.courseName;
        $scope.CourseList.Price = course.price;
        $scope.CourseList.CurrencyType = $filter('filter')($scope.CurrencyType, { Key: course.currencyType })[0].Value;
        $scope.CourseList.StartDate = course.startDate;
        $scope.CourseList.EndDate = course.endDate;
        $scope.CourseList.ImageURL = course.imageURL;
        $scope.CourseList.Description = course.courseFreeContent;
    }

    function getFormattedDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return day + '/' + month + '/' + year;
    }

    $scope.SaveCourse = function (CourseList) {
        EnterpriseService.SaveCourse(CourseList)
        .success(function () {
            alert('Course saved successfully');
            $route.reload();
        })
        .error(function () { });
    }

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
        }
    }

    $scope.fileAttachments = [];

    $scope.setFiles = function (element, documentScope) {
        // Only atmost 2 files can be selected
        if (element.files.length == 1) {
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

                        EnterpriseService.uploadAttachment(fd, documentScope)
                            .success(function (data) {
                                if (data && data.attachedFiles && data.attachedFiles.length > 0) {
                                    switch (documentScope) {
                                        case 1:
                                            $scope.CourseList.FreeContentImageId = data.attachedFiles[0].id
                                            break;
                                        case 2:
                                            $scope.CourseList.PublicContentImageId = data.attachedFiles[0].id
                                            break;
                                        case 3:
                                            $scope.CourseList.PaidContentImageId = data.attachedFiles[0].id
                                            break;
                                    }
                                } else {
                                    $scope.showAttachmentAlertMessage("File cannot be uploaded due to some error. Please try again.", true);
                                }
                            }).error(function (data, status, headers, config) {
                                $scope.showAttachmentAlertMessage(data.Message, true);
                            });
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

.controller('loginController', ['$scope', 'authService', '$location', function ($scope, authService, $location) {
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
        authService.login({ userName: UserName, password: Password }).then(function (data) {
            var ss = data;
            $location.url('/Home');
        });
        
    }
}]);