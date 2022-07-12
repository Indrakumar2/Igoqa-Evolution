import React, { Component, Fragment } from 'react';
import ResourceScheduler from '../../../../components/applicationComponents/scheduler';
import Modal from '../../../../common/baseComponents/modal';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import { getlocalizeData, formInputChangeHandler } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import moment from 'moment';
import { Btn } from '../../../../common/resourceSearch/resourceFields';
import Panel from '../../../../common/baseComponents/panel';
import { schedulerConfigData } from '../../../../components/applicationComponents/scheduler/schedulerconfig';
import { CalendarTaskInfoView, CalendarPTOTaskInfoView } from '../../applicationComponent/calendarPopupTemplate/calendarTaskInfoView';

const localConstant = getlocalizeData();

//Calendar Search Template
export const CalendarSearchView = (props) => {
    const { } = props;
    return (
        <Fragment>
             <form onSubmit={props.calendarSearchClick}> 
                <div className="row mb-0">
                    <CustomInput
                        hasLabel={true}
                        name="resourceName"
                        colSize='s2'
                        label={localConstant.globalCalendar.RESOURCE_NAME}
                        type='text'
                        dataValType='valueText'
                        inputClass="customInputs"
                        maxLength={50}
                        onValueChange={props.searchInputHandler}
                        defaultValue={props.calendarSearchData.resourceName}                     
                    />
                    <CustomInput
                        hasLabel={true}
                        name="customerName"
                        colSize='s3 pl-0'
                        label={localConstant.globalCalendar.CUSTOMER_NAME}
                        type='text'
                        dataValType='valueText'
                        inputClass="customInputs"
                        maxLength={50}
                        onValueChange={props.searchInputHandler}
                        defaultValue={props.calendarSearchData.customerName} 
                    />
                    <CustomInput
                        hasLabel={true}
                        name="supplierName"
                        colSize='s2 pl-0'
                        label={localConstant.globalCalendar.SUPPLIER_NAME}
                        type='text'
                        dataValType='valueText'
                        inputClass="customInputs"
                        maxLength={50}
                        onValueChange={props.searchInputHandler}
                        defaultValue={props.calendarSearchData.supplierName}
                    />
                    <CustomInput
                        hasLabel={true}
                        name="supplierLocation"
                        colSize='s2 pl-0'
                        label={localConstant.globalCalendar.SUPPLIER_CITY}
                        type='text'
                        dataValType='valueText'
                        inputClass="customInputs"
                        maxLength={50}
                        onValueChange={props.searchInputHandler}
                        defaultValue={props.calendarSearchData.supplierLocation} 
                    />
                    <CustomInput
                        hasLabel={true}
                        name="jobReferenceNumber"
                        colSize='s2 pl-0'
                        label={localConstant.globalCalendar.JOB_REFERENCE_NUMBER}
                        type='text'
                        dataValType='valueText'
                        inputClass="customInputs"
                        maxLength={50}
                        onValueChange={props.searchInputHandler}
                        defaultValue={props.calendarSearchData.jobReferenceNumber} 
                    />

                    <input type='submit' id="globalSearchBtn" className={"modal-close waves-effect waves-teal btn-small mt-4x col globalSearchBtn"} value={localConstant.commonConstants.SEARCH} /> 
                    </div>
             </form>
        </Fragment>
    );
};

class GlobalCalendar extends Component {

