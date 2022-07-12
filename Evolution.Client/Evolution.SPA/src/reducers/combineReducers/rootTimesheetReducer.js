import reduceReducers from 'reduce-reducers';
import { TimesheetReducer } from './../timesheet/timesheetReducer';
import { TimesheetDocumentReducer } from './../timesheet/timesheetDocumentReducer';
import { TimesheetGeneralDetailsReducer } from './../timesheet/timesheetGeneralDetailsReducer';
import { TimesheetNotesReducer } from './../timesheet/timesheetNoteReducer';
import { TimesheetReferenceReducer } from './../timesheet/timesheetReferenceReducer';
import { TimesheetTechSpecsAccountsReducer } from '../timesheet/timesheetTechSpecsAccountsReducer';

const initialState = {
    timesheetDetail:{
    },
    timesheetList:[],
    timesheetStatus:[],
    dataToValidateTimesheet: {},
    selectedTimesheetId: null,    
    isbtnDisable:true,
    techSpecRateSchedules:[],
    companyBusinessUnitExpectedMargin:null,
    isShowAllRates: false,
    timesheetTechnicalSpecialistsGrossMargin:null,
    timesheetSelectedTechSpecs:[],
    assignmentUnLinkedExpenses:[],
    isExpenseOpen: true,
    timesheetExchangeRates: []
};
export default reduceReducers(
    TimesheetReducer,
    TimesheetGeneralDetailsReducer,
    TimesheetNotesReducer,
    TimesheetReferenceReducer,
    TimesheetTechSpecsAccountsReducer,
    TimesheetDocumentReducer,
    initialState
);