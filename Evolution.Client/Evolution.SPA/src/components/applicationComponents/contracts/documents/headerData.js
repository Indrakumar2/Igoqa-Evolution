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
                "tooltipField": "contractStatus",
                "filter": "agTextColumnFilter",
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
                },
                "width": 180
            },
            {
                "headerName": "<span class='mandate'>" + localConstant.gridHeader.TYPE + "</span>",
                "headerTooltip": localConstant.gridHeader.TYPE,
                "field": "documentType",
                "tooltipField": "contractStatus",
                "filter": "agTextColumnFilter",
                "cellRenderer": "SelectDocumentType",
                "width": 250,
                "documentModuleName":'ContractDocument'            
            },
            {
                "headerName": localConstant.gridHeader.SIZE_IN_KBS,
                "headerTooltip": localConstant.gridHeader.SIZE_IN_KBS,
                "field": "documentSize",
                "tooltipField": "contractStatus",
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
            //     "tooltip": (params) => {
            //         if (params.data.isVisibleToCustomer === true) {
            //             return "Yes";
            //         } else if (params.data.isVisibleToCustomer === false) {
            //             return "No";
            //         }
            //     },
            //     "filter": "agTextColumnFilter",
            //     "width": 180,
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
                "tooltip": (params) => {
                    if (params.data.isVisibleToTS) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "filter": "agTextColumnFilter",
                "width": 180,
                "headerTooltip": localConstant.gridHeader.TS_VISIBLE,
                "valueGetter": (params) => {
                    if (params.data.isVisibleToTS) {
                        return "Yes";
                    } else{
                        return "No";
                    }
                }
            },
            {
                "headerName": localConstant.gridHeader.OUT_OF_COMPANY_VISIBLE,
                "field": "isVisibleOutOfCompany",
                "tooltip": (params) => {
                    if (params.data.isVisibleOutOfCompany === true) {
                        return "Yes";
                    } else if (params.data.isVisibleOutOfCompany === false) {
                        return "No";
                    }
                },
                "filter": "agTextColumnFilter",
                "width": 185,
                "headerTooltip": localConstant.gridHeader.OUT_OF_COMPANY_VISIBLE,
                "valueGetter": (params) => {
                    if (params.data.isVisibleOutOfCompany === true) {
                        return "Yes";
                    } else if (params.data.isVisibleOutOfCompany === false) {
                        return "No";
                    }
                }
            },
            {
                "headerName": "",
                "field": "moduleRefCode",
                "hide": true
            },
            {
                "headerName": "",
                "field": "documentUniqueName",
                "hide": true
            },
            // {
            //     "headerName": "",
            //     "field": "EditContractDocuments",
            //     "cellRenderer": "EditLink",
            //     "cellRendererParams": {},
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
        "exportFileName":"Documents",
        "columnsFiltersToDestroy":[ 'createdOn' ]
    };
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
            "tooltipField": "documentName",
            "filter": "agTextColumnFilter",
            //"cellRenderer": "FileToBeOpen",
            "cellRendererParams": {
                "dataToRender": "documentName"
            },
            "width": 160
        },
        {
            "headerName": "<span class='mandate'>" + localConstant.gridHeader.TYPE + "</span>",
            "headerTooltip": localConstant.gridHeader.TYPE,
            "field": "documentType",
            "tooltipField": "documentType",
            "filter": "agTextColumnFilter",
            "width": 165
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
            "width": 160
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
            // "tooltipField": "isVisibleToCustomer",
            "filter": "agTextColumnFilter",
            "width": 180,
            "headerTooltip": localConstant.gridHeader.CUSTOMER_VISIBLE,
            "tooltip": (params) => {
                if (params.data.isVisibleToCustomer) {
                    return "Yes";
                } else {
                    return "No";
                }
            },  
            // 594 - CONTRACT-Document Tab Issues - UAT Testing
            //Start
            "valueGetter": (params) => {
                if (params.data.isVisibleToCustomer) {
                    return "Yes";
                } else{
                    return "No";
                }
            }
            //End
        },
        {
            "headerName": localConstant.gridHeader.TS_VISIBLE,
            "field": "isVisibleToTS",
            // "tooltipField": "isVisibleToTS",
            "filter": "agTextColumnFilter",
            "width": 180,
            "headerTooltip": localConstant.gridHeader.TS_VISIBLE,
            "tooltip": (params) => {
                if (params.data.isVisibleToTS) {
                    return "Yes";
                } else {
                    return "No";
                }
            },  
            // 594 - CONTRACT-Document Tab Issues - UAT Testing
            //Start
            "valueGetter": (params) => {
                if (params.data.isVisibleToTS) {
                    return "Yes";
                } else{
                    return "No";
                }
            }
            //End
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
export const ProjectHeaderData = {
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
            "width": 130
        },
        {
            "headerName": localConstant.gridHeader.PROJECT_NUMBER,
            "headerTooltip": localConstant.gridHeader.PROJECT_NUMBER,
            "field": "moduleRefCode",
            "tooltipField": "moduleRefCode",
            "filter": "agNumberColumnFilter",
            "width": 170,
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
            "width": 130
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
            "width": 130
        },
        {
            "headerName": localConstant.gridHeader.UPLOADED_DATE,
            "field": "createdOn",
            "tooltipField": "createdOn",
            "filter": "agDateColumnFilter",
            "tooltip": (params) => {
                return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                
            }, 
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
            "tooltip": (params) => {
                if (params.data.isVisibleToCustomer) {
                    return "Yes";
                } else{
                    return "No";
                }
            },
            "filter": "agTextColumnFilter",
            "width": 180,
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
            "tooltip":  (params) => {
                if (params.data.isVisibleToTS) {
                    return "Yes";
                } else {
                    return "No";
                }
            },
            "filter": "agTextColumnFilter",
            "width": 180,
            "headerTooltip": localConstant.gridHeader.TS_VISIBLE,
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
        },
    ],
    "enableFilter": true,
    "enableSorting": true,
    "rowSelection": "multiple",
    "gridHeight": 45,
    "pagination": true,
    "searchable":true,
    "enableSelectAll":true,
    "clearFilter":true, 
    "exportFileName":"Project Documents",
    "columnsFiltersToDestroy":[ 'createdOn' ]
};

