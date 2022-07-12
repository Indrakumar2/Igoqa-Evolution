import { visitActionTypes } from '../../constants/actionTypes';
import { visitAPIConfig, RequestPayload, assignmentAPIConfig } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData } from '../../services/api/baseApiService';
import {
    isEmptyReturnDefault,
    deepCopy,
    getlocalizeData,
    isEmpty,
    isEmptyOrUndefine,
    parseValdiationMessage
} from '../../utils/commonUtils';
import { ShowLoader, HideLoader } from '../../common/commonAction';
import arrayUtil from '../../utils/arrayUtil';
import { StringFormat } from '../../utils/stringUtil';
import moment from 'moment';

const localConstant = getlocalizeData();

const actions = {
    FetchVisitByID: (payload) => ({
        type: visitActionTypes.FETCH_VISIT_ID,
        data: payload
    }),
    FetchVisitStatus: (payload) => ({
        type: visitActionTypes.FETCH_VISIT_STATUS,
        data: payload
    }),
    FetchUnusedReason: (payload) => ({
        type: visitActionTypes.FETCH_UNUSED_REASON,
        data: payload
    }),
    AddUpdateGeneralDetails: (payload) => ({
        type: visitActionTypes.UPDATE_VISIT_DETAILS,
        data: payload
    }),
    FetchSupplierList: (payload) => ({
        type: visitActionTypes.FETCH_SUPPLIER_LIST,
        data: payload
    }),
    FetchTechnicalSpecialistList: (payload) => ({
        type: visitActionTypes.FETCH_TECHNICAL_SPECIALIST_LIST,
        data: payload
    }),
    SelectedVisitTechSpecs: (payload) => ({
        type: visitActionTypes.SELECTED_VISIT_TECHNICAL_SPECIALISTS,
        data: payload
    }),
    AddVisitTechnicalSpecialist: (payload) => ({
        type: visitActionTypes.VISIT_TECHNICAL_SPECIALIST_ADD,
        data: payload
    }),
    RemoveVisitTechnicalSpecialist: (payload) => ({
        type: visitActionTypes.VISIT_TECHNICAL_SPECIALIST_REMOVE,
        data: payload
    }),
    FetchAssignmentToAddVisit: (payload) => ({
        type: visitActionTypes.FETCH_ASSIGNMENT_TO_ADD_VISIT,
        data: payload
    }),
    UpdateVisitTechnicalSpecialist: (payload) => ({
        type: visitActionTypes.UPDATE_VISIT_TECHNICAL_SPECIALIST,
        data: payload
    }),
    FetchTechnicalSpecialist: (payload) => ({
        type: visitActionTypes.FETCH_VISIT_TECHNICAL_SPECIALIST,
        data: payload
    }),
    FetchTechSpecRateSchedules: (payload) => ({
        type: visitActionTypes.FETCH_VISIT_TECHNICAL_SPECIALIST_RATE_SCHEDULE,
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
    FetchTechSpecRateDefault: (payload) => ({
        type: visitActionTypes.ADD_VISIT_TECHNICAL_SPECIALIST_RATE_SCHEDULE,
        data: payload
    }),
    isTBAVisitStatus: (payload) => ({
        type: visitActionTypes.IS_TBA_VISIT_STATUS,
        data: payload
    }),

    //To add visit calendar data to store
    addVisitCalendarData: (payload) => ({
        type: visitActionTypes.ADD_CALENDAR_DATA,
        data: payload
    }),

    //To update visit calendar data to store
    updateVisitCalendarData: (payload) => ({
        type: visitActionTypes.UPDATE_CALENDAR_DATA,
        data: payload
    }),
    RemoveTSVisitCalendarData: (payload) => ({
        type: visitActionTypes.REMOVE_TS_CALENDAR_DATA,
        data: payload
    }),
};

export const FetchVisitByID = () => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    // if (!isEmpty(state.rootVisitReducer.visitDetails)) {
    //     return;
    // }
    //if (!state.rootVisitReducer.visitDetails) {
    const visitID = state.rootVisitReducer.selectedVisitData.visitId;
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
            dispatch(actions.FetchVisitByID(response.result));
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
    //}

    dispatch(HideLoader());
};

export const FetchVisitStatus = () => async (dispatch, getstate) => {
    if (!isEmpty(getstate().rootVisitReducer.visitStatus)) {
        return;
    }
    const assignmentVisitUrl = visitAPIConfig.visitStatus;
    const response = await FetchData(assignmentVisitUrl, {})
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.FAILED_FETCH_VISIT_STATUS, 'dangerToast assActassignVisitStatErrMsg');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response)) {
        if (response.code === '1') {
            dispatch(actions.FetchVisitStatus(response.result));
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.FAILED_FETCH_VISIT_STATUS, 'dangerToast assActassignVisitStatErrMsg1');
    }
};
export const FetchUnusedReason = () => async (dispatch, getstate) => {
    if (!isEmpty(getstate().rootVisitReducer.unusedReason)) {
        return;
    }
    const assignmentVisitUrl = visitAPIConfig.unusedReason;
    const response = await FetchData(assignmentVisitUrl, {})
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.FAILED_FETCH_VISIT_STATUS, 'dangerToast assActassignVisitStatErrMsg');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response)) {
        if (response.code === '1') {
            dispatch(actions.FetchUnusedReason(response.result));
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.FAILED_FETCH_UNUSED_REASON, 'dangerToast assActassignVisitStatErrMsg1');
    }
};
export const AddUpdateGeneralDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const visitInfo = state.rootVisitReducer.visitDetails.VisitInfo;
    const updatedData = Object.assign({}, visitInfo, data);
    dispatch(actions.AddUpdateGeneralDetails(updatedData));
};

