import React, { Fragment } from 'react';
import ReactGrid from '../../common/baseComponents/reactAgGrid';
import { getlocalizeData } from '../../utils/commonUtils';
const localConstant = getlocalizeData();
const RelatedDocuments = (props) => {
    return (
        <Fragment>
            <div className="row mb-0 pt-2 ml-0" >
              {props.pageMode!==localConstant.commonConstants.VIEW && <div className="center-align s3 offset-s9 right mr-3" >
                    <a onClick={props.DownloadDocumentHandler} className="mr-3 waves-effect d-inline modal-trigger waves-effect waves-light" disabled={props.interactionMode ? true : false} >
                        <i className="zmdi zmdi-download zmdi-hc-lg"></i>
                        <span className="d-block">{localConstant.documents.DOWNLOAD}</span>
                    </a>
                    <a onClick={props.copyRecord} className="mr-3 d-inline waves-effect waves-light doccopy" disabled={props.interactionMode ? true : false} >
                        <i className="zmdi zmdi-copy zmdi-hc-lg"></i>
                        <span className="d-block">{localConstant.documents.COPY}</span>
                    </a>
                </div>}
                <div className="customCard mt-0">
                    <ReactGrid 
                        gridRowData={props.DocumentsData} 
                        gridColData={props.headerData} 
                        onRef={props.onRef}
                        paginationPrefixId={props.paginationPrefixId} />
                </div>
            </div>
         
        </Fragment>
    );
};
export default RelatedDocuments;