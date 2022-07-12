import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();
export const HeaderData = (functionRefs)=> {  //D456 issue      
    return {
    supplierDetailHeader: {
        "columnDefs": [
            {
                "checkboxSelection": true,
                "headerCheckboxSelectionFilteredOnly": true,
                "suppressFilter": true,
                "width": 50
            },
            {
                "headerName": localConstant.gridHeader.supplierDetail.CONTACT_NAME,
                "field": "supplierContactName",
                "tooltipField": "supplierContactName",
                "headerTooltip": localConstant.gridHeader.supplierDetail.CONTACT_NAME,
                "filter": "agTextColumnFilter",
                "width": 180
            },
            {
                "headerName": localConstant.gridHeader.supplierDetail.TELEPHONE_NUMBER,
                "field": "supplierTelephoneNumber",
                "width": 150,
                "tooltipField": "supplierTelephoneNumber",
                "headerTooltip": localConstant.gridHeader.supplierDetail.TELEPHONE_NUMBER,
                "filter": "agTextColumnFilter",
            },
            {
                "headerName": localConstant.gridHeader.supplierDetail.FAX_NUMBER,
                "field": "supplierFaxNumber",
                "width": 130,
                "tooltipField": "supplierFaxNumber",
                "headerTooltip": localConstant.gridHeader.supplierDetail.FAX_NUMBER,
                "filter": "agTextColumnFilter",
            },
            {
                "headerName": localConstant.gridHeader.supplierDetail.MOBILE_NUMBER,
                "field": "supplierMobileNumber",
                "width": 150,
                "tooltipField": "supplierMobileNumber",
                "headerTooltip": localConstant.gridHeader.supplierDetail.MOBILE_NUMBER,
                "filter": "agTextColumnFilter",
            },
            {
                "headerName": localConstant.gridHeader.supplierDetail.EMAIL,
                "field": "supplierEmail",
                "width": 130,
                "tooltipField": "supplierEmail",
                "headerTooltip": localConstant.gridHeader.supplierDetail.EMAIL,
                "filter": "agTextColumnFilter",
            },
            {
                "headerName": localConstant.gridHeader.supplierDetail.OTHER_CONTRACT_DETAILS,
                "field": "otherContactDetails",
                "width": 220,
                "tooltipField": "otherContactDetails",
                "headerTooltip": localConstant.gridHeader.supplierDetail.OTHER_CONTRACT_DETAILS,
                "filter": "agTextColumnFilter",
            },
            {
                "headerName": "",
                "field": "SupplierPORenderColumn",
                "cellRenderer": "EditLink",
                "buttonAction": "",
                "cellRendererParams": {
                },
                "suppressFilter": true,
                "suppressSorting": true,
                "width": 60
            }
        ],
        "pagination": true,
        "enableFilter": true,
        "enableSorting": true,
        "gridHeight": 59,
        "searchable": true,
        "gridActions": true,
        "gridTitlePanel": true,
        "rowSelection": "multiple",
        "clearFilter":true, 
        "exportFileName": "Supplier Details"
    },
    supplierSearchHeader: {
        "columnDefs": [
            {
                "headerCheckboxSelectionFilteredOnly": true,
                "checkboxSelection": true,
                "suppressFilter": true,
                "width": 40
            },
            {
                "headerName": localConstant.gridHeader.NAME,
                "field": "supplierName",
                "headerTooltip":localConstant.gridHeader.NAME,
                "tooltipField": "supplierName",
                "filter": "agTextColumnFilter",
                "width": 170
            },
            {
                "headerName": localConstant.gridHeader.COUNTRY,
                "field": "country",
                "headerTooltip":localConstant.gridHeader.COUNTRY,
                "tooltipField": "country",
                "filter": "agTextColumnFilter",
                "width": 100
            },
            {
                "headerName": localConstant.gridHeader.COUNTY,
                "field": "state",
                "headerTooltip":localConstant.gridHeader.COUNTY,
                "tooltipField": "state",
                "filter": "agTextColumnFilter",
                "width": 100
            },
            {
                "headerName": localConstant.gridHeader.CITY,
                "field": "city",
                "headerTooltip":localConstant.gridHeader.CITY,
                "tooltipField": "city",
                "filter": "agTextColumnFilter",
                "width": 100
            },
            {
                "headerName": localConstant.gridHeader.FULL_ADDRESS,
                "field": "supplierAddress",
                "headerTooltip":localConstant.gridHeader.FULL_ADDRESS,
                "tooltipField": "supplierAddress",
                "filter": "agTextColumnFilter",
                "width": 210
            },
        ],
        "rowSelection": 'single',       
        "enableSorting": true,
        "gridHeight": 50,
        "pagination": true,
        "enableFilter": true,  
        "gridTitlePanel": true,
        "clearFilter":true, 
        "searchable": true,      
    },
    subSupplierDetailHeader: {
        "columnDefs": [
            {
                "headerCheckboxSelectionFilteredOnly": true,
                "checkboxSelection": true,
                "suppressFilter": true,
                "width": 40
            },
            {
                "headerName": localConstant.gridHeader.SUPPLIER_NAME,
                "field": "subSupplierName",
                "headerTooltip":localConstant.gridHeader.SUPPLIER_NAME,
                "tooltipField": "subSupplierName",
                "filter": "agTextColumnFilter",
                "width": 280
            },
            {
                "headerName": localConstant.gridHeader.SUPPLIER_ADDRESS,
                "field": "subSupplierAddress",
                "headerTooltip":localConstant.gridHeader.SUPPLIER_ADDRESS,
                "tooltipField": "subSupplierAddress",
                "filter": "agTextColumnFilter",
                "width": 621
            },
            {
                "headerName": "",
                "field": "editSubSupplier",
                "cellRenderer": "EditLink",
                "cellRendererParams": {
                    "disableField": (e) => functionRefs.disableEditColumn(e)  //D456 issue   
                },
                "suppressFilter": true,
                "suppressSorting": true,
                "width": 60
            },
            {
                "headerName": "",
                "field": "viewSubSupplier",
                "cellRenderer": 'EditLink',
                "cellRendererParams": {
                    'displayLink': 'View',                   
                    "disableField": (e) => functionRefs.disableEditColumn(e)  //D456 issue      
                },
                "suppressFilter": true,
                "suppressSorting": true,
                "width": 60
            }
        ],
        "enableSelectAll":true,        
        "enableSorting": true,
        "rowSelection": 'multiple',
        "gridHeight": 59,
        "pagination": true,
        "enableFilter": true,
        "gridTitlePanel": true,
        "clearFilter":true, 
        "searchable": true,
        }
    };
};

export default HeaderData;