export const FetchSupplierList = (isNewVisit) => async (dispatch, getstate) => {
    // if (!isEmpty(getstate().rootVisitReducer.supplierList)) {
    //     return;
    // }

    let assignmentId = 0;
    if (isNewVisit) {
        if (!isEmptyOrUndefine(getstate().rootVisitReducer.visitDetails) &&
            !isEmptyOrUndefine(getstate().rootVisitReducer.visitDetails.VisitInfo)) {
            assignmentId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;
        }
    } else {
        assignmentId = getstate().rootVisitReducer.selectedVisitData.visitAssignmentId;
    }

    const supplierListUrl = visitAPIConfig.visitBaseUrl + visitAPIConfig.assignment + assignmentId + visitAPIConfig.supplierList;
    const param = {
        AssignmentID: assignmentId
    };
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(supplierListUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.FAILED_FETCH_VISIT_STATUS, 'dangerToast assActassignVisitStatErrMsg');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response)) {
        if (response.code === '1') {
            dispatch(actions.FetchSupplierList(response.result));
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.FAILED_FETCH_VISIT_STATUS, 'dangerToast assActassignVisitStatErrMsg1');
    }
};

export const FetchTechnicalSpecialistList = (isNewVisit) => async (dispatch, getstate) => {
    // if (!isEmpty(getstate().rootVisitReducer.technicalSpecialistList)) {
    //     return;
    // }

    let assignmentId = 0;
    if (isNewVisit) {
        if (!isEmptyOrUndefine(getstate().rootVisitReducer.visitDetails) &&
            !isEmptyOrUndefine(getstate().rootVisitReducer.visitDetails.VisitInfo)) {
            assignmentId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;
        }
    } else {
        assignmentId = getstate().rootVisitReducer.selectedVisitData.visitAssignmentId;
    }

    const techSpecialistUrl = visitAPIConfig.visitBaseUrl + visitAPIConfig.assignment + assignmentId + visitAPIConfig.technicalSpecialistList;
    const param = {
        AssignmentId: assignmentId
    };
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(techSpecialistUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.FAILED_FETCH_VISIT_STATUS, 'dangerToast assActassignVisitStatErrMsg');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response)) {
        if (response.code === '1') {
            dispatch(actions.FetchTechnicalSpecialistList(response.result));
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.FAILED_FETCH_VISIT_STATUS, 'dangerToast assActassignVisitStatErrMsg1');
    }
};

export const SelectedVisitTechSpecs = (selectedTechSpecs) => (dispatch, getstate) => {
    dispatch(actions.SelectedVisitTechSpecs(selectedTechSpecs));
};

export const AddVisitTechnicalSpecialist = (techSpec) => async (dispatch, getstate) => {
    //const visitTechnicalSpecialists = isEmptyReturnDefault(getstate().rootVisitReducer.visitDetails.VisitTechnicalSpecialists);
    //don't add if TechnicalSpecialist already exists in reducer data
    //const isTSExists = visitTechnicalSpecialists.findIndex(ts => ts.pin === techSpec.pin);
    //isTSExists === -1 && dispatch(actions.AddVisitTechnicalSpecialist(techSpec));
    dispatch(actions.AddVisitTechnicalSpecialist(techSpec));
};

