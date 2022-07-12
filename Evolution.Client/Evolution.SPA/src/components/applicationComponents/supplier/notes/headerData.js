import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData = {
    "columnDefs": [
        {
            "headerName":localConstant.gridHeader.DATE ,
            "headerTooltip": localConstant.gridHeader.DATE,
            "field": "createdOn",
            "filter": "agDateColumnFilter",
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "createdOn"
            },
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              },
            "width": 120,
            "sort": "desc",
            "tooltip": (params) => {
                return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
        },
        {
            "headerName": localConstant.gridHeader.USER,
            "headerTooltip": localConstant.gridHeader.USER,
            "field": "userDisplayName",
            "tooltipField": "userDisplayName",
            "filter": "agTextColumnFilter",
            "width": 120,
        },
        {
            "headerName": localConstant.gridHeader.NOTE,
            "headerTooltip": localConstant.gridHeader.NOTE,
            "field": "notes",
            "filter": "agTextColumnFilter",
            "width":700,
            "tooltipField":"notes",
        },
        {
            "headerName": "",
            "field": "ViewSupplierNotes",
            "cellRenderer": "EditLink",
            "cellRendererParams": {
                "displayLink":localConstant.commonConstants.VIEW //D661 issue8
            },
            "suppressFilter": true,
            "suppressSorting": true,
            "width": 75
        }
    ],
    "enableSorting": true,
    "pagination": true,
    "searchable":true,
    "gridHeight": 55,
    "enableFilter": true,
    "clearFilter":true, 
    "exportFileName":"Notes",
    "columnsFiltersToDestroy":[ 'createdOn' ]
};
