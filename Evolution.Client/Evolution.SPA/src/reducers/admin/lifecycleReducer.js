import { adminActionTypes } from '../../constants/actionTypes';
const initialState = {
    lifecycle: [],
};
export const LifeCycleReducer = (state = initialState, actions) => {
    const { data, type } = actions;
    switch (type) {
        case adminActionTypes.lifeCycle.ADD_LIFE_CYCLE:
            state = {
                ...state,

            };
            return state;
        case adminActionTypes.lifeCycle.UPDATE_LIFECYCLE:
            state = {
                ...state,

            };
            return state;
        case adminActionTypes.lifeCycle.DELETE_LIFECYCLE:
            state = {
                ...state,

            };
            return state;
        default:
            return state;
    }

};