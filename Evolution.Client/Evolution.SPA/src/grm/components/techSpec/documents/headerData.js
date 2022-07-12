import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const  HeaderData= (functionRefs) => { //SystemRole based UserType relevant Quick Fixes 
    return{
        "DocumentsHeader":{
                "columnDefs": [
                    {
                        "checkboxSelection": true,
                        "headerCheckboxSelectionFilteredOnly": true,
                        "suppressFilter": true,
                        "width": 40
                    },
                    {
                        "headerName": localConstant.gridHeader.NAME ,
                        "headerTooltip":  localConstant.gridHeader.NAME ,
                        "field": "documentName",
                        "filter": "agTextColumnFilter",
                        "editable":  (params) => {              
                            if (params.data.isFileUploaded) {
                                return true;
                            } else if (!params.data.isFileUploaded) {
                                return false;
                            }
                        }, 
                        //"cellRenderer": "FileToBeOpen",
                        "cellRendererParams": {
                            "dataToRender": "documentName"
                        }
                    },
                    {
                        "headerName": "<span class='mandate'>" + localConstant.gridHeader.TYPE + "</span>",
                        "headerTooltip": localConstant.gridHeader.TYPE,
                        "field": "documentType",
                        "filter": "agTextColumnFilter",
                        "cellRenderer": "SelectDocumentType",
                        "width": 250,
                        "documentModuleName":'TechSpecDocument'
                    },
                    {
                        "headerName":localConstant.gridHeader.SIZE,
                        "headerTooltip": localConstant.gridHeader.SIZE,
                        "field": "documentSize",
                        "filter": "agTextColumnFilter",
                        "width": 130
                    },
                    {
                        "headerName": localConstant.modalConstant.UPLOADED_DATE,
                        "field": "createdOn",
                        "filter": "agDateColumnFilter",           
                        "cellRenderer": "DateComponent",
                        "cellRendererParams": {
                            "dataToRender": "createdOn"
                        },
                        "tooltip": (params) => {
                            return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                    }, 
                        "headerTooltip": localConstant.modalConstant.UPLOADED_DATE,
                        "width": 150,
                        "filterParams": {
                            "comparator": dateUtil.comparator,
                            "inRangeInclusive": true,
                            "browserDatePicker": true,
                        }
                    },
                    // {
                    //     "headerName": localConstant.modalConstant.VISIBLE_TO_CUSTOMER,
                    //     "field": "isVisibleToCustomer",
                    //     "filter": "agTextColumnFilter",
                    //     "width": 120,
                    //     "headerTooltip": localConstant.modalConstant.VISIBLE_TO_CUSTOMER,
                    //     "valueGetter": (params) => {
                    //         if (params.data.isVisibleToCustomer === true) {
                    //             return "Yes";
                    //         } else if (params.data.isVisibleToCustomer === false) {
                    //             return "No";
                    //         }
                    //     }
                    // },
                    {
                        "headerName":localConstant.modalConstant.VISIBLE_TO_TS,
                        "field": "isVisibleToTS",
                        "filter": "agTextColumnFilter",
                        "width": 130,
                        "headerTooltip": localConstant.modalConstant.VISIBLE_TO_TS,
                        "valueGetter": (params) => {
                            if (params.data.isVisibleToTS) {
                                return "Yes";
                            } else {
                                return "No";
                            }
                        }
                    },
                    // {
                    //     "headerName": "",
                    //     "field": "EditTechSpecDocuments",
                    //     "cellRenderer": "EditLink",
                    //     "cellRendererParams": { 
                    //         "disableField": (e) => functionRefs.enableEditColumn(e) //SystemRole based UserType relevant Quick Fixes 
                    //     },
                    //     "suppressFilter": true,
                    //     "suppressSorting": true,
                    //     "width": 50
                    // },
                    {
                        "headerName": '',           
                        "field": "documentName",      //Commented for D653 #11 issue (ref by 12-03-2020 ALM Doc ) enabledocument               
                        "tooltipField": "documentName",           
                        "cellRenderer": "FileToBeOpen",//Commented for D653 #11 issue (ref by 12-03-2020 ALM Doc ) enabledocument
                        "suppressFilter": true,
                        "suppressSorting": true,
                        "cellRendererParams": {
                            "dataToRender": "documentName"
                         },//Commented for D653 #11 issue (ref by 12-03-2020 ALM Doc ) enabledocument
                        "width": 50
                    }
                ],
                "enableFilter": true,
                "enableSorting": true,
                "pagination": true,
                "searchable":true,
                "enableSelectAll":true,
                "rowSelection": "multiple",
                "gridHeight": 45,
                "clearFilter":true, 
                "columnsFiltersToDestroy":[ 'createdOn' ]
            },
    };
};
