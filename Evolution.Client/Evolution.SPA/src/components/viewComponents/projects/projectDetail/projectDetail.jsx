import React, { Component, Fragment } from 'react';
import { ProjectTabDetail } from './projectTabDetail';
import { SaveBarWithCustomButtons } from '../../../applicationComponents/saveBar';
import CustomTabs from '../../../../common/baseComponents/customTab';
import { required,requiredNumeric } from '../../../../utils/validator';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { modalTitleConstant, modalMessageConstant } from '../../../../constants/modalConstants';
import { isEmpty, getlocalizeData, isEmptyReturnDefault,parseQueryParam, scrollToTop,ResetCurrentModuleTabInfo,ObjectIntoQuerySting,isUndefined } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import Modal from '../../../../common/baseComponents/modal';
import ErrorList  from '../../../../common/baseComponents/errorList';
import { activitycode } from '../../../../constants/securityConstant';
import { ButtonShowHide } from '../.././../../utils/permissionUtil';
import { StringFormat } from '../../../../utils/stringUtil';
const localConstant = getlocalizeData();
const tabsToHide = [ 'SupplierPO','Assignments' ];
const modes = [ 'editProject','createProject' ];

class ProjectDetail extends Component {
  constructor(props) {
    super(props);
    this.state = {
      errorList:[],
    };
    const { projectMode } = this.props;
   this.projectTabDetail = [];
    this.errorListButton =[
      {
        name: localConstant.commonConstants.OK,
        action: this.closeErrorList,
        btnID: "closeErrorList",
        btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
        showbtn: true
      }
    ];
    ResetCurrentModuleTabInfo(ProjectTabDetail);
  }

  componentDidMount() {     
    const result = this.props.location.search && parseQueryParam(this.props.location.search);
    const { projectMode, selectedProjectNo } = this.props;

    if(this.props.location.pathname === '/CreateProject'){
      this.props.actions.UpdateProjectMode('createProject');    
      this.props.actions.HandleMenuAction({
       currentPage: localConstant.sideMenu.CREATE_PROJECT,
       currentModule: localConstant.moduleName.PROJECT
       }).then(res=>{
          if(res===true){
            //this.props.actions.GetSelectedContractData(result);
            this.props.actions.FetchContractForProjectCreation(result);
          }
       });
     }
    // if (!selectedProjectNo && projectMode === 'createProject') {
    //   this.props.actions.FetchContractForProjectCreation();
    //  }
    if(result.projectNumber){
      this.props.actions.EditProject('editProject');
      this.props.actions.FetchProjectDetail(result);
      this.props.actions.SaveSelectedProjectNumber(result.projectNumber);
    }
  }
 
  // filterTabDetail = () => {
    
  //   const { projectMode } = this.props;
  //   const filteredProjectTabs = ProjectTabDetail.filter((eachTab) => {
  //     return modes.includes(projectMode) ||
  //     tabsToHide.includes(eachTab.tabBody);
  //   });
  //   return filteredProjectTabs;
  // }

  projectSaveClickHandler = () => {
    const validDoc = this.uploadedDocumentCheck();
    const valid = this.mandatoryFieldsValidationCheck();
    if (valid === true && validDoc) {
      this.props.actions.SaveProjectDetails();
    }
  };

