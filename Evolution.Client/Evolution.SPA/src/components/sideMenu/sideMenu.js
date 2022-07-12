import React, { Component, Fragment } from 'react';
import MaterializeComponent from 'materialize-css';
import { NavLink } from 'react-router-dom';
import { sideMenu } from './sideMenuData.js';
import collapsedLogo from '../../assets/images/collapsedLogo.png';
import Logo from '../../assets/images/logo.png';
import Modal from '../../common/baseComponents/modal';
import authService from '../../authService';
import store from '../../store/reduxStore';
import { CheckForUnsavedChanges,CheckForUnSavedDocChanges } from '../../common/selector';
import { getlocalizeData,isFunction,isEmpty,isUndefined } from '../../utils/commonUtils';
import objectUtil from '../../utils/objectUtil';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { modalTitleConstant } from '../../constants/modalConstants.js';
import SupplierVisitPerformanceReportModal from '../viewComponents/supplier/supplierVisitPerformanceReportModal';
import UnApprovedVisitModal from '../viewComponents/visitReports/unapprovedVisitTimesheetReports';
import ApprovedVisitModal from '../viewComponents/visitReports/approvedVisitTimesheetReports';
import VisitKPIReportModal from '../viewComponents/visitReports/visitTimesheetKPIReport';
import { RemoveDocumentsFromDB } from '../../common/commonAction';
 const localConstant = getlocalizeData();

class AppSideMenu extends Component {

    constructor(props) {
        super(props);
        this.state = {
            unsavedChangesPopup: false,
            menuList: {},
            showSupplierVisitModal:false,
            showUnapprovedVisitTimesheetModal:false,
            showApprovedVisitTimesheetModal:false,
            showVisitTimesheetKPIReportModal:false,
            title:'',           
        };
    }

    confirmationObject = {
        title: "confirmation...",
        message: "Are you sure you want to exit",
        type: "confirm",
        modalClassName: "warningToast",
        buttons: [
            {
                buttonName: "Yes",
                onClickHandler: this.deleteSelectedPayroll,
                className: "modal-close m-1 btn-small"
            },
            {
                buttonName: "No",
                onClickHandler: this.confirmationRejectHandler,
                className: "modal-close m-1 btn-small"
            }
        ]
    };

    currentMenuObj = {};
    state = {
        unsavedChangesPopup: false,
        menuList: {}
    };

    confirmationRejectHandler = (e) => {
        e.preventDefault();
        this.props.actions.HideModal();
    }

