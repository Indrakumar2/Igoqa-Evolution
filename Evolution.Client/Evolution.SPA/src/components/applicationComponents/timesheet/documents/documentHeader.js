import { isUndefined,getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData = (functionRefs) => {
    return {
        "columnDefs": [
       
            {
                // "checkboxSelection": (params)=>{               
                //     if(params.data.isDisableStatus && params.data.recordStatus == null ) {
                //         return false;
                //     }else if(!params.data.isDisableStatus && params.data.recordStatus == null) {
                //         return true;                    
                //     }else{
                //         return true;
                //     }
                
                // },
                "checkboxSelection": true,
                "headerCheckboxSelection":true,         
                "headerCheckboxSelectionFilteredOnly": true,
                "suppressFilter":true,
                "width": 40
            },
            {
                "headerName": localConstant.gridHeader.NAME,
                "headerTooltip": localConstant.gridHeader.NAME,
                "field": "documentName",
                "tooltipField": "documentName",
                "filter": "agTextColumnFilter",
                //"cellRenderer": "FileToBeOpen",
                // "editable":  (params) => {              
                //     if (!params.data.roleBase === true) {
                //         return true;
                //     } else if (!params.data.roleBase === false) {
                //         return false;
                //     }
                // },
                "editable":  (params) => {                      
                //RoleBase False - it means EditMode and true Means NonEditMode
                //isOperatorApporved  - when the visit is Approved and existing  Upload Document Edit option should be disabled
                if(params.data.roleBase && !params.data.isDisableStatus && params.data.recordStatus == null &&!params.data.isFileUploaded){
                    return false; // Other then Approved Scenario and Role Base status is "true" means NonEditMode
                }
                else if(!params.data.roleBase && !params.data.isDisableStatus && params.data.recordStatus == null&&params.data.isFileUploaded){
                    return true;   // Scenario Approved By CHC / OC - Home Company and CHC is Equal means user can Edit And Delete CHC have all Permission 
                }
                else if(!params.data.roleBase && params.data.isDisableStatus && params.data.recordStatus == null&&!params.data.isFileUploaded){
                    return false;  // Scenario Approved By CHC / OC - Home Company and OC is Equal means user don't have Edit and Delete options
                }               
                else if(!params.data.roleBase && params.data.isDisableStatus && params.data.recordStatus !== null &&params.data.isFileUploaded){
                    return true;
                }else if(!params.data.roleBase && !params.data.isDisableStatus&&params.data.isFileUploaded){
                    return true;
                }                       
                },
                "cellRendererParams": {
                    "dataToRender": "documentName"
                },
                "width": 162
            },
            {
                "headerName": "<span class='mandate'>" + localConstant.gridHeader.TYPE + "</span>",
                "headerTooltip": localConstant.gridHeader.TYPE,
                "field": "documentType",
                "tooltipField": "documentType",
                "filter": "agTextColumnFilter",
                "cellRenderer": "SelectDocumentType",
                "width": 250,
                "documentModuleName":'TimesheetDocument',
                "moduleType":"TimesheetDocument"
            },
            {
                "headerName": localConstant.gridHeader.SIZE_IN_KBS,
                "headerTooltip": localConstant.gridHeader.SIZE_IN_KBS,
                "field": "documentSize",
                "tooltipField": "documentSize",
                "filter": "agNumberColumnFilter",
                "filterParams": {
                    "inRangeInclusive":true
                },
                "width": 120
            },
            {
                "headerName":  localConstant.gridHeader.UPLOADED_DATE,
                "field": "createdOn",
                "tooltip": (params) => {
                    return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                },
                "filter": "agDateColumnFilter", //Changes for ITK 326
                "cellRenderer": "DateComponent",
                "cellRendererParams": {
                    "dataToRender": "createdOn"
                },
                "width": 140,
                "headerTooltip": localConstant.gridHeader.UPLOADED_DATE,
                "filterParams": {
                    "comparator": dateUtil.comparator,
                    "inRangeInclusive": true,
                    "browserDatePicker": true
                }
            },
            {
                "headerName":localConstant.timesheet.CUSTOMER_VISIBLE,
                "field": "isVisibleToCustomer",
                "filter": "agTextColumnFilter",
                "width": 130,
                "cellRenderer": "InlineSwitch",
                "documentModuleName":'TimesheetDocument',   // Defect Id 948 isOperatorApporved  - when visit is Approved and existing  Upload Document Switch option should be disabled
                "moduleType":"TimesheetDocument",  
                "cellRendererParams": {
                    "isSwitchLabel": false,
                    "name":"isVisibleToCustomer",
                    "id":"isVisibleToCustomer",
                    "switchClass":"",
                    "disabled":functionRefs.isDisable,
                    // "disabled":props.disabled,
                },
                "tooltip": (params) => {
                    if (params.data.isVisibleToCustomer) {
                        return "Yes";
                    } else{
                        return "No";
                    }
                },  
                "valueGetter": (params) => {
                    if (params.data.isVisibleToCustomer) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "headerTooltip":localConstant.timesheet.CUSTOMER_VISIBLE,
            },        
            // {
            //     "headerName":  localConstant.timesheet.CUSTOMER_VISIBLE,
            //     "headerTooltip": localConstant.timesheet.CUSTOMER_VISIBLE,
            //     "field": "isVisibleToCustomer",
            //     "tooltip":  (params) => {
            //         if (params.data.isVisibleToCustomer === true) {
            //             return "Yes";
            //         } else {
            //             return "No";
            //         }
            //     },
            //     "filter": "agTextColumnFilter",
            //     "valueGetter": (params) => {
            //         if (params.data.isVisibleToCustomer === true) {
            //             return "Yes";
            //         } else {
            //             return "No";
            //         }
            //     },
            //     "width":135
            // },
            {
                "headerName":  localConstant.timesheet.SPECIALIST_VISIBLE,
                "headerTooltip": localConstant.timesheet.SPECIALIST_VISIBLE,
                "field": "isVisibleToTS",
                "tooltip": (params) => {
                    if (params.data.isVisibleToTS) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "filter": "agTextColumnFilter",
                "valueGetter": (params) => {
                    if (params.data.isVisibleToTS) {
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "width":130
            },
            {
                "headerName": "",
                "field": "moduleRefCode",
                "hide": true
            },
            {
                "headerName": "",
                "field": "documentUniqueName",
                "hide": true
            },
            // {
            //     "headerName": "",
            //     "field": "EditTimesheetDocuments",
            //     "cellRenderer": "EditLink",
            //     "cellRendererParams": {},
            //     "suppressFilter": true,
            //     "suppressSorting": true,
            //     "width": 50
            // },
            {
                "headerName": '',           
                "field": "documentName",                      
                "tooltipField": "documentName",           
                "cellRenderer": "FileToBeOpen",
                "suppressFilter": true,
                "suppressSorting": true,
                 "cellRendererParams": {
                     "dataToRender": "documentName"
                 },
                 "width": 50
            }
        ],
        "enableFilter": true,
        "enableSorting": true,
        "rowSelection": "multiple",
        "gridHeight": 45,
        "pagination": true,
        "searchable":true,
        "enableSelectAll":true,
        "clearFilter":true, 
        "exportFileName":"Documents"
    };
};