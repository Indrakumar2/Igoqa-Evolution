import React, { Component, Fragment } from 'react';
import Modal from '../baseComponents/modal';
import CustomInput from '../baseComponents/inputControlls';
import ReactGrid from '../baseComponents/reactAgGrid';
import { configuration } from '../../appConfig';
import { getlocalizeData, isEmptyOrUndefine } from '../../utils/commonUtils';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import Editor from '../../common/baseComponents/quill/customEditor';
import { fieldLengthConstants } from '../../constants/fieldLengthConstants';
const localConstant = getlocalizeData();
//convert the component in stateless component

class ApprovelEmail extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isemailDocumentModal:false,
      attachedFiles:this.props.attachedFiles,
      emailObj:{
        emailSubject:this.props.emailSubject?this.props.emailSubject:'',
        emailToAddress:this.props.toAddress?this.props.toAddress:'',
        editorHtml: this.props.emailContent?this.props.emailContent:''
      }      
    };
    this.props.actions.AddAttachedDocuments(this.props.attachedFiles);
    const refAction  = this.props.approvelEmailButtons[1].action;
    this.props.approvelEmailButtons[1].action = (e)=>{
      refAction(e,this.state);
    };    
  
  this.documnetUploadEmailBtn= [
    {
      name: this.props.moduleCode==="VST"?localConstant.commonConstants.ADD_FROM_VISIT:localConstant.commonConstants.ADD_FROM_TIMESHEET,
      action: (e) => this.documentFromModule(e),
      btnID: "documentTimesheetEmailBtn",
      btnClass: "btn",      
      type:"button"
    },
    {
      name: localConstant.commonConstants.ADD_FROM_COMPUTER,
      action: (e) => this.documentFromComputer(e),
      btnID: "documentComputerEmailBtn",
      btnClass: "btn ml-2",       
      type:"file"
    }
  ];
  
  this.emailDocumentModalButtons = [
    {
      name: localConstant.commonConstants.CANCEL,
      action: (e) => this.closeDocumentModalPopup(e),
      btnID: "cancelApprovelEmailBtn",
      btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
      showbtn: true,
      type:"button"
    },
    {
      name: localConstant.commonConstants.ADD,
      action: (e) => this.addApprovelDocumnet(e),
      btnID: "addDocumentBtn",
      btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
      showbtn: true
    }   
  ];
}

   //Document Modal Popup
   documentFromModule=()=>{
    this.setState((state) => {
      return {
        isemailDocumentModal: !state.isemailDocumentModal
      };
    });
  }

  documentFromComputer = (e)=>{
    const uploadedFiles = Array.from(document.getElementById("documentComputerEmailBtn").files);
    if (uploadedFiles.length > 0) {
      const newlyCreatedRecords = [];
      const filesToBeUpload = [];
      const failureFiles = [];
      const documentUniqueName = [];

      uploadedFiles.map(document => {
          if (parseInt(document.size / 1024) > 16000) {
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
                  requestedBy: this.props.loggedInUser,
                  moduleCode: this.props.moduleCode,
                  moduleCodeReference: (this.props.moduleCodeReference ? this.props.moduleCodeReference.toString() : this.props.moduleCodeReference),
                  isFromModuleDocs:false,
                  documentType:'Evolution Email',
                  documentUniqueName:filesToBeUpload[i].name+Date.now(),
                  subModuleCodeReference: '-1' //D979 - Document added from computer while approving Visit/Timesheet
              };
              documentUniqueName.push(this.updatedData);
              this.updatedData = {};
          }
          this.props.actions.FetchDocumentUniqueName(documentUniqueName, filesToBeUpload, true)
              .then(response => {
                  if (!isEmptyOrUndefine(response)) {
                      for (let i = 0; i < response.length; i++) {
                          if (response[i] && !response[i].status) {
                              this.updatedData = {
                                  documentName: filesToBeUpload[i].name,                                  
                                  documentSize: parseInt(filesToBeUpload[i].size / 1024),
                                  recordStatus: "N",
                                  documentUniqueName: response[i].uploadedFileName,
                                  modifiedBy: this.props.loggedInUser,
                                  moduleCode: response[i].moduleCode,
                                  moduleRefCode: response[i].moduleReferenceCode,
                                  status: "C",
                                  documentType:'Evolution Email',
                                  isFromModuleDocs:false,
                              };
                              newlyCreatedRecords.push(this.updatedData);
                              this.updatedData = {};
                          }
                      }
                      this.setState((state) => {
                        return {
                          attachedFiles: state.attachedFiles.concat(newlyCreatedRecords)
                        };
                      });
                      this.props.actions.AddAttachedDocuments(this.state.attachedFiles);
                  };
                  this.closeDocumentModalPopup(e);
              });
              // this.setState((state) => {
              //   return {
              //     attachedFiles: state.attachedFiles.concat(documentUniqueName)
              //   };
              // });
              this.closeDocumentModalPopup(e);
      }
      if (failureFiles.length > 0) {
          IntertekToaster(failureFiles.toString() + localConstant.contract.Documents.FILE_LIMIT_EXCEDED, 'warningToast timesheetDocSizeReq');
      }
      if(document.getElementById("documentComputerEmailBtn") && document.getElementById("documentComputerEmailBtn").value) {
        document.getElementById("documentComputerEmailBtn").value = '';
      }
  }

  }

  closeDocumentModalPopup= (e)=>{
    e.preventDefault();
    this.setState((state) => {
      return {
        isemailDocumentModal: false
      };
    });
  }

  addApprovelDocumnet = (e)=>{
    e.preventDefault();
    const selectedDocs = this.child.getSelectedRows();
    if(selectedDocs.length >0){
     const attachedDocs =  selectedDocs.map((doc)=>{
          return {
            documentUniqueName:doc.documentUniqueName,
            documentName:doc.documentName,
            moduleCode:doc.moduleCode,
            moduleRefCode:doc.moduleRefCode,
            documentSize: doc.documentSize,
            isFromModuleDocs:true
          };
      });
      const allDocs = this.state.attachedFiles.concat(attachedDocs);
      this.setState((state) => {
        return {
          attachedFiles: allDocs
        };
      });
      this.props.actions.AddAttachedDocuments(allDocs);
      this.closeDocumentModalPopup(e);
      // attachedFiles
    }else{
      IntertekToaster('Select at least one Document to attach', 'warningToast documentCopyReq');
    }
  }
  emailTemplateChange = (e) => {
    if (!e.target) {
      this.setState(prevState => ({
        emailObj: {                   
          ...prevState.emailObj,    
          editorHtml: e
        }
      }));
    }
    if (e.target) {
      const targetName = e.target.name,
            targetValue = e.target.value;
      this.setState(prevState => ({
        emailObj: {
          ...prevState.emailObj,
          [targetName]: targetValue
        }
      }));
    }
  }

  removeAttachmant = async (e, doc) => {
    const removeDocUniqueName = doc.documentUniqueName;
    //Seen Uncessary block & console, hence commented
    // if (!doc.isFromModuleDocs) {
    //   console.log("remove uploaded doc from local storage");
    //   // const deleteUrl = projectAPIConfig.projectDocuments;
    //   // if (!isEmptyOrUndefine(doc)) {
    //   //   const res = await this.props.actions.RemoveDocumentsFromDB([ doc ], deleteUrl);
    //   // }    
    // }
    const attachedFiles = this.state.attachedFiles.filter((doc) => {
      return !(removeDocUniqueName === doc.documentUniqueName);
    });
    this.setState((state) => {
      return {
        attachedFiles: attachedFiles
      };
    });
    this.props.actions.AddAttachedDocuments(attachedFiles);
  }

