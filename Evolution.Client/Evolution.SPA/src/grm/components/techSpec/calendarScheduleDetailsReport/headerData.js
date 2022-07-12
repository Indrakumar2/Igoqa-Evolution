import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();

export const HeaderData = (properties) => {
    return {
   
            "columnDefs": [
                 {
                    "headerName": localConstant.gridHeader.COMPANY,
                     "field": "company",
                     "filter": "agTextColumnFilter",
                     "headerTooltip": localConstant.gridHeader.COMPANY,
                     "tooltipField": "company",
                     "width": 250
                  },
                 {
                  "headerName": localConstant.gridHeader.SUB_DIVISION,
                   "field": "subDivision",
                   "filter": "agTextColumnFilter",
                   "width": 200,
                   "headerTooltip": localConstant.gridHeader.SUB_DIVISION,
                   "tooltipField": "id",

                },
                  {
                    "headerName": localConstant.gridHeader.CH_COORDINATOR, 
                    "field": "chCoordinator",
                    "filter": "agTextColumnFilter",
                    "headerTooltip": localConstant.gridHeader.CH_COORDINATOR, 
                    "tooltipField": "chCoordinator",
                    "width": 200,
                  },
                  {
                    "headerName": localConstant.gridHeader.OC_COORDINATOR, 
                    "field": "ocCoordinator",
                    "filter": "agTextColumnFilter",
                    "headerTooltip": localConstant.gridHeader.OC_COORDINATOR, 
                    "tooltipField": "ocCoordinator",
                    "width": 200,
                  },
                  {
                    "headerName": localConstant.gridHeader.CUSTOMER, 
                    "headerTooltip": localConstant.gridHeader.CUSTOMER, 
                    "field": "customerName",
                    "filter": "agTextColumnFilter",
                    "tooltipField": "customerName",
                    "width": 200,
                  } ,             
                  {
                    "headerName": localConstant.gridHeader.SUPPLIER, 
                    "headerTooltip": localConstant.gridHeader.SUPPLIER, 
                    "field": "supplierName",
                    "filter": "agTextColumnFilter",
                    "tooltipField": "supplierName",
                    "width": 200,
                  } ,
                  {
                    "headerName": localConstant.gridHeader.LOCATION, 
                    "headerTooltip": localConstant.gridHeader.LOCATION, 
                    "field": "supplierLocation",
                    "filter": "agTextColumnFilter",
                    "tooltipField":"supplierLocation",
                    "width": 200,
                  } ,
                  {
                    "headerName": localConstant.gridHeader.EVO_NO, 
                    "headerTooltip":localConstant.gridHeader.EVO_NO,
                    "field": "evoNo",
                    "filter": "agTextColumnFilter",
                    "tooltipField":"evoNo",
                    "width": 200,
                  } ,
                  {
                    "headerName": localConstant.gridHeader.RESOURCE, 
                    "headerTooltip":localConstant.gridHeader.RESOURCE,
                    "field": "resourceName",
                    "filter": "agTextColumnFilter",
                    "tooltipField": "resourceName",
                    "width": 200,
                  } ,
                  {
                    "headerName": localConstant.gridHeader.EPIN, 
                    "headerTooltip": localConstant.gridHeader.EPIN, 
                    "field": "epin",
                    "filter": "agNumberColumnFilter",
                    "tooltipField":"epin",
                    "width": 150,
                  }  ,
                  {
                    "headerName": localConstant.gridHeader.EMPLOYMENT_TYPE, 
                    "headerTooltip": localConstant.gridHeader.EMPLOYMENT_TYPE, 
                    "field": "employmentType",
                    "filter": "agTextColumnFilter",
                    "tooltipField":"employmentType",
                    "width": 200,
                  }  ,
                  {
                    "headerName": localConstant.gridHeader.VISIT_TIMESHEET_START_DATE, 
                    "headerTooltip": localConstant.gridHeader.VISIT_TIMESHEET_START_DATE, 
                    "field": "fromDate",
                    "filter": "agDateColumnFilter",
                    "tooltip": (params) => {
                      return moment(params.data.fromDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                    },
                    "width": 200,
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "fromDate"
                    },
                    "filterParams": {
                      "comparator": dateUtil.comparator,
                      "inRangeInclusive": true,
                      "browserDatePicker": true
                  }
                  }  ,
                  {
                    "headerName": localConstant.gridHeader.VISIT_TIMESHEET_END_DATE, 
                    "headerTooltip": localConstant.gridHeader.VISIT_TIMESHEET_END_DATE, 
                    "field": "toDate",
                    "filter": "agDateColumnFilter",
                    "tooltip": (params) => {
                      return moment(params.data.toDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                    },
                    "width": 200,
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "toDate"
                    },
                    "filterParams": {
                      "comparator": dateUtil.comparator,
                      "inRangeInclusive": true,
                      "browserDatePicker": true
                  }
                  }  ,
                  {
                    "headerName": localConstant.gridHeader.CALENDAR_DATE,
                    "headerTooltip": localConstant.gridHeader.CALENDAR_DATE,
                    "field": "actualDate",
                    "filter": "agDateColumnFilter",
                    "tooltip": (params) => {
                      return moment(params.data.actualDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                    },
                    "width":220,
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "actualDate"
                    },
                    "filterParams": {
                      "comparator": dateUtil.comparator,
                      "inRangeInclusive": true,
                      "browserDatePicker": true
                  }
                },
                  {
                    "headerName": localConstant.gridHeader.VISIT_TIMESHEET_STATUS, 
                    "headerTooltip": localConstant.gridHeader.VISIT_TIMESHEET_STATUS, 
                    "field": "visitTimesheetStatus",
                    "filter": "agTextColumnFilter",
                    "tooltipField":"visitTimesheetStatus",
                    "width": 200,
                  }  ,
                  {
                    "headerName": localConstant.gridHeader.RESOURCE_STATUS, 
                    "headerTooltip":localConstant.gridHeader.RESOURCE_STATUS,
                    "field": "employmentStatus",
                    "filter": "agTextColumnFilter",
                    "tooltipField": "employmentStatus",
                    "width": 200,
                  }  ,
                  {
                    "headerName": localConstant.gridHeader.TIME, 
                    "headerTooltip":localConstant.gridHeader.TIME, 
                    "field": "allocatedTime",
                    "filter": "agTextColumnFilter",
                    "tooltipField":"allocatedTime",
                    "width": 150,
                  }  ,
                  {
                    "headerName": localConstant.gridHeader.NOTES,
                    "headerTooltip": localConstant.gridHeader.NOTES,
                    "field": "notes",
                    "filter": "agTextColumnFilter",
                    "tooltipField":"notes",
                    "width": 200,
                  }  ,
                  {
                    "headerName": localConstant.gridHeader.ASSIGNMENT_CREATION_DATE,
                    "headerTooltip": localConstant.gridHeader.ASSIGNMENT_CREATION_DATE,
                    "field": "assignmentCreationDate",
                    "filter": "agDateColumnFilter",
                    "tooltip": (params) => {
                      return moment(params.data.assignmentCreationDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                    },
                    "width": 200,
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "assignmentCreationDate"
                    },
                    "filterParams": {
                      "comparator": dateUtil.comparator,
                      "inRangeInclusive": true,
                      "browserDatePicker": true
                  }
                  }  ,
                  
            ],
          
            "enableFilter": true,
            "enableSorting": true,
            "pagination": true,
            "gridTitlePanel": true,
            "clearFilter":true, 
            "gridActions": true,
            "searchable":true,
            "exportFileName":"Calendar / Schedule Details Report",
            "customHeader":"Calendar / Schedule Details Report",
            "gridHeight": 25,
        };
        
    };

