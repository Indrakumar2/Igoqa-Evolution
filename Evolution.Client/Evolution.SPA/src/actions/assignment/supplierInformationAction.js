import { assignmentAPIConfig, RequestPayload } from '../../apiConfig/apiConfig';
import { StringFormat } from '../../utils/stringUtil';
import { FetchData } from '../../services/api/baseApiService';
import { isEmpty,getlocalizeData,isEmptyReturnDefault,getNestedObject,deepCopy } from '../../utils/commonUtils';
import { assignmentsActionTypes } from '../../constants/actionTypes';
import { required } from '../../utils/validator';
import IntertekToaster from '../../common/baseComponents/intertekToaster';

const localConstant = getlocalizeData();
const actions ={
    FetchSupplierContacts: (payload) => ({
        type: assignmentsActionTypes.FETCH_SUPPLIER_CONTACTS,
        data: payload
    }),
    FetchSubsuppliers: (payload) => ({
        type: assignmentsActionTypes.FETCH_SUBSUPPLIERS,
        data: payload
    }),
    FetchSubsupplierContacts: (payload) => ({
        type: assignmentsActionTypes.FETCH_SUBSUPPLIER_CONTACTS,
        data: payload
    }),
    FetchMainSupplierName: (payload) => ({
        type:assignmentsActionTypes.FETCH_MAINSUPPLIER_NAME,
        data: payload
    }),
    FetchMainSupplierId:(payload)=>({
        type:assignmentsActionTypes.FETCH_MAINSUPPLIER_ID,
        data: payload
    }),
    AddSubSupplierInformation: (payload) => ({
        type:assignmentsActionTypes.ADD_SUBSUPPLIER_INFORMATION,
        data: payload
    }),
    UpdateSupplierInformation: (payload) => ({
        type: assignmentsActionTypes.UPDATE_SUBSUPPLIER_INFORMATION,
        data: payload
    }),
    AddMainSupplierContactInformation: (payload) => ({
        type:assignmentsActionTypes.ADD_MAIN_SUPPLIER_INFORMATION,
        data: payload
    }), 
    AddSupSupplierTechSpec: (payload) => ({
        type:assignmentsActionTypes.ADD_ASSIGNMENT_SUB_SUPPLIER_TECH_SPEC,
        data: payload
    }),
    UpdateTechSpecAssignedSubSupplier: (payload) => ({
        type:assignmentsActionTypes.UPDATE_ASSIGNMENT_SUB_SUPPLIER_TECH_SPEC,
        data: payload
    }),
    FetchAssignmentTechSpec: (payload) => ({
        type:assignmentsActionTypes.FETCH_ASSIGNMENT_TECHSPEC,
        data: payload
    }),
    UpdateMainSupplierInfo: (payload) => ({
        type:assignmentsActionTypes.UPDATE_MAIN_SUPPLIER_INFO,
        data: payload
    }),
    deletSupplierInformations: (payload) => ({
        type: assignmentsActionTypes.DELETE_SUPPLIER_INFO,
        data: payload
    }),
    UpdateSubSuppliers : (payload) => ({
        type: assignmentsActionTypes.UPDATE_SUBSUPPLIER,
        data: payload
    }),
    subSupplierContactUpdateStatus : (payload) => ({
        type: assignmentsActionTypes.SUB_SUPPLIER_CONTACT_UPDATE_STATUS,
        data: payload
    }),
    deleteSubSupplierTS : (payload) => ({
        type: assignmentsActionTypes.DELETE_SUPPLIER_TECHSPEC,
        data: payload
    })
};

export const FetchSupplierContacts =(selectedsupplierPOId) =>async (dispatch) =>{
    if(!required(selectedsupplierPOId)){
        const url  = StringFormat(assignmentAPIConfig.supplierContact, selectedsupplierPOId);
        const response = await FetchData(url, {})
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.assignments.ERROR_FETCHING_SUPLLIER_CONTACTS, 'dangerToast supplierContactErr');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
        if (!isEmpty(response) && !isEmpty(response.result)) {
            dispatch(actions.FetchSupplierContacts(response.result));
        }
        else{
            dispatch(actions.FetchSupplierContacts([]));
        }
    }
    else{
        dispatch(actions.FetchSupplierContacts([]));
    }
    
};

export const FetchSubsuppliers =(selectedsupplierPOId,assignmentDetail) => async (dispatch,getstate) =>{
    const state = getstate();
    const assignmentData = isEmptyReturnDefault(assignmentDetail,'object');
    const assignmentTechSpec = isEmptyReturnDefault(assignmentData.AssignmentTechnicalSpecialists);
    // const assignmentTechSpec = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentTechnicalSpecialists);
    if(!required(selectedsupplierPOId)){
        const url  = StringFormat(assignmentAPIConfig.subSuppliers, selectedsupplierPOId);
        const response = await FetchData(url, {})
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.assignments.ERROR_FETCHING_SUB_SUPPLIER, 'dangerToast subSupplierErr');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
        if (!isEmpty(response)) {
            if(response.result && response.result.length === 0){
                dispatch(actions.FetchSubsuppliers([]));
                return true;
            }
            else{

                dispatch(subSupplierContactUpdateStatus(false));
                dispatch(actions.FetchSubsuppliers(response.result));
                const subSuppliers=isEmptyReturnDefault(response.result);

                /** Populate sub-supplier with supplier contacts and respective properties from the AssignmentSubSupplier:
                 * if assignmentSubSupplier.length > 0 (excluding recordStatus D)
                 *      forEach of subSupplier
                 *          update isPartOfAssignment, isSubSupplierFirstVisit, supplierContactName and Id
                 *          if isPartOfAssignment, isFirstVisitDisable : false else isFirstVisitDisable : true
                 */
                let contactCount = 0;
                subSuppliers.forEach(iteratedValue => {
                    const assignmentSubSuppliers = isEmptyReturnDefault(assignmentData.AssignmentSubSuppliers);
                    const currentPage = state.CommonReducer.currentPage;
                    const subSupplierIndex = assignmentSubSuppliers.findIndex(x => x.recordStatus !== 'D' && x.subSupplierId === iteratedValue.supplierId);
                    if(subSupplierIndex >= 0){
                        iteratedValue.isPartofAssignment = true;
                        iteratedValue.isSubSupplierFirstVisit = assignmentSubSuppliers[subSupplierIndex].isSubSupplierFirstVisit;
                        iteratedValue.isFirstVisitDisabled = false;
                        iteratedValue.subSupplierContactId = assignmentSubSuppliers[subSupplierIndex].subSupplierContactId;
                        iteratedValue.subSupplierContactName = assignmentSubSuppliers[subSupplierIndex].subSupplierContactName;
                        iteratedValue.supplierContactValidation = required(assignmentSubSuppliers[subSupplierIndex].subSupplierContactId) ? localConstant.commonConstants.SUPPLIER_CONTACT_VALIDATION : "";
                    } else {
                        iteratedValue.isFirstVisitDisabled = true;
                    }
                    dispatch(FetchSubsupplierContacts(iteratedValue.supplierId)).then(response => {
                        if (Array.isArray(response) && response.length > 0) {
                            contactCount++;
                            const supplierId = response[0].supplierId;
                            const subSupplierData = subSuppliers.filter(x => x.supplierId === supplierId);
                            if (subSupplierData && subSupplierData.length > 0) {
                                subSupplierData[0].supplierContacts = response;
                                dispatch(UpdateSubSuppliers(subSupplierData[0]));
                            }
                            if (contactCount === subSuppliers.length) {
                                dispatch(subSupplierContactUpdateStatus(true));
                            }
                        }
                        else {
                            contactCount++;
                            if (contactCount === subSuppliers.length) 
                                dispatch(subSupplierContactUpdateStatus(true));//MS-TS NOV 22
                        }
                    });
                });
                return false;
            }
        }
    }
    else{
        dispatch(actions.FetchSubsuppliers([]));
        return false;
    }
};

