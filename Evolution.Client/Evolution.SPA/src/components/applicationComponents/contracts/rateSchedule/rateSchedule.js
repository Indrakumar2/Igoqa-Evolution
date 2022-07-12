import React, { Component, Fragment } from 'react';
import PropTypes from 'proptypes';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import ReactGridTwo from '../../../../common/baseComponents/reactAgGridTwo';
import { getlocalizeData, numberFormat,isEmpty, isEmptyReturnDefault,bindAction,deepCopy, isEmptyOrUndefine } from '../../../../utils/commonUtils';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import { HeaderData, CopyScheduleHeader, DeleteScheduleHeader, ViewChargeRateHeader,RateScheduleNameHeader,AdminContractRatesHeaderData } from './headerData';
import CustomModal from '../../../../common/baseComponents/customModal';
import Modal from '../../../../common/baseComponents/modal';
import moment from 'moment';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import dateUtil from '../../../../utils/dateUtil';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import LabelWithValue from '../../../../common/baseComponents/customLabelwithValue';
import objectUtil from '../../../../utils/objectUtil';
import { sortCurrency } from '../../../../utils/currencyUtil';
import { generatePdf } from '../../../../utils/pdfUtil';
import { GetRateScheduleTemplate } from "../../../../utils/templateUtil";
import arrayUtil from '../../../../utils/arrayUtil';
import { required,requiredNumeric } from '../../../../utils/validator';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
//import NumberFormat from 'react-number-format';

const localConstant = getlocalizeData();
/** Rate Schedult Popup */
const RateSchedulePopup = (props) => {
    return (
        <Fragment>
            <CustomInput
                hasLabel={true}
                labelClass="customLabel mandate"
                label={localConstant.contract.rateSchedule.SCHEDULE_NAME}
                divClassName="m4"
                type='text'
                refProps='scheduleNameId'
                name="scheduleName"
                defaultValue={props.rateScheduleDefaultData.scheduleName}
                colSize='s4'
                inputClass="customInputs"
                maxLength={fieldLengthConstants.Contract.rateSchedule.SCHEDULE_NAME_MAXLENGTH}
                onValueChange={props.rateScheduleChange}

            />
            <CustomInput
                hasLabel={true}
                label={localConstant.contract.rateSchedule.SCHEDULE_NAME_PRINTED_ON_INVOICE}
                divClassName="m4"
                type='text'
                refProps="scheduleNamePrintedOnInvoiceId"
                name="scheduleNameForInvoicePrint"
                defaultValue={props.rateScheduleDefaultData.scheduleNameForInvoicePrint}
                colSize='s4'
                inputClass="customInputs"
                maxLength={fieldLengthConstants.Contract.rateSchedule.SCHEDULE_NAME_PRINTED_ON_INVOICE_MAXLENGTH}
                onValueChange={props.rateScheduleChange}
            />
            <CustomInput
                hasLabel={true}
                divClassName='col'
                labelClass="customLabel mandate"
                required={true}
                name="chargeCurrency"
                label={localConstant.contract.rateSchedule.CURRENCY}
                type='select'
                colSize='s12 m4'
                defaultValue={props.rateScheduleDefaultData.chargeCurrency}
                className="browser-default customInputs"
                optionsList={props.currency}
                optionName='code'
                optionValue="code"
                id="currencyId"
                onSelectChange={props.rateScheduleChange}
            />
        </Fragment>
    );
};

/** Charge Type Popup */
const ChargeTypePopup = (props) => {
    return (
        <Fragment>
            <CustomInput
                hasLabel={true}
                divClassName='col'
                optionClassName='chargeTypeOption'
                optionClassArray = {localConstant.commonConstants.NDT_CHARGE_RATE_TYPE}
                optionProperty = "chargeType"
                labelClass="customLabel mandate"
                required={true}
                name="chargeType"
                label={localConstant.contract.rateSchedule.CHARGE_TYPE}
                type='select'
                colSize='s12 m3 reset-mb-0'
                className="browser-default customInputs"
                defaultValue={props.chargeTypeDefaultData.chargeType}
                optionsList={props.contractChargeTypes}
                optionName='name'
                optionValue="name"
                id="chargeTypeId"
                onSelectChange={props.chargeTypeChange}
            />
            <CustomInput
                hasLabel={true}
                label={localConstant.contract.rateSchedule.STANDARD_VALUE}
                divClassName="m3"
                type='number'
                refProps="standardValueId"
                name="standardValue"
                defaultValue={(props.chargeTypeDefaultData.standardValue !== 0 && props.chargeTypeDefaultData.standardValue !== undefined) ? props.chargeTypeDefaultData.standardValue : "0.00"}
                colSize='s12'
                inputClass="customInputs"
                maxLength="20"
                disabled={true}
                readOnly={true}
                onValueChange={props.chargeTypeChange}
            />
            {/* <CustomInput
                hasLabel={true}
                label={localConstant.contract.rateSchedule.STANDARD_VALUE}
                divClassName="m3"
                decimalScale={2}
                fixedDecimalScale={true}
                type='decimal'
                name="standardValue"
                // defaultValue={props.chargeTypeDefaultData.standardValue}
                value={(props.chargeTypeDefaultData.standardValue != 0 && props.chargeTypeDefaultData.standardValue !== undefined) ?props.chargeTypeDefaultData.standardValue:"0.00"}
                colSize='s12'
                inputClass="customInputs"
                maxLength="20"
                onValueChange={(e)=>props.chargeTypeChange({ target:{ type:'decimal',value:e.value,name:'standardValue' } })}
            /> */}
            <CustomInput
                hasLabel={true}
                label={localConstant.contract.rateSchedule.CHARGE_VALUE}
                labelClass="customLabel mandate"
                divClassName="m3"
                type='text'
                //step="any"
                 dataType='decimal'
                refProps="chargeValueId"
                name="chargeValue"
                defaultValue={(props.chargeTypeDefaultData.chargeValue !== 0 && props.chargeTypeDefaultData.chargeValue !== undefined) ? props.chargeTypeDefaultData.chargeValue : "0.0000"}
                colSize='s12'
                inputClass="customInputs"
                min="0"
                maxLength={13}
                prefixLimit={16}
                suffixLimit={2}
                max={9999999999999}
                onInput="this.value = this.value.replace(/[^0-9\.]/g, '').split(/\./).slice(0, 2).join('.')"
                required={true}
                onValueChange={props.formInputChangeHandler}
                onValueBlur={props.checkNumber}
                onValueInput={props.decimalWithLimtFormat}
            /> 
                      {/* <CustomInput
                hasLabel={true}
                labelClass="customLabel mandate"
                label={localConstant.contract.rateSchedule.CHARGE_VALUE}
                divClassName="m3"
                decimalScale={2}
                fixedDecimalScale={true}
                type='decimal'
                name="chargeValue"
                // defaultValue={props.chargeTypeDefaultData.chargeValue}
                value = {props.chargeTypeDefaultData.chargeValue}
                colSize='s12'
                inputClass="customInputs"
                maxLength="20"
                onValueChange={(e)=>props.chargeTypeChange({ target:{ type:'decimal',value:e.value,name:'chargeValue' } })}
            /> */}
            <CustomInput
                hasLabel={true}
                label={localConstant.contract.rateSchedule.DISCOUNT_APPLIED}
                divClassName="m3"
                type='number'
                refProps="discountAppliedId"
                name="discountApplied"
                defaultValue={(props.chargeTypeDefaultData.discountApplied != 0 && props.chargeTypeDefaultData.discountApplied !== undefined) ? props.chargeTypeDefaultData.discountApplied : "0.00"}
                colSize='s12'
                inputClass="customInputs"
                maxLength="20"
                disabled={true}
                readOnly={true}
                onValueChange={props.chargeTypeChange}
            />
            <CustomInput
                hasLabel={true}
                label={localConstant.contract.rateSchedule.PERCENTAGE}
                divClassName="m4"
                type='number'
                refProps="percentageId"
                name="percentage"
                defaultValue={(props.chargeTypeDefaultData.percentage !== undefined && props.chargeTypeDefaultData.percentage != 0) ? props.chargeTypeDefaultData.percentage : "0.00"}
                colSize='s12'
                inputClass="customInputs"
                maxLength="20"
                disabled={true}
                readOnly={true}
                onValueChange={props.chargeTypeChange}
            />
            <CustomInput
                hasLabel={true}
                isNonEditDateField={false}
                label={localConstant.contract.rateSchedule.EFFECTIVE_FROM}
                labelClass="customLabel mandate"
                htmlFor="effectiveFrom"
                colSize='m4 s12'
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                onDatePickBlur={props.dateBlurHandler}
                type='date'
                name='effectiveFrom'
                selectedDate={props.state.effectiveFromDate}
                onDateChange={props.effectiveFromChange}
                shouldCloseOnSelect={true}
            />
            <CustomInput
                hasLabel={true}
                isNonEditDateField={false}
                labelClass="customLabel"
                htmlFor="effectiveTo"
                label={localConstant.contract.rateSchedule.EFFECTIVE_TO}
                onDatePickBlur={props.endDateBlurHandler}
                colSize='m4 s12'
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                type='date'
                name='effectiveTo'
                selectedDate={props.state.effectiveToDate}
                onDateChange={props.effectiveToChange}
                shouldCloseOnSelect={true}
            />
            <CustomInput
                hasLabel={true}
                label={localConstant.contract.rateSchedule.DESCRIPTION}
                divClassName="m6"
                type='textarea'
                required={true}
                name='description'
                colSize='s12'
                maxLength={fieldLengthConstants.Contract.rateSchedule.CHARGE_RATE_DESCRIPTION_MAXLENGTH}
                inputClass="customInputs"
                defaultValue={props.chargeTypeDefaultData.description}
                onValueChange={props.chargeTypeChange}
            />

            <CustomInput
                type='switch'
                switchLabel={localConstant.contract.rateSchedule.HIDDEN}
                isSwitchLabel={true}
                checkedStatus={props.chargeTypeDefaultData.isActive}
                //isSwitchLabel={this.props.editedPairollPeriodName.isActive}
                switchName="isActive"
                id="chargeTypeStatus"
                colSize='m3 s12'
                onChangeToggle={props.chargeTypeChange}
            />
            <label className="col s12 m3">
                <input id="descriptionPrintOnInvoice" name="isDescriptionPrintedOnInvoice" type="checkbox" className="filled-in" onClick={props.chargeTypeChange} defaultChecked={props.chargeTypeDefaultData.isDescriptionPrintedOnInvoice} />
                <span className="labelPrimaryColor">{localConstant.contract.rateSchedule.DESCRIPTION_PRINT_ON_INVOICE}</span>
            </label>    
        </Fragment>
    );
};

/** Copy Charge Type Popup */
const CopyChargeTypePopup = (props) => {
    return (
        <Fragment>
            <CustomInput
                hasLabel={true}
                isNonEditDateField={false}
                label={localConstant.contract.rateSchedule.EFFECTIVE_FROM}
                labelClass="customLabel mandate"
                htmlFor="effectiveFrom"
                colSize='m4 s12'
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                onDatePickBlur={props.dateBlurHandler}
                type='date'
                name='effectiveFrom'
                selectedDate={props.effectiveFromDate}
                onDateChange={props.effectiveFromChange}
                shouldCloseOnSelect={true}
            />
            <CustomInput
                hasLabel={true}
                isNonEditDateField={false}
                labelClass="customLabel"
                htmlFor="effectiveTo"
                label={localConstant.contract.rateSchedule.EFFECTIVE_TO}
                onDatePickBlur={props.endDateBlurHandler}
                colSize='m4 s12'
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                type='date'
                name='effectiveTo'
                selectedDate={props.effectiveToDate}
                onDateChange={props.effectiveToChange}
                shouldCloseOnSelect={true}
            />
        </Fragment>
    );
};

/** Admin Contract Rate Popup */
const AdminContractRatePopup = (props) => {
    //427 - Rate Schedules tab while using Admin Contract Rates the Schedule Name is not concatenated with the currency code.
    const { adminSchedules } = props;
    let adminSchedulesList = [];
    let adminInspectionTypeWithoutDup=[];
    if (Array.isArray(adminSchedules) && adminSchedules.length > 0) {
        adminSchedulesList = deepCopy(adminSchedules);
        // const adminSchedulesList = adminSchedules.map(adminSchedule => Object.assign({}, adminSchedule));
        if (adminSchedulesList) {
            adminSchedulesList.forEach(adminSchedule => {
                adminSchedule.combinedNameCode = adminSchedule.name + "-" + adminSchedule.currency;
            });
        }
    }
    // 597 - CONTRACT-Charge Rates Issues - UAT Testing
    if (Array.isArray(props.adminInspectionType) && props.adminInspectionType.length > 0)
        adminInspectionTypeWithoutDup = arrayUtil.removeDuplicates(props.adminInspectionType, "name");    
    return(
        <Fragment>
            <CustomInput
                hasLabel={true}
                divClassName='col'
                labelClass="customLabel mandate"
                required={true}
                name="companyChargeScheduleName"
                label={localConstant.contract.rateSchedule.SCHEDULE_NAME}
                type='select'
                colSize='s12 m4'
                className="browser-default customInputs"
                defaultValue={props.adminChargeScheduleValueChange && props.adminChargeScheduleValueChange.companyChargeScheduleName}
                optionsList={adminSchedulesList}
                optionName='combinedNameCode'
                optionValue="name"
                id="adminScheduleNameId"
                onSelectChange={props.adminContractRatesChange}
            />
            <CustomInput
                hasLabel={true}
                divClassName='col'
                labelClass="customLabel mandate"
                required={true}
                name="companyChgSchInspectionGroupName"
                label={localConstant.contract.rateSchedule.INSPECTION_GROUP_CODE}
                type='select'
                colSize='s12 m4'
                className="browser-default customInputs"
                defaultValue={props.adminChargeScheduleValueChange && props.adminChargeScheduleValueChange.companyChgSchInspectionGroupName}
                optionsList={props.adminInspectionGroup}
                optionName='name'
                optionValue="name"
                id="adminInspectionGroupId"
                onSelectChange={props.adminContractRatesChange}
            />
            <CustomInput
                hasLabel={true}
                divClassName='col'
                labelClass="customLabel mandate"
                required={true}
                name="companyChgSchInspGrpInspectionTypeName"
                label={localConstant.contract.rateSchedule.INSPECTION_TYPE}
                type='select'
                colSize='s12 m4'
                className="browser-default customInputs"
                defaultValue={props.adminChargeScheduleValueChange && props.adminChargeScheduleValueChange.companyChgSchInspGrpInspectionTypeName}
                optionsList={adminInspectionTypeWithoutDup}
                optionName='name'
                optionValue="name"
                id="adminInspectionTypeId"
                onSelectChange={props.adminContractRatesChange}
            />
             <p className="bold col s12">{localConstant.contract.rateSchedule.CHARGE_RATES}</p>
                <div className="col s12">
                    <ReactGrid gridRowData={props.adminChargeRates} gridColData={props.AdminContractRatesHeader} onRef={props.gridRef} paginationPrefixId={localConstant.paginationPrefixIds.adminChargeRates}/>
                </div>
        </Fragment>
    );
};

/** Delete Charge Rate Popup */
const DeleteChargeRatePopup = (props) => {
    return(
        <Fragment>
            <CustomInput
                hasLabel={true}
                isNonEditDateField={false}
                labelClass="customLabel"
                htmlFor="effectiveTo"
                label={localConstant.contract.rateSchedule.EFFECTIVE_TO}
                onDatePickBlur={props.endDateBlurHandler}
                colSize='m12 s12'
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                type='date'
                name='effectiveTo'
                selectedDate={props.state.effectiveToDate}
                onDateChange={props.effectiveToChange}
                shouldCloseOnSelect={true}
            />
        </Fragment>
    );
};

/**View Charge Rate Popup */
const ViewChargeRatePopup = (props) => {
        return(
            <Fragment>
                  <LabelWithValue
                        className="custom-Badge col mb-2"
                        colSize="s12 m3 mt-4"
                        label={`${ localConstant.contract.rateSchedule.SCHEDULE_NAME }:`}
                        value={props.scheduleName}
                    />
                    <div className="col s12">
                        <ReactGrid 
                        gridRowData={props.chargeRates} 
                        gridColData={ViewChargeRateHeader}  
                        paginationPrefixId={localConstant.paginationPrefixIds.viewChargeRates}/>
                    </div>
            </Fragment>
        );
};

/** Copy Schedule Popup */
const CopySchedulePopup = (props) => {
    return (
        <Fragment>
            <ReactGrid gridRowData={props.schedules} gridColData={props.headerData} onRef={props.gridRef} paginationPrefixId={localConstant.paginationPrefixIds.copyRateSchedule}/>
        </Fragment>
    );
};

