import { getlocalizeData } from '../../../../../utils/commonUtils';
import dateUtil from '../../../../../utils/dateUtil';
const localConstant = getlocalizeData();
export const HeaderData={
    "columnDefs": [
        {
            "headerName": localConstant.admin.manageLifecycle.ASSIGNMENT_STATUS,
            "headerTooltip":localConstant.admin.manageLifecycle.ASSIGNMENT_STATUS,
            "field": "name",
            "cellRenderer": "medalCellRenderer",
            "filter": "agDateColumnFilter",
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              },
            "sort": "desc",
            "width": 975
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
            "width": 100
        }
    ],
    "enableSelectAll":true,
    "enableSorting": true,
    "pagination": false,
    "searchable": false,
    "gridActions": true,
    "gridTitlePanel": false,
    "gridHeight": 25,
    "clearFilter":true, 
    "rowSelection": "multiple",
   
};