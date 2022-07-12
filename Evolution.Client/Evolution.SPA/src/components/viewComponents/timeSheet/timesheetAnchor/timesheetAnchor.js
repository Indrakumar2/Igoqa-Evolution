import React,{ Component } from 'react';
import { Link } from 'react-router-dom';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import { getlocalizeData,ObjectIntoQuerySting } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();

class TimesheetAnchor extends Component{

    selectedRowHandler = (e,redirectionURL,queryStr) => {
        // const data ={ timesheetNumber: this.props.data.timesheetNumber,
        //              timesheetId:this.props.data.timesheetId }; 
        // this.props.actions.HandleMenuAction({
        //     currentPage: localConstant.timesheet.EDIT_VIEW_TIMESHEET_MODE,
        //     currentModule: localConstant.moduleName.TIMESHEET
        // });
        this.props.actions.GetSelectedTimesheet(this.props.data);
        //this.props.actions.FetchTimesheetGeneralDetail(this.props.timesheetId);
        // this.props.actions.FetchTimesheetDetail(this.props.data.timesheetId);
        this.props.actions.SetCurrentPageMode();
        this.openTimesheet(redirectionURL,queryStr);
    }

    openTimesheet = (redirectionURL,queryStr) => {
        window.open(redirectionURL + '?'+queryStr,'_blank');
    };

    render(){     
        const redirectionURL = AppMainRoutes.timesheetDetails;
        const linkText = this.props.displayLinkText?this.props.displayLinkText:this.props.data.timesheetNumber;

        const queryObj = {
            tId: this.props.data.timesheetId,
            tNo: this.props.data.timesheetNumber,
            tProNo: this.props.data.timesheetProjectNumber,
            tAssId:this.props.data.timesheetAssignmentId,
            tAssNo:this.props.data.timesheetAssignmentNumber,
        };
        const queryStr = ObjectIntoQuerySting(queryObj);

        return(
            //  <Link target='_blank' to={{ pathname:redirectionURL, search: `?tId=${ this.props.data.timesheetId }&tNo=${ this.props.data.timesheetNumber }&tProNo=${ this.props.data.timesheetProjectNumber }&tAssId=${ this.props.data.timesheetAssignmentId }&tAssNo=${ this.props.data.timesheetAssignmentNumber }` }} onClick={this.selectedRowHandler}  className="link">{linkText}</Link>
            <a href='javascript:void(0)' onClick={(e) => this.selectedRowHandler(e, redirectionURL, queryStr)} className="link" >{linkText}</a>
        );
    }
}

export default TimesheetAnchor;