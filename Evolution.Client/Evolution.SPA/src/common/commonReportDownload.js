import React, { Component, Fragment } from 'react';
import { connect } from "react-redux";
import { modalMessageConstant, modalTitleConstant } from '../constants/modalConstants';
import { DisplayModal, HideModal } from '../common/baseComponents/customModal/customModalAction';
import { DownloadServerFile } from '../actions/reports/visitReportsAction';
import { isEmpty } from '../utils/commonUtils';

class ReportToDownload extends Component {
    constructor(props) {
        super(props);
        this.updatedData = {};
    }
    clickHandler = (e) => {
        if (this.props.data) {
            if (this.props.data.fullPath && !isEmpty(this.props.data.fullPath)) {
                this.updatedData.filename = this.props.data.fullPath;
                this.updatedData.isDelete = false;
                this.props.DownloadServerFile(this.updatedData);
            }
        }
    }
    deleteDocument = () => {
        this.updatedData.isDelete = true;
        this.props.DownloadServerFile(this.updatedData);
        this.props.HideModal();
    }
    downloadDocument = () => {
        this.updatedData.isDelete = false;
        this.props.DownloadServerFile(this.updatedData);
        this.props.HideModal();
    }
    render() {
        const linkToBeDisabled = (this.props.data.isDisabled === true) ? "isDisabled" : "";
        const documentName = this.props.data.FullPath;
        return (
            <Fragment>
                {linkToBeDisabled ?
                    <div></div> :
                    <a onClick={this.clickHandler} className={"textNoWrapEllipsis link " + linkToBeDisabled} disabled={false}><i class="zmdi zmdi-download zmdi-hc-lg"></i></a>
                }
            </Fragment>
        );
    }
}

export default connect(null, {
    DisplayModal,
    HideModal,
    DownloadServerFile,
})(ReportToDownload);