export const UpdateSubSuppliers = (data) => async (dispatch, getstate) => 
{
    const state = getstate();
    const subsuppliers = state.rootAssignmentReducer.subsuppliers;
     let isMainSupplierFirstVisitStatus=false;
    //  const MainSupplierFirstVisit=state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers[0].isMainSupplierFirstVisit;
    let AssignmentSubSupplierDatas = state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers;
    AssignmentSubSupplierDatas = AssignmentSubSupplierDatas && AssignmentSubSupplierDatas.filter(x=>x.recordStatus!=="D");//DEC 18
    if (Array.isArray(AssignmentSubSupplierDatas) && AssignmentSubSupplierDatas.length > 0) {
        AssignmentSubSupplierDatas.forEach(items => {

            if (items.isMainSupplierFirstVisit) {
                isMainSupplierFirstVisitStatus=true;

            }
        });
    }

    const index = subsuppliers.findIndex(iteratedValue => iteratedValue.supplierId === data.supplierId);
    
    const newState = Object.assign([], subsuppliers);

    if (index >= 0) {
        /** if mainsupplier has first vist then to make subsupplier first visit status false */
        if(isMainSupplierFirstVisitStatus)
        {
            if(data.isSubSupplierFirstVisit===true)
            {
           data.isSubSupplierFirstVisit=false;
           IntertekToaster(localConstant.warningMessages.ONLY_ONE_SUPPLIER_lOCATION_ALLOWED, 'warningToast onlyonesupplierReqThirdscenario');
            }
            
        }
        newState[index] = data;
        
        dispatch(actions.UpdateSubSuppliers(newState));
        return true;
    }

};

export const FetchSubsupplierContacts =(data) => async (dispatch) =>{
    if(!required(data)){
        const url  = StringFormat(assignmentAPIConfig.supplierContact, data);
        const response = await FetchData(url, {})
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.assignments.ERROR_FETCHING_SUB_SUPPLIER_CONTACTS, 'dangerToast subSupplierContactsErr');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            return null;
        });
        if (!isEmpty(response) && !isEmpty(response.result)) {
            dispatch(actions.FetchSubsupplierContacts(response.result));
            return response.result;
        };
    }
    else{
        dispatch(actions.FetchSubsupplierContacts([]));
        return null;
    }
};
//D351
    export const FetchMainSupplierName =(supplierPOObj) =>async (dispatch) =>{
        if(!isEmpty(supplierPOObj)){
            dispatch(actions.FetchMainSupplierName(supplierPOObj.supplierPOMainSupplierName));
            dispatch(actions.FetchMainSupplierId(supplierPOObj.supplierPOMainSupplierId));
        } else {
            dispatch(actions.FetchMainSupplierName());
            dispatch(actions.FetchMainSupplierId());
        }
    };
    //D351
/**To check whether contact is updated in the json  or not */
export const subSupplierContactUpdateStatus = (isSubSupplierContactUpdated) => (dispatch, getstate) => {
    dispatch(actions.subSupplierContactUpdateStatus(isSubSupplierContactUpdated));
};

