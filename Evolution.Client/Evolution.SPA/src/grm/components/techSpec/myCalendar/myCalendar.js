import React, { Component, Fragment } from 'react';
import { getlocalizeData } from '../../../../utils/commonUtils';
import ResourceCalendar from '../../../../components/applicationComponents/calendar';
import Modal from '../../../../common/baseComponents/modal';
import moment from 'moment';
import { CalendarTaskInfoView, CalendarPTOTaskInfoView } from '../../applicationComponent/calendarPopupTemplate/calendarTaskInfoView';

const localConstant = getlocalizeData();

// //Calendar Info Popup Template
// export const CalendarTaskInfoView = (props) => {
//     const { PopupData, TemplateType } = props;
//     return (
//         <div style={{ 'max-height': '450px', 'overflow': 'auto' }}>
//             <div class='row'>
//                 <div className='col s4 bold mb-1'>Project Number</div>
//                 <div className='col s8 mb-1'>{(((TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE) ? PopupData.searchParameter.projectNumber : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitProjectNumber : PopupData.timesheetProjectNumber) !== "") &&
//                     (((TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE) ? PopupData.searchParameter.projectNumber : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitProjectNumber : PopupData.timesheetProjectNumber) !== null) ?
//                     ((TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE) ? PopupData.searchParameter.projectNumber : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitProjectNumber : PopupData.timesheetProjectNumber) : "-"}</div>

//                 <div className='col s4 bold mb-1'>Assignment Number</div>
//                 <div className='col s8 mb-1'>{(TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.assignmentNumber : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitAssignmentNumber : PopupData.timesheetAssignmentNumber) !== "" &&
//                     (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.assignmentNumber : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitAssignmentNumber : PopupData.timesheetAssignmentNumber) !== null ?
//                     (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.assignmentNumber : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitAssignmentNumber : PopupData.timesheetAssignmentNumber) : "-"}</div>

//                 <div className='col s4 bold mb-1'>{(TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? "Visit Number" : "Timesheet Number"}</div>
//                 <div className='col s8 mb-1'>{(TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? "" : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitNumber : PopupData.timesheetNumber) !== "" &&
//                     (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? "" : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitNumber : PopupData.timesheetNumber) !== null ?
//                     (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? "" : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitNumber : PopupData.timesheetNumber) : "-"}</div>

//                 <div className='col s4 bold mb-1'>Customer</div>
//                 <div className='col s8 mb-1'>{(TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.customerName : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitCustomerName : PopupData.timesheetCustomerName) !== "" &&
//                     (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.customerName : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitCustomerName : PopupData.timesheetCustomerName) !== null ?
//                     (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.customerName : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitCustomerName : PopupData.timesheetCustomerName) : "-"}</div>

//                 <div className='col s4 bold mb-1'> Supplier </div>
//                 <div className='col s8 mb-1'>{(TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.supplier : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitSupplier : "") !== "" &&
//                     (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.supplier : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitSupplier : "") !== null ?
//                     (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.supplier : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitSupplier : "") : "-"}</div>

//                 <div className='col s4 bold mb-1'>Supplier Location</div>
//                 <div class='col s8 mb-1'>{(TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.supplierLocation : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.supplierLocation : "-") !== "" &&
//                     (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.supplierLocation : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.supplierLocation : "-") !== null ?
//                     (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.supplierLocation : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.supplierLocation : "-") : "-"}</div>

//                 <div className='col s4 bold mb-1'>Supplier Po</div>
//                 <div className='col s8 mb-1'>{(TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? "" : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitSupplierPONumber : "-") !== "" &&
//                     (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? "" : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitSupplierPONumber : "-") !== null ?
//                     (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? "" : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitSupplierPONumber : "-") : "-"}</div>

//                 <div className='col s4 bold mb-1'>Description</div>
//                 <div className='col s8 mb-1'>{(TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? "" : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitMaterialDescription : "") !== "" &&
//                     (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? "" : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitMaterialDescription : "") !== null ?
//                     (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.PRE ? PopupData.searchParameter.materialDescription : (TemplateType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) ? PopupData.visitMaterialDescription : PopupData.timesheetDescription) : "-"}</div>
//             </div>
//         </div>
//     );
// };

// //Calendar PTO Info Popup Template
// export const CalendarPTOTaskInfoView = (props) => {
//     const { PopupData, TemplateType } = props;
//     return (
//         <div style={{ 'max-height': '450px', 'overflow': 'auto' }}>
//             <div className="row">
//                 <div className='col s4 bold mb-1'>Leave Type</div>
//                 <div className='col s8 mb-1'>{(PopupData.leaveCategoryType !== "" && PopupData.leaveCategoryType !== null) ? PopupData.leaveCategoryType : "-"}</div>
//                 <div className='col s4 bold mb-1'>Leave Description</div>
//                 <div className='col s8 mb-1'>{(PopupData.comments !== "" && PopupData.comments !== null) ? PopupData.comments : "-"}</div>
//             </div>
//         </div>
//     );
// };

