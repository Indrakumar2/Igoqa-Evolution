import React, { Component, Fragment } from 'react';
import { DragDropContext } from "react-dnd";
import HTML5Backend from 'react-dnd-html5-backend';
import Scheduler, { SchedulerData, ViewTypes, DemoData, CellUnits } from 'react-big-scheduler';
//include `react-big-scheduler/lib/css/style.css` for styles, link it in html or import it here
import 'react-big-scheduler/lib/css/style.css';
import { schedulerConfigData } from './schedulerconfig';
import moment from 'moment';
import { getlocalizeData } from '../../../utils/commonUtils';

const localConstant = getlocalizeData();

moment.locale('en', {
    week: {
        dow: 1,
        doy: 1,
    },
});

class ResourceScheduler extends Component {

    constructor(props) {
        super(props);
        const defaultCalendarData = {
            resources: [
                { id: "", name: "" }
            ],
            events: []
        };
       //moment.locale('en'); // to show monday to sunday
        //JSON to Scheduler object
        this.schedulerData = new SchedulerData(new moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT), ViewTypes.Week);
        //this.schedulerData.localeMoment.locale('en');
        this.schedulerData.setLocaleMoment(moment);
        this.schedulerData.setDate((moment(this.schedulerData.selectDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT)));
        this.schedulerData.setResources(defaultCalendarData.resources);
        this.schedulerData.setEvents(defaultCalendarData.events);
        this.schedulerData.config.schedulerMaxHeight = schedulerConfigData.schedulerMaxHeight;
        this.schedulerData.config.schedulerWidth = schedulerConfigData.schedulerWidth;
        this.schedulerData.config.nonWorkingTimeHeadBgColor = schedulerConfigData.nonWorkingTimeHeadBgColor;
        this.schedulerData.config.nonWorkingTimeBodyBgColor = schedulerConfigData.nonWorkingTimeBodyBgColor;
        this.schedulerData.config.eventItemPopoverEnabled = props.eventItemPopoverEnabled;
        this.schedulerData.config.addMorePopoverHeaderFormat = schedulerConfigData.addMorePopoverHeaderFormat;
        this.schedulerData.config.eventItemPopoverDateFormat = schedulerConfigData.eventItemPopoverDateFormat;
        this.schedulerData.config.nonAgendaDayCellHeaderFormat = schedulerConfigData.nonAgendaDayCellHeaderFormat;
        this.schedulerData.config.nonAgendaOtherCellHeaderFormat = schedulerConfigData.nonAgendaOtherCellHeaderFormat;
        this.schedulerData.behaviors.isNonWorkingTimeFunc = this.isNonWorkingTime;

        //Fiter views to display
        let filteredViews = [];
        if (props.views) {
            schedulerConfigData.views.map(view => {
                props.views.forEach(element => {
                    if (view.viewName.toLowerCase() === element.toLowerCase())
                        filteredViews.push(view);
                });
            });
        }
        else {
            filteredViews = schedulerConfigData.views;
        }
        this.schedulerData.config.views = filteredViews;