export const AddSubSupplierInformation =(data,editedData,isFromSupplierPO) =>  (dispatch,getstate) =>{
    const state=getstate();
    const assignmentInfo= state.rootAssignmentReducer.assignmentDetail.AssignmentInfo; //MS-TS 
    if(!isEmpty(data)){
    const editedRowData ={};
    editedRowData.assignmentSubSupplierId=data.assignmentSubSupplierId?data.assignmentSubSupplierId:null;
    editedRowData.assignmentId=data.assignmentId?data.assignmentId:null;
    editedRowData.subSupplierId=data.subSupplierId;
    editedRowData.supplierName=data.subSupplierName;
    editedRowData.supplierContactId=data.supplierContactId?data.supplierContactId:null;
    editedRowData.supplierContactName =data.supplierContactName?data.supplierContactName:null;
    editedRowData.mainSupplierContactName=data.mainSupplierContactName; 
    editedRowData.mainSupplierContactId=data.mainSupplierContactId;
    editedRowData.mainSupplierId=assignmentInfo.assignmentSupplierId; //MS-TS 
    editedRowData.isSubSupplierFirstVisit=(data.isSubSupplierFirstVisit !== null && data.isSubSupplierFirstVisit !== undefined)?data.isSubSupplierFirstVisit:null;
    editedRowData.isMainSupplierFirstVisit=data.isMainSupplierFirstVisit;
    editedRowData.assignmentSubSupplierTS=[];
    editedRowData.isPartofAssignment=(data.isPartofAssignment !== null && data.isPartofAssignment !== undefined)?data.isPartofAssignment:null;
    editedRowData.recordStatus=data.recordStatus;
    editedRowData.subSupplierId=data.supplierId;
    const newAssignmentSubSupplier = Object.assign([], state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers);
    const filterSubSupplier = newAssignmentSubSupplier.filter(x=>x.recordStatus !== 'D');
    if(newAssignmentSubSupplier.length !== 0){
        const index = newAssignmentSubSupplier.findIndex(value => (value.subSupplierId === data.subSupplierId));
        if(index >= 0){
            if(data.isPartofAssignment === false){
                     newAssignmentSubSupplier.splice(index,1);
                if(filterSubSupplier && filterSubSupplier.length === 1){
                    editedRowData.assignmentSubSupplierId=null;
                    editedRowData.assignmentId=null;
                    editedRowData.subSupplierId=null;
                    editedRowData.supplierName=null;
                    editedRowData.supplierContactId=null;
                    editedRowData.supplierContactName =null;
                    editedRowData.mainSupplierContactName=data.mainSupplierContactName;
                    editedRowData.mainSupplierContactId=data.mainSupplierContactId;
                    editedRowData.mainSupplierId=assignmentInfo.assignmentSupplierId; //MS-TS
                    editedRowData.isSubSupplierFirstVisit=false;
                    editedRowData.isMainSupplierFirstVisit=data.isMainSupplierFirstVisit;
                    editedRowData.assignmentSubSupplierTS=[];
                    editedRowData.isPartofAssignment=null;
                    editedRowData.recordStatus=data.recordStatus;
                    editedRowData.subSupplierId=null;

                    newAssignmentSubSupplier.push(editedRowData);
                }
            }
            else{
                newAssignmentSubSupplier[index]=Object.assign({},newAssignmentSubSupplier[index],editedRowData);
            }
        }
        else{
            if(filterSubSupplier.length === 1 && required(filterSubSupplier[0].supplierName)){
                const index = newAssignmentSubSupplier.findIndex(x=>(x.supplierName === null || x.supplierName === undefined));
                if(index >= 0){
                    newAssignmentSubSupplier.splice(index,1);
                }
            }
            newAssignmentSubSupplier.push(editedRowData);
        }
    }
    else{
        newAssignmentSubSupplier.push(editedRowData);
    }  
    const editedData = {
        assignmentSubSupplier : newAssignmentSubSupplier,
        isFromSupplierPO : isFromSupplierPO       //added for btnDisable property change
    };
    dispatch(actions.AddSubSupplierInformation(editedData));
}
   //subSupplier Data immutat
    if(!isEmpty(editedData)){
        const newSubsuppliers = Object.assign([], state.rootAssignmentReducer.subsuppliers);
        const newsubSupplierData={};
        newsubSupplierData.supplierContactName=data.supplierContactName;
        newsubSupplierData.supplierContactId=data.supplierContactId;
        newsubSupplierData.isPartofAssignment=data.isPartofAssignment;
        newsubSupplierData.isSubSupplierFirstVisit=data.isSubSupplierFirstVisit;
        newsubSupplierData.recordStatus=data.recordStatus;
       // newsubSupplierData.isSupplierContactDisabled=data.isSupplierContactDisabled;
        newsubSupplierData.isFirstVisitDisabled=data.isFirstVisitDisabled;
        newsubSupplierData.supplierContactValidation=data.supplierContactValidation;
        const editedSupplierRow = Object.assign({}, editedData, newsubSupplierData);
        const index = state.rootAssignmentReducer.subsuppliers.findIndex(subsuppliers => subsuppliers.subSupplierId === editedSupplierRow.subSupplierId);
        newSubsuppliers[index] = editedSupplierRow;
        if (index >= 0) {
        dispatch(actions.FetchSubsuppliers(newSubsuppliers));
        }
    }
};

export const UpdateSupplierInformation=(data,editedData) =>  (dispatch,getstate) =>{
    const state=getstate();
    const assignmentInfo= state.rootAssignmentReducer.assignmentDetail.AssignmentInfo; //MS-TS 
    const editedRowData ={};
    if(!isEmpty(data)){
    editedRowData.supplierName=data.subSupplierName;
    editedRowData.subSupplierId=data.subSupplierId;
    editedRowData.supplierContactId=data.supplierContactId;
    editedRowData.supplierContactName =data.supplierContactName;
    editedRowData.mainSupplierContactName=data.mainSupplierContactName?data.mainSupplierContactName:null;
    editedRowData.mainSupplierContactId=data.mainSupplierContactId?data.mainSupplierContactId:null;
    editedRowData.mainSupplierId=assignmentInfo.assignmentSupplierId;//MS-TS
    editedRowData.assignmentId=data.assignmentId;
    editedRowData.isSubSupplierFirstVisit=data.isSubSupplierFirstVisit?data.isSubSupplierFirstVisit:false;
    editedRowData.isMainSupplierFirstVisit=data.isMainSupplierFirstVisit;
    editedRowData.isPartofAssignment=data.isPartofAssignment?data.isPartofAssignment:false;
    editedRowData.recordStatus=data.recordStatus;
    editedRowData.subSupplierId=data.supplierId;
    const newAssignmentSubSupplier = Object.assign([], state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers);
    if(newAssignmentSubSupplier.length !== 0){
        const index = newAssignmentSubSupplier.findIndex(value => (value.subSupplierId === data.supplierId)); //MS-TS Link CR
        if(index >= 0){
            if(data.isPartofAssignment){

                newAssignmentSubSupplier[index]=Object.assign({},newAssignmentSubSupplier[index],editedRowData);
            }
            else{
                /** TO Delete Subsuppliers only there is no firstvisit present */
                if(data.isSubSupplierFirstVisit===false || required(data.isSubSupplierFirstVisit) )
                {
                if(editedRowData.recordStatus !== 'N'){
                     editedRowData.recordStatus='D';
                    newAssignmentSubSupplier[index]=Object.assign({},newAssignmentSubSupplier[index],editedRowData);
                }
                // else{
                //     newAssignmentSubSupplier.splice(index,1);//MS-TS
                // }
            }
            
            }
        }
        else{
            newAssignmentSubSupplier.push(editedRowData);
        }
    }
    else{
        newAssignmentSubSupplier.push(editedRowData);
    }  
    dispatch(actions.UpdateSupplierInformation(newAssignmentSubSupplier));
}
   // subSupplier Data immutat
    if(!isEmpty(editedData)){
        const newSubsuppliers = Object.assign([], state.rootAssignmentReducer.subsuppliers);
        const newsubSupplierData={};
        newsubSupplierData.supplierContactName=data.supplierContactName;
        newsubSupplierData.supplierContactId=data.supplierContactId;
        newsubSupplierData.isPartofAssignment=data.isPartofAssignment;
        newsubSupplierData.isFirstVisit=data.isFirstVisit;
        newsubSupplierData.recordStatus=data.recordStatus;
        if(data.isPartofAssignment === false){
            newsubSupplierData.recordStatus=editedRowData.recordStatus;
        }
        const editedSupplierRow = Object.assign({}, editedData, newsubSupplierData);
        const index = state.rootAssignmentReducer.subsuppliers.findIndex(subsuppliers => subsuppliers.subSupplierId === editedSupplierRow.subSupplierId);
        newSubsuppliers[index] = editedSupplierRow;
        if (index >= 0) {
        dispatch(actions.FetchSubsuppliers(newSubsuppliers));
        }
    }
};

