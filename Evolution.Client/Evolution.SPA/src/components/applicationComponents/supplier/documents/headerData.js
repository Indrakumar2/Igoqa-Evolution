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
                "field": "documentName",
                "filter": "agTextColumnFilter",
                "headerTooltip":localConstant.gridHeader.NAME ,
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
                "field": "documentType",
                "filter": "agTextColumnFilter",           
                "headerTooltip":localConstant.gridHeader.TYPE ,
                "tooltipField":"documentType",
                "cellRenderer": "SelectDocumentType",
                "width": 160,
                "documentModuleName":'SupplierDocument'
            },
            {
                "headerName":localConstant.gridHeader.SIZE,
                "field": "documentSize",
                "filter": "agNumberColumnFilter",
                "filterParams": {
                    "inRangeInclusive":true
                },
                "width": 130,
                "headerTooltip":localConstant.gridHeader.SIZE ,
                "tooltipField":"documentSize",
            },
            {
                "headerName": localConstant.modalConstant.UPLOADED_DATE,
                "field": "createdOn",
                "filter": "agDateColumnFilter",
                "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "dataToRender": "createdOn"
                },
                "headerTooltip": localConstant.modalConstant.UPLOADED_DATE,
                "tooltip": (params) => {
                    return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
            }, 
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
            //     "filter": "agTextColumnFilter",
            //     "width": 180,
            //     "tooltip": (params) => {
            //         if (params.data.isVisibleToCustomer === true) {
            //             return "Yes";
            //         } else if (params.data.isVisibleToCustomer === false) {
            //             return "No";
            //         }
            //     },
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
                "filter": "agTextColumnFilter",
                "width": 130,
                "headerTooltip": localConstant.modalConstant.VISIBLE_TO_TS,
                "tooltip":(params) => {
                    if (params.data.isVisibleToTS) {
                        return "Yes";
                    } else{
                        return "No";
                    }
                },
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
            //     "field": "EditSupplierDocuments",
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
        "searchable":true,
        "enableSelectAll":true,
        "clearFilter":true, 
        "exportFileName":"Supplier Documents",
        "columnsFiltersToDestroy":[ 'createdOn' ]
    };
};
