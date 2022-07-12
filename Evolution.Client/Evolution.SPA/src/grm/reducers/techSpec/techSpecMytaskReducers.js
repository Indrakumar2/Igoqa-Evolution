import { techSpecActionTypes } from '../../constants/actionTypes';
export const TechSpecMytaskReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {       
        case techSpecActionTypes.techSpecSearch.TECH_SPEC_MYTASK_DATA:
            state = {
                ...state,
                techSpecMytaskData:data,
                isMyTasksLoaded: true
            };
            return state;
            case techSpecActionTypes.techSpecSearch.GET_TECH_SPEC_SELECTED_MYTASK:
            state = {
                ...state,
                 selectedMytaskDetails:data,
                 techSpecIsMytaskPath:true,
                 currentPage:'Draft Edit Profile'
            };
           
            return state;
            case techSpecActionTypes.CLEAR_MY_TASKS_DATA:
                state = {
                    ...state,
                    techSpecMytaskData:[],
                    isMyTasksLoaded:false
                };
                return state;
            case techSpecActionTypes.techSpecSearch.TECH_SPEC_MYTASK_ASSIGN_USERS:
                state = {
                    ...state,
                    techSpecMytaskAssignUsers:data,
                };
                return state;
            case techSpecActionTypes.techSpecSearch.CLEAR_TECH_SPEC_MYTASK_ASSIGN_USERS:
                state = {
                    ...state,
                    techSpecMytaskAssignUsers:[],
                };
                return state;
        default:
            return state;
    }
};