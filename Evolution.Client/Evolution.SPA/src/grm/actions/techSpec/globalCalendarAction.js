import { techSpecActionTypes } from '../../constants/actionTypes';
import { visitAPIConfig, GrmAPIConfig, timesheetAPIConfig, RequestPayload } from '../../../apiConfig/apiConfig';
import { ShowLoader, HideLoader } from '../../../common/commonAction';
import { FetchData, PostData, CreateData } from '../../../services/api/baseApiService';
import { getlocalizeData, mergeobjects, isEmptyReturnDefault, isEmpty } from '../../../utils/commonUtils';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';
import { StringFormat } from '../../../utils/stringUtil';
import { ValidationAlert } from '../../../components/viewComponents/customer/alertAction';
import moment from 'moment';

const localConstant = getlocalizeData();

const actions = {
    FetchCalendarData: (payload) => (
        {
            type: techSpecActionTypes.techSpecSearch.FETCH_GLOBAL_CALENDAR_DATA,
            data: payload
        }
    ),

    FetchVisitByID: (payload) => (
        {
            type: techSpecActionTypes.techSpecSearch.FETCH_GLOBAL_CALENDAR_VISIT_DATA,
            data: payload
        }
    ),

    FetchTimesheetGeneralDetail: (payload) => (
        {
            type: techSpecActionTypes.techSpecSearch.FETCH_GLOBAL_CALENDAR_TIMESHEET_DATA,
            data: payload
        }
    ),

    FetchPreAssignment: (payload) => (
        {
            type: techSpecActionTypes.techSpecSearch.FETCH_GLOBAL_CALENDAR_PREASSIGNMENT_DATA,
            data: payload
        }
    ),

    ClearCalendarData: (payload) => (
        {
            type: techSpecActionTypes.techSpecSearch.CLEAR_GLOBAL_CALENDAR_DATA,
            data: payload
        }
    ),

};

export const FetchCalendarData = (data, isSearch, isCalendarView) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const apiUrl = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalSpecialists + GrmAPIConfig.technicalSpecialistCalendar;
    data.isSearch = isSearch;
    if (isCalendarView !== undefined)
        data.isCalendarView = isCalendarView;
    const requestPayload = new RequestPayload(data);
    const response = await FetchData(apiUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast technicalSpecilistData');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        // dispatch(actions.FetchCalendarData(response.result));
        dispatch(HideLoader());
        return response.result;
    }
    else {
        IntertekToaster(localConstant.errorMessages.CALENDAR_DATA_NOT_FETCHED, 'dangerToast technicalSpecilistData');
        dispatch(HideLoader());
    }
};

export const FetchVisitByID = (visitID) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const url = visitAPIConfig.visitBaseUrl + visitAPIConfig.visits + StringFormat(visitAPIConfig.GetVisitByID, visitID);
    const param = {
        VisitId: visitID
    };
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.FETCH_VISIT_BY_ID, 'wariningToast OperatingCoordinatorValPreAssign');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });

    if (!isEmpty(response)) {
        if (response.code === "1") {
            // dispatch(actions.FetchVisitByID(response.result));
            dispatch(HideLoader());
            return response != null ? response.result : null;
        }
        else if (response.code === "41") {
            if (!isEmptyReturnDefault(response.validationMessages)) {
                //To-Do: Common approach to display validationMessages
            }
        }
        else if (response.code === "11") {
            if (!isEmptyReturnDefault(response.messages)) {
                //To-Do: Common approach to display messages
            }
        }
        else {

        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast OperatingCoordinatorSWWValPreAssign');
    }
    dispatch(HideLoader());
};

export const FetchTimesheetGeneralDetail = (timesheetId) => async (dispatch, getstate) => {

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

export const FetchPreAssignment = (preAssignmentId) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    if (preAssignmentId) {
        const apiUrl = GrmAPIConfig.preAssignmentSave;
        const param = {
            "id": preAssignmentId
        };
        const requestPayload = new RequestPayload(param);
        const response = await FetchData(apiUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.errorMessages.FETCH_PRE_ASSIGNMENT_FAILED, 'dangerToast preAssignmentErr');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response) {
            //To-do Search Result Grid Bind
            if (response.code === "1") {
                if (response.result && response.result.length > 0) {
                    dispatch(HideLoader());
                    if (response.result[0].searchParameter)
                        response.result[0].searchParameter = JSON.parse(response.result[0].searchParameter);
                    return response.result[0];
                }
                else {
                    dispatch(actions.FetchPreAssignment({}));
                    IntertekToaster(localConstant.errorMessages.FETCH_PRE_ASSIGNMENT_FAILED, 'dangerToast preAssignmentErr');
                }
            }
            else if (response.code === "41") {
                if (!isEmptyReturnDefault(response.validationMessages)) {
                    //To-Do: Common approach to display validationMessages
                }
            }
            else if (response.code === "11") {
                if (!isEmptyReturnDefault(response.messages)) {
                    //To-Do: Common approach to display messages
                }
            }
            else {

            }
            dispatch(HideLoader());
        }
        else {
            dispatch(HideLoader());
        }
    }
    else {
        dispatch(HideLoader());
    }
};

