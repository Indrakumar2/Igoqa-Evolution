import { getlocalizeData } from '../../../../utils/commonUtils';

const localConstant = getlocalizeData();
export const HeaderData = {
    "columnDefs": [
        {
            "checkboxSelection": true,
            "headerCheckboxSelectionFilteredOnly": true,
            "suppressFilter":true,
            "width":40
        },
        {
            "headerName": localConstant.gridHeader.BUSINESS_UNIT,
            "headerTooltip": localConstant.gridHeader.BUSINESS_UNIT,
            "field": "marginType",
            "tooltipField": "marginType",
            "filter": "agTextColumnFilter",
            "width":500
        },
        {
            "headerName": localConstant.gridHeader.MINIMUM_EXPECTED_MARGIN,
            "headerTooltip": localConstant.gridHeader.MINIMUM_EXPECTED_MARGIN,
            "field": "minimumMargin",
            "tooltipField": "minimumMargin",
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "width":477
        }, 
        {
            "headerName": "",
            "field": "",
            "cellRenderer": "EditRenderer",
            "cellRendererParams": {
                "action": "EditExpectedMargin",
                "popupId": "add-expected-margin"
            },
            "suppressFilter": true,
            "width": 80
        }
    ],
    "defaultColDef": {
        "width": 80
      },
    "enableSelectAll":true,
    "enableFilter":true, 
    "enableSorting":true,
    "rowSelection": "multiple", 
    "pagination": true,
    "searchable":true,
    "clearFilter":true, 
    "gridHeight":50,
    "exportFileName":"Expected Margin By Business"
};