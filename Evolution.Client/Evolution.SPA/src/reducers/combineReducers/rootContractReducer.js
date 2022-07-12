import { combineReducers } from 'redux';
import reduceReducers from 'reduce-reducers';
import {
    RateScheduleReducer
} from '../../reducers/contracts/rateScheduleReducer';
import {
    GeneralDetailReducer
} from '../../reducers/contracts/generalDetailsReducer';
import {
    ContractReducer
} from '../../reducers/contracts/contractReducer';

import { ContractInvoicingDefaults } from '../../reducers/contracts/invoicingDefaultsReducer';
import { FixedExchangeRatesReducer } from '../../reducers/contracts/fixedExchangeRatesReducer';
import { ContractSearchReducer } from '../../reducers/contracts/contractSearchReducer';
import { DocumentReducer } from '../../reducers/contracts/documentReducer';
import { ContractNoteReducer } from '../../reducers/contracts/contractNoteReducer';
import { ChildContractReducer } from '../../reducers/contracts/childContractReducer';
const initialState = {
    contractDetailTabs:[
        {   
        tabHeader: "General Details",
            tabBody: "GeneralDetails",
            tabActive:true,
            tabDisableStatus:[]    
           
          },
          {
            tabHeader: "Invoicing Defaults",
            tabBody: "InvoicingDefaults",
            tabActive:true,
            tabDisableStatus:[ 'FRW' ]   
          },
          {
            tabHeader: "Fixed Exchange Rates",
            tabBody: "FixedExchangeRates",
            tabActive:true,
            tabDisableStatus:[]   
          },
          {
            tabHeader: "Rate Schedules",
            tabBody: "RateSchedule",
            tabActive:true,
            tabDisableStatus:[]  
          },
          {
            tabHeader: "Documents",
            tabBody: "Documents",
            tabActive:true,
            tabDisableStatus:[] 
          },
          {
            tabHeader: "Child Contracts",
            tabBody: "ChildContracts",
            tabActive:false,
            tabDisableStatus:[ '/CreateContract' ]   
          },
          {
            tabHeader: "Projects",
            tabBody: "Project",
            tabActive:true,
            tabDisableStatus:[ '/CreateContract' ]    
          },
          {
            tabHeader: "Notes",
            tabBody: "Notes",
            tabActive:true,
            tabDisableStatus:[]    
          }
    ],
    contractDetail: {},
    currentPage:'',
    currentUpdatedData:{},
    selectedCustomerData:{},
    isPanelOpen:true,
    //isShowModal:false,
    customerList:[],
    customerContract:[],   
    contractStatus:'',
    contractNo:'',
    /**Child Contracts */
    childContractsOfParent:[],
    selectedContractStatus:'O',
    
    customerContractNo: '',
    contractHoldingCompany:'', 
    isbtnDisable:true,
    /** Invoicing Defaults */
    isInvoiceReferenceModalOpen: false,
    isInvoiceReferenceEdit: false,
    isInvoiceAttachmentTypesModalOpen: false,
    isInvoiceAttachmentTypesEdit: false,
    /** Invoicing Defaults */
 
    /**Documents */
    copyDocumentDetails:[],
    isShowDocumentModal:false,
    isDocumentModelAddView:true,
    documentTypeData:[],
    contractCustomerdocumentsData:[],
    contractProjectdocumentsData:[],
    parentContractDocumentsData:[],
    documentDetailsInfo:{
        documentType:'',
        isVisibleToCustomer:false,
        isVisibleToTS:false,
        isOutOfCompanyVisible:false,
    },
    editContractDocumentDetails:{
        contractDocumentId: '',
        documentSize: '',
        documentType: '',
        isOutOfCompanyVisible: false,
        isVisibleToCustomer: false,
        isVisibleToTS: false,
        modifiedBy: '',
        name: '',
        recordStatus:'',
        uploadDataId:'',
        uploadedOn:''     
      
    }, 
   
    documentTypesEdit:false,
    isEditContractDocumentDetails:false,
    /**Documents */

    /** Rate Schedule */
    isRateScheduleEdit:false,
    isChargeTypeEdit:false,
    isRateScheduleOpen:false,
    isChargeTypeOpen:false,
    isCopyChargeTypeOpen:false,
    isAdminContractRatesOpen:false,
    rateScheduleEditData:{},
    chargeTypeEditData:{},
    selectedRowIndex:0,
    adminRateToCopy:[],
    adminChargeRatesValue:[ {  isRateOnShoreOil:false,
      isRateOnShoreNonOil:false,
      isRateOffShoreOil:false,
      id:-1 } ],
    /** Rate Schedule */

    /** General Details */
    CustomerCodeInCRM: "",
    /** General Details */

    /** Fixed Exchange Rate */
    showButton: false,
    ContractFixedRate: [],
    editFixedExchangeDetails: {},
    chechBoxHideButton:true,
    isExchangeRateEdit: false,
    isExchangeRateModalOpen: false,
    isExchangeEdit: false,
    addFixedData: {},
    currencyData: [],
    /** Fixed Exchange Rate */
};

const ContractDetailReducer = reduceReducers(
    RateScheduleReducer, 
    GeneralDetailReducer,
    DocumentReducer,
    ContractReducer,
    ContractInvoicingDefaults,
    FixedExchangeRatesReducer,
    ContractSearchReducer,
    ContractNoteReducer,
    ChildContractReducer,
    initialState
    );
/*
 * We combine all reducers into a single object before updated data is dispatched (sent) to store
 * Need to get the combined reducer
 * our entire applications state (store) is just whatever gets returned from all your reducers
 * */
export default combineReducers({
    ContractDetailReducer
});