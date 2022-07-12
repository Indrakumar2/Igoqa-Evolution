import { getlocalizeData } from '../../../../utils/commonUtils';

const localConstant = getlocalizeData();
export const HeaderData = {
    "columnDefs": [
        {
            "checkboxSelection": true,
            "headerCheckboxSelectionFilteredOnly": true,
            "suppressFilter": true,
            "width": 40
        },
        {
            "headerName": localConstant.gridHeader.DESCRIPTION,
            "field": "tax",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.DESCRIPTION,
            "tooltipField": "tax",
            "width": 450
        },
        {
            "headerName": localConstant.gridHeader.TYPE,
            "field": "taxType",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.TYPE,
            "tooltip": (params) => {
                if (params.data.taxType === "S") {
                    return "Sales Tax";
                } else if(params.data.taxType === "W") {
                    return "Withholding Tax";
                }
            },
            "width": 250,
            "valueGetter": (params) => {
                if (params.data.taxType === "S") {
                    return "Sales Tax";
                } else if(params.data.taxType === "W") {
                    return "Withholding Tax";
                }
            }
        },
        {
            "headerName": localConstant.gridHeader.RATE,
            "field": "taxRate",
            "filter": "agNumericColumnFilter",
            "headerTooltip": localConstant.gridHeader.RATE,
            "tooltipField": "taxRate",
            "width": 150
        },
        {
            "headerName": "",
            "field": "companyTaxId",
            "cellRenderer": "EditRenderer",
            "cellRendererParams": {
                "action": "EditCompanyTaxes",
                "popupId": "addCompanyTaxes"
            },
            "suppressFilter": true,
            "suppressSorting": true,
            "width": 80
        }
    ],
    "enableFilter": true,
    "enableSorting": true,
    "rowSelection": "multiple",
    "gridHeight": 60,
    "clearFilter":true, 
    "exportFileName":"Company Taxes"
};