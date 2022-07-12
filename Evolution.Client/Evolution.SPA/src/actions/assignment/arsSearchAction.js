import { techSpecActionTypes, assignmentsActionTypes } from '../../constants/actionTypes';
import { techSpecActionTypes as grmTechSpecActionTypes } from '../../grm/constants/actionTypes';
import { masterData, RequestPayload, supplierPOApiConfig, GrmAPIConfig, supplierAPIConfig,userAPIConfig } from '../../apiConfig/apiConfig';
import { getlocalizeData, isEmpty, isEmptyReturnDefault, fetchCoordinatorDetails, parseValdiationMessage, deepCopy } from '../../utils/commonUtils';
import { FetchData, PostData, CreateData } from '../../services/api/baseApiService';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import {
    UpdateAssignmentOnPreAssignment,
    UpdatePreAssignmentSearchParam,
    UpdateActionDetails,
    LoadPreAssignmentMaster,
    searchPreAssignmentTechSpec,
    selectPreAssignmentTechSpec,
    FetchDispositionType,
} from '../../grm/actions/techSpec/preAssignmentAction';
import { AssignTechnicalSpecialist, ARSSearchPanelStatus, AssignResourcesButtonClick } from './assignedSpecialistsAction';
import { UpdateAssignmentGeneralDetails } from './generalDetailsActions';
import { ShowLoader, HideLoader } from '../../common/commonAction';
import { required } from '../../utils/validator';
import { StringFormat } from '../../utils/stringUtil';
import { ValidationAlert } from '../../components/viewComponents/customer/alertAction';
import { FetchAssignmentStatus, FetchAssignmentDetail } from './assignmentAction';
import { isTimesheet, isVisit } from '../../selectors/assignmentSelector';
import moment from 'moment';

const localConstant = getlocalizeData();

const actions = {
    FetchPreAssignmentIds: (payload) => ({
        type: techSpecActionTypes.FETCH_PRE_ASSIGNMENT_IDS,
        data: payload
    }),
    SetDefaultPreAssignmentName: (payload) => ({
        type: techSpecActionTypes.SET_DEFAULT_PREASSIGNMENT_ID,
        data: payload
    }),
    AssignTechnicalSpecialistFromARS: (payload) => ({
        type: techSpecActionTypes.ASSIGN_TECHNICAL_SPECIALISTS,
        data: payload
    }),
    GetSelectedPreAssignment: (payload) => ({
        type: techSpecActionTypes.GET_SELECTED_PRE_ASSIGNMENT,
        data: payload
    }),
    compareARSResourcesWithPreAssignment: (payload) => ({
        type: techSpecActionTypes.COMPARE_ARS_ASSIGNMENT_RESOURCES,
        data: payload
    }),
    unmatchedResources: (payload) => ({
        type: techSpecActionTypes.UNMATCHED_RESOURCES,
        data: payload
    }),
    FetchOperationalManagers: (payload) => ({
        type: techSpecActionTypes.FETCH_OPERATIONAL_MANAGERS,
        data: payload
    }),
    FetchTechnicalSpecialists: (payload) => ({
        type: techSpecActionTypes.FETCH_TECHNICAL_SPECIALISTS,
        data: payload
    }),
    clearARSSearchDetails: (payload) => ({
        type: techSpecActionTypes.CLEAR_ARS_SEARCH_DETAILS,
        data: payload
    }),
    selectedARSMyTask: (payload) => ({
        type: techSpecActionTypes.SELECTED_ARS_MY_TASK,
        data: payload
    }),
    FetchPLOTechSpecServices: (payload) => ({
        type: techSpecActionTypes.FETCH_PLO_TECHSPEC_SERVICES,
        data: payload
    }),
    FetchPLOTechSpecSubCategory: (payload) => ({
        type: techSpecActionTypes.FETCH_PLO_TECHSPEC_SUB_CATEGORY,
        data: payload
    }),
    ClearPLOServices: (payload) => ({
        type: techSpecActionTypes.CLEAR_PLO_TECHSPEC_SERVICES,
        data: payload
    }),
    ClearPLOSubCategory: (payload) => ({
        type: techSpecActionTypes.CLEAR_PLO_TECHSPEC_SUB_CATEGORY,
        data: payload
    }),
    OverridenResources: (payload) => ({
        type: techSpecActionTypes.OVERRIDEN_RESOURCES,
        data: payload
    }),
    AddSupSupplierTechSpec: (payload) => ({
        type: assignmentsActionTypes.ADD_ASSIGNMENT_SUB_SUPPLIER_TECH_SPEC,
        data: payload
    }),
    FetchContractHoldingCoordinator: (payload) => ({
        type: grmTechSpecActionTypes.FETCH_CONTRACT_HOLDING_COORDINATOR,
        data: payload
    }),
    FetchOperatingCoordinator: (payload) => ({
        type: grmTechSpecActionTypes.FETCH_OPERATING_COORDINATOR,
        data: payload
    }),
    ARSCHCoordinatorInfo: (payload) => ({
        type: grmTechSpecActionTypes.ARS_CH_COORDINATOR_INFO,
        data: payload
    }),
    ARSOPCoordinatorInfo: (payload) => ({
        type: grmTechSpecActionTypes.ARS_OP_COORDINATOR_INFO,
        data: payload
    }),
    FetchARSCommentsHistoryReport: (payload) => ({
        type: grmTechSpecActionTypes.FETCH_ARS_COMMENT_HISTORY_REPORT,
        data: payload
    }),
    // SaveMainSupplierSelectedTS:(payload)=>({
    //     type:assignmentsActionTypes.MAIN_SUPPLIER_SELECTED_TECHSPEC,
    //     data: payload
    // })
};

/** Pre-Assignment Popup loading */
export const FetchPreAssignmentIds = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    const assignmentInfo = Object.assign({}, state.rootAssignmentReducer.assignmentDetail.AssignmentInfo);
    const assignmentTaxonomy = Object.assign({}, state.rootAssignmentReducer.assignmentDetail.AssignmentTaxonomy);
    const operatingCompanyCoordinatorUsername = fetchCoordinatorDetails(assignmentInfo.assignmentOperatingCompanyCoordinator,
        state.rootAssignmentReducer.operatingCompanyCoordinators, "displayName", "username");
    const postUrl = GrmAPIConfig.preAssignmentSave;

    /**
     * Removed serviceName and mainSupplierName param for ITK D-625
     */
    const param = {
        "searchType": "PreAssignment",
        "searchAction": "W",
        "companyCode": assignmentInfo.assignmentOperatingCompanyCode,
        "assignedTo": operatingCompanyCoordinatorUsername[0],
    };
    if (data && data.allCoOrdinator) {
        param.assignedTo = "";
    }
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(postUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FETCH_PRE_ASSIGNMENT_FAILED, 'dangerToast fetchPreAssignmentFailed');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast viewRole');
            dispatch(HideLoader());
        });
    if (response && response.code === '1') {
        if (response.result && response.result.length > 0) {
            response.result.forEach(iteratedValue => {
                iteratedValue.searchParameter = JSON.parse(iteratedValue.searchParameter);
            });
            dispatch(actions.FetchPreAssignmentIds(response.result));
        }
        else {
            dispatch(actions.FetchPreAssignmentIds([]));
        }
    }
    else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast fetchPreAssignmentFailed');
    }
    else {
        IntertekToaster(localConstant.errorMessages.FETCH_PRE_ASSIGNMENT_FAILED, "dangerToast fetchPreAssignmentFailed");
    }
    dispatch(HideLoader());
};

