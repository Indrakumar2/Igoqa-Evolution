import { techSpecActionTypes } from '../../constants/actionTypes';
import { techSpecActionTypes as grmTechSpecActionTypes } from '../../grm/constants/actionTypes';

export const ArsReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case techSpecActionTypes.FETCH_PRE_ASSIGNMENT_IDS:
            state = {
                ...state,
                preAssignmentIds:data
            };
            return state;
        case  techSpecActionTypes.SET_DEFAULT_PREASSIGNMENT_ID:
            state ={
                ...state,
                defaultPreAssignmentID:data && data.id || ''        
            };
            return state;
        case techSpecActionTypes.GET_SELECTED_PRE_ASSIGNMENT:
            state = {
                ...state,
                selectedPreAssignment:data
            };
            return state;
        case techSpecActionTypes.COMPARE_ARS_ASSIGNMENT_RESOURCES:
            state = {
                ...state,
                isResourceMatched:data,
                unMatchedResources: !data ? state.unMatchedResources : []
            };
            return state;
        case techSpecActionTypes.UNMATCHED_RESOURCES:
            state = {
                ...state,
                unMatchedResources: data
            };
            return state;
        case techSpecActionTypes.FETCH_OPERATIONAL_MANAGERS:
            state = {
                ...state,
                operationalManagers:data
            };
            return state;
        case techSpecActionTypes.FETCH_TECHNICAL_SPECIALISTS:
            state = {
                ...state,
                technicalSpecialists:data
            };
            return state;
        case techSpecActionTypes.CLEAR_ARS_SEARCH_DETAILS:
            state = {
                ...state,
                operationalManagers:[],
                technicalSpecialists:[],
            };  
            return state;
        case techSpecActionTypes.SELECTED_ARS_MY_TASK:
            state = {
                ...state,
                selectedARSMyTask : data
            };
            return state;
        case techSpecActionTypes.FETCH_PLO_TECHSPEC_SERVICES:
            state = {
                ...state,
                ploTaxonomyService: data
            };
            return state;
        case techSpecActionTypes.FETCH_PLO_TECHSPEC_SUB_CATEGORY:
            state = {
                ...state,
                ploTaxonomySubCategory: data
            };
            return state;
        case techSpecActionTypes.CLEAR_PLO_TECHSPEC_SERVICES:
            state = {
                ...state,
                ploTaxonomyService: []
            };
            return state;
        case techSpecActionTypes.CLEAR_PLO_TECHSPEC_SUB_CATEGORY:
            state = {
                ...state,
                ploTaxonomySubCategory: []
            };
            return state;
        case techSpecActionTypes.OVERRIDEN_RESOURCES:
            state = {
                ...state,
                overridenResources: data,
                isbtnDisable: false//D651
            };
            return state;
        case grmTechSpecActionTypes.ARS_CH_COORDINATOR_INFO:
            state = {
                ...state,
                arsCHCoordinatorInfo: data
            };
            return state;
        case grmTechSpecActionTypes.ARS_OP_COORDINATOR_INFO:
             state = {
                ...state,
                arsOPCoordinatorInfo: data
            };
            return state;
        case grmTechSpecActionTypes.FETCH_ARS_COMMENT_HISTORY_REPORT:
            state = {
                ...state,
                arsCommentHistory: data
            };
            return state;
        default:
            return state;
    }
};