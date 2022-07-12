import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData.js';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, isEmpty, formInputChangeHandler, bindAction, mergeobjects, isUndefined } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import Modal from '../../../../common/baseComponents/modal';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import { configuration } from '../../../../appConfig';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import CustomModal from '../../../../common/baseComponents/customModal';
import UploadDocument from '../../../../common/baseComponents/uploadDocument';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { applicationConstants } from '../../../../constants/appConstants';
import {  levelSpecificActivities } from '../../../../constants/securityConstant';
import { isEditable,isViewable } from '../../../../common/selector';
import moment from 'moment';
const localConstant = getlocalizeData();
const SensitiveDetailsModalPopUp = (props) => {
    return (
        <Fragment>
            <div className="col s6 p-0" >
                <CustomInput
                    hasLabel={true}
                    divClassName='col pl-0 pr-0'
                    label={localConstant.techSpec.sensitiveDocuments.DOCUMENT_TYPE}
                    type='select'
                    colSize='s12'
                    inputClass="customInputs"
                    optionsList={props.fetchDocumentTypeMasterData}
                    labelClass="mandate"
                    optionName="name"
                    optionValue="name"
                    onSelectChange={props.onChangeHandler}
                    name='documentType'
                    defaultValue={props.editedRowData.documentType} 
                    disabled={props.disableField} />
                <CustomInput
                    hasLabel={true}
                    divClassName='col pl-0 pr-0'
                    label={localConstant.techSpec.sensitiveDocuments.DOCUMENT_NAME}
                    type='text'
                    colSize='s12'
                    inputClass="customInputs"
                    onValueChange={props.onChangeHandler}
                    // name='documentName'
                    // Sensitive Document Changes
                    name='documentTitle' 
                    disabled={props.disableField}
                    // defaultValue={props.editedRowData && props.editedRowData.documentName}
                    // Sensitive Document Changes
                    defaultValue={props.editedRowData && props.editedRowData.documentTitle} /> 

                <CustomInput
                    hasLabel={true}
                    divClassName='col pl-0 pr-0 mt-1'
                    label={localConstant.techSpec.sensitiveDocuments.DATE_ADDED}
                    type='date'
                    colSize='s12'
                    inputClass="customInputs"
                    selectedDate={props.dateAdded}
                    onDateChange={props.fetchDateAdded}
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    name='createdOn'
                    onDatePickBlur={props.handleDateBlur}
                    defaultValue={props.editedRowData && props.editedRowData.createdOn}
                    disabled={true} //D223 #1issue(HP ALM 14-02-2020 Doc) 
                />
                <div className="col s12 pl-0 pr-0 " >

                    <UploadDocument
                        //Mode={props.editStampDetails.documents[0].recordStatus === 'D' ? !props.isResourceStatusEdit  : props.isResourceStatusEdit }
                        defaultValue={props.editedRowData && props.editedRowData.documentName ? props.editedRowData.documentName : ''}
                        className="col s12 pl-0 pr-0"
                        label={localConstant.techSpec.sensitiveDocuments.DOCUMENT_ATTACHED}
                        cancelUploadedDocument={props.editUploadDocument}
                        editDocDetails={props.editedRowData}
                        gridName={localConstant.techSpec.common.TECHNICALSPECIALISTDOCUMENTS}
                        disabled={props.disableField}
                    />
                </div>

            </div>
            <div className="col s6 p-0">
                <CustomInput
                    hasLabel={true}
                    divClassName='col pr-0'
                    label={localConstant.techSpec.sensitiveDocuments.EXPIRY}
                    type='date'
                    colSize='s12'
                    inputClass="customInputs"
                    selectedDate={props.expiry}
                    onDateChange={props.fetchExpiryDate}
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    name='expiryDate'
                    onDatePickBlur={props.handleDateBlur}
                    disabled={props.disableField} />
                <CustomInput
                    hasLabel={true}
                    divClassName='col pr-0'
                    label={localConstant.techSpec.sensitiveDocuments.COMMENTS}
                    type='textarea'
                    colSize='s12'
                    inputClass="customInputs"
                    onValueChange={props.onChangeHandler}
                    name='comments'
                    defaultValue={props.editedRowData.comments}
                    maxLength={4000}
                    readOnly={props.disableField} />
            </div>
        </Fragment >
    );
};
class SensitiveDocuments extends Component {
    constructor(props) {
        super(props);
        this.state = {
            dateAdded: moment(),
            expiry: '',
            inValidDateFormat: false,
            isSensitiveDocumentShowModal: false,
            isSensitiveDocumentsEditModal: false,
            uploadSensitiveDocument: false,
            emptyDocType:false,
        };
        this.updatedData = {};
        this.editedRowData = {};   
        this.sensitiveDocPreviousvalue = {}; 
        //Add Buttons
        this.newSensitiveDocumentAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.sensitiveDocumentsCancelModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.addSensitiveDetails,
                btnClass: "btn-small ",
                showbtn: !this.fieldDisableHandler()
            }
        ];
        //Edit Buttons
        this.newSensitiveDocumentEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.sensitiveDocumentsCancelModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.updateSensitiveDetails,
                btnClass: "btn-small",
                showbtn: !this.fieldDisableHandler()
            }
        ];
          //SystemRole based UserType relevant Quick Fixes 
          const functionRefs = {};
          functionRefs["enableEditColumn"] = this.enableEditColumn;
          this.headerData = HeaderData(functionRefs);
      }
      enableEditColumn = (e) => {//SystemRole based UserType relevant Quick Fixes 
        const { selectedProfileDetails,assignedToUser } = this.props;
         if (this.props.isTMUserTypeCheck && !isViewable({ activities: this.props.activities, levelType: 'LEVEL_3',viewActivityLevels: levelSpecificActivities.viewActivitiesTM }) && !this.props.isRCUserTypeCheck) {
            return true;
            }//D1374
         if (selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM){
             return true;
         }
         if ((selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM) && 
         (assignedToUser !== this.props.loggedInUser)){
            return false;
        }
          return  !isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel1 });//D1374 Failed on 29-10-2020
      }
  
    cancelUploadedDocument = () => {
        this.setState({ disableDocument: true });

    }
    //Show modal
    sensitiveDocumentsShowModal = (e) => {
        this.setState({ expiry: '' });
        this.setState({ dateAdded: moment() });
        this.setState((state) => {
            return {
                isSensitiveDocumentShowModal: true,
                isSensitiveDocumentsEditModal: false
            };
        });
        this.editedRowData = {};
    }
    //Hiding the modal
    sensitiveDocumentsHideModal = () => {
        this.setState((state) => {
            return {
                isSensitiveDocumentShowModal: false,
                isSensitiveDocumentsEditModal: false
            };
        });
        this.editedRowData = {};
        this.updatedData = {};
    }
    sensitiveDocumentsCancelModal = (e) => {
        e.preventDefault();
        this.setState((state) => {
            return {
                isSensitiveDocumentShowModal: false,
                isSensitiveDocumentsEditModal: false
            };
        });
        this.onclickModalCancel(); //Added for D223 Popup cancel issue
        this.editedRowData = {};
        this.updatedData = {};
        this.props.actions.RemoveDocUniqueCode();
    }
    onclickModalCancel=()=>{
        if(!isEmpty(this.editedRowData)){
            const RevertDeletedDocument=this.sensitiveDocPreviousvalue; // add old values
            if(!isEmpty(this.props.sensitiveDetails) && !isEmpty(this.sensitiveDocPreviousvalue))
            { 
                const filteredResources = this.props.sensitiveDetails.filter(x => !(this.sensitiveDocPreviousvalue.some(x1 => x1.documentUniqueName === x.documentUniqueName)));
                !isEmpty(filteredResources) && filteredResources.forEach(doc=>{
                    doc.recordStatus="D";
                    RevertDeletedDocument.push(doc);  
                });
                this.sensitiveDocPreviousvalue={};
            }  
            this.props.actions.RevertDeletedSensitiveDocument(RevertDeletedDocument);
            this.gridChild.refreshGrid();
        } 
    }

    editRowHandler = (data) => {
        this.setState((state) => {
            return {
                isSensitiveDocumentShowModal: !state.isSensitiveDocumentShowModal,
                isSensitiveDocumentsEditModal: true
            };
        });
        this.editedRowData = data; 
        this.sensitiveDocPreviousvalue=this.props.sensitiveDetails;
        this.gridDateHandler();
        this.uploadDocument();
    }
    uploadDocument() {
        if (this.editedRowData.documentUniqueName === '' || this.editedRowData.documentUniqueName === undefined) {
            this.setState({ uploadSensitiveDocument: true });
        }
        else
            this.setState({ uploadSensitiveDocument: false });
    }
    gridDateHandler() {
        if (this.editedRowData) {
          
            if (this.editedRowData.expiryDate) {
                this.setState({
                    expiry: moment(this.editedRowData.expiryDate)
                });
               
            } else if(this.editedRowData.createdOn){  //D837 (Ref ALM Doc on 09-04-2020 Issue#2)
                this.setState({
                    dateAdded: moment(this.editedRowData.createdOn),
                });
            }
            else {
                this.setState({
                    // dateAdded: '',
                    expiry: ''
                });
            }
        } else {
            this.setState({
                dateAdded: '',
                expiry: ''
            });
        }
    }
    fetchDateAdded = (date) => {
        this.setState({ dateAdded: date });
        this.updatedData.createdOn = this.state.dateAdded;
    }
    fetchExpiryDate = (date) => {
        this.setState({ expiry: date });
        this.updatedData.expiryDate = this.state.expiry;
    }
    onChangeHandler = (e) => {
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
        if(this.updatedData.documentType==="")
        {
            this.setState( { emptyDocType: true } );
        }
        else{
            this.setState( { emptyDocType: false } );
        }
    }

        //Date Range Validator
        dateRangeValidator = (from, to) => {
            let isInValidDate = false;
            if (to !== "" && to !== null) {
                if (from > to) {
                    isInValidDate = true;
                    IntertekToaster(localConstant.commonConstants.SENSITIVE_DOC_INVALID_DATE_RANGE, 'warningToast');
                } else if(from == to){
                    isInValidDate = true;
                    IntertekToaster(localConstant.commonConstants.SENSITIVE_DOC_INVALID_DATE_EQUAL_VALIDATOR, 'warningToast');
                }
            }
            return isInValidDate;
        }

    addSensitiveDetails = (e) => {
        e.preventDefault();
        let { toDate, createdOn } = "";
        const newlyCreatedRecords = [];
        const docType = this.updatedData.documentType;
        const comments = this.updatedData.comments;
        if (this.state.expiry !== "" && this.state.expiry !== null) {
            toDate = this.state.expiry.format(localConstant.techSpec.common.DATE_FORMAT);
        }
        if (this.state.dateAdded !== "" && this.state.dateAdded !== null) {
            createdOn = this.state.dateAdded.format(localConstant.techSpec.common.DATE_FORMAT);
        }

        if (isEmpty(this.updatedData.documentType)) {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Document Type', 'warningToast contractDocFileTypeReq');
        }
        else {
            if (isEmpty(this.props.docUniqueCode)) {
                IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' a Document', 'warningToast contractNoFileSelectedReq');
            } else {
                if (!this.dateRangeValidator(createdOn, toDate)) { //D687 (As per mail confirmation on 16-03-2020)
                    this.updatedData["documentName"] = this.props.docUniqueCode.documentName;
                    this.updatedData["documentType"] = docType;
                    this.updatedData["createdOn"] = createdOn;
                    this.updatedData["expiryDate"] = toDate;
                    this.updatedData["comments"] = comments;
                    this.updatedData["recordStatus"] = this.props.docUniqueCode.recordStatus;
                    this.updatedData["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                    this.updatedData["selectedEpinNo"] = this.props.epin !== 0 ? this.props.epin : 0;
                    this.updatedData["documentUniqueName"] = this.props.docUniqueCode.documentUniqueName;
                    this.updatedData["modifiedBy"] = this.props.docUniqueCode.modifiedBy;
                    this.updatedData["moduleCode"] = this.props.docUniqueCode.moduleCode;
                    this.updatedData["moduleRefCode"] = this.props.docUniqueCode.moduleRefCode;
                    this.updatedData["subModuleRefCode"] = 0;
                    this.updatedData["status"] = this.props.docUniqueCode.status;
                    this.updatedData["newlyUploadedDoc"] = true;  //D223 changes 
                    newlyCreatedRecords.push(this.updatedData);
                    this.updatedData = {};
                    if (newlyCreatedRecords.length > 0) {
                        this.props.actions.AddSensitiveDetails(newlyCreatedRecords);
                    }
                    this.sensitiveDocumentsHideModal();
                    this.props.actions.StoreDocUniqueCode([]);
                }
            }
        }
    }
    //Modal pop up for delete
    deleteSensitiveDetails = () => {
        const selectedRecords = this.gridChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.SENSITIVE_DOCUMENTS_DELETE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                isOpen: 'true',
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSelected,
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
        else IntertekToaster(localConstant.techSpec.common.REQUIRED_DELETE, 'warningToast');
    }
    //Deleting the grid detail
    deleteSelected = () => {
        const selectedData = this.gridChild.getSelectedRows();
        this.gridChild.removeSelectedRows(selectedData);
        this.props.actions.DeleteSensitiveDetails(selectedData);
        this.props.actions.HideModal();
    }
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }
    //Handling date
    handleDateBlur = (e) => {
        if (e && e.target !== undefined) {
            this.setState({ inValidDateFormat: false });
            if (e.target.value !== "" && e.target.value !== null) {
                if (e && e.target !== undefined) {
                    const isValid = dateUtil.isUIValidDate(e.target.value);
                    if (!isValid) {
                        IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                        this.setState({ inValidDateFormat: true });
                    }
                    else this.setState({ inValidDateFormat: false });
                }
            }
        }
    }
    //Updating the edited data to the grid
    updateSensitiveDetails = async (e) => {
        e.preventDefault();
        let {
            toDate,
            createdOn
        } = "";
        if (this.props.IsRemoveDocument) {
            
            const response = await this.props.actions.UpdateSensitiveDetails(this.updatedData, this.editedRowData);
            // .then(response => {
            if (response && Array.isArray(response) && response.length > 0) {
                response.forEach(doc => {
                    if (this.editedRowData.id === doc.id) {
                        this.editedRowData = doc;
                    }
                });
            }
        } else {
            if (this.props.sensitiveDetails) {
                this.props.sensitiveDetails.forEach(doc => {
                    if (this.editedRowData.id === doc.id) {
                        this.editedRowData = doc;
                    }
                });
            }
        }
        if (this.state.expiry !== "" && this.state.expiry !== null)
            toDate = this.state.expiry.format(localConstant.techSpec.common.DATE_FORMAT);
        if (this.state.dateAdded !== "" && this.state.dateAdded !== null)
            createdOn = this.state.dateAdded.format(localConstant.techSpec.common.DATE_FORMAT);
        // if ((this.updatedData.documentType === "" || this.updatedData.documentType === undefined || this.updatedData.documentType === null) &&
        //     (this.editedRowData.documentType === "" || this.editedRowData.documentType === undefined || this.editedRowData.documentType === null)) {
        //     IntertekToaster(localConstant.project.documents.SELECT_FILE_TYPE, 'warningToast contractFileTypeReq');
        // }
         if( this.state.emptyDocType)
        {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Document Type', 'warningToast contractDocFileTypeReq');
        }
         else {
            if (this.editedRowData.documentName === null || this.editedRowData.documentName === undefined || this.editedRowData.documentName === "") {
                IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' a Document', 'warningToast contractNoFileSelectedReq');
            } else {
                if (!this.dateRangeValidator(createdOn, toDate)) { 
                this.updatedData["createdOn"] = createdOn;
                this.updatedData["expiryDate"] = toDate;
                this.updatedData["recordStatus"] = "M";
                this.props.actions.UpdateSensitiveDetails(this.updatedData, this.editedRowData);
                this.sensitiveDocumentsHideModal();
                this.updatedData = {};
                }
            }

        }

        // });

    }
    editUploadDocument = (e) => {
        const editedArray = [];
        const actualData = Object.assign({}, this.editedRowData);
        actualData.recordStatus = "D";
        actualData.id = 0;
        editedArray.push(actualData);
        this.props.actions.AddSensitiveDetails(editedArray);
          
        // Sensitive Document Changes
       // this.updatedData.documentTitle = null;
        this.updatedData.documentName = null;
        this.updatedData.documentSize = null;
        this.updatedData.status = null;
        // this.updatedData.documentUniqueName = null; //Changes for Scenario defect in Sensitive Document
        this.updatedData.recordStatus = "M";
        //this.props.actions.UpdateSensitiveDetails(this.updatedData, this.editedRowData);
        this.editedRowData = mergeobjects(this.editedRowData, this.updatedData);
        this.props.actions.IsRemoveDocument(true);
         this.updatedData = {};
    }

    /** Field Disable Handler */
    fieldDisableHandler = () => {  
        if ( this.props.currentPage === "Edit/View Resource" && this.props.techManager ) {
            return !(isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel3 }));
        }
        else
        {
            return !(isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel3 }));
        } 
    };

    render() {
        const { sensitiveDetails, fetchDocumentTypeMasterData, interactionMode, selectedProfileDetails } = this.props;
        const doctypeFilter = [
            "TS_EducationQualification", "TS_InternalTraining", "TS_Competency", "TS_CustomerApproval", "TS_Certificate", "TS_Training", "TS_Stamp", "TS_ProfessionalAfiliation", "TS_CertVerification", "TS_TrainingVerification"
        ];  // To avoid showing draft uploaded files
        const Sensitivdoc = sensitiveDetails && sensitiveDetails.filter(x => x.recordStatus !== 'D' && doctypeFilter.includes(x.documentType)==false );
        bindAction(this.headerData.SensitiveDocumentHeader, "EditColumn", this.editRowHandler); 
        const disableField = this.fieldDisableHandler();
        return (
            <Fragment>
                <CustomModal />
                {this.state.isSensitiveDocumentShowModal &&
                    <Modal
                        modalClass="techSpecModal"
                        title={this.state.isSensitiveDocumentsEditModal ? localConstant.techSpec.sensitiveDocuments.EDIT_SENSITIVE_DOCUMENTS :
                            localConstant.techSpec.sensitiveDocuments.ADD_SENSITIVE_DOCUMENTS}
                        buttons={this.state.isSensitiveDocumentsEditModal ? this.newSensitiveDocumentEditButtons : this.newSensitiveDocumentAddButtons}
                        isShowModal={this.state.isSensitiveDocumentShowModal}>
                        <SensitiveDetailsModalPopUp
                            dateAdded={this.state.dateAdded} fetchDateAdded={this.fetchDateAdded}
                            expiry={this.state.expiry } fetchExpiryDate={this.fetchExpiryDate}
                            handleDateBlur={this.handleDateBlur}
                            sensitiveDetails={sensitiveDetails}
                            editedRowData={this.editedRowData}
                            onChangeHandler={this.onChangeHandler}
                            onEditClick={this.addSensitiveDetails}
                            uploadSensitiveDocument={this.state.uploadSensitiveDocument}
                            editUploadDocument={this.editUploadDocument}
                            isSensitiveDocumentsEditModal={this.state.isSensitiveDocumentsEditModal}
                            fetchDocumentTypeMasterData={fetchDocumentTypeMasterData.filter(x => x.moduleName === "Technical Specialist Sensitive Documents")}
                            disableField={disableField} />
                    </Modal>}
                <div className="customCard">
                    <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.sensitiveDocuments.SENSITIVE_DOCUMENTS} colSize="s12" >
                         <ReactGrid gridRowData={Sensitivdoc && Sensitivdoc.filter((x => x.recordStatus !== "D" || x.subModuleRefCode === 0))}
                                gridColData={this.headerData.SensitiveDocumentHeader} onRef={ref => { this.gridChild = ref; }} paginationPrefixId={localConstant.paginationPrefixIds.techSpecSensitiveGridId} />
                      
                        {this.props.pageMode !== localConstant.commonConstants.VIEW && <div className="right-align mt-2 add-text">
                            <button onClick={this.sensitiveDocumentsShowModal} className="waves-effect btn-small" disabled={disableField || interactionMode  || (selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM) }>{localConstant.commonConstants.ADD}</button>
                            <button onClick={this.deleteSensitiveDetails} 
                            className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn"
                                disabled={(sensitiveDetails && sensitiveDetails.filter(x => x.recordStatus !== "D").length <= 0) || interactionMode || disableField || (selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM)  ? true : false}>
                                {localConstant.commonConstants.DELETE}</button>
                        </div> }
                    </CardPanel>
                </div>
            </Fragment>
        );
    }
};
export default SensitiveDocuments;