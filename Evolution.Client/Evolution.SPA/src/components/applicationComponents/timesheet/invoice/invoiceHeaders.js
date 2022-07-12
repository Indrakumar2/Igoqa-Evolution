import { getlocalizeData } from '../../../../utils/commonUtils';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData = {
    "columnDefs": [
        {
            "headerName": localConstant.timesheet.INVOICENO,
            "field": "invoiceNo",
            "tooltipField": "invoiceNo",
            "filter": "agTextColumnFilter",
            "cellRenderer": "timesheetAnchor",
            "width":120
        },
        {
            "headerName":  localConstant.timesheet.INVOICEDATE,
            "field": "invoiceDate",
            "tooltip": (params) => {
                return moment(params.data.invoiceDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filter": "agTextColumnFilter",
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "invoiceDate"
            },
            "width":120
        },
        {
            "headerName":  localConstant.timesheet.CUSTOMER_PROJECT_NAME,
            "field": "customerProjectName",
            "tooltipField": "customerProjectName",
            "filter": "agTextColumnFilter",
            "width":120
        },
        {
            "headerName":  localConstant.gridHeader.ASSIGNMENT_NO,
            "field": "invoiceAssignmentNumber",
            "tooltipField": "invoiceAssignmentNumber",
            "filter": "agTextColumnFilter",
            "cellRenderer": "AssignmentAnchor",
            "cellRendererParams": (params) => {                
                return { assignmentNumber: params.data.timesheetAssignmentNumber.toString().padStart(5,'0'), assignmentId: params.data.assignmentId };
            }, 
            "width":200
        },
        {
            "headerName":  localConstant.timesheet.INVOICE_TOTAL,
            "field": "invoiceTotal",
            "tooltipField": "invoiceTotal",
            "filter": "agTextColumnFilter",
            "width":200
        },
        {
            "headerName":  localConstant.gridHeader.CURRENCY,
            "field": "currency",
            "tooltipField": "currency",
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
    "gridActions": true,
    "gridTitlePanel": true
};