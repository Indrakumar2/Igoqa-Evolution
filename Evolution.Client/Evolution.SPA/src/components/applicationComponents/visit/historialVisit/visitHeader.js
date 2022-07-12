import { isUndefined,getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData = {

    "columnDefs": [       
        {
            "headerName": localConstant.visit.REPORT_NO,
            "headerTooltip": localConstant.visit.REPORT_NO,
            "field": "visitReportNumber",
            "tooltipField":"visitReportNumber",    
            "filter": "agTextColumnFilter",
            //"cellRenderer": "VisitAnchor",
            "width":120
        },
        {
            "headerName":  localConstant.visit.RESOURCES,
            "headerTooltip": localConstant.visit.RESOURCES,
            "field": "techSpecialists",
            "tooltip":(params) => {
                const { techSpecialists } = params.data;
                if (techSpecialists) {
                    return techSpecialists.filter(ts => ts.fullName !== ' ').map(e => e.fullName).join(",");
                } else {
                    return "";
                }
            },   
            "filter": "agTextColumnFilter",
            "valueGetter": (params) => {
                const { techSpecialists } = params.data;
                if (techSpecialists) {
                    return techSpecialists.filter(ts => ts.fullName !== ' ').map(e => e.fullName).join(",");
                } else {
                    return "";
                }
            },
            "width":120
        },
        {
            "headerName":  localConstant.visit.VISIT_DATE,
            "headerTooltip": localConstant.visit.VISIT_DATE,
            "field": "visitStartDate",
            "tooltip": (params) => {
                return moment(params.data.visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filter": "agDateColumnFilter",//D326
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "visitStartDate"
            },
            "width":120
        },
        {
            "headerName":  localConstant.visit.VISIT_STATUS,
            "headerTooltip": localConstant.visit.VISIT_STATUS,
            "field": "visitStatus",
            "sort": "asc",
            "tooltip": (params) => {
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
            "width":200
        },
        {
            "headerName":  localConstant.visit.SUPPLIER_SUBSUPPLIER,
            "headerTooltip": localConstant.visit.SUPPLIER_SUBSUPPLIER,
            "field": "visitSupplier",
            "tooltipField":"visitSupplier", //Changes for ITK 1137    
            "filter": "agTextColumnFilter",
            "width":200
        },
        {
            "headerName":  localConstant.visit.JOBREFERENCENO,
            "headerTooltip": localConstant.visit.JOBREFERENCENO,
            "field": "visitJobReference",
            "tooltipField":"visitJobReference",    
            "filter": "agTextColumnFilter",
            "cellRenderer": "VisitAnchor",
            "cellRendererParams": (params) => {
                return { displayLinkText:params.data.visitJobReference };
            },
            "width":200
        },
        {
            "headerName":  localConstant.visit.FINAL_VISIT,
            "headerTooltip": localConstant.visit.FINAL_VISIT,
            "field": "finalVisit",
            "tooltipField":"finalVisit",    
            "filter": "agTextColumnFilter",
            "width":200
        }
        
    ],
    
    "rowSelection": 'single',   
    "pagination": true,
    "enableFilter": true,
    "enableSorting": true,
    "gridHeight": 60,
    "searchable": true,
    "gridActions": false,
    "gridTitlePanel": true
};