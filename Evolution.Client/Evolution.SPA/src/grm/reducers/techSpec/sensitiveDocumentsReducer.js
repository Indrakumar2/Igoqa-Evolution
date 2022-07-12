import { techSpecActionTypes } from '../../constants/actionTypes';

export const SensitiveDocumentsReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case techSpecActionTypes.sensitiveDocumentsActionTypes.ADD_SENSITIVE_DETAILS:
            if (state.selectedProfileDetails.TechnicalSpecialistSensitiveDocuments == null) {
                state = {
                    ...state,
                    selectedProfileDetails: {
                        ...state.selectedProfileDetails,
                        TechnicalSpecialistSensitiveDocuments: []
                    },
                    isbtnDisable: false
                };
            }
            const newState = Object.assign([], state.selectedProfileDetails.TechnicalSpecialistSensitiveDocuments);
            data.map(document => {
                return newState.push(document);
            });
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistSensitiveDocuments: newState
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.sensitiveDocumentsActionTypes.DELETE_SENSITIVE_DETAILS:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistSensitiveDocuments: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.sensitiveDocumentsActionTypes.UPDATE_SENSITIVE_DOCUMENTS:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistSensitiveDocuments: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.sensitiveDocumentsActionTypes.STORE_DOC_UNIQUE_CODE:
            state = {
                ...state,
                documentUniqueCode: data
            };
            return state;
        case  techSpecActionTypes.sensitiveDocumentsActionTypes.IS_REMOVE_DOCUMENT:
        state = {
            ...state,
            IsRemoveDocument: data
            };
        return state;
        //Added for D223 Popup cancel issue
        case techSpecActionTypes.sensitiveDocumentsActionTypes.CLEAR_SENSITIVE_DOCUMENTS:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistSensitiveDocuments: []
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.sensitiveDocumentsActionTypes.REVERT__DELETE_SENSITIVE_DOCUMENTS:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistSensitiveDocuments: data
                },
                isbtnDisable: false
            };
            return state;
        default:
            return state;
    }
};