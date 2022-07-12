import { contractActionTypes } from '../../constants/actionTypes';
const actions = {
    AddContractNotesDetails: (payload) => ({
        type: contractActionTypes.ADD_CONTRACT_NOTE,
        data: payload
    }),
    EditContractNotesDetails: (payload) => ({
        type: contractActionTypes.EDIT_CONTRACT_NOTE,
        data: payload
    }),
};
export const AddContractNotesDetails = (data) => (dispatch) => {
    dispatch(actions.AddContractNotesDetails(data));
};

export const EditContractNotesDetails = (editedData) => (dispatch,getstate) => { //D661 issue8
    const state = getstate();
    const index = state.RootContractReducer.ContractDetailReducer.contractDetail.ContractNotes.findIndex(iteratedValue => iteratedValue.contractNoteId === editedData.contractNoteId);
    const newState =Object.assign([],state.RootContractReducer.ContractDetailReducer.contractDetail.ContractNotes);
    newState[index] = editedData;
    if (index >= 0) {
        dispatch(actions.EditContractNotesDetails(newState));
    }
};