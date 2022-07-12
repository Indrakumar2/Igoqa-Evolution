import { techSpecActionTypes } from '../../constants/actionTypes';

export const TimeOffRequest = (state, action) => {
    const { type, data } = action;
    switch (type) {       
        case techSpecActionTypes.timeOffRequestActionType.FETCH_TIME_OFF_REQUEST:
            state = {
                ...state,
                resourceNameList:data
            };
            return state;
        case techSpecActionTypes.timeOffRequestActionType.FETCH_CATEGORY:
            state ={
                 ...state,
                timeOffRequestCategory:data
            };
            return state;
        case techSpecActionTypes.timeOffRequestActionType.SAVE_RESOURCE_NAME:
            state ={
                 ...state,
                resourceDetails:data
            };
            return state;
        default:
            return state;
    }
};