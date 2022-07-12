import React, { Component, Fragment } from 'react';
import { isEmpty, isEmptyOrUndefine } from '../../../utils/commonUtils';
import { getlocalizeData, customRegExValidator } from '../../../utils/commonUtils';
import { Helmet } from 'react-helmet';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';
import Logo from '../../../assets/images/logo.png';
import { AppDashBoardRoutes, AppMainRoutes } from '../../../routes/routesConfig';
const localConstant = getlocalizeData();
class ForgotPassword extends Component {
    constructor(props) {
        super(props);
        this.state = {

            forgotuserName: '',
            forgotuserEmail: '',
            isSecurityQuestionExists: false,
            onSubmitPassword: false,
            displayErrorMessage: false,
            userAnswer: '',
            newPassword: '',
            confirmPassword: '',
            isResetPasswordSet: false
        };
    };
    handleCredential = (e) => {
        this.setState({
            [e.target.name]: e.target.value
        });
    }
    emailValidation(value) {
        if ((value) &&
            customRegExValidator(/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/, 'i', value)) {
            IntertekToaster(localConstant.techSpec.contactInformation.EMAIL_VALIDATION, 'warningToast');
            return false;
        }
        return true;
    }
    AuthenticateForgotPasswordUser = (e) => {
        e.preventDefault();
        const loginData = {
            Username: isEmpty(this.props.loginUserName)?this.state.forgotuserName:isEmpty(this.state.forgotuserName)?this.props.loginUserName:this.state.forgotuserName,
            UserEmail: this.state.forgotuserEmail,
        };
        if (this.state.forgotuserName === '' && isEmpty(loginData.Username)) {
            IntertekToaster(localConstant.login.VALID_USER, 'warningToast invalidUser');
            return false;
        }
        if (this.state.forgotuserEmail === '') {
            IntertekToaster(localConstant.login.VALID_EMAIL, 'warningToast invalidUser');
            return false;
        }
        if (this.emailValidation(this.state.forgotuserEmail)) {
            if (this.state.forgotuserEmail !== '' && loginData.Username !== '') {

                this.props.actions.securityQuestion(loginData).then((res) => {
                    if (res === true) {
                        if (this.state.forgotuserEmail !== '' && loginData.Username !== '' && isEmpty(this.props.userSecurity && this.props.userSecurity)) {
                            this.setState({
                               
                                displayErrorMessage: true,
                                isSecurityQuestionExists: false
                            });
                        }else if (this.state.forgotuserEmail !== '' && loginData.Username !== '' && isEmpty(this.props.userSecurity && this.props.userSecurity[0])) {
                            this.setState({
                                displayErrorMessage: true,
                                
                            });
                        }
                        else {
                            this.setState({
                                isSecurityQuestionExists: true
                            });
                        }
                    }
                });
            }
        }
    }
    onClickSubmit = (e) => {
        e.preventDefault();
        const loginData = {
            Username: isEmpty(this.props.loginUserName)?this.state.forgotuserName:isEmpty(this.state.forgotuserName)?this.props.loginUserName:this.state.forgotuserName,
            UserEmail: this.state.forgotuserEmail,
            Question: this.props.userSecurity[0],
            Answer: this.state.userAnswer
        };
        if (this.state.userAnswer === '') {
            IntertekToaster(localConstant.login.VALID_ANSWER, 'warningToast invalidUser');
            return false;
        }
            
        this.props.actions.validateSecurityQuestionAnswer(loginData)
            .then(result => {
                if (result) {                    
                    this.setState({
                        isResetPasswordSet: true,
                    });
                }
            });      
    }
    onClickReset = (e) => {
        const validation = /^[A-Za-z0-9](?=.*[^a-zA-Z0-9])(?!.*\s).{6,14}$/;
        if (isEmptyOrUndefine(this.state.newPassword)) {
            IntertekToaster(localConstant.login.VALID_PASSWORD, 'warningToast');
        } else if (isEmptyOrUndefine(this.state.confirmPassword)) {
            IntertekToaster(localConstant.login.VALID_CONFIRM_PASSWORD, 'warningToast');
        } else if (this.state.newPassword !== this.state.confirmPassword) {
            IntertekToaster(localConstant.login.PASSWORD_NOT_MATCHING, 'warningToast');
        } else if (!validation.test(this.state.newPassword) && !validation.test(this.state.confirmPassword)) {
            IntertekToaster(localConstant.techSpec.contactInformation.PASSWORD_VALIDATION, 'warningToast');
        } else {
            const loginData = {
                Username: isEmpty(this.props.loginUserName)?this.state.forgotuserName:isEmpty(this.state.forgotuserName)?this.props.loginUserName:this.state.forgotuserName,
                UserEmail: this.state.forgotuserEmail,
                Password: this.state.newPassword
            };
            this.props.actions.resetPassword(loginData)
            .then(result => {
                if (result) {                               
                    this.onClickBacktoLogin();
                    IntertekToaster(localConstant.login.PASSWORD_RESET_SUCCESSFULL, 'successToast');
                }
            });  
        }
    }
    onClickBacktoLogin=(e)=>{
        this.props.history.push(AppMainRoutes.login);
        this.props.actions.loginUserName(null);
    }
    componentWillMount() {
        const { loginStatus, history } = this.props;
        if (loginStatus) {
            history.push(AppDashBoardRoutes.assignments);
        }
    }

