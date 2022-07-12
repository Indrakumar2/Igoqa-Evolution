import React, { Component, Fragment } from 'react';
import { connect } from "react-redux";
import { DownloadDocumentData } from './commonAction';
import { configuration } from '../appConfig';
import IntertekToaster from '../common/baseComponents/intertekToaster';
import { getlocalizeData,uploadedDocumentCheck } from '../utils/commonUtils';
const localConstant = getlocalizeData();
class FileToBeOpen extends Component {
    clickHandler = (e) => {
        if (this.props.data) {
            const fileData = [];
            fileData.push(this.props.data);
            const isFileUploaded = uploadedDocumentCheck(fileData); //To check whether the request doc is uploaded or not
            if (isFileUploaded) {
                const allowDonloadFileFormat = configuration.allowedFileFormats.indexOf(this.props.data.documentName.substring(this.props.data.documentName.lastIndexOf(".")).toLowerCase());
                if (allowDonloadFileFormat > -1) {
                    this.props.DownloadDocumentData(this.props.data);
                } else {
                    IntertekToaster(localConstant.commonConstants.NO_LONGER_SUPPORT, 'warningToast CustDocDelDocChk');
                }
            }
        }
    }
    render() {      
        const linkToBeDisabled = (this.props.disableField && 
                                  this.props.disableField() === true) ?"isDisabled":""; 
        const documentName=this.props.documentName; 
        return (
            <Fragment>
                {documentName?
                <a onClick={this.clickHandler} className={"textNoWrapEllipsis link "+linkToBeDisabled} disabled={linkToBeDisabled==='isDisabled'?true:false}>{this.props.data[this.props.dataToRender]}</a>:
                <a onClick={this.clickHandler} className={"textNoWrapEllipsis link " + linkToBeDisabled} disabled={linkToBeDisabled==='isDisabled'?true:false}><i class="zmdi zmdi-download zmdi-hc-lg"></i></a>
                }
            </Fragment>
        );
    }
}

export default connect(null, {
    DownloadDocumentData
}) (FileToBeOpen);