import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import { requiredNumeric } from '../../../../utils/validator';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData = {
    "ContractFixedExchangeRatesHeader": {
        "columnDefs": [
            {
                "checkboxSelection": true,
                "headerCheckboxSelectionFilteredOnly": true,
                "suppressFilter": true,
                "width": 40
            },
            {
                "headerName": localConstant.gridHeader.FROM_CURRENCY,
                "headerTooltip": localConstant.gridHeader.FROM_CURRENCY,
                "field": "fromCurrency",
                "tooltipField": "fromCurrency",
                "cellRenderer": "medalCellRenderer",
                "filter": "agTextColumnFilter"
            },
            {
                "headerName": localConstant.gridHeader.TO_CURRENCY,
                "headerTooltip": localConstant.gridHeader.TO_CURRENCY,
                "field": "toCurrency",
                "tooltipField": "toCurrency",
                "filter": "agTextColumnFilter"
            },
            {
                "headerName": localConstant.gridHeader.EFFECTIVE_DATE,
                "headerTooltip": localConstant.gridHeader.EFFECTIVE_DATE,
                "field": "effectiveFrom",
                "tooltip": (params) => {
                    return moment(params.data.effectiveFrom).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
            }, 
                "filter": "agDateColumnFilter",
                "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "dataToRender": "effectiveFrom"
                },
                "filterParams": {
                    "comparator": dateUtil.comparator,
                    "inRangeInclusive": true,
                    "browserDatePicker": true
                  }
            },
            {
                "headerName":localConstant.gridHeader.EXCHANGE_RATE,
                "headerTooltip": localConstant.gridHeader.EXCHANGE_RATE,
                "field":"exchangeRate",
                "tooltip":(params) => {                    
                    if (!requiredNumeric(params.data.exchangeRate)) {                        
                        return parseFloat(params.data.exchangeRate).toFixed(6);
                    }
                }, 
                "filter": "agNumberColumnFilter",
                "valueGetter": (params) => {                    
                    if (!requiredNumeric(params.data.exchangeRate)) {                        
                        return parseFloat(params.data.exchangeRate).toFixed(6);
                    }
                },
                "width": 237
            },
            {
                "headerName": "",
             //   "field": "exchangeRateId",
                "cellRenderer": "EditRenderer",
                "cellRendererParams": {
                    "action": "EditFixedExchangeRate",
                    "popupId": "",
                    "popupAction": "ExchangeRateEditCheck",
                    "buttonAction": "ExchangeRateModalState"
                },
                "suppressFilter": true,
                "suppressSorting": true,
                "width": 185
            }
        ],
        "enableSelectAll":true,
        "enableFilter": true,
        "enableSorting": true,
        "pagination": true,
        "searchable": true,
        "gridActions": true,
        "gridTitlePanel": true,
        "gridHeight": 55,
        "rowSelection": "multiple",
        "clearFilter":true, 
        "exportFileName":"Contract Fixed Exchange Rates",
        "columnsFiltersToDestroy":[ 'effectiveFrom' ]
    }
};