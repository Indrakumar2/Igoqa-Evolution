import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
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
                "headerTooltip": localConstant.gridHeader.NAME,
                "field": "documentName",
                "tooltipField":"documentName",
                "filter": "agTextColumnFilter",
                "width": 175,
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
                "documentModuleName":'ProjectDocument'
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
                "width": 107
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
            //     "width": 190,
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
                    if (params.data.isVisibleToCustomer) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                },  
                "valueGetter": (params) => {
                    if (params.data.isVisibleToCustomer) {
                        return "Yes";
                    } else {
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
                    } else {
                        return "No";
                    }
                },
                "filter": "agTextColumnFilter",
                "width": 130,
                "headerTooltip": localConstant.modalConstant.VISIBLE_TO_TS,
                "valueGetter": (params) => {
                    if (params.data.isVisibleToTS) {
                        return "Yes";
                    } else{
                        return "No";
                    }
                }
            },
            // {
            //     "headerName": "",
            //     "field": "EditProjectDocuments",
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
        "rowSelection": "multiple",
        "gridHeight": 45,
        "pagination": true,
        "enableSelectAll":true,
        "searchable":true,
        "clearFilter":true, 
        "exportFileName":"Project Documents",
        "columnsFiltersToDestroy":[ 'createdOn' ]
    };
};
export const ContractHeaderData = {
    "columnDefs": [
        {
            "checkboxSelection": true,
            "headerCheckboxSelectionFilteredOnly": true,
            "suppressFilter": true,
            "width": 40
        },
        {
            "headerName": localConstant.gridHeader.NAME ,
            "headerTooltip": localConstant.gridHeader.NAME,
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
            "headerTooltip": localConstant.gridHeader.TYPE,
            "field": "documentType",
            "tooltipField":"documentType",
            "filter": "agTextColumnFilter",
            "width": 150
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
            "width": 150
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
            "width": 200,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },
        {
            "headerName": localConstant.modalConstant.VISIBLE_TO_CUSTOMER,
            "field": "isVisibleToCustomer",
            "tooltip": (params) => {
                if (params.data.isVisibleToCustomer === true) {
                    return "Yes";
                } else if (params.data.isVisibleToCustomer === false) {
                    return "No";
                }
            },
            "filter": "agTextColumnFilter",
            "width": 190,
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
                } else{
                    return "No";
                }
            },
            "filter": "agTextColumnFilter",
            "width": 190,
            "headerTooltip": localConstant.modalConstant.VISIBLE_TO_TS,
            "valueGetter": (params) => {
                if (params.data.isVisibleToTS) {
                    return "Yes";
                } else {
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
    "rowSelection": "multiple",
    "gridHeight": 45,
    "pagination": true,
    "enableSelectAll":true,
    "searchable":true,
    "clearFilter":true, 
    "exportFileName":"Contract Documents",
    "columnsFiltersToDestroy":[ 'createdOn' ]
};
export const CustomerHeaderData = {
    "columnDefs": [
        {
            "checkboxSelection": true,
            "headerCheckboxSelectionFilteredOnly": true,
            "suppressFilter": true,
            "width": 40
        },
        {
            "headerName": localConstant.gridHeader.NAME,
            "headerTooltip": localConstant.gridHeader.NAME,
            "field": "documentName",
            "tooltipField":"documentName",
            "filter": "agTextColumnFilter",
           // "cellRenderer": "FileToBeOpen",
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
            "width": 150
        },
        {
            "headerName": localConstant.gridHeader.SIZE,
            "headerTooltip": localConstant.gridHeader.SIZE,
            "field": "documentSize",
            "tooltipField":"documentSize",
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "width": 150
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
            "width": 200,
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
                if (params.data.isVisibleToCustomer) {
                    return "Yes";
                } else {
                    return "No";
                }
            }   ,   
            "filter": "agTextColumnFilter",
            "width": 190,
            "headerTooltip": localConstant.modalConstant.VISIBLE_TO_CUSTOMER  ,
            "valueGetter": (params) => {
                if (params.data.isVisibleToCustomer) {
                    return "Yes";
                } else {
                    return "No";
                }
            }      
        },
        {
            "headerName": localConstant.modalConstant.VISIBLE_TO_TS,
            "field": "isVisibleToTS",
            "tooltip":(params) => {
                if (params.data.isVisibleToTS) {
                    return "Yes";
                } else {
                    return "No";
                }
            },
            "filter": "agTextColumnFilter",
            "width": 190,
            "headerTooltip": localConstant.modalConstant.VISIBLE_TO_TS,
            "valueGetter": (params) => {
                if (params.data.isVisibleToTS) {
                    return "Yes";
                } else {
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
    "rowSelection": "multiple",
    "gridHeight": 45,
    "pagination": true,
    "enableSelectAll":true,
    "searchable":true,
    "clearFilter":true, 
    "exportFileName":"Customer Documents",
    "columnsFiltersToDestroy":[ 'createdOn' ]
};