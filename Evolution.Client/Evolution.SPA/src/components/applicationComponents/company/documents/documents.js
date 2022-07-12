import React, { Component, Fragment } from 'react';
import PropTypes from 'proptypes';
import { HeaderData } from './headerData.js';
import dateUtil from '../../../../utils/dateUtil';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import CustomModal from '../../../../common/baseComponents/customModal';
import CompanyDocument from '../../../documents/documents';
import { configuration } from '../../../../appConfig';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, isEmpty, bindAction, formInputChangeHandler, isUndefined, uploadedDocumentCheck } from '../../../../utils/commonUtils';
import { MultipleDownload } from '../../../documents/documentsMultidownload';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import Modal from '../../../../common/baseComponents/modal';

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
                defaultValue={props.eidtedCompanyDocuments.documentType}
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
                checkedStatus={props.eidtedCompanyDocuments.isVisibleToCustomer}
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
                readOnly={props.readOnly}
                disabled={true}
                defaultValue={props.eidtedCompanyDocuments.documentName}
            />
        </Fragment>
    );
};

class CompanyDocuments extends Component {
    constructor(props) {
        super(props);
        this.filesToDisplay = [];
        this.updatedData = {};
        this.uniqueName = {};   // unique ID Generation Request Objects
        this.UploadedFile = {}; // Grid file Obiject  
        this.editedRowData = {};
        this.state = {
            isDocumentModalOpen: false,
            isDocumentModelAddView: true,
            isOpen: false
        };
        this.confirmationModalData = {
            title: "",
            message: "",
            type: "",
            modalClassName: "",
            buttons: []
        };
        this.dropArea = '';
        this.functionRef = {};
        this.functionRef['isDisable'] = (this.props.pageMode === localConstant.commonConstants.VIEW);
        this.headerData = HeaderData(this.functionRef);
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
        /** 
         * FetchMasterCompanyDocumentTypes api call is commented, Now we are getting doc type from master data
        */
        //this.props.actions.FetchMasterCompanyDocumentTypes();

        //if((this.props.interactionMode && this.props.pageMode === localConstant.commonConstants.VIEW)){
        //Drag and Drop File Upload
        this.dropArea = document.getElementById("drop-area");
        if (this.dropArea) {
            // Prevent default drag behaviors
            [
                'dragenter', 'dragover', 'dragleave', 'drop'
            ].forEach(eventName => {
                this.dropArea.addEventListener(eventName, this.preventDefaults, false);
                document.body.addEventListener(eventName, this.preventDefaults, false);
            });

            // Highlight drop area when item is dragged over it
            [
                'dragenter', 'dragover'
            ].forEach(eventName => {
                this.dropArea.addEventListener(eventName, this.highlight, false);
            });

            [
                'dragleave', 'drop'
            ].forEach(eventName => {
                this.dropArea.addEventListener(eventName, this.unhighlight, false);
            });
            // Handle dropped files
            this.dropArea.addEventListener('drop', this.handleDrop, false);

        }
        //}
    }
    preventDefaults = (e) => {
        e.preventDefault();
        e.stopPropagation();
    }

    highlight = (e) => {
        this.dropArea.classList.add('highlight');
    }

