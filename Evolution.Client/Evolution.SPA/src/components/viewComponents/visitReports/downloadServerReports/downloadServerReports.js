import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { HeaderData } from './headerData';
import CustomModal from '../../../../common/baseComponents/customModal';
import Panel from '../../../../common/baseComponents/panel';
import { getlocalizeData } from '../../../../utils/commonUtils';
import { tr } from 'react-dom-factories';

const localConstant = getlocalizeData();

class DownloadServerReports extends Component {
    constructor(props) {
       
        super(props);
        this.state = {
            isPanelOpen: true,
            selectedIds:[],
            label: ''
        };
    }

  /**to do ComponentDidMount */
    componentDidMount() {
       this.props.actions.ClearServerReports();
       this.ReportRefresh();
    }

    DeleteFiles = () => {
        const selectedReportIds = this.state.selectedIds;
        const selectedID = [];
        let _reportType = 0;
        if (this.props.currentPage === localConstant.supplier.SUPPLIER_DOWNLOADED_REPORT_MODE) {
            _reportType = 1;
        }
        else if (this.props.currentPage === localConstant.visit.VISIT_DOWNLOADED_REPORT_MODE) {
            _reportType = 2;
        }
        else if (this.props.currentPage === localConstant.timesheet.TIMESHEET_DOWNLOADED_REPORT_MODE) {
            _reportType = 3;
        }
        else if (this.props.currentPage === localConstant.assignments.ASSIGNMENT_DOWNLOADED_REPORT_MODE) {
            _reportType = 4;
        }

        for(let i=0; i< selectedReportIds.length; i++){
            const item = selectedReportIds[i];
            selectedID.push(item.id);
        }
        const params = {
            deleteParams: selectedID,
            loggedInUser: this.props.loggedInUser,
            reportType: _reportType
        }; 
        this.props.actions.DeleteReportFiles(params);
        this.setState({ selectedIds: [] });
    }

    ReportsOnSelectHandler = (e) => {
        const _state = this.state;
        if (e.node.selected) {
            const item = e.node.data;
            if (item && item.processStatus != 1) {
                const arrayObj = _state.selectedIds;
                arrayObj.push(e.node.data);
                this.setState({ selectedIds: arrayObj });
            }
        } else {
            const arrayObj = _state.selectedIds;
            const index = arrayObj.indexOf(e.node.data);
            if(index > -1){
                arrayObj.splice(index, 1);
                this.setState({ selectedIds: arrayObj });
            }
        }
    }

    ReportRefresh = () => {     
        const data = {};
        this.props.actions.ClearServerReports();
        data.loggedInUser = this.props.loggedInUser;
        if (this.props.currentPage === localConstant.supplier.SUPPLIER_DOWNLOADED_REPORT_MODE) {
            data.reportType = 1;
            this.props.actions.FetchServerReportData(data);
            this.setState({ label: 'Supplier Reports' });
        }
        else if (this.props.currentPage === localConstant.visit.VISIT_DOWNLOADED_REPORT_MODE) {
            data.reportType = 2;
            this.props.actions.FetchServerReportData(data);
            this.setState({ label: 'Visit Reports' });
        }
        else if (this.props.currentPage === localConstant.timesheet.TIMESHEET_DOWNLOADED_REPORT_MODE) {
            data.reportType = 3;
            this.props.actions.FetchServerReportData(data);
            this.setState({ label: 'Timesheet Reports' });
        }
        else if (this.props.currentPage === localConstant.assignments.ASSIGNMENT_DOWNLOADED_REPORT_MODE) {
            data.reportType = 4;
            this.props.actions.FetchServerReportData(data);  
            this.setState({ label: 'Assignment Reports' });
        }
        else if (this.props.currentPage === localConstant.techSpec.common.DOWNLOADED_REPORTS_TO_VIEW) {
            data.reportType = 5;
            this.props.actions.FetchServerReportData(data);  
            this.setState({ label: 'Resource Reports' });
        }

    }

    render() {
     const {  serverReportData } = this.props;
     const { isDeleteEnabled } = this.state.selectedIds.length > 0 ? false : true;
        return (
            <Fragment>
                <CustomModal/>
                <div className="companyPageContainer customCard">
                    <h6 className="bold ml-2">{this.state.label}</h6>
                    <div className="customCard">
                        <ReactGrid
                            gridRowData={serverReportData}
                            gridColData={HeaderData} 
                            rowSelected={this.ReportsOnSelectHandler}
                        />
                    </div>
                    <div className="right-align mt-2 col s12 add-text">
                        {
                            this.state.selectedIds.length > 0?
                            <a type="button" onClick={this.DeleteFiles} disabled={false} className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn">Delete</a>
                            :<a type="button" onClick={this.DeleteFiles} disabled={true} className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn">Delete</a>
                        }
                        <a onClick={this.ReportRefresh} className="btn-small btn-primary ml-2 waves-effect"><i className="zmdi zmdi-refresh zmdi-hc-lg"></i></a>
                    </div>
                </div>
               
            </Fragment>
        );
    };
};

export default DownloadServerReports;