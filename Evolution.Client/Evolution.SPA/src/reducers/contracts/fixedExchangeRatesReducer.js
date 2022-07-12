import { contractActionTypes } from '../../constants/actionTypes';
// const initialState = {
//     showButton: false,
//     ContractFixedRate: [],
//     editFixedExchangeDetails: {},
//     chechBoxHideButton: true,
//     isExchangeRateEdit: false,
//     isExchangeRateModalOpen: false,
//     isExchangeEdit: false,
//     addFixedData: {},
//     currencyData: [],
// };

export const FixedExchangeRatesReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case contractActionTypes.FETCH_CONTRACT_FIXEDRATE:
            state = {
                ...state,
                ContractFixedRate: data,

            };
            return state;
        case contractActionTypes.FETCH_CURRENCY:
            state = {
                ...state,
                currencyData: data,

            };
            return state;
        case contractActionTypes.ADD_FIXED_EXHCHANGE_RATE:
            state = {
                ...state,
                contractDetail: {
                    ...state.contractDetail,
                    ContractExchangeRates: data
                },
                isbtnDisable: false
            };
            return state;
        case contractActionTypes.EDIT_FIXED_EXCHANGE_RATE:
            state = {
                ...state,
                editFixedExchangeDetails: data,
            };
            return state;
        case contractActionTypes.DELETE_FIXED_EXCHANGE_RATE:
            state = {
                ...state,
                contractDetail: {
                    ...state.contractDetail,
                    ContractExchangeRates: data
                }, isbtnDisable: false
            };
            return state;
        case contractActionTypes.exchangeRateActionTypes.EXCHANGE_RATE_MODAL_OPEN:
            if (data === false) {
                state = {
                    ...state,
                    editFixedExchangeDetails: {}
                };
            }
            state = {
                ...state,
                isExchangeRateModalOpen: data,
            };
            return state;
        case contractActionTypes.exchangeRateActionTypes.IS_EXCHANGE_RATE_REFERENCE_EDIT:
            state = {
                ...state,
                isExchangeRateEdit: data,
            };
            return state;
        case contractActionTypes.UPDATE_FIXED_EXCHANGE_RATE:
            state = {
                ...state,
                contractDetail: {
                    ...state.contractDetail,
                    ContractExchangeRates: data
                },
                isbtnDisable: false
            };
            return state;
        case contractActionTypes.exchangeRateActionTypes.USE_CONTRACT_EXCHANGE_RATE:
            state = {
                ...state,
                contractDetail: {
                    ...state.contractDetail,
                    ContractInfo: {
                        ...state.contractDetail.ContractInfo,
                        isFixedExchangeRateUsed: data
                    }
                }
            };
            return state;
        default:
            return state;
    }
};