import { techSpecActionTypes } from '../../constants/actionTypes';

const actions = {
    AddComments: (payload) => (
        {
            type: techSpecActionTypes.commentsActionTypes.ADD_COMMENTS,
            data: payload
        }
    ),
    EditComments: (payload) => ({
        type: techSpecActionTypes.commentsActionTypes.EDIT_COMMENTS,
        data: payload
    }),
     
};

export const AddComments = (data) => (dispatch) => {
    dispatch(actions.AddComments(data));
};

export const EditComments = (editedData) => (dispatch,getstate) => { //D661 issue8
    const state = getstate();
    const index = state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistNotes.findIndex(iteratedValue => iteratedValue.id === editedData.id);
    const newState =Object.assign([],state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistNotes);
    newState[index] = editedData;
    if (index >= 0) {
        dispatch(actions.EditComments(newState));
    }
};