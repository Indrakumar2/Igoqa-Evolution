import { getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData = () => {
    return {
        "myTask":{
            "columnDefs": [
            
                // {
                //     "headerName": localConstant.gridHeader.TASK_ID,
                //     "headerTooltip": localConstant.gridHeader.TASK_ID,
                //     "field": "myTaskId",
                //     "tooltipField": "myTaskId",
                //   //  "cellRenderer": "HyperLinkRenderer", //Commented for D363 CR Change
                //     "width":100,
                
                // }, // Fixes for D661
                {
                    "headerName": localConstant.gridHeader.TASK,
                    "headerTooltip": localConstant.gridHeader.TASK,
                    "field": "description",
                    "tooltipField": "description",
                    "filter": "agTextColumnFilter",
                    "cellRenderer": "HyperLinkRenderer",  //D363 CR Change
                    "width":380,
                    
                },
                {
                    "headerName": localConstant.gridHeader.TYPE,
                    "headerTooltip": localConstant.gridHeader.TYPE,
                    "field": "taskType",
                    "tooltipField": "taskType",
                    "filter": "agTextColumnFilter",
                    "width":220,
                    "valueGetter": (params) => {
                        const draftTypes = [
                            "RCRM_EditProfile", "TS_EditProfile", "TM_EditProfile"
                        ];
                        if (draftTypes.includes(params.data.taskType)) {
                            return "Draft Profile";
                        }
                        return params.data.taskType;
                    },
                    
                },
                {
                    "headerName": localConstant.gridHeader.CREATED_ON,
                    "headerTooltip": localConstant.gridHeader.CREATED_ON,
                    "field": "createdOn",
                    "tooltip": (params) => {
                        return moment(params.data.createdOn).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
                }, 
                "filter": "agDateColumnFilter",           
                    "width":150,
                    "cellRenderer": "DateComponent",
                    "cellRendererParams": {
                        "dataToRender": "createdOn"
                    },
                    "filterParams": {
                        "comparator": dateUtil.comparator,
                        "inRangeInclusive": true,
                        "browserDatePicker": true,
                    
                    }
                },
                // D363 CR Change
                {
                    "headerName": localConstant.gridHeader.COMPANY_NAME,
                    "headerTooltip": localConstant.gridHeader.COMPANY_NAME,
                    "field": "companyName",
                    "tooltipField": "companyName",
                    "width": 250,
                },
                // D363 CR Change
                {
                    "headerName": localConstant.gridHeader.REASSIGN,
                    "headerTooltip": localConstant.gridHeader.REASSIGN,
                    "field": "reassign",
                    "tooltipField": "reassign",
                    "cellRenderer": "EditLink",
                    "buttonAction": "",
                    "cellRendererParams": {
                        "displayLink":"Reassign"
                    },
                    "suppressFilter": true,
                    "suppressSorting": true,
                    "width": 150,
                
                } 
                
            ],
            
            "rowSelection": 'single',   
            "pagination": false,
            "enableFilter": true,
            "enableSorting": true,
            "gridHeight": 60, 
            "searchable": true,
            "clearFilter":true, 
        // "gridActions": true,
            "gridTitlePanel": true,
            "exportFileName":" My Tasks",
            "columnsFiltersToDestroy":[ 'createdOn' ]
        },
        "assignTask":{
            "columnDefs": [            
                {
                    "headerCheckboxSelectionFilteredOnly": true,
                    "checkboxSelection": true,
                    "suppressFilter": true,
                    "width": 50
                }, 
                {
                    "headerName": localConstant.gridHeader.USER,
                    "headerTooltip": localConstant.gridHeader.USER,
                    "field": "userName",
                    "tooltipField": "userName",
                    "filter": "agTextColumnFilter",
                    "width":430,
                    
                },                
            ],
            
            "rowSelection": 'single',   
            "pagination": true,
            "enableFilter": true,
            "enableSorting": true,
            "gridHeight": 60, 
            "searchable": true,
            "clearFilter":true,             
            "gridTitlePanel": true,                        
        }
    };
};