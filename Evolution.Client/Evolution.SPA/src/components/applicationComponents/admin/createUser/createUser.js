import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './createUserHeader';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import {
    getlocalizeData,
    formInputChangeHandler,
    isEmpty,
    isEmptyOrUndefine,
    isValidEmailAddress
} from '../../../../utils/commonUtils';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import CustomModal from '../../../../common/baseComponents/customModal';
import { isNumber, isUndefined, isNullOrUndefined } from 'util';
import { getRoleInitialJson } from '../../../../utils/jsonUtil';
import { capitalize } from '../../../../utils/stringUtil';
import { addElementToArray,mapArrayObject,addElementToArrayParam,parseQueryParam } from '../../../../utils/commonUtils';
import cloneDeep from 'lodash/cloneDeep';
import arrayUtil from '../../../../utils/arrayUtil';
import { required } from '../../../../utils/validator';
import ErrorList from '../../../../common/baseComponents/errorList';
import Modal from '../../../../common/baseComponents/modal';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';

const localConstant = getlocalizeData();

export const userViewFieldName = {
    userName: 'userName',
    logonName: 'logonName',
    compCode: 'companyCode',
    compOfficeName: 'companyOfficeName',
    email: 'email',
    userType: 'userType',
    culture: 'culture',
    isActive: 'isActive',
    roleCompCode: 'roleCompanyCode'
};

export const CreateUserDiv = (props) => {  
    return (
        <form onSubmit={props.onSaveClicked} autoComplete="off">
            <div className="row mb-4 dividerRow">
                <span className="boldBorder"></span>
                <h6 className="col s9  mt-3"> <span className="bold">Users: </span> {props.isNewRequest ? localConstant.security.CREATE_USER : localConstant.security.UPDATE_USER}</h6>
                <div className="col s3 right-align mt-3" >
                    <button type="submit" disabled={props.isActionInProgress || !props.isFormStateChanged} className=" ml-1 waves-effect btn">{localConstant.security.SAVE} </button>
                    <button type="button" onClick={props.clearUserData} disabled={props.isActionInProgress} className="ml-2 waves-effect btn" >{localConstant.security.REFRESHCANCEL}</button>
                </div>
                <CustomInput hasLabel={true}
                    name={userViewFieldName.logonName}
                    colSize='s3 pr-0'
                    label={localConstant.security.LOGON_NAME}
                    type='text'
                    inputClass="customInputs"
                    maxLength={fieldLengthConstants.admin.users.LOGON_NAME_MAXLENGTH}
                    onValueChange={props.onUserInfoChanged}
                    defaultValue={isEmpty(props.user.logonName) ? "" : props.user.logonName}
                    //disabled={!props.isNewRequest} //User Module - Ability to change user name  CR Changes
                    labelClass="mandate"
                />
                <CustomInput hasLabel={true}
                    name={userViewFieldName.userName}
                    colSize='s3 pr-0'
                    label={localConstant.security.USER_NAME}
                    type='text'
                    inputClass="customInputs"
                    maxLength={fieldLengthConstants.admin.users.USER_NAME_MAXLENGTH}
                    onValueChange={props.onUserInfoChanged}
                    defaultValue={isEmpty(props.user.userName) ? "" : props.user.userName}
                    labelClass="mandate"
                />
                <CustomInput hasLabel={true}
                    name={userViewFieldName.compCode}
                    colSize='s3 pr-0'
                    label={localConstant.security.DEFAULTCOMAPNY}
                    type='select'
                    inputClass="customInputs"
                    onSelectChange={props.onUserInfoChanged}
                    optionsList={props.companyList}
                    optionName="companyName"
                    optionValue="companyCode"
                    defaultValue={isEmpty(props.user.companyCode) ? "" : props.user.companyCode}
                    labelClass="mandate"
                />
                <CustomInput hasLabel={true}
                    name={userViewFieldName.compOfficeName}
                    colSize='s3 pr-0'
                    label={localConstant.security.OFFICE}
                    type='select'
                    inputClass="customInputs"
                    onSelectChange={props.onUserInfoChanged}
                    optionName="officeName"
                    optionValue="officeName"
                    optionsList={props.selectedCompanyOffices}
                    defaultValue={isEmpty(props.user.companyOfficeName) ? "" : props.user.companyOfficeName}
                    labelClass="mandate"
                />
                <CustomInput hasLabel={true}
                    name={userViewFieldName.email}
                    colSize='s3 pr-0'
                    label={localConstant.security.EMAIL}
                    type='text'
                    inputClass="customInputs"
                    maxLength={fieldLengthConstants.admin.users.USER_EMAIL_MAXLENGTH}
                    onValueChange={props.onUserInfoChanged}
                    defaultValue={isEmpty(props.user.email) ? "" : props.user.email}
                    labelClass="mandate"
                />

                    {/* <CustomInput
                        hasLabel={true}                                              
                        label={localConstant.security.USER_TYPE}
                        type='multiSelect'
                        colSize='s3 pr-0'
                        className="browser-default"
                        optionsList={!isEmpty(props.userTypes) ? addElementToArray(props.userTypes) : [] }
                        name={userViewFieldName.userType}
                        multiSelectdValue={props.getMultiSelectedValue}
                        defaultValue={isEmpty(props.user.userType) ? "" : props.user.userType}/> */}

                {/* <CustomInput hasLabel={true}
                    name={userViewFieldName.userType}
                    divClassName='col pr-0'
                    colSize='s3'
                    label={localConstant.security.USER_TYPE}
                    type='select'
                    inputClass="customInputs"
                    optionsList={props.userTypes.filter(x => x.value !== 'TechnicalSpecialist' && x.name !== 'Customer')}
                    optionName="name"
                    optionValue="value"
                    defaultValue={isEmpty(props.user.userType) ? "" : props.user.userType}
                    onSelectChange={props.onUserInfoChanged}
                    labelClass="mandate"
                /> */}
                {/* <CustomInput
                    hasLabel={true}
                    name={userViewFieldName.culture}
                    colSize='s3'
                    label={localConstant.security.CULTURE}
                    type='text'
                    inputClass="customInputs"
                    maxLength={10}
                    onValueChange={props.onUserInfoChanged}
                    defaultValue={isEmpty(props.user.culture) ? "" : props.user.culture}
                /> */}
                <CustomInput
                    name={userViewFieldName.isActive}
                    type='switch'
                    switchLabel={localConstant.security.USER_ACTIVE}
                    colSize="s2 mt-4 left-align"
                    isSwitchLabel={true}
                    className="lever"
                    switchName="isActive"
                    onChangeToggle={props.handlerChange}
                    checkedStatus={props.user.isActive}
                    switchKey={props.user.isActive}
                />
            </div>
        </form>
    );
};

