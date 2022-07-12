import { techSpecActionTypes } from '../../constants/actionTypes';
import { GrmAPIConfig, RequestPayload } from '../../../apiConfig/apiConfig';
import { isEmpty, getlocalizeData, isEmptyReturnDefault, isEmptyOrUndefine } from '../../../utils/commonUtils';
import { FetchData } from '../../../services/api/baseApiService';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';
import { UpdateCurrentPage, UpdateCurrentModule,SetCurrentPageMode } from '../../../common/commonAction';
import { UpdateTechSpecInfo,FetchSelectedProfileTaxnomyHistoryCount } from '../techSpec/techSpecDetailAction';
import { applicationConstants } from '../../../constants/appConstants';
import { commonActionTypes } from '../../../constants/actionTypes';
const localConstant = getlocalizeData();

const actions = {
    FetchTechSpecData: (payload) => (
        {
            type: techSpecActionTypes.techSpecSearch.TECH_SPEC_SERACH_DATA,
            data:payload
        }
    ),
    GetSelectedProfile:(payload)=>(
        {
            type:techSpecActionTypes.techSpecSearch.GET_TECH_SPEC_SELECTED_PROFILE,
            data:payload
        }
    ),
    GetSelectedDraftProfile:(payload)=>(
        {
            type:techSpecActionTypes.techSpecSearch.GET_TECH_SPEC_DRAFT_SELECTED_PROFILE,
            data:payload
        }
    ),
    TechSpecClearSearch:(payload)=>(
        {
            type:techSpecActionTypes.techSpecSearch.CLEAR_TECHSPEC_DATA,
            data:payload
        }
    ),
    SetSelectedMyTaskDraftRefCode:(payload)=>(
        {
            type:techSpecActionTypes.techSpecSearch.TECH_SPEC_MYTASK_REFCODE,
            data:payload
        }
    ),
    
    SetSelectedMyTaskDraftType:(payload)=>(
        {
            type:techSpecActionTypes.techSpecSearch.TECH_SPEC_MYTASK_TYPE,
            data:payload
        }),
    SaveSelectedTechSpecEpin: (payload) => ({
            type: techSpecActionTypes.techSpecSearch.SAVE_SELECTED_EPIN,
            data: payload
        }),
    SetProfileActionType: (payload) => ({
            type: techSpecActionTypes.professionalDetailsActionTypes.ADD_PROFILE_ACTION_TYPE,
            data: payload
        }),
    UpdateInteractionMode: (payload) => ({
            type: commonActionTypes.INTERACTION_MODE,
            data: payload
    }),
   };

export const GetSelectedProfile = (payload) => async (dispatch) => {  
    dispatch(UpdateCurrentPage(localConstant.techSpec.common.EDIT_VIEW_TECHSPEC));
    dispatch(UpdateCurrentModule("grm"));
    if(payload.viewMode === "true"){ //Resource are  Open from NDT,ARS,QucikSearch and PreAssignment sholud be in view mode
        dispatch(SetCurrentPageMode("View"));
    } else {
        dispatch(SetCurrentPageMode());
    }
    dispatch(actions.GetSelectedProfile(payload));
    return true;    
};
export const TechSpecClearSearch = (payload) => (dispatch) => {
    dispatch(actions.TechSpecClearSearch(payload));
    dispatch(SetCurrentPageMode(null));
};

