import { getlocalizeData, isEmpty } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData = (functionRefs) => {
    return {
        "ApprovedTaxonomyHeader": {
            "columnDefs": [
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.CATEGORY,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.CATEGORY,
                    "field": "taxonomyCategoryName",
                    "tooltipField": "taxonomyCategoryName",
                    "filter": "agTextColumnFilter",
                    "width": 150

                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.SUB_CATEGORY,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.SUB_CATEGORY,
                    "field": "taxonomySubCategoryName",
                    "tooltipField": "taxonomySubCategoryName",
                    "width": 150,
                    "filter": "agTextColumnFilter",
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.SERVICES,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.SERVICES,
                    "field": "taxonomyServices",
                    "tooltipField": "taxonomyServices",
                    "width": 120,
                    "filter": "agTextColumnFilter",
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.APPROVAL_STATUS,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.APPROVAL_STATUS,
                    "field": "approvalStatus",
                    "tooltipField": "approvalStatus",
                    "filter": "agTextColumnFilter",
                    "width": 180

                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.INTERVIEW,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.INTERVIEW,
                    "field": "interview",
                    "tooltipField": "interview",
                    "filter": "agTextColumnFilter",
                    "width": 180

                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.EFFECTIVE_FROM,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.EFFECTIVE_FROM,
                    "field": "fromDate",
                    "tooltip": (params) => {
                        return moment(params.data.fromDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                    "filter": "agDateColumnFilter",
                    "width": 180,
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "fromDate"
                    },
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                    }
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.EFFECTIVE_TO,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.EFFECTIVE_TO,
                    "field": "toDate",
                    "tooltip": (params) => {
                        return moment(params.data.toDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                    "filter": "agDateColumnFilter",
                    "width": 180,
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "toDate"
                    },
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                    }
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.COMMENTS,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.COMMENTS,
                    "field": "comments",
                    "tooltipField": "comments",
                    "filter": "agTextColumnFilter",
                    "width": 150

                },

        ],
        "enableFilter": true,
        "enableSorting": true,        
        "searchable": true,
        "gridActions": true,
        "gridHeight": 20,
        "clearFilter":true, 
        "rowSelection": "multiple",
        "columnsFiltersToDestroy":[ 'fromDate','toDate' ]
    },
    "InternalTrainingHeader": {
        "columnDefs": [
            {
                "checkboxSelection": true,
                "headerCheckboxSelectionFilteredOnly": true,
                "suppressFilter": true,
                "width": 40
            },
            {
                "headerName":localConstant.techSpec.gridHeaderConstants.INTERNAL_TRAINING,
                "headerTooltip":localConstant.techSpec.gridHeaderConstants.INTERNAL_TRAINING,
                "field": "typeName",
                "tooltip": (params) => {
                   
                    if(!isEmpty(params.data.technicalSpecialistInternalTrainingTypes)){
            
                    return  params.data.technicalSpecialistInternalTrainingTypes[0] && params.data.technicalSpecialistInternalTrainingTypes[0].typeName;
                }                    
            } ,
                "cellRenderer": "medalCellRenderer",
                "filter": "agTextColumnFilter",
                "width": 180,
                "valueGetter": (params) => {
                   
                        if(!isEmpty(params.data.technicalSpecialistInternalTrainingTypes)){
                
                        return  params.data.technicalSpecialistInternalTrainingTypes[0] && params.data.technicalSpecialistInternalTrainingTypes[0].typeName;
                    }                    
                } 

                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.DATE_OF_TRAINING,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.DATE_OF_TRAINING,
                    "field": "trainingDate",
                    "tooltip": (params) => {
                        return moment(params.data.trainingDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                    "width": 170,
                    "filter": "agDateColumnFilter",
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "trainingDate",
                    },
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                    }
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.EXPIRY,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.EXPIRY,
                    "field": "expiry",
                    "tooltip": (params) => {
                        return moment(params.data.expiry).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                    "width": 150,
                    "filter": "agDateColumnFilter",
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "expiry"
                    },
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                    }
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.SCORE_FIELD,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.SCORE_FIELD,
                    "field": "score",
                    "tooltipField": "score",
                    "filter": "agTextColumnFilter",
                    "width": 150

                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.COMPETENCY,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.COMPETENCY,
                    "field": "competency",
                    "tooltipField": "competency",
                    "filter": "agTextColumnFilter",
                    "width": 170

                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.NOTES,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.NOTES,
                    "field": "notes",
                    "tooltipField": "notes",
                    "filter": "agTextColumnFilter",
                    "width": 130

                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.TRAINING_DOCUMENT,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.TRAINING_DOCUMENT,
                    "field": "documentName",
                    "tooltipField": "documentName",
                    "filter": "agTextColumnFilter",
                    "width": 200,
                    "cellRenderer": "FileToBeDownload",
                    "cellRendererParams": {
                        "dataToRender": (params) => {
                            if (params.data.documents !== undefined) {
                                if (params.data.documents[0].recordStatus !== 'D') {
                                    return params.data.documents[0].documentName;
                                } else {
                                    return "Removed";
                                }
                            }
                        }
                    },

                },
                {
                    "headerName": "",
                    "field": "EditColumn",
                    "cellRenderer": "EditLink",
                    "cellRendererParams": {
                        "disableField": (e) => functionRefs.disableEditColumn(e)
                    },
                    "suppressFilter": true,
                    "suppressSorting": true,
                    "width": 50
                }
            ],
            "enableSelectAll":true,
            "enableFilter": true,
            "enableSorting": true,
            "searchable": true,
            "gridActions": true,
            "gridHeight": 20,
            "clearFilter":true, 
            "rowSelection": "multiple",
            "columnsFiltersToDestroy":[ 'trainingDate','expiry' ]
        },
        "CompetencyDetailHeader": {
            "columnDefs": [
                {
                    "checkboxSelection": true,
                    "headerCheckboxSelectionFilteredOnly": true,
                    "suppressFilter": true,
                    "width": 40
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.DVA_CHARTERS,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.DVA_CHARTERS,
                    "field": "trainingOrCompetencyTypeNames",
                    "tooltipField": "trainingOrCompetencyTypeNames",
                    "cellRenderer": "medalCellRenderer",
                    "filter": "agTextColumnFilter",
                    "width": 150,
                    // "valueGetter": (params) => {                //    
                    //     const { technicalSpecialistCompetencyTypes } = params.data;
                    //     if (Array.isArray(technicalSpecialistCompetencyTypes) &&
                    //     technicalSpecialistCompetencyTypes.length > 0) {
                    //         return technicalSpecialistCompetencyTypes.filter(ts => ts.typeName !== '').map(e => e.typeName).join(",");
                    //     } else {
                    //         return "";
                    //     }
                    // },                    
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.DURATION,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.DURATION,
                    "field": "duration",
                    "tooltipField": "duration",
                    "filter": "agTextColumnFilter",
                    "width": 120
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.DVA_EFFECTIVE_DATE,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.DVA_EFFECTIVE_DATE,
                    "field": "effectiveDate",
                    "tooltip": (params) => {
                        return moment(params.data.effectiveDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                    "width": 180,
                    "filter": "agDateColumnFilter",
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "effectiveDate"
                    },
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                    }
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.EXPIRY,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.EXPIRY,
                    "field": "expiry",
                    "tooltip": (params) => {
                        return moment(params.data.expiry).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                    "width": 120,
                    "filter": "agDateColumnFilter",
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "expiry"
                    },
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                    }
                },
                
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.SCORE_FIELD,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.SCORE_FIELD,
                    "field": "score",
                    "tooltipField": "score",
                    "filter": "agTextColumnFilter",
                    "width": 150

                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.COMPETENCY,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.COMPETENCY,
                    "field": "competency",
                    "tooltipField": "competency",
                    "filter": "agTextColumnFilter",
                    "width": 150

                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.NOTES,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.NOTES,
                    "field": "notes",
                    "tooltipField": "notes",
                    "filter": "agTextColumnFilter",
                    "width": 120

                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.DVA_DOCUMENT,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.DVA_DOCUMENT,
                    "field": "documentName",
                    "tooltipField": "documentName",
                    "filter": "agTextColumnFilter",
                    "width": 180,
                    "cellRenderer": "FileToBeDownload",
                    "cellRendererParams": {
                        "dataToRender": (params) => {
                            if (params.data.documents !== undefined) {
                                if (params.data.documents[0].recordStatus !== 'D') {
                                    return params.data.documents[0].documentName;
                                } else {
                                    return "Removed";
                                }
                            }
                        }
                    },

                },
                {
                    "headerName": "",
                    "field": "EditColumn",
                    "cellRenderer": "EditLink",
                    "cellRendererParams": {
                        "disableField": (e) => functionRefs.disableEditColumn(e)
                    },
                    "suppressFilter": true,
                    "suppressSorting": true,
                    "width": 50
                }
            ],
            "enableSelectAll":true,
            "enableFilter": true,
            "enableSorting": true,
            "searchable": true,
            "gridActions": true,
            "gridHeight": 20,
            "clearFilter":true, 
            "rowSelection": "multiple",
            "columnsFiltersToDestroy":[ 'effectiveDate','expiry' ]
        },
        "CustomerApprovedDetailHeader": {
            "columnDefs": [
                {
                    "checkboxSelection": true,
                    "headerCheckboxSelectionFilteredOnly": true,
                    "suppressFilter": true,
                    "width": 40
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.CUSTOMER_APPROVAL,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.CUSTOMER_APPROVAL,
                    "field": "customerName",
                    "tooltipField": "customerName",
                    "cellRenderer": "medalCellRenderer",
                    "filter": "agTextColumnFilter",
                    "width": 180
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.SAP_ID,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.SAP_ID,
                    "field": "customerSap",
                    "tooltipField": "customerSap",
                    "filter": "agTextColumnFilter",
                    "width": 130
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.COMMODITY_CODES,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.COMMODITY_CODES,
                    "field": "custCodes",
                    //"tooltipField": "custCodes",
                    "filter": "agTextColumnFilter",
                    "width": 250,
                    //Changes for D553 - Start
                    "valueGetter":(params)=>{
                        if(params.data && (params.data.custCodes === undefined || params.data.custCodes === null)){
                            return 'N/A';
                        }else{
                            return params.data.custCodes;
                        }
                    },
                    "tooltip":(params)=>{
                        if(params.data && (params.data.custCodes === undefined || params.data.custCodes === null)){
                            return 'N/A';
                        }else{
                            return params.data.custCodes;
                        }
                    },  
                    //Changes for D553 - End

                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.EFFECTIVE_FROM,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.EFFECTIVE_FROM,
                    "field": "fromDate",
                    "tooltip": (params) => {
                        return moment(params.data.fromDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                    "width": 150,
                    "filter": "agDateColumnFilter",
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "fromDate"
                    },
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                    }
                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.EXPIRY,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.EXPIRY,
                    "field": "toDate",
                    "tooltip": (params) => {
                        return moment(params.data.toDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                    "width": 130,
                    "filter": "agDateColumnFilter",
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "toDate"
                    },
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true
                    }
                },
               
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.COMMENTS,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.COMMENTS,
                    "field": "comments",
                    "tooltipField": "comments",
                    "filter": "agTextColumnFilter",
                    "width": 130

                },
                {
                    "headerName": localConstant.techSpec.gridHeaderConstants.CUSTOMER_APPROVED,
                    "headerTooltip": localConstant.techSpec.gridHeaderConstants.CUSTOMER_APPROVED,
                    "field": "documentName",
                    "tooltipField": "documentName",
                    "filter": "agTextColumnFilter",
                    "width": 180,
                    "cellRenderer": "FileToBeDownload",
                    "cellRendererParams": {
                        "dataToRender": (params) => {
                            if (params.data.documents !== undefined) {
                                if (params.data.documents[0].recordStatus !== 'D') {
                                    return params.data.documents[0].documentName;
                                } else {
                                    return "Removed";
                                }
                            }
                        }
                    },

                },
                {
                    "headerName": "",
                    "field": "EditColumn",
                    "cellRenderer": "EditLink",
                    "cellRendererParams": {
                        "disableField": (e) => ! functionRefs.enableEditColumn(e)
                    },
                    "suppressFilter": true,
                    "suppressSorting": true,
                    "width": 50
                }
            ],
            "enableSelectAll":true,
            "enableFilter": true,
            "enableSorting": true,
            "searchable": true,
            "gridActions": true,
            "gridHeight": 20,
            "clearFilter":true, 
            "rowSelection": "multiple",
            "columnsFiltersToDestroy":[ 'fromDate','toDate' ]
        }
    };
};