import React, { Component, Fragment } from 'react';
import { getlocalizeData, isEmpty, numberFormat,thousandFormat,isUndefined,ResetCurrentModuleTabInfo, isEmptyOrUndefine, formatToDecimal } from '../../../../utils/commonUtils';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import LabelwithValue from '../../../../common/baseComponents/customLabelwithValue';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import ContractAnchor from '../../../viewComponents/contracts/contractAnchor';
import CustomerAnchor from '../../../viewComponents/customer/customerAnchor';
import { Link } from 'react-router-dom';
import Modal from '../../../../common/baseComponents/modal';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData.js';
import dateUtil from '../../../../utils/dateUtil';
import { decimalCheck } from '../../../../utils/commonUtils';
import CompanyList from '../../../companyList';
import moment from 'moment';
import CustomModal from '../../../../common/baseComponents/customModal';
import {
    modalMessageConstant,
    modalTitleConstant
} from '../../../../constants/modalConstants';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import LabelWithValue from '../../../../common/baseComponents/customLabelwithValue';
import CustomerAndCountrySearch from '../../../applicationComponents/customerAndCountrySearch';
import { required,requiredNumeric } from '../../../../utils/validator';
import BudgetMonetary from '../../budgetMonetary';
import { number } from 'prop-types';
import { contractDetailTabs } from '../../../viewComponents/contracts/contractDetails/contractTabDetails';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';

const localConstant = getlocalizeData();

