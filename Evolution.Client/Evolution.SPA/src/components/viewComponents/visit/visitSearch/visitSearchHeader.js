import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData = {
    visitSearchHeader: {
        "columnDefs": [
            {
                "headerName": localConstant.visit.VISIT_NUMBER,
                "headerTooltip": localConstant.visit.VISIT_NUMBER,
                "field": "visitNumber",
                "tooltipField": "visitNumber",
                "filter": "agNumberColumnFilter",
                "filterParams": {
                    "inRangeInclusive":true
                },
                "cellRenderer": "VisitAnchor",
                "width": 150
            },
            {
                "headerName": localConstant.visit.CUSTOMER,
                "headerTooltip": localConstant.visit.CUSTOMER,
                "field": "visitCustomerName",
                "tooltipField": "visitCustomerName",
                "filter": "agTextColumnFilter",
                "width": 180
            },
            {
                "headerName": localConstant.visit.CONTRACT_NUMBER,
                "headerTooltip": localConstant.visit.CONTRACT_NUMBER,
                "field": "visitContractNumber",
                "tooltipField": "visitContractNumber",
                "filter": "agTextColumnFilter",
                "cellRenderer": "contractAnchorRenderer",            
                "cellRendererParams": (params) => {
                    return { data: { contractNumber: params.data.visitContractNumber, contractHoldingCompanyCode:  params.data.visitContractCompanyCode } };
                },
                "width": 180
            },
            {
                "headerName": localConstant.visit.PROJECT_NO,
                "headerTooltip": localConstant.visit.PROJECT_NO,
                "field": "visitProjectNumber",
                "tooltipField": "visitProjectNumber",
                "filter": "agNumberColumnFilter",
                "filterParams": {
                    "inRangeInclusive":true
                },
                "cellRenderer": "ProjectAnchor",
                "valueGetter": (params) => (params.data.visitProjectNumber),
                "width": 150
            },
            {
                "headerName": localConstant.visit.CUSTOMER_CONTRACT_NO,
                "headerTooltip": localConstant.visit.CUSTOMER_CONTRACT_NO,
                "field": "visitCustomerContractNumber",
                "tooltipField": "visitCustomerContractNumber",
                "filter": "agTextColumnFilter",
                "width": 200
            },
            {
                "headerName": localConstant.visit.ASSIGNMENT_NO,
                "headerTooltip": localConstant.visit.ASSIGNMENT_NO,
                "field": "visitAssignmentNumber",
                "tooltipField": "visitAssignmentNumber",
                "filter": "agNumberColumnFilter",
                "filterParams": {
                    "inRangeInclusive":true
                },
                "cellRenderer": "AssignmentAnchor",
                "cellRendererParams": (params) => {
                    return { assignmentNumber: params.data.visitAssignmentNumber.toString().padStart(5, '0'), assignmentId: params.data.visitAssignmentId };
                },
                "width": 200
            },
            {
                "headerName": localConstant.visit.CUSTOMER_PROJECT_NAME,
                "headerTooltip": localConstant.visit.CUSTOMER_PROJECT_NAME,
                "field": "visitCustomerProjectName",
                "tooltipField": "visitCustomerProjectName",
                "filter": "agTextColumnFilter",
                "width": 230
            },
            {
                "headerName": localConstant.visit.CUSTOMER_PROJECT_NO,
                "headerTooltip": localConstant.visit.CUSTOMER_PROJECT_NO,
                "field": "visitCustomerProjectNumber",
                "tooltipField": "visitCustomerProjectNumber",
                "filter": "agTextColumnFilter",
                "width": 230
            },
            {
                "headerName": localConstant.visit.CH_COORDINATOR_NAME,
                "headerTooltip": localConstant.visit.CH_COORDINATOR_NAME,
                "field": "visitContractCoordinator",
                "tooltipField": "visitContractCoordinator",
                "filter": "agTextColumnFilter",
                "width": 230
            },
            {
                "headerName": localConstant.visit.OC_COORDINATOR_NAME,
                "headerTooltip": localConstant.visit.OC_COORDINATOR_NAME,
                "field": "visitOperatingCompanyCoordinator",
                "tooltipField": "visitOperatingCompanyCoordinator",
                "filter": "agTextColumnFilter",
                "width": 200
            },
            {
                "headerName": localConstant.visit.SUPPLIER_SUBSUPPLIER,
                "headerTooltip": localConstant.visit.SUPPLIER_SUBSUPPLIER,
                "field": "visitSupplier",
                "tooltipField": "visitSupplier",
                "filter": "agTextColumnFilter",
                "width": 200
            },
            {
                "headerName": localConstant.visit.VISIT_STATUS,
                "headerTooltip": localConstant.visit.VISIT_STATUS,
                "field": "visitStatus",
                "tooltip":  (params) => {
                    if(params.data.visitStatus === 'A'){
                        return "Approved By Contract Holder";
                    }
                    else if(params.data.visitStatus === 'C'){
                    return "Awaiting Approval";      
                            }
                    else if(params.data.visitStatus === 'J'){
                    return "Rejected By Operator";
                    }
                    else if(params.data.visitStatus === 'N'){
                        return "Not Submitted";      
                    }
                    else if(params.data.visitStatus === 'O'){
                        return "Approved By Operator";      
                    }
                    else if(params.data.visitStatus === 'Q'){
                        return "Confirmed - Awaiting Visit";      
                    }
                    else if(params.data.visitStatus === 'R'){
                        return "Rejected By Contract Holder";      
                    }
                    else if(params.data.visitStatus === 'S'){
                        return "Confirmed by Supplier - Awaiting Client Confirmation";      
                    }
                    else if(params.data.visitStatus === 'T'){
                        return "Tentative - Pending Approval";      
                    }
                    else if(params.data.visitStatus === 'U'){
                        return "TBA - Date Unknown";      
                    }
                    else if(params.data.visitStatus === 'W'){
                        return "Confirmed by Client - Awaiting Visit";      
                    }
                    else if(params.data.visitStatus === 'D'){
                        return "Unused Visit";      
                    }
                },
                "filter": "agTextColumnFilter",
                "valueGetter": (params) => {
                    if(params.data.visitStatus === 'A'){
                        return "Approved By Contract Holder";
                    }
                    else if(params.data.visitStatus === 'C'){
                    return "Awaiting Approval";      
                            }
                    else if(params.data.visitStatus === 'J'){
                    return "Rejected By Operator";
                    }
                    else if(params.data.visitStatus === 'N'){
                        return "Not Submitted";      
                    }
                    else if(params.data.visitStatus === 'O'){
                        return "Approved By Operator";      
                    }
                    else if(params.data.visitStatus === 'Q'){
                        return "Confirmed - Awaiting Visit";      
                    }
                    else if(params.data.visitStatus === 'R'){
                        return "Rejected By Contract Holder";      
                    }
                    else if(params.data.visitStatus === 'S'){
                        return "Confirmed by Supplier - Awaiting Client Confirmation";      
                    }
                    else if(params.data.visitStatus === 'T'){
                        return "Tentative - Pending Approval";      
                    }
                    else if(params.data.visitStatus === 'U'){
                        return "TBA - Date Unknown";      
                    }
                    else if(params.data.visitStatus === 'W'){
                        return "Confirmed by Client - Awaiting Visit";      
                    }
                    else if(params.data.visitStatus === 'D'){
                        return "Unused Visit";      
                    }
                },   
                "width": 200
            },
            {
                "headerName": localConstant.visit.SUPPLIER_PO_NO,
                "headerTooltip": localConstant.visit.SUPPLIER_PO_NO,
                "field": "visitSupplierPONumber",
                "tooltipField": "visitSupplierPONumber",
                "filter": "agTextColumnFilter",
                "width": 200
            },
            {
                "headerName": localConstant.visit.MATERIAL_DESCRIPTION,
                "headerTooltip": localConstant.visit.MATERIAL_DESCRIPTION,
                "field": "visitMaterialDescription",
                "tooltipField": "visitMaterialDescription",
                "filter": "agTextColumnFilter",
                "width": 200
            },
            {
                "headerName": localConstant.visit.VISIT_DATE,
                "headerTooltip": localConstant.visit.VISIT_DATE,
                "field": "visitStartDate",
                // "tooltip": (params) => {
                //     console.log(params.data.visitStartDate);
                //     let d ='';
                //     d= moment(params.data.visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                //     if(isEmptyOrUndefine(d))
                //     d ==='';
                //     console.log(d);
                //     return d;
                // },
                "tooltip": (params) => {
                    let d='';
                    if(params.data.visitStartDate){
                     d = moment(params.data.visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                    }
                    return d===""?"":d;
                }, 
                "filter": "agDateColumnFilter",
                "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "dataToRender": "visitStartDate"
                },
                "width": 200,
                "filterParams": {
                    "comparator": dateUtil.comparator,
                    "inRangeInclusive": true,
                    "browserDatePicker": true
                }
            },
            {
                "headerName": localConstant.visit.SENT_TO_CUSTOMER,
                "headerTooltip": localConstant.visit.SENT_TO_CUSTOMER,
                "field": "visitReportSentToCustomerDate",
                // "tooltip": (params) => {
                //     return moment(params.data.visitReportSentToCustomerDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                // },
                "tooltip": (params) => {
                    let d ='';
                    if(params.data.visitReportSentToCustomerDate){
                    d = moment(params.data.visitReportSentToCustomerDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                    }
                    return d===""?"":d ;
                },
                "filter": "agDateColumnFilter",
                "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "dataToRender": "visitReportSentToCustomerDate"
                },
                "width": 200,
                "filterParams": {
                    "comparator": dateUtil.comparator,
                    "inRangeInclusive": true,
                    "browserDatePicker": true
                }
            },
            {
                "headerName": localConstant.visit.REPORT_NO,
                "headerTooltip": localConstant.visit.REPORT_NO,
                "field": "visitReportNumber",
                "tooltipField": "visitReportNumber",
                "filter": "agTextColumnFilter",
                "width": 200
            },
            {
                "headerName": localConstant.visit.TECHNICALSPECIALISTS,
                "headerTooltip": localConstant.visit.TECHNICALSPECIALISTS,
                "field": "techSpecialists",
                //"tooltip":"techSpecialists",
                "tooltip": (params) => {
                    const { techSpecialists } = params.data;
                    if (techSpecialists) {                    
                        return techSpecialists.filter(ts => ts.fullName !== ' ').map(e => e.fullName.includes(e.pin) ? e.fullName : (e.fullName + " (" +  e.pin + ")")).join(",");
                    } else {
                        return "";
                    }
                },
                "filter": "agTextColumnFilter",
                "valueGetter": (params) => {
                    const { techSpecialists } = params.data;
                    if (techSpecialists) {                    
                        return techSpecialists.filter(ts => ts.fullName !== ' ').map(e => e.fullName.includes(e.pin) ? e.fullName : (e.fullName + " (" +  e.pin + ")")).join(",");
                    } else {                    
                        return "";
                    }
                },
                "width": 200
            },
            {
                "headerName": localConstant.visit.CONTRACT_COMPANY,
                "headerTooltip": localConstant.visit.CONTRACT_COMPANY,
                "field": "visitContractCompany",
                "tooltipField": "visitContractCompany",
                "filter": "agTextColumnFilter",
                "width": 250
            },
            {
                "headerName": localConstant.visit.OPERATING_COMPANY,
                "headerTooltip": localConstant.visit.OPERATING_COMPANY,
                "field": "visitOperatingCompany",
                "tooltipField": "visitOperatingCompany",
                "filter": "agTextColumnFilter",
                "width": 250
            },
            {
                "headerName": localConstant.visit.NOTIFICATION_REF,
                "headerTooltip": localConstant.visit.NOTIFICATION_REF,
                "field": "visitNotificationReference",
                "tooltipField": "visitNotificationReference",
                "filter": "agTextColumnFilter",
                "width": 200
            }
        ],

        "rowSelection": 'single',
        "pagination": true,
        "enableFilter": true,
        "enableSorting": true,
        "gridHeight": 60,
        "searchable": true,
        "gridActions": true,
        "clearFilter":true,
        "gridTitlePanel": true,
        "exportFileName":"Visit Search Data"
    },
    supplierSearchHeader: {
        "columnDefs": [
            {
                "headerCheckboxSelectionFilteredOnly": true,
                "checkboxSelection": true,
                "suppressFilter": true,
                "width": 40
            },
            {
                "headerName": localConstant.gridHeader.NAME,
                "field": "supplierName",
                "headerTooltip":localConstant.gridHeader.NAME,
                "tooltipField": "supplierName",
                "filter": "agTextColumnFilter",
                "width": 170
            },
            {
                "headerName": localConstant.gridHeader.COUNTRY,
                "field": "country",
                "headerTooltip":localConstant.gridHeader.COUNTRY,
                "tooltipField": "country",
                "filter": "agTextColumnFilter",
                "width": 100
            },
            {
                "headerName": localConstant.gridHeader.COUNTY,
                "field": "state",
                "headerTooltip":localConstant.gridHeader.COUNTY,
                "tooltipField": "state",
                "filter": "agTextColumnFilter",
                "width": 100
            },
            {
                "headerName": localConstant.gridHeader.CITY,
                "field": "city",
                "headerTooltip":localConstant.gridHeader.CITY,
                "tooltipField": "city",
                "filter": "agTextColumnFilter",
                "width": 100
            },
            {
                "headerName": localConstant.gridHeader.FULL_ADDRESS,
                "field": "supplierAddress",
                "headerTooltip":localConstant.gridHeader.FULL_ADDRESS,
                "tooltipField": "supplierAddress",
                "filter": "agTextColumnFilter",
                "width": 210
            },
        ],
        "rowSelection": 'single',       
        "enableSorting": true,
        "gridHeight": 50,
        "pagination": true,
        "enableFilter": true,  
        "gridTitlePanel": true,
        "clearFilter":true, 
        "searchable": true,      
    },
};