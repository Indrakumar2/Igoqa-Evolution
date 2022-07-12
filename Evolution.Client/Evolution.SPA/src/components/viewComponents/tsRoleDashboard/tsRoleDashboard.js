import React, { Component, Fragment } from 'react';
import { withRouter } from 'react-router-dom';
import TSOperationalSystemsManual from '../../../evolutionCommonDocuments/TSOperationalSystemsManual_2016.pdf';
import { isEmptyOrUndefine } from '../../../utils/commonUtils';

const DashboardMsg =(props) =>{
    return(
        <Fragment>
            <h6>{props.title}</h6>
            <div id="divDashboardMsg" className="blogHighlight ql-editor"></div>
        </Fragment>
    );
};

const TsComments =(props)=>
{
return(
    <Fragment>
           
                 <h6 >{props.title}</h6>
                 <div  className="blogHighlight ql-editor">
                 <p> { props.tscomments }</p>
                </div>
    </Fragment>
);
};
const LearningSystem = ()=>{
    return(
        <Fragment>
            <h6>{'My Learning System'}</h6>
            <p>
                Employees :
                <a href="https://intranet.intertek.com/lms.aspx" target="_blank"><span>https://intranet.intertek.com/lms.aspx </span></a> {/** D1199 Issue 1Changes */}
            </p>
            <p>
                Contractors : 
                <a href="https://intertek.csod.com/client/intertek/default2.aspx" target="_blank"><span>https://intertek.csod.com/client/intertek/default2.aspx </span></a>
            </p>

            <p className="small mt-7">Note - These sites are not part of Evolution - any access or functional issues should be reported to the site owner via email to either 
             <a href="mailto:TISCompetency@intertek.com"> TISCompetency@intertek.com</a> or <a href="mailto:TISCompetency@intertek.com">TISCompetency@intertek.com</a></p>
            <p className="small mt-3">
            <i className="zmdi zmdi-notifications"></i><a href={TSOperationalSystemsManual} target="_blank"><span>Resource Userguide</span></a>
            {/* <a href="http://172.22.6.13/Intertek/Evolution/Live/Extranet/images/TSOperationalSystemsManual_2016.pdf" target="_blank"><span>Resource Userguide</span></a> */}
             </p>
             <p className="small mt-3">You will need Adobe Reader to view this document. If you don't have Adobe Reader, you can download it by clicking on this link:   <a href="https://get.adobe.com/reader/" target="_blank">Download the latest version of Adobe Reader (Opens new windows)</a>
            </p>
        </Fragment>
    );
};
class TSRoleDashboard extends Component { 

    componentDidMount(){
        this.props.actions.GetTechSpecDashboardCompanyMessage();
    }

    componentWillMount(){

        this.props.actions.EditMyProfileDetails();
    }

    decodeDashboardMsg(dashboardmsg){
        const divElement = document.getElementById("divDashboardMsg");
        if(divElement && !isEmptyOrUndefine(dashboardmsg)) {            
            divElement.innerHTML = dashboardmsg.replace(/<strong>/g, "<b>").replace(/<\/strong>/g, "</b>");            
        }
    }

    render() {
        this.decodeDashboardMsg(this.props.dashboardmsg);
        return (
            <Fragment>               
               <div className="dashboardOtherUser customCard">
                      <DashboardMsg dashboardmsg={this.props.dashboardmsg} title="Company Message"
                    />
                      <TsComments  title="Resource Comments"  tscomments={this.props.selectedTechSpecInfo.homePageComment} />
                      <LearningSystem/>
                </div>
            </Fragment>

        );
    }
}

export default withRouter(TSRoleDashboard);
