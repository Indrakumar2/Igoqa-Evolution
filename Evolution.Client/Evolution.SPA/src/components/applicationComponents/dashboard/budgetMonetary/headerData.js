import { thousandFormat } from '../../../../utils/commonUtils';
export const HeaderData={
  "columnDefs": [
    {
      "headerName": "Budget Monetary Grouping",
      "field": "groupName",
      "filter": "agTextColumnFilter",
      "cellRenderer": 'agGroupCellRenderer',
      "filterParams": {
          "inRangeInclusive":true
      },
      "headerTooltip": "Budget Monetary Grouping",
      "width": 230,
      "tooltipField": "groupName",
      "sort": "asc"
    },
    {
      "headerName": "Customer",
      "field": "contractCustomerName",
      "filter": "agTextColumnFilter",
      "headerTooltip": "Customer",
      "width": 210,
      "tooltipField": "contractCustomerName",
      "sort": "asc"
    },
    {
      "headerName": "Contract Number",
      "field": "contractNumber",
      "filter": "agTextColumnFilter",
      "filterParams": {
          "inRangeInclusive":true
      },
      "headerTooltip": "Contract Number",
      "width": 180,
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
      "tooltip":  (params) =>{ 
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
      "tooltip":(params) =>{ 
        return params.data.assignmentNumber === 0 ? '' : params.data.assignmentNumber.toString().padStart(5, '0');
      },
      "cellRenderer": "AssignmentAnchor",
     
      "cellRendererParams": (params) => {                
        return { assignmentNumber:params.data.assignmentNumber=== 0 ?'': params.data.assignmentNumber.toString().padStart(5, '0'),assignmentId: params.data.assignmentId };
     },  
      "sort": "asc"
    },
    
    {
      "headerName": "Value",
      "field": "budgetValue",
      "filter": "agNumberColumnFilter",
      "filterParams": {
          "inRangeInclusive":true
      },
      "headerTooltip": "Value",
      "width": 130,
      "valueGetter": (params) =>{ 
        return params.data.budgetValue === 0 ? '0': thousandFormat(params.data.budgetValue);
      },
      "tooltip":(params) =>{ 
        return params.data.budgetValue === 0 ? '0': thousandFormat(params.data.budgetValue);
      },
    },
    {
      "headerName": "Currency",
      "field": "budgetCurrency",
      "filter": "agTextColumnFilter",
      "headerTooltip": "Currency",
      "width": 128,
      "tooltipField": "budgetCurrency"
    },
    {
      "headerName": "Warning%",
      "field": "budgetWarning",
      "filter": "agTextColumnFilter",
      "headerTooltip": "Warning%",
      "width": 128,
      "tooltip":(params) =>{ 
        return params.data.budgetWarning === 0 ? '0': params.data.budgetWarning;
      },
    },
    {
      "headerName": "Invoiced To Date Excl Tax",
      "field": "invoicedToDate",
      "filter": "agDateColumnFilter",
      "headerTooltip": "Invoiced To Date Excl Tax",
      "valueGetter": (params) =>{ 
        return params.data.invoicedToDate === 0 ? '0.00': thousandFormat(params.data.invoicedToDate);
      },
      "width": 210,
      "tooltip": (params) =>{ 
        return params.data.invoicedToDate === 0 ? '0.00': thousandFormat(params.data.invoicedToDate);
      },
      "filterParams": {
        "browserDatePicker": true
      }
    },
    {
      "headerName": "Uninvoiced to Date Excl Tax",
      "field": "uninvoicedToDate",
      "filter": "agDateColumnFilter",
      "headerTooltip": "Uninvoiced to Date Excl Tax",
      "width": 227,
      "valueGetter": (params) =>{ 
        return params.data.uninvoicedToDate === 0 ? '0.00': thousandFormat(params.data.uninvoicedToDate);
      },
      "tooltip": (params) =>{ 
        return params.data.uninvoicedToDate === 0 ? '0.00': thousandFormat(params.data.uninvoicedToDate);
      },
      "filterParams": {
        "browserDatePicker": true
      }
    },
    {
      "headerName": "Remaining",
      "field": "remainingBudgetValue",
      "filter": "agTextColumnFilter",
      "headerTooltip": "Remaining",
      "valueGetter": (params) =>{ 
        return params.data.remainingBudgetValue === 0 ? '0.00': thousandFormat(params.data.remainingBudgetValue);
      },
      "width": 128,
      "tooltip": (params) =>{ 
        return params.data.remainingBudgetValue === 0 ? '0.00': thousandFormat(params.data.remainingBudgetValue);
      },
    }
  ],
  "enableFilter":true, 
  "enableSorting":true, 
  "pagination": true,
  "searchable":true,
  "gridActions":true,
  "gridTitlePanel":true,
  "exportFileName":"Budget Monetary",
  "Grouping":true,
  "clearFilter":true, 
  "GroupingDataName":"childArray"
};