import { createSelector } from 'reselect';
import { activitycode } from '../constants/securityConstant';
import arrayUtil from '../utils/arrayUtil';
import { StringFormat } from '../utils/stringUtil';
import { contractAPIConfig,companyAPIConfig,customerAPIConfig,projectAPIConfig,assignmentAPIConfig,supplierAPIConfig,supplierPOApiConfig,visitAPIConfig,timesheetAPIConfig,GrmAPIConfig } from '../apiConfig/apiConfig';
const getCurrentModule = (state) => state.commonReducer.currentModule;
//ID-583 and 802 - file upload unsaved file Clear function 
export const CheckForUnSavedDocChanges=(state)=>{
    const switchValue=state.CommonReducer.currentModule?state.CommonReducer.currentModule.toLowerCase() :state.CommonReducer.currentModule;
    let ObjDoc={        
        documentData:[],
        deleteUrl:'',
        moduleRefCode:''
    };  
    switch (switchValue) {
        case 'company':   
                const companyInfo = Object.assign({},state.CompanyReducer.companyDetail.CompanyInfo);               
                const companyCode = companyInfo.companyCode;                
            return ObjDoc={
                    documentData:state.CompanyReducer.companyDetail.CompanyDocuments,
                    deleteUrl:companyAPIConfig.companyBaseURL + companyAPIConfig.companyDocuments + companyCode,
                    moduleRefCode:companyCode
                    };
        case 'customer':
            const customerDetails = Object.assign({},state.CustomerReducer.customerDetail.Detail);          
            const customerCode = customerDetails.customerCode;          
            return ObjDoc={
                documentData:state.CustomerReducer.customerDetail.Documents,
                deleteUrl:customerAPIConfig.custBaseUrl + customerAPIConfig.customerDocuments + customerCode,
                moduleRefCode:customerCode
                };
        case 'contract':
            const contractInfo = Object.assign({},state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInfo);
            const contractNumber = contractInfo.contractNumber;
            return ObjDoc={
                    documentData:state.RootContractReducer.ContractDetailReducer.contractDetail.ContractDocuments,
                    deleteUrl:contractAPIConfig.contractBaseUrl + contractAPIConfig.contractDocuments + contractNumber,
                    moduleRefCode:contractNumber
            }; 
        case 'project':
            const ProjectInfo = Object.assign({},state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo); 
            const selectedProjectNo = ProjectInfo.projectNumber;            
            return ObjDoc={
                    documentData:state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectDocuments,
                    deleteUrl:projectAPIConfig.baseUrl + projectAPIConfig.projectDocuments + selectedProjectNo,
                    moduleRefCode:selectedProjectNo
                    }; 
        case 'assignment':
            const AssignmentInfo = Object.assign({},state.rootAssignmentReducer.assignmentDetail.AssignmentInfo);
            const assignmentId = AssignmentInfo.assignmentId;           
            return ObjDoc={
                    documentData:state.rootAssignmentReducer.assignmentDetail.AssignmentDocuments,
                    deleteUrl:assignmentAPIConfig.assignmentDocuments + assignmentId,
                    moduleRefCode:assignmentId
                }; 
        case 'supplier':
            const supplierInfo = Object.assign({},state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierInfo);
            const supplierId = supplierInfo.supplierId;          
            return ObjDoc={
                    documentData:state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierDocuments,
                    deleteUrl:StringFormat(supplierAPIConfig.supplierDocumentsDelete, supplierId),
                    moduleRefCode:supplierId
                }; 
        case 'supplierpo':
            const supplierPoInfo = Object.assign({},state.rootSupplierPOReducer.supplierPOData.SupplierPOInfo);
            const supplierPOId = supplierPoInfo.supplierPOId;
            return ObjDoc={
                    documentData:state.rootSupplierPOReducer.supplierPOData.SupplierPODocuments,
                    deleteUrl:supplierPOApiConfig.supplierPODocuments + supplierPOId,
                    moduleRefCode:supplierPOId
            }; 
        case 'visit':
             const VisitInfo = Object.assign({},state.rootVisitReducer.visitDetails.VisitInfo);
            const visitId = VisitInfo.visitId;
            return ObjDoc={
                documentData:state.rootVisitReducer.visitDetails.VisitDocuments,
                deleteUrl: StringFormat(visitAPIConfig.visitDocumentsDelete, visitId),
                moduleRefCode:visitId
        };
        case 'timesheet':
            const TimesheetInfo = Object.assign({},state.rootTimesheetReducer.timesheetDetail.TimesheetInfo);
            const timesheetId = TimesheetInfo.timesheetId;
            return ObjDoc={
                documentData:state.rootTimesheetReducer.timesheetDetail.TimesheetDocuments,
                deleteUrl: StringFormat(timesheetAPIConfig.timesheetDocumentDelete, timesheetId),
                moduleRefCode:timesheetId
        };
        case 'grm':
            const techSpecInfo =Object.assign({}, state.RootTechSpecReducer.TechSpecDetailReducer);
            const epin = techSpecInfo.selectedEpinNo;
            return ObjDoc={
                documentData:state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistSensitiveDocuments,
                deleteUrl:GrmAPIConfig.grmBaseUrl + GrmAPIConfig.techSpecDocuments + epin + GrmAPIConfig.subModuleRefCode+ 0,
                moduleRefCode:epin
        };        
        default:
            return ObjDoc;
    }
};
const ModuleHasUnsavedChanges = (state) => {
    const switchValue=state.CommonReducer.currentModule?state.CommonReducer.currentModule.toLowerCase() :state.CommonReducer.currentModule;
    switch (switchValue) {
        case 'company':
            return state.CompanyReducer.isbtnDisable;
        case 'customer':
            return state.CustomerReducer.isbtnDisable;
        case 'contract':
            return state.RootContractReducer.ContractDetailReducer.isbtnDisable;
        case 'project':
            return state.RootProjectReducer.ProjectDetailReducer.isbtnDisable;
        case 'assignment':
            return state.rootAssignmentReducer.isbtnDisable;
        case 'supplier':
            return state.RootSupplierReducer.SupplierDetailReducers.isbtnDisable;
        case 'supplierpo':
            return state.rootSupplierPOReducer.isbtnDisable;
        case 'visit':
            return state.rootVisitReducer.isbtnDisable;
        case 'timesheet':
            return state.rootTimesheetReducer.isbtnDisable;
        case 'grm':
            return state.RootTechSpecReducer.TechSpecDetailReducer.isbtnDisable;
        case 'security':
            return state.rootSecurityReducer.isUserDataChanged;
        case 'techspecialist': //D576
            return state.RootTechSpecReducer.TechSpecDetailReducer.isTechSpecDataChanged;
        default:
            return true;
    }
};
export const CheckForUnsavedChanges = createSelector(
    ModuleHasUnsavedChanges,
    (isChanged) => {
        return isChanged;
    }
);

