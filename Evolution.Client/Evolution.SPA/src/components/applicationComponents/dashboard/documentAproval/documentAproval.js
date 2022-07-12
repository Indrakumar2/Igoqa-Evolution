import React,{ Component,Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData.js';
import PropTypes from 'proptypes';
import LabelwithValue from '../../../../common/baseComponents/customLabelwithValue';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData,formInputChangeHandler, isEmpty,bindAction } from '../../../../utils/commonUtils';
import { modalTitleConstant } from '../../../../constants/modalConstants';
import Modal from '../../../../common/baseComponents/modal';
import ErrorList from '../../../../common/baseComponents/errorList';
import CustomModal from '../../../../common/baseComponents/customModal';
import moment from 'moment';
import FileToBeOpen from '../../../../common/commonDocumentDownload';
const localConstant = getlocalizeData();

class DocumentAproval extends Component{
    constructor(props){
        super(props);
        this.state = {
            isChangeModule:false,
            isApproveModalOpen:false,
            errorList:[]
        };
        this.updatedData={};
        this.errors=[];
        this.errorListButton =[
            {
              name: localConstant.commonConstants.OK,
              action: this.closeErrorList,
              btnID: "closeErrorList",
              btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
              showbtn: true
            }
          ];
         
          this.approvedButton =[
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelApproveModal,
                btnID: "cancelApproveModal",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
              },

            {
              name: localConstant.commonConstants.APPROVE,
              action: this.submitApprove,
              btnID: "submitApprove",
              btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
              showbtn: true
            }

          ];
          this.changeModuleButton =[

            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelApprove,
                btnID: "changeModuleCancelApproveModal",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
              },
            {
                name: localConstant.commonConstants.APPROVE,
                action: this.submitApprove,
                btnID: "changeModuleSubmitApprove",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
              },
           
          ];
    }

    componentDidMount(){
        // const elems = document.querySelectorAll('.modal');
        // const instances = MaterializeComponent.Modal.init(elems, { dismissible: false });
        this.props.actions.FetchDocumentApproval();
    }

    changeModule = () => {
        this.setState({
            isChangeModule:true
        });
    }

    cancelApprove = (e) => {
        e.preventDefault();
        this.setState({
            isChangeModule:false
        });
        this.updatedData['contractNumber']="";
        this.updatedData['projectNumber']="";
        this.updatedData['assignmentNumber']="";
        this.updatedData['assignmentFormattedNumber']="";
        this.updatedData['supplierPONumber']="";
        this.updatedData['timesheetNumber']="";
        this.updatedData['visitNumber']="";
        const updatedSelectedRow=this.props.selectedDocumentToApprove;
        const index=this.props.documentApprovalGridData.findIndex(row =>(row.id === updatedSelectedRow.id));
            this.updatedData['moduleCode']=this.props.documentApprovalGridData[index].moduleCode;
            this.updatedData['moduleRefCode']=this.props.documentApprovalGridData[index].moduleRefCode;
            this.updatedData['documentType']=this.props.documentApprovalGridData[index].documentType;
            const moduleIndex=localConstant.commonConstants.documentApprovalmoduleType.findIndex(type=>(type.code===this.updatedData.moduleCode));
                if(moduleIndex>=0){
                    this.updatedData["moduleName"]= localConstant.commonConstants.documentApprovalmoduleType[moduleIndex].name;
                }
        this.props.actions.FetchDocumentType(this.updatedData.moduleName);
        this.props.actions.UpdateSelectedRecord(this.updatedData);
        this.updatedData={};
    }

    cancelApproveModal=(e)=>{
        e.preventDefault();
        this.setState({
            isApproveModalOpen:false
        });
        this.updatedData={};
    }

    SelectedDocumentToApprove=(data)=>{
        this.setState({
            isApproveModalOpen:true,
        });
       this.props.actions.SelectedDocumentToApprove(data);
    }

    RejectDocument=(data)=>{
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message:`${ data.documentName }` + " " +
                    "  Are you sure you want to remove this document?",
            modalClassName: "warningToast",
            type: "confirm",
            buttons: [
              {
                buttonName: "Yes",
                onClickHandler:(e)=> this.rejectDocument(data),
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

    rejectDocument =(data)=>{
        this.props.actions.HideModal();
        this.props.actions.RejectDocument(data);
    }

    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    };

    submitApprove=(e)=>{
        e.preventDefault();
            const valid = this.validationCheck();
            if (valid === true) {
                const submitedValues=Object.assign({}, this.props.selectedDocumentToApprove);
                this.props.actions.ApproveDocument(submitedValues);
                this.cancelApproveModal(e);
                this.updatedData={};
            }
    }

    closeErrorList =(e) =>{
        e.preventDefault();
        this.setState({
          errorList:[]
        });
        this.errors = [];
      }

    validationCheck =()=>{
            const updatedSelectedRow=this.props.selectedDocumentToApprove;
            const index=this.props.documentApprovalGridData.findIndex(row =>(row.id === updatedSelectedRow.id));
            if(this.props.documentApprovalGridData[index].moduleCode === updatedSelectedRow.moduleCode){
                this.updatedData['documentType']=this.props.documentApprovalGridData[index].documentType;
                return true;
            } 
            else{
                const mandatoryFieldsValid = this.mandatoryFieldsValidationCheck();
                if (mandatoryFieldsValid === true) {
                    return true;
                } else {
                    return false;
                }
            }    
    }

    mandatoryFieldsValidationCheck =()=>{
        const updatedSelectedRow=this.props.selectedDocumentToApprove;
        if(updatedSelectedRow.moduleCode === 'CNT' && isEmpty(updatedSelectedRow.moduleRefCode)){
            this.errors.push(`${ localConstant.modalConstant.DOCUMENT_APPROVAL } - ${ localConstant.modalConstant.CONTRACT_NUMBER }`);
          }
          if(updatedSelectedRow.moduleCode !== 'CNT' && isEmpty(updatedSelectedRow.moduleRefCode)){
              if(isEmpty(updatedSelectedRow.contractNumber)){
                 this.errors.push(`${ localConstant.modalConstant.DOCUMENT_APPROVAL } - ${ localConstant.modalConstant.CONTRACT_NUMBER }`);
              }
              if(isEmpty(updatedSelectedRow.projectNumber)){
                 this.errors.push(`${ localConstant.modalConstant.DOCUMENT_APPROVAL } - ${ localConstant.modalConstant.PROJECT_NUMBER }`);
              }
              if(updatedSelectedRow.moduleCode === 'SUPPO'){
                 this.errors.push(`${ localConstant.modalConstant.DOCUMENT_APPROVAL } - ${ localConstant.modalConstant.SUPPLIER_PO }`);
              } else if(updatedSelectedRow.moduleCode !== 'PRJ'){
                         if(isEmpty(updatedSelectedRow.assignmentNumber)){
                             this.errors.push(`${ localConstant.modalConstant.DOCUMENT_APPROVAL } - ${ localConstant.modalConstant.ASSIGNMENT_NUMBER }`);
                         }
                         if(updatedSelectedRow.moduleCode === "TIME"){
                             this.errors.push(`${ localConstant.modalConstant.DOCUMENT_APPROVAL } - ${ localConstant.modalConstant.TIMESHEET }`);
                         } 
                         if(updatedSelectedRow.moduleCode === "VST"){
                             this.errors.push(`${ localConstant.modalConstant.DOCUMENT_APPROVAL } - ${ localConstant.modalConstant.VISIT }`);
                         }
              }
          }
          if(isEmpty(updatedSelectedRow.documentType)){
             this.errors.push(`${ localConstant.modalConstant.DOCUMENT_APPROVAL } - ${ localConstant.modalConstant.DOCUMENT_TYPE }`);
           }
          if(this.errors.length > 0){
             this.setState({
               errorList:this.errors
             });
             return false;
         } else{
             return true;
         }
    }

    inputChangeHandler =(e)=>{
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
            if(inputvalue.name === "documentType"){
                this.updatedData['documentType']=inputvalue.value;
                this.props.actions.UpdateSelectedRecord(this.updatedData);
            }
            if(inputvalue.name === "moduleCode"){
                this.updatedData['moduleRefCode']="";
                this.updatedData['contractNumber']="";
                this.updatedData['projectNumber']="";
                this.updatedData['assignmentNumber']="";
                this.updatedData['assignmentFormattedNumber']="";
                this.updatedData['supplierPONumber']="";
                this.updatedData['timesheetNumber']="";
                this.updatedData['visitNumber']="";
                this.updatedData['documentType']="";
                this.props.actions.UpdateSelectedRecord(this.updatedData);
                const selectedOption = e.nativeEvent.target[e.nativeEvent.target.selectedIndex];
                this.props.actions.FetchDocumentType(selectedOption.text!==localConstant.commonConstants.TIMESHEET?selectedOption.text:"Visit");
            }
            if(inputvalue.name === "contractNumber"){
                this.props.actions.FetchProjectForDocumentApproval(inputvalue.value);
                this.updatedData['contractNumber']=inputvalue.value;
                this.updatedData['moduleRefCode']="";
                this.props.actions.UpdateSelectedRecord(this.updatedData);
            }
            if(inputvalue.name === "projectNumber"){
                this.props.actions.FetchAssignmentForDocumentApproval(inputvalue.value);
                this.props.actions.FetchSupplierPOForDocumentApproval(inputvalue.value);
                this.updatedData['projectNumber']=inputvalue.value;
                this.updatedData['moduleRefCode']="";
                this.props.actions.UpdateSelectedRecord(this.updatedData);
            }
            if(inputvalue.name === "assignmentNumber"){
                this.updatedData['assignmentNumber']=inputvalue.value;
                this.updatedData['moduleRefCode']="";
                this.props.actions.UpdateSelectedRecord(this.updatedData);
                if(this.props.selectedDocumentToApprove.moduleCode === "TIME"){
                    this.props.actions.FetchTimesheetForDocumentApproval(inputvalue.value);
                } else if(this.props.selectedDocumentToApprove.moduleCode === "VST"){
                    this.props.actions.FetchVisitForDocumentApproval(inputvalue.value);
                }
            }
            if(this.props.selectedDocumentToApprove.moduleCode === "CNT" && inputvalue.name === "contractNumber"){
                this.updatedData['moduleRefCode']=inputvalue.value;
                this.updatedData['moduleName']='Contract';
                this.props.actions.UpdateSelectedRecord(this.updatedData);
            }
            if(this.props.selectedDocumentToApprove.moduleCode !== "CNT"){
                if(inputvalue.name === "projectNumber" && this.props.selectedDocumentToApprove.moduleCode === "PRJ"){
                    this.updatedData['moduleRefCode']=inputvalue.value;
                    this.updatedData['moduleName']='Project';
                    this.props.actions.UpdateSelectedRecord(this.updatedData);
                } else if(this.props.selectedDocumentToApprove.moduleCode !=="PRJ" && this.props.selectedDocumentToApprove.moduleCode === "SUPPO"){
                    if(inputvalue.name === "supplierPONumber"){
                        this.updatedData['moduleRefCode']=inputvalue.value;
                        this.updatedData['moduleName']='Supplier PO';
                        this.props.actions.UpdateSelectedRecord(this.updatedData);
                    }
                } else if(this.props.selectedDocumentToApprove.moduleCode !=="PRJ"){
                    if(inputvalue.name === "assignmentNumber" && this.props.selectedDocumentToApprove.moduleCode === "ASGMNT"){
                        this.updatedData['moduleRefCode']=inputvalue.value;
                        this.updatedData['moduleName']='Assignment';
                        this.props.actions.UpdateSelectedRecord(this.updatedData);
                    } else if(this.props.selectedDocumentToApprove.moduleCode ==="TIME"){
                        if(inputvalue.name === "timesheetNumber"){
                            this.updatedData['moduleRefCode']=inputvalue.value;
                            this.updatedData['moduleName']='Timesheet';
                            this.props.actions.UpdateSelectedRecord(this.updatedData);
                        }
                    } else if(this.props.selectedDocumentToApprove.moduleCode ==="VST"){
                        if(inputvalue.name === "visitNumber"){
                            this.updatedData['moduleRefCode']=inputvalue.value;
                            this.updatedData['moduleName']='Visit';
                            this.props.actions.UpdateSelectedRecord(this.updatedData);
                        }
                    }
                }
            }
            this.updatedData={};
    }
  
    render(){   
        const { selectedDocumentToApprove,documentType,selectedDocumentCustomer,documentApprovalGridData,
                contractsForDocumentApproval,projectForDocumentApproval,assignmentForDocumentApproval,
                supplierPOForDocumentApproval,timesheetForDocumentApproval,visitForDocumentApproval } = this.props;
        const rowData =[];
        const data=[];
        const headData = HeaderData;
        const documentApprovalRowData=documentApprovalGridData.filter(row=>row.isForApproval !== false && row.isForApproval !== null);
        documentApprovalRowData.map(value=>{
            if(value.moduleCode){
              const index=localConstant.commonConstants.documentApprovalmoduleType.findIndex(type=>(type.code===value.moduleCode));
              if(index>=0){
                data["moduleName"]= localConstant.commonConstants.documentApprovalmoduleType[index].name;
                const rowvalue=Object.assign({},value,  data);
                rowData.push(rowvalue);
              }
            }
        });
           
        bindAction(headData, "approve", this.SelectedDocumentToApprove);
        bindAction(headData, "reject", this.RejectDocument);
        
        return(
            <Fragment>
                {/* Reject Modal */}
                <CustomModal />
                {this.state.errorList.length > 0 ?
                    <Modal title={ localConstant.commonConstants.CHECK_MANDATORY_FIELD }
                    titleClassName="chargeTypeOption"
                    modalContentClass="extranetModalContent"
                    modalClass="ApprovelModal"
                    modalId="errorListPopup"
                    formId="errorListForm"
                    modalClass="popup-position nestedModal"
                    overlayClass="nestedOverlay"
                    buttons={this.errorListButton}
                    isShowModal={true}>
                         <ErrorList errors={this.state.errorList}/>
                    </Modal> : null
                }
            
                {this.state.isApproveModalOpen ? 
                    <Modal title={localConstant.modalConstant.APPROVE_DOCUMENT} 
                    modalId="approveDocumentPopup" formId="sapproveDocumentForm" 
                    modalClass="popup-position"                  
                    onSubmit={(e)=>this.submitSubSupplier(e)}
                    buttons={this.state.isChangeModule ?  this.changeModuleButton: this.approvedButton } 
                    isShowModal={this.state.isApproveModalOpen}>
                    {/* <form className="col s12">
                        <div className="modal-content pb-0"> */}
                        <div className="row mb-0">
                         {this.state.isApproveModalOpen && !this.state.isChangeModule?
                                <p className="bold pl-2 mt-0 mb-0">{localConstant.modalConstant.DOCUMENT_DETAILS}</p>:
                                <div className="row mb-0">
                                <p className="col s6 bold mt-0 mb-0">{localConstant.modalConstant.CURRENT_DETAILS}</p>
                                <p className="col s6 bold mt-0 mb-0">{localConstant.modalConstant.MOVE_TO}</p>
                                </div>
                                }
                                </div>
                            <div className="row mb-0">
                                <div className="col s12 m6">
                                    {/* <LabelwithValue
                                    colSize="s12 line-h-2"
                                    label ={localConstant.modalConstant.FILE_NAME}
                                    value={selectedDocumentToApprove.documentName} /> */}
                                    <div className={'col s12 line-h-2'}>
                                     <label className="bold">{localConstant.modalConstant.FILE_NAME} </label>
                                    <FileToBeOpen data={selectedDocumentToApprove} dataToRender='documentName' documentName={true} />
                                    </div>
                                     <LabelwithValue
                                    colSize="s12 line-h-2"
                                    label={localConstant.modalConstant.SIZE }
                                    value={selectedDocumentToApprove.documentSize +"KB"} />
                                     {/* <LabelwithValue
                                    colSize="s12 line-h-2"
                                    label={localConstant.modalConstant.DOCUMENT_TYPE}
                                    value="sample.jpg" /> */}
                                     <CustomInput
                                        hasLabel={true}
                                        label={localConstant.modalConstant.DOCUMENT_TYPE}
                                        labelClass="customLabel"
                                        name="documentType"
                                        type='select'
                                        optionsList={documentType}
                                        optionName='name'
                                        optionValue="name"
                                        colSize='m12 s12'
                                        inputClass="customInputs browser"
                                        onSelectChange={this.inputChangeHandler} 
                                        defaultValue={selectedDocumentToApprove.documentType}/>
                                    {
                                        this.state.isChangeModule ?
                                        <Fragment>
                                            
                                            <LabelwithValue
                                            colSize="s12 line-h-2"
                                            label={localConstant.modalConstant.UPLOADED_BY}
                                            value={selectedDocumentToApprove.createdBy} />
                                            <LabelwithValue
                                            colSize="s12 line-h-2"
                                            label={localConstant.modalConstant.UPLOADED_DATE}
                                            value={moment(selectedDocumentToApprove.createdOn).format('DD-MM-YYYY HH:mm')} />
                                            <LabelwithValue
                                            colSize="s12 line-h-2"
                                            label={localConstant.modalConstant.ENTITY}
                                            value={selectedDocumentToApprove.moduleRefCode}/>
                                        </Fragment>:null
                                    }
                                </div>
                                <div className="col s12 m6 leftBorder">   
                                    {
                                        !this.state.isChangeModule ?
                                        <Fragment>
                                           
                                             <LabelwithValue
                                                colSize="s8 line-h-2"
                                                label={localConstant.modalConstant.MODULE}
                                                value={selectedDocumentToApprove.moduleName} />
                                                {/* <span className="link" onClick={this.changeModule}><i title="Change Module"className="zmdi zmdi-edit edit-size"></i></span> */}
                                                <a  onClick={this.changeModule} className="link login-anchor pr-0" >Change Module</a>            
                                                <LabelwithValue
                                                colSize="s12 line-h-2"
                                                label={localConstant.modalConstant.UPLOADED_BY}
                                                value={selectedDocumentToApprove.createdBy} />
                                                <LabelwithValue
                                                colSize="s12 line-h-2"
                                                label={localConstant.modalConstant.UPLOADED_DATE}
                                                value={moment(selectedDocumentToApprove.createdOn).format('DD-MM-YYYY HH:mm')} />
                                                <LabelwithValue
                                                colSize="s12 line-h-2"
                                                label={localConstant.modalConstant.ENTITY}
                                                value={selectedDocumentToApprove.moduleRefCode} />
                                        </Fragment>:null
                                    }  
                                    {
                                        this.state.isChangeModule ?
                              
                                       <Fragment>  
                                        <CustomInput
                                        hasLabel={true}
                                        label={localConstant.modalConstant.MODULE}
                                        labelClass="customLabel "
                                        name="moduleCode"
                                        type='select'
                                        optionsList={localConstant.commonConstants.documentApprovalmoduleType}
                                        optionName='name'
                                        optionValue="code"
                                        colSize='m12 s12'
                                        inputClass="customInputs browser"
                                        onSelectChange={this.inputChangeHandler}
                                        defaultValue={selectedDocumentToApprove.moduleCode}
                                         />

                                        <LabelwithValue
                                        colSize="s12 line-h-2"
                                        label={localConstant.modalConstant.CUSTOMER}
                                        value={selectedDocumentCustomer ? selectedDocumentCustomer[1] :"" } />

                                         <CustomInput
                                        hasLabel={true}
                                        label={localConstant.modalConstant.CONTRACT_NUMBER}
                                        labelClass="customLabel "
                                        type='select'
                                        name="contractNumber"
                                        optionsList={contractsForDocumentApproval}
                                        optionName='contractNumber'
                                        optionValue="contractNumber"
                                        colSize='m12 s12'
                                        inputClass="customInputs browser"
                                        onSelectChange={this.inputChangeHandler}
                                        defaultValue={selectedDocumentToApprove.contractNumber}
                                         />
                                      
                                      {  selectedDocumentToApprove.moduleCode !== "CNT" ? 
                                      <div>
                                         <CustomInput
                                        hasLabel={true}
                                        label={localConstant.modalConstant.PROJECT_NUMBER}
                                        labelClass="customLabel "
                                        type='select'
                                        name="projectNumber"
                                        optionsList={projectForDocumentApproval}
                                        optionName='projectNumber'
                                        optionValue="projectNumber"
                                        colSize='m12 s12'
                                        inputClass="customInputs browser"
                                        onSelectChange={this.inputChangeHandler}
                                        defaultValue={selectedDocumentToApprove.projectNumber} />
                                        </div> : null }

                                        {  selectedDocumentToApprove.moduleCode === "SUPPO" ? <div>
                                         <CustomInput
                                        hasLabel={true}
                                        label={localConstant.modalConstant.SUPPLIER_PO}
                                        labelClass="customLabel"
                                        type='select'
                                        name="supplierPONumber"
                                        optionsList={supplierPOForDocumentApproval}
                                        optionName='supplierPONumber'
                                        optionValue="supplierPOId"
                                        colSize='m12 s12'
                                        inputClass="customInputs browser"
                                        onSelectChange={this.inputChangeHandler}
                                        defaultValue={selectedDocumentToApprove.supplierPONumber} />
                                         </div> : null }

                                        {  selectedDocumentToApprove.moduleCode !== "CNT"    &&
                                           selectedDocumentToApprove.moduleCode !== "SUPPO" &&
                                           selectedDocumentToApprove.moduleCode !== "PRJ" ? <div>
                                         <CustomInput
                                        hasLabel={true}
                                        label={localConstant.modalConstant.ASSIGNMENT_NUMBER}
                                        labelClass="customLabel"
                                        type='select'
                                        name="assignmentNumber"
                                        optionsList={assignmentForDocumentApproval}
                                        optionName='assignmentFormattedNumber'
                                        optionValue="assignmentId"
                                        colSize='m12 s12'
                                        inputClass="customInputs browser"
                                        onSelectChange={this.inputChangeHandler}
                                        defaultValue={selectedDocumentToApprove.assignmentNumber} />
                                         </div> : null }

                                         {  selectedDocumentToApprove.moduleCode === "TIME" ? <div>
                                         <CustomInput
                                        hasLabel={true}
                                        label={localConstant.modalConstant.TIMESHEET}
                                        labelClass="customLabel"
                                        type='select'
                                        name="timesheetNumber" 
                                        optionsList={timesheetForDocumentApproval}
                                        optionName='documentApprovalVisitValue'
                                        optionValue="timesheetId"
                                        colSize='m12 s12'
                                        inputClass="customInputs browser"
                                        onSelectChange={this.inputChangeHandler}
                                        defaultValue={selectedDocumentToApprove.timesheetNumber} />
                                         </div> : null }

                                         {  selectedDocumentToApprove.moduleCode === "VST" ? <div>
                                         <CustomInput
                                        hasLabel={true}
                                        label={localConstant.modalConstant.VISIT}
                                        labelClass="customLabel"
                                        type='select'
                                        name="visitNumber"
                                        optionsList={visitForDocumentApproval}
                                        optionName='documentApprovalVisitValue'
                                        optionValue="visitId"
                                        colSize='m12 s12'
                                        inputClass="customInputs browser"
                                        onSelectChange={this.inputChangeHandler}
                                        defaultValue={selectedDocumentToApprove.visitNumber} />
                                         </div> : null }

                                       </Fragment>
                                    
                                       :null
                                    }
                                </div>
                             </div>
                        {/* </div> */}
                        {/* <div className="modal-footer col s12">
                            {
                                this.state.isChangeModule ?
                                <button type="button" onClick={this.cancelApprove} className="waves-effect waves-teal btn-small">{localConstant.commonConstants.CANCEL}</button>:
                                <button type="button" onClick={this.cancelApproveModal} className="modal-close waves-effect waves-teal btn-small">{localConstant.commonConstants.CANCEL}</button>                            
                            }
                            <button type="reset" onClick={this.submitApprove} className="modal-close waves-effect waves-teal btn-small ml-2">{localConstant.commonConstants.APPROVE}</button>
                        </div> */}
                    {/* </form> */}
                    </Modal> : null 
                }
                {/* <div id="RejectDocument" className="modal popup-position">
                    <form className="col s12">
                        <div className="modal-content pb-0">
                            <div className="row mb-0">
                                <h6>{localConstant.modalConstant.REJECT_DOCUMENT}</h6>
                                <p> {modalMessageConstant.DOCUMENT_APPROVAL_DELETE_MESSAGE}</p>
                            </div>
                        </div>
                        <div className="modal-footer col s12">
                            <button type="reset"  className="modal-close waves-effect waves-teal btn-small ml-2">{localConstant.commonConstants.CLOSE}</button>
                        </div>
                    </form>
                </div> */}
                {/* Aprove Modal */}
                {/* <div id="approveDocument" className="modal popup-position">
                    <form className="col s12">
                        <div className="modal-content pb-0">
                            <div className="row mb-0">
                                <h6>{localConstant.modalConstant.APPROVE_DOCUMENT}</h6>
                                <div className="col s12 m6">
                                    <LabelwithValue
                                    colSize="s12 line-h-2"
                                    label ={localConstant.modalConstant.FILE_NAME}
                                    value={selectedDocumentToApprove.documentName} />
                                     <LabelwithValue
                                    colSize="s12 line-h-2"
                                    label={localConstant.modalConstant.SIZE}
                                    value={selectedDocumentToApprove.documentSize} />
                                     <LabelwithValue
                                    colSize="s12 line-h-2"
                                    label={localConstant.modalConstant.DOCUMENT_TYPE}
                                    value="sample.jpg" />
                                     <CustomInput
                                        hasLabel={true}
                                        label={localConstant.modalConstant.DOCUMENT_TYPE}
                                        labelClass="customLabel"
                                        name="documentType"
                                        type='select'
                                        optionsList={documentType}
                                        optionName='name'
                                        optionValue="name"
                                        inputName='newPayrollPeriodName'
                                        colSize='m12 s12'
                                        inputClass="customInputs browser"
                                        onSelectChange={this.inputChangeHandler} 
                                        defaultValue={selectedDocumentToApprove.documentType}/>
                                    {
                                        this.state.isChangeModule ?
                                        <Fragment>
                                            <LabelwithValue
                                            colSize="s12"
                                            label={localConstant.modalConstant.UPLOADED_BY}
                                            value={selectedDocumentToApprove.createdBy} />
                                            <LabelwithValue
                                            colSize="s12"
                                            label={localConstant.modalConstant.UPLOADED_DATE}
                                            value={selectedDocumentToApprove.createdOn} />
                                            <LabelwithValue
                                            colSize="s12"
                                            label={localConstant.modalConstant.ENTITY}
                                            value={selectedDocumentToApprove.moduleRefCode}/>
                                        </Fragment>:null
                                    }
                                </div>
                                <div className="col s12 m6 leftBorder">   
                                    {
                                        !this.state.isChangeModule ?
                                        <Fragment>
                                             <LabelwithValue
                                                colSize="s11 line-h-2"
                                                label={localConstant.modalConstant.MODULE}
                                                value={selectedDocumentToApprove.moduleName} />
                                                <span className="link" onClick={this.changeModule}><i className="zmdi zmdi-edit"></i></span>
                                                <LabelwithValue
                                                colSize="s12 line-h-2"
                                                label={localConstant.modalConstant.UPLOADED_BY}
                                                value={selectedDocumentToApprove.createdBy} />
                                                <LabelwithValue
                                                colSize="s12 line-h-2"
                                                label={localConstant.modalConstant.UPLOADED_DATE}
                                                value={selectedDocumentToApprove.createdOn} />
                                                <LabelwithValue
                                                colSize="s12 line-h-2"
                                                label={localConstant.modalConstant.ENTITY}
                                                value={selectedDocumentToApprove.moduleRefCode} />
                                        </Fragment>:null
                                    }  
                                    {
                                        this.state.isChangeModule ?
                                       <Fragment>
                                  
                                        <CustomInput
                                        hasLabel={true}
                                        label={localConstant.modalConstant.MODULE}
                                        labelClass="customLabel "
                                        name="moduleCode"
                                        type='select'
                                        optionsList={localConstant.commonConstants.documentApprovalmoduleType}
                                        optionName='name'
                                        optionValue="code"
                                        inputName='newPayrollPeriodName'
                                        colSize='m12 s12'
                                        inputClass="customInputs browser"
                                        onSelectChange={this.inputChangeHandler}
                                        defaultValue={selectedDocumentToApprove.moduleCode}
                                         />

                                        <LabelwithValue
                                        colSize="s12 line-h-2"
                                        label={localConstant.modalConstant.CUSTOMER}
                                        value={selectedDocumentCustomer} />

                                         <CustomInput
                                        hasLabel={true}
                                        label={localConstant.modalConstant.CONTRACT_NUMBER}
                                        labelClass="customLabel "
                                        type='select'
                                        name="contractNumber"
                                        optionsList={contractsForDocumentApproval}
                                        optionName='contractNumber'
                                        optionValue="contractNumber"
                                        inputName='newPayrollPeriodName'
                                        colSize='m12 s12'
                                        inputClass="customInputs browser"
                                        onSelectChange={this.inputChangeHandler} />
                                      
                                      {  selectedDocumentToApprove.moduleCode !== "CNT" ? <div>
                                         <CustomInput
                                        hasLabel={true}
                                        label={localConstant.modalConstant.PROJECT_NUMBER}
                                        labelClass="customLabel "
                                        type='select'
                                        name="projectNumber"
                                        optionsList={projectForDocumentApproval}
                                        optionName='projectNumber'
                                        optionValue="projectNumber"
                                        inputName='newPayrollPeriodName'
                                        colSize='m12 s12'
                                        inputClass="customInputs browser"
                                        onSelectChange={this.inputChangeHandler} />
                                        </div> : null }

                                        {  selectedDocumentToApprove.moduleCode === "SUPPO" ? <div>
                                         <CustomInput
                                        hasLabel={true}
                                        label={localConstant.modalConstant.SUPPLIER_PO}
                                        labelClass="customLabel"
                                        type='select'
                                        optionsList={supplierPOForDocumentApproval}
                                        optionName='supplierPONumber'
                                        optionValue="supplierPONumber"
                                        inputName='newPayrollPeriodName'
                                        colSize='m12 s12'
                                        inputClass="customInputs browser" />
                                         </div> : null }

                                        {  selectedDocumentToApprove.moduleCode !== "CNT"    &&
                                           selectedDocumentToApprove.moduleCode !== "SUPPO" &&
                                           selectedDocumentToApprove.moduleCode !== "PRJ" ? <div>
                                         <CustomInput
                                        hasLabel={true}
                                        label={localConstant.modalConstant.ASSIGNMENT_NUMBER}
                                        labelClass="customLabel"
                                        type='select'
                                        optionsList={assignmentForDocumentApproval}
                                        optionName='assignmentNumber'
                                        optionValue="assignmentId"
                                        inputName='newPayrollPeriodName'
                                        colSize='m12 s12'
                                        inputClass="customInputs browser" />
                                         </div> : null }

                                         {  selectedDocumentToApprove.moduleCode === "TIME" ? <div>
                                         <CustomInput
                                        hasLabel={true}
                                        label={localConstant.modalConstant.TIMESHEET}
                                        labelClass="customLabel"
                                        type='select'
                                        optionsList={[ { value: "option 1" },{ value: "option 2" } ]}
                                        optionName='value'
                                        optionValue="value"
                                        inputName='newPayrollPeriodName'
                                        colSize='m12 s12'
                                        inputClass="customInputs browser" />
                                         </div> : null }

                                         {  selectedDocumentToApprove.moduleCode === "VST" ? <div>
                                         <CustomInput
                                        hasLabel={true}
                                        label={localConstant.modalConstant.VISIT}
                                        labelClass="customLabel"
                                        type='select'
                                        optionsList={[ { value: "option 1" },{ value: "option 2" } ]}
                                        optionName='value'
                                        optionValue="value"
                                        inputName='newPayrollPeriodName'
                                        colSize='m12 s12'
                                        inputClass="customInputs browser" />
                                         </div> : null }

                                       </Fragment>
                                       :null
                                    }
                                </div>
                            </div>
                        </div>
                        <div className="modal-footer col s12">
                            {
                                this.state.isChangeModule ?
                                <button type="button" onClick={this.cancelApprove} className="waves-effect waves-teal btn-small">{localConstant.commonConstants.CANCEL}</button>:
                                <button type="button" onClick={this.cancelApprove} className="modal-close waves-effect waves-teal btn-small">{localConstant.commonConstants.CANCEL}</button>                            
                            }
                            <button type="reset" onClick={this.submitApprove} className="modal-close waves-effect waves-teal btn-small ml-2">{localConstant.commonConstants.APPROVE}</button>
                        </div>
                    </form>
                </div> */}
                <ReactGrid gridRowData={rowData} gridColData={headData} 
                paginationPrefixId={localConstant.paginationPrefixIds.dashboardDocumentApprovalGridId}/>
            </Fragment>
            
        );
    }
}
DocumentAproval.prototypes = {
    headData:PropTypes.array.isrequired,
    rowData:PropTypes.array.isrequired
};
DocumentAproval.defaultprops ={
    headData:[],
    rowData:[]
};
export default DocumentAproval;