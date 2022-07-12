import React, { Component, Fragment } from 'react';
import { HeaderData } from './headerData.js';
import ReactGrid from '../../../../../common/baseComponents/reactAgGrid';
import CardPanel from '../../../../../common/baseComponents/cardPanel';
import Modal from '../../../../../common/baseComponents/modal';
import { getlocalizeData, bindAction } from '../../../../../utils/commonUtils';
import CustomModal from '../../../../../common/baseComponents/customModal';
import CustomInput from '../../../../../common/baseComponents/inputControlls';
import IntertekToaster from '../../../../../common/baseComponents/intertekToaster';
import { modalMessageConstant, modalTitleConstant } from '../../../../../constants/modalConstants';
import { SaveBarWithCustomButtons } from '../../../../../components/applicationComponents/saveBar';
import PropTypes from 'prop-types';
const localConstant = getlocalizeData();
const ReferenceTypeDiv = (props) => (
    <div className="row mb-0 dividerRow">
        <div className="col s12 pr-0">
            <CustomInput
                hasLabel={true}
                name="languageVersion"
                divClassName='bold col s3 pl-0 pr-3'
                colSize='s4'
                label={localConstant.admin.referenceTypes.VIEW_ATLERNATE_LANGUAGE_VERSIONS}
                type='select'
                inputClass="customInputs"
                optionsList={props.languagesList}
                optionName="name"
                optionValue="name"
                defaultValue={props.selectedLanguageVersion}
                onSelectChange={props.inputHandleChange} />
        </div>
    </div>
);
const ReferenceTypeModalPopup = (props) => {
    return (
        <Fragment>
            <CustomInput hasLabel={true}
                label={localConstant.admin.referenceTypes.REFERENCE_TYPE}
                divClassName='col s3 pl-0 pr-3'
                colSize='s9'
                type='text'
                labelClass="mandate"
                required={true}
                className="customInputs browser-default "
                inputClass="customInputs"
                onValueChange={props.handlerChange}
                defaultValue={props.eidtedRefTypes.name}
                name="name"
                id="name"
            />
        </Fragment>
    );
};
class AssignmentReferenceType extends Component {
    constructor(props) {
        super(props);
        this.updatedData = {};
        this.state = {
            isShowReferenceModal: false,
            isReferenceModelAddView: false
        };
        this.referenceAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.hideModal,
                btnID: "cancelReferenceTypeSubmit",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type: "button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.referenceTypeAddHandler,
                btnID: "referenceTypeAddSubmit",
                btnClass: "waves-effect waves-teal btn-small mr-2",
                showbtn: true
            }
        ];
        this.referenceUpdateButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.hideModal,
                btnID: "cancelReferenceTypeSubmit",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type: "button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.referenceTypeUpdateHandler,
                btnID: "referenceTypeUpdateSubmit",
                btnClass: "waves-effect waves-teal btn-small mr-2",
                showbtn: true
            }
        ];
        this.headerSaveCancelButton = [
            {
              name: localConstant.commonConstants.SAVE,
              clickHandler: () => this.referenceTypeSaveHandler(),
              className: "btn-small mr-0 ml-2",         
              isbtnDisable: this.props.isbtnDisable
            }, 
            {
              name:localConstant.commonConstants.CANCEL,
              clickHandler: () => this.referenceTypeCancelHandler(),
              className: "btn-small mr-0 ml-2 waves-effect modal-trigger",          
              isbtnDisable: this.props.isbtnDisable
            }
         ];     
    }
    componentDidMount = () => {
        this.props.actions.FetchReferenceTypeDetails();
        this.props.actions.FetchLanguges();
    }
    referenceTypeAddHandler = (e) => {
        e.preventDefault();
        if (this.updatedData.name === undefined || this.updatedData.name === "") {
            IntertekToaster(localConstant.warningMessages.REFERENCE_TYPE_NAME_MANDATORY, 'warningToast assignmentReferenceTypeReq');
        }
        else {
            this.updatedData["recordStatus"] = "N";
            this.updatedData["name"] = this.updatedData.name;
            this.updatedData["masterDataTypeId"] = 30;
            this.props.actions.AddReferenceTypeDetails(this.updatedData);
            this.updatedData = {};
            this.hideModal(e);
        }
    }
    referenceTypeUpdateHandler = (e) => {
        e.preventDefault();
        this.setState({
            isShowReferenceModal: false,
            isReferenceModelAddView: false
        });
        if (this.updatedData.name === "") {
            IntertekToaster(localConstant.companyDetails.Documents.SELECT_FILE_TYPE, 'warningToast noFileTypeWarning');
        }
        else if (this.updatedData.name) {
            if (this.editedRowData.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            this.updatedData["name"] = this.updatedData.name;
            this.props.actions.UpdateReferenceTypeDetails(this.updatedData, this.editedRowData);
            this.updatedData = {};
        }
    }
    referenceTypeSaveHandler = () => {
        // const valid = this.mandatoryFieldsValidationCheck();
        // if(valid === true){
            this.props.actions.SaveReferenceTypeDetails();
        // }
    }
    referenceTypeCancelHandler = () => {    
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.CANCEL_CHANGES,
                modalClassName: "warningToast",
                type: "confirm",
                buttons: [ {
                    buttonName: "Yes",
                    onClickHandler: this.cancelReferenceTypeChanges,
                    className: "modal-close m-1 btn-small"
                },
                {
                    buttonName: "No",
                    onClickHandler: this.confirmationRejectHandler,
                    className: "modal-close m-1 btn-small"
                } ]
            };           
            this.props.actions.DisplayModal(confirmationObject);
    };    
    cancelReferenceTypeChanges = () =>{ 
        this.props.actions.CancelReferenceTypeChanges();   
        this.props.actions.HideModal();
    }
    inputChangeHandler = (e) => {
        const fieldValue = e.target[e.target.type === "checkbox" ? "checked" : "value"];
        const fieldName = e.target.name;
        const result = { value: fieldValue, name: fieldName };
        return result;
    }
    /* Form Input data Change Handler*/
    formInputChangeHandler = (e) => {
        const result = this.inputChangeHandler(e);
        this.updatedData[result.name] = result.value;
    }
    addReferenceTypeHandler = () => {
        this.setState({
            isShowReferenceModal: true,
            isReferenceModelAddView: true
        });
        this.editedRowData = {};
    }
    hideModal = (e) => {
        e.preventDefault();
        this.setState({
            isShowReferenceModal: false,
            isReferenceModelAddView: false
        });
        this.updatedData = {};
        this.editedRowData = {};
    }
    editRowHandler = (data) => {
        this.setState({
            isShowReferenceModal: true,
            isReferenceModelAddView: false,
            isbtnDisable: false
        });
        this.editedRowData = data;
    }
    deleteReferenceTypeHandler = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length === 0) {
            IntertekToaster(localConstant.warningMessages.SELECT_AT_LEAST_ONE_REF_TYPE_FOR_DELETE, 'warningToast oneorMoreRefTypeReq');
        }
        else {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.REFERENCE_TYPE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: "Yes",
                        onClickHandler: this.deleteReferenceTYpe,
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
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }
    deleteReferenceTYpe = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            this.child.removeSelectedRows(selectedRecords);
            this.props.actions.DeleteReferenceTypeDetails(selectedRecords);
            this.props.actions.HideModal();
        }
    }
    render() {
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        const AssignmentReferenceTypeHeaderData = HeaderData;
        const { assignmentRefTypeData,launguageList,selectedLanguageVersion } = this.props;
        const filteredRefData = Object.assign([], assignmentRefTypeData.filter(x => x.recordStatus !== "D"));
        bindAction(AssignmentReferenceTypeHeaderData, "EditAssignmentReferenceTypes", this.editRowHandler);
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                <SaveBarWithCustomButtons
                    currentMenu={localConstant.admin.ADMIN}
                    currentSubMenu={ this.props.currentPage}
                    buttons={this.headerSaveCancelButton}
                    />
                <div className="customCard singleCardTopPosition">
                    <ReferenceTypeDiv
                        languagesList={launguageList}
                        inputHandleChange={(e) => this.formInputChangeHandler(e)}
                        selectedLanguageVersion={selectedLanguageVersion}
                    />
                    <h6 className="bold col s3 pl-0 pr-3">{localConstant.admin.REFERENCE_TYPE}</h6>
                    <CardPanel className="white lighten-4 black-text" colSize="s12">
                        <ReactGrid gridRowData={filteredRefData}
                            gridColData={AssignmentReferenceTypeHeaderData}
                            handlerChange={(e) => this.formInputChangeHandler(e)}
                            onRef={ref => { this.child = ref; }} />
                        <div className="right-align mt-2 col s12 pr-0 pt-2">
                            <a href="javascript:void(0);" onClick={this.addReferenceTypeHandler} className="btn-small waves-effect modal-trigger">
                                {localConstant.commonConstants.ADD}
                            </a>
                            <a className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" onClick={this.deleteReferenceTypeHandler}>
                                {localConstant.commonConstants.DELETE}
                            </a>
                        </div>
                    </CardPanel>
                    {this.state.isShowReferenceModal &&
                        <Modal id="assignmentReferenceTypeModalPopup"
                            title={'AssignmentReferenceTypes'}
                            modalClass="assignmentReferenceTypeModal"
                            buttons={this.state.isReferenceModelAddView ? this.referenceAddButtons : this.referenceUpdateButtons}
                            isShowModal={this.state.isShowReferenceModal}>
                            <ReferenceTypeModalPopup
                                eidtedRefTypes={this.editedRowData}
                                handlerChange={(e) => this.formInputChangeHandler(e)}
                            />
                        </Modal>}
                </div>
            </Fragment>
        );
    }
}

export default AssignmentReferenceType;
AssignmentReferenceType.propTypes = {
    assignmentRefTypeData: PropTypes.array,
    launguageList:PropTypes.array,
    selectedLanguageVersion:PropTypes.string
};

AssignmentReferenceType.defaultProps = {
    assignmentRefTypeData: [],
    launguageList:[],
    selectedLanguageVersion:""
};