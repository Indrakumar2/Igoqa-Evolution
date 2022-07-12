import dateUtil from '../../../../utils/dateUtil';
import { isEmpty,getlocalizeData } from '../../../../utils/commonUtils';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData = {
    "columnDefs": [
        {
            "headerName": "File Name",
            "headerTooltip": "File Name",
            "field": "documentName",
            "tooltipField": "documentName",
            "cellRenderer": "FileToBeOpen",
            "cellRendererParams": {
                "dataToRender": "documentName",
                "documentName":true
            },
           "filter": "agTextColumnFilter",
            // "filterParams": {
            //     "inRangeInclusive":true
            // },
            "width":200
        },
        {
            "headerName": "Size",
            "headerTooltip": "Size",
            "field": "documentSize",
            "tooltipField": "documentSize",
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "width":150
        },
        {
            "headerName": "Document Type",
            "headerTooltip": "Document Type",
            "tooltipField": "documentType",
            "field": "documentType",
            "filter": "agTextColumnFilter",
            "width":200
        },
        {
            "headerName": "Uploaded Date",
            "headerTooltip": "Uploaded Date",
            "field": "createdOn",
            "tooltip": (params) => {
                return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filter": "agDateColumnFilter",           
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "createdOn"
            },
            "width":200,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },
        {
            "headerName": "Uploaded By",
            "headerTooltip": "Uploaded By",
            "tooltipField": "createdBy",
            "field": "createdBy",
            "filter": "agTextColumnFilter",
            "width":200
        },
        {
            "headerName": "Module",
            "headerTooltip": "Module",
            "tooltipField": "moduleName",
            "field": "moduleName",
            "filter": "agTextColumnFilter",
            // "filterParams": {
            //     "inRangeInclusive":true
            // },
            "width":130
        },
        {
            "headerName": "Entity",
            "headerTooltip": "Entity",
            "field": "moduleRefCode",
            "tooltip": (params) => {
                if(params.data.moduleRefCode === "null"){
                    return "";
                }else{
                    return params.data.moduleRefCode ;
                }
            },
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "valueGetter": (params) => {
                if(params.data.moduleRefCode === "null"){
                    return "";
                }else{
                    return params.data.moduleRefCode ;
                }
            },
            "width":100
        },        
        {
            "headerName": "Approve",
            "field": "approve",
            "cellRenderer": "EditLink",
            // "cellRendererParams": {
            //     "action": "SelectedDocumentToApprove",
            //     "popupId": "approveDocument",
            //     "label":"Approve"
            // },
            "cellRendererParams": {
                "displayLink":"Approve"
            },
            "suppressFilter": true,
            "suppressSorting": true,
            "width": 80
        },        
        {
            "headerName": "Reject",
            "field": "reject",
            
            "cellRenderer": "EditLink",
            "cellRendererParams": {
                "displayLink":"Reject"
            },
            // "cellRendererParams": {
            //     "action": "RejectDocument",
            //     "popupId": "RejectDocument",
            //     "label":"Reject"
            // },
            "suppressFilter": true,
            "suppressSorting": true,
            "width": 80
        }
    ],
    "defaultColDef": {
        "width": 80
      },
    "enableFilter":true, 
    "enableSorting":true, 
    "pagination": true,
    "searchable":true,
     "gridActions":true,
    "gridTitlePanel":true,
    "clearFilter":true, 
    "gridHeight":50,
    "exportFileName":"Document Approval",
    "columnsFiltersToDestroy":[ 'createdOn' ]
};