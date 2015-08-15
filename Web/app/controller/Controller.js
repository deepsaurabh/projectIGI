angular.module('Enterprise.Controller', [])

.controller('IndexController', ['$scope', 'authService', '$location', '$rootScope', function ($scope, authService, $location, $rootScope) {
    $scope.userName = '';
    $scope.isLoggedIn = false;
    $scope.role = "free";

    if (localStorage && localStorage.getItem('authorizationData')) {
        var authorizationDataString = localStorage.getItem('authorizationData');
        authorizationData = JSON.parse(authorizationDataString);
        $scope.userName = authorizationData.userName;
        $scope.isLoggedIn = true;
        $scope.role = authorizationData.role;
    }


    $scope.logout = function () {
        authService.logOut();
        $rootScope.userName = '';
        $rootScope.isLoggedIn = false;
        $rootScope.role = "free";
        window.open('http://localhost:63249/#/Login', '_self')
    }

    $scope.isAdmin = function () {
        if ($scope.role.toLowerCase() == 'admin')
            return true;
        else
            return false;
        //return true; //Just as a test to make sure it works
    }
}])

.controller('homeController', ['$scope', function ($scope) {
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
    $scope.DocumentScope = {
        FreeContent: 1,
        PublicContent: 2,
        PaidContent: 3,
        AllContent: 4
    };
    $scope.CourseList = {
        courseID: 0,
        courseName: '',
        price: 0.0,
        imageURL: '',
        currencyType: '1',
        startDate: '',
        endDate: '',
        freeContent: {
            description: '',
            fileAttachment: []
        },
        publicContent: {
            description: '',
            fileAttachment: []
        },
        paidContent: {
            description: '',
            fileAttachment: []
        },
        isSelected: false
    };

    $scope.isAdmin = $scope.$parent.isAdmin();

    $scope.Courses = [];

    $scope.documentScope = $scope.isAdmin ? $scope.DocumentScope.AllContent : ($scope.$parent.isLoggedIn ? $scope.DocumentScope.FreeContent : $scope.DocumentScope.PublicContent);

    $scope.showAddCourse = true;

    $scope.Init = function () {
        if ($scope.isAdmin) {
            EnterpriseService.GetAllCourse().success(function (data) {
                if (data && data.course) {
                    $scope.Courses = data.course;
                    if ($scope.Courses.length > 0) {
                        $scope.DisplayCourse($scope.Courses[0]);
                    }
                }
            }).error(function () { });
        }
        else if ($scope.$parent.isLoggedIn) {
            EnterpriseService.GetAllFreeCourse().success(function (data) {
                if (data && data.course) {
                    $scope.Courses = data.course;
                    if ($scope.Courses.length > 0) {
                        $scope.DisplayCourse($scope.Courses[0]);
                    }
                }
            }).error(function () { });
        }
        else {
            EnterpriseService.GetAllPublicCourse().success(function (data) {
                if (data && data.course) {
                    $scope.Courses = data.course;
                    if ($scope.Courses.length > 0) {
                        $scope.DisplayCourse($scope.Courses[0]);
                    }
                }
            }).error(function () { });
        }
    }


    $scope.classInitialize = function (scope) {
        if (scope == $scope.DocumentScope.PublicContent) {
            if ($scope.CourseList.freeContent == null || $scope.CourseList.freeContent.fileAttachment == null || $scope.CourseList.freeContent.fileAttachment.length == 0)
                return "row";
        }
        else if (scope == $scope.DocumentScope.PaidContent) {
            if ($scope.CourseList.freeContent == null || $scope.CourseList.freeContent.fileAttachment == null || $scope.CourseList.freeContent.fileAttachment.length == 0) {
                if ($scope.CourseList.publicContent == null || $scope.CourseList.publicContent.fileAttachment == null || $scope.CourseList.publicContent.fileAttachment.length == 0) {
                    return "row";
                }
            }
        }
    }

    $scope.DisplayCourse = function (courseObj) {
        $scope.Courses.forEach(function (key, value) {
            key.isSelected = false;
        });
        courseObj.isSelected = true;
        EnterpriseService.GetCourseById(courseObj.id, $scope.documentScope).success(function (data) {
            if (data && data.course) {
                $scope.CourseList = data.course;
                $scope.CourseList.startDate = getFormattedDate(new Date($scope.CourseList.startDate));
                $scope.CourseList.endDate = getFormattedDate(new Date($scope.CourseList.endDate));
                $scope.CourseList.price = parseFloat($scope.CourseList.price);
                $scope.CourseList.currencyType = $scope.isAdmin ? $scope.CourseList.currencyType.toString() : $filter('filter')($scope.CurrencyType, { Key: $scope.CourseList.currencyType })[0].Value;
            }

        }).error(function () {
        });
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
        if (CourseList.freeContent && CourseList.freeContent.fileAttachment) {
            CourseList.freeContent.fileAttachment.forEach(function (key, value) {
                key.imageURL = ''
            });
        }
        if (CourseList.paidContent && CourseList.paidContent.fileAttachment) {
            CourseList.paidContent.fileAttachment.forEach(function (key, value) {
                key.imageURL = ''
            });
        }
        if (CourseList.publicContent && CourseList.publicContent.fileAttachment) {
            CourseList.publicContent.fileAttachment.forEach(function (key, value) {
                key.imageURL = ''
            });
        }
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

            // Checking number of file attached
            // Checking size limit for the attachments
            // Saving attachment to the DB

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
                var fd = new FormData();
                for (var i in attachedFiles) {
                    fd.append("uploadedFile", attachedFiles[i])
                }

                EnterpriseService.uploadAttachment(fd, documentScope)
                    .success(function (data) {
                        if (data && data.attachedFiles && data.attachedFiles.length > 0) {
                            var attachmentObj = {
                                attachmentID: data.attachedFiles[0].id,
                                documentName: data.attachedFiles[0].documentName,
                                isDeleted: false,
                                imageURL: data.imageURL
                            };
                            switch (documentScope) {
                                case 1:
                                    {
                                        $scope.CourseList.freeContent.fileAttachment.push(attachmentObj);
                                        break;
                                    }
                                case 2:
                                    {
                                        $scope.CourseList.publicContent.fileAttachment.push(attachmentObj);
                                        break;
                                    }
                                case 3:
                                    {
                                        $scope.CourseList.paidContent.fileAttachment.push(attachmentObj);
                                        break;
                                    }
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


                else {
                    $('#txtFileUpload').val("");
                    $scope.showAttachmentAlertMessage("Number of attachments reached the maximum limit.", true);
                }
            }

        }
        else {
            $scope.showAttachmentAlertMessage("Number of attachments reached the maximum limit.", true);
        }
    }

    $scope.DeleteFile = function (DocumentList, index) {
        DocumentList.splice(index, 1);
    }

}])

.controller('toolkitController', ['$scope', 'EnterpriseService', '$filter', '$route', function ($scope, EnterpriseService, $filter, $route) {
    $("#home").removeAttr("style");
    $scope.CurrencyType = [
        { Key: '1', Value: 'INR' },
        { Key: '2', Value: 'Dollar' },
        { Key: '3', Value: 'Euro' }
    ];
    $scope.DocumentScope = {
        FreeContent: 1,
        PublicContent: 2,
        PaidContent: 3,
        AllContent: 4
    };
    $scope.ToolkitList = {
        toolkitID: 0,
        toolkitName: '',
        price: 0.0,
        imageURL: '',
        currencyType: '1',
        startDate: '',
        endDate: '',
        freeContent: {
            description: '',
            fileAttachment: []
        },
        publicContent: {
            description: '',
            fileAttachment: []
        },
        paidContent: {
            description: '',
            fileAttachment: []
        },
        isSelected: false
    };

    $scope.isAdmin = $scope.$parent.isAdmin();

    $scope.Toolkits = [];

    $scope.showAddCourse = true;

    $scope.documentScope = $scope.isAdmin ? $scope.DocumentScope.AllContent : ($scope.$parent.isLoggedIn ? $scope.DocumentScope.FreeContent : $scope.DocumentScope.PublicContent);

    $scope.Init = function () {
        if ($scope.isAdmin) {
            EnterpriseService.GetAllToolkit().success(function (data) {
                if (data && data.toolkit) {
                    $scope.Toolkits = data.toolkit;
                    if ($scope.Toolkits.length > 0) {
                        $scope.DisplayToolkit($scope.Toolkits[0]);
                    }
                }
            }).error(function () { });
        }
        else if ($scope.$parent.isLoggedIn) {
            EnterpriseService.GetAllFreeToolkit().success(function (data) {
                if (data && data.toolkit) {
                    $scope.Toolkits = data.toolkit;
                    if ($scope.Toolkits.length > 0) {
                        $scope.DisplayToolkit($scope.Toolkits[0]);
                    }
                }
            }).error(function () { });
        }
        else {
            EnterpriseService.GetAllPublicToolkit().success(function (data) {
                if (data && data.toolkit) {
                    $scope.Toolkits = data.toolkit;
                    if ($scope.Toolkits.length > 0) {
                        $scope.DisplayToolkit($scope.Toolkits[0]);
                    }
                }
            }).error(function () { });
        }
    }

    $scope.classInitialize = function (scope) {
        if (scope == $scope.DocumentScope.PublicContent) {
            if ($scope.ToolkitList.freeContent == null || $scope.ToolkitList.freeContent.fileAttachment == null || $scope.ToolkitList.freeContent.fileAttachment.length == 0)
                return "row";
        }
        else if (scope == $scope.DocumentScope.PaidContent) {
            if ($scope.ToolkitList.freeContent == null || $scope.ToolkitList.freeContent.fileAttachment == null || $scope.ToolkitList.freeContent.fileAttachment.length == 0) {
                if ($scope.ToolkitList.publicContent == null || $scope.ToolkitList.publicContent.fileAttachment == null || $scope.ToolkitList.publicContent.fileAttachment.length == 0) {
                    return "row";
                }
            }
        }
    }

    $scope.DisplayToolkit = function (toolkitObj) {
        $scope.Toolkits.forEach(function (key, value) {
            key.isSelected = false;
        });
        toolkitObj.isSelected = true;
        EnterpriseService.GetToolkitById(toolkitObj.id, $scope.documentScope).success(function (data) {
            if (data && data.course) {
                $scope.ToolkitList = data.course;
                $scope.ToolkitList.startDate = getFormattedDate(new Date($scope.ToolkitList.startDate));
                $scope.ToolkitList.endDate = getFormattedDate(new Date($scope.ToolkitList.endDate));
                $scope.ToolkitList.price = parseFloat($scope.ToolkitList.price);
                $scope.ToolkitList.currencyType = $scope.isAdmin ? $scope.ToolkitList.currencyType.toString() : $filter('filter')($scope.CurrencyType, { Key: $scope.ToolkitList.currencyType })[0].Value;
            }

        }).error(function () {
        });
    }

    function getFormattedDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return day + '/' + month + '/' + year;
    }

    $scope.SaveToolkit = function (ToolkitList) {
        if (ToolkitList.freeContent && ToolkitList.freeContent.fileAttachment) {
            ToolkitList.freeContent.fileAttachment.forEach(function (key, value) {
                key.imageURL = ''
            });
        }
        if (ToolkitList.paidContent && ToolkitList.paidContent.fileAttachment) {
            ToolkitList.paidContent.fileAttachment.forEach(function (key, value) {
                key.imageURL = ''
            });
        }
        if (ToolkitList.publicContent && ToolkitList.publicContent.fileAttachment) {
            ToolkitList.publicContent.fileAttachment.forEach(function (key, value) {
                key.imageURL = ''
            });
        }
        EnterpriseService.SaveToolkit(ToolkitList)
        .success(function () {
            alert('Toolkit saved successfully');
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

            // Checking number of file attached
            // Checking size limit for the attachments
            // Saving attachment to the DB

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
                var fd = new FormData();
                for (var i in attachedFiles) {
                    fd.append("uploadedFile", attachedFiles[i])
                }

                EnterpriseService.uploadAttachment(fd, documentScope)
                    .success(function (data) {
                        if (data && data.attachedFiles && data.attachedFiles.length > 0) {
                            var attachmentObj = {
                                attachmentID: data.attachedFiles[0].id,
                                documentName: data.attachedFiles[0].documentName,
                                isDeleted: false,
                                imageURL: data.imageURL
                            };
                            switch (documentScope) {
                                case 1:
                                    {
                                        $scope.ToolkitList.freeContent.fileAttachment.push(attachmentObj);
                                        break;
                                    }
                                case 2:
                                    {
                                        $scope.ToolkitList.publicContent.fileAttachment.push(attachmentObj);
                                        break;
                                    }
                                case 3:
                                    {
                                        $scope.ToolkitList.paidContent.fileAttachment.push(attachmentObj);
                                        break;
                                    }
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


                else {
                    $('#txtFileUpload').val("");
                    $scope.showAttachmentAlertMessage("Number of attachments reached the maximum limit.", true);
                }
            }

        }
        else {
            $scope.showAttachmentAlertMessage("Number of attachments reached the maximum limit.", true);
        }
    }

    $scope.DeleteFile = function (DocumentList, index) {
        DocumentList.splice(index, 1);
    }

}])

.controller('loginController', ['$scope', 'authService', '$location', function ($scope, authService, $location) {
    $("#home").removeAttr("style");
    $scope.UserRegisteration = {
        UserName: '',
        Password: '',
        ConfirmPassword: '',
        EmailAddress: '',
        LastName: '',
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
            $scope.UserRegisteration();
        })
        .error(function (error) {
            $scope.errorMessage = error.message;
            alert($scope.errorMessage);
        });
    }
    $scope.Login = function (UserName, Password) {
        authService.login({ userName: UserName, password: Password }).then(function (data) {
            window.open('http://localhost:63249', '_self')
        });

    }
}]);