export const RoleCompanyDiv = (props) => { 
    return (
        <div className="row mb-0 dividerRow">
            <span className="boldBorder"></span>
            <div className="col s12 pr-0">
                <CustomInput
                    hasLabel={true}
                    required={true}
                    name={userViewFieldName.roleCompCode}
                    id="company"
                    divClassName='col pl-0'
                    colSize='s4'
                    label={localConstant.security.SELECTEDCOMPANYLIST}
                    type='select'
                    inputClass="customInputs"
                    optionsList={props.companyList}
                    optionName="companyName"
                    optionValue="companyCode"
                    defaultValue={isEmpty(props.selectedRoleCompanyCode) ? "" : props.selectedRoleCompanyCode}
                    onSelectChange={props.onUserRoleInfoChanged} />

                     <CustomInput
                        hasLabel={true}                                              
                        label={localConstant.security.WORK_FLOW_TYPE} //ITK Team asked this changes in Weekly Status Call on 13-02-2020
                        type='multiSelect'
                        labelClass=""  //as per D669 mail conversation
                        colSize='s3 pr-0'
                        className="browser-default"
                        optionsList={!isEmpty(props.userTypes) ? addElementToArrayParam(props.userTypes,'value','name' ) : [] }
                        name={userViewFieldName.userType}
                        multiSelectdValue={props.getMultiSelectedValue}
                        //defaultValue={isEmpty(props.user.userType) ? "" : props.user.userType}
                        defaultValue ={ !isEmpty(props.selectedUserType) ? props.selectedUserType.filter(s => s.userType !== ' ').map(e => e.userType).join(",") : '' }
                        />
                 <CustomInput
                    name={userViewFieldName.isActive}
                    type='switch'
                    switchLabel={localConstant.security.USER_ACTIVE}
                    colSize="s2 mt-4 left-align"
                    isSwitchLabel={true}
                    className="lever"
                    switchName="isActive"
                    onChangeToggle={props.onUserRoleIsActiveChange}
                    checkedStatus={props.selectedCompanyIsActive? true : false}
                    switchKey={props.selectedCompanyIsActive? true : false}
                />
            </div>
        </div>
    );
};