export const SaveCalendarTask = (calendarData, startDate, endDate) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    if (calendarData) {
        const apiUrl = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalSpecialists + GrmAPIConfig.technicalSpecialistCalendar;
        const param = {
            "id": calendarData.id,
            "technicalSpecialistId": calendarData.resourceId,
            "companyId": calendarData.companyId,
            "companyCode": calendarData.companyCode,
            "calendarType": calendarData.calendarType,
            "calendarRefCode": calendarData.calendarRefCode,
            "calendarStatus": calendarData.calendarStatus,
            "startDateTime": startDate,
            "endDateTime": endDate,
            "createdBy": calendarData.createdBy,
            "isActive": calendarData.isActive,
            "recordStatus": "M",
            "updateCount": calendarData.updateCount,
            "isActive": calendarData.resourceAllocation ? !calendarData.resourceAllocation : true
        };
        const paramList = [];
        paramList.push(param);
        const requestPayload = new RequestPayload(paramList);
        const response = await CreateData(apiUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.errorMessages.FETCH_PRE_ASSIGNMENT_FAILED, 'dangerToast preAssignmentErr');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response) {
            //To-do Search Result Grid Bind
            if (response.code === "1") {
                dispatch(HideLoader());
                IntertekToaster(localConstant.globalCalendar.CALENDAR_UPDATED_SUCCESSFULLY, 'successToast alertActSuccAlt');
                return true;
            }
            else if (response.code === "11" || response.code === "41") {
                if (response.validationMessages.length > 0) {
                    response.validationMessages.map(result => {
                        if (result.messages.length > 0) {
                            result.messages.map(valMessage => {
                                dispatch(ValidationAlert(valMessage.message, "quickSearch"));
                            });
                        }
                        else {
                            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "quickSearch"));
                        }
                    });
                }
                else if (response.messages.length > 0) {
                    response.messages.map(result => {
                        if (result.message.length > 0) {
                            dispatch(ValidationAlert(result.message, "quickSearch"));
                        }
                    });
                }
                else {
                    dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "quickSearch"));
                }
                dispatch(HideLoader());
                return false;
            }
            else {
                dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "quickSearch"));
                dispatch(HideLoader());
                return false;
            }
        }
        else {
            dispatch(HideLoader());
        }
    }
    else {
        dispatch(HideLoader());
    }
};

export const ClearCalendarData = () => async (dispatch, getstate) => {
    dispatch(actions.ClearCalendarData());
};

export const FetchTimeOffRequestData = (id) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const url = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalSpecialists + GrmAPIConfig.timeoffrequest;
    const param = {
        technicalSpecialistTimeOffRequestId: id
    };
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast technicalSpecilistData');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(HideLoader());
        return response;
    }
    else {
        dispatch(HideLoader());
        IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast technicalSpecilistData');
    }
};

export const FetchVisitTimesheetCalendarData = (data, ids) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const apiUrl = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalSpecialists + GrmAPIConfig.getCalendarByTechnicalSpecialistId;
    data.startDateTime = data.startDateTime === "Invalid Date" ? null : data.startDateTime;
    data.endDateTime = data.endDateTime === "Invalid Date" ? null : data.endDateTime;
    data.technicalSpecialistIds = ids;
    const requestPayload = new RequestPayload(data);
    const response = await PostData(apiUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast technicalSpecilistData');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        // dispatch(actions.FetchCalendarData(response.result));
        dispatch(HideLoader());
        return response.result;
    }
    else {
        IntertekToaster(localConstant.errorMessages.CALENDAR_DATA_NOT_FETCHED, 'dangerToast technicalSpecilistData');
        dispatch(HideLoader());
    }
};