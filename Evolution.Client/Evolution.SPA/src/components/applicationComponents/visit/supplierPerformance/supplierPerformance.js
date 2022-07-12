import React, { Component,Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './supplierPerfomanceHeader';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, formInputChangeHandler,isEmpty, bindAction, isEmptyOrUndefine,mergeobjects } from '../../../../utils/commonUtils';
import Modal from '../../../../common/baseComponents/modal';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import CustomModal from '../../../../common/baseComponents/customModal';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
import { visitTabDetails } from '../../../viewComponents/visit/visitDetails/visitTabsDetails';

const localConstant = getlocalizeData();

const SupplierModal = (props) => {
    return (
        <Fragment>
            <div className="col s12 p-0" >
                <CustomInput
                    hasLabel={true}
                    divClassName='col pl-0 pr-0'
                    label={localConstant.visit.SUPPLIER_PERFORMANCE_TYPE}
                    type='select'
                    name='supplierPerformance'
                    colSize='s6'
                    className="browser-default"
                    labelClass="mandate"
                    optionsList={props.supplierPerformanceTypeList}
                    optionName='name'
                    optionValue="name"
                    onSelectChange={props.handlerChange}
                    defaultValue={props.editedSupplier && props.editedSupplier.supplierPerformance}
                />
                <CustomInput 
                    hasLabel={true}                    
                    labelClass="customLabel mandate"
                    name='ncrReference'
                    divClassName='col' 
                    label={localConstant.visit.NCR_REFERENCE}
                    type='text'
                    colSize='s12 m6'
                    maxLength={fieldLengthConstants.visit.supplierPerformance.NCR_REFERENCE_MAXLENGTH}
                    inputClass="customInputs"
                    required={true}
                    onValueChange={props.handlerChange}
                    defaultValue={props.editedSupplier.ncrReference} 
                    autocomplete="off" 
                />
                <CustomInput
                    hasLabel={true}
                    //divClassName='col pl-0 pr-0 mt-0'
                    label={localConstant.visit.NCR_CLOSE_OUTDATE}
                    type='date'
                    name='ncrCloseOutDate'
                    // labelClass="mandate"
                    selectedDate={dateUtil.defaultMoment(props.ncrCloseOutDate && props.ncrCloseOutDate)}
                    onDateChange={props.ncrCloseOutDateChange}
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                />
            </div>          
        </Fragment>
    );
};

