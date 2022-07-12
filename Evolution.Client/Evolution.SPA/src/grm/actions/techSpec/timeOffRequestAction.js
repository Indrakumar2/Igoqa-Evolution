import { techSpecActionTypes } from '../../constants/actionTypes';
import { GrmAPIConfig,masterData,assignmentAPIConfig, RequestPayload } from '../../../apiConfig/apiConfig';
import { isEmpty,getlocalizeData,isEmptyOrUndefine } from '../../../utils/commonUtils';
import { FetchData,PostData } from '../../../services/api/baseApiService';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';
import { applicationConstants } from '../../../constants/appConstants';
import { FetchEmploymentStatus } from '../../../common/masterData/masterDataActions';
import { ShowLoader, HideLoader  } from '../../../common/commonAction';
import { TechSpechUnSavedData } from '../../actions/techSpec/preAssignmentAction';

const localConstant = getlocalizeData();

const actions = {
    FetchTimeOffRequestData: (payload) => (
        {
            type: techSpecActionTypes.timeOffRequestActionType.FETCH_TIME_OFF_REQUEST,
            data:payload
        }
    ),
    FetchCategory:(payload) =>(
        {
        type: techSpecActionTypes.timeOffRequestActionType.FETCH_CATEGORY,
        data:payload
        }
    ),
    SaveResorceNameForManagers:(payload) =>(
        {
            type: techSpecActionTypes.timeOffRequestActionType.SAVE_RESOURCE_NAME,
            data:payload
        }
    ),
};

export const FetchTimeOffRequestData = (inputdata) => async (dispatch,getstate) => {
    dispatch(ShowLoader());  
   // dispatch(FetchEmploymentStatus());
    const userType    = localStorage.getItem(applicationConstants.Authentication.USER_TYPE);
    const unique_name = localStorage.getItem(applicationConstants.Authentication.USER_NAME);
    const ccode       = (!isEmptyOrUndefine(sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE)) 
                            ? sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE) 
                            : localStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE));
    const url = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalSpecialists + GrmAPIConfig.basicInfo; // For PT Issue Resolve Add the New Get API
    const params = {};
    if (!isEmpty(ccode)) {
        params.companyCode=ccode;
      //  params.profileStatus='Active';//def 872 Fix
    }
    if (!isEmpty(inputdata)) {
        params.epin = inputdata.epin;
    }
    if(userType === "TechnicalSpecialist"){
        params.logonName=unique_name;
    }

    const requestPayload = new RequestPayload(params);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast technicalSpecilistData');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });      
        if (response && response.code === "1") {   
            dispatch(actions.FetchTimeOffRequestData(response.result));
            if(response.result.length === 1){
                await  dispatch(FetchCategory(response.result[0].employmentType));
            }  
            dispatch(HideLoader());  
        }
    else {
        dispatch(HideLoader());
        IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast technicalSpecilistData');
    } 
};

export const FetchCategory = (inputdata) => async (dispatch) =>{
    const url = masterData.baseUrl + masterData.timeoffcategory;
    let params = {};
    const isActive=true;
    if (!isEmpty(inputdata)) {
        params={ 
            'employmentType':inputdata,
            'isActive':isActive //Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
        };
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(url, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast technicalSpecilistData');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
             });   
        if (response && response.code === "1") {   
            dispatch(actions.FetchCategory(response.result));
            return response;
        }
    }
    else {
        dispatch(actions.FetchCategory(null));
    }    
    
};

export const TimeOffRequestSave = (inputdata) => async (dispatch,getstate) => {
    dispatch(ShowLoader());
    const userType    = localStorage.getItem(applicationConstants.Authentication.USER_TYPE);
    if(userType === "TechnicalSpecialist"){
        inputdata.resourceName=`${ getstate().RootTechSpecReducer.TechSpecDetailReducer.resourceNameList[0].firstName } ${ getstate().RootTechSpecReducer.TechSpecDetailReducer.resourceNameList[0].lastName }`;
                inputdata.epin= getstate().RootTechSpecReducer.TechSpecDetailReducer.resourceNameList[0].epin;
    }
    inputdata.employmentType=getstate().RootTechSpecReducer.TechSpecDetailReducer.resourceNameList[0].employmentType;
    inputdata.recordStatus="N";
    const timeOffRequestValues=[];
    timeOffRequestValues.push(inputdata);
    if(timeOffRequestValues){
        const postUrl=GrmAPIConfig.grmBaseUrl + assignmentAPIConfig.techSpec + GrmAPIConfig.timeoffrequest;
        const requestPayload = new RequestPayload(timeOffRequestValues);
        const response = await PostData(postUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast empmentTypeData');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
        if (response && response.code == "1") {
            IntertekToaster(localConstant.techSpec.timeOffRequest.SAVED_SUCCESSFULLY, 'successToast TimeoffRequestSubmission');
            dispatch(TechSpechUnSavedData(true));
        }
    }
    dispatch(HideLoader());
};

export const getEmploymentType =(inputdata) => async (dispatch,getstate)=>{
    const params = {};
    if (!isEmpty(inputdata.epin)) {
        params.epin = inputdata.epin;
    }
    const url = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalSpecialists + GrmAPIConfig.info;
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast technicalSpecilistData');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });      
        if (response && response.code === "1") {  
            dispatch(actions.SaveResorceNameForManagers(response.result)); 
            if(response.result.length === 1){
                dispatch(FetchCategory(response.result[0].employmentType));
            }
        }
};