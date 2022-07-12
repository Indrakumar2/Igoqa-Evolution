import { RequestPayload, adminAPIConfig } from '../../apiConfig/apiConfig';
import { FetchData } from '../../services/api/baseApiService';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { parseValdiationMessage } from '../../utils/commonUtils';
import { getlocalizeData } from '../../utils/commonUtils';
import { adminActionTypes } from '../../constants/actionTypes';
const localConstant = getlocalizeData();

const actions = {
    AddLifeCycleDetails: (payload) => (
        {
            type: adminActionTypes.lifeCycle.ADD_LIFE_CYCLE,
            data: payload
        }),
    UpdateLifeCycle: (payload) => ({
        type: adminActionTypes.lifeCycle.UPDATE_LIFECYCLE,
        data: payload
    }),
    DeleteLifeCycle: (payload) => ({
        type: adminActionTypes.lifeCycle.DELETE_LIFECYCLE,
        data: payload
    }),
    FetchLifeCycle: (payload) => ({
        type: adminActionTypes.lifeCycle.FETCH_LIFECYCLE,
        data: payload
    }),
    
};
export const AddLifeCycleDetails = () => async (dispatch, getState) => {
    dispatch(actions.AddLifeCycleDetails());
};
export const UpdateLifeCycle = () => async (dispatch, getState) => {
    dispatch(actions.UpdateLifeCycle());
};
export const DeleteLifeCycle = () => async (dispatch, getState) => {
    dispatch(actions.DeleteLifeCycle());
};
export const FetchLifeCycle = () => async (dispatch, getState) => {
    const Url = adminAPIConfig.auditBaseUrl + adminAPIConfig.assignmentLifecycle;
    const params={
        masterdatatypeId:1
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {   
            // console.error(error); // To show the error details in console         
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast viewRole');
        }); 
    if (response && response.code == "1") { 
        dispatch(actions.FetchLifeCycle(response.result));  
        return response.result;
    }
    else {        
        IntertekToaster(parseValdiationMessage(response), 'dangerToast viewRoleSomthingWrong');
    }
};