class SupplierPerformance extends Component {
    constructor(props) {
        super(props);
        this.state = {          
            ncrCloseOutDate: '',
            isSupplierShowModal: false,
            isSupplierEdit: false
        };
        this.updatedData = {};
    
        //Add Buttons
        this.SupplierAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelSupplierModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.addSupplierPerformance,
                btnClass: "btn-small ",
                showbtn: true
            }
        ];
        //Edit Buttons
        this.SupplierEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelSupplierModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.editSupplierPerformance,
                btnClass: "btn-small",
                showbtn: true
            }
        ];

        const functionRefs = {};
        functionRefs["enableEditColumn"] = this.enableEditColumn;
        this.headerData = HeaderData(functionRefs);
    }
    
    enableEditColumn = (e) => {
        return (this.props.interactionMode || this.props.isTBAVisitStatus || (this.props.isInterCompanyAssignment && this.props.isOperatingCompany && [ 'O','A' ].includes(this.props.visitInfo.visitStatus))? true : false);
    }

    supplierPerformanceValidation = (data) => {
        if(isEmpty(data.supplierPerformance)){
            IntertekToaster(localConstant.visit.PERFORMANCE_TYPE_VALIDIATION, 'warningToast');
            return false;
        }
        if(isEmpty(data.ncrReference)){
            IntertekToaster(localConstant.visit.NCR_REFERENCE_VALIDATION, 'warningToast');
            return false;
        }
        return true;
    }
    addSupplierPerformance = (e) => {        
        e.preventDefault();        
        this.updatedData["supplierPerformanceAddUniqueId"] = Math.floor(Math.random() * 99) -100;
        this.updatedData["visitSupplierPerformanceTypeId"] = null;
        this.updatedData["recordStatus"] = 'N';     
    
        if(this.supplierPerformanceValidation(this.updatedData)){
        this.props.actions.AddSupplierPerformance(this.updatedData);
        this.hideSupplierModal();
        this.updatedData = {};
        }
    }

    editSupplierPerformance = (e) => {
        e.preventDefault(); 
        const combinedData = mergeobjects(this.editedRowData, this.updatedData);
        if (this.editedRowData.recordStatus !== "N") {
            this.updatedData["recordStatus"] = "M";
        }      
        
        this.updatedData["modifiedBy"] = this.props.loginUser;
        if(this.supplierPerformanceValidation(combinedData)){
        this.props.actions.UpdateSupplierPerformance(this.updatedData,this.editedRowData);
        this.hideSupplierModal();
        this.updatedData = {};
        }
    }
  
    //All Input Handle get Name and Value
    handlerChange = (e) => {
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
    }

    componentDidMount() {
        if(this.isPageRefresh()) {
            this.props.actions.FetchSupplierPerformanceType();
            // if (this.props.currentPage === localConstant.visit.EDIT_VIEW_VISIT_MODE) {
            //     this.props.actions.FetchSupplierPerformance();        
            // }
        }
    };

    isPageRefresh() {
        let isRefresh = true;
        visitTabDetails.forEach(row => {
            if(row["tabBody"] === "SupplierPerformance") {
                isRefresh = row["isRefresh"];
                row["isRefresh"] = false;
                row["isCurrentTab"] = true;
            } else {
                row["isCurrentTab"] = false;
            }
        });
        return isRefresh;
    }

    ncrCloseOutDateChange = (date) => {
        this.setState({
            ncrCloseOutDate: date
        }, () => {
            this.updatedData["ncrCloseOutDate"] = this.state.ncrCloseOutDate !== null ? this.state.ncrCloseOutDate.format(localConstant.commonConstants.SAVE_DATE_FORMAT) : '';
        });
    }

    cancelSupplierModal=(e)=>{
        e.preventDefault();
        this.updatedData = {};       
        this.setState((state) => {
            return {
                isSupplierShowModal: false,
            };
        });
    }

    editRowHandler=(data)=>{
        this.setState((state)=>{
            return {
                isSupplierShowModal: true,
                isSupplierEdit: true
            };
        });
        this.editedRowData=data;
        this.setGridDate();
    }

    setGridDate = () => {        
        if (this.editedRowData && !isEmptyOrUndefine(this.editedRowData.ncrCloseOutDate)) {  
            this.setState({
                ncrCloseOutDate: moment(this.editedRowData.ncrCloseOutDate),
            });
        } else {
            this.setState({
                ncrCloseOutDate: ""
            });
        }
    }

    showSupplierModal = (e) => {
        e.preventDefault();
        this.setState({ ncrCloseOutDate: '' });
        this.setState((state) => {
            return {
                isSupplierShowModal: true,
                isSupplierEdit: false
            };
        });        
        this.editedRowData = {};
    }

    hideSupplierModal = () => {
        this.setState((state) => {
            return {
                isSupplierShowModal: false
            };
        });
        this.updatedData = {};
        this.editedRowData = {};
    }

    deleteSelectedSupplierPerformance = () => {
        const selectedData = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedData);
        this.props.actions.DeleteSupplierPerformance(selectedData);
        this.props.actions.HideModal();
    }

    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    deleteSupplierPerformance = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.VISIT_SUPPLIER_PERFORMANCE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSelectedSupplierPerformance,
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
   
    render() {
        const { interactionMode, isOperatingCompany, isInterCompanyAssignment, visitInfo } = this.props;
        //const { SupplierPerformanceData,disableField,interactionMode } = this.props;
        bindAction(this.headerData,"EditColumn",this.editRowHandler);
        const supplierPerformanceDataGrid = this.props.VisitSupplierPerformances && this.props.VisitSupplierPerformances.filter((iteratedValue, i) => {
            return iteratedValue.recordStatus !== "D";
        });
        return (
            <Fragment>
                {/* <CustomModal /> */}
                {this.state.isSupplierShowModal &&
                    <Modal
                        modalClass="visitModal"
                        title={this.state.isVisitRefEdit ? localConstant.visit.ADD_VISIT_SUPPLIER:
                            localConstant.visit.EDIT_VISIT_SUPPLIER}
                        buttons={this.state.isSupplierEdit && this.props.pageMode!==localConstant.commonConstants.VIEW ? this.SupplierEditButtons : this.SupplierAddButtons }
                        isShowModal={this.state.isSupplierShowModal}>
                        <SupplierModal
                            handlerChange = { this.handlerChange }
                            editedSupplier = { this.editedRowData }
                            ncrCloseOutDate = { this.state.ncrCloseOutDate }
                            //referenceType = { this.props.visitReferenceTypes}
                            ncrCloseOutDateChange = {this.ncrCloseOutDateChange}
                            supplierPerformanceTypeList = { this.props.supplierPerformanceTypeList }
                        />
                    </Modal>
                }        
                <div className="customCard">
                     <h6 className="bold">{localConstant.visit.SUPPLIER_PERFORMANCE}</h6>
                    <ReactGrid gridRowData={supplierPerformanceDataGrid} gridColData={this.headerData}
                        onRef = {ref => { this.child = ref; }} />
                    {this.props.pageMode !== localConstant.commonConstants.VIEW ?<div className="right-align mt-2">
                            <a disabled={this.props.isTBAVisitStatus || interactionMode || (isInterCompanyAssignment && isOperatingCompany && [ 'O','A' ].includes(visitInfo.visitStatus))} className="waves-effect btn-small" onClick={this.showSupplierModal} >{localConstant.commonConstants.ADD}</a>
                            <a disabled={this.props.isTBAVisitStatus || interactionMode || (isInterCompanyAssignment && isOperatingCompany && [ 'O','A' ].includes(visitInfo.visitStatus))} className="btn-small dangerBtn ml-2" onClick={this.deleteSupplierPerformance}                                 >
                                {localConstant.commonConstants.DELETE}</a>
                </div>:null}               
                </div>
            </Fragment>
        );
    }
}

export default SupplierPerformance;
