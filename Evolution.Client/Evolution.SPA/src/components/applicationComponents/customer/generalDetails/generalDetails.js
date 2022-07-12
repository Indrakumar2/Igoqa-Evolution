import React, { Component, Fragment } from 'react';
import MaterializeComponent from 'materialize-css';
import { apiConfig } from '../../../../apiConfig/apiConfig';
import { HeaderData, GeneralContactHeader } from './headerData';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import SecondReactGrid from '../../../../common/baseComponents/reactAgGridTwo';

import { modalTitleConstant, modalMessageConstant } from '../../../../constants/modalConstants';
import CustomModal from '../../../../common/baseComponents/customModal';
import { isEmpty,isValidEmailAddress } from '../../../../utils/commonUtils';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { getlocalizeData,bindAction } from '../../../../utils/commonUtils';
import { required,stringWithOnlySpaces } from '../../../../utils/validator';
import Modal from '../../../../common/baseComponents/modal';
import ExtranetUserAccount from '../extranet/extranetUserAccount'; 
import Draggable from 'react-draggable'; // The default
import dateUtil from '../../../../utils/dateUtil';
import arrayUtil from '../../../../utils/arrayUtil';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
const localConstant = getlocalizeData();
class GeneralDetails extends Component {
    constructor(props) {
        super(props);
        this.updatedCustomerAddressData = {};
        this.updatedCustomerContactData = {};
        this.hasContacts = false;
        this.hasActiveContacts = false;
        this.IsVisibleUnAuthorisedProjects=false;
        this.state = {
            isOpen: false ,
            portalAddUserModal:false,
            authorisedProjectData : [] ,
            customerProjectData: [],
            showAddressModal:false,
            showContactModal:false,
                   
        }; 
        this.addressModalRef= React.createRef();
        this.contactModalRef=React.createRef();
        this.addressRef=React.createRef();
        this.confirmationModalData = {
            title: "",
            message: "",
            type: "",
            modalClassName: "",
            buttons: []
        };
        this.extranetAddUserButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelExtranetUserModal,
                btnID: "cancelExtranetUser",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.createExtranetUserModal,
                btnID: "createExtranetUser",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ]; 
        this.functionRef={};
        //bindAction(HeaderData.GeneralContactHeader,"PortalUserColumn",this.addPortalUserRowHandler); 
        //bindAction(this.headerData, "IsPortalUser", (e,data) =>  this.addPortalUserRowHandler(e,data) );
       
    }
 
    IsVisibleUnAuthorisedProject = () =>
    {     
        const selectedCompanyObj = Object.assign([],this.props.companyList).find(x => x.companyCode === this.props.selectedCompany );
        if(selectedCompanyObj)
        {  
            this.IsVisibleUnAuthorisedProjects = !!Object.assign([],this.props.customerAddressData).find(x=>x.Country=== selectedCompanyObj.operatingCountry && x.RecordStatus!= "D");  
        }
    }

    componentDidMount() {
        const select = document.querySelectorAll('select');
        const selectInstances = MaterializeComponent.FormSelect.init(select); 
        const custModal = document.querySelectorAll('.modal');
        const custModalInstances = MaterializeComponent.Modal.init(custModal, { "dismissible": false });  
    }

    handleChange = (e) => {
       const data = this.props.editedAddressReference;
       this.updatedCustomerAddressData[e.target.name] = e.target.value;
        if (e.target.name === "Country") {
            document.getElementById("StateId").value = "";
            document.getElementById("CityId").value = "";
            this.updatedCustomerAddressData["County"] = "";
            this.updatedCustomerAddressData["City"] = "";
            this.props.actions.ClearMasterStateCityData();
            this.updatedCustomerAddressData[e.target.id] = parseInt(e.target.value);
            this.updatedCustomerAddressData[e.target.name] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent !== localConstant.commonConstants.SELECT ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent : "";
            const selectedCountry = e.target.value;           
            this.props.actions.FetchStateId(selectedCountry);
        }
        if (e.target.name  === "County" ) {
            this.props.actions.ClearMasterCityData();
            this.updatedCustomerAddressData["City"] = "";
            const selectedState = e.target.value;
            this.updatedCustomerAddressData[e.target.id] = parseInt(e.target.value);
            this.updatedCustomerAddressData[e.target.name] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent !== localConstant.commonConstants.SELECT ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent : "";
            this.props.actions.FetchCityId(selectedState);
            const updatedCustomerAddress = Object.assign({},this.props.editedAddressReference,this.updatedCustomerAddressData);
            this.props.actions.EditStateAddressReference(updatedCustomerAddress);
        }

        if(e.target.name === 'City')
        {
            this.updatedCustomerAddressData[e.target.id] = parseInt(e.target.value);
            this.updatedCustomerAddressData[e.target.name] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent !== localConstant.commonConstants.SELECT ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent : "";
            //this.updatedCustomerAddressData["City"]= e.target.value;
        }

        if(e.target.name === "Address")
        {
            this.props.actions.DisplayFullAddress(e.target.value);
            this.updatedCustomerAddressData["Address"] =e.target.value;
        }

        //this.updatedCustomerAddressData[e.target.name] = e.target.value;
    }
    handleContactChange = (e) => {
        this.updatedCustomerContactData[e.target.name] = e.target.value;
    }
    handleModalChange = (e) => {
        this.updatedCustomerContactData[e.target.name] = e.target.value; 
    }

    /** Customer Contact Validation */
    contactValidation = (data) => {
        if(isEmpty(data) || required(data.CustomerAddressId)){
            IntertekToaster(localConstant.customer.PLEASE_SELECT_ADDRESS,"warningToast customerAddressVal");
            return false;
        }
        if(required(data.ContactPersonName)){
            IntertekToaster(localConstant.validationMessage.CONTACT_NAME_VALIDATION,"warningToast contractPersonNameVal");
            return false;
        }
        if(required(data.Landline)){
            IntertekToaster(localConstant.validationMessage.TELEPHONE_NO_VALIDATION,"warningToast landlineVal");
            return false;
        }
        if(!isEmpty(data.Email) && !isValidEmailAddress(data.Email)){
            IntertekToaster(localConstant.validationMessage.EMAIL_VALIDATION,"warningToast emailVal");
            return false;
        }
        return true;
    };

    addPortalUserRowHandler = (e, data) => {
        e && e.persist();
        this.setState({ authorisedProjectData :[] });
        if (e.target.checked) {
            const authorizedProjects=[];
            const UserInfo=data.UserInfo;
            this.PortalUserRowEvent = e;
            const projectSearchFilter = {
                contractCustomerCode: this.props.selectedCustomerCode,
                // contractHoldingCompanyCode: this.props.selectedCompany // Defect 726,686
            };
            const selectedCompany=this.props.selectedCompany;
            //let extranetUsers = this.props.extranetUsers;
         
            if (this.props.extranetUser) {
                data = this.mapContactToExtranetUserData(data);
            }

            if (isEmpty(data.UserName) || isEmpty(data.Email)) {
                this.PortalUserRowEvent.target.checked =false; // un-check slider button
                IntertekToaster(`${
                    localConstant.customer.PORTAL_ACCESS
                    } - ${
                    localConstant.customer.REQUIRED_PROTAL_ACCESS_MANDATORY_FIELDS
                    }`, 'warningToast');
                    
            } else {
                this.props.actions.AddExtranetUser(data);
                if(!isEmpty(UserInfo))
                {
                  this.setState({ portalAddUserModal:false } );
                  if(UserInfo.IsActive === false)
                  {
                    data.IsActive=true;
                    data.RecordStatus=UserInfo.UserId >0 ?'M':'N';  //Added for Sanity Defect -125
                    data.UserId=UserInfo.UserId;
                    data.CompanyCode=selectedCompany;
                    data.ExtranetAccessLevel=UserInfo.ExtranetAccessLevel;
                    if(UserInfo.IsShowNewVisit)
                    data.IsShowNewVisit=UserInfo.IsShowNewVisit;
                    // if(data.UserId)
                    // {
                    // extranetUsers = extranetUsers && extranetUsers.filter(x=>x.UserId===UserInfo.UserId);
                    // if(extranetUsers)
                    // {
                    //   data.UpdateCount = extranetUsers.length > 0 && extranetUsers[0].UpdateCount;
                    // }
                    // }
                    data.UpdateCount=UserInfo.UpdateCount;//Added for Sanity Defect 3
                    data.CreatedDate=UserInfo.CreatedDate;
                    data.CustomerUserProjectNumbers=UserInfo.CustomerUserProjectNumbers;
                    data.Comments=UserInfo.Comments;
                    data.Password=UserInfo.Password;
                    data.confirmPassword=UserInfo.confirmPassword;
                    data.IsAccountLocked=UserInfo.IsAccountLocked;
                    data.CustomerUserProjectNumbers.forEach(x=>{
                        const eachProjectData=[];
                        eachProjectData.projectNumber=x.ProjectNumber;
                        eachProjectData.Id=x.Id;
                        eachProjectData.UserId=x.UserId;
                        authorizedProjects.push(eachProjectData);
                    });
                  }
                    this.props.actions.AssignExtranetUserToContact(data,authorizedProjects);
                }else
                {
                    this.setState({ portalAddUserModal: true });
                }
                
                this.props.actions.FetchProjectList(projectSearchFilter).then(res => {
                    if (res) { 
                        this.setState({ customerProjectData: res });
                    }
                });
            } 
        }
        else {
            this.props.actions.DeactivateExtranetUserAccount(data);
        }
    }

    //Cancel Extranet User Modal Popup
    cancelExtranetUserModal=()=>{
        if( this.PortalUserRowEvent){
            this.PortalUserRowEvent.target.checked = !this.PortalUserRowEvent.target.checked;
        } 
        this.setState({ portalAddUserModal:false } );
    }
    
    //Submit New Extranet User Modal Popup
    createExtranetUserModal = (e) => {
        e.preventDefault();
        if (this.extranetUserMandatoryFieldValidation()) {
            this.props.actions.AssignExtranetUserToContact(this.props.extranetUser,this.state.authorisedProjectData);
            this.setState({ portalAddUserModal:false } );
        } 
    }

    extranetUserMandatoryFieldValidation = () => {
        const passwordRegEx = /^[A-Za-z0-9](?=.*[^a-zA-Z0-9])(?!.*\s).{6,127}$/; //Added for Defect 929 (issue 1)
        if (this.props.extranetUser) {
            // Contact Name
            if (isEmpty(this.props.extranetUser.UserName)) {
                IntertekToaster(`${
                    localConstant.customer.PORTAL_ACCESS
                    } - ${
                    localConstant.customer.REQUIRED_CUSTOMER_PORTAL_CUSTOMER_NAME
                    }`, 'warningToast');
                return false;
            }

            // User Name
            if (isEmpty(this.props.extranetUser.LogonName)) {

                IntertekToaster(`${
                    localConstant.customer.PORTAL_ACCESS
                    } - ${
                    localConstant.customer.USER_NAME
                    }`, 'warningToast');
                return false;
            }
            // Email
            if (isEmpty(this.props.extranetUser.Email)) {

                IntertekToaster(`${
                    localConstant.customer.PORTAL_ACCESS
                    } - ${
                    localConstant.customer.REQUIRED_CUSTOMER_PORTAL_EMAIL
                    }`, 'warningToast');
                return false;
            }

            // Valid Email Address
            if (!isEmpty(this.props.extranetUser.Email) && stringWithOnlySpaces(this.props.extranetUser.Email)) {

                IntertekToaster(`${
                    localConstant.customer.PORTAL_ACCESS
                    } - ${
                    localConstant.validationMessage.EMAIL_VALIDATION
                    }`, 'warningToast');
                return false;
            }
           // Password
           if (isEmpty(this.props.extranetUser.Password)) {
            IntertekToaster(`${
                localConstant.customer.PORTAL_ACCESS
                } - ${
                localConstant.customer.PASSWORD
                }`, 'warningToast');
            return false;
            }
            
         //chandra
         const Passwordtrim = this.props.extranetUser.Password.trim();
            if (this.props.extranetUser.Password.length < 7 || isEmpty(Passwordtrim)) {
                IntertekToaster(`${
                    localConstant.customer.PORTAL_ACCESS
                    } - ${
                        localConstant.techSpec.contactInformation.PASSWORD_VALIDATION
                    }`, 'warningToast');
                return false;
                }
            //Added for Defect 929 (issue 1) -Starts
            if (!passwordRegEx.test(this.props.extranetUser.Password)) {
                IntertekToaster(localConstant.techSpec.contactInformation.PASSWORD_VALIDATION, 'warningToast');
                return false;
            }
            //Added for Defect 929 (issue 1) -End
            if (isEmpty(this.props.extranetUser.confirmPassword)) {
                IntertekToaster(`${
                    localConstant.customer.PORTAL_ACCESS
                    } - ${
                    localConstant.customer.CONFIRM_PASSWORD
                    }`, 'warningToast');
                return false;
            }
            if(!isEmpty(this.props.extranetUser.Password) && !isEmpty(this.props.extranetUser.confirmPassword) && this.props.extranetUser.Password!=this.props.extranetUser.confirmPassword  )
            {
                IntertekToaster(`${
                    localConstant.customer.PORTAL_ACCESS
                    } - ${
                    localConstant.customer.MISMATCH_CONFIRM_PASSWORD
                    }`, 'warningToast');
                return false;
                
            }
            
            // // Password Security Question
            // if (isEmpty(this.props.extranetUser.SecurityQuestion1)) {

            //     IntertekToaster(`${
            //         localConstant.customer.PORTAL_ACCOUNTS
            //         } - ${
            //         localConstant.customer.PASSWORD_QUESTION
            //         }`, 'warningToast');
            //     return false;
            // }
            // // Security Answer
            // if (isEmpty(this.props.extranetUser.SecurityQuestion1Answer)) {

            //     IntertekToaster(`${
            //         localConstant.customer.PORTAL_ACCOUNTS
            //         } - ${
            //         localConstant.customer.PASSWORD_ANSWER
            //         }`, 'warningToast');
            //     return false;
            // }
            // //  Extranet Access Level
            // if (isEmpty(this.props.extranetUser.ExtranetAccessLevel)) {

            //     IntertekToaster(`${
            //         localConstant.customer.PORTAL_ACCOUNTS
            //         } - ${
            //         localConstant.customer.PORTAL_ACCESS
            //         }`, 'warningToast');
            //     return false;
            // } 
            // if(this.state.authorisedProjectData.length<=0)
            // {
            //     IntertekToaster(`${
            //         localConstant.customer.PORTAL_ACCOUNTS 
            //         } - ${
            //         localConstant.customer.SELECT_PORTAL_USER_PROJECT_ACCESS
            //         }`, 'warningToast');
            //     return false;
            // }
            return true;
        }
    }
 
    addExtranetUser=(data)=> {   
        data.IsActive=true;
        data.RecordStatus="N";
        data.CreatedDate=dateUtil.postDateFormat(Date.now(),'-') ;
        data.CompanyCode=this.props.selectedCompany;
        this.props.actions.AddExtranetUser(data);
    }

    swapProjectInfos = (selectedRows, srcProjectData, destProjectData) => {
        if (selectedRows) {
            const index = srcProjectData.findIndex(val => (val.projectNumber === selectedRows.projectNumber));
            if (index >= 0) {
                destProjectData.push(...srcProjectData.splice(index, 1));
            }
        }
    }
 
    onCustomerProjectRowSelected = (e) => {   
        const selectedRows = e.api.getSelectedRows(); 
       const authProjectData = [ ...this.state.authorisedProjectData ]; 
        const custProjectData = [ ...this.state.customerProjectData ]; 
        this.swapProjectInfos(selectedRows[0],custProjectData,authProjectData) ; 
        this.setState({
            authorisedProjectData: authProjectData,
            customerProjectData: custProjectData
        }); 
    } 

    onAuthorisedProjectRowSelected = (e) => {   
        const selectedRows = e.api.getSelectedRows(); 
        const authProjectData = [ ...this.state.authorisedProjectData ]; 
        const custProjectData = [ ...this.state.customerProjectData ]; 
        this.swapProjectInfos(selectedRows[0],authProjectData,custProjectData) ; 
        this.setState({
            authorisedProjectData: authProjectData,
            customerProjectData: custProjectData
        });
    }

    assignAllProjectClick = (e) => {  
        const unAuthorisedProjects=this.state.authorisedProjectData.concat(this.props.customerProjectList.filter(x=>this.props.selectedCompany=== x.contractHoldingCompanyCode)); //D-1288
        const distinctUnAuthorisedProjects = arrayUtil.removeDuplicates(unAuthorisedProjects, "projectNumber"); //D-1345
        this.setState({ authorisedProjectData : distinctUnAuthorisedProjects });//D-1288
        // this.setState({ authorisedProjectData : this.props.customerProjectList });
        this.setState({ customerProjectData:[] });
    } 

    removeAllProjectClick = (e) => {   
        this.setState({ authorisedProjectData :[] });
        this.setState({ customerProjectData : this.props.customerProjectList });
    }

    contactSubmitHandler = (e) => {
        e.preventDefault();
        let isDuplicate = false;
        const selectedOption = document.getElementById("addressId").options;
        if (this.props.showButton === true) {
            let portalUserInfo=null;
            const updatedCustomerContact = Object.assign({},this.props.editedContactReference,this.updatedCustomerContactData);
            if(this.contactValidation(updatedCustomerContact)){
                if (this.updatedCustomerContactData.ContactPersonName) { 
                    this.props.customerAddressData.map(address => {
                        address.Contacts.map(contact => {
                            if (address.AddressId == document.getElementById("addressId").value && contact.ContactPersonName.trim() == this.updatedCustomerContactData.ContactPersonName.trim() && contact.recordStaus !== "D") {
                                return isDuplicate = true;
                            }
                            if(contact.ContactId === updatedCustomerContact.ContactId && contact.ContactPersonName.trim() != this.updatedCustomerContactData.ContactPersonName.trim())
                            {
                                portalUserInfo=updatedCustomerContact.UserInfo;
                            }
                        });
                    });
                }
                if (isDuplicate) {
                    IntertekToaster(localConstant.customer.CONTACT_NAME_ALREADY_EXIST, 'warningToast CustGenContDupChk');
                }
                else {
                    if (this.props.editedContactReference.RecordStatus !== "N") {
                        this.updatedCustomerContactData["RecordStatus"] = "M";
                        this.updatedCustomerContactData["modifiedBy"] = this.props.loggedInUser;
                    }
                    
                    this.updatedCustomerContactData["CustomerAddressId"] = document.getElementById("addressId").value;
                    this.updatedCustomerContactData["contactAddress"] = selectedOption[selectedOption.selectedIndex].text;
                    this.props.actions.UpdateContactReference(this.updatedCustomerContactData);
                    if(portalUserInfo)
                    {
                        const authProjects=[];
                        portalUserInfo= this.mapContactToExtranetPortalData(updatedCustomerContact, portalUserInfo);
                        portalUserInfo.UserName=this.updatedCustomerContactData.ContactPersonName;
                        portalUserInfo.CreatedDate=updatedCustomerContact.UserInfo.CreatedDate;
                        portalUserInfo.UserId=updatedCustomerContact.UserInfo.UserId;
                        portalUserInfo.CompanyCode=updatedCustomerContact.UserInfo.CompanyCode;
                        portalUserInfo.RecordStatus= (portalUserInfo.RecordStatus !== undefined && portalUserInfo.RecordStatus !=="N") ? "M" : "N";  //Sanity Defect 185 Fix 
                        portalUserInfo.Password=updatedCustomerContact.UserInfo.Password;
                        portalUserInfo.confirmPassword=updatedCustomerContact.UserInfo.confirmPassword;
                        portalUserInfo.IsShowNewVisit=updatedCustomerContact.UserInfo.IsShowNewVisit;
                        portalUserInfo.IsAccountLocked=updatedCustomerContact.UserInfo.IsAccountLocked;
                        portalUserInfo.IsActive=updatedCustomerContact.UserInfo.IsActive;
                        portalUserInfo.UpdateCount=updatedCustomerContact.UserInfo.UpdateCount;
                        if (updatedCustomerContact.UserInfo && updatedCustomerContact.UserInfo.CustomerUserProjectNumbers) {
                            updatedCustomerContact.UserInfo.CustomerUserProjectNumbers.forEach(x => authProjects.push({
                                projectNumber: x.ProjectNumber
                            }));
                        }
                        this.props.actions.AddExtranetUser(portalUserInfo);
                        this.props.actions.AssignExtranetUserToContact(portalUserInfo,authProjects);
                    }
                    this.updatedCustomerContactData = {};
                    document.getElementById("addCustomerContact").reset();
                    document.getElementById("cancelContactDetail").click();
                    this.contactModalRef.current.style.cssText="display:none";
                }
            }
        }
        else {
                   
            this.updatedCustomerContactData["CustomerAddressId"] = this.addressRef.current.value;
            
            if(this.contactValidation(this.updatedCustomerContactData)){
                if (this.updatedCustomerContactData.ContactPersonName) {
                    this.props.customerAddressData.map(address => {
                        if (address.Contacts) {
                            address.Contacts.map(contact => {
                                if (address.AddressId == document.getElementById("addressId").value && contact.ContactPersonName.trim() == this.updatedCustomerContactData.ContactPersonName.trim() && contact.recordStaus !== "D") {
                                    return isDuplicate = true;
                                }
                            });
                        }
                    });
                }
                if (isDuplicate) {
                    IntertekToaster(localConstant.customer.CONTACT_NAME_ALREADY_EXIST, 'warningToast CustGenContDupCheck');
                }
                else {       
                    this.updatedCustomerContactData["RecordStatus"] = "N";
                    this.updatedCustomerContactData["CustomerAddressId"] = document.getElementById("addressId").value;
                    this.updatedCustomerContactData["parentAddressId"] = document.getElementById("addressId").value;
                    this.updatedCustomerContactData["contactAddress"] = selectedOption[selectedOption.selectedIndex].text;
                    this.updatedCustomerContactData["ContactId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                    this.updatedCustomerContactData["modifiedBy"] = this.props.loggedInUser;
                    this.props.actions.AddCustomerContact(this.updatedCustomerContactData);
                    this.updatedCustomerContactData = {};
                    document.getElementById("addCustomerContact").reset();
                    document.getElementById("cancelContactDetail").click();
                    this.contactModalRef.current.style.cssText="display:none";
                }
            }
        }
    }

    /** Customer Address Validation Handler */
    addressValidation = (data) => {
        if(isEmpty(data) || required(data.Country)){
            IntertekToaster(localConstant.customer.PLEASE_SELECT_A_COUNTRY,"warningToast CountryVal");
            return false;
        }
        if(required(data.County)){
            IntertekToaster(localConstant.customer.PLEASE_SELECT_A_STATE,"warningToast CountyVal");
            return false;
        }
        if(required(data.City)){
            IntertekToaster(localConstant.customer.PLEASE_SELECT_A_CITY,"warningToast CityVal");
            return false;
        }
        if(required(data.Address)){
            IntertekToaster(localConstant.customer.PLEASE_SELECT_ADDRESS,"warningToast AddressVal");
            return false;
        }
        return true;
    };

    addressSubmitHandler = (e) => {
        e.preventDefault();
        if (this.props.showButton === true) {
            const updatedCustomerAddress = Object.assign({},this.props.editedAddressReference,this.updatedCustomerAddressData);
            if(this.addressValidation(updatedCustomerAddress)){
                if (this.props.editedAddressReference.RecordStatus !== "N") {
                    this.updatedCustomerAddressData["RecordStatus"] = "M";
                    this.updatedCustomerAddressData["modifiedBy"] = this.props.loggedInUser;
                }
                this.props.actions.UpdateAddressReference(this.updatedCustomerAddressData);
                this.updatedCustomerAddressData = {};
                document.getElementById("addCustomerAddress").reset();
                document.getElementById("cancelAddressDetail").click();
                this.addressModalRef.current.style.cssText="z-index: 1100;display:none";
                this.setState( { showAddressModal:false } );
            
            }
        }
        if (this.props.showButton === false) {
            if(this.addressValidation(this.updatedCustomerAddressData)){
                this.updatedCustomerAddressData["RecordStatus"] = "N";
                this.updatedCustomerAddressData["Contacts"] = [];
                this.updatedCustomerAddressData["AddressId"] = Math.floor(Math.random() * (Math.pow(10, 5)));
                this.updatedCustomerAddressData["modifiedBy"] = this.props.loggedInUser;
                this.props.actions.AddCustomerAddress(this.updatedCustomerAddressData);
                this.updatedCustomerAddressData = {};
                document.getElementById("addCustomerAddress").reset();
                document.getElementById("cancelAddressDetail").click();
                this.addressModalRef.current.style.cssText="z-index: 1100;display:none";
                this.setState( { showAddressModal:false } );
            }
        }      
    }

    updateAddressHandler = (e) =>{
        if (this.props.customerAddressData) {
            const customerGeneralAddressData = this.props.customerAddressData.filter(address => address.RecordStatus !== "D");
            if(customerGeneralAddressData.length ===1){
                this.updatedCustomerContactData["CustomerAddressId"] = customerGeneralAddressData[0].AddressId;
            }
        }
    }
    clearData = () => {   
        document.getElementById("addCustomerAddress").reset();
        document.getElementById("addCustomerContact").reset();
        //document.getElementById("addressId").removeAttribute("disabled");
        document.getElementById("OtherDetails").value = "";
        this.props.actions.ShowButtonHandler();
        this.props.actions.DisplayFullAddress('');
        this.updateAddressHandler();
        this.addressModalRef.current.style.cssText="display:none";
        this.contactModalRef.current.style.cssText="display:none";
        this.updatedCustomerContactData = {};
        this.updatedCustomerAddressData = {};
        this.setState( { showAddressModal:false } );
        this.setState( { showContactModal:false } );
    }

    addressDeleteClickHandler = () => {
        const selectedData = this.child.getSelectedRows();
        this.hasContacts = false;
        this.hasActiveContacts = false;
        if (selectedData.length === 0) {
            IntertekToaster(localConstant.customer.SELECT_ADDRESS_TO_DELETE, 'warningToast CustGenAddDelChk');
        }
        else {

            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.ADDRESS_DELETE_MESSAGE,
                modalClassName: "warningToast",
                type: "confirm",
                buttons: [
                    {
                        buttonName: "Yes",
                        onClickHandler: this.deleteSelected,
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: "No",
                        onClickHandler: this.confirmationRejectHandler,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            // this.confirmationModalData =confirmationObject;
            // this.setState({isOpen:true})
            this.props.actions.DisplayModal(confirmationObject);
        }
    }

    contactDeleteClickHandler = () => {
        const selectedData = this.secondChild.getSelectedRows();
        if (selectedData.length === 0) {
            IntertekToaster(localConstant.customer.SELECT_CONTACTS_TO_DELETE, 'warningToast CustGenContDelChk');
        }
        else {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.CONTACT_DELETE_MESSAGE,
                modalClassName: "warningToast",
                type: "confirm",
                buttons: [
                    {
                        buttonName: "Yes",
                        onClickHandler: this.deleteContactHandler,
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: "No",
                        onClickHandler: this.confirmationRejectHandler,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            // this.confirmationModalData =confirmationObject;
            // this.setState({isOpen:true})
            this.props.actions.DisplayModal(confirmationObject);
        }
    }

    deleteSelected = () => {
        const selectedData = this.child.getSelectedRows();
        const customesAssociatedAddress = [];
            selectedData.forEach(result => {
                if(!isEmpty(result.Contacts)){
                if (result.Contacts.length > 0) {
                    this.hasContacts = true;
                    result.Contacts.forEach(contact => {
                        if (contact.RecordStatus !== 'D') {
                            this.hasActiveContacts = true;
                            customesAssociatedAddress.push(contact.ContactPersonName);
                        }
                    });
                }
            }
            });
        if (this.hasContacts && this.hasActiveContacts) {
            const alertMessage = customesAssociatedAddress.length;
            const specifiedContacts = "Unable to delete the selected address because it is associated with " + alertMessage + " contact(s).To delete this address please change the address associated with the " + alertMessage + " contact(s).";
            IntertekToaster(specifiedContacts, 'warningToast CustGenAddContChk');
            this.props.actions.HideModal();
        }
        else {
            this.child.removeSelectedRows(selectedData);
            this.props.actions.DeleteCustomerAddress(selectedData);
            //this.setState({isOpen:false})
            this.props.actions.HideModal();
        }
    }
    deleteContactHandler = () => {
        const selectedData = this.secondChild.getSelectedRows();
        if (selectedData.length > 0) {
            this.secondChild.removeSelectedRows(selectedData);
            this.props.actions.DeleteCustomerContact(selectedData);
            // this.setState({isOpen:false})
            this.props.actions.HideModal();
        }
    }
    confirmationRejectHandler = () => {
        // this.setState({isOpen:false})
        this.props.actions.HideModal();
    }
 
    mapContactToExtranetUserData = (data)=>{ 
       const extranetUser={};
        if(data)
        {
            extranetUser.UserType ="Customer";
            extranetUser.ContactId =data.ContactId;
            extranetUser.CustomerAddressId=data.CustomerAddressId;
            extranetUser.UserName=data.ContactPersonName ? data.ContactPersonName : data.UserName;
            extranetUser.LogonName=data.LogonName; 
            extranetUser.Email=data.Email? data.Email : ''; 
        }
        return extranetUser;
    } 
    mapContactToExtranetPortalData = (data,portalData)=>{ 
        const extranetUser={};
         if(data)
         {
             extranetUser.UserType ="Customer";
             extranetUser.ContactId =data.ContactId;
             extranetUser.CustomerAddressId=data.CustomerAddressId;
             extranetUser.UserName=data.ContactPersonName ? data.ContactPersonName : data.UserName;
             extranetUser.LogonName=data.LogonName; 
             extranetUser.Email=data.Email? data.Email : ''; 
             extranetUser.RecordStatus= portalData.UserId !==undefined ? "M" : "N";
             extranetUser.SecurityQuestion1 = portalData.SecurityQuestion1? portalData.SecurityQuestion1 : '';
             extranetUser.SecurityQuestion1Answer = portalData.SecurityQuestion1Answer? portalData.SecurityQuestion1Answer : '';
             extranetUser.ExtranetAccessLevel = portalData.ExtranetAccessLevel? portalData.ExtranetAccessLevel : '';
             extranetUser.Comments = portalData.Comments? portalData.Comments : '';
         }
         return extranetUser;
     }
    showAddressModal=()=>{
       this.addressModalRef.current.style.cssText ="z-index: 1100;display:block";
       this.setState( { showAddressModal:true } );
       this.props.actions.ClearMasterStateCityData();
    }
    showContactModal=()=>
    {
        this.contactModalRef.current.style.cssText="z-index: 1100;display:block";
        this.setState( { showContactModal:true } );

    }
    
    render() {
         
        const {
            customerData,
            customerAddressData,
            customerContactData,
            editedAddressReference,
            editedContactReference,
            extranetUser, 
            securityQuestionsMasterData
        } = this.props;
        const stateMasterData = this.props.stateMasterData;
        const countryMasterData = this.props.countryMasterData;
        const cityMasterData = this.props.cityMasterData;
        const salutationMasterData = this.props.salutationMasterData;
        const vatPrefixMasterData = this.props.vatPrefixMasterData;
        const { showButton } = this.props;
        let extranetUserObj={};     
        let customerGeneralAddressData = [];
        let customerGeneralData = [];
        const customerGeneralContactData = [];
        if (customerAddressData) {
            customerGeneralAddressData = customerAddressData.filter(address => address.RecordStatus !== "D");
        }
        if (customerData) {
            customerGeneralData = customerData;
        }
        if (customerContactData) {
            customerContactData.map(data => {
                const { Contacts } = data;
                if (Contacts !== undefined && Contacts != null && Contacts.length > 0) {
                    Contacts.map(contact => {
                        if (contact.RecordStatus !== 'D') {
                            if (isEmpty(contact.contactAddress)) {
                                contact.contactAddress = data.Address;
                            }
                            contact.parentAddressId = data.AddressId;
                            customerGeneralContactData.push(contact);
                        }
                    });
                }
            });
        }

        this.props.editedContactReference.OtherDetail && (document.getElementById("OtherDetails").value = editedContactReference.OtherDetail);

        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        
     let addressData=[];
        if (Array.isArray(customerGeneralAddressData) && (customerGeneralAddressData).length > 0) {
            addressData = (customerGeneralAddressData).sort((a, b) => {
                return (a.Address < b.Address) ? -1 : (a.Address > b.Address) ? 1 : 0;
            });
        }
        if(extranetUser){ 
            extranetUserObj = this.mapContactToExtranetUserData(extranetUser);
        } 

        if(this.props.companyList && this.props.companyList.length>0)
        {
            this.IsVisibleUnAuthorisedProject();
        }
        //D691 issue 3
        this.functionRef['isDisable']=(this.props.pageMode === localConstant.commonConstants.VIEW);
        this.headerData=GeneralContactHeader(this.functionRef); 
        bindAction(this.headerData, "IsPortalUser", (e,data) =>  this.addPortalUserRowHandler(e,data) );
        return (
            <Fragment>
                {/* <CustomModal modalData={modelData}/>  */}

                {/* this.setState({ portalAddUserModal:false } ); */}
                {this.state.portalAddUserModal &&
                    <Modal title={localConstant.customer.PORTAL_ACCESS} modalId="ExtranetUserDetailsPopup" formId="ExtranetUserDetailsForm" modalClass="popup-position extranetAccountModal" buttons={this.extranetAddUserButtons} isShowModal={this.state.portalAddUserModal}>
                        <ExtranetUserAccount 
                        extranetUserDetails= { extranetUserObj }   
                        addExtranetUser={ this.addExtranetUser  } 
                        // customerProjectDetails ={ this.IsVisibleUnAuthorisedProjects? this.state.customerProjectData.filter(x=> this.props.selectedCompany=== x.contractHoldingCompanyCode) : [] }
                        customerProjectDetails ={ this.state.customerProjectData ? this.state.customerProjectData.filter(x=> this.props.selectedCompany=== x.contractHoldingCompanyCode) :[] }
                        authorisedProjectDetails={this.state.authorisedProjectData}
                        securityQuestions = { securityQuestionsMasterData }
                        onCustomerProjectRowSelected={this.onCustomerProjectRowSelected}  
                        onAuthorisedProjectRowSelected={this.onAuthorisedProjectRowSelected}
                        assignAllProjectClick={this.assignAllProjectClick}
                        removeAllProjectClick={this.removeAllProjectClick}
                        /> 
                       
                    </Modal>
                }
                    <Draggable handle=".handle">
                    <div id="add-address" className="modal" display="none" ref= { this.addressModalRef }>
                        <form onSubmit={this.addressSubmitHandler} id="addCustomerAddress" className="col s12">
                            <div className="modal-content">
                            <h6 className="handle m-0">{!showButton ? localConstant.customer.ADD_ADDRESS:localConstant.customer.EDIT_ADDRESS}  <i class={"zmdi zmdi-close right modal-close"} onClick={this.clearData}></i></h6>
                            <span class="boldBorder"></span>
                                <div className="row mt-3">
                                    <div className="col s3">
                                        <label className="customLabel mandate">{ localConstant.gridHeader.COUNTRY }</label>
                                        <select className="customInputs browser-default" id="CountryId" name="Country" onChange={this.handleChange}>
                                            <option value="">{ localConstant.commonConstants.SELECT }</option>
                                            {countryMasterData.map((data, i) => {
                                                if (data.id === editedAddressReference.CountryId) {
                                                    return <option key={i} value={data.id} selected="true">{data.name}</option>;
                                                }
                                                else {
                                                    return <option key={i} value={data.id}>{data.name}</option>;
                                                }
                                            })}
                                        </select>
                                    </div>
                                    <div className="col s3">
                                        <label className="customLabel mandate">{ localConstant.gridHeader.STATE }</label>
                                        <select className="customInputs browser-default" id="StateId" name="County" onChange={this.handleChange}>
                                            <option value="">{ localConstant.commonConstants.SELECT }</option>
                                            {stateMasterData.map((data, i) => {
                                                if (data.id === editedAddressReference.StateId) {
                                                    return <option key={i} value={data.id} selected="true">{data.name}</option>;
                                                }
                                                else {
                                                    return <option key={i} value={data.id}>{data.name}</option>;
                                                }
                                            })}
                                        </select>
                                    </div>
                                    <div className="col s3">
                                        <label className="customLabel mandate">{ localConstant.gridHeader.CITY }</label>
                                        <select className="customInputs browser-default" id="CityId" name="City" onChange={this.handleChange}>
                                            <option value="">{ localConstant.commonConstants.SELECT }</option>
                                            {cityMasterData.map((data, i) => {
                                                if (data.id === editedAddressReference.CityId) {
                                                    return <option key={i} value={data.id} selected="true">{data.name}</option>;
                                                }
                                                else {
                                                    return <option key={i} value={data.id}>{data.name}</option>;
                                                }
                                            })}
                                        </select>
                                    </div>
                                    <div className="col s3">
                                        <label htmlFor="pin-code" >{ localConstant.gridHeader.POSTAL_CODE }</label>
                                        <input className="customInputs browser-default " maxLength={fieldLengthConstants.Customer.generalDetails.POSTAL_CODE_MAXLENGTH} defaultValue={editedAddressReference.PostalCode} autoComplete="off" name="PostalCode" type="text" onChange={this.handleChange} id="pin-code"  />
                                    </div>

                                </div>
                                <div className="row">
                                    <div className="col s4">
                                        <label htmlFor="full-address" className="customLabel mandate">{ localConstant.gridHeader.FULL_ADDRESS }</label>
                                        <textarea className="customInputs browser-default validate" maxLength={fieldLengthConstants.Customer.generalDetails.FULL_ADDRESS_MAXLENGTH} value={this.props.fullAddress} name="Address" autoComplete="off" onChange={this.handleChange} id="full-address" />
                                    </div>
                                    <div className="col s4">
                                        <label className="customLabel">{ localConstant.gridHeader.EU_VAT_PREFIX }</label>
                                        <select className="customInputs browser-default" id="vatPrefix" defaultValue={editedAddressReference.EUVatPrefix} name="EUVatPrefix" onChange={this.handleChange}>
                                            <option value="">{ localConstant.commonConstants.SELECT }</option>
                                            {vatPrefixMasterData.map((data, i) => {
                                                if (data === editedAddressReference.EUVatPrefix) {
                                                    return <option key={i} value={data} selected="true">{data}</option>;
                                                }
                                                else {
                                                    return <option key={i} value={data}>{data}</option>;
                                                }
                                            })}
                                        </select>
                                    </div>
                                    <div className="col s4">
                                        <label htmlFor="registration-number" className="customLabel">{ localConstant.gridHeader.VAT_TAX_REGISTRATION_NO }</label>
                                        <input className="customInputs browser-default" autoComplete="off" maxLength={fieldLengthConstants.Customer.generalDetails.VAT_TAX_REGISTRATION_NO_MAXLENGTH} name="VatTaxRegNumber" defaultValue={editedAddressReference.VatTaxRegNumber} type="text" onChange={this.handleChange} id="registration-number" />
                                    </div>

                                </div>
                            </div>
                            <div className="modal-footer">
                                <button id="cancelAddressDetail" type="button" onClick={this.clearData} className="modal-close waves-effect waves-teal btn-small mr-2">Cancel</button>
                                {!showButton ? <button type="submit" className="btn-small">{ localConstant.commonConstants.SUBMIT }</button>
                                    :
                                    <button type="submit" className="btn-small">{ localConstant.commonConstants.SUBMIT }</button>}
                            </div>
                        </form>
                    </div>
                    </Draggable>
                    {this.state.showAddressModal && <div className="customModalOverlay"></div> }

                    {/*contact modal         */}
                    <Draggable handle=".handle">
                    <div id="add-contact" className="modal" display="none" ref={this.contactModalRef}>
                    <div className="row">
                        <form onSubmit={this.contactSubmitHandler} id="addCustomerContact" className="col s12">
                            <div className="modal-content">
                                <h6 className="handle m-0">{!showButton?localConstant.customer.ADD_CONTACT:localConstant.customer.EDIT_CONTACT}  <i class={"zmdi zmdi-close right modal-close"} onClick={this.clearData}></i></h6>
                                <span class="boldBorder"></span>
                                <div className="row mt-3">
                                    <div className="col s6">
                                        <label htmlFor="addressId" className="customLabel mandate">{ localConstant.customer.ADDRESS }</label>
                                     
                                       <select  className="customInputs browser-default" id="addressId" ref={ this.addressRef }  value={editedAddressReference.ContactPersonName} name="CustomerAddressId" onChange={this.handleModalChange} >
                                       {addressData.length >1 ?
                                            <option value="" selected>{ localConstant.commonConstants.SELECT }</option> : null}
                                            {
                                                addressData.map((data, i) => {

                                                    if (data.AddressId == editedContactReference.CustomerAddressId) {
                                                    return <option key={i} value={data.AddressId} selected="true">{data.Address}</option>;
                                                    }
                                                    else {
                                                    return <option key={i} value={data.AddressId}>{data.Address}</option>;
                                                    }
                                                })}

                                        </select> 
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col s3">
                                        <label className="customLabel">{ localConstant.gridHeader.SALUTATION }</label>
                                        <select className="customInputs browser-default" id="country" defaultValue={editedContactReference.Salutation} name="Salutation" onChange={this.handleContactChange}>
                                            <option value="">Select Salutation</option>
                                            {salutationMasterData.map((data, i) => {
                                                if (data.name === editedContactReference.Salutation) {
                                                    return <option key={i} value={data.name} selected="true">{data.name}</option>;
                                                }
                                                else {
                                                    return <option key={i} value={data.name}>{data.name}</option>;
                                                }
                                            })}
                                        </select>
                                    </div>
                                    <div className="col s6">
                                        <label htmlFor="ContactPerson" className="customLabel mandate">{ localConstant.customer.CONTACT_NAME }</label>
                                        <input autoComplete="off" type="text" maxLength={fieldLengthConstants.Customer.generalDetails.CONTACT_NAME_MAXLENGTH} className=" browser-default customInputs validate" defaultValue={editedContactReference.ContactPersonName} onInput={this.handleContactChange} name="ContactPersonName" id="ContactPerson" />
                                    </div>
                                    <div className="col s3">
                                        <label htmlFor="Position" className="customLabel">{ localConstant.gridHeader.POSITION }</label>
                                        <input autoComplete="off" type="text" maxLength={fieldLengthConstants.Customer.generalDetails.POSITION_MAXLENGTH} className="browser-default customInputs" defaultValue={editedContactReference.Position} onInput={this.handleContactChange} name="Position" id="Position" />

                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col s4">
                                        <label htmlFor="LandLine" className="customLabel mandate">{ localConstant.gridHeader.TELEPHONE_NO }</label>
                                        <input type="text" maxLength={fieldLengthConstants.Customer.generalDetails.TELEPHONE_NO_MAXLENGTH} autoComplete="off" className="browser-default customInputs validate" defaultValue={editedContactReference.Landline} onInput={this.handleContactChange} name="Landline" id="LandLine" />

                                    </div>
                                    <div className="col s4">
                                        <label htmlFor="Fax" className="customLabel">{ localConstant.gridHeader.FAX_NO }</label>
                                        <input type="text" maxLength={fieldLengthConstants.Customer.generalDetails.FAX_NO_MAXLENGTH} autoComplete="off" className="browser-default customInputs" type="text" defaultValue={editedContactReference.Fax} onInput={this.handleContactChange} name="Fax" id="Fax" />

                                    </div>
                                    <div className="col s4">
                                        <label htmlFor="MobileNo" className="customLabel">{ localConstant.gridHeader.MOBILE_NO }</label>
                                        <input type="text" maxLength={fieldLengthConstants.Customer.generalDetails.MOBILE_NO_MAXLENGTH} autoComplete="off" className="browser-default customInputs" defaultValue={editedContactReference.Mobile} onInput={this.handleContactChange} name="Mobile" id="MobileNo" />
                                    </div>
                                    {/* <div className="input-field col s3">

                                            <input placeholder="Extranet" type="number" onChange={this.handleContactChange} name="defaultUse" id="" />
                                            <label htmlFor="full-address">Extranet</label>
                                        </div> */}
                                </div>
                                <div className="row">
                                    <div className="col s6">
                                        <label htmlFor="Email" className="customLabel">{ localConstant.customer.EMAIL }</label>
                                        <input maxLength={fieldLengthConstants.Customer.generalDetails.EMAIL_MAXLENGTH} autoComplete="off" className="browser-default customInputs" type="text" defaultValue={editedContactReference.Email} onInput={this.handleContactChange} name="Email" id="Email" />
                                    </div>
                                    <div className="col s6">
                                    <label htmlFor="OtherDetails" className="customLabel">{ localConstant.customer.CUST_OTHER_CONTACT_DETAILS }</label>
                                        <textarea id="OtherDetails" autoComplete="off" maxLength={fieldLengthConstants.Customer.generalDetails.CUST_OTHER_CONTACT_DETAILS_MAXLENGTH} name="OtherDetail" onInput={this.handleContactChange} className="browser-default customInputs" />

                                    </div>
                                </div>
                            </div>
                            <div className="modal-footer">
                                <button id="cancelContactDetail" onClick={this.clearData} type="button" className="modal-close waves-effect waves-teal btn-small mr-2">{ localConstant.commonConstants.CANCEL }</button>
                                {!showButton ? <button type="submit" className="btn-small">{ localConstant.commonConstants.SUBMIT }</button>
                                    :
                                    <button type="submit" className="btn-small">{ localConstant.commonConstants.SUBMIT }</button>
                                }
                            </div>
                        </form>
                    </div>
                    </div>
                    </Draggable>
                    {this.state.showContactModal && <div className="customModalOverlay"></div> }

                    <div className="genralDetailContainer customCard mt-0 mr-0">
                    {/* <h6 className="col s12 label-bold mt-2 mb-0 pl-0">Customer Details</h6>
                        <div className="customCard">
                            <div className="row mb-0">
                                <div className="input-field col s5">
                                    <span className="bold">Customer Name: </span> {customerGeneralData.customerName}
                                </div>
                                <div className="input-field col s3">
                                    <span className="bold">Customer Code: </span> {customerGeneralData.customerCode}
                                </div>
                                <div className="input-field col s4">
                                    <span className="bold">Parent Company Name: </span> {customerGeneralData.parentCompanyName}
                                </div>
                            </div>
                        </div> */}

                        <h6 className="col s12 label-bold mt-2 mb-0 pl-0">Addresses</h6>
                       
                            <div className="customCard">
                                <ReactGrid gridRowData={customerGeneralAddressData}
                                    gridColData={HeaderData.GeneralAddressHeader} onRef={ref => { this.child = ref; }} paginationPrefixId={localConstant.paginationPrefixIds.customerAddress}/>
                                     {this.props.pageMode !== localConstant.commonConstants.VIEW && <div className="right-align mt-2">
                                <button onClick={this.showAddressModal} className="btn-small waves-effect modal-trigger">{localConstant.commonConstants.ADD}</button>
                                <button  className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" onClick={this.addressDeleteClickHandler}>{localConstant.commonConstants.DELETE}</button>
                                </div>}
                            </div>
                           
                            <h6 className="col s12 label-bold mt-2 mb-0 pl-0">Contacts</h6>
                        
                        <div className="customCard"> 
                        <ReactGrid gridRowData={customerGeneralContactData} gridColData={this.headerData}  onRef={ref => { this.secondChild = ref; }} paginationPrefixId={localConstant.paginationPrefixIds.customerContact}/>
                        {this.props.pageMode !== localConstant.commonConstants.VIEW &&
                                <div className="right-align mt-2">
                                    <button onClick={this.showContactModal} className="btn-small waves-effect modal-trigger">{localConstant.commonConstants.ADD}</button>
                                    <button className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" onClick={this.contactDeleteClickHandler}>{localConstant.commonConstants.DELETE}</button>
                                </div>}
                        </div>
                            
                        </div>                  
                
            </Fragment>
        );
    }

}

export default GeneralDetails;
