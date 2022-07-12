import { timesheetActionTypes,sideMenu } from '../../constants/actionTypes';
import { getNestedObject } from  '../../utils/commonUtils';
export const TimesheetReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case timesheetActionTypes.FETCH_TIMESHEET_SEARCH_SUCCESS:
        case timesheetActionTypes.FETCH_ASSIGNMENT_TIMESHEETS:        
            state = {
                ...state,
                timesheetList:data                               
            };
            return state;
        case timesheetActionTypes.SELECTED_TIMESHEET_STATUS:
            state={
                ...state,
                selectedTimesheetStatus:data.selectedValue
            };
            return state;
        case timesheetActionTypes.FETCH_UNUSED_REASON:
        state = {
            ...state,
            unusedReason: data
        };
        return state;
        case timesheetActionTypes.FETCH_ASSIGNMENT_TO_ADD_TIMESHEET:
        state = {
            ...state,
            timesheetDetail:data,
            isbtnDisable:true
        };  
        return state;  
        case timesheetActionTypes.CLEAR_TIMESHEET_SEARCH_RESULTS:
        state = {
            ...state,
            timesheetList:[]      
        };  
        return state;
        case timesheetActionTypes.CLEAR_TIMESHEET_DETAILS:
            state = {
                ...state,
                timesheetDetail:{}    
            };  
            return state;
        case timesheetActionTypes.FETCH_TIMESHEET_DETAIL_SUCCESS:
            const timesheetTechnicalSpecialists = (data.TimesheetTechnicalSpecialists && data.TimesheetTechnicalSpecialists.timesheetTechnicalSpecialists)?
            data.TimesheetTechnicalSpecialists.timesheetTechnicalSpecialists:[];
            const grossMargin = getNestedObject(data, [ 'TimesheetTechnicalSpecialists', 'timesheetAccountGrossMargin' ]);
            const roundedGrossMargin = grossMargin ? grossMargin.toFixed(2) : null;
        state = {
            ...state,
            timesheetDetail: {
                ...data,
                TimesheetTechnicalSpecialists:timesheetTechnicalSpecialists
            },
            selectedTimesheetStatus: (data.TimesheetInfo && data.TimesheetInfo.timesheetStatus ? data.TimesheetInfo.timesheetStatus : ''),
            timesheetTechnicalSpecialistsGrossMargin: roundedGrossMargin,
            isbtnDisable: true,
            isShowAllRates: false
        };  
        return state;
        case sideMenu.HANDLE_MENU_ACTION:
        state = {
            ...state,
            //timesheetDetail:{},
            selectedTimesheet:{},
            selectedTimesheetId:null,
            isbtnDisable:true,
            timesheetList:[],
            timesheetTechnicalSpecialistsGrossMargin:null,
            timesheetSelectedTechSpecs:[],
            companyBusinessUnitExpectedMargin:null,
            assignmentUnLinkedExpenses:[]
        };
        return state;
        case timesheetActionTypes.SELECTED_TIMESHEET:
            state={
                ...state,
                selectedTimesheetId:data.timesheetId,
                selectedTimesheet:data
            };
            return state;
        case timesheetActionTypes.UPDATE_TIMESHEET_STATUS:
            state={
                ...state,
                timesheetDetail:{
                    ...state.timesheetDetail,
                    TimesheetInfo:{
                         ...state.timesheetDetail.TimesheetInfo,
                         timesheetStatus: data
                        }
                },
                isbtnDisable:true
            };
            return state;
         case  timesheetActionTypes.CREATE_NEW_TIMESHEET:
         state={
            ...state,
            timesheetDetail: data,
            isbtnDisable: true,
            timesheetTechnicalSpecialistsGrossMargin:null,
            selectedTimesheet: {},
            selectedTimesheetId: null,
            timesheetSelectedTechSpecs:[],
            companyBusinessUnitExpectedMargin:null,
            assignmentUnLinkedExpenses:[]
        };
        return state;
        case timesheetActionTypes.UPDATE_SHOW_ALL_RATES:
            state = {
                ...state,
                isShowAllRates: data
            };
            return state;
        case timesheetActionTypes.CUSTOMER_REPORTING_NOTIFICATION_CONTANT:
        state = {
            ...state,
            customerReportingNotificationContant: data
        };
        return state;
        case timesheetActionTypes.FETCH_TIMESHEET_VALIDATION_DATA:
            state = {
                ...state,
                timesheetValidationData: data
            };
            return state;
        case timesheetActionTypes.CLEAR_CALENDAR_DATA:
            state = {
                ...state,
                timesheetCalendarData: []
            };
            return state;
        case timesheetActionTypes.ERROR_FETCHING_TIMESHEET_CONTRACT_SCHEDULES:
                state = {
                       ...state,
                       contractScheduleCurrency :data
                   };
                   return state;
        case timesheetActionTypes.TIMESHEET_BUTTON_DISABLE:
            state = {
                    ...state,
                    isbtnDisable: true,
                };
                return state;
        case timesheetActionTypes.TIMESHEET_VALID_CALENDAR_DATA_SAVE:
            state = {
                ...state,
                validCalendarData: data,
            };
            return state;
        case timesheetActionTypes.UPDATE_TIMESHEET_EXCHANGE_RATES:
            state = {
                ...state,
                timesheetExchangeRates: data,
            };
            return state;
        case timesheetActionTypes.FETCH_TIMESHEET_STATUS:
            state = {
                ...state,
                timesheetStatus: data
            };
            return state;
        default:
            return state;
    }
};