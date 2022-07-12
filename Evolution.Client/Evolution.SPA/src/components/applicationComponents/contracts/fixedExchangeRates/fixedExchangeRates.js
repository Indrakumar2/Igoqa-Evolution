import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData.js';
import PropTypes from 'proptypes';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import LabelwithValue from '../../../../common/baseComponents/customLabelwithValue';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, numberFormat } from '../../../../utils/commonUtils';
import Modal from '../../../../common/baseComponents/modal';
import { withRouter } from 'react-router-dom';
import moment from 'moment';
import dateUtil from '../../../../utils/dateUtil';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import CustomModal from '../../../../common/baseComponents/customModal';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { isEmpty,formatToDecimal } from '../../../../utils/commonUtils';
import { required } from '../../../../utils/validator';
const localConstant = getlocalizeData();

const FixedExchangeModal = (props) => {
    return (
        <div>
            <CustomInput
                hasLabel={true}
                type='select'
                divClassName='col loadedDivision'
                label="From Currency" colSize='s3'
                optionsList={props.CurrencyData}
                optionName='code'
                optionValue="code"
                labelClass="mandate"
                name="fromCurrency"
                defaultValue={props.selectedRow.fromCurrency}
                className="browser-default customInputs"
                onSelectChange={props.onChange}
            />
            <CustomInput
                hasLabel={true}
                divClassName='col loadedDivision'
                type='select'
                label="To Currency"
                colSize='s3'
                name="toCurrency"
                labelClass="mandate"
                className="browser-default customInputs"
                required={true}
                defaultValue={props.selectedRow.toCurrency}
                optionsList={props.CurrencyData}
                optionName="code"
                optionValue="code"
                onSelectChange={props.onChange}
            />
            <CustomInput
                hasLabel={true}
                isNonEditDateField={false}
                divClassName='col loadedDivision'
                type='date'
                label="Effective Date"
                colSize='s3'
                htmlFor="EffectiveDate"
                name="effectiveFrom"
                defaultValue={props.selectedRow.effectiveFrom}
                onDatePickBlur={props.handleStartDateBlur}
                labelClass=" customLabel mandate"
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                required={true}
                selectedDate={props.fixedDate}
                onDateChange={props.DateVariable}
                shouldCloseOnSelect={true}
            />
            <CustomInput
                hasLabel={true}
                label="Exchange Rate"
                labelClass="  mandate"
                divClassName="s3"
                type='text'
                dataType='decimal'
                isLimitType={true}
                prefixLimit={6}
                suffixLimit={6}
                name="exchangeRate"
                maxLength={16}
                defaultValue={props.exchangeRate && formatToDecimal(props.exchangeRate,6)}
                required={true}
                colSize='s3'
                readOnly={props.interactionMode}
                inputClass="customInputs"
                onValueBlur={props.onChange}
            />
        </div>
    );
};

class FixedExchangeRates extends Component {
    constructor(props) {
        super(props);
        this.updatedData = {};
        this.state = {
            isOpen: false,
            useContractExchangeRate: false,
            FixedExchangeDate: '',
            inValidStartDate: false
        };
        this.confirmationModalData = {
            title: "",
            message: "",
            type: "",
            modalClassName: "",
            buttons: []
        };
        this.createExchangeRate = this.createExchangeRate.bind(this);

        this.exchangeRateAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelExchangeRate,
                btnID: "cancelExchangeRate",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.createExchangeRate,
                btnID: "createExchangeRate",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];

