import { getlocalizeData } from '../../../../utils/commonUtils';

const localConstant = getlocalizeData();
export const HeaderData = {
    "columnDefs": [
        {
            "headerCheckboxSelectionFilteredOnly": true,
            "checkboxSelection": true,
            "suppressFilter": true,
            "width": 40
        },
        {
            "headerName": localConstant.gridHeader.CODE,
            "headerTooltip": localConstant.gridHeader.CODE,
            "field": "customerCode",
            "tooltipField": "customerCode",
            "filter": "agTextColumnFilter",
            "width": 300
        },
        {
            "headerName": localConstant.gridHeader.NAME,
            "headerTooltip": localConstant.gridHeader.NAME,
            "field": "customerName",
            "tooltipField": "customerName",
            "filter": "agTextColumnFilter",
            "width": 365
        }
    ],
    "enableFilter": false,
    "enableSorting": true,
    "pagination": false,
    "gridHeight": 45,
    "clearFilter":true, 
    "exportFileName":"General Details"
};