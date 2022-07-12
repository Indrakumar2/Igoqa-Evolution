import React, { Component, Fragment } from 'react';
import { Link } from 'react-router-dom';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import CustomMultiSelect from '../../../../common/baseComponents/multiSelect';
import {
    getlocalizeData,
    formInputChangeHandler,
    isEmptyReturnDefault,
    isEmptyOrUndefine,
    isEmpty,
    ObjectIntoQuerySting,
    RemoveDuplicateArray //MS-TS sceario 9
} from '../../../../utils/commonUtils';
import moment from 'moment';
import dateUtil from '../../../../utils/dateUtil';
import AssignmentAnchor from '../../../viewComponents/assignment/assignmentAnchor';
import SupplierpoAnchor from '../../../viewComponents/supplierpo/supplierpoAnchor';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { isVisitCreateMode } from '../../../../selectors/visitSelector';
import { visitTabDetails } from '../../../viewComponents/visit/visitDetails/visitTabsDetails';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';

//Visit Calendar
import ResourceScheduler from '../../../../components/applicationComponents/scheduler';
import { schedulerConfigData } from '../../../../components/applicationComponents/scheduler/schedulerconfig';
import Modal from '../../../../common/baseComponents/modal';
import { CalendarTaskInfoView, CalendarPTOTaskInfoView,CalendarTaskEditView } from '../../../../grm/components/applicationComponent/calendarPopupTemplate/calendarTaskInfoView';
import { AppMainRoutes } from '../../../../routes/routesConfig';

const localConstant = getlocalizeData();
const currentDate = moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT);

function createMultiSelectOptions(array, supplierId, subSupplierList, visitTechSpecList) {
    const options = isEmptyReturnDefault(array);
    const multiselectArray = [];
    options.length > 0 && options.map(eachItem => {        
        const supplierIds = [];
        if(supplierId && subSupplierList) {            
            subSupplierList.forEach(item => {    
                item.assignmentSubSupplierTS.forEach(subSupplierTS => {     
                    if(!subSupplierTS.isDeleted && subSupplierTS.epin === parseInt(eachItem.epin)) {   
                        supplierIds.push(item.supplierType === "M" ? item.mainSupplierId : item.subSupplierId);                        
                    }
                    // HOT fix 777 - Deleted Supplier TS is not showing in visit
                    if(subSupplierTS.isDeleted && subSupplierTS.epin === parseInt(eachItem.epin)) {   
                        if(visitTechSpecList && visitTechSpecList.length > 0 &&
                            visitTechSpecList.filter(x => x.pin === parseInt(eachItem.epin)).length > 0) {
                                supplierIds.push(item.supplierType === "M" ? item.mainSupplierId : item.subSupplierId);
                        }
                    }
                });                
            }); 
        }

        if(supplierId && supplierIds.includes(parseInt(supplierId))) {
            multiselectArray.push({ value: eachItem.epin, label: eachItem.technicalSpecialistName.includes(eachItem.epin) ? eachItem.technicalSpecialistName : eachItem.technicalSpecialistName + " (" +  eachItem.epin + ")", 
            color: (eachItem.profileStatus === localConstant.assignments.INACTIVE ? localConstant.visit.RESOURCE_COLOR.INACTIVE : localConstant.visit.RESOURCE_COLOR.ACTIVE) });
        }
    });
    return multiselectArray;
}

function selectMultiSelectOptions(array, techSpecList) {
    const options = isEmptyReturnDefault(array);
    const multiselectArray = [];
    if (options.length > 0) {
        options.map(eachItem => {
            if (eachItem.recordStatus == null || eachItem.recordStatus !== 'D') {
                const profileStatus = techSpecList.filter(ts => ts.epin === eachItem.pin && ts.profileStatus === localConstant.assignments.INACTIVE).length > 0;
                multiselectArray.push({ value: eachItem.pin, label: eachItem.technicalSpecialistName.includes(eachItem.pin) ? eachItem.technicalSpecialistName : eachItem.technicalSpecialistName + " (" +  eachItem.pin + ")", 
                    color: (profileStatus ? localConstant.visit.RESOURCE_COLOR.INACTIVE : localConstant.visit.RESOURCE_COLOR.ACTIVE) });
            }
        });
    }
    return multiselectArray;
}

export const GeneralDetailsDiv = (props) => {
    const isTechSpecControlDisable = () => {
        if (!props.interactionMode
            && !isEmptyOrUndefine(props.visitInfo.visitStartDate)
            && !isEmptyOrUndefine(props.visitInfo.visitEndDate)) {
            return false;
        }
        return true;
    };
    const isDisableFromToDates = (props.visitInfo.visitStatus === 'A' || props.visitInfo.visitStatus === 'O' 
                                        ? (props.isInterCompanyAssignment ? true : false) : false);
    return (<form autoComplete="off">
        <div>
           <span> <label className="bold marginRight">{localConstant.visit.GENERAL_DETAILS}</label></span>

            <span><label class="">{localConstant.visit.ASSIGNMENT_NO + ': '}</label>
            <AssignmentAnchor
                assignmentNumber={props.visitInfo.visitAssignmentNumber ?
                    props.visitInfo.visitAssignmentNumber.toString().padStart(5, '0') : ''}
                assignmentId={props.visitInfo.visitAssignmentId} /></span>
        </div>
        
        <div className="row mb-0">
            {/* <CustomInput
                hasLabel={true}
                name="customer"
                colSize='s4 pr-3'
                label={localConstant.visit.CUSTOMER_NAME}
                type='text'
                inputClass="customInputs"
                maxLength={fieldLengthConstants.visit.generalDetails.CUSTOMER_NAME_MAXLENGTH}
                readOnly={true}
                onValueChange={props.inputHandleChange}
                defaultValue={props.visitInfo.visitCustomerName}
            /> */}

            <div className='col s4 pr-3 row mb-0 widthChange4' title={props.visitInfo.visitCustomerName}>
                <label className='customLabel'>{localConstant.visit.CUSTOMER_NAME}</label>
                <div className='textNoWrapEllipsis'>{props.visitInfo.visitCustomerName} </div>
            </div>
            <CustomInput
                hasLabel={true}
                name="visitStatus"
                colSize='s4 pl-0'
                label={localConstant.visit.VISIT_STATUS}
                labelClass="mandate"
                type='select'
                optionsList={props.visitStatus}
                optionName='name'
                optionValue='code'
                inputClass="customInputs"
                onSelectChange={props.inputHandleChange}
                defaultValue={props.visitInfo.visitStatus}
                disabled={props.isVisitStatusDisabled || props.interactionMode}
            />
            <CustomInput
                hasLabel={true}
                name="unusedReason"
                colSize='s4 pl-0'
                label={localConstant.visit.UNUSED_REASON}
                labelClass="mandate"
                type='select'
                optionsList={props.unusedReason}
                optionName='name'
                optionValue='code'
                inputClass="customInputs"
                onSelectChange={props.inputHandleChange}
                defaultValue={props.visitInfo.visitStatus === "D" ? props.visitInfo.unusedReason : ""}
                disabled={props.visitInfo.visitStatus !== "D" || props.interactionMode}
            />
        </div>

        <div className="row mb-0">
            <div className="col s8 pl-0">
                <CustomInput
                    hasLabel={true}
                    name="visitStartDate"
                    colSize='s6'
                    label={localConstant.visit.DATE_FROM}
                    labelClass="mandate"
                    type='date'
                    inputClass="customInputs"
                    maxLength={60}
                    // onValueChange={props.inputHandleChange}
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    autoComplete="off"
                    selectedDate={dateUtil.defaultMoment(props.visitInfo.visitStartDate)}
                    onDateChange={props.visitStartDateChange}
                    shouldCloseOnSelect={true}
                    onDatePickBlur={(e) => { props.handleDateChangeRaw(e, "visitStartDateChange"); }}
                    disabled={props.interactionMode || isDisableFromToDates}
                />
                <CustomInput
                    hasLabel={true}
                    name="visitEndDate"
                    colSize='s6 pl-0'
                    label={localConstant.visit.DATE_TO}
                    labelClass="mandate"
                    type='date'
                    inputClass="customInputs"
                    maxLength={60}
                    // onValueChange={props.inputHandleChange}
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    autoComplete="off"
                    selectedDate={dateUtil.defaultMoment(props.visitInfo.visitEndDate)}
                    onDateChange={props.visitEndDateChange}
                    shouldCloseOnSelect={true}
                    onDatePickBlur={(e) => { props.handleDateChangeRaw(e, "visitEndDateChange"); }}
                    disabled={props.interactionMode || isDisableFromToDates}
                />
                <CustomInput
                    hasLabel={true}
                    name="visitReportNumber"
                    colSize='s6'
                    label={localConstant.visit.REPORT_NO}
                    type='text'
                    dataValType='valueText'
                    inputClass="customInputs"
                    maxLength={fieldLengthConstants.visit.generalDetails.REPORT_NUMBER_MAXLENGTH}
                    onValueChange={props.inputHandleChange}
                    value={props.visitInfo.visitReportNumber ? props.visitInfo.visitReportNumber : ''}
                    readOnly={props.interactionMode || props.isOCApprovedByOC || props.isOCApprovedByCH}
                />

                <CustomInput
                    hasLabel={true}
                    name="visitReportSentToCustomerDate"
                    colSize='s6 pl-0'
                    label={localConstant.visit.DATE_REPORT_SENDTO_CUSTOMER}
                    type='date'
                    inputClass="customInputs"
                    maxLength={60}
                    // onValueChange={props.inputHandleChange}
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    autoComplete="off"
                    selectedDate={dateUtil.defaultMoment(props.visitInfo.visitReportSentToCustomerDate)}
                    onDateChange={props.visitCustomerDateChange}
                    shouldCloseOnSelect={true}
                    onDatePickBlur={(e) => { props.handleDateChangeRaw(e, "visitCustomerDateChange"); }}
                    disabled={props.visitInfo.visitStatus === "A" || props.interactionMode}
                />
                <CustomInput
                    hasLabel={true}
                    name="supplierId"
                    id="supplierId"
                    colSize='s6'
                    label={localConstant.visit.SUPPLIER}
                    type='select'
                    optionsList={props.supplierList}
                    optionName='supplierName'
                    optionValue='supplierId'
                    inputClass="customInputs"
                    onSelectChange={props.inputHandleChange}
                    defaultValue={props.visitInfo.supplierId}                    
                    readOnly={props.hasSupplierFirstVisit}
                    disabled={props.interactionMode || props.supplierReadonly || props.isOperatorApporved}
                    labelClass="mandate"
                />
                <CustomInput
                    hasLabel={true}
                    name="visitDatePeriod"
                    colSize='s6 pl-0'
                    label={localConstant.visit.VISIT_DATE_PERIOD}
                    type='text'
                    dataValType='valueText'
                    inputClass="customInputs"
                    maxLength={fieldLengthConstants.visit.generalDetails.VISIT_DATE_PERIOD_MAXLENGTH}
                    onValueChange={props.inputHandleChange}
                    value={props.visitInfo.visitDatePeriod ? props.visitInfo.visitDatePeriod : ''}
                    readOnly={props.interactionMode || props.isOCApprovedByOC || props.isOCApprovedByCH}
                />
            </div>
            {
                props.visitInfo.isExtranetSummaryReportVisible &&
                <div className="col s4 pl-0">
                    <CustomInput
                        hasLabel={true}
                        label={localConstant.visit.SUMMARY_OF_REPORT}
                        divClassName='col pl-0 mb-0'
                        colSize="col s12"
                        type='textarea'
                        required={true}
                        name="summaryOfReport"
                        id="summaryOfReport"
                        rows="4"
                        inputClass=" customInputs textAreaInView"
                        labelClass="customLabel"
                        maxLength={fieldLengthConstants.visit.generalDetails.VISIT_SUMMARY_REPORT_MAXLENGTH}
                        value={props.visitInfo.summaryOfReport ? props.visitInfo.summaryOfReport : ""}
                        onValueChange={props.inputHandleChange}
                        readOnly={props.interactionMode}
                    />
                </div>
            }
        </div>

        <div className="row mb-0">
            <div className="custom-Badge col s12 mt-2 mb-2">
                <CustomInput
                    name="isFinalVisit"
                    type='switch'
                    switchLabel={localConstant.visit.FINAL_VISIT}
                    colSize='s4 pl-0 mt-0'
                    isSwitchLabel={true}
                    className="lever"
                    switchName="isFinalVisit"
                    onChangeToggle={props.inputHandleChange}
                    checkedStatus={props.visitInfo.isFinalVisit}
                    switchKey={props.visitInfo.isFinalVisit}
                    disabled={props.isFinalVisitDisabled}
                />
                <div className="col s4 pl-0">
                    <label class="">{localConstant.visit.SUPPLIER_PO + ': '}</label>
                    <a  href='javascript:void(0)' onClick={props.selectedRowHandler} className={"link"}>{ props.supplierPOData.supplierPONumber }</a>            
                     {/* <Link target="_blank" 
                        to={{ pathname:AppMainRoutes.supplierPoDetails, 
                        search:`?supplierPOId=${ props.supplierPOData.supplierPOId }&selectedCompany=${ props.selectedCompany }` }} className={"link"}>                        
                        {props.supplierPOData.supplierPONumber}
                    </Link> */}
                </div>
            </div>
        </div>

        <div className="row mb-0">
            <CustomInput
                hasLabel={true}
                name="visitReference1"
                colSize='s4'
                label={localConstant.visit.VISIT_REF + ' 1'}
                type='text'
                dataValType='valueText'
                inputClass="customInputs"
                maxLength={fieldLengthConstants.visit.generalDetails.VISIT_REFERENCE_MAXLENGTH}
                onValueChange={props.inputHandleChange}
                value={props.visitInfo.visitReference1 ? props.visitInfo.visitReference1 : ''}
                readOnly={props.interactionMode || props.isOCApprovedByOC || props.isOCApprovedByCH}
            />
            <CustomInput
                hasLabel={true}
                name="visitReference2"
                colSize='s4 pl-0'
                label={localConstant.visit.VISIT_REF + ' 2'}
                type='text'
                dataValType='valueText'
                inputClass="customInputs"
                maxLength={fieldLengthConstants.visit.generalDetails.VISIT_REFERENCE_MAXLENGTH}
                onValueChange={props.inputHandleChange}
                value={props.visitInfo.visitReference2 ? props.visitInfo.visitReference2 : ''}
                readOnly={props.interactionMode || props.isOCApprovedByOC || props.isOCApprovedByCH}
            />
            <CustomInput
                hasLabel={true}
                name="visitReference3"
                colSize='s4 pl-0'
                label={localConstant.visit.VISIT_REF + ' 3'}
                type='text'
                dataValType='valueText'
                inputClass="customInputs"
                maxLength={fieldLengthConstants.visit.generalDetails.VISIT_REFERENCE_MAXLENGTH}
                onValueChange={props.inputHandleChange}
                value={props.visitInfo.visitReference3 ? props.visitInfo.visitReference3 : ''}
                readOnly={props.interactionMode || props.isOCApprovedByOC || props.isOCApprovedByCH}
            />
        </div>

        <div className="row mb-0">
            <CustomInput
                hasLabel={true}
                name="visitCompletedPercentage"
                colSize='s4'
                label={localConstant.visit.PERCENTAGE_COMPLETE}
                type='text'
                dataType='numeric'
                max="100"
                inputClass="customInputs"
                maxLength={fieldLengthConstants.visit.generalDetails.PERCENTAGE_COMPLETE_MAXLENGTH}
                onValueChange={props.onPercentageChange}
                value={props.visitInfo.visitCompletedPercentage ? props.visitInfo.visitCompletedPercentage : ''}
                readOnly={props.interactionMode || props.isTBAVisitStatus || props.isCHApprovedByCH}
            />
            <CustomInput
                hasLabel={true}
                name="visitExpectedCompleteDate"
                colSize='s4 pl-0'
                label={localConstant.visit.EXPECTED_COMPLETED_DATE}
                type='date'
                inputClass="customInputs"
                maxLength={60}
                // onValueChange={props.inputHandleChange}
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                autoComplete="off"
                selectedDate={dateUtil.defaultMoment(props.visitInfo.visitExpectedCompleteDate)}
                onDateChange={props.visitExpectedCompleteDateChange}
                shouldCloseOnSelect={true}
                onDatePickBlur={(e) => { props.handleDateChangeRaw(e, "visitExpectedCompleteDateChange"); }}
                disabled={props.isTBAVisitStatus || props.interactionMode || props.isCHApprovedByCH}
            />
        </div>

        <div className="row mb-0">
            <CustomInput
                hasLabel={true}
                name="visitNotificationReference"
                colSize='s4'
                label={localConstant.visit.NOTIFICATION_REF}
                type='text'
                dataValType='valueText'
                inputClass="customInputs"
                maxLength={fieldLengthConstants.visit.generalDetails.NOTIFICATION_REFERENCE_MAXLENGTH}
                onValueChange={props.inputHandleChange}
                value={props.visitInfo.visitNotificationReference ? props.visitInfo.visitNotificationReference : ''}
                readOnly={props.interactionMode}
            />

            <CustomInput
                hasLabel={true}
                name="techSpecialist"
                colSize='s4 pl-0'
                label={localConstant.visit.SELECT_TECH_SPEC}
                type='multiSelect'
                className='browser-default customInputs'
                multiSelectdValue={props.techSpecChange}
                optionsList={createMultiSelectOptions(props.technicalSpecialistList, props.visitInfo.supplierId, props.subSupplierList, props.visitTechnicalSpecialists)}
                defaultValue={selectMultiSelectOptions(props.visitTechnicalSpecialists, props.technicalSpecialistList)}
                disabled={isTechSpecControlDisable() || props.isOperatorApporved}
                labelClass="mandate"
            />

            {props.isJobReferenceNumberVisible && <CustomInput
                hasLabel={true}
                name="jobReference"
                colSize='s4'
                label={localConstant.visit.JOBREFERENCENO}
                type='text'
                dataValType='valueText'
                inputClass="customInputs"
                readOnly={true}
                value={props.visitInfo.visitJobReference}
            />}
        </div>
    </form>
    );
};

