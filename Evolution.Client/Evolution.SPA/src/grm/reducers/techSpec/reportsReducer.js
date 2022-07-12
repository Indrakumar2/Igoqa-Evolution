import { techSpecActionTypes } from '../../constants/actionTypes';
export const ReportsReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {

        case techSpecActionTypes.FETCH_CALENDAR_SCHEDULES_DATA:
            state={
                ...state,
                calendarSchedulesData: data
            };
            return state;

        case techSpecActionTypes.FETCH_COMPANY_SPECIFIC_MATRIX_DATA:
            state={
                ...state,
                companySpecificMatrixReportData:data
            };
            return state;
        case techSpecActionTypes.HANDLE_MENU_ACTION:
            state={
                ...state,
                companySpecificMatrixReportData:{},
                calendarSchedulesData:{},
                taxonomyReportData:{},
            };
            return state;
        case techSpecActionTypes.FETCH_TAXONOMY_REPORT:
            state={
                ...state,
                taxonomyReportData:data
            };
            return state;
        case techSpecActionTypes.FETCH_TS_BASED_ON_COMPANY:
                state={
                    ...state,
                    companyBasedTSData:data
                };
                return state;
        case techSpecActionTypes.CLEAR_SEARCH_DATA:
            state={
                ...state,
                companySpecificMatrixReportData:[],
                calendarSchedulesData:[],
                taxonomyReportData:[],
            };
            return state;
        case techSpecActionTypes.FETCH_CONTRACT_HOLDING_COORDINATOR:
                state = {
                    ...state,
                    coordinators:data
                };
                return state;
        default:
            return state;
    }
};