export const AddMainSupplierContactInformation =(data) => (dispatch,getstate) =>{
    const state=getstate();
    const assignmentInfo = Object.assign({}, state.rootAssignmentReducer.assignmentDetail.AssignmentInfo,data);
    dispatch(actions.AddMainSupplierContactInformation(assignmentInfo));
};

export const AddSupSupplierTechSpec = (data,SupplierId) => (dispatch,getstate) =>{
    const state=getstate();
    const assignmentSubSupplier=Object.assign([],state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers);
    // Instead of subSupplierSupplierId, compare with subSupplierId and also filter it with recordStatus !== D
    const subsupplierIndx=assignmentSubSupplier.findIndex(value => value.subSupplierId === SupplierId);  //MS-TS Link CR
    const assignmentSupsupplierTechSpec= Object.assign([],state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers[subsupplierIndx].assignmentSubSupplierTS);
    const techSpecIndex=assignmentSupsupplierTechSpec.findIndex(techSpec => techSpec.epin === data.epin && techSpec.isAssignedToThisSubSupplier !== false && techSpec.recordStatus !== 'D');
            if(techSpecIndex >= 0){
                if(data.isAssignedToThisSubSupplier === false){
                    if(assignmentSupsupplierTechSpec[techSpecIndex].recordStatus !== 'N'){
                        assignmentSupsupplierTechSpec[techSpecIndex].recordStatus = 'D';
                    }
                    else{
                        assignmentSupsupplierTechSpec.splice(techSpecIndex,1);
                    }
                }else{
                    if(assignmentSupsupplierTechSpec[techSpecIndex].recordStatus !== 'N'){
                        data.recordStatus = 'M';
                    }
                    assignmentSupsupplierTechSpec[techSpecIndex]=Object.assign({},data,assignmentSupsupplierTechSpec[techSpecIndex]);
                }
            } 
            else{
                data.recordStatus = 'N';
                assignmentSupsupplierTechSpec.push(data);
            }
            assignmentSubSupplier[subsupplierIndx].assignmentSubSupplierTS=assignmentSupsupplierTechSpec;
            if(assignmentSubSupplier[subsupplierIndx].recordStatus !== 'N'){
                assignmentSubSupplier[subsupplierIndx].recordStatus = 'M';
            }
            dispatch(actions.AddSupSupplierTechSpec(assignmentSubSupplier)); 
};

// export const UpdateTechSpecAssignedSubSupplier =(data) => (dispatch,getstate)=>{
//     const state=getstate();
//     dispatch(actions.UpdateTechSpecAssignedSubSupplier(data)); 
// };

export const FetchAssignmentTechSpec = (data)=>(dispatch,getstate) =>{
    dispatch(actions.FetchAssignmentTechSpec([]));
    dispatch(actions.FetchAssignmentTechSpec(data));
};

export const UpdateMainSupplierInfo = (data) => (dispatch, getstate) => {
    const state = getstate();
    const assignmentSubSupplier = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers);
    // const assignmentOldSubSupplier = isEmptyReturnDefault(state.rootAssignmentReducer.AssignmentSubSupplierOld); // unwanted code
  const filterSubSupplier = assignmentSubSupplier.filter(x => x.recordStatus !== 'D');
    
    if (filterSubSupplier && filterSubSupplier.length > 0) {
        // const updateSubSupplier = [];
        assignmentSubSupplier.forEach((iteratedValue, index) => {
            if (iteratedValue.recordStatus !== 'D') {
                if (iteratedValue.recordStatus !== 'N') {
                    iteratedValue.recordStatus = 'M';
                }
                if (iteratedValue.assignmentSubSupplierTS === null) {
                    iteratedValue.assignmentSubSupplierTS = [];
                }
                iteratedValue = Object.assign({}, iteratedValue, data);
                assignmentSubSupplier[index] = iteratedValue;
            }
        });
        const obj = {
            AssignmentSubSupplierNewData: assignmentSubSupplier,
        };
        dispatch(actions.UpdateMainSupplierInfo(obj));
    }
//MS_TS changes Nov_25 - 1MS scenario - SS removed part of Assignment scenario
    if(filterSubSupplier.length === 0 )
    {
        assignmentSubSupplier.forEach((iteratedValue, index) => {
                if (iteratedValue.recordStatus !== 'N') {
                    iteratedValue.recordStatus = 'M';
                }
                if (iteratedValue.assignmentSubSupplierTS === null) {
                    iteratedValue.assignmentSubSupplierTS = [];
                }
                iteratedValue = Object.assign({}, iteratedValue, data);
                assignmentSubSupplier[index] = iteratedValue;
           // }
        });
        const obj = {
            AssignmentSubSupplierNewData: assignmentSubSupplier,
        };
        dispatch(actions.UpdateMainSupplierInfo(obj));
    }
    //MS_TS changes Nov_25

// //MSTS
//     if(filterSubSupplier.length === 0)
//     {
//         const mainSupplierData = Object.assign({}, assignmentSubSupplier, data);
//         assignmentSubSupplier[0] = mainSupplierData;
//         const obj = {
//             AssignmentSubSupplierNewData: assignmentSubSupplier[0],
//         };
//         dispatch(actions.UpdateMainSupplierInfo(obj));
//     }
//     //MSTS
    // if(filterSubSupplier.length === 1 && required(filterSubSupplier[0].supplierName)){
    //     if(assignmentSubSupplier[0])
    //     const updateSupplier = Object.assign({},assignmentSubSupplier[0],data);
    //     assignmentSubSupplier[0] = updateSupplier;
    //     dispatch(actions.UpdateMainSupplierInfo(assignmentSubSupplier));
    // }
    // else{
    //     const updateSubSupplier = [];
    //     assignmentSubSupplier.forEach(iteratedValue => {
    //         iteratedValue = Object.assign({},iteratedValue,data);
    //         updateSubSupplier.push(iteratedValue);
    //     });
    //     dispatch(actions.UpdateMainSupplierInfo(updateSubSupplier));
    // }
};

