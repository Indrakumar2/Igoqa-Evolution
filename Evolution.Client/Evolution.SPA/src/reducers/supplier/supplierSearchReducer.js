import { supplierActionTypes } from '../../constants/actionTypes';
import { getlocalizeData } from '../../utils/commonUtils';

const localConstant=getlocalizeData();

export const SupplierSearchReducer = (state,actions) => {
    const { type,data } = actions;
    switch(type){
        case supplierActionTypes.SUPPLIER_FETCH_STATE:
            state = {
                ...state,
                supplierSearch:{
                    ...state.supplierSearch,
                    state:data
                }
            };
            return state;
        case supplierActionTypes.SUPPLIER_FETCH_CITY:
            state = {
                ...state,
                supplierSearch:{
                    ...state.supplierSearch,
                    city:data
                }
            };
            return state;
        case supplierActionTypes.FETCH_SUPPLIER_SEARCH_LIST:
            state = {
                ...state,
                supplierSearch:{
                    ...state.supplierSearch,
                    supplierSearchList:data
                }
            };
            return state;
        case supplierActionTypes.CLEAR_SUPPLIER_SEARCH_LIST:
            state = {
                ...state,
                supplierSearch:{
                    ...state.supplierSearch,
                    supplierSearchList:[],
                    state:[],
                    city:[],
                }
            };
            return state;

        default:
            return state;
    }; 
};