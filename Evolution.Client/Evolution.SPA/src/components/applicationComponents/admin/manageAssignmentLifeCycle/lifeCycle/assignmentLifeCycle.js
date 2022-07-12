import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../../../common/baseComponents/reactAgGrid';
import Modal from '../../../../../common/baseComponents/modal';
import { getlocalizeData, bindAction ,formInputChangeHandler, isEmpty,mergeobjects } from '../../../../../utils/commonUtils';
import { modalMessageConstant, modalTitleConstant } from '../../../../../constants/modalConstants';
import { HeaderData } from './headerData';
import IntertekToaster from '../../../../../common/baseComponents/intertekToaster';
import CustomInput from '../../../../../common/baseComponents/inputControlls';
import CustomModal from '../../../../../common/baseComponents/customModal';
const localConstant = getlocalizeData();
const LifecycleModalPopup = (props) => {
    return (
        <Fragment>
            <CustomInput
                hasLabel={true}
                divClassName='col pr-0'
                label={localConstant.admin.manageLifecycle.LIFECYCLE_DESCRIPTION}
                type='text'
                colSize='s6 '
                inputClass="customInputs"
                defaultValue={props.editedRowData && props.editedRowData.name}
                maxLength={15}
                name="name"
                labelClass="mandate"
                onValueChange={props.formInputHandler}
                autocomplete="off"

            />
            <CustomInput
                hasLabel={true}
                divClassName='col pr-0'
                label={localConstant.admin.manageLifecycle.LONG_ESCRIPTION}
                type='text'
                colSize='s6 '
                inputClass="customInputs"
                defaultValue={ props.editedRowData && props.editedRowData.description}
                maxLength={15}
                name="description"
                onValueChange={props.formInputHandler}
                autocomplete="off"

            />
             <CustomInput
                type='switch'
                switchLabel={localConstant.admin.manageLifecycle.HIDE_FOR_NEWFACILITY}
                colSize="col s12"
                isSwitchLabel={true}
                className="lever"
                switchName="isAlchiddenForNewFacility"
                onChangeToggle={props.formInputHandler}
                checkedStatus={props.editedRowData && props.editedRowData.isAlchiddenForNewFacility} />
       </Fragment>
    );

};
class LifeCycle extends Component {
    constructor(props){
        super(props);
        this.state={
         isShowModal:false,
         isEditMode:false
        };
        this.updatedData={};
        this.addLifecycle = [
            {

                name: localConstant.commonConstants.CANCEL,
                action: this.lifeCycleModalClose,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type: "button"
            },
            {
                
                name: localConstant.commonConstants.SUBMIT,
                action: this.addAssignmentLifecycle,
                btnClass: "btn-small ",
                showbtn: true,
                type: "button"
            },

        ];
        this.editLifecycle = [
            {

                name: localConstant.commonConstants.CANCEL,
                action: this.lifeCycleModalClose,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type: "button"
            },
            {
                
                name: localConstant.commonConstants.SUBMIT,
                action: this.updateAssignmentLifecycle,
                btnClass: "btn-small ",
                showbtn: true,
                type: "button"
            },

        ];
        bindAction(HeaderData, "EditColumn", this.editRowHandler);
    }
    componentDidMount(){
        if(isEmpty(this.props.assignmnetlifecycle)){
        this.props.actions.FetchAssignmentLifeCycle();
        }
    }
    formInputHandler = (e) => {
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
    };
    lifeCycleModalOpen=(e)=>{
        e.preventDefault();
        this.updatedData={};
        this.editedRowData={};
        this.setState({ isShowModal:true,
            isEditMode:false });
            
    }
    lifeCycleModalClose=(e)=>{
        e.preventDefault();
        this.setState({ isShowModal:false });
        this.updatedData={};
        this.editedRowData={};
    }
    addAssignmentLifecycle=(e)=>{
        e.preventDefault();
        if (isEmpty(this.updatedData.name)) {
            IntertekToaster(localConstant.admin.manageLifecycle.LIFECYCLE_VALIDATION, 'warningToast');
            return false;
        }
        this.updatedData["recordStatus"] = "N";
        this.updatedData["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
        this.props.actions.AddAssignmentLifeCycle(this.updatedData);
        this.setState({ isShowModal:false });
        this.updatedData={};
    }
    updateAssignmentLifecycle=(e)=>{
        e.preventDefault();
        const combinedData = mergeobjects(this.editedRowData, this.updatedData);
        if(isEmpty(combinedData.name))
        {
            IntertekToaster(localConstant.admin.manageLifecycle.LIFECYCLE_VALIDATION, 'warningToast');
            return false;
        }
        if (this.editedRowData.recordStatus !== "N")
        this.updatedData["recordStatus"] = "M";
        this.props.actions.UpdateAssignmenttLifeCycle(this.updatedData,this.editedRowData);
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
        this.props.actions.DeleteAssignmentLifeCycle(selectedData);
        this.props.actions.HideModal();
    }
    lifeCycleDeleteHandler = () =>{ 
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.ASSIGMENT_LIFECYCLE,
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
        else IntertekToaster(localConstant.techSpec.common.REQUIRED_DELETE, 'warningToast');
    }
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }
    render() {
        const { assignmnetlifecycle }=this.props;
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        return (

            <Fragment>
                 <CustomModal modalData={modelData} />
                {this.state.isShowModal &&
                 <Modal
                   title={this.state.isEditMode?"Edit Assignment Lifecycle":"Add Assignment Lifecycle"}
                    buttons={this.state.isEditMode?this.editLifecycle:this.addLifecycle}
                    isShowModal={this.state.isShowModal}>
                        <LifecycleModalPopup
                        editedRowData={ this.editedRowData}
                        formInputHandler={this.formInputHandler}
                        />
                    </Modal>
                }
                <div className=" customCard">
                    <h6 className="label-bold">{localConstant.admin.manageLifecycle.ASSIGNMENT_LIFECYCLE}</h6>
                    <ReactGrid gridColData={HeaderData} gridRowData={assignmnetlifecycle && assignmnetlifecycle.filter(x => x.recordStatus !== "D")} onRef={ref => { this.child = ref; }}/>
                    <div className="right-align mt-2">
                    <a className="waves-effect btn-small" onClick={this.lifeCycleModalOpen} >{localConstant.commonConstants.ADD}</a>
                    <a className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" onClick={this.lifeCycleDeleteHandler} >{localConstant.commonConstants.DELETE}</a>
                    </div>
                </div>
            </Fragment>
        );
    }
}
export default LifeCycle;