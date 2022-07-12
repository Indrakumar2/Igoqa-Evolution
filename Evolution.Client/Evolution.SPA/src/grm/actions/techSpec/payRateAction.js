import { techSpecActionTypes } from '../../constants/actionTypes';
import { masterData } from '../../../apiConfig/apiConfig';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';
import { FetchData } from '../../../services/api/baseApiService';
import {  isEmptyReturnDefault,isEmpty } from '../../../utils/commonUtils';
import { convertObjectToArray, mergeobjects,getlocalizeData } from '../../../utils/commonUtils';
import arrayUtil from '../../../utils/arrayUtil';
const localConstant = getlocalizeData();
const actions = {
    UpdatePayRateDetails: (payload) => (
        {
            type: techSpecActionTypes.payRateActionTypes.FETCH_PAY,
            data: payload
        }
    ),
    AddPayRateSchedule: (payload) => (
        {
            type: techSpecActionTypes.payRateActionTypes.ADD_PAYRATE_SCHEDULE,
            data: payload
        }
    ),
    
    UpdatePayRateSchedule: (payload) => (
        {
            type: techSpecActionTypes.payRateActionTypes.UPDATE_PAYRATE_SCHEDULE,
            data: payload
        }
    ),

    EditPayRateSchedule: (payload) => (
        {
            type: techSpecActionTypes.payRateActionTypes.EDIT_PAYRATE_SCHEDULE,
            data: payload
        }
    ),

    DeletePayRateSchedule: (payload) => (
        {
            type: techSpecActionTypes.payRateActionTypes.DELETE_PAYRATE_SCHEDULE,
            data: payload
        }
    ),
    
    //PayRate Detail Modal
   
    AddDetailPayRate: (payload) => (
        {
            type: techSpecActionTypes.payRateActionTypes.ADD_DETAIL_PAYRATE,
            data: payload
        }
    ),

    UpdateDetailPayRate: (payload) => (
        {
            type: techSpecActionTypes.payRateActionTypes.UPDATE_DETAIL_PAYRATE,
            data: payload
        }
    ),
    FetchPayRollNameByCompany: (payload) => (
        {
            type: techSpecActionTypes.payRateActionTypes.PAYROLLNAMEBYCOMPANY,
            data: payload
        }
    ),
    DeleteDetailPayRate: (payload) => (
        {
            type: techSpecActionTypes.payRateActionTypes.DELETE_DETAIL_PAYRATE,
            data: payload
        }
    ),
    ClearDetailPayRate: (payload) => (
        {
            type: techSpecActionTypes.payRateActionTypes.CLEAR_PAYRATE_EDITDETAILS,
            data: payload
        }
    ),
    UpdatePayRate: (payload) => ({
        type:techSpecActionTypes.payRateActionTypes.UPDATE_PAYRATE,
        data: payload
    }),
};
export const UpdatePayRate = (data) => (dispatch, getstate) => {
    const state = getstate();
    const modifiedData = mergeobjects(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo, data);
    dispatch(actions.UpdatePayRate(modifiedData));
};
export const FetchPayRollNameByCompany = (PayRollNamebyComapny) => async (dispatch) => {
if(!PayRollNamebyComapny){
    return false;
}
    const payrateUrl = masterData.baseUrl + masterData.companies+`/${ PayRollNamebyComapny }/`+ masterData.payrolls; 
    const response = await FetchData(payrateUrl, {})
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster( localConstant.techSpec.PayRate.PAYRATE_FAILED_MESSAGE, 'dangerToast ');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        const result = isEmpty(response.result) ? response.result : arrayUtil.sort(response.result,'payrollType','asc'); //D1437 PayRoll Sort Name issue
        dispatch(actions.FetchPayRollNameByCompany(result));
    }
    else {
        IntertekToaster( localConstant.techSpec.PayRate.PAYRATE_FAILED_MESSAGE, 'dangerToast ');
    }
};
export const AddPayRateSchedule = (data) => (dispatch, getstate) => {
   
    dispatch(actions.AddPayRateSchedule(data));
};
export const UpdatePayRateDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const modifiedData = mergeobjects(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo, data);
    dispatch(actions.UpdatePayRateDetails(modifiedData));
};

export const UpdatePayRateSchedule = (data) => (dispatch, getstate) => {  
    const state = getstate();
    const scheduleData =  convertObjectToArray(  state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistPaySchedule);
    const DetailPayData = convertObjectToArray( state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistPayRate);
    if (data.oldScheduleName !== data.payScheduleName) {
        scheduleData.map(iteratedValue => {
            if (iteratedValue.baseScheduleName === data.oldScheduleName) {
                iteratedValue.baseScheduleName = data.payScheduleName;
                if (iteratedValue.recordStatus !== 'N')
                    iteratedValue.recordStatus = 'M';
            }
        });
        DetailPayData.map(iteratedValue=>{
            if(iteratedValue.payScheduleName === data.oldScheduleName){
                iteratedValue.payScheduleName = data.payScheduleName;
                if(iteratedValue.recordStatus !== 'N')
                    iteratedValue.recordStatus = 'M';
            }
            if(iteratedValue.baseScheduleName === data.oldScheduleName){
                iteratedValue.baseScheduleName = data.payScheduleName;
                if(iteratedValue.recordStatus !== 'N')
                    iteratedValue.recordStatus = 'M';
            }
        });
        dispatch(actions.UpdatePayRateSchedule(scheduleData));
        dispatch(actions.UpdateDetailPayRate(DetailPayData));
    }
    const editedRow = mergeobjects( state.RootTechSpecReducer.TechSpecDetailReducer.rateScheduleEditData, data);
    const index = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistPaySchedule)
    .findIndex(iteratedValue => iteratedValue.id === editedRow.id);
    const newState = convertObjectToArray( state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistPaySchedule);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdatePayRateSchedule(newState));
    }
};
export const EditPayRateSchedule = (data) => (dispatch) => {
    dispatch(actions.EditPayRateSchedule(data));
};
export const DeletePayRateSchedule = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistPaySchedule);
    data.forEach(item => {
        newState.forEach((iteratedValue,index) =>{
            if (iteratedValue.payScheduleName === item.payScheduleName && iteratedValue.id === item.id) { //IGO_QC_895 
                if (iteratedValue.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    newState.splice(index, 1);
                }
            }
        });
    });

    dispatch(actions.DeletePayRateSchedule(newState));
};

//PayRate Detail Actions

export const AddDetailPayRate = (data) => (dispatch) => {
  
    dispatch(actions.AddDetailPayRate(data));
   
};

export const UpdateDetailPayRate = (updatedData,editedRowData) => (dispatch, getstate) => {
    const state = getstate();
    const editedRow = mergeobjects(  editedRowData, updatedData);
    const index = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistPayRate)
    .findIndex(iteratedValue => iteratedValue.id === editedRow.id);
    const newState = convertObjectToArray( state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistPayRate);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateDetailPayRate(newState));
    }
};

export const DeleteDetailPayRate = (data) => (dispatch, getstate) => {

    const state = getstate();
    const newState =convertObjectToArray( state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistPayRate);
    data.map(row => {
        newState.map(iteratedValue => {
            if (iteratedValue.id === row.id) {
                const index = newState.findIndex(value => (value.id === row.id));
                if (row.recordStatus !== "N") {
                    newState[index].recordStatus = "D";
                }
                else {
                    newState.splice(index, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteDetailPayRate(newState));
};
    
export const ClearDetailPayRate = (data) => (dispatch) => {
    dispatch(actions.ClearDetailPayRate(data));
};