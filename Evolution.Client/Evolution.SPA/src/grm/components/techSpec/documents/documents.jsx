import React, { Component, Fragment } from 'react';
import { HeaderData } from './headerData';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { getlocalizeData, bindAction, isEmpty,compareObjects,isEmptyReturnDefault,isUndefined,uploadedDocumentCheck } from '../../../../utils/commonUtils';
import { required } from '../../../../utils/validator';
import dateUtil from '../../../../utils/dateUtil';
import CustomModal from '../../../../common/baseComponents/customModal';
import Modal from '../../../../common/baseComponents/modal';
import { configuration } from '../../../../appConfig';
import  TechSpecDocumentMenuButtons  from '../../../../components/documents/documents';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import PropTypes from 'prop-types';
import { commonAPIConfig  } from '../../../../apiConfig/apiConfig';
import { applicationConstants } from '../../../../constants/appConstants';
import { isEditable,isViewable } from '../../../../common/selector';
import {  levelSpecificActivities } from '../../../../constants/securityConstant';
const localConstant = getlocalizeData();
const ModalPopupAddView = (props) => {
    return (
        <Fragment>
            <CustomInput hasLabel={true}
                label={localConstant.documents.SELECT_FILE_TYPE}
                divClassName='col pl-0 mb-4'
                colSize="s6"
                type='select'
                //required={true}
                labelClass="mandate"
                className="customInputs browser-default"
                optionsList={props.techSpecDocumentTypeData}
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
                    switchLabel={localConstant.modalConstant.VISIBLE_TO_TS}
                    isSwitchLabel={true}
                    switchName="isVisibleToTS"
                    id="isVisibleToCustomer"
                    colSize="s3 m4 mt-4"
                    checkedStatus={props.isVisibleToTS}
                    onChangeToggle={props.handlerChange}
                />
            </div> */}
        </Fragment>
    );
};

