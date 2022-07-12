import { getlocalizeData } from '../../../../utils/commonUtils';
import { requiredNumeric } from '../../../../utils/validator';
const localConstant = getlocalizeData();
export const HeaderData = {
    "columnDefs": [ 
        {
            "checkboxSelection": true,
            "headerCheckboxSelectionFilteredOnly": true,
            "suppressFilter": true,
            "width": 40
        },
        {
            "headerName": localConstant.gridHeader.COMPANY,
            "field": "companyName",
            "tooltipField":"companyName",        
            "headerTooltip": localConstant.gridHeader.COMPANY,
            "filter": "agTextColumnFilter",
            "width":120
        },
        {
            "headerName": localConstant.gridHeader.DESCRIPTION,
            "field": "description",
            "width":200,
            "tooltipField":"description",        
            "headerTooltip": localConstant.gridHeader.DESCRIPTION,
            "filter": "agTextColumnFilter",
        },
        {
            "headerName": localConstant.gridHeader.CURRENCY,
            "field": "currencyCode",               
            "width":120,
            "tooltipField":"currencyCode",        
            "headerTooltip": localConstant.gridHeader.CURRENCY,
            "filter": "agTextColumnFilter",
        },
        {
            "headerName": localConstant.gridHeader.EXPENSE_TYPE,
            "field": "expenseType",                      
            "width":150,
            "tooltipField":"expenseType",        
            "headerTooltip": localConstant.gridHeader.EXPENSE_TYPE,
            "filter": "agTextColumnFilter",
        },
        {
            "headerName": localConstant.gridHeader.EXPENSE_RATE,
            "field": "perUnitRate",
            "width": 120,
            "tooltip":(params) => {                    
                if (!requiredNumeric(params.data.perUnitRate)) {                        
                    return parseFloat(params.data.perUnitRate).toFixed(4);
                }
            },        
            "headerTooltip": localConstant.gridHeader.EXPENSE_RATE,
            "filter": "agNumberColumnFilter",
            "valueGetter": (params) => {                    
                if (!requiredNumeric(params.data.perUnitRate)) {                        
                    return parseFloat(params.data.perUnitRate).toFixed(4);
                }
            }
        },
        {
            "headerName": localConstant.gridHeader.UNITS,
            "field": "totalUnit",
            "width": 100,
            "tooltip":(params) => {                    
                if (!requiredNumeric(params.data.totalUnit)) {                        
                    return parseFloat(params.data.totalUnit).toFixed(2);
                }
            },            
            "headerTooltip": localConstant.gridHeader.UNITS,
            "filter": "agNumberColumnFilter",//D326
            "valueGetter": (params) => {                    
                if (!requiredNumeric(params.data.totalUnit)) {                        
                    return parseFloat(params.data.totalUnit).toFixed(2);
                }
            }
        },
        {
            "headerName": localConstant.gridHeader.ALREADY_LINKED,
            "field": "isAlreadyLinked",
            "width": 150,
            "tooltip":(params) => {                    
                if (params.data.isAlreadyLinked === true) {                        
                    return "Yes";
                } else {
                    return "No";
                }
            },        
            "headerTooltip": localConstant.gridHeader.ALREADY_LINKED,
            "filter": "agTextColumnFilter",
            "valueGetter": (params) => {                    
                if (params.data.isAlreadyLinked === true) {                        
                    return "Yes";
                } else {
                    return "No";
                }
            }
        },
        {
            "headerName": "",
            "field": "AdditionalExpensesRenderColumn",
            "cellRenderer": "EditLink",
            "buttonAction":"",
            "cellRendererParams":{
            },
            "suppressFilter": true,
            "suppressSorting": true,
            "width": 60
        }
    ],
    "enableSelectAll":true,    
    "pagination": true,
    "enableFilter":true, 
    "enableSorting":true, 
    "gridHeight":59,
    "searchable":true,
    "gridActions":true,
    "gridTitlePanel":true,
    "clearFilter":true, 
    "rowSelection": "multiple",
    "exportFileName":"Additional Expenses"
};

export default HeaderData;