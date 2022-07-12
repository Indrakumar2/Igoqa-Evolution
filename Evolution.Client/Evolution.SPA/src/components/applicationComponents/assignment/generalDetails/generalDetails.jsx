import React, { Component, Fragment } from 'react';
import { Link } from 'react-router-dom';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import { getlocalizeData, 
    formInputChangeHandler, 
    isEmpty,
    isEmptyOrUndefine, 
    ObjectIntoQuerySting,
    thousandFormat } from '../../../../utils/commonUtils';
import LabelWithValue from '../../../../common/baseComponents/customLabelwithValue';
import ProjectAnchor from '../../../viewComponents/projects/projectAnchor';
import ContractAnchor from '../../../viewComponents/contracts/contractAnchor';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import dateUtil from '../../../../utils/dateUtil';
import arrayUtil from '../../../../utils/arrayUtil';
import PropTypes from 'prop-types';
import CustomModal from '../../../../common/baseComponents/customModal';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import { required } from '../../../../utils/validator';
import BudgetMonetary from '../../budgetMonetary';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
import moment from 'moment';
import { StringFormat } from '../../../../utils/stringUtil';

const localConstant = getlocalizeData();

const budgetConfig = {
    monetaryValues : {
        hasLabel :true,
        divClassName :'col',
        label:localConstant.budget.VALUE,
        type:'text',
        dataType:'decimal',
        valueType:'value',
        colSize:'s6',
        inputClass:'customInputs',
        labelClass:"customLabel mandate",
        max:'99999999999',
        required:true,
        name:'assignmentBudgetValue',
        maxLength: fieldLengthConstants.common.budget.BUDGET_VALUE_MAXLENGTH,
        prefixLimit: fieldLengthConstants.common.budget.BUDGET_VALUE_PREFIX_LIMIT,
        suffixLimit: fieldLengthConstants.common.budget.BUDGET_VALUE_SUFFIX_LIMIT,
        isLimitType:true,
    },
    monetaryWarning : {
        hasLabel :true,
        divClassName :'col',
        label:localConstant.budget.WARNING,
        type:'number',
        dataValType:'valueText',
        colSize:'s3',
        inputClass:'customInputs',
        labelClass:"customLabel mandate",
        required:true,
        max:'100',
        min:'0',
        name:'assignmentBudgetWarning',
        maxLength:fieldLengthConstants.assignment.generalDetails.ASSIGNMENT_BUDGET_WARNING_MAXLENGTH,
    },
    monetaryCurrency : {
        label:localConstant.budget.CURRENCY,
        hasLabel:true,
        divClassName:'col',          
        colSize:'s3 mt-4',
        className:"browser-default",
        labelClass:"mandate",
        name:"assignmentBudgetCurrency",
        disabled:true,
    },
    monetaryTaxes : [
        {
            className:"custom-Badge col",
            colSize:"s12",
            label:`${ localConstant.budget.INVOICED_TO_DATE_EXCL }: `,
            id: "invoicedToDate",
            value: "0.00"
        },
        {
            className:"custom-Badge col",
            colSize:"s12",
            label:`${ localConstant.budget.UNINVOICED_TO_DATE_EXCL }: `,
            id: "uninvoicedToDate",
            value: "0.00"
        },
        {
            className:"custom-Badge col",
            colSize:"s12",
            label:`${ localConstant.budget.REMAINING }: `,
            id: "remaining",
            value: "0.00"
        },
        {
            className:"custom-Badge col text-red-parent",
            colSize:"s12",
            label:null,
            id:"warningPercentage"
        }
    ],
    unitValues : [
        {
            hasLabel:true,
            divClassName:"col",
            label:localConstant.budget.UNITS,
            id:"unitValue",
            type: 'text',
            dataType: 'decimal',
            valueType: 'value',
            colSize:'s6',
            inputClass:'customInputs',
            labelClass:'customLabel mandate',
            max:'999999999',
            required:true,
            name:'assignmentBudgetHours',
            maxLength:fieldLengthConstants.common.budget.BUDGET_HOURS_MAXLENGTH,
            min:'0',
            prefixLimit: fieldLengthConstants.common.budget.BUDGET_HOURS_PREFIX_LIMIT,
            suffixLimit : fieldLengthConstants.common.budget.BUDGET_HOURS_SUFFIX_LIMIT, 
            isLimitType:true,
        },
        {
            hasLabel:true,
            divClassName:'col',
            label:localConstant.budget.WARNING,
            id:"unitWarning",
            type:'number',
            dataValType:'valueText',
            colSize:'s3',
            inputClass:"customInputs",
            labelClass:"customLabel mandate",
            name:'assignmentBudgetHoursWarning',
            maxLength:fieldLengthConstants.assignment.generalDetails.ASSIGNMENT_BUDGET_WARNING_MAXLENGTH,
            max:'100',
            min:'0',
        }
    ],
    unitTaxes: [
        {
            className:"custom-Badge col",
            colSize:"s12",
            id: "invoicedToDateUnit",
            label:` ${ localConstant.budget.INVOICED_TO_DATE }: `,
            value: "0.00"
        },
        {
            className:"custom-Badge col",
            colSize:"s12",
            label:` ${ localConstant.budget.UNINVOICED_TO_DATE }: `,
            id: "uninvoicedToDateUnit",
            value: "0.00"
        },
        {
            className:"custom-Badge col",
            colSize:"s12",
            label:` ${ localConstant.budget.REMAINING }:  `,
            id: "remainingUnit",
            value: "0.00"
        },
        {
            className:"custom-Badge col text-red-parent",
            colSize:"s12",
            label:null,
            id:"warningPercentageUnit"
        }
    ]
};

