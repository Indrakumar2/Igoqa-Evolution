import { timesheetActionTypes } from '../../constants/actionTypes';
import { FetchData,PostData } from '../../services/api/baseApiService';
import { 
    timesheetAPIConfig,
    masterData,
    RequestPayload,
    companyAPIConfig } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { 
    getlocalizeData,
    isEmptyOrUndefine,
    parseValdiationMessage,
    isEmptyReturnDefault,
    isEmpty,
    getNestedObject
 } from '../../utils/commonUtils';
import { StringFormat } from '../../utils/stringUtil';
import arrayUtil  from '../../utils/arrayUtil';
import {
    ShowLoader,
    HideLoader,
} from '../../common/commonAction';
import moment from 'moment';
import { FetchAssignmentAdditionalExpenses } from '../../actions/assignment/assignmentAdditionalExpensesAction';

const localConstant = getlocalizeData();
const actions = {
    FetchTechSpecRateSchedules: (payload) => ({
        type: timesheetActionTypes.FETCH_TECHSPEC_RATE_SCHEDULES,
        data: payload
    }),
    FetchTechSpecRateDefault: (payload) => ({
        type: timesheetActionTypes.ADD_TIMESHEET_TECHNICAL_SPECIALIST_RATE_SCHEDULE,
        data: payload
    }),
    FetchTimesheetTechnicalSpecialists:(payload)=>({
        type: timesheetActionTypes.FETCH_TIMESHEET_TECHNICAL_SPECIALISTS_SUCCESS,
        data:payload
    }),
    FetchTimesheetTechnicalSpecialistTime: (payload) => ({
        type: timesheetActionTypes.FETCH_TIMESHEET_TECHNICAL_SPECIALIST_TIME,
        data: payload
    }),
    FetchTimesheetTechnicalSpecialistTravel: (payload) => ({
        type: timesheetActionTypes.FETCH_TIMESHEET_TECHNICAL_SPECIALIST_TRAVEL,
        data: payload
    }),
    FetchTimesheetTechnicalSpecialistExpense: (payload) => ({
        type: timesheetActionTypes.FETCH_TIMESHEET_TECHNICAL_SPECIALIST_EXPENSE,
        data: payload
    }),
    FetchTimesheetTechnicalSpecialistConsumable: (payload) => ({
        type: timesheetActionTypes.FETCH_TIMESHEET_TECHNICAL_SPECIALIST_CONSUMABLE,
        data: payload
    }),
    AddTimesheetTechnicalSpecialistTime: (payload) => ({
        type: timesheetActionTypes.ADD_TIMESHEET_TECHNICAL_SPECIALIST_TIME,
        data: payload
    }),
    UpdateTimesheetTechnicalSpecialistTime: (payload) => ({
        type: timesheetActionTypes.UPDATE_TIMESHEET_TECHNICAL_SPECIALIST_TIME,
        data: payload
    }),
    DeleteTimesheetTechnicalSpecialistTime: (payload) => ({
        type: timesheetActionTypes.DELETE_TIMESHEET_TECHNICAL_SPECIALIST_TIME,
        data: payload
    }),
    AddTimesheetTechnicalSpecialistTravel: (payload) => ({
        type: timesheetActionTypes.ADD_TIMESHEET_TECHNICAL_SPECIALIST_TRAVEL,
        data: payload
    }),
    UpdateTimesheetTechnicalSpecialistTravel: (payload) => ({
        type: timesheetActionTypes.UPDATE_TIMESHEET_TECHNICAL_SPECIALIST_TRAVEL,
        data: payload
    }),
    DeleteTimesheetTechnicalSpecialistTravel: (payload) => ({
        type: timesheetActionTypes.DELETE_TIMESHEET_TECHNICAL_SPECIALIST_TRAVEL,
        data: payload
    }),
    AddTimesheetTechnicalSpecialistExpense: (payload) => ({
        type: timesheetActionTypes.ADD_TIMESHEET_TECHNICAL_SPECIALIST_EXPENSE,
        data: payload
    }),
    UpdateTimesheetTechnicalSpecialistExpense: (payload) => ({
        type: timesheetActionTypes.UPDATE_TIMESHEET_TECHNICAL_SPECIALIST_EXPENSE,
        data: payload
    }),
    DeleteTimesheetTechnicalSpecialistExpense: (payload) => ({
        type: timesheetActionTypes.DELETE_TIMESHEET_TECHNICAL_SPECIALIST_EXPENSE,
        data: payload
    }),
    AddTimesheetTechnicalSpecialistConsumable: (payload) => ({
        type: timesheetActionTypes.ADD_TIMESHEET_TECHNICAL_SPECIALIST_CONSUMABLE,
        data: payload
    }),
    UpdateTimesheetTechnicalSpecialistConsumable: (payload) => ({
        type: timesheetActionTypes.UPDATE_TIMESHEET_TECHNICAL_SPECIALIST_CONSUMABLE,
        data: payload
    }),
    DeleteTimesheetTechnicalSpecialistConsumable: (payload) => ({
        type: timesheetActionTypes.DELETE_TIMESHEET_TECHNICAL_SPECIALIST_CONSUMABLE,
        data: payload
    }),
    SetLinkedAssignmentExpenses:(payload)=>({
        type: timesheetActionTypes.SET_LINKED_ASSIGNMENT_EXPENSES,
        data:payload
    }),
    ReSetLinkedAssignmentExpenses: (payload) =>({
        type: timesheetActionTypes.RESET_LINKED_ASSIGNMENT_EXPENSES,
        data:payload
    }),
    SaveAssignmentUnLInkedExpenses:(payload)=>({
        type:timesheetActionTypes.SAVE_UNLINKED_ASSIGNMENT_EXPENSES,
        data:payload
    }),
    CompanyExpectedMargin:(payload)=>({
        type:timesheetActionTypes.COMPANY_EXPECTED_MARGIN,
        data:payload
    }),
    UpdateExpenseOpen:(payload)=>({
        type:timesheetActionTypes.EXPENSE_TAB_OPEN,
        data:payload
    }),
};
export const FetchTechSpecRateSchedules = (epin) => async (dispatch, getstate) => {
    const assignmentId = getstate().rootTimesheetReducer.timesheetDetail.TimesheetInfo.timesheetAssignmentId;
    dispatch(ShowLoader());
    const url = StringFormat(timesheetAPIConfig.techSpecRateSchedules, assignmentId);
    let params = {};
    if(epin){
        params = {
            epin: epin
        };
    }
     const requestPayload = new RequestPayload(params);
     dispatch(actions.FetchTechSpecRateSchedules(null));
     const response = await FetchData(url, requestPayload)
         .catch(error => {
            // console.error(error); // To show the error details in console
             dispatch(HideLoader());
            //  IntertekToaster(localConstant.errorMessages.ERROR_FETCHING_TECHSPEC_RATE_SCHEDULES, 'dangerToast fetchTimesheetWrong');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
         });
     if (response && "1" === response.code) {
         dispatch(actions.FetchTechSpecRateSchedules(response.result));
         dispatch(HideLoader());
     }else{
        dispatch(HideLoader());
     }
};

