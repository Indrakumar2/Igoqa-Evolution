import { assignmentsActionTypes } from '../../constants/actionTypes';

export const AssignedSpecialistsReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case assignmentsActionTypes.FETCH_CHARGE_RATES:
            state = {
                ...state,
                chargeRates:data
            };
            return state;
        case assignmentsActionTypes.FETCH_PAY_RATES:
            state = {
                ...state,
                payRates:data
            };
            return state;
        case assignmentsActionTypes.FETCH_PAY_SCHEDULE:
            state = {
                ...state,
                paySchedules:data
            };
            return state;
        case assignmentsActionTypes.ADD_TECHSPEC_SCHEDULES:
            state = {
                ...state,
                assignmentDetail:{
                    ...state.assignmentDetail,
                    AssignmentTechnicalSpecialists:data
                },
                isbtnDisable:false
            };
            return state;
        case assignmentsActionTypes.UPDATE_TECHSPEC_SCHEDULES:
            state = {
                ...state,
                assignmentDetail:{
                    ...state.assignmentDetail,
                    AssignmentTechnicalSpecialists:data
                },
                isbtnDisable:false
            };
            return state;
        case assignmentsActionTypes.DELETE_TECHSPEC_SCHEDULES:
            state = {
                ...state,
                assignmentDetail:{
                    ...state.assignmentDetail,
                    AssignmentTechnicalSpecialists:data
                },
                isbtnDisable:false
            };
            return state;
        case assignmentsActionTypes.ASSIGN_TECHNICAL_SPECIALIST:
            state = {
                ...state,
                assignmentDetail:{
                    ...state.assignmentDetail,
                    AssignmentTechnicalSpecialists:data
                },
                isbtnDisable:false
            };
            return state;
        case assignmentsActionTypes.UPDATE_ASSIGNED_TECHSPEC:
            state = {
                ...state,
                assignmentDetail:{
                    ...state.assignmentDetail,
                    AssignmentTechnicalSpecialists:data
                },
                isbtnDisable:false
            };
            return state;
        case assignmentsActionTypes.DELETE_ASSIGNED_TECHSPEC:
            state = {
                ...state,
                assignmentDetail:{
                    ...state.assignmentDetail,
                    AssignmentTechnicalSpecialists:data
                },
                isbtnDisable:false
            };
            return state;
        case assignmentsActionTypes.UPDATE_TAXONOMY:
            state = {
                ...state,
                assignmentDetail:{
                    ...state.assignmentDetail,
                    AssignmentTaxonomy:data.taxonomy1,
                    // AssignmentTaxonomyold:data.oldtaxonomy?data.oldtaxonomy:null
                },
                AssignmentTaxonomyold:data.oldtaxonomy,
                isbtnDisable:false
            };
            return state;
        case assignmentsActionTypes.CLEAR_CHARGE_AND_PAY_RATES:
            state = {
                ...state,
                chargeRates:[],
                payRates:[]
            };
            return state;
            case assignmentsActionTypes.ARS_SEARCH_PANEL_STATUS:
            state = {
                ...state,
                isARSSearch:data
            };
            return state;
        case assignmentsActionTypes.ASSIGN_RESOURCES_BUTTON_CLICK:
            state = {
                ...state,
                isAssignResourceClicked: data
            };
            return state;
        case assignmentsActionTypes.FETCH_TAXONOMY_BUSINESS_UNIT:
            state = {
                ...state,
                taxonomyBusinessUnit: data
            };
            return state;
        case assignmentsActionTypes.FETCH_NDT_TAXONOMY_SUB_CATEGORY:
            state = {
                ...state,
                ndtSubCategory: data
            };
            return state;
        case assignmentsActionTypes.FETCH_NDT_TAXONOMY_SERVICE:
            state = {
                ...state,
                ndtServices: data
            };
            return state;
        case assignmentsActionTypes.FETCH_TAXONOMY_URL:
        state = {
            ...state,
            taxonomyConstantURL: data
        };
        return state;
        default:
            return state;
    };
};