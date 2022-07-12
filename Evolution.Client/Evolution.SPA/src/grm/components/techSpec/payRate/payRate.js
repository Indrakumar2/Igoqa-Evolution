import React, { Fragment } from 'react';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData.js';
import { getlocalizeData, bindAction, isEmpty, mergeobjects, formInputChangeHandler,numberFormat,isEmptyReturnDefault, isEmptyOrUndefine } from '../../../../utils/commonUtils';
import Modal from '../../../../common/baseComponents/modal';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import CustomModal from '../../../../common/baseComponents/customModal';
import moment from 'moment';
import dateUtil from '../../../../utils/dateUtil';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { applicationConstants } from '../../../../constants/appConstants';
import { sortCurrency } from '../../../../utils/currencyUtil';
import { required,requiredNumeric } from '../../../../utils/validator';
import arrayUtil from '../../../../utils/arrayUtil';
import { levelSpecificActivities } from '../../../../constants/securityConstant';
import { isEditable,isViewable } from '../../../../common/selector';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
const localConstant = getlocalizeData();
const currentDate = moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT);

const PayRateComponent = (props) => {
    return (
        <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.PayRate.PAY_RATE} colSize="s12" >
            <div className="row mb-0" >
                <div className="col s12 p-0">
                    <CustomInput
                        hasLabel={true}
                        divClassName='col pr-0'
                        label={localConstant.techSpec.PayRate.TAX_REF_NO}
                        labelClass={props.technicalSpecialistInfo.profileStatus === 'Active' ? 'mandate' :''} //IGO QC D886 (Ref ALM Confirmation Doc)
                        type='text'
                        colSize='s6 '
                        inputClass="customInputs"
                        value={props.technicalSpecialistInfo.taxReference ? props.technicalSpecialistInfo.taxReference : ""}
                        dataValType='valueText'
                        maxLength={fieldLengthConstants.Resource.payRate.TAX_REF_NO_MAXLENGTH}
                        name="taxReference"
                        onValueChange={props.formInputHandler}
                        autocomplete="off"
                        readOnly={props.isPayRateDisabled || props.interactionMode|| props.disableField}
                    />
                    <CustomInput
                        hasLabel={true}
                        label={localConstant.techSpec.PayRate.PAYROL_REFERNCE}
                        labelClass={props.technicalSpecialistInfo.profileStatus === 'Active' ? 'mandate' :''} //IGO QC D886 (Ref ALM Confirmation Doc)
                        type='text'
                        colSize='s6'
                        inputClass="customInputs"
                        maxLength={fieldLengthConstants.Resource.payRate.PAYROL_REFERNCE_MAXLENGTH}
                        name="payrollReference"
                        onValueChange={props.formInputHandler}
                        value={props.technicalSpecialistInfo.payrollReference ? props.technicalSpecialistInfo.payrollReference : ""}
                        dataValType='valueText'
                        autocomplete="off"
                        readOnly={props.isPayRateDisabled || props.interactionMode || props.disableField} />
                </div>
                <div className="col s12 p-0">
                    <CustomInput
                        hasLabel={true}
                        divClassName='col pr-0'
                        label={localConstant.techSpec.PayRate.PAYROLL_NAME}
                        labelClass={props.technicalSpecialistInfo.profileStatus === 'Active' ? 'mandate' :''} //IGO QC D886 (Ref ALM Confirmation Doc)
                        type='select'
                        colSize='s6'
                        optionsList={props.payrollName}
                        optionName="payrollType"
                        optionValue="payrollType"
                        maxLength={200}
                        name="companyPayrollName"
                        className="browser-default customInputs"
                        defaultValue={props.technicalSpecialistInfo.companyPayrollName}
                        onSelectChange={props.formInputHandler}
                        disabled={props.isPayRateDisabled || props.interactionMode || props.disableField} />
                    <CustomInput
                        hasLabel={true}
                        label={localConstant.techSpec.PayRate.PAYROLL_NOTES}
                        type='text'
                        colSize='s6'
                        inputClass="customInputs"
                        maxLength={200}
                        onValueChange={props.formInputHandler}
                        value={props.technicalSpecialistInfo.payrollNotes ? props.technicalSpecialistInfo.payrollNotes : ""}
                        dataValType='valueText'
                        name="payrollNotes"
                        autocomplete="off"
                        readOnly={props.isPayRateDisabled || props.interactionMode || props.disableField} />
                </div>
            </div>
        </CardPanel>
    );
};
const PayRateScheduleModalPopUp = (props) => {
    return (
        <div className="row mb-0" id="PayRateModalPopUp">
            <CustomInput
                hasLabel={true}
                divClassName='col pr-0'
                label={localConstant.techSpec.PayRate.PAY_RATE_SCEDULEE}
                type='text'
                colSize='s6 '
                required={true}
                labelClass="mandate"
                defaultValue={props.rateScheduleDefaultData.payScheduleName}
                inputClass="customInputs"
                name='payScheduleName'
                maxLength={fieldLengthConstants.Resource.payRate.PAY_RATE_SCEDULEE_MAXLENGTH}
                onValueChange={props.formInputHandler}
                disabled={props.disableField} />
            <CustomInput
                hasLabel={true}
                divClassName='col loadedDivision'
                label={localConstant.techSpec.PayRate.CURRENCY}
                type='select'
                colSize="s3"
                labelClass="mandate"
                optionsList={props.currencyData}
                defaultValue={props.rateScheduleDefaultData.payCurrency}
                optionName="code"
                optionValue="code"
                name="payCurrency"
                className="browser-default customInputs"
                onSelectChange={props.formInputHandler}
                disabled={props.disableField} />
            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.techSpec.PayRate.SCHEDULE_NOTES}
                type='text'
                colSize='s12 '
                inputClass="customInputs"
                name="payScheduleNote"
                id="PayScheduleNote"
                maxLength={fieldLengthConstants.Resource.payRate.SCHEDULE_NOTES_MAXLENGTH}
                defaultValue={props.rateScheduleDefaultData.payScheduleNote}
                onValueChange={props.formInputHandler} 
                disabled={props.disableField} />
            <CustomInput
                type='switch'
                switchLabel={localConstant.techSpec.PayRate.HIDE_RATE}
                colSize="s4"
                isSwitchLabel={true}
                className="lever"
                switchName="isActive"
                onChangeToggle={props.formInputHandler}
                checkedStatus={props.rateScheduleDefaultData.isActive}
                disabled={props.disableField} />
        </div>
    );
};
const PayRateDetailsPopup = (props) => {
    return (
        <Fragment >
            <div className="row mb-0" >

                <CustomInput
                    hasLabel={true}
                    divClassName='col loadedDivision pr-0'
                    label={localConstant.techSpec.PayRate.TYPE}
                    type='select'
                    colSize="s3"
                    name="expenseType"
                    optionsList={props.expenseType}
                    optionName="name"
                    optionValue="name"
                    className="browser-default customInputs"
                    onSelectChange={props.formInputHandler}
                    defaultValue={props.DefaultDetailData && props.DefaultDetailData.expenseType}
                    labelClass="mandate"
                    disabled={props.disableField} />
                <CustomInput
                    hasLabel={true}
                    divClassName='col'
                    label={localConstant.techSpec.PayRate.DESCRIPTION_TEXT}
                    type='text'
                    colSize='s6 '
                    maxLength={fieldLengthConstants.Resource.payRate.DESCRIPTION_TEXT_MAXLENGTH}
                    name="description"
                    inputClass="customInputs"
                    onValueChange={props.formInputHandler}
                    defaultValue={props.DefaultDetailData && props.DefaultDetailData.description}
                    disabled={props.disableField} />
                <CustomInput
                    hasLabel={true}
                    divClassName='col pl-0'
                    label={localConstant.techSpec.PayRate.RATE}
                    type="text"
                    dataType='number'
                    colSize='s3 '
                    name="rate"
                    prefixLimit={9}
                    suffixLimit={2}
                    inputClass="customInputs"
                    checkedStatus={true}
                    //onValueChange={props.formInputHandler}
                    onValueInput={props.decimalWithLimtFormat}
                    onValueBlur={props.checkNumber}
                    defaultValue={props.DefaultDetailData && props.DefaultDetailData.rate} 
                    labelClass="mandate"
                    disabled={props.disableField} />
            </div>
            <div className="row mb-0">
                <CustomInput
                    hasLabel={true}
                    isNonEditDateField={false}
                    divClassName='col n pr-0'
                    type='date'
                    label={localConstant.techSpec.PayRate.EFFECTIVE_FROM}
                    colSize='s4'
                    onDatePickBlur={props.dateBlurHandler}
                    name="effectiveFrom"
                    placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    required={true}
                    onDateChange={props.fetchEffectiveFrom}
                    selectedDate={props.EffectiveFrom}
                    defaultValue={props.DefaultDetailData && props.DefaultDetailData.effectiveFrom}
                    labelClass="mandate" 
                    disabled={props.disableField} />
                <CustomInput
                    hasLabel={true}
                    isNonEditDateField={false}
                    divClassName='col '
                    type='date'
                    label={localConstant.techSpec.PayRate.EFFECTIVE_TO}
                    colSize='s4'
                    onDatePickBlur={props.dateBlurHandler}
                    name="effectiveTo"
                    placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    onDateChange={props.fetchEffectiveTo}
                    selectedDate={props.EffectiveTo}
                    required={true}
                    defaultValue={props.DefaultDetailData && props.DefaultDetailData.effectiveTo} 
                    labelClass="mandate"
                    disabled={props.disableField} />
            </div>
            <div className="row mb-0">
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.techSpec.PayRate.DEFAULT_PAYRATE}
                    colSize="s4 mt-3"
                    isSwitchLabel={true}
                    className="lever"
                    switchName="isDefaultPayRate"
                    checkedStatus={props.DefaultDetailData && props.DefaultDetailData.isDefaultPayRate}
                    onChangeToggle={props.formInputHandler} 
                    disabled={props.disableField}
                    />
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.techSpec.PayRate.HIDE_RATES}
                    colSize="s3 mt-3"
                    isSwitchLabel={true}
                    className="lever"
                    switchName="isActive"
                    onChangeToggle={props.formInputHandler}
                    checkedStatus={props.DefaultDetailData && props.DefaultDetailData.isActive}
                    disabled={props.disableField}
                   />
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.techSpec.PayRate.HIDE_RATE}
                    colSize="s5 mt-3"
                    isSwitchLabel={true}
                    className="lever"
                    switchName="isHideOnTsExtranet"
                    checkedStatus={props.DefaultDetailData && props.DefaultDetailData.isHideOnTsExtranet}
                    onChangeToggle={props.formInputHandler}
                    disabled={props.disableField}
                   />
            </div>
        </Fragment>
    );
};

