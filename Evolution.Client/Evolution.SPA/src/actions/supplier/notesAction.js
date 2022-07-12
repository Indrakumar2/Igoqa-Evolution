import { supplierActionTypes } from '../../constants/actionTypes';

const actions={
    AddSupplierNotes:(payload)=>(
        {
            type:supplierActionTypes.ADD_SUPPLIER_NOTES,
            data:payload
        }
    ),
    EditSupplierNotes: (payload) => ({
        type: supplierActionTypes.EDIT_SUPPLIER_NOTES,
        data: payload
    }),
};

/**
 * Fetch supplier data action.
 */
export const AddSupplierNotes = (data) => (dispatch,getstate)=>{
    dispatch(actions.AddSupplierNotes(data));
};

export const EditSupplierNotes = (editedData) => (dispatch,getstate) => { //D661 issue8
    const state = getstate();
    const index = state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierNotes.findIndex(iteratedValue => iteratedValue.supplierNoteId === editedData.supplierNoteId);
    const newState =Object.assign([],state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierNotes);
    newState[index] = editedData;
    if (index >= 0) {
        dispatch(actions.EditSupplierNotes(newState));
    }
};