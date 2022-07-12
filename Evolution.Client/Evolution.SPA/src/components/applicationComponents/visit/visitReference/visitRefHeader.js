import { isUndefined,getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';

const localConstant = getlocalizeData();
export const HeaderData = (functionRefs) => {
    return {
        "columnDefs": [
            {
                "headerCheckboxSelectionFilteredOnly": true,
                "checkboxSelection": true,
                "suppressFilter": true,
                "width": 40
            },
            {
                "headerName": localConstant.timesheet.REFERENCE_TYPE,
                "headerTooltip": localConstant.visit.REFERENCE_TYPE,
                "field": "referenceType",
                "tooltipField":"referenceType",   
                "filter": "agTextColumnFilter",
                "cellRenderer": "timesheetAnchor",
                "width":450
            },
            {
                "headerName":  localConstant.timesheet.REFERENCE_VALUE,
                "headerTooltip": localConstant.visit.REFERENCE_VALUE,
                "field": "referenceValue",
                "tooltipField":"referenceValue",   
                "filter": "agTextColumnFilter",
                "width":564
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
        
        "enableSorting":true, 
        "pagination": false,    
        "enableFilter": true,
        "gridHeight":54,    
        "rowSelection":"multiple"
    };
};