export const RemoveVisitTechnicalSpecialist = (techSpec) => async (dispatch, getstate) => {
    if (!isEmptyOrUndefine(getstate().rootVisitReducer.visitDetails)) {
        const visitTechnicalSpecialists = deepCopy(getstate().rootVisitReducer.visitDetails.VisitTechnicalSpecialists);
        if (visitTechnicalSpecialists && visitTechnicalSpecialists.length > 0) {
            for (let i = 0; i < visitTechnicalSpecialists.length; i++) {
                if (visitTechnicalSpecialists[i].pin === techSpec.pin) {
                    (visitTechnicalSpecialists[i].recordStatus === "N") ?
                        visitTechnicalSpecialists.splice(i, 1)
                        :
                        visitTechnicalSpecialists[i].recordStatus = "D";

                    //i--;
                }
            }
        }
        dispatch(actions.RemoveVisitTechnicalSpecialist(visitTechnicalSpecialists));
        dispatch(DeleteTechnicalSpecialistTime(techSpec.pin));
        dispatch(DeleteTechnicalSpecialistExpense(techSpec.pin));
        dispatch(DeleteTechnicalSpecialistTravel(techSpec.pin));
        dispatch(DeleteTechnicalSpecialistConsumable(techSpec.pin));        
    }
};

export const DeleteTechnicalSpecialistTime = (pin) => async (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTimes);
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
    dispatch(actions.DeleteTechnicalSpecialistTime(newState));
};

export const DeleteTechnicalSpecialistExpense = (pin) => async (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistExpenses);    
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
    dispatch(actions.DeleteTechnicalSpecialistExpense(newState));
};

export const DeleteTechnicalSpecialistTravel = (pin) => async (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTravels);
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
    dispatch(actions.DeleteTechnicalSpecialistTravel(newState));
};

export const DeleteTechnicalSpecialistConsumable = (pin) => async (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistConsumables);
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
    dispatch(actions.DeleteTechnicalSpecialistConsumable(newState));
};

export const UpdateVisitTechnicalSpecialist = (techSpec, ePin) => async (dispatch, getstate) => {
    dispatch(actions.UpdateVisitTechnicalSpecialist(techSpec));
    dispatch(UpdateTechSpecLineItmes(ePin));
};

export const UpdateTechSpecLineItmes = (ePin) => async (dispatch, getstate) => {    
    const state = getstate();
    const newStateTime = Object.assign([], state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTimes);
    if(newStateTime && newStateTime.length > 0) {
        for (let i = 0; i < newStateTime.length; i++) {
            if (parseInt(newStateTime[i].pin) === parseInt(ePin)) {
                newStateTime[i].recordStatus = "M";
            }
        }
    }
    dispatch(actions.UpdateTechnicalSpecialistTime(newStateTime));

    const newStateExpense = Object.assign([], state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistExpenses);    
    if(newStateExpense && newStateExpense.length > 0) {
        for (let i = 0; i < newStateExpense.length; i++) {
            if (parseInt(newStateExpense[i].pin) === parseInt(ePin)) {
                newStateExpense[i].recordStatus = "M";
            }
        }
    }
    dispatch(actions.UpdateTechnicalSpecialistExpense(newStateExpense));

    const newStateTravel = Object.assign([], state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTravels);
    if(newStateTravel && newStateTravel.length > 0) {
        for (let i = 0; i < newStateTravel.length; i++) {
            if (parseInt(newStateTravel[i].pin) === parseInt(ePin)) {
                newStateTravel[i].recordStatus = "M";
            }
        }
    }
    dispatch(actions.UpdateTechnicalSpecialistTravel(newStateTravel));

    const newStateConsumables = Object.assign([], state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistConsumables);
    if(newStateConsumables && newStateConsumables.length > 0) {
        for (let i = 0; i < newStateConsumables.length; i++) {
            if (parseInt(newStateConsumables[i].pin) === parseInt(ePin)) {
                newStateConsumables[i].recordStatus = "D";
            }
        }
    }
    dispatch(actions.UpdateTechnicalSpecialistConsumable(newStateConsumables));
};

