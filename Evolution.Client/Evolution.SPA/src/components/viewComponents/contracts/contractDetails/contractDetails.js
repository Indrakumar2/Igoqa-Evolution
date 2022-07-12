/* eslint-disable no-unused-expressions */
import React, { Component, Fragment } from 'react';
import moment from 'moment';
import CustomTabs from '../../../../common/baseComponents/customTab';
import dateUtil from '../../../../utils/dateUtil';
import { getlocalizeData, isEmpty,parseQueryParam, scrollToTop,ResetCurrentModuleTabInfo,isUndefined } from '../../../../utils/commonUtils';
import { modalTitleConstant, modalMessageConstant } from '../../../../constants/modalConstants';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { SaveBarWithCustomButtons } from '../../../applicationComponents/saveBar';
import Modal from '../../../../common/baseComponents/modal';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import ErrorList from '../../../../common/baseComponents/errorList';
import { activitycode } from '../../../../constants/securityConstant';
import { ButtonShowHide } from '../.././../../utils/permissionUtil';
import store from '../../../../store/reduxStore';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import { contractDetailTabs } from './contractTabDetails';
import { required, requiredNumeric } from '../../../../utils/validator';
import { StringFormat } from '../../../../utils/stringUtil';

const localConstant = getlocalizeData();

const HeaderData = {
  "columnDefs": [
    {
        "headerName": "Schedule Name",
        "field": "scheduleName",
        "filter": "agTextColumnFilter",
        "width":700,
        "headerTooltip": "Schedule Name",
        "tooltipField":"scheduleName"
    }
],
"enableFilter":false, 
"enableSorting":true,
"gridHeight":30,
"pagination": true,
"searchable":false,
"gridActions":false,
"gridTitlePanel":true,
};

const ScheduleValidationPopup = (props) => {
  return(
    <Fragment>
      <p className="bold confimationBodyText">Below mentioned schedules should have at least one Charge rate associated to it.</p>
      <ReactGrid gridRowData={props.state.scheduleList} gridColData={props.HeaderData} />
    </Fragment>
  );  
};

