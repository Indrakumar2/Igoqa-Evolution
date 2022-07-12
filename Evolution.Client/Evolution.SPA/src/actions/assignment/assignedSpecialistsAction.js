import { assignmentsActionTypes,taxonomyConstantUrl } from '../../constants/actionTypes';
import { contractAPIConfig,assignmentAPIConfig,RequestPayload, masterData, homeAPIConfig } from '../../apiConfig/apiConfig';
import { FetchData,PostData } from '../../services/api/baseApiService';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { getlocalizeData,parseValdiationMessage, isEmptyReturnDefault, isEmpty } from '../../utils/commonUtils';
import { StringFormat } from '../../utils/stringUtil';
import arrayUtil from '../../utils/arrayUtil';
import { ClearSubCategory,ClearServices,FetchTechSpecSubCategory,FetchTechSpecServices } from '../../common/masterData/masterDataActions';
import { required } from '../../utils/validator';
import { deleteNDTSubSupplierTS, deleteNDTMainSupplierTS } from './supplierInformationAction';

const localConstant = getlocalizeData();

const actions = {
    FetchChargeRates: (payload) => ({
        type: assignmentsActionTypes.FETCH_CHARGE_RATES,
        data: payload
    }),
    FetchPayRates: (payload) => ({
        type: assignmentsActionTypes.FETCH_PAY_RATES,
        data: payload
    }),
    FetchPaySchedule: (payload) => ({
        type: assignmentsActionTypes.FETCH_PAY_SCHEDULE,
        data: payload
    }),
    AddTechSpecSchedules: (payload) => ({
        type: assignmentsActionTypes.ADD_TECHSPEC_SCHEDULES,
        data: payload
    }),
    UpdateTechSpecSchedules: (payload) => ({
        type: assignmentsActionTypes.UPDATE_TECHSPEC_SCHEDULES,
        data: payload
    }),
    DeleteTechSpecSchedules: (payload) => ({
        type: assignmentsActionTypes.DELETE_TECHSPEC_SCHEDULES,
        data: payload
    }),
    AssignTechnicalSpecialist: (payload) => ({
        type: assignmentsActionTypes.ASSIGN_TECHNICAL_SPECIALIST,
        data: payload
    }),
    UpdateAssignedTechSpec: (payload) => ({
        type: assignmentsActionTypes.UPDATE_ASSIGNED_TECHSPEC,
        data: payload
    }),
    DeleteAssignedTechSpec: (payload) => ({
        type: assignmentsActionTypes.DELETE_ASSIGNED_TECHSPEC,
        data: payload
    }),
    UpdateTaxomony : (payload) => ({
        type: assignmentsActionTypes.UPDATE_TAXONOMY,
        data: payload
    }),
    ClearChargeAndPayRates: (payload) => ({
        type: assignmentsActionTypes.CLEAR_CHARGE_AND_PAY_RATES,
        data: payload
    }),
    ARSSearchPanelStatus: (payload) => ({
        type: assignmentsActionTypes.ARS_SEARCH_PANEL_STATUS,
        data: payload
    }),
    AssignResourcesButtonClick: (payload) => ({
        type: assignmentsActionTypes.ASSIGN_RESOURCES_BUTTON_CLICK,
        data: payload
    }),
    FetchTaxonomyBusinessUnit: (payload) => ({
        type: assignmentsActionTypes.FETCH_TAXONOMY_BUSINESS_UNIT,
        data: payload
    }),
    FetchNDTTaxonomySubCategory: (payload) => ({
        type: assignmentsActionTypes.FETCH_NDT_TAXONOMY_SUB_CATEGORY,
        data: payload
    }),
    FetchNDTTaxonomyService: (payload) => ({
        type: assignmentsActionTypes.FETCH_NDT_TAXONOMY_SERVICE,
        data: payload
    }),
    FetchTaxonomyURL:(payload)=>{
        return{
            type: assignmentsActionTypes.FETCH_TAXONOMY_URL,
            data: payload
         };
    },
};
/**
 * Fetch Taxonomy URL action
 * @param {*object} data 
 */
export const FetchTaxonomyURL = (data) => async (dispatch, getstate) => {
    const state = getstate();
    if (isEmptyReturnDefault(state.rootAssignmentReducer.taxonomyConstantUrl)) {
        const taxonomyUrl = homeAPIConfig.documentation;
        const isActive=true;
        const params = {
            isActive:isActive //Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
        };
        const requestPayload = new RequestPayload(params);

        const response = await FetchData(taxonomyUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast FetchTaxomyUrlError');
            });
        if (response && response.code === "1") {
            dispatch(actions.FetchTaxonomyURL((response.result && response.result.length > 0) ? response.result[0] : {}));
        }
        else if(response && response.code && (response.code === "11" || response.code === "41")){
            IntertekToaster(parseValdiationMessage(response), 'warningToast dadshboarddocedetails');
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast FetchTaxomyUrlError');
        }
    }
};