export const supplierSearchHeader = {
  "columnDefs": [
    {
        "headerCheckboxSelectionFilteredOnly": true,
        "checkboxSelection": true,
        "suppressFilter": true,
        "width": 40
    },
    {
        "headerName": localConstant.gridHeader.NAME,
        "field": "supplierName",
        "headerTooltip":localConstant.gridHeader.NAME,
        "tooltipField": "supplierName",
        "filter": "agTextColumnFilter",
        "width": 170
    },
    {
        "headerName": localConstant.gridHeader.COUNTRY,
        "field": "country",
        "headerTooltip":localConstant.gridHeader.COUNTRY,
        "tooltipField": "country",
        "filter": "agTextColumnFilter",
        "width": 100
    },
    {
        "headerName": localConstant.gridHeader.COUNTY,
        "field": "state",
        "headerTooltip":localConstant.gridHeader.COUNTY,
        "tooltipField": "state",
        "filter": "agTextColumnFilter",
        "width": 100
    },
    {
        "headerName": localConstant.gridHeader.CITY,
        "field": "city",
        "headerTooltip":localConstant.gridHeader.CITY,
        "tooltipField": "city",
        "filter": "agTextColumnFilter",
        "width": 100
    },
    {
        "headerName": localConstant.gridHeader.FULL_ADDRESS,
        "field": "supplierAddress",
        "headerTooltip":localConstant.gridHeader.FULL_ADDRESS,
        "tooltipField": "supplierAddress",
        "filter": "agTextColumnFilter",
        "width": 210
    },
],
"rowSelection": 'single',       
"enableSorting": true,
"gridHeight": 50,
"pagination": true,
"enableFilter": true,  
"gridTitlePanel": true,
"clearFilter":true, 
"searchable": true,      
};