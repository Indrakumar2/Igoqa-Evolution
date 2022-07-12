import { processApiRequest } from '../../services/api/defaultServiceApi';
import { contractActionTypes, masterDataActionTypes, batchProcessActionTypes } from '../../constants/actionTypes';
import { FetchData, PostData } from '../../services/api/baseApiService';
import { contractAPIConfig, RequestPayload } from '../../apiConfig/apiConfig';
import MaterializeComponent from 'materialize-css';
import { ShowLoader, HideLoader } from '../../common/commonAction';
import { required } from '../../utils/validator';
import { getlocalizeData,parseValdiationMessage } from '../../utils/commonUtils';
import IntertekToaster from '../../common/baseComponents/intertekToaster';

const localConstant = getlocalizeData();

const actions = {
    RateScheduleEditCheck: (payload) => (
        {
            type: contractActionTypes.rateScheduleActionTypes.IS_RATESCHEDULE_EDIT,
            data: payload
        }
    ),
    ChargeTypeEditCheck: (payload) => (
        {
            type: contractActionTypes.rateScheduleActionTypes.IS_CHARGETYPE_EDIT,
            data: payload
        }
    ),
    RateScheduleModalState: (payload) => (
        {
            type: contractActionTypes.rateScheduleActionTypes.IS_RATESCHEDULE_OPEN,
            data: payload
        }
    ),
    ChargeTypeModalState: (payload) => (
        {
            type: contractActionTypes.rateScheduleActionTypes.IS_CHARGETYPE_OPEN,
            data: payload
        }
    ),
    CopyChargeTypeModalState: (payload) => (
        {
            type: contractActionTypes.rateScheduleActionTypes.IS_COPY_CHARGETYPE_OPEN,
            data: payload
        }
    ),
    AdminContractRatesModalState: (payload) => (
        {
            type: contractActionTypes.rateScheduleActionTypes.IS_ADMIN_CONTRACT_RATES_OPEN,
            data: payload
        }
    ),
    FetchRateSchedule: (payload) => (
        {
            type: contractActionTypes.rateScheduleActionTypes.FETCH_RATE_SCHEDULE,
            data: payload
        }
    ),
    AddRateSchedule: (payload) => (
        {
            type: contractActionTypes.rateScheduleActionTypes.ADD_RATE_SCHEDULE,
            data: payload
        }
    ),
    EditRateSchedule: (payload) => (
        {
            type: contractActionTypes.rateScheduleActionTypes.EDIT_RATE_SCHEDULE,
            data: payload
        }
    ),
    UpdateRateSchedule: (payload) => (
        {
            type: contractActionTypes.rateScheduleActionTypes.UPDATE_RATE_SCHEDULE,
            data: payload
        }
    ),
    DeleteRateSchedule: (payload) => (
        {
            type: contractActionTypes.rateScheduleActionTypes.DELETE_RATE_SCHEDULE,
            data: payload
        }
    ),
    AddChargeType: (payload) => (
        {
            type: contractActionTypes.rateScheduleActionTypes.ADD_CHARGE_TYPE,
            data: payload
        }
    ),
    EditChargeType: (payload) => (
        {
            type: contractActionTypes.rateScheduleActionTypes.EDIT_CHARGE_TYPE,
            data: payload
        }
    ),
    UpdateChargeType: (payload) => (
        {
            type: contractActionTypes.rateScheduleActionTypes.UPDATE_CHARGE_TYPE,
            data: payload
        }
    ),
    DeleteChargeType: (payload) => (
        {
            type: contractActionTypes.rateScheduleActionTypes.DELETE_CHARGE_TYPE,
            data: payload
        }
    ),
    FetchAdminChargeRates: (payload) => ({
        type: masterDataActionTypes.FETCH_ADMIN_CHARGE_RATE,
        data: payload
    }),
    FetchAdminInspectionGroup: (payload) => ({
        type: masterDataActionTypes.FETCH_ADMIN_INSPECTION_GROUP,
        data: payload
    }),
    FetchAdminInspectionType: (payload) => ({
        type: masterDataActionTypes.FETCH_ADMIN_INSPECTION_TYPE,
        data: payload
    }),
    AdminChargeRateSelect: (payload) => ({
        type: contractActionTypes.rateScheduleActionTypes.ADMIN_CHARGE_RATE_SELECT,
        data: payload
    }),
    AdminChargeScheduleValueChange: (payload) => ({
        type: contractActionTypes.rateScheduleActionTypes.ADMIN_CHARGE_RATE_CHANGE,
        data: payload
    }),
    ClearAdminChargeScheduleValueChange: () => ({
        type: contractActionTypes.rateScheduleActionTypes.CLEAR_ADMIN_CHARGE_RATE_CHANGE,
    }),
    ChargeRates: (payload) => ({
        type: contractActionTypes.rateScheduleActionTypes.CHARGE_RATES,
        data: payload
    }),
    UpdateChargeRates: (payload) => ({
        type: contractActionTypes.rateScheduleActionTypes.UPDATE_CHARGE_RATES,
        data: payload
    }),
    UpdateSelectedSchedule: (payload) => ({
        type: contractActionTypes.rateScheduleActionTypes.UPDATE_SELECTED_SCHEDULE,
        data: payload
    }),
    fetchBatchProcessData: (payload) => ({
        type: batchProcessActionTypes.FETCH_BATCH_PROCESS,
        data: payload
    }),
    isDuplicateFrameworkContractSchedules: (payload) => ({
        type: contractActionTypes.rateScheduleActionTypes.IS_DUPLICATE_FRAMEWORK_SCHEDULES,
        data: payload
    }),
};

