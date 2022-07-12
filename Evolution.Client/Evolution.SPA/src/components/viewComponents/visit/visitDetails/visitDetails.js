import React, { Component,Fragment } from 'react';
import { getlocalizeData,
  isEmpty, 
  isEmptyOrUndefine,
  isValidEmailAddress,
  parseQueryParam,
  isUndefined,
  ObjectIntoQuerySting,
  isTrue,
  formInputChangeHandler  } from '../../../../utils/commonUtils';
import CustomTabs from '../../../../common/baseComponents/customTab';
import { SaveBarWithCustomButtons } from '../../../applicationComponents/saveBar';
import { visitTabDetails } from './visitTabsDetails';
import moment from 'moment';
import Modal from '../../../../common/baseComponents/modal';
import ErrorList from '../../../../common/baseComponents/errorList';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { required, requiredNumeric } from '../../../../utils/validator';
import { modalTitleConstant, modalMessageConstant } from '../../../../constants/modalConstants';
import CustomModal from '../../../../common/baseComponents/customModal';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import arrayUtil from '../../../../utils/arrayUtil';
import {
  StringFormat,
  replaceAll
} from '../../../../utils/stringUtil';
import VisitAnchor from '../../../../components/viewComponents/visit/visitAnchor';
import dateUtil from '../../../../utils/dateUtil';
import ApprovedEmail from '../../../../common/approvalEmail';
import { HeaderData } from '../../../applicationComponents/visit/documents/documentHeader';
import { activitycode } from '../../../../constants/securityConstant';
import { ButtonShowHide } from '../.././../../utils/permissionUtil';
import { EvolutionReportsAPIConfig  } from '../../../../apiConfig/apiConfig';
import { 
  FormatTwoDecimal,
} from '../../../../common/inlineEditVisitTimesheet/visitTimesheetUtil';
import { DownloadReportFile }  from '../../../../common/reportUtil/ssrsUtil';
import { configuration } from '../../../../appConfig';

const localConstant = getlocalizeData();
let amendmentreason=false;
const validateEmail = (emails,validEmails =[])=>{
  const toAddress = emails.split(';');
  const invalidEmails = [];
    for(let i=0,len =toAddress.length;i<len;i++ ){
      const email = toAddress[i].trim();
      isValidEmailAddress(email)?
      validEmails.push(email):
      invalidEmails.push(email);
    }
    return invalidEmails;
};
const RejectReason = (props) => {
  return (<Fragment>
    <label>{modalMessageConstant.REJECT_REASON_MESSAGE}</label>
    <div className="row">
    <CustomInput
      hasLabel={true}
      isNonEditDateField={false}
      label={localConstant.gridHeader.DATE+':'}
      labelClass="customLabel"
      colSize='s4'
      dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
      type='date'
      name="rejectionDate"
      autocomplete="off"
      selectedDate={isEmptyOrUndefine(props.rejectionDate) ? "" : dateUtil.defaultMoment(props.rejectionDate)}
      onDateChange={props.visitRejectDateChange}
      shouldCloseOnSelect={true}
      disabled={props.interactionMode}
      //onValueChange={props.onValueChange}
      // onDatePickBlur={(e)=>{ props.handleDateChangeRaw(e,"fetchStartDate"); }}
    />
    <h6 className="col s12 p-0 mt-2">{modalTitleConstant.REASON_FOR_REJECTION}:</h6>
    <CustomInput
      hasLabel={true}     
      colSize="s12 mt-3"
      type='textarea'
      name="reasonForRejection"
      rows="10"
      inputClass=" customInputs textAreaInView"
      labelClass="customLabel"
      maxLength={978}
      onValueChange={props.onValueChange}
    />
    </div>
  
  </Fragment>);
};
//For Bug Id 563 -Start
function TSWithMultipleVisitPopup(props) {
  return (<Fragment>
    {
      props.TShasMultipleVisits.map(eachTs => {
        const tsName =  eachTs.techSpecName+'('+eachTs.pin+')';
        const msg = StringFormat(modalMessageConstant.TS_WITH_MULTIPLE_VISIT_TIMESHEET_ON_APPROVE,
          tsName, moment(props.date).format('DD-MMM-YYYY'));
        return (<Fragment>
          <p>{msg}</p>
          <ul className="tsOverBooking">
            {
              eachTs.resourceAdditionalInfos.map((eachObj) =>
                <li key={eachObj.visitJobReference}>
                   <VisitAnchor displayLinkText = {eachObj.visitJobReference} data={eachObj} />
                </li>)
            }
          </ul>
        </Fragment>);
      })
    }
  </Fragment>);
};
//For Bug Id 563 -End

class VisitDetails extends Component {     
  visitButtons =(buttonResponse)=>[		
        //  {
        //     name: 'Email Template',
        //     clickHandler: (e) => this.timesheetEmailClickHandler(e),
        //     className: "btn-small mr-0 ml-2"
        //   }, 
          {
            name: localConstant.commonConstants.SAVE,
            clickHandler: (e) => this.visitSaveClickHandler(e),
            className: "btn-small mr-0 ml-2",
            permissions:[ activitycode.NEW,activitycode.MODIFY ],
            isbtnDisable: this.props.isbtnDisable ,
            showBtn:buttonResponse[0]
          }, 
          {
            name: localConstant.commonConstants.REFRESHCANCEL,
            clickHandler: (e) => this.visitCancelHandler(e),
            className: "btn-small mr-0 ml-2 waves-effect modal-trigger",
            permissions:[ activitycode.NEW,activitycode.MODIFY ],
            isbtnDisable: this.props.isbtnDisable,
            showBtn:buttonResponse[0]
          },
          {//EditMode
            name: localConstant.commonConstants.DELETE,
            clickHandler: (e) => this.visitDeleteHandler(e),
            className: "btn-small mr-0 ml-2 dangerBtn waves-effect modal-trigger",
            //showBtn: !this.props.interactionMode,
            permissions:[ activitycode.DELETE ],
            showBtn:buttonResponse[1]
          },
              {//EditMode	
                name: localConstant.commonConstants.REPORTS,	
                clickHandler: this.reportsClickHandler,	
                className: "btn-small mr-0 ml-2 waves-effect modal-trigger",	
                //showBtn: !this.props.interactionMode,	
                // permissions:[ activitycode.DELETE ],	
                //showBtn:buttonResponse[1]	//Commented because of ITK D1358
              },
          {
            name: localConstant.assignments.CUSTOMER_REPORTING_REQUIREMENTS,
            clickHandler: (e) => this.visitClientReportingReqHandler(e),
            className: "btn-small mr-0 ml-2 waves-effect modal-trigger",
            //showBtn: !this.props.interactionMode
            //showBtn:buttonResponse[0] //Commented because of ITK D1358
          },
          {//Reject status
            name: localConstant.commonConstants.SUBMIT,
            clickHandler: (e) => this.visitSubmitHandler(e),
            className: "btn-small mr-0 ml-2 waves-effect modal-trigger",
            //showBtn: !this.props.interactionMode
            showBtn:buttonResponse[1]
          },
          {
            name: localConstant.commonConstants.APPROVE,
            clickHandler: (e) => this.visitApproveHandler(e),
            className: "btn-small mr-0 ml-2 waves-effect modal-trigger",
            //showBtn: !this.props.interactionMode
            showBtn:buttonResponse[1]
          },
          {
            name: localConstant.commonConstants.REJECT,
            clickHandler: (e) => this.visitRejectHandler(e),
            className: "btn-small mr-0 ml-2 dangerBtn waves-effect modal-trigger",
            //showBtn: !this.props.interactionMode
            showBtn:buttonResponse[1]
          },
          {//EditMode
            name: localConstant.visit.NEW_VISIT,
            clickHandler: (e) => this.visitCreateNewVisitHandler(e),
            className: "btn-small mr-0 ml-2 waves-effect modal-trigger",
            //showBtn: !this.props.interactionMode,
            permissions:[ activitycode.NEW ],
            showBtn:buttonResponse[1]  
          },
  ];		

  constructor(props) {
      super(props);
      
      this.state = {
        errorList: [],
        ncrCloseOutDateList: [],
        visitDateRangeValidation: [],
        isClientReportReqsPopup: false,
        isRejectForRejectPopup: false,
        customSavebarPosition:false,  
        isAmendment_Reason:false,      
        rejectionDate: moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT),
        editorHtml: '',
        isSendApprovalEmailNotification:false,
        isTShasMultipleVisits:[],    
        visitStatus: '',
        finalVisitValidationError: [],
        isOpenReportModal:false,	
        isShowVisitReport:false,
      };
      this.updatedData = {
        rejectionDate: moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT)
      };
      this.validations = {
        'NoLineItems': true,
        'hasZeroExpenses': true,
        'visitAssignmentDateValidation': true,
        'inValidCalendarEntries':true
      };
      this.invalidCalendarData=[];
      this.callBackFuncs ={
        onCancel:()=>{},
        onVisitCancel:()=>{},
        loadCalenderData:()=>{},
        disableVisitStatus:()=>{},
        reloadLineItemsOnSaveValidation:()=>{}
      };      
      this.modelBtns = {
        errorListButton: [
          {
            name: localConstant.commonConstants.OK,
            action: (e) => this.closeErrorList(e),
            btnID: "closeErrorList",
            btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
            showbtn: true
          }
        ],
        finalValidationListButton: [
          {
            name: localConstant.commonConstants.REVERT_ALL_CHANGES,
            action: (e) => this.revertFinalValidation(e),
            btnID: "closeErrorList",
            btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
            showbtn: true
          },
          {
            name: localConstant.commonConstants.CONTINUE_EDITING,
            action: (e) => this.closeFinalValidation(e),
            btnID: "closeErrorList",
            btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
            showbtn: true
          }
        ],
        clientReportRequirements: [
          {
            name: localConstant.commonConstants.CANCEL,
            action: (e) => this.closeModalPopup(e, "closeClientReportReqsPopup"),
            btnID: "clientReportReqBtn",
            btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
            showbtn: true
          }
        ],
        rejectVisit: [
          {
            name: localConstant.commonConstants.REJECT,
            action: (e) => this.submitRejectReason(e),
            btnID: "clientReportReqBtn",
            btnClass: "modal-close waves-effect waves-teal btn-small mr-2 dangerBtn",
            showbtn: true
          },
          {
            name: localConstant.commonConstants.CANCEL,
            action: (e) => this.closeModalPopup(e, "closeReasonForRejectPopup"),
            btnID: "clientReportReqBtn",
            btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
            showbtn: true
          }
        ],
        sendApprovalEmailNotificationBtns: [
          {
            name: localConstant.commonConstants.CANCEL,
            action: (e) => this.closeModalPopup(e, "closeApprovalEmailNotificationPopup"),
            btnID: "clientReportReqBtn",
            btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
            showbtn: true,
            type:"button"
          },{
            name: localConstant.commonConstants.SEND,
            action: this.sendApprovalEmailNotification,
            btnID: "clientReportReqBtn",
            btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
            showbtn: true,
            type:"button"
          }
        ],
        TShasMultipleVisits: [
          {
            name: localConstant.commonConstants.OK,
            action:(e)=>this.TShasMultipleOnApprove(e, 'approve'),
            btnID: "TShasMultipleVisitsBtn",
            btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
            showbtn: true,
            type:"button"
          }
        ]
      };
      const functionRefs = {};
      this.inputRef = React.createRef();
      functionRefs["enableEditColumn"] = ()=>{ return true;};
      this.documentHeaderData = HeaderData(functionRefs);
  }

  TShasMultipleOnApprove = (e, from) => {
    this.closeModalPopup(e, "TShasMultipleVisits");
    const confirmationObject = {
      title: modalTitleConstant.CONFIRMATION,
      message: modalMessageConstant.VISIT_APPROVE_CONFIRM_CHC,
      modalClassName: "warningToast",
      type: "confirm",
      buttons: [
        {
          buttonName: "Yes",
          onClickHandler: (e) => this.TShasMultipleBookingHandler(from),
          className: "modal-close m-1 btn-small"
        },
        {
          buttonName: "No",
          onClickHandler: this.TSHasMultipleBookingCancel,
          className: "modal-close m-1 btn-small"
        }
      ]
    };
    this.props.actions.DisplayModal(confirmationObject);
  }

  TSHasMultipleBookingCancel = () => {    
    this.updatedData["visitStatus"] = this.state.visitStatus;
    this.props.actions.AddUpdateGeneralDetails(this.updatedData);
    this.updatedData = {
      rejectionDate: moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT)
    };
    this.props.actions.HideModal();
  }