const DeletePaySchedulePopup = (props) => {
    return (
        <Fragment>
            <ReactGrid gridRowData={props.schedules} gridColData={props.headerData} onRef={props.gridRef} />
        </Fragment>
    );
};

class PayRate extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            SelectedPayScheduleName: "",
            SelectedPayRateSchedule: {},
            effectiveFromDate: '',
            effectiveToDate: '',
            inValidDateFormat: false,
            payRateShowModal: false,
            payRateEditMode: false,
            initialScheduleBinded: false,
            detailPayRateScheduleModal: false,
            detailpayEditMode: false,
            orderSequenceId:1,
            isDeletePayScheduleModalOpen:false,
            selectedPayRateDetails:[],
            selectedPayScheduleDetails:[],            
            isLoadOnCancel: false,
            isLoadOnSave: false,
            isDatePicker: false
        };        
        this.currencyData = [];
        this.updatedData = {};
        this.payScheduleEditedRow={}; 
        this.isPayRateDisabled = this.disableBasedOnActionType(this.props.oldProfileActionType);
        this.payRateScheduleAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelCreateModalPopup,
                btnID: "cancelCreateRateSchedule",
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.createPayRateSchedule,
                btnID: "createRateSchedule",
                btnClass: "btn-small ",
                showbtn: true
            }
        ];
        this.payRateScheduleEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelCreateModalPopup,
                btnID: "cancelCreateRateSchedule",
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.updatePayRateSchedules,
                btnID: "editRateSchedule",
                btnClass: "btn-small ",
                showbtn:  !this.fieldDisableHandler()
            }
        ];
        this.DetailPayRateAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelPayRateDetailModal,
                btnID: "cancelCreateRateSchedule",
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.detailPayRate,
                btnID: "createRateSchedule",
                btnClass: "btn-small ",
                showbtn: true
            }
        ];
        this.DetailPayRateEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelPayRateDetailModal,
                btnID: "cancelCreateRateSchedule",
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.updatePayRateDetail,
                btnID: "createRateSchedule",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn:  !this.fieldDisableHandler()
            }
        ];
        this.DeletePayScheduleButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelDeletePaySchedule,
                btnID: "cancelDeletePaySchedule",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.payRateScheduleDeleteClickHandler,
                btnID: "deleteSelectedSchedules",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        this.headerData = HeaderData();
        bindAction(this.headerData.PayRateHeader, "EditColumn", this.editRowHandler);
        bindAction(this.headerData.payScheduleHeader, "EditPaySchedule", this.payRateScheduleEditHandler); 
        this.props.callBackFuncs.onCancel =()=>{            
            this.clearLineItemsOnCancel();
        };     
        this.props.callBackFuncs.reloadLineItemsOnSave = () => {
            this.reloadLineItemsOnSave();
        };  
        this.props.callBackFuncs.reloadLineItemsOnSaveValidation = () => {
            this.setState({ isLoadOnSave: false });   
            this.bindPayScheduleGrid();
        };
    }

    reloadLineItemsOnSave = () => {
        if(this.state.selectedPayScheduleDetails) {       
            this.setState({ isLoadOnSave: true });   
            const schedule = this.props.payRateSchedule.filter(x => x.recordStatus !== "D");                           
            const paySchedule = arrayUtil.sort(schedule,"payScheduleName",'asc'); //D774 #34 issue            
            paySchedule.forEach((itratedValue) => {            
                itratedValue.isSelected= false;
            });  
            if(paySchedule && paySchedule.length>0)
                paySchedule[0].isSelected = true;
            this.payScheduleGrid.gridApi.setRowData(paySchedule); 
        }
    }

    clearLineItemsOnCancel = () => {        
        if(this.props.currentPage === localConstant.techSpec.common.Create_Profile) {
            this.setState({
                selectedPayScheduleDetails: [],
                selectedPayRateDetails: []
            });
        } else {
            this.setState({ isLoadOnCancel : true });
            if(this.props.payRateSchedule && this.props.payRateSchedule.length > 0) {
                const payScheduleData = this.props.payRateSchedule;
                payScheduleData[0].isSelected = true;
                this.setState({ 
                    selectedPayScheduleDetails: payScheduleData,
                    SelectedPayRateSchedule: payScheduleData[0]
                });
            } else {
                this.setState({
                    selectedPayScheduleDetails: [],
                    selectedPayRateDetails: [],
                    SelectedPayRateSchedule: {}
                });
            }
            this.payScheduleGrid.gridApi.setRowData(this.state.selectedPayScheduleDetails);
        }          
    }

    disableBasedOnActionType = (actionType) => {
        const userTypes = localStorage.getItem(applicationConstants.Authentication.USER_TYPE);
        const username = localStorage.getItem(applicationConstants.Authentication.USER_NAME);
        switch (actionType) {
            case localConstant.techSpec.common.SEND_TO_TS:
                if (this.props.isTSUserTypeCheck)
                    return false;
                else if(this.props.technicalSpecialistInfo.profileStatus === 'Active') {
                    return false;
                }
                return true;
            case localConstant.techSpec.common.SEND_TO_RC_RM:
                if (this.props.isRMUserTypeCheck || this.props.isRCUserTypeCheck || this.props.isMIUserTypeCheck || this.props.isOMUserTypeCheck)
                    return false;
                return true;
            case localConstant.techSpec.common.SEND_TO_TM:
                if (this.props.technicalSpecialistInfo.profileStatus === 'Active' && this.props.technicalSpecialistInfo.assignedToUser === username) {
                    return false;
                }
                return true;
            default:
                return false;
        }
    }
    fieldDisableHandler = () => {
        const userTypes = localStorage.getItem(applicationConstants.Authentication.USER_TYPE);
        if (this.props.currentPage === "Edit/View Resource") {
            if (this.props.isRMUserTypeCheck || this.props.isRCUserTypeCheck || this.props.isMIUserTypeCheck || this.props.isOMUserTypeCheck) {//IGO Scenario Issue (31-03-2020)
                {/** TM Edit/View Access changes done, as per the Admin User Guide document 20-11-19 (ITK requirement)*/}
                if(this.props.technicalSpecialistInfo.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM){
                    return true;
                }
                return !(isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel4 }));
            }
            else if (this.props.techManager || this.props.isTMUserTypeCheck) {
                return true;
            }
            else if(this.props.isTSUserTypeCheck){ // Hot Fixes Changes done as per ITK requirement
                return true;
            }
        }
        return false;
    };

    componentDidMount = () => {
        const companyCodeObj = this.props.payrollName.find(o => o.companyCode === this.props.technicalSpecialistInfo.companyCode);
        if (this.props.technicalSpecialistInfo && this.props.technicalSpecialistInfo.companyCode !== (companyCodeObj === undefined ? '' : companyCodeObj.companyCode)) {
            this.props.actions.FetchPayRollNameByCompany(this.props.technicalSpecialistInfo.companyCode);
        }
        this.currencyData = sortCurrency(this.props.currencyData);
        //this.bindPayRateDetailonCancel();
        this.bindPayScheduleGrid();
    }

    receivingProps = () => {
        if (this.editedRowData) {
            if (this.editedRowData.effectiveTo !== null && this.editedRowData.effectiveTo !== "") {
                this.setState({
                    effectiveToDate: moment(this.editedRowData.effectiveTo),
                    effectiveFromDate: moment(this.editedRowData.effectiveFrom)
                });
            }
            else {
                this.setState({
                    effectiveToDate: "",
                    effectiveFromDate: moment(this.editedRowData.effectiveFrom),
                });
            }
        } else {
            this.setState({
                effectiveFromDate: '',
                effectiveToDate: ""
            });
        }
    }
    editRowHandler = (data) => {
        this.setState((state) => {
            return {
                detailPayRateScheduleModal: !state.detailPayRateScheduleModal,
                detailpayEditMode: true,
            };
        });
        this.editedRowData = data;
        this.receivingProps();
    }
    fetchEffectiveFrom = (date) => {
        this.setState({
            effectiveFromDate: date
        }, () => {
            this.updatedData.effectiveFrom = this.state.effectiveFromDate;
        });
    }
    fetchEffectiveTo = (date) => {
        this.setState({
            effectiveToDate: date
        }, () => {
            this.updatedData.effectiveTo = this.state.effectiveToDate;
        });
    }
    handleDateBlur = (e) => {
        if (e && e.target !== undefined) {
            this.setState({ inValidDateFormat: false });
            if (e.target.value !== "" && e.target.value !== null) {
                if (e && e.target !== undefined) {
                    const isValid = dateUtil.isUIValidDate(e.target.value);
                    if (!isValid) {
                        IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast');
                        this.setState({ inValidDateFormat: true });
                    }
                    else this.setState({ inValidDateFormat: false });
                }
            }
        }
    }
    //checking  Payschedule name duplicate
    isPayRateScheduleDuplicate = (data) => {
        let isRateScheduleDuplicate = false;
        this.props.payRateSchedule && this.props.payRateSchedule.forEach(iteratedValue => {
            if (this.state.payRateEditMode) {
                if (iteratedValue.recordStatus !== 'D' && iteratedValue.payScheduleName.toUpperCase() === data.payScheduleName.toUpperCase() && iteratedValue.scheduleId !== data.scheduleId) { //IGO_QC_895
                    isRateScheduleDuplicate = true;
                }
            }
            else {
                if (iteratedValue.recordStatus !== 'D' && iteratedValue.payScheduleName.toUpperCase() === data.payScheduleName.toUpperCase()) { //IGO_QC_895
                    isRateScheduleDuplicate = true;
                }
            }
        });
        return isRateScheduleDuplicate;
    }
    payRateScheduleValidation = (data) => {
        if (data.payScheduleName === undefined || data.payScheduleName === "") {
            IntertekToaster(localConstant.contract.common.REQUIRED_TEXT + ' Pay Rate Schedule', 'warningToast rspayScheduleNameReq');
            return false;
        }
        if (data.payCurrency === undefined || data.payCurrency === "") {
            IntertekToaster(localConstant.contract.common.REQUIRED_TEXT + ' Currency', 'warningToast rspayScheduleNameReq');
            return false;
        }
        if (data.payScheduleName && this.isPayRateScheduleDuplicate(data)) {
            IntertekToaster(localConstant.techSpec.PayRate.PAY_RATE_DUPLICATE, 'warningToast rsPayRateScheduleDuplicate');
            return false;
        }
        return true;
    }
    //Showing PayRate Modal
    createPayRateHandler = (e) => {
        e.preventDefault();
        this.setState({
            payRateShowModal: true,
            payRateEditMode: false
        });
        this.updatedData = {};
    }
    //Hiding the Modal
    cancelCreateModalPopup = (e) => {
        e.preventDefault();
        this.props.actions.ClearDetailPayRate();
        this.setState({ payRateShowModal: false });
        this.updatedData = {};
    }
    payInputHandler = (e) => {
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
        this.props.actions.UpdatePayRate(this.updatedData);
        this.updatedData = {};
    }
    formInputHandler = (e) => {
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
    };
    createPayRateSchedule = (e) => {
        e.preventDefault();
        if (this.updatedData.isActive === undefined) {
            this.updatedData.isActive = false;
        }
        if (this.payRateScheduleValidation(this.updatedData)) {
            if (this.updatedData && !isEmpty(this.updatedData)) {
                this.updatedData["epin"] = this.props.technicalSpecialistInfo.epin !== undefined ? this.props.technicalSpecialistInfo.epin : 0;
                this.updatedData["recordStatus"] = "N";
                this.updatedData["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                this.updatedData["baseScheduleName"] = null;
                this.props.actions.AddPayRateSchedule(this.updatedData);
                if(this.state.orderSequenceId >= 1 ){
                    this.updatedData["sequenceId"] = this.state.orderSequenceId + 1;
                    this.setState({
                        orderSequenceId:this.updatedData["sequenceId"]
                    });
                }
                this.setState({
                    SelectedPayRateSchedule: this.updatedData,
                    SelectedPayScheduleName: this.updatedData.payScheduleName,
                    payRateShowModal: false
                });
                this.props.actions.ClearDetailPayRate();
                this.updatedData = {};
            }
        }
    }
    payRateScheduleOnChangeHandler = (e) => {
        this.setState({ SelectedPayScheduleName: e.target.value });
        if (e.target.value === 'select' || e.target.value === 'Select' || e.target.value === "") {
            this.setState({
                SelectedPayScheduleName: '',
                SelectedPayRateSchedule: {},
            });
        }
        else {
            const currentRateSchedule = this.props.payRateSchedule && this.props.payRateSchedule.filter(iteratedValue => {
                if (iteratedValue.payScheduleName === e.target.value) {
                    return iteratedValue;
                }
            });
            this.setState({ SelectedPayRateSchedule: currentRateSchedule[0] });
        }
    }

    payScheduleOnSelectHandler = (e) => {        
        if(e.node.isSelected()){
            const currentRateSchedule = this.props.payRateSchedule && this.props.payRateSchedule.filter(iteratedValue => {
                if (iteratedValue.payScheduleName === e.node.data.payScheduleName && iteratedValue.id === e.node.data.id) { //IGO_QC_895
                    return iteratedValue;
                }
            });
            this.setState({ SelectedPayRateSchedule: currentRateSchedule[0] });
            this.setState({ SelectedPayScheduleName: e.node.data.payScheduleName });
            this.bindPayRateDetailGrid();
        } else {
            // this.setState({ SelectedPayRateSchedule: {} });
            // this.setState({ SelectedPayScheduleName: "" });
            const selectedData = this.payScheduleGrid.getSelectedRows();            
            if(isEmptyOrUndefine(selectedData)) {
                e.node.setSelected(true);
            }
        }
    }

    updatePayRateSchedules = (e) => {
        e.preventDefault();
        let scheduleNameUpdated = false;
        this.updatedData["oldScheduleName"] = this.payScheduleEditedRow.payScheduleName;
        if (this.updatedData) {
            if (this.updatedData.payScheduleName) {
                scheduleNameUpdated = true;
            }
            let editedRow = mergeobjects(this.payScheduleEditedRow, this.updatedData);
            if (this.payRateScheduleValidation(editedRow)) {
                if (this.props.rateScheduleEditData.recordStatus !== "N")
                    this.updatedData["recordStatus"] = "M";
                editedRow = mergeobjects(editedRow, this.updatedData);

                this.props.actions.UpdatePayRateSchedule(editedRow);
                this.setState({
                    SelectedPayRateSchedule: editedRow,
                    SelectedPayScheduleName: editedRow.payScheduleName,
                    payRateShowModal: false
                });
                this.props.actions.ClearDetailPayRate();
                this.updatedData = {};
                this.payScheduleEditedRow={};
            }
        }
    }

    cancelDeletePaySchedule = (e) => {
        e.preventDefault();
        this.setState({
            isDeletePayScheduleModalOpen: false
        });
    }
    
    deletePayScheduleHandler =()=>{
        this.setState({
            isDeletePayScheduleModalOpen:true
        });
    }

    payRateScheduleEditHandler = (data) => {
        const currentRateSchedule = this.props.payRateSchedule && this.props.payRateSchedule.filter(iteratedValue => {
            if (iteratedValue.payScheduleName === this.state.SelectedPayScheduleName) {
                return iteratedValue;
            }
        });
        this.props.actions.EditPayRateSchedule(data);
        this.payScheduleEditedRow=data;
        this.setState({
            payRateEditMode: true,
            payRateShowModal: true
        });
        this.updatedData = {};
    }
    deleteSelected = () => {
        this.setState({ SelectedPayScheduleName: "" });
        const selectedSchedules=this.deletePayScheduleChild.getSelectedRows();
        this.props.actions.DeletePayRateSchedule(selectedSchedules);
        this.props.actions.HideModal();
        this.setState({ SelectedPayRateSchedule: {} });
        this.setState({
            SelectedPayRateSchedule: {
                isActive: false
            }
        });
        const newState = this.state.selectedPayScheduleDetails;
        selectedSchedules.forEach(item => {
            newState.forEach((iteratedValue, index) =>{
                if (iteratedValue.payScheduleName === item.payScheduleName && iteratedValue.id === item.id) { //IGO_QC_895
                    newState.splice(index, 1);
                }
            });
        });        
        if(newState && newState.length > 0) {
            newState[0].isSelected = true;
            this.setState({ selectedPayScheduleDetails: newState });                         
        } else {            
            this.setState({ selectedPayScheduleDetails: [] });    
            this.setState({ selectedChargeRateDetails: [] });            
        } 
        this.setState((state)=>({                
            selectedPayScheduleDetails: state.selectedPayScheduleDetails.filter(item =>{
                return !selectedSchedules.some(ts=>{
                    return ts["id"] === item["id"];
                });
            }),
        }),()=>{
            this.payScheduleGrid.gridApi.setRowData(this.state.selectedPayScheduleDetails);
        });             
        this.setState({ isDeletePayScheduleModalOpen:false });
        this.bindPayScheduleGrid(); //IGO_QC_895
    }
    payRateScheduleDeleteClickHandler = (e) => {
        e.preventDefault();
        let isDetailpayExist = false;
        const selectedSchedules=this.deletePayScheduleChild.getSelectedRows();
        this.props.DetailPayRate && this.props.DetailPayRate.forEach(iteratedValue => {
            selectedSchedules.forEach(item =>{
                if (iteratedValue.recordStatus === 'D' && iteratedValue.payScheduleName === item.payScheduleName && iteratedValue.payScheduleId === item.id) {//IGO_QC_895
                    isDetailpayExist = false;
                }
                if (iteratedValue.recordStatus !== 'D' && iteratedValue.payScheduleName === item.payScheduleName && iteratedValue.payScheduleId === item.id) {//IGO_QC_895
                    isDetailpayExist = true;
                }
            });
        });
        if (isDetailpayExist) {
            IntertekToaster(localConstant.techSpec.PayRate.SELECTED_PAYRATESCHEDULE_ASSOCIATED_WARNING, 'warningToast rsPayRateScheduleAssociated');
            return false;
        }
        if (selectedSchedules.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.PAY_RATE_SCHEDULE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSelected,
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
        } else {
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE, 'warningToast');
        }        
    }
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }
    //PayRateDetail
    payRateDetailModal = (e) => {
        e.preventDefault();
        this.setState({
            detailPayRateScheduleModal: true,
            detailpayEditMode: false,
            effectiveFromDate: moment(),
            effectiveToDate: ''
        });
        this.updatedData = {};
        this.editedRowData = {};
    }
    //Multiple PayRate Detail
    multiplePayRateDetailModal = (e) => {
        e.preventDefault();
        this.setState({
            multipleDetailPayRateScheduleModal: true,
            multipleDetailpayEditMode: false
        });
    }
    cancelmultipleDetailPayRate = (e) => {
        e.preventDefault();
        this.updatedData = {};
        this.editedRowData = {};
        this.setState({ multipleDetailPayRateScheduleModal: false });
    }
    cancelPayRateDetailModal = (e) => {
        e.preventDefault();
        this.setState({ detailPayRateScheduleModal: false });
        this.updatedData = {};
        this.editedRowData = {};
    }
    dateRangeValidator = (from, to) => {
        let isInValidDate = false;
        if (to !== "" && to !== null) {
            if (from > to) {
                isInValidDate = true;
                IntertekToaster(localConstant.techSpecValidationMessage.PAY_RATE_DATE_RANGE_VALIDATION, 'warningToast');
            }
        }
        return isInValidDate;
    }
    payRateDetailValidation = (data) => {
        if (isEmpty(data) || required(data.expenseType)) {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Type', 'warningToast');
            return false;
        }
        if(isEmpty(data.rate)){
            IntertekToaster(localConstant.techSpecValidationMessage.PAY_RATE_MANDATORY_VALIDATION, 'warningToast');
            return false;
        }
        if(required(data.effectiveFrom)){
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Effective From', 'warningToast');
            return false;
        }
        if(required(data.effectiveTo)){
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Effective To', 'warningToast');
            return false;
        }
        return true;
    }
    detailPayRate = (e) => {
        e.preventDefault();
        if (this.updatedData && !isEmpty(this.updatedData)) {
            let fromDate = "";
            if (this.state.effectiveFromDate !== "" && this.state.effectiveFromDate !== null) {
                fromDate = this.state.effectiveFromDate.format(localConstant.techSpec.common.DATE_FORMAT);
            }
            let toDate = "";
            if (this.state.effectiveToDate !== "" && this.state.effectiveToDate !== null) {
                toDate = this.state.effectiveToDate.format(localConstant.techSpec.common.DATE_FORMAT);
            }
            this.updatedData["effectiveFrom"] = fromDate;
            this.updatedData["effectiveTo"] = toDate;
            if (this.payRateDetailValidation(this.updatedData)) {
                if (this.state.inValidDateFormat) {
                    IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast rsValidDateWarn');
                    return false;
                }
                if (!this.dateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {

                    this.updatedData["recordStatus"] = "N";
                    this.updatedData["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                    this.updatedData["payScheduleName"] = this.state.SelectedPayScheduleName;
                    this.updatedData["payScheduleCurrency"] = this.state.SelectedPayRateSchedule.payCurrency;
                    this.updatedData["epin"] = this.props.technicalSpecialistInfo.epin !== undefined ? this.props.technicalSpecialistInfo.epin : 0;
                    if(this.state.orderSequenceId >= 1 ){
                        this.updatedData["sequenceId"] = this.state.orderSequenceId + 1;
                        this.setState({
                            orderSequenceId:this.updatedData["sequenceId"]
                        });
                    }                    
                    this.props.actions.AddDetailPayRate(this.updatedData);                    
                    this.state.selectedPayRateDetails.unshift(this.updatedData);
                    this.child.gridApi.setRowData(this.state.selectedPayRateDetails);
                    this.setState({ detailPayRateScheduleModal: false });
                    this.cancelPayRateDetailModal(e);
                    this.updatedData = {};                    
                }
            }
        }
        else this.payRateDetailValidation(this.updatedData);
    }
    updatePayRateDetail = (e) => {
        e.preventDefault();
        const combinedData = mergeobjects(this.editedRowData, this.updatedData);
        if (this.payRateDetailValidation(combinedData)) {
            let fromDate = "";
            if (this.state.effectiveFromDate !== "" && this.state.effectiveFromDate !== null) {
                fromDate = this.state.effectiveFromDate.format(localConstant.techSpec.common.DATE_FORMAT);
            }
            let toDate = "";
            if (this.state.effectiveToDate !== "" && this.state.effectiveToDate !== null) {
                toDate = this.state.effectiveToDate.format(localConstant.techSpec.common.DATE_FORMAT);
            }
            if (this.state.inValidDateFormat) {
                IntertekToaster(localConstant.techSpec.common.VALID_DATE_WARNING, 'warningToast rsValidDatereqq');
                return false;
            }
            if (!this.dateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
                if (this.editedRowData.recordStatus !== "N")
                    this.updatedData["recordStatus"] = "M";
                this.updatedData["effectiveFrom"] = fromDate;
                this.updatedData["effectiveTo"] = toDate;
                if (this.updatedData && !isEmpty(this.updatedData)) {
                    this.props.actions.UpdateDetailPayRate(this.updatedData, this.editedRowData);
                    this.setState({ detailPayRateScheduleModal: false });
                    this.updatedData = {};                    
                }
            }
        }
    }
    submitmultipleDetailPayRate= (e) => {
        e.preventDefault();
        const selectedRecords = isEmptyReturnDefault(this.multipleDetailPayRateGrid.getSelectedRows());
        const schedule  = this.props.payRateSchedule.filter(x => x.id === this.state.SelectedPayRateSchedule.id); //D384(foreignKey issue)
        if (selectedRecords && selectedRecords.length > 0) {
            selectedRecords.forEach( async (rowData) => {                
                this.updatedData = {};
                this.updatedData["recordStatus"] = "N";
                this.updatedData["isActive"] = true;
                this.updatedData["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                this.updatedData["payScheduleName"] = schedule[0].payScheduleName; //D384(foreignKey issue)
                this.updatedData["payScheduleCurrency"] = schedule[0].payCurrency;//D384(foreignKey issue)
                this.updatedData["epin"] = this.props.technicalSpecialistInfo.epin !== undefined ? this.props.technicalSpecialistInfo.epin : 0;
                if(this.state.orderSequenceId >= 1 ){
                    this.updatedData["sequenceId"] = this.state.orderSequenceId + 1;
                    this.setState({
                        orderSequenceId:this.updatedData["sequenceId"]
                    });
                }
                if(this.payScheduleGrid !== undefined && this.payScheduleGrid.getSelectedRows() !== undefined) {
                    const selectedData = this.payScheduleGrid.getSelectedRows();  
                    this.updatedData["payScheduleId"] = selectedData[0].id;
                }
                this.updatedData["rate"] = "";                
                this.updatedData["effectiveFrom"] = currentDate;
                this.updatedData["expenseType"] = rowData.name;
                this.props.actions.AddDetailPayRate(this.updatedData);
            });
        } else {
            IntertekToaster("Please select the record", 'warningToast GridValidation');
            return false;
        }
        this.updatedData = {};
        this.setState({
            multipleDetailPayRateScheduleModal: false
        });
        this.child.gridApi.setFocusedCell(0, "description");
    }
    detailPayRatedeleteSelected = () => {
        const selectedData = this.child.getSelectedRows();
        const delPayRateItem = selectedData.filter(x=> x.recordStatus != 'N');
        //this.child.removeSelectedRows(selectedData);
        if(delPayRateItem.length > 0){
            this.props.actions.HideModal();
            IntertekToaster(localConstant.techSpec.common.PAYRATE_LINEITEM_DELETE, 'warningToast ');
            return false;
        }
        if(!isEmptyOrUndefine(this.state.selectedPayRateDetails)) {
            this.setState((state)=>({                
                selectedPayRateDetails: state.selectedPayRateDetails.filter(item =>{
                    return !selectedData.some(ts=>{                        
                        return ts["id"] === item["id"];
                    });
                }),
            }),()=>{
                this.child.gridApi.setRowData(this.state.selectedPayRateDetails);
            });
        }

        this.props.actions.DeleteDetailPayRate(selectedData);
        this.props.actions.HideModal();
    }
    detailPayRateDeleteClickHandler = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.PAY_RATE_DETAIL,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: "Yes",
                        onClickHandler: this.detailPayRatedeleteSelected,
                        className: "m-1 btn-small"
                    },
                    {
                        buttonName: "No",
                        onClickHandler: this.confirmationRejectHandler,
                        className: "m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
        else
            IntertekToaster(localConstant.techSpec.common.REQUIRED_DELETE, 'warningToast ');
    }
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    onScheduleCellFocused = (e) => {
        if(e.rowIndex !== null) {
            this.bindPayScheduleGrid();
            const isReadonly = (this.isPayRateDisabled || this.props.interactionMode 
                || this.props.pageMode === localConstant.commonConstants.VIEW || this.fieldDisableHandler()  ? true : false);
                if(!isReadonly) {
                    if(document.getElementById(e.column.getId() + e.rowIndex) !== null) {
                        if(document.getElementById(e.column.getId() + e.rowIndex).type === 'text') {
                            if(document.getElementById(e.column.getId() + e.rowIndex).classList.contains('focused')){
                                     document.getElementById(e.column.getId() + e.rowIndex).focus();                               
                            }else if(document.getElementById(e.column.getId() + e.rowIndex).classList.contains('f_focused')){
                                     document.getElementById(e.column.getId() + e.rowIndex).classList.add("focused");                                
                                     document.getElementById(e.column.getId() + e.rowIndex).classList.remove('f_focused');
                                     document.getElementById(e.column.getId() + e.rowIndex).focus();
                            }else{
                                    document.getElementById(e.column.getId() + e.rowIndex).classList.add('f_focused');
                                   
                                    document.getElementById(e.column.getId() + e.rowIndex).select();
                            }               
                            
                        } else {
                            document.getElementById(e.column.getId() + e.rowIndex).focus();
                        }
                    }
                }
        }
    }

    payScheduleValidation(row) {
        row["payScheduleNameValidation"] = "";
        row["payCurrencyValidation"] = "";
        if(required(row.payScheduleName)){            
            row["payScheduleNameValidation"] = "Pay Rate Schedules is required";
        }
        if(required(row.payCurrency)){            
            row["payCurrencyValidation"] = "Currency is required";
        }
        return row;
    }

    bindPayScheduleGrid() {        
        let rateSchedules = this.props.payRateSchedule && this.props.payRateSchedule.filter(x => x.recordStatus !== "D");
        const scheduleOrder=arrayUtil.bubbleSort(rateSchedules, 'sequenceId');
        rateSchedules = arrayUtil.gridTopSort(scheduleOrder);
        if(!isEmptyOrUndefine(rateSchedules)) {
            if(rateSchedules){
                if(rateSchedules.length > 0){
                    const selectedData = this.payScheduleGrid.getSelectedRows();                
                    rateSchedules.map((itratedValue,index)=>{
                        if(selectedData !== undefined && selectedData.length > 0 && itratedValue.id === selectedData[0].id) { 
                            rateSchedules[index]["isSelected"] = true;
                        } else {
                            rateSchedules[0]["isSelected"] = true;
                            rateSchedules[index]["isSelected"] = false;
                        }

                        rateSchedules[index]["payScheduleNameValidation"] = required(itratedValue.payScheduleName) ? "Pay Rate Schedules is required" : "";                    
                        rateSchedules[index]["payCurrencyValidation"] = required(itratedValue.payCurrency) ? "Currency is required" : "";
                        
                        if(!required(itratedValue.payScheduleName) && itratedValue.payScheduleName.length>=50) { 
                            rateSchedules[index]["payScheduleNameValidation"] = rateSchedules.filter(x=> x.id !== itratedValue.id && x.payScheduleName.substring(0, 50).toLowerCase() === itratedValue.payScheduleName.substring(0, 50).toLowerCase()).length > 0 ? 
                                                                            localConstant.contract.rateSchedule.PAY_RATE_FIRST_50_CHAR_DUPLICATE : "";
                            
                        }
                        else if(!required(itratedValue.payScheduleName)) {                                                                      
                            rateSchedules[index]["payScheduleNameValidation"] = rateSchedules.filter(x=> x.id !== itratedValue.id &&  x.payScheduleName.toLowerCase() === itratedValue.payScheduleName.toLowerCase()).length > 0 ? 
                                                                            localConstant.contract.rateSchedule.RATE_SCHEDULE_DUPLICATE : "";     
                        }
                    });
                }
            }

            if(!isEmptyOrUndefine(rateSchedules)) {
                const payScheduleDetail = [];
                const isReadOnly = (this.isPayRateDisabled || this.props.interactionMode 
                    || this.props.pageMode === localConstant.commonConstants.VIEW || this.fieldDisableHandler()  ? true : false);
                rateSchedules.forEach(row => {                    
                    row.readonly = isReadOnly;
                    payScheduleDetail.push(row);
                });
                this.setState((state) => {
                    return {
                        selectedPayScheduleDetails: payScheduleDetail
                    };
                }); 
            } else {
                this.setState((state) => {
                    return {
                        selectedPayScheduleDetails: []
                    };
                });
            }
        } else {
            this.setState((state) => {
                return {
                    selectedPayScheduleDetails: []
                };
            });            
        }     
    }

    bindPayScheduleoncancel() {

    }

    checkNumber = (e) => {
        if(e.target.value ==="."){
            e.target.value="0";
        }
        if(!isEmpty(e.target.value) &&e.target.value >0 ){
        e.target.value = parseFloat(numberFormat(e.target.value)).toFixed(4);
        }
        else{
            e.target.value="0.0000";
        }
        this.formInputHandler(e);
    }

    decimalWithLimtFormat = (evt) => {  
        const e = evt || window.event;   
        const expression = ("(\\d{0,"+ parseInt(10)+ "})[^.]*((?:\\.\\d{0,"+ parseInt(4) +"})?)");
        const rg = new RegExp(expression,"g"); 
        const match = rg.exec(e.target.value.replace(/[^\d.]/g, ''));
        e.target.value = match[1] + match[2]; 

        if (e.preventDefault) e.preventDefault();
    };

    onCellFocused = (e) => {    
        if(e.rowIndex !== null) {                      
            this.bindPayRateDetailGrid();
            const isReadonly = (this.state.SelectedPayScheduleName === "select" || this.state.SelectedPayScheduleName === "" 
                                    || this.isPayRateDisabled || this.props.interactionMode ? true : false);

            if(e.column.getId() === 'effectiveFrom' || e.column.getId() === 'effectiveTo') {
                this.setState({ isDatePicker : true });
            } else {
                this.setState({ isDatePicker : false });
            }  
            if(!isReadonly) {
                const selectedId = (e.column.getId() === 'isActive' ? ("isActiveRate" + e.rowIndex) : (e.column.getId() + e.rowIndex));
                if(document.getElementById(selectedId) !== null) {
                    if(document.getElementById(selectedId).type === 'text') {
                        document.getElementById(selectedId).select();
                    }
                    //Added Else if condition for Sanity Def 133
                    else {
                        document.getElementById(selectedId).focus();
                    }
                }
            }
        }
    }

    PayRateDetailValidation(row) {        
        row["expenseTypeValidation"] = "";
        row["rateValidation"] = "";
        row["effectiveFromValidation"] = "";
        row["effectiveToValidation"] = "";
        if(required(row.expenseType)){            
            row["expenseTypeValidation"] = "Type is required";
        }
        if(requiredNumeric(row.rate) || row.rate === "NaN"){            
            row["rateValidation"] = "Rate is required";
        }
        if(required(row.effectiveFrom)){            
            row["effectiveFromValidation"] = "Effective From is required";
        }        
        if(!required(row.effectiveFrom) && !this.isValidDateFormat(row.effectiveFrom)){            
            row["effectiveFromValidation"] = localConstant.commonConstants.INVALID_DATE_FORMAT;
            row["effectiveFrom"] = "";
        }
        if(required(row.effectiveTo)){            
            row["effectiveToValidation"] = "Effective To is required";
        }
        if(!required(row.effectiveTo) && !this.isValidDateFormat(row.effectiveTo)){
            row["effectiveToValidation"] = localConstant.commonConstants.INVALID_DATE_FORMAT;
            row["effectiveTo"] = "";
        }
        if (!required(row.effectiveFrom) && !required(row.effectiveTo) && this.isValidDateFormat(row.effectiveFrom) && this.isValidDateFormat(row.effectiveTo) 
                && moment(row.effectiveFrom).isAfter(row.effectiveTo, 'day')) {
            row["effectiveFromValidation"] = localConstant.techSpecValidationMessage.PAY_RATE_DATE_RANGE_VALIDATION;
        }
        return row;
    }

    isValidDateFormat(date) {        
        if(date.indexOf(":") === -1 && dateUtil.isUIValidDate(date)) {
            return false;
        }
        return true;
    }

    bindPayRateDetailGrid() {        
        if(this.state.isLoadOnCancel) {
            let chargeType = [];
            let payRateDetail = [];
            if(this.props.payRateSchedule && this.props.payRateSchedule.length > 0) {
                const payScheduleData = this.props.payRateSchedule;                
                const payScheduleId = payScheduleData[0].id;                
                this.props.DetailPayRateOnCancel && this.props.DetailPayRateOnCancel.map(iteratedValue => {
                    if (iteratedValue.payScheduleId === payScheduleId) {
                        chargeType.push(iteratedValue);
                    }
                });

                if(!isEmptyOrUndefine(chargeType)) {                        
                    const isReadOnly = (this.isPayRateDisabled || this.props.interactionMode || this.fieldDisableHandler() ? true : false);  // Hot Fixes Changes done as per ITK requirement
                    chargeType.forEach(row => {
                        if(row.sequenceId) {
                            row = this.PayRateDetailValidation(row);
                            row.readonly = isReadOnly;
                            row.payRateDisable = this.props.isbtnDisable;
                            payRateDetail.push(row);
                        }
                    });      
                    payRateDetail = payRateDetail.sort((a, b) => (a.sequenceId < b.sequenceId) ? 1 : -1);                                          
                    chargeType = chargeType.sort((a, b) => (a.expenseType > b.expenseType) ? 1 : -1);                                      
                    chargeType.forEach(row => {
                        if(!row.sequenceId) {
                            row = this.PayRateDetailValidation(row);
                            row.readonly = isReadOnly;
                            row.payRateDisable = this.props.isbtnDisable;
                            payRateDetail.push(row);
                        }
                    });
                    this.setState({                     
                        selectedChargeRateDetails: payRateDetail
                    });                                       
                } else {
                    this.setState({                     
                        selectedChargeRateDetails: []
                    }); 
                }
            } else {
                this.setState({                     
                    selectedPayRateDetails: []
                });
            }
            if(this.child && this.child.gridApi) this.child.gridApi.setRowData(payRateDetail);
            this.setState({ isLoadOnCancel : false,isDatePicker : true }); //D1246
        } else {
            const { DetailPayRate } = this.props;
            if(!isEmptyOrUndefine(DetailPayRate)) {
    
                let payScheduleIds;
                let payScheduleName='';
                if(!isEmpty(this.payScheduleGrid.getSelectedRows())) {
                    const selectedData = this.payScheduleGrid.getSelectedRows();  
                    payScheduleIds = selectedData[0].id;
                    payScheduleName = selectedData[0].payScheduleName;
                }
    
                let payRateDetails = DetailPayRate && DetailPayRate.filter((value) =>
                    (value.payScheduleId === payScheduleIds && value.recordStatus !== "D") || (value.payScheduleId === payScheduleIds && value.payScheduleName === payScheduleName)); //D384  IGO_QC_895
                // if(this.props.technicalSpecialistInfo.isDraft){ //D790
                //     payRateDetails = DetailPayRate && DetailPayRate.filter((value) =>
                //      value.payScheduleName === payScheduleName);
                // }
                
                const payRateOrder=arrayUtil.bubbleSort(payRateDetails, 'sequenceId');
                payRateDetails=arrayUtil.gridTopSort(payRateOrder);
    
                if(!isEmptyOrUndefine(payRateDetails)) {
                    let payRateDetailn = [];
                    const isReadOnly = (this.isPayRateDisabled || this.props.interactionMode || this.fieldDisableHandler() ? true : false);  // Hot Fixes Changes done as per ITK requirement
                    payRateDetails.forEach(row => {
                        if(!this.state.isLoadOnSave && row.sequenceId) {
                            row = this.PayRateDetailValidation(row);
                            row.readonly = isReadOnly;
                            row.payRateDisable = this.props.isbtnDisable;
                            payRateDetailn.push(row);
                        }
                    });
                    payRateDetailn = payRateDetailn.sort((a, b) => (a.sequenceId < b.sequenceId) ? 1 : -1);     
                    payRateDetails = payRateDetails.sort((a, b) => (a.expenseType > b.expenseType) ? 1 : -1);
                    payRateDetails.forEach(row => {
                        if(this.state.isLoadOnSave || !row.sequenceId) {
                            row = this.PayRateDetailValidation(row);
                            row.readonly = isReadOnly;
                            row.payRateDisable= true;//this.props.isbtnDisable;
                            payRateDetailn.push(row);
                        }
                    });
                    this.setState((state) => {
                        return {
                            selectedPayRateDetails: payRateDetailn
                        };
                    });
                    if(this.state.isDatePicker && this.child && this.child.gridApi) this.child.gridApi.setRowData(payRateDetailn);
                } else {
                    this.setState((state) => {
                        return {
                            selectedPayRateDetails: []
                        };
                    });
                }
            } else {
                this.setState((state) => {
                    return {
                        selectedPayRateDetails: []
                    };
                });
            }
        }   
        if(this.state.isLoadOnSave) this.setState({ isLoadOnSave : false });       
    }

    addPaySchedule = (e) => {
        e.preventDefault();
        this.updatedData["isActive"] = true;
        this.updatedData["epin"] = this.props.technicalSpecialistInfo.epin !== undefined ? this.props.technicalSpecialistInfo.epin : 0;
        this.updatedData["recordStatus"] = "N";
        this.updatedData["id"] = -Math.floor(Math.random() * (Math.pow(10, 5))); //D660
        this.updatedData["baseScheduleName"] = null;        
        if(this.state.orderSequenceId >= 1 ){
            this.updatedData["sequenceId"] = this.state.orderSequenceId + 1;
            this.setState({
                orderSequenceId:this.updatedData["sequenceId"]
            });
        }
        this.updatedData["payScheduleName"] = "";
        this.updatedData["payScheduleNameValidation"] = "Pay Rate Schedules is required";
        this.updatedData["payCurrency"] = "";
        this.updatedData["payCurrencyValidation"] = "Currency is required";
        this.setState({
            SelectedPayRateSchedule: this.updatedData,
            SelectedPayScheduleName: ""
        });
        this.updatedData.isSelected = true;
        this.props.actions.AddPayRateSchedule(this.updatedData);        
        this.state.selectedPayScheduleDetails.forEach((itratedValue) => {            
            itratedValue.isSelected= false;
        });        
        this.state.selectedPayScheduleDetails.unshift(this.updatedData);
        this.payScheduleGrid.gridApi.setRowData(this.state.selectedPayScheduleDetails);  
        this.payScheduleGrid.gridApi.setFocusedCell(0, "payScheduleName");         
        this.setState((state) => {
            return {
                selectedPayRateDetails: []
            };
        });     
        this.updatedData = {};          
    }

    addPayRate = (e) => {
        e.preventDefault();   
        const schedule  = this.props.payRateSchedule.filter(x => x.id === this.state.SelectedPayRateSchedule.id);
        this.updatedData = {};    
        this.updatedData["effectiveFrom"] = currentDate;
        this.updatedData["effectiveTo"] = "";
        this.updatedData["isActive"] = true;
        this.updatedData["recordStatus"] = "N";
        this.updatedData["id"] = Math.floor(Math.random() * (Math.pow(10, 5)));
        this.updatedData["payScheduleName"] = schedule[0].payScheduleName;
        this.updatedData["payScheduleCurrency"] = schedule[0].payCurrency;
        this.updatedData["epin"] = this.props.technicalSpecialistInfo.epin !== undefined ? this.props.technicalSpecialistInfo.epin : 0;
        this.updatedData["rate"] = "";
        this.updatedData["expenseTypeValidation"] = "Type is required";
        this.updatedData["rateValidation"] = "Rate is required";
        this.updatedData["effectiveToValidation"] = "Effective to is required";        
        if(this.payScheduleGrid !== undefined && this.payScheduleGrid.getSelectedRows() !== undefined) {
            const selectedData = this.payScheduleGrid.getSelectedRows();  
            this.updatedData["payScheduleId"] = selectedData[0].id;
        }
        if(this.state.orderSequenceId >= 1 ){
            this.updatedData["sequenceId"] = this.state.orderSequenceId + 1;
            this.setState({
                orderSequenceId:this.updatedData["sequenceId"]
            });
        }                    
        this.props.actions.AddDetailPayRate(this.updatedData);                    
        this.state.selectedPayRateDetails.unshift(this.updatedData);
        this.child.gridApi.setRowData(this.state.selectedPayRateDetails);     
        this.child.gridApi.setFocusedCell(0, "expenseType");      
        this.updatedData = {};  
    }

    render() {
        const { payRateSchedule, rateScheduleEditData, DetailPayRate, expenseType, payrollName, technicalSpecialistInfo,payRateType } = this.props;
       
       // const payRateType =expenseType && expenseType.filter(x => x.description === 'PayExpenseType'); //def 552

        let payrateScheduleEditData = {};
        if (rateScheduleEditData) {
            payrateScheduleEditData = rateScheduleEditData;
        }
        //let rateSchedules = payRateSchedule && payRateSchedule.filter(x => x.recordStatus !== "D");
        // if (!this.state.initialScheduleBinded && rateSchedules) {
        //     if (rateSchedules.length > 0) {
        //         this.setState({
        //             SelectedPayRateSchedule: rateSchedules[0],
        //             SelectedPayScheduleName: rateSchedules[0].payScheduleName,
        //             initialScheduleBinded: true
        //         });
        //     }
        // }
        // if(rateSchedules){
        //     if(rateSchedules.length > 0){
        //         rateSchedules.map((itratedValue,index)=>{
        //             if (isEmpty(this.state.SelectedPayRateSchedule.payScheduleName)) {
        //                 rateSchedules[0]["isSelected"] = true;
        //             }
        //             else if(itratedValue.payScheduleName === this.state.SelectedPayRateSchedule.payScheduleName){
        //                 rateSchedules[index]["isSelected"] = true;
        //             }
        //             else{
        //                 rateSchedules[index]["isSelected"] = false;
        //             }
        //             if(this.state.isDeletePayScheduleModalOpen && rateSchedules.length > 1){
        //                 rateSchedules[index]["isSelected"] = false;
        //             }
        //         });
        //     }
        // }

        // const scheduleOrder=arrayUtil.bubbleSort(rateSchedules, 'sequenceId');
        // rateSchedules = arrayUtil.gridTopSort(scheduleOrder);

        // DetailPayRate.map(iteratedValue =>{
        //     iteratedValue.rate=parseFloat(iteratedValue.rate).toFixed(4);
        // });

        // let payRateDetails = DetailPayRate && DetailPayRate.filter((value) =>
        //     value.payScheduleName === this.state.SelectedPayRateSchedule.payScheduleName && value.recordStatus !== "D");
        
        // const payRateOrder=arrayUtil.bubbleSort(payRateDetails, 'sequenceId');
        // payRateDetails=arrayUtil.gridTopSort(payRateOrder);

        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        this.userTypes = isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE));
        const disableField = this.fieldDisableHandler();
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                {this.state.payRateShowModal &&
                    <Modal title={this.state.payRateEditMode ? localConstant.techSpec.PayRate.EDIT_PAYRATE_SCHEDULE :
                        localConstant.techSpec.PayRate.PAY_RATE_SCEDULE}
                        buttons={this.state.payRateEditMode ? this.payRateScheduleEditButtons : this.payRateScheduleAddButtons}
                        isShowModal={this.state.payRateShowModal}
                    >
                        < PayRateScheduleModalPopUp
                            rateScheduleDefaultData={payrateScheduleEditData}
                            currencyData={this.currencyData}
                            formInputHandler={(e) => this.formInputHandler(e)}
                            disableField={disableField} />
                    </Modal>}
                {
                    this.state.isDeletePayScheduleModalOpen ? 
                    <Modal title="Delete PaySchedule" modalId="DeleteSchedulePopup" formId="DeleteScheduleForm" buttons={this.DeletePayScheduleButtons} isShowModal={this.state.isDeletePayScheduleModalOpen}>
                    <DeletePaySchedulePopup
                        schedules={this.props.payRateSchedule.filter(x => x.recordStatus !== "D")}
                        headerData={this.headerData.deletePayScheduleHeader}
                        gridRef = {ref => { this.deletePayScheduleChild = ref; }}
                    />
                </Modal>
                :null
                }
                {this.state.detailPayRateScheduleModal &&
                    <Modal id="detailModalPopup" title={this.state.detailpayEditMode ? localConstant.techSpec.PayRate.EDIT_PAY_RATE_DETAILS :
                        localConstant.techSpec.PayRate.ADD_PAY_RATE_DETAILS}
                        buttons={this.state.detailpayEditMode ? this.DetailPayRateEditButtons : this.DetailPayRateAddButtons}
                        isShowModal={this.state.detailPayRateScheduleModal}>
                        <PayRateDetailsPopup
                            fetchEffectiveFrom={this.fetchEffectiveFrom} EffectiveFrom={this.state.effectiveFromDate}
                            fetchEffectiveTo={this.fetchEffectiveTo} EffectiveTo={this.state.effectiveToDate}
                            DetailPay={DetailPayRate}
                            formInputHandler={this.formInputHandler}
                            DefaultDetailData={this.editedRowData}
                            expenseType={payRateType ? payRateType : []}
                            checkNumber={this.checkNumber}
                            decimalWithLimtFormat={this.decimalWithLimtFormat}
                            dateBlurHandler={(e) => this.handleDateBlur(e)}
                            disableField={disableField}
                        />
                    </Modal>}
                {this.state.multipleDetailPayRateScheduleModal ?
                    <Modal modalClass="detailPayRateScheduleModal" 
                        title={this.state.detailpayEditMode ? localConstant.techSpec.PayRate.EDIT_PAY_RATE_DETAILS :
                                localConstant.techSpec.PayRate.ADD_PAY_RATE_DETAILS}
                        buttons={
                            [ {
                                name: localConstant.commonConstants.CANCEL,
                                action: this.cancelmultipleDetailPayRate,
                                btnClass: "btn-small mr-2",
                                showbtn: true,
                                type: "button"
                            },
                            {
                                name: localConstant.commonConstants.SUBMIT,
                                action:(e)=>this.submitmultipleDetailPayRate(e),
                                btnClass: "btn-small mr-2",
                                showbtn: true,
                            } ]
                        }
                        isShowModal={this.state.multipleDetailPayRateScheduleModal}>

                        <div className="customCard left">
                            <ReactGrid
                                gridRowData={payRateType ? payRateType : []}
                                gridColData={this.headerData.MultiplePayRateHeader}
                                onRef={ref => { this.multipleDetailPayRateGrid = ref; }}
                            />
                        </div>
                    </Modal> : null}
                <div className=" customCard">
                    <PayRateComponent payrollName={payrollName}
                        formInputHandler={this.payInputHandler}
                        technicalSpecialistInfo={technicalSpecialistInfo}
                        isPayRateDisabled={this.isPayRateDisabled}
                        interactionMode={this.props.interactionMode}
                        disableField={disableField} />
                    <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.PayRate.PAY_RATE_SCEDULES} titleClass={ this.props.technicalSpecialistInfo.profileStatus === 'Active' ? "pl-0 bold mandate" : "pl-0 bold"} colSize="s12" >
                    <ReactGrid gridRowData={this.state.selectedPayScheduleDetails} 
                        gridColData={this.headerData.payScheduleHeader}
                        rowSelected={this.payScheduleOnSelectHandler} 
                        onRef={ref => { this.payScheduleGrid = ref; }} 
                        onCellFocused={this.onScheduleCellFocused}
                        paginationPrefixId={localConstant.paginationPrefixIds.techSpecPayScheduleGridId}
                    />
                        {/* <div className="row mb-0">
                            <CustomInput
                                hasLabel={true}
                                divClassName='col loadedDivision'
                                label={localConstant.techSpec.PayRate.PAY_RATE_SCEDULE}
                                type='select'
                                name='payScheduleName'
                                colSize="s3"
                                optionsList={rateSchedules}
                                className="browser-default customInputs"
                                onSelectChange={(e) => this.payRateScheduleOnChangeHandler(e)}
                                optionName='payScheduleName'
                                optionValue='payScheduleName'
                                defaultValue={this.state.SelectedPayRateSchedule.payScheduleName}
                                disabled={this.isPayRateDisabled || this.props.interactionMode ||disableField}
                            />
                            </div> */}
                           {this.props.pageMode !== localConstant.commonConstants.VIEW && !disableField && <div className="right-align">                           
                                <button disabled={this.isPayRateDisabled || this.props.interactionMode} onClick={this.addPaySchedule} className="btn-small btn-primary mr-1 customCreateDiv waves-effect">
                                    {localConstant.commonConstants.ADD}</button>                                
                                {/* <button onClick={this.payRateScheduleEditHandler} disabled={this.state.SelectedPayScheduleName === "select" ||
                                    this.state.SelectedPayScheduleName === "Select" || this.state.SelectedPayScheduleName === "" || this.isPayRateDisabled || this.props.interactionMode ? true : false}
                                    className="btn-small btn-primary mr-1 customCreateDiv waves-effect">{localConstant.commonConstants.EDIT}</button> */}
                                <button onClick={this.deletePayScheduleHandler} disabled={this.state.SelectedPayScheduleName === "select" ||
                                    this.state.SelectedPayScheduleName === "Select" || this.state.SelectedPayScheduleName === "" || this.isPayRateDisabled || this.props.interactionMode? true : false}
                                    className="btn-small btn-primary mr-1 dangerBtn modal-trigger waves-effect customCreateDiv" >{localConstant.commonConstants.DELETE}</button>
                            </div>}
                        
                        {/* <div className="row mt-2 ">
                            <div className="col s2 labelPrimaryColor mt-1">
                                <span>
                                    <span class=" pr-0 bold"> {localConstant.techSpec.PayRate.CURRENCY}: </span>
                                    {this.state.SelectedPayRateSchedule.payCurrency} </span>
                            </div>
                            <CustomInput
                                type='switch'
                                switchLabel={localConstant.techSpec.PayRate.HIDE_RATE}
                                colSize="col s3"
                                isSwitchLabel={true}
                                switchName="isActive"
                                className="lever"
                                disabled={true}
                                onChangeToggle={this.formInputHandler}
                                checkedStatus={this.state.SelectedPayRateSchedule.isActive} />
                            <div className="col s6 labelPrimaryColor pl-0 mt-1">
                                <span className="textNoWrapEllipsis col s8" title={this.state.SelectedPayRateSchedule && this.state.SelectedPayRateSchedule.payScheduleNote}>
                                    <strong className="bold">Schedule Notes : </strong>{this.state.SelectedPayRateSchedule && this.state.SelectedPayRateSchedule.payScheduleNote}
                                </span>
                            </div>
                        </div> */}
                    </CardPanel>
                    <CardPanel className="white lighten-4 black-text" title="Pay Rate Details" colSize="s12 " titleClass={ this.props.technicalSpecialistInfo.profileStatus === 'Active' ? "pl-0 bold mandate" : "pl-0 bold"}>
                    <div className="chargeRateGrid">
                        <ReactGrid                             
                            gridRowData={this.state.selectedPayRateDetails}
                            gridColData={this.headerData.PayRateHeader} 
                            onRef={ref => { this.child = ref; }} 
                            onCellFocused={this.onCellFocused} 
                            paginationPrefixId={localConstant.paginationPrefixIds.techSpecPayRateGridId}                           
                        />
                      </div>
                        {this.props.pageMode !== localConstant.commonConstants.VIEW && !disableField && <div className="right-align mt-2">
                        <a onClick={this.multiplePayRateDetailModal}
                                disabled={this.state.SelectedPayScheduleName === "select" || this.state.SelectedPayScheduleName === "" 
                                    || this.isPayRateDisabled || this.props.interactionMode ? true : false}
                                className="btn-small waves-effect">{localConstant.commonConstants.ADD_MULTIPLE_RATES}</a>
                            <a onClick={this.addPayRate}
                                disabled={this.state.SelectedPayScheduleName === "select" || this.state.SelectedPayScheduleName === "Select"
                                    || this.state.SelectedPayScheduleName === "" || this.isPayRateDisabled || this.props.interactionMode ? true : false}
                                className="btn-small ml-2 waves-effect">{localConstant.commonConstants.ADD}</a>
                            <a onClick={this.detailPayRateDeleteClickHandler}
                                disabled={this.state.SelectedPayScheduleName === 'Select' || this.state.SelectedPayScheduleName === '' ||
                                this.state.selectedPayRateDetails.length <= 0 || this.isPayRateDisabled || this.props.interactionMode ? true : false}
                                className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn ">{localConstant.commonConstants.DELETE}</a>
                        </div> }
                    </CardPanel>
                </div>
            </Fragment>
        );
    }
}
export default PayRate;                       
