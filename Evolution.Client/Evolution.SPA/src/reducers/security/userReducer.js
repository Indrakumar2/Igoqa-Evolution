import { userActionTypes } from '../../constants/actionTypes';

export const UserReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case userActionTypes.SET_USER_LANDING_PAGE_DATA:
            state = {
                ...state,
                userLandingPageData: data
            };
            return state;
        case userActionTypes.SET_USER_DETAIL_DATA:
            state = {
                ...state,               
                userDetailData:data
                
            };
            return state;
           
        case userActionTypes.FETCH_COMPANY_OFFICE:
            state = {
                ...state,
                companyOffices: data
            };
            return state;
        case userActionTypes.FETCH_ROLE:
            state = {
                ...state,
                roles: data
            };
            return state;
        case userActionTypes.USER_UNSAVED_DATA:
                state = {
                    ...state,
                    isUserDataChanged: data
                };
                return state;
        default:
            return state;
    }
};