import { techSpecActionTypes } from '../../constants/actionTypes';
import { dashBoardActionTypes } from '../../../constants/actionTypes';
import { GrmAPIConfig, RequestPayload } from '../../../apiConfig/apiConfig';
import { isEmpty, getlocalizeData, parseValdiationMessage, isEmptyOrUndefine,isUndefined } from '../../../utils/commonUtils';
import { FetchData, CreateData } from '../../../services/api/baseApiService';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';
import { ShowLoader, HideLoader } from '../../../common/commonAction';
import { homeAPIConfig } from '../../../apiConfig/apiConfig';
import { applicationConstants } from '../../../constants/appConstants';
import arrayUtil from '../../../utils/arrayUtil';

const localConstant = getlocalizeData();

const actions = {
    FetchTechSpecMytaskData: (payload) => (
        {
            type: techSpecActionTypes.techSpecSearch.TECH_SPEC_MYTASK_DATA,
            data:payload
        }
    ),
    GetSelectedMytask: (payload) => (
        {
            type: techSpecActionTypes.techSpecSearch.GET_TECH_SPEC_SELECTED_MYTASK,
            data:payload
        }
    ),
    ClearMyTasksData: (payload) => ({
        type: techSpecActionTypes.CLEAR_MY_TASKS_DATA,
        data: payload
    }),
    FetchMyTaskAssignUsers: (payload) => ({
        type: techSpecActionTypes.techSpecSearch.TECH_SPEC_MYTASK_ASSIGN_USERS,
        data: payload
    }),
    FetchDashboardCountForMyTask:(payload)=>({
        type:dashBoardActionTypes.FETCH_DASHBOARD_COUNT,
        data:payload
    }),
    ClearMyTaskAssignUser:(payload)=>({
        type: techSpecActionTypes.techSpecSearch.CLEAR_TECH_SPEC_MYTASK_ASSIGN_USERS,
        data: payload
    }),
   };

export const GetSelectedMytask = (payload) => (dispatch) => {
    dispatch(actions.GetSelectedMytask(payload));
};

export const FetchTechSpecMytaskData = (data) => async (dispatch,getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    if(isUndefined(data) && state.RootTechSpecReducer.TechSpecDetailReducer.isMyTasksLoaded){
        dispatch(HideLoader()); 
        return false;
    }
    let myTaskStatus = false;
    if(isUndefined(data)) {
        myTaskStatus = (!isUndefined(state.dashboardReducer.myTaskStatus) ? state.dashboardReducer.myTaskStatus : false);      
    } else {
        myTaskStatus = data;
    }    
    const companyCode = (!isEmptyOrUndefine(sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE)) 
                        ? sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE) 
                        : localStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE));
    const userName = (myTaskStatus === false ? localStorage.getItem(applicationConstants.Authentication.USER_NAME) : "");    
    const url = GrmAPIConfig.grmBaseUrl+homeAPIConfig.myTasks;    
    const params = { companyCode, userName };    
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.RESOURCE_MYTASK_DATA_NOT_FETCHED, 'dangerToast technicalSpecilistData');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader()); 
        });   
        if (!isEmpty(response) && !isEmpty(response.code)) {   
        if (response && response.code === "1") {
            //D363 CR change
            let filtrResult= response.result && response.result.filter(data => data.taskType !== "ProfileChangeHistory"); //D661 issue1 myTask CR
            if(filtrResult.length > 0){
                filtrResult.forEach(itratedValue =>{
                    const overrideTaskStatus = localConstant.resourceSearch.overrideTaskStatus;
                    const ploTaskStatus = localConstant.resourceSearch.ploTaskStatus;
                    const loggedInUserId = localStorage.getItem(applicationConstants.Authentication.USER_NAME);
                    if(itratedValue.companyCode === companyCode){
                        itratedValue.isDisableMyTaskLink = false;
                        itratedValue.isDisableMyTaskReassign = false;
                        if(itratedValue.moduletype === "RSEARCH"){
                            if(itratedValue.taskType === ploTaskStatus[0])
                                itratedValue.isDisableMyTaskReassign = true;
                            if((overrideTaskStatus.includes(itratedValue.taskType) || 
                                ploTaskStatus.includes(itratedValue.taskType)) && itratedValue.assignedTo === loggedInUserId){
                                    itratedValue.isDisableMyTaskLink = false;
                            } else{
                                itratedValue.isDisableMyTaskLink = true;
                            }
                        }
                    } 
                    else {
                        itratedValue.isDisableMyTaskLink = true;
                        itratedValue.isDisableMyTaskReassign = true;
                    }
                });
                filtrResult=arrayUtil.sort(filtrResult,'companyName','asc');
                filtrResult=arrayUtil.boolsort(filtrResult,'isDisableMyTaskLink','asc');
            }

            //D363 CR change
            dispatch(actions.FetchTechSpecMytaskData(filtrResult));  
            const count={ 'MyTaskCount':filtrResult.length }; //changes happened for home dashboard my task and my search count not refreshing
            const taskcount=Object.assign({},state.dashboardReducer.count,count);
            dispatch(actions.FetchDashboardCountForMyTask(taskcount)); 
        }
        else if(response && response.code && (response.code === "11" || response.code === "41")){
            IntertekToaster(parseValdiationMessage(response), 'warningToast fetchtechspecmytaskdata');
        } 
        dispatch(HideLoader());  
}
    else {
        IntertekToaster(localConstant.errorMessages.RESOURCE_MYTASK_DATA_NOT_FETCHED, 'dangerToast mytasksearcherror');
        dispatch(HideLoader());
    }
};

