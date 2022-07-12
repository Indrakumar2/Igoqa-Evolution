import { ViewTypes } from 'react-big-scheduler';

export const schedulerConfigData = {
    localeMoment: 'en',
    schedulerMaxHeight: 400,
    schedulerWidth: '93%',
    nonWorkingTimeHeadBgColor: 'white',
    nonWorkingTimeBodyBgColor: 'white',
    eventItemPopoverEnabled: false,
    addMorePopoverHeaderFormat: 'MMM D, YYYY dddd',
    eventItemPopoverDateFormat: 'MMM D',
    nonAgendaDayCellHeaderFormat: 'ha',
    nonAgendaOtherCellHeaderFormat: 'ddd DD-MMM',

    //Default view types
    views: [
        { viewName: 'Day', viewType: ViewTypes.Day },
        { viewName: 'Week', viewType: ViewTypes.Week },
        { viewName: 'Month', viewType: ViewTypes.Month },
        { viewName: 'Quarter', viewType: ViewTypes.Quarter, showAgenda: false, isEventPerspective: false },
        { viewName: 'Year', viewType: ViewTypes.Year, showAgenda: false, isEventPerspective: false },
    ],

    //Views to display in calendar
    filterViews: [
        'Day', 'Week', 'Month'
    ]
};