class MyCalendar extends Component {

    constructor(props) {
        super(props);
        this.state = {
            calendarEvents: [],
            isOpenCalendarPopUp: false,
            templateType: "",
            calendarTaskData:{}
        };

        this.calendarPopup = [];

        this.calendarViewButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelCalendarPopup,
                btnID: "closeErrorList",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
            }
        ];

        this.onSelectEvent = this.onSelectEvent.bind(this);
    }

    componentDidMount() {
        this.props.actions.FetchCalendarData({
            companyCode: this.props.selectedCompanyCode,
            isActive: true,
            resourceName: this.props.userLogonName
        }, false).then(response => {
            //Setting up data to bind in scheduler
            if (response) {
                const eventDateArray = [];
                if (response.events && Array.isArray(response.events)) {
                    response.events.map(event => {
                        event.start = new Date(event.start);
                        event.end = new Date(event.end);
                    });
                    this.setState({
                        calendarEvents: response.events ? response.events : [],
                    });
                }
            }
        });
    }

    onSelectEvent = (eventItem) => {
        if (eventItem.calendarType === localConstant.globalCalendar.CALENDAR_STATUS.VISIT) {
            this.props.actions.FetchVisitByID(eventItem.calendarRefCode).then(response => {
                this.calendarPopup = response;
                if (response && response !== null) {
                    this.setState({
                        isOpenCalendarPopUp: true,
                        templateType: localConstant.globalCalendar.CALENDAR_STATUS.VISIT,
                        calendarTaskData:eventItem
                    });
                }
            });
        }
        else if (eventItem.calendarType === localConstant.globalCalendar.CALENDAR_STATUS.TIMESHEET) {
            this.props.actions.FetchTimesheetGeneralDetail(eventItem.calendarRefCode).then(response => {
                this.calendarPopup = response;
                if (response && response !== null) {
                    this.setState({
                        isOpenCalendarPopUp: true,
                        templateType: localConstant.globalCalendar.CALENDAR_STATUS.TIMESHEET,
                        calendarTaskData:eventItem
                    });
                }
            });
        }
        else if (eventItem.calendarType === localConstant.globalCalendar.CALENDAR_STATUS.PRE) {
            this.props.actions.FetchPreAssignment(eventItem.calendarRefCode).then(response => {
                this.calendarPopup = response;
                if (response && response !== null) {
                    this.setState({
                        isOpenCalendarPopUp: true,
                        templateType: localConstant.globalCalendar.CALENDAR_STATUS.PRE,
                        calendarTaskData:eventItem
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
                            isCalendarDataUpdated: false,
                            calendarTaskData:eventItem
                        });
                    }
                }
            });
        }
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

    render() {
        const { calendarEvents } = this.state;
        return (
            <Fragment>

                {
                    this.state.isOpenCalendarPopUp ?
                        <Modal title={localConstant.myCalendar.MY_CALENDAR}
                            modalId="calendarPopup"
                            formId="calendarForm"
                            buttons={this.calendarViewButtons}
                            isShowModal={true}>
                            {
                                (this.state.templateType !== localConstant.globalCalendar.CALENDAR_STATUS.PTO) ?
                                    <CalendarTaskInfoView PopupData={this.calendarPopup} TemplateType={this.state.templateType} eventData={this.state.calendarTaskData} />
                                    : <CalendarPTOTaskInfoView PopupData={this.calendarPopup} TemplateType={this.state.templateType} eventData={this.state.calendarTaskData}/>
                            }
                        </Modal> : null
                }

                <div className="customCard">
                    <ResourceCalendar calendarEventList={calendarEvents} onSelectEvent={this.onSelectEvent} />
                </div>
                <div className={'dashboard customCard row'}>
                        <div>
                            <div className="col s12 m2"><div className="col s1 rectangle" style={{ background: "#C00000" }}></div><div className="col s9"><label className="bold">Confirmed</label></div></div>
                            <div className="col s12 m2"><div className="col s1 rectangle" style={{ background: "#FFC700" }}></div><div className="col s9"><label className="bold">Tentative</label></div></div>
                            <div className="col s12 m2"><div className="col s1 rectangle" style={{ background: "#A3A6A9" }}></div><div className="col s9"><label className="bold">TBA</label></div></div>
                            <div className="col s12 m2"><div className="col s1 rectangle" style={{ background: "#21B6D7" }}></div><div className="col s9"><label className="bold">PTO</label></div></div>
                            <div className="col s12 m2"><div className="col s1 rectangle" style={{ background: "#474E54" }}></div><div className="col s9"><label className="bold">Pre-Assignment</label></div></div>
                        </div>
                    </div>
            </Fragment>
        );
    }
}

export default MyCalendar;
