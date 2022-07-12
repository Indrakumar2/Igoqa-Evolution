import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './visitRefHeader';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, formInputChangeHandler, isEmpty, bindAction, mergeobjects, isEmptyOrUndefine } from '../../../../utils/commonUtils';
import Modal from '../../../../common/baseComponents/modal';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import CustomModal from '../../../../common/baseComponents/customModal';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
import { visitTabDetails } from '../../../viewComponents/visit/visitDetails/visitTabsDetails';

const localConstant = getlocalizeData();

const VisitRefModal = (props) => {
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
                    optionsList={props.referenceType}
                    optionName='referenceType'
                    optionValue="referenceType"
                    onSelectChange={props.handlerChange}
                    defaultValue={props.editedVisitReference && props.editedVisitReference.referenceType}
                />
                <CustomInput
                    hasLabel={true}
                    labelClass="customLabel mandate"
                    name='referenceValue'
                    divClassName='col'
                    label={localConstant.visit.REFERENCE_VALUE}
                    type='text'
                    colSize='s12 m6'
                    inputClass="customInputs"
                    maxLength={fieldLengthConstants.visit.visitReference.REFERENCE_VALUE_MAXLENGTH}
                    onValueChange={props.handlerChange}
                    defaultValue={props.editedVisitReference.referenceValue}
                    autocomplete="off"
                />
            </div>
        </Fragment>
    );
};

