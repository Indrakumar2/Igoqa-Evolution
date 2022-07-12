import React, { Component, Fragment } from 'react';
import { Helmet } from 'react-helmet';
import Logo from '../../../assets/images/logo.png';
import MaterializeComponent from 'materialize-css';
import { AppDashBoardRoutes, AppMainRoutes } from '../../../routes/routesConfig';
import { getlocalizeData, isEmpty,ssoParseQueryParam,isEmptyOrUndefine } from '../../../utils/commonUtils';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';
import { Link } from 'react-router-dom';
import Modal from '../../../common/baseComponents/modal';
import Announcement from '../../../common/baseComponents/announcement/announcement';
import auth from '../../../authService';
const localConstant = getlocalizeData();

class AppLogin extends Component {

    constructor(props) {
        super(props);
        this.state = {
            passwordToggle: false,
            userName: '',
            userPassword: '',
            userNameError: false,
            userPasswordError: false,
            isSucess:false,
            forgotPasswordBtn: false,
            onValidatePassword: true,
            isSecurityQuestionExists: false,
            onSubmitPassword: false,
            userAnswer: '',
            isAnnoncementOpen: false,
        };

        this.annoncementButtons=[
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.annoncementCloseClick,
                btnID: "closeAnnouncementList",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
            },
        ];
    };
    componentDidMount() {
        this.props.actions.FetchAnnoncements();
    }
    componentWillMount() {
        const { loginStatus, history } = this.props;
        if (loginStatus) {
            history.push(AppDashBoardRoutes.assignments);
            // this.props.actions.FetchDashboardCount();
        }
        const result=ssoParseQueryParam(this.props.location.search);
        if(!isEmpty(result) && result.sso == "true"){
             const access_token =  auth.getAccessToken();
             const refresh_token = auth.getRefreshToken();
             if(isEmptyOrUndefine(access_token) && isEmptyOrUndefine(refresh_token)) {
                const loginDetails = {
                    Username: result.name,
                    Password: result.password
                };
                this.props.actions.AuthenticateLogin(loginDetails).then(response => {                      
                    this.setState({ isSucess:!response });
                    if (response) {
                        this.props.actions.UserMenu().then(x => {
                            if(result.navigateto == "myProfile"){
                                this.props.history.push({
                                    pathname: AppMainRoutes.profileDetails,
                                    state: {
                                        isFrom: "sso"
                                    }
                                });
                            } else if(result.navigateto == "calendar"){
                                this.props.history.push(AppMainRoutes.myCalendar);
                            }
                        });
                    }
                });
            } else {
                if(result.navigateto == "myProfile"){
                    this.props.history.push({
                        pathname: AppMainRoutes.profileDetails,
                        state: { isFrom: "sso" }
                    });
                } else if(result.navigateto == "calendar"){
                    this.props.history.push(AppMainRoutes.myCalendar);
                }
            }
        }
    }

    passwordToggleHandler = (event) => {
        this.setState({ passwordToggle: !this.state.passwordToggle });
    }

    handleCredential = (e) => {
        this.updatedData={};
        this.updatedData[e.target.name]=e.target.value;
        if(!isEmpty(this.updatedData.userName)){
          this.props.actions.loginUserName(this.updatedData.userName);
        }
        this.setState({
            [e.target.name]: e.target.value
        });
        
    }

    AuthenticateUser = (e) => {
        e.preventDefault();
        const loginData = {
            Username: this.state.userName,
            Password: this.state.userPassword
        };
      
        if (isEmptyOrUndefine(this.state.userName)) {
            IntertekToaster(localConstant.login.VALID_USER, 'warningToast invalidUser');

        } else if (isEmptyOrUndefine(this.state.userPassword)) {
            IntertekToaster(localConstant.login.VALID_PASSWORD, 'warningToast invalidPassword');
        }
        else {
            if (!isEmptyOrUndefine(loginData.Username) && !isEmptyOrUndefine(loginData.Password)) {
                this.setState({ isSucess:!this.state.isSucess });
                this.props.actions.AuthenticateLogin(loginData) 
                    .then(result => {                      
                        this.setState({ isSucess:!result });
                        if (result) {
                            this.props.actions.UserMenu().then(x => {
                                this.props.actions.getViewAllRightsCompanies();
                                this.props.history.push(AppDashBoardRoutes.assignments);
                                // this.props.history.push(AppMainRoutes.dashboard);
                                //this.props.actions.FetchDashboardCount();
                               
                            });
                        }
                    });
            }
        }
    }

    //need to be changed in 2nd phase(Hard coded now)
    announcementColourPicker = (colourCode) => {
        switch (colourCode) {
            case "-16777216":
                return "black";
            case "-16776961":
                return "blue";
            case "-16744448":
                return "green";
            case "-65536":
                return "red";
            case "-256":
                return "yellow";
            case "-32768":
                return "orange";
            case "-1":
                return "white";
            default:
                return "green";
        }
    }

    render() {
        const { annoncementData } = this.props;
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

                                <form className="col s12" onSubmit={this.AuthenticateUser}>
                                    <fieldset className="login-fieldset" >
                                        <legend className="login-legend pl-3 pr-3"><b>{localConstant.login.LOGIN_WELCOME_MESSAGE} </b></legend>
                                        <p className="pl-3 pt-2 pb-2"> {"Please enter your Username and Password to login "}</p>
                                        <div className="row">
                                            <div className="input-field col s12 mb-1 mt-1 pl-4 pr-4">
                                                <i className="tiny material-icons prefix customIcon zmdi zmdi-account-circle left"></i>
                                                <input type="text" name="userName" placeHolder="Username" onChange={this.handleCredential} className={this.state.userNameError ? "validate  invalid" : "validate "} id="username" />
                                                <span className="helper-text" data-error={localConstant.validationMessage.INVALIED_USER_NAME}></span>
                                            </div>
                                            <div className="input-field col s12 mb-1 mt-1 pl-4 pr-4">
                                                <i className="tiny material-icons prefix customIcon zmdi zmdi-key left"> </i>
                                                <input name="userPassword" maxLength={30} placeHolder="Password" onChange={this.handleCredential} type={this.state.passwordToggle ? 'text' : 'password'} className={this.state.userPasswordError ? "validate  invalid" : "validate "} id="password" />
                                                {/* <Link to={{ pathname: "/ForgotPassword" }} className="link login-anchor pr-0" >{localConstant.login.FORGET_PASSWORD}</Link>*/}
                                                {/* Commented for defect-1200 */}
                                                <span className="helper-text" data-error={localConstant.validationMessage.INVALIED_PASSWORD}></span>
                                            </div>

                                            <div className="col s6 offset-s6 right-align ">
                                                <button type="submit" className="waves-effect waves-light btn">
                                                    <i className="material-icons right pt-1"><img alt="Login" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAQAAABKfvVzAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAAAmJLR0QAAKqNIzIAAAAJcEhZcwAADdcAAA3XAUIom3gAAAAHdElNRQfiCAYQLSCFId1jAAAAc0lEQVQ4y9WTvQmAMBSELyK4RJC4hCDEnyyqq9g6g26h1mcTsTRXiV99H9zBe8DfYMVCiTckV9p0wfFUlaArAw9V6XWl434rhgYlzKvTYkSODQGcqbBkyb0iqZU8plgpabR/Rv8ubqU4wFo8CYBOeqBvuACTVZdMnX78HgAAACV0RVh0ZGF0ZTpjcmVhdGUAMjAxOC0wOC0wNlQxNjo0NTozMiswMjowMEEJVYIAAAAldEVYdGRhdGU6bW9kaWZ5ADIwMTgtMDgtMDZUMTY6NDU6MzIrMDI6MDAwVO0+AAAAGXRFWHRTb2Z0d2FyZQB3d3cuaW5rc2NhcGUub3Jnm+48GgAAAABJRU5ErkJggg==" /></i>
                                                    {localConstant.login.BTN_LOGIN}
                                                </button>
                                            </div>

                                        </div>
                                    </fieldset>
                                    <p className="center-align login-footer"> 2019 &copy; Intertek Group plc</p>
                                </form>
                                {annoncementData && annoncementData.length > 0 ?
                                        <Announcement annoncementData={annoncementData} className='login-announcement mt-1  annoncementTextWidth annoncementLineHeight'/>
                                    : null
                                }
                            </div>

                        </div>

                    </div>
                    
                </div>
            </Fragment>
        );
    }
}

export default AppLogin;