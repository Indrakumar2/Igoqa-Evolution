import { contractActionTypes } from '../../constants/actionTypes';

export const ContractNoteReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case contractActionTypes.ADD_CONTRACT_NOTE:  //Notes Add

            if (state.contractDetail.ContractNotes == null) {
                state = {
                    ...state,
                    contractDetail: {
                        ...state.contractDetail,
                        ContractNotes: []
                    },
                    isbtnDisable: false
                };
            }
            state = {
                ...state,
                contractDetail: {
                    ...state.contractDetail,
                    ContractNotes: [
                        data,
                        ...state.contractDetail.ContractNotes 
                    ]
                },
                isbtnDisable: false,
            };
            return state;
            case contractActionTypes.EDIT_CONTRACT_NOTE: //D661 issue8
                state = {
                    ...state,
                    contractDetail: {
                        ...state.contractDetail,
                        ContractNotes: data
                    },
                    isbtnDisable: false
                };
            return state;
        default: return state;
    }
};