import { techSpecActionTypes } from '../../constants/actionTypes';
import { mergeobjects, convertObjectToArray,isEmptyReturnDefault,getlocalizeData,isEmpty,isEmptyOrUndefine } from '../../../utils/commonUtils';
import { masterData,RequestPayload } from '../../../../src/apiConfig/apiConfig';
import { FetchData, PostData } from '../../../../src/services/api/baseApiService';
import IntertekToaster from '../../../../src/common/baseComponents/intertekToaster';
const localConstant = getlocalizeData();

const actions = {
    AddStampDetails: (payload) => (
        {
            type: techSpecActionTypes.resourceStatusActionTypes.ADD_STAMP_DETAILS,
            data: payload
        }
    ),
    DeleteStampDetails: (payload) => (
        {
            type: techSpecActionTypes.resourceStatusActionTypes.DELETE_STAMP_DETAILS,
            data: payload
        }
    ),
    UpdateStampDetails: (payload) => (
        {
            type: techSpecActionTypes.resourceStatusActionTypes.UPDATE_STAMP_DETAILS,
            data: payload
        }
    ),
    UpdateResourceStatus: (payload) => ({
        type: techSpecActionTypes.resourceStatusActionTypes.UPDATE_RESOURCE_STATUS,
        data: payload
    }),
    FeatchTechSpechStampCountryCode: (payload) => ({
        type: techSpecActionTypes.resourceStatusActionTypes.FEATCH_TECH_SPECH_STAMP_COUNTRY_CODE,
        data: payload
    }),
};
export const UpdateResourceStatus = (data) => (dispatch, getstate) => {
    const state = getstate();
    const modifiedData = mergeobjects(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo, data);
    dispatch(actions.UpdateResourceStatus(modifiedData));
};
export const AddStampDetails = (data) => (dispatch, getstate) => {
    dispatch(actions.AddStampDetails(data));
};
// Dispatch action to delete the stamp details from store
export const DeleteStampDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistStamp);
    data.map(row => {
        newState.map((iteratedValue, index) => {
            if (iteratedValue.id === row.id) {
                if (iteratedValue.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    newState.splice(index, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteStampDetails(newState));
};

// Dispatch an action to update the edited stamp details in store
export const UpdateStampDetails = (editedRowData, updatedData) => (dispatch, getstate) => {
    const state = getstate();
    const editedRow = mergeobjects(editedRowData, updatedData);
    const index = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistStamp).findIndex(iteratedValue => iteratedValue.id === editedRow.id);
    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistStamp);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateStampDetails(newState));
    }
};

export const FeatchTechSpechStampCountryCode =()=> async (dispatch, getstate)=>{ 
    if (!isEmptyOrUndefine(getstate().RootTechSpecReducer.TechSpecDetailReducer.masterTechSpecStampCountryCodes)) {
        return false;
    }
    const tsStampCountryCodeUrl = masterData.techSpechStampCountryCode;
    const isActive=true;
    const params = {
        isActive:isActive //Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(tsStampCountryCodeUrl,requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.TECH_SPEC_STAMP_COUNTRY_CODE_VALIDATION, 'wariningToast tsStampCountryCode');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response)) {
        if (response.code === "1") {
            dispatch(actions.FeatchTechSpechStampCountryCode(response.result));
        }
        else if (response.code && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast tsStampCountryCode');
        } 
    }
    else {
        IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast tsStampCountryCode');
    }
};