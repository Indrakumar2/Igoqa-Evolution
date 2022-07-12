import { timesheetActionTypes } from '../../constants/actionTypes';

export const TimesheetDocumentReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {
        case timesheetActionTypes.ADD_TIMESHEET_DOCUMENTS:
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetDocuments: (state.timesheetDetail.TimesheetDocuments === null)?
                    [ ...[],...data ]
                    :[ ...data, ...state.timesheetDetail.TimesheetDocuments ],
                },
                isbtnDisable: false        
            };
            return state;
        case timesheetActionTypes.UPDATE_TIMESHEET_DOCUMENTS_DETAILS:  
            state = {
                ...state,
                timesheetDetail: {
                    ...state.timesheetDetail,
                    TimesheetDocuments: data
                },
                isbtnDisable: false
            };
            return state;
        case timesheetActionTypes.DELETE_TIMESHEET_DOCUMENTS_DETAILS:
        state = {
            ...state,
            timesheetDetail: {
                ...state.timesheetDetail,
                TimesheetDocuments: data,
            },
            isbtnDisable: false
        };
        return state; 
        case timesheetActionTypes.FETCH_TIMESHEET_DOCUMENT_SUCCESS:
                state = {
                    ...state,
                    timesheetDetail:{
                        ...state.timesheetDetail,
                        TimesheetDocuments: data
                    }
                };
            return state;  
        case timesheetActionTypes.ADD_FILES_TO_BE_UPLOADED: //Add files to be uploaded async            
            state = {
                ...state,
                timesheetDetail:{
                    ...state.timesheetDetail,
                    fileToBeUploaded: data,
                }
            };
            return state;

        case timesheetActionTypes.CLEAR_FILES_TO_BE_UPLOADED: //clear files to be uploaded async            
            state = {
                ...state,
                timesheetDetail:{
                    ...state.timesheetDetail,
                    fileToBeUploaded: [],
                }
            };
            return state;   
        default:
            return state;
    }
};