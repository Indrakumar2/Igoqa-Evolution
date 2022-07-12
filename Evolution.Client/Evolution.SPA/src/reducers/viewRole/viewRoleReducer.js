import { ViewRoleActionTypes } from '../../constants/actionTypes';
import { CreateRoleActionTypes } from '../../constants/actionTypes';
import { stat } from 'fs';

export const ViewRoleReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case ViewRoleActionTypes.FETCH_VIEWROLE:     
            state = {
                ...state,
                viewRole:data      
            };
            return state;
        case CreateRoleActionTypes.FETCH_MODULE_DATA:
            state = {
                ...state,
                moduleList: data
            };
            return state;
        case CreateRoleActionTypes.FETCH_ACTIVITY:
            state = {
                ...state,
                moduleListData: data
            };
            return state;
        case CreateRoleActionTypes.FETCH_MODULE_ACTIVITY:
            state = {
                ...state,
                moduleActivityData: data
            };
            return state;
        case ViewRoleActionTypes.SELECTED_ROW_DETAILS:
       
            state = {
                ...state,
                selectedRole: data
            };
            return state;
        case CreateRoleActionTypes.FETCH_ROLE_ACTIVITY:
            state = {
                ...state,
                roleActivityData: data
            };
            return state;
        default:
            return state;
    }
};