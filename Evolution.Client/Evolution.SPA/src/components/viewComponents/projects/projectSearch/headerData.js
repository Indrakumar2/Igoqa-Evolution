import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();
export const headerData = {
    "columnDefs": [
        {
            "headerName": localConstant.project.PROJECT_NO,
            "field": "projectNumber",
            "cellRenderer": "ProjectAnchor",
            "valueGetter": (params) => (params.data.projectNumber),
            "filter": "agNumberColumnFilter",          
            "filterParams": {
                "inRangeInclusive": true
            },
            "headerTooltip": localConstant.project.PROJECT_NO,
            "tooltip":(params) => (params.data.projectNumber),
            "width": 200
        },
        {
            "headerName": localConstant.customer.CUSTOMER_TITLE,
            "field": "contractCustomerName",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.customer.CUSTOMER_TITLE,
            "tooltipField":"contractCustomerName",
            "width": 200            
        },
        {
            "headerName": localConstant.project.CUSTOMER_PROJECT_NO,
            "field": "customerProjectNumber",
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive": true
            },
            "width": 200,
            "headerTooltip": localConstant.project.CUSTOMER_PROJECT_NO,
            "tooltipField":"customerProjectNumber",
        },
        {
            "headerName": localConstant.project.CUSTOMER_PROJECT_NAME,
            "field": "customerProjectName",
            "filter": "agTextColumnFilter",
            "width": 205,                  
            "headerTooltip": localConstant.project.CUSTOMER_PROJECT_NAME,
            "tooltipField":"customerProjectName",
        },
        {
            "headerName": localConstant.project.CONTRACT_NO,
            "field": "contractNumber",
            "filter": "agTextColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "width": 140,
            "headerTooltip": localConstant.project.CONTRACT_NO,
            "tooltipField":"contractNumber",
        },
        {
            "headerName": localConstant.project.COORDINATOR,
            "field": "projectCoordinatorName",
            "filter": "agTextColumnFilter",
            "width": 150,
            "headerTooltip": localConstant.project.COORDINATOR,
            "tooltipField":"projectCoordinatorName",
        },
        {
            "headerName": localConstant.project.COMPANY_NAME,
            "field": "contractHoldingCompanyName",
            "filter": "agTextColumnFilter",
            "width": 200,  
            "sort": "asc",  
            "headerTooltip": localConstant.project.COMPANY_NAME,
            "tooltipField":"contractHoldingCompanyName",
        },
        {
            "headerName": localConstant.gridHeader.STATUS,
            "field": "projectStatus",
            "width": 105,
            "headerTooltip": localConstant.gridHeader.STATUS,
            "tooltip":(params) => {
                if (params.data.projectStatus === 'O') {
                    return "Open";
                } else {
                    return "Closed";    //Sanity Defect Fix
                }
            },
            "valueGetter": (params) => {
                if (params.data.projectStatus === 'O') {
                    return "Open";
                } else {
                    return "Closed";    //Sanity Defect Fix
                }
            }
        },
        {
            "headerName": localConstant.gridHeader.DIVISION,
            "field": "companyDivision",
            "filter": "agTextColumnFilter",
            "width": 140,
            "headerTooltip": localConstant.gridHeader.DIVISION,
            "tooltipField":"companyDivision",
        },
        {
            "headerName": localConstant.gridHeader.OFFICE,
            "field": "companyOffice",
            "filter": "agTextColumnFilter",
            "width": 100,
            "headerTooltip": localConstant.gridHeader.OFFICE,
            "tooltipField":"companyOffice",
        },
        {
            "headerName": localConstant.gridHeader.EREPT_PROJECT_MAPPED,
            "field": "isEReportProjectMapped",
            "filter": "agTextColumnFilter",
            "width": 200,
            "valueGetter": (params) => {
                if (params.data.isEReportProjectMapped === true) {
                    return "Yes";
                } else {
                    return "No";
                }
            },
            "headerTooltip": localConstant.gridHeader.EREPT_PROJECT_MAPPED,
            "tooltip":(params) => {
                if (params.data.isEReportProjectMapped === true) {
                    return "Yes";
                } else {
                    return "No";
                }
            },
        }
    ],
    "rowSelection": 'single',
    "pagination": true,
    "enableFilter": true,
    "enableSorting": true,
    "gridHeight": 59,
    "searchable": true,
    "gridActions": true,
    "gridTitlePanel": true,
    "clearFilter":true, 
    "exportFileName": "Project Search Data"
};

export const customerSearchHeaderData = {

    "columnDefs": [
        {
            "headerCheckboxSelectionFilteredOnly": true,
            "checkboxSelection": true,
            "suppressFilter": true,
            "width": 40,
        },
        {
            "headerName": localConstant.companyDetails.generalDetails.CODE,
            "field": "customerCode",
            "filter": "agTextColumnFilter",
            "width": 300,
            "headerTooltip": localConstant.companyDetails.generalDetails.CODE,
            "tooltipField":"customerCode",
        },
        {
            "headerName": localConstant.companyDetails.generalDetails.NAME,
            "field": "customerName",
            "filter": "agTextColumnFilter",
            "width": 340,
            "headerTooltip": localConstant.companyDetails.generalDetails.NAME,
            "tooltipField":"customerName",
        }
    ],
    "enableFilter": false,
    "enableSorting": true,
    "pagination": false,
    "gridHeight": 45,
    "rowSelection": 'single',
    "clearFilter":true, 
    "exportFileName": "Customer Search"

};