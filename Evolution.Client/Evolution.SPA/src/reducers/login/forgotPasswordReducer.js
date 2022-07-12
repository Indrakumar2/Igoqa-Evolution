import { loginActionTypes } from '../../constants/actionTypes';
const initialState = {
    userSecurity: [],
    validateAnswer:false
};
export const forgotPasswordReducer = (state=initialState , actions) => {
    const { type, data } = actions;
    switch (type) {
        case loginActionTypes.SECURITY_QUESTION:
        state = {
            ...state,
            userSecurity: data,
        };
        return state;
        case loginActionTypes.LOGIN_USERNAME:
        state = {
            ...state,
            loginUserName: data
        };
        return state;
    default:
        return state;
    };

};