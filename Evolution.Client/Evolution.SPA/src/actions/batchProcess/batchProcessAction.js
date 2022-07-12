import { getlocalizeData,parseValdiationMessage } from '../../utils/commonUtils';
import { batchProcessActionTypes } from '../../constants/actionTypes';
import { BatchAPIConfig, RequestPayload } from '../../apiConfig/apiConfig';
import { FetchData } from '../../services/api/baseApiService';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { checkIfBatchProcessCompleted } from '../contracts/rateScheduleAction';

const localConstant = getlocalizeData();

const actions = {
    fetchBatchProcessData: (payload) => ({
        type: batchProcessActionTypes.FETCH_BATCH_PROCESS,
        data: payload
    }),
};

export const FetchBatchProcessData = (data) => async (dispatch) => {
    if(data){
        const contractBatchId = localConstant.batchConstant.contractBatchId;
        const params = { "aintBatchID" : contractBatchId ,"aintParamID" : data };
        const requestPayload = new RequestPayload(params);
        const url = BatchAPIConfig.getBatch;
        const response = await FetchData(url, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.validationMessage.RELATED_FRAMEWORK_BATCH_FAILED, 'dangerToast relatedFrameworkBatchFailed');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response && response.code === "1") {
            const batchData = response.result;
            if(batchData != null){
                if(batchData.processStatus != null  && batchData.processStatus == 0 || batchData.processStatus == 1){
                    const batchId = sessionStorage.getItem('BatchId');
                    if (batchId && batchId == batchData.id) {
                        dispatch(checkIfBatchProcessCompleted(batchId));
                    }    
                }   
            }
            dispatch(actions.fetchBatchProcessData(response.result));
        } else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast relatedFrameworkBatchFailed');
        }
        else{
            IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast relatedFrameworkBatchFailed');
        }
    } else {
        dispatch(actions.fetchBatchProcessData({}));
    }
};