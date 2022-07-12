import { getlocalizeData } from '../../../../utils/commonUtils';
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
                "headerName": localConstant.gridHeader.REFERENCE_TYPE,
                "field": "referenceType",
                "filter": "agTextColumnFilter",
                "width":400,  
                "tooltipField":"referenceType",        
                "headerTooltip": localConstant.gridHeader.REFERENCE_TYPE,
                
            },
            {
                "headerName": localConstant.gridHeader.REFERENCE_VALUE,
                "field": "referenceValue",
                "filter": "agTextColumnFilter",
                "width":400,  
                "tooltipField":"referenceValue",        
                "headerTooltip": localConstant.gridHeader.REFERENCE_VALUE,
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
        "gridHeight":54,    
        "rowSelection":"multiple"
    };
};