export const FetchAssignmentForVisitCreation = (assignmentId) => (dispatch, getstate) => {

    let visitDetails = {};
    const state = getstate(),
        assignemtList = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentList);  
    if (!assignmentId)
        assignmentId = state.rootAssignmentReducer.selectedAssignmentId;
        const assignment = arrayUtil.FilterGetObject(assignemtList, 'assignmentId', assignmentId);

    if (!isEmptyOrUndefine(assignment)) {
        visitDetails = {
            VisitInfo: {
                visitAssignmentCreatedDate: assignment.assignmentCreatedDate,
                visitCustomerProjectName: assignment.assignmentCustomerProjectName,
                // //assignmentId: assignment.assignmentId,
                //visitId: null,
                visitAssignmentNumber: assignment.assignmentNumber,
                visitAssignmentId: assignment.assignmentId,
                visitSupplierPOId: assignment.assignmentSupplierPurchaseOrderId,
                visitSupplierPONumber: assignment.assignmentSupplierPurchaseOrderNumber,
                visitContractCompanyCode: assignment.assignmentContractHoldingCompanyCode,
                visitContractCompany: assignment.assignmentContractHoldingCompany,
                visitContractCoordinator: assignment.assignmentContractHoldingCompanyCoordinator,
                visitContractNumber: assignment.assignmentContractHoldingCompanyCoordinatorCode,
                visitCustomerCode: assignment.assignmentCustomerCode,
                visitCustomerName: assignment.assignmentCustomerName,
                visitOperatingCompanyCode: assignment.assignmentOperatingCompanyCode,
                visitOperatingCompany: assignment.assignmentOperatingCompany,
                visitOperatingCompanyCoordinator: assignment.assignmentOperatingCompanyCoordinator,
                //visitOperatingCoordinatorCode: assignment.assignmentOperatingCompanyCoordinatorCode,
                visitProjectNumber: assignment.assignmentProjectNumber,
                techSpecialists: assignment.techSpecialists,
                //assignmentClientReportingRequirements:assignment.clientReportingRequirements,
                //assignmentProjectBusinessUnit:assignment.assignmentProjectBusinessUnit
            },
            // VisitTechnicalSpecialists: null,
            // VisitTechnicalSpecialistConsumables: null,
            // VisitTechnicalSpecialistExpenses: null,
            // VisitTechnicalSpecialistTimes: null,
            // VisitTechnicalSpecialistTravels: null,
            // VisitReferences: null,
            // VisitDocuments: null,
            // VisitNotes: null,
            // VisitSupplierPerformances: null            
        };
    }
    dispatch(actions.FetchAssignmentToAddVisit(visitDetails));
};

export const FetchVisitTechnicalSpecialists = () => async (dispatch, getstate) => {
    const state = getstate();
    if (!isEmptyOrUndefine(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialists)) {
        return;
    }
    //const selectedTechSpecs = isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitInfo.techSpecialists);
    dispatch(ShowLoader());
    const visitId = state.rootVisitReducer.selectedVisitData.visitId;
    const url = visitAPIConfig.visits + StringFormat(visitAPIConfig.TechnicalSpecialist, visitId);
    // if(selectedTechSpecs.length > 0){
    //     //url = StringFormat(visitAPIConfig.technicalSpecialistGrossmargin, visitId);
    // }

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
};

