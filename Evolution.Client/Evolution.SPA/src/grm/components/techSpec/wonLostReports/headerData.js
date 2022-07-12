import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();

const resourceDatas = (data) => {
  if(data && data.length > 0){
      const techSpecArray = [];
      data.forEach(iteratedValue => {
          const techSpecName = `${ iteratedValue.lastName }${ "," }${ iteratedValue.firstName }${ "(" }${ iteratedValue.epin }${ ")" }`;
          techSpecArray.push(techSpecName);
      });
      return techSpecArray.join();
  }
  return "N/A";
};
export const HeaderData = (properties) => {
    return {
   
            "columnDefs": [
        
                 {
                    "headerName": localConstant.gridHeader.SEARCH_ID,
                     "field": "id",
                     "filter": "agNumberColumnFilter",
                     "width": 200,
                     "headerTooltip": localConstant.gridHeader.SEARCH_ID,
                     "tooltipField": "id",
                     "sort": "asc",
                    
                  },
                  
                 {
                  "headerName": localConstant.gridHeader.SEARCH_TYPE,
                   "field": "searchType",
                   "filter": "agTextColumnFilter",
                   "width": 200,
                   "headerTooltip": localConstant.gridHeader.SEARCH_TYPE,
                   "tooltipField": "id",
                  
                },
                  {
                    "headerName": localConstant.gridHeader.CREATED_ON,
                     "field": "createdOn",
                     "tooltip": (params) => {
                      return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                    
                     }, 
                   "filter": "agDateColumnFilter",
                   "cellRenderer": "DateComponent",
                  "cellRendererParams": {
                      "dataToRender": "createdOn"
                  },

                     "filterParams": {
                         "comparator": dateUtil.comparator,
                         "inRangeInclusive": true,
                         "browserDatePicker": true
                       },
                     "sort": "desc",
                     "width": 200,
                     "headerTooltip": localConstant.gridHeader.CREATED_ON,
                    
                  },

                  {
                    "headerName": localConstant.gridHeader.ACTION, 
                    "field": "action",
                    "filter": "agTextColumnFilter",
                    "headerTooltip": localConstant.gridHeader.ACTION, 
                    "width": 200,
                    "valueGetter": (params) => {
                      if(params.data.action === "L") {
                          return "Lost";
                      } else if(params.data.action === "W") {
                          return "Won";
                      }
                      else{
                        return params.data.action;
                      }
                  },
              
                  "tooltip":(params) => {
                      if(params.data.action === "L") {
                          return "Lost";
                      } else if(params.data.action === "W") {
                          return "Won";
                      }  else  {
                          return params.data.action;
                      }
                  },
                  },
                  {
                    "headerName": localConstant.gridHeader.DISPOSITION_TYPE, 
                    "field": "dispositionType",
                    "filter": "agTextColumnFilter",
                    "headerTooltip": localConstant.gridHeader.DISPOSITION_TYPE, 
                    "width": 200,
                    "valueGetter": (params) => {
                      if(params.data.dispositionType === "LAA") {
                          return "Lost – Another Agency";
                      } else if(params.data.dispositionType === "LCF") {
                          return "Lost – Client Filled";
                      }
                      else if(params.data.dispositionType === "LCR") {
                        return "Lost – Client Rejected";
                      } else if(params.data.dispositionType === "RNA") {
                        return "Resource Not Available";
                      } else if(params.data.dispositionType === "DNMCC") {
                        return "Did Not Match Client Criteria";
                      }
                      else if(params.data.dispositionType === "D/L") {
                        return "Distance/Location";
                      } else if(params.data.dispositionType === "OTH") {
                        return "Other";
                      }
                      else{
                        return params.data.dispositionType;
                      }
                  },
              
                  "tooltip":(params) => {
                      if(params.data.dispositionType === "L") {
                          return "Lost";
                      } else if(params.data.dispositionType === "W") {
                          return "Won";
                      }  else  {
                          return params.data.dispositionType;
                      }
                  },
                  },     
                  {
                    "headerName": localConstant.gridHeader.COMMENTS, 
                    "headerTooltip": localConstant.gridHeader.COMMENTS, 
                    "field": "description",
                    "filter": "agTextColumnFilter",
                    "tooltipField": "description",
                    "width": 200,
                  } ,             
                  {
                    "headerName": localConstant.gridHeader.CUSTOMER_NAME, 
                    "headerTooltip": localConstant.gridHeader.CUSTOMER_NAME, 
                    "field": "searchParameter.customerName",
                    "filter": "agTextColumnFilter",
                    "tooltipField": "searchParameter.customerName",
                    "width": 200,
                  } ,
                  {
                    "headerName": localConstant.gridHeader.SUPPLIER_NAME, 
                    "headerTooltip": localConstant.gridHeader.SUPPLIER_NAME, 
                    "field": "searchParameter.supplier",
                    "filter": "agTextColumnFilter",
                    "tooltipField":"searchParameter.supplier",
                    "width": 200,
                  } ,
                  {
                    "headerName": localConstant.gridHeader.SUPPLIER_LOCATION, 
                    "headerTooltip":localConstant.gridHeader.SUPPLIER_LOCATION,
                    "field": "searchParameter.supplierLocation",
                    "filter": "agTextColumnFilter",
                    "tooltipField":"searchParameter.supplierLocation",
                    "width": 200,
                  } ,
                  {
                    "headerName": localConstant.gridHeader.CATEGORY, 
                    "headerTooltip": localConstant.gridHeader.CATEGORY, 
                    "field": "categoryName",
                    "filter": "agTextColumnFilter",
                    "tooltipField":"categoryName",
                    "width": 200,
                  }  ,
                  {
                    "headerName": localConstant.gridHeader.SUB_CATEGORY, 
                    "headerTooltip": localConstant.gridHeader.SUB_CATEGORY, 
                    "field": "subCategoryName",
                    "filter": "agTextColumnFilter",
                    "tooltipField":"subCategoryName",
                    "width": 200,
                  }  ,
                  {
                    "headerName": localConstant.gridHeader.SERVICE, 
                    "headerTooltip": localConstant.gridHeader.SERVICE, 
                    "field": "serviceName",
                    "filter": "agTextColumnFilter",
                    "tooltipField":"serviceName",
                    "width": 200,
                  }  ,
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
                    "headerName": localConstant.gridHeader.CONTRACT_HOLDER_COMPANY, 
                    "headerTooltip": localConstant.gridHeader.CONTRACT_HOLDER_COMPANY, 
                    "field": "searchParameter.chCompanyName",
                    "filter": "agTextColumnFilter",
                    "tooltipField":"searchParameter.chCompanyName",
                    "width": 200,
                  }  ,
                  {
                    "headerName": localConstant.gridHeader.CH_COORDINATOR_NAME, 
                    "headerTooltip":localConstant.gridHeader.CH_COORDINATOR_NAME, 
                    "field": "searchParameter.chCoordinatorLogOnName",
                    "filter": "agTextColumnFilter",
                    "tooltipField":"searchParameter.chCoordinatorLogOnName",
                    "width": 200,
                  }  ,
                  {
                    "headerName": localConstant.gridHeader.OPERATING_COMPANY, 
                    "headerTooltip":localConstant.gridHeader.OPERATING_COMPANY,
                    "field": "searchParameter.opCompanyName",
                    "filter": "agTextColumnFilter",
                    "tooltipField": "searchParameter.opCompanyName",
                    "width": 200,
                  }  ,
                  {
                    "headerName": localConstant.gridHeader.OC_COORDINATOR_NAME,
                    "headerTooltip": localConstant.gridHeader.OC_COORDINATOR_NAME,
                    "field": "searchParameter.opCoordinatorLogOnName",
                    "filter": "agTextColumnFilter",
                    "tooltipField":"searchParameter.opCoordinatorLogOnName",
                    "width": 200,
                  }  ,
                  
            ],
          
            "enableFilter": true,
            "enableSorting": true,
            "pagination": true,
            "gridTitlePanel": true,
            "clearFilter":true, 
            "gridActions": true,
            "exportFileName":"Won Lost Report",
            "customHeader":properties?properties.customHeader:"Won Lost Report",
            "gridHeight": 25,
        
        };
    };
