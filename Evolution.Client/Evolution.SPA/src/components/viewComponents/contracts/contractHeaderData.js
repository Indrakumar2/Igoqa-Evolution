import dateUtil from '../../../utils/dateUtil';
import { getlocalizeData } from "../../../utils/commonUtils";
import moment from 'moment';
const localConstant = getlocalizeData();

export const ContractHeaderData = {
    "columnDefs": [
        {
            "headerName": localConstant.contractSearch.CONTRACT_NUMBER,
            "field": "contractNumber",
            "cellRenderer": "contractAnchorRenderer",
            "width": 150,
            "headerTooltip": localConstant.contractSearch.CONTRACT_NUMBER,
            "filter": "agTextColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "tooltipField":"contractNumber"
        },
        {
            "headerName": localConstant.contractSearch.CUSTOMER_NAME,
            "field": "contractCustomerName",
            "width": 180,
            "headerTooltip":localConstant.contractSearch.CUSTOMER_NAME,
            "tooltipField": "contractCustomerName"
        },
        {
            "headerName": localConstant.contractSearch.STATUS,
            "field": "contractStatus",
            "headerTooltip":localConstant.contractSearch.STATUS,
            "width": 115,
            "valueGetter": (params) => {
                if (params.data.contractStatus === 'O') {
                    return "Open";
                } else {
                    return "Closed";
                }
            },
            "tooltip": (params) => {
                if (params.data.contractStatus === 'O') {
                    return "Open";
                } else {
                    return "Closed";
                }
            },
            // "tooltipField":"contractStatus"
        },
        {
            "headerName": localConstant.contractSearch.CUSTOMER_CONTRACT_NUMBER,
            "field": "customerContractNumber",
            "width": 200,
            "tooltipField": "customerContractNumber",
            "headerTooltip": localConstant.contractSearch.CUSTOMER_CONTRACT_NUMBER,
            "filter": "agNumberColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            }
        },
        {
            "headerName": localConstant.contractSearch.COMPANY_NAME,
            "field": "contractHoldingCompanyName",
            "width": 200,
            "headerTooltip":localConstant.contractSearch.COMPANY_NAME,
            "tooltipField": "contractHoldingCompanyName",
        },
        {
            "headerName": localConstant.contractSearch.START_DATE,
            "field": "contractStartDate",
            "filter": "agDateColumnFilter",
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "contractStartDate"
            },
            "width": 150,
            "headerTooltip": localConstant.contractSearch.START_DATE,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
            },
            "tooltip": (params) => {
                return moment(params.data.contractStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                
            },

            //   "tooltipField":"contractStartDate"
    },
    
        {
            "headerName": localConstant.contractSearch.END_DATE,
            "field": "contractEndDate",            
            "filter": "agDateColumnFilter",
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "contractEndDate"
            },
            "width": 150,
            "headerTooltip": localConstant.contractSearch.END_DATE,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
            },
            "tooltip": (params) => {
                return moment(params.data.contractEndDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
               
            },

            // "tooltipField":"contractEndDate"  
        },
        {
            "headerName": localConstant.contractSearch.CONTRACT_STRUCTURE,
            "field": "contractType",
            "width": 190,
            "headerTooltip": "Contract Structure",
            // "tooltipField":"contractType",
            "valueGetter": (params) => {
                if (params.data.contractType === 'STD') {
                    return "Standard Contract";
                }
                else if (params.data.contractType === 'PAR') {
                    return "Parent Contract";
                }
                else if (params.data.contractType === 'CHD') {
                    return "Child Contract";
                }
                else if (params.data.contractType === 'FRW') {
                    return "Framework Contract";
                }
                else if (params.data.contractType === 'IRF') {
                    return "Related Framework Contract";
                }
            },
            "tooltip":(params) => {
                if (params.data.contractType === 'STD') {
                    return "Standard Contract";
                }
                else if (params.data.contractType === 'PAR') {
                    return "Parent Contract";
                }
                else if (params.data.contractType === 'CHD') {
                    return "Child Contract";
                }
                else if (params.data.contractType === 'FRW') {
                    return "Framework Contract";
                }
                else if (params.data.contractType === 'IRF') {
                    return "Related Framework Contract";
                }
            },
        },
        {
            "headerName": localConstant.contractSearch.CRM_OPP_REF,
            "field": "contractCRMReference",
            "width": 150,
            "headerTooltip": localConstant.contractSearch.CRM_OPP_REF,
            "tooltipField":"contractCRMReference"
        }
    ],
    "pagination": true,
    "enableFilter": true,
    "enableSorting": true,
    "gridHeight": 60,
    "searchable": true,
    "gridActions": true,
    "gridTitlePanel": true,
    "clearFilter":true, 
    "exportFileName":"Contract Search Data",
    "columnsFiltersToDestroy":[ 'contractStartDate','contractEndDate' ]
};