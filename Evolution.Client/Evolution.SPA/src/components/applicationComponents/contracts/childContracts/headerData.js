import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData={
    "columnDefs":[
        {
            "headerName": localConstant.gridHeader.STATUS,
            "headerTooltip": localConstant.gridHeader.STATUS,
            "field": "contractStatus",
            "tooltip":  (params) => {
                if(params.data.contractStatus === 'O'){
                    return "Open";
                }else if(params.data.contractStatus === 'C'){
                  return "Closed";      
                        }
                
              },
            "filter": "agTextColumnFilter",
            "width":110,
            "valueGetter": (params) => {
                if(params.data.contractStatus === 'O'){
                    return "Open";
                }else if(params.data.contractStatus === 'C'){
                  return "Closed";      
                        }
                
              }
        },
        {
            "headerName":localConstant.gridHeader.CUSTOMER,
            "headerTooltip": localConstant.gridHeader.CUSTOMER,
            "field":"contractCustomerName",
            "tooltipField": "contractCustomerName",
            "filter":"agTextColumnFilter",
            "width":120
        },
        {
            "headerName":localConstant.gridHeader.CONTRACT_HOLDING_COMPANY,
            "headerTooltip": localConstant.gridHeader.CONTRACT_HOLDING_COMPANY,
            "field":"contractHoldingCompanyName",
            "tooltipField": "contractHoldingCompanyName",
            "filter":"agTextColumnFilter",
            "width":180
        },
        {
            "headerName":localConstant.gridHeader.CONTRACT_NUMBER,
            "headerTooltip": localConstant.gridHeader.CONTRACT_NUMBER,
            "field":"contractNumber",
            "tooltipField": "contractNumber",
            "filter":"agNumberColumnFilter",
            "cellRenderer": "contractAnchorRenderer",
            "width":150,
            "filterParams": {
                "inRangeInclusive":true
            },
        },
        {
            "headerName":localConstant.gridHeader.CUSTOMER_CONTRACT_NUMBER,
            "headerTooltip": localConstant.gridHeader.CUSTOMER_CONTRACT_NUMBER,
            "field":"customerContractNumber",
            "tooltipField": "customerContractNumber",
            "filter":"agNumberColumnFilter" ,
            "width": 150,
            "filterParams": {
                "inRangeInclusive": true
            },
        },
        {
            "headerName":localConstant.gridHeader.START_DATE,
            "headerTooltip": localConstant.gridHeader.START_DATE,
            "field":"contractStartDate",
            "tooltip": (params) => {
                return moment(params.data.contractStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filter":"agDateColumnFilter",
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "contractStartDate"
            },
            "width":150,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },
         {
            "headerName":localConstant.gridHeader.END_DATE,
            "headerTooltip": localConstant.gridHeader.END_DATE,
            "field":"contractEndDate",
            "tooltip": (params) => {
                return moment(params.data.contractEndDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        }, 
            "filter":"agDateColumnFilter",
            "cellRenderer": "DateComponent",
            "cellRendererParams": {
                "dataToRender": "contractEndDate"
            },
            "width":150,
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              }
        },
        {
            "headerName":localConstant.gridHeader.CONTRACT_STRUCTURE,
            "headerTooltip": localConstant.gridHeader.CONTRACT_STRUCTURE,
            "field":"contractType",
            "tooltip": (params) => {
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
            "filter":"agTextColumnFilter",
            "width":150,
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
              }
        },
        {
            "headerName":localConstant.gridHeader.CONTRACT_REFERENCE,
            "headerTooltip": localConstant.gridHeader.CONTRACT_REFERENCE,
            "field":"contractCRMReference",
            "tooltipField": "contractCRMReference",
            "filter":"agTextColumnFilter",
            "width":130
        }
    ],
    "enableFilter":true, 
    "enableSorting":true, 
    "pagination": true,
    "gridHeight":60,
    "searchable":true,
    "gridActions":true,
    "gridTitlePanel":true,
    "clearFilter":true, 
    "exportFileName":"Child Contrats"
};