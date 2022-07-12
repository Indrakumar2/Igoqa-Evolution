import { techSpecActionTypes } from '../../constants/actionTypes';
import { masterData,RequestPayload,GrmAPIConfig,assignmentAPIConfig } from '../../../apiConfig/apiConfig';
import { FetchData,PostData,CreateData } from '../../../services/api/baseApiService';
import IntertekToaster from  '../../../common/baseComponents/intertekToaster';
import { isEmpty,isEmptyReturnDefault, mergeobjects,getlocalizeData,isEmptyOrUndefine, deepCopy } from '../../../utils/commonUtils';
import { ShowLoader, HideLoader ,UpdateCurrentModule } from '../../../common/commonAction';
import { ClearSubCategory,ClearServices,FetchTechSpecSubCategory,FetchTechSpecServices } from '../../../common/masterData/masterDataActions';
import { required } from '../../../utils/validator';
import {  ValidationAlert } from '../../../components/viewComponents/customer/alertAction';
import { ClearData } from '../../../components/applicationComponents/customerAndCountrySearch/cutomerAndCountrySearchAction';
import {
    ChangeDataAvailableStatus
} from '../../../components/appLayout/appLayoutActions';

const localConstant = getlocalizeData();

const actions = {
    FetchPreAssignment: (payload) => ({
        type: techSpecActionTypes.FETCH_PRE_ASSIGNMENT,
        data: payload
    }),
    CheckPreassignmentWon: (payload) => ({
        type: techSpecActionTypes.CHECK_PREASSIGNMENT_WON,
        data: payload
    }),

    UpdatePreAssignmentSearchParam: (payload) => ({
        type: techSpecActionTypes.UPDATE_PRE_ASSIGNMENT_DETAILS,
        data: payload
    }),
    FetchContractHoldingCoordinator:(payload) => ({
        type: techSpecActionTypes.FETCH_CONTRACT_HOLDING_COORDINATOR,
        data: payload
    }),
    FetchOperatingCoordinator:(payload) => ({
        type: techSpecActionTypes.FETCH_OPERATING_COORDINATOR,
        data: payload
    }),
    ClearContractHoldingCoordinator:(payload) => ({
        type: techSpecActionTypes.CLEAR_CONTRACT_HOLDING_COORDINATOR,
        data: payload
    }),
    ClearOperatingCoordinator:(payload) => ({
        type: techSpecActionTypes.CLEAR_OPERATING_COORDINATOR,
        data: payload
    }),
    FetchAssignmentType:(payload) => ({
        type: techSpecActionTypes.FETCH_ASSIGNMENT_TYPE,
        data: payload
    }),
    AddSubSupplier:(payload) => ({
        type: techSpecActionTypes.ADD_SUB_SUPPLIER,
        data: payload
    }),
    UpdateSubSupplier:(payload) => ({
        type: techSpecActionTypes.UPDATE_SUB_SUPPLIER,
        data: payload
    }),
    DeleteSubSupplier:(payload) => ({
        type: techSpecActionTypes.DELETE_SUB_SUPPLIER,
        data: payload
    }),
    UpdateActionDetails:(payload) => ({
        type: techSpecActionTypes.UPDATE_ACTION_DETAILS,
        data: payload
    }),
    searchPreAssignmentTechSpec:(payload) =>({
        type:techSpecActionTypes.PRE_ASSIGNMENT_TECH_SPEC_SEARCH,
        data:payload
    }),
    isActiveGoogleMap:(payload) =>({
        type:techSpecActionTypes.SHOW_GOOGLE_MAP,
        data:payload
    }),
    UpdateAssignmentOnPreAssignment:(payload) => ({
        type: techSpecActionTypes.UPDATE_ASSIGNMENT_ON_PRE_ASSIGNMENT,
        data: payload
    }),
    FetchDispositionType:(payload)=>({
        type: techSpecActionTypes.FETCH_DISPOSITION_TYPE,
        data:payload
    }),
    AddOptionalSearch:(payload)=>({
        type: techSpecActionTypes.ADD_OPTIONAL_SEARCH,
        data:payload
    }),
    TechSpechUnSavedData:(payload)=>({
        type: techSpecActionTypes.UNSAVED_BTN_HANDLER,
        data:payload
    }),
};

