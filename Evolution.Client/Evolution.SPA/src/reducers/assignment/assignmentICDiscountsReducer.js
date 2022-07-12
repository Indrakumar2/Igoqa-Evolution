import { assignmentsActionTypes } from '../../constants/actionTypes';

export const AssignmentICDiscountsReducer = (state, actions) => {
    const { type, data } = actions;
    switch (type) {
        case assignmentsActionTypes.IS_IC_DISCOUNT_CHANGED:
            state = {
                ...state,
                isInterCompanyDiscountChanged : data
            };
            return state;
        case assignmentsActionTypes.UPDATE_INTERCOMPANY_DISCOUNTS:
            state = {
                ...state,
                assignmentDetail: {
                     ...state.assignmentDetail,
                    AssignmentInterCompanyDiscounts:{ ...state.assignmentDetail.AssignmentInterCompanyDiscounts, ...data }
                },
                isInterCompanyDiscountChanged : true,
                isbtnDisable: false
            };
            
            return state;
            case assignmentsActionTypes.CLEAR_HOST_DISCOUNTS:
            state = {
                ...state,
                assignmentDetail: {
                     ...state.assignmentDetail,
                    AssignmentInterCompanyDiscounts:{ 
                        ...state.assignmentDetail.AssignmentInterCompanyDiscounts,
                        assignmentHostcompanyDiscount:'',
                        assignmentHostcompanyDescription:''
                     }
                },
                
                isbtnDisable: false
            };
            
            return state;
            case assignmentsActionTypes.CLEAR_CONTRACT_HOLDING_DISCOUNTS:
            const newState = {
                ...state.assignmentDetail.AssignmentInterCompanyDiscounts,
                        assignmentContractHoldingCompanyDiscount:'',
                        assignmentContractHoldingCompanyDescription:'',
                        assignmentHostcompanyDiscount:'',
                        assignmentHostcompanyDescription:'',
                        assignmentAdditionalIntercompany1_Name: '',
                        assignmentAdditionalIntercompany1_Code: '',
                        assignmentAdditionalIntercompany2_Name: '',
                        assignmentAdditionalIntercompany2_Code: '',
                        parentContractHoldingCompanyDescription: '',
                        assignmentAdditionalIntercompany1_Discount: '',
                        assignmentAdditionalIntercompany1_Description: '',
                        assignmentAdditionalIntercompany2_Discount: '',
                        assignmentAdditionAlIntercompany2_Description: '',
            };
            newState.assignmentOperatingCompanyDiscount = calculateOCPercent(newState);
            state = {
                ...state,
                assignmentDetail: {
                     ...state.assignmentDetail,
                    AssignmentInterCompanyDiscounts:newState
                },
                isbtnDisable: false
            };
            
            return state;
        
        default:
            return state;
    }
};

function calculateOCPercent(discounts) {
    if(!discounts){
        return 100.00;
    }
    const actualOCPercent = parseFloat(discounts.assignmentOperatingCompanyDiscount);
    if(!isNaN(actualOCPercent) && parseFloat(discounts.assignmentOperatingCompanyDiscount) === 100.00){
        return 100.00;
    }

    const pCHC = Number(discounts.parentContractHoldingCompanyDiscount);
    const pchValue = isNaN(pCHC) ? 0 : pCHC;

    const cHC = Number(discounts.assignmentContractHoldingCompanyDiscount);
    const chcValue = isNaN(cHC) ? 0 : cHC;

    const hC = Number(discounts.assignmentHostcompanyDiscount);
    const hcValue = isNaN(hC) ? 0 : hC;

    const aICO1 = Number(discounts.assignmentAdditionalIntercompany1_Discount);
    const aicoValue1 = isNaN(aICO1) ? 0 : aICO1;

    const aICO2 = Number(discounts.assignmentAdditionalIntercompany2_Discount);
    const aicoValue2 = isNaN(aICO2) ? 0 : aICO2;

    const totalDiscount = 100;
    const calValue =  Number(pchValue) + Number(chcValue) + Number(aicoValue1) + Number(aicoValue2) + Number(hcValue);

    return totalDiscount - calValue;
}