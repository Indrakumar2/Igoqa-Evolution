export const headerData = {
    "columnDefs": [
        // {
        //     "checkboxSelection": true,
        //     "headerCheckboxSelectionFilteredOnly": true,
        //     "suppressFilter": true,
        //     "width": 37
        // },
        {
            "headerName": "Documentation",
            "headerTooltip": "Documentation",
            "field": "name",
            "tooltipField": "name",
            "cellRenderer": "DocumentationLink",
            "cellRendererParams":  {
                    "dataToRender": "name",
                },
                  "filter": "agTextColumnFilter",
            
            "width": 1250
        },
       
    ],

    "enableFilter":true, 
    "enableSorting":true, 
    "pagination": true,
    "searchable":true,
    "clearFilter":true, 
   // "gridActions":true,
    "gridTitlePanel":true,
    "gridHeight":59,
};
