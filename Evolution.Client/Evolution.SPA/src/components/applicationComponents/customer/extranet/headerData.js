import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();

export const HeaderData = {    
    "ExtranetAccountsHeaderData": {
        "columnDefs": [
            // {
            //     "checkboxSelection": true,
            //     "headerCheckboxSelectionFilteredOnly": true,
            //     "suppressFilter": true,
            //     "width": 40
            // },            
            {
                "headerName": localConstant.gridHeader.USER_NAME,
                "field": "LogonName", 
                "filter": "agTextColumnFilter",
                "tooltipField": "LogonName",   
                "headerTooltip": localConstant.gridHeader.USER_NAME,
                "width": 120
            },
            {
                "headerName": localConstant.gridHeader.EMAIL,
                "field": "Email", 
                "filter": "agTextColumnFilter",
                "tooltipField":"Email",   
                "headerTooltip": localConstant.gridHeader.EMAIL,
                "width": 100,
            },
            {
                "headerName": localConstant.gridHeader.LOGIN_TYPE,
                "field": "UserType",
                "filter": "agTextColumnFilter",
                "tooltipField": "UserType",
                "headerTooltip": localConstant.gridHeader.LOGIN_TYPE,
                "width": 150,   
            },
            {
                "headerName": localConstant.gridHeader.CREATION_DATE,
                "field": "CreatedDate",
                "filter": "agDateColumnFilter",
                "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "dataToRender": "CreatedDate"
                },
                "tooltip": (params) => {
                    return params.data.CreatedDate && moment(params.data.CreatedDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                },
                "headerTooltip": localConstant.gridHeader.CREATION_DATE,
                "width": 150,
                "filterParams": {
                    "comparator": dateUtil.comparator,
                    "inRangeInclusive": true,
                    "browserDatePicker": true
                }
            },
            {
                "headerName": localConstant.gridHeader.LAST_LOGIN_DATE,
                "field": "LastLoginDate",
                "filter": "agDateColumnFilter",
                "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "dataToRender": "LastLoginDate"
                },
                "tooltip": (params) => {
                    return params.data.LastLoginDate && moment(params.data.LastLoginDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                },
                "headerTooltip": localConstant.gridHeader.LAST_LOGIN_DATE,
                "width": 150,
                "filterParams": {
                    "comparator": dateUtil.comparator,
                    "inRangeInclusive": true,
                    "browserDatePicker": true
                }
            },
            {
                "headerName": localConstant.gridHeader.LOCKED_OUT,
                "field": "IsAccountLocked",
                "tooltip":(params) => {
                    if (params.data.IsAccountLocked === true) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "valueGetter": (params) => {
                    if (params.data.IsAccountLocked === true) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "headerTooltip": localConstant.gridHeader.LOCKED_OUT,
                "width": 150,
            },
            {
                "headerName": localConstant.gridHeader.ACTIVE,
                "field": "IsActive",
                "tooltip": (params) => {
                    if (params.data.IsActive === true) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "valueGetter": (params) => {
                    if (params.data.IsActive === true) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "headerTooltip": localConstant.gridHeader.ACTIVE,
                "width": 120,
            },
            {
                "headerName": localConstant.gridHeader.SHOW_NEW_VISITS,
                "field": "IsShowNewVisit",
                "tooltips": (params) => {
                    if (params.data.IsShowNewVisit === true) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "valueGetter": (params) => {
                    if (params.data.IsShowNewVisit === true) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "headerTooltip": localConstant.gridHeader.SHOW_NEW_VISITS,
                "width": 160,
            },
            {
                "headerName": "",
                "field": "PortalUserColumn",
                "cellRenderer": "EditLink",
                "buttonAction":"",
                "cellRendererParams": {
                },
                "suppressFilter": true,
                "suppressSorting": true,
                "width": 50
            }, 
            {
                "headerName":"",
                "field": "", 
                "filter": "agTextColumnFilter", 
                "valueGetter": (params) => {
                    if(params.data.CustomerUserProjectNumbers)
                    return params.data.CustomerUserProjectNumbers.map(e => e.ProjectNumber).join(",");
                },  
                "hide" :true,
                "width": 100,
            }, 
        ],
        "enableFilter": true,
        "enableSorting": true,
        "pagination": false,
        "animatedRows":true,
        "rowDragManaged":true,
        "searchable":true,
        "rowSelection": "multiple",
        "gridHeight": 40,
        "clearFilter":true, 
        "exportFileName":"Extranet"
    },
    "CustomerProjects": {
        "columnDefs": [            
            {
                "headerName": localConstant.gridHeader.CUSTOMER_NAME,
                "field": "contractCustomerName",                
                "filter": "agTextColumnFilter",
                "tooltipField": "contractCustomerName",   
                "headerTooltip": localConstant.gridHeader.CUSTOMER_NAME,
                "width": 150
            },
            {
                "headerName": localConstant.gridHeader.CUSTOMER_PROJECT_NAME,
                "field": "customerProjectName",                
                "filter": "agTextColumnFilter",
                "tooltipField": "customerProjectName",   
                "headerTooltip": localConstant.gridHeader.CUSTOMER_PROJECT_NAME,
                "width": 190
            },
            {
                "headerName": localConstant.gridHeader.STATUS,
                "field": "projectStatus",
                "filter": "agTextColumnFilter",
                //"tooltipField": "projectStatus",
                "tooltip": (params) => {
                    if (params.data.projectStatus === "O") {
                        return "Open";
                    } else {
                        return "Close";
                    }
                },
                "headerTooltip": localConstant.gridHeader.STATUS,
                "width": 120,
            },
            {
                "headerName": localConstant.gridHeader.END_DATE,
                "headerTooltip":  localConstant.gridHeader.END_DATE,
                "field":"projectEndDate",
                "tooltip": (params) => {
                    return moment(params.data.projectEndDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
            }, 
                "filter":"agDateColumnFilter",
                "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "dataToRender": "projectEndDate"
                },
                "width":150,
                "filterParams": {
                    "comparator": dateUtil.comparator,
                    "inRangeInclusive": true,
                    "browserDatePicker": true
                  }
            },
            // {
            //     "headerName": localConstant.gridHeader.END_DATE,
            //     "field": "projectEndDate",
            //     "filter": "agTextColumnFilter",
            //     "tooltipField":"projectEndDate",   
            //     "headerTooltip": localConstant.gridHeader.END_DATE,
            //     "width": 120,
            // },
            {
                "headerName": localConstant.gridHeader.MI_DIVISION,
                "field": "companyDivision",
                "filter": "agTextColumnFilter",
                "tooltipField": "companyDivision",
                "headerTooltip": localConstant.gridHeader.MI_DIVISION,
                "width": 150,   
            },
            {
                "headerName": localConstant.gridHeader.MI_OFFICE,
                "field": "companyOffice",
                "filter": "agTextColumnFilter",
                "tooltipField": "companyOffice",
                "headerTooltip": localConstant.gridHeader.MI_OFFICE,
                "width": 150,   
            },
            // {
            //     "headerName": localConstant.gridHeader.SEARCH_DB_NAME,
            //     "field": "searchDBName",
            //     "filter": "agTextColumnFilter",
            //     "tooltipField": "searchDBName",
            //     "headerTooltip": localConstant.gridHeader.SEARCH_DB_NAME,
            //     "width": 150,   
            // },
            {
                "headerName": localConstant.gridHeader.EREP,
                "field": "isEReportProjectMapped",              
                "tooltip": (params) => {                  
                    if (params.data.isEReportProjectMapped === true) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "headerTooltip": localConstant.gridHeader.EREP,
                "valueGetter": (params) => {                   
                    if (params.data.isEReportProjectMapped === true) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "width": 100,   
            }          
             
        ],
        "enableFilter": true,
        "enableSorting": true,
        "pagination": false,
        "suppressRowClickSelection":true, 
        "searchable":true,
        "rowSelection": "single",
        "gridHeight": 40,
        "clearFilter":false ,
    },
    "AuthorisedProjects": {
        "columnDefs": [
            // {
            //     "checkboxSelection": true,
            //     "headerCheckboxSelectionFilteredOnly": true,
            //     "suppressFilter": true,
            //     "width": 40
            // },            
            {
                "headerName": localConstant.gridHeader.CUSTOMER_NAME,
                "field": "contractCustomerName",                
                "filter": "agTextColumnFilter",
                "tooltipField": "contractCustomerName",   
                "headerTooltip": localConstant.gridHeader.CUSTOMER_NAME,
                "width": 180
            },
            {
                "headerName": localConstant.gridHeader.EXTRANET_CONTRACT_NUMBER,
                "field": "contractNumber",                
                "filter": "agTextColumnFilter",
                "tooltipField": "contractNumber",   
                "headerTooltip": localConstant.gridHeader.EXTRANET_CONTRACT_NUMBER,
                "width": 190
            },
            {
                "headerName": localConstant.gridHeader.PROJECT_NUMBER,
                "field": "projectNumber",
                "filter": "agTextColumnFilter",
                "tooltipField":"projectNumber",   
                "headerTooltip": localConstant.gridHeader.PROJECT_NUMBER,
                "width": 190,
            },
            {
                "headerName": localConstant.gridHeader.CUSTOMER_PROJECT_NUMBER,
                "field": "customerProjectNumber",
                "filter": "agTextColumnFilter",
                "tooltipField":"customerProjectNumber",   
                "headerTooltip": localConstant.gridHeader.CUSTOMER_PROJECT_NUMBER,
                "width": 190,
            },
            {
                "headerName": localConstant.gridHeader.PROJECT_TYPE,
                "field": "projectType",
                "filter": "agTextColumnFilter",
                "tooltipField": "projectType",
                "headerTooltip": localConstant.gridHeader.PROJECT_TYPE,
                "width": 150,   
            }
        ],
        "enableFilter": true,
        "enableSorting": true,
        "pagination": false,
        "animatedRows":true,
        "rowDragManaged":true,
        "searchable":true,
        "rowSelection": "single",
        "gridHeight": 40,
        "clearFilter":false ,
        "suppressRowClickSelection":true    
    },
};