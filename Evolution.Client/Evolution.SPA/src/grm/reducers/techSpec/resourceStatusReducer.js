import { techSpecActionTypes } from '../../constants/actionTypes';

export const ResourceStatusReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case techSpecActionTypes.resourceStatusActionTypes.ADD_STAMP_DETAILS:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistStamp: data
                },
                isbtnDisable: false
            };
            return state;

        case techSpecActionTypes.resourceStatusActionTypes.DELETE_STAMP_DETAILS:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistStamp: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.resourceStatusActionTypes.UPDATE_STAMP_DETAILS:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistStamp: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.resourceStatusActionTypes.UPDATE_RESOURCE_STATUS:
                state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistInfo: data
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.resourceStatusActionTypes.FEATCH_TECH_SPECH_STAMP_COUNTRY_CODE:
            state = {
                ...state,
                masterTechSpecStampCountryCodes: data,
            }; 
            return state;
        default:
            return state;
    }
};