        this.state = {
            viewModel: this.schedulerData,
           // isDisabled: true
        };
    }

    isNonWorkingTime = (schedulerData, time) => {
        // const { localeMoment } = schedulerData;
        // if (schedulerData.cellUnit === CellUnits.Hour) {
        //     const dayOfWeek = localeMoment(time).weekday();
        //     if (dayOfWeek === 0 || dayOfWeek === 6) {
        //         const hour = localeMoment(time).hour();
        //         return true;
        //     }
        // }
        // else {
        //     const dayOfWeek = localeMoment(time).weekday();
        //     if (dayOfWeek === 0 || dayOfWeek === 6)
        //         return true;
        // }

        return false;
    }

    componentWillReceiveProps(props) {
        //To avoid unwanted render - When scheduler data has changed, pass isCalendarDataUpdated as true.
        //let isDisabled = false;
        if (props.calendarStartDate) {
            this.schedulerData.setDate((moment(props.calendarStartDate).format(localConstant.commonConstants.SAVE_DATE_FORMAT)));
            if (props.schedulerData) {
                this.schedulerData.setResources(props.schedulerData.resources);
                this.schedulerData.setEvents(props.schedulerData.events);
            }
        }
        if (props.isCalendarDataUpdated && props.schedulerData.resources) {
            if (props.schedulerData.resources.length === 0) {
                props.schedulerData.resources = [
                    { id: "", name: "" }
                ];
              //  isDisabled = true;
            }
            this.schedulerData.setResources(props.schedulerData.resources);
            this.schedulerData.setEvents(props.schedulerData.events);

            this.setState({
                viewModel: this.schedulerData,
               // isDisabled: isDisabled
            });
        }
    }

    defaultNextClick = (schedulerData) => {
        schedulerData.next();
        if (this.props.nextClick)
            this.props.nextClick(schedulerData);
        else {
            schedulerData.setEvents(this.props.schedulerData.events);
            this.setState({ viewModel: schedulerData });
        }
    };

    defaultPrevClick = (schedulerData) => {
        schedulerData.prev();

        if (this.props.prevClick)
            this.props.prevClick(schedulerData);
        else {
            schedulerData.setEvents(this.props.schedulerData.events);
            this.setState({ viewModel: schedulerData });
        }
    };

    defaultOnViewChange = (schedulerData, view) => {
        schedulerData.setViewType(view.viewType, view.showAgenda, view.isEventPerspective);

        if (this.props.onViewChange)
            this.props.onViewChange(schedulerData);
        else {
            schedulerData.setEvents(this.props.schedulerData.events);
            this.setState({ viewModel: schedulerData });
        }
    };

    defaultOnSelectDate = (schedulerData, date) => {
        schedulerData.setDate((date=moment(date).format(localConstant.commonConstants.SAVE_DATE_FORMAT)));

        if (this.props.onSelectDate)
            this.props.onSelectDate(schedulerData);
        else {
            schedulerData.setEvents(this.props.schedulerData.events);
            this.setState({ viewModel: schedulerData });
        }
    };

    defaultToggleExpandFunc = (schedulerData, slotId) => {
        schedulerData.toggleExpandStatus(slotId);
        this.setState({ viewModel: schedulerData });

        if (this.props.toggleExpandFunc)
            this.props.toggleExpandFunc(schedulerData);
    };

    defaultEventItemClick = (schedulerData, event) => {
        if (this.props.eventItemClick)
            this.props.eventItemClick(schedulerData, event);
    };

    defaultNewEvent = (schedulerData, slotId, slotName, start, end, type, item) => {
        if (this.props.newEvent)
            this.props.newEvent(schedulerData, slotId, slotName, start, end, type, item);
    };

    defaultEventItemPopoverTemplateResolver = (schedulerData, eventItem, title, start, end, statusColor) => {
        if (this.props.eventItemPopoverTemplateResolver) {
            return this.props.eventItemPopoverTemplateResolver(schedulerData, eventItem, title, start, end, statusColor);
        } else {
            return (
                <div>
                    <p>Default Template</p>
                </div>
            );
        }
    };

    shouldComponentUpdate = (nextProps, nextState) => {
        return nextProps.isCalendarDataUpdated;
    }
    render() {

        return (
            <Fragment>
                <div disabled={this.props.isDisabled ? this.props.isDisabled : false}>
                    <Scheduler
                        schedulerData={this.state.viewModel}
                        prevClick={this.defaultPrevClick}
                        nextClick={this.defaultNextClick}
                        onSelectDate={this.defaultOnSelectDate}
                        onViewChange={this.defaultOnViewChange}
                        eventItemClick={this.defaultEventItemClick}
                        toggleExpandFunc={this.defaultToggleExpandFunc}
                        newEvent={this.defaultNewEvent}
                        eventItemPopoverTemplateResolver={this.defaultEventItemPopoverTemplateResolver}
                    />
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

export default DragDropContext(HTML5Backend)(ResourceScheduler);
