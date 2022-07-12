/* eslint-disable no-unused-expressions */
import React, { Component, Fragment } from 'react';
import SaveBar from '../../applicationComponent/saveBar';
import { getlocalizeData, customRegExValidator, formInputChangeHandler, isEmptyReturnDefault, parseQueryParam, scrollToTop } from '../../../../utils/commonUtils';
import CustomTabs from '../../../../common/baseComponents/customTab';
import { modalTitleConstant, modalMessageConstant } from '../../../../constants/modalConstants';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { isEmpty, isEmptyOrUndefine,ResetCurrentModuleTabInfo,isUndefined } from '../../../../utils/commonUtils';
import { ButtonShowHide } from '../../../../utils/permissionUtil';
import arrayUtil from '../../../../utils/arrayUtil';
import moment from 'moment';
import Modal from '../../../../common/baseComponents/modal';
import ErrorList from '../../../../common/baseComponents/errorList';
import { applicationConstants } from '../../../../constants/appConstants';
import { required, requiredNumeric } from '../../../../utils/validator';
import { activitycode ,securitymodule } from '../../../../constants/securityConstant';
import dateUtil from '../../../../utils/dateUtil';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import { userTypeCheck, userTypeGet } from '../../../../selectors/techSpechSelector'; 
import { StringFormat } from '../../../../utils/stringUtil';
import ReactGrid from '../../../../common/baseComponents/reactAgGridTwo';

