import { FetchData,PostData } from '../../services/api/baseApiService';
import { projectActionTypes } from '../../constants/actionTypes';
import { projectAPIConfig,masterData,RequestPayload } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { isEmpty, isEmptyReturnDefault, getlocalizeData,parseValdiationMessage } from '../../utils/commonUtils';
import { required } from '../../utils/validator';
import arrayUtil from '../../utils/arrayUtil';
import { processMICoordinators } from '../../utils/commonUtils';
import { IsGRMMasterDataFetched,IsARSMasterDataLoaded,ShowLoader,HideLoader } from '../../common/commonAction';

const localConstant = getlocalizeData();

const actions = {
    FetchProjectCompanyOffices: (payload) => ({
        type: projectActionTypes.FETCH_PROJECT_COMPANY_OFFICES,
        data: payload
    }),
    FetchProjectCompanyDivision: (payload) =>({
        type: projectActionTypes.FETCH_PROJECT_COMPANY_DIVISION,
        data: payload
    }),
    FetchProjectCompanyCostCenter: (payload) =>({
        type: projectActionTypes.FETCH_PROJECT_COMPANY_COST_CENTER,
        data: payload
    }),
    UpdateProjectGeneralDetails: (payload) => ({
        type: projectActionTypes.UPDATE_PROJECT_GENERAL_DETAILS,
        data: payload
    }),
    FetchProjectCoordinator: (payload) => ({
        type: projectActionTypes.FETCH_PROJECT_COORDINATOR,
        data: payload
    }),
    FetchReportCoordinator: (payload) => ({
        type: projectActionTypes.FETCH_PROJECT_COORDINATOR,
        data: payload
    }),
    FetchProjectMICoordinator: (payload) => ({
        type: projectActionTypes.FETCH_PROJECT_MICOORDINATOR,
        data: payload
    }),
};

