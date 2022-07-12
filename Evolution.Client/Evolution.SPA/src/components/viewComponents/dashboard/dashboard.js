import React, { Component, Fragment } from 'react';
import TabHeader from '../../../common/baseComponents/tabHeader';
import { DashboardRoutes } from '../../../routes/mainRoutes';
import { dashboardMenu } from './dashTabDetails.js';
import Menu from '../../../common/baseComponents/menu';
import LoaderComponent from '../../../common/baseComponents/loader';
import { AppDashBoardRoutes } from '../../../routes/routesConfig';
import { withRouter } from 'react-router-dom';
import { mergeobjects, arrayMap, toPairs, fromPairs, getlocalizeData,isEmpty } from '../../../utils/commonUtils';
import objectUtil from '../../../utils/objectUtil';
import TSRoleDashboard from '../tsRoleDashboard';
import CustomerRoleDashboard from '../customerRoleDashboard';

const localConstant = getlocalizeData();

class Dashboard extends Component {

    constructor(props) {
        super(props);
        this.upadatedHandleChnage = {
            allCoOrdinator: false,
            futureDays: 7,
            currentActiveTab: '',
            contractStatus: "0",
            showMyAssignmentsOnly: false
        };
        this.allCoOrdinatorLabel='';
    }

    componentDidMount() {
        if(isEmpty(this.props.dashboardCount)) //added For home dashboard my task and my search count not refreshing
           this.props.actions.FetchDashboardCount(); 
        if(this.props.currentPage !== localConstant.sideMenu.DASHBOARD){
            this.props.actions.UpdateCurrentPage(localConstant.sideMenu.DASHBOARD);
        }
    }