const localConstant = getlocalizeData();
const SelectionPopUpModal = (props) => {
  return (
      <Fragment>
          <ReactGrid gridRowData={props.optionList} 
          gridColData={props.headerData} onRef={props.gridRef}
           />
      </Fragment>
  );
};
const HeaderData = (searchData) => {
  return {
    'cvOptionsHeader': {
      "columnDefs": [
        {
          "checkboxSelection": true,
          "headerCheckboxSelectionFilteredOnly": true,
          "suppressFilter": true,
          "width": 40,
          "cellRenderer": (params) => {
            if (params.node.rowIndex !== 8 && params.node.rowIndex !== 9) {
              params.node.setSelected(true);
            }
            else {
              params.node.setSelected(false);
            }
          }
        },
        {
          "headerName": "Section",
          "headerTooltip": "Section",
          "field": "menu",
          "tooltipField": "menu",
          "width": 178
      },
      {
          "headerName": "Sub-Section",
          "headerTooltip": "Sub-Section",
          "field": "subMenu",
          "tooltipField": "subMenu",
          "width": 151
      },
      {
          "headerName": "Sub-Filter",
          "headerTooltip": "Sub-Filter",
          "field": "optional",
          "tooltipField": "optional",
          "width": 366
      },
      ],
      "enableFilter": false,
      "enableSorting": false,
      "rowSelection": "multiple",
      "gridHeight": 50,
      "pagination": false,
      "searchable": false,
      "clearFilter": false,
    }
  };
};
class TechSpecDetails extends Component {
  constructor(props) {
    super(props);
    this.overrideTaskStatus = isEmptyReturnDefault(localConstant.resourceSearch.overrideTaskStatus);
    this.ploTaskStatus = isEmptyReturnDefault(localConstant.resourceSearch.ploTaskStatus);
    this.state = {
      isOpen: false,
      creatModeDisbale: false,
      errorList: [],
      showListOfTM: false,
      isSelectionPopUpOpen: false,
    };    
    this.errors = [];
    this.updatedData = {};
    this.locationData={};
    this.userTypes = userTypeGet();
    this.profileActionList = [];
    this.profileActionListWitOutRCRM=[];
    this.headerData = HeaderData();
    /*
   * View  Resource
   */
    this.tsViewLevel0 = (this.props.activities.filter(x => x.activity === activitycode.LEVEL_0_VIEW).length > 0);
    /*
    * Edit Resource
    */
   // this.tsEditLevel0 = (this.props.activities.filter(x => x.activity === activitycode.LEVEL_0_MODIFY).length > 0 ? true : true);
    /*
    * View SensitiveDoc of Resource
    */
    this.tsSensitiveDocView = (this.props.activities.filter(x => x.activity === activitycode.VIEW_SENSITIVE_DOC).length > 0);
    /*
    * Edit SensitiveDoc of Resource
    */
    this.tsSensitiveDocEdit = (this.props.activities.filter(x => x.activity === activitycode.EDIT_SENSITIVE_DOC).length > 0);
    /*
    * View  PayRate of Resource
    */
    this.tsPayrateEdit = (this.props.activities.filter(x => x.activity === activitycode.EDIT_PAYRATE).length > 0);
    /*
    * Edit Payrate of Resource
    */
    this.tsPayrateView = (this.props.activities.filter(x => x.activity === activitycode.VIEW_PAYRATE).length > 0);
    /*
  * View  TM 
  */
    this.tmView = (this.props.activities.filter(x => x.activity === activitycode.VIEW_TM).length > 0) ;
    /*
    * Edit TM
    */
    this.tmEdit = (this.props.activities.filter(x => x.activity === activitycode.EDIT_TM).length > 0);

    /*
    *ResourceInformation Tab(Added for ITK Def957(issue1.1) ref by 14-05-2020 ALM Doc)
    */ 
    this.resourceInformation =(this.props.activities.filter(x => x.activity === activitycode.INTER_COMP_LEVEL_0_VIEW).length > 0);

    this.errorListButton = [
      {
        name: localConstant.commonConstants.OK,
        action: this.closeErrorList,
        btnID: "closeErrorList",
        btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
        showbtn: true
      }
    ];
    this.SelectionPopUpButtons = [
      {
          name: localConstant.commonConstants.CANCEL,
          action: this.cancelSelectionPopUp,
          btnID: "cancelSelectionPopUp",
          btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
          showbtn: true,
          type:"button"
      },
      {
          name: localConstant.commonConstants.SUBMIT,
          action: this.techSpecExportClickHandler,
          btnID: "exportToStandardCV",
          btnClass: "waves-effect waves-teal btn-small ",
          showbtn: true
      }
  ];

    this.techSpecExportClickHandler = this.techSpecExportClickHandler.bind(this);
    this.filterTabDetails=this.filterTabDetails.bind(this);
    this.callBackFuncs ={
      onCancel:()=>{},
      reloadLineItemsOnSave:()=>{},
      reloadLineItemsOnSaveValidation:()=>{},
      reloadTaxonomyLineItemsOnSave:()=>{}
    };

    ResetCurrentModuleTabInfo(this.props.techSpecDetailTabs); //HotFixes changes for TS city county binding null
  }
  technicalManager = () => {
    return (this.tmView || this.tmEdit); //SystemRole based UserType relevant Quick Fixes 
  }
  sensitiveData = () => {
    // System role quick fixes 20-12-2019
     return (this.tsSensitiveDocEdit || this.tsSensitiveDocView) ; /** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/
  }
  payRateData = () => { //Changes for D741
    return ((this.tsPayrateEdit || this.tsPayrateView) && !(this.locationData && this.locationData.isPayRateVisible === "true"));  /** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/
  }
  resourceInformationData =() =>{ //Added for ITK Def957(issue1.1) ref by 14-05-2020 ALM Doc
    return this.resourceInformation;
  }
  /**
   * To check mode of Resource.
   */
  techSpecViewMode = (data) => { 
    const activities = this.props.activities;
    //D667
    if (this.props.location.pathname === AppMainRoutes.profileDetails && activities && activities.length > 0) {
      const viewActivities = [
        activitycode.VIEW,activitycode.LEVEL_0_VIEW, activitycode.LEVEL_1_VIEW, activitycode.LEVEL_2_VIEW, activitycode.VIEW_SENSITIVE_DOC, activitycode.VIEW_PAYRATE, activitycode.VIEW_TM
      ];
       //activitycode.LEVEL_0_MODIFY added for D667
      const editActivities = [
        activitycode.MODIFY,activitycode.LEVEL_0_MODIFY,activitycode.LEVEL_1_MODIFY, activitycode.LEVEL_2_MODIFY, activitycode.EDIT_SENSITIVE_DOC, activitycode.EDIT_PAYRATE, activitycode.EDIT_TM
      ];
      const isViewOnly = (activities.filter(x => viewActivities.includes(x.activity)).length > 0 || !activities.filter(x => x.activity === activitycode.LEVEL_0_VIEW).length > 0);
      const isEnableEdit = (activities.filter(x => editActivities.includes(x.activity)).length > 0);
      if ((isViewOnly && !isEnableEdit) || (data && data.viewMode === "true")) {
        this.props.actions.SetCurrentPageMode("View");
      } else {
        this.props.actions.SetCurrentPageMode(null);
      }
    }
  }
  techSpecCancelClickHandler = (e) => {
    e.preventDefault();
    const confirmationObject = {
      title: modalTitleConstant.CONFIRMATION,
      message: modalMessageConstant.CANCEL_CHANGES,
      modalClassName: "warningToast",
      type: "confirm",
      buttons: [
        {
          buttonName: localConstant.commonConstants.YES,
          onClickHandler: this.cancelTechSpecChanges,
          className: "modal-close m-1 btn-small"
        },
        {
          buttonName: localConstant.commonConstants.NO,
          onClickHandler: this.confirmationRejectHandler,
          className: "modal-close m-1 btn-small"
        }
      ]
    };
    this.props.actions.DisplayModal(confirmationObject);
  }
  cancelTechSpecChanges = () => {   
    if(this.props.currentPage === localConstant.techSpec.common.Edit_View_Technical_Specialist){
        if((!required(this.props.myTaskRefCode))){
          this.props.actions.CancelTechSpecDraftChanges().then(res =>{
            if(res){
              this.props.techSpecDetailTabs && this.props.techSpecDetailTabs.forEach(iteratedValue => {
                if(iteratedValue.tabBody === 'PayRate' && iteratedValue.isCurrentTab) {
                  this.callBackFuncs.onCancel();
                }
              });
            }
          }); 
        } else {
          this.props.actions.CancelEditTechSpecDetails().then(res =>{ 
            if(res){
              this.props.techSpecDetailTabs && this.props.techSpecDetailTabs.forEach(iteratedValue => {
                if(iteratedValue.tabBody === 'PayRate' && iteratedValue.isCurrentTab) {    
                  this.callBackFuncs.onCancel();   
                }
              });
            }
          });
        }
    } else {
      this.props.actions.CancelCreateTechSpecDetails();  
      this.props.techSpecDetailTabs && this.props.techSpecDetailTabs.forEach(iteratedValue => {
        if(iteratedValue.tabBody === 'PayRate' && iteratedValue.isCurrentTab) {    
          this.callBackFuncs.onCancel(); 
        }
      });
    }
    // this.props.currentPage === localConstant.techSpec.common.Edit_View_Technical_Specialist
    //   ? (!required(this.props.myTaskRefCode)) ? this.props.actions.CancelTechSpecDraftChanges() : this.props.actions.CancelEditTechSpecDetails()
    //   : this.props.actions.CancelCreateTechSpecDetails();  
    this.props.actions.HideModal();
  }
  confirmationRejectHandler = () => {
    this.props.actions.HideModal();
  }

  techSpecSaveClickHandler = (e) => { 
    e.preventDefault();
    const validDoc = this.uploadedDocumentCheck();
    const valid = this.mandatoryFieldsValidationCheck();
    if (valid === true && validDoc) {
      const sendToTypes = localConstant.techSpec.common;
      const userTypeData = localConstant.techSpec.userTypes;
      const { TechnicalSpecialistInfo = {} } = this.props.selectedProfileDetails;
      // Started for D946 CR (AssignedBy User Handle In ServiceSide)
      if (TechnicalSpecialistInfo.profileAction === sendToTypes.SEND_TO_TS) {
        this.props.actions.UpdateTechSpecInfo({ assignedToUser: this.props.userName,
                                                assignedByUser: this.props.userName,//d978 reassign emails
                                                pendingWithUser: TechnicalSpecialistInfo.logonName });
      }
      else if(this.props.currentPage === localConstant.techSpec.common.EDIT_VIEW_TECHSPEC && TechnicalSpecialistInfo.profileAction === sendToTypes.CREATE_UPDATE_PROFILE){
        const newValue={ 
           assignedToUser: this.props.userName,
           pendingWithUser: null //D1301
        };
        if(TechnicalSpecialistInfo.approvalStatus === localConstant.techSpec.tsChangeApprovalStatus.Rejected){
          newValue.approvalStatus = localConstant.techSpec.tsChangeApprovalStatus.UpdateAfterReject;
        }
        this.props.actions.UpdateTechSpecInfo(newValue);
       } 
       else if (TechnicalSpecialistInfo.profileAction === sendToTypes.SEND_TO_TM) {
          const newValue={
            pendingWithUser: TechnicalSpecialistInfo.assignedToUser,
          };
          this.props.actions.UpdateTechSpecInfo(newValue);
      }
      else if (TechnicalSpecialistInfo.profileAction === sendToTypes.SEND_TO_RC_RM) {
        const newValue={ 
          assignedToUser: isEmpty(TechnicalSpecialistInfo.assignedByUser) ? TechnicalSpecialistInfo.assignedToRCUser : TechnicalSpecialistInfo.assignedByUser,
          pendingWithUser: isEmpty(TechnicalSpecialistInfo.assignedByUser) ? TechnicalSpecialistInfo.assignedToRCUser : TechnicalSpecialistInfo.assignedByUser,
          approvalStatus: (this.userTypes && this.userTypes.includes(userTypeData.TS) && TechnicalSpecialistInfo.profileStatus === 'Active' ? localConstant.techSpec.tsChangeApprovalStatus.InProgress : null )
        };
        this.props.actions.UpdateTechSpecInfo(newValue);
      } 
      // def 1306 
      if (!isEmptyOrUndefine(TechnicalSpecialistInfo.businessInformationComment) && !isEmptyOrUndefine(this.props.prevBusinessInformationComment) && this.props.prevBusinessInformationComment.trim()  !== TechnicalSpecialistInfo.businessInformationComment.trim()  && TechnicalSpecialistInfo.approvalStatus === localConstant.techSpec.tsChangeApprovalStatus.Rejected) {
        this.props.actions.AddComments({
          "id": - Math.floor(Math.random() * (Math.pow(10, 5))),
          "epin": this.props.epin !== undefined ? this.props.epin : 0,
          "recordStatus": "N",
          "recordType": "TSRejectComment",
          "createdBy": this.props.loginUser,
          "createdDate": moment().format(localConstant.techSpec.common.DATE_TIME_SEC_FORMAT),
          "note": TechnicalSpecialistInfo.businessInformationComment,
        });
      }
      // End for D946 CR  
      this.props.actions.ClearMyTasksData(); //Added for MyTask Grid Values Referesh Defect ID-405
      this.props.actions.SaveTechSpecDetails().then(res =>{
        if(res){
          this.props.techSpecDetailTabs && this.props.techSpecDetailTabs.forEach(iteratedValue => {
            if(iteratedValue.tabBody === 'PayRate' && iteratedValue.isCurrentTab) {
              this.callBackFuncs.reloadLineItemsOnSave();
            } else if(iteratedValue.tabBody === 'TaxonomyApproval' && iteratedValue.isCurrentTab){
              this.callBackFuncs.reloadTaxonomyLineItemsOnSave();
            }
          });
        }
      });
    }
  }; 

  techSpecSaveAsDraftClickHandler = () => {
    const valid = this.mandatoryFieldsDraftValidationCheck();
    const { TechnicalSpecialistInfo = {} } = this.props.selectedProfileDetails;
    const sendToTypes = localConstant.techSpec.common;
    if (valid === true) {
      if(TechnicalSpecialistInfo.approvalStatus === localConstant.techSpec.tsChangeApprovalStatus.Rejected){
        const newValue ={};
        newValue.approvalStatus = localConstant.techSpec.tsChangeApprovalStatus.UpdateAfterReject;
        this.props.actions.UpdateTechSpecInfo(newValue);
        // def 1306 
      if (!isEmptyOrUndefine(TechnicalSpecialistInfo.businessInformationComment) && this.props.prevBusinessInformationComment.trim() !== TechnicalSpecialistInfo.businessInformationComment.trim()  && ( TechnicalSpecialistInfo.approvalStatus === localConstant.techSpec.tsChangeApprovalStatus.Rejected || TechnicalSpecialistInfo.approvalStatus === localConstant.techSpec.tsChangeApprovalStatus.UpdateAfterReject)) {
        this.props.actions.AddComments({
          "id": - Math.floor(Math.random() * (Math.pow(10, 5))),
          "epin": this.props.epin !== undefined ? this.props.epin : 0,
          "recordStatus": "N",
          "recordType": "TSRejectComment",
          "createdBy": this.props.loginUser,
          "createdDate": moment().format(localConstant.techSpec.common.DATE_TIME_SEC_FORMAT),
          "note": TechnicalSpecialistInfo.businessInformationComment,
        });
      }
      }
      if(this.props.currentPage === localConstant.techSpec.common.EDIT_VIEW_TECHSPEC){
        const newValue={ 
           pendingWithUser: this.props.userName
        };
        this.props.actions.UpdateTechSpecInfo(newValue);
       }
      this.props.actions.SaveTechSpecDetails(true);
      this.props.techSpecDetailTabs && this.props.techSpecDetailTabs.forEach(iteratedValue => {
        if(iteratedValue.tabBody === 'PayRate' && iteratedValue.isCurrentTab) {
          this.callBackFuncs.reloadLineItemsOnSave();
        }
      });
    }

  };
  techSpecDraftDeleteClickHandler=()=>{
    const confirmationObject = {
      title: modalTitleConstant.CONFIRMATION,
      message: modalMessageConstant.TECHSPEC_DRAFT_DELETE_MESSAGE,
      modalClassName: "warningToast",
      type: "confirm",
      buttons: [
        {
          buttonName: "Yes",
          onClickHandler: this.deleteDraft,
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
  };

  deleteDraft= () => {
    this.props.actions.HideModal();
    this.props.actions.DeleteTechSpecDraft()
    .then(response => {
      if (response) {
        if (response.code == 1) {
          this.props.history.push('/Dashboard/MyTasks');
          this.props.actions.DeleteAlert("", "Resource Draft");
        }
      }
    });
  }
  
  techSpecExportClickHandler = () => {
    const selectedSections = this.selectGridChild.getSelectedRows();
    if (this.props.selectedProfileDetails) {
      if (this.props.selectedProfileDetails.TechnicalSpecialistInfo)
        this.props.actions.ExportToCV(this.props.selectedProfileDetails.TechnicalSpecialistInfo.epin, false, selectedSections);
    }
    this.setState({
      isSelectionPopUpOpen: false
  });
  }
  techSpecChevronExportClickHandler = (e) => {
    e.preventDefault();
    if (this.props.selectedProfileDetails) {
      if (this.props.selectedProfileDetails.TechnicalSpecialistInfo)
        this.props.actions.ExportToCV(this.props.selectedProfileDetails.TechnicalSpecialistInfo.epin, true);
    }
  }

  componentDidMount() {
    const editDraftTypes=[ 
      'TS_EditProfile','RCRM_EditProfile','TM_EditProfile'
   ];  
    if (!isEmpty(this.props.location.search)) {
      this.props.actions.UpdateCurrentPage("Edit/View Resource");
      //this.props.actions.SetCurrentPageMode("View");
      const obj = parseQueryParam(this.props.location.search);
      obj.epin= obj.epin ==="undefined"? undefined : obj.epin;
      this.locationData = obj;   
      if (obj.taskType === 'CreateProfile') {
             this.props.actions.FetchSavedDraftProfile(obj.taskRefCode,localConstant.techSpec.common.CREATE_PROFILE,obj.taskType);
             }
      else if (editDraftTypes.includes(obj.taskType)) {
             this.props.actions.FetchSavedDraftProfile(obj.taskRefCode,localConstant.techSpec.common.EDIT_VIEW_TECHSPEC,obj.taskType);      
      }
      else if (this.overrideTaskStatus.includes(obj.taskType) || this.ploTaskStatus.includes(obj.taskType) ) {         
            this.props.actions.GetMyTaskARSSearch(obj);
      }
      else { // This is uesed in Resource fetch when epin link from  preassignment, ars ,quick search are clicked in inter company scenario.
        if(obj && obj.interCompanyCode)
        {     
              this.props.actions.FetchUserPermission(this.props.loginUserName, obj.interCompanyCode, securitymodule.TECHSPECIALIST)
              .then(res => { 
                this.props.actions.FetchUserTypeForInterCompanyResource(this.props.loginUserName, obj.interCompanyCode).then(res => {
                 // if(res) {  //def957 fix 
                    this.getProfileDetials(obj); 
                  //}
              });
              });    
        }
        else{
          this.getProfileDetials(obj);
        } 
    }  
    }
    this.techSpecViewMode(this.locationData);
    /**
     * this.props.isGrmMasterDataFeteched is bool flag which will be set as true once the
     * GRM master data is loaded. For subsequent GRM related menus we check for this flag to load master data
     * If user goes out of GRM/Resource module NEED set the flag to false to reload masterdata 
     */
    if(this.props.isGrmMasterDataFeteched === false){
      this.props.actions.grmDetailsMasterData();
      if(this.props.sendToInfo.length === 0){ //Changes For Sanity Def 116
        this.props.actions.FetchProfileAction().then(res => {
          if(res){
             this.filterSendToType();
          }
        });
      } else {
        this.filterSendToType();
      }
    }else {
      this.filterSendToType();
    }
    
    //Added for Extranet-TS SSO
    if(this.props.location.state && this.props.location.state.isFrom === "sso"){
      this.props.actions.EditMyProfileDetails();
    } 
    //SystemRole based UserType relevant Quick Fixes 
    const isRCRM = this.userTypes.includes(localConstant.techSpec.userTypes.RM) || this.userTypes.includes(localConstant.techSpec.userTypes.RC);
    if (!isRCRM){
       this.updatedData['profileAction'] = localConstant.techSpec.common.CREATE_UPDATE_PROFILE;
       this.props.actions.UpdateProfileAction(this.updatedData); 
    }  
    if(this.props.location.pathname === '/CreateProfile' && (this.props.currentPage === localConstant.techSpec.common.CREATE_PROFILE || this.props.currentPage===''))
    {
      if (this.props.currentPage === ''){
        this.props.actions.UpdateCurrentPage(localConstant.techSpec.common.Create_Profile);
      }       
      //Changes for D363 CR Change 
      let selectedCompany =this.props.userRoleCompanyList && this.props.userRoleCompanyList.filter(item => item.companyCode === this.props.selectedCompany);
      if(isEmptyOrUndefine(selectedCompany)){ //Sanity defect 93  
        selectedCompany=this.props.companyList && this.props.companyList.filter(item => item.companyCode === this.props.selectedCompany);
      }
      if(!isEmpty(selectedCompany)){ 
        this.updatedData['companyCode']=selectedCompany[0].companyCode;
        this.updatedData['companyName']=selectedCompany[0].companyName;
      }
      this.updatedData['isEnableLogin'] = true;
      this.props.actions.UpdateProfileAction(this.updatedData);
    }
    this.updatedData={};
    //SystemRole based UserType relevant Quick Fixes 
  };

  componentWillUnmount() {
    this.props.actions.CancelCreateTechSpecDetails(); 
    sessionStorage.removeItem(applicationConstants.RESOURE.INTER_COMPANY_RESOURCE_USER_TYPE);
  };

  closeErrorList = (e) => {
    e.preventDefault();
    this.setState({
      errorList: []
    });
    this.errors = [];
  }
  selectionPopUp = (e) => {
   e.preventDefault();
    this.setState({
      isSelectionPopUpOpen: true,
    });
  };
  cancelSelectionPopUp = (e) => {
    e.preventDefault();
    this.setState({
      isSelectionPopUpOpen: false
    });
  }
  getProfileDetials = (obj)=> {
    this.props.actions.GetSelectedProfile(obj).then(res => {
      if (res === true) {
        if (this.props.selectedProfileInfo && this.props.selectedProfileInfo.epin) {
          this.props.actions.FetchSelectedProfileDatails(this.props.selectedProfileInfo.epin);
        } else if (this.props.selectedProfileInfo && this.props.selectedProfileInfo.taskRefCode && this.props.selectedProfileInfo.myTaskId) {
          this.props.actions.FetchSelectedProfileDatails(this.props.selectedProfileInfo.taskRefCode, this.props.selectedProfileInfo.myTaskId);
        }
      }
    }); 
  }

  contactValidation = () => {
    const contact = Object.assign([], this.props.selectedProfileDetails.TechnicalSpecialistContact);
    let isPrimaryAddressExists = false;
    //let retValue = true;
    const contactInfoMandatoryCheck = {
      'mobileNumber': true,
      'email': true,
      'validPrimaryEmail':true,
      'validSecondaryEmail':false,
      'country': true,
      'county': true,
      'city': true,
      'postalCode':true,
      'address': true,
      'emergencyName': true,
      'emergencyNumber': true,
    };

    if (contact) {
      if (this.props.selectedProfileDetails.TechnicalSpecialistInfo !== undefined) {
        for (let j = 0; j < contact.length; j++) {
          if ("PrimaryAddress" === contact[j].contactType) {
            isPrimaryAddressExists = true;
          }
        }
        for (let i = 0; i < contact.length; i++) {
          if (contact[i].contactType === "PrimaryEmail") {
            if (!required(contact[i].emailAddress.trim())) {
              contactInfoMandatoryCheck.email = false;
            }
            else{
              contactInfoMandatoryCheck.email = true;
            }
          }
          //D377 - SecondaryEmail not using in Error popup and hence MandateCheck no need.
          // if (contact[i].contactType === "SecondaryEmail") {
          //   if (!required(contact[i].emailAddress.trim())) {
          //     //contactInfoMandatoryCheck.email = false;
          //       if(customRegExValidator(/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/, 'i',
          //       contact[i].emailAddress)){
          //         contactInfoMandatoryCheck.validSecondaryEmail=true;
          //       }
          //       else{
          //         contactInfoMandatoryCheck.validSecondaryEmail=false;
          //       }
          //   }
          // }
          if (contact[i].contactType === "PrimaryMobile") {
            if (isEmpty(contact[i].mobileNumber) || contact[i].mobileNumber === undefined || isEmpty(contact[i].mobileNumber.trim())) {
              contactInfoMandatoryCheck.mobileNumber = true;
            } else {
              contactInfoMandatoryCheck.mobileNumber = false;
            }
          }

          if (!isPrimaryAddressExists) {
            contactInfoMandatoryCheck.country = true;
            contactInfoMandatoryCheck.county = true;
            contactInfoMandatoryCheck.city = true;
            contactInfoMandatoryCheck.postalCode=true;
            contactInfoMandatoryCheck.address = true;
          }
          if (isPrimaryAddressExists && contact[i].contactType === "PrimaryAddress") {
            if (contact[i].country === undefined || isEmpty(contact[i].country)) {
              contactInfoMandatoryCheck.country = true;
            } else {
              contactInfoMandatoryCheck.country = false;
            }
            if (contact[i].county === undefined || isEmpty(contact[i].county)) {
              contactInfoMandatoryCheck.county = true;
            }
            else {
              contactInfoMandatoryCheck.county = false;
            }
            if ( ( contact[i].city === undefined || isEmpty(contact[i].city) ) && ( contact[i].postalCode === undefined || isEmpty(contact[i].postalCode ) || isEmpty(contact[i].postalCode.trim()) ) ) {
              contactInfoMandatoryCheck.city = true;
              contactInfoMandatoryCheck.postalCode=true;
            }
            else{
              contactInfoMandatoryCheck.city = false;
              contactInfoMandatoryCheck.postalCode= false;
            }
            if (contact[i].address === undefined || isEmpty(contact[i].address) || isEmpty(contact[i].address.trim())) {

              contactInfoMandatoryCheck.address = true;
            } else {
              contactInfoMandatoryCheck.address = false;
            }
          }
        }
        if (contactInfoMandatoryCheck.mobileNumber) {
          this.errors.push(`${ 
            localConstant.techSpec.contactInformation.CONTACT_INFORMATION 
          } - ${
             localConstant.techSpec.contactInformation.MOBILE_NUMBER
            }`);
        }
        if (contactInfoMandatoryCheck.email) {
          this.errors.push(`${
            localConstant.techSpec.contactInformation.CONTACT_INFORMATION
          } - ${
            localConstant.techSpec.contactInformation.EMAIL
          }`);
        }
        this.validationForDrivingandPassportDetails();
        if (contactInfoMandatoryCheck.country) {
          this.errors.push(`${
            localConstant.techSpec.contactInformation.CONTACT_INFORMATION
          } - ${
            localConstant.techSpec.contactInformation.COUNTRY_OF_RESIDENCE
          }`);
        }
        if (contactInfoMandatoryCheck.county) {
          this.errors.push(`${
            localConstant.techSpec.contactInformation.CONTACT_INFORMATION
          } - ${
            localConstant.techSpec.contactInformation.STATE_PREFECTURE_PROVINCE
          }`);
        }
        if (contactInfoMandatoryCheck.city && contactInfoMandatoryCheck.postalCode) {
          this.errors.push(`${
            localConstant.techSpec.contactInformation.CONTACT_INFORMATION
          } - ${
            localConstant.techSpec.contactInformation.EITHER_CITY_OR_POSTAL_CODE
          }`);
        }
        if (contactInfoMandatoryCheck.address) {
          this.errors.push(`${
            localConstant.techSpec.contactInformation.CONTACT_INFORMATION
            } - ${
            localConstant.techSpec.contactInformation.STREET_ADDRESS
            }`);
        }
      
      // User Name
      if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.logonName) || 
      isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.logonName.trim())) {
        this.errors.push(`${
          localConstant.techSpec.contactInformation.CONTACT_INFORMATION
        } - ${
          localConstant.techSpec.contactInformation.USER_NAME
        }`);
      }
      // // Password
      if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.password)) {
        this.errors.push(`${
          localConstant.techSpec.contactInformation.CONTACT_INFORMATION
        } - ${
          localConstant.techSpec.contactInformation.PASSWORD
        }`);
      }

       if (this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileStatus === 'Active')  {
        for (let j = 0; j < contact.length; j++) {

          if (contact[j].contactType === "Emergency") {
            // if (this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM) {
            if (required(contact[j].emergencyContactName)) {
              contactInfoMandatoryCheck.emergencyName = true;
            } else {
              contactInfoMandatoryCheck.emergencyName = false;
            }
            // }
            // else {
            //   contactInfoMandatoryCheck.emergencyName = false;
            // }
            if (required(contact[j].telephoneNumber)) {
              contactInfoMandatoryCheck.emergencyNumber = true;
            } else {
              contactInfoMandatoryCheck.emergencyNumber = false;
            }
          }
        }
        if (contactInfoMandatoryCheck.emergencyName) {
          this.errors.push(`${
            localConstant.techSpec.contactInformation.CONTACT_INFORMATION
          } - ${
            localConstant.techSpec.contactInformation.EMERGENCY_CONTACT_NAME
          }`);
        }
        if (contactInfoMandatoryCheck.emergencyNumber) {
          this.errors.push(`${
            localConstant.techSpec.contactInformation.CONTACT_INFORMATION
          } - ${
            localConstant.techSpec.contactInformation.EMERGENCY_CONTACT_NUMBER
          }`);
        }
      }
      }
    } else {
      this.errors.push(`${
        localConstant.techSpec.contactInformation.CONTACT_INFORMATION
      } - ${
        localConstant.techSpec.contactInformation.MOBILE_NUMBER
      }`);
    }
    // return retValue;
  }

  uploadedDocumentCheck=()=>{  // when file Uploading user click on sve button showing warning Toaster Here 
    let count = 0;
    if (Array.isArray(this.props.TechnicalSpecialistDocument) && this.props.TechnicalSpecialistDocument.length > 0) {
        this.props.TechnicalSpecialistDocument.map(document =>{                             
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
    const resourceStatusStartDate = this.props.selectedProfileDetails.TechnicalSpecialistInfo &&
      this.props.selectedProfileDetails.TechnicalSpecialistInfo.startDate;
    const resourceStatusEndDate = this.props.selectedProfileDetails.TechnicalSpecialistInfo &&
      this.props.selectedProfileDetails.TechnicalSpecialistInfo.endDate;
    const contactDateOfBirth = this.props.selectedProfileDetails.TechnicalSpecialistInfo &&
      this.props.selectedProfileDetails.TechnicalSpecialistInfo.dateOfBirth;
    const currentDate = moment().format(localConstant.techSpec.common.DATE_FORMAT);

    if (this.props.selectedProfileDetails) {
      const { TechnicalSpecialistInfo = {} } = this.props.selectedProfileDetails;
      const profileAction = TechnicalSpecialistInfo.profileAction;
// /**D-659 starts */
//       if(!isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileStatus) && this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileStatus==='Active')
//       {
//         if (this.props.selectedProfileDetails.TechnicalSpecialistCommodityAndEquipment && this.props.selectedProfileDetails.TechnicalSpecialistCommodityAndEquipment.filter(commodity=>commodity.recordStatus!=='D').length===0) {

//            this.errors.push(`${ 
//                 localConstant.techSpec.resourceCapability.RESOURCE_CAPABILITY } - ${
//                   localConstant.techSpec.common.ACTIVE_COMMODITY
//               } `);
//         }

//         else if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistCommodityAndEquipment))
//         {
          
//           this.errors.push(`${ 
//             localConstant.techSpec.resourceCapability.RESOURCE_CAPABILITY } - ${
//               localConstant.techSpec.common.ACTIVE_COMMODITY
//           } `);
//         }
  
//       }
//       /**D-659 ends */
      /** Send to dropdown on save of RC/RM validation */
      if (this.userTypes.includes(localConstant.techSpec.userTypes.RC) || this.userTypes.includes(localConstant.techSpec.userTypes.RM)) {
        if (required(TechnicalSpecialistInfo.profileAction)) {
          IntertekToaster(`${
            localConstant.validationMessage.SEND_TO_MANDATORY_VALIDATION
          }`, 'warningToast sendToVal');
          return false;
        }
      }
      if (TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_TM) {
        if (required(TechnicalSpecialistInfo.assignedToUser)) {
          IntertekToaster(`${
            localConstant.validationMessage.TECHSPEC_MANAGER_MANDATORY_VALIDATION
          }`, 'warningToast techSpecManagerVal');
          return false;
        }
      }
      //D1348 
      if(this.props.isTSUserTypeCheck && TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM){
        if(required(TechnicalSpecialistInfo.assignedByUser) && required(TechnicalSpecialistInfo.assignedToRCUser)){
          IntertekToaster(`${
            localConstant.validationMessage.RESOURCE_COORDINATOR_MANDATORY_VALIDATION
          }`, 'warningToast techSpecManagerVal');
          return false;
        }
      }//D1348 

      //Action Type Based Validation
      if (profileAction === "Send to TS" || profileAction === "Create/Update Profile") {
        //Contact Validation
        this.validationForRM();
        this.contactValidation();
      }

      if (profileAction === "Send to RC/RM") {
        if (this.props.isTSUserTypeCheck) {
          //Contact Validation
          this.validationForRM();
          this.contactValidation();
          this.validationForTS();
        }
        if (this.props.isTMUserTypeCheck) {
          //Profile Status is Active after Validation
          this.validationForActiveTS();
          //Contact Validation
          this.contactValidation();
          // Taxonomy Approval
          this.validationForTM();
        }
      }
      if (profileAction === "Send to TM") {
        //Contact Validation
        this.validationForRM();
        this.contactValidation();
        this.validationForTS();
      }

      this.profileStatusActiveValidation();
      if (!isEmpty(this.props.TechnicalSpecialistDocument)) {
        const issueDoc = [];
        this.props.TechnicalSpecialistDocument.map(document => {
          if(document.recordStatus!=='D')
          {
          if (isEmpty(document.documentType)) {
            this.errors.push(`${
              localConstant.techSpec.documents.DOCUMENT
            } - ${
              document.documentName
            } - ${
              localConstant.techSpec.documents.SELECT_FILE_TYPE
            } `);
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
        this.errors.push(`${ StringFormat(localConstant.project.documents.UPLOAD_ISSUE, techSpecData) }`);
      }
    }

      //Pay Schedule validation
      if(this.PayScheduleValidation()) {
        this.props.techSpecDetailTabs && this.props.techSpecDetailTabs.forEach(iteratedValue => {
          if(iteratedValue.tabBody === 'PayRate' && iteratedValue.isCurrentTab) {
            this.callBackFuncs.reloadLineItemsOnSaveValidation();
          }
        });
        this.errors.push(`${
          localConstant.techSpec.PayRate.PAY_SCHEDULE
        } - ${
          localConstant.techSpec.PayRate.PAY_RATE_DETAILS_GRID_MANDATORY_FIELDS
        }`);
      }
      if(this.IsAnyPaySchedulesWithoughtPayRate())
      {
        this.errors.push(`${
          localConstant.techSpec.PayRate.PAY_SCHEDULE
        } - ${
          localConstant.techSpec.PayRate.PAY_SCHEDULE_PAYRATE_REQUIRED
        }`); 
      }

      //Pay Rate details validation
      if(this.PayRateDetailValidation()) {
        this.props.techSpecDetailTabs && this.props.techSpecDetailTabs.forEach(iteratedValue => {
          if(iteratedValue.tabBody === 'PayRate' && iteratedValue.isCurrentTab) {
            this.callBackFuncs.reloadLineItemsOnSaveValidation();
          }
        });
        this.errors.push(`${
          localConstant.techSpec.PayRate.PAY_RATE_DETAILS
        } - ${
          localConstant.techSpec.PayRate.PAY_RATE_DETAILS_GRID_MANDATORY_FIELDS
        }`);
      }

      //Added for D1395
      if (isEmptyReturnDefault(this.props.selectedProfileDetails.TechnicalSpecialistWorkHistory).filter(x => x.recordStatus !== "D" && ( isEmptyOrUndefine(x.description) || isEmptyOrUndefine(x.description.replace(/(<([^>]+)>)/ig, '')) )).length>0) {
        this.errors.push(`${ 
          localConstant.techSpec.professionalEducationalDetails.PROFESSIONAL_EDUCATION_DETAILS } - ${
          localConstant.techSpec.professionalEducationalDetails.WORK_HISTORY_DETAILS
        } - ${ localConstant.contract.common.REQUIRED_TEXT } Description`); 
      }
 
      //Added for D1183
      if( this.props.isRCUserTypeCheck && this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.CREATE_UPDATE_PROFILE){
        const certificateData = isEmptyReturnDefault(this.props.selectedProfileDetails.TechnicalSpecialistCertification).filter(x => x.recordStatus !== "D"); 
        const flag = certificateData.filter(x => x.verificationStatus === null || x.verificationStatus === '' || x.verificationStatus === undefined).length > 0;
        if(flag) {
          this.errors.push(`${
            localConstant.techSpec.resourceCapability.RESOURCE_CAPABILITY_CERTIFICATION
          } - ${
            localConstant.techSpec.resourceCapability.CERTIFICATE_DETAILS
          } - ${
            localConstant.techSpec.resourceCapability.VERIFICATION_STATUS
          }`);
        }
      }
  /* D1183 **/
  /** ITKD1261 */
  if(!isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistEducation) && this.props.selectedProfileDetails.TechnicalSpecialistEducation.length > 0 &&
  (this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileAction !== localConstant.techSpec.common.SEND_TO_RC_RM || this.props.isTSUserTypeCheck)){
       const education =this.props.selectedProfileDetails.TechnicalSpecialistEducation.filter(x => x.recordStatus !== 'D');
       const ishavingdocument = education && education.filter(x => x.documents == null || (x.documents && x.documents.length == 0));
       if(ishavingdocument && ishavingdocument.length > 0){
        this.errors.push(`${
          localConstant.techSpec.professionalEducationalDetails.PROFESSIONAL_EDUCATION_DETAILS
        } - ${
          localConstant.techSpecValidationMessage.EDUCATIONAL_SUMMARY_DOCUMENT_VALIDATION
        }`);
       }    
   }
   /** ITKD1261 */

   if(!isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.professionalSummary)){
     const summary =this.props.selectedProfileDetails.TechnicalSpecialistInfo.professionalSummary;
     if(summary.length > 4000){
      this.errors.push(`${
        localConstant.techSpec.professionalEducationalDetails.PROFESSIONAL_EDUCATION_DETAILS
      } - ${
        localConstant.techSpecValidationMessage.OVERALL_PROFESSIONAL_SUMMARY_EXISTS
      }`);
     } 
   }
      if (this.errors.length > 0) {
        this.setState({
          errorList: this.errors
        });
        return false;
      }
      else {
        //Resource status Start Date and End Date validation
        if (resourceStatusEndDate) {
          if (moment(resourceStatusStartDate).isAfter(resourceStatusEndDate)) {
            IntertekToaster(`${
              localConstant.techSpec.resourceStatus.RESOURCE_STATUS
            } - ${
              localConstant.techSpecValidationMessage.START_DATE_END_DATE_VALIDATION
            }`, 'warningToast');
            return false;
          }
        }
        //Start Date Validation(Entering Start Date before entering End Date):
        if ((resourceStatusStartDate === undefined || resourceStatusStartDate === "" || resourceStatusStartDate === null) &&
          !isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.endDate)) {
          IntertekToaster(`${
            localConstant.techSpec.resourceStatus.RESOURCE_STATUS
          } - ${
            localConstant.techSpecValidationMessage.START_DATE_VALIDATION
          }`, 'warningToast');
          return false;
        }
         /** Reg ITKD1302 -- Need to Remove the Name Validation  
         * ref Email-- Reg: Special letters in Resource name
         */
        // // First Name RegEx Validation
        // if (!isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.firstName) && 
        // (!isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.firstName.trim()) 
        // && customRegExValidator(/^[a-zA-Z !@#$%^&_*()+\-=[\]{};':"`\\|,.<>/?]*$/, 'i',
        //   this.props.selectedProfileDetails.TechnicalSpecialistInfo.firstName.trim()) //`symbol include Issue Raised by E-mail (Sub:New issue in ITK End to End testing, on:01-09-2020, from:Francina)
        // )) {
        //   IntertekToaster(`${
        //     localConstant.techSpec.contactInformation.CONTACT_INFORMATION
        //   } - ${
        //     localConstant.techSpecValidationMessage.FIRST_NAME_REGEX_VALIDATION
        //   }`, 'warningToast firstNameReq');
        //   return false;
        // }

        // // Middle Name Validation
        // if (!isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.middleName) &&
        //   customRegExValidator(/^[a-zA-Z !@#$%^&_*()+\-=[\]{};':"`\\|,.<>/?]*$/, 'i',
        //     this.props.selectedProfileDetails.TechnicalSpecialistInfo.middleName)) {
        //   IntertekToaster(`${
        //     localConstant.techSpec.contactInformation.CONTACT_INFORMATION
        //   } - ${
        //     localConstant.techSpecValidationMessage.MIDDLE_NAME_REGEX_VALIDATION
        //   }`, 'warningToast middleNameReq');
        //   return false;
        // }

        // // Last Name RegEx Validation
        // if (!isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.lastName) && 
        //       (!isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.lastName.trim()) &&
        //       customRegExValidator(/^[a-zA-Z !@#$%^&_*()+\-=[\]{};':"`\\|,.<>/?]*$/, 'i',
        //     this.props.selectedProfileDetails.TechnicalSpecialistInfo.lastName.trim()))) {
        //   IntertekToaster(`${
        //     localConstant.techSpec.contactInformation.CONTACT_INFORMATION
        //   } - ${
        //     localConstant.techSpecValidationMessage.LAST_NAME_REGEX_VALIDATION
        //   }`, 'warningToast lastNameReq');
        //   return false;
        // }
         /** Reg ITKD1302 -- Need to Remove the Name Validation  
         * ref Email-- Reg: Special letters in Resource name
         */

        //Date of Birth Validation kar
        const Oldinfo = this.props.selectedProfileDraftDataToCompare;
          if (contactDateOfBirth) {
              if (moment(contactDateOfBirth).isSameOrAfter(currentDate)) {
                 IntertekToaster(`${
                   localConstant.techSpec.contactInformation.CONTACT_INFORMATION
                 } - ${
                   localConstant.techSpecValidationMessage.INVALID_DOB_VALIDATION
                 }`, 'warningToast');
                    return false;
            }
            //Sanity Def 181
            const dob =moment(contactDateOfBirth).format(localConstant.commonConstants.UI_DATE_FORMAT);
            const isValid = dateUtil.isUIValidDate(dob);
            if (!isValid) {
              IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING_DOB, 'warningToast');
              return false;
            } //Sanity Def 181
           } else if(Oldinfo.TechnicalSpecialistInfo && Oldinfo.TechnicalSpecialistInfo.dateOfBirth) { //Added for Saity Def181 
            IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING_DOB, 'warningToast');
            return false;
           }

        //Primary Email Validation
        if(!isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.emailAddress) && 
        customRegExValidator(/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/, 'i',
        this.props.selectedProfileDetails.TechnicalSpecialistInfo.emailAddress)){
            IntertekToaster(`${ localConstant.techSpec.contactInformation.CONTACT_INFORMATION } - ${ localConstant.techSpecValidationMessage.PRIMARY_EMAIL_VALIDATION }`, 'warningToast lastNameReq');
            return false;
        } 

        //Secondary Email Validation
        if(!isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.secondaryEmailAddress) && 
        customRegExValidator(/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/, 'i',
        this.props.selectedProfileDetails.TechnicalSpecialistInfo.secondaryEmailAddress)){
            IntertekToaster(`${ localConstant.techSpec.contactInformation.CONTACT_INFORMATION } - ${ localConstant.techSpecValidationMessage.SECONDARY_EMAIL_VALIDATION }`, 'warningToast lastNameReq');
            return false;
        } 

        if (profileAction === "Send to TS" && !TechnicalSpecialistInfo.isEnableLogin) {
          IntertekToaster(`${
            localConstant.techSpec.contactInformation.CONTACT_INFORMATION
          } - ${
            localConstant.techSpecValidationMessage.ENABLE_EXTRANET_ACCOUNT_VALIDATION
          }`, 'warningToast');
          return false;
        }

        if (!isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileStatus)) {
          if (this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileStatus === "Active") {
            let isApprovalStatusNotActive = true;
            if (this.props.selectedProfileDetails.TechnicalSpecialistTaxonomy && this.props.selectedProfileDetails.TechnicalSpecialistTaxonomy.length > 0) {
              const taxonomyData = this.props.selectedProfileDetails.TechnicalSpecialistTaxonomy.filter(taxonomy => taxonomy.recordStatus !== 'D' && taxonomy.approvalStatus !== 'Approve');
              if (this.props.selectedProfileDetails.TechnicalSpecialistTaxonomy.length === taxonomyData.length) {
                IntertekToaster(`${
                  localConstant.techSpec.resourceStatus.RESOURCE_STATUS
                } - ${
                  localConstant.techSpecValidationMessage.PROFILE_STATUS_ACTIVE_WARNING_VALIDATION
                }`, 'warningToast profileStatusReq');
                isApprovalStatusNotActive = false;
              }
              if (!isApprovalStatusNotActive) {
                return isApprovalStatusNotActive;
              }
            }
            else {
              IntertekToaster(`${
                localConstant.techSpec.resourceStatus.RESOURCE_STATUS
              } - ${
                localConstant.techSpecValidationMessage.PROFILE_STATUS_ACTIVE_WARNING_VALIDATION
              }`, 'warningToast profileStatusReq');
              return false;
            }
          }
        }
      }
      return true;
    }
    else {
      return false;
    }
  }

  profileStatusActiveValidation =()=>{
            // Profile status(Active) Warning
        if (!isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileStatus)) {
          if (this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileStatus === "Active") {
            if(this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.CREATE_UPDATE_PROFILE) //Sanity Def 176
              this.validationForTS(); //IGO QC D886 #7 Issue(Ref ALM Confirmation Doc)
            // let isApprovalStatusNotActive = true;
            // // // if(isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.startDate))
            // // // {
            // // //   IntertekToaster(`${ localConstant.techSpec.resourceStatus.RESOURCE_STATUS } - ${ localConstant.techSpecValidationMessage.START_DATE_VALIDATION }`, 'warningToast profileStatusReq');
            // // //       isApprovalStatusNotActive=false;
            // // //       return false;
            // // // }
            // if (this.props.selectedProfileDetails.TechnicalSpecialistTaxonomy && this.props.selectedProfileDetails.TechnicalSpecialistTaxonomy.length > 0) {
            //   const taxonomyData = this.props.selectedProfileDetails.TechnicalSpecialistTaxonomy.filter(taxonomy => taxonomy.recordStatus !== 'D' && taxonomy.approvalStatus !== 'Approve');
            //   //  taxonomyData.map(taxonomy => {
            //   if (this.props.selectedProfileDetails.TechnicalSpecialistTaxonomy.length === taxonomyData.length) {
            //     IntertekToaster(`${
            //       localConstant.techSpec.resourceStatus.RESOURCE_STATUS
            //     } - ${
            //       localConstant.techSpecValidationMessage.PROFILE_STATUS_ACTIVE_WARNING_VALIDATION
            //     }`, 'warningToast profileStatusReq');
            //     isApprovalStatusNotActive = false;
            //   }
            //   //   });
            //   if (!isApprovalStatusNotActive) {
            //     return isApprovalStatusNotActive;
            //   }
            // }
            // else {
            //   IntertekToaster(`${
            //     localConstant.techSpec.resourceStatus.RESOURCE_STATUS
            //   } - ${
            //     localConstant.techSpecValidationMessage.PROFILE_STATUS_ACTIVE_WARNING_VALIDATION
            //   }`, 'warningToast profileStatusReq');
            //   return false;
            // }
            //Taxonomy Date Range Validation Start
            if (!isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistTaxonomy)&& this.props.isRCUserTypeCheck && this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.CREATE_UPDATE_PROFILE) {
              const techSpecInfo = isEmptyReturnDefault(this.props.selectedProfileDetails.TechnicalSpecialistInfo, 'object');
              const isPreviouslyProfileMadeActive = techSpecInfo && techSpecInfo.isPreviouslyProfileMadeActive; 
              if (!required(techSpecInfo.startDate)) {
                const acceptTaxonomy = this.props.selectedProfileDetails.TechnicalSpecialistInfo.taxonomyStatus;
                if (acceptTaxonomy === "Reject") {
                  const approvedTaxonomy = this.props.selectedProfileDetails.TechnicalSpecialistTaxonomy.filter(x => (x.approvalStatus === 'Approve' && x.recordStatus !== 'D' && x.taxonomyStatus === 'Accept'));
                  if (isEmpty(approvedTaxonomy) && !isPreviouslyProfileMadeActive) {//D954
                    this.errors.push(`${
                      localConstant.techSpec.resourceStatus.RESOURCE_STATUS
                    } - ${ localConstant.techSpecValidationMessage.TM_APPROVAL_STATUS_ACCEPT_REQUIRED }`);
                  }
                }
                if (acceptTaxonomy === "Accept") {
                  const approvedTaxonomy = this.props.selectedProfileDetails.TechnicalSpecialistTaxonomy.filter(x => (x.approvalStatus === 'Approve' && x.recordStatus !== 'D'));
                  if (isEmpty(approvedTaxonomy)) {
                    this.errors.push(`${
                      localConstant.techSpec.resourceStatus.RESOURCE_STATUS
                    } - ${ localConstant.techSpecValidationMessage.IS_HAS_APPROVED_TAXONOMY }`);
                  }
                }
                  const approvedTaxonomy = isEmptyReturnDefault(this.props.selectedProfileDetails.TechnicalSpecialistTaxonomy);
                  const approvedTaxonomywithToDate = approvedTaxonomy.filter(x => (x.approvalStatus === 'Approve' && x.recordStatus !== 'D' && x.toDate !== null)); //D1156 issue 2 
                  const approvedTaxonomywithoutToDate = approvedTaxonomy.filter(x => (x.approvalStatus === 'Approve' && x.recordStatus !== 'D' && x.toDate === null)); //D1156 issue 2 
                    let isApprovedTaxonomy = false; 
                    
                    if(approvedTaxonomywithToDate.length > 0){
                      approvedTaxonomywithToDate.forEach(row => {
                          if (!required(techSpecInfo.endDate)) {
                            if (moment(techSpecInfo.endDate).isSameOrAfter(row.fromDate, 'day')) {
                              isApprovedTaxonomy = true;
                            }
                          }
                        
                        if (!required(row.toDate)) {
                          const count = approvedTaxonomywithToDate.filter( x => moment(x.toDate).isBefore(moment(), 'day')).length;
                          if(count === approvedTaxonomywithToDate.length)
                              isApprovedTaxonomy = false;
                          else{ isApprovedTaxonomy = true; }
                        }   
                        if(moment(techSpecInfo.startDate).isBefore(row.fromDate, 'day') && !isPreviouslyProfileMadeActive) {
                          isApprovedTaxonomy = false;
                        }
                        // else {
                        //   isApprovedTaxonomy = true;
                        // }
                      });
                    }

                    if(approvedTaxonomywithoutToDate.length > 0){
                      approvedTaxonomywithoutToDate.some(function (row, index, _arr) {
                        if (isPreviouslyProfileMadeActive) {
                          if (!required(techSpecInfo.endDate)) {
                            if (moment(techSpecInfo.endDate).isSameOrAfter(row.fromDate, 'day')) {
                              isApprovedTaxonomy = true;
                              return true;
                            }
                          }
                          else {
                            isApprovedTaxonomy = true;
                            return true;
                          }
                        } else if(moment(techSpecInfo.startDate).isBefore(row.fromDate, 'day')) {
                          isApprovedTaxonomy = false;
                        }
                        else {
                          isApprovedTaxonomy = true;
                          return true;
                        }
                      });
                    }

                    if (!isApprovedTaxonomy) {
                        this.errors.push(`${
                          localConstant.techSpec.resourceStatus.RESOURCE_STATUS
                        } -  ${ localConstant.techSpecValidationMessage.DATE_RANGE_VALITATION_TAXONOMY }`);
                    }
              }
            }
            //Taxonomy Date Range Validation End

            ///    if(this.props.selectedProfileDetails.TechnicalSpecialistStamp && this.props.selectedProfileDetails.TechnicalSpecialistStamp.length > 0){  Commented for Defect ID 404
            if (this.props.selectedProfileDetails.TechnicalSpecialistInfo) {
              if (required(this.props.selectedProfileDetails.TechnicalSpecialistInfo.taxReference)) {
                this.errors.push(`${
                  localConstant.techSpec.PayRate.PAY_RATE
                } - ${
                  localConstant.techSpec.PayRate.TAX_REF_NO
                }`);
              }
              if (required(this.props.selectedProfileDetails.TechnicalSpecialistInfo.payrollReference)) {
                this.errors.push(`${
                  localConstant.techSpec.PayRate.PAY_RATE
                } - ${
                  localConstant.techSpec.PayRate.PAYROL_REFERNCE
                }`);
              }
              if (required(this.props.selectedProfileDetails.TechnicalSpecialistInfo.companyPayrollName)) {
                this.errors.push(`${
                  localConstant.techSpec.PayRate.PAY_RATE
                } - ${
                  localConstant.techSpec.PayRate.PAYROLL_NAME
                }`);
              }
            }
            //Sanity Def 151
            let paySchedule = this.props.selectedProfileDetails.TechnicalSpecialistPaySchedule;
            if (isEmpty(paySchedule)) {
              this.errors.push(`${
                localConstant.techSpec.PayRate.PAY_RATE
              } - ${
                localConstant.techSpec.PayRate.PAY_RATE_SCEDULEE
              }`);
            }
            else {
              paySchedule = paySchedule.filter(x => x.recordStatus !== 'D');
              if (isEmpty(paySchedule)) {
                this.errors.push(`${
                  localConstant.techSpec.PayRate.PAY_RATE
                } - ${
                  localConstant.techSpec.PayRate.PAY_RATE_SCEDULEE
                }`);
              }
            }
            //Sanity Def 151
            if (!isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistPaySchedule)) {
              let scheduleHasNoRates = false;
              this.props.selectedProfileDetails.TechnicalSpecialistPaySchedule.forEach(iteratedValue => {
                if (iteratedValue.recordStatus !== 'D') {
                  const scheduleRates = this.props.selectedProfileDetails.TechnicalSpecialistPayRate && this.props.selectedProfileDetails.TechnicalSpecialistPayRate.find(x => (x.payScheduleName === iteratedValue.payScheduleName && x.recordStatus !== 'D'));
                  if (isEmpty(scheduleRates)) {
                    scheduleHasNoRates = true;
                  }
                }
              });
              // if (scheduleHasNoRates) {
              //   this.errors.push(`${
              //     localConstant.techSpec.PayRate.PAY_SCHEDULE
              //   } - ${ 
              //     localConstant.techSpec.PayRate.PAY_SCHEDULE_PAYRATE_REQUIRED 
              //   }`);
              // }
            }            
            ////   }   Commented for Defect ID 404
            if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistWorkHistory)) {
              this.errors.push(`${ 
                localConstant.techSpec.professionalEducationalDetails.PROFESSIONAL_EDUCATION_DETAILS } - ${
                localConstant.techSpec.professionalEducationalDetails.WORK_HISTORY_DETAILS
              } - Should have at least one current company.`);
            }
            if (!isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistWorkHistory)) {
              const techSpecInfo = isEmptyReturnDefault(this.props.selectedProfileDetails.TechnicalSpecialistInfo, 'object');
              if (!required(techSpecInfo.startDate)) {
                const currentCompanyWorkHistory = this.props.selectedProfileDetails.TechnicalSpecialistWorkHistory.filter(x => (x.isCurrentCompany === true && x.recordStatus !== 'D'));
                if (isEmpty(currentCompanyWorkHistory)) {
                  this.errors.push(`${
                    localConstant.techSpec.professionalEducationalDetails.PROFESSIONAL_EDUCATION_DETAILS } - ${
                    localConstant.techSpec.professionalEducationalDetails.WORK_HISTORY_DETAILS
                  } - ${ localConstant.techSpecValidationMessage.WORK_HISTORY_VALIDTION }`);
                }
                else if(currentCompanyWorkHistory.length >1) //def 1395 & 1187 fix
                {
                  this.errors.push(`${ 
                    localConstant.techSpec.professionalEducationalDetails.PROFESSIONAL_EDUCATION_DETAILS } - ${
                    localConstant.techSpec.professionalEducationalDetails.WORK_HISTORY_DETAILS
                  } - ${ localConstant.techSpecValidationMessage.WORK_HISTORY_SINGLE_CURRENT_COMPANY_VALIDTION } `); 
                }
              }
            }

            /**D-659 starts */
            if(!isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileStatus) && this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileStatus==='Active')
            {
              if (this.props.selectedProfileDetails.TechnicalSpecialistCommodityAndEquipment && this.props.selectedProfileDetails.TechnicalSpecialistCommodityAndEquipment.filter(commodity=>commodity.recordStatus!=='D').length===0) {

                this.errors.push(`${ 
                      localConstant.techSpec.resourceCapability.RESOURCE_CAPABILITY_CERTIFICATION } - ${
                        localConstant.techSpec.common.ACTIVE_COMMODITY
                    } `);
              }

              else if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistCommodityAndEquipment))
              {
                
                this.errors.push(`${ 
                  localConstant.techSpec.resourceCapability.RESOURCE_CAPABILITY_CERTIFICATION } - ${
                    localConstant.techSpec.common.ACTIVE_COMMODITY
                } `);
              }
            }
            /**D-659 ends */

          }
        }
  }
  validationForRM = () => {
    if (this.props.selectedProfileDetails.TechnicalSpecialistInfo != undefined) {
      if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.companyCode)) { //D363 CR Change
        this.errors.push(`${
          localConstant.techSpec.resourceStatus.RESOURCE_STATUS
        } - ${
          localConstant.techSpec.resourceStatus.COMPANY
        }`);
      }

      //Sub Division
      if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.subDivisionName)) {
        this.errors.push(`${
          localConstant.techSpec.resourceStatus.RESOURCE_STATUS
        } - ${
          localConstant.techSpec.resourceStatus.SUB_DIVISION
        }`);
      }

      //Profile Status 
      if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileStatus)) {
        this.errors.push(`${
          localConstant.techSpec.resourceStatus.RESOURCE_STATUS
        } - ${
          localConstant.techSpec.resourceStatus.PROFILE_STATUS
        }`);
      }
      
         // TS Change And TM Taxonomy change Approval validation 
   if (this.props.currentPage === "Edit/View Resource" && (this.userTypes.includes(localConstant.techSpec.userTypes.RC) || this.userTypes.includes(localConstant.techSpec.userTypes.RM))) {
    if (this.props.selectedProfileDetails && (this.props.selectedProfileDetails.TechnicalSpecialistInfo.approvalStatus === localConstant.techSpec.tsChangeApprovalStatus.InProgress)) { //IGO QC D889
      this.errors.push(`${
        localConstant.techSpec.resourceStatus.RESOURCE_STATUS
        } -${
        localConstant.techSpec.resourceStatus.TS_APPROVAL_STATUS
        }`);
    }

    if (this.props.selectedProfileDetails && (this.props.selectedProfileDetails.TechnicalSpecialistInfo.approvalStatus === localConstant.techSpec.tsChangeApprovalStatus.Rejected) && isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.businessInformationComment)) {
      this.errors.push(`${
        localConstant.techSpec.resourceStatus.RESOURCE_STATUS
        } -${
          localConstant.techSpec.contactInformation.BUSINESS_INFORMATION_COMMENTS
        }`);
    }

    const taxonomyStatus = (this.props.selectedProfileDetails.TechnicalSpecialistTaxonomy && this.props.selectedProfileDetails.TechnicalSpecialistTaxonomy.filter(x => (x.recordStatus !== 'D' && x.taxonomyStatus === 'IsAcceptRequired')));
    const acceptTaxonomy = this.props.selectedProfileDetails.TechnicalSpecialistInfo.taxonomyStatus;
    const isPreviouslyProfileMadeActive = this.props.selectedProfileDetails.TechnicalSpecialistInfo.isPreviouslyProfileMadeActive; //D684
    const profileStatus= this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileStatus;
    if ((!isEmpty(taxonomyStatus) && taxonomyStatus.length > 0) && isEmpty(acceptTaxonomy) && (this.props.selectedProfileDetails && !this.props.selectedProfileDetails.TechnicalSpecialistInfo.isDraft) &&  isPreviouslyProfileMadeActive) { //sanity issue and D684 Fixed -- //D954
      this.errors.push(`${
        localConstant.techSpec.resourceStatus.RESOURCE_STATUS
        } -${
        localConstant.techSpec.resourceStatus.TAXONOMY_APPROVAL_STATUS
        }`);
    }
      }

      this.validationForActiveTS();

       const profileAction =this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileAction;
      if(profileAction === "Send to RC/RM" || profileAction === "Send to TM"){
         // Salutation
         if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.salutation)) {
          this.errors.push(`${
            localConstant.techSpec.contactInformation.CONTACT_INFORMATION
          } - ${
            localConstant.techSpec.contactInformation.SALUTATION
          }`);
        }
      }
     
      //First Name
      if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.firstName) || 
      isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.firstName.trim())) {
        this.errors.push(`${
          localConstant.techSpec.contactInformation.CONTACT_INFORMATION
        } - ${
          localConstant.techSpec.contactInformation.FIRST_NAME
        }`);
      }
      // Last Name
      if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.lastName) ||
      isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.lastName.trim())) {
        this.errors.push(`${
          localConstant.techSpec.contactInformation.CONTACT_INFORMATION
        } - ${
          localConstant.techSpec.contactInformation.LAST_NAME
        }`);
      }

      // // //TO-DO Once Security Module has been done
      // // // User Name
      // // if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.logonName)) {
      // //   this.errors.push(`${
      // //     localConstant.techSpec.contactInformation.CONTACT_INFORMATION
      // //   } - ${
      // //     localConstant.techSpec.contactInformation.USER_NAME
      // //   }`);
      // // }
      // // // // Password
      // // if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.password)) {
      // //   this.errors.push(`${
      // //     localConstant.techSpec.contactInformation.CONTACT_INFORMATION
      // //   } - ${
      // //     localConstant.techSpec.contactInformation.PASSWORD
      // //   }`);
      // // }
    }
    else {
      this.errors.push(`${
        localConstant.techSpec.contactInformation.CONTACT_INFORMATION
      }`);
    }
  }

  validationForActiveTS=()=>{
   //Employment type
   const profileStatus = this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileStatus;
   if (profileStatus === 'Active') {
     if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.employmentType)) {
       this.errors.push(`${
         localConstant.techSpec.resourceStatus.RESOURCE_STATUS
       } - ${
         localConstant.techSpec.resourceStatus.EMPLOYMENT_TYPE
       }`);
     }
   } 
   if(profileStatus === 'Inactive' || profileStatus === 'Suspended'){
     if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.employmentType)) {
       this.errors.push(`${ localConstant.techSpec.resourceStatus.RESOURCE_STATUS } - ${ localConstant.techSpec.resourceStatus.EMPLOYMENT_TYPE }`);
     }
   }

   //Start Date 
   if (profileStatus === 'Active' && isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.startDate)) {
     this.errors.push(`${
       localConstant.techSpec.resourceStatus.RESOURCE_STATUS
     } - ${
       localConstant.techSpec.resourceStatus.START_DATE
     }`);
   }
  }

  validationForDrivingandPassportDetails=()=>{
 // Licence Expiry Date
      if (!isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.drivingLicenseNo)) {
        if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.drivingLicenseExpiryDate)) {
          this.errors.push(`${
            localConstant.techSpec.contactInformation.CONTACT_INFORMATION
          } - Driving License ${
            localConstant.techSpec.contactInformation.EXPIRY_DATE
          }`);
        }
      }

      //Passport Expiry Date
      if (!isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.passportNo)) {
        if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.passportExpiryDate)) {
          this.errors.push(`${
            localConstant.techSpec.contactInformation.CONTACT_INFORMATION
          } - Passport ${
            localConstant.techSpec.contactInformation.EXPIRY_DATE
          }`);
        }
      }

      // Country Of Origin
      if (!isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.passportNo)) {
        if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.passportCountryName)) {
          this.errors.push(`${
            localConstant.techSpec.contactInformation.CONTACT_INFORMATION
          } - ${
            localConstant.techSpec.contactInformation.COUNTRY_OF_ORIGIN
          }`);
        }
      }
  }

  validationForTS = () => {
    if (this.props.selectedProfileDetails.TechnicalSpecialistInfo != undefined) {
      // // // Salutation
      // // if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.salutation)) {
      // //   this.errors.push(`${
      // //     localConstant.techSpec.contactInformation.CONTACT_INFORMATION
      // //   } - ${
      // //     localConstant.techSpec.contactInformation.SALUTATION
      // //   }`);
      // // }

      //TO-DO Once Security Module has been done
      // Password Security Question
      if(this.props.isTSUserTypeCheck || this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileStatus === 'Active'){ //D655(REf ALM Doc 02-04-2020) //IGO QC D886 profileStatus Check Added for (Ref ALM Confirmation Doc)
      if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.securityQuestion)) {
        this.errors.push(`${
          localConstant.techSpec.contactInformation.CONTACT_INFORMATION
        } - ${
          localConstant.techSpec.contactInformation.PASSWORD_SECURITY_QUESTION
        }`);
      }
      // Security Answer
      if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.securityAnswer)) {
        this.errors.push(`${
          localConstant.techSpec.contactInformation.CONTACT_INFORMATION
        } - ${
          localConstant.techSpec.contactInformation.PASSWORD_ANSWER
        }`);
      }
    }
      // Over All Professional Summary
      // if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.professionalSummary)) {
      //   this.errors.push(`${ localConstant.techSpec.professionalEducationalDetails.PROFESSIONAL_EDUCATION_DETAILS } - ${ localConstant.techSpec.professionalEducationalDetails.PROFESSIONAL_SUMMARY }`);
      // }    Commented for D-228
      // UPLOAD_CV
      if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.professionalAfiliationDocuments) || this.props.selectedProfileDetails.TechnicalSpecialistInfo.professionalAfiliationDocuments.filter(x=>x.recordStatus!=='D').length===0) { // sanity def 200 
        this.errors.push(`${
          localConstant.techSpec.professionalEducationalDetails.PROFESSIONAL_EDUCATION_DETAILS
        } - ${
          localConstant.techSpec.professionalEducationalDetails.UPLOAD_CV
        }`);
      }
    }
    else {
      this.errors.push(`${
        localConstant.techSpec.contactInformation.CONTACT_INFORMATION
      }`);
    }
    const profileAction=this.props.selectedProfileDetails.TechnicalSpecialistInfo && this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileAction;
    if(profileAction !== localConstant.techSpec.common.SEND_TO_TM){
    // Overall Professional Summary
    if(required(this.props.selectedProfileDetails.TechnicalSpecialistInfo.professionalSummary)){ //IGO QC D886 #8Issue (06-10-2020 Failed)
      this.errors.push(`${
        localConstant.techSpec.professionalEducationalDetails.PROFESSIONAL_EDUCATION_DETAILS
      } - ${
        localConstant.techSpec.professionalEducationalDetails.OVERALL_PROFESSIONAL_SUMMARY
      }`);
    }
    // Work History
    if (this.props.selectedProfileDetails.TechnicalSpecialistWorkHistory === undefined || this.props.selectedProfileDetails.TechnicalSpecialistWorkHistory <= 0) {
      this.errors.push(`${
        localConstant.techSpec.professionalEducationalDetails.PROFESSIONAL_EDUCATION_DETAILS
      } - ${
        localConstant.techSpec.professionalEducationalDetails.WORK_HISTORY_DETAILS
      }`);
    }

    //Computer / Electronic Knowledge //IGO QC D886 #3 Issue
    if(this.props.selectedProfileDetails.TechnicalSpecialistComputerElectronicKnowledge === undefined || 
      this.props.selectedProfileDetails.TechnicalSpecialistComputerElectronicKnowledge === null ||  this.props.selectedProfileDetails.TechnicalSpecialistComputerElectronicKnowledge.filter(val => val.recordStatus !== 'D').length <= 0){ //IGO QC D886 #8Issue (06-10-2020 Failed)
      this.errors.push(`${
        localConstant.techSpec.resourceCapability.RESOURCE_CAPABILITY_CERTIFICATION
      } - ${
        localConstant.techSpec.resourceCapability.COMPUTER_ELECTRONIC_KNOWLEDGE
      }`);
    }
    // Language Capability
    if (this.props.selectedProfileDetails.TechnicalSpecialistLanguageCapabilities === undefined || 
      this.props.selectedProfileDetails.TechnicalSpecialistLanguageCapabilities === null || this.props.selectedProfileDetails.TechnicalSpecialistLanguageCapabilities.filter(val => val.recordStatus !== 'D').length <= 0) { //IGO QC D886 #8Issue (06-10-2020 Failed)
      this.errors.push(`${
        localConstant.techSpec.resourceCapability.RESOURCE_CAPABILITY_CERTIFICATION
      } - ${
        localConstant.techSpec.resourceCapability.LANGUAGE_CAPABILITIES
      }`);
    }
  }

    // Certificate Details // Commented Below valiadation for def 774,#21 
    // if (this.props.selectedProfileDetails.TechnicalSpecialistCertification === undefined || this.props.selectedProfileDetails.TechnicalSpecialistCertification <= 0) {
    //   this.errors.push(`${
    //     localConstant.techSpec.resourceCapability.RESOURCE_CAPABILITY_CERTIFICATION
    //   } - ${
    //     localConstant.techSpec.resourceCapability.CERTIFICATE_DETAILS
    //   }`);
    // }

     // Commodity / Equipment Knowledge
   // const profileStatus = this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileStatus;
   // if (profileStatus !== "Pre-qualification") { //D954(Issue #2.1) Ref ALM (28-03-2020 Doc)
      if (this.props.selectedProfileDetails.TechnicalSpecialistCommodityAndEquipment === undefined || this.props.selectedProfileDetails.TechnicalSpecialistCommodityAndEquipment <= 0) {
        this.errors.push(`${
          localConstant.techSpec.resourceCapability.RESOURCE_CAPABILITY_CERTIFICATION
        } - ${
          localConstant.techSpec.resourceCapability.COMMODITY_EQUIPMENT_KNOWLEDGE
        }`);
      }
   // }
  
    // Training Details // Commented Below valiadation for def 774,#21 
  //   if (this.props.selectedProfileDetails.TechnicalSpecialistTraining === undefined || this.props.selectedProfileDetails.TechnicalSpecialistTraining <= 0) {
  //     this.errors.push(`${
  //       localConstant.techSpec.resourceCapability.RESOURCE_CAPABILITY_CERTIFICATION
  //     } - ${
  //       localConstant.techSpec.resourceCapability.TRAINING_DETAILS
  //     }`);
  //   }
  }
  validationForTM = () => {
    if (this.props.selectedProfileDetails.TechnicalSpecialistTaxonomy === undefined || this.props.selectedProfileDetails.TechnicalSpecialistTaxonomy <= 0) {
      this.errors.push(`${
        localConstant.techSpec.taxonomyApproval.TAXONOMY_APPROVAL_DETAILS
      } - ${
        localConstant.techSpec.taxonomyApproval.TAXONOMY_APPROVAL_DETAILS
      }`);
    }
    else if (this.props.selectedProfileDetails.TechnicalSpecialistTaxonomy && !(this.props.selectedProfileDetails.TechnicalSpecialistTaxonomy.filter(taxonomy => taxonomy.recordStatus !== 'D').length > 0)) {
      this.errors.push(`${
        localConstant.techSpec.taxonomyApproval.TAXONOMY_APPROVAL_DETAILS
      } - ${
        localConstant.techSpec.taxonomyApproval.TAXONOMY_APPROVAL_DETAILS
      }`);
    }
  }

  sendToOnChangeHandler = (e) => {
    const result = formInputChangeHandler(e);
    this.updatedData[result.name] = result.value;
    this.updatedData['assignedToUser']='';       // Added for D946 CR 
    if (this.props.isbtnDisable) {
      this.updatedData['isbtnDisable'] = !this.props.isbtnDisable;
    }
    else {
      this.updatedData['isbtnDisable'] = this.props.isbtnDisable;
    }
    this.props.actions.UpdateProfileAction(this.updatedData);
    if(result.value === localConstant.techSpec.common.SEND_TO_TM){
      this.props.actions.FetchTechnicalManager();
    }
    this.updatedData = {};
  }

  technicalManagerOnChangeHandler = (e) => {
    const result = formInputChangeHandler(e);
    this.updatedData[result.name] = result.value;
    if (this.props.isbtnDisable) {
      this.updatedData['isbtnDisable'] = !this.props.isbtnDisable;
    }
    else {
      this.updatedData['isbtnDisable'] = this.props.isbtnDisable;
    }
    this.props.actions.UpdateProfileAction(this.updatedData);
    this.updatedData = {};
  }

  /** Tab filter based on conditions */
  filterTabDetails = () => { 
    const tabsToHide = [];
    if (this.userTypes) {
      if (!this.technicalManager()) {
        tabsToHide.push("TaxonomyApproval");
      } 
      if (this.userTypes.includes(localConstant.techSpec.userTypes.TS)) {
       //if(this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileStatus!== 'Active')
      // {
    //    tabsToHide.push("ResourceStatus");  // Hot Fixes Changes done as per ITK requirement
       //} 
    //    tabsToHide.push("PayRate");  // Hot Fixes Changes done as per ITK requirement
        tabsToHide.push("SensitiveDocuments");
        //tabsToHide.push("Comments");
      }
      // System role quick fixes 20-12-2019
      // if ((this.userTypes.includes(localConstant.techSpec.userTypes.RC) || this.userTypes.includes(localConstant.techSpec.userTypes.RM) || this.userTypes.includes(localConstant.userTypeList.Coordinator))) { //SystemRole based UserType relevant Quick Fixes 
      else{ //D730 (#5 issue Ref by 10-03-2020 ALM Doc )
        if(!this.payRateData())
        {
          tabsToHide.push("PayRate");
        }
        if(!this.sensitiveData())
        {
          tabsToHide.push("SensitiveDocuments");
        } 
        if(this.resourceInformationData()){
          tabsToHide.push("Comments");
        } //Added for ITK Def957(issue1.1) ref by 14-05-2020 ALM Doc
      } 
      // } 
    }
    if(this.props.currentPage === localConstant.techSpec.common.CREATE_PROFILE){
      tabsToHide.push("Documents");
    }
    const techSpecTabs = arrayUtil.negateFilter(this.props.techSpecDetailTabs, 'tabBody', tabsToHide);
    return techSpecTabs;
  };

  mandatoryFieldsDraftValidationCheck = () => {

    if (this.props.selectedProfileDetails) {
      const { TechnicalSpecialistInfo = {} } = this.props.selectedProfileDetails;
      if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.companyCode)) { //D363 CR Change
        this.errors.push(`${
          localConstant.techSpec.resourceStatus.RESOURCE_STATUS
        } - ${
          localConstant.techSpec.resourceStatus.COMPANY
        }`);
      }

      //First Name
      if (isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.firstName) 
      || isEmpty(this.props.selectedProfileDetails.TechnicalSpecialistInfo.firstName.trim())) {
        this.errors.push(`${
          localConstant.techSpec.contactInformation.CONTACT_INFORMATION
        } - ${
          localConstant.techSpec.contactInformation.FIRST_NAME
        }`);
      }
      if (!isEmpty(this.props.TechnicalSpecialistDocument)) {
        const issueDoc = [];
        this.props.TechnicalSpecialistDocument.map(document => {
          if (isEmpty(document.documentType)) {
            this.errors.push(`${
              localConstant.techSpec.Documents.DOCUMENT
            } - ${
              document.documentName
            } - ${
              localConstant.contract.Documents.SELECT_FILE_TYPE
            } `);
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
      });
      if(issueDoc && issueDoc.length > 0){
        let techSpecData = '';
        for (let i = 0; i < issueDoc.length; i++) {
          techSpecData = techSpecData +'\"' +issueDoc[i]+'\"'+ '; \r\n';
        }
        this.errors.push(`${ StringFormat(localConstant.project.documents.UPLOAD_ISSUE, techSpecData) }`);
      }
    }
//Added For Sanity Fix 206
      if(this.PayRateDetailValidation()) {
        this.props.techSpecDetailTabs && this.props.techSpecDetailTabs.forEach(iteratedValue => {
          if(iteratedValue.tabBody === 'PayRate' && iteratedValue.isCurrentTab) {
            this.callBackFuncs.reloadLineItemsOnSaveValidation();
          }
        });
        this.errors.push(`${
          localConstant.techSpec.PayRate.PAY_RATE_DETAILS
        } - ${
          localConstant.techSpec.PayRate.PAY_RATE_DETAILS_GRID_MANDATORY_FIELDS
        }`);
      } //Added For Sanity Fix 206

      if (this.errors.length > 0) {
        this.setState({
          errorList: this.errors
        });
        return false;
      }
      else
        return true;

    }

  }

  PayScheduleValidation() {
    let isValid = false;
    if(!isEmptyOrUndefine(this.props.selectedProfileDetails) 
        && !isEmptyOrUndefine(this.props.selectedProfileDetails.TechnicalSpecialistPaySchedule)) {      
      this.props.selectedProfileDetails.TechnicalSpecialistPaySchedule.filter(x=> x.recordStatus !== "D").forEach(row => {    //IGO QC 895                 
        if(required(row.payScheduleName)){            
          isValid = true;
        }
        if(required(row.payCurrency)){            
          isValid = true;
        }   
        if (!required(row.payScheduleName) && (this.props.selectedProfileDetails.TechnicalSpecialistPaySchedule.filter(x => x.id !== row.id && x.recordStatus !== "D" && x.payScheduleName.toLowerCase() === row.payScheduleName.toLowerCase()).length > 0)) {  //IGO QC 895                                                                       
          isValid = true;
        }     
      });
    } 
    return isValid;  
  }

  IsAnyPaySchedulesWithoughtPayRate() {
    let isValid = false;
    if(!isEmptyOrUndefine(this.props.selectedProfileDetails) 
        && !isEmptyOrUndefine(this.props.selectedProfileDetails.TechnicalSpecialistPaySchedule)) {  
          if(this.props.selectedProfileDetails.TechnicalSpecialistInfo.isDraft){ //D790
            this.props.selectedProfileDetails.TechnicalSpecialistPaySchedule.forEach(row => {                      
              if( row && row.recordStatus !== 'D' && this.props.selectedProfileDetails.TechnicalSpecialistPayRate && this.props.selectedProfileDetails.TechnicalSpecialistPayRate.filter(x => x.payScheduleName.toLowerCase() === row.payScheduleName.toLowerCase() && x.recordStatus !== 'D').length == 0 ) {                                                                      
                isValid = true;
                return false;
              }     
            });
          }  else {
            this.props.selectedProfileDetails.TechnicalSpecialistPaySchedule.forEach(row => {                      
              if(row &&  row.recordStatus !== 'D' && this.props.selectedProfileDetails.TechnicalSpecialistPayRate  && this.props.selectedProfileDetails.TechnicalSpecialistPayRate.filter(x=> (x.payScheduleId == row.id || x.payScheduleName.toLowerCase() === row.payScheduleName.toLowerCase() ) && x.recordStatus !== 'D' ).length == 0) {          //Chanegs for D384 ,1016#2                                                            
                isValid = true;
                return false;
              }     
            });
          }  
    } 
    return isValid;  
  }
 
  PayRateDetailValidation() {
    let isValid = false;
    if(!isEmptyOrUndefine(this.props.selectedProfileDetails) 
        && !isEmptyOrUndefine(this.props.selectedProfileDetails.TechnicalSpecialistPayRate)) {      
      this.props.selectedProfileDetails.TechnicalSpecialistPayRate.forEach(row => {                    
        if(required(row.expenseType)){            
          isValid = true;
        }
        if(requiredNumeric(row.rate) || row.rate === "NaN"){            
          isValid = true;
        }
        if(required(row.effectiveFrom)){            
          isValid = true;
        }        
        if(!required(row.effectiveFrom) && !this.isValidEffectiveDate(row.effectiveFrom)){            
          isValid = true;
        }
        if(required(row.effectiveTo)){            
          isValid = true;
        }
        if(!required(row.effectiveTo) && !this.isValidEffectiveDate(row.effectiveTo)){
          isValid = true;
        }
        if (!required(row.effectiveFrom) && !required(row.effectiveTo) && moment(row.effectiveFrom).isAfter(row.effectiveTo, 'day')) {             
          isValid = true;
        }
        
      });
    } 
    return isValid;       
  }

  isValidEffectiveDate(result) {
    // let isValid = true;
    // const arrDate = result.split(/(?:-| |\/)+/);
    // if(arrDate.length > 2) {
    //     if(isEmptyOrUndefine(arrDate[2])) {
    //         isValid = false;
    //     } else {
    //         if(!dateUtil.isUIValidDate(moment(result).format(localConstant.commonConstants.UI_DATE_FORMAT))) {
    //             isValid = false;
    //         }
    //     }
    // } else {            
    //     isValid = false;
    // }
    return true;
  }

  //SystemRole based UserType relevant Quick Fixes 
  filterSendToType =() =>{ 
    const sendToTypeHide =this.props.sendToInfo;
      if(!isEmptyOrUndefine(sendToTypeHide)){
        this.profileActionListWitOutRCRM = sendToTypeHide.filter(x => x.name !== localConstant.techSpec.common.SEND_TO_RC_RM);
        this.profileActionList = sendToTypeHide;
        //Changes For Sanity Def 116
        // if(this.props.isRCUserTypeCheck|| this.props.isRMUserTypeCheck){ //D910
        //   const profileAction=this.props.selectedProfileDetails.TechnicalSpecialistInfo && this.props.selectedProfileDetails.TechnicalSpecialistInfo.profileAction;
        //   if(profileAction !== localConstant.techSpec.common.SEND_TO_RC_RM){
        //     const value = this.props.sendToInfo.filter(x => x.name !== localConstant.techSpec.common.SEND_TO_RC_RM);
        //     sendToTypeHide = value;
        //   }
        // }
      } 
    return sendToTypeHide;
  } 
