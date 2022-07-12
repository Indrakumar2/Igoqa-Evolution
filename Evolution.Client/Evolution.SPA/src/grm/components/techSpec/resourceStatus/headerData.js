import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData= (functionRefs) => {  // Hot Fixes Changes done as per ITK requirement
    return{
        "StampHeader": {
                "columnDefs": [
                    {
                        "headerCheckboxSelectionFilteredOnly": true,
                        "checkboxSelection": true,
                        "suppressFilter": true,
                        "width": 37
                    },
                    {
                        "headerName": localConstant.techSpec.gridHeaderConstants.STAMP_TYPE,
                        "headerTooltip": localConstant.techSpec.gridHeaderConstants.STAMP_TYPE,
                        "field": "isSoftStamp",
                        "tooltip": (params) => {
                            if (params.data.isSoftStamp === true) {
                                return "Soft Stamp";
                            } else {
                                return "Hard Stamp";
                            }
                        },
                        "filter": "agTextColumnFilter",
                        "width": 200,
                        "valueGetter": (params) => {
                            if (params.data.isSoftStamp === true || params.data.isSoftStamp === "Soft Stamp") {
                                return "Soft Stamp";
                            } else {
                                return "Hard Stamp";
                            }
                        },
                    },
                    {
                        "headerName": localConstant.techSpec.gridHeaderConstants.COUNTRY_CODE,
                        "headerTooltip": localConstant.techSpec.gridHeaderConstants.COUNTRY_CODE,
                        "field": "countryCode",
                        "tooltipField": "countryCode",
                        "filter": "agTextColumnFilter",
                        "width": 200
                    },
                    {
                        "headerName": localConstant.techSpec.gridHeaderConstants.STAMP_NUMBER,
                        "headerTooltip": localConstant.techSpec.gridHeaderConstants.STAMP_NUMBER,
                        "field": "stampNumber",
                        "tooltipField": "stampNumber",
                        "filter": "agNumberColumnFilter",
                        "width": 170,
                        "filterParams": {
                            "inRangeInclusive":true
                        }
                    },
                    {
                        "headerName": localConstant.techSpec.gridHeaderConstants.ISSUED_ON,
                        "headerTooltip": localConstant.techSpec.gridHeaderConstants.ISSUED_ON,
                        "field": "issuedDate",
                        "tooltip": (params) => {
                            return moment(params.data.issuedDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                    }, 
                        "filter": "agDateColumnFilter",
                        "width": 120,
                        "cellRenderer": "DateComponent",
                        "cellRendererParams": {
                            "dataToRender": "issuedDate"
                        },
                        "filterParams": {
                            "comparator": dateUtil.comparator,
                            "inRangeInclusive": true,
                            "browserDatePicker": true
                        },
                        "sort": "desc"
                    },
                    {
                        "headerName": localConstant.techSpec.gridHeaderConstants.RETURNED_ON,
                        "headerTooltip": localConstant.techSpec.gridHeaderConstants.RETURNED_ON,
                        "field": "returnedDate",
                        "tooltip": (params) => {
                            return moment(params.data.returnedDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                    }, 
                        "filter": "agDateColumnFilter",
                        "cellRenderer": "DateComponent",
                        "width": 200,
                        "cellRendererParams": {
                            "dataToRender": "returnedDate"
                        },
                        "filterParams": {
                            "comparator": dateUtil.comparator,
                            "inRangeInclusive": true,
                            "browserDatePicker": true
                        }
                    },
                    {
                        "headerName": localConstant.techSpec.gridHeaderConstants.ACKNOWLEDGEMENT_DOCUMENT,
                        "headerTooltip":localConstant.techSpec.gridHeaderConstants.ACKNOWLEDGEMENT_DOCUMENT,
                        "field": "documentName",
                        "tooltipField": "documentName",
                        "filter": "agTextColumnFilter",
                        "cellRenderer": "FileToBeDownload",
                        "cellRendererParams":  {
                            "dataToRender": "documentName",
                        },   
                        "width": 300
                    },
                    {
                        "headerName": "",
                        "field": "EditColumn",
                        "cellRenderer": "EditLink",
                        "buttonAction": "",
                        "cellRendererParams": {
                            "disableField": (e) => functionRefs.enableEditColumn(e) // Hot Fixes Changes done as per ITK requirement
                        },
                        "suppressFilter": true,
                        "suppressSorting": true,
                        "width": 50
                    }
                ],
                "enableSelectAll":true,
                "enableFilter": true,
                "enableSorting": true,
                "pagination": false,
                "searchable": false,
                "rowHighlight":true,
                "gridTitlePanel": false,
                "gridHeight": 30,
                "clearFilter":true, 
                "rowSelection": 'multiple',
                "columnsFiltersToDestroy":[ 'issuedDate','returnedDate' ]
            }
    };
};