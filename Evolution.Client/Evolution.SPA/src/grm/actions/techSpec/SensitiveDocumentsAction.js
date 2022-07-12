import { techSpecActionTypes } from '../../constants/actionTypes';
import { mergeobjects, convertObjectToArray ,isEmptyReturnDefault } from '../../../utils/commonUtils';

const actions = {
   
    AddSensitiveDetails: (payload) => ({
        type: techSpecActionTypes.sensitiveDocumentsActionTypes.ADD_SENSITIVE_DETAILS,
        data: payload
    }),

    DeleteSensitiveDetails: (payload) => ({
        type: techSpecActionTypes.sensitiveDocumentsActionTypes.DELETE_SENSITIVE_DETAILS,
        data: payload
    }),
    UpdateSensitiveDetails: (payload) => ({
        type: techSpecActionTypes.sensitiveDocumentsActionTypes.UPDATE_SENSITIVE_DOCUMENTS,
        data: payload
    }),
    StoreDocUniqueCode:(payload)=>({
        type:techSpecActionTypes.sensitiveDocumentsActionTypes.STORE_DOC_UNIQUE_CODE,
        data:payload
    }),

    IsRemoveDocument:(payload) =>({
               type: techSpecActionTypes.sensitiveDocumentsActionTypes.IS_REMOVE_DOCUMENT,
               data:payload
    }),
    //Added for D223 Popup cancel issue
    ClearDocumentUploadedSensitiveDocument: (payload) => ({
        type: techSpecActionTypes.sensitiveDocumentsActionTypes.CLEAR_SENSITIVE_DOCUMENTS,
        data: payload
    }),
    RevertDeletedSensitiveDocument: (payload) => ({
        type: techSpecActionTypes.sensitiveDocumentsActionTypes.REVERT__DELETE_SENSITIVE_DOCUMENTS,
        data: payload
    }),
};
export const AddSensitiveDetails = (data) => (dispatch) => {
   
    dispatch(actions.AddSensitiveDetails(data));
};

// Dispatch an action to delete the sensitive details from store
export const DeleteSensitiveDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = convertObjectToArray(isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistSensitiveDocuments));
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
    dispatch(actions.DeleteSensitiveDetails(newState));
};

//Dispatch an action to update the edited sensitive details in store
export const UpdateSensitiveDetails= ( updatedData,editedRowData) => async (dispatch, getstate) => {

    const state = getstate();
    const editedRow = mergeobjects(editedRowData, updatedData);
  
    const index = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistSensitiveDocuments).findIndex(iteratedValue => iteratedValue.id === editedRow.id);
    const newState = convertObjectToArray(isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistSensitiveDocuments));
    newState[index] = editedRow;
    if (index >= 0) {
        await dispatch(actions.UpdateSensitiveDetails(newState));
      
    }
    else {
        await dispatch(actions.StoreDocUniqueCode(editedRow));
    }
    return newState;
};
export const IsRemoveDocument=(data)=>(dispatch)=>
{
    dispatch(actions.IsRemoveDocument(data));
};

//Dispath an action to store document uniqe code
export const StoreDocUniqueCode=()=>(dispatch)=>{
    dispatch(actions.StoreDocUniqueCode([]));
};
//Added for D223 Popup cancel issue
export const RevertDeletedSensitiveDocument =(revetedData)=> (dispatch, getstate) => {
    dispatch(actions.RevertDeletedSensitiveDocument(revetedData));   
};

export const ClearDocumentUploadedSensitiveDocument =()=> (dispatch, getstate) => {    
    dispatch(actions.ClearDocumentUploadedSensitiveDocument());
};