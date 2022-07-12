import { supplierActionTypes } from '../../constants/actionTypes';

export const NotesReducer = (state,actions)=>{
    const { type,data } = actions;
    switch (type) {
        case supplierActionTypes.ADD_SUPPLIER_NOTES:
            state = {
                ...state,
               supplierData:{
                   ...state.supplierData,
                   SupplierNotes:data
               },
               isbtnDisable:false
            };
            return state;
        case supplierActionTypes.EDIT_SUPPLIER_NOTES: //D661 issue8
            state = {
                ...state,
                supplierData: {
                    ...state.supplierData,
                    SupplierNotes: data
                },
                isbtnDisable: false
            };
        return state;
        default:
            return state;
    };
};