/** Fetch Pre-Assignment Action */
export const FetchPreAssignment = (data) => async (dispatch,getstate) => { 
    dispatch(ShowLoader());
    if(data){
        const apiUrl = GrmAPIConfig.preAssignmentSave;
        const param = {
            "id" : data.id
        };
        const requestPayload = new RequestPayload(param);
        const response = await FetchData(apiUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.errorMessages.FETCH_PRE_ASSIGNMENT_FAILED, 'dangerToast preAssignmentErr');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response) {
            //To-do Search Result Grid Bind
            if (response.code === "1") {
                if(response.result && response.result.length > 0){

                    //646 Fixes - If search action is LOST, We shouldn't show the page to user
                    if (response.result[0].searchAction === 'L') {
                        dispatch(HideLoader());
                        dispatch(ChangeDataAvailableStatus(true));
                        return response;
                    }

                    response.result[0].searchParameter = JSON.parse(response.result[0].searchParameter);
                    const preAssignmentSearchData = response.result[0].searchParameter;
                    dispatch(LoadPreAssignmentMaster(preAssignmentSearchData));
                    // ITK D-625
                    const preAssignmentData = deepCopy(response.result[0]);
                    dispatch(searchPreAssignmentTechSpec(preAssignmentData));
                    if(response.result[0].searchAction==="W")
                    {
                      dispatch(actions.CheckPreassignmentWon(true));
                    }
                    else{
                        dispatch(actions.CheckPreassignmentWon(false));
                        response.result[0].searchAction = "";
                    }  
                    dispatch(actions.FetchPreAssignment(response.result[0]));
                    dispatch(UpdateCurrentModule(localConstant.moduleName.TECHSPECIALIST));//D576
                }
                else{
                    dispatch(actions.FetchPreAssignment({}));
                    IntertekToaster(localConstant.errorMessages.FETCH_PRE_ASSIGNMENT_FAILED, 'dangerToast preAssignmentErr');
                }
            }
            else if (response.code === "41") {
                if (!isEmptyReturnDefault(response.validationMessages)) {
                    //To-Do: Common approach to display validationMessages
                }
            }
            else if (response.code === "11") {
                if (!isEmptyReturnDefault(response.messages)) {
                    //To-Do: Common approach to display messages
                }
            }
            else {

            }
            dispatch(HideLoader());
        }
        else{
            dispatch(HideLoader());
        }
    }
    else{
        dispatch(HideLoader());
    } 
};

/** Update Pre-Assignment Details Action */
export const UpdatePreAssignmentSearchParam = (data) => (dispatch,getstate) => {
    const state = getstate();
    if(data){
        const searchParameter =  Object.assign({},state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails.searchParameter,data);
        dispatch(actions.UpdatePreAssignmentSearchParam(searchParameter));
    }
};

export const ClearOptionalSearch = (searchAction, data) => (dispatch, getstate) => { //Changes for ITK D1465
    const state = getstate();
    const searchParameter =  Object.assign({},state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails.searchParameter,data);
    if(searchAction === "OPR" || searchAction === "ARR"){
        searchParameter.optionalSearch.equipmentMaterialDescription = null;
        searchParameter.optionalSearch.customerApproval = null;
        searchParameter.optionalSearch.certification = null;
        searchParameter.optionalSearch.certificationExpiryFrom = null;
        searchParameter.optionalSearch.certificationExpiryTo = null;
        searchParameter.optionalSearch.languageSpeaking = null;
        searchParameter.optionalSearch.languageWriting = null;
        searchParameter.optionalSearch.languageComprehension = null;
        searchParameter.optionalSearch.radius = null;
        searchParameter.optionalSearch.searchInProfile = null;
    }
    dispatch(actions.UpdatePreAssignmentSearchParam(searchParameter));
};

