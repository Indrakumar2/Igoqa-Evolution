import { getlocalizeData } from '../../../utils/commonUtils';
const localConstant = getlocalizeData();
export const HeaderData = {
    "columnDefs": [ 
        {
            "headerName": localConstant.gridHeader.supplier.SUPPLIER_NAME,
            "field": "supplierName",
            "cellRenderer": "supplierAnchorRenderer", 
            "tooltipField":"supplierName",        
            "headerTooltip": localConstant.gridHeader.supplier.SUPPLIER_NAME,
            "filter": "agTextColumnFilter",
            "width":250
        },
        {
            "headerName": localConstant.gridHeader.supplier.COUNTRY,
            "field": "country",
            "width":200,
            "tooltipField":"country",        
            "headerTooltip": localConstant.gridHeader.supplier.COUNTRY,
            "filter": "agTextColumnFilter",
        },
        {
            "headerName": localConstant.gridHeader.supplier.STATE,
            "field": "state",                        
            "width":220,
            "tooltipField":"state",        
            "headerTooltip": localConstant.gridHeader.supplier.STATE,
            "filter": "agTextColumnFilter",
        },
        {
            "headerName": localConstant.gridHeader.supplier.CITY,
            "field": "city",                      
            "width":200,
            "tooltipField":"city",        
            "headerTooltip": localConstant.gridHeader.supplier.CITY,
            "filter": "agTextColumnFilter",
        },
        {
            "headerName": localConstant.gridHeader.supplier.FULL_ADDRESS,
            "field": "supplierAddress",
            "width": 473,
            "tooltipField":"supplierAddress",        
            "headerTooltip": localConstant.gridHeader.supplier.FULL_ADDRESS,
            "filter": "agTextColumnFilter",
        }
    ],
    "pagination": true,
    "enableFilter":true, 
    "enableSorting":true, 
    "gridHeight":59,
    "searchable":true,
    "gridActions":true,
    "gridTitlePanel":true,
    "clearFilter":true, 
    "exportFileName":"Supplier Search Data"
};

export default HeaderData;