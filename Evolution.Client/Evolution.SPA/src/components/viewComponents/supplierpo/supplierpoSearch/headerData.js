import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const headerData = {
    subsupplierSearchHeader:{
        "columnDefs": [            
            {
                "headerCheckboxSelectionFilteredOnly": true,
                "checkboxSelection": true,
                "suppressFilter": true,
                "width": 40
            },
            {
                "headerName": localConstant.gridHeader.NAME,
                "headerTooltip": localConstant.gridHeader.NAME,
                "field": "supplierName",
                "tooltipField": "supplierName",
                "filter": "agTextColumnFilter",
                "width": 170
            },
            {
                "headerName": localConstant.gridHeader.COUNTRY,
                "headerTooltip": localConstant.gridHeader.COUNTRY,
                "field": "country",
                "tooltipField": "country",
                "filter": "agTextColumnFilter",
                "width": 100
            },
            {
                "headerName": localConstant.gridHeader.COUNTY,
                "headerTooltip": localConstant.gridHeader.COUNTY,
                "field": "state",
                "tooltipField": "state",
                "filter": "agTextColumnFilter",
                "width": 100
            },
            {
                "headerName": localConstant.gridHeader.CITY,
                "headerTooltip": localConstant.gridHeader.CITY,
                "field": "city",
                "tooltipField": "city",
                "filter": "agTextColumnFilter",
                "width": 100
            },
            {
                "headerName": localConstant.gridHeader.supplierAddress,
                "headerTooltip": localConstant.gridHeader.supplierAddress,
                "field": "supplierAddress",
                "tooltipField": "supplierAddress",
                "filter": "agTextColumnFilter",          
                "width": 210
            },
            
        ],
        "rowSelection": 'single',
        "gridHeight": 50,
        "searchable": true,
        "gridActions": true,
        "gridTitlePanel": true,
        "enableFilter":true,
        "enableSorting": true,
        "pagination": true,
        "clearFilter":true, 
        "exportFileName":"Sub-Supplier List"
    },
    supplierPoSearch:{
        "columnDefs": [
            {
                "headerName": localConstant.gridHeader.SUPPLIER_PO_NAME,
                "headerTooltip":localConstant.gridHeader.SUPPLIER_PO_NAME,
                "field": "supplierPONumber",
                "tooltipField": "supplierPONumber",
                "filter": "agTextColumnFilter",
                "filterParams": {
                    "inRangeInclusive":true
                },
                "width": 200,
                "cellRenderer": "SupplierpoAnchor",
                // "cellRendererParams": (params) => {                
                //     return { 
                //         supplierPONumber: params.data.supplierPONumber,
                //         supplierPOId: params.data.supplierPOId,                        
                //     };
                // },
            },
            {
                "headerName": localConstant.gridHeader.CUSTOMER,
                "headerTooltip":localConstant.gridHeader.CUSTOMER,
                "field": "supplierPOCustomerName",
                "tooltipField": "supplierPOCustomerName",
                "filter": "agTextColumnFilter",
                "width": 200,
                
            },
            {
                "headerName": localConstant.gridHeader.CONTRACT_NO,
                "headerTooltip":localConstant.gridHeader.CONTRACT_NO,
                "field": "supplierPOContractNumber",
                "tooltipField": "supplierPOContractNumber",
                "filter": "agTextColumnFilter",
                // "cellRenderer": "contractAnchorRenderer",
                "width": 200
            },
            {
                "headerName": localConstant.gridHeader.PROJECT_NO,
                "headerTooltip":localConstant.gridHeader.PROJECT_NO,
                "field": "supplierPOProjectNumber",
                "tooltipField": "supplierPOProjectNumber",
                "filter": "agNumberColumnFilter", 
                "filterParams": {
                    "inRangeInclusive":true
                },
                "cellRenderer": "projectAnchorRenderer",
                "width": 205
            },
            {
                "headerName": localConstant.gridHeader.CUSTOMER_PROJECT_NAME,
                "headerTooltip": localConstant.gridHeader.CUSTOMER_PROJECT_NAME,
                "field": "supplierPOCustomerProjectName",
                "tooltipField": "supplierPOCustomerProjectName",
                "filter": "agTextColumnFilter",     
                "width": 200,
                
            },
            {
                "headerName": localConstant.gridHeader.MAIN_SUPPLIER,
                "headerTooltip": localConstant.gridHeader.MAIN_SUPPLIER,
                "field": "supplierPOMainSupplierName",
                "tooltipField": "supplierPOMainSupplierName",
                "sort": "asc",
                "filter": "agTextColumnFilter",
                "width": 200,
               
            }, {
                "headerName": localConstant.gridHeader.SUB_SUPLIER,
                "headerTooltip": localConstant.gridHeader.SUB_SUPLIER,
                "field": "supplierPOSubSupplierName",
                "tooltipField": "supplierPOSubSupplierName",
                "filter": "agTextColumnFilter",
                "width": 200,
                
            }, {
                "headerName": localConstant.gridHeader.MATERIAL_DESCRIPTION,
                "headerTooltip": localConstant.gridHeader.MATERIAL_DESCRIPTION,
                "field": "supplierPOMaterialDescription",
                "tooltipField": "supplierPOMaterialDescription",
                "filter": "agTextColumnFilter",
                "width": 200,
              
            }, {
                "headerName": localConstant.gridHeader.DELIVERY_DATE,
                "headerTooltip": localConstant.gridHeader.DELIVERY_DATE,
                "field": "supplierPODeliveryDate",
                "tooltip": (params) => {
                    return moment(params.data.supplierPODeliveryDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                  
            }, 
                "filter": "agDateColumnFilter",
                "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "dataToRender": "supplierPODeliveryDate"
                },
                "filterParams": {
                    "comparator": dateUtil.comparator,
                    "inRangeInclusive": true,
                    "browserDatePicker": true
                  },
                "width": 200
            }, {
                "headerName": localConstant.gridHeader.STATUS,
                "headerTooltip": localConstant.gridHeader.STATUS,
                "field": "supplierPOStatus",
                "tooltip": (params) => {
                    if (params.data.supplierPOStatus === 'O') {
                        return "Open";
                    } else {
                        return "Closed";
                    }
                },
                "filter": "agTextColumnFilter",
                "valueGetter": (params) => {
                    if (params.data.supplierPOStatus === 'O') {
                        return "Open";
                    } else {
                        return "Closed";
                    }
                },
                "width": 200
            }, {
                "headerName": localConstant.gridHeader.COMPLETE_DATE,
                "headerTooltip": localConstant.gridHeader.COMPLETE_DATE,
                "field": "supplierPOCompletedDate",
                "tooltip": (params) => {
                    return moment(params.data.supplierPOCompleteDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
            }, 
                "filter": "agDateColumnFilter",
                "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "dataToRender": "supplierPOCompletedDate"
                },
                "filterParams": {
                    "comparator": dateUtil.comparator,
                    "inRangeInclusive": true,
                    "browserDatePicker": true
                  },
                "width": 200
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
        "exportFileName":"Supplier PO Search Data",
        "columnsFiltersToDestroy":[ 'supplierPOCompletedDate','supplierPODeliveryDate' ]
    },
    supplierSearchHeader:{
        "columnDefs": [            
            {
                "headerCheckboxSelectionFilteredOnly": true,
                "checkboxSelection": true,
                "suppressFilter": true,
                "width": 40
            },
            {
                "headerName": localConstant.gridHeader.NAME,
                "headerTooltip": localConstant.gridHeader.NAME,
                "field": "supplierName",
                "tooltipField": "supplierName",
                "filter": "agTextColumnFilter",
                "width": 170
            },
            {
                "headerName": localConstant.gridHeader.COUNTRY,
                "headerTooltip": localConstant.gridHeader.COUNTRY,
                "field": "country",
                "tooltipField": "country",
                "filter": "agTextColumnFilter",
                "width": 100
            },
            {
                "headerName": localConstant.gridHeader.COUNTY,
                "headerTooltip": localConstant.gridHeader.COUNTY,
                "field": "state",
                "tooltipField": "state",
                "filter": "agTextColumnFilter",
                "width": 100
            },
            {
                "headerName": localConstant.gridHeader.CITY,
                "headerTooltip": localConstant.gridHeader.CITY,
                "field": "city",
                "tooltipField": "city",
                "filter": "agTextColumnFilter",
                "width": 100
            },
            {
                "headerName": localConstant.gridHeader.supplierAddress,
                "headerTooltip": localConstant.gridHeader.supplierAddress,
                "field": "supplierAddress",
                "tooltipField": "supplierAddress",
                "filter": "agTextColumnFilter",          
                "width": 210
            },
            
        ],
        "rowSelection": 'single',
        "gridHeight": 50,
        "searchable": true,
        "gridActions": true,
        "gridTitlePanel": true,
        "enableFilter":true,
        "enableSorting": true,
        "pagination": true,
        "clearFilter":true, 
        "exportFileName":"Supplier List"
    }
};