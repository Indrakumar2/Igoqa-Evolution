import { projectActionTypes } from '../../constants/actionTypes';

export const AssignmentReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case projectActionTypes.FETCH_ASSIGNMENT:
            state = {
                ...state,
                assignmentData:data.responseResult,
                selectedAssignmentStatus:data.selectedValue
            };
            return state;
        default:
            return state;
    }
};