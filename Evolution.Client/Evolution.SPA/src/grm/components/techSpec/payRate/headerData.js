import { getlocalizeData, isEmpty } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
const localConstant = getlocalizeData();

export const HeaderData = () => {
    return {
        "PayRateHeader": {
            "columnDefs": [
                {
                    "checkboxSelection": true,
                    "headerCheckboxSelectionFilteredOnly": true,
                    "suppressFilter": true,
                    "width": 37
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.TYPE,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.TYPE,
                    "field": "expenseType",
                    "tooltipField": "expenseType",                    
                    "filter": "agTextColumnFilter",
                    "width": 136,
                    "cellRenderer": "SelectDataType",               
                    "name": "expenseType",
                    "type": "select",
                    "selectListName": "payRateType", //Changes for D1089 (Ref ALM 29-04-2020 Doc)
                    "optionName": "name",
                    "optionValue": "name",
                    "cellRendererParams": (params) => {                                
                        return { rowIndex: params.node.rowIndex, validation: params.data.expenseTypeValidation, readonly: params.data.readonly || params.data.payRateDisable };
                    },
                    "moduleType":'payRate',
                    "suppressKeyboardEvent": suppressArrowKeys
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.DESCRIPTION,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.DESCRIPTION,
                    "field": "description",
                    "tooltipField": "description",
                    "filter": "agTextColumnFilter",
                    "width": 144,
                    "cellRenderer": "SelectDataType",               
                    "name": "description",
                    "type": "textarea",
                    "cellRendererParams": (params) => {                                
                        return { rowIndex: params.node.rowIndex, readonly: params.data.readonly };
                    },
                    "moduleType":'payRate',
                    "maxLength": fieldLengthConstants.Resource.payRate.DESCRIPTION_TEXT_MAXLENGTH,
                    "suppressKeyboardEvent": suppressArrowKeys
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.RATE,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.RATE,
                    "field": "rate",                    
                    "tooltip": (params) => {
                        return params.data.rate === 'NaN' ? '' : params.data.rate;
                    },
                    "filter": "agNumberColumnFilter",
                    "width": 104,
                    "cellRenderer": "SelectDataType",               
                    "name": "rate",
                    "type": "text",
                    "cellRendererParams": (params) => {                                
                        return { rowIndex: params.node.rowIndex, validation: params.data.rateValidation, readonly: params.data.readonly || params.data.payRateDisable };
                    },
                    "moduleType":'payRate',                    
                    "suppressKeyboardEvent": suppressArrowKeys
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.DEFAULT_PAYRATE,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.DEFAULT_PAYRATE,
                    "field":"isDefaultPayRate",
                    "tooltip": (params) => {                               
                        if (params.data.isDefaultPayRate === true) {                        
                            return "Yes";
                        } else {
                            return "No";
                        }
                    },
                    "width": 103,
                    "filter": "agTextColumnFilter",
                    "valueGetter": (params) => {                               
                        if (params.data.isDefaultPayRate === true) {                        
                            return "Yes";
                        } else {
                            return "No";
                        }
                    },
                    "cellRenderer": "SelectDataType",               
                    "name": "isDefaultPayRate",
                    "id": "isDefaultPayRate",
                    "type": "switch",
                    "cellRendererParams": (params) => {                                
                        return { rowIndex: params.node.rowIndex, readonly: params.data.readonly };
                    },
                    "moduleType":'payRate',
                    "suppressKeyboardEvent": suppressArrowKeys
                        
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.EFFECTIVE_FROM,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.EFFECTIVE_FROM,
                    "field":"effectiveFrom",
                    "tooltip": (params) => {
                         //D664 #11
                        if(!isEmpty(params.data.effectiveFrom)){
                            return moment(params.data.effectiveFrom).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                        }
                    }, 
                    "filter": "agDateColumnFilter",
                    "width": 124,
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                    },
                    "cellRenderer": "SelectDataType",               
                    "name": "effectiveFrom",
                    "type": "date",
                    "cellRendererParams": (params) => {                                
                        return { rowIndex: params.node.rowIndex, validation: params.data.effectiveFromValidation, readonly: params.data.readonly || params.data.payRateDisable };
                    },
                    "moduleType":'payRate',
                    "suppressKeyboardEvent": suppressArrowKeys              
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.EFFECTIVE_TO,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.EFFECTIVE_TO,
                    "field":"effectiveTo",
                    "tooltip": (params) => {
                         //D664 #11
                        if(!isEmpty(params.data.effectiveTo)){
                            return moment(params.data.effectiveTo).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                        }
                    }, 
                    "filter": "agDateColumnFilter",
                    "width": 124,    
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                    },
                    "cellRenderer": "SelectDataType",               
                    "name": "effectiveTo",
                    "type": "date",
                    "cellRendererParams": (params) => {                                
                        return { rowIndex: params.node.rowIndex, validation: params.data.effectiveToValidation, readonly: params.data.readonly };
                    },
                    "moduleType":'payRate',
                    "suppressKeyboardEvent": suppressArrowKeys              
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.HIDE_RATE,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.HIDE_RATE,
                    "field":"isActive",
                    "tooltip": (params) => {                               
                        if (params.data.isActive === true) {                        
                            return "No";
                        } else {
                            return "Yes";
                        }
                    },
                    "width": 87,
                    "filter": "agTextColumnFilter",
                    "valueGetter": (params) => {                               
                        if (params.data.isActive === true) {                        
                            return "No";
                        } else {
                            return "Yes";
                        }
                    },
                    "cellRenderer": "SelectDataType",               
                    "name": "isActive",
                    "id": "isActiveRate",
                    "type": "switch",
                    "cellRendererParams": (params) => {                                
                        return { rowIndex: params.node.rowIndex, readonly: params.data.readonly };
                    },
                    "moduleType":'payRate',
                    "suppressKeyboardEvent": suppressArrowKeys
                          
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.HIDERATE_ON_TS_PORT,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.HIDERATE_ON_TS,
                    "field":"isHideOnTsExtranet", 
                    "tooltip": (params) => {                               
                        if (params.data.isHideOnTsExtranet === true) {                        
                            return "Yes";
                        } else {
                            return "No";
                        }
                    },
                    "width": 131,             
                    "filter": "agTextColumnFilter",
                    "valueGetter": (params) => {                               
                        if (params.data.isHideOnTsExtranet === true) {                        
                            return "Yes";
                        } else {
                            return "No";
                        }
                    },
                    "cellRenderer": "SelectDataType",               
                    "name": "isHideOnTsExtranet",
                    "id": "isHideOnTsExtranet",
                    "type": "switch",
                    "cellRendererParams": (params) => {                                
                        return { rowIndex: params.node.rowIndex, readonly: params.data.readonly };
                    },
                    "moduleType":'payRate',
                    "suppressKeyboardEvent": suppressArrowKeys
                  
                },                
            ],
            "enableSelectAll":true,
            "enableFilter": true,
            "enableSorting": true,
            "pagination": true, //testing team suggestion
    
            "searchable": false,
            "gridActions": true,
            "gridTitlePanel": false,
            "gridHeight": 50, //testing team suggestion
            "rowSelection": "multiple",
            "clearFilter":true, 
            "exportFileName":"Pay Rate Details",
            "columnsFiltersToDestroy":[ 'effectiveFrom','effectiveTo' ]
        },
    
        "payScheduleHeader":{
            "columnDefs": [
                {
                    "checkboxSelection": true,
                    "headerCheckboxSelectionFilteredOnly": true,
                    "cellRenderer": (params) => {
                        if (params.node.data) {
                            if (params.node.data.isSelected)
                                params.node.setSelected(params.node.data.isSelected);
                        }
                    },
                    "suppressFilter": true,
                    "width": 37
                },
                {
                    "headerName": localConstant.techSpec.PayRate.PAY_RATE_SCEDULES,
                    "headerTooltip": localConstant.techSpec.PayRate.PAY_RATE_SCEDULES,
                    "field": "payScheduleName",
                    "tooltipField": "payScheduleName",
                    "filter": "agTextColumnFilter",
                    "width": 250,
                    "cellRenderer": "SelectDataType",               
                    "name": "payScheduleName",
                    "type": "textbox",
                    "cellRendererParams": (params) => {                                
                        return { rowIndex: params.node.rowIndex, validation: params.data.payScheduleNameValidation, readonly: params.data.readonly };
                    },
                    "moduleType":'paySchedule',
                    "maxLength": fieldLengthConstants.Resource.payRate.PAY_RATE_SCEDULEE_MAXLENGTH,
                    "suppressKeyboardEvent": suppressArrowKeys
                },
                {
                    "headerName":localConstant.techSpec.PayRate.CURRENCY,
                    "headerTooltip": localConstant.techSpec.PayRate.CURRENCY,
                    "field": "payCurrency",
                    "tooltipField": "payCurrency",
                    "filter": "agTextColumnFilter",
                    "width": 140,
                    "cellRenderer": "SelectDataType",               
                    "name": "payCurrency",
                    "type": "select",
                    "selectListName": "currency",
                    "optionName": "code",
                    "optionValue": "code",
                    "cellRendererParams": (params) => {                                
                        return { rowIndex: params.node.rowIndex, validation: params.data.payCurrencyValidation, readonly: params.data.readonly };
                    },
                    "moduleType":'paySchedule',
                    "suppressKeyboardEvent": suppressArrowKeys
                },
                {
                    "headerName":localConstant.techSpec.PayRate.SCHEDULE_NOTES,
                    "headerTooltip":localConstant.techSpec.PayRate.SCHEDULE_NOTES,
                    "field": "payScheduleNote",
                    "tooltipField": "payScheduleNote",
                    "filter": "agTextColumnFilter",
                    "width": 350,
                    "cellRenderer": "SelectDataType",               
                    "name": "payScheduleNote",
                    "type": "textarea",
                    "cellRendererParams": (params) => {                                
                        return { rowIndex: params.node.rowIndex, readonly: params.data.readonly };
                    },
                    "moduleType":'paySchedule',
                    "maxLength": fieldLengthConstants.Resource.payRate.SCHEDULE_NOTES_MAXLENGTH,
                    "suppressKeyboardEvent": suppressArrowKeys
                },
                {
                    "headerName":localConstant.techSpec.PayRate.HIDE_RATE,
                    "headerTooltip": localConstant.techSpec.PayRate.HIDE_RATE,
                    "field":"isActive",
                    "tooltip": (params) => {                               
                        if (params.data.isActive === true) {                        
                            return "No";
                        } else {
                            return "Yes";
                        }
                    },
                    "width": 200,
                    "filter": "agTextColumnFilter",
                    "valueGetter": (params) => {                               
                        if (params.data.isActive === true) {                        
                            return "No";
                        } else {
                            return "Yes";
                        }
                    },
                    "cellRenderer": "SelectDataType",               
                    "name": "isActive",
                    "id": "isActive",
                    "type": "switch",
                    "cellRendererParams": (params) => {                                
                        return { rowIndex: params.node.rowIndex, readonly: params.data.readonly };
                    },
                    "moduleType":'paySchedule',
                    "suppressKeyboardEvent": suppressArrowKeys                        
                },
            ],
            "enableFilter":true, 
            "enableSorting":true,
            "gridHeight":40,
            "pagination": true,
            "clearFilter":true, 
            "gridActions": true,
            "exportFileName":"Pay Rate Schedules",//Added for D909
            "searchable":true,
        },
    
        'deletePayScheduleHeader':{
    
                "columnDefs": [
                    {
                        "checkboxSelection": true,
                        "headerCheckboxSelectionFilteredOnly": true,
                        "suppressFilter": true,
                        "width": 40
                    },
                    {
                        "headerName": localConstant.techSpec.PayRate.PAY_RATE_SCEDULES,
                        "headerTooltip": localConstant.techSpec.PayRate.PAY_RATE_SCEDULES,
                        "field": "payScheduleName",
                        "tooltipField": "payScheduleName",
                        "filter": "agTextColumnFilter",
                        "width": 180
                    },
                    {
                        "headerName":localConstant.techSpec.PayRate.CURRENCY,
                        "headerTooltip": localConstant.techSpec.PayRate.CURRENCY,
                        "field": "payCurrency",
                        "tooltipField": "payCurrency",
                        "filter": "agTextColumnFilter",
                        "width": 130
                    },
                    {
                        "headerName":localConstant.techSpec.PayRate.SCHEDULE_NOTES,
                        "headerTooltip":localConstant.techSpec.PayRate.SCHEDULE_NOTES,
                        "field": "payScheduleNote",
                        "tooltipField": "payScheduleNote",
                        "filter": "agTextColumnFilter",
                        "width": 200
                    },
                    {
                        "headerName":localConstant.techSpec.PayRate.HIDE_RATE,
                        "headerTooltip": localConstant.techSpec.PayRate.HIDE_RATE,
                        "field":"isActive",
                        "tooltip": (params) => {                               
                            if (params.data.isActive === true) {                        
                                return "No"; //Scenario Fix
                            } else {
                                return "Yes"; //Scenario Fix
                            }
                        },
                        "width": 150,
                        "filter": "agTextColumnFilter",
                        "valueGetter": (params) => {                               
                            if (params.data.isActive === true) {                        
                                return "No";
                            } else {
                                return "Yes";
                            }
                        },
                    }
                ],
                "enableFilter":true, 
                "enableSorting":true,
                "rowSelection": "multiple",
                "gridHeight":30,
                "pagination": true,
                "searchable":true,
                "clearFilter":true,
        },
    
        "MultiplePayRateHeader": {
            "columnDefs": [
                {
                    "checkboxSelection": true,
                    "headerCheckboxSelectionFilteredOnly": true,
                    "suppressFilter": true,
                    "width": 37
                },
                {
                    "headerName": localConstant.techSpec.PayRate.PAY_RATE,
                    "headerTooltip": localConstant.techSpec.PayRate.PAY_RATE,
                    "field": "name",
                    "tooltipField": "name",
                    "filter": "agTextColumnFilter",
                    "width": 205
                },
            ],
            "enableSelectAll":true,
            "enableFilter": false,
            "enableSorting": false,
            "pagination": false,
    
            "searchable": false,
            "gridActions": true,
            "gridTitlePanel": false,
            "gridHeight": 25,
            "rowSelection": "multiple",
            "clearFilter":false,
        },
    };

    function suppressArrowKeys(params) {     
        const KEY_LEFT = 37;
        const KEY_UP = 38;
        const KEY_RIGHT = 39;
        const KEY_DOWN = 40;
        const event = params.event;
        const key = event.which;
        let suppress = false;    
        if(key === KEY_RIGHT || key === KEY_DOWN || key === KEY_LEFT || key === KEY_UP) {
            suppress = true;
        }
        return suppress;
    }
};