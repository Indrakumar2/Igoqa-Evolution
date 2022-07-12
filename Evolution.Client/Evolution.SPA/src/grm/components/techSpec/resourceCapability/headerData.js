import { getlocalizeData ,isEmpty,isBoolean } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData = (functionRefs,fieldToHide) => {
    return {
        "LanguageCapabilitiesHeader": {
            "columnDefs": [
                {
                    "headerCheckboxSelectionFilteredOnly": true,
                    "checkboxSelection": true,
                    "suppressFilter": true,
                    "width": 30
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.LANGUAGE,
                    "headerTooltip":  localConstant.techSpec.gridHeaderConstants.LANGUAGE,
                    "field": "language",
                    "tooltipField": "language",
                    "filter": "agTextColumnFilter",
                    "width": 230
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.SPEAKING,
                    "headerTooltip":  localConstant.techSpec.gridHeaderConstants.SPEAKING,
                    "field": "speakingCapabilityLevel",
                    "tooltipField": "speakingCapabilityLevel",
                    "filter": "agTextColumnFilter",
                    "width": 230
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.WRITING,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.WRITING,
                    "field": "writingCapabilityLevel",
                    "tooltipField": "writingCapabilityLevel",
                    "filter": "agTextColumnFilter",
                    "width": 230
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.COMPREHENSION_CAPABILITY,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.COMPREHENSION_CAPABILITY,
                    "field": "comprehensionCapabilityLevel",
                    "tooltipField": "comprehensionCapabilityLevel",
                    "filter": "agTextColumnFilter",
                    "width": 300
                },
                {
                    "headerName": "",
                    "field": "EditLanguageCapability",

                    "cellRenderer": "EditLink",
                    "buttonAction":"",
                    "cellRendererParams": {
                        "disableField": (e) => functionRefs.enableEditColumn(e)
                    },
                    "suppressFilter": true,
                    "suppressSorting": true,
                    "width": 50
                }
            ],
            "enableSelectAll":true,
            "enableFilter": true,
            "enableSorting": true,
            "pagination": false,
            "gridHeight": 30,
            "rowSelection": 'multiple',
            "clearFilter":true, 
            "searchable":true ,
            "gridActions": true,
            "exportFileName":"Language Capabilities Details", 
        },
        "CertificateDetailsHeader": {
            "columnDefs": [
                {
                    "headerCheckboxSelectionFilteredOnly": true,
                    "checkboxSelection": true,
                    "suppressFilter": true,
                    "width": 30
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.CERTIFICATE_NAME,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.CERTIFICATE_NAME,
                    "field": "certificationName",
                    "tooltipField": "certificationName",
                    "filter": "agTextColumnFilter",
                    "width": 180
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.TYPE,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.TYPE,
                    "field": "type",
                    "tooltip": (params) => {  
                        if(isBoolean(params.data.isExternal)){ // def 774,#33    ---- D218 (Changes for 12-02-2020 Failed ALM Doc)
                            if (params.data.isExternal === true) {                        
                                return "External";
                            } else {
                                return "Internal";
                            }
                        } 
                        else{
                            return " ";
                        }
                    },
                    "filter": "agTextColumnFilter",
                    "width": 150,
                    "valueGetter": (params) => {  
                        if(isBoolean(params.data.isExternal)){ // def 774,#33 ---D218 (Changes for 12-02-2020 Failed ALM Doc)
                            if (params.data.isExternal === true) {                        
                                return "External";
                            } else {
                                return "Internal";
                            }
                        } 
                        else{
                            return " ";
                        }
                    },
                    "hide":fieldToHide
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.CERTIFICATE_ID,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.CERTIFICATE_ID,
                    "field": "certificateRefId",
                    "tooltipField": "certificateRefId",
                    "filter": "agTextColumnFilter",
                    "width": 190
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.CERTIFICATE_DETAILS,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.CERTIFICATE_DETAILS,
                    "field": "description",
                    "tooltipField": "description",
                    "filter": "agTextColumnFilter",
                    "width": 190
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.EFFECTIVE_DATE,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.EFFECTIVE_DATE,
                    "field": "effeciveDate",
                    "tooltip": (params) => {
                        return moment(params.data.effectiveDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                    "width": 170,
                    "filter": "agDateColumnFilter",
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "effeciveDate"
                    },
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                      }               
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.EXPIRY_DATE,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.EXPIRY_DATE,
                    "field": "expiryDate",
                    "tooltip": (params) => {
                        return moment(params.data.expiryDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                    "width": 170,
                    "filter": "agDateColumnFilter",
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "expiryDate"
                    },
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                      }
                },
                
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.VERIFICATION_STATUS,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.VERIFICATION_STATUS,
                    "field": "verificationStatus",
                    "tooltipField": "verificationStatus",
                    "filter": "agTextColumnFilter",
                    "width": 190,
                    "hide":fieldToHide
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.VERIFICATION_DATE,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.VERIFICATION_DATE,
                    "field": "verificationDate",
                    "tooltip": (params) => {
                        return moment(params.data.verificationDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                    "width": 170,
                    "filter": "agDateColumnFilter",
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "verificationDate"
                    },
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                      },
                      "hide":fieldToHide
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.VERIFIED_BY,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.VERIFIED_BY,
                    "field": "verifiedBy",
                    "tooltipField": "verifiedBy",
                    "filter": "agTextColumnFilter",
                    "width": 170,
                    "hide":fieldToHide
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.VERIFICATION_TYPE,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.VERIFICATION_TYPE,
                    "field": "verificationType",
                    "tooltipField": "verificationType",
                    "filter": "agTextColumnFilter",
                    "width": 170,
                    "hide":fieldToHide
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.NOTES,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.NOTES,
                    "field": "notes",
                    "tooltipField": "notes",
                    "filter": "agTextColumnFilter",
                    "width": 250,
                    "hide":fieldToHide
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.CERTIFICATE_UPLOAD,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.CERTIFICATE_UPLOAD,
                    "field": "documentName",
                    "tooltipField": "documentName",
                    "filter": "agTextColumnFilter",
                    "getRowNodeId": (params) =>{
                        return params.data.documentName;
                      },
                    "cellRenderer": "FileToBeDownload",                   
                    "cellRendererParams": {
                        "dataToRender": "documentName"
                        
                    },
                    "width": 250
                },
                {
                    "headerName":'Verfication Document',
                    "headerTooltip":'Verfication Document',
                    "field": "documentName",
                    "tooltipField": "documentName",
                    "filter": "agTextColumnFilter",
                    "objectAttributeName":'verificationDocuments',
                    "cellRenderer": "FileToBeDownload",
                    "cellRendererParams": {
                        "dataToRender": "documentName"

                    },
                    "width": 250,
                    "hide":fieldToHide
                },
                
                {
                    "headerName": "",
                    "field": "EditCertificateDetails",
                    "cellRenderer": "EditLink",
                    "buttonAction":"",
                    "cellRendererParams": {
                        "disableField": (e) => functionRefs.enableEditColumn(e)
                    },
                    "suppressFilter": true,
                    "suppressSorting": true,
                    "width": 50
                }
            ],
            "enableSelectAll":true,
            "enableFilter": true,
            "enableSorting": true,
            "pagination": false,
            "gridHeight": 30,
            "rowSelection": 'multiple'  ,
            "clearFilter":true, 
            "searchable":true,
            "columnsFiltersToDestroy":[ 'verificationDate','expiryDate','effeciveDate' ],
            "gridActions": true,
            "exportFileName":"Certificate Details",
        },
        "CommodityKnowledgeHeader": {
            "columnDefs": [
                {
                    "headerCheckboxSelectionFilteredOnly": true,
                    "checkboxSelection": true,
                    "suppressFilter": true,
                    "width": 35
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.COMMODITY,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.COMMODITY,
                    "field": "commodity",
                    "tooltipField": "commodity",
                    "filter": "agTextColumnFilter",
                   "cellRenderer": 'agGroupCellRenderer',
                   "cellClass": 'gridTextNoWrapEllipsis',
                    "width": 350
                },
                {
                    "headerName":localConstant.gridHeader.EQUIPMENT_KNOWLEDDGE,
                    "headerTooltip":localConstant.gridHeader.EQUIPMENT_KNOWLEDDGE,
                    "field": "value",
                    "tooltipField": "value",
                    "filter": "agTextColumnFilter",
                    "width": 400,
                     //"cellRenderer": "CustomStarRating",               
                },
                {
                    "headerName":localConstant.gridHeader.RATING,
                    "headerTooltip":localConstant.gridHeader.RATING,
                    "field": "rating",
                    "tooltipField": "rating",
                    "filter": "agTextColumnFilter",
                    "width": 250,
                     "cellRenderer": "CustomStarRating",               
                },
                {
                    "headerName": "",
                    "field": "EditCommodityDetails",
                    "tooltipField": "EditCommodityDetails",
                    "cellRenderer": "EditLink",
                    "buttonAction":"",
                    "cellRendererParams": {
                        "disableField": (e) => functionRefs.enableEditColumn(e)
                    },
                    "suppressFilter": true,
                    "suppressSorting": true,
                    "width": 50
                }
            ],
            "enableSelectAll":true,
            "enableFilter": true,
            "enableSorting": true,
            "pagination": false,
            "gridHeight": 30,
            "rowSelection": 'multiple' ,
            "searchable":true,
             "Grouping":true,
            "clearFilter":true, 
            "GroupingDataName":"equipmentKnowledge",
            "gridActions": true,
            "exportFileName":"Commodity / Equipment Knowledge",
        },
        "TrainingDetailsHeader": {
            "columnDefs": [
                {
                    "headerCheckboxSelectionFilteredOnly": true,
                    "checkboxSelection": true,
                    "suppressFilter": true,
                    "width": 30
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.TRAINING_NAME,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.TRAINING_NAME,
                    "field": "trainingName",
                    "tooltipField": "trainingName",
                    "filter": "agTextColumnFilter",
                    "width": 180
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.TYPE,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.TYPE,
                    "field": "type",
                    "tooltip": (params) => {                               
                        if(isBoolean(params.data.isExternal)){ //D222 (Changes for 12-02-2020 Failed ALM Doc)
                            if (params.data.isExternal === true) {                        
                                return "External";
                            } else {
                                return "Internal";
                            }
                        } 
                        else{
                            return " ";
                        } 
                    },
                    "filter": "agTextColumnFilter",
                    "width": 130,
                    "valueGetter": (params) => {    
                        if(isBoolean(params.data.isExternal)){ //D222 (Changes for 12-02-2020 Failed ALM Doc)
                            if (params.data.isExternal === true) {                        
                                return "External";
                            } else {
                                return "Internal";
                            }
                        } 
                        else{
                            return " ";
                        }                          
                        
                    },
                    "hide":fieldToHide
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.TRAINING_ID,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.TRAINING_ID,
                    "field": "trainingRefId",
                    "tooltipField": "trainingRefId",
                    "filter": "agTextColumnFilter",
                    "width": 130
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.DURATION,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.DURATION,
                    "field": "duration",
                    "tooltipField": "duration",
                    "filter": "agTextColumnFilter",
                    "width": 160
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.DATE_OF_TRAINING,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.DATE_OF_TRAINING,
                    "field": "effeciveDate",
                    "tooltip": (params) => {
                        return moment(params.data.effectiveDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                    "filter": "agDateColumnFilter",
                    "width": 170,
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "effeciveDate"
                    },
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                      }
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.EXPIRY_DATE,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.EXPIRY_DATE,
                    "field": "expiryDate",
                    "tooltip": (params) => {
                        return moment(params.data.expiryDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                    "filter": "agDateColumnFilter",
                    "width": 140,
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "expiryDate"
                    },
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                      }
                },
               
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.VERIFICATION_STATUS,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.VERIFICATION_STATUS,
                    "field": "verificationStatus",
                    "tooltipField": "verificationStatus",
                    "filter": "agTextColumnFilter",
                    "width": 180,
                    "hide":fieldToHide
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.VERIFICATION_DATE,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.VERIFICATION_DATE,
                    "field": "verificationDate",
                    "tooltip": (params) => {
                        return moment(params.data.verificationDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                    "filter": "agDateColumnFilter",
                    "width": 180,
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "verificationDate"
                    },
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                      },
                      "hide":fieldToHide
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.VERIFIED_BY,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.VERIFIED_BY,
                    "field": "verifiedBy",
                    "tooltipField": "verifiedBy",
                    "filter": "agTextColumnFilter",
                    "width":150,
                    "hide":fieldToHide
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.VERIFICATION_TYPE,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.VERIFICATION_TYPE,
                    "field": "verificationType",
                    "tooltipField": "verificationType",
                    "filter": "agTextColumnFilter",
                    "width": 180,
                    "hide":fieldToHide
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.NOTES,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.NOTES,
                    "field": "notes",
                    "tooltipField": "notes",
                    "filter": "agTextColumnFilter",
                    "width": 200,
                    "hide":fieldToHide
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.RESOURCE_TRAINING_DOCUMENT,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.RESOURCE_TRAINING_DOCUMENT,
                    "field": "documentName",
                    "tooltipField": "documentName",
                    "filter": "agTextColumnFilter",
                    "cellRenderer": "FileToBeDownload",
                    "cellRendererParams": {
                        "dataToRender": "documentName"
                    },
                    "width": 230
                },
                {
                    "headerName":'Verfication Document',
                    "headerTooltip":'Verfication Document',
                    "field": "documentName",
                    "tooltipField": "documentName",
                    "filter": "agTextColumnFilter",
                    "objectAttributeName":'verificationDocuments',
                    "cellRenderer": "FileToBeDownload",
                    "cellRendererParams": {
                        "dataToRender": "documentName"

                    },
                    "width": 250,
                    "hide":fieldToHide
                },
                {
                    "headerName": "",
                    "field": "EditTrainingDetails",
                    "cellRenderer": "EditLink",
                    "buttonAction":"",
                    "cellRendererParams": {
                        "disableField": (e) => functionRefs.enableEditColumn(e)
                    },
                    "suppressFilter": true,
                    "suppressSorting": true,
                    "width": 50
                }
            ],
            "enableSelectAll":true,
            "enableFilter": true,
            "enableSorting": true,
            "pagination": false,
            "gridHeight": 30,
            "rowSelection": 'multiple'  ,
            "clearFilter":true, 
            "searchable":true,
            "columnsFiltersToDestroy":[ 'verificationDate','expiryDate','effeciveDate' ],
            "gridActions": true,
            "exportFileName":"Training Details",  
        }, 
        "IntertekWorkHistoryReportHeader": {
            "columnDefs": [ 
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.INTERTEK_WORK_HIS_ASSIGNEMT_ID,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.INTERTEK_WORK_HIS_ASSIGNEMT_ID,
                    "field": "assignmentNumber",
                    "tooltipField": "assignmentNumber",
                    "filter": "agTextColumnFilter",
                    "width": 180
                }, {
                    "headerName":localConstant.techSpec.gridHeaderConstants.INTERTEK_WORK_HIS_PROJECT_NUMBER,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.INTERTEK_WORK_HIS_PROJECT_NUMBER,
                    "field": "projectNumber",
                    "tooltipField": "projectNumber",
                    "filter": "agTextColumnFilter",
                    "width": 180
                }, {
                    "headerName":localConstant.techSpec.gridHeaderConstants.INTERTEK_WORK_HIS_CLIENT,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.INTERTEK_WORK_HIS_CLIENT,
                    "field": "client",
                    "tooltipField": "client",
                    "filter": "agTextColumnFilter",
                    "width": 180
                }, {
                    "headerName":localConstant.techSpec.gridHeaderConstants.INTERTEK_WORK_HIS_INSPECT_EQUIPMENT,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.INTERTEK_WORK_HIS_INSPECT_EQUIPMENT,
                    "field": "inspectedEquipment",
                    "tooltipField": "inspectedEquipment",
                    "filter": "agTextColumnFilter",
                    "width": 180
                }, {
                    "headerName":localConstant.techSpec.gridHeaderConstants.INTERTEK_WORK_HIS_SUPPLIER_NAME,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.INTERTEK_WORK_HIS_SUPPLIER_NAME,
                    "field": "supplierName",
                    "tooltipField": "supplierName",
                    "filter": "agTextColumnFilter",
                    "width": 180
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.INTERTEK_WORK_HIS_SUPPLIER_LOCATION,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.INTERTEK_WORK_HIS_SUPPLIER_LOCATION,
                    "field": "location",
                    "tooltipField": "location",
                    "filter": "agTextColumnFilter",
                    "width": 180,
                    "valueGetter": (params) => {
                        return ((!isEmpty(params.data.supplierCity)?( params.data.supplierCity + ','):'') + (!isEmpty(params.data.supplierCounty)?( params.data.supplierCounty + ','):'') + (!isEmpty(params.data.supplierPostalCode)?( params.data.supplierPostalCode + ','):'') + (!isEmpty(params.data.supplierCountry)?( params.data.supplierCountry):'') );
                     
                    },
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.INTERTEK_WORK_HIS_CATEGORY,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.INTERTEK_WORK_HIS_CATEGORY,
                    "field": "category",
                    "tooltipField": "category",
                    "filter": "agTextColumnFilter",
                    "width": 180
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.INTERTEK_WORK_HIS_SUB_CATEGORY,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.INTERTEK_WORK_HIS_SUB_CATEGORY,
                    "field": "subCategory",
                    "tooltipField": "subCategory",
                    "filter": "agTextColumnFilter",
                    "width": 180
                },
                {
                    "headerName":localConstant.techSpec.gridHeaderConstants.INTERTEK_WORK_HIS_SERVICES,
                    "headerTooltip":localConstant.techSpec.gridHeaderConstants.INTERTEK_WORK_HIS_SERVICES,
                    "field": "service",
                    "tooltipField": "service",
                    "filter": "agTextColumnFilter",
                    "width": 180
                },
            ],
            "enableSelectAll":false,
            "enableFilter": true,
            "enableSorting": true,
            "pagination": true,
            "gridHeight": 30, 
            "clearFilter":true, 
            "searchable":true, 
            "gridActions": true,
            "exportFileName":"Intertek Work History Report",  
        }
    };
};