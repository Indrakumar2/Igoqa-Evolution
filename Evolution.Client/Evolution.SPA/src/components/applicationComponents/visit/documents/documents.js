import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData, ChildHeaderData } from './documentHeader';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, bindAction, isEmpty,isUndefined,uploadedDocumentCheck } from '../../../../utils/commonUtils';
import  VisitDocumentMenuButtons  from '../../../documents/documents';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import dateUtil from '../../../../utils/dateUtil';
import Modal from '../../../../common/baseComponents/modal';
import CustomModal from '../../../../common/baseComponents/customModal';
import { configuration } from '../../../../appConfig';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import { visitTabDetails } from '../../../viewComponents/visit/visitDetails/visitTabsDetails';
import { MultipleDownload } from '../../../documents/documentsMultidownload';
import Panel from '../../../../common/baseComponents/panel';
import RelatedDocuments from '../../../documents/relatedDocuments';
import { applicationConstants } from '../../../../constants/appConstants';
import PropTypes from 'prop-types';

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
                required={true}
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

            {/* <div className="row">
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.modalConstant.VISIBLE_TO_CUSTOMER}
                    isSwitchLabel={true}
                    switchName="isVisibleToCustomer"
                    id="isVisibleToCustomerEdit"
                    colSize="s3 m4 mt-4"
                    checkedStatus={props.eidtedProjectDocuments.isVisibleToCustomer}
                    onChangeToggle={props.handlerChange}
                />
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.modalConstant.VISIBLE_TO_TS}
                    isSwitchLabel={true}
                    switchName="isVisibleToTS"
                    id="isVisibleToTS"
                    colSize="s3 m4 mt-4"
                    checkedStatus={props.eidtedProjectDocuments.isVisibleToTS}
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
        const functionRefs = {};
        functionRefs["enableEditColumn"] = this.disableDocumentButtons;
        functionRefs['isDisable'] = (this.props.pageMode === localConstant.commonConstants.VIEW);
        this.headerData = HeaderData(functionRefs);  
        this.confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: modalMessageConstant.DOCUMENT_DELETE_MESSAGE,
            type: "confirm",
            modalClassName: "warningToast",
            buttons: [
                {
                    buttonName: "Yes",
                    onClickHandler: this.deleteVisitRecord,
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

    editRowHandler = (data) => {
        this.setState({
            isDocumentModalOpen: true,
            isDocumentModelAddView: false,
            isbtnDisable: false
        });
        this.editedRowData = data;
    }

    componentDidMount() {        
        if (!(this.props.interactionMode && this.props.pageMode === localConstant.commonConstants.VIEW)) {
            this.dropArea = document.getElementById("drop-area");
            if (this.dropArea && this.dropArea.addEventListener) {
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
                //const customerVisibleStatus = this.updatedData.isVisibleToCustomer ? this.updatedData.isVisibleToCustomer : false;
                let customerVisibleStatus = false;
                let visibleToTSStatus = false;
                this.props.projectDocumentTypeData.map(doc => {
                    if (doc.name === docType) {
                        visibleToTSStatus = doc.isTSVisible;
                    }                   
                });
                 //Defect ID 702 - customer visible switch can Enable while document Upload 
                 // As per discussion with As per discussion with Sumit and Bhavithira, Documnt Type like 'Non Conformance Report','Report - Flash','Release Note' Based customer Visible Switch Enabled
                if(!isUndefined(docType)){                        
                    customerVisibleStatus = (applicationConstants.customerVisibleDocType.filter( x => { return x === docType; }).length > 0);       
                }else{
                    customerVisibleStatus = false;
                }
                
                documentFile.map(document => {     
                    const formatsample = configuration.allowedFileFormats;              
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
                            moduleCode: "VST",
                            requestedBy: this.props.loggedInUser,                            
                            //moduleCodeReference: (this.props.visitInfo && this.props.visitInfo.visitId ? this.props.visitInfo.visitId : 0)// D1186 & D1188
                        };
                        this.updatedData = {
                            documentName: filesToBeUpload[i].name,
                            documentType: docType,
                            documentSize: filesToBeUpload[i].size == 0 ? 0 : parseInt(filesToBeUpload[i].size / 1024) == 0 ? 1 : parseInt(filesToBeUpload[i].size / 1024),//Def 1126
                            createdOn: date,
                            isVisibleToCustomer: customerVisibleStatus,
                            isVisibleToTS: visibleToTSStatus,
                            recordStatus: "N",
                            canDelete:true,
                            id: Math.floor(Math.random() * (Math.pow(10, 5))),
                            modifiedBy: this.props.loggedInUser,
                            moduleCode: "VST",
                            //moduleRefCode: (this.props.visitInfo && this.props.visitInfo.visitId ? this.props.visitInfo.visitId : 0), // D1186 & D1188
                            status: "C",
                            subModuleRefCode: "0",
                            requestedBy: this.props.loggedInUser,
                            isUploaded:false
                        };
                        documentUniqueName.push(this.uniqueName);
                        createdNewRecords.push(this.updatedData);
                        this.props.actions.AddVisitDocumentDetails(createdNewRecords);
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
                            moduleRefCode: (this.props.visitInfo && this.props.visitInfo.visitId ? this.props.visitInfo.visitId : 0), // D1186 & D1188
                            canDelete: true,
                            isUploaded: true
                        };
                    }
                    this.props.actions.UpdateVisitDocumentDetails(updatedDataAfterUploaded, this.UploadedFile[0]);
                }
            }
        }
        else {
            const afterRemovefailedDoc = this.props.documentrowData.filter(x => x.documentName !== uniqueNameDetail.documentName);
            this.props.actions.UpdateVisitDocumentDetails(afterRemovefailedDoc);
        }

        if (this.props.documentrowData.length > 0) {
            const queueDocs = this.props.documentrowData.find(doc => doc.isUploaded === false);
            if (queueDocs) {
                const tempUniqueName = {
                    documentName: queueDocs.documentName,
                    moduleCode: "VST",
                    requestedBy: this.props.loggedInUser,
                };
                this.recursiveFileUpload(tempUniqueName, this.props.fileToBeUploaded);
            }
        }
    };

    isPageRefresh() {
        let isRefresh = true;
        visitTabDetails.forEach(row => {
            if(row["tabBody"] === "Documents") {
                isRefresh = row["isRefresh"];
                row["isRefresh"] = false;
                row["isCurrentTab"] = true;
            } else {
                row["isCurrentTab"] = false;
            }
        });
        return isRefresh;
    }
  
    //All Input Handle get Name and Value
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
        return result;
    }

      /**CustomerVisible Inline Switch */
      customerVisibleSwitchChangeHandler = (e,data) => {
        e.preventDefault();       
        const result = this.formInputChangeHandler(e);
        this.editedRowData = data;
        this.updatedData[result.name] = result.value;

        if (this.editedRowData.recordStatus !== "N") {
            this.updatedData["recordStatus"] = "M";
        }
        this.props.actions.UpdateVisitDocumentDetails(this.updatedData,  this.editedRowData);
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
        // const selectedRecordsFromSecondChild = this.secondChild.getSelectedRows();
        // const selectedRecordsFromThirdChild = this.thirdChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            this.copiedDocumentDetails = selectedRecords;
            IntertekToaster(localConstant.contract.SELECTED_RECORDS_COPIED, 'warningToast projDocumentCopyMesgReq');
        }
        // else if (selectedRecordsFromSecondChild.length > 0) {
        //     this.copiedDocumentDetails = selectedRecordsFromSecondChild;
        //     IntertekToaster(localConstant.contract.SELECTED_RECORDS_COPIED, 'warningToast projDocumentSecndCopyMesgReq');
        // }
        // else if (selectedRecordsFromThirdChild.length > 0) {
        //     this.copiedDocumentDetails = selectedRecordsFromThirdChild;
        //     IntertekToaster(localConstant.contract.SELECTED_RECORDS_COPIED, 'warningToast projDocumentThrdCopyMesgReq');
        // }
        else {
            IntertekToaster(localConstant.project.documents.SELECT_RECORDS_TO_COPY, 'warningToast projDocumentCopyReq');
        }
    }

    copyAssignmentDocumentHandler = () => {
        const selectedRecords = this.secondChild.getSelectedRows();        
        if (selectedRecords.length > 0) {
            this.copiedDocumentDetails = selectedRecords;
            IntertekToaster(localConstant.contract.SELECTED_RECORDS_COPIED, 'warningToast projDocumentCopyMesgReq');
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
                record.moduleCode = "VST";
                delete record.moduleRefCode;
                //record.moduleRefCode =  (this.props.visitInfo && this.props.visitInfo.visitId ? this.props.visitInfo.visitId : 0);
            });
            this.props.actions.PasteDocumentUploadData(recordToPaste)
                .then(response => {
                    if (!isEmpty(response)) {
                        for (let i = 0; i < response.length; i++) {
                            selectType = this.props.projectDocumentTypeData.filter(Type=>{ if(Type.name === response[i].documentType)return Type; });
                            this.updatedData = {
                                documentName: response[i].documentName,
                                documentType:selectType.length > 0 ? selectType[0].name : '',
                                //documentType: recordToPaste[i].documentType,
                                documentSize: recordToPaste[i].documentSize,
                                createdOn: date,
                                isVisibleToCustomer: recordToPaste[i].isVisibleToCustomer,
                                isVisibleToTS: recordToPaste[i].isVisibleToTS,
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
                        this.props.actions.AddVisitDocumentDetails(records);
                    }
                });
        }
        else {
            IntertekToaster(localConstant.project.documents.COPY_RECORDS_TO_PASTE, 'warningToast documentRecordToPasteReq');
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
        else if(this.props.iInterCompanyAssignment && this.props.isOperatorCompany && (this.props.visitInfo.visitStatus === 'O' || this.props.visitInfo.visitStatus === 'A')){
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
            }else{
                IntertekToaster(localConstant.project.documents.SELECT_ATELEAST_ONE_DOC_TO_DELETE, 'warningToast oneDocumentReq');
            }
            
        }else if (selectedRecords.length === 0) {
           IntertekToaster(localConstant.project.documents.SELECT_ATELEAST_ONE_DOC_TO_DELETE, 'warningToast oneDocumentReq');
        
        } else {    
            const isFileUploaded = uploadedDocumentCheck(selectedRecords);
            if (isFileUploaded) {        
            this.props.actions.DisplayModal(this.confirmationObject);
            }
        }
    }

    deleteVisitRecord = () => {
        //Defect 948 #5 - as Per Francina Discussed New scenario Updated  
        let deletedRecords   = this.child.getSelectedRows();
        deletedRecords =  deletedRecords.filter(x => x.canDelete == true);
        const selectedRecords = deletedRecords;//this.child.getSelectedRows();
        if(this.props.iInterCompanyAssignment && this.props.isOperatorCompany && (this.props.visitInfo.visitStatus === 'O' || this.props.visitInfo.visitStatus === 'A')) {
            const records = selectedRecords.filter(x => { if (x.recordStatus != null) return x; });
            if (records.length > 0) {
                const isFileUploaded = uploadedDocumentCheck(records);
                if (isFileUploaded) {
                    this.child.removeSelectedRows(records);
                    this.props.actions.DeleteVisitDocumentDetails(records);
                    this.props.actions.HideModal();
                }
            }
        } else if (selectedRecords.length > 0) {
            const isFileUploaded = uploadedDocumentCheck(selectedRecords);
            if (isFileUploaded) {
                this.child.removeSelectedRows(selectedRecords);
                this.props.actions.DeleteVisitDocumentDetails(selectedRecords);
                this.props.actions.HideModal();
            }
        }
        else
            this.props.actions.HideModal();
        
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
        if (isEmpty(this.updatedData.documentType)) {
                IntertekToaster(localConstant.project.documents.SELECT_FILE_TYPE, 'warningToast contractDocFileTypeReq');
            }
            else
            {
                const documentFile = Array.from(document.getElementById("uploadFiles").files);
                this.fileupload(documentFile);
            }
        
        // let date = new Date();
        // date = dateUtil.postDateFormat(date, '-');
        // // if (isEmpty(this.updatedData.documentType)) {
        // //     IntertekToaster(localConstant.project.documents.SELECT_FILE_TYPE, 'warningToast contractDocFileTypeReq');
        // // }
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
        //                     moduleCode: "VST",
        //                     requestedBy: this.props.loggedInUser,                            
        //                     moduleCodeReference: (this.props.visitInfo && this.props.visitInfo.visitId ? this.props.visitInfo.visitId : 0)
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
        //                                     status: "C"
        //                                 };
        //                                 newlyCreatedRecords.push(this.updatedData);
        //                                 this.updatedData = {};
        //                             }
        //                         }
        //                         if (newlyCreatedRecords.length > 0) {
        //                             this.props.actions.AddVisitDocumentDetails(newlyCreatedRecords);
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
             //Defect ID 702 - customer visible switch can Enable while document Upload 
            // As per Sumit discussion Documnt Type Name Based customer Visible Switch On or Off Enabled
            if(!isUndefined(this.updatedData.documentType)){               
                this.updatedData["isVisibleToCustomer"] = (applicationConstants.customerVisibleDocType.filter( x => { return x === this.updatedData.documentType; }).length > 0 );      
            }else{
                this.updatedData["isVisibleToCustomer"] = false;
            }

            this.updatedData["createdOn"] = date;
            if (this.editedRowData.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            this.props.actions.UpdateVisitDocumentDetails(this.updatedData, this.editedRowData);
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

    documentMultipleDownloadHandlerforSC = (e) => {
        e.preventDefault();
        const selectedRecords = this.secondChild.getSelectedRows();
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
        // if (parent.props.editVisitDocuments && parent.props.editVisitDocuments.recordStatus !== "N") {
        //  this.updatedData["uploadedOn"] = date;             
        //  this.updatedData["recordStatus"] = "M";
        //  }     
        parent.props.actions.UpdateVisitDocumentDetails(this.updatedData, data);
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
    disableDocumentButtons = () => {        
        return this.props.isTBAVisitStatus ? (this.props.isTBAVisitStatus === true ? true : false) : false;
    }

    render() {
        const {
            projectDocumentTypeData,
            VisitDocuments,
            assignmentDocumentsData,
            interactionMode
        } = this.props;
        const isTBAVisitStatus = this.disableDocumentButtons();
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
        bindAction(this.headerData, "EditVisitDocuments", this.editRowHandler);
        bindAction(this.headerData, "isVisibleToCustomer", (e,data) =>  this.customerVisibleSwitchChangeHandler(e,data) );     
        for(let i = 0; i < VisitDocuments.length; i++){
            VisitDocuments[i] = { ...VisitDocuments[i],roleBase: ((this.props.interactionMode && this.props.pageMode === localConstant.commonConstants.VIEW)),
                isFileUploaded: ((isUndefined(VisitDocuments[i].documentUniqueName))?false:true),
                isDisableStatus:(this.props.visitInfo.visitStatus === 'O'? (this.props.isOperatorCompany ? true : false) : false), //Commented below code because of ITK Defect 1426
                //isDisableStatus:(this.props.visitInfo.visitStatus === 'O'? (this.props.isOperatorCompany ? true : false) : (this.props.visitInfo.visitStatus === 'A' ? (this.props.isCoordinatorCompany ? false : true) : false)), // Defect Id 948 isOperatorApporved  - when the visit is Approved and existing  Upload Document Switch option should be disabled                
             };
        }   
        const isDelete = !(this.props.iInterCompanyAssignment && this.props.isOperatorCompany && (this.props.visitInfo.visitStatus === 'O' || this.props.visitInfo.visitStatus === 'A'));
        return (
            <Fragment>
            <div className="customCard">
                <h6 className="bold col s3 pl-0 pr-3">{localConstant.documents.DOCUMENTS}</h6>
                    <VisitDocumentMenuButtons
                        rowData={VisitDocuments&&VisitDocuments.length>0?VisitDocuments.filter(record => record.recordStatus !== "D" && record.subModuleRefCode === '0'):[]} //D979 - Filter only document uploaded, not approved documents
                        //rowData={this.props.Documents}
                        headerData={this.headerData}
                        copyDocumentHandler={this.copyDocumentHandler}
                        pasteDocumentHandler={this.pasteDocumentHandler}
                        uploadDocumentHandler={this.uploadDocumentHandler}
                        deleteDocumentHandler={this.deleteDocumentHandler}
                        interactionMode={!interactionMode || isTBAVisitStatus}
                        DownloadDocumentHandler={this.documentmultipleDownloadHandler}
                        onCellchange={this.onCellchange}
                        supportFileType={configuration}
                        pageMode={this.props.pageMode}
                        onRef={ref => { this.child = ref; }}
                        isDeleteDocument = { isDelete }
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
                <Panel colSize="s12" heading={localConstant.documents.ASSIGNMENT_DOCUMENTS}
                    className="pl-0 pr-0 mt-3 documentPanel bold"
                    onpanelClick={this.panelClick}
                    isopen={this.state.isPanelOpen} isArrow={true} >
                    <RelatedDocuments DocumentsData={assignmentDocumentsData}
                        headerData={ChildHeaderData}
                        interactionMode={interactionMode  || isTBAVisitStatus}
                        title={localConstant.documents.ASSIGNMENT_DOCUMENTS}
                        copyRecord={this.copyAssignmentDocumentHandler}
                        pageMode={this.props.pageMode}
                        DownloadDocumentHandler={this.documentMultipleDownloadHandlerforSC}
                        onRef={ref => { this.secondChild = ref; }}
                        paginationPrefixId={localConstant.paginationPrefixIds.visitAssignmentDocumentId}
                    />
                </Panel>
                {/* <CustomModal modalData={modelData} /> */}
            </Fragment>
        );
    }
}

export default Documents;
Documents.propTypes = {
    assignmentDocumentsData:PropTypes.array,
};

Documents.defaultProps = {
    assignmentDocumentsData:[],
};
