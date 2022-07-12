import { RequestPayload, adminAPIConfig } from '../../apiConfig/apiConfig';
import {  CreateData } from '../../services/api/baseApiService';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import {
    getlocalizeData, mergeobjects, parseValdiationMessage, isEmptyReturnDefault
    , convertObjectToArray
} from '../../utils/commonUtils';
import { FetchAssignmentStatus } from './assignmentStatusAction';
import { FetchAssignmentLifeCycle } from './assignmnetLifecycleAction';
import { adminActionTypes } from '../../constants/actionTypes';
const localConstant = getlocalizeData();
const actions = {
    SaveAssignmentLifecycle: (payload) => (
        {
            type: adminActionTypes.lifeCycle.SAVE_ASSIGNMENT_LIFECYCLE,
            data: payload
        }),
        CancelAssignmentLifecycle: (payload) => (
            {
                type: adminActionTypes.lifeCycle.CANCEL_LIFECYCLE,
                data: payload
            }),
    };
    export const SaveAssignmentLifecycle = (data) => async (dispatch, getState) => {
        const lifecycleDetails=getState().rootAdminReducer.assignmnetlifecycle;
        const assignmentStatus=getState().rootAdminReducer.assignmnetStatus;
        const completeData=[];
        lifecycleDetails && lifecycleDetails.forEach(x=>{
        if(x.recordStatus==='N'|| x.recordStatus==='M'|| x.recordStatus==='D'){
            completeData.push(x);
       }
    });
    assignmentStatus && assignmentStatus.forEach(x=>{
        if(x.recordStatus==='N'|| x.recordStatus==='M'|| x.recordStatus==='D'){
            completeData.push(x);
       }
    });
       
        const Url = 'http://localhost:5101/' + adminAPIConfig.assignmentLifecycle.replace('{masterdatatypeId}', 0);
    const requestPayload = new RequestPayload(completeData);
    const response = await CreateData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast viewRole');
        });
    if (response && response.code == "1") {
        // dispatch(FetchAssignmentStatus(response.result));
        // dispatch(FetchAssignmentLifeCycle(response.result));
        return response.result;
    }
    else {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast viewRoleSomthingWrong');
    }
    };
    export const CancelAssignmentLifecycle = (data) => async (dispatch, getState) => {
        dispatch(actions.CancelAssignmentLifecycle());
    };