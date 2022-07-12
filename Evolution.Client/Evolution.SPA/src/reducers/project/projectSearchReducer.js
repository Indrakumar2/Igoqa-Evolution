import {
    projectActionTypes
} from '../../constants/actionTypes';

export const ProjectSearchReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case projectActionTypes.CLEAR_SEARCH_CUSTOMER_LIST:
            state = {
                ...state,
                customerList: []
            };
            return state;
        case projectActionTypes.FETCH_CUSTOMER_LIST_SUCCESS:
            state = {
                ...state,
                customerList: data
            };
            return state;
        case projectActionTypes.FETCH_PROJECT_LIST_SUCCESS:
            state = {
                ...state,
                projectSearchList: data
            };
            return state;
        case projectActionTypes.CLEAR_SEARCH_PROJECT_LIST:
            state = {
                ...state,
                projectSearchList: []
            };
            return state;

            default:
            return state;
    }
};