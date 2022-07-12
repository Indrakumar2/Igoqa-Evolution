import React, { Component, Fragment ,useState } from 'react';
import { AssignmentTabData } from './assignmentTabData';
import { SaveBarWithCustomButtons } from '../../../applicationComponents/saveBar';
import CustomTabs from '../../../../common/baseComponents/customTab';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';

import {
  isEmpty,
  getlocalizeData,
  parseQueryParam,
  formInputChangeHandler,
  isEmptyReturnDefault,
  scrollToTop,
  ResetCurrentModuleTabInfo,
  ObjectIntoQuerySting,
  isUndefined
} from '../../../../utils/commonUtils';
import { modalTitleConstant, modalMessageConstant } from '../../../../constants/modalConstants';
import { required, requiredNumeric } from '../../../../utils/validator';
import moment from 'moment';
import dateUtil from '../../../../utils/dateUtil';
import arrayUtil from '../../../../utils/arrayUtil';
import ErrorList from '../../../../common/baseComponents/errorList';
import Modal from '../../../../common/baseComponents/modal';
import { activitycode, securitymodule } from '../../../../constants/securityConstant';
import { ButtonShowHide, moduleViewAllRights_Modified } from '../.././../../utils/permissionUtil';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { SupplierVisitSearchFields } from '../../supplier/supplierVisitPerformanceReportModal/supplierVisitSearchFields';
import { headerData } from '../../supplierpo/supplierpoSearch/headerData';
import { EvolutionReportsAPIConfig } from '../../../../apiConfig/apiConfig';
import { DownloadReportFile }  from '../../../../common/reportUtil/ssrsUtil';
import { StringFormat } from '../../../../utils/stringUtil';

const localConstant = getlocalizeData();
let amendmentreason=false;
let amdmentReasonData='';
let existingAssignmentData='';

/** Assigned Specialist Confirmation popup */
const AssignedSpecialistConfirmPopup = (props) => {
  return (
    <Fragment>
      <p>{ localConstant.assignments.TAXONOMY_VALIDATION_1 }</p>
      <p>{ localConstant.assignments.TAXONOMY_VALIDATION_2 }</p>
      <p>{ localConstant.assignments.TAXONOMY_VALIDATION_3 }</p>
      <p>{ localConstant.assignments.TAXONOMY_VALIDATION_4 }</p>
      <label className="col s12 m3 pl-0">
        <input id="confirmationCheckBoxId" name="confirmationCheckBox" type="checkbox" className="filled-in" onClick={props.checkBoxChange} />
        <span className="labelPrimaryColor ml-0"></span>      
      </label>
    </Fragment>
  );
};

class AssignmentDetail extends Component {

  constructor(props) {
    super(props);
    existingAssignmentData=props;
    if(this.props.assignmentDetails.AssignmentInterCompanyDiscounts === undefined){
      this.props.assignmentDetails.AssignmentInterCompanyDiscounts = 15;
    }
   ResetCurrentModuleTabInfo(AssignmentTabData);
    this.assignmentTabData = this.filterTabDetail();
    this.callBackFuncs ={
      onCancel:()=>{},
      ARSAssignmentSaveValidation : (isTsScheduleval) => this.mandatoryFieldsValidationCheck(isTsScheduleval),
      onSave:() =>{},//D709,
      NDTAssignmentSaveValidation:() => this.ndtFieldsValidationCheck()
    };
    this.NcrRef = React.createRef();
    this.inputRef = React.createRef();
    this.excelDatas = {	
    };
  }
  state = {
    errorList: [],
    assignedSpecialistConfirmation: false,
    isConfirmAssignedSpecialistOpen: false,
    isAmendment_Reason:false,
    isOpenReportModal:false,
    isShowAssignmentIntercompanyInstructionReport:false,
    isShowSupplierVisitPerformance:false
  };

  errorListButton =[
    {
      name: localConstant.commonConstants.OK,
      action:(e) => this.closeErrorList(e),
      btnID: "closeErrorList",
      btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
      showbtn: true
    }
  ];
  amendmentButtons =[
    {
      name: localConstant.commonConstants.SAVE,
      action:(e) => this.saveAmendment(e),
      btnID: "saveConfirmId",
      btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
      showbtn: true
    },
    {
      name: localConstant.commonConstants.CANCEL,
      action:(e) => this.hideAmendmentModal(e),
      btnID: "cancelConfirmId",
      btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
      showbtn: true,
      type:"button"
    }
  ];
  
  assignedSpecialistButtons = [
    {
      name: localConstant.commonConstants.SAVE,
      action:(e) => this.assignedSpecialistValSaveClick(e),
      btnID: "saveConfirmId",
      btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
      showbtn: false
    },
    {
      name: localConstant.commonConstants.CANCEL,
      action:(e) => this.cancelAssignedSpecialist(e),
      btnID: "cancelConfirmId",
      btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
      showbtn: true,
      type:"button"
    }
  ];
  componentDidMount() {   
    const queryparam = this.props.location.search;  
    const result = queryparam && parseQueryParam(queryparam);
    const isCopyAssignment = false, isFetchLookValues = true;

    if (this.props.location.pathname === '/CreateAssignment') {
      this.props.actions.HandleMenuAction({
        currentPage: localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE,
        currentModule: localConstant.moduleName.ASSIGNMENT
      }).then(res => {
        if (res === true) {
          if (this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE
            && result.isFrom !== "copyAssignment") {
            const projNo = result.projectNumber ? result.projectNumber : this.props.projectNumber;
            this.props.actions.SaveSelectedProjectNumber(projNo);
            const supplierPoInfo={
              supplierPOId:result.supplierPOId,
              supplierPONumber:decodeURIComponent(result.supplierPONumber),
              supplierPOMainSupplierId:result.supplierPOMainSupplierId,
              supplierPOMainSupplierName:decodeURIComponent(result.supplierPOMainSupplierName) // Scenario 159 fixes
            };
            this.props.actions.FetchProjectForAssignmentCreation(supplierPoInfo,isFetchLookValues);
          }
        }
      });
    }

    if (result && result.assignmentId) {
      this.props.actions.SaveSelectedAssignmentId({
        assignmentId: result.assignmentId,
        assignmentNumber: result.assignmentNumber
      });
      this.props.actions.FetchAssignmentDetail(result.assignmentId, isCopyAssignment, isFetchLookValues);
      this.props.actions.UpdateCurrentPage(localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE);
      this.props.actions.UpdateCurrentModule(localConstant.moduleName.ASSIGNMENT);
      // const isViewAllRights = this.props.isViewAllAssignment.length > 0;

      const isViewAllRights = moduleViewAllRights_Modified(securitymodule.ASSIGNMENT, this.props.isViewAllAssignment);
      this.props.actions.SetCurrentPageMode(null,isViewAllRights);
      // this.props.actions.SetCurrentPageMode(null,securitymodule.ASSIGNMENT,isViewAllRights);
    }
    //Fetch assignment module specific master data.
    this.props.actions.FetchGeneralDetailsMasterData();
  }
  
  componentWillUnmount() {
    this.props.actions.OnAssignmentUnmount();
    // To-Do : instead of clearing only details, try reverting the rootAssignmentReducer state to its initial state.
    // Note : it should not clear the assignment Details on copyAssignment scenario.
    this.props.actions.ClearData();
  }
  /** confirmation cancel for the Assigned Specialist validation */
  cancelAssignedSpecialist = (e) => {
    e.preventDefault();
    this.setState({
      isConfirmAssignedSpecialistOpen : false,
    });
  }