/** Delete Supplier Related informations */
export const deletSupplierInformations = () => (dispatch,getstate) => {
    const state = getstate();
    const assignmentSubSupplier = Object.assign([],state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers);
    if(assignmentSubSupplier.length > 0){
        const subSupplierCopy = deepCopy(assignmentSubSupplier);
        assignmentSubSupplier.forEach((iteratedValue,index) => {
            const subSupplierCopyIndex = subSupplierCopy.findIndex(x => x.subSupplierId === iteratedValue.subSupplierId);
            if(subSupplierCopyIndex >= 0){
                if(iteratedValue.recordStatus === 'N'){
                    subSupplierCopy.splice(subSupplierCopyIndex,1);
                }else{
                    const subSupplierTSCopy = deepCopy(isEmptyReturnDefault(iteratedValue.assignmentSubSupplierTS));
                    iteratedValue.assignmentSubSupplierTS && iteratedValue.assignmentSubSupplierTS.forEach(x => {
                        const subSupplierTSCopyIndex = subSupplierTSCopy.findIndex(y => y.epin === x.epin);
                        if(subSupplierTSCopyIndex >= 0){
                            if(x.recordStatus === 'N')
                                subSupplierTSCopy[subSupplierTSCopyIndex].splice(subSupplierTSCopyIndex,1);
                            else
                                subSupplierTSCopy[subSupplierTSCopyIndex].recordStatus = 'D';
                        }
                    });
                    subSupplierCopy[subSupplierCopyIndex].assignmentSubSupplierTS = subSupplierTSCopy;
                    subSupplierCopy[subSupplierCopyIndex].recordStatus = 'D';
                }
            }
        });
        dispatch(actions.deletSupplierInformations(subSupplierCopy));
        return true;
    }
    return false;
};

/** Delete supplier TS */
export const deleteSubSupplierTS = (data) => (dispatch,getstate) => {
    if(data){
        const state = getstate();
        const assignmentSubSuppliers = Object.assign([],state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers);
        const supplierIdFromLocation = data.supplierLocationId && data.supplierLocationId.split('_')[0];
        /** Assignment Sub-Supplier TS Delete */
        // Instead of subSupplierSupplierId, check it for subSupplierId
        const subSupplierIndex = assignmentSubSuppliers.findIndex(x => x.subSupplierId == supplierIdFromLocation && x.recordStatus !== "D");
        const mainSupplier = assignmentSubSuppliers.filter(x => x.mainSupplierId == supplierIdFromLocation && x.recordStatus !== "D");
        if(subSupplierIndex >= 0){
            const subSupplierTS = Object.assign([],assignmentSubSuppliers[subSupplierIndex].assignmentSubSupplierTS);
            const subSupplierTSIndex = subSupplierTS.findIndex(x => x.epin === data.epin && x.recordStatus !== "D" && (x.isAssignedToThisSubSupplier == null || x.isAssignedToThisSubSupplier == true));
            if(subSupplierTSIndex >= 0){
                if(subSupplierTS[subSupplierTSIndex].recordStatus !== "N") {
                    subSupplierTS[subSupplierTSIndex].recordStatus = "D";
                    subSupplierTS[subSupplierTSIndex].isDeleted = true;
                }
                else {
                    subSupplierTS.splice(subSupplierTSIndex, 1);
                }
            }
            assignmentSubSuppliers[subSupplierIndex].assignmentSubSupplierTS = subSupplierTS;
            if(assignmentSubSuppliers[subSupplierIndex].recordStatus !== 'N')
                assignmentSubSuppliers[subSupplierIndex].recordStatus = "M";
        }
        /** Assignment Main Supplier TS Delete */
        if(mainSupplier && mainSupplier.length > 0){
            assignmentSubSuppliers.forEach(x => {
                const mainSupplierTS = Object.assign([],x.assignmentSubSupplierTS);
                const mainSupplierTSIndex = mainSupplierTS.findIndex(x => x.epin === data.epin && x.recordStatus !== "D" && x.isAssignedToThisSubSupplier == false);
                if(mainSupplierTSIndex >= 0){
                    if (mainSupplierTS[mainSupplierTSIndex].recordStatus !== "N") {
                        mainSupplierTS[mainSupplierTSIndex].recordStatus = "D";
                        mainSupplierTS[mainSupplierTSIndex].isDeleted = true; 
                    }
                    else {
                        mainSupplierTS.splice(mainSupplierTSIndex, 1);
                    }
                }
                x.assignmentSubSupplierTS = mainSupplierTS;
                if(x.recordStatus !== 'N')
                    x.recordStatus = "M";
            });
        }
        dispatch(actions.deleteSubSupplierTS(assignmentSubSuppliers));
    }
};

/** Delete NDT Sub-Supplier TS */
export const deleteNDTSubSupplierTS = (data,subSupplierId) => (dispatch,getstate) => {
    if(data && subSupplierId){
        const state = getstate();
        const assignmentSubSuppliers = Object.assign([],state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers);
        // Instead of subSupplierSupplierId, check with subSupplierId and recordStatus !== D
        const subSupplierIndex = assignmentSubSuppliers.findIndex(x => x.subSupplierId === subSupplierId);
        if(subSupplierIndex >= 0){
            const subSupplierTS = Object.assign([],assignmentSubSuppliers[subSupplierIndex].assignmentSubSupplierTS);
            //const tsIndex = subSupplierTS.findIndex(x =>x.epin === data.epin && x.recordStatus !== 'D');
            const subSupplierTSArray = [];
            for(let i=subSupplierTS.length-1; i >= 0; i--){ //Changes for D1320
                const iteratedValue = subSupplierTS[i];
                if(iteratedValue !== undefined){
                    if(iteratedValue.epin === data.epin && iteratedValue.recordStatus !== 'D'){
                        if(subSupplierTS[i].recordStatus !== 'N'){
                            subSupplierTS[i].recordStatus = "D";
                            if(subSupplierTSArray.indexOf(subSupplierTS[i]) === -1){
                                subSupplierTSArray.push(subSupplierTS[i]);
                            }
                        }
                    }
                    else{
                        if(subSupplierTSArray.indexOf(subSupplierTS[i]) === -1){
                            subSupplierTSArray.push(subSupplierTS[i]);
                        }
                    }
                }
            }
            assignmentSubSuppliers[subSupplierIndex].assignmentSubSupplierTS = subSupplierTSArray;
            if(assignmentSubSuppliers[subSupplierIndex].recordStatus !== 'N')
                assignmentSubSuppliers[subSupplierIndex].recordStatus = "M";
            dispatch(actions.deleteSubSupplierTS(assignmentSubSuppliers));
            //if(tsIndex >= 0){
                // if(subSupplierTS[tsIndex].recordStatus !== 'N')
                //     subSupplierTS[tsIndex].recordStatus = "D";
                // else
                //     subSupplierTS.splice(tsIndex,1);
                // assignmentSubSuppliers[subSupplierIndex].assignmentSubSupplierTS = subSupplierTS;
                // if(assignmentSubSuppliers[subSupplierIndex].recordStatus !== 'N')
                //     assignmentSubSuppliers[subSupplierIndex].recordStatus = "M";
                // dispatch(actions.deleteSubSupplierTS(assignmentSubSuppliers));
            //}
        }
    }
};

