import { getlocalizeData } from '../../../../utils/commonUtils';

const localConstant = getlocalizeData();
export const HeaderData = {
    "RemittanceTextHeader": {
        "columnDefs": [
            {
                "checkboxSelection": true,
                "headerCheckboxSelectionFilteredOnly": true,
                "suppressFilter": true,
                "width": 40
            },
            {
                "headerName": localConstant.gridHeader.IDENTIFIER,
                "headerTooltip": localConstant.gridHeader.IDENTIFIER,
                "field": "msgIdentifier",
                "tooltipField": "msgIdentifier",
                "filter": "agTextColumnFilter",
                "width": 150
            },
            {
                "headerName": localConstant.gridHeader.INVOICE_REMITTANCE_TEXT,
                "headerTooltip": localConstant.gridHeader.INVOICE_REMITTANCE_TEXT,
                "field": "msgText",
              
                "filter": "agTextColumnFilter",
                "width": 500,
                "tooltipField": "msgText"
            },
            {
                "headerName": localConstant.gridHeader.DEFAULT,
                "headerTooltip": localConstant.gridHeader.DEFAULT,
                "field": "isDefaultMsg",
                "tooltip": (params) => {
                    if (params.data.isDefaultMsg === true) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "filter": "agTextColumnFilter",
                "width": 160,
                "valueGetter": (params) => {
                    if (params.data.isDefaultMsg === true) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                }
            },
            {
                "headerName": localConstant.gridHeader.NOT_USED,
                "headerTooltip": localConstant.gridHeader.NOT_USED,
                "field": "isActive",
                "tooltip":( params) => {
                    if (params.data.isActive === false) {//Changes for D-1233
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "filter": "agTextColumnFilter",
                "width": 154,
                "valueGetter": (params) => {
                    if (params.data.isActive === false) {//Changes for D-1233
                        return "Yes";
                    } else {
                        return "No";
                    }
                }
            },
            {
                "headerName": "",
                "field": "InvoiceDefault",
                "tooltipField": "minimumMargin",
                "cellRenderer": "EditLink",
                "buttonAction":"",
                "cellRendererParams":{
                },
                "suppressFilter": true,
                "suppressSorting": true,
                "width": 60
            }
        ],
        "enableSelectAll":true,
        "enableFilter": true,
        "enableSorting": true,
        "pagination": true,
        "searchable":true,
        "rowSelection": "multiple",
        "gridHeight": 30,
        "clearFilter":true, 
        "exportFileName":"Remittance Text"
    },
    "InvoiceFooterTextHeader": {
        "columnDefs": [
            {
                "checkboxSelection": true,
                "headerCheckboxSelectionFilteredOnly": true,
                "suppressFilter": true,
                "width": 40
            },
            {
                "headerName": localConstant.gridHeader.IDENTIFIER,
                "headerTooltip": localConstant.gridHeader.IDENTIFIER,
                "field": "msgIdentifier",
                "tooltipField": "msgIdentifier",
                "filter": "agTextColumnFilter",
                "width": 150
            },
            {
                "headerName": localConstant.gridHeader.INVOICE_FOOTER_TEXT,
                "headerTooltip": localConstant.gridHeader.INVOICE_FOOTER_TEXT,
                "field": "msgText",
                
                "filter": "agTextColumnFilter",
                "width": 620,
                "tooltipField": "msgText"
            },
            {
                "headerName": localConstant.gridHeader.DEFAULT,
                "headerTooltip": localConstant.gridHeader.DEFAULT,
                "field": "isDefaultMsg",
                "tooltip":(params) => {
                    if (params.data.isDefaultMsg === true) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "filter": "agTextColumnFilter",
                "width": 193,
                "valueGetter": (params) => {
                    if (params.data.isDefaultMsg === true) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                }
            },
            {
                "headerName": "",
                "field": "InvoiceFooter",
                "cellRenderer": "EditLink",
                "buttonAction":"",
                "cellRendererParams":{
                },
                "suppressFilter": true,
                "suppressSorting": true,
                "width": 60
            }
        ],
        "enableSelectAll":true,
        "enableFilter": true,
        "enableSorting": true,
        "pagination": true,
        "searchable":true,
        "rowSelection": "multiple",
        "clearFilter":true, 
        "gridHeight": 30,
        "exportFileName":"Invoice Footer Text"
    }
};