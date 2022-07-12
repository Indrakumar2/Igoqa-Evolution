import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const payrollHeaderData = {
    "columnDefs": [
        {
            "checkboxSelection": true,
            "headerCheckboxSelectionFilteredOnly": true,
            "suppressFilter":true,
            "width":40
        },
        {
            "headerName": localConstant.gridHeader.PERIOD_NAME,
            "field": "periodName",
            "tooltipField": "periodName",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.PERIOD_NAME,
            "width":235,
            //"sort": "asc",  //Changed same like Evo      
        },
        {
            "headerName": localConstant.gridHeader.START_DATE,
            "field": "startDate",
            "tooltip": (params) => {
                return moment(params.data.startDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filter": "agDateColumnFilter", 
            "headerTooltip": localConstant.gridHeader.START_DATE,          
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "startDate"
            },
            "width":300,
            "sort":"desc",
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },
        {
            "headerName": localConstant.gridHeader.END_DATE,
            "field": "endDate",
            "tooltip": (params) => {
                return moment(params.data.endDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filter": "agDateColumnFilter",  
            "headerTooltip": localConstant.gridHeader.END_DATE,         
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "endDate"
            },
            "width":200,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },        
        {
            "headerName": localConstant.gridHeader.HIDDEN,
            "field": "isActive",
            "tooltip": (params) => {
                if(params.data.isActive === true){
                    return "No"; 
                }else{
                    return "Yes";        
                }
              },
            "filter": "agTextColumnFilter", 
            "headerTooltip": localConstant.gridHeader.HIDDEN,
            "valueGetter": (params) => {
                if(params.data.isActive === true){
                    return "No"; 
                }else{
                    return "Yes";
                }
                
              },
            "width":237
        },
        //Fix for D1480
        {
            "headerName": "",
            //"field": "id",
            "cellRenderer": "EditRenderer",
            "cellRendererParams": {
                "action": "EditPayrollPeriodName",
                "popupId": "createPayrollPeriod"
            },
            "suppressFilter": true,
            "suppressSorting": true,
            "width": 50
        }
    ],
    "enableSelectAll":true,
    "enableFilter":true, 
    "enableSorting":true,
    "gridHeight":40,
    "pagination": true,
    "searchable":true,
    "gridActions":true,
    "gridTitlePanel":true,
    "rowSelection":"multiple",
    "clearFilter":true, 
    "exportFileName":"Payroll Periods",
    "columnsFiltersToDestroy":[ 'startDate','endDate' ]
};
export const sellReferrenceHeaderData = {
    "columnDefs": [
        {
            "headerName": localConstant.gridHeader.NAME,
            "headerTooltip": localConstant.gridHeader.NAME,
            "field": "name",
            "tooltipField": "name",
            "filter": "agTextColumnFilter",
            "width":325,
            "sort":"asc"
            
        },
        {
            "headerName": localConstant.gridHeader.TYPE,
            "headerTooltip": localConstant.gridHeader.TYPE,
            "field": "chargeType",
            "tooltip": (params) => {
                if (params.data.chargeType === 'T')
                    return "Travel";
                else if (params.data.chargeType === 'R')
                    return "Rate";
                else if (params.data.chargeType === 'C')
                    return 'Consumable';
                else if (params.data.chargeType === 'E')
                    return 'Expense';
                else if (params.data.chargeType === 'Q')
                    return 'Equipment '; 
              },
            "filter": "agTextColumnFilter",
            "width":400,
            "valueGetter": (params) => {
                if (params.data.chargeType === 'T')
                    return "Travel";
                else if (params.data.chargeType === 'R')
                    return "Rate";
                else if (params.data.chargeType === 'C')
                    return 'Consumable';
                else if (params.data.chargeType === 'E')
                    return 'Expense';
                else if (params.data.chargeType === 'Q')
                    return 'Equipment '; 
              }
        },
        {
            "headerName": localConstant.gridHeader.CHARGE_REFERENCE,
            "headerTooltip": localConstant.gridHeader.CHARGE_REFERENCE,
            "field": "chargeReference",
            "tooltipField": "chargeReference",
            "filter": "agTextColumnFilter",
            "width":340
        }
    ],
    "enableFilter":true, 
    "enableSorting":true, 
    "pagination": true,
    "gridHeight":59,
    "searchable":true,
    "gridActions":true,
    "gridTitlePanel":true,
    "clearFilter":true, 
    "exportFileName":"Cost Of Sales Reference"
};