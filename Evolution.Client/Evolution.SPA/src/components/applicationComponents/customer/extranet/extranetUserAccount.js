import React, { Component,Fragment } from 'react';
import MaterializeComponent from 'materialize-css';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData,formInputChangeHandler } from '../../../../utils/commonUtils';
import { HeaderData } from  './headerData';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';

const localConstant = getlocalizeData();
 
const CustomerProjectsGrid = (props) => ( 
    <div className="col s12 mb-2 relative">
    <button type="button" className="btn-small customBtn" onClick ={props.assignAllClick} disabled={props.gridRowData.length>0?false:true} >Assign All</button>
    <ReactGrid 
    gridColData={HeaderData.CustomerProjects} 
    gridRowData={ props.gridRowData} 
    onRowDoubleClicked={ props.onRowSelected} 
    onRef={props.onRef}
    paginationPrefixId={localConstant.paginationPrefixIds.customerProjects}
    />
    </div>
);

const AuthorisedProjectsGrid = (props) => ( 
    <div className="col s12 mb-2 relative">
    <button type="button" className="btn-small customBtn" onClick ={props.removeAllClick} disabled={props.gridRowData.length>0?false:true}>Remove All</button>
    <ReactGrid 
    gridColData={HeaderData.AuthorisedProjects} 
    gridRowData={ props.gridRowData} 
    onRowDoubleClicked={ props.onRowSelected}
    onRef={props.onRef}
    paginationPrefixId={localConstant.paginationPrefixIds.authorisedProjects}
     />
    </div>
);
class ExtranetUserAccount extends Component{
    constructor(props){
        super(props);
        this.extranetUserData = {
            ...this.props.extranetUserDetails
        };
        this.state={
            passwordToggle: false,
            confirmPasswordToggle:false
        };
    };
    passwordToggleHandler = (event) => {
        this.setState({ passwordToggle: !this.state.passwordToggle });
    }
    confirmPasswordToggleHandler = (event) => {
        this.setState({ confirmPasswordToggle: !this.state.confirmPasswordToggle });
    }
    componentDidMount(){
        const tab = document.querySelectorAll('.tabs');
        const tabInstances = MaterializeComponent.Tabs.init(tab); 
    }

    sendDataToParentCallback = () => { 
        this.props.addExtranetUser(this.extranetUserData);
   }

   onChangeHandler = (e) => {    
    e.stopPropagation();
    const inputvalue = formInputChangeHandler(e); 
    this.extranetUserData[inputvalue.name] = inputvalue.value;
    this.sendDataToParentCallback();    
    }

    onPasteHandler = (e) => {    
        e.preventDefault();
        }

