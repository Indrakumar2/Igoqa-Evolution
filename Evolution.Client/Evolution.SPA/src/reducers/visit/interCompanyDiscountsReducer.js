import { visitActionTypes } from '../../constants/actionTypes';

export const InterCompanyDiscountsReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case visitActionTypes.FETCH_INTER_COMPANY_DISCOUNTS:            
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitInterCompanyDiscounts: data
                }
            };
            return state;
        case visitActionTypes.UPDATE_VISIT_INTERCOMPANY_DISCOUNTS:
            state = {
                ...state,
                visitDetails: {
                    ...state.visitDetails,
                    VisitInterCompanyDiscounts: { ...state.visitDetails.VisitInterCompanyDiscounts, ...data }
                },
                //isbtnDisable: true
            };            
            return state;
        default:
            return state;
    }
};