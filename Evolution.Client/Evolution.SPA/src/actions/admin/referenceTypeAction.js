import { RequestPayload, adminAPIConfig, masterData } from '../../apiConfig/apiConfig';
import { FetchData, PostData } from '../../services/api/baseApiService';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { parseValdiationMessage ,isEmpty } from '../../utils/commonUtils';
import { getlocalizeData } from '../../utils/commonUtils';
import { adminActionTypes } from '../../constants/actionTypes';
import { ShowLoader, HideLoader } from '../../common/commonAction';
import {
    FailureAlert,
    ValidationAlert,
    CreateAlert
} from '../../components/viewComponents/customer/alertAction';
// import { StringFormat } from '../../utils/stringUtil';
const localConstant = getlocalizeData();

const actions = {
    AddReferenceTypeDetails: (payload) => ({
        type: adminActionTypes.referenceType.ADD_REFERENCE_TYPE,
        data: payload
    }),
    FetchReferenceTypeDetails: (payload) => ({
        type: adminActionTypes.referenceType.FETCH_REFERENCE_TYPE,
        data: payload
    }),
    UpdateReferenceTypeDetails: (payload) => ({
        type: adminActionTypes.referenceType.UPDATE_REFERENCE_TYPE,
        data: payload
    }),
    DeleteReferenceTypeDetails: (payload) => ({
        type: adminActionTypes.referenceType.DELETE_REFERENCE_TYPE,
        data: payload
    }),
    FetchLanguges: (payload) => ({
        type: adminActionTypes.referenceType.FETCH_LANGUAGES,
        data: payload
    })
};
export const FetchReferenceTypeDetails = () => async (dispatch, getstate) => {
    const Url = "http://localhost:5101/api/master/30/data";
    const params = {
        // masterdatatypeId:30
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast viewRole');
        });
    if (response && response.code == "1") {
        dispatch(actions.FetchReferenceTypeDetails(response.result));
        return response.result;
    }
    else {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast viewRoleSomthingWrong');
    }
};
export const FetchLanguges = () => async (dispatch, getstate) => {
    const languageUrl = "http://localhost:5101/api/master/53/data";
    const params = {
        // masterdatatypeId:53
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(languageUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast viewRole');
        });
    if (response && response.code == "1") {
        dispatch(actions.FetchLanguges(response.result));
        return response.result;
    }
    else {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast viewRoleSomthingWrong');
    }
};
export const AddReferenceTypeDetails = (data) => async (dispatch, getstate) => {
    dispatch(actions.AddReferenceTypeDetails(data));
};
export const UpdateReferenceTypeDetails = (data, editedData) => async (dispatch, getstate) => {
    const state = getstate();
    const editedRow = Object.assign({}, editedData, data);
    const index = state.rootAdminReducer.referenceTypeData.findIndex(refType => refType.id === editedRow.id);
    const newState = Object.assign([], state.rootAdminReducer.referenceTypeData);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateReferenceTypeDetails(newState));
    }
};
export const DeleteReferenceTypeDetails = (data) => async (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootAdminReducer.referenceTypeData);
    data.map(row => {
        newState.map(refType => {
            if (refType.id === row.id) {
                const index = newState.findIndex(value => (value.id === row.id));
                newState[index].recordStatus = "D";
            }
        });
    });
    dispatch(actions.DeleteReferenceTypeDetails(newState));
};
export const CancelReferenceTypeChanges=(data)=>(dispatch)=>{
    dispatch(FetchReferenceTypeDetails());
};
export const SaveReferenceTypeDetails = () => async (dispatch, getstate) => {
    // dispatch(ShowLoader());
    const state = getstate();
    const modifiedData= [];
    const referenceTypeData = state.rootAdminReducer.referenceTypeData;
    if (referenceTypeData) {
       referenceTypeData.map(row => {
            if (row.recordStatus === "N") {
                row.id = 0;
            }
            if(row.recordStatus!==null){
                modifiedData.push(row);
            }
            return modifiedData;
        });
    }
    const saveUrl = "http://localhost:5101/api/master/30/data";
    const requestPayload = new RequestPayload(modifiedData);

    const response = await PostData(saveUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.SUPPLIER_SAVE_VALIDATION, 'warningToast supplierSaveVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast viewRole');
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == "1") {
            dispatch(CreateAlert(response, "ReferenceType"));
            return response;
        }
        else if (response.code == "41") {
            if (response.validationMessages.length > 0) {
                response.validationMessages.forEach((result, index) => {
                    if (result.messages.length > 0) {
                        result.messages.forEach(valMessage => {
                            dispatch(ValidationAlert(valMessage.message, "ReferenceType"));
                        });
                    }
                    else {
                        dispatch(ValidationAlert(localConstant.validationMessage.API_WENT_WRONG, "ReferenceType"));
                    }
                });
            }
        }
        else {
            if (response.messages.length > 0) {
                response.messages.forEach(result => {
                    if (result.message.length > 0) {
                        dispatch(ValidationAlert(result.message, "ReferenceType"));
                    }
                });
            }
            else {
                dispatch(ValidationAlert(localConstant.validationMessage.API_WENT_WRONG, "ReferenceType"));
            }
        }
    }
    else {
        dispatch(FailureAlert(response, "ReferenceType"));
    }
    // dispatch(HideLoader());
};
