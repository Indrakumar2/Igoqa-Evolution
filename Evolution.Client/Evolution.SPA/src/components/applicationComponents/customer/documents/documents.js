import React, { Component, Fragment } from 'react';
import PropTypes from 'proptypes';
import { DocumentsHeaderData, ContractsHeaderData, ProjectHeaderData } from './headerData';
import dateUtil from '../../../../utils/dateUtil';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import { configuration } from '../../../../appConfig';
import Documents from '../../../documents';
import RelatedDocuments from '../../../documents/relatedDocuments';
import Panel from '../../../../common/baseComponents/panel';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { getlocalizeData, isEmpty, bindAction, formInputChangeHandler,isUndefined,uploadedDocumentCheck } from '../../../../utils/commonUtils';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import Modal from '../../../../common/baseComponents/modal';
import { MultipleDownload } from '../../../documents/documentsMultidownload';
const localConstant = getlocalizeData();

const ModalPopupAddView = (props) => {
    return (
        <Fragment>
            <CustomInput
                hasLabel={true}
                label={localConstant.companyDetails.Documents.SELECT_FILE_TYPE}
                divClassName='s6 pl-0'
                type='select'
                labelClass="mandate"
                required={true}
                colSize="s6"
                selected={true}
                className="customInputs browser-default"
                optionsList={props.masterDocumentTypesData}
                optionName='name'
                optionValue="name"
                onSelectChange={props.handlerChange}
                name="documentType"
                id="documentType"
            />
            {/* <CustomInput
                type='switch'
                switchLabel={localConstant.modalConstant.VISIBLE_TO_CUSTOMER}
                isSwitchLabel={true}
                switchName="isVisibleToCustomer"
                id="isVisibleToCustomer"
                className="lever"
                colSize="s6 mt-4"
                onChangeToggle={props.handlerChange}
                checkedStatus={props.isVisibleToCustomer}
            /> */}
            <div className="col s12 pl-0 mt-3">
                <div className="file-field">
                    <div className="btn">
                        <i className="zmdi zmdi-upload zmdi-hc-lg"></i>
                        <input id="uploadFiles" type="file" accept={props.supportFileType.allowedFileFormats} multiple required />
                    </div>
                    <div className="file-path-wrapper">
                        <input className="file-path validate"
                            placeholder="Upload multiple files" />
                    </div>
                </div>
            </div>
        </Fragment>
    );
};

const ModalPopupEditView = (props) => {
    return (
        <Fragment>
            <CustomInput
                hasLabel={true}
                label={localConstant.companyDetails.Documents.SELECT_FILE_TYPE}
                divClassName='s6 pl-0'
                type='select'
                labelClass="mandate"
                colSize="s6"
                required={true}
                selected={true}
                className="customInputs browser-default"
                optionsList={props.masterDocumentTypesData}
                optionName='name'
                optionValue="name"
                onSelectChange={props.handlerChange}
                name="documentType"
                id="documentType"
                defaultValue={props.eidtedCustomerDocuments.documentType}
            />
            {/* <CustomInput
                type='switch'
                switchLabel={localConstant.modalConstant.VISIBLE_TO_CUSTOMER}
                isSwitchLabel={true}
                switchName="isVisibleToCustomer"
                id="isVisibleToCustomer"
                className="lever"
                colSize="s6 mt-4"
                onChangeToggle={props.handlerChange}
                checkedStatus={props.eidtedCustomerDocuments.isVisibleToCustomer}
            /> */}
            <CustomInput
                hasLabel={true}
                divClassName='pl-0'
                label={localConstant.companyDetails.Documents.DOCUMENT_NAME}
                type='text'
                colSize='s12'
                inputClass="customInputs"
                labelClass="mandate"
                name="documentName"
                readOnly={true}
                defaultValue={props.eidtedCustomerDocuments.documentName}
            />
        </Fragment>
    );
};
class CustomerDocuments extends Component {
    constructor(props) {
        super(props);
        this.updatedData = {};
        this.filesToDisplay = [];
        this.editedRowData = {};
        this.state = {
            isDocumentModalOpen: false,
            isDocumentModelAddView: true,
            isPanelOpen: false,
            isProjectPanelOpen:false
        };
        this.confirmationModalData = {
            title: "",
            message: "",
            type: "",
            modalClassName: "",
            buttons: []
        };
        //Defect 691-issue 4 Changes -Start
        this.functionRef={};
        this.functionRef['isDisable']=(this.props.pageMode === localConstant.commonConstants.VIEW);
        this.headerData=DocumentsHeaderData(this.functionRef);
        //Defect 691-issue 4 Changes -End
        bindAction(this.headerData, "EditCustomerDocuments", this.editRowHandler);
        bindAction(this.headerData, "isVisibleToCustomer", (e,data) =>  this.customerVisibleSwitchChangeHandler(e,data) );
    }

