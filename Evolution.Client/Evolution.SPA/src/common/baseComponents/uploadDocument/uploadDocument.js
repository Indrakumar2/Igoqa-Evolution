import React, { Component, Fragment } from 'react';
import { configuration } from '../../../appConfig';
import IntertekToaster from '../../baseComponents/intertekToaster';
import dateUtil from '../../../utils/dateUtil';
import { getlocalizeData, isEmpty, isEmptyReturnDefault, isEmptyOrUndefine } from '../../../utils/commonUtils';
const localConstant = getlocalizeData();
class UploadDocument extends Component {
    constructor(props) {     
        super(props);
        this.state = {
            disableDocument: false,
        };
        this.newlyCreatedRecords=[];
    } 
    uploadDocument = (e) => {  
        e.preventDefault();
        let date = new Date();
        date = dateUtil.postDateFormat(date, '-');
        let documentFile = [];
        const isSyncProcess = this.props.gridName === localConstant.techSpec.common.TECHNICALSPECIALISTSTAMP || this.props.gridName === localConstant.techSpec.common.TECHNICALSPECIALISTINTERNALTRAINING || this.props.gridName === localConstant.techSpec.common.TECHNICALSPECIALISTCOMPETANCY || this.props.gridName === localConstant.techSpec.common.TECHNICALSPECIALISTCUSTOMERAPPROVAL || this.props.gridName === localConstant.techSpec.common.TECHNICALSPECIALISTEDUCATION ||
                               this.props.gridName === localConstant.techSpec.common.TECHNICALSPECIALISTCERTIFICATION || this.props.gridName === localConstant.techSpec.common.TECHNICALSPECIALISTTRAINING || this.props.gridName === localConstant.techSpec.common.TECHNICALSPECIALISTDOCUMENTS; //Sync upload need for Resource Module
        if(this.props.id !== undefined){
            documentFile = Array.from(document.getElementById(this.props.id).files);
        }else{
            documentFile = Array.from(document.getElementById('uploadFiles').files);
        }
        
        if (documentFile.length > 0) {          
            
            const filesToBeUpload = [];
            const failureFiles = [];
            const documentUniqueName = [];
            documentFile.map(document => {
                if (parseInt(document.size / 1024) > configuration.fileLimit) {
                    failureFiles.push(document.name);
                }
                else {
                    filesToBeUpload.push(document);
                }
                return document;
            });
            if (filesToBeUpload.length > 0) {
                for (let i = 0; i < filesToBeUpload.length; i++) {
                    this.updatedData = {
                        documentName: filesToBeUpload[i].name,
                        moduleCode: "TS",
                        requestedBy: this.props.loggedInUser,
                        documentType:this.props.documentType,
                    };
                    if(this.props.currentPage === localConstant.techSpec.common.EDIT_VIEW_TECHSPEC){
                        this.updatedData.moduleCodeReference = `${ this.props.selectedEpinNo }`;
                    } // Added For ITK Def 1538 
                    documentUniqueName.push(this.updatedData);
                    this.updatedData = {};
                }
                this.props.actions.FetchDocumentUniqueName(documentUniqueName, filesToBeUpload,isSyncProcess) //Sync upload need for Resource Module
                    .then(response => {
                        if (!isEmpty(response) &&response!==[ null ]) {
                            for (let i = 0; i < response.length; i++) {
                                if (!response[i].status) {
                                    this.updatedData = {
                                        documentName: filesToBeUpload[i].name,
                                        createdOn: date,
                                        recordStatus: "N", //def 855 doc audit changes
                                        id: Math.floor(Math.random() * (Math.pow(10, 5))),
                                        documentUniqueName: response[i].uploadedFileName,
                                        modifiedBy: this.props.loggedInUser,
                                        moduleCode: response[i].moduleCode,
                                        moduleRefCode: response[i].moduleReferenceCode,
                                        status: "C",
                                        uploadFileRefType:this.props.uploadFileRefType, 
                                        newlyUploadedDoc:true, //D223 changes 
                                        documentType:this.props.documentType,//def 848 doc highlight issue
                                    };
                                    this.newlyCreatedRecords.push(this.updatedData);
                                    //this.newlyCreatedTempRecoard.push(this.updatedData);
                                    this.updatedData = {};
                                }
                            }
                            if (this.newlyCreatedRecords.length > 0) {
                                if (this.props.gridName === "TechnicalSpecialistDocuments") {  
                                    delete this.newlyCreatedRecords[0].id;                                  
                                    this.props.actions.UpdateSensitiveDetails(this.newlyCreatedRecords[0], this.props.editDocDetails);
                                    this.props.actions.IsRemoveDocument(false);
                                }
                                else {
                                    this.props.actions.UploadDocumentDetails(this.newlyCreatedRecords, this.props.editDocDetails, this.props.gridName);
                                }
                                this.updatedData = {};
                            }
                        };
                    });
            }
            if (failureFiles.length > 0) {
                IntertekToaster(failureFiles.toString() + localConstant.project.documents.FILE_LIMIT_EXCEDED, 'warningToast contractDocSizeReq');
            }
        }
        else {
            IntertekToaster(localConstant.techSpec.common.NO_FILE_SELECTED, 'warningToast contractNoFileSelectedReq');
        }
    }
    