export const FetchProjectCompanyOffices = (data) => async(dispatch, getstate) => {    
    const state = getstate();
    const selectedCompany = getstate().appLayoutReducer.selectedCompany;
    // TODO: get the company code in data and bind it in the URL.
    const projectCompanyOfficeUrl = projectAPIConfig.companies +data+ "/"+projectAPIConfig.offices;
    const param = {};
    const requestPayload = new RequestPayload(param);
    const response =  await FetchData(projectCompanyOfficeUrl,requestPayload)
        .catch(error=>{
            // IntertekToaster(localConstant.validationMessage.PROJECT_COMPANY_OFFICE_API_VAL,'wariningToast projectCompanyOfficeVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if(!isEmpty(response)&&!isEmpty(response.code)){
        if(response.code == 1){
            const result = isEmpty(response.result) ? response.result : arrayUtil.sort(response.result,'officeName','asc');
            dispatch(actions.FetchProjectCompanyOffices(result));
        }
        else if (response.code === "11" || response.code === "41" || response.code === "31") {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast ProjectCompanyOffWentWrong');
        }
        else{
            IntertekToaster(localConstant.validationMessage.PROJECT_COMPANY_OFFICE_API_VAL, 'dangerToast projectCompanyOfficeSWWVal');
        }
    }
    else{
        IntertekToaster(localConstant.validationMessage.PROJECT_COMPANY_OFFICE_API_VAL, 'dangerToast projectCompanyOfficeSWWVal');
    }
};

export const FetchProjectCompanyDivision = (data) => async(dispatch,getstate) =>{
    dispatch(actions.FetchProjectCompanyDivision([]));
    dispatch(actions.FetchProjectCompanyCostCenter([]));
    const state = getstate();
    const selectedCompany = getstate().appLayoutReducer.selectedCompany;
    // TODO: get the company code in data and bind it in the URL.
    const projectCompanyDivisionUrl = projectAPIConfig.companies + data + "/"+projectAPIConfig.divisions;
    const param = {};
    const requestPayload = new RequestPayload(param);
    const response =  await FetchData(projectCompanyDivisionUrl,requestPayload)
        .catch(error=>{
            // IntertekToaster(localConstant.validationMessage.PROJECT_COMPANY_DIVISION_API_VAL,'wariningToast projectCompanyDivisionVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if(!isEmpty(response)&&!isEmpty(response.code)){
        if(response.code == 1){
            dispatch(actions.FetchProjectCompanyDivision(response.result));
        }
        else if (response.code === "11" || response.code === "41" || response.code === "31") {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast ProjectCompanyDivWentWrong');
        }
        else{
            IntertekToaster(localConstant.validationMessage.PROJECT_COMPANY_DIVISION_API_VAL, 'dangerToast projectCompanyDivisionSWWVal');
        }
    }
    else{
        IntertekToaster(localConstant.validationMessage.PROJECT_COMPANY_DIVISION_API_VAL, 'dangerToast projectCompanyDivisionSWWVal');
    }
};

export const FetchProjectCompanyCostCenter = (data) => async(dispatch,getstate) => {
    dispatch(actions.FetchProjectCompanyCostCenter([]));
    const state = getstate();
    // const selectedCompany = getstate().appLayoutReducer.selectedCompany;
    const selectedCompany=state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo.contractHoldingCompanyCode; //Changes For Defect 863(issue 6)
    // TODO: get the company code and division name in data and bind it in the URL.
    if(!required(data)){
        const projectCompanyCostCenterUrl = projectAPIConfig.companies + selectedCompany + "/"+projectAPIConfig.division +data+"/"+ projectAPIConfig.costCenter;
        const param = {
            "division": data  //Changes for D-996
        };
        const requestPayload = new RequestPayload(param);
        const response =  await FetchData(projectCompanyCostCenterUrl,requestPayload)
            .catch(error=>{
                // IntertekToaster(localConstant.validationMessage.PROJECT_COMPANY_COST_CENTER_API_VAL,'wariningToast projectCompanyCostCenterVal');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if(!isEmpty(response)&&!isEmpty(response.code)){
            if(response.code == 1){
                dispatch(actions.FetchProjectCompanyCostCenter(response.result));
            }
            else if (response.code === "11" || response.code === "41" || response.code === "31") {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast ProjectCompanyCCWentWrong');
            }
            else{
                IntertekToaster(localConstant.validationMessage.PROJECT_COMPANY_COST_CENTER_API_VAL, 'dangerToast projectCompanyCostCenterSWWVal');
            }
        }
        else{
            IntertekToaster(localConstant.validationMessage.PROJECT_COMPANY_COST_CENTER_API_VAL, 'dangerToast projectCompanyCostCenterSWWVal');
        }
    }
};

export const FetchReportCoordinator = (data) => async(dispatch,getstate) => {
    const state = getstate();
    const projectCoordinatorUrl = masterData.ssrsCoordinators;
    const requestPayload = new RequestPayload(data);
    dispatch(ShowLoader());
    const response = await PostData(projectCoordinatorUrl,requestPayload)
        .catch(error=>{
            // IntertekToaster(localConstant.validationMessage.PROJECT_COORDINATOR_API_VAL,'wariningToast projectCoordinatorVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
        if(!isEmpty(response)&&!isEmpty(response.code)){
            if(response.code == 1){
                const processedMICoordinators = processMICoordinators(response.result);
                dispatch(actions.FetchReportCoordinator(arrayUtil.sort(processedMICoordinators,"displayName","asc")));
                dispatch(HideLoader());
            }
            else if (response.code === "11" || response.code === "41" || response.code === "31") {
                dispatch(HideLoader());
                IntertekToaster(parseValdiationMessage(response), 'dangerToast ProjectCoWentWrong');
            }
            else{
                dispatch(HideLoader());
                IntertekToaster(localConstant.validationMessage.PROJECT_COORDINATOR_API_VAL, 'dangerToast projectCoordinatorSWWVal');
            }
        }
        else{
            dispatch(HideLoader());
            IntertekToaster(localConstant.validationMessage.PROJECT_COORDINATOR_API_VAL, 'dangerToast projectCoordinatorSWWVal');
        }
};

export const FetchProjectCoordinator = (data) => async(dispatch,getstate) => {
    const state = getstate();
    const selectedCompany = state.appLayoutReducer.selectedCompany;
    const projectCoordinatorUrl = projectAPIConfig.user;
    const param = {
        companyCode: selectedCompany,
        userType : 'MICoordinator',
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
                dispatch(actions.FetchProjectCoordinator(response.result));
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

export const UpdateProjectGeneralDetails = (data) => (dispatch,getstate) => {
    const state = getstate();
    const projectGeneralDetailInfo = state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo;
    if(isEmpty(projectGeneralDetailInfo)){
        dispatch(actions.UpdateProjectGeneralDetails({}));
    }
    const updatedProjectInfo = Object.assign({},state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo,data);
    dispatch(actions.UpdateProjectGeneralDetails(updatedProjectInfo));
};

export const FetchProjectMICoordinator = (data) =>async (dispatch,getstate) => {
    const state = getstate();
   if(data){
    dispatch(actions.FetchProjectMICoordinator([]));
    const projectCoordinatorUrl = masterData.miCoordinators;
    const param = data;
    const requestPayload = new RequestPayload(param);
    const response = await PostData(projectCoordinatorUrl,requestPayload)
        .catch(error=>{
            // IntertekToaster(localConstant.validationMessage.PROJECT_MICOORDINATOR_API_VAL,'wariningToast projectMICoordinatorVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
        if(!isEmpty(response)&&!isEmpty(response.code)){
            if(response.code == 1){
                dispatch(actions.FetchProjectMICoordinator(response.result));
            }
            else if (response.code === "11" || response.code === "41" || response.code === "31") {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast ProjectMICoWentWrong');
            }
            else{
                IntertekToaster(localConstant.validationMessage.PROJECT_MICOORDINATOR_API_VAL, 'dangerToast projectCoordinatorSWWVal');
            }
        }
        else{
            IntertekToaster(localConstant.validationMessage.PROJECT_MICOORDINATOR_API_VAL, 'dangerToast projectCoordinatorSWWVal');
        }
   }
};