import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
import { customComparator } from '../../../../utils/arrayUtil';
const localConstant = getlocalizeData();
export const HeaderData = (data) => {
    return {
        "columnDefs": [
            {
                "checkboxSelection": true,
                "headerCheckboxSelectionFilteredOnly": true,
                "suppressFilter": true,
                "width": 33
            },
            {
                "headerName": localConstant.gridHeader.CHARGE_TYPE,
                "field": "chargeType",
                "tooltipField": "chargeType",
                "filter": "agTextColumnFilter",
                "width": 144,
                "headerTooltip": localConstant.gridHeader.CHARGE_TYPE,
                "cellClassRules": {
                    "chargeTypeText": function (params) {
                        return (params.data.chargeRateType === "C" || params.data.chargeRateType === "Q");
                    }
                },
                //"sort": "asc",
                "cellRenderer": "SelectContractRate",
                "name": "chargeType",
                "type": "select",
                "selectListName": "contractChargeTypes",
                "optionName": "name",
                "optionValue": "name",
                "cellRendererParams": (params) => {
                    return { rowIndex: params.node.rowIndex, validation: params.data.chargeTypeValidation, readonly: params.data.readonly, gridApi: params.api };
                },
                "moduleType": 'chargeRate',
                "suppressKeyboardEvent": suppressArrowKeys
            },
            {
                "headerName": localConstant.gridHeader.STANDARD_VALUE,
                "field": "standardValue",
                "tooltipField": "standardValue",
                "filter": "agNumberColumnFilter",
                "width": 90,
                "headerTooltip": localConstant.gridHeader.STANDARD_VALUE,
                "cellRenderer": "SelectContractRate",
                "name": "standardValue",
                "type": "text",
                "decimalType": "2",
                "cellRendererParams": (params) => {
                    return { rowIndex: params.node.rowIndex, readonly: true };
                },
                "moduleType": 'chargeRate',
                "suppressKeyboardEvent": suppressArrowKeys
            },
            {
                "headerName": localConstant.gridHeader.CHARGE_VALUE,
                "field": "chargeValue",
                "tooltipField": "chargeValue",
                "filter": "agNumberColumnFilter",
                "width": 124,
                "headerTooltip": localConstant.gridHeader.CHARGE_VALUE,
                "cellRenderer": "SelectContractRate",
                "name": "chargeValue",
                "type": "text",
                "cellRendererParams": (params) => {
                    return { 
                        rowIndex: params.node.rowIndex, 
                        validation: params.data.chargeValueValidation, 
                        readonly: params.data.readonly, 
                        gridApi: params.api };
                },
                "moduleType": 'chargeRate',
                "maxLength": 21,
                "suppressKeyboardEvent": suppressArrowKeys
            },
            {
                "headerName": localConstant.gridHeader.DESCRIPTION,
                "field": "description",
                "tooltipField": "description",
                "filter": "agTextColumnFilter",
                "width": 273,
                "headerTooltip": localConstant.gridHeader.DESCRIPTION,
                "cellRenderer": "SelectContractRate",
                "name": "description",
                "type": "textarea",
                "comparator": customComparator,//Added for D990 - inline sort
                "cellRendererParams": (params) => {
                    return { 
                        rowIndex: params.node.rowIndex,
                         readonly: params.data.readonly, 
                         gridApi: params.api };
                },
                "moduleType": 'chargeRate',
                "maxLength": 100,
                "suppressKeyboardEvent": suppressArrowKeys
            },
            {
                "headerName": localConstant.gridHeader.DISCOUNT_APPLIED,
                "field": "discountApplied",
                // "tooltipField": "discountApplied",
                "filter": "agNumberColumnFilter",
                "width": 89,
                "headerTooltip": localConstant.gridHeader.DISCOUNT_APPLIED,
                "cellClassRules": {
                    "chargeTypeText": function (params) {
                        return (parseFloat(params.data.discountApplied) < 0);
                    }
                },
                "tooltipField": "discountApplied",
                // "valueGetter": (params) => {
                //     return params.data.discountApplied ? params.data.discountApplied.toFixed(2) : 0.00;
                //     // if(params.data.discountApplied === 0){
                //     //     return params.data.discountApplied.toFixed(2);
                //     // }
                //     // else{
                //     //     return params.data.discountApplied.toFixed(2);
                //     // }
                // },
                // "tooltip": (params) => {
                //     return params.data.discountApplied ? params.data.discountApplied.toFixed(2) : 0.00;
                //     // if(params.data.discountApplied === 0){
                //     //     return params.data.discountApplied.toFixed(2);
                //     // }
                //     // else{
                //     //     return params.data.discountApplied.toFixed(2);
                //     // }
                // },
                "cellRenderer": "SelectContractRate",
                "name": "discountApplied",
                "type": "text",
                "cellRendererParams": (params) => {
                    return { rowIndex: params.node.rowIndex, readonly: true };
                },
                "moduleType": 'chargeRate',
                "maxLength": 20,
                "suppressKeyboardEvent": suppressArrowKeys
            },
            {
                "headerName": "%",
                "field": "percentage",
                "tooltipField": "percentage",
                // "tooltip": (params) => {
                //     return params.data.percentage ? params.data.percentage.toFixed(2) : 0.00;
                //     // if(params.data.percentage === 0){
                //     //     return params.data.percentage.toFixed(2);
                //     // }
                //     // else{
                //     //     return params.data.percentage.toFixed(2);
                //     // }
                // },
                "filter": "agNumberColumnFilter",
                "width": 59,
                "headerTooltip": localConstant.gridHeader.PERCENTAGE,
                // "valueGetter": (params) => {
                //     return params.data.percentage ? params.data.percentage.toFixed(2) : 0.00;
                //     // if(params.data.percentage === 0){
                //     //     return params.data.percentage.toFixed(2);
                //     // }
                //     // else{
                //     //     return params.data.percentage.toFixed(2);
                //     // }
                // },
                "cellClassRules": {
                    "chargeTypeText": function (params) {
                        return (parseFloat(params.data.percentage) < 0);
                    }
                },
                "cellRenderer": "SelectContractRate",
                "name": "percentage",
                "type": "text",
                "decimalType": "2",
                "cellRendererParams": (params) => {
                    return { rowIndex: params.node.rowIndex, readonly: true };
                },
                "moduleType": 'chargeRate',
                "maxLength": 20,
                "suppressKeyboardEvent": suppressArrowKeys
            },
            {
                "headerName": localConstant.gridHeader.PRINT_DESCRIPTION_ON_INVOICE,
                "field": "isDescriptionPrintedOnInvoice",
                "tooltip": (params) => {
                    if (params.data.isDescriptionPrintedOnInvoice === true) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "filter": "agTextColumnFilter",
                "width": 76,
               "valueGetter": (params) => {
                    if (params.data.isDescriptionPrintedOnInvoice === true) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "headerTooltip": localConstant.gridHeader.PRINT_DESCRIPTION_ON_INVOICE,
                "cellRenderer": "SelectContractRate",
                "name": "isDescriptionPrintedOnInvoice",
                "type": "switch",
                "cellRendererParams": (params) => {
                    return { rowIndex: params.node.rowIndex, readonly: params.data.readonly, gridApi: params.api };
                },
                "moduleType": 'chargeRate',
                "suppressKeyboardEvent": suppressArrowKeys
            },
            {
                "headerName": localConstant.gridHeader.EFFECTIVE_FROM,
                "field": "effectiveFrom",
                "tooltip": (params) => {
                    return params.data.effectiveFrom && moment(params.data.effectiveFrom).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                },
                "filter": "agDateColumnFilter",
                "width": 125,
                "headerTooltip": localConstant.gridHeader.EFFECTIVE_FROM,
                //"sort": "asc",
                "filterParams": {
                    "comparator": dateUtil.comparator,
                    "inRangeInclusive": true,
                    "browserDatePicker": true
                },
                "cellRenderer": "SelectContractRate",
                "name": "effectiveFrom",
                "type": "date",
                "cellRendererParams": (params) => {
                    // return { rowIndex: params.node.rowIndex, validation: params.data.effectiveFromValidation, readonly: params.data.readonly, gridApi: params.api, editAction: (e) => data.refreshGridAfterDateSelected(e) };
                    return { rowIndex: params.node.rowIndex, validation: params.data.effectiveFromValidation, readonly: params.data.readonly, gridApi: params.api };
                },
                "moduleType": 'chargeRate',
                "suppressKeyboardEvent": suppressArrowKeys
            },
            {
                "headerName": localConstant.gridHeader.EFFECTIVE_TO,
                "field": "effectiveTo",
                "tooltip": (params) => {
                    return params.data.effectiveTo && moment(params.data.effectiveTo).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                },
                "filter": "agDateColumnFilter",
                "width": 125,
                "headerTooltip": localConstant.gridHeader.EFFECTIVE_TO,
                "filterParams": {
                    "comparator": dateUtil.comparator,
                    "inRangeInclusive": true,
                    "browserDatePicker": true
                },
                "cellRenderer": "SelectContractRate",
                "name": "effectiveTo",
                "type": "date",
                "cellRendererParams": (params) => {
                    // return { rowIndex: params.node.rowIndex, readonly: params.data.readonly, gridApi: params.api,editAction: (e) => data.refreshGridAfterDateSelected(e) };
                    return { rowIndex: params.node.rowIndex, readonly: params.data.readonly, gridApi: params.api };

                },
                "moduleType": 'chargeRate',
                "suppressKeyboardEvent": suppressArrowKeys
            },
            {
                "headerName": localConstant.gridHeader.HIDE,
                "field": "isActive",
                // "tooltipField": "isActive",
                "filter": "agTextColumnFilter",
                "width": 79,
                "valueGetter": (params) => {
                    if (params.data.isActive === true) {
                        return "No";
                    } else {
                        return "Yes";
                    }
                },
                "tooltip": (params) => {
                    if (params.data.isActive === true) {
                        return "No";
                    } else {
                        return "Yes";
                    }
                },
                "headerTooltip": localConstant.gridHeader.HIDDEN,
                "cellRenderer": "SelectContractRate",
                "name": "isActive",
                "type": "switch",
                "cellRendererParams": (params) => {
                    return { rowIndex: params.node.rowIndex, readonly: params.data.readonly, gridApi: params.api };
                },
                "moduleType": 'chargeRate',
                "suppressKeyboardEvent": suppressArrowKeys
            },
        ],
        "enableSelectAll": true,
        "enableFilter": true,
        "enableSorting": true,
        "gridHeight": 40,
        "pagination": true,
        "searchable": true,
        "gridActions": true,
        "rowSelection": "multiple",
        "gridTitlePanel": true,
        "clearFilter": true,
        "exportFileName": "Charge Rates",
        "columnsFiltersToDestroy": [
            'effectiveFrom', 'effectiveTo'
        ]
    };
};

export const CopyScheduleHeader = {
    "columnDefs": [
        {
            "checkboxSelection": true,
            "headerCheckboxSelectionFilteredOnly": true,
            "suppressFilter": true,
            "width": 40
        },
        {
            "headerName": localConstant.gridHeader.SCHEDULE_NAME,
            "field": "scheduleName",
            "tooltipField": "scheduleName",
            "filter": "agTextColumnFilter",
            "width": 230,
            "headerTooltip": localConstant.gridHeader.SCHEDULE_NAME
        },
        {
            "headerName": localConstant.gridHeader.SCHEDULE_NAME_FOR_INVOICE_PRINT,
            "field": "scheduleNameForInvoicePrint",
            "tooltipField": "scheduleNameForInvoicePrint",
            "filter": "agTextColumnFilter",
            "width": 300,
            "headerTooltip": localConstant.gridHeader.SCHEDULE_NAME_FOR_INVOICE_PRINT
        },
        {
            "headerName": localConstant.gridHeader.CHARGE_CURRENCY,
            "field": "chargeCurrency",
            "tooltipField": "chargeCurrency",
            "filter": "agTextColumnFilter",
            "width": 377,
            "headerTooltip": localConstant.gridHeader.CHARGE_CURRENCY
        },
    ],
    "enableFilter": true,
    "enableSorting": true,
    "rowSelection": "multiple",
    "gridHeight": 30,
    "pagination": true,
    "searchable": true,
    "gridActions": true,
    "gridTitlePanel": true,
    "clearFilter": true,
    "exportFileName": "Copy Schedules"
    // "defaultColDef": { "editable": true },
};

export const DeleteScheduleHeader = {
    "columnDefs": [
        {
            "checkboxSelection": true,
            "headerCheckboxSelectionFilteredOnly": true,
            "suppressFilter": true,
            "width": 40
        },
        {
            "headerName": localConstant.gridHeader.SCHEDULE_NAME,
            "field": "scheduleName",
            "tooltipField": "scheduleName",
            "filter": "agTextColumnFilter",
            "width": 230,
            "headerTooltip": localConstant.gridHeader.SCHEDULE_NAME
        },
        {
            "headerName": localConstant.gridHeader.SCHEDULE_NAME_FOR_INVOICE_PRINT,
            "field": "scheduleNameForInvoicePrint",
            "tooltipField": "scheduleNameForInvoicePrint",
            "filter": "agTextColumnFilter",
            "width": 300,
            "headerTooltip": localConstant.gridHeader.SCHEDULE_NAME_FOR_INVOICE_PRINT
        },
        {
            "headerName": localConstant.gridHeader.CHARGE_CURRENCY,
            "field": "chargeCurrency",
            "tooltipField": "chargeCurrency",
            "filter": "agTextColumnFilter",
            "width": 377,
            "headerTooltip": localConstant.gridHeader.CHARGE_CURRENCY
        },
    ],
    "enableFilter": true,
    "enableSorting": true,
    "rowSelection": "multiple",
    "gridHeight": 30,
    "pagination": true,
    "searchable": true,
    "gridActions": true,
    "gridTitlePanel": true,
    "clearFilter": true,
    "exportFileName": "Deleted Charge Schedules"
};

export const ViewChargeRateHeader = {
    "columnDefs": [
        {
            "headerName": localConstant.gridHeader.CHARGE_TYPE,
            "field": "chargeType",
            "tooltipField": "chargeType",
            "filter": "agTextColumnFilter",
            "width": 230,
            "headerTooltip": localConstant.gridHeader.CHARGE_TYPE
        },
        {
            "headerName": localConstant.gridHeader.CHARGE_VALUE,
            "field": "chargeValue",
            "tooltipField": "chargeValue",
            "filter": "agTextColumnFilter",
            "width": 200,
            "headerTooltip": localConstant.gridHeader.CHARGE_VALUE
        },
        {
            "headerName": localConstant.gridHeader.EFFECTIVE_FROM,
            "field": "effectiveFrom",
            "tooltip": (params) => {
                return moment(params.data.effectiveFrom).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
            },
            "cellRenderer": "DateComponent",
            "filter": "agDateColumnFilter",
            "cellRendererParams": {
                "dataToRender": "effectiveFrom"
            },
            "width": 120,
            "headerTooltip": localConstant.gridHeader.EFFECTIVE_FROM,
            "sort": "asc",
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
            }
        },
        {
            "headerName": localConstant.gridHeader.EFFECTIVE_TO,
            "field": "effectiveTo",
            "tooltip": (params) => {
                return moment(params.data.effectiveTo).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
            },
            "cellRenderer": "DateComponent",
            "filter": "agDateColumnFilter",
            "cellRendererParams": {
                "dataToRender": "effectiveTo"
            },
            "width": 100,
            "headerTooltip": localConstant.gridHeader.EFFECTIVE_TO,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
            }
        },
        {
            "headerName": localConstant.gridHeader.DESCRIPTION,
            "field": "description",
            "tooltipField": "description",
            "filter": "agTextColumnFilter",
            "width": 230,
            "headerTooltip": localConstant.gridHeader.DESCRIPTION
        },
    ],
    "enableFilter": false,
    "enableSorting": true,
    "rowSelection": "multiple",
    "gridHeight": 30,
    "pagination": false,
    "searchable": false,
    "gridActions": false,
    "clearFilter": true,
    "gridTitlePanel": false,
    // "exportFileName":"Copy Schedule",
    "columnsFiltersToDestroy": [
        'effectiveFrom', 'effectiveTo'
    ]
};