export const FetchTechSpecRateDefault = (epin) => async (dispatch, getstate) => {
    const state = getstate();
    const assignmentId = state.rootTimesheetReducer.timesheetDetail.TimesheetInfo.timesheetAssignmentId;
    dispatch(ShowLoader());
    const url = StringFormat(timesheetAPIConfig.TechSpecRateSchedule, assignmentId);
    
    const params = {
        epin: epin
    };     
    const requestPayload = new RequestPayload(params);     
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            dispatch(HideLoader());
            // IntertekToaster(localConstant.errorMessages.ERROR_FETCHING_TECHSPEC_RATE_SCHEDULES, 'dangerToast fetchTimesheetWrong');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    
    if (response && "1" === response.code) {  
        dispatch(actions.FetchTechSpecRateDefault(response.result));
        dispatch(FetchTechSpecRateSchedulesDefault(epin, assignmentId, response));
    }
    dispatch(HideLoader());
};

export const FetchTechSpecRateSchedulesDefault = (epin, assignmentId, response) => async (dispatch, getstate) => { 
    if (response && "1" === response.code) {  
        if(!isEmptyOrUndefine(response) && !isEmptyOrUndefine(response.result) && !isEmptyOrUndefine(response.result.chargeSchedules)) {
            const timeSheetStartDate = moment(getstate().rootTimesheetReducer.timesheetDetail.TimesheetInfo.timeSheetStartDate);
            const timesheetInfo = isEmptyReturnDefault(getstate().rootTimesheetReducer.timesheetDetail.TimesheetInfo,'object'); 
            response.result.chargeSchedules.forEach(row => {
                row.chargeScheduleRates.forEach(rowData => {
                    if(moment(rowData.effectiveFrom).isSameOrAfter(timeSheetStartDate, 'day')) {
                        const tempData = {};
                        tempData.expenseDate = rowData.effectiveFrom;
                        tempData.chargeExpenseType = rowData.chargeType;
                        tempData.chargeRateCurrency = rowData.currency;
                        tempData.payRateCurrency = rowData.currency;
                        tempData.chargeRate = 0;
                        tempData.chargeRateDescription = rowData.description;
                        tempData.pin = epin;
                        tempData.timesheetTechnicalSpecialistId = null;
                        tempData.recordStatus = 'N';
                        tempData.TechSpecTimeId = Math.floor(Math.random() * 999) -1000;
                        tempData.assignmentId = assignmentId;
                        tempData["timesheetId"]  = timesheetInfo.timesheetId; 
                        tempData["contractNumber"] = timesheetInfo.timesheetContractNumber;
                        tempData["projectNumber"] = timesheetInfo.timesheetProjectNumber;
                        if(rowData.type === 'R') {                            
                            dispatch(actions.AddTimesheetTechnicalSpecialistTime(tempData));                 
                        } else if(rowData.type === 'T') {                    
                            dispatch(actions.AddTimesheetTechnicalSpecialistTravel(tempData));
                        } else if(rowData.type === 'E') {                    
                            dispatch(actions.AddTimesheetTechnicalSpecialistExpense(tempData));
                        } else if(rowData.type === 'C') {                    
                            dispatch(actions.AddTimesheetTechnicalSpecialistConsumable(tempData));
                        }
                    }                    
                });            
            });
        }
    }
};