class GeneralDetails extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isPanelOpen: true,
            visitStartDate: '',
            visitEndDate: '',
            visitReportSentToCustomerDate: '',
            visitExpectedCompleteDate: '',
            visitSupplierList: [],
            hasSupplierFirstVisit: false,
            isTBAVisitStatus: false,

            //Calendar
            isOpenCalendarPopUp: false,
            isEditTaskMode: false,
            templateType: "",
            calendarTaskData: {},
            calendarData: {
                resources: [],
                events: []
            },
            visitCalendarStartDate: new Date(),
            isCalendarDataUpdated: false,
            isViewMode: false,
            visitStateAddItem: [],
            prevSelectedTechSpecs: []
        };

        this.updatedData = {};
        this.isVisitStatusDisabled = false;
        this.defaultCalendarData = {
            resources: [
                { id: "", name: "" }
            ],
            events: []
        };

        this.confirmationNoFinalVisit = {
            title: localConstant.login.EVOLUTION,
            message: localConstant.errorMessages.FINAL_VISIT_NO_WARNING,
            modalClassName: "warningToast",
            type: "confirm",
            buttons: [
              {
                buttonName: localConstant.commonConstants.OK,
                onClickHandler: this.calendarWarningPopupClick,
                className: "modal-close m-1 btn-small"
              }
            ]
          };
        
        this.confirmationSupplierChange = {
        title: localConstant.login.EVOLUTION,
        message: localConstant.visit.SUPPLIER_CHANGE_VALIDATION,
        modalClassName: "warningToast",
        type: "confirm",
        buttons: [
            {
            buttonName: localConstant.commonConstants.OK,
            onClickHandler: this.calendarWarningPopupClick,
            className: "modal-close m-1 btn-small"
            }
        ]
        };

        this.confirmationResourceInactive =  {
            title: localConstant.login.EVOLUTION,
            message: localConstant.errorMessages.VISIT_RESOURCE_INACTIVE,
            modalClassName: "warningToast",
            type: "confirm",
            buttons: [
                {
                    buttonName: localConstant.commonConstants.YES,
                    onClickHandler: this.assignResourceInactive,
                    className: "modal-close m-1 btn-small"
                },
                {
                    buttonName: localConstant.commonConstants.NO,
                    onClickHandler: this.calendarWarningPopupClick,
                    className: "modal-close m-1 btn-small"
                }
            ]
        };

        //To assign Calendar resources list
        this.calendarResourceList = [];

        //Buttons for calendar popup functionalities
        this.calendarEditButtons = [
            {
                name: localConstant.commonConstants.SAVE,
                action: this.saveCalendarPopup,
                btnID: "closeErrorList",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
            },
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelCalendarPopup,
                btnID: "closeErrorList",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
            }
        ];
        this.calendarViewButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelCalendarPopup,
                btnID: "closeErrorList",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
            }
        ];

        this.confirmationObject = {
            title: localConstant.globalCalendar.WARNING,
            message: localConstant.globalCalendar.THIS_VISIT_CALENDAR_WARNING,
            modalClassName: "warningToast",
            type: "confirm",
            buttons: [
              {
                buttonName: localConstant.commonConstants.OK,
                onClickHandler: this.calendarWarningPopupClick,
                className: "modal-close m-1 btn-small"
              }
            ]
          };

        //Initialize methods for calendar functionality
        this.calendarView = this.calendarView.bind(this);
        this.cancelCalendarPopup = this.cancelCalendarPopup.bind(this);
        this.saveCalendarPopup = this.saveCalendarPopup.bind(this);
        this.calendarNewEvent = this.calendarNewEvent.bind(this);
        this.calendarTaskStartDateChange = this.calendarTaskStartDateChange.bind(this);
        this.calendarTaskEndDateChange = this.calendarTaskEndDateChange.bind(this);
        this.calendarTaskStartTimeChange = this.calendarTaskStartTimeChange.bind(this);
        this.calendarTaskEndTimeChange = this.calendarTaskEndTimeChange.bind(this);
        this.calendarInputChangeHandler = this.calendarInputChangeHandler.bind(this);
        this.calendarHandlerToBindData = this.calendarHandlerToBindData.bind(this);

        this.props.callBackFuncs.loadCalenderData = () => {
            this.calendarDataBind();
        };

        this.props.callBackFuncs.disableVisitStatus = () => {            
            this.isVisitStatusControlDisabled();
        };

        this.endDateTimeLimit = 0;
        this.endDateTimeMinLimit = 0;
    }

    calendarDataBind = () => {
        this.editVisitCalendarDataBind();
    }

    editVisitCalendarDataBind = () => {
        const techIds = [];
        if (this.props.visitTechnicalSpecialists) {
            if (Array.isArray(this.props.visitTechnicalSpecialists) && this.props.visitTechnicalSpecialists.length > 0) {
                this.props.visitTechnicalSpecialists.map(tech => {
                    if (tech.recordStatus !== 'D')
                        techIds.push(tech.pin);
                });
            }
        }

        if (techIds.length > 0) {
            // Fetch visit calendar data
            this.props.actions.FetchVisitTimesheetCalendarData({
                companyCode: this.props.selectedCompanyCode,
                isActive: true,
                startDateTime: this.props.visitInfo ? dateUtil.getWeekStartDate(new Date(this.props.visitInfo.visitStartDate)) : dateUtil.getWeekStartDate(new Date()),
                endDateTime: this.props.visitInfo ? dateUtil.getWeekEndDate(new Date(this.props.visitInfo.visitEndDate)) : dateUtil.getWeekStartDate(new Date()),
                // calendarType: localConstant.globalCalendar.CALENDAR_STATUS.VISIT,
                // calendarRefCode: this.props.visitInfo.visitId ? this.props.visitInfo.visitId : 0
            }, techIds).then(response => {
                if (response) {
                    this.calendarResourceList = [];
                    //Setting up data to bind in scheduler
                    if (this.props.visitTechnicalSpecialists) {
                        this.props.visitTechnicalSpecialists.map(techSpec => {
                            if (techSpec.recordStatus !== 'D') {
                                this.calendarResourceList.push({
                                    id: techSpec.pin,
                                    name: techSpec.technicalSpecialistName.includes(techSpec.pin) ? techSpec.technicalSpecialistName : techSpec.technicalSpecialistName + " (" +techSpec.pin+ ")"
                                });
                            }
                        });
                    }
                    response.resources = this.calendarResourceList;
                        if (this.props.visitCalendarData) {
                            this.props.visitCalendarData.map(calendar => {
                                if (response.events && response.events.length > 0) {
                                    const index = response.events.findIndex(x => x.id === calendar.id);
                                    if (index >= 0)
                                        response.events.splice(index, 1);
                                }
                                if(calendar.recordStatus!=='D'){
                                calendar.title = calendar.calendarStatus;
                                // calendar.start = moment(calendar.startDateTime).format(localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT);
                                // calendar.end = moment(calendar.endDateTime).format(localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT);
                                calendar.resourceId = calendar.technicalSpecialistId;
                                calendar.bgColor = calendar.title === localConstant.globalCalendar.EVENT_STATUS.BOOKED ? localConstant.globalCalendar.EVENT_COLORS.BOOKED : (calendar.title === localConstant.globalCalendar.EVENT_STATUS.TENTATIVE) ? localConstant.globalCalendar.EVENT_COLORS.TENTATIVE : (calendar.title === localConstant.globalCalendar.EVENT_STATUS.TBA) ? localConstant.globalCalendar.EVENT_COLORS.TBA : localConstant.globalCalendar.EVENT_COLORS.PTO;
                                response.events.push(calendar);
                            }
                            });
                        }
                }
                else {
                    response = {
                        resources: this.calendarResourceList,
                        events: []
                    };
                }
                if(response) {
                    this.setState({
                        calendarData: response,
                        isCalendarDataUpdated: true,
                        visitCalendarStartDate: this.props.visitInfo ? this.props.visitInfo.visitStartDate : new Date()
                    });
                }
            });
        }
        else {
            this.setState({
                calendarData: this.defaultCalendarData,
                isCalendarDataUpdated: true,
                visitCalendarStartDate: this.props.visitInfo ? this.props.visitInfo.visitStartDate : new Date()
            });
        }
    }

    techSpecChange = async (selectedTechSpecs) => {             
        const prevSelectedTechSpecs = this.props.visitTechnicalSpecialists;

        const visitAddItem = selectedTechSpecs.filter(function (el) {
            return !prevSelectedTechSpecs.some(function (f) {
                return f.pin === el.value && f.recordStatus !== 'D';
            });
        });
        let visitRemoveItem = prevSelectedTechSpecs.filter(function (el) {
            return !selectedTechSpecs.some(function (f) {
                return f.value === el.pin && f.recordStatus !== 'D';
            });
        });
        if (visitRemoveItem.length > 0 && visitRemoveItem.filter(x => x.recordStatus !== 'D').length > 0) {
            visitRemoveItem = visitRemoveItem.filter(x => x.recordStatus !== 'D');
            // -- if add remove, Otherwise Mark record status as D for edit Remove tech spec from  VisitTechnicalSpecialists json
            // --if add remove, Otherwise Mark record status as D for edit  any expenses exists for tech spec in the json
            for (const rowData of visitRemoveItem) {
                await this.props.actions.RemoveVisitTechnicalSpecialist(rowData);
                if (this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE) {
                    this.props.actions.RemoveCalendarForTechSpec(rowData.pin);
                }
            }            
            this.editVisitCalendarDataBind();
        }
        if (visitAddItem.length > 0) {
            this.setState({
                prevSelectedTechSpecs: prevSelectedTechSpecs,
                visitStateAddItem: visitAddItem
            });
            
            const result = this.props.technicalSpecialistList.filter(ts => ts.epin === visitAddItem[0].value && ts.profileStatus === localConstant.assignments.INACTIVE);            
            if(result && result.length > 0) {                
                this.props.actions.DisplayModal(this.confirmationResourceInactive);
            } else {
                this.assignResourceInactive(visitAddItem, prevSelectedTechSpecs);                
            }            
        }
    }

    updateDefaultLineItems(startDate) {
        const {
            visitTechnicalSpecialists,
            visitTechnicalSpecialistTimes, 
            visitTechnicalSpecialistTravels,
            visitTechnicalSpecialistExpenses,
            visitTechnicalSpecialistConsumables 
        } = this.props;

        if(visitTechnicalSpecialistTimes && visitTechnicalSpecialistTimes.length > 0) {
            const filteredItemsTime = visitTechnicalSpecialistTimes.filter(x => x.isDefault);
            if(filteredItemsTime && filteredItemsTime.length > 0) {
                this.props.actions.DeleteTechnicalSpecialistTime(filteredItemsTime);
            }
        }
        if(visitTechnicalSpecialistTravels && visitTechnicalSpecialistTravels.length > 0) {
            const filteredItemsTravel = visitTechnicalSpecialistTravels.filter(x => x.isDefault);
            if(filteredItemsTravel && filteredItemsTravel.length > 0) {
                this.props.actions.DeleteTechnicalSpecialistTravel(filteredItemsTravel);
            }
        }
        if(visitTechnicalSpecialistExpenses && visitTechnicalSpecialistExpenses.length > 0) {
            const filteredItemsExpenses = visitTechnicalSpecialistExpenses.filter(x => x.isDefault);
            if(filteredItemsExpenses && filteredItemsExpenses.length > 0) {
                this.props.actions.DeleteTechnicalSpecialistExpense(filteredItemsExpenses);
            }
        }
        if(visitTechnicalSpecialistConsumables && visitTechnicalSpecialistConsumables.length > 0) {
            const filteredItemsConsumable = visitTechnicalSpecialistConsumables.filter(x => x.isDefault);
            if(filteredItemsConsumable && filteredItemsConsumable.length > 0) {
                this.props.actions.DeleteTechnicalSpecialistConsumable(filteredItemsConsumable);
            }
        }

        visitTechnicalSpecialists && visitTechnicalSpecialists.forEach(techSpec => {
            this.CreateDefaultLineItems(techSpec.pin, moment(startDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT));
        });
    }
    
    CreateDefaultLineItems = async (pin, startDate) => {
        const chargeRateGridData = isEmptyReturnDefault(this.props.defaultTechSpecRateSchedules);
        const visitStartDate = moment(this.props.visitInfo.visitStartDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
        const assignmentId = this.props.visitInfo.visitAssignmentId;
        const lineItemsAdded = [];
        let payScheduleCurrency = '';
        let chargeScheduleCurrency = '';
        let incurredCurrency = '';
        const calculateExchangeRates = [];

        this.props.companyList && this.props.companyList.map(company => {
            if (company.companyCode === this.props.visitInfo.visitOperatingCompanyCode) {
                incurredCurrency = company.currency;
            };
        });

        const payRateGridData = isEmptyReturnDefault(this.props.defaultTechSpecRateSchedules);
        if (!isEmptyOrUndefine(chargeRateGridData.chargeSchedules)) {
            chargeRateGridData.chargeSchedules.forEach(row => {
                if(row.epin === pin && chargeScheduleCurrency === '') chargeScheduleCurrency = row.chargeScheduleCurrency;
                if(incurredCurrency !== row.chargeScheduleCurrency) {
                    calculateExchangeRates.push({
                        currencyFrom: incurredCurrency,
                        currencyTo: row.chargeScheduleCurrency,
                        effectiveDate: (startDate ? startDate : visitStartDate)
                    });
                }
            });
        }        
        if (!isEmptyOrUndefine(payRateGridData.paySchedules)) {
            payRateGridData.paySchedules.forEach(row => {
                if(row.epin === pin && payScheduleCurrency === '') payScheduleCurrency = row.payScheduleCurrency;
                if(incurredCurrency !== row.payScheduleCurrency) {
                    calculateExchangeRates.push({
                        currencyFrom: incurredCurrency,
                        currencyTo: row.payScheduleCurrency,
                        effectiveDate: (startDate ? startDate : visitStartDate)
                    });
                }
            });
        }      
        if(calculateExchangeRates && calculateExchangeRates.length > 0) {
            const resultRates = await this.props.actions.FetchCurrencyExchangeRate(calculateExchangeRates, this.props.visitInfo.visitContractNumber);  
            if(resultRates && resultRates.length > 0)
            {
                await this.props.actions.UpdateVisitExchangeRates(resultRates);
            }
        }
        if (!isEmptyOrUndefine(chargeRateGridData.chargeSchedules)) {
            chargeRateGridData.chargeSchedules.forEach(row => {
                if (row.epin === pin) {
                    row.chargeScheduleRates.forEach(rowData => {
                        const isTypeAdded = this.isLineItemAlreadyAdded(rowData.type, rowData.chargeType, pin, lineItemsAdded);
                        if ((isEmptyOrUndefine(rowData.effectiveTo) || new Date(rowData.effectiveTo) >= new Date(visitStartDate))
                            && !isTypeAdded && rowData.isActive) {
                            const tempData = {};
                            if (this.props.currentPage === localConstant.visit.EDIT_VIEW_VISIT_MODE) {
                                tempData.visitId = this.props.visitInfo.visitId;
                            }
                            tempData.expenseDate = (startDate ? startDate : visitStartDate);
                            tempData.chargeExpenseType = rowData.chargeType;
                            tempData.pin = pin;
                            tempData.visitTechnicalSpecialistId = null;
                            tempData.recordStatus = 'N';
                            tempData.TechSpecTimeId = Math.floor(Math.random() * 9999) - 10000;
                            tempData.assignmentId = assignmentId;
                            tempData.contractNumber = this.props.visitInfo.visitContractNumber;
                            tempData.projectNumber = this.props.visitInfo.visitProjectNumber;
                            tempData.chargeRateCurrency = rowData.currency;
                            tempData.payRateCurrency = payScheduleCurrency;
                            tempData.isDefault = true;
                            if (rowData.type === 'R') {
                                tempData.TechSpecTimeId = Math.floor(Math.random() * 9999) - 10000;
                                tempData.chargeTotalUnit = tempData.payUnit = "0.00";
                                tempData.chargeRate = tempData.payRate = "0.0000";
                                this.props.actions.AddTechnicalSpecialistTime(tempData);
                            } else if (rowData.type === 'T') {
                                tempData.TechSpecTravelId = Math.floor(Math.random() * 9999) - 10000;
                                tempData.chargeTotalUnit = tempData.payUnit = "0.00";
                                tempData.chargeRate = tempData.payRate = "0.0000";
                                tempData.payExpenseType = rowData.chargeType;
                                this.props.actions.AddTechnicalSpecialistTravel(tempData);
                            } else if (rowData.type === 'E') {
                                tempData.TechSpecExpenseId = Math.floor(Math.random() * 9999) - 10000;
                                tempData.chargeUnit = tempData.payUnit = tempData.payRateTax = "0.00";
                                tempData.chargeRate = tempData.payRate = "0.0000";
                                tempData.currency = incurredCurrency;                                
                                if(rowData.currency) tempData.chargeRateExchange = tempData.payRateExchange = "1.000000";
                                if(incurredCurrency === rowData.currency) {
                                    tempData.chargeRateExchange = "1.000000";
                                } else {
                                    this.props.visitExchangeRates && this.props.visitExchangeRates.map(rates => {
                                        if (rates.currencyFrom === incurredCurrency && rates.currencyTo === rowData.currency) {
                                            tempData.chargeRateExchange = rates.rate;
                                        };
                                    }); 
                                }
                                if(incurredCurrency === payScheduleCurrency) {
                                    tempData.payRateExchange = "1.000000";
                                } else {
                                    this.props.visitExchangeRates && this.props.visitExchangeRates.map(rates => {
                                        if (rates.currencyFrom === incurredCurrency && rates.currencyTo === payScheduleCurrency) {
                                            tempData.payRateExchange = rates.rate;
                                        };
                                    });
                                }
                                this.props.actions.AddTechnicalSpecialistExpense(tempData);
                            } else if (rowData.type === 'C' || rowData.type === 'Q') {
                                tempData.TechSpecConsumableId = Math.floor(Math.random() * 9999) - 10000;
                                tempData.chargeTotalUnit = tempData.payUnit = "0.00";
                                tempData.chargeRate = tempData.payRate = "0.0000";
                                tempData.payExpenseType = rowData.chargeType;
                                tempData.chargeDescription = rowData.description;
                                this.props.actions.AddTechnicalSpecialistConsumable(tempData);
                            }

                            const lineItem = {};
                            lineItem["type"] = rowData.type;
                            lineItem["chargeType"] = rowData.chargeType;
                            lineItem["pin"] = pin;
                            lineItemsAdded.push(lineItem);
                        }
                    });
                }
            });
        }
        
        if (!isEmptyOrUndefine(payRateGridData.paySchedules)) {
            payRateGridData.paySchedules.forEach(row => {
                if (row.epin === pin && row.technicalSpecialistCompanyCode === this.props.selectedCompanyCode) {
                    row.payScheduleRates.forEach(rowData => {
                        const isTypeAdded = this.isLineItemAlreadyAdded(rowData.type, rowData.expenseType, pin, lineItemsAdded);
                        if ((isEmptyOrUndefine(rowData.effectiveTo) || new Date(rowData.effectiveTo) >= new Date(visitStartDate))
                            && !isTypeAdded && rowData.isActive) {
                            const tempData = {};
                            if (this.props.currentPage === localConstant.visit.EDIT_VIEW_VISIT_MODE) {
                                tempData.visitId = this.props.visitInfo.visitId;
                            }
                            tempData.expenseDate = (startDate ? startDate : visitStartDate);
                            tempData.chargeExpenseType = rowData.expenseType;
                            tempData.pin = pin;
                            tempData.visitTechnicalSpecialistId = null;
                            tempData.recordStatus = 'N';
                            tempData.TechSpecTimeId = Math.floor(Math.random() * 9999) - 10000;
                            tempData.assignmentId = assignmentId;
                            tempData.contractNumber = this.props.visitInfo.visitContractNumber;
                            tempData.projectNumber = this.props.visitInfo.visitProjectNumber;
                            tempData.payRateCurrency = rowData.currency;
                            tempData.chargeRateCurrency = chargeScheduleCurrency;
                            tempData.isDefault = true;
                            if (rowData.type === 'R') {
                                tempData.TechSpecTimeId = Math.floor(Math.random() * 9999) - 10000;
                                tempData.chargeTotalUnit = tempData.payUnit = "0.00";
                                tempData.chargeRate = tempData.payRate = "0.0000";
                                this.props.actions.AddTechnicalSpecialistTime(tempData);
                            } else if (rowData.type === 'T') {
                                tempData.TechSpecTravelId = Math.floor(Math.random() * 9999) - 10000;
                                tempData.payExpenseType = rowData.expenseType;
                                tempData.chargeTotalUnit = tempData.payUnit = "0.00";
                                tempData.chargeRate = tempData.payRate = "0.0000";
                                this.props.actions.AddTechnicalSpecialistTravel(tempData);
                            } else if (rowData.type === 'E') {
                                tempData.TechSpecExpenseId = Math.floor(Math.random() * 9999) - 10000;
                                tempData.chargeUnit = tempData.payUnit = tempData.payRateTax = "0.00";
                                tempData.chargeRate = tempData.payRate = "0.0000";
                                tempData.currency = incurredCurrency;
                                if(rowData.currency) tempData.chargeRateExchange = tempData.payRateExchange = "1.000000";
                                if(incurredCurrency === rowData.currency) {
                                    tempData.payRateExchange = "1.000000";
                                } else {
                                    this.props.visitExchangeRates && this.props.visitExchangeRates.map(rates => {
                                        if (rates.currencyFrom === incurredCurrency && rates.currencyTo === rowData.currency) {
                                            tempData.payRateExchange = rates.rate;
                                        };
                                    }); 
                                }
                                if(incurredCurrency === chargeScheduleCurrency) {
                                    tempData.chargeRateExchange = "1.000000";
                                } else {
                                    this.props.visitExchangeRates && this.props.visitExchangeRates.map(rates => {
                                        if (rates.currencyFrom === incurredCurrency && rates.currencyTo === chargeScheduleCurrency) {
                                            tempData.chargeRateExchange = rates.rate;
                                        };
                                    });
                                }
                                this.props.actions.AddTechnicalSpecialistExpense(tempData);
                            } else if (rowData.type === 'C' || rowData.type === 'Q') {
                                tempData.TechSpecConsumableId = Math.floor(Math.random() * 9999) - 10000;
                                tempData.payExpenseType = rowData.expenseType;
                                tempData.chargeTotalUnit = tempData.payUnit = "0.00";
                                tempData.chargeRate = tempData.payRate = "0.0000";
                                tempData.payRateDescription = rowData.description;
                                this.props.actions.AddTechnicalSpecialistConsumable(tempData);
                            }

                            const lineItem = {};
                            lineItem["type"] = rowData.type;
                            lineItem["chargeType"] = rowData.expenseType;
                            lineItem["pin"] = pin;
                            lineItemsAdded.push(lineItem);
                        }
                    });
                }
            });
        }
    }

    isLineItemAlreadyAdded(type, chargeType, pin, lineItemsAdded) {
        let isTypeAdded = false;
        if (lineItemsAdded) {
            lineItemsAdded.forEach(rowData => {
                if (rowData.type === type && rowData.chargeType === chargeType && rowData.pin === pin) {
                    isTypeAdded = true;
                }
            });
        }
        return isTypeAdded;
    }

    //All Input Handle get Name and Value
    inputHandleChange = async (e) => {
        const inputvalue = formInputChangeHandler(e);
        if (inputvalue.name === "visitStatus") {
            this.updatedData[inputvalue.name] = inputvalue.value;
            const isTBAVisitStatus = (inputvalue.value === 'U');
            this.props.actions.isTBAVisitStatus(isTBAVisitStatus);
            this.setState((state) => {
                return {
                    isTBAVisitStatus: isTBAVisitStatus,
                };
            });
            if(inputvalue.value ==='D'){
                this.props.actions.FetchUnusedReason();
            }
            else{
                this.updatedData["unusedReason"] = "";
            }            
        }
        else if (inputvalue.name === "unusedReason" ) {
             this.updatedData[inputvalue.name] = inputvalue.value;          
        } 
        else if(inputvalue.name === "supplierId") {            
            if(this.props.hasLineItems && !(this.props.visitInfo.visitStatus === localConstant.visit.VISIT_STATUS_LIST.TENTATIVE_PENDING_APPROVAL || 
                this.props.visitInfo.visitStatus === localConstant.visit.VISIT_STATUS_LIST.TBA_DATE_UNKNOWN || 
                this.props.visitInfo.visitStatus === localConstant.visit.VISIT_STATUS_LIST.CONFIRMED_AWAITING_VISIT)) {
                    const supplierId = this.props.visitInfo.supplierId;
                    document.getElementById('supplierId').value = supplierId;
                    this.props.actions.DisplayModal(this.confirmationSupplierChange);
            } else {
                this.updatedData[inputvalue.name] = inputvalue.value;                
                this.techSpecChange([]);
            }
        } else {
            this.updatedData[inputvalue.name] = inputvalue.value;
            if(inputvalue.name === "isFinalVisit" && inputvalue.value === false) {                
                const res = await this.props.actions.FetchFinalVisitId(this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE);                
                if(res && res === this.props.visitInfo.visitId) {
                    this.props.actions.DisplayModal(this.confirmationNoFinalVisit);
                }
            }            
        }
        this.props.actions.AddUpdateGeneralDetails(this.updatedData);
        this.updatedData = {};        
    }

    selectedRowHandler = () => {
        const redirectionURL = AppMainRoutes.supplierPoDetails;
        const supplierPOId = this.props.visitInfo.visitSupplierPOId;
        const queryObj={
            supplierPOId:supplierPOId && supplierPOId,
            selectedCompany:this.props.selectedCompanyCode
        };
        const queryStr = ObjectIntoQuerySting(queryObj);
        window.open(redirectionURL + '?'+queryStr,'_blank');
    }

    visitStartDateChange = (date) => {

         //To check this Visit already has calendar data
        //Start
        // let isVisitHasCalendarData = false;
        // if ((Array.isArray(this.state.calendarData.events) && this.state.calendarData.events.length > 0)) {
        //     const calendarValidateData = this.state.calendarData.events;
        //     for (let index = 0; index < calendarValidateData.length; index++) {
        //         if ((!calendarValidateData[index].calendarRefCode || calendarValidateData[index].calendarRefCode === 0 || this.props.visitInfo.visitId) && calendarValidateData[index].recordStatus !== 'D') {
        //             isVisitHasCalendarData = true;
        //             break;
        //         }
        //     }
        //     if (isVisitHasCalendarData)
        //         this.props.actions.DisplayModal(this.confirmationObject);
        // }
        //end

        this.setState({
            visitStartDate: date,
            visitCalendarStartDate: date
        }, () => {
            this.updatedData["visitStartDate"] = this.state.visitStartDate !== null ? this.state.visitStartDate.format() : '';
            this.props.actions.AddUpdateGeneralDetails(this.updatedData);
            this.editVisitCalendarDataBind();
            if (this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE) {
                this.updateDefaultLineItems(this.updatedData["visitStartDate"]);
            }
        });
        if (isEmptyOrUndefine(this.state.visitEndDate) && isEmptyOrUndefine(this.props.visitInfo.visitStartDate)) {
            this.setState({
                visitEndDate: date
            }, () => {
                this.updatedData["visitEndDate"] = this.state.visitStartDate !== null ? this.state.visitStartDate.format() : '';
                this.props.actions.AddUpdateGeneralDetails(this.updatedData);
            });
        }
    }

    visitEndDateChange = (date) => {

         //To check this Visit already has calendar data
        //Start
        // let isVisitHasCalendarData = false;
        // if ((Array.isArray(this.state.calendarData.events) && this.state.calendarData.events.length > 0)) {
        //     const calendarValidateData = this.state.calendarData.events;
        //     for (let index = 0; index < calendarValidateData.length; index++) {
        //         if ((!calendarValidateData[index].calendarRefCode || calendarValidateData[index].calendarRefCode === 0 || this.props.visitInfo.visitId) && calendarValidateData[index].recordStatus !== 'D') {
        //             isVisitHasCalendarData = true;
        //             break;
        //         }
        //     }
        //     if (isVisitHasCalendarData)
        //         this.props.actions.DisplayModal(this.confirmationObject);
        // }
        //end

        this.setState({
            visitEndDate: date,
        }, () => {
            this.updatedData["visitEndDate"] = this.state.visitEndDate !== null ? this.state.visitEndDate.format() : '';
            this.props.actions.AddUpdateGeneralDetails(this.updatedData);
            this.editVisitCalendarDataBind();
        });
    }

    visitCustomerDateChange = (date) => {
        this.setState({
            visitReportSentToCustomerDate: date
        }, () => {
            this.updatedData["visitReportSentToCustomerDate"] = this.state.visitReportSentToCustomerDate !== null ? this.state.visitReportSentToCustomerDate.format() : '';
            this.props.actions.AddUpdateGeneralDetails(this.updatedData);
        });
    }

    visitExpectedCompleteDateChange = (date) => {
        this.setState({
            visitExpectedCompleteDate: date
        }, () => {
            this.updatedData["visitExpectedCompleteDate"] = this.state.visitExpectedCompleteDate !== null ? this.state.visitExpectedCompleteDate.format() : '';
            this.props.actions.AddUpdateGeneralDetails(this.updatedData);
        });
    }

    handleDateChangeRaw = (e, type) => {
        if(e && e.target && e.target.value !== '') {
            if (!(e && !e._isAMomentObject && e.target && e.target.value === ''))
                return false;
            if (e) {
                const date = moment(e._isAMomentObject ? e : e.target.value.split("-").reverse().join("-"));
                if (date.isValid()) {
                    type && this[type](date);
                }
                else {
                    IntertekToaster(localConstant.commonConstants.INVALID_DATE_FORMAT, 'warningToast PercentRange');
                    return false;
                }
            }
        }
    }

    precentageCompleteChange = (e) => {
        const value = e.target.value;
        if (value >= 0 && value <= 100) {
            this.updatedData[e.target.name] = value;
            this.props.actions.AddUpdateGeneralDetails(this.updatedData);
        } else {
            IntertekToaster("Percentage Completed should be with in  0 to 100 range", 'warningToast PercentRange');
            if (value < 0) e.target.value = this.props.visitInfo.visitCompletedPercentage;
            if (value > 100) e.target.value = this.props.visitInfo.visitCompletedPercentage;
        }
    }

    componentDidMount() {
        if (this.props.currentPage === localConstant.visit.EDIT_VIEW_VISIT_MODE) {
            if (!isEmptyOrUndefine(this.props.visitInfo) && !isEmptyOrUndefine(this.props.visitInfo.visitStatus)) {
                const isTBAVisitStatus = (this.props.visitInfo.visitStatus === 'U');
                this.props.actions.isTBAVisitStatus(isTBAVisitStatus);
                this.isVisitStatusControlDisabled();
            }
            if (this.props.visitTechnicalSpecialists)
                this.editVisitCalendarDataBind();            
        } else {
            //Get Tech Id List 
            const techIds = [];
            if (this.props.visitTechnicalSpecialists) {
                if (Array.isArray(this.props.visitTechnicalSpecialists) && this.props.visitTechnicalSpecialists.length > 0) {
                    this.props.visitTechnicalSpecialists.map(tech => {
                        techIds.push(tech.pin);
                    });
                }
            }
            if (techIds.length > 0) {
                // Fetch visit calendar data
                this.props.actions.FetchVisitTimesheetCalendarData({
                    companyCode: this.props.selectedCompanyCode,
                    isActive: true,
                    startDateTime: dateUtil.getWeekStartDate(this.props.visitInfo && this.props.visitInfo.visitStartDate ? new Date(this.props.visitInfo.visitStartDate) : new Date()),
                    endDateTime: dateUtil.getWeekEndDate(this.props.visitInfo && this.props.visitInfo.visitEndDate ? new Date(this.props.visitInfo.visitEndDate) : new Date()),
                    // calendarType: localConstant.globalCalendar.CALENDAR_STATUS.VISIT,
                    // calendarRefCode: this.props.visitId ? this.props.visitId : 0
                }, techIds).then(response => {
                    if (response) {
                        this.calendarResourceList = [];
                        if (this.props.visitTechnicalSpecialists) {
                            this.props.visitTechnicalSpecialists.map(techSpec => {
                                this.calendarResourceList.push({
                                    id: techSpec.pin,
                                    name: techSpec.technicalSpecialistName
                                });
                            });
                        }
                        if (this.props.visitCalendarData) {
                            this.props.visitCalendarData.map(calendar => {
                                if(calendar.recordStatus!=='D'){
                                calendar.title = calendar.calendarStatus;
                                // calendar.start = calendar.startDateTime;
                                // calendar.end = calendar.endDateTime;
                                calendar.resourceId = calendar.technicalSpecialistId;
                                calendar.bgColor = calendar.title === localConstant.globalCalendar.EVENT_STATUS.BOOKED ? localConstant.globalCalendar.EVENT_COLORS.BOOKED : (calendar.title === localConstant.globalCalendar.EVENT_STATUS.TENTATIVE) ? localConstant.globalCalendar.EVENT_COLORS.TENTATIVE : (calendar.title === localConstant.globalCalendar.EVENT_STATUS.TBA) ? localConstant.globalCalendar.EVENT_COLORS.TBA : localConstant.globalCalendar.EVENT_COLORS.PTO;
                                response.events.push(calendar);
                            }
                            });
                        }
                        response = {
                            resources: this.calendarResourceList,
                            events: response.events
                        };
                        this.setState({
                            calendarData: response,
                            isCalendarDataUpdated: true,
                            visitCalendarStartDate: this.props.visitInfo ? this.props.visitInfo.visitStartDate : new Date()
                        });
                    }
                    // else {
                    //     if (this.props.visitCalendarData) {
                    //         this.props.visitCalendarData.map(data => {
                    //             response.events.push(data);
                    //         });
                    //     }
                    //     this.setState({
                    //         calendarData: response,
                    //         isCalendarDataUpdated: true
                    //     });
                    // }
                });
            }
        }  
                  
    };

    isPageRefresh() {
        let isRefresh = true;
        visitTabDetails.forEach(row => {
            if (row["tabBody"] === "GeneralDetails") {
                isRefresh = row["isRefresh"];
                row["isRefresh"] = false;
                row["isCurrentTab"] = true;
            } else {
                row["isCurrentTab"] = false;
            }
        });
        return isRefresh;
    }

    filterVisitStatus() {
        const filteredVisitStatus = [];
        if (!isEmptyOrUndefine(this.props.visitStatus) && this.props.currentPage !== localConstant.visit.EDIT_VIEW_VISIT_MODE) {
            this.props.visitStatus.forEach(statusData => {
                if (statusData.code === 'C' || statusData.code === 'Q' || statusData.code === 'T'
                    || statusData.code === 'U')
                    filteredVisitStatus.push(statusData);
            });
            return filteredVisitStatus;
        } else {
            if (!isEmptyOrUndefine(this.props.visitInfo)) {
                const selectedVisitStatus = this.props.visitInfo.visitStatus;
                if (isEmpty(selectedVisitStatus) || selectedVisitStatus === 'C' || selectedVisitStatus === 'Q' || selectedVisitStatus === 'T'
                    || selectedVisitStatus === 'U' || selectedVisitStatus === 'D') {
                    this.props.visitStatus.forEach(statusData => {
                        if (statusData.code === 'C' || statusData.code === 'Q' || statusData.code === 'T'
                            || statusData.code === 'U' || statusData.code === 'D')
                            filteredVisitStatus.push(statusData);
                    });
                    return filteredVisitStatus;
                }
                return this.props.visitStatus;
            } else {
                return this.props.visitStatus;
            }
        }
    }

    isVisitStatusControlDisabled() {        
        if (!isEmptyOrUndefine(this.props.visitInfo) && !isEmptyOrUndefine(this.props.visitInfo.visitStatus)) {             
            if (this.props.visitInfo.visitStatus === 'C' || this.props.visitInfo.visitStatus === 'A' || this.props.visitInfo.visitStatus === 'J'
                || this.props.visitInfo.visitStatus === 'O' || this.props.visitInfo.visitStatus === 'R' || this.props.visitInfo.visitStatus === 'D'
                ) {                    
                    this.isVisitStatusDisabled = true;
            }            
        }        
    }

    isJobReferenceVisible() {
        if (this.props.currentPage === localConstant.visit.EDIT_VIEW_VISIT_MODE)
            return true;
        else
            return false;
    }

    getVisitSupplierList() {
        //get the old PO Supplier from Visitinfo (this check only for SKELTON Visit)//
        const visitSupplierData = {};
        const visitSupplierId=this.props.visitInfo.supplierId;
        visitSupplierData["supplierId"] = visitSupplierId;
        visitSupplierData["supplierName"] = isEmptyOrUndefine(this.props.visitInfo.supplierLocation) 
        ? this.props.visitInfo.visitSupplier : (this.props.visitInfo.visitSupplier + ', ' + this.props.visitInfo.supplierLocation);
        //get the old PO Supplier from Visitinfo (this check only for SKELTON Visit)//
        const visitSupplierList = [];        
        const subSupplierList = [];
        this.props.subSupplierList.forEach((iteratedValue) => {
            if (iteratedValue.supplierType === "M") {
                if(subSupplierList.indexOf(iteratedValue) === -1)
                subSupplierList.push(iteratedValue);
            }
        });
        this.props.visitSupplierList.forEach(item => { //Changes for Live D669
            this.props.subSupplierList.forEach((iteratedValue) => {
                if (iteratedValue.subSupplierId === item.supplierId && iteratedValue.supplierType !== "M") {
                    if(subSupplierList.indexOf(iteratedValue) === -1)
                    subSupplierList.push(iteratedValue);
                }
            });
        });
        if (!isEmptyOrUndefine(subSupplierList)) {
            subSupplierList.forEach(supplierData => {                
                const addSupplier = supplierData.isDeleted ? (supplierData.assignmentSubSupplierTS && supplierData.assignmentSubSupplierTS.filter(x => x.isDeleted === false).length > 0) : true;                
                if(addSupplier) {
                    const visitSupplier = {};                  
                    visitSupplier["supplierId"] =  (supplierData.supplierType === "M" ? supplierData.mainSupplierId : supplierData.subSupplierId);
                    visitSupplier["supplierName"] = ((supplierData.supplierType === "M" ? supplierData.mainSupplierName : supplierData.subSupplierName) + ', ' 
                                                        + (isEmptyOrUndefine(supplierData.city) ? '' : (supplierData.city + ', ')) 
                                                        + (isEmptyOrUndefine(supplierData.state) ? '' : (supplierData.state + ', '))
                                                        + (isEmptyOrUndefine(supplierData.zipCode) ? '' : (supplierData.zipCode + ', '))
                                                        + (isEmptyOrUndefine(supplierData.country) ? '' : supplierData.country));
                    visitSupplier["isMainSupplier"] = false;
                    visitSupplierList.push(visitSupplier);  
                }                                  
            });
        }  
        return visitSupplierList;
    }
    isFirstVisitSupplier(visitSupplierList) {
        let hasSupplierFirstVisit = false;
        if (!isEmptyOrUndefine(visitSupplierList) && !isEmptyOrUndefine(this.props.visitInfo)) {
            visitSupplierList.forEach(supplierData => {               
                if (this.props.visitInfo.skeltonVisit === 'Yes' && supplierData.isMainSupplier) {
                    hasSupplierFirstVisit = true;
                }
            });
        }
        return hasSupplierFirstVisit;
    }

    /*Calendar Functionalities - Start*/

    //To click calendar event
    eventItemClick = (schedulerData, eventItem) => {
        if (this.props.visitInfo) {
                if (!eventItem.calendarRefCode || eventItem.calendarRefCode === 0 || eventItem.calendarRefCode === this.props.visitInfo.visitId) {
                    // let userTypesArray = [];
                    // if (this.props.userTypes) {
                    //     userTypesArray = this.props.userTypes.split(',');
                    // }
                    // if (userTypesArray.includes(localConstant.userTypeList.Coordinator) || userTypesArray.includes(localConstant.userTypeList.MICoordinator)) {
                        eventItem.startTime = eventItem.start;
                        eventItem.endTime = eventItem.end;
                        eventItem.isNewRecord = false;
                        eventItem.recordStatus = (eventItem.recordStatus && eventItem.recordStatus === "N") ? "N" : "M";
                        this.setState({
                            calendarTaskData: eventItem,
                            isCalendarDataUpdated: false
                        });
                        this.endDateTimeLimit = moment(eventItem.startTime).hour();
                        this.endDateTimeMinLimit = moment(eventItem.startTime).minute();
                        this.calendarPopup = this.props.visitInfo;
                        this.setState({
                            isOpenCalendarPopUp: true,
                            isViewMode: (this.props.interactionMode || 
                                (this.props.isInterCompanyAssignment && this.props.isCoordinatorCompany) || 
                                ((!this.props.isInterCompanyAssignment && this.props.visitInfo.visitStatus === 'A') ? false : 
                                !(this.props.visitInfo.visitStatus !== 'A' && this.props.visitInfo.visitStatus !== 'O'))) ? true : false,
                            isEditTaskMode: true,
                            templateType: localConstant.globalCalendar.CALENDAR_STATUS.VISIT,
                            isCalendarDataUpdated: false
                        });
                    // }
                    // else {
                    //     if (eventItem.logInName === this.props.userLogonName) {
                    //         eventItem.startTime = eventItem.start;
                    //         eventItem.endTime = eventItem.end;
                    //         eventItem.recordStatus = (eventItem.recordStatus && eventItem.recordStatus === "N") ? "N" : "M";
                    //         this.setState({
                    //             calendarTaskData: eventItem,
                    //             isCalendarDataUpdated: false
                    //         });
                    //         this.endDateTimeLimit = moment(eventItem.startTime).hour();
                    //         this.endDateTimeMinLimit = moment(eventItem.startTime).minute();
                    //         this.calendarPopup = this.props.visitInfo;
                    //         this.setState({
                    //             isOpenCalendarPopUp: true,
                    //             isEditTaskMode: true,
                    //             isViewMode: (this.props.interactionMode || (this.props.isInterCompanyAssignment&&this.props.isCoordinatorCompany)||!(this.props.visitInfo.visitStatus !== 'A'&&this.props.visitInfo.visitStatus !== 'O'))?true:false,
                    //             templateType: localConstant.globalCalendar.CALENDAR_STATUS.VISIT,
                    //             isCalendarDataUpdated: false
                    //         });
                    //     }
                    // }
                }
                else {
                    eventItem.startTime = eventItem.start;
                    eventItem.endTime = eventItem.end;
                    this.setState({
                        calendarTaskData: eventItem,
                        isCalendarDataUpdated: false
                    });
                    if (eventItem.calendarType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) {
                        this.props.actions.FetchVisitCalendarByID(eventItem.calendarRefCode).then(response => {
                            if (response && response !== null) {
                                this.calendarPopup = response;
                                this.setState({
                                    isOpenCalendarPopUp: true,
                                    isViewMode: true,
                                    isEditTaskMode: false,
                                    templateType: localConstant.globalCalendar.CALENDAR_STATUS.VISIT,
                                    isCalendarDataUpdated: false
                                });
                            }
                        });
                    }
                    else if (eventItem.calendarType === localConstant.globalCalendar.CALENDAR_STATUS.TIMESHEET) {
                        this.props.actions.FetchTimesheetGeneralDetail(eventItem.calendarRefCode).then(response => {
                            if (response && response !== null) {
                                this.calendarPopup = response;
                                this.setState({
                                    isOpenCalendarPopUp: true,
                                    isViewMode: true,
                                    isEditTaskMode: false,
                                    templateType: localConstant.globalCalendar.CALENDAR_STATUS.TIMESHEET,
                                    isCalendarDataUpdated: false
                                });
                            }
                        });
                    }
                    else if (eventItem.calendarType === localConstant.globalCalendar.CALENDAR_STATUS.PRE) {
                        this.props.actions.FetchPreAssignment(eventItem.calendarRefCode).then(response => {
                            if (response && response !== null) {
                                this.calendarPopup = response;
                                this.setState({
                                    isOpenCalendarPopUp: true,
                                    isViewMode: true,
                                    isEditTaskMode: false,
                                    templateType: localConstant.globalCalendar.CALENDAR_STATUS.PRE,
                                    isCalendarDataUpdated: false
                                });
                            }
                        });
                    }
                    else {
                        this.props.actions.FetchTimeOffRequestData(eventItem.calendarRefCode).then(response => {
                            if (response && response !== null) {
                                if (Array.isArray(response.result)) {
                                    this.calendarPopup = response.result[0];
                                    this.setState({
                                        isOpenCalendarPopUp: true,
                                        isViewMode: true,
                                        isEditTaskMode: false,
                                        templateType: localConstant.globalCalendar.CALENDAR_STATUS.PTO,
                                        isCalendarDataUpdated: false
                                    });
                                }
                            }
                        });
                    }
                }
        }
    };

    //To add new event in calendar
    calendarNewEvent = (schedulerData, slotId, slotName, start, end, type, item) => {
        if (this.props.visitInfo&&!(this.props.interactionMode || (this.props.isInterCompanyAssignment&&this.props.isCoordinatorCompany))) {
            if (!this.props.isInterCompanyAssignment || (this.props.visitInfo.visitStatus !== 'A' && this.props.visitInfo.visitStatus !== 'O')) {
                if (this.props.visitInfo.visitStartDate && this.props.visitInfo.visitEndDate) {

                    const startDate = moment(this.props.visitInfo.visitStartDate).format("DD-MM-YYYY");
                    const endDate = moment(this.props.visitInfo.visitEndDate).format("DD-MM-YYYY");
                    const eventStartDate = moment(start).format("DD-MM-YYYY");
                    const eventEndDate = moment(end).format("DD-MM-YYYY");

                    const isValidStartBetween = moment(start).isBetween(this.props.visitInfo.visitStartDate, this.props.visitInfo.visitEndDate);
                    const isValidEndBetween = moment(end).isBetween(this.props.visitInfo.visitStartDate, this.props.visitInfo.visitEndDate);
                    const isValidStart = (eventStartDate == startDate || eventStartDate == endDate);
                    const isValidEnd = (eventEndDate == endDate || eventEndDate == startDate);

                    if ((isValidStartBetween && isValidEndBetween) || (isValidStart && isValidEnd) || (isValidStartBetween && isValidEnd) || (isValidEndBetween && isValidStart)) {
                        this.setState({
                            calendarTaskData: {
                                ...this.state.calendarTaskData,
                                id: 0,
                                start: start,
                                end: end,
                                startTime: moment(start).set({ hour: 0, minute: 0, second: 0, millisecond: 0 }).add(9, 'hours'),
                                endTime: moment(end).set({ hour: 0, minute: 0, second: 0, millisecond: 0 }).add(17, 'hours'),
                                resourceId: slotId,
                                recordStatus: "N",
                                calendarRefCode: 0,
                                companyCode: this.props.selectedCompanyCode,
                                isNewRecord: true
                            },
                            isCalendarDataUpdated: false
                        });
                        this.calendarPopup = this.props.visitInfo;
                        this.setState({
                            isOpenCalendarPopUp: true,
                            isEditTaskMode: false,
                            templateType: localConstant.globalCalendar.CALENDAR_STATUS.VISIT,
                            isCalendarDataUpdated: false
                        });
                    }
                    else
                        IntertekToaster(localConstant.commonConstants.INVALID_CALENDAR_DATE_RANGE, 'warningToast VisitCalendar');
                }
            }
        }
    };

    //Calendar Control click api call
    calendarHandlerToBindData = (schedularData, isViewChange) => {
        const startDate = schedularData.startDate;

        //Get Tech Id List 
        const techIds = [];
        if (this.props.visitTechnicalSpecialists) {
            if (Array.isArray(this.props.visitTechnicalSpecialists) && this.props.visitTechnicalSpecialists.length > 0) {
                this.props.visitTechnicalSpecialists.map(tech => {
                    techIds.push(tech.pin);
                });
            }
        }

        if (techIds.length > 0) {
            this.props.actions.FetchVisitTimesheetCalendarData({
                companyCode: this.props.selectedCompanyCode,
                isActive: true,
                startDateTime: schedularData.startDate,
                endDateTime: schedularData.endDate,
                // calendarType: localConstant.globalCalendar.CALENDAR_STATUS.VISIT,
                // calendarRefCode: this.props.visitInfo.visitId ? this.props.visitInfo.visitId : 0
            }, techIds).then(response => {
                response.resources = this.calendarResourceList;
                // const eventsData = [];
                if (this.props.visitCalendarData) {
                    this.props.visitCalendarData.map(data => {
                        if (response.events && response.events.length > 0) {
                            const index = response.events.findIndex(x => x.id === data.id);
                            if (index >= 0)
                                response.events.splice(index, 1);
                        }
                        if(data.recordStatus!=='D'){
                        data.title = data.calendarStatus;
                        data.resourceId = data.technicalSpecialistId;
                        data.bgColor = data.title === localConstant.globalCalendar.EVENT_STATUS.BOOKED ? localConstant.globalCalendar.EVENT_COLORS.BOOKED : (data.title === localConstant.globalCalendar.EVENT_STATUS.TENTATIVE) ? localConstant.globalCalendar.EVENT_COLORS.TENTATIVE : (data.title === localConstant.globalCalendar.EVENT_STATUS.TBA) ? localConstant.globalCalendar.EVENT_COLORS.TBA : localConstant.globalCalendar.EVENT_COLORS.PTO;
                        response.events.push(data);
                    }
                    });
                }
                // response.events = eventsData;
                //Setting up data to bind in scheduler
                this.setState({
                    calendarData: response,
                    isCalendarDataUpdated: true,
                    visitCalendarStartDate: startDate
                });
            });
        }
        else {
            this.setState({
                calendarData: this.defaultCalendarData,
                isCalendarDataUpdated: true,
                visitCalendarStartDate: this.props.visitInfo ? this.props.visitInfo.visitStartDate : new Date()
            });
        }
    }

    //To popover task in calendar
    // eventItemPopoverTemplateResolver = (schedulerData, eventItem, title, start, end, statusColor) => {
    //     if (eventItem.calendarType != localConstant.globalCalendar.CALENDAR_STATUS.PTO) {
    //         return (
    //             <div>
    //                 <a className={"waves-effect waves-light pt-1 pr-1 pl-1 pb-1 bold mr-2"} onClick={() => this.calendarEditView(eventItem)}><i className="zmdi zmdi-edit left cal_icon"></i> Edit</a>
    //                 <a className={"waves-effect waves-light pt-1 pr-1 pl-1 pb-1 bold ml-2 "} onClick={() => this.calendarView(eventItem)}><i className="zmdi zmdi-eye left cal_icon"></i> View</a>
    //             </div>
    //         );
    //     }
    //     return (
    //         <div>
    //             <p>{eventItem.title}</p>
    //         </div>
    //     );
    // }

    //Calendar Next Click
    calendarNextClick = (schedularData) => {
        this.calendarHandlerToBindData(schedularData, false);
    }

    //Calendar Previous Click
    calendarPrevClick = (schedularData) => {
        this.calendarHandlerToBindData(schedularData, false);
    }

    //Calendar View change
    calendarOnViewChange = (schedularData) => {
        this.calendarHandlerToBindData(schedularData, true);
    }

    //Calendar select Date
    calendarOnSelectDate = (schedularData) => {
        this.calendarHandlerToBindData(schedularData, false);
    }

    //Calendar view button click
    calendarView = (eventItem) => {

        this.calendarPopup = this.props.visitInfo;
        this.setState({
            isOpenCalendarPopUp: true,
            isEditTaskMode: false,
            templateType: localConstant.globalCalendar.CALENDAR_STATUS.VISIT,
            isCalendarDataUpdated: false
        });
    }

    //Calendar edit button click
    calendarEditView = (eventItem) => {
        eventItem.startTime = eventItem.start;
        eventItem.endTime = eventItem.end;
        eventItem.recordStatus = "M";
        this.setState({
            calendarTaskData: eventItem,
            isCalendarDataUpdated: false
        });
        this.calendarPopup = this.props.visitInfo;
        this.setState({
            isOpenCalendarPopUp: true,
            isEditTaskMode: true,
            templateType: localConstant.globalCalendar.CALENDAR_STATUS.VISIT,
            isCalendarDataUpdated: false
        });
    }

    //To close calendar popup
    cancelCalendarPopup = (e) => {
        e.preventDefault();
        this.setState({
            isOpenCalendarPopUp: false,
            calendarTaskData: {},
            isCalendarDataUpdated: false,
            isViewMode: false
        });
    };

    //Task start date change
    calendarTaskStartDateChange = (date) => {
        this.setState({
            calendarTaskData: {
                ...this.state.calendarTaskData,
                start: date
            },
            isCalendarDataUpdated: false
        });
    }

    //Task end date change
    calendarTaskEndDateChange = (date) => {
        this.setState({
            calendarTaskData: {
                ...this.state.calendarTaskData,
                end: date
            },
            isCalendarDataUpdated: false
        });
    }

    //Task start time change
    calendarTaskStartTimeChange = (date) => {
        // const time = moment(date).format('h');
        this.endDateTimeLimit = moment(date).hour();
        this.endDateTimeMinLimit = moment(date).minute();
        this.setState({
            calendarTaskData: {
                ...this.state.calendarTaskData,
                startTime: date,
                endTime: ""
            },
            isCalendarDataUpdated: false
        });
    }

    //Task end time change
    calendarTaskEndTimeChange = (date) => {
        const time = moment(date).format('h:mm a');
        this.setState({
            calendarTaskData: {
                ...this.state.calendarTaskData,
                endTime: date
            },
            isCalendarDataUpdated: false
        });
    }

    //Calendar popup input change
    calendarInputChangeHandler = (e) => {
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        if (inputvalue.name === "calendarStatus") {
            this.setState({
                calendarTaskData: {
                    ...this.state.calendarTaskData,
                    calendarStatus: inputvalue.value === localConstant.globalCalendar.EVENT_STATUS.CONFIRMED ? localConstant.globalCalendar.EVENT_STATUS.BOOKED : inputvalue.value
                },
                isCalendarDataUpdated: false
            });
        }
        else if (inputvalue.name === "description") {
            this.setState({
                calendarTaskData: {
                    ...this.state.calendarTaskData,
                    description: inputvalue.value
                },
                isCalendarDataUpdated: false
            });
        }
        else if (inputvalue.name === "resourceAllocation") {
            this.setState({
                calendarTaskData: {
                    ...this.state.calendarTaskData,
                    resourceAllocation: inputvalue.value
                },
                isCalendarDataUpdated: false
            });
        }
        else if (inputvalue.name === "foreNoon") {
            this.setState({
                calendarTaskData: {
                    ...this.state.calendarTaskData,
                    foreNoon: inputvalue.value
                },
                isCalendarDataUpdated: false
            });
        }
        else if (inputvalue.name === "afterNoon") {
            this.setState({
                calendarTaskData: {
                    ...this.state.calendarTaskData,
                    afterNoon: inputvalue.value
                },
                isCalendarDataUpdated: false
            });
        }
    }
    checkMandatoryCalendarData = () => {
        if (this.state.calendarTaskData) {
            if (!this.state.calendarTaskData.startTime || this.state.calendarTaskData.startTime === "") {
                IntertekToaster("Enter Start Time", 'warningToast VisitCalendar');
                return false;
            }
            else if (!this.state.calendarTaskData.endTime || this.state.calendarTaskData.endTime === "") {
                IntertekToaster("Enter End Time", 'warningToast VisitCalendar');
                return false;
            }
            else if (!this.state.calendarTaskData.start || this.state.calendarTaskData.start === "") {
                IntertekToaster("Enter Start Date", 'warningToast VisitCalendar');
                return false;
            }
            else if (!this.state.calendarTaskData.end || this.state.calendarTaskData.end === "") {
                IntertekToaster("Enter End Date", 'warningToast VisitCalendar');
                return false;
            }
            else if (!this.state.calendarTaskData.calendarStatus || this.state.calendarTaskData.calendarStatus === "") {
                IntertekToaster("Enter Resource Status", 'warningToast VisitCalendar');
                return false;
            }
            else {
                if (this.state.calendarTaskData.start && this.state.calendarTaskData.end) {
                    const eventStartDate = moment(this.state.calendarTaskData.start).format(localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT);
                    const eventEndDate = moment(this.state.calendarTaskData.end).format(localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT);
                    const isValidEventDate = moment(eventStartDate,localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT).isSame(moment(eventEndDate,localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT)) || moment(eventEndDate,localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT).isAfter(moment(eventStartDate,localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT));
                    if (isValidEventDate) {
                        const startDate = moment(this.props.visitInfo.visitStartDate).format(localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT);
                        const endDate = moment(this.props.visitInfo.visitEndDate).format(localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT);

                        const isValidStartBetween = moment(eventStartDate,localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT).isBetween(moment(startDate,localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT), moment(endDate,localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT));
                        const isValidEndBetween = moment(eventEndDate,localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT).isBetween(moment(startDate,localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT), moment(endDate,localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT));
                        const isValidStart = (eventStartDate == startDate || eventStartDate == endDate);
                        const isValidEnd = (eventEndDate == endDate || eventEndDate == startDate);

                        if ((isValidStartBetween && isValidEndBetween) || (isValidStart && isValidEnd) || (isValidStartBetween&&isValidEnd) || (isValidEndBetween&&isValidStart) || this.state.calendarTaskData.resourceAllocation) {
                            return true;
                        }
                        else {
                            IntertekToaster(localConstant.commonConstants.INVALID_CALENDAR_DATE_RANGE, 'warningToast VisitCalendar');
                            return false;
                        }
                    }
                    else{
                        IntertekToaster(localConstant.commonConstants.VISIT_INVALID_CALENDAR_START_DATE_RANGE, 'warningToast VisitCalendar');
                        return false;
                    }
                }
                else
                    return false;
            }
        }
    }
    //To save calendar popup
    saveCalendarPopup = (e) => {
        e.preventDefault();
        const isValidData = this.checkMandatoryCalendarData();
        if (isValidData) {
            let recordStatus=this.state.calendarTaskData.recordStatus;
            const startTime = moment(this.state.calendarTaskData.startTime).format(localConstant.techSpec.common.CUSTOMINPUT_DATE_TIME_FORMAT);
            const endTime = moment(this.state.calendarTaskData.endTime).format(localConstant.techSpec.common.CUSTOMINPUT_DATE_TIME_FORMAT);
            const startDate = moment(this.state.calendarTaskData.start).format(localConstant.commonConstants.UI_DATE_FORMAT);
            const endDate = moment(this.state.calendarTaskData.end).format(localConstant.commonConstants.UI_DATE_FORMAT);
            if (this.state.calendarTaskData.resourceAllocation) {
                recordStatus="D";
            }
            let calendarDataList = this.state.calendarData.events;

            const index = this.props.visitCalendarData.findIndex(x => x.id === this.state.calendarTaskData.id);

            if ((index < 0)) {
                const calendarDataToSave = this.state.calendarTaskData;
                if (this.state.calendarTaskData.recordStatus === "N")
                    calendarDataToSave.id = (this.props.visitCalendarData) ? -(this.props.visitCalendarData.length + 1) : -1;
                calendarDataToSave.start = moment(startDate + " " + startTime, localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT);
                calendarDataToSave.end = moment(endDate + " " + endTime, localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT);
                calendarDataToSave.recordStatus=recordStatus;
                this.props.actions.addVisitCalendarData(calendarDataToSave, moment.utc(startDate + " " + startTime, localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT), moment.utc(endDate + " " + endTime, localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT));
                if (this.state.calendarTaskData.recordStatus === "M" || this.state.calendarTaskData.recordStatus === "D") {
                    const index = calendarDataList.findIndex(x => x.id === this.state.calendarTaskData.id);
                    calendarDataList.splice(index, 1);
                }
                if(recordStatus!=='D'){
                const newCalendarEvent = this.state.calendarTaskData;
                newCalendarEvent.title = this.state.calendarTaskData.calendarStatus;
                newCalendarEvent.bgColor = newCalendarEvent.title === localConstant.globalCalendar.EVENT_STATUS.BOOKED ? localConstant.globalCalendar.EVENT_COLORS.BOOKED : (newCalendarEvent.title === localConstant.globalCalendar.EVENT_STATUS.TENTATIVE) ? localConstant.globalCalendar.EVENT_COLORS.TENTATIVE : (newCalendarEvent.title === localConstant.globalCalendar.EVENT_STATUS.TBA) ? localConstant.globalCalendar.EVENT_COLORS.TBA : localConstant.globalCalendar.EVENT_COLORS.PTO;
                // newCalendarEvent.start = moment(startDate + " " + startTime, localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT);
                // newCalendarEvent.end = moment(endDate + " " + endTime, localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT);
                calendarDataList.push(newCalendarEvent);
            }
            }
            else {
                let updateCalendarData = [];
                updateCalendarData = this.props.visitCalendarData;
                updateCalendarData.map(calendar => {
                    if (calendar.id === this.state.calendarTaskData.id) {
                        calendar.calendarStatus = this.state.calendarTaskData.calendarStatus;
                        calendar.title = this.state.calendarTaskData.calendarStatus;
                        calendar.bgColor = calendar.title === localConstant.globalCalendar.EVENT_STATUS.BOOKED ? localConstant.globalCalendar.EVENT_COLORS.BOOKED : (calendar.title === localConstant.globalCalendar.EVENT_STATUS.TENTATIVE) ? localConstant.globalCalendar.EVENT_COLORS.TENTATIVE : (calendar.title === localConstant.globalCalendar.EVENT_STATUS.TBA) ? localConstant.globalCalendar.EVENT_COLORS.TBA : localConstant.globalCalendar.EVENT_COLORS.PTO;
                        calendar.start = moment(startDate + " " + startTime, localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT);
                        calendar.end = moment(endDate + " " + endTime, localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT);
                        calendar.startDateTime = moment.utc(startDate + " " + startTime, localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT);
                        calendar.endDateTime = moment.utc(endDate + " " + endTime, localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT);
                        calendar.recordStatus = recordStatus;
                    }
                });
                this.props.actions.updateVisitCalendarData(updateCalendarData);
                // const index = calendarDataList.indexOf({ id:this.state.calendarTaskData.id });
                const index = calendarDataList.findIndex(x => x.id === this.state.calendarTaskData.id);
                calendarDataList.splice(index, 1);
                if(recordStatus!=='D'){
                const newCalendarEvent = this.state.calendarTaskData;
                newCalendarEvent.title = this.state.calendarTaskData.calendarStatus;
                newCalendarEvent.bgColor = newCalendarEvent.title === localConstant.globalCalendar.EVENT_STATUS.BOOKED ? localConstant.globalCalendar.EVENT_COLORS.BOOKED : (newCalendarEvent.title === localConstant.globalCalendar.EVENT_STATUS.TENTATIVE) ? localConstant.globalCalendar.EVENT_COLORS.TENTATIVE : (newCalendarEvent.title === localConstant.globalCalendar.EVENT_STATUS.TBA) ? localConstant.globalCalendar.EVENT_COLORS.TBA : localConstant.globalCalendar.EVENT_COLORS.PTO;
                newCalendarEvent.start = moment(startDate + " " + startTime, localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT);
                newCalendarEvent.end = moment(endDate + " " + endTime, localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT);
                calendarDataList.push(newCalendarEvent);
                }
            }

            if (Array.isArray(calendarDataList) && calendarDataList.length > 0) {
                calendarDataList = calendarDataList.sort((a, b) => {
                    const c = new moment(a.start);
                    const d = new moment(b.start);
                    return c - d;
                });
            }

            this.setState({
                isOpenCalendarPopUp: false,
                //For D924 ,below line commented
                // calendarData: this.state.calendarData.events.push(this.state.calendarTaskData),
                calendarTaskData: {},
                calendarData: {
                    ...this.state.calendarData,
                    events: calendarDataList
                },
                isCalendarDataUpdated: true,
            });
        }
    };

    calendarWarningPopupClick = (e) => {
        e.preventDefault();
        this.props.actions.HideModal();
    }
    /*Calendar functionalities - End */

    assignResourceInactive = async (visitAddItem, prevSelectedTechSpecs) => {        
        prevSelectedTechSpecs = (prevSelectedTechSpecs && prevSelectedTechSpecs.length > 0 ? prevSelectedTechSpecs : this.state.prevSelectedTechSpecs);
        visitAddItem = (visitAddItem && visitAddItem.length > 0 ? visitAddItem: this.state.visitStateAddItem);
        const isTSExists = prevSelectedTechSpecs.findIndex(ts => ts.pin === visitAddItem[0].value);
        if (isTSExists === -1) {
            const visitId = this.props.visitInfo.visitId;
            //Add Newly Added with recordStatus N tech Spec to VisitTechnicalSpecialists Json
            await this.props.actions.AddVisitTechnicalSpecialist({
                visitTechnicalSpecialistId: null,
                visitId: visitId,
                technicalSpecialistName: visitAddItem[0].label,
                pin: visitAddItem[0].value,
                recordStatus: 'N',
                grossMargin: 0
            });
            this.editVisitCalendarDataBind();
            //Defect ID HotFix 650 - Default line items for edit visit also
            //if (this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE) {
                await this.CreateDefaultLineItems(visitAddItem[0].value);
            //}
        } else {
            let ePin = 0;
            prevSelectedTechSpecs.forEach(row => {
                if (row.pin === visitAddItem[0].value && row.recordStatus === 'D') {
                    row.recordStatus = "M";
                    ePin = row.pin;
                }
            });
            await this.props.actions.UpdateVisitTechnicalSpecialist(prevSelectedTechSpecs, ePin);
            this.editVisitCalendarDataBind();
        }        
        this.props.actions.HideModal();            
    };

    render() {
        const filteredVisitStatus = this.filterVisitStatus();        
        const isJobReferenceNumberVisible = this.isJobReferenceVisible();
        const visitSupplierList = this.getVisitSupplierList();
        const hasSupplierFirstVisit = this.isFirstVisitSupplier(visitSupplierList);
        const supplierPOData = {
            supplierPONumber: this.props.visitInfo.visitSupplierPONumber,
            supplierPOId: this.props.visitInfo.visitSupplierPOId
        };
       
        if (this.props.visitInfo && this.props.visitInfo.visitStatus && this.props.visitInfo.visitStatus === 'U'
            && (this.state.isTBAVisitStatusUpdated === undefined || this.state.isTBAVisitStatusUpdated === false)) {
            this.setState({
                isTBAVisitStatus: true,
                isTBAVisitStatusUpdated: true
            });
        }
        let supplierReadonly = true;   
        if (this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE || 
            (this.props.visitInfo && this.props.visitInfo.visitStatus && (this.props.visitInfo.visitStatus === 'C'
            || this.props.visitInfo.visitStatus === 'U' || this.props.visitInfo.visitStatus === 'T' || this.props.visitInfo.visitStatus === 'Q'))) {
            supplierReadonly = false;
        }
        if (this.props.currentPage === localConstant.visit.EDIT_VIEW_VISIT_MODE &&
            (this.props.visitInfo && this.props.visitInfo.visitStatus === 'R' && this.props.isInterCompanyAssignment && this.props.isOperatingCompany)) {
            supplierReadonly = false;
        }
        //Visit Calendar
        const { calendarData } = this.state;
        const calendarDataList = calendarData;
        if (calendarData) {
            if (calendarData.resources && Array.isArray(calendarData.resources)) {
                if (calendarData.resources.length === 0) {
                    calendarDataList.resources = this.defaultCalendarData.resources;
                }
            }
        }

        //Calendar needs to be disable when its empty
        const isCalendarDisabled = calendarData && Array.isArray(calendarData.resources) && (calendarData.resources.length >= 0) && calendarData.resources[0].name !== "" ? false : true;
        let isFinalVisitDisabled = false;
        if (this.props.visitInfo && this.props.visitInfo.visitStatus && [ 'O','A' ].includes(this.props.visitInfo.visitStatus) 
            && this.props.isInterCompanyAssignment && this.props.isOperatingCompany)
        {
            isFinalVisitDisabled = true;
        }
        return (
            <div className="customCard visitTimesheetCalender">
                {
                    this.state.isOpenCalendarPopUp ?
                        <Modal title={this.state.isViewMode ? localConstant.globalCalendar.VIEW_CALENDAR_RECORD : localConstant.globalCalendar.CREATE_EDIT_CALENDAR_RECORD}
                            modalId="calendarPopup"
                            formId="calendarForm"
                            buttons={this.state.isViewMode ? this.calendarViewButtons : this.calendarEditButtons}
                            isShowModal={true}>
                            {this.state.isViewMode ?
                                this.state.templateType !== localConstant.globalCalendar.CALENDAR_STATUS.PTO ? <CalendarTaskInfoView PopupData={this.calendarPopup} TemplateType={this.state.templateType} eventData={this.state.calendarTaskData}/>
                                    : <CalendarPTOTaskInfoView PopupData={this.calendarPopup} TemplateType={this.state.templateType} eventData={this.state.calendarTaskData}/>
                                :
                                <CalendarTaskEditView PopupData={this.calendarPopup}
                                    TemplateType={this.state.templateType}
                                    CalendarTaskData={this.state.calendarTaskData}
                                    calendarTaskStartDateChange={this.calendarTaskStartDateChange}
                                    calendarTaskEndDateChange={this.calendarTaskEndDateChange}
                                    inputChangeHandler={this.calendarInputChangeHandler}
                                    calendarTaskStartTimeChange={this.calendarTaskStartTimeChange}
                                    calendarTaskEndTimeChange={this.calendarTaskEndTimeChange}
                                    disabled={false}
                                    dateHourLimit={this.endDateTimeLimit}
                                    dateMinLimit={this.endDateTimeMinLimit}
                                    isEditTaskMode={this.state.isEditTaskMode} />
                            }
                        </Modal> : null
                }
                <GeneralDetailsDiv
                    visitInfo={this.props.visitInfo}
                    //visitStatus = {this.props.visitStatus}
                    visitStatus={filteredVisitStatus}
                    isVisitStatusDisabled={this.isVisitStatusDisabled}
                    //supplierList={this.props.supplierList}
                    unusedReason = {this.props.unusedReason}
                    supplierPOData={supplierPOData}
                    supplierList={visitSupplierList}
                    hasSupplierFirstVisit={hasSupplierFirstVisit}
                    technicalSpecialistList={this.props.technicalSpecialistList}
                    visitTechnicalSpecialists={this.props.visitTechnicalSpecialists}
                    inputHandleChange={(e) => this.inputHandleChange(e)}
                    selectedRowHandler={this.selectedRowHandler}
                    visitStartDateChange={this.visitStartDateChange}
                    visitEndDateChange={this.visitEndDateChange}
                    visitCustomerDateChange={this.visitCustomerDateChange}
                    visitExpectedCompleteDateChange={this.visitExpectedCompleteDateChange}
                    techSpecChange={this.techSpecChange}
                    handleDateChangeRaw={this.handleDateChangeRaw}
                    onPercentageChange={this.precentageCompleteChange}
                    isJobReferenceNumberVisible={isJobReferenceNumberVisible}
                    isTBAVisitStatus={this.state.isTBAVisitStatus}
                    interactionMode={this.props.interactionMode}
                    subSupplierList={this.props.subSupplierList}
                    supplierReadonly = {supplierReadonly}  
                    isOperatingCompany = {this.props.isOperatingCompany}        
                    isOperatorApporved = {this.props.isOperatorApporved}      
                    isFinalVisitDisabled = {isFinalVisitDisabled}   
                    selectedCompany = {this.selectedCompanyCode}
                    isOCApprovedByOC = {this.props.isOCApprovedByOC}
                    isCHApprovedByOC = {this.props.isCHApprovedByOC}
                    isOCApprovedByCH = {this.props.isOCApprovedByCH}
                    isCHApprovedByCH = {this.props.isCHApprovedByCH}
                    isDisableField = {this.props.isDisableField}
                    isInterCompanyAssignment = {this.props.isInterCompanyAssignment}
                />

                <ResourceScheduler schedulerData={calendarData ? calendarData : this.defaultCalendarData}
                    eventItemClick={this.eventItemClick}
                    eventItemPopoverTemplateResolver={this.eventItemPopoverTemplateResolver}
                    views={schedulerConfigData.filterViews}
                    isCalendarDataUpdated={this.state.isCalendarDataUpdated}
                    nextClick={this.calendarNextClick}
                    prevClick={this.calendarPrevClick}
                    onViewChange={this.calendarOnViewChange}
                    onSelectDate={this.calendarOnSelectDate}
                    eventItemPopoverEnabled={false}
                    newEvent={this.calendarNewEvent}
                    calendarStartDate={this.state.visitCalendarStartDate}
                    // isDisabled={this.props.interactionMode || isCalendarDisabled|| (this.props.isInterCompanyAssignment&&this.props.isCoordinatorCompany)}
                    isDisabled={isCalendarDisabled}
                />

            </div>
        );
    }
}

export default GeneralDetails;
