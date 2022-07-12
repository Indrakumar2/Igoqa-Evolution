import React,{ Component,Fragment } from 'react';
import LoaderComponent from '../loader';
import { NavLink,withRouter } from 'react-router-dom';
class TabHeader extends Component{    
    constructor(props) {
        super(props);
        this.state = {
            dynamicComponents : []       
        };
    }
    
async componentDidMount(){

        // var button = document.getElementById('slide');
        // button.onclick = function () {
        //     var container = document.querySelector('.customTabs');
        //     sideScroll(container,'right',25,100,10);
        // };

        // var back = document.getElementById('slideBack');
        // back.onclick = function () {
        //     var container = document.querySelector('.customTabs');
        //     sideScroll(container,'left',25,100,10);
        // };

        // function sideScroll(element,direction,speed,distance,step){
        //     var scrollAmount = 0;
        //     var slideTimer = setInterval(function(){
        //         if(direction == 'left'){
        //             element.scrollLeft -= step;
        //         } else {
        //             element.scrollLeft += step;
        //         }
        //         scrollAmount += step;
        //         if(scrollAmount >= distance){
        //             window.clearInterval(slideTimer);
        //         }
        //     }, speed);
        // }

    }
    render(){     
        return(  
            <div className="tabHeadContainer">
                {/* <div className="tabPrevBtn tabControls">
                    <a id="slideBack" className="link ml-2"><i className="zmdi zmdi-chevron-left zmdi-hc-2x"></i></a>
                
                </div>                       */}
                <ul className="tabs customTabs">                   
                    {/* {
                        (this.props.tabData).map((iteratedTab,i)=>{
                            return <li className="tab col textCapitalize"  key={i}><NavLink activeClassName={'active'} to={iteratedTab.tabBodyComponent}>{iteratedTab.tabHeader}</NavLink></li>
                        })
                    }                     */}
                    <li className="tab col textCapitalize"><NavLink activeClassName={'active'} to="/Dashboard/ActiveAssignments">Active Assignments ({this.props.dashboardCount.AssignmentCount})</NavLink></li>
                    <li className="tab col textCapitalize"><NavLink activeClassName={'active'} to="/Dashboard/InactiveAssignments">Inactive Assignments ({this.props.dashboardCount.InactiveAssignmentCount})</NavLink></li>
                    <li className="tab col textCapitalize"><NavLink activeClassName={'active'} to="/Dashboard/VisitStatusAndApproval">Visit Status and Approval ({this.props.dashboardCount.VisitCount})</NavLink></li>
                    <li className="tab col textCapitalize"><NavLink activeClassName={'active'} to="/Dashboard/TimesheetPendingApproval">Timesheets Pending Approval ({this.props.dashboardCount.TimesheetCount})</NavLink></li>
                    <li className="tab col textCapitalize"><NavLink activeClassName={'active'} to="/Dashboard/ContractsNearExpiry">Contracts Near Expiry ({this.props.dashboardCount.ContractCount})</NavLink></li>
                    <li className="tab col textCapitalize"><NavLink activeClassName={'active'} to="/Dashboard/Documentation">Documentation ({this.props.dashboardCount.ContractCount})</NavLink></li>	
                    {/* <li className="tab col textCapitalize"><NavLink activeClassName={'active'} to="/documentAproval">Document Aproval ({this.props.documentAprovalCount})</NavLink></li> */}
                </ul>
                {/* <div className="tabNextBtn tabControls">
                 <a id="slide" className="link mr-2"><i className="zmdi zmdi-chevron-right zmdi-hc-2x"></i></a>
              
                </div>  */}
            </div>
        );
    }
}

export default TabHeader;