        this.exchangeRateReferenceEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelExchangeRate,
                btnID: "cancelExchangeRate",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.editExchangeRate,
                btnID: "editExchangeRate",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
    }
    componentWillReceiveProps(nextProps) {
        if (nextProps.isExchangeRateEdit) {
            this.setState({
                FixedExchangeDate: moment(nextProps.editFixedExchangeDetails.effectiveFrom),
            });
        } else {
            this.setState({
                FixedExchangeDate: '',
            });
        }
    }
    handlerChange = (e) => {
        //e.preventDefault();
        if (e.target.type == "text") {
            const v = parseFloat(e.target.value);
            e.target.value = (isNaN(v)) ? '' : v.toFixed(6);
            // // //     //e.target.value = numberFormat(e.target.value);
        }
        this.updatedData[e.target.name] = e.target.value;
    }
 
    /**
  * Closing the popup of Invoice Reference
  */
    cancelExchangeRate = (e) => {
        e.preventDefault();
        this.setState({
            inValidStartDate: false
        });
        this.props.actions.ExchangeRateModalState(false);
        this.updatedData = {};
    }

    handleFixedExchangeDateChange = (date) => {
        this.setState({
            FixedExchangeDate: date
        });
    }

    handleStartDateBlur = (e) => {
        if (e && e.target !== undefined) {
            const isValid = dateUtil.isUIValidDate(e.target.value);
            if (!isValid) {
                IntertekToaster(localConstant.contract.fixedExchangeRate.EFFECTIVE_DATE, 'warningToast ferEffectiveDateReq');
                this.setState({ inValidStartDate: true });
            } else {
                this.setState({ inValidStartDate: false });
            }
        }

    }

    // handleStartDateBlur = (e) => {
    //     let formatDate = '',data = undefined;
    //     if (e && e["isValid"] && e.isValid()) {
    //         formatDate = e.format("DD-MM-YYYY");
    //         data = e;
    //     }
    //     else if (e && e.target) {
    //         formatDate = e.target.value;
    //         // const array = e.target.value.split('-');
    //         // const momentDate = array[2]+'-'+array[1]+'-'+array[0];
    //         // const momentDate = new Date("15-05-2018".replace( /(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"))
    //         // data = moment(momentDate);
    //     }
    //     const isValid = dateUtil.isValidDate(formatDate);
    //     if (!isValid) {
    //         IntertekToaster(localConstant.contract.fixedExchangeRate.EFFECTIVE_DATE, 'warningToast ferEffectiveDateReq');
    //         this.setState({ inValidStartDate: true,FixedExchangeDate:'' });
    //     } else {
    //         this.setState({ inValidStartDate: false,FixedExchangeDate:data ? data:'' });
    //         }
    // }
    useContractExchangeRateHandler = (e) => {
        this.props.actions.UseContractExchangeRate(e.target.checked);
    }

    onClearData = () => {
        document.getElementById("fixedExform").reset();
    }
    deleteSelected = () => {
        const selectedData = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedData);
        this.props.actions.DeleteFixedExchangeRate(selectedData);
        this.props.actions.HideModal();
        this.forceUpdate();
    }
    /**
     * Exchange OnClick Handler
     */
    ExchangeRateCreateHandler = () => {
        this.props.actions.ExchangeRateEditCheck(false);
        this.props.actions.ExchangeRateModalState(true);
    }
    createExchangeRate = (e) => {
        e.preventDefault();
        let alreadySelected = false;
        const NewFixedExchangeDate = this.state.FixedExchangeDate;
        if (this.updatedData.fromCurrency === undefined || this.updatedData.fromCurrency === "") {
            IntertekToaster(localConstant.contract.PLEASE_SELECT_FROM_CURRENCY, 'warningToast ferFromCurrencyReq');
            return false;
        }
        if (this.updatedData.toCurrency === undefined || this.updatedData.toCurrency === "") {
            IntertekToaster(localConstant.contract.PLEASE_SELECT_TO_CURRENCY, 'warningToast ferToCurrencyReq');
            return false;
        }

        if (isEmpty(NewFixedExchangeDate)) {
            IntertekToaster(localConstant.contract.PLEASE_SELECT_EFFECTIVE_DATE, 'warningToast ferEffectiveFromReq');
            return false;
        }
        // if (this.state.inValidStartDate) {
        //     IntertekToaster('Please Enter Valid Effective Date', 'warningToast ferValidEffectiveDateReq');
        //     return false;
        // }

        if (isEmpty(this.updatedData.exchangeRate)) {
            IntertekToaster(localConstant.contract.PLEASE_ENTER_EXCHANGE_RATE, 'warningToast ferExchangeRateReq');
            return false;
        }
        
        // if (this.updatedData.exchangeRate && parseInt(this.updatedData.exchangeRate, 10) <= 0) {
        //     IntertekToaster(localConstant.contract.PLEASE_ENTER_EXCHANGE_RATE_GREATER_THAN_ZERO, 'warningToast ferExchangeRateReq');
        //     return false;
        // }

        // if (!isEmpty(this.updatedData.exchangeRate)) {
        //     const exchangeRate = this.updatedData.exchangeRate.toString();
        //     const decimalData = (exchangeRate.split("."));
        //     if (decimalData.length === 1) {
        //         decimalData.push('00');
        //     }
        //     if (decimalData.length > 2) {
        //         IntertekToaster(localConstant.contract.rateSchedule.INVALID_DECIMAL, 'warningToast ferInvalidDecimalReq');
        //         return false;
        //     }
        //     if (decimalData[1].length > 6) {
        //         IntertekToaster('Only 6 digits is allowed after decimal', 'warningToast ferDecimalDigitsReq');
        //         return false;
        //     }
        // } 

        if (this.updatedData.fromCurrency === this.updatedData.toCurrency) {
            IntertekToaster(localConstant.contract.BOTH_CURRENCIES_CANNOT_BE_SAME, 'warningToast ferCurrencySameReq');
            return false;
        }

        else {
            if (this.props.ContractFixedRate) {
                this.props.ContractFixedRate.map(result => {
                    const effectiveFrom = dateUtil.formatDate(result.effectiveFrom, '-');
                    const fixedExchangeDate = NewFixedExchangeDate.format();
                    const fixedExchangeDateFormated = dateUtil.formatDate(fixedExchangeDate, '-');
                    if ((result.fromCurrency === this.updatedData.fromCurrency) && (result.toCurrency === this.updatedData.toCurrency) && (effectiveFrom === fixedExchangeDateFormated)) {
                        if (result.recordStatus !== 'D') {
                            alreadySelected = true;
                        }
                        else {
                            this.updatedData = result;
                        }
                    }
                });
            }
            if (alreadySelected === true) {
                IntertekToaster(localConstant.contract.EXCHANGE_RATE_ALREADY_EXISTS, 'warningToast ferExchangeRateExistReq');
            }
            else {
                this.updatedData["recordStatus"] = "N";
                this.updatedData["createdBy"] = this.props.loggedInUser;
                this.updatedData['effectiveFrom'] = NewFixedExchangeDate.format();
                this.updatedData["exchangeRateId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                if (this.props.selectedContract.contractNumber && this.props.selectedContract.contractNumber != null) {
                    this.updatedData["contractNumber"] = this.props.selectedContract.contractNumber;
                }
                else {
                    this.updatedData["contractNumber"] = null;
                }
                //this.updatedData["contractId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                this.props.actions.AddFixedExchangeRate(this.updatedData);
                this.props.actions.ExchangeRateModalState(false);
                this.updatedData = {};
                this.onClearData();
            }

        }
    }

    editExchangeRate = (e) => {
        let alreadySelected = false;
        e.preventDefault();
        const NewFixedExchangeDate = this.state.FixedExchangeDate;
        const editedRow = Object.assign({}, this.props.editFixedExchangeDetails, this.updatedData);
        if (isEmpty(editedRow.fromCurrency)) {
            IntertekToaster(localConstant.contract.PLEASE_SELECT_FROM_CURRENCY, 'warningToast ferFromCurrency');
            return false;
        }
        if (isEmpty(editedRow.toCurrency)) {
            IntertekToaster(localConstant.contract.PLEASE_SELECT_TO_CURRENCY, 'warningToast ferToCurrency');
            return false;
        }
        if (isEmpty(NewFixedExchangeDate)) {
            IntertekToaster(localConstant.contract.PLEASE_SELECT_EFFECTIVE_DATE, 'warningToast ferEffectiveFromReq');
            return false;
        }
        // if (this.state.inValidStartDate) {
        //     IntertekToaster('Please Enter Valid Effective Date', 'warningToast ferValidEffectiveDate');
        //     return false;
        // }
        if (required(editedRow.exchangeRate)) {
            IntertekToaster(localConstant.contract.PLEASE_ENTER_EXCHANGE_RATE, 'warningToast ferExchangeRateReq');
            return false;
        }
        // if (editedRow.exchangeRate && parseInt(editedRow.exchangeRate, 10) <= 0) {
        //     IntertekToaster(localConstant.contract.PLEASE_ENTER_EXCHANGE_RATE_GREATER_THAN_ZERO, 'warningToast ferExchangeRateReq');
        //     return false;
        // }
        // if (!isEmpty(editedRow.exchangeRate)) {
        //     const exchangeRate = editedRow.exchangeRate.toString();
        //     const decimalData = (exchangeRate.split("."));
        //     if (decimalData.length === 1) {
        //         decimalData.push('00');
        //     }
        //     if (decimalData.length > 2) {
        //         IntertekToaster(localConstant.contract.rateSchedule.INVALID_DECIMAL, 'warningToast fetInvalidDecimalReq');
        //         return false;
        //     }
        //     if (decimalData[1].length > 6) {
        //         IntertekToaster('Only 6 digits is allowed after decimal', 'warningToast ferDecimalLength');
        //         return false;
        //     }
        // }
        if (editedRow.fromCurrency === editedRow.toCurrency) {
            IntertekToaster(localConstant.contract.BOTH_CURRENCIES_CANNOT_BE_SAME, 'warningToast ferSameCurrencyVal');
            return false;
        }

        else {
            if (this.props.ContractFixedRate) {
                this.props.ContractFixedRate.map(result => {
                    const effectiveFrom = dateUtil.formatDate(result.effectiveFrom, '-');
                    const fixedExchangeDate = NewFixedExchangeDate.format();
                    const fixedExchangeDateFormated = dateUtil.formatDate(fixedExchangeDate, '-');
                    if (result.exchangeRateId !== editedRow.exchangeRateId) {
                        if ((result.fromCurrency === editedRow.fromCurrency) && (result.toCurrency === editedRow.toCurrency) && (effectiveFrom === fixedExchangeDateFormated)) {
                            if (result.recordStatus !== 'D') {
                                alreadySelected = true;
                            }
                            else {
                                this.updatedData = result;
                            }
                        }
                    }
                });
            }
            if (alreadySelected === true) {
                IntertekToaster(localConstant.contract.EXCHANGE_RATE_ALREADY_EXISTS, 'warningToast ferAlreadySelected');
            }
            else {
                if (this.props.editFixedExchangeDetails.recordStatus !== "N") {
                    this.updatedData["recordStatus"] = "M";
                }
                this.updatedData["modifiedBy"] = this.props.loggedInUser;
                if (this.props.selectedContract && this.props.selectedContract.contractNumber) {
                    this.updatedData["contractNumber"] = this.props.selectedContract.contractNumber;
                }
                this.updatedData['effectiveFrom'] =NewFixedExchangeDate.format();
                this.props.actions.UpdateFixedExchangeRate(this.updatedData);
                this.props.actions.ExchangeRateModalState(false);
                this.updatedData = {};
                this.onClearData();
            }
        }
    }
    FixedExchangeDeleteClickHandler = () => {
        const selectedRecords = this.child.getSelectedRows();

        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.FIXED_EXCHANGE_RATE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: "Yes",
                        onClickHandler: this.deleteSelected,
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
        else {
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE, 'warningToast ferSelectRowToDelete');
        }
    }
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    render() {
        const { showButton, editFixedExchangeDetails, interactionMode, currencyData, ContractFixedRate, useContractExchangeRate } = this.props;
        let FixedExchangeDetails = [];
        let contractInfo = [];
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };

        //    FixedExchangeDetails = this.props.ContractFixedRate && this.props.ContractFixedRate.filter(rate => rate.recordStatus !== "D");

        if (ContractFixedRate && useContractExchangeRate.isFixedExchangeRateUsed) {
            FixedExchangeDetails = ContractFixedRate.filter(margin => margin.recordStatus !== "D");
        }
        if (useContractExchangeRate) {
            contractInfo = useContractExchangeRate;
        }

        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                {this.props.isExchangeRateModalOpen ? <Modal title={this.props.isExchangeRateEdit ? "Edit Fixed Exchange Rates" : ' Add Fixed Exchange Rates'} modalId="modalid" formId="fixedExform" modalClass="popup-position" buttons={this.props.isExchangeRateEdit ? this.exchangeRateReferenceEditButtons : this.exchangeRateAddButtons} isShowModal={this.props.isExchangeRateModalOpen}>
                    <FixedExchangeModal 
                    handleStartDateBlur={this.handleStartDateBlur} 
                    showButton={showButton} 
                    fixedDate={this.state.FixedExchangeDate} 
                    DateVariable={this.handleFixedExchangeDateChange} 
                    selectedRow={this.props.editFixedExchangeDetails} 
                    exchangeRate={editFixedExchangeDetails.exchangeRate} 
                    CurrencyData={currencyData} 
                    datevaraiable={this.state.datevaraiable} 
                    onChange={this.handlerChange}
                    interactionMode={this.props.interactionMode} />
                </Modal> : null}

                <div className="genralDetailContainer customCard">
                <h6 className="label-bold">{localConstant.contract.FIXED_EXCHANGES_RATES}</h6>                  
                         <div className="customCard"> 
                        {/* <label className="col s12 pl-0">
                            <input id="useContractExchangeRateId" name="isFixedExchangeRateUsed" type="checkbox" className="filled-in" onClick={this.useContractExchangeRateHandler} checked={contractInfo.isFixedExchangeRateUsed} disabled={interactionMode} />
                            <span className="labelPrimaryColor ml-0">{localConstant.contract.fixedExchangeRate.USE_CONTRACT_EXCHANGE_RATE}</span>
                        </label> */}
                        {/* Added for Sanity Defect 72 Start */}
                        <CustomInput hasLabel={false}
                            type='checkbox'
                            checkBoxArray={ [ { label: localConstant.contract.fixedExchangeRate.USE_CONTRACT_EXCHANGE_RATE, value: contractInfo.isFixedExchangeRateUsed } ] }
                            colSize='s12'
                            id="useContractExchangeRateId"
                            name="isFixedExchangeRateUsed"
                            onCheckboxChange={this.useContractExchangeRateHandler}
                            disabled={interactionMode} 
                            />
                        {/* Added for Sanity Defect 72 End */}
                            <ReactGrid gridRowData={FixedExchangeDetails} gridColData={HeaderData.ContractFixedExchangeRatesHeader} onRef={ref => { this.child = ref; }}  paginationPrefixId={localConstant.paginationPrefixIds.fixedExchangeRate}/></div>
                        {this.props.pageMode!== localConstant.commonConstants.VIEW &&<div className="col s12 right-align mt-2">
                            <button onClick={this.ExchangeRateCreateHandler}
                                disabled={(contractInfo.isFixedExchangeRateUsed === true && contractInfo.isFixedExchangeRateUsed !== undefined) ? interactionMode : true}
                                className="btn-small">
                                {localConstant.commonConstants.ADD}</button>
                            <button href="#confirmation_Modal" disabled={(contractInfo.isFixedExchangeRateUsed === true && contractInfo.isFixedExchangeRateUsed !== undefined) ? interactionMode : true} onClick={this.FixedExchangeDeleteClickHandler} className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn">{localConstant.commonConstants.DELETE}</button>
                        </div>}                   
                </div>
            </Fragment>
        );
    }
}

FixedExchangeRates.propTypes = {
    // FixedExchangeRateDetails: PropTypes.array.isRequired // not used
};

FixedExchangeRates.defaultprops = {
    // FixedExchangeRateDetails: [] // not used
};
export default withRouter(FixedExchangeRates);