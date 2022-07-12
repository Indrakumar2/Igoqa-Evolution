import { getlocalizeData, isEmpty } from '../../../utils/commonUtils';
const localConstant = getlocalizeData();
export const HeaderData = {
    "AuditSearchHeader": {
        "columnDefs": [

            {
                "headerName": localConstant.gridHeader.auditSearch.PARENT_TABLE,
                "headerTooltip": localConstant.gridHeader.auditSearch.PARENT_TABLE,
                "field": "auditModuleName",
                "cellRenderer": "medalCellRenderer",
                "filter": "agTextColumnFilter",
                "width": 250
            },
            {
                "headerName": localConstant.gridHeader.auditSearch.PARENT_ID,
                "headerTooltip": localConstant.gridHeader.auditSearch.PARENT_ID,
                "field": "searchReference",
                "filter": "agTextColumnFilter",
                "valueGetter": (params => {
                    if (params.data.searchReference.includes('$')) {
                        const parentId = params.data.searchReference.split('$')[0];
                        if (parentId.split(':').length > 0)
                            return parentId.split(':')[1].replace('}', '');
                        else
                            return params.data.searchReference.split('$')[0];
                    }
                }),
                "width": 250
            },
            {
                "headerName": localConstant.gridHeader.auditSearch.LAST_OPERATION,
                "headerTooltip": localConstant.gridHeader.auditSearch.LAST_OPERATION,
                "field": "actionType",
                "filter": "agTextColumnFilter",
                "valueGetter": (params => {
                    if (params.data.actionType === "I") {
                        return "Insert";
                    } else if (params.data.actionType === "M") {

                        return "Modify";
                    }
                    else if (params.data.actionType === "D") {
                        return "Delete";
                    }
                }),
                "width": 245
            },
            {
                "headerName": localConstant.gridHeader.auditSearch.LAST_CHANGED_BY,
                "headerTooltip": localConstant.gridHeader.auditSearch.LAST_CHANGED_BY,
                "field": "actionBy",
                "filter": "agTextColumnFilter",
                "width": 250
            },
            {
                "headerName": localConstant.gridHeader.auditSearch.LAST_CHANGED_DATE,
                "headerTooltip": localConstant.gridHeader.auditSearch.LAST_CHANGED_DATE,
                "field": "actionOn",
                "filter": "agDateColumnFilter",
                "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "type": "date-Time",
                    "dataToRender": "actionOn"
                },
                "width": 265
            },
        ],
        "pagination": false,
        "searchable": false,
        "suppressRowClickSelection": true,
        "gridTitlePanel": false,
        "auditgridHeight": 30,
        "enableSorting": true,
        "clearFilter": true,
        "enableFilter": true,
        "rowSelection": 'single',
    },
    "AuditSearchSubHeader": {
        "columnDefs": [

            {
                "headerName": localConstant.gridHeader.auditSearch.TABLE,
                "headerTooltip": localConstant.gridHeader.auditSearch.TABLE,
                "field": "auditSubModuleName",
                "cellRenderer": "medalCellRenderer",
                "filter": "agTextColumnFilter",
                "width": 300
            },
            {
                "headerName": localConstant.gridHeader.auditSearch.OPERATION,
                "headerTooltip": localConstant.gridHeader.auditSearch.OPERATION,
                "field": "actionType",
                "cellRenderer": "medalCellRenderer",
                "valueGetter": (params => {
                    if (params.data.actionType === "I") {
                        return "Insert";
                    } else if (params.data.actionType === "M" && !isEmpty(params.data.newValue) && !isEmpty(params.data.oldValue)) {
                        return "Modify ";

                    }
                    else if (params.data.actionType === "M" && isEmpty(params.data.newValue)) {
                        return "Delete";
                    }
                    else if (params.data.actionType === "M" && isEmpty(params.data.oldValue)) {
                        return "Insert";
                    }
                    else if (params.data.actionType === "D") {
                        return "Delete";
                    }
                }),
                "filter": "agTextColumnFilter",
                "width": 250
            },
            {
                "headerName": localConstant.gridHeader.auditSearch.USER,
                "headerTooltip": localConstant.gridHeader.auditSearch.USER,
                "field": "actionBy",
                "cellRenderer": "medalCellRenderer",

                "filter": "agTextColumnFilter",
                "width": 300
            },
            {
                "headerName": localConstant.gridHeader.auditSearch.DATE_TIME,
                "headerTooltip": localConstant.gridHeader.auditSearch.DATE_TIME,
                "field": "actionOn",

                "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "type": "date-Time",
                    "dataToRender": "actionOn"
                },

                "filter": "agDateColumnFilter",
                "width": 300
            },

            {
                "headerName": "",
                "field": "EditColumn",
                "cellRenderer": "EditLink",
                "buttonAction": "",
                "cellRendererParams": {
                    "displayLink": "View Changes"
                },
                "suppressFilter": true,
                "suppressSorting": true,
                "width": 100
            }
        ],
        "pagination": false,
        "searchable": false,
        "gridActions": true,
        "gridTitlePanel": false,
        "enableFilter": true,
        "auditgridHeight": 30,
        "enableSorting": true,
        "rowSelection": 'multiple',
        "clearFilter": true,
        "auditRowHighlight": true,
    },

};