export const FetchTimesheetTechnicalSpecialists = (timesheetId) => async (dispatch, getstate) => {
    const state = getstate();
    if (!isEmptyOrUndefine(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialists)) {
        return;
    }
    if(!timesheetId){
        return false;
    }
    const timesheetSelectedTechSpecs = isEmptyReturnDefault(state.rootTimesheetReducer.timesheetSelectedTechSpecs);
    dispatch(ShowLoader());
    
    let url = StringFormat(timesheetAPIConfig.technicalSpecialist, timesheetId);
    if(timesheetSelectedTechSpecs.length > 0){
        url = StringFormat(timesheetAPIConfig.technicalSpecialistGrossmargin, timesheetId);
    }

    const response = await FetchData(url, {}).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TIMESHEET_DETAIL, 'dangerToast FetchTimesheetDetailError');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        dispatch(HideLoader());
    });

    if (response && response.code && response.code === "1") {
        dispatch(actions.FetchTimesheetTechnicalSpecialists(response.result));
    }
    else if (response && response.code && (response.code === "11" || response.code === "41")) {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast fetchreferencesErr');
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TIMESHEET_DETAIL, 'dangerToast fetchreferencesErr');
    }
    dispatch(HideLoader());
};

export const FetchTimesheetTechnicalSpecialistTime = (timesheetId) => async(dispatch, getstate) => {
    const state = getstate();
    if (!isEmpty(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistTimes)) {
        return;
    }
    if(!timesheetId){
        return false;
    }
    const url  = StringFormat(timesheetAPIConfig.technicalSpecialistTime, timesheetId);
    const param = {};
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster("Failed to Fetch Resource Time", 'wariningToast');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });

    if (!isEmpty(response)) {
        if (response.code === "1") {
            dispatch(actions.FetchTimesheetTechnicalSpecialistTime(response.result));
        }
        else if (response.code === "41") {
            if (!isEmptyReturnDefault(response.validationMessages)) {                    
            }
        }
        else if (response.code === "11") {
            if (!isEmptyReturnDefault(response.messages)) {                    
            }
        }
        else {

        }
    }
};