export const FetchTechSpecData = (data,isFromAssignment) => async (dispatch,getstate) => { 
    const state = getstate();
    const cHCompany=state.appLayoutReducer.selectedCompany;
    const currentPage = state.CommonReducer.currentPage;
    dispatch(actions.FetchTechSpecData(null));
    const url = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalSpecialists + GrmAPIConfig.search;
    const params = {};
    if(currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE || currentPage === localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE){
        params.profileStatus = "Active";
    }
    if (!isEmpty(data.firstName)) {
        params.firstName = data.firstName;
    }
    if (!isEmpty(data.lastName)) {
        params.lastName = data.lastName;
    }   
    if (!isEmpty(data.fullAddress)) {
        params.fullAddress = data.fullAddress;
    }
    if (data.countryId) { //Added for ITK D1536
        params.countryId = data.countryId;
    }
    if (data.countyId) { //Added for ITK D1536
        params.countyId = data.countyId;
    }
    if (data.cityId) { //Added for ITK D1536
        params.cityId = data.cityId;
    }
    if (!isEmpty(data.postalCode)) {
        params.PinCode = data.postalCode;
    }
    if (!isEmpty(data.pin)) {
        params.ePinString = data.pin;
    }
    if (!isEmpty(data.employmentStatus)) {
        params.EmploymentType = data.employmentStatus;
    }
    if (!isEmpty(data.profileStatus)) {
        params.profileStatus = data.profileStatus;
    }
    if (!isEmpty(data.technicalDiscipline)) {
        params.technicalDiscipline = data.technicalDiscipline;
    }
    if (!isEmpty(data.category)) {
        params.category = data.category;
    }   
    if (!isEmpty(data.subCategory)) {
        params.subCategory = data.subCategory;
    }
    if (!isEmpty(data.service)) {
        params.service = data.service;
    }
    if(!isEmpty(data.logonName))
    {
        params.logonName = data.logonName;
    }
    if(!isEmpty(data.companyName))
    {
        params.companyName = data.companyName;
    }
    /** Changes for Hot Fixes on NDT */
    if(!isEmpty(data.companyCode)){
        params.companyCode=data.companyCode;
    }
    if (!isEmpty(data.searchDocumentType )) {
        params.searchDocumentType= data.searchDocumentType;
    }
    if (!isEmpty(data.documentSearchText)) {
        params.documentSearchText= data.documentSearchText;
    }
        //params.companyCode=cHCompany; 
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast technicalSpecilistData');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });      
        if (response && response.code === "1") {   
            if(isFromAssignment){
                const assignmentResources = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentTechnicalSpecialists).filter(x => x.recordStatus !== 'D');
                const resourceSearchResult = isEmptyReturnDefault(response.result);
                const filteredResources = resourceSearchResult.filter(x => !(assignmentResources.some(x1 => x1.epin === x.epin)));
                let filteredData = isEmptyReturnDefault(filteredResources);
                filteredData = filteredData.map((parentObj) => {
                    if (!isEmptyOrUndefine(parentObj.technicalSpecialistContact) && parentObj.technicalSpecialistContact.length > 0) {
                        parentObj.technicalSpecialistContact
                            .map((obj) => {
                                if (obj.contactType === 'PrimaryAddress') {
                                    parentObj.country = obj.country;
                                    parentObj.county = obj.county;
                                    parentObj.city = obj.city;
                                    parentObj.pinCode = obj.postalCode;
                                    parentObj.fullAddress = obj.address;
                                }
                                if (obj.contactType === 'PrimaryMobile') {
                                    parentObj.mobileNumber = obj.mobileNumber;
                                }
                                if (obj.contactType === 'PrimaryEmail') {
                                    parentObj.emailAddress = obj.emailAddress;
                                }
                                return parentObj;
                            });
                    }
                    return parentObj;
                }
                );
                dispatch(actions.FetchTechSpecData(filteredData));
            }else{
                //Changes - D1332
                if (!isEmptyOrUndefine(response.result)) {
                    response.result = response.result.map((parentObj) => {
                        if (!isEmptyOrUndefine(parentObj.technicalSpecialistContact) && parentObj.technicalSpecialistContact.length > 0) {
                            parentObj.technicalSpecialistContact
                                .map((obj) => {
                                    if (obj.contactType === 'PrimaryAddress') {
                                        parentObj.country = obj.country;
                                        parentObj.county = obj.county;
                                        parentObj.city = obj.city;
                                        parentObj.pinCode = obj.postalCode;
                                        parentObj.fullAddress = obj.address;
                                    }
                                    if (obj.contactType === 'PrimaryMobile') {
                                        parentObj.mobileNumber = obj.mobileNumber;
                                    }
                                    if (obj.contactType === 'PrimaryEmail') {
                                        parentObj.emailAddress = obj.emailAddress;
                                    }
                                    return parentObj;
                                });
                        }
                        return parentObj;
                    }
                    );
                    dispatch(actions.FetchTechSpecData(response.result));
                } else{
                    dispatch(actions.FetchTechSpecData([])); //Added For ITK D1340
                }
            }     
            return response;
        }
    else {
        IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast technicalSpecilistData');
    }
};
export const GetSelectedDraftProfile=(data)=>async(dispatch)=>{
    //const url="http://localhost:5101/api/technicalSpecialists/"+data+"/draft";
    dispatch(UpdateCurrentPage(localConstant.techSpec.common.CREATE_PROFILE));
    dispatch(UpdateCurrentModule("grm"));
    const url= GrmAPIConfig.grmBaseUrl+GrmAPIConfig.technicalSpecilists+data+GrmAPIConfig.draft;
    const params={  };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast technicalSpecilistData');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });      
        if (response && response.code === "1") {    
          
            const res=response.result &&response.result[0].serilizableObject;  
            dispatch(actions.SetSelectedMyTaskDraftRefCode(data));
            dispatch(actions.GetSelectedDraftProfile(JSON.parse(res)));        
        }
    else {
        IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast technicalSpecilistData');
    }

};

