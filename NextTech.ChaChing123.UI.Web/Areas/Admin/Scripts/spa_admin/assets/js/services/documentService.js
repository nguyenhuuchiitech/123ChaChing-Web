﻿(function () {
    'use strict';

    angular
        .module('ChaChingApp')
        .factory('documentService', Service);

    function Service(apiService, notificationService) {
        var baseUrl = 'https://api.123chaching.app/api/Admin/';

        var service = {
            //document categories
            GetAllDocuments: GetAllDocuments, // Get all document categories
            AddDocuments: AddDocuments,
            UpdateDocumentsByID: UpdateDocumentsByID,
            GetAllDocumentsByAccount: GetAllDocumentsByAccount,
            DeleteDocumentsByID: DeleteDocumentsByID,

            //document only
            GetAllDocument: GetAllDocument, // Get all related documents for a specific category
            AddDocument: AddDocument,
            UpdateDocumentByID: UpdateDocumentByID,
            GetDocumentInfoByID: GetDocumentInfoByID,
            DeleteDocumentByID: DeleteDocumentByID
        };

        function GetAllDocuments(entity, completed) {
            apiService.post(baseUrl + '/GetAllDocuments/', entity,
                completed,
                entityFailed
            );
        }

        function AddDocuments(entity, completed) {
            apiService.post(baseUrl + '/AddDocuments/', entity,
                completed,
                entityFailed
            );
        }

        function UpdateDocumentsByID(entity, completed) {
            apiService.post(baseUrl + '/UpdateDocumentsByID/', entity,
                completed,
                entityFailed
            );
        }

        function GetAllDocumentsByAccount(entity, completed) {
            apiService.post(baseUrl + '/GetAllDocumentsByAccount/', entity,
                completed,
                entityFailed
            );
        }

        function DeleteDocumentsByID(entity, completed) {
            apiService.post(baseUrl + '/DeleteDocumentsByID/', entity,
                completed,
                entityFailed
            );
        }

        function GetAllDocument(entity, completed) {
            apiService.post(baseUrl + '/GetAllDocument/', entity,
                completed,
                entityFailed
            );
        }

        function AddDocument(entity, completed) {
            apiService.post(baseUrl + '/AddDocument/', entity,
                completed,
                entityFailed
            );
        }

        function UpdateDocumentByID(entity, completed) {
            apiService.post(baseUrl + '/UpdateDocumentByID/', entity,
                completed,
                entityFailed
            );
        }

        function GetDocumentInfoByID(entity, completed) {
            apiService.post(baseUrl + '/GetDocumentInfoByID/', entity,
                completed,
                entityFailed
            );
        }

        function DeleteDocumentByID(entity, completed) {
            apiService.post(baseUrl + '/DeleteDocumentByID/', entity,
                completed,
                entityFailed
            );
        }

        function entityFailed(response) {
            if (response.data) {
                notificationService.displayError(response.data.Message);
            }
        }

        return service;
    }
})();