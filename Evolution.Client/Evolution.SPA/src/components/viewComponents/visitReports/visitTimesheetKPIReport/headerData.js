import { getlocalizeData,isUndefined } from '../../../../utils/commonUtils';
import store from '../../../../store/reduxStore';
const localConstant = getlocalizeData();

/** Header Data for Assignment Specialist Grid - inline editing ITK CR */
export const HeaderData = {
    "techSpecSearch":{
        "columnDefs": [
            {
                "headerCheckboxSelectionFilteredOnly": true,
                "checkboxSelection": true,
                "suppressFilter": true,
                "width": 40
            },
            {
                "headerName": "ePin",
                "field": "epin",
                "tooltipField":"epin",   
                "headerTooltip":"ePin",
                "filter": "agTextColumnFilter",
                "width":120,
                "cellRenderer": "HyperLinkRenderer",
                "cellRendererParams": {
                        "searchType":"NDT"
                },
            },
            {
                "headerName": "LastName",
                "field": "lastName",
                "tooltipField":"lastName",   
                "headerTooltip":"LastName",
                "filter": "agTextColumnFilter",
                "width":130
            },
            {
                "headerName": "FirstName",
                "field": "firstName",
                "tooltipField":"firstName",   
                "filter": "agTextColumnFilter",
                "width":140
            },
            {
                "headerName": "Status",
                "field": "profileStatus",
                "headerTooltip":"profileStatus",
                "filter": "agTextColumnFilter",
                "width":120
            },
            {
                "headerName": "Company",
                "field": "companyName",
                "tooltipField":"companyName",   
                "headerTooltip":"Company",
                "filter": "agTextColumnFilter",
                "width":200
            },
            {
                "headerName": "Country",
                "field": "country",
                "tooltipField":"country",   
                "headerTooltip":"Country",
                "filter": "agTextColumnFilter",
                "width":120
            },
            {
                "headerName": "City",
                "field": "city",
                "tooltipField":"city",   
                "headerTooltip":"City",
                "filter": "agTextColumnFilter",
                "width":120
            },
            {
                "headerName": "State/County/Province",
                "field": "county",
                "tooltipField":"county",   
                "headerTooltip":"State/County/Province",
                "filter": "agTextColumnFilter",
                "width":220
            },
            {
                "headerName": "Post/Zip Code",
                "field": "pinCode",
                "tooltipField":"pinCode",   
                "headerTooltip":"Post/Zip Code",
                "filter": "agTextColumnFilter",
                "width":160
            },
            {
                "headerName": "Full Address",
                "field": "fullAddress",
                "tooltipField":"fullAddress",
                "headerTooltip":"Full Address",
                "filter": "agTextColumnFilter",
                "width":140
            },
            {
                "headerName": "Mobile No",
                "field": "mobileNumber",
                "tooltipField": "mobileNumber",   
                "headerTooltip":"Mobile No",
                "filter": "agNumberColumnFilter",
                "width":140,
                "filterParams": {
                    "inRangeInclusive":true
                }
            },{
                "headerName": "Email Address",
                "field": "emailAddress",
                "tooltipField": "emailAddress",   
                "headerTooltip":"Email Address",
                "filter": "agTextColumnFilter",
                "width":180
            },
            // {
            //     "headerName": "Business Unit",
            //     "field": "businessUnit",
            //     "tooltipField":"businessUnit",   
            //     "headerTooltip":"Business Unit",
            //     "filter": "agNumberColumnFilter",
            //     "width":180,
            //     "filterParams": {
            //         "inRangeInclusive":true
            //     }
            // },
            {
                "headerName": "Sub Division",
                "field": "subDivisionName",
                "tooltipField":"subDivisionName",   
                "headerTooltip":"Sub Division",
                "filter": "agNumberColumnFilter",
                "width":140,
                "filterParams": {
                    "inRangeInclusive":true
                }
            },
            {
                "headerName": "Employment Status",
                "field": "employmentType",
                "tooltipField":"employmentType",   
                "headerTooltip":"Employment Status",
                "filter": "agNumberColumnFilter",
                "width":250,
                "filterParams": {
                    "inRangeInclusive":true
                }
            }
        ],
        "pagination":true,
        "enableFilter": true,
        "enableSorting": true,
        "gridHeight": 35,
        "gridActions": true,
        "clearFilter":true, 
        "gridTitlePanel": true
    },
};