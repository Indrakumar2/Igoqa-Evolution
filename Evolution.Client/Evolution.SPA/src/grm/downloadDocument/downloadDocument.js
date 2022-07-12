import React, { Component, Fragment } from 'react';
import { connect } from "react-redux";
import { DownloadDocumentData } from '../../common/commonAction';
import {  isEmpty, getlocalizeData } from '../../utils/commonUtils'; 
import { configuration } from '../../appConfig';
import { lastIndexOf } from 'lodash';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
const localConstant = getlocalizeData();

class FileToBeDownload extends Component {
    clickHandler = (e) => {  // def 205 #7 
        let documentToDownload=null;
        if(this.props.colDef.objectAttributeName === 'verificationDocuments'){
            documentToDownload=this.props.data[this.props.colDef.objectAttributeName][0];
        }else if (this.props.data.documents) {
            documentToDownload=this.props.data.documents[0];
        } 
        const allowDonloadFileFormat = configuration.allowedFileFormats.indexOf(documentToDownload.documentName.substring(documentToDownload.documentName.lastIndexOf(".")).toLowerCase());
        if(allowDonloadFileFormat > -1){
            this.props.DownloadDocumentData(documentToDownload);
        } else{
            IntertekToaster(localConstant.commonConstants.NO_LONGER_SUPPORT, 'warningToast CustDocDelDocChk'); 
        }
    }
    render() {     
        let data = "";
        if(this.props.colDef.objectAttributeName === 'verificationDocuments'){
            if(this.props.data[this.props.colDef.objectAttributeName] !== undefined && !isEmpty(this.props.data[this.props.colDef.objectAttributeName])){
                if(this.props.data.verificationStatus !== "Verified"){ //D573 -- added
                    data =  "";
                }
                else if (this.props.data[this.props.colDef.objectAttributeName][0].recordStatus !== 'D') {
                    data =  this.props.data[this.props.colDef.objectAttributeName][0].documentName;
                }
                else {
                    data =  "";
                }
            }            
        } else if (this.props.data.documents !== undefined && !isEmpty(this.props.data.documents)) { 
            if (Array.isArray(this.props.data.documents)) {
                const filterdData = this.props.data.documents.filter(x => x.recordStatus !== 'D');
                if (filterdData.length > 0) {
                    data = filterdData[0].documentName;
                } else {
                    data = "";
                }
            } 
        } 
        return (           
            <Fragment>
                {data?<a onClick={this.clickHandler} className="link"> {data}</a>:null}
            </Fragment>
        );
    }
}

export default connect(null, {
    DownloadDocumentData
})(FileToBeDownload);