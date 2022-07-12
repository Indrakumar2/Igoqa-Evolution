import { timesheetActionTypes } from '../../constants/actionTypes';
export const TimesheetGeneralDetailsReducer = (state, action) => {
    const { type, data } = action;

    switch (type) {
        case timesheetActionTypes.UPDATE_TIMESHEET_DETAILS: {
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetInfo: data,
                },
                isbtnDisable: false
            };
            return state;
        }
        case timesheetActionTypes.SELECTED_TIMESHEET_TECHNICAL_SPECIALISTS:
            state = {
                ...state,
                timesheetSelectedTechSpecs: data
            };
            return state;
        case timesheetActionTypes.TIMESHEET_TECHNICAL_SPECIALIST_ADD:
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetTechnicalSpecialists: state.timesheetDetail.TimesheetTechnicalSpecialists == null
                        ? [ ...[], data ]
                        : [ ...state.timesheetDetail.TimesheetTechnicalSpecialists, data ]
                },
                isbtnDisable: false
            };
            return state;
        case timesheetActionTypes.TIMESHEET_TECHNICAL_SPECIALIST_UPDATE:
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetTechnicalSpecialists: data
                },
                isbtnDisable: false
            };
            return state;
        case timesheetActionTypes.TIMESHEET_TECHNICAL_SPECIALIST_REMOVE:
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetTechnicalSpecialists: data
                },
                isbtnDisable: false
            };
            return state;
        case timesheetActionTypes.FETCH_TIMESHEET_GENERAL_DETAIL_SUCCESS:
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetInfo: data
                }
            };
            return state;
        case timesheetActionTypes.ADD_CALENDAR_DATA:
            state = {
                ...state,
                timesheetCalendarData: state.timesheetCalendarData ? [ ...state.timesheetCalendarData, data ] : [ ...[], data ],
                isbtnDisable: false
            };
            return state;
        case timesheetActionTypes.UPDATE_CALENDAR_DATA:
            state = {
                ...state,
                timesheetCalendarData: data,
                isbtnDisable: false
            };
            return state;

        case timesheetActionTypes.REMOVE_TS_CALENDAR_DATA:
            state = {
                ...state,
                timesheetCalendarData: state.timesheetCalendarData ? state.timesheetCalendarData.filter(a => a.technicalSpecialistId !== data) : [],
                isbtnDisable: false
            };
            return state;

        default: return state;
    }
};