/** Fetch Coordinator for Contract Holding Company action - pre assignment */
export const FetchContractHoldingCoordinator = (data,isBindOperatingCoordinator) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    // const selectedCompany = data;
    // const coordinatorUrl = masterData.user; 
    // const param = {
    //     companyCode: selectedCompany,
    //     userType : 'Coordinator',
    //     isActive: true
    // };
    // const requestPayload = new RequestPayload(param);
    // const response = await FetchData(coordinatorUrl, requestPayload)

    // MI Coordinator API call starts here
    dispatch(actions.FetchContractHoldingCoordinator([]));
    dispatch(actions.FetchOperatingCoordinator([])); 
    if(data){
        const coordinatorUrl = masterData.coordinators;
        const param = data; 
        const requestPayload = new RequestPayload(param);
        const response = await PostData(coordinatorUrl,requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION, 'wariningToast HoldingCompanyCoordinatorValPreAssign');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
        if (!isEmpty(response)) {
            if (response.code === "1") {
                dispatch(actions.FetchContractHoldingCoordinator(response.result));
                if(isBindOperatingCoordinator)
                {
                    dispatch(actions.FetchOperatingCoordinator(response.result)); 
                }
            }
            else if (response.code === "41") {
                if (!isEmptyReturnDefault(response.validationMessages)) {
                    //To-Do: Common approach to display validationMessages
                }
            }
            else if (response.code === "11") {
                if (!isEmptyReturnDefault(response.messages)) {
                    //To-Do: Common approach to display messages
                }
            }
            else {

            }
            dispatch(HideLoader());
        }
        else {
            IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION, 'dangerToast HoldingCompanyCoordinatorSWWValPreAssign');
            dispatch(HideLoader());
        }
    }
    dispatch(HideLoader());
};

/** Fetch Coordinator for Operating Company action - pre assignment */
export const FetchOperatingCoordinator = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    // const selectedCompany = data;
    // const coordinatorUrl = masterData.user;
    // const param = {
    //     companyCode: selectedCompany,
    //     userType : 'Coordinator',
    //     isActive: true
    // };
    // const requestPayload = new RequestPayload(param);
    // const response = await FetchData(coordinatorUrl, requestPayload)

    dispatch(actions.FetchOperatingCoordinator([]));
    if(data){
        const coordinatorUrl = masterData.coordinators;
        const param = data;
        const requestPayload = new RequestPayload(param);
        const response = await PostData(coordinatorUrl,requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION, 'wariningToast OperatingCoordinatorValPreAssign');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (!isEmpty(response)) {
            if (response.code === "1") {
                dispatch(actions.FetchOperatingCoordinator(response.result));
            }
            else if (response.code === "41") {
                if (!isEmptyReturnDefault(response.validationMessages)) {
                    //To-Do: Common approach to display validationMessages
                }
            }
            else if (response.code === "11") {
                if (!isEmptyReturnDefault(response.messages)) {
                    //To-Do: Common approach to display messages
                }
            }
            else {

            }
            dispatch(HideLoader());
        }
        else {
            IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION, 'dangerToast OperatingCoordinatorSWWValPreAssign');
            dispatch(HideLoader());
        }
    }
    dispatch(HideLoader());
};

/** Clear Contract holding coordinator */
export const ClearContractHoldingCoordinator = () => (dispatch,getstate) => {
    const state = getstate();
    const searchParameter = Object.assign({},state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails.searchParameter);
    searchParameter.chCoordinatorLogOnName="";
    dispatch(actions.ClearContractHoldingCoordinator(searchParameter));
};

/** Clear Operating Coordinator */
export const ClearOperatingCoordinator = () => (dispatch,getstate) => {
    const state = getstate();
    const searchParameter = Object.assign({},state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails.searchParameter);
    searchParameter.opCoordinatorLogOnName="";
    dispatch(actions.ClearOperatingCoordinator(searchParameter));
};

/** Fetch Assignment Type action */
export const FetchAssignmentType = () => async (dispatch, getstate) => {
    if (!isEmptyOrUndefine(getstate().RootTechSpecReducer.TechSpecDetailReducer.assignmentType)) {
        return false;
    }
    const assignmentTypeUrl = assignmentAPIConfig.assignmentType;
    const response = await FetchData(assignmentTypeUrl, {})
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.FAILED_FETCH_ASSIGNMENT_TYPE, 'dangerToast assActassignTypeErrMsg');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response)) {
        if (response.code === '1') {
            dispatch(actions.FetchAssignmentType(response.result));
        }
        else if (response.code === "41") {
            if (!isEmptyReturnDefault(response.validationMessages)) {
                //To-Do: Common approach to display validationMessages
            }
        }
        else if (response.code === "11") {
            if (!isEmptyReturnDefault(response.messages)) {
                //To-Do: Common approach to display messages
            }
        }
        else {
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.FAILED_FETCH_ASSIGNMENT_TYPE, 'dangerToast assActassignTypeErrMsg1');
    }
};

/** Add Sub-Supplier action */
export const AddSubSupplier = (data) => (dispatch,getstate) => {
    if(data){
        const state = getstate();
        const searchParameter = Object.assign({},state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails.searchParameter);
        const subSuppliers = Object.assign([],searchParameter.subSupplierInfos);
        subSuppliers.push(data);
        dispatch(actions.AddSubSupplier(subSuppliers));
    }
};

