import { supplierPOActionTypes } from '../../constants/actionTypes';

export const SupplierPONotesReducer = (state,actions)=>{
    const { type,data } = actions;
    switch (type) {
        case supplierPOActionTypes.ADD_SUPPLIERPO_NOTES:
            if (state.supplierPOData.SupplierPONotes == null) {
                state = {
                    ...state,
                    supplierPOData: {
                        ...state.supplierPOData,
                        SupplierPONotes: []
                    },
                    isbtnDisable: false
                };
            }
            state = {
                ...state,
                supplierPOData: {
                    ...state.supplierPOData,
                    SupplierPONotes: [
                        ...state.supplierPOData.SupplierPONotes, data
                    ]
                },
                isbtnDisable: false,
            };
            return state;
        case supplierPOActionTypes.EDIT_SUPPLIERPO_NOTES: //D661 issue8
            state = {
                ...state,
                supplierPOData: {
                    ...state.supplierPOData,
                    SupplierPONotes: data
                },
                isbtnDisable: false
            };
            return state;
        default:
            return state;
    };
};