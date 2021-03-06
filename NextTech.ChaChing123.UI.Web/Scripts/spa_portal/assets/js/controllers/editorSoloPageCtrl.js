﻿'use strict';
/** 
  * controller for User Profile Example
*/

var baseUrl = 'https://api.123chaching.app';
app.controller('editorSoloPageCtrl', ["$scope", "$rootScope", "$sce", "$window", "$location", "$localStorage", "$timeout", "membershipService", "editorService", "soloPageService", "notificationService",
    function ($scope, $rootScope, $sce, $window, $location, $localStorage, $timeout, membershipService, editorService, soloPageService, notificationService) {
        var soloID = 0;
        var funnelID = 0;
        var UserName = "";
        var SessionKey = "";
        var destinationURL = "";
        var formType = 0;
        $scope.isShowVideoSource = false;
        $scope.isShowImageSource = false;

        var _leadName = "";
        var _leadEmail = "";
        var _leadPhone = "";
        var _leadBody = "";
        var _soloPageName = "";
        var _rebuildSoloPageURL = "";
        var _actualSoloPageURL = "";

        $scope.pushToken = "";
        $scope.soloPageName = "";

        $scope.lead = {};
        var soloPageObj = {};

        var isAdditionalParaForSoloPage = $location.url().indexOf('?') !== -1;

        //1.1 check if solo page contains additional parametter on URL i.e when clicking solo page from Zalo, the url is generated more para compared with the original one
        if (isAdditionalParaForSoloPage) {
            var analyzeURL = $location.url().split('?');
            if (analyzeURL.length > 0) {
                _actualSoloPageURL = analyzeURL[0];
                if (_actualSoloPageURL.length > 0) {
                    _rebuildSoloPageURL = $rootScope.baseUrl.url + '#' + _actualSoloPageURL;
                    $window.location.href = _rebuildSoloPageURL;
                    $window.location.reload();
                }
            }
        } else { // 2.1 Handle solo page with normal and given para on URL
            var locationURL = $location.url().split('/');
            if (locationURL[3]) {
                soloID = locationURL[3];
            }

            if (locationURL[4] && locationURL[4].length > 0) {
                UserName = locationURL[4];
            }

            if (locationURL[5] && locationURL[5].length > 0) {
                SessionKey = locationURL[5];
            }

            $scope.soloPageManager = {
                init: function () {
                    $scope.form = {
                        submit: function (form) {
                            var firstError = null;
                            if (form.$invalid) {

                                var field = null, firstError = null;
                                for (field in form) {
                                    if (field[0] != '$') {
                                        if (firstError === null && !form[field].$valid) {
                                            firstError = form[field].$name;
                                        }

                                        if (form[field].$pristine) {
                                            form[field].$dirty = true;
                                        }
                                    }
                                }

                                angular.element('.ng-invalid[name=' + firstError + ']').focus();
                                //SweetAlert.swal("The form cannot be submitted because it contains validation errors!", "Errors are marked with a red, dashed border!", "error");

                                return;

                            } else {
                                //SweetAlert.swal("Good job!", "Your form is ready to be submitted!", "success");
                                //your code for submit
                                //notificationService.displayInfo('You are submitting the form');

                                var bodyBuilder = "";
                                _leadName = ($scope.lead.name) ? $scope.lead.name : "";
                                _leadEmail = ($scope.lead.email) ? $scope.lead.email : "";
                                _leadPhone = ($scope.lead.phone) ? $scope.lead.phone : "";
                                _soloPageName = $scope.soloPageName;

                                if (_leadName.length > 0)
                                    bodyBuilder += _leadName + ', ';

                                if (_leadEmail.length > 0)
                                    bodyBuilder += _leadEmail + ', ';

                                if (_leadPhone.length > 0)
                                    bodyBuilder += _leadPhone;

                                var indexCommaChar = bodyBuilder.indexOf(', ');
                                var isHasCommaChar = indexCommaChar !== -1;
                                if (isHasCommaChar)
                                    _leadBody = bodyBuilder.substring(0, indexCommaChar) + ' da dang ky Solo Page';

                                if (_soloPageName.length > 0)
                                    _leadBody = _leadBody + ' ' + _soloPageName;

                                var leadRes = {
                                    "Name": ($scope.lead.name) ? $scope.lead.name : "",
                                    "Email": ($scope.lead.email) ? $scope.lead.email : "",
                                    "Phone": ($scope.lead.phone) ? $scope.lead.phone : "",
                                    "SoloID": soloID,
                                    "FunnelID": funnelID
                                };

                                var indexOfHttps = -1;
                                destinationURL = angular.element('#refLink').val();
                                formType = angular.element('#formTypeID').val();

                                if (formType != undefined && formType == 4 && destinationURL.length > 0) {
                                    indexOfHttps = destinationURL.indexOf('http') || destinationURL.indexOf('https');
                                    if (indexOfHttps === -1) {
                                        destinationURL = 'http://' + destinationURL;
                                    }
                                    $window.open(destinationURL, '_blank');
                                } else {
                                    $scope.showSpinner = true;
                                    // Load the data from the API
                                    editorService.RegisterLeadBySoloPage(leadRes, function (result) {
                                        if (result.data && result.data.StatusCode === 17) {
                                            membershipService.checkMemberAuthorization();
                                        }

                                        if (result.data && result.data.StatusCode === 0) {
                                            notificationService.displaySuccess('Đăng ký thành công. Chúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất');

                                            // push notification if lead has registrated successfully on SOLO page
                                            var pushToken = $scope.pushToken;
                                            var title = "*** Chuc mung ban! ***";
                                            var sound = "default";
                                            var body = _leadBody;

                                            // Load the data from the API
                                            if (pushToken.length > 0) {
                                                const PUSH_ENDPOINT = 'https://exp.host/--/api/v2/push/send';
                                                let data = {
                                                    "to": pushToken,
                                                    "title": title,
                                                    "body": body,
                                                    "sound": sound,
                                                    "priority": 'high'
                                                };

                                                fetch(PUSH_ENDPOINT, {
                                                    'mode': 'no-cors',
                                                    'method': 'POST',
                                                    'headers': {
                                                        'Accept': 'application/json',
                                                        'Content-Type': 'application/json'
                                                    },
                                                    body: JSON.stringify(data)
                                                }).catch(err => console.log(err));
                                            }

                                            $timeout(function () {
                                                $scope.showSpinner = false;
                                                if (destinationURL && destinationURL.length > 0) {
                                                    indexOfHttps = destinationURL.indexOf('http') || destinationURL.indexOf('https');
                                                    if (indexOfHttps === -1) {
                                                        destinationURL = 'http://' + destinationURL;
                                                    }

                                                    $window.open(destinationURL, '_blank');
                                                }
                                            }, 1000);
                                        } else {
                                            notificationService.displayError(result.data.StatusMsg);
                                            $timeout(function () {
                                                $scope.showSpinner = false;
                                            }, 2000);
                                        }
                                    });
                                }
                            }

                        }
                    };
                },
                previewSoloPage: function () {
                    $scope.formType = 4;
                    $scope.soloPageDetails = {};

                    soloPageObj = {
                        "ID": soloID,
                        "UserName": UserName,
                        "SessionKey": SessionKey
                    };

                    soloPageService.loadSoloPage(soloPageObj, function (result) {
                        if (result.data && result.data.StatusCode === 17) {
                            membershipService.checkMemberAuthorization();
                        }

                        if (!result.data.Details) {
                            $location.path('/error/404');
                            return;
                        }

                        if (result.data && result.data.StatusCode == 0) {
                            var findWatchIndex = -1;
                            var fullResourcePath = "";

                            $scope.soloPageDetails = result.data.Details;
                            $scope.formType = result.data.Details.FromType;
                            $scope.Title = result.data.Details.Title;
                            $scope.SubTitle = result.data.Details.SubTitle;
                            $scope.ButtonName = result.data.Details.ButtonName;
                            $scope.ButtonColor = result.data.Details.ButtonColor;

                            fullResourcePath = result.data.Details.ResourcePath;
                            if (fullResourcePath.length > 0) {
                                var linkImage = $scope.soloPageManager.checkURL(fullResourcePath);
                                if (linkImage) { // Display Image
                                    $scope.isShowImageSource = true;
                                    $scope.ImageSource = fullResourcePath;
                                } else { // Diplay Video
                                    $scope.isShowVideoSource = true;
                                    findWatchIndex = fullResourcePath.indexOf('watch?v=');
                                    if (findWatchIndex != -1) {
                                        fullResourcePath = fullResourcePath.replace('watch?v=', 'embed/');
                                    }

                                    $scope.VideoSource = $sce.trustAsResourceUrl(fullResourcePath);
                                }
                            }

                            document.body.style.backgroundImage = "url('" + result.data.Details.BackgroundPath + "')";
                        }
                        else {
                            notificationService.displayError(result.data.StatusMsg);
                            $scope.isShowVideoSource = false;
                        }
                    });
                },
                publicSoloPage: function () {
                    soloPageObj = {
                        "ID": soloID
                    };

                    editorService.GetDetailSoloPageByID(soloPageObj, function (result) {
                        if (result.data && result.data.StatusCode === 17) {
                            membershipService.checkMemberAuthorization();
                        }

                        if (!result.data.Details) {
                            $location.path('/error/404');
                            return;
                        }

                        if (result.data && result.data.StatusCode == 0) {
                            var findWatchIndex = -1;
                            var fullResourcePath = "";

                            $scope.soloPageDetails = result.data.Details;
                            $scope.formType = result.data.Details.FromType;
                            $scope.Title = result.data.Details.Title;
                            $scope.SubTitle = result.data.Details.SubTitle;
                            $scope.ButtonName = result.data.Details.ButtonName;
                            $scope.ButtonColor = result.data.Details.ButtonColor;
                            $scope.pushToken = result.data.Details.PushToken;
                            $scope.soloPageName = $scope.soloPageDetails.PageName;

                            fullResourcePath = result.data.Details.ResourcePath;
                            if (fullResourcePath.length > 0) {
                                var linkImage = $scope.soloPageManager.checkURL(fullResourcePath);
                                if (linkImage) { // Display Image
                                    $scope.isShowImageSource = true;
                                    $scope.ImageSource = fullResourcePath;
                                } else { // Diplay Video
                                    $scope.isShowVideoSource = true;
                                    findWatchIndex = fullResourcePath.indexOf('watch?v=');
                                    if (findWatchIndex != -1) {
                                        fullResourcePath = fullResourcePath.replace('watch?v=', 'embed/');
                                    }

                                    $scope.VideoSource = $sce.trustAsResourceUrl(fullResourcePath);
                                }
                            }

                            document.body.style.backgroundImage = "url('" + result.data.Details.BackgroundPath + "')";
                        }
                        else {
                            notificationService.displayError(result.data.StatusMsg);
                            $scope.isShowVideoSource = false;
                        }
                    });
                },
                checkURL: function (url) {
                    return (url.match(/\.(jpeg|jpg|gif|png)$/) != null);
                }
            };

            $scope.soloPageManager.init();

            if (soloID > 0 && UserName.length > 0 && SessionKey.length > 0) {
                $scope.soloPageManager.previewSoloPage();
            } else {
                $scope.soloPageManager.publicSoloPage();
            }
        }
    }]);