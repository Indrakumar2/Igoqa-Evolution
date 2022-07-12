import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();

export const HeaderData = {
    "columnDefs": [
        {
            'headerName': '',
            'field': 'delete',
            'width': 60,
            'maxWidth': 70,
            'headerCheckboxSelection': true,
            'headerCheckboxSelectionFilteredOnly': true,
            'checkboxSelection': true,
            'cellStyle': (params) => {
                return params.data.processStatus === 1 ?
                    { 'pointer-events': 'none', 'display': 'none' }
                    : { 'display' : 'block' };
            }
        },
        {
            "headerName": localConstant.report.reportFileName,
            "headerTooltip": localConstant.report.reportFileName,
            "field": "reportFileName",
            "filter": "agTextColumnFilter",
            "tooltipField": "displayFileName",
            "width": 250
        },
        {
            "headerName": localConstant.report.reportCreatedDate,
            "headerTooltip": localConstant.report.reportCreatedDate,
            "field": "createdDate",
            "tooltip": (params) => {
                return moment(params.data.createdDate).format(localConstant.commonConstants.AUDIT_DATE_FORMAT_TIME);
            },
            "width": 200,
            "cellRenderer": (params) => {
                return moment(params.data.createdDate).format(localConstant.commonConstants.AUDIT_DATE_FORMAT_TIME);
            }
        },
        {
            "headerName": localConstant.report.reportStatus,
            "headerTooltip": localConstant.report.reportStatus,
            "field": "fileStatus",
            "filter": "agTextColumnFilter",
            "tooltipField": "fileStatus",
            "width": 200
        },
        {
            "headerName": localConstant.report.reportRemarks,
            "headerTooltip": localConstant.report.reportRemarks,
            "field": "errorMessage",
            "filter": "agTextColumnFilter",
            "tooltipField": "errorMessage",
            "width": 200,
        },
        {
            "headerName": localConstant.report.reportId,
            "headerTooltip": localConstant.report.reportId,
            "field": "id",
            "filter": "agTextColumnFilter",
            "tooltipField": "id",
            "width": 100
        },
        {
            "headerName": localConstant.visit.DOWNLOAD,  
            "headerTooltip": localConstant.visit.DOWNLOAD,
            "field": "documentName",                      
            "tooltipField": "documentName",           
            "cellRenderer": "ReportToDownload",
            "suppressFilter": true,
            "suppressSorting": true,
             "cellRendererParams": {
                 "dataToRender": "documentName"
             },
             "width": 150
        }
    ],

    "enableFilter": false,
    "enableSorting": true,
    "pagination": true,
    "gridTitlePanel": true,
    "clearFilter": true,
    "gridActions": true,
    "searchable": true,
    "rowSelection": "multiple",
    "exportFileName": "Reports",
    "customHeader": "Reports",
    "gridHeight": 25,
};