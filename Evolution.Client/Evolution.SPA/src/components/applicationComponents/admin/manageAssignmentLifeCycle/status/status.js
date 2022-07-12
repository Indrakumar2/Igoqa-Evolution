import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../../../common/baseComponents/reactAgGrid';
import Modal from '../../../../../common/baseComponents/modal';
import { getlocalizeData, bindAction ,formInputChangeHandler, isEmptyReturnDefault } from '../../../../../utils/commonUtils';
import { modalMessageConstant, modalTitleConstant } from '../../../../../constants/modalConstants';
import { HeaderData } from './headerData';
const localConstant = getlocalizeData();
class LifeCycle extends Component {
    constructor(props){
        super(props);
        this.state={
         isShowModal:false
        };
        this.updatedData={};
        this.cancelButtons = [
            {

                name: localConstant.commonConstants.CANCEL,
                action: this.lifeCycleModalClose,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type: "button"
            },
            {
                
                name: localConstant.commonConstants.SUBMIT,
                action: this.OnSubmitClick,
                btnClass: "btn-small ",
                showbtn: true,
                type: "button"
            },

        ];
        bindAction(HeaderData, "EditColumn", this.editRowHandler);
    }
    formInputHandler = (e) => {
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
    };
    lifeCycleModalOpen=()=>{
        this.setState({ isShowModal:true });
    }
    lifeCycleModalClose=()=>{
        this.setState({ isShowModal:false });
    }
    OnSubmitClick=()=>{
        this.setState({ isShowModal:false });
    }
    editRowHandler = (data) => {
        this.setState({ isShowModal: true });
        this.editedRowData = data;
    }
    deleteSelectedrows = () => {
        const selectedData = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedData);
        // this.props.actions.DeleteDetailPayRate(selectedData);
        this.props.actions.HideModal();
    }
    lifeCycleDeleteHandler=()=>{
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: modalMessageConstant.PAY_RATE_SCHEDULE,
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
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }
    render() {
        return (

            <Fragment>
                 <Modal
                    buttons={this.cancelButtons}
                    isShowModal={this.state.isShowModal}>
                    </Modal>
                <div className=" customCard">
                    <ReactGrid gridColData={HeaderData} gridRowData={isEmptyReturnDefault("")}/>
                    <div className="right-align mt-2">
                    <a className="waves-effect btn-small" onClick={this.lifeCycleModalOpen} >{localConstant.commonConstants.ADD}</a>
                    <a className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" onClick={""} >{localConstant.commonConstants.DELETE}</a>
                    </div>
                </div>
            </Fragment>
        );
    }
}
export default LifeCycle;