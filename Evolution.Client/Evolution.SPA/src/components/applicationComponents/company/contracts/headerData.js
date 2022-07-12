import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData={
        "columnDefs":[
            {
                "headerName": localConstant.gridHeader.STATUS,
                "field": "contractStatus",
                "filter": "agTextColumnFilter",
                "headerTooltip": localConstant.gridHeader.STATUS,
                "width":110,
                "tooltip":  (params) => {
                    if(params.data.contractStatus === 'O'){
                        return "Open";
                    }else if(params.data.contractStatus === 'C'){
                      return "Closed";      
                            }
                    
                  },
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
                "filter":"agTextColumnFilter",
                "tooltipField": "contractCustomerName",
                "width":120               
            },
            {
                "headerName":localConstant.gridHeader.CONTRACT_NUMBER,
                "headerTooltip": localConstant.gridHeader.CONTRACT_NUMBER,
                "field":"contractNumber",
                "filter":"agNumberColumnFilter",
                "cellRenderer": "contractAnchorRenderer",
                "tooltipField": "contractNumber",
                "width":150,
                "filterParams": {
                    "inRangeInclusive":true
                },
            },
            {
                "headerName":localConstant.gridHeader.CUSTOMER_CONTRACT_NUMBER,
                "headerTooltip": localConstant.gridHeader.CUSTOMER_CONTRACT_NUMBER,
                "field":"customerContractNumber",
                "filter":"agNumberColumnFilter" ,
                "tooltipField": "customerContractNumber",
                "width":200,
                "filterParams": {
                    "inRangeInclusive": true
                },
            },
            {
                "headerName":localConstant.gridHeader.START_DATE,
                "headerTooltip": localConstant.gridHeader.START_DATE,
                "field":"contractStartDate",
                "filter":"agDateColumnFilter",
                "tooltip": (params) => {
                    return params.data.contractStartDate && moment(params.data.contractStartDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
            }, 
                "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "dataToRender": "contractStartDate"
                },
                "filterParams": {
                    "comparator": dateUtil.comparator,
                    "inRangeInclusive": true,
                    "browserDatePicker": true
                  },
                "width":150
            },
             {
                "headerName":localConstant.gridHeader.END_DATE,
                "headerTooltip": localConstant.gridHeader.END_DATE,
                "field":"contractEndDate",
                "filter":"agDateColumnFilter",
                "tooltip": (params) => {
                    return params.data.contractEndDate && moment(params.data.contractEndDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "dataToRender": "contractEndDate"
                },
                "filterParams": {
                    "comparator": dateUtil.comparator,
                    "inRangeInclusive": true,
                    "browserDatePicker": true
                  },
                "width":150
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
                "width":180,
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
                "filter":"agTextColumnFilter",
                "tooltipField": "contractCRMReference",
                "width":180
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
        "gridRefresh":true,
        "exportFileName":"Contracts",
        "columnsFiltersToDestroy":[ 'contractStartDate','contractEndDate' ]
   };