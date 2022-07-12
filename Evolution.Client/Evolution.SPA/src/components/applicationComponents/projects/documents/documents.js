import React, { Component, Fragment } from 'react';
import { HeaderData, ContractHeaderData, CustomerHeaderData } from '../documents/headerData.js';
import RelatedDocuments from '../../../documents/relatedDocuments';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import Panel from '../../../../common/baseComponents/panel';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { getlocalizeData, bindAction, isEmpty, formInputChangeHandler,isUndefined,uploadedDocumentCheck } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import CustomModal from '../../../../common/baseComponents/customModal';
import Modal from '../../../../common/baseComponents/modal';
import { configuration } from '../../../../appConfig';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import  ProjectDocumentMenuButtons  from '../../../documents/documents';
import { MultipleDownload } from '../../../documents/documentsMultidownload';
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
                //required={true}
                className="customInputs browser-default"
                optionsList={props.projectDocumentTypeData}
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

            <div className="row">
                {/* <CustomInput
                    type='switch'
                    switchLabel={localConstant.modalConstant.VISIBLE_TO_CUSTOMER}
                    isSwitchLabel={true}
                    switchName="isVisibleToCustomer"
                    id="isVisibleToCustomer"
                    colSize="s3 m4 mt-4"
                    checkedStatus={props.isVisibleToCustomer}
                    onChangeToggle={props.handlerChange}
                /> */}
            </div>

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
                //required={true}
                className="customInputs browser-default"
                optionsList={props.projectDocumentTypeData}
                optionName='name'
                optionValue="name"
                onSelectChange={props.handlerChange}
                name="documentType"
                id="documentType"
                defaultValue={props.eidtedProjectDocuments.documentType}
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
                defaultValue={props.eidtedProjectDocuments.documentName}
            />

            <div className="row">
                {/* <CustomInput
                    type='switch'
                    switchLabel={localConstant.modalConstant.VISIBLE_TO_CUSTOMER}
                    isSwitchLabel={true}
                    switchName="isVisibleToCustomer"
                    id="isVisibleToCustomerEdit"
                    colSize="s3 m4 mt-4"
                    checkedStatus={props.eidtedProjectDocuments.isVisibleToCustomer}
                    onChangeToggle={props.handlerChange}
                /> */}
            </div>
        </Fragment>
    );
};
class Documents extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isDocumentModalOpen: false,
            isDocumentModelAddView: true,
            isPanelOpen: false,
            isContractPanelOpen:false
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
         const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        if(tabInfo && tabInfo.componentRenderCount === 0){
            this.props.actions.FetchCustomerDocumentsofProject();
            this.props.actions.FetchContractDocumentsofProject();
        }

        // this.props.actions.FetchProjectDocumentTypes();
      
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
                this.props.projectDocumentTypeData.map(doc => {
                    if (doc.name === docType) {
                        visibleToTSStatus = doc.isTSVisible;
                    }
                });

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
                            moduleCode: "PRJ",
                            requestedBy: this.props.loggedInUser,
                            // isUploaded:false
                            // moduleCodeReference: this.props.companyInfo.companyCode // D1186 & D1188
                        };
                        this.updatedData = {
                            documentName: filesToBeUpload[i].name,
                            moduleCode: "PRJ",
                            requestedBy: this.props.loggedInUser,
                            //moduleCodeReference: this.props.companyInfo.companyCode, // D1186 & D1188
                            documentType: docType,
                            documentSize: filesToBeUpload[i].size == 0 ? 0 : parseInt(filesToBeUpload[i].size / 1024) == 0 ? 1 : parseInt(filesToBeUpload[i].size / 1024),//def1126
                            createdOn: date,
                            isVisibleToCustomer: customerVisibleStatus,
                            isVisibleToTS: visibleToTSStatus,
                            recordStatus: "N",
                            id: Math.floor(Math.random() * (Math.pow(10, 5))),
                            modifiedBy: this.props.loggedInUser,
                            status: "C",
                            newlyAddedRecord: true,
                            canDelete: true,
                            isUploaded: false
                        };
                        documentUniqueName.push(this.uniqueName);
                        createdNewRecords.push(this.updatedData);
                        this.props.actions.AddProjectDocumentDetails(createdNewRecords);
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
                    IntertekToaster(failureFiles.toString() + localConstant.project.documents.FILE_LIMIT_EXCEDED, 'warningToast contractDocSizeReq');
                }

            }
            else {
                IntertekToaster(localConstant.project.documents.NO_FILE_SELECTED, 'warningToast contractNoFileSelectedReq');
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
                            moduleCodeReference: this.props.projectMode === "createProject" ? 0 : this.props.selectedProjectNo,   // D1186 & D1188        
                            canDelete: true,
                            isUploaded: true
                        };
                    }
                    this.props.actions.UpdateProjectDocumentDetails(updatedDataAfterUploaded, this.UploadedFile[0]);
                }
            }
        }
        else {
            const afterRemovefailedDoc = this.props.documentrowData.filter(x => x.documentName !== uniqueNameDetail.documentName);
            this.props.actions.UpdateProjectDocumentDetails(afterRemovefailedDoc);
        }

        if (this.props.documentrowData.length > 0) {
            const queueDocs = this.props.documentrowData.find(doc => doc.isUploaded === false);
            if (queueDocs) {
                const tempUniqueName = {
                    documentName: queueDocs.documentName,
                    moduleCode: "PRJ",
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
    this.props.actions.UpdateProjectDocumentDetails(this.updatedData, this.editedRowData);
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
        if (selectedRecords.length > 0) {
            this.copiedDocumentDetails = selectedRecords;
            IntertekToaster(localConstant.contract.SELECTED_RECORDS_COPIED, 'warningToast projDocumentCopyMesgReq');
        }
        else if (selectedRecordsFromSecondChild.length > 0) {
            this.copiedDocumentDetails = selectedRecordsFromSecondChild;
            IntertekToaster(localConstant.contract.SELECTED_RECORDS_COPIED, 'warningToast projDocumentSecndCopyMesgReq');
        }
        else if (selectedRecordsFromThirdChild.length > 0) {
            this.copiedDocumentDetails = selectedRecordsFromThirdChild;
            IntertekToaster(localConstant.contract.SELECTED_RECORDS_COPIED, 'warningToast projDocumentThrdCopyMesgReq');
        }
        else {
            IntertekToaster(localConstant.project.documents.SELECT_RECORDS_TO_COPY, 'warningToast projDocumentCopyReq');
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
                record.moduleCode = "PRJ";
                delete record.moduleRefCode;
                //record.moduleRefCode =  this.props.projectMode === "createProject" ? 0 : this.props.selectedProjectNo;
            });
            this.props.actions.PasteDocumentUploadData(recordToPaste)
                .then(response => {
                    if (!isEmpty(response)) {
                        for (let i = 0; i < response.length; i++) {
                            selectType = this.props.projectDocumentTypeData.filter(Type=>{ if(Type.name === response[i].documentType)return Type; });
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
                        this.props.actions.AddProjectDocumentDetails(records);
                    }
                });
        }
        else {
            IntertekToaster(localConstant.project.documents.COPY_RECORDS_TO_PASTE, 'warningToast documentRecordToPasteReq');
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
            IntertekToaster(localConstant.project.documents.SELECT_ATELEAST_ONE_DOC_TO_DELETE, 'warningToast oneDocumentReq');
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
                this.props.actions.DeleteProjectDocumentDetails(selectedRecords);
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
    contractPanelClick = (e) => {
        this.setState({
            isContractPanelOpen: !this.state.isContractPanelOpen
        });
    }

    uploadDocumentSubmitHandler = (e) => {
        e.preventDefault();
        const documentFile = Array.from(document.getElementById("uploadFiles").files);
         if (isEmpty(this.updatedData.documentType)) {
            IntertekToaster(localConstant.project.documents.SELECT_FILE_TYPE, 'warningToast contractDocFileTypeReq');
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
        //         this.props.projectDocumentTypeData.map(doc => {
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
        //                     moduleCode: "PRJ",
        //                     requestedBy: this.props.loggedInUser,
        //                     moduleCodeReference: this.props.projectMode === "createProject" ? 0 : this.props.selectedProjectNo,
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
        //                             this.props.actions.AddProjectDocumentDetails(newlyCreatedRecords);
        //                         }
        //                     };
        //                 });
        //         }
        //         if (failureFiles.length > 0) {
        //             IntertekToaster(failureFiles.toString() + localConstant.project.documents.FILE_LIMIT_EXCEDED, 'warningToast contractDocSizeReq');
        //         }

        //     }
        //     else {
        //         IntertekToaster(localConstant.project.documents.NO_FILE_SELECTED, 'warningToast contractNoFileSelectedReq');
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
            IntertekToaster(localConstant.project.documents.SELECT_FILE_TYPE, 'warningToast contractFileTypeReq');
            return false;
        }
        else {
            if (this.updatedData.documentType) {
                this.props.projectDocumentTypeData.map(doc => {
                    if (doc.name === this.updatedData.documentType) {
                        this.updatedData["isVisibleToTS"] = doc.isTSVisible;
                    }
                });
            }
            this.updatedData["createdOn"] = date;
            if (this.editedRowData.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            this.props.actions.UpdateProjectDocumentDetails(this.updatedData, this.editedRowData);
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
    documentMultipleDownloadHandlerforTC = (e) =>
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
        // if (parent.props.editProjectDocumentDetails && parent.props.editProjectDocumentDetails.recordStatus !== "N") {
        //  this.updatedData["uploadedOn"] = date;             
        //  this.updatedData["recordStatus"] = "M";
        //  }     
        parent.props.actions.UpdateProjectDocumentDetails(this.updatedData, data);
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
            projectContractDocumentsData,
            projectCustomerDocumentsData,
            projectDocumentTypeData,
            documentrowData,
            interactionMode
        } = this.props;
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
        bindAction(this.headerData, "EditProjectDocuments", this.editRowHandler);
        bindAction(this.headerData, "isVisibleToCustomer", (e,data) =>  this.customerVisibleSwitchChangeHandler(e,data) );
        for(let i = 0; i < documentrowData.length; i++){
            documentrowData[i] = { ...documentrowData[i],roleBase: ((this.props.interactionMode && this.props.pageMode === localConstant.commonConstants.VIEW)) };
            documentrowData[i] = { ...documentrowData[i],isFileUploaded: ((isUndefined(documentrowData[i].documentUniqueName))?false:true) };
        }  
        return (
            <Fragment>
                <div className="customCard">
                    <h6 className="bold col s3 pl-0 pr-3">{localConstant.documents.PROJECT_DOCUMENTS}</h6>
                    <ProjectDocumentMenuButtons
                    rowData={documentrowData && documentrowData.length > 0 ? documentrowData.filter(record => record.recordStatus !== "D") : []}
                    headerData={this.headerData}
                    copyDocumentHandler={this.copyDocumentHandler}
                    pasteDocumentHandler={this.pasteDocumentHandler}
                    uploadDocumentHandler={this.uploadDocumentHandler}
                    deleteDocumentHandler={this.deleteDocumentHandler}
                    DownloadDocumentHandler={this.documentmultipleDownloadHandler}
                    interactionMode={!this.props.interactionMode}
                    onCellchange={this.onCellchange} 
                    pageMode={this.props.pageMode}
                    supportFileType={configuration}
                    paginationPrefixId={localConstant.paginationPrefixIds.projectDoc}
                    onRef={ref => { this.child = ref; }}
                    />
                </div>
                {this.state.isDocumentModalOpen &&
                    <Modal id="projectDocModalPopup"
                        title={this.state.isDocumentModelAddView ? localConstant.project.documents.UPLOAD_DOC : localConstant.project.documents.EDIT_DOC}
                        modalClass="projectDocModal"
                        buttons={this.documentAddButtons}
                        isShowModal={this.state.isDocumentModalOpen}>

                        {this.state.isDocumentModelAddView ? <ModalPopupAddView
                            projectDocumentTypeData={projectDocumentTypeData}
                            handlerChange={(e) => this.formInputChangeHandler(e)}
                            supportFileType={configuration}
                        />
                            : <ModalPopupEditView
                                projectDocumentTypeData={projectDocumentTypeData}
                                handlerChange={(e) => this.formInputChangeHandler(e)}
                                eidtedProjectDocuments={this.editedRowData}
                            />}
                    </Modal>}
                <Panel colSize="s12" heading={localConstant.documents.CUSTOMER_DOCUMENTS}
                    className="pl-0 pr-0 mt-3 documentPanel bold"
                    onpanelClick={this.panelClick}
                    isopen={this.state.isPanelOpen} isArrow={true} >
                    <RelatedDocuments DocumentsData={projectCustomerDocumentsData}
                        headerData={CustomerHeaderData}
                        interactionMode={interactionMode}
                        title={localConstant.documents.CUSTOMER_DOCUMENTS}
                        copyRecord={this.copyDocumentHandler}
                        DownloadDocumentHandler={this.documentMultipleDownloadHandlerforSC}
                        pageMode={this.props.pageMode}
                        supportFileType={configuration}
                        onRef={ref => { this.secondChild = ref; }}
                        paginationPrefixId={localConstant.paginationPrefixIds.projectCustomerDoc}
                    />
                </Panel>
                <Panel colSize="s12" heading={localConstant.documents.CONTRACT_DOCUMENTS}
                    className="pl-0 pr-0  documentPanel bold"
                    onpanelClick={this.contractPanelClick}
                    isopen={this.state.isContractPanelOpen} isArrow={true} >
                    <RelatedDocuments DocumentsData={projectContractDocumentsData}
                        headerData={ContractHeaderData}
                        interactionMode={interactionMode}
                        title={localConstant.documents.CONTRACT_DOCUMENTS}
                        copyRecord={this.copyDocumentHandler}
                        DownloadDocumentHandler={this.documentMultipleDownloadHandlerforTC}
                        pageMode={this.props.pageMode}
                        supportFileType={configuration}
                        onRef={ref => { this.thirdChild = ref; }}
                        paginationPrefixId={localConstant.paginationPrefixIds.projectContractDoc}
                    />
                </Panel>
                <CustomModal modalData={modelData} />
            </Fragment>
        );
    }
}

export default Documents;