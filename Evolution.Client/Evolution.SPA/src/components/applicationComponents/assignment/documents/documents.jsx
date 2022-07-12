import React, { Component, Fragment } from 'react';
import { HeaderData, ChildHeaderData,VisitDocumentHeaderData,TimesheetDocumentHeaderData } from './headerData';
import RelatedDocuments from '../../../documents/relatedDocuments';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import Panel from '../../../../common/baseComponents/panel';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { getlocalizeData, bindAction, isEmpty,isUndefined, formInputChangeHandler,uploadedDocumentCheck } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import CustomModal from '../../../../common/baseComponents/customModal';
import Modal from '../../../../common/baseComponents/modal';
import { configuration } from '../../../../appConfig';
import  AssignmentDocumentMenuButtons  from '../../../documents/documents';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import PropTypes from 'prop-types';
import { MultipleDownload } from '../../../documents/documentsMultidownload';
const localConstant = getlocalizeData();
const ModalPopupAddView = (props) => {
    return (
        <Fragment>
            <CustomInput hasLabel={true}
                label={localConstant.assignments.SELECT_FILE_TYPE}
                divClassName='col pl-0 mb-4'
                colSize="s6"
                type='select'
                labelClass="mandate"
                //required={true}
                className="customInputs browser-default"
                optionsList={props.assignmentDocumentTypeData}
                optionName='name'
                optionValue="name"
                onSelectChange={props.handlerChange}
                name="documentType"
                id="documentType"

            />
            <div className="col s12 pl-0">
                <div className="file-field">
                    <div className="btn">
                        <i className="zmdi zmdi-upload zmdi-hc-lg"></i>
                        <input id="uploadFiles" type="file" accept={props.supportFileType.allowedFileFormats} multiple required />
                    </div>
                    <div className="file-path-wrapper pl-0">
                        <input className="browser-default file-path validate"
                            placeholder="Upload multiple files" />
                    </div>
                </div>
            </div>

            {/* <div className="row">
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.modalConstant.VISIBLE_TO_CUSTOMER}
                    isSwitchLabel={true}
                    switchName="isVisibleToCustomer"
                    id="isVisibleToCustomer"
                    colSize="s3 m4 mt-4"
                    checkedStatus={props.isVisibleToCustomer}
                    onChangeToggle={props.handlerChange}
                />
            </div> */}
        </Fragment>
    );
};

