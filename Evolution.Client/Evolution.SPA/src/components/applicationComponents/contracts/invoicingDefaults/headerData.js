import { getlocalizeData } from '../../../../utils/commonUtils';

const localConstant = getlocalizeData();
export const HeaderData = {    
    "ContractInvoicingDefaultHeader": {
        "columnDefs": [
            {
                "checkboxSelection": true,
                "headerCheckboxSelectionFilteredOnly": true,
                "suppressFilter": true,
                "width": 40
            },
            // {
            //     "field":"",
            //     "rowDrag":true,
            //     "width":50,
            // },
            {
                "headerName": localConstant.gridHeader.REFERENCE_TYPE,
                "field": "referenceType",
                "filter": "agTextColumnFilter",
                 "rowDrag":true,
                "headerTooltip": localConstant.gridHeader.REFERENCE_TYPE,
                "tooltipField": "referenceType",
                "width": 160
            },
            {
                "headerName": localConstant.gridHeader.ASSIGNMENT,
                "field": "isVisibleToAssignment",
                "tooltip":  (params) => {                               
                    if (params.data.isVisibleToAssignment === true) {                        
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "headerTooltip": localConstant.gridHeader.ASSIGNMENT,
                "suppressFilter": true,
                "width": 150,
                "valueGetter": (params) => {                               
                    if (params.data.isVisibleToAssignment === true) {                        
                        return "Yes";
                    } else {
                        return "No";
                    }
                }
            },
            {
                "headerName": localConstant.gridHeader.VISIT,
                "field": "isVisibleToVisit",
                "headerTooltip": localConstant.gridHeader.VISIT,
                "tooltip": (params) => {                    
                    if (params.data.isVisibleToVisit === true) {                        
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "suppressFilter": true,
                "width": 100,
                "valueGetter": (params) => {                    
                    if (params.data.isVisibleToVisit === true) {                        
                        return "Yes";
                    } else {
                        return "No";
                    }
                }
            },
            {
                "headerName": localConstant.gridHeader.TIMESHEET,
                "field": "isVisibleToTimesheet",
                "headerTooltip":localConstant.gridHeader.TIMESHEET,
                "tooltip":  (params) => {                    
                    if (params.data.isVisibleToTimesheet === true) {                        
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "suppressFilter": true,
                "width": 130,
                "valueGetter": (params) => {                    
                    if (params.data.isVisibleToTimesheet === true) {                        
                        return "Yes";
                    } else {
                        return "No";
                    }
                }
            },
            {
                "headerName": "",
                "field": "",
                "cellRenderer": "EditRenderer",
                "cellRendererParams": {
                    "action": "EditInvoiceDefault",
                    "popupId": "",
                    "popupAction": "InvoiceReferenceEditCheck",
                    "buttonAction": "InvoiceReferenceModalState"
                },
                "suppressFilter": true,
                "suppressSorting": true,
                "width": 50
            },  
        ],
        "enableSelectAll":true,
        "pagination": false,
        "animatedRows":true,
        "rowDragManaged":true,
        "rowSelection": "multiple",
        "gridHeight": 40,
        "clearFilter":true, 
        "exportFileName":"Contract Invoicing Defaults"
    },

    "ContractAttachmentTypesHeader": {
        "columnDefs": [
            {
                "checkboxSelection": true,
                "headerCheckboxSelectionFilteredOnly": true,
                "suppressFilter": true,
                "width": 40
            },
            {
                "headerName": localConstant.gridHeader.DOCUMENT_TYPE,
                "headerTooltip": localConstant.gridHeader.DOCUMENT_TYPE,
                "field": "documentType",
                "filter": "agTextColumnFilter",
                "tooltipField": "documentType",   
                "width": 320
            },
            {
                "headerName": "",
                "field": "contractNumber",
                "tooltipField": "contractNumber",
                "cellRenderer": "EditRenderer",
                "cellRendererParams": {
                    "action": "EditAttachmentTypes",
                    "popupId": "",
                    "popupAction": "InvoiceAttachmentTypesEditCheck",
                    "buttonAction": "InvoiceAttachmentTypesModalState"
                },
                "suppressFilter": true,
                "suppressSorting": true,
                "width": 50
            },       
        ],
        "enableSelectAll":true,
        "enableSorting": true,
        "pagination": false,
        "rowSelection": "multiple",
        "gridHeight": 40,
        "clearFilter":true, 
        "exportFileName":"Contract Attachment Types"
    }
};