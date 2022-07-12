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
const localConstant = getlocalizeData();
const ClassificationModal = (props) =>(
            <div className="row">
                <CustomInput
                    hasLabel={true}
                    type='select'
                    divClassName='col'
                    label={localConstant.gridHeader.CATEGORY}
                    colSize='s12 m4'
                    optionsList={props.categoryOptions}
                    optionName='label'
                    optionValue="label"
                    labelClass="mandate"
                    name="category"
                    className="browser-default customInputs"
                    onSelectChange={props.handlerChange}
                    interactionMode={props.interactionMode}
                    defaultValue={props.isEditMode && props.eidtedClassifications.category}
                />
                <CustomInput
                    hasLabel={true}
                    type='select'
                    divClassName='col'
                    label={localConstant.gridHeader.SUB_CATEGORY}
                    colSize='s12 m4'
                    optionsList={props.subCategoryOptions}
                    optionName='label'
                    optionValue="label"
                    labelClass="mandate"
                    name="subCategory"
                    className="browser-default customInputs"
                    onSelectChange={props.handlerChange}
                    interactionMode={props.interactionMode}
                    defaultValue={props.isEditMode && props.eidtedClassifications.subCategory}
                />
                <CustomInput
                    hasLabel={true}
                    type='select'
                    divClassName='col'
                    label={localConstant.gridHeader.SERVICE}
                    colSize='s12 m4'
                    optionsList={props.servicesOptions}
                    optionName='label'
                    optionValue="label"
                    labelClass="mandate"
                    name="services"
                    className="browser-default customInputs"
                    onSelectChange={props.handlerChange}
                    interactionMode={props.interactionMode}
                    defaultValue={props.isEditMode && props.eidtedClassifications.services}
                />
            </div>
);
class TechnicalDiscipilines extends Component{
    constructor(props){
        super(props);
        this.updatedData = {};
        this.state = {
            isOpen: false,
            isTechnicalDesciplineModalOpen:false,
            isTechnicalDesciplineEditMode:false,            
            selectedRowData:{},
        };
    }
    addTechnicalDesciplines = () => {
        this.setState({
            isTechnicalDesciplineModalOpen:true,
            isTechnicalDesciplineEditMode:false
        });
        this.editedRowData={};
    }
    cancelClientNotification = () => {
        this.setState({
            isTechnicalDesciplineModalOpen:false
        });
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
                isTechnicalDesciplineModalOpen: true,
                isTechnicalDesciplineEditMode:true
            };
        });
        this.editedRowData=data;
    }
    submitNewClassification = (e) =>{
        e.preventDefault();
        this.updatedData["classificationId"] = Math.floor(Math.random() * 99) -100;
        this.props.actions.AddNewClassification(this.updatedData);
        this.setState({
            isTechnicalDesciplineModalOpen:false
        });
        this.updatedData = {};
    }
    deleteTechnicalDesciplines = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.CLASSIFICATION_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSelectedClassification,
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
    deleteSelectedClassification = () => {
        const selectedData = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedData);
        this.props.actions.DeleteClassification(selectedData);
        this.props.actions.HideModal();
    }
    render(){
        bindAction(HeaderData,"EditColumn",this.editRowHandler);
        const classificationButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelClientNotification,
                btnID: "cancelClientNotification",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.state.isTechnicalDesciplineEditMode?this.editClassification:this.submitNewClassification,
                btnID: "editClientNotification",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        const assignmentClassificationGrid = this.props.assignmentClassificationGrid && this.props.assignmentClassificationGrid.filter((iteratedValue, i) => {
            return iteratedValue.recordStatus !== "D";
        });
        const dummyOptions = [
            {
                label:"option1"
            },
            {
                label:"option2"
            },
            {
                label:"option3"
            },
        ];
        return(
            <Fragment>
                <CustomModal modalData={modelData} />
                <div className="customCard">
                    {this.state.isTechnicalDesciplineModalOpen &&
                        <Modal 
                            title={localConstant.assignments.CLASSIFICATIONS}
                            modalId="classificaionModal"
                            formId="classificaionModal" 
                            modalClass="popup-position" 
                            buttons={classificationButtons} 
                            isShowModal={this.state.isTechnicalDesciplineModalOpen} 
                            disabled={this.props.interactionMode}>
                            <ClassificationModal 
                                handlerChange = { this.handlerChange }
                                eidtedClassifications = { this.editedRowData }
                                isEditMode = { this.state.isTechnicalDesciplineEditMode }
                                categoryOptions = {dummyOptions}
                                subCategoryOptions = { dummyOptions }
                                servicesOptions = { dummyOptions }
                            />
                        </Modal>}
                        
                    <CardPanel className="white lighten-4 black-text" title={localConstant.assignments.BUSINESS_UNIT} colSize="s12">
                       <div className="row">
                            <div className="col s12 m6">
                                <span class="bold">{localConstant.assignments.BUSINESS_UNIT} :</span>
                            </div>
                            <div className="col s12 m6">
                                <a class="bold">View Taxonomy/Classification Structure and Keyword Definitions</a>
                            </div>
                       </div>
                    </CardPanel>
                    <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.assignments.CLASSIFICATIONS} colSize="s12">
                        <div className="customCard">
                            <ReactGrid gridRowData={assignmentClassificationGrid}
                                gridColData={HeaderData}
                                onRef={ref => { this.child = ref; }} />
                        </div>
                        <div className="right-align mt-2 mr-2">
                            <a onClick={this.addTechnicalDesciplines} disabled={false} className="btn-small waves-effect modal-trigger">
                                {localConstant.commonConstants.ADD}
                            </a>
                            <a className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" onClick={this.deleteTechnicalDesciplines}>
                                {localConstant.commonConstants.DELETE}
                            </a>
                        </div>
                    </CardPanel>
                </div>
            </Fragment>
        );
    }
}

export default TechnicalDiscipilines;