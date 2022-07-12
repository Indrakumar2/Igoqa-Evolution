import { visitActionTypes } from '../../constants/actionTypes';
import { visitAPIConfig, RequestPayload, customerAPIConfig } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData } from '../../services/api/baseApiService';
import { getlocalizeData, isEmpty, isEmptyReturnDefault } from '../../utils/commonUtils';
import { ShowLoader, HideLoader } from '../../common/commonAction';
import { StringFormat } from '../../utils/stringUtil';

const localConstant = getlocalizeData();

const actions = {
    FetchVisitReference: (payload) => ({
        type: visitActionTypes.FETCH_VISIT_REFERENCE,
        data: payload
    }),
    // FetchReferencetypes: (payload) => ({
    //     type: visitActionTypes.FETCH_REFERENCE_TYPES,
    //     data: payload
    // }),
    AddVisitReference: (payload) => ({
        type: visitActionTypes.ADD_VISIT_REFERENCE,
        data: payload
    }),
    UpdateVisitReference: (payload) => ({
        type: visitActionTypes.UPDATE_VISIT_REFERENCE,
        data: payload
    }),
    DeleteVisitReference: (payload) => ({
        type: visitActionTypes.DELETE_VISIT_REFERENCE,
        data: payload
    }),
};

export const FetchVisitReference = () => async (dispatch, getstate) => {
    const state = getstate();
    if (!isEmpty(state.rootVisitReducer.visitDetails.VisitReferences)) {
        return;
    }
    dispatch(ShowLoader());

    const visitID = state.rootVisitReducer.selectedVisitData.visitId;
    const url = visitAPIConfig.visitBaseUrl + visitAPIConfig.visits + StringFormat(visitAPIConfig.VisitReference, visitID);
    const param = {
        VisitId: visitID
    };
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.FETCH_VISIT_REFERENCE, 'wariningToast');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });

    if (!isEmpty(response)) {
        if (response.code === "1") {
            dispatch(actions.FetchVisitReference(response.result));
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

// export const FetchReferencetypes = (data) => async (dispatch, getstate) => {
//     const state = getstate();
//     const projectNumber = state.rootVisitReducer.selectedVisitData.visitProjectNumber;
//     //const projectNumber = state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentProjectNumber;
//     const params = {};

//     const requestPayload = new RequestPayload(params);
//     const url = customerAPIConfig.projects + projectNumber + visitAPIConfig.visitReferenceTypes;
//     alert(url);
//     const response = await FetchData(url, requestPayload)
//         .catch(error => {
//             IntertekToaster(localConstant.assignments.ERROR_FETCHING_REFERENCE_TYPES, 'dangerToast fetchAssignmentWrong');
//         });
//     if (response && response.code === "1") {
//         dispatch(actions.FetchReferencetypes(response.result));
//     }
// };

export const AddVisitReference = (data) => (dispatch, getstate) => {
    dispatch(actions.AddVisitReference(data));
};

export const UpdateVisitReference = (editedRowData, updatedData) => (dispatch, getstate) => {
    const state = getstate();
    const editedRow = Object.assign({}, updatedData, editedRowData);
    const references = state.rootVisitReducer.visitDetails.VisitReferences;
    let checkProperty = "visitReferenceId";
    if (editedRow.recordStatus === 'N') {
        checkProperty = "visitReferenceTypeAddUniqueId";
    }
    const index = references.findIndex(iteratedValue => iteratedValue[checkProperty] === editedRow[checkProperty]);
    const newState = Object.assign([], references);
    if (index >= 0) {
        newState[index] = editedRow;
        dispatch(actions.UpdateVisitReference(newState));
    }
};

export const DeleteVisitReference = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootVisitReducer.visitDetails.VisitReferences);
    data.forEach(row => {
        newState.forEach((iteratedValue, index) => {
            if (iteratedValue.visitReferenceId === row.visitReferenceId) {
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    const delIndex = newState.findIndex(value => (value.visitReferenceTypeAddUniqueId === row.visitReferenceTypeAddUniqueId));
                    if (delIndex >= 0)
                        newState.splice(delIndex, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteVisitReference(newState));
};