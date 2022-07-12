import React, { Component, Fragment } from 'react';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import {
    getlocalizeData,
    formInputChangeHandler,
    isEmptyOrUndefine,
    isEmptyReturnDefault,
    isEmpty
} from '../../../../utils/commonUtils';
import AssignmentAnchor from '../../../viewComponents/assignment/assignmentAnchor';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import CustomMultiSelect from '../../../../common/baseComponents/multiSelect';

//Visit Calendar
import ResourceScheduler from '../../../../components/applicationComponents/scheduler';
import { schedulerConfigData } from '../../../../components/applicationComponents/scheduler/schedulerconfig';
import Modal from '../../../../common/baseComponents/modal';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
import { CalendarTaskInfoView, CalendarPTOTaskInfoView,CalendarTaskEditView } from '../../../../grm/components/applicationComponent/calendarPopupTemplate/calendarTaskInfoView';

const localConstant = getlocalizeData();
const currentDate = moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT);
function createMultiSelectOptions(array) {    
    const options = isEmptyReturnDefault(array);
    return options.map(eachItem => {        
        return { value: eachItem.pin, label: eachItem.fullName.includes(eachItem.pin) ? eachItem.fullName : eachItem.fullName + " (" +  eachItem.pin + ")",
         color: (eachItem.profileStatus === localConstant.assignments.INACTIVE ? localConstant.timesheet.RESOURCE_COLOR.INACTIVE : localConstant.timesheet.RESOURCE_COLOR.ACTIVE) };
    });
}

function selectMultiSelectOptions(array, techSpecList) {    
    const options = isEmptyReturnDefault(array);
    const multiselectArray = [];
    if (options.length > 0) {
        options.map(eachItem => {            
            if (eachItem.recordStatus == null || eachItem.recordStatus !== 'D') {
                const profileStatus = techSpecList.filter(ts => ts.epin === eachItem.pin && ts.profileStatus === localConstant.assignments.INACTIVE).length > 0;
                multiselectArray.push({ value: eachItem.value, label: eachItem.label.includes(eachItem.value) ? eachItem.label : eachItem.label + " (" +  eachItem.value + ")", 
                color: (profileStatus ? localConstant.timesheet.RESOURCE_COLOR.INACTIVE : localConstant.timesheet.RESOURCE_COLOR.ACTIVE) });
            }
        });
    }
    return multiselectArray;
}