    constructor(props) {
        super(props);
        this.state = {
            isOpenCalendarPopUp: false,
            isEditTaskMode: false,
            templateType: "",
            calendarTaskData: {},
            calendarData: {
                resources: [],
                events: []
            },
            isCalendarDataUpdated: false,
            //Pagination values
            currentPage: 1,
            todosPerPage: 10,
            calendarStart: ""
        };                

        //Buttons
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
        this.calendarSearchButton = [
            {
                name: localConstant.commonConstants.SEARCH,
                action: this.calendarSearchClick,
                btnID: "closeErrorList",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
            }
        ];

        this.calendarSearchData = {};
        this.calendarStartDate = dateUtil.getWeekStartDate(new Date());
        this.calendarEndDate = dateUtil.getWeekEndDate(new Date());
        this.calendarPopup = [];
        this.calendarView = this.calendarView.bind(this);
        this.calendarTaskStartDateChange = this.calendarTaskStartDateChange.bind(this);
        this.saveCalendarPopup = this.saveCalendarPopup.bind(this);
        this.inputChangeHandler = this.inputChangeHandler.bind(this);
        this.calendarTaskStartTimeChange = this.calendarTaskStartTimeChange.bind(this);
        this.calendarTaskEndTimeChange = this.calendarTaskEndTimeChange.bind(this);
        this.searchInputHandler = this.searchInputHandler.bind(this);
        this.calendarSearchClick = this.calendarSearchClick.bind(this);
        this.pagenationClick = this.pagenationClick.bind(this);
        this.pagenationNextClick = this.pagenationNextClick.bind(this);
        this.pagenationPreiousClick = this.pagenationPreiousClick.bind(this);

        this.paginationText = 1;
        this.pageNumbers = [];
    }

    componentDidMount() {   
        this.props.actions.FetchCalendarData({
            companyCode: this.props.selectedCompanyCode,
            isActive: true,
            startDateTime: dateUtil.getWeekStartDate(new Date()),
            endDateTime: dateUtil.getWeekEndDate(new Date()),
            resourceName: this.calendarSearchData.resourceName,
            customerName: this.calendarSearchData.customerName,
            supplierName: this.calendarSearchData.supplierName,
            supplierLocation: this.calendarSearchData.supplierLocation,
            jobReferenceNumber:this.calendarSearchData.jobReferenceNumber
        }, true).then(response => {
            //Setting up data to bind in scheduler
            this.setState({
                calendarData: response,
                isCalendarDataUpdated: true
            });
        });
    }

    componentWillUnmount() {

        //Clear the all calendar local state values
        this.setState({
            isOpenCalendarPopUp: false,
            isEditTaskMode: false,
            templateType: "",
            calendarTaskData: {},
            calendarData: {
                resources: [],
                events: []
            },
            isCalendarDataUpdated: false
        });

    }

    //Task start date change
    calendarTaskStartDateChange = (date) => {
        this.setState({
            calendarTaskData: {
                ...this.state.calendarTaskData,
                startDateTime: date
            },
            isCalendarDataUpdated: false
        });
    }

    //Task end date change
    calendarTaskEndDateChange = (date) => {
        this.setState({
            calendarTaskData: {
                ...this.state.calendarTaskData,
                endDateTime: date
            },
            isCalendarDataUpdated: false
        });
    }

