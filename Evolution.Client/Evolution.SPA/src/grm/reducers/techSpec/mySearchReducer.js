import { techSpecActionTypes } from '../../constants/actionTypes';
export const TechSpecMySearchReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {       
        case techSpecActionTypes.techSpecSearch.MY_SEARCH_DATA:
            state = {
                ...state,
                mySearchData:data,
                isMySearchLoaded:true
            };
            return state;
            case techSpecActionTypes.techSpecSearch.GET_SELECTED_MY_SEARCH_DATA:
            state = {
                ...state,
                 selectedMySearch:data,
                 currentPage:'My Search'
            };
            return state;       
        case techSpecActionTypes.CLEAR_MY_SEARCH_DATA:
            state = {
                ...state,
                mySearchData:[],
                isMySearchLoaded:false
            };
        default:
            return state;
    }
};