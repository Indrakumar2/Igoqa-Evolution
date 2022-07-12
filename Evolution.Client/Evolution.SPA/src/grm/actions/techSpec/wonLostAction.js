import { techSpecActionTypes } from '../../constants/actionTypes';
import { GrmAPIConfig, RequestPayload, projectAPIConfig } from '../../../apiConfig/apiConfig';
import { isEmpty, getlocalizeData, parseValdiationMessage } from '../../../utils/commonUtils';
import { ShowLoader, HideLoader } from '../../../common/commonAction';

import { FetchData } from '../../../services/api/baseApiService';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';

const localConstant = getlocalizeData();

const actions = {

    FetchCordinators: (payload) => ({
        type: techSpecActionTypes.FETCH_PROJECT_COORDINATOR,
        data: payload
    }),

    FetchWonLostDetails: (payload)  =>({
        type: techSpecActionTypes.FETCH_WONLOST_DATAS,
        data: payload
    }),

   };
export const FetchCordinators = (data) => async(dispatch,getstate) => {    
    const projectCoordinatorUrl = projectAPIConfig.user;
    const param = {
        companyCode: data,
        isActive:true
    };
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(projectCoordinatorUrl,requestPayload)
        .catch(error=>{
            // IntertekToaster(localConstant.validationMessage.PROJECT_COORDINATOR_API_VAL,'wariningToast projectCoordinatorVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
        if(!isEmpty(response)&&!isEmpty(response.code)){
            if(response.code == 1){
                dispatch(actions.FetchCordinators(response.result));
            }
            else if (response.code === "11" || response.code === "41" || response.code === "31") {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast ProjectCoWentWrong');
            }
            else{
                IntertekToaster(localConstant.validationMessage.PROJECT_COORDINATOR_API_VAL, 'dangerToast projectCoordinatorSWWVal');
            }
        }
        else{
            IntertekToaster(localConstant.validationMessage.PROJECT_COORDINATOR_API_VAL, 'dangerToast projectCoordinatorSWWVal');
        }
};

export const FetchWonLostDetails = (data) => async(dispatch,getstate) => { 
    dispatch(ShowLoader());   
    const wonLostURL = GrmAPIConfig.wonLostReports;
    const requestPayload = new RequestPayload(data);
    const response =  await FetchData(wonLostURL,requestPayload)
        .catch(error=>{
            // IntertekToaster(localConstant.validationMessage.WON_LOST_FETCH_FAILURE,'wariningToast projectCoordinatorVal3');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
        if(!isEmpty(response)&&!isEmpty(response.code)){
            if(response.code == 1){
                
                if(response.result && response.result.length > 0){
                    response.result.forEach(iteratedValue => {
                        iteratedValue.searchParameter = JSON.parse(iteratedValue.searchParameter);
                    });
                }     
               dispatch(actions.FetchWonLostDetails(response.result));
            }
            else if (response.code === "11" || response.code === "41" || response.code === "31") {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast ProjectCoWentWrong');
            }
            else{
                IntertekToaster(localConstant.validationMessage.WON_LOST_FETCH_FAILURE, 'dangerToast projectCoordinatorSWWVal3');
            }
            dispatch(HideLoader());
        }
        else{
            IntertekToaster(localConstant.validationMessage.WON_LOST_FETCH_FAILURE, 'dangerToast projectCoordinatorSWWVal4');
            dispatch(HideLoader());
        }
};