const GeneralDetail = (props) => (
    <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.contract.GENERAL_DETAILS} colSize="s12">
        <div className="row mb-0">
            {
                props.currentPage === "Create Contract" ?
                    <div className="col s6" disabled>
                        <label className="customLabel ">{localConstant.contract.CONTRACT_HOLDER}</label>
                        <CompanyList selectedCompany={props.selectedCompany} disabled={true} />
                    </div>
                    :
                    <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.contract.CONTRACT_HOLDER}
                        type='text'
                        dataValType='valueText'
                        colSize='s6 reset-mb-0'
                        inputClass="customInputs disabled"
                        labelClass="mandate"
                        name="contractInvoicingCompanyName"
                        readOnly={true}
                        value={props.contractHoldingCompanyName}
                    />
            } 
             
             { props.currentPage === "Create Contract" ?
            <CustomerAndCountrySearch 
                colSize="col s6 pl-0" 
                name="contractCustomerName" 
                isMandate={ true }
                interactionMode={props.interactionMode}
                contractCustomerChange={props.customerChangeHandler}
                disabled={ props.currentPage === "Create Contract" ? false : true } 
               /> : 
               <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.customer.CUSTOMER_NAME}
                labelClass='mandate'
                type='text'
                dataValType='valueText'
                colSize='s6'
                inputClass="customInputs disabled"
                name="contractNumber"
                maxLength={12}
                readOnly={true}
                disabled={true}
                
        value={props.contractCustomerName}/> }  
            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.contract.CONTRACT_NO}
                type='text'
                dataValType='valueText'
                colSize='s6'
                inputClass="customInputs disabled"
                name="contractNumber"
                maxLength={12}
                readOnly={true}
                value={props.contractNumber ? props.contractNumber : ""}
                onValueChange={props.onGeneralDetailsDataChange}
            />
           
            <CustomInput
            hasLabel={true}
            divClassName='col'
            label={localConstant.contract.CUSTOMER_CONTRACT_NUMBER}
            type='text'
            dataValType='valueText'
            colSize='s6'
            inputClass="customInputs"
            labelClass="mandate"
            name="customerContractNumber"
            maxLength={fieldLengthConstants.Contract.generalDetails.CUSTOMER_CONTRACT_NUMBER_MAXLENGTH}
            readOnly={props.interactionMode}
            // disabled={props.interactionMode}
            value={props.customerContractNumber ? props.customerContractNumber : ""}
            onValueChange={props.onGeneralDetailsDataChange}
        />
      
            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.contract.CUSTOMER_CODE_CRM}
                type='select'
                colSize='s12 m3'
                className="browser-default"
                optionsList={props.CRM}
                labelClass="mandate"
                optionName='name'
                optionValue="value"
                name="isCRM"
                onSelectChange={props.onClickCRM}
                disabled={props.interactionMode}
                defaultValue={props.isCrm}
            />
            {
                props.isCrm === true &&
                <Fragment>
                    <CustomInput
                        hasLabel={true}
                        divClassName='col '
                        label={localConstant.contract.CRM_OPP_REF}
                        labelClass="mandate"
                        type='text'
                        dataType='numeric'
                        valueType='value'
                        maxLength={10}
                        colSize='s12 m3'
                        inputClass="customInputs"
                        min={"0"}
                        autocomplete="off"
                        max={9999999999}
                        name="contractCRMReference"
                        value={!requiredNumeric(props.data.contractCRMReference) ? props.data.contractCRMReference : ""}
                        onValueChange={props.onBlurValue}
                        // disabled={props.interactionMode}
                        readOnly={props.interactionMode}
                    />
                    <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.contract.CRM_CONFLICTS_OF_BID_REVIEW_COMMENTS}
                        type='textarea'
                        colSize='s12 m6'
                        inputClass="customInputs"
                        autocomplete="off"
                        labelClass="mandate"
                        maxLength={fieldLengthConstants.Contract.generalDetails.CRM_CONFLICTS_OF_BID_REVIEW_COMMENTS_MAXLENGTH}
                        // disabled={props.interactionMode}
                        readOnly = {props.interactionMode}
                        name="contractConflictOfInterest"
                        value={props.contractConflictOfInterest ? props.contractConflictOfInterest : ""}
                        onValueChange={props.onGeneralDetailsDataChange}
                    />
                </Fragment>
            }
            {
                props.isCrm === false &&
                <CustomInput
                    hasLabel={true}
                    divClassName={'col'}
                    label={localConstant.contract.WHY}
                    type='text'
                    dataValType='valueText'
                    colSize='s12 m3'
                    labelClass="mandate"
                    inputClass="customInputs"
                    name="contractCRMReason"
                    maxLength={fieldLengthConstants.Contract.generalDetails.CRM_REASON_MAXLENGTH }
                    autocomplete="off"
                    readOnly={props.interactionMode}
                    disabled={props.interactionMode}
                    value={props.contractCRMReason ? props.contractCRMReason : ""}
                    onValueChange={props.onGeneralDetailsDataChange}
                />
            }
        </div>
        <div className="row mb-0">
            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.contract.CONTRACT_TYPE}
                labelClass="mandate"
                type='select'
                colSize='s12 m3'
                className="browser-default"
                optionName='label'
                optionValue="value"
                name="contractType"
                optionsList={props.contractTypeData}
                onSelectChange={props.changeContractType}
                defaultValue={props.contractType ? props.contractType : ""}
                disabled={props.currentPage === "Create Contract" ? false : true}
                refProps ={props.selectRefProps}
            />
            {
                props.contractType === 'PAR' ?
                    <Fragment>
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.contract.PARENT_COMPANY_OFFICE}
                            type='select'
                            colSize='s12 m3'
                            className="browser-default"
                            optionsList={props.companyOffices}
                            labelClass="mandate"
                            optionName='officeName'
                            optionValue="officeName"
                            name="parentCompanyOffice"
                            disabled={props.currentPage === "Create Contract" ? false : true}
                            defaultValue={props.parentCompanyOffice}
                            onSelectChange={props.onGeneralDetailsDataChange}
                        />
                    </Fragment> : null
            }
            {
                props.contractType === 'CHD' ?
                    <Fragment>
                        {
                            props.currentPage === 'Create Contract' ?
                                <CustomInput
                                    hasLabel={true}
                                    divClassName='col'
                                    label={localConstant.contract.PARENT_CONTRACT}
                                    type='select'
                                    colSize='s12 m3'
                                    className="browser-default"
                                    optionsList={props.parentContractNumber}
                                    labelClass="mandate"
                                    optionName='contractNumber'
                                    optionValue="contractNumber"
                                    name="parentContractNumber"
                                    disabled={props.currentPage === "Create Contract" ? false : true}
                                    defaultValue={props.data.parentContractNumber}
                                    onSelectChange={props.confirmParentSelection}
                                /> :
                                <div className="custom-Badge col s3 mt-4">{localConstant.contract.PARENT_CONTRACT}:
                                    <ContractAnchor
                                        data={{ contractNumber: props.data.parentContractNumber , contractHoldingCompanyCode: props.data.parentCompanyCode }} />
                                </div>
                        }
                        <CustomInput
                            hasLabel={true}
                            label={localConstant.contract.PARENT_CONTRACT_DISCOUNT}
                            labelClass="mandate"
                            type='number'
                            dataValType='valueText'
                            divClassName="numerInput"
                            colSize='s12 m3'
                            inputClass="customInputs"
                            min="0"
                            max={100}
                            readOnly={props.currentPage === "Create Contract" ? false : true}
                            name="parentContractDiscount"
                            value={props.data.parentContractDiscount ? props.data.parentContractDiscount : ""} //Sanity Defect 23
                            onValueBlur={props.checkNumber}
                            onValueChange={props.discountLengthCheck}
                        />
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.contract.PARENT_CONTRACT_HOLDER}
                            labelClass=""
                            type='text'
                            dataValType='valueText'
                            colSize='s12 m3'
                            className="browser-default" 
                            inputClass="customInputs" 
                            readOnly={true}                          
                            // disabled={true}
                            // name="parentContractHolder"
                            value={props.parentContractHolder ? props.parentContractHolder : ""}
                            onValueBlur={props.onGeneralDetailsDataChange}
                            onValueChange={props.onGeneralDetailsDataChange}
                        />
                    </Fragment> : null
            }
            {
                props.contractType === 'FRW' ?
                    <Fragment>
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.contract.FRAMEWORK_CONTRACT_OFFICE}
                            type='select'
                            colSize='s12 m3'
                            className="browser-default"
                            optionsList={props.companyOffices}
                            labelClass="mandate"
                            optionName='officeName'
                            optionValue="officeName"
                            name="frameworkCompanyOfficeName"
                            defaultValue={props.data.frameworkCompanyOfficeName}
                            disabled={props.currentPage === "Create Contract" ? false : true}
                            onSelectChange={props.onGeneralDetailsDataChange}
                        />
                    </Fragment> : null
            }
            {
                props.contractType === 'IRF' ?
                    <Fragment>
                        {props.currentPage === 'Create Contract' ?
                            <CustomInput
                                hasLabel={true}
                                divClassName='col'
                                label={localConstant.contract.FRAMEWORK_CONTRACT}
                                type='select'
                                colSize='s12 m3'
                                className="browser-default"
                                optionsList={props.parentContractNumber}
                                labelClass="mandate"
                                optionName='contractNumber'
                                optionValue="contractNumber"
                                name="frameworkContractNumber"
                                disabled={props.currentPage === "Create Contract" ? false : true}
                                defaultValue={props.data.frameworkContractNumber}
                                onSelectChange={props.confirmParentSelection}
                            /> :
                            <div className="custom-Badge col s3 mt-4">{localConstant.contract.FRAMEWORK_CONTRACT}: 
                                    <ContractAnchor
                                    data={{ contractNumber: props.data.frameworkContractNumber , contractHoldingCompanyCode: props.data.frameworkCompanyCode }} />
                            </div>
                        }
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.contract.FRAMEWORK_CONTRACT_HOLDER}
                            labelClass=""
                            type='text'
                            dataValType='valueText'
                            colSize='s12 m6' // 596 - CONTRACT-Framework Contract minor UI issues - UAT Testing
                            className="browser-default"
                            readOnly={true}
                            // disabled={true}
                            name="frameworkContractHolder"
                            inputClass="customInputs"
                            value={props.frameworkContractHolder ? props.frameworkContractHolder : ""}
                            onValueBlur={props.onGeneralDetailsDataChange}
                            onValueChange={props.onGeneralDetailsDataChange}
                        />
                    </Fragment> : null
            }
        </div>
        <div className={props.currentPage === "Create Contract" ? "hide" : "row mb-0 mt-2 ml-0 show"}>
            <label>{localConstant.contract.CUSTOMER_CODE}</label>
            <CustomerAnchor data={{ customerCode:props.contractCustomerCode }}/>
        </div>
    </CardPanel>
);
const OtherDetails = (props) => (
    <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.contract.OTHER_DETAILS} colSize="s12">
        <div className="row mb-0">
            <CustomInput
                hasLabel={true}
                isNonEditDateField={false}
                label={localConstant.contract.START_DATE}
                labelClass="customLabel mandate"
                colSize='s3'
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                onDatePickBlur={props.handleOtherDetailStartDateBlur}
                type='date'
                name='contractStartDate'
                selectedDate={dateUtil.defaultMoment(props.startDate)}
                onDateChange={props.fetchStartDate}
                shouldCloseOnSelect={true}
                disabled={props.interactionMode}
            />
            <CustomInput
                hasLabel={true}
                labelClass={props.contractStatus === "C" ? "mandate" : "customLabel"}
                label={localConstant.contract.END_DATE}
                colSize='s3'
                type="date"
                isNonEditDateField={false}
                selectedDate={isEmpty(props.endDate) ? "" : dateUtil.defaultMoment(props.endDate)}
                onDateChange={props.fetchEndDate}
                onDatePickBlur={props.handleOtherDetailEndDateBlur}
                name='contractEndDate'
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                shouldCloseOnSelect={true}
                disabled={props.interactionMode}
            />
            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.contract.STATUS}
                type='select'
                colSize='s3'
                className="browser-default"
                labelClass="mandate"
                optionsList={props.status}
                optionName='name'
                optionValue='value'
                inputClass="customInputs"
                name="contractStatus"
                disabled={props.interactionMode}
                defaultValue={props.contractStatus } //Sanity Defect Fix
                onSelectChange={props.onGeneralDetailsDataChange}
            />
        </div>
        <div className="row mb-0">
            <CustomInput
                hasLabel={true}
                divClassName='col '
                label={localConstant.contract.CLIENT_REPORTING_REQUIREMENTS}
                type='textarea'
                colSize='s12 mt-4'
                inputClass="customInputs textAreaInView"        
                maxLength={fieldLengthConstants.Contract.generalDetails.CLIENT_REPORTING_REQUIREMENTS_MAXLENGTH}
                name="contractClientReportingRequirement"
                // disabled={props.interactionMode}
                readOnly = {props.interactionMode}
                value={!required(props.contractClientReportingRequirement) ? props.contractClientReportingRequirement : ""}
                onValueChange={props.onGeneralDetailsDataChange}
                //onValueBlur={props.onGeneralDetailsDataChange}
            />
        </div>
        <div className="row mb-0">
            <CustomInput
                hasLabel={true}
                divClassName='col '
                label={localConstant.contract.ASSIGNMENT_INSTRUCTIONAL_OPERATIONAL_NOTES}
                type='textarea'
                colSize='s12'
                inputClass="customInputs textAreaInView"
                name="contractOperationalNote"
                // disabled={props.interactionMode}
                readOnly = {props.interactionMode}
                //maxLength={fieldLengthConstants.Contract.generalDetails.ASSIGNMENT_INSTRUCTIONAL_OPERATIONAL_NOTES_MAXLENGTH}
                value={!required(props.contractOperationalNote) ? props.contractOperationalNote : ""}
                onValueChange={props.onGeneralDetailsDataChange}
                //onValueBlur={props.onGeneralDetailsDataChange}
            />
        </div>
    </CardPanel>
);
const ButtonModal = (props) => (
    <Fragment >
        <div className="row mb-0">
            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.contract.NAME}
                type='text'
                colSize='s6'
                name="customerName"
                inputClass="customInputs"
                onValueKeyDown={props.handlerKeyDown}
                defaultValue={props.customerName ? props.customerName : ""}
                autocomplete="off"
            />
            <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.contract.COUNTRY}
                type='select'
                colSize='s6'
                inputClass="customInputs"
                optionName='name'
                optionValue="name"
                name="operatingCountry"
                optionsList={props.countryMasterData}
                onSelectChange={props.searchCustomer}
                defaultValue={props.operatingCountry}
            />
        </div>
    </Fragment>
);

