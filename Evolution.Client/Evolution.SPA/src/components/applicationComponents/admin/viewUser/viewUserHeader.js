import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();
export const headerData = {
        userHeader: {
            "columnDefs": [
                {
                    "headerCheckboxSelectionFilteredOnly": true,
                    "checkboxSelection": true,
                    "suppressFilter": true,
                    "width": 50
                },
                {
                    "headerName":  localConstant.security.USER_NAME,
                    "headerTooltip": localConstant.security.USER_NAME,
                    "field": "userName",
                    "tooltipField":"userName",   
                    "filter": "agTextColumnFilter",
                    //"cellRenderer": "HyperLinkRenderer",
                    "width":180
                },
                {
                    "headerName": localConstant.security.LOGON_NAME,
                    "headerTooltip": localConstant.security.LOGON_NAME,
                    "field": "logonName",
                    "tooltipField":"logonName",   
                    "filter": "agTextColumnFilter",         
                    "width":200
                },
                {
                    "headerName": localConstant.security.EMAIL,
                    "headerTooltip": localConstant.security.EMAIL,
                    "field": "email",
                    "tooltipField":"email",   
                    "filter": "agTextColumnFilter",            
                    "width":200
                },
                {
                    "headerName": localConstant.security.DEFAULT_WORKFLOW_TYPE, //ITK Team asked this changes in Weekly Status Call on 13-02-2020
                    "headerTooltip": localConstant.security.DEFAULT_WORKFLOW_TYPE,
                    "field": "defaultCompanyUserType",
                    "tooltip":valueChange, 
                    "filter": "agTextColumnFilter",  
                    "valueGetter": valueChange,          
                    "width":250
                },  
                {
                    "headerName": localConstant.security.DEFAULTCOMAPNY,
                    "headerTooltip": localConstant.security.DEFAULTCOMAPNY,
                    "field": "companyName",
                    "tooltipField":"companyName",  
                    "filter": "agTextColumnFilter",            
                    "width":250
                },
                {
                    "headerName":localConstant.security.OFFICE,
                    "headerTooltip": localConstant.security.OFFICE,
                    "field": "companyOfficeName",
                    "tooltipField":"companyOfficeName",  
                    "filter": "agTextColumnFilter",            
                    "width":170
                }, 
                // {
                //     "headerName": "Culture",
                //     "field": "culture",
                //     "filter": "agTextColumnFilter",            
                //     "width":170
                // },       
                {
                    "headerName": localConstant.security.IS_ACTIVE,
                    "headerTooltip": localConstant.security.IS_ACTIVE,
                    "field": "isActive",
                    "tooltip":(params) => {
                        if (params.data.isActive === true) {
                            return "Yes";
                        } else {
                            return "No";
                        }
                    },  
                    "filter": "agTextColumnFilter",            
                    "width":150,
                    "valueGetter": (params) => {
                        if (params.data.isActive === true) {
                            return "Yes";
                        } else {
                            return "No";
                        }
                    }
                },   
                {
                    "headerName": "",
                    "field": "EditColumn",
                    "cellRenderer": "EditLink",
                    "buttonAction": "",
                    "cellRendererParams": {
                    },
                    "suppressFilter": true,
                    "suppressSorting": true,
                    "width": 50
                },
            ],
        "enableSelectAll":true,    
        "rowSelection": 'multiple',
        "searchable":true,
        "gridActions":false,
        "gridTitlePanel":true,
        "gridHeight":56,
        "pagination": true,
        "enableFilter": true,
        "clearFilter":true, 
        "enableSorting": true,
        }
};

function valueChange(value) {
    const { defaultCompanyUserType } = value.data;
    if (defaultCompanyUserType) {
        // Changes done as per ITK requirement (D731 issue2)
        const companyUserType=  defaultCompanyUserType.map(itratedValue => {
            if(itratedValue == localConstant.userTypeList.MICoordinator){
                return  itratedValue = localConstant.userTypeList.ITKCoordinator;
            } else if(itratedValue == "N/A"){
                return " ";
            } else {
                return itratedValue;
            }
        });
     return companyUserType.filter(s => s !== ' ').map(e => e).join(",");
    }
};