/** Basic Detials Section */
const BasicDetails = (props) => {
    const isTimesheetAssignment = props.isTimesheetAssignment;
    const disableSupplierPO = (props.isOperatingCompany &&
        props.currentPage !== localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE);
    let disableSPO = false;
    if(!isTimesheetAssignment && props.currentPage === localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE){
        const visitCreated = props.visitList;
        if(visitCreated.length > 0){
            disableSPO = true; //Changes for Live D777
        }
    }
    return (
        <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.assignments.BASIC_DETAILS} colSize="s12">
            <div className="row">
                <LabelWithValue
                    className="custom-Badge col textNoWrapEllipsis marginAlign"
                    colSize="s5"
                    label={`${
                        localConstant.assignments.CUSTOMER_NAME
                        }: `}
                    value={props.generalDetails.assignmentCustomerName}
                    labelClass="customLabel" />
                <div className="custom-Badge col br1 s3 bold widthChange2">{ `${ localConstant.assignments.CONTRACT_NUMBER }: ` }
                    <ContractAnchor
                        data={ { contractNumber: props.generalDetails.assignmentContractNumber ,contractHoldingCompanyCode :props.generalDetails.assignmentContractHoldingCompanyCode } } />
                </div>
                <div className="custom-Badge col br1 s2 bold">{ `${ localConstant.assignments.PROJECT_NUMBER }: ` }
                    {props.generalDetails.assignmentProjectNumber && <ProjectAnchor
                        value={props.generalDetails.assignmentProjectNumber} />
                    }
                </div>
                <div className="row">
                    <div className="custom-Badge col br1 s2 widthChange3"> <span class="bold">{`${ localConstant.assignments.ASSIGNMENT_NUMBER }: `}</span>
                        {props.generalDetails.assignmentNumber ? props.generalDetails.assignmentNumber.toString().padStart(5, '0') : ""}
                    </div>
                </div>
                <div className="col s12 m6">
                    <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.assignments.ASSIGNMENT_REFERENCE}
                        type='text'
                        dataValType='valueText'
                        colSize='s12 pl-0'
                        inputClass="customInputs"
                        labelClass="customLabel"
                        name="assignmentReference"
                        autocomplete="off"
                        maxLength={fieldLengthConstants.assignment.generalDetails.ASSIGNMENT_REFERENCE_MAXLENGTH}
                        id="assignmentReferenceId"
                        readOnly={props.interactionMode}
                        value={props.generalDetails.assignmentReference?props.generalDetails.assignmentReference:''}
                        onValueChange={props.onChangeHandler} />
                    <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.assignments.ASSIGNMENT_TYPE}
                        type='select'
                        colSize='s12 pl-0'
                        className="browser-default"
                        optionsList={props.assignmentType.filter(value => value.description !=='I')}
                        labelClass="customLabel mandate"
                        optionName='name'
                        optionValue="description"
                        name="assignmentType"
                        id="assignmentTypeId"
                        disabled={props.interactionMode}
                        defaultValue={props.generalDetails.assignmentType}
                        onSelectChange={props.onChangeHandler} />
                </div>
                <div className="col s12 m6">
                    <CustomInput hasLabel={false}
                        type='checkbox'
                        checkBoxArray={[ { label: localConstant.assignments.ASSIGNMENT_COMPLETE, value: props.generalDetails.assignmentStatus ==="C" ? true : false } ]}
                        colSize='s6 mt-4'
                        name="assignmentComplete"
                        id="assignmentComplete"
                        onCheckboxChange={props.onChangeHandler}
                        disabled={props.interactionMode || props.isOperatingCompany }
                        checked={ props.generalDetails.assignmentStatus === "C" ? true : false }
                        refProps ={props.checkboxRefProps} />
                    <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.assignments.ASSIGNMENT_STATUS}
                        type='select'
                        colSize='s6'
                        className="browser-default"
                        optionsList={props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE ? props.assignmentStatus.filter(x => x.description === 'P') : props.assignmentStatus}
                        labelClass="customLabel"
                        optionName='name'
                        optionValue="description"
                        name="assignmentStatus"
                        id="assignmentStatusId"
                        selectstatus="true"
                        disabled={props.interactionMode || props.isAssignmentComplete || props.isOperatingCompany }
                        defaultValue={props.generalDetails.assignmentStatus ? props.generalDetails.assignmentStatus : 'P'}
                        onSelectChange={props.onChangeHandler}
                        refProps ={props.selectRefProps} />
                    <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.assignments.SUPPLIER_PURCHASE_ORDER}
                        type='select'
                        colSize='s12'
                        className="browser-default"
                        optionsList={props.supplierPO}
                        labelClass="customLabel mandate"
                        optionName='supplierPONumber'
                        optionValue="supplierPOId"
                        name="assignmentSupplierPurchaseOrderId"
                        id="supplierPurchaseOrderId"
                        disabled={isTimesheetAssignment || disableSupplierPO || props.interactionMode || disableSPO}
                        defaultValue={props.generalDetails.assignmentSupplierPurchaseOrderId? props.generalDetails.assignmentSupplierPurchaseOrderId:''}
                        onSelectChange={props.onChangeHandler} />
                    <div className="col s12">
                    <a  href='javascript:void(0)' onClick={props.selectedRowHandler} className={isTimesheetAssignment === true || !props.generalDetails.assignmentSupplierPurchaseOrderId ? "waves-effect isDisabled" : "link"}>{ localConstant.assignments.VIEW_SUPPLIER_PO }</a>            
                        {/* <Link target="_blank" 
                            to={{ pathname:AppMainRoutes.supplierPoDetails, 
                            search:`?supplierPOId=${ props.generalDetails.assignmentSupplierPurchaseOrderId }&selectedCompany=${ props.selectedCompany }&isAssignmentOpenedAsOC=${ props.isOperatingCompany ? true : false }&chCompany=${ props.generalDetails.assignmentContractHoldingCompanyCode }` }} className={isTimesheetAssignment === true || !props.generalDetails.assignmentSupplierPurchaseOrderId ? "waves-effect isDisabled" : "link"}> 
                            {localConstant.assignments.VIEW_SUPPLIER_PO}
                        </Link> */} 
                    </div>
                    {/* <CustomInput hasLabel={true}
                        type='checkbox'
                        checkBoxArray={[ { label: localConstant.assignments.INTERNAL_ASSIGNMENT, value: props.generalDetails.internalAssignment } ]}
                        colSize='s6 mt-4'
                        name="internalAssignment"
                        id="internalAssignment"
                        onCheckboxChange={props.onChangeHandler}
                        //disabled={props.interactionMode || props.isOperatingCompany }
                        checked= { props.internalAssignment ? true : false } 
                        refProps={props.switchRefProps}
                        /> */}
                    <CustomInput
                        isSwitchLabel={true}
                        type='switch'
                        switchLabel={localConstant.assignments.INTERNAL_ASSIGNMENT}
                        switchName="isInternalAssignment"
                        id="isInternalAssignment"
                        colSize="col s12"
                        //disabled={props.interactionMode}
                        checkedStatus={props.isInternalAssignment}
                        onChangeToggle={props.onChangeHandler}
                        refProps={props.switchRefProps}
                        switchKey={props.isInternalAssignment ? true : false}
                    />
                </div>
            </div>
        </CardPanel>
    );
};

