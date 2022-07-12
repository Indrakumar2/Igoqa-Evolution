import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const headerData = {
    "columnDefs": [
        {
            "headerName": "Contract No",
            "field": "assignmentContractNumber",
            
            "filter": "agTextColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "width":150,  
            "tooltipField":"assignmentContractNumber",        
            "cellRenderer": "contractAnchorRenderer",
            "headerTooltip": "Contract No",
            // "cellRendererParams": {
            //     "dataToRender": "assignmentContractNumber"
            // }
            
        },
        {
            "headerName": "Project No",
            "field": "assignmentProjectNumber",
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "width":150,
            "tooltip":(params) => (params.data.assignmentProjectNumber),    
            "cellRenderer": "ProjectAnchor",
            "valueGetter": (params) => (params.data.assignmentProjectNumber),
            "headerTooltip": "Project No",
        },
        {
            "headerName": "Customer Project Name",
            "field": "assignmentProjectName",
            "filter": "agTextColumnFilter",
            "headerTooltip": "Customer Project Name",
            "width":220,
            "tooltipField":"assignmentProjectName",
            // "valueGetter": function aPlusBValueGetter(params) {
            //     return params.data.projectNumber +'and'+ params.data.projectName;
            //   }
        },
        {
            "headerName": "Assignment No",
            "field": "assignmentNumber",
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },           
            "cellRenderer": "AssignmentAnchor",
            "cellRendererParams": (params) => {                
                return { assignmentNumber: params.data.assignmentNumber.toString().padStart(5, '0'),assignmentId: params.data.assignmentId };
            }, 
            "headerTooltip": "Assignment No",
            "width":180,
            "tooltip":(params) => {                
                return  params.data.assignmentNumber.toString().padStart(5, '0');
            },
        },
        {
            "headerName": "Assignment Ref",
            "field": "assignmentReference",
            "filter": "agTextColumnFilter",
            "headerTooltip": "Assignment Ref",
            "width":180,
            "tooltipField":"assignmentReference",
        },
        {
            "headerName": "Customer",
            "field": "assignmentCustomerName",
            "filter": "agTextColumnFilter",
            "headerTooltip": "Customer",
            "width":150,
            "tooltipField":"assignmentCustomerName",
        },
        {
            "headerName": "Contract Coordinator",
            "field": "assignmentContractHoldingCompanyCoordinator",
            "filter": "agTextColumnFilter",
            "headerTooltip": "Contract Coordinator",
            "width":210,
            "tooltipField":"assignmentContractHoldingCompanyCoordinator",
        },
        {
            "headerName": "Operating Coordinator",
            "field": "assignmentOperatingCompanyCoordinator",
            "filter": "agTextColumnFilter",
            "headerTooltip": "Operating Coordinator",
            "width":220,
            "tooltipField":"assignmentOperatingCompanyCoordinator",
        },
        {
            "headerName": "Resource(s)",
            "field": "techSpecialists",
            "filter": "agTextColumnFilter",
            "headerTooltip": "Resource(s)",
            "width":200,
            "tooltipField":"techSpecialists",
        },
        {
            "headerName": "Supplier PO Number",
            "field": "assignmentSupplierPurchaseOrderNumber",
            "filter": "agTextColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "headerTooltip": "Supplier PO Number",
            "width":210,
            "tooltipField":"assignmentSupplierPurchaseOrderNumber",
        },     
        {
            "headerName": "Main Supplier",
            "field": "assignmentSupplierName",
            "filter": "agTextColumnFilter",
            "headerTooltip": "Main Supplier",
            "width":160,
            "tooltipField":"assignmentSupplierName",
        },
        {
            "headerName": "Assignment Created Date",
            "field": "assignmentCreatedDate",
            "filter": "agDateColumnFilter",           
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "assignmentCreatedDate"
            },
            "headerTooltip": "Assignment Created Date",
            "width":230,
            "tooltip": (params) => {
                return moment(params.data.assignmentCreatedDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },
        {
            "headerName": "Contract Holder Company",
            "field": "assignmentContractHoldingCompany",
            "filter": "agTextColumnFilter",
            "headerTooltip": "Contract Holder Company",
            "width":230,
            "tooltipField":"assignmentContractHoldingCompany",
        },
        {
            "headerName": "Operating Company",
            "field": "assignmentOperatingCompany",
            "filter": "agTextColumnFilter",
            "headerTooltip": "Operating Company",
            "width":210,
            "tooltipField":"assignmentOperatingCompany",
        },
        {
            "headerName": "Assignment Status",
            "field": "assignmentStatus",
            "filter": "agTextColumnFilter",
            "valueGetter": (params) => {
                if(params.data.assignmentStatus == "P"){
                    return "In Progress";
                }else if(params.data.assignmentStatus == "C"){
                  return "Complete";              }
                
              },
              "headerTooltip": "Assignment Status",
              "width":200,
            "tooltip":(params) => {
                if(params.data.assignmentStatus == "P"){
                    return "In Progress";
                }else if(params.data.assignmentStatus == "C"){
                  return "Complete";              }
                
              },
        }
    ],
    "enableFilter":true, 
    "enableSorting":true, 
    "pagination": true,
    "searchable":true,
    "gridActions":true,
    "gridTitlePanel":true,
    "gridHeight":59,
    "clearFilter":true, 
    "multiSortKey": "ctrl",
    "exportFileName":"Inactive Assignments",
    "columnsFiltersToDestroy":[ 'assignmentCreatedDate' ]
};