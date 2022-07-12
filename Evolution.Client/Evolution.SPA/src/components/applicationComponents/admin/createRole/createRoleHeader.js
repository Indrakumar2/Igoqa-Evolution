import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();
export const HeaderData = {

    "columnDefs": [
        {
            "headerCheckboxSelectionFilteredOnly": true,
            "checkboxSelection": true,
            "suppressFilter": true,
            "width": 70,
            "cellRenderer": (params) => {              
                params.node.setSelected(params.node.data.moduleAcivitySelected);
            }
        },
        {
            "headerName": localConstant.security.MODULE,
            "headerTooltip": localConstant.security.MODULE,
            "field": "moduleName",
            "tooltip":valueChange,  
            "filter": "agTextColumnFilter",
            "valueGetter": valueChange,    
            "width": 250
        },
        {
            "headerName":localConstant.security.SYSTEM_ROLE,
            "headerTooltip": localConstant.security.SYSTEM_ROLE,
            "field": "activityName",
            "tooltipField":"activityName",   
            "filter": "agTextColumnFilter",
            "width": 320
        },
        {
            "headerName":localConstant.security.DESCRIPTION,
            "headerTooltip": localConstant.security.DESCRIPTION,
            "field": "description",
            "tooltipField":"description",   
            "filter": "agTextColumnFilter",
            "width": 700
        }
    ],
    "enableSelectAll":true,    
    "rowSelection": 'multiple',
    "searchable":true,
    "gridActions":false,
    "gridTitlePanel":true,
    "gridHeight":56,
    "pagination": true,
    "enableFilter": true,
    "clearFilter":true, 
    "enableSorting": true,

    // "onRowClicked": "RowSelected",
    // "suppressRowClickSelection": true,
    // "enableRangeSelection": true,
    // "enableCellChangeFlash": true,
    // "rowSelection": 'multiple',
    // "rowData": null
};

function valueChange(value) {
    const { moduleName } = value.data;
    if (moduleName === localConstant.security.userRole.TECHNICAL_SPECIALIST) {
        return localConstant.resourceSearch.RESOURCE;
    } 
    return moduleName;
};