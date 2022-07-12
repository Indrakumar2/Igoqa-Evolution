import { getlocalizeData, isEmpty, isEmptyOrUndefine } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData = (functionRefs) => {
    return{
    "HeaderDataWorkHistoryDetails": {
        "columnDefs": [
            {
                "headerCheckboxSelectionFilteredOnly": true,
                "checkboxSelection": true,
                "suppressFilter": true,
                "width": 40
                
            },
            {
                "headerName":  localConstant.techSpec.gridHeaderConstants.CUSTOMER_COMPANY,
                "headerTooltip":  localConstant.techSpec.gridHeaderConstants.CUSTOMER_COMPANY,
                "field": "clientName",
                "tooltipField": "clientName",
                "filter": "agTextColumnFilter", 
                "width": 200
               
            },
            {
                "headerName":  localConstant.techSpec.gridHeaderConstants.PROJECT_NAME,
                "headerTooltip":  localConstant.techSpec.gridHeaderConstants.PROJECT_NAME,
                "field": "projectName",
                "tooltipField": "projectName",
                "filter": "agTextColumnFilter",
                "width": 150
                
            },
            {
                "headerName":localConstant.techSpec.gridHeaderConstants.JOB_TITLE,
                "headerTooltip":localConstant.techSpec.gridHeaderConstants.JOB_TITLE,
                "field": "jobTitle",
                "tooltipField": "jobTitle",
                "filter": "agTextColumnFilter",
                "width": 130
               
            },
            {
                "headerName": localConstant.techSpec.gridHeaderConstants.CURRENT_COMPANY,
                "headerTooltip": localConstant.techSpec.gridHeaderConstants.CURRENT_COMPANY,
                "field": "isCurrentCompany",
                "tooltip": (params) => {                               
                    if (params.data.isCurrentCompany === true) {                        
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "filter": "agTextColumnFilter",
                "valueGetter": (params) => {                               
                    if (params.data.isCurrentCompany === true) {                        
                        return "Yes";
                    } else {
                        return "No";
                    }
                },
                "width": 180
               
            },
            {
                "headerName":localConstant.techSpec.gridHeaderConstants.FROM,
                "headerTooltip":localConstant.techSpec.gridHeaderConstants.FROM,
                "field": "fromDate",
                "tooltip": (params) => {
                    return moment(params.data.fromDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
            }, 
                "filter": "agDateColumnFilter",
                "width": 100,
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
                "headerName": localConstant.techSpec.gridHeaderConstants.TO,
                "headerTooltip": localConstant.techSpec.gridHeaderConstants.TO,
                "field": "toDate",
                "tooltip": (params) => {
                    return moment(params.data.toDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
            }, 
                "filter": "agDateColumnFilter",
                "width": 100,
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
                "headerName": localConstant.techSpec.gridHeaderConstants.DESCRIPTION,
                "headerTooltip": localConstant.techSpec.gridHeaderConstants.DESCRIPTION,
                "field": "description",
                //"tooltipField": "description",
                "filter": "agTextColumnFilter",
                // "cellRenderer":function(params) {
                //     return params.data.description;
                // }, ---- //commented cellRenderer property for ITK D1375
                 //D1187(#Issue2)
                 "tooltip": (params) => {   
                    const parser = new DOMParser();
	                const doc = parser.parseFromString( (isEmptyOrUndefine(params.data.description)? '' : params.data.description ), 'text/html');//def 1395 fix                            
                    return doc.body.innerText;
                 },
                  //D1187(#Issue2)
                  //Addded Value Getter property for ITK D1375
                  "valueGetter": (params) => {                             
                    const parser = new DOMParser();
	                const doc = parser.parseFromString( (isEmptyOrUndefine(params.data.description)? '' : params.data.description ) , 'text/html'); //def 1395 fix                          
                    return doc.body.innerText;
                },
                "width": 170
               
            },
            {
                "headerName":localConstant.techSpec.gridHeaderConstants.RESPONSIBILITY,
                "headerTooltip":localConstant.techSpec.gridHeaderConstants.RESPONSIBILITY,
                "field": "responsibility",
                "tooltipField": "responsibility",
                "filter": "agTextColumnFilter",
                "width": 150
                
            },

            {
                "headerName":localConstant.techSpec.gridHeaderConstants.NO_OF_YEARS,
                "headerTooltip":localConstant.techSpec.gridHeaderConstants.NO_OF_YEARS,
                "field": "noofYearsExp",
                "tooltip": getNoofYearsExp,
                "filter": "agTextColumnFilter",
                "width": 230,
                "valueGetter": getNoofYearsExp,
            },
            {
                "headerName": "",
                "field": "EditColumn",
                "cellRenderer": "EditLink",
                "buttonAction":"",
                "cellRendererParams": {
                    "disableField": (e) => functionRefs.enableEditColumn(e)
                },
                "suppressFilter": true,
                "suppressSorting": true,
                "width": 100
            }
        ],
        "enableSelectAll":true,
        "enableFilter": true,
        "enableSorting": true,
        "pagination": false,
        "gridHeight": 20,
        "searchable":true,
        "gridActions": true,
        "clearFilter":true, 
        "rowSelection": "multiple",
        "columnsFiltersToDestroy":[ 'fromDate','toDate' ]
    },
    "HeaderDataEducationalSummary": {
        "columnDefs": [
            {
                "headerCheckboxSelectionFilteredOnly": true,
                "checkboxSelection": true,
                "suppressFilter": true,
                "width": 40
                
            },
            {
                "headerName":  localConstant.techSpec.gridHeaderConstants.EDUCATION_QUALIFICATION,
                "headerTooltip":  localConstant.techSpec.gridHeaderConstants.EDUCATION_QUALIFICATION,
                "field": "qualification",
                "tooltipField": "qualification",
                "filter": "agTextColumnFilter",
                "width": 250
                
            },
            {
                "headerName": localConstant.techSpec.gridHeaderConstants.UNIVERSITY,
                "headerTooltip": localConstant.techSpec.gridHeaderConstants.UNIVERSITY,
                "field": "institution",
                "tooltipField": "institution",
                "filter": "agTextColumnFilter",
                "width": 280
                
            },
            {
                "headerName": localConstant.techSpec.gridHeaderConstants.DATE_GAINED,
                "headerTooltip": localConstant.techSpec.gridHeaderConstants.DATE_GAINED,
                "field": "toDate",
                "tooltip": (params) => {
                    return moment(params.data.toDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
            }, 
                "filter": "agDateColumnFilter",
                "width": 150,
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
                "headerName":  localConstant.techSpec.gridHeaderConstants.QUALIFICATION_ATTACHMENT,
                "headerTooltip":  localConstant.techSpec.gridHeaderConstants.QUALIFICATION_ATTACHMENT,
                "field": "documentName",
                "tooltipField": "documentName",
                "filter": "agTextColumnFilter",
                "width": 250,            
                "cellRenderer": "FileToBeDownload",
                "cellRendererParams": {
                    "dataToRender":'Educational Summary Document' 
                },   
            },
           
            {
                "headerName": "",
                "field": "EditColumn",
                "cellRenderer": "EditLink",
                "buttonAction":"",
                "cellRendererParams": {
                    "disableField": (e) => functionRefs.enableEditColumn(e)
                },
                "suppressFilter": true,
                "suppressSorting": true,
                "width": 70
            }
        ],
        "enableSelectAll":true,
        "enableFilter": true,
        "enableSorting": true,
        "gridHeight": 20,
        "gridActions": true,
        "rowSelection": "multiple",
        "clearFilter":true, 
        "searchable":true,
        "columnsFiltersToDestroy":[ 'toDate' ]
    }
};
};

function getNoofYearsExp(params) {
    let toDate =params.data.toDate;
    if(isEmpty(toDate) && params.data.isCurrentCompany === true){ //D1130 Issue 5
        toDate = moment();
    }
    if(!isEmpty(params.data.fromDate) && !isEmpty(toDate)){
        const from = moment([
            moment(params.data.fromDate).year(), moment(params.data.fromDate).month(), moment(params.data.fromDate).date()
        ]);
        const to = moment([
            moment(toDate).year(), moment(toDate).month(), moment(toDate).date()
        ]);
        const total = (to.diff(from, 'years') + "yrs").concat(" " + to.diff(from, 'months') % 12 + "months");
            return total;
    } else {
        return params.data.noofYearsExp;
    }
};
