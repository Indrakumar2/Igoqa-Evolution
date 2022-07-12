import { getlocalizeData } from '../../../../utils/commonUtils';
import moment from 'moment';
import dateUtil from '../../../../utils/dateUtil';
import arrayUtil from '../../../../utils/arrayUtil';

const localConstant = getlocalizeData();

export const HeaderData={
    "columnDefs":[
        // {
        //     "headerName": localConstant.gridHeader.TIMESHEET_NUMBER,
        //     "field": "timesheetNumber",
        //     "cellRenderer": "TimesheetAnchor",
        //     "filter": "agNumberColumnFilter",
        //     "filterParams": {
        //         "inRangeInclusive": true
        //     },
        //     "headerTooltip": localConstant.gridHeader.TIMESHEET_NUMBER,
        //     "tooltipField":localConstant.gridHeader.TIMESHEET_NUMBER,
        //     "width": 180
        // },
        {
            "headerName":localConstant.gridHeader.JOB_REFERENCE_NUMBER,
            "headerTooltip": localConstant.gridHeader.JOB_REFERENCE_NUMBER,
            "field":"timesheetJobReference",
            "tooltipField": "timesheetJobReference",
            "filter":"agTextColumnFilter",
            "cellRenderer": "TimesheetAnchor",
            "cellRendererParams": (params) => {
                return { 
                    displayLinkText:params.data.timesheetJobReference   
                 };
            },
            "width":180
        },
        {
            "headerName":localConstant.gridHeader.TIMESHEET_DATE,
            "headerTooltip": localConstant.gridHeader.TIMESHEET_DATE,
            "field":"timesheetStartDate",
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "timesheetStartDate"
            },
            "filter": "agDateColumnFilter",           
            "width":180,
            "tooltip": (params) => {
                return moment(params.data.timesheetStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
               
            },
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
            }  
        },            
        {
            "headerName":localConstant.gridHeader.TIMESHEET_DESCRIPTION,
            "headerTooltip": localConstant.gridHeader.TIMESHEET_DESCRIPTION,
            "field":"timesheetDescription",
            "filter":"agTextColumnFilter",            
            "width":450
        },
        // {
        //     "headerName":localConstant.gridHeader.TECHNICAL_SPECIALISTS,
        //     "headerTooltip": localConstant.gridHeader.TECHNICAL_SPECIALISTS,
        //     "field":"technicalSpecialist",
        //     "filter":"agTextColumnFilter" ,
        //     "width":250
        // },
        {
            "headerName":  localConstant.gridHeader.TECHNICAL_SPECIALISTS,
            "field": "techSpecialists",
            "tooltip": (params) => {
                const { techSpecialists } = params.data;
                if (techSpecialists) {
                    return techSpecialists.filter(ts => ts.fullName !== ' ').map(e => e.fullName).join(",");
                } else {
                    return "";
                }
            },
            "headerTooltip": localConstant.gridHeader.TECHNICAL_SPECIALISTS,
            "filter": "agTextColumnFilter",
            "valueGetter": (params) => {
                const { techSpecialists } = params.data;
                if (techSpecialists) {
                    return techSpecialists.filter(ts => ts.fullName !== ' ').map(e => e.fullName).join(",");
                } else {
                    return "";
                }
            },
            "width": 200
        },
        {
            "headerName": localConstant.gridHeader.STATUS,
            "field": "timesheetStatus",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.STATUS,
            "sort": "asc", 
            "valueGetter": (params) => {
                const statusObj =  arrayUtil.FilterGetObject(localConstant.commonConstants.timesheet_Status,
                    'value',params.data.timesheetStatus);
                return statusObj ? statusObj.name : '';
            },
            "tooltip": (params) => {
                const statusObj =  arrayUtil.FilterGetObject(localConstant.commonConstants.timesheet_Status,
                    'value',params.data.timesheetStatus);
                return statusObj ? statusObj.name : '';
            },
            "width":200
        }                        
    ],
    "enableFilter":true, 
    "enableSorting":true, 
    "pagination": true,
    "gridHeight":60,
    "searchable":true,
    "gridActions":true,
    "gridTitlePanel":true,
    "clearFilter":true, 
    "gridRefresh":true,
    "exportFileName":"Assignment Timesheet's Data",
    "columnsFiltersToDestroy":[ 'timesheetStartDate' ]
};