render(){
  
  const { isemailTemplate, approvelEmailButtons, 
    documentGridHeaderData,
    documentGridRowData  } = this.props;
    const { emailObj } = this.state;
    
  return (
    <Fragment>
      <Modal title={'Send to Approval Email'}
        modalClass="ApprovelModal"
        modalId="approvelEmailPopup"
        formId="approvedEmailPopupFrom"
        buttons={approvelEmailButtons}
        isShowModal={isemailTemplate}>
        <div className="row mb-0">
          <CustomInput
            hasLabel={true}
            name="emailToAddress"
            colSize='s12 pl-0'
            label={'To'}
            type='text'
            dataValType ='valueText'
            inputClass="customInputs"
            value={emailObj.emailToAddress}
            onValueChange={this.emailTemplateChange} 
          />
          <label className="small right text-red">Emails to be seperated by semi colon(;)</label>
          <CustomInput
            hasLabel={true}
            name="emailSubject"
            colSize='s12 pl-0'
            label={'Subject'}
            type='text'
            dataValType ='valueText'
            inputClass="customInputs"
            value={emailObj.emailSubject}
            onValueChange={this.emailTemplateChange} 
            maxLength={fieldLengthConstants.visit.email.SUBJECT_MAXLENGTH}
          />
          <div className="col s6 pl-0">
            <h6 className="bold m-0">Attachments</h6>
            <ul className="attachmentlist">
            {
              this.state.attachedFiles.map((doc,i)=>(
               <li  key={i}>
                 {/* <a href={doc.documentName}  download>{doc.documentName}</a>  */}
                 {doc.documentName}
                 <i class="zmdi zmdi-close  ml-3" onClick = {(e)=>this.removeAttachmant(e,doc)}></i>
                 </li>
              ))
            }
            </ul>
          </div>
          <div className="col s6 right mt-1 mb-1">
            <div class="upload-btn-wrapper ml-2">
              {this.documnetUploadEmailBtn.map((button, i) => {
                return (
                  <Fragment key ={i}>
                    {button.type === 'file' ?
                    <Fragment>
                      <button key={i} 
                    className={button.btnClass} onClick={button.action} >{button.name}</button>
                    <input type={button.type || null} id={button.btnID || null} 
                    name={button.name} onSelect={button.action} onChange={button.action} accept={configuration.allowedFileFormats} multiple required/>
                  </Fragment>
                    :
                    <button type={(button.type) || null} 
                    disabled={button.disabled || null} 
                    id={button.btnID || null}
                     key={i} 
                     className={button.btnClass} 
                     onClick={(e) => button.action(e)}
                     >{button.name}</button>}
                  </Fragment>
                );
              })}
            </div>
          </div>
          <div className="col s12 mb-2">
           <Editor editorHtml={emailObj.editorHtml} 
           onChange = {this.emailTemplateChange}
           />
          </div>
        </div>
      </Modal>

      {/* Document Modal */}
      {this.state.isemailDocumentModal && <Modal title={'Select file to Upload'}
        modalClass="emailDocumentModal nestedModal"
        modalId="emailDocumentPopup"
        formId="emailDocumentFrom"
        overlayClass="nestedOverlay"
        buttons={this.emailDocumentModalButtons}
        isShowModal={this.state.isemailDocumentModal}>
        <div className="row">
              {/* D979 - Filter only document uploaded, not approved documents */}
              <ReactGrid gridRowData={documentGridRowData.filter(record => record.recordStatus !== "D" && record.subModuleRefCode === '0')} gridColData={documentGridHeaderData}                 
                onRef={ref => { this.child = ref; }}/>
        </div>
      </Modal>}
    </Fragment>
  );
}
}

export default ApprovelEmail;