    cancelUploadedDocument = (e) => {
        let documentUniqueName = null;
        if (Array.isArray(this.props.defaultValue) && this.props.defaultValue.length > 0) {
            documentUniqueName = this.props.defaultValue[0].documentUniqueName;
        }
        else if (Array.isArray(this.newlyCreatedRecords) && this.newlyCreatedRecords.length > 0) {
            documentUniqueName = this.newlyCreatedRecords[0].documentUniqueName;
        }
        this.props.cancelUploadedDocument(e, documentUniqueName);
        this.newlyCreatedRecords = [];
    }

    render() {        
        const { label, defaultValue, Mode, className, cancelUploadedDocument,cancelDisplaynone,id,labelClass,disabled ,inputClass } = this.props;//D573 & 848 #1
        const closebtnclass=cancelDisplaynone ? "zmdi zmdi-hc-lg s1" : "zmdi zmdi-close zmdi-hc-lg s1 ";
        return (
            <Fragment>
                {
                    this.props.gridName === "TechnicalSpecialistDocuments" ?
                        (isEmpty(defaultValue) ?
                            <div className={className } >
                                <label className="mandate" >{label } </label>
                                <div className="file-field ">
                                    <div className="btn">
                                        <i className="zmdi zmdi-upload zmdi-hc-lg"></i>
                                        <input id={ id !== undefined ? id : "uploadFiles"} name='documentName' type="file" 
                                            onChange={this.uploadDocument} accept={configuration.allowedFileFormats}
                                            maxLength='100' />
                                    </div>
                                    <div className=" pl-0 file-path-wrapper ">
                                        <input className="browser-default file-path validate " />
                                    </div>
                                </div>
                            </div>

                            :
                            <div className="col s12 uploadedText">
                                <label className="col s12 p-0 mb-2">{label}</label>
                                <span className="pr-2 uploaedTextLimit">{defaultValue}</span>
                                <i onClick={cancelUploadedDocument} className="zmdi zmdi-close zmdi-hc-lg s1 " ></i>
                            </div>
                        ) : ( this.newlyCreatedRecords.length ==0 && ( isEmpty(defaultValue) || defaultValue[0].recordStatus === 'D') ?
                            <div className={className}>
                                <label className={labelClass}>{label}</label>
                                <div className="file-field ">
                                    <div className="btn">
                                        <i className="zmdi zmdi-upload zmdi-hc-lg"></i>
                                        <input disabled={disabled} id={ id !== undefined ? id : "uploadFiles"} name='documentName' type="file"
                                            onChange={this.uploadDocument} accept={configuration.allowedFileFormats}
                                            maxLength='100' />
                                    </div>
                                    <div className=" pl-0 file-path-wrapper ">
                                        <input disabled={disabled} className="browser-default file-path validate" />
                                    </div>
                                </div>
                            </div>

                            : //def 848 highlight fix
                            <div className="col s6 uploadedText"> 
                                <div className="row ml-1 ">
                                <label className="col s9 p-0 mb-2">{label}</label>
                                </div>
                                <div className="row ml-1 ">
                                   <span className={"pr-2 linebreak uploaedTextLimit " + inputClass }>{ isEmptyOrUndefine(defaultValue) ? this.newlyCreatedRecords[0].documentName : defaultValue[0].documentName }</span>
                                <i onClick={ this.cancelUploadedDocument } className={closebtnclass} ></i>
                                </div>
                            </div>
                        )
                }

            </Fragment>
        );
    }
}
export default UploadDocument;
UploadDocument.propTypes = {
};

UploadDocument.defaultProps = {
    disabled: true
}; 