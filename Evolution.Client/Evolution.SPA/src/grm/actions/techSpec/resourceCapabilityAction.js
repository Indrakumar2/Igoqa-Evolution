import { techSpecActionTypes } from '../../constants/actionTypes';
import {  mergeobjects, convertObjectToArray ,isEmptyReturnDefault,getlocalizeData, isUndefined } from '../../../utils/commonUtils';
import { GrmAPIConfig,RequestPayload } from '../../../apiConfig/apiConfig'; 
import IntertekToaster from '../../../common/baseComponents/intertekToaster';
import { FetchData } from '../../../services/api/baseApiService';
import { ShowLoader,HideLoader } from '../../../common/commonAction';
const localConstant = getlocalizeData();
const actions = {
    AddLanguageCapabilityDetails: (payload) => ({
        type: techSpecActionTypes.resourceCapabilityActionTypes.ADD_LANGUAGE_CAPABILITY,
        data: payload
    }),
    AddCertificateDetails: (payload) => ({
        type: techSpecActionTypes.resourceCapabilityActionTypes.ADD_CERTIFICATE_DETAILS,
        data: payload
    }),
    AddCommodityDetails: (payload) => ({
        type: techSpecActionTypes.resourceCapabilityActionTypes.ADD_COMMODITY_DETAILS,
        data: payload
    }),

    AddTrainingDetails: (payload) => ({
        type: techSpecActionTypes.resourceCapabilityActionTypes.ADD_TRAINING_DETAILS,
        data: payload
    }),
    UpdateResourceCapabilityCodeStandard: (payload) => ({
        type: techSpecActionTypes.resourceCapabilityActionTypes.UPDATE_RESOURCE_CAPABILITY_CODESATNDARD,
        data: payload
    }),
    UpdateResourceCapabilityComputerKnowledge: (payload) => ({
        type: techSpecActionTypes.resourceCapabilityActionTypes.UPDATE_RESOURCE_CAPABILITY_COMPUTER_KNOWLEDGE,
        data: payload
    }),
    UpdateLanguageDetails: (payload) => ({
        type: techSpecActionTypes.resourceCapabilityActionTypes.UPDATE_LANGUAGE_CAPABILITY,
        data: payload
    }),
    UpdateCertificateDetails: (payload) => ({
        type: techSpecActionTypes.resourceCapabilityActionTypes.UPDATE_CERTIFICATE_DETAILS,
        data: payload
    }),
    UpdateCommodityDetails: (payload) => ({
        type: techSpecActionTypes.resourceCapabilityActionTypes.UPDATE_COMMODITY_DETAILS,
        data: payload
    }),

    UpdateTrainingDetails: (payload) => ({
        type: techSpecActionTypes.resourceCapabilityActionTypes.UPDATE_TRAINING_DETAILS,
        data: payload
    }),
    DeleteLanguageDetails: (payload) => ({
        type: techSpecActionTypes.resourceCapabilityActionTypes.DELETE_LANGUAGE_CAPABILITY,
        data: payload
    }),
    DeleteCertificateDetails: (payload) => ({
        type: techSpecActionTypes.resourceCapabilityActionTypes.DELETE_CERTIFICATE_DETAILS,
        data: payload
    }),
    DeleteCommodityDetails: (payload) => ({
        type: techSpecActionTypes.resourceCapabilityActionTypes.DELETE_COMMODITY_DETAILS,
        data: payload
    }),
    DeleteTrainingDetails: (payload) => ({
        type: techSpecActionTypes.resourceCapabilityActionTypes.DELETE_TRAINING_DETAILS,
        data: payload
    }),
    FetchIntertekWorkHistoryReport:(payload)=>({
        type: techSpecActionTypes.resourceCapabilityActionTypes.FETCH_INTERTEK_WORK_HISTORY_REPORT_DETAILS,
        data: payload
    }),
    IsRCRMUpdatedResourceCapability:(payload)=>({
        type: techSpecActionTypes.resourceCapabilityActionTypes.IS_RCRM_UPDATED_RESOURCE_CAPABILITY,
        data: payload
    }),
    IsRCRMUpdatedResourceCapabilityCodeStandard:(payload)=>({
        type: techSpecActionTypes.resourceCapabilityActionTypes.IS_RCRM_UPDATED_RESOURCE_CAPABILITY_CODE_STD,
        data: payload
    }),
    IsRCRMUpdatedResourceCapabilityComKnowledge:(payload)=>({
        type: techSpecActionTypes.resourceCapabilityActionTypes.IS_RCRM_UPDATED_RESOURCE_CAPABILITY_COMP_KNOWLEDGE,
        data: payload
    })
};
export const AddLanguageCapabilityDetails = (data) => (dispatch) => {
    dispatch(actions.AddLanguageCapabilityDetails(data));
};

export const IsRCRMUpdatedResourceCapability = (data) => (dispatch) => {
    dispatch(actions.IsRCRMUpdatedResourceCapability(data));
};

export const IsRCRMUpdatedResourceCapabilityCodeStandard = (data) => (dispatch) => {
    dispatch(actions.IsRCRMUpdatedResourceCapabilityCodeStandard(data));
};

export const IsRCRMUpdatedResourceCapabilityComKnowledge = (data) => (dispatch) => {
    dispatch(actions.IsRCRMUpdatedResourceCapabilityComKnowledge(data));
};

