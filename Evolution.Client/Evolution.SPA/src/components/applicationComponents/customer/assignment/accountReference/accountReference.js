import React from 'react';
import ReactGrid from '../../../../../common/baseComponents/reactAgGrid';
import { getlocalizeData } from '../../../../../utils/commonUtils';
const localConstant = getlocalizeData();
const accountReference = (props) => {
    return (
        <div className="customCard">
             <ReactGrid gridRowData={props.accountReference} gridColData={props.headerData.CustomerAccountRefrencesHeader} onRef={props.onRef} paginationPrefixId={localConstant.paginationPrefixIds.accountReference}/>
             {props.pageMode!==localConstant.commonConstants.VIEW && <div className="right-align mt-2">
                <button onClick={props.showAccountReference} className="btn-small waves-effect modal-trigger">{localConstant.companyDetails.common.ADD}</button>
                <button onClick={props.deleteAccountRef} className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn">{localConstant.companyDetails.common.DELETE}</button>
            </div>}
        </div>
    );
};

export default accountReference;