    componentDidMount() {      
        const sidnav = document.querySelectorAll('.sidebar');
        const sidnavInstances = MaterializeComponent.Sidenav.init(sidnav);
        const collapse = document.querySelectorAll('.collapsible');
        const collapseInstances = MaterializeComponent.Collapsible.init(collapse,{
            accordion: true
          });
        const tooltips = document.querySelectorAll('.tooltipped');
        const tooltipInstances = MaterializeComponent.Tooltip.init(tooltips);
        //MaterializeComponent.Collapsible.init(collapse);
    }
    //D684
    confirmPopupForProfile = ()=>{
            const confirmationObject = {
                title: modalTitleConstant.ALERT_WARNING,
                message: localConstant.techSpecValidationMessage.TAXONOMY_HAS_ISACCEPTREQUIRED,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.OK,                
                        onClickHandler: this.closeWarningPopup,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
    }
    visitPerformanceReport=(e)=>
    {
           this.setState({
                    showSupplierVisitModal: true
                });
    }
    UnapprovedVisitCHC=()=>
    {
        this.setState({
            showUnapprovedVisitTimesheetModal: true,
            title: localConstant.visit.UNAPPROVED_VISITS_REPORT_CHC,
        });
    }

    UnapprovedTimesheetCHC=()=>
    {
        this.setState({
            showUnapprovedVisitTimesheetModal:true,
            title: localConstant.timesheet.UNAPPROVED_TIMESHEET_REPORT_CHC,
        });
    }
    UnapprovedVisitOC=()=>
    {
    this.setState({
        showUnapprovedVisitTimesheetModal: true,
        title:localConstant.visit.UNAPPROVED_VISITS_REPORT_OC,
        
    });
}
UnapprovedTimesheetOC=()=>
{
this.setState({
    showUnapprovedVisitTimesheetModal: true,
    title:localConstant.timesheet.UNAPPROVED_TIMESHEET_REPORT_OC,

});
}

ApprovedVisitProforma=()=>
    {
     this.setState({
        showApprovedVisitTimesheetModal:true,
         title:localConstant.visit.APPROVED_VISITS_REPORT_PROFORMA

           });
        
    }
ApprovedTimesheetProforma=()=>{
    this.setState({
        showApprovedVisitTimesheetModal:true,
         title:localConstant.timesheet.APPROVED_TIMESHEET_REPORT_PROFORMA
    });
}
    ApprovedVisitProformaNDT=()=>
    { 
        this.setState(
        {
            showApprovedVisitTimesheetModal:true,
        title:localConstant.visit.APPROVED_VISITS_PROFORMA_NDT,
        }
    );}

    ApprovedTimesheetProformaNDT=()=>
    { 
        this.setState(
        {
       showApprovedVisitTimesheetModal:true,
        title:localConstant.timesheet.APPROVED_TIMESHEET_REPORT_NDT,
        }
    );}

    VisitKPIReportCHC=()=>
    {
        this.setState(
            {
                showVisitTimesheetKPIReportModal:true,
                title:localConstant.visit.VISITS_KPI_REPORT_CHC,
            }
        );
    }
    TimesheetKPIReportCHC=()=>
    {
        this.setState(
            {
                showVisitTimesheetKPIReportModal:true,
                title:localConstant.timesheet.TIMESHEET_KPI_REPORT_CHC,
            }
        );
    }
    VisitKPIReportOC=()=>
    {
        this.setState(
            {
                showVisitTimesheetKPIReportModal:true,
                title: localConstant.visit.VISITS_KPI_REPORT_OC,
            }
        );
    }
    TimesheetKPIReportOC=()=>
    {
        this.setState(
            {
                showVisitTimesheetKPIReportModal:true,
                title: localConstant.timesheet.TIMESHEET_KPI_REPORT_OC,
            }
        );
    }
    
        closeWarningPopup = (e)=>{
            e.preventDefault();
        this.props.actions.HideModal();
        }

    MenuClick = (e, menuAction, currentModule) => {
        e.preventDefault();
        let functionRef = null;
        const state = store.getState();
        const moduleHasChanges = CheckForUnsavedChanges(state);
        this.currentMenuObj = { ...menuAction, currentModule };
        if (isEmpty(menuAction.modalPopupCallback) && moduleHasChanges === false) {

            this.setState((state) => ({
                unsavedChangesPopup: true
            }));
        
        } else {
            if(menuAction.viewUrl === "mySchedule"){
                IntertekToaster("Component Under Construction", 'warningToast ');
            }
            else if(menuAction.viewUrl === "myAssignments"){
                IntertekToaster("Component Under Construction", 'warningToast ');
            } 
            else if(menuAction.viewUrl === "myDocuments"){
                IntertekToaster("Component Under Construction", 'warningToast');
            }
            else {
                if(menuAction.isReportMenu && menuAction.isOpenModalPopup){
                    if(menuAction.modalPopupCallback && isFunction(this[menuAction.modalPopupCallback])){
                        this[menuAction.modalPopupCallback]();
                    }   
                }
                else{
                this.props.history.push(`/${ menuAction.viewUrl }`);
                if(menuAction.modalPopupCallback && isFunction(this[menuAction.modalPopupCallback])){
                    functionRef = this[menuAction.modalPopupCallback];
                }
                menuAction.menuFun && this.props.actions[menuAction.menuFun](this.currentMenuObj,functionRef);
            }
                
            }
          
        }
    }
    
    // notifyUnsavedChanges
    notifyUnsavedChanges = () => {
        const { viewUrl, menuFun } = this.currentMenuObj;
        const state = store.getState();
        const ObjDoc = CheckForUnSavedDocChanges(state);        
        if(!isEmpty(ObjDoc.documentData) && !isUndefined(ObjDoc.moduleRefCode)){
         RemoveDocumentsFromDB(ObjDoc.documentData, ObjDoc.deleteUrl);                                      
        }
        this.props.history.push(`/${ viewUrl }`);
        menuFun && this.props.actions[menuFun](this.currentMenuObj);
    }

    hideModal = () => {
        this.setState((state) => ({
            unsavedChangesPopup: false,
            showSupplierVisitModal: false,
            showUnapprovedVisitTimesheetModal:false,
            showApprovedVisitTimesheetModal:false,
            showVisitTimesheetKPIReportModal:false,
        }));
    }

    filterMenus = () => {
        const sideMenus = sideMenu;
        const availableMenus = authService.getUserMenu();
        const clonedMenus = objectUtil.cloneDeep(sideMenus);
        const menuToBeRender = [];

        if (availableMenus!= undefined && availableMenus.length > 0) {
            clonedMenus.map(cm => {
                let filterMenu = availableMenus.filter(function (fm) {
                    return fm.module == cm.module && fm.menuName == cm.name;
                });
                if (filterMenu.length > 0)
                    filterMenu = cm;
                else
                    filterMenu = null;

                if (cm.subMenu != undefined && cm.subMenu.length > 0) {
                    const subMenus = [];
                    cm.subMenu.map(cm1 => {
                        const filterSubMenu = availableMenus.filter(function (fm) {
                            return fm.module == cm1.module && fm.menuName == cm1.name;
                        });
                        if (filterSubMenu.length > 0)
                            subMenus.push(cm1);
                    });
                    if (filterMenu == null && subMenus.length > 0) {
                        filterMenu = cm;
                    }

                    if (subMenus.length > 0) {
                        filterMenu.subMenu = subMenus;
                    }
                }

                if (filterMenu != null)
                    menuToBeRender.push(filterMenu);
            });
        }
        return menuToBeRender;
    }
    menuHandleClick(){

    }

    render() {
       const { isSideBarStatus } = this.props;
      //const sideMenus = sideMenu;
      const sideMenus = this.filterMenus();//this.props.userMenu);
        return (
            <Fragment>
                 {this.state.showSupplierVisitModal?
         <SupplierVisitPerformanceReportModal showmodal={ this.state.showSupplierVisitModal} hidemodal={this.hideModal} />  
          :null}  

     {this.state.showUnapprovedVisitTimesheetModal? 
        <UnApprovedVisitModal showmodal={ this.state.showUnapprovedVisitTimesheetModal} hidemodal={this.hideModal} title={this.state.title} /> 
     :null }

      { this.state.showApprovedVisitTimesheetModal?
      <ApprovedVisitModal showmodal={this.state.showApprovedVisitTimesheetModal} hidemodal={this.hideModal}  title={this.state.title}/>
            :null
            }

      {this.state.showVisitTimesheetKPIReportModal?
      <VisitKPIReportModal showmodal={this.state.showVisitTimesheetKPIReportModal} hidemodal={this.hideModal} title={this.state.title}/>
      : null }
                <Modal id="unsavedChanges"
                    title={'Unsaved Changes'}
                    modalClass="bold"
                    buttons={[
                        {
                            name: 'No', action: this.hideModal,
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
                
            <div className={isSideBarStatus ? 'sidebar ' :' sidebar sideBarOff ' } >

            <div className="sidebar-wrapper">
                <div className="logo">
                <NavLink to={'/Dashboard/ActiveAssignments'}
                        className= {isSideBarStatus ? 'collapsedLogo p-0' : 'expandedLogo animated fadeIn'}
                        title={"Home"}
                        onClick={(e) => this.MenuClick(e, {
                            "menuText": localConstant.sideMenu.DASHBOARD,
                            "viewUrl": "Dashboard/ActiveAssignments",
                            "menuFun": 'HandleMenuAction',
                            "currentPage": localConstant.sideMenu.DASHBOARD
                        }, "dashboard")}
                    >
                     {isSideBarStatus ? <img src={collapsedLogo} width="50" alt="Intertek Logo" /> :  <img src={Logo} width='250' alt="Intertek Logo" /> }
                       
                    </NavLink> 
                </div>

                <div className= {isSideBarStatus ? "sideNav-vertical" : "sideNavContent animated fadeIn"}>
                        {
                            sideMenus.map((iteratedMenu, i) => {
                                const currentModule = iteratedMenu.module;
                                return (
                                    <ul className= "collapsible collapsible-accordion animated fadeIn">
                                    <li key={i}>
                                        {
                                            iteratedMenu.subMenu.length > 0 ?
                                                <a className={isSideBarStatus ? "greyBorder":"collapsible-header" } onClick={()=>this.menuHandleClick()}>
                                                    <img alt={iteratedMenu.viewUrl} src={iteratedMenu.menuIcon} />
                                                     {!isSideBarStatus && <span>{iteratedMenu.menuText}</span>}
                                                </a>
                                                :
                                                <NavLink to={'/' + iteratedMenu.viewUrl}
                                                    data-position="right"
                                                    data-tooltip={iteratedMenu.subMenu.length === 0 ? iteratedMenu.menuText : null}
                                                    key={i}
                                                    className={iteratedMenu.subMenu.length === 0 ? 'tooltipped greyBorder' : 'greyBorder'}
                                                    activeClassName='activeMenu'>
                                                    <img alt={iteratedMenu.viewUrl} src={iteratedMenu.menuIcon} />
                                                </NavLink>
                                        }
                                        
                                        {                                     
                                                iteratedMenu.subMenu.length > 0 && !isSideBarStatus ?
                                                 <div className="collapsible-body">
                                                <ul>
                                                {
                                                    iteratedMenu.subMenu.length > 0 && iteratedMenu.subMenu.map((subMenu, j) => {
                                                        return (
                                                            <li key={j}>
                                                                <NavLink to={'/' + subMenu.viewUrl}
                                                                    key={i}
                                                                    className='greyBorder'
                                                                    onClick={(e) => this.MenuClick(e, subMenu, currentModule)}>
                                                                    {subMenu.menuText}
                                                                </NavLink>
                                                            </li>
                                                        );
                                                    })
                                                }
                                                </ul>
                                                </div>
                                                :
                                                <ul className="sub-menu">
                                                {
                                                    iteratedMenu.subMenu.length > 0 && iteratedMenu.subMenu.map((subMenu, j) => {
                                                        return (
                                                            <li key={j}>
                                                                <NavLink to={'/' + subMenu.viewUrl}
                                                                    key={i}
                                                                    className='greyBorder'
                                                                    onClick={(e) => this.MenuClick(e, subMenu, currentModule)}>
                                                                    {subMenu.menuText}
                                                                </NavLink>
                                                            </li>
                                                        );
                                                    })
                                                }
                                                </ul>
                                        }
                                
                                    </li>
                                    </ul>
                                );
                            })
                        }
                    </div>
            </div>
            
        </div>

      </Fragment>
        );
    }
}

export default AppSideMenu;