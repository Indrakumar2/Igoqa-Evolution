import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();
export const wonLostPdfHeader= {
    WonLostHeader:[
       {
           "headerName": localConstant.gridHeader.SEARCH_ID,
            "field": "id",
         },
         {
          "headerName": localConstant.gridHeader.SEARCH_TYPE,
           "field": "searchType",
        },
        {
          "headerName": localConstant.gridHeader.CREATED_ON,
           "field": "createdOn",
        },

         {
           "headerName": localConstant.gridHeader.ACTION, 
           "field": "action",
         },
         
         {
          "headerName": localConstant.gridHeader.COMMENTS, 
          "field": "description",
        },
         {
           "headerName": localConstant.gridHeader.CUSTOMER_NAME,
           "field": "customerName",
         } ,
         {
          "headerName": localConstant.gridHeader.SUPPLIER_NAME, 
          "field": "supplier",

        } ,
        {
          "headerName": localConstant.gridHeader.SUPPLIER_LOCATION, 
          "field": "supplierLocation",
        } ,

        {
          "headerName": localConstant.gridHeader.CATEGORY, 
          "field": "categoryName",
        } ,
        {
          "headerName": localConstant.gridHeader.SUB_CATEGORY, 
          "field": "subCategoryName",
        } ,
        {
          "headerName": localConstant.gridHeader.SERVICE, 
          "field": "serviceName",
        } ,

         {
           "headerName":localConstant.gridHeader.CONTRACT_HOLDER_COMPANY, 
           "field": "chCompanyName",

         } ,
         {
           "headerName":  localConstant.gridHeader.CH_COORDINATOR_NAME,
           "field": "chCoordinatorLogOnName",

         }  ,
         {
           "headerName":  localConstant.gridHeader.OPERATING_COMPANY, 
           "field": "opCompanyName",
         }  ,
         {
           "headerName":localConstant.gridHeader.OC_COORDINATOR_NAME, 
           "field": "opCoordinatorLogOnName",
         }  ,
    ]   
};