export const filterDocTypes = createSelector([ (state)=>state.docTypes,(state)=>state.moduleName ],
    (docTypes, moduleName) => {
        if (Array.isArray(docTypes) && docTypes.length > 0) {
            return docTypes.filter(doc=>doc.moduleName === moduleName);
        }
        return [];
});

export const getTSVisible = createSelector([ (state)=>state.docTypes,(state)=>state.docTypeName,(state)=>state.moduleName ],
    (docTypes, docTypeName, moduleName) => {
        if (Array.isArray(docTypes) && docTypes.length > 0) {
            const filteredDocType= docTypes.filter(doc=>doc.moduleName === moduleName && doc.name === docTypeName);
            if(Array.isArray(filteredDocType) && filteredDocType.length > 0)
              return filteredDocType[0].isTSVisible;
        }
        return [];
});
 
export const isEditable = createSelector(
    [
       (state)=> state.activities, (state)=>state.editActivityLevels
    ],
    (activities, editActivityLevels) => {
        let result = false;
        if (activities && editActivityLevels) {
            if (activities.length > 0 && editActivityLevels.length > 0) {
                result = activities.filter(x => editActivityLevels.includes(x.activity)).length > 0;
            }
        }
        return result;
    }
);

export const isViewable = createSelector(
    [
        (state)=> state.activities, (state)=> state.levelType, (state)=> state.viewActivityLevels
    ],
    (activities, levelType, viewActivityLevels) => {  
        let result = false;
        if (activities && viewActivityLevels) {
            if (activities.length > 0 && viewActivityLevels.length > 0) {
                switch (levelType) {
                    case 'LEVEL_0':
                        result = (activities.filter(x => viewActivityLevels.includes(x.activity)).length > 0 || !activities.filter(x => x.activity === activitycode.LEVEL_0_VIEW).length > 0);
                        break;
                    default:
                        result = activities.filter(x => viewActivityLevels.includes(x.activity)).length > 0;
                        break;
                }
            }
        }
        return result;
    }
); 
export const sortSelector = createSelector(
    [
        (array, sortKey,sortOrder) => arrayUtil.sort(array,sortKey,sortOrder)
    ],
    (result) => result
  );

//Added for D549
export const fetchMatchedObject= createSelector (
    [ (state) => state.list, (state) => state.param, (state) => state.id ],
    (list,param,id) => {
            return arrayUtil.FilterGetObject(list,param,id);
    }
);