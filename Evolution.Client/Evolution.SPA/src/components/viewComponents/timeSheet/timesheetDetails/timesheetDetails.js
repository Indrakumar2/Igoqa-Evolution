import React, { Component, Fragment } from 'react';
import { getlocalizeData, 
  isEmpty, 
  isEmptyReturnDefault, 
  isEmptyOrUndefine,
  isValidEmailAddress,
  parseQueryParam,
  isUndefined,
  ResetCurrentModuleTabInfo,
  isTrue,
  formInputChangeHandler,
  ObjectIntoQuerySting } from '../../../../utils/commonUtils';
import CustomTabs from '../../../../common/baseComponents/customTab';
import { SaveBarWithCustomButtons } from '../../../applicationComponents/saveBar';
import { timesheetTabDetails } from './timesheetTabsDetails';
import { required, requiredNumeric } from '../../../../utils/validator';
import moment from 'moment';
import dateUtil from '../../../../utils/dateUtil';
import { StringFormat,replaceAll } from '../../../../utils/stringUtil';
import arrayUtil from '../../../../utils/arrayUtil';
import Modal from '../../../../common/baseComponents/modal';
import ErrorList from '../../../../common/baseComponents/errorList';
import { modalTitleConstant, modalMessageConstant } from '../../../../constants/modalConstants';
import CustomModal from '../../../../common/baseComponents/customModal';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import ApprovedEmail from '../../../../common/approvalEmail';
import { HeaderData } from '../../../applicationComponents/timesheet/documents/documentHeader';
import { activitycode } from '../../../../constants/securityConstant';
import { ButtonShowHide } from '../.././../../utils/permissionUtil';
import TimesheetAncnor from '../timesheetAnchor';
import { configuration } from '../../../../appConfig';
import { EvolutionReportsAPIConfig  } from '../../../../apiConfig/apiConfig';
import { DownloadReportFile }  from '../../../../common/reportUtil/ssrsUtil';
const localConstant = getlocalizeData();

const validateEmail = (emails,validEmails =[])=>{
  const toAddress = emails.split(';');
  const invalidEmails = [];
    for(let i=0,len =toAddress.length;i<len;i++ ){
      const email = toAddress[i].trim();
      isValidEmailAddress(email)?
      validEmails.push(email):
      invalidEmails.push(email);
    };
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
    />
    <h6 className="col s12 p-0 mt-2">{modalTitleConstant.REASON_FOR_REJECTION}:</h6>
    <CustomInput
      hasLabel={true}     
      colSize="s12 mt-0"
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
function TSWithMultipleTimesheetPopup(props) {
  return (<Fragment>
    {
      props.TShasMultipleTimesheets.map(eachTs => {
        const tsName =  eachTs.techSpecName+'('+eachTs.pin+')';
        const msg = StringFormat(modalMessageConstant.TS_WITH_MULTIPLE_VISIT_TIMESHEET_ON_APPROVE,
          tsName, moment(props.date).format('DD-MMM-YYYY'));
        return (<Fragment>
          <p>{msg}</p>
          <ul className="tsOverBooking">
            {
              eachTs.resourceAdditionalInfos.map((eachObj) =>
                <li key={eachObj.timesheetJobReference}>
                  <TimesheetAncnor displayLinkText = {eachObj.timesheetJobReference} data={eachObj}/>
                </li>)
            }
          </ul>
        </Fragment>);
      })
    }
  </Fragment>);
};
//For Bug Id 563 -End

class TimesheetDetails extends Component {
   timesheetButtons=(buttonResponse) => [
    // {
    //   name: 'Email Template',
    //   clickHandler: (e) => this.timesheetEmailClickHandler(e),
    //   className: "btn-small mr-0 ml-2"
    // }, 
    {
      name: localConstant.commonConstants.SAVE,
      clickHandler: (e) => this.timesheetSaveHandler(e),
      className: "btn-small mr-0 ml-2",
      permissions:[ activitycode.NEW,activitycode.MODIFY ],
      isbtnDisable: this.props.isbtnDisable,
      showBtn:buttonResponse[0]
    },
     {
      name:localConstant.commonConstants.REFRESHCANCEL,
      clickHandler: (e) => this.timesheetCancelHandler(e),
      className: "btn-small mr-0 ml-2 waves-effect modal-trigger",
      permissions:[ activitycode.NEW,activitycode.MODIFY ],
      isbtnDisable: this.props.isbtnDisable,
      showBtn:buttonResponse[0]
    },
    {//EditMode
      name: localConstant.commonConstants.DELETE,
      clickHandler: (e) => this.timesheetDeleteHandler(e),
      className: "btn-small mr-0 ml-2 dangerBtn waves-effect modal-trigger",
      showBtn: !this.props.interactionMode,
      permissions:[ activitycode.DELETE ],
      showBtn:buttonResponse[1]
    },
     {//EditMode	
      name: localConstant.commonConstants.REPORTS,	
      clickHandler: this.reportsClickHandler,	
      className: "btn-small mr-0 ml-2 waves-effect modal-trigger",	
      //showBtn: !this.props.interactionMode,	
      // permissions:[ activitycode.DELETE ],	
      ///showBtn:buttonResponse[1] 	//Changes for D1307 //Commented because of ITK D1358
    },
    {
      name: localConstant.assignments.CLIENT_REPORTING_REQUIREMENTS,
      clickHandler: (e) => this.timesheetClientReportingReqHandler(e),
      className: "btn-small mr-0 ml-2 waves-effect modal-trigger",
      // showBtn: !this.props.interactionMode
      //showBtn:buttonResponse[0] //Commented because of ITK D1358
    },
    {//EditMode
      name: localConstant.commonConstants.SUBMIT,
      clickHandler: (e) => this.timesheetSubmitHandler(e),
      className: "btn-small mr-0 ml-2 waves-effect modal-trigger",
      // showBtn: !this.props.interactionMode
      showBtn:buttonResponse[1]
    },
    {
      name: localConstant.commonConstants.APPROVE,
      clickHandler: (e) => this.timesheetApproveHandler(e),
      className: "btn-small mr-0 ml-2 waves-effect modal-trigger",
      // showBtn: !this.props.interactionMode
      showBtn:buttonResponse[0]
    },
    {
      name: localConstant.commonConstants.REJECT,
      clickHandler: (e) => this.timesheetRejectHandler(e),
      className: "btn-small mr-0 ml-2 dangerBtn waves-effect modal-trigger",
      // showBtn: !this.props.interactionMode
      showBtn:buttonResponse[0]
    },
    {//EditMode
      name: localConstant.timesheet.NEW_TIMESHEET,
      clickHandler: (e) => this.timesheetCreateNewTimesheetHandler(e),
      className: "btn-small mr-0 ml-2 waves-effect modal-trigger",
      // showBtn: !this.props.interactionMode,
      permissions:[ activitycode.NEW ],
      showBtn:buttonResponse[1]  
    },
  ];
  constructor(props) {
    super(props);
    this.state = {
      errorList: [],
      isClientReportReqsPopup: false,
      isRejectForRejectPopup: false,
      customSavebarPosition:false,
      isSendApprovalEmailNotification:false,
      isTShasMultipleTimesheets:[],
      editorHtml: '',
      rejectionDate: moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT),
      timesheetStatus: '',
      isOpenReportModal: false,
      isShowVisitReport: false,
      isRejectSave: false,
      timesheetDateRangeValidation: [],
    };
    this.updatedData = {
      rejectionDate: moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT)
    };
    this.invalidCalendarData=[];
    this.validations = {
      'NoLineItems': true,
      'hasZeroExpenses': true,
      'timesheetAssignmentDateValidation': true,
      'inValidCalendarEntries':true
    };
    this.callBackFuncs ={
      onCancel:()=>{},
      onTimesheetCancel:()=>{},
      loadCalenderData:()=>{},
      reloadLineItemsOnSaveValidation:()=>{},
      disableTimesheetStatus:()=>{},
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
      clientReportRequirements: [
        {
          name: localConstant.commonConstants.CANCEL,
          action: (e) => this.closeModalPopup(e, "closeClientReportReqsPopup"),
          btnID: "clientReportReqBtn",
          btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
          showbtn: true,
          type:"button"
        }
      ],
      rejectTimesheet: [
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
          showbtn: true,
          type:"button"
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
        },
        {
          name: localConstant.commonConstants.SEND,
          action: this.sendApprovalEmailNotification,
          btnID: "clientReportReqBtn",
          btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
          showbtn: true,
          type:"button"
        }
      ],
      TShasMultipleTimesheets: [
        {
          name: localConstant.commonConstants.OK,
          action:(e)=>this.TShasMultipleOnApprove(e, 'approve'),
          btnID: "TShasMultipleTimesheetBtn",
          btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
          showbtn: true,
          type:"button"
        }
      ],
    };
    ResetCurrentModuleTabInfo(timesheetTabDetails);
    const functionRefs = {};
    functionRefs["enableEditColumn"] = ()=>{ return true;};
    this.documentHeaderData = HeaderData(functionRefs);
  }

  TShasMultipleOnApprove = (e, from) => {
    this.closeModalPopup(e, "TShasMultipleTimesheets");
    const confirmationObject = {
      title: modalTitleConstant.CONFIRMATION,
      message: modalMessageConstant.TIMESHEET_APPROVE_CONFIRM_CHC,
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
    this.updatedData["timesheetStatus"] = this.state.timesheetStatus;
    this.props.actions.UpdateTimesheetDetails(this.updatedData);
    this.updatedData = {
      rejectionDate: moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT)
    };
    this.props.actions.HideModal();
  }

