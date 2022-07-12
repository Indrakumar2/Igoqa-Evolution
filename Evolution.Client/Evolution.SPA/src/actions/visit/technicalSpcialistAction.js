import { visitActionTypes } from '../../constants/actionTypes';
import { visitAPIConfig, RequestPayload, companyAPIConfig,timesheetAPIConfig } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData,PostData } from '../../services/api/baseApiService';
import { 
    getlocalizeData,
    isEmpty,
    isEmptyReturnDefault,
    parseValdiationMessage,
    isEmptyOrUndefine,
    getNestedObject } from '../../utils/commonUtils';
import { ShowLoader, HideLoader } from '../../common/commonAction';
import { StringFormat } from '../../utils/stringUtil';
import arrayUtil  from '../../utils/arrayUtil';
import moment from 'moment';
import { FetchAssignmentAdditionalExpenses } from '../../actions/assignment/assignmentAdditionalExpensesAction';

const localConstant = getlocalizeData();

const actions = {
    FetchTechnicalSpecialist: (payload) => ({
        type: visitActionTypes.FETCH_VISIT_TECHNICAL_SPECIALIST,
        data: payload
    }),
    FetchTechSpecRateSchedules: (payload) => ({
        type: visitActionTypes.FETCH_VISIT_TECHNICAL_SPECIALIST_RATE_SCHEDULE,
        data: payload
    }),
    FetchTechSpecRateDefault: (payload) => ({
        type: visitActionTypes.ADD_VISIT_TECHNICAL_SPECIALIST_RATE_SCHEDULE,
        data: payload
    }),
    FetchTechnicalSpecialistTime: (payload) => ({
        type: visitActionTypes.FETCH_VISIT_TECHNICAL_SPECIALIST_TIME,
        data: payload
    }),
    FetchTechnicalSpecialistTravel: (payload) => ({
        type: visitActionTypes.FETCH_VISIT_TECHNICAL_SPECIALIST_TRAVEL,
        data: payload
    }),
    FetchTechnicalSpecialistExpense: (payload) => ({
        type: visitActionTypes.FETCH_VISIT_TECHNICAL_SPECIALIST_EXPENSE,
        data: payload
    }),
    FetchTechnicalSpecialistConsumable: (payload) => ({
        type: visitActionTypes.FETCH_VISIT_TECHNICAL_SPECIALIST_CONSUMABLE,
        data: payload
    }),
    AddTechnicalSpecialistTime: (payload) => ({
        type: visitActionTypes.ADD_VISIT_TECHNICAL_SPECIALIST_TIME,
        data: payload
    }),
    UpdateTechnicalSpecialistTime: (payload) => ({
        type: visitActionTypes.UPDATE_VISIT_TECHNICAL_SPECIALIST_TIME,
        data: payload
    }),
    DeleteTechnicalSpecialistTime: (payload) => ({
        type: visitActionTypes.DELETE_VISIT_TECHNICAL_SPECIALIST_TIME,
        data: payload
    }),
    AddTechnicalSpecialistTravel: (payload) => ({
        type: visitActionTypes.ADD_VISIT_TECHNICAL_SPECIALIST_TRAVEL,
        data: payload
    }),
    UpdateTechnicalSpecialistTravel: (payload) => ({
        type: visitActionTypes.UPDATE_VISIT_TECHNICAL_SPECIALIST_TRAVEL,
        data: payload
    }),
    DeleteTechnicalSpecialistTravel: (payload) => ({
        type: visitActionTypes.DELETE_VISIT_TECHNICAL_SPECIALIST_TRAVEL,
        data: payload
    }),
    AddTechnicalSpecialistExpense: (payload) => ({
        type: visitActionTypes.ADD_VISIT_TECHNICAL_SPECIALIST_EXPENSE,
        data: payload
    }),
    UpdateTechnicalSpecialistExpense: (payload) => ({
        type: visitActionTypes.UPDATE_VISIT_TECHNICAL_SPECIALIST_EXPENSE,
        data: payload
    }),
    DeleteTechnicalSpecialistExpense: (payload) => ({
        type: visitActionTypes.DELETE_VISIT_TECHNICAL_SPECIALIST_EXPENSE,
        data: payload
    }),
    AddTechnicalSpecialistConsumable: (payload) => ({
        type: visitActionTypes.ADD_VISIT_TECHNICAL_SPECIALIST_CONSUMABLE,
        data: payload
    }),
    UpdateTechnicalSpecialistConsumable: (payload) => ({
        type: visitActionTypes.UPDATE_VISIT_TECHNICAL_SPECIALIST_CONSUMABLE,
        data: payload
    }),
    DeleteTechnicalSpecialistConsumable: (payload) => ({
        type: visitActionTypes.DELETE_VISIT_TECHNICAL_SPECIALIST_CONSUMABLE,
        data: payload
    }),
    UpdateTechSpecRateChanged: (payload) => ({
        type: visitActionTypes.UPDATE_TECH_SPEC_RATE_CHANGED,
        data: payload
    }),
    CompanyExpectedMargin:(payload)=>({
        type:visitActionTypes.VISIT_COMPANY_EXPECTED_MARGIN,
        data:payload
    }),
    SetLinkedAssignmentExpenses:(payload)=>({
        type: visitActionTypes.SET_LINKED_ASSIGNMENT_EXPENSES,
        data:payload
    }),
    ReSetLinkedAssignmentExpenses: (payload) =>({
        type: visitActionTypes.RESET_LINKED_ASSIGNMENT_EXPENSES,
        data:payload
    }),
    SaveAssignmentUnLInkedExpenses:(payload)=>({
        type:visitActionTypes.SAVE_UNLINKED_ASSIGNMENT_EXPENSES,
        data:payload
    }),
    UpdateVisitStatusByLineItems:(payload)=>({
        type:visitActionTypes.UPDATE_VISIT_STATUS_BY_LINE_ITEMS,
        data:payload
    }),
    UpdateExpenseOpen:(payload)=>({
        type:visitActionTypes.EXPENSE_TAB_OPEN,
        data:payload
    })
};

