import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData = {
    "columnDefs": [
        {
            "headerName": localConstant.gridHeader.PURCHSE_ORDER_NUMBER,
            "field": "supplierPONumber",
            "filter": "agNumberColumnFilter",
            "cellRenderer": "SupplierpoAnchor",
            "cellRendererParams": (params) => {                
                return { 
                    supplierPONumber: params.data.supplierPONumber,
                    supplierPOId: params.data.supplierPOId,                        
                };
            },
            "width":220,  
            "tooltipField":"supplierPONumber",        
            "headerTooltip": localConstant.gridHeader.PURCHSE_ORDER_NUMBER,
            "filterParams": {
                "inRangeInclusive":true
            }            
        },
        {
            "headerName": localConstant.gridHeader.MAIN_SUPPLIER,
            "field": "supplierPOMainSupplierName",
            "filter": "agTextColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "width":180,
            "headerTooltip": localConstant.gridHeader.MAIN_SUPPLIER,  
            "tooltipField":"supplierPOMainSupplierName",
        },
        {
            "headerName": localConstant.gridHeader.MATERIAL_DESCRIPTION,
            "field": "supplierPOMaterialDescription",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.MATERIAL_DESCRIPTION,
            "width":200,  
            "tooltipField":"supplierPOMaterialDescription"
        },
        {
            "headerName": localConstant.gridHeader.DELIVERY_DATE,
            "field": "supplierPODeliveryDate",
            "filter": "agDateColumnFilter", 
            "headerTooltip": localConstant.gridHeader.DELIVERY_DATE,
            "width":220,
            "tooltip": (params) => {
                return moment(params.data.supplierPODeliveryDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "supplierPODeliveryDate"
            },
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },
        {
            "headerName": localConstant.gridHeader.STATUS,
            "field": "supplierPOStatus",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.STATUS,
            "width":160,
            "tooltip":(params) => {
                if(params.data.supplierPOStatus === 'O'){
                    return "Open";
                }else if(params.data.supplierPOStatus === 'C'){
                  return "Closed";      
                        }
                
              },
            "valueGetter": (params) => {
                if(params.data.supplierPOStatus === 'O'){
                    return "Open";
                }else if(params.data.supplierPOStatus === 'C'){
                  return "Closed";      
                        }
                
              }
        },
        {
            "headerName": localConstant.gridHeader.COMPLETE_DATE,
            "field": "supplierPOCompletedDate",
            "filter": "agDateColumnFilter",
            "headerTooltip": localConstant.gridHeader.COMPLETE_DATE,
            "width":180,
            "tooltip": (params) => {
                return moment(params.data.supplierPOCompleteDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        },      
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "supplierPOCompletedDate"
            },
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        }
    ],
    "enableFilter":true, 
    "enableSorting":true, 
    "pagination": true,
    "searchable":true,
    "gridActions":true,
    "gridTitlePanel":true,
    "gridHeight":56,
    "multiSortKey": "ctrl",
    "clearFilter":true, 
    "gridRefresh":true,
    "exportFileName":"Supplier Po",
    "columnsFiltersToDestroy":[ 'supplierPODeliveryDate','supplierPOCompletedDate' ]
};