class ContractDetails extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isOpen: false,
      creatModeDisbale: false,
      isScheduleValidationModalOpen:false,
      selectedContractNumber:"",
      scheduleList:[],
      errorList:[]
    };
    this.confirmationModalData = {
      title: "",
      message: "",
      type: "",
      modalClassName: "",
      buttons: []
    };
    this.scheduleValidationButton = [
      {
          name: localConstant.commonConstants.OK,
          action: this.closeScheduleValidation,
          btnID: "closeScheduleValidation",
          btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
          showbtn: true
      }
    ];
    this.errorListButton =[
      {
        name: localConstant.commonConstants.OK,
        action: this.closeErrorList,
        btnID: "closeErrorList",
        btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
        showbtn: true
      }
    ];
    this.callBackFuncs ={      
      onCancelCharge:()=>{},      
      reloadLineItemsOnSave:()=>{},
      reloadLineItemsOnSaveValidation:()=>{},
      changeFocusForSave:()=>{}  
    }; 
    ResetCurrentModuleTabInfo(contractDetailTabs);
  }
  componentDidMount() {    
    this.props.actions.ClearData();
    sessionStorage.removeItem('selectedSchedule'); //Added for D789
    const result = this.props.location.search && parseQueryParam(this.props.location.search);
    //ITK - D661 & D662 - Menu Right Click open New Tab updating Curent Module name and Page Name.
    const deCodeResult={
      contractNumber:(result!== "" ? decodeURIComponent(result.contractNumber):null),
      selectedCompany:result.selectedCompany
    };
    if(isEmpty(this.props.contractDetails)){
      const data={
        currentModule: "Contract",
        currentPage: "createContract"
      };
      this.props.actions.CreateContract(data);
    }
    if(deCodeResult.contractNumber && isEmpty(this.props.contractDetails)){
      this.setState({ selectedContractNumber: deCodeResult.contractNumber });
      this.props.actions.GetSelectedContract(deCodeResult);
      this.props.actions.FetchContractData(deCodeResult, true) //Changes for D1039
      .then(response => {
        if (response && response.ContractInfo) {       
            this.props.actions.GetSelectedCustomerName(response.ContractInfo);
            if (response.ContractInfo && (response.ContractInfo.contractType === "PAR" || response.ContractInfo.contractType === "FRW")) {
                this.props.actions.FetchCompanyOffices();
            }
            if(response.ContractInfo.contractType === "FRW"){
              document.addEventListener("visibilitychange", this.onWindowFocus);
              this.props.actions.FetchBatchProcessData(response.ContractInfo.id);
            }
        }
    });
    }
    if (this.props.selectedContract && this.props.selectedContract.contractNumber) {
      this.props.actions.FetchContractData(this.props.selectedContract);
    }
  }
  
  onWindowFocus = () => {
    const link = document.querySelector("link[rel*='icon']") || document.createElement('link');
    link.type = 'image/x-icon';
    link.rel = 'shortcut icon';
    link.href = '/favicon.ico';
    document.getElementsByTagName('head')[0].appendChild(link);
  }

  componentWillUnmount() 
  { 
    this.props.actions.AdminContractRatesModalState(false);
    this.props.actions.ClearAdminChargeRates();
    this.props.actions.ClearAdminChargeScheduleValueChange();
    this.props.actions.ClearContractData();
    this.props.actions.ClearData();
    this.props.actions.FetchBatchProcessData();
  }
  closeScheduleValidation = (e) =>{
    e.preventDefault();
    this.setState({
      scheduleList:[],
      isScheduleValidationModalOpen:false
    });
  }

  closeErrorList =(e) =>{
    e.preventDefault();
    this.setState({
      errorList:[]
    });
  }

  contractSaveHandler = async () => {
    this.callBackFuncs.changeFocusForSave();
    const validDoc = this.uploadedDocumentCheck();
    const valid = this.mandatoryFieldsValidationCheck();    
    if (valid === true && validDoc) {
        if(!this.chargeRateOverlap()) {
          const res=await this.props.actions.SaveContractDetails();
          if(res)
          this.props.contractDetailTabs && this.props.contractDetailTabs.forEach(iteratedValue => {
            if(iteratedValue.tabBody === 'RateSchedule') this.callBackFuncs.reloadLineItemsOnSave();
          });  
        } else {
          const confirmationObject = {
            title: modalTitleConstant.ALERT_WARNING,
            message: modalMessageConstant.contract.CHARGE_RATES_OVERLAP,
            modalClassName: "warningToast",
            type: "confirm",       
            buttons: [
              {
                buttonName: "OK",
                className: "modal-close m-1 btn-small",  
                onClickHandler: this.chargeRateOverlapConfirmation
              }
            ]
          };
          this.props.actions.DisplayModal(confirmationObject); 
      }        
    }
  }

  chargeRateOverlapConfirmation = async() => {
    this.props.actions.HideModal();
    const res=await this.props.actions.SaveContractDetails();
    if(res)    //Added for Defect 952
    this.props.contractDetailTabs && this.props.contractDetailTabs.forEach(iteratedValue => {
      if(iteratedValue.tabBody === 'RateSchedule') this.callBackFuncs.reloadLineItemsOnSave();
    });     
  }

  chargeRateOverlap = () => {
    let hasRateOverlap = false;
    this.props.contractSchedules && this.props.contractSchedules.forEach(schedules => {
      if(this.props.contractRates) {
        const contractRates = this.props.contractRates.filter(rates => {
          if(rates.scheduleId === schedules.scheduleId && rates.recordStatus !== 'D') return rates;
        });
        if(contractRates && contractRates.length > 0 && !hasRateOverlap) {
          contractRates.forEach(iteratedValue => {
            const filteredRates = contractRates.filter(item => 
              (item.rateId !== iteratedValue.rateId && item.chargeType === iteratedValue.chargeType &&item.scheduleName===iteratedValue.scheduleName&&item.recordStatus==='N')
              && (moment(item.effectiveFrom).isSame(iteratedValue.effectiveFrom) || moment(item.effectiveTo).isSame(iteratedValue.effectiveFrom)
              || (moment(item.effectiveFrom).isSame(iteratedValue.effectiveTo) || moment(item.effectiveTo).isSame(iteratedValue.effectiveTo))
              || moment(item.effectiveFrom).isBetween(iteratedValue.effectiveFrom,iteratedValue.effectiveTo)
              || moment(item.effectiveTo).isBetween(iteratedValue.effectiveFrom,iteratedValue.effectiveTo))
            );
            if(!hasRateOverlap) hasRateOverlap = filteredRates.length > 0;       
            
          });
        }
      }
    });       
    return hasRateOverlap;
  }

  saveContractDetail = () => {
    this.props.actions.SaveContractDetails.then(response => {
      if(response){
        this.props.contractDetailTabs && this.props.contractDetailTabs.forEach(iteratedValue => {
          if(iteratedValue.tabBody === 'RateSchedule') {
            this.callBackFuncs.reloadLineItemsOnSave();
          }
        }); 
      }
    });
    this.props.actions.HideModal();
  }
  confirmationRejectHandler = () => {
    this.props.actions.HideModal();
  }
  cancelContractChanges = (e) => {
    e.preventDefault();
    //this.props.currentPage === "Create Contract" ? this.props.actions.CancelSearchDetails() : this.props.actions.CancelContractDataOnEdit();

    //Added fixes for D651 - Start
    if (this.props.currentPage === "Create Contract") {
      //for CR11 added 
      /* Existing funcationality start */
      // this.props.actions.CancelSearchDetails().then(res => {
      //   this.props.contractDetailTabs && this.props.contractDetailTabs.forEach(iteratedValue => {
      //     if (iteratedValue.tabBody === 'RateSchedule') {
      //       this.callBackFuncs.onCancelCharge();
      //     }
      //   });
      // }); 
      /* Existing funcationality end */
      // For CR11 close Cancel confirmation
      this.props.actions.CancelSearchDetails();
      this.props.actions.HideModal();
      this.props.actions.ClearData();
    }
    else {
      this.props.actions.CancelContractDataOnEdit().then(res => {
        this.props.contractDetailTabs && this.props.contractDetailTabs.forEach(iteratedValue => {
          if (iteratedValue.tabBody === 'RateSchedule') {
            this.callBackFuncs.onCancelCharge();
          }
        });
      });
      this.props.actions.HideModal();
    }
    //Added fixes for D651 - End
    
    //Comment for CR11
    // this.props.actions.HideModal();
    // this.props.actions.ClearData();
  }
  contractCancelHandler = () => {
    const confirmationObject = {
      title: modalTitleConstant.CONFIRMATION,
      message: modalMessageConstant.CANCEL_CHANGES,
      modalClassName: "warningToast",
      type: "confirm",
      buttons: [
        {
          buttonName: "Yes",
          onClickHandler: this.cancelContractChanges,
          className: "modal-close m-1 btn-small"
        },
        {
          buttonName: "No",
          onClickHandler: this.confirmationRejectHandler,
          className: "modal-close m-1 btn-small"
        }
      ]
    };
    this.props.actions.DisplayModal(confirmationObject);
  }

  contractDeleteHandler = () => {
    const confirmationObject = {
      title: modalTitleConstant.CONFIRMATION,
      message: modalMessageConstant.contract.CONTRACT_DELETE_MESSAGE,
      modalClassName: "warningToast",
      type: "confirm",
      buttons: [
        {
          buttonName: "Yes",
          onClickHandler: this.deleteContract,
          className: "modal-close m-1 btn-small"
        },
        {
          buttonName: "No",
          onClickHandler: this.confirmationRejectHandler,
          className: "modal-close m-1 btn-small"
        }
      ]
    };
    this.props.actions.DisplayModal(confirmationObject);
  }

  deleteContract = () => {
    this.props.actions.HideModal();
    this.props.actions.DeleteContractDetails()
      .then(response => {
        if (response) {
          if (response.code == 1) {
            this.props.history.push(AppMainRoutes.editContracts);
            this.props.actions.EditContract({
              currentPage:"Edit Contract",
              currentModule:"Contract"
            });
            this.props.actions.DeleteAlert(response.result, "Contract");
          }
        }
      });
  }

  uploadedDocumentCheck=()=>{  // when file Uploading user click on sve button showing warning Toaster Here 
    let count = 0;
    if (Array.isArray(this.props.ContractDocuments) && this.props.ContractDocuments.length > 0) {
        this.props.ContractDocuments.map(document =>{                             
                if(isUndefined(document.documentUniqueName)){ 
                IntertekToaster( localConstant.commonConstants.DOCUMENT_STATUS , 'warningToast documentRecordToPasteReq');
                count++;
                }                   
        });
         if(count > 0){
             return false;
         }else{
             return true;
         }
    }
    return true;
}
//Added for D-789 -Start
chargeRateDetailValidation(row) {
  row["chargeTypeValidation"] = "";
  row["chargeValueValidation"] = "";
  row["effectiveFromValidation"] = "";
  if(required(row.chargeType)){            
      row["chargeTypeValidation"] = localConstant.validationMessage.CHARGE_TYPE_VAL;
  }
  if(requiredNumeric(row.chargeValue) || row.chargeValue === "NaN"){            
      row["chargeValueValidation"] = localConstant.validationMessage.CHARGE_VALUE_VAL;
      row["chargeValueValidation"] = localConstant.validationMessage.CHARGE_VALUE_VAL;
  }
  if(required(row.effectiveFrom)){            
      row["effectiveFromValidation"] = localConstant.validationMessage.EFFECTIVE_FROM_VAL;
  }
  if(!required(row.effectiveFrom) && !this.isValidDateFormat(row.effectiveFrom)) {
      row["effectiveFromValidation"] = localConstant.commonConstants.INVALID_DATE_FORMAT;
      row["effectiveFrom"] = "";
  }
  if(!required(row.effectiveTo) && !this.isValidDateFormat(row.effectiveTo)) {
      row["effectiveToValidation"] = localConstant.commonConstants.INVALID_DATE_FORMAT;
      row["effectiveTo"] = "";
  }
  if (!required(row.effectiveFrom) && !required(row.effectiveTo) && this.isValidDateFormat(row.effectiveFrom) && this.isValidDateFormat(row.effectiveTo)
          && moment(row.effectiveFrom).isAfter(row.effectiveTo, 'day')) {
      row["effectiveFromValidation"] = localConstant.techSpecValidationMessage.PAY_RATE_DATE_RANGE_VALIDATION;
  }
  if(!isEmpty(this.props.contractDetails.contractStartDate)){//D589
      if(moment(row.effectiveFrom).isBefore(this.props.contractDetails.contractStartDate)){
          row["effectiveFromValidation"] = localConstant.validationMessage.EFFECTIVE_FROM_BEFORE_VALIDATION;
      }
  }        
  return row;
}
isValidDateFormat(date) {        
  if(date.indexOf(":") === -1 && dateUtil.isUIValidDate(date)) {
      return false;
  }
  return true;
}
//Added for D-789 -End
  /**
   * Validation check for the Genral Details and Invoice Defaults Tab
   */
  mandatoryFieldsValidationCheck = () => {
    let scheduleWithEmptyRates = false;
    const errors=[];
    const state = store.getState();
    // let isScheduleRateDateValid = true;
    if (this.props.contractDetails) {
      /**
       * General Details Tab Validation
       */
      //Customer Contract No
      if (this.props.contractDetails.customerContractNumber === null || this.props.contractDetails.customerContractNumber === "" || this.props.contractDetails.customerContractNumber === undefined) {
            errors.push(
              `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.CUSTOMER_CONTRACT_NUMBER }`
            );
      }
      //Customer
      if (this.props.contractDetails.contractCustomerName === null || this.props.contractDetails.contractCustomerName === "" || this.props.contractDetails.contractCustomerName === undefined) {
            errors.push( `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.CUSTOMER }`);
      }
      //CRM CODE
      if (this.props.contractDetails.isCRM === null || this.props.contractDetails.isCRM === "" || this.props.contractDetails.isCRM === undefined) {
            errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.CUSTOMER_CODE_CRM }`);
      }
      //CRM Code Inner Condition Check
      if (this.props.contractDetails.isCRM !== null || this.props.contractDetails.isCRM !== "" || this.props.contractDetails.isCRM !== undefined) {
        //CRM CODE - if true
        if (this.props.contractDetails.isCRM === true) {
          if (this.props.contractDetails.contractCRMReference === null || this.props.contractDetails.contractCRMReference === "" || this.props.contractDetails.contractCRMReference === undefined) {
              errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.CRM_OPP_REF }`);
          }
          if (this.props.contractDetails.contractConflictOfInterest === null || this.props.contractDetails.contractConflictOfInterest === "" || this.props.contractDetails.contractConflictOfInterest === undefined) {
            errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.CRM_CONFLICTS_OF_BID_REVIEW_COMMENTS }`);
          }
        }
        //CRM CODE - if false
        if (this.props.contractDetails.isCRM === false) {
          if (this.props.contractDetails.contractCRMReason === null || this.props.contractDetails.contractCRMReason === "" || this.props.contractDetails.contractCRMReason === undefined) {
            errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.WHY }`);
          }
        }
      }
      //Contract Type
      if (this.props.contractDetails.contractType === null || this.props.contractDetails.contractType === "" || this.props.contractDetails.contractType === undefined) {
        errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.CONTRACT_TYPE }`);
      }
      //Contract Type Inner Conditions Check
      if (this.props.contractDetails.contractType !== null || this.props.contractDetails.contractType !== "" || this.props.contractDetails.contractType !== undefined) {
        //Contract Type - Parent Contract
        if (this.props.contractDetails.contractType === "PAR") {
          if (this.props.contractDetails.parentCompanyOffice === null || this.props.contractDetails.parentCompanyOffice === "" || this.props.contractDetails.parentCompanyOffice === undefined) {
            errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.PARENT_COMPANY_OFFICE }`); 
          }
        }
        //Contract Type - Child Contract
        if (this.props.contractDetails.contractType === "CHD") {
          if (this.props.contractDetails.parentContractNumber === null || this.props.contractDetails.parentContractNumber === "" || this.props.contractDetails.parentContractNumber === undefined) {
            errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.PARENT_CONTRACT }`); 
          }
          if (this.props.contractDetails.parentContractDiscount === null || this.props.contractDetails.parentContractDiscount === "" || this.props.contractDetails.parentContractDiscount === undefined) {
            errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.PARENT_CONTRACT_DISCOUNT }`);
          }
          // if (this.props.contractDetails.parentContractHolder === null || this.props.contractDetails.parentContractHolder === "" || this.props.contractDetails.parentContractHolder === undefined) {
          //   errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.PARENT_CONTRACT_HOLDER }`);
          // }         /** commented for  D65-v2 */
        }
        //Contract Type - Framework Contract
        if (this.props.contractDetails.contractType === "FRW") {
          if (this.props.contractDetails.frameworkCompanyOfficeName === null || this.props.contractDetails.frameworkCompanyOfficeName === "" || this.props.contractDetails.frameworkCompanyOfficeName === undefined) {
            errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.FRAMEWORK_CONTRACT_OFFICE }`);
          }
        }
        //Contract Type - Related Framework Contract
        if (this.props.contractDetails.contractType === "IRF") {
          if (this.props.contractDetails.frameworkContractNumber === null || this.props.contractDetails.frameworkContractNumber === "" || this.props.contractDetails.frameworkContractNumber === undefined) {
            errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.FRAMEWORK_CONTRACT }`);
          }
          // if (this.props.contractDetails.frameworkContractHolder === null || this.props.contractDetails.frameworkContractHolder === "" || this.props.contractDetails.frameworkContractHolder === undefined) {
          //   errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.FRAMEWORK_CONTRACT_HOLDER }`);
          // }  /** commented for  D65-v2 */
        }
      }
       //Currency
      if (this.props.contractDetails.contractBudgetMonetaryCurrency === null || this.props.contractDetails.contractBudgetMonetaryCurrency === "" || this.props.contractDetails.contractBudgetMonetaryCurrency === undefined) {
        errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.CURRENCY }`);
      }
      //Value
      if (this.props.contractDetails.contractBudgetMonetaryValue === null || this.props.contractDetails.contractBudgetMonetaryValue === "" || this.props.contractDetails.contractBudgetMonetaryValue === undefined) {
        errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.VALUE }`);
      }
      //Hours Unit
      if (this.props.contractDetails.contractBudgetHours === null || this.props.contractDetails.contractBudgetHours === "" || this.props.contractDetails.contractBudgetHours === undefined) {
        errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.HOURS_UNIT }`);
      }

      if (this.props.contractDetails.contractBudgetMonetaryWarning === null || this.props.contractDetails.contractBudgetMonetaryWarning === "" || this.props.contractDetails.contractBudgetMonetaryWarning === undefined) {
        errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.budget.BUDGET_MONETARY_WARNING }`);
      }
      //Hours Unit
      if (this.props.contractDetails.contractBudgetHoursWarning === null || this.props.contractDetails.contractBudgetHoursWarning === "" || this.props.contractDetails.contractBudgetHoursWarning === undefined) {
        errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.budget.BUDGET_HOURS_WARNING }`);
      }
      //Start Date
      if (this.props.contractDetails.contractStartDate === null || this.props.contractDetails.contractStartDate === "" || this.props.contractDetails.contractStartDate === undefined) {
        errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.START_DATE }`);
      }
      else if (Array.isArray(this.props.contractprojectDetail) && this.props.contractprojectDetail.length>0) {
        const prjLessThanContractDate=this.props.contractprojectDetail.filter(x=> moment(x.projectStartDate).isBefore(moment(this.props.contractDetails.contractStartDate)) &&  !moment(x.projectStartDate).isSame(moment(this.props.contractDetails.contractStartDate)));
        if(prjLessThanContractDate.length>0){
          errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.START_DATE_GREATER_THAN_PRJ_DATE_WARNING }`);
        }
      }

      //Status
      if (this.props.contractDetails.contractStatus === null || this.props.contractDetails.contractStatus === "" || this.props.contractDetails.contractStatus === undefined) {
        errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.STATUS }`); 
      }
      if (this.props.contractDetails.contractStatus === "C") {
        if (this.props.contractDetails.contractEndDate === null || this.props.contractDetails.contractEndDate === "" || this.props.contractDetails.contractEndDate === undefined) {
          errors.push(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.END_DATE }`);   
        }
      }
      /**
       * Invoicing Defaults Tab Validation
       */
      if (this.props.contractDetails.contractType !== "FRW") {
        //Invoice Payment Terms
        if (this.props.contractDetails.contractInvoicePaymentTerms === null || this.props.contractDetails.contractInvoicePaymentTerms === "" || this.props.contractDetails.contractInvoicePaymentTerms === undefined) {
          errors.push(`${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.contract.INVOICE_PAYMENT_TERMS }`);  
        }
        //Sales tax
        if (this.props.contractDetails.contractSalesTax === null || this.props.contractDetails.contractSalesTax === "" || this.props.contractDetails.contractSalesTax === undefined) {
          errors.push(`${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.contract.SALES_TAX }`);  
        }
        //Customer Contract Contact
        if (this.props.contractDetails.contractCustomerContact === null || this.props.contractDetails.contractCustomerContact === "" || this.props.contractDetails.contractCustomerContact === undefined) {
          errors.push(`${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.contract.CUSTOMER_CONTRACT_CONTACT }`);  
        }
        //Customer Contract Address
        if (this.props.contractDetails.contractCustomerContactAddress === null || this.props.contractDetails.contractCustomerContactAddress === "" || this.props.contractDetails.contractCustomerContactAddress === undefined) {
          errors.push(`${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.contract.CUSTOMER_CONTRACT_ADDRESS }`);  
        }
        //Invoice Currency
        if (this.props.contractDetails.contractInvoicingCurrency === null || this.props.contractDetails.contractInvoicingCurrency === "" || this.props.contractDetails.contractInvoicingCurrency === undefined) {
          errors.push(`${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.contract.INVOICE_CURRENCY }`);  
        }
        //Customer Invoice Contact
        if (this.props.contractDetails.contractCustomerInvoiceContact === null || this.props.contractDetails.contractCustomerInvoiceContact === "" || this.props.contractDetails.contractCustomerInvoiceContact === undefined) {
          errors.push(`${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.contract.CUSTOMER_INVOICE_CONTACT }`);  
        }
        //Invoice Grouping
        if (this.props.contractDetails.contractInvoiceGrouping === null || this.props.contractDetails.contractInvoiceGrouping === "" || this.props.contractDetails.contractInvoiceGrouping === undefined) {
          errors.push(`${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.contract.INVOICE_GROUPING }`);  
        }
        //Invoice Address
        if (this.props.contractDetails.contractCustomerInvoiceAddress === null || this.props.contractDetails.contractCustomerInvoiceAddress === "" || this.props.contractDetails.contractCustomerInvoiceAddress === undefined) {
          errors.push(`${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.contract.INVOICE_ADDRESS }`);  
        }
        //Invoice Footer
        if (this.props.contractDetails.contractInvoiceFooterIdentifier === null || this.props.contractDetails.contractInvoiceFooterIdentifier === "" || this.props.contractDetails.contractInvoiceFooterIdentifier === undefined) {
          errors.push(`${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.contract.INVOICE_FOOTER }`); 
        }
        //Invoice Remittance Text
        if (this.props.contractDetails.contractInvoiceRemittanceIdentifier === null || this.props.contractDetails.contractInvoiceRemittanceIdentifier === "" || this.props.contractDetails.contractInvoiceRemittanceIdentifier === undefined) {
          errors.push(`${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.contract.INVOICE_REMITTANCE_TEXT }`); 
        }
      }
      if (Array.isArray(this.props.ContractDocuments) && this.props.ContractDocuments.length > 0) {
        const issueDoc = [];
        this.props.ContractDocuments.map(document =>{
          if(document.recordStatus!=='D')
          {
            if (isEmpty(document.documentType)) {
                errors.push(`${ localConstant.contract.Documents.DOCUMENTS } - ${ document.documentName } - ${ localConstant.contract.Documents.SELECT_FILE_TYPE } `);
          }
          // if(document.documentType === "Evolution Email" && (document.recordStatus === "N" || document.recordStatus === "M")){
          //   issueDoc.push(document.documentName);
          // }
          // else
          if (document.documentSize == 0 && document.documentType !== "Evolution Email") {
            const today = new Date();
            const currentYear = today.getFullYear();
            const currentQuarter = Math.floor((today.getMonth() + 3) / 3);
            const createdDate = new Date(document.createdOn);
            const createdYear = createdDate.getFullYear();
            const docCreatedQuarter = Math.floor((createdDate.getMonth() + 3) / 3);
            if (currentYear === createdYear && currentQuarter === docCreatedQuarter){
              issueDoc.push(document.documentName);
            }
          }
        }
      });
      if(issueDoc && issueDoc.length > 0){
        let techSpecData = '';
        for (let i = 0; i < issueDoc.length; i++) {
          techSpecData = techSpecData +'\"' +issueDoc[i]+'\"'+ '; \r\n';
        }
        errors.push(`${ StringFormat(localConstant.project.documents.UPLOAD_ISSUE, techSpecData) }`);
      }
    }
      let hasScheduleError = false;
      let HasRatesError = false;
      /** Schedule should have charge rate validation */
      this.props.contractSchedules && this.props.contractSchedules.forEach(iteratedValue => {
        if(iteratedValue.recordStatus !== "D"){
          const scheduleHasRates = this.props.contractRates && this.props.contractRates.find(rate => (rate.scheduleName === iteratedValue.scheduleName && rate.recordStatus !=="D"));
          if(isEmpty(scheduleHasRates)){
            const schedules = this.state.scheduleList;
            schedules.push({ "scheduleName":iteratedValue.scheduleName });
            this.setState({ 
              scheduleList:schedules,
              isScheduleValidationModalOpen:true
            });
            // IntertekToaster(`${ iteratedValue.scheduleName } should have at least one Charge Rate assosiated to it.`, `warningToast ${ iteratedValue.scheduleName }RateWarn`);
            scheduleWithEmptyRates = true;
          }
          if(required(iteratedValue.scheduleName)) hasScheduleError = true;
          if(required(iteratedValue.chargeCurrency)) hasScheduleError = true;
          if(!required(iteratedValue.scheduleName) && (this.props.contractSchedules.filter(x=> x.scheduleId !== iteratedValue.scheduleId && x.recordStatus != 'D' && x.scheduleName.toLowerCase().trim() === iteratedValue.scheduleName.toLowerCase().trim()).length > 0)) {                                                                      
            hasScheduleError = true;
          }
        }
      });
      
      this.props.contractRates && this.props.contractRates.forEach(iteratedValue => {
        iteratedValue=this.chargeRateDetailValidation(iteratedValue); //Added for D-789
        if (iteratedValue.recordStatus !== "D") {
          if(required(iteratedValue.chargeType)) HasRatesError = true;
          if(requiredNumeric(iteratedValue.chargeValue)) HasRatesError = true;
          if(required(iteratedValue.effectiveFrom)) HasRatesError = true;
          if(!required(iteratedValue.effectiveTo) && !required(iteratedValue.effectiveFrom)
              && moment(iteratedValue.effectiveFrom).isAfter(iteratedValue.effectiveTo, 'day')) {
                HasRatesError = true;
          }
          else if(!required(iteratedValue.effectiveFromValidation)){
            HasRatesError = true;  //Added for Defect 883
          }
        }
      });

      if(hasScheduleError) {
        this.props.contractDetailTabs && this.props.contractDetailTabs.forEach(iteratedValue => {
          if(iteratedValue.tabBody === 'RateSchedule') {
            this.callBackFuncs.reloadLineItemsOnSaveValidation();
          }
        });
        errors.push(`${ localConstant.contract.rateSchedule.CHARGE_SCHEDULES } - ${ localConstant.contract.CHARGE_SCHEDULE_RATE_VALIDATION }`);         
      }
      if(HasRatesError) {
        this.props.contractDetailTabs && this.props.contractDetailTabs.forEach(iteratedValue => {
          if(iteratedValue.tabBody === 'RateSchedule') {
            this.callBackFuncs.reloadLineItemsOnSaveValidation();
          }
        });
        errors.push(`${ localConstant.contract.rateSchedule.RATE_SCHEDULES } - ${ localConstant.contract.CHARGE_SCHEDULE_RATE_VALIDATION }`);
      }

      if(scheduleWithEmptyRates){
        const confirmationObject = {
          title: modalTitleConstant.ALERT_WARNING,
          message: modalMessageConstant.contract.CHARGE_RATES_OVERLAP,
          modalClassName: "warningToast",
          type: "confirm",
          buttons: [
            {
              buttonName: "OK",
              onClickHandler: this.saveContractDetail,
              className: "modal-close m-1 btn-small"
            }
          ]
        };
        return false;
      }
      // /** Schedule Rate start date less than the contract start date */
      // this.props.contractRates && this.props.contractRates.forEach(iteratedValue => {
      //   if (iteratedValue.recordStatus !== "D" && iteratedValue.recordStatus !== null) {
      //     if (moment(this.props.contractDetails.contractStartDate).isAfter(iteratedValue.effectiveFrom,'day') ) {
      //       IntertekToaster(localConstant.contract.CHARGE_RATE_DATE_GREATER_THAN_CONTRACT_DATE, 'warningToast ChargeRateDateWarn');
      //       isScheduleRateDateValid = false;
      //     }
      //   }
      // });
      // if (!isScheduleRateDateValid) {
      //   return false;
      // }
      if(errors.length > 0){
        this.setState({
          errorList:errors
        });
        return false;
      }   else {
        //Start Date Validation Check
                if (this.props.contractDetails.contractStartDate !== null) {
                  const formatedFullDate = moment(this.props.contractDetails.contractStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT);
                  const isValid = dateUtil.isUIValidDate(formatedFullDate);
                  if (!isValid) {
                    IntertekToaster(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.CONTRACT_START_DATE_FORMAT_VALIDATION }`, 'warningToast contractValidStartDateReq');
                    return false;
                  }
                }
      //Start Date & End Date Validation Check
      // if (this.props.contractDetails.contractEndDate !== null) {
              if (!isEmpty(this.props.contractDetails.contractEndDate)) {
                const formatedFullDate = moment(this.props.contractDetails.contractEndDate).format(localConstant.commonConstants.UI_DATE_FORMAT);
                const isValid = dateUtil.isUIValidDate(formatedFullDate);
                if (!isValid) {
                  IntertekToaster(`${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.CONTRACT_END_DATE_FORMAT_VALIDATION }`, 'warningToast contractValidEndDateReq');
                  return false;
                }
                if (moment(this.props.contractDetails.contractStartDate).isAfter(this.props.contractDetails.contractEndDate,'day')) {
                    IntertekToaster(localConstant.validationMessage.END_DATE_SHOULD_LESSTHAN_STARTDATE , 'warningToast contractDetailDateRangeReq');
                    return false;
                }
              }
               //Effective From Date cannot be before contract start date
               if (!isEmpty(this.props.contractDetails.effectiveFrom)) {
                const formatedFullDate = moment(this.props.contractDetails.effectiveFrom).format(localConstant.commonConstants.UI_DATE_FORMAT);
                const isValid = dateUtil.isUIValidDate(formatedFullDate);
                if (!isValid) {
                  IntertekToaster(localConstant.contract.CONTRACT_EFFECTIVE_FROM_DATE_FORMAT_VALIDATION, 'warningToast contractValidEffectiveFromDateReq');
                  return false;
                }
                if (moment(this.props.contractDetails.contractStartDate).isAfter(this.props.contractDetails.effectiveFrom,'day')) {
                    IntertekToaster(localConstant.contract.CHARGE_RATE_DATE_GREATER_THAN_CONTRACT_DATE, 'warningToast ChargeRateDateWarn');
                    return false;
                }              
              }
              if(this.props.contractDetails.contractBudgetHoursWarning > 100 )
              {
                IntertekToaster(localConstant.budget.BUDGET_HOURS_MESSAGE,'warningToast');
                return false;
              }

              if(this.props.contractDetails.contractBudgetMonetaryWarning > 100)
              {
                IntertekToaster(localConstant.budget.BUDGET_MONETARY_MESSAGE,'warningToast');
                return false;
              }              
        }

      return true;
    }
    else {
      IntertekToaster(localConstant.contract.CUSTOMER_CONTRACT_NUMBER_VALIDATION, 'warningToast customerContractNumberReq');
      return false;
    }
  }

  createProjectHandler = () => {
    const createProjectMenuData = {
       menuFun:'CreateProject',
       currentPage:'createProject',
       cuttentModule:'project'
    };
    this.props.actions.GetSelectedCustomerName(this.props.selectedContract);
    this.props.actions.CreateProject(createProjectMenuData);
    const contractNumber=encodeURIComponent(this.props.contractDetails.contractNumber);
    //this.props.history.push('/CreateProject');
     window.open('/CreateProject?contractNumber='+contractNumber,'_blank'); //On Click Button Open Tab for New Project
    // this.props.history.push({ pathname:'/CreateProject',search:`?contractNumber=${
    //   this.props.contractDetails.contractNumber
    //   }` });
  }

  render() {
    // let interactionMode = true;
    const currentPage = this.props.currentPage;
    let selectedContractInfo = [];
    // const contractDetailTabs = this.props.contractDetailTabs;
    if (this.props.selectedContract) {
      selectedContractInfo = this.props.selectedContract;
    }
    // interactionMode = this.props.location.pathname === '/CreateContracts' ? this.state.creatModeDisbale : this.props.interactionMode;
  
    contractDetailTabs.map((tabs, i) => {
      return tabs.tabDisableStatus.map((tabDisable, index) => {
        if ((tabs.tabHeader === "Invoicing Defaults" && tabDisable === this.props.selectedContractType) || tabDisable === this.props.contractDetails.contractType) {
          contractDetailTabs[i].tabActive = false;
        }
        else if (tabs.tabHeader === "Projects" && (tabDisable === currentPage || this.props.contractDetails.contractType === "FRW")) {
          contractDetailTabs[i].tabActive = false;          
        }
        else if (tabs.tabHeader === "Child Contracts") {
          if (tabDisable !== currentPage && this.props.contractDetails.contractType === "PAR") {
            contractDetailTabs[i].tabActive = true;
          }
          else {
            contractDetailTabs[i].tabActive = false;
          }
        }

        else if (tabs.tabHeader === "Documents" && tabDisable === currentPage) {
            contractDetailTabs[i].tabActive = false;
        }

        else {
          contractDetailTabs[i].tabActive = true;
        }

      });
    });

    const isInEditMode= (this.props.interactionMode ===false && currentPage==="Edit Contract");
    const isInViewMode=(this.props.pageMode===localConstant.commonConstants.VIEW)?true:false;
    const isInCreateMode=(currentPage==="Create Contract");
    const response = ButtonShowHide(isInEditMode,isInViewMode,isInCreateMode);
    
    let interactionMode= this.props.interactionMode;
    if(this.props.pageMode===localConstant.commonConstants.VIEW){
     interactionMode=true;
    }
    const contractSave = [
      {
        name: 'save',
        clickHandler: () => this.contractSaveHandler(),
        className: "btn-small mr-0 ml-2",
        permissions:[ activitycode.NEW,activitycode.MODIFY ],
        isbtnDisable: this.props.isbtnDisable ,
        showBtn:response[0]       
      }, {
        name: localConstant.commonConstants.REFRESHCANCEL,
        clickHandler: () => this.contractCancelHandler(),
        className: "btn-small mr-0 ml-2 waves-effect modal-trigger",
        permissions:[  activitycode.NEW,activitycode.MODIFY ] ,
        isbtnDisable: this.props.isbtnDisable,
        showBtn:response[0]  
      },
      {
        name: 'Delete',
        clickHandler: () => this.contractDeleteHandler(),
        className: "btn-small btn-primary mr-0 ml-2 dangerBtn modal-trigger waves-effect",
        permissions:[  activitycode.DELETE ],
        showBtn: response[1]
      },
      {
        name: 'New Project',
        clickHandler: () => this.createProjectHandler(),
        className: "btn-small mr-0 ml-2 waves-effect",
        permissions:[ activitycode.NEW ],
        showBtn: response[1]
      }
    ];

    contractSave.map((btn, i) => {
      if (this.props.currentPage === "Create Contract" && btn.name === "New Project") {
        contractSave.splice(i, 1);
      }
      if (this.props.currentPage === "Create Contract" && btn.name === "Delete") {
        btn.className = "btn-small mr-0 ml-2 waves-effect modal-trigger disabled";
      }
      //To-Do: Need to change the logic in generic way
      if ((this.props.currentPage === "Edit Contract") && (this.props.pageMode!==localConstant.commonConstants.VIEW) && 
      (btn.name === "New Project" || btn.name === "Delete")) {
        btn.showBtn = true;
      }
      if ((this.props.currentPage === "Edit Contract") && (btn.name === "New Project")){
        if(this.props.contractDetails.contractType !== "FRW"){
        btn.className = "btn-small mr-0 ml-2 waves-effect modal-trigger";
        }
        else{
        btn.className = "btn-small mr-0 ml-2 waves-effect modal-trigger hide";
        }
      }
    });

       //Hard Fix
    //Start
    let cNo = this.props.contractDetails.contractNumber ? this.props.contractDetails.contractNumber :
      (selectedContractInfo && selectedContractInfo.contractNumber) ? selectedContractInfo.contractNumber : '';
    if (this.props.currentPage === "Create Contract") {
      cNo = "";
    }
    //End

    return (
      <Fragment>
        {this.state.errorList.length > 0 ?
            <Modal title={ localConstant.commonConstants.CHECK_MANDATORY_FIELD }
            titleClassName="chargeTypeOption"
            modalContentClass="extranetModalContent"
            modalClass="ApprovelModal"
            modalId="errorListPopup"
            formId="errorListForm"
            buttons={this.errorListButton}
            isShowModal={true}>
                <ErrorList errors={this.state.errorList}/>
            </Modal> : null
        }
        {this.state.isScheduleValidationModalOpen ?
          <Modal title="Error"
            titleClassName="chargeTypeOption"
            modalId="scheduleValidationPopup"
            formId="scheduleValidationForm"
            buttons={this.scheduleValidationButton}
            isShowModal={this.state.isScheduleValidationModalOpen}>
            <ScheduleValidationPopup
              state={this.state}
              HeaderData={HeaderData}/>
          </Modal> : null
        }
        <SaveBarWithCustomButtons
          codeLabel={localConstant.sideMenu.CONTRACTS}
          codeValue={cNo}
          //codeValue={this.props.contractDetails.contractNumber}
          currentMenu={localConstant.contract.CONTRACTS}
          currentSubMenu={this.props.currentPage}          
          buttons={contractSave}
          activities={this.props.activities}/>

        <div className="row ml-0 mb-0">
          <div className="col s12 pl-0 pr-0 verticalTabs">
            <CustomTabs
              tabsList={contractDetailTabs}
              moduleName="contract"
              currentPage={this.props.location.pathname}
              interactionMode={interactionMode}
              onSelect = { scrollToTop }
              callBackFuncs = {this.callBackFuncs}
            >
            </CustomTabs>
          </div>
        </div>
      </Fragment>
    );
  }
}

export default ContractDetails;