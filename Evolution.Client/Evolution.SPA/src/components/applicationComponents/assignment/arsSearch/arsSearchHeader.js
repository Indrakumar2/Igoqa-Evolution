import { getlocalizeData } from '../../../../utils/commonUtils';
import { required } from '../../../../utils/validator';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';

const localConstant = getlocalizeData();
export const HeaderData = (functionRefs) => {
    return {
        "searchResourceHeader": {
            "columnDefs": [  
                {
                    "headerName":localConstant.resourceSearch.LOCATION,
                    "headerTooltip":localConstant.resourceSearch.LOCATION,
                    "field": "location",
                    "tooltipField":"location",   
                    "cellRenderer": 'agGroupCellRenderer',
                    "width": 150,
                    "cellClass": 'gridTextNoWrapEllipsis',
                },
                {
                    "headerCheckboxSelectionFilteredOnly": true,
                    "checkboxSelection": (params) => {
                        if(!required(params.node.data.location)){
                            return false;
                        }
                        else{
                            return true;
                        }
                    },            
                    "suppressFilter": true,
                    "cellRenderer": (params) => {
                        if(params.node.data.isSelected){
                            params.node.setSelected(params.node.data.isSelected);
                        }
                    },            

                    "width": 40
                },
                {
                    "field": "supplierLocationId",
                    "hide":true,
                    "width": 120
                },
                {
                    "headerName": localConstant.resourceSearch.EPIN,
                    "headerTooltip": localConstant.resourceSearch.EPIN,
                    "field": "epin",  
                    "tooltipField":"epin",   
                    "filter": "agTextColumnFilter",
                    "cellRenderer": "HyperLinkRenderer",
                    "cellRendererParams": {
                        "getInterCompanyCode": () => functionRefs.getInterCompanyInfo(),
                        "searchType":"ARS", 
                    },
                    "width":100,
                          
                },
                {
                    "headerName": localConstant.resourceSearch.LASTNAME,
                    "headerTooltip": localConstant.resourceSearch.LASTNAME,
                    "field": "lastName",  
                    "tooltipField":"lastName",   
                    "filter": "agTextColumnFilter",
                    "cellRendererParams": {
                        "searchType":"ARS"
                    },

                    "width":120
                },     
                {
                    "headerName": localConstant.resourceSearch.FIRSTNAME,
                    "headerTooltip":localConstant.resourceSearch.FIRSTNAME,
                    "field": "firstName",
                    "tooltipField":"firstName",   
                    "filter": "agTextColumnFilter",
        
                    "width":120
                },        
                {
                    "headerName": localConstant.resourceSearch.EMPLOYMENT_TYPE,
                    "headerTooltip":localConstant.resourceSearch.EMPLOYMENT_TYPE,
                    "field": "employmentType",
                    "tooltipField":"employmentType",   
                    "filter": "agTextColumnFilter",
                    "width":120
                },
                {
                    "headerName": localConstant.resourceSearch.SCHEDULE_STATUS,
                    "headerTooltip": localConstant.resourceSearch.SCHEDULE_STATUS,
                    "field": "scheduleStatus",
                    "tooltipField":"scheduleStatus",   
                    "filter": "agTextColumnFilter",
                    "cellStyle": (params) =>{
                        if (params.value==='TBA') { 
                            return {  backgroundColor: '#d19917' };
                        }
                        else if (params.value==='PTO') { 
                            return {  backgroundColor: '#78a3e8' };
                        }
                        else if (params.value==='Available') { 
                            return { backgroundColor: '#70c19a' };
                        }
                        else if (params.value==='Confirmed') { 
                            return { backgroundColor: '#cc4141' };
                        }
                        else if (params.value==='Tentative') { 
                            return { backgroundColor: '#d3c954' };
                        }
                        else if (params.value === 'Pre-Assignment') {
                            return { backgroundColor: '#474e54' , color: '#ffffff' };
                        }
                        else {
                            return null;
                        }
                    },
                    "width":200
                },
                {
                    "headerName": localConstant.resourceSearch.DISTANCE_FROM_VENDOR_MILES,
                    "headerTooltip":localConstant.resourceSearch.DISTANCE_FROM_VENDOR_MILES,
                    "field": "distanceFromVenderInMile",
                    "tooltipField":"distanceFromVenderInMile",   
                    "filter": "agNumberColumnFilter",
                    "width":120,
                    "valueGetter": (params) => {
                        if(params.data.distanceFromVenderInMile === -1 ){
                            return null;
                        }
                        return params.data.distanceFromVenderInMile;
                    }
                },
                {
                    "headerName": localConstant.resourceSearch.DISTANCE_FROM_VENDOR_KM,
                    "headerTooltip":localConstant.resourceSearch.DISTANCE_FROM_VENDOR_KM,
                    "field": "distanceFromVenderInKm",
                    "tooltipField":"distanceFromVenderInKm",   
                    "filter": "agNumberColumnFilter",
                    "width":120
                },
                {
                    "headerName":localConstant.resourceSearch.SUPPLIER_,
                    "headerTooltip":localConstant.resourceSearch.SUPPLIER_,
                    "field": "isSupplier",
                    "tooltip":(params) => {
                        if(params.data.isSupplier === true){
                            return "Yes";
                        }
                        else if(params.data.isSupplier === false){
                            return "No";
                        }
                    },   
                    "filter": "agTextColumnFilter",
                    "width":120,
                    "valueGetter": (params) => {
                        if(params.data.isSupplier === true){
                            return "Yes";
                        }
                        else if(params.data.isSupplier === false){
                            return "No";
                        }
                    }
                },
                // {
                //     "headerName": "Chevron Export",
                //     "headerTooltip":"Chevron Export",
                //     "field": "ExportColumn",
                //     "cellRenderer": "ExportLink",
                //     "isChevronExport": true,
                //     "buttonAction": "",
                //     "cellRendererParams": {
                //     },
                //     "suppressFilter": true,
                //     "suppressSorting": true,
                //     "width": 50
                // },
                // {
                //     "headerName": "Export",
                //     "headerTooltip":"Export",
                //     "field": "ExportColumn",
                //     "cellRenderer": "ExportLink",
                //     "isChevronExport": false,
                //     "buttonAction": "",
                //     "cellRendererParams": {
                //     },
                //     "suppressFilter": true,
                //     "suppressSorting": true,
                //     "width": 50
                // },
                {
                    "headerName":localConstant.resourceSearch.MOBILE_NO,
                    "headerTooltip":localConstant.resourceSearch.MOBILE_NO,
                    "field": "mobileNumber",
                    "tooltipField":"mobileNumber",   
                    "filter": "agTextColumnFilter",
                    "width":120
                },
                {
                    "headerName":localConstant.resourceSearch.EMAIL,
                    "headerTooltip":localConstant.resourceSearch.EMAIL,
                    "field": "email",
                    "tooltipField":"email",   
                    "filter": "agTextColumnFilter",
                    "width":120
                },
                {
                    "headerName": localConstant.resourceSearch.PROFILE_STATUS,
                    "headerTooltip":localConstant.resourceSearch.PROFILE_STATUS,
                    "field": "profileStatus",
                    "tooltipField":"profileStatus",   
                    "filter": "agTextColumnFilter",
                    "width":120
                },
                {
                    "headerName": localConstant.resourceSearch.SUB_DIVISION,
                    "headerTooltip": localConstant.resourceSearch.SUB_DIVISION,
                    "field": "subDivision",
                    "tooltipField":"subDivision",   
                    "filter": "agNumberColumnFilter",
                    "width":120
                   
                }
            ],
            
            "rowSelection": 'multiple', 
            "rowData":null,  
            "pagination": false,
            "enableFilter": true,
            "enableSorting": true,
            "gridHeight": 60,
            "searchable": true,
            "gridActions": true,
            "gridTitlePanel": true,
            "Grouping":true,
            "exportFileName":"ARS Search",
            "clearFilter":true, 
            "GroupingDataName":"resourceSearchTechspecInfos"
            
        },
        "exceptionSearchResourceHeader": {
            "columnDefs": [  
                {
                    "headerName": localConstant.resourceSearch.SUPLLIER_NAME,
                    "headerTooltip": localConstant.resourceSearch.SUPLLIER_NAME,
                    "field": "supplierName",
                    "tooltipField":"supplierName",   
                    "filter": "agNumberColumnFilter",
                    "width":120
                   
                },
                {
                    "headerName":localConstant.resourceSearch.EPIN,
                    "headerTooltip":localConstant.resourceSearch.EPIN,
                    "field": "epin",
                    "tooltipField":"epin",   
                    "cellRenderer": "HyperLinkRenderer",
                    "cellRendererParams": {
                        "searchType":"Search Exception"
                    },
                    "width": 120
                },
                
                {
                    "headerName": localConstant.resourceSearch.LASTNAME,
                    "headerTooltip": localConstant.resourceSearch.LASTNAME,
                    "field": "lastName",  
                    "tooltipField":"lastName",   
                    "filter": "agTextColumnFilter",               
                    "width":120
                },     
                {
                    "headerName": localConstant.resourceSearch.FIRSTNAME,
                    "headerTooltip":localConstant.resourceSearch.FIRSTNAME,
                    "field": "firstName",
                    "tooltipField":"firstName",   
                    "filter": "agTextColumnFilter",
        
                    "width":120
                },        
                {
                    "headerName": localConstant.resourceSearch.EMPLOYMENT_TYPE,
                    "headerTooltip":localConstant.resourceSearch.EMPLOYMENT_TYPE,
                    "field": "employmentType",
                    "tooltipField":"employmentType",   
                    "filter": "agTextColumnFilter",
                    "width":120
                },
                {
                    "headerName": localConstant.resourceSearch.SCHEDULE_STATUS,
                    "headerTooltip": localConstant.resourceSearch.SCHEDULE_STATUS,
                    "field": "scheduleStatus",
                    "tooltipField":"scheduleStatus",   
                    "filter": "agTextColumnFilter",
                    "cellStyle": (params) =>{
                        if (params.value==='TBA') { 
                            return {  backgroundColor: '#d19917' };
                        }
                        else if (params.value==='PTO') { 
                            return {  backgroundColor: '#78a3e8' };
                        }
                        else if (params.value==='Available') { 
                            return { backgroundColor: '#70c19a' };
                        }
                        else if (params.value==='Confirmed') { 
                            return { backgroundColor: '#cc4141' };
                        }
                        else if (params.value==='Tentative') { 
                            return { backgroundColor: '#d3c954' };
                        } 
                        else if (params.value === 'Pre-Assignment') {
                            return { backgroundColor: '#474e54' , color: '#ffffff' };
                        }
                        else {
                            return null;
                        }
                    },
                    "width":200
                },
                {
                    "headerName": localConstant.resourceSearch.DISTANCE_FROM_VENDOR_MILES,
                    "headerTooltip":localConstant.resourceSearch.DISTANCE_FROM_VENDOR_MILES,
                    "field": "distanceFromVenderInMile",
                    "tooltipField":"distanceFromVenderInMile",   
                    "filter": "agTextColumnFilter",
                    "width":120,
                    "valueGetter": (params) => {
                        if(params.data.distanceFromVenderInMile === -1){
                            return null;
                        }
                        return params.data.distanceFromVenderInMile;
                    }
                },
                {
                    "headerName": localConstant.resourceSearch.DISTANCE_FROM_VENDOR_KM,
                    "headerTooltip":localConstant.resourceSearch.DISTANCE_FROM_VENDOR_KM,
                    "field": "distanceFromVenderInKm",
                    "tooltipField":"distanceFromVenderInKm",   
                    "filter": "agTextColumnFilter",
                    "width":120
                },
                {
                    "headerName":localConstant.resourceSearch.SUPPLIER_,
                    "headerTooltip":localConstant.resourceSearch.SUPPLIER_,
                    "field": "isSupplier",
                    "tooltip":(params) => {
                        if(params.data.isSupplier === true){
                            return "Yes";
                        }
                        else if(params.data.isSupplier === false){
                            return "No";
                        }
                    },   
                    "filter": "agTextColumnFilter",
                    "width":120,
                    "valueGetter": (params) => {
                        if(params.data.isSupplier === true){
                            return "Yes";
                        }
                        else if(params.data.isSupplier === false){
                            return "No";
                        }
                    }
                },
                {
                    "headerName":localConstant.resourceSearch.MOBILE_NO,
                    "headerTooltip":localConstant.resourceSearch.MOBILE_NO,
                    "field": "mobileNumber",
                    "tooltipField":"mobileNumber",   
                    "filter": "agTextColumnFilter",
                    "width":120
                },
                {
                    "headerName":localConstant.resourceSearch.EMAIL,
                    "headerTooltip":localConstant.resourceSearch.EMAIL,
                    "field": "email",
                    "tooltipField":"email",   
                    "filter": "agTextColumnFilter",
                    "width":120
                },
                {
                    "headerName": localConstant.resourceSearch.PROFILE_STATUS,
                    "headerTooltip":localConstant.resourceSearch.PROFILE_STATUS,
                    "field": "profileStatus",
                    "tooltipField":"profileStatus",   
                    "filter": "agTextColumnFilter",
                    "width":120
                },
                {
                    "headerName": localConstant.resourceSearch.SUB_DIVISION,
                    "headerTooltip": localConstant.resourceSearch.SUB_DIVISION,
                    "field": "subDivision",
                    "tooltipField":"subDivision",   
                    "filter": "agNumberColumnFilter",
                    "width":120
                   
                },
                {
                    "headerName": localConstant.resourceSearch.EXCEPTION_COMMENTS,
                    "headerTooltip": localConstant.resourceSearch.EXCEPTION_COMMENTS,
                    "field": "exceptionComment",
                    "tooltipField":"exceptionComment",   
                    "filter": "agNumberColumnFilter",
                    "width":120
                   
                },
                
            ],
            
            "rowSelection": 'multiple', 
            "rowData":null,  
            "pagination": false,
            "enableFilter": true,
            "enableSorting": true,
            "gridHeight": 60,
            "searchable": true,
            "gridActions": true,
            "gridTitlePanel": true,
            "Grouping":true,
            "exportFileName":"Exception List",
            "GroupingDataName":"searchExceptionResourceInfos"
            
        },
        "supplierHeader":{
            "columnDefs": [
                {
                    "headerCheckboxSelectionFilteredOnly": true,
                    "checkboxSelection": true,
                    "suppressFilter": true,
                    "width": 40
                },           
                {
                    "headerName": localConstant.resourceSearch.SUBSUPPLIER,
                    "headerTooltip": localConstant.resourceSearch.SUBSUPPLIER,
                    "field": "subSupplier",
                    "tooltipField":"subSupplier",   
                    "filter": "agTextColumnFilter",
                    "width":440
                },
                {
                    "headerName": localConstant.resourceSearch.SUB_SUPPLIER_LOCATION,
                    "headerTooltip": localConstant.resourceSearch.SUB_SUPPLIER_LOCATION,
                    "field": "subSupplierLocation",
                    "tooltipField":"subSupplierLocation",   
                    "filter": "agTextColumnFilter",
                    "width":470
                },
                {
                    "headerName": "",
                    "field": "EditColumn",
                    "cellRenderer": "EditLink",
                    "buttonAction":"",
                    "cellRendererParams": {
                        "disableField": (e) => functionRefs.disableEdit(e)
                    },
                    "suppressFilter": true,
                    "suppressSorting": true,
                    "width": 50
                }
            ] 
        },
        "AssignedResourceHeader":{
            "columnDefs": [     
                {
                    "headerName": localConstant.resourceSearch.RESOURCE_NAME,
                    "headerTooltip": localConstant.resourceSearch.RESOURCE_NAME,
                    "field": "resourceName",

                    "tooltipField":"resourceName",   
                    "filter": "agTextColumnFilter",
                    "width":295
                },
                {
                    "headerName": localConstant.resourceSearch.SUPPLIER,
                    "headerTooltip": localConstant.resourceSearch.SUPPLIER,
                    "field": "supplierName",
                    "tooltipField":"supplierName",   
                    "filter": "agTextColumnFilter",
                    "width":265
                },
                {
                    "headerName": localConstant.resourceSearch.TAXONOMY,
                    "headerTooltip": localConstant.resourceSearch.TAXONOMY,
                    "field": "taxonomy",
                    "tooltipField":"taxonomy",   
                    "filter": "agTextColumnFilter",
                    "width":250
                },
                {
                    "headerName": localConstant.resourceSearch.ACTIVE_FLAG,
                    "headerTooltip": localConstant.resourceSearch.ACTIVE_FLAG,
                    "field": "profileStatus",
                    "tooltipField":"profileStatus",   
                    "filter": "agTextColumnFilter",
                    "width":200
                }
            ] 
        },
        "OverrideResourceHeader":{
            "columnDefs": [
                {
                    "headerName": "First Name",
                    "field": "techSpecialist.firstName",
                    "tooltipField":"techSpecialist.firstName",   
                    "filter": "agTextColumnFilter",
                    "width": 200
                },
                {
                    "headerName": "Last Name",
                    "field": "techSpecialist.lastName",
                    "tooltipField":"techSpecialist.lastName",   
                    "filter": "agTextColumnFilter",
                    "width": 200
                },
                {
                    "headerName": "",
                    "field": "EditColumn",
                    "cellRenderer": "OverrideApprovalLink",
                    "buttonAction":"",
                    "cellRendererParams": {
                    },
                    "suppressFilter": true,
                    "suppressSorting": true,
                    "width": 170
                }
            ]
        },
        "preAssignmentSearchHeader":{
            "columnDefs": [
                {
                    "headerCheckboxSelectionFilteredOnly": true,
                    "checkboxSelection": true,
                    "suppressFilter": true,
                    "width": 40
                },
                {
                    "headerName":"PreAssignmentID",
                    "headerTooltip":"PreAssignmentID",
                    "field": "id",
                    "tooltipField":"id",   
                    "filter": "agNumberColumnFilter",
                    "width":150
                },
                {
                    "headerName": "Customer Name",
                    "headerTooltip":"CustomerName",
                    "tooltipField":"searchParameter.customerName",   
                    "field": "searchParameter.customerName",
                    "filter": "agTextColumnFilter",
                    "width":200
                },
                {
                    "headerName": "Taxnomy Service",
                    "headerTooltip":"Taxnomy Service",
                    "field": "serviceName",
                    "tooltipField":"serviceName",   
                    "filter": "agTextColumnFilter",
                    "width":150
                },
                {
                    "headerName": "Main Supplier Name",
                    "headerTooltip":"Main Supplier Name",
                    "field": "searchParameter.supplier",
                    "tooltipField":"searchParameter.supplier",   
                    "filter": "agTextColumnFilter",
                    "width":180
                },
            ],
            "enableFilter": true,
            "enableSorting": true,
            "pagination": false,
            "gridHeight": 20,
            "clearFilter":true, 
            "rowSelection": 'single',
        },
        "commentsHistoryReportHeader":{
            "columnDefs": [     
                {
                    "headerName": localConstant.resourceSearch.USER_NAME,
                    "headerTooltip": localConstant.resourceSearch.USER_NAME,
                    "field": "createdBy",
                    "tooltipField":"createdBy",   
                    "filter": "agTextColumnFilter",
                    "width":150
                },
                {
                    "headerName": localConstant.resourceSearch.COMMOENTS,
                    "headerTooltip": localConstant.resourceSearch.COMMOENTS,
                    "field": "notes",
                    "tooltipField":"notes",   
                    "filter": "agTextColumnFilter",
                    "width":365
                },
                {
                    "headerName": localConstant.resourceSearch.CREATION_DATE,
                    "headerTooltip": localConstant.resourceSearch.CREATION_DATE,
                    "field": "createdOn",
                    "tooltip": (params) => {
                        return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                    },  
                    "filter": "agDateColumnFilter",
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "createdOn"
                    },
                    "width":150,
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                    } 
                },
            ],
            "enableSorting":true, 
            "enableFilter": true,
            "searchable": true,
            "pagination": true,  
            "gridHeight":54,  
            "clearFilter":true,   
        },
        'cvOptionsHeader':{
    
            "columnDefs": [
                {
                    "checkboxSelection": true,
                    "headerCheckboxSelectionFilteredOnly": true,
                    "suppressFilter": true,
                    "width": 40,
                    "cellRenderer": (params) => {
                        if (params.node.rowIndex !== 8 && params.node.rowIndex !== 9) {
                            params.node.setSelected(true);
                        }
                        else{
                            params.node.setSelected(false);
                        }
                    }
                },
                {
                    "headerName": "Section",
                    "headerTooltip": "Section",
                    "field": "menu",
                    "tooltipField": "menu",
                    "width": 178
                },
                {
                    "headerName": "Sub-Section",
                    "headerTooltip": "Sub-Section",
                    "field": "subMenu",
                    "tooltipField": "subMenu",
                    "width": 151
                },
                {
                    "headerName": "Sub-Filter",
                    "headerTooltip": "Sub-Filter",
                    "field": "optional",
                    "tooltipField": "optional",
                    "width": 366
                },
            ],
            "enableFilter":false, 
            "enableSorting":false,
            "rowSelection": "multiple",
            "gridHeight":50,
            "pagination": false,
            "searchable":false,
            "clearFilter":false,
        },
    };
};      