export const FetchTechnicalSpecialistTime = (data) => async (dispatch, getstate) => {
    const state = getstate();
    // if (!isEmpty(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTimes)) {
    //     return;
    // }

    const visitID = state.rootVisitReducer.selectedVisitData.visitId;
    const url = StringFormat(visitAPIConfig.technicalSpecialistTime, visitID);
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

export const FetchTechnicalSpecialistTravel = (data) => async (dispatch, getstate) => {
    const state = getstate();
    // if (!isEmpty(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTravels)) {
    //     return;
    // }

    const visitID = state.rootVisitReducer.selectedVisitData.visitId; //186
    const url = StringFormat(visitAPIConfig.technicalSpecialistTravel, visitID);
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

export const FetchTechnicalSpecialistExpense = (data) => async (dispatch, getstate) => {
    const state = getstate();
    // if (!isEmpty(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistExpenses)) {
    //     return;
    // }

    const visitID = state.rootVisitReducer.selectedVisitData.visitId; //186
    const url = StringFormat(visitAPIConfig.technicalSpecilistExpense, visitID);
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

export const FetchTechnicalSpecialistConsumable = (data) => async (dispatch, getstate) => {
    const state = getstate();
    // if (!isEmpty(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistConsumables)) {
    //     return;
    // }

    const visitID = state.rootVisitReducer.selectedVisitData.visitId; //186
    const url = StringFormat(visitAPIConfig.technicalSpecialistConsumable, visitID);
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

// export const FetchTechSpecRateSchedulesDefault = (epin, techSpecId) => async (dispatch, getstate) => {
//     const state = getstate();
//     let assignmentId ;
//     if(state.rootVisitReducer.visitDetails.VisitInfo && 
//         state.rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId){
//             assignmentId = state.rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;
//         }
//     if(!assignmentId){
//         assignmentId = state.rootVisitReducer.selectedVisitData.visitAssignmentId;
//     }
//     dispatch(ShowLoader());
//     const url = StringFormat(visitAPIConfig.TechSpecRateSchedule, assignmentId);

//     const params = {
//         epin: epin //43115 //epin
//     };     
//     const requestPayload = new RequestPayload(params);     
//     const response = await FetchData(url, requestPayload)
//         .catch(error => {
//             dispatch(HideLoader());
//             IntertekToaster(localConstant.errorMessages.ERROR_FETCHING_TECHSPEC_RATE_SCHEDULES, 'dangerToast fetchTimesheetWrong');
//         });
//     if (response && "1" === response.code) {  
//         if(!isEmptyOrUndefine(response) && !isEmptyOrUndefine(response.result) && !isEmptyOrUndefine(response.result.chargeSchedules)) {
//             response.result.chargeSchedules.forEach(row => {
//                 row.chargeScheduleRates.forEach(rowData => {
//                     const tempData = {};
//                     tempData.expenseDate = rowData.effectiveFrom;
//                     tempData.chargeExpenseType = rowData.chargeType;
//                     tempData.chargeRateCurrency = rowData.currency;
//                     tempData.chargeRate = rowData.chargeRate;
//                     tempData.chargeRateDescription = rowData.description;
//                     tempData.pin = epin;
//                     tempData.visitTechnicalSpecialistId = techSpecId;
//                     if(rowData.type === 'R') {                      
//                         dispatch(actions.AddTechnicalSpecialistTime(tempData));                 
//                     } else if(rowData.type === 'T') {                    
//                         dispatch(actions.AddTechnicalSpecialistTravel(tempData));
//                     } else if(rowData.type === 'E') {                    
//                         dispatch(actions.AddTechnicalSpecialistExpense(tempData));
//                     } else if(rowData.type === 'C') {                    
//                         dispatch(actions.AddTechnicalSpecialistConsumable(tempData));
//                     }
//                 });            
//             });
//         }
//     }
//     dispatch(HideLoader());
// };

export const FetchTechSpecRateDefault = (epin) => async (dispatch, getstate) => {
    const state = getstate();
    let assignmentId;
    if (state.rootVisitReducer.visitDetails.VisitInfo &&
        state.rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId) {
        assignmentId = state.rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;
    }
    if (!assignmentId) {
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
        //dispatch(FetchTechSpecRateSchedulesDefault(epin, assignmentId, response));
    }
    dispatch(HideLoader());
};

export const FetchTechSpecRateSchedulesDefault = (epin, assignmentId, response) => async (dispatch, getstate) => {
    if (response && "1" === response.code) {
        if (!isEmptyOrUndefine(response) && !isEmptyOrUndefine(response.result) && !isEmptyOrUndefine(response.result.chargeSchedules)) {
            const visitStartDate = moment(getstate().rootVisitReducer.visitDetails.VisitInfo.visitStartDate);
            response.result.chargeSchedules.forEach(row => {
                row.chargeScheduleRates.forEach(rowData => {
                    if (moment(rowData.effectiveFrom).isSameOrAfter(visitStartDate, 'day')) {
                        const tempData = {};
                        tempData.expenseDate = rowData.effectiveFrom;
                        tempData.chargeExpenseType = rowData.chargeType;
                        tempData.chargeRateCurrency = rowData.currency;
                        //tempData.chargeRate = rowData.chargeRate;
                        tempData.chargeRateDescription = rowData.description;
                        tempData.pin = epin;
                        tempData.visitTechnicalSpecialistId = null;
                        tempData.recordStatus = 'N';
                        tempData.TechSpecTimeId = Math.floor(Math.random() * 99) - 100;
                        tempData.assignmentId = assignmentId;
                        if (rowData.type === 'R') {
                            dispatch(actions.AddTechnicalSpecialistTime(tempData));
                        } else if (rowData.type === 'T') {
                            dispatch(actions.AddTechnicalSpecialistTravel(tempData));
                        } else if (rowData.type === 'E') {
                            dispatch(actions.AddTechnicalSpecialistExpense(tempData));
                        } else if (rowData.type === 'C') {
                            dispatch(actions.AddTechnicalSpecialistConsumable(tempData));
                        }
                    }
                });
            });
        }
    }
};

export const isTBAVisitStatus = (visitStatus) => (dispatch, getstate) => {
    dispatch(actions.isTBAVisitStatus(visitStatus));
};

//To add visit calendar data to store
export const addVisitCalendarData = (calendarData, startDate, endDate) => (dispatch, getstate) => {
    const data = {
        "id": calendarData.id,
        "technicalSpecialistId": calendarData.resourceId,
        "companyId": calendarData.companyId,
        "companyCode": calendarData.companyCode,
        "calendarType": localConstant.globalCalendar.CALENDAR_STATUS.VISIT,
        "calendarRefCode": calendarData.calendarRefCode,
        "calendarStatus": calendarData.calendarStatus,
        "startDateTime": startDate,
        "endDateTime": endDate,
        "createdBy": calendarData.createdBy,
        "isActive": calendarData.isActive,
        "recordStatus": calendarData.recordStatus,
        "updateCount": calendarData.updateCount,
        "isActive": calendarData.resourceAllocation ? !calendarData.resourceAllocation : true,
        "start": calendarData.start,
        "end": calendarData.end,
        "description": calendarData.description
    };
    dispatch(actions.addVisitCalendarData(data));
};

//To add visit calendar data to store
export const updateVisitCalendarData = (calendarData) => (dispatch, getstate) => {
    dispatch(actions.updateVisitCalendarData(calendarData));
};

export const FetchVisitCalendarByID = (visitID) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    if (visitID) {
        const url = visitAPIConfig.visits + StringFormat(visitAPIConfig.GetVisitByID, visitID);
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
                return response.result;
            }
            else if (response.code === "41") {
                if (!isEmptyReturnDefault(response.validationMessages)) {
                    IntertekToaster(parseValdiationMessage(response), 'dangerToast conActDataSomethingWrong');
                }
            }
            else if (response.code === "11") {
                if (!isEmptyReturnDefault(response.messages)) {
                    IntertekToaster(parseValdiationMessage(response), 'dangerToast conActDataSomethingWrong');
                }
            }
            else {

            }
        }
        else {
            IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast OperatingCoordinatorSWWValPreAssign');
        }
    }
    dispatch(HideLoader());
};

export const FetchFinalVisitId = (isNewVisit) => async (dispatch, getstate) => {    
    let assignmentId = 0;
    if(isNewVisit) {
        if(!isEmptyOrUndefine(getstate().rootVisitReducer.visitDetails) &&
                !isEmptyOrUndefine(getstate().rootVisitReducer.visitDetails.VisitInfo)) {
            assignmentId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;
        }
    } else {
        assignmentId = getstate().rootVisitReducer.selectedVisitData.visitAssignmentId;
    }

    if(!assignmentId) assignmentId = getstate().rootVisitReducer.visitDetails.VisitInfo.visitAssignmentId;
    dispatch(ShowLoader());
    const url = visitAPIConfig.visitBaseUrl + visitAPIConfig.visits + visitAPIConfig.GetFinalVisitId;
    const param = {
        VisitAssignmentId: assignmentId
    };
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            dispatch(HideLoader());
            IntertekToaster(localConstant.validationMessage.SOMETHING_WENT_WRONG, 'dangerToast validationData');
            return null;
        });
    if (!isEmpty(response)) {
        dispatch(HideLoader());
        if (response.code === '1') {            
            return response.result;
        }
    }
    else {
        dispatch(HideLoader());
        IntertekToaster(localConstant.validationMessage.SOMETHING_WENT_WRONG, 'dangerToast validationData');
        return null;
    }    
    return null;
};

export const RemoveCalendarForTechSpec = (techspecId) => async (dispatch, getstate) => {
    dispatch(actions.RemoveTSVisitCalendarData(techspecId));
};