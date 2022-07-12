import { contractActionTypes } from '../../constants/actionTypes';

export const ContractReducer = (state, actions) => {
    const { type, data } = actions;
    switch (type) {
        case contractActionTypes.commonActionTypes.FETCH_CONTRACT_DATA:
            state = {
                ...state,
                contractDetail: data,
                ContractSchedulesOnCancel: data.ContractSchedules,
                ContractScheduleRatesOnCancel: data.ContractScheduleRates,
                isbtnDisable: true,
            };
            return state;
        case contractActionTypes.commonActionTypes.UPDATE_INTERACTION_MODE:
            state = {
                ...state,
                interactionMode: data,
            };
            return state;
        case contractActionTypes.SAVE_CONTRACT_DETAILS:

            state = {
                ...state,
                contractSave: data,
                isbtnDisable: true,
            };
            return state;
        case contractActionTypes.commonActionTypes.CURRENT_PAGE:
            state = {
                ...state,
                currentPage: data,
            };
            return state;
            case contractActionTypes.CLEAR_CONTRACT_DETAILS:
            state = {
                ...state,
                contractDetail:{
                    ContractInfo:{
                        contractStatus:'O',
                        contractBudgetMonetaryValue:'',
                        contractBudgetHours:'00.00',
                        contractBudgetMonetaryWarning:75,
                        contractBudgetHoursWarning:75,
                        isFixedExchangeRateUsed:false,
                    }
                },
                customerContractNumber:'',
                contractHoldingCompanyName:'',                
                customerName: '',
                customerList:[],
                // contractCRMReason:'',
                // startDate:null,
                // endDate:null,
                isbtnDisable: true
            };
            return state;
            
        default:
            return state;
    }
};
