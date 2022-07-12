import { timesheetActionTypes } from '../../constants/actionTypes';
import { 
    isEmptyOrUndefine,
    isEmptyReturnDefault,
    deepCopy,
    getlocalizeData,
    parseValdiationMessage,
    getNestedObject,
    checkNested 
} from '../../utils/commonUtils';
import { FetchData } from '../../services/api/baseApiService';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import {  timesheetAPIConfig } from '../../apiConfig/apiConfig';
import { StringFormat } from '../../utils/stringUtil';
import { ShowLoader,HideLoader } from '../../common/commonAction';

const localConstant = getlocalizeData();
const actions = {
    UpdateTimesheetDetails: (payload) => ({
        type: timesheetActionTypes.UPDATE_TIMESHEET_DETAILS,
        data: payload
    }),
    SelectedTimesheetTechSpecs: (payload) =>({
        type:timesheetActionTypes.SELECTED_TIMESHEET_TECHNICAL_SPECIALISTS,
        data: payload
    }),
    AddTimesheetTechnicalSpecialist: (payload) => ({
        type: timesheetActionTypes.TIMESHEET_TECHNICAL_SPECIALIST_ADD,
        data: payload
    }),
    RemoveTimesheetTechnicalSpecialist: (payload) => ({
        type: timesheetActionTypes.TIMESHEET_TECHNICAL_SPECIALIST_REMOVE,
        data: payload
    }),
    FetchTimesheetGeneralDetail:(payload)=>({
        type: timesheetActionTypes.FETCH_TIMESHEET_GENERAL_DETAIL_SUCCESS,
        data: payload 
    }),

     //To add visit calendar data to store
     AddTimesheetCalendarData: (payload) => ({
        type: timesheetActionTypes.ADD_CALENDAR_DATA,
        data: payload
    }),
    //To update visit calendar data to store
    UpdateTimesheetCalendarData: (payload) => ({
        type: timesheetActionTypes.UPDATE_CALENDAR_DATA,
        data: payload
    }),    
    DeleteTimesheetTechnicalSpecialistTime: (payload) => ({
        type: timesheetActionTypes.DELETE_TIMESHEET_TECHNICAL_SPECIALIST_TIME,
        data: payload
    }),
    DeleteTimesheetTechnicalSpecialistExpense: (payload) => ({
        type: timesheetActionTypes.DELETE_TIMESHEET_TECHNICAL_SPECIALIST_EXPENSE,
        data: payload
    }),
    DeleteTimesheetTechnicalSpecialistTravel: (payload) => ({
        type: timesheetActionTypes.DELETE_TIMESHEET_TECHNICAL_SPECIALIST_TRAVEL,
        data: payload
    }),    
    DeleteTimesheetTechnicalSpecialistConsumable: (payload) => ({
        type: timesheetActionTypes.DELETE_TIMESHEET_TECHNICAL_SPECIALIST_CONSUMABLE,
        data: payload
    }),
    RemoveTSTimesheetCalendarData: (payload) => ({
        type: timesheetActionTypes.REMOVE_TS_CALENDAR_DATA,
        data: payload
    }),
    UpdateTimesheetTechnicalSpecialist: (payload) => ({
        type: timesheetActionTypes.TIMESHEET_TECHNICAL_SPECIALIST_UPDATE,
        data: payload
    }),
    UpdateTimesheetTechnicalSpecialistTime: (payload) => ({
        type: timesheetActionTypes.UPDATE_TIMESHEET_TECHNICAL_SPECIALIST_TIME,
        data: payload
    }),
    UpdateTimesheetTechnicalSpecialistTravel: (payload) => ({
        type: timesheetActionTypes.UPDATE_TIMESHEET_TECHNICAL_SPECIALIST_TRAVEL,
        data: payload
    }),
    UpdateTimesheetTechnicalSpecialistExpense: (payload) => ({
        type: timesheetActionTypes.UPDATE_TIMESHEET_TECHNICAL_SPECIALIST_EXPENSE,
        data: payload
    }),
    UpdateTimesheetTechnicalSpecialistConsumable: (payload) => ({
        type: timesheetActionTypes.UPDATE_TIMESHEET_TECHNICAL_SPECIALIST_CONSUMABLE,
        data: payload
    })
};

/** General Details data storing in store */
export const UpdateTimesheetDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const timesheetInfo = state.rootTimesheetReducer.timesheetDetail.TimesheetInfo;
    const updatedData = Object.assign({}, timesheetInfo, data);
    dispatch(actions.UpdateTimesheetDetails(updatedData));
};

export const SelectedTimesheetTechSpecs = (selectedTechSpecs)=>async (dispatch, getstate)=>{
    dispatch(actions.SelectedTimesheetTechSpecs(selectedTechSpecs));
};

