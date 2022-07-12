import { combineReducers } from 'redux';
import { dashboardReducer } from './components/viewComponents/dashboard/dashboardReducer';
import { appLayoutReducer } from './components/appLayout/appLayoutReducers';
import { CompanyReducer } from './components/viewComponents/company/companyReducer';
import { CustomerReducer } from './components/viewComponents/customer/customerReducer';
import { CustomModalReducer } from './common/baseComponents/customModal/customModalReducer';
import { ModalReducer } from './common/baseComponents/modal/modalReducer';
import { UploadDocumentReducer } from './common/baseComponents/uploadDocument/uploadDocumentReducer';
import { loginReducer } from './reducers/login/loginReducer';
import{ forgotPasswordReducer } from './reducers/login/forgotPasswordReducer';
import { headerReducer } from './components/header/headerReducers';
import masterDataReducer from './common/masterData/masterDataReducer';
import RootContractReducer from './reducers/combineReducers/rootContractReducer';
import { CustomerAndCountrySearch } from './components/applicationComponents/customerAndCountrySearch/customerAndCountrySearchReducer';
//import { DocumentReducer } from './reducers/contracts/documentReducer';
import { ContractProjectReducer } from './reducers/contracts/contractProjectReducer';
import RootTechSpecReducer from './grm/combineReducers/rootTechSpecReducer';
import RootProjectReducer from './reducers/combineReducers/rootProjectReducer';
import { CommonReducer } from './common/commonReducer';
import { loginActionTypes } from './constants/actionTypes';
import rootAssignmentReducer from './reducers/combineReducers/rootAssignmentReducer';
//Added for Supplier
import RootSupplierReducer from './reducers/combineReducers/rootSupplierReducer';
//Added for Supplier PO 
import rootSupplierPOReducer from './reducers/combineReducers/rootSupplierPOReducer';

//Added for Timesheet
import rootTimesheetReducer from './reducers/combineReducers/rootTimesheetReducer';
//Added for Visit
import rootVisitReducer from './reducers/combineReducers/rootVisitReducer';
//Added for ViewRole
import rootAdminReducer from './reducers/combineReducers/rootAdminReducer';
import { documentationReducer }  from './reducers/dashboard/documentationReducer';
//Security Reducer
import rootSecurityReducer from './reducers/combineReducers/rootSecurityReducer';
import { auditSearchReducer } from './reducers/auditSearch/auditSearchReducer';

//Reports Reducer
import rootReportReducer from './reducers/combineReducers/rootReportReducer';

//Batch Process Reducer
import { batchProcessReducer } from './reducers/batchProcess/batchProcessReducer';

import { persistReducer } from 'redux-persist';
import storage from 'redux-persist/lib/storage'; // defaults to localStorage
//configuration to persisi reducer
// we can also specify the specific properties to persist 
const persistMasterDataConfig = {
    key: 'evolution2PersistMasterData',
    storage,
  };

// only company will be persisted configuration
const persistAppLayoutConfig = {
    key: 'evolution2PersistAppLayoutData',
    storage: storage,
    whitelist: [ 'companyList','annoncementData' ] // only companyList will be persisted
  };

/*
 * We combine all reducers into a single object before updated data is dispatched (sent) to store
 * Need to get the combined reducer
 * our entire applications state (store) is just whatever gets returned from all your reducers
 * */
const appReducers = combineReducers({
    appLayoutReducer: persistReducer(persistAppLayoutConfig, appLayoutReducer),
    dashboardReducer,
    CompanyReducer,
    CustomerReducer,    
    CustomModalReducer,
    loginReducer,
    headerReducer,
    ModalReducer,
    masterDataReducer: persistReducer(persistMasterDataConfig, masterDataReducer), //persist only masterdata reducer
    RootContractReducer,
    RootProjectReducer,
    RootTechSpecReducer,
    rootTimesheetReducer,
    ContractProjectReducer,
    CommonReducer,
    CustomerAndCountrySearch,
    rootAssignmentReducer,
    RootSupplierReducer,
    rootSupplierPOReducer,
    rootVisitReducer,
    UploadDocumentReducer,
    rootAdminReducer,
    rootSecurityReducer,
    documentationReducer,
    forgotPasswordReducer,
    auditSearchReducer,
    batchProcessReducer,
     //added for Evolution Reports 
     rootReportReducer
});

export const rootReducer = (state, action) =>{
    if(action.type === loginActionTypes.RESET_STORE){
        const { loginReducer } = state;
        state = { loginReducer };
    }
    return appReducers(state, action);
};