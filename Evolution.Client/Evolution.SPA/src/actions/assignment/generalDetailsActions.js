import { RequestPayload, masterData, contractAPIConfig, assignmentAPIConfig } from '../../apiConfig/apiConfig';
import { assignmentsActionTypes } from '../../constants/actionTypes';
import { FetchData,PostData } from '../../services/api/baseApiService';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { isEmpty, getlocalizeData, getNestedObject, parseValdiationMessage, isEmptyReturnDefault } from '../../utils/commonUtils';
import { ShowLoader, HideLoader } from '../../common/commonAction';
import { isVisit, isTimesheet } from '../../selectors/assignmentSelector';
import { FetchMainSupplierName } from './supplierInformationAction';
import { required } from '../../utils/validator';
import arrayUtil from '../../utils/arrayUtil';
import { processMICoordinators } from '../../utils/commonUtils';

const localConstant = getlocalizeData();
const actions = {
    FetchCoordinatorForContractHoldingCompany: (payload) => ({
        type: assignmentsActionTypes.FETCH_COORDINATOR_FOR_CONTRACT_HOLDING_COMPANY,
        data: payload
    }),
    FetchCoordinatorForOperatingCompany: (payload) => ({
        type: assignmentsActionTypes.FETCH_COORDINATOR_FOR_OPERATING_COMPANY,
        data: payload
    }),
    FetchCustomerAssignmentContact: (payload) => ({
        type: assignmentsActionTypes.FETCH_CUSTOMER_ASSIGNMENT_CONTACT,
        data: payload
    }),
    FetchAssignmentCompanyAddress: (payload) => ({
        type: assignmentsActionTypes.FETCH_ASSIGNMENT_COMPANY_ADDRESS,
        data: payload
    }),
    FetchSupplierPO: (payload) => ({
        type: assignmentsActionTypes.FETCH_SUPPLIER_PO,
        data: payload
    }),
    UpdateAssignmentGeneralDetails: (payload) => ({
        type: assignmentsActionTypes.UPDATE_ASSIGNMENT_GENERAL_DETAILS,
        data: payload
    }),
    FetchTimesheetState: (payload) => ({
        type: assignmentsActionTypes.FETCH_ASSIGNMENT_TIMESHEET_STATE,
        data: payload
    }),
    FetchTimesheetCity: (payload) => ({
        type: assignmentsActionTypes.FETCH_ASSIGNMENT_TIMESHEET_CITY,
        data: payload
    }),
};

/**
 * 
 * @param {*} data based on which need to fetch assignment related lookups data
 * @param {*} isFetchLookValues this param will decide to load the module specific data like dropdown, references etc 
 * isFetchLookValues value will be set as false on cancel and save no need to call again if relevant data exists
 */
export const FetchGeneralDetails = (data, isFetchLookValues) => (dispatch, getstate) => {
    const state = getstate();
    const operatingCompany = (data && data.assignmentOperatingCompanyCode) ? [ data.assignmentOperatingCompanyCode ] : [];
    const contractHoldingCompany = (data && data.assignmentContractHoldingCompanyCode) ? [ data.assignmentContractHoldingCompanyCode ] : [];
    const isAddAssignment = state.CommonReducer.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE;
    //if user click on cancel while creating new assignment 
    //Need to fetch operating company's coordinators to reset the actual company's coordinators
    //because user may chane the operating company such that company's coordinators are fetched 
    //On Edit Assignment both operationg company and contractholding company will be disabled
    if (isAddAssignment && !isFetchLookValues) {
        dispatch(FetchCoordinatorForOperatingCompany(operatingCompany));
    }
    if (isFetchLookValues) {
        dispatch(FetchCoordinatorForOperatingCompany(operatingCompany));
        dispatch(FetchCoordinatorForContractHoldingCompany(contractHoldingCompany));
        //contact & address are fetched based on customer code so we can fetch only once per assignment
        dispatch(FetchCustomerAssignmentContact(data.assignmentCustomerCode));
        dispatch(FetchAssignmentCompanyAddress(data.assignmentCustomerCode));

        //Supplier PO Info is only required for visit assignment so below check is added
        const assignmentProjectWorkFlow = getNestedObject(state.rootAssignmentReducer.assignmentDetail,
            [ "AssignmentInfo", "assignmentProjectWorkFlow" ]);
        const assignmentSupplierPOId = getNestedObject(state.rootAssignmentReducer.assignmentDetail,[ "AssignmentInfo","assignmentSupplierPurchaseOrderId" ]);
        const workflowType = {
            workflowTypeParams: localConstant.commonConstants.workFlow,
            workflowType: assignmentProjectWorkFlow && assignmentProjectWorkFlow.trim()
        };
        if (isVisit(workflowType)) {
            dispatch(FetchSupplierPO(data.assignmentProjectNumber,assignmentSupplierPOId));
        }
        if(isTimesheet(workflowType)) {
            dispatch(timesheetWorkLocationMasterData(data));
        }
    }
};

