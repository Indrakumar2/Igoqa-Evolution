import { timesheetActionTypes } from '../../constants/actionTypes';

export const TimesheetNotesReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case timesheetActionTypes.ADD_TIMESHEET_NOTE:
           
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetNotes: (state.timesheetDetail.TimesheetNotes == null)?
                    [ ...[],data ]
                    :[
                        ...state.timesheetDetail.TimesheetNotes, data
                    ]
                },
                isbtnDisable: false,
            };
            return state;
            case timesheetActionTypes.FETCH_TIMESHEET_NOTES_SUCCESS:
                state = {
                    ...state,
                    timesheetDetail:{
                        ...state.timesheetDetail,
                        TimesheetNotes: data
                    }
                };
                return state;
            case timesheetActionTypes.EDIT_TIMESHEET_NOTE: //D661 issue8
                state = {
                    ...state,
                    timesheetDetail: {
                        ...state.timesheetDetail,
                        TimesheetNotes: data
                    },
                    isbtnDisable: false
                };
                return state;
        default:
            return state;
    }
};