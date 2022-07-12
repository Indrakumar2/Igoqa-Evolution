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
            "width":140,  
            "tooltipField":"assignmentCustomerName",        
            "headerTooltip": localConstant.gridHeader.CUSTOMER,
        },
        {
            "headerName": localConstant.gridHeader.SUPPLIER_PO_NAME,
            "field": "assignmentSupplierPurchaseOrderNumber",
            "filter": "agTextColumnFilter",
            "width":140,  
            "tooltipField":"assignmentSupplierPurchaseOrderNumber",        
            "headerTooltip": localConstant.gridHeader.SUPPLIER_PO_NAME,
        },
        {
            "headerName": localConstant.gridHeader.ASSIGNMENT_NO,
            "field": "assignmentNumber",
            "filter": "agNumberColumnFilter",
            "width":170,  
            "tooltipField":"assignmentNumber",        
            "headerTooltip": localConstant.gridHeader.ASSIGNMENT_NO,
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
            "width":170,
            "headerTooltip": localConstant.gridHeader.FIRST_VISIT_DATE,  
            "tooltip": (params) => {
                return moment(params.data.assignmentFirstVisitDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        },            
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "assignmentFirstVisitDate"
            },
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
            "headerTooltip": localConstant.gridHeader.COMPLETE,
            "width":120,  
            "tooltip":(params) => {                               
                if (params.data.isAssignmentCompleted === true) {                        
                    return "Yes";
                } else {
                    return "No";
                }
            },
            "valueGetter": (params) => {                               
                if (params.data.isAssignmentCompleted === true) {                        
                    return "Yes";
                } else {
                    return "No";
                }
            }
        },
        {
            "headerName": localConstant.gridHeader.CONTRACT_COMPANY,
            "field": "assignmentContractHoldingCompany",
            "filter": "agTextColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            }, 
            "headerTooltip": localConstant.gridHeader.CONTRACT_COMPANY,
            "width":200,
            "tooltipField":"assignmentContractHoldingCompany",
        },
        {
            "headerName": localConstant.gridHeader.OPERATING_COMPANY,
            "field": "assignmentOperatingCompany",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.OPERATING_COMPANY,
            "width":200,
            "tooltipField":"assignmentOperatingCompany",
        },
        {
            "headerName": localConstant.gridHeader.TECHNICAL_SPECIALIST,
            "field": "techSpecialists",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.TECHNICAL_SPECIALIST,
            "width":200,
            "tooltipField":"techSpecialists",
        },
        {
            "headerName": localConstant.gridHeader.MATERIAL_DESCRIPTION,
            "field": "materialDescription",    //Changes for D-765
            "filter": "agTextColumnFilter",
            "width":200,  
            "tooltipField":"materialDescription",       //Changes for D-765 
            "headerTooltip": localConstant.gridHeader.MATERIAL_DESCRIPTION,
            
        },
        {
            "headerName": localConstant.gridHeader.PERCENTAGE_COMPLETE,
            "field": "assignmentPercentageCompleted",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.PERCENTAGE_COMPLETE,
            "width":200,
            "tooltipField":"assignmentPercentageCompleted",
        },
        {
            "headerName": localConstant.gridHeader.EXPECTED_COMPLETED_DATE,
            "field": "assignmentExpectedCompleteDate",
            "filter": "agDateColumnFilter",
            "headerTooltip": localConstant.gridHeader.EXPECTED_COMPLETED_DATE,
            "width":250,
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
        {
            "headerName": localConstant.gridHeader.ASSIGNMENT_REFERENCE,
            "field": "assignmentReference",
            "filter": "agTextColumnFilter",
            "width":200,  
            "tooltipField":"assignmentReference",        
            "headerTooltip": localConstant.gridHeader.ASSIGNMENT_REFERENCE,
            
        },       
    ],
    "enableFilter":true, 
    "enableSorting":true, 
    "pagination": true,
    "searchable":true,
    "gridActions":true,
    "gridTitlePanel":true,
    "gridHeight":56,
    "multiSortKey": "ctrl",
    "clearFilter":true, 
    "gridRefresh":true,
    "exportFileName":"Assignments",
    "columnsFiltersToDestroy":[ 'assignmentFirstVisitDate','assignmentExpectedCompleteDate' ]
};