/** Update Sub-Supplier action */
export const UpdateSubSupplier = (data, editedData) => (dispatch,getstate) => {
    if(data){
        const state = getstate();
        const searchParameter = Object.assign({},state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails.searchParameter);
        const subSuppliers = Object.assign([],searchParameter.subSupplierInfos);
        const subSupplierIndex = subSuppliers.findIndex(x=>x.subSupplier === editedData.subSupplier);
        if(subSupplierIndex >= 0){
            subSuppliers[subSupplierIndex] = data;
        }
        dispatch(actions.UpdateSubSupplier(subSuppliers));
    }
};

/** Delete Sub-Supplier action */
export const DeleteSubSupplier = (data) => (dispatch,getstate) => {
    if(data){
        const state = getstate();
        const searchParameter = Object.assign({},state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails.searchParameter);
        const subSuppliers = Object.assign([],searchParameter.subSupplierInfos);
        data.forEach(iteratedValue=>{
            subSuppliers.forEach((row,index)=>{
                if(iteratedValue.subSupplier === row.subSupplier){
                    subSuppliers.splice(index,1);
                }
            });
        });
        dispatch(actions.DeleteSubSupplier(subSuppliers));
    }
};

/** Action details update action */
export const UpdateActionDetails = (data) => (dispatch,getstate) => {
    if(data){
        const state = getstate();
        const actionDetails = Object.assign({},state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails,data);
        // const updatedData = mergeobjects(actionDetails,data);
        dispatch(actions.UpdateActionDetails(actionDetails));
    }
};

/** Save Pre-Assignment Details action */
export const SavePreAssignment = (data) => async (dispatch,getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    const preAssignmentDetails = Object.assign({},state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails);
    preAssignmentDetails.recordStatus = 'N';
    preAssignmentDetails.searchType="PreAssignment";
    const postUrl = GrmAPIConfig.preAssignmentSave;
    const requestPayload = new RequestPayload(preAssignmentDetails);
    const response = await PostData(postUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.PRE_ASSIGNMENT_POST_ERROR, 'dangerToast PreAssCreationErr');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if(response){
        if(response.code === '1'){
            dispatch(HideLoader());
            return true;
        }
        else if (response.code === "11" || response.code === "41") {
            if (response.validationMessages.length > 0) {
                response.validationMessages.map(result => {
                    if (result.messages.length > 0) {
                        result.messages.map(valMessage => {
                            dispatch(ValidationAlert(valMessage.message, "preAssignment"));
                        });
                    }
                    else {
                        dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "preAssignment"));
                    }
                });
            }
            else if (response.messages.length > 0) {
                response.messages.map(result => {
                    if (result.message.length > 0) {
                        dispatch(ValidationAlert(result.message, "preAssignment"));
                    }
                });
            }
            else {
                dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "preAssignment"));
            }
            dispatch(HideLoader());
            return false;
        }
        else {
            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "preAssignment"));
            dispatch(HideLoader());
            return false;
        }
    }
    else{
        dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "preAssignment"));
        dispatch(HideLoader());
        return false;
    }
};

/** Update Pre-Assignment Action */
export const UpdatePreAssignment = (data) => async (dispatch,getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    const preAssignmentDetails = deepCopy(state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails);
    preAssignmentDetails.recordStatus = 'M';
    preAssignmentDetails.searchType="PreAssignment";
    const postUrl = GrmAPIConfig.preAssignmentSave;
    const requestPayload = new RequestPayload(preAssignmentDetails);
    const response = await CreateData(postUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.PRE_ASSIGNMENT_UPDATE_ERROR, 'dangerToast preAssUpdationErr');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if(response){
        if(response.code === '1'){
            dispatch(HideLoader());
            return true;
        }
        else if (response.code === "11" || response.code === "41") {
            if (response.validationMessages.length > 0) {
                response.validationMessages.map(result => {
                    if (result.messages.length > 0) {
                        result.messages.map(valMessage => {
                            dispatch(ValidationAlert(valMessage.message, "preAssignment"));
                        });
                    }
                    else {
                        dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "preAssignment"));
                    }
                });
            }
            else if (response.messages.length > 0) {
                response.messages.map(result => {
                    if (result.message.length > 0) {
                        dispatch(ValidationAlert(result.message, "Assignment"));
                    }
                });
            }
            else {
                dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "preAssignment"));
            }
            dispatch(HideLoader());
            return false;
        }
        else {
            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "preAssignment"));
            dispatch(HideLoader());
            return false;
        }
    }
    else{
        dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "Assignment"));
        dispatch(HideLoader());
        return false;
    }
};