//SystemRole based UserType relevant Quick Fixes 

  render() {
    const { currentPage, isbtnDisable, selectedProfileDetails, sendToInfo } = this.props;
    const { TechnicalSpecialistInfo = {} } = selectedProfileDetails;
    // if (TechnicalSpecialistInfo) {
    //   if (TechnicalSpecialistInfo.profileStatus)
    //     this.technicalSpecialistTabData = this.filterTabDetails();
    // } 
    this.technicalSpecialistTabData = this.filterTabDetails();
    
   // let profileActionList = [];
    let interactionMode = this.props.interactionMode;
    if (this.props.pageMode === localConstant.commonConstants.VIEW && this.props.currentPage !== "Create Profile") {
      interactionMode = true;
    } 
    const isInEditMode = (this.props.interactionMode === false && this.props.currentPage === "Edit/View Resource");
    const isInViewMode = (this.props.pageMode === localConstant.commonConstants.VIEW) ? true : false && this.props.currentPage !== "Create Profile";
    const isInCreateMode = (this.props.currentPage === "Create Profile");
    let showBtn1 = true;
    let showBtn2 = false;
    if (isInCreateMode) {
      showBtn1 = true;
      showBtn2 = true;
    }
    else if (isInViewMode) {
      showBtn1 = false;
      showBtn2 = false;
    }
    else if (isInEditMode) {
      showBtn1 = true;
      showBtn2 = true;
    }
     //SystemRole based UserType relevant Quick Fixes 
    //  profileActionList = this.filterSendToType();
    let techSpecButtons= [
      {
        name: localConstant.commonConstants.SAVE,
        clickHandler: (e) => this.techSpecSaveClickHandler(e),
        className: "btn-small mr-0 ml-2",
        permissions: [
          activitycode.NEW, activitycode.MODIFY,activitycode.LEVEL_0_MODIFY, activitycode.LEVEL_1_MODIFY, activitycode.LEVEL_2_MODIFY,
          activitycode.EDIT_SENSITIVE_DOC, activitycode.EDIT_PAYRATE, activitycode.EDIT_TM
        ],
        isbtnDisable: isbtnDisable,
        showBtn: showBtn1
      },
      {
        name: localConstant.commonConstants.SAVE_AS_DRAFT,
        clickHandler: () => this.techSpecSaveAsDraftClickHandler,
        className: "btn-small mr-0 ml-2 ",
        permissions: [
          activitycode.NEW, activitycode.MODIFY,activitycode.LEVEL_0_MODIFY, activitycode.LEVEL_1_MODIFY, activitycode.LEVEL_2_MODIFY,
          activitycode.EDIT_SENSITIVE_DOC, activitycode.EDIT_PAYRATE, activitycode.EDIT_TM
        ],
        isbtnDisable: this.props.isbtnDisableDraft,
        showBtn: showBtn1
      }, {
        name: localConstant.commonConstants.REFRESHCANCEL,
        clickHandler: (e) => this.techSpecCancelClickHandler(e),
        className: "btn-small mr-0 ml-2 modal-trigger",
        permissions: [
          activitycode.NEW, activitycode.MODIFY,activitycode.LEVEL_0_MODIFY, activitycode.LEVEL_1_MODIFY, activitycode.LEVEL_2_MODIFY,
          activitycode.EDIT_SENSITIVE_DOC, activitycode.EDIT_PAYRATE, activitycode.EDIT_TM
        ],
        isbtnDisable: isbtnDisable,
        showBtn: showBtn1
      },
      {
        name: localConstant.commonConstants.DELETE,
        clickHandler: () => this.techSpecDraftDeleteClickHandler,
        className: "btn-small btn-primary mr-0 ml-2 dangerBtn modal-trigger waves-effect",
        permissions: [
          activitycode.DELETE,
        ],
        isbtnDisable: false,
      },
      {
        name: localConstant.commonConstants.EXPORT_CV,  //D664 #14
        clickHandler: this.selectionPopUp,//this.techSpecExportClickHandler,
        className: "btn-small mr-0 ml-2",
        isbtnDisable: false,
        showBtn: (this.props.currentPage === localConstant.techSpec.common.Edit_View_Technical_Specialist) ? true : false
      },
      {
        name: localConstant.commonConstants.EXPORT_CHEVRON_CV,
        clickHandler: (e) => this.techSpecChevronExportClickHandler(e),
        className: "btn-small mr-0 ml-2",
        isbtnDisable: false,
        showBtn: (this.props.currentPage === localConstant.techSpec.common.Edit_View_Technical_Specialist) ? true : false
      }
      // {
      //   name: localConstant.commonConstants.EXPORT,
      //   clickHandler: () => this.techSpecExportClickHandler,
      //   className: "btn-small mr-0 ml-2",
      //   permissions: [ activitycode.NEW, activitycode.MODIFY, activitycode.LEVEL_1_MODIFY, activitycode.LEVEL_2_MODIFY,
      //   activitycode.EDIT_SENSITIVE_DOC, activitycode.EDIT_PAYRATE, activitycode.EDIT_TM ],
      //   isbtnDisable: false,
      //   showBtn: (this.props.currentPage === localConstant.techSpec.common.Edit_View_Technical_Specialist) ? true : false
      // }
    ];
    if(!this.props.isDraft){  //D380
        techSpecButtons= arrayUtil.negateFilter(techSpecButtons, 'name', [ localConstant.commonConstants.DELETE ]);
    } 
    return (
      <Fragment>
        {this.state.errorList.length > 0 ?
          <Modal title={localConstant.commonConstants.CHECK_MANDATORY_FIELD}
            titleClassName="chargeTypeOption"
            modalContentClass="extranetModalContent"
            modalClass="ApprovelModal"
            modalId="errorListPopup"
            formId="errorListForm"
            buttons={this.errorListButton}
            isShowModal={true}>
            <ErrorList errors={this.state.errorList} />
          </Modal> : null
        }
        <SaveBar codeLabel={ localConstant.resourceSearch.EPIN }
          resoureceLableName={ localConstant.resourceSearch.NAME}
          codeValue={ ( this.props.epin > 0 ? this.props.epin  : (isUndefined(this.props.selectedEpinNo)? '' : this.props.selectedEpinNo )) + ( this.props.isDraft ?' (Draft)' : '' ) }
          currentMenu="Resource "
          currentSubMenu={this.props.currentPage || "Create New Profile"}
          isbtnDisable={isbtnDisable}
          isbtnDisableDraft={this.props.isbtnDisableDraft || ( this.props.isTSUserTypeCheck && this.props.oldProfileAction === localConstant.techSpec.common.SEND_TO_TS ? false : this.props.isTSUserTypeCheck ) } //def 930
          sendtoInfo={this.profileActionList}
          sendtoInfoWthOutRCRM={this.profileActionListWitOutRCRM}
          sendRCRM={this.props.oldProfileAction === localConstant.techSpec.common.SEND_TO_TM || (this.props.oldProfileAction === localConstant.techSpec.common.SEND_TO_RC_RM && this.props.isDraft) || (this.props.oldProfileAction === localConstant.techSpec.common.CREATE_UPDATE_PROFILE && TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM) || (this.props.oldProfileAction === localConstant.techSpec.common.SEND_TO_RC_RM && TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM) ||this.props.isTSUserTypeCheck}
          ShowSendtoInfo={(this.props.isRCUserTypeCheck ? !isEmpty(this.props.oldProfileAction) ? ((this.props.oldProfileAction === localConstant.techSpec.common.SEND_TO_RC_RM && isEmptyOrUndefine(this.props.myTaskRefCode))|| (this.props.oldProfileAction === localConstant.techSpec.common.SEND_TO_TM && !isEmptyOrUndefine(this.props.myTaskRefCode)) || this.props.oldProfileAction === localConstant.techSpec.common.SEND_TO_TS || this.props.oldProfileAction === localConstant.techSpec.common.CREATE_UPDATE_PROFILE) : true :false)}
          listOfTM={this.props.listOfTM}
          listOfRC={this.props.listOfRC}       // Added for D946 CR 
          showListOfRC={(!isEmpty(TechnicalSpecialistInfo) && (TechnicalSpecialistInfo.assignedToUser == null && TechnicalSpecialistInfo.assignedByUser == null) && (this.props.isTSUserTypeCheck || this.props.isTMUserTypeCheck) && this.props.currentPage === localConstant.techSpec.common.EDIT_VIEW_TECHSPEC && TechnicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM) ? true : false }  // Added for D946 CR 
          showListOfTM={this.state.showListOfTM}
          techSpecInfo={TechnicalSpecialistInfo}
          sendToOnChangeHandler={this.sendToOnChangeHandler}
          activities={this.props.activities}
          interactionMode={interactionMode}
          technicalManagerOnChangeHandler={this.technicalManagerOnChangeHandler}
          buttons={!isEmpty(TechnicalSpecialistInfo) && TechnicalSpecialistInfo.taxonomyIsAcceptRequired && TechnicalSpecialistInfo.isPreviouslyProfileMadeActive ? [] :techSpecButtons} 
        />
        <div className="row ml-2 mb-0">
          <div className="col s12 pl-0 pr-2 verticalTabs Grm">
            <CustomTabs
              tabsList={this.technicalSpecialistTabData}
              moduleName="techSpec"
              rootModule="GRM"
              currentPage={currentPage}
              interactionMode={interactionMode}
              activities={this.props.activities}
              techManager={this.technicalManager()}
              onSelect={scrollToTop}
              callBackFuncs = {this.callBackFuncs}
            >
            </CustomTabs>
          </div>
        </div>
        {this.state.isSelectionPopUpOpen ?
                        <Modal title={localConstant.resourceSearch.CV_SECTION_MODAL}
                            modalId="SelectionPopUpModal"
                            formId="SelectionPopUpForm"
                            modalClass="selectionPopUpModal"
                            buttons={this.SelectionPopUpButtons}
                            isShowModal={this.state.isSelectionPopUpOpen}>
                            <SelectionPopUpModal 
                            optionList={localConstant.resourceSearch.optionValueList} 
                            headerData={this.headerData.cvOptionsHeader} 
                            gridRef={ref => { this.selectGridChild = ref; }} 
                            />
                        </Modal>
                        : null
        }  
      </Fragment>
    );
  }
}

export default TechSpecDetails;