export const FetchVisitTechnicalSpecialists = () => async (dispatch, getstate) => {      
    const state = getstate();
    if (!isEmptyOrUndefine(state.rootVisitReducer.visitDetails.TimesheetTechnicalSpecialists)) {
        return;
    }
    const selectedTechSpecs = isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialists);
   
    const visitId = state.rootVisitReducer.selectedVisitData.visitId;    
    if(!visitId){
        return false;
    }
    
    if(selectedTechSpecs.length > 0){        
        const url = StringFormat(visitAPIConfig.technicalSpecialistGrossmargin, visitId);
        dispatch(ShowLoader());
        const params = {
            visitId: visitId
        };
        
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(url, requestPayload).catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_VISIT_DETAIL, 'dangerToast FetchVisitDetailError');        
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
        //dispatch(await FetchTechnicalSpecialistTime());
        if (response && response.code && response.code === "1") {                    
            dispatch(actions.FetchTechnicalSpecialist(response.result));        
        }
        else if (response && response.code && (response.code === "11" || response.code === "41")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast fetchreferencesErr');
        }
        else {
            IntertekToaster(localConstant.errorMessages.FAILED_FETCH_VISIT_DETAIL, 'dangerToast fetchreferencesErr');
        }
        dispatch(HideLoader());
    }     
};