/**Selected Pre-Assignment Value  */
export const SetDefaultPreAssignmentName = (payload) => async (dispatch) => {
    dispatch(actions.SetDefaultPreAssignmentName(payload));
};

/** Load Assignment data to ARS fields */
export const LoadARSSearchData = (techSpecData) => (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    const assignmentInfo = Object.assign({}, state.rootAssignmentReducer.assignmentDetail.AssignmentInfo);
    const assignmentTaxonomy = Object.assign([], state.rootAssignmentReducer.assignmentDetail.AssignmentTaxonomy);
    const assignmentSubSupplier = Object.assign([], state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers);
    const supplierPOList = Object.assign([], state.rootAssignmentReducer.supplierPO);
    const opCoordinatorList = Object.assign([], state.rootAssignmentReducer.operatingCompanyCoordinators);
    const { assignmentProjectWorkFlow } = assignmentInfo;

    const opCompanyCoordinatorUsername = fetchCoordinatorDetails(assignmentInfo.assignmentOperatingCompanyCoordinator,
        opCoordinatorList, "displayName", "username");
    const chCompanyCoordinatorUsername = fetchCoordinatorDetails(assignmentInfo.assignmentContractHoldingCompanyCoordinator,
        opCoordinatorList, "displayName", "username");

    //Checks wheather assignment type is visit or timesheet
    const assinmentType = (workFlowType, listOfWorkFlows) => {
        return listOfWorkFlows.includes(workFlowType && workFlowType.trim());
    };
    const workflowType = {
        workflowTypeParams: localConstant.commonConstants.workFlow,
        workflowType: assignmentInfo.assignmentProjectWorkFlow && assignmentInfo.assignmentProjectWorkFlow.trim()
    };
    const arsDetail = Object.assign({}, state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails);
    arsDetail.searchParameter = Object.assign({}, arsDetail.searchParameter);
    arsDetail.searchParameter.optionalSearch = Object.assign({}, arsDetail.searchParameter.optionalSearch);
    arsDetail.searchParameter.subSupplierInfos = Object.assign([], arsDetail.searchParameter.subSupplierInfos);
    arsDetail.searchParameter.selectedTechSpecInfo = isTimesheet(workflowType) ? techSpecData : Object.assign([], arsDetail.searchParameter.selectedTechSpecInfo);
    arsDetail.searchType = "ARS";
    arsDetail.customerCode = assignmentInfo.assignmentCustomerCode;
    arsDetail.assignmentId = assignmentInfo.assignmentId;
    arsDetail.companyCode = assignmentInfo.assignmentOperatingCompanyCode;
    arsDetail.searchParameter.customerCode = assignmentInfo.assignmentCustomerCode;
    arsDetail.searchParameter.customerName = assignmentInfo.assignmentCustomerName;
    arsDetail.searchParameter.contractNumber = assignmentInfo.assignmentContractNumber;
    arsDetail.searchParameter.projectName = assignmentInfo.assignmentCustomerProjectName;
    arsDetail.searchParameter.projectNumber = assignmentInfo.assignmentProjectNumber;
    arsDetail.searchParameter.assignmentNumber = assignmentInfo.assignmentNumber; // TO-DO: check the assignmentInfo with assignment Number property.
    arsDetail.searchParameter.chCompanyCode = assignmentInfo.assignmentContractHoldingCompanyCode;
    arsDetail.searchParameter.chCompanyName = assignmentInfo.assignmentContractHoldingCompany;
    arsDetail.searchParameter.chCoordinatorLogOnName = chCompanyCoordinatorUsername && chCompanyCoordinatorUsername[0];
    arsDetail.searchParameter.opCompanyCode = assignmentInfo.assignmentOperatingCompanyCode;
    arsDetail.searchParameter.opCompanyName = assignmentInfo.assignmentOperatingCompany;
    arsDetail.searchParameter.opCoordinatorLogOnName = opCompanyCoordinatorUsername && opCompanyCoordinatorUsername[0];
    arsDetail.searchParameter.assignmentType = assignmentInfo.assignmentType;
    arsDetail.searchParameter.materialDescription = assignmentInfo.materialDescription; // TO-DO: assign property of Equipment Knowledge from assignment Info.
    arsDetail.searchParameter.assignmentStatus = assignmentInfo.assignmentStatus; // TO-DO: add assignment status on ars json
    arsDetail.searchParameter.workFlowType = assignmentProjectWorkFlow && assignmentProjectWorkFlow.trim();
    if (assignmentTaxonomy.length > 0) {
        arsDetail.categoryName = assignmentTaxonomy[0].taxonomyCategory;
        arsDetail.subCategoryName = assignmentTaxonomy[0].taxonomySubCategory;
        arsDetail.serviceName = assignmentTaxonomy[0].taxonomyService;
        arsDetail.searchParameter.categoryName = assignmentTaxonomy[0].taxonomyCategory;
        arsDetail.searchParameter.subCategoryName = assignmentTaxonomy[0].taxonomySubCategory;
        arsDetail.searchParameter.serviceName = assignmentTaxonomy[0].taxonomyService;
        arsDetail.searchParameter.serviceId = assignmentTaxonomy[0].taxonomyServiceId;
        
    }
    dispatch(actions.FetchContractHoldingCoordinator(state.rootAssignmentReducer.contractHoldingCompanyCoordinators));
    dispatch(actions.FetchOperatingCoordinator(state.rootAssignmentReducer.operatingCompanyCoordinators));
   
    if (isVisit(workflowType)) {
        // Commented to bind default value of Search Action on openeing on ARS twice from Assignment without saving it.
        // if (!isEmpty(arsDetail.searchAction)) {
        //     arsDetail.searchAction = "";
        // }
        dispatch(updateARSvisitData(arsDetail));
    }
    else if (isTimesheet(workflowType)) {
        // Commented to bind default value of Search Action on openeing on ARS twice from Assignment without saving it.
        // if (!isEmpty(arsDetail.searchAction)) {
        //     arsDetail.searchAction = "";
        // }
        dispatch(updateTimesheetARSData(arsDetail));
    } else {
        dispatch(HideLoader());
    }
};