    //Task start time change
    calendarTaskStartTimeChange = (date) => {
        const time = moment(date).format('h:mm a');
        this.setState({
            calendarTaskData: {
                ...this.state.calendarTaskData,
                startTime: date
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
    
    //Set Focus On Enter Search button 
    focus=()=> {      
        const searchbtn = document.getElementById('globalSearchBtn');
        searchbtn.focus();
      }

    //Calendar popup input change
    inputChangeHandler = (e) => {
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        if (inputvalue.name === "calendarStatus") {
            this.setState({
                calendarTaskData: {
                    ...this.state.calendarTaskData,
                    calendarStatus: inputvalue.value
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

    //Calendar search input change 
    searchInputHandler = (e) => {            
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        if (inputvalue.value.lastIndexOf("*") > 0)
            inputvalue.value = inputvalue.value.substring('*', inputvalue.value.length - 1);
        if (inputvalue.name === "resourceName") {
            this.calendarSearchData.resourceName = inputvalue.value;
        }
        else if (inputvalue.name === "customerName") {
            this.calendarSearchData.customerName = inputvalue.value;
        }
        else if (inputvalue.name === "supplierName") {
            this.calendarSearchData.supplierName = inputvalue.value;
        }
        else if (inputvalue.name === "supplierLocation") {
            this.calendarSearchData.supplierLocation = inputvalue.value;
        }
        else if (inputvalue.name === "jobReferenceNumber") {
            this.calendarSearchData.jobReferenceNumber = inputvalue.value;
        }
        
    }

    //Calendar search button click
    calendarSearchClick = (e) => {      
        this.focus();
        e.preventDefault();
        this.props.actions.FetchCalendarData({
            companyCode: this.props.selectedCompanyCode,
            isActive: true,
            startDateTime: this.calendarStartDate,
            endDateTime: this.calendarEndDate,
            resourceName: this.calendarSearchData.resourceName,
            customerName: this.calendarSearchData.customerName,
            supplierName: this.calendarSearchData.supplierName,
            supplierLocation: this.calendarSearchData.supplierLocation,
            jobReferenceNumber: this.calendarSearchData.jobReferenceNumber
        }, true).then(response => {

            this.paginationText = 1;
            if (this.calendarSearchData.jobReferenceNumber != "" && this.calendarSearchData.jobReferenceNumber !== undefined && this.calendarSearchData.jobReferenceNumber !== null) {
                const getEvent = response.events.filter(x => x.title == this.calendarSearchData.jobReferenceNumber);
                response.events = [];
                response.events.push(...getEvent);
                const getResourceId = getEvent.map(y => y.resourceId);
                const listResource = response.resources.filter(x => getResourceId.includes(x.id));
                response.resources = [];
                response.resources.push(...listResource);    
                this.setState({
                        calendarData: response,
                        isCalendarDataUpdated: true,
                        calendarStart:response.events.length > 0 ? response.events[0].start:""
                    });
            }
            else{
            this.setState({
                calendarData: response,
                isCalendarDataUpdated: true,
                currentPage: this.paginationText
            });
        }
        });
    }

    //To close calendar popup
    cancelCalendarPopup = (e) => {
        e.preventDefault();
        this.setState({
            isOpenCalendarPopUp: false,
            calendarTaskData: {},
            isCalendarDataUpdated: false
        });
    };

    //To save calendar popup
    saveCalendarPopup = (e) => {
        e.preventDefault();
        const startTime = moment(this.state.calendarTaskData.startTime).format('h:mm a');
        const endTime = moment(this.state.calendarTaskData.endTime).format('h:mm a');
        const startDate = moment(this.state.calendarTaskData.start).format('DD-MM-YYYY');
        const endDate = moment(this.state.calendarTaskData.end).format('DD-MM-YYYY');

        this.props.actions.SaveCalendarTask(this.state.calendarTaskData, moment.utc(startDate + " " + startTime, "DD-MM-YYYY HH:mm a"), moment.utc(endDate + " " + endTime, "DD-MM-YYYY HH:mm a")).then(response => {
            this.setState({
                isOpenCalendarPopUp: false,
                calendarTaskData: {},
                isCalendarDataUpdated: false
            });
        });
    };

    //To click calendar event
    eventItemClick = (schedulerData, eventItem) => {
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
                            isEditTaskMode: false,
                            templateType: localConstant.globalCalendar.CALENDAR_STATUS.PTO,
                            isCalendarDataUpdated: false
                        });
                    }
                }
            });
        }
    };

    //Calendar view button click
    calendarView = (eventItem) => {
        if (eventItem.calendarType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) {
            this.props.actions.FetchVisitByID(eventItem.calendarRefCode).then(response => {
                this.calendarPopup = response;
                this.setState({
                    isOpenCalendarPopUp: true,
                    isEditTaskMode: false,
                    templateType: localConstant.globalCalendar.CALENDAR_STATUS.VISIT,
                    isCalendarDataUpdated: false
                });
            });
        }
        else if (eventItem.calendarType === localConstant.globalCalendar.CALENDAR_STATUS.TIMESHEET) {
            this.props.actions.FetchTimesheetGeneralDetail(eventItem.calendarRefCode).then(response => {
                this.calendarPopup = response;
                this.setState({
                    isOpenCalendarPopUp: true,
                    isEditTaskMode: false,
                    templateType: localConstant.globalCalendar.CALENDAR_STATUS.TIMESHEET,
                    isCalendarDataUpdated: false
                });
            });
        }
        else if (eventItem.calendarType === localConstant.globalCalendar.CALENDAR_STATUS.PRE) {
            this.props.actions.FetchPreAssignment(eventItem.calendarRefCode).then(response => {
                this.calendarPopup = response;
                this.setState({
                    isOpenCalendarPopUp: true,
                    isEditTaskMode: false,
                    templateType: localConstant.globalCalendar.CALENDAR_STATUS.PRE,
                    isCalendarDataUpdated: false
                });
            });
        }
    }

    //Calendar edit button click
    calendarEditView = (eventItem) => {
        eventItem.startTime = eventItem.start;
        eventItem.endTime = eventItem.end;
        this.setState({
            calendarTaskData: eventItem,
            isCalendarDataUpdated: false
        });
        if (eventItem.calendarType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) {
            this.props.actions.FetchVisitByID(eventItem.calendarRefCode).then(response => {
                this.calendarPopup = response;
                this.setState({
                    isOpenCalendarPopUp: true,
                    isEditTaskMode: true,
                    templateType: localConstant.globalCalendar.CALENDAR_STATUS.VISIT,
                    isCalendarDataUpdated: false
                });
            });
        }
        else if (eventItem.calendarType === localConstant.globalCalendar.CALENDAR_STATUS.TIMESHEET) {
            this.props.actions.FetchTimesheetGeneralDetail(eventItem.calendarRefCode).then(response => {
                this.calendarPopup = response;
                this.setState({
                    isOpenCalendarPopUp: true,
                    isEditTaskMode: true,
                    templateType: localConstant.globalCalendar.CALENDAR_STATUS.TIMESHEET,
                    isCalendarDataUpdated: false
                });
            });
        }
        else if (eventItem.calendarType === localConstant.globalCalendar.CALENDAR_STATUS.PRE) {
            this.props.actions.FetchPreAssignment(eventItem.calendarRefCode).then(response => {
                this.calendarPopup = response;
                this.setState({
                    isOpenCalendarPopUp: true,
                    isEditTaskMode: true,
                    templateType: localConstant.globalCalendar.CALENDAR_STATUS.PRE,
                    isCalendarDataUpdated: false
                });
            });
        }
    }

    //To popover task in calendar
    eventItemPopoverTemplateResolver = (schedulerData, eventItem, title, start, end, statusColor) => {
        if (eventItem.calendarType != localConstant.globalCalendar.CALENDAR_STATUS.PTO) {
            return (
                <div>
                    <a className={"waves-effect waves-light pt-1 pr-1 pl-1 pb-1 bold mr-2"} onClick={() => this.calendarEditView(eventItem)}><i className="zmdi zmdi-edit left cal_icon"></i> Edit</a>
                    <a className={"waves-effect waves-light pt-1 pr-1 pl-1 pb-1 bold ml-2 "} onClick={() => this.calendarView(eventItem)}><i className="zmdi zmdi-eye left cal_icon"></i> View</a>
                </div>
            );
        }
        return (
            <div>
                <p>{eventItem.title}</p>
            </div>
        );
    }

    //Calendar Next Click
    calendarNextClick = (schedularData) => {
        this.calendarStartDate = schedularData.startDate;
        this.calendarEndDate = schedularData.endDate;
        this.props.actions.FetchCalendarData({
            companyCode: this.props.selectedCompanyCode,
            isActive: true,
            startDateTime: schedularData.startDate,
            endDateTime: schedularData.endDate,
            resourceName: this.calendarSearchData.resourceName,
            customerName: this.calendarSearchData.customerName,
            supplierName: this.calendarSearchData.supplierName,
            supplierLocation: this.calendarSearchData.supplierLocation,
            jobReferenceNumber: this.calendarSearchData.jobReferenceNumber
        }, true).then(response => {
            if (this.calendarSearchData.jobReferenceNumber != "" && this.calendarSearchData.jobReferenceNumber !== undefined && this.calendarSearchData.jobReferenceNumber !== null) {
                const getEvent = response.events.filter(x => x.title == this.calendarSearchData.jobReferenceNumber);
                response.events = [];
                response.events.push(...getEvent);
                const getResourceId = getEvent.map(y => y.resourceId);
                const listResource = response.resources.filter(x => getResourceId.includes(x.id));
                response.resources = [];
                response.resources.push(...listResource);
                this.setState({
                    calendarData: response,
                    isCalendarDataUpdated: true,
                    calendarStart: ""
                });
            }
            else {
                //Setting up data to bind in scheduler
                this.setState({
                    calendarData: response,
                    isCalendarDataUpdated: true
                });
            }
        });
    }

    //Calendar Previous Click
    calendarPrevClick = (schedularData) => {
        this.calendarStartDate = schedularData.startDate;
        this.calendarEndDate = schedularData.endDate;
        this.props.actions.FetchCalendarData({
            companyCode: this.props.selectedCompanyCode,
            isActive: true,
            startDateTime: schedularData.startDate,
            endDateTime: schedularData.endDate,
            resourceName: this.calendarSearchData.resourceName,
            customerName: this.calendarSearchData.customerName,
            supplierName: this.calendarSearchData.supplierName,
            supplierLocation: this.calendarSearchData.supplierLocation,
            jobReferenceNumber: this.calendarSearchData.jobReferenceNumber
        }, true).then(response => {
            if (this.calendarSearchData.jobReferenceNumber != "" && this.calendarSearchData.jobReferenceNumber !== undefined && this.calendarSearchData.jobReferenceNumber !== null) {
                const getEvent = response.events.filter(x => x.title == this.calendarSearchData.jobReferenceNumber);
                response.events = [];
                response.events.push(...getEvent);
                const getResourceId = getEvent.map(y => y.resourceId);
                const listResource = response.resources.filter(x => getResourceId.includes(x.id));
                response.resources = [];
                response.resources.push(...listResource);
                this.setState({
                    calendarData: response,
                    isCalendarDataUpdated: true,
                    calendarStart: ""
                });
            }
            else {
                //Setting up data to bind in scheduler
                this.setState({
                    calendarData: response,
                    isCalendarDataUpdated: true
                });
            }
        });
    }

    //Calendar View change
    calendarOnViewChange = (schedularData) => {
        this.calendarStartDate = schedularData.startDate;
        this.calendarEndDate = schedularData.endDate;
        this.props.actions.FetchCalendarData({
            companyCode: this.props.selectedCompanyCode,
            isActive: true,
            startDateTime: schedularData.startDate,
            endDateTime: schedularData.endDate,
            resourceName: this.calendarSearchData.resourceName,
            customerName: this.calendarSearchData.customerName,
            supplierName: this.calendarSearchData.supplierName,
            supplierLocation: this.calendarSearchData.supplierLocation,
            jobReferenceNumber: this.calendarSearchData.jobReferenceNumber
        }, true).then(response => {
            if (this.calendarSearchData.jobReferenceNumber != "" && this.calendarSearchData.jobReferenceNumber !== undefined && this.calendarSearchData.jobReferenceNumber !== null) {
                const getEvent = response.events.filter(x => x.title == this.calendarSearchData.jobReferenceNumber);
                response.events = [];
                response.events.push(...getEvent);
                const getResourceId = getEvent.map(y => y.resourceId);
                const listResource = response.resources.filter(x => getResourceId.includes(x.id));
                response.resources = [];
                response.resources.push(...listResource);
                this.setState({
                    calendarData: response,
                    isCalendarDataUpdated: true,
                    calendarStart: response.events.length > 0 ? response.events[0].start : ""
                });
            }
            else {
                //Setting up data to bind in scheduler
                this.setState({
                    calendarData: response,
                    isCalendarDataUpdated: true
                });
            }
        });
    }

    //Calendar select Date
    calendarOnSelectDate = (schedularData) => {
        this.calendarStartDate = schedularData.startDate;
        this.calendarEndDate = schedularData.endDate;

        this.props.actions.FetchCalendarData({
            companyCode: this.props.selectedCompanyCode,
            isActive: true,
            startDateTime: schedularData.startDate,
            endDateTime: schedularData.endDate,
            resourceName: this.calendarSearchData.resourceName,
            customerName: this.calendarSearchData.customerName,
            supplierName: this.calendarSearchData.supplierName,
            supplierLocation: this.calendarSearchData.supplierLocation,
            jobReferenceNumber: this.calendarSearchData.jobReferenceNumber
        }, true).then(response => {
            //Setting up data to bind in scheduler
            this.setState({
                calendarData: response,
                isCalendarDataUpdated: true
            });
        });
    }

    //Pagination
    pagenationClick = (event) => {
        this.paginationText = Number(event.target.value) > this.pageNumbers.length ? this.pageNumbers.length : Number(event.target.value);
        this.setState({
            currentPage: this.paginationText,
            isCalendarDataUpdated: true
        });
    }

    //Pagination
    pagenationNextClick = (event) => {
        this.paginationText = ((Number(this.paginationText) + 1) > this.pageNumbers.length) ? this.pageNumbers.length : (Number(this.paginationText) + 1);
        this.setState({
            currentPage: this.paginationText,
            isCalendarDataUpdated: true
        });
    }

    //Pagination
    pagenationPreiousClick = (event) => {
        this.paginationText = ((Number(this.paginationText) - 1) <= 0) ? 1 : (Number(this.paginationText) - 1);
        this.setState({
            currentPage: this.paginationText,
            isCalendarDataUpdated: true
        });
    }

    render() {
        const { calendarData, currentPage, todosPerPage } = this.state;

        // Logic for displaying current todos
        const indexOfLastTodo = currentPage * todosPerPage;
        const indexOfFirstTodo = indexOfLastTodo - todosPerPage;

        // Logic for displaying page numbers
        this.pageNumbers = [];
        let calendarObj = {
            resources: [],
            events: []
        };
        if (calendarData && calendarData.resources.length > 0) {
            for (let i = 1; i <= Math.ceil(calendarData.resources.length / todosPerPage); i++) {
                this.pageNumbers.push(i);
            }
            calendarObj = Object.assign({},calendarData);
            calendarObj.resources = calendarObj.resources.slice(indexOfFirstTodo, indexOfLastTodo);
        }

        return (
            <Fragment>
                {
                    this.state.isOpenCalendarPopUp ?
                        <Modal title={localConstant.globalCalendar.COMPANY_CALENDAR}
                            modalId="calendarPopup"
                            formId="calendarForm"
                            buttons={this.state.isEditTaskMode ? this.calendarEditButtons : this.calendarViewButtons}
                            isShowModal={true}>
                            {this.state.templateType !== localConstant.globalCalendar.CALENDAR_STATUS.PTO ? <CalendarTaskInfoView PopupData={this.calendarPopup} TemplateType={this.state.templateType} eventData={this.state.calendarTaskData} />
                                : <CalendarPTOTaskInfoView PopupData={this.calendarPopup} TemplateType={this.state.templateType} eventData={this.state.calendarTaskData} />}
                        </Modal> : null
                }
                <div className="customCard globalCalender">
                    <CalendarSearchView calendarSearchData={this.calendarSearchData} searchInputHandler={this.searchInputHandler} calendarSearchClick={this.calendarSearchClick} />
                    {this.pageNumbers && this.pageNumbers.length > 0 ?
                        <div className="row custom_pagination_row">
                            <div className="col s12 custom_pagination">
                                <a onClick={this.pagenationPreiousClick} className="link mr-3 left mt-2"><i className="zmdi zmdi-chevron-left zmdi-hc-lg"></i></a>
                                <CustomInput
                                    hasLabel={true}
                                    name="paginationText"
                                    colSize='browser-default center-align mr-1 pagerInput validate pl-0 pr-0'
                                    type='text'
                                    dataType='numeric'
                                    valueType='value'
                                    inputClass="customInputs"
                                    maxLength={50}
                                    onValueChange={this.pagenationClick}
                                    value={this.paginationText}                                 
                                />
                                <span className='ml-2 left mt-2'>  of {this.pageNumbers ? this.pageNumbers.length : 0}</span>
                                <a onClick={this.pagenationNextClick} className="link ml-2 left mt-2"><i className="zmdi zmdi-chevron-right zmdi-hc-lg"></i></a>
                            </div>
                        </div> : null
                    }

                    <ResourceScheduler schedulerData={calendarObj}
                        eventItemClick={this.eventItemClick}
                        eventItemPopoverTemplateResolver={this.eventItemPopoverTemplateResolver}
                        views={schedulerConfigData.filterViews}
                        isCalendarDataUpdated={this.state.isCalendarDataUpdated}
                        isDisabled={false}
                        nextClick={this.calendarNextClick}
                        prevClick={this.calendarPrevClick}
                        onViewChange={this.calendarOnViewChange}
                        onSelectDate={this.calendarOnSelectDate}
                        eventItemPopoverEnabled={false}
                        calendarStartDate={this.state.calendarStart?this.state.calendarStart:""}
                    />
                </div>
            </Fragment>
        );
    }
}

export default GlobalCalendar;