export const AddTimesheetTechnicalSpecialist = (techSpec) => async (dispatch, getstate) => {
    const state = getstate();
    const timesheetTechnicalSpecialists = isEmptyReturnDefault((state.rootTimesheetReducer.timesheetDetail || {}).TimesheetTechnicalSpecialists);
    const isTSExists = timesheetTechnicalSpecialists.findIndex(ts => ts.pin === techSpec.pin);
    isTSExists === -1 && dispatch(actions.AddTimesheetTechnicalSpecialist(techSpec));
};

export const UpdateTechSpecLineItmes = (ePin) => async (dispatch, getstate) => {    
        const state = getstate();
        const newStateTime = Object.assign([], state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistTimes);
        if(newStateTime && newStateTime.length > 0) {
            for (let i = 0; i < newStateTime.length; i++) {
                if (parseInt(newStateTime[i].pin) === parseInt(ePin)) {
                    newStateTime[i].recordStatus = "M";
                }
            }
        }
        dispatch(actions.UpdateTimesheetTechnicalSpecialistTime(newStateTime));
        
        const newStateExpense = Object.assign([], state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistExpenses);
        if(newStateExpense && newStateExpense.length > 0) {
            for (let i = 0; i < newStateExpense.length; i++) {
                if (parseInt(newStateExpense[i].pin) === parseInt(ePin)) {
                    newStateExpense[i].recordStatus = "M";
                }
            }
        }
        dispatch(actions.UpdateTimesheetTechnicalSpecialistExpense(newStateExpense));
        
        const newStateTravel = Object.assign([], state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistTravels);
        if(newStateTravel && newStateTravel.length > 0) {
            for (let i = 0; i < newStateTravel.length; i++) {
                if (parseInt(newStateTravel[i].pin) === parseInt(ePin)) {
                    newStateTravel[i].recordStatus = "M";
                }
            }
        }
        dispatch(actions.UpdateTimesheetTechnicalSpecialistTravel(newStateTravel));
        
        const newStateConsumable = Object.assign([], state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistConsumables);
        if(newStateConsumable && newStateConsumable.length > 0) {
            for (let i = 0; i < newStateConsumable.length; i++) {
                if (parseInt(newStateConsumable[i].pin) === parseInt(ePin)) {
                    newStateConsumable[i].recordStatus = "M";
                }
            }
        }
        dispatch(actions.UpdateTimesheetTechnicalSpecialistConsumable(newStateConsumable));
};

export const UpdateTimesheetTechnicalSpecialist = (techSpec, ePin) => async (dispatch) => {
    dispatch(actions.UpdateTimesheetTechnicalSpecialist(techSpec, ePin));
    dispatch(UpdateTechSpecLineItmes(ePin));
};

export const RemoveTimesheetTechnicalSpecialist = (techSpec) => async (dispatch, getstate) => {
    const timesheetTechnicalSpecialists = deepCopy(getstate().rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialists);
    
    if (timesheetTechnicalSpecialists && timesheetTechnicalSpecialists.length > 0) {
        for (let i = 0; i < timesheetTechnicalSpecialists.length; i++) {
            if (timesheetTechnicalSpecialists[i].pin === techSpec.value) {
                if(timesheetTechnicalSpecialists[i].recordStatus === "N"){
                    timesheetTechnicalSpecialists.splice(i, 1);
                    i--;
                }
                else{
                    timesheetTechnicalSpecialists[i].recordStatus = "D";
                }
            }
        }
    }
    //Mark ALL the lineitems Assocoated to ts to D if New Delete    
    dispatch(actions.RemoveTimesheetTechnicalSpecialist(timesheetTechnicalSpecialists));
    dispatch(DeleteTimesheetTechnicalSpecialistTime(techSpec.value));
    dispatch(DeleteTimesheetTechnicalSpecialistExpense(techSpec.value));
    dispatch(DeleteTimesheetTechnicalSpecialistTravel(techSpec.value));
    dispatch(DeleteTimesheetTechnicalSpecialistConsumable(techSpec.value));
};

export const DeleteTimesheetTechnicalSpecialistTime = (pin) => async(dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistTimes);
    if(newState && newState.length > 0) {
        for (let i = 0; i < newState.length; i++) {
            if (parseInt(newState[i].pin) === parseInt(pin)) {
                if(newState[i].recordStatus === "N"){
                    newState.splice(i, 1);
                    i--;
                } else {
                    newState[i].recordStatus = "D";
                }
            }
        }
    }
    dispatch(actions.DeleteTimesheetTechnicalSpecialistTime(newState));
};

export const DeleteTimesheetTechnicalSpecialistExpense = (pin) => async(dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistExpenses);
    if(newState && newState.length > 0) {
        for (let i = 0; i < newState.length; i++) {
            if (parseInt(newState[i].pin) === parseInt(pin)) {
                if(newState[i].recordStatus === "N"){
                    newState.splice(i, 1);
                    i--;
                } else {
                    newState[i].recordStatus = "D";
                }
            }
        }
    }
    dispatch(actions.DeleteTimesheetTechnicalSpecialistExpense(newState));
};