export const Details = (props) => {
    const isTechSpecControlDisable = () => {
        /** if intercompany assignment and loggedin user is coordinator disable TS dropdown
         if interactionMode Just View (true)
         timesheet is approved by CHC */

         //Commented because of D780
        // if ((props.isInterCompanyAssignment && props.isCoordinatorCompany)
        //     || props.interactionMode
        //     || 'A' === props.status.value) {
        //     return true;
        // }
        
        if (isEmptyOrUndefine(props.timesheetInfo.timesheetStartDate)
            || isEmptyOrUndefine(props.timesheetInfo.timesheetEndDate) || props.interactionMode) {
            return true;
        }
        else {
            return false;
        }
    };
    const isDisableFromToDates = (props.status.value === 'A' ? (props.isInterCompanyAssignment ? true : false) : false);
    return (<form autoComplete="off">
        <div>
        <span> <label className="bold marginRightTS">{localConstant.visit.GENERAL_DETAILS}</label></span>
         <span><label class="">{localConstant.timesheet.ASSIGNMENT_NO + ': '}</label>
                <AssignmentAnchor
                    assignmentNumber={props.timesheetInfo.timesheetAssignmentNumber ?
                        props.timesheetInfo.timesheetAssignmentNumber.toString().padStart(5, '0') : ''}
                    assignmentId={props.timesheetInfo.timesheetAssignmentId} /></span>
        </div>
        <div className="row mb-0">
            {/* <CustomInput
                hasLabel={true}
                divClassName='col'
                label={localConstant.header.CUSTOMER}
                type='text'
                colSize='s4'
                inputClass="customInputs"
                labelClass="customLabel"
                name="timesheetCustomer"
                autocomplete="off"
                id="timesheetCustomer"
                readOnly={true}
                defaultValue={props.timesheetInfo.timesheetCustomerName ? props.timesheetInfo.timesheetCustomerName : ''}
            /> */}
            <div className='col s4 pr-3 row mb-0 widthChange4' title={props.timesheetInfo.timesheetCustomerName}>
                <label className='customLabel'>{localConstant.header.CUSTOMER}</label>
                <div className='textNoWrapEllipsis'>{props.timesheetInfo.timesheetCustomerName} </div>
            </div>
            <CustomInput
                hasLabel={true}
                name="timesheetStatus"
                colSize='s4 pl-0'
                label={localConstant.timesheet.TIMESHEET_STATUS}
                labelClass="mandate"
                type='select'
                optionsList={props.timesheetStatus}
                optionName='name'
                optionValue='code'
                inputClass="customInputs"
                onSelectChange={props.onValueChange}
                defaultValue={props.timesheetInfo.timesheetStatus}
                disabled={props.isTimesheetStatusDisabled || props.interactionMode}
            />
            <CustomInput
                hasLabel={true}
                name="unusedReason"
                colSize='s4 pl-0'
                label={localConstant.timesheet.UNUSED_REASON}
                labelClass="mandate"
                type='select'
                optionsList={props.unusedReason}
                optionName='name'
                optionValue='code'
                inputClass="customInputs"
                onSelectChange={props.onValueChange}
                defaultValue={props.timesheetInfo.timesheetStatus === 'E' ? props.timesheetInfo.unusedReason: ""}
                disabled={props.timesheetInfo.timesheetStatus !== 'E' || props.interactionMode}
            />
        </div>
        <div className="row mb-0">
            <CustomInput
                hasLabel={true}
                isNonEditDateField={false}
                label={localConstant.timesheet.DATE_FROM}
                labelClass="customLabel mandate"
                colSize='s4'
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                type='date'
                name="timesheetStartDate"
                autocomplete="off"
                selectedDate={isEmptyOrUndefine(props.timesheetInfo.timesheetStartDate) ? "" : dateUtil.defaultMoment(props.timesheetInfo.timesheetStartDate)}
                onDateChange={props.fetchStartDate}
                shouldCloseOnSelect={true}
                disabled={props.interactionMode || isDisableFromToDates}
                onDatePickBlur={(e) => { props.handleDateChangeRaw(e, "fetchStartDate"); }}
            // onDateChangeRaw={(e)=>{ props.handleDateChangeRaw(e,"fetchStartDate"); }}
            />
            <CustomInput
                hasLabel={true}
                labelClass="customLabel mandate"
                label={localConstant.timesheet.DATE_TO}
                colSize='s4 pl-0'
                type="date"
                isNonEditDateField={false}
                selectedDate={isEmptyOrUndefine(props.timesheetInfo.timesheetEndDate) ? "" : dateUtil.defaultMoment(props.timesheetInfo.timesheetEndDate)}
                onDateChange={props.fetchEndDate}
                name='timesheetEndDate'
                autocomplete="off"
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                shouldCloseOnSelect={true}
                disabled={props.interactionMode || isDisableFromToDates}
                onDatePickBlur={(e) => { props.handleDateChangeRaw(e, "fetchEndDate"); }}
            // onDateChangeRaw={(e)=>{ props.handleDateChangeRaw(e,  "fetchEndDate"); }}
            />
        </div>
        <div className="row mb-0">
            <CustomInput
                hasLabel={true}
                divClassName='col'
                name="timesheetDatePeriod"
                colSize='s4'
                label={localConstant.timesheet.TIMESHEET_DATE_PERIOD}
                type='text'
                dataValType='valueText'
                inputClass="customInputs"
                labelClass="customLabel"
                autocomplete="off"
                maxLength={fieldLengthConstants.timesheet.generalDetails.TIMESHEET_DATE_PERIOD_MAXLENGTH}
                id="timesheetDatePeriod"
                onValueChange={props.onValueChange}
                readOnly={props.interactionMode}
                value={props.timesheetInfo.timesheetDatePeriod ? props.timesheetInfo.timesheetDatePeriod : ''}
            />
            <CustomInput
                hasLabel={true}
                label={localConstant.timesheet.TIMESHEET_DESCRIPTION}
                divClassName="col"
                type='text'
                dataValType='valueText'
                name='timesheetDescription'
                maxLength={fieldLengthConstants.timesheet.generalDetails.TIMESHEET_DESCRIPTION_MAXLENGTH}
                colSize='s4 pl-0'
                inputClass="customInputs"
                value={props.timesheetInfo.timesheetDescription ? props.timesheetInfo.timesheetDescription : ''}
                onValueChange={props.onValueChange}
                readOnly={props.interactionMode}
            />
        </div>
        <div className="row mb-0">
            <CustomInput
                hasLabel={true}
                name="timesheetReference1"
                colSize='s4'
                label={localConstant.timesheet.TIMESHEET_REF + ' 1'}
                type='text'
                dataValType='valueText'
                inputClass="customInputs"
                maxLength={fieldLengthConstants.timesheet.generalDetails.TIMESHEET_REFERENCE_MAXLENGTH}
                value={props.timesheetInfo.timesheetReference1 ? props.timesheetInfo.timesheetReference1 : ''}
                readOnly={props.interactionMode}
                onValueChange={props.onValueChange} />
            <CustomInput
                hasLabel={true}
                name="timesheetReference2"
                colSize='s4 pl-0'
                label={localConstant.timesheet.TIMESHEET_REF + ' 2'}
                type='text'
                dataValType='valueText'
                inputClass="customInputs"
                maxLength={fieldLengthConstants.timesheet.generalDetails.TIMESHEET_REFERENCE_MAXLENGTH}
                readOnly={props.interactionMode}
                value={props.timesheetInfo.timesheetReference2 ? props.timesheetInfo.timesheetReference2 : ''}
                onValueChange={props.onValueChange} />
            <CustomInput
                hasLabel={true}
                name="timesheetReference3"
                colSize='s4 pl-0'
                label={localConstant.timesheet.TIMESHEET_REF + ' 3'}
                type='text'
                dataValType='valueText'
                inputClass="customInputs"
                maxLength={fieldLengthConstants.timesheet.generalDetails.TIMESHEET_REFERENCE_MAXLENGTH}
                readOnly={props.interactionMode}
                value={props.timesheetInfo.timesheetReference3 ? props.timesheetInfo.timesheetReference3 : ''}
                onValueChange={props.onValueChange} />
           
            <CustomInput
                labelClass="customLabel mandate"
                hasLabel={true}
                divClassName={"col "}
                name="timesheetTechSpecialists"
                colSize='s8'
                label={localConstant.visit.SELECT_TECH_SPEC}
                type='multiSelect'
                className='browser-default customInputs'
                multiSelectdValue={props.techSpecChange}
                optionsList={createMultiSelectOptions(props.timesheetInfo.assignmentTechSpecialists)}
                defaultValue={selectMultiSelectOptions(props.timesheetSelectedTechSpecs, props.timesheetInfo.assignmentTechSpecialists)}
                disabled={isTechSpecControlDisable() || props.interactionMode || [ 'A', 'O' ].includes(props.status.value) }
            />

            {props.isJobReferenceNumberVisible && <CustomInput
                hasLabel={true}
                name="jobReference"
                colSize='s4 pl-0'
                label={localConstant.visit.JOBREFERENCENO}
                type='text'
                dataValType='valueText'
                inputClass="customInputs"
                readOnly={true}
                value={props.timesheetInfo.timesheetJobReference}
            />}
        </div>
        <div className="row mb-0">
            <CustomInput
                hasLabel={true}
                name="timesheetCompletedPercentage"
                colSize='s4'
                label={localConstant.timesheet.PERCENTAGE_COMPLETE}
                type='text'
                dataType='numeric'
                max="100"
                inputClass="customInputs"
                maxLength={fieldLengthConstants.visit.generalDetails.PERCENTAGE_COMPLETE_MAXLENGTH}
                onValueChange={props.onPercentageChange}
                value={props.timesheetInfo.timesheetCompletedPercentage ? props.timesheetInfo.timesheetCompletedPercentage : ''}
                readOnly={
                    props.interactionMode || [ 'A' ].includes(props.status.value)
                }
            />
            <CustomInput
                hasLabel={true}
                isNonEditDateField={false}
                label={localConstant.timesheet.EXPECTED_COMPLETED_DATE}
                labelClass="customLabel"
                colSize='s4 pl-0'
                dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                type='date'
                name="timesheetExpectedCompleteDate"
                autocomplete="off"
                selectedDate={isEmptyOrUndefine(props.timesheetInfo.timesheetExpectedCompleteDate) ? "" : dateUtil.defaultMoment(props.timesheetInfo.timesheetExpectedCompleteDate)}
                onDateChange={props.fetchExpectedCompleteDate}
                shouldCloseOnSelect={true}
                disabled={
                    props.interactionMode || [ 'A' ].includes(props.status.value)
                }
                onDatePickBlur={(e) => { props.handleDateChangeRaw(e, "fetchExpectedCompleteDate"); }}
            // onDateChangeRaw={(e)=>{ props.handleDateChangeRaw(e, "fetchExpectedCompleteDate"); }}
            />
        </div>
    </form>);
};

