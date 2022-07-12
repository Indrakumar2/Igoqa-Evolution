import { techSpecActionTypes } from '../../constants/actionTypes';

export const GlobalCalendarReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {

        case techSpecActionTypes.techSpecSearch.FETCH_GLOBAL_CALENDAR_DATA:
            state = {
                ...state,
                calendarData: data
            };
            return state;

        case techSpecActionTypes.techSpecSearch.FETCH_GLOBAL_CALENDAR_VISIT_DATA:
            state = {
                ...state,
                calendarVisitData: data
            };
            return state;

        case techSpecActionTypes.techSpecSearch.FETCH_GLOBAL_CALENDAR_TIMESHEET_DATA:
            state = {
                ...state,
                calendarTimeSheetData: data
            };
            return state;

        case techSpecActionTypes.techSpecSearch.FETCH_GLOBAL_CALENDAR_PREASSIGNMENT_DATA:
            state = {
                ...state,
                calendarPreAssignmentData: data
            };
            return state;

        case techSpecActionTypes.techSpecSearch.CLEAR_GLOBAL_CALENDAR_DATA:
            state = {
                ...state,
                calendarData: {
                    resources: [],
                    events: []
                }
            };
            return state;

        default:
            return state;
    }
};