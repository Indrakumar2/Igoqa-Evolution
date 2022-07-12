import { getlocalizeData } from '../../../utils/commonUtils';
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
            "headerName":localConstant.commonConstants.CODE,
            "headerTooltip":localConstant.commonConstants.CODE,
            "field": "customerCode",
            "tooltipField": "customerCode",
            "filter": "agTextColumnFilter",
            "width":118
        },
        {
            "headerName": localConstant.commonConstants.NAME,
            "headerTooltip":localConstant.commonConstants.NAME,
            "field": "customerName",
            "tooltipField": "customerName",
            "filter": "agTextColumnFilter",
            "width":595
        }
    ],
    "enableFilter": false,
    "clearFilter":true, 
    "enableSorting": true,
    "pagination": false,
    "gridHeight": 30,
    "rowSelection": 'single',
    
};