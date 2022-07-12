import { techSpecActionTypes } from '../../constants/actionTypes';
import { sideMenu } from '../../../constants/actionTypes';

export const PreAssignmentReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case techSpecActionTypes.FETCH_PRE_ASSIGNMENT:
            state = {
                ...state,
                preAssignmentDetails:data,
                isTechSpecDataChanged:true,
            };
            return state;  
        case techSpecActionTypes.CHECK_PREASSIGNMENT_WON:
            state={
                   ...state,
                   isPreAssignmentWon:data,
                
            };
            return state;
        case techSpecActionTypes.UPDATE_PRE_ASSIGNMENT_DETAILS:
            state = {
                ...state,
                preAssignmentDetails:{
                    ...state.preAssignmentDetails,
                    searchParameter:data
                },
                isTechSpecDataChanged:false,
            };
            return state;
        case techSpecActionTypes.FETCH_CONTRACT_HOLDING_COORDINATOR:
            state = {
                ...state,
                chCoordinators:data
            };
            return state;
        case techSpecActionTypes.FETCH_OPERATING_COORDINATOR:
            state = {
                ...state,
                ocCoordinators:data
            };
            return state;
        case techSpecActionTypes.CLEAR_CONTRACT_HOLDING_COORDINATOR:
            state = {
                ...state,
                chCoordinators:[],
                preAssignmentDetails:{
                    ...state.preAssignmentDetails,
                    searchParameter:data
                }
            };
            return state;
        case techSpecActionTypes.CLEAR_OPERATING_COORDINATOR:
            state = {
                ...state,
                ocCoordinators:[],
                preAssignmentDetails:{
                    ...state.preAssignmentDetails,
                    searchParameter:data
                }
            };
            return state;
        case techSpecActionTypes.FETCH_ASSIGNMENT_TYPE:
            state = {
                ...state,
                assignmentType:data
            };
            return state;
        case techSpecActionTypes.ADD_SUB_SUPPLIER:
            state = {
                ...state,
                preAssignmentDetails:{
                    ...state.preAssignmentDetails,
                    searchParameter:{
                        ...state.preAssignmentDetails.searchParameter,
                        subSupplierInfos:data
                    } 
                },
                isTechSpecDataChanged:false,
            };
            return state;
        case techSpecActionTypes.UPDATE_SUB_SUPPLIER:
            state = {
                ...state,
                preAssignmentDetails:{
                    ...state.preAssignmentDetails,
                    searchParameter:{
                        ...state.preAssignmentDetails.searchParameter,
                        subSupplierInfos:data
                    } 
                },
                isTechSpecDataChanged:false,
            };
            return state;
        case techSpecActionTypes.DELETE_SUB_SUPPLIER:
            state = {
                ...state,
                preAssignmentDetails:{
                    ...state.preAssignmentDetails,
                    searchParameter:{
                        ...state.preAssignmentDetails.searchParameter,
                        subSupplierInfos:data
                    } 
                },
                isTechSpecDataChanged:false,
            };
            return state;
        case techSpecActionTypes.UPDATE_ACTION_DETAILS:
            state = {
                ...state,
                preAssignmentDetails:data,
                isTechSpecDataChanged:false,
            };
            return state;
        case techSpecActionTypes.PRE_ASSIGNMENT_TECH_SPEC_SEARCH:
            state ={
                ...state,
                    techspecList:data,
            };
                return state;
                case techSpecActionTypes.SHOW_GOOGLE_MAP:
                    state ={
                        ...state,
                            isShowGoogleMap:data,
                    };
                        return state;      
        case techSpecActionTypes.UPDATE_ASSIGNMENT_ON_PRE_ASSIGNMENT:
            state = {
                ...state,
                preAssignmentDetails:data
            };
            return state;
        case techSpecActionTypes.FETCH_DISPOSITION_TYPE:
            state = {
                ...state,
                dispositionType:data
            };
            return state;
        case techSpecActionTypes.ADD_OPTIONAL_SEARCH:
            state = {
                ...state,
                preAssignmentDetails:{
                    ...state.preAssignmentDetails,
                    searchParameter:{
                        ...state.preAssignmentDetails.searchParameter,
                        optionalSearch:data
                    } 
                },
                isTechSpecDataChanged:false,
            };
            return state;
        case sideMenu.HANDLE_MENU_ACTION:
            state = {
                ...state,
                preAssignmentDetails:{},
                techspecList:[],
                chCoordinators:[],
                ocCoordinators:[],
                isPreAssignmentWon:false,
            };
            return state;
             //D576
        case  techSpecActionTypes.UNSAVED_BTN_HANDLER:
            state={
                ...state,
                isTechSpecDataChanged:data
            };
            return state;
        default:
            return state;
    }
};