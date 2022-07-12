import { getlocalizeData , numberFormat } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();

export const HeaderData  = (functionRefs)=>  {
    return {
        "Revenue": {
            "columnDefs": [
                {
                    "checkboxSelection": (param) => {
                        if(param.node.rowIndex === 0)
                            return false;
                        else
                            return true;
                    },
                    "headerCheckboxSelectionFilteredOnly": true,
                    "suppressFilter": true,
                    "width": 40
                },
                {
                    "headerName": localConstant.assignments.DESCRIPTION,
                    "field": "description",
                    "filter": "agTextColumnFilter",
                    "headerTooltip": localConstant.assignments.DESCRIPTION,
                    "tooltipField": "description",
                    "width": 250
                },
                {
                    "headerName": localConstant.assignments.VALUE_USD,
                    "field": "value",
                    "filter": "agTextColumnFilter",
                    "headerTooltip": localConstant.assignments.VALUE_USD,
                    "tooltip": (params) => {
                        if (params.data.value === null || params.data.value === undefined)
                            return '';
                        const val = parseFloat(params.data.value).toFixed(2);
                        return (val >= 0) ? val : `(${ Math.abs(val).toFixed(2) })`;
                    },
                    "valueGetter": (params) => {
                        if (params.data.value === null || params.data.value === undefined || params.data.value === '')
                            return '';
                        const val =  parseFloat(numberFormat(params.data.value.toString())).toFixed(2);
                        return (val >= 0) ? val : `(${ Math.abs(val).toFixed(2) })`;
                    },
                    "width": 130
                },
                {
                    "headerName": "",
                    "field": "EditRevenue",
                    "cellRenderer": "EditLink",                
                    "cellRendererParams": {
                        "disableField": (e) => functionRefs.enableEditColumn(e)
                    },
                    "suppressFilter": true,
                    "suppressSorting": true,
                    "width": 60
                },       
            ],
            "rowSelection": "multiple",
            "gridHeight": 20,
            "enableSorting": true,
            "clearFilter":true, 
            "pagination": false,
        },

        "Costs": {
            "columnDefs": [
                {
                    "checkboxSelection": true,
                    "headerCheckboxSelectionFilteredOnly": true,
                    "suppressFilter": true,
                    "width": 40
                },
                {
                    "headerName": localConstant.assignments.DESCRIPTION,
                    "field": "description",
                    "filter": "agTextColumnFilter",
                    "headerTooltip": localConstant.assignments.DESCRIPTION,
                    "tooltipField": "description",
                    "width": 250
                },
                {
                    "headerName": localConstant.assignments.VALUE_USD,
                    "field": "value",
                    "filter": "agTextColumnFilter",
                    "headerTooltip": localConstant.assignments.VALUE_USD,
                    "tooltip": (params) => {
                        if (params.data.value === null || params.data.value === undefined)
                            return '';
                        const val = parseFloat(params.data.value).toFixed(2);
                        return (val >= 0) ? val : `(${ Math.abs(val).toFixed(2) })`;
                    },
                    "valueGetter": (params) => {
                        if (params.data.value === null || params.data.value === undefined)
                            return '';
                        const val = parseFloat(params.data.value).toFixed(2);
                        return (val >= 0) ? val : `(${ Math.abs(val).toFixed(2) })`;
                    },
                    "width": 130
                },
                {
                    "headerName": "",
                    "field": "EditCosts",
                    "cellRenderer": "EditLink",                
                    "cellRendererParams": {
                        "disableField": (e) => functionRefs.enableEditColumn(e)
                    },
                    "suppressFilter": true,
                    "suppressSorting": true,
                    "width": 60
                },       
            ],
            "rowSelection": "multiple",
            "gridHeight": 20,
            "enableSorting": true,
            "clearFilter":true, 
            "pagination": false,
        }
    };
};