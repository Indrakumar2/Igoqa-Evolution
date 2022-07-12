import { getlocalizeData } from '../../../../utils/commonUtils';
import moment from 'moment';
import dateUtil from '../../../../utils/dateUtil';
const localConstant = getlocalizeData();
export const HeaderData = (functionRefs) => {
    return {
        "columnDefs": [
            {
                "checkboxSelection": true,
                "headerCheckboxSelectionFilteredOnly": true,
                "suppressFilter": true,
                "width": 40
            },
            {
                "headerName": localConstant.gridHeader.NAME ,
                "headerTooltip":  localConstant.gridHeader.NAME ,
                "width": 160,
                "field": "documentName",
                "filter": "agTextColumnFilter",
                "tooltipField":"documentName", 
                //"cellRenderer": "FileToBeOpen",
                "editable":  (params) => {              
                    if (!params.data.roleBase === true&&params.data.isFileUploaded) {
                        return true;
                    } else if (!params.data.roleBase === false&&!params.data.isFileUploaded) {
                        return false;
                    }
                },
                "cellRendererParams": {
                    "dataToRender": "documentName"
                }
            },
            {
                "headerName": "<span class='mandate'>" + localConstant.gridHeader.TYPE + "</span>",
                "headerTooltip": localConstant.gridHeader.TYPE,
                "field": "documentType",
                "tooltipField":"documentType", 
                "filter": "agTextColumnFilter",
                "cellRenderer": "SelectDocumentType",
                "width": 245,
                "documentModuleName":'AssignmentDocument'
            },
            {
                "headerName":localConstant.gridHeader.SIZE,
                "headerTooltip": localConstant.gridHeader.SIZE,
                "field": "documentSize",
                "tooltipField":"documentSize", 
                "filter": "agNumberColumnFilter",
                "filterParams": {
                    "inRangeInclusive":true
                },
                "width": 130
            },
            {
                "headerName": localConstant.modalConstant.UPLOADED_DATE,
                "field": "createdOn",
                "tooltip": (params) => {
                    return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                    
                },
                "filter": "agDateColumnFilter",           
                "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "dataToRender": "createdOn"
                },
                "headerTooltip": localConstant.modalConstant.UPLOADED_DATE,
                "width": 130,
                "filterParams": {
                    "comparator": dateUtil.comparator,
                    "inRangeInclusive": true,
                    "browserDatePicker": true
                } 
            },
            // {
            //     "headerName": localConstant.modalConstant.VISIBLE_TO_CUSTOMER,
            //     "field": "isVisibleToCustomer",
            //     "tooltip":(params) => {
            //         if (params.data.isVisibleToCustomer === true) {
            //             return "Yes";
            //         } else if (params.data.isVisibleToCustomer === false) {
            //             return "No";
            //         }
            //     },
            //     "filter": "agTextColumnFilter",
            //     "width": 180,
            //     "headerTooltip": localConstant.modalConstant.VISIBLE_TO_CUSTOMER,
            //     "valueGetter": (params) => {
            //         if (params.data.isVisibleToCustomer === true) {
            //             return "Yes";
            //         } else if (params.data.isVisibleToCustomer === false) {
            //             return "No";
            //         }
            //     }
            // },
    
            {
                "headerName": localConstant.gridHeader.CUSTOMER_VISIBLE,
                "field": "isVisibleToCustomer",
                "filter": "agTextColumnFilter",
                "width": 130,
                "cellRenderer": "InlineSwitch",
                "cellRendererParams": {
                    "isSwitchLabel": false,
                    "name":"isVisibleToCustomer",
                    "id":"isVisibleToCustomer",
                    "switchClass":"",
                    "disabled":functionRefs.isDisable,
                    // "disabled":props.disabled,
                },
                "tooltip": (params) => {
                    if (params.data.isVisibleToCustomer === true) {
                        return "Yes";
                    } else if (params.data.isVisibleToCustomer === false) {
                        return "No";
                    }
                },  
                "valueGetter": (params) => {
                    if (params.data.isVisibleToCustomer === true) {
                        return "Yes";
                    } else if (params.data.isVisibleToCustomer === false) {
                        return "No";
                    }
                },
                "headerTooltip": localConstant.gridHeader.CUSTOMER_VISIBLE,
            }, 
            {
                "headerName":localConstant.modalConstant.VISIBLE_TO_TS,
                "field": "isVisibleToTS",
                "tooltip":(params) => {
                    if (params.data.isVisibleToTS) {
                        return "Yes";
                    } else{
                        return "No";
                    }
                }, 
                "filter": "agTextColumnFilter",
                "width": 130,
                "headerTooltip": localConstant.modalConstant.VISIBLE_TO_TS,
                "valueGetter": (params) => {
                    if (params.data.isVisibleToTS) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                }
            },
            // {
            //     "headerName": "",
            //     "field": "EditAssignmentDocuments",
            //     "cellRenderer": "EditLink",
            //     "cellRendererParams": { },
            //     "suppressFilter": true,
            //     "suppressSorting": true,
            //     "width": 100
            // },
            {
                "headerName": '',           
                "field": "documentName",                      
                "tooltipField": "documentName",           
                "cellRenderer": "FileToBeOpen",
                "suppressFilter": true,
                "suppressSorting": true,
                 "cellRendererParams": {
                     "dataToRender": "documentName"
                 },
                 "width": 50
            }
        ],
        "enableFilter": true,
        "enableSorting": true,
        "pagination": true,
        "searchable":true,
        "enableSelectAll":true,
        "rowSelection": "multiple",
        "clearFilter":true, 
        "gridHeight": 45,
        "columnsFiltersToDestroy":[ 'createdOn' ]
    };
};
export const ChildHeaderData = {
    "columnDefs": [
        {
            "checkboxSelection": true,
            "headerCheckboxSelectionFilteredOnly": true,
            "suppressFilter": true,
            "width": 40
        },
        {
            "headerName": localConstant.gridHeader.NAME ,
            "headerTooltip": localConstant.gridHeader.NAME ,
            "field": "documentName",
            "tooltipField":"documentName", 
            "filter": "agTextColumnFilter",
            //"cellRenderer": "FileToBeOpen",
            "cellRendererParams": {
                "dataToRender": "documentName"
            }
        },
        {
            "headerName": "<span class='mandate'>" + localConstant.gridHeader.TYPE + "</span>",
            "headerTooltip":  localConstant.gridHeader.TYPE,
            "field": "documentType",
            "tooltipField":"documentType", 
            "filter": "agTextColumnFilter",
            "width": 150
        },
        {
            "headerName":localConstant.gridHeader.SIZE,
            "headerTooltip":localConstant.gridHeader.SIZE,
            "field": "documentSize",
            "tooltipField":"documentSize", 
            "filter": "agNumberColumnFilter",
            "width": 130
        },
        {
            "headerName": localConstant.modalConstant.UPLOADED_DATE,
            "field": "createdOn",
            "tooltip": (params) => {
                return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filter": "agDateColumnFilter",
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "createdOn"
            },
            "headerTooltip": localConstant.modalConstant.UPLOADED_DATE,
            "width": 180,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
            } 
        },
        {
            "headerName": localConstant.modalConstant.VISIBLE_TO_CUSTOMER,
            "field": "isVisibleToCustomer",
            "tooltip":(params) => {
                if (params.data.isVisibleToCustomer === true) {
                    return "Yes";
                } else if (params.data.isVisibleToCustomer === false) {
                    return "No";
                }
            }, 
            "filter": "agTextColumnFilter",
            "width": 180,
            "headerTooltip": localConstant.modalConstant.VISIBLE_TO_CUSTOMER,
            "valueGetter": (params) => {
                if (params.data.isVisibleToCustomer === true) {
                    return "Yes";
                } else if (params.data.isVisibleToCustomer === false) {
                    return "No";
                }
            }
        },
        {
            "headerName":localConstant.modalConstant.VISIBLE_TO_TS,
            "field": "isVisibleToTS",
            "tooltip": (params) => {
                if (params.data.isVisibleToTS) {
                    return "Yes";
                } else {
                    return "No";
                }
            }, 
            "filter": "agTextColumnFilter",
            "width": 180,
            "headerTooltip": localConstant.modalConstant.VISIBLE_TO_TS,
            "valueGetter": (params) => {
                if (params.data.isVisibleToTS) {
                    return "Yes";
                } else{
                    return "No";
                }
            }
        },
        {
            "headerName": '',           
            "field": "documentName",                      
            "tooltipField": "documentName",           
            "cellRenderer": "FileToBeOpen",
            "suppressFilter": true,
            "suppressSorting": true,
             "cellRendererParams": {
                 "dataToRender": "documentName"
             },
             "width": 50
        }
    ],
    "enableFilter": true,
    "enableSorting": true,
    "pagination": true,
    "searchable":true,
    "enableSelectAll":true,
    "rowSelection": "multiple",
    "clearFilter":true, 
    "gridHeight": 45,
    "columnsFiltersToDestroy":[ 'createdOn' ]
};
export const TimesheetDocumentHeaderData = {
    "columnDefs": [
        {
            "checkboxSelection": true,
            "headerCheckboxSelectionFilteredOnly": true,
            "suppressFilter": true,
            "width": 40
        },
        {
            "headerName": localConstant.gridHeader.NAME ,
            "headerTooltip": localConstant.gridHeader.NAME ,
            "field": "documentName",
            "tooltipField":"documentName", 
            "filter": "agTextColumnFilter",
            //"cellRenderer": "FileToBeOpen",
            "cellRendererParams": {
                "dataToRender": "documentName"
            },
            "width": 150
        },
        {
            "headerName": localConstant.gridHeader.TIMESHEET_DESCRIPTION,
            "headerTooltip":  localConstant.gridHeader.TIMESHEET_DESCRIPTION,
            "field": "timesheetDescription",
            "tooltipField":"timesheetDescription", 
            "filter": "agTextColumnFilter",
            "width": 170
        },
        {
            "headerName": "<span class='mandate'>" + localConstant.gridHeader.TYPE + "</span>",
            "headerTooltip":  localConstant.gridHeader.TYPE,
            "field": "documentType",
            "tooltipField":"documentType", 
            "filter": "agTextColumnFilter",
            "width": 150
        },
        {
            "headerName":localConstant.gridHeader.SIZE,
            "headerTooltip":localConstant.gridHeader.SIZE,
            "field": "documentSize",
            "tooltipField":"documentSize", 
            "filter": "agNumberColumnFilter",
            "width": 130
        },
        {
            "headerName": localConstant.modalConstant.UPLOADED_DATE,
            "field": "createdOn",
            "tooltip": (params) => {
                return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filter": "agDateColumnFilter",
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "createdOn"
            },
            "headerTooltip": localConstant.modalConstant.UPLOADED_DATE,
            "width": 180,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
            } 
        },
        {
            "headerName": localConstant.modalConstant.VISIBLE_TO_CUSTOMER,
            "field": "isVisibleToCustomer",
            "tooltip":(params) => {
                if (params.data.isVisibleToCustomer === true) {
                    return "Yes";
                } else if (params.data.isVisibleToCustomer === false) {
                    return "No";
                }
            }, 
            "filter": "agTextColumnFilter",
            "width": 180,
            "headerTooltip": localConstant.modalConstant.VISIBLE_TO_CUSTOMER,
            "valueGetter": (params) => {
                if (params.data.isVisibleToCustomer === true) {
                    return "Yes";
                } else if (params.data.isVisibleToCustomer === false) {
                    return "No";
                }
            }
        },
        {
            "headerName":localConstant.modalConstant.VISIBLE_TO_TS,
            "field": "isVisibleToTS",
            "tooltip": (params) => {
                if (params.data.isVisibleToTS) {
                    return "Yes";
                } else {
                    return "No";
                }
            }, 
            "filter": "agTextColumnFilter",
            "width": 180,
            "headerTooltip": localConstant.modalConstant.VISIBLE_TO_TS,
            "valueGetter": (params) => {
                if (params.data.isVisibleToTS) {
                    return "Yes";
                } else{
                    return "No";
                }
            }
        },
        {
            "headerName": '',           
            "field": "documentName",                      
            "tooltipField": "documentName",           
            "cellRenderer": "FileToBeOpen",
            "suppressFilter": true,
            "suppressSorting": true,
             "cellRendererParams": {
                 "dataToRender": "documentName"
             },
             "width": 50
        }
    ],
    "enableFilter": true,
    "enableSorting": true,
    "pagination": true,
    "searchable":true,
    "enableSelectAll":true,
    "rowSelection": "multiple",
    "clearFilter":true, 
    "gridHeight": 45,
    "columnsFiltersToDestroy":[ 'createdOn' ]
};
export const VisitDocumentHeaderData = {
    "columnDefs": [
        {
            "checkboxSelection": true,
            "headerCheckboxSelectionFilteredOnly": true,
            "suppressFilter": true,
            "width": 40
        },
        {
            "headerName": localConstant.gridHeader.NAME ,
            "headerTooltip": localConstant.gridHeader.NAME ,
            "field": "documentName",
            "tooltipField":"documentName", 
            "filter": "agTextColumnFilter",
            //"cellRenderer": "FileToBeOpen",
            "cellRendererParams": {
                "dataToRender": "documentName"
            }
        },
        {
            "headerName": localConstant.gridHeader.REPORT_NO,
            "headerTooltip":  localConstant.gridHeader.REPORT_NO,
            "field": "visitReportNumber",
            "tooltipField":"visitReportNumber", 
            "filter": "agTextColumnFilter",
            "width": 150
        },
        {
            "headerName": "<span class='mandate'>" + localConstant.gridHeader.TYPE + "</span>",
            "headerTooltip":  localConstant.gridHeader.TYPE,
            "field": "documentType",
            "tooltipField":"documentType", 
            "filter": "agTextColumnFilter",
            "width": 150
        },
        {
            "headerName":localConstant.gridHeader.SIZE,
            "headerTooltip":localConstant.gridHeader.SIZE,
            "field": "documentSize",
            "tooltipField":"documentSize", 
            "filter": "agNumberColumnFilter",
            "width": 130
        },
        {
            "headerName": localConstant.modalConstant.UPLOADED_DATE,
            "field": "createdOn",
            "tooltip": (params) => {
                return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filter": "agDateColumnFilter",
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "createdOn"
            },
            "headerTooltip": localConstant.modalConstant.UPLOADED_DATE,
            "width": 180,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
            } 
        },
        {
            "headerName": localConstant.modalConstant.VISIBLE_TO_CUSTOMER,
            "field": "isVisibleToCustomer",
            "tooltip":(params) => {
                if (params.data.isVisibleToCustomer === true) {
                    return "Yes";
                } else if (params.data.isVisibleToCustomer === false) {
                    return "No";
                }
            }, 
            "filter": "agTextColumnFilter",
            "width": 180,
            "headerTooltip": localConstant.modalConstant.VISIBLE_TO_CUSTOMER,
            "valueGetter": (params) => {
                if (params.data.isVisibleToCustomer === true) {
                    return "Yes";
                } else if (params.data.isVisibleToCustomer === false) {
                    return "No";
                }
            }
        },
        {
            "headerName":localConstant.modalConstant.VISIBLE_TO_TS,
            "field": "isVisibleToTS",
            "tooltip": (params) => {
                if (params.data.isVisibleToTS) {
                    return "Yes";
                } else {
                    return "No";
                }
            }, 
            "filter": "agTextColumnFilter",
            "width": 180,
            "headerTooltip": localConstant.modalConstant.VISIBLE_TO_TS,
            "valueGetter": (params) => {
                if (params.data.isVisibleToTS) {
                    return "Yes";
                } else{
                    return "No";
                }
            }
        },
        {
            "headerName": '',           
            "field": "documentName",                      
            "tooltipField": "documentName",           
            "cellRenderer": "FileToBeOpen",
            "suppressFilter": true,
            "suppressSorting": true,
             "cellRendererParams": {
                 "dataToRender": "documentName"
             },
             "width": 50
        }
    ],
    "enableFilter": true,
    "enableSorting": true,
    "pagination": true,
    "searchable":true,
    "enableSelectAll":true,
    "rowSelection": "multiple",
    "clearFilter":true, 
    "gridHeight": 45,
    "columnsFiltersToDestroy":[ 'createdOn' ]
};