import { timesheetActionTypes } from '../../constants/actionTypes';
import { FetchData } from '../../services/api/baseApiService';
import { 
    assignmentAPIConfig,
    RequestPayload,
    customerAPIConfig,
    timesheetAPIConfig } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import {
    isEmptyOrUndefine, 
    getlocalizeData,
    parseValdiationMessage
} from '../../utils/commonUtils';
import { StringFormat } from '../../utils/stringUtil';
import { ShowLoader,HideLoader } from '../../common/commonAction';
import moment from 'moment';

const localConstant = getlocalizeData();
const actions = {
    AddTimesheetReference: (payload,isDefaultReferences) => ({
        type: timesheetActionTypes.ADD_TIMESHEET_REFERENCE,
        data: payload,
        isDefaultReferences:isDefaultReferences
    }),
    DeleteTimesheetReference: (payload) => ({
        type: timesheetActionTypes.DELETE_TIMESHEET_REFERENCE,
        data: payload
    }),
    UpdatetTimesheetReference: (payload) => ({
        type: timesheetActionTypes.UPDATE_TIMESHEET_REFERENCE,
        data: payload
    }),
    FetchReferencetypes: (payload) => ({
        type: timesheetActionTypes.FETCH_TIMESHEET_REFERENCE_TYPES,
        data: payload
    }),
    FetchTimesheetReferences:(payload)=>({
        type: timesheetActionTypes.FETCH_TIMESHEET_REFERENCES_SUCCESS,
        data: payload 
    })
};

export const AddNewTimesheetReference = (data) => (dispatch, getstate) => {
    dispatch(actions.AddTimesheetReference(data,false));
};

export const DeleteTimesheetReference = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootTimesheetReducer.timesheetDetail.TimesheetReferences);
    data.forEach(row => {
        newState.forEach((iteratedValue, index) => {
            if (iteratedValue.timesheetReferenceId === row.timesheetReferenceId) {
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    const delIndex = newState.findIndex(value => (value.timesheetReferenceUniqueId === row.timesheetReferenceUniqueId));
                    if (delIndex >= 0)
                        newState.splice(delIndex, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteTimesheetReference(newState));
};

export const UpdatetTimesheetReference = (editedRowData, updatedData) => (dispatch) => {
    const editedItem = Object.assign({}, updatedData, editedRowData);
    let checkProperty = "timesheetReferenceId";
    if (editedItem.recordStatus === 'N') {
        checkProperty = "timesheetReferenceUniqueId";
    }
    dispatch(actions.UpdatetTimesheetReference({ editedItem, checkProperty }));
};

export const FetchReferencetypes = (isNewTimesheet) => async (dispatch, getstate) => {
    const state = getstate();
    let projectNumber;
    if (isNewTimesheet && !isEmptyOrUndefine(state.rootTimesheetReducer.timesheetDetail) &&
        !isEmptyOrUndefine(state.rootTimesheetReducer.timesheetDetail.TimesheetInfo)) {
        projectNumber = state.rootTimesheetReducer.timesheetDetail.TimesheetInfo.timesheetProjectNumber;
    } else if (state.rootTimesheetReducer.selectedTimesheet &&
        state.rootTimesheetReducer.selectedTimesheet.timesheetProjectNumber) {
        projectNumber = state.rootTimesheetReducer.selectedTimesheet.timesheetProjectNumber;
    }
    if (!projectNumber) {
        //IntertekToaster(localConstant.validationMessage.ERROR_FETCHING_TIMESHEET_REFERENCE_TYPES, 'dangerToast fetchTimesheetWrong');
        return false;
    }
    const requestPayload = new RequestPayload({});
    const url = customerAPIConfig.projects + projectNumber + assignmentAPIConfig.assignmnetReferenceTypes;
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.ERROR_FETCHING_TIMESHEET_REFERENCE_TYPES, 'dangerToast fetchTimesheetWrong');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && "1" === response.code) {
        const refTypes = response.result;
        if (Array.isArray(refTypes) && refTypes.length > 0) {
            const FilteredSet = refTypes.filter(x => x.isVisibleToTimesheet === true);
            dispatch(actions.FetchReferencetypes(FilteredSet));
            isNewTimesheet && dispatch(addTimesheetReferenceByDefault(FilteredSet));
        }
    }
    else if (response && response.code && (response.code === "11" || response.code === "41")) {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast fetchreferencesErr');
    }
    else {
        IntertekToaster('Failed to retrive Timesheet References', 'dangerToast fetchreferencesErr');
    }
};

export const addTimesheetReferenceByDefault = (referenceItems) => async (dispatch, getstate) => {
    const state = getstate();
    referenceItems.forEach(referenceData => {
        const updatedData = {};
        updatedData["referenceType"] = referenceData.referenceType;
        updatedData["referenceValue"] = "TBA";
        updatedData["timesheetReferenceUniqueId"] = Math.floor(Math.random() * 99) - 100;
        updatedData["timesheetReferenceId"] = null;
        updatedData["recordStatus"] = 'N';
        updatedData["timesheetId"] = null;
        updatedData["createdBy"] = state.appLayoutReducer.loginUser;
        updatedData["lastModification"]= moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT);
        dispatch(actions.AddTimesheetReference(updatedData,true));
    });
};

export const FetchTimesheetReferences = (timesheetId) => async (dispatch, getstate) => {
    const state = getstate();
    if (!isEmptyOrUndefine(state.rootTimesheetReducer.timesheetDetail.TimesheetReferences)) {
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
    const url = StringFormat(timesheetAPIConfig.references,timesheetId);
    const response = await FetchData(url, {}).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster('Failed to retrive Timesheet References', 'dangerToast fetchreferencesErr');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });

    if (response && response.code && response.code === "1") {
        dispatch(actions.FetchTimesheetReferences(response.result));
    }
    else if (response && response.code && (response.code === "11" || response.code === "41")) {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast fetchreferencesErr');
    }
    else {
        IntertekToaster('Failed to retrive Timesheet References', 'dangerToast fetchreferencesErr');
    }
    dispatch(HideLoader());
};