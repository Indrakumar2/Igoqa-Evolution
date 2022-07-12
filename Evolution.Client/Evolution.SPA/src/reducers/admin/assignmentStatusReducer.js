import { adminActionTypes } from '../../constants/actionTypes';
const initialState = {
    assignmnetStatus: [],
};
export const AssignmentStatusReducer = (state = initialState, actions) => {
    const { data, type } = actions;
    switch (type) {
        case adminActionTypes.status.ADD_STATUS:
            if(state.assignmnetStatus==null){
                state={
                    ...state.assignmnetStatus,
                    assignmnetStatus:[]
                };
            };
            state = {
                ...state,
                assignmnetStatus: [ ...state.assignmnetStatus, data ]
            };
            return state;
        case adminActionTypes.status.UPDATE_STATUS:
            state = {
                ...state,
                assignmnetStatus:  data 
            };
            return state;
        case adminActionTypes.status.DELETE_STATUS:
            state = {
                ...state,
                assignmnetStatus:  data 
            };
            return state;
        case adminActionTypes.status.FETCH_STATUS:
            state = {
                ...state,
                assignmnetStatus: data
            };
            return state;
        
        default:
            return state;
    }

};