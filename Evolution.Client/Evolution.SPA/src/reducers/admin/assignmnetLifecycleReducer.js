import { adminActionTypes } from '../../constants/actionTypes';
const initialState = {
    assignmnetlifecycle: [],
};
export const LifeCycleReducer = (state = initialState, actions) => {
    const { data, type } = actions;
    switch (type) {
        case adminActionTypes.lifeCycle.ADD_LIFE_CYCLE:
            if(state.assignmnetlifecycle==null){
                state={
                    ...state.assignmnetlifecycle,
                    assignmnetlifecycle:[]
                };
            };
            state = {
                ...state,
                assignmnetlifecycle: [ ...state.assignmnetlifecycle, data ]
            };
            return state;
        case adminActionTypes.lifeCycle.UPDATE_LIFECYCLE:
            state = {
                ...state,
                assignmnetlifecycle:  data 
            };
            return state;
        case adminActionTypes.lifeCycle.DELETE_LIFECYCLE:
            state = {
                ...state,
                assignmnetlifecycle:  data 
            };
            return state;
        case adminActionTypes.lifeCycle.FETCH_LIFECYCLE:
            state = {
                ...state,
                assignmnetlifecycle: data
            };
            return state;
        
        default:
            return state;
    }

};