/**
 * Rate Schedule Edit Button Toggle Action
 * @param {*Boolean} data 
 */
export const RateScheduleEditCheck = (data) => (dispatch, getstate) => {
    dispatch(actions.RateScheduleEditCheck(data));
};

export const FetchRateSchedule = (data) => (dispatch, getstate) => {
    dispatch(actions.FetchRateSchedule(data));
};

/**
 * Charge Type Edit Button Toggle Action
 * @param {*Boolean} data 
 */
export const ChargeTypeEditCheck = (data) => (dispatch, getstate) => {
    dispatch(actions.ChargeTypeEditCheck(data));
};

/**
 * Rate Schedule Modal Toggle Action
 * @param {*Boolean} data 
 */
export const RateScheduleModalState = (data) => (dispatch, getstate) => {
    dispatch(actions.RateScheduleModalState(data));
};

/**
 * Charge Type Modal Toggle Action
 * @param {*Boolean} data 
 */
export const ChargeTypeModalState = (data) => (dispatch, getstate) => {
    dispatch(actions.ChargeTypeModalState(data));
};

/**
 * Copy Charge Type Modal Toggle Action
 * @param {*Boolean} data 
 */
export const CopyChargeTypeModalState = (data) => (dispatch, getstate) => {
    dispatch(actions.CopyChargeTypeModalState(data));
};

/**
 * Admin Contract Rates Modal Toggle Action
 * @param {*Boolean} data 
 */
export const AdminContractRatesModalState = (data) => (dispatch, getstate) => {
    // if(data == false){
    //     dispatch(actions.FetchAdminInspectionGroup([]));
    //     dispatch(actions.FetchAdminInspectionType([]));
    //     dispatch(actions.FetchAdminChargeRates([]));
    // }
    dispatch(actions.AdminContractRatesModalState(data));
};

/**
 * Add New Rate Schedule Action
 * @param {*Object} data 
 */
export const AddRateSchedule = (data) => (dispatch, getstate) => {
    dispatch(actions.AddRateSchedule(data));
};

/**
 * Edit Rate Schedule Action
 * @param {*Object} data 
 */
