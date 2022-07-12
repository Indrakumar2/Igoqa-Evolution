import { assignmentsActionTypes } from '../../constants/actionTypes';
const actions = {
    AddAssignmentNotesDetails: (payload) => ({
        type: assignmentsActionTypes.ADD_ASSIGNMENT_NOTE,
        data: payload
    }),
    EditAssignmentNotesDetails: (payload) => ({
        type: assignmentsActionTypes.EDIT_ASSIGNMENT_NOTE,
        data: payload
    }),
};
export const AddAssignmentNotesDetails = (data) => (dispatch) => {
    dispatch(actions.AddAssignmentNotesDetails(data));
};

export const EditAssignmentNotesDetails = (editedData) => (dispatch,getstate) => { //D661 issue8
    const state = getstate();
    const index =state.rootAssignmentReducer.assignmentDetail.AssignmentNotes.findIndex(iteratedValue => iteratedValue.assignmnetNoteId === editedData.assignmnetNoteId);
    const newState =Object.assign([],state.rootAssignmentReducer.assignmentDetail.AssignmentNotes);
    newState[index] = editedData;
    if (index >= 0) {
        dispatch(actions.EditAssignmentNotesDetails(newState));
    }
};