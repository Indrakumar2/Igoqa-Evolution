import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();
export const HeaderData = {
    "columnDefs": [
        {
            "headerName": localConstant.gridHeader.TASK_ID,
            "field": "assignmentProjectName",
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "width":300,  
            "tooltipField":"assignmentProjectName",      
            "cellRenderer": "HyperLink",  
            "headerTooltip": localConstant.gridHeader.TASK_ID,
            
        },
        {
            "headerName": localConstant.gridHeader.TASK,
            "field": "assignmentProjectName",
            "filter": "agNumberColumnFilter", 
            "filterParams": {
                "inRangeInclusive":true
            },
            "width":300,           
            "headerTooltip": localConstant.gridHeader.TASK,  
            "tooltipField":"assignmentProjectName",
        },
        {
            "headerName": localConstant.gridHeader.TYPE,
            "field": "assignmentProjectName",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.TYPE,
            "width":300,  
            "tooltipField":"assignmentProjectName"
        },
        {
            "headerName": localConstant.gridHeader.CREATED_ON,
            "field": "assignmentNumber",
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "headerTooltip": localConstant.gridHeader.CREATED_ON,
            "width":100,
            "tooltipField":"assignmentNumber",
        },
        {
            "headerName": localConstant.gridHeader.REASSIGN,
            "field": "assignmentCustomerName",            
            "filter": "agTextColumnFilter",
            "cellRenderer": "HyperLink",  
            "headerTooltip": localConstant.gridHeader.REASSIGN,
            "width":180,
            "tooltipField":"assignmentCustomerName",
        },
        
    ],
    "enableFilter":true, 
    "enableSorting":true, 
    "pagination": true,
    "searchable":true,
   // "gridActions":true,
    "gridTitlePanel":true,
     "gridHeight":56,
     "clearFilter":true, 
    "multiSortKey": "ctrl",
    "exportFileName":"My Tasks"
};