/**
 * Fetch Charge Rates action
 * @param {*object} data 
 */
export const FetchChargeRates = (data) => async(dispatch,getstate) => {
    let param = {};
    let contractNumber = "";
    if(data){
        const state = getstate();
        contractNumber = state.rootAssignmentReducer.assignmentDetail.AssignmentInfo && state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentContractNumber;
        const url = assignmentAPIConfig.assignmentContractScheduleRates; // D-714
        if(contractNumber)
            param = { 
                        'ContractNumber':contractNumber,
                        'ScheduleName':data.contractScheduleName,
                        'isActive': true 
                    }; // Changed isActive to True which means Hidden false
        const requestPayload = new RequestPayload(param);
        const response = await FetchData(url, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast fetchChargeRateError');
            });
        if (response && response.code) {
            if (response.code === "1") {
                dispatch(actions.FetchChargeRates(response.result));
            }
            else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast fetchChargeRateError');
            }
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast fetchChargeRateError');
        }
    }
};

/**
 * Fetch Pay Schedule action
 * @param {*String} data 
 */
export const FetchPaySchedule = (data) => async(dispatch,getstate) => {
    const param = {};
    if(data){
        const url = StringFormat(assignmentAPIConfig.assignmentPaySchedule,data);
        const requestPayload = new RequestPayload(param);
        const response = await FetchData(url, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast fetchPayScheduleError');
            });
        if (response && response.code) {
            if (response.code === "1") {
                dispatch(actions.FetchPaySchedule(response.result));
            }
            else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast fetchPayScheduleError');
            }
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast fetchPayScheduleError');
        }
    }
};

/**
 * Fetch Pay Rates action
 * @param {*Object} data 
 */
export const FetchPayRates = (data) => async(dispatch,getstate) => {
    const state = getstate();
    const assignmentInfo = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo,'object');
    const firstVisitDate = assignmentInfo.visitFromDate || assignmentInfo.timesheetFromDate || assignmentInfo.assignmentFirstVisitDate;
    const param = {};
    if(data){
        const url = StringFormat(assignmentAPIConfig.assignmentPayRates,data.technicalSpecialistEpin,data.technicalSpecialistPayScheduleId);
        param.isActive = true; // Temp Fix: Add isActive = false filter (as this field holds isHidden values).
        const requestPayload = new RequestPayload(param);
        const response = await FetchData(url, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast fetchPayRateError');
            });
        if (response && response.code) {
            if (response.code === "1") {
                // const filteredPayRate = (isEmpty(response.result) || isEmpty(firstVisitDate)) ? response.result 
                //                         : response.result.filter(x => (isEmpty(x.effectiveTo) || x.effectiveTo >= firstVisitDate));
                //                         dispatch(actions.FetchPayRates(filteredPayRate));
                dispatch(actions.FetchPayRates(response.result)); // Commented as per the discussion with IGO QA.
            }
            else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast fetchPayRateError');
            }
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast fetchPayRateError');
        }
        return response;
    }
};

/**
 * Add Technical Specialist Schedules action
 * @param {*Object} data 
 */
export const AddTechSpecSchedules = (data) => (dispatch,getstate) => {
    const state = getstate();
    if(data){
        const techSpec = Object.assign([],state.rootAssignmentReducer.assignmentDetail.AssignmentTechnicalSpecialists);
        const techSpecIndex = techSpec.findIndex(iteratedValue => {
            if (iteratedValue.recordStatus === 'N') {
                return iteratedValue["assignmentTechnicalSplUniqueId"] === data["assignmentTechnicalSplUniqueId"];
            }else{
                return iteratedValue.assignmentTechnicalSplId === data.assignmentTechnicalSpecilaistId;
            }
        });
        if(techSpecIndex >= 0){
            const techSpecSchedule = Object.assign([],techSpec[techSpecIndex].assignmentTechnicalSpecialistSchedules);
            techSpecSchedule.map(x=> x.isSelected = false);
            techSpecSchedule.unshift(data);
            techSpec[techSpecIndex].assignmentTechnicalSpecialistSchedules = techSpecSchedule;
            techSpec[techSpecIndex].scheduleValidation=""; //D634 issue 2
            if(techSpec[techSpecIndex].recordStatus !== 'N'){
                techSpec[techSpecIndex].recordStatus = 'M';
            }
            dispatch(actions.AddTechSpecSchedules(techSpec));
        }
    }
};