class VisitReference extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isVisitRefShowModal: false,
            isVisitRefEdit: false,
        };
        this.updatedData = {};
        //Add Buttons
        this.VisitRefAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelVisitRefModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.addVisitDetails,
                btnClass: "btn-small ",
                showbtn: true
            }
        ];
        //Edit Buttons
        this.VisitRefEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelVisitRefModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.updateVisitRefDetails,
                btnClass: "btn-small",
                showbtn: true
            }
        ];
        
        const functionRefs = {};
        functionRefs["enableEditColumn"] = this.enableEditColumn;
        this.headerData = HeaderData(functionRefs);
    }

    enableEditColumn = (e) => {
        return (this.props.interactionMode || this.props.isTBAVisitStatus ? true : false);
    }

    //All Input Handle get Name and Value
    handlerChange = (e) => {
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
    }

    visitReferenceValidation = (data) => {

        if (isEmpty(data.referenceType)) {
            IntertekToaster(localConstant.assignments.PLEASE_FILL_REFERENCE_TYPE, 'warningToast');
            return false;
        }
        if (isEmpty(data.referenceValue)) {
            IntertekToaster(localConstant.assignments.PLEASE_FILL_REFERENCE_VALUE, 'warningToast');
            return false;
        }
        return true;
    }

    //Adding Stamp details to the grid
    addVisitDetails = (e) => {
        e.preventDefault();
        this.updatedData["visitReferenceTypeAddUniqueId"] = Math.floor(Math.random() * 99) - 100;
        this.updatedData["visitReferenceId"] = null;
        this.updatedData["recordStatus"] = 'N';
        let isDuplicateReference;
        if (!isEmpty(this.updatedData.referenceType)) {
            this.props.VisitReferences.forEach((iteratedValue) => {
                if (iteratedValue.referenceType.toUpperCase() === this.updatedData.referenceType.toUpperCase() && iteratedValue.recordStatus !== "D") {
                    isDuplicateReference = true;
                }
            });
            if (isDuplicateReference) {
                IntertekToaster(localConstant.assignments.REFERENCE_TYPE_EXISTS, 'warningToast duplicateScheduleName');
                return false;
            }
        }
        if (this.visitReferenceValidation(this.updatedData)) {
            this.props.actions.AddVisitReference(this.updatedData);
            this.hideVisitRefModal();
            this.updatedData = {};
        }
    }

    updateVisitRefDetails = (e) => {
        e.preventDefault();
        const combinedData = mergeobjects(this.editedRowData, this.updatedData);
        if (this.editedRowData.recordStatus !== "N") {
            this.updatedData["recordStatus"] = "M";
        }
        let isDuplicateReference;
        if (!isEmpty(this.updatedData.referenceType)) {
            if (this.editedRowData.referenceType !== this.updatedData.referenceType) {

                this.props.VisitReferences.forEach((iteratedValue) => {
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
        if (this.visitReferenceValidation(combinedData)) {
            this.props.actions.UpdateVisitReference(this.updatedData, this.editedRowData);
            this.hideVisitRefModal();
            this.updatedData = {};
        }
    }

    editRowHandler = (data) => {
        this.setState((state) => {
            return {
                isVisitRefShowModal: true,
                isVisitRefEdit: true
            };
        });
        this.editedRowData = data;
    }

    showVisitRefModal = (e) => {
        e.preventDefault();
        this.setState((state) => {
            return {
                isVisitRefShowModal: true,
                isVisitRefEdit: false
            };
        });
        this.editedRowData = {};
    }

    //Hiding the modal
    hideVisitRefModal = () => {
        this.setState((state) => {
            return {
                isVisitRefShowModal: false,
            };
        });
        this.updatedData = {};
        this.editedRowData = {};
    }

    //Cancel the updated data in model popup
    cancelVisitRefModal = (e) => {
        e.preventDefault();
        this.updatedData = {};
        this.setState((state) => {
            return {
                isVisitRefShowModal: false,
            };
        });
    }

    deleteSelectedVisitReference = () => {
        const selectedData = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedData);
        this.props.actions.DeleteVisitReference(selectedData);
        this.props.actions.HideModal();
    }

    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    deleteVisitReference = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.VISIT_REFERENCE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSelectedVisitReference,
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
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE, 'warningToast idOneRecordSelectReq');
        }
    }

    addVisitReferenceByDefault() {
        if (!isEmptyOrUndefine(this.props.visitReferenceTypes)) {            
            this.props.visitReferenceTypes.forEach(referenceData => {
                if (referenceData.isVisibleToVisit) {
                    this.updatedData = {};                    
                    this.updatedData["referenceType"] = referenceData.referenceType;
                    this.updatedData["referenceValue"] = "TBA";
                    this.updatedData["visitReferenceTypeAddUniqueId"] = Math.floor(Math.random() * 99) - 100;
                    this.updatedData["visitReferenceId"] = null;
                    this.updatedData["recordStatus"] = 'N';
                    this.props.actions.AddVisitReference(this.updatedData);
                }
            });
            this.updatedData = {};
        }
    }

    filterVisitReferenceTypes(visitReferenceTypes) {
        const filteredVisitReferenceTypes = [];
        if (!isEmptyOrUndefine(visitReferenceTypes)) {
            this.props.visitReferenceTypes.forEach(referenceData => {
                if (referenceData.isVisibleToVisit) {
                    filteredVisitReferenceTypes.push(referenceData);
                }
            });
        }
        return filteredVisitReferenceTypes;
    }

    componentDidMount() {
        //this.props.actions.FetchReferencetypes();
        if(this.isPageRefresh()) {
            if (this.props.currentPage === localConstant.visit.EDIT_VIEW_VISIT_MODE) {
                //this.props.actions.FetchVisitReference();
            } else {
                // if(isEmptyOrUndefine(this.props.VisitReferences)) {
                //     this.addVisitReferenceByDefault();
                // }
            }
        }
    };

    isPageRefresh() {
        let isRefresh = true;
        visitTabDetails.forEach(row => {
            if(row["tabBody"] === "VisitReference") {
                isRefresh = row["isRefresh"];
                row["isRefresh"] = false;
                row["isCurrentTab"] = true;
            } else {
                row["isCurrentTab"] = false;
            }
        });
        return isRefresh;
    }

    render() {
        const { VisitRefData, disableField, interactionMode, visitReferenceTypes, isTBAVisitStatus } = this.props;
        const filteredVisitReferenceTypes = this.filterVisitReferenceTypes(visitReferenceTypes);

        bindAction(this.headerData, "EditColumn", this.editRowHandler);
        const visitReferenceDataGrid = this.props.VisitReferences && this.props.VisitReferences.filter((iteratedValue, i) => {
            return iteratedValue.recordStatus !== "D";
        });
        return (
            <Fragment>
                {/* <CustomModal /> */}
                {this.state.isVisitRefShowModal &&
                    <Modal
                        modalClass="visitModal"
                        title={this.state.isVisitRefEdit ? localConstant.visit.EDIT_VISIT_REFERENCE :
                            localConstant.visit.ADD_VISIT_REFERENCE}
                        buttons={this.state.isVisitRefEdit ? this.VisitRefEditButtons : this.VisitRefAddButtons}
                        isShowModal={this.state.isVisitRefShowModal}>
                        <VisitRefModal
                            handlerChange={this.handlerChange}
                            editedVisitReference={this.editedRowData}
                            referenceType={filteredVisitReferenceTypes}
                        />
                    </Modal>}
                <div className="customCard">
                    <h6 className="bold">{localConstant.visit.VISIT_REFERENCES}</h6>
                    <ReactGrid gridRowData={visitReferenceDataGrid} gridColData={this.headerData}
                        onRef={ref => { this.child = ref; }} />
                    { this.props.pageMode!==localConstant.commonConstants.VIEW ?<div className="right-align mt-2">
                        <a className="waves-effect btn-small" onClick={this.showVisitRefModal} disabled={disableField || interactionMode || isTBAVisitStatus} >{localConstant.commonConstants.ADD}</a>
                        <a className="btn-small dangerBtn ml-2" onClick={this.deleteVisitReference}
                            disabled={(VisitRefData && VisitRefData.filter(x => x.recordStatus !== "D").length <= 0) || disableField || interactionMode || isTBAVisitStatus ? true : false} >
                            {localConstant.commonConstants.DELETE}</a>
                    </div> :null }
                </div>
            </Fragment>
        );
    }
}

export default VisitReference;
