import { ViewRoleActionTypes, CreateRoleActionTypes } from '../../constants/actionTypes';
import { roleAPIConfig, RequestPayload } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData, DeleteData } from '../../services/api/baseApiService';
import { getlocalizeData  } from '../../utils/commonUtils';
import { ShowLoader, HideLoader } from '../../common/commonAction';
import { ValidationAlert, SuccessAlert } from '../../components/viewComponents/customer/alertAction';

const localConstant = getlocalizeData();

const actions = {
    FetchViewRole: (payload) => ({
        type: ViewRoleActionTypes.FETCH_VIEWROLE,
        data: payload
    }),
    DeleteRoleDetails: (payload) => (
        {
            type: ViewRoleActionTypes.DELETE_ROLE_DETAILS,
            data: payload
        }
    ),
    selectedRowData:(payload)=>(
        {
            type: ViewRoleActionTypes.SELECTED_ROW_DETAILS,
            data: payload
        }
    ),
    FetchRoleActivityData: (payload) => ({
        type: CreateRoleActionTypes.FETCH_ROLE_ACTIVITY,
        data: payload
    })
};

export const FetchViewRole = () => async (dispatch) => {       
    const Url = roleAPIConfig.roleBaseUrl + roleAPIConfig.Role;
    const  params = {};
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {   
            // console.error(error); // To show the error details in console         
            // IntertekToaster(localConstant.errorMessages.VIEWROLE_FAILED, 'dangerToast viewRole');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {   
        dispatch(actions.FetchViewRole(response.result));
        //This Api call is not required on displaying user roles.
        // const roleActivityUrl = roleAPIConfig.roleBaseUrl + roleAPIConfig.RoleActivity;
        // const roleActivityParams = {};
        // const roleActivityRequestPayload = new RequestPayload(roleActivityParams);

        // const roleActivityResponse = await FetchData(roleActivityUrl, roleActivityRequestPayload)
        //     .catch(error => {            
        //         IntertekToaster(localConstant.errorMessages.VIEWROLE_FAILED, 'dangerToast viewRole');
        //     });    
        // if (roleActivityResponse && roleActivityResponse.code === "1") {        
        //     dispatch(actions.FetchRoleActivityData(roleActivityResponse.result));      
        // }

        return response.result;
    }
    else {        
        IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast viewRoleSomthingWrong');
    }
};

export const DeleteRoleDetails = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    const newState = Object.assign([], state.rootAdminReducer.roleData);  
    data.map(row => {        
        row.recordStatus = "D";
    });
    const Url = roleAPIConfig.roleBaseUrl + roleAPIConfig.Role;
    const params = {
        data: data,
    };
    const requestPayload = new RequestPayload(params);    
    const response = await DeleteData(Url, requestPayload)
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
    dispatch(actions.DeleteRoleDetails(newState));
};

export const selectedRowData = (data) => async (dispatch) => {
    dispatch(actions.selectedRowData(data));
};