/**
 * Update Technical Specialist Schedules action
 * @param {*Object} data 
 */
export const UpdateTechSpecSchedules = (data) => (dispatch,getstate) => {
    const state = getstate();
    if(data){
        const techSpec = Object.assign([],state.rootAssignmentReducer.assignmentDetail.AssignmentTechnicalSpecialists);
        const techSpecIndex = techSpec.findIndex(iteratedValue => {
            if (iteratedValue.recordStatus === 'N') {
                return iteratedValue["assignmentTechnicalSplUniqueId"] === data["assignmentTechnicalSplUniqueId"];
            }else{
                return iteratedValue.assignmentTechnicalSplId === data.assignmentTechnicalSpecilaistId;
            }
        });
        if(techSpecIndex >= 0){
            const techSpecSchedule = Object.assign([],techSpec[techSpecIndex].assignmentTechnicalSpecialistSchedules);
            const index = techSpecSchedule.findIndex(iteratedValue => {
                let checkProperty = "assignmentTechnicalSpecialistScheduleId";
            if (iteratedValue.recordStatus === 'N') {
                checkProperty = "assignmentTechnicalSpecialistScheduleUniqueId";
            }
            return iteratedValue[checkProperty] === data[checkProperty];
            });
            if(index >= 0){
                techSpecSchedule[index] = data;
                techSpec[techSpecIndex].assignmentTechnicalSpecialistSchedules = techSpecSchedule;
                if(techSpec[techSpecIndex].recordStatus !== 'N'){
                    techSpec[techSpecIndex].recordStatus = 'M';
                }
                dispatch(actions.UpdateTechSpecSchedules(techSpec));
            }
        }
    }
};

/**
 * Delete Technical Specialist Schedules action
 * @param {*Array} data
 */
export const DeleteTechSpecSchedules = (data) => (dispatch, getstate) => {
    const state = getstate();
    if (data && data.length > 0) {
        const techSpec = Object.assign([], state.rootAssignmentReducer.assignmentDetail.AssignmentTechnicalSpecialists);
        const techSpecIndex = techSpec.findIndex(iteratedValue => {
            if (iteratedValue.recordStatus === 'N') {
                return iteratedValue["assignmentTechnicalSplUniqueId"] === data[0].assignmentTechnicalSplUniqueId;
            }else{
                return iteratedValue.assignmentTechnicalSplId === data[0].assignmentTechnicalSpecilaistId;
            }
        });
        if (techSpecIndex >= 0) {
            const techSpecSchedules = Object.assign([], techSpec[techSpecIndex].assignmentTechnicalSpecialistSchedules);
            data.forEach(row => {
                techSpecSchedules.forEach((iteratedValue, index) => {
                    const uniqueScheduleIdProp = row.recordStatus === 'N' ? "assignmentTechnicalSpecialistScheduleUniqueId" : "assignmentTechnicalSpecialistScheduleId";
                    if (iteratedValue[uniqueScheduleIdProp]
                        === row[uniqueScheduleIdProp]) {
                        if (iteratedValue.recordStatus !== "N") {
                            techSpecSchedules[index].recordStatus = "D";
                        }
                        else {
                            techSpecSchedules.splice(index, 1);
                        }
                    }
                });
            });
            techSpec[techSpecIndex].assignmentTechnicalSpecialistSchedules = techSpecSchedules;
            techSpec[techSpecIndex].recordStatus = techSpec[techSpecIndex].recordStatus !== 'N' ? 'M' : techSpec[techSpecIndex].recordStatus;
            dispatch(actions.DeleteTechSpecSchedules(techSpec));
        }
    }
};

/**
 * Assign Technical Specialist action
 * @param {*Array} data 
 */
export const AssignTechnicalSpecialist = (data) => (dispatch,getstate) => {
    dispatch(actions.AssignTechnicalSpecialist(data));
};

/**
 * Update Assigned Technical Specialist action
 * @param {*Object} data 
 */
