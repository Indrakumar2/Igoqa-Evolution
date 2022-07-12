import { projectActionTypes } from '../../constants/actionTypes';

export const SupplierPoReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case projectActionTypes.FETCH_SUPPLIERPO:
            state = {
                ...state,
                supplierPoData:data
            };
            return state;
        default:
            return state;
    }
};