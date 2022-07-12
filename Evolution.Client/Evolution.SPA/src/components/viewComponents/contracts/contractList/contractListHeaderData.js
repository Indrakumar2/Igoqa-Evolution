export const HeaderData = {

    "columnDefs": [
        {
            "headerCheckboxSelectionFilteredOnly": true,
            "checkboxSelection": true,
            "suppressFilter": true,
            "width": 40
        },
        {
            "headerName": "Code",
            "field": "customerCode",
            "filter": "agTextColumnFilter",
            "width":300
        },
        {
            "headerName": "Name",
            "field": "customerName",
            "filter": "agTextColumnFilter",
            "width":340
        }
    ],
    "enableFilter": false,
    "enableSorting": true,
    "pagination": false,
    "gridHeight": 45,
    "clearFilter":true, 
    "rowSelection": 'single'    
    
};