/** Search Pre-Assignment Technical Specialist List */
export const searchPreAssignmentTechSpec = (data) =>async (dispatch,getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    let payloadData = {};
    if(data){
        // ITK D-625
        if(data.searchType === "PreAssignment"){
            let searchParameter=deepCopy(data.searchParameter);
            searchParameter = bindSubSupplierLocation(searchParameter);
            data.searchParameter=searchParameter;  
            payloadData= data;
        }
        else{
            payloadData = data;
        }
    }
    else{
        const preAssignmentDetails = deepCopy(state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails);
        const selectedMyTask = isEmptyReturnDefault(state.rootAssignmentReducer.selectedARSMyTask, 'object');
        preAssignmentDetails.searchType="PreAssignment";
        preAssignmentDetails.taskType = selectedMyTask.taskType;
        let searchParameter=deepCopy(preAssignmentDetails.searchParameter);
        searchParameter = bindSubSupplierLocation(searchParameter);
        preAssignmentDetails.searchParameter=searchParameter;  
        payloadData= preAssignmentDetails; 
    }
    const teschSpecSearchUrl = GrmAPIConfig.preAssignmentTSSearch;
    const requestPayload = new RequestPayload(payloadData);
    const response = await PostData(teschSpecSearchUrl, requestPayload)
    .catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(localConstant.validationMessage.FAILED_FETCH_TECH_SPEC, 'dangerToast techspecErrMsg');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        dispatch(HideLoader());
    });
    if (!isEmpty(response)) {
        if (response.code === '1') {
            if(response.result && response.result.length > 0){
                //const data ={response:response.result,isShowGoogleMap:true};

                if(Array.isArray(response.result) && response.result.length >0 ){
                    response.result.forEach(eachLocObj =>{
                        const location = eachLocObj.supplierLocationId;
                        if(Array.isArray(eachLocObj.resourceSearchTechspecInfos)&& eachLocObj.resourceSearchTechspecInfos.length > 0){
                            eachLocObj.resourceSearchTechspecInfos.forEach(tsObj=>{
                                tsObj.supplierLocationId = location;
                            });
                        }
                    });
                }

                dispatch(actions.searchPreAssignmentTechSpec(response.result));
                if((response.result[0].supplierInfo !==null) && (response.result[0].supplierInfo.length > 0)){
                    dispatch(actions.isActiveGoogleMap(true));
                }else{
                    dispatch(actions.isActiveGoogleMap(false));
                }
                
            }
            else{
                dispatch(actions.searchPreAssignmentTechSpec([]));
                dispatch(actions.isActiveGoogleMap(false));
            }
        }
        else if (response.code === "41") {
            if (!isEmptyReturnDefault(response.validationMessages)) {
                //To-Do: Common approach to display validationMessages
            }
        }
        else if (response.code === "11") {
            if (!isEmptyReturnDefault(response.messages)) {
                //To-Do: Common approach to display messages
            }
        }
        else {
        }
        dispatch(HideLoader());
    }
    else {
        IntertekToaster(localConstant.validationMessage.FAILED_FETCH_TECH_SPEC, 'dangerToast techspecErrMsg1');
        dispatch(HideLoader());
    }
};

/** Update Pre-Assignment with assignment details */
export const UpdateAssignmentOnPreAssignment = (data) => (dispatch,getstate) => {
    // if(data.searchParameter && data.searchParameter.chCompanyCode){
    //     const chCompanyCode = data.searchParameter.chCompanyCode;
    //     dispatch(FetchContractHoldingCoordinator(chCompanyCode));
    // }
    // if(data.searchParameter && data.searchParameter.opCompanyCode){
    //     const opCompanyCode = data.searchParameter.opCompanyCode;
    //     dispatch(FetchOperatingCoordinator(opCompanyCode));
    // }
    dispatch(actions.UpdateAssignmentOnPreAssignment(data));
    // dispatch(searchPreAssignmentTechSpec(data));
};
//D576
export const TechSpechUnSavedData =(data) => (dispatch, getstate)=>{
    dispatch(actions.TechSpechUnSavedData(data));
};

