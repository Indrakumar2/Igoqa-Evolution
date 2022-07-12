import { techSpecActionTypes } from '../../constants/actionTypes';
import { mergeobjects, convertObjectToArray ,isEmptyReturnDefault } from '../../../utils/commonUtils';

const actions = {
   
    AddTechSpecDocDetails: (payload) => ({
        type: techSpecActionTypes.techSpecDocumentsActionTypes.ADD_TECHSPEC_DOC_DETAILS,
        data: payload
    }),

    DeleteTechSpecDocDetails: (payload) => ({
        type: techSpecActionTypes.techSpecDocumentsActionTypes.DELETE_TECHSPEC_DOC_DETAILS,
        data: payload
    }),
    UpdateTechSpecDocDetails: (payload) => ({
        type: techSpecActionTypes.techSpecDocumentsActionTypes.UPDATE_TECHSPEC_DOC_DOCUMENTS,
        data: payload
    }),
    IsRCRMUpdatedDocumentInformation:(payload)=>({
        type: techSpecActionTypes.techSpecDocumentsActionTypes.IS_RCRM_UPDATED_DOCUMENT,
        data: payload
    }),
    AddFilesToBeUpload: (payload) => ({
        type: techSpecActionTypes.techSpecDocumentsActionTypes.ADD_FILES_TO_BE_UPLOADED,
        data: payload
    }),

    ClearFilesToBeUpload: (payload) => ({
        type: techSpecActionTypes.techSpecDocumentsActionTypes.CLEAR_FILES_TO_BE_UPLOADED,
        data: payload
    }),
};
export const AddTechSpecDocDetails = (data) => (dispatch) => { 
    dispatch(actions.AddTechSpecDocDetails(data));
};
export const IsRCRMUpdatedDocumentInformation = (data) => (dispatch) => { 
    dispatch(actions.IsRCRMUpdatedDocumentInformation(data));
};

// Dispatch an action to delete the sensitive details from store
export const DeleteTechSpecDocDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = convertObjectToArray(isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistDocuments));
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
    dispatch(actions.DeleteTechSpecDocDetails(newState));
};

//Dispatch an action to update the edited sensitive details in store
export const UpdateTechSpecDocDetails = ( updatedData,editedRowData) => (dispatch, getstate) => {
    const state = getstate();
    if(editedRowData){
    const editedRow = mergeobjects(editedRowData, updatedData);
  
    const index = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistDocuments).findIndex(iteratedValue => iteratedValue.id === editedRow.id);
    const newState = convertObjectToArray(isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistDocuments));
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateTechSpecDocDetails(newState));
    }
}
else{
    dispatch(actions.UpdateTechSpecDocDetails(updatedData));
}
};

export const AddFilesToBeUpload = (data) => (dispatch) => {
    dispatch(actions.AddFilesToBeUpload(data));
};

export const ClearFilesToBeUpload = (data) => (dispatch) => {
    dispatch(actions.ClearFilesToBeUpload([]));
};