export const FetchSavedDraftProfile=(data,currentPage,draftType)=>async(dispatch)=>{  
    dispatch(UpdateCurrentPage(currentPage));
    dispatch(UpdateCurrentModule("grm"));
    const username = localStorage.getItem(applicationConstants.Authentication.USER_NAME);
    const url= GrmAPIConfig.grmBaseUrl+GrmAPIConfig.technicalSpecilists+data+GrmAPIConfig.draft+ '?draftType='+draftType;
    const params={  };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast technicalSpecilistData');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });      
        if (response && response.code === "1" && response.result && Array.isArray(response.result) && response.result.length>0) {    
            const res=JSON.parse(response.result[0].serilizableObject);
            res.technicalSpecialistInfo.draftId  =response.result[0].draftId;
            res.technicalSpecialistInfo.epin  =response.result[0].draftId;
            res.technicalSpecialistInfo.pendingWithUser  =response.result[0].assignedTo;
            dispatch(actions.SetSelectedMyTaskDraftType(draftType));
            dispatch(actions.SetSelectedMyTaskDraftRefCode(data));
            dispatch(actions.SaveSelectedTechSpecEpin(data)); //for scenario fixes (ref mail Scenario Defects - Resource on18-03-2020)
            dispatch(actions.GetSelectedDraftProfile(res)); 
            dispatch(actions.SetProfileActionType(res.technicalSpecialistInfo && res.technicalSpecialistInfo.profileAction));     
            if(res.technicalSpecialistInfo.approvalStatus === localConstant.techSpec.tsChangeApprovalStatus.UpdateAfterReject){
                const newValue ={};
                newValue.approvalStatus = localConstant.techSpec.tsChangeApprovalStatus.Rejected;
                newValue.isTSApprovalRequired = true;
                dispatch(UpdateTechSpecInfo(newValue));
            }
            if(res.technicalSpecialistInfo.approvalStatus === localConstant.techSpec.tsChangeApprovalStatus.InProgress){
                const newValue ={};
                newValue.isTSApprovalRequired = true;
                dispatch(UpdateTechSpecInfo(newValue));
            } //Sanity Def 125
            if(res.technicalSpecialistInfo.pendingWithUser !== null && response.result[0].assignedTo !== username){ //--IGOQC 951 (13-01-2021) //D1301
                dispatch(actions.UpdateInteractionMode(true));
            } //D1301
            //REf Mail (Sub:RESOURCE MODULE  on 09-10-2020 16:09)
            dispatch(FetchSelectedProfileTaxnomyHistoryCount(res.technicalSpecialistInfo.epin));
        }
    else if(draftType != "TS_EditProfile") {
        IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast technicalSpecilistData');
    } 
    return response; 
};