    componentDidMount() {
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        if(tabInfo && tabInfo.componentRenderCount === 0){
                    this.props.actions.FetchCustomerContractDocuments();
                    this.props.actions.FetchCustomerProjectDocuments();
        }
        //commented, Now we are getting FetchCountry and customer Documents from master data reducer
        // this.props.actions.FetchMasterDocumentTypes();
        if(!(this.props.interactionMode && this.props.pageMode === localConstant.commonConstants.VIEW)){
        //Drag and Drop File Upload
        this.dropArea = document.getElementById("drop-area");
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
            const queueDocs = this.props.DocumentsData.find(doc => doc.isUploaded === false);
        const isQueuePending=queueDocs?true:false;
            if (documentFile.length > 0) {
                const newlyCreatedRecords = [];
                const filesToBeUpload = [];
                const failureFiles = [];
                const failureFormat=[];
                const documentUniqueName = [];
                let createdNewRecords =[];
                const customerVisibleStatus = this.updatedData.isVisibleToCustomer ? this.updatedData.isVisibleToCustomer : false;
                let visibleToTSStatus = false;
                this.props.masterDocumentTypesData.map(doc => {
                    if (doc.name === docType) {
                        visibleToTSStatus = doc.isTSVisible;
                    }
                });
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
                            moduleCode: "CUST",
                            requestedBy: this.props.loggedInUser,
                            // isUploaded:false
                            // moduleCodeReference: this.props.companyInfo.companyCode // D1186 & D1188
                        };
                        this.updatedData = {
                            documentName: filesToBeUpload[i].name,
                            moduleCode: "CUST",
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
                        this.props.actions.AddDocumentDetails(createdNewRecords);
                        // const tempFilesToBeUpload=Object.assign([],this.props.fileToBeUploaded);
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
                    IntertekToaster(failureFiles.toString() + localConstant.customer.documents.FILE_LIMIT_EXCEDED, 'warningToast contractDocSizeReq');
                }
            }
            else {
                IntertekToaster(localConstant.customer.documents.NO_FILE_SELECTED, 'warningToast contractNoFileSelectedReq');
            }
    }

    recursiveFileUpload = async (uniqueNameDetail, filesToBeUpload) => {
        const response = await this.props.actions.FetchDocumentUniqueName([ uniqueNameDetail ], filesToBeUpload);
        // .then(response => {                     
        if (!isEmpty(response)) {
            for (let i = 0; i < response.length; i++) {
                if (response[i] !== null) {
                    let updatedDataAfterUploaded = {};
                    if (!response[i].status) {
                        this.UploadedFile = this.props.DocumentsData.filter(x => x.documentName == response[i].fileName && x.documentUniqueName === undefined);

                        updatedDataAfterUploaded = {
                            recordStatus: "N",
                            documentUniqueName: response[i].uploadedFileName,
                            moduleCodeReference: this.props.customerInfo.customerCode, // D1186 & D1188     
                            canDelete: true,
                            isUploaded: true
                        };
                    }
                    this.props.actions.UpdateDocumentDetails(updatedDataAfterUploaded, this.UploadedFile[0]);
                }
            }
        }
        else {
            const afterRemovefailedDoc = this.props.DocumentsData.filter(x => x.documentName !== uniqueNameDetail.documentName);
            this.props.actions.UpdateDocumentDetails(afterRemovefailedDoc);
        }

        if (this.props.DocumentsData.length > 0) {
            const queueDocs = this.props.DocumentsData.find(doc => doc.isUploaded === false);
            if (queueDocs) {
                const tempUniqueName = {
                    documentName: queueDocs.documentName,
                    moduleCode: "CUST",
                    requestedBy: this.props.loggedInUser,
                };
                this.recursiveFileUpload(tempUniqueName, this.props.fileToBeUploaded);
            }
        }
    };