/** Fetch DispositionType Values  */
export const FetchDispositionType =(data) => async (dispatch,getstate)=>{
    const isActive=true;
    const param = {
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    if(!required(data)){
        if(data === "SD" || data === "L"){
            param.type="S";
        } 
        else if(data === "PLO"){
            param.type="P";
        }
    }
    const requestPayload = new RequestPayload(param);
    const coordinatorUrl = masterData.dispositionType;
    const response = await FetchData(coordinatorUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.DISPOSITION_TYPE_VALIDATION, 'wariningToast dispositionValPreAssign');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response)) {
        if (response.code === "1") {
            dispatch(actions.FetchDispositionType(response.result));
        }
        else if (response.code === "41") {
            if (!isEmptyReturnDefault(response.validationMessages)) {
                //To-Do: Common approach to display validationMessages
            }
        }
        else if (response.code === "11") {
            if (!isEmptyReturnDefault(response.messages)) {
                //To-Do: Common approach to display messages
            }
        }
        else {

        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast HoldingCompanyCoordinatorSWWValPreAssign');
    }
};

/** Load pre-Assignment master data on redirection from dashboard */
export const LoadPreAssignmentMaster = (data) => (dispatch,getstate) => {  
    if(!required(data.chCompanyCode)){ 
        dispatch(FetchContractHoldingCoordinator({
            companyCode: data.chCompanyCode ,
            userTypes: [ "Coordinator","MICoordinator" ],
            isActiveCoordinators: true
        },(data.chCompanyCode == data.opCompanyCode)));// Intercompany CH coordinator not loading issue fix
    }
    if(!required(data.opCompanyCode) && data.chCompanyCode != data.opCompanyCode ){ // To avoid multiple api call if codes are same
        dispatch(FetchOperatingCoordinator({
            companyCode:  data.opCompanyCode,
            userTypes: [ "Coordinator","MICoordinator" ],
            isActiveCoordinators: true
        }));
    }
    if(!required(data.categoryName)){
        dispatch(ClearSubCategory());
        dispatch(ClearServices());
        dispatch(FetchTechSpecSubCategory(data.categoryName));
    }
    if(!required(data.categoryName) && !required(data.subCategoryName)){
        dispatch(ClearServices());
        dispatch(FetchTechSpecServices(data.categoryName,data.subCategoryName));
    }
};

/** Clear Pre-Assignment Details */
export const clearPreAssignmentDetails = (data) => (dispatch,getstate) => {
    dispatch(actions.FetchContractHoldingCoordinator([]));
    dispatch(actions.FetchOperatingCoordinator([]));
    dispatch(actions.FetchPreAssignment({}));
    dispatch(actions.searchPreAssignmentTechSpec([]));
    dispatch(actions.FetchDispositionType([]));
    dispatch(ClearData());
};

export const AddOptionalSearch= (data)=>(dispatch,getstate)=>{
    if(data){
        const state = getstate();
        const searchParameter = Object.assign({},state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails.searchParameter);
        const optionalSearch = Object.assign({},searchParameter.optionalSearch,data);
        dispatch(actions.AddOptionalSearch(optionalSearch));
    }
};

export const selectPreAssignmentTechSpec = (data) => (dispatch,getstate) => {
    const state = getstate();
    const techSpecList = Object.assign([],state.RootTechSpecReducer.TechSpecDetailReducer.techspecList);
    const mainSupplierTS = isEmptyReturnDefault(data.selectedTechSpecInfo);
    const subSupplierInfo = isEmptyReturnDefault(data.subSupplierInfos);
    techSpecList.forEach(y => {
        y.resourceSearchTechspecInfos && y.resourceSearchTechspecInfos.forEach(z => {
            z.isSelected = false; //Changes for IGO - 883
        });
    });
    dispatch(actions.searchPreAssignmentTechSpec([]));   
    mainSupplierTS.forEach(x => {
        techSpecList.forEach(y => {
            y.resourceSearchTechspecInfos && y.resourceSearchTechspecInfos.forEach(z => {
                if(z.epin === x.epin){
                    z.isSelected = true;
                }
            });
        });
    });
    subSupplierInfo.forEach(x => {
        const subSupplieTS = isEmptyReturnDefault(x.selectedSubSupplierTS);
        subSupplieTS.forEach(y => {
            techSpecList.forEach(t => {
                t.resourceSearchTechspecInfos && t.resourceSearchTechspecInfos.forEach(z => {
                    if(z.epin === y.epin){
                        z.isSelected = true;
                    }
                });
            });
        });
    });
    setTimeout(()=>{
        dispatch(actions.searchPreAssignmentTechSpec(techSpecList)); 
    }, 0);
    // dispatch(actions.searchPreAssignmentTechSpec(techSpecList));
};

