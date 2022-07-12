import React,{ Component,Fragment } from 'react';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import { getlocalizeData,bindAction,isEmpty,isValidEmailAddress,deepCopy,isEmptyOrUndefine } from '../../../../utils/commonUtils';
import { stringWithOnlySpaces } from '../../../../utils/validator';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from  './headerData';
import ExtranetUserAccount from '../extranet/extranetUserAccount'; 
import Modal from '../../../../common/baseComponents/modal'; 
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import arrayUtil from '../../../../utils/arrayUtil';

const localConstant = getlocalizeData();
class PortalAccess extends Component{
    constructor(props){
        super(props);
        this.IsVisibleUnAuthorisedProjects=false;
        this.state={ 
            portalAddUserModal:false,
            authorisedProjectData : [] ,
            customerProjectData: []  
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
        bindAction(HeaderData.ExtranetAccountsHeaderData,"PortalUserColumn",this.editPortalUserRowHandler);          
    }
    componentDidMount(){
       this.portalUserList(); 
    }

    portalUserList = () => {
        const userList = [];
        if (this.props.customerAddressData) {
            this.props.customerAddressData.forEach(element => {
                if (element.Contacts) {
                    element.Contacts.forEach(item => {
                        if (item.UserInfo) {    //Removed IsActive Check for ITK Defect -1127
                            if(item.UserInfo.Password){
                                item.UserInfo.confirmPassword=item.UserInfo.Password;//Fixes for IGO-954
                            }
                            userList.push(item.UserInfo);
                        }
                    });
                }
            });
        } 
       this.props.actions.ExtranetUserAccountsList(userList);
    }

    editPortalUserRowHandler = (data) => {
        
        const projectSearchFilter = {
            contractCustomerCode: this.props.selectedCustomerCode,
            // contractHoldingCompanyCode: this.props.selectedCompany // Defect 726,686
        };
        if(data)
        {
            data= this.mapContactToExtranetUserData(data) ;
        }
        this.props.actions.AddExtranetUser(data);
        this.setState({ portalAddUserModal: true });
        this.props.actions.FetchProjectList(projectSearchFilter).then(res => {
            if (res) {
                this.setState({ customerProjectData: res });
                this.fetchAuthorizedProjecList(data.CustomerUserProjectNumbers);
            }
        });
 
    }

    fetchAuthorizedProjecList = (data) => {
         
        const authProjectData = [];
        let custProjectData = [];
        if (data.length > 0) {
            this.props.customerProjectList.forEach(prj => {
                const  idx = data.findIndex(val=> val.ProjectNumber===prj.projectNumber &&val.RecordStatus !=='D'); //Changes for D-726
                if (idx >= 0) {
                    prj.recordStatus=data[idx].RecordStatus;//Changes for D-726
                    authProjectData.push(prj);
                }
                else if(this.props.selectedCompany ==prj.contractHoldingCompanyCode) {  // Defect 726,686
                    custProjectData.push(prj);
                } 
            });
        }
        else {
            custProjectData = [ ...this.props.customerProjectList ];
        }

        this.setState({
            authorisedProjectData: authProjectData,
            customerProjectData: custProjectData
        });
    }
    
    addExtranetUser=(data)=> {  
        if(this.props.extranetUser)
        {
            data= this.mapContactToExtranetUserData(data) ;
        } 
        if(data.UserId>0) 
            data.RecordStatus="M";
            
         this.props.actions.AddExtranetUser(data);
     }
      
    //Cancel Extranet User Modal Popup
    cancelExtranetUserModal=()=>{
        
        this.setState({ portalAddUserModal:false } );
    }
    
    //Submit New Extranet User Modal Popup
    createExtranetUserModal = (e) => { 
        e.preventDefault();
        if (this.extranetUserMandatoryFieldValidation()) {

            const extranetUser= this.props.extranetUser; 
            if(extranetUser.RecordStatus !== "N")
            {
                extranetUser.RecordStatus="M";
            } 
            this.props.actions.AssignExtranetUserToContact(extranetUser,this.state.authorisedProjectData);
            this.setState({ portalAddUserModal:false } );
            this.portalUserList();
        } 
    }

    extranetUserMandatoryFieldValidation = () => { 
        const passwordRegEx = /^[A-Za-z0-9](?=.*[^a-zA-Z0-9])(?!.*\s).{6,127}$/; //Changes for IGO D946
        if (this.props.extranetUser) {
            // Contact Name
            if (isEmpty(this.props.extranetUser.UserName)) {
                IntertekToaster(localConstant.customer.REQUIRED_CUSTOMER_PORTAL_CUSTOMER_NAME, 'warningToast');
                return false;
            }

            // User Name
            if (isEmpty(this.props.extranetUser.LogonName)) {

                IntertekToaster(localConstant.validationMessage.EXTRANET_USER_NAME_VAL, 'warningToast');
                return false;
            }
            // Email
            if (isEmpty(this.props.extranetUser.Email)) {

                IntertekToaster(localConstant.customer.REQUIRED_CUSTOMER_PORTAL_EMAIL, 'warningToast');
                return false;
            }
            // Valid Email Address
            if (!isEmpty(this.props.extranetUser.Email) && (stringWithOnlySpaces(this.props.extranetUser.Email)|| !isValidEmailAddress(this.props.extranetUser.Email))) {

                IntertekToaster(localConstant.validationMessage.EMAIL_VALIDATION, 'warningToast');
                return false;
            }
            // Password
            if (isEmpty(this.props.extranetUser.Password)) {
                IntertekToaster(localConstant.validationMessage.EXTRANET_PASSWORD_VAL, 'warningToast');
                return false;
            }

            if (this.props.extranetUser.Password.length < 7) {
                IntertekToaster(localConstant.techSpec.contactInformation.PASSWORD_VALIDATION, 'warningToast');
                return false;
            }

            if (!passwordRegEx.test(this.props.extranetUser.Password)) {
                IntertekToaster(localConstant.techSpec.contactInformation.PASSWORD_VALIDATION, 'warningToast');
                return false;
            }
            if (isEmpty(this.props.extranetUser.confirmPassword)) {
                IntertekToaster(localConstant.validationMessage.EXTRANET_CNFM_PASSWORD_VAL, 'warningToast');
                return false;
            }
    
            if(!isEmpty(this.props.extranetUser.Password) && !isEmpty(this.props.extranetUser.confirmPassword) && this.props.extranetUser.Password!=this.props.extranetUser.confirmPassword  )
            {
                IntertekToaster(localConstant.customer.MISMATCH_CONFIRM_PASSWORD, 'warningToast');
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
            return true;
        }
    }

    swapProjectInfos = (selectedRows, srcProjectData, destProjectData) => {
        if (selectedRows) {
            const index = srcProjectData.findIndex(val => (val.projectNumber === selectedRows.projectNumber));
            if (index >= 0) {
                srcProjectData[index].recordStatus=null;//Changes for D-726
                destProjectData.push(...srcProjectData.splice(index, 1));
            }
        }
    }

     onCustomerProjectRowSelected = (e) => {   
        const selectedRows = e.api.getSelectedRows() ; 
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
        //Changes for D-726 -Starts
        const authProjectData =Object.assign([],this.state.authorisedProjectData);
        authProjectData.forEach(x=>
            {
                if(selectedRows[0].projectNumber == x.projectNumber){
                    x.recordStatus ='D';
                }
            }
           );
        //Changes for D-726 -End
        const custProjectData = [ ...this.state.customerProjectData ]; 
        const authProjects=deepCopy(authProjectData);
        this.swapProjectInfos(selectedRows[0],authProjects,custProjectData) ; 
        this.setState({
            authorisedProjectData: authProjectData,
            customerProjectData: custProjectData
        });
    }

    assignAllProjectClick = (e) => {  
        //Changes for D-726 -Starts
        const customerProjectList =deepCopy(this.props.customerProjectList);
        customerProjectList.forEach(x=>
           x.recordStatus = null );
        //Changes for D-726 -End
        const unAuthorisedProjects=this.state.authorisedProjectData.concat(customerProjectList.filter(x=>this.props.selectedCompany=== x.contractHoldingCompanyCode)); //D-1288
        const distinctUnAuthorisedProjects = arrayUtil.removeDuplicates(unAuthorisedProjects, "projectNumber");  //D-1345
        this.setState({ authorisedProjectData : distinctUnAuthorisedProjects });//D-1288
        this.setState({ customerProjectData:[] });
    } 

    removeAllProjectClick = (e) => {  
        //Changes for D-726 -Starts
        const authorisedProjectData =deepCopy(this.state.authorisedProjectData);
         authorisedProjectData.forEach(x=>
            x.recordStatus ='D');
        //Changes for D-726 -End
        this.setState({ authorisedProjectData :authorisedProjectData });
        this.setState({ customerProjectData : this.props.customerProjectList });
    }

    mapContactToExtranetUserData = (data)=>{ 
        const extranetUser={ ...data };
         if(data)
         {
             extranetUser.UserType ="Customer";
             extranetUser.ContactId =data.ContactId;
             extranetUser.CustomerAddressId=data.CustomerAddressId;
             extranetUser.UserName=data.ContactPersonName ? data.ContactPersonName : data.UserName;
             extranetUser.confirmPassword=data.confirmPassword !== undefined ? data.confirmPassword : data.Password; //Changes for IGO - D954
             extranetUser.LogonName=data.LogonName; 
             extranetUser.Email=data.Email? data.Email : ''; 
         }
         return extranetUser;
     }

     IsVisibleUnAuthorisedProject = () =>
     {     
         const selectedCompanyObj = Object.assign([],this.props.companyList).find(x => x.companyCode === this.props.selectedCompany );
         if(selectedCompanyObj)
         {  
             this.IsVisibleUnAuthorisedProjects = !!Object.assign([],this.props.customerAddressData).find(x=>x.Country=== selectedCompanyObj.operatingCountry && x.RecordStatus!= "D");  
         }
     }

    render(){ 
        const { 
            securityQuestionsMasterData,
            extranetUser,
            extranetUsers
        } = this.props;
        
        if(this.props.companyList && this.props.companyList.length>0)
        {
            this.IsVisibleUnAuthorisedProject();
        } 

        return(
            <Fragment>
              {this.state.portalAddUserModal &&
                    <Modal title={localConstant.customer.PORTAL_ACCESS} modalId="ExtranetUserDetailsPopup" formId="ExtranetUserDetailsForm" modalClass="popup-position extranetAccountModal" buttons={this.extranetAddUserButtons} isShowModal={this.state.portalAddUserModal}>
                        <ExtranetUserAccount 
                        extranetUserDetails= { extranetUser }   
                        addExtranetUser={ this.addExtranetUser  } 
                        // customerProjectDetails ={ this.IsVisibleUnAuthorisedProjects ? this.state.customerProjectData.filter(x=> this.props.selectedCompany=== x.contractHoldingCompanyCode) : [] } //Changes for D-726 Operating Country check not needed
                        customerProjectDetails ={this.state.customerProjectData ? this.state.customerProjectData.filter(x=> this.props.selectedCompany=== x.contractHoldingCompanyCode) :[]} //Added for D-726 
                        authorisedProjectDetails={this.state.authorisedProjectData? this.state.authorisedProjectData.filter(x=>x.recordStatus !=='D'):[]}//Changes for D-726 //D-1288
                        securityQuestions = { securityQuestionsMasterData }
                        onCustomerProjectRowSelected={this.onCustomerProjectRowSelected}  
                        onAuthorisedProjectRowSelected={this.onAuthorisedProjectRowSelected}
                        assignAllProjectClick={this.assignAllProjectClick}
                        removeAllProjectClick={this.removeAllProjectClick}
                        />  
                    </Modal>
                }
                <div className="genralDetailContainer customCard">
                    <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.customer.PORTAL_ACCOUNTS} colSize="s12">
                        <ReactGrid gridColData={HeaderData.ExtranetAccountsHeaderData}
                            gridRowData={ extranetUsers } paginationPrefixId={localConstant.paginationPrefixIds.extranetPortalAccounts}/>
                    </CardPanel>
                </div>
            </Fragment>
        );
    }
}

export default PortalAccess;