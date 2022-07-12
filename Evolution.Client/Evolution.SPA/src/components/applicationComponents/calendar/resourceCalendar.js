import React, { Component, Fragment } from 'react';
import { Calendar, momentLocalizer } from 'react-big-calendar';
import 'react-big-calendar/lib/sass/styles.scss';
import moment from 'moment';
moment.locale('en', {
    week: {
        dow: 1,
        doy: 1,
    },
});

const localizer = momentLocalizer(moment);

const myEventsList = [
    {
        bgColor: "#cc4141",
        calendarRefCode: 2325966,
        calendarStatus: "Confirmed",
        calendarType: "VISIT",
        companyCode: "UK050",
        companyId: 116,
        createdBy: "jenna.blyth",
        end: new Date("2019-06-15T09:00:00"),
        id: 10062,
        isActive: true,
        recordStatus: null,
        resizable: true,
        resourceId: 43842,
        start: new Date("2019-06-14T02:00:00"),
        title: "Confirmed",
        updateCount: 1,
    },
    {
        bgColor: "#d3c954",
        calendarRefCode: 2325966,
        calendarStatus: "Confirmed",
        calendarType: "VISIT",
        companyCode: "UK050",
        companyId: 116,
        createdBy: "jenna.blyth",
        end: new Date("2019-06-19T09:00:00"),
        id: 10062,
        isActive: true,
        recordStatus: null,
        resizable: true,
        resourceId: 43842,
        start: new Date("2019-06-17T02:00:00"),
        title: "Tentative",
        updateCount: 1,
    }
];

class ResourceCalendar extends Component {

    constructor(props) {
        super(props);
        this.defaultOnSelectEvent = this.defaultOnSelectEvent.bind(this);
        this.defaultEventStyleGetter = this.defaultEventStyleGetter.bind(this);
    }

    defaultOnSelectEvent = (event) => {
        if (this.props.onSelectEvent)
            this.props.onSelectEvent(event);
    }

    defaultEventStyleGetter = (event, start, end, isSelected) => {
        if (this.props.eventStyleGetter)
            this.props.eventStyleGetter(event, start, end, isSelected);
        else {
            const style = {
                backgroundColor: event.bgColor
            };
            return {
                style: style
            };
        }
    };

    render() {
        const { calendarEventList } = this.props;

        return (
            <Fragment>
                <div style={{ "height": 600 }}>
                    <Calendar
                        selectable
                        localizer={localizer}
                        events={calendarEventList}
                        startAccessor="start"
                        endAccessor="end"
                        onSelectEvent={this.defaultOnSelectEvent}
                        eventPropGetter={(this.defaultEventStyleGetter)}
                        views={{ month: true, week: true, day: true }}
                    />
                </div>
            </Fragment>
        );
    }
}

export default ResourceCalendar;