    render(){
        return(
            <div className="row">
                <div className="col s12">
                    <ul className="tabs">
                        <li className="tab col s6"><a className="active" href="#userDetails">{localConstant.customer.PORTAL_USER_DETAILS}</a></li>
                        <li className="tab col s6"><a href="#authorisedProjects">{localConstant.customer.PORTAL_USER_PROJECT_ACCESS}</a></li>
                    </ul>
                </div>
                <div id="userDetails" className="col s12">                   
                        <div className="row mb-0">
                            <CustomInput
                                hasLabel={true}                              
                                label={localConstant.customer.CONTACT_NAME}
                                type='text'
                                dataValType='valueText'
                                colSize='s12 m4'
                                inputClass="customInputs"
                                labelClass="customLabel mandate"
                                name="UserName"
                                maxLength={50}
                                readOnly={true}
                                value={this.props.extranetUserDetails.UserName }
                                onValueBlur={this.onChangeHandler}
                            />
                            <CustomInput
                                hasLabel={true}                              
                                label={localConstant.customer.USER_NAME}
                                type='text'                                
                                colSize='s12 m4'
                                inputClass="customInputs"
                                labelClass="customLabel mandate"
                                name="LogonName"
                                maxLength={fieldLengthConstants.Customer.portalAccess.USER_NAME_MAXLENGTH}
                                disabled={this.props.interactionMode}
                                defaultValue={this.props.extranetUserDetails.LogonName}
                                onValueBlur={this.onChangeHandler}
                            />
                          
                            <CustomInput
                                hasLabel={true} 
                                label={localConstant.customer.EMAIL}
                                type='email'                                
                                colSize='s12 m4'
                                inputClass="customInputs"
                                labelClass="customLabel mandate"
                                name="Email"
                                maxLength={fieldLengthConstants.Customer.portalAccess.EMAIL_MAXLENGTH}
                                disabled={this.props.interactionMode} 
                                defaultValue={this.props.extranetUserDetails.Email}
                                onValueBlur={this.onChangeHandler}
                            />
                        </div>
                        <div className="row mb-0">
                                <CustomInput
                                    hasLabel={true}                                 
                                    label={localConstant.customer.PASSWORD}
                                    type={this.state.passwordToggle ? 'text' : 'password'}
                                    dataTypePassword='text'                                  
                                    colSize='s12 m6'
                                    inputClass="customInputs"
                                    labelClass="customLabel mandate"
                                    name="Password"
                                    maxLength={fieldLengthConstants.Customer.portalAccess.PASSWORD_MAXLENGTH}
                                    disabled={this.props.interactionMode}
                                    showPasswordToggle={this.passwordToggleHandler}
                                    isVisable={this.state.passwordToggle}
                                    defaultValue={this.props.extranetUserDetails.Password}
                                    onValueBlur={this.onChangeHandler}
                                />                       
                                
                                <CustomInput
                                    hasLabel={true}                                 
                                    label={localConstant.customer.CONFIRM_PASSWORD}
                                    type={this.state.confirmPasswordToggle ? 'text' : 'password'}
                                    dataTypePassword='text'
                                    colSize='s12 m6'
                                    inputClass="customInputs"
                                    labelClass="customLabel mandate"
                                    name="confirmPassword"
                                    maxLength={fieldLengthConstants.Customer.portalAccess.CONFIRM_PASSWORD_MAXLENGTH}
                                    disabled={this.props.interactionMode}
                                    showPasswordToggle={this.confirmPasswordToggleHandler}
                                    isVisable={this.state.confirmPasswordToggle}
                                    defaultValue={this.props.extranetUserDetails.confirmPassword ? this.props.extranetUserDetails.confirmPassword: this.props.extranetUserDetails.Password }
                                    onValueBlur={this.onChangeHandler}
                                    onValuePaste={this.onPasteHandler}
                                />  
                            </div>
                            <div className="row mb-0">
                            <CustomInput
                                hasLabel={true}                              
                                label={localConstant.customer.PASSWORD_QUESTION}
                                type='text'                              
                                colSize='s12 m6'
                                inputClass="customInputs"
                                labelClass="customLabel "
                                name="SecurityQuestion1"
                                maxLength={fieldLengthConstants.Customer.portalAccess.PASSWORD_QUESTION_MAXLENGTH}
                                disabled={this.props.interactionMode}
                                defaultValue={this.props.extranetUserDetails.SecurityQuestion1}
                                onValueBlur={this.onChangeHandler}
                            />
                                <CustomInput
                                    hasLabel={true}                                  
                                    label={localConstant.customer.PASSWORD_ANSWER}
                                    type='text'                                    
                                    colSize='s12 m6'
                                    inputClass="customInputs"
                                    labelClass="customLabel"
                                    name="SecurityQuestion1Answer"
                                    maxLength={fieldLengthConstants.Customer.portalAccess.PASSWORD_ANSWER_MAXLENGTH}
                                    disabled={this.props.interactionMode}
                                    defaultValue={this.props.extranetUserDetails.SecurityQuestion1Answer}
                                    onValueBlur={this.onChangeHandler}
                                />
                                 </div>
                            <div className="row mb-0">
                            <CustomInput
                                type='switch'
                                switchLabel={localConstant.customer.LOCKED_OUT}
                                isSwitchLabel={true}
                                checkedStatus={ this.props.extranetUserDetails.IsAccountLocked }
                                switchKey={ this.props.extranetUserDetails.IsAccountLocked }
                                //isSwitchLabel={this.props.editedPairollPeriodName.isActive}
                                switchName="IsAccountLocked"
                                id="chargeTypeStatus"
                                colSize='m3 s12 mt-4'
                                onChangeToggle={this.onChangeHandler}
                            />
                            <CustomInput
                                type='switch'
                                switchLabel={localConstant.customer.SHOW_NEW_VISITS}
                                isSwitchLabel={true}
                                checkedStatus={this.props.extranetUserDetails.IsShowNewVisit}
                                switchKey={ this.props.extranetUserDetails.IsShowNewVisit }
                                //isSwitchLabel={this.props.editedPairollPeriodName.isActive}
                                switchName="IsShowNewVisit"
                                id="IsShowNewVisit"
                                colSize='m3 s12 mt-4'
                                onChangeToggle={this.onChangeHandler}
                            />
                            <CustomInput
                                hasLabel={true}                             
                                label={localConstant.customer.PORTAL_ACCESS}
                                type='select'
                                colSize='s12 m6'
                                className="browser-default"
                                optionsList={ localConstant.commonConstants.Portal_Access}
                                labelClass="customLabel"
                                optionName='name'
                                optionValue="value"
                                name="ExtranetAccessLevel"
                                disabled={this.props.interactionMode}
                                defaultValue={this.props.extranetUserDetails.ExtranetAccessLevel}
                                onSelectChange={this.onChangeHandler}
                            />
                             </div>
                            <div className="row mb-2">
                            <CustomInput
                                hasLabel={true}
                                labelClass="customLabel"
                                label={localConstant.customer.COMMENTS}
                                divClassName="s12"
                                type='textarea'
                                name='Comments'
                                colSize='s12'
                                maxLength={ fieldLengthConstants.Customer.portalAccess.COMMENTS_MAXLENGTH }
                                inputClass="customInputs"
                                defaultValue={this.props.extranetUserDetails.Comments}
                                onValueBlur={this.onChangeHandler}
                            />
                        </div>
                  
                </div>
                <div id="authorisedProjects" className="col s12">
                    {/* <div className="col s6 mb-2 relative">
                    <button type="submit" className="btn-small customBtn">Assign All</button>
                    <ReactGrid 
                    gridColData={HeaderData.CustomerProjects} 
                    gridRowData={ this.props.customerProjectDetails } 
                    onSelectionChanged={this.props.onCustomerProjectRowSelected}
                    onRef={ref => { this.child = ref; }}
                    />
                    </div>
                    <div className="col s6 mb-2 relative">
                    <button type="submit" className="btn-small customBtn">Remove All</button>
                    <ReactGrid 
                    gridColData={HeaderData.AuthorisedProjects} 
                    gridRowData={ this.props.customerProjectDetails }
                    onSelectionChanged={this.props.onAuthorisedProjectRowSelected}
                    onRef={ref1 => { this.child2 = ref1; }}
                     />
                    </div> */}
                     <div className="customCard mt-0 mr-0 width">
                     <h6 class="pl-0 bold"><span>Unauthorised Projects</span></h6>
                    <CustomerProjectsGrid
                        gridRowData={ this.props.customerProjectDetails }
                        onRowSelected={(e)=> this.props.onCustomerProjectRowSelected(e) }
                        //onRef={ref => { this.child = ref; } }
                        assignAllClick={ this.props.assignAllProjectClick }
                    /> 
                    </div>
                    <div className="customCard mt-0 mr-0 width">
                    <h6 class="pl-0 bold"><span>Authorised Projects</span></h6>
                        <AuthorisedProjectsGrid
                        gridRowData={ this.props.authorisedProjectDetails }
                        onRowSelected={ (e) => this.props.onAuthorisedProjectRowSelected(e) }
                        //onRef={ref => { this.secondChild = ref; } }  
                        removeAllClick={ this.props.removeAllProjectClick }     
                    />
                    </div>
                      
                </div>
               
            </div>
        );
    }
}

export default ExtranetUserAccount;