class CreateUser extends Component {

    constructor(props) {
        super(props);

        this.setIntialState();
        this.updatedData = this.props.userDetailData;

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

    setIntialState() {
        this.state = {
            freshRoles: [],
            compRoles: [],
            selectedRoleCompanyCode: '',
            roleBaseCompanyActive: false,
            selectedCompanyOffices: [],
            selectedUserType:[],
            isFormStateChanged: false,
            isActionInProgress: false,
            isNewRequest: true,
            errorList:[],
            modalTitle: ''
        };
    }

    setOperationMode() {        
        if(this.props.location.pathname === '/CreateUser') {
            this.isNewRequest = true;
        } else {
            this.isNewRequest = false;
        } 
    }

    closeErrorList =(e) =>{
        e.preventDefault();
        this.setState({
          errorList:[]
        });
    }

    componentDidMount() {
        if (this.props.location.pathname === AppMainRoutes.updateUser) {
            const result = this.props.location.search && parseQueryParam(this.props.location.search);
            result.logonName = decodeURIComponent(result.logonName);
            this.props.actions.SetUserDetailData(result.logonName).then(res => {
                if (res == true) {
                    this.updatedData = this.props.userDetailData;
                    this.fetchCompanyOffice().then(x => {
                        if (!this.isNewRequest) {
                            const userDetail = this.props.userDetailData;
                            this.filterCompanyOffice(userDetail.user.companyCode);
                        }
                    });

                    this.fetchRoles().then(x => {
                        if (!this.isNewRequest) {
                            const userDetail = this.props.userDetailData;
                            if (!isEmptyOrUndefine(userDetail.companyRoles) && userDetail.companyRoles.length > 0) {
                                let roles = [];
                                userDetail.companyRoles.map(x => {
                                    const mewCompRoles = cloneDeep(this.props.roles);
                                    mewCompRoles.map((role) => role.companyCode = x.companyCode);
                                    if (!isEmptyOrUndefine(x.roles)) {
                                        x.roles.map(x1 => {
                                            const filter = mewCompRoles.filter(f => f.roleName === x1.roleName);
                                            if (filter.length > 0) {
                                                filter[0].isSelected = true;
                                                filter[0].recordStatus = x1.recordStatus;
                                            }
                                        });
                                    }
                                    roles = roles.concat(mewCompRoles);
                                });
                                this.setState({ compRoles: roles });
                            }
                            this.setCompanyRoles(userDetail.user.companyCode);
                        }
                    });
                    this.filterUserType(this.props.userDetailData.user.companyCode);
                }
            });
        }
        else {
            this.fetchCompanyOffice();
            this.fetchRoles();
        }
    }
    componentWillUnmount() {
        //this.props.actions.ResetUserDetailState();
        this.props.actions.UserUnSavedDatas(true);
        this.props.actions.ResetRolesState();
        this.props.actions.ResetCompanyOfficeState();
    }

    fetchCompanyOffice() {
        const { companyOffices } = this.props;
        if (companyOffices.length <= 0) {
            return this.props.actions.FetchCompanyOffice();
        }
        else
            return Promise.resolve(true);
    }

    fetchRoles() {
        const { roles } = this.props;
        if (roles.length <= 0) {
            return this.props.actions.SearchRoles();
        }
        else
            return Promise.resolve(true);
    }

    filterCompanyOffice(companyCode) {      
        this.fetchCompanyOffice().then(x => {
            const { companyOffices } = this.props;
            this.setState({
                selectedCompanyOffices: companyOffices.filter((office) => office.companyCode == companyCode)
            });
        });
    }
filterUserType(companyCode){
     const companyUserTypes = this.props.userDetailData.companyUserTypes;
     if(!isEmpty(companyUserTypes)){
        const selectedUserType = companyUserTypes.filter((x)=>x.companyCode === companyCode);
        if(!isEmpty(selectedUserType)){
          this.setState({
                selectedUserType:selectedUserType
          });
        }else{
          this.setState({
                selectedUserType:[],
                roleBaseCompanyActive:false
          });
        }   
     }
      
};

    onUserInfoChanged = (e) => {       
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        this.updatedData.user[inputvalue.name] = inputvalue.value;
        //D669 Changes(as per mail conversation b/w francina & sumit)
        const datavalue=this;
        let userType = {};
        const userTypeData = [];
        const selectedUserType=[];
        if(inputvalue.name === userViewFieldName.compCode){
            this.updatedData.user["companyName"] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
            this.updatedData.user["companyOfficeName"] = "";
            //D669 Changes(as per mail conversation b/w francina & sumit) --- Start
           if(this.updatedData.companyUserTypes.length === 0 || this.updatedData.user.recordStatus === "N"){
                userType["eventId"] = null;
                userType["userLogonName"]= datavalue.props.userLogonName;
                userType["userType"] = "N/A";
                userType["companyCode"] = inputvalue.value;
                userType["updateCount"] = null;
                userType["lastModification"] = null;
                userType["recordStatus"] = "N";
                userType["isActive"] = true;
                userType["modifiedBy"] = null;
                userType["actionByUser"]=null;
                userType["isSelected"]=true;
                userTypeData.push(userType);
                userType = {};
                
                const companyUserType={
                    companyCode:inputvalue.value,
                    userTypes:userTypeData,
                            isActive:true
                    };
            selectedUserType.push(companyUserType);
            this.updatedData.companyUserTypes=selectedUserType;
           }
        //D669 Changes(as per mail conversation b/w francina & sumit) --- End
        }
        this.setState({ isFormStateChanged: true });
        this.props.actions.UserUnSavedDatas(false);
        if (inputvalue.name == userViewFieldName.compCode) {
            this.filterCompanyOffice(inputvalue.value);
            this.setCompanyRoles(inputvalue.value);
        }
    }

    onUserRoleInfoChanged = (e) => {    
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        if (inputvalue.name == userViewFieldName.roleCompCode) {
            this.setCompanyRoles(inputvalue.value);
        }
        //Added for ITK QA D-642
        this.setState({ isFormStateChanged: true });
        this.props.actions.UserUnSavedDatas(false);
    }

    /** User Role isActive change handler */
    onUserRoleIsActiveChange = (e) => {        
        e.stopPropagation();
        const datavalue=this;
        const inputvalue = formInputChangeHandler(e);
        //D669 Changes(as per mail conversation b/w francina & sumit) 
        const userTypeData = [];
        const selectedUserType=[];
        let userType = {};
        let userTypes = Object.assign([],datavalue.state.selectedUserType);
        if(isEmptyOrUndefine(datavalue.state.selectedRoleCompanyCode)){
            IntertekToaster("Select any one company","warningToast userRoleCompanyCheck");
            return false;
        }
        if(datavalue.state.selectedUserType.length === 0){
            //D669 Changes(as per mail conversation b/w francina & sumit) --- Start
                    userType["eventId"] = null;
                    userType["userLogonName"]= datavalue.props.userLogonName;
                    userType["userType"] = "N/A";
                    userType["companyCode"] = datavalue.state.selectedRoleCompanyCode;
                    userType["updateCount"] = null;
                    userType["lastModification"] = null;
                    userType["recordStatus"] = "N";
                    userType["isActive"] = false;
                    userType["modifiedBy"] = null;
                    userType["actionByUser"]=null;
                    userType["isSelected"]=true;
                    userTypeData.push(userType);
                    userType = {};

                const companyUserType={
                        companyCode:datavalue.state.selectedRoleCompanyCode,
                        userTypes:userTypeData,
                                isActive:false
                        };

                      selectedUserType.push(companyUserType);
                userTypes = selectedUserType;
                datavalue.updatedData.companyUserTypes.push(companyUserType);
            // IntertekToaster("Select at least one User Type to make Active.","warningToast userRoleActive");
            // return false;
            //D669 Changes(as per mail conversation b/w francina & sumit) --- End
        }
        userTypes.forEach(iteratedValue => {
            if(iteratedValue.companyCode === datavalue.state.selectedRoleCompanyCode){
                iteratedValue.isActive = inputvalue.value;
                if(iteratedValue.userTypes && Array.isArray(iteratedValue.userTypes) && iteratedValue.userTypes.length > 0){
                    iteratedValue.userTypes.forEach(element => {
                        element.isActive = inputvalue.value;
                        if(element.recordStatus !== 'N' && element.recordStatus !== 'D'){
                            element.recordStatus = 'M';
                        }
                        else if(element.recordStatus !== 'D'){
                            element.recordStatus = 'N';
                        }
                    });
                }
            }
        });
        datavalue.setState({
            selectedUserType: userTypes
        });
        if(userTypes.length > 0){
            const filteredUserTypeIndex = datavalue.updatedData.companyUserTypes.findIndex(x=> x.companyCode === datavalue.state.selectedRoleCompanyCode);
            if(filteredUserTypeIndex >=0){
                const userTypeListCopy = Object.assign([],datavalue.updatedData.companyUserTypes);
                userTypeListCopy[filteredUserTypeIndex] = userTypes[0];
                datavalue.updatedData.companyUserTypes = userTypeListCopy;
            }
        }
         //Added for ITK QA D-642
        this.setState({ isFormStateChanged: true });
        this.props.actions.UserUnSavedDatas(false);
    }

    setCompanyRoles(companyCode) {
        this.setState({ freshRoles: this.updateCompRoles(companyCode) });
        this.gridChild.gridApi&&this.gridChild.gridApi.setRowData(this.state.freshRoles);
        this.setState({ selectedRoleCompanyCode: companyCode });
        this.filterUserType(companyCode);
    }

    updateCompRoles(compCode) {
        let freshRoles = [];
        const selectedRoles =this.gridChild&&this.gridChild.props.gridRowData;
        if (selectedRoles.length > 0) {
            const compRoles = this.state.compRoles;
            //Deleting  older record
            if (compRoles.length > 0) {
                const existingCompRoles = compRoles.filter((compRole) => compRole.companyCode === this.state.selectedRoleCompanyCode);
                existingCompRoles.map(x => {
                    const index = compRoles.indexOf(x);
                    if (index !== -1)
                        compRoles.splice(index, 1);
                });
            }
            const newCompRoles = selectedRoles.map(x => {
                const oldRole = cloneDeep(x);
                oldRole.companyCode = this.state.selectedRoleCompanyCode;
                return oldRole;
            });
            compRoles.push(...newCompRoles);

            this.setState({ compRoles: compRoles });
        }

        if (!isEmptyOrUndefine(compCode) && this.state.compRoles.length > 0) {
            freshRoles = this.state.compRoles.filter((compRole) => compRole.companyCode === compCode);
        }

        if (freshRoles.length <= 0 && !isEmptyOrUndefine(compCode)) {
            freshRoles = this.props.roles;
            selectedRoles&&this.gridChild.props.gridRowData.map(x=> { x.recordStatus = null;}); //D651 8th issue
        }

        selectedRoles&&this.gridChild.props.gridRowData.map(x=> { x.isSelected=null;});
        // Code Added for Defect 299
        const ActivityUserRole = [];
        if (freshRoles) {
            const activityUserDefault = [
                ...freshRoles
            ];

            activityUserDefault.map((item, i) => { 
              if(item.isSelected){
                ActivityUserRole.push(item); 
              }
            });
            activityUserDefault.map((item ,i) =>{
                if(!item.isSelected){
                    ActivityUserRole.push(item); 
                  }
            });
        }
        //
        return ActivityUserRole;
    }

    prepareCompanyRoleObjet() {
        const userCompRoles = [];
        this.updateCompRoles('');
        if (this.state.compRoles.length > 0) {
            const rolesByCompCode = _.groupBy(this.state.compRoles, 'companyCode');
            _.mapValues(rolesByCompCode, (roles, compCode) => {
                const filteredRoles = roles.filter((role) => role.recordStatus === 'N' || role.recordStatus === 'D' || role.recordStatus === 'M');
                filteredRoles.map(role => {
                    if (role.recordStatus === 'M')
                        role.recordStatus = null;
                });
                if(filteredRoles.length > 0){//D701
                    const compRole = { "companyCode": compCode, "roles": filteredRoles };
                    userCompRoles.push(compRole);
                }
            });
        }
        return userCompRoles;
    }

    onSaveClicked = (e) => {       
        e.preventDefault();
        if (this.onUserInfoValidation(this.updatedData)) {  
            this.updatedData.companyRoles = this.prepareCompanyRoleObjet();         
            this.setState({ isActionInProgress: true });            
            this.props.actions.SaveUser(this.updatedData)
                .then(res => {
                    if (res == true) {
                        this.setIntialState();
                        setTimeout(x => {  
                            this.props.history.push('/Users');
                        }, 1000);
                    }
                    else {
                        this.setState({ isActionInProgress: false });
                    }
                });
        }
    }
    formInputChangeHandler = (e) => {
        const result = this.inputChangeHandler(e);
        if(result.name === userViewFieldName.isActive) {
            this.setState({ isFormStateChanged: true });
            this.props.actions.UserUnSavedDatas(false);
            this.updatedData.user[result.name] = result.value;
        } else {
            this.updatedData[result.name] = result.value;
        }
    }
    /*Input Change Handler*/
    inputChangeHandler = (e) => {
        const fieldValue = e.target[e.target.type === "checkbox" ? "checked" : "value"];
        const fieldName = e.target.name;
        const result = { value: fieldValue, name: fieldName };        
        return result;
    }
    cancelClickHandler = () => {
        if (this.state.isFormStateChanged == true) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.CREATE_USER_CANCEL_MESSAGE,
                modalClassName: "warningToast",
                type: "confirm",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.onModelOkClicked,
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: localConstant.commonConstants.NO,
                        onClickHandler: this.onModelCancelClicked,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
        else {
            this.onModelOkClicked();
        }
    }

    onModelOkClicked = () => {
        this.setIntialState();
        this.props.actions.HideModal();
        this.props.history.push('/Users');
    }

    onModelCancelClicked = () => {
        this.props.actions.HideModal();
    }

    onUserInfoValidation = (data) => {        
        const errors=[];
        if (isEmptyOrUndefine(data.user[userViewFieldName.userName])) {
            errors.push(
                `${ localConstant.security.CREATEUSER } - ${ localConstant.security.USER_NAME }`
              );              
        }

        if (isEmptyOrUndefine(data.user[userViewFieldName.logonName])) {
            errors.push(
                `${ localConstant.security.CREATEUSER } - ${ localConstant.security.LOGON_NAME }`
              );
        }

        if (isEmptyOrUndefine(data.user[userViewFieldName.compCode])) {
            errors.push(
                `${ localConstant.security.CREATEUSER } - ${ localConstant.security.DEFAULTCOMAPNY }`
              );
        }

        if (isEmptyOrUndefine(data.user[userViewFieldName.compOfficeName])) {
            errors.push(
                `${ localConstant.security.CREATEUSER } - ${ localConstant.security.OFFICE }`
              );
        }

        if (isEmptyOrUndefine(data.user[userViewFieldName.email])) {
            errors.push(
                `${ localConstant.security.CREATEUSER } - ${ localConstant.security.EMAIL }`
              );
        }
        //Commented for D669(as per mail conversation b/w francina & sumit) 
        // if (isEmptyOrUndefine(data.companyUserTypes)) {
        //     errors.push(
        //         `${ localConstant.security.CREATEUSER } - ${ localConstant.security.USER_TYPE }`
        //       );
        // }

        if(errors.length > 0) {            
            this.setState({
                modalTitle: localConstant.commonConstants.CHECK_MANDATORY_FIELD
            });
        }

        //Email Validation.
        if (errors.length <= 0 && !isValidEmailAddress(data.user[userViewFieldName.email])) {
            errors.push(
                `${ localConstant.security.EMAIL }: ${ localConstant.commonConstants.EMAIL_FORMAT_INVALID }`
              );
              this.setState({
                modalTitle: 'Error'
              });
        } 

        if(errors.length > 0){
            this.setState({
              errorList:errors
            });
            return false;
          } else {
            return true;
          }
    }

    onRoleGridRowSelected=(e)=>{
        const checked = e.node.selected;
        e.data.isSelected = checked;
        if (e.data.recordStatus == 'M' && !checked){
            e.data.recordStatus = 'D';
            //Added for ITK QA D-642
            this.props.actions.UserUnSavedDatas(false);
            this.setState({ isFormStateChanged: true });
        }
        else if (e.data.recordStatus == 'D' && checked)
            e.data.recordStatus = 'M';
        else if (e.data.recordStatus == null)
        {
            e.data.recordStatus = 'N';
            this.props.actions.UserUnSavedDatas(false);
            this.setState({ isFormStateChanged: true });
            
        }  
        else if (e.data.recordStatus === 'N' && !checked){
            e.data.recordStatus = null;
        }   
    }

    getMultiSelectedValue=(data)=>{ 
        const userTypeData = [];
        let matchedIdRecords = [];
        let addDeletedRecords =[];
        let finalUserType = [];
        const datavalue=this;
        if (!isEmpty(data)) {
            const selectedItem = mapArrayObject(data, 'value').join(',');
            const array = selectedItem.split(',');
            let userType = {};
            if (isEmpty(datavalue.state.selectedUserType)) {
                for (let i = 0; i < array.length; i++) {
                    userType["eventId"] = null;
                    userType["userLogonName"]= datavalue.props.userLogonName;
                    userType["userType"] = array[i];
                    userType["companyCode"] = datavalue.state.selectedRoleCompanyCode;
                    userType["updateCount"] = null;
                    userType["lastModification"] = null;
                    userType["recordStatus"] = "N";
                    userType["isActive"] = false;
                    userType["modifiedBy"] = null;
                    userType["actionByUser"]=null;
                    userType["isSelected"]=true;
                    userTypeData.push(userType);
                    userType = {};
                }
            } else {               
                matchedIdRecords = datavalue.state.selectedUserType[0].userTypes.filter(function (cd, i) {
                    return array.some(d => d === cd.userType);
                });
                const itemsTobeRemoved =  datavalue.state.selectedUserType[0].userTypes.filter(val => {
                    return array.indexOf(val.userType) === -1;
                });
                let recordsToAdd = [];
                recordsToAdd = matchedIdRecords.filter(val => {
                    return array.indexOf(val.userType) > -1;
                });

                recordsToAdd = array.filter(val => {
                    return !recordsToAdd.some(d => d.userType === val );
                });

                addDeletedRecords = matchedIdRecords.filter(val => {
                    return array.indexOf(val.userType) > -1 && val.recordStatus==='D';
                });
 
                if (!isEmpty(recordsToAdd)) {
                    for (let i = 0; i < recordsToAdd.length; i++) {
                        userType["eventId"] = null;
                        userType["userLogonName"]=datavalue.props.userLogonName;
                        userType["userType"] = recordsToAdd[i];
                        userType["companyCode"] = datavalue.state.selectedRoleCompanyCode;
                        userType["updateCount"] = null;
                        userType["lastModification"] = null;
                        userType["recordStatus"] = "N";
                        userType["isActive"] = datavalue.state.selectedUserType[0].isActive;
                        userType["modifiedBy"] = null;
                        userType["actionByUser"]=null;
                        userType["isSelected"]=true;
                        userTypeData.push(userType);
                        userType = {};
                    }
                }
 
                if (!isEmpty(itemsTobeRemoved)) {
                    for (let i = 0; i < itemsTobeRemoved.length; i++) {
                        if (itemsTobeRemoved[i].recordStatus !== "N") {
                            itemsTobeRemoved[i].recordStatus = 'D';
                            userTypeData.push(itemsTobeRemoved[i]);
                        }
                    }
                }

                if (!isEmpty(addDeletedRecords)) {
                    for (let i = 0; i < addDeletedRecords.length; i++) {
                        if (addDeletedRecords[i].recordStatus === "D" && matchedIdRecords.some(d=>d.userType===addDeletedRecords[i].userType && d.recordStatus==="D")) {
                            addDeletedRecords[i].recordStatus = null;
                        }else if(addDeletedRecords[i].recordStatus === "D" )
                        {
                            addDeletedRecords[i].recordStatus = "N";
                            userTypeData.push(addDeletedRecords[i]);
                        }  
                    }
                }
            }
        } else {
            for (let i = 0; i < datavalue.state.selectedUserType[0].userTypes.length; i++) {
                if (datavalue.state.selectedUserType[0].userTypes[i].recordStatus !== "N") {
                    datavalue.state.selectedUserType[0].userTypes[i].recordStatus = 'D';
                    userTypeData.push(datavalue.state.selectedUserType[0].userTypes[i]);
                }
            }
            let userType = {};
                    userType["eventId"] = null;
                    userType["userLogonName"]= datavalue.props.userLogonName;
                    userType["userType"] = "N/A";
                    userType["companyCode"] = datavalue.state.selectedUserType[0].companyCode;
                    userType["updateCount"] = null;
                    userType["lastModification"] = null;
                    userType["recordStatus"] = "N";
                    userType["isActive"] = datavalue.state.selectedUserType[0].isActive;
                    userType["modifiedBy"] = null;
                    userType["actionByUser"]=null;
                    userType["isSelected"]=true;
                    userTypeData.push(userType);
                    userType = {};
        } 
        finalUserType = [
            ...matchedIdRecords, ...userTypeData
        ];       
        datavalue.updatedData.companyUserTypes.filter(function(x){
                if(x.companyCode === datavalue.state.selectedRoleCompanyCode){
                   return(
                        x.userTypes = finalUserType
                   );
                }
            });
        if(isEmpty(datavalue.state.selectedUserType)){
              const companyUserType={
                      companyCode:datavalue.state.selectedRoleCompanyCode,
                      userTypes:finalUserType,
                      isActive:false
              };
            datavalue.setState({
                selectedUserType:[ companyUserType ]
            }); 
           datavalue.updatedData.companyUserTypes.push(companyUserType);
        } else{
            datavalue.state.selectedUserType[0].userTypes = finalUserType;
            datavalue.setState({
                selectedUserType:datavalue.state.selectedUserType
            }); 
        }
        
        this.setState({ isFormStateChanged: true });
        this.props.actions.UserUnSavedDatas(false);
    } 

    render() {     
        const { companyList, userDetailData, userTypes } = this.props;
        const sortedCompanyList = arrayUtil.sort(companyList, 'companyName', 'asc');
        this.setOperationMode();
        
        return (
            <Fragment>
                <CustomModal />
                {this.state.errorList.length > 0 ?
                    <Modal title={ this.state.modalTitle }
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
                <div className="customCard">
                    <CreateUserDiv user={userDetailData.user}
                        companyList={sortedCompanyList}
                        //userTypes={userTypes.filter(x => x.value !== 'TechnicalSpecialist' && x.name !== 'Customer')}                        
                        //getMultiSelectedValue={this.getMultiSelectedValue}
                        onUserInfoChanged={(e) => this.onUserInfoChanged(e)}
                        onSaveClicked={this.onSaveClicked}
                        clearUserData={this.cancelClickHandler}
                        selectedCompanyOffices={this.state.selectedCompanyOffices}
                        isFormStateChanged={this.state.isFormStateChanged}
                        isActionInProgress={this.state.isActionInProgress}
                        handlerChange={(e) => this.formInputChangeHandler(e)}
                        isNewRequest={this.isNewRequest} />

                    <RoleCompanyDiv
                        user={userDetailData.user}
                        userDetailData={userDetailData}
                        companyList={sortedCompanyList}
                        selectedRoleCompanyCode={this.state.selectedRoleCompanyCode}
                        onUserRoleInfoChanged={(e) => this.onUserRoleInfoChanged(e)} 
                        userTypes={userTypes.filter(x => x.value !== 'TechnicalSpecialist' && x.name !== 'Customer')}                        
                        getMultiSelectedValue={this.getMultiSelectedValue}
                        selectedUserType={!isEmpty(this.state.selectedUserType) ? addElementToArrayParam(this.state.selectedUserType[0].userTypes.filter(x => x.recordStatus !== "D"),'userType','userType') : []}
                        onUserRoleIsActiveChange = {(e) => this.onUserRoleIsActiveChange(e)}
                        selectedCompanyIsActive = {(!isEmpty(this.state.selectedUserType)&&this.state.selectedUserType.length > 0)? this.state.selectedUserType[0].isActive : false}
                        />

                    <h6 className="customHeaderSize mt-4">Available Roles</h6>
                    <ReactGrid
                        gridRowData={this.state.freshRoles}
                        gridColData={HeaderData}
                        rowSelected={(e)=>this.onRoleGridRowSelected(e)}
                        onRef={ref => { this.gridChild = ref; }} 
                        paginationPrefixId={localConstant.paginationPrefixIds.userGridId}
                        />
                </div>
            </Fragment>
        );
    }
}

export default CreateUser;
