angular.module('Enterprise.Controller', ['ngAnimate'])

.controller('IndexController', ['$scope', 'authService', '$location', '$rootScope', function ($scope, authService, $location, $rootScope) {
    $scope.logout = function () {
        authService.logOut();
        $rootScope.userName = '';
        $rootScope.isLoggedIn = false;
        $rootScope.role = "free";
        window.open('http://localhost:63249/#/Login', '_self')
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

.controller('courseController', ['$scope', 'EnterpriseService', '$filter', '$route', '$location', function ($scope, EnterpriseService, $filter, $route, $location) {
    $("#home").removeAttr("style");
    $scope.CurrencyType = [
        { Key: '1', Value: 'INR' },
        //{ Key: '2', Value: 'Dollar' },
        //{ Key: '3', Value: 'Euro' }
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
    $scope.isPaidPage = false;
    if ($route && $route.current && $route.current.$$route && $route.current.$$route.originalPath && $route.current.$$route.originalPath.toLowerCase().indexOf('purchasedcourse') > 0) {
        $scope.isPaidPage = true;
    }
    $scope.isAdmin = $scope.isAdmin();

    $scope.Courses = [];

    $scope.documentScope = $scope.isAdmin ? $scope.DocumentScope.AllContent : ($scope.isPaidPage ? $scope.DocumentScope.PaidContent : ($scope.$parent.isLoggedIn ? $scope.DocumentScope.FreeContent : $scope.DocumentScope.PublicContent));

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
        else if ($scope.isPaidPage) {
            EnterpriseService.GetAllPaidCourse().success(function (data) {
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
        return year + '-' + month + '-' + day;
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

        //CourseList.endDate = new Date(angular.element('#EndDate').val());

        //CourseList.startDate = new Date(angular.element('#StartDate').val());

        CourseList.endDate = angular.element('#EndDate').val();

        CourseList.startDate = angular.element('#StartDate').val();

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

    $scope.direction = 'left';
    $scope.currentIndex = 0;

    $scope.setCurrentSlideIndex = function (index) {
        $scope.direction = (index > $scope.currentIndex) ? 'left' : 'right';
        $scope.currentIndex = index;
    };

    $scope.isCurrentSlideIndex = function (index) {
        return $scope.currentIndex === index;
    };

    $scope.prevSlide = function (index) {
        if (index == 1) {
            $scope.currentIndex = ($scope.currentIndex < $scope.CourseList.freeContent.fileAttachment.length - 1) ? ++$scope.currentIndex : 0;

        }
        else if (index == 2) {
            $scope.currentIndex = ($scope.currentIndex < $scope.CourseList.publicContent.fileAttachment.length - 1) ? ++$scope.currentIndex : 0;

        }
        else if (index == 3) {
            $scope.currentIndex = ($scope.currentIndex < $scope.CourseList.paidContent.fileAttachment.length - 1) ? ++$scope.currentIndex : 0;

        }

        $scope.direction = 'left';
    };

    $scope.nextSlide = function (index) {

        if (index == 1) {
            $scope.currentIndex = ($scope.currentIndex > 0) ? --$scope.currentIndex : $scope.CourseList.freeContent.fileAttachment.length - 1;
        }
        else if (index == 2) {
            $scope.currentIndex = ($scope.currentIndex > 0) ? --$scope.currentIndex : $scope.CourseList.publicContent.fileAttachment.length - 1;
        }
        else if (index == 3) {
            $scope.currentIndex = ($scope.currentIndex > 0) ? --$scope.currentIndex : $scope.CourseList.paidContent.fileAttachment.length - 1;
        }

        $scope.direction = 'right';
    };

    $scope.AddToCart = function (course) {
        if (localStorage && localStorage.getItem('authorizationData')) {
            var authorizationDataString = localStorage.getItem('authorizationData');
            authorizationData = JSON.parse(authorizationDataString);
            if (authorizationData.role.toLowerCase() == "customer") {

                var dataToSave = {
                    type: "course",
                    quantity: 1,
                    ItemId: course.courseID,
                    UserName: authorizationData.userName
                };

                EnterpriseService.SaveCart(dataToSave)
               .success(function () {
                   $location.url('/Cart');

               })
               .error(function () {
                   alert("some error")
               });
            }
            else if (authorizationData.role.toLowerCase() == "free") {
                $location.url('/Login');
            }
        }
        else {
            $location.url('/Login');
        }

    }

}])

.controller('toolkitController', ['$scope', 'EnterpriseService', '$filter', '$route', '$location', function ($scope, EnterpriseService, $filter, $route, $location) {

    $scope.AddToCart = function (toolkit) {
        if (localStorage && localStorage.getItem('authorizationData')) {
            var authorizationDataString = localStorage.getItem('authorizationData');
            authorizationData = JSON.parse(authorizationDataString);
            if (authorizationData.role.toLowerCase() == "customer") {

                var dataToSave = {
                    type: "toolkit",
                    quantity: 1,
                    ItemId: toolkit.toolkitID,
                    UserName: authorizationData.userName
                };

                EnterpriseService.SaveCart(dataToSave)
               .success(function () {
                   $location.url('/Cart');

               })
               .error(function () {
                   alert("some error")
               });
            }
            else if (authorizationData.role.toLowerCase() == "free") {
                $location.url('/Login');
            }
        }
        else {
            $location.url('/Login');
        }
    }


    $("#home").removeAttr("style");
    $scope.CurrencyType = [
        { Key: '1', Value: 'INR' },
        //{ Key: '2', Value: 'Dollar' },
        //{ Key: '3', Value: 'Euro' }
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
        //startDate: '',
        //endDate: '',
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

    $scope.isAdmin = $scope.isAdmin();

    $scope.Toolkits = [];

    $scope.showAddCourse = true;

    $scope.isPaidPage = false;
    if ($route && $route.current && $route.current.$$route && $route.current.$$route.originalPath && $route.current.$$route.originalPath.toLowerCase().indexOf('purchasedtoolkit') > 0) {
        $scope.isPaidPage = true;
    }

    $scope.documentScope = $scope.isAdmin ? $scope.DocumentScope.AllContent : ($scope.isPaidPage ? $scope.DocumentScope.PaidContent : ($scope.$parent.isLoggedIn ? $scope.DocumentScope.FreeContent : $scope.DocumentScope.PublicContent));

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
        else if ($scope.isPaidPage) {
            EnterpriseService.GetAllPaidToolkit().success(function (data) {
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
                //$scope.ToolkitList.startDate = getFormattedDate(new Date($scope.ToolkitList.startDate));
                //$scope.ToolkitList.endDate = getFormattedDate(new Date($scope.ToolkitList.endDate));
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


    $scope.direction = 'left';
    $scope.currentIndex = 0;

    $scope.setCurrentSlideIndex = function (index) {
        $scope.direction = (index > $scope.currentIndex) ? 'left' : 'right';
        $scope.currentIndex = index;
    };

    $scope.isCurrentSlideIndex = function (index) {
        return $scope.currentIndex === index;
    };

    $scope.prevSlide = function (index) {
        if (index == 1) {
            $scope.currentIndex = ($scope.currentIndex < $scope.ToolkitList.freeContent.fileAttachment.length - 1) ? ++$scope.currentIndex : 0;

        }
        else if (index == 2) {
            $scope.currentIndex = ($scope.currentIndex < $scope.ToolkitList.publicContent.fileAttachment.length - 1) ? ++$scope.currentIndex : 0;

        }
        else if (index == 3) {
            $scope.currentIndex = ($scope.currentIndex < $scope.ToolkitList.paidContent.fileAttachment.length - 1) ? ++$scope.currentIndex : 0;

        }

        $scope.direction = 'left';
    };

    $scope.nextSlide = function (index) {

        if (index == 1) {
            $scope.currentIndex = ($scope.currentIndex > 0) ? --$scope.currentIndex : $scope.ToolkitList.freeContent.fileAttachment.length - 1;
        }
        else if (index == 2) {
            $scope.currentIndex = ($scope.currentIndex > 0) ? --$scope.currentIndex : $scope.ToolkitList.publicContent.fileAttachment.length - 1;
        }
        else if (index == 3) {
            $scope.currentIndex = ($scope.currentIndex > 0) ? --$scope.currentIndex : $scope.ToolkitList.paidContent.fileAttachment.length - 1;
        }

        $scope.direction = 'right';
    };


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
}])

.controller('cartController', ['$scope', 'EnterpriseService', '$filter', '$route', '$rootScope',
    function ($scope, EnterpriseService, $filter, $route, $rootScope) {
        $("#home").removeAttr("style");
        $scope.myContent = [];
        $scope.total = 0;
        $scope.showContent = function () {

            if ($scope.myContent.length > 0) {
                return true;
            } else {
                return false;
            }

        }

        $scope.update = function (id, quantity, price) {

            var dataToSave = {
                Id : id,
                type: "toolkit",
                quantity: quantity,
                ItemId: id,
                UserName: authorizationData.userName
            };

            EnterpriseService.SaveCart(dataToSave)
              .success(function () {
                  $scope.init();
              })
              .error(function () {
                  alert("some error")
              });
           
        }

        $scope.payment = function () {
            $scope.myContent = [];
            $scope.total = 0;
            //redirect to payment
        }

        $scope.remove = function (id) {

            EnterpriseService.DeleteSpecificCart(id).success(function (data) {
                $scope.init();

            }).error(function () {
            });


        }

        $scope.recount = function () {
            $scope.total = 0;
            for (var index in $scope.myContent) {
                $scope.total = $scope.total + ($scope.myContent[index].price * $scope.myContent[index].quantity);
            }
        }

        $scope.init = function () {
            EnterpriseService.GetUserCart($rootScope.userName).success(function (data) {
                if (data && data.cart) {
                    $scope.myContent = data.cart;
                    $scope.recount();
                }

            }).error(function () {
            });

            //$scope.myContent = [{"id" : 1, "type": "toolkit", "name": "wow 1", "quantity": "1", "price": "123" },
            //{ "id": 2, "type": "toolkit", "name": "wow 1", "quantity": "1", "price": "123" },
            //{ "id": 3, "type": "toolkit", "name": "wow 1", "quantity": "1", "price": "123" },
            //{ "id": 4, "type": "toolkit", "name": "wow 1", "quantity": "1", "price": "123" },
            //{ "id": 5, "type": "course", "name": "wow 1", "quantity": "1", "price": "123" },
            //{ "id": 6, "type": "course", "name": "wow 1", "quantity": "1", "price": "123" },
            //];

        }

        $scope.init();
    }])

.animation('.slide-animation', function () {
    return {
        beforeAddClass: function (element, className, done) {
            var scope = element.scope();

            if (className == 'ng-hide') {
                var finishPoint = element.parent().width();
                if (scope.direction !== 'right') {
                    finishPoint = -finishPoint;
                }
                TweenMax.to(element, 0.5, { left: finishPoint, onComplete: done });
            }
            else {
                done();
            }
        },
        removeClass: function (element, className, done) {
            var scope = element.scope();

            if (className == 'ng-hide') {
                element.removeClass('ng-hide');

                var startPoint = element.parent().width();
                if (scope.direction === 'right') {
                    startPoint = -startPoint;
                }

                TweenMax.fromTo(element, 0.5, { left: startPoint }, { left: 0, onComplete: done });
            }
            else {
                done();
            }
        }
    };
})

;