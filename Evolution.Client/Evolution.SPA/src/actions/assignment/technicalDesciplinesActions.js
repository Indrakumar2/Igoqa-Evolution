import { assignmentsActionTypes } from '../../constants/actionTypes';

const actions = {
    AddNewClassification: (payload) => ({
        type: assignmentsActionTypes.ADD_NEW_CLASSIFICATION,
        data: payload
    }),
    DeleteClassification: (payload) => ({
        type: assignmentsActionTypes.DELETE_CLASSIFICATION,
        data: payload
    })
};

//Add new Assignments Classification
export const AddNewClassification = (data) => (dispatch, getstate) => {
    dispatch(actions.AddNewClassification(data));
};

//Delete Assignments Classification
export const DeleteClassification = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootAssignmentReducer.assignmentDetail.assignmentClassificationData);
    data.map(row => {
        newState.map(iteratedValue => {
            if (iteratedValue.classificationId === row.classificationId) {
                const index = newState.findIndex(value => (value.classificationId === row.classificationId));
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    newState.splice(index, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteClassification(newState));
};
