import { projectActionTypes } from '../../constants/actionTypes';
const actions = {
    AddProjectNotesDetails: (payload) => ({
        type: projectActionTypes.projectNotes.ADD_PROJRCT_NOTE,
        data: payload
    }),
    ProjectNoteShowHideModal: (payload) => ({
        type: projectActionTypes.projectNotes.SHOW_HIDE_NOTE_MODAL,
        data: payload
    }),
    EditProjectNotesDetails: (payload) => ({
        type: projectActionTypes.projectNotes.EDIT_PROJRCT_NOTE,
        data: payload
    }),
};
export const AddProjectNotesDetails = (data) => (dispatch) => {
    dispatch(actions.AddProjectNotesDetails(data));
};
export const ProjectNoteShowHideModal = (payload) => (dispatch) => {
    dispatch(actions.ProjectNoteShowHideModal(payload));
};
export const EditProjectNotesDetails = (editedData) => (dispatch,getstate) => { //D661 issue8
    const state = getstate();
    const index = state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectNotes.findIndex(iteratedValue => iteratedValue.projectNoteId === editedData.projectNoteId);
    const newState =Object.assign([],state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectNotes);
    newState[index] = editedData;
    if (index >= 0) {
        dispatch(actions.EditProjectNotesDetails(newState));
    }
};
