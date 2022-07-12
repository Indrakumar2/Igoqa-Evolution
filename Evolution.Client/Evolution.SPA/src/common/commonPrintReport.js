import React, { Component, Fragment } from 'react';
import { connect } from "react-redux";
import { modalMessageConstant, modalTitleConstant } from '../constants/modalConstants';
import { DisplayModal, HideModal } from '../common/baseComponents/customModal/customModalAction';
import { DownloadServerFile,PrintSSRSReport } from  '../actions/reports/visitReportsAction';
import { EvolutionReportsAPIConfig } from '../apiConfig/apiConfig';
import { isEmpty } from '../utils/commonUtils';

class PrintReport extends Component {
    constructor(props) {
        super(props);
        this.updatedData = {};
    }
    clickHandler = (e) => {
        // console.log(this.props.data);
        const data = this.props.data;
        if (data && data.assignmentId && data.epin) {
            const params = {};
            params["AssignmentId"] = data.assignmentId;
            params["SpecialistId"] = data.epin;
            params['reportname'] = EvolutionReportsAPIConfig.AssignmentTechSpecInstructionReport;
            params['format'] = 'PDF';
            params['reptype'] = 4;
            const fileName = EvolutionReportsAPIConfig.AssignmentTechSpecInstructionReportFileName + '.pdf';
            this.props.PrintSSRSReport(params,'application/pdf',fileName);
        }
    }

    render() {      
        return (
            <Fragment>
                <a onClick={this.clickHandler} className={"textNoWrapEllipsis link"}><i title="Assignment Resource Instruction Report" class="zmdi zmdi-print zmdi-hc-lg"></i></a>
            </Fragment>
        );
    }
}

export default connect(null, {
    DisplayModal,
    HideModal,
    DownloadServerFile,
    PrintSSRSReport
}) (PrintReport);