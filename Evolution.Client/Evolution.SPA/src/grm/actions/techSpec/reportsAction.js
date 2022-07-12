import { techSpecActionTypes } from '../../constants/actionTypes';
import { GrmAPIConfig, RequestPayload, projectAPIConfig,masterData } from '../../../apiConfig/apiConfig';
import { isEmpty, getlocalizeData, parseValdiationMessage } from '../../../utils/commonUtils';
import { ShowLoader, HideLoader } from '../../../common/commonAction';

import { FetchData,PostData } from '../../../services/api/baseApiService';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';

const localConstant = getlocalizeData();

const actions = {
    FetchCalendarScheduleDetails: (payload) => ({
        type: techSpecActionTypes.FETCH_CALENDAR_SCHEDULES_DATA,
        data: payload
    }),
    FetchCompanySpecificMatrixDetails: (payload)  =>({
        type: techSpecActionTypes.FETCH_COMPANY_SPECIFIC_MATRIX_DATA,
        data: payload
    }),
    FetchTaxonomyReport :(payload) =>({
        type: techSpecActionTypes.FETCH_TAXONOMY_REPORT,
        data: payload
    }),
    FetchResourceInfo:(payload)=>({
        type: techSpecActionTypes.FETCH_TS_BASED_ON_COMPANY,
        data: payload
    }),
    ClearSearchData:(payload)=>({
        type: techSpecActionTypes.CLEAR_SEARCH_DATA,
        data: payload
    }),
    FetchContractHoldingCoordinator:(payload) => ({
        type: techSpecActionTypes.FETCH_CONTRACT_HOLDING_COORDINATOR,
        data: payload
    }),
};

