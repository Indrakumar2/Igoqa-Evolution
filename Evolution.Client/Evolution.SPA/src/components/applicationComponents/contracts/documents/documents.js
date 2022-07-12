import React, { Component, Fragment } from 'react';
import CustomModal from '../../../../common/baseComponents/customModal';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import dateUtil from '../../../../utils/dateUtil';
import { configuration } from '../../../../appConfig';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import Modal from '../../../../common/baseComponents/modal';
import { HeaderData, CustomerHeaderData, ProjectHeaderData, ParentHeaderData } from './headerData.js';
import RelatedDocuments from '../../../documents/relatedDocuments';
import Panel from '../../../../common/baseComponents/panel';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { getlocalizeData, isEmpty, bindAction, formInputChangeHandler, isUndefined, uploadedDocumentCheck } from '../../../../utils/commonUtils';
import ContractDocumentMenuButtons from '../../../documents/documents';
import { MultipleDownload } from '../../../documents/documentsMultidownload';
import moment from 'moment';
const localConstant = getlocalizeData();

const ModalPopupAddView = (props) => {
    return (
        <Fragment>

            <CustomInput hasLabel={true}
                label={localConstant.contract.Documents.SELECT_FILE_TYPE}
                divClassName='col pl-0 mb-4'
                labelClass="mandate"
                colSize="s6"
                type='select'
                //required={true}
                className="customInputs browser-default"
                optionsList={props.contractDocumentTypeData}
                optionName='name'
                optionValue="name"
                onSelectChange={props.handlerChange}
                name="documentType"
                id="documentType"
            //defaultValue={props.documentType}
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
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.modalConstant.VISIBLE_TO_OUT_OF_COMPANY}
                    isSwitchLabel={true}
                    switchName="isVisibleOutOfCompany"
                    id="isVisibleOutOfCompany"
                    colSize="s3 m4 mt-4"
                    checkedStatus={props.isVisibleOutOfCompany}
                    onChangeToggle={props.handlerChange}
                />
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
                optionsList={props.contractDocumentTypeData}
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

            <div className="row">
                {/* <CustomInput
                    type='switch'
                    switchLabel={localConstant.modalConstant.VISIBLE_TO_CUSTOMER}
                    isSwitchLabel={true}
                    switchName="isVisibleToCustomer"
                    id="isVisibleToCustomerEdit"
                    colSize="s3 m4 mt-4"
                    checkedStatus={props.isVisibleToCustomer}
                    onChangeToggle={props.handlerChange}
                /> */}
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.modalConstant.VISIBLE_TO_OUT_OF_COMPANY}
                    isSwitchLabel={true}
                    switchName="isVisibleOutOfCompany"
                    id="isVisibleOutOfCompany"
                    colSize="s3 m4 mt-4"
                    checkedStatus={props.isVisibleOutOfCompany}
                    onChangeToggle={props.handlerChange}
                />
            </div>
        </Fragment>
    );
};

