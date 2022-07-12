import { commonActionTypes } from '../constants/actionTypes';
const initialState = {
    loader: false,
    interactionMode: false,
    currentPage:'',
    currentModule:'dashboard',
    currentPageMode:null,
    emailTemplate:'',
    isGrmMasterDataFeteched:false,
    isARSMasterDataFeteched:false
};
export const CommonReducer = (state = initialState, action) => {
    const { type, data } = action;
    switch (type) {
        case commonActionTypes.SHOW_LOADER: 
                state = {
                    ...state,
                    loader:data                   
                    };
            return state;
            case commonActionTypes.HIDE_LOADER: 
            state = {
                ...state,
                 loader:data                
                };
            return state;        
        case commonActionTypes.INTERACTION_MODE:
            state = {
                ...state,
                interactionMode:data
            };
            return state;
        case commonActionTypes.UPDATE_CURRENT_PAGE:
            state = {
                ...state,
                currentPage:data
            };
            return state;
            case commonActionTypes.UPDATE_CURRENT_MODULE:
                state = {
                    ...state,
                    currentModule:data
                };
            return state;
        case  commonActionTypes.GET_CURRENT_MODE:
            state = {
                ...state,
                currentPageMode:data
            };
            return state;
            case commonActionTypes.FETCH_EMAIL_TEMPLATE:
                state ={
                    ...state,
                    emailTemplate:data
                };
                return state;
            case commonActionTypes.IS_GRM_MASTER_DATA_LOADED:
                    state ={
                        ...state,
                        isGrmMasterDataFeteched:data
                    };
                    return state;
            case commonActionTypes.IS_ARS_MASTER_DATA_LOADED:
                state ={
                    ...state,
                    isARSMasterDataFeteched:data
                };
            return state;
        default: 
            return state;
    }
};