export const UpdateAssignedTechSpec = (data) => (dispatch,getstate) => {
    const state = getstate();
    if(data){
        let checkProperty ="assignmentTechnicalSplId";
        if(data.recordStatus === 'N'){
            checkProperty = "assignmentTechnicalSplUniqueId";
        } 
        const assignedTechSpec = Object.assign([],state.rootAssignmentReducer.assignmentDetail.AssignmentTechnicalSpecialists);
        const index = assignedTechSpec.findIndex(iteratedValue => iteratedValue[checkProperty] == data[checkProperty]);
        if(index >= 0){
            assignedTechSpec[index] = data;
            dispatch(actions.UpdateAssignedTechSpec(assignedTechSpec));
        }
    }
};

/**
 * Delete Assigned Technical Specialist action
 * @param {*Array} data 
 */
export const DeleteAssignedTechSpec = (data) => (dispatch,getstate) => {
    const state = getstate();
    if (data) {
        const assignedTechSpec = Object.assign([], state.rootAssignmentReducer.assignmentDetail.AssignmentTechnicalSpecialists);
        data.forEach(row => {
            assignedTechSpec.forEach((iteratedValue, index) => {
                if (iteratedValue.assignmentTechnicalSplId === row.assignmentTechnicalSplId) {
                    if (iteratedValue.recordStatus !== "N") {
                        const techSpecSchedule = Object.assign([],assignedTechSpec[index].assignmentTechnicalSpecialistSchedules);
                        if(techSpecSchedule && techSpecSchedule.length > 0){
                            techSpecSchedule.forEach((element,i) => {
                                if(element.recordStatus !== "N"){
                                    techSpecSchedule[i].recordStatus = "D";
                                }
                                else{
                                    techSpecSchedule.splice(i,1);
                                }
                            });
                        }
                        assignedTechSpec[index].assignmentTechnicalSpecialistSchedules = techSpecSchedule;
                        assignedTechSpec[index].recordStatus = "D";
                    }
                    else {
                        const idx = assignedTechSpec.findIndex(value => (value.assignmentTechnicalSplUniqueId === row.assignmentTechnicalSplUniqueId));
                        if(idx >= 0){
                            assignedTechSpec.splice(idx, 1);
                        }
                    }
                }
            });
        });
        dispatch(actions.DeleteAssignedTechSpec(assignedTechSpec));
    }
};

/**
 * This action to remove dependency of NDT resources with supplier information and deletes the resources from Assignment
 * @param {* NDT Delete techspec list} techSpecList 
 */
export const DeleteNDTAssignedTechSpec = (techSpecList) => (dispatch,getstate) => {
    /**
     * Remove dependency from supplier information - both main and sub supplier.
     * Delete the resource from the Assignment.
     */
    const state = getstate();
    if(techSpecList){
        const assignmentSubSuppliers = Object.assign([],state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers);
        techSpecList.forEach(ts => {
            assignmentSubSuppliers.forEach(x => {
                if(x.recordStatus !== 'D'){
                    if(x.subSupplierId)
                        dispatch(deleteNDTSubSupplierTS(ts, x.subSupplierId));
                    else
                        dispatch(deleteNDTMainSupplierTS(ts));
                }
            });
            dispatch(DeleteAssignedTechSpec([ ts ]));
        });
    }
};

/**
 * Update Taxonomy action
 * @param {*Object} data 
 */
export const UpdateTaxomony = (data) => (dispatch, getstate) => {
    const state = getstate();
    const assignmentOldTaxonomy = isEmptyReturnDefault(state.rootAssignmentReducer.AssignmentTaxonomyold);
    const assignmentTaxonomy = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentTaxonomy);
    if (data) {
        const taxonomy = [];
        taxonomy.push(data);
        const obj = {
            taxonomy1: taxonomy,
            oldtaxonomy: isEmpty(assignmentOldTaxonomy) ? assignmentTaxonomy : assignmentOldTaxonomy
        };
        dispatch(actions.UpdateTaxomony(obj));
    }
};

/** Clear Charge Rates and Pay Rates */
export const ClearChargeAndPayRates = () => (dispatch,getstate) => {
    dispatch(actions.ClearChargeAndPayRates());
};
/** ARS Search Panel Open Mode Flag */
export const ARSSearchPanelStatus = (data) => (dispatch,getstate) => {
    dispatch(actions.ARSSearchPanelStatus(data));
};

/** Assign Resources Button Click boolean toggle funciton */
export const AssignResourcesButtonClick = (data) => (dispatch,getstate) => {
    dispatch(actions.AssignResourcesButtonClick(data));
};  

