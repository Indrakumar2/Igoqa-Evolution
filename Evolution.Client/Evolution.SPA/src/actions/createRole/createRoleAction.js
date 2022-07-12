import { ViewRoleActionTypes, CreateRoleActionTypes } from '../../constants/actionTypes';
import { roleAPIConfig, RequestPayload } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData, PostData, CreateData } from '../../services/api/baseApiService';
import { getlocalizeData } from '../../utils/commonUtils';
import { ShowLoader, HideLoader,UpdateCurrentModule } from '../../common/commonAction';
import { ValidationAlert, SuccessAlert } from '../../components/viewComponents/customer/alertAction';

const localConstant = getlocalizeData();

const actions = {
    CreateRoleData: (payload) => ({
        type: CreateRoleActionTypes.CREATE_ROLE,
        data: payload
    }),   
    FetchModuleData: (payload) => ({
        type: CreateRoleActionTypes.FETCH_MODULE_DATA,
        data: payload
    }),
    FetchActivityData: (payload) => ({
        type: CreateRoleActionTypes.FETCH_ACTIVITY,
        data: payload
    }),
    FetchModuleActivityData: (payload) => ({
        type: CreateRoleActionTypes.FETCH_MODULE_ACTIVITY,
        data: payload
    }),
    FetchRoleActivityData: (payload) => ({
        type: CreateRoleActionTypes.FETCH_ROLE_ACTIVITY,
        data: payload
    }),
    FetchViewRole: (payload) => ({
        type: ViewRoleActionTypes.FETCH_VIEWROLE,
        data: payload
    }),
};

export const CreateRoleData = (data) => async (dispatch) => {
    dispatch(actions.CreateRoleData(data)); 
};

export const FetchRoleData = (roleId) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    const roleUrl = roleAPIConfig.roleBaseUrl + roleAPIConfig.Module;
    const  roleParams = {};
    const roleRequestPayload = new RequestPayload(roleParams);
    const roleResponse = await FetchData(roleUrl, roleRequestPayload)
        .catch(error => {     
            // console.error(error); // To show the error details in console                            
            // IntertekToaster(localConstant.errorMessages.VIEWROLE_FAILED, 'dangerToast viewRole');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (roleResponse && roleResponse.code === "1") {
        // D774 #20
        roleResponse.result.map(itratedValue => {
            if(itratedValue.moduleName === localConstant.security.userRole.TECHNICAL_SPECIALIST) {
                return itratedValue.moduleName = localConstant.resourceSearch.RESOURCE;
            } 
            return itratedValue.moduleName;
        });
        dispatch(actions.FetchModuleData(roleResponse.result));
    }
    //this code is commented because the data which is fetched is not used in CreateRole 
    // const activityUrl = roleAPIConfig.roleBaseUrl + roleAPIConfig.Activity;
    // const activityParams = {};
    // const activityRequestPayload = new RequestPayload(activityParams);

    // const activityResponse = await FetchData(activityUrl, activityRequestPayload)
    //     .catch(error => {            
    //         IntertekToaster(localConstant.errorMessages.VIEWROLE_FAILED, 'dangerToast viewRole');
    //     });    
    // if (activityResponse && activityResponse.code === "1") {        
    //     dispatch(actions.FetchActivityData(activityResponse.result));      
    // }
  
    if (state.CommonReducer.currentPage === localConstant.security.updateRole_CurrentPage) {
        const viewUrl = roleAPIConfig.roleBaseUrl + roleAPIConfig.Role;
        const viewParams = {};
        if (roleId > 0) {
            viewParams.roleId = roleId;
        }
        const viewRequestPayload = new RequestPayload(viewParams);
        const viewResponse = await FetchData(viewUrl, viewRequestPayload);
        if (viewResponse && viewResponse.code === "1") {
            dispatch(actions.FetchViewRole(viewResponse.result));
            dispatch(UpdateCurrentModule(localConstant.moduleName.SECURITY));
            const roleActivityUrl = roleAPIConfig.roleBaseUrl + roleAPIConfig.RoleActivity;
            const roleActivityParams = {};
            if (roleId > 0) {
                roleActivityParams.roleId = roleId;
            }
            const roleActivityRequestPayload = new RequestPayload(roleActivityParams);

            const roleActivityResponse = await FetchData(roleActivityUrl, roleActivityRequestPayload);
            if (roleActivityResponse && roleActivityResponse.code === "1") {
                dispatch(actions.FetchRoleActivityData(roleActivityResponse.result));
            }
        }
    }
    //fetch module activity data
    dispatch(FetchModuleActivityData());
    // const moduleUrl = roleAPIConfig.roleBaseUrl + roleAPIConfig.ModuleActivity;
    // const moduleParams = {};
    // const moduleRequestPayload = new RequestPayload(moduleParams);

    // const moduleResponse = await FetchData(moduleUrl, moduleRequestPayload)
    //     .catch(error => {            
    //         IntertekToaster(localConstant.errorMessages.VIEWROLE_FAILED, 'dangerToast viewRole');
    //     });    
    // if (moduleResponse && moduleResponse.code === "1") {        
    //     dispatch(actions.FetchModuleActivityData(moduleResponse.result));      
    // }
    dispatch(HideLoader());
};

