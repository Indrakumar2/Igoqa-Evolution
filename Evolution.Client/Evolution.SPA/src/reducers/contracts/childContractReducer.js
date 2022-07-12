import { contractActionTypes } from '../../constants/actionTypes';
export const ChildContractReducer = (state , action) => {
    const { type, data } = action;
    switch (type) {
        case contractActionTypes.childContract.FETCH_CHILD_CONTRACTS_OF_PARENT:
            state = {
                ...state,
                childContractsOfParent: data.responseResult,
                selectedContractStatus:data.selectedValue
            };
            return state;      
        default:
        return state;
    }
};