    editRowHandler = (data) => {
        this.setState({
            isDocumentModalOpen: true,
            isDocumentModelAddView: false,
            isbtnDisable: false
        });
        this.editedRowData = data;
    }

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
        this.props.actions.UpdateDocumentDetails(this.updatedData, this.editedRowData);
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

    uploadDocumentSubmitHandler = (e) => {
        e.preventDefault();
        const documentFile = Array.from(document.getElementById("uploadFiles").files);
        if(!this.updatedData.documentType){
            IntertekToaster(localConstant.customer.documents.SELECT_FILE_TYPE, 'warningToast contractDocFileTypeReq');
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
        //         const customerVisibleStatus = this.updatedData.isVisibleToCustomer ? this.updatedData.isVisibleToCustomer : false;
        //         let visibleToTSStatus = false;
        //         this.props.masterDocumentTypesData.map(doc => {
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
        //                     moduleCode: "CUST",
        //                     requestedBy: this.props.loggedInUser,
        //                     moduleCodeReference: this.props.customerInfo.customerCode,
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
        //                                     moduleCode: "CUST",
        //                                     moduleRefCode: this.props.customerInfo.customerCode,
        //                                     status: 'C',
        //                                     newlyAddedRecord:true
        //                                 };
        //                                 newlyCreatedRecords.push(this.updatedData);
        //                                 this.updatedData = {};
        //                             }
        //                         }
        //                         if (newlyCreatedRecords.length > 0) {
        //                             const addModal = {};
        //                             addModal.showModalPop = false;
        //                             this.props.actions.AddDocumentDetails(newlyCreatedRecords);
        //                             this.updatedData = {};
        //                         }
        //                     };
        //                 });
        //         }
        //         if (failureFiles.length > 0) {
        //             IntertekToaster(failureFiles.toString() + localConstant.customer.documents.FILE_LIMIT_EXCEDED, 'warningToast contractDocSizeReq');
        //         }
        //     }
        //     else {
        //         IntertekToaster(localConstant.customer.documents.NO_FILE_SELECTED, 'warningToast contractNoFileSelectedReq');
        //     }
       // }
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
            IntertekToaster(localConstant.customer.documents.SELECT_FILE_TYPE, 'warningToast CustDocUpdtDocTypChk');
        }
        else {
            if (this.updatedData.documentType) {
                const docTypeData = this.props.masterDocumentTypesData;
                for (let i = 0; i < docTypeData.length; i++) {
                    if (docTypeData[i].name === this.updatedData.documentType) {
                        this.updatedData["isVisibleToTS"] = docTypeData[i].isTSVisible;
                    }
                }
            }
            this.updatedData["uploadedOn"] = date;
            this.updatedData["modifiedBy"] = this.props.loggedInUser;
            if (this.props.editDocumentDetails.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            this.props.actions.UpdateDocumentDetails(this.updatedData, this.editedRowData);
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

    copyRecord = () => {
        const selectedRecords = this.child.getSelectedRows();
        const selectedRecordsfromSc = this.secondChild.getSelectedRows();
        const selectedRecordsfromTc = this.thirdChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            this.props.actions.CopyDocumentDetails(selectedRecords);
            IntertekToaster(localConstant.customer.documents.SELECTED_RECORDS_COPIED, 'warningToast documentCopyMesgReq');
        }
        else if (selectedRecordsfromSc.length > 0) {
            this.props.actions.CopyDocumentDetails(selectedRecordsfromSc);
            IntertekToaster(localConstant.customer.documents.SELECTED_RECORDS_COPIED, 'warningToast documentCopyMesgSecndReq');
        }
        else if (selectedRecordsfromTc.length > 0) {
            this.props.actions.CopyDocumentDetails(selectedRecordsfromTc);
            IntertekToaster(localConstant.customer.documents.SELECTED_RECORDS_COPIED, 'warningToast documentCopyMesgThrdReq');
        }
        else {
            IntertekToaster(localConstant.customer.documents.SELECT_RECORDS_TO_COPY, 'warningToast documentCopyReq');
        }
    }

    pasteRecord = () => {
        const records = [];
        let recordToPaste = [];
        let selectType=[];
        recordToPaste = this.props.copiedDocumentDetails;
        if (recordToPaste.length > 0) {
            let date = new Date();
            date = dateUtil.postDateFormat(date, '-');
            recordToPaste.map((record) => {
                record.moduleCode = "CUST";
                delete record.moduleRefCode;
            });
            this.props.actions.PasteDocumentUploadData(recordToPaste)
                .then(response => {
                    if (!isEmpty(response)) {
                        for (let i = 0; i < response.length; i++) {
                            selectType = this.props.masterDocumentTypesData.filter(Type=>{ if(Type.name === response[i].documentType)return Type; });
                            this.updatedData = {
                                documentName: response[i].documentName,
                                documentType:selectType.length > 0 ? selectType[0].name : '',
                                //documentType: recordToPaste[i].documentType,
                                documentSize: recordToPaste[i].documentSize,
                                createdOn: date,
                                isVisibleToCustomer: recordToPaste[i].isVisibleToCustomer,
                                isVisibleToTS: recordToPaste[i].isVisibleToTS,
                                recordStatus: "N",
                                id: Math.floor(Math.random() * (Math.pow(10, 5))),
                                documentUniqueName: response[i].uniqueName,
                                modifiedBy: this.props.loggedInUser,
                                moduleCode: response[i].moduleCode,
                                moduleRefCode: response[i].moduleCodeReference,
                                status: "C",
                                canDelete:true
                            };
                            records.push(this.updatedData);
                            this.updatedData = {};
                        }
                        this.props.actions.AddDocumentDetails(records);
                    }
                });
        }
        else {
            IntertekToaster(localConstant.customer.documents.COPY_RECORDS_TO_PASTE, 'warningToast documentRecordToPasteReq');
        }
    }

    documentDeleteClickHandler = () => {
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
        else if (selectedRecords.length > 0) {
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
                            onClickHandler: this.deleteRecord,
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
        else {
            IntertekToaster(localConstant.customer.documents.SELECT_ATELEAST_ONE_DOC_TO_DELETE, 'warningToast CustDocDelDocChk');
        }
    }
    deleteRecord = () => {
        let selectedRecords = this.child.getSelectedRows();
        selectedRecords = selectedRecords.filter(x => x.canDelete == true);
        if (selectedRecords.length > 0) {
            const isFileUploaded = uploadedDocumentCheck(selectedRecords);
            if (isFileUploaded) {
                this.child.removeSelectedRows(selectedRecords);
                this.props.actions.DeleteDocumentDetails(selectedRecords);
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
    renameConfirmationRejectHandler = (e,fileName,data) => {
        e.preventDefault();
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
        // if (parent.props.editCompanyDocumentDetails && parent.props.editCompanyDocumentDetails.recordStatus !== "N") {
        //  this.updatedData["uploadedOn"] = date;             
        //  this.updatedData["recordStatus"] = "M";
        //  }     
        parent.props.actions.UpdateDocumentDetails(this.updatedData, data);
        this.updatedData={}; 
    }
    updateChangesToGridReferesh=(oldfileNameFormat,data)=>{
        const parent = this;    
        const index = parent.props.DocumentsData.findIndex(document => document.id === data.id);
        const propvalue = parent.props.DocumentsData[index].documentName.split(/\.(?=[^\.]+$)/)[1];
        if(propvalue === undefined){
            const obj = { ...parent.props.DocumentsData[index] };
            obj.documentName = parent.props.DocumentsData[index].documentName+'.'+oldfileNameFormat;                  
            this.child.setData(obj,(index));
        }else{
            this.child.setData(parent.props.DocumentsData[index],index);
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
                        onClickHandler: (e) =>this.renameConfirmationRejectHandler(e,data.oldValue,data.data),
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
            masterDocumentTypesData,
            DocumentsData,
            contractDocumentsData,
            projectDocumentsData
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
        let customerDocumentsData = [];
        if (DocumentsData) {
            customerDocumentsData = DocumentsData.filter(document => document.recordStatus !== 'D');
        }
        for(let i = 0; i < customerDocumentsData.length; i++){
            customerDocumentsData[i] = { ...customerDocumentsData[i],roleBase: ((this.props.interactionMode && this.props.pageMode === localConstant.commonConstants.VIEW)) };
            customerDocumentsData[i] = { ...customerDocumentsData[i],isFileUploaded: ((isUndefined(customerDocumentsData[i].documentUniqueName))?false:true) };
        }  
        return (
            <Fragment>
                <div className="customCard">
                    <h6 className="bold col s3 pl-0 pr-3">{localConstant.documents.DOCUMENTS}</h6>
                    <Documents rowData={customerDocumentsData}
                        headerData={this.headerData}
                        copyDocumentHandler={this.copyRecord}
                        pasteDocumentHandler={this.pasteRecord}
                        uploadDocumentHandler={this.uploadDocumentHandler}
                        deleteDocumentHandler={this.documentDeleteClickHandler}
                        DownloadDocumentHandler={this.documentmultipleDownloadHandler}
                        interactionMode={true} 
                        onCellchange={this.onCellchange} 
                        pageMode={this.props.pageMode}
                        supportFileType={configuration}
                        paginationPrefixId={localConstant.paginationPrefixIds.customerDoc}
                        // customer doesnot have view  mode, so we are  sending interaction mode as true to display the buttons related to documents actions
                        onRef={ref => { this.child = ref; }} />
               
                {this.state.isDocumentModalOpen &&
                    <Modal id="customerDocModalPopup"
                        title={this.state.isDocumentModelAddView ? localConstant.project.documents.UPLOAD_DOC : localConstant.project.documents.EDIT_DOC}
                        modalClass="customerDocModal"
                        buttons={this.documentAddButtons}
                        isShowModal={this.state.isDocumentModalOpen}>

                        {this.state.isDocumentModelAddView ? <ModalPopupAddView
                            masterDocumentTypesData={masterDocumentTypesData}
                            handlerChange={(e) => this.formInputChangeHandler(e)}
                            supportFileType={configuration}
                        />
                            : <ModalPopupEditView
                                masterDocumentTypesData={masterDocumentTypesData}
                                handlerChange={(e) => this.formInputChangeHandler(e)}
                                eidtedCustomerDocuments={this.editedRowData}
                            />}
                    </Modal>}
                <Panel colSize="s12" heading={localConstant.documents.CONTRACT_DOCUMENTS}
                    className="pl-0 mt-2 documentPanel bold" onpanelClick={this.panelClick} isopen={this.state.isPanelOpen} isArrow={true} >
                    <RelatedDocuments DocumentsData={contractDocumentsData ? contractDocumentsData : []}
                        headerData={ContractsHeaderData}
                        title={localConstant.documents.CONTRACT_DOCUMENTS}
                        copyRecord={this.copyRecord}
                        DownloadDocumentHandler={this.documentMultipleDownloadHandlerforSC}
                        pageMode={this.props.pageMode}
                        paginationPrefixId={localConstant.paginationPrefixIds.customerContractDoc}
                        onRef={ref => { this.secondChild = ref; }} />
                </Panel>

                <Panel colSize="s12" heading={localConstant.documents.PROJECT_DOCUMENTS}
                    className="pl-0  documentPanel bold" onpanelClick={this.projectPanelClick} isopen={this.state.isProjectPanelOpen}  isArrow={true} >
                    <RelatedDocuments DocumentsData={projectDocumentsData ? projectDocumentsData : []}
                        headerData={ProjectHeaderData}
                        title={localConstant.documents.PROJECT_DOCUMENTS}
                        copyRecord={this.copyRecord}
                        DownloadDocumentHandler={this.documentMultipleDownloadHandlerforTC}
                        pageMode={this.props.pageMode}
                        paginationPrefixId={localConstant.paginationPrefixIds.customerProjectDoc}
                        onRef={ref => { this.thirdChild = ref; }}
                    />
                </Panel>
                </div>
            </Fragment>
        );
    }
}
CustomerDocuments.propTypes = {
    DocumentsData: PropTypes.array.isRequired,
    masterDocumentTypesData: PropTypes.array.isRequired,
};

CustomerDocuments.defaultprops = {
    DocumentsData: [],
    masterDocumentTypesData: [],
    isPanelOpen: false
};

export default CustomerDocuments;