import { getlocalizeData } from '../../../../utils/commonUtils';
import moment from 'moment';
const localConstant = getlocalizeData();

export const HeaderData = {
    "columnDefs": [        
        {
            "headerName": localConstant.gridHeader.DATE,
            "headerTooltip": localConstant.gridHeader.DATE,
            "field": "CreatedOn",
            "tooltip": (params) => {
                return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filter": "agDateColumnFilter",
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "createdOn"
            },
            "width": 120,
            "sort": "desc"
        },
        {
            "headerName":  localConstant.gridHeader.USER,
            "headerTooltip": localConstant.gridHeader.USER,
            "field": "userDisplayName",
            "tooltipField": "userDisplayName",
            "filter": "agTextColumnFilter",
            "width":120
        },
        {
            "headerName":  localConstant.gridHeader.NOTE,
            "headerTooltip": localConstant.gridHeader.NOTE,
            "field": "note",
            "tooltipField": "note",
            "filter": "agTextColumnFilter",
            "width":455
        },
        {
            "headerName":  localConstant.timesheet.CUSTOMER_VISIBLE,
            "headerTooltip": localConstant.timesheet.CUSTOMER_VISIBLE,
            "field": "isCustomerVisible",
            "tooltip": (params) => {
                if (params.data.isCustomerVisible === true) {
                    return "Yes";
                } else {
                    return "No";
                }
            },
            "filter": "agTextColumnFilter",
            "valueGetter": (params) => {
                if (params.data.isCustomerVisible === true) {
                    return "Yes";
                } else {
                    return "No";
                }
            },
            "width":135
        },
        {
            "headerName":  localConstant.visit.RESOURCE_VISIBLE,
            "headerTooltip": localConstant.visit.RESOURCE_VISIBLE,
            "field": "isSpecialistVisible",
            "tooltip": (params) => {
                if (params.data.isSpecialistVisible === true) {
                    return "Yes";
                } else {
                    return "No";
                }
            },
            "filter": "agTextColumnFilter",
            "valueGetter": (params) => {
                if (params.data.isSpecialistVisible === true) {
                    return "Yes";
                } else {
                    return "No";
                }
            },
            "width":135
        },
        {
            "headerName": "",
            "field": "ViewTimesheetNotes",
            "cellRenderer": "EditLink",
            "cellRendererParams": {
                "displayLink":"Edit/View" //D661 issue8
            },
            "suppressFilter": true,
            "suppressSorting": true,
            "width": 80
        }
    ],
    "enableSorting": true,
    "pagination": true,
    "gridHeight": 55,
    "enableFilter": true,
    "clearFilter":true, 
};