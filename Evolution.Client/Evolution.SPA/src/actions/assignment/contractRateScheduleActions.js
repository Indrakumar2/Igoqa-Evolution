import { assignmentsActionTypes } from '../../constants/actionTypes';
import { FetchData } from '../../services/api/baseApiService';
import { contractAPIConfig,RequestPayload } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { getlocalizeData,isEmptyReturnDefault,isEmpty } from '../../utils/commonUtils';
import moment from 'moment';
const localConstant = getlocalizeData();

const actions = {
    AddNewRateSchedule: (payload) => ({
        type: assignmentsActionTypes.ADD_NEW_RATE_SCHEDULE,
        data: payload
    }),
    DeleteRateSchedule: (payload) => ({
        type: assignmentsActionTypes.DELETE_RATE_SCHEDULE,
        data: payload
    }),
    UpdatetRateSchedule:(payload) => ({
        type: assignmentsActionTypes.UPDATE_RATE_SCHEDULE,
        data: payload
    }),
    FetchContractScheduleName:(payload) => ({
        type: assignmentsActionTypes.FETCH_SCHEDULE_NAME,
        data: payload
    }),
    //Added for Contract Rates Expired Validation -Start
    FetchContractRates:(payload) => ({
        type: assignmentsActionTypes.FETCH_CONTRACT_RATES,
        data: payload
    }),
    //Added for Contract Rates Expired Validation -End
    ClearAssignmentRateSchedule:(payload) => ({
        type: assignmentsActionTypes.CLEAR_ASSIGNMENT_RATE_SCHEDULE,
        data: payload
    }),
};

//Add new Rate Schedule
export const AddNewRateSchedule = (data) => (dispatch, getstate) => {
    dispatch(actions.AddNewRateSchedule(data));
};

//Delete Assignments Classification
export const DeleteRateSchedule = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootAssignmentReducer.assignmentDetail.AssignmentContractSchedules);
    data.forEach(row => {
        newState.forEach((iteratedValue,index) => {
            if (iteratedValue.assignmentContractRateScheduleId === row.assignmentContractRateScheduleId) {
                if (row.recordStatus !== "N") {
                    // const index = newState.findIndex(value => (value.assignmentContractRateScheduleId === row.assignmentContractRateScheduleId));
                    newState[index].recordStatus = "D";
                }
                else {
                    const idx = newState.findIndex(value => (value.assignmentContractRateAddUniqueId === row.assignmentContractRateAddUniqueId));
                    if(idx >= 0){
                        newState.splice(idx, 1);
                    }
                }
            }
        });
    });
    dispatch(actions.DeleteRateSchedule(newState));
};

export const UpdatetRateSchedule = (updatedData, editedRowData) => (dispatch, getstate) => {
    const state = getstate();
    const editedRow = Object.assign({}, editedRowData, updatedData);
    const schedules = state.rootAssignmentReducer.assignmentDetail.AssignmentContractSchedules;

    let checkProperty ="assignmentContractRateScheduleId";
    if(editedRow.recordStatus === 'N'){
        checkProperty = "assignmentContractRateAddUniqueId";
    } 
    const index = schedules.findIndex(iteratedValue => iteratedValue[checkProperty] === editedRow[checkProperty]);
    const newState = Object.assign([], schedules);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdatetRateSchedule(newState));
    }
};

export const FetchContractScheduleName = () =>async (dispatch, getstate) => {
    const state = getstate();
    const ContractNumber = state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentContractNumber;
    const params = {
        ContractNumber:ContractNumber
    };
    const requestPayload = new RequestPayload(params);

    const response = await FetchData(contractAPIConfig.contracts+contractAPIConfig.rateSchedule, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console      
            // IntertekToaster(localConstant.assignments.ERROR_FETCHING_REFERENCE_TYPES,'dangerToast fetchAssignmentWrong');        
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
        if (response && response.code === "1") {
            const contractRates=await dispatch(FetchContractRates());
            if(contractRates && contractRates.code === "1" && contractRates.result)
                dispatch(RateScheduleExpiryCheck(response.result,contractRates.result));
            // dispatch(actions.FetchContractScheduleName(response.result));
        }
};
//Added for Contract Rates Expired Validation -Starts
export const RateScheduleExpiryCheck = (contractSchedules,contractRates) =>async (dispatch, getstate) =>{
    const state = getstate();
    const currentPage=state.CommonReducer.currentPage;
    const assignmentInfo = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo, 'object');
    if(contractSchedules && contractRates && assignmentInfo){
        contractSchedules.forEach(schedule => {
            const assignmentCreatedDate = (currentPage === "addAssignment") ? (assignmentInfo.visitFromDate || assignmentInfo.timesheetFromDate)
                : assignmentInfo.assignmentCreatedDate;
            const filteredData = contractRates.filter(
                x => x.scheduleId === schedule.scheduleId && (isEmpty(x.effectiveTo) ||
                    isEmpty(assignmentCreatedDate) || 
                moment(x.effectiveTo).isAfter(assignmentCreatedDate,'day') ||
                moment(x.effectiveTo).isSame(assignmentCreatedDate,'day'))
            );
            filteredData.length === 0 ? schedule["expired"] = true : schedule["expired"] = false;
        });
    }
    dispatch(actions.FetchContractScheduleName(contractSchedules));
};
export const FetchContractRates = () =>async (dispatch, getstate) => {
    const state = getstate();
    const ContractNumber = state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentContractNumber;
    const requestPayload = new RequestPayload();
    const response = await FetchData(contractAPIConfig.contractRates + ContractNumber,requestPayload)
        .catch(error => {  
            // console.error(error); // To show the error details in console          
            // IntertekToaster(localConstant.assignments.ERROR_FETCHING_CONTRACT_RATES,'dangerToast fetchAssignmentWrong');        
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
        if (response && response.code === "1") {
            dispatch(actions.FetchContractRates(response.result));
        }
        return response;
};
//Added for Contract Rates Expired Validation -End

// Assignment Contract Rate Schedule clear action
export const ClearAssignmentRateSchedule = () => (dispatch) => {
    dispatch(actions.ClearAssignmentRateSchedule([]));
};