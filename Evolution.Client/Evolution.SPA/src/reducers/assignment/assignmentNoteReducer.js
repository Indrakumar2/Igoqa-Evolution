import { assignmentsActionTypes } from '../../constants/actionTypes';

export const AssignmmentNotesReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case assignmentsActionTypes.ADD_ASSIGNMENT_NOTE:
            if (state.assignmentDetail.AssignmentNotes == null) {
                state = {
                    ...state,
                    assignmentDetail: {
                        ...state.assignmentDetail,
                        AssignmentNotes: []
                    },
                    isbtnDisable: false
                };
            }
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentNotes: [
                        ...state.assignmentDetail.AssignmentNotes, data
                    ]
                },
                isbtnDisable: false,
            };
            return state;
        case assignmentsActionTypes.EDIT_ASSIGNMENT_NOTE: //D661 issue8
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentNotes: data
                },
                isbtnDisable: false
            };
            return state;
        default:
            return state;
    }
};