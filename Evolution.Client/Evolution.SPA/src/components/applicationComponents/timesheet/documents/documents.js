import React, { Component, Fragment } from 'react';
import { HeaderData } from './documentHeader';
import CustomModal from '../../../../common/baseComponents/customModal';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, isEmpty, bindAction,formInputChangeHandler,isUndefined,uploadedDocumentCheck  } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import { configuration } from '../../../../appConfig';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import Modal from '../../../../common/baseComponents/modal';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import  TimesheetDocumentMenuButtons  from '../../../documents/documents'; 
import { MultipleDownload } from '../../../documents/documentsMultidownload';
import { applicationConstants } from '../../../../constants/appConstants';
import { required } from '../../../../utils/validator';

const localConstant = getlocalizeData();
const ModalPopupAddView = (props) => {
    return (
        <Fragment>
            <CustomInput hasLabel={true}
                label={localConstant.contract.Documents.SELECT_FILE_TYPE}
                divClassName='col pl-0 mb-4'
                colSize="s6"
                type='select'
                labelClass="mandate"
                required={true} //D526
                className="customInputs browser-default"
                optionsList={props.timesheetDocumentType}
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
                        <input id="uploadFiles" type="file" accept={props.supportFileType} multiple required />
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
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.modalConstant.VISIBLE_TO_TS}
                    isSwitchLabel={true}
                    switchName="isVisibleToTS"
                    id="isVisibleToTS"
                    colSize="s3 m4 mt-4"
                    checkedStatus={props.isVisibleToTS}
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
                label={localConstant.contract.Documents.SELECT_FILE_TYPE}
                divClassName='col pl-0'
                colSize="s6"
                type='select'
                required={true}
                className="customInputs browser-default"
                optionsList={props.timesheetDocumentType}
                optionName='name'
                optionValue="name"
                onSelectChange={props.handlerChange}
                name="documentType"
                id="documentType"
                defaultValue={props.documentType}
            />
            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.contract.Documents.DOCUMENT_NAME}
                type='text'
                colSize='s6'
                inputClass="customInputs"
                labelClass="mandate"
                name="documentName"
                disabled={true}
                defaultValue={props.documentName}
            />

            {/* <div className="row">
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.modalConstant.VISIBLE_TO_CUSTOMER}
                    isSwitchLabel={true}
                    switchName="isVisibleToCustomer"
                    id="isVisibleToCustomerEdit"
                    colSize="s3 m4 mt-4"
                    checkedStatus={props.isVisibleToCustomer}
                    onChangeToggle={props.handlerChange}
                />
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.modalConstant.VISIBLE_TO_TS}
                    isSwitchLabel={true}
                    switchName="isVisibleToTS"
                    id="isVisibleToTS"
                    colSize="s3 m4 mt-4"
                    checkedStatus={props.isVisibleToTS}
                    onChangeToggle={props.handlerChange}
                />
            </div> */}
        </Fragment>
    );
};

