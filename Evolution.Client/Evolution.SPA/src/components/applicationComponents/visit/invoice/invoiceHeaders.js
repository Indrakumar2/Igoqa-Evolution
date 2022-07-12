import { isUndefined,getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';

const localConstant = getlocalizeData();
export const HeaderData = {

    "columnDefs": [
       
        {
            "headerName": localConstant.visit.INVOICENO,
            "field": "invoiceNo",
            "tooltipField":"invoiceNo",    
            "filter": "agTextColumnFilter",
            "cellRenderer": "visitAnchor",
            "width":120
        },
        {
            "headerName":  localConstant.visit.INVOICEDATE,
            "field": "invoiceDate",
            "tooltipField":"invoiceDate",    
            "filter": "agTextColumnFilter",
            "width":120
        },
        {
            "headerName":  localConstant.visit.CUSTOMER_PROJECT_NAME,
            "field": "customerProjectName",
            "tooltipField":"customerProjectName",    
            "filter": "agTextColumnFilter",
            "width":120
        },
        {
            "headerName":  localConstant.visit.ASSIGNMENT_NO,
            "field": "AssignmentNo",
            "tooltipField":"AssignmentNo",    
            "filter": "agTextColumnFilter",
            "width":200
        },
        {
            "headerName":  localConstant.visit.INVOICE_TOTAL,
            "field": "invoiceTotal",
            "tooltipField":"invoiceTotal",    
            "filter": "agTextColumnFilter",
            "width":200
        },
        {
            "headerName":  localConstant.visit.CURRENCY,
            "field": "currency",
            "tooltipField":"currency",    
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