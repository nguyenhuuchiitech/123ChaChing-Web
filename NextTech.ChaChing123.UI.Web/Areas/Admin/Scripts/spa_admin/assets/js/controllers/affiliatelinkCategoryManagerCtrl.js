﻿'use strict';
/**
 * controllers for ng-table
 * Simple table with sorting and filtering on AngularJS
 */

app.controller('AffiliateLinkCategoryManagerCtrl', ["$scope", "$uibModal", "$localStorage", "$timeout", "ngTableParams", "affiliatelinkCategoryService", "membershipService", "notificationService",
    function ($scope, $uibModal, $localStorage, $timeout, ngTableParams, affiliatelinkCategoryService, membershipService, notificationService) {
        var sessionKey = $localStorage.currentUserAdmin ? $localStorage.currentUserAdmin.token : "";
        $scope.documents = {};
        $scope.documentID = 0;
        $scope.documentCategoryID = 0;
        $scope.showSpinner = true;

        function loadAffiliateLinkCategoryWithNgTable() {
            $scope.tableParams = new ngTableParams({
                page: 1, // show first page
                count: 10 // count per page
            }, {
                    getData: function ($defer, params) {
                        var entity = {};

                        entity = {
                            "SessionKey": sessionKey
                        };

                        // Load the data from the API
                        $scope.showSpinner = true;
                        affiliatelinkCategoryService.GetAllAffiliateLinks(entity, function (result) {
                            if (result.data && result.data.StatusCode === 17) {
                                membershipService.checkMemberAuthorization();
                            }

                            if (result.data && result.data.StatusCode === 0) {
                                //var data = result.data.Details.Items;
                                $scope.documents = result.data.Details.Items;
                                var totalRecordCount = result.data.Details.Total;

                                // Tell ngTable how many records we have (so it can set up paging)
                                params.total(totalRecordCount);

                                // Return the customers to ngTable
                                $defer.resolve($scope.documents);

                                $timeout(function () {
                                    $scope.showSpinner = false;
                                }, 1000);
                            } else {
                                $timeout(function () {
                                    $scope.showSpinner = false;
                                }, 1000);
                                notificationService.displayError(result.data.StatusMsg);
                            }
                        });
                    }
                });
        }

        function manageAffiliateLinkActions() {
            $scope.addDocument = function (size) {
                var modalInstance = $uibModal.open({
                    templateUrl: 'myModalAddEditAffiliateLinkCategory.html',
                    controller: 'ModalAddEditAffiliateLinkCategoryCtrl',
                    size: size,
                    resolve: {
                        items: function () {
                            return $scope.items;
                        }
                    }
                });
            };

            $scope.editDocument = function (size) {
                var modalInstance = $uibModal.open({
                    templateUrl: 'myModalAddEditAffiliateLinkCategory.html',
                    controller: 'ModalAddEditAffiliateLinkCategoryCtrl',
                    size: size,
                    resolve: {
                        items: function () {
                            $scope.documentID = size.currentTarget.attributes.data.value;
                            return $scope.documentID;
                        }
                    }
                });
            };

            $scope.deleteDocument = function (size) {
                var modalInstance = $uibModal.open({
                    templateUrl: 'myModalDeleteAffiliateLinkCategory.html',
                    controller: 'ModalDeleteAffiliateLinkCategoryCtrl',
                    size: size,
                    resolve: {
                        items: function () {
                            $scope.documentID = size.currentTarget.attributes.data.value;
                            return $scope.documentID;
                        }
                    }
                });
            };
        }

        $scope.AffiliateLinkCategoryManager = {
            init: function () {
                loadAffiliateLinkCategoryWithNgTable();
                manageAffiliateLinkActions();
            }
        };

        $scope.AffiliateLinkCategoryManager.init();
    }]);

