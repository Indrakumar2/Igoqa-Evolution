import { getlocalizeData } from '../../../../../utils/commonUtils';
import dateUtil from '../../../../../utils/dateUtil';
const localConstant = getlocalizeData();
export const HeaderData={
    "columnDefs": [
        {
            "headerName": localConstant.gridHeader.manageLifecycle.LIFECYCLE_DESCRIPTION,
            "headerTooltip":localConstant.gridHeader.manageLifecycle.LIFECYCLE_DESCRIPTION,
            "field": "name",
            "cellRenderer": "medalCellRenderer",
            "filter": "agDateColumnFilter",
            "filterParams": {
                "comparator": dateUtil.comparator,
                "inRangeInclusive": true,
                "browserDatePicker": true
              },
            "sort": "desc",
            "width": 250
        },
        {
            "headerName": localConstant.gridHeader.manageLifecycle.LONG_ESCRIPTION,
            "headerTooltip":localConstant.gridHeader.manageLifecycle.LONG_ESCRIPTION,
            "field": "description",
            "cellRenderer": "medalCellRenderer",
            "filter": "agTextColumnFilter",
            "width": 575
        },
        {
            "headerName": localConstant.gridHeader.manageLifecycle.HIDE_FOR_NEWFACILITY,
            "headerTooltip":localConstant.gridHeader.manageLifecycle.HIDE_FOR_NEWFACILITY,
            "field": "isAlchiddenForNewFacility",
            "cellRenderer": "medalCellRenderer",
            "filter": "agTextColumnFilter",
            "valueGetter":(params)=>{
                if(params.data.isAlchiddenForNewFacility===true){
                    return "Yes";
                }else{
                    return "No";
                }
            },
            "width": 168
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