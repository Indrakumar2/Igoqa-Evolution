import { techSpecActionTypes } from '../../constants/actionTypes';

export const ContactInformationReducer = (state, action) => {
    const { type, data } = action; 
    switch (type) {
        case techSpecActionTypes.contactInformationActionTypes.UPDATE_CONTACT_INFORMATION:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistInfo: data
                },
                isbtnDisable: false
            };
            return state;
            case techSpecActionTypes.contactInformationActionTypes.UPDATE_CONTACT:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistContact: data
                },
                isbtnDisable: false
            };
            return state;
            case techSpecActionTypes.contactInformationActionTypes.AUTOGENERATE_USER_NAME:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                   
                    TechnicalSpecialistInfo: data
                },
                isbtnDisable: false
            };
            return state;

            case techSpecActionTypes.contactInformationActionTypes.IS_RCRM_UPDATED_CONTACT:
                state = {
                    ...state,
                    RCRMUpdatedTabs: {  
                        ...state.RCRMUpdatedTabs,
                        IsRCRMUpdatedContactInfo: data
                    }, 
                };
        default:
            return state;
    }
};
