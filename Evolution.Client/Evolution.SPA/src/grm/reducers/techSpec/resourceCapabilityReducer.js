import { techSpecActionTypes } from '../../constants/actionTypes';

export const ResourceCapabilityReducer = (state, action) => { 
    const { type, data } = action;
    switch (type) {
       
        case techSpecActionTypes.resourceCapabilityActionTypes.ADD_LANGUAGE_CAPABILITY:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistLanguageCapabilities: data
                },
                isbtnDisable: false
            };

            return state;
        case techSpecActionTypes.resourceCapabilityActionTypes.ADD_CERTIFICATE_DETAILS:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistCertification: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.resourceCapabilityActionTypes.ADD_COMMODITY_DETAILS:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistCommodityAndEquipment: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.resourceCapabilityActionTypes.ADD_TRAINING_DETAILS:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistTraining: data
                },
                isbtnDisable: false
            };
            return state;
            case techSpecActionTypes.resourceCapabilityActionTypes.UPDATE_RESOURCE_CAPABILITY_CODESATNDARD:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistCodeAndStandard: data
                },
                isbtnDisable: false
            };
            return state;
            case techSpecActionTypes.resourceCapabilityActionTypes.UPDATE_RESOURCE_CAPABILITY_COMPUTER_KNOWLEDGE:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistComputerElectronicKnowledge: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.resourceCapabilityActionTypes.UPDATE_LANGUAGE_CAPABILITY:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistLanguageCapabilities: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.resourceCapabilityActionTypes.UPDATE_CERTIFICATE_DETAILS:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistCertification: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.resourceCapabilityActionTypes.UPDATE_COMMODITY_DETAILS:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistCommodityAndEquipment: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.resourceCapabilityActionTypes.UPDATE_TRAINING_DETAILS:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistTraining: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.resourceCapabilityActionTypes.DELETE_LANGUAGE_CAPABILITY:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistLanguageCapabilities: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.resourceCapabilityActionTypes.DELETE_CERTIFICATE_DETAILS:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistCertification: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.resourceCapabilityActionTypes.DELETE_COMMODITY_DETAILS:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistCommodityAndEquipment: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.resourceCapabilityActionTypes.DELETE_TRAINING_DETAILS:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistTraining: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.resourceCapabilityActionTypes.FETCH_INTERTEK_WORK_HISTORY_REPORT_DETAILS:
            state = {
                ...state, 
                    TechnicalSpecialistIntertekWorkHistory: data 
            };
            return state;
        case techSpecActionTypes.resourceCapabilityActionTypes.IS_RCRM_UPDATED_RESOURCE_CAPABILITY:
            state = {
                ...state,
                RCRMUpdatedTabs: { 
                    ...state.RCRMUpdatedTabs, 
                    IsRCRMUpdatedResourceCapabalityInfo: data
                }, 
            };
            return state;
        case techSpecActionTypes.resourceCapabilityActionTypes.IS_RCRM_UPDATED_RESOURCE_CAPABILITY_CODE_STD:
            state = {
                ...state,
                RCRMUpdatedTabs: { 
                    ...state.RCRMUpdatedTabs, 
                    IsRCRMUpdatedResourceCapabalityCodeStandardInfo: data
                }, 
            };
            return state;
        case techSpecActionTypes.resourceCapabilityActionTypes.IS_RCRM_UPDATED_RESOURCE_CAPABILITY_COMP_KNOWLEDGE:
        state = {
            ...state,
            RCRMUpdatedTabs: { 
                ...state.RCRMUpdatedTabs, 
                IsRCRMUpdatedResourceCapabalityCompKnowledInfo: data
            }, 
        };
        return state;  
        default:
            return state;
    }
};