const ModalPopupEditView = (props) => {
    return (
        <Fragment>
            <CustomInput hasLabel={true}
                label={localConstant.assignments.SELECT_FILE_TYPE}
                divClassName='col pl-0'
                colSize="s6"
                type='select'
                //required={true}
                className="customInputs browser-default"
                optionsList={props.assignmentDocumentTypeData}
                optionName='name'
                optionValue="name"
                onSelectChange={props.handlerChange}
                name="documentType"
                id="documentType"
                defaultValue={props.eidtedAssignmentDocuments.documentType}
            />
            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.assignments.DOCUMENT_NAME}
                type='text'
                colSize='s6'
                inputClass="customInputs"
                labelClass="mandate"
                name="documentName"
                readOnly={true}
                // disabled={true}
                defaultValue={props.eidtedAssignmentDocuments.documentName}
            />

            {/* <div className="row">
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.modalConstant.VISIBLE_TO_CUSTOMER}
                    isSwitchLabel={true}
                    switchName="isVisibleToCustomer"
                    id="isVisibleToCustomerEdit"
                    colSize="s3 m4 mt-4"
                    checkedStatus={props.eidtedAssignmentDocuments.isVisibleToCustomer}
                    onChangeToggle={props.handlerChange}
                />
            </div> */}
        </Fragment>
    );
};
class AssignmentDocuments extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isDocumentModalOpen: false,
            isDocumentModelAddView: true,
            isPanelOpen: false,
            isProjectPanelOpen:false,
            isSupplierPanelOpen:false,
            isVisitPanelOpen:false,
            isTimesheetPanelOpen:false
        };
        this.updatedData = {};
        this.filesToDisplay = [];
        this.uploadButtonDisable = false;
        this.copiedDocumentDetails = [];
        this.functionRef={};
        this.functionRef['isDisable']=(this.props.pageMode === localConstant.commonConstants.VIEW);
        this.headerData=HeaderData(this.functionRef);       
    };
    editRowHandler = (data) => {
        this.setState({
            isDocumentModalOpen: true,
            isDocumentModelAddView: false,
            isbtnDisable: false
        });
        this.editedRowData = data;
    }
    componentDidMount() {

        // this.props.actions.FetchAssignmentDocumentTypes();
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        if (tabInfo && tabInfo.componentRenderCount === 0) {
            this.props.actions.FetchAssignmentContractDocuments();
            this.props.actions.FetchAssignmentProjectDocuments();
            if (this.props.isVisitAssignment) {
                if (this.props.assignmentSupplierPurchaseOrderNumber) {
                    this.props.actions.FetchAssignmentSupplierPODocuments();
                }
                this.props.actions.FetchAssignmentVisitDocuments();
            }
            if(this.props.isTimesheetAssignment){
                this.props.actions.FetchAssignmentTimesheetDocuments();
            }
        }
        // this.props.actions.FetchAssignmentSupplierPODocuments();
        // this.props.actions.FetchAssignmentVisits()
        //     .then(response => {
        //         if (response) {
        //             //this.props.actions.FetchAssignmentVisitDocuments();
        //         }
        //     });

        if(!(this.props.interactionMode && this.props.pageMode === localConstant.commonConstants.VIEW)){
            //Drag and Drop File Upload
            this.dropArea = document.getElementById("drop-area");
            if (this.dropArea) {
            // Prevent default drag behaviors
            [ 'dragenter', 'dragover', 'dragleave', 'drop' ].forEach(eventName => {
                this.dropArea.addEventListener(eventName, this.preventDefaults, false);   
                document.body.addEventListener(eventName, this.preventDefaults, false);
            });
            
            // Highlight drop area when item is dragged over it
            [ 'dragenter', 'dragover' ].forEach(eventName => {
                this.dropArea.addEventListener(eventName, this.highlight, false);
            });
            
            [ 'dragleave', 'drop' ].forEach(eventName => {
                this.dropArea.addEventListener(eventName, this.unhighlight, false);
            });          
            // Handle dropped files
            this.dropArea.addEventListener('drop', this.handleDrop, false);
                    }
        } 
    }
    preventDefaults=(e)=> {
        e.preventDefault();
        e.stopPropagation();
      }
      
    highlight=(e)=> {
        this.dropArea.classList.add('highlight');
      }
      
    unhighlight=(e)=> {
        this.dropArea.classList.remove('highlight');
      }
    handleDrop=(e) =>{
        const dt = e.dataTransfer;
        const files = dt.files;
        const documentFile = Array.from(files);   
        this.fileupload(documentFile);
        this.unhighlight(e);
    }  

    fileupload=(files)=>{       
            let date = new Date();
            date = dateUtil.postDateFormat(date, '-');       
            const docType = this.updatedData.documentType;
            const documentFile = files;
            const queueDocs = this.props.documentrowData.find(doc => doc.isUploaded === false);
            const isQueuePending = queueDocs ? true : false;
            if (documentFile.length > 0) {
                const newlyCreatedRecords = [];
                const filesToBeUpload = [];
                const failureFiles = [];
                const failureFormat=[];
                const documentUniqueName = [];
                let createdNewRecords =[];
                const customerVisibleStatus = this.updatedData.isVisibleToCustomer ? this.updatedData.isVisibleToCustomer : false;;
                let visibleToTSStatus = false;
                const assignmentDocType=this.props.assignmentDocumentTypeData;
                for(let i=0;i<assignmentDocType.length;i++)
                {
                    if (assignmentDocType[i].name === docType) {
                        visibleToTSStatus = assignmentDocType[i].isTSVisible;
                    }
                }
                documentFile.map(document => {                   
                    const filterType  =  configuration.allowedFileFormats.indexOf( document.name.substring(document.name.lastIndexOf(".")).toLowerCase());
                    if (filterType >= 0 && document.name.lastIndexOf(".") !== -1) {  // sanity 204 fix
                        if (parseInt(document.size / 1024) > configuration.fileLimit) {
                            failureFiles.push(document.name);
                        }
                        else {
                            filesToBeUpload.push(document);
                        }
                    }else{
                            failureFormat.push(document.name);
                    }                    
                    return document;
                });

                if (failureFormat.length === 0 && filesToBeUpload.length > 0) {
                     for (let i = 0; i < filesToBeUpload.length; i++) {
                        this.uniqueName = {
                            documentName: filesToBeUpload[i].name,
                            moduleCode: "ASGMNT",
                            requestedBy: this.props.loggedInUser,
                           // moduleCodeReference:this.props.currentPage ===localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE?0:this.props.assignmentId 
                        };
                        this.updatedData = {
                            documentName: filesToBeUpload[i].name,
                            documentType: docType,
                            documentSize:  filesToBeUpload[i].size == 0 ? 0 : parseInt(filesToBeUpload[i].size / 1024) == 0 ? 1 : parseInt(filesToBeUpload[i].size / 1024),//def1126
                            createdOn: date,
                            isVisibleToCustomer: customerVisibleStatus,
                            isVisibleToTS: visibleToTSStatus,
                            recordStatus: "N",
                            canDelete: true,
                            id: Math.floor(Math.random() * (Math.pow(10, 5))),
                            modifiedBy: this.props.loggedInUser,
                            moduleCode: "ASGMNT",
                            //moduleRefCode: this.props.currentPage ===localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE?0:this.props.assignmentId ,// D1186 & D1188
                            status: "C",
                            newlyAddedRecord:true,
                            requestedBy: this.props.loggedInUser,
                            isUploaded:false
                        };
                        documentUniqueName.push(this.uniqueName);
                        createdNewRecords.push(this.updatedData);
                        this.props.actions.AddAssignmentDocumentDetails(createdNewRecords);
                        this.updatedData = {};
                        this.uniqueName = {};
                        createdNewRecords = [];
                    }

                    this.child.refreshGrid();

                    let tempFilesToBeUpload = [];
                    tempFilesToBeUpload = [
                        ...filesToBeUpload, ...this.props.fileToBeUploaded
                    ];
                    this.props.actions.AddFilesToBeUpload(tempFilesToBeUpload);
                    this.setState({
                        isDocumentModalOpen: false,
                        isDocumentModelAddView: false
                    });
                    
                    if (!isQueuePending)
                        this.recursiveFileUpload(documentUniqueName[0], filesToBeUpload);
                }
                if (failureFormat.length > 0) {
                    IntertekToaster(failureFiles.toString() + localConstant.companyDetails.Documents.FILE_FORMAT_WRONG, 'warningToast contractDocSizeReq');
                }
                if (failureFiles.length > 0) {
                    IntertekToaster(failureFiles.toString() + localConstant.assignments.FILE_LIMIT_EXCEDED, 'warningToast assignmentDocSizeReq');
                }

            }
            else {
                IntertekToaster(localConstant.assignments.NO_FILE_SELECTED, 'warningToast assignmentNoFileSelectedReq');
            }
    }

    recursiveFileUpload = async (uniqueNameDetail, filesToBeUpload) => {
        const response = await this.props.actions.FetchDocumentUniqueName([ uniqueNameDetail ], filesToBeUpload);
        if (!isEmpty(response)) {
            for (let i = 0; i < response.length; i++) {
                if (response[i] !== null) {
                    let updatedDataAfterUploaded = {};
                    if (!response[i].status) {
                        this.UploadedFile = this.props.documentrowData.filter(x => x.documentName == response[i].fileName && x.documentUniqueName === undefined);

                        updatedDataAfterUploaded = {
                            recordStatus: "N",
                            documentUniqueName: response[i].uploadedFileName,
                            moduleCodeReference: this.props.currentPage ===localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE?0:this.props.assignmentId ,  // D1186 & D1188
                            canDelete: true,
                            isUploaded: true
                        };
                    }
                    this.props.actions.UpdateAssignmentDocumentDetails(updatedDataAfterUploaded, this.UploadedFile[0]);
                }
            }
        }
        else {
            const afterRemovefailedDoc = this.props.documentrowData.filter(x => x.documentName !== uniqueNameDetail.documentName);
            this.props.actions.UpdateAssignmentDocumentDetails(afterRemovefailedDoc);
        }

        if (this.props.documentrowData.length > 0) {
            const queueDocs = this.props.documentrowData.find(doc => doc.isUploaded === false);
            if (queueDocs) {
                const tempUniqueName = {
                    documentName: queueDocs.documentName,
                    moduleCode: "ASGMNT",
                    requestedBy: this.props.loggedInUser,
                };
                this.recursiveFileUpload(tempUniqueName, this.props.fileToBeUploaded);
            }
        }
    };

    /*Input Change Handler*/
    inputChangeHandler = (e) => {
        const fieldValue = e.target[e.target.type === "checkbox" ? "checked" : "value"];
        const fieldName = e.target.name;
        const result = { value: fieldValue, name: fieldName };
        return result;
    }
    /* Form Input data Change Handler*/
    formInputChangeHandler = (e) => {
        const result = this.inputChangeHandler(e);
        this.updatedData[result.name] = result.value;
    }
 
   /**CustomerVisible Inline Switch */
   customerVisibleSwitchChangeHandler = (e,data) => {
    e.preventDefault();
    const result = formInputChangeHandler(e);
    this.editedRowData = data;
    this.updatedData[result.name] = result.value;

    if (this.editedRowData.recordStatus !== "N") {
        this.updatedData["recordStatus"] = "M";
    }
    this.props.actions.UpdateAssignmentDocumentDetails(this.updatedData, this.editedRowData);
    this.updatedData = {};
};

    uploadDocumentHandler = () => {
        const addModal = {};
        addModal.showModalPop = true;
        this.setState({
            isDocumentModalOpen: addModal,
            isDocumentModelAddView: true
        });
        this.editedRowData = {};
    }
    copyDocumentHandler = () => {   
        const selectedRecords = this.child.getSelectedRows();
        const selectedRecordsFromSecondChild = this.secondChild.getSelectedRows();
        const selectedRecordsFromThirdChild = this.thirdChild.getSelectedRows();
        let selectedRecordsFromForthChild=[];
        let selectedRecordsFromFifthChild=[];
        if(this.props.isVisitAssignment){
            selectedRecordsFromForthChild=this.forthChild.getSelectedRows(); 
            selectedRecordsFromFifthChild=this.fifthChild.getSelectedRows();
        }
        let timesheetDocumentsGridData =[];
        if(this.props.isTimesheetAssignment){
             timesheetDocumentsGridData = this.timesheetDocumentsGridData.getSelectedRows();
        }
        if (selectedRecords.length > 0) {
            this.copiedDocumentDetails = selectedRecords;
            IntertekToaster(localConstant.assignments.SELECTED_RECORDS_COPIED, 'warningToast projDocumentCopyMesgReq');
        }
        else if (selectedRecordsFromSecondChild.length > 0) {
            this.copiedDocumentDetails = selectedRecordsFromSecondChild;
            IntertekToaster(localConstant.assignments.SELECTED_RECORDS_COPIED, 'warningToast projDocumentSecndCopyMesgReq');
        }
        else if (selectedRecordsFromThirdChild.length > 0) {
            this.copiedDocumentDetails = selectedRecordsFromThirdChild;
            IntertekToaster(localConstant.assignments.SELECTED_RECORDS_COPIED, 'warningToast projDocumentThrdCopyMesgReq');
        }
        else if (selectedRecordsFromForthChild.length > 0) {
            this.copiedDocumentDetails = selectedRecordsFromForthChild;
            IntertekToaster(localConstant.assignments.SELECTED_RECORDS_COPIED, 'warningToast projDocumentFrthCopyMesgReq');
        }
        else if (selectedRecordsFromFifthChild.length > 0) {
            this.copiedDocumentDetails = selectedRecordsFromFifthChild;
            IntertekToaster(localConstant.assignments.SELECTED_RECORDS_COPIED, 'warningToast projDocumentFrthCopyMesgReq');
        }        
        else if(timesheetDocumentsGridData.length >0 ){
            this.copiedDocumentDetails = timesheetDocumentsGridData;
            IntertekToaster(localConstant.assignments.SELECTED_RECORDS_COPIED, 'warningToast projDocumentFifthCopyMesgReq');
        }
        else {
            IntertekToaster(localConstant.assignments.SELECT_RECORDS_TO_COPY, 'warningToast projDocumentCopyReq');
        }
    }
    pasteDocumentHandler = () => {     
        const records = [];
        let recordToPaste = [];
        let selectType=[];
        recordToPaste = this.copiedDocumentDetails;
        if (recordToPaste.length > 0) {
            let date = new Date();
            date = dateUtil.postDateFormat(date, '-');            
            for(let i=0;i<recordToPaste.length;i++){
                recordToPaste[i].moduleCode="ASGMNT";
                delete recordToPaste[i].moduleRefCode;
                //recordToPaste[i].moduleRefCode=this.props.assignmentId;                
            }
            this.props.actions.PasteDocumentUploadData(recordToPaste)
                .then(response => {                   
                    if (!isEmpty(response)) {
                        for (let i = 0; i < response.length; i++) {
                            selectType = this.props.assignmentDocumentTypeData.filter(Type=>{ if(Type.name === response[i].documentType)return Type; });
                            this.updatedData = {
                                documentName: response[i].documentName,
                                //documentType: recordToPaste[i].documentType,
                                documentType:selectType.length > 0 ? selectType[0].name : '',
                                documentSize: recordToPaste[i].documentSize,
                                createdOn: date,
                                isVisibleToCustomer: recordToPaste[i].isVisibleToCustomer,
                                isVisibleToTS: recordToPaste[i].isVisibleToTS,
                                recordStatus: "N",
                                canDelete: true,
                                id: Math.floor(Math.random() * (Math.pow(10, 5))),
                                documentUniqueName: response[i].uniqueName,
                                modifiedBy: this.props.loggedInUser,
                                moduleCode: response[i].moduleCode,
                                moduleRefCode: response[i].moduleCodeReference,
                                status: "C"
                            };
                            records.push(this.updatedData);
                            this.updatedData = {};
                        }
                        this.props.actions.AddAssignmentDocumentDetails(records);
                    }
                });
        }
        else {
            IntertekToaster(localConstant.assignments.COPY_RECORDS_TO_PASTE, 'warningToast documentRecordToPasteReq');
        }
    }

    deleteDocumentHandler = () => {
        const selectedRecords = this.child.getSelectedRows();
        const today = new Date();
        const currentYear = today.getFullYear();
        const currentQuarter = Math.floor((today.getMonth() + 3) / 3);
        let dateFlag = false;
        for (let i = 0; i < selectedRecords.length; i++) {
            const createdDate = new Date(selectedRecords[i].createdOn);
            const createdYear = createdDate.getFullYear();
            const docCreatedQuarter = Math.floor((createdDate.getMonth() + 3) / 3);
            if (currentYear !== createdYear || (currentYear === createdYear && currentQuarter !== docCreatedQuarter)) {
                dateFlag = true;
                break;
            }
        }
        if (selectedRecords.length === 0) {
            IntertekToaster(localConstant.contract.Documents.SELECT_ATELEAST_ONE_DOC_TO_DELETE, 'warningToast oneDocumentReq');
            return false;
        }
        else if(dateFlag){
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.DOCUMENT_NOT_DELETE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: "OK",
                        onClickHandler: this.confirmationRejectHandler,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
        else {
            const isFileUploaded = uploadedDocumentCheck(selectedRecords);
            if (isFileUploaded) {
                const confirmationObject = {
                    title: modalTitleConstant.CONFIRMATION,
                    message: modalMessageConstant.DOCUMENT_DELETE_MESSAGE,
                    type: "confirm",
                    modalClassName: "warningToast",
                    buttons: [
                        {
                            buttonName: "Yes",
                            onClickHandler: this.deleteContractRecord,
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
    }
    deleteContractRecord = () => {
        let selectedRecords = this.child.getSelectedRows();
        selectedRecords = selectedRecords.filter(x => x.canDelete == true);
        if (selectedRecords.length > 0) {
            const isFileUploaded = uploadedDocumentCheck(selectedRecords);
            if (isFileUploaded) {
                this.child.removeSelectedRows(selectedRecords);
                this.props.actions.DeleteAssignmentDocumentDetails(selectedRecords);
                this.props.actions.HideModal();
            }
        }
    }
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }
    panelClick = (e) => {
        this.setState({
            isPanelOpen: !this.state.isPanelOpen
        });
    }
    projectPanelClick = (e) => {
        this.setState({
            isProjectPanelOpen: !this.state.isProjectPanelOpen
        });
    }
    supplierPanelClick = (e) => {
        this.setState({
            isSupplierPanelOpen: !this.state.isSupplierPanelOpen
        });
    }
    visitPanelClick = (e) => {
        this.setState({
            isVisitPanelOpen: !this.state.isVisitPanelOpen
        });
    }
    timesheetPanelClick = (e) => {
        this.setState({
            isTimesheetPanelOpen: !this.state.isTimesheetPanelOpen
        });
    }
    uploadDocumentSubmitHandler = (e) => {
         e.preventDefault();
         const documentFile = Array.from(document.getElementById("uploadFiles").files);
            if (isEmpty(this.updatedData.documentType)) {
            IntertekToaster(localConstant.assignments.SELECT_FILE_TYPE, 'warningToast assignmentDocFileTypeReq');
            return false;
        }
         this.fileupload(documentFile);
        // let date = new Date();
        // date = dateUtil.postDateFormat(date, '-');
        // // else {
        //     const docType = this.updatedData.documentType;
        //     const documentFile = Array.from(document.getElementById("uploadFiles").files);
        //     if (documentFile.length > 0) {
        //         const newlyCreatedRecords = [];
        //         const filesToBeUpload = [];
        //         const failureFiles = [];
        //         const documentUniqueName = [];
        //         const customerVisibleStatus = this.updatedData.isVisibleToCustomer ? this.updatedData.isVisibleToCustomer : false;;
        //         let visibleToTSStatus = false;
        //         const assignmentDocType=this.props.assignmentDocumentTypeData;
        //         for(let i=0;i<assignmentDocType.length;i++)
        //         {
        //             if (assignmentDocType[i].name === docType) {
        //                 visibleToTSStatus = assignmentDocType[i].isTSVisible;
        //             }
        //         }
        //         documentFile.map(document => {
        //             if (parseInt(document.size / 1024) > 10240) {
        //                 failureFiles.push(document.name);
        //             }
        //             else {
        //                 filesToBeUpload.push(document);
        //             }
        //             return document;
        //         });
        //         if (filesToBeUpload.length > 0) {
        //             for (let i = 0; i < filesToBeUpload.length; i++) {
        //                 this.updatedData = {
        //                     documentName: filesToBeUpload[i].name,
        //                     moduleCode: "ASGMNT",
        //                     requestedBy: this.props.loggedInUser,
        //                     moduleCodeReference:this.props.currentPage ===localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE?0:this.props.assignmentId ,
        //                 };
        //                 documentUniqueName.push(this.updatedData);
        //                 this.updatedData = {};
        //                 this.setState({
        //                     isDocumentModalOpen: false,
        //                     isDocumentModelAddView: false
        //                 });
        //             }
        //             this.props.actions.FetchDocumentUniqueName(documentUniqueName, filesToBeUpload)
        //                 .then(response => {
        //                     if (!isEmpty(response)) {
        //                         for (let i = 0; i < response.length; i++) {
        //                             if (!response[i].status) {
        //                                 this.updatedData = {
        //                                     documentName: response[i].fileName,
        //                                     documentType: docType,
        //                                     documentSize: parseInt(filesToBeUpload[i].size / 1024),
        //                                     createdOn: date,
        //                                     isVisibleToCustomer: customerVisibleStatus,
        //                                     isVisibleToTS: visibleToTSStatus,
        //                                     recordStatus: "N",
        //                                     id: Math.floor(Math.random() * (Math.pow(10, 5))),
        //                                     documentUniqueName: response[i].uploadedFileName,
        //                                     modifiedBy: this.props.loggedInUser,
        //                                     moduleCode: response[i].moduleCode,
        //                                     moduleRefCode: response[i].moduleReferenceCode,
        //                                     status: "C",
        //                                     newlyAddedRecord:true
        //                                 };
        //                                 newlyCreatedRecords.push(this.updatedData);
        //                                 this.updatedData = {};
        //                             }
        //                         }
        //                         if (newlyCreatedRecords.length > 0) {
        //                             this.props.actions.AddAssignmentDocumentDetails(newlyCreatedRecords);
        //                         }
        //                     };
        //                 });
        //         }
        //         if (failureFiles.length > 0) {
        //             IntertekToaster(failureFiles.toString() + localConstant.assignments.FILE_LIMIT_EXCEDED, 'warningToast assignmentDocSizeReq');
        //         }

        //     }
        //     else {
        //         IntertekToaster(localConstant.assignments.NO_FILE_SELECTED, 'warningToast assignmentNoFileSelectedReq');
        //     }
        //}
    }

    editDocumentHandler = (e) => {
        e.preventDefault();
        let date = new Date();
        date = dateUtil.postDateFormat(date, '-');
        this.setState({
            isDocumentModalOpen: false,
            isDocumentModelAddView: false
        });
        if (this.updatedData.documentType === "") {
            IntertekToaster(localConstant.assignments.SELECT_FILE_TYPE, 'warningToast assignmentFileTypeReq');
            return false;
        }
        else {
            if (this.updatedData.documentType) {
                const assignmentDocType=this.props.assignmentDocumentTypeData;
                for(let i=0;i<assignmentDocType.length;i++)
                {
                    if (assignmentDocType[i].name === this.updatedData.documentType) {
                        this.updatedData["isVisibleToTS"] = assignmentDocType[i].isTSVisible;
                    }
                }
            }
            this.updatedData["createdOn"] = date;
            if (this.editedRowData.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            this.props.actions.UpdateAssignmentDocumentDetails(this.updatedData, this.editedRowData);
            this.updatedData = {};
        }
    }

    hideModal = (e) => {
        e.preventDefault();
        this.setState({
            isDocumentModalOpen: false
        });
        this.updatedData = {};
        this.editedRowData = {};
    }
    documentMultipleDownloadHandlerforFC = (e) =>
    {
        let selectedRecords = [];
        if(this.props.isVisitAssignment){
            selectedRecords = this.fifthChild.getSelectedRows();
        }
        if(this.props.isTimesheetAssignment){
            selectedRecords = this.timesheetDocumentsGridData.getSelectedRows();
        }
        if (selectedRecords.length > 0) {
            this.props.actions.MultiDocDownload(selectedRecords);
        }
        else
        {
            IntertekToaster(localConstant.customer.documents.SELECT_ATELEAST_ONE_DOC_TO_DOWNLOAD, 'warningToast CustDocDelDocChk'); 
        }  
    }
    documentMultipleDownloadHandlerforTC = (e) =>
    {
        const selectedRecords = this.forthChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            this.props.actions.MultiDocDownload(selectedRecords);
        }
        else
        {
            IntertekToaster(localConstant.customer.documents.SELECT_ATELEAST_ONE_DOC_TO_DOWNLOAD, 'warningToast CustDocDelDocChk'); 
        }  
    }
    documentMultipleDownloadHandlerforPO = (e) =>
    {
        const selectedRecords = this.thirdChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            this.props.actions.MultiDocDownload(selectedRecords);
        }
        else
        {
            IntertekToaster(localConstant.customer.documents.SELECT_ATELEAST_ONE_DOC_TO_DOWNLOAD, 'warningToast CustDocDelDocChk'); 
        }  
    }
    documentMultipleDownloadHandlerforSC = (e) =>
    {
        const selectedRecords = this.secondChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            this.props.actions.MultiDocDownload(selectedRecords);
        }
        else
        {
            IntertekToaster(localConstant.customer.documents.SELECT_ATELEAST_ONE_DOC_TO_DOWNLOAD, 'warningToast CustDocDelDocChk'); 
        }  
    }

    documentmultipleDownloadHandler = (e) => {
        e.preventDefault();
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            this.props.actions.MultiDocDownload(selectedRecords);
    }
    else
    {
        IntertekToaster(localConstant.customer.documents.SELECT_ATELEAST_ONE_DOC_TO_DOWNLOAD, 'warningToast CustDocDelDocChk'); 
    }
      
    }
    renameConfirmationRejectHandler = (fileName,data) => {
        this.updatedtoStore(fileName,data);
        this.updateChangesToGridReferesh('',data);
        this.props.actions.HideModal();
   }
    updatedtoStore=(fileName,data)=>{
        const parent = this;
        let date = new Date();
        date = dateUtil.postDateFormat(date, '-');
        this.updatedData["documentName"]=fileName;
        this.updatedData["uploadedOn"] = date;  
        // if (parent.props.editassignmentProjectDocumentsData && parent.props.editassignmentProjectDocumentsData.recordStatus !== "N") {
        //  this.updatedData["uploadedOn"] = date;             
        //  this.updatedData["recordStatus"] = "M";
        //  }     
        parent.props.actions.UpdateAssignmentDocumentDetails(this.updatedData, data);
        this.updatedData={}; 
    }
    updateChangesToGridReferesh=(oldfileNameFormat,data)=>{
        const parent = this;    
        const index = parent.props.documentrowData.findIndex(document => document.id === data.id);
        const propvalue = parent.props.documentrowData[index].documentName.split(/\.(?=[^\.]+$)/)[1];
        if(propvalue === undefined){
            const obj = { ...parent.props.documentrowData[index] };
            obj.documentName = parent.props.documentrowData[index].documentName+'.'+oldfileNameFormat;                  
            this.child.setData(obj,(index));
        }else{
            this.child.setData(parent.props.documentrowData[index],index);
        }
    }
    onCellchange=(data)=>{      
        const parent = this;      
        const oldfileNameFormat = data.oldValue.split(/\.(?=[^\.]+$)/)[1];
        const newfileNameFormat = data.value.split(/\.(?=[^\.]+$)/)[1];
        const oldFileName = data.oldValue.split(/\.(?=[^\.]+$)/)[0];
        const newFileName = data.newValue.split(/\.(?=[^\.]+$)/)[0]; 
        if(!isUndefined(data.data.documentUniqueName)){
        if(data.data.recordStatus !=='N'){
            data.data.recordStatus='M';
         }    
         if(oldfileNameFormat === newfileNameFormat){               
                this.updatedtoStore(data.newValue,data.data);                   
         }else if(newfileNameFormat === undefined){
            if(oldFileName !== newFileName){
                const fileNameWithFormat = data.newValue +'.'+oldfileNameFormat;
                this.updatedtoStore(fileNameWithFormat,data.data);                                           
            }else{
                this.updateChangesToGridReferesh(oldfileNameFormat,data.data);                
            }  
         }else{
            //IntertekToaster('Your Exgisting File Format is '+ oldfileNameFormat + ' Please Provide Same Format', 'warningToast documentCopyReq');
             const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: localConstant.commonConstants.FILE_RENAME_MSG_START + oldfileNameFormat.toUpperCase() + localConstant.commonConstants.FILE_RENAME_MSG_END,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    
                    {
                        buttonName: "OK",
                        onClickHandler: ()=>this.renameConfirmationRejectHandler(data.oldValue,data.data),
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            parent.props.actions.DisplayModal(confirmationObject);      
         }
        }
        else{
            IntertekToaster( localConstant.commonConstants.DOCUMENT_STATUS , 'warningToast documentRecordToPasteReq');
        }
    }

    render() {
        const {
            assignmentProjectDocumentsData,
            assignmentVisitDocumentsData,
            assignmentContractDocumentsData,
            assignmentSupplierPODocumentsData,
            assignmentDocumentTypeData,
            documentrowData,
            interactionMode,
            assignmentType,
            assignmentTimesheetDocumentsData
        } = this.props;
        const rowDocData=documentrowData && documentrowData.filter(record => record.recordStatus !== "D");
        this.documentAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.hideModal,
                btnID: "cancelDocumentSubmit",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.state.isDocumentModelAddView ? this.uploadDocumentSubmitHandler : this.editDocumentHandler,
                btnID: "addDocumentsHandler",
                btnClass: "waves-effect waves-teal btn-small mr-2",
                showbtn: true
            }
        ];
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        bindAction(this.headerData, "EditAssignmentDocuments", this.editRowHandler);
        bindAction(this.headerData, "isVisibleToCustomer", (e,data) =>  this.customerVisibleSwitchChangeHandler(e,data) );
        for(let i = 0; i < rowDocData.length; i++){
            rowDocData[i] = { ...rowDocData[i],roleBase: ((this.props.interactionMode && this.props.pageMode === localConstant.commonConstants.VIEW)) };
            rowDocData[i] = { ...rowDocData[i],isFileUploaded: ((isUndefined(rowDocData[i].documentUniqueName))?false:true) };
        } 
        return (
            <Fragment>
                <div className="customCard">
                    <h6 className="bold col s3 pl-0 pr-3">{localConstant.documents.DOCUMENTS}</h6>
                    <AssignmentDocumentMenuButtons
                    rowData={rowDocData}
                    headerData={this.headerData}
                    copyDocumentHandler={this.copyDocumentHandler}
                    pasteDocumentHandler={this.pasteDocumentHandler}
                    uploadDocumentHandler={this.uploadDocumentHandler}
                    deleteDocumentHandler={this.deleteDocumentHandler}
                    DownloadDocumentHandler={this.documentmultipleDownloadHandler}
                    pageMode={this.props.pageMode}
                    interactionMode={!this.props.interactionMode}
                    supportFileType={configuration}
                    onCellchange={this.onCellchange}
                    paginationPrefixId={localConstant.paginationPrefixIds.assignmentDoc}
                    onRef={ref => { this.child = ref; }}
                    />
                </div>
                {this.state.isDocumentModalOpen &&
                    <Modal id="assignmentDocModalPopup"
                        title={this.state.isDocumentModelAddView ? localConstant.assignments.UPLOAD_DOC : localConstant.assignments.EDIT_DOC}
                        modalClass="assignmentDocModal"
                        buttons={this.documentAddButtons}
                        isShowModal={this.state.isDocumentModalOpen}>

                        {this.state.isDocumentModelAddView ? 
                        <ModalPopupAddView
                            assignmentDocumentTypeData={assignmentDocumentTypeData}
                            handlerChange={(e) => this.formInputChangeHandler(e)}
                            supportFileType={configuration}
                        />
                            : <ModalPopupEditView
                            assignmentDocumentTypeData={assignmentDocumentTypeData}
                                handlerChange={(e) => this.formInputChangeHandler(e)}
                                eidtedAssignmentDocuments={this.editedRowData}
                            />}
                    </Modal>}
                <Panel colSize="s12" heading={localConstant.documents.CONTRACT_DOCUMENTS}
                    className="pl-0 pr-0 mt-3 documentPanel bold"
                    onpanelClick={this.panelClick}
                    isopen={this.state.isPanelOpen} isArrow={true} >
                    <RelatedDocuments DocumentsData={assignmentContractDocumentsData}
                        headerData={ChildHeaderData}
                        interactionMode={interactionMode}
                        title={localConstant.documents.CONTRACT_DOCUMENTS}
                        copyRecord={this.copyDocumentHandler}
                        pageMode={this.props.pageMode}
                        DownloadDocumentHandler={this.documentMultipleDownloadHandlerforSC}
                        paginationPrefixId={localConstant.paginationPrefixIds.assignmentContractDoc}
                        onRef={ref => { this.secondChild = ref; }}
                    />
                </Panel>
                <Panel colSize="s12" heading={localConstant.documents.PROJECT_DOCUMENTS}
                    className="pl-0 pr-0  documentPanel bold"
                    onpanelClick={this.projectPanelClick}
                    isopen={this.state.isProjectPanelOpen} isArrow={true} >
                    <RelatedDocuments DocumentsData={assignmentProjectDocumentsData}
                        headerData={ChildHeaderData}
                        interactionMode={interactionMode}
                        title={localConstant.documents.PROJECT_DOCUMENTS}
                        copyRecord={this.copyDocumentHandler}
                        DownloadDocumentHandler={this.documentMultipleDownloadHandlerforPO}
                        pageMode={this.props.pageMode}
                        paginationPrefixId={localConstant.paginationPrefixIds.assignmentProjectDoc}
                        onRef={ref => { this.thirdChild = ref; }}
                    />
                </Panel>
                {this.props.isVisitAssignment? <Panel colSize="s12" heading={localConstant.documents.SUPPLIER_PO_DOCUMENTS}
                    className="pl-0 pr-0  documentPanel bold"
                    onpanelClick={this.supplierPanelClick}
                    isopen={this.state.isSupplierPanelOpen} isArrow={true} >
                    <RelatedDocuments DocumentsData={assignmentSupplierPODocumentsData}
                        headerData={ChildHeaderData}
                        interactionMode={interactionMode}
                        title={localConstant.documents.SUPPLIER_PO_DOCUMENTS}
                        copyRecord={this.copyDocumentHandler}
                        DownloadDocumentHandler={this.documentMultipleDownloadHandlerforTC}
                        pageMode={this.props.pageMode}
                        paginationPrefixId={localConstant.paginationPrefixIds.assignmentSPODoc}
                        onRef={ref => { this.forthChild = ref; }}
                    />
                </Panel>:null                
            }
             {this.props.isVisitAssignment?<Panel colSize="s12" heading={localConstant.documents.VISIT_DOCUMENTS}
                    className="pl-0 pr-0  documentPanel bold"
                    onpanelClick={this.visitPanelClick}
                    isopen={this.state.isVisitPanelOpen} isArrow={true} >
                    <RelatedDocuments DocumentsData={assignmentVisitDocumentsData}
                        headerData={VisitDocumentHeaderData}
                        interactionMode={interactionMode}
                        title={localConstant.documents.VISIT_DOCUMENTS}
                        copyRecord={this.copyDocumentHandler}
                        DownloadDocumentHandler={this.documentMultipleDownloadHandlerforFC}
                        onRef={ref => { this.fifthChild = ref; }}
                        paginationPrefixId={localConstant.paginationPrefixIds.assignmentTISDoc}
                        pageMode={this.props.pageMode}
                    />
                </Panel>:null}
            {this.props.isTimesheetAssignment ?<Panel colSize="s12" heading={localConstant.documents.TIMESHEET_DOCUMENTS}
                    className="pl-0 pr-0  documentPanel bold"
                    onpanelClick={this.timesheetPanelClick}
                    isopen={this.state.isTimesheetPanelOpen} isArrow={true} >
                    <RelatedDocuments DocumentsData={assignmentTimesheetDocumentsData}
                        headerData={TimesheetDocumentHeaderData}
                        interactionMode={interactionMode}
                        title={localConstant.documents.TIMESHEET_DOCUMENTS}
                        copyRecord={this.copyDocumentHandler}
                        DownloadDocumentHandler={this.documentMultipleDownloadHandlerforFC}
                        onRef={ref => { this.timesheetDocumentsGridData = ref; }}
                        paginationPrefixId={localConstant.paginationPrefixIds.assignmentTSSDoc}
                        pageMode={this.props.pageMode}
                    />
                </Panel> :null }   
                <CustomModal modalData={modelData} />
            </Fragment>
        );
    }
}

export default AssignmentDocuments;
AssignmentDocuments.propTypes = {
    documentrowData: PropTypes.array,
    documentsTypeData:PropTypes.array,
    assignmentProjectDocumentsData: PropTypes.array,
    assignmentVisitDocumentsData:PropTypes.array,
    assignmentTimesheetDocumentsData:PropTypes.array,
    assignmentContractDocumentsData: PropTypes.array,
    assignmentSupplierPODocumentsData:PropTypes.array,
};

AssignmentDocuments.defaultProps = {
    documentrowData: [],
    documentsTypeData:[],
    assignmentProjectDocumentsData: [],
    assignmentVisitDocumentsData: [],
    assignmentTimesheetDocumentsData:[],
    assignmentContractDocumentsData: [],
    assignmentSupplierPODocumentsData:[],
};