  projectCancelClickHandler = () => {
    const confirmationObject = {
      title: modalTitleConstant.CONFIRMATION,
      message: modalMessageConstant.CANCEL_CHANGES,
      modalClassName: "warningToast",
      type: "confirm",
      buttons: [
        {
          buttonName: "Yes",
          onClickHandler: this.cancelProjectChanges,
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

  cancelProjectChanges = () => {  
    const result = this.props.location.search && parseQueryParam(this.props.location.search);
    this.props.projectMode === "createProject" ? this.props.actions.CancelCreateProjectDetails(result) : this.props.actions.CancelEditProjectDetails();
    this.props.actions.HideModal();
  }

  projectDeleteClickHandler = () => {  
    const confirmationObject = {
      title: modalTitleConstant.CONFIRMATION,
      message: modalMessageConstant.PROJECT_DELETE_MESSAGE,
      modalClassName: "warningToast",
      type: "confirm",
      buttons: [
        {
          buttonName: "Yes",
          onClickHandler: this.deleteProject,
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

  deleteProject = () => {
    this.props.actions.HideModal();
    this.props.actions.DeleteProjectDetails()
      .then(response => {
        if (response) {
          if (response.code == 1) {
            this.props.history.push('/EditProject');
            this.props.actions.DeleteAlert(response.result, "Project");
          }
        }
      });
  }

  confirmationRejectHandler = () => {
    this.props.actions.HideModal();
  }

  uploadedDocumentCheck=()=>{  // when file Uploading user click on sve button showing warning Toaster Here 
    let count = 0;
    if (Array.isArray(this.props.projectDocuments) && this.props.projectDocuments.length > 0) {
        this.props.projectDocuments.map(document =>{                             
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

  mandatoryFieldsValidationCheck = () => { 
    const { generalDetails, selectedContract } = this.props;
    const projectStartDate = generalDetails.projectStartDate && new Date(generalDetails.projectStartDate);
    const projectEndDate = generalDetails.projectEndDate && new Date(generalDetails.projectEndDate);
    const contractStartDate = selectedContract.contractStartDate && new Date(selectedContract.contractStartDate);
    const contractEndDate = selectedContract.contractEndDate && new Date(selectedContract.contractEndDate);
    const contractBudgetMonetaryValue = parseFloat(selectedContract.contractBudgetMonetaryValue);
    const contractBudgerUnits = parseFloat(selectedContract.contractBudgetHours);
    const errors=[];

    if (this.props.projectDetails) {
      if (isEmpty(generalDetails.customerProjectNumber)) {
        errors.push( `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.project.CUSTOMER_PROJECT_NUMBER }`);
      }
      if (isEmpty(generalDetails.customerProjectName)) {
        errors.push( `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.project.CUSTOMER_PROJECT_NAME }`);
      }
      if (isEmpty(generalDetails.companyDivision)) {
        errors.push( `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.project.DIVISION }`);
      }
      if (isEmpty(generalDetails.companyCostCenterCode)) {
        errors.push( `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.project.COST_CENTRE }`);
      }
      if (isEmpty(generalDetails.companyOffice)) {
        errors.push( `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.project.OFFICE }`);
      }
      if (isEmpty(generalDetails.projectType)) {
        errors.push( `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.project.BUSINESS_UNIT }`);
      }
      let businessUnit = this.props.businessUnit.filter(iteratedValue => iteratedValue.name === generalDetails.projectType);
      businessUnit = isEmptyReturnDefault(businessUnit);
      if (businessUnit.length > 0 && businessUnit[0].invoiceType === "NDT") {
        if (isEmpty(generalDetails.logoText)) {
          errors.push( `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.companyDetails.generalDetails.LOGO }`);
        }
      }
      if (isEmpty(generalDetails.workFlowType)) {
        errors.push( `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.project.WORKFLOW_TYPE }`);
      }
      if (isEmpty(generalDetails.industrySector)) {
        errors.push( `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.project.INDUSTRY_SECTOR }`);
      }
      if (generalDetails.isProjectForNewFacility==="" || generalDetails.isProjectForNewFacility===undefined) {
        errors.push( `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.project.PROJECT_IS_FOR_NEW_FACILITY }`);
      }
      if (isEmpty(generalDetails.projectCoordinatorName)) {
        errors.push( `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.project.COORDINATOR }`);
      }
      if (generalDetails.isManagedServices===""||generalDetails.isManagedServices===undefined) {
        errors.push( `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.project.MANAGED_SERVICES }`);
      }
      if (generalDetails.isManagedServices === true ) {
        if (requiredNumeric(generalDetails.managedServiceType)) {
          errors.push( `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.project.TYPES_OF_SERVICE }`);
        }
      }
      if (requiredNumeric(generalDetails.projectBudgetValue)) {
        errors.push( `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.budget.BUDGET_MONETARY  }`);
      }
      if(generalDetails.projectBudgetWarning === undefined || generalDetails.projectBudgetWarning === null || generalDetails.projectBudgetWarning === "")
      {
        errors.push( `${ localConstant.contract.GENERAL_DETAILS }- ${ localConstant.budget.BUDGET_MONETARY_WARNING }`);
      }
      if (requiredNumeric(generalDetails.projectBudgetHoursUnit)) {
        errors.push( `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.budget.BUDGET_HOURS }`);
      }
      if(generalDetails.projectBudgetHoursWarning === undefined || generalDetails.projectBudgetHoursWarning === null || generalDetails.projectBudgetHoursWarning === "")
      {
        errors.push( `${ localConstant.contract.GENERAL_DETAILS }- ${ localConstant.budget.BUDGET_HOURS_WARNING }`);
      }
      if (isEmpty(generalDetails.projectStartDate)) {
        errors.push( `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.START_DATE }`);
      }
      if (isEmpty(generalDetails.projectStatus)) {
        errors.push( `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.STATUS }`);
      }
      if (generalDetails.projectStatus === 'C') {
        if (isEmpty(generalDetails.projectEndDate)) {
          errors.push( `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.contract.END_DATE }`);
        }
      }
      // if (moment(contractStartDate).isAfter(projectStartDate, 'day') || moment(projectStartDate).isAfter(contractEndDate, 'day')) {
      //   IntertekToaster(localConstant.validationMessage.PROJECT_START_DATE_MISMATCH, 'warningToast projectStartDateMismatchVal');
      //   return false;
      // }

      // if (projectStartDate < contractStartDate || projectStartDate > contractEndDate) {
      //   IntertekToaster(localConstant.validationMessage.PROJECT_START_DATE_MISMATCH, 'warningToast projectStartDateMismatchVal');
      //   return false;
      // }
      // if (!isEmpty(projectEndDate)) {
      //   if (moment(projectStartDate).isAfter(projectEndDate, 'day')) {
      //     IntertekToaster(localConstant.validationMessage.PROJECT_START_DATE_END_DATE_MISMATCH, 'warningToast projectStartDateEndDateMismatchVal');
      //     return false;
      //   }
      //   if (moment(contractStartDate).isAfter(projectEndDate, 'day')) {
      //     IntertekToaster(localConstant.validationMessage.PROJECT_END_DATE_MISMATCH, 'warningToast projectEndDateMismatchVal');
      //     return false;
      //   }
      // }
      /**
       * Invoice Payment Terms validation check 
       * */
      if (required(this.props.projectDetails.projectInvoicePaymentTerms)) {
        errors.push( `${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.contract.INVOICE_PAYMENT_TERMS }`);
      }
      /**
       * Customer Project Contact validation check
       */
      if (required(this.props.projectDetails.projectCustomerContact)) {
        errors.push( `${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.project.invoicingDefaults.CUSTOMER_PROJECT_CONTACT }`);
      }
      /**
       * Project Company Address validation check
       */
      if (required(this.props.projectDetails.projectCustomerContactAddress)) {
        errors.push( `${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.project.invoicingDefaults.PROJECT_COMPANY_ADDRESS }`);
      }
      /**
       * Customer Invoice Contact validation check
       */
      if (required(this.props.projectDetails.projectCustomerInvoiceContact)) {
        errors.push( `${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.project.invoicingDefaults.CUSTOMER_INVOICE_CONTACT }`);
      }
      /**
       * Invoice Address validation check
       */
      if (required(this.props.projectDetails.projectCustomerInvoiceAddress)) {
        errors.push( `${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.contract.INVOICE_ADDRESS }`);
      }
      /**
       * Invoice Remittance Text validation check
       */
      if (required(this.props.projectDetails.projectInvoiceRemittanceIdentifier)) {
        errors.push( `${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.contract.INVOICE_REMITTANCE_TEXT }`);
      }
      /**
       * Sales Tax validation check
       */
      if (required(this.props.projectDetails.projectSalesTax)) {
        errors.push( `${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.contract.SALES_TAX }`);
      }
      /**
       * Default Invoice Currency validation check
       */
      if (required(this.props.projectDetails.projectInvoicingCurrency)) {
        errors.push( `${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.project.invoicingDefaults.DEFAULT_INVOICE_CURRENCY }`);
      }
      /**
       * Default Invoice Grouping validation check
       */
      if (required(this.props.projectDetails.projectInvoiceGrouping)) {
        errors.push( `${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.project.invoicingDefaults.DEFAULT_INVOICE_GROUPING }`);
      }
      /**
       * Invoice Footer validation check
       */
      if (required(this.props.projectDetails.projectInvoiceFooterIdentifier)) {
        errors.push( `${ localConstant.contract.INVOICING_DEFAULTS } - ${ localConstant.contract.INVOICE_FOOTER }`);
      }
      if(!isEmpty(this.props.projectDocuments)){
        const issueDoc = [];
        this.props.projectDocuments.map(document => {
          if (document.recordStatus !== "D") {
            if (isEmpty(document.documentType)) {
              errors.push(`${ localConstant.project.documents.DOCUMENT } - ${ document.documentName } - ${ localConstant.project.documents.SELECT_FILE_TYPE } `);
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
        if (issueDoc && issueDoc.length > 0) {
          let techSpecData = '';
          for (let i = 0; i < issueDoc.length; i++) {
            techSpecData = techSpecData +'\"' +issueDoc[i]+'\"'+ '; \r\n';
          }
          errors.push(`${ StringFormat(localConstant.project.documents.UPLOAD_ISSUE, techSpecData) }`);
        }
      }

      if(errors.length > 0){
        this.setState({
          errorList:errors
        });
        return false;
      } else {
        //Added the below validation in API for D-1304
            // if(contractBudgetMonetaryValue > 0 && ((parseFloat(generalDetails.projectBudgetValue) > contractBudgetMonetaryValue) || (parseFloat(generalDetails.projectBudgetHoursUnit) > contractBudgerUnits))){
            //   IntertekToaster(`${ localConstant.validationMessage.PROJECT_BUDGET_EXCEED_VALIDATION }`, 'warningToast projectBudgetValueVal');
            //   return false;
            // }
            if (!(dateUtil.isValidDate(dateUtil.formatDate(generalDetails.projectStartDate, '-')))) {
              IntertekToaster(`${ localConstant.validationMessage.INVALID_PROJECT_START_DATE }`, 'warningToast projectEndDateVal');
              return false;
            }
            if(contractStartDate && isEmpty(contractEndDate)){
              if(moment(contractStartDate).isAfter(projectStartDate,'day')){
                 IntertekToaster( `${ localConstant.validationMessage.PROJECT_START_DATE_INBETWEEN_VALIDATION }`, 'warningToast projectStartDateMismatchVal');
                 return false;
              }
            }
            if(projectEndDate){
              if (!(dateUtil.isValidDate(dateUtil.formatDate(generalDetails.projectEndDate, '-')))) {
                IntertekToaster( `${ localConstant.validationMessage.INVALID_PROJECT_END_DATE }`, 'warningToast projectEndDateVal');
                return false;
              }
              if (moment(projectStartDate).isAfter(projectEndDate, 'day')) {
                IntertekToaster( `${ localConstant.validationMessage.END_DATE_SHOULD_LESSTHAN_STARTDATE }`, 'warningToast projectStartDateEndDateMismatchVal');
                return false;
              }
            }
            if(contractEndDate){
              if(!dateUtil.dateInBetween(projectStartDate,contractStartDate,contractEndDate)){
                IntertekToaster( `${ localConstant.validationMessage.PROJECT_START_DATE_INBETWEEN_VALIDATION }`, 'warningToast projectStartDateBetweenMismatchVal');
                return false;
              }
              if(projectEndDate){
                if(!dateUtil.dateInBetween(projectEndDate,contractStartDate,contractEndDate)){
                  IntertekToaster( `${ localConstant.validationMessage.PROJECT_END_DATE_INBETWEEN_VALIDATION }`, 'warningToast projectEndDateBetweenMismatchVal');
                  return false;
                }
              }
            }
            if (generalDetails.contractStatus === 'C') {
              if (!(dateUtil.isValidDate(dateUtil.formatDate(generalDetails.projectEndDate, '-')))) {
                IntertekToaster( `${ localConstant.validationMessage.INVALID_PROJECT_END_DATE }`, 'warningToast projectEndDateVal');
                return false;
              }
            }
            if(generalDetails.projectBudgetWarning > 100)
            {
              IntertekToaster(localConstant.budget.BUDGET_MONETARY_MESSAGE,'warningToast');
              return false;
            }

            if(generalDetails.projectBudgetHoursWarning > 100)
            {
              IntertekToaster(localConstant.budget.BUDGET_HOURS_MESSAGE, 'warningToast');
              return false;
            }
      }

      return true;
    }
    else {
      IntertekToaster(localConstant.validationMessage.CUSTOMER_PROJECT_NUMBER_VAL, 'warningToast customerProjectNumberVal');
      return false;
    }
  }

  closeErrorList =(e) =>{
    e.preventDefault();
    this.setState({
      errorList:[]
    });
  }
  
  enableSupplierPo = () => {

    const  projectMode  = this.props.projectMode;
    const { generalDetails } = this.props;
    const workFlowType = generalDetails && generalDetails.workFlowType;
    this.projectTabDetail =ProjectTabDetail;
    const filteredProjectTabs = ProjectTabDetail.filter((eachTab) => {
      if (projectMode === 'editProject' && (workFlowType === "V" || workFlowType === "S")) {
        return true;
      } else if (projectMode !== 'editProject') {
        return 'SupplierPO' !== eachTab.tabBody &&
          'Assignments' !== eachTab.tabBody;
      } else {
        return 'SupplierPO' !== eachTab.tabBody;
      }
    });
  }

  createSupplierPOHandler = () => {    
    /**
     * TO-DO: Whenever we are releasing the supplier Po Need to uncomment the below code
     */
    const queryobj={
      projectNumber:this.props.selectedProjectNo
    };
    const queryStr = ObjectIntoQuerySting(queryobj);
    window.open('/SupplierPODetails?'+queryStr,'_blank');
    //this.props.actions.FetchProjectForSupplierPOCreation(queryobj.projectNumber);
    //this.props.history.push('/SupplierPODetails');
  }
  createAssignmentHandler = () => {
    this.props.actions.SaveSelectedProjectNumber(this.props.selectedProjectNo);
    this.props.actions.HandleMenuAction({
      currentPage:localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE,
      currentModule:"assignment"
    });
    window.open(AppMainRoutes.createAssignment+'?projectNumber='+this.props.selectedProjectNo,'_blank');
    //this.props.history.push(AppMainRoutes.createAssignment);
    // IntertekToaster('Assignment is under Construction', 'warningToast CreateAssignment');
    // return false;
  }

  // tabSelect = (index, lastIndex, event) => {
  //   const tabId = event.target.getAttribute("tabuniqueid");
  //   /**
  //    * SetCurrentModuleTabInfo Method will set currentTab and isTabRendered(bool params)
  //    * isTabRendered is used to avoid unnecessary api calls in each tab of respective module
  //    */
  //   SetCurrentModuleTabInfo(ProjectTabDetail, tabId);
  //   scrollToTop();
  // }

  render() {
    this.enableSupplierPo();
    const isInEditMode= (this.props.interactionMode ===false && this.props.projectMode==="editProject" );
    const isInViewMode= (this.props.pageMode===localConstant.commonConstants.VIEW)?true:false;
    const isInCreateMode= (this.props.projectMode === "createProject");
    const response = ButtonShowHide(isInEditMode,isInViewMode,isInCreateMode);
    let interactionMode= this.props.interactionMode;
    if(this.props.pageMode===localConstant.commonConstants.VIEW){
     interactionMode=true;
    }

    const projectSave = [
      {
        name: localConstant.commonConstants.SAVE,
        clickHandler: () => this.projectSaveClickHandler(),
        className: "btn-small mr-0 ml-2",
        permissions:[ activitycode.NEW,activitycode.MODIFY ],
        isbtnDisable: this.props.isbtnDisable,
        showBtn: response[0]     
      }, {
        name: localConstant.commonConstants.REFRESHCANCEL,
        clickHandler: () => this.projectCancelClickHandler(),
        className: "btn-small mr-0 ml-2 waves-effect modal-trigger",
        permissions:[ activitycode.NEW,activitycode.MODIFY ],
        isbtnDisable: this.props.isbtnDisable ,
        showBtn:response[0]     
      },
      {
        name: localConstant.commonConstants.DELETE,
        clickHandler: () => this.projectDeleteClickHandler(),
        className: "btn-small btn-primary mr-0 ml-2 dangerBtn modal-trigger waves-effect",
        permissions:[  activitycode.DELETE ],
        showBtn: response[1]        
      },
      {
        name: localConstant.commonConstants.NEW_SUPPLIER_PO,
        clickHandler: () => this.createSupplierPOHandler(),
        className: "btn-small mr-0 ml-2 waves-effect modal-trigger",  
        permissions:[ activitycode.NEW ],
        showBtn: response[1]  
      },
      {
        name: localConstant.commonConstants.NEW_ASSIGNMENT,
        clickHandler: () => this.createAssignmentHandler(),
        className: "btn-small mr-0 ml-2 waves-effect modal-trigger", 
        permissions:[ activitycode.NEW ],
        showBtn: response[1]       
      }
    ];

    projectSave.map((btn, i) => {
      const workFlowType = this.props.generalDetails.workFlowType && this.props.generalDetails.workFlowType.trim();
      if (this.props.projectMode === "createProject" && 
          btn.name === localConstant.commonConstants.DELETE) {
        btn.className = "btn-small mr-0 ml-2 waves-effect modal-trigger disabled";
      }
      
      if ((this.props.projectMode === "createProject" || this.props.interactionMode === true)
       && (btn.name === localConstant.commonConstants.NEW_SUPPLIER_PO || btn.name === localConstant.commonConstants.NEW_ASSIGNMENT)) {
        btn.className = "btn-small mr-0 ml-2 waves-effect modal-trigger hide";
        btn.showBtn = false;
      }
      
      if (this.props.projectMode === "editProject" && this.props.pageMode!==localConstant.commonConstants.VIEW
        && btn.name === localConstant.commonConstants.DELETE
        && this.props.interactionMode === false) {
        btn.showBtn = true;
      }

      if (this.props.projectMode === "editProject"
        && btn.name === localConstant.commonConstants.NEW_SUPPLIER_PO
        && (workFlowType === 'M'||workFlowType === 'N')){
          btn.className = "btn-small mr-0 ml-2 waves-effect modal-trigger hide"; 
        }
    });
    const  projectModeUrl  = this.props.location.pathname;
    const { generalDetails } = this.props;
    const workFlowType = generalDetails && generalDetails.workFlowType;
    const projectDetailTabs = ProjectTabDetail;
    projectDetailTabs.map((tabs,i)=>{
    return tabs.tabDisableStatus.map((tabDisable, index) => {
      if(tabs.tabBody === 'SupplierPO' ){
        if(tabDisable === projectModeUrl || this.props.projectMode === "editProject"){          
          if(workFlowType === "V" || workFlowType === "S"){
            if(this.props.pageMode !== localConstant.commonConstants.VIEW)//Added for D995
              projectDetailTabs[i].tabActive = true;
            else
              projectDetailTabs[i].tabActive = false;
          } else if(workFlowType === "M" || workFlowType === "N"){
            projectDetailTabs[i].tabActive = false;
          }
        } else {
          projectDetailTabs[i].tabActive = false;
        }
      } else if(tabs.tabBody === 'Assignments'){
          if(tabDisable === projectModeUrl){
            projectDetailTabs[i].tabActive = true;
          } if(this.props.projectMode === "editProject"){
            projectDetailTabs[i].tabActive = true;
          } else{
            projectDetailTabs[i].tabActive = false;
          }
      }
      else if(tabs.tabBody === 'Documents'){
        if(this.props.projectMode === "editProject")
          projectDetailTabs[i].tabActive = true;
          else
          projectDetailTabs[i].tabActive = false;
      }
    });
    });

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

        <SaveBarWithCustomButtons
          codeLabel={localConstant.sideMenu.PROJECTS}
          codeValue={this.props.selectedProjectNo ? `(${ this.props.projectDetails? this.props.projectDetails.contractCustomerName.substring(0,5):"" } : ${ this.props.selectedProjectNo })` : ''}
          currentMenu={localConstant.moduleName.PROJECT}
          currentSubMenu={this.props.projectMode === "createProject"? localConstant.project.ADD_PROJECT: this.props.interactionMode === true ? "View Project" : "Edit Project"}
          buttons={projectSave}
          activities={this.props.activities}
        />
        <div className="row ml-2 mb-0">
          <div className="col s12 pl-0 pr-2 verticalTabs">
            <CustomTabs
              tabsList={projectDetailTabs}
              moduleName="project"
              interactionMode={interactionMode}
              onSelect={scrollToTop}
            />
          </div>
        </div>
      </Fragment>
    );
  }
}

export default ProjectDetail;
