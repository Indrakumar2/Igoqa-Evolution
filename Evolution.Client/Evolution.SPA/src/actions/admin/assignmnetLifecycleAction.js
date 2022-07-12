import { RequestPayload, adminAPIConfig } from '../../apiConfig/apiConfig';
import { FetchData, PostData } from '../../services/api/baseApiService';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import {
    getlocalizeData, mergeobjects, parseValdiationMessage, isEmptyReturnDefault
    , convertObjectToArray
} from '../../utils/commonUtils';
import { adminActionTypes } from '../../constants/actionTypes';
const localConstant = getlocalizeData();

const actions = {
    AddAssignmentLifeCycle: (payload) => (
        {
            type: adminActionTypes.lifeCycle.ADD_LIFE_CYCLE,
            data: payload
        }),
    UpdateAssignmenttLifeCycle: (payload) => ({
        type: adminActionTypes.lifeCycle.UPDATE_LIFECYCLE,
        data: payload
    }),
    DeleteAssignmentLifeCycle: (payload) => ({
        type: adminActionTypes.lifeCycle.DELETE_LIFECYCLE,
        data: payload
    }),
    FetchAssignmentLifeCycle: (payload) => ({
        type: adminActionTypes.lifeCycle.FETCH_LIFECYCLE,
        data: payload
    }),
    // ClearAssignmentLifeCycle: (payload) => ({
    //     type: adminActionTypes.lifeCycle.CLEAR_LIFECYCLE,
    //     data: payload
    // }),

};
export const AddAssignmentLifeCycle = (data) => async (dispatch, getState) => {
    dispatch(actions.AddAssignmentLifeCycle(data));
};
export const UpdateAssignmenttLifeCycle = (updatedData, editedRowData) => (dispatch, getstate) => {
    const state = getstate();
    const editedRow = mergeobjects(editedRowData, updatedData);
    const index = isEmptyReturnDefault(state.rootAdminReducer.assignmnetlifecycle)
        .findIndex(iteratedValue => iteratedValue.id === editedRow.id);
    const newState = convertObjectToArray(state.rootAdminReducer.assignmnetlifecycle);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateAssignmenttLifeCycle(newState));
    }

};
export const DeleteAssignmentLifeCycle = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = convertObjectToArray(state.rootAdminReducer.assignmnetlifecycle);
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
    dispatch(actions.DeleteAssignmentLifeCycle(newState));
};
// export const ClearAssignmentLifeCycle = () => async (dispatch, getState) => {
//     dispatch(actions.ClearAssignmentLifeCycle());
// };
export const FetchAssignmentLifeCycle = () => async (dispatch, getState) => {
    const Url = 'http://localhost:5101/' + adminAPIConfig.assignmentLifecycle.replace('{masterdatatypeId}', 1);
    const requestPayload = new RequestPayload();
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast viewRole');
        });
    if (response && response.code == "1") {
        dispatch(actions.FetchAssignmentLifeCycle(response.result));
        return response.result;
    }
    else {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast viewRoleSomthingWrong');
    }
};