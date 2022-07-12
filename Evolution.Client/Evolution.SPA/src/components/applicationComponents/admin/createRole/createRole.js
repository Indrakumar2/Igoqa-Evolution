import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './createRoleHeader';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, formInputChangeHandler,parseQueryParam,isTrue } from '../../../../utils/commonUtils';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import CustomModal from '../../../../common/baseComponents/customModal';
import arrayUtil from '../../../../utils/arrayUtil';
import Modal from '../../../../common/baseComponents/modal';
import ErrorList from '../../../../common/baseComponents/errorList';
import { activitycode,levelSpecificActivities,contractActivityCode,assignmentActivityCode,custmoduleList,supplierPOActivityCode  } from '../../../../constants/securityConstant'; 
import { isEmpty , sortModuleCustomerwiseList } from '../../../../utils/commonUtils';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
const localConstant = getlocalizeData();

export const CreateRoleDiv = (props) => (
    <form onSubmit={props.saveRole} onReset={props.clearRoleData} autoComplete="off">
        <div className="row mb-4 dividerRow">
         <span className="boldBorder"></span>
        <h6 className="col s9  mt-3"><span className="bold">User Roles :</span> Create Roles</h6>
        <div className="col s3 right-align mt-3" >
                <button type="submit" className=" ml-1 waves-effect btn ">{localConstant.admin.SAVE} </button>
                <button type="reset" className="ml-1 waves-effect btn" >{localConstant.admin.REFRESHCANCEL}</button>
            </div>
            <CustomInput
                hasLabel={true}
                name="roleName"            
                colSize='s4 pr-0'
                label={localConstant.admin.ROLE_NAME}
                labelClass="customLabel mandate"
                type='text'
                inputClass="customInputs"
                //required={true}
                maxLength={fieldLengthConstants.admin.userRoles.ROLE_NAME_MAXLENGTH}
                onValueChange={props.inputHandleChange}
                defaultValue={props.selectedRole.roleName}
                disabled={ (props.history.location.pathname).includes("UpdateRole") }
            />
            <CustomInput
                hasLabel={true}
                name="description"
                colSize='s8'
                label={localConstant.admin.DESCRIPTION}
                type='text'
                inputClass="customInputs"
                maxLength={fieldLengthConstants.admin.userRoles.DESCRIPTION_MAXLENGTH}
                onValueChange={props.inputHandleChange}
                defaultValue={props.selectedRole.description} />
           
        </div>
    </form>
);

export const ModuleTypeDiv = (props) => (
         <div className="row mb-0 dividerRow">
         <span className="boldBorder"></span>
            <div className="col s12 pr-0">
                <CustomInput
                    hasLabel={true}
                    name="module"
                    divClassName='col pl-0'
                    colSize='s4'
                    label={localConstant.admin.MODULE}
                    type='select'
                    inputClass="customInputs"
                    optionsList={props.moduleList}
                    optionName="moduleName"
                    optionValue="moduleId"
                    onSelectChange ={props.inputHandleChange}/>
                    <CustomInput
                    name='isAllowedDuringLock'
                    type='switch'
                    switchLabel={localConstant.security.ALLOW_ACCESS_DURING_LOCK_OUT}
                    colSize="s4 mt-4 left-align"
                    isSwitchLabel={true}
                    className="lever"
                    switchName="isAllowedDuringLock"
                    onChangeToggle={props.inputHandleChange}
                    checkedStatus={isTrue(props.selectedRole.isAllowedDuringLock)}
                    switchKey={isTrue(props.selectedRole.isAllowedDuringLock)}
                />
               
            </div>
        </div>
);

