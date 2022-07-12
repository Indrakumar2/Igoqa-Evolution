import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();
export const HeaderData = (functionRefs)=> {
    return {
    "columnDefs": [
        {
            "headerCheckboxSelectionFilteredOnly": true,
            "checkboxSelection": true,
            "suppressFilter": true,
            "width": 40
        },
        {
            "headerName": localConstant.gridHeader.CONTRACT_SCHEDULE_NAME,
            "field": "contractScheduleName",
            "filter": "agTextColumnFilter",
            "width":986,  
            "tooltipField":"contractScheduleName",        
            "headerTooltip": localConstant.gridHeader.CONTRACT_SCHEDULE_NAME,
            
        },
        {
            "headerName": "",
            "field": "EditColumn",
            "cellRenderer": "EditLink",
            "buttonAction":"",
            "cellRendererParams": {
                "disableField": (e) => functionRefs.enableEditColumn(e)
            },
            "suppressFilter": true,
            "suppressSorting": true,
            "width": 50
        }
    ],
    "enableSelectAll":true,    
    "enableSorting":true, 
    "pagination": false,    
    "gridHeight":54,    
    "clearFilter":true, 
    "rowSelection":"multiple"
};
};