/** Fetch Coordinator for Contract Holding Company action 
 * Changes: Changing this action to fetch MICoordinator for the Contract Holding Company
 * data - Array of company codes.
*/
export const FetchCoordinatorForContractHoldingCompany = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    dispatch(actions.FetchCoordinatorForContractHoldingCompany([]));
    if(data){
        const assignmentMICoordinatorUrl = masterData.miCoordinators;
        const param = data;
        const requestPayload = new RequestPayload(param);
        const response = await PostData(assignmentMICoordinatorUrl,requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION, 'warningToast HoldingCompanyCoordinatorVal');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                dispatch(HideLoader());
            });
        if (!isEmpty(response)) {
            if (response.code === "1") {
                const processedMICoordinators = processMICoordinators(response.result);
                dispatch(actions.FetchCoordinatorForContractHoldingCompany(arrayUtil.sort(processedMICoordinators,"displayName","asc")));
            }
            else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast HoldingCompanyCoordinatorVal');
            }
            else {
                IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION, 'warningToast HoldingCompanyCoordinatorVal');
            }            
        }
        else {
            IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast HoldingCompanyCoordinatorSWWVal');
        }
        dispatch(HideLoader());
    }
    dispatch(HideLoader());
};

/** Fetch Coordinator for Operating Company action 
 * Changes: Changing this action to fetch MICoordinator for the Operating Company
 * data - Array of company codes.
*/
export const FetchCoordinatorForOperatingCompany = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    dispatch(actions.FetchCoordinatorForOperatingCompany([]));
    if(data){
        const assignmentMICoordinatorUrl = masterData.miCoordinators;
        const param = data;
        const requestPayload = new RequestPayload(param);
        const response = await PostData(assignmentMICoordinatorUrl,requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION, 'warningToast operatingCoordinatorVal');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                dispatch(HideLoader());
            });
        if (!isEmpty(response)) {
            if (response.code === "1") {
                const processedMICoordinators = processMICoordinators(response.result);
                dispatch(actions.FetchCoordinatorForOperatingCompany(arrayUtil.sort(processedMICoordinators,"displayName","asc")));
            }
            else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast operatingCoordinatorVal');
            }
            else {
                IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION, 'warningToast operatingCoordinatorVal');    
            }
        }
        else {
            IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast operatingCoordinatorSWWVal');
        }
        dispatch(HideLoader());
    } 
    dispatch(HideLoader());
};

// Search Page
export const FetchCoordinatorForContractHoldingCompanyForSearch = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    dispatch(actions.FetchCoordinatorForContractHoldingCompany([]));
    if (data) {
        const assignmentMICoordinatorUrl = masterData.miCoordinatorsStatus;
        const param = data;
        const requestPayload = new RequestPayload(param);
        if (param) {
            const response = await PostData(assignmentMICoordinatorUrl, requestPayload)
                .catch(error => {
                    // console.error(error); // To show the error details in console
                    // IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION, 'warningToast HoldingCompanyCoordinatorVal');
                    IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                    dispatch(HideLoader());
                });
            if (!isEmpty(response)) {
                if (response.code === "1") {
                    const processedMICoordinators = processMICoordinators(response.result);
                    dispatch(actions.FetchCoordinatorForContractHoldingCompany(arrayUtil.sort(processedMICoordinators, "displayName", "asc")));
                }
                else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
                    IntertekToaster(parseValdiationMessage(response), 'dangerToast HoldingCompanyCoordinatorVal');
                }
                else {
                    IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION, 'warningToast HoldingCompanyCoordinatorVal');
                }
            }
            else {
                IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast HoldingCompanyCoordinatorSWWVal');
            }
        }
        else {
            dispatch(actions.FetchCoordinatorForContractHoldingCompany([]));
        }

        dispatch(HideLoader());
    }
    dispatch(HideLoader());
};

