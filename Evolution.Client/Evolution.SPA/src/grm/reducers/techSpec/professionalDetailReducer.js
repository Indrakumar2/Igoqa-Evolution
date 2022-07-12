import { techSpecActionTypes } from '../../constants/actionTypes';

export const ProfessionalDetailReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {

        case techSpecActionTypes.professionalDetailsActionTypes.ADD_WORK_HISTORY_DETAILS:
            if (state.selectedProfileDetails.TechnicalSpecialistWorkHistory == null) {
                state = {
                    ...state,
                    selectedProfileDetails: {
                        ...state.selectedProfileDetails,
                        TechnicalSpecialistWorkHistory: []
                    },
                    isbtnDisable: false
                };
            }

            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistWorkHistory: [
                        ...state.selectedProfileDetails.TechnicalSpecialistWorkHistory, data
                    ]
                },
                isbtnDisable: false
            };
            return state;

        case techSpecActionTypes.professionalDetailsActionTypes.UPDATE_WORK_HISTORY_DETAILS:

            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistWorkHistory: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.professionalDetailsActionTypes.DELETE_WORK_HISTORY_DETAILS:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistWorkHistory: data,
                },
                isbtnDisable: false
            };
            return state;

        case techSpecActionTypes.professionalDetailsActionTypes.ADD_EDUCATIONAL_DETAILS:
            if (state.selectedProfileDetails.TechnicalSpecialistEducation == null) {
                state = {
                    ...state,
                    selectedProfileDetails: {
                        ...state.selectedProfileDetails,
                        TechnicalSpecialistEducation: []
                    },
                    isbtnDisable: false
                };
            }

            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistEducation: [
                        ...state.selectedProfileDetails.TechnicalSpecialistEducation, data
                    ]
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.professionalDetailsActionTypes.UPDATE_EDUCATIONAL_DETAILS:

            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistEducation: data
                },
                isbtnDisable: false
            };

            return state;
        case techSpecActionTypes.professionalDetailsActionTypes.DELETE_EDUCATIONAL_DETAILS:

            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistEducation: data,
                },
                isbtnDisable: false
            };

            return state;
        case techSpecActionTypes.professionalDetailsActionTypes.UPDATE_PROFESSIONAL_SUMMARY:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistInfo: data
                },
                isbtnDisable: data.btnDisable 
            };
            return state;

        case techSpecActionTypes.professionalDetailsActionTypes.ADD_PROFESSIONAL_EDUCATION_DOCUMENTS:
            if (state.selectedProfileDetails.TechnicalSpecialistInfo === null) {
                state = {
                    ...state,
                    selectedProfileDetails: {
                        ...state.selectedProfileDetails,
                        TechnicalSpecialistInfo: {
                            ...state.selectedProfileDetails.TechnicalSpecialistInfo,
                            professionalAfiliationDocuments: []
                        }
                    },
                    isbtnDisable: false
                };
            }
            else if (state.selectedProfileDetails.TechnicalSpecialistInfo.professionalAfiliationDocuments === undefined) {
                state = {
                    ...state,
                    selectedProfileDetails: {
                        ...state.selectedProfileDetails,
                        TechnicalSpecialistInfo: {
                            ...state.selectedProfileDetails.TechnicalSpecialistInfo,
                            professionalAfiliationDocuments: []
                        }
                    },
                    isbtnDisable: false
                };
            }

            const newState = Object.assign([], state.selectedProfileDetails.TechnicalSpecialistInfo.professionalAfiliationDocuments);
            data.map(document => {
                return newState.push(document);
            });

            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistInfo: {
                        ...state.selectedProfileDetails.TechnicalSpecialistInfo,
                        professionalAfiliationDocuments: newState
                    }
                },
                isbtnDisable: false
            };
            return state;

        case techSpecActionTypes.professionalDetailsActionTypes.REMOVE_PROFESSIONAL_EDUCATION_DOCUMENT:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistInfo: {
                        ...state.selectedProfileDetails.TechnicalSpecialistInfo,
                        professionalAfiliationDocuments: data
                    }
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.professionalDetailsActionTypes.ADD_PROFILE_ACTION_TYPE:
            state = {
                ...state,
                oldProfileActionType: data
            };
            return state;

        case techSpecActionTypes.professionalDetailsActionTypes.IS_RCRM_UPDATED_PROFESSIONAL_EDU_DETAILS:
            state = {
                ...state,
                RCRMUpdatedTabs: {  
                    ...state.RCRMUpdatedTabs,
                    IsRCRMUpdatedProfessionalEduDetails: data
                }, 
            };

        default:
            return state;
    }
};
