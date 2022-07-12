import React,{ Component,Fragment } from 'react';
import { getlocalizeData,bindAction, isEmpty } from '../../../../utils/commonUtils';
import Modal from '../../../../common/baseComponents/modal';
import CustomModal from '../../../../common/baseComponents/customModal';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
import { required } from '../../../../utils/validator';
const localConstant = getlocalizeData();
const RateScheduleModal = (props) =>{
    const referenceType=props.referenceType.filter(x=>x.isVisibleToAssignment===true);
    
    return(
            <div className="row">

                <CustomInput
                    hasLabel={true}
                    type='select'
                    divClassName='col'
                    label={localConstant.gridHeader.REFERENCE_TYPE}
                    colSize='s6'
                    optionsList={referenceType}
                    optionName='referenceType'
                    optionValue="referenceType"
                    labelClass="mandate"
                    name="referenceType"
                    className="browser-default customInputs"
                    onSelectChange={(e)=>props.handlerChange(e)}
                    defaultValue={props.eidtedAssignmentReference && props.eidtedAssignmentReference.referenceType}
                />

                <CustomInput 
                    hasLabel={true}                    
                    labelClass="customLabel mandate"
                    name='referenceValue'
                    divClassName='col' 
                    label={localConstant.gridHeader.REFERENCE_VALUE}
                    type='text'
                    colSize='s12 m6'
                    inputClass="customInputs"
                    //maxLength={fieldLengthConstants.assignment.assignmentReferences.REFERENCE_VALUE_MAXLENGTH}
                    required={true}
                    onValueChange={(e)=>props.handlerChange(e)}
                    defaultValue={props.eidtedAssignmentReference.referenceValue} 
                    autocomplete="off" 
                    readOnly={props.interactionMode}
                />

            </div>
    );
};
class AssignmentReferences extends Component{
    constructor(props){
        super(props);
        this.updatedData = {};
        this.state = {
            isOpen: false,
            isAssignmentReferenceModalOpen:false,
            isAssignmentReferenceEditMode:false,          
            selectedRowData:{},
        };
        // const functionRefs = {};
        this.functionRefs = {};
        this.functionRefs["enableEditColumn"] = this.enableEditColumn;
        this.functionRefs["disabled"]=(this.props.isInterCompanyAssignment && this.props.isOperatingCompany)|| this.props.interactionMode? true:false;
        this.headerData = HeaderData(this.functionRefs);
    }
    addAssignmentReference = () => {
        this.setState({
            isAssignmentReferenceModalOpen:true,
            isAssignmentReferenceEditMode:false
        });
        this.editedRowData={};
    }
    cancelAssignmentReference = (e) => {
        e.preventDefault();
        this.setState({
            isAssignmentReferenceModalOpen:false
        });
        this.updatedData = {};
    }
    handlerChange = (e) => {
        const result = this.inputChangeHandler(e);
        this.updatedData[e.target.name] = result;
    }

    inputChangeHandler = (e) => {
        const value = e.target[e.target.type === "checkbox" ? "checked" : "value"];
        return value;
    }

    editRowHandler=(data)=>{
        this.setState((state)=>{
            return {
                isAssignmentReferenceModalOpen: true,
                isAssignmentReferenceEditMode:true
            };
        });
        this.editedRowData=data;
    }

    /** Assignment Reference validation */
    assignmentReferenceValidation = (data) => {
        if(isEmpty(data) || required(data.referenceType)){
            IntertekToaster(localConstant.assignments.PLEASE_SELECT_REFERENCE_TYPE,'warningToast AssignmentReferenceType');
            return false;
        }
        if(required(data.referenceValue)){
            IntertekToaster(localConstant.assignments.PLEASE_FILL_REFERENCE_VALUE,'warningToast AssignmentReferenceValue');
            return false;
        }
        return true;
    };