const GetSubSupplierTS = (resourceList, assignmentSubSupplier) => {
    const arsSupplierResourceList=isEmptyReturnDefault(resourceList);
    if(!isEmpty(assignmentSubSupplier)){
        const assignmentSubSupplierTS = isEmptyReturnDefault(assignmentSubSupplier.assignmentSubSupplierTS);
        assignmentSubSupplierTS.forEach(x => {
            if(x.isAssignedToThisSubSupplier !== false && x.recordStatus !=="D" && x.isDeleted != true){
                const isTSExist = resourceList.find(y => y.epin === x.epin && x.recordStatus !== 'D');
                if(!isTSExist){
                    arsSupplierResourceList.push({ epin : x.epin });
                }
            }
        });
    }
    return arsSupplierResourceList;
};

const GetMainSupplierTS = (searchParamTSInfo,assignmentSubSupplier)  =>  {//MS-TS Link CR
    const mainSupplierTS = searchParamTSInfo;
    //Optimized - NOV 06 - MS-TS Link CR
    const assignmentSubSupplierMainTs=assignmentSubSupplier.length > 0 && assignmentSubSupplier[0];
    assignmentSubSupplierMainTs && assignmentSubSupplierMainTs.assignmentSubSupplierTS.forEach(value=>{
        if(value.isAssignedToThisSubSupplier === false && value.recordStatus !=="D" && value.isDeleted != true){
            const isTSExist = mainSupplierTS.find(x => x.epin == value.epin);
            if(!isTSExist)
                mainSupplierTS.push({ epin:value.epin });
        }
    });
    return mainSupplierTS;
};

export const updateARSvisitData = (data) => (dispatch, getstate) => {
    const state = getstate();
    const currentPage=state.CommonReducer.currentPage;//MS-TS Link
    const assignmentInfo = Object.assign({},state.rootAssignmentReducer.assignmentDetail.AssignmentInfo);
    const assignmentSubSupplier = Object.assign([],state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers);
    const supplierPOList = Object.assign([],state.rootAssignmentReducer.supplierPO);
    const subSupplierList = Object.assign([],state.rootAssignmentReducer.subsuppliers);
    const mainSupplierSelectedTS = isEmptyReturnDefault(state.rootAssignmentReducer.mainSupplierSelectedTS);
    const assignmentTechnicalSpecialists = Object.assign([], state.rootAssignmentReducer.assignmentDetail.AssignmentTechnicalSpecialists);
    const assignmentVisitStatus = localConstant.commonConstants.assignmentVisitStatus.filter(x => x.code === (assignmentInfo.visitStatus ? assignmentInfo.visitStatus : 'T'));
    if (data) {
        const arsDetail = deepCopy(data);
        arsDetail.searchParameter.supplier = assignmentInfo.assignmentSupplierName;
        arsDetail.searchParameter.supplierId = assignmentInfo.assignmentSupplierId;
        arsDetail.searchParameter.supplierPurchaseOrder = assignmentInfo.assignmentSupplierPurchaseOrderNumber; // TO-DO: add supplierPO in ars json
        arsDetail.searchParameter.assignmentCreatedDate = assignmentInfo.visitFromDate && moment(assignmentInfo.visitFromDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT).toString();
        arsDetail.searchParameter.firstVisitFromDate = assignmentInfo.visitFromDate && moment(assignmentInfo.visitFromDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT).toString();
        arsDetail.searchParameter.firstVisitToDate = assignmentInfo.visitToDate && moment(assignmentInfo.visitToDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT).toString();
        arsDetail.searchParameter.firstVisitStatus = assignmentVisitStatus.length > 0 && assignmentVisitStatus[0].name;
        arsDetail.searchParameter.selectedTechSpecInfo = GetMainSupplierTS(arsDetail.searchParameter.selectedTechSpecInfo,assignmentSubSupplier);//MS-TS Link CR
        /** getting supplierPOId from supplierPONumber */
        const supplierPOIndex = supplierPOList.findIndex(x => x.id  == assignmentInfo.assignmentSupplierPurchaseOrderId);
        const supplierPOObj = supplierPOList.find(x =>  x.id == assignmentInfo.assignmentSupplierPurchaseOrderId);
        if (supplierPOObj) {
            const supLoc = !required(supplierPOObj.city) ? supplierPOObj.city : supplierPOObj.zipCode;

            let mainSupplierFullAddress = {};
            if(supplierPOObj.country)
                mainSupplierFullAddress.country = supplierPOObj.country;
            if(supplierPOObj.state)
                mainSupplierFullAddress.state = supplierPOObj.state;
            if(supplierPOObj.city)
                mainSupplierFullAddress.city = supplierPOObj.city;
            if(supplierPOObj.zipCode)
                mainSupplierFullAddress.zipCode = supplierPOObj.zipCode;
            
            mainSupplierFullAddress = Object.values(mainSupplierFullAddress).join(",");

            // arsDetail.searchParameter.supplierLocation = `${ assignmentInfo.assignmentSupplierName }, ${ supLoc }`;
            // arsDetail.searchParameter.supplierFullAddress = `${ assignmentInfo.assignmentSupplierName }, ${ mainSupplierFullAddress }`;//supplierPOObj.supplierPOMainSupplierAddress;
            const mainSupplierName =  assignmentInfo.assignmentSupplierName.replace(/,/g, ''); //Changes for D1564
            arsDetail.searchParameter.supplierLocation = `${ supLoc }`;
            arsDetail.searchParameter.supplierFullAddress = `${ mainSupplierFullAddress }`;//supplierPOObj.supplierPOMainSupplierAddress;
            arsDetail.searchParameter.materialDescription = supplierPOObj.supplierPOMaterialDescription;

            const filteredAssignmentSubSupplier = assignmentSubSupplier.filter(x => x.recordStatus !== 'D');
            // If main supplier is first visit
            if(filteredAssignmentSubSupplier.length > 0 && filteredAssignmentSubSupplier[0].isMainSupplierFirstVisit){
                arsDetail.searchParameter.firstVisitSupplierId = filteredAssignmentSubSupplier[0].mainSupplierId;
                arsDetail.searchParameter.firstVisitLocation = `${ assignmentInfo.assignmentSupplierName }, ${ mainSupplierFullAddress }`;
            }
            
            if (filteredAssignmentSubSupplier.length > 0 && filteredAssignmentSubSupplier.some(sub => sub.subSupplierName)) {
                filteredAssignmentSubSupplier.forEach((iteratedValue, index) => {
                    if (iteratedValue.subSupplierId != null) {
                        const arsSupplierInfo = arsDetail.searchParameter.subSupplierInfos;
                        arsDetail.searchParameter.firstVisitSupplierId = (iteratedValue && iteratedValue.isSubSupplierFirstVisit) ? iteratedValue.subSupplierId : arsDetail.searchParameter.firstVisitSupplierId;
                        const subSupplierInfo = subSupplierList.filter(x => x.supplierId === iteratedValue.subSupplierId);
                        const subsupplierName = subSupplierInfo[0] && subSupplierInfo[0].subSupplierName.replace(/,/g, ''); //Changes for Hot Fix D669
                        const arsSubSupplierIndex = arsSupplierInfo.findIndex(x => x.subSupplierId === iteratedValue.subSupplierId);
                        if (subSupplierInfo.length > 0) {
                            const subSupLoc = !required(subSupplierInfo[0].city) ? subSupplierInfo[0].city : subSupplierInfo[0].postalCode;
                            let subSupplierAddress = {};
                            const subSupplierInfoObj = subSupplierInfo[0];
                            if (subSupplierInfoObj.country)
                                subSupplierAddress.country = subSupplierInfoObj.country;
                            if (subSupplierInfoObj.state)
                                subSupplierAddress.state = subSupplierInfoObj.state;
                            if (subSupplierInfoObj.city)
                                subSupplierAddress.city = subSupplierInfoObj.city;
                            if (subSupplierInfoObj.zipCode)
                                subSupplierAddress.zipCode = subSupplierInfoObj.zipCode;

                            subSupplierAddress = Object.values(subSupplierAddress).join(",");

                            if (iteratedValue && iteratedValue.isSubSupplierFirstVisit) {
                                arsDetail.searchParameter.firstVisitLocation = `${ subsupplierName }, ${ subSupplierAddress }`;
                            }

                            if (arsSubSupplierIndex >= 0) {
                                arsSupplierInfo[arsSubSupplierIndex].subSupplier = subsupplierName;
                                arsSupplierInfo[arsSubSupplierIndex].subSupplierId = subSupplierInfo[0].supplierId;
                                arsSupplierInfo[arsSubSupplierIndex].subSupplierLocation = `${ subSupLoc }`;
                                arsSupplierInfo[arsSubSupplierIndex].subSupplierFullAddress = `${ subSupplierAddress }`;
                                arsSupplierInfo[arsSubSupplierIndex].selectedSubSupplierTS = GetSubSupplierTS(arsSupplierInfo[arsSubSupplierIndex].selectedSubSupplierTS, iteratedValue);
                            } else {
                                const subSupplier = {};
                                subSupplier.subSupplier = subsupplierName;
                                subSupplier.subSupplierId = subSupplierInfo[0].supplierId;
                                subSupplier.subSupplierLocation = `${ subSupLoc }`;
                                subSupplier.subSupplierFullAddress = `${ subSupplierAddress }`; //subSupplierInfo[0].subSupplierAddress;
                                subSupplier.selectedSubSupplierTS = GetSubSupplierTS([], iteratedValue);
                                arsSupplierInfo.push(subSupplier);
                            }
                        }
                        arsDetail.searchParameter.subSupplierInfos = arsSupplierInfo;
                    }
                });
                dispatch(UpdateAssignmentOnPreAssignment(arsDetail));
                dispatch(searchPreAssignmentTechSpec(arsDetail));  
            }
            else {
                dispatch(UpdateAssignmentOnPreAssignment(arsDetail));
                dispatch(searchPreAssignmentTechSpec(arsDetail));
            }
        } else {
            dispatch(HideLoader());
        }
    }
    else{
        dispatch(HideLoader());
    }
};
            