/** Fetch category base on business unit */
export const FetchTaxonomyBusinessUnit = (data) =>async (dispatch,getstate) => {
    if(data){
        const url = assignmentAPIConfig.taxonomyBusinessUnit;
        const requestPayload = new RequestPayload(data);
        const response = await FetchData(url, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast fetchtaxonomyBusinessUnitError');
            });
        if (response && response.code) {
            if (response.code === "1") {
                const result = isEmpty(response.result) ? response.result : arrayUtil.sort(response.result,'category','asc');
                dispatch(actions.FetchTaxonomyBusinessUnit(result));
            }
            else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast fetchtaxonomyBusinessUnitError');
            }
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast fetchtaxonomyBusinessUnitError');
        }
        return response;
    }
    else{
        dispatch(actions.FetchTaxonomyBusinessUnit([]));

    }
};

/** Fetch sub category for NDT */
export const FetchNDTTaxonomySubCategory = (data) =>async (dispatch,getstate) => {
    if(data) {
        const Url = masterData.baseUrl + masterData.taxonomySubCategory;
        const params = { 'taxonomyCategory':data };
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(Url, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast TechSpecSubCategory');
            });
            
        if (response && response.code) {
            if (response.code === "1") {
                const result = isEmpty(response.result) ? response.result : arrayUtil.sort(response.result, 'taxonomySubCategoryName', 'asc');
                dispatch(actions.FetchNDTTaxonomySubCategory(result));
            }
            else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast fetchtaxonomyndtsubcategoryError');
            }
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast fetchtaxonomyndtsubcategoryError');
        }
    }
    else{
        dispatch(actions.FetchNDTTaxonomySubCategory([]));
    }
};

/** Fetch services for NDT */
export const FetchNDTTaxonomyService = (data) =>async (dispatch,getstate) => {
    if(data) {
        const Url = masterData.baseUrl + masterData.taxonomyServices;
        const params = { 'taxonomySubCategory':data };
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(Url, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast TechSpecSubCategory');
            });
            
        if (response && response.code) {
            if (response.code === "1") {
                const result = isEmpty(response.result) ? response.result : arrayUtil.sort(response.result, 'taxonomyServiceName', 'asc');
                dispatch(actions.FetchNDTTaxonomyService(result));
            }
            else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast fetchtaxonomyndtserviceError');
            }
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast fetchtaxonomyndtserviceError');
        }
    }
    else{
        dispatch(actions.FetchNDTTaxonomyService([]));
    }
};

/** clear NDT sub category and services */
export const clearNDTSubCategory = () => (dispatch) => {
    dispatch(FetchNDTTaxonomySubCategory());
    dispatch(FetchNDTTaxonomyService());
};

/** Assigned Specialist Taxonomy LOV Handling 
 *  Param: Assignment Taxonomy
 */
export const handleAssignedSpecialistTaxonomyLOV = (assignmentTaxonomy) => (dispatch) => {
    if(assignmentTaxonomy){
        if(required(assignmentTaxonomy.taxonomyCategory)){
            dispatch(ClearSubCategory());
        }
        if(!required(assignmentTaxonomy.taxonomyCategory)){  
            dispatch(ClearSubCategory());
            dispatch(FetchTechSpecSubCategory(assignmentTaxonomy.taxonomyCategory));
        }
        if(required(assignmentTaxonomy.taxonomySubCategory)){
            dispatch(ClearServices());
        }
        if(!required(assignmentTaxonomy.taxonomyCategory) && !required(assignmentTaxonomy.taxonomySubCategory)){
            dispatch(ClearServices());
            dispatch(FetchTechSpecServices(assignmentTaxonomy.taxonomyCategory,assignmentTaxonomy.taxonomySubCategory));//def 916 fix
        } 
    }
};

// Clear assigned resources from the Assignment - Also its dependencies.
export const ClearAssignedSpecialist = () => (dispatch,getstate) => {
    const state = getstate();
    const assignedSpecialist = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentTechnicalSpecialists);
    dispatch(DeleteNDTAssignedTechSpec(assignedSpecialist));
};

export const FetchTaxonomyBusinessUnitForSearch = () =>async (dispatch,getstate) => {
    const url = masterData.taxonomyCategory;
    const requestPayload = new RequestPayload();
    const response = await FetchData(url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast fetchtaxonomyBusinessUnitError');
        });
    if (response && response.code) {
        if (response.code === "1") {
            const result = isEmpty(response.result) ? response.result : arrayUtil.sort(response.result,'name','asc');
            dispatch(actions.FetchTaxonomyBusinessUnit(result));
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast fetchtaxonomyBusinessUnitError');
        }
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast fetchtaxonomyBusinessUnitError');
    }
    return response;
};