class GeneralDetails extends Component {
    constructor(props) {
        super(props);
        this.state = {
            startDate: '',
            endDate: '',
            contractType: '',
            currencyWarningPercentage: 75,
            hoursWarningPercentage: 75,
            searchCompanyCode: '',
            searchCompanyName: '',
            tempname:props.generalDetailsData ? props.generalDetailsData.contractCustomerName :''
        };
        this.updatedData = {};
        this.customerCountry = "";
        this.selectedcustomerName = [];
        this.selectRef=React.createRef();
        this.confirmationModalData = {
            title: "",
            message: "",
            type: "",
            modalClassName: "",
            buttons: []
        };
    }

    componentDidMount() {       
        if (this.props.generalDetailsData) {
            if (this.props.generalDetailsData.contractEndDate !== null && this.props.generalDetailsData.contractEndDate !== "") {
                this.setState({
                    startDate: moment(this.props.generalDetailsData.contractStartDate),
                    endDate: moment(this.props.generalDetailsData.contractEndDate)
                });
            }
            else {
                this.setState({
                    startDate: moment(this.props.generalDetailsData.contractStartDate),
                    endDate: ''
                });
            }
        }
        else {
            if (!isEmpty(this.props.selectedData)) {
                if (this.props.selectedData.contractEndDate !== null && this.props.selectedData.contractEndDate !== "") {
                    this.setState({
                        startDate: moment(this.props.selectedData.contractStartDate),
                        endDate: moment(this.props.selectedData.contractEndDate)
                    });
                }
                else {
                    this.setState({
                        startDate: moment(this.props.selectedData.contractStartDate),
                        endDate: ''
                    });
                }
            }
        }
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        if (tabInfo && tabInfo.componentRenderCount === 0) {
        if (this.props.currentPage !== "Create Contract") {
            if (this.props.selectedData && this.props.selectedData.contractType) {
                if (this.props.selectedData.contractType === "PAR" || this.props.selectedData.contractType === "FRW") {
                    this.props.actions.FetchCompanyOffices();
                }
                else if (this.props.selectedData.contractType === "CHD") {
                    this.props.actions.FetchParentContractNumber('PAR', this.props.selectedData.contractCustomerCode);
                }
                else if (this.props.selectedData.contractType === "IRF") {
                    this.props.actions.FetchParentContractNumber('FRW', this.props.selectedData.contractCustomerCode);
                }
            }
            else {
                if (this.props.generalDetailsData) {
                    this.props.actions.FetchContractData()
                        .then(response => {
                            if (response) {
                                this.props.actions.GetSelectedCustomerName(response.ContractInfo);
                                if (response.ContractInfo && (response.ContractInfo.contractType === "PAR" || response.ContractInfo.contractType === "FRW")) {
                                    this.props.actions.FetchCompanyOffices();
                                }
                            }
                        });
                }
            }
        }   
    }      
    }
    componentWillUnmount = () => {
        this.props.actions.CustomerHideModal();
    }
    componentWillReceiveProps = (nextProps) => {
        
         if(nextProps.defaultCustomerName.length>0 )
         {
        if(this.state.tempname!=nextProps.defaultCustomerName[0].customerName)
        {
           this.setState(
               {
                tempname:nextProps.defaultCustomerName[0].customerName
               }
           );
           ResetCurrentModuleTabInfo(contractDetailTabs,[ "InvoicingDefaults" ]); 
            const selectedCustomer=nextProps.defaultCustomerName;
            this.updatedData = {};
            this.props.actions.OnSubmitCustomerName(selectedCustomer);
            this.customerCountry = this.updatedData.operatingCountry;
            this.props.actions.FetchInvoicingDefaultForContract(selectedCustomer[0].customerCode);
            this.props.actions.FetchCustomerContacts(selectedCustomer[0].customerCode)
                .then(response => {
                    if (response === null) {
                        IntertekToaster(modalMessageConstant.CUSTOMER_ADDRESS_CONFIRMATION, 'warningToast');
                    }
                });
            const selectedCompanyCode = this.props.selectedCompany;
            let selectedCompanyName = "";
            this.props.companyList.map(company => {
                if (company.companyCode === selectedCompanyCode) {
                    selectedCompanyName = company.companyName;
                };
            });

            this.updatedData["contractCustomerCode"] = selectedCustomer[0].customerCode;
            this.updatedData["contractCustomerName"] = selectedCustomer[0].customerName;
            this.updatedData["contractHoldingCompanyCode"] = selectedCompanyCode;
            this.updatedData["contractHoldingCompanyName"] = selectedCompanyName;
            // this.updatedData["contractBudgetMonetaryWarning"] = this.state.currencyWarningPercentage; //Commented for defect 936, updated value is getting defaulted here .
            // this.updatedData["contractBudgetHoursWarning"] = this.state.hoursWarningPercentage; //Commented for defect 936, updated value is getting defaulted here .
            this.updatedData["recordStatus"] = "N";
            if (this.props.generalDetailsData === undefined || !this.props.generalDetailsData.contractBudgetHours) {
                this.updatedData["contractBudgetHours"] = 0;
            }
            this.props.actions.AddUpdateGeneralDetails(this.updatedData);     
        }
    }
       
    }

