import { visitActionTypes } from '../../constants/actionTypes';
import { visitAPIConfig, RequestPayload, masterData } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData } from '../../services/api/baseApiService';
import { getlocalizeData,isEmpty,isEmptyReturnDefault } from '../../utils/commonUtils';
import { ShowLoader, HideLoader } from '../../common/commonAction';
import { StringFormat } from '../../utils/stringUtil';

const localConstant = getlocalizeData();

const actions = {
    FetchSupplierPerformance: (payload) => ({
        type: visitActionTypes.FETCH_VISIT_SUPPLIER_PERFORMANCE,
        data: payload
    }),
    AddSupplierPerformance: (payload) => ({
        type: visitActionTypes.ADD_SUPPLIER_PERFORMANCE,
        data: payload
    }),
    UpdateSupplierPerformance: (payload) => ({
        type: visitActionTypes.UPDATE_SUPPLIER_PERFORMANCE,
        data: payload
    }),
    DeleteSupplierPerformance: (payload) => ({
        type: visitActionTypes.DELETE_SUPPLIER_PERFORMANCE,
        data: payload
    }),
    FetchSupplierPerformanceType:(payload) => ({
        type: visitActionTypes.FETCH_SUPPLIER_PERFORMANCE_TYPE,
        data: payload
    })  
};

export const FetchSupplierPerformance = () => async(dispatch, getstate) => {
    const state = getstate();
    if (!isEmpty(state.rootVisitReducer.visitDetails.SupplierPerformance)) {
        return;
    }
    dispatch(ShowLoader());
    
    const visitID = state.rootVisitReducer.selectedVisitData.visitId;
    const url = visitAPIConfig.visitBaseUrl + visitAPIConfig.visits + StringFormat(visitAPIConfig.SupplierPerformance, visitID);
    const param = {
        VisitId: visitID
    };
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.validationMessage.FETCH_VISIT_SUPPLIER_PERFORMANCE, 'wariningToast');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });

        if (!isEmpty(response)) {
            if (response.code === "1") {                
                dispatch(actions.FetchSupplierPerformance(response.result));
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

export const AddSupplierPerformance = (data) => (dispatch, getstate) => {    
    dispatch(actions.AddSupplierPerformance(data));
};

export const UpdateSupplierPerformance = (editedRowData, updatedData) => (dispatch, getstate) => {
    const state = getstate();
    const editedRow = Object.assign({},updatedData, editedRowData );
    const references = state.rootVisitReducer.visitDetails.VisitSupplierPerformances;
    let checkProperty = "visitSupplierPerformanceTypeId";
    if (editedRow.recordStatus === 'N') {
        checkProperty = "supplierPerformanceAddUniqueId";
    }
    const index = references.findIndex(iteratedValue => iteratedValue[checkProperty] === editedRow[checkProperty]);
    
    const newState = Object.assign([], references);
    if (index >= 0) {
        newState[index] = editedRow;
        dispatch(actions.UpdateSupplierPerformance(newState));
    }
};

export const DeleteSupplierPerformance = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootVisitReducer.visitDetails.VisitSupplierPerformances);
    data.forEach(row => {
        newState.forEach((iteratedValue, index) => {
            if (iteratedValue.visitSupplierPerformanceTypeId === row.visitSupplierPerformanceTypeId) {
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    const delIndex = newState.findIndex(value => (value.supplierPerformanceAddUniqueId === row.supplierPerformanceAddUniqueId));
                    if (delIndex >= 0)
                        newState.splice(delIndex, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteSupplierPerformance(newState));
};

export const FetchSupplierPerformanceType = () => async (dispatch, getstate) => {
    const profileActionUrl = masterData.baseUrl + masterData.supplierPerformanceType;
    const isActive= true;
    const params = {
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(profileActionUrl, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console
        //IntertekToaster(localConstant.validationMessage.TECHNICAL_MANAGER_VALIDATION, 'dangerToast mstDtActProfileAction');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
    if (response && response.code === "1") {
        dispatch(actions.FetchSupplierPerformanceType(response.result));
    }
    else {
        IntertekToaster(localConstant.techSpec.common.WENT_WRONG, 'dangerToast mstDtActProfileActionSmtWrng'); 
    }
};