export const deleteNDTMainSupplierTS = (data) => (dispatch,getstate) => {
    const state = getstate();
    /** Assignment Main Supplier TS Delete */
    if(data){
        const assignmentSubSuppliers = Object.assign([],state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers);
        assignmentSubSuppliers.forEach(x => {
            if(x.recordStatus !== 'D'){
                const mainSupplierTS = Object.assign([],x.assignmentSubSupplierTS);
                const mainSupplierTSIndex = mainSupplierTS.findIndex(x => x.epin === data.epin && x.recordStatus !== "D");
                if(mainSupplierTSIndex >= 0){
                    if(mainSupplierTS[mainSupplierTSIndex].recordStatus !== "N")
                        mainSupplierTS[mainSupplierTSIndex].recordStatus = "D";
                    else
                        mainSupplierTS.splice(mainSupplierTSIndex,1); 
                }
                x.assignmentSubSupplierTS = mainSupplierTS;
                if(x.recordStatus !== 'N')
                    x.recordStatus = "M";
            }
        });
        dispatch(actions.deleteSubSupplierTS(assignmentSubSuppliers));
    }
};

export const updatePartOfAssignment = (rowData, updatedData) => async(dispatch,getstate) => {
    const state = getstate();
    const currentPage = state.CommonReducer.currentPage;
    const subSupplierList = isEmptyReturnDefault(state.rootAssignmentReducer.subsuppliers);
    const subSupplierListIndex = subSupplierList.findIndex(x => x.supplierId === rowData.supplierId);
    const assignmentInfo= isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo,'object');
    const assignmentSubSupplier = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers);
    const filteredSubSupplier = assignmentSubSupplier.filter(x => x.recordStatus !== 'D');
    const assignmentTechSpec = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentTechnicalSpecialists);
    const selectedSubSupplierIndex = assignmentSubSupplier.findIndex(x => x.subSupplierId === rowData.supplierId && x.recordStatus !== 'D');

    let isFirstVisitDiable = false;
    assignmentSubSupplier.forEach(x => {
        const techSpec = getNestedObject(x,[ 'assignmentSubSupplierTS' ]);
        if(Array.isArray(techSpec) && techSpec.length > 0)
            isFirstVisitDiable = true;
    });

    /** AssignmentSubSupplier property update section - starts */
    if(updatedData.isPartofAssignment){
        const subSupplierIndex = assignmentSubSupplier.findIndex(x => x.recordStatus !== 'D');
        if(subSupplierIndex >= 0 && required(assignmentSubSupplier[subSupplierIndex].subSupplierId)){
            if(filteredSubSupplier.length === 1 && filteredSubSupplier[0].recordStatus !== 'N'){
                const subSupplier = {};
                const mainSupplierData = filteredSubSupplier.length > 0 ? filteredSubSupplier[0] : {};
                subSupplier.assignmentId = assignmentInfo.assignmentId;
                subSupplier.assignmentSubSupplierId = null;
                subSupplier.subSupplierId = rowData.supplierId;
                subSupplier.subSupplierName = rowData.subSupplierName;
                subSupplier.mainSupplierId = mainSupplierData.mainSupplierId;
                subSupplier.mainSupplierContactId = mainSupplierData.mainSupplierContactId;
                subSupplier.mainSupplierContactName = mainSupplierData.mainSupplierContactName;
                subSupplier.isMainSupplierFirstVisit = mainSupplierData.isMainSupplierFirstVisit;
                subSupplier.supplierType = 'S';
                subSupplier.assignmentSubSupplierTS = [];
                subSupplier.isSubSupplierFirstVisit = false;
                subSupplier.isPartofAssignment = updatedData.isPartofAssignment;
                subSupplier.recordStatus = 'N';
    
                assignmentSubSupplier.push(subSupplier);
                dispatch(actions.UpdateSupplierInformation(assignmentSubSupplier));
            } else {
                const selectedSubSupplier = isEmptyReturnDefault(assignmentSubSupplier[subSupplierIndex],'object');
                selectedSubSupplier.assignmentSubSupplierId = selectedSubSupplier.assignmentSubSupplierId ? selectedSubSupplier.assignmentSubSupplierId : null;
                selectedSubSupplier.subSupplierId = rowData.supplierId;
                selectedSubSupplier.subSupplierName = rowData.subSupplierName;
                selectedSubSupplier.supplierType = 'S';
                selectedSubSupplier.isSubSupplierFirstVisit = false;
                selectedSubSupplier.isPartofAssignment = updatedData.isPartofAssignment;
                selectedSubSupplier.recordStatus = (selectedSubSupplier.recordStatus !== 'N') ? 'M' : selectedSubSupplier.recordStatus;
    
                assignmentSubSupplier[subSupplierIndex] = selectedSubSupplier;
                dispatch(actions.UpdateSupplierInformation(assignmentSubSupplier));
            }
        }
        else{
            const subSupplier = {};
            const mainSupplierData = filteredSubSupplier.length > 0 ? filteredSubSupplier[0] : {};
            subSupplier.assignmentId = assignmentInfo.assignmentId;
            subSupplier.assignmentSubSupplierId = null;
            subSupplier.subSupplierId = rowData.supplierId;
            subSupplier.subSupplierName = rowData.subSupplierName;
            subSupplier.mainSupplierId = mainSupplierData.mainSupplierId;
            subSupplier.mainSupplierContactId = mainSupplierData.mainSupplierContactId;
            subSupplier.mainSupplierContactName = mainSupplierData.mainSupplierContactName;
            subSupplier.isMainSupplierFirstVisit = mainSupplierData.isMainSupplierFirstVisit;
            subSupplier.supplierType = 'S';
            subSupplier.assignmentSubSupplierTS = [];
            subSupplier.isSubSupplierFirstVisit = false;
            subSupplier.isPartofAssignment = updatedData.isPartofAssignment;
            subSupplier.recordStatus = 'N';

            assignmentSubSupplier.push(subSupplier);
            dispatch(actions.UpdateSupplierInformation(assignmentSubSupplier));
        }
    } else {
        if(selectedSubSupplierIndex >= 0){
            assignmentSubSupplier[selectedSubSupplierIndex].isPartofAssignment = updatedData.isPartofAssignment;
            // const filteredSubSupplier = assignmentSubSupplier.filter(x => x.recordStatus !== 'D');
            if(assignmentSubSupplier[selectedSubSupplierIndex].recordStatus !== 'N'){
                // assignmentSubSupplier[selectedSubSupplierIndex].recordStatus = 'D';
                if(filteredSubSupplier.length == 1){
                    
                    const firstSubSupplierObj = deepCopy(filteredSubSupplier[0]);

                    const subSupplier = {};
                    subSupplier.assignmentId = firstSubSupplierObj.assignmentId;
                    subSupplier.assignmentSubSupplierId = firstSubSupplierObj.assignmentSubSupplierId;
                    subSupplier.subSupplierId = firstSubSupplierObj.subSupplierId;
                    subSupplier.subSupplierName = firstSubSupplierObj.subSupplierName;
                    subSupplier.mainSupplierId = null;
                    subSupplier.mainSupplierContactId = null;
                    subSupplier.mainSupplierContactName = null;
                    subSupplier.isMainSupplierFirstVisit = firstSubSupplierObj.isMainSupplierFirstVisit;
                    subSupplier.assignmentSubSupplierTS = firstSubSupplierObj.assignmentSubSupplierTS;
                    subSupplier.isSubSupplierFirstVisit = firstSubSupplierObj.isSubSupplierFirstVisit;
                    subSupplier.isPartofAssignment = isSubSupplierFirstVisit.isPartofAssignment;
                    subSupplier.recordStatus = 'D';

                    assignmentSubSupplier[selectedSubSupplierIndex].subSupplierId = null;
                    assignmentSubSupplier[selectedSubSupplierIndex].subSupplierName = null;
                    assignmentSubSupplier[selectedSubSupplierIndex].isSubSupplierFirstVisit = false;
                    assignmentSubSupplier[selectedSubSupplierIndex].subSupplierContactId = null;
                    assignmentSubSupplier[selectedSubSupplierIndex].subSupplierContactName = null;

                    assignmentSubSupplier.push(subSupplier);
                } else {
                    assignmentSubSupplier[selectedSubSupplierIndex].recordStatus = 'D';
                }
                dispatch(actions.UpdateSupplierInformation(assignmentSubSupplier));
            }
            else {
                if(filteredSubSupplier.length == 1){
                    assignmentSubSupplier[selectedSubSupplierIndex].subSupplierId = null;
                    assignmentSubSupplier[selectedSubSupplierIndex].subSupplierName = null;
                    assignmentSubSupplier[selectedSubSupplierIndex].isSubSupplierFirstVisit = false;
                    assignmentSubSupplier[selectedSubSupplierIndex].subSupplierContactId = null;
                    assignmentSubSupplier[selectedSubSupplierIndex].subSupplierContactName = null;
                } else {
                    assignmentSubSupplier.splice(selectedSubSupplierIndex,1);
                }
                dispatch(actions.UpdateSupplierInformation(assignmentSubSupplier));
            }
        }
    }
    /** AssignmentSubSupplier property update section - ends */
    
    /** Sub-Supplier list property update section - starts */
    if(subSupplierListIndex >= 0){
        const subSupplierListData = isEmptyReturnDefault(subSupplierList[subSupplierListIndex],'object');
        subSupplierListData.subSupplierContactId = null;
        subSupplierListData.subSupplierContactName = null;
        subSupplierListData.supplierContactValidation = updatedData.isPartofAssignment ? localConstant.commonConstants.SUPPLIER_CONTACT_VALIDATION : "";
        subSupplierListData.isFirstVisitDisabled = isFirstVisitDiable ? isFirstVisitDiable : !updatedData.isPartofAssignment;
        subSupplierListData.isPartofAssignment = updatedData.isPartofAssignment;
        subSupplierListData.isSubSupplierFirstVisit = false;

        subSupplierList[subSupplierListIndex] = subSupplierListData;
        dispatch(actions.UpdateSubSuppliers(subSupplierList));
    }
    /** Sub-Supplier list property update section - ends */
    return true;
};
/** Part of Assignment Update Action 1:
 * if isPartOfAssignment
 *      if assignmentSubSupplier.length > 0 and has sub-supplier id (excluding recordStatus D) 
 *          then add below mention props as seperate Object to AssignmentSubSupplier
 *          isSubSupplierPartOfAssignment : true
 *          isSubSupplierFirstVisit : false
 *          subSupplier related information
 *          mainSupplier related information
 *          recordStatus as N
 *          subSupplierTS array with main supplier TS too.
 *      else assignmentSubSupplier.length > 0 and not has sub-supplier id (excluding recordStatus D) 
 *          update the first index without recordStatus D with...
 *          isSubSupplierPartOfAssignment : true
 *          isSubSupplierFirstVisit : false
 *          subSupplier related information
 *          mainSupplier related information
 *          recordStatus as N
 * else
 *      get the sub-supplier Id with record status is not D from AssignmentSubSupplier
 *      if recordStatus is N ? splice : change the status to D.
 */

