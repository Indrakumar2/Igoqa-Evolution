import { assignmentsActionTypes } from '../../constants/actionTypes';

const actions = {
    AssignmentInstructionsChange: (payload) => ({
        type: assignmentsActionTypes.ASSIGNMENT_INSTRUCTIONS_CHANGE,
        data: payload
    }),
};

export const AssignmentInstructionsChange = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign({}, state.rootAssignmentReducer.assignmentDetail.AssignmentInstructions,data);
    dispatch(actions.AssignmentInstructionsChange(newState));
};