export const RateScheduleNameHeader = {
    "columnDefs": [
        {
            "checkboxSelection": true,
            "headerCheckboxSelectionFilteredOnly": true,
            "cellRenderer": (params) => {
                //console.log(index);
                const localValue = sessionStorage.getItem("selectedSchedule"); //Added for D789
                if (localValue) {
                    const localSchedule = JSON.parse(localValue);
                    if (!localSchedule) {
                        if (params.node.data) {
                            if (params.node.data.isSelected)
                                params.node.setSelected(params.node.data.isSelected);
                        }
                    }
                    else {
                        if (localSchedule.scheduleId === params.node.data.scheduleId && localSchedule.contractNumber === params.node.data.contractNumber) {
                            params.node.data.isSelected = true;
                            params.node.setSelected(params.node.data.isSelected);
                        }
                    }
                }
                else {
                    if (params.node.data) {
                        if (params.node.data.isSelected)
                            params.node.setSelected(params.node.data.isSelected);
                    }
                }
            },
            "suppressFilter": true,
            "width": 40
        },
        {
            "headerName": localConstant.contract.rateSchedule.SCHEDULE_NAME,
            "field": "scheduleName",
            "tooltipField": "scheduleName",
            "filter": "agTextColumnFilter",
            "width": 300,
            "headerTooltip": localConstant.contract.rateSchedule.SCHEDULE_NAME,
            "cellRenderer": "SelectContractRate",
            "name": "scheduleName",
            "type": "textbox",
            "cellRendererParams": (params) => {
                return { rowIndex: params.node.rowIndex, validation: params.data.scheduleNameValidation, readonly: params.data.readonly };
            },
            "moduleType": 'chargeSchedule',
            "maxLength": 200,
            "suppressKeyboardEvent": suppressArrowKeys
        },
        {
            "headerName": localConstant.contract.rateSchedule.NAME_PRINTED_ON_INVOICE,
            "field": "scheduleNameForInvoicePrint",

            "tooltipField": "scheduleNameForInvoicePrint",
            "filter": "agTextColumnFilter",
            "width": 300,
            "headerTooltip": localConstant.contract.rateSchedule.NAME_PRINTED_ON_INVOICE,
            "cellRenderer": "SelectContractRate",
            "name": "scheduleNameForInvoicePrint",
            "type": "textbox",
            "cellRendererParams": (params) => {
                return { rowIndex: params.node.rowIndex, readonly: params.data.readonly };
            },
            "moduleType": 'chargeSchedule',
            "maxLength": 20,
            "suppressKeyboardEvent": suppressArrowKeys
        },
        {
            "headerName": localConstant.contract.rateSchedule.CURRENCY,
            "field": "chargeCurrency",
            "tooltipField": "chargeCurrency",
            "filter": "agTextColumnFilter",
            "width": 340,
            "headerTooltip": localConstant.contract.rateSchedule.CURRENCY,
            "cellRenderer": "SelectContractRate",
            "name": "chargeCurrency",
            "type": "select",
            "selectListName": "currency",
            "optionName": "code",
            "optionValue": "code",
            "cellRendererParams": (params) => {
                return { rowIndex: params.node.rowIndex, validation: params.data.chargeCurrencyValidation, readonly: params.data.readonly };
            },
            "moduleType": 'chargeSchedule',
            "suppressKeyboardEvent": suppressArrowKeys
        },
    ],
    "enableFilter": true,
    "enableSorting": true,
    "gridHeight": 40,
    "pagination": true,
    "clearFilter": true,
    "searchable": true,
    "gridActions": true,
    "exportFileName": "Charge Rate Schedules",
};
export const AdminContractRatesHeaderData = {
    "columnDefs": [
        // {
        //     "checkboxSelection": true,
        //     "headerCheckboxSelectionFilteredOnly": true,
        //     "suppressFilter": true,
        //     "width": 50
        // },
        {
            "headerName": localConstant.gridHeader.ITEM_DESCRIPTION,
            "field": "name",
            "tooltipField": "name",
            "filter": "agTextColumnFilter",
            "width": 200,
            "headerTooltip": localConstant.gridHeader.ITEM_DESCRIPTION
        },
        {
            "headerName": localConstant.gridHeader.ITEM_SIZE,
            "field": "itemSize",
            "tooltipField": "itemSize",
            "filter": "agTextColumnFilter",
            "width": 100,
            "headerTooltip": localConstant.gridHeader.ITEM_SIZE
        },
        {
            "headerName": localConstant.gridHeader.ITEM_THICKNESS,
            "field": "itemThickness",
            "tooltipField": "itemThickness",
            "filter": "agTextColumnFilter",
            "width": 100,
            "headerTooltip": localConstant.gridHeader.ITEM_THICKNESS
        },
        {
            "headerName": localConstant.gridHeader.FLIM_SIZE,
            "field": "filmSize",
            "tooltipField": "filmSize",
            "filter": "agTextColumnFilter",
            "width": 100,
            "headerTooltip": localConstant.gridHeader.FLIM_SIZE
        },
        {
            "headerName": localConstant.gridHeader.FLIM_TYPE,
            "field": "filmType",
            "tooltipField": "filmType",
            "filter": "agTextColumnFilter",
            "width": 100,
            "headerTooltip": localConstant.gridHeader.FLIM_TYPE
        },
        {
            "headerName": localConstant.gridHeader.TYPE,
            "field": "expenseType",
            "tooltipField": "expenseType",
            "filter": "agTextColumnFilter",
            "width": 100,
            "headerTooltip": "Type"
        },

        // {
        //     "field": "id",
        //     "tooltipField": "id",
        //     "headerName": "Choose Rate Type",
        //     "headerTooltip": "Choose Rate Type",
        //     "width": 50,
        //     "cellRenderer": "SelectChargeValue",
        // },
        // {
        //     // "checkboxSelection": true,
        //     // "cellRenderer":"CheckBoxRenderer",
        //     "headerName": localConstant.gridHeader.RATE_ONSHORE_OIL,
        //     "field": "rateOnShoreOil",
        //     "tooltipField": "rateOnShoreOil",
        //     "filter": "agTextColumnFilter",
        //     // "cellRendererParams": {
        //     //     "dataToRender": "rateOnShoreOil"
        //     // },
        //     "width":100,            
        //     "headerTooltip": localConstant.gridHeader.RATE_ONSHORE_OIL
        // },

        // {
        //     // "checkboxSelection": true,
        //     // "cellRenderer":"CheckBoxRenderer",
        //     "headerName": localConstant.gridHeader.RATE_ONSHORE,
        //     "field": "rateOnShoreNonOil",
        //     "tooltipField": "rateOnShoreNonOil",
        //     "filter": "agTextColumnFilter",
        //     // "cellRendererParams": {
        //     //     "dataToRender": "rateOnShoreNonOil"
        //     // },
        //     "width":100,
        //     "headerTooltip": localConstant.gridHeader.RATE_ONSHORE
        // },

        // {
        //     // "cellRenderer":"CheckBoxRenderer",
        //     // "checkboxSelection": true,
        //     "headerName": localConstant.gridHeader.RATE_OFFSHORE_OIL,
        //     "field": "rateOffShoreOil",
        //     "tooltipField": "rateOffShoreOil",
        //     "filter": "agTextColumnFilter",
        //     // "cellRendererParams": {
        //     //     "dataToRender": "rateOffShoreOil"
        //     // },
        //     "width": 120,
        //     "headerTooltip": localConstant.gridHeader.RATE_OFFSHORE_OIL
        // },
        {
            "headerName": localConstant.gridHeader.RATE_ONSHORE_OIL,
            "field": "rateOnShoreOil",
            "tooltipField": "rateOnShoreOil",
            "headerTooltip": localConstant.gridHeader.RATE_ONSHORE_OIL,
            "width": 200,
            "name": "isRateOnShoreOil",
            "cellRenderer": "InlineCheckbox",
            "labelName": "rateOnShoreOil",
            "cellRendererParams": {

            },
        },
        {
            "headerName": localConstant.gridHeader.RATE_ONSHORE,
            "field": "rateOnShoreNonOil",
            "tooltipField": "rateOnShoreNonOil",
            "headerTooltip": localConstant.gridHeader.RATE_ONSHORE,
            "width": 200,
            "name": "isRateOnShoreNonOil",
            "labelName": "rateOnShoreNonOil",
            "cellRenderer": "InlineCheckbox",
            "cellRendererParams": {

            },
        },
        {
            "headerName": localConstant.gridHeader.RATE_OFFSHORE_OIL,
            "tooltipField": "rateOffShoreOil",
            "headerTooltip": localConstant.gridHeader.RATE_ONSHORE_OIL,
            "field": "rateOffShoreOil",
            "width": 200,
            "name": "isRateOffShoreOil",
            "cellRenderer": "InlineCheckbox",
            "labelName": "rateOffShoreOil",
            "cellRendererParams": {

            },

        },
    ],
    "enableFilter": true,
    "enableSorting": true,
    "rowSelection": "multiple",
    "gridHeight": 30,
    "pagination": true,
    "searchable": true,
    "gridActions": true,
    "gridTitlePanel": true,
    "clearFilter": true,
    "pinnedTopRowData": [],
    "exportFileName": "Admin Contract Rates"
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