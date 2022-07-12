import { getlocalizeData,isUndefined,formatToDecimal, thousandFormat } from '../../../../utils/commonUtils';
import { requiredNumeric } from '../../../../utils/validator';
import store from '../../../../store/reduxStore';
const localConstant = getlocalizeData();

/** Header Data for Assignment Specialist Grid - inline editing ITK CR */
export const AssignedSpecialistHeader = (props) => {
    return {
        "columnDefs": [
            {
                "headerCheckboxSelectionFilteredOnly": true,
                "checkboxSelection": true,
                "suppressFilter": true,
                "width": 40
            },
            {
                "headerName": localConstant.assignments.SPECIALIST_NAME,
                "field": "technicalSpecialistName",
                "filter": "agTextColumnFilter",
                "width":230,  
                "tooltipField":"technicalSpecialistName",      
                "headerTooltip": localConstant.assignments.SPECIALIST_NAME,
                "cellRenderer": "SelectDataType",   //Added for D634 issue 2            
                "type": "label",
                "cellRendererParams": (params) => {                                
                    return { labelValue: `${ params.data.technicalSpecialistName } (${ params.data.epin })` }; // ITK D-710
                },
            },
            {
                "headerName": localConstant.assignments.INACTIVE,
                "field": "isActive",
                "filter": "agTextColumnFilter",
                "width":230,
                "cellRenderer": "InlineSwitch",
                "cellRendererParams": {
                    "isSwitchLabel": false,
                    "name":"isActive",
                    "id":"isActiveId",
                    "switchClass":"",
                    "invertSwitchValue":true,
                    "disabled":props.disabled,
                },
                "tooltip":(params)=>{
                    if(params.data && params.data.isActive===false){
                        return 'Yes';
                    }else{
                        return "No";
                    }
                },      
                "valueGetter":(params)=>{
                    if(params.data && params.data.isActive===false){
                        return 'Yes';
                    }else{
                        return "No";
                    }
                },
                "headerTooltip": localConstant.assignments.INACTIVE,
            },
            {
                "headerName": localConstant.assignments.SUPERVISOR,
                "field": "isSupervisor",
                "filter": "agTextColumnFilter",
                "width":150,
                "cellRenderer": "InlineSwitch",
                "cellRendererParams": {
                    "isSwitchLabel": false,
                    "name":"isSupervisor",
                    "id":"isSupervisorId",
                    "switchClass":"",
                    "disabled":props.disabled,
                },
                "tooltip":(params)=>{
                    if(params.data && params.data.isSupervisor===false){
                        return 'No';
                    }else{
                        return "Yes";
                    }
                }, 
                "valueGetter":(params)=>{
                    if(params.data && params.data.isSupervisor===false){
                        return 'No';
                    }else{
                        return "Yes";
                    }
                },       
                "headerTooltip": localConstant.assignments.SUPERVISOR,   
            },
            {
                "headerName": '',  
                "headerTooltip": localConstant.assignments.ASSIGNMENT_RESOURCE_REPORT,
                "field": "epin",                      
                "tooltipField": localConstant.assignments.ASSIGNMENT_RESOURCE_REPORT,           
                "cellRenderer": "PrintReport",
                "suppressFilter": true,
                "suppressSorting": true,
                 "width": 150
            }
        ],
        // "gridTitlePanel": true,
        "enableSelectAll":true,    
        "enableSorting":true, 
        "enableFilter": true,
        // "searchable": true,
        // "pagination": true,    
        "gridHeight":54,  
        "clearFilter":true,   
        "rowSelection":"single"
    };
};

