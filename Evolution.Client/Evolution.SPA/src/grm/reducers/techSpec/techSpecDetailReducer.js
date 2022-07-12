import { isUndefined } from '../../../utils/commonUtils';
import { techSpecActionTypes } from '../../constants/actionTypes';
export const TechSpecReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case techSpecActionTypes.techSpecSearch.FETCH_SELECTED_PROFILE_DETAILS:
            state = {
                ...state,
                selectedProfileDetails: data,
                selectedProfileDraft:data, 
                prevBussinessInformationComment: isUndefined(data.TechnicalSpecialistInfo) ? '' : data.TechnicalSpecialistInfo.businessInformationComment,//def 1306
                TechnicalSpecialistPayScheduleOnCancel: data.TechnicalSpecialistPaySchedule,
                TechnicalSpecialistPayRateOnCancel: data.TechnicalSpecialistPayRate,
                isbtnDisable: true
            };
           
            return state;
        case techSpecActionTypes.techSpecSearch.SAVE_SELECTED_EPIN:
            state = {
                ...state,
                selectedEpinNo: data
            };
            return state;
           
            case techSpecActionTypes.techSpecSearch.UPDATE_SAVE_BTN:
        state={
            ...state,
            isbtnDisable: true
        };
        return state;
        case techSpecActionTypes.techSpecSearch.TS_BTN:
            state={
                ...state,
                isbtnDisable: false
            };
            return state;
        case techSpecActionTypes.commentsActionTypes.CLEAR_TECHSPEC_DETAILS:
        state = {
            ...state,
            selectedProfileDetails: {},
            TechnicalSpecialistCustomerApproval: {},
            oldProfileActionType:'',
            isbtnDisable: true,
            myTaskRefCode:'',
            TechnicalSpecialistPayScheduleOnCancel: [],
            TechnicalSpecialistPayRateOnCancel: [],
        };
        return state;
        case techSpecActionTypes.commentsActionTypes.UPDATE_PROFILE_ACTION:
            state ={
                ...state,
                selectedProfileDetails:{
                    ...state.selectedProfileDetails,
                    TechnicalSpecialistInfo:data
                },
                isbtnDisable: data.isbtnDisable === undefined ? true : data.isbtnDisable
            };
            return state;
        case techSpecActionTypes.GET_TECH_SPEC_DRAFT:
            state = {
                ...state,
                draftDataToCompare:data
            };
            return state;

            case techSpecActionTypes.FETCH_INTER_COMPANY_RESOURCE_USERTYPES:
                state = {
                    ...state,
                    interCompanyResourceUserType:data
                };
                return state;
        default:
            return state;
    }
};