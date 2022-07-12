import { isUndefined,getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';

const localConstant = getlocalizeData();
export const HeaderData = {

    "columnDefs": [
       
        {
            "headerName": localConstant.timesheet.REFERENCE_TYPE,
            "field": "reportNo",
            "tooltipField":"reportNo",    
            "filter": "agTextColumnFilter",
            "cellRenderer": "timesheetAnchor",
            "width":440
        },
        {
            "headerName":  localConstant.timesheet.REFERENCE_VALUE,
            "field": "technicalSpecialist",
            "tooltipField":"technicalSpecialist",    
            "filter": "agTextColumnFilter",
            "width":600
        },
                
    ],
    
    "rowSelection": 'single',   
    "pagination": true,
    "enableFilter": true,
    "enableSorting": true,
    "gridHeight": 60,
    "searchable": true,
    "gridActions": true,
    "gridTitlePanel": true
};