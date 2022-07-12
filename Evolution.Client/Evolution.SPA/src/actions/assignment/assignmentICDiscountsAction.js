import { assignmentsActionTypes } from '../../constants/actionTypes';
const actions={
    UpdateICDiscounts:(payload)=>({
        type: assignmentsActionTypes.UPDATE_INTERCOMPANY_DISCOUNTS,
        data:payload
    }),
    ClearHostDiscounts:(payload)=>({
        type:assignmentsActionTypes.CLEAR_HOST_DISCOUNTS,
        data:payload
    }),
    ClearContractHoldingDiscounts:(payload)=>({
        type: assignmentsActionTypes.CLEAR_CONTRACT_HOLDING_DISCOUNTS,
        data:payload
    }),
    isInterCompanyDiscountChanged:(payload)=>({
        type: assignmentsActionTypes.IS_IC_DISCOUNT_CHANGED,
        data:payload
    }),
};

/**
 * Update the edited record to assignment ICDiscounts
 * @param {*Object} data 
 */
export const UpdateICDiscounts = (data) => (dispatch) => {
        dispatch(actions.UpdateICDiscounts(data));    
};
export const ClearHostDiscounts = (data) => (dispatch) => {
    dispatch(actions.ClearHostDiscounts(data));    
};
export const ClearContractHoldingDiscounts = (data) => (dispatch) => {
    dispatch(actions.ClearContractHoldingDiscounts(data));    
};
// To check that IC Discount related data got changed or not
export const isInterCompanyDiscountChanged = (data) => (dispatch) => {
    dispatch(actions.isInterCompanyDiscountChanged(data));
};