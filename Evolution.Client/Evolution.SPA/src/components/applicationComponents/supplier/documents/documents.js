import React, { Component, Fragment } from 'react';
import { HeaderData } from '../documents/headerData.js';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { getlocalizeData, bindAction, isEmpty, formInputChangeHandler,isUndefined,uploadedDocumentCheck } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import CustomModal from '../../../../common/baseComponents/customModal';
import Modal from '../../../../common/baseComponents/modal';
import { configuration } from '../../../../appConfig';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import { MultipleDownload } from '../../../documents/documentsMultidownload';
const localConstant = getlocalizeData();
const SupplierDocumentMenuButtons = (props) => {
    return (
        <Fragment>
            <div className="row mb-0 pt-2" >
                <div className="center-align s3 offset-s9 right mr-3" >
                <a onClick={props.DownloadDocumentHandler} className="mr-3 waves-effect d-inline modal-trigger waves-effect waves-light">
                        <i className="zmdi zmdi-download zmdi-hc-lg"></i>
                        <span className="d-block">{localConstant.documents.DOWNLOAD}</span>
                    </a>
                    <a onClick={props.uploadDocumentHandler} className="mr-3 d-inline modal-trigger waves-effect waves-light">
                        <i className="zmdi zmdi-upload zmdi-hc-lg"></i>
                        <span className="d-block">{localConstant.documents.UPLOAD}</span>
                    </a>
                    <a onClick={props.copyDocumentHandler} className="mr-3 d-inline waves-effect waves-light">
                        <i className="zmdi zmdi-copy zmdi-hc-lg"></i>
                        <span className="d-block">{localConstant.documents.COPY}</span>
                    </a>
                    <a onClick={props.pasteDocumentHandler} className="mr-3 d-inline waves-effect waves-light">
                        <i className="zmdi zmdi-paste zmdi-hc-lg"></i>
                        <span className="d-block">{localConstant.documents.PASTE}</span>
                    </a>
                    <a onClick={props.deleteDocumentHandler} className="mr-3 d-inline waves-effect waves-light modal-trigger">
                        <i className="zmdi zmdi-delete zmdi-hc-lg danger-txt"></i>
                        <span className="d-block danger-txt">{localConstant.documents.DELETE}</span>
                    </a>
                </div>
            </div>
            <div id="drop-area">
                <form className="my-form">
                    <i className="zmdi zmdi-download"></i>
                    <p>{localConstant.commonConstants.DRAG_AND_DROP }</p>
                    <input type="file" id="fileElem" multiple accept={props.supportFileType.allowedFileFormats} onChange={()=>this.fileupload(this.files)}></input>
                    <label className="button" for="fileElem">Select some files</label>
                </form>               
            </div>
        </Fragment>
    );
};