/** Companies and Coordinators Section */
const ComapniesAndCoordinators = (props) => {
    const isEditMode = (props.currentPage === localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE);
    return (
        <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.assignments.COMPANIES_AND_COORDINATORS} colSize="s12">
            <div className="row">
                <CustomInput
                    hasLabel={true}
                    divClassName='col'
                    label={localConstant.assignments.CONTRACT_HOLDING_COMPANY}
                    type='text'
                    dataValType='valueText'
                    colSize='s12 m5'
                    inputClass="customInputs"
                    labelClass="customLabel"
                    name="contractHoldingCompany"
                    maxLength={fieldLengthConstants.assignment.generalDetails.CONTRACT_HOLDING_COMPANY_MAXLENGTH}
                    id="contractHoldingCompany"
                    readOnly="true"
                    value={props.generalDetails.assignmentContractHoldingCompany} />
                <CustomInput
                    hasLabel={true}
                    divClassName='col'
                    label={localConstant.assignments.CONTRACT_HOLDING_COORDINATOR}
                    type='select'
                    colSize='s12 m4'
                    className="browser-default"
                    optionsList={props.contractHoldingCompanyCoordinators}
                    labelClass="customLabel mandate"
                    optionName='miDisplayName'
                    optionValue="username"
                    name="assignmentContractHoldingCompanyCoordinatorCode"
                    id="assignmentContractHoldingCompanyCoordinator"
                    disabled={(props.interactionMode || (props.selectedCompany === props.generalDetails.assignmentOperatingCompanyCode &&
                        props.generalDetails.assignmentOperatingCompanyCode !== props.generalDetails.assignmentContractHoldingCompanyCode &&
                      props.currentPage !== localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE)) ? true : false}
                    defaultValue={props.generalDetails.assignmentContractHoldingCompanyCoordinatorCode}
                    onSelectChange={props.onChangeHandler} />
                <div className="col s12 m3 mt-4">
                    <a className={props.interactionMode === true ? "isDisabled" : null} href={"mailto: " + props.contractHoldingCompanyCoordinatorEmail}>
                        {props.contractHoldingCompanyCoordinatorEmail}</a>
                </div>
                {isEditMode ? 
                    <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.assignments.OPERATING_COMPANY}
                        type='text'
                        dataValType='valueText'
                        colSize='s12 m5'
                        inputClass="customInputs"
                        labelClass="customLabel"
                        name="assignmentOperatingCompany"
                        maxLength={fieldLengthConstants.assignment.generalDetails.OPERATING_COMPANY_MAXLENGTH}
                        id="operatingCompanyId"
                        readOnly="true"
                        value={props.generalDetails.assignmentOperatingCompany}
                    /> : 
                    <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.assignments.OPERATING_COMPANY}
                        type='select'
                        colSize='s12 m5'
                        className="browser-default"
                        optionsList={props.company}
                        labelClass="customLabel"
                        optionName='companyName'
                        optionValue="companyCode"
                        name="assignmentOperatingCompany"
                        id="operatingCompanyId"
                        disabled={props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE ? false : true}
                        defaultValue={props.generalDetails.assignmentOperatingCompanyCode}
                        refProps = { props.ocCompanyRef }
                        onSelectChange={props.operatingCompanyChangeHandler}
                    /> }
                <CustomInput
                    hasLabel={true}
                    divClassName='col'
                    label={localConstant.assignments.OPERATING_COMPANY_COORDINATOR}
                    type='select'
                    colSize='s12 m4'
                    className="browser-default"
                    optionsList={props.operatingCompanyCoordinators}
                    labelClass={ props.isOperatingCoordinatorMandatory ? "customLabel mandate" : "customLabel"}
                    optionName='miDisplayName'
                    optionValue="username"
                    name="assignmentOperatingCompanyCoordinatorCode"
                    id="operatingCompanyCoordinatorId"
                    disabled={props.interactionMode}
                    defaultValue={props.generalDetails.assignmentOperatingCompanyCoordinatorCode}
                    onSelectChange={props.onChangeHandler} />
                <div className="col s12 m3 mt-4">
                    <a className={props.interactionMode === true ? "isDisabled" : null} href={"mailto: " + props.operatingCompanyCoordinatorEmail}>
                        {props.operatingCompanyCoordinatorEmail}</a>
                </div>
                { props.isTimesheetAssignment ?
                    <div>
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.assignments.WORK_LOCATION_COUNTRY}
                            type='select'
                            colSize='s12 m5'
                            className="browser-default"
                            optionsList={props.country}
                            labelClass="customLabel mandate"
                            optionName='name'
                            optionValue="id" //Added for ITK D1536
                            name="workLocationCountry"
                            id="workLocationCountryId"
                            disabled={props.interactionMode || isEditMode}
                            defaultValue={props.generalDetails.workLocationCountryId}
                            onSelectChange={props.locationChangeHandler} />
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.assignments.WORK_LOCATION}
                            type='select'
                            colSize='s12 m4'
                            className="browser-default"
                            optionsList={props.state}
                            labelClass="customLabel mandate"
                            optionName='name'
                            optionValue="id" //Added for ITK D1536
                            name="workLocationCounty"
                            id="workLocationCountyId"
                            disabled={props.interactionMode || isEditMode}
                            defaultValue={props.generalDetails.workLocationCountyId}
                            onSelectChange={props.locationChangeHandler} />
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.assignments.WORK_LOCATION_CITY}
                            type='select'
                            colSize='s12 m3'
                            className="browser-default"
                            optionsList={props.city}
                            labelClass="customLabel"
                            optionName='name'
                            optionValue="id" //Added for ITK D1536
                            name="workLocationCity"
                            id="workLocationCityId"
                            disabled={props.interactionMode || isEditMode}
                            defaultValue={props.generalDetails.workLocationCityId}
                            onSelectChange={props.locationChangeHandler} />
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.assignments.WORK_LOCATION_ZIP_CODE}
                            type='text'
                            dataValType='valueText'
                            colSize='s12 m5'
                            inputClass="customInputs"
                            labelClass="customLabel"
                            name="workLocationPinCode"
                            maxLength={fieldLengthConstants.assignment.generalDetails.WORK_LOCATION_ZIP_CODE_MAXLENGTH} // hot fix.
                            id="workLocationZipCodeId"
                            readOnly={props.interactionMode}
                            value={props.generalDetails.workLocationPinCode}
                            onValueChange={props.onChangeHandler} />
                    </div> : null
                }
                { props.isHostCompany.isShow ?
                    <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.assignments.HOST_COMPANY}
                        type='select'
                        colSize='s12 m7'
                        className="browser-default"
                        optionsList={props.company}
                        labelClass="customLabel"
                        optionName='companyName'
                        optionValue="companyCode"
                        name="assignmentHostCompany"
                        id="assignmentHostCompanyId"
                        disabled={props.interactionMode || props.isHostCompany.isDisable || isEditMode}
                        defaultValue={props.generalDetails.assignmentHostCompanyCode}
                        onSelectChange={props.onChangeHandler} /> 
                    : null
                }
            </div>
        </CardPanel>
    );
};

/** First Visit Section */
const FirstVisit = (props) => {
   const isTimesheetAssignment = props.isTimesheetAssignment;
   let fromDate="",toDate="",status="";
    if(isTimesheetAssignment){
        //TIMESHEET_DETAILS
        fromDate = "timesheetFromDate";
        toDate ="timesheetToDate";
        status = "timesheetStatus";
    }else{
        //Visit Details
        fromDate = "visitFromDate";
        toDate = "visitToDate";
        status="visitStatus";
    }

    /**Added this method to remove Rejected by Operator and Rejected by Contract holder options */
    const loadDropDownValues = () => {
        const isTimesheetAssignment = props.isTimesheetAssignment;
        const data = isTimesheetAssignment ? localConstant.commonConstants.assignmentTimesheetStatus : localConstant.commonConstants.assignmentVisitStatus;
       // if (props.currentPage === 'addAssignment') {
            const objData = Object.assign([], data);
            for (let i = objData.length - 1; i >= 0; i--) {
                const item = objData[i];
                if (item.code === 'J' || item.code === 'R' || item.code === 'D' || item.code === 'E') {
                    objData.splice(i, 1);
                }
            }
            return objData;
        // }
        // return data;
    };

    /**To check whether the DateFrom, DateTo, FirstVisit Staus enable/disable mode. */    
    const firstVisitMode = () => {
        if (props.interactionMode) {
            return true;
        }
        if (props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE) {
            if (!isEmpty(props.generalDetails)) {
                if (props.generalDetails.assignmentOperatingCompanyCode === props.generalDetails.assignmentContractHoldingCompanyCode) {
                    return false;
                } else {
                    return true;
                } 
            }
        }
        else {
            if (props.isInterCompanyAssignment && props.isOperatorCompany) {
                if (!isEmpty(props.generalDetails[fromDate]) && props.isbtnDisable) { //Removed the interCompanyAssignmentDateInitailState property and added isbtnDisable property
                    return true;
                } else {
                    return false;
                }
            } else {
                return true;
            }
        }
    };

    const disableMode = firstVisitMode();
   
    return (
        <CardPanel className="white lighten-4 black-text mb-2" title={isTimesheetAssignment ?
            localConstant.assignments.TIMESHEET_DETAILS : localConstant.assignments.FIRST_VISIT} colSize="s12">
            <div className="row">
                <CustomInput
                    hasLabel={true}
                    isNonEditDateField={false}
                    label={localConstant.assignments.DATE_FROM}
                    labelClass="customLabel mandate"
                    colSize='s12 m3'
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    type='date'
                    name={fromDate}
                    autocomplete="off"
                    selectedDate={dateUtil.defaultMoment(props.generalDetails[fromDate])}
                    onDateChange={props.fetchStartDate}
                    shouldCloseOnSelect={true}
                    disabled={firstVisitMode()} />
                <CustomInput
                    hasLabel={true}
                    isNonEditDateField={false}
                    label={localConstant.assignments.DATE_TO}
                    labelClass="customLabel mandate"
                    colSize='s12 m3'
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    type='date'
                    name={toDate}
                    autocomplete="off"
                    selectedDate={dateUtil.defaultMoment(props.generalDetails[toDate])}
                    onDateChange={props.fetchEndDate}
                    shouldCloseOnSelect={true}
                    disabled={(firstVisitMode() || (props.generalDetails[fromDate]? false:true)) } />
                { !disableMode ?
                    <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={isTimesheetAssignment ?
                            localConstant.assignments.TIMESHEET_STATUS : localConstant.assignments.FIRST_VISIT_STATUS}
                        type='select'
                        colSize='s12 m6'
                        className="browser-default"
                        optionsList= {(loadDropDownValues())}
                        labelClass="customLabel"
                        optionName='name'
                        optionValue="code"
                        name={status}
                        id="firstVisitStatusId"
                        disabled={props.interactionMode}
                        defaultValue={  props.generalDetails[status] ?props.generalDetails[status]: status === 'timesheetStatus' ? 'N' : 'T' }
                        onSelectChange={props.onChangeHandler} /> 
                    : (isTimesheetAssignment ?
                        <LabelWithValue
                            className="custom-Badge col br1 mt-4"
                            colSize="s12 m6"
                            label={`${
                                localConstant.assignments.TIMESHEET_STATUS
                                } : `}
                            value={localConstant.assignments.TIMESHEET_STATUS_TEXT}
                        /> : <LabelWithValue
                            className="custom-Badge col br1 mt-4"
                            colSize="s12 m6"
                            label={`${
                                localConstant.assignments.FIRST_VISIT_STATUS
                                } : `}
                            value={localConstant.assignments.FIRST_VISIT_STATUS_TEXT}
                        />
                    )
                }
            </div>
        </CardPanel>
    );
};