class ContractDocuments extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isDocumentModalOpen: false,
            isDocumentModelAddView: true,
            isPanelOpen: false,
            isProjectPanelOpen: false,
            isParentPanelOpen: false
        };
        this.updatedData = {};
        this.filesToDisplay = [];
        this.uploadButtonDisable = false;
        this.editedRowData = {};
        this.functionRef = {};
        this.functionRef['isDisable'] = (this.props.pageMode === localConstant.commonConstants.VIEW);
        this.headerData = HeaderData(this.functionRef);
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
        // this.props.actions.FetchContractDocumentTypes();
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        const isContractDocumentsFetched = (tabInfo && tabInfo.componentRenderCount === 0) ? true : false;
        if (isContractDocumentsFetched) {
            if (this.props.contractInfo && this.props.contractInfo.parentContractNumber) {
                this.props.actions.FetchContractDocuments(this.props.contractInfo.parentContractNumber);
            }
            if (this.props.currentPage === "Create Contract") {
                if (this.props.generalDetailsCreateContractCustomerDetails) {
                    this.props.actions.FetchCustomerDocumentsofContracts();
                    this.props.actions.FetchProjectDocumentsofContracts();
                }
            }
            else {
                this.props.actions.FetchCustomerDocumentsofContracts();
                this.props.actions.FetchProjectDocumentsofContracts();
            }
        }
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
    fileupload = (files) => {
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
            const failureFormat = [];
            const documentUniqueName = [];
            let createdNewRecords = [];
            const customerVisibleStatus = this.updatedData.isVisibleToCustomer ? this.updatedData.isVisibleToCustomer : false;
            let visibleToTSStatus = false;
            const outOfCompanyVisible = this.updatedData.isVisibleOutOfCompany ? this.updatedData.isVisibleOutOfCompany : false;
            this.props.contractDocumentTypeData.map(doc => {
                if (doc.name === docType) {
                    visibleToTSStatus = doc.isTSVisible;
                }
            });
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
                        moduleCode: "CNT",
                        requestedBy: this.props.loggedInUser,
                        // isUploaded:false
                        // moduleCodeReference: this.props.companyInfo.companyCode // D1186 & D1188
                    };
                    this.updatedData = {
                        documentName: filesToBeUpload[i].name,
                        moduleCode: "CNT",
                        requestedBy: this.props.loggedInUser,
                        //moduleCodeReference: this.props.companyInfo.companyCode, // D1186 & D1188
                        documentType: docType,
                        documentSize: filesToBeUpload[i].size == 0 ? 0 : parseInt(filesToBeUpload[i].size / 1024) == 0 ? 1 : parseInt(filesToBeUpload[i].size / 1024),//def1126
                        createdOn: date,
                        isVisibleToCustomer: customerVisibleStatus,
                        isVisibleToTS: visibleToTSStatus,
                        isVisibleOutOfCompany: outOfCompanyVisible, //Changes for Live D762
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
                IntertekToaster(failureFiles.toString() + localConstant.contract.Documents.FILE_LIMIT_EXCEDED, 'warningToast contractDocSizeReq');
            }
        }
        else {
            IntertekToaster(localConstant.contract.Documents.NO_FILE_SELECTED, 'warningToast contractNoFileSelectedReq');
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
                            moduleCodeReference: this.props.contractInfo.contractNumber,   // D1186 & D1188        
                            canDelete: true,
                            isUploaded: true
                        };
                    }
                    this.props.actions.UpdateDocumentDetails(updatedDataAfterUploaded, this.UploadedFile[0]);
                }
            }
        }
        else {
            const afterRemovefailedDoc = this.props.documentrowData.filter(x => x.documentName !== uniqueNameDetail.documentName);
            this.props.actions.UpdateDocumentDetails(afterRemovefailedDoc);
        }

        if (this.props.documentrowData.length > 0) {
            const queueDocs = this.props.documentrowData.find(doc => doc.isUploaded === false);
            if (queueDocs) {
                const tempUniqueName = {
                    documentName: queueDocs.documentName,
                    moduleCode: "CNT",
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
    copyDocumentHandler = () => {
        const selectedRecords = this.child.getSelectedRows();
        const selectedRecordsFromSecondChild = this.secondChild.getSelectedRows();
        const selectedRecordsFromThirdChild = this.thirdChild.getSelectedRows();
        let selectedRecordsFromFourthChild = [];

        if (this.props.contractInfo.contractType === 'CHD' && this.props.contractInfo.parentContractNumber) {
            selectedRecordsFromFourthChild = this.fourthChild.getSelectedRows();
        }
        if (selectedRecords.length > 0) {
            this.props.actions.CopyDocumentDetails(selectedRecords);
            IntertekToaster(localConstant.contract.SELECTED_RECORDS_COPIED, 'warningToast documentCopyMesgReq');
        }
        else if (selectedRecordsFromSecondChild.length > 0) {
            this.props.actions.CopyDocumentDetails(selectedRecordsFromSecondChild);
            IntertekToaster(localConstant.contract.SELECTED_RECORDS_COPIED, 'warningToast documentCopyMesgSecndReq');
        }
        else if (selectedRecordsFromThirdChild.length > 0) {
            this.props.actions.CopyDocumentDetails(selectedRecordsFromThirdChild);
            IntertekToaster(localConstant.contract.SELECTED_RECORDS_COPIED, 'warningToast documentCopyMesgThrdReq');
        }
        else if (selectedRecordsFromFourthChild.length > 0) {
            this.props.actions.CopyDocumentDetails(selectedRecordsFromFourthChild);
            IntertekToaster(localConstant.contract.SELECTED_RECORDS_COPIED, 'warningToast documentCopyMesgThrdReq');
        }
        else {
            IntertekToaster(localConstant.contract.Documents.SELECT_RECORDS_TO_COPY, 'warningToast documentCopyReq');
        }
    }
    pasteDocumentHandler = () => {
        const records = [];
        let recordToPaste = [];
        let selectType = '';
        recordToPaste = this.props.copiedDocumentDetails;
        if (recordToPaste.length > 0) {
            let date = new Date();
            date = dateUtil.postDateFormat(date, '-');
            recordToPaste.map((record) => {
                record.moduleCode = "CNT";
                delete record.moduleRefCode;
            });
            this.props.actions.PasteDocumentUploadData(recordToPaste)
                .then(response => {
                    if (!isEmpty(response)) {
                        for (let i = 0; i < response.length; i++) {
                            selectType = this.props.contractDocumentTypeData.filter(Type => { if (Type.name === response[i].documentType) return Type; });
                            this.updatedData = {
                                documentName: response[i].documentName,
                                //documentType: recordToPaste[i].documentType,
                                documentType: selectType.length > 0 ? selectType[0].name : '',
                                documentSize: recordToPaste[i].documentSize,
                                createdOn: date,
                                isVisibleToCustomer: recordToPaste[i].isVisibleToCustomer,
                                isVisibleToTS: recordToPaste[i].isVisibleToTS,
                                isVisibleOutOfCompany: recordToPaste[i].isVisibleOutOfCompany ? recordToPaste[i].isVisibleOutOfCompany : false,
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
            IntertekToaster(localConstant.contract.Documents.COPY_RECORDS_TO_PASTE, 'warningToast documentRecordToPasteReq');
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
                this.props.actions.DeleteContractDocumentDetails(selectedRecords);
                this.props.actions.HideModal();
            }
        }
    }
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    uploadDocumentSubmitHandler = (e) => {
        e.preventDefault();
        const documentFile = Array.from(document.getElementById("uploadFiles").files);
        if (!this.updatedData.documentType) {
            IntertekToaster(localConstant.contract.Documents.SELECT_FILE_TYPE, 'warningToast contractDocFileTypeReq');
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
            IntertekToaster(localConstant.contract.Documents.SELECT_FILE_TYPE, 'warningToast contractFileTypeReq');
            return false;
        }
        else {
            if (this.updatedData.documentType) {
                this.props.contractDocumentTypeData.map(doc => {
                    if (doc.name === this.updatedData.documentType) {
                        this.updatedData["isVisibleToTS"] = doc.isTSVisible;
                    }
                });
            }
            this.updatedData["createdOn"] = date;
            if (this.editedRowData.recordStatus !== "N") {
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

    parentPanelClick = (e) => {
        this.setState({
            isParentPanelOpen: !this.state.isParentPanelOpen
        });
    }
    documentMultipleDownloadHandlerforFC = (e) => {
        const selectedRecords = this.fourthChild.getSelectedRows();
        if (selectedRecords.length > 0) {

            MultipleDownload(selectedRecords);
        }
        else {
            IntertekToaster(localConstant.customer.documents.SELECT_ATELEAST_ONE_DOC_TO_DOWNLOAD, 'warningToast ContractDocDelDocChk');
        }
    }

    documentMultipleDownloadHandlerforTC = (e) => {
        const selectedRecords = this.thirdChild.getSelectedRows();
        if (selectedRecords.length > 0) {

            this.props.actions.MultiDocDownload(selectedRecords);
        }
        else {
            IntertekToaster(localConstant.customer.documents.SELECT_ATELEAST_ONE_DOC_TO_DOWNLOAD, 'warningToast CustDocDelDocChk');
        }
    }
    documentMultipleDownloadHandlerforSC = (e) => {
        const selectedRecords = this.secondChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            this.props.actions.MultiDocDownload(selectedRecords);
        }
        else {
            IntertekToaster(localConstant.customer.documents.SELECT_ATELEAST_ONE_DOC_TO_DOWNLOAD, 'warningToast CustDocDelDocChk');
        }
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
    renameConfirmationRejectHandler = (fileName, data) => {
        this.updatedtoStore(fileName, data);
        this.updateChangesToGridReferesh('', data);
        this.props.actions.HideModal();
    }
    updatedtoStore = (fileName, data) => {
        const parent = this;
        let date = new Date();
        date = dateUtil.postDateFormat(date, '-');
        this.updatedData["documentName"] = fileName;
        this.updatedData["uploadedOn"] = date;
        // if (parent.props.editContractDocumentDetails && parent.props.editContractDocumentDetails.recordStatus !== "N") {
        //  this.updatedData["uploadedOn"] = date;             
        //  this.updatedData["recordStatus"] = "M";
        //  }     
        parent.props.actions.UpdateDocumentDetails(this.updatedData, data);
        this.updatedData = {};
    }
    updateChangesToGridReferesh = (oldfileNameFormat, data) => {
        const parent = this;
        const index = parent.props.documentrowData.findIndex(document => document.id === data.id);
        const propvalue = parent.props.documentrowData[index].documentName.split(/\.(?=[^\.]+$)/)[1];
        if (propvalue === undefined) {
            const obj = { ...parent.props.documentrowData[index] };
            obj.documentName = parent.props.documentrowData[index].documentName + '.' + oldfileNameFormat;
            this.child.setData(obj, (index));
        } else {
            this.child.setData(parent.props.documentrowData[index], index);
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
        const { customerDocumentsData, contractInfo, parentContractDocumentsData, projectDocumentsData, contractDocumentTypeData, documentDetailsInfo, documentrowData, interactionMode } = this.props;
        for (let i = 0; i < documentrowData.length; i++) {
            documentrowData[i] = { ...documentrowData[i], roleBase: ((this.props.interactionMode && this.props.pageMode === localConstant.commonConstants.VIEW)) };
            documentrowData[i] = { ...documentrowData[i], isFileUploaded: ((isUndefined(documentrowData[i].documentUniqueName)) ? false : true) };
        }

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
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        bindAction(this.headerData, "EditContractDocuments", this.editRowHandler);
        bindAction(this.headerData, "isVisibleToCustomer", (e, data) => this.customerVisibleSwitchChangeHandler(e, data));
        return (
            <Fragment>
                <div className="customCard">
                    <h6 className="bold col s3 pl-0 pr-3">{localConstant.documents.DOCUMENTS}</h6>
                    <ContractDocumentMenuButtons
                        rowData={documentrowData && documentrowData.length > 0 ? documentrowData.filter(record => record.recordStatus !== "D") : []}
                        headerData={this.headerData}
                        copyDocumentHandler={this.copyDocumentHandler}
                        pasteDocumentHandler={this.pasteDocumentHandler}
                        uploadDocumentHandler={this.uploadDocumentHandler}
                        deleteDocumentHandler={this.deleteDocumentHandler}
                        DownloadDocumentHandler={this.documentmultipleDownloadHandler}
                        interactionMode={!this.props.interactionMode}
                        onCellchange={this.onCellchange}
                        supportFileType={configuration}
                        pageMode={this.props.pageMode}
                        paginationPrefixId={localConstant.paginationPrefixIds.contractDoc}
                        onRef={ref => { this.child = ref; }}
                    />
                </div>
                <Panel colSize="s12" heading={localConstant.documents.CUSTOMER_DOCUMENTS}
                    className="pl-0 pr-0 mt-2 documentPanel bold"
                    onpanelClick={this.panelClick}
                    isopen={this.state.isPanelOpen} isArrow={true}>
                    <RelatedDocuments DocumentsData={customerDocumentsData}
                        headerData={CustomerHeaderData}
                        interactionMode={this.props.interactionMode}
                        title={localConstant.documents.CUSTOMER_DOCUMENTS}
                        copyRecord={this.copyDocumentHandler}
                        DownloadDocumentHandler={this.documentMultipleDownloadHandlerforSC}
                        pageMode={this.props.pageMode}
                        paginationPrefixId={localConstant.paginationPrefixIds.contractCustomerDoc}
                        onRef={ref => { this.secondChild = ref; }}
                    />
                </Panel>
                <Panel colSize="s12" heading={localConstant.documents.PROJECT_DOCUMENTS}
                    className="pl-0 pr-0 documentPanel bold"
                    onpanelClick={this.projectPanelClick} isopen={this.state.isProjectPanelOpen} isArrow={true}>
                    <RelatedDocuments DocumentsData={projectDocumentsData}
                        headerData={ProjectHeaderData}
                        interactionMode={this.props.interactionMode}
                        title={localConstant.documents.PROJECT_DOCUMENTS}
                        copyRecord={this.copyDocumentHandler}
                        DownloadDocumentHandler={this.documentMultipleDownloadHandlerforTC}
                        pageMode={this.props.pageMode}
                        paginationPrefixId={localConstant.paginationPrefixIds.contractProjectDoc}
                        onRef={ref => { this.thirdChild = ref; }}
                    />
                </Panel>
                { contractInfo && contractInfo.contractType === 'CHD' && contractInfo.parentContractNumber ?
                    <Panel colSize="s12" heading={localConstant.documents.PARENT_CONTRACT_DOCUMENTS}
                        className="pl-0 pr-0 documentPanel bold"
                        onpanelClick={this.parentPanelClick} isopen={this.state.isParentPanelOpen} isArrow={true}>
                        <RelatedDocuments DocumentsData={parentContractDocumentsData}
                            headerData={ParentHeaderData}
                            interactionMode={this.props.interactionMode}
                            title={localConstant.documents.PARENT_CONTRACT_DOCUMENTS}
                            copyRecord={this.copyDocumentHandler}
                            DownloadDocumentHandler={this.documentMultipleDownloadHandlerforFC}
                            pageMode={this.props.pageMode}
                            paginationPrefixId={localConstant.paginationPrefixIds.parentContractDoc}
                            onRef={ref => { this.fourthChild = ref; }}
                        />
                    </Panel> : null
                }
                {this.state.isDocumentModalOpen &&
                    <Modal id="contractDocModalPopup"
                        title={this.state.isDocumentModelAddView ? localConstant.documents.UPLOAD_DOC : localConstant.documents.EDIT_DOC}
                        modalClass="contactDocModal"
                        buttons={this.documentAddButtons}
                        isShowModal={this.state.isDocumentModalOpen}>

                        {this.state.isDocumentModelAddView ? <ModalPopupAddView
                            contractDocumentTypeData={contractDocumentTypeData}
                            handlerChange={(e) => this.formInputChangeHandler(e)}
                            supportFileType={configuration}
                            documentType={documentDetailsInfo.documentType}
                            isVisibleToCustomer={documentDetailsInfo.isVisibleToCustomer}
                            isVisibleOutOfCompany={documentDetailsInfo.isVisibleOutOfCompany} />
                            : <ModalPopupEditView
                                contractDocumentTypeData={contractDocumentTypeData}
                                handlerChange={(e) => this.formInputChangeHandler(e)}
                                documentType={this.editedRowData.documentType}
                                documentName={this.editedRowData.documentName}
                                isVisibleToCustomer={this.editedRowData.isVisibleToCustomer}
                                isVisibleOutOfCompany={this.editedRowData.isVisibleOutOfCompany}></ModalPopupEditView>}
                    </Modal>}

                <CustomModal modalData={modelData} />
            </Fragment>

        );
    }
}
export default ContractDocuments;