    unhighlight = (e) => {
        this.dropArea.classList.remove('highlight');
    }
    handleDrop = (e) => {
        const dt = e.dataTransfer;
        const files = dt.files;
        const documentFile = Array.from(files);
        this.fileupload(documentFile);
        this.unhighlight(e);
    }
    fileupload = async (files) => {
        //e.preventDefault();
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
            const failureFormat = [];
            const documentUniqueName = [];
            let createdNewRecords = [];
            const customerVisibleStatus = this.updatedData.isVisibleToCustomer ? this.updatedData.isVisibleToCustomer : false;
            let visibleToTSStatus = false;
            const docTypeData = this.props.masterDocumentTypesData;
            for (let i = 0; i < docTypeData.length; i++) {
                if (docTypeData[i].name === docType) {
                    visibleToTSStatus = docTypeData[i].isTSVisible;
                }
            }
            documentFile.map(document => {
                const filterType = configuration.allowedFileFormats.indexOf(document.name.substring(document.name.lastIndexOf(".")).toLowerCase());
                if (filterType >= 0 && document.name.lastIndexOf(".") !== -1) {  // sanity 204 fix
                    if (parseInt(document.size / 1024) > configuration.fileLimit) {
                        failureFiles.push(document.name);
                    }
                    else {
                        filesToBeUpload.push(document);
                    }
                } else {
                    failureFormat.push(document.name);
                }

                return document;
            });
            if (failureFormat.length === 0 && filesToBeUpload.length > 0) {
                for (let i = 0; i < filesToBeUpload.length; i++) {
                    this.uniqueName = {
                        documentName: filesToBeUpload[i].name,
                        moduleCode: "COMP",
                        requestedBy: this.props.loggedInUser,
                        // isUploaded:false
                        // moduleCodeReference: this.props.companyInfo.companyCode // D1186 & D1188
                    };
                    this.updatedData = {
                        documentName: filesToBeUpload[i].name,
                        moduleCode: "COMP",
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
                IntertekToaster(failureFiles.toString() + localConstant.companyDetails.Documents.FILE_LIMIT_EXCEDED, 'warningToast contractDocSizeReq');
            }
        }
        else {
            IntertekToaster(localConstant.companyDetails.Documents.NO_FILE_SELECTED, 'warningToast contractNoFileSelectedReq');
        }
    }

    recursiveFileUpload = async (uniqueNameDetail, filesToBeUpload) => {
        const response = await this.props.actions.FetchDocumentUniqueName([ uniqueNameDetail ], filesToBeUpload);
        if (!isEmpty(response)) {
            for (let i = 0; i < response.length; i++) {
                if (response[i] !== null) {
                    let updatedDataAfterUploaded = {};
                    if (!response[i].status) {
                        this.UploadedFile = this.props.DocumentsData.filter(x => x.documentName == response[i].fileName && x.documentUniqueName === undefined);

                        updatedDataAfterUploaded = {
                            recordStatus: "N",
                            documentUniqueName: response[i].uploadedFileName,
                            moduleCodeReference: this.props.companyInfo.companyCode, // D1186 & D1188     
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
                    moduleCode: "COMP",
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
    customerVisibleSwitchChangeHandler = (e, data) => {
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
        if (!this.updatedData.documentType) {
            IntertekToaster(localConstant.companyDetails.Documents.SELECT_FILE_TYPE, 'warningToast contractDocFileTypeReq');
            return false;
        }
        this.fileupload(documentFile);
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
            IntertekToaster(localConstant.companyDetails.Documents.SELECT_FILE_TYPE, 'warningToast noFileTypeWarning');
        }
        else {
            if (this.updatedData.documentType) {
                this.props.masterDocumentTypesData.map(doc => {
                    if (doc.name === this.updatedData.documentType) {
                        this.updatedData["isVisibleToTS"] = doc.isTSVisible;
                    }
                });
            }
            this.updatedData["uploadedOn"] = date;
            if (this.props.editCompanyDocumentDetails.recordStatus !== "N") {
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
        if (selectedRecords.length > 0) {
            this.props.actions.CopyDocumentDetails(selectedRecords);
            IntertekToaster(localConstant.companyDetails.Documents.SELECTED_RECORDS_COPIED, 'warningToast documentCopyMesgReq');
        }
        else {
            IntertekToaster(localConstant.companyDetails.Documents.SELECT_RECORDS_TO_COPY, 'warningToast documentCopyReq');
        }
    }

    pasteRecord = () => {
        const records = [];
        let recordToPaste = [];
        recordToPaste = this.props.copiedDocumentDetails;
        if (recordToPaste.length > 0) {
            let date = new Date();
            date = dateUtil.postDateFormat(date, '-');
            recordToPaste.map((record) => {
                delete record.moduleRefCode;
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
                                id: Math.floor(Math.random() * (Math.pow(10, 5))),
                                documentUniqueName: response[i].uniqueName,
                                modifiedBy: this.props.loggedInUser,
                                moduleCode: response[i].moduleCode,
                                moduleRefCode: response[i].moduleCodeReference,
                                status: "C",
                                canDelete: true
                            };
                            records.push(this.updatedData);
                            this.updatedData = {};
                        }
                        this.props.actions.AddDocumentDetails(records);
                    }
                });
        }
        else {
            IntertekToaster(localConstant.companyDetails.Documents.COPY_RECORDS_TO_PASTE, 'warningToast documentRecordToPasteReq');
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
                            onClickHandler: this.deletecompanyRecord,
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

    deletecompanyRecord = () => {
        let selectedRecords = this.child.getSelectedRows();
        selectedRecords = selectedRecords.filter(x => x.canDelete == true);
        if (selectedRecords.length > 0) {
            const isFileUploaded = uploadedDocumentCheck(selectedRecords);
            if (isFileUploaded) {
                this.child.removeSelectedRows(selectedRecords);
                this.props.actions.DeleteCompanyDocumentDetails(selectedRecords);
                this.props.actions.HideModal();
            }
        }
    }
    renameConfirmationRejectHandler = (fileName, data) => {
        this.updatedtoStore(fileName, data);
        this.updateChangesToGridReferesh('', data);
        this.props.actions.HideModal();
    }
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    documentmultipleDownloadHandler = (e) => {
        e.preventDefault();
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            this.props.actions.MultiDocDownload(selectedRecords);
        }
        else {
            IntertekToaster(localConstant.customer.documents.SELECT_ATELEAST_ONE_DOC_TO_DOWNLOAD, 'warningToast CustDocDelDocChk');
        }

    }
    updatedtoStore = (fileName, data) => {
        const parent = this;
        let date = new Date();
        date = dateUtil.postDateFormat(date, '-');
        this.updatedData["documentName"] = fileName;
        this.updatedData["uploadedOn"] = date;
        // if (parent.props.editCompanyDocumentDetails.recordStatus !== "N") {
        //  this.updatedData["uploadedOn"] = date;             
        //  this.updatedData["recordStatus"] = "M";
        //  }     
        parent.props.actions.UpdateDocumentDetails(this.updatedData, data);
        this.updatedData = {};
    }
    updateChangesToGridReferesh = (oldfileNameFormat, data) => {
        const parent = this;
        const index = parent.props.DocumentsData.findIndex(document => document.id === data.id);
        const propvalue = parent.props.DocumentsData[index].documentName.split(/\.(?=[^\.]+$)/)[1];
        const propName = parent.props.DocumentsData[index].documentName.split(/\.(?=[^\.]+$)/)[0];

        if (propvalue === undefined) {
            const obj = { ...parent.props.DocumentsData[index] };
            obj.documentName = parent.props.DocumentsData[index].documentName + '.' + oldfileNameFormat;
            this.child.setData(obj, (index));
        } else {
            this.child.setData(parent.props.DocumentsData[index], index);
        }
    }
    onCellchange = (data) => {

        const parent = this;
        const oldfileNameFormat = data.oldValue.split(/\.(?=[^\.]+$)/)[1];
        const newfileNameFormat = data.value.split(/\.(?=[^\.]+$)/)[1];
        const oldFileName = data.oldValue.split(/\.(?=[^\.]+$)/)[0];
        const newFileName = data.newValue.split(/\.(?=[^\.]+$)/)[0];
        if (!isUndefined(data.data.documentUniqueName)) {
            if (data.data.recordStatus !== 'N') {
                data.data.recordStatus = 'M';
            }
            if (oldfileNameFormat === newfileNameFormat) {
                this.updatedtoStore(data.newValue, data.data);
            } else if (newfileNameFormat === undefined) {
                if (oldFileName !== newFileName) {
                    const fileNameWithFormat = data.newValue + '.' + oldfileNameFormat;
                    this.updatedtoStore(fileNameWithFormat, data.data);
                } else {
                    this.updateChangesToGridReferesh(oldfileNameFormat, data.data);
                }
            } else {
                //IntertekToaster('Your Exgisting File Format is '+ oldfileNameFormat + ' Please Provide Same Format', 'warningToast documentCopyReq');
                const confirmationObject = {
                    title: modalTitleConstant.CONFIRMATION,
                    message: localConstant.commonConstants.FILE_RENAME_MSG_START + oldfileNameFormat.toUpperCase() + localConstant.commonConstants.FILE_RENAME_MSG_END,
                    type: "confirm",
                    modalClassName: "warningToast",
                    buttons: [

                        {
                            buttonName: "OK",
                            onClickHandler: () => this.renameConfirmationRejectHandler(data.oldValue, data.data),
                            className: "modal-close m-1 btn-small"
                        }
                    ]
                };
                parent.props.actions.DisplayModal(confirmationObject);
            }
        }
        else {
            IntertekToaster(localConstant.commonConstants.DOCUMENT_STATUS, 'warningToast documentRecordToPasteReq');
        }
    }
    render() {
        const { masterDocumentTypesData, DocumentsData } = this.props;
        this.documentAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.hideModal,
                btnID: "cancelDocumentSubmit",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type: "button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.state.isDocumentModelAddView ? this.uploadDocumentSubmitHandler : this.editDocumentHandler,
                btnID: "addDocumentsHandler",
                btnClass: "waves-effect waves-teal btn-small mr-2",
                showbtn: true
            }
        ];
        let companyDocumentsData = [];
        if (DocumentsData) {
            companyDocumentsData = DocumentsData.filter(document => document.recordStatus !== 'D');
        }
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };

        bindAction(this.headerData, "isVisibleToCustomer", (e, data) => this.customerVisibleSwitchChangeHandler(e, data));
        bindAction(this.headerData, "EditCompanyDocuments", this.editRowHandler);
        for (let i = 0; i < companyDocumentsData.length; i++) {
            companyDocumentsData[i] = { ...companyDocumentsData[i], roleBase: ((this.props.interactionMode && this.props.pageMode === localConstant.commonConstants.VIEW)) };
            companyDocumentsData[i] = { ...companyDocumentsData[i], isFileUploaded: ((isUndefined(companyDocumentsData[i].documentUniqueName)) ? false : true) };
        }

        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                <div className="customCard">
                    <h6 className="bold col s3 pl-0 pr-3">{localConstant.documents.DOCUMENTS}</h6>
                    <CompanyDocument rowData={companyDocumentsData}
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
                        paginationPrefixId={localConstant.paginationPrefixIds.companyDoc}
                        // Company doesnot have view  mode, so we are  sending interaction mode as true to display the buttons related to documents actions
                        onRef={ref => { this.child = ref; }} />
                </div>
                {this.state.isDocumentModalOpen &&
                    <Modal id="companyDocModalPopup"
                        title={this.state.isDocumentModelAddView ? localConstant.project.documents.UPLOAD_DOC : localConstant.project.documents.EDIT_DOC}
                        modalClass="companyDocModal"
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
                                eidtedCompanyDocuments={this.editedRowData}
                                readOnly={this.state.isDocumentModelAddView ? false : true}
                            />}
                    </Modal>}
            </Fragment>
        );
    }

}
CompanyDocuments.propTypes = {
    DocumentsData: PropTypes.array.isRequired,
    masterDocumentTypesData: PropTypes.array.isRequired,
    gridProps: PropTypes.array.isRequired
};

CompanyDocuments.defaultprops = {
    DocumentsData: [],
    masterDocumentTypesData: [],
    gridProps: {}
};

export default CompanyDocuments;