    handleAllCoOrdinator = (e) => {
        this.upadatedHandleChnage["allCoOrdinator"] = e.target.checked;
        this.upadatedHandleChnage["currentActiveTab"] = this.props.location.pathname;
        this.props.actions.ToggleAllCoordinator(this.upadatedHandleChnage);
        //dispatch current selected tab action creator
        this.dashBoradRefresh();
     
    }
    validate = (e) => {
        if (e.charCode === 46 || e.charCode === 45) {
            e.preventDefault();
        }
    }
    handleFutureDays = (e) => {
        this.upadatedHandleChnage["futureDays"] = e.target.value;
        this.upadatedHandleChnage["allCoOrdinator"] =this.props.isAllCoordinator;
        this.upadatedHandleChnage["currentActiveTab"] = this.props.location.pathname;
        this.props.actions.ToggleAllCoordinator(this.upadatedHandleChnage);
        //Refresh Dashboard Data 
        this.props.actions.Dashboardrefresh(this.props.location.pathname);
        //Refresh dashboard Count
        this.props.actions.FetchDashboardCount();
    }
    dashBoradRefresh = () => {       
        this.props.actions.ToggleAllCoordinator(this.upadatedHandleChnage);
        //Refresh Dashboard Data 
        this.props.actions.Dashboardrefresh(this.props.location.pathname);
        //Refresh dashboard Count
        this.props.actions.FetchDashboardCount();
    }
    handleBudgetContractStatusChange = (e) => {
        this.upadatedHandleChnage[e.target.name] = e.target.value;
        this.props.actions.BudgetPropertyChange(this.upadatedHandleChnage);
        if (this.props.location.pathname === AppDashBoardRoutes.budgetMonetary) //D1133 fixes
            this.props.actions.FetchBudget(localConstant.Dashboard.BUDGETMONETARY);
        else
            this.props.actions.FetchBudget(localConstant.Dashboard.BUDGETHOUR);
    }
    handleBudgetShowAssignemtsChange = (e) => {
        if (e.target.checked) {
            this.upadatedHandleChnage[e.target.name] = true;
        }
        this.props.actions.BudgetPropertyChange(this.upadatedHandleChnage);
        if (this.props.location.pathname === AppDashBoardRoutes.budgetMonetary) //D773 fixes
            this.props.actions.FetchBudget(localConstant.Dashboard.BUDGETMONETARY);
        else
            this.props.actions.FetchBudget(localConstant.Dashboard.BUDGETHOUR);

    }
    handleProjectChange = (e) => {
        if (e.target.checked) {
            this.upadatedHandleChnage[e.target.name] = false;
        }
        this.props.actions.BudgetPropertyChange(this.upadatedHandleChnage);
        if (this.props.location.pathname === AppDashBoardRoutes.budgetMonetary) //D773 fixes
            this.props.actions.FetchBudget(localConstant.Dashboard.BUDGETMONETARY);
        else
            this.props.actions.FetchBudget(localConstant.Dashboard.BUDGETHOUR);

    }
    handleAllUsersTask = (e) => {        
        this.upadatedHandleChnage["myTaskStatus"] = e.target.checked;        
        const response = this.props.actions.MyTaskPropertyChange(this.upadatedHandleChnage);        
        if(response) {            
            this.props.actions.FetchTechSpecMytaskData(e.target.checked);
        }
    }
     /*
    handleAllCoOrdinatorSearch funtion added For home dashboard my task and my search count not refreshing
    */
    handleAllCoOrdinatorSearch =(e)=>{
        this.upadatedHandleChnage["mySearchStatus"] = e.target.checked; 
        const response = this.props.actions.MySearchPropertyChange(this.upadatedHandleChnage); 
        if(response) {            
            this.props.actions.FetchMySearchData(e.target.checked);
        }
    }
    render() {
        let count=this.props.dashboardCount;
        if(isEmpty(this.props.dashboardCount)){
            count={
                    AssignmentCount: 0,
                    InactiveAssignmentCount: 0,
                    VisitCount: 0,
                    TimesheetCount: 0,
                    ContractCount: 0,
                    DocumentApprovalCount:0,
                    MyTaskCount:0,
                    MySearchCount:0,
            };
        }
        if(this.props.location.pathname) {
            if(this.props.location.pathname === AppDashBoardRoutes.visitStatus)
                this.allCoOrdinatorLabel = localConstant.commonConstants.ALL_COORDINATORS_VISITS;
            else if(this.props.location.pathname === AppDashBoardRoutes.timesheet)
                this.allCoOrdinatorLabel = localConstant.commonConstants.ALL_COORDINATORS_TIMESHEETS;
            else 
                this.allCoOrdinatorLabel = localConstant.commonConstants.ALL_COORDINATORS_ASSIGNMENTS;
        }
        return (
            <Fragment> 
                { (this.props.userTypes !== 'TechnicalSpecialist' && this.props.userTypes !== 'Customer') && <div className='row mb-0'>
                    
                    <Menu userType={this.props.loginUserType} menulist={dashboardMenu} menuClass="horizontalMenu scrollableTabs" isScrollable={true} name='tabHeader' url='tabBodyComponent' count={count} countkey="tabCount"></Menu> 
                    
                    <div className='customCard'>
                        <DashboardRoutes />
                    </div>
                    <div className={'dashboard customCard row'}>
                    <div className={
//(this.props.location.pathname === AppDashBoardRoutes.documentAproval) ||
                            (this.props.location.pathname === AppDashBoardRoutes.budgetMonetary) ||
                            (this.props.location.pathname === AppDashBoardRoutes.BudgetHours) ||
                            (this.props.location.pathname === AppDashBoardRoutes.contractsNearExpiry) ||
                            (this.props.location.pathname === AppDashBoardRoutes.mytasks) ||
                            (this.props.location.pathname === AppDashBoardRoutes.mysearch)?
                            'hide' :
                            'show pt-2 pl-0 col s12 m3'}>
                        <label>
                            <input className='filled-in' type="checkbox" onClick={this.handleAllCoOrdinator} checked={this.props.isAllCoordinator?true:false} />                      
                            <span>{ this.allCoOrdinatorLabel }</span>
                        </label>
                    </div>
                    {/* <div className={
                        (this.props.location.pathname === AppDashBoardRoutes.ContractsNearExpiry)  ?
                            'show pt-2 pl-0 col s12 m3 ' :
                            'hide'}>
                       
                        <label className='pl-0'>
                        <input className='filled-in' type="checkbox" onClick={""} defaultChecked={""} />
                            <span> My Assignments</span>
                        </label>
                    </div>  */}
                    {/* <div className="pt-2 pl-0 col s12 m3 ">
                    </div>
                    <div className={
                        (this.props.location.pathname === AppDashBoardRoutes.ContractsNearExpiry)  ?
                            'col s12 m2 show center-align pt-2' : 'hide'}>
                        <label className="bold">Future Days to Display</label>
                    </div>
                    <div className={
                        (this.props.location.pathname === AppDashBoardRoutes.ContractsNearExpiry) ?
                            'col s12 m1 show' : 'hide'}>
                        <input placeholder="Future Days to Display" onChange={this.handleFutureDays} onKeyPress={this.validate} type="number" className="center-align browser-default customInputs validate" defaultValue='7' />
                    </div> */}

                    <div className={
                            (this.props.location.pathname === AppDashBoardRoutes.mysearch) ?
                            'show pt-2 pl-0 col s12 m3' :
                            'hide'}>
                        <label>
                            <input className='filled-in' type="checkbox" onClick={this.handleAllCoOrdinatorSearch} checked={this.props.mySearchStatus?true:false}/>
                            <span>{ localConstant.commonConstants.ALL_USERS_SEARCHES }</span>
                        </label>
                    </div>
                    <div className={
                            (this.props.location.pathname === AppDashBoardRoutes.mytasks) ?
                            'show pt-2 pl-0 col s12 m3' :
                            'hide'}>
                        <label>
                            <input className='filled-in' type="checkbox" onClick={this.handleAllUsersTask} checked={this.props.myTaskStatus?true:false}/>
                            <span>{ localConstant.commonConstants.All_USERS_TASK}</span>
                        </label>
                    </div>
                    <div className={
                        (this.props.location.pathname === AppDashBoardRoutes.budgetMonetary) || (this.props.location.pathname === AppDashBoardRoutes.BudgetHours) ?
                            'show col s12 m2 mt-2 bold right-align' :
                            'hide'}>
                        <label>Budget Contract Status :</label>
                    </div>
                    <div className={
                        (this.props.location.pathname === AppDashBoardRoutes.budgetMonetary) || (this.props.location.pathname === AppDashBoardRoutes.BudgetHours) ?
                            'show col s12 m1 pl-0' :
                            'hide'}>
                        <select className='browser-default' name="contractStatus" onChange={this.handleBudgetContractStatusChange}>
                            {/* <option value="" disabled defaultValue>Budget Contract Status</option> */}
                            <option value="0">All</option>
                            <option value="1" selected={true}>Open</option>
                            <option value="2">Closed</option>
                        </select>
                    </div>
                   {/* <div className={
                        (this.props.location.pathname === AppDashBoardRoutes.budgetMonetary) || (this.props.location.pathname === AppDashBoardRoutes.BudgetHours) ?
                            'col s12 m2 show center-align pt-2' : 'hide'}>
                        <label className="bold">Future Days to Display</label>
                    </div>
                    <div className={
                        (this.props.location.pathname === AppDashBoardRoutes.budgetMonetary) || (this.props.location.pathname === AppDashBoardRoutes.BudgetHours) ?
                            'col s12 m1 show pr-3' : 'hide'}>
                        <input placeholder="Future Days to Display" onBlur={this.handleFutureDays} onKeyPress={this.validate} type="number" className="center-align browser-default customInputs validate" defaultValue='7' />
                    </div>*/}
                    <div className={
                        (this.props.location.pathname === AppDashBoardRoutes.budgetMonetary) || (this.props.location.pathname === AppDashBoardRoutes.BudgetHours) ?
                            'show col s12 m5 mt-2 pr-3' :
                            'hide'}>
                        <label className='pr-3'>
                            <input className="with-gap" name="showMyAssignmentsOnly" value="allProjects" type="radio" defaultChecked={true} onChange={this.handleProjectChange} />
                            <span>Show Data for My Projects</span>
                        </label>
                        <label>
                            <input className="with-gap" name="showMyAssignmentsOnly" value="showMyAssignmentsOnly" type="radio" onChange={this.handleBudgetShowAssignemtsChange} />
                            <span>Show Data for My Assignments</span>
                        </label>
                    </div>
                    
                    <div className={
                        (this.props.location.pathname === AppDashBoardRoutes.visitStatus) || (this.props.location.pathname === AppDashBoardRoutes.timesheet) ?
                            'col s12 m2 show right-align pt-1' : 'hide'}>
                        <label className="bold">Future Days to Display</label>
                    </div>
                    <div className={
                        (this.props.location.pathname === AppDashBoardRoutes.visitStatus) || (this.props.location.pathname === AppDashBoardRoutes.timesheet) ?
                            'col s12 m1 show' : 'hide'}>
                        <input placeholder="Future Days to Display" onBlur={this.handleFutureDays} onKeyPress={this.validate} type="number" className="center-align browser-default customInputs validate" defaultValue='7' />
                    </div>
                    <a onClick={this.dashBoradRefresh} className="right mt-1 mr-1 btn-small bold"><i className="zmdi zmdi-refresh zmdi-hc-lg"></i></a>
                    <span className={
                            (this.props.location.pathname === AppDashBoardRoutes.budgetMonetary) ||
                            (this.props.location.pathname === AppDashBoardRoutes.BudgetHours) ||
                            (this.props.location.pathname === AppDashBoardRoutes.ContractsNearExpiry) ||
                            (this.props.location.pathname === AppDashBoardRoutes.mysearch) ||
                            (this.props.location.pathname === AppDashBoardRoutes.mytasks) ?
                            'hide' :
                            'right dashboardInfo p-relative'}
                        >
                        <div className="triangleTopLeft"></div>
                        &nbsp;&nbsp;No Operating Coordinator or Resource has been assigned
                    </span>
                    </div>
                    {/* <div className={'dashboard customCard row'}>
                        <div>
                            <div className="col s12 m2"><div className="col s1 rectangle" style={{ background: "#C00000" }}></div><div className="col s9"><label className="bold">Booked</label></div></div>
                            <div className="col s12 m2"><div className="col s1 rectangle" style={{ background: "#FFC700" }}></div><div className="col s9"><label className="bold">Tentative</label></div></div>
                            <div className="col s12 m2"><div className="col s1 rectangle" style={{ background: "#A3A6A9" }}></div><div className="col s9"><label className="bold">TBA</label></div></div>
                            <div className="col s12 m2"><div className="col s1 rectangle" style={{ background: "#21B6D7" }}></div><div className="col s9"><label className="bold">PTO</label></div></div>
                            <div className="col s12 m2"><div className="col s1 rectangle" style={{ background: "#474E54" }}></div><div className="col s9"><label className="bold">Pre-Assignment</label></div></div>
                        </div>
                    </div> */}
                </div>
                }

                { this.props.userTypes === 'TechnicalSpecialist' && <TSRoleDashboard/> }
                { this.props.userTypes === 'Customer' && <CustomerRoleDashboard/> }

            </Fragment>

        );
    }
}

export default withRouter(Dashboard);