//For Bug Id 563 -Start
  TShasMultipleBookingHandler = async (from)=>{
    const status = this.getNextVisitStatus('approve');   
    this.props.actions.HideModal();    
    if (from === 'approve' && 'A' === status.visitStatus) {
      this.processApprovalEmailNotification();
    } else if(from === 'approve' && 'O' === status.visitStatus)   {
        const obj ={
        status:status,
        isFromApproveOrReject:from,
        isValidationRequired:false,
        isSuccesAlert: true
      };
      await this.props.actions.UpdateVisitStatus(obj);
    }
  }
  //For Bug Id 563 -End

  async componentDidMount() {     
    //this.props.actions.SetCurrentPageMode();
    const result = this.props.location.search && parseQueryParam(this.props.location.search);
    const isFetchLookValues = true;
    let isNewVisit = false;
   // if (this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE) {     
    if(!isEmptyOrUndefine(result)) {
      // d1040 - To check CRN project 
      if (isUndefined(result.vAssId)) {
        isNewVisit = true;
        if(!isTrue(result.isCVisitFromEVist)){
            const data = { assignmentId:result.assignmentId,isVisitTimesheet:true };
            const response = await this.props.actions.FetchAssignmentsDetailInfo(data);           
            if(response){
            this.props.actions.SaveSelectedAssignmentId({ assignmentId:parseInt(result.assignmentId),assignmentNumber:parseInt(response.assignmentNumber) });  
            this.props.actions.FetchAssignmentForVisitCreation(parseInt(result.assignmentId),isFetchLookValues);
          }
        }
      } else if(isTrue(result.isCVisitFromEVist)){
        isNewVisit = true;
        const response = await this.props.actions.FetchAssignmentsDetailInfo({ assignmentId:result.vAssId ,isVisitTimesheet:true });           
        if(response){
          this.props.actions.SaveSelectedAssignmentId({ assignmentId:parseInt(result.vAssId),assignmentNumber:parseInt(response.assignmentNumber) });  
          this.props.actions.FetchAssignmentForVisitCreation(parseInt(result.vAssId));
        }
      } else {
        isNewVisit = false;
        const selectedParamIds={
          visitId:result.vId,
          visitAssignmentId:result.vAssId,
          visitProjectNumber:result.vProNo,
          visitSupplierPOId:result.vSPOId
        };
        this.props.actions.GetSelectedVisit(selectedParamIds);    
        const newRes = await this.props.actions.FetchVisitDetail(result.vId, isFetchLookValues);
        if (newRes) {
          this.callBackFuncs.loadCalenderData();
          this.callBackFuncs.disableVisitStatus();
        }
        if (!isEmptyOrUndefine(this.props.visitInfo) && !isEmptyOrUndefine(this.props.visitInfo.visitStatus) 
        && this.props.visitInfo.visitStatus == "D"){
          this.props.actions.UpdateInteractionMode(true);
        }
        else{
          // console.log("Fetch visit failed");
        }
      }
    }
          
    this.props.actions.FetchReferencetypes(isNewVisit);
    if(this.props.visitInfo && this.props.visitInfo.isVisitOnPopUp) this.visitClientReportingReqHandler(); //IGO QC 864 - client pop in visit
    this.setState({ customSavebarPosition:true });
    this.updateTabDetailsRefresh();    
  }

  componentWillUnmount(){
    this.props.actions.ClearCalendarData();   
  }

  //Approval Modal Popup
  timesheetEmailClickHandler = (e) => {
    e.preventDefault();
    this.processApprovalEmailNotification();
  }

  updateTabDetailsRefresh() {
    visitTabDetails.forEach(row => {
      if(row["tabBody"] === "GeneralDetails") {        
        row["isCurrentTab"] = true;
      }  
      row["isRefresh"] = true;
    });
  }

  filterTabDetail = () => {
    const tabsToHide = [];
    tabsToHide.push(...[
      "Invoice"
    ]);
    if (this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE) {
      tabsToHide.push("Documents");
    }
    const visitTabs = arrayUtil.negateFilter(visitTabDetails, 'tabBody', tabsToHide);
    return visitTabs;
  }

  closeErrorList = (e) => {
    e.preventDefault();
    this.setState({
      errorList: [],
      ncrCloseOutDateList: [],
      visitDateRangeValidation: []
    });
  }
  revertFinalValidation = (e) => {  
    e.preventDefault();  
    this.setState({
      errorList: [],
      finalVisitValidationError: []
    });
    this.cancelVisit();
  }
  closeFinalValidation = (e) => { 
    e.preventDefault();       
    this.setState({
      errorList: [],
      finalVisitValidationError: []
    });
  }
  handleVisitButtons() {
    const btnsToHide = [];		
    const {		
      currentPage,		
      isInterCompanyAssignment,		
      isOperator,		
      isCoordinator,		
      visitStatus,
      visitInfo,
      isCoordinatorCompany,
      isOperatorCompany,
      notLoggedinCompany		
    } = this.props;		
    const isInEditMode= (this.props.interactionMode === false && this.props.currentPage === localConstant.visit.EDIT_VIEW_VISIT_MODE);
    const isInViewMode= (this.props.pageMode === localConstant.commonConstants.VIEW) ? true : false;
    const isInCreateMode= (this.props.interactionMode === false && currentPage === localConstant.visit.CREATE_VISIT_MODE);
    let buttonResponse;
    if(visitStatus === "D"){
      buttonResponse = ButtonShowHide(true,false,false);
    }
    else{
      buttonResponse = ButtonShowHide(isInEditMode,isInViewMode,isInCreateMode);
    }
    let interactionMode= this.props.interactionMode;
    if(this.props.pageMode===localConstant.commonConstants.VIEW){
      interactionMode=true;
     }
    //const visitStatus = (isEmptyOrUndefine(this.props.visitInfo) ? "" : this.props.visitInfo.visitStatus);		
    if (currentPage === localConstant.visit.CREATE_VISIT_MODE) {		
      btnsToHide.push(		
        localConstant.commonConstants.DELETE,		
        localConstant.commonConstants.SUBMIT,		
        localConstant.visit.NEW_VISIT,		
        localConstant.commonConstants.APPROVE,		
        localConstant.commonConstants.REJECT		
      );		
    }

    if (isInterCompanyAssignment && isOperatorCompany && [ 'O', 'A' ].includes(visitStatus)) {		
      btnsToHide.push(		
        localConstant.commonConstants.DELETE
      );		
    }

    if((visitInfo.isFinalVisit === true && 'A'=== visitStatus) ||
     (!isOperatorCompany && visitStatus !== "D")){
      btnsToHide.push(		
        localConstant.visit.NEW_VISIT,		
      );
    }

    if ((isInterCompanyAssignment && isOperatorCompany && 'O' === visitStatus) 
      || (isInterCompanyAssignment && isCoordinatorCompany && ([ 'R', 'C', 'J' ].includes(visitStatus)))
      || ('A' === visitStatus) || notLoggedinCompany) {		
      btnsToHide.push(		
        localConstant.commonConstants.APPROVE,		
        localConstant.commonConstants.REJECT,		
        localConstant.commonConstants.SUBMIT,		
      );		
    }
    else if (isInterCompanyAssignment 
      && ((isCoordinatorCompany && 'O' === visitStatus) 
      || (isOperatorCompany && 'R' === visitStatus))){		
      btnsToHide.push(		
        localConstant.commonConstants.SUBMIT		
      );		
    }			
    else if (isInterCompanyAssignment 
      && (isOperatorCompany ) && [ 'J', 'R' ].includes(visitStatus)) {		
      btnsToHide.push(		
        localConstant.commonConstants.APPROVE,		
        localConstant.commonConstants.REJECT		
      );		
    }	
    else if (!isInterCompanyAssignment && [ 'R', 'J' ].includes(visitStatus)) {		
      btnsToHide.push(		
        localConstant.commonConstants.APPROVE,		
        localConstant.commonConstants.REJECT		
      );		
    }		
    else if ([ 'C' ].includes(visitStatus)) {		
      btnsToHide.push(		
        localConstant.commonConstants.SUBMIT		
      );		
    }	
    // 'T', -- hidden
    else if ([ 'N', 'Q', 'S', 'U', 'W', 'T', 'D' ].includes(visitStatus)) {		
      btnsToHide.push(		
        localConstant.commonConstants.SUBMIT,
        localConstant.commonConstants.APPROVE,		
        localConstant.commonConstants.REJECT			
      );		
    }
    return arrayUtil.negateFilter(this.visitButtons(buttonResponse), 'name', btnsToHide);		
  };		

  validateVisitOnApprove = () => {
    const { visitInfo, isInterCompanyAssignment, isOperator, isCoordinator, visitValidationData, isOperatorCompany, isCoordinatorCompany } = this.props;    
    const isNewVisit = (this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE);    
    if([ 'Q','T','U' ].includes(visitInfo.visitStatus)) {      
      IntertekToaster(`${ localConstant.validationMessage.VISIT_APPROVE_OTHER_STATUS }`, 'warningToast VisitOtherStatus');
      return false;
    }
    if(visitInfo.isFinalVisit == null || visitInfo.isFinalVisit === false) {       
        if(visitValidationData.visitAssignmentDates.filter(x => x.visitId !== visitInfo.visitId && visitInfo.visitNumber <= x.visitNumber).length === 0 || isNewVisit === true) {
          IntertekToaster(`${ localConstant.validationMessage.VISIT_FINAL_VISIT_VALIDATION }`, 'warningToast VisitFinalVisitVal');
          return false;
        }
    }
    if (this.props.isCoordinatorCompany) {
      if (moment(visitInfo.visitEndDate).isAfter(moment(), 'day')) {
        if (this.props.customerReportingNotificationContant && this.props.customerReportingNotificationContant.length > 0) {
          IntertekToaster(`${ localConstant.validationMessage.VISIT_DATE_SENT_TO_CUSTOMER_VALIDATION_DEFAULT }`, 'warningToast visitSentDateEndDateMismatchVal');
          return false;
        }
      }
    }
    if (this.props.visitCalendarData.length > 0) {
      IntertekToaster(`${ localConstant.validationMessage.VISIT_APPROVE_VALIDATION_TO_SAVE }`, 'warningToast visitSentDateEndDateMismatchVal');
      return false;
    }    
    
    if(isInterCompanyAssignment) {
      // if(isOperator) {
      //   if (required(visitInfo.visitReportNumber)) {
      //     IntertekToaster(`${ localConstant.validationMessage.VISIT_REPORT_NUMBER_REQUIRED }`, 'warningToast VisitReportNumberVal');
      //     return false;
      //   }
      // } else if(isCoordinator) {
      //   if (required(visitInfo.visitReportSentToCustomerDate)) {
      //     IntertekToaster(`${ localConstant.validationMessage.VISIT_REPORT_SENT_DATE_REQUIRED }`, 'warningToast VisitReportNumberVal');
      //     return false;
      //   }
      // }
      if(isOperatorCompany) {
        if (required(visitInfo.visitReportNumber)) {
          IntertekToaster(`${ localConstant.validationMessage.VISIT_REPORT_NUMBER_REQUIRED }`, 'warningToast VisitReportNumberVal');
          return false;
        }
      } else if (isCoordinatorCompany && required(visitInfo.visitReportSentToCustomerDate)) {
        if (this.props.customerReportingNotificationContant && this.props.customerReportingNotificationContant.length > 0) {
          // Do nothing 
        }
        else {
          IntertekToaster(`${ localConstant.validationMessage.VISIT_REPORT_SENT_DATE_REQUIRED }`, 'warningToast VisitReportNumberVal');
          return false;
        }
      }
    } else {      
      if (required(visitInfo.visitReportNumber)) {
        IntertekToaster(`${ localConstant.validationMessage.VISIT_REPORT_NUMBER_REQUIRED }`, 'warningToast VisitReportNumberVal');
        return false;
      }
      if (required(visitInfo.visitReportSentToCustomerDate)) {
        if (this.props.customerReportingNotificationContant && this.props.customerReportingNotificationContant.length > 0) {
          // Do nothing
        }
        else {
          IntertekToaster(`${ localConstant.validationMessage.VISIT_REPORT_SENT_DATE_REQUIRED }`, 'warningToast VisitReportNumberVal');
          return false;
        }
      }
    }
    
    return true;
  }

  subSupplierTechSpecValidation()
  {
    const subSupplierTechSpec = this.props.subSupplierList;
    const visitInfo = this.props.visitInfo;
    const visitTechnicalSpecialists = this.props.visitTechnicalSpecialists.filter(x => x.recordStatus !== 'D');
    let isExits = true;
    if(!isEmptyOrUndefine(subSupplierTechSpec)) {    
      const selectedSubSupplier =  subSupplierTechSpec.filter(x => x.subSupplierSupplierId == visitInfo.supplierId);        
      if(selectedSubSupplier.length > 0 && selectedSubSupplier[0].assignmentSubSupplierTS 
          && selectedSubSupplier[0].assignmentSubSupplierTS.length > 0) {            
            visitTechnicalSpecialists.forEach((eachTs) => {
              const isSubSuPExists = selectedSubSupplier[0].assignmentSubSupplierTS.filter(x => x.epin === eachTs.pin).length > 0 ? true : false; 
              if(isSubSuPExists === false) {
                isExits = false;
              }
            });

      } else if (selectedSubSupplier.length > 0) {
        isExits = false;
      }      
    } 
    return isExits;
  }  

  uploadedDocumentCheck=()=>{  // when file Uploading user click on sve button showing warning Toaster Here 
    let count = 0;
    if (Array.isArray(this.props.VisitDocuments) && this.props.VisitDocuments.length > 0) {
        this.props.VisitDocuments.map(document =>{                             
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
    let errors = [];
    const { visitInfo, visitTechnicalSpecialists, visitValidationData } = this.props;    
    this.invalidCalendarData=[];
    if (visitInfo) {
      if (required(visitInfo.visitStartDate)) {
        errors.push(`${ localConstant.visit.GENERAL_DETAILS } - ${ localConstant.visit.DATE_FROM }`);
      }
      if (required(visitInfo.visitEndDate)) {
        errors.push(`${ localConstant.visit.GENERAL_DETAILS } - ${ localConstant.visit.DATE_TO }`);
      }
      if (isEmpty( visitTechnicalSpecialists) || visitTechnicalSpecialists.filter(calendarData => calendarData.recordStatus !== 'D').length === 0) {
        errors.push(`${ localConstant.visit.GENERAL_DETAILS } - ${ localConstant.visit.SELECT_TECH_SPEC }`);
      }
      if (required(visitInfo.visitStatus)) {
        errors.push(`${ localConstant.visit.GENERAL_DETAILS } - ${ localConstant.visit.VISIT_STATUS }`);
      }
      if (required(visitInfo.supplierId)) {
        errors.push(`${ localConstant.visit.GENERAL_DETAILS } - ${ localConstant.visit.SUPPLIER }`);
      }
      if(!isEmpty(this.props.VisitDocuments)){
        const issueDoc= [];
        this.props.VisitDocuments.map(document =>{
            if (isEmpty(document.documentType) && document.recordStatus!=='D' && document.subModuleRefCode === '0') { //ID 507 #2 deleted few documents without selecting “Document type”, 
                                                                                 //when click on save validation showing alert box
                 errors.push(`${ localConstant.visit.documents.DOCUMENT } - ${ document.documentName } - ${ localConstant.visit.documents.SELECT_FILE_TYPE } `);
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
        });
        if(issueDoc && issueDoc.length > 0){
          let techSpecData = '';
          for (let i = 0; i < issueDoc.length; i++) {
            techSpecData = techSpecData +'\"' +issueDoc[i]+'\"'+ '; \r\n';
          }
          errors.push(`${ StringFormat(localConstant.project.documents.UPLOAD_ISSUE, techSpecData) }`);
        }
      }
      if (visitInfo.visitStatus === "D" && required(visitInfo.unusedReason)) {
        errors.push(`${ localConstant.visit.GENERAL_DETAILS } - ${ localConstant.visit.REASON }`);
      }
    }

    if(errors.length < 1) {
      let hasErrors = false;
      hasErrors = this.validateTechSpecTime();
      if(hasErrors === false) {
        hasErrors = this.validateTechSpecExpense();
      }
      if(hasErrors === false) {
        hasErrors = this.validateTechSpecTravel();
      }
      if(hasErrors === false) {
        hasErrors = this.validateTechSpecConsumable();
      }
      if(hasErrors) {
        this.callBackFuncs.reloadLineItemsOnSaveValidation();
        errors.push(`${ localConstant.visit.TECHNICAL_SPECIALIST_VALIDATION }`);
      }
    }

    if(errors.length < 1) {
      const dateRangeValidation = [];
      const previousDateRanges = [];
      const nextDateRanges = [];
      let hasVisitDate = true;
      let visitRange;
      if(visitValidationData && visitValidationData.visitAssignmentDates && visitValidationData.visitAssignmentDates.length > 0) {
        let currentVisitIndex = (this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE ? visitValidationData.visitAssignmentDates.length 
                                  : visitValidationData.visitAssignmentDates.findIndex(x => x.visitId === visitInfo.visitId));   
        // if(visitInfo.visitId === visitValidationData.firstVisitId && visitValidationData.visitAssignmentDates.length > 1) {
        //   const futureVisit = visitValidationData.visitAssignmentDates[currentVisitIndex + 1];
        //   if(futureVisit && visitInfo.visitStartDate > futureVisit.visitStartDate) {
        //     dateRangeValidation.push(`${ StringFormat(localConstant.visit.FIRST_VISIT_DATE_RANGE_VALIDATION, moment(futureVisit.visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT)) }`);
        //     this.setState({
        //       visitDateRangeValidation: dateRangeValidation
        //     });
        //     return false;
        //   }
        // } else 
        if(currentVisitIndex >= 0){
          const previousVisits =  visitValidationData.visitAssignmentDates.filter(x => [ "Q", "T", "U", "W", "D" ].includes(x.visitStatus));;          
          if(previousVisits && previousVisits.length > 0) { 
            const techIds = [];  
            if (this.props.visitTechnicalSpecialists) {
              if (Array.isArray(this.props.visitTechnicalSpecialists) && this.props.visitTechnicalSpecialists.length > 0) {
                  this.props.visitTechnicalSpecialists.map(tech => {
                      if (tech.recordStatus !== 'D')
                          techIds.push(tech.pin);
                  });
              }
            }
            
            for(let i=0; i < techIds.length; i++) {
              const TechSpecVisits = [];
              if (visitValidationData.technicalSpecialists) {
                if (Array.isArray(visitValidationData.technicalSpecialists) && visitValidationData.technicalSpecialists.length > 0) {
                    visitValidationData.technicalSpecialists.map(tech => {
                        if (techIds[i] === tech.pin || tech.visitId === visitInfo.visitId) TechSpecVisits.push(tech.visitId);
                    });
                }
              }
              const previousVisitList = previousVisits.filter(x => TechSpecVisits.includes(x.visitId));
              currentVisitIndex = previousVisitList.findIndex(x => x.visitId === visitInfo.visitId);
              if(this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE && previousVisitList.length > 0) {
                currentVisitIndex = previousVisitList.length - 1;
              }
              if(currentVisitIndex === 0 && previousVisitList.length > 1) {
                const nextVisit = previousVisitList[currentVisitIndex + 1];
                const currentDate = moment(visitInfo.visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT);
                const nextDate = moment(nextVisit.visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT); 
                if(nextVisit && moment(currentDate).isAfter(moment(nextDate))) {
                  previousDateRanges.push([ moment(nextVisit.visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT) ]);
                  hasVisitDate = false;
                  visitRange = 1;
                }
              } else if(((this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE && currentVisitIndex >= 0)
                    || (this.props.currentPage === localConstant.visit.EDIT_VIEW_VISIT_MODE && currentVisitIndex > 0)) 
                    && currentVisitIndex === previousVisitList.length - 1) {                
                currentVisitIndex = (this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE ? previousVisitList.length - 1 : previousVisitList.length - 2);
                //const previousVisit = previousVisitList[currentVisitIndex];
                const currentDate = moment(visitInfo.visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT);             
                previousVisitList.forEach( row => {
                  const previousDate = moment(row.visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT);
                  if(row && (row.visitNumber !== visitInfo.visitNumber) && moment(currentDate).isBefore(moment(previousDate))) {
                    nextDateRanges.push([ moment(row.visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT) ]);
                    hasVisitDate = false;
                    visitRange = 2;
                  }
                });
              } else if(currentVisitIndex > 0 && currentVisitIndex < previousVisitList.length - 1) {
                const previousVisit = previousVisitList[currentVisitIndex - 1];
                const futureVisit = previousVisitList[currentVisitIndex + 1];
                const currentDate = moment(visitInfo.visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT);
                const previousDate = moment(previousVisit.visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT);
                const nextDate = moment(futureVisit.visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT);
                if(previousVisit && futureVisit && 
                  (moment(currentDate).isBefore(moment(previousDate)) || moment(currentDate).isAfter(moment(nextDate)))) {
                  previousDateRanges.push([ moment(previousVisit.visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT) ]);
                  nextDateRanges.push([ moment(futureVisit.visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT) ]);
                  hasVisitDate = false;
                  visitRange = 3;
                } else {
                  previousDateRanges.push([ moment(previousVisit.visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT) ]);
                  nextDateRanges.push([ moment(futureVisit.visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT) ]);
                }
              }
            }
          }                    
        }
      }
      if(!hasVisitDate && visitInfo.visitStatus && visitInfo.visitStatus !== 'D') {
        let minDate = '';
        let maxDate = '';
        if(visitRange === 1) {
          previousDateRanges.forEach( row => {
            minDate = (minDate === '' || moment(minDate).isAfter(moment(row)) ? row : minDate);
          });
          dateRangeValidation.push(`${ StringFormat(localConstant.visit.FIRST_VISIT_DATE_RANGE_VALIDATION, minDate) }`);
          this.setState({
            visitDateRangeValidation: dateRangeValidation
          });
        } else if (visitRange === 2) {
          nextDateRanges.forEach( row => {
            maxDate = (maxDate === '' || moment(maxDate).isBefore(moment(row)) ? row : maxDate);
          });
          dateRangeValidation.push(`${ StringFormat(localConstant.visit.CREATE_VISIT_DATE_RANGE_VALIDATION, maxDate) }`);
          this.setState({
            visitDateRangeValidation: dateRangeValidation
          });
        } else if (visitRange === 3) {
          previousDateRanges.forEach( row => {
            minDate = (minDate === '' || moment(minDate).isBefore(moment(row)) ? row : minDate);
          });
          nextDateRanges.forEach( row => {
            maxDate = (maxDate === '' || moment(maxDate).isAfter(moment(row)) ? row : maxDate);
          });
          dateRangeValidation.push(`${ StringFormat(localConstant.visit.EDIT_VISIT_DATE_RANGE_VALIDATION, minDate, maxDate) }`);
          this.setState({
            visitDateRangeValidation: dateRangeValidation
          });
        }
        return false;
      }
    }    

    if(visitInfo.isFinalVisit === true && visitValidationData.supplierPerformanceNCRDate) {      
      const supplierPerformance = visitValidationData.supplierPerformanceNCRDate.filter(x => x.visitId !== visitInfo.visitId);
      const visitSupplierPerformancesData = this.props.visitSupplierPerformances.filter(x => (x.ncrCloseOutDate === null || x.ncrCloseOutDate === undefined || x.ncrCloseOutDate === '') && x.recordStatus !== "D");
      if(supplierPerformance.length > 0 || visitSupplierPerformancesData.length > 0) {
        errors.push(`${ localConstant.visit.VISIT_OPEN_NCR_DISPLAY_TEXT }`);
        supplierPerformance.forEach(row => {        
          errors.push(<VisitAnchor displayLinkText = {row.visitJobReference} data = {row} /> );
        });
        if(visitSupplierPerformancesData.length > 0) {
          errors.push(<VisitAnchor displayLinkText = {visitInfo.visitJobReference} data = {visitInfo} /> );
        }
        this.setState({
          ncrCloseOutDateList: errors
        });
        return false;
      }
    }

    if ((visitInfo.visitId !== visitValidationData.finalVisitId) && visitInfo.isFinalVisit === true && visitValidationData.hasFinalVisit === true) {
        errors = [];
        errors.push(`${ localConstant.visit.FINAL_VISIT_VALIDATION_HEADER }`);
        errors.push(`${ localConstant.validationMessage.FINAL_VISIT_VALIDATION }`);
        this.setState({
          finalVisitValidationError: errors
        });
        return false; 
    }
    
    //Final Visit with visits with future date and following visits with Awaiting Approval
    if(errors.length < 1 && visitInfo.isFinalVisit === true && 
      ((visitValidationData.awaitingApprovalVisitId && visitValidationData.awaitingApprovalVisitId > visitInfo.visitId)
      || (visitValidationData.visitAssignmentDates && visitValidationData.visitAssignmentDates.filter(x => x.visitId !== visitInfo.visitId && x.fromDate > visitInfo.visitStartDate && [ 'A', 'C', 'J', 'O', 'R' ].includes(x.visitStatus)).length > 0)
      || visitValidationData.hasRateUnitFinalVisit)) {
        errors.push(`${ localConstant.visit.FINAL_VISIT_VALIDATION_HEADER }`);
        errors.push(`${ localConstant.validationMessage.VISIT_FUTURE_AWAITING_APPROVAL }`);
        this.setState({
          finalVisitValidationError: errors
        });
        return false;   
    }
    //Commented this code because MS-TS changes changed the behaviour
    // if(errors.length < 1) {
    //   const isSubSupplierTechSpec = this.subSupplierTechSpecValidation();
    //   if(isSubSupplierTechSpec === false) {
    //     errors.push(`${ localConstant.visit.SUB_SUPPLIER_TECH_SPCE_VALIDATION_TEXT1 }`);
    //     errors.push(`${ localConstant.visit.SUB_SUPPLIER_TECH_SPCE_VALIDATION_TEXT2 }`);
    //     errors.push(`${ localConstant.visit.SUB_SUPPLIER_TECH_SPCE_VALIDATION_TEXT3 }`);
    //     errors.push(`${ localConstant.visit.SUB_SUPPLIER_TECH_SPCE_VALIDATION_TEXT4 }`);
    //     //errors.push(nl2br(localConstant.visit.SUB_SUPPLIER_TECH_SPCE_VALIDATION_TEXT));      
    //     //IntertekToaster('TS is not Assigned to SubSupplier', 'warningToast VisitEndDateVal');
    //     this.setState({
    //       subSupplierTechSpecValidation: errors
    //     });
    //     return false;
    //   }
    // }
    if (errors.length < 1) {
      if (this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE) {
        const filterVisitCalendarData = this.props.visitCalendarData.filter(calendarData => calendarData.recordStatus !== 'D');
        if (filterVisitCalendarData.length === 0)
          errors.push(`${ localConstant.visit.AT_LEAST_ONE_CALENDAR_ENTRY_VALIDATION }`);
        else {
          const validCalendarData = [];
          const validSaveCalendarData=[];
          const startDate = moment(visitInfo.visitStartDate).format(localConstant.commonConstants.DATE_FORMAT);
          const endDate = moment(visitInfo.visitEndDate).format(localConstant.commonConstants.DATE_FORMAT);
          filterVisitCalendarData.forEach((calendarData) => {
            const eventStartDate = moment(calendarData.startDateTime).format(localConstant.commonConstants.DATE_FORMAT);
            const eventEndDate = moment(calendarData.endDateTime).format(localConstant.commonConstants.DATE_FORMAT);
            const isValidStartBetween = moment(calendarData.startDateTime).isBetween(visitInfo.visitStartDate, visitInfo.visitEndDate);
            const isValidEndBetween = moment(calendarData.endDateTime).isBetween(visitInfo.visitStartDate, visitInfo.visitEndDate);
            const isValidStart = (eventStartDate == startDate || eventStartDate == endDate);
            const isValidEnd = (eventEndDate == endDate || eventEndDate == startDate);
            if ((isValidStartBetween && isValidEndBetween) || (isValidStart && isValidEnd) || (isValidStartBetween&&isValidEnd) || (isValidEndBetween&&isValidStart) ) {
              validCalendarData.push(calendarData);
            }
            if ((isValidStartBetween && isValidEndBetween) || (isValidStart && isValidEnd) || (isValidStartBetween&&isValidEnd) || (isValidEndBetween&&isValidStart) || calendarData.recordStatus === 'D') {
              validSaveCalendarData.push(calendarData);
            }
            else{
              this.invalidCalendarData.push(calendarData);
            }
          });

          if (validCalendarData.length === 0)
            errors.push(`${ localConstant.visit.AT_LEAST_ONE_CALENDAR_ENTRY_VALIDATION }`);
          else{
            this.props.actions.SaveValidCalendarDataForSave(validSaveCalendarData);
          }
        }
      }
      else
      this.props.actions.SaveValidCalendarDataForSave(this.props.visitCalendarData);
    }
    if (errors.length > 0) {
      this.setState({
        errorList: errors
      });
      return false;
    }
    else {
      if (!moment(visitInfo.visitStartDate).isValid()) {
        IntertekToaster(`${ localConstant.validationMessage.INVALID_VISIT_START_DATE }`, 'warningToast VisitEndDateVal');
        return false;
      }
      if (!moment(visitInfo.visitEndDate).isValid()) {
        IntertekToaster(`${ localConstant.validationMessage.INVALID_VISIT_END_DATE }`, 'warningToast visitEndDateVal');
        return false;
      }      
      if (!isEmptyOrUndefine(visitInfo.visitReportSentToCustomerDate) && !moment(visitInfo.visitReportSentToCustomerDate).isValid()) {
        IntertekToaster(`${ localConstant.validationMessage.INVALID_VISIT_DATE_REPORT_SENDTO_CUSTOMER }`, 'warningToast visitReportSentToCustomerDateVal');
        return false;
      }
      if (!isEmptyOrUndefine(visitInfo.visitExpectedCompleteDate) && !moment(visitInfo.visitExpectedCompleteDate).isValid()) {
        IntertekToaster(`${ localConstant.validationMessage.INVALID_VISIT_EXPECTED_COMPLETED_DATE }`, 'warningToast visitExpectedCompleteDateVal');
        return false;
      }
      if (moment(visitInfo.visitStartDate).isAfter(visitInfo.visitEndDate, 'day')) {
        IntertekToaster( `${ localConstant.validationMessage.VISIT_START_DATE_END_DATE_MISMATCH }`, 'warningToast visitStartDateEndDateMismatchVal');
        return false;
      }
      if (!isEmptyOrUndefine(visitInfo.visitReportSentToCustomerDate) && moment(visitInfo.visitEndDate).isAfter(visitInfo.visitReportSentToCustomerDate, 'day')) {
        IntertekToaster( `${ localConstant.validationMessage.VISIT_DATE_SENT_TO_CUSTOMER_VALIDATION }`, 'warningToast visitSentDateEndDateMismatchVal');
        return false;
      }
      //date diff      
      if (moment(visitInfo.visitEndDate).diff(visitInfo.visitStartDate, 'days') > 365) {        
        const validateDate = moment(visitInfo.visitStartDate).add(365, 'days').format(localConstant.commonConstants.UI_DATE_FORMAT); 
        IntertekToaster( `${ localConstant.validationMessage.VISIT_ENDDATE_IS_TO_FAR } ${ validateDate }`, 'warningToast visitvalidateEndDateMismatchVal');
        return false;
      }
      if (visitInfo.visitCompletedPercentage && (visitInfo.visitCompletedPercentage < 0 || visitInfo.visitCompletedPercentage > 100)) {
        IntertekToaster("Percentage Completed should be with in  0 to 100 range", 'warningToast PercentRange');
        return false;
      }
      if(this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE && visitValidationData.hasFinalVisit === true) {
        IntertekToaster(`${ localConstant.validationMessage.VISIT_CREATE_FINAL_VISIT }`, 'warningToast PercentRange');
        return false;
      }
      //Has TBA Status already
      if(this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE && visitValidationData.hasTBAStatusVisit === true) {
        IntertekToaster(`${ localConstant.validationMessage.VISIT_HAS_TBA_STATUS }`, 'warningToast VisitStatusTBA');
        return false;
      }
    }
    return true;
  }

  validateTechSpecTime() {
    let hasErrors = false;
    const { timeLineItems } = this.props;
    if(!isEmptyOrUndefine(timeLineItems)) {
      timeLineItems.forEach(row => {
        if(row.recordStatus !== 'D') {
          if(required(row.expenseDate)){            
            hasErrors = true;
          }
          if(dateUtil.isUIValidDate(row.expenseDate)){
            hasErrors = true;
          }
          if(required(row.chargeExpenseType)){                    
            hasErrors = true;
          }
          if(isEmptyOrUndefine(row.invoicingStatus) || !(row.invoicingStatus === 'C' || row.invoicingStatus === 'D')) {
            if(requiredNumeric(row.chargeTotalUnit)){            
              hasErrors = true;
            }
            if(requiredNumeric(row.chargeRate)){            
              hasErrors = true;
            }
            if(!requiredNumeric(row.chargeRate) && row.chargeRate > 0
                && !requiredNumeric(row.chargeTotalUnit) && row.chargeTotalUnit > 0
                && required(row.chargeRateCurrency)){            
                  hasErrors = true;
            }
            if(!(isEmptyOrUndefine(row.chargeWorkUnit) && isEmptyOrUndefine(row.chargeTravelUnit) && isEmptyOrUndefine(row.chargeWaitUnit)
                && isEmptyOrUndefine(row.chargeReportUnit))) {
                  const chargeWorkUnit = isNaN(parseFloat(row.chargeWorkUnit))?0:parseFloat(row.chargeWorkUnit);
                  const chargeTravelUnit = isNaN(parseFloat(row.chargeTravelUnit))?0:parseFloat(row.chargeTravelUnit);
                  const chargeWaitUnit = isNaN(parseFloat(row.chargeWaitUnit))?0:parseFloat(row.chargeWaitUnit);
                  const chargeReportUnit = isNaN(parseFloat(row.chargeReportUnit))?0:parseFloat(row.chargeReportUnit);
                  const sumValue = FormatTwoDecimal(chargeWorkUnit + chargeTravelUnit + chargeWaitUnit + chargeReportUnit);
                  const chargeUnit = FormatTwoDecimal(isNaN(parseFloat(row.chargeTotalUnit))?0:parseFloat(row.chargeTotalUnit));
                  if(sumValue !== chargeUnit) {
                    hasErrors = true;
                  }
            }            
          }
          if(isEmptyOrUndefine(row.costofSalesStatus) || row.costofSalesStatus !== 'X') {
            if(requiredNumeric(row.payUnit)){            
              hasErrors = true;
            }
            if(requiredNumeric(row.payRate)){            
              hasErrors = true;
            }
            const isCHUser = (this.props.isCoordinatorCompany && this.props.isInterCompanyAssignment ? true : false);
            if(!requiredNumeric(row.payUnit) && row.payUnit > 0 && (!isCHUser && (requiredNumeric(row.payRate) || row.payRate <= 0))) {
              hasErrors = true;
            }
            if(!requiredNumeric(row.payRate) && row.payRate > 0
                && !requiredNumeric(row.payUnit) && row.payUnit > 0
                && required(row.payRateCurrency)){            
                  hasErrors = true;
            }
          }          
        }          
      });      
    }
    return hasErrors;
  }

  validateTechSpecExpense() {
    let hasErrors = false;
    const { expenseLineItems } = this.props;
    if(!isEmptyOrUndefine(expenseLineItems)) {
      expenseLineItems.forEach(row => {
        if(row.recordStatus !== 'D') {
          if(required(row.expenseDate)){            
            hasErrors = true;
          }
          if(dateUtil.isUIValidDate(row.expenseDate)){
            hasErrors = true;
          }
          if(required(row.chargeExpenseType)){            
            hasErrors = true;            
          }
          if(isEmptyOrUndefine(row.invoicingStatus) || !(row.invoicingStatus === 'C' || row.invoicingStatus === 'D')) {
            if(requiredNumeric(row.chargeUnit)){            
              hasErrors = true;
            }
            if(requiredNumeric(row.chargeRate)){                   
              hasErrors = true;
            }
            if(!requiredNumeric(row.chargeUnit) && row.chargeUnit > 0 && !requiredNumeric(row.chargeRate) && row.chargeRate > 0) {     
                if(required(row.chargeRateCurrency)) hasErrors = true;
            }
            if(required(row.currency)) {
              hasErrors = true;
            }
          }
          if(isEmptyOrUndefine(row.costofSalesStatus) || row.costofSalesStatus !== 'X') {
            if(requiredNumeric(row.payUnit)){            
              hasErrors = true;
            }
            if(requiredNumeric(row.payRate)){                    
              hasErrors = true;
            }
            const isCHUser = (this.props.isCoordinatorCompany && this.props.isInterCompanyAssignment ? true : false);
            if(!requiredNumeric(row.payUnit) && row.payUnit > 0 && (!isCHUser && (requiredNumeric(row.payRate) || row.payRate <= 0))) {
              hasErrors = true;
            }
            if(requiredNumeric(row.payRateTax)){            
              hasErrors = true;
            }          
            if(!requiredNumeric(row.payRateTax)){
                const TaxMultiply = row.payUnit * row.payRate;            
                if(row.payRateTax > TaxMultiply) {                
                  hasErrors = true;        
                }            
            }
            if(!requiredNumeric(row.payUnit) && row.payUnit > 0 && !requiredNumeric(row.payRate) && row.payRate > 0) {            
              if(required(row.payRateCurrency)) hasErrors = true;  
            }
            if(required(row.currency)) {
              hasErrors = true;
            }
          }
        }
      });
    }
    return hasErrors;
  }

  validateTechSpecTravel() {
    let hasErrors = false;
    const { travelLineItems } = this.props;
    if(!isEmptyOrUndefine(travelLineItems)) {
      travelLineItems.forEach(row => {
        if(row.recordStatus !== 'D') {
          if(required(row.expenseDate)){            
            hasErrors = true;
          }
          if(dateUtil.isUIValidDate(row.expenseDate)){
            hasErrors = true;
          }
          if(required(row.chargeExpenseType)){            
            hasErrors = true;
          }
          if(required(row.payExpenseType)){            
            hasErrors = true;
          }
          if(isEmptyOrUndefine(row.invoicingStatus) || !(row.invoicingStatus === 'C' || row.invoicingStatus === 'D')) {
            if(requiredNumeric(row.chargeTotalUnit)){            
              hasErrors = true;
            }
            if(requiredNumeric(row.chargeRate)){            
              hasErrors = true;
            }
            if(!requiredNumeric(row.chargeTotalUnit) && row.chargeTotalUnit > 0 
              && !requiredNumeric(row.chargeRate) && row.chargeRate > 0
              && required(row.chargeRateCurrency)){            
                hasErrors = true;
            }
          }
          if(isEmptyOrUndefine(row.costofSalesStatus) || row.costofSalesStatus !== 'X') {
            if(requiredNumeric(row.payUnit)){            
              hasErrors = true;
            }
            if(requiredNumeric(row.payRate)){            
              hasErrors = true;
            }
            const isCHUser = (this.props.isCoordinatorCompany && this.props.isInterCompanyAssignment ? true : false);
            if(!requiredNumeric(row.payUnit) && row.payUnit > 0 && (!isCHUser && (requiredNumeric(row.payRate) || row.payRate <= 0))) {
              hasErrors = true;
            }
            if(!requiredNumeric(row.payUnit) && row.payUnit > 0
              && !requiredNumeric(row.payRate) && row.payRate > 0
              && required(row.payRateCurrency)){                     
                hasErrors = true;
            }
          }
        }
      });
    }
    return hasErrors;
  }

  validateTechSpecConsumable() {
    let hasErrors = false;
    const { consumableLineItems } = this.props;
    if(!isEmptyOrUndefine(consumableLineItems)) {
      consumableLineItems.forEach(row => {
        if(row.recordStatus !== 'D') {
          if(required(row.expenseDate)){            
            hasErrors = true;
          }
          if(dateUtil.isUIValidDate(row.expenseDate)){
            hasErrors = true;
          }
          if(required(row.chargeExpenseType)){            
            hasErrors = true;
          }
          if(required(row.payExpenseType)){            
            hasErrors = true;
          }
          if(isEmptyOrUndefine(row.invoicingStatus) || !(row.invoicingStatus === 'C' || row.invoicingStatus === 'D')) {
            if(requiredNumeric(row.chargeTotalUnit)){            
              hasErrors = true;
            }
            if(requiredNumeric(row.chargeRate)){            
              hasErrors = true;
            }
            if(!requiredNumeric(row.chargeTotalUnit) && row.chargeTotalUnit > 0 
              && !requiredNumeric(row.chargeRate) && row.chargeRate > 0
              && required(row.chargeRateCurrency)){            
                hasErrors = true;
            }
          }
          if(isEmptyOrUndefine(row.costofSalesStatus) || row.costofSalesStatus !== 'X') {
            if(requiredNumeric(row.payUnit)){            
              hasErrors = true;
            }
            if(requiredNumeric(row.payRate)){            
              hasErrors = true;
            }
            const isCHUser = (this.props.isCoordinatorCompany && this.props.isInterCompanyAssignment ? true : false);
            if(!requiredNumeric(row.payUnit) && row.payUnit > 0 && (!isCHUser && (requiredNumeric(row.payRate) || row.payRate <= 0))) {
              hasErrors = true;
            }
            if(!requiredNumeric(row.payUnit) && row.payUnit > 0
              && !requiredNumeric(row.payRate) && row.payRate > 0
              && required(row.payRateCurrency)){                     
                hasErrors = true;
            }
          }
        }
      });
    }
    return hasErrors;
  }

    validateTechSpecHasNoLineItems = () => {
      const {
        timeLineItems,
        travelLineItems,
        expenseLineItems,
        consumableLineItems,
        visitSelectedTechSpecs
      } = this.props;
       const techSpecsWithNoLineItems = [];
      if (visitSelectedTechSpecs.length > 0 &&
        timeLineItems.length === 0 && travelLineItems.length === 0 &&
        expenseLineItems.length === 0 && consumableLineItems.length === 0) {
        techSpecsWithNoLineItems.push(visitSelectedTechSpecs[0].label);
      }
      return techSpecsWithNoLineItems;
    }

    validateSave = () => {
      const validDoc = this.uploadedDocumentCheck();
      const valid = this.mandatoryFieldsValidationCheck();
     
      if (!valid||!validDoc) {
        return false;
      }
      const { 
        expenseLineItems
       } = this.props;
       
      if (this.validations["NoLineItems"]) {
        const techSpecsWithNoLineItems = this.validateTechSpecHasNoLineItems();
        if (techSpecsWithNoLineItems.length > 0) {
          const msg = StringFormat(modalMessageConstant.NO_VISIT_ENTRIES_FOR_TECHSPEC, techSpecsWithNoLineItems.join('\r\n'));
          this.saveConfirmHandler("NoLineItems", msg);
          return false;
        }
      }
    
      if (this.validations["hasZeroExpenses"] && expenseLineItems.length > 0) {
        const hasZeroExpenses = expenseLineItems.some(expense => {
          return (expense.chargeTotalUnit === 0 || expense.chargeRate === 0);
        });
        if (hasZeroExpenses) {
          this.saveConfirmHandler("hasZeroExpenses", modalMessageConstant.ZERO_CHARGE_RATE_OR_UNITS_VISIT);
          return false;
        }
      }
      const { visitInfo } = this.props;

      if (this.validations["visitAssignmentDateValidation"]) {
        
        if([ 'C','Q','T','U' ].includes(visitInfo.visitStatus)  && this.visitAssignmentDateValidation()) {
          this.visitAssignmentDateValidationConfirmation("visitAssignmentDateValidation");
          return false;
        }
      }

      if (this.validations["inValidCalendarEntries"]) {
      if (this.invalidCalendarData.length > 0){
        this.visitInvalidCalendarRemoveConfirmation('inValidCalendarEntries');
        return false;
      }
    }
      return true;
    }

    visitAssignmentDateValidation = () => {
      const { visitInfo, visitValidationData } = this.props;
      let isExists = false;
      if(visitValidationData && visitValidationData.visitAssignmentDates && visitValidationData.visitAssignmentDates.length > 0) {        
        const fromdate = moment(visitInfo.visitStartDate).format(localConstant.commonConstants.DATE_FORMAT);
        if(this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE) {
          visitValidationData.visitAssignmentDates.forEach(rowData => {              
            if((rowData.visitStatus === 'C' || rowData.visitStatus === 'Q' 
              || rowData.visitStatus === 'T' || rowData.visitStatus === 'U') 
              && moment(rowData.visitStartDate).format(localConstant.commonConstants.DATE_FORMAT) === fromdate) {
                isExists = true;
            }
          });
        } else {
          visitValidationData.visitAssignmentDates.forEach(rowData => {              
              if((rowData.visitStatus === 'C' || rowData.visitStatus === 'Q' 
                || rowData.visitStatus === 'T' || rowData.visitStatus === 'U') && rowData.visitId !== visitInfo.visitId 
                && moment(rowData.visitStartDate).format(localConstant.commonConstants.DATE_FORMAT) === fromdate) {
                  isExists = true;
              }
          });
        }
        return isExists;
      }
    }
    
    visitAssignmentDateValidationConfirmation = (validationType) => {      
      const confirmationObject = {
      title: modalTitleConstant.CONFIRMATION,
      message: modalMessageConstant.VISIT_ASSIGNMENT_DATE_VALIDATION,
      modalClassName: "warningToast",
      type: "confirm",
      buttons: [
        {
        buttonName: "Yes",
        onClickHandler: (e) => {
          if (validationType)
            this.validations[validationType] = false;
          this.visitSave(true);
        },
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

    visitInvalidCalendarRemoveConfirmation = (validationType) => {      
      const confirmationObject = {
      title: modalTitleConstant.CONFIRMATION,
      message: modalMessageConstant.VISIT_INVALID_CALENDAR_DATA_VALIDATION,
      modalClassName: "warningToast",
      type: "confirm",
      buttons: [
        {
        buttonName: "Yes",
        onClickHandler: (e) => {
          if (validationType)
          this.validations[validationType] = false;
          this.visitSave(true);
        },
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

    visitSaveClickHandler = () => {
      // const valid = this.mandatoryFieldsValidationCheck();
      // if (valid) {
      //   this.props.actions.SaveVisitDetails();
      // }
      this.visitSave();
    }

    hideAmendmentModal (e){  
     this.setState({
        isAmendment_Reason: false,
        });
      this.cancelVisit();
    }
    
   saveAmendment(e){
    amendmentreason=true;
   const data=this.inputRef.current.value;
   this.props.visitInfo['AmendmentReason']=data;
   this.setState({
      isAmendment_Reason: false,
    });
      amendmentreason=true;
      const errors=[];
      if(isEmpty(this.props.visitInfo['AmendmentReason'])){
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
      this.visitSave (e);
   }

  visitSave = async (closeConfirmDialog) => {
    const { timeLineItems, travelLineItems, expenseLineItems, consumableLineItems, VisitDocuments } = this.props;
    const doctypes = [ 'Report - Inspection', 'Report - Expediting', 'Report - Flash', 'Risk Assessment',
      'Non Conformance Report', 'Punch List', 'Timesheet - Client', 'Timesheet - Intertek', 'Timesheet - Moody',
      'Timesheet - Technical Specialist' ];
    const time = (timeLineItems.length > 0 && timeLineItems.filter(timeValue => ((timeValue.chargeTotalUnit !== "0.00" && timeValue.chargeTotalUnit !== 0) || (timeValue.payUnit !== "0.00" && timeValue.payUnit !== 0)) && timeValue.recordStatus !=='D').length !== 0);
    const travel = (travelLineItems.length > 0 && travelLineItems.filter(travelValue => ((travelValue.chargeTotalUnit !== "0.00" && travelValue.chargeTotalUnit !== 0) || (travelValue.payUnit !== "0.00" && travelValue.payUnit !== 0)) && travelValue.recordStatus !=='D').length !== 0);
    const expense = (expenseLineItems.length > 0 && expenseLineItems.filter(expenseValue => ((expenseValue.chargeUnit !== "0.00" && expenseValue.chargeUnit !== 0) || (expenseValue.payUnit !== "0.00" && expenseValue.payUnit !== 0)) && expenseValue.recordStatus !=='D').length !== 0);
    const consumable = (consumableLineItems.length > 0 && consumableLineItems.filter(consumableValue => ((consumableValue.chargeTotalUnit !== "0.00" && consumableValue.chargeTotalUnit !== 0) || (consumableValue.payUnit !== "0.00" && consumableValue.payUnit !== 0)) && consumableValue.recordStatus !=='D').length !== 0);
    const docVal = (VisitDocuments.length > 0 && VisitDocuments.filter(docType => doctypes.includes(docType.documentType) && docType.recordStatus !== "D").length !== 0);
    if (this.props.visitInfo.visitStatus == "D") {
      if (time || travel || expense || consumable || docVal) {
        IntertekToaster("This record cannot be set to Unused as there are either resource account entries added or excluded documents uploaded – please correct these and try again", 'warningToast VisitStatusTBA');
        return false;
      }
    }
    const isNewVisit = (this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE);
    if (closeConfirmDialog) {
      this.props.actions.HideModal();
    }
    if ((this.props.isInterCompanyAssignment) && (this.props.VisitInterCompanyDiscounts.ICDChange) && (this.props.VisitInterCompanyDiscounts.contractType === 'STD' || this.props.VisitInterCompanyDiscounts.contractType == 'IRF' || this.props.VisitInterCompanyDiscounts.contractType == 'FRW')) {
      if (!amendmentreason) {
        this.setState({
          isAmendment_Reason: true,
        });
      }
      if (amendmentreason) {
        const res = await this.props.actions.FetchVisitValidationData(isNewVisit);
        if (res) {
          const valid = this.validateSave();
          amendmentreason = false;
          if (valid) {
            const { visitInfo } = this.props;
            if (visitInfo && visitInfo.isFinalVisit === true) {
              this.isFinalVisitHandler();
            } else {
              this.isFinalVisit(false);
            }
          }
        }
      }
    }
    else {
      amendmentreason = true;
      const res = await this.props.actions.FetchVisitValidationData(isNewVisit);
      if (res) {
        const valid = this.validateSave();
        if (valid) {
          const { visitInfo } = this.props;
          if (visitInfo && visitInfo.isFinalVisit === true) {
            this.isFinalVisitHandler();
          } else {
            this.isFinalVisit(false);
          }
        }
      }
    }
  }
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

    isFinalVisit = async (closeConfirmDialog) => {      
      const status = this.getNextVisitStatus('fromSave');
      if (closeConfirmDialog) {
        this.props.actions.HideModal();
      }
      const res = await this.props.actions.SaveVisitDetails(status);
      this.callBackFuncs.loadCalenderData();
      if(res) {
         this.callBackFuncs.disableVisitStatus();
         this.callBackFuncs.reloadLineItemsOnSaveValidation();
      }

      for (const key in this.validations) {
        this.validations[key] = true;
      }      
    }

    isFinalVisitHandler = () => {
      const confirmationObject = {
        title: modalTitleConstant.CONFIRMATION,
        message: modalMessageConstant.VISIT_FINAL_VISIT_SAVE,
        modalClassName: "warningToast",
        type: "confirm",
        buttons: [
          {
            buttonName: "Yes",
            onClickHandler: (e) => this.isFinalVisit(true),
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

  visitUpdateStatus = async (e, isSave, from) => {
    e.preventDefault();
    this.props.actions.HideModal();
    if (isSave) {
      this.visitSave(true);
    } else {
      const status = this.getNextVisitStatus(from);
      if (from === 'approve' && status.visitStatus === 'A') {
        this.updatedData["visitApprovedByContractCompany"] = true;
      }      
      //dispatch a new action to update only state
      //before calling below method check if there is any change in status
      const obj = {
          status: status,
          isFromApproveOrReject:from,
          isValidationRequired:false,
          isSuccesAlert: true
      };
      if(from === 'approve' && 'A' === status.visitStatus && this.state.isTShasMultipleVisits.length === 0 ){        
        obj.isValidationRequired = true;  
        obj.isSuccesAlert = false;      
        const response = await this.props.actions.UpdateVisitStatus(obj);
        if (from === 'approve' && 'A' === status.visitStatus && response) {
          if (response.length > 0) {
            this.setState({
              isTShasMultipleVisits: response
            });            
            this.props.actions.HideModal();
            return false;
          } else{
            //start email processing             
            this.processApprovalEmailNotification();
          }
        }
      } else {
        if(from !== 'reject') await this.props.actions.UpdateVisitStatus(obj);
      }

      if(from === 'reject' && [ 'R', 'J' ].includes(status.visitStatus)) {
        this.openReasonForRejectPopup(e);
      }
    }
    this.props.actions.HideModal();
  }

  getEmailTemplateValue = ()=>{
    let {      
      visitStartDate,
    } = this.props.visitInfo;

    const visitOrTimesheet = `(${ this.props.visitInfo.visitAssignmentNumber.toString().padStart(5,'0') } : ${ this.props.visitInfo.visitReportNumber ? this.props.visitInfo.visitReportNumber : "N/A" })`;
    visitStartDate = visitStartDate?moment(visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT):'';    
    return {
      AssignmentNumber:this.props.visitInfo.visitAssignmentNumber.toString().padStart(5,'0'),
      Company:this.props.visitInfo.visitContractCompany,
      ContractNumber:this.props.visitInfo.visitContractNumber,
      CoordinatorName:this.props.loggedInUser,
      CustomerProjectName:this.props.visitInfo.visitCustomerProjectName,
      CustomerName:this.props.visitInfo.visitCustomerName,
      ProjectNumber:this.props.visitInfo.visitProjectNumber,
      ReportNumber:this.props.visitInfo.visitReportNumber,
      Supplier:this.props.visitInfo.visitSupplier,
      SupplierPO:this.props.visitInfo.visitSupplierPONumber,
      VisitDate:visitStartDate,
      VisitDatePeriod:this.props.visitInfo.visitDatePeriod,
      VisitOrTimesheet:localConstant.modalConstant.VISIT,
      VisitId:this.props.visitInfo.visitId,
      VisitURL: (isEmptyOrUndefine(configuration.extranetURL) ? "" : configuration.extranetURL + this.props.visitInfo.visitId)
    };
  }

  replaceEmailTemplatePlaceHolders = ()=>{
      let  emailTemplate  = this.props.emailTemplate;
      if(!emailTemplate){
        return "";
      }
      const templateValues = this.getEmailTemplateValue();
      for(const key in templateValues){
          const replacePlaceHolder = "@" + key + "@";
          const replaceValue = templateValues[key]?templateValues[key]:'';
          emailTemplate = replaceAll(emailTemplate, replacePlaceHolder, replaceValue);
      }
      return emailTemplate;
  }
  
    processApprovalEmailNotification = async () => {
      const showApprovalEmailPopup = await this.props.actions.ProcessApprovalEmailNotification();
      if (showApprovalEmailPopup) {
        const visitDocuments = this.props.VisitDocuments;
        this.attachedFiles = [];
        if(visitDocuments) {
          const customerVisibleDocuments = visitDocuments.filter(record => record.recordStatus !== "D" && record.subModuleRefCode === '0' && record.isVisibleToCustomer === true);
          if(customerVisibleDocuments && customerVisibleDocuments.length > 0) {
            this.attachedFiles = customerVisibleDocuments.map((doc)=>{
              return {
                documentUniqueName:doc.documentUniqueName,
                documentName:doc.documentName,
                moduleCode:doc.moduleCode,
                moduleRefCode:doc.moduleRefCode,
                documentSize: doc.documentSize,
                isFromModuleDocs:true
              };
            });
          }
        }
        const editorHtml = this.replaceEmailTemplatePlaceHolders();
        this.toAddress = this.props.customerReportingNotificationContant.map(e => e.email).join("; ");
        this.setState((state) => {
          return {
            isSendApprovalEmailNotification: !state.isSendApprovalEmailNotification,
            editorHtml:editorHtml
          };
        });
      } else {
        const status = this.getNextVisitStatus('approve');
        const obj ={
          status:status,
          isFromApproveOrReject:'approve',
          isValidationRequired:false,
          isSuccesAlert: true
        };
        this.props.actions.UpdateVisitStatus(obj);
      }
    }

    visitCancelHandler = () => {
      const confirmationObject = {
        title: modalTitleConstant.CONFIRMATION,
        message: modalMessageConstant.CANCEL_CHANGES,
        modalClassName: "warningToast",
        type: "confirm",
        buttons: [
          {
            buttonName: "Yes",
            onClickHandler: (e) => this.cancelVisit(e),
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

    cancelVisit = async () => {
      this.props.actions.HideModal();
      if(this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE) {
        this.props.actions.CancelCreateVisitDetails();
      } else {
        await this.props.actions.CancelEditVisitDetails();
      }      
      for (const key in this.validations) {
        this.validations[key] = true;
      }   
      this.props.actions.HideModal();              
      this.callBackFuncs.onVisitCancel();
      this.props.actions.ClearCalendarData();
      this.callBackFuncs.loadCalenderData(); 
      if (!isEmptyOrUndefine(this.props.visitInfo) && !isEmptyOrUndefine(this.props.visitInfo.visitStatus) 
      && this.props.visitInfo.visitStatus == "D"){
        this.props.actions.UpdateInteractionMode(true);
      }
    }

    confirmationRejectHandler = (e) => {
      this.updatedData = {
        rejectionDate: moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT)
      };
      this.props.actions.HideModal();
    }

    visitDeleteHandler = (e) => {
      const confirmationObject = {
        title: modalTitleConstant.CONFIRMATION,
        message: modalMessageConstant.VISIT_DELETE_MESSAGE,
        modalClassName: "warningToast",
        type: "confirm",
        buttons: [
          {
            buttonName: "Yes",
            onClickHandler: this.deleteVisit,
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
  
    deleteVisit = () => {
      this.props.actions.HideModal();
      if(this.invoiceChargeSideCheck()){
        if(this.invoicePaySideCheck()){
          this.props.actions.DeleteVisit()
         .then(response => {
           if (response) {
             if (response.code == 1) {
               this.props.history.push('/EditVisit');
               this.props.actions.DeleteAlert(response.result, "Visit");
             }
           }
       });
        } else{
          this.visitDeleteWarningHandler(modalMessageConstant.VISIT_DELETE_HANDLER_WARNING_FOR_PAY);
        }
      } else {
        this.visitDeleteWarningHandler(modalMessageConstant.VISIT_DELETE_HANDLER_WARNING);
      }
    }
    /** LineTimes Locked Condition Checking  (Ref by QA Team Mail - Visit & Timesheet delete validation on 29-05-2020)*/
    invoiceChargeSideCheck = ()=>{
      let flag = true;
      if(this.props.timeLineItems.length > 0 && this.props.timeLineItems.filter(timeValue => timeValue.invoicingStatus === "C" || timeValue.invoicingStatus === "D").length > 0){
            flag = false;
      }
      if(this.props.travelLineItems.length > 0 && this.props.travelLineItems.filter(travelValue => travelValue.invoicingStatus === "C" || travelValue.invoicingStatus === "D").length > 0){
            flag = false;
      } 
     if (this.props.expenseLineItems.length > 0 && this.props.expenseLineItems.filter(expenseValue => expenseValue.invoicingStatus === "C" || expenseValue.invoicingStatus === "D").length > 0){
            flag = false;
      } 
      if(this.props.consumableLineItems.length > 0 && this.props.consumableLineItems.filter(consumableValue => consumableValue.invoicingStatus === "C" || consumableValue.invoicingStatus === "D").length > 0){
            flag = false;
      }
      return flag;
    }

    invoicePaySideCheck = ()=>{
      let flag = true;
      if(this.props.timeLineItems.length > 0 && this.props.timeLineItems
                   .filter(timeValue => timeValue.costofSalesStatus === "X" 
                                    || timeValue.costofSalesStatus === "A"
                                    || timeValue.costofSalesStatus === "S" 
                                    || timeValue.costofSalesStatus === "R").length > 0){
            flag = false;
      }
      if(this.props.travelLineItems.length > 0 && this.props.travelLineItems
                  .filter(travelValue => travelValue.costofSalesStatus === "X"
                                    || travelValue.costofSalesStatus === "A"
                                    || travelValue.costofSalesStatus === "S" 
                                    || travelValue.costofSalesStatus === "R").length > 0){
            flag = false;
      } 
     if (this.props.expenseLineItems.length > 0 && this.props.expenseLineItems
                  .filter(expenseValue => expenseValue.costofSalesStatus === "X"
                                    || expenseValue.costofSalesStatus === "A"
                                    || expenseValue.costofSalesStatus === "S" 
                                    || expenseValue.costofSalesStatus === "R").length > 0){
            flag = false;
      } 
      if(this.props.consumableLineItems.length > 0 && this.props.consumableLineItems
                  .filter(consumableValue => consumableValue.costofSalesStatus === "X"
                                    || consumableValue.costofSalesStatus === "A"
                                    || consumableValue.costofSalesStatus === "S" 
                                    || consumableValue.costofSalesStatus === "R").length > 0){
            flag = false;
      }
      return flag;
    }

    visitDeleteWarningHandler = (value) => {
      const confirmationObject = {
        title: modalTitleConstant.ALERT_WARNING,
        message: value,
        modalClassName: "warningToast",
        type: "confirm",
        buttons: [
          {
            buttonName: "Revert all Changes",
            onClickHandler: this.cancelVisit,
            className: "modal-close m-1 btn-small"
          },
          {
            buttonName: "Continue Editing",
            onClickHandler: this.confirmationRejectHandler,
            className: "modal-close m-1 btn-small"
          }
        ]
      };
      this.props.actions.DisplayModal(confirmationObject);
    }

    visitClientReportingReqHandler = () => {
      this.setState({
        isClientReportReqsPopup:true
      });
    }

    visitSubmitHandler = (e) => {
      if (!this.props.isbtnDisable) {
        const confirmationObject = {
          title: modalTitleConstant.CONFIRMATION,
          //message: <p className="confirmTextWarp">{modalMessageConstant.VISIT_SAVE_CONFIRM}</p>,
          message: modalMessageConstant.VISIT_SAVE_CONFIRM,
          modalClassName: "warningToast",
          type: "confirm",
          buttons: [
            {
              buttonName: "Yes",
              onClickHandler: (event) => this.visitUpdateStatus(event, false, 'submit'),
              className: "modal-close m-1 btn-small"
            },
            {
              buttonName: "No",
              onClickHandler: (event) => this.confirmationRejectHandler(event),
              className: "modal-close m-1 btn-small"
            }
          ]
        };
        this.props.actions.DisplayModal(confirmationObject);
      }
      else {
        this.visitUpdateStatus(e, false, 'submit');
      }
    }

    visitApproveHandler = async (e) => {
      e.preventDefault();  
      const isNewVisit = (this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE);
      await this.props.actions.ProcessApprovalEmailNotification();
      const res = await this.props.actions.FetchVisitValidationData(isNewVisit);
      if(res) {
        const validDoc = this.uploadedDocumentCheck();
        const valid = this.mandatoryFieldsValidationCheck();
      if (!valid||!validDoc) {
        return false;
      }
      if(this.validateVisitOnApprove()) {
        this.setState({ visitStatus: this.props.visitInfo.visitStatus });        
          if (!this.props.isbtnDisable) {
            const confirmationObject = {
              title: modalTitleConstant.CONFIRMATION,
              //message: <p className="confirmTextWarp">{modalMessageConstant.VISIT_SAVE_CONFIRM}</p>,
              message: modalMessageConstant.VISIT_SAVE_CONFIRM,
              modalClassName: "warningToast",
              type: "confirm",
              buttons: [
                {
                  buttonName: "Yes",
                  onClickHandler: (event) => this.visitUpdateStatus(event, false, 'approve'),
                  className: "modal-close m-1 btn-small"
                },
                {
                  buttonName: "No",
                  //onClickHandler: (event) => this.visitApproveUpdateStatusConfirm(event),
                  onClickHandler: (event) => this.confirmationRejectHandler(event),
                  className: "modal-close m-1 btn-small"
                }
              ]
            };
            this.props.actions.DisplayModal(confirmationObject);
          }
          else {
            this.visitApproveUpdateStatusConfirm(e);
          }       
        } 
      }           
    }

    visitApproveUpdateStatusConfirm = async (e) => {
      const status = this.getNextVisitStatus('approve');
      const obj = {
        status: status,
        isFromApproveOrReject:'approve',
        isValidationRequired:true,
        isSuccesAlert: false
      };      
      const response = await this.props.actions.UpdateVisitStatus(obj);      
      if (response && response.length > 0) {
        this.setState({
          isTShasMultipleVisits: response
        });        
        return false;
      } else {
        const { isInterCompanyAssignment, isOperator, isOperatorCompany } = this.props;
        let message = modalMessageConstant.VISIT_APPROVE_CONFIRM_CHC;
        if (isInterCompanyAssignment && (isOperator || isOperatorCompany))
          message = modalMessageConstant.VISIT_APPROVE_CONFIRM_OC;
          const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: message,
            modalClassName: "warningToast",
            type: "confirm",
            buttons: [
              {
                buttonName: "Yes",
                onClickHandler: (event) => this.visitUpdateStatus(event, false, 'approve'),
                className: "modal-close m-1 btn-small"
              },
              {
                buttonName: "No",
                onClickHandler: this.TSHasMultipleBookingCancel,
                className: "modal-close m-1 btn-small"
              }
            ]
          };
          this.props.actions.DisplayModal(confirmationObject);
      }
    }

    visitRejectHandler = (e) => {
      e.preventDefault();
      if (!this.props.isbtnDisable) {
        const confirmationObject = {
          title: modalTitleConstant.CONFIRMATION,
          // message: <p className="confirmTextWarp">{modalMessageConstant.VISIT_SAVE_CONFIRM}</p>,
          message: modalMessageConstant.VISIT_SAVE_CONFIRM,
          modalClassName: "warningToast",
          type: "confirm",
          buttons: [
            {
              buttonName: "Yes",
              onClickHandler: (event) => this.visitUpdateStatus(event, false, 'reject'),
              className: "modal-close m-1 btn-small"
            },
            {
              buttonName: "No",
              // onClickHandler: (event) => this.openReasonForRejectPopup(event),
              onClickHandler: (event) => this.confirmationRejectHandler(event),
              className: "modal-close m-1 btn-small"
            }
          ]
        };
        this.props.actions.DisplayModal(confirmationObject);
      }
      else {
        this.openReasonForRejectPopup(e);
      }
    }

    openReasonForRejectPopup = (e) => {
      e.preventDefault();
      this.setState({
        rejectionDate: moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT),
        isRejectForRejectPopup: true
      });
      this.updatedData = {
        rejectionDate: moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT)
      };
    }

    visitCreateNewVisitHandler = () => {    
      //this.callBackFuncs.onCancel(); 
      //this.props.actions.CreateNewVisit(); 
      const selectedParamIds={
        vId:this.props.visitInfo.visitId,
        vAssId:this.props.visitInfo.visitAssignmentId,
        isCVisitFromEVist:true
      };
      const queryStr = ObjectIntoQuerySting(selectedParamIds);   
      window.open('/VisitDetails?'+queryStr,'_blank');
      //  this.props.history.push({
      //    pathname: '/VisitDetails',
      //    search:`?isCVisitFromEVist=${ true }`  
      //  });
    }

    saveConfirmHandler = (validationType, msg) => {      
      const confirmationObject = {
        title: modalTitleConstant.CONFIRMATION,
        //message: <p className="confirmTextWarp">{msg}</p> ,
        message: msg,
        modalClassName: "warningToast",
        type: "confirm",
        buttons: [
          {
            buttonName: "Yes",
            onClickHandler: (e) => {
              if (validationType)
                this.validations[validationType] = false;
              this.visitSave(true);
            },
            className: "modal-close m-1 btn-small"
          },
          {
            buttonName: "No",
            onClickHandler: (e) => this.confirmationRejectHandler(e),
            className: "modal-close m-1 btn-small"
          }
        ]
      };
      this.props.actions.DisplayModal(confirmationObject);
    }

    closeModalPopup = (e, type) => {      
      e.preventDefault();
      if (type === "closeClientReportReqsPopup") {
        this.setState({
          isClientReportReqsPopup: false
        });
      }
      if (type === "closeReasonForRejectPopup") {
        if(!isEmptyOrUndefine(this.updatedData["reasonForRejection"])) this.updatedData["reasonForRejection"] = "";
        this.setState({
          isRejectForRejectPopup: false,
          rejectionDate: moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT)
        });
        this.updatedData = {
          rejectionDate: moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT)
        };
      }      
      if (type === "closeApprovalEmailNotificationPopup") {
        this.TSHasMultipleBookingCancel();
        this.toAddress = '';
        this.setState({
          isSendApprovalEmailNotification: false,
          editorHtml: ''
        });
      }
      if(type === "TShasMultipleVisits"){
        this.setState({
          isTShasMultipleVisits:[]
        });
      }
    }

    inputHandleChange = (e) => {
      this.updatedData[e.target.name] = e.target.value;
    }

    submitRejectReason = (e) => {
      e.preventDefault();
      if(isEmptyOrUndefine(this.updatedData["reasonForRejection"])) {
        IntertekToaster(localConstant.validationMessage.VISIT_REASON_FOR_REJECTION, 'warningToast ReasonForRejection');
      } else {
        const nextStatus = this.getNextVisitStatus('reject');      
        const visitNotes = {
          note: modalTitleConstant.REASON_FOR_REJECTION + ": " + this.updatedData["reasonForRejection"],
          visibleToSpecialist: true,
          visibleToCustomer: false,
          recordStatus: 'N',
          createdBy: this.props.loggedInUserName,
          createdOn: this.updatedData["rejectionDate"],
          actionByUser: this.props.loggedInUserName
        };
        //dispatch a new action to update only state
        const obj ={
            status:nextStatus,
            isFromApproveOrReject:'reject',
            visitNotes: visitNotes,
            isSuccesAlert: true,
            ...this.updatedData
        };
        this.props.actions.UpdateVisitStatus(obj);
        this.closeModalPopup(e,'closeReasonForRejectPopup');
      }      
    }

    getNextVisitStatus = (isFrom) => {
      const {
        isInterCompanyAssignment,
        isOperator,
        isCoordinator,
        visitStatus,
        isOperatorCompany,
        isCoordinatorCompany
      } = this.props;
      let nextStatus = visitStatus;
  
      //common status updates      
      //Domestic
      if (!isInterCompanyAssignment) {
        if ('approve' === isFrom) {
          nextStatus = 'A';
        }
        else if ('reject' === isFrom) {
          nextStatus = 'J';
        }
        else {
          nextStatus = 'C';
        }
      }//Intercompany
      if (isInterCompanyAssignment) {
        // if (isOperator && 'approve' === isFrom) {
        //   nextStatus = 'O';
        // }
        // else if (isOperator && 'reject' === isFrom) {
        //   nextStatus = 'J';
        // }
        // else if ((isOperator || isOperatorCompany) && 'submit' === isFrom) {
        //   nextStatus = 'C';
        // }
        // else if (isCoordinator && 'approve' === isFrom) {
        //   nextStatus = 'A';
        // }
        // else if (isCoordinator && 'reject' === isFrom) {
        //   nextStatus = 'R';
        // }
        if (isOperatorCompany && 'approve' === isFrom) {
          nextStatus = 'O';
        }
        else if (isOperatorCompany && 'reject' === isFrom) {
          nextStatus = 'J';
        }
        else if (isOperatorCompany && 'submit' === isFrom) {
          nextStatus = 'C';
        }
        else if (isCoordinatorCompany && 'approve' === isFrom) {
          nextStatus = 'A';
        }
        else if (isCoordinatorCompany && 'reject' === isFrom) {
          nextStatus = 'R';
        }
        // else if (isOperatorCompany && 'approve' === isFrom) {
        //   nextStatus = 'O';
        // }
        // else if (isOperatorCompany && 'reject' === isFrom) {
        //   nextStatus = 'J';
        // }
        // else if (isCoordinatorCompany && 'approve' === isFrom) {
        //   nextStatus = 'A';
        // }
        // else if (isCoordinatorCompany && 'reject' === isFrom) {
        //   nextStatus = 'R';
        // }
      }
      return { visitStatus: nextStatus };
    }

    // getNextVisitStatus = (isFrom) => {
    //   const {
    //     isInterCompanyAssignment,
    //     isOperator,
    //     isCoordinator,
    //     visitStatus,
    //     timeLineItems,
    //     travelLineItems,
    //     expenseLineItems,
    //     consumableLineItems
    //   } = this.props;
    //   let nextStatus = visitStatus.value;
  
    //   //common status updates
    //   if ('fromSave' === isFrom) {
    //     if ('N'  === visitStatus.value &&
    //       !(timeLineItems.length > 0 || travelLineItems.length > 0 ||
    //         expenseLineItems.length > 0 || consumableLineItems.length > 0)) {
    //       nextStatus = 'C';
    //     }
    //   }
    //   else {
    //     //Domestic
    //     if (!isInterCompanyAssignment) {
    //       if ('C' === visitStatus.value && 'approve' === isFrom) {
    //         nextStatus = 'A';
    //       }
    //      else if ('C' === visitStatus.value && 'reject' === isFrom) {
    //         nextStatus = 'R';
    //       }
    //      else if (visitStatus.value === 'R') {
    //         nextStatus = 'C';
    //       }
    //     }//Intercompany
    //     if (isInterCompanyAssignment) {
    //       if (isOperator && 'C' === visitStatus.value && 'approve' === isFrom) {
    //         nextStatus = 'O';
    //       }
    //       else if (isOperator && 'C' === visitStatus.value && 'reject' === isFrom) {
    //         nextStatus = 'J';
    //       }
    //       else if (isOperator && 'J' === visitStatus.value && 'submit' === isFrom) {
    //         nextStatus = 'C';
    //       }
    //       else if (isCoordinator && 'O' === visitStatus.value && 'approve' === isFrom) {
    //         nextStatus = 'A';
    //       }
    //       else if (isCoordinator && 'O' === visitStatus.value && 'reject' === isFrom) {
    //         nextStatus = 'R';
    //       }
    //       else if (isOperator && 'R' === visitStatus.value && 'approve' === isFrom) {
    //         nextStatus = 'O';
    //       }
    //       else if (isOperator && 'R' === visitStatus.value && 'reject' === isFrom) {
    //         nextStatus = 'J';
    //       }
    //     }
    //   }
    //   return { visitStatus: nextStatus };
    // }

  visitRejectDateChange = (date) => {    
      this.updatedData["rejectionDate"] = moment(date).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
      this.setState({
        rejectionDate: moment(date).format(localConstant.commonConstants.SAVE_DATE_FORMAT)
      });
  }

  sendApprovalEmailNotification = (e, data) => {
    e.preventDefault();
    const customerContact = this.props.customerReportingNotificationContant;
    let invalidEmails = [];const validEmails = [];
     
    if (required(data.emailObj.emailToAddress)) {
      IntertekToaster(localConstant.validationMessage.EMAIL_TO_ADDRESS, 'warningToast timesheetEndDateVal');
      return false;
    }
    invalidEmails = validateEmail(data.emailObj.emailToAddress,validEmails);
    if (!required(data.emailObj.emailToAddress) && invalidEmails.length > 0) {
      IntertekToaster(`Email's ${ invalidEmails } are Invalid.`, 'warningToast invalidEmails');
      return false;
    }
    if (required(data.emailObj.emailSubject)) {
      IntertekToaster(localConstant.validationMessage.EMAIL_SUBJECT, 'warningToast timesheetEndDateVal');
      return false;
    }
    if (required(data.emailObj.editorHtml) || data.emailObj.editorHtml === '<p><br></p>') {
      IntertekToaster(localConstant.validationMessage.EMAIL_CONTENT, 'warningToast timesheetEndDateVal');
      return false;
    }
    
    const attachedDocuments = this.props.attachedDocs ? this.props.attachedDocs : [];
    if(attachedDocuments && attachedDocuments.length > 0) {
      let documentSize = 0;
      attachedDocuments.forEach(rowData => {
        if(rowData.documentSize) documentSize += rowData.documentSize;
      });
      if(documentSize > localConstant.commonConstants.DOCUMENT_SIZE) {
        IntertekToaster(localConstant.validationMessage.EMAIL_DOCUMENT_SIZE, 'dangerToast documentSize');
        return false;
      }
    }    

    this.toAddress = validEmails.map((email, i) => {
      const obj = {
        displayName: '',
        address: email
      };
      for (let i = 0, len = customerContact.length; i < len; i++) {
        if (customerContact[i].email === email) {
          obj.displayName = customerContact[i].contactPersonName;
          break;
        }
      }
      return obj;
    });
  
  const notificationData = {
      attachments: this.FormatAttachedFiles(attachedDocuments),
      emailContent:data.emailObj.editorHtml,
      emailSubject:data.emailObj.emailSubject,
      toAddress:this.toAddress 
    };
    const status = this.getNextVisitStatus('approve');    
      const obj ={
        status:status,
        isFromApproveOrReject:'approve',
        isValidationRequired:false,
        isSuccesAlert: true,
        emailContent: data.emailObj.editorHtml,
        attachments: this.FormatAttachedFiles(attachedDocuments),
        toAddress: this.toAddress ? this.toAddress : '',
        emailSubject: data.emailObj.emailSubject,
        dateSentToCustomer:moment().format('DD-MMM-YYYY')
    };
    this.props.actions.UpdateVisitStatus(obj);
    this.closeModalPopup(e, "closeApprovalEmailNotificationPopup");
  }

  FormatAttachedFiles(attachedFiles) {
    const docAttachments = [];
    if(attachedFiles && attachedFiles.length > 0) {
      attachedFiles.forEach(rowData => {
        const docAttachment = {};
        docAttachment.uniqueKey = rowData.documentUniqueName;
        docAttachment.AttachmentFileName = rowData.documentName;
        docAttachments.push(docAttachment);
      });
    }
    return docAttachments;
  }
  //Report Change Handler	
onReportChange = (e) => {	
  const result = formInputChangeHandler(e);	
  if (result.value === 'Visit Report') {	
    this.setState({	
      isShowVisitReport: true	
    });	
  } else {	
    this.setState({	
      isShowVisitReport: false	
    });	
  }	
}	
reportsClickHandler = ( ) => {	
  this.setState({ isShowVisitReport: false, isOpenReportModal: true });	
}	
HideModal =() =>	
{	
  this.setState ( { isOpenReportModal:false } );	
}	
  //To Download Report	
    GetReportHandler = async (e) => {	
      if (!this.props.isbtnDisable) {
        const confirmationObject = {
          title: modalTitleConstant.CONFIRMATION,
          message: modalMessageConstant.VISIT_TIMESHEET_REPORT_WARNING_MESSAGE,
          type: "confirm",
          modalClassName: "warningToast",
          buttons: [
            {
              buttonName: "Yes",
              onClickHandler: this.continueToVisitReport,
              className: "modal-close m-1 btn-small"
            },
            {
              buttonName: "No",
              onClickHandler: this.rejectVisitReport,
              className: "modal-close m-1 btn-small"
            }
          ]
        };
        this.props.actions.DisplayModal(confirmationObject); //Changes for D1287
      }
      else{
        this.getVisitReportHandler();
      }
    }

    getVisitReportHandler = () => {
      const _localStorage = localStorage.getItem('Username');
      let reportFileName = '';
      let params = {};
      params['username'] = _localStorage;
      if (this.state.isShowVisitReport) {
        params = {
          'VisitId': this.props.visitInfo.visitId,
          'format': 'EXCEL'
        };
        params['reportname'] = EvolutionReportsAPIConfig.Visit;
        reportFileName = 'Visit';
        DownloadReportFile(params, 'application/vnd.ms-excel', reportFileName, '.xls', this.props);
      }
    }

    continueToVisitReport = () => {
      this.props.actions.HideModal();
      this.getVisitReportHandler();
    } 

    rejectVisitReport = () => {
      this.props.actions.HideModal();
    }

    render() {
        const { currentPage,VisitDocuments } = this.props;
        this.visitTabData = this.filterTabDetail();
        const modelData = { isOpen: this.state.isOpen };
        const { assignmentClientReportingRequirements, visitAssignmentNumber, visitNumber,visitStartDate } = this.props.visitInfo;
          let interactionMode= this.props.interactionMode;
         if(this.props.pageMode===localConstant.commonConstants.VIEW){
          interactionMode=true;
         }
        return (
            <Fragment>
                            { this.state.isOpenReportModal ?   <Modal	
        title={localConstant.commonConstants.VISITREPORTS	}
         isShowModal={ this.state.isOpenReportModal }	
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
                optionsList={ localConstant.commonConstants.visitReportList }	
                optionName='name'	
                optionValue="value"	
                className="browser-default"	
                onSelectChange={ this.onReportChange }	
            />	
             {/* <button type="submit" className="mt-4x mr-2 modal-close waves-effect waves-green btn ">Get Report</button> */}	
             </div>	
                       	
         </Modal> :null }	
              <CustomModal modalData={modelData} />
                  {
              this.state.isTShasMultipleVisits.length > 0 ?
                <Modal title={modalTitleConstant.ALLOCATED_VISIT_TIMESHEET}
                  modalClass="visitModal"
                  modalId="TShasMultipleVisits"
                  formId="TShasMultipleVisits"
                  buttons={this.modelBtns.TShasMultipleVisits}
                  isShowModal={(this.state.isTShasMultipleVisits.length > 0)}>
                    <TSWithMultipleVisitPopup 
                    TShasMultipleVisits = {this.state.isTShasMultipleVisits}
                    date={visitStartDate}
                    />
                </Modal>
                : null
            }
                {this.state.isSendApprovalEmailNotification ? 
                  <ApprovedEmail 
                  emailSubject="Evolution2 - Visit Completed"
                  isemailTemplate = { this.state.isSendApprovalEmailNotification }
                  approvelEmailButtons={this.modelBtns.sendApprovalEmailNotificationBtns}
                  documentGridRowData={VisitDocuments.filter(record => record.recordStatus !== "D")}
                  documentGridHeaderData={this.documentHeaderData}
                  emailContent={this.state.editorHtml}
                  moduleCode="VST"
                  moduleCodeReference={this.props.visitInfo.visitId}
                  toAddress = {this.toAddress}     
                  attachedFiles = {this.attachedFiles}             
                  /> : null}
              {/* Client Reporting Requirements Popup */}
                {this.state.isClientReportReqsPopup ?
                  <Modal title={localConstant.assignments.CUSTOMER_REPORTING_REQUIREMENTS}
                    modalClass="visitModal"
                    modalId="clientReportingRequirementsPopup"
                    formId="clientReportingRequirementsForm"
                    buttons={this.modelBtns.clientReportRequirements}
                    isShowModal={this.state.isClientReportReqsPopup}>
                    <CustomInput
                      hasLabel={true}
                      divClassName='col pl-0 mb-4'
                      colSize="col s12"
                      type='textarea'
                      name="clientReportinfRequirements"
                      rows="10"
                      inputClass=" customInputs textAreaInView"
                      labelClass="customLabel"
                      maxLength={4000}
                      defaultValue={isEmpty(assignmentClientReportingRequirements) ? '' : assignmentClientReportingRequirements}
                      disabled={true}
                      readOnly={true}
                    />
                  </Modal> : null}
              {/* Client Reporting Popup */}              
              
              {/* Reject Popup START */}
                {this.state.isRejectForRejectPopup ?
                  <Modal
                    title={modalTitleConstant.REASON_FOR_REJECTION}
                    modalClass="visitModal"
                    modalId="reasonForRejectPopup"
                    formId="reasonForRejectForm"
                    buttons={this.modelBtns.rejectVisit}
                    isShowModal={this.state.isRejectForRejectPopup}>
                    <RejectReason
                      onValueChange={this.inputHandleChange}
                      visitRejectDateChange={this.visitRejectDateChange}
                      rejectionDate={this.state.rejectionDate}
                    />
                  </Modal> : null}
              {/* reject Popup END */}

              {this.state.errorList.length > 0 ?
                <Modal title={localConstant.commonConstants.CHECK_MANDATORY_FIELD}
                  titleClassName="chargeTypeOption"
                  modalContentClass="extranetModalContent"
                  modalClass="ApprovelModal"
                  modalId="errorListPopup"
                  formId="errorListForm"
                  buttons={this.modelBtns.errorListButton}
                  isShowModal={true}>
                  <ErrorList errors={this.state.errorList} />
                </Modal> : null
              } 
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
                  {this.state.ncrCloseOutDateList.length > 0 ?
                <Modal title={localConstant.visit.VISIT_OPEN_NCR}
                  titleClassName="chargeTypeOption"
                  modalContentClass="extranetModalContent"
                  modalId="errorListPopup"
                  formId="errorListForm"
                  buttons={this.modelBtns.errorListButton}
                  isShowModal={true}>
                  <ErrorList errors={this.state.ncrCloseOutDateList} />
                </Modal> : null
              }              
              {this.state.finalVisitValidationError.length > 0 ?
                <Modal title='Error'
                  titleClassName="chargeTypeOption"
                  modalContentClass="extranetModalContent"
                  modalId="errorListPopup"
                  formId="errorListForm"
                  buttons={this.modelBtns.finalValidationListButton}
                  isShowModal={true}>
                  <ErrorList errors={this.state.finalVisitValidationError} />
                </Modal> : null
              }
              {this.state.visitDateRangeValidation.length > 0 ?
                <Modal title={localConstant.login.EVOLUTION}
                  titleClassName="chargeTypeOption"
                  modalContentClass="extranetModalContent"
                  modalId="errorListPopup"
                  formId="errorListForm"
                  buttons={this.modelBtns.errorListButton}
                  isShowModal={true}>
                  <ErrorList errors={this.state.visitDateRangeValidation} />
                </Modal> : null
              }
             <SaveBarWithCustomButtons
                    codeLabel={localConstant.sideMenu.VISIT}                                       
                    codeValue={visitNumber ? `(${ visitAssignmentNumber.toString().padStart(5,'0') } : ${ this.props.visitInfo ? (this.props.visitInfo.visitReportNumber ? this.props.visitInfo.visitReportNumber : "N/A") : "" })` : ''}
                    currentMenu={localConstant.sideMenu.VISIT}
                    currentSubMenu= {this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE ?
                      localConstant.visit.CREATE_VISIT :
                      localConstant.visit.EDIT_VIEW_VISIT}
                    isbtnDisable={this.props.isbtnDisable}
                    buttons={this.handleVisitButtons()}
                    isbuttonDiv={true}
                    interactionMode={interactionMode}
                    activities={this.props.activities}
                />
            <div className="row ml-0 mb-0">
              <div className={(this.state.customSavebarPosition === true ? ((this.props.pageMode===localConstant.commonConstants.VIEW) ? "col s12 pl-0 pr-2 verticalTabs customSavebarPosition" :"col s12 pl-0 pr-2 verticalTabs customSavebarPosition") : "col s12 pl-0 pr-2 verticalTabs")}>
                <CustomTabs tabsList={this.visitTabData}
                  moduleName="visits"
                  currentPage={currentPage}
                  interactionMode={interactionMode}
                  callBackFuncs = {this.callBackFuncs}
                >
                </CustomTabs>
              </div>
            </div>
          </Fragment>
        );
    }
}

export default VisitDetails;
