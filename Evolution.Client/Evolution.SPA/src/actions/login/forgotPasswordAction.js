import { loginActionTypes } from '../../constants/actionTypes';
import { RequestPayload, loginApiConfig } from '../../apiConfig/apiConfig';
import { PostData, FetchData } from '../../services/api/baseApiService';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import authService from '../../authService';
import { authenticateLoginSuccess } from './loginAction';
import { getlocalizeData, parseValdiationMessage, isEmpty, isEmptyReturnDefault } from '../../utils/commonUtils';
import { ShowLoader, HideLoader } from '../../common/commonAction';
const localConstant = getlocalizeData();

const actions = {
    securityQuestion: (payload) => {
        return {
            type: loginActionTypes.SECURITY_QUESTION,
            data: payload
        };
    },
    loginUserName: (payload) => {
        return {
            type: loginActionTypes.LOGIN_USERNAME,
            data: payload
        };
    }
};

export const securityQuestion = (data) => async (dispatch, getstate) => {
    const securityQuestionUrl = loginApiConfig.securityQuestion;
    const params = {
        applicatName: localConstant.commonConstants.APP_NAME,
        userLogonName: data.Username,
        userEmail: data.UserEmail
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(securityQuestionUrl, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_SUPPLIER_PO_DATA, 'dangerToast FetchAssignmentSearchResultsErr');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });

    if (!isEmpty(response)) {
        if (response.code === "1") {
            dispatch(actions.securityQuestion(response.result));
            return true;
        }
        else {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast');
            return false;
        }
    }

};

export const validateSecurityQuestionAnswer = (data) => async (dispatch) => {
    const signInByQuestion = loginApiConfig.signInByQuestion;
    const params = {
        'application': localConstant.commonConstants.APP_NAME,
        'userLogonName': data.Username,
        'userEmail': data.UserEmail,
        'questionAnswers': [
            {
                'question': data.Question,
                'answer': data.Answer
            }
        ]
    };
    const requestPayload = new RequestPayload(params);
    dispatch(ShowLoader());
    const response = await PostData(signInByQuestion, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FETCH_ASSIGNMENT_VISIT_DOC_FAILED, 'wariningToast fetchAssignVisitVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            return true;
        }

        else if (response.code == 11) {
            if (response.messages.length > 0) {
                response.messages.map((result, index) => {

                    IntertekToaster(result.message, 'warningToast');
                });
            }
        }
        else {

            IntertekToaster(parseValdiationMessage(response), 'dangerToast');
            dispatch(HideLoader());
            return false;
        }

    }

};
export const loginUserName = (data) =>  (dispatch, getstate) => {
    dispatch(actions.loginUserName(data));
};

export const resetPassword = (data) => async (dispatch) => {
    const resetPassword = loginApiConfig.resetPassword;
    const params = {
        'application': localConstant.commonConstants.APP_NAME,
        'username': data.Username,        
        'passwordHash': data.Password
    };
    const requestPayload = new RequestPayload(params);
    dispatch(ShowLoader());
    const response = await PostData(resetPassword, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console            
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'warningToast resetNewPassword');
            dispatch(HideLoader());
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code === "1") {            
            if(!response.result) IntertekToaster(localConstant.login.SAME_PASSWORD_VALIDATION, 'warningToast');
            dispatch(HideLoader());
            return response.result;           
        } else if (response.code === "11" || response.code === "41") {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast');
            dispatch(HideLoader());
            return false;
        } else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, "resetNewPassword");
            dispatch(HideLoader());
            return false;
        }
    }
};
