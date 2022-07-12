import { getlocalizeData, thousandFormat, formatToDecimal } from '../../../../utils/commonUtils';
import moment from 'moment';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
const localConstant = getlocalizeData();

export const HeaderData = (functionRefs) => {
    return {
        "technicalSpecialist":{
            "columnDefs": [
                {
                    "headerCheckboxSelectionFilteredOnly": true,
                    "checkboxSelection": true,
                    "suppressFilter": true,
                    "width": 40,
                    "cellRenderer": (params) => {                    
                        if(params.node.rowIndex === 0) { 
                            params.node.setSelected(true);
                        }
                    }
                },
                {                    
                    "headerName": localConstant.gridHeader.RESOURCE,
                    "headerTooltip": localConstant.gridHeader.RESOURCE,
                    "field": "technicalSpecialistName",
                    "tooltip":(params) => (params.data.technicalSpecialistName + " (" +params.data.pin + ")"),   
                    "filter": "agTextColumnFilter",                
                    valueGetter: (params) => {
                        return params.data.technicalSpecialistName.includes(params.data.pin) ? params.data.technicalSpecialistName : params.data.technicalSpecialistName + " (" +params.data.pin + ")";
                    },
                    "width":503,
                },
                {
                    "headerName":  localConstant.visit.GROSSMARGIN,
                    "headerTooltip": localConstant.visit.GROSSMARGIN,
                    "field": "grossMargin",
                    "tooltip": (params) => {
                        return params.data.grossMargin ? params.data.grossMargin.toFixed(2) :'0.00';
                    },
                    "filter": "agTextColumnFilter",       
                    // valueGetter: (params) => {
                    //     return params.data.grossMargin ? params.data.grossMargin : '0.00';
                    // }, 
                    
                    cellRenderer: function(params) {
                        if (params.data.grossMargin >= 0) {
                            return params.data.grossMargin ? params.data.grossMargin.toFixed(2) : '0.00';
                        }
                        else {
                            return params.data.grossMargin ? "<span class='text-red' style='font-size: large;'><i class='zmdi zmdi-alert-triangle'></i></span> " + params.data.grossMargin.toFixed(2) : "<span class='text-red' style='font-size: large;'><i class='zmdi zmdi-alert-triangle'></i></span> 0.00";
                        }
                    }, // Added for D-927          
                    "width":450
                },
                {
                    "headerName": "Rate Schedules",
                    "headerTooltip": "Rate Schedules",
                    "field": "RateSchedules",
                    "tooltipField":"RateSchedules",   
                    "cellRenderer": "EditLink",
                    "buttonAction": "",
                    "cellRendererParams": {
                        "displayLink":"Rate Schedules",
                        "disableField": (e) => functionRefs.enableEditColumn(e)
                    },
                    "suppressFilter": true,
                    "suppressSorting": true,
                    "width": 150
                }  
                
            ],
            
            "rowSelection": 'single',   
            "pagination": true,
            "enableFilter": false,
            "enableSorting": false,
            "gridHeight": 60,
            "searchable": false,
            "gridActions": false,
            "gridTitlePanel": true,
            "suppressRowClickSelection": false     
        },
        "payRateModifyHeader":{
            "columnDefs": [
                {
                    "headerCheckboxSelectionFilteredOnly": true,
                    "checkboxSelection": true,
                    "suppressFilter": true,
                    "width": 40
                },
                {
                    "headerName": localConstant.visit.DESCRIPTION,
                    "headerTooltip": localConstant.visit.DESCRIPTION,
                    "field": "description",
                    "tooltipField":"description",
                    "filter": "agTextColumnFilter",                
                    "width":250
                },
                {
                    "headerName":  localConstant.visit.CURRENCY,
                    "headerTooltip": localConstant.visit.CURRENCY,
                    "field": "currency",
                    "tooltipField":"currency",
                    "filter": "agTextColumnFilter",                
                    "width":200
                },                       
                {
                    "headerName":  localConstant.visit.SCHEDULE_NAME,
                    "headerTooltip": localConstant.visit.SCHEDULE_NAME,
                    "field": "scheduleName",
                    "tooltipField":"scheduleName",
                    "filter": "agTextColumnFilter",                
                    "width":200
                },        
            ],
            
            "rowSelection": 'single',   
            "pagination": true,
            "enableFilter": true,
            "enableSorting": true,
            "gridHeight": 60,
            "searchable": true,
            "gridActions": true,
            "gridTitlePanel": true,
            "suppressRowClickSelection": false  ,
            "exportFileName": localConstant.visit.PAY_RATES 
        },
        "payRateHeader":{
            "columnDefs": [
                {
                    "headerCheckboxSelectionFilteredOnly": true,
                    "checkboxSelection": true,
                    "suppressFilter": true,
                    "width": 40
                },
                {
                    "headerName": localConstant.visit.DESCRIPTION,
                    "headerTooltip": localConstant.visit.DESCRIPTION,
                    "field": "description",
                    "tooltipField":"description",
                    "filter": "agTextColumnFilter",                
                    "width":200
                },
                {
                    "headerName":  localConstant.visit.CURRENCY,
                    "headerTooltip": localConstant.visit.CURRENCY,
                    "field": "currency",
                    "tooltipField":"currency",
                    "filter": "agTextColumnFilter",                
                    "width":150
                },
                {
                    "headerName":  localConstant.visit.RATE,
                    "headerTooltip": localConstant.visit.RATE,
                    "field": "chargeRate",
                    "tooltipField":"chargeRate",
                    "filter": "agTextColumnFilter",                
                    "width":150,
                    "valueGetter": (params) => {                         
                        if(params.data.chargeRate === undefined) {
                            return params.data.chargeRate;
                        } else {
                            return thousandFormat(params.data.chargeRate.toFixed(4)); //Changes for ITK D1565 
                        }
                    },
                },                         
                {
                    "headerName":  localConstant.visit.SCHEDULE_NAME,
                    "headerTooltip": localConstant.visit.SCHEDULE_NAME,
                    "field": "scheduleName",
                    "tooltipField":"scheduleName",
                    "filter": "agTextColumnFilter",                
                    "width":150
                },        
            ],
            
            "rowSelection": 'single',   
            "pagination": true,
            "enableFilter": true,
            "enableSorting": true,
            "gridHeight": 60,
            "searchable": true,
            "gridActions": true,
            "gridTitlePanel": true,
            "suppressRowClickSelection": false  ,
            "exportFileName": localConstant.visit.PAY_RATES 
        },
        "chargeRateHeader":{
            "columnDefs": [
                {
                    "headerCheckboxSelectionFilteredOnly": true,
                    "checkboxSelection": true,
                    "suppressFilter": true,
                    "width": 40
                },
                {
                    "headerName": localConstant.visit.DESCRIPTION,
                    "headerTooltip": localConstant.visit.DESCRIPTION,
                    "field": "description",
                    "tooltipField":"description",
                    "filter": "agTextColumnFilter",                
                    "width":200
                },
                {
                    "headerName":  localConstant.visit.CURRENCY,
                    "headerTooltip": localConstant.visit.CURRENCY,
                    "field": "currency",
                    "tooltipField":"currency",
                    "filter": "agTextColumnFilter",                
                    "width":150
                },
                {
                    "headerName":  localConstant.visit.RATE,
                    "headerTooltip": localConstant.visit.RATE,
                    "field": "chargeRate",
                    "tooltipField":"chargeRate",
                    "filter": "agTextColumnFilter",                
                    "width":150,
                    "valueGetter": (params) => {                            
                        if(params.data.chargeRate === undefined) {
                            return params.data.chargeRate;
                        } else {
                            return thousandFormat(params.data.chargeRate);
                        }
                    },
                },                         
                {
                    "headerName":  localConstant.visit.SCHEDULE_NAME,
                    "headerTooltip": localConstant.visit.SCHEDULE_NAME,
                    "field": "scheduleName",
                    "tooltipField":"scheduleName",
                    "filter": "agTextColumnFilter",                
                    "width":150
                },        
            ],
            
            "rowSelection": 'single',   
            "pagination": true,
            "enableFilter": true,
            "enableSorting": true,
            "gridHeight": 60,
            "searchable": true,
            "gridActions": true,
            "gridTitlePanel": true,
            "suppressRowClickSelection": false  ,
            "exportFileName": localConstant.visit.RATES
        },
        "chargeScheduleRates":{
            "columnDefs": [
                {
                    "headerName": `${ localConstant.gridHeader.SCHEDULE_NAME }: `,
                    "headerTooltip": `${ localConstant.gridHeader.SCHEDULE_NAME }: `,
                    "field": "contractScheduleName",
                    "tooltipField":"contractScheduleName",   
                    "cellRenderer": 'agGroupCellRenderer',
                    "width": 200
                },
                {
                    "headerName": localConstant.gridHeader.TYPE,
                    "headerTooltip": localConstant.gridHeader.TYPE,
                    "field": "chargeType", 
                    "tooltipField":"chargeType",   
                    "filter": "agTextColumnFilter",
                    "width":100
                },
                {
                    "headerName":  localConstant.assignments.RATE,
                    "headerTooltip": localConstant.assignments.RATE,
                    "field": "chargeRate",
                    //"tooltipField":"chargeRate", 
                    "tooltip": (params) => {
                        if(params.data.chargeRate === undefined) {
                            return params.data.chargeRate;
                        } else {
                            return thousandFormat(params.data.chargeRate);
                        }
                    },  
                    "filter": "agTextColumnFilter",
                    "width":100,
                    "valueGetter": (params) => {
                        if(params.data.chargeRate === undefined) {
                            return params.data.chargeRate;
                        } else {
                            return thousandFormat(params.data.chargeRate);
                        }
                    },
                },
                {
                    "headerName":  localConstant.gridHeader.CURRENCY,
                    "headerTooltip": localConstant.gridHeader.CURRENCY,
                    "field": "currency",

                    "tooltipField":"currency",   
                    "filter": "agTextColumnFilter",
                    "width":100
                },
                {
                    "headerName":  localConstant.gridHeader.DESCRIPTION,
                    "headerTooltip": localConstant.gridHeader.DESCRIPTION,
                    "field": "description",
                    "tooltipField":"description",   
                    "filter": "agTextColumnFilter",
                    "width":135
                },
                {
                    "headerName":  localConstant.gridHeader.EFFECTIVE_FROM,
                    "headerTooltip": localConstant.gridHeader.EFFECTIVE_FROM,
                    "field": "effectiveFrom",
                    "tooltip": (params) => {
                        return moment(params.data.effectiveFrom).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                },    
                    "filter": "agTextColumnFilter",
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "effectiveFrom"
                    },
                    "width":150
                },
                {
                    "headerName":  localConstant.gridHeader.EFFECTIVE_TO,
                    "headerTooltip": localConstant.gridHeader.EFFECTIVE_TO,
                    "field": "effectiveTo",
                    "tooltip": (params) => {
                        return moment(params.data.effectiveTo).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                    "filter": "agTextColumnFilter",
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "effectiveTo"
                    },
                    "width":150
                }
            ],
            
            "rowSelection": 'single',   
            "pagination": false,
            "enableFilter": false,
            "enableSorting": true,
            "gridHeight": 60,
            "searchable": false,
            "gridActions": true,
            "gridTitlePanel": false,
            "Grouping":true,
            "GroupingDataName":"chargeScheduleRates"       
        },
        "payScheduleRate":{
            "columnDefs": [
                {
                    "headerName": `${ localConstant.gridHeader.SCHEDULE_NAME }: `,
                    "headerTooltip": `${ localConstant.gridHeader.SCHEDULE_NAME }: `,
                    "field": "technicalSpecialistPayScheduleName",
                    "tooltipField":"technicalSpecialistPayScheduleName",   
                    "cellRenderer": 'agGroupCellRenderer',
                    "width": 200
                },           
                {
                    "headerName": localConstant.gridHeader.TYPE,
                    "headerTooltip": localConstant.gridHeader.TYPE,
                    "field": "expenseType",
                    "tooltipField":"expenseType",   
                    "filter": "agTextColumnFilter",
                    "width":100
                },
                {
                    "headerName":  localConstant.assignments.RATE,
                    "headerTooltip": localConstant.assignments.RATE,
                    "field": "updatedPayRate",
                    //"tooltipField":"payRate",
                    "tooltip": (params) => {                            
                        if(params.data.updatedPayRate === undefined || params.data.updatedPayRate === null) {
                            return params.data.updatedPayRate;
                        } else {
                            return thousandFormat(params.data.updatedPayRate.toFixed(4));
                        }
                    }, 
                    "filter": "agTextColumnFilter",
                    "width":100,
                    "valueGetter": (params) => {                            
                        if(params.data.updatedPayRate === undefined || params.data.updatedPayRate === null) {
                            return params.data.updatedPayRate;
                        } else {
                            return thousandFormat(params.data.updatedPayRate.toFixed(4));
                        }
                    },
                },
                {
                    "headerName":  localConstant.gridHeader.CURRENCY,
                    "headerTooltip": localConstant.gridHeader.CURRENCY,
                    "field": "currency",
                    "tooltipField":"currency",   
                    "filter": "agTextColumnFilter",
                    "width":100
                },
                {
                    "headerName":  localConstant.gridHeader.DESCRIPTION,
                    "headerTooltip": localConstant.gridHeader.DESCRIPTION,
                    "field": "description",
                    "tooltipField":"description",   
                    "filter": "agTextColumnFilter",
                    "width":135
                },
                {
                    "headerName":  localConstant.gridHeader.EFFECTIVE_FROM,
                    "headerTooltip": localConstant.gridHeader.EFFECTIVE_FROM,
                    "field": "effectiveFrom",
                    "tooltip": (params) => {
                        return moment(params.data.effectiveFrom).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                    }, 
                    "filter": "agTextColumnFilter",
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "effectiveFrom"
                    },
                    "width":150
                },
                {
                    "headerName":  localConstant.gridHeader.EFFECTIVE_TO,
                    "headerTooltip": localConstant.gridHeader.EFFECTIVE_TO,
                    "field": "effectiveTo",
                    "tooltip": (params) => {
                        return moment(params.data.effectiveTo).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                    },   
                    "filter": "agTextColumnFilter",
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "effectiveTo"
                    },
                    "width":150
                }     
                
            ],
            
            "rowSelection": 'single',   
        "pagination": false,
        "enableFilter": false,
        "enableSorting": true,
        //"gridHeight": 60,
        "searchable": false,
        "gridActions": true,
        "gridTitlePanel": false,
        "Grouping":true,
        "GroupingDataName":"payScheduleRates"     
        },
        "timeHeaderDetails":{
            "columnDefs":[
                {
                    "headerCheckboxSelectionFilteredOnly": true,
                    "checkboxSelection": (params) => {
                        return ((params.data.invoicingStatus && (params.data.invoicingStatus === 'C' || params.data.invoicingStatus === 'D')) 
                                || (params.data.costofSalesStatus && params.data.costofSalesStatus === 'X') ? false : true);
                    },
                    "suppressFilter": true,
                    "width": 26,
                    "pinned": 'left'
                },            
                {
                    "headerName": "",
                    "rowClass" :"Parent",
                    "children": [
                        { 
                            "headerName": "Date", 
                            "headerTooltip": localConstant.gridHeader.DATE,                             
                            "tooltip": (params) => {
                                let d='';
                                if(params.data.expenseDate){
                                 d = moment(params.data.expenseDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                                }
                                return d;
                            },
                            "rowClass" :"child", 
                            "field": 'expenseDate',  
                            "width": 105,
                            "headerClass": 'my-css-class',
                            "filter": "agDateColumnFilter",                            
                            "cellRenderer": "SelectType", 
                            "enableRowGroup": true,
                            "moduleName":'Visit',                            
                            "name": "expenseDate",
                            "type": "date",
                            "moduleType": "Time",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.expenseDateRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus, isLineItemEditableExpense: params.data.isLineItemEditableExpense,
                                    recordStatus: params.data.recordStatus, hasNoChargeRate: params.data.hasNoChargeRate };
                            },
                            "chargePayType": "",
                            "suppressKeyboardEvent": suppressArrowKeys,
                            "pinned": 'left'
                        },
                        { 
                            "headerName": "Type", 
                            "headerTooltip": localConstant.gridHeader.TYPE, 
                            "tooltipField":"chargeExpenseType", 
                            "field": "chargeExpenseType", 
                            "cellRenderer": "SelectType",
                            "width":135,
                            "enableRowGroup": true,
                            "moduleName":'Visit',
                            "selectListName": "TimeChargeType",
                            "name": "chargeExpenseType",
                            "optionName": "name",
                            "optionValue": "name",
                            "type": "select",
                            "moduleType": "Time",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.chargeExpenseTypeRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus, isLineItemEditableExpense: params.data.isLineItemEditableExpense,
                                    recordStatus: params.data.recordStatus, hasNoChargeRate: params.data.hasNoChargeRate };
                            },
                            "chargePayType": "",
                            "suppressKeyboardEvent": suppressArrowKeys,
                            "pinned": 'left'
                        },
                        { 
                            "headerName": "Full Description ", 
                            "headerTooltip": "Full Description", 
                            "tooltipField":"expenseDescription", 
                            "field": "expenseDescription",
                            "enableRowGroup": true, 
                            "width":120, 
                            "moduleName":'Visit',
                            "cellRenderer": "SelectType",
                            "name": "expenseDescription",
                            "maxLength": fieldLengthConstants.visit.resourceAccounts.time.EXPENSE_DESCRIPTION_MAXLENGTH,
                            "type": "textarea",
                            "moduleType": "Time",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, lineItemPermision: params.data.lineItemPermision, 
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus, isLineItemEditableExpense: params.data.isLineItemEditableExpense,
                                    recordStatus: params.data.recordStatus };
                            },
                            "chargePayType": "",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        {
                            "headerName":  localConstant.gridHeader.DESCRIPTION_PRINT_ON_INVOICE_ICON,
                            "headerTooltip": localConstant.gridHeader.DESCRIPTION_PRINT_ON_INVOICE,
                            "field": "isInvoicePrintExpenseDescription",
                            "tooltip": (params) => {
                                if (params.data.isInvoicePrintExpenseDescription === true) {
                                    return "Yes";
                                } else {
                                    return "No";
                                }
                            },
                            "filter": "agTextColumnFilter",
                            // "valueGetter": (params) => {
                            //     if (params.data.isInvoicePrintExpenseDescription === true) {
                            //         return "Yes";
                            //     } else {
                            //         return "No";
                            //     }
                            // },
                            "width":41,
                            "cellRenderer": "SelectType",
                            "name": "isInvoicePrintExpenseDescription",
                            "type": "switch",
                            "moduleName":'Visit',
                            "moduleType": "Time",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, lineItemPermision: params.data.lineItemPermision, 
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus, isLineItemEditableExpense: params.data.isLineItemEditableExpense,
                                    recordStatus: params.data.recordStatus };
                            },
                            "chargePayType": "",
                            "suppressKeyboardEvent": suppressArrowKeys             
                        }                                       
                    ]
                },
                {
                    "headerName": "Charge",
                    "rowClass" :"Parent",
                    "children": [                        
                        { 
                            "headerName": " ", 
                            "headerTooltip": "",                             
                            "field": "invoicingStatus",
                            "width":30, 
                            "moduleName":'Visit',
                            "cellRenderer": "SelectType",
                            "name": "InvoiceStatus",
                            "type": "InvoiceStatus",
                            "moduleType": "Time",
                            "cellRendererParams": (params) => {                                
                                return { unPaidStatus: params.data.unPaidStatus, unPaidStatusReason: params.data.unPaidStatusReason };
                            },
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "Work ", 
                            "headerTooltip": "Work", 
                            "tooltipField":"chargeWorkUnit", 
                            "field": "chargeWorkUnit",
                            "enableRowGroup": true, 
                            "width":65, 
                            "cellRenderer": "SelectType", 
                            "name": "chargeWorkUnit", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Time",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, lineItemPermision: params.data.lineItemPermision, 
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "decimalType": "two",
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys                   
                        },
                        { 
                            "headerName": "Travel ", 
                            "headerTooltip": "Travel ", // Added on 18th Jan (As per mail recieved from Francina)
                            "tooltipField":"chargeTravelUnit", 
                            "field": "chargeTravelUnit",
                            "enableRowGroup": true, 
                            "width":65, 
                            "cellRenderer": "SelectType", 
                            "name": "chargeTravelUnit", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Time",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, lineItemPermision: params.data.lineItemPermision, 
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "decimalType": "two",
                            "chargePayType": "charge",      
                            "suppressKeyboardEvent": suppressArrowKeys                      
                        },
                        { 
                            "headerName": "Wait ", 
                            "headerTooltip": "Wait", 
                            "tooltipField":"chargeWaitUnit", 
                            "field": "chargeWaitUnit",
                            "enableRowGroup": true, 
                            "width":65,
                            "cellRenderer": "SelectType", 
                            "name": "chargeWaitUnit", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Time",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, lineItemPermision: params.data.lineItemPermision, 
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "decimalType": "two", 
                            "chargePayType": "charge",   
                            "suppressKeyboardEvent": suppressArrowKeys                        
                        },
                        { 
                            "headerName": "Report ", 
                            "headerTooltip": "Report", 
                            "tooltipField":"chargeReportUnit", 
                            "field": "chargeReportUnit",
                            "enableRowGroup": true, 
                            "width":65, 
                            "cellRenderer": "SelectType", 
                            "name": "chargeReportUnit", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Time",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, lineItemPermision: params.data.lineItemPermision, 
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "decimalType": "two",    
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys                        
                        },
                        { 
                            "headerName": "Units ", 
                            "headerTooltip": "Units", 
                            "tooltipField":"chargeTotalUnit", 
                            "field": "chargeTotalUnit",
                            "enableRowGroup": true, 
                            "width":78, 
                            "cellRenderer": "SelectType", 
                            "name": "chargeTotalUnit", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Time",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.chargeTotalUnitRequired, 
                                    lineItemPermision: params.data.lineItemPermision, invoicingStatus: params.data.invoicingStatus, 
                                    costofSalesStatus: params.data.costofSalesStatus, recordStatus: params.data.recordStatus };
                            },
                            "decimalType": "two", 
                            "chargePayType": "charge",  
                            "suppressKeyboardEvent": suppressArrowKeys                         
                        },
                        { 
                            "headerName": "Rate", 
                            "headerTooltip": "Rate", 
                            "tooltipField":"chargeRate", 
                            "field": "chargeRate", 
                            "enableRowGroup": true, 
                            "width": 100,
                            "cellRenderer": "SelectType", 
                            "name": "chargeRate", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Time",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.chargeRateRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus, ratesReadonly: params.data.chargeRateReadonly };
                            },
                            "decimalType": "four",
                            "EditChargeRateRowHandler": (updatedData) => functionRefs.EditChargeRateRowHandler(updatedData),     
                            "chargePayType": "charge",       
                            "suppressKeyboardEvent": suppressArrowKeys                            
                        },                                  
                        { 
                            "headerName": "Currency ", 
                            "headerTooltip": "Currency", 
                            "tooltipField":"chargeRateCurrency", 
                            "field": "chargeRateCurrency", 
                            "enableRowGroup": true,  
                            "width":90, 
                            "cellRenderer": "SelectType",
                            "moduleName":'Visit', 
                            "selectListName": "CurrencyMasterData", 
                            "name": "chargeRateCurrency",
                            "optionName": "code", 
                            "optionValue": "code", 
                            "type": "select", 
                            "moduleType": "Time",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.chargeRateCurrencyRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },       
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys                     
                         },
                        { 
                            "headerName": "Description ", 
                            "headerTooltip": "Description", 
                            "tooltipField":"chargeRateDescription", 
                            "field": "chargeRateDescription",
                            "enableRowGroup": true, 
                            "width":96, 
                            "cellRenderer": "SelectType", 
                            "name": "chargeRateDescription", 
                            "maxLength": fieldLengthConstants.visit.resourceAccounts.time.CHARGE_RATE_DESCRIPTION_MAXLENGTH,
                            "type": "textarea", 
                            "moduleName":'Visit', 
                            "moduleType": "Time",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, lineItemPermision: params.data.lineItemPermision, 
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },    
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys                        
                        },
                        {
                            "headerName":  localConstant.gridHeader.DESCRIPTION_PRINT_ON_INVOICE_ICON,
                            "headerTooltip": localConstant.gridHeader.DESCRIPTION_PRINT_ON_INVOICE,
                            "field": "isInvoicePrintPayRateDescrition",
                            "tooltip": (params) => {
                                if (params.data.isInvoicePrintPayRateDescrition === true) {
                                    return "Yes";
                                } else {
                                    return "No";
                                }
                            },
                            "filter": "agTextColumnFilter",
                            "width":41,
                            "cellRenderer": "SelectType",
                            "name": "isInvoicePrintPayRateDescrition",
                            "type": "switch",
                            "moduleName":'Visit',
                            "moduleType": "Time",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, lineItemPermision: params.data.lineItemPermision, 
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys             
                        }
                    ]
                },
                {
                    "headerName": "Pay",
                    "children": [                        
                        { 
                            "headerName": " ", 
                            "headerTooltip": "",                             
                            "field": "costofSalesStatus",                            
                            "width":30, 
                            "moduleName":'Visit',
                            "cellRenderer": "SelectType",
                            "name": "CostofSalesStatus",
                            "type": "CostofSalesStatus",
                            "moduleType": "Time",
                            "cellRendererParams": (params) => {                                
                                return { unPaidStatus: params.data.unPaidStatus, unPaidStatusReason: params.data.unPaidStatusReason };
                            },
                            "chargePayType": "pay",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "Units ", 
                            "headerTooltip": "Units", 
                            "tooltipField":"payUnit", 
                            "field": "payUnit",
                            "width":78, 
                            "cellRenderer": "SelectType", 
                            "name": "payUnit", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Time",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.payUnitRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "decimalType": "two",     
                            "chargePayType": "pay",    
                            "suppressKeyboardEvent": suppressArrowKeys                   
                        },
                        { 
                            "headerName": "Rate", 
                            "headerTooltip": "Rate",
                            "tooltip": (params) => {
                                if(params.data.lineItemPermision.isCHUser) {
                                    return " ";
                                } else if(params.data.modifyPayUnitPayRate) {
                                    return "0.0000";
                                } else {
                                    return params.data.payRate;
                                }
                            },
                            "field": "payRate", 
                            "width": 100,
                            "cellRenderer": "SelectType", 
                            "name": "payRate", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Time",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.payRateRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus, ratesReadonly: params.data.payRateReadonly, modifyPayUnitPayRate: params.data.modifyPayUnitPayRate };
                            },
                            "decimalType": "four",
                            "EditPayRateRowHandler": (updatedData) => functionRefs.EditPayRateRowHandler(updatedData),  
                            "chargePayType": "pay",       
                            "suppressKeyboardEvent": suppressArrowKeys                       
                        },
                        { 
                            "headerName": "Currency ", 
                            "headerTooltip": "Currency", 
                            "tooltipField":"payRateCurrency", 
                            "field": "payRateCurrency", 
                            "width":90, 
                            "cellRenderer": "SelectType", 
                            "moduleName":'Visit',
                            "selectListName": "CurrencyMasterData", 
                            "name": "payRateCurrency", 
                            "optionName": "code",
                            "optionValue": "code", 
                            "type": "select", 
                            "moduleType": "Time",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.payRateCurrencyRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },             
                            "chargePayType": "pay",    
                            "suppressKeyboardEvent": suppressArrowKeys               
                         },
                        { 
                            "headerName": "Description ", 
                            "headerTooltip": "Description", 
                            "tooltipField":"payRateDescription", 
                            "field": "payRateDescription", 
                            "width":115, 
                            "cellRenderer": "SelectType", 
                            "name": "payRateDescription", 
                            "maxLength": fieldLengthConstants.visit.resourceAccounts.time.PAY_RATE_DESCRIPTION_MAXLENGTH,
                            "type": "textarea", 
                            "moduleName":'Visit', 
                            "moduleType": "Time",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, lineItemPermision: params.data.lineItemPermision, invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },   
                            "chargePayType": "pay",
                            "suppressKeyboardEvent": suppressArrowKeys                         
                        }
                    ]
                },
            ],
            "rowSelection": 'multiple',         
            "pagination": false,
            "enableFilter": false,
            "enableSorting": true,          
            "searchable": false,
            "gridActions": true,
            "gridTitlePanel": false,
            "headerHeight":40,
        },
        "expensesDetails":{
            "columnDefs":[
                {
                    "headerCheckboxSelectionFilteredOnly": true,
                    "checkboxSelection": (params) => {
                        return ((params.data.invoicingStatus && (params.data.invoicingStatus === 'C' || params.data.invoicingStatus === 'D')) 
                                || (params.data.costofSalesStatus && params.data.costofSalesStatus === 'X') ? false : true);
                    },
                    "suppressFilter": true,
                    "width":26,
                    "pinned": 'left'
                },
                {
                    "headerName": "",
                    "rowClass" :"Parent",
                    "children": [
                        { 
                            "headerName": "Date",
                            "headerTooltip": "Date", 
                            "rowClass" :"child", 
                            "tooltip": (params) => {
                                let d='';
                                if(params.data.expenseDate){
                                 d = moment(params.data.expenseDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                                }
                                return d;
                            },
                            "field": 'expenseDate',  
                            "width": 105,
                            "filter": "agDateColumnFilter",
                            "cellRenderer": "SelectType",                                                       
                            "moduleName":'Visit',                            
                            "name": "expenseDate",
                            "type": "date",
                            "moduleType": "Expense",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.expenseDateRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus, isLineItemEditableExpense: params.data.isLineItemEditableExpense,
                                    recordStatus: params.data.recordStatus, hasNoChargeRate: params.data.hasNoChargeRate };
                            },
                            "chargePayType": "",
                            "suppressKeyboardEvent": suppressArrowKeys,
                            "pinned": 'left'                         
                        },
                        { 
                            "headerName": "Type", 
                            "headerTooltip": "Type", 
                            "tooltipField":"chargeExpenseType", 
                            "field": "chargeExpenseType", 
                            "cellRenderer": "SelectType",
                            "width":130,
                            "moduleName":'Visit',
                            "selectListName": "ExpenseChargeType",
                            "name": "chargeExpenseType",
                            "optionName": "name",
                            "optionValue": "name",
                            "type": "select",
                            "moduleType": "Expense",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.chargeExpenseTypeRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus, isLineItemEditableExpense: params.data.isLineItemEditableExpense,
                                    recordStatus: params.data.recordStatus, hasNoChargeRate: params.data.hasNoChargeRate  };
                            },
                            "chargePayType": "",
                            "suppressKeyboardEvent": suppressArrowKeys,
                            "pinned": 'left'
                        },
                        { 
                            "headerName": "Description ", 
                            "headerTooltip": "Description", 
                            "tooltipField":"expenseDescription", 
                            "field": "expenseDescription", 
                            "width":110, 
                            "moduleName":'Visit',
                            "cellRenderer": "SelectType",
                            "name": "expenseDescription",
                            "maxLength": fieldLengthConstants.visit.resourceAccounts.expense.EXPENSE_DESCRIPTION_MAXLENGTH,
                            "type": "textarea",
                            "moduleType": "Expense",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, lineItemPermision: params.data.lineItemPermision, 
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus, isLineItemEditableExpense: params.data.isLineItemEditableExpense,
                                    recordStatus: params.data.recordStatus, hasNoChargeRate: params.data.hasNoChargeRate };
                            },
                            "chargePayType": "",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "Currency ", 
                            "headerTooltip": "Currency", 
                            "tooltipField":"currency", 
                            "field": "currency", 
                            "width":90, 
                            "cellRenderer": "SelectType",
                            "moduleName":'Visit', 
                            "selectListName": "CurrencyMasterData", 
                            "name": "currency",
                            "optionName": "code", "optionValue": "code", "type": "select", "moduleType": "Expense",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.currencyRequired, lineItemPermision: params.data.lineItemPermision,
                                     invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus, isLineItemEditableExpense: params.data.isLineItemEditableExpense,
                                     recordStatus: params.data.recordStatus, hasNoChargeRate: params.data.hasNoChargeRate  };
                            },
                            "chargePayType": "",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "CH Exp ", 
                            "headerTooltip": "ContractHolderExpense.When Checked,this Expense item will be paid by the Contract Holder.It will not appear on the Inter Company Invoice raised by the Operating Company.",
                            "field": "isContractHolderExpense",   
                            "tooltip": (params) => {
                                return params.data.isContractHolderExpense ? "Yes" : "No";
                            },
                            "width":66,
                            "cellRenderer": "SelectType",
                            "name": "isContractHolderExpense",
                            "type": "switch",
                            "moduleName":'Visit',
                            "moduleType": "Expense",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, lineItemPermision: params.data.lineItemPermision, invoicingStatus: params.data.invoicingStatus,
                                     costofSalesStatus: params.data.costofSalesStatus, recordStatus: params.data.recordStatus  };
                            },
                            "chargePayType": "",
                            "suppressKeyboardEvent": suppressArrowKeys
                        }                     
                    ]
                },
                {
                    "headerName": "Charge",
                    "children": [
                        
                        { 
                            "headerName": " ", 
                            "headerTooltip": "",                             
                            "field": "invoicingStatus",
                            "width":30, 
                            "moduleName":'Visit',
                            "cellRenderer": "SelectType",
                            "name": "InvoiceStatus",
                            "type": "InvoiceStatus",
                            "moduleType": "Expense",
                            "cellRendererParams": (params) => {                                
                                return { unPaidStatus: params.data.unPaidStatus, unPaidStatusReason: params.data.unPaidStatusReason };
                            },
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "Units ", 
                            "headerTooltip": "Units", 
                            "tooltipField":"chargeUnit", 
                            "field": "chargeUnit",                             
                            "width":65, 
                            "cellRenderer": "SelectType", 
                            "name": "chargeUnit", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Expense",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.chargeUnitRequired, 
                                    lineItemPermision: params.data.lineItemPermision, invoicingStatus: params.data.invoicingStatus, 
                                    costofSalesStatus: params.data.costofSalesStatus, recordStatus: params.data.recordStatus };
                            },
                            "decimalType": "two",
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },                        
                        { 
                            "headerName": "Rate", 
                            "headerTooltip": "Rate", 
                            "tooltipField":"chargeRate", 
                            "field": "chargeRate",
                            "width": 100,
                            "cellRenderer": "SelectType", 
                            "name": "chargeRate", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Expense",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.chargeRateRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus }; // ITK 1582 removed chargeRateReadonly in expense
                            },
                            "decimalType": "four",
                            "EditChargeRateRowHandler": (updatedData) => functionRefs.EditChargeRateRowHandler(updatedData),
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "Exchange ", 
                            "headerTooltip": "Exchange", 
                            "tooltipField":"chargeRateExchange", 
                            "field": "chargeRateExchange", 
                            "width":82, 
                            "cellRenderer": "SelectType", 
                            "name": "chargeRateExchange", 
                            "type": "text", "moduleName":'Visit', "moduleType": "Expense",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, lineItemPermision: params.data.lineItemPermision, invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "decimalType": "six",
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },                 
                        { 
                            "headerName": "Currency ", 
                            "headerTooltip": "Currency", 
                            "tooltipField":"chargeRateCurrency", 
                            "field": "chargeRateCurrency", 
                            "width":90, 
                            "cellRenderer": "SelectType",
                            "moduleName":'Visit', 
                            "selectListName": "CurrencyMasterData", 
                            "name": "chargeRateCurrency",
                            "optionName": "code", 
                            "optionValue": "code", 
                            "type": "select", 
                            "moduleType": "Expense",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.chargeRateCurrencyRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus, hasNoChargeRate: params.data.hasNoChargeRate };
                            },
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },                        
                    ]
                },
                {
                    "headerName": "Pay",
                    "children": [
                        { 
                            "headerName": " ", 
                            "headerTooltip": "",                             
                            "field": "costofSalesStatus",                            
                            "width":30, 
                            "moduleName":'Visit',
                            "cellRenderer": "SelectType",
                            "name": "CostofSalesStatus",
                            "type": "CostofSalesStatus",
                            "moduleType": "Expense",
                            "cellRendererParams": (params) => {                                
                                return { unPaidStatus: params.data.unPaidStatus, unPaidStatusReason: params.data.unPaidStatusReason };
                            },
                            "chargePayType": "pay",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "Units ", 
                            "headerTooltip": "Units", 
                            "tooltipField":"payUnit", 
                            "field": "payUnit", 
                            "width":68, 
                            "cellRenderer": "SelectType", 
                            "name": "payUnit", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Expense",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.payUnitRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "decimalType": "two",
                            "chargePayType": "pay",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },                        
                        { 
                            "headerName": "Nett/Unit", 
                            "headerTooltip": "Nett/Unit",
                            "tooltip": (params) => {
                                if(params.data.lineItemPermision.isCHUser) {
                                    return " ";
                                } else {
                                    return params.data.payRate;
                                }
                            }, 
                            "field": "payRate",                            
                            "width": 100,
                            "cellRenderer": "SelectType",
                            "name": "payRate", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Expense",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.payRateRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus }; // ITK 1582 removed payRateReadonly in expense
                            },
                            "decimalType": "four",
                            "EditPayRateRowHandler": (updatedData) => functionRefs.EditPayRateRowHandler(updatedData),
                            "chargePayType": "pay",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "Tax ", 
                            "headerTooltip": "Tax", 
                            "tooltip": (params) => {
                                if(params.data.lineItemPermision.isCHUser) {
                                    return " ";
                                } else {
                                    return params.data.payRateTax;
                                }
                            },
                            "field": "payRateTax",
                            "width":70, 
                            "cellRenderer": "SelectType", 
                            "name": "payRateTax", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Expense",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.payRateTaxRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "decimalType": "two",
                            "chargePayType": "pay",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "Gross ", 
                            "headerTooltip": "Gross", 
                            "tooltip": (params) => {
                                if(params.data.lineItemPermision.isCHUser) {
                                    return " ";
                                } else {
                                    const payUnit = isNaN(parseFloat(params.data.payUnit))?0:parseFloat(params.data.payUnit),
                                    payRate = isNaN(parseFloat(params.data.payRate))?0:parseFloat(params.data.payRate),
                                    payRateTax = isNaN(parseFloat(params.data.payRateTax))?0:parseFloat(params.data.payRateTax);                                                                
                                    return ((payUnit * payRate) + payRateTax).toFixed(2);
                                }
                            },
                            "field": "payRateGross",
                            "width":80,
                            "valueGetter": (params) => {
                                if(params.data.lineItemPermision.isCHUser) {
                                    return "";
                                } else {
                                    const payUnit = isNaN(parseFloat(params.data.payUnit))?0:parseFloat(params.data.payUnit),
                                    payRate = isNaN(parseFloat(params.data.payRate))?0:parseFloat(params.data.payRate),
                                    payRateTax = isNaN(parseFloat(params.data.payRateTax))?0:parseFloat(params.data.payRateTax);                                                                
                                    return ((payUnit * payRate) + payRateTax).toFixed(2);
                                }
                            },
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "Exchange ", 
                            "headerTooltip": "Exchange", 
                            "tooltipField":"payRateExchange", 
                            "field": "payRateExchange",
                            "width":83, 
                            "cellRenderer": "SelectType", 
                            "name": "payRateExchange", 
                            "type": "text", "moduleName":'Visit', "moduleType": "Expense",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, lineItemPermision: params.data.lineItemPermision, invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "decimalType": "six",
                            "chargePayType": "pay",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },                    
                        { 
                            "headerName": "Currency ", 
                            "headerTooltip": "Currency", 
                            "tooltipField":"payRateCurrency", 
                            "field": "payRateCurrency",
                            "width":90, 
                            "cellRenderer": "SelectType", 
                            "moduleName":'Visit',
                            "selectListName": "CurrencyMasterData", 
                            "name": "payRateCurrency", 
                            "optionName": "code",
                            "optionValue": "code", 
                            "type": "select", 
                            "moduleType": "Expense",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.payRateCurrencyRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "chargePayType": "pay",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                    ]
                },                
            ],
            "rowSelection": 'multiple',         
            "pagination": false,
            "enableFilter": false,
            "enableSorting": true,            
            "searchable": false,
            "gridActions": true,
            "gridTitlePanel": false,
            "headerHeight":40,
        },
        "travelDetails":{
            "columnDefs":[
                {
                    "headerCheckboxSelectionFilteredOnly": true,
                    "checkboxSelection": (params) => {
                        return ((params.data.invoicingStatus && (params.data.invoicingStatus === 'C' || params.data.invoicingStatus === 'D')) 
                                || (params.data.costofSalesStatus && params.data.costofSalesStatus === 'X') ? false : true);
                    },
                    "suppressFilter": true,
                    "width": 30,
                    "pinned": 'left'
                }, 
                {
                    "headerName": "",
                    "rowClass" :"Parent",
                    "children": [
                        { 
                            "headerName": "Date",
                            "headerTooltip": "Date", 
                            "rowClass" :"child", 
                            "tooltip": (params) => {
                                let d='';
                                if(params.data.expenseDate){
                                 d = moment(params.data.expenseDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                                }
                                return d;
                            }, 
                            "field": 'expenseDate',  
                            "width": 105,                            
                            "filter": "agDateColumnFilter",                            
                            "cellRenderer": "SelectType",
                            "enableRowGroup": true,
                            "moduleName":'Visit',                            
                            "name": "expenseDate",
                            "type": "date",
                            "moduleType": "Travel",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.expenseDateRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus, isLineItemEditableExpense: params.data.isLineItemEditableExpense,
                                    recordStatus: params.data.recordStatus, hasNoChargeRate: params.data.hasNoChargeRate };
                            },
                            "chargePayType": "",
                            "suppressKeyboardEvent": suppressArrowKeys,
                            "pinned": 'left'
                        }                        
                    ]
                },
                {
                    "headerName": "Charge",
                    "children": [
                        { 
                            "headerName": "Description ", 
                            "headerTooltip": "Description", 
                            "tooltipField":"expenseDescription", 
                            "field": "expenseDescription", 
                            "width":150, 
                            "moduleName":'Visit',
                            "cellRenderer": "SelectType",
                            "name": "expenseDescription",
                            "maxLength": fieldLengthConstants.visit.resourceAccounts.travel.EXPENSE_DESCRIPTION_MAXLENGTH,
                            "type": "textarea",
                            "moduleType": "Travel",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, lineItemPermision: params.data.lineItemPermision, invoicingStatus: params.data.invoicingStatus,
                                     costofSalesStatus: params.data.costofSalesStatus, isLineItemEditableExpense: params.data.isLineItemEditableExpense, recordStatus: params.data.recordStatus,hasNoChargeRate: params.data.hasNoChargeRate };
                            },
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        {
                            "headerName":  localConstant.gridHeader.DESCRIPTION_PRINT_ON_INVOICE_ICON,
                            "headerTooltip": localConstant.gridHeader.DESCRIPTION_PRINT_ON_INVOICE,
                            "field": "isInvoicePrintExpenseDescription",
                            "tooltip": (params) => {
                                if (params.data.isInvoicePrintExpenseDescription === true) {
                                    return "Yes";
                                } else {
                                    return "No";
                                }
                            },
                            "filter": "agTextColumnFilter",
                            // "valueGetter": (params) => {
                            //     if (params.data.isInvoicePrintExpenseDescription === true) {
                            //         return "Yes";
                            //     } else {
                            //         return "No";
                            //     }
                            // },
                            "width":39,
                            "cellRenderer": "SelectType",
                            "name": "isInvoicePrintExpenseDescription",
                            "type": "switch",
                            "moduleName":'Visit',
                            "moduleType": "Travel",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, lineItemPermision: params.data.lineItemPermision, invoicingStatus: params.data.invoicingStatus,
                                     costofSalesStatus: params.data.costofSalesStatus, isLineItemEditableExpense: params.data.isLineItemEditableExpense, recordStatus: params.data.recordStatus, hasNoChargeRate: params.data.hasNoChargeRate };
                            },
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": " ", 
                            "headerTooltip": "",                             
                            "field": "invoicingStatus",
                            "width":30, 
                            "moduleName":'Visit',
                            "cellRenderer": "SelectType",
                            "name": "InvoiceStatus",
                            "type": "InvoiceStatus",
                            "moduleType": "Travel",
                            "cellRendererParams": (params) => {                                
                                return { unPaidStatus: params.data.unPaidStatus, unPaidStatusReason: params.data.unPaidStatusReason };
                            },
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "Charge Type", 
                            "headerTooltip": "Charge Type", 
                            "tooltipField":"chargeExpenseType", 
                            "field": "chargeExpenseType", 
                            "cellRenderer": "SelectType",
                            "width":120,                            
                            "moduleName":'Visit',
                            "selectListName": "TravelChargeType",
                            "name": "chargeExpenseType",
                            "optionName": "name",
                            "optionValue": "name",
                            "type": "select",
                            "moduleType": "Travel",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.chargeExpenseTypeRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "Units ", 
                            "headerTooltip": "Units", 
                            "tooltipField":"chargeTotalUnit", 
                            "field": "chargeTotalUnit",
                            "width":100, 
                            "cellRenderer": "SelectType", 
                            "name": "chargeTotalUnit", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Travel",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.chargeTotalUnitRequired, 
                                    lineItemPermision: params.data.lineItemPermision, invoicingStatus: params.data.invoicingStatus, 
                                    costofSalesStatus: params.data.costofSalesStatus, recordStatus: params.data.recordStatus };
                            },
                            "decimalType": "two",
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },                        
                        { 
                            "headerName": "Rate", 
                            "headerTooltip": "Rate", 
                            "tooltipField":"chargeRate", 
                            "field": "chargeRate",
                            "width": 100,
                            "cellRenderer": "SelectType", 
                            "name": "chargeRate", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Travel",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.chargeRateRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus, ratesReadonly: params.data.chargeRateReadonly };
                            },
                            "decimalType": "four",
                            "EditChargeRateRowHandler": (updatedData) => functionRefs.EditChargeRateRowHandler(updatedData),
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "Currency ", 
                            "headerTooltip": "Currency", 
                            "tooltipField":"chargeRateCurrency", 
                            "field": "chargeRateCurrency", 
                            "width":90, 
                            "cellRenderer": "SelectType",
                            "moduleName":'Visit', 
                            "selectListName": "CurrencyMasterData", 
                            "name": "chargeRateCurrency",
                            "optionName": "code", 
                            "optionValue": "code", 
                            "type": "select",
                            "moduleType": "Travel",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.chargeRateCurrencyRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        }
                                                
                    ]
                },
                {
                    "headerName": "Pay",
                    "children": [
                        { 
                            "headerName": " ", 
                            "headerTooltip": "",                             
                            "field": "costofSalesStatus",                            
                            "width":30, 
                            "moduleName":'Visit',
                            "cellRenderer": "SelectType",
                            "name": "CostofSalesStatus",
                            "type": "CostofSalesStatus",
                            "moduleType": "Travel",
                            "cellRendererParams": (params) => {                                
                                return { unPaidStatus: params.data.unPaidStatus, unPaidStatusReason: params.data.unPaidStatusReason };
                            },
                            "chargePayType": "pay",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "Pay Type", 
                            "headerTooltip": "Pay Type", 
                            "tooltipField":"payExpenseType", 
                            "field": "payExpenseType", 
                            "cellRenderer": "SelectType",
                            "width":120,                            
                            "moduleName":'Visit',
                            "selectListName": "TravelChargeType",
                            "name": "payExpenseType",
                            "optionName": "name",
                            "optionValue": "name",
                            "type": "select",
                            "moduleType": "Travel",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.payExpenseTypeRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "chargePayType": "pay",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },                    
                        { 
                            "headerName": "Units ", 
                            "headerTooltip": "Units", 
                            "tooltipField":"payUnit", 
                            "field": "payUnit",
                            "width":100, 
                            "cellRenderer": "SelectType", 
                            "name": "payUnit", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Travel",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.payUnitRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "decimalType": "two",
                            "chargePayType": "pay",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },                        
                        { 
                            "headerName": "Rate", 
                            "headerTooltip": "Rate",
                            "tooltip": (params) => {
                                if(params.data.lineItemPermision.isCHUser) {
                                    return " ";
                                } else {
                                    return params.data.payRate;
                                }
                            },
                            "field": "payRate",
                            "width": 100,
                            "cellRenderer": "SelectType", 
                            "name": "payRate", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Travel",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.payRateRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus, ratesReadonly: params.data.payRateReadonly };
                            },
                            "decimalType": "four",
                            "EditPayRateRowHandler": (updatedData) => functionRefs.EditPayRateRowHandler(updatedData),
                            "chargePayType": "pay",
                            "suppressKeyboardEvent": suppressArrowKeys  
                        },           
                        { 
                            "headerName": "Currency ", 
                            "headerTooltip": "Currency", 
                            "tooltipField":"payRateCurrency", 
                            "field": "payRateCurrency",
                            "width":90,
                            "cellRenderer": "SelectType", 
                            "moduleName":'Visit',
                            "selectListName": "CurrencyMasterData", 
                            "name": "payRateCurrency", 
                            "optionName": "code",
                            "optionValue": "code", 
                            "type": "select", 
                            "moduleType": "Travel",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.payRateCurrencyRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "chargePayType": "pay",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                    ]
                },                
            ],
            "rowSelection": 'multiple',         
            "pagination": false,
            "enableFilter": false,
            "enableSorting": true,
            //"gridHeight": 60,
            "searchable": false,
            "gridActions": true,
            "gridTitlePanel": false,
            "headerHeight":40,           
        },
        "consumableDetails":{
            "columnDefs":[
                {
                    "headerCheckboxSelectionFilteredOnly": true,
                    "checkboxSelection": (params) => {
                        return ((params.data.invoicingStatus && (params.data.invoicingStatus === 'C' || params.data.invoicingStatus === 'D')) 
                                || (params.data.costofSalesStatus && params.data.costofSalesStatus === 'X') ? false : true);
                    },
                    "suppressFilter": true,
                    "width": 23,
                    "pinned": 'left'
                },
                {
                    "headerName": "",
                    "rowClass" :"Parent",
                    "children": [
                        { 
                            "headerName": "Date",
                            "headerTooltip": "Date", 
                            "tooltip": (params) => {
                                let d='';
                                if(params.data.expenseDate){
                                 d = moment(params.data.expenseDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                                }
                                return d;
                            },
                            "rowClass" :"child", 
                            "field": 'expenseDate',
                            "width": 105,                            
                            "filter": "agDateColumnFilter",                            
                            "cellRenderer": "SelectType",  
                            "moduleName":'Visit',                            
                            "name": "expenseDate",
                            "type": "date",
                            "moduleType": "Consumable",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.expenseDateRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus, isLineItemEditableExpense: params.data.isLineItemEditableExpense,
                                    recordStatus: params.data.recordStatus, hasNoChargeRate: params.data.hasNoChargeRate };
                            },
                            "chargePayType": "",
                            "suppressKeyboardEvent": suppressArrowKeys,
                            "pinned": 'left'
                        }                        
                    ]
                },
                {
                    "headerName": "Charge",
                    "children": [       
                        { 
                            "headerName": "Description ", 
                            "headerTooltip": "Description", 
                            "tooltipField":"chargeDescription", 
                            "field": "chargeDescription",                             
                            "width":150, 
                            "moduleName":'Visit',
                            "cellRenderer": "SelectType",
                            "name": "chargeDescription",
                            "maxLength": fieldLengthConstants.visit.resourceAccounts.consumable.CHARGE_DESCRIPTION_MAXLENGTH,
                            "type": "textarea",
                            "moduleType": "Consumable",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, lineItemPermision: params.data.lineItemPermision, invoicingStatus: params.data.invoicingStatus,
                                     costofSalesStatus: params.data.costofSalesStatus, isLineItemEditableExpense: params.data.isLineItemEditableExpense, recordStatus: params.data.recordStatus };
                            },
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        {
                            "headerName":  localConstant.gridHeader.DESCRIPTION_PRINT_ON_INVOICE_ICON,
                            "headerTooltip": localConstant.gridHeader.DESCRIPTION_PRINT_ON_INVOICE,
                            "field": "isInvoicePrintChargeDescription",
                            "tooltip": (params) => {
                                if (params.data.isInvoicePrintChargeDescription === true) {
                                    return "Yes";
                                } else {
                                    return "No";
                                }
                            },
                            "filter": "agTextColumnFilter",
                            "width":45,
                            "cellRenderer": "SelectType",
                            "name": "isInvoicePrintChargeDescription",
                            "type": "switch",
                            "moduleName":'Visit',
                            "moduleType": "Consumable",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, lineItemPermision: params.data.lineItemPermision, invoicingStatus: params.data.invoicingStatus,
                                     costofSalesStatus: params.data.costofSalesStatus, isLineItemEditableExpense: params.data.isLineItemEditableExpense, recordStatus: params.data.recordStatus,hasNoChargeRate: params.data.hasNoChargeRate };
                            },
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },                 
                        { 
                            "headerName": " ", 
                            "headerTooltip": "",                             
                            "field": "invoicingStatus",
                            "width":30, 
                            "moduleName":'Visit',
                            "cellRenderer": "SelectType",
                            "name": "InvoiceStatus",
                            "type": "InvoiceStatus",
                            "moduleType": "Consumable",
                            "cellRendererParams": (params) => {                                
                                return { unPaidStatus: params.data.unPaidStatus, unPaidStatusReason: params.data.unPaidStatusReason };
                            },
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "Charge Type ", 
                            "headerTooltip": "Charge Type", 
                            "tooltipField":"chargeExpenseType",
                            "field": "chargeExpenseType",
                            "cellRenderer": "SelectType",
                            "width":110,                            
                            "moduleName":'Visit',
                            "selectListName": "ConsumableChargeType",
                            "name": "chargeExpenseType",
                            "optionName": "name",
                            "optionValue": "name",
                            "type": "select",
                            "moduleType": "Consumable",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.chargeExpenseTypeRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus, recordStatus: params.data.recordStatus, hasNoChargeRate: params.data.hasNoChargeRate  };
                            },
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "Units ", 
                            "headerTooltip": "Units", 
                            "tooltipField":"chargeTotalUnit",
                            "field": "chargeTotalUnit",
                            "width":100, 
                            "cellRenderer": "SelectType", 
                            "name": "chargeTotalUnit", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Consumable",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.chargeTotalUnitRequired, 
                                    lineItemPermision: params.data.lineItemPermision, invoicingStatus: params.data.invoicingStatus, 
                                    costofSalesStatus: params.data.costofSalesStatus, recordStatus: params.data.recordStatus };
                            },
                            "decimalType": "two",
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },                        
                        { 
                            "headerName": "Rate", 
                            "tooltipField":"chargeRate",
                            "headerTooltip": "Rate", 
                            "field": "chargeRate",
                            "width": 100,
                            "cellRenderer": "SelectType", 
                            "name": "chargeRate", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Consumable",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.chargeRateRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus, ratesReadonly: params.data.chargeRateReadonly };
                            },
                            "decimalType": "four",
                            "EditChargeRateRowHandler": (updatedData) => functionRefs.EditChargeRateRowHandler(updatedData),
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "Currency ", 
                            "tooltipField":"chargeRateCurrency", 
                            "headerTooltip": "Currency", 
                            "field": "chargeRateCurrency",
                            "width":90, 
                            "cellRenderer": "SelectType",
                            "moduleName":'Visit', 
                            "selectListName": "CurrencyMasterData", 
                            "name": "chargeRateCurrency",
                            "optionName": "code", 
                            "optionValue": "code", 
                            "type": "select", 
                            "moduleType": "Consumable",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.chargeRateCurrencyRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "chargePayType": "charge",
                            "suppressKeyboardEvent": suppressArrowKeys
                        }                                                
                    ]
                },
                {
                    "headerName": "Pay",
                    "children": [
                        { 
                            "headerName": " ", 
                            "headerTooltip": "",                             
                            "field": "costofSalesStatus",                            
                            "width":30, 
                            "moduleName":'Visit',
                            "cellRenderer": "SelectType",
                            "name": "CostofSalesStatus",
                            "type": "CostofSalesStatus",
                            "moduleType": "Consumable",
                            "cellRendererParams": (params) => {                                
                                return { unPaidStatus: params.data.unPaidStatus, unPaidStatusReason: params.data.unPaidStatusReason };
                            },
                            "chargePayType": "pay",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "Description ", 
                            "headerTooltip": "Description", 
                            "tooltipField":"payRateDescription", 
                            "field": "payRateDescription",
                            "width":150, 
                            "moduleName":'Visit',
                            "cellRenderer": "SelectType",
                            "name": "payRateDescription",
                            "maxLength": fieldLengthConstants.visit.resourceAccounts.consumable.PAY_RATE_DESCRIPTION_MAXLENGTH,
                            "type": "textarea",
                            "moduleType": "Consumable",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, lineItemPermision: params.data.lineItemPermision, invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "chargePayType": "pay",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "Pay Type ", 
                            "headerTooltip": "Pay Type", 
                            "tooltipField":"payExpenseType",
                            "field": "payExpenseType",
                            "cellRenderer": "SelectType",
                            "width":100,                            
                            "moduleName":'Visit',
                            "selectListName": "ConsumableChargeType",
                            "name": "payExpenseType",
                            "optionName": "name",
                            "optionValue": "name",
                            "type": "select",
                            "moduleType": "Consumable",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.payExpenseTypeRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus, recordStatus: params.data.recordStatus
                                    , hasNoChargeRate: params.data.hasNoChargeRate };
                            },
                            "chargePayType": "pay",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                        { 
                            "headerName": "Units ", 
                            "headerTooltip": "Units", 
                            "tooltipField":"payUnit",
                            "field": "payUnit",
                            "width":100, 
                            "cellRenderer": "SelectType", 
                            "name": "payUnit", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Consumable",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.payUnitRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "decimalType": "two",
                            "chargePayType": "pay",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },                             
                        { 
                            "headerName": "Rate", 
                            "headerTooltip": "Rate",
                            "tooltip": (params) => {
                                if(params.data.lineItemPermision.isCHUser) {
                                    return " ";
                                } else {
                                    return params.data.payRate;
                                }
                            },
                            "field": "payRate",
                            "width": 100,
                            "cellRenderer": "SelectType", 
                            "name": "payRate", 
                            "type": "text", 
                            "moduleName":'Visit', 
                            "moduleType": "Consumable",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.payRateRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus, ratesReadonly: params.data.payRateReadonly };
                            },
                            "decimalType": "four",
                            "EditPayRateRowHandler": (updatedData) => functionRefs.EditPayRateRowHandler(updatedData),
                            "chargePayType": "pay",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },                 
                        { 
                            "headerName": "Currency ", 
                            "headerTooltip": "Currency",
                            "tooltipField":"payRateCurrency",
                            "field": "payRateCurrency",
                            "width":90, 
                            "cellRenderer": "SelectType", 
                            "moduleName":'Visit',
                            "selectListName": "CurrencyMasterData", 
                            "name": "payRateCurrency", 
                            "optionName": "code",
                            "optionValue": "code", 
                            "type": "select", 
                            "moduleType": "Consumable",
                            "cellRendererParams": (params) => {                                
                                return { rowIndex: params.node.rowIndex, validation: params.data.payRateCurrencyRequired, lineItemPermision: params.data.lineItemPermision,
                                    invoicingStatus: params.data.invoicingStatus, costofSalesStatus: params.data.costofSalesStatus };
                            },
                            "chargePayType": "pay",
                            "suppressKeyboardEvent": suppressArrowKeys
                        },
                    ]
                },
            ],
            "rowSelection": 'multiple',         
            "pagination": false,
            "enableFilter": false,
            "enableSorting": true,
            //"gridHeight": 60,
            "searchable": false,
            "gridActions": true,
            "gridTitlePanel": false,
            "headerHeight":40,           
        },
        "UnLinkedAssignmentExpenses":{
            "columnDefs": [
                {                    
                    "checkboxSelection": true,
                    "headerCheckboxSelectionFilteredOnly": true,                    
                    "suppressFilter": true,
                    "width": 40,
                },
                {
                    "headerName": localConstant.gridHeader.DESCRIPTION,
                    "field": "description",
                    "width":150,
                    "tooltipField":"description",        
                    "headerTooltip": localConstant.gridHeader.DESCRIPTION,
                    "filter": "agTextColumnFilter",
                },
                {
                    "headerName": localConstant.gridHeader.CURRENCY,
                    "field": "currencyCode",               
                    "width":90,
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
                    "width": 80,
                    "tooltipField":"perUnitRate",        
                    "headerTooltip": localConstant.gridHeader.EXPENSE_RATE,
                    "filter": "agTextColumnFilter",
                    "valueGetter": (params) => {                            
                        if(params.data.perUnitRate === undefined) {
                            return params.data.perUnitRate;
                        } else {
                            return thousandFormat(params.data.perUnitRate);
                        }
                    },
                },
                {
                    "headerName": localConstant.gridHeader.UNITS,
                    "field": "totalUnit",
                    "width": 80,
                    "tooltipField":"totalUnit",        
                    "headerTooltip": localConstant.gridHeader.UNITS,
                    "filter": "agTextColumnFilter",
                },
                {
                    "headerName": localConstant.gridHeader.ALREADY_LINKED,
                    "field": "isAlreadyLinked",
                    "width": 100,
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
                }
            ],
    
            "rowSelection": 'multiple',
            "pagination": false,
            "enableFilter": false,
            "enableSorting": false,
            "gridHeight": 60,
            "searchable": false,
            "gridActions": false,
            "gridTitlePanel": true,
        }
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