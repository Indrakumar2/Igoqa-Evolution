import React, { Component,Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './timesheetRefHeader';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import {
    getlocalizeData,
    bindAction, 
    formInputChangeHandler,
    isEmptyOrUndefine,
    isEmpty,
    mergeobjects } from '../../../../utils/commonUtils';
import Modal from '../../../../common/baseComponents/modal';
import CustomModal from '../../../../common/baseComponents/customModal';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
const localConstant = getlocalizeData();

const TimeSheetRefModal = (props) => {
        return (
        <Fragment>
            <div className="col s12 p-0" >
                <CustomInput
                    hasLabel={true}
                    divClassName='col pl-0 pr-0'
                    label={localConstant.visit.REFERENCE_TYPE}
                    type='select'
                    name='referenceType'
                    colSize='s6'
                    className="browser-default"
                    labelClass="mandate"
                    optionsList={props.timesheetReferenceTypes}
                    optionName='referenceType'
                    optionValue='referenceType'
                    onSelectChange={props.onChangeHandler}
                    disabled={props.interactionMode}
                    defaultValue={props.eidtedReference.referenceType}
                    />
                   
                    {/* {console.log(props.eidtedReference.referenceValue)} */}
                     <CustomInput 
                    hasLabel={true}                    
                    labelClass="customLabel mandate"
                    name='referenceValue'
                    divClassName='col' 
                    label={localConstant.gridHeader.REFERENCE_VALUE}
                    type='text'
                    colSize='s12 m6'
                    maxLength={fieldLengthConstants.timesheet.timesheetReference.REFERENCE_VALUE_MAXLENGTH}
                    inputClass="customInputs"
                    required={true}
                    onValueChange={(e)=>props.onChangeHandler(e)}
                    defaultValue={isEmptyOrUndefine(props.eidtedReference.referenceValue) ?  localConstant.visit.DEFAULT_REFERENCE_VALUE : props.eidtedReference.referenceValue}//{props.eidtedReference.referenceValue}
                    disabled={props.interactionMode}
                />
            </div>
          
        </Fragment>
    );
};
class TimesheetReference extends Component {
    constructor(props) {
        super(props);
        this.state = {          
            isTimesheetRefShowModal: false,
            isTimesheetRefEdit: false,
            selectedRowData:{},
            isOpen:false
        };
        this.updatedData = {};

        const functionRefs = {};
        functionRefs["enableEditColumn"] = this.enableEditColumn;
        this.headerData = HeaderData(functionRefs);
    }

    enableEditColumn = (e) => {
        return this.props.interactionMode;
    }
  
    //All Input Handle get Name and Value
    inputHandleChange = (e) => {
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
    }
    
    showTimesheetRefModal = (e) => {
        e.preventDefault();      
        this.setState((state) => {
            return {
                isTimesheetRefShowModal: true,
                isTimesheetRefEdit: false
            };
        });        
        this.editedRowData = {};
    }
      //Hiding the modal
      hideTimesheetRefModal = () => {
        this.setState((state) => {
            return {
                isTimesheetRefShowModal: false,
            };
        });
        this.updatedData = {};
        this.editedRowData = {};
    }
     //Cancel the updated data in model popup
     canceltimesheetRefModal=(e)=>{
        e.preventDefault();
        this.updatedData = {};       
            this.setState((state) => {
                return {
                    isTimesheetRefShowModal: false,
                };
            });
    }
    editRowHandler=(data)=>{
        this.setState((state)=>{
            return {
                isTimesheetRefShowModal: true,
                isTimesheetRefEdit: true,
            };
        });
        this.editedRowData=data;
    }
    timesheetReferenceValidation = (data) => {
        // if(this.updatedData.referenceType === '' || this.updatedData.referenceType === undefined){
        data.referenceValue=isEmptyOrUndefine(data.referenceValue) ?  localConstant.visit.DEFAULT_REFERENCE_VALUE : data.referenceValue;  
        if (isEmptyOrUndefine(data.referenceType)) {
            IntertekToaster(localConstant.assignments.PLEASE_FILL_REFERENCE_TYPE, 'warningToast');
            return false;
        }
        if (isEmptyOrUndefine(data.referenceValue)) {
            IntertekToaster(localConstant.assignments.PLEASE_FILL_REFERENCE_VALUE, 'warningToast');
            return false;
        }
        return true;
    }
    addNewTimesheetReference = (e) =>{
        e.preventDefault();
        this.updatedData["timesheetReferenceUniqueId"] = Math.floor(Math.random() * 99) -100;
        this.updatedData["timesheetReferenceId"] = null;
        this.updatedData["recordStatus"] = 'N';
        this.updatedData["timesheetId"] = this.props.timesheetId;
        this.updatedData["createdBy"] = this.props.loginUser;
        //Removeafter testing
        this.updatedData["lastModification"]= "2019-03-09T16:05:07";
        let isDuplicateReference; 
       
        if (!isEmpty(this.updatedData.referenceType)) {
            this.props.timesheetReferenceDataGrid.forEach((iteratedValue) => {
                if (iteratedValue.referenceType.toUpperCase() === this.updatedData.referenceType.toUpperCase() && iteratedValue.recordStatus !== "D") {
                    isDuplicateReference = true;
                } else {
                    isDuplicateReference = false;
                }
            });
            if (isDuplicateReference) {
                IntertekToaster(localConstant.assignments.REFERENCE_TYPE_EXISTS, 'warningToast duplicateScheduleName');
                return false;
            }
        }
        if (this.timesheetReferenceValidation(this.updatedData)) {
            this.props.actions.AddNewTimesheetReference(this.updatedData);
            this.hideTimesheetRefModal();
            this.updatedData = {};
        }
    }

    editTimesheetReference = (e) => {
        e.preventDefault();
        const combinedData = mergeobjects(this.editedRowData, this.updatedData);
        if (this.editedRowData.recordStatus !== "N") {
            this.updatedData["recordStatus"] = "M";
        }
        let isDuplicateReference;
        if (!isEmpty(this.updatedData.referenceType)) {
            if (this.editedRowData.referenceType !== this.updatedData.referenceType) {

                this.props.timesheetReferenceTypes.forEach((iteratedValue) => {
                    if (iteratedValue.referenceType.toUpperCase() === this.updatedData.referenceType.toUpperCase()) {
                        isDuplicateReference = true;
                    }
                });
                if (isDuplicateReference) {
                    IntertekToaster(localConstant.assignments.REFERENCE_TYPE_EXISTS, 'warningToast duplicateScheduleName');
                    return false;
                }
            }
        }
        this.updatedData["modifiedBy"] = this.props.loginUser;
        //Removeafter testing
        this.updatedData["lastModification"]= "2019-03-09T16:05:07";
        if (this.timesheetReferenceValidation(combinedData)) {
            this.props.actions.UpdatetTimesheetReference(this.updatedData,this.editedRowData);
            this.hideTimesheetRefModal();
            this.updatedData = {};
        }
    }

    deleteTimesheetReference = (e) => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.TIMESHEET_REFERENCE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSelectedTimesheetReference,
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
    deleteSelectedTimesheetReference = () => {
        const selectedData = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedData);
        this.props.actions.DeleteTimesheetReference(selectedData);
        this.props.actions.HideModal();
    }
    timesheetRefButtons(){
        return [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.canceltimesheetRefModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action:this.state.isTimesheetRefEdit?(e)=>this.editTimesheetReference(e):(e)=>this.addNewTimesheetReference(e),
                btnClass: "btn-small ",
                showbtn: true
            }
        ];
    }
    render() {
        const { timesheetReferenceDataGrid,timesheetReferenceTypes,interactionMode } = this.props;
        const modelData = { isOpen: this.state.isOpen };
        const timesheetReferenceData = timesheetReferenceDataGrid && timesheetReferenceDataGrid.filter((iteratedValue, i) => {
            return iteratedValue.recordStatus !== "D";
        });
        bindAction(this.headerData,"EditColumn",this.editRowHandler);
        return (
            <Fragment>
            {/* <CustomModal modalData={modelData} /> */}
            <div className="customCard">
            {this.state.isTimesheetRefShowModal &&
                <Modal
                   modalClass ="visitModal"
                    title={this.state.isTimesheetRefEdit 
                        ? localConstant.visit.ADD_TIMESHEET_REFERENCE:
                        localConstant.visit.EDIT_TIMESHEET_REFERENCE}
                    buttons={this.timesheetRefButtons()}
                    isShowModal={this.state.isTimesheetRefShowModal}>
                    <TimeSheetRefModal 
                        timesheetReferenceTypes={timesheetReferenceTypes}
                        onChangeHandler={(e)=>this.inputHandleChange(e)}
                        interactionMode={interactionMode}
                        eidtedReference = { this.editedRowData }
                    />
                </Modal>}
                <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.timesheet.TIMESHEET_REFERENCES} colSize="s12">
                <ReactGrid gridRowData={timesheetReferenceData}
                 gridColData={this.headerData} 
                 onRef = {ref => { this.child = ref; }} />
                   {this.props.pageMode!==localConstant.commonConstants.VIEW? <div className="right-align mt-2">
                            <a className="waves-effect btn-small"
                                onClick={this.showTimesheetRefModal}
                                disabled={interactionMode} >
                                {localConstant.commonConstants.ADD}</a>
                            <a className="btn-small ml-2 dangerBtn"
                                onClick={(e) => this.deleteTimesheetReference(e)}
                                disabled={interactionMode} >
                                {localConstant.commonConstants.DELETE}</a>
                    </div>:null}
                </CardPanel>
            </div>
            </Fragment>
        );
    }
}

export default TimesheetReference;
