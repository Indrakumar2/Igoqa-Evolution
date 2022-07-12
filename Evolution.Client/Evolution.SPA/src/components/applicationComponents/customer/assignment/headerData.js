import { getlocalizeData } from '../../../../utils/commonUtils';

const localConstant = getlocalizeData();
export const HeaderData={
    "CustomerAssignmentHeader": {
        "columnDefs": [
            {
                "checkboxSelection": true,
                "headerCheckboxSelectionFilteredOnly": true,
                "suppressFilter":true,
                "width":40
            },
            {
                "headerName": localConstant.gridHeader.ASSIGNMENT_REFERENCE_TYPE,
                "headerTooltip": localConstant.gridHeader.ASSIGNMENT_REFERENCE_TYPE,
                "field": "assignmentRefType",
                "tooltipField": "assignmentRefType",
                "filter": "agTextColumnFilter",                
                "width": 979
            },
            {
                "headerName": "",
                "field": "customerAssignmentReferenceId", 
                
                "width": 50,               
                "cellRenderer": "EditRenderer",
                "cellRendererParams": {
                    "action": "EditAssignmentReference",
                    "popupId": "assignmentReferenceType"
                },
                "suppressFilter": true,
                "suppressSorting": true
            }
        ],
        "enableSelectAll":true,
        "enableFilter": true,
        "enableSorting": true,
        "pagination": false,
        "rowSelection": "multiple",
        "gridHeight":40,
        "searchable":true,
        "clearFilter":true, 
        "exportFileName":"Assignment References"
    },
    "CustomerAccountRefrencesHeader": {
        "columnDefs": [
            {
                "checkboxSelection": true,
                "headerCheckboxSelectionFilteredOnly": true,
                "suppressFilter":true,
                "width":40
            },
            {
                "headerName": localConstant.gridHeader.COMPANY_NAME,
                "headerTooltip": localConstant.gridHeader.COMPANY_NAME,
                "field": "companyName",
                "tooltipField": "companyName",
                "filter": "agTextColumnFilter",                
                "width": 480
            },
            {
                "headerName": localConstant.gridHeader.ACCOUNTS_REFERENCE,
                "headerTooltip": localConstant.gridHeader.ACCOUNTS_REFERENCE,
                "field": "accountReferenceValue",
                "tooltipField": "accountReferenceValue",
                "filter": "agTextColumnFilter",
                "width": 499
            },
            {
                "headerName": "",
                "field": "customerCompanyAccountReferenceId",   
                "width": 50,             
                "cellRenderer": "EditRenderer",
                "cellRendererParams": {
                    "action": "EditAccountReference",
                    "popupId": "accountReferenceModal"
                },
                "suppressFilter": true,
                "suppressSorting": true
            }
        ],
        "enableSelectAll":true,
        "enableFilter": true,
        "enableSorting": true,
        "pagination": false,
        "rowSelection": "multiple",
        "gridHeight":40,
        "clearFilter":true, 
        "searchable":true,
        "exportFileName":"Account References"
    }
};