import { RequestPayload, adminAPIConfig } from '../../apiConfig/apiConfig';
import { FetchData, PostData } from '../../services/api/baseApiService';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
// import { isEmpty, parseValdiationMessage } from '../../utils/commonUtils';
import {
    getlocalizeData, mergeobjects, parseValdiationMessage, isEmptyReturnDefault
    , convertObjectToArray
} from '../../utils/commonUtils';
import { adminActionTypes } from '../../constants/actionTypes';
const localConstant = getlocalizeData();

const actions = {
    AddAssignmentStatus: (payload) => (
        {
            type:  adminActionTypes.status.ADD_STATUS,
            data: payload
        }),
    UpdateAssignmentStatus: (payload) => ({
        type:  adminActionTypes.status.UPDATE_STATUS,
        data: payload
    }),
    DeleteAssignmentStatus: (payload) => ({
        type: adminActionTypes.status.DELETE_STATUS,
        data: payload
    }),
    FetchAssignmentStatus: (payload) => ({
        type: adminActionTypes.status.FETCH_STATUS,
        data: payload
    }),
   
};
export const AddAssignmentStatus = (data) => async (dispatch, getState) => {
    dispatch(actions.AddAssignmentStatus(data));
};
export const UpdateAssignmentStatus = (updatedData, editedRowData) => (dispatch, getstate) => {
    const state = getstate();
    const editedRow = mergeobjects(editedRowData, updatedData);
    const index = isEmptyReturnDefault(state.rootAdminReducer.assignmnetStatus)
        .findIndex(iteratedValue => iteratedValue.id === editedRow.id);
    const newState = convertObjectToArray(state.rootAdminReducer.assignmnetStatus);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateAssignmentStatus(newState));
    }

};
export const DeleteAssignmentStatus = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = convertObjectToArray(state.rootAdminReducer.assignmnetStatus);
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
    dispatch(actions.DeleteAssignmentStatus(newState));
};

export const FetchAssignmentStatus = () => async (dispatch, getState) => {
    const Url = 'http://localhost:5101/' + adminAPIConfig.assignmentLifecycle.replace('{masterdatatypeId}', 2);
    const requestPayload = new RequestPayload();
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast viewRole');
        });
       
    if (response && response.code == "1") {
        dispatch(actions.FetchAssignmentStatus(response.result));
        return response.result;
    }
    else {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast viewRoleSomthingWrong');
    }
};