import { timesheetActionTypes } from '../../constants/actionTypes';
import {
    isEmptyOrUndefine, 
    getlocalizeData,
    parseValdiationMessage 
} from '../../utils/commonUtils';
import { FetchData } from '../../services/api/baseApiService';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import {  timesheetAPIConfig } from '../../apiConfig/apiConfig';
import { StringFormat } from '../../utils/stringUtil';
import { ShowLoader,HideLoader } from '../../common/commonAction';

const localConstant = getlocalizeData();

const actions = {
    AddTimesheetNotes: (payload) => ({
        type: timesheetActionTypes.ADD_TIMESHEET_NOTE,
        data: payload
    }),
    FetchTimesheetNotes:(payload)=>({
        type: timesheetActionTypes.FETCH_TIMESHEET_NOTES_SUCCESS,
        data: payload 
    }),
    EditTimesheetNotes:(payload)=>({
        type: timesheetActionTypes.EDIT_TIMESHEET_NOTE,
        data: payload
    }),
};

export const AddTimesheetNotes = (data) => (dispatch) => {
    dispatch(actions.AddTimesheetNotes(data));
};

export const EditTimesheetNotes = (editedData) => (dispatch,getstate) => { //D661 issue8
    const state = getstate();
    const index =state.rootTimesheetReducer.timesheetDetail.TimesheetNotes.findIndex(iteratedValue => iteratedValue.timesheetNoteId === editedData.timesheetNoteId);
    const newState =Object.assign([],state.rootTimesheetReducer.timesheetDetail.TimesheetNotes);
    newState[index] = editedData;
    if (index >= 0) {
        dispatch(actions.EditTimesheetNotes(newState));
    }
};

export const FetchTimesheetNotes = (timesheetId) => async (dispatch, getstate) => {
    const state = getstate();
    if (!isEmptyOrUndefine(state.rootTimesheetReducer.timesheetDetail.TimesheetNotes)) {
        return;
    }
    dispatch(ShowLoader());
    const url = StringFormat(timesheetAPIConfig.notes,timesheetId);
    const response = await FetchData(url, {}).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster('Failed to retrive timesheet Notes', 'dangerToast Fetchnoteserror');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });

    if (response && response.code && response.code === "1") {
        dispatch(actions.FetchTimesheetNotes(response.result));
    }
    else if (response && response.code && (response.code === "11" || response.code === "41")) {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast conActDataSomethingWrong');
    }
    else {
        IntertekToaster('Failed to retrive timesheet Notes', 'dangerToast Fetchnoteserror');
    }
    dispatch(HideLoader());
};
