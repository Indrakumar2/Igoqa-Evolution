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
            "width": 70
        },
        {
            "headerName": localConstant.security.ROLE_NAME,
            "headerTooltip": localConstant.security.ROLE_NAME,
            "field": "roleName",
            "tooltipField":"roleName",   
            "filter": "agTextColumnFilter",
            //"cellRenderer": "HyperLinkRenderer",
            "width":400,
            
        },
        {
            "headerName":localConstant.security.DESCRIPTION,
            "headerTooltip": localConstant.security.DESCRIPTION,
            "field": "description",
            "tooltipField":"description",   
            "filter": "agTextColumnFilter",
            "width":650,
           
        },
        {
            "headerName": "",
            "field": "EditColumn",
            "cellRenderer": "EditLink",
            "buttonAction": "",
            "cellRendererParams": {
            },
            "suppressFilter": true,
            "suppressSorting": true,
            "width": 80
        },
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
};