import { techSpecActionTypes } from '../../constants/actionTypes';

export const PayRateReducer = (state, action) => {
    const { type ,data } = action;
    switch (type) {
        case  techSpecActionTypes.payRateActionTypes.FETCH_PAY:
        state = {
            ...state,
            selectedProfileDetails:{
                ...state.selectedProfileDetails,
                TechnicalSpecialistInfo: data,
            },
           
            isbtnDisable: false
        };
     
        return state;        
            case techSpecActionTypes.payRateActionTypes.ADD_PAYRATE_SCHEDULE:
            if (state.selectedProfileDetails.TechnicalSpecialistPaySchedule == null) {
                state = {
                    ...state,
                    selectedProfileDetails: {
                        ...state.selectedProfileDetails,
                        TechnicalSpecialistPaySchedule: []
                    },
                    isbtnDisable: false
                };
            }
            
            state = {
                ...state,
                selectedProfileDetails:{
                    ...state.selectedProfileDetails,
                    
                    TechnicalSpecialistPaySchedule:[ ...state.selectedProfileDetails.TechnicalSpecialistPaySchedule,data ]
                },
                isbtnDisable: false
            };
        
            return state;
            case techSpecActionTypes.payRateActionTypes.UPDATE_PAYRATE_SCHEDULE:
        
            state = {
                ...state,
                selectedProfileDetails:{
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistPaySchedule:data
                },
                isbtnDisable: false
            };
         
              return state;
            case techSpecActionTypes.payRateActionTypes.EDIT_PAYRATE_SCHEDULE:
            state = {
                ...state,
                rateScheduleEditData:data,
                isbtnDisable: false
            };
            return state;
            case techSpecActionTypes.payRateActionTypes.DELETE_PAYRATE_SCHEDULE:
           
            state = {
                ...state,
                selectedProfileDetails:{
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistPaySchedule: data,
                },
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.payRateActionTypes.FETCH_PAYRATE_DETAIL:
            state = {
                ...state,
                TechnicalSpecialistPayRate: data,
            };
            return state;
        case techSpecActionTypes.payRateActionTypes.ADD_DETAIL_PAYRATE:
        if (state.selectedProfileDetails.TechnicalSpecialistPayRate == null) {
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistPayRate: []
                },
                isbtnDisable: false
            };
        }
        
            state = {
                ...state,
                selectedProfileDetails:{
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistPayRate:[ ...state.selectedProfileDetails.TechnicalSpecialistPayRate,data ]
                },
                isbtnDisable: false
            };
            return state;

            case techSpecActionTypes.payRateActionTypes.UPDATE_DETAIL_PAYRATE:
            
            state = {
                ...state,
                selectedProfileDetails:{
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistPayRate:data
                },
                isbtnDisable: false
            };
            return state;    
            case   techSpecActionTypes.payRateActionTypes.DELETE_DETAIL_PAYRATE:
          
            state = {
                ...state,
                selectedProfileDetails:{
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistPayRate: data,
                },
                isbtnDisable: false
            };
          
            return state;
            case   techSpecActionTypes.payRateActionTypes.CLEAR_PAYRATE_EDITDETAILS:
            state = {
                ...state,
                rateScheduleEditData:  {},
            };
            return state;
            case techSpecActionTypes.payRateActionTypes.UPDATE_PAYRATE:
            state = {
                ...state,
                selectedProfileDetails: {
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistInfo: data
                },
                isbtnDisable: false
            };
            return state;
            case  techSpecActionTypes.payRateActionTypes.PAYROLLNAMEBYCOMPANY:
            state = {
                ...state,
                payRollNameByCompany: data
            };
            return state;
        default:
            return state;
    }
};