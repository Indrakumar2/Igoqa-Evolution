import { getlocalizeData } from '../../../utils/commonUtils';
const localConstant = getlocalizeData();
export const HeaderData = {
    "columnDefs": [
        {
            "headerName": localConstant.companySearch.FULL_USE,
            "field": "isFullUse",  
            "headerTooltip":localConstant.companySearch.FULL_USE,                 
            "width":200,
            "sort": "desc",
            "tooltip":(params) => {
                if (params.data.isFullUse === true) {
                    return "Yes";
                } else {
                    return "No";
                }
            },
            "valueGetter": (params) => {
                if (params.data.isFullUse === true) {
                    return "Yes";
                } else {
                    return "No";
                }
            }
        },        
        {
            "headerName": localConstant.companySearch.ACTIVE,
            "field": "isActive",                        
            "width":200,
            "tooltip":(params) => {
                if (params.data.isActive === true) {
                    return "Yes";
                } else {
                    return "No";
                }
            },
            "headerTooltip":localConstant.companySearch.ACTIVE,
            "valueGetter": (params) => {
                if (params.data.isActive === true) {
                    return "Yes";
                } else {
                    return "No";
                }
            }
        },        
        {
            "headerName": localConstant.companySearch.CODE,
            "field": "companyCode",
            "cellRenderer": "companyAnchorRenderer",   
            "headerTooltip":localConstant.companySearch.CODE,         
            "width":200,
            "tooltipField":"companyCode"
        },
        {
            "headerName": localConstant.companySearch.COMPANY_NAME,
            "field": "companyName",  
            "headerTooltip":localConstant.companySearch.COMPANY_NAME,
            "sort": "asc",
            "width":715,
            "tooltipField":"companyName"
        },                     
    ],
    "pagination": true,
    "enableFilter":true, 
    "enableSorting":true, 
    "gridHeight":59,
    "searchable":true,
    "gridActions":true,
    "gridTitlePanel":true,
    "clearFilter":true, 
    "exportFileName":"Company Search Data"
};

export default HeaderData;