// Search Page
export const FetchCoordinatorForOperatingCompanyForSearch = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    dispatch(actions.FetchCoordinatorForOperatingCompany([]));
    if (data) {
        const assignmentMICoordinatorUrl = masterData.miCoordinatorsStatus;
        const param = data;
        const requestPayload = new RequestPayload(param);
        if (param) {
            const response = await PostData(assignmentMICoordinatorUrl, requestPayload)
                .catch(error => {
                    // console.error(error); // To show the error details in console
                    // IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION, 'warningToast operatingCoordinatorVal');
                    IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                    dispatch(HideLoader());
                });
            if (!isEmpty(response)) {
                if (response.code === "1") {
                    const processedMICoordinators = processMICoordinators(response.result);
                    dispatch(actions.FetchCoordinatorForOperatingCompany(arrayUtil.sort(processedMICoordinators, "displayName", "asc")));
                }
                else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
                    IntertekToaster(parseValdiationMessage(response), 'dangerToast operatingCoordinatorVal');
                }
                else {
                    IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION, 'warningToast operatingCoordinatorVal');
                }
            }
            else {
                IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast operatingCoordinatorSWWVal');
            }
        }
        else {
            dispatch(actions.FetchCoordinatorForOperatingCompany([]));
        }
        dispatch(HideLoader());
    }
    dispatch(HideLoader());
};

/**Customer Assignment Contacts */
export const FetchCustomerAssignmentContact = (data) => async (dispatch, getstate) => {
    const selectedCustomer = data;
    // const customerContractContact = contractAPIConfig.customers + selectedCustomer + contractAPIConfig.contacts;
    const customerContractContact = contractAPIConfig.customers + selectedCustomer + contractAPIConfig.contacts; // Optimization
    const response = await FetchData(customerContractContact, {})
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.CUSTOMER_CONTRACT_CONTACT_NOT_FETCHED, 'dangerToast GenDetActCustCntCont');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response)) {
        if (response.code === "1") {
            dispatch(actions.FetchCustomerAssignmentContact(response.result));
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast GenDetActCustCntCont');
        }
        else {
            IntertekToaster(localConstant.errorMessages.CUSTOMER_CONTRACT_CONTACT_NOT_FETCHED, 'dangerToast GenDetActCustCntCont');
        }
    }
    else {
        IntertekToaster(localConstant.errorMessages.CUSTOMER_CONTRACT_CONTACT_NOT_FETCHED, 'dangerToast GenDetActCustCntContErr');
    }
};

/** Assignment Company Address */
export const FetchAssignmentCompanyAddress = (data) => async (dispatch, getstate) => {
    const selectedCustomer = data;
    const customerContractAddress = contractAPIConfig.customers + selectedCustomer + contractAPIConfig.addresses;
    const response = await FetchData(customerContractAddress, {})
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.CUSTOMER_CONTRACTOR_ADDRESS_NOT_FETCHED, 'dangerToast GenDetActCustContAddress');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response)) {
        if (response.code === "1") {
            dispatch(actions.FetchAssignmentCompanyAddress(response.result));
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast GenDetActCustContAddress');
        }
        else {
            IntertekToaster(localConstant.errorMessages.CUSTOMER_CONTRACTOR_ADDRESS_NOT_FETCHED, 'dangerToast GenDetActCustContAddress');
        }
    }
    else {
        IntertekToaster(localConstant.errorMessages.CUSTOMER_CONTRACTOR_ADDRESS_NOT_FETCHED, 'dangerToast GenDetActCustContAddressErr');
    }
};

