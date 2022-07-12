import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();

export const HeaderData = {
    "columnDefs": [
        {
            "headerName": localConstant.gridHeader.CUSTOMER,
            "field": "assignmentCustomerName",
            "filter": "agTextColumnFilter",
            "headerTooltip":localConstant.gridHeader.CUSTOMER,
            "tooltipField":"assignmentCustomerName",
            "width": 150
        },
        {
            "headerName": localConstant.gridHeader.PROJECT_NO,
            "field": "assignmentProjectNumber",
            "filter": "agTextColumnFilter",
            "width": 130,
            "headerTooltip":localConstant.gridHeader.CUSTOMER,
            "tooltipField":"assignmentProjectNumber",
            "cellRenderer": "projectAnchorRenderer",
        },
        {
            "headerName": localConstant.gridHeader.ASSIGNMENT_NO,
            "field": "assignmentNumber",
            "filter": "agTextColumnFilter",
            "width": 160,
            "headerTooltip": localConstant.gridHeader.ASSIGNMENT_NO,
            "tooltip":(params) => {                
                return params.data.assignmentNumber.toString().padStart(5, '0');
            }, 
            "cellRenderer": "AssignmentAnchor",
            "cellRendererParams": (params) => {                
                return { assignmentNumber: params.data.assignmentNumber.toString().padStart(5, '0'), assignmentId: params.data.assignmentId };
            }, 
            "filterParams": {
                "inRangeInclusive":true
            }
        },
        {
            "headerName": localConstant.gridHeader.FIRST_VISIT_DATE,
            "field": "assignmentFirstVisitDate",
            "filter": "agDateColumnFilter",           
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "assignmentFirstVisitDate"
            },
            "headerTooltip": localConstant.gridHeader.FIRST_VISIT_DATE,
            "tooltip": (params) => {
                return moment(params.data.assignmentFirstVisitDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "width": 150,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
            
        },
        {
            "headerName": localConstant.gridHeader.COMPLETE,
            "field": "isAssignmentCompleted",
            "filter": "agTextColumnFilter",
            "width": 120,
            "valueGetter": (params) => {
                if (params.data.isAssignmentCompleted === true) {
                    return "Yes";
                } else if (params.data.isAssignmentCompleted === false) {
                    return "No";
                }
            },
            "headerTooltip": localConstant.gridHeader.COMPLETE,
            "tooltip": (params) => {
                if (params.data.isAssignmentCompleted === true) {
                    return "Yes";
                } else if (params.data.isAssignmentCompleted === false) {
                    return "No";
                }
            },
        },
        {
            "headerName": localConstant.gridHeader.CONTRACT_COMPANY,
            "field": "assignmentContractHoldingCompany",
            "filter": "agTextColumnFilter",
            "width": 180,
            "headerTooltip": localConstant.gridHeader.CONTRACT_COMPANY,
            "tooltipField": "assignmentContractHoldingCompany"
        },
        {
            "headerName": localConstant.gridHeader.DESCRIPTION,
            "field": "materialDescription",
            "filter": "agTextColumnFilter",
            "width": 150,
            "headerTooltip": localConstant.gridHeader.DESCRIPTION,
            "tooltipField": "materialDescription"
        },
        {
            "headerName": localConstant.gridHeader.PERCENTAGE_COMPLETE,
            "field": "assignmentPercentageCompleted",
            "filter": "agTextColumnFilter",
            "width": 190,
            "headerTooltip": localConstant.gridHeader.PERCENTAGE_COMPLETE,
            "tooltipField": "assignmentPercentageCompleted"
        },
        {
            "headerName": localConstant.gridHeader.EXPECTED_COMPLETED_DATE,
            "field": "assignmentExpectedCompleteDate",
            "filter": "agDateColumnFilter",           
            "width": 220,
            "headerTooltip": localConstant.gridHeader.EXPECTED_COMPLETED_DATE, 
            "tooltip": (params) => {
                return moment(params.data.assignmentExpectedCompleteDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        },    
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "assignmentExpectedCompleteDate"
            },
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },
    ],
    "enableFilter": true,
    "enableSorting": true,
    "gridHeight": 45,
    "pagination": true,
    "clearFilter":true, 
    "gridRefresh":true,
    "searchable": true,
    "columnsFiltersToDestroy":[ 'assignmentFirstVisitDate','assignmentExpectedCompleteDate' ]
};