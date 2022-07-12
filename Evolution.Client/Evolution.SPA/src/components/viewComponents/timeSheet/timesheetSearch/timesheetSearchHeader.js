import { getlocalizeData } from '../../../../utils/commonUtils';
import arrayUtil from '../../../../utils/arrayUtil';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData = {

    "columnDefs": [
        {
            "headerName": localConstant.gridHeader.TIMESHEET_NUMBER,
            "field": "timesheetNumber",
            "cellRenderer": "TimesheetAnchor",
            "cellRendererParams": (params) => {
                return { timesheetNumber: params.data.timesheetNumber ,timesheetId:params.data.timesheetId };
            },
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive": true
            },
            "headerTooltip": localConstant.gridHeader.TIMESHEET_NUMBER,
            "tooltipField":'timesheetNumber',
            "width": 180
        },
        {
            "headerName": localConstant.gridHeader.CUSTOMER,
            "headerTooltip": localConstant.gridHeader.CUSTOMER,
            "field": "timesheetCustomerName",
            "tooltipField": "timesheetCustomerName",
            "filter": "agTextColumnFilter", 
            "width":150
        },
        {
            "headerName":  localConstant.gridHeader.CONTRACT_NO,
            "field": "timesheetContractNumber",
            "tooltipField": "timesheetContractNumber",
            "filter": "agTextColumnFilter",
            "filterParams": {
                "inRangeInclusive": true
            },
            "cellRenderer": "contractAnchorRenderer",
            "headerTooltip": localConstant.gridHeader.CONTRACT_NO,
            "cellRendererParams": (params) => {
                return { data: { contractNumber: params.data.timesheetContractNumber, contractHoldingCompanyCode:  params.data.timesheetContractHolderCompanyCode } };
            }, 
            "width":150
        },
        {
            "headerName":  localConstant.gridHeader.PROJECT_NO,
            "headerTooltip":  localConstant.gridHeader.PROJECT_NO,
            "field": "timesheetProjectNumber",
            "tooltipField": "timesheetProjectNumber",
            "filter": "agTextColumnFilter",
            "cellRenderer": "ProjectAnchor",
            "valueGetter": (params) => (params.data.timesheetProjectNumber),
            "width":150
        },
        {
            "headerName":  localConstant.gridHeader.TIMESHEET_DATE,
            "headerTooltip":localConstant.gridHeader.TIMESHEET_DATE,
            "field": "timesheetStartDate",
            "tooltip": (params) => {
                let d = '';
                if(params.data.timesheetStartDate){
                    d = moment(params.data.timesheetStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }
                return d;
            }, 
            "filter": "agDateColumnFilter",
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "timesheetStartDate"
            },
            "width":200,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
            }
        },
        {
            "headerName":  localConstant.gridHeader.STATUS,
            "headerTooltip":  localConstant.gridHeader.STATUS,
            "field": "timesheetStatus",
            "filter": "agTextColumnFilter",
            "valueGetter": (params) => {

              const statusObj =  arrayUtil.FilterGetObject(localConstant.commonConstants.timesheet_Status,
                    'value',params.data.timesheetStatus);
                return statusObj ? statusObj.name : '';
            },
            "tooltip": (params) => {
              const statusObj =  arrayUtil.FilterGetObject(localConstant.commonConstants.timesheet_Status,
                'value',params.data.timesheetStatus);
            return statusObj ? statusObj.name : '';
            }, 
            "width":200
        },
        {
            "headerName":  localConstant.gridHeader.ASSIGNMENT_NO,
            "headerTooltip":  localConstant.gridHeader.ASSIGNMENT_NO,
            "field": "timesheetAssignmentNumber",
            "tooltipField": "timesheetAssignmentNumber",
            "filter": "agTextColumnFilter",
            "cellRenderer": "AssignmentAnchor",
            "cellRendererParams": (params) => {                
                return { assignmentNumber: params.data.timesheetAssignmentNumber.toString().padStart(5,'0'), assignmentId: params.data.timesheetAssignmentId };
            }, 
            "width":200
        },
        {
            "headerName":  localConstant.gridHeader.TECHNICAL_SPECIALIST,
            "field": "techSpecialists",
            "tooltip": (params) => {
                const { techSpecialists } = params.data;
                if (techSpecialists) {
                    return techSpecialists.filter(ts => ts.fullName !== ' ').map(e => e.fullName.includes(e.pin) ? e.fullName : (e.fullName + " (" +  e.pin + ")")).join(",");
                } else {
                    return "";
                }
            },
            "headerTooltip": localConstant.gridHeader.TECHNICAL_SPECIALIST,
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
            "headerName":  localConstant.gridHeader.CH_COORDINATOR_NAME,
            "headerTooltip":  localConstant.gridHeader.CH_COORDINATOR_NAME,
            "field": "timesheetContractHolderCoordinator",
            "tooltipField": "timesheetContractHolderCoordinator",
            "filter": "agTextColumnFilter",
            "width":200
        },
        {
            "headerName":  localConstant.gridHeader.OC_COORDINATOR_NAME,
            "headerTooltip":  localConstant.gridHeader.OC_COORDINATOR_NAME,
            "field": "timesheetOperatingCompanyCoordinator",
            "tooltipField": "timesheetOperatingCompanyCoordinator",
            "filter": "agTextColumnFilter",
            "width":200
        },
        {
            "headerName":  localConstant.gridHeader.CONTRACT_COMPANY,
            "headerTooltip":  localConstant.gridHeader.CONTRACT_COMPANY,
            "field": "timesheetContractHolderCompany",
            "tooltipField": "timesheetContractHolderCompany",
            "filter": "agTextColumnFilter",
            "width":200
        },
        {
            "headerName":  localConstant.gridHeader.OPERATING_COMPANY,
            "headerTooltip":  localConstant.gridHeader.OPERATING_COMPANY,
            "field": "timesheetOperatingCompany",
            "tooltipField": "timesheetOperatingCompany",
            "filter": "agTextColumnFilter",
            "width":200
        },
        {
            "headerName":  localConstant.gridHeader.CUSTOMER_PROJECT_NAME,
            "headerTooltip":  localConstant.gridHeader.CUSTOMER_PROJECT_NAME,
            "field": "customerProjectName",
            "tooltipField": "customerProjectName",
            "filter": "agTextColumnFilter",
            "width":230
        },
        {
            "headerName":  localConstant.gridHeader.TIMESHEET_DESCRIPTION,
            "headerTooltip":  localConstant.gridHeader.TIMESHEET_DESCRIPTION,
            "field": "timesheetDescription",
            "tooltipField": "timesheetDescription",
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
    "clearFilter":true,
    "gridTitlePanel": true,
    "exportFileName":"Timesheet Search Data"
};