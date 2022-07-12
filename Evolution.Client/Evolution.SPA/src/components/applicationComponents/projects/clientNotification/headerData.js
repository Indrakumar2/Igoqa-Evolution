import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();
export const HeaderData = {
    "columnDefs": [
        {
            "headerCheckboxSelectionFilteredOnly": true,
            "checkboxSelection": true,
            "suppressFilter": true,
            "width": 40
        },
        {
            "headerName": localConstant.gridHeader.CUSTOMER_CONTRACT_NAME,
            "field": "customerContact",
            "filter": "agTextColumnFilter",
            "width":220,  
            "tooltipField":"customerContact",        
            "headerTooltip": localConstant.gridHeader.CUSTOMER_CONTRACT_NAME,
            
        },
        
        {
            "headerName": localConstant.gridHeader.FLASH_REPORTING_NOTIFICATION,
            "field": "isSendFlashReportingNotification",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.FLASH_REPORTING_NOTIFICATION,
            "width":230,
            "tooltip": (params) => {
                if(params.data.isSendFlashReportingNotification === true){
                    return "Yes";
                }else{
                  return "No";              }
                
              },
            "valueGetter": (params) => {
                if(params.data.isSendFlashReportingNotification === true){
                    return "Yes";
                }else{
                  return "No";              }
                
              },
        },
        {
            "headerName": localConstant.gridHeader.INSPECTION_RELEASE_NOTES,
            "field": "isSendInspectionReleaseNotesNotification",
            "filter": "agTextColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            }, 
            "headerTooltip": localConstant.gridHeader.INSPECTION_RELEASE_NOTES,
            "width":230,
            "tooltip":(params) => {
                if(params.data.isSendInspectionReleaseNotesNotification === true){
                    return "Yes";
                }else{
                  return "No";              }
                
              },
            "valueGetter": (params) => {
                if(params.data.isSendInspectionReleaseNotesNotification === true){
                    return "Yes";
                }else{
                  return "No";              }
                
              },
        },
        {
            "headerName": localConstant.gridHeader.CUSTOMER_REPORTING_NOTIFICATION,
            "field": "isSendCustomerReportingNotification",
            "filter": "agTextColumnFilter",
            "filterParams": {
                "inRangeInclusive":true
            },
            "width":280,
            "headerTooltip": localConstant.gridHeader.CUSTOMER_REPORTING_NOTIFICATION,  
            "tooltip":(params) => {
                if(params.data.isSendCustomerReportingNotification === true){
                    return "Yes";
                }else{
                  return "No";              }
                
              },
            "valueGetter": (params) => {
                if(params.data.isSendCustomerReportingNotification === true){
                    return "Yes";
                }else{
                  return "No";              }
                
              },
        },
        {
            "headerName": localConstant.gridHeader.CUSTOMER_DIRECT_REPORTING,
            "field": "isSendCustomerDirectReportingNotification",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.CUSTOMER_DIRECT_REPORTING,
            "width":230,  
            "tooltip":(params) => {
                if(params.data.isSendCustomerDirectReportingNotification === true){
                    return "Yes";
                }else{
                  return "No";              }
                
              },
            "valueGetter": (params) => {
                if(params.data.isSendCustomerDirectReportingNotification === true){
                    return "Yes";
                }else{
                  return "No";              }
                
              },
        },
        {
            "headerName": localConstant.gridHeader.NCR_Reporting,
            "field": "isSendNCRReportingNotification",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.NCR_Reporting,
            "width":150,
            "tooltip": (params) => {
                if(params.data.isSendNCRReportingNotification === true){
                    return "Yes";
                }else{
                  return "No";              }
                
              },
            "valueGetter": (params) => {
                if(params.data.isSendNCRReportingNotification === true){
                    return "Yes";
                }else{
                  return "No";              }
                
              },
        },
        {
            "headerName": "",
            "field": "EditColumn",
            "cellRenderer": "EditLink",
            "buttonAction":"",
            "cellRendererParams": {
            },
            "suppressFilter": true,
            "suppressSorting": true,
            "width": 50
        }
    ],
    "enableSelectAll":true,    
    "enableFilter": true,
    "enableSorting":true, 
    "pagination": false,    
    "gridHeight":54,    
    "rowSelection":"multiple",
    "clearFilter":true, 
    "exportFileName":"Client Notification"
};