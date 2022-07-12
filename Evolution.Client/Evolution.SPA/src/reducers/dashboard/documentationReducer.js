import { dashBoardActionTypes } from '../../constants/actionTypes';
const initialState = {
};
export const documentationReducer = (state = initialState, actions) => {
    const {
        type,data
    } = actions;
    switch (type) {

        case dashBoardActionTypes.FETCH_DOCUMENTATION_DETAILS:
            state={
                ...state,
                documentationDetails:data
            };
            return state;
        default:
        return state;
    }

};