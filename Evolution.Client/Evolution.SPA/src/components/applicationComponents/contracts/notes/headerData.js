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
            "tooltip": (params) => {
                return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
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
            "sort": "desc"
        },
        {
            "headerName": localConstant.gridHeader.USER,
            "headerTooltip": localConstant.gridHeader.USER,
            "field": "userDisplayName",
            "tooltipField": "userDisplayName",
            "filter": "agTextColumnFilter",
            "width": 120
        },
        {
            "headerName": localConstant.gridHeader.NOTE,
            "headerTooltip": localConstant.gridHeader.NOTE,
            "field": "notes",
            "tooltipField": "notes",
            "filter": "agTextColumnFilter",
            "width":700
        },
        {
            "headerName": "",
            "field": "ViewContractNotes",
            "tooltipField": "ViewContractNotes",
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
