import { isUndefined,getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData = {

    "columnDefs": [
       
        {
            "headerName": localConstant.visit.DATE,
            "headerTooltip": localConstant.visit.DATE,
            "field": "createdOn",
            "tooltip": (params) => {
                return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
            }, 
            "filter": "agDateColumnFilter",
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "createdOn"
            },
            "width":120
        },
        {
            "headerName":  localConstant.visit.USER,
            "headerTooltip": localConstant.visit.USER,
            "field": "userDisplayName",
            "tooltipField": "userDisplayName",   
            "filter": "agTextColumnFilter",
            "width":150
        },
        {
            "headerName":  localConstant.visit.NOTE,
            "headerTooltip": localConstant.visit.NOTE,
            "field": "note",
            "tooltipField":"note",    
            "filter": "agTextColumnFilter",
            "width":350
        },
        {
            "headerName":  localConstant.visit.CUSTOMER_VISIBLE,
            "headerTooltip": localConstant.visit.CUSTOMER_VISIBLE,
            "field": "visibleToCustomer",
            "tooltip": (params) => {
                return params.data.visibleToCustomer ? "Yes" : "No";
            },   
            "filter": "agTextColumnFilter",
            "width":150,
            "valueGetter": (params) => {
                return params.data.visibleToCustomer ? "Yes" : "No";
            },
        },
        {
            "headerName":  localConstant.visit.RESOURCE_VISIBLE,
            "headerTooltip": localConstant.visit.RESOURCE_VISIBLE,
            "field": "visibleToSpecialist",
            "tooltip":(params) => {
                return params.data.visibleToSpecialist ? "Yes" : "No";
            },
            "filter": "agTextColumnFilter",
            "width":150,
            "valueGetter": (params) => {
                return params.data.visibleToSpecialist ? "Yes" : "No";
            },
        },
        {
            "headerName": "",
            "field": "ViewVisitNotes",
            "cellRenderer": "EditLink",
            "cellRendererParams": {
                "displayLink":"Edit/View" //D661 issue8
            },
            "suppressFilter": true,
            "suppressSorting": true,
            "width": 75
        }
    ],
    
    "enableSorting": true,
    "pagination": true,
    "gridHeight": 60,
    "enableFilter": true,
    "clearFilter":true, 
};