  /** Confirm Assigned Specialist check box change handler */
  assignedSpecialistChange = (e) => {
    const result = formInputChangeHandler(e);
    if(result.value){
      this.setState({ assignedSpecialistConfirmation:true });
    }
    else{
      this.setState({ assignedSpecialistConfirmation:false });
    }
  }

  closeErrorList =(e) =>{
    e.preventDefault();
    this.setState({
      errorList:[]
    });
  }

  filterTabDetail = () => {
    const tabsToHide = [];
    const { isTimesheetAssignment, isVisitAssignment } = this.props; 
    if (this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE) {
      tabsToHide.push(...[
        "visits", "timesheets","documents"
      ]);
    }
    if (isVisitAssignment) {
      tabsToHide.push("timesheets");
    }
    if (isTimesheetAssignment) {
      tabsToHide.push("supplierInformation","visits");
    }
    
    const assignmentTabs = arrayUtil.negateFilter(AssignmentTabData, 'tabBody', tabsToHide);
    return assignmentTabs;
  }
  hideAmendmentModal (e){  
    const interCompanyDiscount=this.props.assignmentDetails.AssignmentInterCompanyDiscounts;
    this.setState({
      isAmendment_Reason: false,
      });
      if (this.props.currentPage != localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE) {
      this.cancelAssignment();
      }
  }
  
 saveAmendment=(e)=>{
 const interCompanyDiscount=this.props.assignmentDetails.AssignmentInterCompanyDiscounts;
 amendmentreason=true;
 const data=this.inputRef.current.value;
 interCompanyDiscount['AmendmentReason']=data;
 this.setState({
    isAmendment_Reason: false,
    });
    amendmentreason=true;
    const errors=[];
    if(isEmpty(interCompanyDiscount['AmendmentReason'])){
     errors.push(
       `${ "Amendment Reason" } - ${  "Inter Company Discount" }`
     );
     amendmentreason=false;
   }
   
   if(errors.length > 0){
     this.setState({
       errorList:errors
     });
     return false;
   } 
    this.saveAssignment (e);
 }

  saveAssignment(e) {
    const { techSpecList,assignmentDetails,selectedTechSpec } = this.props;
    let techSpecListTot;
    let techRes;
    const techList =[];
    let modalFlag = false;
    const interCompanyDiscount = this.props.assignmentDetails.AssignmentInterCompanyDiscounts;
    const validDoc = this.uploadedDocumentCheck();
    const valid = this.mandatoryFieldsValidationCheck();
    const getObj = localStorage.getItem("techList");
    const getResource = JSON.parse(getObj);
    let getOverrideResource = null;
    if (getResource !== null) {
      getOverrideResource = getResource.filter(x => x.InActiveDangerTag == true).map(y => y.epin);
    }
    if (valid && validDoc) {
      if (techSpecList.length > 0 && assignmentDetails.AssignmentTechnicalSpecialists && assignmentDetails.AssignmentTechnicalSpecialists.length > 0) {
        for (let i = 0; i < techSpecList.length; i++) {
          for (let j = 0; j < techSpecList[i].resourceSearchTechspecInfos.length; j++) {
            techList.push(techSpecList[i].resourceSearchTechspecInfos[j].epin);
          }
        }
        techSpecListTot = selectedTechSpec.map(x => x.epin);
        techRes = assignmentDetails.AssignmentTechnicalSpecialists.filter(x=>x.recordStatus !=="D").map(x => x.epin).filter(x => techList.includes(x));
        const checkOverride = techRes.filter(x => !techSpecListTot.includes(x));
        if(getOverrideResource !== null && getOverrideResource.length > 0 && checkOverride.length > 0 
          && !checkOverride.some(x => getOverrideResource.includes(x))){
          modalFlag = true;
        }
        if (modalFlag === false && getOverrideResource !== null && techSpecListTot !== null && 
          !techRes.filter(y=>!getOverrideResource.includes(y)).every(x => techSpecListTot.includes(x))){
          modalFlag = true;
        }
      }
      if(techSpecList.length > 0 && modalFlag){
        const confirmationObject = {
          title: modalTitleConstant.CONFIRMATION,
          message: modalMessageConstant.TECH_SPEC_DELETE_ON_ARS_DESELECT,
          type: "confirm",
          modalClassName: "warningToast",
          buttons: [
            {
              buttonName: localConstant.commonConstants.OK,
              onClickHandler: this.confirmationRejectHandler,
              className: "modal-close m-1 btn-small"
            }
          ]
        };
        this.props.actions.DisplayModal(confirmationObject);
      }
      else if (this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE) {
        if ((interCompanyDiscount && interCompanyDiscount.changeCHCDiscount) &&  (this.props.isInterCompanyAssignment) && (interCompanyDiscount.contractType == 'STD' || interCompanyDiscount.contractType == 'FRW' || interCompanyDiscount.contractType == 'IRF')) {
          if (!amendmentreason) {
            this.setState({
              isAmendment_Reason: true,
            });
          }
          if (amendmentreason) {
            amendmentreason = false;
            if (!this.state.assignedSpecialistConfirmation) {
              if ((!isEmpty(this.props.assignmentOldTaxonomy) || !isEmpty(this.props.assignmentOldLifeCycle) || !isEmpty(this.props.assignmentOldSupplierPO)) && this.props.currentPage !== localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE) {
                this.setState({
                  isConfirmAssignedSpecialistOpen: true
                });
              }
              else {
                this.setState({
                  isConfirmAssignedSpecialistOpen: false,
                  assignedSpecialistConfirmation: false
                });
                this.props.actions.SaveAssignmentDetails();
                this.callBackFuncs.onSave();//D709
                localStorage.removeItem("techList");
                if (this.props.isInterCompanyAssignment && this.props.isOperatorCompany)
                  ResetCurrentModuleTabInfo(AssignmentTabData, [ "timesheets", "visits" ]);
              }
            }
            else {
              this.setState({
                isConfirmAssignedSpecialistOpen: false,
                assignedSpecialistConfirmation: false
              });
            }
            ResetCurrentModuleTabInfo(AssignmentTabData, [ "assignedSpecialists" ]);//D546 - Saving the assignment should get updated Visit ot Timesheet Data 
          }
        }
        else {
          if (!this.state.assignedSpecialistConfirmation) {
            if ((!isEmpty(this.props.assignmentOldTaxonomy) || !isEmpty(this.props.assignmentOldLifeCycle) || !isEmpty(this.props.assignmentOldSupplierPO)) && this.props.currentPage !== localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE) {
              this.setState({
                isConfirmAssignedSpecialistOpen: true
              });
            }
            else {
              this.setState({
                isConfirmAssignedSpecialistOpen: false,
                assignedSpecialistConfirmation: false
              });
              this.props.actions.SaveAssignmentDetails();
              this.callBackFuncs.onSave();//D709
              localStorage.removeItem("techList");
              if (this.props.isInterCompanyAssignment && this.props.isOperatorCompany)
                ResetCurrentModuleTabInfo(AssignmentTabData, [ "timesheets", "visits" ]);
            }
          }
          else {
            this.setState({
              isConfirmAssignedSpecialistOpen: false,
              assignedSpecialistConfirmation: false
            });
          }
          ResetCurrentModuleTabInfo(AssignmentTabData, [ "assignedSpecialists" ]);//D546 - Saving the assignment should get updated Visit ot Timesheet Data 
        }

      } else {
        if ( (interCompanyDiscount && interCompanyDiscount.changeCHCDiscount)  &&  (this.props.isInterCompanyAssignment) && ( (this.props.assignmentDetails.AssignmentInfo.assignmentContractType == 'STD' ) || (this.props.assignmentDetails.AssignmentInfo.assignmentContractType == 'FRW' ) || (this.props.assignmentDetails.AssignmentInfo.assignmentContractType == 'IRF' ))) {
          if (!amendmentreason) {
            this.setState({
              isAmendment_Reason: true,
            });
          }
          if (amendmentreason) {
            amendmentreason = false;
            if (!this.state.assignedSpecialistConfirmation) {
              if ((!isEmpty(this.props.assignmentOldTaxonomy) || !isEmpty(this.props.assignmentOldLifeCycle) || !isEmpty(this.props.assignmentOldSupplierPO)) && this.props.currentPage !== localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE) {
                this.setState({
                  isConfirmAssignedSpecialistOpen: true
                });
              }
              else {
                this.setState({
                  isConfirmAssignedSpecialistOpen: false,
                  assignedSpecialistConfirmation: false
                });
                this.props.actions.SaveAssignmentDetails();
                this.callBackFuncs.onSave();//D709
                localStorage.removeItem("techList");
                if (this.props.isInterCompanyAssignment && this.props.isOperatorCompany)
                  ResetCurrentModuleTabInfo(AssignmentTabData, [ "timesheets", "visits" ]);
              }
            }
            else {
              this.setState({
                isConfirmAssignedSpecialistOpen: false,
                assignedSpecialistConfirmation: false
              });
            }
            ResetCurrentModuleTabInfo(AssignmentTabData, [ "assignedSpecialists" ]);//D546 - Saving the assignment should get updated Visit ot Timesheet Data 
          }
        }
        else {
          if (!this.state.assignedSpecialistConfirmation) {
            if ((!isEmpty(this.props.assignmentOldTaxonomy) || !isEmpty(this.props.assignmentOldLifeCycle) || !isEmpty(this.props.assignmentOldSupplierPO)) && this.props.currentPage !== localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE) {
              this.setState({
                isConfirmAssignedSpecialistOpen: true
              });
            }
            else {
              this.setState({
                isConfirmAssignedSpecialistOpen: false,
                assignedSpecialistConfirmation: false
              });
              this.props.actions.SaveAssignmentDetails();
              this.callBackFuncs.onSave();//D709
              localStorage.removeItem("techList");
              if (this.props.isInterCompanyAssignment && this.props.isOperatorCompany)
                ResetCurrentModuleTabInfo(AssignmentTabData, [ "timesheets", "visits" ]);
            }
          }
          else {
            this.setState({
              isConfirmAssignedSpecialistOpen: false,
              assignedSpecialistConfirmation: false
            });
          }
          ResetCurrentModuleTabInfo(AssignmentTabData, [ "assignedSpecialists" ]);//D546 - Saving the assignment should get updated Visit ot Timesheet Data 
        }
      }
    }
  }
 