/** Part of Assignment Update Action 2:
 * hasAssignedResource
 * if isPartOfAssignment
 *      if subSuppliers.length > 0 and has sub-supplier id 
 *          add sub-supplier contact validation
 *          contact id and name as empty
 *          isSubSupplierPartOfAssignment : true
 *          isSubSupplierFirstVisit : false
 *          isContactDisable : false
 *          isFirstVisitDisable : hasAssignedResource
 * else
 *      if subSuppliers.length > 0 and has sub-supplier id 
 *          add sub-supplier contact validation
 *          contact id and name as empty
 *          isSubSupplierPartOfAssignment : false
 *          isSubSupplierFirstVisit : false
 *          isContactDisable : true
 *          isFirstVisitDisable : true
 */

export const updateSubSupplierContact = (rowData,updatedData) => async(dispatch,getstate) => {
    const state = getstate();
    const subSupplierList = isEmptyReturnDefault(state.rootAssignmentReducer.subsuppliers);
    const subSupplierListIndex = subSupplierList.findIndex(x => x.supplierId === rowData.supplierId);
    const assignmentSubSupplier = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers);
    const subSupplierIndex = assignmentSubSupplier.findIndex(x => x.subSupplierId === rowData.supplierId && x.recordStatus !== 'D');
    
    /** AssignmentSubSupplier property update section - starts */
    if(subSupplierIndex >= 0){
        assignmentSubSupplier[subSupplierIndex].subSupplierContactId = updatedData.subSupplierContactId;
        assignmentSubSupplier[subSupplierIndex].subSupplierContactName = updatedData.subSupplierContactName;
        assignmentSubSupplier[subSupplierIndex].recordStatus = assignmentSubSupplier[subSupplierIndex].recordStatus !== 'N' ? 'M' : assignmentSubSupplier[subSupplierIndex].recordStatus;

        dispatch(actions.UpdateSupplierInformation(assignmentSubSupplier));
    }
    /** AssignmentSubSupplier property update section - ends */

    /** Sub-Supplier list property update section - starts */
    if(subSupplierListIndex >= 0){
        subSupplierList[subSupplierListIndex].subSupplierContactId = updatedData.subSupplierContactId;
        subSupplierList[subSupplierListIndex].subSupplierContactName = updatedData.subSupplierContactName;
        subSupplierList[subSupplierListIndex].supplierContactValidation = required(updatedData.subSupplierContactId) ? localConstant.commonConstants.SUPPLIER_CONTACT_VALIDATION : "";

        dispatch(actions.UpdateSubSuppliers(subSupplierList));
    }
    /** Sub-Supplier list property update section - ends */
    return true;
};