    render() {

        return (

            <Fragment>
                <Helmet
                    title="Intertek-Login">
                </Helmet>
                <div className="row LoginPageContent">
                    <div className="col l12 m12 loginParentStyle">
                        <div className="row">
                            <div className="col l6 m6">
                                <div className="col s11 circleOuterStyle">
                                    <div className="col s12 circleStyle circle right-align">
                                        <img className="Logoimg pr-5" src={Logo} alt="Logoimage" />
                                        <div className="circle lightBlueCircle">
                                            <div className="circle whiteCircle"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="col l5 m5 mt-8 Loginform">
                               
                                {this.state.displayErrorMessage ?
                                <Fragment>
                                    <fieldset className="login-fieldset col s11 ">
                                        <legend className="login-legend p-3"><b>{localConstant.login.FORGET_PASSWORD}</b></legend>
                                        <div className="center-align">
                    
                                            {(this.props.userSecurity && this.props.userSecurity[0] === null) ?
                                                <p > {localConstant.login.QUESTION_NOT_FOUND}</p>
                                                :
                                                <p > {localConstant.login.VALIDATION_MESSAGE}</p>

                                            }
                                            <button type="button" className="waves-effect waves-teal btn-small ml-2 pl-3 center-align mt-2 mb-3" onClick={this.onClickBacktoLogin}> {localConstant.login.BACK_TO_LOGIN} </button>
                                        </div>
                                        
                                    </fieldset> 
                                    <div className="clear"></div>
                                    <p className="center-align login-footer"> 2019 &copy; Intertek Group plc</p>

                                    </Fragment>:
                                    <form className="col s12" onSubmit={this.AuthenticateUser}>
                                        <fieldset className="login-fieldset" >
                                            <legend className="login-legend pl-3 pr-3"><b>{localConstant.login.LOGIN_WELCOME_MESSAGE} </b></legend>
                                            {!this.state.isResetPasswordSet ? 
                                                <Fragment>
                                                    <p className="pl-3 pt-2 pb-2"> {localConstant.login.FORGOT_PASSWORLD_WELCOME}</p>
                                                    <div className="row">
                                                        <div className="input-field col s12  mt-1 pl-4 pr-4">
                                                            <i className="tiny material-icons prefix customIcon zmdi zmdi-account-circle left"></i>
                                                            <input type="text" name="forgotuserName" placeHolder="Username" onChange={this.handleCredential} defaultValue={this.props.loginUserName}
                                                                disabled={this.state.isSecurityQuestionExists && (this.props.userSecurity && !isEmpty(this.props.userSecurity[0])) ? true : false}
                                                            />
                                                        </div>
        
                                                        <div className="input-field col s12  mt-1 pl-4 pr-4">
                                                            <i className="tiny material-icons prefix customIcon zmdi zmdi-email left"> </i>
                                                            <input type="text" name="forgotuserEmail" placeHolder="Email" onChange={this.handleCredential}
                                                                disabled={this.state.isSecurityQuestionExists && (this.props.userSecurity && !isEmpty(this.props.userSecurity[0])) ? true : false} />
                                                        </div>
                                                        {this.state.isSecurityQuestionExists && (this.props.userSecurity && !isEmpty(this.props.userSecurity[0])) ?
                                                            <Fragment>
                                                                <p className="pl-4"> {this.props.userSecurity}</p>
                                                                <div className="input-field col s12  mt-1 pl-4 pr-4">
                                                                    <i className="tiny material-icons prefix customIcon zmdi zmdi-help left"></i>
                                                                    <input type="text" name="userAnswer" placeHolder="Security Question Answer" onChange={this.handleCredential}
                                                                    />
                                                                </div>
                                                            </Fragment>
                                                            :
                                                            null
                                                        }
                                                        <div className="pl-2 col s6 left-align mt-0 pl-3">
                                                            <i className="material-icons right pt-1"><img alt="Login" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAAGXcA1uAAAAAXNSR0IArs4c6QAAARNJREFUSA3tlcsOwUAUhkmwkYjLs4j7K9h5BEE8mBXBQqSv4LZlY2+JnYX6jrQSbWcyra5wkj/Tc87//3NJ20kktGHbdtpPoFp/VUkKr0T1kJQGTFtFiKvOFHevV1I7sbbptfosZ6pm0AJ9rpCqodYFOS8CIuNz0xUQFEWl43xdj/1eQdtoYxAvoGdKPkMempItyKYxeprCXoG50QwuCcEGTNzcaESwAx0j8p8U7QQ44BZYgGw0B4UKwwaQ9/wOugpa+DJmNSDfhMQgvINf4V4PFVoWyIEjmIE4YptyXPaMB1B28iXjzXn+ZDi9iTkW+Ruv5XyIKQi4Rd8k0RKM5e+9BRJj4O40mqFKhXEJ9FX936g/ADYk7y1IIvnlAAAAAElFTkSuQmCC" /></i>
                                                            <button type="button" className="waves-effect waves-teal btn-small ml-2 pl-3" onClick={this.onClickBacktoLogin}> {localConstant.login.BACK_TO_LOGIN} </button>
                                                        </div>
                                                        {this.state.isSecurityQuestionExists && (this.props.userSecurity && !isEmpty(this.props.userSecurity[0])) ?
                                                            <div className="pl-2 col s6 right-align pr-3">
                                                                <button type="submit" className="waves-effect waves-teal btn-small right-align" onClick={this.onClickSubmit}> {localConstant.login.SUBMIT}</button>
                                                            </div> :
                                                            <div className="pl-2 col s6 right-align pr-3">
                                                                <button type="submit" className="waves-effect waves-teal btn-small right-align" onClick={this.AuthenticateForgotPasswordUser}>
                                                                    {localConstant.login.VALIDATE}</button>
                                                            </div>
                                                        }
        
                                                    </div> 
                                                </Fragment>
                                                : 
                                                <Fragment>
                                                    <p className="pl-3 pt-2 pb-2"> {localConstant.login.RESET_PASSWORD_MESSAGE}</p>
                                                    <div className="row">
                                                        <div className="input-field col s12  mt-1 pl-4 pr-4">
                                                            <i className="tiny material-icons prefix customIcon zmdi zmdi-key left"></i>
                                                            <input type="password" name="newPassword" placeHolder="Password" onChange={this.handleCredential} value={this.state.newPassword} />
                                                        </div>
        
                                                        <div className="input-field col s12  mt-1 pl-4 pr-4">
                                                            <i className="tiny material-icons prefix customIcon zmdi zmdi-key left"> </i>
                                                            <input type="password" name="confirmPassword" placeHolder="Confirm Password" onChange={this.handleCredential} value={this.state.confirmPassword} />
                                                        </div>

                                                        <div className="pl-2 col s6 left-align mt-0 pl-3">
                                                            <i className="material-icons right pt-1"><img alt="Login" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAAGXcA1uAAAAAXNSR0IArs4c6QAAARNJREFUSA3tlcsOwUAUhkmwkYjLs4j7K9h5BEE8mBXBQqSv4LZlY2+JnYX6jrQSbWcyra5wkj/Tc87//3NJ20kktGHbdtpPoFp/VUkKr0T1kJQGTFtFiKvOFHevV1I7sbbptfosZ6pm0AJ9rpCqodYFOS8CIuNz0xUQFEWl43xdj/1eQdtoYxAvoGdKPkMempItyKYxeprCXoG50QwuCcEGTNzcaESwAx0j8p8U7QQ44BZYgGw0B4UKwwaQ9/wOugpa+DJmNSDfhMQgvINf4V4PFVoWyIEjmIE4YptyXPaMB1B28iXjzXn+ZDi9iTkW+Ruv5XyIKQi4Rd8k0RKM5e+9BRJj4O40mqFKhXEJ9FX936g/ADYk7y1IIvnlAAAAAElFTkSuQmCC" /></i>
                                                            <button type="button" className="waves-effect waves-teal btn-small ml-2 pl-3" onClick={this.onClickBacktoLogin}> {localConstant.login.BACK_TO_LOGIN} </button>
                                                        </div>                                                        
                                                        <div className="pl-2 col s6 right-align pr-3">
                                                            <button type="button" className="waves-effect waves-teal btn-small right-align" onClick={this.onClickReset}> {localConstant.login.RESET_PASSWORD}</button>
                                                        </div>        
                                                    </div> 
                                                </Fragment>
                                            }                                            
                                        </fieldset>
                                        <p className="center-align login-footer"> 2019 &copy; Intertek Group plc</p>
                                    </form>
                                }
                            </div>
                        </div>
                    </div>
                </div>
                {/* <Footer /> */}
            </Fragment>
        );
    }
}
export default ForgotPassword;