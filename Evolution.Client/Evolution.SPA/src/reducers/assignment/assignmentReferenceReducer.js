import { assignmentsActionTypes } from '../../constants/actionTypes';

export const AssignmentReferenceReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case assignmentsActionTypes.ADD_NEW_ASSIGNMENT_REFERENCE:
            if (state.assignmentDetail.AssignmentReferences == null) {
                state = {
                    ...state,
                    assignmentDetail: {
                        ...state.assignmentDetail,
                        AssignmentReferences: []
                    }
                };
            }
            state = {
                ...state,
                assignmentDetail:{
                    ...state.assignmentDetail,
                    AssignmentReferences:[ ...state.assignmentDetail.AssignmentReferences,data ]
                },
                isbtnDisable: false
            };
            return state;
            case assignmentsActionTypes.ADD_PROJECT_ASSIGNMENT_REFERENCE:
                if (state.assignmentDetail.AssignmentReferences == null) {
                    state = {
                        ...state,
                        assignmentDetail: {
                            ...state.assignmentDetail,
                            AssignmentReferences: []
                        }
                    };
                }
                state = {
                    ...state,
                    assignmentDetail:{
                        ...state.assignmentDetail,
                        AssignmentReferences:[ ...state.assignmentDetail.AssignmentReferences,data ]
                    },
                };
                return state;
            case assignmentsActionTypes.DELETE_ASSIGNMENT_REFERENCE:
            state = {
                ...state,
                assignmentDetail:{
                    ...state.assignmentDetail,
                    AssignmentReferences:data
                },
                isbtnDisable: false
            };
            return state;
            case assignmentsActionTypes.UPDATE_ASSIGNMENT_TAB_REFERENCE:
            state = {
                ...state,
                assignmentDetail:{
                    ...state.assignmentDetail,
                    AssignmentReferences:data
                },
                isbtnDisable: false
            };
            return state;
            case assignmentsActionTypes.FETCH_REFERENCE_TYPES:
            state = {
                ...state,
                AssignmentReferenceTypes:data
            };
            return state;
        default:
            return state;
    }
};