    customerChangeHandler = (data) => {
        const type = this.selectRef.current.value;
        if(this.props.generalDetailsData.contractCustomerCode !== data){
            if(type === 'CHD'){
                this.props.actions.ClearParentContractGeneralDetail();
                this.props.actions.FetchParentContractNumber('PAR', data);
                this.updatedData['contractType'] = 'CHD';
                // this.updatedData["contractBudgetHours"] = 0.00;     //Commented for Defect 936, the value will be updated from parent contract ,no need to send it as 0 from here.
                this.props.actions.AddUpdateGeneralDetails(this.updatedData); 
            }
            else if(type === 'IRF'){
                this.props.actions.ClearParentContractGeneralDetail();
                this.props.actions.FetchParentContractNumber('FRW', data);
                this.updatedData['contractType'] = 'IRF';
                // this.updatedData["contractBudgetHours"] = 0.00; //Commented for Defect 936 , the value will be updated from framework contract , no need to update it as 0 from here.
                this.props.actions.AddUpdateGeneralDetails(this.updatedData);
            } 
            //Changes for Defect 882 -Starts
            else if(type ==="FRW"){
                this.props.actions.FetchInvoicingDefaults(data); //Storing the default Invoicing Data(For "Not Nullable" fields in Database) same like Evolution 
            }
            //Changes for Defect 882 -End
        }
        
    }

    fetchStartDate = (date) => {
        this.setState({
            startDate: date
        }, () => {
            this.updatedData.contractStartDate = this.state.startDate !== null ? this.state.startDate.format() : "";
            this.props.actions.AddUpdateGeneralDetails(this.updatedData);
        });
    }
    fetchEndDate = (date) => {
        const isValid = dateUtil.isUIValidDate(date);
            if (!isValid) {
                if(!moment(date).isAfter((this.state.startDate),'day')){
                    if(!moment(this.state.startDate).isSame(moment(date), 'day')){
                        //IntertekToaster(localConstant.validationMessage.END_DATE_SHOULD_LESSTHAN_STARTDATE, 'warningToast gdEndDateValCheck');
                    }
                }
            }
        this.setState({
            endDate: date
        }, () => {
            this.updatedData.contractEndDate = this.state.endDate !== null ? this.state.endDate.format() : "";
            this.props.actions.AddUpdateGeneralDetails(this.updatedData);
        });
    }
    handleOtherDetailStartDateBlur = (e) => {
        if (e && e.target !== undefined) {
            const isValid = dateUtil.isUIValidDate(e.target.value);//DateValidation
            if (!isValid) {
                IntertekToaster(localConstant.techSpec.common.INVALID_START_DATE, 'warningToast gdStartDateVal');
            }
        }
    }

    handleOtherDetailEndDateBlur = (e) => {
        if (e && e.target !== undefined) {
            const isValid = dateUtil.isUIValidDate(e.target.value);
            if (!isValid) {
                // IntertekToaster("Invalid End Date", 'warningToast gdEndDateVal');
            }
        }
        // if(!moment(this.updatedData.contractEndDate).isAfter((this.state.startDate),'day')){
        //     if(!moment(this.state.startDate).isSame(moment(this.updatedData.contractEndDate), 'day')){
        //         IntertekToaster("End Date Cannot be lesser than or Same as start Date", 'warningToast gdEndDateValCheck');
        //     }
        // }
    }

    onClickCRM = (e) => {
        if (e.target.value === "true") {
            this.updatedData[e.target.name] =true;
            this.props.actions.IfCRMYes();
        }
        else if (e.target.value === "false") {
            this.updatedData[e.target.name] =false;
            this.props.actions.IfCRMNo();
        }
        else {
            this.updatedData[e.target.name] =e.target.value;
            this.props.actions.IfCRMSelect();
            // if (this.props.generalDetailsData) {
            //     this.props.generalDetailsData.contractConflictOfInterest = null;
            //     this.props.generalDetailsData.contractCRMReference = null;
            //     this.props.generalDetailsData.contractCRMReason = null;
            // }
        }
        // this.updatedData[e.target.name] = (e.target.value === "true" ? true : e.target.value === "true" );
        this.props.actions.AddUpdateGeneralDetails(this.updatedData);
    }

    onClickShowModal = (e) => {
        e.preventDefault();
        this.props.actions.CustomerShowModal();
        if (this.props.generalDetailsCreateContractCustomerDetails.customerName) {
            this.updatedData.customerName = this.props.generalDetailsCreateContractCustomerDetails.customerName;
            this.props.actions.FetchCustomerList(this.updatedData);
        }
    }
   
    hideModal = (e) => {
        e.preventDefault();
        const selectedCustomer = this.child.getSelectedRows();
        if (isEmpty(selectedCustomer)) {
            this.props.actions.ClearCustomerList();
        }
        this.props.actions.CustomerHideModal();
        this.updatedData = {};
        this.customerCountry = "";
    }
    blurValue = (e) => {
        if (e.target.value > 99999999999999) {
        }
        else {
            e.target.value = numberFormat(e.target.value);
            this.generalDetailsHandlerChange(e);
        }
    }
    checkNumber = (e) => {
        // if(e.target.value ==="." || e.target.value === 0){
        //     e.target.value="0";
        // }
        if (e.target.value && e.target.value >= 0) {
            e.target.value = parseFloat(numberFormat(e.target.value)).toFixed(2);
            this.generalDetailsHandlerChange(e);
        }
        else {
            e.target.value = "";
            this.generalDetailsHandlerChange(e);
        }
    }

    onWarningBlur =(e) => {
        if(!e.target.value)
        {
            e.target.value = 75;
            this.generalDetailsHandlerChange(e);
        }
    }

    onBlurValue = (e) => {
        if (e.target.value > 9999999999) {
        }
        else {
            e.target.value = numberFormat(e.target.value);
            this.generalDetailsHandlerChange(e);
        }
    }
    discountLengthCheck = (e) => {
        if (e.target.value > 100) { }
        else {
            e.target.value = numberFormat(e.target.value);
            this.generalDetailsHandlerChange(e);
        }
    }

