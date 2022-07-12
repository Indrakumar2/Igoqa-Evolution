import { techSpecActionTypes } from '../../constants/actionTypes';
import { sideMenu } from '../../../constants/actionTypes';

export const QuickSearchReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case techSpecActionTypes.techSpecSearch.FETCH_QUICK_SEARCH_DATA:
            state = {
                ...state,
                quickSearchDetails: data,
                isTechSpecDataChanged:true,
            };
            return state;
        case techSpecActionTypes.quickSearchActionTypes.UPDATE_QUICK_SEARCH_INFORMATION:
            state = {
                ...state,
                quickSearchDetails: data,
                isTechSpecDataChanged:false,
            };
            return state;
        case techSpecActionTypes.quickSearchActionTypes.ADD_SEARCH_RESULT_DATA:
            state = {
                ...state,
                quickSearchResults: data
            };
            return state;
        case techSpecActionTypes.quickSearchActionTypes.CLEAR_QUICK_SEARCH_INFORMATION:
            state = {
                ...state,
                quickSearchDetails: {
                    searchParameter: {
                        projectName:"",
                        optionalSearch: {}
                    },
                },
                isTechSpecDataChanged:true,
            };

            return state;
            case techSpecActionTypes.quickSearchActionTypes.CLEAR_QUICK_SEARCH_RESULTS:
            state = {
                ...state,
                quickSearchResults:[],
                isTechSpecDataChanged:true,
            };
            return state;
        case sideMenu.HANDLE_MENU_ACTION:
            state = {
                ...state,
                quickSearchDetails:{
                    searchAction:"SS",
                    searchParameter:{}
                },
                quickSearchResults:[],
                isTechSpecDataChanged:true
            };
            return state;
        case techSpecActionTypes.ADD_OPTIONAL_SEARCH:
        state = {
            ...state,
            quickSearchDetails:{
                ...state.quickSearchDetails,
                searchParameter:{
                    ...state.quickSearchDetails.searchParameter,
                    optionalSearch:data
                } 
            },
            isTechSpecDataChanged:false,
        };
        return state;      
        default:
            return state;
    }
};