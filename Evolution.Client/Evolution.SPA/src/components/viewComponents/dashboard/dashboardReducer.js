import {
    dashBoardActionTypes
} from '../../../constants/actionTypes';
import { AppDashBoardRoutes } from '../../../routes/routesConfig';
const initialState = {
    assignmentGridData: [],    
    inactiveAssignmentData: [],
    visitStatusAprovalGrid: [],
    timeSheetPendingAproval: [],
    contractsNearExpiry: [],
    documentAproval: '',
    selectedDocumentToApprove:{},
    documentType:[],
    selectedDocumentCustomer:{},
    contractsForDocumentApproval:[],
    projectForDocumentApproval:[],
    assignmentForDocumentApproval:[],
    supplierPOForDocumentApproval:[],
    timesheetForDocumentApproval:[],
    visitForDocumentApproval:[],
    budgetMonetary: [],
    budgetHours: [],
    budget:[],
    documentApproval: [],
    futureDays: 7,
    allCoOrdinator: false,
    budgetContractStatus:"1",
    showMyAssignmentsOnly: false,
    myTaskStatus: false,
    mySearchStatus:false, //added For home dashboard my task and my search count not refreshing
    //dashboardCount
    count: {
        // AssignmentCount: 0,
        // InactiveAssignmentCount: 0,
        // VisitCount: 0,
        // TimesheetCount: 0,
        // ContractCount: 0,
        // DocumentApprovalCount:0,
    },
    //this status let us know weather data is loaded/unloaded
    isActiveAssignmentsLoaded:false,
    isInactiveAssignmentLoaded:false,
    isVisitStatusAprovalsLoaded: false,
    isTimeSheetPendingAprovalLoaded: false,
    isDocumentAprovalLoaded:false,
    isBudgetLoaded:false,
    //technical Specialist Dashboard
    techSpecDashboardmessage:"",
};

