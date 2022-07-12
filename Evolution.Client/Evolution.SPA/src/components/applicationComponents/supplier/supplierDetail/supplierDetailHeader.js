import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();
export const HeaderData = {
    "columnDefs": [ 
        {
            "checkboxSelection": true,
            "headerCheckboxSelectionFilteredOnly": true,
            "suppressFilter": true,
            "width": 43
        },
        {
            "headerName": localConstant.gridHeader.supplierDetail.CONTACT_NAME,
            "field": "supplierContactName",
            "tooltipField":"supplierContactName",        
            "headerTooltip": localConstant.gridHeader.supplierDetail.CONTACT_NAME,
            "filter": "agTextColumnFilter",
            "width":180
        },
        {
            "headerName": localConstant.gridHeader.supplierDetail.TELEPHONE_NUMBER,
            "field": "supplierTelephoneNumber",
            "width":150,
            "tooltipField":"supplierTelephoneNumber",        
            "headerTooltip": localConstant.gridHeader.supplierDetail.TELEPHONE_NUMBER,
            "filter": "agTextColumnFilter",
        },
        {
            "headerName": localConstant.gridHeader.supplierDetail.FAX_NUMBER,
            "field": "supplierFaxNumber",               
            "width":130,
            "tooltipField":"supplierFaxNumber",        
            "headerTooltip": localConstant.gridHeader.supplierDetail.FAX_NUMBER,
            "filter": "agTextColumnFilter",
        },
        {
            "headerName": localConstant.gridHeader.supplierDetail.MOBILE_NUMBER,
            "field": "supplierMobileNumber",                      
            "width":150,
            "tooltipField":"supplierMobileNumber",        
            "headerTooltip": localConstant.gridHeader.supplierDetail.MOBILE_NUMBER,
            "filter": "agTextColumnFilter",
        },
        {
            "headerName": localConstant.gridHeader.supplierDetail.EMAIL,
            "field": "supplierEmail",
            "width": 130,
            "tooltipField":"supplierEmail",        
            "headerTooltip": localConstant.gridHeader.supplierDetail.EMAIL,
            "filter": "agTextColumnFilter",
        },
        {
            "headerName": localConstant.gridHeader.supplierDetail.OTHER_CONTRACT_DETAILS,
            "field": "otherContactDetails",
            "width": 220,
            "tooltipField":"otherContactDetails",        
            "headerTooltip": localConstant.gridHeader.supplierDetail.OTHER_CONTRACT_DETAILS,
            "filter": "agTextColumnFilter",
        },
        {
            "headerName": "",
            "field": "SupplierRenderColumn",
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
    "rowSelection": "multiple",
    "clearFilter":true, 
    "exportFileName":"Supplier Contact Details"
};

export default HeaderData;