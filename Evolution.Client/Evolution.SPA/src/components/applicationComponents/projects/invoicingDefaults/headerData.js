import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();

export const HeaderData = {    
    "ProjectInvoicingDefaultHeader": { 
        "columnDefs": [
            {
                "checkboxSelection": true,
                "headerCheckboxSelectionFilteredOnly": true,
                "suppressFilter": true,
                "width": 40
            },            
            {
                "headerName": localConstant.project.invoicingDefaults.REFERENCE_TYPE,
                "field": "referenceType",
                "filter": "agTextColumnFilter",
                "rowDrag":true,
                "headerTooltip": localConstant.project.invoicingDefaults.REFERENCE_TYPE,
                "tooltipField": "referenceType",
                "width": 170
            },
            {
                "headerName": localConstant.project.invoicingDefaults.IS_ASSIGNMENT,
                "field": "isVisibleToAssignment",
                "tooltip": (params) => {                               
                    if (params.data.isVisibleToAssignment === true) {                        
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "headerTooltip": localConstant.project.invoicingDefaults.IS_ASSIGNMENT,
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
                "headerName": localConstant.project.invoicingDefaults.IS_VISIT,
                "field": "isVisibleToVisit",
                "tooltip": (params) => {                    
                    if (params.data.isVisibleToVisit === true) {                        
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "headerTooltip": localConstant.project.invoicingDefaults.IS_VISIT,
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
                "headerName": localConstant.project.invoicingDefaults.IS_TIMESHEET,
                "field": "isVisibleToTimesheet",
                "tooltip": (params) => {                    
                    if (params.data.isVisibleToTimesheet === true) {                        
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "headerTooltip": localConstant.project.invoicingDefaults.IS_TIMESHEET,
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
                "field": "EditInvoiceRenderColumn",
                "cellRenderer": "EditLink",
                "buttonAction":"",
                "cellRendererParams": {
                },
                "suppressFilter": true,
                "suppressSorting": true,
                "width": 60
            },  
        ],
        "enableSelectAll":true,
        "pagination": false,
        "animatedRows":true,
        "rowDragManaged":true,
        "rowSelection": "multiple",
        "gridHeight": 40,
        "clearFilter":true, 
        "exportFileName":"Invoicing Defaults"
    },

    "ProjectAttachmentTypesHeader": {
        "columnDefs": [
            {
                "checkboxSelection": true,
                "headerCheckboxSelectionFilteredOnly": true,
                "suppressFilter": true,
                "width": 40
            },
            {
                "headerName": localConstant.project.invoicingDefaults.DOCUMENT_TYPE,
                "headerTooltip": localConstant.project.invoicingDefaults.DOCUMENT_TYPE,
                "field": "documentType",
                "filter": "agTextColumnFilter",
                "tooltipField": "documentType",   
                "width": 310
            },
            {
                "headerName": "",
                "field": "EditInvoiceAttachmentTypeColumn",
                "cellRenderer": "EditLink",
                "buttonAction":"",
                "cellRendererParams": {                    
                },
                "suppressFilter": true,
                "suppressSorting": true,
                "width": 60
            },       
        ],
        "enableSelectAll":true,
        "enableSorting": true,
        "pagination": false,
        "rowSelection": "multiple",
        "gridHeight": 40,
        "clearFilter":true, 
        "exportFileName":"Attachemnt Types"
    }
};