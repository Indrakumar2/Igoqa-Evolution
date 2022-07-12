import { supplierPOActionTypes } from '../../constants/actionTypes';

const actions={
    AddSupplierPoNotesDetails:(payload)=>(
        {
            type:supplierPOActionTypes.ADD_SUPPLIERPO_NOTES,
            data:payload
        }
    ),
    EditSupplierPoNotesDetails: (payload) => ({
        type: supplierPOActionTypes.EDIT_SUPPLIERPO_NOTES,
        data: payload
    }),
};

/**
 * Fetch supplier data action.
 */
export const AddSupplierPoNotesDetails = (data) => (dispatch,getstate)=>{
    dispatch(actions.AddSupplierPoNotesDetails(data));
};

export const EditSupplierPoNotesDetails = (editedData) => (dispatch,getstate) => { //D661 issue8
    const state = getstate();
    const index = state.rootSupplierPOReducer.supplierPOData.SupplierPONotes.findIndex(iteratedValue => iteratedValue.supplierPONoteId === editedData.supplierPONoteId);
    const newState =Object.assign([],state.rootSupplierPOReducer.supplierPOData.SupplierPONotes);
    newState[index] = editedData;
    if (index >= 0) {
        dispatch(actions.EditSupplierPoNotesDetails(newState));
    }
};