import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData= (functionRefs) => {//SystemRole based UserType relevant Quick Fixes 
    return{
        "TaxonomyApprovalHeader": {
            "columnDefs": [
                {
                    "checkboxSelection": true,
                    "headerCheckboxSelectionFilteredOnly": true,
                    "suppressFilter": true,
                    "width": 40
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.CATEGORY,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.CATEGORY,
                    "field": "taxonomyCategoryName",
                    "tooltipField": "taxonomyCategoryName",
                    "filter": "agTextColumnFilter",
                    "width": 150

                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.SUB_CATEGORY,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.SUB_CATEGORY,
                    "field": "taxonomySubCategoryName",
                    "tooltipField": "taxonomySubCategoryName",
                    "width": 150,
                    "filter": "agTextColumnFilter",
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.SERVICES,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.SERVICES,
                    "field": "taxonomyServices",
                    "tooltipField": "taxonomyServices",
                    "width": 120,
                    "filter": "agTextColumnFilter",
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.APPROVAL_STATUS,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.APPROVAL_STATUS,
                    "field": "approvalStatus",
                    "tooltipField": "approvalStatus",
                    "filter": "agTextColumnFilter",
                    "width": 180

                }, //Added for D737 
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.APPROVED_BY,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.APPROVED_BY,
                    "field": "approvedBy",
                    "tooltipField": "approvedBy",
                    "filter": "agTextColumnFilter",
                    "width": 180
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.INTERVIEW,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.INTERVIEW,
                    "field": "interview",
                    "tooltipField": "interview",
                    "filter": "agTextColumnFilter",
                    "width": 180

                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.EFFECTIVE_FROM,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.EFFECTIVE_FROM,
                    "field": "fromDate",
                    "tooltip": (params) => {
                        return moment(params.data.fromDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                    "filter": "agDateColumnFilter",
                    "width": 180,
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "fromDate"
                    },
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                    }
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.EFFECTIVE_TO,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.EFFECTIVE_TO,
                    "field": "toDate",
                    "tooltip": (params) => {
                        return moment(params.data.toDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                    "filter": "agDateColumnFilter",
                    "width": 180,
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "toDate"
                    },
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                    }
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.COMMENTS,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.COMMENTS,
                    "field": "comments",
                    "tooltipField": "comments",
                    "filter": "agDateColumnFilter",
                    "width": 150
                },
                {
                    "headerName": "",
                    "field": "EditColumn",
                    "cellRenderer": "EditLink",
                    "cellRendererParams": {
                        "disableField": (e) => functionRefs.enableEditColumn(e)//SystemRole based UserType relevant Quick Fixes 
                    },
                    "suppressFilter": true,
                    "suppressSorting": true,
                    "width": 50
                }
            ],
            "enableFilter": true,
            "enableSorting": true,
            "pagination": true,
            "searchable": true,
            "gridActions": true,
            "gridHeight": 30,
            "rowSelection": "multiple",
            "clearFilter":true, 
            "exportFileName":"Taxonomy Approval Details",
            "columnsFiltersToDestroy":[ 'fromDate','toDate' ]
        },
    };
};