export const updateTimesheetARSData = (data) => (dispatch,getstate) => {
    const state = getstate();
    const assignmentInfo = Object.assign({}, state.rootAssignmentReducer.assignmentDetail.AssignmentInfo);
    const assignmentTimesheetStatus = localConstant.commonConstants.timesheet_Status.filter(x => x.value === (assignmentInfo.timesheetStatus ? assignmentInfo.timesheetStatus : 'N'));
    if (data) {
        const arsDetail = data;
        arsDetail.searchParameter.assignmentCreatedDate = assignmentInfo.timesheetFromDate;
        arsDetail.searchParameter.firstVisitFromDate = assignmentInfo.timesheetFromDate && moment(assignmentInfo.timesheetFromDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT).toString();
        arsDetail.searchParameter.firstVisitToDate = assignmentInfo.timesheetToDate && moment(assignmentInfo.timesheetToDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT).toString();
        arsDetail.searchParameter.firstVisitStatus = assignmentTimesheetStatus.length > 0 && assignmentTimesheetStatus[0].name;
        const timesheetLocationArray = [];
        assignmentInfo.workLocationCity && timesheetLocationArray.push(assignmentInfo.workLocationCity);
        assignmentInfo.workLocationCounty && timesheetLocationArray.push(assignmentInfo.workLocationCounty);
        assignmentInfo.workLocationCountry && timesheetLocationArray.push(assignmentInfo.workLocationCountry);
        const timesheetLocation = timesheetLocationArray.length > 0 ? timesheetLocationArray.join() : "";
        arsDetail.searchParameter.firstVisitLocation = timesheetLocation;
        arsDetail.searchParameter.supplierLocation = timesheetLocation;
        arsDetail.searchParameter.supplierFullAddress = timesheetLocation;
        dispatch(UpdateAssignmentOnPreAssignment(arsDetail));
        dispatch(searchPreAssignmentTechSpec(arsDetail));
    }
};

/** Load Supplier PO details to ARS search data */
export const LoadSupplierPODetails = (data) => async (dispatch, getstate) => {
    if (data) {
        const state = getstate();
        const supplierUrl = StringFormat(supplierPOApiConfig.supplierPODetails, data.supplierPOId);
        const params = {};
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(supplierUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                //IntertekToaster(localConstant.validationMessage.SUPPLER_PO_FETCH_VALIDATION, 'warningToast supplierPoFetchForARSVal');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                return false;
            });
        if (!isEmpty(response)) {
            const supplierPOInfo = isEmptyReturnDefault(response.SupplierPOInfo, 'object');
            return supplierPOInfo;
        }
        else {
            IntertekToaster(localConstant.supplier.supplierValidationMessages.API_WENT_WRONG, 'dangerToast fetchSupplierPoForARSApiFailure');
            return false;
        }
    }
    else {
        return false;
    }
};