export const AddCertificateDetails = (data) => (dispatch) => {
    dispatch(actions.AddCertificateDetails(data));
};

export const AddCommodityDetails = (data) => (dispatch) => {
       dispatch(actions.AddCommodityDetails(data));
};

export const AddTrainingDetails = (data) => (dispatch) => {
    dispatch(actions.AddTrainingDetails(data));
};

export const UpdateResourceCapabilityCodeStandard = (data) => (dispatch,getstate) => {
      dispatch(actions.UpdateResourceCapabilityCodeStandard(data));  
};

export const UpdateResourceCapabilityComputerKnowledge = (data) => (dispatch,getstate) => {
      dispatch(actions.UpdateResourceCapabilityComputerKnowledge(data));
};

// Dispatch an action to update the edited language details in store
export const UpdateLanguageDetails = (editedRowData, updatedData) => (dispatch, getstate) => {
      const state = getstate();
    const editedRow = mergeobjects(editedRowData, updatedData);
    const index = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistLanguageCapabilities).findIndex(iteratedValue => iteratedValue.id === editedRow.id);
    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistLanguageCapabilities);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateLanguageDetails(newState));
    }
};
// Dispatch an action to update the edited Certificate Details in store
export const UpdateCertificateDetails = ( updatedData,editedCertificateDetails) => (dispatch, getstate) => {  
    const state = getstate();
    const editedRow = mergeobjects(editedCertificateDetails, updatedData);
    const index = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCertification).findIndex(iteratedValue => iteratedValue.id === editedRow.id);
    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCertification);
    //editedRow.documents=editedRow.documents.filter(x =>  x.recordStatus !== 'D');
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateCertificateDetails(newState));
    }
};
// Dispatch an action to update the edited commodity details in store
export const UpdateCommodityDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
   // const index = state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCommodityAndEquipment.findIndex(iteratedValue => iteratedValue.id === data.id);
    let newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCommodityAndEquipment);
    const updateState = newState.filter(value => {
        return !data.some(item => item.id === value.id);
    });
    newState = data.concat(updateState);
    const removednewlydeletedrecord = newState.filter(val => !(val.recordStatus === "D" && isUndefined(val.updateCount)));
    dispatch(actions.UpdateCommodityDetails(removednewlydeletedrecord));
};
    
// Dispatch an action to update the edited training details in store
export const UpdateTrainingDetails = (editedTrainingDetails, updatedData) => (dispatch, getstate) => {
    const state = getstate();
    const editedRow = mergeobjects(editedTrainingDetails, updatedData);
    const index = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistTraining).findIndex(iteratedValue => iteratedValue.id === editedRow.id);
    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistTraining);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateTrainingDetails(newState));
    }
};
// Dispatch action to delete the language details from store
export const DeleteLanguageDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistLanguageCapabilities);
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
    dispatch(actions.DeleteLanguageDetails(newState));
};
// Dispatch action to delete the certificate details from store
export const DeleteCertificateDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCertification);
    data.map(row => {
        newState.map((iteratedValue, index) => {
            if (iteratedValue.id === row.id) {//D556
                if (iteratedValue.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    newState.splice(index, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteCertificateDetails(newState));
};
// Dispatch action to delete the certificate details from store
export const DeleteCommodityDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCommodityAndEquipment);

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

    dispatch(actions.DeleteCommodityDetails(newState));
};
// Dispatch action to delete the training details from store
export const DeleteTrainingDetails = (data) => (dispatch, getstate) => {   
    const state = getstate();
    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistTraining);
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
    dispatch(actions.DeleteTrainingDetails(newState));
};

/** fetch Intertek Work History Report */
export const FetchIntertekWorkHistoryReport = (data) => async (dispatch, getstate) => {
    if (data) {
        dispatch(ShowLoader()); 
        const reportUrl = GrmAPIConfig.intertekWorkHistoryReport;
        const params = {
            'epin': data
        };
        const requestPayload = new RequestPayload(params); 
        const response = await FetchData(reportUrl, requestPayload)
            .catch(error => {  
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.errorMessages.WORK_HISTORY_DATA_NOT_FETCHED, 'dangerToast IntertekWorkHistoryReportDataNotFound');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });

        if (response) {
            if (response.code == 1) {
                if (response.result && response.result.length > 0) {
                    response.result.forEach(x =>{
                        x.assignmentNumber = x.assignmentNumber.toString().padStart(5, '0');
                    }); //D1130 (Issue 6.2)
                    dispatch(actions.FetchIntertekWorkHistoryReport(response.result));
                }
                else {
                    dispatch(actions.FetchIntertekWorkHistoryReport([]));
                    IntertekToaster(localConstant.errorMessages.WORK_HISTORY_DATA_NOT_FOUND, 'warningToast IntertekWorkHistoryWentWrong');
                }
            } 
            else if (response.code && (response.code === "11" || response.code === "41" || response.code === "31")) {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast IntertekWorkHistoryWentWrong');
            }
            else {
                IntertekToaster(localConstant.errorMessages.WORK_HISTORY_DATA_NOT_FETCHED, 'dangerToast IntertekWorkHistoryError');
            } 
        }
       dispatch(HideLoader());
    }
};