/** Other Information */
const OtherInformation = (props) => {
    return (
        <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.assignments.OTHER_DETAILS} colSize="s12">
            <div className="row mb-0">
                <div className='col s12 m6'>
                    <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.assignments.LIFE_CYCLE}
                        type='select'
                        colSize='s12'
                        className="browser-default"
                        optionsList= { props.isProjectForNewFacility ? props.assignmentLifeCycle.filter(x=>x.isAlchiddenForNewFacility===false) : props.assignmentLifeCycle}
                        labelClass="customLabel mandate"
                        optionName='name'
                        optionValue="name"
                        name="assignmentLifecycle"
                        id="lifeCycleId"
                        disabled={props.interactionMode||props.isInterCompanyAssignment}
                        defaultValue={props.generalDetails.assignmentLifecycle}
                        onSelectChange={props.onChangeHandler} />
                    { props.isVisitAssignment ?
                        <CustomInput
                            hasLabel={true}
                            divClassName='col'
                            label={localConstant.assignments.REVIEW_AND_MODERATION_PROCESS}
                            type='select'
                            colSize='s12'
                            className="browser-default"
                            optionsList={props.reviewAndModerationProcess}
                            labelClass="customLabel mandate"
                            optionName='name'
                            optionValue="name"
                            name="assignmentReviewAndModerationProcess"
                            id="reviewAndModerationProcessId"
                            disabled={props.interactionMode||props.isInterCompanyAssignment}
                            defaultValue={props.generalDetails.assignmentReviewAndModerationProcess ? props.generalDetails.assignmentReviewAndModerationProcess :""}
                            onSelectChange={props.onChangeHandler}
                        /> : null
                    }
                    { props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE ? null :
                        <div>
                        <LabelWithValue
                            className="custom-Badge col"
                            colSize="s5 mb-1"
                            label={` ${
                                localConstant.assignments.PERCENTAGE_COMPLETE
                                }: `}
                            value={props.generalDetails.assignmentPercentageCompleted} />
                        <LabelWithValue
                            className="custom-Badge col"
                            colSize="s7 mb-1"
                            label={` ${
                                localConstant.assignments.EXPECTED_COMPLETED_DATE
                                }: `}
                            value={props.generalDetails.assignmentExpectedCompleteDate ? moment(props.generalDetails.assignmentExpectedCompleteDate).format(localConstant.commonConstants.EXPECTED_DATE_FORMAT):''} />
                        </div> 
                    }
                    <CustomInput
                        type='switch'
                        switchLabel={localConstant.assignments.CUSTOMER_FORMAT_REPORT_REQUIRED}
                        isSwitchLabel={true}
                        switchName="isAssignmentCustomerFormatReportRequired"
                        id="isAssignmentCustomerFormatReportRequired"
                        colSize="s12 mt-2"
                        disabled={props.interactionMode||props.isInterCompanyAssignment}
                        checkedStatus={props.generalDetails.isAssignmentCustomerFormatReportRequired}
                        onChangeToggle={props.onChangeHandler}
                        switchKey={props.generalDetails.isAssignmentCustomerFormatReportRequired? true : false} /> 
                </div>
                <div className='col s12 m6'>
                    <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.assignments.CUSTOMER_ASSIGNMENT_CONTACT}
                        type='select'
                        colSize='s12'
                        className="browser-default"
                        optionsList={props.customerAssignmentContact}
                        labelClass="customLabel mandate"
                        optionName='contactPersonName'
                        optionValue="contactPersonName"
                        name="assignmentCustomerAssigmentContact"
                        id="assignmentCustomerAssigmentContact"
                        disabled={props.interactionMode||props.isInterCompanyAssignment}
                        defaultValue={props.generalDetails.assignmentCustomerAssigmentContact}
                        onSelectChange={props.onChangeHandler} />
                    <div className="col s12">
                        <a className={props.interactionMode === true ? "isDisabled" : null} href={"mailto: " + props.assignmentContactEmail}>
                            {props.assignmentContactEmail}</a>
                    </div>
                    <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.assignments.ASSIGNMENT_COMPANY_ADDRESS}
                        type='select'
                        colSize='s12'
                        className="browser-default"
                        isTooltip={true}
                        optionsList={props.assignmentCompanyAddress}
                        labelClass="customLabel mandate"
                        optionName='address'
                        optionValue="address"
                        name="assignmentCompanyAddress"
                        id="assignmentCompanyAddress"
                        disabled={props.interactionMode||props.isInterCompanyAssignment}
                        defaultValue={props.generalDetails.assignmentCompanyAddress}
                        onSelectChange={props.onChangeHandler} />
                </div>
            </div>
            <div className="row">
                <div className='col s12'>
                    <CustomInput
                        hasLabel={true}
                        divClassName='col'
                        label={localConstant.assignments.CUSTOMER_REPORTING_REQUIREMENTS}
                        type='textarea'
                        colSize='s12'
                        inputClass="customInputs"
                        maxLength={fieldLengthConstants.assignment.generalDetails.CUSTOMER_REPORTING_REQUIREMENTS}
                        name="clientReportingRequirements"
                        readOnly={props.interactionMode || props.isInterCompanyAssignment}
                        value={props.generalDetails.clientReportingRequirements ? props.generalDetails.clientReportingRequirements : ""}
                        onValueChange={props.onChangeHandler} />
                </div>
            </div>
        </CardPanel>
    );
};

class GeneralDetails extends Component {   
    constructor(props) {
        super(props);
        this.updatedData = {};
        this.state = {
            startDate: '',
            endDate: ''    
        };
        this.isEndDateBound = false;
        
        this.fetchEndDate = this.fetchEndDate.bind(this);
        this.fetchStartDate = this.fetchStartDate.bind(this);
        this.checkboxRef= React.createRef();
        this.selectRef=React.createRef();
        this.ocCompanyRef = React.createRef();
        this.switchRefProps = React.createRef();
    }

    selectedRowHandler = () => {
        const redirectionURL = AppMainRoutes.supplierPoDetails;
        const supplierPOId = this.props.assignmentGeneralDetails.assignmentSupplierPurchaseOrderId;
        const queryObj={
            supplierPOId:supplierPOId && supplierPOId,
            selectedCompany:this.props.selectedCompany,
            isAssignmentOpenedAsOC:this.props.isOperatingCompany ? true : false,
            chCompany:this.props.assignmentGeneralDetails.assignmentContractHoldingCompanyCode
        };
        const queryStr = ObjectIntoQuerySting(queryObj);
        window.open(redirectionURL + '?'+queryStr,'_blank');
    }

