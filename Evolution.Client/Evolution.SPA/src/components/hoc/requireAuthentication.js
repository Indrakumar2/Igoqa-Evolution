import React, { Component, Fragment } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import authService from '../../authService';
import { bindActionCreators } from 'redux';
import { AppMainRoutes } from '../../routes/routesConfig';
import { handleLogOut, RefreshToken } from '../../actions/login/loginAction';
import { FetchUserPermission, FetchUserPermissionsData } from '../appLayout/appLayoutActions';
import AppLayout from '../appLayout';
import { equal, getlocalizeData, isUndefined, isEmptyReturnDefault, parseQueryParam, isEmptyOrUndefine } from '../../utils/commonUtils';
import { CheckPermission,moduleViewAllRights, moduleViewAllRights_Modified } from '../../utils/permissionUtil';
import Title from '../../common/baseComponents/pageTitle';
import { applicationConstants } from '../../constants/appConstants';
import { securitymodule, interCompany_viewActivitiesLevel0 } from '../../constants/securityConstant';
import { Prompt } from 'react-router-dom';
import TextLoader from '../../common/baseComponents/textLoader';
import { isArray } from 'util';
export default function (ComposedComponent, pageTitle, permissionCodes, moduleName, renderAppLayout) {
    const localConstant = getlocalizeData();
    class Authentication extends Component {
        constructor(props) {
            super(props);
            this.state = {
                renderAppLayout: renderAppLayout ? renderAppLayout : this.props.renderAppLayout
            };
            this.userTypes = isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE));
        }
        componentWillMount() {
            // console.log("RequireAuthentication - componentWillMount : ");
            // console.log(this.props.serverError);
            this.HandleLogOutAndRedirect(this.props.serverError);
        }
        componentDidUpdate(prevProps) {
            // console.log("RequireAuthentication - componentDidUpdate : ");
            // console.log(this.props.serverError);
            const isEqual = equal(prevProps.serverError, this.props.serverError);
            if (!isEqual)
                this.HandleLogOutAndRedirect(this.props.serverError);
        }
        //To Check the User Permission
        CheckPermission() {  
            let isInterCompanyResourceFetch=false;// This is uesed in Resource fetch when epin link from  preassignment, ars ,quick search are clicked in inter company scenario.
            if (permissionCodes !== undefined && permissionCodes !== null && permissionCodes.length > 0) {
                this.setState({
                    isSecurityChecked: false
                });
                // calling an API for permission
                let compCode = this.props.companyCode;
                const userLogonName = this.props.logonUser;
                if(this.props.location && this.props.location.search) //Def 957 Fix
                {
                    const queryParam = parseQueryParam(this.props.location.search);
                    compCode= queryParam && queryParam.interCompanyCode ? queryParam.interCompanyCode : compCode; 
                    isInterCompanyResourceFetch= (compCode!==this.props.companyCode);
                } 
                let module = moduleName;
                if (isArray(module)) {
                    if (this.userTypes.includes(localConstant.techSpec.userTypes.TS)) {
                        module = securitymodule.MYPROFILE;
                    }
                    //Commented for D562 and D761
                    //else if (this.userTypes.includes(localConstant.techSpec.userTypes.RC) || this.userTypes.includes(localConstant.techSpec.userTypes.RM) || this.userTypes.includes(localConstant.techSpec.userTypes.TM)) {
                    else {
                        module = securitymodule.TECHSPECIALIST;
                    }
                }
                if (!isUndefined(userLogonName) && userLogonName !== null&&userLogonName !== "" && isInterCompanyResourceFetch ==false) {
                    this.props.actions.FetchUserPermission(userLogonName, compCode, module)
                        .then(res => {
                            if (res.code === "1" && !isEmptyOrUndefine(res.result)) {
                                const isViewAllRights = this.props.viewAllRightsCompanies.length > 0; 
                                    this.props.actions.FetchUserPermissionsData(res.result); 
                                    if (!CheckPermission(permissionCodes, res.result)){
                                        if(moduleViewAllRights_Modified(module, viewAllRightsCompanies)){
                                        // if(moduleViewAllRights(module,isViewAllRights)){
                                            this.setState({
                                                isSecurityChecked: true
                                            });
                                        } 
                                        this.props.history.push(AppMainRoutes.forbidden);
                                    }
                                    else
                                        this.setState({
                                            isSecurityChecked: true
                                        });
                            } 
                            else if(isInterCompanyResourceFetch && module == securitymodule.TECHSPECIALIST)//def 957 fix
                            {
                                this.props.actions.FetchUserPermissionsData(interCompany_viewActivitiesLevel0.activities); 
                                this.setState({
                                    isSecurityChecked: true
                                });
                            }
                            else
                            {
                                this.props.history.push(AppMainRoutes.forbidden);
                            }
                        }).catch(error => {
                            // console.error(error); // To show the error details in console
                            this.props.history.push(AppMainRoutes.forbidden);
                        });
                }
                else if(isInterCompanyResourceFetch && module == securitymodule.TECHSPECIALIST)//def 957 fix
                            { 
                                this.props.actions.FetchUserPermissionsData(interCompany_viewActivitiesLevel0.activities); 
                                this.setState({
                                    isSecurityChecked: true
                                });
                            }
            }
            else {
                this.setState({
                    isSecurityChecked: true
                });
            }
        }
        HandleLogOutAndRedirect(serverError) {
            if (serverError) {
                // console.log("HandleLogOutAndRedirect : ");
                // console.log(serverError);
                if (serverError.status === 403) {
                    authService.removeForbidden();
                    this.props.history.push(AppMainRoutes.forbidden);
                }
                else {
                this.props.actions.HandleLogOut();
                this.props.history.push({
                    pathname: AppMainRoutes.error,
                    state: {
                        status: serverError.status
                    }
                });
                }
            }
            else if (authService.isRefreshTokenExpired()) {
                // console.log("authService.isRefreshTokenExpired(true) : ");
                    const refreshToken = authService.getRefreshToken();
                    const checkUserDetailsInSession = authService.checkUserDetailsInSession();
                    if (checkUserDetailsInSession && refreshToken) {
                        this.props.actions.HandleLogOut(res => {
                            this.props.history.push(AppMainRoutes.login);
                        });
                    } else {
                        this.props.history.push(AppMainRoutes.login);
                    }
            }
            else {
                this.CheckPermission();
            }
        }
        render() {
            return (
                <Fragment>
                    {
                        this.state.renderAppLayout ?
                            <AppLayout>
                                <Title title={pageTitle} />
                                {
                                    this.state.isSecurityChecked ?
                                        <ComposedComponent {...this.props} /> :
                                        <TextLoader />
                                }
                            </AppLayout> :
                            <ComposedComponent {...this.props} />
                    }
                </Fragment>
            );
        }
    }
    const mapStateToProps = (state) => {
        return {
            isAuthenticated: authService.isAuthenticated(),
            serverError: state.loginReducer.serverError,
            companyCode: state.appLayoutReducer.selectedCompany,
            logonUser: state.appLayoutReducer.username,
            isSecurityChecked: false,
            viewAllRightsCompanies: isEmptyReturnDefault(state.masterDataReducer.viewAllRightsCompanies),
            // activities:state.appLayoutReducer.activities
        };
    };
    const mapDispatchToProps = dispatch => {
        return {
            actions: bindActionCreators(
                {
                    HandleLogOut: handleLogOut,
                    RefreshToken: RefreshToken,
                    FetchUserPermission: FetchUserPermission,
                    FetchUserPermissionsData: FetchUserPermissionsData,
                    // getViewAllRightsCompanies: getViewAllRightsCompanies
                },
                dispatch
            ),
        };
    };
    Authentication.propTypes = {
        isAuthenticated: PropTypes.bool,
        renderAppLayout: PropTypes.bool,
        activities: []
    };
    Authentication.defaultProps = {
        isAuthenticated: false,
        renderAppLayout: true,
    };
    return connect(
        mapStateToProps,
        mapDispatchToProps
    )(Authentication);
};