class RateSchedule extends Component {
    constructor(props) {
        super(props);
        this.state = {
            selectedScheduleName: "",
            initialScheduleBinded:false,
            SelectedRateSchedule: {},
            adminEffectiveFromDate:'',
            adminEffectiveToDate:'',
            effectiveFromDate: '',
            effectiveToDate: "",
            copyEffectiveFromDate:'',
            copyEffectiveToDate:'',
            inValidDateFormat: false,
            isDeleteChargeRatesModalOpen: false,
            isViewChargeRatesModalOpen:false,
            isCopyScheduleModalOpen: false,
            isDeleteScheduleModalOpen:false,
            /*AddScheduletoRF*/
            isAddScheduletoRFCModelOpen:false,
            orderSequenceId:1, 
            selectedChargeScheduleDetails: [],            
            selectedChargeRateDetails: [],
            isLoadOnCancel: false,
            isLoadOnSave: false,
            isDatePicker: false,
        };
        this.filteredRowCount=0;
        this.filteredSchedule=[];
        this.isChargeRateOverlaps=false;
        this.previousIndex = this.props.selectedRowIndex;
        this.rateScheduleAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelRateSchedule,
                btnID: "cancelCreateRateSchedule",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.createRateSchedule,
                btnID: "createRateSchedule",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        this.chargeTypeAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelChargeType,
                btnID: "cancelCreateChargeType",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.createChargeType,
                btnID: "createChargeType",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        this.rateScheduleEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelRateSchedule,
                btnID: "cancelCreateRateSchedule",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.updateRateSchedule,
                btnID: "editRateSchedule",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        this.chargeTypeEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelChargeType,
                btnID: "cancelCreateChargeType",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.updateChargeType,
                btnID: "editChargeType",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        this.copyChargeTypeAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.chargeRateCopyCancel,
                btnID: "chargeRateCopyBtn",
                btnClass: "waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.OK,
                action: this.chargeRateCopy,
                btnID: "chargeRateCopyBtn",
                btnClass: "waves-effect waves-teal btn-small mr-2",
                showbtn: true
            }
        ];
        this.adminContractRatesButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelAdminContractRates,
                btnID: "cancelAdminContractRates",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.addAdminContractRates,
                btnID: "addAdminContractRates",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        this.deleteChargeRatesButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelDeleteChargeRates,
                btnID: "cancelDeleteChargeRates",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.VIEW,
                action: this.viewChargeRates,
                btnID: "viewChargeRates",
                btnClass: "waves-effect waves-teal btn-small mr-2",
                showbtn: true
            },
            {
                name: localConstant.commonConstants.OK,
                action: this.deleteChargeRates,
                btnID: "deleteChargeRates",
                btnClass: "waves-effect waves-teal btn-small mr-2",
                showbtn: true
            }
       
        ];
        this.copyScheduleButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelCopySchedule,
                btnID: "cancelCopySchedule",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.copySelectedSchedules,
                btnID: "copySelectedSchedules",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        /*AddScheduletoRF*/
        this.addScheduleToRFbuttons= [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.canceladdScheduleToRF,
                btnID: "canceladdScheduleToRF",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.addscheduleConfirm,
                btnID: "addScheduleToRF",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        this.DeleteScheduleButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelDeleteSchedule,
                btnID: "cancelDeleteSchedule",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.RateScheduleDeleteHandler,
                btnID: "deleteSelectedSchedules",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        this.viewChargeRatesButtons=[
            {
                name:localConstant.commonConstants.OK,
                action:this.cancelViewChargeRate,
                btnID:"cancelViewChargeRate",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        
        this.updatedData = {};
        this.adminUpdatedData = {};
        /*AddScheduletoRF*/
        this.rateScheduleEditedRow={};
        this.viewChargeRates=[];
        bindAction(RateScheduleNameHeader, "EditScheduleName", this.RateScheduleEditHandler);

        this.props.callBackFuncs.onCancelCharge = () => {
            this.clearLineItemsOnCancel();
        };

        this.props.callBackFuncs.reloadLineItemsOnSave = () => {            
            this.reloadLineItemsOnSave();
        };

        this.props.callBackFuncs.reloadLineItemsOnSaveValidation = () => {            
            this.setState({ isLoadOnSave: false });            
            if(this.gridChargeSchedule) {
                this.bindChargeScheduleDetail();
            }
        };

        this.props.callBackFuncs.changeFocusForSave = () => {
            if (this.child && this.gridChargeSchedule) {
                this.child.gridApi.clearFocusedCell(); //Fix for D789
                this.gridChargeSchedule.gridApi.tabToPreviousCell();
                //this.child.gridApi.tabToNextCell();//Changes for D1217
                this.gridChargeSchedule.gridApi.tabToNextCell();
            }
        };
        //Commented For Defect 952
        // this.functionRef = {};
        // this.functionRef["selectAllRow"]= this.props.adminChargeRatesValue && this.props.adminChargeRatesValue.length> 0 && [ this.props.adminChargeRatesValue[0] ];
        // this.adminRatesHeader = AdminContractRatesHeaderData(this.functionRef);
        this.adminRatesHeader=AdminContractRatesHeaderData;
        bindAction(this.adminRatesHeader, "rateOnShoreOil", (data,e,type) => this.chargeRatesOnChangeHandler(data,e,'rateOnShoreOil'));
        bindAction(this.adminRatesHeader, "rateOnShoreNonOil", (data,e,type) => this.chargeRatesOnChangeHandler(data,e,'rateOnShoreNonOil'));
        bindAction(this.adminRatesHeader, "rateOffShoreOil", (data,e,type) => this.chargeRatesOnChangeHandler(data,e,'rateOffShoreOil'));

        // const functionRefs = {}; 
        // functionRefs["refreshGridAfterDateSelected"] = this.refreshGridAfterDateSelected;
        // this.headerData = HeaderData(functionRefs);
        this.headerData = HeaderData();

        //bindAction(HeaderData, "effectiveFrom", (data,e,type) => this.test(data,e,'effectiveFrom'));
    }

    // refreshGridAfterDateSelected=(inputValue)=>{ 
        // const focusedCell = this.child.gridApi.getFocusedCell();  
        // this.child.gridApi.clearFocusedCell();  
        //  if(!isEmptyOrUndefine(inputValue))
        //     this.child.gridApi.setFocusedCell(focusedCell.rowIndex, focusedCell.column.colId,false); 
    // }
    reloadLineItemsOnSave = () => {
        //this.props.actions.UpdateSelectedSchedule(0); //Changes for D1039
        this.setState((state) => { //Changes for D-1039
            return {
                selectedChargeScheduleDetails: this.props.rateSchedule
            };
        } ,()=>{
            if (this.gridChargeSchedule) {
                if (this.state.selectedChargeScheduleDetails) {
                    this.setState({ isLoadOnSave: true });
                    //const chargeSchedule = arrayUtil.sort(this.state.selectedChargeScheduleDetails, "scheduleName", 'asc');
                    //     chargeSchedule.forEach((itratedValue) => {
                    //         itratedValue.isSelected = false;
                    //     });
                    //     chargeSchedule[0].isSelected = true; //Changes for D1039
                    const chargeSchedule = this.state.selectedChargeScheduleDetails;
                    chargeSchedule.forEach((itratedValue,index)=>{
                        if (index===this.props.selectedRowIndex && !this.state.isCopyScheduleModalOpen && !this.state.isDeleteScheduleModalOpen) {
                            itratedValue.isSelected = true;  //Changes for D1039
                        }
                        else{
                            itratedValue.isSelected= false;
                        } 
                        itratedValue.readonly = (this.props.interactionMode || this.props.pageMode=== localConstant.commonConstants.VIEW || this.props.isScheduleBatchProcess || (this.props.contractInfo.contractType === "IRF" && itratedValue.baseScheduleId > 0) ? true : false); //Changes for D-789 for IRF Disable Functionality
                    });
                    this.gridChargeSchedule.gridApi.setRowData(chargeSchedule);
                    // }
                }
            }
       });
    }

    clearLineItemsOnCancel = () => {   
        this.props.actions.UpdateSelectedSchedule(0);     
        if(this.props.currentPage === "Create Contract") {
            this.setState({
                selectedChargeScheduleDetails: [],
                selectedChargeRateDetails: []
            });
        } else {
            if(this.gridChargeSchedule) {
                this.setState({ isLoadOnCancel : true });
                let chargeScheduleData = [];
                if(this.props.rateScheduleOnCancel && this.props.rateScheduleOnCancel.length > 0) {
                    chargeScheduleData = this.props.rateScheduleOnCancel;
                    chargeScheduleData.forEach((itratedValue) => {
                        itratedValue.isSelected = false;
                        itratedValue.recordStatus = null;   //Added for D-1217
                    });
                    chargeScheduleData[0].isSelected = true;
                    sessionStorage.removeItem('selectedSchedule'); //Added for D789
                    this.setState({ 
                        selectedChargeScheduleDetails: chargeScheduleData,
                        SelectedRateSchedule: chargeScheduleData[0],
                        selectedChargeRateDetails: [],
                    });
                } else {
                    this.setState({
                        selectedChargeScheduleDetails: [],
                        selectedChargeRateDetails: [],
                        SelectedRateSchedule: {}
                    });
                }
                this.gridChargeSchedule.gridApi.setRowData(this.state.selectedChargeScheduleDetails);
            }
        }          
    }
    chargeRatesOnChangeHandler =(data,e,ratesType)=>{
        if (data && data.id !== -1) {
            if(e.target.checked){
                this.props.actions.AdminRateScheduleSelect(data,ratesType);
            }
            else{
                this.props.actions.AdminRateScheduleSelect(data,"");
            }
            const adminChargeRates = this.props.adminChargeRatesValue;
            const selectedData = adminChargeRates.filter(value => value.id === data.id);
            const updatedRates = {
                "isRateOnShoreOil": (ratesType==='rateOnShoreOil'?e.target.checked:false),
                "isRateOnShoreNonOil": (ratesType==='rateOnShoreNonOil'?e.target.checked:false),
                "isRateOffShoreOil": (ratesType==='rateOffShoreOil'?e.target.checked:false)
            };
            const updatedAdminChargeRate = Object.assign({}, selectedData[0], updatedRates);
            this.props.actions.UpdateChargeRatesTypes(updatedAdminChargeRate).then(res => {
                if (res) {
                    this.secondChild.refreshGrid();
                }
            });
        }
        else {
            if(e.target.checked){
                this.props.actions.AdminRateScheduleSelectAll(e.target.checked,ratesType);
            }
            else{
                this.props.actions.AdminRateScheduleSelectAll(e.target.checked,"");
            }
            this.props.actions.UpdateAllChargeRates(ratesType, e.target.checked).then(res => {
                if (res) {
                    this.secondChild.refreshGrid();
                }
            });  
        }
    }
    
    componentDidMount() {
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        if (tabInfo && tabInfo.componentRenderCount === 0) {
            this.props.actions.FetchExpenseType();
            this.props.actions.FetchAdminSchedule(true); //Changes for Defect 112
            this.props.actions.AdminRateScheduleSelect();
        }
        //  this.props.actions.ClearAdminChargeRates();
        this.updateOrderSequenceId();
        this.bindChargeScheduleDetail();
        const contractType = this.props.contractInfo.contractType;
        if(contractType == 'FRW'){
            document.addEventListener("visibilitychange", this.onWindowFocus);
        }  
        // this.bindChargeRateDetailGrid();
    }

    onWindowFocus = () => {
        const link = document.querySelector("link[rel*='icon']") || document.createElement('link');
        link.type = 'image/x-icon';
        link.rel = 'shortcut icon';
        link.href = '/favicon.ico';
        if (!document.hidden) {
            document.getElementsByTagName('head')[0].appendChild(link);    
        }
    }

    updateOrderSequenceId() {
        let orderSequenceId = 0;        
        this.props.rateSchedule && this.props.rateSchedule.forEach(item => {
            if(item.sequenceId && item.sequenceId > orderSequenceId) {
                orderSequenceId = item.sequenceId;
            }
        });
        if(orderSequenceId > 0) {
            this.setState({ orderSequenceId: orderSequenceId });
        }
    }

    componentWillReceiveProps(nextProps) {
        // if(!nextProps.isAdminContractRatesModalOpen){
            if (nextProps.isChargeTypeEdit) {
                if (nextProps.chargeTypeEditData.effectiveTo !== null && nextProps.chargeTypeEditData.effectiveTo !== "") {
                    this.setState({
                        effectiveToDate: moment(nextProps.chargeTypeEditData.effectiveTo),
                        effectiveFromDate: moment(nextProps.chargeTypeEditData.effectiveFrom)
                    });
                }
                else {
                    this.setState({
                        effectiveToDate: "",
                        effectiveFromDate:moment(nextProps.chargeTypeEditData.effectiveFrom)
                    });
                }
            } else {
                this.setState({
                    effectiveFromDate: '',
                    effectiveToDate: ""
                });
            }
        //}
    }

    /**
     * Validation For Rate Schedule.
     */
    rateScheduleValidation = (data) => {
        if (data.scheduleName == undefined || data.scheduleName == "") {
            IntertekToaster(localConstant.contract.common.REQUIRED_TEXT + ' Schedule Name','warningToast rsScheduleNameReq');
            return false;
        }
        if (data.chargeCurrency === undefined || data.chargeCurrency === 'select' || data.chargeCurrency === 'Select' || data.chargeCurrency === "") {
            IntertekToaster(localConstant.contract.common.REQUIRED_SELECT + ' Charge Currency','warningToast rsCurrencyReq');
            return false;
        }
        if (data.scheduleName && this.isRateScheduleDuplicate(data)) {
            IntertekToaster(localConstant.contract.rateSchedule.RATE_SCHEDULE_DUPLICATE,'warningToast rsRateScheduleDuplicate');
            return false;
        }
        return true;
    }

    /**
     * Validation for adding admin contract rates
     */
    adminContractRatesValidation = (value) => {
        const data=Object.assign({},this.props.adminChargeScheduleValueChange,value);
        if (data.companyChargeScheduleName === undefined || data.companyChargeScheduleName === 'select' || data.companyChargeScheduleName === 'Select' || data.companyChargeScheduleName === "") {
            IntertekToaster(localConstant.contract.common.REQUIRED_SELECT + ' Schedule Name','warningToast adminRateScheduleToaster');
            return false;
        }
        if (data.companyChgSchInspectionGroupName === undefined || data.companyChgSchInspectionGroupName === 'select' || data.companyChgSchInspectionGroupName === 'Select' || data.companyChgSchInspectionGroupName === "") {
            IntertekToaster(localConstant.contract.common.REQUIRED_SELECT + ' Inspection Group','warningToast rsInspecitonGroupReq');
            return false;
        }
        if (data.companyChgSchInspGrpInspectionTypeName === undefined || data.companyChgSchInspGrpInspectionTypeName === 'select' || data.companyChgSchInspGrpInspectionTypeName === 'Select' || data.companyChgSchInspGrpInspectionTypeName === "") {
            IntertekToaster(localConstant.contract.common.REQUIRED_SELECT + ' Inspection Type','warningToast rsInspectionTypeReq');
            return false;
        }
        const isValidDates = this.chargeTypeDateValidation(this.state.adminEffectiveFromDate,this.state.adminEffectiveToDate);
        // if (isEmpty(this.state.effectiveFromDate)) {
        //     IntertekToaster(localConstant.contract.common.REQUIRED_SELECT + 'Effective From Date','warningToast rsEffectiveFromDateReq');
        //     return false;
        // }
        // if (!isEmpty(this.state.effectiveFromDate) && !this.state.inValidDateFormat){
        //     if(!isEmpty(this.props.contractInfo.contractStartDate)){
        //         if(moment(this.props.contractInfo.contractStartDate).isAfter(this.state.effectiveFromDate)){
        //             IntertekToaster(localConstant.contract.CHARGE_RATE_DATE_GREATER_THAN_CONTRACT_DATE,'warningToast scheduleContractDateCompare');
        //             return false;
        //         }
        //     }
        // }
        if(!isValidDates){
            return false;
        }
        return true;
    }

    /** ChargeType Date Validation */
    chargeTypeDateValidation = (fromDate,toDate) => {
        const { contractStartDate,contractEndDate } = this.props.contractInfo;
        if(isEmpty(contractStartDate)){
            IntertekToaster(localConstant.contract.CONTRACT_START_DATE_VALIDATION,'warningToast scheduleContractDateCompare');
            return false;
        }
        const isValidContractStartDate = dateUtil.isUIValidDate(moment(contractStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT));
        if(!isValidContractStartDate){
            IntertekToaster(localConstant.contract.CONTRACT_START_DATE_FORMAT_VALIDATION,'warningToast scheduleContractDateCompare');
            return false;
        }
        if(isEmpty(fromDate)){
            IntertekToaster(localConstant.contract.common.REQUIRED_SELECT + 'Effective From Date','warningToast rsFromDate');
            return false;
        }
        const fromDateValid = dateUtil.isUIValidDate(moment(fromDate).format(localConstant.commonConstants.UI_DATE_FORMAT));
        if(!fromDateValid){
            IntertekToaster(localConstant.validationMessage.EFFECTIVE_FROM_VALIDATION,'warningToast scheduleContractDateCompare');
            return false;
        }
        if(contractStartDate && isEmpty(contractEndDate)){
            if(moment(contractStartDate).isAfter(fromDate,'day')){
                IntertekToaster(localConstant.contract.CHARGE_RATE_DATE_GREATER_THAN_CONTRACT_DATE,'warningToast scheduleContractDateCompare');
                return false;
            }
        }
      
            if(!isEmpty(contractStartDate)){
                if(moment(fromDate).isBefore(contractStartDate)){
                    IntertekToaster(localConstant.validationMessage.EFFECTIVE_FROM_BEFORE_VALIDATION,'warningToast scheduleContractDateCompare');
                    return false;
                }
            }  
        if(toDate){
            const toDateValid = dateUtil.isUIValidDate(moment(toDate).format(localConstant.commonConstants.UI_DATE_FORMAT));
            if(!toDateValid){
                IntertekToaster(localConstant.validationMessage.EFFECTIVE_TO_VALIDATION,'warningToast scheduleContractDateCompare');
                return false;
            }
            if(this.dateRangeValidator(fromDate.format(),toDate.format())){
                return false;
            }
        }
        if(contractEndDate){
            const contractEndDateValid = dateUtil.isUIValidDate(moment(contractEndDate).format(localConstant.commonConstants.UI_DATE_FORMAT));
            if(!contractEndDateValid){
                IntertekToaster(localConstant.contract.CONTRACT_END_DATE_FORMAT_VALIDATION,'warningToast scheduleContractDateCompare');
                return false;
            }
        }
        return true;
    }

    /**
     * Validation for Charge Type.
     */
    chargeTypeValidation = (data) => {
     
        const { contractStartDate,contractEndDate } = this.props.contractInfo;
        if(isEmpty(contractStartDate)){
            IntertekToaster(localConstant.contract.CONTRACT_START_DATE_VALIDATION,'warningToast scheduleContractDateCompare');
            return false;
        }
        if(isEmpty(data.chargeType)){
            IntertekToaster(localConstant.contract.common.REQUIRED_SELECT + ' Charge Type','warningToast rsChargeTypeReq');
            return false;
        }
            if(isEmpty(data.chargeValue.toString())){
            IntertekToaster(localConstant.validationMessage.CHARGE_VALUE_VALIDATION,'warningToast rsChargeVlaueFormat');
            return false;
        }
        const dateVal = this.chargeTypeDateValidation(this.state.effectiveFromDate,this.state.effectiveToDate);
        if(!dateVal){
            return false;
        }
        return true;
    }

    isRateScheduleDuplicate = (data) => {
        let isRateScheduleDuplicate = false;
        this.props.rateSchedule && this.props.rateSchedule.map(iteratedValue => {
            if (this.props.isRateScheduleEdit) {
                if (iteratedValue.scheduleName.toUpperCase() === data.scheduleName.toUpperCase() && iteratedValue.scheduleId != data.scheduleId) {
                    isRateScheduleDuplicate = true;
                }
            }
            else {
                if (iteratedValue.scheduleName.toUpperCase() === data.scheduleName.toUpperCase()) {
                    isRateScheduleDuplicate = true;
                }
            }
        });
        return isRateScheduleDuplicate;
    }

    /**
     * Input Change Handler
     */
    inputChangeHandler = (e) => {
        if(e.target.type === "number"){
            if(e.target.value != "0"){
                e.target.value=numberFormat(e.target.value);
            }
        }
        const fieldValue = e.target[e.target.type === "checkbox" ? "checked" : "value"];
        const fieldName = e.target.name;
        const result = { value: fieldValue, name: fieldName };
        return result;
    }
       /** Handler to make the entered decimal data as two digit */
    checkNumber = (e) => {
      
        if(e.target.value ==="."){
            e.target.value="0";
        }
        if(!isEmpty(e.target.value) && e.target.value > 0){
        e.target.value = parseFloat(numberFormat(e.target.value)).toFixed(4);
        this.formInputChangeHandler(e);
        }
        else{
            e.target.value = "0.0000";
            this.formInputChangeHandler(e);
        }
    }

    decimalWithLimtFormat = (evt) => {  
        const e = evt || window.event;   
        const expression = ("(\\d{0,"+ parseInt(16)+ "})[^.]*((?:\\.\\d{0,"+ parseInt(4) +"})?)");
        const rg = new RegExp(expression,"g"); 
        const match = rg.exec(e.target.value.replace(/[^\d.]/g, ''));
        e.target.value = match[1] + match[2]; 

        if (e.preventDefault) e.preventDefault();
    }; 

    blurValue = (e) => {
      
        if (e.target.value > 9999999999999) {
        }
        else {
            e.target.value = numberFormat(e.target.value);
           
        }
      
    }
    /**
     * Form Input data Change Handler
     */
    formInputChangeHandler = (e) => {
        const result = this.inputChangeHandler(e);
        this.updatedData[result.name] = result.value;
    }

     /** Admin charge rate input change handler */
     adminChargeRateInputChangeHandler = (e) => {
        const result = this.inputChangeHandler(e);
        this.adminUpdatedData[result.name] = result.value;
    }

    /**
     * Rate Schedule Onchange Handler
     */
    RateScheduleOnchangeHandler = (e) => {
        this.setState({ selectedScheduleName: e.target.value });
        if (e.target.value === 'select' || e.target.value === 'Select' || e.target.value === "") {
            this.setState({
                selectedScheduleName: '',
                SelectedRateSchedule: {},
            });
        }
        else {
            const currentRateSchedule = this.props.rateSchedule && this.props.rateSchedule.filter(iteratedValue => {
                if (iteratedValue.scheduleName === e.target.value) {
                    return iteratedValue;
                }
            });
            this.setState({ SelectedRateSchedule: currentRateSchedule[0] });
        }
    }

    clearFilters =()=>{
        const scheduleIndex=this.props.rateSchedule && this.filteredSchedule.scheduleId && this.props.rateSchedule.findIndex(x=>x.scheduleId ===this.filteredSchedule.scheduleId);
        this.props.actions.UpdateSelectedSchedule(scheduleIndex);
        this.state.selectedChargeScheduleDetails.map((e,index)=> {
            if(scheduleIndex === index)
                e.isSelected=true;
            else
                e.isSelected=false;
        });
        if(this.gridChargeSchedule && this.gridChargeSchedule.gridApi){
            this.gridChargeSchedule.gridApi.setRowData(this.state.selectedChargeScheduleDetails); 
        }
    }
    filterGrid =(e,data)=>{
        this.filteredRowCount =e;
        this.filteredSchedule=data[0];
    }
    RateScheduleOnSelectHandler = (e) => {
        if(e.node.selected){
            const currentRateSchedule = this.props.rateSchedule && this.props.rateSchedule.filter(iteratedValue => {
                if (iteratedValue.scheduleName === e.node.data.scheduleName && iteratedValue.scheduleId === e.node.data.scheduleId) { //Changes for D789
                    return iteratedValue;
                }
            });
            this.setState({ SelectedRateSchedule: currentRateSchedule[0] });
            sessionStorage.setItem('selectedSchedule', JSON.stringify(currentRateSchedule[0])); //Added for D789
            this.setState({ selectedScheduleName: e.node.data.scheduleName });    
            this.bindChargeRateDetailGrid({ movetofirstpage : true });  //D-1267 Fix

            //Clear all adminContractRates
            this.props.actions.ClearAdminContractRates();
            this.props.actions.AdminContractRatesModalState(false);
            // this.ClearAdminContractRatesScheduleChange();
        } 
        else if(this.state.selectedScheduleName === e.node.data.scheduleName ){ // ITK Def Id : 443
            this.setState({ SelectedRateSchedule: {} });
            this.setState({ selectedScheduleName: "" });
        }   
    }

    /**
     * Rate Schedule Onclick Handler
     */
    RateScheduleCreateHandler = () => {
        this.props.actions.RateScheduleEditCheck(false);
        this.props.actions.RateScheduleModalState(true);
    }

    /**
     * Add New Rate Schedule Function
     */
    createRateSchedule = (e) => {
        e.preventDefault();
        if (this.rateScheduleValidation(this.updatedData)) {
            if (this.updatedData && !isEmpty(this.updatedData)) {
                this.updatedData["recordStatus"] = "N";
                if(!isEmpty(this.props.selectedContract.contractNumber)){
                    this.updatedData["contractNumber"]=this.props.selectedContract.contractNumber;
                }
                this.updatedData["scheduleId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                this.updatedData["baseScheduleName"] = null;
                this.updatedData["createdBy"] = this.props.loggedInUser;
                if(this.state.orderSequenceId >= 1 ){
                    this.updatedData["sequenceId"] = this.state.orderSequenceId + 1;
                    this.setState({
                        orderSequenceId:this.updatedData["sequenceId"]
                    });
                }
                this.props.actions.AddRateSchedule(this.updatedData);
                this.setState({ 
                    SelectedRateSchedule: this.updatedData,
                    //  selectedScheduleName: this.updatedData.scheduleName
                     });
                this.cancelRateSchedule(e);
            }
        }
    }

    /**
     * Cancel Add New Rate Schedule
     */
    cancelRateSchedule = (e) => {
        e.preventDefault();
        this.props.actions.RateScheduleModalState(false);
        this.updatedData = {};
    }

    /**
     * Rate Schedule Edit Onclick Handler
     */
    RateScheduleEditHandler = (data) => {
        // TODO: Store the selected rate schedule object in the store and trigger the modal
        const currentRateSchedule = this.props.rateSchedule && this.props.rateSchedule.filter(iteratedValue => {
            if (iteratedValue.scheduleName == this.state.selectedScheduleName) {
                return iteratedValue;
            }
        });
        /*AddScheduletoRF*/
        this.props.actions.EditRateSchedule(data);
        this.rateScheduleEditedRow=data;
        this.props.actions.RateScheduleEditCheck(true);
        this.props.actions.RateScheduleModalState(true);
    }

    /**
     * Update Rate Schedule with edited data in store
     */
    updateRateSchedule = (e) => {
        e.preventDefault();
        let scheduleNameUpdated = false;
        /*AddScheduletoRF*/
        this.updatedData["oldScheduleName"] = this.rateScheduleEditedRow.scheduleName;

        if (this.updatedData) {
            if(this.updatedData.scheduleName){
                scheduleNameUpdated = true;
            }
            /*AddScheduletoRF*/
            let editedRow = Object.assign({}, this.rateScheduleEditedRow, this.updatedData);
            if (this.rateScheduleValidation(editedRow)) {
                if (this.props.rateScheduleEditData.recordStatus !== "N")
                    this.updatedData["recordStatus"] = "M";
                   
                this.updatedData["modifiedBy"] = this.props.loggedInUser;   
                editedRow = Object.assign({},editedRow,this.updatedData);
                this.props.actions.UpdateRateSchedule(editedRow);
                this.setState({ SelectedRateSchedule: editedRow , selectedScheduleName: editedRow.scheduleName });
                this.cancelRateSchedule(e);
                this.rateScheduleEditedRow={};
            }
        }
    }

    /**
     * Rate Schedule delete handler - will trigger the confirmation and check if charge type exists.
     */
    RateScheduleDeleteHandler = (e) => {
        e.preventDefault();
        //let isChargeTypeExist = false;
        let isScheduleAssociated = '';
        //let isBaseScheduleExist = false;
        const selectedSchedules=this.deleteScheduleChild.getSelectedRows();

        //Commented this because this code is not using anywhere
        // this.props.chargeTypes && this.props.chargeTypes.forEach(iteratedValue => {
        //     selectedSchedules.forEach(selectedvalue =>{
        //         if (iteratedValue.recordStatus !== 'D' && iteratedValue.scheduleName == selectedvalue.scheduleName) {
        //             isChargeTypeExist = true;
        //         }
        //     });
        // });        
       /* This validation is commented as it is not present in Evo 1*/
        // this.props.rateSchedule && this.props.rateSchedule.forEach(iteratedValue => {
        //     selectedSchedules.forEach(selectedvalue =>{
        //     if (iteratedValue.recordStatus !== 'D' && selectedvalue.baseScheduleName && iteratedValue.baseScheduleName == selectedvalue.baseScheduleName) {
        //         isBaseScheduleExist = true;
        //     }
        // });
        // });

        // if (isChargeTypeExist) {
        //     IntertekToaster(localConstant.contract.rateSchedule.SELECTED_RATESCHEDULE_ASSOCIATED_WARNING,'warningToast rsRateScheduleAssociated');
        //     return false;
        // }
        /* This validation is commented as it is not present in Evo 1*/
        // if (isBaseScheduleExist) {
        //     IntertekToaster(localConstant.contract.rateSchedule.RATE_SCHEDULE_HAS_COPY_SCHEDULE,'warningToast rsRateScheduleHasCopy');
        //     return false;
        // }

        selectedSchedules.forEach(selectedvalue => {            
            if(!selectedvalue.canBeDeleted) {
                isScheduleAssociated = (isEmpty(isScheduleAssociated) ? selectedvalue.scheduleName : (isScheduleAssociated + ', ' + selectedvalue.scheduleName));
            } 
        });

        if(!isEmpty(isScheduleAssociated)) {
            IntertekToaster(isScheduleAssociated + ' ' + localConstant.contract.rateSchedule.SCHEDULE_CANNOT_BE_DELETED, 'warningToast');
            return false;
        }
        if (selectedSchedules.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.contract.rateSchedule.RATE_SCHEDULE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteRateSchedule,
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
        } else {
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE, 'warningToast');
        }
    }

    /**
     * delete selected rate schedule from the store
     */
    /*Added for defect Id-286 raised on 26-08-19 */
    deleteRateSchedule = () => {
        const selectedSchedules = this.deleteScheduleChild.getSelectedRows();
        const chargeRates = isEmptyReturnDefault(this.props.chargeTypes);
        const selectedRatesToBeDeleted = [];
        // let isSavedSchedule=false;
        if (chargeRates != null && selectedSchedules != null) {
            // selectedSchedules.forEach(iteratedScheduleValue => {
            // });
            for (let i = 0; i < selectedSchedules.length; i++) {
                const iteratedScheduleValue = selectedSchedules[i];
                chargeRates.forEach(iteratedRateValue => {
                    if (iteratedScheduleValue.scheduleName === iteratedRateValue.scheduleName && iteratedRateValue.recordStatus !== "D" && iteratedRateValue.scheduleId === iteratedScheduleValue.scheduleId) {
                        selectedRatesToBeDeleted.push(iteratedRateValue);
                    }
                });
                // if (iteratedScheduleValue.recordStatus !== 'N' && selectedRatesToBeDeleted.length > 0) {
                //     isSavedSchedule = true;
                //     break;
                // }
            }
        }
            this.setState({ selectedScheduleName: "" });
            this.props.actions.DeleteRateSchedule(selectedSchedules);
            this.props.actions.DeleteChargeType(selectedRatesToBeDeleted);
            this.setState({ SelectedRateSchedule: {} });
            const newState = this.state.selectedChargeScheduleDetails;
            selectedSchedules.forEach(item => {
                newState.forEach((iteratedValue, index) => {
                    if (iteratedValue.scheduleName === item.scheduleName && iteratedValue.scheduleId === item.scheduleId) {
                        // newState.splice(index, 1);
                        iteratedValue.recordStatus="D";
                    }
                });

            });
        newState.forEach((iteratedValue, index) => {
            if (!required(iteratedValue.scheduleName)) {
                newState[index]["scheduleNameValidation"] = newState.filter(x => x.scheduleId !== iteratedValue.scheduleId && x.recordStatus != 'D' && x.scheduleName.toLowerCase() === iteratedValue.scheduleName.toLowerCase()).length > 0 ?
                    localConstant.contract.rateSchedule.RATE_SCHEDULE_DUPLICATE : "";
            }
        });
            if (newState && newState.length > 0) {
                newState[0].isSelected = true;
                this.setState({ selectedChargeScheduleDetails: newState });
            } else {
                this.setState({ selectedChargeScheduleDetails: [] });
                this.setState({ selectedChargeRateDetails: [] });
            }

            this.setState((state) => ({
                selectedChargeScheduleDetails: state.selectedChargeScheduleDetails.filter(item => {
                    return !selectedSchedules.some(ts => {
                        return ts["scheduleId"] === item["scheduleId"];
                    });
                }),
            }), () => {
                this.gridChargeSchedule.gridApi.setRowData(this.state.selectedChargeScheduleDetails);
            });
            this.props.actions.UpdateSelectedSchedule(0);
            this.setState({ isDeleteScheduleModalOpen: false });
        this.props.actions.HideModal();
    }

    /**
     * Charge Rate Onclick Handler
     */
    chargeRateCreateHandler = () => {        
        this.props.actions.ChargeTypeEditCheck(false);
        this.setState({ effectiveFromDate : '' });
        this.props.actions.ChargeTypeModalState(true);

    }

    /**
     * Add New Charge Type Function
     */
    createChargeType = (e) => {
        e.preventDefault();
        if(this.updatedData.chargeValue === undefined){
            this.updatedData.chargeValue = 0;
        }
        let chargeRateOverlaps = false;
        if (this.chargeTypeValidation(this.updatedData)) {
        if (this.updatedData && !isEmpty(this.updatedData)) {        

                const fromDate = this.state.effectiveFromDate.format();
                let toDate = "";
                if (this.state.effectiveToDate !== "" && this.state.effectiveToDate !== null) {
                    toDate = this.state.effectiveToDate.format();
                }
                // if (this.state.inValidDateFormat) {
                //     IntertekToaster(localConstant.contract.common.VALID_DATE_WARNING,'warningToast rsValidDateWarn');
                //     return false;
                // } 
                // if (!this.dateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
                    let chargeRateType = null;
                    this.props.contractChargeTypes.forEach(iteratedValue => {
                        if(iteratedValue.name === this.updatedData.chargeType){
                            chargeRateType = iteratedValue.chargeType;
                        }
                    });
                    this.updatedData["chargeRateType"] = chargeRateType;
                    this.updatedData["rateId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                    this.updatedData["effectiveFrom"] = fromDate;
                    this.updatedData["effectiveTo"] = toDate;
                    this.updatedData["scheduleName"] = this.state.selectedScheduleName;
                    if(!isEmpty(this.props.selectedContract.contractNumber)){
                        this.updatedData["contractNumber"]=this.props.selectedContract.contractNumber;
                    }
                    this.updatedData["standardValue"] = 0.00;
                    this.updatedData["percentage"] = 0.00;
                    const discountApplied = (parseFloat(this.updatedData.chargeValue) - parseFloat(this.updatedData.standardValue));
                    this.updatedData["discountApplied"] = parseFloat(discountApplied);
                    if (this.updatedData.standardValue && (parseFloat(this.updatedData.standardValue) > 0)) {
                        const percentage = (discountApplied / parseFloat(editedRow.standardValue)) * 100;
                        this.updatedData["percentage"] = parseFloat(percentage);
                    }
                    if(isEmpty(this.updatedData.chargeValue))
                    {
                        this.updatedData["chargeValue"] = 0.00;
                    }
                    this.updatedData["chargeValue"]=parseFloat(this.updatedData.chargeValue);
                    // this.updatedData["discountApplied"] = 0.00;
                    this.updatedData["createdBy"] = this.props.loggedInUser;
                    this.updatedData["recordStatus"] = "N";
                    if(this.state.orderSequenceId >= 1 ){
                        this.updatedData["sequenceId"] = this.state.orderSequenceId + 1;
                        this.setState({
                            orderSequenceId:this.updatedData["sequenceId"]
                        });
                    }
                    if(this.props.contractRates)    { 
                        const contractRates=this.props.contractRates;  
                        for (let i = 0; i < contractRates.length; i++) { 
                            //  for( const key in this.updatedData)
                            //  {

                                const chargeTypeElement = this.updatedData["chargeType"];  
                                const chargeValueElement=this.updatedData["chargeValue"];  
                                const chargeEffectiveFromDate=this.updatedData["effectiveFrom"]; 
                                const chargeEffectiveToDate=this.updatedData["effectiveTo"];        
                                                      
                                if(contractRates[i].chargeType===chargeTypeElement && contractRates[i].scheduleName===this.state.selectedScheduleName)
                                { 
                                    /**
                                     * DEF ID 94 And 592 Fixes
                                     */
                                    if(!isEmpty(chargeEffectiveFromDate) && !isEmpty(chargeEffectiveToDate) && !isEmpty(contractRates[i].effectiveTo) )
                                    {
                                        if(moment(chargeEffectiveFromDate).isSameOrAfter(contractRates[i].effectiveFrom) && moment(contractRates[i].effectiveTo).isSameOrAfter(chargeEffectiveToDate))
                                        {
                                         chargeRateOverlaps = true;
                                         this.isChargeRateOverlaps=true ;
                                         break;                        
                                        }
                                    }
                                    else if(!isEmpty(chargeEffectiveFromDate) && ( isEmpty(chargeEffectiveToDate) || isEmpty(contractRates[i].effectiveTo)))
                                    {
                                        if(moment(chargeEffectiveFromDate).isSameOrAfter(contractRates[i].effectiveFrom))
                                        {
                                         chargeRateOverlaps = true;
                                         this.isChargeRateOverlaps=true ;
                                         break;                        
                                        }
                                    } 
                                } 
                             if (chargeRateOverlaps) {
                                break;
                              }

                        }

                    }
                    if (chargeRateOverlaps) {
                      
                        const confirmationObject = {
                          title: modalTitleConstant.ALERT_WARNING,
                          message: modalMessageConstant.contract.CHARGE_RATES_OVERLAP,
                          modalClassName: "warningToast",
                          type: "confirm",
                     
                          buttons: [
                            {
                              buttonName: "OK",
                              className: "modal-close m-1 btn-small",  
                              onClickHandler: this.saveChargeType
                            }
                          ]
                        };
                        this.props.actions.DisplayModal(confirmationObject); 
                      }
                      else {
                        this.props.actions.AddChargeType(this.updatedData);
                        this.cancelChargeType();
                      }				  
                //}
            }
        }
    }
    updateChargeTypeData = () => { 
        if (this.updatedData && !isEmpty(this.updatedData) && !this.isChargeRateOverlaps) {
            this.updatedData["modifiedBy"] = this.props.loggedInUser;    
            this.props.actions.UpdateChargeType(this.updatedData);
            this.cancelChargeType();
        }    
        this.props.actions.HideModal();
  }
    saveChargeType = () => { 
        // if(!this.isChargeRateOverlaps) // D592 - Fixes
        // {
            this.props.actions.AddChargeType(this.updatedData); 
            this.cancelChargeType();
        // }   
        this.props.actions.HideModal();
  }

  /**
     * Cancel Add New Charge Type
     */
    cancelChargeType = () => {
        this.props.actions.ChargeTypeModalState(false);
        this.setState({
            effectiveFromDate: '',
            effectiveToDate: ""
        });
       this.updatedData = {};
    }

    /**
     * Charge Type Update Function
     */
    updateChargeType = (e) => {
        e.preventDefault();
        const editedRow = Object.assign({}, this.props.chargeTypeEditData, this.updatedData);

        if (!isEmpty(this.updatedData) && this.chargeTypeValidation(editedRow)) {
            const fromDate = this.state.effectiveFromDate.format();
            let toDate = "";
            if (this.state.effectiveToDate !== "" && this.state.effectiveToDate !== null) {
                toDate = this.state.effectiveToDate.format();
            }
            let chargeRateType = null;
            this.props.contractChargeTypes.forEach(iteratedValue => {
                if (iteratedValue.name === editedRow.chargeType) {
                    chargeRateType = iteratedValue.chargeType;
                }
            });
            this.updatedData["chargeRateType"] = chargeRateType;
            this.updatedData["effectiveFrom"] = fromDate;
            this.updatedData["effectiveTo"] = toDate;
            const discountApplied = (parseFloat(editedRow.chargeValue) - parseFloat(editedRow.standardValue));
            this.updatedData["discountApplied"] = parseFloat(discountApplied);
            if (editedRow.standardValue && (parseFloat(editedRow.standardValue) > 0)) {
                const percentage = (discountApplied / parseFloat(editedRow.standardValue)) * 100;
                this.updatedData["percentage"] = parseFloat(percentage);
            }
            if (this.props.chargeTypeEditData.recordStatus !== "N") {
                this.updatedData["recordStatus"] = "M";
            }
            if (!isEmpty(this.updatedData.chargeValue)) {
                this.updatedData["chargeValue"] = parseFloat(this.updatedData.chargeValue);
            }
            if (!isEmpty(this.updatedData["effectiveToDate"]) || !isEmpty(this.updatedData["effectiveFromDate"])) {
                this.overlapCheckHandler(editedRow);
            }
            else {
                this.updatedData["modifiedBy"] = this.props.loggedInUser;
                this.props.actions.UpdateChargeType(this.updatedData);
                this.cancelChargeType();
            }
        }
        else {
            this.cancelChargeType();
        }
    }
    overlapCheckHandler = (editedRow) => {
        let chargeRateOverlaps = false;
        if(this.props.contractRates)    { 
            const contractRates=this.props.contractRates;  
            for (let i = 0; i < contractRates.length; i++) {
                const chargeTypeElement = editedRow.chargeType;
                const chargeValueElement = this.updatedData["chargeValue"];
                const chargeEffectiveFromDate = this.updatedData["effectiveFrom"];
                const chargeEffectiveToDate = this.updatedData["effectiveTo"];

                if (contractRates[i].chargeType === chargeTypeElement  && contractRates[i].scheduleName===this.state.selectedScheduleName && contractRates[i].rateId!=editedRow.rateId) {
                   /** 
                    *  DEF ID 94 And 592 Fixes
                    */
                    if (!isEmpty(chargeEffectiveFromDate) && !isEmpty(chargeEffectiveToDate) && !isEmpty(contractRates[i].effectiveTo)) {
                        if (moment(chargeEffectiveFromDate).isSameOrAfter(contractRates[i].effectiveFrom) && moment(contractRates[i].effectiveTo).isSameOrAfter(chargeEffectiveToDate)) {
                            chargeRateOverlaps = true;
                            this.isChargeRateOverlaps = true;
                            break;
                        }
                    }
                    else if (!isEmpty(chargeEffectiveFromDate) && (isEmpty(chargeEffectiveToDate) || isEmpty(contractRates[i].effectiveTo))) {
                        if (moment(chargeEffectiveFromDate).isSameOrAfter(contractRates[i].effectiveFrom)) {
                            chargeRateOverlaps = true;
                            this.isChargeRateOverlaps = true;
                            break;
                        }
                    } 
                }
                if (chargeRateOverlaps) {
                    break;
                }
            }
        }
        if (chargeRateOverlaps) {
            const confirmationObject = {
                title: modalTitleConstant.ALERT_WARNING,
                message: modalMessageConstant.contract.CHARGE_RATES_OVERLAP,
                modalClassName: "warningToast",
                type: "confirm",

                buttons: [
                    {
                        buttonName: "OK",
                        className: "modal-close m-1 btn-small",
                        onClickHandler: this.updateChargeTypeData
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
        else {
            if (this.updatedData && !isEmpty(this.updatedData)) {
                this.updatedData["modifiedBy"] = this.props.loggedInUser;
                this.props.actions.UpdateChargeType(this.updatedData);
                this.cancelChargeType();
            }
        }
    }
    
    /**
     * Delete charge rate handler - get the selected row from grid and throw confirmation to delete.
     */
    chargeRateDeleteHandler = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.contract.rateSchedule.CHARGE_TYPE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteChargeType,
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
            IntertekToaster(localConstant.commonConstants.SELECT_RECORD_TO_DELETE,'warningToast scheduleRatesDeleteToaster');
        }
    }

    /**
     * Delete charge type which is selected in the grid from store.
     */
    deleteChargeType = () => {
        const selectedRecords = this.child.getSelectedRows();
        const chargeType = [];
        this.props.chargeTypes && this.props.chargeTypes.map(iteratedValue => {
            if (this.state.SelectedRateSchedule && iteratedValue.scheduleName === this.state.SelectedRateSchedule.scheduleName && iteratedValue.recordStatus !== "D") {
                chargeType.push(iteratedValue);
            }
        });    

        //Commented this condition because this validation is already on save. Client does not want this validation
        // if(selectedRecords.length === chargeType.length){
        //     IntertekToaster(localConstant.contract.SCHEDULE_HAS_RATES_VALIDATION, 'warningToast ScheduleRateWarn');
        //     this.props.actions.HideModal();
        //     return false;
        // }
        this.child.removeSelectedRows(selectedRecords);        
        this.props.actions.DeleteChargeType(selectedRecords);

        this.setState((state)=>({                
            selectedChargeRateDetails: state.selectedChargeRateDetails.filter(item =>{
                return !selectedRecords.some(ts=>{
                    return ts["rateId"] === item["rateId"];
                });
            }),
        }),()=>{
            this.child.gridApi.setRowData(this.state.selectedChargeRateDetails);
        });

        this.props.actions.HideModal();
        this.forceUpdate();
    }

    /**
     * date change handler for effective from date
     */
    effectiveFromChangeHandler = (date) => {
        this.setState({ effectiveFromDate: date });
        this.updatedData["effectiveFromDate"] = date;
    }

    adminEffectiveFromDateHandler =(date) =>{
        this.setState({ adminEffectiveFromDate: date 
        }, () => {
                this.adminUpdatedData.effectiveFrom = this.state.adminEffectiveFromDate !== null ? this.state.adminEffectiveFromDate.format() : "";
                this.props.actions.AdminChargeScheduleValueChange(this.adminUpdatedData);
            });
    }

    copyRatesEffectiveFromDateHandler =(date) =>{
        this.setState({ copyEffectiveFromDate : date });
    }    /**
     * date change handler for effective to date
     */
    effectiveToChangeHandler = (date) => {
        this.setState({ effectiveToDate: date });
        this.updatedData["effectiveToDate"] = date;
    }

    copyRatesEffectiveToDateHandler =(date) => {
        this.setState({  
            copyEffectiveToDate:date,
            //adminEffectiveToDate: date, // 597 - CONTRACT-Charge Rates Issues - UAT Testing
            // copyEffectiveFromDate: date   
        }, () => {
            // this.adminUpdatedData.effectiveTo = this.state.adminEffectiveToDate !== null ? this.state.adminEffectiveToDate.format() : "";
            // this.props.actions.AdminChargeScheduleValueChange(this.adminUpdatedData);
        });
    }

    adminEffectiveToDateHandler =(date) =>{
        this.setState({ adminEffectiveToDate: date 
        }, () => {
            this.adminUpdatedData.effectiveTo = this.state.adminEffectiveToDate !== null ? this.state.adminEffectiveToDate.format() : "";
            this.props.actions.AdminChargeScheduleValueChange(this.adminUpdatedData);
        });
    }

    /**
     * Confirmation rejection handler
     */
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    /**
     * copy charge rate handler
     */
    copyChargeRateHandler = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            this.props.actions.CopyChargeTypeModalState(true);
        }
        else {
            IntertekToaster(localConstant.contract.rateSchedule.CHARGE_TYPE_COPY_WARNING,'warningToast rsChargeRateCopyWarn');
        }
    }

    chargeRateCopyCancel = (e) =>{
        e.preventDefault();
        this.props.actions.CopyChargeTypeModalState(false);
        this.setState({
            copyEffectiveFromDate: '',
            copyEffectiveToDate: ""
        });
    }

    chargeRateCopy = (e) => {
        e.preventDefault();
        const dateVal = this.chargeTypeDateValidation(this.state.copyEffectiveFromDate,this.state.copyEffectiveToDate);
        if(!dateVal){
            return false;
        }
        const fromDate = this.state.copyEffectiveFromDate.format();
        let toDate = "";
        if (this.state.copyEffectiveToDate !== "" && this.state.copyEffectiveToDate !== null) {
            toDate = this.state.copyEffectiveToDate.format();
        }
        if (this.state.inValidDateFormat) {
            IntertekToaster(localConstant.contract.common.VALID_DATE_WARNING,'warningToast rsValidDateChargeType');
            return false;
        }
        if (!this.dateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
            const selectedRecords = this.child.getSelectedRows();
            if (selectedRecords.length > 0) {
                selectedRecords.forEach(iteratedValue => {
                    const rateObj = objectUtil.cloneDeep(iteratedValue);
                    rateObj.chargeValue=0;  //Added for D-1167
                    rateObj.discountApplied=0;//Added for D-1167
                    rateObj.effectiveFrom= fromDate;
                    rateObj.effectiveTo = toDate;
                    if(this.state.orderSequenceId >= 1 ){
                        rateObj["sequenceId"] = this.state.orderSequenceId + 1; //Added for D-1167
                        this.setState({
                            orderSequenceId:rateObj["sequenceId"]
                        });
                    }   
                    this.copyChargeRate(this.state.selectedScheduleName, rateObj);                    
                    this.state.selectedChargeRateDetails.unshift(rateObj);  //Added for D-1167   
                   // this.state.selectedChargeRateDetails.push(this.getCopyChargeRate(this.state.selectedScheduleName, rateObj));                   
                });
                IntertekToaster(localConstant.contract.rateSchedule.CHARGE_TYPE_COPY_SUCCESS,'successToast rsChargeTypeCopySuccess');
                this.props.actions.CopyChargeTypeModalState(false);
                this.setState({
                    copyEffectiveFromDate: '',
                    copyEffectiveToDate: ""
                });    
                this.child.gridApi.setRowData(this.state.selectedChargeRateDetails);        
            }
        }
    }

    getCopyChargeRate = (scheduleName, iteratedValue, scheduleId,baseScheduleId) => {
        const chargeType = Object.assign({}, iteratedValue);
        // const chargeType = {};
        // const fromDate = this.state.effectiveFromDate.format();
        // let toDate = "";
        // if (this.state.effectiveToDate !== "" && this.state.effectiveToDate !== null) {
        //     toDate = this.state.effectiveToDate.format();
        // }
        chargeType.rateId = Math.floor(Math.random() * (Math.pow(10, 5)));
        chargeType.scheduleName = scheduleName;
        chargeType.standardValue = iteratedValue.standardValue;
        chargeType.chargeValue = iteratedValue.chargeValue;
        chargeType.description =iteratedValue.description;
        chargeType.discountApplied = iteratedValue.discountApplied;
        chargeType.percentage = iteratedValue.percentage;
        if (baseScheduleId == null)
            chargeType.baseScheduleId = baseScheduleId; 
        chargeType.isDescriptionPrintedOnInvoice = iteratedValue.isDescriptionPrintedOnInvoice;
        // chargeType.effectiveFrom = fromDate;
        // chargeType.effectiveTo = toDate;
        // chargeType.baseScheduleName = this.state.selectedScheduleName; //Commented For D-1167
        // chargeType.baseRateId = iteratedValue.rateId;//Commented For D-1167
        chargeType.isActive = iteratedValue.isActive;
        chargeType.recordStatus = "N";        
        if(scheduleId) chargeType.scheduleId = scheduleId;
        return chargeType;
    }

    /**
     * Copy the selected charge rate
     */
    copyChargeRate = (scheduleName, iteratedValue, scheduleId,baseScheduleId) => {
        this.props.actions.AddChargeType(this.getCopyChargeRate(scheduleName, iteratedValue, scheduleId,baseScheduleId));
    }

    /**
     * Copy selected rate schedule
     */
    copyRateSchedule = (selectedRateSchedule) => {
        const copyRateScheduleObj = Object.assign({}, selectedRateSchedule);
        const copyRateScheduleName = selectedRateSchedule.scheduleName + localConstant.contract.rateSchedule.COPY_SCHEDULE_NAME;
        copyRateScheduleObj.scheduleName = copyRateScheduleName;
        copyRateScheduleObj.baseScheduleName = selectedRateSchedule.scheduleName;
        copyRateScheduleObj.recordStatus = localConstant.contract.rateSchedule.SCHEDULE_NEW_RECORD_STATUS;
        copyRateScheduleObj.baseScheduleId = null;
        copyRateScheduleObj.readonly = false;
        copyRateScheduleObj.canBeDeleted = true;   //Added for Sanity Defect 73
        // Added for Copying Rates to resolve conflict in renaming;
        copyRateScheduleObj.scheduleId = Math.floor(Math.random() * (Math.pow(10, 5)));
        if (this.state.orderSequenceId >= 1) {
            copyRateScheduleObj["sequenceId"] = this.state.orderSequenceId + 1;
            this.setState({
                orderSequenceId: copyRateScheduleObj["sequenceId"]
            });
        }
        this.props.chargeTypes && this.props.chargeTypes.map(iteratedValue => {
            if (iteratedValue.scheduleName === selectedRateSchedule.scheduleName && iteratedValue.recordStatus !== "D") {
                this.copyChargeRate(copyRateScheduleObj.scheduleName, iteratedValue, copyRateScheduleObj.scheduleId,copyRateScheduleObj.baseScheduleId);
            }
        });

        copyRateScheduleObj.isSelected = true;
        sessionStorage.setItem('selectedSchedule', JSON.stringify(copyRateScheduleObj)); //Added for D789
        this.props.actions.AddRateSchedule(copyRateScheduleObj);
        this.state.selectedChargeScheduleDetails.forEach((itratedValue) => {
            itratedValue.isSelected = false;
        });
        this.state.selectedChargeScheduleDetails.unshift(copyRateScheduleObj);
        this.gridChargeSchedule.gridApi.setRowData(this.state.selectedChargeScheduleDetails);
        //this.bindChargeScheduleDetail();
    }

    adminContractRatesChangeHandler = (e) =>{
        this.adminChargeRateInputChangeHandler(e);
        if(e.target.name === 'companyChargeScheduleName' ){
            this.props.actions.FetchAdminInspectionGroup(e.target.value,true);  //Changes for Defect 112
            delete this.adminUpdatedData['companyChgSchInspectionGroupName'];
            delete this.adminUpdatedData['companyChgSchInspGrpInspectionTypeName'];
        }
        if(e.target.name === 'companyChgSchInspectionGroupName' ){
            const schName=this.props.adminChargeScheduleValueChange.companyChargeScheduleName;
            this.props.actions.FetchAdminInspectionType(e.target.value,schName,true); //Changes for Defect 112
            delete this.adminUpdatedData['companyChgSchInspGrpInspectionTypeName'];
        }
        if (e.target.name === 'companyChgSchInspGrpInspectionTypeName') {
            const schName = this.props.adminChargeScheduleValueChange.companyChargeScheduleName;
            const groupName = this.props.adminChargeScheduleValueChange.companyChgSchInspectionGroupName;
            this.props.actions.FetchAdminChargeRates({
                companyChgSchInspGrpInspectionTypeName: e.target.value,
                companyChgSchInspGroupName: groupName,
                companyChargeScheduleName: schName
            }).then(res => {
                if (res) {
                    this.secondChild.refreshGrid();
                }
            });  
        }
        this.props.actions.AdminRateScheduleSelect();
        this.props.actions.AdminChargeScheduleValueChange(this.adminUpdatedData);
    }

      /**
     * Admin Contract Rates Button Handler
     */
    adminContractRatesHandler = () => {
        this.setState({
            effectiveFromDate: '',
            effectiveToDate: "",
            adminEffectiveFromDate:this.props.adminChargeScheduleValueChange && this.props.adminChargeScheduleValueChange.effectiveFrom ? dateUtil.defaultMoment(this.props.adminChargeScheduleValueChange.effectiveFrom) : dateUtil.defaultMoment(this.props.contractInfo.contractStartDate), 
            adminEffectiveToDate:this.props.adminChargeScheduleValueChange && this.props.adminChargeScheduleValueChange.effectiveTo ? dateUtil.defaultMoment(this.props.adminChargeScheduleValueChange.effectiveTo) : dateUtil.defaultMoment(this.props.contractInfo.contractEndDate)
        });
        // this.props.actions.AdminContractRatesModalState(true);
        this.props.actions.AdminContractRatesModalState(!this.props.isAdminContractRatesModalOpen);
        // if(!this.props.isAdminContractRatesModalOpen){
        //     // this.props.actions.AdminRateScheduleSelect();
        this.adminUpdatedData={
            effectiveFrom:(this.props.adminChargeScheduleValueChange && this.props.adminChargeScheduleValueChange.effectiveFrom) ? dateUtil.defaultMoment(this.props.adminChargeScheduleValueChange.effectiveFrom) : this.props.contractInfo.contractStartDate, 
            effectiveTo:(this.props.adminChargeScheduleValueChange && this.props.adminChargeScheduleValueChange.effectiveTo) ? dateUtil.defaultMoment(this.props.adminChargeScheduleValueChange.effectiveTo) : this.props.contractInfo.contractEndDate
        };
        this.props.actions.AdminChargeScheduleValueChange(this.adminUpdatedData);
        this.props.actions.ClearChargeRates();
        // }
    }

    ClearAdminContractRatesScheduleChange = () => {
        this.setState({
            effectiveFromDate: '',
            effectiveToDate: "",
            adminEffectiveFromDate: ''
        });
        this.adminUpdatedData = {};
        this.props.actions.AdminChargeScheduleValueChange(this.adminUpdatedData);
        this.props.actions.AdminContractRatesModalState(false);
    }

    /**
     * Admin Contract Rate cancel button handler
     */
    cancelAdminContractRates = (e) =>{
        e.preventDefault();
        this.props.actions.AdminContractRatesModalState(false);
        this.setState({
            effectiveFromDate: '',
            effectiveToDate: ""
        });
        this.adminUpdatedData = {};
    }

    /**
     * Add Admin Contract Rates button handler. adds the selected admin contract rates.
     */
    addAdminContractRates = (e) =>{
        e.preventDefault();
        if(this.adminContractRatesValidation(this.adminUpdatedData)){
            if (this.adminUpdatedData && !isEmpty(this.adminUpdatedData)) {
                const fromDate = this.state.adminEffectiveFromDate ? this.state.adminEffectiveFromDate.format(localConstant.commonConstants.SAVE_DATE_FORMAT) :'';
                let toDate = "";
                if (this.state.adminEffectiveToDate !== "" && this.state.adminEffectiveToDate !== null) {
                    toDate = this.state.adminEffectiveToDate.format(localConstant.commonConstants.SAVE_DATE_FORMAT);
                }
                if (this.state.inValidDateFormat) {
                    IntertekToaster(localConstant.contract.common.VALID_DATE_WARNING,'warningToast rsValidDateWarning');
                    return false;
                }
                if (!this.dateRangeValidator(fromDate, toDate) && !this.state.inValidDateFormat) {
                    if(this.props.adminSelectedChargeRates.length >0){
                        // this.updatedData ={};
                        let adminContractRate = {};
                        this.props.adminSelectedChargeRates.map(iteratedValue =>{
                            adminContractRate["rateId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                            adminContractRate["effectiveFrom"] = fromDate;
                            adminContractRate["effectiveTo"] = toDate;
                            adminContractRate["description"] = iteratedValue.name;
                            adminContractRate["scheduleName"] = this.state.selectedScheduleName;
                            adminContractRate["standardInspectionTypeChargeRateId"] = iteratedValue.id; //NDT Invoice Issue
                            adminContractRate["standardValue"] = iteratedValue[iteratedValue.selectedChargeValue];
                            if(!isEmpty(this.props.selectedContract.contractNumber)){
                                adminContractRate["contractNumber"]=this.props.selectedContract.contractNumber;
                            }
                            if(iteratedValue[iteratedValue.selectedChargeValue] && (parseFloat(iteratedValue[iteratedValue.selectedChargeValue]) > 0)){
                                const discountApplied = (parseFloat(iteratedValue[iteratedValue.selectedChargeValue]) - parseFloat(iteratedValue[iteratedValue.selectedChargeValue]));
                                const percentage = (discountApplied / parseFloat(iteratedValue[iteratedValue.selectedChargeValue])) *100;
                                adminContractRate["discountApplied"]=parseFloat(discountApplied);
                                adminContractRate["percentage"] = parseFloat(percentage);
                            }
                            adminContractRate["isActive"] = true;    
                            adminContractRate["discountApplied"] = 0.00;
                            adminContractRate["percentage"] = 0.00;
                            adminContractRate["chargeValue"] = iteratedValue[iteratedValue.selectedChargeValue];
                            adminContractRate["chargeType"] = iteratedValue.expenseType;
                            adminContractRate["recordStatus"] = "N";   
                            if(this.state.orderSequenceId >= 1 ){
                                adminContractRate["sequenceId"] = this.state.orderSequenceId + 1;
                                this.setState({
                                    orderSequenceId:adminContractRate["sequenceId"]
                                });
                            }
                            if(this.gridChargeSchedule !== undefined && this.gridChargeSchedule.getSelectedRows() !== undefined) {
                                const selectedData = this.gridChargeSchedule.getSelectedRows();  
                                adminContractRate["scheduleId"] = selectedData[0].scheduleId;
                            }
                            this.props.actions.AddChargeType(adminContractRate);
                            this.state.selectedChargeRateDetails.unshift(adminContractRate);                
                            this.child.gridApi.setRowData(this.state.selectedChargeRateDetails);          
                            this.child.gridApi.setFocusedCell(0, "chargeType");
                            //this.props.actions.AdminRateScheduleSelect();  
                            adminContractRate = {};
                            IntertekToaster(localConstant.contract.rateSchedule.ADMIN_CHARGE_RATE_SUCCESS, 'successToast adminRateScheduleSuccess');
                        });
                    }
                    else{
                        IntertekToaster(localConstant.contract.rateSchedule.USE_ADMIN_CONTRACT_RATES_WARNING,'warningToast rsUseAdminRateWarning');
                    }
                }
            }
        }
    }

     /**
     * Date range validator
     */
    dateRangeValidator = (from, to) => {
        let isInValidDate = false;
        if (to !== "" && to !== null) {
            if (moment(from).isAfter(to,'day')) {
                isInValidDate = true;
                IntertekToaster(localConstant.commonConstants.INVALID_DATE_RANGE,'warningToast rsInvalidDateRange');
            }
        }
        return isInValidDate;
    }

    handleDateBlur = (e) => {
        if (e && e.target !== undefined) {
            const isValid = dateUtil.isUIValidDate(e.target.value);
            if (!isValid) {
                this.setState({ inValidDateFormat: true });
            } else {
                this.setState({ inValidDateFormat: false });
            }
        }
    }

    handleEndDateBlur = (e) => {
        if (e && e.target !== undefined) {
            this.setState({ inValidDateFormat: false });
            if (e.target.value !== "" && e.target.value !== null) {
                this.handleDateBlur(e);
            }
        }
    }

     /** Delete Charge Rates handler function */
    deleteChargeRatesHandler = () =>{
        const cutOffDate = '01/01/'+new Date().getFullYear();
        this.setState({ isDeleteChargeRatesModalOpen:true,
            effectiveToDate:  moment(cutOffDate) 
        });
    }

    /** View Charge Rates till given effective to date for delete */
    viewChargeRates = (e) =>{
            e.preventDefault();
            // const cutOffDate = '01/01/'+new Date().getFullYear();
            const cutOffDate=moment(this.state.effectiveToDate).format(localConstant.commonConstants.EXPECTED_DATE_FORMAT).toString();
        if(this.state.inValidDateFormat || this.state.effectiveToDate === "" || this.state.effectiveToDate === null){
            IntertekToaster(localConstant.contract.common.VALID_DATE_WARNING,'warningToast drsValidDate12');
            return false;
        }
        let chargeRates =  this.props.chargeTypes.filter (iteratedValue => !isEmpty(iteratedValue.effectiveTo)
                                                                        && new Date(iteratedValue.effectiveTo) <= new Date(this.state.effectiveToDate.format()));                                                    
        if(chargeRates.length === 0){
            IntertekToaster(localConstant.contract.rateSchedule.CUT_OFF_DATE_WARNING, 'warningToast drsCutOffDateWarning');
            return false;
        }
        chargeRates = arrayUtil.sort(chargeRates,"chargeType",'asc');  
        const scheduleDetails=[]; 
        const distinctScheduleNames = [ ...new Set(chargeRates.map(iteratedValue =>  iteratedValue.scheduleName )) ];   
        const schedules = this.props.rateSchedule.filter( el => distinctScheduleNames.includes(el.scheduleName));
        distinctScheduleNames.forEach(schedulename =>{
            const index = schedules.findIndex(x=>x.scheduleName === schedulename);
            if(index >= 0){
                scheduleDetails.push(schedules[index]);
            }
        });
        scheduleDetails.map( item =>
            {
                item["ChargeRates"] = chargeRates.filter( x=> x.scheduleName === item.scheduleName);
            }) ;          
       const docDefinition = GetRateScheduleTemplate( this.props.contractInfo.contractHoldingCompanyName
                                                    , this.props.contractInfo.contractCustomerName
                                                    , this.props.contractInfo.contractNumber
                                                    , this.props.contractInfo.customerContractNumber
                                                    , cutOffDate
                                                    , scheduleDetails);
        generatePdf(docDefinition, "DeleteRatesReport.pdf");
    }

    /** Delete charge rates based on effective date */
    deleteChargeRates = (e) =>{
        e.preventDefault();
        const chargeRates = [];
        let chargeRatesCount = 0;
        if(this.state.inValidDateFormat || this.state.effectiveToDate === "" || this.state.effectiveToDate === null){
            IntertekToaster(localConstant.contract.common.VALID_DATE_WARNING,'warningToast drsValidDate12');
            return false;
        }
        this.props.chargeTypes.map(iteratedValue => {
            if (iteratedValue.recordStatus !== "D" && !isEmpty(iteratedValue.effectiveTo)) {
                if (new Date(iteratedValue.effectiveTo) <= new Date(this.state.effectiveToDate.format())) {
                    chargeRates.push(iteratedValue);
                    chargeRatesCount++;
                }
            }
        });
        if(chargeRatesCount === 0){
            IntertekToaster(localConstant.contract.rateSchedule.CUT_OFF_DATE_WARNING, 'warningToast drsCutOffDateWarning');
            return false;
        }
        
        this.props.actions.DeleteChargeType(chargeRates);
        this.setState((state)=>({                
            selectedChargeRateDetails: state.selectedChargeRateDetails.filter(item =>{
                return !chargeRates.some(ts=>{
                    return ts["rateId"] === item["rateId"];
                });
            }),
        }),()=>{
            this.child.gridApi.setRowData(this.state.selectedChargeRateDetails);
        });

        const message= localConstant.contract.rateSchedule.DELETE_RATES_SUCCESS + ' ' + chargeRatesCount + ' Rate(s).  ' + localConstant.contract.rateSchedule.DELETE_RATES_SUCCESS_SAVE_MESSAGE;
       
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: message,
            modalClassName: "warningToast",
            type: "confirm",
            buttons: [
                {
                    buttonName: localConstant.commonConstants.OK,
                    onClickHandler: this.onModalOkClicked,
                    className: "modal-close m-1 btn-small"
                }
            ]
        };
        this.props.actions.DisplayModal(confirmationObject);
        // IntertekToaster(`${ localConstant.contract.rateSchedule.DELETE_RATES_SUCCESS } ${ chargeRatesCount } Rate(s). ${ localConstant.contract.rateSchedule.DELETE_RATES_SUCCESS_SAVE_MESSAGE }` , 'successToast drsCutOffDateSuccess');
        this.setState({ 
            isDeleteChargeRatesModalOpen:false,
            effectiveToDate:"" 
        });
    }
    onModalOkClicked = () => {
        this.props.actions.HideModal();
    };
    /** Cancel charge rates delete */
    cancelDeleteChargeRates = (e) =>{
        e.preventDefault();
        this.setState({ 
            isDeleteChargeRatesModalOpen:false,
            effectiveToDate:"" 
        });
    }

    /** Cancel View charge rates Popup */
    cancelViewChargeRate =(e) =>{
        e.preventDefault();
        this.setState({ 
            isViewChargeRatesModalOpen:false
        });
        this.viewChargeRates=[];
    }

    /** Cancel copy schedule. */
    cancelCopySchedule = (e) => {
        e.preventDefault();
        this.setState({
            isCopyScheduleModalOpen: false
        });
    } 
    /*AddScheduletoRF*/
 /**Cancel addScheduleToRF */
     canceladdScheduleToRF = (e) => {
        e.preventDefault();
        this.setState({
            isAddScheduletoRFCModelOpen:false
        });
    }   
 /** Cancel Delete schedule. */
    cancelDeleteSchedule = (e) => {
        e.preventDefault();
        this.setState({
            isDeleteScheduleModalOpen: false
        });
    } 
    /** Copy selected schedules with validations */
    copySelectedSchedules = (e) => {
        e.preventDefault();
        let rateScheduleDuplicate = false;
        let rateScheduleNew=false;
        const selectedRecords = this.copyScheduleChild.getSelectedRows();
        if(selectedRecords.length === 0){
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_SCHEDULE_VALIDATION,"warningToast copyScheduleZeroVal");
            return false;
        }
        if(selectedRecords.length > 10){
            IntertekToaster(localConstant.validationMessage.MORE_THAN_10_SCHEDULES_VALIDATION,"warningToast copyScheduleTenVal");
            return false;
        }
        selectedRecords.forEach(iteratedValue=>{
            const copyRateScheduleName = iteratedValue.scheduleName + "(copy)";
            const data = { 'scheduleName': copyRateScheduleName };
            // Defect 629 - Contracts - deleting copied schedule in correct warning
            if (iteratedValue.recordStatus !== null) {
                IntertekToaster((iteratedValue.scheduleName ? iteratedValue.scheduleName : "") + " " + localConstant.contract.rateSchedule.SAVE_SCHEDULE_BEFORE, 'warningToast copyScheduleValidate');
                rateScheduleNew = true;
            }
            else if (this.isRateScheduleDuplicate(data) && !rateScheduleNew) {
                IntertekToaster(`${ copyRateScheduleName } ${ localConstant.contract.rateSchedule.RATE_SCHEDULE_DUPLICATE }`, `warningToast rsDuplicateRateSchedule`);
                rateScheduleDuplicate = true;    
            }
        });
        if(!rateScheduleDuplicate && !rateScheduleNew){
            selectedRecords.forEach(iteratedValue => {
                this.copyRateSchedule(iteratedValue);
            });
            IntertekToaster(localConstant.contract.rateSchedule.RATE_SCHEDULE_COPY_SUCCESS, 'successToast rsRateScheduleCopySuccess');  
            this.setState({
                isCopyScheduleModalOpen: false       
            });
        }
    }

    /** Copy rate schedule handler */
    copyRateScheduleHandler = () => {
        this.setState({
            isCopyScheduleModalOpen: true
        });
        this.updateOrderSequenceId();
        this.bindChargeScheduleDetail();
    }  

    /** Delete rate schedule handler */
    deleteRateScheduleHandler =()=>{
        this.setState({
            isDeleteScheduleModalOpen:true
        });
    }
    /*AddScheduletoRF*/
    
    addScheduleToRelatedConfirmation = (e) => {
        const modifiedSchedule = this.props.rateSchedule.filter(x => !required(x.recordStatus));
        const modifiedRates = this.props.chargeTypes.filter(x => !required(x.recordStatus)); //fix for D789
        if(modifiedSchedule.length > 0 || modifiedRates.length>0){
            IntertekToaster(localConstant.validationMessage.CHARGE_SCHEDULE_AddRF,"warningToast copyScheduleTenVal");
            return false;
        }
        this.addscheduleConfirm(e);
    }

    /**Add Schedule to Related Framework  */
    addScheduleToRelatedHandler=()=>{
        this.props.actions.HideModal();
        const contractId = this.props.contractInfo.id;
        this.props.actions.ShowLoader();
        this.props.actions.isRelatedFrameworkExisits(contractId).then(res => {
            if(res){
                this.props.actions.isDuplicateFrameworkContractSchedules(contractId).then(duplicates =>{
                    if(duplicates && duplicates.result.length > 0){
                        const result = duplicates.result;
                        const scheduleNames = [];
                        let schedules ='';
                        let contracts='';
                        const relatedContractNumber = [];
                        result.map((x,key) => {
                            if (scheduleNames.indexOf(x.name) === -1)
                                scheduleNames.push(x.name);
                            if (relatedContractNumber.indexOf(x.contractNumber) === -1) {
                                relatedContractNumber.push(x.contractNumber);
                            }
                        });
                    //    schedules = scheduleNames.join().match(/.{1,55}/g);
                    //    contracts = relatedContractNumber.join().match(/.{1,51}/g);
                        schedules = scheduleNames.join().toString().replace(/,/g, '\n');
                        contracts= relatedContractNumber.join().toString().replace(/,/g, '\n');

                        const confirmMessage = "The following Schedules already exist: \n "+ schedules + " \n\n They will not be copied over to the following Related Framework Contracts: \n" + contracts + "\n \n Do you wish to continue?";
                        const confirmationObject = {
                            title: modalTitleConstant.CONFIRMATION,
                            message: confirmMessage,
                            type: "confirm",
                            modalClassName: "warningToast",
                            isSetModalHeight: true, //D789 - To set the modal height
                            buttons: [
                                {
                                    buttonName: localConstant.commonConstants.YES,
                                    onClickHandler:this.confirmBatchProcess,
                                    className: "modal-close m-1 btn-small"
                                },
                                {
                                    buttonName: localConstant.commonConstants.NO,
                                    onClickHandler:this.rejectBatchProcess,
                                    className: "modal-close m-1 btn-small",
                                }
                            ]
                        };
                    this.props.actions.HideLoader();
                    this.props.actions.DisplayModal(confirmationObject);
                    }
                    else{
                        this.confirmBatchProcess();
                    }
                });
            } else {
                this.props.actions.HideLoader();
            }
        });
    }
    confirmBatchProcess = () =>{
        this.props.actions.ShowLoader();
        const contractId = this.props.contractInfo.id;
        this.props.actions.frameworkScheduleCopyBatch(contractId).then(res =>{
            if(res){
                this.props.actions.HideLoader();
                const Id = sessionStorage.getItem('BatchId');
                if(Id){
                    this.checkIfBatchProcessCompleted(Id);
                }
            }
            else{
                this.props.actions.HideLoader();
            } 
        });
        this.props.actions.HideModal();
    }

    checkIfBatchProcessCompleted = (Id) => {
        this.props.actions.checkIfBatchProcessCompleted(Id);
    }

    rejectBatchProcess = () => {
        this.props.actions.HideModal();
    }

   /*AddScheduletoRF*/
   
   /**confirmAddSchedules */
addscheduleConfirm =(e)=>
{
    e.preventDefault();
    const contractId = this.props.contractInfo.id;
    //this.props.actions.ShowLoader();
    this.props.actions.isRelatedFrameworkExisits(contractId).then(res => {
        if (res) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.ADD_SCHEDULE_TO_RF,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler:this.addScheduleToRelatedHandler,
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: localConstant.commonConstants.NO,
                        onClickHandler:this.confirmationRejectHandler,
                        className: "modal-close m-1 btn-small",
                    }
                ]
            };

            this.props.actions.DisplayModal(confirmationObject);
        }
        else {
            this.props.actions.HideLoader();
        }
    });
}
/*AddScheduletoRF*/
    /** Add Schedules to Related Framework Contracts */
    addScheduleToRelatedFramework = (e) => {
        e.preventDefault();
        this.props.actions.HideModal();
        const selectedRecords = this.copyScheduleChild.getSelectedRows();
        if(selectedRecords.length === 0){
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_SCHEDULE_VALIDATION,"warningToast copyScheduleZeroVal");
            return false;
        }
        if(selectedRecords.length > 10){
            IntertekToaster(localConstant.validationMessage.MORE_THAN_10_SCHEDULES_VALIDATION,"warningToast copyScheduleTenVal");
            return false;
        }
      let chk=0;
        selectedRecords.map(iteration=> { 
            if(iteration.recordStatus== "N")
            {
                chk=1;
            IntertekToaster(localConstant.validationMessage.CHARGE_SCHEDULE_AddRF,"warningToast copyScheduleTenVal");
            return false;
            }
            } );
            if(chk==0)
            {
        this.props.actions.AddShedudletoIFContract(selectedRecords);
        this.setState({
            isAddScheduletoRFCModelOpen: false
        });
           }
    }

    decimalWithLimtFormat = (evt) => {  
        const e = evt || window.event;   
        const expression = ("(\\d{0,"+ parseInt(10)+ "})[^.]*((?:\\.\\d{0,"+ parseInt(4) +"})?)");
        const rg = new RegExp(expression,"g"); 
        const match = rg.exec(e.target.value.replace(/[^\d.]/g, ''));
        e.target.value = match[1] + match[2]; 

        if (e.preventDefault) e.preventDefault();
    };

    // PROD def 651 fix
    addCellBlurListener = (data) => {
        const target = document.getElementById(data);
        if (target) {
            target.addEventListener('blur', this.onCellBlur);
        }
    };
    // PROD def 651 fix
    onCellBlur = () => {
        this.bindChargeRateDetailGrid();
        this.removeCellBlurListener();
    }
   // PROD def 651 fix
    removeCellBlurListener = () => {
        const target = document.activeElement;
        if (target) {
            target.removeEventListener('blur', this.onCellBlur);
        }
    };

    onCellFocused = (e) => { 
        if(e.rowIndex !== null) {  
            this.bindChargeRateDetailGrid();
            let selectedScheduleName = this.state.selectedScheduleName;
            selectedScheduleName=selectedScheduleName.toLowerCase();
            const isReadOnly = (selectedScheduleName === "select" || selectedScheduleName === "" || this.props.interactionMode 
                                        || this.props.pageMode=== localConstant.commonConstants.VIEW || this.props.isScheduleBatchProcess ? true : false);
            const currentEl=e.column.getId()+e.rowIndex;
            if(e.column.getId() === 'effectiveFrom' || e.column.getId() === 'effectiveTo') {
                this.setState({ isDatePicker : true });
            } else {
                this.addCellBlurListener(currentEl); // PROD def 651 fix
                this.setState({ isDatePicker : false });
            }                              
            if(!isReadOnly) {
                if(document.getElementById(e.column.getId() + e.rowIndex) !== null) {
                    if(document.getElementById(e.column.getId() + e.rowIndex).type === 'text') {
                        if(document.getElementById(e.column.getId() + e.rowIndex).classList.contains('focused')){
                            document.getElementById(e.column.getId() + e.rowIndex).focus();                               
                   }else if(document.getElementById(e.column.getId() + e.rowIndex).classList.contains('f_focused')){
                            document.getElementById(e.column.getId() + e.rowIndex).classList.add("focused");                                
                            document.getElementById(e.column.getId() + e.rowIndex).classList.remove('f_focused');
                            document.getElementById(e.column.getId() + e.rowIndex).focus();
                        } else {
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

    chargeRateDetailValidation(row) {
        row["chargeTypeValidation"] = "";
        row["chargeValueValidation"] = "";
        row["effectiveFromValidation"] = "";
        if(required(row.chargeType)){            
            row["chargeTypeValidation"] = localConstant.validationMessage.CHARGE_TYPE_VAL;
        }
        if(requiredNumeric(row.chargeValue) || row.chargeValue === "NaN"){            
            row["chargeValueValidation"] = localConstant.validationMessage.CHARGE_VALUE_VAL;
            row["chargeValueValidation"] = localConstant.validationMessage.CHARGE_VALUE_VAL;
        }
        if(required(row.effectiveFrom)){            
            row["effectiveFromValidation"] = localConstant.validationMessage.EFFECTIVE_FROM_VAL;
        }
        if(!required(row.effectiveFrom) && !this.isValidDateFormat(row.effectiveFrom)) {
            row["effectiveFromValidation"] = localConstant.commonConstants.INVALID_DATE_FORMAT;
            row["effectiveFrom"] = "";
        }
        if(!required(row.effectiveTo) && !this.isValidDateFormat(row.effectiveTo)) {
            row["effectiveToValidation"] = localConstant.commonConstants.INVALID_DATE_FORMAT;
            row["effectiveTo"] = "";
        }
        if (!required(row.effectiveFrom) && !required(row.effectiveTo) && this.isValidDateFormat(row.effectiveFrom) && this.isValidDateFormat(row.effectiveTo)
                && moment(row.effectiveFrom).isAfter(row.effectiveTo, 'day')) {
            row["effectiveFromValidation"] = localConstant.techSpecValidationMessage.PAY_RATE_DATE_RANGE_VALIDATION;
        }
        if(!isEmpty(this.props.contractInfo.contractStartDate)){//D589
            if(moment(row.effectiveFrom).isBefore(this.props.contractInfo.contractStartDate)){
                row["effectiveFromValidation"] = localConstant.validationMessage.EFFECTIVE_FROM_BEFORE_VALIDATION;
            }
        }        
        return row;
    }

    isValidDateFormat(date) {        
        if(date.indexOf(":") === -1 && dateUtil.isUIValidDate(date)) {
            return false;
        }
        return true;
    }

    bindChargeRateDetailGrid(movetofirstpage) {    
        if(this.state.isLoadOnCancel) {                        
            let chargeRateDetail = [];
            if(this.props.rateScheduleOnCancel && this.props.rateScheduleOnCancel.length > 0) {
                const chargeType = [];
                const chargeScheduleData = this.props.rateScheduleOnCancel;                
                const scheduleId = chargeScheduleData[0].scheduleId;                         
                this.props.chargeTypesOnCancel && this.props.chargeTypesOnCancel.map(iteratedValue => {
                    //Added fixes for D651
                    if (iteratedValue.scheduleId === scheduleId) {                    
                        iteratedValue.standardValue=parseFloat(iteratedValue.standardValue).toFixed(2);
                        iteratedValue.percentage=parseFloat(iteratedValue.percentage).toFixed(2);
                        chargeType.push(iteratedValue);
                    }
                });

                if(!isEmptyOrUndefine(chargeType)) {                        
                    const isReadOnly = (this.props.interactionMode || this.props.pageMode=== localConstant.commonConstants.VIEW || this.props.isScheduleBatchProcess ? true : false);                    
                    chargeType.forEach(row => {
                        if(row.sequenceId) {
                            row = this.chargeRateDetailValidation(row);
                            row.readonly = isReadOnly || (this.props.contractInfo.contractType === "IRF" && row.baseRateId >0) ; //Changes for D-789
                            chargeRateDetail.push(row);
                        }
                    });      
                    chargeRateDetail = chargeRateDetail.sort((a, b) => (a.sequenceId < b.sequenceId) ? 1 : -1);     
                   // chargeType = chargeType.sort((a, b) => (a.chargeType > b.chargeType) ? 1 : -1);
                    chargeType.forEach(row => {
                        if(!row.sequenceId) {
                            row = this.chargeRateDetailValidation(row);
                            row.readonly = isReadOnly || (this.props.contractInfo.contractType === "IRF" && row.baseRateId >0) ; //Changes for D-789
                            chargeRateDetail.push(row);
                        }
                    });                    
                    this.setState({                     
                        selectedChargeRateDetails: chargeRateDetail
                    });                                   
                } else {
                    this.setState({                     
                        selectedChargeRateDetails: []
                    }); 
                }    
               
            } else {
                this.setState({                     
                    selectedChargeRateDetails: []
                });
            }            
            if(this.child && this.child.gridApi) this.child.gridApi.setRowData(chargeRateDetail);            
            this.setState({ isLoadOnCancel : false });  
        } else {            
            const chargeTypes  =  this.props.chargeTypes;
            if(!isEmptyOrUndefine(chargeTypes)) {                
                const chargeType = [];
                chargeTypes && chargeTypes.map(iteratedValue => {                    
                    if (this.state.SelectedRateSchedule && iteratedValue.scheduleId === this.state.SelectedRateSchedule.scheduleId && iteratedValue.recordStatus !== "D") {                    
                        //iteratedValue.chargeValue=parseFloat(iteratedValue.chargeValue).toFixed(4); //D990 - sort
                        iteratedValue.standardValue=parseFloat(iteratedValue.standardValue).toFixed(2);
                        //iteratedValue.discountApplied=parseFloat(iteratedValue.discountApplied).toFixed(2);
                        iteratedValue.percentage=parseFloat(iteratedValue.percentage).toFixed(2);
                        chargeType.push(iteratedValue);
                    }
                });                                
                // const chargeRateOrder=arrayUtil.bubbleSort(chargeType, 'sequenceId');
                // chargeType=arrayUtil.gridTopSort(chargeRateOrder);                

                if(!isEmptyOrUndefine(chargeTypes)) {
                    let chargeRateDetail = [];
                    let selectedScheduleName = this.state.selectedScheduleName ;
                    selectedScheduleName = isEmpty(selectedScheduleName) ? "" : selectedScheduleName.toLowerCase();
                    const isReadOnly = (selectedScheduleName === "select" || selectedScheduleName === "" || this.props.interactionMode 
                                            || this.props.pageMode=== localConstant.commonConstants.VIEW || this.props.isScheduleBatchProcess ? true : false);                    
                    chargeType.forEach(row => {
                        if(!this.state.isLoadOnSave && row.sequenceId) {
                            row = this.chargeRateDetailValidation(row);
                            row.readonly = isReadOnly || (this.props.contractInfo.contractType === "IRF" && row.baseRateId >0 && row.baseScheduleId > 0) ; //Changes for D-789
                            chargeRateDetail.push(row);
                        }
                    });      
                    chargeRateDetail = chargeRateDetail.sort((a, b) => (a.sequenceId < b.sequenceId) ? 1 : -1);
                    // chargeType = chargeType.sort((a, b) => (a.chargeType > b.chargeType) ? 1 : -1);                                                               
                    chargeType.forEach(row => {
                        if(this.state.isLoadOnSave || !row.sequenceId) {
                            row = this.chargeRateDetailValidation(row);
                            row.readonly = isReadOnly || (this.props.contractInfo.contractType === "IRF" && row.baseRateId >0 && row.baseScheduleId > 0) ; //Changes for D-789
                            chargeRateDetail.push(row);
                        }
                    });                    
                    this.setState({                     
                        selectedChargeRateDetails: chargeRateDetail
                    });
                    if(this.state.isDatePicker && this.child && this.child.gridApi) this.child.gridApi.setRowData(chargeRateDetail);               
                    //D-1267 Fix -Start
                    if(movetofirstpage && this.child && this.child.gridApi){ //def 651 issue 7 fix
                        this.child.gridApi.paginationGoToPage(0);   
                    }
                    //D-1267 Fix -End
                } else {
                    this.setState({                     
                        selectedChargeRateDetails: []
                    });
                }
            } else {
                this.setState({                     
                    selectedChargeRateDetails: []
                });
            }            
        }
        if(this.state.isLoadOnSave) this.setState({ isLoadOnSave : false });  
    }

    addChargeRate = (e) => {
        const selectedRecords = this.gridChargeSchedule.getSelectedRows();
        let isValid = true;
        if (selectedRecords.length > 0) {            
            const currentRateSchedule = this.props.rateSchedule && this.props.rateSchedule.filter(iteratedValue => {
                if (iteratedValue.scheduleId === selectedRecords[0].scheduleId) {
                    return iteratedValue;
                }
            });
            if(Array.isArray(currentRateSchedule)&&currentRateSchedule.length>0){
            if(currentRateSchedule && !isEmptyOrUndefine(currentRateSchedule[0].scheduleName) && !isEmptyOrUndefine(currentRateSchedule[0].chargeCurrency)) {
                e.preventDefault();        
                this.updatedData = {};
                this.updatedData["chargeRateType"] = "";
                this.updatedData["chargeTypeValidation"] = localConstant.validationMessage.CHARGE_TYPE_VAL;
                this.updatedData["rateId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                this.updatedData["effectiveFrom"] = "";
                this.updatedData["effectiveFromValidation"] = localConstant.validationMessage.EFFECTIVE_FROM_VAL;
                this.updatedData["effectiveTo"] = "";
                this.updatedData["scheduleName"] = currentRateSchedule[0].scheduleName; //D106 fixes
                if(!isEmpty(this.props.selectedContract.contractNumber)){
                    this.updatedData["contractNumber"]=this.props.selectedContract.contractNumber;
                }
                this.updatedData["standardValue"] = 0.00;
                this.updatedData["percentage"] = 0.00;        
                this.updatedData["discountApplied"] = 0.00;
                this.updatedData["chargeValue"] = 0.00;        
                this.updatedData["createdBy"] = this.props.loggedInUser;
                this.updatedData["isActive"] = true;
                this.updatedData["recordStatus"] = "N";
                if(this.state.orderSequenceId >= 1 ){
                    this.updatedData["sequenceId"] = this.state.orderSequenceId + 1;
                    this.setState({
                        orderSequenceId:this.updatedData["sequenceId"]
                    });
                }
                if(this.gridChargeSchedule !== undefined && this.gridChargeSchedule.getSelectedRows() !== undefined) {
                    const selectedData = this.gridChargeSchedule.getSelectedRows();  
                    this.updatedData["scheduleId"] = selectedData[0].scheduleId;
                }
                this.props.actions.AddChargeType(this.updatedData);    
                this.state.selectedChargeRateDetails.unshift(this.updatedData);                
                this.child.gridApi.paginationGoToPage(0);     //D-1267 Fix          
                this.child.gridApi.setRowData(this.state.selectedChargeRateDetails);          
                this.child.gridApi.setFocusedCell(0, "chargeType");   
            } else {
                isValid = false;
            }
        }
        } else {
            isValid = false;
        }
        if(!isValid)
            IntertekToaster(localConstant.contract.CHARGE_SCHEDULE_MANDATORY,'warningToast');            
    }

    bindChargeScheduleDetail() {        
        const scheduleOrder=arrayUtil.bubbleSort(this.props.rateSchedule, 'sequenceId');   //Changes for D-1039
        const rateSchedules = arrayUtil.gridTopSort(scheduleOrder); //Changes for D1039
        if(rateSchedules){
            if(rateSchedules.length > 0){
                rateSchedules.map((itratedValue,index)=>{
                    if (this.state.SelectedRateSchedule && isEmpty(this.state.SelectedRateSchedule.scheduleName)) {
                        rateSchedules[0]["isSelected"] = true;
                    }
                    else if(this.state.SelectedRateSchedule && itratedValue.scheduleName === this.state.SelectedRateSchedule.scheduleName){
                        rateSchedules[index]["isSelected"] = true;
                    }
                    else{
                        rateSchedules[index]["isSelected"] = false;
                    }
                    if(this.state.isDeleteScheduleModalOpen && rateSchedules.length > 1){
                        rateSchedules[index]["isSelected"] = false;
                    }
                                
                    rateSchedules[index]["scheduleNameValidation"] = required(itratedValue.scheduleName) ? localConstant.validationMessage.SCHEDULE_NAME_VAL : "";                    
                    rateSchedules[index]["chargeCurrencyValidation"] = required(itratedValue.chargeCurrency) ? localConstant.validationMessage.CHARGE_CURRENCY_VAL : "";
                    if(!required(itratedValue.scheduleName)) {                                                                      
                        rateSchedules[index]["scheduleNameValidation"] = rateSchedules.filter(x=> x.scheduleId !== itratedValue.scheduleId && x.scheduleName.toLowerCase().trim() === itratedValue.scheduleName.toLowerCase().trim()).length > 0 ? 
                                                                        localConstant.contract.rateSchedule.RATE_SCHEDULE_DUPLICATE : "";     
                    }
                });
            }
        }

        if(rateSchedules){
            if(rateSchedules.length > 0){                
                rateSchedules.forEach((itratedValue,index)=>{
                    // Added for Defect Id-286
                    if (index===this.props.selectedRowIndex && !this.state.isCopyScheduleModalOpen && !this.state.isDeleteScheduleModalOpen) {
                        itratedValue.isSelected = true; 
                    }
                    else{
                        itratedValue.isSelected= false;
                    } 
            
                    itratedValue.readonly = (this.props.interactionMode || this.props.pageMode=== localConstant.commonConstants.VIEW || this.props.isScheduleBatchProcess || (this.props.contractInfo.contractType === "IRF" && itratedValue.baseScheduleId > 0) ? true : false); //Changes for D-789
                });
            }
        }
        if(rateSchedules && rateSchedules.length > 0){
            this.setState((state) => {
                return {
                    selectedChargeScheduleDetails: rateSchedules
                };
            });
        }
    }

    onScheduleCellFocused = (e) => {    
        if(e.rowIndex !== null) { 
            //if(this.filteredRowCount === this.state.selectedChargeScheduleDetails.length){ // 789 Selection Issue
                this.props.actions.UpdateSelectedSchedule(e.rowIndex); //Changes for D1039  
            //}
            this.bindChargeScheduleDetail();            
            if(this.previousIndex !== e.rowIndex && this.filteredRowCount >= this.state.selectedChargeScheduleDetails.length){ //Changes for D1217
                if(this.gridChargeSchedule.gridApi){
                    this.gridChargeSchedule.gridApi.setRowData(this.state.selectedChargeScheduleDetails); 
                }
            }
            this.previousIndex = e.rowIndex;
            const isReadOnly = (this.props.interactionMode || this.props.pageMode=== localConstant.commonConstants.VIEW || this.props.isScheduleBatchProcess ? true : false);
            if(!isReadOnly) {
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

    addChargeSchedule = (e) => {
        e.preventDefault();        
        this.updatedData = {};
        this.updatedData["recordStatus"] = "N";
        if(!isEmpty(this.props.selectedContract.contractNumber)){
            this.updatedData["contractNumber"]=this.props.selectedContract.contractNumber;
        }
        this.updatedData["scheduleId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
        this.updatedData["baseScheduleName"] = null;
        this.updatedData["scheduleName"] = "";
        this.updatedData["scheduleNameValidation"] = localConstant.validationMessage.SCHEDULE_NAME_VAL;
        this.updatedData["chargeCurrencyValidation"] = localConstant.validationMessage.CHARGE_CURRENCY_VAL;
        this.updatedData["createdBy"] = this.props.loggedInUser;
        if(this.state.orderSequenceId >= 1 ){
            this.updatedData["sequenceId"] = this.state.orderSequenceId + 1;
            this.setState({
                orderSequenceId:this.updatedData["sequenceId"]
            });
        }
        this.updatedData["canBeDeleted"] = true;
        this.updatedData.isSelected = true;
        this.props.actions.AddRateSchedule(this.updatedData);        
        sessionStorage.setItem('selectedSchedule', JSON.stringify(this.updatedData)); //Added for D789
        this.setState({ 
            SelectedRateSchedule: this.updatedData
        });
        this.state.selectedChargeScheduleDetails.forEach((itratedValue) => {            
            itratedValue.isSelected= false;
        });        
        this.state.selectedChargeScheduleDetails.unshift(this.updatedData);
        this.gridChargeSchedule.gridApi.setRowData(this.state.selectedChargeScheduleDetails);     
        this.gridChargeSchedule.gridApi.setFocusedCell(0, "scheduleName"); 
        this.setState((state) => {
            return {
                selectedChargeRateDetails: []
            };
        });                
    }

    render() {
        const { currency, scheduleBatch }  = this.props;
        const chargeTypes  =  isEmptyReturnDefault(this.props.chargeTypes);
        const batchMessage = scheduleBatch.batchStatus;

        //const scheduleOrder=arrayUtil.bubbleSort(this.props.rateSchedule, 'sequenceId');
        // const rateSchedules = arrayUtil.gridTopSort(scheduleOrder);
        // if(rateSchedules){
        //     if(rateSchedules.length > 0){
        //         rateSchedules.map((itratedValue,index)=>{
        //             if (isEmpty(this.state.SelectedRateSchedule.scheduleName)) {
        //                 rateSchedules[0]["isSelected"] = true;
        //             }
        //             else if(itratedValue.scheduleName === this.state.SelectedRateSchedule.scheduleName){
        //                 rateSchedules[index]["isSelected"] = true;
        //             }
        //             else{
        //                 rateSchedules[index]["isSelected"] = false;
        //             }
        //             if(this.state.isDeleteScheduleModalOpen && rateSchedules.length > 1){
        //                 rateSchedules[index]["isSelected"] = false;
        //             }                    
        //         });                
        //     }
        // }
        
        // let chargeType = [];
        // chargeTypes && chargeTypes.map(iteratedValue => {
        //     if (iteratedValue.scheduleName === this.state.SelectedRateSchedule.scheduleName && iteratedValue.recordStatus !== "D") {
        //         iteratedValue.chargeValue=parseFloat(iteratedValue.chargeValue).toFixed(4);
        //         iteratedValue.standardValue=parseFloat(iteratedValue.standardValue).toFixed(2);
        //         chargeType.push(iteratedValue);
        //     }
        // });
        
        // const chargeRateOrder=arrayUtil.bubbleSort(chargeType, 'sequenceId');
        // chargeType=arrayUtil.gridTopSort(chargeRateOrder);
      
        // if(rateSchedules){
        //     if(rateSchedules.length > 0){
        //         rateSchedules.forEach((itratedValue,index)=>{
        //             // Added for Defect Id-286
        //             if (index===0 && !this.state.isCopyScheduleModalOpen && !this.state.isDeleteScheduleModalOpen) {
        //                 itratedValue.isSelected = true; 
        //             }
        //             // if (index === 0 && !this.state.isDeleteScheduleModalOpen) {
        //             //     itratedValue.isSelected = true;
        //             // }  
        //             else{
        //                 itratedValue.isSelected= false;
        //             } 
        //         });
        //     }
        // }        
 
        // let contractInfo = [];
        // if (this.props.contractInfo) {
        //     contractInfo = this.props.contractInfo;
        // }

        const modelData = {};
        // const rateScheduleModalData={ ...this.rateScheduleModalData,isShowModal:this.state.isOpen };

        // const defaultRateScheduleSortOrder =[
        //     { "colId": "chargeType",
        //       "sort": "asc" },
        //     { "colId": "effectiveFrom",
        //       "sort": "desc" }            
        // ];

        let selectedScheduleName = this.state.selectedScheduleName;
        selectedScheduleName = isEmpty(selectedScheduleName) ? "" : selectedScheduleName.toLowerCase();

         // 593 - CONTRACT-Export Rates to CSV file name - UAT Testing 
         //start
         const changedFileNameHeaderData = deepCopy(this.headerData);
         if (changedFileNameHeaderData) {
             if (this.state.SelectedRateSchedule) {
                 changedFileNameHeaderData.exportFileName = this.state.SelectedRateSchedule.scheduleName + " " + changedFileNameHeaderData.exportFileName;
             }
         }
        const adminChargeRatesValue=this.props.adminChargeRatesValue && this.props.adminChargeRatesValue.filter((iteratedValue, i) => {
            return iteratedValue.recordStatus !== "D" && i !==0;
        });
         //End
        //Added for Defect 952 -Starts
        if(adminChargeRatesValue && adminChargeRatesValue.length <= 0){
            this.adminRatesHeader.pinnedTopRowData =[];
        }
        else if(this.props.adminChargeRatesValue && this.props.adminChargeRatesValue.length>0){
            this.adminRatesHeader.pinnedTopRowData =[ this.props.adminChargeRatesValue[0] ];
        }
        //Added for Defect 952 -End
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                {this.props.isRateScheduleModalOpen &&
                    <Modal title={this.props.isRateScheduleEdit ? "Edit Rate Schedule":'Add Rate Schedule'} modalId="rateSchedulePopup" formId="rateScheduleForm" modalClass="popup-position" buttons={this.props.isRateScheduleEdit ? this.rateScheduleEditButtons : this.rateScheduleAddButtons} isShowModal={this.props.isRateScheduleModalOpen}>
                        <RateSchedulePopup
                            rateScheduleChange={(e) => this.formInputChangeHandler(e)}
                            currency={ currency }
                            rateScheduleDefaultData={this.props.rateScheduleEditData}
                        />
                    </Modal>}
                {this.props.isChargeTypeModalOpen &&
                    <Modal title={this.props.isChargeTypeEdit ?"Edit Charge Type":"Add Charge Type"} modalId="chargeRatePopup" formId="chargeRateForm" modalClass="popup-position" buttons={this.props.isChargeTypeEdit ? this.chargeTypeEditButtons : this.chargeTypeAddButtons} isShowModal={this.props.isChargeTypeModalOpen}>
                        <ChargeTypePopup
                            chargeTypeChange={(e) => this.formInputChangeHandler(e)}
                            effectiveFromChange={(e) => this.effectiveFromChangeHandler(e)}
                            effectiveToChange={(e) => this.effectiveToChangeHandler(e)}
                            checkNumber={this.checkNumber}
                            decimalWithLimtFormat={this.decimalWithLimtFormat}
                            blurValue={this.blurValue}
                            state={this.state}
                            chargeTypeDefaultData={this.props.chargeTypeEditData}
                            contractChargeTypes={this.props.contractChargeTypes}
                            dateBlurHandler={(e) => this.handleDateBlur(e)}
                            endDateBlurHandler={(e) => this.handleEndDateBlur(e)}   
                        />
                    </Modal>}
                {this.props.isCopyChargeTypeModalOpen &&
                    <Modal title="Charge Type Date" modalId="copyChargeRatePopup" formId="copyChargeRateForm" modalClass="popup-position" buttons={this.copyChargeTypeAddButtons} isShowModal={this.props.isCopyChargeTypeModalOpen}>
                    <CopyChargeTypePopup
                            effectiveFromChange={(e) => this.copyRatesEffectiveFromDateHandler(e)}
                            effectiveToChange={(e) => this.copyRatesEffectiveToDateHandler(e)}
                            dateBlurHandler={(e) => this.handleDateBlur(e)}
                            endDateBlurHandler={(e) => this.handleEndDateBlur(e)}
                            effectiveFromDate={this.state.copyEffectiveFromDate}
                            effectiveToDate = {this.state.copyEffectiveToDate}
                        />
                    </Modal>
                }
                {/* {this.props.isAdminContractRatesModalOpen && 
                    <Modal title="Admin Contract Rates" modalId="adminContractRatesPopup" formId="adminContractRatesForm" modalClass="adminContractRatesModal" buttons={this.adminContractRatesButtons} isShowModal={this.props.isAdminContractRatesModalOpen}>
                        <AdminContractRatePopup 
                            adminSchedules = {this.props.adminSchedules}
                            adminInspectionGroup = {this.props.adminInspectionGroup}
                            adminInspectionType = {this.props.adminInspectionType}
                            adminChargeRates = {this.props.adminChargeRates}
                            adminContractRatesChange = {this.adminContractRatesChangeHandler}
                        />
                        <CopyChargeTypePopup
                            effectiveFromChange={(e) => this.effectiveFromChangeHandler(e)}
                            effectiveToChange={(e) => this.effectiveToChangeHandler(e)}
                            dateBlurHandler={(e) => this.handleDateBlur(e)}
                            endDateBlurHandler={(e) => this.handleEndDateBlur(e)}
                            state={this.state}
                        />
                    </Modal>
                } */}
                   {this.state.isDeleteChargeRatesModalOpen &&
                    <Modal title="Delete Rates" modalId="deleteChargeRatesPopup" formId="deleteChargeRatesForm" modalClass="deleteChargeRates" buttons={this.deleteChargeRatesButtons} isShowModal={this.state.isDeleteChargeRatesModalOpen}>
                        <DeleteChargeRatePopup 
                            state={this.state}
                            effectiveToChange={(e)=>this.effectiveToChangeHandler(e)}
                            endDateBlurHandler={(e)=>this.handleDateBlur(e)}
                        />
                    </Modal>
                }
                {this.state.isViewChargeRatesModalOpen && 
                    <Modal title="Contract Rates" modalId="viewChargeRatesPopup" formId="viewChargeRatesForm" modalClass="popup-position extranetAccountModal" modalContentClass="extranetModalContent" buttons={this.viewChargeRatesButtons} isShowModal={this.state.isViewChargeRatesModalOpen}>
                    {
                        this.viewChargeRates.map(itratedValue =>{
                            return(
                                <ViewChargeRatePopup 
                                scheduleName={itratedValue[0].scheduleName}
                                chargeRates={itratedValue}
                            />
                            );
                        })
                    }
                    </Modal>
                }
                {this.state.isCopyScheduleModalOpen ?
                    <Modal title="Copy Schedule" modalId="copySchedulePopup" formId="copyScheduleForm" buttons={this.copyScheduleButtons} isShowModal={this.state.isCopyScheduleModalOpen}>
                        <CopySchedulePopup
                            schedules={this.state.selectedChargeScheduleDetails}
                            headerData={CopyScheduleHeader}
                            gridRef = {ref => { this.copyScheduleChild = ref; }}
                        />
                    </Modal> : null
                }
                {
                    this.state.isDeleteScheduleModalOpen ? 
                    <Modal title="Delete Schedule" modalId="DeleteSchedulePopup" formId="DeleteScheduleForm" buttons={this.DeleteScheduleButtons} isShowModal={this.state.isDeleteScheduleModalOpen}>
                    <CopySchedulePopup
                        schedules={this.state.selectedChargeScheduleDetails}
                        headerData={DeleteScheduleHeader}
                        gridRef = {ref => { this.deleteScheduleChild = ref; }}
                    />
                </Modal>
                :null
                }
                  {this.state.isAddScheduletoRFCModelOpen?
                 <Modal title="Add schedule to related framework" modalId="addScheduleToRelatedFrameworkPopup" formId="rfCopyForm" buttons={this.addScheduleToRFbuttons} isShowModal={this.state.isAddScheduletoRFCModelOpen}>
                 <CopySchedulePopup
                     schedules={this.state.selectedChargeScheduleDetails}
                     headerData={CopyScheduleHeader}
                     gridRef = {ref => { this.copyScheduleChild = ref; }}
                 />
             </Modal> 
                :null}
                <div className="customCard">
                    { 
                        <span className={ 'right dashboardInfo p-relative' }>
                            &nbsp;&nbsp;{ batchMessage}
                        </span>
                    }
                     <h6 className="label-bold">{localConstant.contract.rateSchedule.CHARGE_SCHEDULES} </h6>
                    
                        <div className="customCard">
                            <ReactGrid gridRowData={this.state.selectedChargeScheduleDetails.filter(x=> x.recordStatus !=="D")} 
                                       gridColData={RateScheduleNameHeader}
                                       rowSelected={this.RateScheduleOnSelectHandler} 
                                       onCellFocused={this.onScheduleCellFocused}
                                       onCurrentPageChanged={this.props.selectedRowIndex}
                                       paginationPrefixId={localConstant.paginationPrefixIds.rateSchedule}
                                       //filterGrid={(e,data)=>this.filterGrid(e,data)} //commented for def 789 (scroll issue)
                                       clearFilters ={this.clearFilters}
                                       onRef={ref => { this.gridChargeSchedule = ref; }}/>                                      
                        </div>
                        {this.props.pageMode!==localConstant.commonConstants.VIEW && <div className="right-align mt-2 col s12 add-text">                            
                            <a onClick={this.addChargeSchedule} className="btn-small waves-effect" disabled={this.props.interactionMode || this.props.isScheduleBatchProcess ? true : false}>{localConstant.contract.common.ADD}</a>
                            {/* <a onClick={this.RateScheduleEditHandler} disabled={selectedScheduleName === "select" || this.state.selectedScheduleName === "" || this.props.interactionMode || !this.props.rateSchedule ? true : false} className="btn-small btn-primary mr-1 editTxtColor customCreateDiv waves-effect">{localConstant.contract.common.EDIT}</a> */}
                           {/* Fix for D789 */}
                            <a className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" href="#confirmation_Modal" onClick={this.deleteRateScheduleHandler} disabled={this.props.interactionMode || this.props.isScheduleBatchProcess ? true : false}>{localConstant.contract.common.DELETE}</a>
                            <a onClick={this.copyRateScheduleHandler} disabled={this.props.interactionMode || this.props.isScheduleBatchProcess ? true : false} className={this.props.currentPage === "Create Contract" ?"hide":"btn-small btn-primary ml-2 waves-effect"}>{localConstant.contract.rateSchedule.COPY_SCHEDULE}</a>
                            { this.props.contractInfo && this.props.contractInfo.contractType === "FRW" ?
                            /*AddScheduletoRF*/
                                 <a onClick={this.addScheduleToRelatedConfirmation} disabled={this.props.interactionMode || this.props.isScheduleBatchProcess ? true : false} className={this.props.currentPage === "Create Contract" ?"hide":"btn-small btn-primary ml-2 waves-effect"}>{localConstant.contract.rateSchedule.ADD_SCHEDULE_TO_RELATED_FRAMEWORK}</a>:null
                            }
                        </div>}
                    {/* comment started for D-262  */}
                    {/* <h6 className="bold" >{localConstant.contract.rateSchedule.RATE_SCHEDULES}</h6>
                    <div className="row mb-2">
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.contract.rateSchedule.SCHEDULE_NAME}
                            type='select'
                            colSize='s4 m4'
                            className="browser-default customInputs"
                            optionsList={rateSchedules}
                            optionName='scheduleName'
                            optionValue="scheduleName"
                            id="scheduleName"
                            onSelectChange={(e) => this.RateScheduleOnchangeHandler(e)}
                            // disabled = {this.props.interactionMode}
                            defaultValue={this.state.SelectedRateSchedule.scheduleName}
                        />
                      
                        <div className="col s8">
                            <a onClick={this.RateScheduleCreateHandler} className="btn-small btn-primary mr-1 customCreateDiv waves-effect" disabled={this.props.interactionMode ? true : false}>{localConstant.contract.common.ADD}</a>
                            <a onClick={this.RateScheduleEditHandler} disabled={selectedScheduleName === "select" || this.state.selectedScheduleName === "" || this.props.interactionMode || !this.props.rateSchedule ? true : false} className="btn-small btn-primary mr-1 editTxtColor customCreateDiv waves-effect">{localConstant.contract.common.EDIT}</a>
                            <a className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" href="#confirmation_Modal" onClick={this.RateScheduleDeleteHandler} disabled={this.state.selectedScheduleName === "select" || this.state.selectedScheduleName === "Select" || this.state.selectedScheduleName === "" || this.props.interactionMode ? true : false}>{localConstant.contract.common.DELETE}</a>
                            <a onClick={this.copyRateScheduleHandler} disabled={selectedScheduleName === "select" || this.state.selectedScheduleName === "" || this.props.interactionMode ? true : false} className={this.props.currentPage === "Create Contract" ?"hide":"btn-small btn-primary mr-1 editTxtColor customCreateDiv waves-effect"}>{localConstant.contract.rateSchedule.COPY_SCHEDULE}</a>
                            { this.props.contractInfo && this.props.contractInfo.contractType === "FRW" ?
                                <a onClick={this.addScheduleToRelatedFramework} disabled={selectedScheduleName === "select" || this.state.selectedScheduleName === "" || this.props.interactionMode ? true : false} className={this.props.currentPage === "Create Contract" ?"hide":"btn-small btn-primary mr-1 editTxtColor customCreateDiv waves-effect"}>{localConstant.contract.rateSchedule.ADD_SCHEDULE_TO_RELATED_FRAMEWORK}</a>:null
                            }
                        </div>}
                    </div>

                    <div className="row mb-1">
                        <div className="col s5 labelPrimaryColor">
                            <span className="bold">{localConstant.contract.rateSchedule.NAME_PRINTED_ON_INVOICE} :</span> {this.props.rateSchedule ? this.state.SelectedRateSchedule.scheduleNameForInvoicePrint:null} |
                        <span className="bold">{localConstant.contract.rateSchedule.CURRENCY} : </span>{this.props.rateSchedule ? this.state.SelectedRateSchedule.chargeCurrency:null}
                        </div>
                    </div> */}
                     {/* comment ended for D-262  */}
                     {this.props.pageMode!==localConstant.commonConstants.VIEW && <div className="right-align mt-2 col s12 add-text">
                            <a onClick={this.adminContractRatesHandler} className="btn-small ml-2 waves-effect" disabled={selectedScheduleName === "select" || this.state.selectedScheduleName === "" || this.props.interactionMode || this.props.isScheduleBatchProcess ? true : false}>{localConstant.contract.rateSchedule.USE_ADMIN_CONTRACT_RATES}</a>
                            <a onClick={this.deleteChargeRatesHandler} className={this.props.currentPage === "Create Contract"?"hide":"btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn"} disabled={chargeTypes.length <= 0 || this.props.interactionMode || this.props.isScheduleBatchProcess ? true : false}>{localConstant.contract.rateSchedule.DELETE_RATES}</a>
                    </div>}
                    {this.props.isAdminContractRatesModalOpen ?
                        <CardPanel className="white lighten-4 black-text" title="Admin Contract Rates" colSize="s12">
                            <div className="row">
                                <AdminContractRatePopup
                                    adminSchedules={this.props.adminSchedules}
                                    adminInspectionGroup={this.props.adminInspectionGroup}
                                    adminInspectionType={this.props.adminInspectionType}
                                    adminChargeRates={adminChargeRatesValue}
                                    adminContractRatesChange={this.adminContractRatesChangeHandler}
                                    adminChargeScheduleValueChange={this.props.adminChargeScheduleValueChange}
                                    selectAllOnShoresHandler={this.selectAllOnShoresHandler}
                                    AdminContractRatesHeader={this.adminRatesHeader}
                                    gridRef = {ref => { this.secondChild = ref; }}
                                />
                                <CopyChargeTypePopup
                                    effectiveFromChange={(e) => this.adminEffectiveFromDateHandler(e)}
                                    effectiveToChange={(e) => this.adminEffectiveToDateHandler(e)}
                                    dateBlurHandler={(e) => this.handleDateBlur(e)}
                                    endDateBlurHandler={(e) => this.handleEndDateBlur(e)}
                                    effectiveFromDate = {this.state.adminEffectiveFromDate}
                                    effectiveToDate = {this.state.adminEffectiveToDate}
                                />
                                <div className="right-align mt-2 col s12 add-text">
                                    {/* <a onClick={this.cancelAdminContractRates} className="waves-effect btn-small ml-2" disabled={selectedScheduleName === "select" || this.state.selectedScheduleName === "" || this.props.interactionMode ? true : false}>{localConstant.commonConstants.CANCEL}</a> */}
                                    <a onClick={this.addAdminContractRates} className="btn-small ml-2 waves-effect" disabled={selectedScheduleName === "select" || this.state.selectedScheduleName === "" || this.props.interactionMode || this.props.isScheduleBatchProcess ? true : false}>{localConstant.commonConstants.SUBMIT}</a>
                                </div>
                            </div>
                        </CardPanel> : null
                    }
                    <span className='right dashboardInfo'>&nbsp;&nbsp;Charge Type - Items in RED are for NDT use only (Consumables and Equipment)</span>
                    
                    <h6 className="label-bold">{localConstant.contract.rateSchedule.CHARGE_RATES} </h6>
                    <div className="chargeRateGrid"> 
                        <ReactGrid 
                            gridRowData={this.state.selectedChargeRateDetails} 
                            gridColData={changedFileNameHeaderData} 
                            onRef={ref => { this.child = ref; }}
                            gridTwoPagination={true} 
                            // columnPrioritySort={defaultRateScheduleSortOrder}
                            onCellFocused={this.onCellFocused}
                            paginationPrefixId={localConstant.paginationPrefixIds.chargeRates}
                        />                           
                    </div>
                    {this.props.pageMode!== localConstant.commonConstants.VIEW && <div className="right-align mt-2 col s12 add-text">
                        <a onClick={this.addChargeRate} className="btn-small waves-effect" disabled={selectedScheduleName === "select" || this.state.selectedScheduleName === "" || this.props.interactionMode || this.props.isScheduleBatchProcess ? true : false}>{localConstant.commonConstants.ADD}</a>                            
                        <a href="#confirmation_Modal" onClick={this.chargeRateDeleteHandler} className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" disabled={selectedScheduleName === "select" || this.state.selectedScheduleName === '' || this.state.selectedChargeRateDetails.length <= 0 || this.props.interactionMode || this.props.isScheduleBatchProcess ? true : false}>{localConstant.commonConstants.DELETE}</a>
                        <a onClick={this.copyChargeRateHandler} className={this.props.currentPage === "Create Contract"?"hide":"btn-small ml-2 waves-effect"} disabled={selectedScheduleName === "select" || this.state.selectedScheduleName === "" || this.state.selectedChargeRateDetails.length <= 0 || this.props.interactionMode || this.props.isScheduleBatchProcess ? true : false}>{localConstant.contract.rateSchedule.COPY_CHARGETYPE}</a>
                    </div> }
                </div>
            </Fragment>
        );
    }
}

RateSchedule.propTypes = {
    // rateSchedule: PropTypes.array.isRequired,  //  not used
    // SelectedScheduleName: PropTypes.string.isRequired  // not used
};

RateSchedule.defaultprops = {
    // rateSchedule: [],  //  not used
    // SelectedScheduleName: "",  //  not used
};

export default RateSchedule;