export const HeaderData = (props)=> { ////def 957
    return {
    "technicalSpecialistSchedules":{
        "columnDefs": [
            {
                "headerCheckboxSelectionFilteredOnly": true,
                "checkboxSelection": true,
                "cellRenderer": (params) => { // D-709
                    if (params.node.data) {
                        if (params.node.data.isSelected)
                            params.node.setSelected(params.node.data.isSelected);
                    }
                },
                "suppressFilter": true,
                "width": 40
            },
            {
                "headerName": localConstant.assignments.CHARGE_SCHEDULE,
                "field": "contractScheduleName",
                "filter": "agTextColumnFilter",
                "width":230,  
                "tooltipField":"contractScheduleName",        
                "headerTooltip": localConstant.assignments.CHARGE_SCHEDULE,
            },
            {
                "headerName": localConstant.assignments.PAY_SCHEDULE,
                "field": "technicalSpecialistPayScheduleName",
                "filter": "agTextColumnFilter",
                "width":230,  
                "tooltipField":"technicalSpecialistPayScheduleName",        
                "headerTooltip": localConstant.assignments.PAY_SCHEDULE,
            },
            {
                "headerName": localConstant.assignments.SCHEDULE_NOTES_TO_PRINT_ON_INVOICE,
                "field": "scheduleNoteToPrintOnInvoice",
                "filter": "agTextColumnFilter",
                "width":450,  
                "tooltipField":"scheduleNoteToPrintOnInvoice",        
                "headerTooltip": localConstant.assignments.SCHEDULE_NOTES_TO_PRINT_ON_INVOICE,   
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
        "enableFilter": true,
        "gridTitlePanel": true,
        "enableSelectAll":true,    
        "enableSorting":true, 
        "pagination": true,    
        "gridHeight":54,  
        "clearFilter":true,   
        "rowSelection":"single"
    },
    "chargeScheduleRates":{
        "columnDefs": [
            {
                "headerName": localConstant.assignments.TYPE,
                "field": "chargeType",
                "filter": "agTextColumnFilter",
                "width":200,  
                "tooltipField":"chargeType",        
                "headerTooltip": localConstant.assignments.TYPE,
            },
            {
                "headerName": localConstant.assignments.RATE,
                "field": "chargeValue",
                "filter": "agTextColumnFilter",
                "width": 150,
                "tooltip": (params) => {
                    if(params.data.chargeValue === undefined) {
                        return params.data.chargeValue;
                    } else {
                        return thousandFormat(params.data.chargeValue);
                    }
                    //return formatToDecimal(params.data.chargeValue,4);
                },
                "valueGetter": (params) => {
                    if(params.data.chargeValue === undefined) {
                        return params.data.chargeValue;
                    } else {
                        return thousandFormat(params.data.chargeValue);
                    }
                    //return formatToDecimal(params.data.chargeValue,4); //Changes for ITK D1532
                },
                "headerTooltip": localConstant.assignments.RATE,
            },
            {
                "headerName": localConstant.assignments.CURRENCY,
                "field": "currency",
                "filter": "agTextColumnFilter",
                "width":150,  
                "tooltipField":"currency",        
                "headerTooltip": localConstant.assignments.CURRENCY,
            }
        ],    
        "enableSorting":true, 
        "pagination": false,    
        "gridHeight":54,    
        "clearFilter":true, 
        "rowSelection":"multiple"
    },
    "payScheduleRates":{
        "columnDefs": [
            {
                "headerName": localConstant.assignments.TYPE,
                "field": "expenseType",
                "filter": "agTextColumnFilter",
                "width":200,  
                "tooltipField":"expenseType",        
                "headerTooltip": localConstant.assignments.TYPE,
            },
            {
                "headerName": localConstant.assignments.RATE,
                "field": "rate",
                "filter": "agTextColumnFilter",
                "width":150,        
                "headerTooltip": localConstant.assignments.RATE,
                "tooltip": (params) => {
                    if(params.data.rate === undefined) {
                        return params.data.rate;
                    } else {
                        return thousandFormat(formatToDecimal(params.data.rate,4));
                    }
                },
                "valueGetter": (params) => {
                    if(params.data.rate === undefined) {
                        return params.data.rate;
                    } else {
                        return thousandFormat(formatToDecimal(params.data.rate,4)); //Changes for D1532
                    } 
                },
            },
            {
                "headerName": localConstant.assignments.CURRENCY,
                "field": "payScheduleCurrency",
                
                "filter": "agTextColumnFilter",
                "width":150,  
                "tooltipField":"payScheduleCurrency",        
                "headerTooltip": localConstant.assignments.CURRENCY,
            }
        ],    
        "enableSorting":true, 
        "pagination": false,    
        "gridHeight":54,    
        "clearFilter":true, 
        "rowSelection":"multiple"
    },
    "techSpecSearch":{
        "columnDefs": [
            {
                "headerCheckboxSelectionFilteredOnly": true,
                "checkboxSelection": true,
                "suppressFilter": true,
                "width": 40
            },
            {
                "headerName": "ePin",
                "field": "epin",
                "tooltipField":"epin",   
                "headerTooltip":"ePin",
                "filter": "agTextColumnFilter",
                "width":120,
                "cellRenderer": "HyperLinkRenderer",
                "cellRendererParams": {
                        "searchType":"NDT",
                        "getInterCompanyCode": () => props.getInterCompanyInfo(),
                },
            },
            {
                "headerName": "Last Name", //Changes for Hot Fixes on NDT
                "field": "lastName",
                "tooltipField":"lastName",   
                "headerTooltip":"Last Name",//Changes for Hot Fixes on NDT
                "filter": "agTextColumnFilter",
                "width":130
            },
            {
                "headerName": "First Name",//Changes for Hot Fixes on NDT
                "field": "firstName",
                "tooltipField":"first Name",   //Changes for Hot Fixes on NDT
                "filter": "agTextColumnFilter",
                "width":140
            },
            {
                "headerName": "Status",
                "field": "profileStatus",
                "headerTooltip":"profileStatus",
                "filter": "agTextColumnFilter",
                "width":120
            },
            {
                "headerName": "Company",
                "field": "companyName",
                "tooltipField":"companyName",   
                "headerTooltip":"Company",
                "filter": "agTextColumnFilter",
                "width":200
            },
            {
                "headerName": "Country",
                "field": "country",
                "tooltipField":"country",   
                "headerTooltip":"Country",
                "filter": "agTextColumnFilter",
                "width":120
            },
            {
                "headerName": "City",
                "field": "city",
                "tooltipField":"city",   
                "headerTooltip":"City",
                "filter": "agTextColumnFilter",
                "width":120
            },
            {
                "headerName": "State/County/Province",
                "field": "county",
                "tooltipField":"county",   
                "headerTooltip":"State/County/Province",
                "filter": "agTextColumnFilter",
                "width":220
            },
            {
                "headerName": "Post/Zip Code",
                "field": "pinCode",
                "tooltipField":"pinCode",   
                "headerTooltip":"Post/Zip Code",
                "filter": "agTextColumnFilter",
                "width":160
            },
            {
                "headerName": "Full Address",
                "field": "fullAddress",
                "tooltipField":"fullAddress",
                "headerTooltip":"Full Address",
                "filter": "agTextColumnFilter",
                "width":140
            },
            {
                "headerName": "Mobile No",
                "field": "mobileNumber",
                "tooltipField": "mobileNumber",   
                "headerTooltip":"Mobile No",
                "filter": "agNumberColumnFilter",
                "width":140,
                "filterParams": {
                    "inRangeInclusive":true
                }
            },{
                "headerName": "Email Address",
                "field": "emailAddress",
                "tooltipField": "emailAddress",   
                "headerTooltip":"Email Address",
                "filter": "agTextColumnFilter",
                "width":180
            },{
                "headerName": "Sub Division",
                "field": "subDivisionName",//Changes for Hot Fixes on NDT
                "tooltipField":"subDivisionName",  //Changes for Hot Fixes on NDT 
                "headerTooltip":"Sub Division",
                "filter": "agNumberColumnFilter",
                "width":140,
                "filterParams": {
                    "inRangeInclusive":true
                }
            },
            {
                "headerName": "Employment Status",
                "field": "employmentType",//Changes for Hot Fixes on NDT
                "tooltipField":"employmentType",   //Changes for Hot Fixes on NDT
                "headerTooltip":"Employment Status",
                "filter": "agNumberColumnFilter",
                "width":250,
                "filterParams": {
                    "inRangeInclusive":true
                }
            }
        ],
        
        "rowSelection": 'multiple',
        "enableFilter": true,
        "enableSorting": true,
        "gridHeight": 35,
        "gridActions": true,
        "clearFilter":true, 
        "gridTitlePanel": true
    },
    "viewTechnicalSpecialist":{
        "columnDefs": [
            {
                "headerName": "Resource Name",
                "field": "technicalSpecialistName",
                "headerTooltip":"technicalSpecialistName",
                "filter": "agTextColumnFilter",
                "width":250
            },
            {
                "headerName": "Inactive",
                "field": "isActive",
                "headerTooltip":"Inactive",
                "filter": "agTextColumnFilter",
                "valueGetter":(params)=>{
                    if(params.data.isActive===false){
                        return 'No';
                    }else{
                        return "Yes";
                    }
                },
                "width":220
            },
            {
                "headerName": "Supervisor",
                "field": "isSupervisor",
                "headerTooltip":"Supervisor",
                "filter": "agTextColumnFilter",
                "valueGetter":(params)=>{
                    if(params.data.isSupervisor===false){
                        return 'No';
                    }else{
                        return "Yes";
                    }
                },
                "width":225
            },
        ]
    }
};
};