    submitNewAssignmentReference = (e) =>{
        e.preventDefault();
        this.updatedData["assignmentReferenceTypeAddUniqueId"] = Math.floor(Math.random() * 99) -100;
        this.updatedData["assignmentReferenceTypeId"] = null;
        this.updatedData["recordStatus"] = 'N';
        this.updatedData["assignmentId"] = this.props.assignmentId;
        this.updatedData["modifiedBy"] = this.props.loginUser;
        let isDuplicateReference=false; 
        if(this.assignmentReferenceValidation(this.updatedData)){
            this.props.assignmentReferenceDataGrid.forEach((iteratedValue)=>{
                if(iteratedValue.referenceType.toUpperCase() === this.updatedData.referenceType.toUpperCase()){
                    isDuplicateReference = true;
                }
            });
            if(isDuplicateReference){
                IntertekToaster(localConstant.assignments.REFERENCE_TYPE_EXISTS + this.updatedData.referenceType, 'warningToast duplicateScheduleName');
                return false;
            }
             
            this.props.actions.AddNewAssignmentReference(this.updatedData);
            this.setState({
                isAssignmentReferenceModalOpen:false
            });
            this.updatedData = {};
        }
    }
    isDuplicateReferenceType =(e)=>{
        this.props.actions.UpdatetAssignmentReference(this.updatedData,this.editedRowData);
        this.updatedData={};
        this.props.actions.HideModal();
    }
    editAssignmentReference = (data,e) => {
        this.editedRowData=data;
        let isDuplicateReference =false;
            if(e.target.value && this.editedRowData.referenceType !== e.target.value){
                this.props.assignmentReferenceDataGrid.forEach((iteratedValue)=>{
                    if(e.target.value && iteratedValue.referenceType.toUpperCase() === e.target.value.toUpperCase()){
                        isDuplicateReference = true;
                    }
                });
                 if(isDuplicateReference){
                    this.updatedData["referenceTypeValidation"]=localConstant.assignments.REFERENCE_TYPE_EXISTS + e.target.value; 
                    const confirmationObject = {
                        title: localConstant.assignments.ASSIGNMENT_REFERENCE,
                        message: localConstant.assignments.REFERENCE_TYPE_EXISTS + e.target.value,
                        type: "confirm",
                        modalClassName: "warningToast",
                        buttons: [
                            {
                                buttonName: localConstant.commonConstants.OK,
                                onClickHandler: this.isDuplicateReferenceType,
                                className: "modal-close m-1 btn-small"
                            }
                        ]
                    };
                    this.props.actions.DisplayModal(confirmationObject);
                }
                else{
                    this.updatedData[e.target.name] = e.target.value;
                    this.updatedData["referenceTypeValidation"]="";
                }
                if (this.editedRowData.recordStatus !== "N") {
                    this.updatedData["recordStatus"] = "M";
                }
            }
            this.updatedData["modifiedBy"] = this.props.loginUser;
            this.props.actions.UpdatetAssignmentReference(this.updatedData,this.editedRowData);
            this.updatedData={};
    }
    editReferenceValue = (data,e)=>{
        this.editedRowData=data;
        this.updatedData[e.target.name] = e.target.value;
        if(required(this.updatedData.referenceValue)){
            this.updatedData["referenceValueValidation"]=localConstant.assignments.PLEASE_FILL_REFERENCE_VALUE;
        }
        else{
            this.updatedData["referenceValueValidation"] = "";
        }
        if (this.editedRowData.recordStatus !== "N") {
            this.updatedData["recordStatus"] = "M";
        }
        this.updatedData["modifiedBy"] = this.props.loginUser;
        this.props.actions.UpdatetAssignmentReference(this.updatedData, this.editedRowData);
        this.updatedData = {};
    }
    deleteAssignmentReference = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.ASSIGNMENT_REFERENCE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSelectedAssignmentReference,
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: localConstant.commonConstants.NO,
                        onClickHandler: this.confirmationRejectHandler,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
        else {
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE,'warningToast idOneRecordSelectReq');
        }
    }
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }
    deleteSelectedAssignmentReference = () => {
        const selectedData = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedData);
        this.props.actions.DeleteAssignmentReference(selectedData);
        this.props.actions.HideModal();
    }
    componentDidMount = () => {
        // this.props.actions.FetchReferencetypes();
    }
    enableEditColumn = (e) => {
        return this.props.isInterCompanyAssignment && this.props.isOperatingCompany? true:false;
    }
    
    onCellFocused = (e) => {  
        if(e.rowIndex !== null) {     
            if(document.getElementById(e.column.getId() + e.rowIndex) !== null) {
                if(document.getElementById(e.column.getId() + e.rowIndex).type === 'text') {
                    document.getElementById(e.column.getId() + e.rowIndex).select();
                } else {
                    document.getElementById(e.column.getId() + e.rowIndex).focus();
                }
            }
        }
    }
    render(){
        const { interactionMode } = this.props;
        bindAction(this.headerData,"referenceValue",(data,e) => this.editReferenceValue(data,e));
        bindAction(this.headerData,"referenceType",(data,e) => this.editAssignmentReference(data,e));
        bindAction(this.headerData,"EditColumn",this.editRowHandler);
        if(this.props.assignmentReferenceTypes){
            // const referenceType=this.props.assignmentReferenceTypes.filter(x=>x.isVisibleToAssignment===true);
            this.headerData.columnDefs.forEach((iteratedValue) => {       
                if (iteratedValue.field && iteratedValue.field === "referenceType") {
                    if(isEmpty(iteratedValue.cellRendererParams.optionList)) {        
                       return iteratedValue.cellRendererParams.isAssignmentReference = true;//this.props.assignmentReferenceTypes;
                    }
                }
            });
        }
        const assignmentReferenceButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelAssignmentReference,
                btnID: "cancelAssignmentReference",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.state.isAssignmentReferenceEditMode?(e)=>this.editAssignmentReference(e):this.submitNewAssignmentReference,
                btnID: "editClientNotification",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        const assignmentReferenceDataGrid = this.props.assignmentReferenceDataGrid && this.props.assignmentReferenceDataGrid.filter((iteratedValue, i) => {
            return iteratedValue.recordStatus !== "D";
        });
        const isInterCompanyAssignment=this.props.isInterCompanyAssignment;
        return(
            <Fragment>
                <CustomModal modalData={modelData} />
                <div className="customCard">
                    {this.state.isAssignmentReferenceModalOpen && 
                        <Modal 
                            title={localConstant.assignments.ASSIGNMENT_REFERENCES}
                            modalId="classificaionModal"
                            formId="classificaionModal" 
                            modalClass="popup-position" 
                            buttons={assignmentReferenceButtons} 
                            isShowModal={this.state.isAssignmentReferenceModalOpen} 
                            disabled={interactionMode}>
                            <RateScheduleModal 
                                handlerChange = { this.handlerChange }
                                eidtedAssignmentReference = { this.editedRowData }
                                isEditMode = { this.state.isAssignmentReferenceEditMode }
                                referenceType = { this.props.assignmentReferenceTypes}
                                interactionMode={interactionMode}
                            />
                        </Modal>}
                    <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.assignments.ASSIGNMENT_REFERENCES} colSize="s12">
                        
                            <ReactGrid
                                gridRowData={assignmentReferenceDataGrid}
                                gridColData={this.headerData} 
                                onRef={ref => { this.child = ref; }} 
                                onCellFocused={this.onCellFocused} 
                                paginationPrefixId={localConstant.paginationPrefixIds.assignmentRef} />
                      
                        {this.props.pageMode !== localConstant.commonConstants.VIEW  &&<div className="right-align mt-2 mr-2">
                            <a onClick={this.addAssignmentReference}
                                disabled={(isInterCompanyAssignment && this.props.isOperatingCompany) ? true : false}
                                className="btn-small waves-effect modal-trigger">
                                {localConstant.commonConstants.ADD}
                            </a>
                            <a className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn"
                                disabled={(isInterCompanyAssignment && this.props.isOperatingCompany) ? true : false}
                                onClick={this.deleteAssignmentReference}>
                                {localConstant.commonConstants.DELETE}
                            </a>
                        </div>}
                    </CardPanel>
                </div>
            </Fragment>
        );
    }
}

export default AssignmentReferences;