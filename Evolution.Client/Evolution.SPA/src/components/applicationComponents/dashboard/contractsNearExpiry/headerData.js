import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const headerData = {
    "columnDefs": [
        {
            "headerName":  localConstant.gridHeader.CONTRACT_NO,
            "field": "contractNumber",
            "filter": "agTextColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },             
            "cellRenderer": "contractAnchorRenderer",
            // "cellRendererParams":
            // {
            //     "dataToRender":"contractNumber"
            // },
            "width":250,
            "headerTooltip": localConstant.gridHeader.CONTRACT_NO,
            "tooltipField":"contractNumber",
        },
        {
            "headerName": localConstant.gridHeader.CUSTOMER_NAME,
            "field": "contractCustomerName",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.CUSTOMER_NAME,
            "width":380,
            "tooltipField":"contractCustomerName",
        },
        {
            "headerName": localConstant.gridHeader.CUSTOMER_CONTRACT_NUMBER,
            "field": "customerContractNumber",
            "filter": "agTextColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "headerTooltip": localConstant.gridHeader.CUSTOMER_CONTRACT_NUMBER,
            "width":260,
            "tooltipField":"customerContractNumber",
        },
        {
            "headerName": localConstant.gridHeader.START_DATE,
            "field": "contractStartDate",
            "filter": "agDateColumnFilter",           
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "contractStartDate"
            },
            "width":200,
            "tooltip": (params) => {
                return moment(params.data.contractStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "headerTooltip": localConstant.gridHeader.START_DATE,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },
        {
            "headerName": localConstant.gridHeader.END_DATE,
            "field": "contractEndDate",
            "filter": "agDateColumnFilter" ,          
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "contractEndDate"
            },
            "width":200,
            "tooltip": (params) => {
                return moment(params.data.contractEndDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "headerTooltip": localConstant.gridHeader.END_DATE,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true, 
            }
        },
        {
            "headerName": localConstant.gridHeader.CRM_REF,
            "field": "contractCRMReference",
            "filter": "agTextColumnFilter",
            "width":120,
            "tooltipField":"contractCRMReference",
            "headerTooltip": localConstant.gridHeader.CRM_REF,
        },
        {
            "headerName": localConstant.gridHeader.CONTRACT_STRUCTURE,
            "field": "contractStructure",
            "filter": "agTextColumnFilter",
            "width":180,
            "tooltip":(params) => {
                if (params.data.contractType === 'STD')
                    return "Standard";
                else if (params.data.contractType === 'PAR')
                    return "Parent";
                else if (params.data.contractType === 'FRW')
                    return 'Framework';
                else if (params.data.contractType === 'CHD')
                    return 'Child';
                else if (params.data.contractType === 'IRF')
                    return 'Related Framework'; 
              },
            "valueGetter": (params) => {
                if (params.data.contractType === 'STD')
                    return "Standard";
                else if (params.data.contractType === 'PAR')
                    return "Parent";
                else if (params.data.contractType === 'FRW')
                    return 'Framework';
                else if (params.data.contractType === 'CHD')
                    return 'Child';
                else if (params.data.contractType === 'IRF')
                    return 'Related Framework'; 
              },
              "headerTooltip": localConstant.gridHeader.CONTRACT_STRUCTURE,
        },
        // {
        //     "headerName":localConstant.gridHeader.CONTRACT_NEAR_EXPIRY,
        //     "field": "contractFutureDays",
        //     "filter": "agTextColumnFilter",
        //     "width":210,
        //     "tooltipField":"contractFutureDays",
        //     "headerTooltip": localConstant.gridHeader.CONTRACT_NEAR_EXPIRY,
        // }
    ],
    "enableFilter":true, 
    "enableSorting":true, 
    "pagination": true,
    "searchable":true,
    "gridActions":true,
    "gridTitlePanel":true,
    "gridHeight":59,
    "clearFilter":true, 
    "exportFileName":"Contracts Near Expiry",
    "columnsFiltersToDestroy":[ 'contractStartDate' ,'contractEndDate' ]
};