import { getlocalizeData } from '../../../../utils/commonUtils';

const localConstant = getlocalizeData();
export const HeaderData ={ 
    "GeneralAddressHeader":{
    "columnDefs": [
        {
            "checkboxSelection": true,
            "headerCheckboxSelectionFilteredOnly": true,
            "suppressFilter":true,
            "width":40
        },
        {
            "headerName": localConstant.gridHeader.COUNTRY,
            "field": "Country",
            "tooltipField": "Country",
            "filter": "agTextColumnFilter",
            "width":130,
            "headerTooltip": localConstant.gridHeader.COUNTRY
        },
        {
            "headerName": localConstant.gridHeader.STATE,
            "field": "County",
            "tooltipField": "County",
            "filter": "agTextColumnFilter",
            "width":240,
            "headerTooltip": localConstant.gridHeader.STATE

        },
        {
            "headerName": localConstant.gridHeader.CITY,
            "field": "City",
            "tooltipField": "City",
            "filter": "agTextColumnFilter",
            "width":100,
            "headerTooltip": localConstant.gridHeader.CITY
        },
        {
            "headerName": localConstant.gridHeader.FULL_ADDRESS,
            "field": "Address",
            "tooltipField": "Address",
            "filter": "agTextColumnFilter",
            "headerTooltip": localConstant.gridHeader.FULL_ADDRESS,
            "width":170          
        },
        {
            "headerName": localConstant.gridHeader.POSTAL_CODE,
            "field": "PostalCode",
            "tooltipField": "PostalCode",
            "filter": "agTextColumnFilter",
            "width":150,
            "headerTooltip": localConstant.gridHeader.POSTAL_CODE
        },
        {
            "headerName": localConstant.gridHeader.EU_VAT_PREFIX,
            "field": "EUVatPrefix",
            "tooltipField": "EUVatPrefix",
            "filter": "agTextColumnFilter",
            "width":160,
            "headerTooltip": localConstant.gridHeader.EU_VAT_PREFIX
        },
        {
            "headerName": localConstant.gridHeader.VAT_TAX_REGISTRATION_NO,
            "field": "VatTaxRegNumber",
            "tooltipField": "VatTaxRegNumber",
            "filter": "agTextColumnFilter",
            "width":250,
            "headerTooltip": localConstant.gridHeader.VAT_TAX_REGISTRATION_NO
        },
        {
            "headerName": "",
            "field": "EditColumn",
            "cellRenderer": "EditRenderer",
            "cellRendererParams": {
                "action": "EditAddressReference",
                "popupId": "add-address"
            },
            "suppressFilter":true,
            "suppressSorting": true,
            "width": 80
        }
    ],
    "enableSelectAll":true,
    "enableFilter":true, 
    "enableSorting":true, 
    "pagination": false,
    "rowSelection":"multiple",
    "searchable":true,
    "gridActions":true,
    "gridTitlePanel":false,
    "gridHeight":50,
    "clearFilter":true, 
    "exportFileName":"Addresses"
},
};