/** Supplier PO 
 *  defaultAssignmentSupplierPO for defaulting the supplier po related mainsupplier data
*/
export const FetchSupplierPO = (data,defaultAssignmentSupplierPOId) => async (dispatch, getstate) => {
    const selectedProject = data;
    // const supplierPOUrl = assignmentAPIConfig.supplierPO;
    const supplierPOUrl = assignmentAPIConfig.assignmentSupplierPOLOV; // Optimization
    const param = {
        SupplierPOProjectNumber: selectedProject
    };
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(supplierPOUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_SUPPLIER_PO, 'dangerToast GenDetActFetSuppPO');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response)) {
        if (response.code === "1") {
            dispatch(actions.FetchSupplierPO(response.result));
            if(!isEmpty(response.result) && defaultAssignmentSupplierPOId){
                const supplierPOlist = Object.assign([],response.result);
                const filteredSupplierPO = supplierPOlist.filter(x => x.supplierPOId == defaultAssignmentSupplierPOId);
                if(filteredSupplierPO && filteredSupplierPO.length > 0){
                    dispatch(FetchMainSupplierName(filteredSupplierPO[0]));
                }
            }
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast GenDetActFetSuppPO');
        }
        else {
            IntertekToaster(localConstant.errorMessages.FAILED_FETCH_SUPPLIER_PO, 'dangerToast GenDetActFetSuppPO');
        }
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_SUPPLIER_PO, 'dangerToast GenDetActFetSuppPOErr');
    }
};

/** General Details data storing in store */
export const UpdateAssignmentGeneralDetails = (data) => (dispatch, getstate) => {

    const state = getstate();
    const generalDetailInfo = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo, 'object');
    const assignmentOldLifeCycle = state.rootAssignmentReducer.AssignmentLifeCycleOld;
    const assignmentOldSupplierPO = state.rootAssignmentReducer.AssignmentSupplierPOOld;
    if(data){
        const updatedProjectInfo = Object.assign({}, generalDetailInfo, data);
        if (data.assignmentLifecycle) {
            const oldAssignmentLifeCycle = isEmpty(assignmentOldLifeCycle) ? generalDetailInfo.assignmentLifecycle : assignmentOldLifeCycle;
            updatedProjectInfo.AssignmentLifeCycleOld = oldAssignmentLifeCycle;   
        }
        if (data.assignmentSupplierPurchaseOrderNumber) {
            const oldAssignmentSupplierPO = isEmpty(assignmentOldSupplierPO) ? generalDetailInfo.assignmentSupplierPurchaseOrderNumber : assignmentOldSupplierPO;
            updatedProjectInfo.AssignmentSupplierPOOld = oldAssignmentSupplierPO;
        }
        dispatch(actions.UpdateAssignmentGeneralDetails(updatedProjectInfo));
    }
};

/** Timesheet Assignment Work Location Master Data Handling 
 * Param : data - AssignmentInfo Object
 * If data (i.e. Assignment Info) is not there, then it will clear the State and City from the reducer.
*/
export const timesheetWorkLocationMasterData = (data) => (dispatch) => {
    if(data){
        required(data.workLocationCountryId) ? dispatch(FetchTimesheetState()) : dispatch(FetchTimesheetState(data.workLocationCountryId));
        required(data.workLocationCountyId) ? dispatch(FetchTimesheetCity()) : dispatch(FetchTimesheetCity(data.workLocationCountyId));
    } else {
        dispatch(FetchTimesheetState());
        dispatch(FetchTimesheetCity());
    }
};

/** Timesheet Assignment Work Location State Action */
export const FetchTimesheetState = (data) => async (dispatch) => {
    if(data){
        const stateurl = masterData.baseUrl + masterData.state;
        const params = { 'countryId': data }; //Added for ITK D1536
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(stateurl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast FetchState');
            });
        if (response && response.code === "1") {
            dispatch(actions.FetchTimesheetState(response.result));
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast FetchState');
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast FetchState');
        }
    } else {
        dispatch(actions.FetchTimesheetState([]));
    }
};

/** Timesheet Assignment Work Location City Action */
export const FetchTimesheetCity = (data) => async (dispatch) => {
    if(data){
        const cityurl = masterData.baseUrl + masterData.city;
        const params = { 'stateId': data }; //Added for ITK D1536
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(cityurl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast FetchCity');
            });
        if (response && response.code === "1") {
            dispatch(actions.FetchTimesheetCity(response.result));
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast FetchCity');
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast FetchCity');
        }
    } else {
        dispatch(actions.FetchTimesheetCity([]));
    }
};