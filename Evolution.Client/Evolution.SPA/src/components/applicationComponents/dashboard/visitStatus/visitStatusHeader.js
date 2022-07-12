import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const headerData = {
    "columnDefs": [
        {
            "headerName": localConstant.gridHeader.VISIT_DATE,
            "field": "visitStartDate",
            "filter": "agDateColumnFilter",           
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "visitStartDate"
            },
            "headerTooltip": localConstant.gridHeader.VISIT_DATE,
            "width":150,
            "tooltip": (params) => {
                return moment(params.data.visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },
        {
            "headerName": localConstant.gridHeader.VISIT_CUSTOMER_CONTRACT,
            "field": "visitId",
            "filter": "agTextColumnFilter",            
            "cellRenderer":"VisitAnchor",
            "cellRendererParams": (params) => {
                return { displayLinkText:params.data.visitContractNumber+'/'+params.data.visitProjectNumber+'/'+params.data.visitAssignmentNumber+'/'+params.data.visitNumber };
            },
            "valueGetter": (params) => {
                return params.data.visitContractNumber+'/'+params.data.visitProjectNumber+'/'+params.data.visitAssignmentNumber+'/'+params.data.visitNumber;
              },
              "headerTooltip": localConstant.gridHeader.VISIT_CUSTOMER_CONTRACT,
              "width":350,
            "tooltip":(params) => {
                return params.data.visitContractNumber+'/'+params.data.visitProjectNumber+'/'+params.data.visitAssignmentNumber+'/'+params.data.visitNumber;
                
              },
        },
        {
            "headerName":localConstant.gridHeader.REPORT_NUMBER,
            "field": "visitReportNumber",
            "filter": "agTextColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "headerTooltip": localConstant.gridHeader.REPORT_NUMBER,
            "width":180,
            "tooltipField":"visitReportNumber",
        },
        {
            "headerName":localConstant.gridHeader.CUSTOMER_NAME,
            "field": "visitCustomerName",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.CUSTOMER_NAME,
            "width":160,
            "tooltipField":"visitCustomerName",
        },
        {
            "headerName": localConstant.gridHeader.CUSTOMER_PROJECT_NAME,
            "field": "visitCustomerProjectName",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.CUSTOMER_PROJECT_NAME,
            "width":230,
            "tooltipField":"visitCustomerProjectName",
        },
        {
            "headerName": localConstant.gridHeader.SUPPLIER_PO_NUMBER,
            "field": "visitSupplierPONumber",
            "filter": "agTextColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "headerTooltip": localConstant.gridHeader.SUPPLIER_PO_NUMBER,
            "width":200,
            "tooltipField":"visitSupplierPONumber",
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
            "headerName": localConstant.gridHeader.STATUS,
            "field": "visitStatus",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.STATUS,
            "width":120,
            "tooltipField":"visitStatus",
        },
        {
            "headerName":localConstant.gridHeader.CONTRACT_COORDINATOR,
            "field": "visitContractCoordinator",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.CONTRACT_COORDINATOR,
            "width":200,
            "tooltipField":"visitContractCoordinator",
        },
        {
            "headerName": localConstant.gridHeader.OPERATING_COORDINATOR,
            "field": "visitOperatingCompanyCoordinator",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.OPERATING_COORDINATOR,
            "width":200,
            "tooltipField":"visitOperatingCompanyCoordinator",
        },
        {
            "headerName": localConstant.gridHeader.ASSIGNMENT_CREATE_DATE,
            "field": "visitAssignmentCreatedDate",
            "filter": "agDateColumnFilter",
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "visitAssignmentCreatedDate"
            },
            "headerTooltip": localConstant.gridHeader.ASSIGNMENT_CREATE_DATE,
            "width":220,
            "tooltip": (params) => {
                return moment(params.data.visitAssignmentCreatedDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },
        {
            "headerName": localConstant.gridHeader.CONTRACT_HOLDER_COMPANY,
            "field": "visitContractCompany",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.CONTRACT_HOLDER_COMPANY,
            "width":220,
            "tooltipField":"visitContractCompany",
        },
        {
            "headerName": localConstant.gridHeader.OPERATING_COMPANY,
            "field": "visitOperatingCompany",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.OPERATING_COMPANY,
            "width":200,
            "tooltipField":"visitOperatingCompany",
        },
        {
            "headerName": localConstant.gridHeader.NOTIFICATION_REF,
            "field": "visitNotificationReference",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.NOTIFICATION_REF,
            "width":200,
            "tooltipField":"visitNotificationReference",
        },
        {
            "headerName": localConstant.gridHeader.END_DATE,
            "field": "visitEndDate",
            "filter": "agDateColumnFilter",           
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "visitEndDate"
            },
            "headerTooltip": localConstant.gridHeader.END_DATE,
            "width":130,
            "tooltip": (params) => {
                return moment(params.data.visitEndDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },
        {
            "headerName": localConstant.gridHeader.VISIT_ASSiGNMENT_REFERENCE,
            "field": "visitAssignmentReference",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.VISIT_ASSiGNMENT_REFERENCE,
            "width":230,
            "tooltipField":"visitAssignmentReference",
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
    "exportFileName":"Visits Pending Approval",
    "columnsFiltersToDestroy":[ 'visitStartDate' ,'visitAssignmentCreatedDate','visitEndDate' ]
};