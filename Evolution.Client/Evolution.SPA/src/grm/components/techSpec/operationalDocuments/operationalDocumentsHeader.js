import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();

export const HeaderData = {
    "columnDefs": [
       
        {
            "headerName": localConstant.gridHeader.NAME,
            "headerTooltip": localConstant.gridHeader.NAME,
            "field": "documentName",
            "tooltipField": "documentName",
            "cellRenderer": "documentName",           
            "filterParams": {
                "inRangeInclusive":true
            },
            "width":400,
        },
        
        {
            "headerName": localConstant.gridHeader.DOCUMENT_TYPE,
            "headerTooltip": localConstant.gridHeader.DOCUMENT_TYPE,
            "field": "documentType",   
            "tooltipField": "documentType",        
            "width":300,
        },
        
        {
            "headerName": localConstant.gridHeader.SIZE_KB,
            "headerTooltip": localConstant.gridHeader.SIZE_KB,
            "field": "sizeKb",
            "tooltipField": "sizeKb",
            "filter": "agTextColumnFilter",
            "width":300,
        },
        {
            "headerName": localConstant.gridHeader.CREATION_DATE,
            "headerTooltip": localConstant.gridHeader.CREATION_DATE,
            "field": "createdDate",
            "tooltip": (params) => {
                return moment(params.data.createdDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
        "filter": "agDateColumnFilter",           
            "width":300,
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "createdDate"
            },
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true,
            }
        },
       
    ],
    "rowSelection": 'single',   
    "pagination": false,
    "enableFilter": true,
    "enableSorting": true,
    "gridHeight": 60,
    "searchable": true,
    "gridActions": true,
    "clearFilter":true, 
    "gridTitlePanel": true,
    "columnsFiltersToDestroy":[ 'createdDate' ]
   
};