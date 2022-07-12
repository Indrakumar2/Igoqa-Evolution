import { getlocalizeData } from '../../../../utils/commonUtils';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
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
            "headerName": localConstant.gridHeader.REFERENCE_TYPE,
            "field": "referenceType",
            "filter": "agTextColumnFilter",
            "width":400,  
            "tooltipField":"referenceType",        
            "headerTooltip": localConstant.gridHeader.REFERENCE_TYPE,
            "cellRenderer": "InlineSelect",
            "cellRendererParams": {                          
                    "optionName":"referenceType",
                    "optionValue":"referenceType",
                    "name":"referenceType",
                    "disabled":functionRefs.disabled,
                    'isWithoutSelect':true,
                    "id":"assignmentReferenceType",
            },
            "suppressKeyboardEvent": suppressArrowKeys    
            
        },
        {
            "headerName": localConstant.gridHeader.REFERENCE_VALUE,
            "field": "referenceValue",
            "filter": "agTextColumnFilter",
            "width":560,  
            // "editable":!functionRefs.disabled,
            "tooltipField":"referenceValue",        
            "headerTooltip": localConstant.gridHeader.REFERENCE_VALUE,
            "cellRenderer": "InlineTextbox",
            "cellRendererParams": {                          
                    "disabled":functionRefs.disabled,
                    "validationProp": "referenceValueValidation",
                    "id":"referenceValue",
                    "name":"referenceValue",
                    "maxLength": fieldLengthConstants.assignment.assignmentReferences.REFERENCE_VALUE_MAXLENGTH,
            },
            "suppressKeyboardEvent": suppressArrowKeys
        }
    ],
    "enableSelectAll":true,    
    "enableSorting":true, 
    "pagination": false,    
    "gridHeight":54,   
    "clearFilter":true,  
    "rowSelection":"multiple"
    };
    function suppressArrowKeys(params) {
        const KEY_LEFT = 37;
        const KEY_UP = 38;
        const KEY_RIGHT = 39;
        const KEY_DOWN = 40;
        const event = params.event;
        const key = event.which;
        let suppress = false;
        if (key === KEY_RIGHT || key === KEY_DOWN || key === KEY_LEFT || key === KEY_UP) {
            suppress = true;
        }
        return suppress;
    }
};