/** Sub-Supplier Contact Update Action 1:
 * if isPartOfAssignment
 *      if data.subSupplierId === assignmentSubSupplier.subSupplierId && recordStatus !== D
 *          update the contact
 *          recordStatus !== N ? M : recordStatus
 */

/** Sub-Supplier Contact Update Action 2:
 * if data.subSupplierId === subSuppliers.supplierId
 *      update the contact
 *      if contactId is empty ? update contact validation : remove contact validation
 */

export const updateSubSupplierFirstVisit = (rowData,updatedData) => async(dispatch,getstate) => {
    const state = getstate();
    const subSupplierList = isEmptyReturnDefault(state.rootAssignmentReducer.subsuppliers);
    const subSupplierListIndex = subSupplierList.findIndex(x => x.supplierId === rowData.supplierId);
    const assignmentSubSupplier = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers);
    const subSupplierIndex = assignmentSubSupplier.findIndex(x => x.subSupplierId === rowData.supplierId && x.recordStatus !== 'D');
    /** AssignmentSubSupplier property update section - starts */
    if(subSupplierIndex >= 0){
        assignmentSubSupplier[subSupplierIndex].isSubSupplierFirstVisit = updatedData.isSubSupplierFirstVisit;
        assignmentSubSupplier[subSupplierIndex].recordStatus = assignmentSubSupplier[subSupplierIndex].recordStatus !== 'N' ? 'M' : assignmentSubSupplier[subSupplierIndex].recordStatus;

        dispatch(actions.UpdateSupplierInformation(assignmentSubSupplier));
    }
    /** AssignmentSubSupplier property update section - ends */

    /** Sub-Supplier list property update section - starts */
    if(subSupplierListIndex >= 0){
        subSupplierList[subSupplierListIndex].isSubSupplierFirstVisit = updatedData.isSubSupplierFirstVisit;

        dispatch(actions.UpdateSubSuppliers(subSupplierList));
    }
    /** Sub-Supplier list property update section - ends */
    return true;
};

/** Sub-Supplier FirstVisit Update Action 1:
 * if isPartOfAssignment
 *      if data.subSupplierId === assignmentSubSupplier.subSupplierId && recordStatus !== D
 *          update isSubSupplierFirstVisit
 *          recordStatus !== N ? M : recordStatus
 */

/** Sub-Supplier FirstVisit Update Action 2:
 *      if data.subSupplierId === assignmentSubSupplier.subSupplierId && recordStatus !== D
 *          update isSubSupplierFirstVisit
 */

export const addMainSupplierData = (supplierPOObj) => (dispatch,getstate) => {
    const state = getstate();
    const assignmentData = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail,'object');
    const assignmentId = getNestedObject(assignmentData,
    [ "AssignmentInfo", "assignmentId" ]);
    const assignmentSubSupplier = isEmptyReturnDefault(assignmentData.AssignmentSubSuppliers);
    if(!isEmpty(supplierPOObj)){
        const mainSupplierData = {};
        mainSupplierData["assignmentSubSupplierId"]= null;
        mainSupplierData["assignmentId"]= assignmentId ? assignmentId : null;
        mainSupplierData["subSupplierId"]= null;
        mainSupplierData["subSupplierName"]= null;
        mainSupplierData["subSupplierContactId"]= null;
        mainSupplierData["subSupplierContactName"]= null;
        mainSupplierData["mainSupplierContactName"]= null;
        mainSupplierData["mainSupplierContactId"]= null;
        mainSupplierData["isSubSupplierFirstVisit"]= null;
        mainSupplierData["assignmentSubSupplierTS"]= [];
        mainSupplierData["recordStatus"]= "N";
        mainSupplierData["mainSupplierId"]= supplierPOObj.supplierPOMainSupplierId ? supplierPOObj.supplierPOMainSupplierId:null;
        mainSupplierData["mainSupplierName"]= supplierPOObj.supplierPOMainSupplierName ? supplierPOObj.supplierPOMainSupplierName:null;
        dispatch(FetchSupplierContacts(supplierPOObj.supplierPOMainSupplierId));
        dispatch(FetchSubsuppliers(supplierPOObj.supplierPOId,assignmentData)).then(response =>{
            mainSupplierData['isMainSupplierFirstVisit'] = response;
            assignmentSubSupplier.push(mainSupplierData);
            dispatch(actions.UpdateSupplierInformation(assignmentSubSupplier));
        });
    }
};