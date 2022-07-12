import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();
export const defaultFilterData = {
    timesheetContractNumber: "",
    timesheetProjectNumber: "",
    timesheetAssignmentNumber: "",
    timesheetStartDate: "",
    timesheetEndDate: "",
    timesheetStatus: "",
    timesheetContractHolderCompanyCode:"",
    timesheetContractHolderCompany: "",
    timesheetContractHolderCoordinator: "",
    timesheetOperatingCompanyCoordinator:"",
    timesheetOperatingCompanyCoordinatorCode:"",
    timesheetOperatingCompany: "",
    technicalSpecialistName:"",
    timesheetDescription:"",
    searchDocumentType:"",
    documentSearchText:"",
    timesheetCategory:'',
    timesheetSubCategory:'',
    timesheetService:''  
};

export const searchFilterWithFunctionRefs = (functionRefs)=>{
    return {
        "component": "SearchFilter",
        "text": 'heading of the comp',
        "properties": {},
        "subComponents": [
            {
                "component": "CustomerAndCountrySearch",
                "properties": {
                    "colSize":"col s3 pl-0"
                }
            },
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'timesheetContractNumber',
                    'id': 'timesheetContractNumber',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.assignments.CONTRACT_NUMBER,
                    "onValueChange": (e)=>{ functionRefs.changeHandler(e); },
                    "type": 'text',
                    "dataValType": 'valueText',
                    "colSize": 's3 pl-0',
                    "inputClass": "customInputs",
                    "name": 'timesheetContractNumber',
                    "maxLength": 20,
                    "value":''
                }
            }, 
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'timesheetProjectNumber',
                    'id': 'timesheetProjectNumber',
                    'hasLabel': true,
                    "divClassName": "col pl-0",
                    "label": localConstant.project.PROJECT_NUMBER,
                    "onValueChange": (e)=>{ functionRefs.changeHandler(e); },
                    "type": 'text',
                    "dataType": 'numeric',
                    "valueType":'value',
                    "value":'',
                    "colSize": 's3 pl-0',
                    "inputClass": "customInputs",
                    "name": 'timesheetProjectNumber',
                    "maxLength": 20,
                }
            },
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'timesheetAssignmentNumber',
                    'id': 'timesheetAssignmentNumber',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.assignments.ASSIGNMENT_NUMBER,
                    "onValueChange": (e)=>functionRefs.changeHandler(e),
                    "type": 'text',
                    "dataType": 'numeric',
                    "valueType":'value',
                    "value":'',
                    "colSize": 's3 pl-0',
                    "inputClass": "customInputs",
                    "name": 'timesheetAssignmentNumber',
                    "maxLength": 20,
                }
            },
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'timesheetStartDate',
                    'id': 'timesheetStartDate',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.timesheet.EARLIEST_DATE,
                    "dateFormat":localConstant.commonConstants.UI_DATE_FORMAT,
                    "onDateChange":(e)=>{ functionRefs.handleEariliestDateChange(e); },
                    "autocomplete":"off",
                    "selectedDate":"",
                    "type": 'date',
                    "colSize": 's3',
                    "inputClass": "customInputs",
                    "name": 'timesheetStartDate',
                    "shouldCloseOnSelect": true,
                }
            }, 
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'timesheetEndDate',
                    'id': 'timesheetEndDate',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.timesheet.LATEST_DATE,
                    "dateFormat":localConstant.commonConstants.UI_DATE_FORMAT,
                    "onDateChange":(e)=>{ functionRefs.handleLatestDateChange(e); },
                    "autocomplete":"off",
                    "selectedDate":"",
                    "type": 'date',
                    "colSize": 's3 pl-0',
                    "inputClass": "customInputs",
                    "name": 'timesheetEndDate',
                    "shouldCloseOnSelect": true
                }
            },
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'timesheetStatus',
                    'id': 'timesheetStatus',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.gridHeader.STATUS,
                    "colSize": 's3 pl-0',
                    "inputClass": "customInputs",
                    "name": 'timesheetStatus',
                    "type": 'select',
                    'optionsList':[],
                    'optionName':'name',
                    'optionValue':'value',
                    "onSelectChange": (e)=>{ functionRefs.changeHandler(e); },
                    "defaultValue":''
                }
            },           
            {
                "component": "CustomInput",
                // "properties": {
                //     'dataKey': 'companyList',
                //     'id': 'timesheetContractHolderCompanyCode',
                //     'hasLabel': true,
                //     "divClassName": "col",
                //     "label": localConstant.assignments.CONTRACT_HOLDING_COMPANY,
                //     "colSize": 's3 pl-0',
                //     "inputClass": "customInputs",
                //     "name": 'timesheetContractHolderCompany',
                //     "type": 'select',
                //     'optionsList':[],
                //     'optionName':'companyName',
                //     'optionValue':'companyCode',
                //     "onSelectChange": (e)=>{ functionRefs.changeHandler(e); },
                //     "defaultValue":''
                // }
                "properties": {
                    'dataKey': 'companyList',
                    'id': 'timesheetContractHolderCompanyCode',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.assignments.CONTRACT_HOLDING_COMPANY,
                    "colSize": 's3 pl-0',
                    "inputClass": "customInputs",
                    "name": 'timesheetContractHolderCompany',
                    "type": 'select',
                    "dataType": 'select',
                    'optionsList':[],
                    'optionName':'companyName',
                    'optionValue':'id',
                    "onSelectChange": (e)=>{ functionRefs.changeHandler(e); },
                    "defaultValue":''                    
                },
                
            },
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'contractHoldingCompanyCoordinators',
                    'id': 'timesheetContractHolderCoordinatorCode',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.gridHeader.CH_COORDINATOR_NAME,
                    "type": 'select',
                    "colSize": 's3',
                    "inputClass": "customInputs",
                    "name": 'timesheetContractHolderCoordinator',
                    'optionsList':[],
                    'optionName':'displayName',
                    'optionValue':'id',
                    "onSelectChange": (e)=>{ functionRefs.changeHandler(e); },
                    "defaultValue":''
                }
            },  
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'companyList',
                    'id': 'timesheetOperatingCompanyCode',                                       
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.assignments.OPERATING_COMPANY,
                    "colSize": 's3 pl-0',
                    "inputClass": "customInputs",
                    "name": 'timesheetOperatingCompany',
                    "type": 'select',
                    'optionsList':[],
                    'optionName':'companyName',
                    'optionValue':'id',
                    "onSelectChange": (e)=>{ functionRefs.changeHandler(e); },
                    "defaultValue":''
                }
            },
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'operatingCompanyCoordinators',
                    'id': 'timesheetOperatingCompanyCoordinatorCode',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.gridHeader.OC_COORDINATOR_NAME,
                    "colSize": 's3 pl-0',
                    "inputClass": "customInputs",
                    "name": 'timesheetOperatingCompanyCoordinator',
                    "type": 'select',
                    'optionsList':[],
                    'optionName':'displayName',
                    'optionValue':'id',
                    "onSelectChange": (e)=>{ functionRefs.changeHandler(e); },
                    "defaultValue":''
                }
            },            
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'technicalSpecialistName',
                    'id': 'technicalSpecialistName',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.gridHeader.TECHNICAL_SPECIALIST,
                    "onValueChange": (e)=>{ functionRefs.changeHandler(e); },
                    "type": 'text',
                    'dataValType':'valueText',
                    'value':'',  
                    "colSize": 's3 pl-0',
                    "inputClass": "customInputs",
                    "name": 'technicalSpecialistName',
                }
            },
           
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'taxonomyCategory',
                    'id': 'timesheetCategory',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.assignments.CATEGORY,
                    "colSize": 's4',
                    "inputClass": "customInputs",
                    "name": 'timesheetCategory',
                    "type": 'select',
                    'optionsList': [],
                    'optionName': 'name',
                    'optionValue': "id",
                    'defaultValue': '',
                    "onSelectChange": (e) => { functionRefs.changeHandler(e); },
                }
            }, 
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'techSpecSubCategory',
                    'id': 'timesheetSubCategory',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.assignments.SUB_CATEGORY,
                    "colSize": 's4',
                    "inputClass": "customInputs",
                    "name": 'timesheetSubCategory',
                    "type": 'select',
                    'optionsList':[],
                    'optionName':'taxonomySubCategoryName',
                'optionValue':"id",
                    'defaultValue':'',
                    "onSelectChange": (e)=>{ functionRefs.changeHandler(e); },
                }
            }, 
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'techSpecServices',
                    'id': 'timesheetService',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.assignments.SERVICE,
                    "colSize": 's4',
                    "inputClass": "customInputs",
                    "name": 'timesheetService',
                    "type": 'select',
                    'optionsList':[],
                    'optionName':'taxonomyServiceName',
                'optionValue':"id",
                    'defaultValue':'',
                    "onSelectChange": (e)=>{ functionRefs.changeHandler(e); },
                }
            }, 

            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'timesheetDescription',
                    'id': 'timesheetDescription',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.timesheet.TIMESHEET_DESCRIPTION,
                    "colSize": 's3',
                    "inputClass": "customInputs",
                    "name": 'timesheetDescription',
                    "type": 'text',
                    'dataValType':'valueText',
                    'value':'',  
                    "onValueChange": (e)=>{ functionRefs.changeHandler(e); },
                    'className': "browser-default",                   
                }
            },            
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'documentTypes',
                    'id': 'searchDocumentType',
                    'hasLabel': true,
                    'divClassName': 'col',
                    'name': "searchDocumentType",
                    "onSelectChange": (e)=>{ functionRefs.changeHandler(e); },
                    'label': localConstant.contract.SEARCH_DOCUMENTS,
                    "inputClass": "customInputs",
                    'type': 'select',
                    'colSize': 's3 pl-0',
                    //'className': "browser-default",
                    'optionName':'name',
                    'optionValue':'name',
                    'optionsList':[],
                    'defaultValue':" "
                    //'disabled': true,
                }
            },
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'documentSearchText',
                    'id': 'documentSearchText',
                    'hasLabel': true,
                    'divClassName': 'col',
                    'label': localConstant.contract.SEARCH_TEXT,
                    "type": 'text',
                    'dataValType':'valueText',
                    'value':'',  
                    'colSize': 's3 pl-0 ',
                    'className': "browser-default",
                    'inputClass': "customInputs textareaHeight",
                    "onValueChange": (e)=>{ functionRefs.changeHandler(e); },
                    'name': "documentSearchText",
                }
            },
           
            {
                "component": "Button",
                'text':localConstant.commonConstants.BTN_SEARCH,
                "properties": {
                    'id': 'timesheetSearchBtn',
                    'hasLabel': false, 
                    'type': 'Submit',
                    'disabled': false,
                    'className': "mr-2 mt-4x modal-close waves-effect waves-green btn col",
                }
            },
            {
                "component": "ButtonWithDiv",
                'text':localConstant.commonConstants.BTN_RESET,
                "properties": {
                    'id': 'timesheetResetBtn',
                    'hasLabel': false, 
                    'type': 'button',
                    'onClick':(e) => { functionRefs.resetAssignment(e); },
                    'disabled': false,
                    'className': "mr-2 mt-4x modal-close waves-effect waves-green btn col",
                }
            },

            // {
            //     "component": "ExportToCSV",
            //     'text':localConstant.commonConstants.EXPORT,
            //     "properties": {
            //         'csvExportClick':(e) => { functionRefs.csvExportClick(e); },
            //         'buttonClass':"mr-2 mt-4x modal-close waves-effect waves-green btn col",
            //         'csvClass':"displayNone",
            //         'filename':"Timesheet Search Data.csv",
            //     }
            // }
        ]
    };
};
