import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();

export const defaultFilterData = {
    assignmentNumber: "",
    assignmentStatus: "P",
    assignmentReference: "",
    customerName: "",
    contractHoldingCompanyCode: "",
    operatingCompanyCode: "",
    projectNumber: "",
    supplierPurchaseOrderNumber: "",
    technicalSpecialistName: "",
    searchDocumentType:"",
    documentSearchText:"",
    OperatingCompanyCoordinator:"",
    ContractHoldingCoordinator:"",
    workFlowTypeIn:"",
    materialDescription:'',
    assignmentCategory:'',
    assignmentSubCategory:'',
    assignmentService:''
};

export const searchFilterWithFunctionRefs = (functionRefs)=>{
    return {
        "component": "SearchFilter",
        "text": 'heading of the comp',
        "properties": {},
        "subComponents": [
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'assignmentNumber',
                    'id': 'assignmentNumber',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.assignments.ASSIGNMENT_NUMBER,
                    "onValueChange": (e)=> { functionRefs.changeHandler(e); } ,
                    "type": 'text',
                    "dataType": 'numeric',
                    "valueType":'value',
                    "colSize": 's2',
                    "inputClass": "customInputs",
                    "name": 'assignmentNumber',
                    'value':'',
                    "maxLength": 20,
                }
            },
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'assignmentStatus',
                    'id': 'assignmentStatus',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.assignments.ASSIGNMENT_STATUS,
                    "colSize": 's2',
                    "inputClass": "customInputs",
                    "name": 'assignmentStatus',
                    "type": 'select',
                    'optionsList':[],
                    'optionName':'name',
                    'optionValue':'description',
                    "onSelectChange": (e)=>{ functionRefs.changeHandler(e); },// DEF 181 fix
                    'defaultValue':'',
                    'optionSelecetLabel':'All'
                }
            }, 
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'assignmentReference',
                    'id': 'assignmentReference',
                    'hasLabel': true,
                    'divClassName': 'col',
                    'label': localConstant.assignments.ASSIGNMENT_REF,
                    'type': 'text',
                    'colSize': 's2',
                    'className': "browser-default",
                    'inputClass': "customInputs",
                    'dataValType':'valueText',
                    'value':'',
                    "onValueChange": (e)=>{ functionRefs.changeHandler(e); },
                    'name': "assignmentReference",
                }
            },
            {
                "component": "CustomerAndCountrySearch",
                "properties": {
                    "divClassName":'s6',
                    "isSupplier":true
                }
            }, 
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'companyList',
                    'id': 'contractHoldingCompanyCode',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.assignments.CONTRACT_HOLDING_COMPANY,
                    "colSize": 's4',
                    "inputClass": "customInputs",
                    "name": 'contractHoldingCompanyCode',
                    "type": 'select',
                    'optionsList':[],
                    'optionName':'companyName',
                    'optionValue':'id',
                    'defaultValue':'',
                    "onSelectChange": (e)=>{ functionRefs.changeHandler(e); },
                }
            },
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'ContractHoldingCoordinator',
                    'id': 'ContractHoldingCoordinator',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.assignments.CONTRACT_HOLDING_COORDINATOR,
                    "colSize": 's4',
                    "inputClass": "customInputs",
                    "name": 'ContractHoldingCoordinator',
                    "type": 'text',
                    'dataValType':'valueText',
                    'value':'',
                    "onValueChange": (e)=>{ functionRefs.changeHandler(e); },
                }
            }, 
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'companyList',
                    'id': 'operatingCompanyCode',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.assignments.OPERATING_COMPANY,
                    "colSize": 's4',
                    "inputClass": "customInputs",
                    "name": 'operatingCompanyCode',
                    "type": 'select',
                    'optionsList':[],
                    'defaultValue' : '',
                    'optionName':'companyName',
                    'optionValue':'id',
                    "onSelectChange": (e)=>{ functionRefs.changeHandler(e); },
                }
            }, 
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'OperatingCompanyCoordinator',
                    'id': 'OperatingCompanyCoordinator',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.assignments.OPERATING_COORDINATOR,
                    "labelClass": "CustomLabel",
                    "onValueChange": (e)=>{ functionRefs.changeHandler(e); },
                    "type": 'text',
                    "colSize": 's4',
                    'dataValType':'valueText',
                    'value':'',
                    "inputClass": "customInputs",
                    "name": 'OperatingCompanyCoordinator',
                }
            },    
             {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'projectNumber',
                    'id': 'projectNumber',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.project.PROJECT_NO,
                    "onValueChange": (e)=>{ functionRefs.changeHandler(e); },
                    "type": 'text',
                    "dataType": 'numeric',
                    "valueType":'value',
                    "colSize": 's4',
                    'value':'',
                    "inputClass": "customInputs",
                    "name": 'projectNumber',
                    "maxLength": 20,
                }
            },                
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'supplierPurchaseOrderNumber',
                    'id': 'supplierPurchaseOrderNumber',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.assignments.SUPPLIER_PURCHASE_ORDER,
                    "onValueChange": (e)=>{ functionRefs.changeHandler(e); },
                    "type": 'text',
                    "colSize": 's4',
                    'dataValType':'valueText',
                    'value':'',
                    "inputClass": "customInputs",
                    "name": 'supplierPurchaseOrderNumber',
                }
            }, 

            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'taxonomyCategory',
                    'id': 'assignmentCategory',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.assignments.CATEGORY,
                    "colSize": 's2',
                    "inputClass": "customInputs",
                    "name": 'assignmentCategory',
                    "type": 'select',
                    'optionsList': [],
                    'optionName': 'name',
                    'optionValue': "id",
                    'defaultValue': '',
                    "onSelectChange": (e) => { functionRefs.changeHandler(e); },
                    'disabled': false,
                }
            }, 
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'techSpecSubCategory',
                    'id': 'assignmentSubCategory',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.assignments.SUB_CATEGORY,
                    "colSize": 's2',
                    "inputClass": "customInputs",
                    "name": 'assignmentSubCategory',
                    "type": 'select',
                    'optionsList':[],
                    'optionName':'taxonomySubCategoryName',
                'optionValue':"id",
                    'defaultValue':'',
                    "onSelectChange": (e)=>{ functionRefs.changeHandler(e); },
                    'disabled': false,
                }
            }, 
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'techSpecServices',
                    'id': 'assignmentService',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.assignments.SERVICE,
                    "colSize": 's2',
                    "inputClass": "customInputs",
                    "name": 'assignmentService',
                    "type": 'select',
                    'optionsList':[],
                    'optionName':'taxonomyServiceName',
                'optionValue':"id",
                    'defaultValue':'',
                    "onSelectChange": (e)=>{ functionRefs.changeHandler(e); },
                    'disabled': false,
                }
            }, 
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'materialDescription',
                    'id': 'materialDescription',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.assignments.MATERIAL_DESCRIPTION,
                    "onValueChange": (e)=>{ functionRefs.changeHandler(e); },
                    "type": 'text',
                    "colSize": 's6',
                    'dataValType':'valueText',
                    'value':'',
                    "inputClass": "customInputs",
                    "name": 'materialDescription',
                    "maxLength": 200
                }
            }, 

            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'technicalSpecialistName',
                    'id': 'technicalSpecialistName',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.assignments.TECHNICAL_SPECIALIST,
                    "onValueChange": (e)=>{ functionRefs.changeHandler(e); },
                    "type": 'text',
                    "colSize": 's4',
                    'dataValType':'valueText',
                    'value':'',
                    "inputClass": "customInputs",
                    "name": 'technicalSpecialistName',
                   
                }
            },             
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'documentTypesData',
                    'id': 'searchDocumentType',
                    'hasLabel': true,
                    'divClassName': 'col',
                    'name': "searchDocumentType",
                    "onSelectChange": (e)=>{ functionRefs.changeHandler(e); },
                    'label': localConstant.commonConstants.SEARCH_DOCUMENTS,
                    "optionName":"name",
                    "optionValue":"name",
                    "optionsList":[],
                    'type': 'select',
                    'colSize': 's3',
                    "inputClass": "customInputs",
                    'className': "browser-default",
                    'defaultValue':""
                }
            },
            {
                "component": "CustomInput",
                "properties": {
                    'id': 'documentSearchText',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.commonConstants.SEARCH_TEXT,
                    "onValueChange": (e) => { functionRefs.changeHandler(e); },
                    "type": 'text',
                    "colSize": 's3',
                    'dataValType':'valueText',
                    'value':'',
                    "inputClass": "customInputs",
                    "name": 'documentSearchText',
                    "maxLength": 200
                }
            },            
            {
                "component": "ButtonWithDiv",
                'text':localConstant.commonConstants.BTN_SEARCH,               
                "properties": {
                    'id': 'assignmentSearchBtn',
                    'hasLabel': false,
                    'type': 'Submit',                  
                    'disabled': false,
                    'className': "mt-4x btn",
                }
            },
            {
                "component": "ButtonWithDiv",
                'text':localConstant.commonConstants.BTN_RESET,
                "properties": {
                    'id': 'assignmentResetBtn',
                    'hasLabel': false, 
                    "divClassName": "pl-0",                 
                    'type': 'button',
                    'onClick':(e) => { functionRefs.resetAssignment(e); },
                    'disabled': false,
                    'className': " mt-4x btn",
                }
            },
            // {
            //     "component": "ExportToCSV",
            //     'text':localConstant.commonConstants.EXPORT,
            //     "properties": {
            //         'csvExportClick':(e) => { functionRefs.csvExportClick(e); },
            //         'buttonClass':"mt-4x btn",
            //         'csvClass':"displayNone",
            //         'filename':"Timesheet Search Data.csv",
            //     }
            // }

        ]
    };
};
