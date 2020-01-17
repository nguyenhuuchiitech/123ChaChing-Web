﻿'use strict';
/** 
  * controller for Validation Form example
*/

(function () {
    'use strict';

    angular
        .module('ChaChingApp')
        .controller('UserRegistrationCtrl', ['$scope', '$location', '$localStorage', 'membershipService', 'notificationService', function ($scope, $location, $localStorage, membershipService, notificationService) {
            $scope.master = $scope.user;
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
                        $scope.isLoading = true;

                        var userRegistration = {};

                        userRegistration = {
                            "fullname": $scope.userReg.fullname,
                            "username": $scope.userReg.username,
                            "email": $scope.userReg.email,
                            "phone": $scope.userReg.phone,
                            "password": $scope.userReg.password,
                            "accounttype": $scope.userReg.accounttype,
                            "refcode": $scope.userReg.refcode
                        };


                        membershipService.register(userRegistration, function (result) {
                            if (result.data && result.data.StatusCode == 0) {
                                $scope.isLoading = false;
                                notificationService.displaySuccess('Đăng ký thành công');
                                $location.path('/app/home');
                            }
                            else {
                                $scope.isLoading = false;
                                notificationService.displayError(result.data.StatusMsg);
                            }
                        });
                    }

                }
            };

            $scope.initForm = {
                init: function () {
                    var refObj = $localStorage.refcodeVal;
                    var currentPackage = $localStorage.currentPackage;
                    //Initial the account type for user
                    var accountType = 1;
                    //1. Handle register page when user signup via reference code URL
                    
                    if (refObj) {
                        var userRegistration = {};

                        userRegistration = {
                            "fullname": "",
                            "username": "",
                            "email": "",
                            "phone": "",
                            "password": "",
                            "accounttype": "",
                            "refcode": refObj.refcode //$scope.userReg.refcode <- before
                        };

                        $scope.userReg = userRegistration;
						
						//2.1 Trigger Registration Tab
						var activeTab = 0;
						activeTab = $('.nav-tabs > .active').length;
						if(activeTab > 0) { 
							console.log(activeTab); 
							//$('.nav-tabs > .active').next('li').find('a').trigger('click'); 
						} else {
							console.log("cannot execute"); 
						}
						
                        delete $localStorage.refcodeVal;
                    }
                }
            }

            $scope.initForm.init();
        }]);
})();
