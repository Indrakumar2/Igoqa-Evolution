import { projectActionTypes } from '../../constants/actionTypes';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { contractAPIConfig, customerAPIConfig, RequestPayload, projectAPIConfig } from '../../apiConfig/apiConfig';
import { FetchData } from '../../services/api/baseApiService';
import {
    getlocalizeData,
    parseValdiationMessage,
    isEmptyOrUndefine,
    isEmpty,
    getNestedObject
} from '../../utils/commonUtils';
import { StringFormat } from '../../utils/stringUtil';
const localConstant = getlocalizeData();

const actions = {
    AddClientNotification: (payload) => ({
        type: projectActionTypes.clientNotification.ADD_CLIENT_NOTIFICATION,
        data: payload
    }),
    DeleteClientNotification: (payload) => ({
        type: projectActionTypes.clientNotification.DELETE_CLIENT_NOTIFICATION,
        data: payload
    }),
    EditedClientNotificationData: (payload) => ({
        type: projectActionTypes.clientNotification.EDITED_CLIENT_NOTIFICATION_DATA,
        data: payload
    }),
    UpdatetNotificationData: (payload) => ({
        type: projectActionTypes.clientNotification.UPDATE_CLIENT_NOTIFICATION_DATA,
        data: payload
    }),
    FetchCustomerContact: (payload) => ({
        type: projectActionTypes.FETCH_CUSTOMER_CONTACT,
        data: payload
    })
};

//Add new client notification
export const AddClientNotification = (data) => (dispatch, getstate) => {
    dispatch(actions.AddClientNotification(data));
};
//Delete Clent Notification
export const DeleteClientNotification = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectNotifications);
    data.map(row => {
        newState.map(iteratedValue => {
            if (iteratedValue.projectClientNotificationId === row.projectClientNotificationId) {
                const index = newState.findIndex(value => (value.projectClientNotificationId === row.projectClientNotificationId));
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    newState.splice(index, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteClientNotification(newState));
};
//Edit Client Notification
export const EditedClientNotificationData = (data) => (dispatch, getstate) => {
    dispatch(actions.EditedClientNotificationData(data));
};
//Update client notification
export const UpdatetNotificationData = (updatedData, editedRowData) => (dispatch, getstate) => {
    const state = getstate();
    const editedRow = Object.assign({}, editedRowData, updatedData);
    const index = state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectNotifications.findIndex(iteratedValue => iteratedValue.projectClientNotificationId === editedRow.projectClientNotificationId);
    const newState = Object.assign([], state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectNotifications);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdatetNotificationData(newState));
    }
};
//Fetch Customer Contact Data
export const FetchCustomerContact = (contractCustomerCode) => async (dispatch, getstate) => {
    const state = getstate();
    if (!contractCustomerCode) {
        contractCustomerCode = getNestedObject(state.RootProjectReducer.ProjectDetailReducer.projectDetail, 
            [ 'ProjectInfo', 'contractCustomerCode' ]);
    }
    if (!contractCustomerCode) {
        IntertekToaster(localConstant.project.CUSTOMER_CONTACTS_NOT_FETCHED, 'dangerToast projectAssignment');
        return [];
    }
    const projectContracts = customerAPIConfig.customer + contractCustomerCode + contractAPIConfig.contacts;
    const params = {};
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(projectContracts, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.project.CUSTOMER_CONTACTS_NOT_FETCHED, 'dangerToast projectAssignment');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchCustomerContact(response.result));
        return response.result;
    }
    else if (response && response.code && (response.code === "11" || response.code === "41" || response.code === "31")) {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast CustContractWentWrong');
    }
    else {
        IntertekToaster(localConstant.project.CUSTOMER_CONTACTS_NOT_FETCHED, 'dangerToast projectAssignment');
        return [];
    }
};

export const FetchProjectClientNotifications = (projectNo, showErrors) => async (dispatch) => {
    if (!projectNo) {
        return false;
    }
    const url = StringFormat(projectAPIConfig.projectNotifications, projectNo);
    const response = await FetchData(url, {})
        .catch(error => {
            // console.error(error); // To show the error details in console
            // showErrors && IntertekToaster(localConstant.project.CUSTOMER_CONTACTS_NOT_FETCHED, 'dangerToast projectAssignment');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            return error;
        });
    if (response && response.code === "1") {
        //If required dispatch an action to save Project Client Notifications 
        return response.result;
    }
    else if (response && response.code && (response.code === "11" || response.code === "41" || response.code === "31")) {
        showErrors && IntertekToaster(parseValdiationMessage(response), 'warningToast conActDataSomethingWrong');
        return false;
    }
    else {
        showErrors && IntertekToaster(localConstant.project.CUSTOMER_CONTACTS_NOT_FETCHED, 'dangerToast assignemntSomthingWrong');
        return false;
    }
};