export const dashboardReducer = (state = initialState, actions) => {
    const {
        type,
        data
    } = actions;
    switch (type) {
        case dashBoardActionTypes.FETCH_ASSIGNMENST_DATA:
            state = {
                ...state,
                assignmentGridData: data,
                isActiveAssignmentsLoaded: true
            };
            return state;
        case dashBoardActionTypes.FETCH_IN_ACTIVE_ASSIGNMENST_DATA:
            state = {
                ...state,
                inactiveAssignmentData:data,
                isInactiveAssignmentLoaded:true
            };
            return state;
        case dashBoardActionTypes.FETCH_VISIT_STATUS_APPROVAL:
            state = {
                ...state,
                visitStatusAprovalGrid: data,
                isVisitStatusAprovalsLoaded:true
            };
            return state;

        case dashBoardActionTypes.FETCH_TIMESHEET_PENDING_APROVAL:
            state = {
                ...state,
                timeSheetPendingAproval: data,
                isTimeSheetPendingAprovalLoaded:true
            };
            return state;
        case dashBoardActionTypes.FETCH_CONTRACTS_NEAR_EXPIRY:
            state = {
                ...state,
                contractsNearExpiry: data
            };
            return state;
        case dashBoardActionTypes.FETCH_DOCUMENT_APROVAL:
            state = {
                ...state,
                documentAproval: data,
                isDocumentAprovalLoaded:true
            };
            return state;
        case dashBoardActionTypes.SELECTED_DOCUMENT_TO_APPROVE:
            state = {
                ...state,
                selectedDocumentToApprove: data
            };
            return state;

        case dashBoardActionTypes.FETCH_BUDGET_MONETARY:
            state = {
                ...state,
                budgetMonetary: data
            };
            return state;
        case dashBoardActionTypes.FETCH_BUDGET_HOURS:
            state = {
                ...state,
                budgetHours: data
            };
            return state;
        case dashBoardActionTypes.FETCH_DOCUMENTATION_APPROVAL:
            state = {
                ...state,
                documentApproval: data
            };
            return state;
        case dashBoardActionTypes.BUDGET_PROPERTY_CHANGE:
            state = {
                ...state,
                budgetContractStatus:data.contractStatus,
                showMyAssignmentsOnly: data.showMyAssignmentsOnly,
            };
            return state;
        case dashBoardActionTypes.FETCH_BUDGET:
            state = {
                ...state,
                budget:data
            };
            return state;
        case dashBoardActionTypes.CLEAR_DASHBOARD_REDUCER:
            state = {
                ...state,
                assignmentGridData: [],    
                inactiveAssignmentData: [],
                visitStatusAprovalGrid: [],
                timeSheetPendingAproval: [],
                contractsNearExpiry: [],
                documentAproval: '',
                budgetMonetary: [],
                budgetHours: [],
                budget:[],
                documentApproval: [],
                futureDays: 7,
                allCoOrdinator: false,
                //dashboardCount
                count: {
                    AssignmentCount: 0,
                    InactiveAssignmentCount: 0,
                    VisitCount: 0,
                    TimesheetCount: 0,
                    ContractCount: 0,
                    DocumentApprovalCount:0
                },
                //this status let us know weather data is loaded/unloaded
                isActiveAssignmentsLoaded:false,
                isInactiveAssignmentLoaded:false,
                isVisitStatusAprovalsLoaded: false,
                isTimeSheetPendingAprovalLoaded: false,
                isDocumentAprovalLoaded:false 
            };
            return state;
        case dashBoardActionTypes.TOGGLE_ALL_COORDINATOR:
          //Logic to make currently selected tab Load status change
            state = {
                ...state,
                allCoOrdinator: data.allCoOrdinator,
                futureDays: data.futureDays,
                isActiveAssignmentsLoaded:false,
                inactiveAssignmentDataLoaded:false,
                isInactiveAssignmentLoaded:false,
                isVisitStatusAprovalsLoaded: false,
                isTimeSheetPendingAprovalLoaded: false,
                isDocumentAprovalLoaded:false,
                assignmentGridData:[],
                inactiveAssignmentData:[],
                visitStatusAprovalGrid: [],
                timeSheetPendingAproval: [],
                contractsNearExpiry:[],
                documentApproval:[]   
            };
            return state;
            case dashBoardActionTypes.FETCH_DASHBOARD_COUNT:
            state = {
                ...state,
                count: data
            };
            return state;
            case dashBoardActionTypes.FETCH_DOCUMENT_APPROVAL_TYPE:
            state={
                ...state,
                documentType:data
            };
            return state;
            case dashBoardActionTypes.FETCH_CUSTOMER_NAME:
            state={
                ...state,
                selectedDocumentCustomer:data
            };
            return state;
            case dashBoardActionTypes.FETCH_CONTRACT_FOR_DOC_APPROVAL:
            state={
                ...state,
                contractsForDocumentApproval:data
            };
            return state;
            case dashBoardActionTypes.FETCH_PROJECT_FOR_DOC_APPROVAL:
            state={
                ...state,
                projectForDocumentApproval:data
            };
            return state;
            case dashBoardActionTypes.FETCH_ASSIGNMENT_FOR_DOC_APPROVAL:
            state={
                ...state,
                assignmentForDocumentApproval:data
            };
            return state;
            case dashBoardActionTypes.FETCH_SUPPLIERPO_FOR_DOC_APPROVAL:
            state={
                ...state,
                supplierPOForDocumentApproval:data
            };
            return state;
            case dashBoardActionTypes.FETCH_TIMESHEET_FOR_DOC_APPROVAL:
            state={
                ...state,
                timesheetForDocumentApproval:data
            };
            return state;
            case dashBoardActionTypes.FETCH_VISIT_FOR_DOC_APPROVAL:
            state={
                ...state,
                visitForDocumentApproval:data
            };
            return state;
            case dashBoardActionTypes.GET_TECH_SPEC_DASHBOARD_MEASSAGE:
            state={
                ...state,
                techSpecDashboardmessage:data
            };
            return state;
            case dashBoardActionTypes.MYTASK_PROPERTY_CHANGE:
            state = {
                ...state,
                myTaskStatus:data.myTaskStatus,
            };
            return state;
            case dashBoardActionTypes.MYSEARCH_PROPERTY_CHANGE:
                state={
                    ...state,
                    mySearchStatus:data.mySearchStatus
                };
            return state;
        default:
            return state;
    }

};