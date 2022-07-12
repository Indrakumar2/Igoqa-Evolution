import { techSpecActionTypes } from '../../constants/actionTypes';
import { mergeobjects, convertObjectToArray,isEmptyReturnDefault } from '../../../utils/commonUtils';
const actions = {
    AddTaxonomyApprovalDetails: (payload) => (
        {
            type: techSpecActionTypes.taxonomyApprovalActionTypes.ADD_TAXONOMY_APPROVAL,
            data: payload
        }
    ),
    DeleteTaxonomyApprovalDetails: (payload) => (
        {
            type: techSpecActionTypes.taxonomyApprovalActionTypes.DELETE_TAXONOMY_APPROVAL,
            data: payload
        }),
    UpdateTaxonomyApprovaldetails: (payload) => (
        {
            type: techSpecActionTypes.taxonomyApprovalActionTypes.UPDATE_TAXONOMY_APPROVAL,
            data: payload
        }
    ),
    UpdateTQMComments:(payload)=>({
        type: techSpecActionTypes.taxonomyApprovalActionTypes.UPDATE_TAXONOMY_TQM_COMMENT,
        data: payload
    }),
};
export const UpdateTQMComments = (data) => (dispatch, getstate) => {
    dispatch(actions.UpdateTQMComments(data));
};
export const AddTaxonomyApprovalDetails = (data) => (dispatch) => {
    dispatch(actions.AddTaxonomyApprovalDetails(data));

};
export const DeleteTaxonomyApprovalDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistTaxonomy);
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
    dispatch(actions.DeleteTaxonomyApprovalDetails(newState));
};
export const UpdateTaxonomyApprovaldetails = (updatedData, editedRowData) => (dispatch, getstate) => {
    const state = getstate();
    const editedRow = mergeobjects(editedRowData, updatedData);
    const index = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistTaxonomy).findIndex(iteratedValue => iteratedValue.id === editedRow.id);
    const newState = convertObjectToArray(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistTaxonomy);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateTaxonomyApprovaldetails(newState));
    }
};