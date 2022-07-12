import { techSpecActionTypes } from '../../constants/actionTypes';
import { convertObjectToArray, mergeobjects, isEmptyReturnDefault } from '../../../utils/commonUtils';
const actions = {

    AddWorkHistoryDetails: (payload) => (
        {
            type: techSpecActionTypes.professionalDetailsActionTypes.ADD_WORK_HISTORY_DETAILS,
            data: payload
        }
    ),

    UpdateWorkHistoryDetails: (payload) => (
        {
            type: techSpecActionTypes.professionalDetailsActionTypes.UPDATE_WORK_HISTORY_DETAILS,
            data: payload
        }
    ),
    DeleteWorkHistoryDetails: (payload) => (
        {
            type: techSpecActionTypes.professionalDetailsActionTypes.DELETE_WORK_HISTORY_DETAILS,
            data: payload
        }
    ),

    AddEducationalDetails: (payload) => (
        {
            type: techSpecActionTypes.professionalDetailsActionTypes.ADD_EDUCATIONAL_DETAILS,
            data: payload
        }
    ),

    UpdateEducationalDetails: (payload) => (
        {
            type: techSpecActionTypes.professionalDetailsActionTypes.UPDATE_EDUCATIONAL_DETAILS,
            data: payload
        }
    ),
    DeleteEducationalDetails: (payload) => (
        {
            type: techSpecActionTypes.professionalDetailsActionTypes.DELETE_EDUCATIONAL_DETAILS,
            data: payload
        }
    ),

    UpdateProfessionalSummary: (payload) => ({
        type: techSpecActionTypes.professionalDetailsActionTypes.UPDATE_PROFESSIONAL_SUMMARY,
        data: payload
    }),

    AddProfessionalEducationDocuments: (payload) => ({
        type: techSpecActionTypes.professionalDetailsActionTypes.ADD_PROFESSIONAL_EDUCATION_DOCUMENTS,
        data: payload
    }),
    RemoveProfessionalEducationDocument: (payload) => ({
        type: techSpecActionTypes.professionalDetailsActionTypes.REMOVE_PROFESSIONAL_EDUCATION_DOCUMENT,
        data: payload
    }),
    IsRCRMUpdatedProfessionalEducationInformation:(payload)=>({
        type: techSpecActionTypes.professionalDetailsActionTypes.IS_RCRM_UPDATED_PROFESSIONAL_EDU_DETAILS,
        data: payload
    })
};

export const IsRCRMUpdatedProfessionalEducationInformation = (data) => (dispatch,) => { 
    dispatch(actions.IsRCRMUpdatedProfessionalEducationInformation(data));
};

export const UpdateProfessionalSummary = (data) => (dispatch, getstate) => {
    const state = getstate();
    const modifiedData = mergeobjects(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo, data);
    dispatch(actions.UpdateProfessionalSummary(modifiedData));
};

export const AddWorkHistoryDetails = (data) => (dispatch, getstate) => {
    dispatch(actions.AddWorkHistoryDetails(data));
};

export const UpdateWorkHistoryDetails = (updatedData, editedRowData) => (dispatch, getstate) => {    
    const state = getstate();
    const editedRow = mergeobjects(editedRowData, updatedData);
    const index = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistWorkHistory)
        .findIndex(iteratedValue => iteratedValue.id === editedRow.id);
    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistWorkHistory);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateWorkHistoryDetails(newState));
    }
};
export const DeleteWorkHistoryDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistWorkHistory);
    data.map(row => {
        newState.map(iteratedValue => {
            if (iteratedValue.id === row.id) {
                const index = newState.findIndex(value => (value.id === row.id));

                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    newState.splice(index, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteWorkHistoryDetails(newState));
};

export const AddEducationalDetails = (data) => (dispatch) => {
    dispatch(actions.AddEducationalDetails(data));

};

export const UpdateEducationalDetails = (updatedData, editedRowData) => (dispatch, getstate) => {
    const state = getstate();
    const editedRow = mergeobjects(editedRowData, updatedData);
    const index = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistEducation).findIndex(iteratedValue => iteratedValue.id === editedRow.id);
    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistEducation);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateEducationalDetails(newState));
    }
};

export const DeleteEducationalDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistEducation);
    data.map(row => {
        newState.map(iteratedValue => {
            if (iteratedValue.id === row.id) {
                const index = newState.findIndex(value => (value.id === row.id));
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    newState.splice(index, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteEducationalDetails(newState));
};

export const AddProfessionalEducationDocuments = (data) => (dispatch) => {
    dispatch(actions.AddProfessionalEducationDocuments(data));
};

export const RemoveProfessionalEducationDocument = (data) => (dispatch, getstate) => {
    const eductionalDocuments = getstate().RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo.professionalAfiliationDocuments;
    eductionalDocuments.forEach(doc => {
        if (doc.id === data) {
            doc.recordStatus = "D";
        }
    });
    dispatch(actions.RemoveProfessionalEducationDocument(eductionalDocuments));
};