/** Cancel Pre-Assignment - Create Mode */
export const CancelCreatePreAssignmentDetails = () => (dispatch,getstate) => {
    dispatch(clearPreAssignmentDetails());
    dispatch(ClearSubCategory());
    dispatch(ClearServices());
};

/** Cancel Pre-Assignment - Edit Mode */
export const CancelEditPreAssignmentDetails = () => (dispatch,getstate) => {
    dispatch(ClearData());
    const state = getstate();
    const preAssignmentDetails = Object.assign({},state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails);
    dispatch(FetchPreAssignment({ id:preAssignmentDetails.id }));
};

/** Get Pre-Assignment Geo location information */
export const GetGeoLocationInfo = (data) =>async (dispatch,getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    let searchParameter = {};
    if(data){
        searchParameter = data;
    }
    else{
        //searchParameter = Object.assign({},state.RootTechSpecReducer.TechSpecDetailReducer.techspecList);
        searchParameter = Object.keys(state.RootTechSpecReducer.TechSpecDetailReducer.techspecList).map(function (i) {
            return state.RootTechSpecReducer.TechSpecDetailReducer.techspecList[i];
          });   
    }
    const tsGeoLocationSearchUrl = GrmAPIConfig.preAssignmentGeoLocation;
    const requestPayload = new RequestPayload(searchParameter);
    const response = await PostData(tsGeoLocationSearchUrl, requestPayload)
    .catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(localConstant.validationMessage.FAILED_FETCH_GEO_LOCATION, 'dangerToast techspecErrMsg');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        dispatch(HideLoader());
    });
    if (!isEmpty(response)) {
        dispatch(HideLoader());
        if (response.code === '1') {
            if(response.result && response.result.length > 0){
                if(Array.isArray(response.result) && response.result.length >0 ){
                    response.result.forEach(eachLocObj =>{
                        const location = eachLocObj.supplierLocationId;
                        if(Array.isArray(eachLocObj.resourceSearchTechspecInfos)&& eachLocObj.resourceSearchTechspecInfos.length > 0){
                            eachLocObj.resourceSearchTechspecInfos.forEach(tsObj=>{
                                tsObj.supplierLocationId = location;
                            });
                        }
                    });
                }
                dispatch(actions.searchPreAssignmentTechSpec(response.result));
            }
            else{
                dispatch(actions.searchPreAssignmentTechSpec([]));
            } 
            return true;
        }
        else if (response.code === "41") {
            if (!isEmptyReturnDefault(response.validationMessages)) {
                //To-Do: Common approach to display validationMessages
            }
        }
        else if (response.code === "11") {
            if (!isEmptyReturnDefault(response.messages)) {
                //To-Do: Common approach to display messages
            }
        } 
    }
    else {
        IntertekToaster(localConstant.validationMessage.FAILED_FETCH_GEO_LOCATION, 'dangerToast techspecErrMsg1');
        dispatch(HideLoader());  
    }
    return false;
};

/**
 * ITK D-625
 * Common function for binding supplier name and supplier location for preAssignment Search.
 * @param {*} searchParam 
 */
export const bindSubSupplierLocation = (searchParam) => {
    if(searchParam){
        let subSupplierInfo = [];
        searchParam.supplierLocation=  searchParam.supplierLocation;
        searchParam.supplierFullAddress= searchParam.supplierLocation;//D-911
        if(searchParam.subSupplierInfos && searchParam.subSupplierInfos.length >0){
            subSupplierInfo = searchParam.subSupplierInfos.map((val,i) =>{
                const subSupplier = Object.assign({},val); // D-625
                subSupplier["subSupplier"]=val.subSupplier;
                subSupplier["subSupplierLocation"]= `${ val.subSupplierLocation }`;
                subSupplier["subSupplierFullAddress"] = subSupplier.subSupplierLocation; //SubSupplierFullAddress Changes
                return subSupplier;
            });
        }
        searchParam.subSupplierInfos=subSupplierInfo;
    }
    return searchParam;
};