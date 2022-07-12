import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();
export const HeaderData = {
    "columnDefs": [
        {
            "headerName": "Timesheet Date",
            "field": "timesheetStartDate",
            "filter": "agDateColumnFilter",           
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "timesheetStartDate"
            },
            "headerTooltip": "Timesheet Date",
            "width":180,
            "tooltip": (params) => {
                return moment(params.data.timesheetStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();

        }, 
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },
        {
            "headerName": "Customer/Contract/Project/Assignment/Timesheet",
            "field": "timesheetId",
            "filter": "agTextColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "cellRenderer":"TimesheetAnchor",
            "cellRendererParams": (params) => {
                return { displayLinkText:params.data.timesheetContractNumber+'/'+params.data.timesheetProjectNumber+'/'+params.data.timesheetAssignmentNumber+'/'+params.data.timesheetNumber };
            },
            "valueGetter": (params) => {
                return params.data.timesheetContractNumber+'/'+params.data.timesheetProjectNumber+'/'+params.data.timesheetAssignmentNumber+'/'+params.data.timesheetNumber;
              },
              "headerTooltip": "Customer/Contract/Project/Assignment/Timesheet",
              "width":380,
            "tooltip": (params) => {
                return params.data.timesheetContractNumber+'/'+params.data.timesheetProjectNumber+'/'+params.data.timesheetAssignmentNumber+'/'+params.data.timesheetNumber;
                
              },
        },
        {
            "headerName": "Customer Name",
            "field": "timesheetCustomerName",
            "filter": "agTextColumnFilter",
            "headerTooltip": "Customer Name",
            "width":180,
            "tooltipField":"timesheetCustomerName",
        },
        {
            "headerName": "Customer Project Name",
            "field": "customerProjectName",
            "filter": "agTextColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "headerTooltip": "Customer Project Name",
            "width":230,
            "tooltipField":"customerProjectName",
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
            "headerName": "Status",
            "field": "timesheetStatus",
            "filter": "agTextColumnFilter",
            "valueGetter": (params) => {
                  if (params.data.timesheetStatus === "A")
                        return "Approved By Contract Holder";
                    if (params.data.timesheetStatus === "C")
                         return "Awaiting Approval";
                    if (params.data.timesheetStatus === "J")
                        return "Rejected By Operator";
                    if (params.data.timesheetStatus === "O")
                        return "Approved By Operator";
                    if (params.data.timesheetStatus === "N")
                        return " Not Submitted";
                    if (params.data.timesheetStatus == "R")
                        return "Rejected By Contract Holder";
                    if (params.data.timesheetStatus == "E")
                        return "Unused Timesheet";
                
              },
              "headerTooltip": "Status",
              "width":120,
            "tooltip":(params) => {
                if (params.data.timesheetStatus === "A")
                      return "Approved By Contract Holder";
                  if (params.data.timesheetStatus === "C")
                       return "Awaiting Approval";
                  if (params.data.timesheetStatus === "J")
                      return "Rejected By Operator";
                  if (params.data.timesheetStatus === "O")
                      return "Approved By Operator";
                  if (params.data.timesheetStatus === "N")
                      return " Not Submitted";
                  if (params.data.timesheetStatus == "R")
                      return "Rejected By Contract Holder";
                  if (params.data.timesheetStatus == "E")
                      return "Unused Timesheet";
              
            },
        },
        {
            "headerName": "Contract Coordinator",
            "field": "timesheetContractCoordinator",
            "filter": "agTextColumnFilter",
            "headerTooltip": "Contract Coordinator",
            "width":210,
            "tooltipField":"timesheetContractCoordinator",
        },
        {
            "headerName": "Operating Coordinator",
            "field": "timesheetOperatingCoordinator",
            "filter": "agTextColumnFilter",
            "headerTooltip": "Operating Coordinator",
            "width":210,
            "tooltipField":"timesheetOperatingCoordinator",
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
            "width":220,
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
            "headerName": "Timesheet Description",
            "field": "timesheetDescription",
            "filter": "agTextColumnFilter",
            "headerTooltip": "Timesheet Description",
            "width":210,
            "tooltipField":"timesheetDescription",
        },
        {
            "headerName": "Operating Company",
            "field": "timesheetOperatingCompany",
            "filter": "agTextColumnFilter",
            "headerTooltip": "Operating Company",
            "width":200,
            "tooltipField":"timesheetOperatingCompany",
        },
        {
            "headerName": "Assignment Ref",
            "field": "timesheetReference1",
            "filter": "agTextColumnFilter",
            "headerTooltip": "Assignment Ref",
            "width":200,
            "tooltipField":"timesheetReference1",
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
    "exportFileName":"Timesheet Pending Approval",
    "columnsFiltersToDestroy":[ 'timesheetStartDate','assignmentCreatedDate' ]
};