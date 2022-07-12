import { assignmentsActionTypes } from '../../constants/actionTypes';
import { FetchData } from '../../services/api/baseApiService';
import { assignmentAPIConfig, RequestPayload, customerAPIConfig } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { getlocalizeData } from '../../utils/commonUtils';
const localConstant = getlocalizeData();
const actions = {
    AddNewAssignmentReference: (payload) => ({
        type: assignmentsActionTypes.ADD_NEW_ASSIGNMENT_REFERENCE,
        data: payload
    }),
    DeleteAssignmentReference: (payload) => ({
        type: assignmentsActionTypes.DELETE_ASSIGNMENT_REFERENCE,
        data: payload
    }),
    UpdatetAssignmentReference: (payload) => ({
        type: assignmentsActionTypes.UPDATE_ASSIGNMENT_TAB_REFERENCE,
        data: payload
    }),
    FetchReferencetypes: (payload) => ({
        type: assignmentsActionTypes.FETCH_REFERENCE_TYPES,
        data: payload
    }),
    AddProjectAssignmentReference: (payload) => ({
        type: assignmentsActionTypes.ADD_PROJECT_ASSIGNMENT_REFERENCE,
        data: payload
    }),
};

export const AddNewAssignmentReference = (data) => (dispatch, getstate) => {
    dispatch(actions.AddNewAssignmentReference(data));
};
export const AddProjectAssignmentReference = (data) => (dispatch, getstate) => {
    dispatch(actions.AddProjectAssignmentReference(data));
};
export const DeleteAssignmentReference = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootAssignmentReducer.assignmentDetail.AssignmentReferences);
    data.forEach(row => {
        newState.forEach((iteratedValue, index) => {
            if (iteratedValue.assignmentReferenceTypeId === row.assignmentReferenceTypeId) {
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    const delIndex = newState.findIndex(value => (value.assignmentReferenceTypeAddUniqueId === row.assignmentReferenceTypeAddUniqueId));
                    if (delIndex >= 0)
                        newState.splice(delIndex, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteAssignmentReference(newState));
};
export const UpdatetAssignmentReference = (editedRowData, updatedData) => (dispatch, getstate) => {
    const state = getstate();
    const editedRow = Object.assign({},updatedData, editedRowData );
    const references = state.rootAssignmentReducer.assignmentDetail.AssignmentReferences;
    let checkProperty = "assignmentReferenceTypeId";
    if (editedRow.recordStatus === 'N') {
        checkProperty = "assignmentReferenceTypeAddUniqueId";
    }
    const index = references.findIndex(iteratedValue => iteratedValue[checkProperty] === editedRow[checkProperty]);
    
    const newState = Object.assign([], references);
    if (index >= 0) {
        newState[index] = editedRow;
        dispatch(actions.UpdatetAssignmentReference(newState));
    }
};

export const FetchReferencetypes = (data) => async (dispatch, getstate) => {
    const state = getstate();
    const projectNumber = state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentProjectNumber;
    const params = {};

    const requestPayload = new RequestPayload(params);
    const url = customerAPIConfig.projects + projectNumber + assignmentAPIConfig.assignmnetReferenceTypes;
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.assignments.ERROR_FETCHING_REFERENCE_TYPES, 'dangerToast fetchAssignmentWrong');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchReferencetypes(response.result));
    }
};