    assignedSpecialistValSaveClick = (e) => {
    e.preventDefault();
    if (this.state.assignedSpecialistConfirmation){
      this.setState({
        isConfirmAssignedSpecialistOpen: false,
        assignedSpecialistConfirmation: false
      });
      const validDoc = this.uploadedDocumentCheck();
      const valid = this.mandatoryFieldsValidationCheck();
      if (valid && validDoc) {
        this.props.actions.SaveAssignmentDetails();
        ResetCurrentModuleTabInfo(AssignmentTabData,[ "assignedSpecialists" ]);//D546 - Taxanomy Save confirmation
      }
    }
  };

  assignmentCancelHandler = () => {
    const confirmationObject = {
      title: modalTitleConstant.CONFIRMATION,
      message: modalMessageConstant.CANCEL_CHANGES,
      modalClassName: "warningToast",
      type: "confirm",
      buttons: [
        {
          buttonName: "Yes",
          onClickHandler:(e)=> this.cancelAssignment(e),
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
  saveconfirnAssignment=(e)=>{
    const validDoc = this.uploadedDocumentCheck();
    const valid = this.mandatoryFieldsValidationCheck();
    if (valid && validDoc) {
      this.props.actions.SaveAssignmentDetails();
    }
    this.props.actions.HideModal();
  }
  cancelAssignment = (e) => {
    // D - 631
    if(this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE){
      const routeHistory = this.props.history && this.props.history.location;
      const result = this.props.location.search && parseQueryParam(this.props.location.search);
      const supplierPoInfo={
        supplierPOId:result.supplierPOId,
        supplierPONumber:result.supplierPONumber,
        supplierPOMainSupplierId:result.supplierPOMainSupplierId,
        supplierPOMainSupplierName:decodeURIComponent(result.supplierPOMainSupplierName) // Scenario 159 fixes
      };
      if(routeHistory && routeHistory.state && routeHistory.state.isFrom == "copyAssignment")
        this.props.actions.cancelCopyAssignment();
      else{
        this.props.actions.handleAssignedSpecialistTaxonomyLOV({});
        this.props.actions.CancelCreateAssignmentDetails(supplierPoInfo);
      }
    }
    else{
      !isEmpty(this.props.assignmentOldTaxonomy) && this.props.actions.handleAssignedSpecialistTaxonomyLOV(!isEmpty(this.props.taxonomy) && this.props.taxonomy[0]);
      this.props.actions.CancelEditAssignmentDetails();
    }
    this.callBackFuncs.onCancel();
    this.props.actions.HideModal();
    ResetCurrentModuleTabInfo(AssignmentTabData,[ "assignedSpecialists" ]);//D546 - Refreshing the assignment should get updated Visit ot Timesheet Data (mandetory fetch in didmount)
  }

  confirmationRejectHandler = (e) => {
    this.props.actions.HideModal();
  }

  assignmentDeleteHandler = () => {
    let confirmationObject = {};
    if(this.props.isInterCompanyAssignment && this.props.isOperatorCompany){
      confirmationObject = {
        title: modalTitleConstant.CONFIRMATION,
        message: modalMessageConstant.OP_ASSIGNMENT_DELETE_MESSAGE,
        modalClassName: "warningToast",
        type: "confirm",
        buttons: [
          {
            buttonName: localConstant.commonConstants.OK,
            onClickHandler: this.confirmationRejectHandler,
            className: "modal-close m-1 btn-small"
          }
        ]
      };
    }
    else {
      confirmationObject = {
        title: modalTitleConstant.CONFIRMATION,
        message: modalMessageConstant.ASSIGNMENT_DELETE_MESSAGE,
        modalClassName: "warningToast",
        type: "confirm",
        buttons: [
          {
            buttonName: "Yes",
            onClickHandler: this.deleteAssignment,
            className: "modal-close m-1 btn-small"
          },
          {
            buttonName: "No",
            onClickHandler: this.confirmationRejectHandler,
            className: "modal-close m-1 btn-small"
          }
        ]
      };
    }
    this.props.actions.DisplayModal(confirmationObject);
  }

  deleteAssignment = () => {
    this.props.actions.HideModal();
    this.props.actions.DeleteAssignment()
      .then(response => {
        if (response) {
          if (response.code == 1) {
            this.props.history.push('/EditAssignment');
            this.props.actions.DeleteAlert(response.result, "Assignment");
          }
        }
      });
  }
  changeAmdmentReasonTextArea(e){
    amdmentReasonData=e.target.value;
    }

  assignmentCopyHandler(e) {
    if (!this.props.isbtnDisable) {
      const confirmationObject = {
        title: modalTitleConstant.CONFIRMATION,
        message: modalMessageConstant.ASSIGNMENT_COPY_CONFIRM,
        modalClassName: "warningToast",
        type: "confirm",
        buttons: [
          {
            buttonName: "Yes",
            onClickHandler: (e) => this.cancelChangesAndCopyAssignment(e),
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
    else {
      this.copyAssignment();
    }
  }

  async cancelChangesAndCopyAssignment() {
    const res = await this.props.actions.CancelEditAssignmentDetails(true);
    if (!isEmpty(res)) {
      this.copyAssignment();
    }else{
      this.props.actions.HideModal();
    }
  }
  copyAssignment() { 
    this.props.actions.copyAssignment();
    this.props.actions.HideModal();
    this.props.history.push({
      pathname: '/createAssignment',
      state: {
        isFrom: "copyAssignment"
      },
      search: `?isFrom=copyAssignment`
    });
  }

  validateContributionCalculator = (contributionCalculator)=>{
        const ContributionCalculatorObj  = isEmptyReturnDefault(contributionCalculator[0],'object');
        const assignmentContributionRevenueCosts  = isEmptyReturnDefault(ContributionCalculatorObj.assignmentContributionRevenueCosts);
        const revenueData = assignmentContributionRevenueCosts.filter(eachItem =>eachItem.type ==='A');
        //Bill Rate Cannot be Zero or Empty. Please  enter the value for Bill Rate!!!
        if (isEmpty(revenueData)) {
          return false;
        }

        if (!isEmpty(revenueData)) {
          for (let i = 0; i < revenueData.length; i++) {
              const revenueDesc = revenueData[i].description && revenueData[i].description.trim();
              const revenueValue = revenueData[i].value ? parseFloat(revenueData[i].value) : 0;
              if (revenueDesc.toLowerCase() === ("Bill Rate").toLowerCase() && revenueValue !== 0) {
                return true;
              }
              else if (revenueDesc.toLowerCase() === ("Bill Rate").toLowerCase() && revenueValue === 0) {
                return false;
              }      
          }
      }
      return true;
  }

  uploadedDocumentCheck=()=>{  // when file Uploading user click on sve button showing warning Toaster Here 
    let count = 0;
    if (Array.isArray(this.props.documentrowData) && this.props.documentrowData.length > 0) {
        this.props.documentrowData.map(document =>{                             
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
//Sanity Defect-188 -09-Oct-2020
ndtFieldsValidationCheck() {
  let assignmentGeneralDetails = {};
  assignmentGeneralDetails = this.props.assignmentDetails.AssignmentInfo;
  let fromDate="",toDate="";
  //To-do: this code to be improved this is doen for hot fix
  if(this.props.isTimesheetAssignment){
    //TIMESHEET_DETAILS
    fromDate = "timesheetFromDate";
    toDate ="timesheetToDate";
  }
  if(this.props.isVisitAssignment){
      //Visit Details
      fromDate = "visitFromDate";
      toDate = "visitToDate";
  }
  const errors=[];
  if ((this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE && 
        this.props.isInterCompanyAssignment === false) || 
        (this.props.isInterCompanyAssignment === true && 
          this.props.isOperatorCompany === true)) {
    if (required(assignmentGeneralDetails[fromDate])) {
      errors.push(
        `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.validationMessage.ASSIGNMENT_DATE_FROM_VALIDATION }`
      );
    }
    if (required(assignmentGeneralDetails[toDate])) {
      errors.push(
        `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.validationMessage.ASSIGNMENT_DATE_TO_VALIDATION }`
      );
    }
  }
  if(errors.length > 0){
    this.setState({
      errorList:errors
    });
    return false;
  }
  return true;
}

  mandatoryFieldsValidationCheck = (skipTSScheduleVal) => {
    const taxonomy  = this.props.taxonomy[0] ?this.props.taxonomy[0]:{};
    let assignmentGeneralDetails = {};    
    assignmentGeneralDetails = this.props.assignmentDetails.AssignmentInfo;
    const projectStartDate = this.props.dataToValidateAssignment.fromDate;
    const projectEndDate = this.props.dataToValidateAssignment.toDate;
    const interCompanyDiscount=this.props.assignmentDetails.AssignmentInterCompanyDiscounts;
    const { isTimesheetAssignment,
            isSettlingTypeMargin, 
            isVisitAssignment, 
            assignmentContributionCalculator,
            isInterCompanyAssignment,
            isOperatorCompany } = this.props;
    const assignmentReferences=this.props.assignmentDetails.AssignmentReferences;
    const assignmentContractSchedules = isEmptyReturnDefault(this.props.assignmentDetails.AssignmentContractSchedules).filter(x =>x.recordStatus !== 'D');
    let fromDate="",toDate="";
    //To-do: this code to be improved this is doen for hot fix
    if(isTimesheetAssignment){
        //TIMESHEET_DETAILS
        fromDate = "timesheetFromDate";
        toDate ="timesheetToDate";
      }
      if(isVisitAssignment){
          //Visit Details
          fromDate = "visitFromDate";
          toDate = "visitToDate";
      }

      const errors=[];
    if (!isEmpty(assignmentGeneralDetails)) {       
      /**General Details Validation Starts */
      if (required(assignmentGeneralDetails.assignmentType)) {
        errors.push(
          `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.validationMessage.ASSIGNMENT_TYPE }`
        );
      }
      if(requiredNumeric(assignmentGeneralDetails.assignmentBudgetWarning)){ //Changes for D1369 
        errors.push(
          `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.budget.BUDGET_MONETARY_WARNING }`
        );
      }
      if(requiredNumeric(assignmentGeneralDetails.assignmentBudgetHoursWarning)){
        errors.push(
          `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.budget.BUDGET_HOURS_WARNING }`
        );
      }
      if (isVisitAssignment && required(assignmentGeneralDetails.assignmentSupplierPurchaseOrderId)) {
        errors.push(
          `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.validationMessage.SUPPLIER_PURCHASE_ORDER_NUMBER }`
        );
      }
      if (isVisitAssignment && required(assignmentGeneralDetails.assignmentReviewAndModerationProcess)) {
        errors.push(
          `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.validationMessage.REVIEW_AND_MODERATION_PROCESS_SELECT }`
        );
      }
      if (required(assignmentGeneralDetails.assignmentContractHoldingCompanyCoordinator)) {
        errors.push(
          `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.validationMessage.CONTRACT_HOLDING_COMPANY_COORDINATOR }`
        );
      }
      if((!isInterCompanyAssignment || (isInterCompanyAssignment && isOperatorCompany)) && required(assignmentGeneralDetails.assignmentOperatingCompanyCoordinator)){
        errors.push(
          `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.validationMessage.OPERATING_COMPANY_COORDINATOR }`
        );
      }
      if (requiredNumeric(assignmentGeneralDetails.assignmentBudgetValue)) {
        errors.push(
          `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.validationMessage.BUDGET_MONETARY_VALUE_VAL }`
        );
      }
      if (requiredNumeric(assignmentGeneralDetails.assignmentBudgetHours)) {
        errors.push(
          `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.validationMessage.BUDGET_HOURS_UNIT_VAL }`
        );
      }
      if (required(assignmentGeneralDetails.assignmentLifecycle)) {
        errors.push(
          `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.validationMessage.ASSIGNMENT_LIFE_CYCLE }`
        );
      }
      if (required(assignmentGeneralDetails.assignmentCompanyAddress)) {
        errors.push(
          `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.validationMessage.ASSIGNMENT_COMPANY_ADDRESS }`
        );
      }
      if (required(assignmentGeneralDetails.assignmentCustomerAssigmentContact)) {
        errors.push(
          `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.validationMessage.CUSTOMER_ASSIGNMENT_CONTACT }`
        );
      }
      if (isTimesheetAssignment) {
        if (required(assignmentGeneralDetails.workLocationCountry)) {
          errors.push(
            `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.validationMessage.ASSIGNMENT_COUNTRY }`
          );
        }
        if (required(assignmentGeneralDetails.workLocationCounty)) {
          errors.push(
            `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.validationMessage.ASSIGNMENT_STATE }`
          );
        }
        if (isEmpty(assignmentGeneralDetails.workLocationCity) && isEmpty(assignmentGeneralDetails.workLocationPinCode)) {
          errors.push(
                    `${ localConstant.supplier.GENERAL_DETAILS } - ${ localConstant.supplier.CITY } 
                    or ${ localConstant.supplier.POSTAL_CODE }`
                     );
      }
      }
      if ((this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE && this.props.isInterCompanyAssignment === false) || (this.props.isInterCompanyAssignment === true && this.props.isOperatorCompany === true)) {
          if (required(assignmentGeneralDetails[fromDate])) {
            errors.push(
              `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.validationMessage.ASSIGNMENT_DATE_FROM_VALIDATION }`
            );
          }
          if (required(assignmentGeneralDetails[toDate])) {
            errors.push(
              `${ localConstant.contract.GENERAL_DETAILS } - ${ localConstant.validationMessage.ASSIGNMENT_DATE_TO_VALIDATION }`
            );
          }
        } 
      
      /**General Details Validation Ends */
      /** Assignment Sub Suppleir Validation Starts */
       if(isVisitAssignment){
         let throwContactValidation=false;
         let throwVisitValidation=false;
        if(this.props.assignmentSubSuppliers.length>0){
          const filteredSubSuppliers = this.props.assignmentSubSuppliers.filter(x=>x.recordStatus !== 'D');
          if(filteredSubSuppliers && filteredSubSuppliers.length > 0){
            if(required(filteredSubSuppliers[0].mainSupplierContactName)){
              errors.push(
                `${ localConstant.assignments.SUPPLIER_INFORMATON } - ${ localConstant.validationMessage.MAIN_SUPPLIER_CONTACT_REQUIRED }`
              );
            }
            filteredSubSuppliers.forEach(items=>
              {
                if(items.subSupplierId){
                  if(required(items.subSupplierContactName))
                  {     
                   throwContactValidation=true;
                  }
                  else{
                    throwContactValidation=false;
                  }  
                }
              
              });
              //MS-TS NOV 23 
                const data=filteredSubSuppliers.filter( x=>(x.isMainSupplierFirstVisit=== true) || (x.isMainSupplierFirstVisit=== false && x.isSubSupplierFirstVisit=== true) );
                const assignmentTScount=this.props.assignmentDetails.AssignmentTechnicalSpecialists;
                if(data.length > 0 || (assignmentTScount && assignmentTScount.length > 0 ))
                {
                 throwVisitValidation=false;
                }else{
                  throwVisitValidation=true;
                }
              //MS-TS NOV 23 
          }
        }
        if(throwContactValidation)
        {
                 errors.push(
                        `${ localConstant.assignments.SUPPLIER_INFORMATON } - ${ localConstant.validationMessage.SUB_SUPPLIER_CONTACT_VAL }`
                              );
        }
        if(throwVisitValidation)
        {
          errors.push(
            `${ localConstant.assignments.SUPPLIER_INFORMATON } - ${ localConstant.validationMessage.SUPPLIER_FIRST_VISIT_VALIDATION }`
                  );
        }
      }
      /** Assignment Sub Suppleir Validation Ends */

      /**Assignment Contract Schedules Validation Starts */
      if (isEmpty(assignmentContractSchedules)) {
        errors.push(
          `${ localConstant.assignments.CONTRACT_RATE_SCHEDULES } - ${ localConstant.validationMessage.ASSIGNMENT_CONTRACT_RATE_SCHEDULE }`
        );
      }
      /**Assignment Contract Schedules Validation Ends */
      
      /**Assignment References Validation Starts */
      if (assignmentReferences) {
        for (let i = 0; i < assignmentReferences.length; i++) {
          if (required(assignmentReferences[i].referenceValue)) {
            errors.push(`${ localConstant.assignments.ASSIGNMENT_REFERENCE } - ${ localConstant.assignments.PLEASE_FILL_REFERENCE_VALUE }`
            );
            break;
          }
        }
      }  
      /**Assignment References Validation Ends */ 
      
      /** Assigned Specialist Validation Starts */
        if(this.props.isContractHolderCompany){
          if(required(taxonomy.taxonomyCategory)){
            errors.push(
              `${ localConstant.assignments.Assigned_Specialists } - ${ localConstant.validationMessage.ASSIGNMENT_TAXONOMY_CATEGORY_VAL }`
            );
          }
          if(required(taxonomy.taxonomySubCategory)){
            errors.push(
              `${ localConstant.assignments.Assigned_Specialists } - ${ localConstant.validationMessage.ASSIGNMENT_TAXONOMY_SUB_CATEGORY_VAL }`
            );
          }
          if(required(taxonomy.taxonomyService)){
            errors.push(
              `${ localConstant.assignments.Assigned_Specialists } - ${ localConstant.validationMessage.ASSIGNMENT_TAXONOMY_SERVICES }`
            );
          }
      }
      
     /** inter company discounts validation */
     if((this.props.isCoordinator && this.props.isContractHolderCompany) || this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE){
      if( !isEmpty(interCompanyDiscount) && !isEmpty(interCompanyDiscount.assignmentAdditionalIntercompany1_Name)){
        if(required(interCompanyDiscount.assignmentAdditionalIntercompany1_Discount)){
          errors.push(
            `${ localConstant.assignments.InterCompanyDiscounts } - ${  localConstant.validationMessage.ADDITIONALINTERCO1_OFFICE }`
          );
        }
        if(required(interCompanyDiscount.assignmentAdditionalIntercompany1_Description)){
         errors.push(
            `${ localConstant.assignments.InterCompanyDiscounts } - ${  localConstant.validationMessage.ADDITIONALINTERCO1_OFFICE_DES }`
          );
        }
        }

      if( !isEmpty(interCompanyDiscount) && !isEmpty(interCompanyDiscount.assignmentAdditionalIntercompany2_Name)){
      if(required(interCompanyDiscount.assignmentAdditionalIntercompany2_Discount)){
        errors.push(
          `${ localConstant.assignments.InterCompanyDiscounts } - ${  localConstant.validationMessage.ADDITIONALINTERCO2_OFFICE }`
        );
      }
      if(required(interCompanyDiscount.assignmentAdditionalIntercompany2_Description)){
        errors.push(
          `${ localConstant.assignments.InterCompanyDiscounts } - ${  localConstant.validationMessage.ADDITIONALINTERCO2_OFFICE_DES }`
        );
      }
    }
  
    if( this.props.assignmentDetails.AssignmentInfo.assignmentContractHoldingCompany !==this.props.assignmentDetails.AssignmentInfo.assignmentOperatingCompany){
      if(requiredNumeric(interCompanyDiscount.assignmentContractHoldingCompanyDiscount)){
        errors.push(
          `${ localConstant.assignments.InterCompanyDiscounts } - ${  localConstant.validationMessage.CONTRACTHOLDING_COMPANY_DISCOUNT }`
        );
      }
      if(required(interCompanyDiscount.assignmentContractHoldingCompanyDescription)){
        
        errors.push(
          `${ localConstant.assignments.InterCompanyDiscounts } - ${  localConstant.validationMessage.CONTRACTHOLDING_COMPANY_DES }`
        );
      }
     }
    if(!isEmpty( this.props.assignmentDetails.AssignmentInfo.assignmentParentContractCompany)){
      if(required(interCompanyDiscount.parentContractHoldingCompanyDescription)){
        
        errors.push(
          `${ localConstant.assignments.InterCompanyDiscounts } - ${  localConstant.validationMessage.PARENTCONTRACT_DES }`
        );
      }
    }

    if(!isEmpty( this.props.assignmentDetails.AssignmentInfo.assignmentHostCompany) 
     && isSettlingTypeMargin // ITK D - 712 & 715
     && this.props.assignmentDetails.AssignmentInfo.assignmentOperatingCompanyCode !== this.props.assignmentDetails.AssignmentInfo.assignmentHostCompanyCode){
      
      if(required(interCompanyDiscount.assignmentHostcompanyDiscount)){
        errors.push(
          `${ localConstant.assignments.InterCompanyDiscounts } - ${  localConstant.validationMessage.HOST_COMPANY_DISCOUNT }`
        );
      }
      if(required(interCompanyDiscount.assignmentHostcompanyDescription)){
        errors.push(
          `${ localConstant.assignments.InterCompanyDiscounts } - ${  localConstant.validationMessage.HOST_COMPANY_DES }`
        );
      }
    }
    }
      if (isTimesheetAssignment && isSettlingTypeMargin
        && assignmentGeneralDetails.assignmentOperatingCompanyCode !== assignmentGeneralDetails.assignmentContractHoldingCompanyCode
        && ((this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE)
          || (this.props.isOperator && this.props.isOperatorCompany))) {
        //check if contribution calculator bill rate is zero
        const isValid = this.validateContributionCalculator(assignmentContributionCalculator);
        if (!isValid) {
          errors.push(
            `${ localConstant.assignments.InterCompanyDiscounts } - ${ localConstant.validationMessage.ENTER_CONTRIBUTION_CALCULATOR }`
          );
        }
      }
//Added for D634 issue 2
      if(this.props.isOperatorCompany && !skipTSScheduleVal){
        const assignedSpecialists = Object.assign([],this.props.assignmentDetails.AssignmentTechnicalSpecialists);
        if (assignedSpecialists.length >0) {
          let hasNoSchedule = false;
          assignedSpecialists.forEach(iteratedValue => {
            const schedule = iteratedValue.recordStatus !== 'D' && iteratedValue.assignmentTechnicalSpecialistSchedules && iteratedValue.assignmentTechnicalSpecialistSchedules.filter(x => x.recordStatus !== "D");
            if (schedule && schedule.length === 0) {
              iteratedValue.scheduleValidation=localConstant.validationMessage.TECHSPEC_SHOULDHAVE_ATLEAST_ONE_SCHEDULE;
              hasNoSchedule = true;
            } else {
              iteratedValue.scheduleValidation="";
            }
          });
          this.props.actions.AssignTechnicalSpecialist(assignedSpecialists);
          if (hasNoSchedule) {
            errors.push(
              `${ localConstant.assignments.Assigned_Specialists } - ${ localConstant.assignments.Assigned_Resources_GRID_MANDATORY_FIELDS }`
            );
          }
        }
      }
      if(!isEmpty(this.props.documentrowData)){
        const issueDoc = [];
        this.props.documentrowData.map(document =>{
          if(document.recordStatus!=='D')
          {
            if (isEmpty(document.documentType)) {
                errors.push(`${ localConstant.assignments.documents.DOCUMENT } - ${ document.documentName } - ${ localConstant.assignments.documents.SELECT_FILE_TYPE } `);
           }
          //  if(document.documentType === "Evolution Email" && (document.recordStatus === "N" || document.recordStatus === "M")){
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
     if(errors.length > 0){
        this.setState({
          errorList:errors
        });
        return false;
      } 
      if(this.props.isSupplierPOChanged){
        this.setState({
          isConfirmAssignedSpecialistOpen:false
        });
      }else {
        if ((this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE && this.props.isInterCompanyAssignment === false) || (this.props.isInterCompanyAssignment === true && this.props.isOperatorCompany === true)) {
          if (required(assignmentGeneralDetails[fromDate])) {
            IntertekToaster(localConstant.validationMessage.ASSIGNMENT_DATE_FROM_VALIDATION, 'warningToast AssDetfrmDt');
            return false;
          }
          if (projectStartDate && isEmpty(projectEndDate)) {
            if (moment(projectStartDate).isAfter(assignmentGeneralDetails[fromDate], 'day')) {
              IntertekToaster(localConstant.validationMessage.ASSIGNMENT_FROM_DATE_MISMATCH, 'warningToast AssFrmDtMisMat');
              return false;
            }
          }
          if (required(assignmentGeneralDetails[toDate])) {
            IntertekToaster(localConstant.validationMessage.ASSIGNMENT_DATE_TO_VALIDATION, 'warningToast AssDetToDt');
            return false;
          }
          if (projectEndDate) {
            if (!dateUtil.dateInBetween(assignmentGeneralDetails[fromDate], projectStartDate, projectEndDate)) {
              IntertekToaster(localConstant.validationMessage.ASSIGNMENT_DATE_FROM_IN_BETWEEN_VALIDATION, 'warningToast AssDetDtFrmInBtwnValid');
              return false;
            }
            if (assignmentGeneralDetails[toDate]) {
              if (!dateUtil.dateInBetween(assignmentGeneralDetails[toDate], projectStartDate, projectEndDate)) {
                IntertekToaster(localConstant.validationMessage.ASSIGNMENT_DATE_TO_IN_BETWEEN_VALIDATION, 'warningToast AssDetDtToInBtwnValid');
                return false;
              }
            }
          }
            if (assignmentGeneralDetails[toDate]) {
              if (moment(assignmentGeneralDetails[fromDate]).isAfter(assignmentGeneralDetails[toDate], 'day')) {
                IntertekToaster(localConstant.validationMessage.ASSIGNMENT_DATE_FROM_DATE_TO_MISMATCH, 'warningToast AssDetDtFrmDtToMismatch');
                return false;
              }
            }
          
          else {
            if (required(assignmentGeneralDetails[toDate])) {
              IntertekToaster(localConstant.validationMessage.ASSIGNMENT_DATE_TO_VALIDATION, 'warningToast AssDetToDt');
              return false;
            }
          }
        }
        if(assignmentGeneralDetails.assignmentBudgetWarning > 100 )
        {
          IntertekToaster(localConstant.budget.BUDGET_HOURS_MESSAGE,'warningToast');
          return false;
        }

        if(assignmentGeneralDetails.assignmentBudgetHoursWarning > 100)
        {
          IntertekToaster(localConstant.budget.BUDGET_MONETARY_MESSAGE,'warningToast');
          return false;
        }
      }
    }
    return true;
  } 
  
  arsSerchResourceSaveBtn=()=>{
    this.props.actions.ARSSearchPanelStatus(false);
  }
  handleAssignmentsButtons () {
    const conditionalButtons = [
      localConstant.commonConstants.DELETE,
      localConstant.commonConstants.REFRESH,
      localConstant.commonConstants.COPY_ASSIGNMENT
    ];
    if (this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE) {
      return arrayUtil.negateFilter(this.assignmentsButtons, 'name', conditionalButtons);
    }
    else {
      const { AssignmentInfo ={} } = this.props.assignmentDetails;
      this.assignmentsButtons.forEach((eachBth)=>{
        if (conditionalButtons.includes(eachBth.name)
          && this.props.interactionMode === false) {
          eachBth.showBtn = true;
          if (eachBth.name === localConstant.commonConstants.COPY_ASSIGNMENT) {
            if (AssignmentInfo.assignmentContractHoldingCompanyCode !== this.props.selectedCompany) {
              eachBth.className = 'btn-small mr-0 ml-2 waves-effect modal-trigger disabled';
            } else {
              eachBth.className = 'btn-small mr-0 ml-2 waves-effect modal-trigger';
            }
          }
        } else {
          eachBth.showBtn = false;
        }
      },this);
      
    }

    return this.assignmentsButtons;
  };
  reportsClickHandler=()=>{

    this.setState( { isOpenReportModal : true } );

  }

  HideModal=()=>
  {
    this.setState( { isOpenReportModal: false } );
    this.setState( { isShowSupplierVisitPerformance:false } );
    this.props.actions.ClearCustomerData(); //Changes for IGO - D901
  }
  //to set state based on the Reports
  onReportChange =(e)=>
  {
    const result = formInputChangeHandler(e);
    if(result.value==='Assignment Inter Company Instruction Report')
    {
      this.setState( { isShowAssignmentResourceInstructionReport:false } );
      this.setState( { isShowAssignmentIntercompanyInstructionReport:true } );
      this.setState( { isShowSupplierVisitPerformance : false } );
    }
    else if(result.value==='Supplier Visit Performance Report' ){
      this.setState( { isShowAssignmentResourceInstructionReport:false } );
      this.setState( { isShowSupplierVisitPerformance : true } );
      this.setState( { isShowAssignmentIntercompanyInstructionReport:false } );
    }
    else if(result.value ==='Assignment Resource Instruction Report')
    {
      this.setState( { isShowAssignmentResourceInstructionReport:true } );
      this.setState( { isShowSupplierVisitPerformance : false } );
      this.setState( { isShowAssignmentIntercompanyInstructionReport:false } );
    }
    else{
      this.setState( { isShowAssignmentResourceInstructionReport:false } );
      this.setState( { isShowSupplierVisitPerformance : false } );
      this.setState( { isShowAssignmentIntercompanyInstructionReport:false } );
    }

  }
  //To Download Report
  
  GetReportHandler = async (e) => {
    let params = {};
    let isError = false;
    let reportFileName = '';
    let extension = '.xls';
    let content_Type = 'application/vnd.ms-excel';
    const _localStorage = localStorage.getItem('Username');
    
    if (this.state.isShowAssignmentIntercompanyInstructionReport) {
      //TO Check Whether Assignment is Intercompany or Not. Generates Report only for Intercompany Assignment--
      if (this.props.isInterCompanyAssignment) {

        params = {
          'AssignmentId': this.props.assignmentDetails.AssignmentInfo.assignmentId,
          'format': 'PDF'
        };
        extension = '.pdf';
        content_Type = 'application/pdf';
        params['reportname'] = EvolutionReportsAPIConfig.AssignmentInterCompanyInstructionReports;
        reportFileName = 'AssignmentInterCompanyInstructionReport';
      }
      else {
        isError = true;
        IntertekToaster(localConstant.validationMessage.INTERCOMPANY_ASSIGNMENT_REQUIRED, 'warningToast AssDetToDt');
      }
    }
    else if (this.state.isShowAssignmentResourceInstructionReport) {
      params = {
        'AssignmentId': this.props.assignmentDetails.AssignmentInfo.assignmentId,
        'format': 'PDF'
      };
      extension = '.pdf';
      reportFileName = 'AssignmentTechSpecInstructionReport';
      content_Type = 'application/pdf';
      params['reportname'] = EvolutionReportsAPIConfig.AssignmentTechSpecInstructionReport;
    }
    else if (this.state.isShowSupplierVisitPerformance) {
      // to do-- pass the required Parameters //
      if (this.NcrRef.current.checked === true) {  // Made Open as default checked as per D-869//
        this.excelDatas['NCR'] = 'open';
      }
      params = this.excelDatas;
      params['format'] = 'EXCEL';
      reportFileName = 'SupplierVisitPerformanceReport';
      params['reportname'] = EvolutionReportsAPIConfig.SupplierVisitPerformanceReport;
      params['background'] = true;
      params['reptype'] = 4;
      const SupplierName = this.excelDatas['SupplierName'];
      const CustomerName = this.excelDatas['CustomerName'];
      const CHCompanyName = this.excelDatas['CHCompanyName'];
      const OCCompanyName = this.excelDatas['OCCompanyName'];
      const AssignmentNumber = this.excelDatas['AssignmentNumber'];
      const SupplierPONumber = this.excelDatas['SupplierPONumber'];
      const ProjectNumber = this.excelDatas['ProjectNumber'];
      if (SupplierName == undefined && CustomerName == undefined && CHCompanyName == undefined && OCCompanyName == undefined && AssignmentNumber == undefined && SupplierPONumber == undefined && ProjectNumber == undefined) {
        IntertekToaster(localConstant.commonConstants.ERR_REPORT, 'warningToast ssrs_report_message');
        isError = true;
      }
    }
    
    if(this.state.isShowAssignmentIntercompanyInstructionReport || this.state.isShowSupplierVisitPerformance || this.state.isShowAssignmentResourceInstructionReport){
      if(!isError){
        params['username'] = _localStorage;
        DownloadReportFile(params, content_Type, reportFileName, extension, this.props);
      }
    }
    
  }

  supplierPopupOpen = (data) => {
    this.props.actions.ClearSupplierSearchList();
  }

  getMainSupplier = (data) => {

      const params = {
          supplierName: data.serachInput,
          country: data.selectedCountry
      };
      this.props.actions.FetchSupplierSearchList(params);
  }

  getSelectedMainSupplier = (data) => {
     
      if (data) {
          const params = {
              supplierPOMainSupplierName: data[0] && data[0].supplierName,
              supplierPOMainSupplierAddress: data[0] && data[0].supplierAddress,
              supplierPOMainSupplierId: data[0] && data[0].supplierId
          };
          this.excelDatas['supplierName'] = data[0] && data[0].supplierName;
          this.props.actions.updateSupplierDetails(params);       
      }   
  }

  handlerChange = (e) => {
    if (e.target.value != '') {
      this.excelDatas[e.target.name] = e.target.value;
    }
    else {
      this.excelDatas[e.target.name] = undefined;
    }    
  }

  clearSupplierVisit = () => {
    this.excelDatas['SupplierName'] = undefined;
  }

  handleSupplier = (e) => {
    if (e) {
      this.excelDatas['SupplierName'] = e[0].supplierName;
    }
    else {
      this.excelDatas['SupplierName'] = undefined;
    }
  }

  OnChangeSearchCustomer =(e) =>
  {
    if (e) {
      this.excelDatas['CustomerName'] = e[0].customerName;
    }
    else {
      this.excelDatas['CustomerName'] = undefined;
    }
    this.props.actions.UpdateReportCustomer(e);
  }

  ClearReportsData =()=>
  {
      this.props.actions.ClearReportsData();
  }

  render() {
    
    const { AssignmentInfo = {} } = this.props.assignmentDetails,
      { assignmentNumber } = AssignmentInfo;
    const isInEditMode = (this.props.interactionMode === false && this.props.currentPage === localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE);
    const isInViewMode = (this.props.pageMode === localConstant.commonConstants.VIEW) ? true : false;
    const isInCreateMode = (this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE);
    const response = ButtonShowHide(isInEditMode, isInViewMode, isInCreateMode);
    let interactionMode = this.props.interactionMode;
    const { companyList } = this.props;
    const sortedCompanyList = arrayUtil.sort(companyList, 'companyName', 'asc');
    const defaultSort = [
      {
        "colId": "supplierName",
        "sort": "asc"
      },
    ];
    if (this.props.pageMode === localConstant.commonConstants.VIEW) {
      interactionMode = true;
    }
    const  assignmentsButtons = [
        {
          name: localConstant.commonConstants.SAVE,
          clickHandler: (e) => this.saveAssignment(e),
          className: "btn-small mr-0 ml-2",
          permissions:[ activitycode.NEW,activitycode.MODIFY ],
          isbtnDisable: this.props.isbtnDisable ,
          showBtn:response[0]
        }, 
        {
          name: localConstant.commonConstants.REFRESHCANCEL,
          clickHandler: (e) => this.assignmentCancelHandler(e),
          className: "btn-small mr-0 ml-2 waves-effect modal-trigger",
          permissions:[ activitycode.NEW,activitycode.MODIFY ],
          isbtnDisable: this.props.isbtnDisable,
          showBtn:response[0]
        },
        {                                    
          name: localConstant.commonConstants.DELETE,
          clickHandler: (e) => this.assignmentDeleteHandler(e),
          className: "btn-small btn-primary mr-0 ml-2 dangerBtn modal-trigger waves-effect",
          permissions:[ activitycode.DELETE ],
          showBtn:response[1] 
        },
        {                                    	
          name: localConstant.commonConstants.REPORTS,	
          clickHandler: this.reportsClickHandler,	
          className: "btn-small mr-0 ml-2",	
          showBtn:this.props.currentPage === localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE && true,	
        },
        {
          name: localConstant.commonConstants.COPY_ASSIGNMENT,
          clickHandler: (e) => this.assignmentCopyHandler(e),
          className: "btn-small mr-0 ml-2 waves-effect modal-trigger",
          permissions:[ activitycode.NEW ],
          showBtn:response[1]  
        }
      ];
      assignmentsButtons.map((btn, i) => {
        if (this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE) {
          return arrayUtil.negateFilter(assignmentsButtons, 'name', [
            localConstant.commonConstants.DELETE,
            localConstant.commonConstants.REFRESH,
            localConstant.commonConstants.COPY_ASSIGNMENT
          ]);
        }
        else {
          const { AssignmentInfo ={} } = this.props.assignmentDetails;
          if(this.props.isInterCompanyAssignment && this.props.isOperatorCompany){
            if(btn.name === localConstant.commonConstants.COPY_ASSIGNMENT){
              btn.showBtn = false;
            }
          }
          if (AssignmentInfo.assignmentContractHoldingCompanyCode !== this.props.selectedCompany && (this.props.pageMode===localConstant.commonConstants.VIEW)) {
            return arrayUtil.negateFilter(assignmentsButtons, 'name', [
              localConstant.commonConstants.COPY_ASSIGNMENT
            ]);
          } 
        }
        return this.assignmentsButtons;
      });
    if (!isEmpty(AssignmentInfo)) {
      this.assignmentTabData = this.filterTabDetail();
    }
    if(this.state.assignedSpecialistConfirmation){
      this.assignedSpecialistButtons[0].showbtn = true;
    }
    else{
      this.assignedSpecialistButtons[0].showbtn = false;
    }
    return (
      <Fragment>
         { this.state.isOpenReportModal ?   <Modal
        title="Assignment Reports"
         isShowModal={this.state.isOpenReportModal}
         modalClass="bold"
         buttons={[
             {
                 name: localConstant.commonConstants.GET_REPORT,
                 type: "button",
                 action: this.GetReportHandler,
                 btnClass: 'btn-small mr-0 ml-2 waves-effect modal-trigger',
                 showbtn: true
             },
             {
              name: localConstant.commonConstants.CANCEL,
              type: "button",
              action: this.HideModal,
              btnClass: 'btn-small mr-0 ml-2 waves-effect modal-trigger',
              showbtn: true
          }
         ]}
         >
          
           <div className="row mb-0">
               <CustomInput hasLabel={true}
                name="action"
                id="report"
                label={ localConstant.commonConstants.REPORTS_LIST }
                type='select'
                colSize='s4'
                inputClass="customInputs"
                optionsList={ localConstant.commonConstants.assignmentReportList}
                optionName='name'
                optionValue="value"
                className="browser-default"
                onSelectChange={ this.onReportChange }
            />

             </div>
             { this.state.isShowSupplierVisitPerformance ?
                < SupplierVisitSearchFields
                    handlerChange={ this.handlerChange }
                    handleSupplier={this.handleSupplier}
                    supplierList={this.props.supplierList }
                    sortedCompanyList={ sortedCompanyList }
                    OnChangeSearchCustomer={this.OnChangeSearchCustomer}
                    cancelSupplierVisit={this.clearSupplierVisit}
                    clearSupplierVisit={this.clearSupplierVisit}
                    defaultSort={ defaultSort }
                    supplierPopupOpen={ data => this.supplierPopupOpen(data) } 
                    getMainSupplier={data=> this.getSelectedMainSupplier(data) }
                    getSelectedMainSupplier={ data=>this.getSelectedMainSupplier(data) }
                    headerData={ headerData }
                    reportsCustomerName={this.props.reportsCustomerName}
                    ClearReportsData={ this.ClearReportsData }
                    NcrRef={this.NcrRef}
                    defaultReportCustomerName={this.props.defaultReportCustomerName }
                />
                 :null  }
                       
         </Modal> :null }
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
        {this.state.isConfirmAssignedSpecialistOpen ? <Modal title = { localConstant.assignments.CONFIRM_ASSIGNED_SPECIALIST }
          modalId = "assignedSpecialistCnfModal"
          formId = "assignedSpecialistCnfForm"
          buttons = {this.assignedSpecialistButtons}
          isShowModal = {true}>
            <AssignedSpecialistConfirmPopup 
              checkBoxChange = {this.assignedSpecialistChange}/>
              
        </Modal>: null }
        { this.state.isAmendment_Reason ? <Modal 
          modalId = "Amendment Reason"
          formId = "Amendment Reason"
          isShowModal = {true} 
          buttons = {this.amendmentButtons} > 
          <h5 >Amend Inter Company Data</h5>
          <p>Amending Inter Company discount or description will impact all existing data </p>
          <p>Please enter the reason for amending this data </p>
          <br/>
          <h5>IC Amendment Reason</h5>
          <form>
          <textarea  id="amendmentReasonId" maxLength={500} ref={this.inputRef} name="amendmentReason"/>
          <div className="col-md-12 pull-right" style={{ float: "right" }}> 
           
          </div>
           </form></Modal> : null
      }
          
                <SaveBarWithCustomButtons
          codeLabel={localConstant.sideMenu.ASSIGNMENTS}
          SelectReports={ true }	
          reportsClickHandler= { this.reportsClickHandler }
          codeValue={assignmentNumber ? `(${ this.props.assignmentDetails ? AssignmentInfo.assignmentCustomerName.substring(0, 5).trim() : "" } : ${ assignmentNumber.toString().padStart(5, '0') })` : ''}
          currentMenu={localConstant.moduleName.ASSIGNMENT}
          currentSubMenu={this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_NEW ?
            localConstant.assignments.ADD_ASSIGNMENT :
            localConstant.assignments.EDIT_ASSIGNMENT}
          isARSSearch={this.props.isARSSearch}
          buttons={assignmentsButtons}
          activities={this.props.activities} />
        <div className="row ml-2 mb-0">
          <div className="col s12 pl-0 pr-2 verticalTabs">
            <CustomTabs
              callBackFuncs = {this.callBackFuncs}
              tabsList={this.assignmentTabData}
              moduleName="assignment"
              interactionMode={interactionMode}
              onSelect = { scrollToTop }
              forceRenderTabPanel={true}
            />
          </div>
        </div>
      </Fragment>
    );
  }
}

export default AssignmentDetail;