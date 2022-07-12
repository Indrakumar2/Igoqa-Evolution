import { isUndefined,getlocalizeData } from '../../../../utils/commonUtils';
import dateUtil from '../../../../utils/dateUtil';
import moment from 'moment';
const localConstant = getlocalizeData();
export const HeaderData = (functionRefs) => {
    return {
        "columnDefs": [  
            {
                "headerCheckboxSelectionFilteredOnly": true,
                "checkboxSelection": true,
                "suppressFilter": true,
                "width": 40
            },     
            {
                "headerName": localConstant.visit.SUPPLIER_PERFORMANCE_TYPE,
                "headerTooltip": localConstant.visit.SUPPLIER_PERFORMANCE_TYPE,
                "field": "supplierPerformance",
                "tooltipField":"supplierPerformance",    
                "filter": "agTextColumnFilter",
                "width":300
            },
            {
                "headerName":  localConstant.visit.NCR_REFERENCE,
                "headerTooltip": localConstant.visit.NCR_REFERENCE,
                "field": "ncrReference",
                "tooltipField":"ncrReference",    
                "filter": "agTextColumnFilter",
                "width":364
            },
            {
                "headerName":  localConstant.visit.NCR_CLOSE_OUTDATE,
                "headerTooltip": localConstant.visit.NCR_CLOSE_OUTDATE,
                "field": "ncrCloseOutDate",
                "tooltip": (params) => {
                    return moment(params.data.ncrCloseOutDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
            }, 
                //"filter": "agTextColumnFilter",
                "filter": "agDateColumnFilter",
                "cellRenderer": "DateComponent",
            
                "cellRendererParams": {
                    "dataToRender": "ncrCloseOutDate"
                },
                "width":350
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
                "width": 50
            }      
        ],
        
        "enableSorting":true, 
        "pagination": false,    
        "enableFilter": true,
        "gridHeight":54,    
        "rowSelection":"multiple"
    };
};