/** Load Supplier and sub-supplier details */
export const LoadSupplierDetails = (data) => async (dispatch, getstate) => {
    if (data && data.supplierId) {
        const state = getstate();
        const supplierUrl = StringFormat(supplierAPIConfig.fetchSupplier, data.supplierId);
        const params = {};
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(supplierUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
              //  IntertekToaster(localConstant.validationMessage.SUPPLER_FETCH_VALIDATION, 'warningToast supplierFetchVal');
              IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                return false;
            });
        if (!isEmpty(response)) {
            return response;
        }
        else {
            IntertekToaster(localConstant.validationMessage.API_WENT_WRONG, 'dangerToast fetchSupplierApiFailure');
            return false;
        }
    }
    else {
        return false;
    }
};

/** Assign Technical Specialist Action */
export const AssignTechnicalSpecialistFromARS = (data) => (dispatch, getstate) => {
    if (data && data.length > 0) {
        const state = getstate();
        const assignmentInfo = Object.assign({}, state.rootAssignmentReducer.assignmentDetail.AssignmentInfo);
        const assignedTechSpec = Object.assign([], state.rootAssignmentReducer.assignmentDetail.AssignmentTechnicalSpecialists);
        const preAssignment = Object.assign({}, state.rootAssignmentReducer.selectedPreAssignment);
        const loginUser = state.appLayoutReducer.loginUser;
        data.forEach(iteratedValue => {
            const techSpec = {};
            techSpec.technicalSpecialistName = iteratedValue.lastName ? `${ iteratedValue.lastName } ${ iteratedValue.firstName }` : iteratedValue.firstName;
            techSpec.epin = iteratedValue.epin;
            techSpec.isActive = true;
            techSpec.isSupervisor = false;
            techSpec.assignmentId = assignmentInfo.assignmentId;
            // techSpec.assignmentTechnicalSplId = Math.floor(Math.random() * 99) -100;
            techSpec.assignmentTechnicalSplId = null;
            techSpec.assignmentTechnicalSplUniqueId = Math.floor(Math.random() * 9999) - 10000;
            techSpec.assignmentTechnicalSpecialistSchedules = [];
            techSpec.modifiedBy = loginUser;
            techSpec.recordStatus = "N";
            assignedTechSpec.push(techSpec);
        });
        dispatch(AssignTechnicalSpecialist(assignedTechSpec));
        const preAssignmentDetail = { "preAssignmentId": preAssignment.id };
        dispatch(UpdateAssignmentGeneralDetails(preAssignmentDetail));
    }
};

/** Get ARS Search data for Already created assignment */
export const GetARSSearchData = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    if (data) {
        const apiUrl = GrmAPIConfig.arsSearch;
        const param = {
            "assignmentId": data.assignmentId
        };
        const requestPayload = new RequestPayload(param);
        const response = await FetchData(apiUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                //IntertekToaster(localConstant.errorMessages.FETCH_ARS_SEARCH_FAILED, 'dangerToast arsFetchdashErr');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response) {
            //To-do Search Result Grid Bind
            if (response.code === "1") {
                if (!isEmpty(response.result)) {
                    const preAssignmentSearchData = response.result.searchParameter;
                    dispatch(LoadPreAssignmentMaster(preAssignmentSearchData));
                    dispatch(FetchAssignmentStatus());
                    dispatch(searchPreAssignmentTechSpec(response.result));
                    response.result.searchAction = "";
                    response.result.description = "";
                    dispatch(UpdateActionDetails(response.result));
                    // TO-DO: Remove the commented lines and update the hardcoded id
                    // if(!isEmpty(response.result.id) && response.result.id !== 0){
                    // dispatch(GetMyTaskARSSearch({ taskRefCode:23 }));
                    // }
                }
                else {
                    dispatch(UpdateActionDetails({}));
                    IntertekToaster(localConstant.errorMessages.FETCH_ARS_SEARCH_FAILED, 'dangerToast arsFetchdashErr');
                }
            }
            else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast arsFetchdashErr');
            }
            else {
                IntertekToaster(localConstant.errorMessages.FETCH_ARS_SEARCH_FAILED, `dangerToast arsFetchdashErr`);
            }
        }
        else {
            IntertekToaster(localConstant.errorMessages.FETCH_ARS_SEARCH_FAILED, `dangerToast arsFetchdashErr`);
        }
        dispatch(HideLoader());
    }
    else {
        IntertekToaster(localConstant.errorMessages.FETCH_ARS_SEARCH_FAILED, `dangerToast arsFetchdashErr`);
        dispatch(HideLoader());
    }

};

/** Get selected pre assignment details */
export const GetSelectedPreAssignment = (data) => async (dispatch, getstate) => {
    //  dispatch(ShowLoader());

    if (data && data.id) {
        dispatch(actions.GetSelectedPreAssignment(data));
        dispatch(compareARSResourcesWithPreAssignment(data));
        // const apiUrl = GrmAPIConfig.preAssignmentSave;
        // const param = {
        //     "id" : data.id
        // };
        // const requestPayload = new RequestPayload(param);
        // const response = await FetchData(apiUrl, requestPayload)
        //     .catch(error => {
        //         IntertekToaster(localConstant.errorMessages.FETCH_PRE_ASSIGNMENT_FAILED, 'dangerToast preAssignmentErr');
        //     });
        // if (response) {
        //     //To-do Search Result Grid Bind
        //     if (response.code === "1") {
        //         if(response.result && response.result.length > 0){
        //             response.result[0].searchParameter = JSON.parse(response.result[0].searchParameter);
        //             dispatch(actions.GetSelectedPreAssignment(response.result[0]));
        //             dispatch(compareARSResourcesWithPreAssignment(response.result[0]));
        //         }
        //         else{
        //             dispatch(actions.GetSelectedPreAssignment({}));
        //             IntertekToaster(localConstant.errorMessages.FETCH_PRE_ASSIGNMENT_FAILED, 'dangerToast preAssignmentErr');
        //         }
        //     }
        //     else if (response.code === "41") {
        //         if (!isEmptyReturnDefault(response.validationMessages)) {
        //             //To-Do: Common approach to display validationMessages
        //         }
        //     }
        //     else if (response.code === "11") {
        //         if (!isEmptyReturnDefault(response.messages)) {
        //             //To-Do: Common approach to display messages
        //         }
        //     }
        //     else {

        //     }
        //     dispatch(HideLoader());
        // }
        // else{
        //     dispatch(HideLoader());
        // }
    }
    else {
        dispatch(actions.GetSelectedPreAssignment({}));
        dispatch(actions.compareARSResourcesWithPreAssignment(true));
        dispatch(selectPreAssignmentTechSpec({}));
        dispatch(HideLoader());
    }
};