class CreateRole extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isPanelOpen: true,
            errorList:[],
            selectedUserRoles:[]           
        };
        this.updatedData = {};
        this.hasRoleLoaded = false;
        this.hasRoleDataLoaded = true;
        this.SelectedModuleActivity = [];
        this.selectedModuleId = '';
        this.hasDataChanged = false;
        this.selectedModuleData=[];
        this.selectedRoleData=[];
        this.errorListButton =[
            {
              name: localConstant.commonConstants.OK,
              action: this.closeErrorList,
              btnID: "closeErrorList",
              btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
              showbtn: true
            }
          ];
    }
    closeErrorList =(e) =>{
        e.preventDefault();
        this.setState({
          errorList:[]
        });
      }
    setIntialState() {
        this.state = {
            filteredModuleActivityDataUpdate: []
        };         
    }
    
    componentDidMount() {
        /**
         * Updating the current Module and page
         * Passing roleId to FetchRoleData() to fetch roles and roleactivities based on roleId
         */
        const result=this.props.location.search && parseQueryParam(this.props.location.search);     //D771
        this.props.actions.UpdateCurrentModule(localConstant.moduleName.SECURITY);
        if(this.props.location.pathname === AppMainRoutes.updateRole){  
            if(result){
            this.props.actions.UpdateCurrentPage(localConstant.security.updateRole_CurrentPage); 
            result.roleName = decodeURIComponent(result.roleName);  //%20 Value Binding Issue Fixes for Quick Fix  
            result.description = decodeURIComponent(result.description); //%20 Value Binding Issue Fixes for Quick Fix
            result.isAllowedDuringLock=decodeURIComponent(result.isAllowedDuringLock);
            //this.props.history.push('/UpdateRole');             
            this.props.actions.selectedRowData(result);
        }
        }
        if(this.props.location.pathname === AppMainRoutes.createRole){
            this.props.actions.UpdateCurrentPage(localConstant.security.createRole_CurrentPage); 
        }
        //In update case get the role based on roleId which is in querystring
        this.props.actions.FetchRoleData(result.roleId);      
    }
    componentWillUnmount() {
        this.props.actions.UserUnSavedDatas(true);
        
    }    

    filterModuleActivity() {
        const { selectedRole } = this.props;        
        const filteredModuleActivityData = [];

        let filterRoleActivityData = [];  
        const filterRoleActivities = []; 
        if (this.props.location.pathname === '/UpdateRole') {
            this.props.roleActivityData.forEach(data => {
                if (data.role.roleId === parseInt(selectedRole.roleId)) {
                    filterRoleActivityData = data.modules;
                }
            });
            filterRoleActivityData.forEach(data => {
                data.activities.forEach(activity => {
                    let hasAlready = false;
                    if (this.selectedModuleData.length > 0) {
                        this.selectedModuleData.forEach(selectedModule => {
                            if (data.module.moduleId === selectedModule.moduleId && activity.activityId === selectedModule.activityId && selectedModule.recordStatus === 'D')
                                hasAlready = true;
                        });
                    }
                    if (!hasAlready)
                        filterRoleActivities.push(data.module.moduleId + ',' + activity.activityId);
                });
            });
            if (this.selectedModuleData.length > 0) {
                this.selectedModuleData.forEach(data => {
                    if (data.recordStatus === 'N')
                        filterRoleActivities.push(data.moduleId + ',' + data.activityId);
                });
            }
        }
        else {
            //const selectedData = this.gridChild && this.gridChild.getSelectedRows();
            if (this.selectedModuleData.length > 0) {
                this.selectedModuleData.forEach(data => {
                    if (data.recordStatus !== 'D')
                        return;
                    this.props.roleActivityData.forEach(roleActivitydata => {
                        roleActivitydata.modules.forEach(roleData => {
                            if (roleData.module.moduleId === data.moduleId) {
                                roleData.activities.forEach(roleActivity => {
                                    if (roleActivity.activityId === data.activityId) {
                                        // const filterModuleActivityData = roleData;
                                        // filterModuleActivityData.activities = filterModuleActivityData.activities.filter(x => x.activityId === data.activityId);
                                        // filterRoleActivityData.push(filterModuleActivityData);
                                        filterRoleActivities.push(roleData.module.moduleId + ',' + roleActivity.activityId);
                                    }
                                });
                            }
                        });
                    });
                });
                this.selectedModuleData.forEach(data => {
                    if (data.recordStatus === 'N')
                        filterRoleActivities.push(data.moduleId + ',' + data.activityId);
                });
            }
        }        

        this.props.moduleActivityData.map(data => {
            data.activities.map(activity => {                               
                if(arrayUtil.contains(filterRoleActivities, (data.module.moduleId + ',' + activity.activityId)))
                {                  
                    activity.moduleAcivitySelected = true;
                }
                else
                {
                    activity.moduleAcivitySelected = false;
                }
                
                activity.moduleActivityID = data.module.moduleId + activity.activityId;
                activity.moduleName = data.module.moduleName;
                activity.moduleId =  data.module.moduleId;
                filteredModuleActivityData.push(activity);
                this.hasRoleLoaded = true;
            });
        });
                
        if(this.hasRoleLoaded === true && this.hasRoleDataLoaded === true) {            
            delete this.state.filteredModuleActivityDataUpdate;
            this.setState({
                filteredModuleActivityDataUpdate: filteredModuleActivityData
            });
            this.hasRoleDataLoaded = false;
            if (filteredModuleActivityData) {
                const modulename="moduleName";
                const resultData = sortModuleCustomerwiseList(custmoduleList,filteredModuleActivityData,modulename);
                const ActivityData = arrayUtil.boolsort(resultData,"moduleAcivitySelected","desc");
                this.setState((state) => {
                    return {                                              
                        selectedUserRoles : ActivityData 
                    };
                });    
            }
        }
        
    }

    saveFilterModuleActivity(ddModuleId) { 
        const selectedData = this.gridChild.getSelectedRows();
            if(selectedData !== undefined) {                
                if(this.selectedModuleId == '') {
                    this.SelectedModuleActivity = selectedData;
                    this.selectedModuleId = ddModuleId;
                } else {
                    selectedData.map(activity => {                    
                        let hasValue = false;
                        this.SelectedModuleActivity.map(moduleActivity => {
                            if(this.selectedModuleId == moduleActivity.moduleId && activity.moduleId === moduleActivity.moduleId && activity.activityId === moduleActivity.activityId) {
                                hasValue = true;
                            }
                        });
                        if (this.selectedModuleId == activity.moduleId && hasValue === false) {
                            let hasAlredy = false;
                            if (Array.isArray(this.props.roleActivityData) && this.props.roleActivityData.length > 0 && this.props.selectedRole) {
                                this.props.roleActivityData.forEach(roleData => {
                                    if (roleData.role.roleId === parseInt(this.props.selectedRole.roleId)) {
                                        roleData.modules.forEach(selectedRoleModules => {
                                            selectedRoleModules.activities.map(deleteActivity => {
                                                if (selectedRoleModules.module.moduleId === activity.moduleId && deleteActivity.activityId === activity.activityId) {
                                                    hasAlredy = true;
                                                }
                                            });
                                        });
                                    }
                                });
                            }
                            if (!hasAlredy)
                                activity.recordStatus = "N";
                            this.SelectedModuleActivity.push(activity);
                        }
                    });
                    // this.SelectedModuleActivity.map(activity => {                    
                    //     let hasValue = false;                         
                    //     selectedData.map(moduleActivity => {
                    //         if(this.selectedModuleId == activity.moduleId && activity.moduleId === moduleActivity.moduleId && activity.activityId === moduleActivity.activityId) {
                    //             hasValue = true;
                    //         }
                    //     });
                    //     if(this.selectedModuleId == activity.moduleId && hasValue === false) {
                    //         activity.recordStatus = "D";
                    //     }
                    // });
                }  
            }            
            const filterRoleActivities = [];            
            this.SelectedModuleActivity.map(activity => {
                if(activity.recordStatus !== "D") {
                    filterRoleActivities.push(activity.moduleId + ',' + activity.activityId);
                }
            });
            
            const filteredModuleActivityData = [];
            this.props.moduleActivityData.map(data => {
                data.activities.map(activity => {                    
                    if(ddModuleId == data.module.moduleId)
                    {
                        if(arrayUtil.contains(filterRoleActivities, (data.module.moduleId + ',' + activity.activityId))) {
                            activity.moduleAcivitySelected = true;
                        } else {
                            activity.moduleAcivitySelected = false;
                        }
                        activity.moduleActivityID = data.module.moduleId + activity.activityId;
                        activity.moduleName = data.module.moduleName;
                        activity.moduleId =  data.module.moduleId;
                        filteredModuleActivityData.push(activity);
                        this.selectedModuleId = ddModuleId;
                    } else if(ddModuleId == '') {
                        activity.moduleActivityID = data.module.moduleId + activity.activityId;
                        activity.moduleName = data.module.moduleName;
                        activity.moduleId =  data.module.moduleId;
                        filteredModuleActivityData.push(activity);
                        this.selectedModuleId = ddModuleId;
                    }
                });
            });
            const ActivityData = arrayUtil.boolsort(filteredModuleActivityData,"moduleAcivitySelected","desc");
            this.setState((state) => {
                return {
                    selectedUserRoles: ActivityData,
                };
            });
            this.gridChild.gridApi.setRowData(this.state.selectedUserRoles);
    }
    inputHandleChange = (e) => {
       //e.preventDefault();
        e.stopPropagation();    
        if (e.target.name === 'module') { 
            const valid = this.mandatoryFieldsValidationCheck();
            if (valid) {
                this.saveFilterModuleActivity(e.target.value);
            }
            else
            {
                e.target.value=this.updatedData.module;
            }
        }
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
        this.hasDataChanged = true;
        this.props.actions.UserUnSavedDatas(false);
    }

    //Validation Check
    mandatoryFieldsValidationCheck = () => {
        const selectedData=this.state.selectedUserRoles.filter(a=>a.moduleAcivitySelected===true);
        const contract=[],project=[],supplier=[],supplierPo=[],techSpec=[],assignment=[],security=[],visit=[],timeSheet=[];
        if (Array.isArray(selectedData) && selectedData.length > 0) {
            selectedData.forEach(data => {
                switch (data.moduleName) {
                    case localConstant.security.userRole.CONTRACT:
                        contract.push(data);
                        break;
                    case localConstant.security.userRole.PROJECT:
                        project.push(data);
                        break;
                    case localConstant.security.userRole.SUPPLIER:
                        supplier.push(data);
                        break;
                    case localConstant.security.userRole.SUPPLIER_PO:
                        supplierPo.push(data);
                        break;
                    case localConstant.security.userRole.TECHNICAL_SPECIALIST:
                        techSpec.push(data);
                        break;
                    case localConstant.security.userRole.ASSIGNMENT:
                        assignment.push(data);
                        break;
                    case localConstant.security.userRole.SECURITY:
                        security.push(data);
                        break;
                    case localConstant.security.userRole.VISIT:
                        visit.push(data);
                        break;
                    case localConstant.security.userRole.TIMESHEET:
                        timeSheet.push(data);
                        break;
                    default:
                        break;
                }
            });
        }
        let validData=true; 
        if ( contract.length > 0) {
            validData= this.SelectedData(contract, localConstant.security.userRole.CONTRACT);
            if(!validData)
            return validData;
        }
        if ( project.length > 0) {
            validData= this.SelectedData(project, localConstant.security.userRole.PROJECT);
            if(!validData)
            return validData;
        }
        if ( supplier.length > 0) {
            validData= this.SelectedData(supplier, localConstant.security.userRole.SUPPLIER);
            if(!validData)
            return validData;
        }
        if ( supplierPo.length > 0) {
            validData= this.SelectedData(supplierPo, localConstant.security.userRole.SUPPLIER_PO);
            if(!validData)
            return validData;
        }
        if ( techSpec.length > 0) {
            validData= this.SelectedData(techSpec, localConstant.security.userRole.RESOURCE);
            if(!validData)
            return validData;
        }
        if ( assignment.length > 0) {
            validData= this.SelectedData(assignment, localConstant.security.userRole.ASSIGNMENT);
            if(!validData)
            return validData;
        }
        if ( security.length > 0) {
            validData= this.SelectedData(security, localConstant.security.userRole.SECURITY);
            if(!validData)
            return validData;
        }
        if ( visit.length > 0) {
            validData= this.SelectedData(visit, localConstant.security.userRole.VISIT);
            if(!validData)
            return validData;
        }
        if ( timeSheet.length > 0) {
            validData= this.SelectedData(timeSheet, localConstant.security.userRole.TIMESHEET);
            if(!validData)
            return validData;
        } 

        return validData;
    }
    SelectedData = (selectedData, moduleName) => {
        const errors = [];
        
        const createDeleteActivities = [
            activitycode.NEW, activitycode.DELETE
        ];
        const editActivities = [
            activitycode.MODIFY, activitycode.LEVEL_1_MODIFY, activitycode.LEVEL_2_MODIFY, activitycode.EDIT_SENSITIVE_DOC, activitycode.EDIT_PAYRATE, activitycode.EDIT_TM
        ];

        const deleteActivities=[
            activitycode.DELETE
        ];
        //Global View for Contract
        const viewGlobalContractActivities=[
            contractActivityCode.View_Global
        ];

        //Global View for SupplierPO
        const viewGlobalSupplierPOActivities=[
            supplierPOActivityCode.View_Global
        ];

         //Global View for Assignment
        const viewGlobalAssignmentActivities=[ assignmentActivityCode.View_ALL_Assignments ];
       // const isEditActivitySelected = (selectedData.filter(x => editActivities.includes(x.activityCode)).length === 0);
       // const isCreateDeleteActivitySelected = (selectedData.filter(x => createDeleteActivities.includes(x.activityCode)).length > 0);

       // const isDeleteActivitySelected = (selectedData.filter(x => deleteActivities.includes(x.activityCode)).length > 0);
        //Global View for Contract
       // const isViewGlobalContractSelected= (selectedData.filter(x => viewGlobalContractActivities.includes(x.activityCode)).length > 0);
        //Global View for Assignment
       // const isViewGlobalAssignmentSelected = (selectedData.filter(x => viewGlobalAssignmentActivities.includes(x.activityCode)).length > 0);

        let isEditActivitySelected = true,isCreateDeleteActivitySelected,isDeleteActivitySelected,isViewGlobalContractSelected,isViewGlobalAssignmentSelected,isViewGlobalSupplierPOSelected;
        selectedData.forEach(iteratedValue => {
            if (editActivities.includes(iteratedValue.activityCode))
                isEditActivitySelected = false;
            if (createDeleteActivities.includes(iteratedValue.activityCode))
                isCreateDeleteActivitySelected = true;
            if (deleteActivities.includes(iteratedValue.activityCode))
                isDeleteActivitySelected = true;
            if (viewGlobalContractActivities.includes(iteratedValue.activityCode))
                isViewGlobalContractSelected = true;
            if (viewGlobalAssignmentActivities.includes(iteratedValue.activityCode))
                isViewGlobalAssignmentSelected = true;
            if (viewGlobalSupplierPOActivities.includes(iteratedValue.activityCode))
                isViewGlobalSupplierPOSelected = true;
        });

        if (isEditActivitySelected && isCreateDeleteActivitySelected && moduleName !== localConstant.security.userRole.RESOURCE) {
            errors.push(
                `${
                localConstant.security.USER_ROLE
                }- ${
                moduleName
                } - ${
                localConstant.validationMessage.CREATE_OR_DELETE_SYSTEM_ROLE
                }`
            );
        }

        if ((moduleName === localConstant.security.userRole.RESOURCE) && (((selectedData.filter(x => (x.activityCode === activitycode.NEW)).length > 0) && !(selectedData.filter(x => levelSpecificActivities.editActivitiesLevel2.includes(x.activityCode)).length > 0))||(isDeleteActivitySelected && !selectedData.filter(x => x.activityCode === activitycode.NEW).length > 0))) {
            errors.push(
                `${
                localConstant.security.USER_ROLE
                }- ${
                moduleName
                } - ${
                localConstant.validationMessage.TS_MODIFY_MIN_SYSTEM_ROLE
                }`
            );
        }

        // if(moduleName === localConstant.security.userRole.RESOURCE && isDeleteActivitySelected && !selectedData.filter(x => x.activityCode === activitycode.NEW).length > 0){
        //     errors.push(
        //         `${
        //         localConstant.security.USER_ROLE
        //         }- ${
        //         moduleName
        //         } - ${
        //         localConstant.validationMessage.DELETE_SYSTEM_ROLE
        //         }`
        //     );
        // }

        //Global View for Contract
        if(moduleName === localConstant.security.userRole.CONTRACT && isViewGlobalContractSelected && !selectedData.filter(x => x.activityCode === activitycode.VIEW).length > 0){
            errors.push(
                `${
                localConstant.security.USER_ROLE
                }- ${
                moduleName
                } - ${
                localConstant.validationMessage.VIEW_GLOBAL_SYSTEM_ROLE
                }`
            );
        }

        //Global View for Contract
        if(moduleName === localConstant.security.userRole.SUPPLIER_PO && isViewGlobalSupplierPOSelected && !selectedData.filter(x => x.activityCode === activitycode.VIEW).length > 0){
            errors.push(
                `${
                localConstant.security.USER_ROLE
                }- ${
                moduleName
                } - ${
                localConstant.validationMessage.VIEW_GLOBAL_SYSTEM_ROLE
                }`
            );
        }

        //Global View for Assignment
        if(moduleName === localConstant.security.userRole.ASSIGNMENT && isViewGlobalAssignmentSelected && !selectedData.filter(x => x.activityCode === activitycode.VIEW).length > 0){
            errors.push(
                 `${
                    localConstant.security.USER_ROLE
                   }- ${
                        moduleName
                        } - ${
                    localConstant.validationMessage.VIEW_ALL_ASSIGNMENT_SYSTEM_ROLE
                 }`
                );
        }

        if (errors.length > 0) {
            this.setState({
                errorList: errors
            });
            return false;
        }
        else {
            return true;
        }
    }    
    CheckTextInput = (data) => {
        if(!isEmpty(this.props.selectedRole.roleName)){
            return true;         
        }
        if(isEmpty(data.roleName)){
            IntertekToaster(localConstant.validationMessage.USER_ROLE_ROLE_NAME,"warningToast RoleNameVal"); 
            return false;
        }        
        else
        {
            return true;
        }
        
    }

    saveRole = async (e) => {
        e.preventDefault();
        const valid = this.mandatoryFieldsValidationCheck(); 
        if (valid) {
            const editedData = Object.assign({},this.updatedData);            
            if(this.CheckTextInput(editedData)){
                this.addNewRole(e);
            }        
        }
      }
    //Add New Role 
    addNewRole = (e) => {      
        e.preventDefault();
        this.selectedModuleId = (this.updatedData["module"] === undefined ? '' : this.updatedData["module"]);
        this.updatedData["applicationName"] = "Evolution2";
        let isNewRole = false;
        const saveData = {};
        saveData.role = {};
        if(this.props.selectedRole !== undefined && this.props.selectedRole.roleId !== undefined) {
        
            this.props.viewRole.map(data => {
                if(data.roleId === parseInt(this.props.selectedRole.roleId))
                {
                    data.applicationName = "Evolution2";
                    if(this.updatedData["roleName"] === undefined) {
                        data.roleName = this.props.selectedRole.roleName;
                    } else {
                        data.roleName = this.updatedData["roleName"];
                    }
                    if(this.updatedData["description"] === undefined) {
                        data.description = this.props.selectedRole.description;
                    } else {
                        data.description = this.updatedData["description"];
                    }
                    if(this.updatedData["isAllowedDuringLock"] === undefined){
                        data.isAllowedDuringLock = this.props.selectedRole.isAllowedDuringLock!=="null"?isTrue(this.props.selectedRole.isAllowedDuringLock):null;
                    }
                    else {
                        data.isAllowedDuringLock = isTrue(this.updatedData["isAllowedDuringLock"]);
                    }
                    data.recordStatus = "M";
                    saveData.role = data;
                }
            });
        } else {
            saveData.role.applicationName = "Evolution2";
            saveData.role.roleId = null;
            saveData.role.recordStatus = "N";
            saveData.role.roleName = this.updatedData["roleName"];
            saveData.role.description = this.updatedData["description"];
            saveData.role.isAllowedDuringLock = isTrue(this.updatedData["isAllowedDuringLock"]);
            isNewRole = true;
            
        }
        saveData.modules = [];   
        const filteredModuleData = this.selectedModuleData.filter(x => x.recordStatus != null);
        saveData.modules = Object.values(filteredModuleData.reduce((result, obj) => {
            // Create new group
            if (!result[obj.moduleId]) {
                result[obj.moduleId] = {
                    "module": {
                        moduleId: obj.moduleId,
                        applicationName: obj.applicationName,
                        moduleName: obj.moduleName,
                        description: obj.description,
                        updateCount: null,
                        recordStatus: obj.recordStatus,
                        lastModification: null,
                        modifiedBy: null,
                        eventId: null,
                        actionByUser: null,
                        userCompanyCode: null
                    },
                    "activities": [ {
                        activityId: obj.activityId,
                        applicationName: obj.applicationName,
                        activityCode: obj.activitycode,
                        activityName: obj.activityName,
                        description: obj.description,
                        moduleId: null,
                        updateCount: null,
                        recordStatus: obj.recordStatus,
                        lastModification: null,
                        modifiedBy: null,
                        eventId: null,
                        actionByUser: null,
                        userCompanyCode: null,
                    } ]
                };
                return result;
            }
            // Append to group
            result[obj.moduleId].activities.push(obj);
            return result;
        }, {})); 
        
        const roleData = [];
        roleData.push(saveData);
        if(isNewRole)
        {
            this.props.actions.AddRoleData(roleData)
                .then(response => {
                    if (response && response.code === "1") {
                        //After adding role page to navigated to userRoles so not required to fetch FetchRoleData
                        //this.props.actions.FetchRoleData();
                        setTimeout(() => { this.props.history.push(AppMainRoutes.userRoles); }, 1000);//need to show success alert
                    }
                    else{
                        this.selectedModuleData=[];
                    }
                });                
        } 
        else
        {
            this.props.actions.UpdateRoleData(roleData)
                .then(response => {
                    if (response && response.code === "1") {
                        //After adding role page to navigated to userRoles so not required to fetch FetchRoleData
                        //this.props.actions.FetchRoleData();
                        setTimeout(() => { this.props.history.push(AppMainRoutes.userRoles); }, 1000); //need to show success alert
                    }
                    else{
                        this.selectedModuleData=[];
                    }
                });
        }
        
    }

    CancelClickHandler = () => {
        if(this.hasDataChanged) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.CREATE_ROLE_CANCEL_MESSAGE,
                modalClassName: "warningToast",
                type: "confirm",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.cancelTechSpecChanges,
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: localConstant.commonConstants.NO,
                        onClickHandler: this.confirmationRejectHandler,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        } else {
            this.props.history.push(AppMainRoutes.userRoles);
        }
    }

    cancelTechSpecChanges = () => {
        this.props.actions.HideModal();
        this.props.history.push(AppMainRoutes.userRoles);
    }

    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }
     //Validation for Stamp Details
     roleValidation = (data) => {
        if (data.roleName === undefined || data.roleName === "") {
            IntertekToaster(localConstant.techSpec.common.REQUIRED_SELECT + ' Role Name', 'warningToast');
            return false;
        }       
        return true;
    }

    onRoleGridRowSelected(e) {
        if (e.node.selected !== e.data.moduleAcivitySelected) {
            this.hasDataChanged = true;
            this.props.actions.UserUnSavedDatas(false);
        }
        const checked = e.node.selected;
        e.data.moduleAcivitySelected = checked;

        let hasAlredy = false;
        if (Array.isArray(this.props.roleActivityData) && this.props.roleActivityData.length > 0 && this.props.selectedRole) {
            this.props.roleActivityData.forEach(roleData => {
                if (roleData.role.roleId === parseInt(this.props.selectedRole.roleId)) {
                    roleData.modules.forEach(selectedRoleModules => {
                        selectedRoleModules.activities.map(deleteActivity => {
                            if (selectedRoleModules.module.moduleId === e.data.moduleId && deleteActivity.activityId === e.data.activityId) {
                                hasAlredy = true;
                            }
                        });
                    });
                }
            });
        }

        if (checked) {
            if (!hasAlredy) {
                e.data.recordStatus = 'N';
            }
            let hasAlreadySelected = false;
            this.selectedModuleData.forEach((selectedRoleModules, index) => {
                if (selectedRoleModules.moduleId === e.data.moduleId && selectedRoleModules.activityId === e.data.activityId) {
                    if (selectedRoleModules.recordStatus === 'N' || selectedRoleModules.recordStatus===null)
                        hasAlreadySelected = true;
                    if (selectedRoleModules.recordStatus === 'D') {
                        this.selectedModuleData.splice(index, 1);
                        hasAlreadySelected = true;
                        index--;
                    }
                }
            });
            if (!hasAlreadySelected)
                this.selectedModuleData.push(e.data);
        }
        else {
            if (this.selectedModuleData.length > 0) {
               // const unSelectedModuleData = deepCopy(this.selectedModuleData);
                if (hasAlredy) {
                    // const selectedData = Object.assign({},e.data);
                    this.selectedModuleData.forEach((x, index) => {
                        if ((x.moduleId === e.data.moduleId && x.activityId === e.data.activityId && x.activityCode === e.data.activityCode && x.activityName === e.data.activityName && x.moduleName === e.data.moduleName&&x.recordStatus === 'D')) {
                            this.selectedModuleData.splice(index, 1);
                            index--;
                        }
                        else if ((x.moduleId === e.data.moduleId && x.activityId === e.data.activityId && x.activityCode === e.data.activityCode && x.activityName === e.data.activityName && x.moduleName === e.data.moduleName)) {
                            x.recordStatus = 'D';
                        }
                    });
                    //this.selectedModuleData.push(selectedData);
                }
                else {
                    this.selectedModuleData.forEach((x, index) => {
                        if ((x.moduleId === e.data.moduleId && x.activityId === e.data.activityId && x.activityCode === e.data.activityCode && x.activityName === e.data.activityName && x.moduleName === e.data.moduleName)) {
                            this.selectedModuleData.splice(index, 1);
                            index--;
                        }
                    });
                }
            }
        }
    }

    render() {
        const { selectedRole } = this.props;
        this.filterModuleActivity(); 
        return (
            <Fragment>
            {this.state.errorList.length > 0 ?
            <Modal title={ localConstant.commonConstants.CHECK_MANDATORY_FIELD }
            titleClassName="chargeTypeOption"
            modalContentClass="extranetModalContent"
            modalClass="ApprovelModal"
            modalId="errorListPopup"
            formId="errorListForm"
            buttons={this.errorListButton}
            isShowModal={true}>
                <ErrorList errors={this.state.errorList}/>
            </Modal> : null
        }
                <CustomModal />
                <div className="customCard">
                    <CreateRoleDiv
                        inputHandleChange={(e) => this.inputHandleChange(e)}
                        saveRole={this.saveRole}
                        clearRoleData={this.CancelClickHandler}
                        selectedRole={selectedRole}
                        history={this.props.history}/>
                    <ModuleTypeDiv 
                        moduleList={arrayUtil.sort(this.props.moduleList,'moduleName','asc')} 
                        inputHandleChange={(e) => this.inputHandleChange(e)}
                        selectedRole={selectedRole} />
                    <h6 className="customHeaderSize mt-4">Available Modules </h6>
                    <ReactGrid 
                       // gridRowData={this.state.filteredModuleActivityDataUpdate ? this.state.filteredModuleActivityDataUpdate : null}                        
                        gridRowData={this.state.selectedUserRoles}
                        gridColData={HeaderData} 
                        rowSelected={this.onRoleGridRowSelected.bind(this)}
                        onRef={ref => { this.gridChild = ref; }} 
                        paginationPrefixId={localConstant.paginationPrefixIds.userRoleGridId}
                        />
                </div>
            </Fragment>
        );
    }
}

export default CreateRole;
