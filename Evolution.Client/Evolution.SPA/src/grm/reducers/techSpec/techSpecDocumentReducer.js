import { techSpecActionTypes } from '../../constants/actionTypes';

export const TechSpecDocumentsReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case techSpecActionTypes.techSpecDocumentsActionTypes.ADD_TECHSPEC_DOC_DETAILS:
            if (state.selectedProfileDetails.TechnicalSpecialistDocuments == null) {
                state = {
                    ...state,
                    selectedProfileDetails: {
                        ...state.selectedProfileDetails,
                        TechnicalSpecialistDocuments: []
                    },
                    isbtnDisable: false
                };
            }
            
            const newState = data.concat(state.selectedProfileDetails.TechnicalSpecialistDocuments);

            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistDocuments: newState
                },
                isbtnDisable: false
            };
            return state;

        case techSpecActionTypes.techSpecDocumentsActionTypes.DELETE_TECHSPEC_DOC_DETAILS:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistDocuments: data
                },
                isbtnDisable: false
            };
            return state;
            
        case techSpecActionTypes.techSpecDocumentsActionTypes.UPDATE_TECHSPEC_DOC_DOCUMENTS:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistDocuments: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.techSpecDocumentsActionTypes.IS_RCRM_UPDATED_DOCUMENT:
            state = {
                ...state,
                RCRMUpdatedTabs: {  
                    ...state.RCRMUpdatedTabs,
                    IsRCRMUpdatedDocumentInfo: data
                }, 
            };
        case techSpecActionTypes.techSpecDocumentsActionTypes.ADD_FILES_TO_BE_UPLOADED: //Add files to be uploaded async            
        state = {
            ...state,
            selectedProfileDetails: {
                ...state.selectedProfileDetails,
                fileToBeUploaded: data
            },
            isbtnDisable: false
        };
            return state;

        case techSpecActionTypes.techSpecDocumentsActionTypes.CLEAR_FILES_TO_BE_UPLOADED: //clear files to be uploaded async            
        state = {
            ...state,
            selectedProfileDetails: {
                ...state.selectedProfileDetails,
                fileToBeUploaded: []
            },
            isbtnDisable: false
        };
            return state;  
        default:
            return state;
    }
};