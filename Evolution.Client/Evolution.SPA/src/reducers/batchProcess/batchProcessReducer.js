import { batchProcessActionTypes } from '../../constants/actionTypes';

const initialState = {
    batchData: {},
};

export const batchProcessReducer = (state = initialState, actions) => {
    const { data, type } = actions;
    switch (type) {
        case batchProcessActionTypes.FETCH_BATCH_PROCESS:
            state = {
                ...state,
                batchData: data,
            };
            return state;
        default:
            return state;
    };
};