export const EditRateSchedule = (data) => (dispatch, getstate) => {
    // TODO: Dispacth an action that stores the selected rate schedule record for edit
    dispatch(actions.EditRateSchedule(data));
};

/**
 * Update Rate Schedule Action
 * @param {*Object} data 
 */
export const UpdateRateSchedule = (data) => (dispatch, getstate) => {
    // TODO: Dispatch an action Update the edited Rate Schedule data in store
    const state = getstate();
    const scheduleData = Object.assign([], state.RootContractReducer.ContractDetailReducer.contractDetail.ContractSchedules);
    const chargeTypeData = Object.assign([], state.RootContractReducer.ContractDetailReducer.contractDetail.ContractScheduleRates);
    if (data.oldScheduleName !== data.scheduleName) {
        scheduleData.map(iteratedValue => {
            if (!required(iteratedValue.baseScheduleName) && iteratedValue.baseScheduleName == data.oldScheduleName) {   //Added required check for D-789 Performance Issue 
                iteratedValue.baseScheduleName = data.scheduleName;
                if (iteratedValue.recordStatus !== 'N')
                    iteratedValue.recordStatus = 'M';
            }
        });
        chargeTypeData.map(iteratedValue => {
            if (iteratedValue.scheduleName == data.oldScheduleName) {
                iteratedValue.scheduleName = data.scheduleName;
                if (iteratedValue.recordStatus !== 'N')
                    iteratedValue.recordStatus = 'M';
            }
            if (!required(iteratedValue.baseScheduleName) && iteratedValue.baseScheduleName == data.oldScheduleName) { //Added required check for D-789 Performance Issue 
                iteratedValue.baseScheduleName = data.scheduleName;
                if (iteratedValue.recordStatus !== 'N')
                    iteratedValue.recordStatus = 'M';
            }
        });
        dispatch(actions.UpdateRateSchedule(scheduleData));
        dispatch(actions.UpdateChargeType(chargeTypeData));
    }

    const editedRow = Object.assign({}, state.RootContractReducer.ContractDetailReducer.rateScheduleEditData, data);
    const index = state.RootContractReducer.ContractDetailReducer.contractDetail.ContractSchedules.findIndex(iteratedValue => iteratedValue.scheduleId === editedRow.scheduleId);
    const newState = Object.assign([], state.RootContractReducer.ContractDetailReducer.contractDetail.ContractSchedules);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateRateSchedule(newState));
    }
};

/**
 * Delete Rate Schedule Action
 * @param {*String} data 
 */
