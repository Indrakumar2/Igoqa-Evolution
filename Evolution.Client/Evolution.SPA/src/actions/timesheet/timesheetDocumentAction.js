import { timesheetActionTypes } from '../../constants/actionTypes';
import {  timesheetAPIConfig, RequestPayload } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData } from '../../services/api/baseApiService';
import { getlocalizeData,isEmpty, getNestedObject } from '../../utils/commonUtils';
import { ShowLoader,HideLoader } from '../../common/commonAction';
import { StringFormat } from '../../utils/stringUtil';
const localConstant = getlocalizeData();

const actions = {
    FetchTimesheetDocuments: (payload) => ({
        type: timesheetActionTypes.FETCH_TIMESHEET_DOCUMENT_SUCCESS,
        data: payload
    }),
    AddTimesheetDocumentDetails: (payload) => ({
        type: timesheetActionTypes.ADD_TIMESHEET_DOCUMENTS,
        data: payload
    }),
    UpdateTimesheetDocumentDetails: (payload) => ({
        type: timesheetActionTypes.UPDATE_TIMESHEET_DOCUMENTS_DETAILS,
        data: payload
    }),
    DeleteTimesheetDocumentDetails: (payload) => ({
        type: timesheetActionTypes.DELETE_TIMESHEET_DOCUMENTS_DETAILS,
        data: payload
    }),
    AddFilesToBeUpload: (payload) => ({
        type: timesheetActionTypes.ADD_FILES_TO_BE_UPLOADED,
        data: payload
    }),

    ClearFilesToBeUpload: (payload) => ({
        type: timesheetActionTypes.CLEAR_FILES_TO_BE_UPLOADED,
        data: payload
    }),
};

export const FetchTimesheetDocuments = () => async (dispatch, getstate) => {
    const state =getstate();
    const timesheetId = getNestedObject(state.rootTimesheetReducer.timesheetDetail,[ 'TimesheetInfo','timesheetId' ]);
    if(!timesheetId){
        return false;
    }
    
        const param = {
            ModuleCode: "TIME",
            ModuleRefCode: timesheetId
        };
        const requestPayload = new RequestPayload(param);
        const response = await FetchData(timesheetAPIConfig.documents, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.errorMessages.FETCH_TIMESHEET_DOC_FAILED, 'wariningToast fetchAssignTimesheetVal');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (!isEmpty(response) && !isEmpty(response.code)) {
            if (response.code == 1) {
                dispatch(actions.FetchTimesheetDocuments(response.result));
            }
            else if (response.code == 41 || response.code == 11) {
                if (response.validationMessages.length > 0) {
                    response.validationMessages.map((result, index) => {
                        if (result.messages.length > 0) {
                            result.messages.map(valMessage => {
                                IntertekToaster(valMessage, 'warningToast');
                            });
                        }
                        else {
                            IntertekToaster(localConstant.errorMessages.FETCH_TIMESHEET_DOC_FAILED, 'dangerToast assignmentTimesheetrDocumentsSWWVal');
                        }
                    });
                }
            }
            else {
                if (response.messages.length > 0) {
                    response.messages.map(result => {
                        if (result.message.length > 0) {
                            IntertekToaster(result.message, 'warningToast');
                        }
                    });
                }
            }
        }
};

export const AddTimesheetDocumentDetails = (data) => (dispatch) => {
    dispatch(actions.AddTimesheetDocumentDetails(data));
};

export const UpdateTimesheetDocumentDetails = (data, editedData) => (dispatch, getstate) => {
    const state = getstate();
    if(editedData){
    const editedRow = Object.assign({}, editedData, data);    
    const index = state.rootTimesheetReducer.timesheetDetail.TimesheetDocuments.findIndex(document => document.id === editedRow.id);
    const newState = Object.assign([], state.rootTimesheetReducer.timesheetDetail.TimesheetDocuments);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateTimesheetDocumentDetails(newState));
    }
}
else{
    dispatch(actions.UpdateTimesheetDocumentDetails(data)); 
}
};

export const DeleteTimesheetDocumentDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootTimesheetReducer.timesheetDetail.TimesheetDocuments);
    data.map(row => {
        newState.map(document => {
            if (document.id === row.id) {
                const index = newState.findIndex(value => (value.id === row.id));
                newState[index].recordStatus = "D";
            }
        });
    });
    dispatch(actions.DeleteTimesheetDocumentDetails(newState));
};

export const AddFilesToBeUpload = (data) => (dispatch) => {
    dispatch(actions.AddFilesToBeUpload(data));
};

export const ClearFilesToBeUpload = (data) => (dispatch) => {
    dispatch(actions.ClearFilesToBeUpload([]));
};