class GeneralDetails extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isPanelOpen: true,
            startDate: '',
            endDate: '',
            expectedCompleteDate: '',

            //Calendar
            isOpenCalendarPopUp: false,
            isEditTaskMode: false,
            templateType: "",
            calendarTaskData: {},
            calendarData: {
                resources: [],
                events: []
            },
            isCalendarDataUpdated: false,
            isViewMode: false,
            timesheetAddItem: [],
            selectedTechSpecs:[]
        };
        this.updatedData = {};
        this.isTimesheetStatusDisabled = false;
        this.defaultCalendarData = {
            resources: [
                { id: "", name: "" }
            ],
            events: []
        };

        this.confirmationResourceInactive =  {
            title: localConstant.login.EVOLUTION,
            message: localConstant.errorMessages.TIMESHEET_RESOURCE_INACTIVE,
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
            message: localConstant.globalCalendar.THIS_TIMESHEET_CALENDAR_WARNING,
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

        this.props.callBackFuncs.disableTimesheetStatus = () => {            
            this.isTimesheetStatusControlDisabled();
        };

        this.endDateTimeLimit = 0;
        this.endDateTimeMinLimit = 0;
    }

    calendarDataBind = () => {
        this.editTimesheetCalendarDataBind();
    }

    editTimesheetCalendarDataBind = () => {
        const techIds = [];
        if (this.props.timesheetSelectedTechSpecs) {
            if (Array.isArray(this.props.timesheetSelectedTechSpecs) && this.props.timesheetSelectedTechSpecs.length > 0) {
                this.props.timesheetSelectedTechSpecs.map(tech => {
                    if (tech.recordStatus !== 'D')
                        techIds.push(tech.value);
                });
            }
        }

        if (techIds.length > 0) {
            // Fetch visit calendar data
            this.props.actions.FetchVisitTimesheetCalendarData({
                companyCode: this.props.selectedCompanyCode,
                isActive: true,
                startDateTime: this.props.timesheetInfo ? dateUtil.getWeekStartDate(new Date(this.props.timesheetInfo.timesheetStartDate)) : dateUtil.getWeekStartDate(new Date()),
                endDateTime: this.props.timesheetInfo ? dateUtil.getWeekEndDate(new Date(this.props.timesheetInfo.timesheetEndDate)) : dateUtil.getWeekEndDate(new Date()),
                // calendarType: localConstant.globalCalendar.CALENDAR_STATUS.TIMESHEET,
                // calendarRefCode: this.props.timesheetId ? this.props.timesheetId : ""
            }, techIds).then(response => {
                if (response) {
                    this.calendarResourceList = [];
                    //Setting up data to bind in scheduler
                    if (this.props.timesheetSelectedTechSpecs) {
                        this.props.timesheetSelectedTechSpecs.map(techSpec => {
                            if (techSpec.recordStatus !== 'D') {
                                this.calendarResourceList.push({
                                    id: techSpec.value,
                                    name: techSpec.label.includes(techSpec.value) ? techSpec.label : techSpec.label + " (" + techSpec.value + ")"
                                });
                            }
                        });
                    }
                    response.resources = this.calendarResourceList;

                        if (this.props.timesheetCalendarData) {
                            this.props.timesheetCalendarData.map(calendar => {

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
                        timesheetCalendarStartDate: this.props.timesheetInfo ? this.props.timesheetInfo.timesheetStartDate : new Date()
                    });
                }
            });
        }
        else {
            this.setState({
                calendarData: this.defaultCalendarData,
                isCalendarDataUpdated: true,
                timesheetCalendarStartDate: this.props.timesheetInfo ? this.props.timesheetInfo.timesheetStartDate : new Date()
            });
        }
    }

    componentDidMount() {
        if (this.props.currentPage === localConstant.timesheet.EDIT_VIEW_TIMESHEET_MODE) {
            //TO-DO:Need to check with Ramesh
            // this.props.actions.FetchTimesheetGeneralDetail(this.props.timesheetId);
            if (this.props.timesheetSelectedTechSpecs)
                this.editTimesheetCalendarDataBind();
            this.isTimesheetStatusControlDisabled();
        }
        else {
            const techIds = [];
            if (this.props.timesheetSelectedTechSpecs) {
                if (Array.isArray(this.props.timesheetSelectedTechSpecs) && this.props.timesheetSelectedTechSpecs.length > 0) {
                    this.props.timesheetSelectedTechSpecs.map(tech => {
                        techIds.push(tech.value);
                    });
                }
            }
            if (techIds.length > 0) {
                // Fetch visit calendar data
                this.props.actions.FetchVisitTimesheetCalendarData({
                    companyCode: this.props.selectedCompanyCode,
                    isActive: true,
                    startDateTime: dateUtil.getWeekStartDate(this.props.timesheetInfo&&this.props.timesheetInfo.timesheetStartDate?new Date(this.props.timesheetInfo.timesheetStartDate):new Date()),
                    endDateTime: dateUtil.getWeekEndDate(this.props.timesheetInfo&&this.props.timesheetInfo.timesheetEndDate?new Date(this.props.timesheetInfo.timesheetEndDate):new Date()),
                    // calendarType: localConstant.globalCalendar.CALENDAR_STATUS.TIMESHEET,
                    // calendarRefCode: this.props.timesheetId ? this.props.timesheetId : ""
                }, techIds).then(response => {
                    if (response) {
                        this.calendarResourceList = [];
                        if (this.props.timesheetSelectedTechSpecs) {
                            this.props.timesheetSelectedTechSpecs.map(techSpec => {
                                this.calendarResourceList.push({
                                    id: techSpec.value,
                                    name: techSpec.label
                                });
                            });
                        }
                        response.resources = this.calendarResourceList;
                        if (this.props.timesheetCalendarData) {
                            this.props.timesheetCalendarData.map(calendar => {
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
                            timesheetCalendarStartDate: this.props.timesheetInfo ? this.props.timesheetInfo.timesheetStartDate : new Date()
                        });
                    }
                });
            }
            else {
                this.setState({
                    calendarData: this.defaultCalendarData,
                    isCalendarDataUpdated: true,
                    timesheetCalendarStartDate: this.props.timesheetInfo ? this.props.timesheetInfo.timesheetStartDate : new Date()
                });
            }
        }
    }

    techSpecChange = async (selectedTechSpecs) => {
        const prevSelectedTechSpecs = this.props.timesheetSelectedTechSpecs;
        const timesheetAddItem = selectedTechSpecs.filter(function (el) {
            return !prevSelectedTechSpecs.some(function (f) {
                return f.value === el.value;
            });
        });
        const timesheetRemoveItem = prevSelectedTechSpecs.filter(function (el) {
            return !selectedTechSpecs.some(function (f) {
                return f.value === el.value;
            });
        });
        if (timesheetRemoveItem.length > 0) {
            // -- if add remove, Otherwise Mark record status as D for edit Remove tech spec from  TimesheetTechnicalSpecialists json
            // --if add remove, Otherwise Mark record status as D for edit  any expenses exists for tech spec in the json
            await this.props.actions.RemoveTimesheetTechnicalSpecialist(timesheetRemoveItem[0]);
            if (this.props.currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE) {
                this.props.actions.RemoveTSTimesheetCalendarData(timesheetRemoveItem[0].value);
            }
            await this.props.actions.SelectedTimesheetTechSpecs(selectedTechSpecs);
            this.editTimesheetCalendarDataBind();
        }
        if (timesheetAddItem.length > 0) {
            this.setState({                
                timesheetAddItem: timesheetAddItem,
                selectedTechSpecs: selectedTechSpecs
            });
            const result = this.props.timesheetInfo.assignmentTechSpecialists.filter(ts => ts.pin === timesheetAddItem[0].value && ts.profileStatus === localConstant.assignments.INACTIVE);            
            if(result && result.length > 0) {                
                this.props.actions.DisplayModal(this.confirmationResourceInactive);
            } else {
                this.assignResourceInactive(timesheetAddItem, selectedTechSpecs);                
            }            
        }        
    }

    updateDefaultLineItems(startDate) {
        const {
            timesheetTechnicalSpecialists,
            timesheetTechnicalSpecialistTimes, 
            timesheetTechnicalSpecialistTravels,
            timesheetTechnicalSpecialistExpenses,
            timesheetTechnicalSpecialistConsumables 
        } = this.props;

        if(timesheetTechnicalSpecialistTimes && timesheetTechnicalSpecialistTimes.length > 0) {
            const filteredItemsTime = timesheetTechnicalSpecialistTimes.filter(x => x.isDefault);
            if(filteredItemsTime && filteredItemsTime.length > 0) {
                this.props.actions.DeleteTimesheetTechnicalSpecialistTime(filteredItemsTime);
            }
        }
        if(timesheetTechnicalSpecialistTravels && timesheetTechnicalSpecialistTravels.length > 0) {
            const filteredItemsTravel = timesheetTechnicalSpecialistTravels.filter(x => x.isDefault);
            if(filteredItemsTravel && filteredItemsTravel.length > 0) {
                this.props.actions.DeleteTimesheetTechnicalSpecialistTravel(filteredItemsTravel);
            }
        }
        if(timesheetTechnicalSpecialistExpenses && timesheetTechnicalSpecialistExpenses.length > 0) {
            const filteredItemsExpenses = timesheetTechnicalSpecialistExpenses.filter(x => x.isDefault);
            if(filteredItemsExpenses && filteredItemsExpenses.length > 0) {
                this.props.actions.DeleteTimesheetTechnicalSpecialistExpense(filteredItemsExpenses);
            }
        }
        if(timesheetTechnicalSpecialistConsumables && timesheetTechnicalSpecialistConsumables.length > 0) {
            const filteredItemsConsumable = timesheetTechnicalSpecialistConsumables.filter(x => x.isDefault);
            if(filteredItemsConsumable && filteredItemsConsumable.length > 0) {
                this.props.actions.DeleteTimesheetTechnicalSpecialistConsumable(filteredItemsConsumable);
            }
        }

        timesheetTechnicalSpecialists && timesheetTechnicalSpecialists.forEach(techSpec => {
            this.CreateDefaultLineItems(techSpec.pin, moment(startDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT));
        });
    }
    
    CreateDefaultLineItems = async (pin, startDate) => {
        const chargeRateGridData = isEmptyReturnDefault(this.props.techSpecRateSchedules);
        const timesheetStartDate = moment(this.props.timesheetInfo.timesheetStartDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT);
        const assignmentId = this.props.timesheetInfo.timesheetAssignmentId;
        const lineItemsAdded = [];
        let payScheduleCurrency = '';
        let chargeScheduleCurrency = '';
        let incurredCurrency = '';
        const calculateExchangeRates = [];

        this.props.companyList && this.props.companyList.map(company => {
            if (company.companyCode === this.props.timesheetInfo.timesheetOperatingCompanyCode) {
                incurredCurrency = company.currency;
            };
        });
        const payRateGridData = isEmptyReturnDefault(this.props.techSpecRateSchedules);
        if (!isEmptyOrUndefine(chargeRateGridData.chargeSchedules)) {
            chargeRateGridData.chargeSchedules.forEach(row => {
                if(row.epin === pin && chargeScheduleCurrency === '') chargeScheduleCurrency = row.chargeScheduleCurrency;
                if(incurredCurrency !== row.chargeScheduleCurrency) {
                    calculateExchangeRates.push({
                        currencyFrom: incurredCurrency,
                        currencyTo: row.chargeScheduleCurrency,
                        effectiveDate: (startDate ? startDate : timesheetStartDate)
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
                        effectiveDate: (startDate ? startDate : timesheetStartDate)
                    });
                }
            });
        }
        if(calculateExchangeRates && calculateExchangeRates.length > 0) {
            const resultRates = await this.props.actions.FetchCurrencyExchangeRate(calculateExchangeRates, this.props.timesheetInfo.timesheetContractNumber);  
            if(resultRates && resultRates.length > 0)
            {
                await this.props.actions.UpdateTimesheetExchangeRates(resultRates);
            }
        }
        if (!isEmptyOrUndefine(chargeRateGridData.chargeSchedules)) {
            chargeRateGridData.chargeSchedules.forEach(row => {
                if (row.epin === pin) {
                    row.chargeScheduleRates.forEach(rowData => {
                        const isTypeAdded = this.isLineItemAlreadyAdded(rowData.type, rowData.chargeType, pin, lineItemsAdded);
                        if ((isEmptyOrUndefine(rowData.effectiveTo) || new Date(rowData.effectiveTo) >= new Date(timesheetStartDate))
                            && !isTypeAdded && rowData.isActive) {
                            const tempData = {};
                            if(this.props.currentPage === localConstant.timesheet.EDIT_VIEW_TIMESHEET_MODE) {
                                tempData.timesheetId = this.props.timesheetId;
                            }
                            tempData.expenseDate = (startDate ? startDate : timesheetStartDate);
                            tempData.chargeExpenseType = rowData.chargeType;
                            tempData.pin = pin;
                            tempData.timesheetTechnicalSpecialistId = null;
                            tempData.recordStatus = 'N';
                            tempData.TechSpecTimeId = Math.floor(Math.random() * 9999) - 10000;
                            tempData.assignmentId = assignmentId;
                            tempData.contractNumber = this.props.timesheetInfo.timesheetContractNumber;
                            tempData.projectNumber = this.props.timesheetInfo.timesheetProjectNumber;
                            tempData.chargeRateCurrency = rowData.currency;
                            tempData.payRateCurrency = payScheduleCurrency;
                            tempData.isDefault = true;
                            if (rowData.type === 'R') {
                                tempData.TechSpecTimeId = Math.floor(Math.random() * 9999) - 10000;
                                tempData.chargeTotalUnit = tempData.payUnit = "0.00";
                                tempData.chargeRate = tempData.payRate = "0.0000";                                
                                this.props.actions.AddTimesheetTechnicalSpecialistTime(tempData);
                            } else if (rowData.type === 'T') {
                                tempData.TechSpecTravelId = Math.floor(Math.random() * 9999) - 10000;
                                tempData.chargeUnit = tempData.payUnit = "0.00";
                                tempData.chargeRate = tempData.payRate = "0.0000";
                                tempData.payExpenseType = rowData.chargeType;
                                this.props.actions.AddTimesheetTechnicalSpecialistTravel(tempData);
                            } else if (rowData.type === 'E') {
                                tempData.TechSpecExpenseId = Math.floor(Math.random() * 9999) - 10000;
                                tempData.chargeUnit = tempData.payUnit = tempData.payRateTax = "0.00";
                                tempData.chargeRate = tempData.payRate = "0.0000";
                                tempData.currency = incurredCurrency;
                                if(rowData.currency) tempData.chargeRateExchange = tempData.payRateExchange = "1.000000";
                                if(incurredCurrency === rowData.currency) {
                                    tempData.chargeRateExchange = "1.000000";
                                } else {
                                    this.props.timesheetExchangeRates && this.props.timesheetExchangeRates.map(rates => {
                                        if (rates.currencyFrom === incurredCurrency && rates.currencyTo === rowData.currency) {
                                            tempData.chargeRateExchange = rates.rate;
                                        };
                                    }); 
                                }
                                if(incurredCurrency === payScheduleCurrency) {
                                    tempData.payRateExchange = "1.000000";
                                } else {
                                    this.props.timesheetExchangeRates && this.props.timesheetExchangeRates.map(rates => {
                                        if (rates.currencyFrom === incurredCurrency && rates.currencyTo === payScheduleCurrency) {
                                            tempData.payRateExchange = rates.rate;
                                        };
                                    });
                                }
                                this.props.actions.AddTimesheetTechnicalSpecialistExpense(tempData);
                            } else if (rowData.type === 'C' || rowData.type === 'Q') {
                                tempData.TechSpecConsumableId = Math.floor(Math.random() * 9999) - 10000;
                                tempData.chargeUnit = tempData.payUnit = "0.00";
                                tempData.chargeRate = tempData.payRate = "0.0000";
                                tempData.payType = rowData.chargeType;
                                tempData.chargeDescription = rowData.description;
                                this.props.actions.AddTimesheetTechnicalSpecialistConsumable(tempData);
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
                        if ((isEmptyOrUndefine(rowData.effectiveTo) || new Date(rowData.effectiveTo) >= new Date(timesheetStartDate))
                            && !isTypeAdded && rowData.isActive) {
                            const tempData = {};
                            if(this.props.currentPage === localConstant.timesheet.EDIT_VIEW_TIMESHEET_MODE) {
                                tempData.timesheetId = this.props.timesheetId;
                            }
                            tempData.expenseDate = (startDate ? startDate : timesheetStartDate);
                            tempData.chargeExpenseType = rowData.expenseType;
                            tempData.pin = pin;
                            tempData.timesheetTechnicalSpecialistId = null;
                            tempData.recordStatus = 'N';
                            tempData.TechSpecTimeId = Math.floor(Math.random() * 9999) - 10000;
                            tempData.assignmentId = assignmentId;
                            tempData.contractNumber = this.props.timesheetInfo.timesheetContractNumber;
                            tempData.projectNumber = this.props.timesheetInfo.timesheetProjectNumber;
                            tempData.payRateCurrency = rowData.currency;
                            tempData.chargeRateCurrency = chargeScheduleCurrency;
                            tempData.isDefault = true;
                            if (rowData.type === 'R') {
                                tempData.TechSpecTimeId = Math.floor(Math.random() * 9999) - 10000;
                                tempData.chargeTotalUnit = tempData.payUnit = "0.00";
                                tempData.chargeRate = tempData.payRate = "0.0000";
                                this.props.actions.AddTimesheetTechnicalSpecialistTime(tempData);
                            } else if (rowData.type === 'T') {
                                tempData.TechSpecTravelId = Math.floor(Math.random() * 9999) - 10000;
                                tempData.payExpenseType = rowData.expenseType;
                                tempData.chargeUnit = tempData.payUnit = "0.00";
                                tempData.chargeRate = tempData.payRate = "0.0000";
                                this.props.actions.AddTimesheetTechnicalSpecialistTravel(tempData);
                            } else if (rowData.type === 'E') {
                                tempData.TechSpecExpenseId = Math.floor(Math.random() * 9999) - 10000;
                                tempData.chargeUnit = tempData.payUnit = tempData.payRateTax = "0.00";
                                tempData.chargeRate = tempData.payRate = "0.0000";
                                tempData.currency = incurredCurrency;
                                if(rowData.currency) tempData.chargeRateExchange = tempData.payRateExchange = "1.000000";
                                if(incurredCurrency === rowData.currency) {
                                    tempData.payRateExchange = "1.000000";
                                } else {
                                    this.props.timesheetExchangeRates && this.props.timesheetExchangeRates.map(rates => {
                                        if (rates.currencyFrom === incurredCurrency && rates.currencyTo === rowData.currency) {
                                            tempData.payRateExchange = rates.rate;
                                        };
                                    }); 
                                }
                                if(incurredCurrency === chargeScheduleCurrency) {
                                    tempData.chargeRateExchange = "1.000000";
                                } else {
                                    this.props.timesheetExchangeRates && this.props.timesheetExchangeRates.map(rates => {
                                        if (rates.currencyFrom === incurredCurrency && rates.currencyTo === chargeScheduleCurrency) {
                                            tempData.chargeRateExchange = rates.rate;
                                        };
                                    });
                                }
                                this.props.actions.AddTimesheetTechnicalSpecialistExpense(tempData);
                            } else if (rowData.type === 'C' || rowData.type === 'Q') {
                                tempData.TechSpecConsumableId = Math.floor(Math.random() * 9999) - 10000;
                                tempData.payType = rowData.expenseType;
                                tempData.chargeUnit = tempData.payUnit = "0.00";
                                tempData.chargeRate = tempData.payRate = "0.0000";
                                tempData.payRateDescription = rowData.description;
                                this.props.actions.AddTimesheetTechnicalSpecialistConsumable(tempData);
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
    inputHandleChange = (e) => {
        const inputvalue = formInputChangeHandler(e);
        // if (isTimesheetCreateMode) {
        //     this.updatedData['timesheetStatus'] = 'N';
        // }
        
        this.updatedData[inputvalue.name] = inputvalue.value;
        if (inputvalue.name == 'timesheetStatus') {
            if (inputvalue.value == 'E') {
                this.props.actions.FetchUnusedReason();
            }
            else {
                this.updatedData["unusedReason"] = "";
            }
        }
        this.props.actions.UpdateTimesheetDetails(this.updatedData);
        this.updatedData = {};
    }

    fetchStartDate = (date) => {

        //To check this timesheet already has calendar data
        //Start
        // let isTimesheetHasCalendarData = false;
        // if ((Array.isArray(this.state.calendarData.events) && this.state.calendarData.events.length > 0)) {
        //     const calendarValidateData = this.state.calendarData.events;
        //     for (let index = 0; index < calendarValidateData.length; index++) {
        //         if ((!calendarValidateData[index].calendarRefCode || calendarValidateData[index].calendarRefCode === 0 || this.props.timesheetId) && calendarValidateData[index].recordStatus !== 'D') {
        //             isTimesheetHasCalendarData = true;
        //             break;
        //         }
        //     }
        //     if (isTimesheetHasCalendarData)
        //         this.props.actions.DisplayModal(this.confirmationObject);
        // }
        //end
        this.setState({
            startDate: date,
            timesheetCalendarStartDate: date
        }, () => {
            this.updatedData.timesheetStartDate = this.state.startDate !== null ? this.state.startDate.format() : '';
            this.props.actions.UpdateTimesheetDetails(this.updatedData);
            if (this.props.currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE) {
                this.updateDefaultLineItems(this.updatedData.timesheetStartDate);
            }
            this.updatedData = {};
            this.editTimesheetCalendarDataBind();            
        });
        if (isEmptyOrUndefine(this.state.endDate) && isEmptyOrUndefine(this.props.timesheetInfo.timesheetStartDate)) {
            this.setState({
                endDate: date
            }, () => {
                this.updatedData["timesheetEndDate"] = this.state.startDate !== null ? this.state.startDate.format() : '';
                this.props.actions.UpdateTimesheetDetails(this.updatedData);
                this.updatedData = {};
            });
        }
    }

    /** End date updation handler */
    fetchEndDate = (date) => {

        //To check this timesheet already has calendar data
        //Start
        // let isTimesheetHasCalendarData = false;
        // if ((Array.isArray(this.state.calendarData.events) && this.state.calendarData.events.length > 0)) {
        //     const calendarValidateData = this.state.calendarData.events;
        //     for (let index = 0; index < calendarValidateData.length; index++) {
        //         if ((!calendarValidateData[index].calendarRefCode || calendarValidateData[index].calendarRefCode === 0 || this.props.timesheetId) && calendarValidateData[index].recordStatus !== 'D') {
        //             isTimesheetHasCalendarData = true;
        //             break;
        //         }
        //     }
        //     if (isTimesheetHasCalendarData)
        //         this.props.actions.DisplayModal(this.confirmationObject);
        // }
        //end

        this.setState({
            endDate: date,
        }, () => {
            this.updatedData.timesheetEndDate = this.state.endDate !== null ? this.state.endDate.format() : "";
            this.props.actions.UpdateTimesheetDetails(this.updatedData);
            this.updatedData = {};
            this.editTimesheetCalendarDataBind();
        });
    }

    fetchExpectedCompleteDate = (date) => {
        this.setState({
            expectedCompleteDate: date
        }, () => {
            this.updatedData.timesheetExpectedCompleteDate = this.state.expectedCompleteDate !== null ? this.state.expectedCompleteDate.format() : '';
            this.props.actions.UpdateTimesheetDetails(this.updatedData);
            this.updatedData = {};
        });
    }

    handleDateChangeRaw = (e, type) => {
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

    precentageCompleteChange = (e) => {
        const value = e.target.value;
        if (value >= 0 && value <= 100) {
            this.updatedData[e.target.name] = value;
            this.props.actions.UpdateTimesheetDetails(this.updatedData);
            this.updatedData = {};
        } else {
            IntertekToaster("Percentage Completed should be with in  0 to 100 range", 'warningToast PercentRange');
            if (value < 0) e.target.value = this.props.timesheetInfo.timesheetCompletedPercentage;
            if (value > 100) e.target.value = this.props.timesheetInfo.timesheetCompletedPercentage;
        }
    }

    /*Calendar Functionalities - Start*/

    //To click calendar event
    eventItemClick = (schedulerData, eventItem) => {
        if (this.props.timesheetStatus) {
                if (!eventItem.calendarRefCode || eventItem.calendarRefCode === 0 || eventItem.calendarRefCode === this.props.timesheetId) {
                    // let userTypesArray = [];
                    // if (this.props.userTypes) {
                    //     userTypesArray = this.props.userTypes.split(',');
                    // }
                    // if (userTypesArray.includes(localConstant.userTypeList.Coordinator) || userTypesArray.includes(localConstant.userTypeList.MICoordinator)) {
                        eventItem.startTime = eventItem.start;
                        eventItem.endTime = eventItem.end;
                        eventItem.recordStatus = (eventItem.recordStatus && eventItem.recordStatus === "N") ? "N" : "M";;
                        this.setState({
                            calendarTaskData: eventItem,
                            isCalendarDataUpdated: false
                        });
                        this.endDateTimeLimit = moment(eventItem.startTime).hour();
                        this.endDateTimeMinLimit = moment(eventItem.startTime).minute();
                        this.calendarPopup = this.props.timesheetInfo;
                        this.setState({
                            isOpenCalendarPopUp: true,
                            isEditTaskMode: true,
                            isViewMode: (this.props.interactionMode || 
                                (this.props.isInterCompanyAssignment && this.props.isCoordinatorCompany) || 
                                ((!this.props.isInterCompanyAssignment && this.props.timesheetStatus.value === 'A') ? false : 
                                !(this.props.timesheetStatus.value !== 'A' && this.props.timesheetStatus.value !== 'O') )) ? true : false,
                            templateType: localConstant.globalCalendar.CALENDAR_STATUS.TIMESHEET,
                            isCalendarDataUpdated: false
                        });
                    // }
                    // else {
                    //     if (eventItem.logInName === this.props.userLogonName) {
                    //         eventItem.startTime = eventItem.start;
                    //         eventItem.endTime = eventItem.end;
                    //         eventItem.recordStatus = (eventItem.recordStatus && eventItem.recordStatus === "N") ? "N" : "M";;
                    //         this.setState({
                    //             calendarTaskData: eventItem,
                    //             isCalendarDataUpdated: false
                    //         });
                    //         this.endDateTimeLimit = moment(eventItem.startTime).hour();
                    //         this.endDateTimeMinLimit = moment(eventItem.startTime).minute();
                    //         this.calendarPopup = this.props.timesheetInfo;
                    //         this.setState({
                    //             isOpenCalendarPopUp: true,
                    //             isEditTaskMode: true,
                    //             isViewMode: (this.props.interactionMode || (this.props.isInterCompanyAssignment&&this.props.isCoordinatorCompany)||!(this.props.timesheetStatus.value !== 'A' && this.props.timesheetStatus.value !== 'O'))?true:false,
                    //             templateType: localConstant.globalCalendar.CALENDAR_STATUS.TIMESHEET,
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
                        this.props.actions.FetchVisitByID(eventItem.calendarRefCode).then(response => {
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
                        this.props.actions.FetchTimesheetCalendarDetail(eventItem.calendarRefCode).then(response => {
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
        if (this.props.timesheetInfo && !(this.props.interactionMode || (this.props.isInterCompanyAssignment&&this.props.isCoordinatorCompany))) {
            if (this.props.timesheetStatus) {
                if (!this.props.isInterCompanyAssignment || (this.props.timesheetStatus.value !== 'A' && this.props.timesheetStatus.value !== 'O')) {
                    if (this.props.timesheetInfo.timesheetStartDate && this.props.timesheetInfo.timesheetEndDate) {

                        const startDate = moment(this.props.timesheetInfo.timesheetStartDate).format("DD-MM-YYYY");
                        const endDate = moment(this.props.timesheetInfo.timesheetEndDate).format("DD-MM-YYYY");
                        const eventStartDate = moment(start).format("DD-MM-YYYY");
                        const eventEndDate = moment(end).format("DD-MM-YYYY");

                        const isValidStartBetween = moment(start).isBetween(this.props.timesheetInfo.timesheetStartDate, this.props.timesheetInfo.timesheetEndDate);
                        const isValidEndBetween = moment(end).isBetween(this.props.timesheetInfo.timesheetStartDate, this.props.timesheetInfo.timesheetEndDate);
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
                                    companyCode: this.props.selectedCompanyCode
                                },
                                isCalendarDataUpdated: false
                            });
                            this.calendarPopup = this.props.timesheetInfo;
                            this.setState({
                                isOpenCalendarPopUp: true,
                                isEditTaskMode: false,
                                templateType: localConstant.globalCalendar.CALENDAR_STATUS.TIMESHEET,
                                isCalendarDataUpdated: false
                            });
                        }
                        else
                            IntertekToaster(localConstant.commonConstants.INVALID_CALENDAR_DATE_RANGE, 'warningToast TimeSheetCalendar');
                    }
                }
            }
        }
    };

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

    //Calendar Control click api call
    calendarHandlerToBindData = (schedularData, isViewChange) => {
        const startDate = schedularData.startDate;

        const techIds = [];
        if (this.props.timesheetSelectedTechSpecs) {
            if (Array.isArray(this.props.timesheetSelectedTechSpecs) && this.props.timesheetSelectedTechSpecs.length > 0) {
                this.props.timesheetSelectedTechSpecs.map(tech => {
                    techIds.push(tech.value);
                });
            }
        }
        if (techIds.length > 0) {
            this.props.actions.FetchVisitTimesheetCalendarData({
                companyCode: this.props.selectedCompanyCode,
                isActive: true,
                startDateTime: schedularData.startDate,
                endDateTime: schedularData.endDate,
                // calendarType: localConstant.globalCalendar.CALENDAR_STATUS.TIMESHEET,
                // calendarRefCode: this.props.timesheetId ? this.props.timesheetId : ""
            }, techIds).then(response => {
                response.resources = this.calendarResourceList;
                // const eventsData = [];
                if (this.props.timesheetCalendarData) {
                    this.props.timesheetCalendarData.map(data => {

                        if (response.events && response.events.length > 0) {
                            const index = response.events.findIndex(x => x.id === data.id);
                            if (index >= 0)
                                response.events.splice(index, 1);
                        }
                        if(data.recordStatus!=='D'){
                        data.title = data.calendarStatus;
                        // data.start = moment(data.startDateTime).format(localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT);
                        // data.end = moment(data.endDateTime).format(localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT);
                        data.resourceId = data.technicalSpecialistId;
                        data.bgColor = data.title === localConstant.globalCalendar.EVENT_STATUS.BOOKED ? localConstant.globalCalendar.EVENT_COLORS.BOOKED : (data.title === localConstant.globalCalendar.EVENT_STATUS.TENTATIVE) ? localConstant.globalCalendar.EVENT_COLORS.TENTATIVE : (data.title === localConstant.globalCalendar.EVENT_STATUS.TBA) ? localConstant.globalCalendar.EVENT_COLORS.TBA : localConstant.globalCalendar.EVENT_COLORS.PTO;
                        response.events.push(data);
                        }
                    });
                }
                //Setting up data to bind in scheduler
                this.setState({
                    calendarData: response,
                    isCalendarDataUpdated: true,
                    timesheetCalendarStartDate: startDate
                });
            });
        }
        else {
            this.setState({
                calendarData: this.defaultCalendarData,
                isCalendarDataUpdated: true,
                timesheetCalendarStartDate: this.props.timesheetInfo ? this.props.timesheetInfo.timesheetStartDate : new Date()
            });
        }
    }
    //Calendar Next Click
    calendarNextClick = (schedularData) => {
        this.calendarHandlerToBindData(schedularData, true);
    }

    //Calendar Previous Click
    calendarPrevClick = (schedularData) => {
        this.calendarHandlerToBindData(schedularData, true);
    }

    //Calendar View change
    calendarOnViewChange = (schedularData) => {
        this.calendarHandlerToBindData(schedularData, true);
    }

    //Calendar select Date
    calendarOnSelectDate = (schedularData) => {
        this.calendarHandlerToBindData(schedularData, true);
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
                IntertekToaster("Enter Calendar Status", 'warningToast VisitCalendar');
                return false;
            }
            else {
                if (this.state.calendarTaskData.start && this.state.calendarTaskData.end) {
                    const eventStartDate = moment(this.state.calendarTaskData.start).format(localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT);
                    const eventEndDate = moment(this.state.calendarTaskData.end).format(localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT);
                    const isValidEventDate = moment(eventStartDate,localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT).isSame(moment(eventEndDate,localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT)) || moment(eventEndDate,localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT).isAfter(moment(eventStartDate,localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT));
                    if (isValidEventDate) {
                        const startDate = moment(this.props.timesheetInfo.timesheetStartDate).format(localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT);
                        const endDate = moment(this.props.timesheetInfo.timesheetEndDate).format(localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT);

                        const isValidStartBetween = moment(eventStartDate,localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT).isBetween(moment(startDate,localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT),moment(endDate,localConstant.techSpec.common.CUSTOMINPUT_DATE_FORMAT));
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
                    } else {
                        IntertekToaster(localConstant.commonConstants.TIMESHEET_INVALID_CALENDAR_START_DATE_RANGE, 'warningToast VisitCalendar');
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
            let recordStatus = this.state.calendarTaskData.recordStatus;
            const startTime = moment(this.state.calendarTaskData.startTime).format(localConstant.techSpec.common.CUSTOMINPUT_DATE_TIME_FORMAT);
            const endTime = moment(this.state.calendarTaskData.endTime).format(localConstant.techSpec.common.CUSTOMINPUT_DATE_TIME_FORMAT);
            const startDate = moment(this.state.calendarTaskData.start).format(localConstant.commonConstants.UI_DATE_FORMAT);
            const endDate = moment(this.state.calendarTaskData.end).format(localConstant.commonConstants.UI_DATE_FORMAT);
            if (this.state.calendarTaskData.resourceAllocation) {
                recordStatus = "D";
            }
            let calendarDataList = this.state.calendarData.events;

            const index = this.props.timesheetCalendarData.findIndex(x => x.id === this.state.calendarTaskData.id);

            if (index < 0) {
                const calendarDataToSave = this.state.calendarTaskData;
                if (this.state.calendarTaskData.recordStatus === "N")
                    calendarDataToSave.id = (this.props.timesheetCalendarData) ? -(this.props.timesheetCalendarData.length + 1) : -1;
                calendarDataToSave.start = moment(startDate + " " + startTime, localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT);
                calendarDataToSave.end = moment(endDate + " " + endTime, localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT);
                calendarDataToSave.recordStatus=recordStatus;
                this.props.actions.AddTimesheetCalendarData(calendarDataToSave, moment.utc(startDate + " " + startTime, localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT), moment.utc(endDate + " " + endTime, localConstant.commonConstants.CALENDAR_CONVERT_DATE_FORMAT));
                if (this.state.calendarTaskData.recordStatus === "M" || this.state.calendarTaskData.recordStatus === "D") {
                    const index = calendarDataList.findIndex(x => x.id === this.state.calendarTaskData.id);
                    calendarDataList.splice(index, 1);
                }
                if(recordStatus!=='D'){
                const newCalendarEvent = this.state.calendarTaskData;
                newCalendarEvent.title = this.state.calendarTaskData.calendarStatus;
                newCalendarEvent.bgColor = newCalendarEvent.title === localConstant.globalCalendar.EVENT_STATUS.BOOKED ? localConstant.globalCalendar.EVENT_COLORS.BOOKED : (newCalendarEvent.title === localConstant.globalCalendar.EVENT_STATUS.TENTATIVE) ? localConstant.globalCalendar.EVENT_COLORS.TENTATIVE : (newCalendarEvent.title === localConstant.globalCalendar.EVENT_STATUS.TBA) ? localConstant.globalCalendar.EVENT_COLORS.TBA : localConstant.globalCalendar.EVENT_COLORS.PTO;
                // newCalendarEvent.start = startDate + " " + startTime;
                // newCalendarEvent.end = endDate + " " + endTime;
                calendarDataList.push(newCalendarEvent);
                }
            }
            else {
                let updateCalendarData = [];
                updateCalendarData = this.props.timesheetCalendarData;
                updateCalendarData.map(calendar => {
                    if (calendar.id === this.state.calendarTaskData.id) {
                        // calendar = this.state.calendarTaskData;
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
                this.props.actions.UpdateTimesheetCalendarData(updateCalendarData);
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
                // this.props.actions.updateVisitCalendarData(updateCalendarData);
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

    assignResourceInactive = async (timesheetAddItem, selectedTechSpecs) => {        
        timesheetAddItem = (timesheetAddItem && timesheetAddItem.length > 0 ? timesheetAddItem : this.state.timesheetAddItem);  
        selectedTechSpecs  = (selectedTechSpecs && selectedTechSpecs.length > 0 ? selectedTechSpecs: this.state.selectedTechSpecs);
        const { timesheetId } = this.props;
        const prevSelectedTechSpecs = this.props.timesheetTechnicalSpecialists;
        const isTSExists = prevSelectedTechSpecs.findIndex(ts => ts.pin === timesheetAddItem[0].value);
        if (isTSExists === -1) {
            //Add Newly Added with recordStatus N tech Spec to TimesheetTechnicalSpecialists Json
            await this.props.actions.AddTimesheetTechnicalSpecialist({
                timesheetTechnicalSpecialistId: null,
                timesheetId: timesheetId,
                technicalSpecialistName: timesheetAddItem[0].label,
                pin: timesheetAddItem[0].value,
                recordStatus: 'N'
            });
            //Defect ID HotFix 650 - Default line items for edit timesheet also
            //if (this.props.currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE) {
                await this.CreateDefaultLineItems(timesheetAddItem[0].value);
            //}
        } else {            
            let ePin = 0;
            prevSelectedTechSpecs && prevSelectedTechSpecs.forEach(row => {
                if (row.pin === timesheetAddItem[0].value && row.recordStatus === 'D') {
                    row.recordStatus = "M";
                    ePin = row.pin;
                }
            });
            await this.props.actions.UpdateTimesheetTechnicalSpecialist(prevSelectedTechSpecs, ePin);
        }
        
        await this.props.actions.SelectedTimesheetTechSpecs(selectedTechSpecs);
        this.editTimesheetCalendarDataBind();
        this.props.actions.HideModal();            
    };

    filterTimesheetStatus() {
        const filteredTimesheetStatus = [];
        if (!isEmptyOrUndefine(this.props.timesheetStatusList) && this.props.currentPage !== localConstant.timesheet.EDIT_VIEW_TIMESHEET_MODE) {
            this.props.timesheetStatusList.forEach(statusData => {
                if ([ 'C', 'N' ].includes(statusData.code))
                    filteredTimesheetStatus.push(statusData);
            });
            return filteredTimesheetStatus;
        } else {
            if (!isEmptyOrUndefine(this.props.timesheetInfo)) {
                const selectedTimesheetStatus = this.props.timesheetInfo.timesheetStatus;
                if (isEmpty(selectedTimesheetStatus) || [ 'C', 'N', 'E' ].includes(selectedTimesheetStatus)) {
                    this.props.timesheetStatusList.forEach(statusData => {
                        if ([ 'C', 'N', 'E' ].includes(statusData.code))
                            filteredTimesheetStatus.push(statusData);
                    });
                    return filteredTimesheetStatus;
                }
                return this.props.timesheetStatusList;
            } else {
                return this.props.timesheetStatusList;
            }
        }
    }

    isTimesheetStatusControlDisabled() {
        if (this.props.currentPage === localConstant.timesheet.EDIT_VIEW_TIMESHEET_MODE
            && !isEmptyOrUndefine(this.props.timesheetInfo) && !isEmptyOrUndefine(this.props.timesheetInfo.timesheetStatus)) {    
            if ([ 'C', 'A', 'J', 'O', 'R', 'E' ].includes(this.props.timesheetInfo.timesheetStatus)) {                    
                this.isTimesheetStatusDisabled = true;
            } else {
                this.isTimesheetStatusDisabled = false;
            }    
        }
    }

    render() {
        const {
            timesheetStatus,
            timesheetInfo,
            interactionMode,
            timesheetSelectedTechSpecs
        } = this.props;
        const filteredTimesheetStatus = this.filterTimesheetStatus();
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

        return (
            <div className="customCard visitTimesheetCalender">
                <Details
                    fetchStartDate={this.fetchStartDate}
                    fetchEndDate={this.fetchEndDate}
                    timesheetInfo={timesheetInfo}
                    status={timesheetStatus}
                    unusedReason = {this.props.unusedReason}
                    onValueChange={(e) => this.inputHandleChange(e)}
                    onPercentageChange={this.precentageCompleteChange}
                    interactionMode={interactionMode}
                    handleDateChangeRaw={this.handleDateChangeRaw}
                    techSpecChange={(e) => this.techSpecChange(e)}
                    isCoordinatorCompany={this.props.isCoordinatorCompany}
                    timesheetSelectedTechSpecs={timesheetSelectedTechSpecs}
                    isInterCompanyAssignment={this.props.isInterCompanyAssignment}
                    fetchExpectedCompleteDate={this.fetchExpectedCompleteDate}
                    timesheetStatus = {filteredTimesheetStatus}
                    isTimesheetStatusDisabled ={this.isTimesheetStatusDisabled}
                    isJobReferenceNumberVisible={this.props.currentPage === localConstant.timesheet.EDIT_VIEW_TIMESHEET_MODE}
                />

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
                    calendarStartDate={this.state.timesheetCalendarStartDate}
                   // isDisabled={interactionMode || isCalendarDisabled|| (this.props.isInterCompanyAssignment&&this.props.isCoordinatorCompany)}
                   isDisabled={isCalendarDisabled}
                />
            </div>
        );
    }
}

export default GeneralDetails;