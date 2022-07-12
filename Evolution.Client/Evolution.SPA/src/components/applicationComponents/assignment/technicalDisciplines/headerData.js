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
            "headerName": localConstant.gridHeader.CATEGORY,
            "field": "category",
            "filter": "agTextColumnFilter",
            "width":250,  
            "tooltipField":"category",        
            "headerTooltip": localConstant.gridHeader.CATEGORY,
            
        },
        
        {
            "headerName": localConstant.gridHeader.SUB_CATEGORY,
            "field": "subCategory",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.SUB_CATEGORY,
            "width":250,
            "tooltipField":"subCategory"
        },
        {
            "headerName": localConstant.gridHeader.SERVICE,
            "field": "services",
            "filter": "agNumberColumnFilter", 
            "headerTooltip": localConstant.gridHeader.SERVICE,
            "width":250,
            "tooltipField":"services",
        },
        {
            "headerName": "",
            "field": "EditColumn",
            "cellRenderer": "EditLink",
            "buttonAction":"",
            "cellRendererParams": {
            },
            "suppressFilter": true,
            "suppressSorting": true,
            "width": 50
        }
    ],    
    "enableSorting":true, 
    "pagination": false,    
    "gridHeight":54, 
    "clearFilter":true,    
    "rowSelection":"multiple"
};