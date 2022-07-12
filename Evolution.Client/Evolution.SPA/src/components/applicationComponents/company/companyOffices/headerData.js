import { getlocalizeData } from '../../../../utils/commonUtils';

const localConstant = getlocalizeData();
export const HeaderData ={
    "columnDefs": [              
        {
            "headerCheckboxSelectionFilteredOnly": true,
            "checkboxSelection": true,
            "suppressFilter": true,
            "width":40
        },
        {
            "headerName": localConstant.gridHeader.OFFICE_NAME,
            "field": "officeName",            
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.OFFICE_NAME,
            "tooltipField": "officeName",
            "width": 145
        },
        {
            "headerName": localConstant.gridHeader.ACCOUNTS_REFERENCE,
            "field": "accountRef",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.ACCOUNTS_REFERENCE,
            "tooltipField": "accountRef",
            "width": 180
        },
        {
            "headerName": localConstant.gridHeader.FULL_ADDRESS,
            "field": "fullAddress",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.FULL_ADDRESS,
            "tooltipField": "fullAddress",
            "width": 150
        },
        {
            "headerName": localConstant.gridHeader.COUNTRY,
            "field": "country",
            "headerTooltip": localConstant.gridHeader.COUNTRY,
            "filter": "agTextColumnFilter",
            "tooltipField": "country",
            "width": 120
        },
        {
            "headerName": localConstant.gridHeader.STATE,
            "field": "state",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.STATE,
            "tooltipField": "state",
            "width": 208
        },        
        {
            "headerName": localConstant.gridHeader.CITY,
            "field": "city",
            "filter": "agTextColumnFilter", 
            "headerTooltip": localConstant.gridHeader.CITY,     
            "tooltipField": "city",      
            "width": 100
        },
        {
            "headerName": localConstant.gridHeader.POSTAL_CODE,
            "field": "postalCode",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.POSTAL_CODE,   
            "tooltipField": "postalCode",
            "width": 150
        },
        // {
        //     "headerName": "",
        //     "field": "addressId",
        //     "cellRenderer": "EditRenderer",
        //     "cellRendererParams": {
        //         "action": "EditCompanyOffice",
        //         "popupId": "add-location"
        //     },
        //     "suppressFilter": true,
        //     "suppressSorting": true,
        //     "width": 80
        // },       
        {
            "headerName": "",
            "field": "EditColumn",
            "cellRenderer": "EditLink",
            "buttonAction":"",
            "cellRendererParams": {
            },
            "suppressFilter": true,
            "suppressSorting": true,
            "width": 50
        },
        {
            "headerName": localConstant.gridHeader.RECORD_STATUS,
            "field": "recordStatus",
            "hide": true
        },
        {
            "headerName": "Address Id",
            "field": "addressId",
            "hide": true
        }
    ],
    "enableSelectAll":true,
    "enableFilter": true,
    "enableSorting": true,
    "pagination": true,
    "searchable":true,
    "rowSelection": "multiple",
    "gridHeight":60,
    "clearFilter":true, 
    "exportFileName":"Company Offices"
};