export const FetchTimesheetTechnicalSpecialistTravel = (timesheetId) => async(dispatch, getstate) => {
    const state = getstate();
    if (!isEmpty(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistTravels)) {
        return;
    }
    if(!timesheetId){
        return false;
    }
    const url  = StringFormat(timesheetAPIConfig.technicalSpecialistTravel, timesheetId);
    const param = {};
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster("Failed to Fetch Resource Time", 'wariningToast');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });

    if (!isEmpty(response)) {
        if (response.code === "1") {
            dispatch(actions.FetchTimesheetTechnicalSpecialistTravel(response.result));
        }
        else if (response.code === "41") {
            if (!isEmptyReturnDefault(response.validationMessages)) {                    
            }
        }
        else if (response.code === "11") {
            if (!isEmptyReturnDefault(response.messages)) {                    
            }
        }
        else {

        }
    }
};

export const FetchTimesheetTechnicalSpecialistExpense = (timesheetId) => async(dispatch, getstate) => {
    const state = getstate();
    if (!isEmpty(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistExpenses)) {
        return;
    }
    if(!timesheetId){
        return false;
    }
    const url  = StringFormat(timesheetAPIConfig.technicalSpecilistExpense, timesheetId);
    const param = {};
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster("Failed to Fetch Resource Time", 'wariningToast');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });

    if (!isEmpty(response)) {
        if (response.code === "1") {
            dispatch(actions.FetchTimesheetTechnicalSpecialistExpense(response.result));
        }
        else if (response.code === "41") {
            if (!isEmptyReturnDefault(response.validationMessages)) {                    
            }
        }
        else if (response.code === "11") {
            if (!isEmptyReturnDefault(response.messages)) {                    
            }
        }
        else {

        }
    }
};

export const FetchTimesheetTechnicalSpecialistConsumable = (timesheetId) => async(dispatch, getstate) => {
    const state = getstate();
    if (!isEmptyOrUndefine(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistConsumables)) {
        return;
    }
    if(!timesheetId){
        return false;
    }
    const url  = StringFormat(timesheetAPIConfig.technicalSpecialistConsumable, timesheetId);
    const param = {};
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster("Failed to Fetch Resource Time", 'wariningToast');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });

    if (!isEmpty(response)) {
        if (response.code === "1") {
            dispatch(actions.FetchTimesheetTechnicalSpecialistConsumable(response.result));
        }
        else if (response.code === "41") {
            if (!isEmptyReturnDefault(response.validationMessages)) {                    
            }
        }
        else if (response.code === "11") {
            if (!isEmptyReturnDefault(response.messages)) {                    
            }
        }
        else {

        }
    }
};

export const AddTimesheetTechnicalSpecialistTime = (data) => async(dispatch, getstate) => {
    dispatch(actions.AddTimesheetTechnicalSpecialistTime(data));
};

export const UpdateTimesheetTechnicalSpecialistTime = (editedRowData, updatedData) => async(dispatch, getstate) => {
    const state = getstate();
    const editedRow = Object.assign({},updatedData, editedRowData );
    const techSpecTime = state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistTimes;
    let checkProperty = "timesheetTechnicalSpecialistAccountTimeId";
    if (editedRow.recordStatus === 'N') {
        checkProperty = "TechSpecTimeId";
    }
    const index = techSpecTime.findIndex(iteratedValue => iteratedValue[checkProperty] === editedRow[checkProperty]);
    
    const newState = Object.assign([], techSpecTime);
    if (index >= 0) {
        newState[index] = editedRow;
        dispatch(actions.UpdateTimesheetTechnicalSpecialistTime(newState));
    }
};