export const FetchTechnicalSpecialist = () => async (dispatch, getstate) => {
    const state = getstate();
    if (!isEmpty(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialists)) {
        return;
    }
    dispatch(ShowLoader());
    
    const visitID = state.rootVisitReducer.selectedVisitData.visitId;
    const url = visitAPIConfig.visitBaseUrl + visitAPIConfig.visits + StringFormat(visitAPIConfig.TechnicalSpecialist, visitID);
    const param = {
        VisitId: visitID
    };
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.validationMessage.FETCH_VISIT_TECHNICAL_SPECIALIST, 'wariningToast');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });

        if (!isEmpty(response)) {
            if (response.code === "1") {
                dispatch(actions.FetchTechnicalSpecialist(response.result));
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
        else {
            //IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast');
        }

    dispatch(HideLoader());
};

export const FetchTechSpecRateSchedules = (epin) => async (dispatch, getstate) => {
    const state = getstate();
    let assignmentId ;
    if(state.rootVisitReducer.visitDetails.VisitInfo && 
        state.rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId){
            assignmentId = state.rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;
        }
    if(!assignmentId){
        assignmentId = state.rootVisitReducer.selectedVisitData.visitAssignmentId;
    }
    dispatch(ShowLoader());
    const url = StringFormat(visitAPIConfig.TechSpecRateSchedule, assignmentId);
    
    const params = {
        epin: epin
    };

     const requestPayload = new RequestPayload(params);
     //dispatch(actions.FetchTechSpecRateSchedules(null));
     const response = await FetchData(url, requestPayload)
         .catch(error => {
            // console.error(error); // To show the error details in console
             dispatch(HideLoader());
            //  IntertekToaster(localConstant.errorMessages.ERROR_FETCHING_TECHSPEC_RATE_SCHEDULES, 'dangerToast fetchTimesheetWrong');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
         });
     if (response && "1" === response.code) {
         //dispatch(actions.FetchTechSpecRateSchedules(response.result));
     }
    dispatch(HideLoader());
};

export const FetchTechSpecRateDefault = (epin) => async (dispatch, getstate) => {
    const state = getstate();
    let assignmentId ;
    if(state.rootVisitReducer.visitDetails.VisitInfo && 
        state.rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId){
            assignmentId = state.rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;
        }
    if(!assignmentId){
        assignmentId = state.rootVisitReducer.selectedVisitData.visitAssignmentId;
    }
    dispatch(ShowLoader());
    const url = StringFormat(visitAPIConfig.TechSpecRateSchedule, assignmentId);
    
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
            const visitStartDate = moment(getstate().rootVisitReducer.visitDetails.VisitInfo.visitStartDate);
            response.result.chargeSchedules.forEach(row => {
                row.chargeScheduleRates.forEach(rowData => {
                    if(moment(rowData.effectiveFrom).isSameOrAfter(visitStartDate, 'day')) {
                        const tempData = {};
                        tempData.expenseDate = rowData.effectiveFrom;
                        tempData.chargeExpenseType = rowData.chargeType;
                        tempData.chargeRateCurrency = rowData.currency;
                        //tempData.chargeRate = rowData.chargeRate;
                        tempData.chargeRateDescription = rowData.description;
                        tempData.pin = epin;
                        tempData.visitTechnicalSpecialistId = null;
                        tempData.recordStatus = 'N';
                        tempData.TechSpecTimeId = Math.floor(Math.random() * 99) -100;
                        tempData.assignmentId = assignmentId;
                        if(rowData.type === 'R') {                            
                            dispatch(actions.AddTechnicalSpecialistTime(tempData));                 
                        } else if(rowData.type === 'T') {                    
                            dispatch(actions.AddTechnicalSpecialistTravel(tempData));
                        } else if(rowData.type === 'E') {                    
                            dispatch(actions.AddTechnicalSpecialistExpense(tempData));
                        } else if(rowData.type === 'C') {                    
                            dispatch(actions.AddTechnicalSpecialistConsumable(tempData));
                        }
                    }                    
                });            
            });
        }
    }
};