const ModalPopupAddView = (props) => {
    return (
        <Fragment>

            <CustomInput hasLabel={true}
                label={localConstant.supplier.documents.SELECT_FILE_TYPE}
                divClassName='col pl-0 mb-4'
                colSize="s6"
                labelClass="mandate"
                type='select'
                //required={true}
                className="customInputs browser-default"
                optionsList={props.supplierDocumentTypeData}
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
{/* 
            <div className="row">
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
                label={localConstant.supplier.documents.SELECT_FILE_TYPE}
                divClassName='col pl-0'
                colSize="s6"
                type='select'
                //required={true}
                className="customInputs browser-default"
                optionsList={props.supplierDocumentTypeData}
                optionName='name'
                optionValue="name"
                onSelectChange={props.handlerChange}
                name="documentType"
                id="documentType"
                defaultValue={props.eidtedSupplierDocuments.documentType}
            />
            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.supplier.documents.DOCUMENT_NAME}
                type='text'
                colSize='s6'
                inputClass="customInputs"
                labelClass="mandate"
                name="documentName"
                disabled={true}
                defaultValue={props.eidtedSupplierDocuments.documentName}
            />

            {/* <div className="row">
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.modalConstant.VISIBLE_TO_CUSTOMER}
                    isSwitchLabel={true}
                    switchName="isVisibleToCustomer"
                    id="isVisibleToCustomerEdit"
                    colSize="s3 m4 mt-4"
                    checkedStatus={props.eidtedSupplierDocuments.isVisibleToCustomer}
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
          /** 
         * FetchSupplierDocumentTypes api call is commented, Now we are getting doc type from master data
        */
        // this.props.actions.FetchSupplierDocumentTypes();
        
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
                this.props.supplierDocumentTypeData.map(doc => {
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

                const currPage = this.props.currentPage;
                const selSup = this.props.selectedSupplierNo;
                if (failureFormat.length === 0 && filesToBeUpload.length > 0) {
                    for (let i = 0; i < filesToBeUpload.length; i++) {
                        this.uniqueName = {
                            documentName: filesToBeUpload[i].name,
                            moduleCode: "SUP",
                            requestedBy: this.props.loggedInUser,
                            // isUploaded:false
                            // moduleCodeReference: this.props.companyInfo.companyCode // D1186 & D1188
                        };
                        this.updatedData = {
                            documentName: filesToBeUpload[i].name,
                            moduleCode: "SUP",
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
                        this.props.actions.AddSupplierDocumentDetails(createdNewRecords);
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
                    IntertekToaster(failureFiles.toString() + localConstant.supplier.documents.FILE_LIMIT_EXCEDED, 'warningToast contractDocSizeReq');
                }

            }
            else {
                IntertekToaster(localConstant.supplier.documents.NO_FILE_SELECTED, 'warningToast contractNoFileSelectedReq');
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
                            moduleCodeReference: this.props.currentPage === localConstant.supplier.CREATE_SUPPLIER ? 0 : this.props.selectedSupplierNo,
                            canDelete: true,
                            isUploaded: true
                        };
                    }
                    this.props.actions.UpdateSupplierDocumentDetails(updatedDataAfterUploaded, this.UploadedFile[0]);
                }
            }
        }
        else {
            const afterRemovefailedDoc = this.props.documentrowData.filter(x => x.documentName !== uniqueNameDetail.documentName);
            this.props.actions.UpdateSupplierDocumentDetails(afterRemovefailedDoc);
        }

        if (this.props.documentrowData.length > 0) {
            const queueDocs = this.props.documentrowData.find(doc => doc.isUploaded === false);
            if (queueDocs) {
                const tempUniqueName = {
                    documentName: queueDocs.documentName,
                    moduleCode: "SUP",
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
        this.props.actions.UpdateSupplierDocumentDetails(this.updatedData, this.editedRowData);
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
            IntertekToaster(localConstant.supplier.documents.SELECTED_RECORDS_COPIED, 'warningToast projDocumentCopyMesgReq');
        }       
        else {
            IntertekToaster(localConstant.supplier.documents.SELECT_RECORDS_TO_COPY, 'warningToast projDocumentCopyReq');
        }
    }
    pasteDocumentHandler = () => {
        const records = [];
        let recordToPaste = []; 
        recordToPaste = this.copiedDocumentDetails;
        if (recordToPaste.length > 0) {
            let date = new Date();
            date = dateUtil.postDateFormat(date, '-');
            recordToPaste.map((record) => {
                record.moduleCode = "SUP";
                delete record.moduleRefCode;
                //record.moduleRefCode = this.props.selectedSupplierNo?this.props.selectedSupplierNo:'0';
            });
            this.props.actions.PasteDocumentUploadData(recordToPaste)
                .then(response => {
                    if (!isEmpty(response)) {
                        for (let i = 0; i < response.length; i++) {
                            this.updatedData = {
                                documentName: response[i].documentName,
                                documentType: recordToPaste[i].documentType, 
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
                        this.props.actions.AddSupplierDocumentDetails(records);
                    }
                });
        }
        else {
            IntertekToaster(localConstant.supplier.documents.COPY_RECORDS_TO_PASTE, 'warningToast documentRecordToPasteReq');
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
            IntertekToaster(localConstant.supplier.documents.SELECT_ATELEAST_ONE_DOC_TO_DELETE, 'warningToast oneDocumentReq');
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
                this.props.actions.DeleteSupplierDocumentDetails(selectedRecords);
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
    uploadDocumentSubmitHandler = (e) => {
        e.preventDefault();
        const documentFile = Array.from(document.getElementById("uploadFiles").files);
              if (isEmpty(this.updatedData.documentType)) {
            IntertekToaster(localConstant.supplier.documents.SELECT_FILE_TYPE, 'warningToast contractDocFileTypeReq');
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
        //         this.props.supplierDocumentTypeData.map(doc => {
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
        //         const currPage = this.props.currentPage;
        //         const selSup = this.props.selectedSupplierNo;
        //         if (filesToBeUpload.length > 0) {
        //             for (let i = 0; i < filesToBeUpload.length; i++) {
        //                 this.updatedData = {
        //                     documentName: filesToBeUpload[i].name,
        //                     moduleCode: "SUP",
        //                     requestedBy: this.props.loggedInUser,
        //                     moduleCodeReference: this.props.currentPage === localConstant.supplier.CREATE_SUPPLIER ? 0 : this.props.selectedSupplierNo,
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

        //                             for (let i = 0; i < response.length; i++) {
        //                                 if (!response[i].status) {
        //                                     this.updatedData = {
        //                                         documentName: response[i].fileName,
        //                                         documentType: docType,
        //                                         documentSize: parseInt(filesToBeUpload[i].size / 1024),
        //                                         createdOn: date,
        //                                         isVisibleToCustomer: customerVisibleStatus,
        //                                         isVisibleToTS: visibleToTSStatus,
        //                                         recordStatus: "N",
        //                                         id: Math.floor(Math.random() * (Math.pow(10, 5))),
        //                                         documentUniqueName: response[i].uploadedFileName,
        //                                         modifiedBy: this.props.loggedInUser,
        //                                         moduleCode: response[i].moduleCode,
        //                                         moduleRefCode: response[i].moduleReferenceCode,
        //                                         status: "C"
        //                                     };
        //                                     newlyCreatedRecords.push(this.updatedData);
        //                                     this.updatedData = {};
        //                                 }
        //                             }
        //                             if (newlyCreatedRecords.length > 0) {
        //                                 this.props.actions.AddSupplierDocumentDetails(newlyCreatedRecords);
        //                             }
                               
        //                     };
        //                 });
        //         }
        //         if (failureFiles.length > 0) {
        //             IntertekToaster(failureFiles.toString() + localConstant.supplier.documents.FILE_LIMIT_EXCEDED, 'warningToast contractDocSizeReq');
        //         }

        //     }
        //     else {
        //         IntertekToaster(localConstant.supplier.documents.NO_FILE_SELECTED, 'warningToast contractNoFileSelectedReq');
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
            IntertekToaster(localConstant.supplier.documents.SELECT_FILE_TYPE, 'warningToast contractFileTypeReq');
            return false;
        }
        else {
            if (this.updatedData.documentType) {
                this.props.supplierDocumentTypeData.map(doc => {
                    if (doc.name === this.updatedData.documentType) {
                        this.updatedData["isVisibleToTS"] = doc.isTSVisible;
                    }
                });
            }
            this.updatedData["createdOn"] = date;
            if (this.editedRowData.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            this.props.actions.UpdateSupplierDocumentDetails(this.updatedData, this.editedRowData);
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
        // if (parent.props.editSupplierDocumentDetails && parent.props.editSupplierDocumentDetails.recordStatus !== "N") {
        //  this.updatedData["uploadedOn"] = date;             
        //  this.updatedData["recordStatus"] = "M";
        //  }     
        parent.props.actions.UpdateSupplierDocumentDetails(this.updatedData, data);
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
            supplierDocumentTypeData,
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
        bindAction(this.headerData, "EditSupplierDocuments", this.editRowHandler);
        bindAction(this.headerData, "isVisibleToCustomer", (e,data) =>  this.customerVisibleSwitchChangeHandler(e,data) );
        for(let i = 0; i < documentrowData.length; i++){
            documentrowData[i] = { ...documentrowData[i],roleBase: ((this.props.interactionMode && this.props.pageMode === localConstant.commonConstants.VIEW)) };
            documentrowData[i] = { ...documentrowData[i],isFileUploaded: ((isUndefined(documentrowData[i].documentUniqueName))?false:true) };
        }  
        return (
            <Fragment>
                <div className="customCard">
                    <h6 className="bold col s3 pl-0 pr-3">{localConstant.documents.DOCUMENTS}</h6>
                    {!interactionMode && this.props.pageMode!==localConstant.commonConstants.VIEW && <SupplierDocumentMenuButtons
                        uploadDocumentHandler={this.uploadDocumentHandler}
                        copyDocumentHandler={this.copyDocumentHandler}
                        pasteDocumentHandler={this.pasteDocumentHandler}
                        deleteDocumentHandler={this.deleteDocumentHandler}
                        DownloadDocumentHandler={this.documentmultipleDownloadHandler}
                        supportFileType={configuration} />}
                    <CardPanel className="white lighten-4 black-text" colSize="s12">
                        <ReactGrid gridRowData={documentrowData && documentrowData.length > 0?documentrowData.filter(record => record.recordStatus !== "D"):[]}
                         gridColData={this.headerData} 
                         interactionMode={this.props.interactionMode}
                         onCellchange={this.onCellchange}
                         paginationPrefixId={localConstant.paginationPrefixIds.supplierDoc}
                         onRef={ref => { this.child = ref; }}
                          />
                    </CardPanel>
                </div>
                {this.state.isDocumentModalOpen &&
                    <Modal id="projectDocModalPopup"
                        title={this.state.isDocumentModelAddView ? localConstant.supplier.documents.UPLOAD_DOC : localConstant.supplier.documents.EDIT_DOC}
                        modalClass="projectDocModal"
                        buttons={this.documentAddButtons}
                        isShowModal={this.state.isDocumentModalOpen}>

                        {this.state.isDocumentModelAddView ? <ModalPopupAddView
                            supplierDocumentTypeData={supplierDocumentTypeData}
                            handlerChange={(e) => this.formInputChangeHandler(e)}
                            supportFileType={configuration}
                        />
                            : <ModalPopupEditView
                            supplierDocumentTypeData={supplierDocumentTypeData}
                                handlerChange={(e) => this.formInputChangeHandler(e)}
                                eidtedSupplierDocuments={this.editedRowData}
                            />}
                    </Modal>}               
                <CustomModal modalData={modelData} />
            </Fragment>
        );
    }
}

export default Documents;