export const FetchCalendarScheduleDetails = (data) => async(dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    const URL = GrmAPIConfig.calendarScheduleDetails;
    const companies = data.selectedCompanies;
    const companyList = companies ? companies : state.appLayoutReducer.companyList;
    let companyIdS = [];
    if(!isEmpty(companyList)){
        companyList.forEach(x => {
            companyIdS.push(x.id);
        });
    }
    companyIdS = companyIdS.join(',');
    companyIdS = companyIdS.replace(/"/g,"");
    const param = {
        companyID: companyIdS,
        month:data.month,
        year:data.year,
    };
    if(!isEmpty(data.customerName)){
        param.customerName = data.customerName;
    }
    if(!isEmpty(data.supplierName)){
        param.supplierName = data.supplierName;
    }
    if(!isEmpty(data.projectNumber)){
        param.projectNo = data.projectNumber;
    }
    if(!isEmpty(data.assignmentNumber)){
        param.assignmentNo = data.assignmentNumber;
    }
    if(!isEmpty(data.chCoordinator)){
        param.CHCoordinator = data.chCoordinator;
    }
    if(!isEmpty(data.ocCoordinator)){
        param.OCCoordinator = data.ocCoordinator;
    }
    if(!isEmpty(data.resourceEpins)){
        param.resourceEpins = data.resourceEpins;
    }
    if(!isEmpty(data.epinList)){
        param.EPins = data.epinList;
    }
    const requestPayload = new RequestPayload(param);
    const response = await PostData(URL,requestPayload)
         .catch(error=>{
            //  IntertekToaster(localConstant.validationMessage.CALENDAR_SCHEDULE_DETAILS_REPORT_FAILURE,'warningToast calendarScheduleReportFails');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
         });
         if(!isEmpty(response) && !isEmpty(response.code)){
             if(response.code === "1"){
                 dispatch(actions.FetchCalendarScheduleDetails(response.result));
             }
             else if(response.code === "11" || response.code === "41" || response.code === "31"){
                IntertekToaster(parseValdiationMessage(response), 'dangerToast ReportFetchWentWrong');
             }
             else{
                 IntertekToaster(localConstant.validationMessage.CALENDAR_SCHEDULE_DETAILS_REPORT_FAILURE,'dangerToast ReportFetchWentWrong');
             }
             dispatch(HideLoader());
         }
         else{
            IntertekToaster(localConstant.validationMessage.CALENDAR_SCHEDULE_DETAILS_REPORT_FAILURE, 'dangerToast ReportFetchWentWrong');
            dispatch(HideLoader());
        }
};

export const FetchCompanySpecificMatrixDetails = (isByResource,companies) => async(dispatch,getstate) => { 
    dispatch(ShowLoader());   
    const state=getstate();
    const URL = GrmAPIConfig.companySpecificMatrixReport;
    const companyList = companies ? companies :state.appLayoutReducer.userRoleCompanyList;
    const companyCodes= [];
    const isResourceBased = isByResource !==undefined ?isByResource :true;
    if(!isEmpty(companyList))
    {
        companyList.forEach(x => {
            companyCodes.push(x.companyCode);
        });
    }
    const param = {
        companyCode: companyCodes.join(','),
        isByResource:isResourceBased
    };
    const requestPayload = new RequestPayload(param);
    const response =  await PostData(URL,requestPayload)
        .catch(error=>{
            // IntertekToaster(localConstant.validationMessage.COMPANY_SPECIFIC_MATRIX_REPORT_FAILURE,'wariningToast companySpecificMatrixReportFails');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
        if(!isEmpty(response)&&!isEmpty(response.code)){
            if(response.code == 1){
               dispatch(actions.FetchCompanySpecificMatrixDetails(response.result));
            }
            else if (response.code === "11" || response.code === "41" || response.code === "31") {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast ReportFetchWentWrong');
            }
            else{
                IntertekToaster(localConstant.validationMessage.COMPANY_SPECIFIC_MATRIX_REPORT_FAILURE, 'dangerToast ReportFetchWentWrong');
            }
            dispatch(HideLoader());
        }
        else{
            IntertekToaster(localConstant.validationMessage.COMPANY_SPECIFIC_MATRIX_REPORT_FAILURE, 'dangerToast ReportFetchWentWrong');
            dispatch(HideLoader());
        }
};

export const FetchResourceInfo= (companyIds, isActive) => async(dispatch,getstate) => { 
    dispatch(ShowLoader());   
    const state=getstate();
    const URL = GrmAPIConfig.grmBaseUrl + GrmAPIConfig.technicalSpecialists + 'GetTSBasedOnCompany?isActive=' +isActive;
    const params = {};
    params.data =companyIds;
    const response = await PostData(URL, params)
    .catch(error => {
        // IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast technicalSpecilistData');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });      
    if (response && response.code === "1") {   
            if(response.code == 1){
                const resourceList=[];
                const result=response.result;
                if(result && result.length>0){
                    result.forEach(iteratedValue =>{
                        const resourceName={
                            'fullName':` ${ iteratedValue.lastName },${ iteratedValue.firstName } (${ iteratedValue.pin }) `,
                        };
                        const value=Object.assign({},iteratedValue,resourceName);
                        resourceList.push(value);
                });
                }
               dispatch(actions.FetchResourceInfo(resourceList));
            }
            else if (response.code === "11" || response.code === "41" || response.code === "31") {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast ReportFetchWentWrong');
            }
            else{
                IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast ReportFetchWentWrong');
            }
            dispatch(HideLoader());
        }
        else{
            IntertekToaster(localConstant.errorMessages.TS_DATA_NOT_FETCHED, 'dangerToast ReportFetchWentWrong');
            dispatch(HideLoader());
        }
};

export const FetchTaxonomyReport= (searchData) => async(dispatch,getstate) => { 
    dispatch(ShowLoader());   
    const state=getstate();
    const URL = GrmAPIConfig.taxonomyReport;
    const params = {};
    if (!isEmpty(searchData.companyIds)) {
        params.CompanyIds=searchData.companyIds;
    }
    if (!isEmpty(searchData.resourceEpins)) {
        params.resourceEpins=searchData.resourceEpins;
    }
    if(!isEmpty(searchData.epinList)) {
        params.epinList = searchData.epinList;
    }
    // if (!isEmpty(searchData.companyCode)) {
    //     params.companyIds=searchData.companyIds;
    // }
    if (!isEmpty(searchData.employmentStatus)) {
        params.employmentStatus = searchData.employmentStatus;
    }
    if (!isEmpty(searchData.profileStatus)) {
        params.profileStatus = searchData.profileStatus;
    }
    if (!isEmpty(searchData.approvalStatus)) {
        params.approvalStatus = searchData.approvalStatus;
    }
    if (!isEmpty(searchData.category)) {
        params.category = searchData.category;
    }   
    if (!isEmpty(searchData.subCategory)) {
        params.subCategory = searchData.subCategory;
    }
    if (!isEmpty(searchData.service)) {
        params.service = searchData.service;
    }
    const requestPayload = new RequestPayload(params);
    const response = await PostData(URL, requestPayload)
    .catch(error => {
        // IntertekToaster(localConstant.validationMessage.TAXONOMY_REPORT_FAILURE, 'dangerToast taxonomyReport');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });      
    if (response && response.code === "1") {
        if (response.code == 1) {
            dispatch(actions.FetchTaxonomyReport(response.result));
        }
        else if (response.code === "11" || response.code === "41" || response.code === "31") {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast taxonomyReport');
        }
        else {
            IntertekToaster(localConstant.validationMessage.TAXONOMY_REPORT_FAILURE, 'dangerToast taxonomyReport');
        }
        dispatch(HideLoader());
    }
    else {
        IntertekToaster(localConstant.validationMessage.TAXONOMY_REPORT_FAILURE, 'dangerToast taxonomyReport');
        dispatch(HideLoader());
    }
};

export const ClearSearchData = () => (dispatch) => {
    dispatch(actions.ClearSearchData());
};

/** Fetch Coordinator for Contract Holding Company action - Calendar Schedule Report */
export const FetchContractHoldingCoordinator = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());

    // MI Coordinator API call starts here
    dispatch(actions.FetchContractHoldingCoordinator([]));
    //dispatch(actions.FetchOperatingCoordinator([])); 
    if(data){
        const coordinatorUrl = masterData.reportCoordinators;
        const param = data; 
        const requestPayload = new RequestPayload(param);
        const response = await PostData(coordinatorUrl,requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION, 'wariningToast HoldingCompanyCoordinatorValPreAssign');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
        if (!isEmpty(response)) {
            if (response.code === "1") {
                dispatch(actions.FetchContractHoldingCoordinator(response.result));
            }
            else {
                IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION, 'wariningToast HoldingCompanyCoordinatorValPreAssign');
            }
            dispatch(HideLoader());
        }
        else {
            IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION, 'dangerToast HoldingCompanyCoordinatorSWWValPreAssign');
            dispatch(HideLoader());
        }
    }
    dispatch(HideLoader());
};