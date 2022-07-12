import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const DocumentsHeaderData= (functionRefs)=> {
    return {
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
            "editable":  (params) => {              
                if (!params.data.roleBase === true&&params.data.isFileUploaded) {
                    return true;
                } else if (!params.data.roleBase === false&&!params.data.isFileUploaded) {
                    return false;
                }
            },
            "tooltipField": "documentName",
            "filter": "agTextColumnFilter",
            //"cellRenderer": "FileToBeOpen",
            "cellRendererParams": {
                "dataToRender": "documentName"
            },
            "width":150
        },
        {
            "headerName": "<span class='mandate'>" + localConstant.gridHeader.TYPE + "</span>",
            "headerTooltip": localConstant.gridHeader.TYPE,
            "field": "documentType",
            "tooltipField": "documentType",
            "filter": "agTextColumnFilter",
            "cellRenderer": "SelectDocumentType",
            "width": 250,
            "documentModuleName":'CustomerDocument'            
        },
        {
            "headerName": localConstant.gridHeader.SIZE_IN_KBS,
            "field": "documentSize",
            "tooltipField": "documentSize",
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "width": 150,
            "headerTooltip": localConstant.gridHeader.SIZE_IN_KBS
        },
        {
            "headerName": localConstant.gridHeader.UPLOADED_DATE,
            "field": "createdOn",
            "tooltip": (params) => {
                return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filter": "agDateColumnFilter",
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "createdOn"
            },
            "headerTooltip": localConstant.gridHeader.UPLOADED_DATE,
            "width": 180,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },
        // {
        //     "headerName": localConstant.gridHeader.CUSTOMER_VISIBLE,
        //     "field": "isVisibleToCustomer",
        //     "tooltip":  (params) => {
        //         if (params.data.isVisibleToCustomer === true) {
        //             return "Yes";
        //         } else if (params.data.isVisibleToCustomer === false) {
        //             return "No";
        //         }
        //     },
        //     "filter": "agTextColumnFilter",
        //     "width": 190,
        //     "headerTooltip": localConstant.gridHeader.CUSTOMER_VISIBLE,
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
            "width": 150,
            "cellRenderer": "InlineSwitch",
            "cellRendererParams": {
                "isSwitchLabel": false,
                "name":"isVisibleToCustomer",
                "id":"isVisibleToCustomer",
                "switchClass":"",
                "disabled":functionRefs.isDisable,  //Defect 691-issue 4 Changes
                // "disabled":props.disabled,
            },
            "tooltip": (params) => {
                if (params.data.isVisibleToCustomer) {
                    return "Yes";
                } else{
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
            "headerName": localConstant.gridHeader.TS_VISIBLE,
            "field": "isVisibleToTS",
            "tooltip":  (params) => {
                if (params.data.isVisibleToTS) {
                    return "Yes";
                } else {
                    return "No";
                }
            },
            "filter": "agTextColumnFilter",
            "width": 150,
            "headerTooltip": localConstant.gridHeader.TS_VISIBLE,
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
        //     "field": "EditCustomerDocuments",
        //     "cellRenderer": "EditLink",
        //     "cellRendererParams": { },
        //     "suppressFilter": true,
        //     "suppressSorting": true,
        //     "width": 50
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
        },
        {
            "headerName": "upload Data Id",
            "field": "uploadDataId",
            "hide": true
        }
    ],
    "enableSelectAll":true,
    "enableFilter": true,
    "enableSorting": true,
    "rowSelection": "multiple",
    "gridHeight": 45,
    "pagination": true,
    "searchable":true,
    "clearFilter":true, 
    "exportFileName":"Documents",
    "columnsFiltersToDestroy":[ 'createdOn' ]
};
};
export const ContractsHeaderData=  {
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
            "tooltipField": "documentName",
            "filter": "agTextColumnFilter",
            //"cellRenderer": "FileToBeOpen",
            "cellRendererParams": {
                "dataToRender": "documentName"
            },
            "width":150
        },
        {
            "headerName":localConstant.gridHeader.CONTRACT_NUMBER,
            "headerTooltip": localConstant.gridHeader.CONTRACT_NUMBER,
            "field":"moduleRefCode",
            "tooltipField": "moduleRefCode",
            "filter":"agNumberColumnFilter",
            //"cellRenderer": "contractAnchorRenderer",
            "width":150,
            "filterParams": {
                "inRangeInclusive":true
            },
        },
        {
            "headerName": "<span class='mandate'>" + localConstant.gridHeader.TYPE + "</span>",
            "headerTooltip": localConstant.gridHeader.TYPE,
            "field": "documentType",
            "tooltipField": "documentType",
            "filter": "agTextColumnFilter",
            "width": 150
        },
        {
            "headerName": localConstant.gridHeader.SIZE_IN_KBS,
            "field": "documentSize",
            "tooltipField": "documentSize",
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "width": 140,
            "headerTooltip": localConstant.gridHeader.SIZE_IN_KBS
        },
        {
            "headerName": localConstant.gridHeader.UPLOADED_DATE,
            "field": "createdOn",
            "tooltip": (params) => {
                return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
               
        }, 
            "filter": "agDateColumnFilter",
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "createdOn"
            },
            "headerTooltip": localConstant.gridHeader.UPLOADED_DATE,
            "width": 180,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },
        {
            "headerName": localConstant.gridHeader.CUSTOMER_VISIBLE,
            "field": "isVisibleToCustomer",
            "tooltip" :(params) => {
                if (params.data.isVisibleToCustomer) {
                    return "Yes";
                } else {
                    return "No";
                }

            },
            "filter": "agTextColumnFilter",
            "width": 170,
            "headerTooltip": localConstant.gridHeader.CUSTOMER_VISIBLE,
            "valueGetter": (params) => {
                if (params.data.isVisibleToCustomer) {
                    return "Yes";
                } else {
                    return "No";
                }
            }
        },
        {
            "headerName": localConstant.gridHeader.TS_VISIBLE,
            "field": "isVisibleToTS",
            "tooltipField": "isVisibleToTS",
            "filter": "agTextColumnFilter",
            "width": 190,
            "headerTooltip": localConstant.gridHeader.TS_VISIBLE,
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
        },
    ],
    "enableSelectAll":true,
    "enableFilter": true,
    "enableSorting": true,
    "rowSelection": "multiple",
    "gridHeight": 45,
    "pagination": true,
    "searchable":true,
    "clearFilter":true, 
    "exportFileName":"Contract Documents",
    "columnsFiltersToDestroy":[ 'createdOn' ]
};
export const ProjectHeaderData=  {
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
            "tooltipField": "documentName",
            "filter": "agTextColumnFilter",
            //"cellRenderer": "FileToBeOpen",
            "cellRendererParams": {
                "dataToRender": "documentName"
            },
            "width":150
        },
        {
            "headerName":localConstant.gridHeader.PROJECT_NUMBER,
            "headerTooltip": localConstant.gridHeader.PROJECT_NUMBER,
            "field":"moduleRefCode",
            "tooltipField": "moduleRefCode",
            "filter":"agNumberColumnFilter",
            // "cellRenderer": "contractAnchorRenderer",
            "width":170,
            "filterParams": {
                "inRangeInclusive":true
            },
        },
        {
            "headerName":"<span class='mandate'>" + localConstant.gridHeader.TYPE + "</span>",
            "headerTooltip": localConstant.gridHeader.TYPE,
            "field": "documentType",
            "tooltipField": "documentType",
            "filter": "agTextColumnFilter",
            "width": 150
        },
        {
            "headerName": localConstant.gridHeader.SIZE_IN_KBS,
            "field": "documentSize",
            "tooltipField": "documentSize",
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "width": 140,
            "headerTooltip": localConstant.gridHeader.SIZE_IN_KBS
        },
        {
            "headerName": localConstant.gridHeader.UPLOADED_DATE,
            "field": "createdOn",
            "tooltip": (params) => {
                return  moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filter": "agDateColumnFilter",
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "createdOn"
            },
            "headerTooltip": localConstant.gridHeader.UPLOADED_DATE,
            "width": 180,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },
        {
            "headerName": localConstant.gridHeader.CUSTOMER_VISIBLE,
            "field": "isVisibleToCustomer",
            "tooltip":  (params) => {
                if (params.data.isVisibleToCustomer) {
                    return "Yes";
                } else{
                    return "No";
                }
            },
            "filter": "agTextColumnFilter",
            "width": 170,
            "headerTooltip": localConstant.gridHeader.CUSTOMER_VISIBLE,
            "valueGetter": (params) => {
                if (params.data.isVisibleToCustomer) {
                    return "Yes";
                } else {
                    return "No";
                }
            }
        },
        {
            "headerName": localConstant.gridHeader.TS_VISIBLE,
            "field": "isVisibleToTS",
            "tooltip": (params) => {
                if (params.data.isVisibleToTS) {
                    return "Yes";
                } else {
                    return "No";
                }
            },
            "filter": "agTextColumnFilter",
            "width": 190,
            "headerTooltip": localConstant.gridHeader.TS_VISIBLE,
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
        },
     ],
    "enableSelectAll":true,
    "enableFilter": true,
    "enableSorting": true,
    "rowSelection": "multiple",
    "gridHeight":45,
    "pagination": true,
    "searchable":true,
    "clearFilter":true, 
    "exportFileName":"Project Documents",
    "columnsFiltersToDestroy":[ 'createdOn' ]
};