export const FetchTechnicalSpecialistTime = () => async (dispatch, getstate) => {    
    const state = getstate();
    if (!isEmpty(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTimes)) {
        return;
    }
    
    //const visitID = state.rootVisitReducer.selectedVisitData.visitId;    
    const visitID = state.rootVisitReducer.visitDetails.VisitInfo.visitId;    
    const url  = StringFormat(visitAPIConfig.technicalSpecialistTime, visitID);
    const param = {};
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.validationMessage.FETCH_VISIT_TECHNICAL_SPECIALIST, 'wariningToast');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });

    if (!isEmpty(response)) {
        if (response.code === "1") {            
            dispatch(actions.FetchTechnicalSpecialistTime(response.result));            
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
    else {
        //IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast');
    }
};

export const FetchTechnicalSpecialistTravel = () => async(dispatch, getstate) => {
    const state = getstate();
    if (!isEmpty(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTravels)) {
        return;
    }
    
    //const visitID = state.rootVisitReducer.visitDetails.selectedVisitData.visitId; //186
    const visitID = state.rootVisitReducer.visitDetails.VisitInfo.visitId;
    const url  = StringFormat(visitAPIConfig.technicalSpecialistTravel, visitID);
    const param = {};
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.FETCH_VISIT_TECHNICAL_SPECIALIST, 'wariningToast');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });

    if (!isEmpty(response)) {
        if (response.code === "1") {
            dispatch(actions.FetchTechnicalSpecialistTravel(response.result));
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
    else {
        //IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast');
    }
};

export const FetchTechnicalSpecialistExpense = () => async(dispatch, getstate) => {
    const state = getstate();
    if (!isEmpty(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistExpenses)) {
        return;
    }
    
    //const visitID = state.rootVisitReducer.selectedVisitData.visitId; //186
    const visitID = state.rootVisitReducer.visitDetails.VisitInfo.visitId;
    const url  = StringFormat(visitAPIConfig.technicalSpecilistExpense, visitID);
    const param = {};
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.FETCH_VISIT_TECHNICAL_SPECIALIST, 'wariningToast');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });

    if (!isEmpty(response)) {
        if (response.code === "1") {
            dispatch(actions.FetchTechnicalSpecialistExpense(response.result));
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
    else {
        //IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast');
    }
};

export const FetchTechnicalSpecialistConsumable = () => async(dispatch, getstate) => {
    const state = getstate();
    if (!isEmpty(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistConsumables)) {
        return;
    }
    
    //const visitID = state.rootVisitReducer.selectedVisitData.visitId; //186
    const visitID = state.rootVisitReducer.visitDetails.VisitInfo.visitId;
    const url  = StringFormat(visitAPIConfig.technicalSpecialistConsumable, visitID);
    const param = {};
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.FETCH_VISIT_TECHNICAL_SPECIALIST, 'wariningToast');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });

    if (!isEmpty(response)) {
        if (response.code === "1") {
            dispatch(actions.FetchTechnicalSpecialistConsumable(response.result));
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
    else {
        //IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast');
    }
};

export const AddTechnicalSpecialistTime = (data) => async (dispatch, getstate) => {    
    dispatch(actions.AddTechnicalSpecialistTime(data));        
};

export const UpdateTechnicalSpecialistTime = (editedRowData, updatedData) => async(dispatch, getstate) => {
    const state = getstate();
    const editedRow = Object.assign({},updatedData, editedRowData );
    const techSpecTime = state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTimes;
    let checkProperty = "visitTechnicalSpecialistAccountTimeId";
    if (editedRow.recordStatus === 'N') {
        checkProperty = "TechSpecTimeId";
    }
    const index = techSpecTime.findIndex(iteratedValue => iteratedValue[checkProperty] === editedRow[checkProperty]);
    
    const newState = Object.assign([], techSpecTime);
    if (index >= 0) {
        newState[index] = editedRow;
        dispatch(actions.UpdateTechnicalSpecialistTime(newState));
    }
};

export const DeleteTechnicalSpecialistTime = (data) => async(dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTimes);
    data.forEach(row => {
        newState.forEach((iteratedValue, index) => {
            if (iteratedValue.visitTechnicalSpecialistAccountTimeId === row.visitTechnicalSpecialistAccountTimeId) {
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
    dispatch(actions.DeleteTechnicalSpecialistTime(newState));
};

export const AddTechnicalSpecialistTravel = (data) => async(dispatch, getstate) => {
    dispatch(actions.AddTechnicalSpecialistTravel(data));
};

export const UpdateTechnicalSpecialistTravel = (editedRowData, updatedData) => async(dispatch, getstate) => {
    const state = getstate();
    const editedRow = Object.assign({},updatedData, editedRowData );
    const techSpecTravel = state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTravels;
    let checkProperty = "visitTechnicalSpecialistAccountTravelId";
    if (editedRow.recordStatus === 'N') {
        checkProperty = "TechSpecTravelId";
    }
    const index = techSpecTravel.findIndex(iteratedValue => iteratedValue[checkProperty] === editedRow[checkProperty]);
    
    const newState = Object.assign([], techSpecTravel);
    if (index >= 0) {
        newState[index] = editedRow;
        dispatch(actions.UpdateTechnicalSpecialistTravel(newState));
    }
};

export const DeleteTechnicalSpecialistTravel = (data) => async(dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTravels);
    data.forEach(row => {
        newState.forEach((iteratedValue, index) => {
            if (iteratedValue.visitTechnicalSpecialistAccountTravelId === row.visitTechnicalSpecialistAccountTravelId) {
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
    dispatch(actions.DeleteTechnicalSpecialistTravel(newState));
};

export const AddTechnicalSpecialistExpense = (data) => async(dispatch, getstate) => {
    dispatch(actions.AddTechnicalSpecialistExpense(data));
};

export const UpdateTechnicalSpecialistExpense = (editedRowData, updatedData) => async(dispatch, getstate) => {
    const state = getstate();
    const editedRow = Object.assign({},updatedData, editedRowData );
    const techSpecExpense = state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistExpenses;
    let checkProperty = "visitTechnicalSpecialistAccountExpenseId";
    if (editedRow.recordStatus === 'N') {
        checkProperty = "TechSpecExpenseId";
    }
    const index = techSpecExpense.findIndex(iteratedValue => iteratedValue[checkProperty] === editedRow[checkProperty]);
    
    const newState = Object.assign([], techSpecExpense);
    if (index >= 0) {
        newState[index] = editedRow;
        dispatch(actions.UpdateTechnicalSpecialistExpense(newState));
    }
};

export const DeleteTechnicalSpecialistExpense = (data) => async(dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistExpenses);    
    data.forEach(row => {
        newState.forEach((iteratedValue, index) => {
            if (iteratedValue.visitTechnicalSpecialistAccountExpenseId === row.visitTechnicalSpecialistAccountExpenseId) {
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
    dispatch(actions.DeleteTechnicalSpecialistExpense(newState));
};

export const AddTechnicalSpecialistConsumable = (data) => async(dispatch, getstate) => {
    dispatch(actions.AddTechnicalSpecialistConsumable(data));
};

export const UpdateTechnicalSpecialistConsumable = (editedRowData, updatedData) => async(dispatch, getstate) => {
    const state = getstate();
    const editedRow = Object.assign({},updatedData, editedRowData );
    const techSpecConsumable = state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistConsumables;
    let checkProperty = "visitTechnicalSpecialistAccountConsumableId";
    if (editedRow.recordStatus === 'N') {
        checkProperty = "TechSpecConsumableId";
    }
    const index = techSpecConsumable.findIndex(iteratedValue => iteratedValue[checkProperty] === editedRow[checkProperty]);
    
    const newState = Object.assign([], techSpecConsumable);
    if (index >= 0) {
        newState[index] = editedRow;
        dispatch(actions.UpdateTechnicalSpecialistConsumable(newState));
    }
};

export const DeleteTechnicalSpecialistConsumable = (data) => async(dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistConsumables);
    data.forEach(row => {
        newState.forEach((iteratedValue, index) => {
            if (iteratedValue.visitTechnicalSpecialistAccountConsumableId === row.visitTechnicalSpecialistAccountConsumableId) {
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
    dispatch(actions.DeleteTechnicalSpecialistConsumable(newState));
};

export const FetchCompanyExpectedMargin = () => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const companyCode = getNestedObject(getstate().rootVisitReducer.visitDetails,[ 'VisitInfo','visitContractCompanyCode' ]);
    if(companyCode){
        const url =StringFormat(companyAPIConfig.expectedMargins,companyCode);
        const response = await FetchData(url, {})
            .catch(error => {
                // console.error(error); // To show the error details in console
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                dispatch(HideLoader());
            });
        if (response && "1" === response.code) {
            const assignmentProjectBusinessUnit = getNestedObject(getstate().rootVisitReducer.visitDetails,[ 'VisitInfo','assignmentProjectBusinessUnit' ]);
            const expectedMarginObj =  arrayUtil.FilterGetObject(response.result,'marginType',assignmentProjectBusinessUnit);
            dispatch(actions.CompanyExpectedMargin(expectedMarginObj?expectedMarginObj.minimumMargin:null));
        }
    }
    dispatch(HideLoader());
};
export const UpdateTechSpecRateChanged = (data) => async(dispatch, getstate) => {
    dispatch(actions.UpdateTechSpecRateChanged(data));
};

export const FetchAssignmentAdditionalExpensesForVisit = (assignmentId) => async (dispatch, getstate) => {
    if (!assignmentId) {
        const visitInfo = isEmptyReturnDefault(getstate().rootVisitReducer.visitDetails.VisitInfo, 'object');
        assignmentId = visitInfo.visitAssignmentId;
    }
    if (!assignmentId) {
        return false;
    }
    const expenses = await dispatch(FetchAssignmentAdditionalExpenses(assignmentId));
    if (expenses && expenses.length > 0) {
            dispatch(actions.SaveAssignmentUnLInkedExpenses(expenses));
    }
};

export const SaveAssignmentUnLInkedExpenses = (unLinkedExpenses) => async (dispatch, getstate) => {
         dispatch(actions.SaveAssignmentUnLInkedExpenses(unLinkedExpenses));
};

export const SetLinkedAssignmentExpenses = (data)=> async(dispatch, getstate) => {
    dispatch(actions.SetLinkedAssignmentExpenses(data));
};

export const UpdateVisitStatusByLineItems = (data)=> async(dispatch, getstate) => {
    dispatch(actions.UpdateVisitStatusByLineItems(data));
};

export const FetchCurrencyExchangeRate = (currencyObj) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const contractNumber = getNestedObject(getstate().rootVisitReducer.visitDetails,[ 'VisitInfo','visitContractNumber' ]);
    const params = {
        exchangeRates:currencyObj,
        contractNumber:contractNumber
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