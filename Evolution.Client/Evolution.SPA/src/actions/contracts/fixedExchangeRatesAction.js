import { contractActionTypes } from '../../constants/actionTypes';

const actions = {
    FetchContractFixedRate: (payload) => ({
        type: contractActionTypes.FETCH_CONTRACT_FIXEDRATE,
        data: payload
    }),
    AddFixedExchangeRate: (payload) => ({
        type: contractActionTypes.ADD_FIXED_EXHCHANGE_RATE,
        data: payload
    }), 
    EditFixedExchangeRate: (payload) => ({
        type: contractActionTypes.EDIT_FIXED_EXCHANGE_RATE,
        data: payload
    }),
    UpdateFixedExchangeRate: (payload) => ({
        type:  contractActionTypes.UPDATE_FIXED_EXCHANGE_RATE,
        data: payload
    }),
    DeleteFixedExchangeRate: (payload) => ({
        type: contractActionTypes.DELETE_FIXED_EXCHANGE_RATE,
        data: payload
    }),
    UseContractExchangeRate: (payload) => ({
        type: contractActionTypes.exchangeRateActionTypes.USE_CONTRACT_EXCHANGE_RATE,
        data: payload
    }),
    FetchCurrency:(payload) =>({
            type: contractActionTypes.FETCH_CURRENCY,
            data:payload
    }),
    ExchangeRateModalState: (payload) => ({
        type: contractActionTypes.exchangeRateActionTypes.EXCHANGE_RATE_MODAL_OPEN,
        data: payload
    }),
    ExchangeRateEditCheck: (payload) => ({
        type: contractActionTypes.exchangeRateActionTypes.IS_EXCHANGE_RATE_REFERENCE_EDIT,
        data: payload
    }),

};

export const ExchangeRateModalState = (data) => (dispatch) => {
    dispatch(actions.ExchangeRateModalState(data));
};

export const ExchangeRateEditCheck = (data) => (dispatch) => {
    dispatch(actions.ExchangeRateEditCheck(data));
};

export const AddFixedExchangeRate = (data) => (dispatch,getstate) => {
    const state = getstate();
    const modifiedData = Object.assign([], state.RootContractReducer.ContractDetailReducer.contractDetail.ContractExchangeRates);
    modifiedData.push(data);
    dispatch(actions.AddFixedExchangeRate(modifiedData));
};
export const EditFixedExchangeRate = (data) => (dispatch) => {
    dispatch(actions.EditFixedExchangeRate(data));
};
export const UpdateFixedExchangeRate =(data)=>(dispatch, getstate)=> {
    const state = getstate();
    const editRow = Object.assign({}, state.RootContractReducer.ContractDetailReducer.editFixedExchangeDetails,data);
    const index = state.RootContractReducer.ContractDetailReducer.contractDetail.ContractExchangeRates.findIndex(note => (note.exchangeRateId === editRow.exchangeRateId));
    const newState = Object.assign([], state.RootContractReducer.ContractDetailReducer.contractDetail.ContractExchangeRates);
    newState[index] = editRow;
    if (index >= 0) {
        dispatch(actions.UpdateFixedExchangeRate(newState));
    }
};
export const DeleteFixedExchangeRate = (data) => (dispatch, getstate) => {
const state = getstate();
const newState = state.RootContractReducer.ContractDetailReducer.contractDetail.ContractExchangeRates;
data.map(row=>{
    newState.map((iteratedValue, index) => {
        if (iteratedValue.exchangeRateId === row.exchangeRateId) {
            if (iteratedValue.recordStatus !== "N") {
                newState[index].recordStatus = "D";
            }
            else {
                newState.splice(index, 1);
            }
        }
    });
});
 dispatch(actions.DeleteFixedExchangeRate(newState));
};

/**
 * Use Contract Exchange Rate toggle action which deletes the exchange rate if the data = false
 * @param {*Boolean} data 
 */
export const UseContractExchangeRate = (data) => (dispatch,getstate) => {    
    const state = getstate();
    if(data == false){
        const newState = Object.assign([], state.RootContractReducer.ContractDetailReducer.contractDetail.ContractExchangeRates);
        dispatch(DeleteFixedExchangeRate(newState));
    }
    dispatch(actions.UseContractExchangeRate(data));
};