export const compareARSResourcesWithPreAssignment = (data) => (dispatch, getstate) => {
    const state = getstate();
    const arsDetils = state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails;
    const arsSearchParameter = Object.assign({}, arsDetils.searchParameter);
    const techSpecList = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.techspecList);
    if (data) {
        const searchParameter = Object.assign({}, data.searchParameter);
        const unmatchedResources = [];
        let preAssignmentResources = isEmptyReturnDefault(searchParameter.selectedTechSpecInfo);
        if (searchParameter.subSupplierInfos && searchParameter.subSupplierInfos.length > 0) {
            searchParameter.subSupplierInfos.forEach(x => {
                const subSupplierTS = isEmptyReturnDefault(x.selectedSubSupplierTS);
                preAssignmentResources = [ ...preAssignmentResources, ...subSupplierTS ];
            });
        }
        if (preAssignmentResources.length > 0) {
            if (techSpecList.length > 0) {
                preAssignmentResources.forEach(x => {
                    let isResourceFound = false;
                    for (let i = 0; i < techSpecList.length; i++) {
                        const resources = isEmptyReturnDefault(techSpecList[i].resourceSearchTechspecInfos);
                        const result = resources.find(val => val.epin === x.epin);
                        if (result) {
                            isResourceFound = true;
                            break;
                        }
                    }
                    if (!isResourceFound) {
                        !(unmatchedResources.find(y => y.epin === x.epin)) && unmatchedResources.push(x);
                    }
                });
                if (unmatchedResources.length > 0) {
                    dispatch(actions.compareARSResourcesWithPreAssignment(false));
                    dispatch(actions.unmatchedResources(unmatchedResources));
                }
            } else {
                dispatch(actions.compareARSResourcesWithPreAssignment(false));
                dispatch(actions.unmatchedResources(preAssignmentResources));
            }
        } else {
            dispatch(actions.compareARSResourcesWithPreAssignment(true));
        }
        dispatch(selectPreAssignmentTechSpec(searchParameter));
    } else {
        dispatch(actions.compareARSResourcesWithPreAssignment(true));
    }
};

/** Load Operational Managers for Override - Preferred Resources - ARS */
export const FetchOperationalManagers = (data) => async (dispatch, getstate) => {
    const selectedCompany = data;
    const coordinatorUrl = masterData.user;
    const param = {
        companyCode: selectedCompany,
        userType: 'OperationManager',
        isActive: true
    };
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(coordinatorUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            //IntertekToaster(localConstant.errorMessages.FETCH_OPERATIONAL_MANAGERS_FAILED, 'wariningToast operationalManagersErr');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchOperationalManagers(response.result));
    }
    else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast oprationalManagersErr');
    }
    else {
        IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast oprationalManagersErr');
    }
};

/** Load Operational Managers for Override - Preferred Resources - ARS */
export const FetchTechnicalSpecialists = (data) => async (dispatch, getstate) => {
    const selectedCompany = data;
    const coordinatorUrl = GrmAPIConfig.technicalSpecialists + GrmAPIConfig.search;
    const param = {
        companyCode: selectedCompany,
        profileStatus: "Active"
    };
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(coordinatorUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
           // IntertekToaster(localConstant.errorMessages.FETCH_TECHNICAL_SPECIALIST_FAILED, 'dangerToast techSpecFetchErr');
           IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchTechnicalSpecialists(response.result));
    }
    else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast oprationalManagersErr');
    }
    else {
        IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast techSpecFetchErr');
    }
};

/** ARS Search Save */
export const SaveARSSearch = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    const currentPage = state.CommonReducer.currentPage;
    const ARSDetails = Object.assign({}, state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails);
    ARSDetails.recordStatus = 'N';
    ARSDetails.searchType = "ARS";
    const postUrl = GrmAPIConfig.ARS;
    const requestPayload = new RequestPayload(ARSDetails);
    const response = await PostData(postUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            //IntertekToaster(localConstant.errorMessages.ARS_SEARCH_SAVE_ERROR, 'dangerToast ARSSaveErr');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (response) {
        if (response.code === '1') {
            dispatch(AssignResourcesButtonClick(false));
            if (currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE || currentPage === localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE) {
                dispatch(FetchAssignmentDetail());
            }
            else {
                dispatch(HideLoader());
            }
            return true;
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast ArsSearch');
            dispatch(HideLoader());
            return false;
        }
        else {
            dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "ArsSearch"));
            dispatch(HideLoader());
            return false;
        }
    }
    else {
        dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "ArsSearch"));
        dispatch(HideLoader());
        return false;
    }
};

/** ARS Search Update */
export const UpdateARSSearch = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    const currentPage = state.CommonReducer.currentPage;
    const selectedMyTask = isEmptyReturnDefault(state.rootAssignmentReducer.selectedARSMyTask, 'object');
    const overridenResources = isEmptyReturnDefault(state.rootAssignmentReducer.overridenResources);
    const ARSDetails = Object.assign({}, state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails);
    if (selectedMyTask.taskType !== "OM Verify and Validate") {
        ARSDetails.overridenPreferredResources = overridenResources.concat(isEmptyReturnDefault(ARSDetails.overridenPreferredResources));
    }
    ARSDetails.recordStatus = 'M';
    ARSDetails.searchType = "ARS";
    const postUrl = GrmAPIConfig.ARS;
    const requestPayload = new RequestPayload(ARSDetails);
    const response = await CreateData(postUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            //IntertekToaster(localConstant.errorMessages.ARS_SEARCH_UPDATE_ERROR, 'dangerToast ARSSaveErr');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (response) {
        if (response.code === '1') {
            dispatch(AssignResourcesButtonClick(false));
            if (currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE || currentPage === localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE) {
                dispatch(FetchAssignmentDetail());
            }
            else {
                dispatch(HideLoader());
            }
            return true;
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast ArsSearch');
            dispatch(HideLoader());
            return false;
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast ArsSearch');
            dispatch(HideLoader());
            return false;
        }
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast ArsSearch');
        dispatch(HideLoader());
        return false;
    }
};

