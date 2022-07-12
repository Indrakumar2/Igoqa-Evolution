import { getlocalizeData } from '../../../../utils/commonUtils';
import moment from 'moment';
import dateUtil from '../../../../utils/dateUtil';
const localConstant = getlocalizeData();
export const HeaderData={
    "VisitHeader":{
        "columnDefs":[
            
            {
                "headerName":localConstant.gridHeader.JOB_REFERENCE_NUMBER,
                "headerTooltip": localConstant.gridHeader.JOB_REFERENCE_NUMBER,
                "field":"visitJobReference",
                "tooltipField": "visitJobReference",
                "filter":"agTextColumnFilter",
                "cellRenderer": "VisitAnchor",
                "cellRendererParams": (params) => {
                    return { displayLinkText:params.data.visitJobReference };
                },
                "width":180
            },
            {
                "headerName":localConstant.gridHeader.REPORT_NUMBER,
                "headerTooltip": localConstant.gridHeader.REPORT_NUMBER,
                "field":"visitReportNumber",
                "tooltipField": "visitReportNumber",
                "filter":"agTextColumnFilter",                
                "width":150
            },
            // {
            //     "headerName": localConstant.gridHeader.TECHNICAL_SPECIALISTS,
            //     "headerTooltip": localConstant.gridHeader.TECHNICAL_SPECIALISTS,
            //     "field":"technicalSpecialist",
            //     "tooltipField": "technicalSpecialist",
            //     "filter":"agTextColumnFilter",
            //     "width":160
            // },
            {
                "headerName": localConstant.visit.TECHNICALSPECIALIST,
                "headerTooltip":localConstant.visit.TECHNICALSPECIALIST,
                "field": "techSpecialists",
                "tooltip": (params) => {
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
                "width": 200
            },
            {
                "headerName":localConstant.gridHeader.VISIT_DATE,
                "headerTooltip": localConstant.gridHeader.VISIT_DATE,
                "field":"visitStartDate",
                "tooltip": (params) => {
                    return moment(params.data.visitStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
            }, 
                "filter":"agDateColumnFilter",
                "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "dataToRender": "visitStartDate"
                },
                "width":120,
                "filterParams": {
                    "comparator": dateUtil.comparator,
                    "inRangeInclusive": true,
                    "browserDatePicker": true
                } 
            },            
            {
                "headerName": localConstant.gridHeader.STATUS,
                "headerTooltip": localConstant.gridHeader.STATUS,
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
                "width":160
            },
            {
                "headerName":localConstant.gridHeader.SUPPLIER_NAME,
                "headerTooltip": localConstant.gridHeader.SUPPLIER_NAME,
                "field":"visitSupplier",
                "tooltipField": "visitSupplier",
                "filter":"agTextColumnFilter",
                "width":190                
            },
            {
                "headerName":localConstant.gridHeader.FINAL_VISIT,
                "headerTooltip": localConstant.gridHeader.FINAL_VISIT,
                "field":"finalVisit",
                "tooltipField": "finalVisit",
                "filter":"agTextColumnFilter",
                "width":120
            }
        ],
        "enableFilter":true, 
        "enableSorting":true, 
        "pagination": true,
        "gridHeight":55,
        "searchable":true,
        "gridActions":true,
        "clearFilter":true, 
        "gridTitlePanel":true,
        "exportFileName":"Assignment Visit's Data",
        "columnsFiltersToDestroy":[ 'visitStartDate' ]
    }
};
