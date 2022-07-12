import React, { Fragment } from 'react';
import { getlocalizeData } from '../../../../utils/commonUtils';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();

//Calendar Info Popup Template
export const CalendarTaskInfoView = (props) => {
    const { PopupData, TemplateType, eventData } = props;
    return (
        <div class='row'>
            <div className='col s4 bold mb-1'>{localConstant.globalCalendar.PROJECT_NUMBER}</div>
            <div className='col s8 mb-1'>{(((TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE) ? PopupData.searchParameter.projectNumber : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitProjectNumber : PopupData.timesheetProjectNumber) !== "") &&
                (((TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE) ? PopupData.searchParameter.projectNumber : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitProjectNumber : PopupData.timesheetProjectNumber) !== null) ?
                ((TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE) ? PopupData.searchParameter.projectNumber : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitProjectNumber : PopupData.timesheetProjectNumber) : "-"}</div>

            <div className='col s4 bold mb-1'>{localConstant.resourceSearch.ASSIGNMENT_NUMBER}</div>
            <div className='col s8 mb-1'>{(TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.assignmentNumber : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitAssignmentNumber : PopupData.timesheetAssignmentNumber) !== "" &&
                (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.assignmentNumber : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitAssignmentNumber : PopupData.timesheetAssignmentNumber) !== null ?
                (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.assignmentNumber : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitAssignmentNumber : PopupData.timesheetAssignmentNumber) : "-"}</div>

            <div className='col s4 bold mb-1'>{(TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? localConstant.visit.VISIT_NUMBER : localConstant.gridHeader.TIMESHEET_NUMBER}</div>
            <div className='col s8 mb-1'>{(TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? "" : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitNumber : PopupData.timesheetNumber) !== "" &&
                (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? "" : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitNumber : PopupData.timesheetNumber) !== null ?
                (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? "" : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitNumber : PopupData.timesheetNumber) : "-"}</div>

            <div className='col s4 bold mb-1'>{localConstant.techSpec.gridHeaderConstants.CUSTOMER}</div>
            <div className='col s8 mb-1'>{(TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.customerName : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitCustomerName : PopupData.timesheetCustomerName) !== "" &&
                (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.customerName : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitCustomerName : PopupData.timesheetCustomerName) !== null ?
                (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.customerName : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitCustomerName : PopupData.timesheetCustomerName) : "-"}</div>

            <div className='col s4 bold mb-1'> {localConstant.security.userRole.SUPPLIER} </div>
            <div className='col s8 mb-1'>{(TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.supplier : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitSupplier : "") !== "" &&
                (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.supplier : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitSupplier : "") !== null ?
                (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.supplier : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitSupplier : "") : "-"}</div>

            <div className='col s4 bold mb-1'>{localConstant.resourceSearch.SUPPLIER_LOCATION}</div>
            <div class='col s8 mb-1'>{(TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.supplierLocation : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.supplierLocation : "-") !== "" &&
                (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.supplierLocation : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.supplierLocation : "-") !== null ?
                (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.supplierLocation : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.supplierLocation : "-") : "-"}</div>

            <div className='col s4 bold mb-1'>{localConstant.resourceSearch.SUPPLIER_PO}</div>
            <div className='col s8 mb-1'>{(TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? "" : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitSupplierPONumber : "-") !== "" &&
                (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? "" : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitSupplierPONumber : "-") !== null ?
                (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? "" : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitSupplierPONumber : "-") : "-"}</div>

{/* As requested by Francina we changed this */}
            <div className='col s4 bold mb-1'>{localConstant.globalCalendar.MATERIAL_DESCRIPTION}</div>
            <div className='col s8 mb-1'>{(TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? "" : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitMaterialDescription : "") !== "" &&
                (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? "" : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitMaterialDescription : "") !== null ?
                (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.materialDescription : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitMaterialDescription : PopupData.timesheetDescription) : "-"}</div>
            <div className='col s4 bold mb-1'> { localConstant.commonConstants.START_DATE_TIME } </div>
            <div className='col s8 mb-1'>{eventData.start ? moment(eventData.start).format(localConstant.commonConstants.CALENDAR_TWELVE_FORMAT) : ""}</div>
            <div className='col s4 bold mb-1'>{ localConstant.commonConstants.END_DATE_TIME } </div>
            <div className='col s8 mb-1'>{eventData.end ? moment(eventData.end).format(localConstant.commonConstants.CALENDAR_TWELVE_FORMAT) : ""}</div>

            <div className='col s4 bold mb-1'>{ localConstant.globalCalendar.RESOURCE_STATUS } </div>
            <div className='col s8 mb-1'>{eventData.calendarStatus ?eventData.calendarStatus==="PREASGMNT"?"Pre-Assignment":eventData.calendarStatus : "-"}</div>

            <div className='col s4 bold mb-1'>{ localConstant.globalCalendar.NOTE } </div>
            <div className='col s8 mb-1'>{eventData.description ?eventData.description : "-"}</div>
        </div>
    );
};

//Calendar PTO Info Popup Template
export const CalendarPTOTaskInfoView = (props) => {
    const { PopupData, TemplateType, eventData } = props;
    return (
        <div className="row calendarPopUpOverFlow">
            <div className='col s4 bold mb-1'>Leave Type</div>
            <div className='col s8 mb-1'>{(PopupData.leaveCategoryType !== "" && PopupData.leaveCategoryType !== null) ? PopupData.leaveCategoryType : "-"}</div>
            <div className='col s4 bold mb-1'>Leave Description</div>
            {/* <div className='col s8 mb-1' data-position="bottom" title={(PopupData.comments !== "" && PopupData.comments !== null) ? PopupData.comments : "-"}>{(PopupData.comments !== "" && PopupData.comments !== null) ? PopupData.comments : "-"}</div> */}
            <div className='col s8 mb-1'>{(PopupData.comments !== "" && PopupData.comments !== null) ? PopupData.comments : "-"}</div>
            <div className='col s4 bold mb-1'>{localConstant.commonConstants.START_DATE_TIME}</div>
            <div className='col s8 mb-1'>{eventData.start ? moment(eventData.start).format(localConstant.commonConstants.CALENDAR_TWELVE_FORMAT) : ""}</div>
            <div className='col s4 bold mb-1'>{ localConstant.commonConstants.END_DATE_TIME }</div>
            <div className='col s8 mb-1'>{eventData.end ? moment(eventData.end).format(localConstant.commonConstants.CALENDAR_TWELVE_FORMAT) : ""}</div>
        </div>
    );
};

//Calendar Update Popup Template
export const CalendarTaskEditView = (props) => {
    const { CalendarTaskData, calendarTaskStartDateChange, calendarTaskEndDateChange, calendarTaskStartTimeChange, calendarTaskEndTimeChange, isEditTaskMode } = props;
    return (
        <Fragment>
            <div class='row mb-0'>
                <div className='col s6'>
                    <CustomInput
                        hasLabel={true}
                        name="startDateTime"
                        isNonEditDateField={false}
                        colSize={'s12 pl-0'}
                        labelClass="customLabel mandate"
                        label={localConstant.globalCalendar.START_DATE}
                        type='date'
                        placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                        dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                        inputClass="customInputs"
                        onDateChange={calendarTaskStartDateChange}
                        selectedDate={dateUtil.defaultMoment(CalendarTaskData.start)}
                        disabled={props.disabled}
                    />
                </div>
                <div className='col s6 pl-0'>
                    <CustomInput
                        hasLabel={true}
                        name="endDateTime"
                        isNonEditDateField={false}
                        colSize={'s12 pl-0'}
                        labelClass="customLabel mandate"
                        label={localConstant.globalCalendar.END_DATE}
                        type='date'
                        laceholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                        dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                        inputClass="customInputs"
                        onDateChange={calendarTaskEndDateChange}
                        selectedDate={dateUtil.defaultMoment(CalendarTaskData.end)}
                        disabled={props.disabled}
                    />
                </div>
            </div>
            <div class='row mb-0'>
                <div className='col s6'>
                    <CustomInput
                        hasLabel={true}
                        name="startTime"
                        isNonEditDateField={false}
                        colSize={'s12 pl-0'}
                        labelClass="customLabel mandate"
                        label={localConstant.globalCalendar.START_TIME}
                        type='datetime'
                        placeholderText={localConstant.techSpec.common.CUSTOMINPUT_DATE_TIME_FORMAT}
                        dateFormat={localConstant.techSpec.common.CUSTOMINPUT_DATE_TIME_FORMAT}
                        inputClass="customInputs"
                        onDateChange={calendarTaskStartTimeChange}
                        selectedDate={dateUtil.defaultMoment(CalendarTaskData.startTime)}
                        timeFormat={localConstant.techSpec.common.CUSTOMINPUT_DATE_TIME_FORMAT}
                        timeIntervals={15}
                        timeCaption="time"
                    />
                </div>
                <div className='col s6 pl-0'>
                    <CustomInput
                        hasLabel={true}
                        name="endTime"
                        isNonEditDateField={false}
                        colSize={'s12 pl-0'}
                        labelClass="customLabel mandate"
                        label={localConstant.globalCalendar.END_TIME}
                        type='datetime'
                        placeholderText={localConstant.techSpec.common.CUSTOMINPUT_DATE_TIME_FORMAT}
                        dateFormat={localConstant.techSpec.common.CUSTOMINPUT_DATE_TIME_FORMAT}
                        inputClass="customInputs"
                        onDateChange={calendarTaskEndTimeChange}
                        timeFormat={localConstant.techSpec.common.CUSTOMINPUT_DATE_TIME_FORMAT}
                        timeIntervals={15}
                        timeCaption="time"
                        selectedDate={dateUtil.defaultMoment(CalendarTaskData.endTime)}
                        minTime={moment().hours(props.dateHourLimit).minutes(props.dateMinLimit)}
                        maxTime={moment().hours(23).minutes(45)}
                    />
                </div>
            </div>

            <div class='row mb-0'>
                <div className='col s6'>
                    <CustomInput
                        hasLabel={true}
                        name="calendarStatus"
                        colSize='s12 pl-0'
                        labelClass="customLabel mandate"
                        label={localConstant.globalCalendar.RESOURCE_STATUS}
                        type='select'
                        inputClass="customInputs"
                        optionsList={localConstant.globalCalendar.RESOURCE_STATUS_LIST}
                        optionName='name'
                        optionValue='name'
                        maxLength={250}
                        onSelectChange={props.inputChangeHandler}
                        defaultValue={CalendarTaskData.calendarStatus === localConstant.globalCalendar.EVENT_STATUS.BOOKED ? localConstant.globalCalendar.EVENT_STATUS.CONFIRMED : CalendarTaskData.calendarStatus}
                    />
                </div>
            </div>

            <div class='row mb-0'>
                <div className='col s12'>
                    <CustomInput
                        hasLabel={true}
                        name="description"
                        isNonEditDateField={false}
                        colSize={'s12 pl-0'}
                        label={localConstant.globalCalendar.NOTE}//As requested by Francina
                        type='text'
                        inputClass="customInputs"
                        onValueChange={props.inputChangeHandler}
                        defaultValue={CalendarTaskData.description}
                    />
                </div>
            </div>
            {(isEditTaskMode && CalendarTaskData.id && CalendarTaskData.id > 0) ?
                <div className="row mb-0">
                    <div className='col s12'>
                        <label className={'col s3 mt-0 pl-0 '} >
                            <input type="checkbox" onChange={props.inputChangeHandler}
                                name="resourceAllocation" checked={CalendarTaskData.resourceAllocation} className="filled-in" />
                            <span>{localConstant.globalCalendar.RELEASE_ALLOCATION}</span>
                        </label>
                    </div>
                </div>
                :
                null
            }
        </Fragment>
    );
};