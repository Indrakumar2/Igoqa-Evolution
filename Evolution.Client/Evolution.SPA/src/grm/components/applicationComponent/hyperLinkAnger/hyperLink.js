import React, { Component, Fragment } from 'react';
import { Link } from 'react-router-dom';
import { getlocalizeData, isEmptyReturnDefault, ObjectIntoQuerySting, isEmptyOrUndefine, isFunction } from '../../../../utils/commonUtils';
import { AppMainRoutes, AppDashBoardRoutes } from '../../../../routes/routesConfig';

const localConstant = getlocalizeData();

class HyperLink extends Component {
    constructor(props){
        super(props);
        this.overrideTaskStatus = isEmptyReturnDefault(localConstant.resourceSearch.overrideTaskStatus);
        this.ploTaskStatus = isEmptyReturnDefault(localConstant.resourceSearch.ploTaskStatus);
    }     
    selectedRowHandler = (e,redirectionURL,queryStr) => {      
    const editDraftTypes=[ 
       'TS_EditProfile','RCRM_EditProfile','TM_EditProfile'
    ];
        if (this.props.data.taskType === 'CreateProfile') {
            this.props.actions.FetchSavedDraftProfile(this.props.data.taskRefCode,localConstant.techSpec.common.CREATE_PROFILE,this.props.data.taskType);
        }
        else if (editDraftTypes.includes(this.props.data.taskType)) {
            this.props.actions.FetchSavedDraftProfile(this.props.data.taskRefCode,localConstant.techSpec.common.EDIT_VIEW_TECHSPEC,this.props.data.taskType);
        }
        else if (this.overrideTaskStatus.includes(this.props.data.taskType) || this.ploTaskStatus.includes(this.props.data.taskType) ) {
            // this.props.actions.ARSSearchPanelStatus(true);
            this.props.actions.GetMyTaskARSSearch(this.props.data);
            return true;
        }
        this.openResource(redirectionURL,queryStr);
    }
    //D363 CR change
    openResource = (redirectionURL,queryStr) => {
        window.open(redirectionURL + '?'+queryStr,'_blank');
    };

    EnableButtonLink = (value) => {
        if(value.data.isDisableMyTaskLink){
            return "isDisabled";
        }
    }
    //D363 CR change

    render() {
        let linkName = "";
        let targetVal=false; 
        const searchType=this.props.searchType;
        const viewMode=this.props.match.path === AppMainRoutes.techSpec|| this.props.match.path === AppDashBoardRoutes.mytasks ? false : true ; //D484
      if(this.props.match.path === AppMainRoutes.quickSearch || (searchType === "ARS"||searchType === "NDT" || searchType==="Search Exception") || this.props.match.path === AppMainRoutes.preAssignment || this.props.match.path === AppMainRoutes.techSpec || this.props.match.path === AppDashBoardRoutes.mytasks )
      {
        targetVal=true;
        linkName = this.props.data.epin ? this.props.data.epin : this.props.data.description; //D363 CR change
      }
    //   else{        
    //     linkName = this.props.data.epin ? this.props.data.epin : this.props.data.myTaskId;
    //   } 

        const redirectionURL = AppMainRoutes.profileDetails;  
        const queryObj={
            epin:this.props.data.epin,
            myTaskId:this.props.data.myTaskId,
            taskRefCode:this.props.data.taskRefCode,
            taskType:this.props.data.taskType,
            viewMode:viewMode,
            isPayRateVisible:false // def 957 
        }; 
        if(isFunction(this.props.getInterCompanyCode))
       { 
           const opCode=this.props.getInterCompanyCode(); 
            if(!isEmptyOrUndefine(opCode))
            {
                queryObj.viewMode=true; //def 957
                //queryObj.isPayRateVisible=false;
                queryObj.interCompanyCode=opCode;
            }
       }
        const linkToBeDisabled = this.EnableButtonLink(this.props);  //D363 CR change
        const queryStr = ObjectIntoQuerySting(queryObj);
        return (
            <Fragment>
            { (this.overrideTaskStatus.includes(this.props.data.taskType) || this.ploTaskStatus.includes(this.props.data.taskType)) ? <a className={linkToBeDisabled} onClick={this.selectedRowHandler}href="javascript:void(0)"> {linkName} </a>
               : 
               <a href='javascript:void(0)' disabled={linkToBeDisabled==='isDisabled'?true:false} onClick={(e) => this.selectedRowHandler(e, redirectionURL, queryStr)} className="link" >{linkName}</a>
            //    <Link  to={
            //     targetVal?   
            //     { pathname:redirectionURL,search:`?${ ObjectIntoQuerySting(queryObj) }`
            //  }:  { pathname:redirectionURL }} target={targetVal?"_blank":'_self'} className={linkToBeDisabled} disabled={linkToBeDisabled==='isDisabled'?true:false}>{linkName}</Link>    //selectedRowHandler Actions are handled in techSpecDetails.js Also
            }
            </Fragment>            
        );
    }
}

export default HyperLink;