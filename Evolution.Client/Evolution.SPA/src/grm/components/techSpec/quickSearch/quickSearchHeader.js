import { getlocalizeData } from '../../../../utils/commonUtils';
import { required } from '../../../../utils/validator';
const localConstant = getlocalizeData();

const isTechSpecSelected = (selectedTechSpec,data,searchTechSpec) => {
    let istechspec = false;
    if (searchTechSpec) {
        if (selectedTechSpec && searchTechSpec && selectedTechSpec.length > 0 && searchTechSpec.length > 0) {
            selectedTechSpec.forEach(iteratedValue => {
                const techSpec = searchTechSpec.findIndex(x => x.epin === iteratedValue.epin);
                if (techSpec >= 0) {
                    istechspec = true;
                }
            });
        }
    }
    else{
        const techSpec = selectedTechSpec.findIndex(x => x.epin === data.epin);
        if (techSpec >= 0) {
            istechspec = true;
        }
    }
    return istechspec;
};

export const HeaderData = (searchData) => {
    return{
        "searchResourceHeader": {
            "columnDefs": [
                {
                    "headerName":localConstant.resourceSearch.SUPPLIER_LOCATION,
                    "headerTooltip":localConstant.resourceSearch.SUPPLIER_LOCATION,
                    "field": "location",
                    "tooltipField": "location",
                    "cellRenderer": 'agGroupCellRenderer',
                    "width": 230,
                    "cellClass": 'gridTextNoWrapEllipsis',
                },
                {
                    "headerCheckboxSelectionFilteredOnly": true,
                    "checkboxSelection": (params) => {
                        if(!required(params.node.data.location)){
                            return false;
                        }
                        else{
                            return true;
                        }
                    },  
                    "suppressFilter": true,
                    "cellRenderer": (params) => {
                        if(params.node.data.isSelected){
                            params.node.setSelected(params.node.data.isSelected);
                        }
                    },           
                    "width": 40
                }, 
                {
                    "field": "epin",
                    "headerName": localConstant.resourceSearch.EPIN,
                     /** SMN Defect ID - 345 */
                    "headerTooltip": localConstant.resourceSearch.EPIN,
                    "tooltipField": "epin",
                    "cellRenderer": "HyperLinkRenderer",
                    "cellRendererParams": {
                        "getInterCompanyCode":  () => searchData.getInterCompanyInfo()
                    },
                    "width": 100
                },
                {
                    "headerName": localConstant.resourceSearch.LASTNAME,
                    "headerTooltip": localConstant.resourceSearch.LASTNAME,
                    "field": "lastName",
                    "tooltipField": "lastName",
                    "filter": "agTextColumnFilter",
                     "width": 150
                },
                {
                    "headerName": localConstant.resourceSearch.FIRSTNAME,
                    "headerTooltip":localConstant.resourceSearch.FIRSTNAME,
                    "field": "firstName",
                    "tooltipField": "firstName",
                    "filter": "agTextColumnFilter",
                    //"cellRenderer": "HyperLinkRenderer",
                    "width": 150
                },
                {
                    "headerName": localConstant.resourceSearch.EMPLOYMENT_TYPE,
                    "headerTooltip":localConstant.resourceSearch.EMPLOYMENT_TYPE,
                    "field": "employmentType",
                    "tooltipField": "employmentType",
                    "filter": "agTextColumnFilter",
                    "width": 180
                },
                {
                    "headerName": localConstant.resourceSearch.SCHEDULE_STATUS,
                    "headerTooltip": localConstant.resourceSearch.SCHEDULE_STATUS,
                    "field": "scheduleStatus",
                    "tooltipField": "scheduleStatus",
                    "filter": "agTextColumnFilter",
                    "cellStyle": (params) =>{
                        if (params.value==='TBA') { 
                            return {  backgroundColor: '#d19917' };
                        }
                        else if (params.value==='PTO') { 
                            return {  backgroundColor: '#78a3e8' };
                        }
                        else if (params.value==='Available') { 
                            return { backgroundColor: '#70c19a' };
                        }
                        else if (params.value==='Confirmed') { 
                            return { backgroundColor: '#cc4141' };
                        }
                        else if (params.value==='Tentative') { 
                            return { backgroundColor: '#d3c954' };
                        }
                        else if (params.value === 'Pre-Assignment') {
                            return { backgroundColor: '#474e54' , color: '#ffffff' };
                        }
                        else {
                            return null;
                        }
                    },
                    "width": 200
                },
                {
                    "headerName": localConstant.resourceSearch.DISTANCE_FROM_VENDOR_MILES,
                    "headerTooltip": localConstant.resourceSearch.DISTANCE_FROM_VENDOR_MILES,
                    "field": "distanceFromVenderInMile",
                    "tooltipField": "distanceFromVenderInMile",
                    "filter": "agNumberColumnFilter",
                    "width": 250,
                    "valueGetter": (params) => {
                        if (params.data.distanceFromVenderInMile === -1) {
                            return null;
                        }
                        return params.data.distanceFromVenderInMile;
                    }
                },
                {
                    "headerName": localConstant.resourceSearch.DISTANCE_FROM_VENDOR_KM,
                    "headerTooltip":localConstant.resourceSearch.DISTANCE_FROM_VENDOR_KM,
                    "field": "distanceFromVenderInKm",
                    "tooltipField": "distanceFromVenderInKm",
                    "filter": "agNumberColumnFilter",
                    "width": 230
                },
                {
                    "headerName":localConstant.resourceSearch.SUPPLIER_,
                    "headerTooltip":localConstant.resourceSearch.SUPPLIER_,
                    "field": "isSupplier",
                    "tooltip": 
                    (params) => {
                        if(params.data.isSupplier===true){
                        
                            return "Yes";
                        }
                        else if(params.data.isSupplier===false){
                            return "No";
                        }
                    },
                    "filter": "agTextColumnFilter",
                    "width": 200,
                    "valueGetter":    (params) => {
                        if(params.data.isSupplier===true){
                        
                            return "Yes";
                        }
                        else if(params.data.isSupplier===false){
                            return "No";
                        }
                    },
                },
                // {
                //     "headerName": "Chevron Export",
                //     "field": "ExportColumn",
                //     "cellRenderer": "ExportLink",
                //     "isChevronExport": true,
                //     "buttonAction": "",
                //     "cellRendererParams": {
                //     },
                //     "suppressFilter": true,
                //     "suppressSorting": true,
                //     "width": 50
                // },
                // {
                //     "headerName": "Export",
                //     "field": "ExportColumn",
                //     "cellRenderer": "ExportLink",
                //     "isChevronExport": false,
                //     "buttonAction": "",
                //     "cellRendererParams": {
                //     },
                //     "suppressFilter": true,
                //     "suppressSorting": true,
                //     "width": 50
                // },
                {
                    "headerName":localConstant.resourceSearch.MOBILE_NO,
                    "headerTooltip":localConstant.resourceSearch.MOBILE_NO,
                    "field": "mobileNumber",
                    "tooltipField": "mobileNumber",
                    "filter": "agTextColumnFilter",
                    "width": 150
                },
                {
                    "headerName":localConstant.resourceSearch.EMAIL,
                    "headerTooltip":localConstant.resourceSearch.EMAIL,
                    "field": "email",
                    "tooltipField": "email",
                    "filter": "agTextColumnFilter",
                    "width": 150
                },
                {
                    "headerName": localConstant.resourceSearch.PROFILE_STATUS,
                    "headerTooltip":localConstant.resourceSearch.PROFILE_STATUS,
                    "field": "profileStatus",
                    "tooltipField": "profileStatus",
                    "filter": "agTextColumnFilter",
                    "width":150
                },
                {
                    "headerName": localConstant.resourceSearch.SUB_DIVISION,
                    "headerTooltip": localConstant.resourceSearch.SUB_DIVISION,
                    "field": "subDivision",
                    "tooltipField": "subDivision",
                    "filter": "agNumberColumnFilter",
                    "width": 150
    
                }
            ],
            "rowSelection": "multiple",
            "rowData":null,  
            "pagination": false,
            "enableFilter": true,
            "enableSorting": true,
            "gridHeight": 60,
            "searchable": true,
            "gridActions": true,
            "gridTitlePanel": true,
            "Grouping":true,
            "clearFilter":true, 
            "exportFileName":"Quick Search",
            "GroupingDataName":"resourceSearchTechspecInfos"
        },
        "supplierHeader": {
            "columnDefs": [
                {
                    "headerName": localConstant.resourceSearch.SUBSUPPLIER,
                    "headerTooltip": localConstant.resourceSearch.SUBSUPPLIER,
                    "field": "lastName",
                    "tooltipField": "lastName",
                    "filter": "agTextColumnFilter",
                    "width": 635
                },
                {
                    "headerName": localConstant.resourceSearch.SUB_SUPPLIER_LOCATION,
                    "headerTooltip": localConstant.resourceSearch.SUB_SUPPLIER_LOCATION,
                    "field": "lastName",
                    "tooltipField": "lastName",
                    "filter": "agTextColumnFilter",
                    "width": 600
                },
            ]
        },
        'cvOptionsHeader':{
    
            "columnDefs": [
                {
                    "checkboxSelection": true,
                    "headerCheckboxSelectionFilteredOnly": true,
                    "suppressFilter": true,
                    "width": 40,
                    "cellRenderer": (params) => {
                        if (params.node.rowIndex !== 8 && params.node.rowIndex !== 9) {
                            params.node.setSelected(true);
                        }
                        else{
                            params.node.setSelected(false);
                        }
                    }
                },
                {
                    "headerName": "Section",
                    "headerTooltip": "Section",
                    "field": "menu",
                    "tooltipField": "menu",
                    "width": 178
                },
                {
                    "headerName": "Sub-Section",
                    "headerTooltip": "Sub-Section",
                    "field": "subMenu",
                    "tooltipField": "subMenu",
                    "width": 151
                },
                {
                    "headerName": "Sub-Filter",
                    "headerTooltip": "Sub-Filter",
                    "field": "optional",
                    "tooltipField": "optional",
                    "width": 366
                },
            ],
            "enableFilter":false, 
            "enableSorting":false,
            "rowSelection": "multiple",
            "gridHeight":50,
            "pagination": false,
            "searchable":false,
            "clearFilter":false,
        },
    };
};