app.controller('ModalAddEditAffiliateLinkCategoryCtrl', ["$scope", "$window", "$localStorage", "$timeout", "$uibModalInstance", "items", "affiliatelinkCategoryService", "membershipService", "notificationService",
    function ($scope, $window, $localStorage, $timeout, $uibModalInstance, items, affiliatelinkCategoryService, membershipService, notificationService) {
        var sessionKey = $localStorage.currentUserAdmin ? $localStorage.currentUserAdmin.token : "";
        $scope.entity = {};
        $scope.documentCategories = {};
        $scope.documentCategoryID = 0;
        $scope.isShowLinkDestination = false;
        $scope.isShowImageUpload = false;
        $scope.documentHeading = "Thêm Mới Danh Mục Affiliate Link";
        var documentID = 0;
        var documentTitle = "";
        var documentDescription = "";
        var documentOrder = "";
        var documentType = "";

        var parts = [];
        var categoryRowInfo = items;
        if (items !== undefined) {
            parts = categoryRowInfo.split('|');
            if (parts.length > 0) {
                documentID = parts[0];
                documentTitle = parts[1];
                documentDescription = parts[2];
                documentOrder = parts[3];
            }
        }

        if (documentID === 0)
            $scope.documentHeading = "Thêm Mới Danh Mục Affiliate Link";
        else
            $scope.documentHeading = "Cập Nhật Danh Mục Affiliate Link";

        $scope.ok = function () {

        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        function addEditAffiliateLinkForm() {
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
                        if (documentID > 0) {
                            $scope.entity = {
                                "ID": documentID,
                                "Title": $scope.entity.Title,
                                "Content": $scope.entity.Content,
                                "Order": $scope.entity.Order,
                                "Active": "1",
                                "SessionKey": sessionKey
                            };

                            $scope.showSpinner = true;
                            // Load the data from the API
                            affiliatelinkCategoryService.UpdateAffiliateLinksByID($scope.entity, function (result) {
                                if (result.data && result.data.StatusCode === 17) {
                                    membershipService.checkMemberAuthorization();
                                }

                                if (result.data && result.data.StatusCode === 0) {
                                    notificationService.displaySuccess(result.data.StatusMsg);
                                    $timeout(function () {
                                        $scope.showSpinner = false;
                                        $uibModalInstance.dismiss('cancel');
                                        $window.location.reload();
                                    }, 1000);
                                } else {
                                    notificationService.displayError(result.data.StatusMsg);
                                    $timeout(function () {
                                        $scope.showSpinner = false;
                                        $uibModalInstance.dismiss('cancel');
                                    }, 1000);
                                }
                            });
                        } else {
                            $scope.entity = {
                                "Title": $scope.entity.Title,
                                "Content": $scope.entity.Content,
                                "Order": $scope.entity.Order,
                                "Active": "1",
                                "SessionKey": sessionKey
                            };

                            $scope.showSpinner = true;
                            // Load the data from the API
                            affiliatelinkCategoryService.AddAffiliateLinks($scope.entity, function (result) {
                                if (result.data && result.data.StatusCode === 17) {
                                    membershipService.checkMemberAuthorization();
                                }

                                if (result.data && result.data.StatusCode === 0) {
                                    notificationService.displaySuccess(result.data.StatusMsg);
                                    $timeout(function () {
                                        $scope.showSpinner = false;
                                        $uibModalInstance.dismiss('cancel');
                                        $window.location.reload();
                                    }, 1000);
                                } else {
                                    notificationService.displayError(result.data.StatusMsg);
                                    $timeout(function () {
                                        $scope.showSpinner = false;
                                        $uibModalInstance.dismiss('cancel');
                                    }, 1000);
                                }
                            });
                        }
                    }
                }
            };
        }

        function loadAffiliateLinkDetails() {
            if (documentID > 0) {
                $scope.entity = {
                    ID: documentID,
                    Title: documentTitle,
                    Content: documentDescription,
                    Order: documentOrder,
                    Active: documentType
                };
            }
        }

        $scope.ModalAddEditAffiliateLinkCategoryManager = {
            addEdit: function () {
                loadAffiliateLinkDetails();
            },
            handleAddAndEditDocument: function () {
                addEditAffiliateLinkForm();
            }
        };

        $scope.ModalAddEditAffiliateLinkCategoryManager.addEdit();
        $scope.ModalAddEditAffiliateLinkCategoryManager.handleAddAndEditDocument();
    }]);

app.controller('ModalDeleteAffiliateLinkCategoryCtrl', ["$scope", "$window", "$localStorage", "$timeout", "$uibModalInstance", "items", "affiliatelinkCategoryService", "membershipService", "notificationService",
    function ($scope, $window, $localStorage, $timeout, $uibModalInstance, items, affiliatelinkCategoryService, membershipService, notificationService) {
        var sessionKey = $localStorage.currentUserAdmin ? $localStorage.currentUserAdmin.token : "";
        $scope.orderID = 0;

        $scope.ok = function () {
            var ID = items;

            var title = {
                "ID": ID,
                "SessionKey": sessionKey
            };

            $scope.showSpinner = true;
            affiliatelinkCategoryService.DeleteAffiliateLinksByID(title, function (result) {
                if (result.data && result.data.StatusCode === 17) {
                    membershipService.checkMemberAuthorization();
                }

                if (result.data && result.data.StatusCode === 0) {
                    notificationService.displaySuccess('Xóa Danh Mục Affiliate Link Thành Công');

                    $timeout(function () {
                        $scope.showSpinner = false;
                        $uibModalInstance.dismiss('cancel');
                        $window.location.reload();
                    }, 1000);
                }
                else {
                    notificationService.displayError(result.data.StatusMsg);
                    $timeout(function () {
                        $scope.showSpinner = false;
                        $uibModalInstance.dismiss('cancel');
                    }, 1000);
                }
            });
        };
        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    }]);