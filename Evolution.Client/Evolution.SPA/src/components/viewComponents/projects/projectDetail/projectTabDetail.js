import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();
export const ProjectTabDetail = [
  {
    tabHeader: "General Details",
    tabBody: "GeneralDetails",
    tabActive:true,
    tabDisableStatus:[],    
    isCurrentTab: true
  },
  {
    tabHeader: "Invoicing Defaults",
    tabBody: "InvoicingDefaults",
    tabActive:true,
    tabDisableStatus:[],
    isCurrentTab: false   
  },
  {
    tabHeader: "Documents",
    tabBody: "Documents",
    tabActive:true,
    tabDisableStatus:[ '/ProjectDetails' ],
    isCurrentTab: false 
  },
  {
    tabHeader: "Supplier PO",
    tabBody: "SupplierPO",
    tabActive:false,
    tabDisableStatus:[ '/ProjectDetails', ],
    isCurrentTab: false 
  },
  {
    tabHeader: "Assignments",
    tabBody: "Assignments",
    tabActive:false,
    tabDisableStatus:[ '/ProjectDetails' ],
    isCurrentTab: false  
  },
  {
    tabHeader: "Notes",
    tabBody: "Notes",
    tabActive:true,
    tabDisableStatus:[],
    isCurrentTab: false 
  },
  {
    tabHeader: localConstant.project.CLIENT_NOTIFICATION,  //Changes for D-972
    tabBody: "ClientNotification",
    tabActive:true,
    tabDisableStatus:[],
    isCurrentTab: false    
  }
];