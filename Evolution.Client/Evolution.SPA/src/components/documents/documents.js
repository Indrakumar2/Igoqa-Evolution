import React, { Fragment } from 'react';
import ReactGrid from '../../common/baseComponents/reactAgGrid';
import CardPanel from '../../common/baseComponents/cardPanel';
import { getlocalizeData } from '../../utils/commonUtils';
import LoaderComponent from '../../common/baseComponents/loader';
const localConstant = getlocalizeData();
const commonDocuments = (props) => {
     return (
        <Fragment>
            { ((props.interactionMode || props.viewMode) && props.pageMode!==localConstant.commonConstants.VIEW) && <div className="row mb-0 pt-2" >
                <div className="center-align s3 offset-s9 right mr-3" >
                <a onClick={props.DownloadDocumentHandler} className="mr-3 waves-effect d-inline modal-trigger waves-effect waves-light">
                        <i className="zmdi zmdi-download zmdi-hc-lg"></i>
                        <span className="d-block">{localConstant.documents.DOWNLOAD}</span>
                    </a>
                    { !props.viewMode &&            //For D-456
                    <a onClick={props.uploadDocumentHandler} className="mr-3 waves-effect d-inline modal-trigger waves-effect waves-light">
                        <i className="zmdi zmdi-upload zmdi-hc-lg"></i>
                        <span className="d-block">{localConstant.documents.UPLOAD}</span>
                    </a>
                    }
                    <a onClick={props.copyDocumentHandler} className="mr-3 d-inline waves-effect waves-light">
                        <i className="zmdi zmdi-copy zmdi-hc-lg"></i>
                        <span className="d-block">{localConstant.documents.COPY}</span>
                    </a>
                    { !props.viewMode &&      //For D-456
                    <a onClick={props.pasteDocumentHandler} className="mr-3 d-inline waves-effect waves-light">
                        <i className="zmdi zmdi-paste zmdi-hc-lg"></i>
                        <span className="d-block">{localConstant.documents.PASTE}</span>
                    </a>
                    }
                    { !props.viewMode && (props.isDeleteDocument === false ? false : true) &&  //For D-456
                    <a onClick={props.deleteDocumentHandler} className="mr-3 d-inline waves-effect waves-light modal-trigger">
                        <i className="zmdi zmdi-delete zmdi-hc-lg danger-txt"></i>
                        <span className="d-block danger-txt">{localConstant.documents.DELETE}</span>
                    </a>
                    }
                </div>
            </div>}

            {props.interactionMode && props.pageMode!==localConstant.commonConstants.VIEW && <div id="drop-area">
                <form className="my-form">
                    <i className="zmdi zmdi-download"></i>
                    <p>{localConstant.commonConstants.DRAG_AND_DROP }</p>
                    <input type="file" id="fileElem" multiple accept={props.supportFileType.allowedFileFormats} onChange={()=>this.fileupload(this.files)}></input>
                    <label className="button" for="fileElem">Select some files</label>
                </form>               
            </div>}

            <CardPanel className="white lighten-4 black-text" colSize="s12">
                <ReactGrid gridRowData={props.rowData} gridColData={props.headerData} 
                interactionMode={props.interactionMode} onCellchange={props.onCellchange}
                onRef={props.onRef} roleBase={props.interactionMode && props.pageMode!==localConstant.commonConstants.VIEW}
                rowClassRules={props.rowClass ? props.rowClass : false }
                draftData={props.draftData ?  props.draftData : null }
                paginationPrefixId={props.paginationPrefixId}
                />
            </CardPanel>
        </Fragment>
    );
};

export default commonDocuments;