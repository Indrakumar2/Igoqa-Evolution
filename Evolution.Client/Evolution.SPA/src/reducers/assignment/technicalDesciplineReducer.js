import { assignmentsActionTypes } from '../../constants/actionTypes';

export const TechnicalDesciplineReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case assignmentsActionTypes.ADD_NEW_CLASSIFICATION:
            if (state.assignmentDetail.assignmentClassificationData == null) {
                state = {
                    ...state,
                    assignmentDetail: {
                        ...state.assignmentDetail,
                        assignmentClassificationData: []
                    },
                    isbtnDisable: false
                };
            }

            state = {
                ...state,
                assignmentDetail:{
                    ...state.assignmentDetail,
                    assignmentClassificationData:[ ...state.assignmentDetail.assignmentClassificationData,data ]
                },
                isbtnDisable: false
            };
            return state;
        default:
            return state;
    }
};