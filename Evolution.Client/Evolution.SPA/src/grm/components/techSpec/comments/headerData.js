import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData = {
    "BusinessInformationHeader":{
        "columnDefs": [
            {
                "headerName":localConstant.gridHeader.DATE ,
                "headerTooltip": localConstant.gridHeader.DATE,
                "field": "createdDate",
                "tooltip": (params) => {
                    return moment(params.data.createdDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                    
                }, 
                "filter": "agDateColumnFilter",
                "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "dataToRender": "createdDate"
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
                "field": "createdBy",
                "tooltipField": "createdBy",
                "filter": "agTextColumnFilter",
                "width": 120
            },
            {
                "headerName": localConstant.techSpec.gridHeaderConstants.COMMENTS,
                "headerTooltip": localConstant.techSpec.gridHeaderConstants.COMMENTS,
                "field": "note",
                "tooltipField": "note",
                "filter": "agTextColumnFilter",
                "width": 700
            },
        
             {
                "headerName": "",
                "field": "ViewTSComments",
                "cellRenderer": "EditLink",
                "buttonAction":"",
                "cellRendererParams": {
                    "displayLink":localConstant.commonConstants.VIEW //D661 issue8
                },
                "suppressFilter": true,
                "suppressSorting": true,
                "width": 75
            }
                 
        ],
        "enableFilter": true,
        "enableSorting": true,
        "pagination": false,
        "clearFilter":true, 
        "gridHeight": 30,
        "columnsFiltersToDestroy":[ 'createdDate' ]
    },
    "ResourceNoteHeader":{
        "columnDefs": [
            {
                "headerName":localConstant.gridHeader.DATE ,
                "headerTooltip": localConstant.gridHeader.DATE,
                "field": "createdDate",
                "tooltip": (params) => {
                    return moment(params.data.createdDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
            }, 
                "filter": "agDateColumnFilter",
                "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "dataToRender": "createdDate"
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
                "field": "createdBy",
                "tooltipField": "createdBy",
                "filter": "agTextColumnFilter",
                "width": 120
            },
            {
                "headerName": localConstant.techSpec.gridHeaderConstants.COMMENTS,
                "headerTooltip": localConstant.techSpec.gridHeaderConstants.COMMENTS,
                "field": "note",
                "tooltipField": "note",
                "filter": "agTextColumnFilter",
                "width": 700
            },
        
             {
                "headerName": "",
                "field": "ViewTSComments",
                "cellRenderer": "EditLink",
                "buttonAction":"",
                "cellRendererParams": {
                    "displayLink":localConstant.commonConstants.VIEW //D661 issue8
                },
                "suppressFilter": true,
                "suppressSorting": true,
                "width": 75
            }
                 
        ],
        "enableFilter": true,
        "enableSorting": true,
        "pagination": false,
        "clearFilter":true, 
        "gridHeight": 30,
        "columnsFiltersToDestroy":[ 'createdDate' ]
    }
};