export const DeleteTimesheetTechnicalSpecialistTravel = (pin) => async(dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistTravels);
    if(newState && newState.length > 0) {
        for (let i = 0; i < newState.length; i++) {
            if (parseInt(newState[i].pin) === parseInt(pin)) {
                if(newState[i].recordStatus === "N"){
                    newState.splice(i, 1);
                    i--;
                } else {
                    newState[i].recordStatus = "D";
                }
            }
        }
    }
    dispatch(actions.DeleteTimesheetTechnicalSpecialistTravel(newState));
};

export const DeleteTimesheetTechnicalSpecialistConsumable = (pin) => async(dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistConsumables);
    if(newState && newState.length > 0) {
        for (let i = 0; i < newState.length; i++) {
            if (parseInt(newState[i].pin) === parseInt(pin)) {
                if(newState[i].recordStatus === "N"){
                    newState.splice(i, 1);
                    i--;
                } else {
                    newState[i].recordStatus = "D";
                }
            }
        }
    }
    dispatch(actions.DeleteTimesheetTechnicalSpecialistConsumable(newState));
};

export const FetchTimesheetGeneralDetail = (timesheetId,isRefresh) => async (dispatch, getstate) => {
    const state = getstate();
    if (!isRefresh && !isEmptyOrUndefine(state.rootTimesheetReducer.timesheetDetail.TimesheetInfo)) {
        return;
    }
    
    if (!timesheetId) {
        timesheetId = state.rootTimesheetReducer.timesheetDetail.TimesheetInfo ?
            state.rootTimesheetReducer.timesheetDetail.TimesheetInfo.timesheetId :
            state.rootTimesheetReducer.selectedTimesheetId;
    }
    if (!timesheetId) {
        return false;
    }
    dispatch(ShowLoader());
    const url = StringFormat(timesheetAPIConfig.timesheetInfo, timesheetId);

    const response = await FetchData(url, {}).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TIMESHEET_DETAIL, 'dangerToast FetchAssignmentSearchResultsErr');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });

    if (response && response.code && response.code === "1") {
        const timesheetInfo = Array.isArray(response.result) && response.result.length > 0 ? response.result[0] : {};
        dispatch(actions.FetchTimesheetGeneralDetail(timesheetInfo));
        const techSpecs = timesheetInfo.techSpecialists && timesheetInfo.techSpecialists.map(eachItem => {
            return { value: eachItem.pin, label: eachItem.fullName };
        });
        dispatch(actions.SelectedTimesheetTechSpecs(techSpecs));
    }
    else if (response && response.code && (response.code === "11" || response.code === "41")) {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast conActDataSomethingWrong');
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TIMESHEET_DETAIL, 'dangerToast FetchAssignmentSearchResultsErr');
    }
    dispatch(HideLoader());
};

//To add visit calendar data to store
export const AddTimesheetCalendarData = (calendarData, startDate, endDate) => (dispatch, getstate) => {
    const data = {
        "id": calendarData.id,
        "technicalSpecialistId": calendarData.resourceId,
        "companyId": calendarData.companyId,
        "companyCode": calendarData.companyCode,
        "calendarType": localConstant.globalCalendar.CALENDAR_STATUS.TIMESHEET,
        "calendarRefCode": calendarData.calendarRefCode,
        "calendarStatus": calendarData.calendarStatus,
        "startDateTime": startDate,
        "endDateTime": endDate,
        "createdBy": calendarData.createdBy,
        "isActive": calendarData.isActive,
        "recordStatus": calendarData.recordStatus,
        "updateCount": calendarData.updateCount,
        "isActive": calendarData.resourceAllocation ? !calendarData.resourceAllocation : true,
        "start":calendarData.start,
        "end":calendarData.end,
        "description":calendarData.description
    };
    dispatch(actions.AddTimesheetCalendarData(data));
};

//To update Timesheet calendar data to store
export const UpdateTimesheetCalendarData = (calendarData) => (dispatch, getstate) => {
    dispatch(actions.UpdateTimesheetCalendarData(calendarData));
};

export const FetchTimesheetCalendarDetail = (timesheetId) => async (dispatch, getstate) => {

    dispatch(ShowLoader());
    const url = StringFormat(timesheetAPIConfig.timesheetInfo, timesheetId);

    const response = await FetchData(url, {}).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TIMESHEET_DETAIL, 'dangerToast FetchAssignmentSearchResultsErr');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });

    if (response && response.code && response.code === "1") {
        dispatch(HideLoader());
        return response != null ? response.result[0] : null;
    }
    else if (response && response.code && (response.code === "11" || response.code === "41")) {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast conActDataSomethingWrong');
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TIMESHEET_DETAIL, 'dangerToast FetchAssignmentSearchResultsErr');
    }
    dispatch(HideLoader());
};

export const RemoveTSTimesheetCalendarData = (techspecId) => async (dispatch, getstate) => {
    dispatch(actions.RemoveTSTimesheetCalendarData(techspecId));
};