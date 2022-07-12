import { getlocalizeData } from '../../../../../utils/commonUtils';
import dateUtil from '../../../../../utils/dateUtil';
const localConstant = getlocalizeData();
export const HeaderData = {
    "columnDefs": [
        {
           
            "headerCheckboxSelectionFilteredOnly": true,
            "checkboxSelection": true,
            "suppressFilter": true,
            "width": 100
        },
        {
            "headerName":  localConstant.admin.referenceTypes.REFERENCE_TYPE,
            "headerTooltip": localConstant.admin.referenceTypes.REFERENCE_TYPE,
            "field": "name",
            "tooltipField":"name",   
            "filter": "agDateColumnFilter",
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              },
            "sort": "desc",
            "width":600
        },
        {
            "headerName":  localConstant.admin.referenceTypes.LANGUAGE_DESCRIPTION,
            "headerTooltip": localConstant.admin.referenceTypes.REFELANGUAGE_DESCRIPTIONRENCE_TYPE,
            "field": "languageDescription",
            "tooltipField":"languageDescription",   
            "filter": "agTextColumnFilter",
            "hide":false,
            "width":300
        },
        {
            "headerName": "",
            "field": "EditAssignmentReferenceTypes",
            "cellRenderer": "EditLink",
            "cellRendererParams": { },
            "suppressFilter": true,
            "suppressSorting": true,
            "width": 100
        },
    ],
    "enableSelectAll":true,
    "enableFilter": true,
    "enableSorting": true,
    "rowSelection": "multiple",
    "gridHeight": 60,
    "pagination": true,
    "searchable":true,
    "clearFilter":true, 
    "exportFileName":"AssignmentReferenceTypes"
};