export const DeleteTimesheetTechnicalSpecialistTime = (data) => async(dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistTimes);
    data.forEach(row => {
        newState.forEach((iteratedValue, index) => {
            if (iteratedValue.timesheetTechnicalSpecialistAccountTimeId === row.timesheetTechnicalSpecialistAccountTimeId) {
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    const delIndex = newState.findIndex(value => (value.TechSpecTimeId === row.TechSpecTimeId));
                    if (delIndex >= 0)
                        newState.splice(delIndex, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteTimesheetTechnicalSpecialistTime(newState));
};

export const AddTimesheetTechnicalSpecialistTravel = (data) => async(dispatch, getstate) => {
    dispatch(actions.AddTimesheetTechnicalSpecialistTravel(data));
};

export const UpdateTimesheetTechnicalSpecialistTravel = (editedRowData, updatedData) => async(dispatch, getstate) => {
    const state = getstate();
    const editedRow = Object.assign({},updatedData, editedRowData );
    const techSpecTravel = state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistTravels;
    let checkProperty = "timesheetTechnicalSpecialistAccountTravelId";
    if (editedRow.recordStatus === 'N') {
        checkProperty = "TechSpecTravelId";
    }
    const index = techSpecTravel.findIndex(iteratedValue => iteratedValue[checkProperty] === editedRow[checkProperty]);
    
    const newState = Object.assign([], techSpecTravel);
    if (index >= 0) {
        newState[index] = editedRow;
        dispatch(actions.UpdateTimesheetTechnicalSpecialistTravel(newState));
    }
};

export const DeleteTimesheetTechnicalSpecialistTravel = (data) => async(dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistTravels);
    data.forEach(row => {
        newState.forEach((iteratedValue, index) => {
            if (iteratedValue.timesheetTechnicalSpecialistAccountTravelId === row.timesheetTechnicalSpecialistAccountTravelId) {
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    const delIndex = newState.findIndex(value => (value.TechSpecTravelId === row.TechSpecTravelId));
                    if (delIndex >= 0)
                        newState.splice(delIndex, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteTimesheetTechnicalSpecialistTravel(newState));
};

export const AddTimesheetTechnicalSpecialistExpense = (data) => async(dispatch, getstate) => {
    dispatch(actions.AddTimesheetTechnicalSpecialistExpense(data));
};

export const UpdateTimesheetTechnicalSpecialistExpense = (editedRowData, updatedData) => async(dispatch, getstate) => {
    const state = getstate();
    const editedRow = Object.assign({},updatedData, editedRowData );
    const techSpecExpense = state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistExpenses;
    let checkProperty = "timesheetTechnicalSpecialistAccountExpenseId";
    if (editedRow.recordStatus === 'N') {
        checkProperty = "TechSpecExpenseId";
    }
    const index = techSpecExpense.findIndex(iteratedValue => iteratedValue[checkProperty] === editedRow[checkProperty]);
    
    const newState = Object.assign([], techSpecExpense);
    if (index >= 0) {
        newState[index] = editedRow;
        dispatch(actions.UpdateTimesheetTechnicalSpecialistExpense(newState));
    }
};

export const DeleteTimesheetTechnicalSpecialistExpense = (data) => async(dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistExpenses);
    data.forEach(row => {
        newState.forEach((iteratedValue, index) => {
            if (iteratedValue.timesheetTechnicalSpecialistAccountExpenseId === row.timesheetTechnicalSpecialistAccountExpenseId) {
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    const delIndex = newState.findIndex(value => (value.TechSpecExpenseId === row.TechSpecExpenseId));
                    if (delIndex >= 0){
                        dispatch(actions.ReSetLinkedAssignmentExpenses([ newState[delIndex] ]));
                        newState.splice(delIndex, 1);
                    }
                }
            }
        });
    });
    dispatch(actions.DeleteTimesheetTechnicalSpecialistExpense(newState));
};

export const AddTimesheetTechnicalSpecialistConsumable = (data) => async(dispatch, getstate) => {
    dispatch(actions.AddTimesheetTechnicalSpecialistConsumable(data));
};

export const UpdateTimesheetTechnicalSpecialistConsumable = (editedRowData, updatedData) => async(dispatch, getstate) => {
    const state = getstate();
    const editedRow = Object.assign({},updatedData, editedRowData );
    const techSpecConsumable = state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistConsumables;
    let checkProperty = "timesheetTechnicalSpecialistAccountConsumableId";
    if (editedRow.recordStatus === 'N') {
        checkProperty = "TechSpecConsumableId";
    }
    const index = techSpecConsumable.findIndex(iteratedValue => iteratedValue[checkProperty] === editedRow[checkProperty]);
    
    const newState = Object.assign([], techSpecConsumable);
    if (index >= 0) {
        newState[index] = editedRow;
        dispatch(actions.UpdateTimesheetTechnicalSpecialistConsumable(newState));
    }
};

export const DeleteTimesheetTechnicalSpecialistConsumable = (data) => async(dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistConsumables);
    data.forEach(row => {
        newState.forEach((iteratedValue, index) => {
            if (iteratedValue.timesheetTechnicalSpecialistAccountConsumableId === row.timesheetTechnicalSpecialistAccountConsumableId) {
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    const delIndex = newState.findIndex(value => (value.TechSpecConsumableId === row.TechSpecConsumableId));
                    if (delIndex >= 0)
                        newState.splice(delIndex, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteTimesheetTechnicalSpecialistConsumable(newState));
};

export const FetchAssignmentAdditionalExpensesForTimesheet = (assignmentId) => async (dispatch, getstate) => {
    if (!assignmentId) {
        const timesheetInfo = isEmptyReturnDefault(getstate().rootTimesheetReducer.timesheetDetail.TimesheetInfo, 'object');
        assignmentId = timesheetInfo.timesheetAssignmentId;
    }
    if (!assignmentId) {
        return false;
    }
    const expenses = await dispatch(FetchAssignmentAdditionalExpenses(assignmentId));
    if (expenses && expenses.length > 0) {
        //const unLinkedExpenses = expenses.filter(x => !x.isAlreadyLinked);
        //if (unLinkedExpenses.length > 0) {
            dispatch(actions.SaveAssignmentUnLInkedExpenses(expenses));
        //}
    }
};

export const SaveAssignmentUnLInkedExpenses = (unLinkedExpenses) => async (dispatch, getstate) => {
         dispatch(actions.SaveAssignmentUnLInkedExpenses(unLinkedExpenses));
};

export const SetLinkedAssignmentExpenses = (data)=> async(dispatch, getstate) => {
    dispatch(actions.SetLinkedAssignmentExpenses(data));
};

export const FetchCompanyExpectedMargin = (currencyObj) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const companyCode = getNestedObject(getstate().rootTimesheetReducer.timesheetDetail,[ 'TimesheetInfo','timesheetContractCompanyCode' ]);
    if(companyCode){
        const url =StringFormat(companyAPIConfig.expectedMargins,companyCode);
        const response = await FetchData(url, {})
            .catch(error => {
                // console.error(error); // To show the error details in console
                dispatch(HideLoader());
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response && "1" === response.code) {
            const assignmentProjectBusinessUnit = getNestedObject(getstate().rootTimesheetReducer.timesheetDetail,[ 'TimesheetInfo','assignmentProjectBusinessUnit' ]);
            const expectedMarginObj =  arrayUtil.FilterGetObject(response.result,'marginType',assignmentProjectBusinessUnit);
            dispatch(actions.CompanyExpectedMargin(expectedMarginObj?expectedMarginObj.minimumMargin:null));
        }
    }
    dispatch(HideLoader());
};

// export const FetchCurrencyExchangeRate = (currencyObj) => async (dispatch, getstate) => {
//     dispatch(ShowLoader());
//     const params = currencyObj;
//     const requestPayload = new RequestPayload(params);
//     const response = await PostData(masterData.exchangeRate, requestPayload)
//         .catch(error => {
//             dispatch(HideLoader());
//         });
//     if (response && "1" === response.code) {
//         dispatch(HideLoader());
//         return response.result;
//     }
//     else if (response && response.code && (response.code === "11" || response.code === "41")) {
//         IntertekToaster(parseValdiationMessage(response), 'warningToast conActDataSomethingWrong');
//         dispatch(HideLoader());
//         return false;
//     }
//     else {
//         IntertekToaster(parseValdiationMessage(response), 'warningToast conActDataSomethingWrong');
//         dispatch(HideLoader());
//         return false;
//     }
// };
export const FetchCurrencyExchangeRate = (currencyObj,contractNumber) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    if(!contractNumber){
        contractNumber = getNestedObject(getstate().rootTimesheetReducer.timesheetDetail,[ 'TimesheetInfo','timesheetContractNumber' ]);
    }
    const params = {
        exchangeRates:currencyObj,
        contractNumber:contractNumber?contractNumber:''
    };
    const requestPayload = new RequestPayload(params);
    const response = await PostData(timesheetAPIConfig.GetExpenseChargeExchangeRates, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (response && "1" === response.code) {
        dispatch(HideLoader());
        return response.result;
    }
    else if (response && response.code && (response.code === "11" || response.code === "41")) {
        IntertekToaster(parseValdiationMessage(response), 'warningToast conActDataSomethingWrong');
        dispatch(HideLoader());
        return false;
    }
    else {
        IntertekToaster(parseValdiationMessage(response), 'warningToast conActDataSomethingWrong');
        dispatch(HideLoader());
        return false;
    }
};

export const UpdateExpenseOpen = (data)=> async(dispatch, getstate) => {
    dispatch(actions.UpdateExpenseOpen(data));
};