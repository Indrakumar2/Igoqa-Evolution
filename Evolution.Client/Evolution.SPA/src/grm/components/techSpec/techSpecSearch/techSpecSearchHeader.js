import { isUndefined } from '../../../../utils/commonUtils';
import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();
export const HeaderData = {

    "columnDefs": [
        {
            "headerName": localConstant.resourceSearch.EPIN,
            "field": "epin",
            "tooltipField": "epin",
            "headerTooltip":localConstant.resourceSearch.EPIN,
            "filter": "agNumberColumnFilter",
            "cellRenderer": "HyperLinkRenderer",
            "width":120
        },    
        {
            "headerName": "Last Name",
            "field": "lastName",
            "tooltipField": "lastName",
            "headerTooltip":"Last Name",
            "filter": "agTextColumnFilter",
            "width":150
        },
        {
            "headerName": "First Name",
            "field": "firstName",
            "tooltipField": "firstName",
            "filter": "agTextColumnFilter",
            "headerTooltip":"First Name",
            "width":150
        },
        {
            "headerName": "Profile Status",
            "field": "profileStatus",
            "tooltipField": "profileStatus",
            "headerTooltip":"Profile Status",
            "filter": "agTextColumnFilter",
            "width":120
        },
        {
            "headerName": localConstant.resourceSearch.PENDING_WITH,
            "field": "pendingWithUser",
            "tooltipField": "pendingWithUser",
            "headerTooltip":localConstant.resourceSearch.PENDING_WITH,
            "filter": "agTextColumnFilter",
            "width":130
        }, //Added for D655(#15 Issue)
        {
            "headerName": "Company",
            "field": "companyName",
            "tooltipField": "companyName",
            "headerTooltip":"Company",
            "filter": "agTextColumnFilter",
            "width":200
        },
        {
            "headerName": "Country",
            "field": "country",
            "tooltipField": "country",
            //Changes - D1332
            // "tooltip":(params) => {              
            //     if(!isUndefined(params.data.technicalSpecialistContact)){
            //         const country = params.data.technicalSpecialistContact.filter(item =>{
            //             if(item.contactType === 'PrimaryAddress')
            //             {
            //                 return item.country;
            //             }
            //         }).map((obj)=> { return obj.country; });
            //         return country;
            //     }             
                   
            // },
            "headerTooltip":"Country",
            "filter": "agTextColumnFilter",
            "width":120,
            //Changes - D1332
            // "valueGetter": (params) => {              
            //     if(!isUndefined(params.data.technicalSpecialistContact)){
            //         const country = params.data.technicalSpecialistContact.filter(item =>{
            //             if(item.contactType === 'PrimaryAddress')
            //             {
            //                 return item.country;
            //             }
            //         }).map((obj)=> { return obj.country; });
            //         return country;
            //     }             
                   
            // },
        },
        {
            "headerName": "City",
            "field": "city",
            "headerTooltip":"City",
            "tooltipField": "city",
            //Changes - D1332
            // "tooltip":  (params) => {
            //     if(!isUndefined(params.data.technicalSpecialistContact)){
            //     const city = params.data.technicalSpecialistContact.filter(item =>{
            //         if(item.contactType === 'PrimaryAddress')
            //         {
            //             return item.city;
            //         }                    
            //     }).map((obj)=> { return obj.city; });
            //     return city;
            //     } 
            // },
            "filter": "agTextColumnFilter",
            "width":120,
            //Changes - D1332
            // "valueGetter": (params) => {
            //     if(!isUndefined(params.data.technicalSpecialistContact)){
            //     const city = params.data.technicalSpecialistContact.filter(item =>{
            //         if(item.contactType === 'PrimaryAddress')
            //         {
            //             return item.city;
            //         }                    
            //     }).map((obj)=> { return obj.city; });
            //     return city;
            //     } 
            // },
        },
        {
            "headerName": "State/County/Province",
            "field": "county",
            "tooltipField": "county",
            //Changes - D1332
            // "tooltip": (params) => {
            //     if(!isUndefined(params.data.technicalSpecialistContact)){
            //     const county = params.data.technicalSpecialistContact.filter(item =>{
            //         if(item.contactType === 'PrimaryAddress')
            //         {
            //             return item.county;
            //         }                    
            //     }).map((obj)=> { return obj.county; });
            //     return county;
            // }
            // },
            "headerTooltip":"State/County/Province",
            "filter": "agTextColumnFilter",
            "width":200,
            //Changes - D1332
            // "valueGetter": (params) => {
            //     if(!isUndefined(params.data.technicalSpecialistContact)){
            //     const county = params.data.technicalSpecialistContact.filter(item =>{
            //         if(item.contactType === 'PrimaryAddress')
            //         {
            //             return item.county;
            //         }                    
            //     }).map((obj)=> { return obj.county; });
            //     return county;
            // }
            // },
        },
        {
            "headerName": "Post/Zip Code",
            "field": "pinCode",
            "tooltipField": "pinCode",
            //Changes - D1332
            // "tooltip": (params) => {
            //     if(!isUndefined(params.data.technicalSpecialistContact)){
            //     const postalCode = params.data.technicalSpecialistContact.filter(item =>{
            //         if(item.contactType === 'PrimaryAddress')
            //         {
            //             return item.postalCode;
            //         }                    
            //     }).map((obj)=> { return obj.postalCode; });
            //     return postalCode;
            // }
            // },
            "headerTooltip":"Post/Zip Code",
            "filter": "agTextColumnFilter",
            "width":150,
            //Changes - D1332
            // "valueGetter": (params) => {
            //     if(!isUndefined(params.data.technicalSpecialistContact)){
            //     const postalCode = params.data.technicalSpecialistContact.filter(item =>{
            //         if(item.contactType === 'PrimaryAddress')
            //         {
            //             return item.postalCode;
            //         }                    
            //     }).map((obj)=> { return obj.postalCode; });
            //     return postalCode;
            // }
            // },
            
        },
        {
            "headerName": "Full Address",
            "field": "fullAddress",
            "tooltipField": "fullAddress",
            //Changes - D1332
            // "tooltip":(params) => {
            //     if(!isUndefined(params.data.technicalSpecialistContact)){
            //     const address = params.data.technicalSpecialistContact.filter(item =>{
            //         if(item.contactType === 'PrimaryAddress')
            //         {
            //             return item.address;
            //         }                    
            //     }).map((obj)=> { return obj.address; });
            //     return address;
            // }
            // },
            "headerTooltip":"Full Address",
            "filter": "agTextColumnFilter",
            "width":150,
            //Changes - D1332
            // "valueGetter": (params) => {
            //     if(!isUndefined(params.data.technicalSpecialistContact)){
            //     const address = params.data.technicalSpecialistContact.filter(item =>{
            //         if(item.contactType === 'PrimaryAddress')
            //         {
            //             return item.address;
            //         }                    
            //     }).map((obj)=> { return obj.address; });
            //     return address;
            // }
            // },
        },
        {
            "headerName": "Mobile No",
            "field": "mobileNumber",
            "headerTooltip":"Mobile No",
            "tooltipField": "mobileNumber",
            //Changes - D1332
            // "tooltip": (params) => {
            //     if(!isUndefined(params.data.technicalSpecialistContact)){
            //     const mobileNumber = params.data.technicalSpecialistContact.filter(item =>{
            //         if(item.contactType === 'PrimaryMobile')
            //         {
            //             return item.mobileNumber;
            //         }                    
            //     }).map((obj)=> { return obj.mobileNumber; });
            //     return mobileNumber;
            // }
            // },
            "filter": "agNumberColumnFilter",
            "width":150,
            //Changes - D1332
            // "valueGetter": (params) => {
            //     if(!isUndefined(params.data.technicalSpecialistContact)){
            //     const mobileNumber = params.data.technicalSpecialistContact.filter(item =>{
            //         if(item.contactType === 'PrimaryMobile')
            //         {
            //             return item.mobileNumber;
            //         }                    
            //     }).map((obj)=> { return obj.mobileNumber; });
            //     return mobileNumber;
            // }
            // },
            "filterParams": {
                "inRangeInclusive":true
            }
        },{
            "headerName": "Email Address",
            "field": "emailAddress",
            "tooltipField": "emailAddress",
            //Changes - D1332
            // "tooltip":  (params) => {
            //     if(!isUndefined(params.data.technicalSpecialistContact)){
            //     const emailAddress = params.data.technicalSpecialistContact.filter(item =>{
            //         if(item.contactType === 'PrimaryEmail')
            //         {
            //             return item.emailAddress;
            //         }                    
            //     }).map((obj)=> { return obj.emailAddress; });
            //     return emailAddress;
            // }                   
            // },
            "headerTooltip":"Email Address",
            "filter": "agTextColumnFilter",
            "width":150,
            //Changes - D1332
            // "valueGetter": (params) => {
            //     if(!isUndefined(params.data.technicalSpecialistContact)){
            //     const emailAddress = params.data.technicalSpecialistContact.filter(item =>{
            //         if(item.contactType === 'PrimaryEmail')
            //         {
            //             return item.emailAddress;
            //         }                    
            //     }).map((obj)=> { return obj.emailAddress; });
            //     return emailAddress;
            // }                   
            // },
        },
        //Changes - D913
        // {
        //     "headerName": "Business Unit",
        //     "field": "businessUnit",
        //     "tooltipField": "businessUnit",
        //     "headerTooltip":"Business Unit",
        //     "filter": "agNumberColumnFilter",
        //     "width":150,
        //     "filterParams": {
        //         "inRangeInclusive":true
        //     }
        // },
        {
            "headerName": "Sub Division",
            "field": "subDivisionName",
            "tooltipField": "subDivisionName",
            "headerTooltip":"Sub Division",
            "filter": "agNumberColumnFilter",
            "width":150,
            "filterParams": {
                "inRangeInclusive":true
            }
        },
        {
            "headerName": "Employment Status",
            "field": "employmentType",
            "tooltipField": "employmentType",
            "headerTooltip":"Employment Status",
            "filter": "agTextColumnFilter",
            "width":160
        }
    ],
    
    "rowSelection": 'single',   
    "pagination": true,
    "enableFilter": true,
    "enableSorting": true,
    "gridHeight": 60,
    "searchable": true,
    "exportFileName":"Resource Search Data",
    "gridActions": true,
    "clearFilter":true, 
    "gridTitlePanel": true
};