const ModalPopupEditView = (props) => {
    let work = [];
    let selectedData = [];
    if(!isEmpty(props.editedTechSpecDocuments)){
        work = props.draftDocuments.filter(x=>x.id === props.editedTechSpecDocuments.id);
    } 
    if(!isEmpty(props.selectedProfileDraftDocuments)){
        selectedData = props.selectedProfileDraftDocuments.filter(x => x.id === props.editedTechSpecDocuments.id);
    } 
    return (
        <Fragment>
            <CustomInput hasLabel={true}
                label={localConstant.documents.SELECT_FILE_TYPE}
                divClassName='col pl-0'
                colSize="s6"
                type='select'
                //required={true}
                inputClass={"browser-default customInputs " + (props.compareDraftData(props.editedTechSpecDocuments.documentType,(selectedData.length>0?selectedData[0].documentType:undefined), (work.length>0?work[0].documentType:undefined))?"inputHighlight":"test")}                
                optionsList={props.techSpecDocumentTypeData}
                optionName='name'
                optionValue="name"
                onSelectChange={props.handlerChange}
                name="documentType"
                id="documentType"
                defaultValue={props.editedTechSpecDocuments.documentType}
            />
            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.documents.DOCUMENT_NAME}
                type='text'
                colSize='s6'
                inputClass={"browser-default customInputs " + (props.compareDraftData(props.editedTechSpecDocuments.documentName,(selectedData.length>0?selectedData[0].documentName:undefined), (work.length>0?work[0].documentName:undefined))?"inputHighlight":"")}               
                labelClass="mandate"
                name="documentName"
                disabled={true}
                defaultValue={props.editedTechSpecDocuments.documentName}
            />

            {/* <div className="row">
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.modalConstant.VISIBLE_TO_TS}
                    isSwitchLabel={true}
                    switchName="isVisibleToTS"
                    id="isVisibleToCustomerEdit"
                    colSize="s3 m4 mt-4"
                    checkedStatus={props.editedTechSpecDocuments.isVisibleToTS}
                    onChangeToggle={props.handlerChange}
                />
            </div> */}
        </Fragment>
    );
};
class TechnicalSpecialistDocuments extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isDocumentModalOpen: false,
            isDocumentModelAddView: true,
            isPanelOpen: false,            
            isRowUpdate:false,
        };
        this.updatedData = {};
        this.filesToDisplay = [];
        this.uploadButtonDisable = false;
        this.copiedDocumentDetails = [];
        this.updateFromRC_RM=[];
        //SystemRole based UserType relevant Quick Fixes 
        this.userTypes = isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE));
        const functionRefs = {};
        functionRefs["enableEditColumn"] = this.enableEditColumn;
        this.headerData = HeaderData(functionRefs);       
    };
    
    enableEditColumn = (e) => { //SystemRole based UserType relevant Quick Fixes 
        if (this.props.isTMUserTypeCheck && !isViewable({ activities: this.props.activities, levelType: 'LEVEL_3',viewActivityLevels: levelSpecificActivities.viewActivitiesTM }) && !this.props.isRCUserTypeCheck) {
            return true;
        }//D1374
        return !isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel1 }) && !isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel0 }); //D1374 Failed on 29-10-2020
    }

    componentDidMount=()=>{
        this.props.actions.FetchDocumentTypeMasterData(); //To fetch master document data
        //if(!(this.props.interactionMode && this.props.pageMode === localConstant.commonConstants.VIEW)){
         //Drag and Drop File Upload
         this.dropArea = document.getElementById("drop-area");
         // Prevent default drag behaviors
         if(this.dropArea){
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
         
        //}
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
        if((localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RM))|| (localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RC))){
            this.props.actions.IsRCRMUpdatedDocumentInformation(true);
        } 
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
                const visibleToTSStatus = this.updatedData.isVisibleToTS ? this.updatedData.isVisibleToTS : false;
                //let visibleToTSStatus = false;
                //const techSpecDocType=this.props.fetchDocumentTypeMasterData.filter(x => x.moduleName === "Technical Specialist");
                // for(let i=0;i<techSpecDocType.length;i++)
                // {
                //     if (techSpecDocType[i].name === docType) { 
                //         visibleToTSStatus = techSpecDocType[i].isTSVisible;
                //     }
                // }
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
                            moduleCode: "TS",
                            requestedBy: this.props.loggedInUser,
                            //moduleCodeReference:this.props.currentPage ==="Create Profile"?0:this.props.epin // D1186 & D1188
                        };
                    
                        this.updatedData = {
                            documentName: filesToBeUpload[i].name,
                            documentType: docType,
                            documentSize:  filesToBeUpload[i].size == 0 ? 0 : parseInt(filesToBeUpload[i].size / 1024) == 0 ? 1 : parseInt(filesToBeUpload[i].size / 1024),//def1126
                            createdOn: date,
                           // isVisibleToCustomer: customerVisibleStatus,
                            isVisibleToTS: visibleToTSStatus,
                            recordStatus: "N",//def 855 doc audit changes
                            canDelete:true,
                            id: Math.floor(Math.random() * (Math.pow(10, 5))),
                            modifiedBy: this.props.loggedInUser,
                            moduleCode: "TS",
                            //moduleRefCode: this.props.currentPage ==="Create Profile"?0:this.props.epin, // D1186 & D1188
                            subModuleRefCode: -1,
                            status: "C",
                            requestedBy: this.props.loggedInUser,
                            isUploaded: false
                        };
                        documentUniqueName.push(this.uniqueName);
                        createdNewRecords.push(this.updatedData);
                        this.props.actions.AddTechSpecDocDetails(createdNewRecords);
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
                    IntertekToaster(failureFiles.toString() + localConstant.documents.FILE_LIMIT_EXCEDED, 'warningToast techSpecDocSizeReq');
                }

            }
            else { 
                IntertekToaster(localConstant.documents.NO_FILE_SELECTED, 'warningToast techSpecNoFileSelectedReq');
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
                            moduleCodeReference: this.props.currentPage ==="Create Profile"?0:this.props.epin,
                            canDelete: true,
                            isUploaded: true
                        };
                    }
                    this.props.actions.UpdateTechSpecDocDetails(updatedDataAfterUploaded, this.UploadedFile[0]);
                }
            }
        }
        else {
            const afterRemovefailedDoc = this.props.documentrowData.filter(x => x.documentName !== uniqueNameDetail.documentName);
            this.props.actions.UpdateTechSpecDocDetails(afterRemovefailedDoc);
        }

        if (this.props.documentrowData.length > 0) {
            const queueDocs = this.props.documentrowData.find(doc => doc.isUploaded === false);
            if (queueDocs) {
                const tempUniqueName = {
                    documentName: queueDocs.documentName,
                    moduleCode: "TS",
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
        if((localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RM))|| (localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RC))){
            this.props.actions.IsRCRMUpdatedDocumentInformation(true);
        } 
        this.updatedData[result.name] = result.value;
    }
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
            IntertekToaster(localConstant.documents.SELECTED_RECORDS_COPIED, 'warningToast techSpecDocumentCopyMesgReq');
        }
        else {
            IntertekToaster(localConstant.documents.SELECT_RECORDS_TO_COPY, 'warningToast techSpecDocumentCopyReq');
        }
    }
    pasteDocumentHandler = () => {
        const records = [];
        let recordToPaste = [];
        recordToPaste = this.copiedDocumentDetails;
        if (recordToPaste.length > 0) {
            let date = new Date();
            date = dateUtil.postDateFormat(date, '-');
            for(let i=0;i<recordToPaste.length;i++){
                recordToPaste[i].moduleCode="TS";
                recordToPaste[i].moduleRefCode=0;//this.props.currentPage ==="Create Profile"?0:this.props.epin;enabledocument
            }
            this.props.actions.PasteDocumentUploadData(recordToPaste)
                .then(response => {
                    if (!isEmpty(response)) {
                        for (let i = 0; i < response.length; i++) {
                            this.updatedData = {
                                documentName: response[i].documentName,
                                documentType: recordToPaste[i].documentType,
                                documentSize: recordToPaste[i].documentSize,
                                createdOn: date,
                                                              
                               // isVisibleToCustomer: recordToPaste[i].isVisibleToCustomer,
                                isVisibleToTS: recordToPaste[i].isVisibleToTS,
                                recordStatus: "M",
                                canDelete:true,
                                id: Math.floor(Math.random() * (Math.pow(10, 5))),
                                documentUniqueName: response[i].uniqueName,
                                modifiedBy: this.props.loggedInUser,
                                moduleCode: response[i].moduleCode,
                                //moduleRefCode: response[i].moduleCodeReference,  enabledocument
                                subModuleRefCode: -1,
                                status: "C"
                            };
                            records.push(this.updatedData);
                            this.updatedData = {};
                        }
                        // this.props.actions.AddTechSpecDocumentDetails(records);
                        this.props.actions.AddTechSpecDocDetails(records);
                    }
                });
        }
        else {
            IntertekToaster(localConstant.documents.COPY_RECORDS_TO_PASTE, 'warningToast documentRecordToPasteReq');
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
            IntertekToaster(localConstant.documents.SELECT_ATELEAST_ONE_DOC_TO_DELETE, 'warningToast oneDocumentReq');
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
        let deletedRecords   = this.child.getSelectedRows();
        deletedRecords =  deletedRecords.filter(x => x.canDelete == true); 
        const selectedRecords = deletedRecords; // this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const isFileUploaded = uploadedDocumentCheck(selectedRecords);
            if (isFileUploaded) {
                this.child.removeSelectedRows(selectedRecords);
                this.props.actions.DeleteTechSpecDocDetails(selectedRecords);
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
        if((localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RM))|| (localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RC))){
            this.props.actions.IsRCRMUpdatedDocumentInformation(true);
        } 
        if (isEmpty(this.updatedData.documentType)) {
            IntertekToaster(localConstant.documents.SELECT_FILE_TYPE, 'warningToast techSpecDocFileTypeReq');
            return false;
        }
        this.fileupload(documentFile);
        // let date = new Date();
        // date = dateUtil.postDateFormat(date, '-');
        // // if (isEmpty(this.updatedData.documentType)) {
        // //     IntertekToaster(localConstant.documents.SELECT_FILE_TYPE, 'warningToast techSpecDocFileTypeReq');
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
        //         const techSpecDocType=this.props.fetchDocumentTypeMasterData.filter(x => x.moduleName === "Technical Specialist");
        //         for(let i=0;i<techSpecDocType.length;i++)
        //         {
        //             if (techSpecDocType[i].name === docType) {
        //                 visibleToTSStatus = techSpecDocType[i].isTSVisible;
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
        //                     moduleCode: "TS",
        //                     requestedBy: this.props.loggedInUser,
        //                     moduleCodeReference:this.props.currentPage ==="Create Profile"?0:this.props.epin ,
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
        //                                     recordStatus: "M",
        //                                     id: Math.floor(Math.random() * (Math.pow(10, 5))),
        //                                     documentUniqueName: response[i].uploadedFileName,
        //                                     modifiedBy: this.props.loggedInUser,
        //                                     moduleCode: response[i].moduleCode,
        //                                     moduleRefCode: response[i].moduleReferenceCode,
        //                                     subModuleRefCode: -1,
        //                                     status: "C"
        //                                 };
        //                                 newlyCreatedRecords.push(this.updatedData);
        //                                 this.updatedData = {};
        //                             }
        //                         }
        //                         if (newlyCreatedRecords.length > 0) {
        //                             this.props.actions.AddTechSpecDocDetails(newlyCreatedRecords);
        //                         }
        //                     };
        //                 });
        //         }
        //         if (failureFiles.length > 0) {
        //             IntertekToaster(failureFiles.toString() + localConstant.documents.FILE_LIMIT_EXCEDED, 'warningToast techSpecDocSizeReq');
        //         }

        //     }
        //     else {
        //         IntertekToaster(localConstant.documents.NO_FILE_SELECTED, 'warningToast techSpecNoFileSelectedReq');
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
            IntertekToaster(localConstant.documents.SELECT_FILE_TYPE, 'warningToast techSpeFileTypeReq');
            return false;
        }
        else {
            if (this.updatedData.documentType) {
                const techSpecDocType=this.props.fetchDocumentTypeMasterData.filter(x => x.moduleName === "Technical Specialist");
                for(let i=0;i<techSpecDocType.length;i++)
                {
                    if (techSpecDocType[i].name === this.updatedData.documentType) {
                        this.updatedData["isVisibleToTS"] = techSpecDocType[i].isTSVisible;
                    }
                }
            }
            this.updatedData["createdOn"] = date;
            if (this.editedRowData.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
                this.updateFromRC_RM.push(this.updatedData);
            }
            this.props.actions.UpdateTechSpecDocDetails(this.updatedData, this.editedRowData);
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
        // if (parent.props.documentrowData.recordStatus !== "N") {
        //  this.updatedData["uploadedOn"] = date;             
        //  this.updatedData["recordStatus"] = "M";
        //  }     
        parent.props.actions.UpdateTechSpecDocDetails(this.updatedData, data);
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
    excludeProperty = (object) => {
        delete object.recordStatus;
        delete object.lastModification;
        delete object.updateCount;
        delete object.modifiedBy;
        delete object.roleBase;
        return object;
    }
    excludeGridProperty = (object) => {
        delete object.recordStatus;
        delete object.lastModification;
        delete object.updateCount;
        delete object.modifiedBy;
        delete object.roleBase;
        delete object.documentType;
        return object;
    }
    excludeCurrentProperty = (object) => {
        delete object.roleBase;
        return object;
    }
    documentsDraftData = (originalData) => {    
        let work = 0;
        if (originalData && originalData.data && originalData.data.recordStatus!="M" && originalData.data.recordStatus!="N") {
            if (!isEmpty(this.props.draftDocuments) && ( this.props.isRCUserTypeCheck || this.props.isRMUserTypeCheck )) {
                work = this.props.draftDocuments.filter(x => x.id === originalData.data.id);
                if (work.length > 0) {
                    const result = compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));
                    if (!result && work[0].documentType != originalData.data.documentType ) {
                        return true;
                    }
                    else if (!result ) {
                        return false;   //RCRM Editing exgisting Row not showing highlight 
                    }
                } else if (!isEmpty(this.updateFromRC_RM)) {
                    work = this.updateFromRC_RM.filter(x => x.id === originalData.data.id);
                    if (work.length > 0) {
                        let currentResult = false;
                        if(work[0].documentType === undefined){
                            currentResult = compareObjects(this.excludeGridProperty(work[0]), this.excludeGridProperty(originalData.data));    
                        }else{
                            currentResult = compareObjects(work[0], this.excludeCurrentProperty(originalData.data));
                        }
                        return !currentResult;
                    }
                    else if (this.props.isRCRMUpdate) {
                        work = this.props.selectedProfileDraftDocuments.filter(x => x.id === originalData.data.id);
                        if (work.length > 0) {
                            const compareDataResult = compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));                                                       
                            return compareDataResult;
                        }
                    }
                }else if(!isEmpty(this.props.selectedProfileDraftDocuments)){
                        work = this.props.selectedProfileDraftDocuments.filter(x => x.id === originalData.data.id);
                        if (work.length > 0) {
                        const compareDataResult = compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));                                                       
                        return compareDataResult;
                    }
                }else{
                    return false;
                }
                
            } else {
                this.updateFromRC_RM = [];
                return false;
            }
        }
        return false;
    }
        /** Compare field value with draft */
        compareDraftData = (draftValue,selectedDraftValue,fieldValue) => {
            const _this = this;
            if(!isEmpty(_this.props.draftDataToCompare) && ( this.props.isRCUserTypeCheck || this.props.isRMUserTypeCheck )){
                if(!_this.props.isRCRMUpdate){
                    if(draftValue === undefined && !required(fieldValue)){
                        return true;
                    }
                    else if(draftValue !== undefined && draftValue !== fieldValue){
                        return true;
                    }
                    return false;   
                }else if(_this.props.isRCRMUpdate){
                    //RC RM While modifing Value Not showing Highlight
                    if(draftValue === undefined && !required(fieldValue)){
                        return false;
                    }
                    else if(draftValue !== undefined && draftValue !== fieldValue){
                        if(selectedDraftValue === fieldValue){
                            return true;
                        }
                        return false;
                    }
                    return false; 
                }                    
            }
            return false;
            };

    render() {     
        const {
            fetchDocumentTypeMasterData,
            documentrowData,
            interactionMode,
            draftDataToCompare,
            draftDocuments,
            selectedProfileDraft,            
            selectedProfileDraftDocuments,
           selectedProfileDetails,

        } = this.props;
        const rowDocData=documentrowData && documentrowData.filter(record => record.recordStatus !== "D");
        this.documentAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.hideModal,
                btnID: "cancelDocumentSubmit",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
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
        bindAction(this.headerData.DocumentsHeader, "EditTechSpecDocuments", this.editRowHandler);

        for(let i = 0; i < rowDocData.length; i++){
            rowDocData[i] = { ...rowDocData[i],roleBase: ((this.props.interactionMode || this.props.pageMode === localConstant.commonConstants.VIEW || ((selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !this.props.isTSUserTypeCheck)) || this.enableEditColumn())) }; //SystemRole based UserType relevant Quick Fixes  //ITK QA D1306 -- 02-09-2020
            //rowDocData[i] = { ...rowDocData[i],roleBase: true }; //SystemRole based UserType relevant Quick Fixes => ///D653 #11 issue (ref by 12-03-2020 ALM Doc ) enabledocument
            rowDocData[i] = { ...rowDocData[i],isFileUploaded: ((isUndefined(rowDocData[i].documentUniqueName))?false:(this.props.interactionMode || (selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !this.props.isTSUserTypeCheck || this.enableEditColumn()))?false:true) }; //ITK QA D1306 -- 02-09-2020
        } 
        return (
            <Fragment>
                <div className="customCard">
                    <h6 className="bold col s3 pl-0 pr-3">{localConstant.documents.DOCUMENTS}</h6>
                    <TechSpecDocumentMenuButtons
                        rowData={rowDocData}
                        headerData={this.headerData.DocumentsHeader}
                        copyDocumentHandler={this.copyDocumentHandler}
                        pasteDocumentHandler={this.pasteDocumentHandler}
                        uploadDocumentHandler={this.uploadDocumentHandler}
                        deleteDocumentHandler={this.deleteDocumentHandler}
                        DownloadDocumentHandler={this.documentmultipleDownloadHandler}
                        interactionMode={!this.props.interactionMode && !(selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM && !this.props.isTSUserTypeCheck || this.enableEditColumn()) } //ITK QA D1306 -- 02-09-2020
                        //interactionMode={false} //D653 #11 issue (ref by 12-03-2020 ALM Doc ) enabledocument
                        supportFileType={configuration}
                        onCellchange={this.onCellchange} 
                        onRef={ref => { this.child = ref; }}
                        draftDataToCompare={draftDataToCompare}
                        selectedDraftDataToCompare={this.props.selectedDraftDataToCompare}
                        selectedProfileDraftDocuments={this.props.selectedProfileDraftDocuments}
                        rowClass={{ rowHighlight: true }}
                        draftData={this.documentsDraftData}
                        pageMode={this.props.pageMode} //D667
                        paginationPrefixId={localConstant.paginationPrefixIds.techSpecDocumentGridId}
                    />
                </div>
                {this.state.isDocumentModalOpen &&
                    <Modal id="techSpecDocModalPopup"
                        title={this.state.isDocumentModelAddView ? localConstant.documents.UPLOAD_DOC : localConstant.documents.EDIT_DOC}
                        modalClass="techSpecDocModal"
                        buttons={this.documentAddButtons}
                        isShowModal={this.state.isDocumentModalOpen}>

                        {this.state.isDocumentModelAddView ? 
                        <ModalPopupAddView
                            techSpecDocumentTypeData={fetchDocumentTypeMasterData.filter(x => x.moduleName === "Technical Specialist")}
                            handlerChange={(e) => this.formInputChangeHandler(e)}
                            supportFileType={configuration}
                        />
                            : <ModalPopupEditView
                            techSpecDocumentTypeData={fetchDocumentTypeMasterData.filter(x => x.moduleName === "Technical Specialist")}
                                handlerChange={(e) => this.formInputChangeHandler(e)}
                                editedTechSpecDocuments={this.editedRowData}
                                draftDocuments={this.props.draftDocuments}
                                compareDraftData={this.compareDraftData}
                                selectedProfileDraftDocuments={selectedProfileDraftDocuments} 

                            />}
                    </Modal>}
                <CustomModal modalData={modelData} />
            </Fragment>
        );
    }
}

export default TechnicalSpecialistDocuments;
TechnicalSpecialistDocuments.propTypes = {
    documentrowData: PropTypes.array,
    documentsTypeData:PropTypes.array
};

TechnicalSpecialistDocuments.defaultProps = {
    documentrowData: [],
    documentsTypeData:[]
};