    /** General Details Change handler */
    generalDetailsOnchangeHandler = (e) => {
        this.updatedData["modifiedBy"] = this.props.loggedInUser;
        if (e.target.name === "isInternalAssignment") {
            if (this.props.isInternalAssignment) {
                this.updatedData[e.target.name] = false;
                this.props.actions.UpdateAssignmentGeneralDetails(this.updatedData);
                const confirmationObject = {
                    title: modalTitleConstant.CONFIRMATION,
                    message: modalMessageConstant.APPEAR_ON_CV,
                    modalClassName: "warningToast",
                    type: "confirm",
                    buttons: [
                        {
                            buttonName: "Yes",
                            onClickHandler: this.hideModal,
                            className: "modal-close m-1 btn-small"
                        },
                        {
                            buttonName: "No",
                            onClickHandler: this.confirmInternalAssignment,
                            className: "modal-close m-1 btn-small"
                        }
                    ]
                };
                this.props.actions.DisplayModal(confirmationObject);

            }
            else {
                this.updatedData["isInternalAssignment"] = true;
                this.props.actions.UpdateAssignmentGeneralDetails(this.updatedData);
                this.switchRefProps.current.value = true;
                const confirmationObject = {
                    title: modalTitleConstant.CONFIRMATION,
                    message: modalMessageConstant.NOT_APPEAR_ON_CV,
                    modalClassName: "warningToast",
                    type: "confirm",
                    buttons: [
                        {
                            buttonName: "Yes",
                            onClickHandler:this.hideModal,
                            className: "modal-close m-1 btn-small"
                        },
                        {
                            buttonName: "No",
                            onClickHandler: this.rejectInternalAssignment,
                            className: "modal-close m-1 btn-small"
                        }
                    ]
                };
                this.props.actions.DisplayModal(confirmationObject);
            }
        }
        if (e.target.name === "assignmentHostCompany") {
            this.updatedData[e.target.name] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text !== localConstant.commonConstants.SELECT ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text : "";
            this.updatedData["assignmentHostCompanyCode"] =  e.target.value;
            this.props.actions.UpdateAssignmentGeneralDetails(this.updatedData);
            this.updatedData = {};
        }
        else if(e.target.name === "workLocationCounty" || e.target.name === "workLocationCountry" || e.target.name === "workLocationCity"){
            this.updatedData[e.target.id]=parseInt(e.target.value);
            this.updatedData[e.target.name]=e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent !== localConstant.commonConstants.SELECT ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent : ""; 
        } //Added for ITK D1536
        else if(e.target.name === "assignmentSupplierPurchaseOrderId"){
            this.updatedData[e.target.name] =  e.target.value;
            this.updatedData["assignmentSupplierPurchaseOrderNumber"] = !isEmpty(e.target.value) ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].innerText : "";
            if(!required(e.target.value)){
                this.supplierPOOnchangeHandler(this.updatedData);
            }
            else{
                this.props.actions.UpdateAssignmentGeneralDetails(this.updatedData);
                this.clearSupplierPODependentData();
            }
        }
        else if(e.target.name === "assignmentStatus" || e.target.name === "assignmentComplete"){
            const result = formInputChangeHandler(e);
            this.assignmentStatusOnChangeHandler(result);
        } 
        else if(e.target.name === "assignmentContractHoldingCompanyCoordinatorCode"){
            this.updatedData[e.target.name] = e.target.value;
            const coordinatorDisplayName = this.props.contractHoldingCompanyCoordinators.filter(x => x.username === e.target.value);
            this.updatedData["assignmentContractHoldingCompanyCoordinator"] = coordinatorDisplayName.length > 0 ? coordinatorDisplayName[0].displayName : null;
        } 
        else if(e.target.name === "assignmentOperatingCompanyCoordinatorCode"){
            this.updatedData[e.target.name] = e.target.value;
            const coordinatorDisplayName = this.props.operatingCompanyCoordinators.filter(x => x.username === e.target.value);
            this.updatedData["assignmentOperatingCompanyCoordinator"] = coordinatorDisplayName.length > 0 ? coordinatorDisplayName[0].displayName : null;
        }
        else {
            const result = formInputChangeHandler(e);
            this.updatedData[ result.name] =  result.value;
            this.props.actions.UpdateAssignmentGeneralDetails(this.updatedData);
            this.updatedData = {};
        }
        this.props.actions.UpdateAssignmentGeneralDetails(this.updatedData);
        if (e.target.name === "assignmentHostCompany") {
            this.props.actions.ClearHostDiscounts(null);
        }
        
