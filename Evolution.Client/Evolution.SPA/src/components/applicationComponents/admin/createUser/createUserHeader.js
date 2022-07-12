import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();
export const HeaderData = {

    "columnDefs": [
        {
            //"headerName": localConstant.security.GRANTED,
            //"headerTooltip": localConstant.security.GRANTED,
            //"field": "isSelected",
            "headerCheckboxSelectionFilteredOnly": true,
            "checkboxSelection": true,
            "suppressFilter": true,
            "width": 70,
            "cellRenderer": (params) => {
                params.node.setSelected(params.node.data.isSelected);
            }
        },
        {
            "headerName":  localConstant.security.ROLE_NAME,
            "headerTooltip": localConstant.security.ROLE_NAME,
            "field": "roleName",
            "tooltipField":"roleName",   
            "filter": "agTextColumnFilter",
            "width":537
        },
        {
            "headerName":localConstant.security.DESCRIPTION,
            "headerTooltip": localConstant.security.DESCRIPTION,
            "field": "description",
            "tooltipField":"description",   
            "filter": "agTextColumnFilter",
            "width":700
        }
    ],
    "enableSelectAll":false,    
    "enableFilter": true,
    "enableSorting": true,
    "pagination": false,
    "searchable": true,
    "gridTitlePanel": true,    
    "gridHeight":30,
    "clearFilter":true, 
    "rowSelection": 'multiple'
};