export const GeneralContactHeader = (functionRefs) => {
    return {
        "columnDefs": [
            {
                "checkboxSelection": true,
                // "headerCheckboxSelectionFilteredOnly": true,
                "headerCheckboxSelection": true,
                "suppressFilter":true,
                "width":40
            },
            {
                "headerName": localConstant.gridHeader.FULL_ADDRESS,
                "field": "contactAddress",
                "tooltipField": "contactAddress",
                "filter": "agTextColumnFilter",
                "headerTooltip": localConstant.gridHeader.FULL_ADDRESS ,
                "width":150     
            },
            {
                "headerName": "",
                "field": "CustomerAddressId",
                "tooltipField": "lastLoginDate",
                "filter": "agTextColumnFilter",
                "hide":true           
            },
            {
                "headerName": localConstant.gridHeader.CONTACT_NAME,
                "field": "ContactPersonName",
                "tooltipField": "ContactPersonName",
                "filter": "agTextColumnFilter",
                "headerTooltip": localConstant.gridHeader.CONTACT_NAME,
                "width":150          
            },
            {
                "headerName": localConstant.gridHeader.SALUTATION,
                "field": "Salutation",
                "tooltipField": "Salutation",
                "filter": "agTextColumnFilter",
                "width":130,
                "headerTooltip": localConstant.gridHeader.SALUTATION
            },
            {
                "headerName": localConstant.gridHeader.POSITION,
                "field": "Position",
                "tooltipField": "Position",
                "filter": "agTextColumnFilter",
                "headerTooltip": localConstant.gridHeader.POSITION,
                "width":130
            },
            {
                "headerName": localConstant.gridHeader.TELEPHONE_NO,
                "field": "Landline",
                "tooltipField": "Landline",
                "filter": "agNumberColumnFilter",
                "headerTooltip": localConstant.gridHeader.TELEPHONE_NO,
                "width":170,
                "filterParams": {
                    "inRangeInclusive":true
                },
            },
            // {
            //     "headerName": localConstant.gridHeader.FAX_NO,
            //     "field": "Fax",
            //     "filter": "agTextColumnFilter",
            //     "width":100,
            //     "headerTooltip": localConstant.gridHeader.FAX_NO
            // },
            {
                "headerName": localConstant.gridHeader.MOBILE_NO,
                "field": "Mobile",
                "tooltipField": "Mobile",
                "filter": "agNumberColumnFilter",
                "width":170,
                "headerTooltip": localConstant.gridHeader.MOBILE_NO,
                "filterParams": {
                    "inRangeInclusive":true
                },
            },
            {
                "headerName": localConstant.gridHeader.EMAIL,
                "field": "Email",
                "tooltipField": "Email",
                "filter": "agTextColumnFilter",
                "width":100,
                "headerTooltip": localConstant.gridHeader.EMAIL
            },
            {
                "headerName": localConstant.gridHeader.OTHER_CONTACT_DETAILS,
                "field": "OtherDetail",
                "tooltipField": "OtherDetail",
                "filter": "agTextColumnFilter",
                "headerTooltip": localConstant.gridHeader.OTHER_CONTACT_DETAILS,
                "width":220  
            },
            // {
            //     "headerName": localConstant.gridHeader.PORTAL,
            //     "field": "extranet",
            //     "tooltip":(params) => {
            //         if (params.data.extranet === true) {
            //             return "Yes";
            //         } else {
            //             return "No";
            //         }
            //     },
            //     "filter": "agTextColumnFilter",
            //         "valueGetter": (params) => {
            //             if(params.data.extranet === true){
            //                 return "Yes";
            //             }else {
            //               return "No";      
            //                 }
            //           },
            //           "width": 100
            // }, 
            // {
            //     "headerName": localConstant.gridHeader.PORTAL_USER,
            //     "field": "PortalUserColumn",
            //     "cellRenderer": "EditLink",
            //     "buttonAction":"",
            //     "cellRendererParams": {
            //         "displayLink":'Add User'
            //     },
            // }, 
             {
                "headerName": localConstant.gridHeader.PORTAL,
                "field": "IsPortalUser",
                "filter": "agTextColumnFilter",
                "width":120,
                "cellRenderer": "InlineSwitch",
                "cellRendererParams": {
                    "isSwitchLabel": false,
                    "name":"IsPortalUser",
                    "id":"isPortalUserId",
                    "switchClass":"", 
                    "disabled":functionRefs.isDisable,
                },
                "tooltip":(params)=>{
                    if(params.data && !params.data.IsPortalUser){
                        return 'No';
                    }else{
                        return "Yes";
                    }
                },      
                "valueGetter":(params)=>{
                    if(params.data && !params.data.IsPortalUser){
                        return 'No';
                    }else{
                        return "Yes";
                    }
                },
                "headerTooltip": localConstant.gridHeader.PORTAL,
            },
            {
                "headerName": "",
                "field": "",
                "cellRenderer": "EditRenderer",
                "cellRendererParams": {
                    "action": "EditContactReference",
                    "popupId": "add-contact"
                },
                "suppressFilter":true,
                "width": 80
            }
        ],    
        "enableSelectAll":true,
        "enableFilter":true, 
        "enableSorting":true, 
        "pagination": false,
        "rowSelection":"multiple",
        "searchable":true,
        "gridActions":true,
        "gridTitlePanel":false,
        "gridHeight":50,
        "clearFilter":true, 
        "exportFileName":"Contacts"
    };
};