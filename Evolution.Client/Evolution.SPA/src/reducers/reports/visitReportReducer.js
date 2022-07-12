import { reportActionTypes } from '../../constants/actionTypes';

export const visitReports = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case reportActionTypes.REPORTS_FETCH_CUSTOMER:
            state = {
                ...state,
                customerList: data
            };
            return state;
        case reportActionTypes.REPORTS_FETCH_CUSTOMERCONTRACTS:
            state = {
                ...state,
                customerContracts: data
            };
            return state;
        case reportActionTypes.REPORTS_FETCH_APPROVEDUNAPPROVEDCUSTOMERCONTRACTS:
            state = {
                ...state,
                customerContracts: data
            };
            return state;
        case reportActionTypes.REPORTS_FETCH_KPICONTRACTS:
            state = {
                ...state,
                customerContracts: data
            };
            return state;
        case reportActionTypes.REPORTS_FETCH_UNAPPROVED_CONTRACTS:
            state = {
                ...state,
                customerContracts: data
            };
            return state;
        case reportActionTypes.REPORTS_FETCH_UNAPPROVEDVISIT:
            state = {
                ...state,
                coordinatorCustomerList: data
            };
            return state;
        case reportActionTypes.REPORTS_FETCH_APPROVEDUNAPPROVEDVISIT:
            state = {
                ...state,
                customerList: data
            };
            return state;
        case reportActionTypes.REPORTS_KPI_CUSTOMER:
            state = {
                ...state,
                customerList: data
            };
            return state;
        case reportActionTypes.REPORT_FETCH_CONTRACTPROJECTS:
            state = {
                ...state,
                contractProjects: data
            };
            return state;
        case reportActionTypes.FETCH_CUSTOMERS_BASED_ON_COORDINATORS:
            state = {
                ...state,
                coordinatorCustomerList: data
            };
            return state;
        case reportActionTypes.CLEAR_CUSTOMER:
            state = {
                ...state,
                coordinatorCustomerList: [],
                customerContracts: [],
                contractProjects: []
            };
            return state;
        case reportActionTypes.CLEAR_CUSTOMERCONTRACTS:
            state = {
                ...state,
                customerContracts: [],
                contractProjects: []
            };
            return state;
        case reportActionTypes.CLEAR_CONTRACTPROJECTS:
            state = {
                ...state,
                contractProjects: []
            };
            return state;
        case reportActionTypes.CLEAR_CONTRACTPROJECTS:
            state = {
                ...state,
                serverReportData: []
            };
            return state;    
        case reportActionTypes.FETCH_SERVER_REPORT_DATA:
            state = {
                ...state,
                serverReportData: data
            };
            return state;
            case reportActionTypes.DOWNLOAD_EXPORT_CV_AS_ZIP:
                state = {
                    ...state,
                    serverReportData: data
                };
                return state;
        default:
            return state;
    }
};