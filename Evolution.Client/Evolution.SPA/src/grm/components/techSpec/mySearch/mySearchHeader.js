import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();

const resourceDatas = (data) => {
    if(data && data.length > 0){
        const techSpecArray = [];
        data.forEach(iteratedValue => {
            const techSpecName = `${ iteratedValue.lastName }, ${ iteratedValue.firstName } (${ iteratedValue.epin })`;
            techSpecArray.push(techSpecName);
        });
        return techSpecArray.join();
    }
    return "N/A";
};

export const HeaderData = {
    "columnDefs": [
       
        {
            "headerName": localConstant.gridHeader.SEARCH_ID,
            "headerTooltip": localConstant.gridHeader.SEARCH_ID,
            "field": "id",
            
            "cellRenderer": "mySearchRenderer",
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "width":180,
            "tooltipField":"id"
        },
        {
            "headerName": localConstant.gridHeader.SEARCH_TYPE,
            "headerTooltip": localConstant.gridHeader.SEARCH_TYPE,
            "field": "searchType",
            "filter": "agTextColumnFilter",
            "width":220,
            "valueGetter": (params) => {
                if(params.data.searchType === "Quick"){
                    return "Quick Search";
                }
                else if(params.data.searchType === "PreAssignment"){
                    return "Pre Assignment";
                }
            },
            "tooltip": (params) => {
                if(params.data.searchType === "Quick"){
                    return "Quick Search";
                }
                else if(params.data.searchType === "PreAssignment"){
                    return "Pre Assignment";
                }
            },
        },
        {
            "headerName": localConstant.gridHeader.STATUS,
            "headerTooltip": localConstant.gridHeader.STATUS,
            "field": "searchAction",
            "filter": "agTextColumnFilter",
            "valueGetter": (params) => {
                if (params.data.searchAction === "SD") {
                    return "Search Disposition";
                } else if(params.data.searchAction === "L") {
                    return "Lost";
                } else if(params.data.searchAction === "W") {
                    return "Won";
                } else if(params.data.searchAction === "CUP" || params.data.searchAction === "SS") {
                    return "New";
                } else  {
                    return "N/A";
                }
            },
            "width":180,
            "tooltip":(params) => {
                if (params.data.searchAction === "SD") {
                    return "Search Disposition";
                } else if(params.data.searchAction === "L") {
                    return "Lost";
                } else if(params.data.searchAction === "W") {
                    return "Won";
                } else if(params.data.searchAction === "CUP" || params.data.searchAction === "SS") {
                    return "New";
                } else  {
                    return "N/A";
                }
            },
        },
        {
            "headerName": localConstant.gridHeader.CATEGORY,
            "headerTooltip": localConstant.gridHeader.CATEGORY,
            "field": "searchParameter.categoryName",
            "filter": "agTextColumnFilter",
            "width":220,
            "tooltipField":"searchParameter.categoryName"
        },
        {
            "headerName": localConstant.gridHeader.SUB_CATEGORY,
            "headerTooltip": localConstant.gridHeader.SUB_CATEGORY,
            "field": "searchParameter.subCategoryName",
            "filter": "agTextColumnFilter",
            "width":220,
            "tooltipField": "searchParameter.subCategoryName",
        },
        {
            "headerName": localConstant.gridHeader.SERVICE,
            "headerTooltip": localConstant.gridHeader.SERVICE,
            "field": "searchParameter.serviceName",
            "filter": "agTextColumnFilter",
            "width":220,
            "tooltipField":"searchParameter.serviceName",
        },
        {
            "headerName": localConstant.gridHeader.PROJECT_NAME,
            "headerTooltip": localConstant.gridHeader.PROJECT_NAME,
            "field": "searchParameter.projectName",
            "filter": "agTextColumnFilter",
            "width":220,
            "tooltipField":"searchParameter.projectName"
        },
        {
            "headerName": localConstant.gridHeader.CUSTOMER_NAME,
            "headerTooltip": localConstant.gridHeader.CUSTOMER_NAME,
            "field": "searchParameter.customerName",
            "filter": "agTextColumnFilter",
            "width":220,
            "tooltipField": "searchParameter.customerName",
        },
        {
            "headerName": localConstant.gridHeader.CONTRACT_HOLDING_COMPANY,
            "headerTooltip": localConstant.gridHeader.CONTRACT_HOLDING_COMPANY,
            "field": "searchParameter.chCompanyName",
            "filter": "agTextColumnFilter",
            "width":220,
            "tooltipField": "searchParameter.chCompanyName"
        },
        {
            "headerName": localConstant.gridHeader.CH_COORDINATOR,
            "headerTooltip": localConstant.gridHeader.CH_COORDINATOR,
            "field": "searchParameter.chCoordinatorLogOnName",
            "filter": "agTextColumnFilter",
            "width":220,
            "tooltipField":"searchParameter.chCoordinatorLogOnName"
        },
        {
            "headerName": localConstant.gridHeader.OPERATING_COMPANY,
            "headerTooltip": localConstant.gridHeader.OPERATING_COMPANY,
            "field": "searchParameter.opCompanyName",
            "filter": "agTextColumnFilter",
            "width":220,
            "tooltipField":"searchParameter.opCompanyName",
        },
        {
            "headerName": localConstant.gridHeader.OC_COORDINATOR,
            "headerTooltip": localConstant.gridHeader.OC_COORDINATOR,
            "field": "searchParameter.opCoordinatorLogOnName",
            "filter": "agTextColumnFilter",
            "width":220,
            "tooltipField":"searchParameter.opCoordinatorLogOnName"
        },
        {
            "headerName": localConstant.gridHeader.SUPPLIER,
            "headerTooltip": localConstant.gridHeader.SUPPLIER,
            "field": "searchParameter.supplier",
            "filter": "agTextColumnFilter",
            "width":220,
            "tooltipField":"searchParameter.supplier",
        },
        {
            "headerName": localConstant.gridHeader.SUPPLIER_LOCATION,
            "headerTooltip": localConstant.gridHeader.SUPPLIER_LOCATION,
            "field": "searchParameter.supplierLocation",
            "filter": "agTextColumnFilter",
            "width":220,
            "tooltipField":"searchParameter.supplierLocation"
        },
        {
            "headerName": localConstant.gridHeader.MATERIALS,
            "headerTooltip": localConstant.gridHeader.MATERIALS,
            "field": "searchParameter.materialDescription",
            "filter": "agTextColumnFilter",
            "width":220,
            "tooltipField":"searchParameter.materialDescription"
        },
        {
            "headerName": localConstant.gridHeader.RESOURCES_SCHEDULED,
            "headerTooltip": localConstant.gridHeader.RESOURCES_SCHEDULED,
            "field": "searchParameter.selectedTechSpecInfo",
            "filter": "agTextColumnFilter",
            "tooltip":(params)=>{
                return resourceDatas(params.data.searchParameter.selectedTechSpecInfo);
            },
            "valueGetter":(params)=>{
                return resourceDatas(params.data.searchParameter.selectedTechSpecInfo);
            },
            "width":220
        },
        {
            "headerName": localConstant.gridHeader.CREATED_BY,
            "headerTooltip": localConstant.gridHeader.CREATED_BY,
            "field": "createdBy",
            "filter": "agTextColumnFilter",
            "width":220,
            "tooltipField": "createdBy",
        },
        {
            "headerName": localConstant.gridHeader.CREATION_DATE,
            "headerTooltip": localConstant.gridHeader.CREATION_DATE,
            "field": "createdOn",
            "filter": "agDateColumnFilter",           
            "width":180,
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "createdOn"
            },
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true,
            },
            "tooltip": (params) => {
                return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
               
            },  
        },
        // {
        //     "headerName": localConstant.gridHeader.ASSIGNMENT_TYPE,
        //     "headerTooltip": localConstant.gridHeader.ASSIGNMENT_TYPE,
        //     "field": "searchParameter.assignmentType",
        //     "filter": "agTextColumnFilter",
        //     "valueGetter": (params) => {
        //         if(params.data.searchParameter){
        //             if (params.data.searchParameter.assignmentType === "A") {
        //                 return "Ad Hoc";
        //             } else if(params.data.searchParameter.assignmentType === "R") {
        //                 return "Resident";
        //             } else  {
        //                 return "N/A";
        //             }
        //         } else {
        //             return "N/A";
        //         }
        //     },
        //     "width":220,
        //     "tooltip":(params) => {
        //         if(params.data.searchParameter){
        //             if (params.data.searchParameter.assignmentType === "A") {
        //                 return "Ad Hoc";
        //             } else if(params.data.searchParameter.assignmentType === "R") {
        //                 return "Resident";
        //             } else  {
        //                 return "N/A";
        //             }
        //         } else {
        //             return "N/A";
        //         }
        //     },
        // },
    ],
    "rowSelection": 'single',   
    "pagination": false,
    "enableFilter": true,
    "enableSorting": true,
    "gridHeight": 60,
    "searchable": true,
   // "gridActions": true,
    "gridTitlePanel": true,
    "clearFilter":true, 
    "exportFileName":"My Searches",
    "columnsFiltersToDestroy":[ 'createdOn' ]
};