    changeContractType = (e) => {
        // this.updatedData["contractBudgetHours"] = 0.00; //Commented for Defect -936
        this.props.generalDetailsData.parentContractNumber ? this.updatedData.parentContractNumber = null : ''; //Added for Contract Save issue
        this.props.generalDetailsData.parentCompanyOffice ? this.updatedData.parentCompanyOffice = null : '';
        this.props.generalDetailsData.parentContractDiscount ? this.updatedData.parentContractDiscount = null : '';
        this.props.generalDetailsData.parentContractHolder ? this.updatedData.parentContractHolder = null : '';
        this.props.generalDetailsData.frameworkContractNumber ? this.updatedData.frameworkContractNumber = null : '';
        this.props.generalDetailsData.frameworkContractHolder ? this.updatedData.frameworkContractHolder = null : '';
        this.props.generalDetailsData.frameworkCompanyOfficeName ? this.updatedData.frameworkCompanyOfficeName = null : '';
        this.props.actions.AddUpdateGeneralDetails(this.updatedData);
        if (e.target.value === "CHD") {
            if (this.props.currentPage === "Create Contract") {
                if (this.props.generalDetailsCreateContractCustomerDetails) {
                    this.props.actions.FetchParentContractNumber('PAR', this.props.generalDetailsCreateContractCustomerDetails.customerCode);
                }
                else {
                    IntertekToaster(localConstant.contract.CUSTOMER_NAME_VALIDATION, 'warningToast gdCustomerReq');
                   }
            }
            // if (this.props.generalDetailsData) {
            //     this.props.generalDetailsData.parentCompanyOffice = null;
            //     this.props.generalDetailsData.frameworkCompanyOfficeName = null;
            //     this.props.generalDetailsData.frameworkContractNumber = null;
            //     this.props.generalDetailsData.frameworkContractHolder = null;
            // }
        }
        else if (e.target.value === "IRF") {
            if (this.props.currentPage === "Create Contract") {
                if (this.props.generalDetailsCreateContractCustomerDetails) {
                    this.props.actions.FetchParentContractNumber('FRW', this.props.generalDetailsCreateContractCustomerDetails.customerCode);
                }
                else {
                    IntertekToaster(localConstant.contract.CUSTOMER_NAME_VALIDATION, 'warningToast gdCustomerReqq');
                }
            }
            // if (this.props.generalDetailsData) {
            //     this.props.generalDetailsData.parentCompanyOffice = null;
            //     this.props.generalDetailsData.frameworkCompanyOfficeName = null;
            //     this.props.generalDetailsData.parentContractNumber = null;
            //     this.props.generalDetailsData.parentContractDiscount = null;
            //     this.props.generalDetailsData.parentContractHolder = null;
            // }
        }
        else if (e.target.value === "PAR" || e.target.value === "FRW") {
            this.props.actions.FetchCompanyOffices();
            // if (e.target.value === "PAR") {
            //     if (this.props.generalDetailsData) {
            //         this.props.generalDetailsData.frameworkContractNumber = null;
            //         this.props.generalDetailsData.frameworkContractHolder = null;
            //         this.props.generalDetailsData.frameworkCompanyOfficeName = null;
            //         this.props.generalDetailsData.parentContractNumber = null;
            //         this.props.generalDetailsData.parentContractDiscount = null;
            //         this.props.generalDetailsData.parentContractHolder = null;
            //     }
            // }
            if (e.target.value === "FRW") {
                // if (this.props.generalDetailsData) {
                //     this.props.generalDetailsData.frameworkContractNumber = null;
                //     this.props.generalDetailsData.frameworkContractHolder = null;
                //     this.props.generalDetailsData.parentCompanyOffice = null;
                //     this.props.generalDetailsData.parentContractNumber = null;
                //     this.props.generalDetailsData.parentContractDiscount = null;
                //     this.props.generalDetailsData.parentContractHolder = null;
                // }
                this.props.actions.SelectedContractType(e.target.value);
            }
            if (this.props.generalDetailsCreateContractCustomerDetails) {
                this.props.actions.FetchInvoicingDefaults();
            }
            else {
                IntertekToaster(localConstant.contract.CUSTOMER_NAME_VALIDATION, 'warningToast gdCustomerReqqq');
            }
        }
        if (e.target.value !== "FRW") {
            this.props.actions.SelectedContractType("");
        }
        this.setState({
            contractType: e.target.value
        });
        this.generalDetailsHandlerChange(e);
    }

