import { assignmentsActionTypes } from '../../constants/actionTypes';
import { getlocalizeData } from '../../utils/commonUtils';

const localConstant = getlocalizeData();
const actions = {
    AddRevenueData: (payload) => ({
        type: assignmentsActionTypes.ADD_REVENUE_DATA,
        data: payload
    }),
    UpdateRevenueData: (payload) => ({
        type: assignmentsActionTypes.UPDATE_REVENUE_DATA,
        data: payload
    }),
    DeleteRevenueData: (payload) => ({
        type: assignmentsActionTypes.DELETE_REVENUE_DATA,
        data: payload
    }),
    AddCostData: (payload) => ({
        type: assignmentsActionTypes.ADD_COST_DATA,
        data: payload
    }),
    UpdateCostData: (payload) => ({
        type: assignmentsActionTypes.UPDATE_COST_DATA,
        data: payload
    }),
    DeleteCostData: (payload) => ({
        type: assignmentsActionTypes.DELETE_COST_DATA,
        data: payload
    }),
    AddDefaultContributionData:(payload)=>({
        type: assignmentsActionTypes.ADD_DEFAULT_CONTRIBUTION_DATA,
        data: payload
    }),
    UpdateContributionCalculator:(payload)=>({
        type: assignmentsActionTypes.UPDATE_CONTRIBUTION_CALCULATOR,
        data: payload
    }),
    ResetContributionCalculator:(payload)=>({
        type: assignmentsActionTypes.RESET_CONTRIBUTION_CALCULATOR,
        data: payload
    }),
    savedContributionCalculatorChanges:()=>({
        type:assignmentsActionTypes.SAVED_CONTRIBUTION_CALCULATOR_CHANGES
    })
};

/**
 * Add Revenue Data
 */
export const AddRevenueData = (data) => (dispatch) => {
    dispatch(actions.AddRevenueData(data));
};

/**
 * Update Revenue Data
 */
export const UpdateRevenueData = (editedItem) => (dispatch, getstate) => {
    // const editedItem = Object.assign({}, editedRowData, data);
    let checkProperty ="assignmentContributionRevenueCostId";
    if(editedItem.recordStatus === 'N'){
        checkProperty = "assignmentContributionRevenueCostUniqueId";
    } 
    dispatch(actions.UpdateRevenueData({ editedItem, checkProperty }));
};

/**
 * Delete Revenue Data
 */
export const DeleteRevenueData = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = [ ...state.rootAssignmentReducer.assignmentDetail.AssignmentContributionCalculators ];
    //get the reference of assignmentContributionRevenueCosts to contributions
    const contributions = newState[0].assignmentContributionRevenueCosts;
    data.forEach(row => {
        contributions.forEach((item, i) => {
            if (item.type === 'A' && 
            item.assignmentContributionRevenueCostId === row.assignmentContributionRevenueCostId) {
                if (row.recordStatus !== "N") {
                    contributions[i].recordStatus = "D";
                }  
                else {
                    const index = contributions.findIndex(value => (value.assignmentContributionRevenueCostUniqueId
                        === row.assignmentContributionRevenueCostUniqueId));
                    if (index >= 0)
                        contributions.splice(index, 1);
                }
            }
        });
    });

    dispatch(actions.DeleteRevenueData(newState));
};

/**
 * Add Cost Data
 */
export const AddCostData = (data) => (dispatch) => {
    dispatch(actions.AddCostData(data));
};

/**
 * Update Cost Data
 */
export const UpdateCostData = (editedItem) => (dispatch, getstate) => {
 let checkProperty ="assignmentContributionRevenueCostId";
 if(editedItem.recordStatus === 'N'){
     checkProperty = "assignmentContributionRevenueCostUniqueId";
 } 
 dispatch(actions.UpdateCostData({ editedItem, checkProperty }));
};

/**
 * Delete Cost Data
 */
export const DeleteCostData = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = [ ...state.rootAssignmentReducer.assignmentDetail.AssignmentContributionCalculators ];
    //get the reference of assignmentContributionRevenueCosts to contributions
    const contributions = newState[0].assignmentContributionRevenueCosts;
    data.forEach(row => {
        contributions.forEach((item, i) => {
            if (item.type ==='B' &&  item.assignmentContributionRevenueCostId === row.assignmentContributionRevenueCostId) {
                if (row.recordStatus !== "N") {
                   contributions[i].recordStatus = "D";
                }
                else {
                    const index = contributions.findIndex(value => (value.assignmentContributionRevenueCostUniqueId
                        === row.assignmentContributionRevenueCostUniqueId));
                   if (index >= 0)
                       contributions.splice(index, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteCostData(newState));
};

export const AddDefaultContributionData=(data)=>(dispatch)=>{
    dispatch(actions.AddDefaultContributionData(data));
};

export const UpdateContributionCalculator = (data)=>(dispatch)=>{
    dispatch(actions.UpdateContributionCalculator(data));
};

export const resetContributionCalculator = () => async (dispatch, getstate) => {
    const state = getstate();
    dispatch(actions.ResetContributionCalculator(state.rootAssignmentReducer.contributionCalculationSavedChanges));
};
export const savedContributionCalculatorChanges = () => async (dispatch) => {
    dispatch(actions.savedContributionCalculatorChanges());
};