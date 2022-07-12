import React, { Component, Fragment } from 'react';
import CompanyList from '../companyList';
import MaterializeComponent from 'materialize-css';
import AboutEvolution from '../aboutEvolution';
import { getlocalizeData, isEmptyOrUndefine, isEmpty, isUndefined } from '../../utils/commonUtils';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { CheckForUnsavedChanges, CheckForUnSavedDocChanges } from '../../common/selector';
import Announcement from '../../common/baseComponents/announcement/announcement';
import store from '../../store/reduxStore';
import Modal from '../../common/baseComponents/modal';
import authService from '../../authService';
import { modalMessageConstant, modalTitleConstant } from '../../constants/modalConstants';
import { applicationConstants } from '../../constants/appConstants';
import { configuration } from '../../appConfig';
import {
    AppMainRoutes
} from '../../routes/routesConfig';
import { RemoveDocumentsFromDB } from '../../common/commonAction';
import moment from 'moment';

const localConstant = getlocalizeData();
export const IdleFlag = (props) => {
    return (
        <>
            <div className={props.idleAutoFlag ? "modal show customModal" : ""} style={props.idleAutoFlag ? { display: 'block', maxWidth: 400 } : { display: 'none' }}>
                <div id="confirmation_Modal" className="confimationModal">
                    <div className="modal-content p-0">
                        <div className={'warningToast pl-2 mb-2'}>
                            <h5 className="bold">{modalTitleConstant.CONFIRMATION}</h5>
                        </div>
                        <p className="p-2 confimationBodyText">{modalMessageConstant.CONFIRMATION_SESSION}</p>
                    </div>
                    <div className="modal-footer">
                        <button className='m-1 btn-small' onClick={props.onHandleClickYes}>{'Yes'}</button>
                        <button className='m-1 btn-small' onClick={props.onHandleClickNo}>{'No'}</button>
                    </div>
                </div>
            </div>
            {props.idleAutoFlag && <div className="customModalOverlay"></div>}
        </>
    );
};
class AppHeader extends Component {
    constructor(props) {
        super(props);
        this.state = {
            unsavedChangesPopup: false,
            announcementModal: false,
            idleAutoFlag: false,
            isAnnoncementOpen: false,
            announcementDesc: "Test"
        };
        this.unsavedChangesPopup = false;

        this.IDLE_COUNT = configuration.idleTimeOut; //Idle Waiting Time in Seconds
        this.AUTO_CLOSE_WAITING_TIME = configuration.idleWaitingTime; // autoClose Waiting time in milliseconds 
        //Browser Idle 
        this.events = [
            "load",
            "click",
            "mousemove",
            "mousedown",
            "scroll",
            "keypress"
        ];
        this.events.forEach(eventName => {
            window.addEventListener(eventName, this.resetIdleTime, false);
        });
        document.addEventListener("visibilitychange", this.browserTabActive, true);
        this.clearTimer();
        this.startTimer();
    }
    componentDidMount() {
        const dropdown = document.querySelectorAll('.dropdown-trigger');
        const dropdownInstance = MaterializeComponent.Dropdown.init(dropdown, { coverTrigger: false });
        const toolTip = document.querySelectorAll('.tooltipped');
        const instancestollTip = MaterializeComponent.Tooltip.init(toolTip);
        const custModal = document.querySelectorAll('.modal');
        const custModalInstances = MaterializeComponent.Modal.init(custModal, { "dismissible": false });
        this.props.actions.FetchAllMasterData();
        //this.props.actions.FetchAnnoncements();
        if (sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE) == null)
        sessionStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE,   
        localStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_CODE));
        if (sessionStorage.getItem(applicationConstants.Authentication.COMPANY_CURRENCY) == null)
            sessionStorage.setItem(applicationConstants.Authentication.COMPANY_CURRENCY, 
            localStorage.getItem(applicationConstants.Authentication.COMPANY_CURRENCY));
        if (sessionStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_ID) == null)
            sessionStorage.setItem(applicationConstants.Authentication.DEFAULT_COMPANY_ID, 
            localStorage.getItem(applicationConstants.Authentication.DEFAULT_COMPANY_ID));
        if (sessionStorage.getItem(applicationConstants.Authentication.USER_MENU) == null)
        {
            this.props.actions.UserMenu().then(res=>{
                this.props.history.push(this.props.history.location.pathname);
            });
        }
    }
    
    hideModal = () => {
        this.setState((state) => ({
            unsavedChangesPopup: false
        }));
    }

    hideUnAvailableModal = () => {
        window.close();
    }

    hideAnnouncementModal = (e) => {
        e.preventDefault();
        this.setState((state) => ({
            announcementModal: false
        }));
    }
    showAnnouncementModal = (e) => {
        e.preventDefault();
        this.setState((state) => ({
            announcementModal: true
        }));
    }
    // notifyUnsavedChanges
    notifyUnsavedChanges = async () => {
        //ID-583 and 802 - file upload unsaved file Clear function 
        const state = store.getState();
        const ObjDoc = CheckForUnSavedDocChanges(state);
        if (!isEmpty(ObjDoc.documentData) && !isUndefined(ObjDoc.moduleRefCode)) {
            await RemoveDocumentsFromDB(ObjDoc.documentData, ObjDoc.deleteUrl);
        }
        this.handleLogOut();
    }
    logOutUser = () => {
        const state = store.getState();
        const moduleHasChanges = CheckForUnsavedChanges(state);
        if (moduleHasChanges === false) {
            this.setState((state) => ({
                unsavedChangesPopup: true
            }));
            this.unsavedChangesPopup = true;
            // if(window.confirm(localConstant.commonConstants.UNSAVED_DATA_VALIDATION))
            // {
            //     this.props.actions.handleLogOut().then(res=>{
            //         this.props.history.push({
            //             pathname: '/login',
            //             state: 'Logout'
            //           });
            //     });
            //     this.props.actions.ClearDashboardReducer();
            // }
        }
        else {
            this.handleLogOut();
        }
    }
    handleLogOut = () => {
        this.props.actions.handleLogOut().then(res => {
            this.props.history.push({
                pathname: '/Login',
                state: 'Logout'
            });
        });
        this.props.actions.ClearDashboardReducer();
    }
    about = () => {
        this.props.actions.FetchAboutInformation();
        this.props.actions.AboutShowModal();
    }
    onhandleClickOk = () => {
        IntertekToaster("Ok", 'successToast errorUnique2');
    }
    changePassword = () => {
        this.props.history({ pathname: '/changepassword' });
    }
    onhandleClickCancel = (e) => {
        e.preventDefault();
        this.props.actions.AboutHideModal();
    }

    refreshClick = (e) => {
        e.preventDefault();
        this.props.actions.ClearRefreshMasterData();
        this.props.actions.ReloadMasterData();
        this.props.actions.DashboardDocumentationDetails();
        //this.props.actions.FetchMarginType();
        this.props.actions.FetchCostSaleReference(true);
        this.props.actions.FetchAssignmentRefTypes();
        this.props.actions.FetchTaxesOnRefresh();
        this.props.actions.FetchGeneralDetailsMasterData();
        this.props.actions.FetchChargeTypes();
        this.props.actions.FetchSupplierPerformanceType();
        this.props.actions.FetchPayRollNameByCompany(this.props.selectedCompany);
        this.props.actions.FetchAnnoncements();
    }

    calendarClick = () => {
        let userTypesArray = [];
        if (this.props.userTypes) {
            userTypesArray = this.props.userTypes.split(',');
        }
        if (!userTypesArray.includes(localConstant.userTypeList.TechnicalSpecialist)) {
            // this.props.history.push({ pathname: AppMainRoutes.companyCalendar });
            window.open(AppMainRoutes.companyCalendar);
        }
        else {
            // this.props.history.push({ pathname: AppMainRoutes.myCalendar });
            window.open(AppMainRoutes.myCalendar, '_blank');
        }
    }
    clearTimer = () => {
        window.clearTimeout(this.autoClooseTime);
        window.clearInterval(this.waringTimeOut);
        localStorage.setItem(applicationConstants.Authentication.IDLE_START, new Date());
        localStorage.setItem(applicationConstants.Authentication.IDLE_FLAG, false);
    }
    resetIdleTime = () => {
        if (this.props.history.location.pathname !== AppMainRoutes.login) {
            if (this.props.location.pathname !== AppMainRoutes.error) {
                localStorage.setItem(applicationConstants.Authentication.IDLE_FLAG, false);
                this.clearTimer();
                this.startTimer();
            }
        }
    }
    startTimer = () => {
        if (this.props.history.location.pathname !== AppMainRoutes.login) {
            if (this.props.location.pathname !== AppMainRoutes.error) {
                const idleFlag = JSON.parse(localStorage.getItem(applicationConstants.Authentication.IDLE_FLAG));
                if (!(idleFlag)) {
                    this.waringTimeOut = window.setInterval(this.idleWaring, 1000);
                }
            }
        }
    }
    browserTabActive = () => {
        //if (!(document.hidden)) { //def 835 : logout all opend browser windows after idel waittime . 
        if (this.props.history.location.pathname !== AppMainRoutes.login) {
            if (this.props.location.pathname !== AppMainRoutes.error) {
                const idleFlag = JSON.parse(localStorage.getItem(applicationConstants.Authentication.IDLE_FLAG));
                if (idleFlag) {
                    // this.clearTimer();
                    this.setState({ idleAutoFlag: true });
                } else {
                    this.setState({ idleAutoFlag: false });
                    this.clearTimer();
                    this.startTimer();
                }
            }
        }
        // }else{
        //     this.clearTimer();
        // }
    }
    idleWaring = () => {
        if (isEmptyOrUndefine(localStorage.getItem(applicationConstants.Authentication.IDLE_START)) || isEmptyOrUndefine(localStorage.getItem(applicationConstants.Authentication.IDLE_FLAG))) {
            this.clearTimer();
            this.startTimer();
        }
        //console.log((moment(new Date()).diff(moment(localStorage.getItem(applicationConstants.Authentication.IDLE_START)),'seconds'))); 
        if ((moment(new Date()).diff(moment(localStorage.getItem(applicationConstants.Authentication.IDLE_START)), 'seconds')) >= this.IDLE_COUNT) {

            window.clearInterval(this.waringTimeOut);
            localStorage.setItem(applicationConstants.Authentication.IDLE_FLAG, true);
            this.setState({ idleAutoFlag: true });
            this.autoClooseTime = setTimeout(() => {
                const idleFlag = JSON.parse(localStorage.getItem(applicationConstants.Authentication.IDLE_FLAG));
                if (isEmptyOrUndefine(idleFlag) || idleFlag) {
                    localStorage.setItem(applicationConstants.Authentication.IDLE_FLAG, false);
                    this.setState({ idleAutoFlag: false });
                    this.clearTimer();
                    this.autoLogout();
                    // console.log("Auto Logged Out As Application Was Idel.");
                }
            }, this.AUTO_CLOSE_WAITING_TIME);
        }
    }
    onHandleClickYes = (e) => {
        localStorage.setItem(applicationConstants.Authentication.IDLE_FLAG, false);
        this.setState({ idleAutoFlag: false });
        if (authService.isRefreshTokenExpired()) {
            this.autoLogout();
        }
        this.clearTimer();
        this.startTimer();
    }
    onHandleClickNo = () => {
        localStorage.setItem(applicationConstants.Authentication.IDLE_FLAG, false);
        this.setState({ idleAutoFlag: false });
        this.clearTimer();
        this.autoLogout();
    }
    autoLogout = () => {
        this.props.actions.handleLogOut().then(res => {
            this.props.history.push(AppMainRoutes.login);
        });
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
        const { annoncementData, isDataAvailable, systemSettingData } = this.props;
        // const annoncementData = [
        //     { text: "OneOneOneOneOneOneOneOneOneOneOneOneOneOneOneOneOneOneOne",header:"Test Header1", textColour: "white", backgroundColour: "blue" },
        //     { text: "OneOneOneOneOneOneOneOneOneOneOneOneOneOneOneOneOneOneOne",header:"Test Header2", textColour: "white", backgroundColour: "Red" },
        //     { text: "OneOneOneOneOneOneOneOneOneOneOneOneOneOneOneOneOneOneOne",header:"Test Header3", textColour: "black", backgroundColour: "green" }
        // ];
        return (
            <Fragment>

                <ul id="userActions" className="dropdown-content">
                    {/* <li>
                    <a className="link" href='javascript:void(0)' onClick={this.changePassword}>{localConstant.login.CHANGE_PASSWORD}</a>
                    </li>                 */}
                    <li>
                        <a className="link" href='javascript:void(0)' onClick={this.logOutUser}>{localConstant.login.LOG_OUT}</a>
                    </li>

                </ul>
                <ul id="notifications" className="dropdown-content notifications">
                    {/* <li>
                        <a className="link" href='javascript:void(0)'>{'My Task'}</a>
                    </li> */}
                    {/* {!isEmptyOrUndefine(annoncementData) && annoncementData.length>0? <li> <a className="link" href='javascript:void(0)'                        
                        onClick={this.showAnnouncementModal}>{'Announcement'}</a>
                    </li>:null} */}

                </ul>
                <nav className={this.props.isSideBarStatus ? 'customNav header-panel' : 'customNav headerHaff-wdith'}>
                    <div className="nav-wrapper">
                        <a className="menuIcon" onClick={this.props.panelClick}><i className="zmdi zmdi-menu"></i></a>
                        {annoncementData && annoncementData.length > 0 ?
                            <Announcement annoncementData={annoncementData} className="left navMenus mt-1  annoncementTextWidth annoncementLineHeight" />
                            : null
                        }
                        <ul className="right navMenus mt-1">
                            <li className="btn-small bold" onClick={this.refreshClick} title={localConstant.commonConstants.REFRESH_LOOKUP}><i className="zmdi zmdi-refresh zmdi-hc-lg header_refresh"></i></li>

                            <li className='hide-on-med-and-down'>
                                <a className="bold waves-effect link modal-trigger" onClick={this.about}>{localConstant.header.ABOUT_EVOLUTION}</a>
                            </li>
                            <li className='hide-on-med-and-down'>
                                <span className="bold pr-3">{`${ localConstant.header.COMPANY } : `}</span>
                            </li>
                            <li className='hide-on-med-and-down pt-1'>
                                <CompanyList selectedCompany={this.props.selectedCompany} />
                            </li>

                            {/* Calendar CR - Request #1*/}
                            <li className="p-relative" data-position="bottom" title={localConstant.globalCalendar.COMPANY_CALENDAR}><span className="calenderIcon" onClick={this.calendarClick}><i className="zmdi  zmdi-calendar-note"></i></span></li>
                            {/* <li className="p-relative" data-position="bottom" title={"Notifications"}><a href="javascript:void(0)" className="dropdown-trigger" data-target="notifications"><i className="zmdi zmdi-notifications">
                                </i><span className={annoncementData && annoncementData.length > 0 ? "badge notificationbadge active" : "badge notificationbadge" }>{annoncementData && annoncementData.length>0?annoncementData.length:0}</span></a></li> */}
                            <li className='hide-on-med-and-down userAccountBtn'>
                                <a href="javascript:void(0)" className="white-text dropdown-trigger left" data-target="userActions">
                                    <i className="zmdi zmdi-account-circle left"></i>
                                    {this.props.loginUser}<i className="zmdi zmdi-chevron-down right">
                                    </i>
                                </a>
                            </li>
                        </ul>
                    </div>
                </nav>
                <Modal modalClass="aboutModal handle" buttons={[ { name: 'Cancel', btnClass: 'modal-close waves-effect waves-teal btn-small mr-2', action: this.onhandleClickCancel, showbtn: true } ]} isShowModal={this.props.showModal}>
                    <AboutEvolution systemSettingData={systemSettingData} />
                </Modal>
                {/* <Modal modalClass="announcementModal" 
                title={!isEmptyOrUndefine(annoncementData) && annoncementData.length>0 ? "Announcement: " + annoncementData[0].header:'Announcement'}
                buttons={[ { name: 'ok', btnClass: 'btn-small mr-1',
                 action: this.hideAnnouncementModal, showbtn: true } ]} 
                 isShowModal={this.state.announcementModal}>
                   {this.state.announcementModal? <Announcement 
                     annoncementData={annoncementData}  />:null}
                </Modal> */}
                <Modal id="unsavedChanges"
                    title={'Unsaved Changes'}
                    modalClass="bold"
                    buttons={[
                        {
                            name: 'Cancel', action: this.hideModal,
                            type: "button",
                            btnClass: 'btn-small mr-2',
                            showbtn: true
                        },
                        {
                            name: 'Yes',
                            type: "button",
                            action: this.notifyUnsavedChanges,
                            btnClass: 'btn-small',
                            showbtn: true
                        }
                    ]}
                    isShowModal={this.state.unsavedChangesPopup}>
                    <p>{localConstant.commonConstants.UNSAVED_DATA_VALIDATION}</p>
                </Modal>

                <Modal id="unAvailableData"
                    title={'Warning'}
                    modalClass="unAvailableDataModal"
                    modalId="unAvailableDataPopup"
                    formId="unAvailableDataForm"
                    buttons={[
                        {
                            name: 'Close',
                            action: this.hideUnAvailableModal,
                            type: "button",
                            btnClass: 'btn-small mr-2',
                            showbtn: true
                        },
                    ]}
                    isShowModal={isDataAvailable}>
                    <p>{localConstant.commonConstants.REQUEST_PAGE_UNAVAILABLE}</p>
                </Modal>

                {<IdleFlag idleAutoFlag={this.state.idleAutoFlag} onHandleClickYes={this.onHandleClickYes} onHandleClickNo={this.onHandleClickNo}></IdleFlag>}
            </Fragment>
        );
    }
}

export default AppHeader;