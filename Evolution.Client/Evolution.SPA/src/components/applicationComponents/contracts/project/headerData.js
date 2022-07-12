import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData = {
    "columnDefs": [
        {
            "headerName": localConstant.gridHeader.CUSTOMER_PROJECT_NO,
            "headerTooltip": localConstant.gridHeader.CUSTOMER_PROJECT_NO,
            "field": "customerProjectNumber",
            "tooltipField": "customerProjectNumber",
            "filter": "agNumberColumnFilter",
            "width":200,
            "filterParams": {
                "inRangeInclusive":true
            },
        },
        {
            "headerName": localConstant.gridHeader.CUSTOMER_PROJECT_NAME,
            "headerTooltip": localConstant.gridHeader.CUSTOMER_PROJECT_NAME,
            "field": "customerProjectName",
            "tooltipField": "customerProjectName",
            "filter": "agTextColumnFilter",
            "width":230
        },
        {
            "headerName": localConstant.gridHeader.CONTRACT_NUMBER,
            "headerTooltip": localConstant.gridHeader.CONTRACT_NUMBER,
            "field": "contractNumber",
            "tooltipField": "contractNumber",
            "filter": "agNumberColumnFilter",
            "width":160,
            "filterParams": {
                "inRangeInclusive":true
            },
        },
        {
            "headerName": localConstant.gridHeader.PROJECT_NO,
            "headerTooltip": localConstant.gridHeader.PROJECT_NO,
            "field": "projectNumber",
            "tooltip": (params) => (params.data.projectNumber),
            "filter": "agNumberColumnFilter",
            "cellRenderer": "projectAnchorRenderer",
            "valueGetter": (params) => (params.data.projectNumber),
            "width":140,
            "filterParams": {
                "inRangeInclusive":true
            },
        },
        {
            "headerName": localConstant.gridHeader.CUSTOMER_NAME,
            "headerTooltip": localConstant.gridHeader.CUSTOMER_NAME,
            "field": "contractCustomerName",
            "tooltipField": "contractCustomerName",
            "filter": "agTextColumnFilter",
            "width":170
        },
        {
            "headerName": localConstant.gridHeader.PROJECT_STATUS,
            "headerTooltip": localConstant.gridHeader.PROJECT_STATUS,
            "field": "projectStatus",
            "tooltip": (params) => {
                if(params.data.projectStatus === 'O'){
                    return "Open";
                }else if(params.data.projectStatus === 'C'){
                  return "Closed";      
                        }
                
              },
            "filter": "agTextColumnFilter",
            "width":170,
            "valueGetter": (params) => {
                if(params.data.projectStatus === 'O'){
                    return "Open";
                }else if(params.data.projectStatus === 'C'){
                  return "Closed";      
                        }
                
              }
        },
        {
            "headerName": localConstant.gridHeader.START_DATE,
            "headerTooltip": localConstant.gridHeader.START_DATE,
            "field": "projectStartDate",
            "tooltip": (params) => {
                return moment(params.data.projectStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filter": "agDateColumnFilter",
            "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "dataToRender": "projectStartDate"
                },
            "width":150,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },
        {
            "headerName": localConstant.gridHeader.END_DATE,
            "headerTooltip": localConstant.gridHeader.END_DATE,
            "field": "projectEndDate",
            "tooltip": (params) => {
                return moment(params.data.projectEndDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filter": "agDateColumnFilter",
            "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "dataToRender": "projectEndDate"
                },
            "width":150,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },
        {
            "headerName": localConstant.gridHeader.DIVISION,
            "headerTooltip": localConstant.gridHeader.DIVISION,
            "field": "companyDivision",
            "tooltipField": "companyDivision",
            "filter": "agTextColumnFilter",
            "width":130
        },
        {
            "headerName": localConstant.gridHeader.OFFICE,
            "headerTooltip": localConstant.gridHeader.OFFICE,
            "field": "companyOffice",
            "tooltipField": "companyOffice",
            "filter": "agTextColumnFilter",
            "width":130
        },
        {
            "headerName": localConstant.gridHeader.EREP,
            "headerTooltip": localConstant.gridHeader.EREP,
            "field": "isEReportProjectMapped",
            "tooltip": (params) => {
                if (params.data.isEReportProjectMapped === true) {
                    return "Yes";
                } else if (params.data.isEReportProjectMapped === false) {
                    return "No";
                }
            },
            "filter": "agTextColumnFilter",
            "width":130,
            "valueGetter": (params) => {
                if (params.data.isEReportProjectMapped === true) {
                    return "Yes";
                } else if (params.data.isEReportProjectMapped === false) {
                    return "No";
                }
            }
        }
    ],
    "defaultColDef": {
        "width": 130
    },
        "enableFilter":true, 
        "enableSorting":true, 
        "pagination": true,
        "gridHeight":55,
        "searchable":true,
        "gridActions":true,
        "gridTitlePanel":true,
        "clearFilter":true, 
        "gridRefresh":true,
        "exportFileName":"Projects",
        "columnsFiltersToDestroy":[ 'projectStartDate','projectEndDate' ]
};