//For Bug Id 563 -Start
  TShasMultipleBookingHandler = async(from)=>{
    const status = this.getNextTimesheetStatus('approve');    
    this.props.actions.HideModal();
    if(from === 'approve' && 'A' === status.timesheetStatus){
        this.processApprovalEmailNotification();
    } else if(from === 'approve' && 'O' === status.timesheetStatus){
        const obj ={
        status:status,
        isFromApproveOrReject:from,
        isValidationRequired:false,
        isSuccesAlert: true,
        isAuditUpdate: this.props.isbtnDisable
      };
      await this.props.actions.UpdateTimesheetStatus(obj);
    }
  }
  //For Bug Id 563 -End

    async componentDidMount() {
     const result = this.props.location.search && parseQueryParam(this.props.location.search);  
     const isFetchLookValues = true;
     this.props.actions.SetCurrentPageMode();    
     if(!isEmptyOrUndefine(result)) {
      if (isUndefined(result.tId)){
        if(!isTrue(result.isCTSheetFromETSheet)){         
          const data = { assignmentId:result.assignmentId ,isVisitTimesheet:true };
          const response = await this.props.actions.FetchAssignmentsDetailInfo(data);           
          if(response){           
            this.props.actions.SaveSelectedAssignmentId({ assignmentId:parseInt(result.assignmentId),assignmentNumber:parseInt(response.assignmentNumber) });             
            this.props.actions.FetchAssignmentForTimesheetCreation(parseInt(result.assignmentId),isFetchLookValues);
          }  
        }                 
      }else if(isTrue(result.isCTSheetFromETSheet)){             
        const response = await this.props.actions.FetchAssignmentsDetailInfo({ assignmentId:parseInt(result.AId),isVisitTimesheet:true  });
        if(response){
        this.props.actions.SaveSelectedAssignmentId({ assignmentId:parseInt(result.AId),assignmentNumber:parseInt(response.assignmentNumber) });
        this.props.actions.FetchAssignmentForTimesheetCreation(parseInt(result.AId));
        }
      }else{          
        //this.props.actions.FetchTimesheetTechnicalSpecialists(this.props.timesheetId);
        const selectedParamIds={
          timesheetId:result.tId,
          timesheetNumber:result.tNo,
          timesheetProjectNumber:result.tProNo,
          timesheetAssignmentId:result.tAssId,
          timesheetAssignmentNumber:result.tAssNo     
        };  
        this.props.actions.GetSelectedTimesheet(selectedParamIds);
        const newRes = await this.props.actions.FetchTimesheetDetail(selectedParamIds.timesheetId, isFetchLookValues);
        if (newRes) {
          this.callBackFuncs.loadCalenderData();
          this.callBackFuncs.disableTimesheetStatus();
        }
        if (!isEmptyOrUndefine(this.props.timesheetInfo) && !isEmptyOrUndefine(this.props.timesheetInfo.timesheetStatus) 
        && this.props.timesheetInfo.timesheetStatus == "E"){
          this.props.actions.UpdateInteractionMode(true);
        }
        else{
          // console.log("Fetch timesheet failed");
        }
      }
     }
               
    this.props.actions.FetchReferencetypes(this.props.isTimesheetCreateMode);
    if(this.props.timesheetInfo && this.props.timesheetInfo.isVisitOnPopUp && this.props.timesheetInfo.isVisitOnPopUp === true) this.timesheetClientReportingReqHandler(); //IGO QC 864 - client pop in timesheet
    this.setState({ customSavebarPosition:true });    
  }

  componentWillUnmount(){ 
    this.props.actions.ClearTimesheetDetails();
    this.props.actions.ClearTimesheetCalendarData();
  }

  handleTimesheetButtons() {        	
    const btnsToHide = [];		
    const {		
      currentPage,		
      isInterCompanyAssignment,				
      timesheetStatus,
      isCoordinatorCompany,
      isOperatorCompany				
    } = this.props;		
    const isInEditMode= (this.props.interactionMode ===false && currentPage===localConstant.timesheet.EDIT_VIEW_TIMESHEET_MODE);
    const isInViewMode=(this.props.pageMode===localConstant.commonConstants.VIEW)?true:false;
    const isInCreateMode=(this.props.interactionMode ===false && currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE);
    let buttonResponse;
    if(timesheetStatus === "E"){
      buttonResponse = ButtonShowHide(true,false,false);
    }
    else{
      buttonResponse = ButtonShowHide(isInEditMode,isInViewMode,isInCreateMode);
    }
    if (currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE) {		
      btnsToHide.push(		
        localConstant.commonConstants.DELETE,		
        localConstant.commonConstants.SUBMIT,		
        localConstant.timesheet.NEW_TIMESHEET,
        localConstant.commonConstants.APPROVE,		
        localConstant.commonConstants.REJECT		
      );		
    }		

    if (isInterCompanyAssignment && isOperatorCompany && [ 'O', 'A' ].includes(timesheetStatus.value)) {		
      btnsToHide.push(		
        localConstant.commonConstants.DELETE
      );		
    }

    if(isInterCompanyAssignment && isCoordinatorCompany && timesheetStatus.value !== "E"){
      btnsToHide.push(		
        localConstant.timesheet.NEW_TIMESHEET
      );
    }
   
    if ((isInterCompanyAssignment && isOperatorCompany && 'O' === timesheetStatus.value)		
      || (isInterCompanyAssignment && isCoordinatorCompany && [ 'C','N','R','J','E' ].includes(timesheetStatus.value))//intercompany timesheet and logged in as CHC with states 'Not Submitted, Awaiting Approval' CHC cannot approve or reject and Submit		
      || ('A' === timesheetStatus.value)) { // if timesheet is approved by CHC		
      btnsToHide.push(		
        localConstant.commonConstants.APPROVE,		
        localConstant.commonConstants.REJECT,		
        localConstant.commonConstants.SUBMIT,		
      );		
    }		
    else if (isInterCompanyAssignment 
      && ((isCoordinatorCompany && 'O' === timesheetStatus.value) 
      || (isOperatorCompany && 'R' === timesheetStatus.value))) {		
      btnsToHide.push(		
        localConstant.commonConstants.SUBMIT		
      );		
    }		
    else if (isInterCompanyAssignment 
      && isOperatorCompany && [ 'J', 'R' ].includes(timesheetStatus.value)) {		
      btnsToHide.push(		
        localConstant.commonConstants.APPROVE,		
        localConstant.commonConstants.REJECT		
      );		
    }		
    else if (!isInterCompanyAssignment && [ 'R', 'J' ].includes(timesheetStatus.value)) {		
      btnsToHide.push(		
        localConstant.commonConstants.APPROVE,		
        localConstant.commonConstants.REJECT		
      );		
    }		
    else if ([ 'C' ].includes(timesheetStatus.value)) {		
      btnsToHide.push(		
        localConstant.commonConstants.SUBMIT		
      );	
      if(this.props.selectedTimesheetStatus === 'N') {
        btnsToHide.push(
          localConstant.commonConstants.APPROVE,		
          localConstant.commonConstants.REJECT	
        );
      }	
    }		
    else if ([ 'N' ].includes(timesheetStatus.value)) {		
      if (currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE) {
        btnsToHide.push(		
          localConstant.commonConstants.SUBMIT          		
        );
      } else {
        btnsToHide.push(
          localConstant.commonConstants.APPROVE,		
          localConstant.commonConstants.REJECT	
        );
      }
      		
    }
    else if('E' === timesheetStatus.value) {
      btnsToHide.push(		
        localConstant.commonConstants.APPROVE,		
        localConstant.commonConstants.REJECT,		
        localConstant.commonConstants.SUBMIT,		
      );
      buttonResponse[1]= true;		
    }		
    return arrayUtil.negateFilter(this.timesheetButtons(buttonResponse), 'name', btnsToHide);		
  };		

  filterTabDetail = () => {
    const tabsToHide = [];
    tabsToHide.push(...[
      "Invoice"
    ]);
    if (this.props.currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE) {
      tabsToHide.push("Documents");
    }
    const timesheetTabs = arrayUtil.negateFilter(timesheetTabDetails, 'tabBody', tabsToHide);
    return timesheetTabs;
  }

  closeErrorList = (e) => {
    e.preventDefault();
    this.setState({
      errorList: [],
      timesheetDateRangeValidation: []
    });
  }

  uploadedDocumentCheck=()=>{  // when file Uploading user click on sve button showing warning Toaster Here 
    let count = 0;
    if (Array.isArray(this.props.timesheetDocuments) && this.props.timesheetDocuments.length > 0) {
        this.props.timesheetDocuments.map(document =>{                             
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
    const errors = [], { timesheetInfo, timesheetSelectedTechSpecs, timesheetTechnicalSpecilists, timesheetValidationData } = this.props;
    this.invalidCalendarData=[];
    if (timesheetInfo) {
      if (required(timesheetInfo.timesheetStartDate)) {
        errors.push(`${ localConstant.timesheet.GENERAL_DETAILS } - ${ localConstant.timesheet.DATE_FROM }`);
      }
      if (required(timesheetInfo.timesheetEndDate)) {
        errors.push(`${ localConstant.timesheet.GENERAL_DETAILS } - ${ localConstant.timesheet.DATE_TO }`);
      }
      if (isEmpty(timesheetSelectedTechSpecs) || timesheetSelectedTechSpecs.filter(calendarData => calendarData.recordStatus !== 'D').length === 0) {
        errors.push(`${ localConstant.timesheet.GENERAL_DETAILS } - ${ localConstant.timesheet.RESOURCE }`);
      }
      if(!isEmpty(this.props.timesheetDocuments)){
        const issueDoc = [];
        this.props.timesheetDocuments.map(document =>{
            if (isEmpty(document.documentType) && document.recordStatus!=='D' && document.subModuleRefCode === '0') { //ID 507 #2 deleted few documents without selecting “Document type”, 
                                                                                 //when click on save validation showing alert box
                errors.push(`${ localConstant.timesheet.documents.DOCUMENT } - ${ document.documentName } - ${ localConstant.timesheet.documents.SELECT_FILE_TYPE } `);
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
      if (timesheetInfo.timesheetStatus === 'E' && required(timesheetInfo.unusedReason)) {
        errors.push(`${ localConstant.timesheet.GENERAL_DETAILS } - ${ localConstant.timesheet.REASON }`);
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
      let timesheetRange;
      let hasTimesheetDate = true;
      if(timesheetValidationData && timesheetValidationData.timesheetAssignmentDates && timesheetValidationData.timesheetAssignmentDates.length > 0) {
        let currentTimesheetIndex = (this.props.currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE ? timesheetValidationData.timesheetAssignmentDates.length 
                                  : timesheetValidationData.timesheetAssignmentDates.findIndex(x => x.timesheetId === timesheetInfo.timesheetId));   
        if(currentTimesheetIndex >= 0){
          const previousTimesheet =  timesheetValidationData.timesheetAssignmentDates.filter(x => [ "N", "Q", "T", "U", "W", "E" ].includes(x.timesheetStatus));          
          if(previousTimesheet && previousTimesheet.length > 0) { 
            const techIds = [];  
            if (this.props.timesheetSelectedTechSpecs) {
              if (Array.isArray(this.props.timesheetSelectedTechSpecs) && this.props.timesheetSelectedTechSpecs.length > 0) {
                  this.props.timesheetSelectedTechSpecs.map(tech => {
                      if (tech.recordStatus !== 'D')
                          techIds.push(tech.value);
                  });
              }
            }
            
            for(let i=0; i < techIds.length; i++) {
              const TechSpecTImesheets = [];
              if (timesheetValidationData.technicalSpecialists) {
                if (Array.isArray(timesheetValidationData.technicalSpecialists) && timesheetValidationData.technicalSpecialists.length > 0) {
                  timesheetValidationData.technicalSpecialists.map(tech => {
                        if (techIds[i] === tech.pin || tech.timesheetId === timesheetInfo.timesheetId) TechSpecTImesheets.push(tech.timesheetId);
                    });
                }
              }
              const previousTimesheetList = previousTimesheet.filter(x => TechSpecTImesheets.includes(x.timesheetId));
              currentTimesheetIndex = previousTimesheetList.findIndex(x => x.timesheetId === timesheetInfo.timesheetId);
              if(this.props.currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE && previousTimesheetList.length > 0) {
                currentTimesheetIndex = previousTimesheetList.length - 1;
              }
              if(currentTimesheetIndex === 0 && previousTimesheetList.length > 1) {
                const nextTimesheet = previousTimesheetList[currentTimesheetIndex + 1];
                const currentDate = moment(timesheetInfo.timesheetStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT);
                const nextDate = moment(nextTimesheet.timesheetStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT); 
                if(nextTimesheet && moment(currentDate).isAfter(moment(nextDate))) {
                  previousDateRanges.push([ moment(nextTimesheet.timesheetStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT) ]);
                  timesheetRange = 1;
                  hasTimesheetDate = false;
                }
              } else if(((this.props.currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE && currentTimesheetIndex >= 0)
                  || (this.props.currentPage === localConstant.timesheet.EDIT_VIEW_TIMESHEET_MODE && currentTimesheetIndex > 0)) 
                  && currentTimesheetIndex === previousTimesheetList.length - 1) {                
                currentTimesheetIndex = (this.props.currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE ? previousTimesheetList.length - 1 : previousTimesheetList.length - 2);
                //const previousTimesheet = previousTimesheetList[currentTimesheetIndex];
                const currentDate = moment(timesheetInfo.timesheetStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT);     
                previousTimesheetList.forEach( row => {
                  const previousDate = moment(row.timesheetStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT);
                  if(row && (row.timesheetNumber !== timesheetInfo.timesheetNumber) && moment(currentDate).isBefore(moment(previousDate))) {
                    nextDateRanges.push([ moment(row.timesheetStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT) ]);
                    timesheetRange = 2;
                    hasTimesheetDate = false;
                  }
                });
              } else if(currentTimesheetIndex > 0 && currentTimesheetIndex < previousTimesheetList.length - 1) {
                const previousTimesheet = previousTimesheetList[currentTimesheetIndex - 1];
                const futureTimesheet = previousTimesheetList[currentTimesheetIndex + 1];
                const currentDate = moment(timesheetInfo.timesheetStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT);
                const previousDate = moment(previousTimesheet.timesheetStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT);
                const nextDate = moment(futureTimesheet.timesheetStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT);
                if(previousTimesheet && futureTimesheet && 
                  (moment(currentDate).isBefore(moment(previousDate)) || moment(currentDate).isAfter(moment(nextDate)))) {
                  previousDateRanges.push([ moment(previousTimesheet.timesheetStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT) ]);
                  nextDateRanges.push([ moment(futureTimesheet.timesheetStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT) ]);
                  timesheetRange = 3;
                  hasTimesheetDate = false;
                } else {
                  previousDateRanges.push([ moment(previousTimesheet.timesheetStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT) ]);
                  nextDateRanges.push([ moment(futureTimesheet.timesheetStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT) ]);
                }
              }
            }
          }                    
        }
      }
      if(!hasTimesheetDate && timesheetInfo && timesheetInfo.timesheetStatus !== 'E') {
        let minDate = '';
        let maxDate = '';
        if(timesheetRange === 1) {
          previousDateRanges.forEach( row => {
            minDate = (minDate === '' || moment(minDate).isAfter(moment(row)) ? row : minDate);
          });
          dateRangeValidation.push(`${ StringFormat(localConstant.timesheet.FIRST_TIMESHEET_DATE_RANGE_VALIDATION, minDate) }`);
          this.setState({
            timesheetDateRangeValidation: dateRangeValidation
          });
        } else if (timesheetRange === 2) {
          nextDateRanges.forEach( row => {
            maxDate = (maxDate === '' || moment(maxDate).isBefore(moment(row)) ? row : maxDate);
          });
          dateRangeValidation.push(`${ StringFormat(localConstant.timesheet.CREATE_TIMESHEET_DATE_RANGE_VALIDATION, maxDate) }`);
          this.setState({
            timesheetDateRangeValidation: dateRangeValidation
          });
        } else if (timesheetRange === 3) {
          previousDateRanges.forEach( row => {
            minDate = (minDate === '' || moment(minDate).isBefore(moment(row)) ? row : minDate);
          });
          nextDateRanges.forEach( row => {
            maxDate = (maxDate === '' || moment(maxDate).isAfter(moment(row)) ? row : maxDate);
          });
          dateRangeValidation.push(`${ StringFormat(localConstant.timesheet.EDIT_TIMESHEET_DATE_RANGE_VALIDATION, minDate, maxDate) }`);
          this.setState({
            timesheetDateRangeValidation: dateRangeValidation
          });
        }
        return false;
      }
    }

    if (errors.length < 1) {
      if (this.props.currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE) {
        const filterTimesheetCalendarData = this.props.timesheetCalendarData.filter(calendarData => calendarData.recordStatus !== 'D');
        if (filterTimesheetCalendarData.length === 0)
          errors.push(`${ localConstant.visit.AT_LEAST_ONE_CALENDAR_ENTRY_TIMEHSEET_VALIDATION }`);
        else {
          const validCalendarData = [];
          const validSaveCalendarData=[];
          const startDate = moment(timesheetInfo.timesheetStartDate).format(localConstant.commonConstants.DATE_FORMAT);
          const endDate = moment(timesheetInfo.timesheetEndDate).format(localConstant.commonConstants.DATE_FORMAT);
          filterTimesheetCalendarData.forEach((calendarData) => {
            const eventStartDate = moment(calendarData.startDateTime).format(localConstant.commonConstants.DATE_FORMAT);
            const eventEndDate = moment(calendarData.endDateTime).format(localConstant.commonConstants.DATE_FORMAT);
            const isValidStartBetween = moment(calendarData.startDateTime).isBetween(timesheetInfo.timesheetStartDate, timesheetInfo.timesheetEndDate);
            const isValidEndBetween = moment(calendarData.endDateTime).isBetween(timesheetInfo.timesheetStartDate, timesheetInfo.timesheetEndDate);
            const isValidStart = (eventStartDate == startDate || eventStartDate == endDate);
            const isValidEnd = (eventEndDate == endDate || eventEndDate == startDate);
            if ((isValidStartBetween && isValidEndBetween) || (isValidStart && isValidEnd) || (isValidStartBetween&&isValidEnd) || (isValidEndBetween&&isValidStart)) {
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
          errors.push(`${ localConstant.visit.AT_LEAST_ONE_CALENDAR_ENTRY_TIMEHSEET_VALIDATION }`);
          else{
            this.props.actions.SaveValidTimesheetCalendarDataForSave(validSaveCalendarData);
          }
        }
      }
      else
      this.props.actions.SaveValidTimesheetCalendarDataForSave(this.props.timesheetCalendarData);
    }
    if (errors.length > 0) {
      this.setState({
        errorList: errors
      });
      return false;
    }
    else {
      if (!moment(timesheetInfo.timesheetStartDate).isValid()) {
        IntertekToaster(localConstant.validationMessage.INVALID_TIMESHEET_START_DATE, 'warningToast TimesheetEndDateVal');
        return false;
      }
      if (!moment(timesheetInfo.timesheetEndDate).isValid()) {
        IntertekToaster(localConstant.validationMessage.INVALID_TIMESHEET_END_DATE, 'warningToast timesheetEndDateVal');
        return false;
      }
      if (!required(timesheetInfo.timesheetExpectedCompleteDate) && !moment(timesheetInfo.timesheetExpectedCompleteDate).isValid()) {
        IntertekToaster(localConstant.validationMessage.INVALID_TIMESHEET_EXPECTED_COMPLETED_DATE, 'warningToast timesheetEndDateVal');
        return false;
      }
      if (moment(timesheetInfo.timesheetStartDate).isAfter(timesheetInfo.timesheetEndDate, 'day')) {
        IntertekToaster(localConstant.validationMessage.TIMESHEET_START_DATE_END_DATE_MISMATCH, 'warningToast timesheetStartDateEndDateMismatchVal');
        return false;
      }
      //date diff  
      if (Math.abs(moment(timesheetInfo.timesheetStartDate).diff(timesheetInfo.timesheetEndDate, 'days')) > 365) {
        const validateDate = moment(timesheetInfo.timesheetStartDate).add(365, 'days').format(localConstant.commonConstants.UI_DATE_FORMAT);
        IntertekToaster(`${ localConstant.validationMessage.TIMESHEET_ENDDATE_IS_TO_FAR } ${ validateDate }`, 'warningToast timesheetvalidateEndDateMismatchVal');
        return false;
      }
      if (timesheetInfo.timesheetCompletedPercentage && (timesheetInfo.timesheetCompletedPercentage < 0 || timesheetInfo.timesheetCompletedPercentage > 100)) {
        IntertekToaster(localConstant.validationMessage.PERCENTAGE_COMPLETED, 'warningToast PercentRange');
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
            if(required(row.currency)){            
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
            const payUnit = isNaN(parseFloat(row.payUnit))?0:parseFloat(row.payUnit),
            payRate = isNaN(parseFloat(row.payRate))?0:parseFloat(row.payRate),
            payRateTax = isNaN(parseFloat(row.payRateTax))?0:parseFloat(row.payRateTax); 
            if(payRateTax > (payUnit * payRate )){            
              hasErrors = true;
            }
            if(!requiredNumeric(row.payUnit) && row.payUnit > 0 && !requiredNumeric(row.payRate) && row.payRate > 0) {              
              if(required(row.payRateCurrency)) hasErrors = true;
            }
            if(required(row.currency)){            
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
            if(requiredNumeric(row.chargeUnit)){            
              hasErrors = true;
            }
            if(requiredNumeric(row.chargeRate)){            
              hasErrors = true;
            }
            if (!requiredNumeric(row.chargeUnit) && row.chargeUnit > 0
              && !requiredNumeric(row.chargeRate) && row.chargeRate > 0
              && required(row.chargeRateCurrency)) {            
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
            if (!requiredNumeric(row.payUnit) && row.payUnit > 0
              && !requiredNumeric(row.payRate) && row.payRate > 0
              && required(row.payRateCurrency)) {            
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
          if(isEmptyOrUndefine(row.invoicingStatus) || !(row.invoicingStatus === 'C' || row.invoicingStatus === 'D')) {
            if(requiredNumeric(row.chargeUnit)){            
              hasErrors = true;
            }
            if(requiredNumeric(row.chargeRate)){            
              hasErrors = true;
            }
            if (!requiredNumeric(row.chargeUnit) && row.chargeUnit > 0
              && !requiredNumeric(row.chargeRate) && row.chargeRate > 0
              && required(row.chargeRateCurrency)) {            
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
            if (!requiredNumeric(row.payUnit) && row.payUnit > 0
              && !requiredNumeric(row.payRate) && row.payRate > 0
              && required(row.payRateCurrency)) {              
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
      timesheetSelectedTechSpecs
    } = this.props;
    let timesheetTechnicalSpecilists = isEmptyReturnDefault(this.props.timesheetTechnicalSpecilists);
    timesheetTechnicalSpecilists = timesheetTechnicalSpecilists.length === 0 ?
    timesheetSelectedTechSpecs:timesheetTechnicalSpecilists;
     const techSpecsWithNoLineItems = [];
    // if (timesheetSelectedTechSpecs.length > 0 &&
    //   timeLineItems.length === 0 && travelLineItems.length === 0 &&
    //   expenseLineItems.length === 0 && consumableLineItems.length === 0) {
    //   techSpecsWithNoLineItems.push(timesheetSelectedTechSpecs[0].label);
    // }
    //loop through each technical Specialist and validate
    //No timesheet entries for Tech
    for (let i = 0, len = timesheetTechnicalSpecilists.length; i < len; i++) {
      if(timesheetTechnicalSpecilists[i].recordStatus !== 'D') {
        const eachTS = timesheetTechnicalSpecilists[i];
        if(eachTS.technicalSpecialistName.includes('(')){
          eachTS.technicalSpecialistName = eachTS.technicalSpecialistName.substring(0, eachTS.technicalSpecialistName.indexOf('('));
        }
        const tsTimeLineItems = timeLineItems.filter(x => x.pin == eachTS.pin && x.recordStatus !== 'D' && x.chargeRate !== "0.0000" && x.chargeTotalUnit !== "0.00");
        const tsTravelLineItems = travelLineItems.filter(x => x.pin == eachTS.pin && x.recordStatus !== 'D' && x.chargeRate !== "0.0000" && x.chargeTotalUnit !== "0.00");
        const tsExpenseLineItems = expenseLineItems.filter(x => x.pin == eachTS.pin && x.recordStatus !== 'D' && x.chargeRate !== "0.0000" && x.chargeTotalUnit !== "0.00");
        const tsConsumableLineItems = consumableLineItems.filter(x => x.pin == eachTS.pin && x.recordStatus !== 'D' && x.chargeRate !== "0.0000" && x.chargeTotalUnit !== "0.00");
        if (tsTimeLineItems.length === 0 && tsTravelLineItems.length === 0 &&
          tsExpenseLineItems.length === 0 && tsConsumableLineItems.length === 0) {
          //need to changnge the logic
          techSpecsWithNoLineItems.push(eachTS.technicalSpecialistName ? eachTS.technicalSpecialistName + ' (' + eachTS.pin + ')' : eachTS.label);
        }
      }
    }

    let techSpecData = '';
    if (techSpecsWithNoLineItems.length > 0) {
      let x = 0;
      while (x < techSpecsWithNoLineItems.length) {
        if (x % 3 == 2) {
          techSpecData = techSpecData + techSpecsWithNoLineItems[x] + '; \r\n';
        } else {
          techSpecData =  techSpecData + techSpecsWithNoLineItems[x] + '; ';
        }         
        x++;
      }
    }
    return techSpecData;
  }
  
  //Approval Modal Popup
  timesheetEmailClickHandler = (e) => {
    e.preventDefault();
    this.processApprovalEmailNotification();
  }
  
  validateSave = (from, reject) => {
    const validDoc = this.uploadedDocumentCheck();
    const valid = this.mandatoryFieldsValidationCheck();
    if (!valid || !validDoc) {
      return false;
    }
    const { 
      expenseLineItems,
      isTimesheetCreateMode,
      timesheetInfo
     } = this.props;

     if(isTimesheetCreateMode && 'C' === timesheetInfo.assignmentStatus){
      IntertekToaster(localConstant.validationMessage.TIMESHEET_ASSIGNMENT_COMPLETE, 'warningToast timesheetEndDateVal');
      return false;
     }
     
    if (this.validations["NoLineItems"]) {
      const techSpecsWithNoLineItems = this.validateTechSpecHasNoLineItems();
      if (techSpecsWithNoLineItems != '') {
        const msg = StringFormat(modalMessageConstant.NO_TIMESHEET_ENTRIES_FOR_TECHSPEC, techSpecsWithNoLineItems);
        this.saveConfirmHandler("NoLineItems", msg, from, reject);
        return false;
      }
    }
  
    if (this.validations["hasZeroExpenses"] && expenseLineItems.length > 0) {
      const hasZeroExpenses = expenseLineItems.some(expense => {
        return ( (isNaN(parseFloat(expense.chargeUnit))?0:parseFloat(expense.chargeUnit)) <= 0 
              || (isNaN(parseFloat(expense.chargeRate))?0:parseFloat(expense.chargeRate)) <= 0);
      });
      if (hasZeroExpenses) {
        this.saveConfirmHandler("hasZeroExpenses", modalMessageConstant.ZERO_CHARGE_RATE_OR_UNITS, from, reject);
        return false;
      }
    }
    if (this.validations["timesheetAssignmentDateValidation"]) {
      const { timesheetStatus } = this.props;
      if([ 'N','C' ].includes(timesheetStatus.value) && this.timesheetAssignmentDateValidation()) {
        // this.timesheetAssignmentDateValidationConfirmation("timesheetAssignmentDateValidation");
        this.saveConfirmHandler("timesheetAssignmentDateValidation", modalMessageConstant.TIMESHEET_ASSIGNMENT_DATE_VALIDATION, from, reject);
        return false;
      }
    }
    
    if (this.validations["inValidCalendarEntries"]) {
      if (this.invalidCalendarData.length > 0){
        this.timesheetInvalidCalendarRemoveConfirmation('inValidCalendarEntries');
        return false;
      }
    }
    return true;
  }
  //Timesheet Save handler
  timesheetSaveHandler = (e) => {
    // e.preventDefault();
    this.setState({
      isRejectSave: false
    });
    this.timesheetSave(false, 'fromSave', false);
  }

  timesheetSave = async (closeConfirmDialog, from, reject) => {
    const { timeLineItems, travelLineItems, expenseLineItems, consumableLineItems, timesheetDocuments } = this.props;
    const doctypes = [ 'Report - Inspection', 'Report - Expediting', 'Report - Flash', 'Risk Assessment',
      'Non Conformance Report', 'Punch List', 'Timesheet - Client', 'Timesheet - Intertek', 'Timesheet - Moody',
      'Timesheet - Technical Specialist' ];
    const time = (timeLineItems.length > 0 && timeLineItems.filter(timeValue => ((timeValue.chargeTotalUnit !== "0.00" && timeValue.chargeTotalUnit !== 0) || (timeValue.payUnit !== "0.00" && timeValue.payUnit !== 0)) && timeValue.recordStatus !== 'D').length !== 0);
    const travel = (travelLineItems.length > 0 && travelLineItems.filter(travelValue => ((travelValue.chargeUnit !== "0.00" && travelValue.chargeUnit !== 0) || (travelValue.payUnit !== "0.00" && travelValue.payUnit !== 0)) && travelValue.recordStatus !== 'D').length !== 0);
    const expense = (expenseLineItems.length > 0 && expenseLineItems.filter(expenseValue => ((expenseValue.chargeUnit !== "0.00" && expenseValue.chargeUnit !== 0) || (expenseValue.payUnit !== "0.00" && expenseValue.payUnit !== 0)) && expenseValue.recordStatus !== 'D').length !== 0);
    const consumable = (consumableLineItems.length > 0 && consumableLineItems.filter(consumableValue => ((consumableValue.chargeUnit !== "0.00" && consumableValue.chargeUnit !== 0) || (consumableValue.payUnit !== "0.00" && consumableValue.payUnit !== 0)) && consumableValue.recordStatus !== 'D').length !== 0);
    const docVal = (timesheetDocuments.length > 0 && timesheetDocuments.filter(docType => doctypes.includes(docType.documentType) && docType.recordStatus !== "D").length !== 0);

    if (this.props.timesheetInfo.timesheetStatus == "E") {
      if (time || travel || expense || consumable || docVal) {
        IntertekToaster("This record cannot be set to Unused as there are either resource account entries added or excluded documents uploaded – please correct these and try again", 'warningToast VisitStatusTBA');
        return false;
      }
    }
    if (closeConfirmDialog) {
      this.props.actions.HideModal();
    }
    if(this.props.timesheetInfo && this.props.timesheetInfo.timesheetAssignmentId) {
      await this.props.actions.FetchTimesheetValidationData(this.props.timesheetInfo.timesheetAssignmentId);
    }
    const valid = this.validateSave(from, reject);
    if (valid) {
      const status = this.getNextTimesheetStatus(from);

      let res = {};
      if(from === 'approve' && ('A' === status.timesheetStatus || 'N' === status.timesheetStatus )) {
        if(!this.props.isInterCompanyAssignment && this.props.currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE) {
          status.timesheetStatus = 'N';
          res = await this.props.actions.SaveTimesheetDetails(status, reject);           
        }
        this.timesheetUpdateStatus(false, from, reject);
      } else {
        let isRejectSave = reject;
        if(reject && from === 'reject') 
        {
          status.timesheetStatus = this.props.timesheetInfo.timesheetStatus;
          isRejectSave = true;
        }
        res = await this.props.actions.SaveTimesheetDetails(status, isRejectSave); 
        if(reject) {
          this.openReasonForRejectPopup();
        }       
      }
      
      this.callBackFuncs.loadCalenderData();
      if(res) {
        this.callBackFuncs.disableTimesheetStatus();
        this.callBackFuncs.reloadLineItemsOnSaveValidation();
      }

      for (const key in this.validations) {
        this.validations[key] = true;
      }
    }
  }

  timesheetUpdateStatus = async (isSave, from, reject) => {
    this.props.actions.HideModal();
    if (isSave) {
      this.timesheetSave(true, from, reject);
    } else {
      const status = this.getNextTimesheetStatus(from);
      const obj ={
        status:status,
        isFromApproveOrReject:from,
        isValidationRequired:false,
        isSuccesAlert: true,
        isAuditUpdate: this.props.isbtnDisable
    };    
    //For Bug Id 563 -Start
      if(from === 'approve' && ('A' === status.timesheetStatus || 'N' === status.timesheetStatus ) &&
        this.state.isTShasMultipleTimesheets.length === 0 ){
        obj.isValidationRequired = true;
        obj.isSuccesAlert = false;
        //For Bug Id 563 -End      
        const response = await this.props.actions.UpdateTimesheetStatus(obj);
        if(from === 'approve' && 'A' === status.timesheetStatus && response){
          //For Bug Id 563 -Start
          if (response.length > 0) {
              this.setState({
                isTShasMultipleTimesheets: response
              });
              this.props.actions.HideModal();
              return false;
          }else{
            this.processApprovalEmailNotification();
          }
        }
      } else {
        await this.props.actions.UpdateTimesheetStatus(obj);
        this.callBackFuncs.disableTimesheetStatus();
      }
      if(from === 'reject' && 'R' === status.visitStatus) {
        this.openReasonForRejectPopup();
      }
    }
  }

  getEmailTemplateValue = ()=>{
    let {
      timesheetStartDate,
    } = this.props.timesheetInfo;

    const visitOrTimesheet = `(${ this.props.timesheetInfo.timesheetAssignmentNumber.toString().padStart(5, '0') } : ${ this.props.timesheetInfo.timesheetDescription ? this.props.timesheetInfo.timesheetDescription : 'N/A' })`;    
    timesheetStartDate = timesheetStartDate?moment(timesheetStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT):'';
    return {
      AssignmentNumber:this.props.timesheetInfo.timesheetAssignmentNumber.toString().padStart(5,'0'),
      Company:this.props.timesheetInfo.timesheetContractCompany,
      ContractNumber:this.props.timesheetInfo.timesheetContractNumber,
      CoordinatorName:this.props.loggedInUser,
      CustomerProjectName:this.props.timesheetInfo.customerProjectName?this.props.timesheetInfo.customerProjectName:'',
      CustomerName:this.props.timesheetInfo.timesheetCustomerName,
      ProjectNumber:this.props.timesheetInfo.timesheetProjectNumber,
      ReportNumber:'',
      Supplier:'',
      SupplierPO:'',
      VisitDate:timesheetStartDate,
      VisitDatePeriod:this.props.timesheetInfo.timesheetDatePeriod?this.props.timesheetInfo.timesheetDatePeriod:'',
      VisitOrTimesheet:localConstant.modalConstant.TIMESHEET,
      TimesheetId:this.props.timesheetInfo.timesheetId,
      VisitURL: (isEmptyOrUndefine(configuration.extranetURL) ? "" : configuration.extranetURL + this.props.timesheetInfo.timesheetId)
    };
  }

  replaceEmailTemplatePlaceHolders = () => {
    let emailTemplate = this.props.emailTemplate;
    if (!emailTemplate) {
      return "";
    }
    const templateValues = this.getEmailTemplateValue();
    for (const key in templateValues) {
      const replacePlaceHolder = "@" + key + "@";
      const replaceValue = templateValues[key] ? templateValues[key] : '';
      emailTemplate = replaceAll(emailTemplate, replacePlaceHolder, replaceValue);
    }
    return emailTemplate;
  }

  processApprovalEmailNotification = async () => {
    const showApprovalEmailPopup = await this.props.actions.ProcessApprovalEmailNotification();    
    if (showApprovalEmailPopup) {
      const timesheetDocuments = this.props.timesheetDocuments;
        this.attachedFiles = [];
        if(timesheetDocuments) {
          const customerVisibleDocuments = timesheetDocuments.filter(record => record.recordStatus !== "D" && record.subModuleRefCode === '0' && record.isVisibleToCustomer === true);
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
          isSendApprovalEmailNotification: true,
          editorHtml:editorHtml
        };
      });
    } else {
      const status = this.getNextTimesheetStatus('approve');
      const obj ={
        status:status,
        isFromApproveOrReject:'approve',
        isValidationRequired:false,
        isSuccesAlert: true,
        isAuditUpdate: this.props.isbtnDisable
      };
      this.props.actions.UpdateTimesheetStatus(obj);
    }
  }

  timesheetCancelHandler = () => {
    const confirmationObject = {
      title: modalTitleConstant.CONFIRMATION,
      message: modalMessageConstant.CANCEL_CHANGES,
      modalClassName: "warningToast",
      type: "confirm",
      buttons: [
        {
          buttonName: "Yes",
          onClickHandler: (e) => this.cancelTimesheet(e),
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

  //Timesheet Cancel handler
  cancelTimesheet = async () => {
    this.props.actions.HideModal();
    if(this.props.currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE) {
      this.props.actions.CancelCreateTimesheetDetails();
    } else {
      await this.props.actions.CancelEditTimesheetDetails();
    }
    
    for (const key in this.validations) {
      this.validations[key] = true;
    }    
    this.callBackFuncs.onTimesheetCancel();
    this.props.actions.ClearTimesheetCalendarData();
    this.callBackFuncs.loadCalenderData();
  }
  confirmationRejectHandler = (e) => {
    this.updatedData = {
      rejectionDate: moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT)
    };  
    this.props.actions.HideModal();
  }
  //Timesheet Delete handler
  timesheetDeleteHandler = (e) => {
    const confirmationObject = {
      title: modalTitleConstant.CONFIRMATION,
      message: modalMessageConstant.TIMESHEET_DELETE_MESSAGE,
      modalClassName: "warningToast",
      type: "confirm",
      buttons: [
        {
          buttonName: "Yes",
          onClickHandler: this.deleteTimesheet,
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

  //For Timesheet Reports
  reportsClickHandler = ( ) => {	
    this.setState({ isShowVisitReport: false, isOpenReportModal: true });
  } 
  HideModal =() =>	
  {	
    this.setState ( { isOpenReportModal:false } );	
  }	

    //Report Change Handler	
  onReportChange = (e) => {
    const result = formInputChangeHandler(e);
    if (result.value === 'Timesheet Report') {
      this.setState({
        isShowVisitReport: true
      });
    } else {
      this.setState({
        isShowVisitReport: false
      });
    }
  }	

  deleteTimesheet = () => {
    this.props.actions.HideModal();
    if(this.invoiceChargeSideCheck()){
      if(this.invoicePaySideCheck()){
    this.props.actions.DeleteTimesheet()
      .then(response => {
        if (response) {
          if (response.code == 1) {
            this.props.history.push('/EditTimesheet');
            this.props.actions.DeleteAlert(response.result, "Timesheet");
          }
        }
      });
    }
    else{
      this.timesheetDeleteWarningHandler(modalMessageConstant.TIMESHEET_DELETE_HANDLER_WARNING_FOR_PAY);
    }
    } else {
      this.timesheetDeleteWarningHandler(modalMessageConstant.TIMESHEET_DELETE_HANDLER_WARNING);
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

  timesheetDeleteWarningHandler = (value) => {
    const confirmationObject = {
      title: modalTitleConstant.ALERT_WARNING,
      message: value,
      modalClassName: "warningToast",
      type: "confirm",
      buttons: [
        {
          buttonName: "Revert all Changes",
          onClickHandler: this.cancelTimesheet,
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

  timesheetClientReportingReqHandler = () => {
    this.setState({
      isClientReportReqsPopup: true
    });
  }
  //Timesheet submit Handler
  timesheetSubmitHandler = (e) => {
    e.preventDefault();
    const validDoc = this.uploadedDocumentCheck();
    const valid = this.mandatoryFieldsValidationCheck();
      if(!valid || !validDoc){
        return false;
      }
    if (!this.props.isbtnDisable) {
      const confirmationObject = {
        title: modalTitleConstant.CONFIRMATION,
        // message: <p className="confirmTextWarp">{modalMessageConstant.TIMESHEET_SAVE_CONFIRM}</p>,
        message: modalMessageConstant.TIMESHEET_SAVE_CONFIRM,
        modalClassName: "warningToast",
        type: "confirm",
        buttons: [
          {
            buttonName: "Yes",
            onClickHandler: (event) => this.timesheetUpdateStatus(true, 'submit', false),
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
      this.timesheetUpdateStatus(false, 'submit', false);
    }
  }
  isTSSelected = ()=>{
    const timesheetTechnicalSpecilists = isEmptyReturnDefault(this.props.timesheetTechnicalSpecilists);
      if(timesheetTechnicalSpecilists.length === 0){
        return false;
      }
      return true;
  }
  //Timesheet approve Handler
  timesheetApproveHandler = (e) => {
    e.preventDefault();
    const validDoc = this.uploadedDocumentCheck();
    const valid = this.mandatoryFieldsValidationCheck();
      if(!valid || !validDoc){
        return false;
      }
    this.setState({ timesheetStatus: this.props.timesheetInfo.timesheetStatus });
    if (!this.props.isbtnDisable) {
      const confirmationObject = {
        title: modalTitleConstant.CONFIRMATION,
        // message: <p className="confirmTextWarp">{modalMessageConstant.TIMESHEET_SAVE_CONFIRM}</p>,
        message: modalMessageConstant.TIMESHEET_SAVE_CONFIRM,
        modalClassName: "warningToast",
        type: "confirm",
        buttons: [
          {
            buttonName: "Yes",
            onClickHandler: (event) => this.timesheetUpdateStatus(true, 'approve', false),
            className: "modal-close m-1 btn-small"
          },
          {
            buttonName: "No",
            //onClickHandler: (event) => this.timesheetApproveUpdateStatusConfirm(event),
            onClickHandler: (event) => this.confirmationRejectHandler(event),
            className: "modal-close m-1 btn-small"
          }
        ]
      };
      this.props.actions.DisplayModal(confirmationObject);
    }
    else {
      this.timesheetApproveUpdateStatusConfirm(e);
    }
  }

  timesheetApproveUpdateStatusConfirm = async (e) => {
    const status = this.getNextTimesheetStatus('approve');
    const obj ={
      status:status,
      isFromApproveOrReject:'approve',
      isValidationRequired:true,
      isSuccesAlert: false,
      isAuditUpdate: this.props.isbtnDisable
    };
    const response = await this.props.actions.UpdateTimesheetStatus(obj);      
    if (response && response.length > 0) {
        this.setState({
          isTShasMultipleTimesheets: response
        });          
        return false;
    } else {
      const { isInterCompanyAssignment, isOperator, isOperatorCompany } = this.props;
      let message = modalMessageConstant.TIMESHEET_APPROVE_CONFIRM_CHC;
      if (isInterCompanyAssignment && (isOperator || isOperatorCompany))
        message = modalMessageConstant.TIMESHEET_APPROVE_CONFIRM_OC;
        const confirmationObject = {
          title: modalTitleConstant.CONFIRMATION,
          message: message,
          modalClassName: "warningToast",
          type: "confirm",
          buttons: [
            {
              buttonName: "Yes",
              onClickHandler: (event) => this.timesheetUpdateStatus(false, 'approve', false),
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
  }

  //Timesheet reject Handler
  timesheetRejectHandler = (e) => {
    e.preventDefault();
    const validDoc = this.uploadedDocumentCheck();
    const valid = this.mandatoryFieldsValidationCheck();
    if(!valid || !validDoc){
      return false;
    }
    if (!this.props.isbtnDisable && this.props.currentPage === localConstant.timesheet.EDIT_VIEW_TIMESHEET_MODE) {
      const confirmationObject = {
        title: modalTitleConstant.CONFIRMATION,
        // message: <p className="confirmTextWarp">{modalMessageConstant.TIMESHEET_SAVE_CONFIRM}</p>,
        message: modalMessageConstant.TIMESHEET_SAVE_CONFIRM,
        modalClassName: "warningToast",
        type: "confirm",
        buttons: [
          {
            buttonName: "Yes",
            onClickHandler: (event) => this.timesheetUpdateStatus(true, 'reject', true),
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
      if(this.props.currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE) {        
        this.setState({
          isRejectSave: true
        });
        this.timesheetSave(false, 'fromSave', true);        
      } else {
        this.openReasonForRejectPopup();
      }      
    }
  }

  openReasonForRejectPopup = () => {
    this.setState({
      rejectionDate: moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT),
      isRejectForRejectPopup: true,
      isRejectSave: false
    }); 
    this.updatedData = {
      rejectionDate: moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT)
    };
  }
  //Timesheet create New Timesheet Handler
  timesheetCreateNewTimesheetHandler = () => {
    if('C' === this.props.timesheetInfo.assignmentStatus){
      IntertekToaster(localConstant.validationMessage.TIMESHEET_ASSIGNMENT_COMPLETE, 'warningToast timesheetEndDateVal');
      return false;
     }
     //On Button Click New Tabs
       const queryObj={
        tId:this.props.timesheetInfo.timesheetId,            
        AId:this.props.timesheetInfo.timesheetAssignmentId,            
        isCTSheetFromETSheet:true
    };
     const queryStr = ObjectIntoQuerySting(queryObj); 
     window.open('/TimesheetDetails?'+ queryStr, '_blank');  

    //this.callBackFuncs.onCancel();
    //this.props.actions.CreateNewTimesheet(); //On Button Click New Tabs
    // this.props.history.push({
    //   pathname: '/TimesheetDetails',
    //   search:`?isCTSheetFromETSheet=${ true }`      
    // });
   
  }

  saveConfirmHandler = (validationType, msg, from, reject) => {
    const confirmationObject = {
      title: modalTitleConstant.CONFIRMATION,
      // message: <p className="confirmTextWarp">{msg}</p> ,
      message: msg ,
      modalClassName: "warningToast",
      type: "confirm",
      isMaxWidth: ( validationType == 'NoLineItems' ? true : false),
      buttons: [
        {
          buttonName: "Yes",
          onClickHandler: (e) => {
            if (validationType)
              this.validations[validationType] = false;
            this.timesheetSave(true, from, reject);            
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
    if(type === "closeApprovalEmailNotificationPopup"){
      this.TSHasMultipleBookingCancel();
      this.toAddress = '';
      this.setState({
        isSendApprovalEmailNotification: false,
        editorHtml:''
      });
    }
    if(type === "TShasMultipleTimesheets"){
      this.setState({
        isTShasMultipleTimesheets:[]
      });
    }
  }

  timesheetAssignmentDateValidation = () => {
    const { timesheetInfo, timesheetValidationData } = this.props;
    let isExists = false;
    if(timesheetValidationData && timesheetValidationData.timesheetAssignmentDates && timesheetValidationData.timesheetAssignmentDates.length > 0) {        
      const fromdate = moment(timesheetInfo.timesheetStartDate).format(localConstant.commonConstants.DATE_FORMAT);
      if(this.props.currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE) {
        timesheetValidationData.timesheetAssignmentDates.forEach(rowData => {              
          if((rowData.timesheetStatus === 'N' || rowData.timesheetStatus === 'C') 
              && moment(rowData.timesheetStartDate).format(localConstant.commonConstants.DATE_FORMAT) === fromdate) {
              isExists = true;
          }
        });
      } else {
        timesheetValidationData.timesheetAssignmentDates.forEach(rowData => {              
            if((rowData.timesheetStatus === 'N' || rowData.timesheetStatus === 'C') && rowData.timesheetId !== timesheetInfo.timesheetId 
              && moment(rowData.timesheetStartDate).format(localConstant.commonConstants.DATE_FORMAT) === fromdate) {
                isExists = true;
            }
        });
      }
      return isExists;
    }
  }
  
  timesheetInvalidCalendarRemoveConfirmation = (validationType) => {      
    const confirmationObject = {
      title: modalTitleConstant.CONFIRMATION,
      message: modalMessageConstant.TIMESHEET_INVALID_CALENDAR_DATA_VALIDATION,
      modalClassName: "warningToast",
      type: "confirm",
      buttons: [
        {
        buttonName: "Yes",
        onClickHandler: (e) => {
          if (validationType)
            this.validations[validationType] = false;
          this.timesheetSave(true, 'fromSave', false);
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

  timesheetAssignmentDateValidationConfirmation = (validationType) => { 
    const confirmationObject = {
    title: modalTitleConstant.CONFIRMATION,
    message: modalMessageConstant.TIMESHEET_ASSIGNMENT_DATE_VALIDATION,
    modalClassName: "warningToast",
    type: "confirm",
    buttons: [
      {
      buttonName: "Yes",
      onClickHandler: (e) => {
        if (validationType)
          this.validations[validationType] = false;
        this.timesheetSave(true, 'fromSave', false);
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

  //All Input Handle get Name and Value
  inputHandleChange = (e) => {
    this.updatedData[e.target.name] = e.target.value;
  }

  submitRejectReason = async (e) => {
    e.preventDefault();
    if(isEmptyOrUndefine(this.updatedData["reasonForRejection"])) {
      IntertekToaster(localConstant.validationMessage.VISIT_REASON_FOR_REJECTION, 'warningToast ReasonForRejection');
    } else {
      const timesheetNotes = {
        timesheetId: this.props.timesheetInfo.timesheetId,
        note: modalTitleConstant.REASON_FOR_REJECTION + ": " + this.updatedData["reasonForRejection"],
        isSpecialistVisible: true,
        isCustomerVisible: false,
        recordStatus: 'N',
        createdBy: this.props.loggedInUserName,
        createdOn: this.updatedData["rejectionDate"],
        actionByUser: this.props.loggedInUserName
      };
      if(this.props.currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE) {        
        this.props.actions.AddTimesheetNotes(timesheetNotes);        
      }
      const nextStatus = this.getNextTimesheetStatus('reject');      
      const obj ={
        status:nextStatus,
        isFromApproveOrReject:'reject',
        timesheetNotes: timesheetNotes,
        isSuccesAlert: true,
        isAuditUpdate: this.props.isbtnDisable,
        ...this.updatedData
      };
      this.props.actions.UpdateTimesheetStatus(obj);
      this.closeModalPopup(e,'closeReasonForRejectPopup');
    }
  }

  getNextTimesheetStatus = (isFrom) => {
    const {
      isInterCompanyAssignment,
      isOperator,
      isCoordinator,
      timesheetStatus,      
      isOperatorCompany,
      isCoordinatorCompany
    } = this.props;
    let nextStatus = timesheetStatus.value;    
    // { name: 'Approved By Contract Holder', value: 'A' },g
    // { name: 'Awaiting Approval', value: 'C' },
    // { name: 'Rejected By Operator', value: 'J' },g
    // { name: 'Approved By Operator', value: 'O' },g
    // { name: 'Not Submitted', value: 'N' },
    // { name: 'Rejected By Contract Holder', value: 'R' }g
    
    //Domestic
    if (!isInterCompanyAssignment) {
      if ('C' === timesheetStatus.value && 'approve' === isFrom) {
        nextStatus = 'A';
      }
      else if ('N' === timesheetStatus.value && 'approve' === isFrom) {
        nextStatus = 'A';
      }
      else if ('C' === timesheetStatus.value && 'reject' === isFrom) {
        nextStatus = 'J';
      }
      else if ('N' === timesheetStatus.value && 'reject' === isFrom) {
        nextStatus = 'J';
      }
      else if('submit' === isFrom){
        nextStatus = 'C';
      }
    }//Intercompany
    if (isInterCompanyAssignment) {
      if (isOperatorCompany && 'C' === timesheetStatus.value && 'approve' === isFrom) {
        nextStatus = 'O';
      }
      else if (isOperatorCompany && 'N' === timesheetStatus.value && 'approve' === isFrom) {
        nextStatus = 'O';
      }
      else if (isOperatorCompany && 'C' === timesheetStatus.value && 'reject' === isFrom) {
        nextStatus = 'J';
      }
      else if (isOperatorCompany && 'N' === timesheetStatus.value && 'reject' === isFrom) {
        nextStatus = 'J';
      }
      else if (isOperatorCompany && 'J' === timesheetStatus.value && 'submit' === isFrom) {
        nextStatus = 'C';
      }
      else if (isOperatorCompany && 'N' === timesheetStatus.value && 'submit' === isFrom) {
        nextStatus = 'C';
      }
      else if (isCoordinatorCompany && 'O' === timesheetStatus.value && 'approve' === isFrom) {
        nextStatus = 'A';
      }
      else if (isCoordinatorCompany && 'O' === timesheetStatus.value && 'reject' === isFrom) {
        nextStatus = 'R';
      }
      else if (isOperatorCompany && 'R' === timesheetStatus.value && 'approve' === isFrom) {
        nextStatus = 'O';
      }
      else if (isOperatorCompany && 'R' === timesheetStatus.value && 'reject' === isFrom) {
        nextStatus = 'J';
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
    return { timesheetStatus: nextStatus };
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
      attachments:this.FormatAttachedFiles(attachedDocuments),
      emailContent:data.emailObj.editorHtml,
      emailSubject:data.emailObj.emailSubject,
      toAddress:this.toAddress 
    };
    const status = this.getNextTimesheetStatus('approve');
      const obj ={
        status:status,
        isFromApproveOrReject:'approve',
        isValidationRequired:false,
        isSuccesAlert: true,
        isAuditUpdate: this.props.isbtnDisable,
        emailContent: data.emailObj.editorHtml,
        attachments:this.FormatAttachedFiles(attachedDocuments),
        toAddress: this.toAddress ? this.toAddress : '',
        emailSubject: data.emailObj.emailSubject
    };
    this.props.actions.UpdateTimesheetStatus(obj);
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

  visitRejectDateChange = (date) => {    
    this.updatedData["rejectionDate"] = moment(date).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
    this.setState({
        rejectionDate: moment(date).format(localConstant.commonConstants.SAVE_DATE_FORMAT)
    });
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
              onClickHandler: this.continueToTimesheetReport,
              className: "modal-close m-1 btn-small"
            },
            {
              buttonName: "No",
              onClickHandler: this.rejectTimesheetReport,
              className: "modal-close m-1 btn-small"
            }
          ]
        };
        this.props.actions.DisplayModal(confirmationObject); //Changes for D1287
      }
      else{
        this.getTimesheetReportHandler();
      }
    }

    getTimesheetReportHandler = () => {
      const _localStorage = localStorage.getItem('Username');
      let reportFileName = '';
      let params = {};
      params['username'] = _localStorage;
      if (this.state.isShowVisitReport) {
        params = {
          'TimesheetId': this.props.timesheetInfo.timesheetId,
          'format': 'EXCEL'
        };
        params['reportname'] = EvolutionReportsAPIConfig.Timesheet;
        reportFileName = 'Timesheet';
        DownloadReportFile(params, 'application/vnd.ms-excel', reportFileName, '.xls', this.props);
      }
    }

    continueToTimesheetReport = () => {
      this.props.actions.HideModal();
      this.getTimesheetReportHandler();
    }

    rejectTimesheetReport = () => {
      this.props.actions.HideModal();
    }

  render() {
    const { currentPage, timesheetDocuments  } = this.props;
    this.timesheetTabData = this.filterTabDetail();
    const modelData = { isOpen: this.state.isOpen };
    const { assignmentClientReportingRequirements,      
      timesheetAssignmentNumber,
      timesheetNumber,timesheetStartDate } = this.props.timesheetInfo;
         let interactionMode= this.props.interactionMode;
         if(this.props.pageMode===localConstant.commonConstants.VIEW){
          interactionMode=true;
         }
    return (
      <Fragment>
        { this.state.isOpenReportModal ?   <Modal	
        title={localConstant.commonConstants.TIMESHEETREPORTS	}
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
                optionsList={ localConstant.commonConstants.timesheetReportList }	
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
          this.state.isTShasMultipleTimesheets.length > 0 ?
            <Modal scrollable={true} title={modalTitleConstant.ALLOCATED_VISIT_TIMESHEET}
              modalClass="visitModal"
              modalId="TShasMultipleTimesheets"
              formId="TShasMultipleTimesheets"
              buttons={this.modelBtns.TShasMultipleTimesheets}
              isShowModal={(this.state.isTShasMultipleTimesheets.length > 0)}>
                 <TSWithMultipleTimesheetPopup 
                 TShasMultipleTimesheets = {this.state.isTShasMultipleTimesheets}
                 date={timesheetStartDate}
                 />
            </Modal>
            : null
        }
        
        {this.state.isSendApprovalEmailNotification ? 
                <ApprovedEmail 
                 emailSubject="Evolution2 - Timesheet Completed"
                 isemailTemplate = { this.state.isSendApprovalEmailNotification }
                 approvelEmailButtons={this.modelBtns.sendApprovalEmailNotificationBtns}
                 documentGridRowData={timesheetDocuments.filter(record => record.recordStatus !== "D")}
                 documentGridHeaderData={this.documentHeaderData}
                 emailContent={this.state.editorHtml}
                 moduleCode="TIME"
                 moduleCodeReference={this.props.timesheetInfo.timesheetId}
                 toAddress = {this.toAddress}
                 attachedFiles = {this.attachedFiles}
                /> : null}

        {/* Client Reporting Requirements Popup */}
        {this.state.isClientReportReqsPopup ?
          <Modal title={localConstant.assignments.CLIENT_REPORTING_REQUIREMENTS}
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
            buttons={this.modelBtns.rejectTimesheet}
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
        {this.state.timesheetDateRangeValidation.length > 0 ?
          <Modal title={localConstant.login.EVOLUTION}
            titleClassName="chargeTypeOption"
            modalContentClass="extranetModalContent"
            modalId="errorListPopup"
            formId="errorListForm"
            buttons={this.modelBtns.errorListButton}
            isShowModal={true}>
            <ErrorList errors={this.state.timesheetDateRangeValidation} />
          </Modal> : null
        }
        <SaveBarWithCustomButtons
         codeLabel={localConstant.sideMenu.TIMESHEET}
         codeValue={(this.props.timesheetInfo && timesheetNumber) ? 
          `(${ timesheetAssignmentNumber.toString().padStart(5,'0') } : 
          ${ this.props.timesheetInfo.timesheetDescription ? this.props.timesheetInfo.timesheetDescription: 'N/A' })`:''}
         currentMenu={localConstant.sideMenu.TIMESHEET}
         currentSubMenu={this.props.currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE ?
            localConstant.timesheet.CREATE_TIMESHEET :
            localConstant.timesheet.EDIT_VIEW_TIMESHEET}
          isbtnDisable={this.props.isbtnDisable}
          buttons={this.handleTimesheetButtons()}
          interactionMode={interactionMode}
          isbuttonDiv={true}
          codeLabelDivClass = 'col s12 m3 pt-1 nowarp'
          activities={this.props.activities}
        />
        <div className="row ml-2 mb-0">
          <div className={(this.state.customSavebarPosition === true ? ((this.props.pageMode===localConstant.commonConstants.VIEW) ? "col s12 pl-0 pr-2 verticalTabs customSavebarPosition" :"col s12 pl-0 pr-2 verticalTabs customSavebarPosition") : "col s12 pl-0 pr-2 verticalTabs")}>
            <CustomTabs tabsList={this.timesheetTabData}
              moduleName="timesheets"
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

export default TimesheetDetails;