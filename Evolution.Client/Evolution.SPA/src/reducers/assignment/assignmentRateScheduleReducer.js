import { assignmentsActionTypes } from '../../constants/actionTypes';

export const AssignmentRateScheduleReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case assignmentsActionTypes.ADD_NEW_RATE_SCHEDULE:
            if (state.assignmentDetail.AssignmentContractSchedules == null) {
                state = {
                    ...state,
                    assignmentDetail: {
                        ...state.assignmentDetail,
                        AssignmentContractSchedules: []
                    }
                };
            }

            state = {
                ...state,
                assignmentDetail:{
                    ...state.assignmentDetail,
                    AssignmentContractSchedules:[ ...state.assignmentDetail.AssignmentContractSchedules,data ]
                },
                isbtnDisable:false
            };
            return state;

            case assignmentsActionTypes.DELETE_RATE_SCHEDULE:
            state = {
                ...state,
                assignmentDetail:{
                    ...state.assignmentDetail,
                    AssignmentContractSchedules:data
                },
                isbtnDisable:false 
            };
            return state;

            case assignmentsActionTypes.UPDATE_RATE_SCHEDULE:
            state = {
                ...state,
                assignmentDetail:{
                    ...state.assignmentDetail,
                    AssignmentContractSchedules:data
                },
                isbtnDisable:false 
            };
            return state;
            case assignmentsActionTypes.FETCH_SCHEDULE_NAME:
            state = {
                ...state,
                RateScheduleNames:data
            };
            return state;
            //Added for Contract Rates Expired Validation -Start
            case assignmentsActionTypes.FETCH_CONTRACT_RATES:
            state = {
                ...state,
                ContractRates:data
            };
            return state;
            //Added for Contract Rates Expired Validation -End
            case assignmentsActionTypes.CLEAR_ASSIGNMENT_RATE_SCHEDULE:
                state = {
                    ...state,
                    assignmentDetail:{
                        ...state.assignmentDetail,
                        AssignmentContractSchedules:data
                    },
                    isbtnDisable:false
                };
                return state;
        default:
            return state;
    }
};