export const DeleteRateSchedule = (data) => (dispatch, getstate) => {
    // TODO: Dispatch action to delete the rate schedule from store
    const state = getstate();
    const newState = state.RootContractReducer.ContractDetailReducer.contractDetail.ContractSchedules;
    data.forEach(item => {
        newState.forEach((iteratedValue, index) => {
            if (iteratedValue.scheduleName === item.scheduleName && iteratedValue.scheduleId === item.scheduleId) {
                if (iteratedValue.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    newState.splice(index, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteRateSchedule(newState));
};

/**
 * Add New Charge Type Action
 * @param {*Object} data 
 */
export const AddChargeType = (data) => (dispatch, getstate) => {
    //TODO: dispatch action to add new charge type
    // const chargeType = Object.assign([],getstate().RootContractReducer.ContractDetailReducer.contractDetail.ContractScheduleRates);
    // chargeType.push(data);
    // dispatch(actions.AddChargeType(chargeType));
    dispatch(actions.AddChargeType(data));
};

/**
 * Edit Charge Type Action
 * @param {*Object} data 
 */
export const EditChargeType = (data) => (dispatch, getstate) => {
    // TODO: Dispacth an action that stores the selected charge type record for edit
    dispatch(actions.EditChargeType(data));
};

export const AdminChargeScheduleValueChange = (data) => (dispatch, getstate) => {
    const state = getstate();
    //const finalisedData=Object.assign({},state.RootContractReducer.ContractDetailReducer.AdminChargeScheduleValueChange,data);
    dispatch(actions.AdminChargeScheduleValueChange(data));
};

export const ClearAdminChargeScheduleValueChange = () => (dispatch) => {
    dispatch(actions.ClearAdminChargeScheduleValueChange());
};

/**
 * Update Charge Type Action
 * @param {*Object} data 
 */
export const UpdateChargeType = (data) => async (dispatch, getstate) => {
    // TODO: Dispatch an action Update the edited Charge Type data in store
    const state = getstate();
    const editedRow = Object.assign({}, state.RootContractReducer.ContractDetailReducer.chargeTypeEditData, data);
    const index = state.RootContractReducer.ContractDetailReducer.contractDetail.ContractScheduleRates.findIndex(iteratedValue => iteratedValue.rateId === editedRow.rateId);
    const newState = Object.assign([], state.RootContractReducer.ContractDetailReducer.contractDetail.ContractScheduleRates);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateChargeType(newState));
    }
    return true;
};

/**
 * Delete Charge Type Action
 * @param {*Object} data 
 */
export const DeleteChargeType = (data) => (dispatch, getstate) => {
    // TODO: Dispatch action to delete the charge type from store
    const state = getstate();
    const newState = state.RootContractReducer.ContractDetailReducer.contractDetail.ContractScheduleRates;
    data.map(row => {
        newState.map((iteratedValue, index) => {
            if (iteratedValue.rateId === row.rateId) {
                if (iteratedValue.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    newState.splice(index, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteChargeType(newState));
};

/**
 * Copy the selected rate schedule and store it as new data.
 * @param {*String} data 
 */
export const CopyRateSchedule = (data) => (dispatch, getstate) => {
    const state = getstate();

};

export const AdminRateScheduleSelect = (data, type) => (dispatch, getstate) => {
    let index = -1;
    if (data) {
        const adminChargeRate = Object.assign([], getstate().masterDataReducer.adminChargeRate);
        const adminRatesToCopy = Object.assign([], getstate().RootContractReducer.ContractDetailReducer.adminRateToCopy);
        const selectedData = adminChargeRate.filter(value => value.id == data.id);
        const updatedAdminChargeRate = Object.assign({}, selectedData[0], { "selectedChargeValue": type });
        index = adminRatesToCopy.findIndex(iteratedValue => iteratedValue.id == data.id);
        if (type !== "") {
            if (index != -1) {
                adminRatesToCopy[index] = updatedAdminChargeRate;
            }
            else {
                adminRatesToCopy.push(updatedAdminChargeRate);
            }
        }
        else {
            adminRatesToCopy.splice(index, 1);
        }
        dispatch(actions.AdminChargeRateSelect(adminRatesToCopy));
    }
    else {
        dispatch(actions.AdminChargeRateSelect([]));
    }
};
export const AdminRateScheduleSelectAll = (checked, type) => (dispatch, getstate) => {
    const state = getstate();
    let adminRatesToCopy = state.RootContractReducer.ContractDetailReducer.adminChargeRatesValue;
    if (checked) {
        adminRatesToCopy.forEach(iteratedValue => {
            iteratedValue.selectedChargeValue = type;
        });
        adminRatesToCopy = adminRatesToCopy.filter((x, i) => i !== 0);
    }
    else {
        adminRatesToCopy = [];
    }
    dispatch(actions.AdminChargeRateSelect(adminRatesToCopy));
};
export const ChargeRates = (data) => (dispatch, getstate) => {
    if (data.length > 0) {
        const state = getstate();
        const adminChargeRates = state.RootContractReducer.ContractDetailReducer.adminChargeRatesValue;
        adminChargeRates.forEach(iteratedValue => {
            iteratedValue.isRateOnShoreOil = false;
            iteratedValue.isRateOnShoreNonOil = false;
            iteratedValue.isRateOffShoreOil = false;
        });
        //Added for D112 - To avoid default (fom data)- Start
        data.forEach(iteratedValue => {
            iteratedValue.isRateOnShoreOil = false;
            iteratedValue.isRateOnShoreNonOil = false;
            iteratedValue.isRateOffShoreOil = false;
        });
        //Added for D112 - To avoid default - Start
        dispatch(actions.UpdateChargeRates(adminChargeRates));
    }
    dispatch(actions.ChargeRates(data));
    // dispatch(actions.ChargeRates(data));
};
export const UpdateAllChargeRates = (ratesType, checked) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    const chargeRates = state.RootContractReducer.ContractDetailReducer.adminChargeRatesValue;
    dispatch(actions.UpdateChargeRates([]));
    chargeRates.forEach(iteratedValue => {
        iteratedValue.isRateOnShoreOil = (ratesType === 'rateOnShoreOil' ? checked : false);
        iteratedValue.isRateOnShoreNonOil = (ratesType === 'rateOnShoreNonOil' ? checked : false);
        iteratedValue.isRateOffShoreOil = (ratesType === 'rateOffShoreOil' ? checked : false);
    });
    dispatch(actions.UpdateChargeRates(chargeRates));
    dispatch(HideLoader());
    return true;
};
export const UpdateChargeRatesTypes = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    const chargeRates = state.RootContractReducer.ContractDetailReducer.adminChargeRatesValue;
    const index = chargeRates.findIndex(iteratedValue => iteratedValue.id === data.id);
    const newState = Object.assign([], chargeRates);
    if (index >= 0) {
        newState[index] = data;

        const rateOnShoreOilData = newState.filter((x, i) => !x.isRateOnShoreOil && i !== 0);
        const rateOnShoreNonOilData = newState.filter((x, i) => !x.isRateOnShoreNonOil && i !== 0);
        const rateOffShoreOilData = newState.filter((x, i) => !x.isRateOffShoreOil && i !== 0);

        newState[0].isRateOnShoreOil = (rateOnShoreOilData.length === 0 ? true : false);
        newState[0].isRateOnShoreNonOil = (rateOnShoreNonOilData.length === 0 ? true : false);
        newState[0].isRateOffShoreOil = (rateOffShoreOilData.length === 0 ? true : false);
        dispatch(actions.UpdateChargeRates(newState));
    }
    dispatch(HideLoader());
    return true;
};

export const ClearAdminContractRates = () => (dispatch) => {
    dispatch(actions.AdminChargeRateSelect([]));
    dispatch(actions.ChargeRates([]));
    dispatch(actions.FetchAdminInspectionGroup([]));
    dispatch(actions.FetchAdminInspectionType([]));
    dispatch(actions.AdminChargeScheduleValueChange({}));
    dispatch(actions.AdminContractRatesModalState(false));
    // dispatch(actions.FetchAdminChargeRates([])); 
};

export const ClearChargeRates = () => (dispatch) => {
    dispatch(actions.ChargeRates([]));
};

export const UpdateSelectedSchedule = (data) => (dispatch) => {
    dispatch(actions.UpdateSelectedSchedule(data));
};

export const isRelatedFrameworkExisits = (data) => async (dispatch) => {
    if (data) {
        const params = { "contractId": data };
        const requestPayload = new RequestPayload(params);
        const url = contractAPIConfig.isRFCExists;
        const response = await FetchData(url, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.validationMessage.RELATED_FRAMEWORK_CHECK_FAILED, 'dangerToast relatedFrameworkCheckFailed');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                dispatch(HideLoader());
                return false;
            });
        if (response && response.code === "1") {
            return true;
        } else if (response.code === "11" || response.code === "41" || response.code === "31") {
            IntertekToaster(parseValdiationMessage(response), 'warningToast relatedFrameworkCheckFailed');
            return false;
        }
        else {
            IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast relatedFrameworkCheckFailed');
            return false;
        }
    } else {
        return false;
    }
};

export const isDuplicateFrameworkContractSchedules = (data) => async (dispatch) => {
    if (data) {
        const params = { "contractId": data };
        const requestPayload = new RequestPayload(params);
        const url = contractAPIConfig.isDuplicateFrameworkSchedules;
        const response = await FetchData(url, requestPayload)
            .catch(error => {
                // console.error(error);
                // IntertekToaster(localConstant.validationMessage.IS_DUPLICATE_FRAMEWORK_SCHEDULES_CHECK_FAILED, 'dangerToast duplicateFrameworkSchedulesCheckFailed');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                dispatch(HideLoader());
                return false;
            });
        if (response && response.code === "1") {
           return response;
        } else if (response.code === "11" || response.code === "41" || response.code === "31") {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast duplicateFrameworkSchedulesCheckFailed');
            return false;
        } else {
            IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast duplicateFrameworkSchedulesCheckFailed');
            return false;
        }
    } else {
            return false;
    }
};

export const checkIfBatchProcessCompleted = (data) => async (dispatch) => {
    if(data){
        const batchId = data;
        const url = contractAPIConfig.getBatchStatus + "?aintBatchId=" + batchId;
        processApiRequest(url, { method: 'GET' })
        .then(response => { 
            if (response && response.data && response.data.code && response.data.code == "1") {
                const batchData = response.data.result;
                if(batchData.processStatus == 2){
                    sessionStorage.removeItem('BatchId');
                    sessionStorage.setItem('showMessage', batchData.batchStatus);
                    const link = document.querySelector("link[rel*='icon']") || document.createElement('link');
                    link.type = 'image/x-icon';
                    link.rel = 'shortcut icon';
                    link.href = '/favicon.ico';
                    if (document.hidden) {
                        link.href = '/newfavicon.ico';
                    }
                    document.getElementsByTagName('head')[0].appendChild(link);
                    IntertekToaster(localConstant.validationMessage.RELATED_FRAMEWORK_COPY_SUCCESSFULL, 'successToast batchSuccess');        
                }
                else if(batchData.processStatus == 0 || batchData.processStatus == 1){
                    setTimeout(() => {
                        dispatch(checkIfBatchProcessCompleted(batchId));
                    }, 30000);
                }
                else if(batchData.processStatus == 3){
                    sessionStorage.removeItem('BatchId');
                    sessionStorage.setItem('showMessage', batchData.batchStatus);
                    const errorMessage = batchData.errorMessage;
                    IntertekToaster(errorMessage, 'dangerToast batchError');
                }
                dispatch(actions.fetchBatchProcessData(batchData));
            }
            else {
                IntertekToaster(localConstant.validationMessage.RELATED_FRAMEWORK_BATCH_ERROR, 'dangerToast CurrencyApiWrong');
            }
        }).catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(error, 'dangerToast batchError');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    }
};

export const frameworkScheduleCopyBatch = (data) => async (dispatch) => {
    const contractBatchId = localConstant.batchConstant.contractBatchId;
    if (data) {
        const requestPayload = new RequestPayload();
        const url = contractAPIConfig.rfcCopyBatch + "?" + "aintBatchID=" + contractBatchId + "&aintParamID=" + data;
        const response = await PostData(url, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.validationMessage.RELATED_FRAMEWORK_BATCH_FAILED, 'dangerToast relatedFrameworkBatchFailed');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                return false;
            });
        if (response && response.code === "1") {
            const batchData = response.result;
            if(batchData){
                const Id = batchData.id;
                sessionStorage.setItem('BatchId', Id);
                dispatch(actions.fetchBatchProcessData(response.result));
                IntertekToaster(localConstant.validationMessage.RELATED_FRAMEWORK_BATCH_INITIATED, 'successToast relatedFrameworkBatchFailed');
            }
            return true;
        } else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast relatedFrameworkBatchFailed');
            return false;
        }
        else {
            IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast relatedFrameworkBatchFailed');
            return false;
        }
    } else {
        IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast relatedFrameworkBatchFailed');
        return false;
    }
};