import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData= (functionRefs) =>{//SystemRole based UserType relevant Quick Fixes 
    return{
        "SensitiveDocumentHeader": {
            "columnDefs": [
                {
                    "headerCheckboxSelectionFilteredOnly": true,
                    "checkboxSelection": true,
                    "suppressFilter": true,
                    "width": 40
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.FILE_NAME,//D837 (Ref ALM Doc on 09-04-2020 Issue#3)
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.FILE_NAME,//D837 (Ref ALM Doc on 09-04-2020 Issue#3)
                    "field": "documentName",
                    "tooltipField": "documentName",
                    "filter": "agTextColumnFilter",
                    //"cellRenderer": "FileToBeOpen",
                    "cellRendererParams": {
                        "dataToRender": "documentName"
                    },
                    "width": 200
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.DOCUMENT_TYPE,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.DOCUMENT_TYPE,
                    "field": "documentType",
                    "tooltipField": "documentType",
                    "filter": "agTextColumnFilter",
                    "width": 180
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.DOCUMENT_NAME,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.DOCUMENT_NAME,
                    "field": "documentTitle",
                    "tooltipField": "documentTitle",
                    "filter": "agTextColumnFilter",
                    "width": 180
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.DOCUMENT_ADDED,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.DOCUMENT_ADDED,
                    "field": "createdOn",
                    "tooltip": (params) => {
                        return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                    "width": 170,
                    "filter": "agDateColumnFilter",
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "createdOn"
                    },
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                    }
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.EXPIRY,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.EXPIRY,
                    "field": "expiryDate",
                    "tooltip": (params) => {
                        return moment(params.data.expiryDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                    "width": 150,
                    "filter": "agDateColumnFilter",
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "expiryDate"
                    },
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                    }
                },
            
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.COMMENTS,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.COMMENTS,
                    "field": "comments",
                    "tooltipField": "comments",
                    "filter": "agTextColumnFilter",
                    "width": 170
                },
                {
                    "headerName": "",
                    "field": "EditColumn",
                    "cellRenderer": "EditLink",
                    "buttonAction": "",
                    "cellRendererParams": {
                        "disableField": (e) => functionRefs.enableEditColumn(e)//SystemRole based UserType relevant Quick Fixes 
                    },
                    "suppressFilter": true,
                    "suppressSorting": true,
                    "width": 75
                },
                {
                    "headerName": '',           
                    "field": "documentName",                      
                    "tooltipField": "documentName",           
                    "cellRenderer": "FileToBeOpen",
                    "suppressFilter": true,
                    "suppressSorting": true,
                    "cellRendererParams": {
                        "dataToRender": "documentName"
                    },
                    "width": 50
                }
            ],
            "enableSelectAll":true,
            "enableFilter": true,   
            "enableSorting": true,
            "pagination": false,
            "gridHeight": 50,
            "rowSelection": 'multiple',
            "clearFilter":true, 
            "searchable":true,
            "columnsFiltersToDestroy":[ 'createdOn','expiryDate' ]
        },
    };
};
