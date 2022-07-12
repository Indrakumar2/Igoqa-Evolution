import { getlocalizeData } from '../../../../utils/commonUtils';

const localConstant = getlocalizeData();

export const HeaderData = (functionRefs) => {
    return {
    "subSupplierHeader" : {
        "columnDefs": [ 
            {
                "headerCheckboxSelectionFilteredOnly": true,
                "checkboxSelection": true,
                "suppressFilter": true,
                "width": 40
            },
            {
                "field": "supplierId",
                "hide":true
            },    
            {
                "headerName": localConstant.assignments.SUB_SUPPLIER,
                "field": "subSupplierName",
                "filter": "agTextColumnFilter",
                "cellRenderer": "supplierAnchorRenderer",
                "cellRendererParams": {
                    "supplierIdKey" : "supplierId",
                    "supplierNameKey" : "subSupplierName"
                },
                "headerTooltip": localConstant.assignments.SUB_SUPPLIER,
                "tooltipField": "subSupplierName",
                "width": 300
            },

            {
                "headerName": localConstant.assignments.PART_OF_ASSIGNMENT,
                "field": "isPartofAssignment",
                "headerTooltip": localConstant.assignments.PART_OF_ASSIGNMENT,
                "width": 300,
                "cellRenderer": "InlineSwitch",
                "cellRendererParams": {
                    "isSwitchLabel": false,
                    "name":"isPartofAssignment",
                    "id":"isPartofAssignment",
                    "validationProp"  :"partOfAssignmentValidation",
                    "switchClass":"",
                    "disabled":functionRefs.isSubSupplierDisable(),
                },
                "tooltip": (params) => {                               
                            if (params.data.isPartofAssignment === true) {                        
                                return "Yes";
                            } else {
                                return "No";
                            }
                        },
                        "valueGetter": (params) => {                               
                                    if (params.data.isPartofAssignment === true) {                        
                                        return "Yes";
                                    } else {
                                        return "No";
                                    }
                                }, 
            },

            {
                "headerName":  localConstant.assignments.SUPPLIER_CONTACT,
                "field": "subSupplierContactName",
                "filter": "agTextColumnFilter",
                "width":250,  
                "tooltipField":"subSupplierContactName",        
                "headerTooltip":  localConstant.assignments.SUPPLIER_CONTACT,
                "cellRenderer": "InlineSelect",
                "cellRendererParams":{                  
                        "optionName":"supplierContactName",
                        "optionValue":"supplierContactId",
                        "name":"subSupplierContactId",
                        "optionListProps": "supplierContacts",
                        "optionSelecetLabel":"Select",
                        "validationProp":"supplierContactValidation",
                        "isDisabled":"isSupplierContactDisabled",
                        "id":"subsupplierContactId",
                        "disabled":functionRefs.isSubSupplierContactDisable(),
                },
                "suppressKeyboardEvent": suppressArrowKeys            
            },

            {
                "headerName": localConstant.assignments.FIRST_VISIT,
                "field": "isSubSupplierFirstVisit",
                "headerTooltip": localConstant.assignments.FIRST_VISIT,
                "cellRenderer": "InlineSwitch",
                "cellRendererParams": {
                    "isSwitchLabel": false,
                    "name":"isSubSupplierFirstVisit",
                    "id":"isSubSupplierFirstVisit",
                    "switchClass":"",
                    "isDisabled":"isFirstVisitDisabled",
                    "disabled":functionRefs.isSubSupplierContactDisable(),
                },
                "tooltip":  (params) => {                               
                    if (params.data.isSubSupplierFirstVisit === true) {                        
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "suppressFilter": true,
                "width": 150,
                "valueGetter": (params) => {                               
                    if (params.data.isSubSupplierFirstVisit === true) {                        
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
            },
        ],
        "enableFilter": true,
        "enableSorting": true,
        "pagination": false,
        "rowSelection": "single",
        "clearFilter":true, 
        "gridHeight": 40
    },
    "technicalSpecialistHeader":{
        "columnDefs": [           
            {
                "headerName": localConstant.assignments.TECHNICAL_SPECIALIST,
                "field": "technicalSpecialistName",
                "filter": "agTextColumnFilter",
                "headerTooltip": localConstant.assignments.TECHNICAL_SPECIALIST,
                "tooltipField": "technicalSpecialistName",
                "valueGetter": (params) => {                               
                    if (params.data.technicalSpecialistName)                       
                        return `${ params.data.technicalSpecialistName } (${ params.data.epin })`;
                },
                "width": 400
            },
            {
                "headerName": localConstant.assignments.ASSIGNED_TO_THIS_SUB_SUPPLIER,
                "field": "isAssignedToThisSubSupplier",
                "tooltipField": "isAssignedToThisSubSupplier",
                "headerTooltip": localConstant.assignments.ASSIGNED_TO_THIS_SUB_SUPPLIER,
                "headerCheckboxSelectionFilteredOnly": true,
                "checkboxSelection": true,
                "cellStyle": () =>functionRefs.isAssignedtoThisSupplierDisable() ? { 'pointer-events': 'none' } : '',
                "cellRenderer": (params) => {
                    if(params.node.data.isAssignedToThisSubSupplier === true) { 
                        params.node.setSelected(true);
                    }
                },
                "suppressFilter": true,
                "width": 250
            },
            {
                "headerName": '',  
                "headerTooltip": localConstant.assignments.ASSIGNMENT_RESOURCE_REPORT,
                "field": "epin",                      
                "tooltipField": localConstant.assignments.ASSIGNMENT_RESOURCE_REPORT,           
                "cellRenderer": "PrintReport",
                "suppressFilter": true,
                "suppressSorting": true,
                 "width": 150
            }
        ],
        "enableSorting": true,
        "pagination": false,
        "rowSelection": "multiple",
        "clearFilter":true, 
        "gridHeight": 40
    }      
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