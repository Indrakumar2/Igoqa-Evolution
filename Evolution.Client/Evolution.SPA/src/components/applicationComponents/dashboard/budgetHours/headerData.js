import { thousandFormat } from '../../../../utils/commonUtils';
export const HeaderData={
    "columnDefs": [
        {
            "headerName": "Budget Hours Grouping",
            "field": "groupName",
            "filter": "agTextColumnFilter",
            "cellRenderer": 'agGroupCellRenderer',
            "filterParams": {
                "inRangeInclusive":true
            },
            "headerTooltip": "Budget Hours Grouping",
            "width": 230,
            "tooltipField": "groupName",
            "sort": "asc"
          },
        {
            "headerName": "Customer",
            "field": "contractCustomerName",
            "filter": "agTextColumnFilter",
            "headerTooltip": "Customer",
            "width": 250,
            "tooltipField": "contractCustomerName",
            "sort": "asc"
        },
        {
            "headerName": "Contract No",
            "field": "contractNumber",
            "filter": "agTextColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "headerTooltip": "Contract Number",
            "width": 220,
            "tooltipField": "contractNumber",
            "cellRenderer": "contractAnchorRenderer",
            "sort": "asc"
        },
        {
            "headerName": "Customer Contract Number",
            "field": "customerContractNumber",
            "filter": "agTextColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "headerTooltip": "Customer Contract Number",
            "width": 230,
            "tooltipField": "customerContractNumber"
        },
        {
            "headerName": "Project",
            "field": "projectNumber",
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "headerTooltip": "Project",
            "width": 130,
            "tooltip":(params) =>{ 
                return params.data.projectNumber === 0 ? '': params.data.projectNumber;
              },
            "cellRenderer": "projectAnchorRenderer",
            "valueGetter": (params) =>{ 
                return params.data.projectNumber === 0 ? '': params.data.projectNumber;
              },
            "sort": "asc"
        },
        {
            "headerName": "Assignment",
            "field": "assignmentNumber",
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "headerTooltip": "Assignment",
            "width": 150,
            "cellRenderer": "AssignmentAnchor",
     
            "cellRendererParams": (params) => {                
              return { assignmentNumber:params.data.assignmentNumber=== 0 ?'': params.data.assignmentNumber.toString().padStart(5, '0'),assignmentId: params.data.assignmentId };
           }, 
            "sort": "asc"
        },
        {
            "headerName": "Units",
            "field": "budgetHours",
            "filter": "agTextColumnFilter",
            "headerTooltip": "Units",
            "width": 100,
            "valueGetter": (params) =>{ 
              return params.data.budgetHours === 0 ? '0': thousandFormat(params.data.budgetHours);
            },
            "tooltip":(params) =>{ 
                return params.data.budgetHours === 0 ? '0': thousandFormat(params.data.budgetHours);
              },
        },
        {
            "headerName": "Warning%",
            "field": "budgetHoursWarning",
            "filter": "agTextColumnFilter",
            "headerTooltip": "Warning%",
            "width": 128,
            "tooltip":(params) =>{ 
                return params.data.budgetHoursWarning === 0 ? '0': params.data.budgetHoursWarning;
              },
        },
        {
            "headerName": "Invoiced To Date",
            "field": "hoursInvoicedToDate",
            "filter": "agDateColumnFilter",
            "valueGetter": (params) =>{ 
                return params.data.hoursInvoicedToDate === 0 ? '0.00': thousandFormat(params.data.hoursInvoicedToDate);
              },
            "headerTooltip": "Invoiced To Date",
            "width": 210,
            "tooltip":(params) =>{ 
                return params.data.hoursInvoicedToDate === 0 ? '0.00': thousandFormat(params.data.hoursInvoicedToDate);
              },
            "filterParams": {
                "browserDatePicker": true
              }
        },
        {
            "headerName": "Uninvoiced To Date",
            "field": "hoursUninvoicedToDate",
            "filter": "agDateColumnFilter",
            "valueGetter": (params) =>{ 
                return params.data.hoursUninvoicedToDate === 0 ? '0.00': thousandFormat(params.data.hoursUninvoicedToDate);
              },
            "headerTooltip": "Uninvoiced To Date",
            "width": 227,
            "tooltip": (params) =>{ 
                return params.data.hoursUninvoicedToDate === 0 ? '0.00': thousandFormat(params.data.hoursUninvoicedToDate);
              },
            "filterParams": {
                "browserDatePicker": true
              }
        },
        {
            "headerName": "Remaining",
            "field": "remainingBudgetHours",
            "filter": "agTextColumnFilter",
            "valueGetter": (params) =>{ 
                return params.data.remainingBudgetHours === 0 ? '0.00': thousandFormat(params.data.remainingBudgetHours);
              },
            "headerTooltip": "Remaining",
            "width": 150,
            "tooltip":  (params) =>{ 
                return params.data.remainingBudgetHours === 0 ? '0.00': thousandFormat(params.data.remainingBudgetHours);
              },
        }
    ],
    "enableFilter": true,
    "enableSorting": true,
    "pagination": true,
    "searchable": true,
    "gridActions": true,
    "gridTitlePanel": true,
    "gridHeight": 60,
    "exportFileName":"Budget Hours",
    "Grouping":true,
    "clearFilter":true, 
    "GroupingDataName":"childArray"
};