import { assignmentsActionTypes } from '../../constants/actionTypes';

export const AdditionalExpensesReducer = (state, actions) => {
    const { type, data } = actions;
    switch (type) {
        case assignmentsActionTypes.ADD_ADDITIONAL_EXPENSES:
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentAdditionalExpenses:data
                },
                isbtnDisable:false
            };
            return state;
        case assignmentsActionTypes.UPDATE_ADDITIONAL_EXPENSES:
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentAdditionalExpenses:data
                },
                isbtnDisable:false
            };
            return state;
        case assignmentsActionTypes.DELETE_ADDITIONAL_EXPENSES:
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentAdditionalExpenses:data
                },
                isbtnDisable:false
            };
            return state;
        case assignmentsActionTypes.FETCH_ASSIGNMENT_ADDITIONAL_EXPENSES:
            state = {
                ...state,
                assignmentAdditionalExpenses:data
            };
            return state;
        case assignmentsActionTypes.CLEAR_ADDITIONAL_EXPENSES:
            state = {
                ...state,
                assignmentDetail: {
                    ...state.assignmentDetail,
                    AssignmentAdditionalExpenses:data
                },
                isbtnDisable:false
            };
            return state;
        default:
            return state;
    };
};