export const ClearMyTasksData = (data) => (dispatch,getstate) => {
    dispatch(actions.ClearMyTasksData());
};

export const FetchMyTaskAssignUsers = () => async (dispatch,getstate) => {
    dispatch(ShowLoader());
    if(!isEmptyOrUndefine(getstate().RootTechSpecReducer.TechSpecDetailReducer.techSpecMytaskAssignUsers)){
        dispatch(HideLoader()); 
        return false;
    }
    const companyCode = (!isEmptyOrUndefine(sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE)) 
                        ? sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE) 
                        : localStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE));    
    const fetchUrl = GrmAPIConfig.grmBaseUrl + homeAPIConfig.myTasksAssignUsers;
    const params = { companyCode };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(fetchUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast FetchMyTaskAssignUsers');
            dispatch(HideLoader());
        });

    if (!isEmptyOrUndefine(response) && !isEmptyOrUndefine(response.code) && response.code === "1") {
        const result = arrayUtil.sort(response.result, 'userName', 'asc');
        dispatch(actions.FetchMyTaskAssignUsers(result));        
        dispatch(HideLoader());
    }
    else {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast FetchMyTaskAssignUsers');
        dispatch(HideLoader());
    }
};

export const UpdateMyTaskReassign = (payload) => async (dispatch, getstate) => {
    dispatch(ShowLoader());    
    const state = getstate();
    let url = GrmAPIConfig.grmBaseUrl+homeAPIConfig.mytasksReassign;    
    if(payload && payload.length > 0 && payload[0].moduletype === "RSEARCH"){
        url = GrmAPIConfig.grmBaseUrl+homeAPIConfig.mytasksReassignARS;
    }
    const requestPayload = new RequestPayload(payload);    
    const response = await CreateData(url, requestPayload)
        .catch(error => {      
            // console.error(error); // To show the error details in console                  
            dispatch(HideLoader()); 
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast UpdateMyTaskReassign');
            return false;
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {   
        if (response && response.code === "1") {
            dispatch(HideLoader());  
            const myTaskStatus = (!isUndefined(state.dashboardReducer.myTaskStatus) ? state.dashboardReducer.myTaskStatus : false);
            dispatch(FetchTechSpecMytaskData(myTaskStatus));
            return true;
        }
        else if(response && response.code && (response.code === "11" || response.code === "41")){
            IntertekToaster(parseValdiationMessage(response), 'warningToast fetchtechspecmytaskdata');
            dispatch(HideLoader());  
            return false;
        }         
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_TO_REASSIGN_THE_TASK, 'dangerToast mytasksearcherror');
        dispatch(HideLoader());
        return false;
    }        
};

export const ClearMyTaskAssignUser = () => (dispatch,getstate) =>{
    dispatch(actions.ClearMyTaskAssignUser());
};