import { visitActionTypes } from '../../constants/actionTypes';

export const HistoricalVisitReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case visitActionTypes.FETCH_HISTORICAL_VISIT:     
            state = {
                ...state,
                historicalVisits: data      
            };
            return state;        
        default:
            return state;
    }
};