        if(e.target.name==="assignmentBudgetValue" || e.target.name==="assignmentBudgetHours"){
            const value =  e.target.value.toString().split('.')[0];
            if (value.length > 16) {
                e.target.value = value.substring(0, 16);
                this.updatedData[e.target.name] = e.target.value;
            }
            this.props.actions.UpdateAssignmentGeneralDetails(this.updatedData);
        }
        if(e.target.name === 'assignmentBudgetWarning' || e.target.name === 'assignmentBudgetHoursWarning')
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
            this.props.actions.UpdateAssignmentGeneralDetails(this.updatedData);
        }
        this.updatedData = {};
    }

    /** Clear supplierPO related data */
    clearSupplierPODependentData = () => {
        this.props.actions.deletSupplierInformations();
        this.props.actions.FetchMainSupplierName();
        this.props.actions.FetchSupplierContacts();
        this.props.actions.FetchSubsuppliers();
    };

    /** Assignment Complete Handler Start */
    assignmentStatusRejectHandler= () =>{
        const assignmentStatus = this.props.assignmentGeneralDetails.assignmentStatus;
        this.updatedData['assignmentStatus'] = assignmentStatus;
        this.updatedData['isAssignmentCompleted'] = assignmentStatus === 'C' ? true : false;
        this.selectRef.current.value = assignmentStatus;
        this.checkboxRef.current.checked = assignmentStatus === 'C' ? true : false;
        this.props.actions.HideModal();
    }
    hideModal = () => {
        this.props.actions.HideModal();
    }
    confirmInternalAssignment = () => {
        this.props.actions.HideModal();
        this.updatedData["isInternalAssignment"] = true;
        this.props.actions.UpdateAssignmentGeneralDetails(this.updatedData);
        this.switchRefProps.current.value = true;
        
    }
    rejectInternalAssignment = () => {
        this.props.actions.HideModal();
        this.updatedData["isInternalAssignment"] = false;
        this.props.actions.UpdateAssignmentGeneralDetails(this.updatedData);
        this.switchRefProps.current.value = false;
        
    }
    assignmentStatusOnChangeHandler = (data) => {
        if(isEmpty(data))
            return false;
        const assignmentStatusValue = data.name === "assignmentComplete" ? data.value ? 'C' : 'P' : data.value;
        const assignmentCompleteCheck = data.name === "assignmentStatus" ? data.value === 'C' ? true : false : data.value;
        const { isVisitAssignment  } = this.props;
        if (isVisitAssignment) {
            this.props.actions.FetchAssignmentVisits().then(response => {
                if (response && response.length > 0) {
                    const finalVisit = response.filter(x => x.finalVisit === "Yes");
                    if (finalVisit.length > 0) {
                        if(assignmentStatusValue !== 'C'){
                            const confirmationObject = {
                                title: modalTitleConstant.CONFIRMATION,
                                message: modalMessageConstant.ASSIGNMENT_COMPLETE_VAL_ON_FINAL_VISIT,
                                modalClassName: "warningToast",
                                type: "confirm",
                                buttons: [
                                    {
                                        buttonName: "Ok",
                                        onClickHandler: this.assignmentStatusRejectHandler,
                                        className: "modal-close m-1 btn-small"
                                    },
                                ]
                            };
                            this.props.actions.DisplayModal(confirmationObject);
                        }
                        else {
                            this.updatedData['assignmentStatus'] = assignmentStatusValue;
                            this.selectRef.current.value = assignmentStatusValue;
                            this.updatedData['isAssignmentCompleted']=assignmentCompleteCheck;
                            this.props.actions.UpdateAssignmentGeneralDetails(this.updatedData);
                            this.updatedData = {};
                        }
                    } else {
                        if(assignmentStatusValue === 'C'){
                            const confirmationObject = {
                                title: modalTitleConstant.CONFIRMATION,
                                message: modalMessageConstant.ASSIGNMENT_STATUS_COMPLETE_VISIT1,
                                modalClassName: "warningToast",
                                type: "confirm",
                                buttons: [
                                    {
                                        buttonName: "Ok",
                                        onClickHandler: this.assignmentStatusRejectHandler,
                                        className: "modal-close m-1 btn-small"
                                    },
                                ]
                            };
                            this.props.actions.DisplayModal(confirmationObject);
                        } else {
                            this.updatedData['assignmentStatus'] = assignmentStatusValue;
                            this.updatedData['isAssignmentCompleted']=assignmentCompleteCheck;
                            this.selectRef.current.value = assignmentStatusValue;
                            this.props.actions.UpdateAssignmentGeneralDetails(this.updatedData);
                            this.updatedData = {};
                        }
                    }
                } else {
                    this.updatedData['assignmentStatus'] = assignmentStatusValue;
                    this.updatedData['isAssignmentCompleted']=assignmentCompleteCheck;
                    this.selectRef.current.value = assignmentStatusValue;
                    this.props.actions.UpdateAssignmentGeneralDetails(this.updatedData);
                    this.updatedData = {};
                }
            });
        } else {
            this.updatedData['assignmentStatus'] = assignmentStatusValue;
            this.updatedData['isAssignmentCompleted']=assignmentCompleteCheck;
            this.selectRef.current.value = assignmentStatusValue;
            this.props.actions.UpdateAssignmentGeneralDetails(this.updatedData);
            this.updatedData = {};
        }
    };
    /** Assignment Complete Handler Ends */

    /** supplierPO Onchange Handler */
    supplierPOOnchangeHandler = (supplierPO) => {
        const { assignmentGeneralDetails }  =this.props;
        const supplierPOId = assignmentGeneralDetails.assignmentSupplierPurchaseOrderId && assignmentGeneralDetails.assignmentSupplierPurchaseOrderId.toString();
        if (!isEmptyOrUndefine(supplierPOId)) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.SUPPLIER_PO_CHANGE_MESSAGE,
                modalClassName: "warningToast",
                type: "confirm",
                buttons: [
                    {
                        buttonName: "Yes",
                        onClickHandler: (e) => this.deleteSupplierInfos(e, supplierPO),
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: "No",
                        onClickHandler: this.supplierConfirmationRejectHandler,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        } else {
                supplierPO.isSupplierPOChanged = false;
                this.props.actions.UpdateAssignmentGeneralDetails(supplierPO);
                this.populateMainSupplier(supplierPO);
                this.updatedData = {};
        }
    };

    /** Populate main supplier details and Sub-Suppliers */
    populateMainSupplier = (supplierPOData) =>{
        const { supplierPO } = this.props;
        const supplierPOObj = arrayUtil.FilterGetObject(supplierPO, "supplierPOId", parseInt(supplierPOData.assignmentSupplierPurchaseOrderId));
        this.props.actions.addMainSupplierData(supplierPOObj);
        this.props.actions.FetchMainSupplierName(supplierPOObj);//D351
        supplierPOData.isSupplierPOChanged = this.props.currentPage === localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE ? true : false;
        this.props.actions.UpdateAssignmentGeneralDetails(supplierPOData);
        document.getElementById('supplierPurchaseOrderId').value = supplierPOData.assignmentSupplierPurchaseOrderId;
        this.updatedData = {};
    };

    /** Delete supplier related informations */
    deleteSupplierInfos =async (e,supplierPOData) => {
        const response = await this.props.actions.deletSupplierInformations();
        if(response){
            this.populateMainSupplier(supplierPOData);
        }
        this.props.actions.HideModal();
    };

    /** Start date updation handler */
    fetchStartDate = (date) => {
        //To-DO the code to be improved
        let fromDate='';
        if(this.props.isTimesheetAssignment){
            //TIMESHEET_DETAILS
            fromDate = "timesheetFromDate";
        }else{
            //Visit Details
            fromDate = "visitFromDate";
        }
        this.setState({
            startDate: date
        }, () => {
            this.updatedData[fromDate] = this.state.startDate !== null ? this.state.startDate.format() : '';
            this.props.actions.UpdateAssignmentGeneralDetails(this.updatedData);

            // The End Date should be bound to Start Date in contract Create Mode for the very first date change of start date.
            if(!this.isEndDateBound )
            {
                this.fetchEndDate(date);
                this.isEndDateBound = true;
            }
            this.updatedData = {};
        });
    }

    /** End date updation handler */
    fetchEndDate = (date) => {
        let toDate='';
        if(this.props.isTimesheetAssignment){
            //TIMESHEET_DETAILS
            toDate ="timesheetToDate";
        }else{
            //Visit Details
            toDate = "visitToDate";
        }
        this.setState({
            endDate: date
        }, () => {
            this.updatedData[toDate] = this.state.endDate !== null ? this.state.endDate.format() : "";
            this.props.actions.UpdateAssignmentGeneralDetails(this.updatedData);
            this.updatedData = {};
        });
    }

    onWarningBlur =(e) => {
        // Def - 890
        // if(!e.target.value)
        //     e.target.value = 75;
        this.generalDetailsOnchangeHandler(e);
    }

    validatePercentage =(e) => {
        if (e.charCode === 45 || e.charCode === 46 ) {
            e.preventDefault();
        }
    }
    /** Location change handler */
    locationChangeHandler = (e) => {
        if (e.target.name === 'workLocationCountry') {
            this.props.actions.FetchTimesheetState();
            this.props.actions.FetchTimesheetCity();
            this.props.actions.FetchTimesheetState(e.target.value);
            this.updatedData['workLocationCounty'] = null;
            this.updatedData['workLocationCity'] = null;
        }
        if (e.target.name === 'workLocationCounty') {
            this.props.actions.FetchTimesheetCity();
            this.props.actions.FetchTimesheetCity(e.target.value);
            this.updatedData['workLocationCity'] = null;
        }
        this.generalDetailsOnchangeHandler(e);
    };

    operatingCompanyChangeHandler = (e) => {
        const value = e.target.value;
        const name = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text !== localConstant.commonConstants.SELECT ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text : "";
        const opCompanyList = value && [ value ];
        const { 
            AssignmentContractSchedules,
            AssignmentTechnicalSpecialists,
            AssignmentInterCompanyDiscounts,
            AssignmentAdditionalExpenses } = this.props.assignmentDetail;
        const icDiscountChanged = !isEmpty(AssignmentInterCompanyDiscounts) ? 
                                    this.props.isInterCompanyAssignment ? true : this.props.isInterCompanyDiscountChanged : false; 
        
        if(this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE) {
            let tabsToValidate = "";
            tabsToValidate = !isEmpty(AssignmentContractSchedules) ?
                                localConstant.assignments.contractRateSchedule.CONTRACT_RATE_SCHEDULES : tabsToValidate;
            tabsToValidate = !isEmpty(AssignmentTechnicalSpecialists) ? 
                                tabsToValidate ? 
                                `${ tabsToValidate } \n ${ localConstant.assignments.Assigned_Specialists }` 
                                : localConstant.assignments.Assigned_Specialists : tabsToValidate;
            tabsToValidate = !isEmpty(AssignmentAdditionalExpenses) ?
                                tabsToValidate ?
                                `${ tabsToValidate } \n ${ localConstant.assignments.ADDITIONAL_EXPENSES }`
                                : localConstant.assignments.ADDITIONAL_EXPENSES : tabsToValidate;
            tabsToValidate = icDiscountChanged ?
                                tabsToValidate ?
                                `${ tabsToValidate } \n ${ localConstant.assignments.INTER_COMPANY_DISCOUNT_TAB_NAME }`
                                : localConstant.assignments.INTER_COMPANY_DISCOUNT_TAB_NAME : tabsToValidate;
            if(!required(tabsToValidate)) {
                const confirmationObject = {
                    title: modalTitleConstant.CONFIRMATION,
                    message: StringFormat(modalMessageConstant.CHANGING_OPERATING_COMPANY,tabsToValidate),
                    type: "confirm",
                    modalClassName: "warningToast",
                    buttons: [
                        {
                            buttonName: "Yes",
                            onClickHandler: () => this.changedOperatingCompanyCode(name, value),
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
            } else {
                this.props.actions.FetchCoordinatorForOperatingCompany(opCompanyList);
                if(this.props.isSettlingTypeMargin) { // ITK D - 712 & 715
                    this.props.actions.UpdateAssignmentGeneralDetails({
                        'assignmentOperatingCompany': name, 
                        'assignmentOperatingCompanyCode': value, 
                        'assignmentOperatingCompanyCoordinator': '',
                        'assignmentOperatingCompanyCoordinatorCode': '',
                        'assignmentHostCompany': name,
                        'assignmentHostCompanyCode': value
                    });
                }
                else {
                    this.props.actions.UpdateAssignmentGeneralDetails({
                        'assignmentOperatingCompany': name,
                        'assignmentOperatingCompanyCode': value,
                        'assignmentOperatingCompanyCoordinator': '',
                        'assignmentOperatingCompanyCoordinatorCode': ''
                    });
                }
                if (value !== this.props.assignmentGeneralDetails.assignmentContractHoldingCompanyCode) {
                    this.props.actions.UpdateAssignmentGeneralDetails({
                        'visitFromDate': '',
                        'visitToDate': '',
                        'timesheetFromDate': '',
                        'timesheetToDate': '',
                        'visitStatus': null,
                        'timesheetStatus': null,
                    });
                }
            }
        }
    }

    changedOperatingCompanyCode = (name, value) => {
        const opCompanyList = value && [ value ];
        this.props.actions.FetchCoordinatorForOperatingCompany(opCompanyList);
        if(this.props.isSettlingTypeMargin) { // ITK D - 712 & 715
            this.props.actions.UpdateAssignmentGeneralDetails({
                'assignmentOperatingCompany': name, 
                'assignmentOperatingCompanyCode': value, 
                'assignmentOperatingCompanyCoordinator': '',
                'assignmentOperatingCompanyCoordinatorCode': '',
                'assignmentHostCompany': name,
                'assignmentHostCompanyCode': value
            });
        }
        else {
            this.props.actions.UpdateAssignmentGeneralDetails({
                'assignmentOperatingCompany': name,
                'assignmentOperatingCompanyCode': value,
                'assignmentOperatingCompanyCoordinator': '',
                'assignmentOperatingCompanyCoordinatorCode': ''
            });
        }
        if (value !== this.props.assignmentGeneralDetails.assignmentContractHoldingCompanyCode &&
            this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE) {
            this.props.actions.UpdateAssignmentGeneralDetails({
                'visitFromDate': '',
                'visitToDate': '',
                'timesheetFromDate': '',
                'timesheetToDate': '',
                'visitStatus': null,
                'timesheetStatus': null,
            });
        }
        this.props.actions.HideModal();
        this.props.actions.ClearICDependentData();
    }

    supplierConfirmationRejectHandler= () =>{
        const supplierPOId = this.props.assignmentGeneralDetails.assignmentSupplierPurchaseOrderId;
        document.getElementById('supplierPurchaseOrderId').value = supplierPOId;
       this.props.actions.HideModal();
    }

    confirmationRejectHandler = () => {
        this.props.actions.UpdateAssignmentGeneralDetails({
            'assignmentOperatingCompany': this.props.assignmentGeneralDetails.assignmentOperatingCompany,
            'assignmentOperatingCompanyCode': this.props.assignmentGeneralDetails.assignmentOperatingCompanyCode
        });
        this.ocCompanyRef.current.value = this.props.assignmentGeneralDetails.assignmentOperatingCompanyCode;
        this.props.actions.HideModal();
    }

    isHostCompany() {
        const isHost = {
            isShow:false,
            isDisable:true
        };
        if(this.props.isSettlingTypeMargin){ // ITK D - 712 & 715
            isHost.isShow = true;
            isHost.isDisable = false;
        }
        return isHost;
    };

    render() {
        const {
            companyList,
            country,
            state,
            city,
            assignmentGeneralDetails,
            assignmentType,
            assignmentStatus,
            assignmentLifeCycle,
            contractHoldingCompanyCoordinators,
            operatingCompanyCoordinators,
            customerAssignmentContact,
            assignmentCompanyAddress,
            supplierPO,
            currentPage,
            interactionMode,
            selectedCompany,
            isInterCompanyAssignment,
            isOperatorCompany,
            isbtnDisable,
            assignmentValidationData,    //Added for Assignment Lifecycle Validation
            reviewAndModerationProcess,
            chCoordinatorEmail,
            opCoordinatorEmail,
            assignmentContactEmail,
            isInternalAssignment
        } = this.props;
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };

        // configure this data in constructor and update the field value and readOnly property alone inside render.
        //*************************Budget Monetary Section STARTS **********************/
        const budgetData = budgetConfig; 
        budgetData.monetaryValues = {
            ...budgetConfig.monetaryValues,
            readOnly:this.props.interactionMode||(isInterCompanyAssignment && isOperatorCompany),
            value:assignmentGeneralDetails.assignmentBudgetValue?thousandFormat(assignmentGeneralDetails.assignmentBudgetValue):'',
            onValueChange:this.generalDetailsOnchangeHandler,
        };
        budgetData.monetaryWarning = {
            ...budgetConfig.monetaryWarning,
            readOnly:this.props.interactionMode||(isInterCompanyAssignment && isOperatorCompany),
            value:assignmentGeneralDetails.assignmentBudgetWarning, //Changes for D1383
            onValueChange:this.generalDetailsOnchangeHandler,
            onValueBlur:this.onWarningBlur,
            onValueKeypress: this.validatePercentage
        };
        budgetData.monetaryCurrency = {
            ...budgetConfig.monetaryCurrency,
            value:assignmentGeneralDetails.assignmentBudgetCurrency,
            defaultValue:assignmentGeneralDetails.assignmentBudgetCurrency ? assignmentGeneralDetails.assignmentBudgetCurrency:'',
        };
        budgetData.monetaryTaxes.forEach(x => {
            if(currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE)
                x.className = "hide";
            if(x.id === "warningPercentage"){
                x.label = assignmentGeneralDetails.assignmentBudgetValueWarningPercentage > 0 ? thousandFormat(assignmentGeneralDetails.assignmentBudgetValueWarningPercentage)  + " " + ` ${ localConstant.commonConstants.ASSIGNMENT_BUDGET_WARNING } ` :null;
                x.className = this.props.currentPage !== localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE  && assignmentGeneralDetails.assignmentBudgetValueWarningPercentage <= 0 ?"hide":this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE ? "hide" :"custom-Badge col text-red-parent";
            }
            x.value = x.id === "invoicedToDate" ? assignmentGeneralDetails.assignmentInvoicedToDate : 
                        x.id === "uninvoicedToDate" ? assignmentGeneralDetails.assignmentUninvoicedToDate :
                        x.id === "remaining" ? assignmentGeneralDetails.assignmentRemainingBudgetValue : x.value;
        });
        budgetData.unitValues.forEach(x => {
            if(x.id === "unitValue"){
                x.readOnly = this.props.interactionMode||(isInterCompanyAssignment && isOperatorCompany);
                x.value = assignmentGeneralDetails.assignmentBudgetHours ===0 ?'0.00':assignmentGeneralDetails.assignmentBudgetHours?thousandFormat(assignmentGeneralDetails.assignmentBudgetHours):'';
                x.onValueChange = this.generalDetailsOnchangeHandler;
            }
            if(x.id === "unitWarning"){
                x.readOnly = this.props.interactionMode||(isInterCompanyAssignment && isOperatorCompany);
                x.value = assignmentGeneralDetails.assignmentBudgetHoursWarning; //Changes for D1383
                x.onValueChange = this.generalDetailsOnchangeHandler;
                x.onValueBlur = this.onWarningBlur;
                x.onValueKeypress = this.validatePercentage;
            }
        });
        budgetData.unitTaxes.forEach(x =>{
            if(currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE)
                x.className = "hide";
            if(x.id === "warningPercentageUnit"){
                x.label = assignmentGeneralDetails.assignmentBudgetHourWarningPercentage > 0 ? thousandFormat(assignmentGeneralDetails.assignmentBudgetHourWarningPercentage)  + " " + ` ${ localConstant.commonConstants.ASSIGNMENT_BUDGET_WARNING } ` :null;
                x.className = this.props.currentPage !== localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE  && assignmentGeneralDetails.assignmentBudgetHourWarningPercentage <= 0 ?"hide": this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE ? "hide" :"custom-Badge col text-red-parent";
            }
            x.value = x.id === "invoicedToDateUnit" ? assignmentGeneralDetails.assignmentHoursInvoicedToDate : 
                        x.id === "uninvoicedToDateUnit" ? assignmentGeneralDetails.assignmentHoursUninvoicedToDate :
                        x.id === "remainingUnit" ? assignmentGeneralDetails.assignmentRemainingBudgetHours : x.value;
        });        
        //End
        
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                <div className="genralDetailContainer customCard">
                    <BasicDetails
                        generalDetails={assignmentGeneralDetails}
                        assignmentType={assignmentType}
                        assignmentStatus={assignmentStatus}
                        onChangeHandler={this.generalDetailsOnchangeHandler}
                        selectedRowHandler = {this.selectedRowHandler}
                        supplierPO={supplierPO}
                        currentPage={currentPage}
                        interactionMode={interactionMode}
                        selectedCompany={this.props.selectedCompany}
                        isTimesheetAssignment = {this.props.isTimesheetAssignment}
                        isAssignmentComplete={
                            (this.props.isVisitAssignment && this.props.assignmentGeneralDetails.assignmentStatus === "C") 
                            || (this.props.isTimesheetAssignment && this.props.assignmentGeneralDetails.assignmentStatus === "C" ) 
                        }
                        isOperatingCompany= {this.props.isInterCompanyAssignment && this.props.isOperatorCompany }
                        visitList={ this.props.visitList }
                        checkboxRefProps={ this.checkboxRef }
                        selectRefProps={ this.selectRef }
                        isInternalAssignment = { isInternalAssignment }
                        switchRefProps={ this.switchRefProps }
                        />
                    <ComapniesAndCoordinators
                        company={companyList}
                        country={country}
                        state={state}
                        city={city}
                        locationChangeHandler={this.locationChangeHandler}
                        onChangeHandler={this.generalDetailsOnchangeHandler}
                        operatingCompanyChangeHandler={this.operatingCompanyChangeHandler}
                        generalDetails={assignmentGeneralDetails}
                        contractHoldingCompanyCoordinators={ contractHoldingCompanyCoordinators }
                        operatingCompanyCoordinators={ operatingCompanyCoordinators }
                        contractHoldingCompanyCoordinatorEmail={chCoordinatorEmail}
                        operatingCompanyCoordinatorEmail={opCoordinatorEmail}
                        interactionMode={interactionMode}
                        currentPage={currentPage}
                        selectedCompany={selectedCompany}
                        isHostCompany={this.isHostCompany()}
                        isOperatingCoordinatorMandatory = { (isInterCompanyAssignment && isOperatorCompany) || !isInterCompanyAssignment }
                        isTimesheetAssignment = {this.props.isTimesheetAssignment}
                        ocCompanyRef = { this.ocCompanyRef }
                    />
                    <FirstVisit
                        generalDetails={assignmentGeneralDetails}
                        onChangeHandler={this.generalDetailsOnchangeHandler}
                        fetchStartDate={this.fetchStartDate}
                        fetchEndDate={this.fetchEndDate}
                        currentPage={currentPage}
                        interactionMode={interactionMode}
                        isInterCompanyAssignment={isInterCompanyAssignment}
                        isOperatorCompany={isOperatorCompany}
                        isTimesheetAssignment = {this.props.isTimesheetAssignment}
                        isbtnDisable={isbtnDisable} />
                    <BudgetMonetary 
                        monetaryValues={budgetData.monetaryValues}
                        monetaryCurrency={budgetData.monetaryCurrency}
                        monetaryTaxes={budgetData.monetaryTaxes}
                        unitValues={budgetData.unitValues}
                        unitTaxes={budgetData.unitTaxes}
                        isCurrencyEditable={false}                
                        monetaryWarning={budgetData.monetaryWarning} />
                    <OtherInformation
                        generalDetails={assignmentGeneralDetails}
                        assignmentLifeCycle={ assignmentLifeCycle }
                        isProjectForNewFacility = {assignmentValidationData.isProjectForNewFacility}
                        customerAssignmentContact={customerAssignmentContact && arrayUtil.sort(customerAssignmentContact,'contactPersonName','asc')}
                        assignmentCompanyAddress={assignmentCompanyAddress}
                        onChangeHandler={this.generalDetailsOnchangeHandler}
                        reviewAndModerationProcess={reviewAndModerationProcess}
                        assignmentContactEmail={assignmentContactEmail}
                        interactionMode={interactionMode}
                        currentPage={this.props.currentPage}
                        isInterCompanyAssignment={this.props.isInterCompanyAssignment && this.props.isOperatorCompany}
                        isVisitAssignment = {this.props.isVisitAssignment} />
                </div>
            </Fragment>
        );
    }
}

export default GeneralDetails;
GeneralDetails.propTypes = {
    companyList: PropTypes.array,
    country: PropTypes.array,
    state: PropTypes.array,
    city: PropTypes.array,
    assignmentGeneralDetails: PropTypes.object,
    assignmentStatus: PropTypes.array,
    assignmentType: PropTypes.array,
    assignmentLifeCycle: PropTypes.array,
    contractHoldingCompanyCoordinators: PropTypes.array,
    operatingCompanyCoordinators: PropTypes.array,
    customerAssignmentContact: PropTypes.array,
    assignmentCompanyAddress: PropTypes.array,
    supplierPO: PropTypes.array,
    selectedCompany: PropTypes.array,
};

GeneralDetails.defaultProps = {
    companyList: [],
    country: [],
    state: [],
    city: [],
    assignmentGeneralDetails: {},
    assignmentStatus: [],
    assignmentType: [],
    assignmentLifeCycle: [],
    contractHoldingCompanyCoordinators: [],
    operatingCompanyCoordinators: [],
    customerAssignmentContact: [],
    assignmentCompanyAddress: [],
    supplierPO: [],
    selectedCompany: [],
};