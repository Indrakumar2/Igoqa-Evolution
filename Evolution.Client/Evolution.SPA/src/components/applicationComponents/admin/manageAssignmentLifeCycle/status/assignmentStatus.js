import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../../../common/baseComponents/reactAgGrid';
import Modal from '../../../../../common/baseComponents/modal';
import { getlocalizeData, bindAction ,formInputChangeHandler, isEmpty,mergeobjects } from '../../../../../utils/commonUtils';
import { modalMessageConstant, modalTitleConstant } from '../../../../../constants/modalConstants';
import { HeaderData } from './headerData';
import IntertekToaster from '../../../../../common/baseComponents/intertekToaster';
import CustomInput from '../../../../../common/baseComponents/inputControlls';
import CustomModal from '../../../../../common/baseComponents/customModal';
import CardPanel from '../../../../../common/baseComponents/cardPanel';
const localConstant = getlocalizeData();
const AssignmentStatusModalPopup = (props) => {
    return (
        <Fragment>
            <CustomInput
                hasLabel={true}
                divClassName='col pr-0'
                label={localConstant.admin.manageLifecycle.ASSIGMNMENT_STATUS}
                type='text'
                colSize='s6 '
                inputClass="customInputs"
                defaultValue={props.editedRowData && props.editedRowData.name}
                maxLength={15}
                name="name"
                onValueChange={props.formInputHandler}
                autocomplete="off"
                labelClass="mandate"
                disabled={ props.editedRowData && props.editedRowData.name ===localConstant.admin.manageLifecycle.COMPLETE || props.editedRowData.name ===localConstant.admin.manageLifecycle.IN_PROGRESS  && props.isEditMode ?true:false}

            />
            
        </Fragment>
    );

};
class Status extends Component {
    constructor(props){
        super(props);
        this.state={
         isShowModal:false,
         isEditMode:false
        };
        this.updatedData={};
        this.addStatus = [
            {

                name: localConstant.commonConstants.CANCEL,
                action: this.assignmentStatusModalClose,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type: "button"
            },
            {
                
                name: localConstant.commonConstants.SUBMIT,
                action: this.addAssignmentStatus,
                btnClass: "btn-small ",
                showbtn: true,
                type: "button"
            },

        ];
        this.editStatus= [
            {

                name: localConstant.commonConstants.CANCEL,
                action: this.assignmentStatusModalClose,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type: "button"
            },
            {
                
                name: localConstant.commonConstants.SUBMIT,
                action: this.updateAssignmentStatus,
                btnClass: "btn-small ",
                showbtn: true,
                type: "button"
            },

        ];
        bindAction(HeaderData, "EditColumn", this.editRowHandler);
    }
    componentDidMount(){
        if(isEmpty(this.props.assignmnetStatus)){
            this.props.actions.FetchAssignmentStatus();
        }
        
    }
    formInputHandler = (e) => {
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
    };
    assignmentStatusModalOpen=(e)=>{
        e.preventDefault();
        this.updatedData={};
        this.editedRowData={};
        this.setState({ isShowModal:true,
            isEditMode:false });
            
    }
    assignmentStatusModalClose=(e)=>{
        e.preventDefault();
        this.setState({ isShowModal:false });
        this.updatedData={};
        this.editedRowData={};
    }
    addAssignmentStatus=()=>{
        if(isEmpty(this.updatedData.name)){
            IntertekToaster(localConstant.admin.manageLifecycle.ASSIGNMENT_STATUSES, 'warningToast');
        return false;
        }
        this.updatedData["recordStatus"] = "N";
        this.updatedData["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
        this.props.actions.AddAssignmentStatus(this.updatedData);
        this.setState({ isShowModal:false });
        this.updatedData={};
    }
    updateAssignmentStatus=(e)=>{
        e.preventDefault();
        const combinedData = mergeobjects(this.editedRowData, this.updatedData);
        if(isEmpty(combinedData.name))
        {
            IntertekToaster(localConstant.admin.manageLifecycle.ASSIGNMENT_STATUSES, 'warningToast');
            return false;
        }
        if (this.editedRowData.recordStatus !== "N")
        this.updatedData["recordStatus"] = "M";
        this.props.actions.UpdateAssignmentStatus(this.updatedData,this.editedRowData);
        this.setState({ isShowModal: false });
    }
    editRowHandler = (data) => {
        this.setState({
            isEditMode: true,
            isShowModal: true
        });
        this.editedRowData = data;
    }
    deleteSelectedrows = () => {
        const selectedData = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedData);
        this.props.actions.DeleteAssignmentStatus(selectedData);
        this.props.actions.HideModal();
    }
    assignmentStatusDeleteHandler = () =>{ 
        const selectedRecords = this.child.getSelectedRows();
       const records=selectedRecords.filter(x=>
        x.name ==="Complete" || x.name ==="In Progress" );
       if(!isEmpty(records)){
        IntertekToaster(modalMessageConstant.ASSIGMENT_COMPLETE_STATUS, 'warningToast');
        return false;
       }
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.ASSIGMENT_STATUS,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSelectedrows,
                        className: "m-1 btn-small"
                    },
                    {
                        buttonName: localConstant.commonConstants.NO,
                        onClickHandler: this.confirmationRejectHandler,
                        className: "m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
        else{
            IntertekToaster(localConstant.techSpec.common.REQUIRED_DELETE, 'warningToast');
        }
    }
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }
    render() {
        const { assignmnetStatus }=this.props;
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        return (

            <Fragment>
                 <CustomModal modalData={modelData} />
                {this.state.isShowModal &&
                 <Modal
                   title={this.state.isEditMode?"Edit Assignment Status":"Add Assignment Status"}
                    buttons={this.state.isEditMode?this.editStatus:this.addStatus}
                    isShowModal={this.state.isShowModal}>
                        <AssignmentStatusModalPopup
                        editedRowData={ this.editedRowData}
                        formInputHandler={this.formInputHandler}
                        isEditMode={this.state.isEditMode}
                        />
                    </Modal>
                }
                <div className=" customCard">
                    <h6 className="label-bold">{localConstant.admin.manageLifecycle.ASSIGMNMENT_STATUS}</h6>
                    <CardPanel className="white lighten-4 black-text" colSize="s12">
                        <ReactGrid gridColData={HeaderData}
                            gridRowData={assignmnetStatus && assignmnetStatus.filter(x => x.recordStatus !== "D")}
                            onRef={ref => { this.child = ref; }} />
                         <div className="right-align mt-2 col s12 pr-0 pt-2">
                            <a className="waves-effect btn-small" onClick={this.assignmentStatusModalOpen} >{localConstant.commonConstants.ADD}</a>
                            <a className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" onClick={this.assignmentStatusDeleteHandler} >{localConstant.commonConstants.DELETE}</a>
                        </div>
                    </CardPanel>
                </div>               
            </Fragment>
        );
    }
}
export default Status;