/** ARS Search Get from dashboard */
export const GetMyTaskARSSearch = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    if (data) {
        dispatch(selectedARSMyTask(data));
        const apiUrl = GrmAPIConfig.ARS;
        const param = {
            "id": data.taskRefCode,
            "searchType": 'ARS'
        };
        const requestPayload = new RequestPayload(param);
        const response = await FetchData(apiUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                //IntertekToaster(localConstant.errorMessages.FETCH_ARS_SEARCH_FAILED, 'dangerToast ARSSearchGetErr');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                dispatch(HideLoader());
            });
        if (response) {
            //To-do Search Result Grid Bind
            if (response.code === "1") {
                if (!isEmpty(response.result)) {
                    response.result[0].searchParameter = JSON.parse(response.result[0].searchParameter);
                    const myTaskData = { ...response.result[0] };
                    const preAssignmentSearchData = { ...response.result[0].searchParameter };

                    // To fetch coordinator details
                    dispatch(FetchARSCoordinatorDetails(preAssignmentSearchData));
                    
                    if (data.taskType === "PLO to RC") {
                        const ploTaxonomy = {
                            "categoryName": preAssignmentSearchData.categoryName,
                            "subCategoryName": preAssignmentSearchData.subCategoryName,
                            "serviceName": preAssignmentSearchData.serviceName
                        };
                        preAssignmentSearchData.ploTaxonomyInfo = ploTaxonomy;
                        response.result[0].searchParameter.ploTaxonomyInfo = ploTaxonomy;
                        dispatch(FetchDispositionType("PLO"));
                    }
                    if (localConstant.resourceSearch.ploTaskStatus.includes(data.taskType)) {
                        dispatch(LoadPreAssignmentMaster(preAssignmentSearchData));
                        dispatch(LoadPLOTaxonomy(preAssignmentSearchData.ploTaxonomyInfo));
                    }
                    dispatch(FetchAssignmentStatus());
                    myTaskData.searchParameter = preAssignmentSearchData;
                    myTaskData.taskType = data.taskType;
                    dispatch(searchPreAssignmentTechSpec(myTaskData));
                    if (data.taskType === "OM Verify and Validate") {
                        response.result[0].searchAction = "ARR";
                    }
                    else {
                        response.result[0].searchAction = "";
                    }
                    response.result[0].description = "";
                    dispatch(UpdateActionDetails(response.result[0]));
                    dispatch(ARSSearchPanelStatus(true));
                }
                else {
                    dispatch(UpdateActionDetails({}));
                    IntertekToaster(localConstant.errorMessages.FETCH_ARS_SEARCH_FAILED, 'dangerToast ARSSearchGetErr');
                }
            }
            else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast ArsSearch');
            }
            else {
                dispatch(ValidationAlert(localConstant.errorMessages.SOMETHING_WENT_WRONG, "ArsSearch"));
            }
            dispatch(HideLoader());
        }
        else {
            dispatch(HideLoader());
        }
    }
    else {
        dispatch(HideLoader());
    }
};

/**
 * call action to fetch ch and oc coordinator info for ARS data binding
 */
export const FetchARSCoordinatorDetails = (data) =>async (dispatch,getstate) => {
    const chCoordinatorInfo = await dispatch(FetchCoordinatorInfo(data.chCoordinatorLogOnName ? { "LogonName":data.chCoordinatorLogOnName } : ""));
    const opCoordinatorInfo = await dispatch(FetchCoordinatorInfo(data.opCoordinatorLogOnName ? { "LogonName":data.opCoordinatorLogOnName } : ""));
    if(chCoordinatorInfo && chCoordinatorInfo.length > 0){
        dispatch(actions.ARSCHCoordinatorInfo(chCoordinatorInfo[0]));
    }
    if(opCoordinatorInfo && opCoordinatorInfo.length > 0){
        dispatch(actions.ARSOPCoordinatorInfo(opCoordinatorInfo[0]));
    }
};

/** Fetching coordinator info for ARS data binding */
export const FetchCoordinatorInfo = (data) =>async () => {
    const fetchUrl = userAPIConfig.user;
    const params = isEmptyReturnDefault(data,'object');
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(fetchUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast conActDataSomethingWrong');
            return false;
        });
    if(response){
        if(response.code == "1"){
            return response.result;
        }
        else{
            return false;
        }
    }
    else{
        return false;
    }
};

export const selectedARSMyTask = (data) => (dispatch, getstate) => {
    dispatch(actions.selectedARSMyTask(data));
};

export const clearARSSearchDetails = () => (dispatch, getstate) => {
    dispatch(actions.compareARSResourcesWithPreAssignment(true));
    dispatch(actions.GetSelectedPreAssignment({}));
    dispatch(actions.FetchPreAssignmentIds([]));
    dispatch(actions.OverridenResources([]));
    dispatch(actions.clearARSSearchDetails());
    dispatch(actions.selectedARSMyTask({}));
};

export const ApproveRejectOverride = (data, value) => (dispatch, getstate) => {
    const state = getstate();
    if (data) {
        const overrideResources = Object.assign([], state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails.overridenPreferredResources);
        overrideResources.forEach(iteratedValue => {
            if (iteratedValue.techSpecialist) {
                if (iteratedValue.techSpecialist.epin === data.techSpecialist.epin && iteratedValue.id === data.id) {
                    if (!required(value))
                        iteratedValue.isApproved = (value === "Approve") ? true : false;
                    else
                        iteratedValue.isApproved = null;
                    if (iteratedValue.recordStatus !== "N") {
                        iteratedValue.recordStatus = "M";
                    }
                }
            }
        });
        const updatedData = {};
        updatedData.overridenPreferredResources = overrideResources;
        dispatch(UpdateActionDetails(updatedData));
    }
};

/** Fetch PLO Taxonomy sub-catogory list */
export const FetchPLOTechSpecSubCategory = (data) => async (dispatch, getstate) => {
    //const Url = masterData.baseUrl + masterData.currencies;
    const Url = masterData.baseUrl + masterData.taxonomySubCategory;
    const params = { 'taxonomyCategory': data };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            //IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TECH_DICIPLINE_SUBCATEGORY, 'dangerToast TechSpecSubCategory');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });

    if (response) {
        if (response.code === "1") {
            dispatch(actions.FetchPLOTechSpecSubCategory(response.result));
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast TechSpecSubCategory');
        }
        else {
            IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TECH_DICIPLINE_SUBCATEGORY, 'dangerToast TechSpecSubCategory');
        }
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TECH_DICIPLINE_SUBCATEGORY, 'dangerToast TechSpecSubCategory');
    }
};

/** Fetch PLO Taxonomy services list */
export const FetchPLOTechSpecServices = (data) => async (dispatch, getstate) => {
    //const Url = masterData.baseUrl + masterData.currencies;
    const Url = masterData.baseUrl + masterData.taxonomyServices;
    const params = { 'taxonomySubCategory': data };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            //IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TECH_DICIPLINE_SERVICES, 'dangerToast TechSpecServices');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchPLOTechSpecServices(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TECH_DICIPLINE_SERVICES, 'dangerToast TechSpecServices');
    }
};