    currencyWarningHandler = (e) => {
        this.setState({
            currencyWarningPercentage: e.target.value
        });
        this.generalDetailsHandlerChange(e);
    }
    hoursWarningHandler = (e) => {
        this.setState({
            hoursWarningPercentage: e.target.value
        });
        this.generalDetailsHandlerChange(e);
    }
    confirmParentSelection = (e) => {
        const selectedParentContractNumber = e.target.value;
        //const contractType = this.state.contractType;
        const contractType = this.selectRef.current.value;
        this.generalDetailsHandlerChange(e);
        if(contractType === "CHD"){
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.COPY_CONFIRMATION,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: () => this.copyParentContractDetails(selectedParentContractNumber, contractType),
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: localConstant.commonConstants.NO,
                        onClickHandler: () => this.confirmationRejectHandler(selectedParentContractNumber,contractType),
                        className: "modal-close m-1 btn-small",
                    }
                ]
            };
            if (selectedParentContractNumber === "Select" || selectedParentContractNumber === "") {
    
            } else {
                this.props.actions.DisplayModal(confirmationObject);
            }
        }
        if(contractType === "IRF"){
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.COPY_FRAMEWORK_CONFIRMATION,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: () => this.copyParentContractDetails(selectedParentContractNumber, contractType),
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: localConstant.commonConstants.NO,
                        onClickHandler: () => this.confirmationRejectHandler(selectedParentContractNumber,contractType),
                        className: "modal-close m-1 btn-small",
                    }
                ]
            };
            if (selectedParentContractNumber === "Select" || selectedParentContractNumber === "") {
    
            } else {
                this.props.actions.DisplayModal(confirmationObject);
            }
        }
    }
    confirmationRejectHandler = (selectedParentContractNumber, contractType) => {
        // const selectedCompanyCode = this.props.selectedCompany;
        // let selectedCompanyName = "";
        // this.props.companyList.map(company => {
        //     if (company.companyCode === selectedCompanyCode) {
        //         selectedCompanyName = company.companyName;
        //     };
        // });
        this.props.actions.FetchParentContractDetail(selectedParentContractNumber).then(res => {
            if (res) {
                if (contractType === localConstant.contract.CHD) {
                    this.updatedData["parentContractHolder"] = res.ContractInfo.contractHoldingCompanyName;
                    this.updatedData["parentCompanyCode"] = res.ContractInfo.contractHoldingCompanyCode;
                    this.props.actions.AddUpdateGeneralDetails(this.updatedData);
                }
                else if (contractType === localConstant.contract.IRF) {
                    this.updatedData["frameworkContractHolder"] = res.ContractInfo.contractHoldingCompanyName;
                    this.updatedData["frameworkCompanyCode"] = res.ContractInfo.contractHoldingCompanyCode;
                    this.props.actions.AddUpdateGeneralDetails(this.updatedData);
                }
            }
        });
        this.updatedData = {};
        this.props.actions.HideModal();
    }
    copyParentContractDetails = (selectedParentContractNumber, contractType) => {
        const contractInfo=this.props.generalDetailsData;
        if (contractType === localConstant.contract.CHD) {
            contractInfo.parentCoframeworkCompanyOfficeName=null;
            contractInfo.frameworkContractNumber=null;
            contractInfo.frameworkContractHolder=null;
            this.props.actions.AddUpdateGeneralDetails(contractInfo);
            if (this.props.generalDetailsCreateContractCustomerDetails) {
                this.props.actions.FetchInvoicingDefaults();
            }
            else {
                IntertekToaster(localConstant.contract.CUSTOMER_NAME_VALIDATION, 'warningToast gdCustomerReqqq');
            }
        }
        else if(contractType === localConstant.contract.IRF){
            contractInfo.parentCompanyOffice = null;
            contractInfo.frameworkCompanyOfficeName = null;
            contractInfo.parentContractNumber = null;
            contractInfo.parentContractDiscount = null;
            contractInfo.parentContractHolder = null;
            this.props.actions.AddUpdateGeneralDetails(contractInfo);
            if (this.props.generalDetailsCreateContractCustomerDetails) {
                this.props.actions.FetchInvoicingDefaults();
            }
            else {
                IntertekToaster(localConstant.contract.CUSTOMER_NAME_VALIDATION, 'warningToast gdCustomerReqqq');
            }
        }
        const selectedCompanyCode = this.props.selectedCompany;
        let selectedCompanyName = "";
        this.props.companyList.map(company => {
            if (company.companyCode === selectedCompanyCode) {
                selectedCompanyName = company.companyName;
            };
        });
        this.setState({
            searchCompanyCode:selectedCompanyCode,
            searchCompanyName:selectedCompanyName
        });
        this.props.actions.HideModal();
        this.props.actions.FetchParentContractGeneralDetail(selectedParentContractNumber, contractType, this.props.generalDetailsData.customerContractNumber);
    }
    //selected customer Country Change
    selectedCustomerCountrySearch = (e) => {
        this.updatedData[e.target.name] = e.target.value;
        this.props.actions.FetchCustomerList(this.updatedData);
    }
    //selected Customer search on Text Box Onchange
    ButtonModal = (e) => {
        e.preventDefault();
        this.updatedData[e.target.name] = e.target.value;
        this.props.actions.FetchCustomerList(this.updatedData);
    }

    handlerKeyDown = (e) => {
        this.updatedData[e.target.name] = e.target.value.trim();
        if (this.updatedData.customerName !== '' && e.keyCode === 13) {
            const strValue=e.target.value;
            if(strValue[strValue.length - 1] != '*'){
                strValue = strValue +'*';
             }           
            this.updatedData.customerName = strValue;
            this.props.actions.FetchCustomerList(this.updatedData);
        }
    }

    //onButton Click Open Modalpoup
    selectCustomerSearch = (e) => {
        e.preventDefault();
        this.props.actions.ContractShowModal(this.updatedData);
        this.props.actions.FetchCustomerList(this.updatedData);
    }

    /**
     *  Common method for getting the value
     * */
    inputChangeHandler = (e) => {
        const value = e.target[e.target.type === "checkbox" ? "checked" : "value"];
        return value;
    }

    validatePercentage =(e) => {
        if (e.charCode === 45 || e.charCode === 46) {
            e.preventDefault();
        }
    }
    /**
     * On Change of the value need to update the backend data
     */
    generalDetailsHandlerChange = (e) => {
        const result = this.inputChangeHandler(e);
        this.updatedData[e.target.name] = result;

        //Commented for IGO QC D-890
        // //restriction of contract budget warning and hours warning
        // if(e.target.name === 'contractBudgetMonetaryWarning' || e.target.name === 'contractBudgetHoursWarning')
        // {
        //     const value = e.target.value.toString();
        //     if(value.length >2)
        //     {
        //       e.target.value = value.substring(0,2);
        //       this.updatedData[e.target.name]= e.target.value;
        //     }   
        // }
        // restriction of contract budget values and contract budget hours
        if(e.target.name === 'contractBudgetMonetaryValue' || e.target.name === 'contractBudgetHours')
          {

        this.updatedData[e.target.name]=  decimalCheck(e.target.value,2);
            }

        if (this.props.currentPage === "Create Contract") {
            if(e.target.name === 'contractBudgetMonetaryCurrency')
            {
            if (this.updatedData.contractBudgetMonetaryCurrency) {
                if( isEmptyOrUndefine(this.props.generalDetailsData.isFirstUpdate) && !(this.props.generalDetailsData.isFirstUpdate)){
                    this.updatedData["contractInvoicingCurrency"] = this.updatedData.contractBudgetMonetaryCurrency;
                    this.updatedData["isFirstUpdate"]=true;
                }
            }
           
           }
            this.updatedData["recordStatus"] = "N";
        }
        
        else {
            this.updatedData["recordStatus"] = "M";
        }
        if(this.props.selectedCompany)
        {
            const selectedCompanyCode = this.props.selectedCompany;
            let selectedCompanyName = "";
            this.props.companyList.map(company => {
                if (company.companyCode === selectedCompanyCode) {
                    selectedCompanyName = company.companyName;
                };
            });
        this.updatedData["contractHoldingCompanyCode"]=selectedCompanyCode;
        this.updatedData["contractHoldingCompanyName"]=selectedCompanyName;
        }
        if(e.target.name === 'contractBudgetMonetaryWarning' || e.target.name === 'contractBudgetHoursWarning')
        {
            const value = e.target.value.toString();
            if (value.length > 2) {
                const budgetwarning = parseInt(value.substring(0, 3));
                if(budgetwarning > 100){
                    e.target.value = 100; 
                }
                else{
                    e.target.value = budgetwarning;
                }
                this.updatedData[e.target.name] = e.target.value;
            }
        }
        this.props.actions.AddUpdateGeneralDetails(this.updatedData);
        this.updatedData = {};
    }

    onValueClickHandler = () => {
        this.props.actions.GetSelectedCustomerCode(this.props.generalDetailsData.contractCustomerCode);
    }

    render() {
        const statusValues = [
            { name: 'Open', value: 'O' },
            { name: 'Closed', value: 'C' }
        ];
        const crmValues = [
            { name: 'Yes', value: true },
            { name: 'No', value: false }
        ];
        const { customerList, generalDetailsData, currentPage, generalDetailsCreateContractCustomerDetails } = this.props;
        let generalDetailsInfo = [];
        if (generalDetailsData) {
            generalDetailsInfo = generalDetailsData;
        }
            if(generalDetailsInfo.contractClientReportingRequirement===null)
        {
            generalDetailsInfo.contractClientReportingRequirement=undefined;
        }
        const contractTypeData = [
            { label: "Standard Contract", value: "STD" },
            { label: "Is Parent Contract", value: "PAR" },
            { label: "Is Child Contract", value: "CHD" },
            { label: "Is Framework Contract", value: "FRW" },
            { label: "Is Related Framework Contract", value: "IRF" }
        ];
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
       
        const monetaryValues = {
                hasLabel:true,
                divClassName:'col',
                label:localConstant.budget.VALUE,
                type:'text',
                dataType:'decimal',
                valueType:'value',
                colSize:'s6',
                inputClass:'customInputs',
                labelClass:"customLabel mandate",
                max:'99999999999',
                name:'contractBudgetMonetaryValue',
                maxLength: fieldLengthConstants.common.budget.BUDGET_VALUE_MAXLENGTH,
                min:'0',
                prefixLimit: fieldLengthConstants.common.budget.BUDGET_VALUE_PREFIX_LIMIT,
                suffixLimit:fieldLengthConstants.common.budget.BUDGET_VALUE_SUFFIX_LIMIT,
                isLimitType:true,
                disabled:this.props.interactionMode,
                readOnly:this.props.interactionMode,
                value:generalDetailsInfo.contractBudgetMonetaryValue?thousandFormat(generalDetailsInfo.contractBudgetMonetaryValue):generalDetailsInfo.contractBudgetMonetaryValue ===0?formatToDecimal(generalDetailsInfo.contractBudgetMonetaryValue,2):'',//Added FormatToDecimal for D-1164
                onValueChange:this.generalDetailsHandlerChange,
                // onValueBlur:this.checkNumber
            };

        const monetaryWarning = 
            {
                hasLabel:true,
                divClassName:'col',
                label:localConstant.budget.WARNING,
                type:'number',
                dataValType:'valueText',
                colSize:'s3',
                inputClass:"customInputs",
                labelClass:"customLabel mandate",
                name:"contractBudgetMonetaryWarning",
                max:'100',
                min:'0',
                maxLength:fieldLengthConstants.Contract.generalDetails.BUDGET_WARNING_MAXLENGTH ,
                value: generalDetailsInfo.contractBudgetMonetaryWarning,
                readOnly:this.props.interactionMode,
                disabled:this.props.interactionMode,
                onValueChange:this.generalDetailsHandlerChange,
                onValueKeypress:this.validatePercentage
                // onValueBlur:this.onWarningBlur    //Commented for IGO QC D-890
            };        

        const monetaryCurrency={
            label:`${ localConstant.budget.CURRENCY }`,
            value:generalDetailsInfo.contractBudgetMonetaryCurrency,
            hasLabel:true,
            divClassName:'col',          
            type:'select',
            colSize:'s3',
            className:"browser-default",
            labelClass:"mandate",
            optionsList:this.props.currencyData,
            optionName:"code",
            optionValue:"code",
            name:"contractBudgetMonetaryCurrency",
            disabled:this.props.interactionMode,
            defaultValue:generalDetailsInfo.contractBudgetMonetaryCurrency ? generalDetailsInfo.contractBudgetMonetaryCurrency:'',
            // onSelectChange:this.generalDetailsHandlerChange,
            onValueBlur:this.generalDetailsHandlerChange,//D-675 Changes
        };

        const monetaryTaxes=[
            {
                className:this.props.currentPage === "Create Contract"?"hide":"custom-Badge col",
                colSize:"s12",
                label:`${ localConstant.budget.INVOICED_TO_DATE_EXCL }:`,
                value:generalDetailsInfo.contractInvoicedToDate?thousandFormat(generalDetailsInfo.contractInvoicedToDate):"0.00"
            },
            {
                className:this.props.currentPage === "Create Contract"?"hide":"custom-Badge col",
                colSize:"s12",
                label:`${ localConstant.budget.UNINVOICED_TO_DATE_EXCL }:`,
                value:generalDetailsInfo.contractUninvoicedToDate?thousandFormat(generalDetailsInfo.contractUninvoicedToDate):"0.00"
            },
            {
                className:this.props.currentPage === "Create Contract"?"hide":"custom-Badge col",
                colSize:"s12",
                label:` ${ localConstant.budget.REMAINING }:`,
                value: generalDetailsInfo.contractRemainingBudgetValue ?thousandFormat(generalDetailsInfo.contractRemainingBudgetValue): "0.00"
            },
            {
                className:this.props.currentPage !== "Create Contract" && generalDetailsInfo.contractBudgetValueWarningPercentage <= 0 ?"hide":this.props.currentPage === "Create Contract"?"hide" : "custom-Badge col text-red-parent",
                colSize:"s12",
                label: generalDetailsInfo.contractBudgetValueWarningPercentage > 0 ? thousandFormat(generalDetailsInfo.contractBudgetValueWarningPercentage) + " " + ` ${ localConstant.commonConstants.CONTRACT_BUDGET_WARNING } `: null
            }
        ];

        const unitValues =[
            {
                hasLabel:true,
                divClassName:"col",
                label:localConstant.budget.UNITS,
                type: 'text',
                dataType: 'decimal',
                valueType: 'value',
                colSize:'s6',
                inputClass:"customInputs",
                labelClass:'customLabel mandate',
                max:'999999999',
                min:'0',
                name:'contractBudgetHours',
                maxLength: fieldLengthConstants.common.budget.BUDGET_HOURS_MAXLENGTH,
                prefixLimit:fieldLengthConstants.common.budget.BUDGET_HOURS_PREFIX_LIMIT, 
                suffixLimit :fieldLengthConstants.common.budget.BUDGET_HOURS_SUFFIX_LIMIT, 
                isLimitType:true,
                readOnly:this.props.interactionMode,
                disabled:this.props.interactionMode,
                value: generalDetailsInfo.contractBudgetHours?thousandFormat(generalDetailsInfo.contractBudgetHours):generalDetailsInfo.contractBudgetHours === 0?formatToDecimal(generalDetailsInfo.contractBudgetHours,2):'', //Added FormatToDecimal for D-1164
                onValueChange:this.generalDetailsHandlerChange,
                //onValueBlur:this.checkNumber,
            },
            {
                hasLabel:true,
                divClassName:'col',
                label:localConstant.budget.WARNING,
                type:'number',
                dataValType:'valueText',
                colSize:'s3',
                inputClass:"customInputs",
                labelClass:"customLabel mandate",
                name:'contractBudgetHoursWarning',
                max:'100',
                min:'0',
                maxLength:fieldLengthConstants.Contract.generalDetails.BUDGETHOURS_WARNING_MAXLENGTH,
                readOnly:this.props.interactionMode,
                disabled:this.props.interactionMode,
                value: generalDetailsInfo.contractBudgetHoursWarning,
                onValueChange:this.generalDetailsHandlerChange,
                onValueKeypress:this.validatePercentage
                // onValueBlur:this.onWarningBlur   //Commented for IGO QC D-890
            }
        ];

        const unitTaxes = [
            {
                className:this.props.currentPage === "Create Contract"?"hide":"custom-Badge col",
                colSize:"s12",
                label:`${ localConstant.budget.INVOICED_TO_DATE }:`,
                value:generalDetailsInfo.contractHoursInvoicedToDate?thousandFormat(generalDetailsInfo.contractHoursInvoicedToDate):"0.00",
            },
            {
                className:this.props.currentPage === "Create Contract"?"hide":"custom-Badge col",
                colSize:"s12",
                label:`${ localConstant.budget.UNINVOICED_TO_DATE }:`,
                value:generalDetailsInfo.contractHoursUninvoicedToDate?thousandFormat(generalDetailsInfo.contractHoursUninvoicedToDate):"0.00",
            },
            {
                className:this.props.currentPage === "Create Contract"?"hide":"custom-Badge col",
                colSize:"s12",
                label:` ${ localConstant.budget.REMAINING }: `,
                value: generalDetailsInfo.contractRemainingBudgetHours ?thousandFormat(generalDetailsInfo.contractRemainingBudgetHours): "0.00"
            },
            {
                className:this.props.currentPage !== "Create Contract" && generalDetailsInfo.contractBudgetHourWarningPercentage <= 0 ?"hide":this.props.currentPage === "Create Contract"?"hide":"custom-Badge col text-red-parent",
                colSize:"s12",
                label: generalDetailsInfo.contractBudgetHourWarningPercentage > 0 ? thousandFormat(generalDetailsInfo.contractBudgetHourWarningPercentage) + " " + ` ${ localConstant.commonConstants.CONTRACT_BUDGETHOUR_WARNING } `: null
            }
        ];

        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                <div className="genralDetailContainer customCard">
                    <GeneralDetail contractTypeData={contractTypeData}
                        onClickShowModal={this.onClickShowModal}
                        CustomerCodeInCRM={this.props.CustomerCodeInCRM}
                        contractNumber={this.props.currentPage === "Create Contract" ? "" : generalDetailsInfo.contractNumber}
                        customerContractNumber={generalDetailsInfo.customerContractNumber}
                        isCrm={generalDetailsInfo.isCRM}
                        contractCRMReason={generalDetailsInfo.contractCRMReason}
                        contractType={generalDetailsInfo.contractType}
                        CRM={crmValues} onClickCRM={this.onClickCRM}
                        contractHoldingCompanyName={generalDetailsInfo.contractHoldingCompanyName}
                        parentContractHolder={generalDetailsInfo.parentContractHolder}
                        frameworkContractHolder={generalDetailsInfo.frameworkContractHolder}
                        customerName={generalDetailsCreateContractCustomerDetails && generalDetailsCreateContractCustomerDetails.customerName}
                        onBlurValue={this.onBlurValue}
                        discountLengthCheck={this.discountLengthCheck}
                        checkNumber={this.checkNumber}
                        contractConflictOfInterest={generalDetailsInfo.contractConflictOfInterest}
                        contractCustomerCode={generalDetailsInfo.contractCustomerCode}
                        changeContractType={this.changeContractType}
                        onEditContractType={this.state.contractType}
                        companyOffices={this.props.companyOffices}
                        interactionMode={this.props.interactionMode}
                        parentContractNumber={this.props.parentContractNumber}
                        confirmParentSelection={this.confirmParentSelection}
                        contractCustomerName={generalDetailsInfo.contractCustomerName}
                        parentCompanyOffice={generalDetailsInfo.parentCompanyOffice}
                        data={generalDetailsInfo}
                        onGeneralDetailsDataChange={this.generalDetailsHandlerChange}
                        currentPage={currentPage}
                        onValueClickHandler={this.onValueClickHandler}
                        customerChangeHandler={this.customerChangeHandler}
                        selectRefProps={ this.selectRef }
                    />

                    <BudgetMonetary
                        monetaryValues={monetaryValues}
                        monetaryCurrency={monetaryCurrency}
                        monetaryTaxes={monetaryTaxes}
                        unitValues={unitValues}
                        unitTaxes={unitTaxes}
                        isCurrencyEditable={true}
                        monetaryWarning={monetaryWarning}
                        interactionMode={this.props.interactionMode}
                    />
                  
                    <OtherDetails status={statusValues}
                        startDate={generalDetailsInfo.contractStartDate ? generalDetailsInfo.contractStartDate : ''} fetchStartDate={this.fetchStartDate}
                        endDate={generalDetailsInfo.contractEndDate ? generalDetailsInfo.contractEndDate : ""} fetchEndDate={this.fetchEndDate}
                        contractStatus={generalDetailsInfo.contractStatus}
                        contractClientReportingRequirement={generalDetailsInfo.contractClientReportingRequirement}
                        contractOperationalNote={generalDetailsInfo.contractOperationalNote}
                        handleOtherDetailStartDateBlur={this.handleOtherDetailStartDateBlur}
                        handleOtherDetailEndDateBlur={this.handleOtherDetailEndDateBlur}
                        interactionMode={this.props.interactionMode}
                        onGeneralDetailsDataChange={this.generalDetailsHandlerChange}
                            />
                    {this.props.isShowModal &&
                        <Modal id="contractGeneralModalPopup" title={'Customer list'} modalClass="contractModal" buttons={
                            [
                                { name: 'Cancel', type: 'reset', action: this.hideModal, showbtn: true, btnClass: "btn-small mr-2" },
                                { name: 'Submit', type: 'submit', action: this.getCustomerName, showbtn: true, btnClass: "btn-small" }
                            ]
                        }
                            isShowModal={this.props.isShowModal} >
                            <ButtonModal
                                searchCustomer={this.ButtonModal}
                                countryMasterData={this.props.countryMasterData}
                                customerName={generalDetailsCreateContractCustomerDetails && generalDetailsCreateContractCustomerDetails.customerName}
                                operatingCountry={this.customerCountry}
                                handlerKeyDown={this.handlerKeyDown}
                            />
                            <ReactGrid gridRowData={customerList} gridColData={HeaderData} onRef={ref => { this.child = ref; }} paginationPrefixId={localConstant.paginationPrefixIds.contractCustomerList}/>
                        </Modal>
                    }
                </div>
            </Fragment>
        );
    }
}
export default GeneralDetails;