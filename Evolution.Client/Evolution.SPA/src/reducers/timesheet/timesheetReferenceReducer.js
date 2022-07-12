import { timesheetActionTypes } from '../../constants/actionTypes';
import { updateObjectInArray } from '../../utils/arrayUtil';

export const TimesheetReferenceReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case timesheetActionTypes.ADD_TIMESHEET_REFERENCE:
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetReferences:state.timesheetDetail.TimesheetReferences==null
                    ?[ ...[], data ]
                    : [ ...state.timesheetDetail.TimesheetReferences, data ]
                },
                isbtnDisable: action.isDefaultReferences === true?true:false
            };
            return state;

        case timesheetActionTypes.DELETE_TIMESHEET_REFERENCE:
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetReferences: data
                },
                isbtnDisable: false
            };
            return state;
        case timesheetActionTypes.UPDATE_TIMESHEET_REFERENCE:
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetReferences: updateObjectInArray(state.timesheetDetail.TimesheetReferences, data)
                },
                isbtnDisable: false
            };
            return state;
        case timesheetActionTypes.FETCH_TIMESHEET_REFERENCE_TYPES:
            state = {
                ...state,
                timesheetReferenceTypes: data
            };
            return state;
        case timesheetActionTypes.FETCH_TIMESHEET_REFERENCES_SUCCESS:
        state = {
            ...state,
            timesheetDetail:{
                ...state.timesheetDetail,
                TimesheetReferences: data
            }
        };
        return state;
        default:
            return state;
    }
};