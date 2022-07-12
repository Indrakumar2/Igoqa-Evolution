import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();
export const HeaderData ={
    "columnDefs": [       
        {
            "headerName": localConstant.customerSearch.CUSTOMER_CODE,
            "field": "customerCode",
            "cellRenderer": "customerAnchorRenderer",
            "headerTooltip": localConstant.customerSearch.CUSTOMER_CODE,
            "tooltipField": "customerCode",          
            "width":200
        },
        {
            "headerName": localConstant.customerSearch.PARENT_COMPANY_NAME,
            "field": "parentCompanyName",
            "headerTooltip":localConstant.customerSearch.PARENT_COMPANY_NAME,
            "tooltipField": "parentCompanyName", 
            "width":400
        },
        {
            "headerName": localConstant.customerSearch.CUSTOMER_COMPANY_NAME,
            "field": "customerName",
            "headerTooltip":localConstant.customerSearch.CUSTOMER_COMPANY_NAME,
            "tooltipField":"customerName",
            "width":742
        }
    ],
    "pagination": true,
    "enableFilter":true, 
    "enableSorting":true, 
    "gridHeight":66,
    "searchable":true,
    "gridActions":true,
    "gridTitlePanel":true,
    "clearFilter":true, 
    "exportFileName":"Customer Search Data"

};