/** Clear PLO Taxonomy sub-category */
export const ClearPLOSubCategory = () => (dispatch) => {
    dispatch(actions.ClearPLOSubCategory());
};
/** Clear PLO Taxonomy services */
export const ClearPLOServices = () => (dispatch) => {
    dispatch(actions.ClearPLOServices());
};

/** Load PLO Taxonomy data */
export const LoadPLOTaxonomy = (data) => (dispatch, getstate) => {
    if (data) {
        if (data.categoryName) {
            dispatch(ClearPLOSubCategory());
            dispatch(ClearPLOServices());
            dispatch(FetchPLOTechSpecSubCategory(data.categoryName));
        }
        if (data.subCategoryName) {
            dispatch(ClearPLOServices());
            dispatch(FetchPLOTechSpecServices(data.subCategoryName));
        }
    }
};

export const OverridenResources = (data) => (dispatch, getstate) => {
    dispatch(actions.OverridenResources(data));
};

export const GetAssigedResourcesForARS = (data) => async (dispatch, getstate) => {
    const state = getstate();
    if (data) {
        const apiUrl = GrmAPIConfig.arsSearch;
        const param = {
            "assignmentId": data.assignmentId
        };
        const requestPayload = new RequestPayload(param);
        const response = await FetchData(apiUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                //IntertekToaster(localConstant.errorMessages.FETCH_ARS_SEARCH_FAILED, 'dangerToast getAssigneResourecteErr');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response) {
            //To-do Search Result Grid Bind
            if (response.code === "1") {
                if (!isEmpty(response.result)) {
                    const searchParam = isEmptyReturnDefault(response.result.searchParameter, 'object');
                    const assignedResources = isEmptyReturnDefault(searchParam.assignedResourceInfos);
                    dispatch(UpdatePreAssignmentSearchParam({ assignedResourceInfos: assignedResources }));
                }
            }
            else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast getAssigneResourecteErr');
            }
            else {
                IntertekToaster(localConstant.errorMessages.FETCH_ARS_SEARCH_FAILED, `dangerToast getAssigneResourecteErr`);
            }
        }
        else {
            IntertekToaster(localConstant.errorMessages.FETCH_ARS_SEARCH_FAILED, `dangerToast getAssigneResourecteErr`);
        }
    }
    else {
        IntertekToaster(localConstant.errorMessages.FETCH_ARS_SEARCH_FAILED, `dangerToast getAssigneResourecteErr`);
    }
};

/** Get ARS search Geo location information */
export const GetGeoLocationInfo = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    let searchParameter = {};
    if (data) {
        searchParameter = data;
    }
    else {
        searchParameter = Object.keys(state.RootTechSpecReducer.TechSpecDetailReducer.techspecList).map(function (i) {
            return state.RootTechSpecReducer.TechSpecDetailReducer.techspecList[i];
        });
    }
    const tsGeoLocationSearchUrl = GrmAPIConfig.preAssignmentGeoLocation;
    const requestPayload = new RequestPayload(searchParameter);
    const response = await PostData(tsGeoLocationSearchUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            //IntertekToaster(localConstant.validationMessage.FAILED_FETCH_GEO_LOCATION, 'dangerToast techspecErrMsg');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (!isEmpty(response)) {
        if (response.code === '1') {
            if (response.result && response.result.length > 0) {
                dispatch(actions.searchPreAssignmentTechSpec(response.result));
            }
            else {
                dispatch(actions.searchPreAssignmentTechSpec([]));
            }
            return true;
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast techspecErrMsg1');
        }
        else {
            IntertekToaster(localConstant.validationMessage.FAILED_FETCH_GEO_LOCATION, 'dangerToast techspecErrMsg1');
        }
        dispatch(HideLoader());
    }
    else {
        IntertekToaster(localConstant.validationMessage.FAILED_FETCH_GEO_LOCATION, 'dangerToast techspecErrMsg1');
        dispatch(HideLoader());
    }
    return false;
};

export const SetTechSpecToAssignmentSubSupplier = (data, assignmentSubsupplierIndex) => (dispatch, getstate) => {
    const state = getstate();
    const assignmentSubSupplier = deepCopy(state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers);
    // const subsupplierIndx=assignmentSubSupplier.findIndex(value => value.subSupplierId === eachObj.subSupplierId);
    const assignmentSupsupplierTechSpec = isEmptyReturnDefault(deepCopy(state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers[assignmentSubsupplierIndex].assignmentSubSupplierTS));

    for (let i = 0, len = data.length; i < len; i++) {
        const eachObj = data[i], newTSObj = {
            assignmentSubSupplierTSId: null,
            assignmentSubSupplierId: eachObj.assignmentSubSupplierId,
            epin: eachObj.epin,
            recordStatus: 'N',
            isAssignedToThisSubSupplier: eachObj.isAssignedToThisSubSupplier //MS-TS Link CR
        };
        const techSpecIndex = assignmentSupsupplierTechSpec.findIndex(techSpec => techSpec.epin === eachObj.epin && techSpec.isAssignedToThisSubSupplier === eachObj.isAssignedToThisSubSupplier && techSpec.recordStatus !== 'D' && techSpec.isDeleted != true);//MS-TS
        if (techSpecIndex === -1) {
            assignmentSupsupplierTechSpec.push(newTSObj);
        }
    }
       
    assignmentSubSupplier[assignmentSubsupplierIndex].assignmentSubSupplierTS = assignmentSupsupplierTechSpec;
    if (assignmentSubSupplier[assignmentSubsupplierIndex].recordStatus !== 'N') {
        assignmentSubSupplier[assignmentSubsupplierIndex].recordStatus = 'M';
    }
    dispatch(actions.AddSupSupplierTechSpec(assignmentSubSupplier));
};

/**
 * This action is to fetch Comments History Report of ARS flow.
 * If assignmentId is null or empty, then it will clear the report to empty array
 * @param {* int} assignmentId 
 */
export const FetchARSCommentsHistoryReport = (assignmentId) => async (dispatch) => {
    if(assignmentId) {
        const Url = GrmAPIConfig.arsResourceNotes;
        const params = { 'assignmentId':assignmentId };
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(Url, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast FetchARSCommentsHistoryReport');
            });
            
        if (response && response.code) {
            if (response.code === "1") {
                dispatch(actions.FetchARSCommentsHistoryReport(response.result));
            }
            else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast FetchARSCommentsHistoryReport');
            }
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast FetchARSCommentsHistoryReport');
        }
    }
    else{
        dispatch(actions.FetchARSCommentsHistoryReport([]));
    }
};