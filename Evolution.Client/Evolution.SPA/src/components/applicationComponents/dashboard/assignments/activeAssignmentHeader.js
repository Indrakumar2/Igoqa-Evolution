import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData = {
    "columnDefs": [
        {
            "headerName": localConstant.gridHeader.CONTRACT_NO,
            "field": "assignmentContractNumber",
            
            "filter": "agTextColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "width":150,  
            "tooltipField":"assignmentContractNumber",        
            "cellRenderer": "contractAnchorRenderer",
            "headerTooltip": localConstant.gridHeader.CONTRACT_NO,
            // "cellRendererParams": {
            //     "dataToRender": "assignmentContractNumber"
            // }
            
        },
        {
            "headerName": localConstant.gridHeader.PROJECT_NO,
            "field": "assignmentProjectNumber",
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "width":150,           
            "cellRenderer": "ProjectAnchor",
            "valueGetter": (params) => (params.data.assignmentProjectNumber),
            "headerTooltip": localConstant.gridHeader.PROJECT_NO,  
            "tooltipField":"assignmentProjectNumber",
        },
        {
            "headerName": localConstant.gridHeader.CUSTOMER_PROJECT_NAME,
            "field": "assignmentProjectName",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.CUSTOMER_PROJECT_NAME,
            "width":230,  
            "tooltipField":"assignmentProjectName",
            // "valueGetter": function aPlusBValueGetter(params) {
            //     return params.data.projectNumber +'and'+ params.data.projectName;
            //   }
        },
        {
            "headerName": localConstant.gridHeader.ASSIGNMENT_NO,
            "field": "assignmentNumber",
            "filter": "agNumberColumnFilter", 
            "filterParams": {
                "inRangeInclusive":true
            },          
            "cellRenderer": "AssignmentAnchor",
            // "cellRenderer": "HyperLink",
            "cellRendererParams": (params) => {                
                return { assignmentNumber: params.data.assignmentNumber.toString().padStart(5, '0'),assignmentId: params.data.assignmentId };
            },    
            "headerTooltip": localConstant.gridHeader.ASSIGNMENT_NO,
            "width":180,
            "tooltip": (params) => {                
                return params.data.assignmentNumber.toString().padStart(5, '0') ;
            },   
        },
        {
            "headerName": localConstant.gridHeader.ASSIGNMENT_REFERENCE,
            "field": "assignmentReference",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.ASSIGNMENT_REFERENCE,
            "width":210,
            "tooltipField":"assignmentReference",
        },
        {
            "headerName": localConstant.gridHeader.CUSTOMER,
            "field": "assignmentCustomerName",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.CUSTOMER,
            "width":150,
            "tooltipField":"assignmentCustomerName",
        },
        {
            "headerName": localConstant.gridHeader.CONTRACT_COORDINATOR,
            "field": "assignmentContractHoldingCompanyCoordinator",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.CONTRACT_COORDINATOR,
            "width":200,
            "tooltipField":"assignmentContractHoldingCompanyCoordinator",
        },
        {
            "headerName": localConstant.gridHeader.OPERATING_COORDINATOR,
            "field": "assignmentOperatingCompanyCoordinator",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.OPERATING_COORDINATOR,
            "width":200,
            "tooltipField":"assignmentOperatingCompanyCoordinator",
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
            "headerName": localConstant.gridHeader.SUPPLIER_PO_NUMBER,
            "field": "assignmentSupplierPurchaseOrderNumber",
            "filter": "agTextColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "headerTooltip": localConstant.gridHeader.SUPPLIER_PO_NUMBER,
            "width":220,
            "tooltipField":"assignmentSupplierPurchaseOrderNumber",
        },
        {
            "headerName":localConstant.gridHeader.MATERIAL_DESCRIPTION,
            "field": "assignmentSupplierPoMaterial",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.MATERIAL_DESCRIPTION,
            "width":220,
            "tooltipField":"assignmentSupplierPoMaterial",
        },
        {
            "headerName": localConstant.gridHeader.MAIN_SUPPLIER,
            "field": "assignmentSupplierName",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.MAIN_SUPPLIER,
            "width":160,
            "tooltipField":"assignmentSupplierName",
        },
        {
            "headerName": localConstant.gridHeader.ASSIGNMENT_CREATE_DATE,
            "field": "assignmentCreatedDate",
            "filter": "agDateColumnFilter",           
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "assignmentCreatedDate"
            },
            "headerTooltip": localConstant.gridHeader.ASSIGNMENT_CREATE_DATE,
            "width":220,
            "tooltip": (params) => {
                return moment(params.data.assignmentCreatedDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filterParams": {
                "comparator": dateUtil.comparator,                
                "inRangeInclusive": true,
                "browserDatePicker": true,                
              }
        },
        {
            "headerName": localConstant.gridHeader.CONTRACT_HOLDER_COMPANY,
            "field": "assignmentContractHoldingCompany",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.CONTRACT_HOLDER_COMPANY,
            "width":230,
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
            "headerName": localConstant.gridHeader.PERCENTAGE_COMPLETE,
            "field": "assignmentPercentageCompleted",
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "headerTooltip": localConstant.gridHeader.PERCENTAGE_COMPLETE,
            "width":200,
            "tooltipField":"assignmentPercentageCompleted",
        },
        {
            "headerName": localConstant.gridHeader.EXPECTED_COMPLETED_DATE,
            "field": "assignmentExpectedCompleteDate",
            "filter": "agDateColumnFilter",          
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "assignmentExpectedCompleteDate"
            },
            "headerTooltip": localConstant.gridHeader.EXPECTED_COMPLETED_DATE,
            "width":230,
            "tooltip": (params) => {
                return moment(params.data.assignmentExpectedCompleteDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
         },
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },
        {
            "headerName": localConstant.gridHeader.ASSIGNMENT_STATUS,
            "field": "assignmentStatus",
            "filter": "agTextColumnFilter",
            "valueGetter": (params) => {
                if(params.data.assignmentStatus === "P"){
                    return "In Progress";
                }else if(params.data.assignmentStatus === "C"){
                  return "Complete";              }
                
              },
              "headerTooltip": localConstant.gridHeader.ASSIGNMENT_STATUS,
              "width":200,
              "tooltip":(params) => {
                if(params.data.assignmentStatus === "P"){
                    return "In Progress";
                }else if(params.data.assignmentStatus === "C"){
                  return "Complete";              }
                
              },
        }
    ],
    "enableFilter":true,
    // "clearFilter":false, 
    "enableSorting":true, 
    "pagination": true,
    "searchable":true,
    "gridActions":true,
    "gridTitlePanel":true,
    "gridHeight":56,
    "multiSortKey": "ctrl",
    "clearFilter":true, 
    "columnsFiltersToDestroy":[ 'assignmentCreatedDate' ,'assignmentExpectedCompleteDate' ],
    "exportFileName":"Active Assignments"
};