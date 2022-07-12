import { getlocalizeData } from '../../../../utils/commonUtils';
import moment from 'moment';
import dateUtil from '../../../../utils/dateUtil';
const localConstant = getlocalizeData();
export const headerData = {
    "columnDefs": [
        {
            "headerName": localConstant.gridHeader.CUSTOMER,
            "field": "assignmentCustomerName",
            "tooltipField": "assignmentCustomerName",
            "headerTooltip": localConstant.gridHeader.CUSTOMER,
            "filter": "agTextColumnFilter",
            "width": 200,
           
        },
        {
            "headerName": localConstant.gridHeader.SUPPLIER_PO,
            "field": "assignmentSupplierPurchaseOrderNumber",
            "tooltipField": "assignmentSupplierPurchaseOrderNumber",
            "headerTooltip": localConstant.gridHeader.SUPPLIER_PO,
            "filter": "agTextColumnFilter",
            "width": 200
        },
        {
            "headerName": localConstant.gridHeader.MATERIAL_DESCRIPTION,
            "field": "materialDescription",
            "tooltipField": "materialDescription",
            "headerTooltip": localConstant.gridHeader.MATERIAL_DESCRIPTION,
            "filter": "agTextColumnFilter",
            "width": 200
        },
        {
            "headerName": localConstant.gridHeader.PROJECT_NO,
            "field": "assignmentProjectNumber",
            "tooltip": (params) => (params.data.assignmentProjectNumber),
            "headerTooltip": localConstant.gridHeader.PROJECT_NO,
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive": true
            },
            "valueGetter": (params) => (params.data.assignmentProjectNumber),
            "width": 205
        },
        {
            "headerName": localConstant.gridHeader.ASSIGNMENT_NO,
            "field": "assignmentNumber",
            //tooltipchanges
            "tooltip": (params) => {
                return params.data.assignmentNumber.toString().padStart(5, '0');
            },
            "headerTooltip": localConstant.gridHeader.ASSIGNMENT_NO,
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive": true
            },
            "cellRenderer": "AssignmentAnchor",
            "cellRendererParams": (params) => {
                return { assignmentNumber: params.data.assignmentNumber.toString().padStart(5, '0'), assignmentId: params.data.assignmentId };
            },
            "width": 200
        },
        {
            "headerName": localConstant.gridHeader.TECHNICAL_SPECIALIST,
            "field": "techSpecialists",
            "tooltip": (params) => {
                const { techSpecialists } = params.data;
                if (techSpecialists) {
                    return techSpecialists.filter(ts => ts.fullName !== ' ').map(e => e.fullName).join(",");
                } else {
                    return "";
                }
            },
            "headerTooltip": localConstant.gridHeader.TECHNICAL_SPECIALIST,
            "filter": "agTextColumnFilter",
            "valueGetter": (params) => {
                const { techSpecialists } = params.data;
                if (techSpecialists) {
                    return techSpecialists.filter(ts => ts.fullName !== ' ').map(e => e.fullName).join(",");
                } else {
                    return "";
                }
            },
            "width": 200
        },
        {
            "headerName": localConstant.gridHeader.FIRST_VISIT_DATE,
            "field": "assignmentFirstVisitDate",
            "tooltip": (params) => {
                let d = '';
                if (params.data.assignmentFirstVisitDate) {
                    d = moment(params.data.assignmentFirstVisitDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }
                return d === "" ? "" : d;
            },
            "headerTooltip": localConstant.gridHeader.FIRST_VISIT_DATE,
            "filter": "agDateColumnFilter",
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "assignmentFirstVisitDate"
            },
            "width": 200,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
            }
        },
        {
            "headerName": localConstant.gridHeader.COMPLETE,
            "field": "isAssignmentCompleted",
            "tooltip": (params) => {
                if (params.data.isAssignmentCompleted) {
                    return "Yes";
                } else {
                    return "No";
                }
            },
            "headerTooltip": localConstant.gridHeader.COMPLETE,
            "filter": "agTextColumnFilter",
            "valueGetter": (params) => {
                if (params.data.isAssignmentCompleted) {
                    return "Yes";
                } else {
                    return "No";
                }
            },
            "width": 200
        },
        {
            "headerName": localConstant.gridHeader.CONTRACT_COMPANY,
            "field": "assignmentContractHoldingCompany",
            "tooltipField": "assignmentContractHoldingCompany",
            "headerTooltip": localConstant.gridHeader.CONTRACT_COMPANY,
            "filter": "agTextColumnFilter",
            "width": 200
        },
        {
            "headerName": localConstant.gridHeader.OPERATING_COMPANY,
            "field": "assignmentOperatingCompany",
            "tooltipField": "assignmentOperatingCompany",
            "headerTooltip": localConstant.gridHeader.OPERATING_COMPANY,
            "filter": "agTextColumnFilter",
            "width": 200
        },
        {
            "headerName": localConstant.gridHeader.PERCENTAGE_COMPLETE,
            "field": "assignmentPercentageCompleted",
            "tooltipField": "assignmentPercentageCompleted",
            "headerTooltip": localConstant.gridHeader.PERCENTAGE_COMPLETE,
            "filter": "agNumberColumnFilter",
            "width": 200
        },
        {
            "headerName": localConstant.gridHeader.EXPECTED_COMPLETED_DATE,
            "field": "assignmentExpectedCompleteDate",
            //tooltipchanges
            // "tooltip": (params) => {
            //     return moment(params.data.assignmentExpectedCompleteDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
            // },
            "tooltip": (params) => {
                let d = '';
                if (params.data.assignmentExpectedCompleteDate) {
                    d = moment(params.data.assignmentExpectedCompleteDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }
                return d === "" ? "" : d;
            },
            "headerTooltip": localConstant.gridHeader.EXPECTED_COMPLETED_DATE,
            "filter": "agDateColumnFilter",
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "assignmentExpectedCompleteDate"
            },
            "width": 200,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
            }
        },
        {
            "headerName": localConstant.gridHeader.ASSIGNMENT_REF,
            "field": "assignmentReference",
            "tooltipField": "assignmentReference",
            "headerTooltip": localConstant.gridHeader.ASSIGNMENT_REF,
            "filter": "agTextColumnFilter",
            "width": 200
        },
        {
            "headerName": localConstant.gridHeader.INTERNAL_ASSIGNMENT,
            "field": "internalAssigment",
            "filter": "agTextColumnFilter",
            "valueGetter": (params) => {
                if(params.data.isInternalAssignment === true){
                    return "Yes";
                }else if(params.data.isInternalAssignment === false){
                  return "No";              }
              },
              "headerTooltip": localConstant.gridHeader.INTERNAL_ASSIGNMENT,
              "width":200,
              "tooltip":(params) => {
                if(params.data.isInternalAssignment === true){
                    return "Yes";
                }else if(params.data.isInternalAssignment === false){
                  return "No";
                }    
              },
        },
    ],
    "rowSelection": 'single',
    "pagination": true,
    "enableFilter": true,
    "enableSorting": true,
    "gridHeight": 59,
    "searchable": true,
    "gridActions": true,
    "gridTitlePanel": true,
    "clearFilter": true,
    "exportFileName": "Assignment Search Data",
    "columnsFiltersToDestroy": [ 'assignmentFirstVisitDate', 'assignmentExpectedCompleteDate' ]

};