export const FetchModuleData = (data) => async (dispatch) => {
    const Url = roleAPIConfig.roleBaseUrl + roleAPIConfig.Module;
    const  params = {};
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {  
            // console.error(error); // To show the error details in console          
            // IntertekToaster(localConstant.errorMessages.VIEWROLE_FAILED, 'dangerToast viewRole');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });    
    if (response && response.code === "1") {        
        dispatch(actions.FetchModuleData(response.result));      
        return response.result;
    }
    else {        
        IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast viewRoleSomthingWrong');
    }
};

export const FetchActivityData = (data) => async (dispatch) => {
    const Url = roleAPIConfig.roleBaseUrl + roleAPIConfig.Activity;
    const params = {};
    const requestPayload = new RequestPayload(params);

    const response = await FetchData(Url, requestPayload)
        .catch(error => {  
            // console.error(error); // To show the error details in console          
            // IntertekToaster(localConstant.errorMessages.VIEWROLE_FAILED, 'dangerToast viewRole');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });    
    if (response && response.code === "1") {        
        dispatch(actions.FetchActivityData(response.result));      
        return response.result;
    }
    else {        
        IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast viewRoleSomthingWrong');
    }
};

export const FetchModuleActivityData = (data) => async (dispatch) => {
    const Url = roleAPIConfig.roleBaseUrl + roleAPIConfig.ModuleActivity;
    const params = {};
    const requestPayload = new RequestPayload(params);
    let retresponse = null;
    const response = await FetchData(Url, requestPayload)
        .catch(error => {  
            // console.error(error); // To show the error details in console          
            // IntertekToaster(localConstant.errorMessages.VIEWROLE_FAILED, 'dangerToast viewRole');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });    
    if (response && response.code === "1") {   
        dispatch(actions.FetchModuleActivityData(response.result));      
        retresponse = response.result;
    }
    else {        
        IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast viewRoleSomthingWrong');
    }
    return retresponse;
};

export const FetchRoleActivityData = (data) => async (dispatch) => {
    const Url = roleAPIConfig.roleBaseUrl + roleAPIConfig.RoleActivity;
    const params = {};
    const requestPayload = new RequestPayload(params);

    const response = await FetchData(Url, requestPayload)
        .catch(error => {    
            // console.error(error); // To show the error details in console        
            // IntertekToaster(localConstant.errorMessages.VIEWROLE_FAILED, 'dangerToast viewRole');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });    
    if (response && response.code === "1") {        
        dispatch(actions.FetchRoleActivityData(response.result));      
        return response.result;
    }
    else {        
        IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast viewRoleSomthingWrong');
    }
};

export const AddRoleData = (payload) => async (dispatch) => {
    dispatch(ShowLoader());    
    const Url = roleAPIConfig.roleBaseUrl + roleAPIConfig.RoleActivity;
    const requestPayload = new RequestPayload(payload);
    const response = await PostData(Url, requestPayload)
        .catch(error => {  
            // console.error(error); // To show the error details in console    
            // IntertekToaster(localConstant.errorMessages.VIEWROLE_FAILED, 'dangerToast viewRole');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(SuccessAlert(response.result, "Role"));
        dispatch(HideLoader());
        return response;
    } else if (response.code == 11 || response.code == 41) {
        if (response.validationMessages.length > 0) {
            response.validationMessages.forEach(result => {
                if (result.messages.length > 0) {
                    result.messages.forEach(valMessage => {
                        dispatch(ValidationAlert(valMessage.message, "viewRole"));
                    });
                }
                else {
                    dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "viewRole"));
                }
            });
        }
        else if (response.messages.length > 0) {
            response.messages.forEach(result => {
                if (result.message.length > 0) {
                    dispatch(ValidationAlert(result.message, "viewRole"));
                }
            });
        }
        else {
            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "viewRole"));
        }
    } else {
        dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "viewRole"));
    }
    dispatch(HideLoader());
};

export const UpdateRoleData = (payload) => async (dispatch) => {
    dispatch(ShowLoader());  
    const Url = roleAPIConfig.roleBaseUrl + roleAPIConfig.RoleActivity;
    const requestPayload = new RequestPayload(payload);
    const response = await CreateData(Url, requestPayload)
        .catch(error => {      
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.VIEWROLE_FAILED, 'dangerToast viewRole');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
        if (response && response.code === "1") {
            dispatch(SuccessAlert(response.result, "Role"));
            dispatch(HideLoader());
            return response;
        } else if (response.code == 11 || response.code == 41) {
            if (response.validationMessages.length > 0) {
                response.validationMessages.forEach(result => {
                    if (result.messages.length > 0) {
                        result.messages.forEach(valMessage => {
                            dispatch(ValidationAlert(valMessage.message, "viewRole"));
                        });
                    }
                    else {
                        dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "viewRole"));
                    }
                });
            }
            else if (response.messages.length > 0) {
                response.messages.forEach(result => {
                    if (result.message.length > 0) {
                        dispatch(ValidationAlert(result.message, "viewRole"));
                    }
                });
            }
            else {
                dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "viewRole"));
            }
        } else {
            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "viewRole"));
        }
    dispatch(HideLoader());
};