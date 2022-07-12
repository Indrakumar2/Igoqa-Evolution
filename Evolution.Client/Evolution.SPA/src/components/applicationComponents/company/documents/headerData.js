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
                "filter": "agTextColumnFilter",
                "tooltipField": "documentName",           
                // "cellRenderer": "FileToBeOpen",
                 "cellRendererParams": {
                     "dataToRender": "documentName"
                 }
            },
            {
                "headerName": "<span class='mandate'>" + localConstant.gridHeader.TYPE + "</span>",
                "headerTooltip": localConstant.gridHeader.TYPE,
                "field": "documentType",
                "tooltipField": "documentType",
                "filter": "agTextColumnFilter",
                "cellRenderer": "SelectDocumentType",
                "width": 250,
                "documentModuleName":'CompanyDocument'
            },
            {
                "headerName": localConstant.gridHeader.SIZE_IN_KBS,
                "headerTooltip": localConstant.gridHeader.SIZE_IN_KBS,
                "field": "documentSize",
                "tooltipField": "documentSize",
                "filter": "agNumberColumnFilter",
                "filterParams": {
                    "inRangeInclusive":true
                },
                "width": 150
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
                "filterParams": {
                    "comparator": dateUtil.comparator,
                    "inRangeInclusive": true,
                    "browserDatePicker": true
                  },
                "headerTooltip": localConstant.gridHeader.UPLOADED_DATE,
                "width":160
            },
            // {
            //     "headerName": localConstant.gridHeader.CUSTOMER_VISIBLE,
            //     "field": "isVisibleToCustomer",
            //     "tooltip": (params) => {
            //         if (params.data.isVisibleToCustomer === true) {
            //             return "Yes";
            //         } else if (params.data.isVisibleToCustomer === false) {
            //             return "No";
            //         }
            //     },
            //     "filter": "agTextColumnFilter",
            //     "width": 150,
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
                "headerName": localConstant.gridHeader.TS_VISIBLE,
                "field": "isVisibleToTS",
                "tooltip":  (params) => {
                    if (params.data.isVisibleToTS) {
                        return "Yes";
                    } else{
                        return "No";
                    }
                },
                "filter": "agTextColumnFilter",
                "width": 150,
                "headerTooltip": localConstant.gridHeader.TS_VISIBLE,
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
            //     "field": "EditCompanyDocuments",
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
        "enableSelectAll":true,
        "enableFilter": true,
        "enableSorting": true,
        "rowSelection": "multiple",
        "gridHeight": 60,
        "pagination": true,
        "clearFilter":true, 
        "searchable":true,
        "exportFileName":"Documents",
        "columnsFiltersToDestroy":[ 'createdOn' ]
    };   
};