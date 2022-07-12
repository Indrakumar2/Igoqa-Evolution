import { assignmentsActionTypes, sideMenu } from '../../constants/actionTypes';

export const AssignmentReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case assignmentsActionTypes.FETCH_ASSIGNMENT_PROJECT_INFO:  
            state = {
                ...state,
                assignmentDetail: data.assignmentData,
                dataToValidateAssignment: data.assignmentValidationData,
                isbtnDisable: true
            };
            return state;
        case assignmentsActionTypes.FETCH_ASSIGNMENT_PROJECT_DATE_INFO_EDITMODE:
                state = {
                    ...state,
                    dataToValidateAssignment: data,
                   // isbtnDisable:true
                };
                return state;
        case assignmentsActionTypes.FETCH_ASSIGNMENT_SEARCH_SUCCESS:
            state = {
                ...state,
                assignmentList: data
            };
            return state;
        case assignmentsActionTypes.FETCH_ASSIGNMENT_SEARCH_ERROR:
            state = {
                ...state,
                assignmentListError: data
            };
            return state;
        case assignmentsActionTypes.FETCH_ASSIGNMENT_STATUS_SUCCESS:
            state = {
                ...state,
                assignmentStatus: data
            };
            return state;
        case assignmentsActionTypes.FETCH_ASSIGNMENT_STATUS_ERROR:
            state = {
                ...state,
                assignmentStatusError: data
            };
            return state;
        case assignmentsActionTypes.FETCH_ASSIGNMENT_TYPE:
            state = {
                ...state,
                assignmentType: data
            };
            return state;
        case assignmentsActionTypes.FETCH_ASSIGNMENT_LIFE_CYCLE:
            state = {
                ...state,
                assignmentLifeCycle: data
            };
            return state;
        case assignmentsActionTypes.SAVE_SELECTED_ASSIGNMENT_Id:
            state = {
                ...state,
                selectedAssignmentId: data
            };
            return state;
        case assignmentsActionTypes.FETCH_ASSIGNMENT_DETAIL_SUCCESS:
            state = {
                ...state,
                assignmentDetail: data,
                isbtnDisable: true
            };
            return state;
        case assignmentsActionTypes.CLEAR_ASSIGNMENT_SEARCH_RESULTS:
            state = {
                ...state,
                assignmentList: []
            };
            return state;
        case assignmentsActionTypes.CLEAR_GRID_FORM_DATAS:
            state = {
                ...state,
                assignmentList: []
            };
            return state;
        case assignmentsActionTypes.CLEAR_ASSIGNMENT_DETAILS:
                state = {
                    ...state,
                    assignmentDetail:{}
                };
            return state;
        case assignmentsActionTypes.FETCH_VISIT_STATUS:
            state = {
                ...state,
                visitStatus: data
            };
            return state;
        case sideMenu.HANDLE_MENU_ACTION:
            state = {
                ...state,
                //assignmentDetail:{},
                isARSSearch:false,
                preAssignmentIds:[],
                selectedPreAssignment:{},
                isResourceMatched:true,
                isbtnDisable:true,
                assignmentList: []
            };
            return state;
        case assignmentsActionTypes.SAVE_SELECTED_ASSIGNMENT_ID:
            state = {
                ...state,
                selectedAssignmentNumber: data.assignmentNumber,
                selectedAssignmentId: data.assignmentId
            };
            return state;
        // D - 631
        case assignmentsActionTypes.UPDATE_COPY_ASSIGNMENT_INITIAL_STATE:
            state = {
                ...state,
                copyAssignmentInitialState: data
            };
            return state;
        case assignmentsActionTypes.CLEAR_TAXONOMY_OLD_VALUES:
            state = {
                ...state,
                AssignmentTaxonomyold:[],
                AssignmentLifeCycleOld:"",
                AssignmentSupplierPOOld:""
            };
            return state;
        case assignmentsActionTypes.FETCH_REVIEW_AND_MODERATION_PROCESS:
            state = {
                ...state,
                reviewAndModerationProcess: data
            };
            return state;
        case assignmentsActionTypes.ASSIGNMENT_SAVE_BUTTON_DISABLE:
            state = {
                ...state,
                isbtnDisable: data
            };
            return state;
        case assignmentsActionTypes.CLEAR_ASSIGNMENT_MASTER_DATA:
            state = {
                ...state,
                assignmentStatus:[],
                assignmentType: [],
                assignmentLifeCycle:[],
                reviewAndModerationProcess:[]
            };
            return state;
        case assignmentsActionTypes.SELECTED_TECH_SPEC:
            state = {
                ...state,
                selectedTechSpec: data
            };
            return state;
        case assignmentsActionTypes.IS_INTERNAL_ASSIGNMENT:
            state = {
                ...state,
                isInternalAssignment: data
            };
            return state;
        default: return state;
    }
};