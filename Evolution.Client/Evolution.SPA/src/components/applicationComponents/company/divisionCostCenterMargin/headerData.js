import { getlocalizeData } from '../../../../utils/commonUtils';

const localConstant = getlocalizeData();
export const headData ={
    "columnDefs": [
        {
            "checkboxSelection": true,
            "headerCheckboxSelectionFilteredOnly": true,
            "suppressFilter":true,
            "width":40
        },
        {
            "headerName": localConstant.gridHeader.COST_CENTRE_CODE,
            "headerTooltip": localConstant.gridHeader.COST_CENTRE_CODE,
            "field": "costCenterCode",
            "tooltipField": "costCenterCode",
            "filter": "agTextColumnFilter",
            "width":300
        },
        {
            "headerName": localConstant.gridHeader.COST_CENTRE_NAME,
            "headerTooltip": localConstant.gridHeader.COST_CENTRE_NAME,
            "field": "costCenterName",
            "filter": "agTextColumnFilter",
            "tooltipField": "costCenterName",
            "width":665
        },
        {
            "headerName": "",
            "field": "id",
            "cellRenderer": "EditRenderer",
            "cellRendererParams": {
                "action": "EditCompanyDivisionCostcentre",
                "popupId": "createCostcenter"
            },
            "suppressFilter": true,
            "suppressSorting": true,
            "width": 87
        }
    ],
    "enableSelectAll":true,
    "enableFilter":true, 
    "enableSorting":true, 
    "pagination": true,
    "searchable":true,
    "gridHeight":40,
    "rowSelection":"multiple",
    "clearFilter":true, 
    "exportFileName":"Division Cost Center"
};