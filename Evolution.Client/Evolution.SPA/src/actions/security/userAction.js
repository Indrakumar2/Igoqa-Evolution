import {
    FetchData,
    PostData,
    CreateData,
    DeleteData
} from '../../services/api/baseApiService';
import { userAPIConfig, RequestPayload } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { userActionTypes } from '../../constants/actionTypes';
import { ShowLoader, HideLoader,UpdateInteractionMode,UpdateCurrentModule } from '../../common/commonAction';
import {
    getlocalizeData,
    isEmpty,
    isUndefined,
    isEmptyOrUndefine,
    parseValdiationMessage,
    FilterSave
} from '../../utils/commonUtils';
import { getInitialUserDetailJson } from '../../utils/jsonUtil';
import arrayUtil from '../../utils/arrayUtil';

const localConstant = getlocalizeData();

const actions = {
    SetUserLandingPageData: (payload) => ({
        type: userActionTypes.SET_USER_LANDING_PAGE_DATA,
        data: payload
    }),
    SetUserDetailData: (payload) => ({
        type: userActionTypes.SET_USER_DETAIL_DATA,
        data: payload
    }),
    SetCompanyOffices: (payload) => ({
        type: userActionTypes.FETCH_COMPANY_OFFICE,
        data: payload
    }),
    SetRoles: (payload) => ({
        type: userActionTypes.FETCH_ROLE,
        data: payload
    }),
    SetUserCurrentAction: (payload) => ({
        type: userActionTypes.SET_USER_CURRENT_ACTION,
        data: payload
    }),
    UserUnSavedDatas: (payload) => ({
        type: userActionTypes.USER_UNSAVED_DATA,
        data: payload
    }),
};
export const SearchUser = () => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const fetchUrl = userAPIConfig.user + "?isGetAllUser=true&excludeUserTypes=TechnicalSpecialist&excludeUserTypes=Customer";
    const params = {};
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(fetchUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast conActDataSomethingWrong');
            dispatch(HideLoader());
        });

    if (!isUndefined(response) && !isUndefined(response.code) && response.code == "1") {
        const result = arrayUtil.sort(response.result, 'userName', 'asc');
        dispatch(actions.SetUserLandingPageData(result));
        // dispatch(UpdateInteractionMode(true));
        dispatch(HideLoader());
    }
    else {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast conActDataSomethingWrong');
        dispatch(HideLoader());
    }
};

export const SearchRoles = () => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const fetchUrl = userAPIConfig.role;
    const params = {};
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(fetchUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast conActDataSomethingWrong');
            dispatch(HideLoader());
            return false;
        });

    if (!isUndefined(response) && !isUndefined(response.code) && response.code == "1") {
        let result = null;
        if (!isEmptyOrUndefine(response.result)) {
            result = arrayUtil.sort(response.result, 'roleName', 'asc');
            result.map(x => {
                x.isSelected = null;
                x.companyCode = null;
            });
        }
        dispatch(actions.SetRoles(result));
        dispatch(HideLoader());
        return true;
    }
    else {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast conActDataSomethingWrong');
        dispatch(HideLoader());
        return false;
    }
};

export const FetchCompanyOffice = () => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const fetchUrl = userAPIConfig.companyOffice.replace('{companyCode}', ' ');
    const params = {};
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(fetchUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast conActDataSomethingWrong');
            dispatch(HideLoader());
            return false;
        });

    if (!isUndefined(response) && !isUndefined(response.code) && response.code == "1") {
        dispatch(actions.SetCompanyOffices(response.result));
        dispatch(HideLoader());
        return true;
    }
    else {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast conActDataSomethingWrong');
        dispatch(HideLoader());
        return false;
    }
};

export const SetUserDetailData = (userLogonName) => async (dispatch, getstate) => {   
    dispatch(ShowLoader());
    const fetchUrl = userAPIConfig.userDetail;
    const params = { "LogonName": userLogonName };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(fetchUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FETCH_USER_FAILED, 'dangerToast conActDataNotFound');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
            return false;
        });

    if (!isUndefined(response) && !isUndefined(response.code) && response.code == "1") {
        let result = getInitialUserDetailJson();
        if (!isEmptyOrUndefine(response.result) && response.result.length > 0) {
            response.result.map((x) => {
                x.user.recordStatus = 'M';
                if (!isEmptyOrUndefine(x.companyRoles) && x.companyRoles.length > 0) {
                    x.companyRoles.map(x1 => {
                        x1.roles.map(x2 => {
                            x2.recordStatus = 'M';
                        });
                    });
                }
            });

            result = response.result[0];
        }     
        dispatch(actions.SetUserDetailData(result));
        dispatch(UpdateCurrentModule(localConstant.moduleName.SECURITY));
        dispatch(HideLoader());
        return true;
    }
    else {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast conActDataSomethingWrong');
        dispatch(HideLoader());
        return false;
    }
};

export const ResetUserDetailState = () => async (dispatch, getstate) => {
    dispatch(actions.SetUserDetailData(getInitialUserDetailJson()));
};

export const ResetCompanyOfficeState = () => async (dispatch, getstate) => {
    dispatch(actions.SetCompanyOffices([]));
};

export const ResetUserLandingPageDataState = () => async (dispatch, getstate) => {
    dispatch(actions.SetUserLandingPageData({}));
};

export const ResetRolesState = () => async (dispatch, getstate) => {
    dispatch(actions.SetRoles([]));
};

export const SaveUser = (userData) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const params = [];    
    params.push(FilterSave(userData));
    const fetchUrl = userAPIConfig.userDetail;
    const requestPayload = new RequestPayload(params);
    let response = null;
    if (userData.user.recordStatus === 'M') {     
        response = CreateData(fetchUrl, requestPayload);
    }
    else {
        response = PostData(fetchUrl, requestPayload);
    }
    response = await response
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast conActDataSomethingWrong');
            dispatch(HideLoader());
            return false;
        });

    if (!isUndefined(response) && !isUndefined(response.code) && response.code == "1") {
        IntertekToaster(localConstant.commonConstants.RECORD_SAVE_SUCCESS, 'successToast');
        dispatch(HideLoader());
        return true;
    }
    else {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast conActDataSomethingWrong');
        dispatch(HideLoader());
        return false;
    }
};

export const DeleteUser = (userNames) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const fetchUrl = userAPIConfig.userDetail;
    const params = {
        data: userNames,
    };
    const requestPayload = new RequestPayload(params);    
    let response = DeleteData(fetchUrl, requestPayload);
    response = await response
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast conActDataSomethingWrong');
            dispatch(HideLoader());
            return false;
        });

    if (!isUndefined(response) && !isUndefined(response.code) && response.code == "1") {
        IntertekToaster(localConstant.commonConstants.RECORD_DELETED_SUCCESS, 'successToast');
        dispatch(HideLoader());
        return true;
    }
    else {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast conActDataSomethingWrong');
        dispatch(HideLoader());
        return false;
    }
};
export const UserUnSavedDatas = (isChanged) => async (dispatch, getstate) => {
    dispatch(actions.UserUnSavedDatas(isChanged));
};