class Documents extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isDocumentModalOpen: false,
            isDocumentModelAddView: true,
            isPanelOpen: false
        };
        this.updatedData = {};
        this.filesToDisplay = [];
        this.uploadButtonDisable = false;
        this.editedRowData = {};
        this.copiedDocumentDetails = [];
        this.functionRef={};
        this.functionRef['isDisable']=(this.props.pageMode === localConstant.commonConstants.VIEW);
        this.headerData=HeaderData(this.functionRef);
        this.confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: modalMessageConstant.DOCUMENT_DELETE_MESSAGE,
            type: "confirm",
            modalClassName: "warningToast",
            buttons: [
                {
                    buttonName: "Yes",
                    onClickHandler: this.deleteTimesheetRecord,
                    className: "modal-close m-1 btn-small"
                },
                {
                    buttonName: "No",
                    onClickHandler: this.confirmationRejectHandler,
                    className: "modal-close m-1 btn-small"
                }
            ]
        };         
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
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        if (tabInfo && tabInfo.componentRenderCount === 0 &&
            this.props.currentPage === localConstant.timesheet.EDIT_VIEW_TIMESHEET_MODE){
            this.props.actions.FetchTimesheetDocuments();
        }
        if (!(this.props.interactionMode && this.props.pageMode === localConstant.commonConstants.VIEW)) {
            //Drag and Drop File Upload
            this.dropArea = document.getElementById("drop-area");
            if(this.dropArea && this.dropArea.addEventListener) {
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
            const queueDocs = this.props.timesheetDocuments.find(doc => doc.isUploaded === false);
            const isQueuePending = queueDocs ? true : false;
            if (documentFile.length > 0) {
                const newlyCreatedRecords = [];
                const filesToBeUpload = [];
                const failureFiles = [];
                const failureFormat=[];
                const documentUniqueName = [];
                let createdNewRecords =[];
                let customerVisibleStatus = false;
                let visibleToTSStatus = false;
                const outOfCompanyVisible = this.updatedData.isVisibleOutOfCompany ? this.updatedData.isVisibleOutOfCompany : false;
                this.props.timesheetDocTypes.map(doc => {
                    if (doc.name === docType) {
                        visibleToTSStatus = doc.isTSVisible;
                    }
                });
                 //Defect ID 702 - customer visible switch can Enable while document Upload 
                 // As per discussion with Sumit, Documnt Type like 'Non Conformance Report','Report - Flash','Release Note' Based customer Visible Switch Enabled
                if(!isUndefined(docType)){                        
                    customerVisibleStatus = (applicationConstants.customerVisibleDocType.filter( x => { return x === docType; }).length > 0);       
                }else{
                    customerVisibleStatus = false;
                }
                documentFile.map(document => {                   
                    const filterType  =  configuration.allowedFileFormats.indexOf( document.name.substring(document.name.lastIndexOf(".")).toLowerCase());                 
                    if(filterType >= 0 && document.name.lastIndexOf(".") !==-1 ){  // sanity 204 fix
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
                            moduleCode: "TIME",
                            requestedBy: this.props.loggedInUser,
                            //moduleCodeReference: this.props.timesheetId?this.props.timesheetId:0 // D1186 & D1188
                        };
                    
                        this.updatedData = {
                            documentName: filesToBeUpload[i].name,
                            documentType: docType,
                            documentSize:  filesToBeUpload[i].size == 0 ? 0 : parseInt(filesToBeUpload[i].size / 1024) == 0 ? 1 : parseInt(filesToBeUpload[i].size / 1024),//def1126
                            createdOn: date,
                            isVisibleToCustomer: customerVisibleStatus,
                            isVisibleToTS: visibleToTSStatus,
                            isVisibleOutOfCompany: outOfCompanyVisible,
                            recordStatus: "N",
                            canDelete:true,
                            id: Math.floor(Math.random() * (Math.pow(10, 5))),
                            modifiedBy: this.props.loggedInUser,
                            moduleCode: "TIME",
                            //moduleRefCode: this.props.timesheetId?this.props.timesheetId:0,// D1186 & D1188
                            status: "C",
                            subModuleRefCode: "0",
                            requestedBy: this.props.loggedInUser,
                            isUploaded:false
                        };
                        documentUniqueName.push(this.uniqueName);
                        createdNewRecords.push(this.updatedData);
                        this.props.actions.AddTimesheetDocumentDetails(createdNewRecords);
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
                    IntertekToaster(failureFiles.toString() + localConstant.contract.Documents.FILE_LIMIT_EXCEDED, 'warningToast timesheetDocSizeReq');
                }
            }
            else {
                IntertekToaster(localConstant.contract.Documents.NO_FILE_SELECTED, 'warningToast timesheetNoFileSelectedReq');
            }
    }

    recursiveFileUpload = async (uniqueNameDetail, filesToBeUpload) => {
        const response = await this.props.actions.FetchDocumentUniqueName([ uniqueNameDetail ], filesToBeUpload);
        if (!isEmpty(response)) {
            for (let i = 0; i < response.length; i++) {
                if (response[i] !== null) {
                    let updatedDataAfterUploaded = {};
                    if (!response[i].status) {
                        this.UploadedFile = this.props.timesheetDocuments.filter(x => x.documentName == response[i].fileName && x.documentUniqueName === undefined);

                        updatedDataAfterUploaded = {
                            recordStatus: "N",
                            documentUniqueName: response[i].uploadedFileName,
                            moduleRefCode: this.props.timesheetId?this.props.timesheetId:0, // D1186 & D1188                                                
                            canDelete: true,
                            isUploaded: true
                        };
                    }
                    this.props.actions.UpdateTimesheetDocumentDetails(updatedDataAfterUploaded, this.UploadedFile[0]);
                }
            }
        }
        else {
            const afterRemovefailedDoc = this.props.timesheetDocuments.filter(x => x.documentName !== uniqueNameDetail.documentName);
            this.props.actions.UpdateTimesheetDocumentDetails(afterRemovefailedDoc);
        }

        if (this.props.timesheetDocuments.length > 0) {
            const queueDocs = this.props.timesheetDocuments.find(doc => doc.isUploaded === false);
            if (queueDocs) {
                const tempUniqueName = {
                    documentName: queueDocs.documentName,
                    moduleCode: "TIME",
                    requestedBy: this.props.loggedInUser,
                };
                this.recursiveFileUpload(tempUniqueName, this.props.fileToBeUploaded);
            }
        }
    };

    //All Input Handle get Name and Value
    inputHandleChange = (e) => {
        // e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
        return inputvalue;
    }   

    /**CustomerVisible Inline Switch */
    customerVisibleSwitchChangeHandler = (e,data) => {
        e.preventDefault();       
        const result = this.inputHandleChange(e);
        this.editedRowData = data;
        this.updatedData[result.name] = result.value;

        if (this.editedRowData.recordStatus !== "N") {
            this.updatedData["recordStatus"] = "M";
        }
        this.props.actions.UpdateTimesheetDocumentDetails(this.updatedData,  this.editedRowData);
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
        if (selectedRecords.length > 0) {
            this.copiedDocumentDetails = selectedRecords;
            IntertekToaster(localConstant.contract.SELECTED_RECORDS_COPIED, 'warningToast documentCopyMesgReq');
        } 
        else {
            IntertekToaster(localConstant.contract.Documents.SELECT_RECORDS_TO_COPY, 'warningToast documentCopyReq');
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
            recordToPaste.map((record) => {
                record.moduleCode = "TIME";
                delete record.moduleRefCode;
                record.moduleRefCode = this.props.timesheetId;
            });

            this.props.actions.PasteDocumentUploadData(recordToPaste)
                .then(response => {
                    if (!isEmpty(response)) {
                        for (let i = 0; i < response.length; i++) {
                            selectType = this.props.timesheetDocTypes.filter(Type=>{ if(Type.name === response[i].documentType)return Type; });
                            this.updatedData = {
                                documentName: response[i].documentName,
                                //documentType: recordToPaste[i].documentType,
                                documentType:selectType.length > 0 ? selectType[0].name : '',
                                documentSize: recordToPaste[i].documentSize,
                                createdOn: date,
                                isVisibleToCustomer: recordToPaste[i].isVisibleToCustomer,
                                isVisibleToTS: recordToPaste[i].isVisibleToTS,
                                isVisibleOutOfCompany: recordToPaste[i].isVisibleOutOfCompany ? recordToPaste[i].isVisibleOutOfCompany : false,
                                recordStatus: "N",
                                canDelete:true,
                                id: Math.floor(Math.random() * (Math.pow(10, 5))),
                                documentUniqueName: response[i].uniqueName,
                                modifiedBy: this.props.loggedInUser,
                                moduleCode: response[i].moduleCode,
                                moduleRefCode: response[i].moduleCodeReference,
                                status: "C",
                                subModuleRefCode: "0"
                            };
                            records.push(this.updatedData);
                            this.updatedData = {};
                        }
                        this.props.actions.AddTimesheetDocumentDetails(records);
                    }
                });
        }
        else {
            IntertekToaster(localConstant.contract.Documents.COPY_RECORDS_TO_PASTE, 'warningToast documentRecordToPasteReq');
        }
    }

    deleteDocumentHandler = () => {
         //Defect 948 #5 - as Per Francina Discussed New scenario Updated     
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
        if(dateFlag){
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
        else if(this.props.iInterCompanyAssignment && this.props.isOperatorCompany && (this.props.timesheetInfo.timesheetStatus === 'O' || this.props.timesheetInfo.timesheetStatus === 'A')){
            if (selectedRecords.length > 0){
                const records = selectedRecords.filter(x=> { if (x.recordStatus != null) return x;});
                if(records.length === 0){
                    IntertekToaster(localConstant.project.documents.SELECT_ATELEAST_ONENEW_DOC_TO_DELETE, 'warningToast oneDocumentReq');
                }else{          
                    const isFileUploaded = uploadedDocumentCheck(selectedRecords);
            if (isFileUploaded) {          
                   this.props.actions.DisplayModal(this.confirmationObject);
            }
                }
            }
            else{
                IntertekToaster(localConstant.project.documents.SELECT_ATELEAST_ONE_DOC_TO_DELETE, 'warningToast oneDocumentReq');
            }        
        }
        else if (selectedRecords.length === 0) {
            IntertekToaster(localConstant.contract.Documents.SELECT_ATELEAST_ONE_DOC_TO_DELETE, 'warningToast oneDocumentReq');
        }
        else {
            const isFileUploaded = uploadedDocumentCheck(selectedRecords);
            if (isFileUploaded) {
        this.props.actions.DisplayModal(this.confirmationObject);
            }
        }
    }
    deleteTimesheetRecord = () => {
        //Defect 948 #5 - as Per Francina Discussed New scenario Updated   
        let deletedRecords   = this.child.getSelectedRows();
        deletedRecords =  deletedRecords.filter(x => x.canDelete == true);  
        const selectedRecords = deletedRecords;// this.child.getSelectedRows();        
        if(this.props.iInterCompanyAssignment && this.props.isOperatorCompany && (this.props.timesheetInfo.timesheetStatus === 'O' || this.props.timesheetInfo.timesheetStatus === 'A')) {
            const records = selectedRecords.filter(x => { if (x.recordStatus != null) return x; });
            if (records.length > 0) {
                const isFileUploaded = uploadedDocumentCheck(records);
                if (isFileUploaded) {
                    this.child.removeSelectedRows(records);
                    this.props.actions.DeleteTimesheetDocumentDetails(records);
                    this.props.actions.HideModal();
                }
            }

        } else if (selectedRecords.length > 0) {
            const isFileUploaded = uploadedDocumentCheck(selectedRecords);
            if (isFileUploaded) {
                this.child.removeSelectedRows(selectedRecords);
                this.props.actions.DeleteTimesheetDocumentDetails(selectedRecords);
                this.props.actions.HideModal();
            }
        }
        else
            this.props.actions.HideModal();
    }
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    uploadDocumentSubmitHandler = (e) => {
        e.preventDefault();
        // D526 Mandatory field
        if (required(this.updatedData.documentType)) {
            IntertekToaster(localConstant.contract.Documents.SELECT_FILE_TYPE, 'warningToast timesheetDocFileTypeReq');
        }
        else
        {
            const documentFile = Array.from(document.getElementById("uploadFiles").files);
            this.fileupload(documentFile);
        }
        
        // let date = new Date();
        // date = dateUtil.postDateFormat(date, '-');
        // // if (this.updatedData.documentType === undefined || this.updatedData.documentType === "") {
        // //     IntertekToaster(localConstant.contract.Documents.SELECT_FILE_TYPE, 'warningToast timesheetDocFileTypeReq');
        // // }
        // // else {
        //     const docType = this.updatedData.documentType;
        //     const documentFile = Array.from(document.getElementById("uploadFiles").files);
        //     if (documentFile.length > 0) {
        //         const newlyCreatedRecords = [];
        //         const filesToBeUpload = [];
        //         const failureFiles = [];
        //         const documentUniqueName = [];
        //         const customerVisibleStatus = this.updatedData.isVisibleToCustomer ? this.updatedData.isVisibleToCustomer : false;
        //         let visibleToTSStatus = false;
        //         const outOfCompanyVisible = this.updatedData.isVisibleOutOfCompany ? this.updatedData.isVisibleOutOfCompany : false;
        //         this.props.timesheetDocTypes.map(doc => {
        //             if (doc.name === docType) {
        //                 visibleToTSStatus = doc.isTSVisible;
        //             }
        //         });
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
        //                     moduleCode: "TIME",
        //                     requestedBy: this.props.loggedInUser,
        //                     moduleCodeReference: this.props.timesheetId?this.props.timesheetId:0,
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
        //                             if (response[i] && !response[i].status) {
        //                                 this.updatedData = {
        //                                     documentName: response[i].fileName,
        //                                     documentType: docType,
        //                                     documentSize: parseInt(filesToBeUpload[i].size / 1024),
        //                                     createdOn: date,
        //                                     isVisibleToCustomer: customerVisibleStatus,
        //                                     isVisibleToTS: visibleToTSStatus,
        //                                     isVisibleOutOfCompany: outOfCompanyVisible,
        //                                     recordStatus: "N",
        //                                     id: Math.floor(Math.random() * (Math.pow(10, 5))),
        //                                     documentUniqueName: response[i].uploadedFileName,
        //                                     modifiedBy: this.props.loggedInUser,
        //                                     moduleCode: response[i].moduleCode,
        //                                     moduleRefCode: response[i].moduleReferenceCode,
        //                                     status: "C"
        //                                 };
        //                                 newlyCreatedRecords.push(this.updatedData);
        //                                 this.updatedData = {};
        //                             }
        //                         }
        //                         if (newlyCreatedRecords.length > 0) {
        //                             this.props.actions.AddTimesheetDocumentDetails(newlyCreatedRecords);
        //                         }
        //                     };
        //                 });
        //         }
        //         if (failureFiles.length > 0) {
        //             IntertekToaster(failureFiles.toString() + localConstant.contract.Documents.FILE_LIMIT_EXCEDED, 'warningToast timesheetDocSizeReq');
        //         }
        //     }
        //     else {
        //         IntertekToaster(localConstant.contract.Documents.NO_FILE_SELECTED, 'warningToast timesheetNoFileSelectedReq');
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
            IntertekToaster(localConstant.contract.Documents.SELECT_FILE_TYPE, 'warningToast contractFileTypeReq');
            return false;
        }
        else {
            if (this.updatedData.documentType) {
                this.props.timesheetDocTypes.forEach(doc => {
                    if (doc.name === this.updatedData.documentType) {
                        this.updatedData["isVisibleToTS"] = doc.isTSVisible;
                    }
                });
            }
            this.updatedData["createdOn"] = date;
            if (this.editedRowData.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            this.props.actions.UpdateTimesheetDocumentDetails(this.updatedData, this.editedRowData);
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
        // if (parent.props.timesheetDocuments && parent.props.timesheetDocuments.recordStatus !== "N") {
        //  this.updatedData["uploadedOn"] = date;             
        //  this.updatedData["recordStatus"] = "M";
        //  }     
        parent.props.actions.UpdateTimesheetDocumentDetails(this.updatedData, data);
        this.updatedData={}; 
    }
    updateChangesToGridReferesh=(oldfileNameFormat,data)=>{
        const parent = this;    
        const index = parent.props.timesheetDocuments.findIndex(document => document.id === data.id);
        const propvalue = parent.props.timesheetDocuments[index].documentName.split(/\.(?=[^\.]+$)/)[1];
        if(propvalue === undefined){
            const obj = { ...parent.props.timesheetDocuments[index] };
            obj.documentName = parent.props.timesheetDocuments[index].documentName+'.'+oldfileNameFormat;                  
            this.child.setData(obj,(index));
        }else{
            this.child.setData(parent.props.timesheetDocuments[index],index);
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
            timesheetDocuments, 
            timesheetDocTypes,
            interactionMode } = this.props;

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
        bindAction(this.headerData, "EditTimesheetDocuments", this.editRowHandler);
        bindAction(this.headerData, "isVisibleToCustomer", (e,data) =>  this.customerVisibleSwitchChangeHandler(e,data) );     
        for(let i = 0; i < timesheetDocuments.length; i++){
            timesheetDocuments[i] = { ...timesheetDocuments[i],roleBase: ((this.props.interactionMode && this.props.pageMode === localConstant.commonConstants.VIEW)),
                isFileUploaded: ((isUndefined(timesheetDocuments[i].documentUniqueName))?false:true),
                isDisableStatus:(this.props.timesheetInfo.timesheetStatus === 'O'? (this.props.isOperatorCompany ? true : false) : (this.props.timesheetInfo.timesheetStatus === 'A' ? (this.props.isCoordinatorCompany ? false : true) : false)), // Defect Id 948 isOperatorApporved  - when the Timesheet is Approved and existing  Upload Document Switch option should be disabled
                // isDisableStatus:this.props.isApproved_OC_CHC, // Defect Id 948 isOperatorApporved  - when the visit is Approved and existing  Upload Document Switch option should be disabled
                //isCheckBoxstatus:this.props.timesheetInfo.timesheetStatus === 'O' ? true :false //Defect Id 948 isOperatorApporved and CHC
                
            };

        } 
        return (
            <div className="customCard">
            {/* <CustomModal modalData={modelData} /> */}
                <h6 className="bold col s3 pl-0 pr-3">{localConstant.documents.DOCUMENTS}</h6>
                    <TimesheetDocumentMenuButtons
                        rowData={timesheetDocuments && timesheetDocuments.length > 0 ? timesheetDocuments.filter(record => record.recordStatus !== "D" && record.subModuleRefCode === '0') : []} //D979 - Filter only document uploaded, not approved documents
                        headerData={this.headerData}
                        copyDocumentHandler={this.copyDocumentHandler}
                        pasteDocumentHandler={this.pasteDocumentHandler}
                        uploadDocumentHandler={this.uploadDocumentHandler}
                        deleteDocumentHandler={this.deleteDocumentHandler}
                        DownloadDocumentHandler={this.documentmultipleDownloadHandler}
                        interactionMode={!interactionMode}
                        onCellchange={this.onCellchange}
                        supportFileType={configuration} 
                        pageMode={this.props.pageMode}
                        onRef={ref => { this.child = ref; }}
                        isDeleteDocument = {this.props.iInterCompanyAssignment && this.props.isOperatorCompany && (this.props.timesheetInfo.timesheetStatus === 'O' || this.props.timesheetInfo.timesheetStatus === 'A') ? false : true}
                    />
                    {this.state.isDocumentModalOpen &&
                    <Modal id="timesheetDocModalPopup"
                        title={this.state.isDocumentModelAddView
                             ? localConstant.documents.UPLOAD_DOC : localConstant.documents.EDIT_DOC}
                        modalClass="timesheetDocModal"
                        buttons={this.documentAddButtons}
                        isShowModal={this.state.isDocumentModalOpen}>

                        {this.state.isDocumentModelAddView ? <ModalPopupAddView
                            timesheetDocumentType={timesheetDocTypes}
                            handlerChange={(e) => this.inputHandleChange(e)}
                            supportFileType={configuration.allowedFileFormats}  />
                            : <ModalPopupEditView
                                timesheetDocumentType={timesheetDocTypes}
                                handlerChange={(e) => this.inputHandleChange(e)}
                                documentType={this.editedRowData.documentType}
                                documentName={this.editedRowData.documentName}
                                isVisibleToCustomer={this.editedRowData.isVisibleToCustomer}
                                isVisibleToTS={this.editedRowData.isVisibleToTS}
                                />}
                    </Modal>}
            </div>
        );
    }
}

export default Documents;
