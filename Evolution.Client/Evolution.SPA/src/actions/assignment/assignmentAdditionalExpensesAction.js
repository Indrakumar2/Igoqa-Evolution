import { assignmentsActionTypes } from '../../constants/actionTypes';
import { 
    getlocalizeData,
    isEmptyOrUndefine,
    parseValdiationMessage,
 } from '../../utils/commonUtils';
 import { 
    assignmentAPIConfig,
    RequestPayload } from '../../apiConfig/apiConfig';
import { StringFormat } from '../../utils/stringUtil';
import {
    ShowLoader,
    HideLoader,
} from '../../common/commonAction';
import { ValidationAlert } from '../../components/viewComponents/customer/alertAction';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData } from '../../services/api/baseApiService';

const localConstant = getlocalizeData();
const actions={
   AddAdditionalExpenses:(payload)=>({
        type: assignmentsActionTypes.ADD_ADDITIONAL_EXPENSES,
        data:payload
    }),
    UpdateAdditionalExpenses:(payload)=>({
        type: assignmentsActionTypes.UPDATE_ADDITIONAL_EXPENSES,
        data:payload
    }),
    DeleteAdditionalExpenses:(payload)=>({
        type: assignmentsActionTypes.DELETE_ADDITIONAL_EXPENSES,
        data:payload
    }),
    FetchAssignmentAdditionalExpenses:(payload)=>({
        type: assignmentsActionTypes.FETCH_ASSIGNMENT_ADDITIONAL_EXPENSES,
        data:payload
    }),
    ClearAdditionalExpenses:(payload)=>({
        type: assignmentsActionTypes.CLEAR_ADDITIONAL_EXPENSES,
        data:payload
    }),
};

/**
 * Add new row of additional expenses data
 * @param {*Object} data 
 */
export const AddAdditionalExpenses= (data) => (dispatch,getstate) => {
    dispatch(actions.AddAdditionalExpenses(data));
};

/**
 * Update the edited record to additional expenses
 * @param {*Object} data 
 */
export const UpdateAdditionalExpenses = (data) => (dispatch, getstate) => {
    let index = -1;
    const state = getstate();
    if (data && state.rootAssignmentReducer.assignmentDetail.AssignmentAdditionalExpenses) {
        const additionalExpenses = Object.assign([], state.rootAssignmentReducer.assignmentDetail.AssignmentAdditionalExpenses);
        let checkProperty = "assignmentAdditionalExpenseId";
        if (data.recordStatus === 'N') {
            checkProperty = "assignmentAdditionalExpenseUniqueId";
        }
        index = state.rootAssignmentReducer.assignmentDetail.AssignmentAdditionalExpenses
        .findIndex(iteratedValue => iteratedValue[checkProperty] == data[checkProperty]);
        if (index >= 0) {
            additionalExpenses[index] = data;
            dispatch(actions.UpdateAdditionalExpenses(additionalExpenses));
        }
    }
};

/**
 * Delete selected records from additional expenses
 * @param {*Object} data 
 */
export const DeleteAdditionalExpenses = (data) => (dispatch,getstate) => {
    const state = getstate();
    if(data)
    {
        const additionalExpenses = Object.assign([],state.rootAssignmentReducer.assignmentDetail.AssignmentAdditionalExpenses);
        data.map(row=>{
            additionalExpenses.map((iteratedValue, index) => {
                if (iteratedValue.assignmentAdditionalExpenseId === row.assignmentAdditionalExpenseId) {
                    if (iteratedValue.recordStatus !== "N") {
                        additionalExpenses[index].recordStatus = "D";
                    }
                    else {
                        const idx = additionalExpenses.findIndex(value => (value.assignmentAdditionalExpenseUniqueId === row.assignmentAdditionalExpenseUniqueId));
                        if(idx >= 0){
                            additionalExpenses.splice(idx, 1);
                        }
                    }
                }
            });
        });
        dispatch(actions.DeleteAdditionalExpenses(additionalExpenses));
    }
};

export const FetchAssignmentAdditionalExpenses = (assignmentId) => async(dispatch, getstate) => {
    if(!assignmentId){
        return false;
    }
    const url  = StringFormat(assignmentAPIConfig.additionalExpenses, assignmentId);
    const param = {};
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
    if (!isEmptyOrUndefine(response)) {
        if (response.code === "1") {
            dispatch(actions.FetchAssignmentAdditionalExpenses(response.result));
            return response.result;
        }
        else if (response.code && (response.code === "11" || response.code === "41")) {
            IntertekToaster(parseValdiationMessage(response), 'warningToast conActDataSomethingWrong');
            return false;
        }
        else {
            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "Timesheet"));
            return false;
        }
    }
};

// Action Description : To clear Assignemnt Additonal Expenses
export const ClearAdditionalExpenses = () => (dispatch) => {
    dispatch(actions.ClearAdditionalExpenses([]));
};