export const ParentHeaderData = {
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
            "width": 130
        },
        {
            "headerName": "<span class='mandate'>" + localConstant.gridHeader.TYPE + "</span>",
            "headerTooltip": localConstant.gridHeader.TYPE,
            "field": "documentType",
            "tooltipField": "documentType",
            "filter": "agTextColumnFilter",
            "width": 130
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
            "width": 130
        },
        {
            "headerName": localConstant.gridHeader.UPLOADED_DATE,
            "field": "createdOn",
            "tooltipField": "createdOn",
            "filter": "agDateColumnFilter",
            "tooltip": (params) => {
                return moment(params.data.createdOn).format('DD-MMM-YYYY').toString();
               
            }, 
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
            "tooltip": (params) => {
                if (params.data.isVisibleToCustomer) {
                    return "Yes";
                } else {
                    return "No";
                }
            },
            "filter": "agTextColumnFilter",
            "width": 180,
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
            "tooltip":  (params) => {
                if (params.data.isVisibleToTS) {
                    return "Yes";
                } else{
                    return "No";
                }
            },
            "filter": "agTextColumnFilter",
            "width": 180,
            "headerTooltip": localConstant.gridHeader.TS_VISIBLE,
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
    "rowSelection": "multiple",
    "gridHeight": 45,
    "pagination": true,
    "searchable":true,
    "enableSelectAll":true,
    "clearFilter":true, 
    "exportFileName":"Parent Contract Documents",
    "columnsFiltersToDestroy":[ 'createdOn' ]
};