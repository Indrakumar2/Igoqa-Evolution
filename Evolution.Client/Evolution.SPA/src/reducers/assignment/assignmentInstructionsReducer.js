import { assignmentsActionTypes } from '../../constants/actionTypes';

export const AssignmentInstructionsReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case assignmentsActionTypes.ASSIGNMENT_INSTRUCTIONS_CHANGE:
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentInstructions: data
                },
                isbtnDisable: false
            };
        return state; 
        default:
            return state;
    }
};