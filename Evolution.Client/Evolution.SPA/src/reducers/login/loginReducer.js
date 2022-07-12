import { loginActionTypes } from '../../constants/actionTypes';
const initialState = {
    userDetails: {},
    isAuthenticated: false,
    authData: {},
    serverError: false
};

export const loginReducer = (state = initialState, actions) => {
    const { type, data } = actions;
    switch (type) {
        case loginActionTypes.AUTHENTICATE_LOGIN_SUCCESS:
            state = {
                ...state,
                userDetails: data,
                isAuthenticated: true,
                serverError: false
            };
            return state;
        case loginActionTypes.AUTHENTICATE_LOGIN_FAILED:
            state = {
                ...state,
                isAuthenticated: false,
                userDetails: {},
                authData: {},
                serverError: data
            };
            return state;
        case loginActionTypes.AUTHENTICATE_FORBIDDEN:
            state = {
                ...state,
                serverError: data
            };
            return state;
        case loginActionTypes.REMOVE_FORBIDDEN:
            state = {
                ...state,
                serverError: false
            };
            return state;
        case loginActionTypes.TOKEN_STORAGE:
            state = {
                ...state,
                authData: data
            };
            return state;
        case loginActionTypes.SET_USER_MENU:
            state = {
                ...state,
                userMenu: data
            };
           
            return state;
        default:
            return state;
    }
};