import { getlocalizeData } from '../../../../utils/commonUtils';
import { headerData } from './headerData';
const localConstant = getlocalizeData();
export const defaultSupplierSearchField={
    supplierPONumber:'',
    supplierPODeliveryDate:'',
    supplierPOCompletedDate:'',
    supplierPOContractNumber:'',
    SupplierPoProjectNumber:'',
    supplierPOStatus:'O',
    supplierPOCustomerProjectName:'',
    searchDocumentType:'',
    documentSearchText:'',
    supplierPOContractHolderCompany:'',
    materialDescription:''
};
export const searchFilterWithFunctionRefs = (functionRefs) => {
    return {
        "component": "SearchFilter",
        "text": 'heading of the comp',
        "properties": {},
        "subComponents": [
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'companyList',
                    'id': 'supplierPOContractHolderCompany',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.supplierpo.CONTRACT_HOLDING_COMPANY,
                    "colSize": 's3',
                    "inputClass": "customInputs",
                    "name": 'supplierPOContractHolderCompany',
                    "type": 'select',
                    "dataType": 'select',
                    'optionsList':[],
                    'optionName':'companyName',
                    'optionValue':'id',
                    "onSelectChange": (e)=>{ functionRefs.changeHandler(e); },
                    "defaultValue":'',
                    "disabled":false                    
                }
            },
            {
                "component": "CustomInput",
                "properties": {
                    'id': 'materialDescription',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.supplierpo.MATERIAL_DESCRIPTION,
                    "onValueChange": (e) => { functionRefs.changeHandler(e); },
                    "type": 'text',
                    'dataValType':'valueText',
                    'value':'',
                    "colSize": 's3',
                    "inputClass": "customInputs",
                    "name": 'materialDescription',
                    "maxLength": 200
                }
            },
            {
                "component": "CustomInput",
                "properties": {
                    "hasLabel": true,
                    "isNonEditDateField": false,
                    'dataKey': 'supplierPODeliveryDate',
                    "label": localConstant.supplierpo.DELIVERY_DATE,
                    "labelClass": "customLabel",
                    "colSize": 's3 pl-0',
                    "dateFormat":localConstant.commonConstants.UI_DATE_FORMAT,
                    "type": 'date',
                    "name": 'supplierPODeliveryDate',
                    "autocomplete": "off",
                    "selectedDate": '',
                    "onDateChange": (e) => { functionRefs.deliveryDateChange(e); },
                    "shouldCloseOnSelect": true,
                }
            },
            
            {
                "component": "CustomInput",
                "properties": {
                    "hasLabel": true,
                    "isNonEditDateField": false,
                    'dataKey': 'supplierPOCompletedDate',
                    "label": localConstant.supplierpo.COMPLETED_DATE,
                    "labelClass": "customLabel",
                    "colSize": 's3 pl-0',
                    "dateFormat": localConstant.commonConstants.UI_DATE_FORMAT,
                    "type": 'date',
                    "name": 'supplierPOCompletedDate',
                    "autocomplete": "off",
                    "selectedDate": '',
                    "onDateChange": (e) => { functionRefs.completedDateChange(e); },
                    "shouldCloseOnSelect": true,
                }
            },
            {
                "component": "CustomInput",
                "properties": {
                    'id': 'supplierPONumber',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.supplierpo.SUPPLIER_PO,
                    "onValueChange": (e) => { functionRefs.changeHandler(e); },
                    "type": 'text',
                    'dataValType':'valueText',
                    'value':'',
                    "colSize": 's3',
                    "inputClass": "customInputs",
                    "name": 'supplierPONumber',
                    "maxLength": 150
                }
            },
            {
                "component": "CustomInput",
                "properties": {
                    'id': 'supplierPOContractNumber',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.supplierpo.CONTARCT_NUMBER,
                    "onValueChange": (e) => { functionRefs.changeHandler(e); },
                    "type": 'text',
                    'dataValType':'valueText',
                    'value':'',                    
                    "colSize": 's3' ,
                    "inputClass": "customInputs",
                    "name": 'supplierPOContractNumber',
                    "maxLength": 40,
                }
            },
            {
                "component": "CustomInput",
                "properties": {
                    'id': 'SupplierPoProjectNumber',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.supplierpo.PROJECT_NUMBER,
                    "onValueChange": (e) => { functionRefs.changeHandler(e); },
                    "type": 'text',
                    "dataType": 'numeric',
                    "valueType":'value',
                    "colSize": 's3 pl-0',
                    "inputClass": "customInputs",
                    "name": 'SupplierPoProjectNumber',
                    "maxLength": 40,
                    "value":''
                }
            },
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'supplierPOStatus',
                    'id': 'supplierPOStatus',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.supplierpo.STATUS,
                    "colSize": 's3 pl-0',
                    "inputClass": "customInputs",
                    "name": 'supplierPOStatus',
                    "type": 'select',
                    'optionsList': [],
                    'optionName': 'name',
                    'optionValue': 'value',
                    "onSelectChange": (e) => { functionRefs.changeHandler(e); },
                    
                    'optionSelecetLabel': 'Open',
                    'defaultValue': 'O'
                }
            },

            {
                "component": "CustomerAndCountrySearch",
                "properties": {
                    "colSize": 'col s3 pl-0',
                    "name": "supplierPOCustomerName",
                    "isSupplier":true
                }
            },
           
            {
                "component": "CustomInput",
                "properties": {
                    'id': 'supplierPOCustomerProjectName',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.supplierpo.CUSTOMER_PROJECT_NAME,
                    "onValueChange": (e) => { functionRefs.changeHandler(e); },
                    "type": 'text',
                    'dataValType':'valueText',
                    'value':'',
                    "colSize": 's3',
                    "inputClass": "customInputs",
                    "name": 'supplierPOCustomerProjectName',
                    "maxLength": 60,
                }
            },
            {
                "component": "InputWithPopUpSearch",
                "properties": {
                    "colSize": 'col s3 pl-0 supProChildDiv',
                    "label": localConstant.supplierpo.MAIN_SUPPLIER,
                    "headerData": headerData.supplierSearchHeader,
                    "name": "supplierPOMainSupplierName",
                    "searchModalTitle": localConstant.supplierpo.SUPPLIER_LIST,
                    "dataKey": "supplierList",
                    "type": "grid",
                    "gridRowData": null,
                    "defaultInputValue": "",
                    "onAddonBtnClick": (data) => { functionRefs.supplierPopupOpen(data); },
                    "onModalSelectChange": (data) => { functionRefs.fetchMainSupplier(data); },
                    "onInputBlur": (data) => { functionRefs.fetchMainSupplier(data); },
                    "onSubmitModalSearch": (data) => { functionRefs.getSelectedMainSupplier(data); },
                    "handleInputChange":(data)=>{functionRefs.getSelectedMainSupplier(data);}, // Added for Defect 866 Starts
                    "objectKeySelector": "supplierName",
                    "callBackFuncs":functionRefs.clearSearchRef,
                    "columnPrioritySort"  :[
                        {
                            "colId":'supplierName',
                            "sort":'asc'
                        }
                    ]  
                }
            },
            
            {
                "component": "InputWithPopUpSearch",
                "properties": {
                    "colSize": 'col s3 subSupplierDiv pl-0',
                    "label": localConstant.resourceSearch.SUB_SUPPLIER,
                    "headerData": headerData.subsupplierSearchHeader,
                    "name": "supplierPOSubSupplierName",
                    "searchModalTitle": localConstant.supplierpo.SUPPLIER_LIST,
                    "dataKey": "supplierList",
                    "type": "grid",
                    "gridRowData": null,
                    "defaultInputValue": "",
                    "onAddonBtnClick": (data) => { functionRefs.supplierPopupOpen(data); },
                    "onModalSelectChange": (data) => { functionRefs.fetchMainSupplier(data); },
                    "onInputBlur": (data) => { functionRefs.fetchMainSupplier(data); },
                    "onSubmitModalSearch": (data) => { functionRefs.getSelectedSubSupplier(data); },
                    "handleInputChange":(data)=>{functionRefs.getSelectedSubSupplier(data);},  // Added for Defect 866 Starts
                    "objectKeySelector": "supplierName",
                    "callBackFunc":functionRefs.clearSearchSubSupplierRef , 
                    "columnPrioritySort"  :[
                        {
                            "colId":'supplierName',
                            "sort":'asc'
                        }
                    ]   
                    
                }
            },
            
            {
                "component": "CustomInput",
                "properties": {
                    'dataKey': 'documentTypesData',
                    'id': 'searchDocumentType',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.supplierpo.SEARCH_DOCUMENTS,
                    "onSelectChange": (e) => { functionRefs.changeHandler(e); },
                    "type": 'select',
                    "dataType": 'select',
                    "optionName":"name",
                    "optionValue":"name",
                    "optionsList":[],
                    "colSize": 's4',
                    "inputClass": "customInputs",
                    "name": 'searchDocumentType',
                    "maxLength": 255,
                    "defaultValue":""
                }
            },
            {
                "component": "CustomInput",
                "properties": {
                    'id': 'documentSearchText',
                    'hasLabel': true,
                    "divClassName": "col",
                    "label": localConstant.supplierpo.SEARCH_TEXT,
                    "onValueChange": (e) => { functionRefs.changeHandler(e); },
                    "type": 'text',
                    'dataValType':'valueText',
                    'value':'',
                    "colSize": 's4 pl-0',
                    "inputClass": "customInputs",
                    "name": 'documentSearchText',
                    "maxLength": 200
                }
            },
           
            {
                "component": "ButtonWithDiv",
                'text': localConstant.commonConstants.BTN_SEARCH,
                "properties": {
                    'id': 'supplierPOSearchBtn',
                    'hasLabel': false,
                    'type': 'Submit',
                    'disabled': false,
                    'className': "mr-0 mt-4x modal-close waves-effect waves-green btn",
                }
            },
            {
                "component": "ButtonWithDiv",
                'text': localConstant.commonConstants.BTN_RESET,
                "properties": {
                    'id': 'supplierPOResetBtn',
                    'hasLabel': false,
                    'type': 'button',
                    'onClick':(e) => { functionRefs.resetSupplierpo(e); },
                    'disabled': false,
                    'className': "mr-0 mt-4x modal-close waves-effect waves-green btn",
                }
            },
            // {
            //     "component": "ExportToCSV",
            //     'text':localConstant.commonConstants.EXPORT,
            //     "properties": {
            //         'csvExportClick':(e) => { functionRefs.csvExportClick(e); },
            //         'buttonClass':"mr-0 mt-4x modal-close waves-effect waves-green btn",
            //         'csvClass':"displayNone",
            //         'filename':"Supplier List.csv",
            //     }
            // }
        ]
    };
};
