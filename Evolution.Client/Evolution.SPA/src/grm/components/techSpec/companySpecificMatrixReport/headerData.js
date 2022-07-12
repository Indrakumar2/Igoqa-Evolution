import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();

export const HeaderData = {
    "columnDefs": [
        {
            "headerName": "Resource Name",
            "headerTooltip": "Resource Name",
            "field": "resourceName",
            "cellRenderer": 'agGroupCellRenderer',
            "filterParams": {
                "inRangeInclusive":true
            },
            "filter": "agTextColumnFilter",
            "tooltipField": "resourceName",
            "width": 300
        },
        {
            "headerName": "Taxonomy Service Name",
            "headerTooltip": "Taxonomy Service Name",
            "field": "taxonomyServiceName",
            "filter": "agTextColumnFilter",
            "width": 300,
            "tooltipField": "taxonomyServiceName"
        },
        // {
        //     "headerName": "Is Taxonomy Applicable",
        //     "headerTooltip": "Is Taxonomy Applicable",
        //     "field": "isTaxonomyApplicable",
        //     "filter": "agTextColumnFilter",
        //     "width": 200,
        //     "tooltipField": "isTaxonomyApplicable",
        //     "valueGetter": (params) =>{ 
        //         if(params.data.isTaxonomyApplicable) {
        //             return "X";
        //         }
        //         else  {
        //             return "";
        //         }
        //     },
        //     "tooltip":(params) => {
        //         if(params.data.isTaxonomyApplicable) {
        //             return "X";
        //         }
        //         else  {
        //             return "";
        //         }
        //     },
        // },
        {
            "headerName": "Employee",
            "headerTooltip": "Employee",
            "field": "isEmployee",
            "filter": "agTextColumnFilter",
            "width": 180,
            "valueGetter": (params) =>{ 
                if(params.data.isEmployee) {
                    return "X";
                }
                else  {
                    return "";
                }
            },
            "tooltip":(params) => {
                if(params.data.isEmployee) {
                    return "X";
                }
                else  {
                    return "";
                }
            },
        },
        {
            "headerName": "Contractor",
            "headerTooltip": "Contractor",
            "field": "Contractor",
            "filter": "agTextColumnFilter",
            "tooltipField": "Contractor",
            "width": 180,
            "valueGetter": (params) =>{ 
                if(params.data.isContractor) {
                    return "X";
                }
                else  {
                    return "";
                }
            },
            "tooltip":(params) => {
                if(params.data.isContractor) {
                    return "X";
                }
                else  {
                    return "";
                }
            },
        },
        {
            "headerName": "Company",
            "headerTooltip": "Company",
            "field": "company",
            "filter": "agTextColumnFilter",
            "tooltipField": "company",
            "width": 300,
        },
    ],

    "enableFilter": true,
    "enableSorting": true,
    "pagination": true,
    "gridTitlePanel": true,
    "clearFilter": true,
    "Grouping":true,
    "gridActions": true,
    "customHeader": "Company Specifix Matrix Report",
    "exportFileName": "Company Specifix Matrix Report",
    "gridHeight": 25,
    "GroupingDataName":"childArray"
};
export const TaxonomyHeaderData = {
    "columnDefs": [
        {
            "headerName": "Taxonomy Service Name",
            "headerTooltip": "Taxonomy Service Name",
            "field": "taxonomyServiceName",
            "cellRenderer": 'agGroupCellRenderer',
            "filterParams": {
                "inRangeInclusive":true
            },
            "filter": "agTextColumnFilter",
            "tooltipField": "taxonomyServiceName",
            "width": 300
        },
        {
            "headerName": "Resource Name",
            "headerTooltip": "Resource Name",
            "field": "resourceName",
            "filter": "agTextColumnFilter",
            "width": 300,
            "tooltipField": "resourceName"
        },
        // {
        //     "headerName": "Is Taxonomy Applicable",
        //     "headerTooltip": "Is Taxonomy Applicable",
        //     "field": "isTaxonomyApplicable",
        //     "filter": "agTextColumnFilter",
        //     "width": 200,
        //     "tooltipField": "isTaxonomyApplicable",
        //     "valueGetter": (params) =>{ 
        //         if(params.data.isTaxonomyApplicable) {
        //             return "X";
        //         }
        //         else  {
        //             return "";
        //         }
        //     },
        //     "tooltip":(params) => {
        //         if(params.data.isTaxonomyApplicable) {
        //             return "X";
        //         }
        //         else  {
        //             return "";
        //         }
        //     },
        // },
        {
            "headerName": "Employee",
            "headerTooltip": "Employee",
            "field": "isEmployee",
            "filter": "agTextColumnFilter",
            "width": 180,
            "valueGetter": (params) =>{ 
                if(params.data.isEmployee) {
                    return "X";
                }
                else  {
                    return "";
                }
            },
            "tooltip":(params) => {
                if(params.data.isEmployee) {
                    return "X";
                }
                else  {
                    return "";
                }
            },
        },
        {
            "headerName": "Contractor",
            "headerTooltip": "Contractor",
            "field": "Contractor",
            "filter": "agTextColumnFilter",
            "tooltipField": "Contractor",
            "width": 180,
            "valueGetter": (params) =>{ 
                if(params.data.isContractor) {
                    return "X";
                }
                else  {
                    return "";
                }
            },
            "tooltip":(params) => {
                if(params.data.isContractor) {
                    return "X";
                }
                else  {
                    return "";
                }
            },
        },
        {
            "headerName": "Company",
            "headerTooltip": "Company",
            "field": "company",
            "filter": "agTextColumnFilter",
            "tooltipField": "company",
            "width": 300,
        },
    ],

    "enableFilter": true,
    "enableSorting": true,
    "pagination": true,
    "gridTitlePanel": true,
    "clearFilter": true,
    "Grouping":true,
    "gridActions": true,
    "customHeader": "Company Specifix Matrix Report",
    "exportFileName": "Company Specifix Matrix Report",
    "gridHeight": 25,
    "GroupingDataName":"childArray"
};
